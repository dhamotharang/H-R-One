using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Data;
using HROne.DataAccess;
using HROne.Common;
namespace HROne.Lib.Entities
{
    [DBClass("AttendancePreparationProcess")]
    public class EAttendancePreparationProcess : BaseObjectWithRecordInfo
    {
        public const string STATUS_CONFIRMED = "C";
        public const string STATUS_NORMAL = "N";
        public const string STATUS_CANCELLED = "X";
        public const string STATUS_CONFIRMED_DESC = "Confirmed";
        public const string STATUS_NORMAL_DESC = "Normal";
        public const string STATUS_CANCELLED_DESC = "Cancelled";

        public const string SHIFT_CODE_7N = "1/7N";
        public const string SHIFT_CODE_7Y = "1/7Y";
        public const string SHIFT_CODE_HOUR = "ïr";
        public const string SHIFT_CODE_DAY = "»’";
        public const string SHIFT_CODE_418 = "418";
        public const string SHIFT_CODE_7P = "1/7P";
        public const string SHIFT_CODE_SIXDAY = "¡˘»’";
        public const string SHIFT_CODE_DAYP = "»’/P";


        public static DBManager db = new DBManagerWithRecordInfo(typeof(EAttendancePreparationProcess));
        public static WFValueList VLBonusProcessList = new WFDBCodeList(db, "AttendancePreparationProcessID", "CONVERT(nvarchar(7), AttendancePreparationProcessMonth, 121)", "AttendancePreparationProcessDesc", "AttendancePreparationProcessMonth, AttendancePreparationProcessDesc");

        public static EAttendancePreparationProcess GetObject(DatabaseConnection dbConn, int ID)
        {
            EAttendancePreparationProcess m_object = new EAttendancePreparationProcess();
            m_object.AttendancePreparationProcessID = ID;
            if (EAttendancePreparationProcess.db.select(dbConn, m_object))
                return m_object;
            return null;
        }

        protected int m_AttendancePreparationProcessID;
        [DBField("AttendancePreparationProcessID", true, true), TextSearch, Export(false)]
        public int AttendancePreparationProcessID
        {
            get { return m_AttendancePreparationProcessID; }
            set { m_AttendancePreparationProcessID = value; modify("AttendancePreparationProcessID"); }
        }

        protected DateTime m_AttendancePreparationProcessMonth;
        [DBField("AttendancePreparationProcessMonth", format = "yyyy-MM-dd"), TextSearch, Export(false), Required]
        public DateTime AttendancePreparationProcessMonth
        {
            get { return m_AttendancePreparationProcessMonth; }
            set { m_AttendancePreparationProcessMonth = value; modify("AttendancePreparationProcessMonth"); }
        }

        protected string m_AttendancePreparationProcessDesc;
        [DBField("AttendancePreparationProcessDesc"), TextSearch, DBAESEncryptStringField, MaxLength(100, 50), Export(false), Required]
        public string AttendancePreparationProcessDesc
        {
            get { return m_AttendancePreparationProcessDesc; }
            set { m_AttendancePreparationProcessDesc = value; modify("AttendancePreparationProcessDesc"); }
        }

        protected string m_AttendancePreparationProcessStatus;
        [DBField("AttendancePreparationProcessStatus"), TextSearch, MaxLength(3, 3), Export(false), Required]
        public string AttendancePreparationProcessStatus
        {
            get { return m_AttendancePreparationProcessStatus; }
            set { m_AttendancePreparationProcessStatus = value; modify("AttendancePreparationProcessStatus"); }
        }

        protected DateTime m_AttendancePreparationProcessPayDate;
        [DBField("AttendancePreparationProcessPayDate", format = "yyyy-MM-dd"), TextSearch, Export(false), Required]
        public DateTime AttendancePreparationProcessPayDate
        {
            get { return m_AttendancePreparationProcessPayDate; }
            set { m_AttendancePreparationProcessPayDate = value; modify("AttendancePreparationProcessPayDate"); }
        }

        protected int m_AttendancePreparationProcessEmpCount;
        [DBField("AttendancePreparationProcessEmpCount"), TextSearch, Export(false), Required]
        public int AttendancePreparationProcessEmpCount
        {
            get { return m_AttendancePreparationProcessEmpCount; }
            set { m_AttendancePreparationProcessEmpCount = value; modify("AttendancePreparationProcessEmpCount"); }
        }

        protected DateTime m_AttendancePreparationProcessPeriodTo;
        [DBField("AttendancePreparationProcessPeriodTo", format = "yyyy-MM-dd"), TextSearch, Export(false), Required]
        public DateTime AttendancePreparationProcessPeriodTo
        {
            get { return m_AttendancePreparationProcessPeriodTo; }
            set { m_AttendancePreparationProcessPeriodTo = value; modify("AttendancePreparationProcessPeriodTo"); }
        }

        protected DateTime m_AttendancePreparationProcessPeriodFr;
        [DBField("AttendancePreparationProcessPeriodFr", format = "yyyy-MM-dd"), TextSearch, Export(false), Required]
        public DateTime AttendancePreparationProcessPeriodFr
        {
            get { return m_AttendancePreparationProcessPeriodFr; }
            set { m_AttendancePreparationProcessPeriodFr = value; modify("AttendancePreparationProcessPeriodFr"); }
        }
    }
}
