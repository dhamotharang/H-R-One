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
using HROne.Lib.Entities;
using System.Net.Mail;

public partial class LateWaiveForm : HROneWebControl 
{
    public Binding binding;
    public SearchBinding sbinding;
    public DBManager db = ERequestLateWaive.db;
    public DBManager sdb = EAttendanceRecord.db;
    protected ListInfo info;
    protected DataView view;

    public ERequestLateWaive obj;
    public int CurID = -1;
    public int CurEmpID = -1;


    protected void Page_Load(object sender, EventArgs e)
    {
        sbinding = new SearchBinding(dbConn, sdb);
        //sbinding.add(new FieldDateRangeSearchBinder(RequestLateWaivePeriodFrom, RequestLateWaivePeriodTo, "AttendanceRecordDate").setUseCurDate(false));
        sbinding.init(Request.QueryString, null);
        info = ListFooter.ListInfo;

        if (!WebUtils.CheckAccess(Response, Session))
            return;
        binding = new Binding(dbConn, db);
        binding.add(EmpID);

        //binding.add(new TextBoxBinder(db, RequestLateWaivePeriodFrom.TextBox, RequestLateWaivePeriodFrom.ID));
        //binding.add(new TextBoxBinder(db, RequestLateWaivePeriodTo.TextBox, RequestLateWaivePeriodTo.ID));
        //binding.add(RequestLateWaiveHourFrom);
        //binding.add(RequestLateWaiveHourTo);
        //binding.add(RequestOTHours);
        binding.add(RequestLateWaiveReason);
        binding.init(Request, Session);

        HROne.Common.WebUtility.WebControlsLocalization(Session, this.Controls);

        EESSUser user = WebUtils.GetCurUser(Session);
        if (user != null)
        {
            CurEmpID = user.EmpID;
            EmpID.Value = CurEmpID.ToString();
            // Start 0000112, Miranda, 2015-01-11
            //// set default period is current month
            //DateTime now = AppUtils.ServerDateTime().Date;
            //DateTime firstDay = new DateTime(now.Year, now.Month, 1);
            //DateTime lastDay = firstDay.AddMonths(1).AddDays(-1);
            //// Start Miranda, 0000112, 2014-12-29
            //RequestLateWaivePeriodFrom.Value = firstDay.ToString("yyyy-MM-dd");
            //RequestLateWaivePeriodTo.Value = lastDay.ToString("yyyy-MM-dd");
            //// End Miranda, 0000112, 2014-12-29
            // End 0000112, Miranda, 2015-01-11
        }
    }

    protected void Page_PreRender(object sender, EventArgs e)
    {
        if (view == null)
        {
            view = loadData(info, sdb, Repeater);
        }
        if (!Page.IsPostBack)
        {
            // Start 0000112, Miranda, 2015-01-11
            // set default period is current month
            DateTime now = AppUtils.ServerDateTime().Date;
            DateTime firstDay = new DateTime(now.Year, now.Month, 1);
            DateTime lastDay = firstDay.AddMonths(1).AddDays(-1);
            RequestLateWaivePeriodFrom.Value = firstDay.ToString("yyyy-MM-dd");
            RequestLateWaivePeriodTo.Value = lastDay.ToString("yyyy-MM-dd");
            Search_Click(sender, e);
            // End 0000112, Miranda, 2015-01-11
            // Start 0000112, Miranda, 2015-01-17
            if (CurEmpID > 0)
            // End 0000112, Miranda, 2015-01-17
            {
                loadObject();
            }
        }
    }
    protected bool loadObject()
    {
        obj = new ERequestLateWaive();
        bool isNew = WebFormWorkers.loadKeys(db, obj, Request);
        if (!db.select(dbConn, obj))
            return false;

        Hashtable values = new Hashtable();
        db.populate(obj, values);
        binding.toControl(values);

        return true;
    }

