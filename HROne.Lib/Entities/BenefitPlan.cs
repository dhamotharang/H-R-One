using System;
using System.Text;
using HROne.DataAccess;

namespace HROne.Lib.Entities
{
    [DBClass("BenefitPlan")]
    public class EBenefitPlan : BaseObject
    {
        public const string PAYMENT_BASE_FIXED_AMOUNT = Values.PAYMENT_BASE_FIXED_AMOUNT;
        public const string PAYMENT_BASE_RECURRING_BASIC_SALARY = Values.PAYMENT_BASE_RECURRING_BASIC_SALARY;

        public static DBManager db = new DBManager(typeof(EBenefitPlan));
        public static WFValueList VLBenefitPlan = new WFDBCodeList(EBenefitPlan.db, "BenefitPlanID", "BenefitPlanCode", "BenefitPlanDesc", "BenefitPlanCode");
        public static WFValueList VLPaymentBaseMethod = new AppUtils.NewWFTextList(new string[] { PAYMENT_BASE_RECURRING_BASIC_SALARY, PAYMENT_BASE_FIXED_AMOUNT }, new string[] { "Recurring Basic Salary", "Fixed Amount" });

        protected int m_BenefitPlanID;
        [DBField("BenefitPlanID", true, true), TextSearch, Export(false)]
        public int BenefitPlanID
        {
            get { return m_BenefitPlanID; }
            set { m_BenefitPlanID = value; modify("BenefitPlanID"); }
        }

        protected string m_BenefitPlanCode;
        [DBField("BenefitPlanCode"), TextSearch, MaxLength(20, 10), Export(false), Required]
        public string BenefitPlanCode
        {
            get { return m_BenefitPlanCode; }
            set { m_BenefitPlanCode = value; modify("BenefitPlanCode"); }
        }

        protected string m_BenefitPlanDesc;
        [DBField("BenefitPlanDesc"), TextSearch, MaxLength(100, 50), Export(false), Required]
        public string BenefitPlanDesc
        {
            get { return m_BenefitPlanDesc; }
            set { m_BenefitPlanDesc = value; modify("BenefitPlanDesc"); }
        }

        protected string m_BenefitPlanERPaymentBaseMethod;
        [DBField("BenefitPlanERPaymentBaseMethod"), TextSearch, Export(false)]
        public string BenefitPlanERPaymentBaseMethod
        {
            get { return m_BenefitPlanERPaymentBaseMethod; }
            set { m_BenefitPlanERPaymentBaseMethod = value; modify("BenefitPlanERPaymentBaseMethod"); }
        }

        protected int m_BenefitPlanERPaymentCodeID;
        [DBField("BenefitPlanERPaymentCodeID"), TextSearch, Export(false)]
        public int BenefitPlanERPaymentCodeID
        {
            get { return m_BenefitPlanERPaymentCodeID; }
            set { m_BenefitPlanERPaymentCodeID = value; modify("BenefitPlanERPaymentCodeID"); }
        }

        protected string m_BenefitPlanERMultiplier;
        [DBField("BenefitPlanERMultiplier"), TextSearch, MaxLength(50, 20), Export(false)]
        public string BenefitPlanERMultiplier
        {
            get { return m_BenefitPlanERMultiplier; }
            set { m_BenefitPlanERMultiplier = value; modify("BenefitPlanERMultiplier"); }
        }

        protected double m_BenefitPlanERAmount;
        [DBField("BenefitPlanERAmount", "0.00"), TextSearch, MaxLength(11), Export(false)]
        public double BenefitPlanERAmount
        {
            get { return m_BenefitPlanERAmount; }
            set { m_BenefitPlanERAmount = value; modify("BenefitPlanERAmount"); }
        }

        protected string m_BenefitPlanEEPaymentBaseMethod;
        [DBField("BenefitPlanEEPaymentBaseMethod"), TextSearch, Export(false)]
        public string BenefitPlanEEPaymentBaseMethod
        {
            get { return m_BenefitPlanEEPaymentBaseMethod; }
            set { m_BenefitPlanEEPaymentBaseMethod = value; modify("BenefitPlanEEPaymentBaseMethod"); }
        }

