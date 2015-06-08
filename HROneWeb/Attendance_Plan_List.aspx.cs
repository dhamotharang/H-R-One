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

public partial class Attendance_Plan_List : HROneWebPage
{
    private const string FUNCTION_CODE = "ATT003";

    protected DBManager db = EAttendancePlan.db;
    protected SearchBinding binding;
    protected ListInfo info;
    protected DataView view;
    private bool IsAllowEdit = true;
    
    
    protected void Page_Load(object sender, EventArgs e)
    {
       
        if (!WebUtils.CheckAccess(Response, Session, FUNCTION_CODE, WebUtils.AccessLevel.Read))
            return;
        toolBar.FunctionCode = FUNCTION_CODE;

        

        binding = new SearchBinding(dbConn, db);
        binding.add(new LikeSearchBinder(AttendancePlanCode, "AttendancePlanCode"));
        binding.add(new LikeSearchBinder(AttendancePlanDesc, "AttendancePlanDesc"));
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
        cb.Visible = IsAllowEdit;
        e.Item.FindControl("ItemSelect").Visible = toolBar.DeleteButton_Visible;

    }

    protected void Delete_Click(object sender, EventArgs e)
    {
        PageErrors errors = PageErrors.getErrors(db, Page.Master);
        errors.clear();

        ArrayList list = WebUtils.SelectedRepeaterItemToBaseObjectList(db, Repeater, "ItemSelect");

        foreach (EAttendancePlan o in list)
        {
            if (EAttendancePlan.db.select(dbConn, o))
            {
                DBFilter empPosFilter = new DBFilter();
                empPosFilter.add(new Match("AttendancePlanID", o.AttendancePlanID));
                empPosFilter.add("EmpID", true);
                ArrayList empPosList = EEmpPositionInfo.db.select(dbConn, empPosFilter);
                if (empPosList.Count > 0)
                {
                    errors.addError(string.Format(HROne.Translation.PageErrorMessage.ERROR_CODE_USED_BY_EMPLOYEE, new string[] { HROne.Common.WebUtility.GetLocalizedString("Attendance Plan"), o.AttendancePlanCode }));
                    foreach (EEmpPositionInfo empPos in empPosList)
                    {
                        EEmpPersonalInfo empInfo = new EEmpPersonalInfo();
                        empInfo.EmpID = empPos.EmpID;
                        if (EEmpPersonalInfo.db.select(dbConn, empInfo))
                            errors.addError("- " + empInfo.EmpNo + ", " + empInfo.EmpEngFullName);
                        else
                            EEmpPositionInfo.db.delete(dbConn, empPos);

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
        HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "Attendance_Plan_Edit.aspx");
    }
}
