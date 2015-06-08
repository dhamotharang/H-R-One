using System;
using System.Collections.Generic;
using System.Text;
using HROne.Common;
using CrystalDecisions.CrystalReports.Engine;
using HROne.Taxation;
using HROne.DataAccess;
namespace HROne.Reports.Taxation
{
    public class ControlListProcess:GenericReportProcess 
    {
        private int TaxFormID;
        private string NameOfSignature;

        public ControlListProcess(DatabaseConnection dbConn, int TaxFormID, string NameOfSignature)
            : base(dbConn)
        {
            this.TaxFormID = TaxFormID;
            this.NameOfSignature = NameOfSignature;
            if (string.IsNullOrEmpty(this.NameOfSignature))
                this.NameOfSignature = " ";
        }

        public override ReportDocument GenerateReport()
        {
            if (TaxFormID > 0)
            {

                if (reportDocument == null)
                {
                    reportDocument = new ReportTemplate.Report_Taxation_ControlList();
                }
                else
                {

                }

                System.Data.DataSet dataSet = TaxationGeneration.GenerateTaxationDataSet(dbConn, TaxFormID, null);

                reportDocument.SetDataSource(dataSet);
                reportDocument.SetParameterValue("NameOfSignature", NameOfSignature);
                return reportDocument;
            }
            else
                return null;
        }
    }
}
