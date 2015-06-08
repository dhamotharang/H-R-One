using System;
using System.Collections;
using System.Text;
using HROne.Lib.Entities;
using HROne.Common;
using CrystalDecisions.CrystalReports.Engine;
using HROne.DataAccess;

namespace HROne.MPFFile
{
    public class HSBCMPFGatewayFileEncryptedCoverProcess : GenericReportProcess
    {
        DataSet.HSBCCoverDataSet dataSet = null;

        public HSBCMPFGatewayFileEncryptedCoverProcess(DatabaseConnection dbConn, HSBCMPFGatewayFileEncrypted resultBankFileObject)
            : base(dbConn)
        {
            EMPFPlan mpfPlan = new EMPFPlan();
            mpfPlan.MPFPlanID = resultBankFileObject.MPFPlanID;
            if (EMPFPlan.db.select(dbConn, mpfPlan))
            {

                //System.Xml.XmlDocument extendPropertiesDocument = HROne.CommonLib.Utility.GetXmlDocumentByDataString(resultBankFileObject.MPFPlanExtendXMLString);

                dataSet = new HROne.MPFFile.DataSet.HSBCCoverDataSet();
                DataSet.HSBCCoverDataSet.mpfcoverRow row = dataSet.mpfcover.NewmpfcoverRow();
                row.ContributionPeriod = resultBankFileObject.PayPeriodFr.ToString("dd/MM/yyyy") + " - " + resultBankFileObject.PayPeriodTo.ToString("dd/MM/yyyy");
                row.EmployerAddress = mpfPlan.MPFPlanCompanyAddress;
                row.ER_ID = resultBankFileObject.EmployerID;
                row.ER_NAME = mpfPlan.MPFPlanCompanyName;
                row.FILE_NAME = resultBankFileObject.ActualMPFFileName();
                row.PayCenterCode = resultBankFileObject.PayCenterCode;
                row.BankKeyID = resultBankFileObject.KeyID;
                row.ContactName = mpfPlan.MPFPlanContactName;
                row.ContactPhoneNumber = mpfPlan.MPFPlanContactNo;
                row.ProductVersion = resultBankFileObject.ProductAndVersion;
                row.RemoteProfileID = resultBankFileObject.RemoteProfileID;
                dataSet.mpfcover.Rows.Add(row);
            }
        }


        public override ReportDocument GenerateReport()
        {

            if (reportDocument == null)
            {
                if (reportCultureInfo.Name.Equals("zh-cht", StringComparison.CurrentCultureIgnoreCase))
                    reportDocument = new ReportTemplate.Report_Payroll_MPF_HSBCCover_Cht();
                else
                    reportDocument = new ReportTemplate.Report_Payroll_MPF_HSBCCover();

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
            //reportDocument.SetParameterValue("PayPeriodFr", PayPeriodFr);
        }
    }
}
