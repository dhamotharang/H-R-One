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
using HROne.BankFile;

public partial class Customize_FandV_Report_Payroll_MPFRemittanceStatement_List : HROneWebPage
{
    private const string FUNCTION_CODE = "CUSTOM005";
    protected SearchBinding empSBinding, sbinding;
    public DBManager db = EPayrollGroup.db;
    public EPayrollGroup obj;

    protected ListInfo empInfo;
    protected DataView empView;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!WebUtils.CheckAccess(Response, Session, FUNCTION_CODE, WebUtils.AccessLevel.Read))
            return;

        empSBinding = new SearchBinding(dbConn, EEmpPersonalInfo.db);
        empSBinding.add(new DropDownVLSearchBinder(MPFPlanID, "MPFPlanID",EMPFPlan.VLMPFPlan));
        empSBinding.add(new DropDownVLSearchBinder(ORSOPlanID, "ORSOPlanID", EORSOPlan.VLORSOPlan));
        empSBinding.initValues("EmpStatus", null, EEmpPersonalInfo.VLEmpStatus, HROne.Common.WebUtility.GetSessionUICultureInfo(Session));
        empSBinding.init(DecryptedRequest, null);

        empInfo = ListFooter.ListInfo;

        HROne.Common.WebUtility.WebControlsLocalization(this, this.Controls);
    }

    protected void Page_PreRender(object sender, EventArgs e)
    {
        //if (!Page.IsPostBack)
        //{
        //    empView = emploadData(empInfo, EEmpPayroll.db, empRepeater);
        //}

    }

    public DataView emploadData(ListInfo info, DBManager db, Repeater repeater)
    {
        DBFilter filter = EmployeeSearchControl1.GetEmpInfoFilter(AppUtils.ServerDateTime().Date, AppUtils.ServerDateTime().Date);//new DBFilter();// empSBinding.createFilter();

        //if (info != null && info.orderby != null && !info.orderby.Equals(""))
        //    filter.add(info.orderby, info.order);

        string select = "* ";
        string from = "from [" + EEmpPersonalInfo.db.dbclass.tableName + "] ee ";

        DBFilter empPayrollFilter = new DBFilter();
        empPayrollFilter.add(new MatchField("ee.EmpID", "ep.EmpID"));
        empPayrollFilter.add(new Match("ep.EmpPayStatus", "C"));
//        empPayrollFilter.add(new Match("ep.PayPeriodID", CurPayPeriodID));


        DateTime dtPayPeriodFr, dtPayPeriodTo;
        if (DateTime.TryParse(PayPeriodFr.Value, out dtPayPeriodFr) && DateTime.TryParse(PayPeriodTo.Value, out dtPayPeriodTo))
        {
            DBFilter payPeriodFilter = new DBFilter();

            // refer to Date To of payperiod
            payPeriodFilter.add(new Match("pp.PayPeriodTo", "<=", dtPayPeriodTo));
            payPeriodFilter.add(new Match("pp.PayPeriodTo", ">=", dtPayPeriodFr));
            empPayrollFilter.add( new IN("PayperiodID ","Select payperiodID from PayrollPeriod pp",payPeriodFilter));
        }

        DBFilter mpfRecordFilter = new DBFilter();
        mpfRecordFilter.add(new Match("MPFPlanID",MPFPlanID.SelectedValue));

        DBFilter orsoRecordFilter = new DBFilter();
        orsoRecordFilter.add(new Match("ORSOPlanID", ORSOPlanID.SelectedValue));

        OR m_orPlan = new OR();
        m_orPlan.add(new IN("EmpPayrollID", "SELECT EmpPayrollID FROM MPFRecord", mpfRecordFilter));
        m_orPlan.add(new IN("EmpPayrollID", "SELECT EmpPayrollID FROM ORSORecord", orsoRecordFilter));

        empPayrollFilter.add(m_orPlan);

        Exists exists = new Exists(EEmpPayroll.db.dbclass.tableName + " ep", empPayrollFilter);

        filter.add(exists);
        filter.add(WebUtils.AddRankFilter(Session, "ee.EmpID", true));

        DataTable table = filter.loadData(dbConn, null, select, from);
        table = EmployeeSearchControl1.FilterEncryptedEmpInfoField(table, info);
        if (table.Rows.Count != 0)
        {
            btnGenerate1.Visible = true;
            btnGenerate2.Visible = true;
        }
        else
        {
            btnGenerate1.Visible = false;
            btnGenerate2.Visible = false;
        }
        empView = new DataView(table);
        //if (info != null)
        //{
        //    info.loadPageList(null, empPrevPage, empNextPage, empFirstPage, empLastPage);

        //    CurPage.Value = info.page.ToString();
        //    NumPage.Value = info.numPage.ToString();
        //}
        if (repeater != null)
        {
            repeater.DataSource = empView;
            repeater.DataBind();
        }

        return empView;
    }
    protected void empChangeOrder_Click(object sender, EventArgs e)
    {
        LinkButton l = (LinkButton)sender;
        String id = l.ID.Substring(1);
        if (empInfo.orderby == null)
            empInfo.order = true;
        else if (empInfo.orderby.Equals(id))
            empInfo.order = !empInfo.order;
        else
            empInfo.order = true;
        empInfo.orderby = id;

        empView = emploadData(empInfo, EEmpPayroll.db, empRepeater);

    }
    protected void empRepeater_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        DataRowView row = (DataRowView)e.Item.DataItem;
        CheckBox cb = (CheckBox)e.Item.FindControl("ItemSelect");
        WebFormUtils.LoadKeys(EEmpPersonalInfo.db, row, cb);

    }

    protected void Search_Click(object sender, EventArgs e)
    {
        empInfo.page = 0;
        empView = emploadData(empInfo, db, empRepeater);

    }
    protected void Reset_Click(object sender, EventArgs e)
    {
        empSBinding.clear();
        EmployeeSearchControl1.Reset();
        empInfo.page = 0;

        empView = emploadData(empInfo, db, empRepeater);
    }

    protected void btnGenerate_Click(object sender, EventArgs e)
    {
        PageErrors errors = PageErrors.getErrors(db, Page.Master);
        errors.clear();

        ArrayList empList = new ArrayList();

        int m_mpfPlanID = 0;
        int m_orsoPlanID = 0;

        int.TryParse(MPFPlanID.SelectedValue, out m_mpfPlanID);
        int.TryParse(ORSOPlanID.SelectedValue, out m_orsoPlanID);

        if (m_mpfPlanID == 0 && m_orsoPlanID == 0)
        {
            errors.addError("No MPF Plan / ORSO Plan selected");
            return;
        }

        foreach (RepeaterItem i in empRepeater.Items)
        {
            CheckBox cb = (CheckBox)i.FindControl("ItemSelect");
            if (cb.Checked)
            {
                EEmpPersonalInfo o = (EEmpPersonalInfo)EEmpPersonalInfo.db.createObject();
                WebFormUtils.GetKeys(EEmpPersonalInfo.db, o, cb);
                empList.Add(o);
            }
        }

        string strEmpList = string.Empty;
        string strPayBatchList = string.Empty;
        string strPayPeriodRequest = string.Empty;
        DateTime dtPayPeriodFr = new DateTime();
        DateTime dtPayPeriodTo = new DateTime();

        if (empList.Count > 0)
        {
            foreach (EEmpPersonalInfo o in empList)
            {
                if (strEmpList == string.Empty)
                    strEmpList = ((EEmpPersonalInfo)o).EmpID.ToString();
                else
                    strEmpList += "_" + ((EEmpPersonalInfo)o).EmpID.ToString();

            }
            if (DateTime.TryParse(PayPeriodFr.Value, out dtPayPeriodFr) && DateTime.TryParse(PayPeriodTo.Value, out dtPayPeriodTo))
                strPayPeriodRequest = "&PayPeriodFr=" + dtPayPeriodFr.Ticks + "&PayPeriodTo=" + dtPayPeriodTo.Ticks;
            else
                errors.addError("Invalid Date Format"); 
        }
        else
            errors.addError("Employee or Payroll Batch not selected");

        if (errors.isEmpty())
        {
            try
            {
                HROne.MPFFile.MPFRemittanceStatementProcessFandV rpt = new HROne.MPFFile.MPFRemittanceStatementProcessFandV(dbConn, empList, m_mpfPlanID, m_orsoPlanID, dtPayPeriodFr, dtPayPeriodTo, txtChequeNo.Text.ToUpper());

                //string reportFileName = WebUtils.GetLocalizedReportFile(Server.MapPath("~/Report_Payroll_MPFRemittanceStatement.rpt"));
                //string ExportFormat = ((Button)sender).CommandArgument;
                string exportFileName = System.IO.Path.GetTempFileName();
                exportFileName += ".xls";
                System.IO.File.Delete(exportFileName);
                HROne.Export.ExcelExport export = new HROne.Export.ExcelExport(exportFileName);

                rpt.GenerateExcelReport(exportFileName);

                WebUtils.TransmitFile(Response, exportFileName, "MPFRemittanceStatement_" + AppUtils.ServerDateTime().ToString("yyyyMMddHHmmss") + ".xls", true);
                Response.End();
            }
            catch (Exception ex)
            {
                errors.addError(ex.Message);
            }
        }
    }

    protected void btnRefreshEmpList_Click(object sender, EventArgs e)
    {

    }

    protected void PayPeriod_Changed(object sender, EventArgs e)
    {
        DateTime dtPeriodFr;

        if (DateTime.TryParse(PayPeriodFr.Value, out dtPeriodFr))
        {
            if (PayPeriodTo.Value == "")
            {
                PayPeriodTo.Value = dtPeriodFr.AddMonths(1).AddDays(-1).ToString("yyyy-MM-dd");
            }
        }

        //DateTime dtPeriodFr = new DateTime(), dtPeriodTo = new DateTime();
        //DateTime.TryParse(PayPeriodFr.Value, out dtPeriodFr);
        //DateTime.TryParse(PayPeriodTo.Value, out dtPeriodTo);
        //if (!dtPeriodFr.Ticks.Equals(0) && !dtPeriodTo.Ticks.Equals(0)) 
        //{
        //    if (dtPeriodFr >= dtPeriodTo)
        //    {
        //        if (sender == PayPeriodFr.TextBox)
        //            PayPeriodTo.Value = dtPeriodFr.AddMonths(DefaultMonthPeriod).AddDays(-1).ToString("yyyy-MM-dd");
        //        else if (sender == PayPeriodTo.TextBox)
        //            PayPeriodFr.Value = dtPeriodTo.AddDays(1).AddMonths(-DefaultMonthPeriod).ToString("yyyy-MM-dd");
        //    }
        //}
        //else
        //{
        //    if (sender == PayPeriodFr.TextBox && !dtPeriodFr.Ticks.Equals(0) && dtPeriodTo.Ticks.Equals(0))
        //    {
        //        PayPeriodTo.Value = dtPeriodFr.AddMonths(DefaultMonthPeriod).AddDays(-1).ToString("yyyy-MM-dd");

        //    }
        //    else if (sender == PayPeriodTo.TextBox && !dtPeriodTo.Ticks.Equals(0) && dtPeriodFr.Ticks.Equals(0))
        //        PayPeriodFr.Value = dtPeriodTo.AddDays(1).AddMonths(-DefaultMonthPeriod).ToString("yyyy-MM-dd");
        //}
    }
    // Start "fix cannot generate Bank File problem in iMgr", Ricky So, 2015-05-20

    public int DefaultMonthPeriod
    {
        set { hiddenFieldDefaultMonthPeriod.Value = value.ToString(); }
        get { return int.Parse(hiddenFieldDefaultMonthPeriod.Value); }
    }
}
