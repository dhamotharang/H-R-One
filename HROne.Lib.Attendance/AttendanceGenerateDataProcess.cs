using System;
using System.Collections;
using System.Text;
using HROne.DataAccess;
using HROne.Lib.Entities;
using HROne.Lib;

namespace HROne.Attendance
{
    public class AttendanceProcess
    {
        protected DatabaseConnection dbConn;
        public AttendanceProcess(DatabaseConnection dbConn)
        {
            this.dbConn = dbConn;
        }
        public void GenerateAttendanceData(int EmpID, DateTime DateFrom, DateTime DateTo, bool hasTimeCardRecord)
        {
            //  Clear old attenance record
            DBFilter attendanceRecordFilter = new DBFilter();
            attendanceRecordFilter.add(new Match("EmpID", EmpID));
            attendanceRecordFilter.add(new Match("AttendanceRecordDate", "<=", DateTo));
            attendanceRecordFilter.add(new Match("AttendanceRecordDate", ">=", DateFrom));
            EAttendanceRecord.db.delete(dbConn, attendanceRecordFilter);

            EEmpPersonalInfo empInfo = new EEmpPersonalInfo();
            empInfo.EmpID = EmpID;

            if (EEmpPersonalInfo.db.select(dbConn, empInfo))
            {
                if (DateFrom < empInfo.EmpDateOfJoin)
                    DateFrom = empInfo.EmpDateOfJoin;
                DBFilter empTermFilter = new DBFilter();
                empTermFilter.add(new Match("EmpID", empInfo.EmpID));
                ArrayList empTermList = EEmpTermination.db.select(dbConn, empTermFilter);
                if (empTermList.Count > 0)
                {
                    EEmpTermination empTerm = (EEmpTermination)empTermList[0];
                    if (DateTo > empTerm.EmpTermLastDate)
                        DateTo = empTerm.EmpTermLastDate;
                }

                for (DateTime rosterDate = DateFrom; rosterDate <= DateTo; rosterDate = rosterDate.AddDays(1))
                {
                    System.Collections.Generic.List<EAttendanceRecord> attendanceRecordList = CreateAttendanceRecordObject(EmpID, rosterDate, hasTimeCardRecord, 0);
                    foreach (EAttendanceRecord attendanceRecord in attendanceRecordList)
                        EAttendanceRecord.db.insert(dbConn, attendanceRecord);

                }
            }
        }
        public System.Collections.Generic.List<EAttendanceRecord> CreateAttendanceRecordObject(int EmpID, DateTime rosterDate, bool hasTimeCardRecord, int DefaultRosterCodeID)
        {
            System.Collections.Generic.List<EAttendanceRecord> attendanceRecordList = new System.Collections.Generic.List<EAttendanceRecord>();
            //DBFilter rosterTableFilter = new DBFilter();
            //rosterTableFilter.add(new Match("EmpID", EmpID));
            //rosterTableFilter.add(new Match("RosterTableDate", "=", rosterDate));
            //ArrayList rosterTableList = ERosterTable.db.select(dbConn, rosterTableFilter);
            //if (rosterTableList.Count > 0)
            //    foreach (ERosterTable rosterTable in rosterTableList)
            //        attendanceRecordList.Add(GetAttendanceTimeRecord(rosterTable, hasTimeCardRecord, DefaultRosterCodeID));
            //else
            //{
            //    ERosterTable rosterTable = new ERosterTable();
            //    rosterTable.EmpID = EmpID;
            //    rosterTable.RosterTableDate = rosterDate;
            //    rosterTable.RosterCodeID = 0;

            //    EEmpPositionInfo empPosInfo = AppUtils.GetLastPositionInfo(dbConn, rosterDate, EmpID);
            //    if (empPosInfo != null)
            //        if ((empPosInfo.EmpPosEffTo.Ticks.Equals(0) || rosterDate <= empPosInfo.EmpPosEffTo))
            //        {
            //            EWorkHourPattern workPattern = new EWorkHourPattern();
            //            workPattern.WorkHourPatternID = empPosInfo.WorkHourPatternID;
            //            if (EWorkHourPattern.db.select(dbConn, workPattern))
            //            {
            //                ERosterCode rosterCode = new ERosterCode();
            //                rosterCode.RosterCodeID = workPattern.GetDefaultRosterCodeID(dbConn, rosterDate);
            //                if (ERosterCode.db.select(dbConn, rosterCode))
            //                {
            //                    rosterTable.RosterCodeID = rosterCode.RosterCodeID;
            //                }
            //            }
            //        }
            //    attendanceRecordList.Add(GetAttendanceTimeRecord(rosterTable, hasTimeCardRecord, DefaultRosterCodeID));
            //}
            System.Collections.Generic.List<ERosterTable> rosterTableList = ERosterTable.GetRosterTableList(dbConn, EmpID, rosterDate);
            for (int idx=0; idx < rosterTableList.Count;idx++)
            {
                ERosterTable previousRosterTable = null;
                ERosterTable nextRosterTable = null;
                if (idx == 0)
                {
                    System.Collections.Generic.List<ERosterTable> previousRosterTableList = ERosterTable.GetRosterTableList(dbConn, EmpID, rosterDate.AddDays(-1));
                    if (previousRosterTableList.Count > 0)
                        previousRosterTable = previousRosterTableList[previousRosterTableList.Count - 1];
                }
                else
                    previousRosterTable = rosterTableList[idx - 1];

                if (idx == rosterTableList.Count - 1)
                {
                    System.Collections.Generic.List<ERosterTable> nextRosterTableList = ERosterTable.GetRosterTableList(dbConn, EmpID, rosterDate.AddDays(1));
                    if (nextRosterTableList.Count > 0)
                        nextRosterTable = nextRosterTableList[0];
                }
                else
                    nextRosterTable = rosterTableList[idx + 1];


                attendanceRecordList.Add(GetAttendanceTimeRecord(rosterTableList[idx], previousRosterTable, nextRosterTable, hasTimeCardRecord, DefaultRosterCodeID));
            }
            if (attendanceRecordList.Count <= 0)
            {
                EAttendanceRecord attendanceRecord = new EAttendanceRecord();
                attendanceRecord.AttendanceRecordDate = rosterDate;
                attendanceRecord.EmpID = EmpID;
                attendanceRecordList.Add(attendanceRecord);
            }
            return attendanceRecordList;
        }
        private EAttendanceRecord GetAttendanceTimeRecord(ERosterTable currentRosterTable, ERosterTable previousRosterTable, ERosterTable nextRosterTable, bool hasTimeCardRecord, int DefaultRosterCodeID)
        {
            int EmpID = currentRosterTable.EmpID;

            if (currentRosterTable.RosterCodeID <= 0)
                currentRosterTable.RosterCodeID = DefaultRosterCodeID;

            EAttendanceRecord attendanceRecord = new EAttendanceRecord();
            attendanceRecord.AttendanceRecordDate = currentRosterTable.RosterTableDate;
            attendanceRecord.RosterCodeID = currentRosterTable.RosterCodeID;
            attendanceRecord.RosterTableID = currentRosterTable.RosterTableID;
            attendanceRecord.EmpID = EmpID;
            attendanceRecord.LeaveAppID = currentRosterTable.LeaveAppID;

            if (!currentRosterTable.RosterTableOverrideInTime.Ticks.Equals(0))
                attendanceRecord.AttendanceRecordRosterCodeInTimeOverride = currentRosterTable.RosterTableOverrideInTime;
            if (!currentRosterTable.RosterTableOverrideOutTime.Ticks.Equals(0))
                attendanceRecord.AttendanceRecordRosterCodeOutTimeOverride = currentRosterTable.RosterTableOverrideOutTime;

            DateTime startDateTime = currentRosterTable.RosterTableDate;
            DateTime endDateTime = currentRosterTable.RosterTableDate.AddDays(1);


            ERosterCode currentRosterCode = new ERosterCode();
            currentRosterCode.RosterCodeID = attendanceRecord.RosterCodeID;
            if (!ERosterCode.db.select(dbConn, currentRosterCode))
            {
                currentRosterCode.RosterCodeType = ERosterCode.ROSTERTYPE_CODE_RESTDAY;
            }


            if (hasTimeCardRecord)
            {
                currentRosterCode = GetActualRosterDateTime(currentRosterTable.RosterTableDate, currentRosterCode, attendanceRecord.AttendanceRecordRosterCodeInTimeOverride, attendanceRecord.AttendanceRecordRosterCodeOutTimeOverride, attendanceRecord.AttendanceRecordRosterCodeLunchStartTimeOverride, attendanceRecord.AttendanceRecordRosterCodeLunchEndTimeOverride);
                startDateTime = currentRosterCode.RosterCodeDayStartTime;
                endDateTime = currentRosterCode.RosterCodeCutOffTime;

                //  change the startDateTime and endDateTime for timecard criteria if the time is overlap
                if (previousRosterTable != null)
                {
                    ERosterCode perviousRosterCode = new ERosterCode();
                    perviousRosterCode.RosterCodeID = previousRosterTable.RosterCodeID;
                    if (ERosterCode.db.select(dbConn, perviousRosterCode))
                    {
                        perviousRosterCode = GetActualRosterDateTime(previousRosterTable.RosterTableDate, perviousRosterCode, new DateTime(), new DateTime(), new DateTime(), new DateTime());
                        if (startDateTime < perviousRosterCode.RosterCodeCutOffTime)
                            if (currentRosterCode.RosterCodeType.Equals(ERosterCode.ROSTERTYPE_CODE_NORMAL)
                            || currentRosterCode.RosterCodeType.Equals(ERosterCode.ROSTERTYPE_CODE_OVERNIGHT))
                                startDateTime = new DateTime((currentRosterCode.RosterCodeDayStartTime.Ticks + currentRosterCode.RosterCodeInTime.Ticks) / 2);
                            else
                                startDateTime = perviousRosterCode.RosterCodeCutOffTime;
                    }
                }
                if (nextRosterTable != null)
                {
                    ERosterCode nextRosterCode = new ERosterCode();
                    nextRosterCode.RosterCodeID = nextRosterTable.RosterCodeID;
                    if (ERosterCode.db.select(dbConn, nextRosterCode))
                    {
                        nextRosterCode = GetActualRosterDateTime(nextRosterTable.RosterTableDate, nextRosterCode, new DateTime(), new DateTime(), new DateTime(), new DateTime());
                        if (nextRosterCode.RosterCodeDayStartTime < endDateTime)
                            if (nextRosterCode.RosterCodeType.Equals(ERosterCode.ROSTERTYPE_CODE_NORMAL)
                            || nextRosterCode.RosterCodeType.Equals(ERosterCode.ROSTERTYPE_CODE_OVERNIGHT))
                                endDateTime = new DateTime((nextRosterCode.RosterCodeDayStartTime.Ticks + nextRosterCode.RosterCodeInTime.Ticks) / 2);
                            else
                            {
                                //  endDateTime no change if next day is NOT working day
                            }
                    }
                }



                string timeCardNo = GetTimeCardNo(EmpID);

                DBFilter timeCardRecordFilter = new DBFilter();
                OR orTimeCardRecordHolder = new OR();
                orTimeCardRecordHolder.add(new Match("EmpID", EmpID));
                if (!string.IsNullOrEmpty(timeCardNo))
                    orTimeCardRecordHolder.add(new Match("TimeCardRecordCardNo", timeCardNo));
                timeCardRecordFilter.add(orTimeCardRecordHolder);
                timeCardRecordFilter.add(new Match("TimeCardRecordDateTime", ">=", startDateTime));
                timeCardRecordFilter.add(new Match("TimeCardRecordDateTime", "<=", endDateTime));
                timeCardRecordFilter.add("TimeCardRecordDateTime", true);
                ArrayList timeCardRecordList = ETimeCardRecord.db.select(dbConn, timeCardRecordFilter);


                DateTime lastEntryTime = new DateTime();
                foreach (ETimeCardRecord timeCardRecord in timeCardRecordList)
                {
                    DateTime entryTime = timeCardRecord.TimeCardRecordDateTime;
                    if (((TimeSpan)lastEntryTime.Subtract(entryTime)).Duration().TotalSeconds <= 30)
                    {
                        lastEntryTime = entryTime;
                        continue;
                    }
                    lastEntryTime = entryTime;
                    //  Remove second and millsecond on attendance
                    entryTime = new DateTime(entryTime.Year, entryTime.Month, entryTime.Day, entryTime.Hour, entryTime.Minute, 0);

                    //  Check InOut Index for future enchancement use
                    if (attendanceRecord.AttendanceRecordWorkStart.Ticks.Equals(0))
                    {
                        attendanceRecord.AttendanceRecordWorkStart = entryTime;
                        attendanceRecord.AttendanceRecordWorkStartLocation = timeCardRecord.TimeCardRecordLocation;
                        attendanceRecord.AttendanceRecordWorkStartTimeCardRecordID = timeCardRecord.TimeCardRecordID;
                    }
                    else
                    {
                        if (attendanceRecord.AttendanceRecordWorkEnd.Ticks.Equals(0))
                        {
                            attendanceRecord.AttendanceRecordWorkEnd = entryTime;
                            attendanceRecord.AttendanceRecordWorkEndLocation = timeCardRecord.TimeCardRecordLocation;
                            attendanceRecord.AttendanceRecordWorkEndTimeCardRecordID = timeCardRecord.TimeCardRecordID;
                        }
                        else
                        {
                            DateTime tmpLastEndTime = attendanceRecord.AttendanceRecordWorkEnd;
                            string tmpLastEndLocation = attendanceRecord.AttendanceRecordWorkEndLocation;
                            int tmpLastEndTimeCardID = attendanceRecord.AttendanceRecordWorkEndTimeCardRecordID;
                            attendanceRecord.AttendanceRecordWorkEnd = entryTime;
                            attendanceRecord.AttendanceRecordWorkEndLocation = timeCardRecord.TimeCardRecordLocation;
                            attendanceRecord.AttendanceRecordWorkEndTimeCardRecordID = timeCardRecord.TimeCardRecordID;

                            if (tmpLastEndTime < currentRosterCode.RosterCodeDayStartTime)
                            {
                                attendanceRecord.AttendanceRecordWorkStart = tmpLastEndTime;
                                attendanceRecord.AttendanceRecordWorkStartLocation = tmpLastEndLocation;
                                attendanceRecord.AttendanceRecordWorkStartTimeCardRecordID = tmpLastEndTimeCardID;
                            }
                            else if (currentRosterCode.RosterCodeHasLunch)

                                if (attendanceRecord.AttendanceRecordLunchIn.Ticks.Equals(0))
                                {
                                    attendanceRecord.AttendanceRecordLunchIn = tmpLastEndTime;
                                    attendanceRecord.AttendanceRecordLunchInLocation = tmpLastEndLocation;
                                    attendanceRecord.AttendanceRecordLunchInTimeCardRecordID = tmpLastEndTimeCardID;
                                }
                                else if (attendanceRecord.AttendanceRecordLunchOut.Ticks.Equals(0))
                                {
                                    attendanceRecord.AttendanceRecordLunchOut = attendanceRecord.AttendanceRecordLunchIn;
                                    attendanceRecord.AttendanceRecordLunchOutLocation = attendanceRecord.AttendanceRecordLunchInLocation;
                                    attendanceRecord.AttendanceRecordLunchOutTimeCardRecordID = attendanceRecord.AttendanceRecordLunchInTimeCardRecordID;
                                    attendanceRecord.AttendanceRecordLunchIn = tmpLastEndTime;
                                    attendanceRecord.AttendanceRecordLunchInLocation = tmpLastEndLocation;
                                    attendanceRecord.AttendanceRecordLunchInTimeCardRecordID = tmpLastEndTimeCardID;
                                }
                                else
                                    if (currentRosterCode.RosterCodeLunchEndTime >= tmpLastEndTime)
                                    {
                                        //  if previous time is within lunch time, put the time as lunch hour
                                        DateTime tmpLunchOutTime = attendanceRecord.AttendanceRecordLunchIn;
                                        string tmpLunchOutLocation = attendanceRecord.AttendanceRecordLunchInLocation;
                                        int tmpLunchOutTimeCardID = attendanceRecord.AttendanceRecordLunchInTimeCardRecordID;
                                        attendanceRecord.AttendanceRecordLunchIn = tmpLastEndTime;
                                        attendanceRecord.AttendanceRecordLunchInLocation = tmpLastEndLocation;
                                        attendanceRecord.AttendanceRecordLunchInTimeCardRecordID = tmpLastEndTimeCardID;
                                        if (currentRosterCode.RosterCodeLunchStartTime > attendanceRecord.AttendanceRecordLunchOut)
                                        {
                                            attendanceRecord.AttendanceRecordLunchOut = tmpLunchOutTime;
                                            attendanceRecord.AttendanceRecordLunchOutLocation = tmpLunchOutLocation;
                                            attendanceRecord.AttendanceRecordLunchOutTimeCardRecordID = tmpLunchOutTimeCardID;
                                        }
                                    }

                        }
                    }
                }
                attendanceRecord = GetAttendanceTimeResult(attendanceRecord);
            }
            else
            {
                if (currentRosterCode.RosterCodeType.Equals(ERosterCode.ROSTERTYPE_CODE_NORMAL) || currentRosterCode.RosterCodeType.Equals(ERosterCode.ROSTERTYPE_CODE_OVERNIGHT))
                {
                    //  Create Dummy In/Out time
                    attendanceRecord.AttendanceRecordWorkStart = currentRosterCode.RosterCodeInTime;
                    if (currentRosterCode.RosterCodeCountWorkHourOnly)
                        attendanceRecord.AttendanceRecordWorkEnd = currentRosterCode.RosterCodeInTime.AddHours(currentRosterCode.RosterCodeDailyWorkingHour);
                    else
                        attendanceRecord.AttendanceRecordWorkEnd = currentRosterCode.RosterCodeOutTime;
                    attendanceRecord = GetAttendanceTimeResult(attendanceRecord);

                    attendanceRecord.AttendanceRecordWorkStart = new DateTime();
                    attendanceRecord.AttendanceRecordWorkEnd = new DateTime();
                    //attendanceRecord.AttendanceRecordCalculateWorkingHour = rosterCode.RosterCodeDailyWorkingHour;
                    //attendanceRecord.AttendanceRecordCalculateWorkingDay = rosterCode.RosterCodeWorkingDayUnit;
                    //attendanceRecord.AttendanceRecordActualWorkingHour = attendanceRecord.AttendanceRecordCalculateWorkingHour;
                    //attendanceRecord.AttendanceRecordActualWorkingDay = attendanceRecord.AttendanceRecordCalculateWorkingDay;
                }
            }
            return attendanceRecord;
        }

