using System;
using System.Collections;
using System.Text;
using HROne.DataAccess;
////using perspectivemind.validation;

namespace HROne.Lib.Entities
{
    [DBClass("LeaveApplication")]
    public class ELeaveApplication : BaseObjectWithRecordInfo
    {
        public static DBManager db = new DBManagerWithRecordInfo(typeof(ELeaveApplication));
        public static WFValueList VLLeaveAppYear = new AppUtils.WFDBDistinctList(db, "Year(LeaveAppDateFrom)", " Year(LeaveAppDateFrom)", "Year(LeaveAppDateFrom) desc");

        public const string LEAVEUNIT_DAYS = "D";
        public const string LEAVEUNIT_HOUR = "H";
        public const string LEAVEUNIT_AM = "A";
        public const string LEAVEUNIT_PM = "P";

        protected int m_LeaveAppID;
        [DBField("LeaveAppID", true, true), TextSearch, Export(false)]
        public int LeaveAppID
        {
            get { return m_LeaveAppID; }
            set { m_LeaveAppID = value; modify("LeaveAppID"); }
        }
        protected int m_EmpID;
        [DBField("EmpID"), TextSearch, Export(false)]
        public int EmpID
        {
            get { return m_EmpID; }
            set { m_EmpID = value; modify("EmpID"); }
        }
        protected int m_LeaveCodeID;
        [DBField("LeaveCodeID"), TextSearch, Export(false), Required]
        public int LeaveCodeID
        {
            get { return m_LeaveCodeID; }
            set { m_LeaveCodeID = value; modify("LeaveCodeID"); }
        }
        protected string m_LeaveAppUnit;
        [DBField("LeaveAppUnit"), TextSearch, Export(false), Required]
        public string LeaveAppUnit
        {
            get { return m_LeaveAppUnit; }
            set { m_LeaveAppUnit = value; modify("LeaveAppUnit"); }
        }
        protected DateTime m_LeaveAppDateFrom;
        [DBField("LeaveAppDateFrom"), TextSearch, MaxLength(10), Export(false), Required]
        public DateTime LeaveAppDateFrom
        {
            get { return m_LeaveAppDateFrom; }
            set { m_LeaveAppDateFrom = value; modify("LeaveAppDateFrom"); }
        }
        protected string m_LeaveAppDateFromAM;
        [DBField("LeaveAppDateFromAM"), TextSearch, MaxLength(2), Export(false), Required]
        public string LeaveAppDateFromAM
        {
            get { return m_LeaveAppDateFromAM; }
            set { m_LeaveAppDateFromAM = value; modify("LeaveAppDateFromAM"); }
        }
        protected DateTime m_LeaveAppDateTo;
        [DBField("LeaveAppDateTo"), TextSearch, MaxLength(10), Export(false), Required]
        public DateTime LeaveAppDateTo
        {
            get { return m_LeaveAppDateTo; }
            set { m_LeaveAppDateTo = value; modify("LeaveAppDateTo"); }
        }
        protected string m_LeaveAppDateToAM;
        [DBField("LeaveAppDateToAM"), TextSearch, MaxLength(10), Export(false), Required]
        public string LeaveAppDateToAM
        {
            get { return m_LeaveAppDateToAM; }
            set { m_LeaveAppDateToAM = value; modify("LeaveAppDateToAM"); }
        }
        protected DateTime m_LeaveAppTimeFrom;
        [DBField("LeaveAppTimeFrom", "HH:mm"), TextSearch, MaxLength(5), Export(false)]
        public DateTime LeaveAppTimeFrom
        {
            get { return m_LeaveAppTimeFrom; }
            set { m_LeaveAppTimeFrom = value; modify("LeaveAppTimeFrom"); }
        }
        protected DateTime m_LeaveAppTimeTo;
        [DBField("LeaveAppTimeTo", "HH:mm"), TextSearch, MaxLength(5), Export(false)]
        public DateTime LeaveAppTimeTo
        {
            get { return m_LeaveAppTimeTo; }
            set { m_LeaveAppTimeTo = value; modify("LeaveAppTimeTo"); }
        }
        protected double m_LeaveAppDays;
        [DBField("LeaveAppDays", "0.####"), TextSearch, MaxLength(6), Export(false), Required]
        public double LeaveAppDays
        {
            get { return m_LeaveAppDays; }
            set { m_LeaveAppDays = value; modify("LeaveAppDays"); }
        }
        protected double m_LeaveAppHours;
        [DBField("LeaveAppHours", "0.####"), TextSearch, MaxLength(6), Export(false)]
        public double LeaveAppHours
        {
            get { return m_LeaveAppHours; }
            set { m_LeaveAppHours = value; modify("LeaveAppHours"); }
        }
        protected string m_LeaveAppRemark;
        [DBField("LeaveAppRemark"), TextSearch, Export(false)]
        public string LeaveAppRemark
        {
            get { return m_LeaveAppRemark; }
            set { m_LeaveAppRemark = value; modify("LeaveAppRemark"); }
        }

