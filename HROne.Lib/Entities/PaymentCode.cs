using System;
using System.Collections.Generic;
using System.Text;
using HROne.DataAccess;
////using perspectivemind.validation;
using HROne.Common;
 
namespace HROne.Lib.Entities
{
	[DBClass("PaymentCode")]
    public class EPaymentCode : BaseObject
    {
		public static DBManager db=new DBManager(typeof(EPaymentCode));

        public static WFValueList VLPaymentCode = new AppUtils.EncryptedDBCodeList(EPaymentCode.db, "PaymentCodeID", new string[] { "PaymentCode", "PaymentCodeDesc" }, " - ", "PaymentCode");

        public static EPaymentCode GetObject(DatabaseConnection dbConn, int ID)
        {
            if (ID > 0)
            {
                EPaymentCode obj = new EPaymentCode();
                obj.PaymentCodeID = (int)ID;
                if (db.select(dbConn, obj))
                    return obj;
            }
            return null;
        }

        public static EPaymentCode GetObject(DatabaseConnection dbConn, string Code)
        {
            if (Code != null && Code != "")
            {
                DBFilter m_filter = new DBFilter();
                m_filter.add(new Match("PaymentCode", Code));

                foreach (EPaymentCode o in db.select(dbConn, m_filter))
                {
                    return o;
                }
            }
            return null;
        }

        protected int m_PaymentCodeID;
        [DBField("PaymentCodeID",true,true), TextSearch, Export(false)]
        public int PaymentCodeID
        {
            get { return m_PaymentCodeID; }
            set { m_PaymentCodeID = value; modify("PaymentCodeID"); }
        }
        protected string m_PaymentCode;
        [DBField("PaymentCode"), TextSearch, DBAESEncryptStringField, MaxLength(20, 20), Export(false), Required]
        public string PaymentCode
        {
            get { return m_PaymentCode; }
            set { m_PaymentCode = value.Trim().ToUpper(); modify("PaymentCode"); }
        }
        protected string m_PaymentCodeDesc;
        [DBField("PaymentCodeDesc"), TextSearch, DBAESEncryptStringField, MaxLength(100, 50), Export(false), Required]
        public string PaymentCodeDesc
        {
            get { return m_PaymentCodeDesc; }
            set { m_PaymentCodeDesc = value; modify("PaymentCodeDesc"); }
        }
        protected int m_PaymentTypeID;
        [DBField("PaymentTypeID"), TextSearch, Export(false), Required]
        public int PaymentTypeID
        {
            get { return m_PaymentTypeID; }
            set { m_PaymentTypeID = value; modify("PaymentTypeID"); }
        }

        protected bool m_PaymentCodeIsProrata;
        [DBField("PaymentCodeIsProrata"), TextSearch, Export(false), Required]
        public bool PaymentCodeIsProrata
        {
            get { return m_PaymentCodeIsProrata; }
            set { m_PaymentCodeIsProrata = value; modify("PaymentCodeIsProrata"); }
        }
        protected bool m_PaymentCodeIsProrataLeave;
        [DBField("PaymentCodeIsProrataLeave"), TextSearch, Export(false), Required]
        public bool PaymentCodeIsProrataLeave
        {
            get { return m_PaymentCodeIsProrataLeave; }
            set { m_PaymentCodeIsProrataLeave = value; modify("PaymentCodeIsProrataLeave"); }
        }
        protected bool m_PaymentCodeIsProrataStatutoryHoliday;
        [DBField("PaymentCodeIsProrataStatutoryHoliday"), TextSearch, Export(false), Required]
        public bool PaymentCodeIsProrataStatutoryHoliday
        {
            get { return m_PaymentCodeIsProrataStatutoryHoliday; }
            set { m_PaymentCodeIsProrataStatutoryHoliday = value; modify("PaymentCodeIsProrataStatutoryHoliday"); }
        }        

        protected bool m_PaymentCodeIsMPF;
        [DBField("PaymentCodeIsMPF"), TextSearch, Export(false), Required]
        public bool PaymentCodeIsMPF
        {
            get { return m_PaymentCodeIsMPF; }
            set { m_PaymentCodeIsMPF = value; modify("PaymentCodeIsMPF"); }
        }
        protected bool m_PaymentCodeIsTopUp;
        [DBField("PaymentCodeIsTopUp"), TextSearch, Export(false), Required]
        public bool PaymentCodeIsTopUp
        {
            get { return m_PaymentCodeIsTopUp; }
            set { m_PaymentCodeIsTopUp = value; modify("PaymentCodeIsTopUp"); }
        }
        protected bool m_PaymentCodeIsWages;
        [DBField("PaymentCodeIsWages"), TextSearch, Export(false), Required]
        public bool PaymentCodeIsWages
        {
            get { return m_PaymentCodeIsWages; }
            set { m_PaymentCodeIsWages = value; modify("PaymentCodeIsWages"); }
        }

