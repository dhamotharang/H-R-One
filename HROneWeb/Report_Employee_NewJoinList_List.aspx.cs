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

public partial class Report_Employee_NewJoinList_List : HROneWebPage
{
    private const string FUNCTION_CODE = "RPT112";
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

        DateTime dtEmpDateOfJoinFrom = new DateTime(), dtEmpDateOfJoinTo = new DateTime();
        if (DateTime.TryParse(EmpDateOfJoinFrom.Value, out dtEmpDateOfJoinFrom))
            filter.add(new Match("e.EmpDateOfJoin", ">=", dtEmpDateOfJoinFrom));
            
        if (DateTime.TryParse(EmpDateOfJoinTo.Value, out dtEmpDateOfJoinTo))
            filter.add(new Match("e.EmpDateOfJoin", "<=", dtEmpDateOfJoinTo));

        filter.add(new NullTerm("NOT e.EmpDateOfJoin"));
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

    protected void EmpDateOfJoin_Changed(object sender, EventArgs e)
    {
        int DefaultMonthPeriod = 1;
        DateTime dtPeriodFr = new DateTime(), dtPeriodTo = new DateTime();
        DateTime.TryParse(EmpDateOfJoinFrom.Value, out dtPeriodFr);
        DateTime.TryParse(EmpDateOfJoinTo.Value, out dtPeriodTo);
        if (!dtPeriodFr.Ticks.Equals(0) && !dtPeriodTo.Ticks.Equals(0))
        {
            if (dtPeriodFr >= dtPeriodTo)
            {
                if (sender == EmpDateOfJoinFrom.TextBox)
                    EmpDateOfJoinTo.Value = dtPeriodFr.AddMonths(DefaultMonthPeriod).AddDays(-1).ToString("yyyy-MM-dd");
                else if (sender == EmpDateOfJoinTo.TextBox)
                    EmpDateOfJoinFrom.Value = dtPeriodTo.AddDays(1).AddMonths(-DefaultMonthPeriod).ToString("yyyy-MM-dd");
            }
        }
        else
        {
            if (sender == EmpDateOfJoinFrom.TextBox && !dtPeriodFr.Ticks.Equals(0) && dtPeriodTo.Ticks.Equals(0))
            {
                EmpDateOfJoinTo.Value = dtPeriodFr.AddMonths(DefaultMonthPeriod).AddDays(-1).ToString("yyyy-MM-dd");
                EmpDateOfJoinTo.Focus();
            }
            else if (sender == EmpDateOfJoinTo.TextBox && !dtPeriodTo.Ticks.Equals(0) && dtPeriodFr.Ticks.Equals(0))
                EmpDateOfJoinFrom.Value = dtPeriodTo.AddDays(1).AddMonths(-DefaultMonthPeriod).ToString("yyyy-MM-dd");
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

        DateTime dtEmpDateOfJoinFrom = new DateTime(), dtEmpDateOfJoinTo = new DateTime();
        if (!DateTime.TryParse(EmpDateOfJoinFrom.Value, out dtEmpDateOfJoinFrom) || !DateTime.TryParse(EmpDateOfJoinTo.Value, out dtEmpDateOfJoinTo))
            errors.addError(string.Format(HROne.Translation.PageErrorMessage.ERROR_FIELD_REQUIRED, new string[] { ProbationLastDateRangeHeader.Text }));


        ArrayList empList = WebUtils.SelectedRepeaterItemToBaseObjectList(EEmpPersonalInfo.db, Repeater, "ItemSelect");
        if (empList.Count <= 0)
            errors.addError(HROne.Translation.PageErrorMessage.ERROR_NO_EMPLOYEE_SELECTED);

        if (errors.isEmpty())
        {
            HROne.Reports.Employee.EmployeeNewJoinListProcess rpt = new HROne.Reports.Employee.EmployeeNewJoinListProcess(dbConn, empList, dtEmpDateOfJoinFrom, dtEmpDateOfJoinTo);
            string reportFileName = WebUtils.GetLocalizedReportFile(Server.MapPath("~/Report_Employee_NewJoinList.rpt"));
            //WebUtils.RegisterReportExport(this, rpt, reportFileName, ((Button)sender).CommandArgument, "NewJoin", true);
            WebUtils.ReportExport(dbConn, user, errors, lblReportHeader.Text, Response, rpt, reportFileName, ((Button)sender).CommandArgument, "NewJoin", true);
            //Server.Transfer("Report_Employee_TerminationList_View.aspx?"
            //+ "EmpTermID=" + strTermEmpList
            //);
        }
        //        emploadData(empInfo, EEmpPayroll.db, empRepeater);
    }

}
