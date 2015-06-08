using System;
using System.Collections.Generic;
using System.Text;
using HROne.DataAccess;
////using perspectivemind.validation;

namespace HROne.Lib.Entities
{
    [DBClass("EmpRecurringPayment")]
    public class EEmpRecurringPayment : BaseObjectWithRecordInfo
    {
        public static DBManager db = new DBManagerWithRecordInfo(typeof(EEmpRecurringPayment));

        public static EEmpRecurringPayment GetObject(DatabaseConnection dbConn, int ID)
        {
            if (ID > 0)
            {
                EEmpRecurringPayment obj = new EEmpRecurringPayment();
                obj.EmpRPID = (int)ID;
                if (db.select(dbConn, obj))
                    return obj;
            }
            return null;
        }       
        
        protected int m_EmpRPID;
        [DBField("EmpRPID", true, true), TextSearch, Export(false)]
        public int EmpRPID
        {
            get { return m_EmpRPID; }
            set { m_EmpRPID = value; modify("EmpRPID"); }
        }
        protected int m_EmpID;
        [DBField("EmpID"), TextSearch, Export(false)]
        public int EmpID
        {
            get { return m_EmpID; }
            set { m_EmpID = value; modify("EmpID"); }
        }
        protected DateTime m_EmpRPEffFr;
        [DBField("EmpRPEffFr"), TextSearch, Export(false),Required]
        public DateTime EmpRPEffFr
        {
            get { return m_EmpRPEffFr; }
            set { m_EmpRPEffFr = value; modify("EmpRPEffFr"); }
        }
        protected DateTime m_EmpRPEffTo;
        [DBField("EmpRPEffTo"), TextSearch, Export(false)]
        public DateTime EmpRPEffTo
        {
            get { return m_EmpRPEffTo; }
            set { m_EmpRPEffTo = value; modify("EmpRPEffTo"); }
        }
        protected int m_PayCodeID;
        [DBField("PayCodeID"), TextSearch, Export(false),Required]
        public int PayCodeID
        {
            get { return m_PayCodeID; }
            set { m_PayCodeID = value; modify("PayCodeID"); }
        }


		protected double m_EmpRPBasicSalary;
        [DBField("EmpRPBasicSalary","0.00"), TextSearch, MaxLength(11), Export(false)]
        public double EmpRPBasicSalary
        {
            get { return m_EmpRPBasicSalary; }
            set { m_EmpRPBasicSalary = value; modify("EmpRPBasicSalary"); }
        }
        
        protected double m_EmpRPFPS;
        [DBField("EmpRPFPS","0.00"), TextSearch, MaxLength(6), Export(false)]
        public double EmpRPFPS
        {
            get { return m_EmpRPFPS; }
            set { m_EmpRPFPS = value; modify("EmpRPFPS"); }
        }

        protected double m_EmpRPAmount;
        [DBField("EmpRPAmount","0.00"), TextSearch, MaxLength(11), Export(false), Required]
        public double EmpRPAmount
        {
            get { return m_EmpRPAmount; }
            set { m_EmpRPAmount = value; modify("EmpRPAmount"); }
        }
        protected string m_CurrencyID;
        [DBField("CurrencyID"), TextSearch, Export(false)]
        public string CurrencyID
        {
            get { return m_CurrencyID; }
            set { m_CurrencyID = value; modify("CurrencyID"); }
        }
        protected string m_EmpRPUnit;
        [DBField("EmpRPUnit"), TextSearch, Export(false),Required]
        public string EmpRPUnit
        {
            get { return m_EmpRPUnit; }
            set { m_EmpRPUnit = value; modify("EmpRPUnit"); }
        }
        protected bool m_EmpRPUnitPeriodAsDaily;
        [DBField("EmpRPUnitPeriodAsDaily"), TextSearch, Export(false)]
        public bool EmpRPUnitPeriodAsDaily
        {
            get { return m_EmpRPUnitPeriodAsDaily; }
            set { m_EmpRPUnitPeriodAsDaily = value; modify("EmpRPUnitPeriodAsDaily"); }
        }
        protected int m_EmpRPUnitPeriodAsDailyPayFormID;
        [DBField("EmpRPUnitPeriodAsDailyPayFormID"), TextSearch, Export(false)]
        public int EmpRPUnitPeriodAsDailyPayFormID
        {
            get { return m_EmpRPUnitPeriodAsDailyPayFormID; }
            set { m_EmpRPUnitPeriodAsDailyPayFormID = value; modify("EmpRPUnitPeriodAsDailyPayFormID"); }
        }
        protected string m_EmpRPMethod;
        [DBField("EmpRPMethod"), TextSearch, Export(false),Required]
        public string EmpRPMethod
        {
            get { return m_EmpRPMethod; }
            set { m_EmpRPMethod = value; modify("EmpRPMethod"); }
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
        protected string m_EmpRPRemark;
        [DBField("EmpRPRemark"), TextSearch, Export(false)]
        public string EmpRPRemark
        {
            get { return m_EmpRPRemark; }
            set { m_EmpRPRemark = value; modify("EmpRPRemark"); }
        }
        protected bool m_EEmpRPIsNonPayrollItem;
        [DBField("EmpRPIsNonPayrollItem"), TextSearch, Export(false)]
        public bool EmpRPIsNonPayrollItem
        {
            get { return m_EEmpRPIsNonPayrollItem; }
            set { m_EEmpRPIsNonPayrollItem = value; modify("EmpRPIsNonPayrollItem"); }
        }

        //  For Synchronize Use only
        protected string m_SynID;
        [DBField("SynID"), TextSearch, Export(false)]
        public string SynID
        {
            get { return m_SynID; }
            set { m_SynID = value; modify("SynID"); }
        }

        protected string m_SchemeCode;
        [DBField("SchemeCode"), MaxLength(10, 10), TextSearch, DBAESEncryptStringField, Export(true)]
        public String SchemeCode
        {
            get { return m_SchemeCode; }
            set { m_SchemeCode = value; modify("SchemeCode"); }
        }

        protected String m_Capacity;
        [DBField("Capacity"), MaxLength(70, 60), TextSearch, DBAESEncryptStringField, Export(true)]
        public String Capacity
        {
            get { return m_Capacity; }
            set { m_Capacity = value; modify("Capacity"); }
        }

        protected Decimal m_Point;
        [DBField("Point", "0.00"), MaxLength(5), Export(true)]
        public Decimal Point
        {
            get { return m_Point; }
            set { m_Point = value; modify("Point"); }
        }

    }
}
