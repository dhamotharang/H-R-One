using System;
using System.Collections;
using System.Text;
using HROne.Lib.Entities;
using HROne.Common;
using CrystalDecisions.CrystalReports.Engine;
using HROne.DataAccess;
using HROne.LeaveCalc;
namespace HROne.Reports.Payroll
{
    public class PayrollDetailProcess : GenericReportProcess
    {
        private IList EmpList;
        private IList PayBatchList;
        private IList PayPeriodList;
        private bool IsShowLeaveBalance;
        private bool IsShowMinimumWage;
        private ReportType reportType;
        private IList HLevelIDDisplayList = null;
        private DataSet.Payroll_PaySlip dataSet;

        public CrystalDecisions.Shared.PageMargins PaperMargin = new CrystalDecisions.Shared.PageMargins(-1, -1, -1, -1);

        public enum ReportType
        {
            TrialRun = 1,
            History = 2,
            PaySlip = 3,
            PaySlipDotMatrix = 4
        }

        /**
         * For backward compatible with old method 
         **/
        public PayrollDetailProcess(DatabaseConnection dbConn, IList EmpList, ReportType reportType, int PayPeriodID, IList PayBatchList)
            : this(dbConn, EmpList, reportType, PayPeriodID, PayBatchList, null)
        {
        }
        public PayrollDetailProcess(DatabaseConnection dbConn, IList EmpList, ReportType reportType, int PayPeriodID, IList PayBatchList, IList HLevelIDDisplayList)
            : base(dbConn)
        {
            EPayrollPeriod payPeriod = new EPayrollPeriod();
            payPeriod.PayPeriodID = PayPeriodID;
            ArrayList payPeriodList = new ArrayList();
            payPeriodList.Add(payPeriod);
            Init(EmpList, reportType, payPeriodList, PayBatchList, HLevelIDDisplayList);
        }

        protected void Init(IList EmpList, ReportType reportType, IList PayPeriodList, IList PayBatchList, IList HLevelIDDisplayList)
        {
            this.EmpList = EmpList;
            this.reportType = reportType;
            this.PayPeriodList = PayPeriodList;
            this.PayBatchList = PayBatchList;
            this.HLevelIDDisplayList = HLevelIDDisplayList;
            if (this.reportType.Equals(ReportType.PaySlip) || this.reportType.Equals(ReportType.PaySlipDotMatrix))
            {
                IsShowLeaveBalance = true & ESystemParameter.getParameter(dbConn, ESystemParameter.PARAM_CODE_PAY_SLIP_HIDE_LEAVE_BALANCE).Equals("N");
                IsShowMinimumWage = true & !ESystemParameter.getParameter(dbConn, ESystemParameter.PARAM_CODE_PAY_SLIP_HIDE_MINIMUM_WAGE_INFO).Equals("Y");
            }
            else
            {
                IsShowLeaveBalance = false;
                IsShowMinimumWage = false;
            }
        }

        public PayrollDetailProcess(DatabaseConnection dbConn, ArrayList EmpList, ReportType reportType, ArrayList PayPeriodList, ArrayList PayBatchList, ArrayList HLevelIDDisplayList)
            : base(dbConn)
        {
            Init(EmpList, reportType, PayPeriodList, PayBatchList, HLevelIDDisplayList);
        }

