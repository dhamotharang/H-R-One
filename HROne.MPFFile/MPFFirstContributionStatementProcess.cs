using System;
using System.Collections;
using System.Text;
using HROne.Common;
using HROne.Lib.Entities;
using HROne.DataAccess;
using CrystalDecisions.CrystalReports.Engine;

namespace HROne.MPFFile
{
    public class MPFFirstContributionStatementProcess :GenericReportProcess
    {
        private DateTime PayPeriodFr, PayPeriodTo;
        private ArrayList EmpList;
        private int MPFPlanID;
        private string ChequeNo;

        public MPFFirstContributionStatementProcess(DatabaseConnection dbConn, ArrayList EmpList, int MPFPlanID, DateTime PayPeriodFr, DateTime PayPeriodTo)
            : base(dbConn)
        {
            this.EmpList = EmpList;
            this.PayPeriodFr = PayPeriodFr;
            this.PayPeriodTo = PayPeriodTo;
            this.MPFPlanID = MPFPlanID;
        }

        public override ReportDocument GenerateReport()
        {

            if (PayPeriodFr.Ticks != 0 && PayPeriodTo.Ticks != 0 && EmpList != null)
            {
                MPFFile.GenericMPFFile mpfFile = new MPFFile.GenericMPFFile(dbConn);

                mpfFile.LoadMPFFileDetail(EmpList, MPFPlanID, PayPeriodFr, PayPeriodTo);
                System.Data.DataSet dataSet = mpfFile.CreateFirstContributionStatementDataSet();

                if (reportDocument == null)
                {
                     reportDocument = new ReportTemplate.Report_Payroll_MPFFirstContributionStatement();
                }
                else
                {

                }

                reportDocument.SetDataSource(dataSet);
                reportDocument.SetParameterValue("ContributionPeriod", PayPeriodFr.ToString("yyyy-MM-dd") + " - " + PayPeriodTo.ToString("yyyy-MM-dd"));
                reportDocument.SetParameterValue("ContributionPeriodFrom", PayPeriodFr);
                reportDocument.SetParameterValue("ContributionPeriodTo", PayPeriodTo);
                reportDocument.SetParameterValue("TotalAdditionalEmployeeMC", mpfFile.TotalAdditionalEmployeeMC);
                reportDocument.SetParameterValue("TotalAdditionalEmployeeVC", mpfFile.TotalAdditionalEmployeeVC);
                reportDocument.SetParameterValue("TotalBackpayEmployeeMC", mpfFile.TotalBackPaymentEmployeeMC);
                reportDocument.SetParameterValue("TotalBackpayEmployeeVC", mpfFile.TotalBackPaymentEmployeeVC);
                reportDocument.SetParameterValue("TotalExistingEmployeeMC", mpfFile.TotalExistingEmployeeMC);
                reportDocument.SetParameterValue("TotalExistingEmployeeVC", mpfFile.TotalExistingEmployeeVC);
                reportDocument.SetParameterValue("TotalNewJoinEmployeeMC", mpfFile.TotalNewJoinEmployeeMC);
                reportDocument.SetParameterValue("TotalNewJoinEmployeeVC", mpfFile.TotalNewJoinEmployeeVC);

                return reportDocument;
            }
            else
                return null;

        }
    }
}
