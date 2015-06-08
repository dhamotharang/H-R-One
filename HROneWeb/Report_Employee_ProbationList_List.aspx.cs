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

using HROne.Lib.Entities;

public partial class Report_Employee_ProbationList : HROneWebPage
{
    private const string FUNCTION_CODE = "RPT113";
    protected DBManager db = EEmpPersonalInfo.db;
    protected SearchBinding binding;
    protected ListInfo info;
    protected DataView view;
    

    protected void Page_Load(object sender, EventArgs e)
    {
       
        if (!WebUtils.CheckAccess(Response, Session, FUNCTION_CODE, WebUtils.AccessLevel.Read))
            return;

        binding = new SearchBinding(dbConn, db);
       
        binding.init(DecryptedRequest, null);

        if (!IsPostBack)
            WebFormUtils.loadValues(dbConn, EmpProbaUnit, Values.VLEmpUnit, null, null, string.Empty, "combobox.notselected");

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

        DateTime dtLastProbationDateFrom = new DateTime(), dtLastProbationDateTo = new DateTime();


        OR orEmpProbaLastDate = new OR();
        AND andEmpProbaLastDateRange = new AND();
        andEmpProbaLastDateRange.add(new MatchField("e.EmpProbaLastDate", ">=", "e.EmpDateOfJoin"));
        if (DateTime.TryParse(LastProbationDateFrom.Value, out dtLastProbationDateFrom))
        {
            //andEmpProbaLastDateRange = new AND();
            andEmpProbaLastDateRange.add(new Match("e.EmpProbaLastDate", ">=", dtLastProbationDateFrom));
        }
        if (DateTime.TryParse(LastProbationDateTo.Value, out dtLastProbationDateTo))
        {
            //if (andEmpProbaLastDateRange == null)
            //    andEmpProbaLastDateRange = new AND();
            andEmpProbaLastDateRange.add(new Match("e.EmpProbaLastDate", "<=", dtLastProbationDateTo));
        }
        if (andEmpProbaLastDateRange != null)
            orEmpProbaLastDate.add(andEmpProbaLastDateRange);
        orEmpProbaLastDate.add(new NullTerm("e.EmpProbaLastDate"));
        filter.add(orEmpProbaLastDate);

        int probationPeriod = -1;
            if (int.TryParse(EmpProbaPeriod.Text, out probationPeriod))
            {
                if (!string.IsNullOrEmpty(EmpProbaUnit.SelectedValue))
                {
                    filter.add(new Match("e.EmpProbaPeriod", probationPeriod));
                    filter.add(new Match("e.EmpProbaUnit", EmpProbaUnit.SelectedValue));
                }
            }
            else if (!string.IsNullOrEmpty(EmpProbaPeriod.Text))
                EmpProbaPeriod.Text = string.Empty;


        DBFilter empInfoFilter = EmployeeSearchControl1.GetEmpInfoFilter(AppUtils.ServerDateTime(), AppUtils.ServerDateTime());
        empInfoFilter.add(new MatchField("e.EmpID", "ee.EmpID"));
        filter.add(new Exists(EEmpPersonalInfo.db.dbclass.tableName + " ee", empInfoFilter));

        DataTable table = filter.loadData(dbConn, null, select, from);
        table = FilterNullProbationLastDate(table, dtLastProbationDateFrom, dtLastProbationDateTo);
        table = EmployeeSearchControl1.FilterEncryptedEmpInfoField(table, info);
        view = new DataView(table);
        if (repeater != null)
        {
            repeater.DataSource = view;
            repeater.DataBind();
        }

        return view;
    }

    protected void LastProbationDate_Changed(object sender, EventArgs e)
    {
        int DefaultMonthPeriod = 1;
        DateTime dtPeriodFr = new DateTime(), dtPeriodTo = new DateTime();
        DateTime.TryParse(LastProbationDateFrom.Value, out dtPeriodFr);
        DateTime.TryParse(LastProbationDateTo.Value, out dtPeriodTo);
        if (!dtPeriodFr.Ticks.Equals(0) && !dtPeriodTo.Ticks.Equals(0))
        {
            if (dtPeriodFr >= dtPeriodTo)
            {
                if (sender == LastProbationDateFrom.TextBox)
                    LastProbationDateTo.Value = dtPeriodFr.AddMonths(DefaultMonthPeriod).AddDays(-1).ToString("yyyy-MM-dd");
                else if (sender == LastProbationDateTo.TextBox)
                    LastProbationDateFrom.Value = dtPeriodTo.AddDays(1).AddMonths(-DefaultMonthPeriod).ToString("yyyy-MM-dd");
            }
        }
        else
        {
            if (sender == LastProbationDateFrom.TextBox && !dtPeriodFr.Ticks.Equals(0) && dtPeriodTo.Ticks.Equals(0))
            {
                LastProbationDateTo.Value = dtPeriodFr.AddMonths(DefaultMonthPeriod).AddDays(-1).ToString("yyyy-MM-dd");
                LastProbationDateTo.Focus();
            }
            else if (sender == LastProbationDateTo.TextBox && !dtPeriodTo.Ticks.Equals(0) && dtPeriodFr.Ticks.Equals(0))
                LastProbationDateFrom.Value = dtPeriodTo.AddDays(1).AddMonths(-DefaultMonthPeriod).ToString("yyyy-MM-dd");
        }
        view = loadData(info, db, Repeater);

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
        WebFormUtils.LoadKeys(EEmpPersonalInfo.db, row, cb);
        cb.Checked = true;
    }


