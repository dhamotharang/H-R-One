using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

using HROne.DataAccess;

using HROne.Lib.Entities;

public partial class ESS_MonthlyAttendanceReport : HROneWebPage
{
    public Binding binding;
    public DBManager db = EEmpPersonalInfo.db;
    public int CurID = -1;



    protected void Page_Load(object sender, EventArgs e)
    {
        if (!WebUtils.CheckAccess(Response, Session))
            return;
        binding = new Binding(dbConn, db);

        EESSUser user = WebUtils.GetCurUser(Session);
        if (user != null)
            CurID = user.EmpID;


        if (!IsPostBack)
        {

        }

        HROne.Common.WebUtility.WebControlsLocalization(this, this.Controls);

        if (!Page.IsPostBack)
        {

            if (CurID > 0)
            {
                loadObject();
            }
            else
            {

            }
        }
    }

    protected bool loadObject()
    {
        EEmpPersonalInfo obj = new EEmpPersonalInfo();
        obj.EmpID = CurID;
        // bool isNew = WebFormWorkers.loadKeys(db, obj, Request);
        if (!db.select(dbConn, obj))
            return false;

        Hashtable values = new Hashtable();
        db.populate(obj, values);
        binding.toControl(values);
        return true;
    }

    // Start 000118, Ricky So, 2015-01-15
    protected void btnGenerate_Click(object sender, EventArgs e)
    {
        PageErrors errors = PageErrors.getErrors(db, Page.Master);
        errors.clear();

        DateTime m_asOfDate = AppUtils.ServerDateTime().Date;
        if (!DateTime.TryParse(AsOfDate.Value, out m_asOfDate))
            m_asOfDate = AppUtils.ServerDateTime().Date;

        ArrayList EmpIDList = new ArrayList();

        DBFilter m_empRosterTableGroupFilter = new DBFilter();
        OR m_orEffTo = new OR();

        m_orEffTo.add(new Match("EmpRosterTableGroupEffTo", ">=", m_asOfDate));
        m_orEffTo.add(new NullTerm("EmpRosterTableGroupEffTo"));
        m_empRosterTableGroupFilter.add(new Match("EmpID", CurID));
        m_empRosterTableGroupFilter.add(new Match("EmpRosterTableGroupEffFr", "<", m_asOfDate));
        m_empRosterTableGroupFilter.add(m_orEffTo);
        m_empRosterTableGroupFilter.add("EmpRosterTableGroupID", true);

        foreach (EEmpRosterTableGroup m_empRosterTableGroup in EEmpRosterTableGroup.db.select(dbConn, m_empRosterTableGroupFilter))
        {
            if (m_empRosterTableGroup.EmpRosterTableGroupIsSupervisor == true)
            {
                DBFilter m_rosterGroupFilter = new DBFilter();
                m_rosterGroupFilter.add(new Match("RosterTableGroupID", m_empRosterTableGroup.RosterTableGroupID));
                m_rosterGroupFilter.add(new Match("EmpRosterTableGroupEffFr", "<", m_asOfDate));
                m_rosterGroupFilter.add(m_orEffTo);
                m_rosterGroupFilter.add("EmpID", true);

                foreach (EEmpRosterTableGroup m_groupMemberInfo in EEmpRosterTableGroup.db.select(dbConn, m_rosterGroupFilter))
                {
                    if (!EmpIDList.Contains(m_groupMemberInfo.EmpID))
                        EmpIDList.Add(m_groupMemberInfo.EmpID);
                }
            }
        }
    
        if (!EmpIDList.Contains(CurID))
            EmpIDList.Add(CurID);

        if (EmpIDList.Count > 0)
        {
            ArrayList leaveTypeList = new ArrayList();
            leaveTypeList.Add(ELeaveType.ANNUAL_LEAVE_TYPE(dbConn));

            MonthlyAttendanceReportProcess rpt = new MonthlyAttendanceReportProcess(dbConn, EmpIDList, m_asOfDate.Year, m_asOfDate.Month, Server.MapPath("~/ESS_MonthlyAttendanceReport.xsd"));
            string reportFileName = WebUtils.GetLocalizedReportFile(Server.MapPath("~/ESS_MonthlyAttendanceReport.rpt"));
            WebUtils.ReportExport(Response, rpt, reportFileName, "PDF", "AttendanceReport");
        }
        else
            errors.addError("No employee can be generated");
    }

    //protected void btnGenerate_Click(object sender, EventArgs e)
    //{
    //    PageErrors errors = PageErrors.getErrors(db, Page.Master);
    //    errors.clear();

