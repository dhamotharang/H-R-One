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

public partial class UserGroup_List : HROneWebPage
{
    private const string FUNCTION_CODE = "SEC002";

    protected DBManager db = EUserGroup.db;
    protected SearchBinding binding;
    protected ListInfo info;
    protected DataView view;
    private bool IsAllowEdit = true;
    

    protected void Page_Load(object sender, EventArgs e)
    {
       
        if (!WebUtils.CheckAccess(Response, Session, FUNCTION_CODE, WebUtils.AccessLevel.Read))
            return;
        if (!WebUtils.CheckPermission(Session, FUNCTION_CODE, WebUtils.AccessLevel.ReadWrite))
        {
            IsAllowEdit = false;
        }
        toolBar.FunctionCode = FUNCTION_CODE;
        SelectAllPanel.Visible = toolBar.DeleteButton_Visible;

        binding = new SearchBinding(dbConn, db);
        binding.add(new LikeSearchBinder(UserGroupName, "UserGroupName"));
        binding.add(new LikeSearchBinder(UserGroupDesc, "UserGroupDesc"));

        binding.init(DecryptedRequest, null);

        HROne.Common.WebUtility.WebControlsLocalization(this, this.Controls);
        info = ListFooter.ListInfo;
        if (!Page.IsPostBack)
        {
            view = loadData(info, db, Repeater);
        }
        
    }
    public DataView loadData(ListInfo info, DBManager db, Repeater repeater)
    {
        DBFilter filter = binding.createFilter();

        if (info != null && info.orderby != null && !info.orderby.Equals(""))
            filter.add(info.orderby, info.order);

        string select = "c.*";
        string from = "from " + db.dbclass.tableName + " c ";

        DataTable table = WebUtils.GetDataTableFromSelectQueryWithFilter(dbConn, select, from, filter, info);
        
        view = new DataView(table);
        ListFooter.Refresh();
        if (repeater != null)
        {
            repeater.DataSource = view;
            repeater.DataBind();
        }

        //int maxRowCount = 3;
        //if (!WebUtils.IsTrialVersion(Session))
        //{
        //    if (WebUtils.productLicense(Session) != null)
        //        maxRowCount = WebUtils.productLicense(Session).NumOfUsers;
        //}
        //if (view.ToTable().Rows.Count >= maxRowCount)
        //    toolBar.NewButton_Visible = false;
        //else
        //    toolBar.NewButton_Visible = toolBar.DeleteButton_Visible;

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
        WebFormUtils.LoadKeys(db, row, cb);
        cb.Visible = IsAllowEdit;
    }
    protected void Delete_Click(object sender, EventArgs e)
    {
        ArrayList list = WebUtils.SelectedRepeaterItemToBaseObjectList(db, Repeater, "ItemSelect");

        foreach (EUserGroup o in list)
        {
            if (EUserGroup.db.select(dbConn, o))
            {

                DBFilter userGroupFilter = new DBFilter();
                userGroupFilter.add(new Match("UserGroupID", o.UserGroupID));
                EUserGroupAccess.db.delete(dbConn, userGroupFilter);
                EUserGroupFunction.db.delete(dbConn, userGroupFilter);

                WebUtils.StartFunction(Session, FUNCTION_CODE);
                db.delete(dbConn, o);
                WebUtils.EndFunction(dbConn);
            }
        }
        loadData(info, db, Repeater);
    }

    public void New_Click(object sender, EventArgs e)
    {
        HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "UserGroup_Edit.aspx");
    }
}
