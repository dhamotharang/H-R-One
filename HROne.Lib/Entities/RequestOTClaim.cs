using System;
using System.Collections;
using System.Text;
using HROne.DataAccess;

namespace HROne.Lib.Entities
{
    [DBClass("RequestOTClaim")]
    public class ERequestOTClaim : BaseObject
    {

        public static DBManager db = new DBManager(typeof(ERequestOTClaim));

        protected int m_RequestOTClaimID;
        [DBField("RequestOTClaimID", true, true), TextSearch, Export(false)]
        public int RequestOTClaimID
        {
            get { return m_RequestOTClaimID; }
            set { m_RequestOTClaimID = value; modify("RequestOTClaimID"); }
        }

        protected int m_EmpID;
        [DBField("EmpID"), TextSearch, Export(false), Required]
        public int EmpID
        {
            get { return m_EmpID; }
            set { m_EmpID = value; modify("EmpID"); }
        }

        protected DateTime m_RequestOTClaimPeriodFrom;
        [DBField("RequestOTClaimPeriodFrom"), TextSearch, MaxLength(10), Export(false), Required]
        public DateTime RequestOTClaimPeriodFrom
        {
            get { return m_RequestOTClaimPeriodFrom; }
            set { m_RequestOTClaimPeriodFrom = value; modify("RequestOTClaimPeriodFrom"); }
        }

        protected DateTime m_RequestOTClaimPeriodTo;
        [DBField("RequestOTClaimPeriodTo"), TextSearch, MaxLength(10), Export(false), Required]
        public DateTime RequestOTClaimPeriodTo
        {
            get { return m_RequestOTClaimPeriodTo; }
            set { m_RequestOTClaimPeriodTo = value; modify("RequestOTClaimPeriodTo"); }
        }

        protected DateTime m_RequestOTClaimHourFrom;
        [DBField("RequestOTClaimHourFrom", "HH:mm"), TextSearch, MaxLength(5), Export(false)]
        public DateTime RequestOTClaimHourFrom
        {
            get { return m_RequestOTClaimHourFrom; }
            set { m_RequestOTClaimHourFrom = value; modify("RequestOTClaimHourFrom"); }
        }

        protected DateTime m_RequestOTClaimHourTo;
        [DBField("RequestOTClaimHourTo", "HH:mm"), TextSearch, MaxLength(5), Export(false)]
        public DateTime RequestOTClaimHourTo
        {
            get { return m_RequestOTClaimHourTo; }
            set { m_RequestOTClaimHourTo = value; modify("RequestOTClaimHourTo"); }
        }

        protected double m_RequestOTHours;
        [DBField("RequestOTHours", "0.####"), TextSearch, MaxLength(7), Export(false)]
        public double RequestOTHours
        {
            get { return m_RequestOTHours; }
            set { m_RequestOTHours = value; modify("RequestOTHours"); }
        }

        protected string m_RequestOTClaimRemark;
        [DBField("RequestOTClaimRemark"), TextSearch, Export(false)]
        public string RequestOTClaimRemark
        {
            get { return m_RequestOTClaimRemark; }
            set { m_RequestOTClaimRemark = value; modify("RequestOTClaimRemark"); }
        }

        protected DateTime m_RequestOTClaimEffectiveDate;
        [DBField("RequestOTClaimEffectiveDate", "yyyy-MM-dd"), TextSearch, MaxLength(10), Export(false), Required]
        public DateTime RequestOTClaimEffectiveDate
        {
            get { return m_RequestOTClaimEffectiveDate; }
            set { m_RequestOTClaimEffectiveDate = value; modify("RequestOTClaimEffectiveDate"); }
        }

        protected DateTime m_RequestOTClaimDateExpiry;
        [DBField("RequestOTClaimDateExpiry", "yyyy-MM-dd"), TextSearch, MaxLength(10), Export(false), Required]
        public DateTime RequestOTClaimDateExpiry
        {
            get { return m_RequestOTClaimDateExpiry; }
            set { m_RequestOTClaimDateExpiry = value; modify("RequestOTClaimDateExpiry"); }
        }
        
