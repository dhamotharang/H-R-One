using System;
using System.Collections;
using System.Text;
using HROne.Lib.Entities;
using HROne.Common;
using HROne.DataAccess;
using CrystalDecisions.CrystalReports.Engine;

namespace HROne.Reports.Payroll
{
    public class PayrollSummaryProcess : GenericReportProcess
    {

        private ArrayList EmpPayrollList;
        //private int PayPeriodID;
        private ReportType reportType;
        private ArrayList HLevelIDDisplayList = null;

        [Obsolete("PayPeriodID is no longer used")]
        public PayrollSummaryProcess(DatabaseConnection dbConn, ArrayList EmpPayrollList, ReportType reportType, int PayPeriodID)
            : base(dbConn)
        {
            this.EmpPayrollList = EmpPayrollList;
            this.reportType = reportType;
            //this.PayPeriodID = PayPeriodID;

        }

        public PayrollSummaryProcess(DatabaseConnection dbConn, ArrayList EmpPayrollList, ReportType reportType, ArrayList HLevelIDDisplayList)
            : base(dbConn)
        {

            this.EmpPayrollList = EmpPayrollList;
            this.reportType = reportType;
            this.HLevelIDDisplayList = HLevelIDDisplayList;
            //this.PayPeriodID = PayPeriodID;

        }

        public enum ReportType
        {
            TrialRun = 1,
            History = 2,
        }

