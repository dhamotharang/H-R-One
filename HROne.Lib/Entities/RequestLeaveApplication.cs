using System;
using System.Collections;
using System.Text;
using HROne.DataAccess;
////using perspectivemind.validation;

namespace HROne.Lib.Entities
{
    [DBClass("RequestLeaveApplication")]
    public class ERequestLeaveApplication : BaseObject
    {

        public static DBManager db = new DBManager(typeof(ERequestLeaveApplication));

        protected int m_RequestLeaveAppID;
        [DBField("RequestLeaveAppID", true, true), TextSearch, Export(false)]
        public int RequestLeaveAppID
        {
            get { return m_RequestLeaveAppID; }
            set { m_RequestLeaveAppID = value; modify("RequestLeaveAppID"); }
        }

        protected int m_EmpID;
        [DBField("EmpID"), TextSearch, Export(false), Required]
        public int EmpID
        {
            get { return m_EmpID; }
            set { m_EmpID = value; modify("EmpID"); }
        }

        protected int m_RequestLeaveCodeID;
        [DBField("RequestLeaveCodeID"), TextSearch, Export(false), Required]
        public int RequestLeaveCodeID
        {
            get { return m_RequestLeaveCodeID; }
            set { m_RequestLeaveCodeID = value; modify("RequestLeaveCodeID"); }
        }

        protected string m_RequestLeaveAppUnit;
        [DBField("RequestLeaveAppUnit"), TextSearch, MaxLength(50, 10), Export(false), Required]
        public string RequestLeaveAppUnit
        {
            get { return m_RequestLeaveAppUnit; }
            set { m_RequestLeaveAppUnit = value; modify("RequestLeaveAppUnit"); }
        }

        protected DateTime m_RequestLeaveAppDateFrom;
        [DBField("RequestLeaveAppDateFrom"), TextSearch, MaxLength(10), Export(false), Required]
        public DateTime RequestLeaveAppDateFrom
        {
            get { return m_RequestLeaveAppDateFrom; }
            set { m_RequestLeaveAppDateFrom = value; modify("RequestLeaveAppDateFrom"); }
        }

        protected string m_RequestLeaveAppDateFromAM;
        [DBField("RequestLeaveAppDateFromAM"), TextSearch, MaxLength(2), Export(false), Required]
        public string RequestLeaveAppDateFromAM
        {
            get { return m_RequestLeaveAppDateFromAM; }
            set { m_RequestLeaveAppDateFromAM = value; modify("RequestLeaveAppDateFromAM"); }
        }

        protected DateTime m_RequestLeaveAppDateTo;
        [DBField("RequestLeaveAppDateTo"), TextSearch, MaxLength(10), Export(false), Required]
        public DateTime RequestLeaveAppDateTo
        {
            get { return m_RequestLeaveAppDateTo; }
            set { m_RequestLeaveAppDateTo = value; modify("RequestLeaveAppDateTo"); }
        }

        protected string m_RequestLeaveAppDateToAM;
        [DBField("RequestLeaveAppDateToAM"), TextSearch, MaxLength(2), Export(false), Required]
        public string RequestLeaveAppDateToAM
        {
            get { return m_RequestLeaveAppDateToAM; }
            set { m_RequestLeaveAppDateToAM = value; modify("RequestLeaveAppDateToAM"); }
        }

        protected DateTime m_RequestLeaveAppTimeFrom;
        [DBField("RequestLeaveAppTimeFrom", "HH:mm"), TextSearch, MaxLength(5), Export(false)]
        public DateTime RequestLeaveAppTimeFrom
        {
            get { return m_RequestLeaveAppTimeFrom; }
            set { m_RequestLeaveAppTimeFrom = value; modify("RequestLeaveAppTimeFrom"); }
        }

        protected DateTime m_RequestLeaveAppTimeTo;
        [DBField("RequestLeaveAppTimeTo", "HH:mm"), TextSearch, MaxLength(5), Export(false)]
        public DateTime RequestLeaveAppTimeTo
        {
            get { return m_RequestLeaveAppTimeTo; }
            set { m_RequestLeaveAppTimeTo = value; modify("RequestLeaveAppTimeTo"); }
        }

