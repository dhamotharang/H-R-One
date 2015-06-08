using System;
using System.Collections;
using System.Text;
using HROne.Lib.Entities;
using HROne.Common;
using CrystalDecisions.CrystalReports.Engine;
using HROne.DataAccess;
using System.Data;

namespace HROne.Reports.Payroll
{
    public class NewJoinPaymentSummaryProcess : HROne.Common.GenericReportProcess
    {
        protected IList EmpList;
        protected IList PayPeriodList;
        protected IList payBatchList;

        protected DataSet.Payroll_NewJoinPaymentSummary dataSet;
        string PreparedBy;
        string ReviewedBy;
        protected ReportType reportType;

        public enum ReportType
        {
            Unspecify,
            TrialRun,
            History
        }

        public NewJoinPaymentSummaryProcess(DatabaseConnection dbConn, IList EmpList, string PreparedBy, string ReviewedBy)
            : this(dbConn, EmpList, null, null, ReportType.Unspecify, PreparedBy, ReviewedBy)
        {
        }
        public NewJoinPaymentSummaryProcess(DatabaseConnection dbConn, IList EmpList, IList PayPeriodList, IList payBatchList, ReportType reportType, string PreparedBy, string ReviewedBy)
            :base(dbConn)
        {
            dataSet = new DataSet.Payroll_NewJoinPaymentSummary();

            this.EmpList = EmpList;
            this.PayPeriodList = PayPeriodList;
            this.payBatchList = payBatchList;
            this.reportType = reportType;
            this.PreparedBy = PreparedBy;
            this.ReviewedBy = ReviewedBy;
        }

        public override ReportDocument GenerateReport()
        {
            if (EmpList != null)
            {
                GenerateDetailByEmpID();
                if (reportDocument == null)
                {
                    reportDocument = new ReportTemplate.Report_Payroll_NewJoinPaymentSummary();
                }

                reportDocument.SetDataSource(dataSet);

                foreach (ReportDocument o in reportDocument.Subreports)
                {
                    o.SetDataSource(dataSet);
                }

                reportDocument.SetParameterValue("PreparedBy", PreparedBy);
                reportDocument.SetParameterValue("ReviewedBy", ReviewedBy);

                return reportDocument;
            }
            else
                return null;

        }

