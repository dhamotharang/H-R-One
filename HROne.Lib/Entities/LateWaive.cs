using System;
using System.Collections;
using System.Text;
using HROne.DataAccess;

namespace HROne.Lib.Entities
{
    [DBClass("LateWaive")]
    public class ELateWaive : BaseObjectWithRecordInfo
    {
        public static DBManager db = new DBManagerWithRecordInfo(typeof(ELateWaive));

        protected int m_LateWaiveID;
        [DBField("LateWaiveID", true, true), TextSearch, Export(false)]
        public int LateWaiveID
        {
            get { return m_LateWaiveID; }
            set { m_LateWaiveID = value; modify("LateWaiveID"); }
        }

        protected int m_EmpID;
        [DBField("EmpID"), TextSearch, Export(false)]
        public int EmpID
        {
            get { return m_EmpID; }
            set { m_EmpID = value; modify("EmpID"); }
        }

        protected int m_AttendanceRecordID;
        [DBField("AttendanceRecordID"), TextSearch, Export(false)]
        public int AttendanceRecordID
        {
            get { return m_AttendanceRecordID; }
            set { m_AttendanceRecordID = value; modify("AttendanceRecordID"); }
        }

        protected string m_LateWaiveReason;
        [DBField("LateWaiveReason"), TextSearch, Export(false)]
        public string LateWaiveReason
        {
            get { return m_LateWaiveReason; }
            set { m_LateWaiveReason = value; modify("LateWaiveReason"); }
        }

        protected int m_LateWaiveCancelID;
        [DBField("LateWaiveCancelID"), TextSearch, Export(false)]
        public int LateWaiveCancelID
        {
            get { return m_LateWaiveCancelID; }
            set { m_LateWaiveCancelID = value; modify("LateWaiveCancelID"); }
        }

        protected int m_EmpRequestID;
        [DBField("EmpRequestID"), TextSearch, Export(false)]
        public int EmpRequestID
        {
            get { return m_EmpRequestID; }
            set { m_EmpRequestID = value; modify("EmpRequestID"); }
        }

        protected int m_RequestLateWaiveID;
        [DBField("RequestLateWaiveID"), TextSearch, Export(false)]
        public int RequestLateWaiveID
        {
            get { return m_RequestLateWaiveID; }
            set { m_RequestLateWaiveID = value; modify("RequestLateWaiveID"); }
        }

        //  For Synchronize Use only
        protected string m_SynID;
        [DBField("SynID"), TextSearch, Export(false)]
        public string SynID
        {
            get { return m_SynID; }
            set { m_SynID = value; modify("SynID"); }
        }

    }
}