        protected bool m_PaymentCodeIsORSO;
        [DBField("PaymentCodeIsORSO"), TextSearch, Export(false), Required]
        public bool PaymentCodeIsORSO
        {
            get { return m_PaymentCodeIsORSO; }
            set { m_PaymentCodeIsORSO = value; modify("PaymentCodeIsORSO"); }
        }

        protected string m_PaymentCodeRoundingRule;
        [DBField("PaymentCodeRoundingRule"), TextSearch, MaxLength(50, 25), Export(false), Required]
        public string PaymentCodeRoundingRule
        {
            get { return m_PaymentCodeRoundingRule; }
            set { m_PaymentCodeRoundingRule = value; modify("PaymentCodeRoundingRule"); }
        }

        protected int m_PaymentCodeDecimalPlace;
        [DBField("PaymentCodeDecimalPlace"), TextSearch, Export(false), Required]
        public int PaymentCodeDecimalPlace 
        {
            get { return m_PaymentCodeDecimalPlace; }
            set { m_PaymentCodeDecimalPlace = value; modify("PaymentCodeDecimalPlace"); }
        }

        protected bool m_PaymentCodeRoundingRuleIsAbsoluteValue;
        [DBField("PaymentCodeRoundingRuleIsAbsoluteValue"), TextSearch, Export(false)]
        public bool PaymentCodeRoundingRuleIsAbsoluteValue
        {
            get { return m_PaymentCodeRoundingRuleIsAbsoluteValue; }
            set { m_PaymentCodeRoundingRuleIsAbsoluteValue = value; modify("PaymentCodeRoundingRuleIsAbsoluteValue"); }
        }

        protected bool m_PaymentCodeHideInPaySlip;
        [DBField("PaymentCodeHideInPaySlip"), TextSearch, Export(false)]
        public bool PaymentCodeHideInPaySlip
        {
            get { return m_PaymentCodeHideInPaySlip; }
            set { m_PaymentCodeHideInPaySlip = value; modify("PaymentCodeHideInPaySlip"); }
        }

        protected int m_PaymentCodeDisplaySeqNo;
        [DBField("PaymentCodeDisplaySeqNo"), TextSearch, MaxLength(2), Export(false), Required]
        public int PaymentCodeDisplaySeqNo
        {
            get { return m_PaymentCodeDisplaySeqNo; }
            set { m_PaymentCodeDisplaySeqNo = value; modify("PaymentCodeDisplaySeqNo"); }
        }

        protected bool m_PaymentCodeNotRemoveContributionFromTopUp;
        [DBField("PaymentCodeNotRemoveContributionFromTopUp"), TextSearch, Export(false), Required]
        public bool PaymentCodeNotRemoveContributionFromTopUp
        {
            get { return m_PaymentCodeNotRemoveContributionFromTopUp; }
            set { m_PaymentCodeNotRemoveContributionFromTopUp = value; modify("PaymentCodeNotRemoveContributionFromTopUp"); }
        }
        // Start 000159, Ricky So, 2015-01-23
        protected bool m_PaymentCodeIsHitRateBased;
        [DBField("PaymentCodeIsHitRateBased"), TextSearch, Export(false)]
        public bool PaymentCodeIsHitRateBased
        {
            get { return m_PaymentCodeIsHitRateBased; }
            set { m_PaymentCodeIsHitRateBased = value; modify("PaymentCodeIsHitRateBased"); }
        }

        protected double m_PaymentCodeDefaultRateAtMonth1;
        [DBField("PaymentCodeDefaultRateAtMonth1", "0.00"), TextSearch, MaxLength(6), Export(false)]
        public double PaymentCodeDefaultRateAtMonth1
        {
            get { return m_PaymentCodeDefaultRateAtMonth1; }
            set { m_PaymentCodeDefaultRateAtMonth1 = value; modify("PaymentCodeDefaultRateAtMonth1"); }
        }

        protected double m_PaymentCodeDefaultRateAtMonth2;
        [DBField("PaymentCodeDefaultRateAtMonth2", "0.00"), TextSearch, MaxLength(6), Export(false)]
        public double PaymentCodeDefaultRateAtMonth2
        {
            get { return m_PaymentCodeDefaultRateAtMonth2; }
            set { m_PaymentCodeDefaultRateAtMonth2 = value; modify("PaymentCodeDefaultRateAtMonth2"); }
        }

        protected double m_PaymentCodeDefaultRateAtMonth3;
        [DBField("PaymentCodeDefaultRateAtMonth3", "0.00"), TextSearch, MaxLength(6), Export(false)]
        public double PaymentCodeDefaultRateAtMonth3
        {
            get { return m_PaymentCodeDefaultRateAtMonth3; }
            set { m_PaymentCodeDefaultRateAtMonth3 = value; modify("PaymentCodeDefaultRateAtMonth3"); }
        }

        // End 000159, Ricky So, 2015-01-23
    }
}
