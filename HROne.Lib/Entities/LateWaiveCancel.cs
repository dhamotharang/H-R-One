using System;
using System.Collections;
using System.Text;
using HROne.DataAccess;

namespace HROne.Lib.Entities
{
    [DBClass("LateWaiveCancel")]
    public class ELateWaiveCancel : BaseObject
    {
        public static DBManager db = new DBManager(typeof(ELateWaiveCancel));

        protected int m_LateWaiveCancelID;
        [DBField("LateWaiveCancelID", true, true), TextSearch, Export(false)]
        public int LateWaiveCancelID
        {
            get { return m_LateWaiveCancelID; }
            set { m_LateWaiveCancelID = value; modify("LateWaiveCancelID"); }
        }

        protected int m_EmpID;
        [DBField("EmpID"), TextSearch, Export(false), Required]
        public int EmpID
        {
            get { return m_EmpID; }
            set { m_EmpID = value; modify("EmpID"); }
        }

        protected int m_LateWaiveID;
        [DBField("LateWaiveID"), TextSearch, Export(false), Required]
        public int LateWaiveID
        {
            get { return m_LateWaiveID; }
            set { m_LateWaiveID = value; modify("LateWaiveID"); }
        }

        protected DateTime m_LateWaiveCancelCreateDateTime;
        [DBField("LateWaiveCancelCreateDateTime"), TextSearch, MaxLength(10), Export(false), Required]
        public DateTime LateWaiveCancelCreateDateTime
        {
            get { return m_LateWaiveCancelCreateDateTime; }
            set { m_LateWaiveCancelCreateDateTime = value; modify("LateWaiveCancelCreateDateTime"); }
        }

        protected string m_LateWaiveCancelReason;
        [DBField("LateWaiveCancelReason"), TextSearch, Export(false)]
        public string LateWaiveCancelReason
        {
            get { return m_LateWaiveCancelReason; }
            set { m_LateWaiveCancelReason = value; modify("LateWaiveCancelReason"); }
        }

        protected int m_EmpRequestID;
        [DBField("EmpRequestID"), TextSearch, Export(false)]
        public int EmpRequestID
        {
            get { return m_EmpRequestID; }
            set { m_EmpRequestID = value; modify("EmpRequestID"); }
        }

        protected int m_RequestLateWaiveCancelID;
        [DBField("RequestLateWaiveCancelID"), TextSearch, Export(false)]
        public int RequestLateWaiveCancelID
        {
            get { return m_RequestLateWaiveCancelID; }
            set { m_RequestLateWaiveCancelID = value; modify("RequestLateWaiveCancelID"); }
        }
    }
}