        protected double m_RequestLeaveDays;
        [DBField("RequestLeaveDays", "0.0000"), TextSearch, MaxLength(6), Export(false), Required]
        public double RequestLeaveDays
        {
            get { return m_RequestLeaveDays; }
            set { m_RequestLeaveDays = value; modify("RequestLeaveDays"); }
        }
        protected double m_RequestLeaveAppHours;
        [DBField("RequestLeaveAppHours", "0.####"), TextSearch, MaxLength(6), Export(false)]
        public double RequestLeaveAppHours
        {
            get { return m_RequestLeaveAppHours; }
            set { m_RequestLeaveAppHours = value; modify("RequestLeaveAppHours"); }
        }
        protected string m_RequestLeaveAppRemark;
        [DBField("RequestLeaveAppRemark"), TextSearch, Export(false)]
        public string RequestLeaveAppRemark
        {
            get { return m_RequestLeaveAppRemark; }
            set { m_RequestLeaveAppRemark = value; modify("RequestLeaveAppRemark"); }
        }
        protected bool m_RequestLeaveAppHasMedicalCertificate;
        [DBField("RequestLeaveAppHasMedicalCertificate"), TextSearch, Export(false)]
        public bool RequestLeaveAppHasMedicalCertificate
        {
            get { return m_RequestLeaveAppHasMedicalCertificate; }
            set { m_RequestLeaveAppHasMedicalCertificate = value; modify("RequestLeaveAppHasMedicalCertificate"); }
        }

        protected DateTime m_RequestLeaveAppCreateDate;
        [DBField("RequestLeaveAppCreateDate"), TextSearch, Export(false)]
        public DateTime RequestLeaveAppCreateDate
        {
            get { return m_RequestLeaveAppCreateDate; }
            set { m_RequestLeaveAppCreateDate = value; modify("RequestLeaveAppCreateDate"); }
        }