        protected bool m_LeaveAppNoPayProcess;
        [DBField("LeaveAppNoPayProcess"), TextSearch, Export(false)]
        public bool LeaveAppNoPayProcess
        {
            get { return m_LeaveAppNoPayProcess; }
            set { m_LeaveAppNoPayProcess = value; modify("LeaveAppNoPayProcess"); }
        }

        protected bool m_LeaveAppHasMedicalCertificate;
        [DBField("LeaveAppHasMedicalCertificate"), TextSearch, Export(false)]
        public bool LeaveAppHasMedicalCertificate
        {
            get { return m_LeaveAppHasMedicalCertificate; }
            set { m_LeaveAppHasMedicalCertificate = value; modify("LeaveAppHasMedicalCertificate"); }
        }

        protected int m_EmpPaymentID;
        [DBField("EmpPaymentID"), TextSearch, Export(false)]
        public int EmpPaymentID
        {
            get { return m_EmpPaymentID; }
            set { m_EmpPaymentID = value; modify("EmpPaymentID"); }
        }

        protected int m_EmpPayrollID;
        [DBField("EmpPayrollID"), TextSearch, Export(false)]
        public int EmpPayrollID
        {
            get { return m_EmpPayrollID; }
            set { m_EmpPayrollID = value; modify("EmpPayrollID"); }
        }

        protected int m_LeaveAppCancelID;
        [DBField("LeaveAppCancelID"), TextSearch, Export(false)]
        public int LeaveAppCancelID
        {
            get { return m_LeaveAppCancelID; }
            set { m_LeaveAppCancelID = value; modify("LeaveAppCancelID"); }
        }

        protected int m_EmpRequestID;
        [DBField("EmpRequestID"), TextSearch, Export(false)]
        public int EmpRequestID
        {
            get { return m_EmpRequestID; }
            set { m_EmpRequestID = value; modify("EmpRequestID"); }
        }

        protected int m_RequestLeaveAppID;
        [DBField("RequestLeaveAppID"), TextSearch, Export(false)]
        public int RequestLeaveAppID
        {
            get { return m_RequestLeaveAppID; }
            set { m_RequestLeaveAppID = value; modify("RequestLeaveAppID"); }
        }

        //  For Synchronize Use only
        protected string m_SynID;
        [DBField("SynID"), TextSearch, Export(false)]
        public string SynID
        {
            get { return m_SynID; }
            set { m_SynID = value; modify("SynID"); }
        }