        protected int m_BenefitPlanEEPaymentCodeID;
        [DBField("BenefitPlanEEPaymentCodeID"), TextSearch, Export(false)]
        public int BenefitPlanEEPaymentCodeID
        {
            get { return m_BenefitPlanEEPaymentCodeID; }
            set { m_BenefitPlanEEPaymentCodeID = value; modify("BenefitPlanEEPaymentCodeID"); }
        }

        protected string m_BenefitPlanEEMultiplier;
        [DBField("BenefitPlanEEMultiplier"), TextSearch, MaxLength(50, 20), Export(false)]
        public string BenefitPlanEEMultiplier
        {
            get { return m_BenefitPlanEEMultiplier; }
            set { m_BenefitPlanEEMultiplier = value; modify("BenefitPlanEEMultiplier"); }
        }

        protected double m_BenefitPlanEEAmount;
        [DBField("BenefitPlanEEAmount", "0.00"), TextSearch, MaxLength(11), Export(false)]
        public double BenefitPlanEEAmount
        {
            get { return m_BenefitPlanEEAmount; }
            set { m_BenefitPlanEEAmount = value; modify("BenefitPlanEEAmount"); }
        }

        protected string m_BenefitPlanSpousePaymentBaseMethod;
        [DBField("BenefitPlanSpousePaymentBaseMethod"), TextSearch, Export(false)]
        public string BenefitPlanSpousePaymentBaseMethod
        {
            get { return m_BenefitPlanSpousePaymentBaseMethod; }
            set { m_BenefitPlanSpousePaymentBaseMethod = value; modify("BenefitPlanSpousePaymentBaseMethod"); }
        }

        protected int m_BenefitPlanSpousePaymentCodeID;
        [DBField("BenefitPlanSpousePaymentCodeID"), TextSearch, Export(false)]
        public int BenefitPlanSpousePaymentCodeID
        {
            get { return m_BenefitPlanSpousePaymentCodeID; }
            set { m_BenefitPlanSpousePaymentCodeID = value; modify("BenefitPlanSpousePaymentCodeID"); }
        }

        protected string m_BenefitPlanSpouseMultiplier;
        [DBField("BenefitPlanSpouseMultiplier"), TextSearch, MaxLength(50, 20), Export(false)]
        public string BenefitPlanSpouseMultiplier
        {
            get { return m_BenefitPlanSpouseMultiplier; }
            set { m_BenefitPlanSpouseMultiplier = value; modify("BenefitPlanSpouseMultiplier"); }
        }

        protected double m_BenefitPlanSpouseAmount;
        [DBField("BenefitPlanSpouseAmount", "0.00"), TextSearch, MaxLength(11), Export(false)]
        public double BenefitPlanSpouseAmount
        {
            get { return m_BenefitPlanSpouseAmount; }
            set { m_BenefitPlanSpouseAmount = value; modify("BenefitPlanSpouseAmount"); }
        }

        protected string m_BenefitPlanChildPaymentBaseMethod;
        [DBField("BenefitPlanChildPaymentBaseMethod"), TextSearch, Export(false)]
        public string BenefitPlanChildPaymentBaseMethod
        {
            get { return m_BenefitPlanChildPaymentBaseMethod; }
            set { m_BenefitPlanChildPaymentBaseMethod = value; modify("BenefitPlanChildPaymentBaseMethod"); }
        }

        protected int m_BenefitPlanChildPaymentCodeID;
        [DBField("BenefitPlanChildPaymentCodeID"), TextSearch, Export(false)]
        public int BenefitPlanChildPaymentCodeID
        {
            get { return m_BenefitPlanChildPaymentCodeID; }
            set { m_BenefitPlanChildPaymentCodeID = value; modify("BenefitPlanChildPaymentCodeID"); }
        }

        protected string m_BenefitPlanChildMultiplier;
        [DBField("BenefitPlanChildMultiplier"), TextSearch, MaxLength(50, 20), Export(false)]
        public string BenefitPlanChildMultiplier
        {
            get { return m_BenefitPlanChildMultiplier; }
            set { m_BenefitPlanChildMultiplier = value; modify("BenefitPlanChildMultiplier"); }
        }

        protected double m_BenefitPlanChildAmount;
        [DBField("BenefitPlanChildAmount", "0.00"), TextSearch, MaxLength(11), Export(false)]
        public double BenefitPlanChildAmount
        {
            get { return m_BenefitPlanChildAmount; }
            set { m_BenefitPlanChildAmount = value; modify("BenefitPlanChildAmount"); }
        }

    }
}