        public bool IsOverlapLeaveApplication(DatabaseConnection dbConn, out ArrayList OverlapLeaveApplicationList)
        {

            OverlapLeaveApplicationList = new ArrayList();

            DBFilter overlapCheckingFilter = new DBFilter();
            overlapCheckingFilter.add(new Match("EmpID", EmpID));
            overlapCheckingFilter.add(new Match("RequestLeaveAppID", "<>", RequestLeaveAppID));
            overlapCheckingFilter.add(new Match("RequestLeaveAppDateFrom", "<=", RequestLeaveAppDateTo));
            overlapCheckingFilter.add(new Match("RequestLeaveAppDateTo", ">=", RequestLeaveAppDateFrom));
            if (RequestLeaveAppUnit.Equals("H"))
                overlapCheckingFilter.add(new Match("RequestLeaveAppUnit", "D"));
            //// Start 0000201, Ricky S0, 2015-06-01
            //else if (RequestLeaveAppUnit.Equals("D"))
            //{   
            //    if (RequestLeaveAppDateToAM == "AM")
            //    {
            //        OR m_orFrom = new OR();
            //        m_orFrom.add(new Match("RequestLeaveAppDateFromAM", "AM"));
            //        m_orFrom.add(new NullTerm("RequestLeaveAppDateFrom"));
            //        overlapCheckingFilter.add(m_orFrom);
            //    }
            //    else if (RequestLeaveAppDateFromAM == "PM")
            //    {
            //        OR m_orTo = new OR();
            //        m_orFrom.add(new Match("RequestLeaveAppDateToAM", "PM"));
            //        m_orFrom.add(new NullTerm("RequestLeaveAppDateTo"));
            //        overlapCheckingFilter.add(m_orTo);
            //    }
            //}
            //// End 0000201, Ricky S0, 2015-06-01
            else if (RequestLeaveAppUnit.Equals("A"))
            {
                //  AM Leave Application
                OR orLeaveAppUnit = new OR();
                orLeaveAppUnit.add(new Match("RequestLeaveAppUnit", "D"));
                orLeaveAppUnit.add(new Match("RequestLeaveAppUnit", "A"));
                overlapCheckingFilter.add(orLeaveAppUnit);
            }
            else if (RequestLeaveAppUnit.Equals("P"))
            {
                //  PM Leave Application
                OR orLeaveAppUnit = new OR();
                orLeaveAppUnit.add(new Match("RequestLeaveAppUnit", "D"));
                orLeaveAppUnit.add(new Match("RequestLeaveAppUnit", "P"));
                overlapCheckingFilter.add(orLeaveAppUnit);
            }

            // Start 0000201, Ricky So, 2015-06-01
            // remove those are not classified as overlap
            //ArrayList overlapDailyLeaveAppList = db.select(dbConn, overlapCheckingFilter);
            ArrayList overlapDailyLeaveAppList = new ArrayList();
            ArrayList potentialOverlapLeaveAppList = db.select(dbConn, overlapCheckingFilter);
            foreach (ERequestLeaveApplication potentialOverlapLeaveApp in potentialOverlapLeaveAppList)
            {
                
                if (this.RequestLeaveAppDateFrom == potentialOverlapLeaveApp.RequestLeaveAppDateTo)
                {
                    if (this.RequestLeaveAppDateFromAM == "PM" || this.RequestLeaveAppUnit == "P")
                    {
                        if (potentialOverlapLeaveApp.RequestLeaveAppDateToAM == "AM")
                            continue;
                        else if (potentialOverlapLeaveApp.RequestLeaveAppUnit == "A")
                            continue;
                    }
                    else if (this.RequestLeaveAppUnit == "A")
                    {
                        //if (potentialOverlapLeaveApp.RequestLeaveAppDateToAM == "PM")
                        //    continue;
                        //else if (potentialOverlapLeaveApp.RequestLeaveAppUnit == "P")
                        //    continue;
                    }
                    //else if (this.RequestLeaveAppUnit == "")
                    //{
                    //}
                }
                
                if (this.RequestLeaveAppDateTo == potentialOverlapLeaveApp.RequestLeaveAppDateFrom)
                {
                    if (this.RequestLeaveAppDateToAM == "AM")
                    {
                        if (potentialOverlapLeaveApp.RequestLeaveAppDateFromAM == "PM")
                            continue;
                        else if (potentialOverlapLeaveApp.RequestLeaveAppUnit == "P")
                            continue;
                    }
                    else if (this.RequestLeaveAppUnit == "P")
                    {

                    }
                    //else if (this.RequestLeaveAppUnit == "")
                    //{
                    //}
                }

                if (this.RequestLeaveAppDateFrom == potentialOverlapLeaveApp.RequestLeaveAppDateFrom)
                {
                    if (this.RequestLeaveAppUnit == "A")
                    {
                        if (potentialOverlapLeaveApp.RequestLeaveAppUnit == "P" || potentialOverlapLeaveApp.RequestLeaveAppDateFromAM == "PM")
                            continue;
                    }
                    else if (this.RequestLeaveAppUnit == "P")
                    {
                        if (potentialOverlapLeaveApp.RequestLeaveAppUnit == "A")
                            continue;
                    }
                    else if (this.RequestLeaveAppUnit == "D")
                    {
                        if (this.RequestLeaveAppDateFromAM == "PM" && (potentialOverlapLeaveApp.RequestLeaveAppDateFromAM == "AM" || potentialOverlapLeaveApp.RequestLeaveAppUnit == "A"))
                            continue;
                    }
                }

                overlapDailyLeaveAppList.Add(potentialOverlapLeaveApp);
            }
            // End 0000201, Ricky So, 2015-06-01

            if (overlapDailyLeaveAppList.Count > 0)
            {
                foreach (ERequestLeaveApplication previousRequestLeaveApp in overlapDailyLeaveAppList)
                {
                    //------------------------------------------------------
                    //Check record status from the EmpRequest table
                    DBFilter filterStatus = new DBFilter();

                    filterStatus.add(new Match("EmpID", EmpID));
                    filterStatus.add(new Match("EmpRequestRecordID", previousRequestLeaveApp.RequestLeaveAppID));
                    filterStatus.add(new Match("EmpRequestType", EEmpRequest.TYPE_EELEAVEAPP));
                    //filterStatus.add(new Match("EmpRequestStatus", "<>", EEmpRequest.STATUS_USRCANCEL));

                    //filterStatus.add(new Match("EmpRequestStatus", "<>", EEmpRequest.STATUS_FSTREJ));

                    //filterStatus.add(new Match("EmpRequestStatus", "<>", EEmpRequest.STATUS_SNDREJ));

                    //filterStatus.add(new Match("EmpRequestStatus", "<>", EEmpRequest.STATUS_SNDAPP));

                    filterStatus.add(new Match("EmpRequestStatus", "<>", EEmpRequest.STATUS_CANCELLED));
                    filterStatus.add(new Match("EmpRequestStatus", "<>", EEmpRequest.STATUS_REJECTED));
                    filterStatus.add(new Match("EmpRequestStatus", "<>", EEmpRequest.STATUS_APPROVED));

                    if (EEmpRequest.db.count(dbConn, filterStatus) > 0)
                    {
                        OverlapLeaveApplicationList.Add(previousRequestLeaveApp);
                    }

                    //------------------------------------------------------

                }
                //OverlapLeaveApplicationList.AddRange(overlapDailyLeaveAppList);
            }
            if (!RequestLeaveAppUnit.Equals("D"))
            {
                DateTime newLeaveAppTimeFrom = RequestLeaveAppDateFrom.Date.Add(new TimeSpan(RequestLeaveAppTimeFrom.Hour, RequestLeaveAppTimeFrom.Minute, RequestLeaveAppTimeFrom.Second));
                DateTime newLeaveAppTimeTo = RequestLeaveAppDateTo.Date.Add(new TimeSpan(RequestLeaveAppTimeTo.Hour, RequestLeaveAppTimeTo.Minute, RequestLeaveAppTimeTo.Second));
                while (newLeaveAppTimeFrom > newLeaveAppTimeTo)
                    newLeaveAppTimeTo = newLeaveAppTimeTo.AddDays(1);

                overlapCheckingFilter = new DBFilter();
                overlapCheckingFilter.add(new Match("EmpID", EmpID));
                overlapCheckingFilter.add(new Match("RequestLeaveAppID", "<>", RequestLeaveAppID));
                overlapCheckingFilter.add(new Match("RequestLeaveAppDateFrom", "<=", RequestLeaveAppDateTo));

                overlapCheckingFilter.add(new Match("RequestLeaveAppDateTo", ">=", RequestLeaveAppDateFrom));
                overlapCheckingFilter.add(new Match("RequestLeaveAppUnit", "<>", "D"));
                ArrayList leaveAppList = db.select(dbConn, overlapCheckingFilter);
                foreach (ERequestLeaveApplication previousRequestLeaveApp in leaveAppList)
                {
                                        //------------------------------------------------------
                    //Check record status from the EmpRequest table
                    DBFilter filterStatus = new DBFilter();

                    filterStatus.add(new Match("EmpID", EmpID));
                    filterStatus.add(new Match("EmpRequestRecordID", previousRequestLeaveApp.RequestLeaveAppID));
                    filterStatus.add(new Match("EmpRequestType", EEmpRequest.TYPE_EELEAVEAPP));
                    //filterStatus.add(new Match("EmpRequestStatus", "<>", EEmpRequest.STATUS_USRCANCEL));

                    //filterStatus.add(new Match("EmpRequestStatus", "<>", EEmpRequest.STATUS_FSTREJ));

                    //filterStatus.add(new Match("EmpRequestStatus", "<>", EEmpRequest.STATUS_SNDREJ));

                    //filterStatus.add(new Match("EmpRequestStatus", "<>", EEmpRequest.STATUS_SNDAPP));
                    filterStatus.add(new Match("EmpRequestStatus", "<>", EEmpRequest.STATUS_CANCELLED));
                    filterStatus.add(new Match("EmpRequestStatus", "<>", EEmpRequest.STATUS_REJECTED));
                    filterStatus.add(new Match("EmpRequestStatus", "<>", EEmpRequest.STATUS_APPROVED));


                    if (EEmpRequest.db.count(dbConn, filterStatus) > 0)
                    {

                        DateTime oldLeaveAppTimeFrom = previousRequestLeaveApp.RequestLeaveAppDateFrom.Date.Add(new TimeSpan(previousRequestLeaveApp.RequestLeaveAppTimeFrom.Hour, previousRequestLeaveApp.RequestLeaveAppTimeFrom.Minute, previousRequestLeaveApp.RequestLeaveAppTimeFrom.Second));
                        DateTime oldLeaveAppTimeTo = previousRequestLeaveApp.RequestLeaveAppDateTo.Date.Add(new TimeSpan(previousRequestLeaveApp.RequestLeaveAppTimeTo.Hour, previousRequestLeaveApp.RequestLeaveAppTimeTo.Minute, previousRequestLeaveApp.RequestLeaveAppTimeTo.Second));
                        while (oldLeaveAppTimeFrom > oldLeaveAppTimeTo)
                            oldLeaveAppTimeTo = oldLeaveAppTimeTo.AddDays(1);

                        if (!newLeaveAppTimeFrom.Equals(newLeaveAppTimeTo) && !oldLeaveAppTimeFrom.Equals(oldLeaveAppTimeTo))
                            //  Only check if the following case exists
                            // Time A From------- Time B From ---------- Time B To ------------Time A To
                            if (newLeaveAppTimeFrom <= oldLeaveAppTimeFrom && oldLeaveAppTimeTo <= newLeaveAppTimeTo || oldLeaveAppTimeFrom <= newLeaveAppTimeFrom && newLeaveAppTimeTo <= oldLeaveAppTimeTo)
                                OverlapLeaveApplicationList.Add(previousRequestLeaveApp);
                    }
                }
            }
            //  Also Check Leave Application
            ArrayList OverlapOutstandardLeaveApplicationList;
            ELeaveApplication leaveApp = new ELeaveApplication();
            leaveApp.EmpID = EmpID;
            leaveApp.LeaveAppDateFrom = RequestLeaveAppDateFrom;
            leaveApp.LeaveAppDateFromAM = RequestLeaveAppDateFromAM;
            leaveApp.LeaveAppDateTo = RequestLeaveAppDateTo;
            leaveApp.LeaveAppDateToAM = RequestLeaveAppDateToAM;
            leaveApp.LeaveAppDays = RequestLeaveDays;
            leaveApp.LeaveAppTimeFrom = RequestLeaveAppTimeFrom;
            leaveApp.LeaveAppTimeTo = RequestLeaveAppTimeTo;
            leaveApp.LeaveAppUnit = RequestLeaveAppUnit;
            leaveApp.LeaveCodeID = RequestLeaveCodeID;
            if (leaveApp.IsOverlapLeaveApplication(dbConn, out OverlapOutstandardLeaveApplicationList))
            {
                // Start 0000233, Miranda, 2015-07-05
                if (RequestLeaveAppUnit.Equals("D"))
                {
                    OverlapLeaveApplicationList.AddRange(OverlapOutstandardLeaveApplicationList);
                }
                // End 0000233, Miranda, 2015-07-05
            }

            if (OverlapLeaveApplicationList.Count > 0)
                return true;
            else
                return false;
        }