        public bool IsOverlapLeaveApplication(DatabaseConnection dbConn, out ArrayList OverlapLeaveApplicationList)
        {
            OverlapLeaveApplicationList = new ArrayList();

            DBFilter overlapCheckingFilter = new DBFilter();
            overlapCheckingFilter.add(new Match("EmpID", EmpID));
            overlapCheckingFilter.add(new Match("LeaveAppID", "<>", LeaveAppID));
            overlapCheckingFilter.add(new Match("LeaveAppDateFrom", "<=", LeaveAppDateTo));
            overlapCheckingFilter.add(new Match("LeaveAppDateTo", ">=", LeaveAppDateFrom));

            OR leaveCancelIDOrTerm = new OR();
            leaveCancelIDOrTerm.add(new NullTerm("LeaveAppCancelID"));
            leaveCancelIDOrTerm.add(new Match("LeaveAppCancelID", "<=", 0));
            overlapCheckingFilter.add(leaveCancelIDOrTerm);

            if (LeaveAppUnit.Equals("H"))
                overlapCheckingFilter.add(new Match("LeaveAppUnit", "D"));
            else if (LeaveAppUnit.Equals("A"))
            {
                //  AM Leave Application
                OR orLeaveAppUnit = new OR();
                orLeaveAppUnit.add(new Match("LeaveAppUnit", "D"));
                orLeaveAppUnit.add(new Match("LeaveAppUnit", "A"));
                overlapCheckingFilter.add(orLeaveAppUnit);
            }
            else if (LeaveAppUnit.Equals("P"))
            {
                //  PM Leave Application
                OR orLeaveAppUnit = new OR();
                orLeaveAppUnit.add(new Match("LeaveAppUnit", "D"));
                orLeaveAppUnit.add(new Match("LeaveAppUnit", "P"));
                overlapCheckingFilter.add(orLeaveAppUnit);
            }

            // Start 0000201, Ricky So, 2015-06-01
            // remove those are not classified as overlap
            //ArrayList overlapDailyLeaveAppList = db.select(dbConn, overlapCheckingFilter);
            ArrayList overlapDailyLeaveAppList = new ArrayList();
            ArrayList potentialOverlapLeaveAppList = db.select(dbConn, overlapCheckingFilter);
            foreach (ELeaveApplication potentialOverlapLeaveApp in potentialOverlapLeaveAppList)
            {

                if (this.LeaveAppDateFrom == potentialOverlapLeaveApp.LeaveAppDateTo)
                {
                    if (this.LeaveAppDateFromAM == "PM" || this.LeaveAppUnit == "P")
                    {
                        if (potentialOverlapLeaveApp.LeaveAppDateToAM == "AM")
                            continue;
                        else if (potentialOverlapLeaveApp.LeaveAppUnit == "A")
                            continue;
                    }
                    else if (this.LeaveAppUnit == "A")
                    {
                        //if (potentialOverlapLeaveApp.LeaveAppDateToAM == "PM")
                        //    continue;
                        //else if (potentialOverlapLeaveApp.LeaveAppUnit == "P")
                        //    continue;
                    }
                    //else if (this.LeaveAppUnit == "")
                    //{

                    //}
                }

                if (this.LeaveAppDateTo == potentialOverlapLeaveApp.LeaveAppDateFrom)
                {
                    if (this.LeaveAppDateToAM == "AM") 
                    {
                        if (potentialOverlapLeaveApp.LeaveAppDateFromAM == "PM")
                            continue;
                        else if (potentialOverlapLeaveApp.LeaveAppUnit == "P")
                            continue;
                    }
                    else if (this.LeaveAppUnit == "P")
                    {

                    }
                    //else if (this.LeaveAppUnit == "")
                    //{

                    //}
                }

                if (this.LeaveAppDateFrom == potentialOverlapLeaveApp.LeaveAppDateFrom)
                {
                    if (this.LeaveAppUnit == "A")
                    {
                        if (potentialOverlapLeaveApp.LeaveAppUnit == "P" || potentialOverlapLeaveApp.LeaveAppDateFromAM == "PM")
                            continue;
                    }
                    else if (this.LeaveAppUnit == "P")
                    {
                        if (potentialOverlapLeaveApp.LeaveAppUnit == "A")
                            continue;
                    }
                    else if (this.LeaveAppUnit == "D")
                    {
                        if (this.LeaveAppDateFromAM == "PM" && (potentialOverlapLeaveApp.LeaveAppDateFromAM == "AM" || potentialOverlapLeaveApp.LeaveAppUnit == "A"))
                            continue;
                    }
                }

                overlapDailyLeaveAppList.Add(potentialOverlapLeaveApp);
            }
            // End 0000201, Ricky So, 2015-06-01



            if (overlapDailyLeaveAppList.Count > 0)
            {
                OverlapLeaveApplicationList.AddRange(overlapDailyLeaveAppList);
            }
            if (!LeaveAppUnit.Equals("D"))
            {
                DateTime newLeaveAppTimeFrom = LeaveAppDateFrom.Date.Add(new TimeSpan(LeaveAppTimeFrom.Hour, LeaveAppTimeFrom.Minute, LeaveAppTimeFrom.Second));
                DateTime newLeaveAppTimeTo = LeaveAppDateTo.Date.Add(new TimeSpan(LeaveAppTimeTo.Hour, LeaveAppTimeTo.Minute, LeaveAppTimeTo.Second));
                while (newLeaveAppTimeFrom > newLeaveAppTimeTo)
                    newLeaveAppTimeTo = newLeaveAppTimeTo.AddDays(1);

                overlapCheckingFilter = new DBFilter();
                overlapCheckingFilter.add(new Match("EmpID", EmpID));
                overlapCheckingFilter.add(new Match("LeaveAppID", "<>", LeaveAppID));
                overlapCheckingFilter.add(new Match("LeaveAppDateFrom", "<=", LeaveAppDateTo));
                overlapCheckingFilter.add(new Match("LeaveAppDateTo", ">=", LeaveAppDateFrom));
                overlapCheckingFilter.add(new Match("LeaveAppUnit", "<>", "D"));
                ArrayList leaveAppList = db.select(dbConn, overlapCheckingFilter);
                foreach (ELeaveApplication oldLeaveApp in leaveAppList)
                {
                    DateTime oldLeaveAppTimeFrom = oldLeaveApp.LeaveAppDateFrom.Date.Add(new TimeSpan(oldLeaveApp.LeaveAppTimeFrom.Hour, oldLeaveApp.LeaveAppTimeFrom.Minute, oldLeaveApp.LeaveAppTimeFrom.Second));
                    DateTime oldLeaveAppTimeTo = oldLeaveApp.LeaveAppDateTo.Date.Add(new TimeSpan(oldLeaveApp.LeaveAppTimeTo.Hour, oldLeaveApp.LeaveAppTimeTo.Minute, oldLeaveApp.LeaveAppTimeTo.Second));
                    while (oldLeaveAppTimeFrom > oldLeaveAppTimeTo)
                        oldLeaveAppTimeTo = oldLeaveAppTimeTo.AddDays(1);

                    if (!newLeaveAppTimeFrom.Equals(newLeaveAppTimeTo) && !oldLeaveAppTimeFrom.Equals(oldLeaveAppTimeTo))
                        //  Only check if the following case exists
                        // Time A From------- Time B From ---------- Time B To ------------Time A To
                        if (newLeaveAppTimeFrom <= oldLeaveAppTimeFrom && oldLeaveAppTimeTo <= newLeaveAppTimeTo || oldLeaveAppTimeFrom <= newLeaveAppTimeFrom && newLeaveAppTimeTo <= oldLeaveAppTimeTo)
                            OverlapLeaveApplicationList.Add(oldLeaveApp);
                }
            }
            if (OverlapLeaveApplicationList.Count > 0)
                return true;
            else
                return false;
        }