        public override ReportDocument GenerateReport()
        {

            if (EmpList!=null )
            {
                GenerateDetailByEmpID();
                if (reportDocument == null)
                {
                    if (reportType.Equals(ReportType.PaySlip))
                        reportDocument = new ReportTemplate.Report_Payroll_PaySlip();
                    else if (reportType.Equals(ReportType.PaySlipDotMatrix))
                        reportDocument = new ReportTemplate.Report_Payroll_PaySlipDotMatrix();
                    else if (reportType.Equals(ReportType.TrialRun))
                        reportDocument = new ReportTemplate.Report_Payroll_TrialRunDetail();
                    else if (reportType.Equals(ReportType.History))
                        reportDocument = new ReportTemplate.Report_Payroll_HistoryDetail();

                }
                else
                {

                }
                if (PaperMargin.topMargin>=0)
                    reportDocument.PrintOptions.ApplyPageMargins(PaperMargin);
                reportDocument.SetDataSource(dataSet);

                //reportDocument.Subreports["Report_Payroll_PaySlip_SubMPF"].SetDataSource(dataSet);
                //reportDocument.Subreports["Report_Payroll_PaySlip_SubORSO"].SetDataSource(dataSet);
                //if (IsShowLeaveBalance)
                //    reportDocument.Subreports["Report_Payroll_PaySlip_SubLeaveBalance"].SetDataSource(dataSet);
                return reportDocument;
            }
            else
                return null;

        }

