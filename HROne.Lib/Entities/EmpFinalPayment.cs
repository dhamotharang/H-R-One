using System;
using System.Collections.Generic;
using System.Text;
using HROne.DataAccess;
////using perspectivemind.validation;

namespace HROne.Lib.Entities
{
    [DBClass("EmpFinalPayment")]
    public class EEmpFinalPayment : BaseObjectWithRecordInfo
    {
        public static DBManager db = new DBManagerWithRecordInfo(typeof(EEmpFinalPayment));
        protected int m_EmpFinalPayID;
        [DBField("EmpFinalPayID", true, true), TextSearch, Export(false)]
        public int EmpFinalPayID
        {
            get { return m_EmpFinalPayID; }
            set { m_EmpFinalPayID = value; modify("EmpFinalPayID"); }
        }
        protected int m_EmpID;
        [DBField("EmpID"), TextSearch, Export(false)]
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
        protected double m_EmpFinalPayAmount;
        [DBField("EmpFinalPayAmount", "0.00"), TextSearch, Export(false), Required]
        public double EmpFinalPayAmount
        {
            get { return m_EmpFinalPayAmount; }
            set { m_EmpFinalPayAmount = value; modify("EmpFinalPayAmount"); }
        }
        protected string m_CurrencyID;
        [DBField("CurrencyID"), TextSearch, Export(false)]
        public string CurrencyID
        {
            get { return m_CurrencyID; }
            set { m_CurrencyID = value; modify("CurrencyID"); }
        }
        protected string m_EmpFinalPayMethod;
        [DBField("EmpFinalPayMethod"), TextSearch, Export(false), Required]
        public string EmpFinalPayMethod
        {
            get { return m_EmpFinalPayMethod; }
            set { m_EmpFinalPayMethod = value; modify("EmpFinalPayMethod"); }
        }
        protected int m_EmpAccID;
        [DBField("EmpAccID"), TextSearch, Export(false)]
        public int EmpAccID
        {
            get { return m_EmpAccID; }
            set { m_EmpAccID = value; modify("EmpAccID"); }
        }

        protected int m_CostCenterID;
        [DBField("CostCenterID"), TextSearch, Export(false)]
        public int CostCenterID
        {
            get { return m_CostCenterID; }
            set { m_CostCenterID = value; modify("CostCenterID"); }
        }

        protected string m_EmpFinalPayRemark;
        [DBField("EmpFinalPayRemark"), TextSearch, DBAESEncryptStringField, Export(false)]
        public string EmpFinalPayRemark
        {
            get { return m_EmpFinalPayRemark; }
            set { m_EmpFinalPayRemark = value; modify("EmpFinalPayRemark"); }
        }
        protected bool m_EmpFinalPayIsAutoGen;
        [DBField("EmpFinalPayIsAutoGen"), TextSearch, Export(false)]
        public bool EmpFinalPayIsAutoGen
        {
            get { return m_EmpFinalPayIsAutoGen; }
            set { m_EmpFinalPayIsAutoGen = value; modify("EmpFinalPayIsAutoGen"); }
        }
        protected double m_EmpFinalPayNumOfDayAdj;
        [DBField("EmpFinalPayNumOfDayAdj", "0.##"), MaxLength(5), TextSearch, Export(false)]
        public double EmpFinalPayNumOfDayAdj
        {
            get { return m_EmpFinalPayNumOfDayAdj; }
            set { m_EmpFinalPayNumOfDayAdj = value; modify("EmpFinalPayNumOfDayAdj"); }
        }

        protected bool m_EmpFinalPayIsRestDayPayment;
        [DBField("EmpFinalPayIsRestDayPayment"), TextSearch, Export(false)]
        public bool EmpFinalPayIsRestDayPayment
        {
            get { return m_EmpFinalPayIsRestDayPayment; }
            set { m_EmpFinalPayIsRestDayPayment = value; modify("EmpFinalPayIsRestDayPayment"); }
        }

        protected int m_LeaveAppID;
        [DBField("LeaveAppID"), TextSearch, Export(false)]
        public int LeaveAppID
        {
            get { return m_LeaveAppID; }
            set { m_LeaveAppID = value; modify("LeaveAppID"); }
        }

        protected string m_LeaveAppIDList;
        [DBField("LeaveAppIDList"), TextSearch, Export(false)]
        public string LeaveAppIDList
        {
            get { return m_LeaveAppIDList; }
            set { m_LeaveAppIDList = value; modify("LeaveAppIDList"); }
        }

        public object m_PayRecID;
        [DBField("PayRecID"), TextSearch, Export(false)]
        public object PayRecID
        {
            get { return m_PayRecID; }
            set { m_PayRecID = value; modify("PayRecID"); }
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
