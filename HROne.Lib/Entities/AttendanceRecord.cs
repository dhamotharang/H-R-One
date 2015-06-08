using System;
using System.Collections.Generic;
using System.Text;
using HROne.DataAccess;
using HROne.CommonLib;

namespace HROne.Lib.Entities
{
    [DBClass("AttendanceRecord")]
    public class EAttendanceRecord : BaseObject
    {
        public static DBManager db = new DBManager(typeof(EAttendanceRecord));
        public static WFValueList VLAttendanceRecordYear = new AppUtils.WFDBDistinctList(db, "Year(AttendanceRecordDate)", " Year(AttendanceRecordDate)", "Year(AttendanceRecordDate) desc");
        public const string FIELD_EXTENDDATA_OVERRIDE_DAILY_PAYMENT = "OverrideDailyPayment";
        public const string FIELD_EXTENDDATA_WORK_AS_OVERTIME = "WorkAsOvertime";

        protected int m_AttendanceRecordID;
        [DBField("AttendanceRecordID", true, true), TextSearch, Export(false)]
        public int AttendanceRecordID
        {
            get { return m_AttendanceRecordID; }
            set { m_AttendanceRecordID = value; modify("AttendanceRecordID"); }
        }
        protected int m_EmpID;
        [DBField("EmpID"), TextSearch, Export(false), Required]
        public int EmpID
        {
            get { return m_EmpID; }
            set { m_EmpID = value; modify("EmpID"); }
        }
        protected DateTime m_AttendanceRecordDate;
        [DBField("AttendanceRecordDate"), TextSearch, Export(false), Required]
        public DateTime AttendanceRecordDate
        {
            get { return m_AttendanceRecordDate; }
            set { m_AttendanceRecordDate = value; modify("AttendanceRecordDate"); }
        }
        protected int m_RosterCodeID;
        [DBField("RosterCodeID"), TextSearch, Export(false)]
        public int RosterCodeID
        {
            get { return m_RosterCodeID; }
            set { m_RosterCodeID = value; modify("RosterCodeID"); }
        }
        protected int m_RosterTableID;
        [DBField("RosterTableID"), TextSearch, Export(false)]
        public int RosterTableID
        {
            get { return m_RosterTableID; }
            set { m_RosterTableID = value; modify("RosterTableID"); }
        }
        protected DateTime m_AttendanceRecordWorkStart;
        [DBField("AttendanceRecordWorkStart", "HH:mm"), TextSearch, MaxLength(5), Export(false)]
        public DateTime AttendanceRecordWorkStart
        {
            get { return m_AttendanceRecordWorkStart; }
            set { m_AttendanceRecordWorkStart = value; modify("AttendanceRecordWorkStart"); }
        }

        protected string m_AttendanceRecordWorkStartLocation;
        [DBField("AttendanceRecordWorkStartLocation"), TextSearch, MaxLength(50, 5), Export(false)]
        public string AttendanceRecordWorkStartLocation
        {
            get { return m_AttendanceRecordWorkStartLocation; }
            set { m_AttendanceRecordWorkStartLocation = value; modify("AttendanceRecordWorkStartLocation"); }
        }
        protected int m_AttendanceRecordWorkStartTimeCardRecordID;
        [DBField("AttendanceRecordWorkStartTimeCardRecordID"), TextSearch, Export(false)]
        public int AttendanceRecordWorkStartTimeCardRecordID
        {
            get { return m_AttendanceRecordWorkStartTimeCardRecordID; }
            set { m_AttendanceRecordWorkStartTimeCardRecordID = value; modify("AttendanceRecordWorkStartTimeCardRecordID"); }
        }
        
