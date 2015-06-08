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
//using perspectivemind.validation;
//using CrystalDecisions.Shared;

using HROne.Lib.Entities;

public partial class Report_Payroll_DiscrepancyList_List : HROneWebPage
{
    protected DBManager db = EEmpPersonalInfo.db;
    protected SearchBinding binding;
    protected ListInfo info;
    protected DataView view;
    

    protected void Page_Load(object sender, EventArgs e)
    {
       
        if (!WebUtils.CheckAccess(Response, Session, "RPT206", WebUtils.AccessLevel.Read))
            return;

        

        binding = new SearchBinding(dbConn, db);
        //binding.add(new LikeSearchBinder(EmpNo, "EmpNo"));
        //binding.add(new LikeSearchBinder(EmpEngSurname, "EmpEngSurname"));
        //binding.add(new LikeSearchBinder(EmpEngOtherName, "EmpEngOtherName"));
        //binding.add(new LikeSearchBinder(EmpChiFullName, "EmpChiFullName"));
        //binding.add(new LikeSearchBinder(EmpAlias, "EmpAlias"));
        //binding.add(new DropDownVLSearchBinder(EmpGender, "EmpGender", EPayrollGroup.VLPayrollGroup).setLocale(ci));

        //binding.add(new FieldDateRangeSearchBinder(JoinDateFrom, JoinDateTo, "EmpDateOfJoin").setUseCurDate(false));

       
        binding.init(DecryptedRequest, null);

        SearchBinding tempBinding = new SearchBinding(dbConn, db);
        tempBinding.add(new DropDownVLSearchBinder(ProcessMonth, "", Values.VLMonth));
        tempBinding.add(new DropDownVLSearchBinder(ReferenceMonth, "", Values.VLMonth));

        tempBinding.init(DecryptedRequest, null);

        info = ListFooter.ListInfo;

        HROne.Common.WebUtility.WebControlsLocalization(this, this.Controls);

    }
    protected void Page_PreRender(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            view = loadData(info, db, Repeater);
        }


    }

    public DataView loadData(ListInfo info, DBManager db, Repeater repeater)
    {
        DBFilter filter = binding.createFilter();

        //if (info != null && info.orderby != null && !info.orderby.Equals(""))
        //    filter.add(info.orderby, info.order);


        string select = " e.*";
        string from = "from [" + db.dbclass.tableName + "] e ";//LEFT JOIN " + EEmpPositionInfo.db.dbclass.tableName + " p ON c.EmpID=p.EmpID AND p.EmpPosEffTo IS NULL";
        filter.add(WebUtils.AddRankFilter(Session, "e.EmpID", true));
        //foreach (RepeaterItem item in HierarchyLevel.Items)
        //{
        //    DBFilter sub = null;
        //    DropDownList c = (DropDownList)item.FindControl("HElementID");
        //    string v = c.SelectedValue;
        //    if (!v.Equals(""))
        //    {
        //        if (sub == null)
        //        {
        //            sub = new DBFilter();
        //            sub.add(new MatchField("p.EmpPosID", "EmpPosID"));
        //        }
        //        sub.add(new Match("HLevelID", c.Attributes["HLevelID"]));
        //        sub.add(new Match("HElementID", v));
        //    }
        //    if (sub != null)
        //        filter.add(new Exists(EEmpHierarchy.db.dbclass.tableName, sub));
        //}

        //DateTime dtPeriodFr= new DateTime();
        //DateTime dtPeriodTo=new DateTime();
        //int intProcessYear = 0, intReferenceYear = 0, intProcessMonth = 0, intReferenceMonth = 0;

        //if (int.TryParse(((TextBox)EmployeeSearchControl1.AdditionElementControl.FindControl("ProcessYear")).Text, out intProcessYear) && int.TryParse(((DropDownList)EmployeeSearchControl1.AdditionElementControl.FindControl("ProcessMonth")).SelectedValue, out intProcessMonth))
        //{
        //    dtPeriodTo = new DateTime(intProcessYear, intProcessMonth, DateTime.DaysInMonth(intProcessYear, intProcessMonth));
        //}
        //if (int.TryParse(((TextBox)EmployeeSearchControl1.AdditionElementControl.FindControl("ReferenceYear")).Text, out intReferenceYear) && int.TryParse(((DropDownList)EmployeeSearchControl1.AdditionElementControl.FindControl("ReferenceMonth")).SelectedValue, out intReferenceMonth))
        //{
        //    dtPeriodFr = new DateTime(intReferenceYear, intReferenceMonth, DateTime.DaysInMonth(intReferenceYear, intReferenceMonth));
        //}
        //if (dtPeriodFr>dtPeriodTo)
        //    if (dtPeriodTo.Ticks != 0)
        //    {
        //        DateTime dtTemp = dtPeriodTo;
        //        dtPeriodTo = dtPeriodFr;
        //        dtPeriodFr = dtTemp;
        //    }
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
        info.page = 0;
        view = loadData(info, db, Repeater);

    }
    protected void Reset_Click(object sender, EventArgs e)
    {
        binding.clear();
        EmployeeSearchControl1.Reset();
        info.page = 0;
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
    }


    protected void btnGenerate_Click(object sender, EventArgs e)
    {
        PageErrors errors = PageErrors.getErrors(db, Page.Master);
        errors.clear();

        int intProcessYear = 0, intReferenceYear = 0, intProcessMonth = 0, intReferenceMonth = 0;
        string strEmpList = string.Empty;

            ArrayList empList = new ArrayList();

        if (int.TryParse(ProcessYear.Text, out intProcessYear) && int.TryParse(ReferenceYear.Text, out intReferenceYear) && int.TryParse(ReferenceMonth.SelectedValue, out intReferenceMonth) && int.TryParse(ProcessMonth.SelectedValue, out intProcessMonth))
        {

            foreach (RepeaterItem item in Repeater.Items)
            {
                CheckBox cb = (CheckBox)item.FindControl("ItemSelect");
                if (cb.Checked)
                {
                    EEmpPersonalInfo et = new EEmpPersonalInfo();
                    WebFormUtils.GetKeys(db, et, cb);
                    empList.Add(et);
                }
            }
            if (empList.Count > 0)
            {

                foreach (EEmpPersonalInfo o in empList)
                {
                    if (strEmpList == string.Empty)
                        strEmpList = o.EmpID.ToString();
                    else
                        strEmpList += "_" + o.EmpID.ToString();

                }
            }
            else
                errors.addError("Employee not selected");
        }
        else
            errors.addError("Invalid Process Month/Year or Reference Month/Year");

        if (errors.isEmpty())
        {
            HROne.Reports.Payroll.DiscrepancyListReportProcess rpt = new HROne.Reports.Payroll.DiscrepancyListReportProcess(dbConn, empList, intProcessYear, intProcessMonth, intReferenceYear, intReferenceMonth);

            string reportFileName = WebUtils.GetLocalizedReportFile(Server.MapPath("~/Report_Payroll_DiscrepancyList.rpt"));

            WebUtils.ReportExport(dbConn, user, errors, lblReportHeader.Text, Response, rpt, reportFileName, ((Button)sender).CommandArgument, "DiscrepancyList", true);
            //Server.Transfer("Report_Payroll_DiscrepancyList_View.aspx?"
            //+ "EmpID=" + strEmpList
            //+ "&ProcessMonth=" + intProcessMonth
            //+ "&ProcessYear=" + intProcessYear
            //+ "&ReferenceMonth=" + intReferenceMonth
            //+ "&ReferenceYear=" + intReferenceYear
            //);
        }
        //        emploadData(empInfo, EEmpPayroll.db, empRepeater);

    }
}
