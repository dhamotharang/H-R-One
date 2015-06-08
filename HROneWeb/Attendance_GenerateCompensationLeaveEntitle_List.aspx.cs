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
using HROne.Lib.Entities;
using HROne.DataAccess;
//using perspectivemind.validation;

public partial class Attendance_GenerateCompensationLeaveEntitle_List : HROneWebPage
{
    private const string FUNCTION_CODE = "ATT015";
    protected SearchBinding empSBinding, sbinding;
    public DBManager db = EPayrollGroup.db;
    public EPayrollGroup obj;

    protected ListInfo info;
    protected DataView empView;
    

    protected void Page_Load(object sender, EventArgs e)
    {
       
        if (!WebUtils.CheckAccess(Response, Session, FUNCTION_CODE, WebUtils.AccessLevel.Read))
            return;


        empSBinding = new SearchBinding(dbConn, EEmpPersonalInfo.db);
        empSBinding.initValues("EmpStatus", null, EEmpPersonalInfo.VLEmpStatus, HROne.Common.WebUtility.GetSessionUICultureInfo(Session));
        empSBinding.init(DecryptedRequest, null);

        HROne.Common.WebUtility.WebControlsLocalization(this, this.Controls);
        info = ListFooter.ListInfo;
    }

    protected void Page_PreRender(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            empView = emploadData(info, EEmpPayroll.db, empRepeater);
        }

    }




    public DataView emploadData(ListInfo info, DBManager db, Repeater repeater)
    {

        DBFilter filter = new DBFilter();// empSBinding.createFilter();

        //if (info != null && info.orderby != null && !info.orderby.Equals(""))
        //    filter.add(info.orderby, info.order);

        string select = "* ";
        string from = "from [" + EEmpPersonalInfo.db.dbclass.tableName + "] e ";


        DBFilter empInfoFilter = EmployeeSearchControl1.GetEmpInfoFilter(AppUtils.ServerDateTime(), AppUtils.ServerDateTime());
        empInfoFilter.add(new MatchField("e.EmpID", "ee.EmpID"));
        DateTime dtPeriodFr, dtPeriodTo;
        if (DateTime.TryParse(PeriodFr.Value, out dtPeriodFr) && DateTime.TryParse(PeriodTo.Value, out dtPeriodTo))
        {
            empInfoFilter.add(new Match("EmpDateOfJoin", "<=", dtPeriodTo));
            DBFilter empTermFilter = new DBFilter();
            empTermFilter.add(new MatchField("ee.EmpID", "et.EmpID"));
            empTermFilter.add(new Match("et.EmpTermLastDate", "<", dtPeriodFr));
            empInfoFilter.add(new Exists(EEmpTermination.db.dbclass.tableName + " et ", empTermFilter, true));

            DBFilter attendanceRecordFilter = new DBFilter();
            attendanceRecordFilter.add(new MatchField("e.EmpID", "ar.EmpID"));
            attendanceRecordFilter.add(new Match("ar.AttendanceRecordDate", "<=", dtPeriodTo));
            attendanceRecordFilter.add(new Match("ar.AttendanceRecordDate", ">=", dtPeriodFr));
            //attendanceRecordFilter.add(new Match("ar.AttendanceRecordActualOvertimeMins", ">", 0));
            empInfoFilter.add(new Exists(EAttendanceRecord.db.dbclass.tableName + " ar", attendanceRecordFilter));

            DBFilter empPosFilter = new DBFilter();
            empPosFilter.add(new MatchField("e.EmpID", "ep.EmpID"));
            empPosFilter.add(new Match("ep.EmpPosEffFr", "<=", dtPeriodTo));
            OR orEmpPosEffToTerm = new OR();
            orEmpPosEffToTerm.add(new Match("ep.EmpPosEffTo", ">=", dtPeriodFr));
            orEmpPosEffToTerm.add(new NullTerm("ep.EmpPosEffTo"));
            empPosFilter.add(orEmpPosEffToTerm);
            DBFilter attendancePlanFilter = new DBFilter();
            attendancePlanFilter.add(new MatchField("ep.AttendancePlanID", "ap.AttendancePlanID"));
            attendancePlanFilter.add(new Match("ap.AttendancePlanOTGainAsCompensationLeaveEntitle", true));
            empPosFilter.add(new Exists(EAttendancePlan.db.dbclass.tableName + " ap", attendancePlanFilter));
            empInfoFilter.add(new Exists(EEmpPositionInfo.db.dbclass.tableName + " ep", empPosFilter));
        }
        else
        {
            btnGenerate.Visible = false;
            empView = null;
            repeater.DataSource = null;
            repeater.DataBind();

            return null;
        }

        filter.add(new Exists(EEmpPersonalInfo.db.dbclass.tableName + " ee", empInfoFilter));




        DataTable table = filter.loadData(dbConn, null, select, from);
        table = EmployeeSearchControl1.FilterEncryptedEmpInfoField(table, info);

        if (table.Rows.Count != 0)
        {
            btnGenerate.Visible = true;
        }
        else
        {
            btnGenerate.Visible = false;
        }
        empView = new DataView(table);
        ListFooter.Refresh();

        if (repeater != null)
        {
            repeater.DataSource = empView;
            repeater.DataBind();
        }

        return empView;
    }
    //protected void empFirstPage_Click(object sender, EventArgs e)
    //{
    //    empView = emploadData(empInfo, EEmpPayroll.db, empRepeater);
    //}
    //protected void empPrevPage_Click(object sender, EventArgs e)
    //{
    //    empView = emploadData(empInfo, EEmpPayroll.db, empRepeater);
    //}
    //protected void empNextPage_Click(object sender, EventArgs e)
    //{
    //    empView = emploadData(empInfo, EEmpPayroll.db, empRepeater);
    //}
    //protected void empLastPage_Click(object sender, EventArgs e)
    //{
    //    empView = emploadData(empInfo, EEmpPayroll.db, empRepeater);
    //}
    protected void empChangeOrder_Click(object sender, EventArgs e)
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

        empView = emploadData(info, EEmpPayroll.db, empRepeater);

    }
    protected void empRepeater_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        DataRowView row = (DataRowView)e.Item.DataItem;
        CheckBox cb = (CheckBox)e.Item.FindControl("ItemSelect");
        WebFormUtils.LoadKeys(EEmpPersonalInfo.db, row, cb);

    }

    protected void Search_Click(object sender, EventArgs e)
    {
        info.page = 0;

        empView = emploadData(info, db, empRepeater);

    }
    protected void Reset_Click(object sender, EventArgs e)
    {
        EmployeeSearchControl1.Reset();
        empSBinding.clear();
        info.page = 0;

        empView = emploadData(info, db, empRepeater);
    }

    protected void btnGenerate_Click(object sender, EventArgs e)
    {
        PageErrors errors = PageErrors.getErrors(db, Page.Master);
        errors.clear();


        DateTime dtPeriodFr = new DateTime();
        DateTime dtPeriodTo = new DateTime();

        if (!(DateTime.TryParse(PeriodFr.Value, out dtPeriodFr) && DateTime.TryParse(PeriodTo.Value, out dtPeriodTo)))
        {
            errors.addError("Invalid Date Format"); 
        }

        if (errors.isEmpty())
        {

            ArrayList list = new ArrayList();
            list = WebUtils.SelectedRepeaterItemToBaseObjectList(EEmpPersonalInfo.db, empRepeater, "ItemSelect");
            GenerateCompensationLeaveEntitlement(list, dtPeriodFr, dtPeriodTo);
            errors.addError("Generated Successful");
        }
        emploadData(info, db, empRepeater);
    }

    protected void GenerateCompensationLeaveEntitlement(ArrayList empInfoList, DateTime PeriodFrom, DateTime PeriodTo)
    {
        foreach (EEmpPersonalInfo empInfo in empInfoList)
        {
            ArrayList compensationLeaveEntitleList = new ArrayList();

            DBFilter attendanceRecordFilter = new DBFilter();
            attendanceRecordFilter.add(new Match("EmpID", empInfo.EmpID));
            attendanceRecordFilter.add(new Match("AttendanceRecordDate", "<=", PeriodTo));
            attendanceRecordFilter.add(new Match("AttendanceRecordDate", ">=", PeriodFrom));
            //attendanceRecordFilter.add(new Match("AttendanceRecordActualOvertimeMins", ">", 0));
            attendanceRecordFilter.add("AttendanceRecordDate", true);

            ArrayList attendanceRecordList = EAttendanceRecord.db.select(dbConn, attendanceRecordFilter);

            EEmpPositionInfo currentEmpPosInfo = null;
            EAttendancePlan currentAttendancePlan = null;
            foreach (EAttendanceRecord attendanceRecord in attendanceRecordList)
            {
                if (currentEmpPosInfo != null)
                    if (currentEmpPosInfo.EmpPosEffFr <= attendanceRecord.AttendanceRecordDate && (currentEmpPosInfo.EmpPosEffTo >= attendanceRecord.AttendanceRecordDate || currentEmpPosInfo.EmpPosEffTo.Ticks.Equals(0)))
                    {
                        currentEmpPosInfo = null;
                        currentAttendancePlan = null;
                    }
                if (currentEmpPosInfo == null)
                {
                    currentEmpPosInfo = AppUtils.GetLastPositionInfo(dbConn, attendanceRecord.AttendanceRecordDate, empInfo.EmpID);
                    EAttendancePlan attendancePlan = new EAttendancePlan();
                    attendancePlan.AttendancePlanID = currentEmpPosInfo.AttendancePlanID;
                    if (EAttendancePlan.db.select(dbConn, attendancePlan))
                        currentAttendancePlan = attendancePlan;
                }
            

                if (currentAttendancePlan != null)
                    if (currentAttendancePlan.AttendancePlanOTGainAsCompensationLeaveEntitle && currentAttendancePlan.AttendancePlanOTMinsUnit > 0)
                    {
                        int actualMins = AppUtils.ApplyRoundingRule(attendanceRecord.AttendanceRecordActualOvertimeMins, currentAttendancePlan.AttendancePlanOTMinsRoundingRule, currentAttendancePlan.AttendancePlanOTMinsUnit);

                        ECompensationLeaveEntitle compLeaveEntitle = new ECompensationLeaveEntitle();
                        compLeaveEntitle.EmpID = empInfo.EmpID;
                        compLeaveEntitle.CompensationLeaveEntitleEffectiveDate = attendanceRecord.AttendanceRecordDate;
                        compLeaveEntitle.CompensationLeaveEntitleClaimPeriodFrom = attendanceRecord.AttendanceRecordDate;
                        compLeaveEntitle.CompensationLeaveEntitleClaimPeriodTo = attendanceRecord.AttendanceRecordDate;
                        compLeaveEntitle.CompensationLeaveEntitleHoursClaim = actualMins / 60.0;
                        compLeaveEntitle.CompensationLeaveEntitleIsAutoGenerated = true;
                        compensationLeaveEntitleList.Add(compLeaveEntitle);
                    }
            }
            SubmitCompensationLeaveEntitlementList(compensationLeaveEntitleList);
            //HROne.LeaveCalc.LeaveBalanceCalc leaaveBalCal = new HROne.LeaveCalc.LeaveBalanceCalc(dbConn, empInfo.EmpID);
            //leaaveBalCal.RecalculateAfter(PeriodFrom , ELeaveType.COMPENSATION_LEAVE_TYPE(dbConn).LeaveTypeID);

        }
    }

    protected void SubmitCompensationLeaveEntitlementList(ArrayList CompensationLeaveEntitlementList)
    {
        foreach (ECompensationLeaveEntitle compLeave in CompensationLeaveEntitlementList)
        {
            DBFilter previousCompLeaveEntitleFilter = new DBFilter();
            previousCompLeaveEntitleFilter.add(new Match("EmpID", compLeave.EmpID));
            previousCompLeaveEntitleFilter.add(new Match("CompensationLeaveEntitleEffectiveDate", compLeave.CompensationLeaveEntitleEffectiveDate));
            previousCompLeaveEntitleFilter.add(new Match("CompensationLeaveEntitleIsAutoGenerated", true));
            ArrayList previousCompLeaveEntitleList = ECompensationLeaveEntitle.db.select(dbConn, previousCompLeaveEntitleFilter);
            if (previousCompLeaveEntitleList.Count > 0)
            {
                if (compLeave.CompensationLeaveEntitleHoursClaim > 0)
                {
                    ECompensationLeaveEntitle existingLeaveEntitle = ((ECompensationLeaveEntitle)previousCompLeaveEntitleList[0]);
                    if (!existingLeaveEntitle.CompensationLeaveEntitleHoursClaim.Equals(compLeave))
                    {
                        compLeave.CompensationLeaveEntitleID = existingLeaveEntitle.CompensationLeaveEntitleID;
                        ECompensationLeaveEntitle.db.update(dbConn, compLeave);
                    }
                }
                else
                {
                    foreach (ECompensationLeaveEntitle existingLeaveEntitle in previousCompLeaveEntitleList)
                    {
                        ECompensationLeaveEntitle.db.delete(dbConn, existingLeaveEntitle);
                    }
                }
            }
            else
                if (compLeave.CompensationLeaveEntitleHoursClaim > 0)
                    ECompensationLeaveEntitle.db.insert(dbConn, compLeave);

        }
    }
}
