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

public partial class Emp_LeaveHistory_Form : HROneWebControl
{
    protected DBManager db = ELeaveApplication.db;
    protected SearchBinding binding;
    protected ListInfo info;
    protected DataView view;
    protected bool isAllowLeaveCancel = true;
    public int CurID = -1;

    protected void Page_Load(object sender, EventArgs e)
    {
        isAllowLeaveCancel = ESystemParameter.getParameter(dbConn, ESystemParameter.PARAM_CODE_ESS_FUNCTION_CANCEL_LEAVE_APPLICATION).Equals("N", StringComparison.CurrentCultureIgnoreCase) ? false : true;

        EESSUser user = WebUtils.GetCurUser(Session);
        if (user != null)
        {
            CurID = user.EmpID;
            EmpID.Value = CurID.ToString();
        }

        DBFilter leaveTypeFilter = new DBFilter();
        {
            DBFilter leaveAppFilter = new DBFilter();
            leaveAppFilter.add(new Match("EmpID",CurID));
            DBFilter leaveCodeFilter = new DBFilter();
            leaveCodeFilter.add(new IN("LeaveCodeID", "SELECT DISTINCT la.LeaveCodeID FROM " + ELeaveApplication.db.dbclass.tableName + " la", leaveAppFilter));
            leaveTypeFilter.add(new IN("LeaveTypeID", "SELECT DISTINCT lc.LeaveTypeID FROM " + ELeaveCode.db.dbclass.tableName + " lc", leaveCodeFilter));
        }
        binding = new SearchBinding(dbConn, db);
        binding.initValues("LeaveAppUnit", null, Values.VLLeaveUnit, null);
        binding.add(new DropDownVLSearchBinder(LeaveType, "l.LeaveTypeID", ELeaveType.VLLeaveType).setFilter(leaveTypeFilter));
        //binding.add(new FieldDateRangeSearchBinder(LeaveAppDateFrom.TextBox, LeaveAppDateTo.TextBox, "LeaveAppDateFrom").setUseCurDate(false));

        binding.init(Request.QueryString, null);

        info = ListFooter.ListInfo;
    }
    protected void Page_PreRender(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            LeaveAppDateFrom.Value = AppUtils.ServerDateTime().ToString("yyyy-01-01");
            LeaveAppDateTo.Value = AppUtils.ServerDateTime().ToString("yyyy-12-31");
            if (view == null)
            {
                view = loadData(info, db, Repeater);
            }
        }
    }

    //public void loadState()
    //{
    //    info = new ListInfo();
    //    int page = 0;
    //    if (!CurPage.Value.Equals(""))
    //        page = Int32.Parse(CurPage.Value);
    //    info.loadState(Request, page);
    //    info.order = Order.Value.Equals("True");
    //    info.orderby = OrderBy.Value;
    //    if (info.orderby == "")
    //        info.orderby = null;
    //    info.recordPerPage = -1;
    //}
    public DataView loadData(ListInfo info, DBManager db, Repeater repeater)
    {
        DBFilter filter = binding.createFilter();
        OR leaveCancelIDOrTerm = new OR();
        leaveCancelIDOrTerm.add(new NullTerm("LeaveAppCancelID"));
        leaveCancelIDOrTerm.add(new Match("LeaveAppCancelID", "<=", 0));
        filter.add(leaveCancelIDOrTerm);

        DateTime dtPeriodFr, dtPeriodTo;
        if (DateTime.TryParse(LeaveAppDateFrom.Value, out dtPeriodFr))
            filter.add(new Match("LeaveAppDateFrom", ">=", dtPeriodFr));
        if (DateTime.TryParse(LeaveAppDateTo.Value, out dtPeriodTo))
            filter.add(new Match("LeaveAppDateFrom", "<", dtPeriodTo.AddDays(1)));

        string strPeriodStartDate = ESystemParameter.getParameter(dbConn, ESystemParameter.PARAM_CODE_ESS_LEAVE_HISTORY_START_DATE);
        DateTime dtPeriodStartDate;
        if (DateTime.TryParse(strPeriodStartDate, out dtPeriodStartDate))
        {
            filter.add(new Match("LeaveAppDateFrom", ">=", dtPeriodStartDate));
        }

        //if (info != null && info.orderby != null && !info.orderby.Equals(""))
        //    filter.add(info.orderby, info.order);

        filter.add(new Match("EmpID", this.CurID));

        string select = "c.*, l.LeaveCode, l.LeaveCodeDesc";
        string from = "from [" + db.dbclass.tableName + "] c LEFT JOIN " +
            ELeaveCode.db.dbclass.tableName + " l ON c.LeaveCodeID=l.LeaveCodeID";


        DataTable table = filter.loadData(dbConn, info, select, from);

        view = new DataView(table);

        ListFooter.Refresh();

        if (repeater != null)
        {
            repeater.DataSource = view;
            repeater.DataBind();
        }
        return view;

    }

    protected void Repeater_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        DataRowView row = (DataRowView)e.Item.DataItem;
        ELeaveApplication obj = new ELeaveApplication();
        ELeaveApplication.db.toObject(row.Row, obj);

        Button cancelButton = (Button)e.Item.FindControl("Cancel");

        DBFilter requestLeaveAppCancelFilter = new DBFilter();
        requestLeaveAppCancelFilter.add(new Match("EmpID", CurID));
        requestLeaveAppCancelFilter.add(new Match("LeaveAppID", obj.LeaveAppID));

        DBFilter empRequestFilter = new DBFilter();
        empRequestFilter.add(new Match("EmpID", CurID));
        empRequestFilter.add(new Match("EmpRequestType", EEmpRequest.TYPE_EELEAVECANCEL));
        empRequestFilter.add(new IN("EmpRequestRecordID", "SELECT RequestLeaveAppCancelID FROM " + ERequestLeaveApplicationCancel.db.dbclass.tableName, requestLeaveAppCancelFilter));
        empRequestFilter.add(EEmpRequest.EndStatusDBTerms("EmpRequestStatus", true));

        if (obj.LeaveAppCancelID > 0 || EEmpRequest.db.count(dbConn, empRequestFilter) > 0)
            cancelButton.Visible = false;
        else
        {
            ELeaveCode leaveCode = new ELeaveCode();
            leaveCode.LeaveCodeID = obj.LeaveCodeID;
            if (ELeaveCode.db.select(dbConn, leaveCode))
            {
                if (leaveCode.LeaveCodeHideInESS)
                    cancelButton.Visible = false;
                else
                {
                    //  Temporary set to invisible to add more constraint before launch
                    cancelButton.Visible = isAllowLeaveCancel;
                    cancelButton.Attributes["LeaveAppID"] = obj.LeaveAppID.ToString();
                }
            }
            else
                cancelButton.Visible = false;
        }
        
        // Start 0000094, Ricky So, 2014-09-09
        Label RequestQty = (Label)e.Item.FindControl("LeaveAppDays");

        string m_unit = ((DataRowView)e.Item.DataItem)["LeaveAppUnit"].ToString();
        double m_appHours;
        double m_appDays;

        Double.TryParse(((DataRowView)e.Item.DataItem)["LeaveAppHours"].ToString(), out m_appHours);
        Double.TryParse(((DataRowView)e.Item.DataItem)["LeaveAppDays"].ToString(), out m_appDays);

        switch (m_unit)
        {
            case "H": RequestQty.Text = m_appHours.ToString("0.000");
                break;
            default: RequestQty.Text = m_appDays.ToString("0.000");
                break;
        }
        // End 0000094, Ricky So, 2014-09-09

        HROne.Common.WebUtility.WebControlsLocalization(Session, e.Item.Controls);
    }

    protected void Repeater_ItemCommand(object source, RepeaterCommandEventArgs e)
    {
        PageErrors errors = PageErrors.getErrors(ELeaveApplication.db, Page.Master);

        int LeaveAppID=0;
        if (int.TryParse(((Button)e.Item.FindControl("Cancel")).Attributes["LeaveAppID"], out LeaveAppID))
        {
            ELeaveApplication obj = new ELeaveApplication();
            obj.LeaveAppID = LeaveAppID;
            if (ELeaveApplication.db.select(dbConn, obj))
            {
                //if (obj.EmpPayrollID > 0 || obj.EmpPaymentID > 0)
                //{
                //    //  message prompt to user
                //}
                //else
                {
                    ELeaveCode leaveCode = new ELeaveCode();
                    leaveCode.LeaveCodeID = obj.LeaveCodeID;
                    if (ELeaveCode.db.select(dbConn, leaveCode))
                    {
                        if (leaveCode.LeaveCodeHideInESS)
                        {
                            //  message prompt to user
                        }
                        else
                        {
                            HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "ESS_EmpLeaveApplicationCancel.aspx?LeaveAppID=" + LeaveAppID);

                            //AppUtils.StartFunction(dbConn, 0, "PER009", obj.EmpID, true);
                            //ELeaveApplication.db.delete(dbConn, obj);
                            //AppUtils.EndFunction(dbConn);
                            //ESSAuthorizationProcess authorization = new ESSAuthorizationProcess(dbConn);
                            //authorization.CancelLeaveApplicatoin(obj);
                            //errors.addError(HROne.Common.WebUtility.GetLocalizedString("The leave application is cancelled"));
                        }
                    }
                    else
                    {
                        //  message prompt to user
                    }
                }
            }
        }
        view = loadData(info, db, Repeater);



    }

    protected void Search_Click(object sender, EventArgs e)
    {
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

        // Start 0000094, Ricky So, 2014-09-15
        if (l.ID == "_LeaveDuration")
        {
            info.orderby = "LeaveAppUnit, LeaveAppDays";
        }
        else
            info.orderby = id;
        //info.orderby = id;
        // End 0000094, Ricky So, 2014-09-15
        view = loadData(info, db, Repeater);

    }
}
