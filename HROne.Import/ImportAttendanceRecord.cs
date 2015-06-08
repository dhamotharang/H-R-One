using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using HROne.Lib.Entities;
using HROne.Import;
using HROne.DataAccess;
//using perspectivemind.validation;
using HROne.CommonLib;

namespace HROne.Import
{
    [DBClass("UploadAttendanceRecord")]
    public class EUploadAttendanceRecord : ImportDBObject
    {
        public static DBManager db = new DBManager(typeof(EUploadAttendanceRecord));

        protected int m_UploadAttendanceRecordID;
        [DBField("UploadAttendanceRecordID", true, true), TextSearch, Export(false)]
        public int UploadAttendanceRecordID
        {
            get { return m_UploadAttendanceRecordID; }
            set { m_UploadAttendanceRecordID = value; modify("UploadAttendanceRecordID"); }
        }

        protected int m_AttendanceRecordID;
        [DBField("AttendanceRecordID"), TextSearch, Export(false)]
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
        //protected int m_AttendanceRecordCalculateLateMins;
        //[DBField("AttendanceRecordCalculateLateMins"), TextSearch, MaxLength(4), Export(false)]
        //public int AttendanceRecordCalculateLateMins
        //{
        //    get { return m_AttendanceRecordCalculateLateMins; }
        //    set { m_AttendanceRecordCalculateLateMins = value; modify("AttendanceRecordCalculateLateMins"); }
        //}
        protected int m_AttendanceRecordActualLateMins;
        [DBField("AttendanceRecordActualLateMins"), TextSearch, MaxLength(4), Export(false)]
        public int AttendanceRecordActualLateMins
        {
            get { return m_AttendanceRecordActualLateMins; }
            set { m_AttendanceRecordActualLateMins = value; modify("AttendanceRecordActualLateMins"); }
        }

        //protected int m_AttendanceRecordCalculateEarlyLeaveMins;
        //[DBField("AttendanceRecordCalculateEarlyLeaveMins"), TextSearch, MaxLength(4), Export(false)]
        //public int AttendanceRecordCalculateEarlyLeaveMins
        //{
        //    get { return m_AttendanceRecordCalculateEarlyLeaveMins; }
        //    set { m_AttendanceRecordCalculateEarlyLeaveMins = value; modify("AttendanceRecordCalculateEarlyLeaveMins"); }
        //}
        protected int m_AttendanceRecordActualEarlyLeaveMins;
        [DBField("AttendanceRecordActualEarlyLeaveMins"), TextSearch, MaxLength(4), Export(false)]
        public int AttendanceRecordActualEarlyLeaveMins
        {
            get { return m_AttendanceRecordActualEarlyLeaveMins; }
            set { m_AttendanceRecordActualEarlyLeaveMins = value; modify("AttendanceRecordActualEarlyLeaveMins"); }
        }

        //protected int m_AttendanceRecordCalculateOvertimeMins;
        //[DBField("AttendanceRecordCalculateOvertimeMins"), TextSearch, MaxLength(4), Export(false)]
        //public int AttendanceRecordCalculateOvertimeMins
        //{
        //    get { return m_AttendanceRecordCalculateOvertimeMins; }
        //    set { m_AttendanceRecordCalculateOvertimeMins = value; modify("AttendanceRecordCalculateOvertimeMins"); }
        //}
        protected int m_AttendanceRecordActualOvertimeMins;
        [DBField("AttendanceRecordActualOvertimeMins"), TextSearch, MaxLength(4), Export(false)]
        public int AttendanceRecordActualOvertimeMins
        {
            get { return m_AttendanceRecordActualOvertimeMins; }
            set { m_AttendanceRecordActualOvertimeMins = value; modify("AttendanceRecordActualOvertimeMins"); }
        }
        protected int m_AttendanceRecordActualLunchOvertimeMins;
        [DBField("AttendanceRecordActualLunchOvertimeMins"), TextSearch, MaxLength(4), Export(false)]
        public int AttendanceRecordActualLunchOvertimeMins
        {
            get { return m_AttendanceRecordActualLunchOvertimeMins; }
            set { m_AttendanceRecordActualLunchOvertimeMins = value; modify("AttendanceRecordActualLunchOvertimeMins"); }
        }
        
        //protected int m_AttendanceRecordCalculateLunchTimeMins;
        //[DBField("AttendanceRecordCalculateLunchTimeMins"), TextSearch, MaxLength(4), Export(false)]
        //public int AttendanceRecordCalculateLunchTimeMins
        //{
        //    get { return m_AttendanceRecordCalculateLunchTimeMins; }
        //    set { m_AttendanceRecordCalculateLunchTimeMins = value; modify("AttendanceRecordCalculateLunchTimeMins"); }
        //}
        protected int m_AttendanceRecordActualLunchTimeMins;
        [DBField("AttendanceRecordActualLunchTimeMins"), TextSearch, MaxLength(4), Export(false)]
        public int AttendanceRecordActualLunchTimeMins
        {
            get { return m_AttendanceRecordActualLunchTimeMins; }
            set { m_AttendanceRecordActualLunchTimeMins = value; modify("AttendanceRecordActualLunchTimeMins"); }
        }

        //protected double m_AttendanceRecordCalculateWorkingDay;
        //[DBField("AttendanceRecordCalculateWorkingDay", "#,##0.000"), TextSearch, MaxLength(7), Export(false)]
        //public double AttendanceRecordCalculateWorkingDay
        //{
        //    get { return m_AttendanceRecordCalculateWorkingDay; }
        //    set { m_AttendanceRecordCalculateWorkingDay = value; modify("AttendanceRecordCalculateWorkingDay"); }
        //}
        protected double m_AttendanceRecordActualWorkingDay;
        [DBField("AttendanceRecordActualWorkingDay", "#,##0.000"), TextSearch, MaxLength(7), Export(false)]
        public double AttendanceRecordActualWorkingDay
        {
            get { return m_AttendanceRecordActualWorkingDay; }
            set { m_AttendanceRecordActualWorkingDay = value; modify("AttendanceRecordActualWorkingDay"); }
        }

        //protected double m_AttendanceRecordCalculateWorkingHour;
        //[DBField("AttendanceRecordCalculateWorkingHour", "#,##0.000"), TextSearch, MaxLength(8), Export(false)]
        //public double AttendanceRecordCalculateWorkingHour
        //{
        //    get { return m_AttendanceRecordCalculateWorkingHour; }
        //    set { m_AttendanceRecordCalculateWorkingHour = value; modify("AttendanceRecordCalculateWorkingHour"); }
        //}
        protected double m_AttendanceRecordActualWorkingHour;
        [DBField("AttendanceRecordActualWorkingHour", "#,##0.000"), TextSearch, MaxLength(8), Export(false)]
        public double AttendanceRecordActualWorkingHour
        {
            get { return m_AttendanceRecordActualWorkingHour; }
            set { m_AttendanceRecordActualWorkingHour = value; modify("AttendanceRecordActualWorkingHour"); }
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

        //protected int m_AttendanceRecordCalculateLunchLateMins;
        //[DBField("AttendanceRecordCalculateLunchLateMins"), TextSearch, MaxLength(4), Export(false)]
        //public int AttendanceRecordCalculateLunchLateMins
        //{
        //    get { return m_AttendanceRecordCalculateLunchLateMins; }
        //    set { m_AttendanceRecordCalculateLunchLateMins = value; modify("AttendanceRecordCalculateLunchLateMins"); }
        //}

        protected int m_AttendanceRecordActualLunchLateMins;
        [DBField("AttendanceRecordActualLunchLateMins"), TextSearch, MaxLength(4), Export(false)]
        public int AttendanceRecordActualLunchLateMins
        {
            get { return m_AttendanceRecordActualLunchLateMins; }
            set { m_AttendanceRecordActualLunchLateMins = value; modify("AttendanceRecordActualLunchLateMins"); }
        }

        //protected int m_AttendanceRecordCalculateLunchEarlyLeaveMins;
        //[DBField("AttendanceRecordCalculateLunchEarlyLeaveMins"), TextSearch, MaxLength(4), Export(false)]
        //public int AttendanceRecordCalculateLunchEarlyLeaveMins
        //{
        //    get { return m_AttendanceRecordCalculateLunchEarlyLeaveMins; }
        //    set { m_AttendanceRecordCalculateLunchEarlyLeaveMins = value; modify("AttendanceRecordCalculateLunchEarlyLeaveMins"); }
        //}

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
    }