    protected DataView loadData(ListInfo info, DBManager db, Repeater repeater)
    {
        DBFilter filter = sbinding.createFilter();
        if (info != null && info.orderby != null && !info.orderby.Equals(""))
            filter.add(info.orderby, info.order);

        DateTime dtPeriodFr, dtPeriodTo;
        if (DateTime.TryParse(RequestLateWaivePeriodFrom.Value, out dtPeriodFr))
            filter.add(new Match("AttendanceRecordDate", ">=", dtPeriodFr));
        if (DateTime.TryParse(RequestLateWaivePeriodTo.Value, out dtPeriodTo))
            filter.add(new Match("AttendanceRecordDate", "<", dtPeriodTo.AddDays(1)));
        filter.add(new Match("AttendanceRecordActualLateMins", ">", 0));

        OR m_or = new OR();
        m_or.add(new Match("AttendanceRecordWaivedLateMins", 0));
        m_or.add(new NullTerm("AttendanceRecordWaivedLateMins"));

        filter.add(m_or);

        // Start 0000112, Miranda, 2015-01-17
        filter.add(new Match("EmpID", CurEmpID));
        // End 0000112, Miranda, 2015-01-17
        string select = "A.*, r.RosterCodeDesc";
        string from = "from " + sdb.dbclass.tableName + " A LEFT JOIN " +
            ERosterCode.db.dbclass.tableName + " r ON A.RosterCodeID=r.RosterCodeID";
        DataTable table = filter.loadData(dbConn, info, select, from);
        //DataTable table = WebUtils.GetDataTableFromSelectQueryWithFilter(dbConn, select, from, filter, info);
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
        CheckBox cb = (CheckBox)e.Item.FindControl("ItemSelect");
        HtmlAnchor requestLink = (HtmlAnchor)e.Item.FindControl("requestLink");

        WebFormUtils.LoadKeys(sdb, row, cb);
        cb.Checked = false;
        cb.Visible = true;

        EAttendanceRecord obj = new EAttendanceRecord();
        EAttendanceRecord.db.toObject(((DataRowView)e.Item.DataItem).Row, obj);
        // Start 0000112, Miranda, 2015-01-11
        if (hasSubmitted(obj.AttendanceRecordID))
        {
            cb.Visible = false;
        }
        // End 0000112, Miranda, 2015-01-11
    }

    // Start 0000112, Miranda, 2015-01-11
    private bool hasSubmitted(int attendanceRecordID)
    {
        DBFilter filter = new DBFilter();
        filter.add(new Match("AttendanceRecordID", attendanceRecordID));
        // Start 0000112, Miranda, 2015-01-17
        filter.add("RequestLateWaiveCreateDate", false);
        // End 0000112, Miranda, 2015-01-17
        ArrayList list = ERequestLateWaive.db.select(dbConn, filter);
        if (list != null && list.Count > 0)
        {
            // Start 0000112, Miranda, 2015-01-17
            ERequestLateWaive reqLateWaive = (ERequestLateWaive)list[0];// get the latest submitted record of the attendance record.
            
            DBFilter empReqFilter = new DBFilter();
            empReqFilter.add(new Match("EmpID", CurEmpID));
            empReqFilter.add(new Match("EmpRequestRecordID", reqLateWaive.RequestLateWaiveID));
            empReqFilter.add(new Match("EmpRequestType", EEmpRequest.TYPE_EELATEWAIVE));
            ArrayList empReqList = EEmpRequest.db.select(dbConn, empReqFilter);
            if (empReqList != null && empReqList.Count > 0)
            {
                bool hasRejected = false;
                bool hasCancelled = false;
                string empRequestStatus = null;
                for (int i = 0; i < empReqList.Count; i++ )
                {
                    empRequestStatus = ((EEmpRequest)empReqList[i]).EmpRequestStatus;
                    if (EEmpRequest.STATUS_CANCELLED.Equals(empRequestStatus))
                    {
                        hasCancelled = true;
                    }
                    else if (EEmpRequest.STATUS_REJECTED.Equals(empRequestStatus))
                    {
                        hasRejected = true;
                    }
                }
                return hasRejected ? true : (hasCancelled ? false : true);
            } else {
                return true;
            }
            // End 0000112, Miranda, 2015-01-17
        }
        else
        {
            return false;
        }
    }
    // End 0000112, Miranda, 2015-01-11

