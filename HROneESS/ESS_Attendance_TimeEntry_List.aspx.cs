using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using HROne.Lib.Entities;
using HROne.DataAccess;
using Telerik.Web.UI;

public partial class ESS_Attendance_TimeEntry_List : HROneWebPage
{
    protected SearchBinding empSBinding, sbinding;

    int m_ESSUserID = -1;
//    int selectedRosterTableGroupID;

    const string FIELD_EMPID = "EmpID";
    const string FIELD_EMPNO = "EmpNo";
    const string FIELD_EMPFULLNAME = "EmpEngFullName";
    const string FIELD_POSITION = "Position";
    const string FIELD_ATTENDANCERECORDWORKSTART = "AttendanceRecordWorkStart";
    //   const string FIELD_ATTENDANCERECORDLUNCHOUT = "AttendanceRecordLunchOut";
    //   const string FIELD_ATTENDANCERECORDLUNCHIN = "AttendanceRecordLunchIn";
    const string FIELD_ATTENDANCERECORDWORKEND = "AttendanceRecordWorkEnd";
    const string FIELD_LEAVEINFO = "LeaveInfo";
    const string FIELD_ATTENDANCERECORDRC = "AttendanceRecordRC";
    const string FIELD_ROSTERCODEID = "RosterCodeID";
    const string FIELD_DATERANGEFROM = "DateRangeFr";
    const string XML_NODE_NAME_ROSTER_TABLE_GROUP_EMAIL_LIST = "RosterTableGroupNotificationEmailAddress";
    ListInfo info;

    AND commonRosterCodeAndTerms = new AND();
    const DayOfWeek STARTING_WEEK = DayOfWeek.Monday;

    protected int selectedRosterTableGroupID
    {
        get
        {
            int parsedValue;
            if (int.TryParse(hiddenRosterTableGroupID.Value, out parsedValue) == true)
            {
                return parsedValue;
            }
            else
                return -1;
        }

        set
        {
            hiddenRosterTableGroupID.Value = value.ToString();
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!WebUtils.CheckAccess(Response, Session))
            return;

        Year.Attributes.Add("onchange", "year=parseInt(" + Year.ClientID + ".value); if (!(year>1900)) {if (" + Year.ClientID + ".value!='') alert('Invalid Year');return true; }if (" + Month.ClientID + ".selectedIndex==0) return true;");
        Month.Attributes.Add("onchange", "year=parseInt(" + Year.ClientID + ".value); if (!(year>1900)) return true;");

        empSBinding = new SearchBinding(dbConn, EEmpPersonalInfo.db);
        empSBinding.add(new DropDownVLSearchBinder(Month, "Month", Values.VLMonth));
        empSBinding.initValues("EmpStatus", null, EEmpPersonalInfo.VLEmpStatus, HROne.Common.WebUtility.GetSessionUICultureInfo(Session));
        //empSBinding.init(Request.QueryString, null);
        empSBinding.init(DecryptedRequest, null);

        EESSUser user = WebUtils.GetCurUser(Session);
        if (user != null)
            m_ESSUserID = user.EmpID;

        HtmlHead head = (HtmlHead)Page.Header;
        HtmlLink link = new HtmlLink();
        link.Attributes.Add("href", Page.ResolveClientUrl("~/CSS/ModalPopup.css"));
        link.Attributes.Add("type", "text/css");
        link.Attributes.Add("rel", "stylesheet");
        head.Controls.Add(link);

        link = new HtmlLink();
        link.Attributes.Add("href", Page.ResolveClientUrl("~/css.css"));
        link.Attributes.Add("type", "text/css");
        link.Attributes.Add("rel", "stylesheet");
        head.Controls.Add(link);

        info = ListFooter.ListInfo;

        if (!IsPostBack)
        {
            EmployeeSearchControl1.EmpStatusValue = "AT";
        }

        HROne.Common.WebUtility.WebControlsLocalization(this, this.Controls);
    }