        public EAttendanceRecord GetAttendanceTimeResult(EAttendanceRecord attendanceRecord)
        {

            //  initialize record result
            attendanceRecord.AttendanceRecordCalculateLateMins = 0;
            attendanceRecord.AttendanceRecordCalculateEarlyLeaveMins = 0;
            attendanceRecord.AttendanceRecordCalculateLunchEarlyLeaveMins = 0;
            attendanceRecord.AttendanceRecordCalculateLunchLateMins = 0;
            attendanceRecord.AttendanceRecordCalculateOvertimeMins = 0;
            attendanceRecord.AttendanceRecordCalculateLunchTimeMins = 0;
            attendanceRecord.AttendanceRecordIsAbsent = false;
            attendanceRecord.AttendanceRecordCalculateWorkingHour = 0;
            attendanceRecord.AttendanceRecordCalculateWorkingDay = 0;

            DateTime dateTimeStart = attendanceRecord.AttendanceRecordDate;
            ERosterCode rosterCode = new ERosterCode();
            rosterCode.RosterCodeID = attendanceRecord.RosterCodeID;
            if (ERosterCode.db.select(dbConn, rosterCode))
            {
                dateTimeStart = attendanceRecord.AttendanceRecordDate.Add(rosterCode.RosterCodeInTime.TimeOfDay);// attendanceRecord.AttendanceRecordDate.AddHours(rosterCode.RosterCodeInTime.Hour).AddMinutes(rosterCode.RosterCodeInTime.Minute);
            }

            //  resolve overnight issue
            if (!attendanceRecord.AttendanceRecordWorkStart.Ticks.Equals(0))
            {
                attendanceRecord.AttendanceRecordWorkStart = attendanceRecord.AttendanceRecordDate.Add(attendanceRecord.AttendanceRecordWorkStart.TimeOfDay);// attendanceRecord.AttendanceRecordDate.AddHours(attendanceRecord.AttendanceRecordWorkStart.Hour).AddMinutes(attendanceRecord.AttendanceRecordWorkStart.Minute);
                //  Shift the day only when work In/out time is set under roster code
                if (!rosterCode.RosterCodeCountWorkHourOnly)
                    if (Math.Abs(((TimeSpan)dateTimeStart.Subtract(attendanceRecord.AttendanceRecordWorkStart)).TotalHours) > 12)
                        if (attendanceRecord.AttendanceRecordWorkStart < dateTimeStart)
                            attendanceRecord.AttendanceRecordWorkStart = attendanceRecord.AttendanceRecordWorkStart.AddDays(1);
                        else
                            attendanceRecord.AttendanceRecordWorkStart = attendanceRecord.AttendanceRecordWorkStart.AddDays(-1);

            }

            if (!attendanceRecord.AttendanceRecordWorkStart.Ticks.Equals(0) && !attendanceRecord.AttendanceRecordLunchOut.Ticks.Equals(0))
            {
                attendanceRecord.AttendanceRecordLunchOut = attendanceRecord.AttendanceRecordWorkStart.Date.Add(attendanceRecord.AttendanceRecordLunchOut.TimeOfDay); //attendanceRecord.AttendanceRecordWorkStart.Date.AddHours(attendanceRecord.AttendanceRecordLunchOut.Hour).AddMinutes(attendanceRecord.AttendanceRecordLunchOut.Minute);
                while (attendanceRecord.AttendanceRecordWorkStart > attendanceRecord.AttendanceRecordLunchOut)
                    attendanceRecord.AttendanceRecordLunchOut = attendanceRecord.AttendanceRecordLunchOut.AddDays(1);
            }
            if (!attendanceRecord.AttendanceRecordWorkStart.Ticks.Equals(0) && !attendanceRecord.AttendanceRecordLunchIn.Ticks.Equals(0))
            {
                attendanceRecord.AttendanceRecordLunchIn = attendanceRecord.AttendanceRecordWorkStart.Date.Add(attendanceRecord.AttendanceRecordLunchIn.TimeOfDay); //attendanceRecord.AttendanceRecordWorkStart.Date.AddHours(attendanceRecord.AttendanceRecordLunchIn.Hour).AddMinutes(attendanceRecord.AttendanceRecordLunchIn.Minute);
                while (attendanceRecord.AttendanceRecordWorkStart > attendanceRecord.AttendanceRecordLunchIn)
                    attendanceRecord.AttendanceRecordLunchIn = attendanceRecord.AttendanceRecordLunchIn.AddDays(1);
            }

            if (!attendanceRecord.AttendanceRecordWorkStart.Ticks.Equals(0) && !attendanceRecord.AttendanceRecordWorkEnd.Ticks.Equals(0))
            {
                attendanceRecord.AttendanceRecordWorkEnd = attendanceRecord.AttendanceRecordWorkStart.Date.Add(attendanceRecord.AttendanceRecordWorkEnd.TimeOfDay); //attendanceRecord.AttendanceRecordWorkStart.Date.AddHours(attendanceRecord.AttendanceRecordWorkEnd.Hour).AddMinutes(attendanceRecord.AttendanceRecordWorkEnd.Minute);
                while (attendanceRecord.AttendanceRecordWorkStart > attendanceRecord.AttendanceRecordWorkEnd)
                    attendanceRecord.AttendanceRecordWorkEnd = attendanceRecord.AttendanceRecordWorkEnd.AddDays(1);
            }

            string remark = attendanceRecord.AttendanceRecordRemark;
            if (remark == null)
                remark = string.Empty;
            DateTime startDateTime = attendanceRecord.AttendanceRecordDate;
            DateTime endDateTime = startDateTime.AddDays(1);

            //ERosterCode rosterCode = new ERosterCode();
            //rosterCode.RosterCodeID = attendanceRecord.RosterCodeID;
            //ERosterCode.db.select(dbConn, rosterCode);
            if (!ERosterCode.db.select(dbConn, rosterCode))
            {
                rosterCode.RosterCodeType = ERosterCode.ROSTERTYPE_CODE_RESTDAY;
            }
            {
                rosterCode = GetActualRosterDateTime(attendanceRecord.AttendanceRecordDate, rosterCode, attendanceRecord.AttendanceRecordRosterCodeInTimeOverride, attendanceRecord.AttendanceRecordRosterCodeOutTimeOverride, attendanceRecord.AttendanceRecordRosterCodeLunchStartTimeOverride, attendanceRecord.AttendanceRecordRosterCodeLunchEndTimeOverride);
                startDateTime = rosterCode.RosterCodeDayStartTime;
                endDateTime = rosterCode.RosterCodeCutOffTime;
                attendanceRecord.AttendanceRecordCalculateWorkingHour = rosterCode.RosterCodeDailyWorkingHour;
                attendanceRecord.AttendanceRecordCalculateWorkingDay = rosterCode.RosterCodeWorkingDayUnit;

            }

            DateTime attendanceInTime = attendanceRecord.AttendanceRecordWorkStart;
            DateTime attendanceOutTime = attendanceRecord.AttendanceRecordWorkEnd;
            DateTime rosterInTime = rosterCode.RosterCodeInTime;
            DateTime rosterOutTime = rosterCode.RosterCodeOutTime;

            //  Support for OT within working hour period.
            if (!rosterCode.RosterCodeCountWorkHourOnly && rosterCode.RosterCodeHasOT && rosterCode.RosterCodeOTStartTime < rosterCode.RosterCodeOutTime && rosterCode.RosterCodeOTStartTime >= rosterCode.RosterCodeInTime)
                rosterOutTime = rosterCode.RosterCodeOTStartTime;

            if (attendanceOutTime < rosterInTime)
                attendanceRecord.AttendanceRecordIsAbsent = true;

            #region Adjust actual IN/Out time for leave application

            DBFilter leaveApplicationFilter = new DBFilter();
            leaveApplicationFilter.add(new Match("EmpID", attendanceRecord.EmpID));
            leaveApplicationFilter.add(new Match("LeaveAppDateTo", ">=", attendanceRecord.AttendanceRecordDate));
            if (rosterCode.RosterCodeOutTime.Ticks.Equals(0))
                leaveApplicationFilter.add(new Match("LeaveAppDateFrom", "<=", attendanceRecord.AttendanceRecordDate));
            else
                leaveApplicationFilter.add(new Match("LeaveAppDateFrom", "<=", rosterCode.RosterCodeOutTime));
            ArrayList leaveApplicationList = ELeaveApplication.db.select(dbConn, leaveApplicationFilter);
            foreach (ELeaveApplication leaveApplication in leaveApplicationList)
            {
                string leaveDetailRemark = string.Empty;
                if (leaveApplication.LeaveAppUnit.Equals("H"))
                {
                    DateTime leaveTimeFrom = leaveApplication.LeaveAppDateFrom.Add(leaveApplication.LeaveAppTimeFrom.TimeOfDay);//leaveApplication.LeaveAppDateFrom.AddHours(leaveApplication.LeaveAppTimeFrom.Hour).AddMinutes(leaveApplication.LeaveAppTimeFrom.Minute);
                    DateTime leaveTimeTo = leaveApplication.LeaveAppDateFrom.Add(leaveApplication.LeaveAppTimeTo.TimeOfDay);//leaveApplication.LeaveAppDateFrom.AddHours(leaveApplication.LeaveAppTimeTo.Hour).AddMinutes(leaveApplication.LeaveAppTimeTo.Minute);

                    if (leaveTimeFrom < rosterOutTime && leaveTimeTo > rosterInTime)
                    {
                        if (leaveTimeFrom < rosterCode.RosterCodeLunchStartTime && leaveTimeTo > rosterInTime)
                            attendanceRecord.AttendanceRecordCalculateWorkingHour -= ((TimeSpan)(
                            (DateTime)(leaveTimeTo < rosterCode.RosterCodeLunchStartTime ? leaveTimeTo : rosterCode.RosterCodeLunchStartTime)).Subtract(
                            (DateTime)(leaveTimeFrom > rosterInTime ? leaveTimeFrom : rosterInTime)
                            )
                            ).TotalHours;
                        if (leaveTimeFrom < rosterOutTime && leaveTimeTo > rosterCode.RosterCodeLunchEndTime)
                            attendanceRecord.AttendanceRecordCalculateWorkingHour -= ((TimeSpan)(
                            (DateTime)(leaveTimeTo < rosterOutTime ? leaveTimeTo : rosterOutTime)).Subtract(
                            (DateTime)(leaveTimeFrom > rosterCode.RosterCodeLunchEndTime ? leaveTimeFrom : rosterCode.RosterCodeLunchEndTime)
                            )
                            ).TotalHours;
                        attendanceRecord.AttendanceRecordCalculateWorkingDay -= leaveApplication.LeaveAppDays;



                        if (leaveTimeFrom <= attendanceInTime && attendanceInTime <= leaveTimeTo)
                            attendanceInTime = leaveTimeFrom;
                        else if (leaveTimeFrom <= rosterInTime && rosterInTime <= leaveTimeTo)
                            rosterInTime = leaveTimeTo;

                        if (leaveTimeFrom <= attendanceOutTime && attendanceOutTime <= leaveTimeTo)
                            attendanceOutTime = leaveTimeTo;
                        else if (leaveTimeFrom <= rosterOutTime && rosterOutTime <= leaveTimeTo)
                            rosterOutTime = leaveTimeFrom;

                        leaveDetailRemark = GenerateLeaveRemark(leaveApplication);
                    }
                }
                else
                {
                    if (leaveApplication.LeaveAppDateFrom <= attendanceRecord.AttendanceRecordDate && leaveApplication.LeaveAppDateTo >= attendanceRecord.AttendanceRecordDate)
                    {
                        rosterInTime = attendanceRecord.AttendanceRecordWorkStart;
                        rosterOutTime = attendanceRecord.AttendanceRecordWorkEnd;
                        attendanceRecord.AttendanceRecordIsAbsent = false;

                        attendanceRecord.AttendanceRecordCalculateWorkingHour = 0;
                        attendanceRecord.AttendanceRecordCalculateWorkingDay = 0;

                        leaveDetailRemark = GenerateLeaveRemark(leaveApplication);
                    }
                }

                if (!string.IsNullOrEmpty(leaveDetailRemark))
                    if (!remark.Contains(leaveDetailRemark))
                        if (string.IsNullOrEmpty(remark))
                            remark = leaveDetailRemark;
                        else
                            remark += "\r\n" + leaveDetailRemark;
            }

            if (attendanceRecord.AttendanceRecordCalculateWorkingHour < 0)
                attendanceRecord.AttendanceRecordCalculateWorkingHour = 0;
            if (attendanceRecord.AttendanceRecordCalculateWorkingDay < 0)
                attendanceRecord.AttendanceRecordCalculateWorkingDay = 0;

            if (rosterOutTime <= rosterInTime && !(rosterCode.RosterCodeType.Equals(ERosterCode.ROSTERTYPE_CODE_RESTDAY) || rosterCode.RosterCodeType.Equals(ERosterCode.ROSTERTYPE_CODE_STATUTORYHOLIDAY) || rosterCode.RosterCodeType.Equals(ERosterCode.ROSTERTYPE_CODE_PUBLICHOLIDAY)))
                attendanceRecord.AttendanceRecordIsAbsent = false;

            #endregion

            if (attendanceRecord.AttendanceRecordIsAbsent)
            {
                attendanceRecord.AttendanceRecordCalculateWorkingDay = 0;
                attendanceRecord.AttendanceRecordCalculateWorkingHour = 0;
            }
            else
            {
                if (!rosterCode.RosterCodeCountWorkHourOnly)
                {
                    //  Roster with book in/out time
                    if (rosterInTime < attendanceInTime && rosterCode.RosterCodeGraceInTime < attendanceInTime && !attendanceInTime.Ticks.Equals(0))
                        attendanceRecord.AttendanceRecordCalculateLateMins = Convert.ToInt32(Math.Truncate(((TimeSpan)rosterInTime.Subtract(attendanceInTime)).Duration().TotalMinutes));
                    else
                        attendanceRecord.AttendanceRecordCalculateLateMins = 0;

                    if (rosterOutTime > attendanceOutTime && rosterCode.RosterCodeGraceOutTime > attendanceOutTime && !attendanceOutTime.Ticks.Equals(0))
                        attendanceRecord.AttendanceRecordCalculateEarlyLeaveMins = Convert.ToInt32(Math.Truncate(((TimeSpan)rosterOutTime.Subtract(attendanceOutTime)).Duration().TotalMinutes));
                    else
                        attendanceRecord.AttendanceRecordCalculateEarlyLeaveMins = 0;

                    int lunchOTmins = 0;
                    if (rosterCode.RosterCodeHasLunch)
                    {

                        //if (!rosterCode.RosterCodeLunchStartTime.Ticks.Equals(0) && !rosterCode.RosterCodeLunchEndTime.Ticks.Equals(0))
                        //    attendanceRecord.AttendanceRecordCalculateLunchTimeMins = Convert.ToInt32(((TimeSpan)rosterCode.RosterCodeLunchEndTime.Subtract(rosterCode.RosterCodeLunchStartTime)).Duration().TotalMinutes);
                        if (attendanceRecord.AttendanceRecordCalculateWorkingHour >= rosterCode.RosterCodeLunchDeductMinimumWorkHour)
                            attendanceRecord.AttendanceRecordCalculateLunchTimeMins = Convert.ToInt32(rosterCode.RosterCodeLunchDurationHour * 60.0);

                        if (rosterCode.RosterCodeLunchStartTime.Ticks.Equals(0) && rosterCode.RosterCodeLunchEndTime.Ticks.Equals(0))
                        {
                            if (!attendanceRecord.AttendanceRecordLunchOut.Ticks.Equals(0) && !attendanceRecord.AttendanceRecordLunchIn.Ticks.Equals(0))
                            {
                                int lunchLateMin = Convert.ToInt32(Math.Truncate(((TimeSpan)attendanceRecord.AttendanceRecordLunchOut.Subtract(attendanceRecord.AttendanceRecordLunchIn)).Duration().TotalMinutes)) - attendanceRecord.AttendanceRecordCalculateLunchTimeMins;
                                if (lunchLateMin > 0)
                                    attendanceRecord.AttendanceRecordCalculateLunchLateMins = lunchLateMin;
                                else
                                    lunchOTmins = -lunchLateMin;
                            }

                        }
                        else if (attendanceRecord.AttendanceRecordLunchOut.Ticks.Equals(0) && attendanceRecord.AttendanceRecordLunchIn.Ticks.Equals(0))
                        {
                            lunchOTmins = attendanceRecord.AttendanceRecordCalculateLunchTimeMins;
                        }
                        else
                        {
                            if (!rosterCode.RosterCodeLunchStartTime.Ticks.Equals(0) && !attendanceRecord.AttendanceRecordLunchOut.Ticks.Equals(0))
                                if (rosterCode.RosterCodeLunchStartTime > attendanceRecord.AttendanceRecordLunchOut)
                                    attendanceRecord.AttendanceRecordCalculateLunchEarlyLeaveMins = Convert.ToInt32(Math.Truncate(((TimeSpan)rosterCode.RosterCodeLunchStartTime.Subtract(attendanceRecord.AttendanceRecordLunchOut)).Duration().TotalMinutes));
                                else
                                    lunchOTmins += Convert.ToInt32(Math.Truncate(((TimeSpan)rosterCode.RosterCodeLunchStartTime.Subtract(attendanceRecord.AttendanceRecordLunchOut)).Duration().TotalMinutes));


                            if (!rosterCode.RosterCodeLunchEndTime.Ticks.Equals(0) && !attendanceRecord.AttendanceRecordLunchIn.Ticks.Equals(0))
                                if (rosterCode.RosterCodeLunchEndTime < attendanceRecord.AttendanceRecordLunchIn)
                                    attendanceRecord.AttendanceRecordCalculateLunchLateMins = Convert.ToInt32(Math.Truncate(((TimeSpan)rosterCode.RosterCodeLunchEndTime.Subtract(attendanceRecord.AttendanceRecordLunchIn)).Duration().TotalMinutes));
                                else
                                    lunchOTmins += Convert.ToInt32(Math.Truncate(((TimeSpan)rosterCode.RosterCodeLunchEndTime.Subtract(attendanceRecord.AttendanceRecordLunchIn)).Duration().TotalMinutes));

                        }
                    }

                    if (rosterCode.RosterCodeHasOT)
                    {
                        DateTime OTStartTime;
                        DateTime OTEligibleStartTime = rosterCode.RosterCodeOTStartTime;
                        DateTime OTEligibleEndTime = rosterCode.RosterCodeOTEndTime;
                        if (rosterCode.RosterCodeIsOTStartFromOutTime)
                            OTStartTime = rosterCode.RosterCodeOutTime;
                        else
                            OTStartTime = OTEligibleStartTime;

                        int otShiftMins = 0;
                        if (rosterCode.RosterCodeOTShiftStartTimeForLate && rosterInTime < attendanceInTime)
                        {
                            otShiftMins = Convert.ToInt32(Math.Truncate(((TimeSpan)rosterInTime.Subtract(attendanceInTime)).Duration().TotalMinutes));
                            OTStartTime = OTStartTime.AddMinutes(otShiftMins);
                            OTEligibleStartTime = OTEligibleStartTime.AddMinutes(otShiftMins);
                            OTEligibleEndTime = OTEligibleEndTime.AddMinutes(otShiftMins);
                        }

                        if (attendanceOutTime > OTEligibleStartTime)
                        {
                            if (otShiftMins > 0)
                                attendanceRecord.AttendanceRecordCalculateLateMins = 0;

                            if (attendanceOutTime < OTEligibleEndTime)
                                attendanceRecord.AttendanceRecordCalculateOvertimeMins = Convert.ToInt32(Math.Truncate(((TimeSpan)OTStartTime.Subtract(attendanceOutTime)).Duration().TotalMinutes));
                            else
                                attendanceRecord.AttendanceRecordCalculateOvertimeMins = Convert.ToInt32(Math.Truncate(((TimeSpan)OTStartTime.Subtract(OTEligibleEndTime)).Duration().TotalMinutes));
                        }
                        else
                            attendanceRecord.AttendanceRecordCalculateOvertimeMins = 0;
                        if (rosterCode.RosterCodeOTIncludeLunch && lunchOTmins > 0)
                            attendanceRecord.AttendanceRecordCalculateLunchOvertimeMins = lunchOTmins;
                        else
                            attendanceRecord.AttendanceRecordCalculateLunchOvertimeMins = 0;
                    }


                }
                else
                {
                    TimeSpan duration = attendanceOutTime.Subtract(attendanceInTime);

                    int durationMins = Convert.ToInt32(duration.TotalMinutes);
                    if (rosterCode.RosterCodeHasLunch)
                    {
                        if (rosterCode.RosterCodeLunchIsDeductWorkingHour)
                        {
                            int lunchMins = 0;
                            if (!attendanceRecord.AttendanceRecordLunchOut.Ticks.Equals(0) && !attendanceRecord.AttendanceRecordLunchIn.Ticks.Equals(0))
                                lunchMins = Convert.ToInt32(attendanceRecord.AttendanceRecordLunchIn.Subtract(attendanceRecord.AttendanceRecordLunchOut).TotalMinutes);

                            if (durationMins >= rosterCode.RosterCodeLunchDeductMinimumWorkHour * 60 && lunchMins < rosterCode.RosterCodeLunchDurationHour * 60)
                                lunchMins = Convert.ToInt32(rosterCode.RosterCodeLunchDurationHour * 60.0);

                            //  Lunch Unit Rounding
                            lunchMins = AppUtils.ApplyRoundingRule(lunchMins, rosterCode.RosterCodeLunchDeductWorkingHourMinsRoundingRule, rosterCode.RosterCodeLunchDeductWorkingHourMinsUnit);
                            durationMins -= lunchMins;
                            attendanceRecord.AttendanceRecordCalculateLunchTimeMins = lunchMins;

                        }
                    }
                    //  Use Calculate work hour under Attendance record for work hour after deduct leave hour unit
                    int workHourMins = Convert.ToInt32(Math.Round(attendanceRecord.AttendanceRecordCalculateWorkingHour * 60, 0));
                    if (durationMins >= workHourMins)
                    {
                        attendanceRecord.AttendanceRecordCalculateOvertimeMins = durationMins - workHourMins;
                        if (!rosterCode.RosterCodeIsOTStartFromOutTime)
                        {
                            attendanceRecord.AttendanceRecordCalculateOvertimeMins -= rosterCode.RosterCodeCountOTAfterWorkHourMin;
                            if (attendanceRecord.AttendanceRecordCalculateOvertimeMins < 0)
                                attendanceRecord.AttendanceRecordCalculateOvertimeMins = 0;
                        }
                    }
                    else
                    {
                        if (rosterCode.RosterCodeUseHalfWorkingDaysHours && rosterCode.RosterCodeDailyWorkingHour.Equals(attendanceRecord.AttendanceRecordCalculateWorkingHour) && rosterCode.RosterCodeWorkingDayUnit.Equals(attendanceRecord.AttendanceRecordCalculateWorkingDay))
                        {
                            if (durationMins <= workHourMins / 2)
                            {
                                attendanceRecord.AttendanceRecordCalculateWorkingHour /= 2;
                                attendanceRecord.AttendanceRecordCalculateWorkingDay /= 2;
                                workHourMins /= 2;
                            }
                        }
                        attendanceRecord.AttendanceRecordCalculateEarlyLeaveMins = workHourMins - durationMins;
                    }
                }
                if (rosterCode.RosterCodeType.Equals(ERosterCode.ROSTERTYPE_CODE_RESTDAY) || rosterCode.RosterCodeType.Equals(ERosterCode.ROSTERTYPE_CODE_STATUTORYHOLIDAY) || rosterCode.RosterCodeType.Equals(ERosterCode.ROSTERTYPE_CODE_PUBLICHOLIDAY))
                {
                    attendanceRecord.AttendanceRecordCalculateLateMins = 0;
                    attendanceRecord.AttendanceRecordCalculateEarlyLeaveMins = 0;
                }
            }

            attendanceRecord.AttendanceRecordRemark = remark;
            attendanceRecord.AttendanceRecordActualWorkingHour = attendanceRecord.AttendanceRecordCalculateWorkingHour;
            attendanceRecord.AttendanceRecordActualWorkingDay = attendanceRecord.AttendanceRecordCalculateWorkingDay;

            attendanceRecord.AttendanceRecordActualLunchTimeMins = attendanceRecord.AttendanceRecordCalculateLunchTimeMins;

            attendanceRecord.AttendanceRecordActualEarlyLeaveMins = AppUtils.ApplyRoundingRule(attendanceRecord.AttendanceRecordCalculateEarlyLeaveMins, rosterCode.RosterCodeEarlyLeaveMinsRoundingRule, rosterCode.RosterCodeEarlyLeaveMinsUnit);
            attendanceRecord.AttendanceRecordActualLateMins = AppUtils.ApplyRoundingRule(attendanceRecord.AttendanceRecordCalculateLateMins, rosterCode.RosterCodeLateMinsRoundingRule, rosterCode.RosterCodeLateMinsUnit);

            attendanceRecord.AttendanceRecordActualLunchEarlyLeaveMins = AppUtils.ApplyRoundingRule(attendanceRecord.AttendanceRecordCalculateLunchEarlyLeaveMins, rosterCode.RosterCodeLunchEarlyLeaveMinsRoundingRule, rosterCode.RosterCodeLunchEarlyLeaveMinsUnit);
            attendanceRecord.AttendanceRecordActualLunchLateMins = AppUtils.ApplyRoundingRule(attendanceRecord.AttendanceRecordCalculateLunchLateMins, rosterCode.RosterCodeLunchLateMinsRoundingRule, rosterCode.RosterCodeLunchLateMinsUnit);
            
            attendanceRecord.AttendanceRecordActualOvertimeMins = AppUtils.ApplyRoundingRule(attendanceRecord.AttendanceRecordCalculateOvertimeMins, rosterCode.RosterCodeOTMinsRoundingRule, rosterCode.RosterCodeOTMinsUnit);
            attendanceRecord.AttendanceRecordActualLunchOvertimeMins = AppUtils.ApplyRoundingRule(attendanceRecord.AttendanceRecordCalculateLunchOvertimeMins, rosterCode.RosterCodeOTMinsRoundingRule, rosterCode.RosterCodeOTMinsUnit);

            if (rosterCode.RosterCodeType.Equals(ERosterCode.ROSTERTYPE_CODE_RESTDAY) || rosterCode.RosterCodeType.Equals(ERosterCode.ROSTERTYPE_CODE_STATUTORYHOLIDAY) || rosterCode.RosterCodeType.Equals(ERosterCode.ROSTERTYPE_CODE_PUBLICHOLIDAY))
            {
                attendanceRecord.AttendanceRecordIsAbsent = false;
            }
            return attendanceRecord;
        }

