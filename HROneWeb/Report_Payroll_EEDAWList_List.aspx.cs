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
using CrystalDecisions.Shared;

public partial class Report_Payroll_EEDAWList_List : HROneWebPage
{
    private const string FUNCTION_CODE = "RPT203";
    protected DBManager db = EEmpPersonalInfo.db;
    protected SearchBinding binding;
    protected ListInfo info;
    protected DataView view;
    

    protected void Page_Load(object sender, EventArgs e)
    {
       
        if (!WebUtils.CheckAccess(Response, Session, FUNCTION_CODE, WebUtils.AccessLevel.Read))
            return;

        

        binding = new SearchBinding(dbConn, db);
        //binding.add(new LikeSearchBinder(EmpNo, "EmpNo"));
        //binding.add(new LikeSearchBinder(EmpName, "EmpEngSurname", "EmpEngOtherName", "EmpChiFullName"));
        //binding.add(new FieldDateRangeSearchBinder(JoinDateFrom, JoinDateTo, "EmpDateOfJoin").setUseCurDate(false));
        //binding.add(new DropDownVLSearchBinder(EmpStatus, "EmpStatus", EEmpPersonalInfo.VLEmpStatus).setLocale(ci));
        //binding.add(new DropDownVLSearchBinder((DropDownList)EmployeeSearchControl1.AdditionElementControl.FindControl("PayGroup"), "pp.PayGroupID", EPayrollGroup.VLPayrollGroup).setLocale(ci));
        binding.initValues("EmpStatus", null, EEmpPersonalInfo.VLEmpStatus, ci);
        binding.init(DecryptedRequest, null);

        info = ListFooter.ListInfo;

        HROne.Common.WebUtility.WebControlsLocalization(this, this.Controls);

    }
    protected void Page_PreRender(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            view = loadEmpData(info, db, Repeater);
        }

    }

    public DataView loadEmpData(ListInfo info, DBManager db, Repeater repeater)
    {
        //  Skip checking payroll status since NEW transfer company staff does not have payroll information

        //DBFilter filter = binding.createFilter();

        ////if (info != null && info.orderby != null && !info.orderby.Equals(""))
        ////    filter.add(info.orderby, info.order);

        string select = " e.* ";
        string from = "from [" + db.dbclass.tableName + "]  e ";

        //IN inTerm = new IN("e.EmpID", "Select ep.EmpID from EmpPayroll ep, PayrollPeriod pp ", filter);

        //filter.add(new MatchField("e.EmpID", "ep.EmpID"));
        //filter.add(new MatchField("ep.PayperiodID", "pp.PayperiodID"));
        //filter.add(new Match("ep.EmpPayStatus", "C"));

        //DateTime dtPayPeriodFr;
        //DateTime dtPayPeriodTo;
        //if (DateTime.TryParse(((WebDatePicker)EmployeeSearchControl1.AdditionElementControl.FindControl("AsOfDate")).Value, out dtPayPeriodTo))
        //{
        //    dtPayPeriodFr = dtPayPeriodTo.AddMonths(-12);
        //    filter.add(new Match("pp.PayPeriodFr", "<=", dtPayPeriodTo));
        //    filter.add(new Match("pp.PayPeriodTo", ">=", dtPayPeriodFr));
        //}
        //filter.add("EmpNo", true);

        DBFilter resultFilter = new DBFilter();
        //resultFilter.add(inTerm);

        DBFilter empInfoFilter = EmployeeSearchControl1.GetEmpInfoFilter(AppUtils.ServerDateTime(), AppUtils.ServerDateTime());
        empInfoFilter.add(new MatchField("e.EmpID", "ee.EmpID"));
        resultFilter.add(new Exists(EEmpPersonalInfo.db.dbclass.tableName + " ee", empInfoFilter));

        DataTable table = resultFilter.loadData(dbConn, null, select, from);
        table = EmployeeSearchControl1.FilterEncryptedEmpInfoField(table, info);

        view = new DataView(table);
        
        //if (info != null)
        //{
        //    info.loadPageList(null, PrevPage, NextPage, FirstPage, LastPage);

        //    CurPage.Value = info.page.ToString();
        //    NumPage.Value = info.numPage.ToString();
        //}
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
        view = loadEmpData(info, db, Repeater);

    }
    protected void Reset_Click(object sender, EventArgs e)
    {
        binding.clear();
        EmployeeSearchControl1.Reset();
        info.page = 0;
        view = loadEmpData(info, db, Repeater);

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

        view = loadEmpData(info, db, Repeater);

    }
    protected void Repeater_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        DataRowView row = (DataRowView)e.Item.DataItem;
        CheckBox cb = (CheckBox)e.Item.FindControl("ItemSelect");
        WebFormUtils.LoadKeys(db, row, cb);
    }

    protected void btnGenerate_Click(object sender, EventArgs e)
    {
        PageErrors errors = PageErrors.getErrors(db, Page.Master);
        errors.clear();

        ArrayList empList = new ArrayList();
        DateTime dtAsOfDate = new DateTime();
        ArrayList payBatchList = new ArrayList();

        foreach (RepeaterItem i in Repeater.Items)
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
        //String strPayPeriodRequest = string.Empty;
        if (empList.Count > 0)
        {

            //foreach (EEmpPersonalInfo o in empList)
            //{
            //    if (strEmpList == string.Empty)
            //        strEmpList = ((EEmpPersonalInfo)o).EmpID.ToString();
            //    else
            //        strEmpList += "_" + ((EEmpPersonalInfo)o).EmpID.ToString();

            //}

            if (DateTime.TryParse(((WebDatePicker)EmployeeSearchControl1.AdditionElementControl.FindControl("AsOfDate")).Value, out dtAsOfDate))
            {
                //strPayPeriodRequest = "&AsOfDate=" + dtAsOfDate.Ticks;
            }
            else
                errors.addError("As Of Date", "Invalid Date Format");


            //            errors.addError("Complete");
            //Response.Write("<script>alert('Completed'); </script>");
        }
        else
            errors.addError("Employee not selected");

        if (errors.isEmpty())
        {
            HROne.Reports.Payroll.EEDAWListReportProcess rpt = new HROne.Reports.Payroll.EEDAWListReportProcess(dbConn, empList, dtAsOfDate);

            string reportFileName = WebUtils.GetLocalizedReportFile(Server.MapPath("~/Report_Payroll_EEDAWList.rpt"));

            WebUtils.ReportExport(dbConn, user, errors, lblReportHeader.Text, Response, rpt, reportFileName, ((Button)sender).CommandArgument, "EEDAWList", true);
            //Server.Transfer("Report_Payroll_EEDAWList_View.aspx?"
            //+ "EmpID=" + strEmpList
            //+ strPayPeriodRequest
            //);
        }
        //        emploadData(empInfo, EEmpPayroll.db, empRepeater);
    }

}