    protected void Page_PreRender(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            hiddenRosterTableGroupID.Value = "-1";
            hiddenRosterClientID.Value = "-1";
            hiddenRosterClientSiteID.Value = "-1";

            DateTime currentDateTime = AppUtils.ServerDateTime().Date;
            Year.Text = currentDateTime.Year.ToString();
            Month.SelectedValue = currentDateTime.Month.ToString();
            
            loadWeek();

            DateTime lastDateOfWeek = currentDateTime.AddDays(1);
            while (lastDateOfWeek.DayOfWeek != STARTING_WEEK)
                lastDateOfWeek = lastDateOfWeek.AddDays(1);
            lastDateOfWeek.AddDays(-1);
            int weekNo = lastDateOfWeek.Day / 7;
            if (lastDateOfWeek.Day % 7 > 0)
                weekNo++;

            Week.SelectedValue = weekNo.ToString();

            Week_SelectedIndexChanged(Year, null);
        }
    }

    protected void YearMonth_SelectedIndexChanged(object sender, EventArgs e)
    {
        loadWeek();
        Week_SelectedIndexChanged(Week, EventArgs.Empty);
    }

    protected void loadWeek()
    {
        int intYear = 0;
        int intMonth = 0;

        Week.Items.Clear();
        if (int.TryParse(Year.Text, out intYear) && int.TryParse(Month.SelectedValue, out intMonth))
        {
            if (intYear > 2000 && intYear < 2999 && intMonth >= 1 && intMonth <= 12)
            {
                DateTime firstDayOfMonth = new DateTime(intYear, intMonth, 1);
                int numOfWeek = 0;
                if (firstDayOfMonth.DayOfWeek != STARTING_WEEK)
                {
                    numOfWeek++;
                    while (firstDayOfMonth.DayOfWeek != STARTING_WEEK)
                        firstDayOfMonth = firstDayOfMonth.AddDays(1);
                }
                int numberOfDaysLeft = DateTime.DaysInMonth(intYear, intMonth) - firstDayOfMonth.Day + 1;
                numOfWeek += numberOfDaysLeft / 7;
                if (numberOfDaysLeft % 7 > 0)
                    numOfWeek++;
                for (int idx = 1; idx <= numOfWeek; idx++)
                    Week.Items.Add(new ListItem(idx.ToString()));
            }
        }
    }

    protected void Week_SelectedIndexChanged(object sender, EventArgs e)
    {
        int intWeek = 0;
        int intYear = 0;
        int intMonth = 0;

        if (int.TryParse(Year.Text, out intYear) && int.TryParse(Month.SelectedValue, out intMonth) && int.TryParse(Week.SelectedValue, out intWeek))
        {
            if (intYear > 2000 && intYear < 2999 && intMonth >= 1 && intMonth <= 12)
            {
                ArrayList list = LoadRosterTableGroup(intYear, intMonth, intWeek, m_ESSUserID);

                if (list.Count >= 1)
                {
                    selectedRosterTableGroupID = ((ERosterTableGroup)list[0]).RosterTableGroupID;

                    BuildCommonRosterCodeAndTermsFilter();
                    LoadEmployeeList(intYear, intMonth, intWeek);
                }
            }
        }
    }

    //protected int BuildCommonRosterCodeAndTermsFilter()
    //{
    //if (cboRosterTableGroup.Text == "admin - admin")
    //{
    //    commonRosterCodeAndTerms.terms().Clear();

    //    OR orRosterClientTerm = new OR();
    //    orRosterClientTerm.add(new Match("RosterClientID", 0));
    //    orRosterClientTerm.add(new Match("RosterClientID", 0));
    //    commonRosterCodeAndTerms.add(orRosterClientTerm);

    //    OR orRosterClientSiteTerm = new OR();
    //    orRosterClientSiteTerm.add(new Match("RosterClientSiteID", 0));
    //    orRosterClientSiteTerm.add(new Match("RosterClientSiteID", 0));
    //    commonRosterCodeAndTerms.add(orRosterClientSiteTerm);

    //    return 1;
    //}
    //else
    //{
    //    commonRosterCodeAndTerms.terms().Clear();

    //    OR orRosterClientTerm = new OR();
    //    orRosterClientTerm.add(new Match("RosterClientID", 0));
    //    orRosterClientTerm.add(new Match("RosterClientID", 0));
    //    commonRosterCodeAndTerms.add(orRosterClientTerm);

    //    OR orRosterClientSiteTerm = new OR();
    //    orRosterClientSiteTerm.add(new Match("RosterClientSiteID", 0));
    //    orRosterClientSiteTerm.add(new Match("RosterClientSiteID", 0));
    //    commonRosterCodeAndTerms.add(orRosterClientSiteTerm);

    //    return 2;
    //}

    //convert RosterTableCode from combobox to corresponding ID (i.e. selectedRosterTableGroupID)
    //String rosterTableGroupCode = cboRosterTableGroup.Text;

    // either ^^^^ or vvvv can be used 

    //if (rosterTableGroupCode != "Not Selected" && rosterTableGroupCode != "")
    //{
    //    rosterTableGroupCode = rosterTableGroupCode.Substring(0, rosterTableGroupCode.IndexOf(" - "));

    //    DBFilter groupFilter = new DBFilter();
    //    groupFilter.add(new Match("RosterTableGroupCode", rosterTableGroupCode));

    //    ERosterTableGroup group = (ERosterTableGroup)GetFirstItem(ERosterTableGroup.db.select(dbConn, groupFilter));

    //    if (group != null)
    //    {
    //        selectedRosterTableGroupID = group.RosterTableGroupID;

    //        // class level variable
    //        commonRosterCodeAndTerms.terms().Clear();

    //        OR orRosterClientTerm = new OR();
    //        orRosterClientTerm.add(new Match("RosterClientID", group.RosterClientID));
    //        orRosterClientTerm.add(new Match("RosterClientID", 0));
    //        commonRosterCodeAndTerms.add(orRosterClientTerm);
    //        OR orRosterClientSiteTerm = new OR();
    //        orRosterClientSiteTerm.add(new Match("RosterClientSiteID", group.RosterClientSiteID));
    //        orRosterClientSiteTerm.add(new Match("RosterClientSiteID", 0));
    //        commonRosterCodeAndTerms.add(orRosterClientSiteTerm);

    //        return group.RosterTableGroupID;
    //    }
    //}

    //return -1;
    //}

    protected int BuildCommonRosterCodeAndTermsFilter()
    {
        // temp use only
        //convert RosterTableCode from combobox to corresponding ID (i.e. selectedRosterTableGroupID)
        //String rosterTableGroupCode = cboRosterTableGroup.Text;

        if (selectedRosterTableGroupID > 0)
        {
            DBFilter groupFilter = new DBFilter();
            groupFilter.add(new Match("RosterTableGroupID", selectedRosterTableGroupID));

            ERosterTableGroup group = (ERosterTableGroup)GetFirstItem(ERosterTableGroup.db.select(dbConn, groupFilter));

            if (group != null)
            {
                hiddenRosterTableGroupID.Value = group.RosterTableGroupID.ToString();
                hiddenRosterClientID.Value = group.RosterClientID.ToString();
                hiddenRosterClientSiteID.Value = group.RosterClientSiteID.ToString();

                return group.RosterTableGroupID;
            }
        }
        hiddenRosterTableGroupID.Value = "0";
        hiddenRosterClientID.Value = "0";
        hiddenRosterClientSiteID.Value = "0";

        return -1;
    }


    private void CopyFromLastWeek(int intCurrentYear, int intCurrentMonth, int intCurrentWeek)
    {

        int lineNo = 0;

        // finding date range for input current week data
        DateTime dateThisWeekRangeFr = new DateTime(intCurrentYear, intCurrentMonth, 1);
        while (dateThisWeekRangeFr.DayOfWeek != STARTING_WEEK)
            dateThisWeekRangeFr = dateThisWeekRangeFr.AddDays(-1);

        dateThisWeekRangeFr = dateThisWeekRangeFr.AddDays((intCurrentWeek - 1) * 7);

        foreach (RepeaterItem item in timeCardRecordRepeater.Items)
        {
            lineNo++;
            HtmlInputHidden hiddenEmpID = (HtmlInputHidden)item.FindControl("EmpID");
            int empID = -1;
            if (int.TryParse(hiddenEmpID.Value, out empID))
            {
                for (int dayIdx = 0; dayIdx < 7; dayIdx++)
                {
                    DateTime currentDate = dateThisWeekRangeFr.AddDays(dayIdx);
                    DateTime lastWeekDate = currentDate.AddDays(-7);

                    Panel InOutRecordPanel = (Panel)item.FindControl("HidePanel" + dayIdx);

                    // according to loaded screen settings, find last week records only if current day record is visible.
                    if (InOutRecordPanel.Visible)
                    {
                        DropDownList WorkStartHour = (DropDownList)item.FindControl(FIELD_ATTENDANCERECORDWORKSTART + "Hour" + dayIdx);
                        DropDownList WorkStartMinute = (DropDownList)item.FindControl(FIELD_ATTENDANCERECORDWORKSTART + "Minute" + dayIdx);

                        DropDownList WorkEndHour = (DropDownList)item.FindControl(FIELD_ATTENDANCERECORDWORKEND + "Hour" + dayIdx);
                        DropDownList WorkEndMinute = (DropDownList)item.FindControl(FIELD_ATTENDANCERECORDWORKEND + "Minute" + dayIdx);

                        DropDownList RosterCodeID = (DropDownList)item.FindControl(FIELD_ROSTERCODEID + dayIdx);

                        RosterCodeID.SelectedIndex = 0;
                        WorkStartHour.SelectedIndex = 0;
                        WorkStartMinute.SelectedIndex = 0;
                        WorkEndHour.SelectedIndex = 0;
                        WorkEndMinute.SelectedIndex = 0;


                        // get last week values from db
                        int dayOfWeek = ((TimeSpan)lastWeekDate.Subtract(dateThisWeekRangeFr)).Days;

                        List<ERosterTable> rosterTableList = ERosterTable.GetRosterTableList(dbConn, empID, lastWeekDate);

                        TimeSpan cutOffTimeSpan = new TimeSpan();
                        if (rosterTableList.Count > 0)
                        {
                            ERosterTable currentRosterTable = (ERosterTable)rosterTableList[0];
                            ERosterCode currentRosterCode = new ERosterCode();
                            currentRosterCode.RosterCodeID = currentRosterTable.RosterCodeID;
                            if (ERosterCode.db.select(dbConn, currentRosterCode))
                            {
                                if (currentRosterTable.RosterTableID > 0)
                                {
                                    RosterCodeID.SelectedValue = currentRosterCode.RosterCodeID.ToString();
                                }
                                cutOffTimeSpan = currentRosterCode.RosterCodeCutOffTime.TimeOfDay;
                            }
                        }


                        DBFilter timeCardRecordFilter = new DBFilter();
                        OR orTimeCardRecordHolder = new OR();
                        orTimeCardRecordHolder.add(new Match("EmpID", empID));
                        timeCardRecordFilter.add(orTimeCardRecordHolder);
                        timeCardRecordFilter.add(new Match("TimeCardRecordDateTime", ">=", lastWeekDate.Add(cutOffTimeSpan)));
                        timeCardRecordFilter.add(new Match("TimeCardRecordDateTime", "<", lastWeekDate.AddDays(1).Add(cutOffTimeSpan)));
                        timeCardRecordFilter.add(new Match("TimeCardRecordInOutIndex", ">=", 3));

                        ArrayList timeCardRecordList = ETimeCardRecord.db.select(dbConn, timeCardRecordFilter);


                        foreach (ETimeCardRecord timeCardRecord in timeCardRecordList)
                        {
                            if (timeCardRecord.TimeCardRecordInOutIndex == ETimeCardRecord.TimeCardRecordInOutIndexEnum.WorkStart)
                            {
                                WorkStartHour.Text = timeCardRecord.TimeCardRecordDateTime.Hour.ToString("00");
                                WorkStartMinute.Text = timeCardRecord.TimeCardRecordDateTime.Minute.ToString("00");
                                //attendanceInOutRow[FIELD_ATTENDANCERECORDWORKSTART + dayOfWeek] = timeCardRecord.TimeCardRecordDateTime;
                            }
                            if (timeCardRecord.TimeCardRecordInOutIndex == ETimeCardRecord.TimeCardRecordInOutIndexEnum.WorkEnd)
                            {
                                WorkEndHour.Text = timeCardRecord.TimeCardRecordDateTime.Hour.ToString("00");
                                WorkEndMinute.Text = timeCardRecord.TimeCardRecordDateTime.Minute.ToString("00");
                                //attendanceInOutRow[FIELD_ATTENDANCERECORDWORKEND + dayOfWeek] = timeCardRecord.TimeCardRecordDateTime;
                            }
                        }
                    }
                }
            }
        }
    }

    private void LoadEmployeeList(int intYear, int intMonth, int intWeek)
    {

        Dictionary<int, List<DateTime>> staffRosterDateList = new Dictionary<int, List<DateTime>>();
        DataTable attendanceInOutTable = new DataTable();

        #region "prepareDataTable"
        attendanceInOutTable.Columns.Add(FIELD_DATERANGEFROM, typeof(DateTime));
        attendanceInOutTable.Columns.Add(FIELD_EMPID, typeof(int));
        attendanceInOutTable.Columns.Add(FIELD_EMPNO, typeof(string));
        attendanceInOutTable.Columns.Add(FIELD_EMPFULLNAME, typeof(string));
        attendanceInOutTable.Columns.Add(FIELD_POSITION, typeof(string));
        for (int idx = 0; idx < 7; idx++)
        {
            attendanceInOutTable.Columns.Add(FIELD_ATTENDANCERECORDWORKSTART + idx, typeof(DateTime));
            //            attendanceInOutTable.Columns.Add(FIELD_ATTENDANCERECORDLUNCHOUT + idx, typeof(DateTime));
            //            attendanceInOutTable.Columns.Add(FIELD_ATTENDANCERECORDLUNCHIN + idx, typeof(DateTime));
            attendanceInOutTable.Columns.Add(FIELD_ATTENDANCERECORDWORKEND + idx, typeof(DateTime));
            attendanceInOutTable.Columns.Add(FIELD_ROSTERCODEID + idx, typeof(int));
            attendanceInOutTable.Columns.Add(FIELD_LEAVEINFO + idx, typeof(string));
        }

        DateTime dateRangeFr = new DateTime(intYear, intMonth, 1);
        while (dateRangeFr.DayOfWeek != STARTING_WEEK)
            dateRangeFr = dateRangeFr.AddDays(-1);
        dateRangeFr = dateRangeFr.AddDays((intWeek - 1) * 7);
        DateTime dateRangeTo = dateRangeFr.AddDays(6);

        AttendanceInOutDate0.Text = dateRangeFr.AddDays(0).ToString("dd-MMM");
        AttendanceInOutDate1.Text = dateRangeFr.AddDays(1).ToString("dd-MMM");
        AttendanceInOutDate2.Text = dateRangeFr.AddDays(2).ToString("dd-MMM");
        AttendanceInOutDate3.Text = dateRangeFr.AddDays(3).ToString("dd-MMM");
        AttendanceInOutDate4.Text = dateRangeFr.AddDays(4).ToString("dd-MMM");
        AttendanceInOutDate5.Text = dateRangeFr.AddDays(5).ToString("dd-MMM");
        AttendanceInOutDate6.Text = dateRangeFr.AddDays(6).ToString("dd-MMM");
        #endregion "prepareDataTable"

        if (selectedRosterTableGroupID > 0)
        {
            EEmpRosterTableGroup empRosterInfo = GetSingleEmployeeRosterInfo(dbConn, AppUtils.ServerDateTime().Date, m_ESSUserID, selectedRosterTableGroupID);

            if (empRosterInfo.EmpRosterTableGroupIsSupervisor)
            {
                DateTime actualRangeFr = dateRangeFr;
                DateTime actualRangeTo = dateRangeTo;

                if (actualRangeFr < new DateTime(intYear, intMonth, 1))
                    actualRangeFr = new DateTime(intYear, intMonth, 1);
                if (actualRangeTo > new DateTime(intYear, intMonth, DateTime.DaysInMonth(intYear, intMonth)))
                    actualRangeTo = new DateTime(intYear, intMonth, DateTime.DaysInMonth(intYear, intMonth));

                DBFilter staffEmpRosterFilter = new DBFilter();

                staffEmpRosterFilter.add(new Match("RosterTableGroupID", empRosterInfo.RosterTableGroupID));

                {
                    OR orFromTerm = new OR();
                    orFromTerm.add(new Match("EmpRosterTableGroupEffFr", "<=", actualRangeTo));
                    orFromTerm.add(new NullTerm("EmpRosterTableGroupEffFr"));
                    staffEmpRosterFilter.add(orFromTerm);
                }

                {
                    OR orToTerm = new OR();
                    orToTerm.add(new Match("EmpRosterTableGroupEffTo", ">=", actualRangeFr));
                    orToTerm.add(new NullTerm("EmpRosterTableGroupEffTo"));
                    staffEmpRosterFilter.add(orToTerm);
                }

                ArrayList staffEmpRosterInfoList = EEmpRosterTableGroup.db.select(dbConn, staffEmpRosterFilter);

                foreach (EEmpRosterTableGroup staffEmpRosterInfo in staffEmpRosterInfoList)
                {
                    DateTime staffRangeFr = actualRangeFr;
                    DateTime staffRangeTo = actualRangeTo;

                    if (!staffEmpRosterInfo.EmpRosterTableGroupEffFr.Ticks.Equals(0) && staffEmpRosterInfo.EmpRosterTableGroupEffFr > staffRangeFr)
                        staffRangeFr = staffEmpRosterInfo.EmpRosterTableGroupEffFr;

                    if (!staffEmpRosterInfo.EmpRosterTableGroupEffTo.Ticks.Equals(0) && staffEmpRosterInfo.EmpRosterTableGroupEffTo < staffRangeTo)
                        staffRangeTo = staffEmpRosterInfo.EmpRosterTableGroupEffTo;

                    List<DateTime> availableDateList;

                    if (staffRosterDateList.ContainsKey(staffEmpRosterInfo.EmpID))
                        availableDateList = staffRosterDateList[staffEmpRosterInfo.EmpID];
                    else
                    {
                        availableDateList = new List<DateTime>();
                        staffRosterDateList.Add(staffEmpRosterInfo.EmpID, availableDateList);
                    }

                    for (DateTime staffAvailableDate = staffRangeFr; staffAvailableDate <= staffRangeTo; staffAvailableDate = staffAvailableDate.AddDays(1))
                        if (!availableDateList.Contains(staffAvailableDate))
                            availableDateList.Add(staffAvailableDate);
                }

            }
        }

        foreach (int empID in staffRosterDateList.Keys)
        {
            DBFilter empInfoFilter = EmployeeSearchControl1.GetEmpInfoFilter(dateRangeFr, dateRangeTo);
            empInfoFilter.add(new Match("ee.EmpID", empID));
            DataTable table = empInfoFilter.loadData(dbConn, "SELECT * FROM " + EEmpPersonalInfo.db.dbclass.tableName + " ee");

            if (table.Rows.Count > 0)
                table = EmployeeSearchControl1.FilterEncryptedEmpInfoField(table, new ListInfo());

            if (table.Rows.Count > 0)
            {
                EEmpPersonalInfo empInfo = new EEmpPersonalInfo();
                EEmpPersonalInfo.db.toObject(table.Rows[0], empInfo);

                string timeCardNo = string.Empty;
                if (!string.IsNullOrEmpty(empInfo.EmpTimeCardNo))
                    timeCardNo = empInfo.EmpTimeCardNo;
                else
                    timeCardNo = empInfo.EmpNo;

                DataRow attendanceInOutRow = attendanceInOutTable.NewRow();
                attendanceInOutRow[FIELD_EMPID] = empID;
                attendanceInOutRow[FIELD_EMPNO] = empInfo.EmpNo;
                attendanceInOutRow[FIELD_EMPFULLNAME] = empInfo.EmpEngFullNameWithAlias;
                attendanceInOutRow[FIELD_DATERANGEFROM] = dateRangeFr;

                EEmpPositionInfo empPos = AppUtils.GetLastPositionInfo(dbConn, dateRangeTo, empID);
                if (empPos != null)
                {
                    EPosition position = new EPosition();
                    position.PositionID = empPos.PositionID;
                    if (EPosition.db.select(dbConn, position))
                        attendanceInOutRow[FIELD_POSITION] = position.PositionDesc;
                }

                #region "load date values"

                List<DateTime> availableDateList = staffRosterDateList[empID];
                foreach (DateTime currentDate in availableDateList)
                {
                    int dayOfWeek = ((TimeSpan)currentDate.Subtract(dateRangeFr)).Days;

                    List<ERosterTable> rosterTableList = ERosterTable.GetRosterTableList(dbConn, empID, currentDate);


                    TimeSpan cutOffTimeSpan = new TimeSpan();
                    if (rosterTableList.Count > 0)
                    {
                        ERosterTable currentRosterTable = (ERosterTable)rosterTableList[0];
                        ERosterCode currentRosterCode = new ERosterCode();
                        currentRosterCode.RosterCodeID = currentRosterTable.RosterCodeID;
                        if (ERosterCode.db.select(dbConn, currentRosterCode))
                        {
                            if (currentRosterTable.RosterTableID > 0)
                                attendanceInOutRow[FIELD_ROSTERCODEID + dayOfWeek] = currentRosterCode.RosterCodeID;
                            cutOffTimeSpan = currentRosterCode.RosterCodeCutOffTime.TimeOfDay;
                        }
                    }


                    DBFilter timeCardRecordFilter = new DBFilter();
                    OR orTimeCardRecordHolder = new OR();
                    orTimeCardRecordHolder.add(new Match("EmpID", empID));
                    timeCardRecordFilter.add(orTimeCardRecordHolder);
                    timeCardRecordFilter.add(new Match("TimeCardRecordDateTime", ">=", currentDate.Add(cutOffTimeSpan)));
                    timeCardRecordFilter.add(new Match("TimeCardRecordDateTime", "<", currentDate.AddDays(1).Add(cutOffTimeSpan)));
                    timeCardRecordFilter.add(new Match("TimeCardRecordInOutIndex", ">=", 3));

                    ArrayList timeCardRecordList = ETimeCardRecord.db.select(dbConn, timeCardRecordFilter);

                    foreach (ETimeCardRecord timeCardRecord in timeCardRecordList)
                    {
                        if (timeCardRecord.TimeCardRecordInOutIndex == ETimeCardRecord.TimeCardRecordInOutIndexEnum.WorkStart)
                            attendanceInOutRow[FIELD_ATTENDANCERECORDWORKSTART + dayOfWeek] = timeCardRecord.TimeCardRecordDateTime;
                        if (timeCardRecord.TimeCardRecordInOutIndex == ETimeCardRecord.TimeCardRecordInOutIndexEnum.WorkEnd)
                            attendanceInOutRow[FIELD_ATTENDANCERECORDWORKEND + dayOfWeek] = timeCardRecord.TimeCardRecordDateTime;
                    }


                    // retrieve employee leave information of the day                    
                    DBFilter leaveAppFilter = new DBFilter();
                    ELeaveApplication leaveAppData = new ELeaveApplication();
                    leaveAppFilter.add(new Match("EmpID", empID));
                    leaveAppFilter.add(new Match("LeaveAppDateFrom", "<=", currentDate.Add(cutOffTimeSpan)));
                    leaveAppFilter.add(new Match("LeaveAppDateTo", ">=", currentDate.Add(cutOffTimeSpan)));

                    ArrayList leaveAppDataList = ELeaveApplication.db.select(dbConn, leaveAppFilter);

                    string s = "";
                    foreach (ELeaveApplication itemData in leaveAppDataList)
                    {
                        if (s != "")
                        {
                            s = s + "<br>";
                        }

                        // get leave code master
                        DBFilter leaveCodeMasterFilter = new DBFilter();
                        ELeaveCode leaveCodeMasterData = new ELeaveCode();
                        leaveCodeMasterFilter.add(new Match("leaveCodeID", itemData.LeaveCodeID));
                        ArrayList leaveCodeMasterDataList = ELeaveCode.db.select(dbConn, leaveCodeMasterFilter);

                        if (leaveCodeMasterDataList != null)
                        {
                            s = s + ((ELeaveCode)leaveCodeMasterDataList[0]).LeaveCode + " - " + ((ELeaveCode)leaveCodeMasterDataList[0]).LeaveCodeDesc;
                        }
                    }
                    if (s != "")    // display leave information for the employe on this day !
                    {
                        attendanceInOutRow[FIELD_LEAVEINFO + dayOfWeek] = s;
                    }
                }

                #endregion "load date values"

                attendanceInOutTable.Rows.Add(attendanceInOutRow);
            }

        }
        attendanceInOutTable = WebUtils.DataTableSortingAndPaging(attendanceInOutTable, info);
        timeCardRecordRepeater.DataSource = attendanceInOutTable;
        timeCardRecordRepeater.DataBind();

        if (attendanceInOutTable.Rows.Count > 0)
            SubmitPanel.Visible = true;
        else
            SubmitPanel.Visible = false;
    }

    protected void Search_Click(object sender, EventArgs e)
    {
        Week_SelectedIndexChanged(Week, EventArgs.Empty);
    }

    protected void Reset_Click(object sender, EventArgs e)
    {
        //binding.clear();
        EmployeeSearchControl1.Reset();
        EmployeeSearchControl1.EmpStatusValue = "AT";   // active or terminated
        //info.page = 0;

        //view = loadData(info, db, Repeater);
        Week_SelectedIndexChanged(Week, EventArgs.Empty);

    }
    protected void ChangeOrder_Click(object sender, EventArgs e)
    {
        LinkButton l = (LinkButton)sender;
        String id = l.ID.Substring(1);
        //loadState();
        if (info.orderby == null)
            info.order = true;
        else if (info.orderby.Equals(id))
            info.order = !info.order;
        else
            info.order = true;
        info.orderby = id;
        Week_SelectedIndexChanged(Week, EventArgs.Empty);
    }

    protected void Repeater_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        DataRowView row = (DataRowView)e.Item.DataItem;
        HtmlInputHidden hiddenEmpID = (HtmlInputHidden)e.Item.FindControl("EmpID");
        hiddenEmpID.Value = row[FIELD_EMPID].ToString();
        EEmpTermination empTermination = EEmpTermination.GetObjectByEmpID(dbConn, (int)row[FIELD_EMPID]);
        EEmpPersonalInfo empInfo = new EEmpPersonalInfo();
        empInfo.EmpID = (int)row[FIELD_EMPID];
        EEmpPersonalInfo.db.select(dbConn, empInfo);

        int intYear = 0;
        int intMonth = 0;
        if (int.TryParse(Year.Text, out intYear) && int.TryParse(Month.SelectedValue, out intMonth))
        {
            DateTime firstDayOfMonth = new DateTime(intYear, intMonth, 1);
            DateTime lastDayOfMonth = new DateTime(intYear, intMonth, DateTime.DaysInMonth(intYear, intMonth));

            for (int idx = 0; idx < 7; idx++)
            {
                DateTime currentDate = ((DateTime)row[FIELD_DATERANGEFROM]).AddDays(idx);

                Panel InOutRecordPanel = (Panel)e.Item.FindControl("HidePanel" + idx);
                if (currentDate < firstDayOfMonth || currentDate > lastDayOfMonth)
                    InOutRecordPanel.Visible = false;
                else
                {
                    InOutRecordPanel.Visible = true;
                    if (((empTermination != null) && empTermination.EmpTermLastDate < currentDate) || (empInfo.EmpDateOfJoin > currentDate))
                        InOutRecordPanel.Visible = false;

                    EEmpRosterTableGroup group = (EEmpRosterTableGroup)GetFirstItem(GetEmployeeRosterInfo(dbConn, currentDate, (int)row[FIELD_EMPID]));
                    if (group == null)
                        InOutRecordPanel.Visible = false;
                    else
                    {
                        if (group.RosterTableGroupID != selectedRosterTableGroupID)
                            InOutRecordPanel.Visible = false;
                    }

                }
                if (InOutRecordPanel.Visible)
                {

                    DropDownList WorkStartHour = (DropDownList)e.Item.FindControl(FIELD_ATTENDANCERECORDWORKSTART + "Hour" + idx);
                    DropDownList WorkStartMinute = (DropDownList)e.Item.FindControl(FIELD_ATTENDANCERECORDWORKSTART + "Minute" + idx);

                    DropDownList WorkEndHour = (DropDownList)e.Item.FindControl(FIELD_ATTENDANCERECORDWORKEND + "Hour" + idx);
                    DropDownList WorkEndMinute = (DropDownList)e.Item.FindControl(FIELD_ATTENDANCERECORDWORKEND + "Minute" + idx);

                    DropDownList RosterCodeID = (DropDownList)e.Item.FindControl(FIELD_ROSTERCODEID + idx);

                    Label leaveInfo = (Label)e.Item.FindControl(FIELD_LEAVEINFO + idx);
                    leaveInfo.Text = "--";


                    WorkStartHour.Items.Clear();
                    WorkEndHour.Items.Clear();
                    WorkStartMinute.Items.Clear();
                    WorkEndMinute.Items.Clear();
                    RosterCodeID.Items.Clear();

                    WorkStartHour.Items.Add(new ListItem("--", string.Empty));
                    WorkEndHour.Items.Add(new ListItem("--", string.Empty));

                    WorkStartMinute.Items.Add(new ListItem("--", string.Empty));
                    WorkEndMinute.Items.Add(new ListItem("--", string.Empty));

                    for (int hour = 0; hour < 24; hour++)
                    {
                        WorkStartHour.Items.Add(hour.ToString("00"));
                        WorkEndHour.Items.Add(hour.ToString("00"));
                    }

                    for (int minute = 0; minute < 60; minute += 15)
                    {
                        WorkStartMinute.Items.Add(minute.ToString("00"));
                        WorkEndMinute.Items.Add(minute.ToString("00"));
                    }

                    // class level variable
                    commonRosterCodeAndTerms.terms().Clear();

                    OR orRosterClientTerm = new OR();
                    orRosterClientTerm.add(new Match("RosterClientID", int.Parse(hiddenRosterClientID.Value)));
                    orRosterClientTerm.add(new Match("RosterClientID", 0));
                    commonRosterCodeAndTerms.add(orRosterClientTerm);
                    OR orRosterClientSiteTerm = new OR();
                    orRosterClientSiteTerm.add(new Match("RosterClientSiteID", int.Parse(hiddenRosterClientSiteID.Value)));
                    orRosterClientSiteTerm.add(new Match("RosterClientSiteID", 0));
                    commonRosterCodeAndTerms.add(orRosterClientSiteTerm);

                    OR orRosterCodeTerms = new OR();
                    orRosterCodeTerms.add(commonRosterCodeAndTerms);
                    if (row[FIELD_ROSTERCODEID + idx] != DBNull.Value)
                    {
                        orRosterCodeTerms.add(new Match("RosterCodeID", (int)row[FIELD_ROSTERCODEID + idx]));

                    }
                    DBFilter rosterCodeFilter = new DBFilter();
                    rosterCodeFilter.add(orRosterCodeTerms);

                    HROne.DataAccess.WebFormUtils.loadValues(dbConn, RosterCodeID, ERosterCode.VLRosterCode, rosterCodeFilter);

                    if (row[FIELD_ATTENDANCERECORDWORKSTART + idx] != DBNull.Value)
                    {

                        DateTime date = ((DateTime)row[FIELD_ATTENDANCERECORDWORKSTART + idx]);
                        WorkStartHour.SelectedValue = date.ToString("HH");
                        WorkStartMinute.SelectedValue = date.ToString("mm");
                    }

                    if (row[FIELD_ATTENDANCERECORDWORKEND + idx] != DBNull.Value)
                    {
                        DateTime date = ((DateTime)row[FIELD_ATTENDANCERECORDWORKEND + idx]);
                        WorkEndHour.SelectedValue = date.ToString("HH");
                        WorkEndMinute.SelectedValue = date.ToString("mm");
                    }

                    if (row[FIELD_LEAVEINFO + idx] != DBNull.Value)
                    {
                        leaveInfo.Text = row[FIELD_LEAVEINFO + idx].ToString();
                    }


                    if (row[FIELD_ROSTERCODEID + idx] != DBNull.Value)
                    {
                        RosterCodeID.SelectedValue = RosterCodeID.Items.FindByValue(row[FIELD_ROSTERCODEID + idx].ToString()).Value;
                    }
                }

            }
        }
    }

    protected void AttendanceRecordWorkStart_SelectIndexChange(object sender, EventArgs e)
    {
        string sourceID = ((DropDownList)sender).ID;
        string idx = sourceID.Substring(sourceID.Length - 1);

        RepeaterItem item = null;
        Control parentControl = ((Control)sender).Parent;
        while (item == null && parentControl.Parent != null)
        {
            parentControl = parentControl.Parent;
            if (parentControl is RepeaterItem)
                item = (RepeaterItem)parentControl;
        }
        if (item != null)
        {
            DropDownList WorkStartHour = (DropDownList)item.FindControl(FIELD_ATTENDANCERECORDWORKSTART + "Hour" + idx);
            DropDownList WorkStartMinute = (DropDownList)item.FindControl(FIELD_ATTENDANCERECORDWORKSTART + "Minute" + idx);
            DropDownList RosterCodeID = (DropDownList)item.FindControl(FIELD_ROSTERCODEID + idx);
            DropDownList WorkEndHour = (DropDownList)item.FindControl(FIELD_ATTENDANCERECORDWORKEND + "Hour" + idx);
            DropDownList WorkEndMinute = (DropDownList)item.FindControl(FIELD_ATTENDANCERECORDWORKEND + "Minute" + idx);

            if (RosterCodeID.SelectedValue.Equals(string.Empty) && !WorkStartHour.SelectedValue.Equals(string.Empty) && !WorkStartMinute.SelectedValue.Equals(string.Empty)
                && WorkEndHour.SelectedValue.Equals(string.Empty) && WorkEndMinute.SelectedValue.Equals(string.Empty)
                )
            {
                TimeSpan workStart = new TimeSpan(int.Parse(WorkStartHour.SelectedValue), int.Parse(WorkStartMinute.SelectedValue), 0);

                // class level variable
                commonRosterCodeAndTerms.terms().Clear();

                OR orRosterClientTerm = new OR();
                orRosterClientTerm.add(new Match("RosterClientID", int.Parse(hiddenRosterClientID.Value)));
                orRosterClientTerm.add(new Match("RosterClientID", 0));
                commonRosterCodeAndTerms.add(orRosterClientTerm);
                OR orRosterClientSiteTerm = new OR();
                orRosterClientSiteTerm.add(new Match("RosterClientSiteID", int.Parse(hiddenRosterClientSiteID.Value)));
                orRosterClientSiteTerm.add(new Match("RosterClientSiteID", 0));
                commonRosterCodeAndTerms.add(orRosterClientSiteTerm);

                OR orRosterCodeTerms = new OR();
                orRosterCodeTerms.add(commonRosterCodeAndTerms);
                if (!string.IsNullOrEmpty(RosterCodeID.SelectedValue))
                {
                    orRosterCodeTerms.add(new Match("RosterCodeID", RosterCodeID.SelectedValue));
                }
                DBFilter rosterCodeFilter = new DBFilter();
                rosterCodeFilter.add(orRosterCodeTerms);

                OR orRosterCodeTypeTerms = new OR();
                orRosterCodeTypeTerms.add(new Match("RosterCodeType", ERosterCode.ROSTERTYPE_CODE_NORMAL));
                orRosterCodeTypeTerms.add(new Match("RosterCodeType", ERosterCode.ROSTERTYPE_CODE_OVERNIGHT));
                rosterCodeFilter.add(orRosterCodeTypeTerms);

                ArrayList rosterCodeList = ERosterCode.db.select(dbConn, rosterCodeFilter);
                double minMinutesDiff = 9999;
                ERosterCode selectedRosterCode = null;
                foreach (ERosterCode rosterCode in rosterCodeList)
                {
                    if (RosterCodeID.Items.FindByValue(rosterCode.RosterCodeID.ToString()) != null)
                        if (!rosterCode.RosterCodeInTime.Ticks.Equals(0))
                        {

                            double minutesDiff = Math.Abs(rosterCode.RosterCodeInTime.TimeOfDay.Subtract(workStart).TotalMinutes);
                            if (minutesDiff < 60 && minutesDiff < minMinutesDiff)
                            {
                                minMinutesDiff = minutesDiff;
                                selectedRosterCode = rosterCode;
                            }
                        }
                }
                if (selectedRosterCode != null)
                {
                    if (!selectedRosterCode.RosterCodeOutTime.Ticks.Equals(0))
                    {
                        WorkEndHour.SelectedValue = selectedRosterCode.RosterCodeOutTime.ToString("HH");
                        WorkEndMinute.SelectedValue = selectedRosterCode.RosterCodeOutTime.ToString("mm");
                        RosterCodeID.SelectedValue = selectedRosterCode.RosterCodeID.ToString();
                    }
                }
            }
        }
    }

    protected void AttendanceRecordRC_SelectIndexChange(object sender, EventArgs e)
    {
        string sourceID = ((DropDownList)sender).ID;
        string idx = sourceID.Substring(sourceID.Length - 1);

        RepeaterItem item = null;
        Control parentControl = ((Control)sender).Parent;
        while (item == null && parentControl.Parent != null)
        {
            parentControl = parentControl.Parent;
            if (parentControl is RepeaterItem)
                item = (RepeaterItem)parentControl;
        }
        if (item != null)
        {
            DropDownList WorkStartHour = (DropDownList)item.FindControl(FIELD_ATTENDANCERECORDWORKSTART + "Hour" + idx);
            DropDownList WorkStartMinute = (DropDownList)item.FindControl(FIELD_ATTENDANCERECORDWORKSTART + "Minute" + idx);
            DropDownList RosterCodeID = (DropDownList)item.FindControl(FIELD_ROSTERCODEID + idx);

            DropDownList WorkEndHour = (DropDownList)item.FindControl(FIELD_ATTENDANCERECORDWORKEND + "Hour" + idx);
            DropDownList WorkEndMinute = (DropDownList)item.FindControl(FIELD_ATTENDANCERECORDWORKEND + "Minute" + idx);

            if (!RosterCodeID.SelectedValue.Equals(string.Empty))
            {
                ERosterCode RosterCode = new ERosterCode();
                RosterCode.RosterCodeID = int.Parse(RosterCodeID.SelectedValue);
                if (ERosterCode.db.select(dbConn, RosterCode))
                {
                    if (RosterCode.RosterCodeType.Equals(ERosterCode.ROSTERTYPE_CODE_NORMAL) || RosterCode.RosterCodeType.Equals(ERosterCode.ROSTERTYPE_CODE_OVERNIGHT))
                    {
                        if (WorkStartHour.SelectedValue.Equals(string.Empty) && WorkStartMinute.SelectedValue.Equals(string.Empty)
                            && WorkEndHour.SelectedValue.Equals(string.Empty) && WorkEndMinute.SelectedValue.Equals(string.Empty)
                            )
                        {
                            WorkStartHour.SelectedValue = RosterCode.RosterCodeInTime.ToString("HH");
                            WorkStartMinute.SelectedValue = RosterCode.RosterCodeInTime.ToString("mm");
                            WorkEndHour.SelectedValue = RosterCode.RosterCodeOutTime.ToString("HH");
                            WorkEndMinute.SelectedValue = RosterCode.RosterCodeOutTime.ToString("mm");
                        }
                    }
                    else
                    {
                        WorkStartHour.SelectedValue = string.Empty;
                        WorkStartMinute.SelectedValue = string.Empty;
                        WorkEndHour.SelectedValue = string.Empty;
                        WorkEndMinute.SelectedValue = string.Empty;
                    }
                }
            }
        }
    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        SaveTimeCardRecords();
        PageErrors errors = PageErrors.getErrors(ETimeCardRecord.db, this.Master);
        if (!errors.isEmpty())
            return;

        int intWeek = 0;
        int intYear = 0;
        int intMonth = 0;
        if (int.TryParse(Year.Text, out intYear) && int.TryParse(Month.SelectedValue, out intMonth) && int.TryParse(Week.SelectedValue, out intWeek))
        {
            if (intYear > 2000 && intYear < 2999 && intMonth >= 1 && intMonth <= 12)
            {
                System.IO.FileInfo file = GenerateExcelFile();

                if (selectedRosterTableGroupID > 0)
                {
                    ERosterTableGroup rosterTableGroup = new ERosterTableGroup();
                    rosterTableGroup.RosterTableGroupID = selectedRosterTableGroupID;
                    if (ERosterTableGroup.db.select(dbConn, rosterTableGroup))
                    {
                        string emailList = string.Join(";", rosterTableGroup.GetRosterTableGroupExtendData(XML_NODE_NAME_ROSTER_TABLE_GROUP_EMAIL_LIST).Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries));
                        if (!string.IsNullOrEmpty(emailList))
                        {
                            DateTime dateRangeFr = new DateTime(intYear, intMonth, 1);
                            while (dateRangeFr.DayOfWeek != STARTING_WEEK)
                                dateRangeFr = dateRangeFr.AddDays(-1);
                            dateRangeFr = dateRangeFr.AddDays((intWeek - 1) * 7);
                            DateTime dateRangeTo = dateRangeFr.AddDays(6);

                            string subject = rosterTableGroup.RosterTableGroupCode + " is submitted (" + dateRangeFr.ToString("dd/MM") + " - " + dateRangeTo.ToString("dd/MM") + ")";
                            string message = "Submitted By:\t" + WebUtils.GetCurUser(Session).EmpEngFullNameWithAlias;
                            if (AppUtils.Sent_Email(dbConn, emailList, string.Empty, string.Empty, string.Empty, subject, message, file.FullName, "AttendanceTimeEntry_" + AppUtils.ServerDateTime().ToString("yyyyMMddHHmmss") + ".xls", true))
                                errors.addError("E-mail is sent");

                        }
                    }
                }

            }
        }
    }

    protected void btnCopyFromLastWeek_Click(object sender, EventArgs e)
    {
        int intCurrentWeek = 0;
        int intCurrentMonth = 0;
        int intCurrentYear = 0;

        if (int.TryParse(Year.Text, out intCurrentYear) && int.TryParse(Month.SelectedValue, out intCurrentMonth) && int.TryParse(Week.SelectedValue, out intCurrentWeek))
        {
            if (intCurrentYear > 2000 && intCurrentYear < 2999 && intCurrentMonth >= 1 && intCurrentMonth <= 12)
            {
                CopyFromLastWeek(intCurrentYear, intCurrentMonth, intCurrentWeek);
            }
        }

        PageErrors errors = PageErrors.getErrors(ETimeCardRecord.db, this.Master);
        if (errors.isEmpty())
            errors.addError("Last week data copied");
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        SaveTimeCardRecords();
        PageErrors errors = PageErrors.getErrors(ETimeCardRecord.db, this.Master);
        if (errors.isEmpty())
            errors.addError("Saved");

    }

    protected void btnExport_Click(object sender, EventArgs e)
    {
        SaveTimeCardRecords();
        PageErrors errors = PageErrors.getErrors(ETimeCardRecord.db, this.Master);
        if (errors.isEmpty())
        {
            System.IO.FileInfo file = GenerateExcelFile();
            TransmitFile(Response, file.FullName, "AttendanceTimeEntry_" + AppUtils.ServerDateTime().ToString("yyyyMMddHHmmss") + ".xls", true);
        }
    }

    protected void SaveTimeCardRecords()
    {
        PageErrors errors = PageErrors.getErrors(ETimeCardRecord.db, this.Master);
        int lineNo = 0;
        foreach (RepeaterItem item in timeCardRecordRepeater.Items)
        {
            lineNo++;
            for (int idx = 0; idx < 7; idx++)
            {
                DropDownList WorkStartHour = (DropDownList)item.FindControl(FIELD_ATTENDANCERECORDWORKSTART + "Hour" + idx);
                DropDownList WorkStartMinute = (DropDownList)item.FindControl(FIELD_ATTENDANCERECORDWORKSTART + "Minute" + idx);

                DropDownList WorkEndHour = (DropDownList)item.FindControl(FIELD_ATTENDANCERECORDWORKEND + "Hour" + idx);
                DropDownList WorkEndMinute = (DropDownList)item.FindControl(FIELD_ATTENDANCERECORDWORKEND + "Minute" + idx);

                if ((WorkStartHour.SelectedValue.Equals(string.Empty) && !WorkStartMinute.SelectedValue.Equals(string.Empty))
                || (!WorkStartHour.SelectedValue.Equals(string.Empty) && WorkStartMinute.SelectedValue.Equals(string.Empty)))
                {
                    errors.addError("Invalid input on line " + lineNo.ToString() + "of " + "\"In\"" + "field");
                }

                if ((WorkEndHour.SelectedValue.Equals(string.Empty) && !WorkEndMinute.SelectedValue.Equals(string.Empty))
                || (!WorkEndHour.SelectedValue.Equals(string.Empty) && WorkEndMinute.SelectedValue.Equals(string.Empty)))
                {
                    errors.addError("Invalid input on line " + lineNo.ToString() + "of " + "\"Out\"" + "field");
                }
            }

        }

        if (!errors.isEmpty())
            return;

        int intWeek = 0;
        int intYear = 0;
        int intMonth = 0;

        if (int.TryParse(Year.Text, out intYear) && int.TryParse(Month.SelectedValue, out intMonth) && int.TryParse(Week.SelectedValue, out intWeek))
        {
            DateTime firstDayOfMonth = new DateTime(intYear, intMonth, 1);
            DateTime lastDayOfMonth = new DateTime(intYear, intMonth, DateTime.DaysInMonth(intYear, intMonth));

            DateTime dateRangeFr = new DateTime(intYear, intMonth, 1);
            while (dateRangeFr.DayOfWeek != STARTING_WEEK)
                dateRangeFr = dateRangeFr.AddDays(-1);
            dateRangeFr = dateRangeFr.AddDays((intWeek - 1) * 7);

            foreach (RepeaterItem item in timeCardRecordRepeater.Items)
            {
                lineNo++;
                HtmlInputHidden hiddenEmpID = (HtmlInputHidden)item.FindControl("EmpID");
                int empID = -1;
                if (int.TryParse(hiddenEmpID.Value, out empID))
                {
                    for (int dayIdx = 0; dayIdx < 7; dayIdx++)
                    {
                        DateTime currentDate = dateRangeFr.AddDays(dayIdx);

                        Panel InOutRecordPanel = (Panel)item.FindControl("HidePanel" + dayIdx);
                        if (!InOutRecordPanel.Visible)
                            continue;
                        DropDownList WorkStartHour = (DropDownList)item.FindControl(FIELD_ATTENDANCERECORDWORKSTART + "Hour" + dayIdx);
                        DropDownList WorkStartMinute = (DropDownList)item.FindControl(FIELD_ATTENDANCERECORDWORKSTART + "Minute" + dayIdx);

                        DropDownList WorkEndHour = (DropDownList)item.FindControl(FIELD_ATTENDANCERECORDWORKEND + "Hour" + dayIdx);
                        DropDownList WorkEndMinute = (DropDownList)item.FindControl(FIELD_ATTENDANCERECORDWORKEND + "Minute" + dayIdx);

                        DropDownList RosterCodeID = (DropDownList)item.FindControl(FIELD_ROSTERCODEID + dayIdx);

                        List<ERosterTable> rosterTableList = ERosterTable.GetRosterTableList(dbConn, empID, currentDate);

                        TimeSpan cutOffTimeSpan = new TimeSpan();
                        if (rosterTableList.Count > 0)
                        {
                            ERosterTable currentRosterTable = (ERosterTable)rosterTableList[0];
                            ERosterCode currentRosterCode = new ERosterCode();
                            currentRosterCode.RosterCodeID = currentRosterTable.RosterCodeID;
                            if (ERosterCode.db.select(dbConn, currentRosterCode))
                            {
                                //activeRosterCodeID = currentRosterCode.RosterCodeID;
                                cutOffTimeSpan = currentRosterCode.RosterCodeCutOffTime.TimeOfDay;
                            }
                        }

                        if (!WorkStartHour.SelectedValue.Equals(string.Empty) && !WorkStartMinute.SelectedValue.Equals(string.Empty))
                            CreateOrUpdateTimeCardRecord(empID, currentDate, cutOffTimeSpan, ETimeCardRecord.TimeCardRecordInOutIndexEnum.WorkStart, new TimeSpan(int.Parse(WorkStartHour.SelectedValue), int.Parse(WorkStartMinute.SelectedValue), 0), false);
                        else
                            CreateOrUpdateTimeCardRecord(empID, currentDate, cutOffTimeSpan, ETimeCardRecord.TimeCardRecordInOutIndexEnum.WorkStart, new TimeSpan(), true);

                        if (!WorkEndHour.SelectedValue.Equals(string.Empty) && !WorkEndMinute.SelectedValue.Equals(string.Empty))
                            CreateOrUpdateTimeCardRecord(empID, currentDate, cutOffTimeSpan, ETimeCardRecord.TimeCardRecordInOutIndexEnum.WorkEnd, new TimeSpan(int.Parse(WorkEndHour.SelectedValue), int.Parse(WorkEndMinute.SelectedValue), 0), false);
                        else
                            CreateOrUpdateTimeCardRecord(empID, currentDate, cutOffTimeSpan, ETimeCardRecord.TimeCardRecordInOutIndexEnum.WorkEnd, new TimeSpan(), true);



                        if (rosterTableList.Count > 0)
                        {
                            foreach (ERosterTable rosterTable in rosterTableList)
                            {
                                if (rosterTable.RosterTableID > 0)
                                {
                                    ERosterTable.db.delete(dbConn, rosterTable);
                                }
                            }
                        }
                        if (!RosterCodeID.SelectedValue.Equals(string.Empty))
                        {
                            ERosterCode rosterCode = new ERosterCode();
                            rosterCode.RosterCodeID = int.Parse(RosterCodeID.SelectedValue);
                            if (ERosterCode.db.select(dbConn, rosterCode))
                            {
                                ERosterTable rosterTable = new ERosterTable();
                                rosterTable.EmpID = empID;
                                rosterTable.RosterTableDate = currentDate;
                                rosterTable.RosterCodeID = rosterCode.RosterCodeID;

                                ERosterTable.db.insert(dbConn, rosterTable);
                            }
                        }
                    }

                }
            }
        }
    }
    protected void CreateOrUpdateTimeCardRecord(int empID, DateTime timeCardRecordDate, TimeSpan cutOffTimeSpan, ETimeCardRecord.TimeCardRecordInOutIndexEnum InOutIndex, TimeSpan newTime, bool deleteRecord)
    {
        DBFilter timeCardRecordFilter = new DBFilter();
        OR orTimeCardRecordHolder = new OR();
        orTimeCardRecordHolder.add(new Match("EmpID", empID));

        timeCardRecordFilter.add(orTimeCardRecordHolder);
        timeCardRecordFilter.add(new Match("TimeCardRecordDateTime", ">=", timeCardRecordDate.Add(cutOffTimeSpan)));
        timeCardRecordFilter.add(new Match("TimeCardRecordDateTime", "<", timeCardRecordDate.AddDays(1).Add(cutOffTimeSpan)));
        timeCardRecordFilter.add(new Match("TimeCardRecordInOutIndex", "=", InOutIndex));
        if (deleteRecord)
            ETimeCardRecord.db.delete(dbConn, timeCardRecordFilter);
        else
        {
            ArrayList timeCardRecordList = ETimeCardRecord.db.select(dbConn, timeCardRecordFilter);

            ETimeCardRecord timecardRecord = null;
            for (int idx = 0; idx < timeCardRecordList.Count; idx++)
                if (idx == 0)
                    timecardRecord = (ETimeCardRecord)timeCardRecordList[0];
                else
                    ETimeCardRecord.db.delete(dbConn, (ETimeCardRecord)timeCardRecordList[0]);
            if (timecardRecord == null)
            {
                timecardRecord = new ETimeCardRecord();
                timecardRecord.EmpID = empID;
                timecardRecord.TimeCardRecordInOutIndex = InOutIndex;
            }
            timecardRecord.TimeCardRecordDateTime = timeCardRecordDate.Add(newTime);
            if (timecardRecord.TimeCardRecordID <= 0)
                ETimeCardRecord.db.insert(dbConn, timecardRecord);
            else
                ETimeCardRecord.db.update(dbConn, timecardRecord);
        }
    }

    protected System.IO.FileInfo GenerateExcelFile()
    {

        int intWeek = 0;
        int intYear = 0;
        int intMonth = 0;

        if (int.TryParse(Year.Text, out intYear) && int.TryParse(Month.SelectedValue, out intMonth) && int.TryParse(Week.SelectedValue, out intWeek))
        {
            List<EHierarchyLevel> hLevelList = new List<EHierarchyLevel>();
            {
                DBFilter hierarchyLevelFilter = new DBFilter();
                //hierarchyLevelFilter.add(new Match("HLevelSeqNo", "<=", 2));
                hierarchyLevelFilter.add("HLevelSeqNo", true);
                ArrayList tmpHLevelList = EHierarchyLevel.db.select(dbConn, hierarchyLevelFilter);
                foreach (EHierarchyLevel hLevel in tmpHLevelList)
                    if (hLevelList.Count < 2)
                        hLevelList.Add(hLevel);
                    else
                        break;
            }
            DateTime dateRangeFr = new DateTime(intYear, intMonth, 1);
            while (dateRangeFr.DayOfWeek != STARTING_WEEK)
                dateRangeFr = dateRangeFr.AddDays(-1);
            dateRangeFr = dateRangeFr.AddDays((intWeek - 1) * 7);

            NPOI.HSSF.UserModel.HSSFWorkbook workbook = new NPOI.HSSF.UserModel.HSSFWorkbook();
            NPOI.HSSF.UserModel.HSSFSheet workSheet = (NPOI.HSSF.UserModel.HSSFSheet)workbook.CreateSheet();
            workSheet.SetColumnWidth(0, 4000);
            workSheet.SetColumnWidth(1, 4000);
            workSheet.SetColumnWidth(2, 9000);
            workSheet.SetColumnWidth(3, 4000);
            workSheet.SetColumnWidth(4, 2000);
            for (int dayIdx = 0; dayIdx < 7; dayIdx++)
            {
                workSheet.SetColumnWidth(3 + dayIdx, 3000);
            }
            workSheet.SetColumnWidth(10, 3000);

            const int COLUMN_HEADER_ROW = 2;
            const int DETAIL_START_ROW = 4;

            NPOI.HSSF.UserModel.HSSFCellStyle HeaderStyleLeft = (NPOI.HSSF.UserModel.HSSFCellStyle)workbook.CreateCellStyle();
            NPOI.HSSF.UserModel.HSSFFont HeaderFont = (NPOI.HSSF.UserModel.HSSFFont)workbook.CreateFont();
            HeaderFont.Boldweight = 900;
            HeaderFont.FontHeightInPoints = 16;
            HeaderStyleLeft.SetFont(HeaderFont);

            NPOI.HSSF.UserModel.HSSFCellStyle HeaderStyleCenter = (NPOI.HSSF.UserModel.HSSFCellStyle)workbook.CreateCellStyle();
            HeaderStyleCenter.CloneStyleFrom(HeaderStyleLeft);
            HeaderStyleCenter.Alignment = NPOI.SS.UserModel.HorizontalAlignment.CENTER;
            NPOI.HSSF.UserModel.HSSFCellStyle HeaderStyleRight = (NPOI.HSSF.UserModel.HSSFCellStyle)workbook.CreateCellStyle();
            HeaderStyleRight.CloneStyleFrom(HeaderStyleLeft);
            HeaderStyleRight.Alignment = NPOI.SS.UserModel.HorizontalAlignment.RIGHT;

            NPOI.HSSF.UserModel.HSSFCellStyle columnHeaderStyle = (NPOI.HSSF.UserModel.HSSFCellStyle)workbook.CreateCellStyle();
            columnHeaderStyle.Alignment = NPOI.SS.UserModel.HorizontalAlignment.CENTER;
            columnHeaderStyle.VerticalAlignment = NPOI.SS.UserModel.VerticalAlignment.CENTER;
            NPOI.HSSF.UserModel.HSSFFont columnHeaderFont = (NPOI.HSSF.UserModel.HSSFFont)workbook.CreateFont();
            columnHeaderFont.Boldweight = 900;
            columnHeaderStyle.SetFont(columnHeaderFont);
            columnHeaderStyle.BorderTop = NPOI.SS.UserModel.BorderStyle.THIN;
            columnHeaderStyle.BorderLeft = NPOI.SS.UserModel.BorderStyle.THIN;
            columnHeaderStyle.BorderRight = NPOI.SS.UserModel.BorderStyle.THIN;
            columnHeaderStyle.BorderBottom = NPOI.SS.UserModel.BorderStyle.THIN;

            NPOI.HSSF.UserModel.HSSFCellStyle columnHeaderStyleFirstTop = (NPOI.HSSF.UserModel.HSSFCellStyle)workbook.CreateCellStyle();
            columnHeaderStyleFirstTop.CloneStyleFrom(columnHeaderStyle);
            columnHeaderStyleFirstTop.BorderTop = NPOI.SS.UserModel.BorderStyle.MEDIUM;
            columnHeaderStyleFirstTop.BorderLeft = NPOI.SS.UserModel.BorderStyle.MEDIUM;

            NPOI.HSSF.UserModel.HSSFCellStyle columnHeaderStyleMiddleTop = (NPOI.HSSF.UserModel.HSSFCellStyle)workbook.CreateCellStyle();
            columnHeaderStyleMiddleTop.CloneStyleFrom(columnHeaderStyle);
            columnHeaderStyleMiddleTop.BorderTop = NPOI.SS.UserModel.BorderStyle.MEDIUM;

            NPOI.HSSF.UserModel.HSSFCellStyle columnHeaderStyleLastTop = (NPOI.HSSF.UserModel.HSSFCellStyle)workbook.CreateCellStyle();
            columnHeaderStyleLastTop.CloneStyleFrom(columnHeaderStyle);
            columnHeaderStyleLastTop.BorderTop = NPOI.SS.UserModel.BorderStyle.MEDIUM;
            columnHeaderStyleLastTop.BorderRight = NPOI.SS.UserModel.BorderStyle.MEDIUM;

            NPOI.HSSF.UserModel.HSSFCellStyle columnHeaderStyleFirstBottom = (NPOI.HSSF.UserModel.HSSFCellStyle)workbook.CreateCellStyle();
            columnHeaderStyleFirstBottom.CloneStyleFrom(columnHeaderStyle);
            columnHeaderStyleFirstBottom.BorderLeft = NPOI.SS.UserModel.BorderStyle.MEDIUM;
            columnHeaderStyleFirstBottom.BorderBottom = NPOI.SS.UserModel.BorderStyle.MEDIUM;

            NPOI.HSSF.UserModel.HSSFCellStyle columnHeaderStyleMiddleBottom = (NPOI.HSSF.UserModel.HSSFCellStyle)workbook.CreateCellStyle();
            columnHeaderStyleMiddleBottom.CloneStyleFrom(columnHeaderStyle);
            columnHeaderStyleMiddleBottom.BorderBottom = NPOI.SS.UserModel.BorderStyle.MEDIUM;

            NPOI.HSSF.UserModel.HSSFCellStyle columnHeaderStyleLastBottom = (NPOI.HSSF.UserModel.HSSFCellStyle)workbook.CreateCellStyle();
            columnHeaderStyleLastBottom.CloneStyleFrom(columnHeaderStyle);
            columnHeaderStyleLastBottom.BorderRight = NPOI.SS.UserModel.BorderStyle.MEDIUM;
            columnHeaderStyleLastBottom.BorderBottom = NPOI.SS.UserModel.BorderStyle.MEDIUM;

            NPOI.HSSF.UserModel.HSSFCellStyle detailStyle = (NPOI.HSSF.UserModel.HSSFCellStyle)workbook.CreateCellStyle();
            detailStyle.BorderLeft = NPOI.SS.UserModel.BorderStyle.THIN;
            detailStyle.BorderRight = NPOI.SS.UserModel.BorderStyle.THIN;
            detailStyle.BorderBottom = NPOI.SS.UserModel.BorderStyle.THIN;

            NPOI.HSSF.UserModel.HSSFCellStyle detailStyleCenterAlignment = (NPOI.HSSF.UserModel.HSSFCellStyle)workbook.CreateCellStyle();
            detailStyleCenterAlignment.CloneStyleFrom(detailStyle);
            detailStyleCenterAlignment.Alignment = NPOI.SS.UserModel.HorizontalAlignment.CENTER;

            NPOI.HSSF.UserModel.HSSFCellStyle detailStyleFirst = (NPOI.HSSF.UserModel.HSSFCellStyle)workbook.CreateCellStyle();
            detailStyleFirst.CloneStyleFrom(detailStyle);
            detailStyleFirst.BorderLeft = NPOI.SS.UserModel.BorderStyle.MEDIUM;

            NPOI.HSSF.UserModel.HSSFCellStyle detailStyleLast = (NPOI.HSSF.UserModel.HSSFCellStyle)workbook.CreateCellStyle();
            detailStyleLast.CloneStyleFrom(detailStyle);
            detailStyleLast.BorderRight = NPOI.SS.UserModel.BorderStyle.MEDIUM;

            NPOI.HSSF.UserModel.HSSFRow HeaderRow = (NPOI.HSSF.UserModel.HSSFRow)workSheet.CreateRow(0);
            NPOI.HSSF.UserModel.HSSFCell HeaderCell = (NPOI.HSSF.UserModel.HSSFCell)HeaderRow.CreateCell(5);
            HeaderCell.SetCellValue(dateRangeFr.ToString("dd-MMM-yy"));
            HeaderCell.CellStyle = HeaderStyleRight;

            HeaderCell = (NPOI.HSSF.UserModel.HSSFCell)HeaderRow.CreateCell(6);
            HeaderCell.SetCellValue("~");
            HeaderCell.CellStyle = HeaderStyleCenter;

            HeaderCell = (NPOI.HSSF.UserModel.HSSFCell)HeaderRow.CreateCell(7);
            HeaderCell.SetCellValue(dateRangeFr.AddDays(6).ToString("dd-MMM-yy"));
            HeaderCell.CellStyle = HeaderStyleLeft;

            NPOI.HSSF.UserModel.HSSFRow columnHeaderRow1 = (NPOI.HSSF.UserModel.HSSFRow)workSheet.CreateRow(COLUMN_HEADER_ROW);
            NPOI.HSSF.UserModel.HSSFRow columnHeaderRow2 = (NPOI.HSSF.UserModel.HSSFRow)workSheet.CreateRow(COLUMN_HEADER_ROW + 1);
            NPOI.HSSF.UserModel.HSSFCell columnHeaderCell;

            int colpos = 0;
            foreach (EHierarchyLevel hLevel in hLevelList)
            {
                columnHeaderCell = (NPOI.HSSF.UserModel.HSSFCell)columnHeaderRow1.CreateCell(colpos);
                columnHeaderCell.SetCellValue(hLevel.HLevelDesc);
                columnHeaderCell.CellStyle = columnHeaderStyleFirstTop;
                columnHeaderCell = (NPOI.HSSF.UserModel.HSSFCell)columnHeaderRow2.CreateCell(colpos);
                columnHeaderCell.SetCellValue(string.Empty);
                columnHeaderCell.CellStyle = columnHeaderStyleFirstBottom;

                colpos++;
            }
            columnHeaderCell = (NPOI.HSSF.UserModel.HSSFCell)columnHeaderRow1.CreateCell(colpos);
            columnHeaderCell.SetCellValue("Name");
            columnHeaderCell.CellStyle = columnHeaderStyleMiddleTop;
            columnHeaderCell = (NPOI.HSSF.UserModel.HSSFCell)columnHeaderRow2.CreateCell(colpos);
            columnHeaderCell.SetCellValue(string.Empty);
            columnHeaderCell.CellStyle = columnHeaderStyleMiddleBottom;

            colpos++;

            columnHeaderCell = (NPOI.HSSF.UserModel.HSSFCell)columnHeaderRow1.CreateCell(colpos);
            columnHeaderCell.SetCellValue("Title");
            columnHeaderCell.CellStyle = columnHeaderStyleMiddleTop;
            columnHeaderCell = (NPOI.HSSF.UserModel.HSSFCell)columnHeaderRow2.CreateCell(colpos);
            columnHeaderCell.SetCellValue(string.Empty);
            columnHeaderCell.CellStyle = columnHeaderStyleMiddleBottom;

            colpos++;

            columnHeaderCell = (NPOI.HSSF.UserModel.HSSFCell)columnHeaderRow1.CreateCell(colpos);
            columnHeaderCell.SetCellValue("Staff No.");
            columnHeaderCell.CellStyle = columnHeaderStyleMiddleTop;
            columnHeaderCell = (NPOI.HSSF.UserModel.HSSFCell)columnHeaderRow2.CreateCell(colpos);
            columnHeaderCell.SetCellValue(string.Empty);
            columnHeaderCell.CellStyle = columnHeaderStyleMiddleBottom;

            colpos++;

            for (int colIdx = 0; colIdx < colpos; colIdx++)
                workSheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(COLUMN_HEADER_ROW, COLUMN_HEADER_ROW + 1, colIdx, colIdx));


            for (int dayIdx = 0; dayIdx < 7; dayIdx++)
            {
                DateTime currentDate = dateRangeFr.AddDays(dayIdx);

                columnHeaderCell = (NPOI.HSSF.UserModel.HSSFCell)columnHeaderRow1.CreateCell(colpos + dayIdx);
                columnHeaderCell.SetCellValue(currentDate.ToString("dd-MMM-yy"));
                columnHeaderCell.CellStyle = columnHeaderStyleMiddleTop;

                columnHeaderCell = (NPOI.HSSF.UserModel.HSSFCell)columnHeaderRow2.CreateCell(colpos + dayIdx);
                columnHeaderCell.SetCellValue(currentDate.ToString("ddd"));
                columnHeaderCell.CellStyle = columnHeaderStyleMiddleBottom;
            }
            colpos += 7;

            columnHeaderCell = (NPOI.HSSF.UserModel.HSSFCell)columnHeaderRow1.CreateCell(colpos);
            columnHeaderCell.SetCellValue("Signature");
            columnHeaderCell.CellStyle = columnHeaderStyleLastTop;
            columnHeaderCell = (NPOI.HSSF.UserModel.HSSFCell)columnHeaderRow2.CreateCell(colpos);
            columnHeaderCell.SetCellValue(string.Empty);
            columnHeaderCell.CellStyle = columnHeaderStyleLastBottom;

            workSheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(COLUMN_HEADER_ROW, COLUMN_HEADER_ROW + 1, colpos, colpos));

            int RowPos = 0;
            foreach (RepeaterItem item in timeCardRecordRepeater.Items)
            {
                HtmlInputHidden hiddenEmpID = (HtmlInputHidden)item.FindControl("EmpID");
                int empID = -1;
                if (int.TryParse(hiddenEmpID.Value, out empID))
                {
                    EEmpPersonalInfo empInfo = new EEmpPersonalInfo();
                    empInfo.EmpID = empID;
                    if (!EEmpPersonalInfo.db.select(dbConn, empInfo))
                        continue;

                    string positionTitle = string.Empty;
                    List<string> hElementDescList = new List<string>();
                    EEmpPositionInfo empPos = AppUtils.GetLastPositionInfo(dbConn, dateRangeFr, empID);
                    if (empPos != null)
                    {
                        EPosition position = new EPosition();
                        position.PositionID = empPos.PositionID;
                        if (EPosition.db.select(dbConn, position))
                            positionTitle = position.PositionDesc;

                        foreach (EHierarchyLevel hLevel in hLevelList)
                        {
                            string hElementDesc = string.Empty;

                            DBFilter empHierarchy1Filter = new DBFilter();
                            empHierarchy1Filter.add(new Match("EmpPosID", empPos.EmpPosID));
                            empHierarchy1Filter.add(new Match("HLevelID", hLevel.HLevelID));
                            ArrayList empHierarchyList = EEmpHierarchy.db.select(dbConn, empHierarchy1Filter);
                            if (empHierarchyList.Count > 0)
                            {
                                EHierarchyElement hElement = new EHierarchyElement();
                                hElement.HElementID = ((EEmpHierarchy)empHierarchyList[0]).HElementID;
                                if (EHierarchyElement.db.select(dbConn, hElement))
                                    hElementDesc = hElement.HElementDesc;
                            }
                            hElementDescList.Add(hElementDesc);
                        }
                    }

                    NPOI.HSSF.UserModel.HSSFRow detailRow = (NPOI.HSSF.UserModel.HSSFRow)workSheet.CreateRow(DETAIL_START_ROW + RowPos);
                    NPOI.HSSF.UserModel.HSSFRow detailRow2 = (NPOI.HSSF.UserModel.HSSFRow)workSheet.CreateRow(DETAIL_START_ROW + RowPos + 1);   // second row of record

                    NPOI.HSSF.UserModel.HSSFCell detailCell;
                    NPOI.HSSF.UserModel.HSSFCell detailCell2;
                    colpos = 0;

                    foreach (string hElementDesc in hElementDescList)
                    {
                        workSheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(DETAIL_START_ROW + RowPos, DETAIL_START_ROW + RowPos + 1, colpos, colpos));
                        detailCell = (NPOI.HSSF.UserModel.HSSFCell)detailRow.CreateCell(colpos);
                        detailCell.SetCellValue(hElementDesc);
                        detailCell.CellStyle = detailStyleFirst;

                        detailCell2 = (NPOI.HSSF.UserModel.HSSFCell)detailRow2.CreateCell(colpos);
                        detailCell2.SetCellValue(hElementDesc);
                        detailCell2.CellStyle = detailStyleFirst;

                        colpos++;
                    }

                    workSheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(DETAIL_START_ROW + RowPos, DETAIL_START_ROW + RowPos + 1, colpos, colpos));
                    detailCell = (NPOI.HSSF.UserModel.HSSFCell)detailRow.CreateCell(colpos);
                    detailCell.SetCellValue(empInfo.EmpEngFullName);
                    detailCell.CellStyle = detailStyleFirst;

                    detailCell2 = (NPOI.HSSF.UserModel.HSSFCell)detailRow2.CreateCell(colpos);
                    detailCell2.SetCellValue(empInfo.EmpEngFullName);
                    detailCell2.CellStyle = detailStyleFirst;

                    colpos++;

                    workSheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(DETAIL_START_ROW + RowPos, DETAIL_START_ROW + RowPos + 1, colpos, colpos));
                    detailCell = (NPOI.HSSF.UserModel.HSSFCell)detailRow.CreateCell(colpos);
                    detailCell.SetCellValue(positionTitle);
                    detailCell.CellStyle = detailStyle;

                    detailCell2 = (NPOI.HSSF.UserModel.HSSFCell)detailRow2.CreateCell(colpos);
                    detailCell2.SetCellValue(positionTitle);
                    detailCell2.CellStyle = detailStyle;

                    colpos++;

                    workSheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(DETAIL_START_ROW + RowPos, DETAIL_START_ROW + RowPos + 1, colpos, colpos));
                    detailCell = (NPOI.HSSF.UserModel.HSSFCell)detailRow.CreateCell(colpos);
                    detailCell.SetCellValue(empInfo.EmpNo);
                    detailCell.CellStyle = detailStyle;

                    detailCell2 = (NPOI.HSSF.UserModel.HSSFCell)detailRow2.CreateCell(colpos);
                    detailCell2.SetCellValue(empInfo.EmpNo);
                    detailCell2.CellStyle = detailStyle;

                    colpos++;

                    for (int dayIdx = 0; dayIdx < 7; dayIdx++)
                    {
                        //first line of employee record
                        detailCell = (NPOI.HSSF.UserModel.HSSFCell)detailRow.CreateCell(colpos + dayIdx);
                        detailCell.CellStyle = detailStyleCenterAlignment;
                        // second line of employee data 
                        detailCell2 = (NPOI.HSSF.UserModel.HSSFCell)detailRow2.CreateCell(colpos + dayIdx);
                        detailCell2.CellStyle = detailStyleCenterAlignment;

                        Panel HidePanel = (Panel)item.FindControl("HidePanel" + dayIdx);

                        if (!HidePanel.Visible)
                            continue;

                        DateTime currentDate = dateRangeFr.AddDays(dayIdx);

                        DropDownList WorkStartHour = (DropDownList)item.FindControl(FIELD_ATTENDANCERECORDWORKSTART + "Hour" + dayIdx);
                        DropDownList WorkStartMinute = (DropDownList)item.FindControl(FIELD_ATTENDANCERECORDWORKSTART + "Minute" + dayIdx);

                        DropDownList WorkEndHour = (DropDownList)item.FindControl(FIELD_ATTENDANCERECORDWORKEND + "Hour" + dayIdx);
                        DropDownList WorkEndMinute = (DropDownList)item.FindControl(FIELD_ATTENDANCERECORDWORKEND + "Minute" + dayIdx);

                        DropDownList RosterCodeID = (DropDownList)item.FindControl(FIELD_ROSTERCODEID + dayIdx);

                        TimeSpan timeSpanFrom = new TimeSpan(-1);
                        TimeSpan timeSpanTo = new TimeSpan(-1);

                        Label leaveInfo = (Label)item.FindControl(FIELD_LEAVEINFO + dayIdx);

                        if (!WorkStartHour.SelectedValue.Equals(string.Empty) && !WorkStartMinute.SelectedValue.Equals(string.Empty))
                            timeSpanFrom = new TimeSpan(int.Parse(WorkStartHour.SelectedValue), int.Parse(WorkStartMinute.SelectedValue), 0);


                        if (!WorkEndHour.SelectedValue.Equals(string.Empty) && !WorkEndMinute.SelectedValue.Equals(string.Empty))
                            timeSpanTo = new TimeSpan(int.Parse(WorkEndHour.SelectedValue), int.Parse(WorkEndMinute.SelectedValue), 0);
                        if (timeSpanFrom.Ticks >= 0 && timeSpanTo.Ticks >= 0)
                        {
                            detailCell.SetCellValue(new DateTime().Add(timeSpanFrom).ToString("HH:mm") + " - " + new DateTime().Add(timeSpanTo).ToString("HH:mm"));
                        }
                        else
                            detailCell.SetCellValue(RosterCodeID.SelectedItem.Text);

                        if (!leaveInfo.Text.Equals("--"))
                        {
                            detailCell2.SetCellValue(leaveInfo.Text.Replace("<br>", "\n"));
                        }


                    }
                    colpos += 7;
                    workSheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(DETAIL_START_ROW + RowPos, DETAIL_START_ROW + RowPos + 1, colpos, colpos));
                    detailCell = (NPOI.HSSF.UserModel.HSSFCell)detailRow.CreateCell(colpos);
                    detailCell.SetCellValue(string.Empty);
                    detailCell.CellStyle = detailStyleLast;

                    detailCell2 = (NPOI.HSSF.UserModel.HSSFCell)detailRow2.CreateCell(colpos);
                    detailCell2.SetCellValue(string.Empty);
                    detailCell2.CellStyle = detailStyleLast;


                    RowPos += 2;
                }
            }

            string strTmpFolder = HROne.Common.Folder.GetOrCreateSessionTempFolder(Session.SessionID).FullName;// System.IO.Path.GetTempPath(); //Environment.GetFolderPath(Environment.SpecialFolder.InternetCache);
            string strTmpFile = System.IO.Path.Combine(strTmpFolder, AppUtils.ServerDateTime().ToString("~yyyyMMddHHmmss_AttendanceTimeCardRecord.xls"));
            System.IO.Stream fileoutputstream = new System.IO.FileStream(strTmpFile, System.IO.FileMode.OpenOrCreate);
            workbook.Write(fileoutputstream);
            fileoutputstream.Close();
            return new System.IO.FileInfo(strTmpFile);
        }
        return null;
    }

    public static void TransmitFile(HttpResponse Response, string FilenameWithFullPath, string clientSideFileName, bool DeleteAfterTransmit)
    {
        FileInfo transmiteFileInfo = new System.IO.FileInfo(FilenameWithFullPath);
        if (transmiteFileInfo.Exists)
        {
            if (Response.IsClientConnected)
            {
                Response.ClearContent();
                Response.ClearHeaders();
                Response.AddHeader("Content-Disposition", "attachment;filename=" + clientSideFileName);
                Response.ContentType = "application/download";
                Response.AppendHeader("Content-Length", transmiteFileInfo.Length.ToString());
                Response.Expires = -1;
                if (DeleteAfterTransmit)
                {
                    Response.WriteFile(FilenameWithFullPath, true);
                    Response.Flush();
                    System.IO.File.Delete(FilenameWithFullPath);
                }
                else
                {
                    Response.TransmitFile(FilenameWithFullPath);
                    Response.Flush();
                }
                Response.End();
            }
            else
                transmiteFileInfo.Delete();
        }
        else
            throw new System.IO.FileNotFoundException("Internal File Not Found: " + FilenameWithFullPath, FilenameWithFullPath);
    }

    private ArrayList GetEmployeeRosterInfo(DatabaseConnection dbConn, DateTime date, int empID)
    {
        DBFilter filter = new DBFilter();
        filter.add(new Match("EmpID", empID));
        filter.add(new Match("EmpRosterTableGroupEffFr", "<=", date));
        filter.add("EmpRosterTableGroupEffTo", false);
        return EEmpRosterTableGroup.db.select(dbConn, filter);
    }

    private EEmpRosterTableGroup GetSingleEmployeeRosterInfo(DatabaseConnection dbConn, DateTime date, int empID, int rosterTableGroupID)
    {
        DBFilter filter = new DBFilter();
        filter.add(new Match("EmpID", empID));
        filter.add(new Match("RosterTableGroupID", rosterTableGroupID));
        filter.add(new Match("EmpRosterTableGroupEffFr", "<=", date));
        filter.add("EmpRosterTableGroupEffTo", false);
        ArrayList list = EEmpRosterTableGroup.db.select(dbConn, filter);
        if (list.Count > 0)
            return (EEmpRosterTableGroup)list[0];
        else
            return null;
    }

    protected ArrayList LoadRosterTableGroup(int intYear, int intMonth, int intWeek, int userID)
    {
        if (intYear > 2000 && intYear < 2999 && intMonth >= 1 && intMonth <= 12)
        {

            DateTime asOfDate = new DateTime(intYear, intMonth, 1).AddDays((intWeek - 1) * 7);

            DBFilter subQueryFilter = new DBFilter();
            subQueryFilter.add(new Match("EmpID", userID));
            subQueryFilter.add(new Match("EmpRosterTableGroupEffFr", "<=", asOfDate));

            OR orToTerm = new OR();
            orToTerm.add(new Match("EmpRosterTableGroupEffTo", ">=", asOfDate));
            orToTerm.add(new NullTerm("EmpRosterTableGroupEffTo"));

            subQueryFilter.add(orToTerm);

            IN inFilter = new IN("RosterTableGroupID", "SELECT RosterTableGroupID FROM EmpRosterTableGroup", subQueryFilter);

            DBFilter filter = new DBFilter();

            filter.add(inFilter);
            filter.add("RosterTableGroupCode", true);

            ArrayList rosterTableGroupInfoList = ERosterTableGroup.db.select(dbConn, filter);

            //string oldText = cboRosterTableGroup.Text;
            //cboRosterTableGroup.Items.Clear();
            //cboRosterTableGroup.Items.Add("Not Selected");
            //foreach (ERosterTableGroup rosterTableGroupInfo in rosterTableGroupInfoList)
            //{
            //    string newText = rosterTableGroupInfo.RosterTableGroupCode + " - " + rosterTableGroupInfo.RosterTableGroupDesc;
            //    cboRosterTableGroup.Items.Add(newText);
            //    if (newText.Equals(oldText))
            //        cboRosterTableGroup.SelectedValue = oldText;
            //}
            //if (cboRosterTableGroup.Text.Equals(""))
            //{
            //    cboRosterTableGroup.SelectedValue = "Not Selected";
            //}

            return rosterTableGroupInfoList;
        }
        return new ArrayList(); //return empty list
    }

    private object GetFirstItem(ArrayList list)
    {
        if (list == null || list.Count <= 0)
            return null;
        else
            return list[0];
    }

}



