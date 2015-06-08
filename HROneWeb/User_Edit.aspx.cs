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
using HROne.Translation;
public partial class User_Edit : HROneWebPage
{
    private const string FUNCTION_CODE = "SEC001";
    private const string NO_CHANGE_PASSWORD = "%NO_CHANGE_PASSWORD%";
    public Binding binding;
    public SearchBinding userGroupBinding;
    public SearchBinding rankBinding;
    public SearchBinding companyBinding;

    public DBManager db = EUser.db;
    public EUser obj;
    public int CurID = -1;
    public Hashtable CurUserGroups=new Hashtable();
    public Hashtable CurRanks = new Hashtable();
    public Hashtable CurCompanies = new Hashtable();
    // Start 0000069, KuangWei, 2014-08-26
    public Hashtable CurPayGroups = new Hashtable();
    public SearchBinding payGroupBinding;
    // End 0000069, KuangWei, 2014-08-26
    

    protected void Page_Load(object sender, EventArgs e)
    {
       
        if (!WebUtils.CheckAccess(Response, Session, FUNCTION_CODE, WebUtils.AccessLevel.ReadWrite))
            return;
        toolBar.FunctionCode = FUNCTION_CODE;

        

        binding = new Binding(dbConn, db);
        binding.add(UserID);
        binding.add(LoginID);
        binding.add(UserName);
        binding.add(new DropDownVLBinder(db, UserAccountStatus, EUser.VLAccountStatus));
        binding.add(new TextBoxBinder(db, ExpiryDate.TextBox, ExpiryDate.ID));
        binding.add(FailCount); 
        binding.add(new CheckBoxBinder(db,UserChangePassword));
        binding.add(UserChangePasswordPeriod);
        binding.add(new DropDownVLBinder(db, UserChangePasswordUnit, Values.VLUsrPasswordUnit));
        binding.add(new CheckBoxBinder(db, UsersCannotCreateUsersWithMorePermission));
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

		if(!Page.IsPostBack) 
		{
            if (CurID > 0)
            {
                Password.Attributes.Add("value", NO_CHANGE_PASSWORD);
                Password2.Attributes.Add("value", NO_CHANGE_PASSWORD);
            }

            EUser user = WebUtils.GetCurUser(Session);
            if (CurID > 0)
                loadObject();
            else
            {
                toolBar.DeleteButton_Visible = false;
                if (user.UsersCannotCreateUsersWithMorePermission)
                    UsersCannotCreateUsersWithMorePermission.Checked = true;
            }
            if (user.UsersCannotCreateUsersWithMorePermission)
            {
                UsersCannotCreateUsersWithMorePermission.Enabled = false;
            }
            if (CurID.Equals(1))
                toolBar.DeleteButton_Visible = false;
            if (user.UserID == CurID)
            {
                toolBar.DeleteButton_Visible = false;
                UserAccountStatus.Enabled = false;
                ExpiryDate.Enabled = false;
                FailCount.Enabled = false;
            }

            loadUserGroups();
            loadCompanies();
            loadRanks();
            // Start 0000069, KuangWei, 2014-08-26
            loadPayGroups();
            // End 0000069, KuangWei, 2014-08-26
        }

        Password.Attributes.Add("onfocus", "this.select();");
        Password2.Attributes.Add("onfocus", "this.select();");

    }
    public void loadUserGroups()
    {
        DBFilter filter=new DBFilter();
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
	    obj=new EUser();
	    bool isNew=WebFormWorkers.loadKeys(db, obj, DecryptedRequest);
	    if(!db.select(dbConn, obj))
		    return false;

	    Hashtable values=new Hashtable();
	    db.populate(obj,values);
	    binding.toControl(values);


        DBFilter filter = new DBFilter();
        filter.add(new Match("UserID", this.CurID));
        ArrayList list;
        list=EUserGroupAccess.db.select(dbConn, filter);
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

        if (WebUtils.GetCurUser(Session).UserID == obj.UserID)
            UsersCannotCreateUsersWithMorePermission.Enabled = false;
        else
        {
            UsersCannotCreateUsersWithMorePermission.Enabled = true;
        }
        return true;
    }

