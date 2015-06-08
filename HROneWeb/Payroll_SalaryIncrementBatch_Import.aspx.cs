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

public partial class Payroll_SalaryIncrementBatch_Import : HROneWebPage
{
    private const string FUNCTION_CODE = "PAY016";

    private DBManager db = ESalaryIncrementBatch.db;
    protected SearchBinding sbinding;
    protected Binding ebinding;
    protected ListInfo info;
    private DataView view;

    public int CurID = -1;

    protected void Page_Load(object sender, EventArgs e)
    {

        if (!WebUtils.CheckAccess(Response, Session, FUNCTION_CODE, WebUtils.AccessLevel.ReadWrite))
            return;
        if (ESystemParameter.getParameter(dbConn, ESystemParameter.PARAM_CODE_PAYSCALE_POINT_SYSTEM) != "Y")
        {
            HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "~/AccessDeny.aspx");
            return;
        }

        if (!int.TryParse(DecryptedRequest["ID"], out CurID))
            CurID = -1;

        HROne.Common.WebUtility.WebControlsLocalization(this, this.Controls);
    }

     protected void Upload_Click(object sender, EventArgs e)
    {
        PageErrors errors = PageErrors.getErrors(db, Page.Master);
        errors.clear();

        if (CNDImportFile.HasFile)
        {
            string strTmpFolder = HROne.Common.Folder.GetOrCreateSessionTempFolder(Session.SessionID).FullName; ; //Environment.GetFolderPath(Environment.SpecialFolder.InternetCache);
            string strTmpFile = System.IO.Path.Combine(strTmpFolder, AppUtils.ServerDateTime().ToString("~yyyyMMddHHmmss_") + CNDImportFile.FileName);
            CNDImportFile.SaveAs(strTmpFile);

            ImportPayScaleSalaryIncrement m_ImportProcess = new ImportPayScaleSalaryIncrement(dbConn, Session.SessionID, WebUtils.GetCurUser(Session).UserID, this.CurID);

            DataTable table = m_ImportProcess.UploadToTempDatabase(strTmpFile, WebUtils.GetCurUser(Session).UserID, string.Empty);

            if (m_ImportProcess.errors.List.Count > 0)
            {
                foreach (string errorString in m_ImportProcess.errors.List)
                    errors.addError(errorString);
            }
            else
            {

                if (table.Rows.Count > 0)
                {
                    errors.addError(HROne.Translation.PageMessage.IMPORT_SUCCESSFUL);
                    HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "Payroll_SalaryIncrementBatch_View.aspx?BatchID=" + CurID);
                }
                else
                {
                    errors.addError("No records uploaded");
                }
            }
            System.IO.File.Delete(strTmpFile);
        }
        else
            errors.addError(HROne.Translation.PageErrorMessage.ERROR_INVALID_FILE);
    }

 
    //protected void Import_Click(object sender, EventArgs e)
    //{
    //    ImportPayScaleSalaryIncrement BackpayImport = new ImportPayScaleSalaryIncrement(dbConn, Session.SessionID, WebUtils.GetCurUser(Session).UserID);
    //    WebUtils.StartFunction(Session, FUNCTION_CODE);
    //    BackpayImport.ImportToDatabase();
    //    WebUtils.EndFunction(dbConn);
    //    //loadData(info, db, Repeater);
    //    PageErrors.getErrors(db, Page).addError(HROne.Translation.PageMessage.IMPORT_SUCCESSFUL);

    //}

}
