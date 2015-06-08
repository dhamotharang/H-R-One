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
using System.Globalization;
using HROne.Lib.Entities;
using HROne.Translation;

public partial class Attendance_AttendanceRecordAdjust_List : HROneWebPage
{
    private const string FUNCTION_CODE = "ATT008";
    protected DBManager db = EEmpPersonalInfo.db;
    protected SearchBinding binding;
    protected ListInfo info;
    protected DataView view;

    

    protected void Page_Load(object sender, EventArgs e)
    {
       
        if (!WebUtils.CheckAccess(Response, Session, FUNCTION_CODE, WebUtils.AccessLevel.Read))
            return;

        HROne.Common.WebUtility.WebControlsLocalization(this, this.Controls);
        

        binding = new SearchBinding(dbConn, db);
        //binding.add(new LikeSearchBinder(EmpNo, "EmpNo"));
        //binding.add(new LikeSearchBinder(EmpEngSurname, "EmpEngSurname"));
        //binding.add(new LikeSearchBinder(EmpEngOtherName, "EmpEngOtherName"));
        //binding.add(new LikeSearchBinder(EmpChiFullName, "EmpChiFullName"));
        //binding.add(new LikeSearchBinder(EmpAlias, "EmpAlias"));
        //binding.add(new DropDownVLSearchBinder(EmpGender, "EmpGender", Values.VLGender));
        //binding.add(new FieldDateRangeSearchBinder(JoinDateFrom, JoinDateTo, "EmpDateOfJoin").setUseCurDate(false));
        //binding.add(new DropDownVLSearchBinder(EmpStatus, "EmpStatus", EEmpPersonalInfo.VLEmpStatus));
        binding.initValues("EmpStatus", null, EEmpPersonalInfo.VLEmpStatus, null);
        binding.init(DecryptedRequest, null);

        info = ListFooter.ListInfo;

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

        filter.add(new IN("EmpID", "Select distinct EmpID from " + EAttendanceRecord.db.dbclass.tableName,new DBFilter()));

        DBFilter empInfoFilter = EmployeeSearchControl1.GetEmpInfoFilter(AppUtils.ServerDateTime(), AppUtils.ServerDateTime());
        empInfoFilter.add(new MatchField("e.EmpID", "ee.EmpID"));
        filter.add(new Exists(EEmpPersonalInfo.db.dbclass.tableName + " ee", empInfoFilter));

        DataTable table = filter.loadData(dbConn, null, select, from);
        table = EmployeeSearchControl1.FilterEncryptedEmpInfoField(table, info);

        view = new DataView(table);
        ListFooter.Refresh();

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

        //foreach (RepeaterItem item in HierarchyLevel.Items)
        //{
        //    DropDownList c = (DropDownList)item.FindControl("HElementID");
        //    c.SelectedIndex = 0;
        //}
        view = loadData(info, db, Repeater);

    }
    protected void FirstPage_Click(object sender, EventArgs e)
    {
        view = loadData(info, db, Repeater);
    }
    protected void PrevPage_Click(object sender, EventArgs e)
    {
        view = loadData(info, db, Repeater);
    }
    protected void NextPage_Click(object sender, EventArgs e)
    {
        view = loadData(info, db, Repeater);
    }
    protected void LastPage_Click(object sender, EventArgs e)
    {
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
    //protected void Delete_Click(object sender, EventArgs e)
    //{
    //    PageErrors errors = PageErrors.getErrors(db, Page.Master);
    //    errors.clear();

    //    ArrayList list = WebUtils.SelectedRepeaterItemToBaseObjectList(db, Repeater, "ItemSelect");
    //    foreach (EEmpPersonalInfo o in list)
    //    {


    //        DBFilter empPayrollFilter = new DBFilter();
    //        empPayrollFilter.add(new Match("EmpID", o.EmpID));
    //        DBFilter paymentRecordFilter = new DBFilter();
    //        paymentRecordFilter.add(new IN("EmpPayrollID", "Select EmpPayrollID from [" + EEmpPayroll.db.dbclass.tableName + "]", empPayrollFilter));

    //        if (EPaymentRecord.db.count(dbConn, paymentRecordFilter) <= 0)
    //        {

    //            WebUtils.StartFunction(Session, FUNCTION_CODE);
    //            db.delete(dbConn, o);
    //            WebUtils.EndFunction(dbConn);
    //            EmpUtils.DeleteEmp(o.EmpID);
    //        }
    //        else
    //        {
    //            EEmpPersonalInfo.db.select(dbConn, o);
    //            errors.addError(string.Format(HROne.Translation.PageErrorMessage.ERROR_DELETE_EMP_PAYMENT_EXISTS, new string[] { o.EmpNo }));
    //        }
    //    }
    //    loadState();
    //    loadData(info, db, Repeater);
    //}
    public void New_Click(object sender, EventArgs e)
    {
        HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "Emp_Edit.aspx");
    }
    //protected void loadHierarchy()
    //{
    //    DBFilter filter;
    //    ArrayList list;


    //    filter = new DBFilter();
    //    filter.add("HLevelSeqNo", true);
    //    list = EHierarchyLevel.db.select(dbConn, filter);
    //    HierarchyLevel.DataSource = list;
    //    HierarchyLevel.DataBind();


    //}
    //protected void HierarchyLevel_ItemDataBound(object sender, RepeaterItemEventArgs e)
    //{
    //    EHierarchyLevel level = (EHierarchyLevel)e.Item.DataItem;
    //    DBFilter filter = new DBFilter();
    //    filter.add(new Match("HLevelID", level.HLevelID));
    //    CultureInfo ci = HROne.Common.WebUtility.GetSessionCultureInfo(Session);
    //    DropDownList c = (DropDownList)e.Item.FindControl("HElementID");
    //    string selected = c.SelectedValue;
    //    WebFormUtils.loadValues(c, EHierarchyElement.VLHierarchyElement, filter, ci, selected, "combobox.notselected");
    //    c.Attributes["HLevelID"] = level.HLevelID.ToString();


    //}
}
