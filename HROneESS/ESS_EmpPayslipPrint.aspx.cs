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

public partial class ESS_EmpPayslipPrint : HROneWebPage
{
    public Binding binding;
    public DBManager db = EEmpPersonalInfo.db;
    public EEmpPersonalInfo obj;
    public int CurID = -1;



    protected void Page_Load(object sender, EventArgs e)
    {
        if (!WebUtils.CheckAccess(Response, Session))
            return;
        binding = new Binding(dbConn, db);

        EESSUser user = WebUtils.GetCurUser(Session);
        if (user != null)
            CurID = user.EmpID;

        DBFilter empPayrollFilter = new DBFilter();
        empPayrollFilter.add(new Match("EmpID", CurID));
        empPayrollFilter.add(new Match("EmpPayStatus", "=", "C"));

        if (ESystemParameter.getParameter(dbConn, ESystemParameter.PARAM_CODE_ESS_PAYSLIP_AUTO_RELEASE).Equals("N", StringComparison.CurrentCultureIgnoreCase))
        {
            DBFilter payBatchFilter = new DBFilter();
            payBatchFilter.add(new Match("PayBatchIsESSPaySlipRelease", true));
            empPayrollFilter.add(new IN("PayBatchID", "SELECT pb.PayBatchID FROM " + EPayrollBatch.db.dbclass.tableName + " pb", payBatchFilter));
        }
        string strPeriodStartDate = ESystemParameter.getParameter(dbConn, ESystemParameter.PARAM_CODE_ESS_PAYSLIP_START_DATE);

        if (!IsPostBack)
        {



            DBFilter yearFilter = new DBFilter();
            yearFilter.add(new IN("PayPeriodID", "Select distinct PayPeriodID from EmpPayroll", empPayrollFilter));
            yearFilter.add("Year(PayPeriodFr)", false);
            DateTime dtPeriodStartDate;
            if (DateTime.TryParse(strPeriodStartDate, out dtPeriodStartDate))
            {
                yearFilter.add(new Match("PayPeriodFr", ">=", dtPeriodStartDate));
            }


            Year.Items.Clear();
            DataTable table = yearFilter.loadData(dbConn, "Select Distinct Year(PayPeriodFr) from PayrollPeriod"); //dbConn.GetDataTable("Select Distinct Year(PayPeriodFr) from PayrollPeriod where PayPeriodID in (Select distinct PayPeriodID from EmpPayroll where EmpID=" + CurID + " and EmpPayStatus ='C') order by Year(PayPeriodFR) desc");
            foreach (DataRow row in table.Rows)
            {
                Year.Items.Add(row[0].ToString());
            }
            if (Year.Items.Count > 0)
            {
                Year.SelectedIndex = 0;
            }
        }

        DateTime payPeriodFr = new DateTime();
        DateTime payPeriodTo = new DateTime();
        if (!string.IsNullOrEmpty(Year.SelectedValue))
        {
            payPeriodFr = new DateTime(int.Parse(Year.SelectedValue), 1, 1);
            payPeriodTo = new DateTime(int.Parse(Year.SelectedValue), 12, 31);
        }



        // binding.add(EmpNo);

        DBFilter payPeriodFilter = new DBFilter();
        if (!payPeriodFr.Ticks.Equals(0) && !payPeriodTo.Ticks.Equals(0))
        {
            payPeriodFilter.add(new Match("PayPeriodFr", ">=", payPeriodFr));
            payPeriodFilter.add(new Match("PayPeriodFr", "<=", payPeriodTo));
            DateTime dtPeriodStartDate;
            if (DateTime.TryParse(strPeriodStartDate, out dtPeriodStartDate))
            {
                payPeriodFilter.add(new Match("PayPeriodFr", ">=", dtPeriodStartDate));
            }
        }
        payPeriodFilter.add(new IN("PayPeriodID", "Select distinct PayPeriodID from " + EEmpPayroll.db.dbclass.tableName, empPayrollFilter));
        binding.add(new DropDownVLBinder(EPayrollPeriod.db, PayPeriodID, EPayrollPeriod.VLPayrollPeriod, payPeriodFilter));

        binding.init(Request, Session);


        HROne.Common.WebUtility.WebControlsLocalization(this, this.Controls);




        if (!Page.IsPostBack)
        {

            if (CurID > 0)
            {
                loadObject();
            }
            else
            {

            }
        }
    }

    protected bool loadObject()
    {
        obj = new EEmpPersonalInfo();
        obj.EmpID = CurID;
        // bool isNew = WebFormWorkers.loadKeys(db, obj, Request);
        if (!db.select(dbConn, obj))
            return false;

        Hashtable values = new Hashtable();
        db.populate(obj, values);
        binding.toControl(values);
        return true;
    }
    protected void btnGenerate_Click(object sender, EventArgs e)
    {
        ArrayList EmpList = new ArrayList();
        EEmpPersonalInfo empInfo = new EEmpPersonalInfo();
        empInfo.EmpID = CurID;
        EmpList.Add(empInfo);

        if (!string.IsNullOrEmpty(PayPeriodID.SelectedValue))
        {
            string hLevelIDListString = ESystemParameter.getParameter(dbConn, ESystemParameter.PARAM_CODE_PAY_SLIP_HIERARCHY_DISPLAY_SEQUENCE);
            string[] hlevelIDList = hLevelIDListString.Split(new string[] { "|" }, StringSplitOptions.RemoveEmptyEntries);

            DBFilter payBatchFilter = new DBFilter();
            DBFilter empPayrollFilter = new DBFilter();
            empPayrollFilter.add(new Match("ep.PayPeriodID", PayPeriodID.SelectedValue));
            payBatchFilter.add(new IN("PayBatchID", "SELECT DISTINCT ep.PayBatchID FROM " + EEmpPayroll.db.dbclass.tableName + " ep", empPayrollFilter));
            if (ESystemParameter.getParameter(dbConn, ESystemParameter.PARAM_CODE_ESS_PAYSLIP_AUTO_RELEASE).Equals("N", StringComparison.CurrentCultureIgnoreCase))
                payBatchFilter.add(new Match("PayBatchIsESSPaySlipRelease", true));


            HROne.Reports.Payroll.PayrollDetailProcess rpt = new HROne.Reports.Payroll.PayrollDetailProcess(dbConn, EmpList, HROne.Reports.Payroll.PayrollDetailProcess.ReportType.PaySlip, null, EPayrollBatch.db.select(dbConn, payBatchFilter), new ArrayList(hlevelIDList));
            string reportFileName = WebUtils.GetLocalizedReportFile(Server.MapPath("~/Report_Payroll_PaySlip.rpt"));
            WebUtils.ReportExport(Response, rpt, reportFileName, "PDF", "PaySlip");
        }
    }
}
