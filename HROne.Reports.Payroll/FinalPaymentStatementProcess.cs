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
    public class FinalPaymentStatementProcess : HROne.Common.GenericReportProcess
    {
        protected IList EmpList;
        protected IList PayPeriodList;
        protected IList payBatchList;
        //private int PayPeriodID;
        protected bool IsShowLeaveBalance;

        protected DataSet.Payroll_FinalPaymentStatement dataSet;
        string PreparedBy;
        string ReviewedBy;
        protected ReportType reportType;

        public enum ReportType
        {
            Unspecify,
            TrialRun,
            History
        }

        public FinalPaymentStatementProcess(DatabaseConnection dbConn, IList EmpList, string PreparedBy, string ReviewedBy)
            : this(dbConn, EmpList, null, null, ReportType.Unspecify, PreparedBy, ReviewedBy)
        {
        }
        public FinalPaymentStatementProcess(DatabaseConnection dbConn, IList EmpList, IList PayPeriodList, IList payBatchList, ReportType reportType, string PreparedBy, string ReviewedBy)
            :base(dbConn)
        {
            dataSet = new DataSet.Payroll_FinalPaymentStatement();
            //dataSet.ReadXmlSchema(SchemeFile);

            this.EmpList = EmpList;
            this.PayPeriodList = PayPeriodList;
            this.payBatchList = payBatchList;
            this.reportType = reportType;
            this.PreparedBy = PreparedBy;
            this.ReviewedBy = ReviewedBy;
            //if (this.reportType.Equals(ReportType.PaySlip))
            //    IsShowLeaveBalance = true & !ESystemParameter.getParameter(ESystemParameter.PARAM_CODE_PAY_SLIP_HIDE_LEAVE_BALANCE).Equals("Y");
            //else
            IsShowLeaveBalance = false;
        }


        public override ReportDocument GenerateReport()
        {

            if (EmpList != null)
            {
                GenerateDetailByEmpID();
                if (reportDocument == null)
                {
                    reportDocument = new ReportTemplate.Report_Payroll_FinalPaymentStatement();
                }
                else
                {

                }

                reportDocument.SetDataSource(dataSet);

                foreach (ReportDocument o in reportDocument.Subreports)
                {
                    o.SetDataSource(dataSet);
                }

                reportDocument.SetParameterValue("PreparedBy", PreparedBy);
                reportDocument.SetParameterValue("ReviewedBy", ReviewedBy);

                
                //if (IsShowLeaveBalance)
                //    reportDocument.Subreports["Report_Payroll_PaySlip_SubLeaveBalance"].SetDataSource(dataSet);
                return reportDocument;
            }
            else
                return null;

        }

        public void GenerateDetailByEmpID()
        {
            //dataSet = new DataSet.Payroll_PaySlip();

            //EPayrollPeriod payPeriod = new EPayrollPeriod();
            //payPeriod.PayPeriodID = PayPeriodID;
            //EPayrollPeriod.db.select(dbConn, payPeriod);

            //EPayrollGroup payGroup = new EPayrollGroup();
            //payGroup.PayGroupID = payPeriod.PayGroupID;
            //EPayrollGroup.db.select(dbConn, payGroup);

            //string strPayPeriod = payPeriod.PayPeriodFr.ToString("yyyy-MM-dd") + " - " + payPeriod.PayPeriodTo.ToString("yyyy-MM-dd");


            DataTable empInfoTable = dataSet.Tables["EmpInfo"];
            DataTable dataTable = dataSet.Tables["PaySlip"];
            DataTable mpfTable = dataSet.Tables["PaySlip_MPF"];
            DataTable orsoTable = dataSet.Tables["PaySlip_ORSO"];

            foreach (EEmpTermination empTermination in EmpList)
            {
                string empDiv = string.Empty;
                string empDep = string.Empty;

                EEmpTermination.db.select(dbConn, empTermination);

                ECessationReason cessationReason = new ECessationReason();
                cessationReason.CessationReasonID = empTermination.CessationReasonID;
                ECessationReason.db.select(dbConn, cessationReason);

                //DBFilter bankAccountFilter = new DBFilter();
                //bankAccountFilter.add(new Match("EmpID", empInfo.EmpID));
                //bankAccountFilter.add(new Match("EmpAccDefault", 1));
                //ArrayList bankAccounts = EEmpBankAccount.db.select(dbConn, bankAccountFilter);
                //EEmpBankAccount bankAccount;
                //if (bankAccounts.Count > 0)
                //    bankAccount = (EEmpBankAccount)bankAccounts[0];
                //else
                //    bankAccount = new EEmpBankAccount();

                EEmpPersonalInfo empInfo = new EEmpPersonalInfo();
                empInfo.EmpID = empTermination.EmpID;
                if (EEmpPersonalInfo.db.select(dbConn, empInfo))
                {

                    //DBFilter positionFilter = new DBFilter();
                    //positionFilter.add(new Match("EmpID", empInfo.EmpID));
                    ////positionFilter.add(new Match("PayGroupID", payGroup.PayGroupID));
                    //positionFilter.add(new Match("EmpPosEffFr", "<=", empTermination.EmpTermLastDate));

                    //OR orPosEffToTerms = new OR();
                    //orPosEffToTerms.add(new Match("EmpPosEffTo", ">=", empTermination.EmpTermLastDate));
                    //orPosEffToTerms.add(new NullTerm("EmpPosEffTo"));
                    //positionFilter.add(orPosEffToTerms);

                    //ArrayList positionInfos = EEmpPositionInfo.db.select(dbConn, positionFilter);
                    EEmpPositionInfo empPos = AppUtils.GetLastPositionInfo(dbConn, empTermination.EmpTermLastDate, empInfo.EmpID);// new EPosition();
                    if (empPos == null)
                        empPos = new EEmpPositionInfo();
                    EPosition position = new EPosition();
                    ECompany company = new ECompany();
                    ////if (positionInfos.Count > 0)
                    ////{
                    //    EEmpPositionInfo empPos = (EEmpPositionInfo)positionInfos[0];
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
                                //if (hLevel.HLevelDesc.Equals("Department", StringComparison.CurrentCultureIgnoreCase) || hLevel.HLevelCode.Equals("DEP", StringComparison.CurrentCultureIgnoreCase))
                                //    empDep = hElement.HElementDesc;
                            }
                        }
                    }

                    //}

                    DBFilter basicSalaryRPFilter = new DBFilter();
                    basicSalaryRPFilter.add(new Match("EmpID", empInfo.EmpID));
                    basicSalaryRPFilter.add(new Match("EmpRPEffFr", "<=", empTermination.EmpTermLastDate));

                    OR orbasicSalaryRPToTerm = new OR();
                    orbasicSalaryRPToTerm.add(new Match("EmpRPEffTo", ">=", empTermination.EmpTermLastDate));
                    orbasicSalaryRPToTerm.add(new NullTerm("EmpRPEffTo"));
                    basicSalaryRPFilter.add(orbasicSalaryRPToTerm);

                    //filter.add(EmpRPEffFr,true);

                    DBFilter basicSalaryPaymentCodeFilter = new DBFilter();
                    basicSalaryPaymentCodeFilter.add(new Match("PaymentTypeID", EPaymentType.SystemPaymentType.BasicSalaryPaymentType(dbConn).PaymentTypeID));

                    basicSalaryRPFilter.add(new IN("PayCodeID", "Select PaymentCodeID from PaymentCode", basicSalaryPaymentCodeFilter));

                    ArrayList recurringPayments = EEmpRecurringPayment.db.select(dbConn, basicSalaryRPFilter);


                    EEmpRecurringPayment basicSalaryRP = new EEmpRecurringPayment();
                    if (recurringPayments.Count > 0)
                        basicSalaryRP = (EEmpRecurringPayment)recurringPayments[0];



                    DBFilter payPeriodFilter = new DBFilter();
                    payPeriodFilter.add(new Match("PayPeriodTo", ">=", empTermination.EmpTermLastDate));

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

                    DBFilter mpfRecordFilter = new DBFilter();
                    mpfRecordFilter.add(inEmpPayroll);
                    mpfRecordFilter.add("MPFRecPeriodFr", true);
                    ArrayList mpfRecords = EMPFRecord.db.select(dbConn, mpfRecordFilter);

                    DBFilter orsoRecordFilter = new DBFilter();
                    orsoRecordFilter.add(inEmpPayroll);
                    orsoRecordFilter.add("ORSORecPeriodFr", true);
                    ArrayList orsoRecords = EORSORecord.db.select(dbConn, orsoRecordFilter);

                    EPayrollBatch payBatch = null;

                    DBFilter payBatchFilter = new DBFilter();
                    payBatchFilter.add(new IN("PayBatchID", "Select PayBatchID from EmpPayroll", empPayrollFilter));
                    if (!string.IsNullOrEmpty(PayBatchInTerm))
                        payBatchFilter.add(new IN("PayBatchID", PayBatchInTerm, null));
                    payBatchFilter.add("PayBatchValueDate", false);

                    ArrayList payBatchs = EPayrollBatch.db.select(dbConn, payBatchFilter);
                    if (payBatchs.Count > 0)
                        payBatch = (EPayrollBatch)payBatchs[0];

                    bool hasMPF = false, hasORSO = false, hasLeaveBalance = false;

                    if (IsShowLeaveBalance)
                        hasLeaveBalance = GenerateLeaveBalanceData(empInfo.EmpID, empTermination.EmpTermLastDate);

                    //double intALBalance = 0;
                    //HROne.LeaveCalc.LeaveBalanceCalc calc = new HROne.LeaveCalc.LeaveBalanceCalc(empInfo.EmpID, payPeriod.PayPeriodFr.AddDays(-1));
                    //ArrayList leaveBalanceList = calc.getCurrentBalanceList();
                    //foreach (ELeaveBalance b in leaveBalanceList)
                    //{
                    //    if (b.LeaveTypeID.Equals(ELeaveType.ANNUAL_LEAVE_TYPE(dbConn).LeaveTypeID))
                    //        intALBalance = b.Balance;
                    //}
                    hasMPF = GenerateMPFData(empInfo.EmpID, mpfRecords);
                    hasORSO = GenerateORSOData(empInfo.EmpID, orsoRecords);

                    DataRow empInfoRow = empInfoTable.NewRow();
                    empInfoRow["EmpID"] = empInfo.EmpID;
                    empInfoRow["EmpNo"] = empInfo.EmpNo;
                    empInfoRow["EmpName"] = empInfo.EmpEngFullName;
                    empInfoRow["EmpChineseName"] = empInfo.EmpChiFullName;
                    empInfoRow["EmpHKID"] = empInfo.EmpHKID;
                    empInfoRow["EmpDateOfJoin"] = empInfo.EmpDateOfJoin;

                    //empInfoRow["PayGroupDesc"] = payGroup.PayGroupCode + " - " + payGroup.PayGroupDesc;
                    //empInfoRow["PayrollPeriod"] = strPayPeriod;
                    //empInfoRow["PeriodFr"] = payPeriod.PayPeriodFr;
                    //empInfoRow["PeriodTo"] = payPeriod.PayPeriodTo;
                    //empInfoRow["MPFDate"] = new DateTime(payPeriod.PayPeriodTo.Year, payPeriod.PayPeriodTo.Month, 10).AddMonths(1);

                    //empInfoRow["BankAccount"] = bankAccount.EmpBankCode + " - " + bankAccount.EmpBranchCode + "-" + bankAccount.EmpAccountNo;
                    empInfoRow["CompanyName"] = company.CompanyName;
                    empInfoRow["PositionDesc"] = position.PositionDesc;
                    empInfoRow["BasicSalary"] = basicSalaryRP.EmpRPAmount;
                    empInfoRow["EmpTermResignDate"] = empTermination.EmpTermResignDate;
                    empInfoRow["EmpTermLastDate"] = empTermination.EmpTermLastDate;
                    empInfoRow["CessationReasonDesc"] = cessationReason.CessationReasonDesc;
                    empInfoRow["HasMPF"] = hasMPF;
                    empInfoRow["HasORSO"] = hasORSO;
                    empInfoRow["HasLeaveBalance"] = hasLeaveBalance;
                    int YearOfServiceMonth = Convert.ToInt32(Math.Truncate(HROne.CommonLib.Utility.MonthDifference(empInfo.EmpServiceDate, empTermination.EmpTermLastDate)));
                    empInfoRow["YearOfServiceYear"] = YearOfServiceMonth / 12;
                    empInfoRow["YearOfServiceMonth"] = YearOfServiceMonth % 12;
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

                    //empInfoRow["AnnualLeaveBalance"] = intALBalance;
                    //empInfoRow["LeaveBalanceAsOfDate"] = payPeriod.PayPeriodFr.AddDays(-1);
                    empInfoTable.Rows.Add(empInfoRow);

                    GeneratePayrollData(empInfo.EmpID, paymentRecords);
                }
            }
            DataRow[] allowanceRows = dataSet.Tables["PaySlip"].Select("PayAmount > 0", "PaymentCodeDisplaySeqNo");
            foreach (DataRow importRow in allowanceRows)
                dataSet.Tables["PaySlip_Allowance"].ImportRow(importRow);

            DataRow[] deductionRows = dataSet.Tables["PaySlip"].Select("PayAmount < 0", "PaymentCodeDisplaySeqNo");
            foreach (DataRow importRow in deductionRows)
                dataSet.Tables["PaySlip_Deduction"].ImportRow(importRow);

        }

        private bool GenerateLeaveBalanceData(int EmpID, DateTime AsOfDate)
        {
            bool hasLeaveBalance = false;
            HROne.LeaveCalc.LeaveBalanceCalc calc = new HROne.LeaveCalc.LeaveBalanceCalc(dbConn, EmpID, AsOfDate);
            ArrayList balanceItems = calc.getCurrentBalanceList();
            foreach (ELeaveBalance b in balanceItems)
            {
                hasLeaveBalance = true;
                DataRow rr = dataSet.Tables["LeaveBalance"].NewRow();
                rr["LeaveBalanceID"] = b.LeaveBalanceID;
                rr["EmpID"] = EmpID;
                rr["LeaveBalanceEffectiveDate"] = b.LeaveBalanceEffectiveDate;
                rr["LeaveBalanceBF"] = b.LeaveBalanceBF;
                rr["LeaveBalanceEntitled"] = b.LeaveBalanceEntitled;
                rr["LeaveBalanceForfeiture"] = b.LeaveBalanceForfeiture + b.ExpiryForfeit;
                rr["Taken"] = b.Taken;
                rr["Adjust"] = b.Adjust;
                rr["Reserved"] = b.Reserved;
                rr["Balance"] = b.getBalance();
                rr["Name"] = b.Name;
                rr["Description"] = b.Description;
                rr["StringFormat"] = b.StringFormat;


                dataSet.Tables["LeaveBalance"].Rows.Add(rr);

            }
            return hasLeaveBalance;

        }


        private bool GenerateMPFData(int EmpID, ArrayList mpfRecords)
        {
            bool hasMPF = false;
            DataRow mpfRow = null;
            foreach (EMPFRecord mpfRecord in mpfRecords)
            {
                hasMPF = true;

                if (mpfRow == null)
                    mpfRow = CreatePaySlipMPFRow(EmpID, mpfRecord.MPFRecPeriodFr, mpfRecord.MPFRecPeriodTo);
                if (!(mpfRecord.MPFRecPeriodFr.Equals((DateTime)mpfRow["PeriodFr"]) && mpfRecord.MPFRecPeriodTo.Equals((DateTime)mpfRow["PeriodTo"])))
                {
                    dataSet.Tables["PaySlip_MPF"].Rows.Add(mpfRow);
                    mpfRow = CreatePaySlipMPFRow(EmpID, mpfRecord.MPFRecPeriodFr, mpfRecord.MPFRecPeriodTo);
                }
                mpfRow["RelevantIncome"] = (double)mpfRow["RelevantIncome"] + mpfRecord.MPFRecActMCRI;
                mpfRow["MPFEE"] = (double)mpfRow["MPFEE"] + mpfRecord.MPFRecActMCEE;
                mpfRow["MPFER"] = (double)mpfRow["MPFER"] + mpfRecord.MPFRecActMCER;
                mpfRow["VCEE"] = (double)mpfRow["VCEE"] + mpfRecord.MPFRecActVCEE;
                mpfRow["VCER"] = (double)mpfRow["VCER"] + mpfRecord.MPFRecActVCER;
            }
            if (mpfRow != null)
                dataSet.Tables["PaySlip_MPF"].Rows.Add(mpfRow);
            return hasMPF;
        }

        private bool GenerateORSOData(int EmpID, ArrayList orsoRecords)
        {
            bool hasORSO = false;
            DataRow orsoRow = null;
            foreach (EORSORecord orsoRecord in orsoRecords)
            {
                hasORSO = true;

                if (orsoRow == null)
                    orsoRow = CreatePaySlipORSORow(EmpID, orsoRecord.ORSORecPeriodFr, orsoRecord.ORSORecPeriodTo);
                if (!(orsoRecord.ORSORecPeriodFr.Equals((DateTime)orsoRow["PeriodFr"]) && orsoRecord.ORSORecPeriodTo.Equals((DateTime)orsoRow["PeriodTo"])))
                {
                    dataSet.Tables["PaySlip_ORSO"].Rows.Add(orsoRow);
                    orsoRow = CreatePaySlipORSORow(EmpID, orsoRecord.ORSORecPeriodFr, orsoRecord.ORSORecPeriodTo);
                }
                orsoRow["RelevantIncome"] = (double)orsoRow["RelevantIncome"] + orsoRecord.ORSORecActRI;
                orsoRow["EE"] = (double)orsoRow["EE"] + orsoRecord.ORSORecActEE;
                orsoRow["ER"] = (double)orsoRow["ER"] + orsoRecord.ORSORecActER;
            }
            if (orsoRow != null)
                dataSet.Tables["PaySlip_ORSO"].Rows.Add(orsoRow);
            return hasORSO;
        }

        private void GeneratePayrollData(int EmpID, ArrayList paymentRecords)
        {
            DataRow row = null;
            foreach (EPaymentRecord paymentRecord in paymentRecords)
            {
                string paymentMethod = string.Empty;
                // no longer group by payment method because payment method does not display in report 
                //if (paymentRecord.PayRecMethod.Equals("A", StringComparison.CurrentCultureIgnoreCase))
                //    paymentMethod = "Autopay";
                //else if (paymentRecord.PayRecMethod.Equals("Q", StringComparison.CurrentCultureIgnoreCase))
                //    paymentMethod = "Cheque";
                //else if (paymentRecord.PayRecMethod.Equals("C", StringComparison.CurrentCultureIgnoreCase))
                //    paymentMethod = "Cash";
                //else if (paymentRecord.PayRecMethod.Equals("O", StringComparison.CurrentCultureIgnoreCase))
                //    paymentMethod = "Others";
                //else
                //    paymentMethod = paymentRecord.PayRecMethod;


                EPaymentCode paymentCode = new EPaymentCode();
                paymentCode.PaymentCodeID = paymentRecord.PaymentCodeID;
                EPaymentCode.db.select(dbConn, paymentCode);
                //  Hide payment if payment code is chosen for hidden under payslip
                if (!paymentCode.PaymentCodeHideInPaySlip)
                {
                    //                    string paymentCodeFullName = paymentCode.PaymentCode + " - " + paymentCode.PaymentCodeDesc;
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
            //foreach (DataRow dataRow in dataSet.Tables["PaySlip"].Rows)
            //{
            //    int paymentTypeID = (int)dataRow["PaymentTypeID"];
            //    if (paymentTypeID.Equals(EPaymentType.SystemPaymentType.MPFEmployeeMandatoryContributionPaymentType(dbConn).PaymentTypeID) || paymentTypeID.Equals(EPaymentType.SystemPaymentType.MPFEmployeeVoluntaryContributionPaymentType(dbConn).PaymentTypeID) || paymentTypeID.Equals(EPaymentType.SystemPaymentType.PFundEmployeeContributionPaymentType(dbConn).PaymentTypeID))
            //    {
            //        if (dataRow.IsNull("Remark"))
            //        {
            //            dataRow["remark"] = (-(double)((double)dataRow["PayAmount"] * 20)).ToString("$#,##0.00") + " x 5%";
            //        }
            //    }
            //}

        }

        private DataRow CreatePaySlipMPFRow(int EmpID, DateTime PeriodFr, DateTime PeriodTo)
        {
            DataRow mpfRow = dataSet.Tables["PaySlip_MPF"].NewRow();
            mpfRow["EmpID"] = EmpID;
            mpfRow["PeriodFr"] = PeriodFr;
            mpfRow["PeriodTo"] = PeriodTo;
            mpfRow["RelevantIncome"] = 0;
            mpfRow["MPFEE"] = 0;
            mpfRow["MPFER"] = 0;
            mpfRow["VCEE"] = 0;
            mpfRow["VCER"] = 0;
            return mpfRow;
        }

        private DataRow CreatePaySlipORSORow(int EmpID, DateTime PeriodFr, DateTime PeriodTo)
        {
            DataRow orsoRow = dataSet.Tables["PaySlip_ORSO"].NewRow();
            orsoRow["EmpID"] = EmpID;
            orsoRow["PeriodFr"] = PeriodFr;
            orsoRow["PeriodTo"] = PeriodTo;
            orsoRow["RelevantIncome"] = 0;
            orsoRow["EE"] = 0;
            orsoRow["ER"] = 0;
            return orsoRow;
        }
    }
}
