using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using HROne.Lib.Entities;
using HROne.DataAccess;
using HROne.CommonLib;

namespace HROne.LeaveCalc
{
    public class StatutoryHolidayBalanceProcess : LeaveBalanceProcess
    {
        public StatutoryHolidayBalanceProcess(DatabaseConnection dbConn, int EmpID)
            : base(dbConn, EmpID, 0)
        {
            EntitlePeriodUnit = "Y";
            m_LeaveTypeID = ELeaveType.STATUTORYHOLIDAY_LEAVE_TYPE(dbConn).LeaveTypeID;
            m_BalanceUnit = ELeaveBalance.LeaveBalanceUnit.Day;
        }

        protected override double CalculateProrata(LeaveProrataEntitle prorataEntitle)
        {
            ELeavePlan leavePlan = new ELeavePlan();
            leavePlan.LeavePlanID = prorataEntitle.LeavePlanEntitle.LeavePlanID;
            if (ELeavePlan.db.select(dbConn, leavePlan))
            {
                if (leavePlan.LeavePlanUseStatutoryHolidayEntitle)
                {
                    DBFilter statutoryHolidayFilter = new DBFilter();
                    statutoryHolidayFilter.add(new Match("StatutoryHolidayDate", ">=", prorataEntitle.From));
                    statutoryHolidayFilter.add(new Match("StatutoryHolidayDate", "<=", prorataEntitle.To));
                    //ArrayList statutoryHolidayList = EStatutoryHoliday.db.select(dbConn, statutoryHolidayFilter);


                    return EStatutoryHoliday.db.count(dbConn, statutoryHolidayFilter);
                }
                else
                    return 0;
            }
            else
                return 0;
        }

