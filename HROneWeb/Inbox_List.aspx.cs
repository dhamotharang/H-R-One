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
//using perspectivemind.validation;
using System.Globalization;
using HROne.Lib.Entities;
using HROne.Translation;

public partial class Inbox_List : HROneWebPage
{
    protected DBManager db = EInbox.db;
    protected SearchBinding binding;
    protected ListInfo info;
    protected DataView view;
    

    protected void Page_Load(object sender, EventArgs e)
    {
       
        if (!WebUtils.CheckAccess(Response, Session))
            return;
        //toolBar.FunctionCode = FUNCTION_CODE;
        SelectAllPanel.Visible = toolBar.DeleteButton_Visible;

        info = ListFooter.ListInfo;

        HROne.Common.WebUtility.WebControlsLocalization(this, this.Controls);


        binding = new SearchBinding(dbConn, db);
        binding.init(DecryptedRequest, null);



    }
    protected void Page_PreRender(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            view = loadData(info, db, Repeater);
        }
        lblTotalSize.Text = ((double)(EInboxAttachment.GetTotalSize(dbConn, user.UserID)/1000.0/1000.0)).ToString("0.000") + "MB";
    }

    public DataView loadData(ListInfo info, DBManager db, Repeater repeater)
    {
        DBFilter filter = binding.createFilter();

        if (info != null && info.orderby != null && !info.orderby.Equals(""))
            filter.add(info.orderby, info.order);
        else
            filter.add("inb.InboxCreateDate", false);

        OR orUserIDFilter = new OR();
        orUserIDFilter.add(new Match("inb.UserID", WebUtils.GetCurUser(Session).UserID));
        orUserIDFilter.add(new Match("inb.UserID", 0));
        filter.add(orUserIDFilter);
        filter.add(new NullTerm("inb.InboxDeleteDate"));
        string select = "inb.*";
        string from = "from [" + db.dbclass.tableName + "] inb ";

        DataTable table = WebUtils.GetDataTableFromSelectQueryWithFilter(dbConn, select, from, filter, info);
        foreach (DataRow row in table.Rows)
        {
            EInbox obj = new EInbox();
            db.toObject(row, obj);

            if (string.IsNullOrEmpty(obj.InboxSubject))
                row["InboxSubject"] = AppUtils.GetActualInboxSubject(dbConn, obj);
            if (string.IsNullOrEmpty(obj.InboxFromUserName))
                row["InboxFromUserName"] = obj.GetFromUserName(dbConn);
        }


        view = new DataView(table);

        if (repeater != null)
        {
            repeater.DataSource = view;
            repeater.DataBind();
        }

        return view;
    }
    protected void Search_Click(object sender, EventArgs e)
    {
        info.page = 0;
        view = loadData(info, db, Repeater);

    }
    protected void Reset_Click(object sender, EventArgs e)
    {
        binding.clear();
        info.page = 0;
        view = loadData(info, db, Repeater);

    }
    protected void FirstPage_Click(object sender, EventArgs e)
    {
        view = loadData(info, db, Repeater);

    }
    protected void PrevPage_Click(object sender, EventArgs e)
    {
        view = loadData(info, db, Repeater);

    }
    protected void NextPage_Click(object sender, EventArgs e)
    {
        view = loadData(info, db, Repeater);

    }
    protected void LastPage_Click(object sender, EventArgs e)
    {
        view = loadData(info, db, Repeater);

    }
    protected void ChangeOrder_Click(object sender, EventArgs e)
    {
        LinkButton l = (LinkButton)sender;
        String id = l.ID.Substring(1);
        if (info.orderby == null)
            info.order = true;
        else if (info.orderby.Equals(id))
            info.order = !info.order;
        else
            info.order = true;
        info.orderby = id;

        view = loadData(info, db, Repeater);

    }
    protected void Repeater_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        DataRowView row = (DataRowView)e.Item.DataItem;

        EInbox obj = new EInbox();
        db.toObject(((DataRowView)e.Item.DataItem).Row, obj);

        CheckBox cb = (CheckBox)e.Item.FindControl("ItemSelect");
        WebFormUtils.LoadKeys(db, row, cb);

        if (obj.UserID.Equals(0))
            cb.Visible = false;
        Label InboxFromUserName = (Label)e.Item.FindControl("InboxFromUserName");
        Label InboxCreateDate = (Label)e.Item.FindControl("InboxCreateDate");
        Label InboxSubject = (Label)e.Item.FindControl("InboxSubject");
        InboxFromUserName.Text = obj.InboxFromUserName;
        InboxCreateDate.Text = obj.InboxCreateDate.ToString("yyyy-MM-dd HH:mm:ss");
        InboxSubject.Text = obj.InboxSubject;

        if (obj.InboxReadDate.Ticks.Equals(0))
        {
            InboxFromUserName.Font.Bold = true;
            InboxCreateDate.Font.Bold = true;
            InboxSubject.Font.Bold = true;
        }

        cb.Visible = toolBar.DeleteButton_Visible;
    }
    protected void Delete_Click(object sender, EventArgs e)
    {
        PageErrors errors = PageErrors.getErrors(db, Page.Master);
        errors.clear();

        ArrayList list = WebUtils.SelectedRepeaterItemToBaseObjectList(db, Repeater, "ItemSelect");
        foreach (EInbox o in list)
        {
            o.Delete(dbConn);
        }
        loadData(info, db, Repeater);
    }
    public void New_Click(object sender, EventArgs e)
    {
        HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "Emp_Edit.aspx");
    }
    //protected void loadHierarchy()
    //{
    //    DBFilter filter;
    //    ArrayList list;


    //    filter = new DBFilter();
    //    filter.add("HLevelSeqNo", true);
    //    list = EHierarchyLevel.db.select(dbConn, filter);
    //    HierarchyLevel.DataSource = list;
    //    HierarchyLevel.DataBind();


    //}

}