        public override CrystalDecisions.CrystalReports.Engine.ReportDocument GenerateReport()
        {

            if (EmpPayrollList != null)
            {



                DataSet.PayrollTrialRunSummaryDataSet dataSet = new DataSet.PayrollTrialRunSummaryDataSet();
                DataSet.PayrollTrialRunSummaryDataSet.TrialRunSummaryDataTable dataTable = dataSet.TrialRunSummary;

                foreach (EEmpPayroll empPayroll in EmpPayrollList)
                {
                    EEmpPayroll.db.select(dbConn, empPayroll);

                    EPayrollPeriod payPeriod = new EPayrollPeriod();
                    payPeriod.PayPeriodID = empPayroll.PayPeriodID;
                    EPayrollPeriod.db.select(dbConn, payPeriod);

                    string strPayPeriod = payPeriod.PayPeriodFr.ToString("yyyy-MM-dd") + " - " + payPeriod.PayPeriodTo.ToString("yyyy-MM-dd");

                    EPayrollGroup payGroup = new EPayrollGroup();
                    payGroup.PayGroupID = payPeriod.PayGroupID;
                    EPayrollGroup.db.select(dbConn, payGroup);

                    EPayrollBatch payBatch = new EPayrollBatch();
                    payBatch.PayBatchID = empPayroll.PayBatchID;
                    EPayrollBatch.db.select(dbConn, payBatch);

                    EEmpPersonalInfo empInfo = new EEmpPersonalInfo();
                    empInfo.EmpID = empPayroll.EmpID;
                    EEmpPersonalInfo.db.select(dbConn, empInfo);


                    DataSet.PayrollTrialRunSummaryDataSet.TrialRunSummaryRow row = dataTable.NewTrialRunSummaryRow();
                    row.EmpPayrollID = empPayroll.EmpPayrollID;
                    row.EmpID = empPayroll.EmpID;
                    row.EmpNo = empInfo.EmpNo;
                    row.EmpName = empInfo.EmpEngFullName;
                    row.EmpAlias = empInfo.EmpAlias;
                    row.PayGroupDesc = payGroup.PayGroupCode + " - " + payGroup.PayGroupDesc;
                    row.PayGroupID = empPayroll.PayPeriodID;
                    row.PayrollPeriod = strPayPeriod;
                    if (!payBatch.PayBatchValueDate.Ticks.Equals(0))
                        row.PayBatchValueDate = payBatch.PayBatchValueDate;
                    row.PayBatchRemark = payBatch.PayBatchRemark;
                    row.BasicSalary = 0;
                    row.LeaveAllowance = 0;
                    row.PFundEE = 0;
                    row.PFundER = 0;
                    row.Overtime = 0;
                    row.Bonus = 0;
                    row.Commission = 0;
                    row.Others = 0;
                    row.NetIncome = 0;
                    row.TotalIncome = 0;
                    row.PayMethodAutoPay = 0;
                    row.PayMethodCash = 0;
                    row.PayMethodCheque = 0;
                    row.PayMethodOthers = 0;

                    AND recurringBasicSalaryAndTerms = new AND();
                    recurringBasicSalaryAndTerms.add(new Match("EmpRPEffFr", "<=", payPeriod.PayPeriodTo));
                    
                    DBFilter basicSalaryPaymentCodeFilter = new DBFilter();
                    basicSalaryPaymentCodeFilter.add(new Match("PaymentTypeID", EPaymentType.SystemPaymentType.BasicSalaryPaymentType(dbConn).PaymentTypeID));
                    recurringBasicSalaryAndTerms.add(new IN("PayCodeID", "Select PaymentCodeID FROM " + EPaymentCode.db.dbclass.tableName, basicSalaryPaymentCodeFilter));
                    EEmpRecurringPayment empRP = (EEmpRecurringPayment) AppUtils.GetLastObj(dbConn, EEmpRecurringPayment.db, "EmpRPEffFr", empPayroll.EmpID, recurringBasicSalaryAndTerms);
                    if (empRP != null)
                        row.RecurringBasicSalary = empRP.EmpRPAmount;

                    DBFilter paymentRecordFilter = new DBFilter();
                    paymentRecordFilter.add(new Match("EmpPayrollID", empPayroll.EmpPayrollID));
                    paymentRecordFilter.add(new Match("PayRecStatus", "A"));
                    ArrayList paymentRecords = EPaymentRecord.db.select(dbConn, paymentRecordFilter);
                    foreach (EPaymentRecord paymentRecord in paymentRecords)
                    {
                        EPaymentCode payCode = new EPaymentCode();
                        payCode.PaymentCodeID = paymentRecord.PaymentCodeID;
                        EPaymentCode.db.select(dbConn, payCode);

                        EPaymentType payType = new EPaymentType();
                        payType.PaymentTypeID = payCode.PaymentTypeID;
                        EPaymentType.db.select(dbConn, payType);

                        if (payType.PaymentTypeCode.Equals(EPaymentType.SystemPaymentType.BasicSalaryPaymentType(dbConn).PaymentTypeCode, StringComparison.CurrentCultureIgnoreCase))
                        {
                            row.BasicSalary += paymentRecord.PayRecActAmount;
                            row.TotalIncome += paymentRecord.PayRecActAmount;
                        }
                        else if (payType.PaymentTypeCode.Equals(EPaymentType.SystemPaymentType.LeaveDeductionPaymentType(dbConn).PaymentTypeCode, StringComparison.CurrentCultureIgnoreCase))
                        {
                            row.LeaveAllowance += paymentRecord.PayRecActAmount;
                            row.TotalIncome += paymentRecord.PayRecActAmount;
                        }
                        else if (payType.PaymentTypeCode.Equals(EPaymentType.SystemPaymentType.LeaveAllowancePaymentType(dbConn).PaymentTypeCode, StringComparison.CurrentCultureIgnoreCase))
                        {
                            row.LeaveAllowance += paymentRecord.PayRecActAmount;
                            row.TotalIncome += paymentRecord.PayRecActAmount;
                        }
                        else if (payType.PaymentTypeCode.Equals(EPaymentType.SystemPaymentType.MPFEmployeeMandatoryContributionPaymentType(dbConn).PaymentTypeCode, StringComparison.CurrentCultureIgnoreCase))
                            row.PFundEE += paymentRecord.PayRecActAmount;
                        else if (payType.PaymentTypeCode.Equals(EPaymentType.SystemPaymentType.MPFEmployeeVoluntaryContributionPaymentType(dbConn).PaymentTypeCode, StringComparison.CurrentCultureIgnoreCase))
                            row.PFundEE += paymentRecord.PayRecActAmount;
                        else if (payType.PaymentTypeCode.Equals(EPaymentType.SystemPaymentType.PFundEmployeeContributionPaymentType(dbConn).PaymentTypeCode, StringComparison.CurrentCultureIgnoreCase))
                            row.PFundEE += paymentRecord.PayRecActAmount;
                        else if (payType.PaymentTypeCode.Equals(EPaymentType.SystemPaymentType.MPFEmployerMandatoryContributionPaymentType(dbConn).PaymentTypeCode, StringComparison.CurrentCultureIgnoreCase))
                            row.PFundER += paymentRecord.PayRecActAmount;
                        else if (payType.PaymentTypeCode.Equals(EPaymentType.SystemPaymentType.MPFEmployerVoluntaryContributionPaymentType(dbConn).PaymentTypeCode, StringComparison.CurrentCultureIgnoreCase))
                            row.PFundER += paymentRecord.PayRecActAmount;
                        else if (payType.PaymentTypeCode.Equals(EPaymentType.SystemPaymentType.PFundEmployerContributionPaymentType(dbConn).PaymentTypeCode, StringComparison.CurrentCultureIgnoreCase))
                            row.PFundER += paymentRecord.PayRecActAmount;
                        else if (payType.PaymentTypeCode.Equals(EPaymentType.SystemPaymentType.OverTimePaymentType(dbConn).PaymentTypeCode, StringComparison.CurrentCultureIgnoreCase))
                        {
                            row.Overtime += paymentRecord.PayRecActAmount;
                            row.TotalIncome += paymentRecord.PayRecActAmount;
                        }
                        else if (payType.PaymentTypeCode.Equals(EPaymentType.SystemPaymentType.BonusPaymentType(dbConn).PaymentTypeCode, StringComparison.CurrentCultureIgnoreCase))
                        {
                            row.Bonus += paymentRecord.PayRecActAmount;
                            row.TotalIncome += paymentRecord.PayRecActAmount;
                        }
                        else if (payType.PaymentTypeCode.Equals(EPaymentType.SystemPaymentType.CommissionPaymentType(dbConn).PaymentTypeCode, StringComparison.CurrentCultureIgnoreCase))
                        {
                            row.Commission += paymentRecord.PayRecActAmount;
                            row.TotalIncome += paymentRecord.PayRecActAmount;
                        }
                        else
                        {
                            row.Others += paymentRecord.PayRecActAmount;
                            row.TotalIncome += paymentRecord.PayRecActAmount;
                        }
                        if (paymentRecord.PayRecMethod.Equals("A"))
                            row.PayMethodAutoPay += paymentRecord.PayRecActAmount;
                        else if (paymentRecord.PayRecMethod.Equals("C"))
                            row.PayMethodCash += paymentRecord.PayRecActAmount;
                        else if (paymentRecord.PayRecMethod.Equals("Q"))
                            row.PayMethodCheque += paymentRecord.PayRecActAmount;
                        else
                            row.PayMethodOthers += paymentRecord.PayRecActAmount;

                        row.NetIncome += paymentRecord.PayRecActAmount;


                    }
                    row.TotalWagesForMinWages = HROne.Payroll.PayrollProcess.GetTotalWagesWithoutRestDayPayment(dbConn, empInfo.EmpID, payPeriod.PayPeriodFr, payPeriod.PayPeriodTo, null);
                    row.TotalWorkingHours = HROne.Payroll.PayrollProcess.GetTotalEmpPayrollWorkingHours(dbConn, empInfo.EmpID, payPeriod.PayPeriodID);
                    row.MinWagesRequired = row.TotalWorkingHours * HROne.Payroll.PayrollProcess.GetMinimumWages(dbConn, empInfo.EmpID, payPeriod.PayPeriodTo);
                    DBFilter mpfRecordFilter = new DBFilter();
                    mpfRecordFilter.add(new Match("EmpPayrollID", empPayroll.EmpPayrollID));
                    ArrayList mpfRecords = EMPFRecord.db.select(dbConn, mpfRecordFilter);
                    foreach (EMPFRecord mpfRecord in mpfRecords)
                    {
                        row.PFundER += mpfRecord.MPFRecActVCER + mpfRecord.MPFRecActMCER;
                    }
                    ArrayList orsoRecords = EORSORecord.db.select(dbConn, mpfRecordFilter);
                    foreach (EORSORecord orsoRecord in orsoRecords)
                    {
                        row.PFundER += orsoRecord.ORSORecActER;
                    }

                    DBFilter positionFilter = new DBFilter();
                    positionFilter.add(new Match("EmpID", empInfo.EmpID));
                    positionFilter.add(new Match("PayGroupID", payGroup.PayGroupID));
                    positionFilter.add(new Match("EmpPosEffFr", "<=", payPeriod.PayPeriodTo));

                    OR orPosEffToTerms = new OR();
                    orPosEffToTerms.add(new Match("EmpPosEffTo", ">=", payPeriod.PayPeriodFr));
                    orPosEffToTerms.add(new NullTerm("EmpPosEffTo"));
                    positionFilter.add(orPosEffToTerms);

                    ArrayList positionInfos = EEmpPositionInfo.db.select(dbConn, positionFilter);
                    EEmpPositionInfo empPos = null;
                    if (positionInfos.Count > 0)
                        empPos = (EEmpPositionInfo)positionInfos[0];
                    else
                        empPos = AppUtils.GetLastPositionInfo(dbConn, payPeriod.PayPeriodTo, empInfo.EmpID);

                    if (HLevelIDDisplayList != null && empPos != null)
                        for (int count = 0; count < HLevelIDDisplayList.Count && count < 2; count++)
                        {
                            int hLevelID = Convert.ToInt32(HLevelIDDisplayList[count]);

                            EHierarchyLevel hLevel = new EHierarchyLevel();
                            hLevel.HLevelID = hLevelID;
                            if (EHierarchyLevel.db.select(dbConn, hLevel))
                            {
                                row["HLevelDesc" + (count + 1)] = hLevel.HLevelDesc;

                                DBFilter empHierarchyFilter = new DBFilter();
                                empHierarchyFilter.add(new Match("EmpPosID", empPos.EmpPosID));
                                empHierarchyFilter.add(new Match("HLevelID", hLevelID));
                                ArrayList empHierarchyList = EEmpHierarchy.db.select(dbConn, empHierarchyFilter);
                                if (empHierarchyList.Count > 0)
                                {
                                    EEmpHierarchy empHierarchy = (EEmpHierarchy)empHierarchyList[0];

                                    EHierarchyElement hElement = new EHierarchyElement();
                                    hElement.HElementID = empHierarchy.HElementID;
                                    if (EHierarchyElement.db.select(dbConn, hElement))
                                    {
                                        row["HElementCode" + (count + 1)] = hElement.HElementCode;
                                        row["HElementDesc" + (count + 1)] = hElement.HElementDesc;
                                    }
                                }
                            }
                        }

                    dataTable.Rows.Add(row);


                }
                if (reportDocument == null)
                {
                    if (reportType.Equals(ReportType.TrialRun))
                        reportDocument = new ReportTemplate.Report_Payroll_TrialRunSummary();
                    else if (reportType.Equals(ReportType.History))
                        reportDocument = new ReportTemplate.Report_Payroll_HistorySummary();
                }
                else
                {

                }

                reportDocument.SetDataSource(dataSet);

                return reportDocument;
            }
            else
                return null;
        }
    }
}