    /// <summary>
    /// Summary description for ImportRosterTable
    /// </summary>
    public class ImportAttendanceRecordProcess : ImportProcessInteface
    {

        
        private DateTime UploadDateTime = AppUtils.ServerDateTime();
        private DBManager tempDB = EUploadAttendanceRecord.db;
        private DBManager uploadDB = EAttendanceRecord.db;



        public ImportErrorList errors = new ImportErrorList();
        private bool m_IsRecalculate = false;
        public ImportAttendanceRecordProcess(DatabaseConnection dbConn, string SessionID, bool IsRecalculate)
            : base(dbConn, SessionID)
        {
            m_IsRecalculate = IsRecalculate;
        }
        public const string TABLE_NAME = "AttendanceRecord";

        protected const string FIELD_EMP_NO = "Emp No";
        protected const string FIELD_ROSTERCODE = "Roster Code";
        protected const string FIELD_DATE = "Date";
        protected const string FIELD_ROSTERCODE_OVERRIDE_WORKIN = "Override Roster In";
        protected const string FIELD_ROSTERCODE_OVERRIDE_LUNCHOUT = "Override Roster Meal Break Start";
        protected const string FIELD_ROSTERCODE_OVERRIDE_LUNCHIN = "Override Roster Meal Break End";
        protected const string FIELD_ROSTERCODE_OVERRIDE_WORKOUT = "Override Roster Out";
        public const string FIELD_WORKIN = "In";
        public const string FIELD_WORKIN_LOCATION = "In Location";
        public const string FIELD_WORKIN_TIMECARDID = "AttendanceRecordWorkStartTimeCardRecordID";
        public const string FIELD_LUNCHOUT = "Meal Break Out";
        public const string FIELD_LUNCHOUT_LOCATION = "Meal Break Out Location";
        public const string FIELD_LUNCHOUT_TIMECARDID = "AttendanceRecordLunchOutTimeCardRecordID";
        public const string FIELD_LUNCHIN = "Meal Break In";
        public const string FIELD_LUNCHIN_LOCATION = "Meal Break In Location";
        public const string FIELD_LUNCHIN_TIMECARDID = "AttendanceRecordLunchInTimeCardRecordID";
        public const string FIELD_WORKOUT = "Out";
        public const string FIELD_WORKOUT_LOCATION = "Out Location";
        public const string FIELD_WORKOUT_TIMECARDID = "AttendanceRecordWorkEndTimeCardRecordID";
        protected const string FIELD_LATEMINS = "Late (mins)";
        protected const string FIELD_EARLYLEAVEMINS = "Early Leave (mins)";
        protected const string FIELD_LUNCH_LATEMINS = "Meal Break Late (mins)";
        protected const string FIELD_LUNCH_EARLYLEAVEMINS = "Meal Break Early Leave (mins)";
        protected const string FIELD_OVERTIMEMINS = "Overtime (mins)";
        protected const string FIELD_LUNCH_OVERTIMEMINS = "Meal Break Overtime (mins)";
        protected const string FIELD_LUNCHTIMEMINS = "Meal Break Time (mins)";
        protected const string FIELD_ISABSENT = "Absent";
        protected const string FIELD_WORKINGDAY = "Working Day";
        protected const string FIELD_WORKINGHOUR = "Working Hour";
        protected const string FIELD_REMARK = "Remark";
        protected const string FIELD_HAS_BONUS = "Has Bonus";
        protected const string FIELD_EXTENDDATA_OVERRIDE_DAILY_PAYMENT = "Override Daily Payment";
        protected const string FIELD_WORK_ON_RESTDAY = "Work on Rest Day";
        protected const string FIELD_EXTENDDATA_WORK_AS_OVERTIME = "Work as OT (Use OT rate)";