    //    DateTime currentDate = AppUtils.ServerDateTime().Date;
    //    DateTime asOfDate = currentDate;
    //    if (!DateTime.TryParse(AsOfDate.Value, out asOfDate))
    //        asOfDate = currentDate;
    //    ESSAuthorizationProcess authProcess = new ESSAuthorizationProcess(dbConn);
    //    List<EAuthorizationGroup> groupList = authProcess.GetAuthorizerAuthorizationGroupList(CurID);
    //    ArrayList EmpIDList = new ArrayList();
    //    if (groupList.Count > 0)
    //    {
    //        DBFilter authWorkFlowDetailFilter = new DBFilter();
    //        string authGroupIDList = string.Empty;
    //        foreach (EAuthorizationGroup group in groupList)
    //        {
    //            if (string.IsNullOrEmpty(authGroupIDList))
    //                authGroupIDList = group.AuthorizationGroupID.ToString();
    //            else
    //                authGroupIDList += ", " + group.AuthorizationGroupID.ToString();
    //        }
    //        authWorkFlowDetailFilter.add(new IN("AuthorizationGroupID", authGroupIDList, null));

    //        DBFilter empPosFilter = EEmpPositionInfo.CreateDateRangeDBFilter("epi", currentDate, currentDate);
    //        empPosFilter.add(new IN("epi.AuthorizationWorkFlowIDLeaveApp", "SELECT DISTINCT awfd.AuthorizationWorkFlowID FROM " + EAuthorizationWorkFlowDetail.db.dbclass.tableName + " awfd", authWorkFlowDetailFilter));

    //        DBFilter empInfoFilter = new DBFilter();
    //        empInfoFilter.add(new IN("EmpID", "SELECT DISTINCT epi.EmpID FROM " + EEmpPositionInfo.db.dbclass.tableName + " epi", empPosFilter));

    //        DBFilter empTermFilter = new DBFilter();
    //        empTermFilter.add(new Match("EmpTermLastDate","<", currentDate <asOfDate?currentDate:asOfDate));
    //        empInfoFilter.add(new IN("NOT EMPID", "SELECT et.EmpID FROM " + EEmpTermination.db.dbclass.tableName + " et", empTermFilter));

    //        ArrayList EmpList = EEmpPersonalInfo.db.select(dbConn, empInfoFilter);
    //        foreach (EEmpPersonalInfo empInfo in EmpList)
    //        {
    //            // Start 000152, Ricky So, 2015-01-07
    //            if (!EmpIDList.Contains (empInfo.EmpID)) 
    //                EmpIDList.Add(empInfo.EmpID);
    //            // End 000152, Ricky So, 2015-01-07
    //        }
    //    }
    //    // Start 0000118, Ricky So, 2014/10/28
    //    if (!EmpIDList.Contains(WebUtils.GetCurUser(Session).EmpID))
    //        EmpIDList.Add(WebUtils.GetCurUser(Session).EmpID);
    //    // End 0000118, Ricky So, 2014/10/28   
    //    if (EmpIDList.Count > 0)
    //    {
    //        ArrayList leaveTypeList = new ArrayList();
    //        leaveTypeList.Add(ELeaveType.ANNUAL_LEAVE_TYPE(dbConn));

    //        MonthlyAttendanceReportProcess rpt = new MonthlyAttendanceReportProcess(dbConn, EmpIDList, asOfDate.Year, asOfDate.Month, Server.MapPath("~/ESS_MonthlyAttendanceReport.xsd"));
    //        string reportFileName = WebUtils.GetLocalizedReportFile(Server.MapPath("~/ESS_MonthlyAttendanceReport.rpt"));
    //        WebUtils.ReportExport(Response, rpt, reportFileName, "PDF", "AttendanceReport");
    //    }
    //    else
    //        errors.addError("No employee can be generated");
    //}
    // End 000118, Ricky So, 2015-01-15

    protected class MonthlyAttendanceReportProcess : HROne.Common.GenericReportProcess
    {
        ArrayList EmpIDList;
        int year;
        int month;
        string schemaXSDPath;

        public MonthlyAttendanceReportProcess(DatabaseConnection dbConn, ArrayList EmpIDList, int year, int month, string schemaXSDPath)
            : base(dbConn)
        {
            this.EmpIDList = EmpIDList;
            this.year = year;
            this.month = month;
            this.schemaXSDPath = schemaXSDPath;
        }

