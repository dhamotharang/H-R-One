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

public partial class Report_Payroll_PFundStatement_List : HROneWebPage
{
    private const string FUNCTION_CODE = "RPT253";
    protected SearchBinding empSBinding;
    public DBManager db = EPayrollGroup.db;
    public EPayrollGroup obj;

    protected ListInfo empInfo;
    protected DataView empView;
    

    protected void Page_Load(object sender, EventArgs e)
    {
       
        if (!WebUtils.CheckAccess(Response, Session, FUNCTION_CODE, WebUtils.AccessLevel.Read))
            return;

        empInfo = ListFooter.ListInfo;

        HROne.Common.WebUtility.WebControlsLocalization(this, this.Controls);

        empSBinding = new SearchBinding(dbConn, EEmpPersonalInfo.db);
        empSBinding.add(new DropDownVLSearchBinder(ORSOPlanID, "ORSOPlanID",EORSOPlan.VLORSOPlan));
        empSBinding.init(DecryptedRequest, null);        

        // Start 0000140, Ricky So, 201/12/15
        //if (!Page.IsPostBack)
        //{
        //    empView = emploadData(empInfo, EEmpPayroll.db, empRepeater);
        //}
        // End 0000140, Ricky So, 201/12/15
    
    }

    //// Start 0000140, Ricky So, 201/12/15
    //protected void Page_Prerender(object sender, EventArgs e)
    //{
    //    if (!Page.IsPostBack)
    //    {
    //        empView = emploadData(empInfo, EEmpPayroll.db, empRepeater);
    //    }
    //}
    //// End 0000140, Ricky So, 201/12/15

