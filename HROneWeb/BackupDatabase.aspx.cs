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
using HROne.DataAccess;

public partial class BackupDatabase : HROneWebPage
{
    private const string FUNCTION_CODE = "SYS000";
    

    protected void Page_Load(object sender, EventArgs e)
    {
       

        if (!WebUtils.CheckAccess(Response, Session, FUNCTION_CODE, WebUtils.AccessLevel.ReadWrite))
            return;

    }
    protected void btnBackup_Click(object sender, EventArgs e)
    {
        DateTime fileDateTime = AppUtils.ServerDateTime();
        string strTempPath = HROne.Common.Folder.GetOrCreateSessionTempFolder(Session.SessionID).FullName;
        string databaseFile = System.IO.Path.Combine(strTempPath, "HROne_" + fileDateTime.ToString("yyyyMMddHHmmss") + ".bak");
        string errorMessage = string.Empty;
        AppUtils.BackUpDatabase(dbConn, databaseFile, out errorMessage);
        if (System.IO.File.Exists(databaseFile))
        {
            System.IO.FileInfo sourceFileInfo = new System.IO.FileInfo(databaseFile);
            string destinationZipFile = System.IO.Path.Combine(strTempPath, "HROne_" + fileDateTime.ToString("yyyyMMddHHmmss") + ".zip");
            zip.Compress(strTempPath, sourceFileInfo.Name, destinationZipFile, ZipPassword.Text);
            sourceFileInfo.Delete();
            if (System.IO.File.Exists(destinationZipFile))
//                WebUtils.RegisterDownloadFileJavaScript(this, destinationZipFile, "HROne_" + fileDateTime.ToString("yyyyMMddHHmmss") + ".zip", true, 0);
                WebUtils.TransmitFile(this.Response, destinationZipFile, "HROne_" + fileDateTime.ToString("yyyyMMddHHmmss") + ".zip", true);
        }
        else
        {
            PageErrors errors = PageErrors.getErrors(HROne.Lib.Entities.ESystemParameter.db, Page.Master);
            if (errors != null)
                errors.addError(errorMessage);
        }
    }
}
