using System;
using System.Collections;
using System.Text;
using HROne.Lib.Entities;
using HROne.Common;
using CrystalDecisions.CrystalReports.Engine;
using HROne.DataAccess;

namespace HROne.BankFile
{
    public class AutopayListProcess:GenericReportProcess 
    {
        private ArrayList EmpList;
        private ArrayList PayBatchList;
        private DateTime PayPeriodFr;
        System.Data.DataSet dataSet = null;

        public AutopayListProcess(DatabaseConnection dbConn, ArrayList EmpList, ArrayList PayBatchList, int PayPeriodID)
            : base(dbConn)
        {
            this.EmpList = EmpList;
            this.PayBatchList = PayBatchList;

            EPayrollPeriod payPeriod = new EPayrollPeriod();
            payPeriod.PayPeriodID = PayPeriodID;
            if (EPayrollPeriod.db.select(dbConn, payPeriod))
                PayPeriodFr = payPeriod.PayPeriodFr;
        }

        public AutopayListProcess(DatabaseConnection dbConn, ArrayList EmpList, ArrayList PayBatchList, DateTime PayPeriodFr)
            : base(dbConn)
        {
            this.EmpList = EmpList;
            this.PayBatchList = PayBatchList;
            this.PayPeriodFr = PayPeriodFr;
        }

        public AutopayListProcess(DatabaseConnection dbConn, System.Data.DataSet dataSet, int PayPeriodID)
            : base(dbConn)
        {
            this.dataSet = dataSet;
            EPayrollPeriod payPeriod = new EPayrollPeriod();
            payPeriod.PayPeriodID = PayPeriodID;
            if (EPayrollPeriod.db.select(dbConn, payPeriod))
                PayPeriodFr = payPeriod.PayPeriodFr;
        }

        public AutopayListProcess(System.Data.DataSet dataSet, DateTime PayPeriodFr)
            : base(null)
        {
            this.dataSet = dataSet;
            this.PayPeriodFr = PayPeriodFr;
        }

        public AutopayListProcess(DatabaseConnection dbConn, System.Data.DataSet dataSet, DateTime PayPeriodFr, System.Globalization.CultureInfo reportCultureInfo)
            : base(dbConn, reportCultureInfo)
        {
            this.dataSet = dataSet;
            this.PayPeriodFr = PayPeriodFr;
        }

        public override ReportDocument GenerateReport()
        {

            if (dataSet == null)
            {
                GenericBankFile bankFileObject = null;

                bankFileObject = new GenericBankFile(dbConn);
                bankFileObject.LoadBankFileDetail(PayBatchList, EmpList);
                if (bankFileObject != null)
                    dataSet = bankFileObject.CreateAutopayListDataSet();
            }
            if (reportDocument == null)
            {
                reportDocument = new ReportTemplate.Report_Payroll_AutopayList();
            }
            else
            {

            }
            return base.GenerateReport();
        }
        protected override void setDataSource()
        {
            reportDocument.SetDataSource(dataSet);
        }
        protected override void setParameters()
        {
            reportDocument.SetParameterValue("PayPeriodFr", PayPeriodFr);
        }
    }
}
