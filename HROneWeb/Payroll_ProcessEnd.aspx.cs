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

public partial class Payroll_ProcessEnd : HROneWebPage
{
    private const string FUNCTION_CODE = "PAY008";
    public Binding binding;

    public DBManager db = EPayrollGroup.db;
    public EPayrollGroup obj;
    public int CurID = -1;
    public int CurPayPeriodID = -1;

    protected SearchBinding sNotConfirmEmpBinding;
    protected ListInfo NotConfirmEmpInfo;
    protected DataView NotConfirmEmpView;

    protected SearchBinding sNotTrialRunEmpBinding;
    protected ListInfo NotTrialRunEmpInfo;
    protected DataView NotTrialRunEmpView;

    // Start 0000096, KuangWei, 2014-09-18
    private int notTrialRunCount = 0;
    // End 0000096, KuangWei, 2014-09-18
    

    protected void Page_Load(object sender, EventArgs e)
    {
       
        if (!WebUtils.CheckAccess(Response, Session, FUNCTION_CODE, WebUtils.AccessLevel.Read))
            return;
        if (!WebUtils.CheckPermission(Session, FUNCTION_CODE, WebUtils.AccessLevel.ReadWrite))
        {
            panelProcessEndOption.Visible = false;
        }
        else
        {
            panelProcessEndOption.Visible = true;
        }

        btnProcessEnd.OnClientClick = HROne.Translation.PromptMessage.PAYROLL_PROCESS_END_GENERIC_JAVASCRIPT;

        binding = new Binding(dbConn, db);
        // Start 0000069, KuangWei, 2014-08-26
        //binding.add(new DropDownVLBinder(db, PayGroupID, EPayrollGroup.VLPayrollGroup));
        initPayrollGroup();
        // End 0000069, KuangWei, 2014-08-26
        binding.add(CurrentPayPeriodID);

        DBFilter payPeriodFilter = new DBFilter();
        payPeriodFilter.add(new Match("PayPeriodStatus", "<>", "T"));
        payPeriodFilter.add(new Match("PayPeriodStatus", "<>", "E"));
        if (!int.TryParse(PayGroupID.SelectedValue, out CurID))
            if (!int.TryParse(DecryptedRequest["PayGroupID"], out CurID))
                CurID = -1;
        payPeriodFilter.add(new Match("PayGroupID", CurID));
        payPeriodFilter.add("PayPeriodFr", false);

        binding.add(new DropDownVLBinder(EPayrollPeriod.db, PayPeriodID, EPayrollPeriod.VLPayrollPeriod, payPeriodFilter));

        binding.init(Request, Session);

        sNotConfirmEmpBinding = new SearchBinding(dbConn, EEmpPersonalInfo.db);
        //sNotConfirmEmpBinding.add(new HiddenMatchBinder(CurrentPayPeriodID,"ep.PayPeriodID" ));

        sNotTrialRunEmpBinding = new SearchBinding(dbConn, EEmpPersonalInfo.db);
        //sNotTrialRunEmpBinding.add(new HiddenMatchBinder(CurrentPayPeriodID, "pp.PayPeriodID"));

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
        NotConfirmEmpInfo = NotConfirm_ListFooter.ListInfo;  //new ListInfo();

        NotTrialRunEmpInfo = NotTrialRun_ListFooter.ListInfo;  //new ListInfo();

        if (!Page.IsPostBack)
        {
            loadObject();
            //loadState();
            if (CurID > 0)
            {
                panelPayPeriod.Visible = true;
                NotConfirmEmpView = loadNotConfirmData(NotConfirmEmpInfo, EEmpPayroll.db, NotConfirm_Repeater);
                NotTrialRunEmpView = loadNotTrialRunData(NotTrialRunEmpInfo, EEmpPersonalInfo.db, NotTrialRun_Repeater);
            }
            else
            {
                panelPayPeriod.Visible = false;
                panelProcessEndOption.Visible = false;
            }

        }

    }

    // Start 0000069, KuangWei, 2014-08-26
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
    // End 0000069, KuangWei, 2014-08-26

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

        try
        {
            PayPeriodID.SelectedValue = CurPayPeriodID.ToString();
        }
        catch
        {
            CurPayPeriodID = 0;
        }
        if (CurPayPeriodID <= 0 && obj.CurrentPayPeriodID > 0)
        {
            CurPayPeriodID = obj.CurrentPayPeriodID;
            try
            {
                PayPeriodID.SelectedValue = CurPayPeriodID.ToString();
            }
            catch
            {
                CurPayPeriodID = 0;
            }
        }
        ucPayroll_PeriodInfo.CurrentPayPeriodID = CurPayPeriodID;

        if (CurPayPeriodID > 0)
        {
            panelPayPeriod.Visible = true;
            panelNotConfirmEmployeeList.Visible = false;
            panelNotTrialRunEmployeeList.Visible = false;
        }
        else
        {
            panelPayPeriod.Visible = false;
        }

