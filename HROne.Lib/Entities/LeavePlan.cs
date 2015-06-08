using System;
using System.Collections.Generic;
using System.Text;
using HROne.DataAccess;
////using perspectivemind.validation;
using HROne.Common;

namespace HROne.Lib.Entities
{
    [DBClass("LeavePlan")]
    public class ELeavePlan : BaseObject
    {
        public static DBManager db = new DBManager(typeof(ELeavePlan));
        public static WFValueList VLLeavePlan = new AppUtils.EncryptedDBCodeList(ELeavePlan.db, "LeavePlanID", new string[] { "LeavePlanCode", "LeavePlanDesc" }, " - ", "LeavePlanCode");
        public static WFValueList VLRestDayEntitlementPeriod = new AppUtils.NewWFTextList(new string[] { "M", "W" }, new string[] { "per month", "per week" });
        public static WFValueList VLRestDayGainWeekDay = new AppUtils.NewWFTextList(new string[] { "SUN", "MON", "TUE", "WED", "THU", "FRI", "SAT", "JOIN" }, new string[] { "Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday", "Determined by Date of Join" });
        public static WFValueList VLYearOfServiceReferenceMethod = new AppUtils.NewWFTextList(new string[] { "True", "False" }, new string[] { "Common Leave Year Start Date", "Service Start Date" });

        public static ELeavePlan GetObject(DatabaseConnection dbConn, object ID)
        {
            if (ID is int)
            {
                ELeavePlan obj = new ELeavePlan();
                obj.LeavePlanID = (int)ID;
                if (db.select(dbConn, obj))
                    return obj;
            }
            return null;
        }

        protected int m_LeavePlanID;
        [DBField("LeavePlanID", true, true), TextSearch, Export(false)]
        public int LeavePlanID
        {
            get { return m_LeavePlanID; }
            set { m_LeavePlanID = value; modify("LeavePlanID"); }
        }
        protected string m_LeavePlanCode;
        [DBField("LeavePlanCode"), TextSearch, DBAESEncryptStringField, MaxLength(20, 20), Export(false), Required]
        public string LeavePlanCode
        {
            get { return m_LeavePlanCode; }
            set { m_LeavePlanCode = value; modify("LeavePlanCode"); }
        }
        protected string m_LeavePlanDesc;
        [DBField("LeavePlanDesc"), TextSearch, DBAESEncryptStringField, MaxLength(100, 50), Export(false), Required]
        public string LeavePlanDesc
        {
            get { return m_LeavePlanDesc; }
            set { m_LeavePlanDesc = value; modify("LeavePlanDesc"); }
        }

        protected int m_ALProrataRoundingRuleID;
        [DBField("ALProrataRoundingRuleID"), TextSearch, Export(false)]
        public int ALProrataRoundingRuleID
        {
            get { return m_ALProrataRoundingRuleID; }
            set { m_ALProrataRoundingRuleID = value; modify("ALProrataRoundingRuleID"); }
        }

        protected bool m_LeavePlanALRoundingRuleIsApplyFirstYearBroughtForwardOnly;
        [DBField("LeavePlanALRoundingRuleIsApplyFirstYearBroughtForwardOnly"), TextSearch, Export(false)]
        public bool LeavePlanALRoundingRuleIsApplyFirstYearBroughtForwardOnly
        {
            get { return m_LeavePlanALRoundingRuleIsApplyFirstYearBroughtForwardOnly; }
            set { m_LeavePlanALRoundingRuleIsApplyFirstYearBroughtForwardOnly = value; modify("LeavePlanALRoundingRuleIsApplyFirstYearBroughtForwardOnly"); }
        }

        protected bool m_LeavePlanUseCommonLeaveYear;
        [DBField("LeavePlanUseCommonLeaveYear"), TextSearch, Export(false)]
        public bool LeavePlanUseCommonLeaveYear
        {
            get { return m_LeavePlanUseCommonLeaveYear; }
            set { m_LeavePlanUseCommonLeaveYear = value; modify("LeavePlanUseCommonLeaveYear"); }
        }

