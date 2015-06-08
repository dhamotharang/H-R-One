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

public partial class Emp_Request_LeaveCancel_List : HROneWebControl
{
    protected DBManager db = ERequestLeaveApplicationCancel.db;
    protected SearchBinding binding;
    protected Binding ebinding;
    protected ListInfo info;
    protected DataView view;
    //public static WFValueList VLStatusUnit = new WFTextList(new string[] { EEmpRequest.STATUS_USRCANCEL, EEmpRequest.STATUS_USRSUBMIT, EEmpRequest.STATUS_FSTAPP, EEmpRequest.STATUS_FSTREJ, EEmpRequest.STATUS_SNDAPP, EEmpRequest.STATUS_SNDREJ }, new string[] { EEmpRequest.STATUS_USRCANCEL, EEmpRequest.STATUS_USRSUBMIT, EEmpRequest.STATUS_FSTAPP, EEmpRequest.STATUS_FSTREJ, EEmpRequest.STATUS_SNDAPP, EEmpRequest.STATUS_SNDREJ });

    public int CurID = -1;

    protected void Page_Load(object sender, EventArgs e)
    {

        binding = new SearchBinding(dbConn, db);
        //binding.add(new DropDownVLSearchBinder(EmpRequestStatus, "EmpRequestStatus", VLStatusUnit));
        binding.initValues("LeaveAppUnit", null, Values.VLLeaveUnit, null);
        binding.initValues("EmpRequestStatus", null, EEmpRequest.VLRequestStatus, null);
        //binding.add(new FieldDateRangeSearchBinder(RequestFromDate, RequestToDate, "RequestLeaveAppCreateDate").setUseCurDate(false));

        binding.init(Request.QueryString, null);

        info = ListFooter.ListInfo;

        EESSUser user = WebUtils.GetCurUser(Session);
        if (user != null)
        {
            CurID = user.EmpID;
            EmpID.Value = CurID.ToString();
        }
    }
    protected void Page_PreRender(object sender, EventArgs e)
    {
        if (view == null)
        {
            view = loadData(info, db, Repeater);
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
    public DataView loadData(ListInfo info, DBManager db, DataList repeater)
    {
        DBFilter filter = binding.createFilter();

        DateTime dtPeriodFr, dtPeriodTo;
        if (DateTime.TryParse(RequestFromDate.Value, out dtPeriodFr))
            filter.add(new Match("RequestLeaveAppCancelCreateDateTime", ">=", dtPeriodFr));
        if (DateTime.TryParse(RequestToDate.Value, out dtPeriodTo))
            filter.add(new Match("RequestLeaveAppCancelCreateDateTime", "<", dtPeriodTo.AddDays(1)));

        //if (info != null && info.orderby != null && !info.orderby.Equals(""))
        //    filter.add(info.orderby, info.order);

        filter.add(new Match("c.EmpID", this.CurID));

        if (!EmpRequestStatus.SelectedValue.Equals(string.Empty))
        {
            if (EmpRequestStatus.SelectedValue.Equals("REQUEESTSTATUS_PROCESSING"))
            {
                //AND andTerms = new AND();
                //andTerms.add(new Match("EmpRequestStatus", "<>", EEmpRequest.STATUS_APPROVED));
                //andTerms.add(new Match("EmpRequestStatus", "<>", EEmpRequest.STATUS_CANCELLED));
                //andTerms.add(new Match("EmpRequestStatus", "<>", EEmpRequest.STATUS_REJECTED));
                filter.add(EEmpRequest.EndStatusDBTerms("EmpRequestStatus", true));
            }
            else if (EmpRequestStatus.SelectedValue.Equals("REQUEESTSTATUS_END_PROCESS"))
            {
                //OR orTerms = new OR();
                //orTerms.add(new Match("EmpRequestStatus", EEmpRequest.STATUS_APPROVED));
                //orTerms.add(new Match("EmpRequestStatus", EEmpRequest.STATUS_CANCELLED));
                //orTerms.add(new Match("EmpRequestStatus", EEmpRequest.STATUS_REJECTED));
                filter.add(EEmpRequest.EndStatusDBTerms("EmpRequestStatus", false));
            }
        }

        string select = "c.RequestLeaveAppCancelID, c.RequestLeaveAppCancelCreateDateTime, l.LeaveCode, l.LeaveCodeDesc, r.EmpRequestStatus, r.EmpRequestID, r.EmpRequestLastAuthorizationWorkFlowIndex, la.* ";
        string from = "from " + db.dbclass.tableName + " c " +
             " LEFT JOIN " + ELeaveApplication.db.dbclass.tableName + " la ON c.LeaveAppID = la.LeaveAppID " +
             "  LEFT JOIN " + ELeaveCode.db.dbclass.tableName +  " l ON la.LeaveCodeID=l.LeaveCodeID " +
            " LEFT JOIN " + EEmpRequest.db.dbclass.tableName + " r On r.EmpRequestRecordID = c.RequestLeaveAppCancelID and r.EmpRequestType = '" + EEmpRequest.TYPE_EELEAVECANCEL + "'";


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

    protected void Repeater_ItemDataBound(object sender, DataListItemEventArgs e)
    {

        HtmlInputHidden h = (HtmlInputHidden)e.Item.FindControl("_RequestLeaveAppCancelID");
        h.Value = ((DataRowView)e.Item.DataItem)["RequestLeaveAppCancelID"].ToString();

        // Start 0000094, Ricky So, 2014-09-09
        Label RequestQty = (Label)e.Item.FindControl("RequestLeaveDays");

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


        string submitStatus=((DataRowView)e.Item.DataItem)["EmpRequestStatus"].ToString() ;
        if ((submitStatus.Equals(EEmpRequest.STATUS_SUBMITTED) || submitStatus.Equals(EEmpRequest.STATUS_ACCEPTED)) && !submitStatus.Equals(EEmpRequest.STATUS_APPROVED))
            e.Item.FindControl("Cancel").Visible = true;
        else
            e.Item.FindControl("Cancel").Visible = false;
        HROne.Common.WebUtility.WebControlsLocalization(Session, e.Item.Controls);

    }
    protected void Repeater_ItemCommand(object source, DataListCommandEventArgs e)
    {
        PageErrors errors = PageErrors.getErrors(ELeaveApplication.db, Page.Master);

        DBFilter filterStatus = new DBFilter();
        DBManager Requestdb = EEmpRequest.db;
        HtmlInputHidden h = (HtmlInputHidden)e.Item.FindControl("_RequestLeaveAppCancelID");

        //------------------------------------------------------
        //Select Filter record from the EmpRequest table by EmpRequestRecordID and Request Status
        filterStatus.add(new Match("EmpRequestRecordID", Int32.Parse(h.Value)));
        OR orStatus = new OR();

        orStatus.add(new Match("EmpRequestStatus", EEmpRequest.STATUS_SUBMITTED));
        orStatus.add(new Match("EmpRequestStatus", EEmpRequest.STATUS_ACCEPTED));

        filterStatus.add(orStatus);
        filterStatus.add(new Match("EmpRequestStatus", "<>", EEmpRequest.STATUS_APPROVED));
        //------------------------------------------------------
        if (Requestdb.count(dbConn, filterStatus) > 0)
        {
            ArrayList EmpRequestList = Requestdb.select(dbConn, filterStatus);
            EEmpRequest EmpRequest = (EEmpRequest)EmpRequestList[0];
            ESSAuthorizationProcess authorization = new ESSAuthorizationProcess(dbConn);
            authorization.CancelAction(EmpRequest);
            errors.addError(HROne.Common.WebUtility.GetLocalizedString("The leave application is cancelled"));
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
            info.order = !info.order;

        // Start 0000094, Ricky So, 2014-09-15
        if (l.ID == "_RequestLeaveDuration")
        {
            info.orderby = "LeaveAppDays, LeaveAppUnit";
        }
        else
            info.orderby = id;
        //info.orderby = id;
        // End 0000094, Ricky So, 2014-09-15  
        
        view = loadData(info, db, Repeater);

    }
    protected void EmpRequestStatus_SelectedIndexChanged(object sender, EventArgs e)
    {
        view = loadData(info, db, Repeater);
    }
}
