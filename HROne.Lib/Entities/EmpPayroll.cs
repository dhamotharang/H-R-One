using System;
using System.Collections;
using System.Data;
using System.Configuration;
using HROne.DataAccess;
////using perspectivemind.validation;
using HROne.Common;

namespace HROne.Lib.Entities
{
    [DBClass("EmpPayroll")]

    public class EEmpPayroll : BaseObject
    {
        public static DBManager db = new DBManager(typeof(EEmpPayroll));

        public static EEmpPayroll GetObject(DatabaseConnection dbConn, int ID)
        {
            if (ID > 0)
            {
                EEmpPayroll obj = new EEmpPayroll();
                obj.EmpPayrollID = ID;
                if (EEmpPayroll.db.select(dbConn, obj))
                    return obj;
            }
            return null;
        }

        protected int m_EmpPayrollID;
        [DBField("EmpPayrollID", true, true), TextSearch, Export(false)]
        public int EmpPayrollID
        {
            get { return m_EmpPayrollID; }
            set { m_EmpPayrollID = value; modify("EmpPayrollID"); }
        }

        protected int m_PayPeriodID;
        [DBField("PayPeriodID"), TextSearch, Export(false), Required]
        public int PayPeriodID
        {
            get { return m_PayPeriodID; }
            set { m_PayPeriodID = value; modify("PayPeriodID"); }
        }

        protected int m_EmpID;
        [DBField("EmpID"), TextSearch, Export(false), Required]
        public int EmpID
        {
            get { return m_EmpID; }
            set { m_EmpID = value; modify("EmpID"); }
        }

        /*  Payroll Status
         * T - Trial Run
         * C - Confirm
         */
        protected string m_EmpPayStatus;
        [DBField("EmpPayStatus"), TextSearch, Export(false), Required]
        public string EmpPayStatus
        {
            get { return m_EmpPayStatus; }
            set { m_EmpPayStatus = value; modify("EmpPayStatus"); }
        }

        protected int m_PayBatchID;
        [DBField("PayBatchID"), TextSearch, Export(false)]
        public int PayBatchID
        {
            get { return m_PayBatchID; }
            set { m_PayBatchID = value; modify("PayBatchID"); }
        }

        protected string m_EmpPayIsRP;
        [DBField("EmpPayIsRP"), TextSearch, Export(false), Required]
        public string EmpPayIsRP
        {
            get { return m_EmpPayIsRP; }
            set { m_EmpPayIsRP = value; modify("EmpPayIsRP"); }
        }

        protected string m_EmpPayIsCND;
        [DBField("EmpPayIsCND"), TextSearch, Export(false), Required]
        public string EmpPayIsCND
        {
            get { return m_EmpPayIsCND; }
            set { m_EmpPayIsCND = value; modify("EmpPayIsCND"); }
        }

        protected string m_EmpPayIsYEB;
        [DBField("EmpPayIsYEB"), TextSearch, Export(false), Required]
        public string EmpPayIsYEB
        {
            get { return m_EmpPayIsYEB; }
            set { m_EmpPayIsYEB = value; modify("EmpPayIsYEB"); }
        }

        protected string m_EmpPayIsAdditionalRemuneration;
        [DBField("EmpPayIsAdditionalRemuneration"), TextSearch, Export(false), Required]
        public string EmpPayIsAdditionalRemuneration
        {
            get { return m_EmpPayIsAdditionalRemuneration; }
            set { m_EmpPayIsAdditionalRemuneration = value; modify("EmpPayIsAdditionalRemuneration"); }
        }
        
        protected string m_EmpPayIsHistoryAdj;
        [DBField("EmpPayIsHistoryAdj"), TextSearch, Export(false), Required]
        public string EmpPayIsHistoryAdj
        {
            get { return m_EmpPayIsHistoryAdj; }
            set { m_EmpPayIsHistoryAdj = value; modify("EmpPayIsHistoryAdj"); }
        }

