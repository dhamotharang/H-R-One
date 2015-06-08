using System;
using System.IO;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using HROne.DataAccess;

public partial class FileDownload : HROneWebPage
{
    protected const string FILE_PATH_STRING = "path";
    protected const string FILE_NAME_STRING = "name";
    protected const string REMOVE_FILE_STRING = "RemoveAfterDownload";

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!WebUtils.CheckAccess(Response, Session))
            return;

        if (Request.QueryString[FILE_PATH_STRING] != null)
        {
            DBAESEncryptStringFieldAttribute decrypt = new DBAESEncryptStringFieldAttribute(Session.SessionID);
            string tmpFilePath = decrypt.fromDB(Request.QueryString[FILE_PATH_STRING]).ToString();
            if (File.Exists(tmpFilePath))
            {
                string filename=string.Empty;
                if (Request.QueryString[FILE_PATH_STRING] != null)
                    filename = decrypt.fromDB(Request.QueryString[FILE_NAME_STRING]).ToString();
                else
                    filename = new FileInfo(tmpFilePath).Name;
                bool blnRemoveAfterDownload = false;
                if (Request.QueryString[REMOVE_FILE_STRING] != null)
                    blnRemoveAfterDownload = (Request.QueryString[REMOVE_FILE_STRING]).ToString().Equals("Y", StringComparison.CurrentCultureIgnoreCase);
                WebUtils.TransmitFile(Response, tmpFilePath, filename, blnRemoveAfterDownload);
            }
        }


    }
}
