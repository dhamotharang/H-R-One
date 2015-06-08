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

public partial class Report_LeaveSummary : HROneWebPage
{
    private const string FUNCTION_CODE = "RPT132";
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
        //binding.add(new LikeSearchBinder(EmpEngSurname, "EmpEngSurname"));
        //binding.add(new LikeSearchBinder(EmpEngOtherName, "EmpEngOtherName"));
        //binding.add(new LikeSearchBinder(EmpChiFullName, "EmpChiFullName"));
        //binding.add(new LikeSearchBinder(EmpAlias, "EmpAlias"));
        //binding.add(new DropDownVLSearchBinder(EmpGender, "EmpGender", EPayrollGroup.VLPayrollGroup).setLocale(ci));

        //binding.add(new FieldDateRangeSearchBinder(JoinDateFrom, JoinDateTo, "EmpDateOfJoin").setUseCurDate(false));
        //binding.add(new DropDownVLSearchBinder(EmpStatus, "EmpStatus", EEmpPersonalInfo.VLEmpStatus).setLocale(ci));
        binding.initValues("EmpStatus", null, EEmpPersonalInfo.VLEmpStatus, ci);
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


        string select = "e.*";
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
        EmployeeSearchControl1.EmpStatusValue = "A";
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

        DateTime dtAsOfDate = new DateTime();
        if (!DateTime.TryParse(AsOfDate.Value, out dtAsOfDate))
        {
            if (string.IsNullOrEmpty(AsOfDate.Value))
                dtAsOfDate = AppUtils.ServerDateTime().Date;
            else
                return;
        }
        EEmpPersonalInfo p = new EEmpPersonalInfo();
        ArrayList values = new ArrayList();
        foreach (RepeaterItem item in Repeater.Items)
        {
            CheckBox cb = (CheckBox)item.FindControl("ItemSelect");
            if (cb.Checked)
            {
                WebFormUtils.GetKeys(db, p, cb);
                values.Add(p.EmpID);
            }
        }
        if (values.Count <= 0)
            errors.addError("Employee not selected");

        if (errors.isEmpty())
        {

            HROne.Reports.Employee.LeaveSummaryProcess rpt = new HROne.Reports.Employee.LeaveSummaryProcess(dbConn, values, dtAsOfDate);
            string reportFileName = WebUtils.GetLocalizedReportFile(Server.MapPath("~/Report_Employee_LeaveSummary.rpt"));
            WebUtils.ReportExport(dbConn, user, errors, lblReportHeader.Text, Response, rpt, reportFileName, ((Button)sender).CommandArgument, "LeaveSummary", true);
        }

        //Session["AsOfDate"] = AsOfDate.Text;
        //Session["Report_EmployeeList"] = values;
        //HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "Report_LeaveSummary_View.aspx");
    }

}
