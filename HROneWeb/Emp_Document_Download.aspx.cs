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
using HROne.Lib.Entities;

public partial class Emp_Document_Download : HROneWebPage
{
    private const string FUNCTION_CODE = "PER014";

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!WebUtils.CheckAccess(Response, Session, FUNCTION_CODE, WebUtils.AccessLevel.Read))
            return;


        int EmpDocumentID = 0;
        int EmpID = 0;

        try
        {
            EmpDocumentID = int.Parse(DecryptedRequest["EmpDocumentID"]);
            EmpID = int.Parse(DecryptedRequest["EmpID"]);
        }
        catch
        {
            return;
        }

        //string pathDelimiter = System.IO.Path.DirectorySeparatorChar.ToString();

        EEmpDocument empDocument = new EEmpDocument();
        empDocument.EmpDocumentID = EmpDocumentID;

        if (EEmpDocument.db.select(dbConn, empDocument))
            if (empDocument.EmpID.Equals(EmpID))
            {
                string documentFilePath = empDocument.GetDocumentPhysicalPath(dbConn);
                string transferFilePath = documentFilePath;
                string strTmpFolder = string.Empty;
                if (empDocument.EmpDocumentIsCompressed)
                {

                    transferFilePath = empDocument.GetExtractedFilePath(dbConn);
                }
                if (System.IO.File.Exists(transferFilePath))
                {
                    Response.ClearContent();
                    Response.ClearHeaders();
                    Response.ContentType = "application/download"; //Fixed download problem on https
                    Response.AddHeader("Content-Disposition", "attachment;filename=" + Server.UrlEncode(empDocument.EmpDocumentOriginalFileName));
                    Response.AppendHeader("Content-Length", new System.IO.FileInfo(transferFilePath).Length.ToString());
                    Response.Expires = -1;
                    Response.WriteFile(transferFilePath, true);
                    Response.Flush();
                    //                    WebUtils.TransmitFile(Response, strTmpFolder + pathDelimiter+ fileList[0], empDocument.EmpDocumentOriginalFileName, true);
                }
                empDocument.RemoveExtractedFile();
                Response.End();

            }

    }
}
