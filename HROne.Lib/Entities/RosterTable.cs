using System;
using System.Collections.Generic;
using System.Text;
using HROne.DataAccess;
////using perspectivemind.validation;

namespace HROne.Lib.Entities
{
    [DBClass("RosterTable")]
    public class ERosterTable : BaseObject
    {
        public static DBManager db = new DBManager(typeof(ERosterTable));

        protected int m_RosterTableID;
        [DBField("RosterTableID", true, true), TextSearch, Export(false)]
        public int RosterTableID
        {
            get { return m_RosterTableID; }
            set { m_RosterTableID = value; modify("RosterTableID"); }
        }

        protected int m_EmpID;
        [DBField("EmpID"), TextSearch, Export(false)]
        public int EmpID
        {
            get { return m_EmpID; }
            set { m_EmpID = value; modify("EmpID"); }
        }

        protected DateTime m_RosterTableDate;
        [DBField("RosterTableDate"), TextSearch, Export(false)]
        public DateTime RosterTableDate
        {
            get { return m_RosterTableDate; }
            set { m_RosterTableDate = value; modify("RosterTableDate"); }
        }
        protected int m_RosterCodeID;
        [DBField("RosterCodeID"), TextSearch, Export(false)]
        public int RosterCodeID
        {
            get { return m_RosterCodeID; }
            set { m_RosterCodeID = value; modify("RosterCodeID"); }
        }

        protected int m_LeaveAppID;
        [DBField("LeaveAppID"), TextSearch, Export(false)]
        public int LeaveAppID
        {
            get { return m_LeaveAppID; }
            set { m_LeaveAppID = value; modify("LeaveAppID"); }
        }
        protected DateTime m_RosterTableOverrideInTime;
        [DBField("RosterTableOverrideInTime", "HH:mm"), TextSearch, Export(false)]
        public DateTime RosterTableOverrideInTime
        {
            get { return m_RosterTableOverrideInTime; }
            set { m_RosterTableOverrideInTime = value; modify("RosterTableOverrideInTime"); }
        }
        protected DateTime m_RosterTableOverrideOutTime;
        [DBField("RosterTableOverrideOutTime", "HH:mm"), TextSearch, Export(false)]
        public DateTime RosterTableOverrideOutTime
        {
            get { return m_RosterTableOverrideOutTime; }
            set { m_RosterTableOverrideOutTime = value; modify("RosterTableOverrideOutTime"); }
        }

        public static List<ERosterTable> GetRosterTableList(DatabaseConnection dbConn, int EmpID, DateTime RosterDate)
        {

            SortedList<TimeSpan, ERosterTable> ResultRosterTableList = new SortedList<TimeSpan, ERosterTable>();

            DBFilter rosterTableFilter = new DBFilter();
            rosterTableFilter.add(new Match("EmpID", EmpID));
            rosterTableFilter.add(new Match("RosterTableDate", "=", RosterDate));
            System.Collections.ArrayList rosterTableList = ERosterTable.db.select(dbConn, rosterTableFilter);
            if (rosterTableList.Count > 0)
            {
                int count = 0;
                foreach (ERosterTable rosterTable in rosterTableList)
                {
                    // sort the time if roster table is multiple roster code
                    // use millsecond for first in first out basis if the in time is same
                    ERosterCode rosterCode = new ERosterCode();
                    rosterCode.RosterCodeID = rosterTable.RosterCodeID;
                    if (ERosterCode.db.select(dbConn, rosterCode))
                        ResultRosterTableList.Add(rosterCode.RosterCodeInTime.TimeOfDay.Add(new TimeSpan(0, 0, 0, 0, count++)), rosterTable);
                }
            }
            else
            {
                ERosterTable rosterTable = new ERosterTable();
                rosterTable.EmpID = EmpID;
                rosterTable.RosterTableDate = RosterDate;
                rosterTable.RosterCodeID = 0;

                EEmpPositionInfo empPosInfo = AppUtils.GetLastPositionInfo(dbConn, RosterDate, EmpID);
                if (empPosInfo != null)
                    if ((empPosInfo.EmpPosEffTo.Ticks.Equals(0) || RosterDate <= empPosInfo.EmpPosEffTo))
                    {
                        EWorkHourPattern workPattern = new EWorkHourPattern();
                        workPattern.WorkHourPatternID = empPosInfo.WorkHourPatternID;
                        if (EWorkHourPattern.db.select(dbConn, workPattern))
                        {
                            ERosterCode rosterCode = new ERosterCode();
                            rosterCode.RosterCodeID = workPattern.GetDefaultRosterCodeID(dbConn, RosterDate);
                            if (ERosterCode.db.select(dbConn, rosterCode))
                            {
                                rosterTable.RosterCodeID = rosterCode.RosterCodeID;
                            }
                        }
                    }
                ResultRosterTableList.Add(new TimeSpan(), rosterTable);
            }
            return new List<ERosterTable>(ResultRosterTableList.Values);
        }

