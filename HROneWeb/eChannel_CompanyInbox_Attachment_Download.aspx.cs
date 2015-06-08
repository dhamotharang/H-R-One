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
using HROne.SaaS.Entities;
using HROne.DataAccess;

public partial class eChannel_CompanyInbox_Attachment_Download : HROneWebPage
{
    private const string FUNCTION_CODE = "ECH000";
    private int CurCompanyInboxAttachmentID;
    private int CurCompanyInboxID;
    private int CurCompanyDBID;
    private DBManager db = ECompanyInboxAttachment.db;
    protected DatabaseConnection masterDBConn;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!WebUtils.CheckAccess(Response, Session, FUNCTION_CODE, WebUtils.AccessLevel.Read))
            return;

        if (Application["MasterDBConfig"] != null)
            masterDBConn = ((DatabaseConfig)Application["MasterDBConfig"]).CreateDatabaseConnectionObject();
        else
            Response.Redirect("~/AccessDeny.aspx");


        if (!int.TryParse(DecryptedRequest["CompanyInboxAttachmentID"], out CurCompanyInboxAttachmentID))
            CurCompanyInboxAttachmentID = -1;

        if (!int.TryParse(DecryptedRequest["CompanyInboxID"], out CurCompanyInboxID))
            CurCompanyInboxID = -1;

        if (Session["CompanyDBID"] != null)
            CurCompanyDBID = (int)Session["CompanyDBID"];
        

        //string pathDelimiter = System.IO.Path.DirectorySeparatorChar.ToString();

        ECompanyInboxAttachment inboxAttachment = new ECompanyInboxAttachment();
        inboxAttachment.CompanyInboxAttachmentID = CurCompanyInboxAttachmentID;

        if (ECompanyInboxAttachment.db.select(masterDBConn, inboxAttachment))
            if (inboxAttachment.CompanyInboxID.Equals(CurCompanyInboxID))
            {
                ECompanyInbox inbox = new ECompanyInbox();
                inbox.CompanyInboxID = CurCompanyInboxID;
                if (ECompanyInbox.db.select(masterDBConn, inbox))
                {
                    if (inbox.CompanyDBID.Equals(CurCompanyDBID) || inbox.CompanyDBID.Equals(0))
                    {

                        string documentFilePath = inboxAttachment.GetDocumentPhysicalPath(masterDBConn);// ESystemParameter.getParameter(masterDBConn, ESystemParameter.PARAM_CODE_BANKFILE_UPLOAD_FOLDER);
                        string transferFilePath = documentFilePath;
                        string strTmpFolder = string.Empty;
                        if (inboxAttachment.CompanyInboxAttachmentIsCompressed)
                        {

                            transferFilePath = inboxAttachment.GetExtractedFilePath(masterDBConn);
                        }
                        if (System.IO.File.Exists(transferFilePath))
                        {
                            Response.ClearContent();
                            Response.ClearHeaders();
                            Response.ContentType = "application/download"; //Fixed download problem on https
                            Response.AddHeader("Content-Disposition", "attachment;filename=" + Server.UrlEncode(inboxAttachment.CompanyInboxAttachmentOriginalFileName));
                            Response.AppendHeader("Content-Length", new System.IO.FileInfo(transferFilePath).Length.ToString());
                            Response.Expires = -1;
                            Response.WriteFile(transferFilePath, true);
                            Response.Flush();
                            //                    WebUtils.TransmitFile(Response, strTmpFolder + pathDelimiter+ fileList[0], empDocument.EmpDocumentOriginalFileName, true);
                        }
                        inboxAttachment.RemoveExtractedFile();
                        Response.End();
                    }
                }
            }

    }
}