    protected void btnGenerate_Click(object sender, EventArgs e)
    {
        PageErrors errors = PageErrors.getErrors(db, Page.Master);
        errors.clear();

        DateTime dtLastProbationDateFrom = new DateTime(), dtLastProbationDateTo = new DateTime();
        if (!DateTime.TryParse(LastProbationDateFrom.Value, out dtLastProbationDateFrom) || !DateTime.TryParse(LastProbationDateTo.Value, out dtLastProbationDateTo))
            errors.addError(string.Format(HROne.Translation.PageErrorMessage.ERROR_FIELD_REQUIRED, new string[] { ProbationLastDateRangeHeader.Text }));

        int probationPeriod = -1;
        if (!int.TryParse(EmpProbaPeriod.Text, out probationPeriod))
            probationPeriod = -1;

        ArrayList empList = WebUtils.SelectedRepeaterItemToBaseObjectList(EEmpPersonalInfo.db, Repeater, "ItemSelect");
        if (empList.Count <= 0)
            errors.addError(HROne.Translation.PageErrorMessage.ERROR_NO_EMPLOYEE_SELECTED);

        if (errors.isEmpty())
        {
            HROne.Reports.Employee.EmployeeProbationListProcess rpt = new HROne.Reports.Employee.EmployeeProbationListProcess(dbConn, empList, dtLastProbationDateFrom, dtLastProbationDateTo, probationPeriod, EmpProbaUnit.SelectedValue);
            string reportFileName = WebUtils.GetLocalizedReportFile(Server.MapPath("~/Report_Employee_ProbationList.rpt"));
            //WebUtils.RegisterReportExport(this, rpt, reportFileName, ((Button)sender).CommandArgument, "ProbationList", true);
            WebUtils.ReportExport(dbConn, user, errors, lblReportHeader.Text, Response, rpt, reportFileName, ((Button)sender).CommandArgument, "ProbationList", true);

            //Server.Transfer("Report_Employee_TerminationList_View.aspx?"
            //+ "EmpTermID=" + strTermEmpList
            //);
        }
        //        emploadData(empInfo, EEmpPayroll.db, empRepeater);
    }

    protected DataTable FilterNullProbationLastDate(DataTable table, DateTime PeriodFrom, DateTime PeriodTo)
    {

        //  To Resolve last probation inputted in very old version of HROne with empty value
        DataRow[] rowList = table.Select("EmpProbaLastDate IS NULL");

        foreach (DataRow row in rowList)
        {
            if (!row.IsNull("EmpDateOfJoin") && !row.IsNull("EmpProbaPeriod") && !row.IsNull("EmpProbaUnit"))
            {
                DateTime m_EmpDateOfJoin = (DateTime)row["EmpDateOfJoin"];
                int EmpProbaPeriod = (int)row["EmpProbaPeriod"];
                string EmpProbaUnit = (string)row["EmpProbaUnit"];

                if (EmpProbaPeriod > 0)
                {
                    DateTime tmpProbaLastDate = new DateTime();
                    if (EmpProbaUnit.Equals("D"))
                        tmpProbaLastDate = m_EmpDateOfJoin.AddDays(EmpProbaPeriod).AddDays(-1);
                    else if (EmpProbaUnit.Equals("M"))
                        tmpProbaLastDate = m_EmpDateOfJoin.AddMonths(EmpProbaPeriod).AddDays(-1);
                    if (!tmpProbaLastDate.Ticks.Equals(0))
                        row["EmpProbaLastDate"] = tmpProbaLastDate;
                }

            }
        }
        DataView view = new DataView(table);
        string filter = string.Empty;
        if (!PeriodFrom.Ticks.Equals(0)) 
            filter = "EmpProbaLastDate >=#" + PeriodFrom.ToString("yyyy-MM-dd") + "# ";
        if (!PeriodTo.Ticks.Equals(0))
            if (string.IsNullOrEmpty(filter))
                filter = "EmpProbaLastDate >=#" + PeriodFrom.ToString("yyyy-MM-dd") + "# ";
            else
                filter += "AND EmpProbaLastDate >=#" + PeriodFrom.ToString("yyyy-MM-dd") + "# ";
        if (!string.IsNullOrEmpty(filter))
        {
            view.RowFilter = filter;
            return view.ToTable();
        }
        else
            return table;
    }
}