        protected override void beforeInsert(DatabaseConnection dbConn, DBManager db)
        {
            base.beforeInsert(dbConn, db);
            //  trigger on beforeInsert for updating RosterTable.LeaveAppID before saving to database
            CreateLeaveApplication(dbConn, this);

        }

        protected override void beforeUpdate(DatabaseConnection dbConn, DBManager db)
        {
            base.beforeUpdate(dbConn, db);
            //  trigger on beforeUpdate for updating RosterTable.LeaveAppID before saving to database
            if (oldValueObject != null)
            {
                ERosterTable oldRosterTable = (ERosterTable)oldValueObject;
                if (oldRosterTable.RosterCodeID != this.RosterCodeID)
                {
                    DeleteLeaveApplication(dbConn, oldRosterTable);
                    CreateLeaveApplication(dbConn, this);
                }
            }
            else
            {
                CreateLeaveApplication(dbConn, this);
            }
        }

        protected override void afterDelete(DatabaseConnection dbConn, DBManager db)
        {
            base.afterDelete(dbConn, db);

            DeleteLeaveApplication(dbConn, this);


        }

        private static void CreateLeaveApplication(DatabaseConnection dbConn, ERosterTable rosterTable)
        {
            if (rosterTable.LeaveAppID == 0)
            {
                DBFilter attendanceRecordFilter = new DBFilter();
                attendanceRecordFilter.add(new Match("EmpID", rosterTable.EmpID));
                attendanceRecordFilter.add(new Match("AttendanceRecordDate", rosterTable.RosterTableDate));
                attendanceRecordFilter.add(new Match("RosterCodeID", rosterTable.RosterCodeID));
                attendanceRecordFilter.add(new Match("LeaveAppID", ">", 0));
                System.Collections.ArrayList existingAttendanceRecordList = EAttendanceRecord.db.select(dbConn, attendanceRecordFilter);
                //  do not create leave application if the attendance record for that day exists
                if (existingAttendanceRecordList.Count > 0)
                {
                    rosterTable.LeaveAppID = ((EAttendanceRecord)existingAttendanceRecordList[0]).LeaveAppID;
                    return;
                }
            }
            if (rosterTable.LeaveAppID == 0)
            {
                DBFilter attendanceRecordFilter = new DBFilter();
                attendanceRecordFilter.add(new Match("EmpID", rosterTable.EmpID));
                attendanceRecordFilter.add(new Match("AttendanceRecordDate", rosterTable.RosterTableDate));
                attendanceRecordFilter.add(new Match("RosterCodeID", "<>", rosterTable.RosterCodeID));
                //  do not create leave application if the attendance record for that day exists
                if (EAttendanceRecord.db.count(dbConn, attendanceRecordFilter) > 0)
                    return;
            }
            if (rosterTable.LeaveAppID > 0)
            {
                ELeaveApplication leaveApp = new ELeaveApplication();
                leaveApp.LeaveAppID = rosterTable.LeaveAppID;
                if (!ELeaveApplication.db.select(dbConn, leaveApp))
                    rosterTable.LeaveAppID = 0;
            }
            //  Check  LeaveAppID <= 0 To prevent override LeaveAppID created by external function
            if (rosterTable.RosterCodeID > 0 && rosterTable.LeaveAppID <= 0)
            {
                ERosterCode rosterCode = new ERosterCode();
                rosterCode.RosterCodeID = rosterTable.RosterCodeID;
                if (ERosterCode.db.select(dbConn, rosterCode))
                {
                    if (rosterCode.RosterCodeType.Equals(ERosterCode.ROSTERTYPE_CODE_LEAVE) && rosterCode.LeaveCodeID > 0)
                    {
                        ELeaveCode leaveCode = new ELeaveCode();
                        leaveCode.LeaveCodeID = rosterCode.LeaveCodeID;
                        if (ELeaveCode.db.select(dbConn, leaveCode))
                        {
                            ELeaveApplication leaveApp = ELeaveApplication.GetOrCreateSingleDayLeaveApplicationObject(dbConn, rosterTable.EmpID, rosterCode.LeaveCodeID, rosterTable.RosterTableDate);

                            if (leaveApp != null)
                            {
                                if (leaveApp.LeaveAppID <= 0)
                                {
                                    ELeaveApplication.db.insert(dbConn, leaveApp);
                                    rosterTable.LeaveAppID = leaveApp.LeaveAppID;
                                }
                            }
                        }
                    }
                    else if (rosterCode.RosterCodeType.Equals(ERosterCode.ROSTERTYPE_CODE_RESTDAY))
                        ELeaveBalance.DeleteLeaveBalanceAfter(dbConn, rosterTable.EmpID, ELeaveType.RESTDAY_LEAVE_TYPE(dbConn).LeaveTypeID, rosterTable.RosterTableDate);
                    else if (rosterCode.RosterCodeType.Equals(ERosterCode.ROSTERTYPE_CODE_STATUTORYHOLIDAY))
                        ELeaveBalance.DeleteLeaveBalanceAfter(dbConn, rosterTable.EmpID, ELeaveType.STATUTORYHOLIDAY_LEAVE_TYPE(dbConn).LeaveTypeID, rosterTable.RosterTableDate);
                    else if (rosterCode.RosterCodeType.Equals(ERosterCode.ROSTERTYPE_CODE_PUBLICHOLIDAY))
                        ELeaveBalance.DeleteLeaveBalanceAfter(dbConn, rosterTable.EmpID, ELeaveType.PUBLICHOLIDAY_LEAVE_TYPE(dbConn).LeaveTypeID, rosterTable.RosterTableDate);
                }
            }
        }