    protected void Save_Click(object sender, EventArgs e)
    {
        EUser c = new EUser();

        Hashtable values = new Hashtable();
        binding.toValues(values);

        PageErrors errors = PageErrors.getErrors(db, Page.Master);
        errors.clear();


        db.validate(errors, values);

        if (!errors.isEmpty())
            return;

        db.parse(values, c);

        if (Password.Text.Equals(""))
        {
            errors.addError("Password", HROne.Translation.PageErrorMessage.ERROR_FIELD_REQUIRED);
            return;
        }
        if (!Password2.Text.Equals(Password.Text))
        {
            errors.addError("Password", HROne.Translation.PageErrorMessage.ERROR_PASSWORD_NOT_MATCH);
            return;
        }
        if (!Password.Text.Equals(NO_CHANGE_PASSWORD))
            c.UserPassword = HROne.CommonLib.Hash.PasswordHash(Password.Text);
        else
            if (CurID < 0)
                c.UserPassword = HROne.CommonLib.Hash.PasswordHash(string.Empty);

        if (c.UserAccountStatus.Equals("A"))
        {
            if (WebUtils.TotalActiveUser(dbConn, c.UserID) >= WebUtils.productLicense(Session).NumOfUsers)
            {
                errors.addError(string.Format(PageErrorMessage.ERROR_MAX_LICENSE_LIMITCH_REACH, new string[] { WebUtils.productLicense(Session).NumOfUsers + " " + HROne.Common.WebUtility.GetLocalizedString("User") }));
                return;
            }
        }

        ArrayList newCompanyList = WebUtils.SelectedRepeaterItemToBaseObjectList(ECompany.db, Companies, "ItemSelect");
        ArrayList newUserGroupList = WebUtils.SelectedRepeaterItemToBaseObjectList(EUserGroup.db, UserGroups, "ItemSelect");
        ArrayList newRankList = WebUtils.SelectedRepeaterItemToBaseObjectList(ERank.db, Ranks, "ItemSelect");
        // Start 0000069, KuangWei, 2014-08-26
        ArrayList newPayGroupList = WebUtils.SelectedRepeaterItemToBaseObjectList(EPayrollGroup.db, PayGroups, "ItemSelect");
        // End 0000069, KuangWei, 2014-08-26

        if (WebUtils.GetCurUser(Session).UsersCannotCreateUsersWithMorePermission)
        {
            if (c.UsersCannotCreateUsersWithMorePermission == false)
            {
                errors.addError(PageErrorMessage.ERROR_INVALID_PERMISSION);
                return;
            }
            // Start 0000069, KuangWei, 2014-08-26
            else if (!c.IsAllowSubmitPermission(dbConn, WebUtils.GetCurUser(Session).UserID)
                || !EUser.IsAllowSubmitPermission(dbConn, WebUtils.GetCurUser(Session).UserID, newCompanyList, newUserGroupList, newRankList, newPayGroupList)
                // End 0000069, KuangWei, 2014-08-26
           )
            {
                errors.addError(PageErrorMessage.ERROR_INVALID_PERMISSION);
                return;
            }
            //  do nothing
        }


        WebUtils.StartFunction(Session, FUNCTION_CODE);
        if (CurID < 0)
        {
            //            Utils.MarkCreate(Session, c);
            c.UserChangePasswordDate = AppUtils.ServerDateTime();
            db.insert(dbConn, c);
            CurID = c.UserID;
            //            url = Utils.BuildURL(-1, CurID);
        }
        else
        {
            //            Utils.Mark(Session, c);
            db.update(dbConn, c);
        }
        {
            DBFilter notUserGroupAccess = new DBFilter();
            notUserGroupAccess.add(new Match("UserID", c.UserID));
            foreach (EUserGroup userGroup in newUserGroupList)
            {
                DBFilter filter = new DBFilter();
                filter.add(new Match("UserID", c.UserID));
                filter.add(new Match("UserGroupID", userGroup.UserGroupID));
                if (EUserGroupAccess.db.count(dbConn, filter) <= 0)
                {
                    EUserGroupAccess o = new EUserGroupAccess();
                    o.UserID = c.UserID;
                    o.UserGroupID = userGroup.UserGroupID;
                    EUserGroupAccess.db.insert(dbConn, o);
                }
                notUserGroupAccess.add(new Match("UserGroupID", "<>", userGroup.UserGroupID));
            }
            ArrayList unselectedUserGroupAccessList = EUserGroupAccess.db.select(dbConn, notUserGroupAccess);
            foreach (EUserGroupAccess userGroupAccess in unselectedUserGroupAccessList)
                EUserGroupAccess.db.delete(dbConn, userGroupAccess);
        }
        {
            DBFilter notUserCompany = new DBFilter();
            notUserCompany.add(new Match("UserID", c.UserID));
            foreach (ECompany company in newCompanyList)
            {
                DBFilter filter = new DBFilter();
                filter.add(new Match("UserID", c.UserID));
                filter.add(new Match("CompanyID", company.CompanyID));
                if (EUserCompany.db.count(dbConn, filter) <= 0)
                {
                    EUserCompany o = new EUserCompany();
                    o.UserID = c.UserID;
                    o.CompanyID = company.CompanyID;
                    EUserCompany.db.insert(dbConn, o);
                }
                notUserCompany.add(new Match("CompanyID", "<>", company.CompanyID));
            }
            ArrayList unselectedUserCompanyList = EUserCompany.db.select(dbConn, notUserCompany);
            foreach (EUserCompany userCompany in unselectedUserCompanyList)
                EUserCompany.db.delete(dbConn, userCompany);
        }
        {
            DBFilter notUserRank = new DBFilter();
            notUserRank.add(new Match("UserID", c.UserID));
            foreach (ERank rank in newRankList)
            {
                DBFilter filter = new DBFilter();
                filter.add(new Match("UserID", c.UserID));
                filter.add(new Match("RankID", rank.RankID));
                if (EUserRank.db.count(dbConn, filter) <= 0)
                {
                    EUserRank o = new EUserRank();
                    o.UserID = c.UserID;
                    o.RankID = rank.RankID;
                    EUserRank.db.insert(dbConn, o);
                }
                notUserRank.add(new Match("RankID", "<>", rank.RankID));
            }
            ArrayList unselectedUserRankList = EUserRank.db.select(dbConn, notUserRank);
            foreach (EUserRank userRank in unselectedUserRankList)
                EUserRank.db.delete(dbConn, userRank);
        }
        // Start 0000069, KuangWei, 2014-08-26
        {
            DBFilter notPayrollGroupUser = new DBFilter();
            notPayrollGroupUser.add(new Match("UserID", c.UserID));
            foreach (EPayrollGroup payrollGroup in newPayGroupList)
            {
                DBFilter filter = new DBFilter();
                filter.add(new Match("UserID", c.UserID));
                filter.add(new Match("PayGroupID", payrollGroup.PayGroupID));
                if (EPayrollGroupUsers.db.count(dbConn, filter) <= 0)
                {
                    EPayrollGroupUsers o = new EPayrollGroupUsers();
                    o.UserID = c.UserID;
                    o.PayGroupID = payrollGroup.PayGroupID;
                    EPayrollGroupUsers.db.insert(dbConn, o);
                }
                notPayrollGroupUser.add(new Match("PayGroupID", "<>", payrollGroup.PayGroupID));
            }
            ArrayList unselectedPayGroupUserList = EPayrollGroupUsers.db.select(dbConn, notPayrollGroupUser);
            foreach (EPayrollGroupUsers payrollGroupUsers in unselectedPayGroupUserList)
                EPayrollGroupUsers.db.delete(dbConn, payrollGroupUsers);
        }

        // update paygroup isPublic flag
        foreach (EPayrollGroup payrollGroup in EPayrollGroup.db.select(dbConn, new DBFilter()))
        {
            DBFilter m_countFilter = new DBFilter();
            m_countFilter.add(new Match("PayGroupID", payrollGroup.PayGroupID));

            payrollGroup.PayGroupIsPublic = (EPayrollGroupUsers.db.count(dbConn, m_countFilter) <= 0);

            EPayrollGroup.db.update(dbConn, payrollGroup);
        }

        // End 0000069, KuangWei, 2014-08-26
        WebUtils.EndFunction(dbConn);

        HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "~/User_View.aspx?UserID=" + CurID);


    }

    protected void Delete_Click(object sender, EventArgs e)
    {
        EUser activeUser = WebUtils.GetCurUser(Session);

        EUser c = new EUser();
        c.UserID= CurID;
        if (EUser.db.select(dbConn, c))
        {
            bool isAllowDelete = true;
            if (activeUser.UsersCannotCreateUsersWithMorePermission)
            {
                if (!c.UsersCannotCreateUsersWithMorePermission)
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

                errors.addError(PageErrorMessage.ERROR_INVALID_PERMISSION);
                return;
            }
        }
        HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "User_List.aspx");
    }

    protected void UserGroups_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        DataRowView row = (DataRowView)e.Item.DataItem;
        CheckBox cb = (CheckBox)e.Item.FindControl("ItemSelect");
        WebFormUtils.LoadKeys(EUserGroup.db, row, cb);


        if (CurUserGroups.ContainsKey(row["UserGroupID"]))
            cb.Checked = true;
        EUser user = WebUtils.GetCurUser(Session);
        if (user.UserID == CurID)
            cb.Enabled = false;

    }
    protected void Companies_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        DataRowView row = (DataRowView)e.Item.DataItem;
        CheckBox cb = (CheckBox)e.Item.FindControl("ItemSelect");
        WebFormUtils.LoadKeys(ECompany.db, row, cb);


        if (CurCompanies.ContainsKey(row["CompanyID"]))
            cb.Checked = true;

    }
    protected void Ranks_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        DataRowView row = (DataRowView)e.Item.DataItem;
        CheckBox cb = (CheckBox)e.Item.FindControl("ItemSelect");
        WebFormUtils.LoadKeys(ERank.db, row, cb);


        if (CurRanks.ContainsKey(row["RankID"]))
            cb.Checked = true;


    }

    // Start 0000069, KuangWei, 2014-08-26
    protected void PayGroup_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        DataRowView row = (DataRowView)e.Item.DataItem;
        CheckBox cb = (CheckBox)e.Item.FindControl("ItemSelect");
        WebFormUtils.LoadKeys(EPayrollGroup.db, row, cb);

        if (CurPayGroups.ContainsKey(row["PayGroupID"]))
            cb.Checked = true;
    }
    // End 0000069, KuangWei, 2014-08-26

    protected void Back_Click(object sender, EventArgs e)
    {
        if (CurID > 0)
            HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "User_View.aspx?UserID=" + CurID);
        else
            HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "User_List.aspx");

    }
}
