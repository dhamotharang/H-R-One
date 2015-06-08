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
using HROne.Translation;
using HROne.Lib.Entities;

public partial class Attendance_Formula_List : HROneWebPage
{
    private const string FUNCTION_CODE = "ATT002";

    protected DBManager db = EAttendanceFormula.db;
    protected SearchBinding binding;
    protected ListInfo info;
    protected DataView view;
    

    protected void Page_Load(object sender, EventArgs e)
    {
       
        if (!WebUtils.CheckAccess(Response, Session, FUNCTION_CODE, WebUtils.AccessLevel.Read))
            return;
        toolBar.FunctionCode = FUNCTION_CODE;
        SelectAllPanel.Visible = toolBar.DeleteButton_Visible;

        

        binding = new SearchBinding(dbConn, db);
        //binding.initValues("AVCPlanIsResidual", null, Values.VLYesNo, HROne.Common.WebUtility.GetSessionCultureInfo(Session));
        binding.add(new LikeSearchBinder(AttendanceFormulaCode, "AttendanceFormulaCode"));
        binding.add(new LikeSearchBinder(AttendanceFormulaDesc, "AttendanceFormulaDesc"));


        binding.init(DecryptedRequest, null);

        info = ListFooter.ListInfo;

        HROne.Common.WebUtility.WebControlsLocalization(this, this.Controls);
        
    }

    protected void Page_PreRender(object sender, EventArgs e)
    {
        //if (!Page.IsPostBack)
        {
            view = loadData(info, db, Repeater);
        }
    }

    public DataView loadData(ListInfo info, DBManager db, Repeater repeater)
    {
        DBFilter filter = binding.createFilter();

        if (info != null && info.orderby != null && !info.orderby.Equals(""))
            filter.add(info.orderby, info.order);

        string select = "c.*";
        string from = "from [" + db.dbclass.tableName + "] c ";

        DataTable table = WebUtils.GetDataTableFromSelectQueryWithFilter(dbConn, select, from, filter, info);
        
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
        //view = loadData(info, db, Repeater);

    }
    protected void Reset_Click(object sender, EventArgs e)
    {
        binding.clear();
        //view = loadData(info, db, Repeater);

    }
    protected void FirstPage_Click(object sender, EventArgs e)
    {
        //view = loadData(info, db, Repeater);
    }
    protected void PrevPage_Click(object sender, EventArgs e)
    {
        //view = loadData(info, db, Repeater);
    }
    protected void NextPage_Click(object sender, EventArgs e)
    {
        //view = loadData(info, db, Repeater);
    }
    protected void LastPage_Click(object sender, EventArgs e)
    {
        //view = loadData(info, db, Repeater);
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

        //view = loadData(info, db, Repeater);

    }
    protected void Repeater_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        DataRowView row = (DataRowView)e.Item.DataItem;
        CheckBox cb = (CheckBox)e.Item.FindControl("ItemSelect");
        WebFormUtils.LoadKeys(db, row, cb);
        e.Item.FindControl("ItemSelect").Visible = toolBar.DeleteButton_Visible;
    }
    protected void Delete_Click(object sender, EventArgs e)
    {
        PageErrors errors = PageErrors.getErrors(db, Page.Master);
        errors.clear();

        ArrayList list = WebUtils.SelectedRepeaterItemToBaseObjectList(db, Repeater, "ItemSelect");

        foreach (EAttendanceFormula o in list)
        {
            if (EAttendanceFormula.db.select(dbConn, o))
            {
                DBFilter attendancePlanFilter = new DBFilter();
                attendancePlanFilter.add(new Match("AttendancePlanOTFormula", o.AttendanceFormulaID));
                attendancePlanFilter.add(new Match("AttendancePlanLateFormula", o.AttendanceFormulaID));
                ArrayList attendancePlanList = EAttendancePlan.db.select(dbConn, attendancePlanFilter);
                if (attendancePlanList.Count > 0)
                {
                    errors.addError(string.Format(HROne.Translation.PageErrorMessage.ERROR_CODE_USED_BY_EMPLOYEE, new string[] { HROne.Common.WebUtility.GetLocalizedString("AttendanceFormula Code"), o.AttendanceFormulaCode }));
                    foreach (EAttendancePlan attendancePlan in attendancePlanList)
                    {
                        errors.addError("- " + attendancePlan.AttendancePlanCode + ", " + attendancePlan.AttendancePlanDesc);
                    }
                    errors.addError(HROne.Translation.PageErrorMessage.ERROR_ACTION_ABORT);

                }
                else
                {
                    WebUtils.StartFunction(Session, FUNCTION_CODE);
                    db.delete(dbConn, o);
                    WebUtils.EndFunction(dbConn);
                }
            }
        }
        //loadData(info, db, Repeater);
    }

    public void New_Click(object sender, EventArgs e)
    {
        HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "Attendance_Formula_Edit.aspx");
    }
}
