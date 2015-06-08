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

public partial class eChannel_CompanyInbox_Detail : HROneWebPage
{
    private const string FUNCTION_CODE = "ECH000";
    private int CurID;
    private int CurCompanyDBID;
    private DBManager db = ECompanyInbox.db;
    private Binding binding;
    protected DatabaseConnection masterDBConn;

    protected SearchBinding sbinding;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!WebUtils.CheckAccess(Response, Session, FUNCTION_CODE, WebUtils.AccessLevel.Read))
            return;

        if (Application["MasterDBConfig"] != null)
            masterDBConn = ((DatabaseConfig)Application["MasterDBConfig"]).CreateDatabaseConnectionObject();
        else
            Response.Redirect("~/AccessDeny.aspx");


        if (!int.TryParse(DecryptedRequest["CompanyInboxID"], out CurID))
            CurID = -1;

        if (Session["CompanyDBID"] != null)
            CurCompanyDBID = (int)Session["CompanyDBID"];


        HROne.Common.WebUtility.WebControlsLocalization(this, this.Controls);

        binding = new Binding(masterDBConn, ECompanyInbox.db);
        binding.add(CompanyInboxID);
        binding.add(CompanyInboxCreateDate);
        binding.add(CompanyInboxSubject);
        binding.add(CompanyInboxMessage);
        binding.init(Request, Session);

        sbinding = new SearchBinding(masterDBConn, ECompanyInboxAttachment.db);
        sbinding.init(null);

        HROne.Common.WebUtility.WebControlsLocalization(this, this.Controls);

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
        ECompanyInbox obj = new ECompanyInbox();
        bool isNew = WebFormWorkers.loadKeys(db, obj, DecryptedRequest);
        if (!db.select(masterDBConn, obj))
            if (CurID <= 0)
                return false;
            else
                HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "~/AccessDeny.aspx");
        if (obj.CompanyDBID != 0 && obj.CompanyDBID != CurCompanyDBID)
            HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "~/AccessDeny.aspx");
        if (obj.CompanyInboxReadDate.Ticks.Equals(0))
        {
            obj.CompanyInboxReadDate = AppUtils.ServerDateTime();
            db.update(masterDBConn, obj);
        }
        Hashtable values = new Hashtable();
        db.populate(obj, values);
        binding.toControl(values);

        DBFilter inboxAttachmentFilter = new DBFilter();
        inboxAttachmentFilter.add(new Match("CompanyInboxID", obj.CompanyInboxID));
        ArrayList inboxAttachmentList = ECompanyInboxAttachment.db.select(masterDBConn, inboxAttachmentFilter);
        if (inboxAttachmentList.Count > 0)
        {
            CompanyInboxAttachmentRepeater.DataSource = inboxAttachmentList;
            CompanyInboxAttachmentRepeater.DataBind();
        }
        else
        {
            AttachmentRow.Visible = false;
        }
        return true;
    }
    protected void CompanyInboxAttachmentRepeater_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        ECompanyInboxAttachment attachment = (ECompanyInboxAttachment)e.Item.DataItem;

    }

    //protected void Delete_Click(object sender, EventArgs e)
    //{
    //    EInbox o = new EInbox();
    //    o.InboxID = CurID;
    //    if (db.select(dbConn, o))
    //    {

    //        string uploadFolder = uploadFolder = AppUtils.GetDocumentUploadFolder(dbConn);

    //        DBFilter attachmentFilter = new DBFilter();
    //        attachmentFilter.add(new Match("InboxID", o.InboxID));
    //        ArrayList attachmentList = EInboxAttachment.db.select(dbConn, attachmentFilter);

    //        foreach (EInboxAttachment attachment in attachmentList)
    //        {
    //            try
    //            {
    //                string UploadFile = System.IO.Path.Combine(uploadFolder, attachment.InboxAttachmentStoredFileName);
    //                if (System.IO.File.Exists(UploadFile))
    //                    System.IO.File.Delete(UploadFile);
    //            }
    //            catch
    //            {
    //            }
    //            finally
    //            {
    //                EInboxAttachment.db.delete(dbConn, attachment);
    //            }
    //        }
    //        db.delete(dbConn, o);
    //    }
    //    HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "~/Inbox_List.aspx");
    //}

    protected void Back_Click(object sender, EventArgs e)
    {
        HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "~/Echannel_CompanyInbox_List.aspx");
    }

}
