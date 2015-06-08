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
using HROne.Translation;

public partial class Payroll_Group_User_Edit : HROneWebPage
{
    private const string FUNCTION_CODE = "PAY001";

    public Binding binding;
    public SearchBinding userBinding;
    public DBManager db = EPayrollGroup.db;
    public EPayrollGroup obj;
    public int CurID = -1;
    public Hashtable CurUsers = new Hashtable();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!WebUtils.CheckAccess(Response, Session, FUNCTION_CODE, WebUtils.AccessLevel.ReadWrite))
            return;
        toolBar.FunctionCode = FUNCTION_CODE;

        binding = new Binding(dbConn, db);
        binding.add(new LabelVLBinder(db, PayGroupID, EPayrollGroup.VLPayrollGroup));
        //binding.add(new CheckBoxBinder(db, PayGroupIsPublic));
        binding.init(Request, Session);

        userBinding = new SearchBinding(dbConn, EUser.db);

        if (!int.TryParse(DecryptedRequest["PayGroupID"], out CurID))
            CurID = -1;

        HROne.Common.WebUtility.WebControlsLocalization(this, this.Controls);

        if (!Page.IsPostBack)
        {
            if (CurID > 0)
            {
                loadObject();
            }
            else
            {
                if (DecryptedRequest["PayGroupID"] != null)
                {
                    Hashtable values = new Hashtable();
                    values.Add("PayGroupID", DecryptedRequest["PayGroupID"]);
                    binding.toControl(values);
                }
            }

            toolBar.SaveButton_Visible = true;
            toolBar.EditButton_Visible = false;
            toolBar.DeleteButton_Visible = false;

            loadUsers();
        }
    }

    public void loadUsers()
    {
        DBFilter filter = new DBFilter();
        DataTable table = EUser.db.loadDataSet(dbConn, null, filter);
        DataView view = new DataView(table);
        Users.DataSource = view;
        Users.DataBind();
    }

    protected bool loadObject()
    {
        obj = new EPayrollGroup();
        WebFormWorkers.loadKeys(db, obj, DecryptedRequest);

        Hashtable values = new Hashtable();
        db.populate(obj, values);
        binding.toControl(values);

        DBFilter filter = new DBFilter();
        filter.add(new Match("PayGroupID", CurID));
        ArrayList list = EPayrollGroupUsers.db.select(dbConn, filter);
        foreach (EPayrollGroupUsers o in list)
        {
            CurUsers.Add(o.UserID, o);
        }

        return true;
    }

    protected void Save_Click(object sender, EventArgs e)
    {
        EPayrollGroup c = new EPayrollGroup();

        Hashtable values = new Hashtable();
        binding.toValues(values);

        PageErrors errors = PageErrors.getErrors(db, Page.Master);
        errors.clear();

        db.validate(errors, values);

        if (!errors.isEmpty())
            return;

        db.parse(values, c);

        //c.PayGroupID = CurID;

        ArrayList newUserList = WebUtils.SelectedRepeaterItemToBaseObjectList(EUser.db, Users, "UserSelect");

        bool m_isPublic = (newUserList.Count <= 0);

        DBFilter notUser = new DBFilter();
        notUser.add(new Match("PayGroupID", CurID));
        foreach (EUser user in newUserList)
        {
            DBFilter filter = new DBFilter();
            filter.add(new Match("UserID", user.UserID));
            filter.add(new Match("PayGroupID", CurID));
            if (EPayrollGroupUsers.db.count(dbConn, filter) <= 0)
            {
                EPayrollGroupUsers o = new EPayrollGroupUsers();
                o.UserID = user.UserID;
                o.PayGroupID = CurID;
                EPayrollGroupUsers.db.insert(dbConn, o);
            }
            notUser.add(new Match("UserID", "<>", user.UserID));
        }
        ArrayList unselectedUserList = EPayrollGroupUsers.db.select(dbConn, notUser);
        foreach (EPayrollGroupUsers payrollGroupUsers in unselectedUserList)
            EPayrollGroupUsers.db.delete(dbConn, payrollGroupUsers);

        // update payroll group IsPublic flag
        EPayrollGroup m_group = EPayrollGroup.GetObject(dbConn, CurID);
        if (m_group != null)
        {
            m_group.PayGroupIsPublic = m_isPublic;
            EPayrollGroup.db.update(dbConn, m_group);
        }


        HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "Payroll_Group_View.aspx?PayGroupID=" + CurID);
    }

    protected void Back_Click(object sender, EventArgs e)
    {
        HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "Payroll_Group_View.aspx?PayGroupID=" + CurID);
    }

    protected void Users_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        DataRowView row = (DataRowView)e.Item.DataItem;
        CheckBox cb = (CheckBox)e.Item.FindControl("UserSelect");
        WebFormUtils.LoadKeys(EUser.db, row, cb);

        if (CurUsers.ContainsKey(row["UserID"]))
            cb.Checked = true;
    }
}
