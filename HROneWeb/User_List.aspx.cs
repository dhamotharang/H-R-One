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

using HROne.Lib.Entities;

public partial class User_List : HROneWebPage
{
    private const string FUNCTION_CODE = "SEC001";

    protected DBManager db = EUser.db;
    protected SearchBinding binding;
    protected ListInfo info;
    protected DataView view;
    

    protected void Page_Load(object sender, EventArgs e)
    {
       
        if (!WebUtils.CheckAccess(Response, Session, FUNCTION_CODE, WebUtils.AccessLevel.Read))
            return;
        toolBar.FunctionCode = FUNCTION_CODE;
        SelectAllPanel.Visible = toolBar.DeleteButton_Visible;

        binding = new SearchBinding(dbConn, db);
        binding.add(new LikeSearchBinder(LoginID, "LoginID"));
        binding.add(new LikeSearchBinder(UserName, "UserName"));
        binding.initValues("UserAccountStatus", null, EUser.VLAccountStatus, null);
        binding.init(DecryptedRequest, null);
        HROne.Common.WebUtility.WebControlsLocalization(this, this.Controls);
        info = ListFooter.ListInfo;

        if (!Page.IsPostBack)
        {
            view = loadData(info, db, Repeater);
        }

        if (WebUtils.TotalActiveUser(dbConn, 0) >= WebUtils.productLicense(Session).NumOfUsers)
            toolBar.NewButton_Visible = false;
        else
            toolBar.NewButton_Visible = toolBar.DeleteButton_Visible;

    }
    public DataView loadData(ListInfo info, DBManager db, Repeater repeater)
    {
        DBFilter filter = binding.createFilter();

        filter.add(new Match("UserAccountStatus", "<>", "D"));

        if (info != null && info.orderby != null && !info.orderby.Equals(""))
            filter.add(info.orderby, info.order);

        string select = "c.*";
        string from = "from [" + db.dbclass.tableName + "] c ";

        DataTable table = WebUtils.GetDataTableFromSelectQueryWithFilter(dbConn, select, from, filter, info);
        
        view = new DataView(table);
        ListFooter.Refresh();
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
        CheckBox cb = (CheckBox)e.Item.FindControl("ItemSelect");

        EUser obj = new EUser();
        db.toObject(((DataRowView)e.Item.DataItem).Row, obj);

        EUser user = WebUtils.GetCurUser(Session);
        if (obj.UserID.Equals(1))
            cb.Visible = false;
        else if (user != null)
            if (obj.UserID.Equals(user.UserID))
                cb.Visible = false;
            else if (user.UsersCannotCreateUsersWithMorePermission)
            {
                if (!obj.UsersCannotCreateUsersWithMorePermission)
                    cb.Visible = false;
                else if (!obj.IsAllowSubmitPermission(dbConn, user.UserID))
                    cb.Visible = false;
            }
        WebFormUtils.LoadKeys(db, row, cb);
    }
    protected void Delete_Click(object sender, EventArgs e)
    {
        ArrayList list = WebUtils.SelectedRepeaterItemToBaseObjectList(db, Repeater, "ItemSelect");

        EUser activeUser = WebUtils.GetCurUser(Session);
        if (activeUser != null)
            foreach (EUser user in list)
            {
                if (EUser.db.select(dbConn, user))
                {
                    bool isAllowDelete = true;
                    if (activeUser.UsersCannotCreateUsersWithMorePermission)
                    {
                        if (!user.UsersCannotCreateUsersWithMorePermission)
                        {
                            isAllowDelete = false;
                        }
                        else if (!user.IsAllowSubmitPermission(dbConn, activeUser.UserID))
                        {
                            isAllowDelete = false;
                        }
                    }

                    if (isAllowDelete)
                    {
                        WebUtils.StartFunction(Session, FUNCTION_CODE);
                        user.UserAccountStatus = "D";
                        db.update(dbConn, user);
                        WebUtils.EndFunction(dbConn);
                        EInbox.DeleteAllByUserID(dbConn, user.UserID);
                    }
                }
            }
        loadData(info, db, Repeater);
    }
    public void New_Click(object sender, EventArgs e)
    {
        HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "User_Edit.aspx");
    }

}
