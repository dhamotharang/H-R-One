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
using HROne.DataAccess;

public partial class Inbox_Detail : HROneWebPage
{
    private int CurID;
    private DBManager db = EInbox.db;
    private Binding binding;

    protected SearchBinding sbinding;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!WebUtils.CheckAccess(Response, Session))
            return;

        if (!int.TryParse(DecryptedRequest["InboxID"], out CurID))
            CurID = -1;

        HROne.Common.WebUtility.WebControlsLocalization(this, this.Controls);

        binding = new Binding(dbConn, EInbox.db);
        binding.add(InboxID);
        binding.add(InboxCreateDate);
        binding.add(InboxSubject);
        binding.add(InboxMessage);
        binding.init(Request, Session);

        sbinding = new SearchBinding(dbConn, EInboxAttachment.db);
        sbinding.init(null);
    }
    protected void Page_PreRender(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            if (CurID > 0)
            {
                loadObject();
            }
            //else
            //    toolBar.DeleteButton_Visible = false;
        }
    }
    protected bool loadObject()
    {
        EInbox obj = new EInbox();
        bool isNew = WebFormWorkers.loadKeys(db, obj, DecryptedRequest);
        if (!db.select(dbConn, obj))
            if (CurID <= 0)
                return false;
            else
                HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "~/AccessDeny.aspx");
        if (obj.UserID != 0 && obj.UserID != user.UserID)
            HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "~/AccessDeny.aspx");
        if (obj.InboxReadDate.Ticks.Equals(0))
        {
            obj.InboxReadDate = AppUtils.ServerDateTime();
            db.update(dbConn, obj);
        }
        if (obj.InboxType.Equals(EInbox.INBOX_TYPE_DATE_OF_BIRTH) || obj.InboxType.Equals(EInbox.INBOX_TYPE_DATE_OF_BIRTH_18) || obj.InboxType.Equals(EInbox.INBOX_TYPE_DATE_OF_BIRTH_65) || obj.InboxType.Equals(EInbox.INBOX_TYPE_PROBATION))
        {
            HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "~/Emp_View.aspx?EmpID=" + obj.EmpID);
        }
        else if (obj.InboxType.Equals(EInbox.INBOX_TYPE_TERMINATION))
        {
            HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "~/EmpTab_Termination_View.aspx?EmpID=" + obj.EmpID);
        }
        else if (obj.InboxType.Equals(EInbox.INBOX_TYPE_WORKPERMITEXPIRY))
        {
            HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "~/Emp_Permit_view.aspx?EmpID=" + obj.EmpID + "&EmpPermitID=" + obj.InboxRelatedRecID);
        }
        Hashtable values = new Hashtable();
        db.populate(obj, values);
        binding.toControl(values);

        InboxFromUserID.Text = obj.GetFromUserName(dbConn);
        InboxSubject.Text = AppUtils.GetActualInboxSubject(dbConn, obj);
        DBFilter inboxAttachmentFilter = new DBFilter();
        inboxAttachmentFilter.add(new Match("InboxID", obj.InboxID));
        ArrayList inboxAttachmentList = EInboxAttachment.db.select(dbConn, inboxAttachmentFilter);
        if (inboxAttachmentList.Count > 0)
        {
            InboxAttachmentRepeater.DataSource = inboxAttachmentList;
            InboxAttachmentRepeater.DataBind();
        }
        else
        {
            AttachmentRow.Visible = false;
        }
        System.Collections.Generic.List<string> inboxTypeArray = new System.Collections.Generic.List<string>( obj.InboxType.Split(new string[] { "|" }, StringSplitOptions.RemoveEmptyEntries));
        if (inboxTypeArray.Contains(EInbox.INBOX_TYPE_FOR_ECHANNEL) && !inboxTypeArray.Contains(EInbox.INBOX_TYPE_FOR_ECHANNEL_SUBMITTED))
        {
            eChannelRow.Visible = true;
            InboxMessage.Text += "<br/>"
                + "<p style=\"color:red\">" + HttpUtility.HtmlEncode(HROne.Translation.PageMessage.BANKFILE_CONSOLIDATION_MESSAGE) + "<br/>"
                + HttpUtility.HtmlEncode(HROne.Translation.PageMessage.BANKFILE_CLEARING_MESSAGE) + "</p><br/>";
        }
        else
            eChannelRow.Visible = false;

        return true;
    }
    protected void InboxAttachmentRepeater_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        EInboxAttachment attachment = (EInboxAttachment)e.Item.DataItem;

    }

    protected void Delete_Click(object sender, EventArgs e)
    {
        EInbox o = new EInbox();
        o.InboxID = CurID;
        if (db.select(dbConn, o))
        {
            o.Delete(dbConn);
            //string uploadFolder = uploadFolder = AppUtils.GetDocumentUploadFolder(dbConn);

            //DBFilter attachmentFilter = new DBFilter();
            //attachmentFilter.add(new Match("InboxID", o.InboxID));
            //ArrayList attachmentList = EInboxAttachment.db.select(dbConn, attachmentFilter);

            //foreach (EInboxAttachment attachment in attachmentList)
            //{
            //    try
            //    {
            //        string UploadFile = System.IO.Path.Combine(uploadFolder, attachment.InboxAttachmentStoredFileName);
            //        if (System.IO.File.Exists(UploadFile))
            //            System.IO.File.Delete(UploadFile);
            //    }
            //    catch
            //    {
            //    }
            //    finally
            //    {
            //        EInboxAttachment.db.delete(dbConn, attachment);
            //    }
            //}
            //db.delete(dbConn, o);
        }
        HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "~/Inbox_List.aspx");
    }

    protected void Back_Click(object sender, EventArgs e)
    {
        HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "~/Inbox_List.aspx");
    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        EInbox o = new EInbox();
        o.InboxID = CurID;
        if (db.select(dbConn, o))
        {
            System.Collections.Generic.List<string> inboxTypeArray = new System.Collections.Generic.List<string>(o.InboxType.Split(new string[] { "|" }, StringSplitOptions.RemoveEmptyEntries));
            if (inboxTypeArray.Contains(EInbox.INBOX_TYPE_FOR_ECHANNEL) && !inboxTypeArray.Contains(EInbox.INBOX_TYPE_FOR_ECHANNEL_SUBMITTED))
            {
                if (inboxTypeArray.Contains(EInbox.INBOX_TYPE_AUTOPAY_FILE))
                    HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "~/eChannel_SubmitAutopayFile.aspx?inboxID=" + CurID);
                else if (inboxTypeArray.Contains(EInbox.INBOX_TYPE_MPF_FILE))
                    HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "~/eChannel_SubmitMPFContributionFile.aspx?inboxID=" + CurID);
                eChannelRow.Visible = false;
            }
            else
            {
                PageErrors errors = PageErrors.getErrors(db, this.Master);
                errors.addError("File is already submitted");
            }
        }        
        //HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "~/Inbox_List.aspx");

    }
}