        protected DateTime m_AttendanceRecordLunchOut;
        [DBField("AttendanceRecordLunchOut", "HH:mm"), TextSearch, MaxLength(5), Export(false)]
        public DateTime AttendanceRecordLunchOut
        {
            get { return m_AttendanceRecordLunchOut; }
            set { m_AttendanceRecordLunchOut = value; modify("AttendanceRecordLunchOut"); }
        }
        protected string m_AttendanceRecordLunchOutLocation;
        [DBField("AttendanceRecordLunchOutLocation"), TextSearch, MaxLength(50, 5), Export(false)]
        public string AttendanceRecordLunchOutLocation
        {
            get { return m_AttendanceRecordLunchOutLocation; }
            set { m_AttendanceRecordLunchOutLocation = value; modify("AttendanceRecordLunchOutLocation"); }
        }
        protected int m_AttendanceRecordLunchOutTimeCardRecordID;
        [DBField("AttendanceRecordLunchOutTimeCardRecordID"), TextSearch, Export(false)]
        public int AttendanceRecordLunchOutTimeCardRecordID
        {
            get { return m_AttendanceRecordLunchOutTimeCardRecordID; }
            set { m_AttendanceRecordLunchOutTimeCardRecordID = value; modify("AttendanceRecordLunchOutTimeCardRecordID"); }
        }

        protected DateTime m_AttendanceRecordLunchIn;
        [DBField("AttendanceRecordLunchIn", "HH:mm"), TextSearch, MaxLength(5), Export(false)]
        public DateTime AttendanceRecordLunchIn
        {
            get { return m_AttendanceRecordLunchIn; }
            set { m_AttendanceRecordLunchIn = value; modify("AttendanceRecordLunchIn"); }
        }

        protected string m_AttendanceRecordLunchInLocation;
        [DBField("AttendanceRecordLunchInLocation"), TextSearch, MaxLength(50, 5), Export(false)]
        public string AttendanceRecordLunchInLocation
        {
            get { return m_AttendanceRecordLunchInLocation; }
            set { m_AttendanceRecordLunchInLocation = value; modify("AttendanceRecordLunchInLocation"); }
        }
        protected int m_AttendanceRecordLunchInTimeCardRecordID;
        [DBField("AttendanceRecordLunchInTimeCardRecordID"), TextSearch, Export(false)]
        public int AttendanceRecordLunchInTimeCardRecordID
        {
            get { return m_AttendanceRecordLunchInTimeCardRecordID; }
            set { m_AttendanceRecordLunchInTimeCardRecordID = value; modify("AttendanceRecordLunchInTimeCardRecordID"); }
        }

        protected DateTime m_AttendanceRecordWorkEnd;
        [DBField("AttendanceRecordWorkEnd", "HH:mm"), TextSearch, MaxLength(5), Export(false)]
        public DateTime AttendanceRecordWorkEnd
        {
            get { return m_AttendanceRecordWorkEnd; }
            set { m_AttendanceRecordWorkEnd = value; modify("AttendanceRecordWorkEnd"); }
        }

        protected string m_AttendanceRecordWorkEndLocation;
        [DBField("AttendanceRecordWorkEndLocation"), TextSearch, MaxLength(50, 5), Export(false)]
        public string AttendanceRecordWorkEndLocation
        {
            get { return m_AttendanceRecordWorkEndLocation; }
            set { m_AttendanceRecordWorkEndLocation = value; modify("AttendanceRecordWorkEndLocation"); }
        }
        protected int m_AttendanceRecordWorkEndTimeCardRecordID;
        [DBField("AttendanceRecordWorkEndTimeCardRecordID"), TextSearch, Export(false)]
        public int AttendanceRecordWorkEndTimeCardRecordID
        {
            get { return m_AttendanceRecordWorkEndTimeCardRecordID; }
            set { m_AttendanceRecordWorkEndTimeCardRecordID = value; modify("AttendanceRecordWorkEndTimeCardRecordID"); }
        }

        protected int m_AttendanceRecordCalculateLateMins;
        [DBField("AttendanceRecordCalculateLateMins"), TextSearch, MaxLength(4), Export(false)]
        public int AttendanceRecordCalculateLateMins
        {
            get { return m_AttendanceRecordCalculateLateMins; }
            set { m_AttendanceRecordCalculateLateMins = value; modify("AttendanceRecordCalculateLateMins"); }
        }
        protected int m_AttendanceRecordActualLateMins;
        [DBField("AttendanceRecordActualLateMins"), TextSearch, MaxLength(4), Export(false)]
        public int AttendanceRecordActualLateMins
        {
            get { return m_AttendanceRecordActualLateMins; }
            set { m_AttendanceRecordActualLateMins = value; modify("AttendanceRecordActualLateMins"); }
        }

