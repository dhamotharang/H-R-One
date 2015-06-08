using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using HROne.DataAccess;
using HROne.Lib.Entities;

public partial class Report_Employee_Residency : HROneWebPage
{
    private const string FUNCTION_CODE = "RPT109";
    // Start 0000185, KuangWei, 2015-05-05
    protected DBManager db = EEmpPersonalInfo.db;
    protected SearchBinding binding;
    protected ListInfo info;
    protected DataView view;
    // End 0000185, KuangWei, 2015-05-05

    protected void Page_Load(object sender, EventArgs e)
    {

        if (!IsPostBack)
        {
            WebFormUtils.loadValues(dbConn, HLevelID, HROne.Lib.Entities.EHierarchyLevel.VLHierarchy, new DBFilter(), ci, null, null);
            //HLevelID.Items.RemoveAt(0);
        }
        if (HLevelID.Items.Count > 0)
            GroupBy.Items.FindByValue("HLevel").Enabled = true;
        else
            GroupBy.Items.FindByValue("HLevel").Enabled = false;

        if (GroupBy.SelectedValue.Equals("HLevel", StringComparison.CurrentCultureIgnoreCase))
            HLevelID.Visible = true;
        else
            HLevelID.Visible = false;

        if (!WebUtils.CheckAccess(Response, Session, FUNCTION_CODE, WebUtils.AccessLevel.Read))
            return;

        // Start 0000185, KuangWei, 2015-05-05
        binding = new SearchBinding(dbConn, db);
        binding.init(DecryptedRequest, null);

        info = ListFooter.ListInfo;
        // End 0000185, KuangWei, 2015-05-05

        HROne.Common.WebUtility.WebControlsLocalization(this, this.Controls);

    }
    protected void btnGenerate_Click(object sender, EventArgs e)
    {
        HROne.DataAccess.PageErrors errors = HROne.DataAccess.PageErrors.getErrors(null, Page.Master);
        errors.clear();

        DateTime asOfDate;


        if (!DateTime.TryParse(AsOfDate.Value, out asOfDate))
        {
            asOfDate = DateTime.Today;
        }
        HROne.Reports.Employee.EmployeeResidencyProcess rpt = null;
        // Start 0000185, KuangWei, 2015-05-05
        ArrayList empList = WebUtils.SelectedRepeaterItemToBaseObjectList(db, Repeater, "ItemSelect");
        if (GroupBy.SelectedValue.Equals("Company", StringComparison.CurrentCultureIgnoreCase))
            rpt = new HROne.Reports.Employee.EmployeeResidencyProcess(dbConn, asOfDate, HROne.Reports.Employee.EmployeeResidencyProcess.ReportGroupBy.Company, 0, empList);
        else if (GroupBy.SelectedValue.Equals("HLevel", StringComparison.CurrentCultureIgnoreCase))
            rpt = new HROne.Reports.Employee.EmployeeResidencyProcess(dbConn, asOfDate, HROne.Reports.Employee.EmployeeResidencyProcess.ReportGroupBy.Hierarchy, int.Parse(HLevelID.SelectedValue), empList);
        else if (GroupBy.SelectedValue.Equals("PayGroup", StringComparison.CurrentCultureIgnoreCase))
            rpt = new HROne.Reports.Employee.EmployeeResidencyProcess(dbConn, asOfDate, HROne.Reports.Employee.EmployeeResidencyProcess.ReportGroupBy.PayrollGroup, 0, empList);
        //string reportFileName = WebUtils.GetLocalizedReportFile(Server.MapPath("~/Report_Employee_HeadCount.rpt"));
        // End 0000185, KuangWei, 2015-05-05

        WebUtils.ReportExport(dbConn, user, errors, lblReportHeader.Text, Response, rpt, string.Empty, ((Button)sender).CommandArgument, "EmployeeResidencyReport", true);
        //HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "Report_Employee_HeadCount_View.aspx?CurrentDate=" + currentDate.Ticks + "&ReferenceDate=" + referenceDate.Ticks);


        //else
        //    loadState();    
    }
    protected void GroupBy_SelectedIndexChanged(object sender, EventArgs e)
    {

    }

    // Start 0000185, KuangWei, 2015-05-05
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

        string select = "e.*";
        string from = "from [" + db.dbclass.tableName + "] e ";

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
        cb.Checked = true; 
        WebFormUtils.LoadKeys(db, row, cb);
    }

    protected void Gender_SelectedIndexChanged(object sender, EventArgs e)
    {
        info.page = 0;
        view = loadData(info, db, Repeater);
    }
    // End 0000185, KuangWei, 2015-05-05
}
