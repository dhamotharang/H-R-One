using System;
using HROne.DataAccess;

namespace HROne.Lib.Entities
{
    [DBClass("EmpBenefit")]
    public class EEmpBenefit : BaseObjectWithRecordInfo
    {
        public static DBManager db = new DBManagerWithRecordInfo(typeof(EEmpBenefit));

        protected int m_EmpBenefitID;
        [DBField("EmpBenefitID", true, true), TextSearch, Export(false)]
        public int EmpBenefitID
        {
            get { return m_EmpBenefitID; }
            set { m_EmpBenefitID = value; modify("EmpBenefitID"); }
        }

        protected int m_EmpID;
        [DBField("EmpID"), TextSearch, Export(false)]
        public int EmpID
        {
            get { return m_EmpID; }
            set { m_EmpID = value; modify("EmpID"); }
        }

        protected DateTime m_EmpBenefitEffectiveDate;
        [DBField("EmpBenefitEffectiveDate"), TextSearch, Export(false), Required]
        public DateTime EmpBenefitEffectiveDate
        {
            get { return m_EmpBenefitEffectiveDate; }
            set { m_EmpBenefitEffectiveDate = value; base.modify("EmpBenefitEffectiveDate"); }
        }

        protected DateTime m_EmpBenefitExpiryDate;
        [DBField("EmpBenefitExpiryDate"), TextSearch, Export(false)]
        public DateTime EmpBenefitExpiryDate
        {
            get { return m_EmpBenefitExpiryDate; }
            set { m_EmpBenefitExpiryDate = value; base.modify("EmpBenefitExpiryDate"); }
        }

        protected int m_EmpBenefitPlanID;
        [DBField("EmpBenefitPlanID"), TextSearch, Export(false), Required]
        public int EmpBenefitPlanID
        {
            get { return m_EmpBenefitPlanID; }
            set { m_EmpBenefitPlanID = value; modify("EmpBenefitPlanID"); }
        }

        protected double m_EmpBenefitERPremium;
        [DBField("EmpBenefitERPremium", "0.00"), TextSearch, MaxLength(11), Export(false)]
        public double EmpBenefitERPremium
        {
            get { return m_EmpBenefitERPremium; }
            set { m_EmpBenefitERPremium = value; modify("EmpBenefitERPremium"); }
        }

        protected double m_EmpBenefitEEPremium;
        [DBField("EmpBenefitEEPremium", "0.00"), TextSearch, MaxLength(11), Export(false)]
        public double EmpBenefitEEPremium
        {
            get { return m_EmpBenefitEEPremium; }
            set { m_EmpBenefitEEPremium = value; modify("EmpBenefitEEPremium"); }
        }

        protected double m_EmpBenefitSpousePremium;
        [DBField("EmpBenefitSpousePremium", "0.00"), TextSearch, MaxLength(11), Export(false)]
        public double EmpBenefitSpousePremium
        {
            get { return m_EmpBenefitSpousePremium; }
            set { m_EmpBenefitSpousePremium = value; modify("EmpBenefitSpousePremium"); }
        }

        protected double m_EmpBenefitChildPremium;
        [DBField("EmpBenefitChildPremium", "0.00"), TextSearch, MaxLength(11), Export(false)]
        public double EmpBenefitChildPremium
        {
            get { return m_EmpBenefitChildPremium; }
            set { m_EmpBenefitChildPremium = value; modify("EmpBenefitChildPremium"); }
        }

        //  For Synchronize Use only
        protected string m_SynID;
        [DBField("SynID"), TextSearch, Export(false)]
        public string SynID
        {
            get { return m_SynID; }
            set { m_SynID = value; modify("SynID"); }
        }
    }
    
}