        private string GetTimeCardNo(int EmpID)
        {
            EEmpPersonalInfo empInfo = new EEmpPersonalInfo();
            empInfo.EmpID = EmpID;
            if (EEmpPersonalInfo.db.select(dbConn, empInfo))
                if (!string.IsNullOrEmpty(empInfo.EmpTimeCardNo))
                    return empInfo.EmpTimeCardNo;
                else
                    return empInfo.EmpNo;
            else
                return string.Empty;
        }

        //private static DateTime GetPreviousCutOffTime(int EmpID, ERosterTable currentRosterTable, DateTime currentCutOffTime)
        //{
        //    DBFilter rosterTableFilter = new DBFilter();
        //    rosterTableFilter.add(new Match("EmpID", EmpID));
        //    rosterTableFilter.add(new Match("RosterTableDate", "=", currentRosterTable.RosterTableDate ));
        //    rosterTableFilter.add(new Match("RosterTableID", "<>", currentRosterTable.RosterTableID));
        //    ArrayList rosterTableList = ERosterTable.db.select(dbConn, rosterTableFilter);
        //    foreach (ERosterTable rosterTable in rosterTableList)
        //    {
        //        ERosterCode rosterCode = new ERosterCode();
        //        rosterCode.RosterCodeID = rosterTable.RosterCodeID;
        //        if (ERosterCode.db.select(dbConn, rosterCode))
        //        {
        //            rosterCode = GetActualRosterDateTime(rosterTable.RosterTableDate, rosterCode);
        //            if (rosterCode.RosterCodeCutOffTime < currentCutOffTime)
        //                return rosterCode.RosterCodeCutOffTime;
        //        }


