using System;
using System.Data;
using System.Configuration;
using HROne.DataAccess;
////using perspectivemind.validation;

namespace HROne.Lib.Entities
{
    [DBClass("ClaimsAndDeductions")]

    public class EClaimsAndDeductions : BaseObject
    {
        public static DBManager db = new DBManager(typeof(EClaimsAndDeductions));

        protected int m_CNDID;
        [DBField("CNDID", true, true), TextSearch, Export(false)]
        public int CNDID
        {
            get { return m_CNDID; }
            set { m_CNDID = value; modify("CNDID"); }
        }

        protected DateTime m_CNDEffDate;
        [DBField("CNDEffDate"), TextSearch, Export(false), Required]
        public DateTime CNDEffDate
        {
            get { return m_CNDEffDate; }
            set { m_CNDEffDate = value; modify("CNDEffDate"); }
        }

        protected int m_EmpID;
        [DBField("EmpID"), TextSearch, Export(false), Required]
        public int EmpID
        {
            get { return m_EmpID; }
            set { m_EmpID = value; modify("EmpID"); }
        }

        protected int m_PayCodeID;
        [DBField("PayCodeID"), TextSearch, Export(false), Required]
        public int PayCodeID
        {
            get { return m_PayCodeID; }
            set { m_PayCodeID = value; modify("PayCodeID"); }
        }

        protected double m_CNDAmount;
        [DBField("CNDAmount", "0.00"), TextSearch, Export(false), Required]
        public double CNDAmount
        {
            get { return m_CNDAmount; }
            set { m_CNDAmount = value; modify("CNDAmount"); }
        }

        protected string m_CurrencyID;
        [DBField("CurrencyID"), TextSearch, Export(false), Required]
        public string CurrencyID
        {
            get { return m_CurrencyID; }
            set { m_CurrencyID = value; modify("CurrencyID"); }
        }

        protected string m_CNDPayMethod;
        [DBField("CNDPayMethod"), TextSearch, Export(false), Required]
        public string CNDPayMethod
        {
            get { return m_CNDPayMethod; }
            set { m_CNDPayMethod = value; modify("CNDPayMethod"); }
        }

        protected int m_EmpAccID;
        [DBField("EmpAccID"), TextSearch, Export(false)]
        public int EmpAccID
        {
            get { return m_EmpAccID; }
            set { m_EmpAccID = value; modify("EmpAccID"); }
        }

        protected double m_CNDNumOfDayAdj;
        [DBField("CNDNumOfDayAdj", "0.##"), MaxLength(5), TextSearch, Export(false)]
        public double CNDNumOfDayAdj
        {
            get { return m_CNDNumOfDayAdj; }
            set { m_CNDNumOfDayAdj = value; modify("CNDNumOfDayAdj"); }
        }

        protected bool m_CNDIsRestDayPayment;
        [DBField("CNDIsRestDayPayment"), TextSearch, Export(false)]
        public bool CNDIsRestDayPayment
        {
            get { return m_CNDIsRestDayPayment; }
            set { m_CNDIsRestDayPayment = value; modify("CNDIsRestDayPayment"); }
        }

        protected int m_CostCenterID;
        [DBField("CostCenterID"), TextSearch, Export(false)]
        public int CostCenterID
        {
            get { return m_CostCenterID; }
            set { m_CostCenterID = value; modify("CostCenterID"); }
        }

        protected string m_CNDRemark;
        [DBField("CNDRemark"), TextSearch, Export(false)]
        public String CNDRemark
        {
            get { return m_CNDRemark; }
            set { m_CNDRemark = value; modify("CNDRemark"); }
        }

        protected object m_EmpPayrollID;
        [DBField("EmpPayrollID"), TextSearch, Export(false)]
        public object EmpPayrollID
        {
            get { return m_EmpPayrollID; }
            set { m_EmpPayrollID = value; modify("EmpPayrollID"); }
        }

        protected object m_PayRecID;
        [DBField("PayRecID"), TextSearch, Export(false)]
        public object PayRecID
        {
            get { return m_PayRecID; }
            set { m_PayRecID = value; modify("PayRecID"); }
        }

        protected int m_CNDImportBatchID;
        [DBField("CNDImportBatchID"), TextSearch, Export(false)]
        public int CNDImportBatchID
        {
            get { return m_CNDImportBatchID; }
            set { m_CNDImportBatchID = value; modify("CNDImportBatchID"); }
        }
        
    }
}