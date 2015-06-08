using System;
using System.Collections;
using System.Text;
using HROne.Common;
using HROne.Lib.Entities;
using CrystalDecisions.CrystalReports.Engine;
using HROne.DataAccess;
namespace HROne.Reports.Taxation
{
    public class TaxationFormProcess:GenericReportProcess 
    {
        private ArrayList TaxEmpList;
        private int TaxFormID;
        private string TaxFormType;
        private string NameOfSignature;
        private ReportLanguage reportLanguage;

        public enum ReportLanguage
        {
            English = 1,
            TraditionalChinese = 2
        }

        //public TaxationFormProcess(ArrayList TaxEmpList, int TaxFormID, string TaxFormType, string NameOfSignature)
        //    :this(TaxEmpList,TaxFormID,TaxFormType,NameOfSignature,ReportLanguage.English)
        //{
        //}

        public TaxationFormProcess(DatabaseConnection dbConn, ArrayList TaxEmpList, int TaxFormID, string TaxFormType, string NameOfSignature, ReportLanguage reportLanguage)
            : base(dbConn)
        {
            this.TaxEmpList = TaxEmpList;
            this.TaxFormID = TaxFormID;
            this.TaxFormType = TaxFormType;
            this.NameOfSignature = NameOfSignature;
            this.reportLanguage = reportLanguage;
        }

        public override ReportDocument GenerateReport()
        {
            if (TaxFormID > 0 && TaxEmpList != null && !string.IsNullOrEmpty(TaxFormType))
            {




                HROne.Taxation.DataSet.Taxation_IR56B_DataSet dataSet = HROne.Taxation.TaxationGeneration.GenerateTaxationDataSet(dbConn, TaxFormID, TaxEmpList);

                if (reportDocument == null)
                {
                    if (reportLanguage == ReportLanguage.TraditionalChinese)
                    {
                        if (TaxFormType.Equals("B", StringComparison.CurrentCultureIgnoreCase))
                            reportDocument = new ReportTemplate.Report_Taxation_IR56B_CHT();
                        else if (TaxFormType.Equals("E", StringComparison.CurrentCultureIgnoreCase))
                            reportDocument = new ReportTemplate.Report_Taxation_IR56E_CHT();
                        else if (TaxFormType.Equals("F", StringComparison.CurrentCultureIgnoreCase))
                            reportDocument = new ReportTemplate.Report_Taxation_IR56F_CHT();
                        else if (TaxFormType.Equals("G", StringComparison.CurrentCultureIgnoreCase))
                            reportDocument = new ReportTemplate.Report_Taxation_IR56G_CHT();
                        // Start 0000020, KuangWei, 2014-07-16
                        else if (TaxFormType.Equals("M", StringComparison.CurrentCultureIgnoreCase))
                            reportDocument = new ReportTemplate.Report_Taxation_IR56M_CHT();
                        // End 0000020, KuangWei, 2014-07-16
                    }
                    else
                    {
                        if (TaxFormType.Equals("B", StringComparison.CurrentCultureIgnoreCase))
                            reportDocument = new ReportTemplate.Report_Taxation_IR56B();
                        else if (TaxFormType.Equals("E", StringComparison.CurrentCultureIgnoreCase))
                            reportDocument = new ReportTemplate.Report_Taxation_IR56E();
                        else if (TaxFormType.Equals("F", StringComparison.CurrentCultureIgnoreCase))
                            reportDocument = new ReportTemplate.Report_Taxation_IR56F();
                        else if (TaxFormType.Equals("G", StringComparison.CurrentCultureIgnoreCase))
                            reportDocument = new ReportTemplate.Report_Taxation_IR56G();
                        // Start 0000020, KuangWei, 2014-07-16
                        else if (TaxFormType.Equals("M", StringComparison.CurrentCultureIgnoreCase))
                            reportDocument = new ReportTemplate.Report_Taxation_IR56M();
                        // End 0000020, KuangWei, 2014-07-16
                    }
                }
                else
                {

                }

                reportDocument.SetDataSource(dataSet);
                if (reportDocument.ParameterFields["NameOfSignature"] != null)
                    reportDocument.SetParameterValue("NameOfSignature", NameOfSignature);
                return reportDocument;
            }
            else
                return null;
        }
    }
}