    public DataView emploadData(ListInfo info, DBManager db, Repeater repeater)
    {
        DBFilter filter = new DBFilter();// empSBinding.createFilter();

        //if (info != null && info.orderby != null && !info.orderby.Equals(""))
        //    filter.add(info.orderby, info.order);

        string select = "* ";
        string from = "from [" + EEmpPersonalInfo.db.dbclass.tableName + "] e ";


        DBFilter empPayrollFilter = new DBFilter();

        empPayrollFilter.add(new MatchField("e.EmpID", "ep.EmpID"));
        empPayrollFilter.add(new Match("ep.EmpPayStatus", "C"));
//        empPayrollFilter.add(new Match("ep.PayPeriodID", CurPayPeriodID));

        // Start 000140, Ricky So, 2014/12/15
        DBFilter empInfoFilter = EmployeeSearchControl1.GetEmpInfoFilter(AppUtils.ServerDateTime().Date, AppUtils.ServerDateTime().Date);
        empInfoFilter.add(new MatchField("e.EmpID", "ee.EmpID"));
        empPayrollFilter.add(new Exists(EEmpPersonalInfo.db.dbclass.tableName + " ee", empInfoFilter));
        // End 000140, Ricky So, 2014/12/15

        DateTime dtPayPeriodFr, dtPayPeriodTo;
        if (DateTime.TryParse(PayPeriodFr.Value, out dtPayPeriodFr) && DateTime.TryParse(PayPeriodTo.Value, out dtPayPeriodTo))
        {
            DBFilter payPeriodFilter = new DBFilter();

            // refer to Date To of payperiod
            payPeriodFilter.add(new Match("pp.PayPeriodTo", "<=", dtPayPeriodTo));
            payPeriodFilter.add(new Match("pp.PayPeriodTo", ">=", dtPayPeriodFr));
            empPayrollFilter.add(new IN("PayperiodID ", "Select payperiodID from PayrollPeriod pp", payPeriodFilter));
        }
        else
        {
            empPayrollFilter.add(new Match("1", "2"));
        }

        DBFilter mpfRecordFilter =new DBFilter();
        mpfRecordFilter.add(new Match("ORSOPlanID",ORSOPlanID.SelectedValue));
        empPayrollFilter.add(new IN("EmpPayrollID", "Select EmpPayrollID from ORSORecord", mpfRecordFilter));

        Exists exists = new Exists(EEmpPayroll.db.dbclass.tableName + " ep", empPayrollFilter);

        filter.add(exists);

        DataTable table = WebUtils.GetDataTableFromSelectQueryWithFilter(dbConn, select, from, filter, info);


        if (table.Rows.Count != 0)
        {
            ReportExportControl2.Visible = true;
            ReportExportControl1.Visible = true;
        }
        else
        {
            ReportExportControl2.Visible = false;
            ReportExportControl1.Visible = false;
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
        empView = emploadData(empInfo, db, empRepeater);

    }
    protected void Reset_Click(object sender, EventArgs e)
    {
        empSBinding.clear();
        empView = emploadData(empInfo, db, empRepeater);
    }
    protected void btnGenerate_Click(object sender, EventArgs e)
    {
        PageErrors errors = PageErrors.getErrors(db, Page.Master);
        ArrayList empList = new ArrayList();

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


        //string strEmpList = string.Empty;
        //string strPayBatchList = string.Empty;
        //string strPayPeriodRequest = string.Empty;
            DateTime dtPayPeriodFr= new DateTime();
            DateTime dtPayPeriodTo=new DateTime();
        if (empList.Count > 0 )
        {

            //foreach (EEmpPersonalInfo o in empList)
            //{
            //    if (strEmpList == string.Empty)
            //        strEmpList = ((EEmpPersonalInfo)o).EmpID.ToString();
            //    else
            //        strEmpList += "_" + ((EEmpPersonalInfo)o).EmpID.ToString();

            //}
            if (DateTime.TryParse(PayPeriodFr.Value, out dtPayPeriodFr) && DateTime.TryParse(PayPeriodTo.Value, out dtPayPeriodTo))
            {
                //strPayPeriodRequest = "&PayPeriodFr=" + dtPayPeriodFr.Ticks + "&PayPeriodTo=" + dtPayPeriodTo.Ticks;
            }
            else
                errors.addError("Invalid Date Format");


            //            error.getErrors().addError("Complete");
            //Response.Write("<script>alert('Completed'); </script>");
        }
        else
            errors.addError("Employee or Payroll Batch not selected");

        if (errors.isEmpty())
        {

            HROne.Reports.Payroll.PFundStatementProcess rpt = new HROne.Reports.Payroll.PFundStatementProcess(dbConn, empList, int.Parse(ORSOPlanID.SelectedValue), dtPayPeriodFr, dtPayPeriodTo);

            string reportFileName = WebUtils.GetLocalizedReportFile(Server.MapPath("~/Report_Payroll_PFundStatement.rpt"));

            WebUtils.ReportExport(dbConn, user, errors, lblReportHeader.Text, Response, rpt, reportFileName, ((Button)sender).CommandArgument, "PFundStatement", true);

            //Server.Transfer("Report_Payroll_PFundStatement_View.aspx?"
            //    + "ORSOPlanID=" + ORSOPlanID.SelectedValue
            //    + strPayPeriodRequest
            //    + "&EmpID=" + strEmpList);
        }
//        emploadData(empInfo, EEmpPayroll.db, empRepeater);
    }

    protected void btnRefreshEmpList_Click(object sender, EventArgs e)
    {

    }

    // Start 0000004, Miranda, 2014-06-19
    protected void PayPeriod_Changed(object sender, EventArgs e)
    {
        DateTime dtPeriodFr = new DateTime(), dtPeriodTo = new DateTime();
        DateTime.TryParse(PayPeriodFr.Value, out dtPeriodFr);
        DateTime.TryParse(PayPeriodTo.Value, out dtPeriodTo);
        if (!dtPeriodFr.Ticks.Equals(0) && !dtPeriodTo.Ticks.Equals(0))
        {
            if (dtPeriodFr >= dtPeriodTo)
            {
                if (sender == PayPeriodFr.TextBox)
                    PayPeriodTo.Value = dtPeriodFr.AddMonths(DefaultMonthPeriod).AddDays(-1).ToString("yyyy-MM-dd");
                else if (sender == PayPeriodTo.TextBox)
                    PayPeriodFr.Value = dtPeriodTo.AddDays(1).AddMonths(-DefaultMonthPeriod).ToString("yyyy-MM-dd");
            }
        }
        else
        {
            if (sender == PayPeriodFr.TextBox && !dtPeriodFr.Ticks.Equals(0) && dtPeriodTo.Ticks.Equals(0))
            {
                PayPeriodTo.Value = dtPeriodFr.AddMonths(DefaultMonthPeriod).AddDays(-1).ToString("yyyy-MM-dd");

            }
            else if (sender == PayPeriodTo.TextBox && !dtPeriodTo.Ticks.Equals(0) && dtPeriodFr.Ticks.Equals(0))
                PayPeriodFr.Value = dtPeriodTo.AddDays(1).AddMonths(-DefaultMonthPeriod).ToString("yyyy-MM-dd");
        }
        // Start 0000140, Ricky So, 201/12/15
        empView = emploadData(empInfo, db, empRepeater);
        // End 0000140, Ricky So, 201/12/15
    }

    public int DefaultMonthPeriod
    {
        set { hiddenFieldDefaultMonthPeriod.Value = value.ToString(); }
        get { return int.Parse(hiddenFieldDefaultMonthPeriod.Value); }
    }
    // End 0000004, Miranda, 2014-06-19
}