    protected void Save_Click(object sender, EventArgs e)
    {
        ERequestLateWaive c = new ERequestLateWaive();

        Hashtable values = new Hashtable();
        binding.toValues(values);

        PageErrors errors = PageErrors.getErrors(db, Page);
        errors.clear();


        db.validate(errors, values);

        if (!errors.isEmpty())
            return;


        db.parse(values, c);

        //if (c.RequestLateWaiveHourTo < c.RequestLateWaiveHourFrom)
        //    errors.addError("RequestLateWaiveTimeFrom", "Invald hours");
        //if (c.RequestLateWaivePeriodTo < c.RequestLateWaivePeriodFrom)
        //        errors.addError("RequestLateWaivePeriodFrom", "Date To cannot be earlier than Date From");

        //if (c.RequestOTHours <= 0)
        //    errors.addError(string.Format(HROne.Translation.PageErrorMessage.ERROR_FIELD_REQUIRED, new string[] { lblOTHours.Text }));

        if (!errors.isEmpty())
            return;

        // Start Ricky So, 2014-09-05, Avoid checking time overlap for CL Requisition
        //ArrayList overlapLateWaiveList = new ArrayList();
        //if (c.IsOverlapLateWaive(dbConn, out overlapLateWaiveList))
        //{
        //    string strHourlyOverlapMessage = string.Empty;

        //    foreach (BaseObject overlapLateWaive in overlapLateWaiveList)
        //    {
        //        if (overlapLateWaive is ELateWaive)
        //        {
        //            ELateWaive previousLateWaive = (ELateWaive)overlapLateWaive;
        //            if (string.IsNullOrEmpty(strHourlyOverlapMessage))
        //                strHourlyOverlapMessage = "Leave time cannot overlap with previous CL Requisition";
        //        }
        //        else if (overlapLateWaive is ERequestLateWaive)
        //        {
        //            ERequestLateWaive previousRequestLateWaive = (ERequestLateWaive)overlapLateWaive;
        //            if (string.IsNullOrEmpty(strHourlyOverlapMessage))
        //                strHourlyOverlapMessage = "Leave time cannot overlap with previous CL Requisition";

        //        }
        //    }

        //    if (!string.IsNullOrEmpty(strHourlyOverlapMessage))
        //        errors.addError(strHourlyOverlapMessage);
        //}
        // End Ricky So, 2014-09-05

        if (!errors.isEmpty())
            return;

        try
        {
            ESSAuthorizationProcess authorization = new ESSAuthorizationProcess(dbConn);
            // loap each attendance record which is ticked
            ArrayList list = new ArrayList();
            foreach (RepeaterItem i in Repeater.Items)
            {
                CheckBox cb = (CheckBox)i.FindControl("ItemSelect");
                if (cb.Checked)
                {
                    DBObject o = sdb.createObject();
                    WebFormUtils.GetKeys(sdb, o, cb);
                    list.Add(o);
                }

            }
            foreach (EAttendanceRecord o in list)
            {
                c.AttendanceRecordID = o.AttendanceRecordID;
                authorization.SubmitLateWaive(c);
            }
        }
        catch (Exception ex)
        {
            errors.addError(ex.Message);

        }

        if (!errors.isEmpty())
            return;
        HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "~/ESS_EmpRequestStatus.aspx");


    }

    protected void Search_Click(object sender, EventArgs e)
    {
        info.page = 0;
        view = loadData(info, sdb, Repeater);
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

        view = loadData(info, sdb, Repeater);

    }

    //protected void LateWaiveTime_TextChanged(object sender, EventArgs e)
    //{
    //        DateTime dtTimeFrom = new DateTime();
    //        if (!DateTime.TryParseExact(RequestLateWaiveHourFrom.Text, "HH:mm", null, System.Globalization.DateTimeStyles.None, out dtTimeFrom))
    //            RequestLateWaiveHourFrom.Text = string.Empty;

    //        DateTime dtTimeTo = new DateTime();
    //        if (!DateTime.TryParseExact(RequestLateWaiveHourTo.Text, "HH:mm", null, System.Globalization.DateTimeStyles.None, out dtTimeTo))
    //            RequestLateWaiveHourTo.Text = string.Empty;

    //        if (dtTimeFrom.Ticks.Equals(0) || dtTimeTo.Ticks.Equals(0))
    //            return;

    //        DateTime tmpLateWaiveDateFrom;
    //        if (DateTime.TryParse(RequestLateWaivePeriodFrom.Value, out tmpLateWaiveDateFrom))
    //        {
    //            double workhour = 0;
    //            EEmpPositionInfo currentEmpPos = AppUtils.GetLastPositionInfo(dbConn, tmpLateWaiveDateFrom, CurEmpID);
    //            if (currentEmpPos != null)
    //            {
    //                EWorkHourPattern workPattern = new EWorkHourPattern();
    //                workPattern.WorkHourPatternID = currentEmpPos.WorkHourPatternID;
    //                if (EWorkHourPattern.db.select(dbConn, workPattern))
    //                    workhour = workPattern.GetDefaultWorkHour(dbConn, tmpLateWaiveDateFrom);
    //            }
    //            if (workhour > 0)
    //            {
    //                double timeDiff = ((TimeSpan)dtTimeTo.Subtract(dtTimeFrom)).TotalHours;
    //                if (timeDiff < 0) timeDiff += 1;
    //                RequestOTHours.Text = timeDiff.ToString("0.####");
    //            }
    //        }
    //}
}