        protected int m_LeavePlanCommonLeaveYearStartDay;
        [DBField("LeavePlanCommonLeaveYearStartDay"), TextSearch, MaxLength(2), Export(false)]
        public int LeavePlanCommonLeaveYearStartDay
        {
            get { return m_LeavePlanCommonLeaveYearStartDay; }
            set { m_LeavePlanCommonLeaveYearStartDay = value; modify("LeavePlanCommonLeaveYearStartDay"); }
        }

        protected int m_LeavePlanCommonLeaveYearStartMonth;
        [DBField("LeavePlanCommonLeaveYearStartMonth"), TextSearch, Export(false)]
        public int LeavePlanCommonLeaveYearStartMonth
        {
            get { return m_LeavePlanCommonLeaveYearStartMonth; }
            set { m_LeavePlanCommonLeaveYearStartMonth = value; modify("LeavePlanCommonLeaveYearStartMonth"); }
        }

        protected bool m_LeavePlanNoCountFirstIncompleteYearOfService;
        [DBField("LeavePlanNoCountFirstIncompleteYearOfService"), TextSearch, Export(false)]
        public bool LeavePlanNoCountFirstIncompleteYearOfService
        {
            get { return m_LeavePlanNoCountFirstIncompleteYearOfService; }
            set { m_LeavePlanNoCountFirstIncompleteYearOfService = value; modify("LeavePlanNoCountFirstIncompleteYearOfService"); }
        }

        protected bool m_LeavePlanProrataSkipFeb29;
        [DBField("LeavePlanProrataSkipFeb29"), TextSearch, Export(false)]
        public bool LeavePlanProrataSkipFeb29
        {
            get { return m_LeavePlanProrataSkipFeb29; }
            set { m_LeavePlanProrataSkipFeb29 = value; modify("LeavePlanProrataSkipFeb29"); }
        }

        protected bool m_LeavePlanResetYearOfService;
        [DBField("LeavePlanResetYearOfService"), TextSearch, Export(false)]
        public bool LeavePlanResetYearOfService
        {
            get { return m_LeavePlanResetYearOfService; }
            set { m_LeavePlanResetYearOfService = value; modify("LeavePlanResetYearOfService"); }
        }
        
        protected bool m_LeavePlanComparePreviousLeavePlan;
        [DBField("LeavePlanComparePreviousLeavePlan"), TextSearch, Export(false)]
        public bool LeavePlanComparePreviousLeavePlan
        {
            get { return m_LeavePlanComparePreviousLeavePlan; }
            set { m_LeavePlanComparePreviousLeavePlan = value; modify("LeavePlanComparePreviousLeavePlan"); }
        }

        protected int m_LeavePlanLeavePlanCompareRank;
        [DBField("LeavePlanLeavePlanCompareRank"), TextSearch, Export(false)]
        public int LeavePlanLeavePlanCompareRank
        {
            get { return m_LeavePlanLeavePlanCompareRank; }
            set { m_LeavePlanLeavePlanCompareRank = value; modify("LeavePlanLeavePlanCompareRank"); }
        }

        protected bool m_LeavePlanUseStatutoryHolidayEntitle;
        [DBField("LeavePlanUseStatutoryHolidayEntitle"), TextSearch, Export(false)]
        public bool LeavePlanUseStatutoryHolidayEntitle
        {
            get { return m_LeavePlanUseStatutoryHolidayEntitle; }
            set { m_LeavePlanUseStatutoryHolidayEntitle = value; modify("LeavePlanUseStatutoryHolidayEntitle"); }
        }

        protected bool m_LeavePlanUsePublicHolidayEntitle;
        [DBField("LeavePlanUsePublicHolidayEntitle"), TextSearch, Export(false)]
        public bool LeavePlanUsePublicHolidayEntitle
        {
            get { return m_LeavePlanUsePublicHolidayEntitle; }
            set { m_LeavePlanUsePublicHolidayEntitle = value; modify("LeavePlanUsePublicHolidayEntitle"); }
        }
        