        protected int m_AttendanceRecordWaivedLateMins;
        [DBField("AttendanceRecordWaivedLateMins"), TextSearch, MaxLength(4), Export(false)]
        public int AttendanceRecordWaivedLateMins
        {
            get { return m_AttendanceRecordWaivedLateMins; }
            set { m_AttendanceRecordWaivedLateMins = value; modify("AttendanceRecordWaivedLateMins"); }
        }

        protected int m_AttendanceRecordCalculateEarlyLeaveMins;
        [DBField("AttendanceRecordCalculateEarlyLeaveMins"), TextSearch, MaxLength(4), Export(false)]
        public int AttendanceRecordCalculateEarlyLeaveMins
        {
            get { return m_AttendanceRecordCalculateEarlyLeaveMins; }
            set { m_AttendanceRecordCalculateEarlyLeaveMins = value; modify("AttendanceRecordCalculateEarlyLeaveMins"); }
        }
        protected int m_AttendanceRecordActualEarlyLeaveMins;
        [DBField("AttendanceRecordActualEarlyLeaveMins"), TextSearch, MaxLength(4), Export(false)]
        public int AttendanceRecordActualEarlyLeaveMins
        {
            get { return m_AttendanceRecordActualEarlyLeaveMins; }
            set { m_AttendanceRecordActualEarlyLeaveMins = value; modify("AttendanceRecordActualEarlyLeaveMins"); }
        }

        protected int m_AttendanceRecordCalculateOvertimeMins;
        [DBField("AttendanceRecordCalculateOvertimeMins"), TextSearch, MaxLength(4), Export(false)]
        public int AttendanceRecordCalculateOvertimeMins
        {
            get { return m_AttendanceRecordCalculateOvertimeMins; }
            set { m_AttendanceRecordCalculateOvertimeMins = value; modify("AttendanceRecordCalculateOvertimeMins"); }
        }
        protected int m_AttendanceRecordActualOvertimeMins;
        [DBField("AttendanceRecordActualOvertimeMins"), TextSearch, MaxLength(4), Export(false)]
        public int AttendanceRecordActualOvertimeMins
        {
            get { return m_AttendanceRecordActualOvertimeMins; }
            set { m_AttendanceRecordActualOvertimeMins = value; modify("AttendanceRecordActualOvertimeMins"); }
        }

        protected int m_AttendanceRecordCalculateLunchOvertimeMins;
        [DBField("AttendanceRecordCalculateLunchOvertimeMins"), TextSearch, MaxLength(4), Export(false)]
        public int AttendanceRecordCalculateLunchOvertimeMins
        {
            get { return m_AttendanceRecordCalculateLunchOvertimeMins; }
            set { m_AttendanceRecordCalculateLunchOvertimeMins = value; modify("AttendanceRecordCalculateLunchOvertimeMins"); }
        }
        protected int m_AttendanceRecordActualLunchOvertimeMins;
        [DBField("AttendanceRecordActualLunchOvertimeMins"), TextSearch, MaxLength(4), Export(false)]
        public int AttendanceRecordActualLunchOvertimeMins
        {
            get { return m_AttendanceRecordActualLunchOvertimeMins; }
            set { m_AttendanceRecordActualLunchOvertimeMins = value; modify("AttendanceRecordActualLunchOvertimeMins"); }
        }

        protected double m_AttendanceRecordCalculateWorkingDay;
        [DBField("AttendanceRecordCalculateWorkingDay", "0.###"), TextSearch, MaxLength(6), Export(false)]
        public double AttendanceRecordCalculateWorkingDay
        {
            get { return m_AttendanceRecordCalculateWorkingDay; }
            set { m_AttendanceRecordCalculateWorkingDay = value; modify("AttendanceRecordCalculateWorkingDay"); }
        }
        protected double m_AttendanceRecordActualWorkingDay;
        [DBField("AttendanceRecordActualWorkingDay", "0.###"), TextSearch, MaxLength(6), Export(false)]
        public double AttendanceRecordActualWorkingDay
        {
            get { return m_AttendanceRecordActualWorkingDay; }
            set { m_AttendanceRecordActualWorkingDay = value; modify("AttendanceRecordActualWorkingDay"); }
        }