        public DataTable UploadToTempDatabase(DataTable rawDataTable, int UserID)
        {
            if (rawDataTable == null)
                return GetImportDataFromTempDatabase(null);

            int rowCount = 1;
            foreach (DataRow row in rawDataTable.Rows)
            {
                rowCount++;

                EUploadAttendanceRecord uploadAttendanceRecord = ConvertFromDataRow(row, rowCount, UserID, errors);

                uploadAttendanceRecord.AttendanceRecordIsAbsent = row[FIELD_ISABSENT].ToString().Equals("Yes", StringComparison.CurrentCultureIgnoreCase);
                uploadAttendanceRecord.AttendanceRecordRemark = row[FIELD_REMARK].ToString();

                if (rawDataTable.Columns.Contains(FIELD_HAS_BONUS))
                {
                    string hasBonusFlag = row[FIELD_HAS_BONUS].ToString().Trim();
                    if (string.IsNullOrEmpty(hasBonusFlag))
                    {
                        uploadAttendanceRecord.AttendanceRecordOverrideBonusEntitled = false;
                        uploadAttendanceRecord.AttendanceRecordHasBonus = false;
                    }
                    else
                    {
                        uploadAttendanceRecord.AttendanceRecordOverrideBonusEntitled = true;
                        if (hasBonusFlag.Equals("Y", StringComparison.CurrentCultureIgnoreCase)
                            || hasBonusFlag.Equals("Yes", StringComparison.CurrentCultureIgnoreCase))
                            uploadAttendanceRecord.AttendanceRecordHasBonus = true;
                        else
                            uploadAttendanceRecord.AttendanceRecordHasBonus = false;

                    }
                }
                else
                {
                    uploadAttendanceRecord.AttendanceRecordOverrideBonusEntitled = false;
                    uploadAttendanceRecord.AttendanceRecordHasBonus = false;
                }

                uploadAttendanceRecord.SessionID = m_SessionID;
                uploadAttendanceRecord.TransactionDate = UploadDateTime;

                ArrayList additionUploadRecordList = new ArrayList();

                if (uploadAttendanceRecord.EmpID != 0)
                {
                    DBFilter dbFilter = new DBFilter();
                    dbFilter.add(new Match("EmpID", uploadAttendanceRecord.EmpID));
                    dbFilter.add(new Match("AttendanceRecordDate", uploadAttendanceRecord.AttendanceRecordDate));

                    DBFilter uploadAttendanceRecordFilter = new DBFilter();
                    uploadAttendanceRecordFilter.add(new Match("uar.SessionID", m_SessionID));
                    dbFilter.add(new IN("NOT AttendanceRecordID", "SELECT uar.AttendanceRecordID FROM " + EUploadAttendanceRecord.db.dbclass.tableName + " uar", uploadAttendanceRecordFilter));

                    ArrayList list = EAttendanceRecord.db.select(dbConn, dbFilter);
                    foreach (EAttendanceRecord attendanceRecord in list)
                    {
                        if (uploadAttendanceRecord.AttendanceRecordID.Equals(0))
                        {
                            uploadAttendanceRecord.AttendanceRecordID = attendanceRecord.AttendanceRecordID;

                            //uploadAttendanceRecord.AttendanceRecordCalculateEarlyLeaveMins = attendanceRecord.AttendanceRecordCalculateEarlyLeaveMins;
                            //uploadAttendanceRecord.AttendanceRecordCalculateLateMins = attendanceRecord.AttendanceRecordCalculateLateMins;
                            //uploadAttendanceRecord.AttendanceRecordCalculateLunchEarlyLeaveMins = attendanceRecord.AttendanceRecordCalculateLunchEarlyLeaveMins;
                            //uploadAttendanceRecord.AttendanceRecordCalculateLunchLateMins = attendanceRecord.AttendanceRecordCalculateLunchLateMins;
                            //uploadAttendanceRecord.AttendanceRecordCalculateOvertimeMins = attendanceRecord.AttendanceRecordCalculateOvertimeMins;
                            //uploadAttendanceRecord.AttendanceRecordCalculateLunchTimeMins = attendanceRecord.AttendanceRecordCalculateLunchTimeMins;
                            //uploadAttendanceRecord.AttendanceRecordCalculateWorkingDay = attendanceRecord.AttendanceRecordCalculateWorkingDay;
                            //uploadAttendanceRecord.AttendanceRecordCalculateWorkingHour = attendanceRecord.AttendanceRecordCalculateWorkingHour;
                            //uploadAttendanceRecord.RosterCodeID = attendanceRecord.RosterCodeID;
                            uploadAttendanceRecord.RosterTableID = attendanceRecord.RosterTableID;

                        }
                        //else
                        //{
                        //    //  Set Next Existing record to 0;
                        //    EUploadAttendanceRecord uploadNextAttendanceRecord = new EUploadAttendanceRecord();
                        //    ImportDBObject.CopyObjectProperties(attendanceRecord, uploadNextAttendanceRecord);

                        //    uploadNextAttendanceRecord.AttendanceRecordWorkStart = new DateTime();
                        //    uploadNextAttendanceRecord.AttendanceRecordLunchOut = new DateTime();
                        //    uploadNextAttendanceRecord.AttendanceRecordLunchIn = new DateTime();
                        //    uploadNextAttendanceRecord.AttendanceRecordWorkEnd = new DateTime();
                        //    uploadNextAttendanceRecord.AttendanceRecordActualLateMins = 0;
                        //    uploadNextAttendanceRecord.AttendanceRecordActualEarlyLeaveMins = 0;

                        //    uploadNextAttendanceRecord.AttendanceRecordActualOvertimeMins = 0;
                        //    uploadNextAttendanceRecord.AttendanceRecordActualLunchTimeMins = 0;

                        //    uploadNextAttendanceRecord.AttendanceRecordActualWorkingDay = 0;
                        //    uploadNextAttendanceRecord.AttendanceRecordActualWorkingHour = 0;

                        //    uploadNextAttendanceRecord.AttendanceRecordIsAbsent = false;
                        //    uploadNextAttendanceRecord.AttendanceRecordRemark = string.Empty;

                        //    uploadNextAttendanceRecord.SessionID = m_SessionID;
                        //    uploadNextAttendanceRecord.TransactionDate = UploadDateTime;
                        //    uploadNextAttendanceRecord.ImportActionStatus = ImportDBObject.ImportActionEnum.DELETE;

                        //    additionUploadRecordList.Add(uploadNextAttendanceRecord);
                        //}
                    }
                }


                if (uploadAttendanceRecord.AttendanceRecordID <= 0)
                    uploadAttendanceRecord.ImportActionStatus = ImportDBObject.ImportActionEnum.INSERT;
                else
                    uploadAttendanceRecord.ImportActionStatus = ImportDBObject.ImportActionEnum.UPDATE;


                Hashtable values = new Hashtable();
                tempDB.populate(uploadAttendanceRecord, values);
                PageErrors pageErrors = new PageErrors(EUploadAttendanceRecord.db);
                tempDB.validate(pageErrors, values);
                if (pageErrors.errors.Count == 0)
                {

                    tempDB.insert(dbConn, uploadAttendanceRecord);
                    foreach (EUploadAttendanceRecord uploadNextAttendanceRecord in additionUploadRecordList)
                    {
                        tempDB.insert(dbConn, uploadNextAttendanceRecord);
                    }
                }
                else
                {
                    pageErrors.addError(rawDataTable.TableName);
                    throw new HRImportException(pageErrors.getPrompt() + "(line " + rowCount.ToString() + ")");

                    //if (EmpID == 0)
                    //    errors.addError(HROne.Translation.PageErrorMessage.ERROR_INVALID_EMP_NO, new string[] { EmpNo, rowCount.ToString() });
                    //else if (PayCodeID == 0)
                    //    errors.addError(HROne.Translation.PageErrorMessage.ERROR_INVALID_FIELD_VALUE, new string[] { FIELD_PAYMENT_CODE + "=" + PaymentCode, EmpNo, rowCount.ToString() });
                    //else if (PayCodeID == 0)
                    //    errors.addError(HROne.Translation.PageErrorMessage.ERROR_INVALID_FIELD_VALUE, new string[] { FIELD_PAYMENT_CODE + "=" + PaymentCode, EmpNo, rowCount.ToString() });
                    //else if (EffDate.Ticks == 0)
                    //    errors.addError(HROne.Translation.PageErrorMessage.ERROR_INVALID_FIELD_VALUE, new string[] { FIELD_EFFECTIVE_DATE + "=" + EffDateString, EmpNo, rowCount.ToString() });
                    //else if (double.TryParse(amountString, out amount))
                    //    errors.addError(HROne.Translation.PageErrorMessage.ERROR_INVALID_FIELD_VALUE, new string[] { FIELD_AMOUNT + "=" + amountString, EmpNo, rowCount.ToString() });
                }

            }
            if (errors.List.Count > 0)
            {
                ClearTempTable();
                throw (new HRImportException(rawDataTable.TableName + "\r\n" + errors.Message()));
            }
            return GetImportDataFromTempDatabase(null);
        }

        public override DataTable UploadToTempDatabase(string Filename, int UserID, string ZipPassword)
        {
            DataSet rawDataSet = HROne.Import.ExcelImport.parse(Filename, ZipPassword, FIELD_EMP_NO);

            ClearTempTable();
            try
            {
                foreach (DataTable rawDataTable in rawDataSet.Tables)
                {
                    if (rawDataTable.TableName.StartsWith(TABLE_NAME, StringComparison.CurrentCultureIgnoreCase))
                        UploadToTempDatabase(rawDataTable, UserID);
                }
            }
            catch (HRImportException he)
            {
                ClearTempTable();
                throw he;
            }
            return GetImportDataFromTempDatabase(null);
        }

