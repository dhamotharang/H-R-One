using System;
using System.Collections;
using System.Text;
using HROne.DataAccess;

namespace HROne.Lib.Entities
{
    [DBClass("OTClaimCancel")]
    public class EOTClaimCancel : BaseObject
    {

        public static DBManager db = new DBManager(typeof(EOTClaimCancel));

        protected int m_OTClaimCancelID;
        [DBField("OTClaimCancelID", true, true), TextSearch, Export(false)]
        public int OTClaimCancelID
        {
            get { return m_OTClaimCancelID; }
            set { m_OTClaimCancelID = value; modify("OTClaimCancelID"); }
        }

        protected int m_EmpID;
        [DBField("EmpID"), TextSearch, Export(false), Required]
        public int EmpID
        {
            get { return m_EmpID; }
            set { m_EmpID = value; modify("EmpID"); }
        }

        protected int m_OTClaimID;
        [DBField("OTClaimID"), TextSearch, Export(false), Required]
        public int OTClaimID
        {
            get { return m_OTClaimID; }
            set { m_OTClaimID = value; modify("OTClaimID"); }
        }

        protected DateTime m_OTClaimCancelCreateDateTime;
        [DBField("OTClaimCancelCreateDateTime"), TextSearch, MaxLength(10), Export(false), Required]
        public DateTime OTClaimCancelCreateDateTime
        {
            get { return m_OTClaimCancelCreateDateTime; }
            set { m_OTClaimCancelCreateDateTime = value; modify("OTClaimCancelCreateDateTime"); }
        }

        protected string m_OTClaimCancelReason;
        [DBField("OTClaimCancelReason"), TextSearch, Export(false)]
        public string OTClaimCancelReason
        {
            get { return m_OTClaimCancelReason; }
            set { m_OTClaimCancelReason = value; modify("OTClaimCancelReason"); }
        }

        protected int m_EmpRequestID;
        [DBField("EmpRequestID"), TextSearch, Export(false)]
        public int EmpRequestID
        {
            get { return m_EmpRequestID; }
            set { m_EmpRequestID = value; modify("EmpRequestID"); }
        }

        protected int m_RequestOTClaimCancelID;
        [DBField("RequestOTClaimCancelID"), TextSearch, Export(false)]
        public int RequestOTClaimCancelID
        {
            get { return m_RequestOTClaimCancelID; }
            set { m_RequestOTClaimCancelID = value; modify("RequestOTClaimCancelID"); }
        }
    }
}