        public void GenerateDetailByEmpID()
        {
            DataTable empInfoTable = dataSet.Tables["EmpInfo"];
            DataTable dataTable = dataSet.Tables["PaySlip"];

            foreach (EEmpPersonalInfo empInfo in EmpList)
            {
                string empDiv = string.Empty;
                string empDep = string.Empty;

                EEmpPersonalInfo.db.select(dbConn, empInfo);

                EEmpPositionInfo empPos = AppUtils.GetLastPositionInfo(dbConn, empInfo.EmpDateOfJoin, empInfo.EmpID);
                if (empPos == null)
                    empPos = new EEmpPositionInfo();
                EPosition position = new EPosition();
                ECompany company = new ECompany();
                position.PositionID = empPos.PositionID;
                EPosition.db.select(dbConn, position);
                company.CompanyID = empPos.CompanyID;
                ECompany.db.select(dbConn, company);

                DBFilter emphierarchyFilter = new DBFilter();
                emphierarchyFilter.add(new Match("EmpPosID", empPos.EmpPosID));
                ArrayList empHierarchyList = EEmpHierarchy.db.select(dbConn, emphierarchyFilter);
                foreach (EEmpHierarchy empHierarchy in empHierarchyList)
                {
                    EHierarchyElement hElement = new EHierarchyElement();
                    hElement.HElementID = empHierarchy.HElementID;
                    if (EHierarchyElement.db.select(dbConn, hElement))
                    {
                        EHierarchyLevel hLevel = new EHierarchyLevel();
                        hLevel.HLevelID = hElement.HLevelID;
                        if (EHierarchyLevel.db.select(dbConn, hLevel))
                        {
                            if (hLevel.HLevelDesc.Equals("Division", StringComparison.CurrentCultureIgnoreCase) || hLevel.HLevelCode.Equals("DIV", StringComparison.CurrentCultureIgnoreCase))
                                empDiv = hElement.HElementDesc;
                        }
                    }
                }

                DBFilter basicSalaryRPFilter = new DBFilter();
                basicSalaryRPFilter.add(new Match("EmpID", empInfo.EmpID));
                basicSalaryRPFilter.add(new Match("EmpRPEffFr", "<=", empInfo.EmpDateOfJoin));

                OR orbasicSalaryRPToTerm = new OR();
                orbasicSalaryRPToTerm.add(new Match("EmpRPEffTo", ">=", empInfo.EmpDateOfJoin));
                orbasicSalaryRPToTerm.add(new NullTerm("EmpRPEffTo"));
                basicSalaryRPFilter.add(orbasicSalaryRPToTerm);

                DBFilter basicSalaryPaymentCodeFilter = new DBFilter();
                basicSalaryPaymentCodeFilter.add(new Match("PaymentTypeID", EPaymentType.SystemPaymentType.BasicSalaryPaymentType(dbConn).PaymentTypeID));

                basicSalaryRPFilter.add(new IN("PayCodeID", "Select PaymentCodeID from PaymentCode", basicSalaryPaymentCodeFilter));

                ArrayList recurringPayments = EEmpRecurringPayment.db.select(dbConn, basicSalaryRPFilter);


                EEmpRecurringPayment basicSalaryRP = new EEmpRecurringPayment();
                if (recurringPayments.Count > 0)
                    basicSalaryRP = (EEmpRecurringPayment)recurringPayments[0];


                DBFilter payPeriodFilter = new DBFilter();
                payPeriodFilter.add(new Match("PayPeriodFr", "<=", empInfo.EmpDateOfJoin));
                payPeriodFilter.add(new Match("PayPeriodTo", ">=", empInfo.EmpDateOfJoin));

                DBFilter empPayrollFilter = new DBFilter();
                empPayrollFilter.add(new Match("EmpID", empInfo.EmpID));
                empPayrollFilter.add(new IN("PayPeriodID", "Select PayPeriodID from PayrollPeriod", payPeriodFilter));

                string PayBatchInTerm = string.Empty;
                if (payBatchList != null)
                {
                    foreach (EPayrollBatch payrollBatch in payBatchList)
                    {
                        if (string.IsNullOrEmpty(PayBatchInTerm))
                            PayBatchInTerm = payrollBatch.PayBatchID.ToString();
                        else
                            PayBatchInTerm += "," + payrollBatch.PayBatchID.ToString();
                    }
                }
                if (!string.IsNullOrEmpty(PayBatchInTerm))
                    empPayrollFilter.add(new IN("PayBatchID", PayBatchInTerm, null));

                if (PayPeriodList != null)
                {
                    string payPeriodIDList = string.Empty;
                    foreach (EPayrollPeriod payPeriod in PayPeriodList)
                    {
                        if (string.IsNullOrEmpty(payPeriodIDList))
                            payPeriodIDList = payPeriod.PayPeriodID.ToString();
                        else
                            payPeriodIDList += "," + payPeriod.PayPeriodID.ToString();
                    }
                    if (!string.IsNullOrEmpty(payPeriodIDList))
                        empPayrollFilter.add(new IN("PayPeriodID", payPeriodIDList, null));
                }

                //  if ReportType.Unspecify, no filter is add (for backward compatibility with previous UI)
                if (reportType.Equals(ReportType.TrialRun))
                    empPayrollFilter.add(new Match("EmpPayStatus", "=", "T"));
                else if (reportType.Equals(ReportType.History))
                    empPayrollFilter.add(new Match("EmpPayStatus", "<>", "T"));

                IN inEmpPayroll = new IN("EmpPayrollID", "Select EmpPayrollID from EmpPayroll", empPayrollFilter);

                DBFilter paymentRecordFilter = new DBFilter();
                paymentRecordFilter.add(inEmpPayroll);
                paymentRecordFilter.add(new Match("PayRecStatus", "A"));
                paymentRecordFilter.add("PaymentCodeID", true);
                paymentRecordFilter.add("PayRecMethod", true);

                ArrayList paymentRecords = EPaymentRecord.db.select(dbConn, paymentRecordFilter);

                EPayrollBatch payBatch = null;

                DBFilter payBatchFilter = new DBFilter();
                payBatchFilter.add(new IN("PayBatchID", "Select PayBatchID from EmpPayroll", empPayrollFilter));
                if (!string.IsNullOrEmpty(PayBatchInTerm))
                    payBatchFilter.add(new IN("PayBatchID", PayBatchInTerm, null));
                payBatchFilter.add("PayBatchValueDate", false);

                ArrayList payBatchs = EPayrollBatch.db.select(dbConn, payBatchFilter);
                if (payBatchs.Count > 0)
                    payBatch = (EPayrollBatch)payBatchs[0];

                DataRow empInfoRow = empInfoTable.NewRow();
                empInfoRow["EmpID"] = empInfo.EmpID;
                empInfoRow["EmpNo"] = empInfo.EmpNo;
                empInfoRow["EmpName"] = empInfo.EmpEngFullName;
                empInfoRow["EmpChineseName"] = empInfo.EmpChiFullName;
                empInfoRow["EmpHKID"] = empInfo.EmpHKID;
                empInfoRow["EmpDateOfJoin"] = empInfo.EmpDateOfJoin;

                empInfoRow["CompanyName"] = company.CompanyName;
                empInfoRow["PositionDesc"] = position.PositionDesc;
                empInfoRow["BasicSalary"] = basicSalaryRP.EmpRPAmount;
                if (payBatch != null)
                    if (payBatch.PayBatchValueDate.Ticks != 0)
                        empInfoRow["ValueDate"] = payBatch.PayBatchValueDate;
                if (empDep.Equals(string.Empty) && empDiv.Equals(string.Empty))
                    empInfoRow["Department"] = empDiv;
                else if (empDiv.Equals(string.Empty))
                    empInfoRow["Department"] = empDep;
                else if (empDep.Equals(string.Empty))
                    empInfoRow["Department"] = empDiv;
                else
                    empInfoRow["Department"] = empDiv + " / " + empDep;

                empInfoTable.Rows.Add(empInfoRow);

                GeneratePayrollData(empInfo.EmpID, paymentRecords);
            }
            DataRow[] allowanceRows = dataSet.Tables["PaySlip"].Select("PayAmount > 0", "PaymentCodeDisplaySeqNo");
            foreach (DataRow importRow in allowanceRows)
                dataSet.Tables["PaySlip_Allowance"].ImportRow(importRow);

            DataRow[] deductionRows = dataSet.Tables["PaySlip"].Select("PayAmount < 0", "PaymentCodeDisplaySeqNo");
            foreach (DataRow importRow in deductionRows)
                dataSet.Tables["PaySlip_Deduction"].ImportRow(importRow);

        }