        protected virtual EUploadAttendanceRecord ConvertFromDataRow(DataRow row, int rowCount, int UserID, ImportErrorList errors)
        {
            DataTable rawDataTable = row.Table;

            EUploadAttendanceRecord uploadAttendanceRecord = new EUploadAttendanceRecord();
            //EAttendanceRecord lastAttendanceRecord = null;

            string EmpNo = row[FIELD_EMP_NO].ToString().Trim();
            uploadAttendanceRecord.EmpID = Parse.GetEmpID(dbConn, EmpNo, UserID);
            if (uploadAttendanceRecord.EmpID < 0)
                errors.addError(ImportErrorMessage.ERROR_ACCESS_DENIED_EMP_NO, new string[] { EmpNo, rowCount.ToString() });
            else if (uploadAttendanceRecord.EmpID == 0)
                errors.addError(ImportErrorMessage.ERROR_INVALID_EMP_NO, new string[] { EmpNo, rowCount.ToString() });
            uploadAttendanceRecord.RosterCodeID = Parse.GetRosterCodeID(dbConn, row[FIELD_ROSTERCODE].ToString());

            uploadAttendanceRecord.AttendanceRecordDate = Parse.toDateTimeObject(row[FIELD_DATE]);

            if (rawDataTable.Columns.Contains(FIELD_ROSTERCODE_OVERRIDE_WORKIN))
            {
                string tmpString = row[FIELD_ROSTERCODE_OVERRIDE_WORKIN].ToString();
                if (!tmpString.Equals(string.Empty))
                    if (tmpString.Equals(ImportDBObject.FIELD_VALUE_OVERRIDE_EMPTY, StringComparison.CurrentCultureIgnoreCase))
                        uploadAttendanceRecord.AttendanceRecordRosterCodeInTimeOverride = new DateTime();
                    else
                        uploadAttendanceRecord.AttendanceRecordRosterCodeInTimeOverride = Parse.toDateTimeObject(row[FIELD_ROSTERCODE_OVERRIDE_WORKIN]);
            }
            if (rawDataTable.Columns.Contains(FIELD_ROSTERCODE_OVERRIDE_LUNCHOUT))
            {
                string tmpString = row[FIELD_ROSTERCODE_OVERRIDE_LUNCHOUT].ToString();
                if (!tmpString.Equals(string.Empty))
                    if (tmpString.Equals(ImportDBObject.FIELD_VALUE_OVERRIDE_EMPTY, StringComparison.CurrentCultureIgnoreCase))
                        uploadAttendanceRecord.AttendanceRecordRosterCodeLunchStartTimeOverride = new DateTime();
                    else
                        uploadAttendanceRecord.AttendanceRecordRosterCodeLunchStartTimeOverride = Parse.toDateTimeObject(row[FIELD_ROSTERCODE_OVERRIDE_LUNCHOUT]);
            }
            if (rawDataTable.Columns.Contains(FIELD_ROSTERCODE_OVERRIDE_LUNCHIN))
            {
                string tmpString = row[FIELD_ROSTERCODE_OVERRIDE_LUNCHIN].ToString();
                if (!tmpString.Equals(string.Empty))
                    if (tmpString.Equals(ImportDBObject.FIELD_VALUE_OVERRIDE_EMPTY, StringComparison.CurrentCultureIgnoreCase))
                        uploadAttendanceRecord.AttendanceRecordRosterCodeLunchEndTimeOverride = new DateTime();
                    else
                        uploadAttendanceRecord.AttendanceRecordRosterCodeLunchEndTimeOverride = Parse.toDateTimeObject(row[FIELD_ROSTERCODE_OVERRIDE_LUNCHIN]);
            }
            if (rawDataTable.Columns.Contains(FIELD_ROSTERCODE_OVERRIDE_WORKOUT))
            {
                string tmpString = row[FIELD_ROSTERCODE_OVERRIDE_WORKOUT].ToString();
                if (!tmpString.Equals(string.Empty))
                    if (tmpString.Equals(ImportDBObject.FIELD_VALUE_OVERRIDE_EMPTY, StringComparison.CurrentCultureIgnoreCase))
                        uploadAttendanceRecord.AttendanceRecordRosterCodeOutTimeOverride = new DateTime();
                    else
                        uploadAttendanceRecord.AttendanceRecordRosterCodeOutTimeOverride = Parse.toDateTimeObject(row[FIELD_ROSTERCODE_OVERRIDE_WORKOUT]);
            }

            //DateTime dateTimeStart = uploadAttendanceRecord.AttendanceRecordDate;
            //if (uploadAttendanceRecord.RosterCodeID > 0)
            //{
            //    ERosterCode rosterCode = new ERosterCode();
            //    rosterCode.RosterCodeID = uploadAttendanceRecord.RosterCodeID;
            //    if (ERosterCode.db.select(dbConn, rosterCode))
            //    {
            //        dateTimeStart = uploadAttendanceRecord.AttendanceRecordDate.AddHours(rosterCode.RosterCodeInTime.Hour).AddMinutes(rosterCode.RosterCodeInTime.Minute);
            //    }
            //}
            if (rawDataTable.Columns.Contains(FIELD_WORKIN))
            {
                string tmpString = row[FIELD_WORKIN].ToString();
                if (!tmpString.Equals(string.Empty))
                    if (tmpString.Equals(ImportDBObject.FIELD_VALUE_OVERRIDE_EMPTY, StringComparison.CurrentCultureIgnoreCase))
                        uploadAttendanceRecord.AttendanceRecordWorkStart = new DateTime();
                    else
                        uploadAttendanceRecord.AttendanceRecordWorkStart = Parse.toDateTimeObject(row[FIELD_WORKIN]);
            }
            //if (!uploadAttendanceRecord.AttendanceRecordWorkStart.Ticks.Equals(0))
            //{
            //    uploadAttendanceRecord.AttendanceRecordWorkStart = uploadAttendanceRecord.AttendanceRecordDate.AddHours(uploadAttendanceRecord.AttendanceRecordWorkStart.Hour).AddMinutes(uploadAttendanceRecord.AttendanceRecordWorkStart.Minute);
            //    if (Math.Abs(((TimeSpan)dateTimeStart.Subtract(uploadAttendanceRecord.AttendanceRecordWorkStart)).TotalHours) > 12)
            //        if (uploadAttendanceRecord.AttendanceRecordWorkStart < dateTimeStart)
            //            uploadAttendanceRecord.AttendanceRecordWorkStart =uploadAttendanceRecord.AttendanceRecordWorkStart.AddDays(1);
            //        else
            //            uploadAttendanceRecord.AttendanceRecordWorkStart =uploadAttendanceRecord.AttendanceRecordWorkStart.AddDays(-1);

            //}

            if (rawDataTable.Columns.Contains(FIELD_LUNCHOUT))
            {
                string tmpString = row[FIELD_LUNCHOUT].ToString();
                if (!tmpString.Equals(string.Empty))
                    if (tmpString.Equals(ImportDBObject.FIELD_VALUE_OVERRIDE_EMPTY, StringComparison.CurrentCultureIgnoreCase))
                        uploadAttendanceRecord.AttendanceRecordLunchOut = new DateTime();
                    else
                        uploadAttendanceRecord.AttendanceRecordLunchOut = Parse.toDateTimeObject(row[FIELD_LUNCHOUT]);
            }
            //if (!uploadAttendanceRecord.AttendanceRecordLunchOut.Ticks.Equals(0))
            //{
            //    uploadAttendanceRecord.AttendanceRecordLunchOut = uploadAttendanceRecord.AttendanceRecordDate.AddHours(uploadAttendanceRecord.AttendanceRecordLunchOut.Hour).AddMinutes(uploadAttendanceRecord.AttendanceRecordLunchOut.Minute);
            //    if (uploadAttendanceRecord.AttendanceRecordLunchOut < uploadAttendanceRecord.AttendanceRecordWorkStart)
            //        uploadAttendanceRecord.AttendanceRecordLunchOut.AddDays(1);
            //}
            if (rawDataTable.Columns.Contains(FIELD_LUNCHIN))
            {
                string tmpString = row[FIELD_LUNCHIN].ToString();
                if (!tmpString.Equals(string.Empty))
                    if (tmpString.Equals(ImportDBObject.FIELD_VALUE_OVERRIDE_EMPTY, StringComparison.CurrentCultureIgnoreCase))
                        uploadAttendanceRecord.AttendanceRecordLunchIn = new DateTime();
                    else
                        uploadAttendanceRecord.AttendanceRecordLunchIn = Parse.toDateTimeObject(row[FIELD_LUNCHIN]);
            }
            //if (!uploadAttendanceRecord.AttendanceRecordLunchIn.Ticks.Equals(0))
            //{
            //    uploadAttendanceRecord.AttendanceRecordLunchIn = uploadAttendanceRecord.AttendanceRecordDate.AddHours(uploadAttendanceRecord.AttendanceRecordLunchIn.Hour).AddMinutes(uploadAttendanceRecord.AttendanceRecordLunchIn.Minute);
            //    if (uploadAttendanceRecord.AttendanceRecordLunchIn < uploadAttendanceRecord.AttendanceRecordWorkStart)
            //        uploadAttendanceRecord.AttendanceRecordLunchIn.AddDays(1);
            //}

            if (rawDataTable.Columns.Contains(FIELD_WORKOUT))
            {
                string tmpString = row[FIELD_WORKOUT].ToString();
                if (!tmpString.Equals(string.Empty))
                    if (tmpString.Equals(ImportDBObject.FIELD_VALUE_OVERRIDE_EMPTY, StringComparison.CurrentCultureIgnoreCase))
                        uploadAttendanceRecord.AttendanceRecordWorkEnd = new DateTime();
                    else
                        uploadAttendanceRecord.AttendanceRecordWorkEnd = Parse.toDateTimeObject(row[FIELD_WORKOUT]);
            }
            //if (!uploadAttendanceRecord.AttendanceRecordWorkEnd.Ticks.Equals(0))
            //{
            //    uploadAttendanceRecord.AttendanceRecordWorkEnd = uploadAttendanceRecord.AttendanceRecordDate.AddHours(uploadAttendanceRecord.AttendanceRecordWorkEnd.Hour).AddMinutes(uploadAttendanceRecord.AttendanceRecordWorkEnd.Minute);
            //    if (uploadAttendanceRecord.AttendanceRecordWorkEnd < uploadAttendanceRecord.AttendanceRecordWorkStart)
            //        uploadAttendanceRecord.AttendanceRecordWorkEnd.AddDays(1);
            //}

            if (rawDataTable.Columns.Contains(FIELD_WORKIN_LOCATION))
            {
                string tmpString = row[FIELD_WORKIN_LOCATION].ToString();
                if (!tmpString.Equals(string.Empty))
                    if (tmpString.Equals(ImportDBObject.FIELD_VALUE_OVERRIDE_EMPTY, StringComparison.CurrentCultureIgnoreCase))
                        uploadAttendanceRecord.AttendanceRecordWorkStartLocation = string.Empty;
                    else
                        uploadAttendanceRecord.AttendanceRecordWorkStartLocation = tmpString;
            }
            if (rawDataTable.Columns.Contains(FIELD_LUNCHOUT_LOCATION))
            {
                string tmpString = row[FIELD_LUNCHOUT_LOCATION].ToString();
                if (!tmpString.Equals(string.Empty))
                    if (tmpString.Equals(ImportDBObject.FIELD_VALUE_OVERRIDE_EMPTY, StringComparison.CurrentCultureIgnoreCase))
                        uploadAttendanceRecord.AttendanceRecordLunchOutLocation = string.Empty;
                    else
                        uploadAttendanceRecord.AttendanceRecordLunchOutLocation = tmpString;
            }
            if (rawDataTable.Columns.Contains(FIELD_LUNCHIN_LOCATION))
            {
                string tmpString = row[FIELD_LUNCHIN_LOCATION].ToString();
                if (!tmpString.Equals(string.Empty))
                    if (tmpString.Equals(ImportDBObject.FIELD_VALUE_OVERRIDE_EMPTY, StringComparison.CurrentCultureIgnoreCase))
                        uploadAttendanceRecord.AttendanceRecordLunchInLocation = string.Empty;
                    else
                        uploadAttendanceRecord.AttendanceRecordLunchInLocation = tmpString;
            }
            if (rawDataTable.Columns.Contains(FIELD_WORKOUT_LOCATION))
            {
                string tmpString = row[FIELD_WORKOUT_LOCATION].ToString();
                if (!tmpString.Equals(string.Empty))
                    if (tmpString.Equals(ImportDBObject.FIELD_VALUE_OVERRIDE_EMPTY, StringComparison.CurrentCultureIgnoreCase))
                        uploadAttendanceRecord.AttendanceRecordWorkEndLocation = string.Empty;
                    else
                        uploadAttendanceRecord.AttendanceRecordWorkEndLocation = tmpString;
            }
            int testMinutes = 0;
            if (!string.IsNullOrEmpty(row[FIELD_LATEMINS].ToString()))
                if (int.TryParse(row[FIELD_LATEMINS].ToString(), out testMinutes))
                    uploadAttendanceRecord.AttendanceRecordActualLateMins = testMinutes;
                else
                    errors.addError(ImportErrorMessage.ERROR_INVALID_FIELD_VALUE, new string[] { FIELD_LATEMINS + "=" + row[FIELD_LATEMINS].ToString(), EmpNo, rowCount.ToString() });
            else
                uploadAttendanceRecord.AttendanceRecordActualLateMins = 0;

            if (!string.IsNullOrEmpty(row[FIELD_EARLYLEAVEMINS].ToString()))
                if (int.TryParse(row[FIELD_EARLYLEAVEMINS].ToString(), out testMinutes))
                    uploadAttendanceRecord.AttendanceRecordActualEarlyLeaveMins = testMinutes;
                else
                    errors.addError(ImportErrorMessage.ERROR_INVALID_FIELD_VALUE, new string[] { FIELD_EARLYLEAVEMINS + "=" + row[FIELD_EARLYLEAVEMINS].ToString(), EmpNo, rowCount.ToString() });
            else
                uploadAttendanceRecord.AttendanceRecordActualEarlyLeaveMins = 0;

            if (rawDataTable.Columns.Contains(FIELD_LUNCH_LATEMINS))
                if (!string.IsNullOrEmpty(row[FIELD_LUNCH_LATEMINS].ToString()))
                    if (int.TryParse(row[FIELD_LUNCH_LATEMINS].ToString(), out testMinutes))
                        uploadAttendanceRecord.AttendanceRecordActualLunchLateMins = testMinutes;
                    else
                        errors.addError(ImportErrorMessage.ERROR_INVALID_FIELD_VALUE, new string[] { FIELD_LUNCH_LATEMINS + "=" + row[FIELD_LUNCH_LATEMINS].ToString(), EmpNo, rowCount.ToString() });
                else
                    uploadAttendanceRecord.AttendanceRecordActualLunchLateMins = 0;

            if (rawDataTable.Columns.Contains(FIELD_LUNCH_EARLYLEAVEMINS))
                if (!string.IsNullOrEmpty(row[FIELD_LUNCH_EARLYLEAVEMINS].ToString()))
                    if (int.TryParse(row[FIELD_LUNCH_EARLYLEAVEMINS].ToString(), out testMinutes))
                        uploadAttendanceRecord.AttendanceRecordActualLunchEarlyLeaveMins = testMinutes;
                    else
                        errors.addError(ImportErrorMessage.ERROR_INVALID_FIELD_VALUE, new string[] { FIELD_LUNCH_EARLYLEAVEMINS + "=" + row[FIELD_LUNCH_EARLYLEAVEMINS].ToString(), EmpNo, rowCount.ToString() });
                else
                    uploadAttendanceRecord.AttendanceRecordActualLunchEarlyLeaveMins = 0;

            if (!string.IsNullOrEmpty(row[FIELD_OVERTIMEMINS].ToString()))
                if (int.TryParse(row[FIELD_OVERTIMEMINS].ToString(), out testMinutes))
                    uploadAttendanceRecord.AttendanceRecordActualOvertimeMins = testMinutes;
                else
                    errors.addError(ImportErrorMessage.ERROR_INVALID_FIELD_VALUE, new string[] { FIELD_OVERTIMEMINS + "=" + row[FIELD_OVERTIMEMINS].ToString(), EmpNo, rowCount.ToString() });
            else
                uploadAttendanceRecord.AttendanceRecordActualOvertimeMins = 0;

            if (rawDataTable.Columns.Contains(FIELD_LUNCH_OVERTIMEMINS))
                if (!string.IsNullOrEmpty(row[FIELD_LUNCH_OVERTIMEMINS].ToString()))
                    if (int.TryParse(row[FIELD_LUNCH_OVERTIMEMINS].ToString(), out testMinutes))
                        uploadAttendanceRecord.AttendanceRecordActualLunchOvertimeMins = testMinutes;
                    else
                        errors.addError(ImportErrorMessage.ERROR_INVALID_FIELD_VALUE, new string[] { FIELD_LUNCH_OVERTIMEMINS + "=" + row[FIELD_LUNCH_OVERTIMEMINS].ToString(), EmpNo, rowCount.ToString() });
                else
                    uploadAttendanceRecord.AttendanceRecordActualLunchOvertimeMins = 0;

            if (rawDataTable.Columns.Contains(FIELD_LUNCHTIMEMINS))
                if (!string.IsNullOrEmpty(row[FIELD_LUNCHTIMEMINS].ToString()))
                    if (int.TryParse(row[FIELD_LUNCHTIMEMINS].ToString(), out testMinutes))
                        uploadAttendanceRecord.AttendanceRecordActualLunchTimeMins = testMinutes;
                    else
                        errors.addError(ImportErrorMessage.ERROR_INVALID_FIELD_VALUE, new string[] { FIELD_LUNCHTIMEMINS + "=" + row[FIELD_LUNCHTIMEMINS].ToString(), EmpNo, rowCount.ToString() });
                else
                    uploadAttendanceRecord.AttendanceRecordActualLunchTimeMins = 0;


            double testWorkingDayHour;

            if (!string.IsNullOrEmpty(row[FIELD_WORKINGDAY].ToString()))
                if (double.TryParse(row[FIELD_WORKINGDAY].ToString(), out testWorkingDayHour))
                    uploadAttendanceRecord.AttendanceRecordActualWorkingDay = testWorkingDayHour;
                else
                    errors.addError(ImportErrorMessage.ERROR_INVALID_FIELD_VALUE, new string[] { FIELD_WORKINGDAY + "=" + row[FIELD_WORKINGDAY].ToString(), EmpNo, rowCount.ToString() });
            else
                uploadAttendanceRecord.AttendanceRecordActualWorkingDay = 0;

            if (!string.IsNullOrEmpty(row[FIELD_WORKINGHOUR].ToString()))
                if (double.TryParse(row[FIELD_WORKINGHOUR].ToString(), out testWorkingDayHour))
                    uploadAttendanceRecord.AttendanceRecordActualWorkingHour = testWorkingDayHour;
                else
                    errors.addError(ImportErrorMessage.ERROR_INVALID_FIELD_VALUE, new string[] { FIELD_WORKINGHOUR + "=" + row[FIELD_WORKINGHOUR].ToString(), EmpNo, rowCount.ToString() });
            else
                uploadAttendanceRecord.AttendanceRecordActualWorkingHour = 0;

            if (rawDataTable.Columns.Contains(FIELD_WORK_ON_RESTDAY))
            {
                uploadAttendanceRecord.AttendanceRecordWorkOnRestDay = row[FIELD_WORK_ON_RESTDAY].ToString().Equals("Yes", StringComparison.CurrentCultureIgnoreCase);
            }

            if (rawDataTable.Columns.Contains(FIELD_EXTENDDATA_OVERRIDE_DAILY_PAYMENT))
            {
                if (!string.IsNullOrEmpty(row[FIELD_EXTENDDATA_OVERRIDE_DAILY_PAYMENT].ToString()))
                {
                    double overrideDailyPayment = 0;
                    if (double.TryParse(row[FIELD_EXTENDDATA_OVERRIDE_DAILY_PAYMENT].ToString(), out overrideDailyPayment))
                        uploadAttendanceRecord.SetAttendanceRecordExtendData(EAttendanceRecord.FIELD_EXTENDDATA_OVERRIDE_DAILY_PAYMENT, row[FIELD_EXTENDDATA_OVERRIDE_DAILY_PAYMENT].ToString());
                    else
                        errors.addError(ImportErrorMessage.ERROR_INVALID_FIELD_VALUE, new string[] { FIELD_EXTENDDATA_OVERRIDE_DAILY_PAYMENT + "=" + row[FIELD_EXTENDDATA_OVERRIDE_DAILY_PAYMENT].ToString(), EmpNo, rowCount.ToString() });
                }
            }
            if (rawDataTable.Columns.Contains(FIELD_EXTENDDATA_WORK_AS_OVERTIME))
            {
                if (!string.IsNullOrEmpty(row[FIELD_EXTENDDATA_WORK_AS_OVERTIME].ToString()))
                {
                    if (row[FIELD_EXTENDDATA_WORK_AS_OVERTIME].ToString().Equals("Y", StringComparison.CurrentCultureIgnoreCase)
                    || row[FIELD_EXTENDDATA_WORK_AS_OVERTIME].ToString().Equals("Yes", StringComparison.CurrentCultureIgnoreCase))
                        uploadAttendanceRecord.SetAttendanceRecordExtendData(EAttendanceRecord.FIELD_EXTENDDATA_WORK_AS_OVERTIME, "YES");
                    else if (row[FIELD_EXTENDDATA_WORK_AS_OVERTIME].ToString().Equals("N", StringComparison.CurrentCultureIgnoreCase)
                    || row[FIELD_EXTENDDATA_WORK_AS_OVERTIME].ToString().Equals("No", StringComparison.CurrentCultureIgnoreCase))
                        uploadAttendanceRecord.SetAttendanceRecordExtendData(EAttendanceRecord.FIELD_EXTENDDATA_WORK_AS_OVERTIME, string.Empty);
                    else
                        errors.addError(ImportErrorMessage.ERROR_INVALID_FIELD_VALUE, new string[] { FIELD_EXTENDDATA_WORK_AS_OVERTIME + "=" + row[FIELD_EXTENDDATA_WORK_AS_OVERTIME].ToString(), EmpNo, rowCount.ToString() });
                }
            }
            return uploadAttendanceRecord;
        }

