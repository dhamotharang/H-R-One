using System;
using System.Collections;
using System.Text;
using HROne.DataAccess;

namespace HROne.Lib.Entities
{
    [DBClass("OTClaim")]
    public class EOTClaim : BaseObjectWithRecordInfo
    {
        public static DBManager db = new DBManagerWithRecordInfo(typeof(EOTClaim));

        protected int m_OTClaimID;
        [DBField("OTClaimID", true, true), TextSearch, Export(false)]
        public int OTClaimID
        {
            get { return m_OTClaimID; }
            set { m_OTClaimID = value; modify("OTClaimID"); }
        }
        protected int m_EmpID;
        [DBField("EmpID"), TextSearch, Export(false)]
        public int EmpID
        {
            get { return m_EmpID; }
            set { m_EmpID = value; modify("EmpID"); }
        }
        protected DateTime m_OTClaimDateFrom;
        [DBField("OTClaimDateFrom"), TextSearch, MaxLength(10), Export(false), Required]
        public DateTime OTClaimDateFrom
        {
            get { return m_OTClaimDateFrom; }
            set { m_OTClaimDateFrom = value; modify("OTClaimDateFrom"); }
        }
        protected DateTime m_OTClaimDateTo;
        [DBField("OTClaimDateTo"), TextSearch, MaxLength(10), Export(false), Required]
        public DateTime OTClaimDateTo
        {
            get { return m_OTClaimDateTo; }
            set { m_OTClaimDateTo = value; modify("OTClaimDateTo"); }
        }
        protected DateTime m_OTClaimTimeFrom;
        [DBField("OTClaimTimeFrom", "HH:mm"), TextSearch, MaxLength(5), Export(false)]
        public DateTime OTClaimTimeFrom
        {
            get { return m_OTClaimTimeFrom; }
            set { m_OTClaimTimeFrom = value; modify("OTClaimTimeFrom"); }
        }
        protected DateTime m_OTClaimTimeTo;
        [DBField("OTClaimTimeTo", "HH:mm"), TextSearch, MaxLength(5), Export(false)]
        public DateTime OTClaimTimeTo
        {
            get { return m_OTClaimTimeTo; }
            set { m_OTClaimTimeTo = value; modify("OTClaimTimeTo"); }
        }
        protected double m_OTClaimHours;
        [DBField("OTClaimHours", "0.####"), TextSearch, MaxLength(7), Export(false)]
        public double OTClaimHours
        {
            get { return m_OTClaimHours; }
            set { m_OTClaimHours = value; modify("OTClaimHours"); }
        }
        protected string m_OTClaimRemark;
        [DBField("OTClaimRemark"), TextSearch, Export(false)]
        public string OTClaimRemark
        {
            get { return m_OTClaimRemark; }
            set { m_OTClaimRemark = value; modify("OTClaimRemark"); }
        }

        protected int m_OTClaimCancelID;
        [DBField("OTClaimCancelID"), TextSearch, Export(false)]
        public int OTClaimCancelID
        {
            get { return m_OTClaimCancelID; }
            set { m_OTClaimCancelID = value; modify("OTClaimCancelID"); }
        }

        protected int m_EmpRequestID;
        [DBField("EmpRequestID"), TextSearch, Export(false)]
        public int EmpRequestID
        {
            get { return m_EmpRequestID; }
            set { m_EmpRequestID = value; modify("EmpRequestID"); }
        }

        protected int m_RequestOTClaimID;
        [DBField("RequestOTClaimID"), TextSearch, Export(false)]
        public int RequestOTClaimID
        {
            get { return m_RequestOTClaimID; }
            set { m_RequestOTClaimID = value; modify("RequestOTClaimID"); }
        }

        //  For Synchronize Use only
        protected string m_SynID;
        [DBField("SynID"), TextSearch, Export(false)]
        public string SynID
        {
            get { return m_SynID; }
            set { m_SynID = value; modify("SynID"); }
        }

        public bool IsOverlapOTClaim(DatabaseConnection dbConn, out ArrayList OverlapOTClaimList)
        {
            OverlapOTClaimList = new ArrayList();

            DBFilter overlapCheckingFilter = new DBFilter();
            overlapCheckingFilter.add(new Match("EmpID", EmpID));
            overlapCheckingFilter.add(new Match("OTClaimID", "<>", OTClaimID));
            overlapCheckingFilter.add(new Match("OTClaimDateFrom", "<=", OTClaimDateTo));
            overlapCheckingFilter.add(new Match("OTClaimDateTo", ">=", OTClaimDateFrom));

            OR leaveCancelIDOrTerm = new OR();
            leaveCancelIDOrTerm.add(new NullTerm("OTClaimCancelID"));
            leaveCancelIDOrTerm.add(new Match("OTClaimCancelID", "<=", 0));
            overlapCheckingFilter.add(leaveCancelIDOrTerm);

            DateTime newOTClaimTimeFrom = OTClaimDateFrom.Date.Add(new TimeSpan(OTClaimTimeFrom.Hour, OTClaimTimeFrom.Minute, OTClaimTimeFrom.Second));
            DateTime newOTClaimTimeTo = OTClaimDateTo.Date.Add(new TimeSpan(OTClaimTimeTo.Hour, OTClaimTimeTo.Minute, OTClaimTimeTo.Second));
            while (newOTClaimTimeFrom > newOTClaimTimeTo)
                newOTClaimTimeTo = newOTClaimTimeTo.AddDays(1);

            overlapCheckingFilter = new DBFilter();
            overlapCheckingFilter.add(new Match("EmpID", EmpID));
            overlapCheckingFilter.add(new Match("OTClaimID", "<>", OTClaimID));
            overlapCheckingFilter.add(new Match("OTClaimDateFrom", "<=", OTClaimDateTo));
            overlapCheckingFilter.add(new Match("OTClaimDateTo", ">=", OTClaimDateFrom));
            ArrayList OTClaimList = db.select(dbConn, overlapCheckingFilter);
            foreach (EOTClaim oldOTClaim in OTClaimList)
            {
                DateTime oldOTClaimTimeFrom = oldOTClaim.OTClaimDateFrom.Date.Add(new TimeSpan(oldOTClaim.OTClaimTimeFrom.Hour, oldOTClaim.OTClaimTimeFrom.Minute, oldOTClaim.OTClaimTimeFrom.Second));
                DateTime oldOTClaimTimeTo = oldOTClaim.OTClaimDateTo.Date.Add(new TimeSpan(oldOTClaim.OTClaimTimeTo.Hour, oldOTClaim.OTClaimTimeTo.Minute, oldOTClaim.OTClaimTimeTo.Second));
                while (oldOTClaimTimeFrom > oldOTClaimTimeTo)
                    oldOTClaimTimeTo = oldOTClaimTimeTo.AddDays(1);

                if (!newOTClaimTimeFrom.Equals(newOTClaimTimeTo) && !oldOTClaimTimeFrom.Equals(oldOTClaimTimeTo))
                    //  Only check if the following case exists
                    // Time A From------- Time B From ---------- Time B To ------------Time A To
                    if (newOTClaimTimeFrom <= oldOTClaimTimeFrom && oldOTClaimTimeTo <= newOTClaimTimeTo || oldOTClaimTimeFrom <= newOTClaimTimeFrom && newOTClaimTimeTo <= oldOTClaimTimeTo)
                        OverlapOTClaimList.Add(oldOTClaim);
            }
            if (OverlapOTClaimList.Count > 0)
                return true;
            else
                return false;
        }

    }
}