        public override CrystalDecisions.CrystalReports.Engine.ReportDocument GenerateReport()
        {
            if (EmpIDList.Count > 0)
            {
                DataSet ds = new DataSet();
                ds.ReadXmlSchema(schemaXSDPath);
                DataTable empInfoTable = ds.Tables["EmpInfo"];
                DataTable attendanceRecordTable = ds.Tables["AttendanceRecord"];

                DateTime periodFrom = new DateTime(year, month, 1);
                DateTime periodTo = new DateTime(year, month, DateTime.DaysInMonth(year, month));

                foreach (int EmpID in EmpIDList)
                {
                    EEmpPersonalInfo empInfo = new EEmpPersonalInfo();
                    empInfo.EmpID = EmpID;
                    if (EEmpPersonalInfo.db.select(dbConn, empInfo))
                    {
                        DataRow empRow = empInfoTable.NewRow();
                        empRow["EmpID"] = EmpID;
                        empRow["EmpNo"] = empInfo.EmpNo;
                        empRow["EmpName"] = empInfo.EmpEngFullName;
                        empRow["PeriodFrom"] = periodFrom;
                        empRow["PeriodTo"] = periodTo;

                        EEmpPositionInfo empPos = AppUtils.GetLastPositionInfo(dbConn, periodTo, EmpID);
                        if (empPos != null)
                        {
                            EAuthorizationWorkFlow workFlow = new EAuthorizationWorkFlow();
                            workFlow.AuthorizationWorkFlowID = empPos.AuthorizationWorkFlowIDLeaveApp;
                            if (EAuthorizationWorkFlow.db.select(dbConn, workFlow))
                                empRow["Section"] = workFlow.AuthorizationWorkFlowDescription;

                            EWorkHourPattern workHourPattern = new EWorkHourPattern();
                            workHourPattern.WorkHourPatternID = empPos.WorkHourPatternID;
                            if (EWorkHourPattern.db.select(dbConn, workHourPattern))
                                empRow["RosterGroup"] = workHourPattern.WorkHourPatternCode + " - " + workHourPattern.WorkHourPatternDesc;

                        }
                        empInfoTable.Rows.Add(empRow);
                    }

                    DBFilter attendanceRecordFilter = new DBFilter();
                    attendanceRecordFilter.add(new Match("EmpID", EmpID));
                    attendanceRecordFilter.add(new Match("AttendanceRecordDate", ">=", periodFrom));
                    attendanceRecordFilter.add(new Match("AttendanceRecordDate", "<=", periodTo));
                    attendanceRecordFilter.add("AttendanceRecordDate", true);
                    ArrayList attendanceRecordList = EAttendanceRecord.db.select(dbConn, attendanceRecordFilter);

                    foreach (EAttendanceRecord attendanceRecord in attendanceRecordList)
                    {
                        DataRow attendanceRecordRow = attendanceRecordTable.NewRow();


                        attendanceRecordRow["AttendanceRecordID"] = attendanceRecord.AttendanceRecordID;
                        attendanceRecordRow["EmpID"] = attendanceRecord.EmpID;
                        attendanceRecordRow["AttendanceRecordDate"] = attendanceRecord.AttendanceRecordDate;
                        if (attendanceRecord.AttendanceRecordWorkStart.Ticks > 0)
                            attendanceRecordRow["AttendanceRecordWorkStart"] = attendanceRecord.AttendanceRecordWorkStart;
                        if (attendanceRecord.AttendanceRecordWorkEnd.Ticks > 0)
                            attendanceRecordRow["AttendanceRecordWorkEnd"] = attendanceRecord.AttendanceRecordWorkEnd;
                        attendanceRecordRow["AttendanceRecordLateMins"] = attendanceRecord.AttendanceRecordActualLateMins ;
                        attendanceRecordRow["AttendanceRecordEarlyLeaveMins"] = attendanceRecord.AttendanceRecordActualEarlyLeaveMins;
                        if (attendanceRecord.AttendanceRecordIsAbsent && string.IsNullOrEmpty(attendanceRecord.AttendanceRecordRemark))
                            attendanceRecordRow["AttendanceRecordRemark"] = "Absent";
                        else
                            attendanceRecordRow["AttendanceRecordRemark"] = attendanceRecord.AttendanceRecordRemark;
                        attendanceRecordRow["IsPublicHoliday"] = EPublicHoliday.IsHoliday(dbConn, attendanceRecord.AttendanceRecordDate) || attendanceRecord.AttendanceRecordDate.DayOfWeek == DayOfWeek.Sunday;
                        // Start 0000058, KuangWei, 2014-07-10        
                        attendanceRecordRow["AttendanceRecordOvertimeMins"] = attendanceRecord.AttendanceRecordActualOvertimeMins;
                        // End 0000058, KuangWei, 2014-07-10
                        attendanceRecordTable.Rows.Add(attendanceRecordRow);

                    }
                }

                //System.Data.DataTable table = null;
                //foreach (int EmpID in EmpList)
                //{
                //    string select = "P.*,EmpPos.*,Pos.*,Comp.* ";
                //    string from = "from EmpPersonalInfo P LEFT JOIN EmpPositionInfo EmpPos ON P.EmpID=EmpPos.EmpID AND EmpPos.EmpPosEffTo IS NULL LEFT JOIN Position Pos ON EmpPos.PositionID=Pos.PositionID LEFT JOIN Company Comp ON EmpPos.CompanyID=Comp.CompanyID";
                //    DBFilter filter = new DBFilter();
                //    OR or = new OR();
                //    filter.add(new Match("P.EmpID", EmpID));
                //    System.Data.DataTable resulttable = filter.loadData(null, select, from);
                //    if (table == null)
                //        table = resulttable;
                //    else
                //        table.Merge(resulttable);
                //}
                //DBAESEncryptStringFieldAttribute.decode(table, "EmpHKID", true);
                //DBAESEncryptStringFieldAttribute.decode(table, "EmpPassportNo", false);


                //if (reportDocument == null)
                //{
                //    reportDocument = new ReportTemplate.Report_Employee_List();
                //}
                //else
                //{

                //}
                reportDocument.SetDataSource(ds);

                return reportDocument;
            }
            else
                return null;
        }
    }
}

