using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Text;
using HROne.DataAccess;
using HROne.Lib.Entities;
using HROne.Lib;
using HROne.Common;

namespace HROne.Reports.Payroll
{
    public class PaymentSummaryListProcess : GenericExcelReportProcess
    {
        public enum ReportType
        {
            TrialRun = 1,
            History = 2
        }

        ArrayList EmpList;
        ReportType reportType;
        ArrayList PayPeriodList;
        ArrayList PayBatchList;
        string exportFileName;
        public PaymentSummaryListProcess(DatabaseConnection dbConn, System.Globalization.CultureInfo reportCultureInfo, ArrayList EmpList, ReportType reportType, ArrayList PayPeriodList, ArrayList PayBatchList)
            : base(dbConn, reportCultureInfo)
        {
            this.EmpList = EmpList;
            this.reportType = reportType;
            this.PayPeriodList = PayPeriodList;
            this.PayBatchList = PayBatchList;
        }

        protected override System.Data.DataSet CreateDataSource()
        {
            const string PAYMENTCODE_PREFIX = "[Payment] ";

            System.Data.DataSet dataSet = new System.Data.DataSet(); //export.GetDataSet();
            DataTable dataTable = new DataTable("Payroll$");
            dataSet.Tables.Add(dataTable);
            dataTable.Columns.Add("Company", typeof(string));

            DBFilter hierarchyLevelFilter = new DBFilter();
            Hashtable hierarchyLevelHashTable = new Hashtable();
            hierarchyLevelFilter.add("HLevelSeqNo", true);
            ArrayList hierarchyLevelList = EHierarchyLevel.db.select(dbConn, hierarchyLevelFilter);
            foreach (EHierarchyLevel hlevel in hierarchyLevelList)
            {
                dataTable.Columns.Add(hlevel.HLevelDesc, typeof(string));
                hierarchyLevelHashTable.Add(hlevel.HLevelID, hlevel);
            }
            dataTable.Columns.Add("Payroll Group", typeof(string));
            dataTable.Columns.Add("Position", typeof(string));
            dataTable.Columns.Add("EmpNo", typeof(string));
            dataTable.Columns.Add("English Name", typeof(string));
            dataTable.Columns.Add("Chinese Name", typeof(string));
            dataTable.Columns.Add("HKID", typeof(string));
            dataTable.Columns.Add("From", typeof(DateTime));
            dataTable.Columns.Add("To", typeof(DateTime));
            int firstSummaryColumnPos = dataTable.Columns.Count;
            dataTable.Columns.Add("Net Payment", typeof(double));
            dataTable.Columns.Add("Relevant Income", typeof(double));
            dataTable.Columns.Add("Non-Relevant Income", typeof(double));
            dataTable.Columns.Add("Wages Payable for Min Wages", typeof(double));
            dataTable.Columns.Add("Total Hours Worked", typeof(double));
            dataTable.Columns.Add("Min Wages Required", typeof(double));
            dataTable.Columns.Add("Employer Mandatory Contribution", typeof(double));
            //            dataTable.Columns.Add("Employee Mandatory Contribution", typeof(double));
            dataTable.Columns.Add("Employer Voluntary Contribution", typeof(double));
            //            dataTable.Columns.Add("Employee Voluntary Contribution", typeof(double));
            dataTable.Columns.Add("Employer P-Fund Contribution", typeof(double));
            //            dataTable.Columns.Add("Employee P-Fund Contribution", typeof(double));
            dataTable.Columns.Add("Total Employer Contribution", typeof(double));
            dataTable.Columns.Add("Total Employee Contribution", typeof(double));
            dataTable.Columns.Add("Total Taxable Payment", typeof(double));
            dataTable.Columns.Add("Total Non-Taxable Payment", typeof(double));
            int firstDetailColumnPos = dataTable.Columns.Count;


            foreach (EEmpPersonalInfo empInfo in EmpList)
            {
                EEmpPersonalInfo.db.select(dbConn, empInfo);



                //DBFilter empPayrollFilterForPayrollPeriod = new DBFilter();
                //empPayrollFilterForPayrollPeriod.add(new Match("ep.EmpID", empInfo.EmpID));
                //empPayrollFilterForPayrollPeriod.add(Payroll_PeriodSelectionList1.GetEmpPayrollDBTerm());

                //DBFilter payPeriodFilter = new DBFilter();
                //payPeriodFilter.add(new IN("PayPeriodID", "SELECT PayPeriodID from " + EEmpPayroll.db.dbclass.tableName + " ep", empPayrollFilterForPayrollPeriod));
                //ArrayList payPeriodList = EPayrollPeriod.db.select(dbConn, payPeriodFilter);
                if (PayPeriodList == null)
                {
                    OR orPayBatchIDTerms = new OR();
                    foreach (EPayrollBatch payBatch in PayBatchList)
                        orPayBatchIDTerms.add(new Match("ep.PayBatchID", payBatch.PayBatchID));

                    DBFilter empPayrollFilter = new DBFilter();
                    empPayrollFilter.add(orPayBatchIDTerms);

                    DBFilter payPeriodFilter = new DBFilter();
                    payPeriodFilter.add(new IN(EPayrollPeriod.db.dbclass.tableName + ".PayPeriodID ", "SELECT DISTINCT PayPeriodID FROM " + EEmpPayroll.db.dbclass.tableName + " ep", empPayrollFilter));

                    PayPeriodList = EPayrollPeriod.db.select(dbConn, payPeriodFilter);
                }

                foreach (EPayrollPeriod payPeriod in PayPeriodList)
                {
                    DBFilter empPayrollFilter = new DBFilter();
                    empPayrollFilter.add(new Match("EmpID", empInfo.EmpID));

                    if (reportType.Equals(ReportType.TrialRun))
                        empPayrollFilter.add(new Match("EmpPayStatus", "=", "T"));
                    else
                        empPayrollFilter.add(new Match("EmpPayStatus", "<>", "T"));

                    if (PayBatchList != null)
                    {
                        OR orPayrollBatch = new OR();
                        foreach (EPayrollBatch payrollBatch in PayBatchList)
                            orPayrollBatch.add(new Match("PayBatchID", payrollBatch.PayBatchID));
                        empPayrollFilter.add(orPayrollBatch);
                    }
                    empPayrollFilter.add(new Match("PayPeriodID", payPeriod.PayPeriodID));

                    //  Check if the EmpPayroll record for that payroll period exists
                    if (EEmpPayroll.db.count(dbConn, empPayrollFilter) > 0 && EPayrollPeriod.db.select(dbConn, payPeriod))
                    {

                        EPayrollGroup payrollGroup = new EPayrollGroup();
                        payrollGroup.PayGroupID = payPeriod.PayGroupID;
                        EPayrollGroup.db.select(dbConn, payrollGroup);

                        DataRow row = dataTable.NewRow();
                        row["EmpNo"] = empInfo.EmpNo;
                        row["English Name"] = empInfo.EmpEngFullName;
                        row["Chinese Name"] = empInfo.EmpChiFullName;
                        row["HKID"] = empInfo.EmpHKID;
                        row["From"] = payPeriod.PayPeriodFr;
                        row["To"] = payPeriod.PayPeriodTo;
                        row["Payroll Group"] = payrollGroup.PayGroupDesc;
                        DBFilter empPosFilter = new DBFilter();

                        EEmpPositionInfo empPos = AppUtils.GetLastPositionInfo(dbConn, payPeriod.PayPeriodTo, empInfo.EmpID);
                        if (empPos != null)
                        {
                            ECompany company = new ECompany();
                            company.CompanyID = empPos.CompanyID;
                            if (ECompany.db.select(dbConn, company))
                                row["Company"] = company.CompanyCode;

                            DBFilter empHierarchyFilter = new DBFilter();
                            empHierarchyFilter.add(new Match("EmpPosID", empPos.EmpPosID));
                            ArrayList empHierarchyList = EEmpHierarchy.db.select(dbConn, empHierarchyFilter);
                            foreach (EEmpHierarchy empHierarchy in empHierarchyList)
                            {
                                EHierarchyLevel hierarchyLevel = (EHierarchyLevel)hierarchyLevelHashTable[empHierarchy.HLevelID];
                                if (hierarchyLevel != null)
                                {
                                    EHierarchyElement hierarchyElement = new EHierarchyElement();
                                    hierarchyElement.HElementID = empHierarchy.HElementID;
                                    if (EHierarchyElement.db.select(dbConn, hierarchyElement))
                                        row[hierarchyLevel.HLevelDesc] = hierarchyElement.HElementDesc;
                                }
                            }
                            EPosition position = new EPosition();
                            position.PositionID = empPos.PositionID;
                            if (EPosition.db.select(dbConn, position))
                                row["Position"] = position.PositionDesc;


                        }

                        double netAmount = 0, releventIncome = 0, nonRelevantIncome = 0, taxableAmount = 0, nonTaxableAmount = 0;
                        double mcER = 0, mcEE = 0;
                        double vcER = 0, vcEE = 0;
                        double pFundER = 0, pFundEE = 0;

                        //DBFilter empPayrollFilterForPaymentRecord = new DBFilter(empPayrollFilterForPayrollPeriod);
                        //empPayrollFilterForPaymentRecord.add(new Match("PayPeriodID", payPeriod.PayPeriodID));
                        //DBFilter paymentRecordFilter = new DBFilter();
                        //paymentRecordFilter.add(new IN("EmpPayrollID", "Select EmpPayrollID from " + EEmpPayroll.db.dbclass.tableName + " ep ", empPayrollFilterForPaymentRecord));
                        //paymentRecordFilter.add(new Match("PayRecStatus", "A"));


                        IN inEmpPayroll = new IN("EmpPayrollID", "Select ep.EmpPayrollID from " + EEmpPayroll.db.dbclass.tableName + " ep", empPayrollFilter);

                        DBFilter empPayrollFilterForPaymentRecord = new DBFilter();
                        empPayrollFilterForPaymentRecord.add(inEmpPayroll);
                        ArrayList paymentRecords = EPaymentRecord.db.select(dbConn, empPayrollFilterForPaymentRecord);

                        foreach (EPaymentRecord paymentRecord in paymentRecords)
                        {
                            EPaymentCode payCode = new EPaymentCode();
                            payCode.PaymentCodeID = paymentRecord.PaymentCodeID;
                            EPaymentCode.db.select(dbConn, payCode);
                            //  Always Use Payment Code Description for grouping payment code with same description
                            string fieldName = PAYMENTCODE_PREFIX + payCode.PaymentCodeDesc;
                            if (dataTable.Columns[fieldName] == null)
                                dataTable.Columns.Add(new DataColumn(fieldName, typeof(double)));
                            if (row[fieldName] == null || row[fieldName] == DBNull.Value)
                                row[fieldName] = 0;
                            row[fieldName] = (double)row[fieldName] + paymentRecord.PayRecActAmount;


                            netAmount += paymentRecord.PayRecActAmount;
                            if (payCode.PaymentCodeIsMPF)
                                releventIncome += paymentRecord.PayRecActAmount;
                            else
                                nonRelevantIncome += paymentRecord.PayRecActAmount;

                            DBFilter taxPaymentMapFilter = new DBFilter();
                            taxPaymentMapFilter.add(new Match("PaymentCodeID", paymentRecord.PaymentCodeID));
                            if (ETaxPaymentMap.db.count(dbConn, taxPaymentMapFilter) > 0)
                                taxableAmount += paymentRecord.PayRecActAmount;
                            else
                                nonTaxableAmount += paymentRecord.PayRecActAmount;

                        }

                        row["Net Payment"] = HROne.CommonLib.GenericRoundingFunctions.RoundingTo(netAmount, ExchangeCurrency.DefaultCurrencyDecimalPlaces(), ExchangeCurrency.DefaultCurrencyDecimalPlaces());
                        row["Relevant Income"] = HROne.CommonLib.GenericRoundingFunctions.RoundingTo(releventIncome, ExchangeCurrency.DefaultCurrencyDecimalPlaces(), ExchangeCurrency.DefaultCurrencyDecimalPlaces());
                        row["Non-Relevant Income"] = HROne.CommonLib.GenericRoundingFunctions.RoundingTo(nonRelevantIncome, ExchangeCurrency.DefaultCurrencyDecimalPlaces(), ExchangeCurrency.DefaultCurrencyDecimalPlaces());
                        row["Wages Payable for Min Wages"] = HROne.Payroll.PayrollProcess.GetTotalWagesWithoutRestDayPayment(dbConn, empInfo.EmpID, payPeriod.PayPeriodFr, payPeriod.PayPeriodTo, null);
                        row["Total Hours Worked"] = HROne.Payroll.PayrollProcess.GetTotalEmpPayrollWorkingHours(dbConn, empInfo.EmpID, payPeriod.PayPeriodID);
                        row["Min Wages Required"] = (double)row["Total Hours Worked"] * HROne.Payroll.PayrollProcess.GetMinimumWages(dbConn, empInfo.EmpID, payPeriod.PayPeriodTo);
                        row["Total Taxable Payment"] = HROne.CommonLib.GenericRoundingFunctions.RoundingTo(taxableAmount, ExchangeCurrency.DefaultCurrencyDecimalPlaces(), ExchangeCurrency.DefaultCurrencyDecimalPlaces());
                        row["Total Non-Taxable Payment"] = HROne.CommonLib.GenericRoundingFunctions.RoundingTo(nonTaxableAmount, ExchangeCurrency.DefaultCurrencyDecimalPlaces(), ExchangeCurrency.DefaultCurrencyDecimalPlaces());

                        ArrayList mpfRecords = EMPFRecord.db.select(dbConn, empPayrollFilterForPaymentRecord);
                        foreach (EMPFRecord mpfRecord in mpfRecords)
                        {
                            vcER += mpfRecord.MPFRecActVCER;
                            mcER += +mpfRecord.MPFRecActMCER;
                            vcEE += mpfRecord.MPFRecActVCEE;
                            mcEE += mpfRecord.MPFRecActMCEE;
                        }
                        ArrayList orsoRecords = EORSORecord.db.select(dbConn, empPayrollFilterForPaymentRecord);
                        foreach (EORSORecord orsoRecord in orsoRecords)
                        {
                            pFundER += orsoRecord.ORSORecActER;
                            pFundEE += orsoRecord.ORSORecActEE;
                        }
                        row["Employer Mandatory Contribution"] = mcER;
                        //                        row["Employee Mandatory Contribution"] = mcEE;
                        row["Employer Voluntary Contribution"] = vcER;
                        //                        row["Employee Voluntary Contribution"] = vcEE;
                        row["Employer P-Fund Contribution"] = pFundER;
                        //                        row["Employee P-Fund Contribution"] = pFundEE;

                        row["Total Employer Contribution"] = mcER + vcER + pFundER;
                        row["Total Employee Contribution"] = mcEE + vcEE + pFundEE;

                        dataTable.Rows.Add(row);
                    }
                }
            }

            DBFilter paymentCodeFilter = new DBFilter();
            paymentCodeFilter.add("PaymentCodeDisplaySeqNo", false);
            paymentCodeFilter.add("PaymentCode", false);
            ArrayList paymentCodeList = EPaymentCode.db.select(dbConn, paymentCodeFilter);
            foreach (EPaymentCode paymentCode in paymentCodeList)
            {
                if (dataTable.Columns.Contains(PAYMENTCODE_PREFIX + paymentCode.PaymentCodeDesc))
                {
                    DataColumn paymentColumn = dataTable.Columns[PAYMENTCODE_PREFIX + paymentCode.PaymentCodeDesc];
                    paymentColumn.SetOrdinal(firstDetailColumnPos);
                    if (!dataTable.Columns.Contains(paymentCode.PaymentCodeDesc))
                        paymentColumn.ColumnName = paymentCode.PaymentCodeDesc;
                    else
                    {
                        System.Diagnostics.Debug.Write("System reserved payment column is used");
                    }
                }
            }
            for (int i = 0; i < firstDetailColumnPos; i++)
                dataTable.Columns[i].ColumnName = HROne.Common.WebUtility.GetLocalizedString(dataTable.Columns[i].ColumnName);

            for (int i = firstSummaryColumnPos; i < firstDetailColumnPos; i++)
                dataTable.Columns[firstSummaryColumnPos].SetOrdinal(dataTable.Columns.Count - 1);


            return dataSet;
        }
        protected override void CreateWorkBookStyle(NPOI.HSSF.UserModel.HSSFWorkbook workBook)
        {
        }
        protected override System.IO.FileInfo SaveToFile(NPOI.HSSF.UserModel.HSSFWorkbook workBook)
        {

            return new System.IO.FileInfo(exportFileName);
        }
        protected override void GenerateWorkbookDetail(NPOI.HSSF.UserModel.HSSFWorkbook workBook, System.Data.DataSet dataSet)
        {
            exportFileName = System.IO.Path.GetTempFileName();
            System.IO.File.Delete(exportFileName);
            exportFileName += ".xls";
            HROne.Export.ExcelExport export = new HROne.Export.ExcelExport(exportFileName);
            export.Update(dataSet);

        }
    }
}
