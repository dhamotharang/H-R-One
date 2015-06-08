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

public partial class ShowHelpPDF : HROneWebPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (DecryptedRequest["help"] != null)
        {
            string previousURL = DecryptedRequest["help"];
            string helpSearchString = previousURL.Substring(previousURL.LastIndexOf('/') + 1, previousURL.LastIndexOf('.') - (previousURL.LastIndexOf('/') + 1));
            string helpFolder = MapPath("~/help");
            System.IO.DirectoryInfo directoryInfo = new System.IO.DirectoryInfo(helpFolder);
            System.IO.FileInfo[] fileInfoList = directoryInfo.GetFiles("*" + helpSearchString + "*.pdf", System.IO.SearchOption.AllDirectories);
            if (fileInfoList.GetLength(0) > 0)
            {
                System.IO.FileInfo selectedfile = null;

                foreach (System.IO.FileInfo fileInfo in fileInfoList)
                {
                    if (selectedfile==null)
                        selectedfile = fileInfo;
                    else if (selectedfile.Name.Length > fileInfo.Name.Length)
                        selectedfile = fileInfo;
                }

                Response.ClearContent();
                Response.ClearHeaders();
                Response.ContentType = "application/download"; //Fixed download problem on https
                Response.AddHeader("Content-Disposition", "inline;filename=" + selectedfile.FullName);
                Response.AppendHeader("Content-Length", new System.IO.FileInfo(selectedfile.FullName).Length.ToString());
                Response.Expires = -1;
                Response.WriteFile(selectedfile.FullName, true);
                Response.Flush();

                //Response.Clear();
                //Response.TransmitFile(fileInfoList[0].FullName);
                Response.End();
            }
        }
    }
}