        private void GeneratePayrollData(int EmpID, ArrayList paymentRecords)
        {
            DataRow row = null;
            foreach (EPaymentRecord paymentRecord in paymentRecords)
            {
                string paymentMethod = string.Empty;

                EPaymentCode paymentCode = new EPaymentCode();
                paymentCode.PaymentCodeID = paymentRecord.PaymentCodeID;
                EPaymentCode.db.select(dbConn, paymentCode);
                //  Hide payment if payment code is chosen for hidden under payslip
                if (!paymentCode.PaymentCodeHideInPaySlip)
                {
                    if (row == null)
                        row = dataSet.Tables["PaySlip"].NewRow();
                    else if (!((int)row["EmpID"] == EmpID && (string)row["PayMethod"] == paymentMethod && (string)row["PaymentCode"] == paymentCode.PaymentCode))
                    {
                        dataSet.Tables["PaySlip"].Rows.Add(row);
                        row = dataSet.Tables["PaySlip"].NewRow();
                    }
                    if (row.IsNull("EmpID"))
                    {
                        row["EmpID"] = EmpID;
                        row["PaymentTypeID"] = paymentCode.PaymentTypeID;
                        row["PaymentCode"] = paymentCode.PaymentCode;
                        row["PaymentCodeDesc"] = paymentCode.PaymentCodeDesc;
                        row["PaymentCodeDisplaySeqNo"] = paymentCode.PaymentCodeDisplaySeqNo;
                        row["PaymentCodeIsMPF"] = paymentCode.PaymentCodeIsMPF;
                        row["PayMethod"] = paymentMethod;
                        row["PayAmount"] = 0;
                    }
                    row["PayAmount"] = (double)row["PayAmount"] + paymentRecord.PayRecActAmount;
                    if (!string.IsNullOrEmpty(paymentRecord.PayRecRemark))
                    {
                        string[] paymentRemarkArray = paymentRecord.PayRecRemark.Split(new string[] { "\r\n", "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries);
                        if (paymentRemarkArray.Length > 0)
                        {
                            string firstPaymentRemark = paymentRemarkArray[0];
                            if (row.IsNull("Remark"))
                                row["Remark"] = firstPaymentRemark;
                            else
                                row["Remark"] += " + " + firstPaymentRemark;
                        }
                    }
                }
            }
            if (row != null)
                dataSet.Tables["PaySlip"].Rows.Add(row);
        }

    }
}
