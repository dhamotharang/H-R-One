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
using HROne.Import;
using HROne.Lib.Entities;
using HROne.DataAccess;
//using perspectivemind.validation;

public partial class SelectFile_For_Import : HROneWebPage
{
    //private const string FUNCTION_CODE = "PAY003";

    //private DBManager db = EUploadClaimsAndDeductions.db;
    //protected SearchBinding sbinding;
    //protected Binding ebinding;
    //protected ListInfo info;
    //private DataView view;

    public int gPID;
    public string gProcessName;
    public string gP1;
    static string prevPage = String.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {

        if (!Page.IsPostBack)
        {
            prevPage = Request.UrlReferrer.ToString();
        }

        //binding = new SearchBinding(dbConn, db);

        if (!string.IsNullOrEmpty(DecryptedRequest["Process"]))
            gProcessName = DecryptedRequest["Process"].ToString();

        if (!int.TryParse(DecryptedRequest["PID"], out gPID))
            gPID = -1;

        if (!string.IsNullOrEmpty(DecryptedRequest["p1"]))
            gP1 = DecryptedRequest["p1"].ToString();

        HROne.Common.WebUtility.WebControlsLocalization(this, this.Controls);
    }

    protected void btnBack_Click(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(prevPage))
        {
            HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, prevPage);
        }
        else
            HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "~/Emp_list.aspx");
    }

    protected void Upload_Click(object sender, EventArgs e)
    {
        PageErrors errors = PageErrors.getErrors(EEmpPersonalInfo.db, Page.Master);
        errors.clear();

        if (FileUploadControl.HasFile)
        {
            string strTmpFolder = HROne.Common.Folder.GetOrCreateSessionTempFolder(Session.SessionID).FullName;
            //Environment.GetFolderPath(Environment.SpecialFolder.InternetCache);
            string strTmpFile = System.IO.Path.Combine(strTmpFolder, AppUtils.ServerDateTime().ToString("~yyyyMMddHHmmss_") + FileUploadControl.FileName);
            FileUploadControl.SaveAs(strTmpFile);

            string ZipPassword = "";
            DataTable rawDataTable = HROne.Import.ExcelImport.parse(strTmpFile, ZipPassword).Tables[0];

            if (string.Compare(gProcessName, "BonusProcess", true) == 0)
            {
                HROne.Import.ImportBonusProcess m_importProcess = new HROne.Import.ImportBonusProcess(dbConn, Session.SessionID, WebUtils.GetCurUser(Session).UserID, gPID);

                m_importProcess.ImportEmpBonusProcess(gPID, rawDataTable, gP1, errors);
            }
            else if (string.Compare(gProcessName, "DoublePayAdjustment", true) == 0)
            {
                HROne.Import.ImportDoublePayAdjustmentProcess m_importProcess = new HROne.Import.ImportDoublePayAdjustmentProcess(dbConn, Session.SessionID, WebUtils.GetCurUser(Session).UserID);
                m_importProcess.UploadToTempDatabase(rawDataTable, WebUtils.GetCurUser(Session).UserID);
            }

            System.IO.File.Delete(strTmpFile);
            if (errors.isEmpty())
            {
                errors.addError("Import Completed");
            }
        }
        else
            errors.addError(HROne.Translation.PageErrorMessage.ERROR_INVALID_FILE);
    }
}
