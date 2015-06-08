using System;
using System.Text;
using System.Globalization;
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

public partial class Report_Payroll_NewJoinPaymentSummary : HROneWebPage
{
    private const string FUNCTION_CODE = "RPT213";
    protected DBManager db = EEmpPersonalInfo.db;
    protected SearchBinding binding;
    protected ListInfo info;
    protected DataView view;

    private const string SYSTEMPARAMETER_PREPAREDBY = "REPORT_PAYROLL_NEWJOINPAYMENTSUMMARY_PREPAREDBY";
    private const string SYSTEMPARAMETER_REVIEWEDBY = "REPORT_PAYROLL_NEWJOINPAYMENTSUMMARY_REVIEWEDBY";
    

    protected void Page_Load(object sender, EventArgs e)
    {
       
        if (!WebUtils.CheckAccess(Response, Session, FUNCTION_CODE, WebUtils.AccessLevel.Read))
            return;

        binding = new SearchBinding(dbConn, db);
       
        binding.init(DecryptedRequest, null);
        info = ListFooter.ListInfo;

        HROne.Common.WebUtility.WebControlsLocalization(this, this.Controls);

    }

    protected void Page_PreRender(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            txtPreparedBy.Text = ESystemParameter.getParameter(dbConn, SYSTEMPARAMETER_PREPAREDBY);
            txtReviewedBy.Text = ESystemParameter.getParameter(dbConn, SYSTEMPARAMETER_REVIEWEDBY);
            //---------------------------------------
            // Show active employee 
            this.EmployeeSearchControl1.EmpStatusValue = "A";
            //---------------------------------------
            view = loadData(info, db, Repeater);
        }
    }

    public DataView loadData(ListInfo info, DBManager db, Repeater repeater)
    {
        DBFilter filter = binding.createFilter();

        if (info != null && info.orderby != null && !info.orderby.Equals(""))
            filter.add(info.orderby, info.order);

        string select = "e.*";
        string from = "from [" + db.dbclass.tableName + "] e ";
        filter.add(WebUtils.AddRankFilter(Session, "e.EmpID", true));

        DBFilter empPayrollFilter = new DBFilter();
        empPayrollFilter.add(Payroll_PeriodSelectionList1.GetEmpPayrollDBTerm("ep"));
        filter.add(new IN("e.EmpID", "SELECT DISTINCT ep.EmpID FROM " + EEmpPayroll.db.dbclass.tableName + " ep", empPayrollFilter));

        DBFilter payPeriodFilter = new SearchBinding(dbConn, EPayrollPeriod.db).createFilter(); ;
        payPeriodFilter.add(new IN("pp.PayPeriodID", "SELECT DISTINCT ep.PayPeriodID FROM " + EEmpPayroll.db.dbclass.tableName + " ep", empPayrollFilter));
        string sqlStr = " min(pp.PayPeriodFr) as PayPeriodFr, max(pp.PayPeriodTo) as PayPeriodTo ";
        string fromStr = " from [" + EPayrollPeriod.db.dbclass.tableName + "] pp ";
        DataTable payPeriodTable = payPeriodFilter.loadData(dbConn, null, sqlStr, fromStr);
        int rowCount = payPeriodTable.Rows.Count;
        if (rowCount > 0 && !string.IsNullOrEmpty(payPeriodTable.Rows[0]["PayPeriodFr"].ToString()))
        {
            filter.add(new Match("e.EmpDateOfJoin", ">=", ((DateTime)payPeriodTable.Rows[0]["PayPeriodFr"]).ToString("yyyy/MM/dd")));
            filter.add(new Match("e.EmpDateOfJoin", "<=", ((DateTime)payPeriodTable.Rows[0]["PayPeriodTo"]).ToString("yyyy/MM/dd")));
        }

        DBFilter empInfoFilter = EmployeeSearchControl1.GetEmpInfoFilter(AppUtils.ServerDateTime(), AppUtils.ServerDateTime());
        empInfoFilter.add(new MatchField("e.EmpID", "ee.EmpID"));
        filter.add(new Exists(EEmpPersonalInfo.db.dbclass.tableName + " ee", empInfoFilter));

        DataTable table = filter.loadData(dbConn, null, select, from);
        table = EmployeeSearchControl1.FilterEncryptedEmpInfoField(table, info);

        view = new DataView(table);
        if (repeater != null)
        {
            repeater.DataSource = view;
            repeater.DataBind();
        }

        return view;
    }

    protected void Search_Click(object sender, EventArgs e)
    {
        view = loadData(info, db, Repeater);

    }

    protected void Reset_Click(object sender, EventArgs e)
    {
        binding.clear();
        EmployeeSearchControl1.Reset();
        this.EmployeeSearchControl1.EmpStatusValue = "A";

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
        WebFormUtils.LoadKeys(EEmpPersonalInfo.db, row, cb);
    }

    protected void btnGenerate_Click(object sender, EventArgs e)
    {
        PageErrors errors = PageErrors.getErrors(db, Page.Master);
        errors.clear();

        ArrayList empJoinList = new ArrayList();
        foreach (RepeaterItem item in Repeater.Items)
        {
            CheckBox cb = (CheckBox)item.FindControl("ItemSelect");
            if (cb.Checked)
            {
                EEmpPersonalInfo empJoin = new EEmpPersonalInfo();
                WebFormUtils.GetKeys(EEmpPersonalInfo.db, empJoin, cb);
                empJoinList.Add(empJoin);
            }
        }

        if (empJoinList.Count < 1)
            errors.addError("Employee not selected");

        if (errors.isEmpty())
        {
            ESystemParameter.setParameter(dbConn, SYSTEMPARAMETER_PREPAREDBY, txtPreparedBy.Text);
            ESystemParameter.setParameter(dbConn, SYSTEMPARAMETER_REVIEWEDBY, txtReviewedBy.Text);

            HROne.Reports.Payroll.NewJoinPaymentSummaryProcess rpt = null;
            if (Payroll_PeriodSelectionList1.SelectedPayrollStatus.Equals("C"))
            {
                ArrayList payBatchList = this.Payroll_PeriodSelectionList1.GetPayBatchList();
                rpt = new HROne.Reports.Payroll.NewJoinPaymentSummaryProcess(dbConn, empJoinList, null, payBatchList, HROne.Reports.Payroll.NewJoinPaymentSummaryProcess.ReportType.History, txtPreparedBy.Text, txtReviewedBy.Text);
            }
            else if (Payroll_PeriodSelectionList1.SelectedPayrollStatus.Equals("T"))
            {
                ArrayList payPeriodList = this.Payroll_PeriodSelectionList1.GetTrialRunPayPeriodList();
                rpt = new HROne.Reports.Payroll.NewJoinPaymentSummaryProcess(dbConn, empJoinList, payPeriodList, null, HROne.Reports.Payroll.NewJoinPaymentSummaryProcess.ReportType.TrialRun, txtPreparedBy.Text, txtReviewedBy.Text);
            }

            string reportFileName = WebUtils.GetLocalizedReportFile(Server.MapPath("~/Report_Payroll_NewJoinPaymentSummary.rpt"));

            WebUtils.ReportExport(dbConn, user, errors, lblReportHeader.Text, Response, rpt, reportFileName, ((Button)sender).CommandArgument, "NewJoinPaymentSummary", true);
        }
    }

    protected void Payroll_PeriodSelectionList1_PayrollBatchChecked(object sender, EventArgs e)
    {
        info.page = 0;
        view = loadData(info, db, Repeater);
    }

}
