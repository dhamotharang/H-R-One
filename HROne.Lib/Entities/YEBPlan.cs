using System;
using System.Text;
using HROne.DataAccess;
////using perspectivemind.validation;

namespace HROne.Lib.Entities
{
    [DBClass("YEBPlan")]
    public class EYEBPlan : BaseObject
    {
        public const string PAYMENT_BASE_AVERAGE_BASIC_SALARY = "AVGBASIC";
        public const string PAYMENT_BASE_AVERAGE_BASIC_SALARY2 = "AVGBASIC2";
        public const string PAYMENT_BASE_MONTHLY_AVERAGE_WAGES = Values.PAYMENT_BASE_MONTHLY_AVERAGE_WAGES;
        public const string PAYMENT_BASE_RECURRING_BASIC_SALARY = Values.PAYMENT_BASE_RECURRING_BASIC_SALARY;

        public static DBManager db = new DBManager(typeof(EYEBPlan));
        public static WFValueList VLYEBPlan = new WFDBCodeList(EYEBPlan.db, "YEBPlanID", "YEBPlanCode", "YEBPlanDesc", "YEBPlanCode");
        public static WFValueList VLProrataMethod = new AppUtils.NewWFTextList(new string[] { "D", "M" }, new string[] { "Daily", "Monthly" });
        public static WFValueList VLPaymentBaseMethod = new AppUtils.NewWFTextList(new string[] { PAYMENT_BASE_MONTHLY_AVERAGE_WAGES, PAYMENT_BASE_RECURRING_BASIC_SALARY, PAYMENT_BASE_AVERAGE_BASIC_SALARY, PAYMENT_BASE_AVERAGE_BASIC_SALARY2 }, new string[] { "Monthly Average Wages", "Recurring Basic Salary", "Average Basic Salary", "Average Basic Salary2" });

        protected int m_YEBPlanID;
        [DBField("YEBPlanID", true, true), TextSearch, Export(false)]
        public int YEBPlanID
        {
            get { return m_YEBPlanID; }
            set { m_YEBPlanID = value; modify("YEBPlanID"); }
        }
        protected string m_YEBPlanCode;
        [DBField("YEBPlanCode"), TextSearch, MaxLength(20, 10), Export(false), Required]
        public string YEBPlanCode
        {
            get { return m_YEBPlanCode; }
            set { m_YEBPlanCode = value; modify("YEBPlanCode"); }
        }
        protected string m_YEBPlanDesc;
        [DBField("YEBPlanDesc"), TextSearch, MaxLength(100, 50), Export(false), Required]
        public string YEBPlanDesc
        {
            get { return m_YEBPlanDesc; }
            set { m_YEBPlanDesc = value; modify("YEBPlanDesc"); }
        }

        protected string m_YEBPlanPaymentBaseMethod;
        [DBField("YEBPlanPaymentBaseMethod"), TextSearch, Export(false), Required]
        public string YEBPlanPaymentBaseMethod
        {
            get { return m_YEBPlanPaymentBaseMethod; }
            set { m_YEBPlanPaymentBaseMethod = value; modify("YEBPlanPaymentBaseMethod"); }
        }

        protected int m_YEBPlanRPPaymentCodeID;
        [DBField("YEBPlanRPPaymentCodeID"), TextSearch, Export(false)]
        public int YEBPlanRPPaymentCodeID
        {
            get { return m_YEBPlanRPPaymentCodeID; }
            set { m_YEBPlanRPPaymentCodeID = value; modify("YEBPlanRPPaymentCodeID"); }
        }

        protected string m_YEBPlanProrataMethod;
        [DBField("YEBPlanProrataMethod"), TextSearch, Export(false), Required]
        public string YEBPlanProrataMethod
        {
            get { return m_YEBPlanProrataMethod; }
            set { m_YEBPlanProrataMethod = value; modify("YEBPlanProrataMethod"); }
        }

        protected double m_YEBPlanMultiplier;
        [DBField("YEBPlanMultiplier", "0.###"), TextSearch, MaxLength(5, 5), Export(false), Required]
        public double YEBPlanMultiplier
        {
            get { return m_YEBPlanMultiplier; }
            set { m_YEBPlanMultiplier = value; modify("YEBPlanMultiplier"); }
        }

        protected int m_YEBPlanPaymentCodeID;
        [DBField("YEBPlanPaymentCodeID"), TextSearch, Export(false), Required]
        public int YEBPlanPaymentCodeID
        {
            get { return m_YEBPlanPaymentCodeID; }
            set { m_YEBPlanPaymentCodeID = value; modify("YEBPlanPaymentCodeID"); }
        }

        protected bool m_YEBPlanIsEligibleAfterProbation;
        [DBField("YEBPlanIsEligibleAfterProbation"), TextSearch, Export(false)]
        public bool YEBPlanIsEligibleAfterProbation
        {
            get { return m_YEBPlanIsEligibleAfterProbation; }
            set { m_YEBPlanIsEligibleAfterProbation = value; modify("YEBPlanIsEligibleAfterProbation"); }
        }

        protected int m_YEBPlanEligiblePeriod;
        [DBField("YEBPlanEligiblePeriod"), MaxLength(3), TextSearch, Export(false)]
        public int YEBPlanEligiblePeriod
        {
            get { return m_YEBPlanEligiblePeriod; }
            set { m_YEBPlanEligiblePeriod = value; modify("YEBPlanEligiblePeriod"); }
        }

        protected string m_YEBPlanEligibleUnit;
        [DBField("YEBPlanEligibleUnit"), TextSearch, Export(false)]
        public string YEBPlanEligibleUnit
        {
            get { return m_YEBPlanEligibleUnit; }
            set { m_YEBPlanEligibleUnit = value; modify("YEBPlanEligibleUnit"); }
        }

        protected bool m_YEBPlanEligiblePeriodIsCheckEveryYEBYear;
        [DBField("YEBPlanEligiblePeriodIsCheckEveryYEBYear"), TextSearch, Export(false)]
        public bool YEBPlanEligiblePeriodIsCheckEveryYEBYear
        {
            get { return m_YEBPlanEligiblePeriodIsCheckEveryYEBYear; }
            set { m_YEBPlanEligiblePeriodIsCheckEveryYEBYear = value; modify("YEBPlanEligiblePeriodIsCheckEveryYEBYear"); }
        }

        protected bool m_YEBPlanEligiblePeriodIsExcludeMax3MonthsProbation;
        [DBField("YEBPlanEligiblePeriodIsExcludeMax3MonthsProbation"), TextSearch, Export(false)]
        public bool YEBPlanEligiblePeriodIsExcludeMax3MonthsProbation
        {
            get { return m_YEBPlanEligiblePeriodIsExcludeMax3MonthsProbation; }
            set { m_YEBPlanEligiblePeriodIsExcludeMax3MonthsProbation = value; modify("YEBPlanEligiblePeriodIsExcludeMax3MonthsProbation"); }
        }

        protected bool m_YEBPlanIsGlobal;
        [DBField("YEBPlanIsGlobal"), TextSearch, Export(false)]
        public bool YEBPlanIsGlobal
        {
            get { return m_YEBPlanIsGlobal; }
            set { m_YEBPlanIsGlobal = value; modify("YEBPlanIsGlobal"); }
        }
    }
}