        public override DataTable GetImportDataFromTempDatabase(ListInfo info)
        {
            DBFilter sessionFilter = new DBFilter();
            sessionFilter.add(new Match("SessionID", m_SessionID));
            sessionFilter.add(new MatchField("e.EmpID", "c.EmpID"));
            //if (info != null && info.orderby != null && !info.orderby.Equals(""))
            //    sessionFilter.add(info.orderby, info.order);
            DataTable result = sessionFilter.loadData(dbConn, null, "e.*, c.* ", " from " + tempDB.dbclass.tableName + " c, " + EEmpPersonalInfo.db.dbclass.tableName + " e");


            return result;



        }

        public static void ClearTempTable(DatabaseConnection dbConn, string sessionID)
        {
            ImportAttendanceRecordProcess import = new ImportAttendanceRecordProcess(dbConn, sessionID, false);
            import.ClearTempTable();
        }

        public override void ClearTempTable()
        {
            //  Clear Old Import Session
            DBFilter sessionFilter = new DBFilter();
            if (!string.IsNullOrEmpty(m_SessionID))
                sessionFilter.add(new Match("SessionID", m_SessionID));
            ArrayList uploadEmpContractList = tempDB.select(dbConn, sessionFilter);
            foreach (EUploadAttendanceRecord uploadAttendanceRecord in uploadEmpContractList)
            {
                tempDB.delete(dbConn, uploadAttendanceRecord);

            }

        }

