using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using HROne.DataAccess;
//using perspectivemind.validation;
using HROne.Lib.Entities;
using HROne.Payroll;

public partial class Payroll_CostAllocation_TrialRun : HROneWebPage
{
    private const string FUNCTION_CODE = "CST001";

    public Binding binding;

    protected SearchBinding sbinding;
    public DBManager db = EPayrollGroup.db;
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
            btnTrialRun.Visible = false;
            ConfirmPayrollSelectAllPanel.Visible = false;
        } 
        binding = new Binding(dbConn, db);
        // Start 0000069, KuangWei, 2014-08-26
        //binding.add(new DropDownVLBinder(db, PayGroupID, EPayrollGroup.VLPayrollGroup));
        initPayrollGroup();
        // End 0000069, KuangWei, 2014-08-26
        binding.add(CurrentPayPeriodID);

        //***** Start 2014/04/11, Ricky So, 0000019

        //DBFilter payPeriodFilter = new DBFilter();
        //if (!int.TryParse(PayGroupID.SelectedValue, out CurID))
        //    if (!int.TryParse(DecryptedRequest["PayGroupID"], out CurID))
        //        CurID = -1;
        //payPeriodFilter.add(new Match("PayGroupID", CurID));

        //payPeriodFilter.add("PayPeriodFr", false);
        //DBFilter empPayrollFilter = new DBFilter();
        //empPayrollFilter.add(new Match("EmpPayStatus", "C"));
        //empPayrollFilter.add(new IN("not EmpPayrollID", "Select Distinct EmpPayrollID from " + ECostAllocation.db.dbclass.tableName, new DBFilter()));

        //payPeriodFilter.add(new IN("PayPeriodID", "Select Distinct PayPeriodID from " + EEmpPayroll.db.dbclass.tableName , empPayrollFilter));

        //binding.add(new DropDownVLBinder(EPayrollPeriod.db, PayPeriodID, EPayrollPeriod.VLPayrollPeriod, payPeriodFilter));
        //***** End 2014/04/11, Ricky So, 0000019

        binding.init(Request, Session);

        sbinding = new SearchBinding(dbConn, EEmpPersonalInfo.db);

        if (!int.TryParse(PayPeriodID.SelectedValue, out CurPayPeriodID))
            if (!int.TryParse(DecryptedRequest["PayPeriodID"], out CurPayPeriodID))
            {
                EPayrollGroup obj = new EPayrollGroup();
                obj.PayGroupID = CurID;
                if (EPayrollGroup.db.select(dbConn, obj))
                    CurPayPeriodID = obj.CurrentPayPeriodID;
                else
                    CurPayPeriodID = -1;
            }

        HROne.Common.WebUtility.WebControlsLocalization(this, this.Controls);

        info = ListFooter.ListInfo;
        info.recordPerPage = 0;

        if (!Page.IsPostBack)
        {
            loadObject();
            if (CurID > 0)
            {
                panelPayPeriod.Visible = true;
                view = loadData(info, EEmpPayroll.db, Repeater);
            }
            else
                panelPayPeriod.Visible = false;

            PayPeriodID.Items.Clear();
            PayPeriodID.Items.Add(new ListItem("Not Selected", "-1"));

        }

    }

    // Start 0000069, KuangWei, 2014-09-03
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
    // End 0000069, KuangWei, 2014-09-03

    protected bool loadObject()
    {
        obj = new EPayrollGroup();
        obj.PayGroupID = CurID;
        db.select(dbConn, obj);
        //if (!db.select(dbConn, obj))
        //return false;

        Hashtable values = new Hashtable();
        db.populate(obj, values);
        binding.toControl(values);

        ListItem selectedPayPeriod = PayPeriodID.Items.FindByValue(CurPayPeriodID.ToString());
        if (selectedPayPeriod != null)
            PayPeriodID.SelectedValue=selectedPayPeriod.Value;
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
            panelConfirmDetail.Visible = true;
        }
        else
        {
            panelPayPeriod.Visible = false;
            panelConfirmDetail.Visible = false;
        }
        
        return true;
    }

    //***** Start 2014/04/11, Ricky So, 0000019
    protected void reloadPayrollPeriod()
    {
        DBFilter payPeriodFilter = new DBFilter();
        if (!int.TryParse(PayGroupID.SelectedValue, out CurID))
            if (!int.TryParse(DecryptedRequest["PayGroupID"], out CurID))
                CurID = -1;

        PayPeriodID.Items.Clear();
        PayPeriodID.Items.Add(new ListItem("Not Selected", "-1"));

        if (CurID != -1)
        {
            payPeriodFilter.add(new Match("PayGroupID", CurID));
            payPeriodFilter.add("PayPeriodFr", false);
            DBFilter empPayrollFilter = new DBFilter();

            empPayrollFilter.add(new Match("EmpPayStatus", "C"));

            // Start 0000138, Ricky So, 2014/12/23
            DBFilter m_costAllocationFilter = new DBFilter();
            m_costAllocationFilter.add(new Match("CostAllocationStatus", "C"));
            empPayrollFilter.add(new IN("not EmpPayrollID", "Select Distinct EmpPayrollID from " + ECostAllocation.db.dbclass.tableName, m_costAllocationFilter));
            //empPayrollFilter.add(new IN("not EmpPayrollID", "Select Distinct EmpPayrollID from " + ECostAllocation.db.dbclass.tableName, new DBFilter()));
            // End 0000138, Ricky So, 2014/12/23

            payPeriodFilter.add(new IN("PayPeriodID", "Select Distinct PayPeriodID from " + EEmpPayroll.db.dbclass.tableName, empPayrollFilter));

            ArrayList list = EPayrollPeriod.db.select(dbConn, payPeriodFilter);
            foreach (EPayrollPeriod o in list)
            {
                PayPeriodID.Items.Add(new ListItem(o.PayPeriodFr.ToString("yyyy/MM/dd") + " - " + o.PayPeriodTo.ToString("yyyy/MM/dd"), o.PayPeriodID.ToString("0")));
            }
        }
        if (PayPeriodID.Items.Count > 1)
        {
            PayPeriodID.SelectedIndex = 1;
        }
        else
            PayPeriodID.SelectedIndex = 0;

    }
    //***** End 2014/04/11, Ricky So, 0000019

    protected void PayGroupID_SelectedIndexChanged(object sender, EventArgs e)
    {
        //***** Start 2014/04/11, Ricky So, 0000019
        reloadPayrollPeriod();
        //***** End 2014/04/11, Ricky So, 0000019

        loadObject();
        view = loadData(info, EEmpPayroll.db, Repeater);
    }
    protected void PayPeriodID_SelectedIndexChanged(object sender, EventArgs e)
    {
        loadObject();
        view = loadData(info, EEmpPayroll.db, Repeater);
    }


 
    public DataView loadData(ListInfo info, DBManager db, Repeater repeater)
    {
        DBFilter filter = sbinding.createFilter();

        //if (info != null && info.orderby != null && !info.orderby.Equals(""))
        //    filter.add(info.orderby, info.order);

        string select = "e.*, ep.EmpPayrollID";
        string from = "from [" + db.dbclass.tableName + "] ep, EmpPersonalInfo e ";

        filter.add(new MatchField("e.EmpID", "ep.EmpID"));
        filter.add(new Match("ep.EmpPayStatus", "C"));
        filter.add(new Match("ep.PayPeriodID", CurPayPeriodID));
        filter.add(new IN("not ep.EmpPayrollID", "Select EmpPayrollID from " + ECostAllocation.db.dbclass.tableName, new DBFilter()));

        // Start 0000185, KuangWei, 2015-04-21
        //filter.add(WebUtils.AddRankFilter(Session, "e.EmpID", true));
        DBFilter empInfoFilter = EmployeeSearchControl1.GetEmpInfoFilter(AppUtils.ServerDateTime(), AppUtils.ServerDateTime());
        empInfoFilter.add(new MatchField("e.EmpID", "ee.EmpID"));
        filter.add(new Exists(EEmpPersonalInfo.db.dbclass.tableName + " ee", empInfoFilter));

        DataTable table = WebUtils.GetDataTableFromSelectQueryWithFilter(dbConn, select, from, filter, info);
        table = EmployeeSearchControl1.FilterEncryptedEmpInfoField(table, info);
        // End 0000185, KuangWei, 2015-04-21

        if (table.Rows.Count != 0)
            btnTrialRun.Visible = IsAllowEdit;
        else
            btnTrialRun.Visible = false;

        view = new DataView(table);
        ListFooter.Refresh();
        if (repeater != null)
        {
            repeater.DataSource = view;
            repeater.DataBind();
        }

        return view;
    }
    //protected void FirstPage_Click(object sender, EventArgs e)
    //{
    //    view = loadData(info, EEmpPayroll.db, Repeater);

    //}
    //protected void PrevPage_Click(object sender, EventArgs e)
    //{
    //    view = loadData(info, EEmpPayroll.db, Repeater);

    //}
    //protected void NextPage_Click(object sender, EventArgs e)
    //{
    //    view = loadData(info, EEmpPayroll.db, Repeater);

    //}
    //protected void LastPage_Click(object sender, EventArgs e)
    //{

    //    view = loadData(info, EEmpPayroll.db, Repeater);

    //}
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
        WebFormUtils.LoadKeys(EEmpPayroll.db, row, cb);
        cb.Visible = IsAllowEdit;
        if (cb.Visible == true)
            cb.Checked = true;
    }

    protected void btnTrialRun_Click(object sender, EventArgs e)
    {
        ArrayList list = new ArrayList();
        foreach (RepeaterItem i in Repeater.Items)
        {
            CheckBox cb = (CheckBox)i.FindControl("ItemSelect");
            if (cb.Checked)
            {
                EEmpPayroll o = (EEmpPayroll)EEmpPayroll.db.createObject();
                WebFormUtils.GetKeys(EEmpPayroll.db, o, cb);
                list.Add(o);
            }

        }

        DateTime processDateTime = AppUtils.ServerDateTime() ;


        if (list.Count > 0)
        {
            //EPayrollPeriod payPeriod = new EPayrollPeriod();
            //payPeriod.PayPeriodID = CurPayPeriodID;
            //payPeriod.PayPeriodStatus = "C";
            //payPeriod.PayPeriodConfirmDate = processDateTime;
            //payPeriod.PayPeriodConfirmBy = WebUtils.GetCurUser(Session).UserID;
            //EPayrollPeriod.db.update(dbConn, payPeriod);

            //EPayrollBatch payBatch = new EPayrollBatch();
            //payBatch.PayBatchConfirmDate = processDateTime;
            //EPayrollBatch.db.insert(dbConn, payBatch);
            CostAllocationProcess costAllocationProcess = new CostAllocationProcess(dbConn);

            foreach (EEmpPayroll o in list)
            {
                if (EEmpPayroll.db.select(dbConn, o))
                {
                    WebUtils.StartFunction(Session, FUNCTION_CODE, o.EmpID, true);
                    costAllocationProcess.TrialRun(o.EmpPayrollID, WebUtils.GetCurUser(Session));
                    WebUtils.EndFunction(dbConn);

                    //EEmpPayroll empPayroll = new EEmpPayroll();
                    //empPayroll.EmpPayrollID = o.EmpPayrollID;
                    //EEmpPayroll.db.select(dbConn, empPayroll);
                    //if (empPayroll.EmpPayStatus.Equals("T", StringComparison.CurrentCultureIgnoreCase))
                    //{
                    //    empPayroll.EmpPayStatus = "C";
                    //    empPayroll.EmpPayConfirmDate = processDateTime;
                    //    empPayroll.PayBatchID = payBatch.PayBatchID;
                    //    EEmpPayroll.db.update(dbConn, empPayroll);
                    //}
                }
            }
            if (costAllocationProcess.hasError)
            {
                PageErrors.getErrors(db, Page).addGlobalError("Complete with error");
                PageErrors.getErrors(db, Page).addGlobalError(costAllocationProcess.ErrorLog.Replace("\r","\\r"));
            }
            else
                PageErrors.getErrors(db, Page).addError("Complete");
            //Response.Write("<script>alert('Completed'); </script>");
        }
        loadObject();
        loadData(info, EEmpPayroll.db, Repeater);
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
