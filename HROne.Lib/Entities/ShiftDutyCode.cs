using System;
using System.Collections.Generic;
using System.Text;
using HROne.DataAccess;
using HROne.Common;
namespace HROne.Lib.Entities
{
    [DBClass("ShiftDutyCode")]
    public class EShiftDutyCode : BaseObject
    {
        public static DBManager db = new DBManager(typeof(EShiftDutyCode));
        public static WFValueList VLShiftDutyCode = new WFDBCodeList(db, "ShiftDutyCodeID", "ShiftDutyCode", "ShiftDutyCodeDesc", "ShiftDutyCode");

        public static EShiftDutyCode GetObject(DatabaseConnection dbConn, object ID)
        {
            if (ID is int)
            {
                EShiftDutyCode obj = new EShiftDutyCode();
                obj.ShiftDutyCodeID = (int)ID;
                if (db.select(dbConn, obj))
                    return obj;
            }
            return null;
        }

        protected int m_ShiftDutyCodeID;
        [DBField("ShiftDutyCodeID", true, true), TextSearch, Export(false)]
        public int ShiftDutyCodeID
        {
            get { return m_ShiftDutyCodeID; }
            set { m_ShiftDutyCodeID = value; modify("ShiftDutyCodeID"); }
        }
        protected string m_ShiftDutyCode;
        [DBField("ShiftDutyCode"), TextSearch, MaxLength(20), Export(false), Required]
        public string ShiftDutyCode
        {
            get { return m_ShiftDutyCode; }
            set { m_ShiftDutyCode = value; modify("ShiftDutyCode"); }
        }
        protected string m_ShiftDutyCodeDesc;
        [DBField("ShiftDutyCodeDesc"), TextSearch, MaxLength(70), Export(false)]
        public string ShiftDutyCodeDesc
        {
            get { return m_ShiftDutyCodeDesc; }
            set { m_ShiftDutyCodeDesc = value; modify("ShiftDutyCodeDesc"); }
        }
        protected DateTime m_ShiftDutyFromTime;
        [DBField("ShiftDutyFromTime", "HH:mm"), TextSearch, MaxLength(5), Export(false), Required]
        public DateTime ShiftDutyFromTime
        {
            get { return m_ShiftDutyFromTime; }
            set { m_ShiftDutyFromTime = value; modify("ShiftDutyFromTime"); }
        }
        protected DateTime m_ShiftDutyToTime;
        [DBField("ShiftDutyToTime", "HH:mm"), TextSearch, MaxLength(5), Export(false), Required]
        public DateTime ShiftDutyToTime
        {
            get { return m_ShiftDutyToTime; }
            set { m_ShiftDutyToTime = value; modify("ShiftDutyToTime"); }
        }   
    }
}
