using System;
using System.Collections;
using System.Text;
using HROne.Lib.Entities;
using HROne.DataAccess;
using HROne.Common;
using CrystalDecisions.CrystalReports.Engine;
namespace HROne.Reports.Taxation
{
    public class TaxationPaymentMappingProcess : GenericReportProcess
    {

        public TaxationPaymentMappingProcess(DatabaseConnection dbConn)
            : base(dbConn)
        {
        }

        public override ReportDocument GenerateReport()
        {

            DataSet.TaxationPaymentMapping dataSet = new DataSet.TaxationPaymentMapping();

            ArrayList paymentCodeList = EPaymentCode.db.select(dbConn, new DBFilter());

            foreach (EPaymentCode paymentCode in paymentCodeList)
            {
                DataSet.TaxationPaymentMapping.PaymentCodeRow row = dataSet.PaymentCode.NewPaymentCodeRow();
                row.PaymentCode = paymentCode.PaymentCode;
                row.PaymentCodeDesc = paymentCode.PaymentCodeDesc;
                row.PaymentCodeID = paymentCode.PaymentCodeID;
                row.PaymentTypeID = paymentCode.PaymentTypeID;
                dataSet.PaymentCode.Rows.Add(row);
            }

            ArrayList taxPaymentList = ETaxPayment.db.select(dbConn, new DBFilter());
            foreach (ETaxPayment taxPayment in taxPaymentList)
            {
                DataSet.TaxationPaymentMapping.TaxPaymentRow row = dataSet.TaxPayment.NewTaxPaymentRow();
                row.TaxFormType = taxPayment.TaxFormType;
                row.TaxPayCode = taxPayment.TaxPayCode;
                row.TaxPayDesc = taxPayment.TaxPayDesc;
                row.TaxPayID = taxPayment.TaxPayID;
                row.TaxPayIsShowNature = taxPayment.TaxPayIsShowNature;
                row.TaxPayNature = taxPayment.TaxPayNature;
                dataSet.TaxPayment.Rows.Add(row);
            }

            ArrayList taxPaymentMapList = ETaxPaymentMap.db.select(dbConn, new DBFilter());
            foreach (ETaxPaymentMap taxPaymentMap in taxPaymentMapList)
            {
                DataSet.TaxationPaymentMapping.TaxPaymentMapRow row = dataSet.TaxPaymentMap.NewTaxPaymentMapRow();
                row.PaymentCodeID = taxPaymentMap.PaymentCodeID;
                row.TaxPayID = taxPaymentMap.TaxPayID;
                row.TaxPayMapID = taxPaymentMap.TaxPayMapID;
                dataSet.TaxPaymentMap.Rows.Add(row);
            }

            if (reportDocument == null)
            {
                reportDocument = new ReportTemplate.Report_Taxation_PaymentMapping();

            }
            else
            {

            }

            reportDocument.SetDataSource(dataSet);
            return reportDocument;
        }
    }
}