        public static double GetEstimatedNumOfLeaveDays(DatabaseConnection dbConn, int EmpID, DateTime DateFrom, DateTime DateTo, int LeaveCodeID, out DateTime[] DateSkipArray)
        {
            System.Collections.ArrayList dateSkipArrayList = new System.Collections.ArrayList();

            EEmpPositionInfo empPos = null;
            double totalDays = 0;

            ELeaveType leaveType = new ELeaveType();
            leaveType.LeaveTypeIsUseWorkHourPattern = true;

            ELeaveCode leaveCode = new ELeaveCode();
            leaveCode.LeaveCodeID = LeaveCodeID;
            if (ELeaveCode.db.select(dbConn, leaveCode))
            {
                leaveType.LeaveTypeID = leaveCode.LeaveTypeID;
                ELeaveType.db.select(dbConn, leaveType);
            }
            for (DateTime currentDate = DateFrom; currentDate <= DateTo; currentDate = currentDate.AddDays(1))
            {
                //  default every date count = 1
                double dateCount = 1;
                if (EStatutoryHoliday.IsHoliday(dbConn, currentDate))
                {
                    if (!leaveType.LeaveTypeIsSkipStatutoryHolidayChecking)
                        dateCount = 0;
                }
                else if (EPublicHoliday.IsHoliday(dbConn, currentDate))
                {
                    if (!leaveType.LeaveTypeIsSkipPublicHolidayChecking)
                        dateCount = 0;
                }

                if (empPos == null)
                    empPos = AppUtils.GetLastPositionInfo(dbConn, currentDate, EmpID);
                else
                    if (!empPos.EmpPosEffTo.Ticks.Equals(0) && empPos.EmpPosEffTo <= currentDate)
                        empPos = AppUtils.GetLastPositionInfo(dbConn, currentDate, EmpID);

                if (empPos != null)
                {
                    EWorkHourPattern workPattern = new EWorkHourPattern();
                    workPattern.WorkHourPatternID = empPos.WorkHourPatternID;
                    if (EWorkHourPattern.db.select(dbConn, workPattern))
                        if (leaveType.LeaveTypeIsUseWorkHourPattern)
                            dateCount = workPattern.GetDefaultDayUnit(dbConn, currentDate, leaveType.LeaveTypeIsSkipStatutoryHolidayChecking, leaveType.LeaveTypeIsSkipPublicHolidayChecking);
                }


                totalDays += dateCount;

                if (dateCount <= 0)
                    dateSkipArrayList.Add(currentDate);
            }

            DateSkipArray = (DateTime[])dateSkipArrayList.ToArray(typeof(DateTime));
            return totalDays;
        }

