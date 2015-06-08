using System;
using System.Collections;
using System.Text;
using HROne.DataAccess;
////using perspectivemind.validation;

namespace HROne.Lib.Entities
{
    [DBClass("LeaveApplicationCancel")]
    public class ELeaveApplicationCancel : BaseObject
    {

        public static DBManager db = new DBManager(typeof(ELeaveApplicationCancel));

        protected int m_LeaveAppCancelID;
        [DBField("LeaveAppCancelID", true, true), TextSearch, Export(false)]
        public int LeaveAppCancelID
        {
            get { return m_LeaveAppCancelID; }
            set { m_LeaveAppCancelID = value; modify("LeaveAppCancelID"); }
        }

        protected int m_EmpID;
        [DBField("EmpID"), TextSearch, Export(false), Required]
        public int EmpID
        {
            get { return m_EmpID; }
            set { m_EmpID = value; modify("EmpID"); }
        }

        protected int m_LeaveAppID;
        [DBField("LeaveAppID"), TextSearch, Export(false), Required]
        public int LeaveAppID
        {
            get { return m_LeaveAppID; }
            set { m_LeaveAppID = value; modify("LeaveAppID"); }
        }

        protected DateTime m_LeaveAppCancelCreateDateTime;
        [DBField("LeaveAppCancelCreateDateTime"), TextSearch, MaxLength(10), Export(false), Required]
        public DateTime LeaveAppCancelCreateDateTime
        {
            get { return m_LeaveAppCancelCreateDateTime; }
            set { m_LeaveAppCancelCreateDateTime = value; modify("LeaveAppCancelCreateDateTime"); }
        }

        protected string m_LeaveAppCancelReason;
        [DBField("LeaveAppCancelReason"), TextSearch, Export(false)]
        public string LeaveAppCancelReason
        {
            get { return m_LeaveAppCancelReason; }
            set { m_LeaveAppCancelReason = value; modify("LeaveAppCancelReason"); }
        }

        protected int m_EmpRequestID;
        [DBField("EmpRequestID"), TextSearch, Export(false)]
        public int EmpRequestID
        {
            get { return m_EmpRequestID; }
            set { m_EmpRequestID = value; modify("EmpRequestID"); }
        }

        protected int m_RequestLeaveAppCancelID;
        [DBField("RequestLeaveAppCancelID"), TextSearch, Export(false)]
        public int RequestLeaveAppCancelID
        {
            get { return m_RequestLeaveAppCancelID; }
            set { m_RequestLeaveAppCancelID = value; modify("RequestLeaveAppCancelID"); }
        }
    }
}
