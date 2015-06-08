using System;
using System.Collections.Generic;
using System.Text;
using HROne.DataAccess;
////using perspectivemind.validation;

namespace HROne.Lib.Entities
{
    [DBClass("TimeCardRecord")]
    public class ETimeCardRecord : BaseObject 
    {
        public enum TimeCardRecordInOutIndexEnum
        {
            Unspecify = 0,
            InEntry = 0x1,
            OutEntry = 0x2,
            WorkEntry=0x4,
            MealBreakEntry = 0x8,
            WorkStart = WorkEntry | InEntry,
            MealBreakOut = MealBreakEntry | OutEntry,
            MealBreakIn = MealBreakEntry | InEntry,
            WorkEnd = WorkEntry | OutEntry
        }

        public static DBManager db = new DBManager(typeof(ETimeCardRecord));

        protected int m_TimeCardRecordID;
        [DBField("TimeCardRecordID", true, true), TextSearch, Export(false)]
        public int TimeCardRecordID
        {
            get { return m_TimeCardRecordID; }
            set { m_TimeCardRecordID = value; modify("TimeCardRecordID"); }
        }

        protected int m_EmpID;
        [DBField("EmpID"), TextSearch, Export(false)]
        public int EmpID
        {
            get { return m_EmpID; }
            set { m_EmpID = value; modify("EmpID"); }
        }

        protected string m_TimeCardRecordCardNo;
        [DBField("TimeCardRecordCardNo"), TextSearch, Export(false)]
        public string TimeCardRecordCardNo
        {
            get { return m_TimeCardRecordCardNo; }
            set { m_TimeCardRecordCardNo = value; modify("TimeCardRecordCardNo"); }
        }

        protected DateTime m_TimeCardRecordDateTime;
        [DBField("TimeCardRecordDateTime", "yyyy-MM-dd HH:mm:ss"), TextSearch, Export(false)]
        public DateTime TimeCardRecordDateTime
        {
            get { return m_TimeCardRecordDateTime; }
            set { m_TimeCardRecordDateTime = value; modify("TimeCardRecordDateTime"); }
        }

        protected string m_TimeCardRecordLocation;
        [DBField("TimeCardRecordLocation"), TextSearch, Export(false)]
        public string TimeCardRecordLocation
        {
            get { return m_TimeCardRecordLocation; }
            set { m_TimeCardRecordLocation = value; modify("TimeCardRecordLocation"); }
        }

        protected TimeCardRecordInOutIndexEnum m_TimeCardRecordInOutIndex;
        [DBField("TimeCardRecordInOutIndex"), TextSearch, Export(false)]
        public TimeCardRecordInOutIndexEnum TimeCardRecordInOutIndex
        {
            get { return m_TimeCardRecordInOutIndex; }
            set { m_TimeCardRecordInOutIndex = value; modify("TimeCardRecordInOutIndex"); }
        }

        protected string m_TimeCardRecordOriginalData;
        [DBField("TimeCardRecordOriginalData"), TextSearch, Export(false)]
        public string TimeCardRecordOriginalData
        {
            get { return m_TimeCardRecordOriginalData; }
            set { m_TimeCardRecordOriginalData = value; modify("TimeCardRecordOriginalData"); }
        }
    }
}