        protected DateTime m_RequestOTClaimCreateDate;
        [DBField("RequestOTClaimCreateDate"), TextSearch, Export(false)]
        public DateTime RequestOTClaimCreateDate
        {
            get { return m_RequestOTClaimCreateDate; }
            set { m_RequestOTClaimCreateDate = value; modify("RequestOTClaimCreateDate"); }
        }

        public bool IsOverlapOTClaim(DatabaseConnection dbConn, out ArrayList OverlapOTClaimList)
        {

            OverlapOTClaimList = new ArrayList();

            DBFilter overlapCheckingFilter = new DBFilter();
            overlapCheckingFilter.add(new Match("EmpID", EmpID));
            overlapCheckingFilter.add(new Match("RequestOTClaimID", "<>", RequestOTClaimID));
            overlapCheckingFilter.add(new Match("RequestOTClaimPeriodFrom", "<=", RequestOTClaimPeriodTo));
            overlapCheckingFilter.add(new Match("RequestOTClaimPeriodTo", ">=", RequestOTClaimPeriodFrom));
            ArrayList overlapDailyOTClaimList = db.select(dbConn, overlapCheckingFilter);
            if (overlapDailyOTClaimList.Count > 0)
            {
                foreach (ERequestOTClaim previousRequestOTClaim in overlapDailyOTClaimList)
                {
                    //------------------------------------------------------
                    //Check record status from the EmpRequest table
                    DBFilter filterStatus = new DBFilter();

                    filterStatus.add(new Match("EmpID", EmpID));
                    filterStatus.add(new Match("EmpRequestRecordID", previousRequestOTClaim.RequestOTClaimID));
                    filterStatus.add(new Match("EmpRequestType", EEmpRequest.TYPE_EEOTCLAIM));

                    filterStatus.add(new Match("EmpRequestStatus", "<>", EEmpRequest.STATUS_CANCELLED));
                    filterStatus.add(new Match("EmpRequestStatus", "<>", EEmpRequest.STATUS_REJECTED));
                    filterStatus.add(new Match("EmpRequestStatus", "<>", EEmpRequest.STATUS_APPROVED));

                    if (EEmpRequest.db.count(dbConn, filterStatus) > 0)
                    {
                        OverlapOTClaimList.Add(previousRequestOTClaim);
                    }
                    //------------------------------------------------------
                }
            }
            DateTime newOTClaimTimeFrom = RequestOTClaimPeriodFrom.Date.Add(new TimeSpan(RequestOTClaimHourFrom.Hour, RequestOTClaimHourFrom.Minute, RequestOTClaimHourFrom.Second));
            DateTime newOTClaimTimeTo = RequestOTClaimPeriodTo.Date.Add(new TimeSpan(RequestOTClaimHourTo.Hour, RequestOTClaimHourTo.Minute, RequestOTClaimHourTo.Second));
            while (newOTClaimTimeFrom > newOTClaimTimeTo)
                newOTClaimTimeTo = newOTClaimTimeTo.AddDays(1);

            overlapCheckingFilter = new DBFilter();
            overlapCheckingFilter.add(new Match("EmpID", EmpID));
            overlapCheckingFilter.add(new Match("RequestOTClaimID", "<>", RequestOTClaimID));
            overlapCheckingFilter.add(new Match("RequestOTClaimPeriodFrom", "<=", RequestOTClaimPeriodTo));
            overlapCheckingFilter.add(new Match("RequestOTClaimPeriodTo", ">=", RequestOTClaimPeriodFrom));
            ArrayList OTClaimList = db.select(dbConn, overlapCheckingFilter);
            foreach (ERequestOTClaim previousRequestOTClaim in OTClaimList)
            {
                //------------------------------------------------------
                //Check record status from the EmpRequest table
                DBFilter filterStatus = new DBFilter();

                filterStatus.add(new Match("EmpID", EmpID));
                filterStatus.add(new Match("EmpRequestRecordID", previousRequestOTClaim.RequestOTClaimID));
                filterStatus.add(new Match("EmpRequestType", EEmpRequest.TYPE_EEOTCLAIM));
                filterStatus.add(new Match("EmpRequestStatus", "<>", EEmpRequest.STATUS_CANCELLED));
                filterStatus.add(new Match("EmpRequestStatus", "<>", EEmpRequest.STATUS_REJECTED));
                filterStatus.add(new Match("EmpRequestStatus", "<>", EEmpRequest.STATUS_APPROVED));

                if (EEmpRequest.db.count(dbConn, filterStatus) > 0)
                {

                    DateTime oldOTClaimTimeFrom = previousRequestOTClaim.RequestOTClaimPeriodFrom.Date.Add(new TimeSpan(previousRequestOTClaim.RequestOTClaimHourFrom.Hour, previousRequestOTClaim.RequestOTClaimHourFrom.Minute, previousRequestOTClaim.RequestOTClaimHourFrom.Second));
                    DateTime oldOTClaimTimeTo = previousRequestOTClaim.RequestOTClaimPeriodTo.Date.Add(new TimeSpan(previousRequestOTClaim.RequestOTClaimHourTo.Hour, previousRequestOTClaim.RequestOTClaimHourTo.Minute, previousRequestOTClaim.RequestOTClaimHourTo.Second));
                    while (oldOTClaimTimeFrom > oldOTClaimTimeTo)
                        oldOTClaimTimeTo = oldOTClaimTimeTo.AddDays(1);

                    if (!newOTClaimTimeFrom.Equals(newOTClaimTimeTo) && !oldOTClaimTimeFrom.Equals(oldOTClaimTimeTo))
                        //  Only check if the following case exists
                        // Time A From------- Time B From ---------- Time B To ------------Time A To
                        if (newOTClaimTimeFrom <= oldOTClaimTimeFrom && oldOTClaimTimeTo <= newOTClaimTimeTo || oldOTClaimTimeFrom <= newOTClaimTimeFrom && newOTClaimTimeTo <= oldOTClaimTimeTo)
                            OverlapOTClaimList.Add(previousRequestOTClaim);
                }
            }
            //  Also Check CL Requisition
            ArrayList OverlapOutstandardOTClaimList;
            EOTClaim OTClaim = new EOTClaim();
            OTClaim.EmpID = EmpID;
            OTClaim.OTClaimDateFrom = RequestOTClaimPeriodFrom;
            OTClaim.OTClaimDateTo = RequestOTClaimPeriodTo;
            OTClaim.OTClaimTimeFrom = RequestOTClaimHourFrom;
            OTClaim.OTClaimTimeTo = RequestOTClaimHourTo;
            if (OTClaim.IsOverlapOTClaim(dbConn, out OverlapOutstandardOTClaimList))
            {
                OverlapOTClaimList.AddRange(OverlapOutstandardOTClaimList);
            }

            if (OverlapOTClaimList.Count > 0)
                return true;
            else
                return false;
        }

        public static ERequestOTClaim CreateDummyRequestOTClaim(EOTClaim OTClaim)
        {
            ERequestOTClaim requestOTClaim = new ERequestOTClaim();
            requestOTClaim.EmpID = OTClaim.EmpID;
            requestOTClaim.RequestOTClaimPeriodFrom = OTClaim.OTClaimDateFrom;
            requestOTClaim.RequestOTClaimPeriodTo = OTClaim.OTClaimDateTo;
            requestOTClaim.RequestOTHours = OTClaim.OTClaimHours;
            requestOTClaim.RequestOTClaimRemark = OTClaim.OTClaimRemark;
            requestOTClaim.RequestOTClaimHourFrom = OTClaim.OTClaimTimeFrom;
            requestOTClaim.RequestOTClaimHourTo = OTClaim.OTClaimTimeTo;
            return requestOTClaim;
        }
    }
}
