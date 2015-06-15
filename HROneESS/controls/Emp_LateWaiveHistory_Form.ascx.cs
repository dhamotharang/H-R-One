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
using HROne.Lib.Entities;

public partial class Emp_LateWaiveHistory_Form : HROneWebControl
{
    protected DBManager sdb = ERequestLateWaive.db;
    protected SearchBinding sbinding;
    protected DBManager db = EEmpRequest.db;
    public Binding binding;
    protected ListInfo info;
    protected DataView view;
    public int CurID = -1;
    public int CurEntitleID = -1;
    private string[] lateWaiveReqType = new string[] { EEmpRequest.TYPE_EELATEWAIVE, EEmpRequest.TYPE_EELATEWAIVECANCEL };

    protected void Page_Load(object sender, EventArgs e)
    {
        EESSUser user = WebUtils.GetCurUser(Session);
        if (user != null)
        {
            CurID = user.EmpID;
            EmpID.Value = CurID.ToString();
        }

        //DBFilter lateYearFilter = new DBFilter();
        //lateYearFilter.add(new Match("EmpID", CurID));
        //DBFilter empReqFilter = new DBFilter();
        //empReqFilter.add(new Match("EmpID", CurID));
        //empReqFilter.add(new IN("EmpRequestType", lateWaiveReqType));
        //lateYearFilter.add(new IN("RequestLateWaiveID", "select EmpRequestRecordID from " + EEmpRequest.db.dbclass.tableName, empReqFilter));
        sbinding = new SearchBinding(dbConn, sdb);
        //sbinding.add(new DropDownVLSearchBinder(Year, "Year(RequestLateWaiveCreateDate)", ERequestLateWaive.VLLateWaiveYear).setFilter(lateYearFilter));

        sbinding.init(Request.QueryString, null);

        binding = new Binding(dbConn, sdb);
        //binding.add(RequestLateWaiveID);
        binding.add(EmpID);
        //binding.add(AttendanceRecordID);
        //binding.add(RequestLateWaiveReason);
        //binding.add(RequestLateWaiveCreateDate);
        //binding.add(EmpRequestID);
        //binding.add(AttendanceRecordDate);
        //binding.add(RosterCodeDesc);
        //binding.add(AttendanceRecordWorkStart);
        //binding.add(AttendanceRecordWorkEnd);
        //binding.add(AttendanceRecordActualLateMins);
        binding.init(Request, Session);

        if (!int.TryParse(DecryptedRequest["EmpRequestID"], out CurEntitleID))
            CurEntitleID = -1;

        info = ListFooter.ListInfo;
    }
    protected void Page_PreRender(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (view == null)
            {
                view = loadData(info, sdb, Repeater);
            }
        }
        LateWaiveDetailsTable.Visible = CurEntitleID != -1;
    }

    public DataView loadData(ListInfo info, DBManager db, Repeater repeater)
    {
        DBFilter filter = sbinding.createFilter();
        filter.add(new Match("c.EmpID", this.CurID));
        filter.add(new IN("e.EmpRequestType", lateWaiveReqType));
        //filter.add(new Match("Year(RequestLateWaiveCreateDate)", "=", Year.Text));
        //Start 0000210, Miranda, 2015-06-14
        //if (info != null && info.orderby != null && !info.orderby.Equals(""))
        //    filter.add(info.orderby, info.order);
        //End 0000210, Miranda, 2015-06-14
        string select = "c.*, e.EmpRequestID, a.AttendanceRecordDate, a.AttendanceRecordWorkStart, a.AttendanceRecordWorkEnd, a.AttendanceRecordActualLateMins, r.RosterCodeDesc ";
        string from = "from [" + sdb.dbclass.tableName + "] c" +
            " right join " + EEmpRequest.db.dbclass.tableName + " e on e.EmpRequestRecordID = c.RequestLateWaiveID and e.EmpID = c.EmpID" +
            " left join " + EAttendanceRecord.db.dbclass.tableName + " a on a.AttendanceRecordID = c.AttendanceRecordID" +
            " left join " + ERosterCode.db.dbclass.tableName + " r on r.RosterCodeID = a.RosterCodeID";
        
        DataTable table = filter.loadData(dbConn, info, select, from);

        view = new DataView(table);

        ListFooter.Refresh();

        if (repeater != null)
        {
            repeater.DataSource = view;
            repeater.DataBind();
        }
        if (CurEntitleID != -1)
        {
            EEmpRequest detail = new EEmpRequest();
            detail.EmpRequestID = this.CurEntitleID;
            if (EEmpRequest.db.select(dbConn, detail))
            {
                // Start 0000112, Miranda, 2015-01-11
                lblActionDate.Text = detail.EmpRequestCreateDate.ToString("yyyy-MM-dd HH:mm:ss");
                // End 0000112, Miranda, 2015-01-11
                lblActionBy.Text = detail.EmpRequestLastActionBy;
                lblStatus.Text = detail.EmpRequestStatus;
                lblReason.Text = detail.EmpRequestRejectReason;
            }
        }
        return view;

    }

    protected void Repeater_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        DataRowView row = (DataRowView)e.Item.DataItem;
        ERequestLateWaive obj = new ERequestLateWaive();
        ERequestLateWaive.db.toObject(row.Row, obj);

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

    //protected void Year_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    view = loadData(info, sdb, Repeater);
    //}

}
