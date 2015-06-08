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
using HROne.Lib.Entities;

public partial class User_View : HROneWebPage
{
    private const string FUNCTION_CODE = "SEC001";
    
    public Binding binding;
    public SearchBinding userGroupBinding;
    public SearchBinding rankBinding;
    public SearchBinding companyBinding;
    public DBManager db = EUser.db;
    public EUser obj;
    public int CurID = -1;
    public Hashtable CurUserGroups = new Hashtable();
    public Hashtable CurRanks = new Hashtable();
    public Hashtable CurCompanies = new Hashtable();
    // Start 0000069, KuangWei, 2014-08-26
    public Hashtable CurPayGroups = new Hashtable();
    public SearchBinding payGroupBinding;
    // End 0000069, KuangWei, 2014-08-26
    

    protected void Page_Load(object sender, EventArgs e)
    {
       
        if (!WebUtils.CheckAccess(Response, Session, FUNCTION_CODE, WebUtils.AccessLevel.Read))
            return;
        toolBar.FunctionCode = FUNCTION_CODE;

        binding = new Binding(dbConn, db);
        binding.add(UserID);
        binding.add(LoginID);
        binding.add(UserName);
        binding.add(new LabelVLBinder(db, UserAccountStatus, EUser.VLAccountStatus));
        binding.add(ExpiryDate);
        binding.add(FailCount);        
        binding.add(new LabelVLBinder(db, UserChangePassword, Values.VLTrueFalseYesNo));
        binding.add(UserChangePasswordPeriod);
        binding.add(new LabelVLBinder(db, UserChangePasswordUnit, Values.VLUsrPasswordUnit));
        binding.add(new LabelVLBinder(db, UsersCannotCreateUsersWithMorePermission, Values.VLTrueFalseYesNo));
        binding.init(Request, Session);

        rankBinding = new SearchBinding(dbConn, ERank.db);
        companyBinding = new SearchBinding(dbConn, ECompany.db);
        userGroupBinding = new SearchBinding(dbConn, EUserGroup.db);
        // Start 0000069, KuangWei, 2014-08-26
        payGroupBinding = new SearchBinding(dbConn, EPayrollGroup.db);
        // End 0000069, KuangWei, 2014-08-26

        if (!int.TryParse(DecryptedRequest["UserID"], out CurID))
            CurID = -1; 

        HROne.Common.WebUtility.WebControlsLocalization(this, this.Controls);

        if (!Page.IsPostBack)
        {
            if (CurID > 0)
                loadObject();
            else
                toolBar.DeleteButton_Visible = false;
            if (CurID.Equals(1))
                toolBar.DeleteButton_Visible = false;
            EUser user = WebUtils.GetCurUser(Session);
            if (user.UserID == CurID)
                toolBar.DeleteButton_Visible = false;
            loadUserGroups();
            loadCompanies();
            loadRanks();
            // Start 0000069, KuangWei, 2014-08-26
            loadPayGroups();
            // End 0000069, KuangWei, 2014-08-26
        }
    }
    public void loadUserGroups()
    {
        DBFilter filter = new DBFilter();
        DataTable table = EUserGroup.db.loadDataSet(dbConn, null, filter);
        DataView view = new DataView(table);
        UserGroups.DataSource = view;
        UserGroups.DataBind();
    }
    public void loadCompanies()
    {
        DBFilter filter = new DBFilter();
        DataTable table = ECompany.db.loadDataSet(dbConn, null, filter);
        DataView view = new DataView(table);
        Companies.DataSource = view;
        Companies.DataBind();
    }
    public void loadRanks()
    {
        DBFilter filter = new DBFilter();
        DataTable table = ERank.db.loadDataSet(dbConn, null, filter);
        DataView view = new DataView(table);
        Ranks.DataSource = view;
        Ranks.DataBind();
    }
    // Start 0000069, KuangWei, 2014-08-26
    public void loadPayGroups()
    {
        DBFilter filter = new DBFilter();
        DataTable table = EPayrollGroup.db.loadDataSet(dbConn, null, filter);
        DataView view = new DataView(table);
        PayGroups.DataSource = view;
        PayGroups.DataBind();
    }
    // End 0000069, KuangWei, 2014-08-26
    protected bool loadObject()
    {
        obj = new EUser();
        bool isNew = WebFormWorkers.loadKeys(db, obj, DecryptedRequest);
        if (!db.select(dbConn, obj))
            return false;

        Hashtable values = new Hashtable();
        db.populate(obj, values);
        binding.toControl(values);


        DBFilter filter = new DBFilter();
        filter.add(new Match("UserID", this.CurID));
        ArrayList list;
        list = EUserGroupAccess.db.select(dbConn, filter);


        foreach (EUserGroupAccess o in list)
        {
            CurUserGroups.Add(o.UserGroupID, o);
        }
        list = EUserCompany.db.select(dbConn, filter);
        foreach (EUserCompany o in list)
        {
            CurCompanies.Add(o.CompanyID, o);
        }
        list = EUserRank.db.select(dbConn, filter);
        foreach (EUserRank o in list)
        {
            CurRanks.Add(o.RankID, o);
        }
        // Start 0000069, KuangWei, 2014-08-26
        list = EPayrollGroupUsers.db.select(dbConn, filter);
        foreach (EPayrollGroupUsers o in list)
        {
            CurPayGroups.Add(o.PayGroupID, o);
        }
        // End 0000069, KuangWei, 2014-08-26

        if (WebUtils.GetCurUser(Session).UsersCannotCreateUsersWithMorePermission == false)
        {
            toolBar.EditButton_Visible = WebUtils.CheckPermission(Session, FUNCTION_CODE, WebUtils.AccessLevel.ReadWrite);
            toolBar.DeleteButton_Visible = WebUtils.CheckPermission(Session, FUNCTION_CODE, WebUtils.AccessLevel.ReadWrite);
        }
        else if (obj.UsersCannotCreateUsersWithMorePermission == false)
        {
            toolBar.EditButton_Visible = false;
            toolBar.DeleteButton_Visible = false;
        }
        // Start 0000069, KuangWei, 2014-08-26
        else if (EUser.IsAllowSubmitPermission(dbConn, WebUtils.GetCurUser(Session).UserID, CurCompanies.Keys, CurUserGroups.Keys, CurRanks.Keys, CurPayGroups.Keys))
        // End 0000069, KuangWei, 2014-08-26
        {
            toolBar.EditButton_Visible = WebUtils.CheckPermission(Session, FUNCTION_CODE, WebUtils.AccessLevel.ReadWrite);
            toolBar.DeleteButton_Visible = WebUtils.CheckPermission(Session, FUNCTION_CODE, WebUtils.AccessLevel.ReadWrite);
        }
        else
        {
            toolBar.EditButton_Visible = false;
            toolBar.DeleteButton_Visible = false;
        }
        return true;
    }