        protected double m_AttendanceRecordCalculateWorkingHour;
        [DBField("AttendanceRecordCalculateWorkingHour", "0.###"), TextSearch, MaxLength(6), Export(false)]
        public double AttendanceRecordCalculateWorkingHour
        {
            get { return m_AttendanceRecordCalculateWorkingHour; }
            set { m_AttendanceRecordCalculateWorkingHour = value; modify("AttendanceRecordCalculateWorkingHour"); }
        }
        protected double m_AttendanceRecordActualWorkingHour;
        [DBField("AttendanceRecordActualWorkingHour", "0.###"), TextSearch, MaxLength(6), Export(false)]
        public double AttendanceRecordActualWorkingHour
        {
            get { return m_AttendanceRecordActualWorkingHour; }
            set { m_AttendanceRecordActualWorkingHour = value; modify("AttendanceRecordActualWorkingHour"); }
        }

        protected int m_AttendanceRecordCalculateLunchTimeMins;
        [DBField("AttendanceRecordCalculateLunchTimeMins"), TextSearch, MaxLength(4), Export(false)]
        public int AttendanceRecordCalculateLunchTimeMins
        {
            get { return m_AttendanceRecordCalculateLunchTimeMins; }
            set { m_AttendanceRecordCalculateLunchTimeMins = value; modify("AttendanceRecordCalculateLunchTimeMins"); }
        }

        protected int m_AttendanceRecordActualLunchTimeMins;
        [DBField("AttendanceRecordActualLunchTimeMins"), TextSearch, MaxLength(4), Export(false)]
        public int AttendanceRecordActualLunchTimeMins
        {
            get { return m_AttendanceRecordActualLunchTimeMins; }
            set { m_AttendanceRecordActualLunchTimeMins = value; modify("AttendanceRecordActualLunchTimeMins"); }
        }

        protected bool m_AttendanceRecordIsAbsent;
        [DBField("AttendanceRecordIsAbsent"), TextSearch, Export(false)]
        public bool AttendanceRecordIsAbsent
        {
            get { return m_AttendanceRecordIsAbsent; }
            set { m_AttendanceRecordIsAbsent = value; modify("AttendanceRecordIsAbsent"); }
        }

        protected bool m_AttendanceRecordWorkOnRestDay;
        [DBField("AttendanceRecordWorkOnRestDay"), TextSearch, Export(false)]
        public bool AttendanceRecordWorkOnRestDay
        {
            get { return m_AttendanceRecordWorkOnRestDay; }
            set { m_AttendanceRecordWorkOnRestDay = value; modify("AttendanceRecordWorkOnRestDay"); }
        }
        
        protected string m_AttendanceRecordRemark;
        [DBField("AttendanceRecordRemark"), TextSearch, Export(false)]
        public string AttendanceRecordRemark
        {
            get { return m_AttendanceRecordRemark; }
            set { m_AttendanceRecordRemark = value; modify("AttendanceRecordRemark"); }
        }

        protected bool m_AttendanceRecordOverrideBonusEntitled;
        [DBField("AttendanceRecordOverrideBonusEntitled"), TextSearch, Export(false)]
        public bool AttendanceRecordOverrideBonusEntitled
        {
            get { return m_AttendanceRecordOverrideBonusEntitled; }
            set { m_AttendanceRecordOverrideBonusEntitled = value; modify("AttendanceRecordOverrideBonusEntitled"); }
        }

        protected bool m_AttendanceRecordHasBonus;
        [DBField("AttendanceRecordHasBonus"), TextSearch, Export(false)]
        public bool AttendanceRecordHasBonus
        {
            get { return m_AttendanceRecordHasBonus; }
            set { m_AttendanceRecordHasBonus = value; modify("AttendanceRecordHasBonus"); }
        }