        public static ELeaveApplication GetOrCreateSingleDayLeaveApplicationObject(DatabaseConnection dbConn, int EmpID, int LeaveCodeID, DateTime LeaveDate)
        {
            DBFilter leaveFilter = new DBFilter();
            leaveFilter.add(new Match("EmpID", EmpID));
            leaveFilter.add(new Match("LeaveAppDateFrom", "<=", LeaveDate));
            leaveFilter.add(new Match("LeaveAppDateTo", ">=", LeaveDate));
            leaveFilter.add(new Match("LeaveAppDays", ">=", 1));
            leaveFilter.add(new Match("LeaveCodeID", LeaveCodeID));
            ArrayList oldLeaveAppList = ELeaveApplication.db.select(dbConn, leaveFilter);
            if (oldLeaveAppList.Count > 0)
                return ((ELeaveApplication)oldLeaveAppList[0]);
            else
            {
                ELeaveApplication leaveApp = new ELeaveApplication();
                leaveApp.LeaveAppDateFrom = LeaveDate;
                leaveApp.LeaveAppDateTo = LeaveDate;
                leaveApp.LeaveAppDays = 1;
                leaveApp.LeaveAppUnit = "D";
                leaveApp.LeaveCodeID = LeaveCodeID;
                leaveApp.EmpID = EmpID;

                return leaveApp;
            }
        }

        protected override void afterInsert(DatabaseConnection dbConn, DBManager db)
        {
            base.afterInsert(dbConn, db);

            ELeaveCode leaveCode = new ELeaveCode();
            leaveCode.LeaveCodeID = this.LeaveCodeID;
            if (ELeaveCode.db.select(dbConn, leaveCode))
                ELeaveBalance.DeleteLeaveBalanceAfter(dbConn, this.EmpID, leaveCode.LeaveTypeID, this.LeaveAppDateFrom);

        }

        protected override void afterUpdate(DatabaseConnection dbConn, DBManager db)
        {
            int oldLeaveCodeID = 0;
            int newLeaveCodeID = m_LeaveCodeID;
            DateTime oldLeaveDateFrom = new DateTime();
            DateTime newLeaveDateFrom = m_LeaveAppDateFrom;
            if (oldValueObject != null)
            {
                ELeaveApplication oldLeaveApp = (ELeaveApplication)oldValueObject;
                oldLeaveCodeID = oldLeaveApp.LeaveCodeID;
                oldLeaveDateFrom = oldLeaveApp.LeaveAppDateFrom;
            }
            {
                ELeaveCode leaveCode = new ELeaveCode();
                leaveCode.LeaveCodeID = oldLeaveCodeID;
                if (ELeaveCode.db.select(dbConn, leaveCode))
                    ELeaveBalance.DeleteLeaveBalanceAfter(dbConn, this.EmpID, leaveCode.LeaveTypeID, oldLeaveDateFrom);
                leaveCode.LeaveCodeID = newLeaveCodeID;
                if (ELeaveCode.db.select(dbConn, leaveCode))
                    ELeaveBalance.DeleteLeaveBalanceAfter(dbConn, this.EmpID, leaveCode.LeaveTypeID, newLeaveDateFrom);
            }

            base.afterUpdate(dbConn, db);
        }

        protected override void afterDelete(DatabaseConnection dbConn, DBManager db)
        {
            base.afterDelete(dbConn, db);

            ELeaveCode leaveCode = new ELeaveCode();
            leaveCode.LeaveCodeID = this.LeaveCodeID;
            if (ELeaveCode.db.select(dbConn, leaveCode))
                ELeaveBalance.DeleteLeaveBalanceAfter(dbConn, this.EmpID, leaveCode.LeaveTypeID, this.LeaveAppDateFrom);
        }
    }
}