        public override void ImportToDatabase()
        {
            DBFilter sessionFilter = new DBFilter();
            sessionFilter.add("EmpID", true);
            sessionFilter.add("AttendanceRecordDate", true);
            if (!string.IsNullOrEmpty(m_SessionID))
                sessionFilter.add(new Match("SessionID", m_SessionID));
            ArrayList uploadAttendanceRecordList = tempDB.select(dbConn, sessionFilter);

            HROne.Attendance.AttendanceProcess attendanceProcess = new HROne.Attendance.AttendanceProcess(dbConn);
            int currentEmpID = 0;
            DateTime currentAttendanceRecordDate = new DateTime();
            foreach (EUploadAttendanceRecord obj in uploadAttendanceRecordList)
            {
                if (currentEmpID != obj.EmpID || currentAttendanceRecordDate != obj.AttendanceRecordDate)
                {
                    DBFilter existingUploadAttendanceRecordFilter = new DBFilter();
                    existingUploadAttendanceRecordFilter.add(new Match("EmpID", obj.EmpID));
                    existingUploadAttendanceRecordFilter.add(new Match("AttendanceRecordDate", obj.AttendanceRecordDate));
                    existingUploadAttendanceRecordFilter.add(new Match("SessionID", m_SessionID));

                    DBFilter deleteAttendanceRecordFilter = new DBFilter();
                    deleteAttendanceRecordFilter.add(new Match("EmpID", obj.EmpID));
                    deleteAttendanceRecordFilter.add(new Match("AttendanceRecordDate", obj.AttendanceRecordDate));
                    deleteAttendanceRecordFilter.add(new IN("NOT AttendanceRecordID", "SELECT uar.AttendanceRecordID FROM " + EUploadAttendanceRecord.db.dbclass.tableName + " uar", existingUploadAttendanceRecordFilter));
                    EAttendanceRecord.db.delete(dbConn, deleteAttendanceRecordFilter);

                    currentEmpID = obj.EmpID;
                    currentAttendanceRecordDate = obj.AttendanceRecordDate;
                }

                EAttendanceRecord attendanceRecord = null;

                if (obj.ImportActionStatus != ImportDBObject.ImportActionEnum.INSERT)
                {
                    attendanceRecord = new EAttendanceRecord();
                    attendanceRecord.AttendanceRecordID = obj.AttendanceRecordID;
                    if (!uploadDB.select(dbConn, attendanceRecord))
                        //  Change the action status to INSERT if status is UPDATE
                        if (obj.ImportActionStatus == ImportDBObject.ImportActionEnum.UPDATE)
                            obj.ImportActionStatus = ImportDBObject.ImportActionEnum.INSERT;

                }
                if (obj.ImportActionStatus == ImportDBObject.ImportActionEnum.INSERT)
                {
                    //  Insert mode
                    System.Collections.Generic.List<EAttendanceRecord> list = attendanceProcess.CreateAttendanceRecordObject(obj.EmpID, obj.AttendanceRecordDate, true, 0);
                    if (list.Count > 0)
                        attendanceRecord = list[0];
                    else
                        attendanceRecord = new EAttendanceRecord();
                }

                obj.ExportToObject(attendanceRecord);

                if (m_IsRecalculate)
                {
                    attendanceRecord = attendanceProcess.GetAttendanceTimeResult(attendanceRecord);
                }
                if (obj.ImportActionStatus == ImportDBObject.ImportActionEnum.INSERT)
                {
                    uploadDB.insert(dbConn, attendanceRecord);

                }
                else if (obj.ImportActionStatus == ImportDBObject.ImportActionEnum.UPDATE)
                {
                    uploadDB.update(dbConn, attendanceRecord);

                }
                EUploadAttendanceRecord.db.delete(dbConn, obj);

            }
        }
        public DataTable Export(ArrayList EmpInfoList, DateTime PeriodFrom, DateTime PeriodTo)
        {
            return Export(EmpInfoList, PeriodFrom, PeriodTo, false);
        }
        public DataTable Export(ArrayList EmpInfoList, DateTime PeriodFrom, DateTime PeriodTo, bool CreateTempIfNotExists)
        {
            DataTable tmpDataTable = new DataTable("AttendanceRecord$");

            DBFilter hierarchyLevelFilter = new DBFilter();
            Dictionary<int, EHierarchyLevel> hierarchyLevelHashTable = new Dictionary<int, EHierarchyLevel>();
            hierarchyLevelFilter.add("HLevelSeqNo", true);
            ArrayList hierarchyLevelList = EHierarchyLevel.db.select(dbConn, hierarchyLevelFilter);
            foreach (EHierarchyLevel hlevel in hierarchyLevelList)
            {
                hierarchyLevelHashTable.Add(hlevel.HLevelID, hlevel);
            }

            CreateDataColumn(tmpDataTable, hierarchyLevelHashTable);

            HROne.Attendance.AttendanceProcess attendanceProcess = new HROne.Attendance.AttendanceProcess(dbConn);
            foreach (EEmpPersonalInfo empInfo in EmpInfoList)
            {
                if (EEmpPersonalInfo.db.select(dbConn, empInfo))
                {

                    for (DateTime currentDate = PeriodFrom; currentDate <= PeriodTo; currentDate = currentDate.AddDays(1))
                    {
                        DBFilter attendanceRecordFilter = new DBFilter();
                        attendanceRecordFilter.add(new Match("EmpID", empInfo.EmpID));
                        //attendanceRecordFilter.add(new Match("AttendanceRecordDate", ">=", PeriodFrom));
                        //attendanceRecordFilter.add(new Match("AttendanceRecordDate", "<=", PeriodTo));
                        attendanceRecordFilter.add(new Match("AttendanceRecordDate", "=", currentDate));
                        attendanceRecordFilter.add("AttendanceRecordDate", true);
                        ArrayList attendanceRecordList = EAttendanceRecord.db.select(dbConn, attendanceRecordFilter);
                        if (attendanceRecordList.Count <= 0 && CreateTempIfNotExists)
                            //  Create temp data if not exists
                            attendanceRecordList = new ArrayList(attendanceProcess.CreateAttendanceRecordObject(empInfo.EmpID, currentDate, true, 0));
                        foreach (EAttendanceRecord attendanceRecord in attendanceRecordList)
                        {
                            DataRow row = tmpDataTable.NewRow();
                            ConvertToDataRow(empInfo, attendanceRecord, row, hierarchyLevelHashTable);
                            tmpDataTable.Rows.Add(row);
                        }
                    }
                }
            } 
            return tmpDataTable;
        }