        protected bool m_LeavePlanUseRestDayEntitle;
        [DBField("LeavePlanUseRestDayEntitle"), TextSearch, Export(false)]
        public bool LeavePlanUseRestDayEntitle
        {
            get { return m_LeavePlanUseRestDayEntitle; }
            set { m_LeavePlanUseRestDayEntitle = value; modify("LeavePlanUseRestDayEntitle"); }
        }
		 
        protected string m_LeavePlanRestDayEntitlePeriod;
        [DBField("LeavePlanRestDayEntitlePeriod"), TextSearch, Export(false)]
        public string LeavePlanRestDayEntitlePeriod
        {
            get { return string.IsNullOrEmpty(m_LeavePlanRestDayEntitlePeriod) ? string.Empty : m_LeavePlanRestDayEntitlePeriod; }
            set { m_LeavePlanRestDayEntitlePeriod = value; modify("LeavePlanRestDayEntitlePeriod"); }
        }

        protected double m_LeavePlanRestDayEntitleDays;
        [DBField("LeavePlanRestDayEntitleDays", "0.##"), TextSearch, MaxLength(5), Export(false)]
        public double LeavePlanRestDayEntitleDays
        {
            get { return m_LeavePlanRestDayEntitleDays; }
            set { m_LeavePlanRestDayEntitleDays = value; modify("LeavePlanRestDayEntitleDays"); }
        }

        //  MON-Monday, TUE-Tuesday, WED-Wednesday, THU-Thurday, FRI-Friday, SAT-SATURDAY, SUN-Sunday
        //  JOIN-follow Date of Join
        protected string m_LeavePlanRestDayWeeklyEntitleStartDay;
        [DBField("LeavePlanRestDayWeeklyEntitleStartDay"), TextSearch, Export(false)]
        public string LeavePlanRestDayWeeklyEntitleStartDay
        {
            get { return m_LeavePlanRestDayWeeklyEntitleStartDay; }
            set { m_LeavePlanRestDayWeeklyEntitleStartDay = value; modify("LeavePlanRestDayWeeklyEntitleStartDay"); }
        }

        protected double m_LeavePlanRestDayMonthlyEntitleProrataBase;
        [DBField("LeavePlanRestDayMonthlyEntitleProrataBase", "0.##"), TextSearch, MaxLength(5), Export(false)]
        public double LeavePlanRestDayMonthlyEntitleProrataBase
        {
            get { return m_LeavePlanRestDayMonthlyEntitleProrataBase; }
            set { m_LeavePlanRestDayMonthlyEntitleProrataBase = value; modify("LeavePlanRestDayMonthlyEntitleProrataBase"); }
        }

        protected int m_LeavePlanRestDayMonthlyEntitleProrataRoundingRuleID;
        [DBField("LeavePlanRestDayMonthlyEntitleProrataRoundingRuleID"), TextSearch, Export(false)]
        public int LeavePlanRestDayMonthlyEntitleProrataRoundingRuleID
        {
            get { return m_LeavePlanRestDayMonthlyEntitleProrataRoundingRuleID; }
            set { m_LeavePlanRestDayMonthlyEntitleProrataRoundingRuleID = value; modify("LeavePlanRestDayMonthlyEntitleProrataRoundingRuleID"); }
        }

        // Start 0000006, Miranda, 2014-06-16 
        public ELeavePlan Copy(DatabaseConnection dbConn)
        {
            ELeavePlan obj = new ELeavePlan();
            obj.LeavePlanID = LeavePlanID;
            if (ELeavePlan.db.select(dbConn, obj))
            {
                obj.LeavePlanID = 0;
                obj.LeavePlanCode = obj.LeavePlanCode + "_COPY";
                obj.LeavePlanDesc = "Copy of " + obj.LeavePlanDesc;

                ELeavePlan.db.insert(dbConn, obj);
                return obj;
            }
            return null;
        }
        // End 0000006, Miranda, 2014-06-16

    }
}
