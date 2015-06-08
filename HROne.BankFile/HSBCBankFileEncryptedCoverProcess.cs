using System;
using System.Collections;
using System.Text;
using HROne.Lib.Entities;
using HROne.Common;
using CrystalDecisions.CrystalReports.Engine;
using HROne.DataAccess;

namespace HROne.BankFile
{
    public class HSBCBankFileEncryptedCoverProcess:GenericReportProcess 
    {
        DataSet.HSBCCoverDataSet dataSet = null;

        public HSBCBankFileEncryptedCoverProcess(DatabaseConnection dbConn, HSBCBankFileEncrypted resultBankFileObject )
            : base(dbConn)
        {
            dataSet = new HROne.BankFile.DataSet.HSBCCoverDataSet();
            DataSet.HSBCCoverDataSet.payadvRow row = dataSet.payadv.NewpayadvRow();
            row.AMOUNT = resultBankFileObject.TotalAmount;
            row.C_ACCT = resultBankFileObject.BranchCode + resultBankFileObject.AccountNo;
            row.C_NAME = resultBankFileObject.AccountHolderName;
            row.COUNT = resultBankFileObject.RecordCount;
            row.DEB_CRED = "D"; //  Debit or Credit?
            row.FILE_NAME = resultBankFileObject.ActualBankFileName();
            row.HASH_TOTAL = resultBankFileObject.HashTotal;
            row.PAY_CODE = resultBankFileObject.BankPaymentCode;
            row.PAY_COL = "P";  //  Payment or Collection?
            row.PLAN_CODE = resultBankFileObject.PlanCode;
            row.REFERENCE = resultBankFileObject.FirstPartyReference;
            row.VAL_DATE = resultBankFileObject.ValueDate;
            row.BankKeyID = resultBankFileObject.KeyID;
            row.ContactName = resultBankFileObject.ContactPerson;
            row.ContactPhoneNumber = resultBankFileObject.ContactPersonPhoneNumber;
            row.ProductVersion = resultBankFileObject.ProductAndVersion;
            row.RemoteProfileID = resultBankFileObject.RemoteProfileID;
            dataSet.payadv.Rows.Add(row);
        }


        public override ReportDocument GenerateReport()
        {

            if (reportDocument == null)
            {
                if (reportCultureInfo.Name.Equals("zh-cht", StringComparison.CurrentCultureIgnoreCase))
                    reportDocument = new ReportTemplate.HSBCBankFileEncryptedCover_Cht();
                else
                    reportDocument = new ReportTemplate.HSBCBankFileEncryptedCover();

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