        protected virtual void CreateDataColumn(DataTable tmpDataTable, Dictionary<int, EHierarchyLevel> hierarchyLevelHashTable)
        {
            tmpDataTable.Columns.Add(FIELD_EMP_NO, typeof(string));
            tmpDataTable.Columns.Add("EnglishName", typeof(string));
            tmpDataTable.Columns.Add("ChineseName", typeof(string));

            tmpDataTable.Columns.Add("Company", typeof(string));

            foreach (EHierarchyLevel hlevel in hierarchyLevelHashTable.Values)
            {
                tmpDataTable.Columns.Add(hlevel.HLevelDesc, typeof(string));
            }

            tmpDataTable.Columns.Add(FIELD_DATE, typeof(DateTime));
            tmpDataTable.Columns.Add(FIELD_ROSTERCODE, typeof(string));
            tmpDataTable.Columns.Add(FIELD_ROSTERCODE_OVERRIDE_WORKIN, typeof(string));
            tmpDataTable.Columns.Add(FIELD_ROSTERCODE_OVERRIDE_LUNCHOUT, typeof(string));
            tmpDataTable.Columns.Add(FIELD_ROSTERCODE_OVERRIDE_LUNCHIN, typeof(string));
            tmpDataTable.Columns.Add(FIELD_ROSTERCODE_OVERRIDE_WORKOUT, typeof(string));
            tmpDataTable.Columns.Add(FIELD_WORKIN, typeof(string));
            tmpDataTable.Columns.Add(FIELD_WORKIN_LOCATION, typeof(string));
            tmpDataTable.Columns.Add(FIELD_WORKIN_TIMECARDID, typeof(int));
            tmpDataTable.Columns.Add(FIELD_LUNCHOUT, typeof(string));
            tmpDataTable.Columns.Add(FIELD_LUNCHOUT_LOCATION, typeof(string));
            tmpDataTable.Columns.Add(FIELD_LUNCHOUT_TIMECARDID, typeof(int));
            tmpDataTable.Columns.Add(FIELD_LUNCHIN, typeof(string));
            tmpDataTable.Columns.Add(FIELD_LUNCHIN_LOCATION, typeof(string));
            tmpDataTable.Columns.Add(FIELD_LUNCHIN_TIMECARDID, typeof(int));
            tmpDataTable.Columns.Add(FIELD_WORKOUT, typeof(string));
            tmpDataTable.Columns.Add(FIELD_WORKOUT_LOCATION, typeof(string));
            tmpDataTable.Columns.Add(FIELD_WORKOUT_TIMECARDID, typeof(int));
            tmpDataTable.Columns.Add(FIELD_LATEMINS, typeof(int));
            tmpDataTable.Columns.Add(FIELD_EARLYLEAVEMINS, typeof(int));
            tmpDataTable.Columns.Add(FIELD_LUNCH_LATEMINS, typeof(int));
            tmpDataTable.Columns.Add(FIELD_LUNCH_EARLYLEAVEMINS, typeof(int));
            tmpDataTable.Columns.Add(FIELD_WORKINGHOUR, typeof(double));
            tmpDataTable.Columns.Add(FIELD_OVERTIMEMINS, typeof(int));
            tmpDataTable.Columns.Add(FIELD_LUNCH_OVERTIMEMINS, typeof(int));
            tmpDataTable.Columns.Add(FIELD_LUNCHTIMEMINS, typeof(int));
            tmpDataTable.Columns.Add("Total Working Hour", typeof(string));
            tmpDataTable.Columns.Add(FIELD_ISABSENT, typeof(string));
            tmpDataTable.Columns.Add(FIELD_WORKINGDAY, typeof(double));
            tmpDataTable.Columns.Add(FIELD_HAS_BONUS, typeof(string));
            tmpDataTable.Columns.Add(FIELD_WORK_ON_RESTDAY, typeof(string));
            tmpDataTable.Columns.Add("Change Site", typeof(string));
            tmpDataTable.Columns.Add(FIELD_REMARK, typeof(string));
            tmpDataTable.Columns.Add(FIELD_EXTENDDATA_OVERRIDE_DAILY_PAYMENT, typeof(double));
            tmpDataTable.Columns.Add(FIELD_EXTENDDATA_WORK_AS_OVERTIME, typeof(string));

        }

