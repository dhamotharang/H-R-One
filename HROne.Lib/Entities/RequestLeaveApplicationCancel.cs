using System;
using System.Collections;
using System.Text;
using HROne.DataAccess;
////using perspectivemind.validation;

namespace HROne.Lib.Entities
{
    [DBClass("RequestLeaveApplicationCancel")]
    public class ERequestLeaveApplicationCancel : BaseObject
    {

        public static DBManager db = new DBManager(typeof(ERequestLeaveApplicationCancel));

        protected int m_RequestLeaveAppCancelID;
        [DBField("RequestLeaveAppCancelID", true, true), TextSearch, Export(false)]
        public int RequestLeaveAppCancelID
        {
            get { return m_RequestLeaveAppCancelID; }
            set { m_RequestLeaveAppCancelID = value; modify("RequestLeaveAppCancelID"); }
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

        protected DateTime m_RequestLeaveAppCancelCreateDateTime;
        [DBField("RequestLeaveAppCancelCreateDateTime"), TextSearch, MaxLength(10), Export(false), Required]
        public DateTime RequestLeaveAppCancelCreateDateTime
        {
            get { return m_RequestLeaveAppCancelCreateDateTime; }
            set { m_RequestLeaveAppCancelCreateDateTime = value; modify("RequestLeaveAppCancelCreateDateTime"); }
        }

        protected string m_RequestLeaveAppCancelReason;
        [DBField("RequestLeaveAppCancelReason"), TextSearch, Export(false)]
        public string RequestLeaveAppCancelReason
        {
            get { return m_RequestLeaveAppCancelReason; }
            set { m_RequestLeaveAppCancelReason = value; modify("RequestLeaveAppCancelReason"); }
        }
    }
}
