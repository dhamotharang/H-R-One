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
//using perspectivemind.validation;
using HROne.Payroll;

public partial class Payroll_CostAllocation_Confirm : HROneWebPage
{
    private const string FUNCTION_CODE = "CST003";

    public Binding binding;

    protected SearchBinding sbinding;
    public DBManager db = ECostAllocation.db;
    public EPayrollGroup obj;
    public int CurID = -1;
    public int CurPayPeriodID = -1;

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
            ConfirmPayrollSelectAllPanel.Visible = false;
        }
        binding = new Binding(dbConn, db);
        //binding.add(new DropDownVLBinder(EPayrollGroup.db, PayGroupID, EPayrollGroup.VLPayrollGroup));
        binding.add(CurrentPayPeriodID);

        DBFilter payPeriodFilter = new DBFilter();
        if (!int.TryParse(PayGroupID.SelectedValue, out CurID))
            if (!int.TryParse(DecryptedRequest["PayGroupID"], out CurID))
                CurID = -1;
        payPeriodFilter.add(new Match("PayGroupID", CurID));
        payPeriodFilter.add("PayPeriodFr", false);
        DBFilter empPayrollFilter = new DBFilter();
        empPayrollFilter.add(new Match("EmpPayStatus", "C"));
        empPayrollFilter.add(new IN("EmpPayrollID", "Select Distinct EmpPayrollID from " + ECostAllocation.db.dbclass.tableName, new DBFilter()));

        payPeriodFilter.add(new IN("PayPeriodID", "Select Distinct PayPeriodID from " + EEmpPayroll.db.dbclass.tableName, empPayrollFilter));

        binding.add(new DropDownVLBinder(EPayrollPeriod.db, PayPeriodID, EPayrollPeriod.VLPayrollPeriod, payPeriodFilter));

        binding.init(Request, Session);

        sbinding = new SearchBinding(dbConn, EEmpPersonalInfo.db);

        if (!int.TryParse(PayPeriodID.SelectedValue, out CurPayPeriodID))
        {
            if (!int.TryParse(DecryptedRequest["PayPeriodID"], out CurPayPeriodID))
            {
                EPayrollGroup obj = new EPayrollGroup();
                obj.PayGroupID = CurID;
                if (EPayrollGroup.db.select(dbConn, obj))
                    CurPayPeriodID = obj.CurrentPayPeriodID;
                else
                    CurPayPeriodID = -1;
            }
            initPayrollGroup();
        }
        
        info = ListFooter.ListInfo;

        HROne.Common.WebUtility.WebControlsLocalization(this, this.Controls);
    }

    protected void initPayrollGroup()
    {
        DBFilter m_filter = new DBFilter();
        DBFilter m_userFilter = new DBFilter();

        PayGroupID.Items.Add(new ListItem("Not Selected", "-1"));

        m_userFilter.add(new Match("UserID", WebUtils.GetCurUser(Session).UserID));

        OR m_or = new OR();

        m_or.add(new IN("PayGroupID", "SELECT PayGroupID FROM PayrollGroupUsers ", m_userFilter));
        m_or.add(new Match("PayGroupIsPublic", true));

        m_filter.add(m_or);
        m_filter.add("PayGroupCode", true);

        // since sorting is not feasible directly using DBFilter (because of the encrypted data), we use a local data table as a temp buffer
        DataTable m_localTable = new DataTable();
        m_localTable.Columns.Add("PayGroupCode", typeof(string));
        m_localTable.Columns.Add("PayGroupDesc", typeof(string));
        m_localTable.Columns.Add("PayGroupID", typeof(int));

        foreach (EPayrollGroup o in EPayrollGroup.db.select(dbConn, m_filter))
        {
            DataRow m_row = m_localTable.NewRow();
            m_row["PayGroupCode"] = o.PayGroupCode;
            m_row["PayGroupDesc"] = o.PayGroupDesc;
            m_row["PayGroupID"] = o.PayGroupID;
            m_localTable.Rows.Add(m_row);
        }

        foreach (DataRow m_o in m_localTable.Select("", "payGroupCode"))
        {
            PayGroupID.Items.Add(new ListItem(m_o["PayGroupCode"].ToString() + " - " + m_o["PayGroupDesc"].ToString(), m_o["PayGroupID"].ToString()));
        }
    }

    protected void Page_PreRender(object sender, EventArgs e)
    {

        if (!Page.IsPostBack)
        {
            loadObject();
            if (CurID > 0)
            {
                panelPayPeriod.Visible = true;
                view = loadData(info, db, Repeater);
            }
            else
                panelPayPeriod.Visible = false;
        }
    }

    protected bool loadObject()
    {
        obj = new EPayrollGroup();
        obj.PayGroupID = CurID;
        EPayrollGroup.db.select(dbConn, obj);
        //if (!EPayrollGroup.db.select(dbConn, obj))
        //return false;

        Hashtable values = new Hashtable();
        EPayrollGroup.db.populate(obj, values);
        binding.toControl(values);

        ListItem selectedPayPeriod = PayPeriodID.Items.FindByValue(CurPayPeriodID.ToString());
        if (selectedPayPeriod != null)
            PayPeriodID.SelectedValue = selectedPayPeriod.Value;
        else
        {
            CurPayPeriodID = obj.CurrentPayPeriodID;
            selectedPayPeriod = PayPeriodID.Items.FindByValue(CurPayPeriodID.ToString());
            if (selectedPayPeriod != null)
                PayPeriodID.SelectedValue = selectedPayPeriod.Value;
            else
                CurPayPeriodID = 0;
        }

        ucPayroll_PeriodInfo.CurrentPayPeriodID = CurPayPeriodID;
        if (CurPayPeriodID > 0)
        {
            panelPayPeriod.Visible = true;
            panelCostAllocationAdjustmentDetail.Visible = true;
        }
        else
        {
            panelPayPeriod.Visible = false;
            panelCostAllocationAdjustmentDetail.Visible = false;
        }
        
        return true;
    }

    protected void PayGroupID_SelectedIndexChanged(object sender, EventArgs e)
    {
        loadObject();
        view = loadData(info, db, Repeater);
    }
    protected void PayPeriodID_SelectedIndexChanged(object sender, EventArgs e)
    {
        loadObject();
        view = loadData(info, db, Repeater);
    }

    public DataView loadData(ListInfo info, DBManager db, Repeater repeater)
    {
        DBFilter filter = sbinding.createFilter();

        //if (info != null && info.orderby != null && !info.orderby.Equals(""))
        //    filter.add(info.orderby, info.order);

        string select = " e.*, ca.CostAllocationID ";
        string from = "from " + EEmpPersonalInfo.db.dbclass.tableName + " e, " + ECostAllocation.db.dbclass.tableName + " ca";

        filter.add(new MatchField("e.EmpID", "ca.EmpID"));
        filter.add(new Match("ca.CostAllocationStatus", "T"));

        DBFilter empPayrollFilter = new DBFilter();
        empPayrollFilter.add(new Match("ep.EmpPayStatus", "C"));
        empPayrollFilter.add(new Match("ep.PayPeriodID", CurPayPeriodID));

        // Start 0000185, KuangWei, 2015-04-21
        //filter.add(WebUtils.AddRankFilter(Session, "e.EmpID", true));
        // End 0000185, KuangWei, 2015-04-21

        DBFilter inCostAllocationFilter = new DBFilter();
        filter.add(new IN("ca.EmpPayrollID", "Select EmpPayrollID from " + EEmpPayroll.db.dbclass.tableName + " ep", empPayrollFilter));

        // Start 0000185, KuangWei, 2015-04-21
        DBFilter empInfoFilter = EmployeeSearchControl1.GetEmpInfoFilter(AppUtils.ServerDateTime(), AppUtils.ServerDateTime());
        empInfoFilter.add(new MatchField("e.EmpID", "ee.EmpID"));
        filter.add(new Exists(EEmpPersonalInfo.db.dbclass.tableName + " ee", empInfoFilter));

        DataTable table = WebUtils.GetDataTableFromSelectQueryWithFilter(dbConn, select, from, filter, info);
        table = EmployeeSearchControl1.FilterEncryptedEmpInfoField(table, info);
        // End 0000185, KuangWei, 2015-04-21


        view = new DataView(table);

        ListFooter.Refresh();

        if (repeater != null)
        {
            repeater.DataSource = view;
            repeater.DataBind();
        }

        return view;
    }
    protected void FirstPage_Click(object sender, EventArgs e)
    {
        view = loadData(info, EEmpPayroll.db, Repeater);
    }
    protected void PrevPage_Click(object sender, EventArgs e)
    {
        view = loadData(info, EEmpPayroll.db, Repeater);
    }
    protected void NextPage_Click(object sender, EventArgs e)
    {
        view = loadData(info, EEmpPayroll.db, Repeater);
    }
    protected void LastPage_Click(object sender, EventArgs e)
    {
        view = loadData(info, EEmpPayroll.db, Repeater);
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

        view = loadData(info, EEmpPayroll.db, Repeater);

    }
    protected void Repeater_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        DataRowView row = (DataRowView)e.Item.DataItem;
        CheckBox cb = (CheckBox)e.Item.FindControl("ItemSelect");
        WebFormUtils.LoadKeys(db, row, cb);
        cb.Visible = IsAllowEdit;
        if (cb.Visible == true)
            cb.Checked = true;
    }

    protected void btnConfirm_Click(object sender, EventArgs e)
    {
        ArrayList list = WebUtils.SelectedRepeaterItemToBaseObjectList(ECostAllocation.db, Repeater, "ItemSelect");

        CostAllocationProcess costAllocationProcess = new CostAllocationProcess(dbConn);

        foreach (ECostAllocation obj in list)
        {
            ECostAllocation.db.select(dbConn, obj);
            WebUtils.StartFunction(Session, FUNCTION_CODE, obj.EmpID, true);

            costAllocationProcess.Confirm(obj.CostAllocationID, WebUtils.GetCurUser(Session));
            WebUtils.EndFunction(dbConn);
        }
        PageErrors.getErrors(db, Page).addError("Complete");

        view = loadData(info, EEmpPayroll.db, Repeater);

    }

    // Start 0000185, KuangWei, 2015-04-21
    protected void Search_Click(object sender, EventArgs e)
    {
        view = loadData(info, EEmpPayroll.db, Repeater);
    }

    protected void Reset_Click(object sender, EventArgs e)
    {
        sbinding.clear();
        EmployeeSearchControl1.Reset();
        EmployeeSearchControl1.EmpStatusValue = "A";
        view = loadData(info, EEmpPayroll.db, Repeater);
    }
    // End 0000185, KuangWei, 2015-04-21   
}
