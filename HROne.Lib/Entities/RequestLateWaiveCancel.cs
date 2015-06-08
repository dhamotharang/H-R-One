using System;
using System.Collections;
using System.Text;
using HROne.DataAccess;

namespace HROne.Lib.Entities
{
    [DBClass("RequestLateWaiveCancel")]
    public class ERequestLateWaiveCancel : BaseObject
    {

        public static DBManager db = new DBManager(typeof(ERequestLateWaiveCancel));

        protected int m_RequestLateWaiveCancelID;
        [DBField("RequestLateWaiveCancelID", true, true), TextSearch, Export(false)]
        public int RequestLateWaiveCancelID
        {
            get { return m_RequestLateWaiveCancelID; }
            set { m_RequestLateWaiveCancelID = value; modify("RequestLateWaiveCancelID"); }
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

        protected DateTime m_RequestLateWaiveCancelCreateDateTime;
        [DBField("RequestLateWaiveCancelCreateDateTime"), TextSearch, MaxLength(10), Export(false), Required]
        public DateTime RequestLateWaiveCancelCreateDateTime
        {
            get { return m_RequestLateWaiveCancelCreateDateTime; }
            set { m_RequestLateWaiveCancelCreateDateTime = value; modify("RequestLateWaiveCancelCreateDateTime"); }
        }

        protected string m_RequestLateWaiveCancelReason;
        [DBField("RequestLateWaiveCancelReason"), TextSearch, Export(false)]
        public string RequestLateWaiveCancelReason
        {
            get { return m_RequestLateWaiveCancelReason; }
            set { m_RequestLateWaiveCancelReason = value; modify("RequestLateWaiveCancelReason"); }
        }
    }
}