        public void GenerateDetailByEmpID()
        {
            dataSet = new DataSet.Payroll_PaySlip();

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
            foreach (EPayrollPeriod payPeriod in PayPeriodList )
                if (EPayrollPeriod.db.select(dbConn, payPeriod))
                {
                    EPayrollGroup payGroup = new EPayrollGroup();
                    payGroup.PayGroupID = payPeriod.PayGroupID;
                    EPayrollGroup.db.select(dbConn, payGroup);

                    string strPayPeriod = payPeriod.PayPeriodFr.ToString("yyyy-MM-dd") + " - " + payPeriod.PayPeriodTo.ToString("yyyy-MM-dd");


                    DataSet.Payroll_PaySlip.EmpInfoDataTable empInfoTable = dataSet.EmpInfo;
                    DataSet.Payroll_PaySlip.PaySlipDataTable dataTable = dataSet.PaySlip;
                    DataSet.Payroll_PaySlip.PaySlip_MPFDataTable mpfTable = dataSet.PaySlip_MPF;
                    DataSet.Payroll_PaySlip.PaySlip_ORSODataTable orsoTable = dataSet.PaySlip_ORSO;
                    foreach (EEmpPersonalInfo empInfo in EmpList)
                    {

                        EEmpPersonalInfo.db.select(dbConn, empInfo);

                        if ((reportType.Equals(ReportType.PaySlip) || reportType.Equals(ReportType.PaySlipDotMatrix)) && empInfo.MasterEmpID > 0 && empInfo.EmpIsCombinePaySlip)
                            continue;

                        DBFilter bankAccountFilter = new DBFilter();
                        if ((reportType.Equals(ReportType.PaySlip) || reportType.Equals(ReportType.PaySlipDotMatrix)))
                            bankAccountFilter.add(empInfo.GetAllRoleEmpIDTerms(dbConn, "EmpID", EEmpPersonalInfo.RoleFilterOptionEnum.Payslip));
                        else
                            bankAccountFilter.add(new Match("EmpID", empInfo.EmpID));
                        bankAccountFilter.add(new Match("EmpAccDefault", 1));
                        ArrayList bankAccounts = EEmpBankAccount.db.select(dbConn, bankAccountFilter);
                        EEmpBankAccount bankAccount;
                        if (bankAccounts.Count > 0)
                            bankAccount = (EEmpBankAccount)bankAccounts[0];
                        else
                            bankAccount = new EEmpBankAccount();


                        //  Get Last Position using selected payroll group;
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

                        EPosition position = new EPosition();
                        ECompany company = new ECompany();
                        if (empPos != null)
                        {
                            position.PositionID = empPos.PositionID;
                            EPosition.db.select(dbConn, position);
                            company.CompanyID = empPos.CompanyID;
                            ECompany.db.select(dbConn, company);

                        }

                        DBFilter empPayrollFilter = new DBFilter();
                        if ((reportType.Equals(ReportType.PaySlip) || reportType.Equals(ReportType.PaySlipDotMatrix)))
                            empPayrollFilter.add(empInfo.GetAllRoleEmpIDTerms(dbConn, "EmpID", EEmpPersonalInfo.RoleFilterOptionEnum.Payslip));
                        else 
                            empPayrollFilter.add(new Match("EmpID", empInfo.EmpID));

                        if (reportType.Equals(ReportType.TrialRun))
                            empPayrollFilter.add(new Match("EmpPayStatus", "=", "T"));
                        else
                            empPayrollFilter.add(new Match("EmpPayStatus", "<>", "T"));

                        if (PayBatchList != null)
                        {
                            bool skipPayBatch = false;
                            if ((reportType.Equals(ReportType.PaySlip) || reportType.Equals(ReportType.PaySlipDotMatrix)))
                                if (empInfo.GetOtherRoleList(dbConn, EEmpPersonalInfo.RoleFilterOptionEnum.Payslip).Count > 0)
                                    skipPayBatch = true;

                            if (!skipPayBatch)
                            {
                                OR orPayrollBatch = new OR();
                                foreach (EPayrollBatch payrollBatch in PayBatchList)
                                    orPayrollBatch.add(new Match("PayBatchID", payrollBatch.PayBatchID));
                                empPayrollFilter.add(orPayrollBatch);
                            }
                        }
                        empPayrollFilter.add(new Match("PayPeriodID", payPeriod.PayPeriodID));

                        IN inEmpPayroll = new IN("EmpPayrollID", "Select EmpPayrollID from EmpPayroll", empPayrollFilter);

                        ArrayList empPayrollList = EEmpPayroll.db.select(dbConn, empPayrollFilter);

                        if (empPayrollList.Count > 0)
                        {

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

                            ////  need double confirm
                            //DBFilter payBatchFilter = new DBFilter();
                            //payBatchFilter.add(new IN("PayBatchID", "Select PayBatchID from EmpPayroll", empPayrollFilter));
                            //payBatchFilter.add("PayBatchValueDate", false);

                            //ArrayList payBatchs = EPayrollBatch.db.select(dbConn, payBatchFilter);
                            //EPayrollBatch payBatch = null;
                            //if (payBatchs.Count > 0)
                            //    payBatch = (EPayrollBatch)payBatchs[0];

                            DataSet.Payroll_PaySlip.EmpInfoRow empInfoRow = empInfoTable.NewEmpInfoRow();
                            empInfoRow.ActualEmpID = empInfo.EmpID;
                            empInfoRow.EmpNo = empInfo.EmpNo;
                            empInfoRow.EmpName = empInfo.EmpEngFullName;
                            empInfoRow.EmpAlias = empInfo.EmpAlias;
                            empInfoRow.EmpEngFullNameWithAlias = empInfo.EmpEngFullNameWithAlias;
                            empInfoRow.EmpChineseName = empInfo.EmpChiFullName;
                            empInfoRow.EmpDateOfJoin = empInfo.EmpDateOfJoin;
                            empInfoRow.PayGroupDesc = payGroup.PayGroupCode + " - " + payGroup.PayGroupDesc;
                            empInfoRow.PayrollPeriod = strPayPeriod;
                            empInfoRow.PeriodFr = payPeriod.PayPeriodFr;
                            empInfoRow.PeriodTo = payPeriod.PayPeriodTo;
                            empInfoRow.MPFDate = new DateTime(payPeriod.PayPeriodTo.Year, payPeriod.PayPeriodTo.Month, 10).AddMonths(1);

                            empInfoRow.BankAccount = bankAccount.EmpBankCode + " - " + bankAccount.EmpBranchCode + "-" + bankAccount.EmpAccountNo;
                            empInfoRow.CompanyCode = company.CompanyCode;
                            empInfoRow.CompanyName = company.CompanyName;
                            empInfoRow.PositionDesc = position.PositionDesc;

                            empInfoRow.IsShowMinWageInfo = IsShowMinimumWage;
                            empInfoRow.TotalWagesForMinWages = HROne.Payroll.PayrollProcess.GetTotalWagesWithoutRestDayPayment(dbConn, empInfo.EmpID, payPeriod.PayPeriodFr, payPeriod.PayPeriodTo, null);
                            empInfoRow.TotalWorkingHours = HROne.Payroll.PayrollProcess.GetTotalEmpPayrollWorkingHours(dbConn, empInfo.EmpID, payPeriod.PayPeriodID);
                            empInfoRow.MinWagesRequired = empInfoRow.TotalWorkingHours * HROne.Payroll.PayrollProcess.GetMinimumWages(dbConn, empInfo.EmpID, payPeriod.PayPeriodTo);
                            //if (payBatch != null)
                            //    if (payBatch.PayBatchValueDate.Ticks != 0)
                            //        empInfoRow.ValueDate = payBatch.PayBatchValueDate;

                            if (HLevelIDDisplayList != null && empPos != null)
                                for (int count = 0; count < HLevelIDDisplayList.Count && count < 2; count++)
                                {
                                    int hLevelID = Convert.ToInt32(HLevelIDDisplayList[count]);

                                    EHierarchyLevel hLevel = new EHierarchyLevel();
                                    hLevel.HLevelID = hLevelID;
                                    if (EHierarchyLevel.db.select(dbConn, hLevel))
                                    {
                                        empInfoRow["HLevelDesc" + (count + 1)] = hLevel.HLevelDesc;

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
                                                empInfoRow["HElementCode" + (count + 1)] = hElement.HElementCode;
                                                empInfoRow["HElementDesc" + (count + 1)] = hElement.HElementDesc;
                                            }
                                        }
                                    }
                                }
                            foreach (EEmpPayroll empPayroll in empPayrollList)
                                if (!string.IsNullOrEmpty(empPayroll.EmpPayRemark))
                                    if (empInfoRow.IsRemarkNull())
                                        empInfoRow.Remark = empPayroll.EmpPayRemark.Trim();
                                    else if (string.IsNullOrEmpty(empInfoRow.Remark))
                                        empInfoRow.Remark = empPayroll.EmpPayRemark.Trim();
                                    else
                                        empInfoRow.Remark += "\r\n" + empPayroll.EmpPayRemark.Trim();

                            empInfoTable.Rows.Add(empInfoRow);
                            empInfoRow.EmpID = empInfoRow.PaySlipID;

                            DateTime LastValueDate;
                            GeneratePayrollData(empInfoRow.PaySlipID, paymentRecords, out LastValueDate);

                            bool hasMPF = false, hasORSO = false, hasLeaveBalance = false;

                            if (IsShowLeaveBalance)
                                hasLeaveBalance = GenerateLeaveBalanceData(empInfoRow.PaySlipID, empInfo.EmpID, payPeriod.PayPeriodTo);

                            hasMPF = GenerateMPFData(empInfoRow.PaySlipID, mpfRecords);
                            hasORSO = GenerateORSOData(empInfoRow.PaySlipID, orsoRecords);

                            empInfoRow.HasMPF = hasMPF;
                            empInfoRow.HasORSO = hasORSO;
                            empInfoRow.HasLeaveBalance = hasLeaveBalance;

                            if (!LastValueDate.Ticks.Equals(0))
                                empInfoRow.ValueDate = LastValueDate;
                        }
                    }
                }
            System.Data.DataRow[] allowanceRows = dataSet.PaySlip.Select("PayAmount > 0", "PaymentCodeDisplaySeqNo");
            foreach (System.Data.DataRow importRow in allowanceRows)
                dataSet.PaySlip_Allowance.ImportRow(importRow);