        public static ERequestLeaveApplication CreateDummyRequestLeaveAppliction(ELeaveApplication leaveApp)
        {
            ERequestLeaveApplication requestLeaveApp = new ERequestLeaveApplication();
            requestLeaveApp.EmpID = leaveApp.EmpID;
            requestLeaveApp.RequestLeaveAppDateFrom = leaveApp.LeaveAppDateFrom;
            requestLeaveApp.RequestLeaveAppDateTo = leaveApp.LeaveAppDateTo;
            requestLeaveApp.RequestLeaveAppHasMedicalCertificate = leaveApp.LeaveAppHasMedicalCertificate;
            requestLeaveApp.RequestLeaveDays = leaveApp.LeaveAppDays;
            requestLeaveApp.RequestLeaveAppHours = leaveApp.LeaveAppHours;
            requestLeaveApp.RequestLeaveAppRemark = leaveApp.LeaveAppRemark;
            requestLeaveApp.RequestLeaveAppTimeFrom = leaveApp.LeaveAppTimeFrom;
            requestLeaveApp.RequestLeaveAppTimeTo = leaveApp.LeaveAppTimeTo;
            requestLeaveApp.RequestLeaveAppUnit = leaveApp.LeaveAppUnit;
            requestLeaveApp.RequestLeaveCodeID = leaveApp.LeaveCodeID;
            requestLeaveApp.RequestLeaveAppRemark = leaveApp.LeaveAppRemark;
            return requestLeaveApp;
        }
    }
}