        protected int m_AttendanceRecordCalculateLunchLateMins;
        [DBField("AttendanceRecordCalculateLunchLateMins"), TextSearch, MaxLength(4), Export(false)]
        public int AttendanceRecordCalculateLunchLateMins
        {
            get { return m_AttendanceRecordCalculateLunchLateMins; }
            set { m_AttendanceRecordCalculateLunchLateMins = value; modify("AttendanceRecordCalculateLunchLateMins"); }
        }

        protected int m_AttendanceRecordActualLunchLateMins;
        [DBField("AttendanceRecordActualLunchLateMins"), TextSearch, MaxLength(4), Export(false)]
        public int AttendanceRecordActualLunchLateMins
        {
            get { return m_AttendanceRecordActualLunchLateMins; }
            set { m_AttendanceRecordActualLunchLateMins = value; modify("AttendanceRecordActualLunchLateMins"); }
        }

        protected int m_AttendanceRecordCalculateLunchEarlyLeaveMins;
        [DBField("AttendanceRecordCalculateLunchEarlyLeaveMins"), TextSearch, MaxLength(4), Export(false)]
        public int AttendanceRecordCalculateLunchEarlyLeaveMins
        {
            get { return m_AttendanceRecordCalculateLunchEarlyLeaveMins; }
            set { m_AttendanceRecordCalculateLunchEarlyLeaveMins = value; modify("AttendanceRecordCalculateLunchEarlyLeaveMins"); }
        }

        protected int m_AttendanceRecordActualLunchEarlyLeaveMins;
        [DBField("AttendanceRecordActualLunchEarlyLeaveMins"), TextSearch, MaxLength(4), Export(false)]
        public int AttendanceRecordActualLunchEarlyLeaveMins
        {
            get { return m_AttendanceRecordActualLunchEarlyLeaveMins; }
            set { m_AttendanceRecordActualLunchEarlyLeaveMins = value; modify("AttendanceRecordActualLunchEarlyLeaveMins"); }
        }

        protected DateTime m_AttendanceRecordRosterCodeInTimeOverride;
        [DBField("AttendanceRecordRosterCodeInTimeOverride", "HH:mm"), TextSearch, MaxLength(5), Export(false)]
        public DateTime AttendanceRecordRosterCodeInTimeOverride
        {
            get { return m_AttendanceRecordRosterCodeInTimeOverride; }
            set { m_AttendanceRecordRosterCodeInTimeOverride = value; modify("AttendanceRecordRosterCodeInTimeOverride"); }
        }

        protected DateTime m_AttendanceRecordRosterCodeOutTimeOverride;
        [DBField("AttendanceRecordRosterCodeOutTimeOverride", "HH:mm"), TextSearch, MaxLength(5), Export(false)]
        public DateTime AttendanceRecordRosterCodeOutTimeOverride
        {
            get { return m_AttendanceRecordRosterCodeOutTimeOverride; }
            set { m_AttendanceRecordRosterCodeOutTimeOverride = value; modify("AttendanceRecordRosterCodeOutTimeOverride"); }
        }

        protected DateTime m_AttendanceRecordRosterCodeLunchStartTimeOverride;
        [DBField("AttendanceRecordRosterCodeLunchStartTimeOverride", "HH:mm"), TextSearch, MaxLength(5), Export(false)]
        public DateTime AttendanceRecordRosterCodeLunchStartTimeOverride
        {
            get { return m_AttendanceRecordRosterCodeLunchStartTimeOverride; }
            set { m_AttendanceRecordRosterCodeLunchStartTimeOverride = value; modify("AttendanceRecordRosterCodeLunchStartTimeOverride"); }
        }