            System.Data.DataRow[] deductionRows = dataSet.PaySlip.Select("PayAmount < 0", "PaymentCodeDisplaySeqNo");
            foreach (System.Data.DataRow importRow in deductionRows)
                dataSet.PaySlip_Deduction.ImportRow(importRow);
        }

        private bool GenerateLeaveBalanceData(int PaySlipID, int EmpID, DateTime AsOfDate)
        {
            bool hasLeaveBalance = false;
            LeaveBalanceCalc calc = new LeaveBalanceCalc(dbConn, EmpID, AsOfDate);
            ArrayList balanceItems = calc.getCurrentBalanceList();
            foreach (ELeaveBalance b in balanceItems)
            {
                if (b.LeaveTypeID.Equals(ELeaveType.ANNUAL_LEAVE_TYPE(dbConn).LeaveTypeID))
                {
                    hasLeaveBalance = true;
                    DataSet.Payroll_PaySlip.LeaveBalanceRow rr = dataSet.LeaveBalance.NewLeaveBalanceRow();
                    rr.LeaveBalanceID = b.LeaveBalanceID;
                    rr.EmpID = PaySlipID;
                    rr.LeaveBalanceEffectiveDate = b.LeaveBalanceEffectiveDate;
                    rr.LeaveBalanceBF = b.LeaveBalanceBF;
                    rr.LeaveBalanceEntitled = b.LeaveBalanceEntitled;
                    rr.LeaveBalanceForfeiture = b.LeaveBalanceForfeiture + b.ExpiryForfeit;
                    rr.Taken = b.Taken;
                    rr.Adjust = b.Adjust;
                    rr.Reserved = b.Reserved;
                    rr.Balance = b.getBalance();
                    rr.Name = b.Name;
                    rr.Description = b.Description;
                    rr.StringFormat = b.StringFormat;


                    dataSet.LeaveBalance.Rows.Add(rr);
                }
            }
            return hasLeaveBalance;

        }


        private bool GenerateMPFData(int PaySlipID, ArrayList mpfRecords)
        {
            bool hasMPF = false;
            DataSet.Payroll_PaySlip.PaySlip_MPFRow mpfRow = null;
            foreach (EMPFRecord mpfRecord in mpfRecords)
            {
                hasMPF = true;

                if (mpfRow == null)
                    mpfRow = CreatePaySlipMPFRow(PaySlipID, mpfRecord.MPFRecPeriodFr, mpfRecord.MPFRecPeriodTo);
                if (!(mpfRecord.MPFRecPeriodFr.Equals(mpfRow.PeriodFr) && mpfRecord.MPFRecPeriodTo.Equals(mpfRow.PeriodTo)))
                {
                    dataSet.PaySlip_MPF.Rows.Add(mpfRow);
                    mpfRow = CreatePaySlipMPFRow(PaySlipID, mpfRecord.MPFRecPeriodFr, mpfRecord.MPFRecPeriodTo);
                }
                mpfRow.RelevantIncome += mpfRecord.MPFRecActMCRI;
                mpfRow.MPFEE += mpfRecord.MPFRecActMCEE;
                mpfRow.MPFER += mpfRecord.MPFRecActMCER;
                mpfRow.VCEE += mpfRecord.MPFRecActVCEE;
                mpfRow.VCER += mpfRecord.MPFRecActVCER;
            }
            if (mpfRow != null)
                dataSet.PaySlip_MPF.Rows.Add(mpfRow);
            return hasMPF;
        }

        private bool GenerateORSOData(int PaySlipID, ArrayList orsoRecords)
        {
            bool hasORSO = false;
            DataSet.Payroll_PaySlip.PaySlip_ORSORow orsoRow = null;
            foreach (EORSORecord orsoRecord in orsoRecords)
            {
                hasORSO = true;

                if (orsoRow == null)
                    orsoRow = CreatePaySlipORSORow(PaySlipID, orsoRecord.ORSORecPeriodFr, orsoRecord.ORSORecPeriodTo);
                if (!(orsoRecord.ORSORecPeriodFr.Equals(orsoRow.PeriodFr) && orsoRecord.ORSORecPeriodTo.Equals(orsoRow.PeriodTo)))
                {
                    dataSet.PaySlip_ORSO.Rows.Add(orsoRow);
                    orsoRow = CreatePaySlipORSORow(PaySlipID, orsoRecord.ORSORecPeriodFr, orsoRecord.ORSORecPeriodTo);
                }
                // Start 0000206, Ricky So, 2015-05-22
                //orsoRow.RelevantIncome += orsoRecord.ORSORecActRI;
                // End 0000206, Ricky So, 2015-05-22
                orsoRow.EE += orsoRecord.ORSORecActEE;
                orsoRow.ER += orsoRecord.ORSORecActER;
            }
            // Start 0000206, Ricky So, 2015-05-22
            if (orsoRow != null)
            {
                EEmpPayroll m_empPayroll = EEmpPayroll.GetObject(dbConn, ((EORSORecord) orsoRecords[0]).EmpPayrollID);
                EPayrollPeriod m_payPeriod = EPayrollPeriod.GetObject(dbConn, m_empPayroll.PayPeriodID);

                DBFilter m_basicSalaryFilter = new DBFilter();
                m_basicSalaryFilter.add(AppUtils.GetPayemntCodeDBTermByPaymentType(dbConn, "PayCodeID", "BASICSAL"));
                m_basicSalaryFilter.add(new Match("EmpID", m_empPayroll.EmpID));
                m_basicSalaryFilter.add(new Match("EmpRPEffFr", "<=", m_payPeriod.PayPeriodTo));

                OR m_or = new OR();
                m_or.add(new Match("EmpRPEffTo", ">=", m_payPeriod.PayPeriodFr));
                m_or.add(new NullTerm("EmpRPEffTo"));

                m_basicSalaryFilter.add(m_or);

                foreach (EEmpRecurringPayment m_payment in EEmpRecurringPayment.db.select(dbConn, m_basicSalaryFilter))
                {
                    orsoRow.RelevantIncome += m_payment.EmpRPAmount;
                }
            }
            // End 0000206, Ricky So, 2015-05-22

            if (orsoRow != null)
                dataSet.PaySlip_ORSO.Rows.Add(orsoRow);
            return hasORSO;
        }

        private void GeneratePayrollData(int PaySlipID, ArrayList paymentRecords, out DateTime LastValueDate)
        {
            LastValueDate = new DateTime();
            DataSet.Payroll_PaySlip.PaySlipRow row = null;
            foreach (EPaymentRecord paymentRecord in paymentRecords)
            {
                string paymentMethod = string.Empty;
                if (paymentRecord.PayRecMethod.Equals("A", StringComparison.CurrentCultureIgnoreCase))
                    paymentMethod = "Autopay";
                else if (paymentRecord.PayRecMethod.Equals("Q", StringComparison.CurrentCultureIgnoreCase))
                    paymentMethod = "Cheque";
                else if (paymentRecord.PayRecMethod.Equals("C", StringComparison.CurrentCultureIgnoreCase))
                    paymentMethod = "Cash";
                else if (paymentRecord.PayRecMethod.Equals("O", StringComparison.CurrentCultureIgnoreCase))
                    paymentMethod = "Others";
                else
                    paymentMethod = paymentRecord.PayRecMethod;


                EPaymentCode paymentCode = new EPaymentCode();
                paymentCode.PaymentCodeID = paymentRecord.PaymentCodeID;
                EPaymentCode.db.select(dbConn, paymentCode);

                DateTime valueDate = new DateTime();
                EEmpPayroll empPayroll = new EEmpPayroll();
                empPayroll.EmpPayrollID = paymentRecord.EmpPayrollID;
                if (EEmpPayroll.db.select(dbConn, empPayroll))
                {
                    EPayrollBatch payBatch = new EPayrollBatch();
                    payBatch.PayBatchID = empPayroll.PayBatchID;
                    if (EPayrollBatch.db.select(dbConn, payBatch))
                    {
                        if (!payBatch.PayBatchValueDate.Ticks.Equals(0))
                            valueDate = payBatch.PayBatchValueDate;
                    }
                    if (!empPayroll.EmpPayValueDate.Ticks.Equals(0))
                        if (valueDate.Ticks.Equals(0))
                            valueDate = empPayroll.EmpPayValueDate;
                        else
                            valueDate = empPayroll.EmpPayValueDate > valueDate ? empPayroll.EmpPayValueDate : valueDate;
                }

                //  Hide payment if payment code is chosen for hidden under payslip
                if (!this.reportType.Equals(ReportType.PaySlip) || !paymentCode.PaymentCodeHideInPaySlip)
                {
                    //                    string paymentCodeFullName = paymentCode.PaymentCode + " - " + paymentCode.PaymentCodeDesc;
                    if (row == null)
                        row = dataSet.PaySlip.NewPaySlipRow();
                    else if (!(row.EmpID == PaySlipID && row.PayMethod == paymentMethod && row.PaymentCode == paymentCode.PaymentCode && (valueDate.Equals(row.ValueDate) || !this.reportType.Equals(ReportType.PaySlip))))
                    {
                        if (row.ValueDate.Ticks.Equals(0))
                            row.SetValueDateNull();
                        dataSet.PaySlip.Rows.Add(row);
                        row = dataSet.PaySlip.NewPaySlipRow();
                    }
                    if (row.IsNull("EmpID"))
                    {
                        row.EmpID = PaySlipID;
                        row.PaymentCode = paymentCode.PaymentCode;
                        row.PaymentCodeDesc = paymentCode.PaymentCodeDesc;
                        row.PaymentCodeDisplaySeqNo = paymentCode.PaymentCodeDisplaySeqNo;
                        row.PayMethod = paymentMethod;
                        row.PayAmount = 0;
                        //if (!valueDate.Ticks.Equals(0))
                        row.ValueDate = valueDate;
                    }
                    row.PayAmount += paymentRecord.PayRecActAmount;
                    if (!string.IsNullOrEmpty(paymentRecord.PayRecRemark))
                        if (row.IsRemarkNull())
                            row.Remark = paymentRecord.PayRecRemark;
                        else
                            row.Remark += "\r\n" + paymentRecord.PayRecRemark;
                }
                if (valueDate > LastValueDate)
                    LastValueDate = valueDate;
            }
            if (row != null)
            {
                if (row.ValueDate.Ticks.Equals(0))
                    row.SetValueDateNull();
                dataSet.PaySlip.Rows.Add(row);
            }
        }

        private DataSet.Payroll_PaySlip.PaySlip_MPFRow CreatePaySlipMPFRow(int PaySlipID, DateTime PeriodFr, DateTime PeriodTo)
        {
            DataSet.Payroll_PaySlip.PaySlip_MPFRow mpfRow = dataSet.PaySlip_MPF.NewPaySlip_MPFRow();
            mpfRow.EmpID = PaySlipID;
            mpfRow.PeriodFr = PeriodFr;
            mpfRow.PeriodTo = PeriodTo;
            mpfRow.RelevantIncome = 0;
            mpfRow.MPFEE = 0;
            mpfRow.MPFER = 0;
            mpfRow.VCEE = 0;
            mpfRow.VCER = 0;
            return mpfRow;
        }

        private DataSet.Payroll_PaySlip.PaySlip_ORSORow CreatePaySlipORSORow(int PaySlipID, DateTime PeriodFr, DateTime PeriodTo)
        {
            DataSet.Payroll_PaySlip.PaySlip_ORSORow orsoRow = dataSet.PaySlip_ORSO.NewPaySlip_ORSORow();
            orsoRow.EmpID = PaySlipID;
            orsoRow.PeriodFr = PeriodFr;
            orsoRow.PeriodTo = PeriodTo;
            orsoRow.RelevantIncome = 0;
            orsoRow.EE = 0;
            orsoRow.ER = 0;
            return orsoRow;
        }
    }

}