        protected override double LoadTaken(ELeaveBalance balanceItem, DateTime AsOfDate)
        {

            base.LoadTaken(balanceItem, AsOfDate);

            DateTime DateFrom, DateTo;
            DateFrom = balanceItem.LeaveBalanceEffectiveDate;
            DateTo = balanceItem.LeaveBalanceEffectiveEndDate;

            DBFilter rosterCodeFilter = new DBFilter();
            rosterCodeFilter.add(new Match("RosterCodeType", ERosterCode.ROSTERTYPE_CODE_STATUTORYHOLIDAY));

            //  Use AttendanceRecord as primary source for Rest Day taken
            {
                DBFilter attendanceRecordFilter = new DBFilter();
                attendanceRecordFilter.add(new Match("AttendanceRecordDate", ">=", DateFrom));
                attendanceRecordFilter.add(new Match("EmpID", balanceItem.EmpID));
                attendanceRecordFilter.add(new IN("RosterCodeID", "Select RosterCodeID from " + ERosterCode.db.dbclass.tableName, rosterCodeFilter));
                ArrayList AttendanceRecordList = EAttendanceRecord.db.select(dbConn, attendanceRecordFilter);

                foreach (EAttendanceRecord attendanceRecord in AttendanceRecordList)
                {
                    //  Check if duplicate rest day leave application exists 
                    //  
                    double actualDayTaken = 1;
                    DBFilter leaveApplicationFilter = new DBFilter();
                    DBFilter leaveCodeFilter = new DBFilter();
                    leaveCodeFilter.add(new Match("LeaveTypeID", LeaveTypeID));
                    leaveApplicationFilter.add(new Match("EmpID", balanceItem.EmpID));
                    leaveApplicationFilter.add(new Match("LeaveAppDateFrom", "<=", attendanceRecord.AttendanceRecordDate));
                    leaveApplicationFilter.add(new Match("LeaveAppDateTo", ">=", attendanceRecord.AttendanceRecordDate));
                    leaveApplicationFilter.add(new IN("LeaveCodeID", "SELECT LeaveCodeID FROM " + ELeaveCode.db.dbclass.tableName, leaveCodeFilter));
                    ArrayList leaveAppList = ELeaveApplication.db.select(dbConn, leaveApplicationFilter);
                    foreach (ELeaveApplication leaveApp in leaveAppList)
                        actualDayTaken -= leaveApp.LeaveAppDays;

                    if (actualDayTaken > 0)
                        if (attendanceRecord.AttendanceRecordDate <= AsOfDate && attendanceRecord.AttendanceRecordDate <= DateTo)
                            balanceItem.Taken += actualDayTaken;
                        else
                            balanceItem.Reserved += actualDayTaken;
                }
            }
            //  Use rostertable for Rest Day taken only if attendence record does not exist
            {
                DBFilter rosterTableFilter = new DBFilter();
                rosterTableFilter.add(new Match("RosterTableDate", ">=", DateFrom));
                rosterTableFilter.add(new Match("EmpID", balanceItem.EmpID));
                rosterTableFilter.add(new IN("RosterCodeID", "Select RosterCodeID from " + ERosterCode.db.dbclass.tableName, rosterCodeFilter));
                DBFilter notExistsFilter = new DBFilter();
                notExistsFilter.add(new MatchField("RosterTableDate", "AttendanceRecordDate"));
                notExistsFilter.add(new Match("EmpID", balanceItem.EmpID));
                rosterTableFilter.add(new Exists(EAttendanceRecord.db.dbclass.tableName + " ar", notExistsFilter, true));
                ArrayList rosterTableList = ERosterTable.db.select(dbConn, rosterTableFilter);

                foreach (ERosterTable rosterTable in rosterTableList)
                {
                    //  Check if duplicate rest day leave application exists 
                    //  
                    double actualDayTaken = 1;
                    DBFilter leaveApplicationFilter = new DBFilter();
                    DBFilter leaveCodeFilter = new DBFilter();
                    leaveCodeFilter.add(new Match("LeaveTypeID", LeaveTypeID));
                    leaveApplicationFilter.add(new Match("EmpID", balanceItem.EmpID));
                    leaveApplicationFilter.add(new Match("LeaveAppDateFrom", "<=", rosterTable.RosterTableDate));
                    leaveApplicationFilter.add(new Match("LeaveAppDateTo", ">=", rosterTable.RosterTableDate));
                    leaveApplicationFilter.add(new IN("LeaveCodeID", "SELECT LeaveCodeID FROM " + ELeaveCode.db.dbclass.tableName, leaveCodeFilter));
                    ArrayList leaveAppList = ELeaveApplication.db.select(dbConn, leaveApplicationFilter);
                    foreach (ELeaveApplication leaveApp in leaveAppList)
                        actualDayTaken -= leaveApp.LeaveAppDays;

                    if (actualDayTaken > 0)
                        if (rosterTable.RosterTableDate <= AsOfDate && rosterTable.RosterTableDate <= DateTo)
                            balanceItem.Taken += actualDayTaken;
                        else
                            balanceItem.Reserved += actualDayTaken;
                }
            }

            //DBFilter filter = new DBFilter();
            //filter.add(new Match("RosterTableDate", ">=", DateFrom));
            //filter.add(new Match("EmpID", balanceItem.EmpID));
            //DBFilter rosterCodeFilter = new DBFilter();
            //rosterCodeFilter.add(new Match("RosterCodeType", ERosterCode.ROSTERTYPE_CODE_STATUTORYHOLIDAY));
            //filter.add(new IN("RosterCodeID", "Select RosterCodeID from " + ERosterCode.db.dbclass.tableName, rosterCodeFilter));
            //ArrayList rosterTableList = ERosterTable.db.select(dbConn, filter);

            //foreach (ERosterTable rosterTable in rosterTableList)
            //{
            //    //  Check if duplicate rest day leave application exists 
            //    //  
            //    double actualDayTaken = 1;
            //    DBFilter leaveApplicationFilter = new DBFilter();
            //    DBFilter leaveCodeFilter = new DBFilter();
            //    leaveCodeFilter.add(new Match("LeaveTypeID", LeaveTypeID));
            //    leaveApplicationFilter.add(new Match("EmpID", balanceItem.EmpID));
            //    leaveApplicationFilter.add(new Match("LeaveAppDateFrom", "<=", rosterTable.RosterTableDate));
            //    leaveApplicationFilter.add(new Match("LeaveAppDateTo", ">=", rosterTable.RosterTableDate));
            //    leaveApplicationFilter.add(new IN("LeaveCodeID", "SELECT LeaveCodeID FROM " + ELeaveCode.db.dbclass.tableName, leaveCodeFilter));
            //    ArrayList leaveAppList = ELeaveApplication.db.select(dbConn, leaveApplicationFilter);
            //    foreach (ELeaveApplication leaveApp in leaveAppList)
            //        actualDayTaken -= leaveApp.LeaveAppDays;

            //    if (actualDayTaken > 0)
            //        if (rosterTable.RosterTableDate <= AsOfDate && rosterTable.RosterTableDate <= DateTo)
            //            balanceItem.Taken += actualDayTaken;
            //        else
            //            balanceItem.Reserved += actualDayTaken;
            //}

            return balanceItem.Taken;
        }

        protected override double MaximumBroughtForward(DateTime AsOfDate)
        {
            return 9999;
        }
        protected override void GetLeaveYearStartDate(DateTime AsofDate, out int cutoffMonth, out int cutoffDay)
        {
            cutoffMonth = 1;
            cutoffDay = 1;
            return;
        }

    }
}