        protected virtual void ConvertToDataRow(EEmpPersonalInfo empInfo, EAttendanceRecord attendanceRecord, DataRow row, Dictionary<int, EHierarchyLevel> hierarchyLevelHashTable)
        {
            row[FIELD_EMP_NO] = empInfo.EmpNo;
            row["EnglishName"] = empInfo.EmpEngFullName;
            row["ChineseName"] = empInfo.EmpChiFullName;

            DBFilter empPosFilter = new DBFilter();

            EEmpPositionInfo empPos = AppUtils.GetLastPositionInfo(dbConn, attendanceRecord.AttendanceRecordDate, empInfo.EmpID);
            if (empPos != null)
            {
                ECompany company = new ECompany();
                company.CompanyID = empPos.CompanyID;
                if (ECompany.db.select(dbConn, company))
                    row["Company"] = company.CompanyCode;
                DBFilter empHierarchyFilter = new DBFilter();
                empHierarchyFilter.add(new Match("EmpPosID", empPos.EmpPosID));
                ArrayList empHierarchyList = EEmpHierarchy.db.select(dbConn, empHierarchyFilter);
                foreach (EEmpHierarchy empHierarchy in empHierarchyList)
                {
                    EHierarchyLevel hierarchyLevel = (EHierarchyLevel)hierarchyLevelHashTable[empHierarchy.HLevelID];
                    if (hierarchyLevel != null)
                    {
                        EHierarchyElement hierarchyElement = new EHierarchyElement();
                        hierarchyElement.HElementID = empHierarchy.HElementID;
                        if (EHierarchyElement.db.select(dbConn, hierarchyElement))
                            row[hierarchyLevel.HLevelDesc] = hierarchyElement.HElementCode;
                    }
                }
            }

            row["Date"] = attendanceRecord.AttendanceRecordDate;
            TimeSpan totalWorkHourTimeSpan = attendanceRecord.TotalWorkingHourTimeSpan(dbConn);
            ERosterCode rosterCode = new ERosterCode();
            rosterCode.RosterCodeID = attendanceRecord.RosterCodeID;
            if (ERosterCode.db.select(dbConn, rosterCode))
            {
                row[FIELD_ROSTERCODE] = rosterCode.RosterCode;
            }

            if (!attendanceRecord.AttendanceRecordRosterCodeInTimeOverride.Ticks.Equals(0))
                row[FIELD_ROSTERCODE_OVERRIDE_WORKIN] = attendanceRecord.AttendanceRecordRosterCodeInTimeOverride.ToString("HH:mm");
            if (!attendanceRecord.AttendanceRecordRosterCodeLunchStartTimeOverride.Ticks.Equals(0))
                row[FIELD_ROSTERCODE_OVERRIDE_LUNCHOUT] = attendanceRecord.AttendanceRecordRosterCodeLunchStartTimeOverride.ToString("HH:mm");
            if (!attendanceRecord.AttendanceRecordRosterCodeLunchEndTimeOverride.Ticks.Equals(0))
                row[FIELD_ROSTERCODE_OVERRIDE_LUNCHIN] = attendanceRecord.AttendanceRecordRosterCodeLunchEndTimeOverride.ToString("HH:mm");
            if (!attendanceRecord.AttendanceRecordRosterCodeOutTimeOverride.Ticks.Equals(0))
                row[FIELD_ROSTERCODE_OVERRIDE_WORKOUT] = attendanceRecord.AttendanceRecordRosterCodeOutTimeOverride.ToString("HH:mm");

            if (!attendanceRecord.AttendanceRecordWorkStart.Ticks.Equals(0))
                row[FIELD_WORKIN] = attendanceRecord.AttendanceRecordWorkStart.ToString("HH:mm");
            if (!attendanceRecord.AttendanceRecordLunchOut.Ticks.Equals(0))
                row[FIELD_LUNCHOUT] = attendanceRecord.AttendanceRecordLunchOut.ToString("HH:mm");
            if (!attendanceRecord.AttendanceRecordLunchIn.Ticks.Equals(0))
                row[FIELD_LUNCHIN] = attendanceRecord.AttendanceRecordLunchIn.ToString("HH:mm");
            if (!attendanceRecord.AttendanceRecordWorkEnd.Ticks.Equals(0))
                row[FIELD_WORKOUT] = attendanceRecord.AttendanceRecordWorkEnd.ToString("HH:mm");

            row[FIELD_WORKIN_LOCATION] = attendanceRecord.AttendanceRecordWorkStartLocation;
            row[FIELD_LUNCHOUT_LOCATION] = attendanceRecord.AttendanceRecordLunchOutLocation;
            row[FIELD_LUNCHIN_LOCATION] = attendanceRecord.AttendanceRecordLunchInLocation;
            row[FIELD_WORKOUT_LOCATION] = attendanceRecord.AttendanceRecordWorkEndLocation;

            row[FIELD_WORKIN_TIMECARDID] = attendanceRecord.AttendanceRecordWorkStartTimeCardRecordID;
            row[FIELD_LUNCHOUT_TIMECARDID] = attendanceRecord.AttendanceRecordLunchOutTimeCardRecordID;
            row[FIELD_LUNCHIN_TIMECARDID] = attendanceRecord.AttendanceRecordLunchInTimeCardRecordID;
            row[FIELD_WORKOUT_TIMECARDID] = attendanceRecord.AttendanceRecordWorkEndTimeCardRecordID;

            row[FIELD_LATEMINS] = attendanceRecord.AttendanceRecordActualLateMins;
            row[FIELD_EARLYLEAVEMINS] = attendanceRecord.AttendanceRecordActualEarlyLeaveMins;
            row[FIELD_LUNCH_LATEMINS] = attendanceRecord.AttendanceRecordActualLunchLateMins;
            row[FIELD_LUNCH_EARLYLEAVEMINS] = attendanceRecord.AttendanceRecordActualLunchEarlyLeaveMins;
            row[FIELD_WORKINGHOUR] = attendanceRecord.AttendanceRecordActualWorkingHour;
            row[FIELD_OVERTIMEMINS] = attendanceRecord.AttendanceRecordActualOvertimeMins;
            row[FIELD_LUNCH_OVERTIMEMINS] = attendanceRecord.AttendanceRecordActualLunchOvertimeMins;
            row[FIELD_LUNCHTIMEMINS] = attendanceRecord.AttendanceRecordCalculateLunchTimeMins;


            row["Total Working Hour"] = totalWorkHourTimeSpan.Hours + ":" + totalWorkHourTimeSpan.Minutes.ToString("00");
            row[FIELD_ISABSENT] = attendanceRecord.AttendanceRecordIsAbsent ? "Yes" : "No";
            row[FIELD_WORKINGDAY] = attendanceRecord.AttendanceRecordActualWorkingDay;
            row[FIELD_REMARK] = attendanceRecord.AttendanceRecordRemark; ;
            if (attendanceRecord.AttendanceRecordOverrideBonusEntitled)
                row[FIELD_HAS_BONUS] = attendanceRecord.AttendanceRecordHasBonus ? "Yes" : "No";
            row["Change Site"] = attendanceRecord.IsChangeSite(dbConn) ? "Yes" : "";

            row[FIELD_WORK_ON_RESTDAY] = attendanceRecord.AttendanceRecordWorkOnRestDay ? "Yes" : "No";
            
            double dailyOverrideValue = 0;
            if (double.TryParse(attendanceRecord.GetAttendanceRecordExtendData(EAttendanceRecord.FIELD_EXTENDDATA_OVERRIDE_DAILY_PAYMENT), out dailyOverrideValue))
                row[FIELD_EXTENDDATA_OVERRIDE_DAILY_PAYMENT] = dailyOverrideValue;
            if (attendanceRecord.GetAttendanceRecordExtendData(EAttendanceRecord.FIELD_EXTENDDATA_WORK_AS_OVERTIME).Equals("YES", StringComparison.CurrentCultureIgnoreCase))
                row[FIELD_EXTENDDATA_WORK_AS_OVERTIME] = "Yes";
        }
    }
}