        private static void DeleteLeaveApplication(DatabaseConnection dbConn, ERosterTable rosterTable)
        {

            if (rosterTable.LeaveAppID > 0)
            {
                DBFilter attendanceRecordFilter = new DBFilter();
                attendanceRecordFilter.add(new Match("LeaveAppID", rosterTable.LeaveAppID));

                //  do not delete leave application if the attendance record use that leave application
                if (EAttendanceRecord.db.count(dbConn, attendanceRecordFilter) > 0)
                    return;

                ERosterCode rosterCode = new ERosterCode();
                rosterCode.RosterCodeID = rosterTable.RosterCodeID;
                if (ERosterCode.db.select(dbConn, rosterCode))
                {

                    ELeaveApplication leaveApp = new ELeaveApplication();
                    leaveApp.LeaveAppID = rosterTable.LeaveAppID;
                    if (ELeaveApplication.db.select(dbConn, leaveApp))
                        if (leaveApp.EmpPayrollID <= 0 
                            && rosterCode.LeaveCodeID.Equals(leaveApp.LeaveCodeID)  
                            && rosterTable.RosterTableDate.Equals(leaveApp.LeaveAppDateFrom)
                            && rosterTable.RosterTableDate.Equals(leaveApp.LeaveAppDateTo)
                            )
                        {
                            ELeaveCode leaveCode = new ELeaveCode();
                            leaveCode.LeaveCodeID = leaveApp.LeaveCodeID;
                            if (ELeaveCode.db.select(dbConn, leaveCode))
                                ELeaveApplication.db.delete(dbConn, leaveApp);
                        }
                }
            }
            if (rosterTable.RosterCodeID > 0 && rosterTable.LeaveAppID <= 0)
            {
                ERosterCode rosterCode = new ERosterCode();
                rosterCode.RosterCodeID = rosterTable.RosterCodeID;
                if (ERosterCode.db.select(dbConn, rosterCode))
                {
                    if (rosterCode.RosterCodeType.Equals(ERosterCode.ROSTERTYPE_CODE_RESTDAY))
                        ELeaveBalance.DeleteLeaveBalanceAfter(dbConn, rosterTable.EmpID, ELeaveType.RESTDAY_LEAVE_TYPE(dbConn).LeaveTypeID, rosterTable.RosterTableDate);
                    else if (rosterCode.RosterCodeType.Equals(ERosterCode.ROSTERTYPE_CODE_STATUTORYHOLIDAY))
                        ELeaveBalance.DeleteLeaveBalanceAfter(dbConn, rosterTable.EmpID, ELeaveType.STATUTORYHOLIDAY_LEAVE_TYPE(dbConn).LeaveTypeID, rosterTable.RosterTableDate);
                    else if (rosterCode.RosterCodeType.Equals(ERosterCode.ROSTERTYPE_CODE_PUBLICHOLIDAY))
                        ELeaveBalance.DeleteLeaveBalanceAfter(dbConn, rosterTable.EmpID, ELeaveType.PUBLICHOLIDAY_LEAVE_TYPE(dbConn).LeaveTypeID, rosterTable.RosterTableDate);
                }
            }
        }
    }
}