        protected DateTime m_AttendanceRecordRosterCodeLunchEndTimeOverride;
        [DBField("AttendanceRecordRosterCodeLunchEndTimeOverride", "HH:mm"), TextSearch, MaxLength(5), Export(false)]
        public DateTime AttendanceRecordRosterCodeLunchEndTimeOverride
        {
            get { return m_AttendanceRecordRosterCodeLunchEndTimeOverride; }
            set { m_AttendanceRecordRosterCodeLunchEndTimeOverride = value; modify("AttendanceRecordRosterCodeLunchEndTimeOverride"); }
        }
        protected int m_LeaveAppID;
        [DBField("LeaveAppID"), TextSearch, Export(false)]
        public int LeaveAppID
        {
            get { return m_LeaveAppID; }
            set { m_LeaveAppID = value; modify("LeaveAppID"); }
        }

        public TimeSpan TotalWorkingHourTimeSpan(DatabaseConnection dbConn)
        {
            TimeSpan totalWorkHourTimeSpan = AttendanceRecordWorkEnd.Subtract(AttendanceRecordWorkStart);

            ERosterCode rosterCode = new ERosterCode();
            rosterCode.RosterCodeID = RosterCodeID;
            if (ERosterCode.db.select(dbConn, rosterCode))
            {
                if (rosterCode.RosterCodeHasLunch)
                    if (!AttendanceRecordLunchOut.Ticks.Equals(0) && !AttendanceRecordLunchIn.Ticks.Equals(0))
                    {
                        int lunchMins = Convert.ToInt32(AttendanceRecordLunchIn.Subtract(AttendanceRecordLunchOut).TotalMinutes);

                        totalWorkHourTimeSpan = totalWorkHourTimeSpan.Subtract(new TimeSpan(0, lunchMins, 0));
                    }
            }
            return totalWorkHourTimeSpan;
        }

        public bool IsChangeSite(DatabaseConnection dbConn)
        {
            ERosterCode rosterCode = new ERosterCode();
            rosterCode.RosterCodeID = m_RosterCodeID;
            if (ERosterCode.db.select(dbConn, rosterCode))
            {
                ERosterClientSite site = new ERosterClientSite();
                site.RosterClientSiteID = rosterCode.RosterClientSiteID;
                if (ERosterClientSite.db.select(dbConn, site))
                {
                    ERosterClient client = new ERosterClient();
                    client.RosterClientID = site.RosterClientID;
                    if (ERosterClient.db.select(dbConn, client))
                    {
                        EHierarchyLevel hLevel = new EHierarchyLevel();
                        hLevel.HLevelID = client.RosterClientMappingSiteCodeToHLevelID;
                        if (EHierarchyLevel.db.select(dbConn, hLevel))
                        {
                            EEmpPositionInfo empPos = AppUtils.GetLastPositionInfo(dbConn, m_AttendanceRecordDate, m_EmpID);
                            if (empPos != null)
                            {
                                DBFilter empHierarchyFilter = new DBFilter();
                                empHierarchyFilter.add(new Match("EmpPosID", empPos.EmpPosID));
                                empHierarchyFilter.add(new Match("HLevelID", hLevel.HLevelID));

                                System.Collections.ArrayList empHierarchyList = EEmpHierarchy.db.select(dbConn, empHierarchyFilter);
                                foreach (EEmpHierarchy empHierarchy in empHierarchyList)
                                {
                                    EHierarchyElement hElement = new EHierarchyElement();
                                    hElement.HElementID = empHierarchy.HElementID;
                                    if (EHierarchyElement.db.select(dbConn, hElement))
                                        if (!hElement.HElementCode.Trim().Equals(site.RosterClientSiteCode.Trim(), StringComparison.CurrentCultureIgnoreCase))
                                            return true;
                                }
                            }
                        }
                    }
                }
            }
            return false;
        }

        protected string m_AttendanceRecordExtendData;
        [DBField("AttendanceRecordExtendData"), TextSearch, Export(false)]
        public string AttendanceRecordExtendData
        {
            get { return m_AttendanceRecordExtendData; }
            set { m_AttendanceRecordExtendData = value; modify("AttendanceRecordExtendData"); }
        }

        public string GetAttendanceRecordExtendData(string FieldName)
        {
            System.Xml.XmlNodeList node = Utility.GetXmlDocumentByDataString(AttendanceRecordExtendData).GetElementsByTagName(FieldName);
            if (node.Count > 0)
                return node[0].InnerText;
            else
                return string.Empty;

        }

