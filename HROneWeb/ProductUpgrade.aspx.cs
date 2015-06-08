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
using System.IO;
//using ICSharpCode.SharpZipLib.Zip;

public partial class ProductUpgrade : HROneWebPage
{

    private string strSourcePath;

    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void Page_LoadComplete(object sender, EventArgs e)
    {
        if (!File.Exists(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory , @"Default.aspx.cs")))

            if (!string.IsNullOrEmpty(strSourcePath) && !string.IsNullOrEmpty(strSourcePath))
            {
                System.Diagnostics.Process process = new System.Diagnostics.Process();
                process.StartInfo.FileName = System.IO.Path.Combine(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"bin"), "HROne.Patching.exe");
                process.StartInfo.Arguments = "HROneWeb \"" + strSourcePath + "\"";
                process.StartInfo.UserName = Username.Text;
                foreach (char c in Password.Text.ToCharArray())
                    process.StartInfo.Password.AppendChar(c);
                process.Start();
            }
    }

    protected void Upload_Click(object sender, EventArgs e)
    {
        string strTmpFolder = HROne.Common.Folder.GetOrCreateSessionTempFolder(Session.SessionID).FullName; ; //Environment.GetFolderPath(Environment.SpecialFolder.InternetCache);
        string strTmpFile = System.IO.Path.Combine(strTmpFolder, AppUtils.ServerDateTime().ToString("~yyyyMMddHHmmss_") + BinFile.FileName);
        //strTmpFile = strTmpFile.Replace(Path.DirectorySeparatorChar.ToString() + Path.DirectorySeparatorChar, Path.DirectorySeparatorChar);
        BinFile.SaveAs(strTmpFile);

        strSourcePath = strTmpFile;

        UploadPanel.Visible = false;
        UpgradingPanel.Visible = true;

        Response.Write(@"<script language='javascript'>");
        Response.Write(@"setTimeout('Redirect()',15000);");
        Response.Write(@"function Redirect()");
        Response.Write(@"{");
        Response.Write(@"window.location = 'Logout.aspx"
            + "';");
        Response.Write(@"}");
        Response.Write(@"</script>");



    }



    //private void CopyFolder(string sourcePath, string destinationPath)
    //{

    //    DirectoryInfo sourceDir = new DirectoryInfo(sourcePath);
    //    foreach (FileInfo fileInfo in sourceDir.GetFiles())
    //    {
    //        if (!File.Exists(AppDomain.CurrentDomain.BaseDirectory + Path.DirectorySeparatorChar.ToString() + @"Default.aspx.cs"))
    //        {
    //            fileInfo.CopyTo(destinationPath + Path.DirectorySeparatorChar.ToString() + fileInfo.Name, true);
    //        }
    //    }
    //    foreach (DirectoryInfo dir in sourceDir.GetDirectories())
    //    {
    //        CopyFolder(dir.FullName, destinationPath + Path.DirectorySeparatorChar.ToString() + dir.Name);
    //    }
    //}
}