    protected void Delete_Click(object sender, EventArgs e)
    {
        EUser activeUser = WebUtils.GetCurUser(Session);

        EUser c = new EUser();
        c.UserID = CurID;
        if (EUser.db.select(dbConn, c))
        {
            bool isAllowDelete = true;
            if (activeUser.UsersCannotCreateUsersWithMorePermission)
            {
                if (!c.UsersCannotCreateUsersWithMorePermission )
                {
                    isAllowDelete = false;
                }
                else if (!c.IsAllowSubmitPermission(dbConn, activeUser.UserID))
                {
                    isAllowDelete = false;
                }
            }
            if (isAllowDelete)
            {
                WebUtils.StartFunction(Session, FUNCTION_CODE);
                c.UserAccountStatus = "D";
                db.update(dbConn, c);
                WebUtils.EndFunction(dbConn);
                EInbox.DeleteAllByUserID(dbConn, c.UserID);
            }
            else
            {
                PageErrors errors = PageErrors.getErrors(db, Page.Master);
                errors.clear();
                errors.addError(HROne.Translation.PageErrorMessage.ERROR_INVALID_PERMISSION);
                return;
            }
        }
        HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "User_List.aspx");
    }
    protected void UserGroups_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {

        HtmlInputCheckBox selected = (HtmlInputCheckBox)e.Item.FindControl("UserGroupSelect");
        int id = (int)((DataRowView)e.Item.DataItem)["UserGroupID"];
        selected.Attributes["id"] = id.ToString(); ;
        if (CurUserGroups.ContainsKey(id))
            selected.Checked = true;
    }
    protected void Companies_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        HtmlInputCheckBox selected = (HtmlInputCheckBox)e.Item.FindControl("CompanySelect");
        int id = (int)((DataRowView)e.Item.DataItem)["CompanyID"];
        selected.Attributes["id"] = id.ToString(); ;

        if (CurCompanies.ContainsKey(id))
            selected.Checked = true;
    }
    protected void Ranks_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        HtmlInputCheckBox selected = (HtmlInputCheckBox)e.Item.FindControl("RankSelect");
        int id = (int)((DataRowView)e.Item.DataItem)["RankID"];
        selected.Attributes["id"] = id.ToString(); ;

        if (CurRanks.ContainsKey(id))
            selected.Checked = true;
    }
    // Start 0000069, KuangWei, 2014-08-26
    protected void PayGroup_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        HtmlInputCheckBox selected = (HtmlInputCheckBox)e.Item.FindControl("PayGroupSelect");
        int id = (int)((DataRowView)e.Item.DataItem)["PayGroupID"];
        selected.Attributes["id"] = id.ToString(); ;

        if (CurPayGroups.ContainsKey(id))
            selected.Checked = true;

    }
    // End 0000069, KuangWei, 2014-08-26
    protected void Edit_Click(object sender, EventArgs e)
    {
        HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "User_Edit.aspx?UserID=" + CurID);
    }

    protected void Back_Click(object sender, EventArgs e)
    {
        HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "User_List.aspx");
    }
}