        return true;
    }

    protected void PayGroupID_SelectedIndexChanged(object sender, EventArgs e)
    {
        loadObject();
        NotConfirmEmpView = loadNotConfirmData(NotConfirmEmpInfo, EEmpPayroll.db, NotConfirm_Repeater);
        NotTrialRunEmpView = loadNotTrialRunData(NotTrialRunEmpInfo, EEmpPersonalInfo.db, NotTrialRun_Repeater);
    }
    protected void PayPeriodID_SelectedIndexChanged(object sender, EventArgs e)
    {
        loadObject();
        NotConfirmEmpView = loadNotConfirmData(NotConfirmEmpInfo, EEmpPayroll.db, NotConfirm_Repeater);
        NotTrialRunEmpView = loadNotTrialRunData(NotTrialRunEmpInfo, EEmpPersonalInfo.db, NotTrialRun_Repeater);
    }
    //public void loadState()
    //{
    //    NotConfirmEmpInfo = NotConfirm_ListFooter.ListInfo;  //new ListInfo();
    //    //int page = 0;
    //    //if (!NotConfirm_CurPage.Value.Equals(""))
    //    //    page = Int32.Parse(NotConfirm_CurPage.Value);
    //    //NotConfirmEmpInfo.loadState(Request, page);

    //    NotTrialRunEmpInfo = NotTrialRun_ListFooter.ListInfo;  //new ListInfo();
    //    //page = 0;
    //    //if (!NotTrialRun_CurPage.Value.Equals(""))
    //    //    page = Int32.Parse(NotTrialRun_CurPage.Value);
    //    //NotTrialRunEmpInfo.loadState(Request, page);
    //}
 
    public DataView loadNotConfirmData(ListInfo info, DBManager db, Repeater repeater)
    {
        DBFilter filter = sNotConfirmEmpBinding.createFilter();

        //if (info != null && info.orderby != null && !info.orderby.Equals(""))
        //    filter.add(info.orderby, info.order);

        string select = "e.*, ep.EmpPayrollID";
        string from = "from [" + db.dbclass.tableName + "] ep, EmpPersonalInfo e ";

        filter.add(new MatchField("e.EmpID", "ep.EmpID"));
        filter.add(new Match("ep.EmpPayStatus", "T"));
        filter.add(new Match("ep.PayPeriodID", CurPayPeriodID));

        // Start 0000185, KuangWei, 2015-04-21
        DBFilter empInfoFilter = EmployeeSearchControl1.GetEmpInfoFilter(AppUtils.ServerDateTime(), AppUtils.ServerDateTime());
        empInfoFilter.add(new MatchField("e.EmpID", "ee.EmpID"));
        filter.add(new Exists(EEmpPersonalInfo.db.dbclass.tableName + " ee", empInfoFilter));

        DataTable table = WebUtils.GetDataTableFromSelectQueryWithFilter(dbConn, select, from, filter, info);
        table = EmployeeSearchControl1.FilterEncryptedEmpInfoField(table, info);
        // End 0000185, KuangWei, 2015-04-21

        if (table.Rows.Count > 0)
        {
            panelNotConfirmEmployeeList.Visible = true;
            panelProcessEndOption.Visible = false;
        }
        else
            panelNotConfirmEmployeeList.Visible = false;

        NotConfirmEmpView = new DataView(table);

        NotConfirm_ListFooter.Refresh();
        if (repeater != null)
        {
            repeater.DataSource = NotConfirmEmpView;
            repeater.DataBind();
        }

        return NotConfirmEmpView;
    }
    protected void NotConfirm_FirstPage_Click(object sender, EventArgs e)
    {
        //loadState();
        //NotConfirmEmpInfo.page = 0;
        NotConfirmEmpView = loadNotConfirmData(NotConfirmEmpInfo, EEmpPayroll.db, NotConfirm_Repeater);

    }
    protected void NotConfirm_PrevPage_Click(object sender, EventArgs e)
    {
        //loadState();
        //NotConfirmEmpInfo.page--;
        NotConfirmEmpView = loadNotConfirmData(NotConfirmEmpInfo, EEmpPayroll.db, NotConfirm_Repeater);

    }
    protected void NotConfirm_NextPage_Click(object sender, EventArgs e)
    {
        //loadState();
        //NotConfirmEmpInfo.page++;
        NotConfirmEmpView = loadNotConfirmData(NotConfirmEmpInfo, EEmpPayroll.db, NotConfirm_Repeater);

    }
    protected void NotConfirm_LastPage_Click(object sender, EventArgs e)
    {
        //loadState();

        //NotConfirmEmpInfo.page = Int32.Parse(NotConfirm_NumPage.Value);
        NotConfirmEmpView = loadNotConfirmData(NotConfirmEmpInfo, EEmpPayroll.db, NotConfirm_Repeater);

    }
    protected void NotConfirm_ChangeOrder_Click(object sender, EventArgs e)
    {
        LinkButton l = (LinkButton)sender;
        String id = l.ID.Substring("NotConfirm_".Length);
        //loadState();
        if (NotConfirmEmpInfo.orderby == null)
            NotConfirmEmpInfo.order = true;
        else if (NotConfirmEmpInfo.orderby.Equals(id))
            NotConfirmEmpInfo.order = !NotConfirmEmpInfo.order;
        else
            NotConfirmEmpInfo.order = true;
        NotConfirmEmpInfo.orderby = id;

        NotConfirmEmpView = loadNotConfirmData(NotConfirmEmpInfo, EEmpPayroll.db, NotConfirm_Repeater);

    }
    protected void NotConfirm_Repeater_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        DataRowView row = (DataRowView)e.Item.DataItem;
        CheckBox cb = (CheckBox)e.Item.FindControl("ItemSelect");
        WebFormUtils.LoadKeys(EEmpPayroll.db, row, cb);
    }


    public DataView loadNotTrialRunData(ListInfo info, DBManager db, Repeater repeater)
    {
        DBFilter filter = sNotTrialRunEmpBinding.createFilter();

        //if (info != null && info.orderby != null && !info.orderby.Equals(""))
        //    filter.add(info.orderby, info.order);

        string select = "e.* ";
        string from = "from [" + db.dbclass.tableName + "] e, [EmpPositionInfo] ep, [PayrollPeriod] pp ";

        filter.add(new MatchField("e.EmpID", "ep.EmpID"));
        filter.add(new MatchField("ep.PayGroupID", "pp.PayGroupID"));
        filter.add(new MatchField("ep.EmpPosEffFr", "<=", "pp.PayPeriodTo"));
        filter.add(new MatchField("e.EmpDateOfJoin", "<=", "pp.PayPeriodTo"));
        filter.add(new Match("pp.PayPeriodID", CurPayPeriodID));

        OR orFilter = new OR();
        orFilter.add(new MatchField("ep.EmpPosEffTo", ">=", "pp.PayPeriodFr"));
        orFilter.add(new NullTerm("ep.EmpPosEffTo"));

        filter.add(orFilter);


        filter.add(new IN("Not e.empid", "Select et.empid from [EmpTermination] et where et.EmpTermLastDate<pp.PayPeriodFr", new DBFilter() ));

        filter.add(new IN("Not e.empid", "Select ep.empid from [EmpPayroll] ep where pp.PayPeriodID=ep.PayPeriodID and ep.EmpPayIsRP='Y'", new DBFilter()));

        // Start 0000185, KuangWei, 2015-04-21
        DBFilter empInfoFilter = EmployeeSearchControl1.GetEmpInfoFilter(AppUtils.ServerDateTime(), AppUtils.ServerDateTime());
        empInfoFilter.add(new MatchField("e.EmpID", "ee.EmpID"));
        filter.add(new Exists(EEmpPersonalInfo.db.dbclass.tableName + " ee", empInfoFilter));

        DataTable table = WebUtils.GetDataTableFromSelectQueryWithFilter(dbConn, select, from, filter, info);
        table = EmployeeSearchControl1.FilterEncryptedEmpInfoField(table, info);
        // End 0000185, KuangWei, 2015-04-21

        if (table.Rows.Count > 0)
        // Start 0000096, KuangWei, 2014-09-18
        {
            panelNotTrialRunEmployeeList.Visible = true; //& panelProcessEndOption.Visible;
            notTrialRunCount = table.Rows.Count;
            //string msg = notTrialRunCount + " of employees have not been run!!! Are you sure to end this payroll process?";
            string msg = "Certain number of employees have not been run!!! Are you sure to end this payroll process?";
            btnProcessEnd.OnClientClick = HROne.Translation.PromptMessage.CreateConfirmDialogJavascript(HROne.Common.WebUtility.GetLocalizedString(msg));
        }
        // End 0000096, KuangWei, 2014-09-18
        else
            panelNotTrialRunEmployeeList.Visible = false;

        NotTrialRunEmpView = new DataView(table);

        NotTrialRun_ListFooter.Refresh();

        if (repeater != null)
        {
            repeater.DataSource = NotTrialRunEmpView;
            repeater.DataBind();
        }

        return NotTrialRunEmpView;
    }
    protected void NotTrialRun_FirstPage_Click(object sender, EventArgs e)
    {
        //loadState();
        //NotTrialRunEmpInfo.page = 0;
        NotTrialRunEmpView = loadNotTrialRunData(NotTrialRunEmpInfo, EEmpPersonalInfo.db, NotTrialRun_Repeater);

    }
    protected void NotTrialRun_PrevPage_Click(object sender, EventArgs e)
    {
        //loadState();
        //NotTrialRunEmpInfo.page--;
        NotTrialRunEmpView = loadNotTrialRunData(NotTrialRunEmpInfo, EEmpPersonalInfo.db, NotTrialRun_Repeater);

    }
    protected void NotTrialRun_NextPage_Click(object sender, EventArgs e)
    {
        //loadState();
        //NotTrialRunEmpInfo.page++;
        NotTrialRunEmpView = loadNotTrialRunData(NotTrialRunEmpInfo, EEmpPersonalInfo.db, NotTrialRun_Repeater);

    }
    protected void NotTrialRun_LastPage_Click(object sender, EventArgs e)
    {
        //loadState();

        //NotTrialRunEmpInfo.page = Int32.Parse(NotTrialRun_NumPage.Value);
        NotTrialRunEmpView = loadNotTrialRunData(NotTrialRunEmpInfo, EEmpPersonalInfo.db, NotTrialRun_Repeater);

    }
    protected void NotTrialRun_ChangeOrder_Click(object sender, EventArgs e)
    {
        LinkButton l = (LinkButton)sender;
        String id = l.ID.Substring("NotTrialRun_".Length);
        //loadState();
        if (NotTrialRunEmpInfo.orderby == null)
            NotTrialRunEmpInfo.order = true;
        else if (NotTrialRunEmpInfo.orderby.Equals(id))
            NotTrialRunEmpInfo.order = !NotTrialRunEmpInfo.order;
        else
            NotTrialRunEmpInfo.order = true;
        NotTrialRunEmpInfo.orderby = id;

        NotTrialRunEmpView = loadNotTrialRunData(NotTrialRunEmpInfo, EEmpPersonalInfo.db, NotTrialRun_Repeater);

    }
    protected void NotTrialRun_Repeater_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        DataRowView row = (DataRowView)e.Item.DataItem;
        CheckBox cb = (CheckBox)e.Item.FindControl("ItemSelect");
        WebFormUtils.LoadKeys(EEmpPersonalInfo.db, row, cb);
    }
    protected void btnProcessEnd_Click(object sender, EventArgs e)
    {
        PageErrors errors = PageErrors.getErrors(db, Page.Master);
        PayrollProcess payrollProcess= new PayrollProcess(dbConn);
        WebUtils.StartFunction(Session, FUNCTION_CODE);
        payrollProcess.PayrollProcessEnd(CurPayPeriodID, WebUtils.GetCurUser(Session).UserID);
        EPayrollPeriod payPeriod = payrollProcess.GenerateNextPayrollPeriod(CurID);
        WebUtils.EndFunction(dbConn);
        errors.addError("Complete");

        panelNotConfirmEmployeeList.Visible = false;
        panelNotTrialRunEmployeeList.Visible = false;
        panelProcessEndOption.Visible = false;
        //CurPayPeriodID = payPeriod.PayPeriodID;
        //binding.init(Request, Session);

        ////loadState();
        //if (CurID > 0)
        //{
        //    panelPayPeriod.Visible = true;
        //    loadObject();
        //    NotConfirmEmpView = loadNotConfirmData(NotConfirmEmpInfo, EEmpPayroll.db, NotConfirm_Repeater);
        //    NotTrialRunEmpView = loadNotTrialRunData(NotTrialRunEmpInfo, EEmpPersonalInfo.db, NotTrialRun_Repeater);
        //}
        //else
        //{
        //    panelPayPeriod.Visible = false;
        //    panelProcessEndOption.Visible = false;
        //}
        //        Response.Redirect(Request.Url.LocalPath + "?" + Request.QueryString);
    }

    // Start 0000185, KuangWei, 2015-04-21
    protected void Search_Click(object sender, EventArgs e)
    {
        NotConfirmEmpView = loadNotConfirmData(NotConfirmEmpInfo, EEmpPayroll.db, NotConfirm_Repeater);
        NotTrialRunEmpView = loadNotTrialRunData(NotTrialRunEmpInfo, EEmpPersonalInfo.db, NotTrialRun_Repeater);
    }

    protected void Reset_Click(object sender, EventArgs e)
    {
        sNotConfirmEmpBinding.clear();
        sNotTrialRunEmpBinding.clear();
        EmployeeSearchControl1.Reset();
        EmployeeSearchControl1.EmpStatusValue = "A";
        NotConfirmEmpView = loadNotConfirmData(NotConfirmEmpInfo, EEmpPayroll.db, NotConfirm_Repeater);
        NotTrialRunEmpView = loadNotTrialRunData(NotTrialRunEmpInfo, EEmpPersonalInfo.db, NotTrialRun_Repeater);
    }
    // End 0000185, KuangWei, 2015-04-21
}