        protected DateTime m_EmpPayTrialRunDate;
        [DBField("EmpPayTrialRunDate", "yyyy-MM-dd HH:mm:ss"), TextSearch, Export(false)]
        public DateTime EmpPayTrialRunDate
        {
            get { return m_EmpPayTrialRunDate; }
            set { m_EmpPayTrialRunDate = value; modify("EmpPayTrialRunDate"); }
        }

        protected int m_EmpPayTrialRunBy;
        [DBField("EmpPayTrialRunBy"), TextSearch, Export(false)]
        public int EmpPayTrialRunBy
        {
            get { return m_EmpPayTrialRunBy; }
            set { m_EmpPayTrialRunBy = value; modify("EmpPayTrialRunBy"); }
        }

        protected DateTime m_EmpPayConfirmDate;
        [DBField("EmpPayConfirmDate", "yyyy-MM-dd HH:mm:ss"), TextSearch, Export(false)]
        public DateTime EmpPayConfirmDate
        {
            get { return m_EmpPayConfirmDate; }
            set { m_EmpPayConfirmDate = value; modify("EmpPayConfirmDate"); }
        }

        protected int m_EmpPayConfirmBy;
        [DBField("EmpPayConfirmBy"), TextSearch, Export(false)]
        public int EmpPayConfirmBy
        {
            get { return m_EmpPayConfirmBy; }
            set { m_EmpPayConfirmBy = value; modify("EmpPayConfirmBy"); }
        }

        protected double m_EmpPayNumOfDayCount;
        [Obsolete, DBField("EmpPayNumOfDayCount"), TextSearch, Export(false)]
        public double EmpPayNumOfDayCount
        {
            get { return m_EmpPayNumOfDayCount; }
            set { m_EmpPayNumOfDayCount = value; modify("EmpPayNumOfDayCount"); }
        }

        protected double m_EmpPayTotalWorkingHours;
        [DBField("EmpPayTotalWorkingHours", "0.####"), TextSearch, MaxLength(10), Export(false)]
        public double EmpPayTotalWorkingHours
        {
            get { return m_EmpPayTotalWorkingHours; }
            set { m_EmpPayTotalWorkingHours = value; modify("EmpPayTotalWorkingHours"); }
        }

        protected DateTime m_EmpPayValueDate;
        [DBField("EmpPayValueDate"), TextSearch, Export(false)]
        public DateTime EmpPayValueDate
        {
            get { return m_EmpPayValueDate; }
            set { m_EmpPayValueDate = value; modify("EmpPayValueDate"); }
        }


        protected string m_EmpPayRemark;
        [DBField("EmpPayRemark"), DBAESEncryptStringField, TextSearch, Export(false)]
        public string EmpPayRemark
        {
            get { return m_EmpPayRemark; }
            set { m_EmpPayRemark = value; modify("EmpPayRemark"); }
        }
        public string[] PayrollProcessTypeDescription()
        {
            System.Collections.Generic.List<string> processTypeDescriptionList = new System.Collections.Generic.List<string>();


            if (m_EmpPayIsRP.Equals("Y"))
                processTypeDescriptionList.Add("Recurring Payment");
            if (m_EmpPayIsCND.Equals("Y"))
                processTypeDescriptionList.Add("Claims and Deductions");
            if (!string.IsNullOrEmpty(m_EmpPayIsYEB))
            if (m_EmpPayIsYEB.Equals("Y"))
                processTypeDescriptionList.Add("Year End Bonus");
            if (!string.IsNullOrEmpty(m_EmpPayIsAdditionalRemuneration))
                if (m_EmpPayIsAdditionalRemuneration.Equals("Y"))
                    processTypeDescriptionList.Add("Additional Remuneration");
            return processTypeDescriptionList.ToArray();
        }

        public double GetTotalPaymentAmount(DatabaseConnection dbConn)
        {
            double totalPaymentAmount = 0;
            DBFilter payrollRecordFilter = new DBFilter();
            payrollRecordFilter.add(new Match("EmpPayrollID", EmpPayrollID));

            ArrayList list = EPaymentRecord.db.select(dbConn, payrollRecordFilter);

            foreach (EPaymentRecord payrollRecord in list)
                totalPaymentAmount += payrollRecord.PayRecActAmount;

            return totalPaymentAmount;
        }
    }
}
