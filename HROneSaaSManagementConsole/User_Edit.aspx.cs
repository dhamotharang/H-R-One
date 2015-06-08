using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using HROne.DataAccess;
using HROne.SaaS.Entities;

public partial class User_Edit : HROneWebPage
{
    private const string FUNCTION_CODE = "ADM003";
    private const string NO_CHANGE_PASSWORD = "%NO_CHANGE_PASSWORD%";
    public Binding binding;
    public SearchBinding systemFunctionBinding;

    public DBManager db = EUser.db;
    public EUser obj;
    public int CurID = -1;
    public Dictionary<int, EUserFunction> CurSystemFunction = new Dictionary<int, EUserFunction>();


    protected void Page_Load(object sender, EventArgs e)
    {

        if (!WebUtils.CheckAccess(Response, Session, FUNCTION_CODE))
            return;
        //toolBar.FunctionCode = FUNCTION_CODE;


        binding = new Binding(dbConn, db);
        binding.add(UserID);
        binding.add(LoginID);
        binding.add(UserName);
        binding.add(new DropDownVLBinder(db, UserAccountStatus, EUser.VLAccountStatus));
        binding.add(new TextBoxBinder(db, ExpiryDate.TextBox, ExpiryDate.ID));
        binding.add(FailCount); 
        binding.add(new CheckBoxBinder(db,UserChangePassword));
        binding.add(UserChangePasswordPeriod);
        binding.add(new DropDownVLBinder(db, UserChangePasswordUnit, HROne.Lib.Entities.Values.VLUsrPasswordUnit));
        binding.add(new CheckBoxBinder(db, UsersCannotCreateUsersWithMorePermission));
        binding.init(Request, Session);

        systemFunctionBinding = new SearchBinding(dbConn, ESystemFunction.db);

        if (!int.TryParse(DecryptedRequest["UserID"], out CurID))
            CurID = -1; 

        HROne.Common.WebUtility.WebControlsLocalization(this, this.Controls);

		if(!Page.IsPostBack) 
		{
            Password.Attributes.Add("value", NO_CHANGE_PASSWORD);
            Password2.Attributes.Add("value", NO_CHANGE_PASSWORD);
            EUser user = WebUtils.GetCurUser(Session);
            if (CurID > 0)
                loadObject();
            else
            {
                toolBar.DeleteButton_Visible = false;
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
            loadSystemFunction();

        }

        Password.Attributes.Add("onfocus", "this.select();");
        Password2.Attributes.Add("onfocus", "this.select();");

    }

    public void loadSystemFunction()
    {
        DBFilter filter = new DBFilter();
        DataTable table = ESystemFunction.db.loadDataSet(dbConn, null, filter);
        DataView view = new DataView(table);
        SystemFunctionRepeater.DataSource = view;
        SystemFunctionRepeater.DataBind();
    }

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
        list = EUserFunction.db.select(dbConn, filter);
        foreach (EUserFunction o in list)
        {
            CurSystemFunction.Add(o.FunctionID, o);
        }

        if (WebUtils.GetCurUser(Session).UserID == obj.UserID)
            UsersCannotCreateUsersWithMorePermission.Enabled = false;
        else
        {
            UsersCannotCreateUsersWithMorePermission.Enabled = true;
        }
        return true;
    }

    protected void SystemFunctionRepeater_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        DataRowView row = (DataRowView)e.Item.DataItem;
        CheckBox cb = (CheckBox)e.Item.FindControl("ItemSelect");
        WebFormUtils.LoadKeys(ESystemFunction.db, row, cb);


        if (CurSystemFunction.ContainsKey((int)row["FunctionID"]))
            cb.Checked = true;
        EUser user = WebUtils.GetCurUser(Session);
        if (user.UserID == CurID)
            cb.Enabled = false;

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

        if (!Password2.Text.Equals(Password.Text))
        {
            errors.addError("Password", "Confirm Password does not match");
            return;
        }
        if (!Password.Text.Equals(NO_CHANGE_PASSWORD))
            c.UserPassword = HROne.CommonLib.Hash.PasswordHash(Password.Text);
        else
            if (CurID < 0)
                c.UserPassword = HROne.CommonLib.Hash.PasswordHash(string.Empty);

        ArrayList newSystemFunctionList = WebUtils.SelectedRepeaterItemToBaseObjectList(ESystemFunction.db, SystemFunctionRepeater, "ItemSelect");

        //WebUtils.StartFunction(Session, FUNCTION_CODE);
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

        //WebUtils.EndFunction(dbConn);

        {
            DBFilter notSystemFunction = new DBFilter();
            notSystemFunction.add(new Match("UserID", c.UserID));
            foreach (ESystemFunction function in newSystemFunctionList)
            {
                DBFilter filter = new DBFilter();
                filter.add(new Match("UserID", c.UserID));
                filter.add(new Match("FunctionID", function.FunctionID));
                if (EUserFunction.db.count(dbConn, filter) <= 0)
                {
                    EUserFunction o = new EUserFunction();
                    o.UserID = c.UserID;
                    o.FunctionID = function.FunctionID;
                    EUserFunction.db.insert(dbConn, o);
                }
                notSystemFunction.add(new Match("FunctionID", "<>", function.FunctionID));
            }
            ArrayList unselectedUserFunctionList = EUserFunction.db.select(dbConn, notSystemFunction);
            foreach (EUserFunction userFunction in unselectedUserFunctionList)
                EUserFunction.db.delete(dbConn, userFunction);
        }
        //HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "~/User_View.aspx?UserID=" + CurID);
        HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "User_List.aspx");


    }

    protected void Delete_Click(object sender, EventArgs e)
    {
        EUser activeUser = WebUtils.GetCurUser(Session);

        EUser c = new EUser();
        c.UserID= CurID;
        if (EUser.db.select(dbConn, c))
        {
            bool isAllowDelete = true;

            if (isAllowDelete)
            {
                //WebUtils.StartFunction(Session, FUNCTION_CODE);
                c.UserAccountStatus = "D";
                db.update(dbConn, c);
                //WebUtils.EndFunction(dbConn);
            }
            else
            {
                PageErrors errors = PageErrors.getErrors(db, Page.Master);
                errors.clear();

                errors.addError("Invalid Permission");
                return;
            }
        }
        HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "User_List.aspx");
    }

    protected void Back_Click(object sender, EventArgs e)
    {
        //if (CurID > 0)
        //    HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "User_View.aspx?UserID=" + CurID);
        //else
            HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "User_List.aspx");

    }
}
