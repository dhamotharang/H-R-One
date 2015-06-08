using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Data.OleDb;
using HROne.SaaS.Entities;
using HROne.DataAccess;
//using perspectivemind.validation;

public partial class eChannel_SubmitMPFContributionFile : HROneWebPage
{
    private const string FUNCTION_CODE = "ECH002";
    protected bool IsAllowEdit;

    private DBManager db = ECompanyMPFFile.db;
    DateTime bankFileCutOffDateTime = new DateTime();
    DateTime currentDateTime = new DateTime();
    DatabaseConnection masterDBConn;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!WebUtils.CheckAccess(Response, Session, FUNCTION_CODE, WebUtils.AccessLevel.Read))
            return;

        if (Application["MasterDBConfig"] != null)
            masterDBConn = ((DatabaseConfig)Application["MasterDBConfig"]).CreateDatabaseConnectionObject();
        else
            Response.Redirect("~/AccessDeny.aspx");

        IsAllowEdit = WebUtils.CheckPermission(Session, FUNCTION_CODE, WebUtils.AccessLevel.ReadWrite);

        currentDateTime = AppUtils.ServerDateTime();
        bankFileCutOffDateTime = currentDateTime.Date;


        string tmpDateTimeString = ESystemParameter.getParameter(masterDBConn, ESystemParameter.PARAM_CODE_BANKFILE_CUTOFF_TIME);

        TimeSpan bankFileCutOffTimeSpan;
        if (TimeSpan.TryParse(tmpDateTimeString, out bankFileCutOffTimeSpan))
            bankFileCutOffDateTime = bankFileCutOffDateTime.Add(bankFileCutOffTimeSpan);

        HROne.Common.WebUtility.WebControlsLocalization(this, this.Controls);

        if (bankFileCutOffDateTime < currentDateTime)
            UploadBankFilePanel.Visible = false;
        else
            UploadBankFilePanel.Visible = IsAllowEdit;

        if (!IsPostBack)
            if (DecryptedRequest["InboxID"] != null)
            {
                int inboxID = 0;
                if (int.TryParse(DecryptedRequest["InboxID"].ToString(), out inboxID))
                    UploadFromInbox(inboxID);
            }
    }

    protected void Page_PreRender(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            //view = loadData(info, db, Repeater);
        }

    }


    protected void Upload_Click(object sender, EventArgs e)
    {

        PageErrors errors = PageErrors.getErrors(db, Page.Master);
        errors.clear();

        if (bankFileCutOffDateTime < currentDateTime)
        {
            errors.addError(string.Format(HROne.Translation.PageErrorMessage.ERROR_SAAS_FILE_SUBMIT_AFTER_CUTOFF, new string[] { bankFileCutOffDateTime.ToString("HH:mm") }));
            return;
        }

        if (BankFileUpload.HasFile)
        {
            //DataTable dataTable = HROne.CSVProcess.CSVReader.parse(CNDImportFile.PostedFile.InputStream);
            string strTmpFolder = HROne.Common.Folder.GetOrCreateSessionTempFolder(Session.SessionID).FullName; ; //Environment.GetFolderPath(Environment.SpecialFolder.InternetCache);
            string strTmpFile = System.IO.Path.Combine(strTmpFolder, currentDateTime.ToString("~yyyyMMddHHmmss_") + BankFileUpload.FileName);
            BankFileUpload.SaveAs(strTmpFile);

            try
            {
                UploadToMasterDB(strTmpFile);
            }
            catch (Exception ex)
            {
                System.IO.File.Delete(strTmpFile);
                errors.addError(ex.Message);
            }
        }
        else
            errors.addError(HROne.Translation.PageErrorMessage.ERROR_INVALID_FILE);

    }

    protected void UploadToMasterDB(string uploadFile)
    {
        // uploadFile will be MOVED to bankfile folder
        if (!IsAllowEdit)
            throw new Exception("Access Deny"); 
        
        string FileType = string.Empty;
        string TrusteeCode = string.Empty;

        {
            HROne.MPFFile.HSBCMPFFile mpfFile = new HROne.MPFFile.HSBCMPFFile(dbConn);
            //if (mpfFile.IsValidFormat(uploadFile, out TrusteeCode))
            //{
            //    //FileType = "AMCND";
            //    mpfFile.LoadFromFile(uploadFile);
            //    HROne.MPFFile.HSBCMPFGatewayFile mpfFile2 = mpfFile.ConvertToMPFGatewayFile(string.Empty);
            //    System.IO.FileInfo fileInfo = mpfFile2.GenerateMPFFile();
            //    System.IO.File.Delete(uploadFile);
            //    fileInfo.MoveTo(uploadFile);
            //    FileType = "AMPFF";
            //}
            //else
            {
                HROne.MPFFile.HSBCMPFGatewayFile mpfFile2 = new HROne.MPFFile.HSBCMPFGatewayFile(dbConn, string.Empty);
                if (mpfFile2.IsValidFormat(uploadFile, out TrusteeCode))
                    FileType = "AMPFF";
            }
        }

        if (string.IsNullOrEmpty(FileType))
            throw new Exception("Invalid MPF file or file format is not supported");

        int exchangeProfileID = 0;
        DBFilter HSBCExchangeProfileFilter = new DBFilter();
        HSBCExchangeProfileFilter.add(new Match("CompanyDBID", (int)Session["CompanyDBID"]));
        HSBCExchangeProfileFilter.add(new Match("HSBCExchangeProfileIsLocked", false));
        ArrayList exchangeProfileList = EHSBCExchangeProfile.db.select(masterDBConn, HSBCExchangeProfileFilter);

        if (exchangeProfileList.Count>0)
            if (exchangeProfileList.Count == 1)
                exchangeProfileID = ((EHSBCExchangeProfile)exchangeProfileList[0]).HSBCExchangeProfileID;
            else
            {
                foreach (EHSBCExchangeProfile profile in exchangeProfileList)
                {
                    if (profile.HSBCExchangeProfileBankCode.Equals(TrusteeCode, StringComparison.CurrentCultureIgnoreCase))
                        exchangeProfileID = profile.HSBCExchangeProfileID;
                }
                if (exchangeProfileID == 0)
                    exchangeProfileID = ((EHSBCExchangeProfile)exchangeProfileList[0]).HSBCExchangeProfileID;
            }

        {
            string UploadBankFilePath = ESystemParameter.getParameter(masterDBConn, ESystemParameter.PARAM_CODE_BANKFILE_UPLOAD_FOLDER);
            if (System.IO.Directory.Exists(UploadBankFilePath))
            {
                System.IO.FileInfo mpfFileInfo = new System.IO.FileInfo(uploadFile);
                string relativePath = System.IO.Path.Combine(System.IO.Path.Combine(System.IO.Path.Combine(Session["CompanyDBID"].ToString(), "MPF"), FileType), mpfFileInfo.Name);
                string fullPath = System.IO.Path.Combine(UploadBankFilePath, relativePath);

                System.IO.Directory.CreateDirectory((new System.IO.FileInfo(fullPath)).Directory.FullName);
                mpfFileInfo.MoveTo(fullPath);

                HROne.SaaS.Entities.ECompanyMPFFile companyMPFFile = new HROne.SaaS.Entities.ECompanyMPFFile();
                companyMPFFile.CompanyDBID = (int)Session["CompanyDBID"];
                companyMPFFile.HSBCExchangeProfileID = exchangeProfileID;
                companyMPFFile.CompanyMPFFileTrusteeCode = TrusteeCode;
                companyMPFFile.CompanyMPFFileFileType = FileType;
                companyMPFFile.CompanyMPFFileDataFileRelativePath = relativePath;
                companyMPFFile.CompanyMPFFileSubmitDateTime = currentDateTime;
                if (!HROne.Lib.Entities.ESystemParameter.getParameter(dbConn, HROne.Lib.Entities.ESystemParameter.PARAM_CODE_ECHANNEL_SIGNATURE_REQUIRED_FOR_MPF_FILE).Equals("Y", StringComparison.CurrentCultureIgnoreCase))
                    companyMPFFile.CompanyMPFFileConfirmDateTime = currentDateTime;
                HROne.SaaS.Entities.ECompanyMPFFile.db.insert(masterDBConn, companyMPFFile);


            }
        }
        //else
        //{
        //    System.IO.File.Delete(uploadFile);
        //}
    }

    protected void UploadFromInbox(int InboxID)
    {
        PageErrors errors = PageErrors.getErrors(db, Page.Master);
        errors.clear();

        if (bankFileCutOffDateTime < currentDateTime)
        {
            errors.addError(string.Format(HROne.Translation.PageErrorMessage.ERROR_SAAS_FILE_SUBMIT_AFTER_CUTOFF, new string[] { bankFileCutOffDateTime.ToString("HH:mm") }));
            return;
        }

        HROne.Lib.Entities.EInbox o = new HROne.Lib.Entities.EInbox();
        o.InboxID = InboxID;
        if (HROne.Lib.Entities.EInbox.db.select(dbConn, o))
        {
            if (!o.UserID.Equals(user.UserID))
                Response.Redirect("~/AccessDeny.aspx");

            System.Collections.Generic.List<string> inboxTypeArray = new System.Collections.Generic.List<string>(o.InboxType.Split(new string[] { "|" }, StringSplitOptions.RemoveEmptyEntries));

            if (inboxTypeArray.Contains(HROne.Lib.Entities.EInbox.INBOX_TYPE_FOR_ECHANNEL) && !inboxTypeArray.Contains(HROne.Lib.Entities.EInbox.INBOX_TYPE_FOR_ECHANNEL_SUBMITTED))
            {
                string uploadFolder = uploadFolder = AppUtils.GetDocumentUploadFolder(dbConn);

                DBFilter attachmentFilter = new DBFilter();
                attachmentFilter.add(new Match("InboxID", o.InboxID));
                ArrayList attachmentList = HROne.Lib.Entities.EInboxAttachment.db.select(dbConn, attachmentFilter);

                HROne.Lib.Entities.EInboxAttachment attachment = (HROne.Lib.Entities.EInboxAttachment)attachmentList[0];

                string documentFilePath = attachment.GetDocumentPhysicalPath(dbConn);
                string transferFilePath = documentFilePath;

                string strTmpFolder = HROne.Common.Folder.GetOrCreateSessionTempFolder(Session.SessionID).FullName; ; //Environment.GetFolderPath(Environment.SpecialFolder.InternetCache);
                string strTmpFile = System.IO.Path.Combine(strTmpFolder, currentDateTime.ToString("~yyyyMMddHHmmss_") + attachment.InboxAttachmentOriginalFileName);

                if (attachment.InboxAttachmentIsCompressed)
                {
                    transferFilePath = attachment.GetExtractedFilePath(dbConn);
                    System.IO.File.Move(transferFilePath, strTmpFile);
                }
                else
                {
                    System.IO.File.Copy(transferFilePath, strTmpFile);
                }
                try
                {
                    UploadToMasterDB(strTmpFile);
                    inboxTypeArray.Add(HROne.Lib.Entities.EInbox.INBOX_TYPE_FOR_ECHANNEL_SUBMITTED);
                    o.InboxType = string.Join("|", inboxTypeArray.ToArray());
                    HROne.Lib.Entities.EInbox.db.update(dbConn, o);
                }
                catch (Exception ex)
                {
                    System.IO.File.Delete(strTmpFile);
                    errors.addError(ex.Message);
                }
            }
            else
            {
                errors.addError("File is already submitted");
            }




        }

    }
}
