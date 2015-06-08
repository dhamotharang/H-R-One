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
using HROne.Translation;
using HROne.Lib.Entities;

public partial class Payroll_Group_List : HROneWebPage
{
    private const string FUNCTION_CODE = "PAY001";

    protected DBManager db = EPayrollGroup.db;
    protected SearchBinding binding;
    protected ListInfo info;
    protected DataView view;
    private bool IsAllowEdit = true;
    

    protected void Page_Load(object sender, EventArgs e)
    {
       
        if (!WebUtils.CheckAccess(Response, Session, FUNCTION_CODE, WebUtils.AccessLevel.Read))
            return;
        toolBar.FunctionCode = FUNCTION_CODE;

        

        binding = new SearchBinding(dbConn, db);
        binding.add(new LikeSearchBinder(PayGroupCode, "PayGroupCode"));
        binding.add(new LikeSearchBinder(PayGroupDesc, "PayGroupDesc"));
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
        WebFormUtils.LoadKeys(db, row, cb);
        cb.Visible = IsAllowEdit;
        e.Item.FindControl("ItemSelect").Visible = toolBar.DeleteButton_Visible;
    }

    protected void Copy_Click(object sender, EventArgs e)
    {

        ArrayList list = WebUtils.SelectedRepeaterItemToBaseObjectList(db, Repeater, "ItemSelect");

        foreach (EPayrollGroup o in list)
        {
            if (EPayrollGroup.db.select(dbConn, o))
            {
                EPayrollGroup newPayrollGroup = o.Copy(dbConn);
                EPayrollPeriod oldCurrentPayrollPeriod = new EPayrollPeriod();
                oldCurrentPayrollPeriod.PayPeriodID = o.CurrentPayPeriodID;
                if (EPayrollPeriod.db.select(dbConn, oldCurrentPayrollPeriod))
                {
                    EPayrollPeriod newPayPeriod = oldCurrentPayrollPeriod.Copy(dbConn, newPayrollGroup.PayGroupID);
                    newPayrollGroup.CurrentPayPeriodID = newPayPeriod.PayPeriodID;
                    EPayrollGroup.db.update(dbConn, newPayrollGroup);

                }
                // Start 0000069, Ricky So, 2014-08-08
                DBFilter m_filter = new DBFilter();
                m_filter.add(new Match("PayGroupID", o.PayGroupID));
                foreach (EPayrollGroupUsers users in EPayrollGroupUsers.db.select(dbConn, m_filter))
                {
                    EPayrollGroupUsers new_users = new EPayrollGroupUsers();
                    new_users.PayGroupID = newPayrollGroup.PayGroupID;
                    new_users.UserID = users.UserID;

                    EPayrollGroupUsers.db.insert(dbConn, new_users);
                }
                // End 0000069, Ricky So, 2014-08-08
            }
        }
        loadData(info, db, Repeater);
    }

    protected void Delete_Click(object sender, EventArgs e)
    {
        PageErrors errors = PageErrors.getErrors(db, Page.Master);
        errors.clear();

        ArrayList list = WebUtils.SelectedRepeaterItemToBaseObjectList(db, Repeater, "ItemSelect");

        foreach (EPayrollGroup o in list)
        {
            if (EPayrollGroup.db.select(dbConn, o))
            {

                DBFilter payPeriodFilter = new DBFilter();
                payPeriodFilter.add(new Match("PayGroupID", o.PayGroupID));
                DBFilter empPayrollFilter = new DBFilter();

                empPayrollFilter.add(new IN("PayPeriodID", "Select PayPeriodID from PayrollPeriod", payPeriodFilter));

                ArrayList empPayrollList = EEmpPayroll.db.select(dbConn, empPayrollFilter);
                if (empPayrollList.Count == 0)
                {
                    DBFilter empPosFilter = new DBFilter();
                    empPosFilter.add(new Match("PayGroupID", o.PayGroupID));
                    empPosFilter.add("empid", true);
                    ArrayList empPosList = EEmpPositionInfo.db.select(dbConn, empPosFilter);
                    if (empPosList.Count > 0)
                    {
                        errors.addError(string.Format(HROne.Translation.PageErrorMessage.ERROR_CODE_USED_BY_EMPLOYEE, new string[] { HROne.Common.WebUtility.GetLocalizedString("Payroll Group"), o.PayGroupCode }));
                        foreach (EEmpPositionInfo empPos in empPosList)
                        {
                            EEmpPersonalInfo empInfo = new EEmpPersonalInfo();
                            empInfo.EmpID = empPos.EmpID;
                            if (EEmpPersonalInfo.db.select(dbConn, empInfo))
                                errors.addError("- " + empInfo.EmpNo + ", " + empInfo.EmpEngFullName);
                            else
                                EEmpPositionInfo.db.delete(dbConn, empPos);

                        }
                        errors.addError(HROne.Translation.PageErrorMessage.ERROR_ACTION_ABORT);

                    }
                    else
                    {
                        EPayrollPeriod.db.delete(dbConn, payPeriodFilter);
                        WebUtils.StartFunction(Session, FUNCTION_CODE);
                        EPayrollGroup.db.delete(dbConn, o);
                        WebUtils.EndFunction(dbConn);
                    }
                }
                else
                {
                    EPayrollGroup.db.select(dbConn, o);
                    errors.addError("Payroll Code '" + o.PayGroupCode + "' is in use. Action abort!");
                }
            }
        }
        loadData(info, db, Repeater);
    }
    public void New_Click(object sender, EventArgs e)
    {
        HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "Payroll_Group_Edit.aspx");
    }
}