        //    }
        //}

        //public static ERosterCode GetActualRosterDateTime(DateTime currentDateTime, ERosterCode rosterCode)
        //{
        //    return GetActualRosterDateTime(currentDateTime, rosterCode, new DateTime(), new DateTime());
        //}
        //public static ERosterCode GetActualRosterDateTime(DateTime currentDateTime, ERosterCode OriginalRosterCode, int EmpID)
        //{
        //    DBFilter rosterTableFilter = new DBFilter();
        //    rosterTableFilter.add(new Match("EmpID", EmpID));
        //    rosterTableFilter.add(new Match("RosterCodeID", OriginalRosterCode.RosterCodeID));
        //    rosterTableFilter.add(new Match("RosterTableDate", currentDateTime));
        //    ArrayList rosterTableList = ERosterTable.db.select(dbConn, rosterTableFilter);
        //    if (rosterTableList.Count > 0)
        //    {
        //        ERosterTable rosterTable = (ERosterTable)rosterTableList[0];
        //        return GetActualRosterDateTime(currentDateTime, OriginalRosterCode, rosterTable.RosterTableOverrideInTime, rosterTable.RosterTableOverrideOutTime);
        //    }
        //    else
        //        return GetActualRosterDateTime(currentDateTime, OriginalRosterCode, new DateTime(), new DateTime());
        //}
        //public static ERosterCode GetActualRosterDateTime(DateTime currentDateTime, ERosterCode OriginalRosterCode, DateTime OverrideInTime, DateTime OverrideOutTime)
        //{
        //    return GetActualRosterDateTime(currentDateTime, OriginalRosterCode, OverrideInTime, OverrideOutTime, new DateTime(), new DateTime());
        //}
        public static ERosterCode GetActualRosterDateTime(DateTime currentDateTime, ERosterCode OriginalRosterCode, DateTime OverrideInTime, DateTime OverrideOutTime, DateTime OverrideLunchStartTime, DateTime OverrideLunchEndTime)
        {
            ERosterCode rosterCode = new ERosterCode();
            ERosterCode.db.copyObject(OriginalRosterCode, rosterCode);

            //if (rosterCode.RosterCodeOTStartTime.TimeOfDay.Equals(rosterCode.RosterCodeOutTime))
            //    rosterCode.RosterCodeOTStartTime = new DateTime();

            if (!OverrideInTime.Ticks.Equals(0))
            {
                if (!rosterCode.RosterCodeGraceInTime.Ticks.Equals(0))
                {
                    TimeSpan different = rosterCode.RosterCodeGraceInTime.Subtract(rosterCode.RosterCodeInTime);
                    rosterCode.RosterCodeGraceInTime = OverrideInTime.Add(different);
                }
                rosterCode.RosterCodeInTime = OverrideInTime;
            }

            if (!OverrideOutTime.Ticks.Equals(0))
            {
                if (!rosterCode.RosterCodeGraceOutTime.Ticks.Equals(0))
                {
                    TimeSpan different = rosterCode.RosterCodeGraceOutTime.Subtract(rosterCode.RosterCodeOutTime);
                    rosterCode.RosterCodeGraceOutTime = OverrideOutTime.Add(different);
                }
                rosterCode.RosterCodeOutTime = OverrideOutTime;
            }
            if (!OverrideLunchStartTime.Ticks.Equals(0))
            {
                rosterCode.RosterCodeLunchStartTime = OverrideLunchStartTime;
            }
            if (!OverrideLunchEndTime.Ticks.Equals(0))
            {
                rosterCode.RosterCodeLunchEndTime = OverrideLunchEndTime;
            }
            if (!rosterCode.RosterCodeInTime.TimeOfDay.Equals(OriginalRosterCode.RosterCodeInTime.TimeOfDay) 
                || !rosterCode.RosterCodeOutTime.TimeOfDay.Equals(OriginalRosterCode.RosterCodeOutTime.TimeOfDay)
                || !rosterCode.RosterCodeLunchStartTime.TimeOfDay.Equals(OriginalRosterCode.RosterCodeLunchStartTime.TimeOfDay)
                || !rosterCode.RosterCodeLunchEndTime.TimeOfDay.Equals(OriginalRosterCode.RosterCodeLunchEndTime.TimeOfDay))
            {
                double inTimeShiftMins = OriginalRosterCode.RosterCodeInTime.TimeOfDay.Subtract(rosterCode.RosterCodeInTime.TimeOfDay).TotalMinutes;
                double lunchOutTimeShiftMins = ((TimeSpan)OriginalRosterCode.RosterCodeLunchStartTime.TimeOfDay.Subtract(rosterCode.RosterCodeLunchStartTime.TimeOfDay)).TotalMinutes;
                double lunchInTimeShiftMins = OriginalRosterCode.RosterCodeLunchEndTime.TimeOfDay.Subtract(rosterCode.RosterCodeLunchEndTime.TimeOfDay).TotalMinutes;
                double outTimeShiftMins = ((TimeSpan)OriginalRosterCode.RosterCodeOutTime.TimeOfDay.Subtract(rosterCode.RosterCodeOutTime.TimeOfDay)).TotalMinutes;

                if (OriginalRosterCode.RosterCodeLunchStartTime.Ticks.Equals(0) && OriginalRosterCode.RosterCodeLunchEndTime.Ticks.Equals(0))
                {
                    if (!rosterCode.RosterCodeLunchStartTime.Ticks.Equals(0) && !rosterCode.RosterCodeLunchEndTime.Ticks.Equals(0))
                    {
                        lunchInTimeShiftMins = 0;
                        lunchOutTimeShiftMins = rosterCode.RosterCodeLunchEndTime.TimeOfDay.Subtract(rosterCode.RosterCodeLunchStartTime.TimeOfDay).TotalMinutes - OriginalRosterCode.RosterCodeLunchDurationHour * 60; 
                    }
                    else
                    {
                        lunchInTimeShiftMins = 0;
                        lunchOutTimeShiftMins = 0;
                    }
                }
                else
                {
                }
                if (OriginalRosterCode.RosterCodeOutTime.TimeOfDay < OriginalRosterCode.RosterCodeInTime.TimeOfDay)
                    outTimeShiftMins += 24 * 60;
                if (rosterCode.RosterCodeOutTime.TimeOfDay < rosterCode.RosterCodeInTime.TimeOfDay)
                    outTimeShiftMins -= 24 * 60;
                rosterCode.RosterCodeDailyWorkingHour += ((inTimeShiftMins + lunchInTimeShiftMins) - (outTimeShiftMins + lunchOutTimeShiftMins)) / 60.0;
                rosterCode.RosterCodeLunchDurationHour -= ((lunchInTimeShiftMins) - (lunchOutTimeShiftMins)) / 60.0;
                if (outTimeShiftMins < 0)
                {
                    if (!rosterCode.RosterCodeOTStartTime.Ticks.Equals(0))
                        rosterCode.RosterCodeOTStartTime = rosterCode.RosterCodeOTStartTime.Add(new TimeSpan(0, Convert.ToInt32(-outTimeShiftMins), 0));
                    if (!rosterCode.RosterCodeOTEndTime.Ticks.Equals(0))
                        rosterCode.RosterCodeOTEndTime = rosterCode.RosterCodeOTEndTime.Add(new TimeSpan(0, Convert.ToInt32(-outTimeShiftMins), 0));
                }
            }

            if (rosterCode.RosterCodeGraceInTime.Ticks.Equals(0))
                rosterCode.RosterCodeGraceInTime=rosterCode.RosterCodeInTime;

            if (rosterCode.RosterCodeGraceOutTime.Ticks.Equals(0))
                rosterCode.RosterCodeGraceOutTime=rosterCode.RosterCodeOutTime;

            DateTime InDateTime = currentDateTime.Add(rosterCode.RosterCodeInTime.TimeOfDay);// currentDateTime.AddHours(rosterCode.RosterCodeInTime.Hour).AddMinutes(rosterCode.RosterCodeInTime.Minute);
            DateTime OutDateTime = currentDateTime.Add(rosterCode.RosterCodeOutTime.TimeOfDay);// currentDateTime.AddHours(rosterCode.RosterCodeOutTime.Hour).AddMinutes(rosterCode.RosterCodeOutTime.Minute);
            DateTime GraceInDateTime = currentDateTime.Add(rosterCode.RosterCodeGraceInTime.TimeOfDay);// currentDateTime.AddHours(rosterCode.RosterCodeGraceInTime.Hour).AddMinutes(rosterCode.RosterCodeGraceInTime.Minute);
            DateTime GraceOutDateTime = currentDateTime.Add(rosterCode.RosterCodeGraceOutTime.TimeOfDay);//currentDateTime.AddHours(rosterCode.RosterCodeGraceOutTime.Hour).AddMinutes(rosterCode.RosterCodeGraceOutTime.Minute);
            DateTime CutOffDateTime = currentDateTime.Add(rosterCode.RosterCodeCutOffTime.TimeOfDay);//currentDateTime.AddHours(rosterCode.RosterCodeCutOffTime.Hour).AddMinutes(rosterCode.RosterCodeCutOffTime.Minute);



            if (!rosterCode.RosterCodeCountWorkHourOnly)
            {
                if (rosterCode.RosterCodeType.Equals(ERosterCode.ROSTERTYPE_CODE_OVERNIGHT))
                {
                    OutDateTime = OutDateTime.AddDays(1);
                    CutOffDateTime = CutOffDateTime.AddDays(1);
                }

                if (InDateTime >= CutOffDateTime)
                {
                    CutOffDateTime = CutOffDateTime.AddDays(1);
                }
                if (InDateTime > OutDateTime)
                {
                    OutDateTime = OutDateTime.AddDays(1);
                    //                CutOffDateTime = CutOffDateTime.AddDays(1);
                }
                if (OutDateTime > CutOffDateTime)
                {
                    CutOffDateTime = CutOffDateTime.AddDays(1);
                }
            }
            else
            {
                InDateTime = CutOffDateTime;
                OutDateTime = InDateTime.AddDays(1).AddMilliseconds(-1);
                CutOffDateTime = CutOffDateTime.AddDays(1);
            }

            DateTime StartDateTime = CutOffDateTime.AddDays(-1);
            while (StartDateTime > InDateTime)
                StartDateTime = StartDateTime.AddDays(-1);

            rosterCode.RosterCodeInTime = InDateTime;
            rosterCode.RosterCodeOutTime = OutDateTime;
            rosterCode.RosterCodeCutOffTime = CutOffDateTime;
            rosterCode.RosterCodeDayStartTime = StartDateTime;

            if (InDateTime < GraceInDateTime)
                while (((TimeSpan)InDateTime.Subtract(GraceInDateTime)).Duration().TotalMinutes > ((TimeSpan)InDateTime.Subtract(GraceInDateTime.AddDays(-1))).Duration().TotalMinutes)
                    GraceInDateTime = GraceInDateTime.AddDays(-1);

            if (InDateTime > GraceInDateTime)
                while (((TimeSpan)InDateTime.Subtract(GraceInDateTime)).Duration().TotalMinutes > ((TimeSpan)InDateTime.Subtract(GraceInDateTime.AddDays(1))).Duration().TotalMinutes)
                    GraceInDateTime = GraceInDateTime.AddDays(1);

            if (OutDateTime < GraceOutDateTime)
                while (((TimeSpan)OutDateTime.Subtract(GraceOutDateTime)).Duration().TotalMinutes > ((TimeSpan)OutDateTime.Subtract(GraceOutDateTime.AddDays(-1))).Duration().TotalMinutes)
                    GraceOutDateTime = GraceOutDateTime.AddDays(-1);

            if (OutDateTime > GraceOutDateTime)
                while (((TimeSpan)OutDateTime.Subtract(GraceOutDateTime)).Duration().TotalMinutes > ((TimeSpan)OutDateTime.Subtract(GraceOutDateTime.AddDays(1))).Duration().TotalMinutes)
                    GraceOutDateTime = GraceOutDateTime.AddDays(1);

            rosterCode.RosterCodeGraceInTime = GraceInDateTime;
            rosterCode.RosterCodeGraceOutTime = GraceOutDateTime;


            if (rosterCode.RosterCodeHasLunch)
            {
                if (!rosterCode.RosterCodeLunchStartTime.Ticks.Equals(0))
                {
                    rosterCode.RosterCodeLunchStartTime = currentDateTime.Add(rosterCode.RosterCodeLunchStartTime.TimeOfDay);// currentDateTime.AddHours(rosterCode.RosterCodeLunchStartTime.Hour).AddMinutes(rosterCode.RosterCodeLunchStartTime.Minute);
                }
                if (!rosterCode.RosterCodeLunchEndTime.Ticks.Equals(0))
                {
                    rosterCode.RosterCodeLunchEndTime = currentDateTime.Add(rosterCode.RosterCodeLunchEndTime.TimeOfDay); //currentDateTime.AddHours(rosterCode.RosterCodeLunchEndTime.Hour).AddMinutes(rosterCode.RosterCodeLunchEndTime.Minute);
                }

                if (rosterCode.RosterCodeLunchEndTime < rosterCode.RosterCodeLunchStartTime && !rosterCode.RosterCodeLunchStartTime.Ticks.Equals(0))
                    rosterCode.RosterCodeLunchEndTime = rosterCode.RosterCodeLunchEndTime.AddDays(1);

                if (rosterCode.RosterCodeLunchStartTime < rosterCode.RosterCodeInTime && !rosterCode.RosterCodeLunchStartTime.Ticks.Equals(0))
                {
                    rosterCode.RosterCodeLunchStartTime = rosterCode.RosterCodeLunchStartTime.AddDays(1);
                    rosterCode.RosterCodeLunchEndTime = rosterCode.RosterCodeLunchEndTime.AddDays(1);
                }

                if (!rosterCode.RosterCodeCountWorkHourOnly && rosterCode.RosterCodeOutTime < rosterCode.RosterCodeLunchEndTime)
                {
                    rosterCode.RosterCodeHasLunch = false;
                    rosterCode.RosterCodeLunchStartTime = new DateTime();
                    rosterCode.RosterCodeLunchEndTime = new DateTime();
                }
            }
            else
            {
                rosterCode.RosterCodeLunchStartTime = new DateTime();
                rosterCode.RosterCodeLunchEndTime = new DateTime();
            }
            if (rosterCode.RosterCodeHasOT)
            {
                if (rosterCode.RosterCodeOTStartTime.Ticks.Equals(0))
                    rosterCode.RosterCodeOTStartTime = rosterCode.RosterCodeOutTime;

                if (rosterCode.RosterCodeOTEndTime.Ticks.Equals(0))
                    rosterCode.RosterCodeOTEndTime = rosterCode.RosterCodeCutOffTime;

                rosterCode.RosterCodeOTStartTime = currentDateTime.Add(rosterCode.RosterCodeOTStartTime.TimeOfDay); //currentDateTime.AddHours(rosterCode.RosterCodeOTStartTime.Hour).AddMinutes(rosterCode.RosterCodeOTStartTime.Minute);
                rosterCode.RosterCodeOTEndTime = currentDateTime.Add(rosterCode.RosterCodeOTEndTime.TimeOfDay); //currentDateTime.AddHours(rosterCode.RosterCodeOTEndTime.Hour).AddMinutes(rosterCode.RosterCodeOTEndTime.Minute);

                if (rosterCode.RosterCodeOTEndTime < rosterCode.RosterCodeOTStartTime)
                    rosterCode.RosterCodeOTEndTime=rosterCode.RosterCodeOTEndTime.AddDays(1);
                
                //  compare OT start time  to In Time for supporting OT hour during roster period.
                if (rosterCode.RosterCodeOTStartTime < rosterCode.RosterCodeInTime)
                {
                    rosterCode.RosterCodeOTStartTime=rosterCode.RosterCodeOTStartTime.AddDays(1);
                    rosterCode.RosterCodeOTEndTime=rosterCode.RosterCodeOTEndTime.AddDays(1);
                }

                //if (rosterCode.RosterCodeCutOffTime < rosterCode.RosterCodeOTEndTime)
                //{
                //    rosterCode.RosterCodeHasOT = false;
                //    rosterCode.RosterCodeOTStartTime = new DateTime();
                //    rosterCode.RosterCodeOTEndTime = new DateTime();
                //}
            }
            else
            {
                rosterCode.RosterCodeOTStartTime = new DateTime();
                rosterCode.RosterCodeOTEndTime = new DateTime();
            }
            return rosterCode;
        }
        protected string GenerateLeaveRemark(ELeaveApplication leaveApp)
        {
            string remark=string.Empty ;

            ELeaveCode leaveCode= new ELeaveCode();
            leaveCode.LeaveCodeID = leaveApp.LeaveCodeID;
            if (ELeaveCode.db.select(dbConn, leaveCode))
                remark += leaveCode.LeaveCode + " - " + leaveCode.LeaveCodeDesc;
            else
                remark += "Unknown Leave";

            // Start 0000056, Ricky So, 2014/07/03
            switch (leaveApp.LeaveAppUnit)
            {
                case "H": remark += (" " + leaveApp.LeaveAppTimeFrom.ToString("HH:mm") + " - " + leaveApp.LeaveAppTimeTo.ToString("HH:mm"));
                          break;
                case "A": remark += " (A.M.)";
                          break;
                case "P": remark += " (P.M.)";
                          break;
                // Start 0000201, Ricky So, 2015-06-01
                case "D": if (leaveApp.LeaveAppDateFromAM == "PM")
                              remark += " (P.M.)";
                          else if (leaveApp.LeaveAppDateToAM == "AM")
                              remark += " (A.M.)";
                          break;    
                // End 0000201, Ricky So, 2015-06-01
            }
            //if (leaveApp.LeaveAppUnit.Equals("H"))
            //    remark += leaveApp.LeaveAppTimeFrom.ToString("HH:mm") + " - " + leaveApp.LeaveAppTimeTo.ToString("HH:mm");
            // End 0000056, Ricky So, 2014/07/03
            return remark;            

        }

    }
}
