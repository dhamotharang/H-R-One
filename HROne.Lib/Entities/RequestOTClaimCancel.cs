using System;
using System.Collections;
using System.Text;
using HROne.DataAccess;

namespace HROne.Lib.Entities
{
    [DBClass("RequestOTClaimCancel")]
    public class ERequestOTClaimCancel : BaseObject
    {

        public static DBManager db = new DBManager(typeof(ERequestOTClaimCancel));

        protected int m_RequestOTClaimCancelID;
        [DBField("RequestOTClaimCancelID", true, true), TextSearch, Export(false)]
        public int RequestOTClaimCancelID
        {
            get { return m_RequestOTClaimCancelID; }
            set { m_RequestOTClaimCancelID = value; modify("RequestOTClaimCancelID"); }
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

        protected DateTime m_RequestOTClaimCancelCreateDateTime;
        [DBField("RequestOTClaimCancelCreateDateTime"), TextSearch, MaxLength(10), Export(false), Required]
        public DateTime RequestOTClaimCancelCreateDateTime
        {
            get { return m_RequestOTClaimCancelCreateDateTime; }
            set { m_RequestOTClaimCancelCreateDateTime = value; modify("RequestOTClaimCancelCreateDateTime"); }
        }

        protected string m_RequestOTClaimCancelReason;
        [DBField("RequestOTClaimCancelReason"), TextSearch, Export(false)]
        public string RequestOTClaimCancelReason
        {
            get { return m_RequestOTClaimCancelReason; }
            set { m_RequestOTClaimCancelReason = value; modify("RequestOTClaimCancelReason"); }
        }
    }
}