        public void SetAttendanceRecordExtendData(string FieldName, string Value)
        {
            System.Xml.XmlDocument xmlDoc = new System.Xml.XmlDocument();
            if (string.IsNullOrEmpty(AttendanceRecordExtendData))
            {
                System.Xml.XmlElement rootNode = xmlDoc.CreateElement("AttendanceRecordExtendData");
                xmlDoc.AppendChild(rootNode);

            }
            else
                xmlDoc.LoadXml(AttendanceRecordExtendData);

            if (!Value.Equals(string.Empty))
            {

                System.Xml.XmlElement xmlElement = xmlDoc.CreateElement(FieldName);
                xmlElement.InnerText = Value.Trim();
                xmlDoc.DocumentElement.AppendChild(xmlElement);
                AttendanceRecordExtendData = xmlDoc.InnerXml;
            }
        }

        protected override void beforeInsert(DatabaseConnection dbConn, DBManager db)
        {
            base.beforeInsert(dbConn, db);
            //  trigger on beforeInsert for updating AttendanceRecord.LeaveAppID before saving to database
            CreateLeaveApplication(dbConn, this);

        }

        protected override void beforeUpdate(DatabaseConnection dbConn, DBManager db)
        {
            base.beforeUpdate(dbConn, db);
            //  trigger on beforeUpdate for updating AttendanceRecord.LeaveAppID before saving to database
            if (oldValueObject != null)
            {
                EAttendanceRecord oldAttendanceRecord = (EAttendanceRecord)oldValueObject;
                if (oldAttendanceRecord.RosterCodeID != this.RosterCodeID)
                {
                    DeleteLeaveApplication(dbConn, oldAttendanceRecord);
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

            //  Delete leave application only if roster table is not mapped to this leave application
            if (this.LeaveAppID > 0)
            {
                DBFilter filter = new DBFilter();
                filter.add(new Match("LeaveAppID", this.LeaveAppID));
                if (ERosterTable.db.count(dbConn, filter) <= 0)
                    DeleteLeaveApplication(dbConn, this);
            }

        }

        private static void CreateLeaveApplication(DatabaseConnection dbConn, EAttendanceRecord attendanceRecord)
        {
            if (attendanceRecord.LeaveAppID > 0)
            {
                ELeaveApplication leaveApp = new ELeaveApplication();
                leaveApp.LeaveAppID = attendanceRecord.LeaveAppID;
                if (!ELeaveApplication.db.select(dbConn, leaveApp))
                    attendanceRecord.LeaveAppID = 0;
            }

            //  Check  LeaveAppID <= 0 To prevent override LeaveAppID created by external function
            if (attendanceRecord.RosterCodeID > 0 && attendanceRecord.LeaveAppID <= 0)
            {
                ERosterCode rosterCode = new ERosterCode();
                rosterCode.RosterCodeID = attendanceRecord.RosterCodeID;
                if (ERosterCode.db.select(dbConn, rosterCode))
                {
                    if (rosterCode.RosterCodeType.Equals(ERosterCode.ROSTERTYPE_CODE_LEAVE) && rosterCode.LeaveCodeID > 0)
                    {
                        ELeaveCode leaveCode = new ELeaveCode();
                        leaveCode.LeaveCodeID = rosterCode.LeaveCodeID;
                        if (ELeaveCode.db.select(dbConn, leaveCode))
                        {
                            ELeaveApplication leaveApp = ELeaveApplication.GetOrCreateSingleDayLeaveApplicationObject(dbConn, attendanceRecord.EmpID, rosterCode.LeaveCodeID, attendanceRecord.AttendanceRecordDate);

                            if (leaveApp != null)
                            {
                                if (leaveApp.LeaveAppID <= 0)
                                {
                                    ELeaveApplication.db.insert(dbConn, leaveApp);
                                    attendanceRecord.LeaveAppID = leaveApp.LeaveAppID;
                                }
                            }
                        }
                    }
                    else if (rosterCode.RosterCodeType.Equals(ERosterCode.ROSTERTYPE_CODE_RESTDAY))
                        ELeaveBalance.DeleteLeaveBalanceAfter(dbConn, attendanceRecord.EmpID, ELeaveType.RESTDAY_LEAVE_TYPE(dbConn).LeaveTypeID, attendanceRecord.AttendanceRecordDate);
                    else if (rosterCode.RosterCodeType.Equals(ERosterCode.ROSTERTYPE_CODE_STATUTORYHOLIDAY))
                        ELeaveBalance.DeleteLeaveBalanceAfter(dbConn, attendanceRecord.EmpID, ELeaveType.STATUTORYHOLIDAY_LEAVE_TYPE(dbConn).LeaveTypeID, attendanceRecord.AttendanceRecordDate);
                    else if (rosterCode.RosterCodeType.Equals(ERosterCode.ROSTERTYPE_CODE_PUBLICHOLIDAY))
                        ELeaveBalance.DeleteLeaveBalanceAfter(dbConn, attendanceRecord.EmpID, ELeaveType.PUBLICHOLIDAY_LEAVE_TYPE(dbConn).LeaveTypeID, attendanceRecord.AttendanceRecordDate);
                }
            }
        }

        private static void DeleteLeaveApplication(DatabaseConnection dbConn, EAttendanceRecord attendanceRecord)
        {

            if (attendanceRecord.LeaveAppID > 0)
            {
                ERosterCode rosterCode = new ERosterCode();
                rosterCode.RosterCodeID = attendanceRecord.RosterCodeID;
                if (ERosterCode.db.select(dbConn, rosterCode))
                {

                    ELeaveApplication leaveApp = new ELeaveApplication();
                    leaveApp.LeaveAppID = attendanceRecord.LeaveAppID;
                    if (ELeaveApplication.db.select(dbConn, leaveApp))
                        if (leaveApp.EmpPayrollID <= 0
                            && rosterCode.LeaveCodeID.Equals(leaveApp.LeaveCodeID)
                            && attendanceRecord.AttendanceRecordDate.Equals(leaveApp.LeaveAppDateFrom)
                            && attendanceRecord.AttendanceRecordDate.Equals(leaveApp.LeaveAppDateTo)
                            )
                        {
                            //ELeaveCode leaveCode = new ELeaveCode();
                            //leaveCode.LeaveCodeID = leaveApp.LeaveCodeID;
                            //if (ELeaveCode.db.select(dbConn, leaveCode))
                                ELeaveApplication.db.delete(dbConn, leaveApp);
                        }
                }
            }
            if (attendanceRecord.RosterCodeID > 0 && attendanceRecord.LeaveAppID <= 0)
            {
                ERosterCode rosterCode = new ERosterCode();
                rosterCode.RosterCodeID = attendanceRecord.RosterCodeID;
                if (ERosterCode.db.select(dbConn, rosterCode))
                {
                    if (rosterCode.RosterCodeType.Equals(ERosterCode.ROSTERTYPE_CODE_RESTDAY))
                        ELeaveBalance.DeleteLeaveBalanceAfter(dbConn, attendanceRecord.EmpID, ELeaveType.RESTDAY_LEAVE_TYPE(dbConn).LeaveTypeID, attendanceRecord.AttendanceRecordDate);
                    else if (rosterCode.RosterCodeType.Equals(ERosterCode.ROSTERTYPE_CODE_STATUTORYHOLIDAY))
                        ELeaveBalance.DeleteLeaveBalanceAfter(dbConn, attendanceRecord.EmpID, ELeaveType.STATUTORYHOLIDAY_LEAVE_TYPE(dbConn).LeaveTypeID, attendanceRecord.AttendanceRecordDate);
                    else if (rosterCode.RosterCodeType.Equals(ERosterCode.ROSTERTYPE_CODE_PUBLICHOLIDAY))
                        ELeaveBalance.DeleteLeaveBalanceAfter(dbConn, attendanceRecord.EmpID, ELeaveType.PUBLICHOLIDAY_LEAVE_TYPE(dbConn).LeaveTypeID, attendanceRecord.AttendanceRecordDate);
                }
            }
        }
    }


}
