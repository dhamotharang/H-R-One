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

public partial class eChannel_SubmitAutopayFile : HROneWebPage
{
    private const string FUNCTION_CODE = "ECH001";

    private DBManager db = ECompanyAutopayFile.db;
    DateTime bankFileCutOffDateTime = new DateTime();
    DateTime currentDateTime = new DateTime();
    DatabaseConnection masterDBConn;

    protected bool IsAllowEdit = false;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!WebUtils.CheckAccess(Response, Session, FUNCTION_CODE, WebUtils.AccessLevel.Read))
            return;

        if (Application["MasterDBConfig"] != null)
            masterDBConn = ((DatabaseConfig)Application["MasterDBConfig"]).CreateDatabaseConnectionObject();
        else
            Response.Redirect("~/AccessDeny.aspx");

        currentDateTime = AppUtils.ServerDateTime();
        bankFileCutOffDateTime = currentDateTime.Date;
        while (HROne.SaaS.Entities.EPublicHoliday.IsHoliday(masterDBConn, bankFileCutOffDateTime) || bankFileCutOffDateTime.DayOfWeek == DayOfWeek.Sunday)
            bankFileCutOffDateTime = bankFileCutOffDateTime.AddDays(1);

        IsAllowEdit = WebUtils.CheckPermission(Session, FUNCTION_CODE, WebUtils.AccessLevel.ReadWrite);

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

        HROne.BankFile.HSBCBankFile bankFile = new HROne.BankFile.HSBCBankFile(dbConn);
        bankFile.LoadBankFileDetail(uploadFile);

        DateTime firstValueDate = bankFile.GetFirstValueDate();
        if (firstValueDate <= bankFileCutOffDateTime)
        {
            throw new Exception("Invalid value date:" + firstValueDate.ToString("dd-MMM"));
        }
        if (firstValueDate.DayOfWeek == DayOfWeek.Saturday || firstValueDate.DayOfWeek == DayOfWeek.Sunday)
        {
            throw new Exception("Invalid value date:" + firstValueDate.ToString("dd-MMM"));
        }
        if (HROne.SaaS.Entities.EPublicHoliday.IsHoliday(masterDBConn, firstValueDate))
        {
            throw new Exception("Invalid value date:" + firstValueDate.ToString("dd-MMM"));
        }
        DBFilter bankPaymentCodeFilter = new DBFilter();
        bankPaymentCodeFilter.add(new Match("CompanyDBID", (int)Session["CompanyDBID"]));
        ArrayList BankPaymentCodeList = EHSBCBankPaymentCode.db.select(masterDBConn, bankPaymentCodeFilter);
        string bankFileBranchAccountNoString = bankFile.BranchCode + bankFile.AccountNo;
        string bankFileInOutFlag = string.Empty;
        if ((bankFile.PlanCode.Equals("E") || bankFile.PlanCode.Equals("F")))
            bankFileInOutFlag = "O";
        else if ((bankFile.PlanCode.Equals("G") || bankFile.PlanCode.Equals("H")))
            bankFileInOutFlag = "I";
        int exchangeProfileID = 0;
        foreach (EHSBCBankPaymentCode bankPaymentCode in BankPaymentCodeList)
        {
            if (bankPaymentCode.HSBCBankPaymentCodeBankAccountNo.Substring(3).Trim().Equals(bankFileBranchAccountNoString.Trim())
                && bankPaymentCode.HSBCBankPaymentCode.Trim().Equals(bankFile.BankPaymentCode.Trim())
                && bankPaymentCode.HSBCBankPaymentCodeAutoPayInOutFlag.Trim().Equals(bankFileInOutFlag)
                )
            {
                HROne.SaaS.Entities.EHSBCExchangeProfile profile = new EHSBCExchangeProfile();
                profile.HSBCExchangeProfileID = bankPaymentCode.HSBCExchangeProfileID;
                if (HROne.SaaS.Entities.EHSBCExchangeProfile.db.select(masterDBConn, profile))
                    if (!profile.HSBCExchangeProfileIsLocked)
                    {
                        bankFile.BankCode = bankPaymentCode.HSBCBankPaymentCodeBankAccountNo.Substring(0, 3);
                        exchangeProfileID = bankPaymentCode.HSBCExchangeProfileID;
                        break;
                    }
            }
        }

        if (string.IsNullOrEmpty(bankFile.BankCode))
            throw new Exception("Bank Account No. or Bank Payment Code is not registered.");

        //if (errors.isEmpty())
        {
            string UploadBankFilePath = ESystemParameter.getParameter(masterDBConn, ESystemParameter.PARAM_CODE_BANKFILE_UPLOAD_FOLDER);
            if (System.IO.Directory.Exists(UploadBankFilePath))
            {
                System.IO.FileInfo bankFileInfo = new System.IO.FileInfo(uploadFile);
                string relativePath = System.IO.Path.Combine(System.IO.Path.Combine(Session["CompanyDBID"].ToString(), "Autopay"), bankFileInfo.Name);
                string fullPath = System.IO.Path.Combine(UploadBankFilePath, relativePath);

                System.IO.Directory.CreateDirectory((new System.IO.FileInfo(fullPath)).Directory.FullName);
                bankFileInfo.MoveTo(fullPath);

                ECompanyAutopayFile CompanyAutopayFile = new ECompanyAutopayFile();
                CompanyAutopayFile.CompanyDBID = (int)Session["CompanyDBID"];
                CompanyAutopayFile.HSBCExchangeProfileID = exchangeProfileID;
                CompanyAutopayFile.CompanyAutopayFileBankCode = bankFile.BankCode;
                CompanyAutopayFile.CompanyAutopayFileDataFileRelativePath = relativePath;
                CompanyAutopayFile.CompanyAutopayFileSubmitDateTime = currentDateTime;
                CompanyAutopayFile.CompanyAutopayFileValueDate = firstValueDate;
                if (!HROne.Lib.Entities.ESystemParameter.getParameter(dbConn, HROne.Lib.Entities.ESystemParameter.PARAM_CODE_ECHANNEL_SIGNATURE_REQUIRED_FOR_AUTOPAY_FILE).Equals("Y", StringComparison.CurrentCultureIgnoreCase))
                    CompanyAutopayFile.CompanyAutopayFileConfirmDateTime = currentDateTime;
                ECompanyAutopayFile.db.insert(masterDBConn, CompanyAutopayFile);

            }
        }
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
