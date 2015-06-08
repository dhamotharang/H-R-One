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

public partial class Inbox_Attachment_Download : HROneWebPage
{

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!WebUtils.CheckAccess(Response, Session))
            return;

        int InboxAttachmentID = 0;
        int InboxID = 0;

        try
        {
            InboxAttachmentID = int.Parse(DecryptedRequest["InboxAttachmentID"]);
            InboxID = int.Parse(DecryptedRequest["InboxID"]);
        }
        catch
        {
            return;
        }

        //string pathDelimiter = System.IO.Path.DirectorySeparatorChar.ToString();

        EInboxAttachment inboxAttachment = new EInboxAttachment();
        inboxAttachment.InboxAttachmentID = InboxAttachmentID;

        if (EInboxAttachment.db.select(dbConn, inboxAttachment))
            if (inboxAttachment.InboxID.Equals(InboxID))
            {
                EInbox inbox = new EInbox();
                inbox.InboxID = InboxID;
                if (EInbox.db.select(dbConn, inbox))
                {
                    if (inbox.UserID.Equals(user.UserID) || inbox.UserID.Equals(0))
                    {
                        string documentFilePath = inboxAttachment.GetDocumentPhysicalPath(dbConn);
                        string transferFilePath = documentFilePath;
                        string strTmpFolder = string.Empty;
                        if (inboxAttachment.InboxAttachmentIsCompressed)
                        {

                            transferFilePath = inboxAttachment.GetExtractedFilePath(dbConn);
                        }
                        if (System.IO.File.Exists(transferFilePath))
                        {
                            Response.ClearContent();
                            Response.ClearHeaders();
                            Response.ContentType = "application/download"; //Fixed download problem on https
                            Response.AddHeader("Content-Disposition", "attachment;filename=" + Server.UrlEncode(inboxAttachment.InboxAttachmentOriginalFileName));
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
