using System;
using System.Data;
using System.Configuration;
using System.Collections;
using HROne.Lib.Entities;
using HROne.Lib;
using HROne.DataAccess;
using HROne.CommonLib;

namespace HROne.Payroll
{

    public class PayrollProcess
    {
        DatabaseConnection dbConn;

        public PayrollProcess(DatabaseConnection dbConn)
        {
            this.dbConn = dbConn;
        }

        public EEmpPayroll PayrollTrialRun(int PayPeriodID, int EmpID, bool TrialRunRecurringPayment, bool TrialRunClaimsAndDeduction, bool TrialRunAdditionalRemuneration, bool TrialRunYearEndBonus, bool SkipCNDMPF, int UserID)
        {
            EPayrollPeriod payrollPeriod = new EPayrollPeriod();
            payrollPeriod.PayPeriodID = PayPeriodID;
            EPayrollPeriod.db.select(dbConn, payrollPeriod);


            DBFilter filter = new DBFilter();
            filter.add(new Match("EmpID", EmpID));
            filter.add(new Match("PayGroupID", payrollPeriod.PayGroupID));
            filter.add(new Match("EmpPosEffFr", "<=", payrollPeriod.PayPeriodTo));

            OR orFilter = new OR();
            orFilter.add(new Match("EmpPosEffTo", ">=", payrollPeriod.PayPeriodFr));
            orFilter.add(new NullTerm("EmpPosEffTo"));
            filter.add(orFilter);

            filter.add("EmpPosEffFr", true);
            ArrayList positions = EEmpPositionInfo.db.select(dbConn, filter);

            //  If no position record found, assume the trial run parameter is incorrected
            if (positions.Count == 0)
                return null;

            ArrayList paymentRecords = new ArrayList();
            ArrayList mpfRecords = new ArrayList();
            ArrayList orsoRecords = new ArrayList();

            double dblNumberOfDays = 0;

            //  Merge position info with same payrol group Into 1 if continuous

            ArrayList mergePositionList = new ArrayList();

            foreach (EEmpPositionInfo empPos in positions)
            {
                if (mergePositionList.Count <= 0)
                    mergePositionList.Add(empPos);
                else
                {
                    EEmpPositionInfo lastPosition = (EEmpPositionInfo)mergePositionList[mergePositionList.Count - 1];
                    if (lastPosition.EmpPosEffTo.AddDays(1).Equals(empPos.EmpPosEffFr))
                        lastPosition.EmpPosEffTo = empPos.EmpPosEffTo;
                    else
                        mergePositionList.Add(empPos);
                }
            }

            double dblTotalWorkingHour = 0;
            foreach (EEmpPositionInfo empPos in mergePositionList)
            {
                DateTime dateFrom, dateTo;

                dateFrom = (empPos.EmpPosEffFr > payrollPeriod.PayPeriodFr ? empPos.EmpPosEffFr : payrollPeriod.PayPeriodFr);
                if (empPos.EmpPosEffTo.ToBinary() == 0)
                    dateTo = payrollPeriod.PayPeriodTo;
                else
                    dateTo = (empPos.EmpPosEffTo < payrollPeriod.PayPeriodTo ? empPos.EmpPosEffTo : payrollPeriod.PayPeriodTo);

                if (TrialRunRecurringPayment)
                {
                    if (!IsFinalPayment(EmpID, dateFrom, dateTo))
                    {
                        paymentRecords.AddRange(RecurringPaymentTrialRun(EmpID, payrollPeriod, dateFrom, dateTo, false));
                        paymentRecords.AddRange(ContractGratuityTrialRun(EmpID, dateFrom, dateTo));
                    }
                    else
                        paymentRecords.AddRange(FinalPaymentTrialRun(EmpID));
                }
                if (TrialRunYearEndBonus)
                {
                    if (payrollPeriod.PayPeriodTo <= dateTo)
                    {
                        DBFilter empTermFilter = new DBFilter();
                        empTermFilter.add(new Match("EmpID", EmpID));
                        empTermFilter.add(new Match("EmpTermLastDate", "<=", payrollPeriod.PayPeriodTo));
                        if (EEmpTermination.db.count(dbConn, empTermFilter) == 0)
                            paymentRecords.AddRange(YearEndBonusTrialRun(EmpID, payrollPeriod, dateFrom, dateTo, false));
                    }
                }
                if (TrialRunClaimsAndDeduction)
                    paymentRecords.AddRange(ClaimsAndDeductionTrialRun(EmpID, payrollPeriod, dateFrom, dateTo));

                //  Leave Deduction is calculated AFTER CND to prevent insuffectient amount to deduct for payroll calculated by CND.
                if (TrialRunRecurringPayment)
                {
                    if (!IsFinalPayment(EmpID, dateFrom, dateTo))
                    {
                        paymentRecords.AddRange(LeaveAndStatutoryHolidayTrialRun(EmpID, payrollPeriod, dateFrom, dateTo, paymentRecords, false));
                    }
                }

                dblNumberOfDays += (dateTo.Subtract(dateFrom).Days + 1);
                dblTotalWorkingHour += GetTotalWorkingHours(EmpID, payrollPeriod, dateFrom, dateTo);
            }

            paymentRecords.AddRange(GetRoundingDifferencePaymentList(EmpID, paymentRecords));

            if (TrialRunAdditionalRemuneration)
                paymentRecords.AddRange(AdditionalRemunerationTrialRun(EmpID, payrollPeriod, paymentRecords, dblTotalWorkingHour));
            if ((IsRecurringPaymentTrialRunConfirmExists(EmpID, PayPeriodID) || TrialRunRecurringPayment) || !SkipCNDMPF)
            {
                mpfRecords = MPFProcess.MPFTrialRun(dbConn, EmpID, payrollPeriod, paymentRecords);
                orsoRecords = ORSOProcess.ORSOTrialRun(dbConn, EmpID, payrollPeriod, paymentRecords, (mpfRecords.Count> 0 ) ? (EMPFRecord) mpfRecords[0] : null );

                ArrayList mpfPaymentRecord = MPFProcess.GenerateMPFEEPaymentRecords(dbConn, EmpID, payrollPeriod, mpfRecords, paymentRecords);
                ArrayList orsoPaymentRecord = ORSOProcess.GenerateORSOEEPaymentRecords(dbConn, EmpID, orsoRecords, paymentRecords);

                if (mpfPaymentRecord != null)
                    paymentRecords.AddRange(mpfPaymentRecord);
                if (orsoPaymentRecord != null)
                    paymentRecords.AddRange(orsoPaymentRecord);
            }

            EEmpPayroll empPayroll = new EEmpPayroll();
            empPayroll.EmpID = EmpID;
            if (TrialRunRecurringPayment || TrialRunAdditionalRemuneration)
            {
                empPayroll.EmpPayTotalWorkingHours = dblTotalWorkingHour - GetTotalEmpPayrollWorkingHours(dbConn, EmpID, payrollPeriod.PayPeriodID);
                //  Change Day Count to Payment Record
                //empPayroll.EmpPayNumOfDayCount = 0;//dblNumberOfDays;
            }
            else
            {
                empPayroll.EmpPayTotalWorkingHours = 0;
                //empPayroll.EmpPayNumOfDayCount = 0;
            }
            empPayroll.EmpPayStatus = "T";
            empPayroll.EmpPayIsRP = TrialRunRecurringPayment ? "Y" : "N";
            empPayroll.EmpPayIsCND = TrialRunClaimsAndDeduction ? "Y" : "N";
            empPayroll.EmpPayIsYEB = TrialRunYearEndBonus ? "Y" : "N";
            empPayroll.EmpPayIsAdditionalRemuneration = TrialRunAdditionalRemuneration ? "Y" : "N";
            empPayroll.EmpPayIsHistoryAdj = "N";
            empPayroll.PayPeriodID = PayPeriodID;
            empPayroll.EmpPayTrialRunDate = AppUtils.ServerDateTime();
            empPayroll.EmpPayTrialRunBy = UserID;
            EEmpPayroll.db.insert(dbConn, empPayroll);


            foreach (EPaymentRecord paymentRecord in paymentRecords)
            {
                if (paymentRecord != null)
                {
                    if (paymentRecord.PaymentCodeID > 0)
                    {
                        if (paymentRecord.PayRecMethod.Equals("A"))
                        {
                            if (paymentRecord.EmpAccID != 0)
                            {
                                EEmpBankAccount bankAccount = new EEmpBankAccount();
                                bankAccount.EmpBankAccountID = paymentRecord.EmpAccID;
                                if (!EEmpBankAccount.db.select(dbConn, bankAccount))
                                    paymentRecord.EmpAccID = 0;
                            }
                            if (paymentRecord.EmpAccID == 0)
                            {
                                EEmpBankAccount bankAccount = EEmpBankAccount.GetDefaultBankAccount(dbConn, EmpID);
                                if (bankAccount != null)
                                    paymentRecord.EmpAccID = bankAccount.EmpBankAccountID;
                                //else
                                //    errors.addError("EmpAccID", HROne.Translation.PageErrorMessage.ERROR_ACCOUNT_REQUIRED);
                            }
                        }

                        paymentRecord.EmpPayrollID = empPayroll.EmpPayrollID;
                        EPaymentRecord.db.insert(dbConn, paymentRecord);
                    }
                    if (paymentRecord.RelatedObject != null)
                    {
                        UpdateRelatedObject(paymentRecord.RelatedObject, empPayroll.EmpPayrollID, paymentRecord.PayRecID);
                    }
                }
            }



            foreach (EMPFRecord mpfRecord in mpfRecords)
            {
                if (mpfRecord != null)
                {
                    mpfRecord.EmpPayrollID = empPayroll.EmpPayrollID;
                    EMPFRecord.db.insert(dbConn, mpfRecord);
                }
            }

            foreach (EORSORecord orsoRecord in orsoRecords)
            {
                if (orsoRecord != null)
                {
                    orsoRecord.EmpPayrollID = empPayroll.EmpPayrollID;
                    EORSORecord.db.insert(dbConn, orsoRecord);
                }
            }

            return empPayroll;

        }

        public void PayrollConfirm(EEmpPayroll empPayroll, int PayrollBatchID, int UserID)
        {
            if (empPayroll != null)
            {
                EEmpPayroll.db.select(dbConn, empPayroll);
                if (empPayroll.EmpPayStatus.Equals("T", StringComparison.CurrentCultureIgnoreCase))
                {
                    empPayroll.EmpPayStatus = "C";
                    empPayroll.EmpPayConfirmDate = AppUtils.ServerDateTime();
                    empPayroll.EmpPayConfirmBy = UserID;
                    empPayroll.PayBatchID = PayrollBatchID;
                    EEmpPayroll.db.update(dbConn, empPayroll);
                }
            }
        }

        private void UpdateRelatedObject(object RelatedObject, int EmpPayrollID, int PayRecID)
        {
            if (RelatedObject.GetType() == typeof(EClaimsAndDeductions))
            {
                EClaimsAndDeductions objCND = (EClaimsAndDeductions)RelatedObject;
                objCND.EmpPayrollID = EmpPayrollID;
                objCND.PayRecID = PayRecID;
                EClaimsAndDeductions.db.update(dbConn, objCND);
            }
            else if (RelatedObject.GetType() == typeof(ELeaveApplication))
            {
                ELeaveApplication objLeaveApp = (ELeaveApplication)RelatedObject;
                objLeaveApp.EmpPaymentID = PayRecID;
                objLeaveApp.EmpPayrollID = EmpPayrollID;
                ELeaveApplication.db.update(dbConn, objLeaveApp);
            }
            else if (RelatedObject.GetType() == typeof(EEmpFinalPayment))
            {
                EEmpFinalPayment objFinalPayment = (EEmpFinalPayment)RelatedObject;
                objFinalPayment.PayRecID = PayRecID;
                EEmpFinalPayment.db.update(dbConn, objFinalPayment);
                ELeaveApplication objLeaveApp = new ELeaveApplication();
                objLeaveApp.LeaveAppID = objFinalPayment.LeaveAppID;
                if (ELeaveApplication.db.select(dbConn, objLeaveApp))
                {
                    objLeaveApp.EmpPaymentID = PayRecID;
                    objLeaveApp.EmpPayrollID = EmpPayrollID;
                    ELeaveApplication.db.update(dbConn, objLeaveApp);
                }
                if (!string.IsNullOrEmpty(objFinalPayment.LeaveAppIDList))
                {
                    string[] leaveAppIDList = objFinalPayment.LeaveAppIDList.Split(new char[] { '|' });
                    foreach (string leaveAppID in leaveAppIDList)
                    {

                        objLeaveApp.LeaveAppID = int.Parse(leaveAppID, System.Globalization.NumberStyles.HexNumber);
                        if (ELeaveApplication.db.select(dbConn, objLeaveApp))
                        {
                            objLeaveApp.EmpPaymentID = PayRecID;
                            objLeaveApp.EmpPayrollID = EmpPayrollID;
                            ELeaveApplication.db.update(dbConn, objLeaveApp);
                        }
                    }
                }
            }
            else if (RelatedObject is ICollection)
                foreach (object childRelatedObject in (ICollection)RelatedObject)
                    UpdateRelatedObject(childRelatedObject, EmpPayrollID, PayRecID);

        }

        //  known issue:    cal amount=0.545 will be rounded to 0.5 (1d.p) because rounding process use act amount (0.55) for rounding
        public ArrayList GetRoundingDifferencePaymentList(int EmpID, ArrayList paymentRecordList)
        {
            ArrayList differenceArrayList = new ArrayList();
            System.Collections.Generic.Dictionary<int, double> restDayPaymentHash = new System.Collections.Generic.Dictionary<int, double>();
            System.Collections.Generic.Dictionary<int, double> nonRestDayPaymentHash = new System.Collections.Generic.Dictionary<int, double>();
            System.Collections.Generic.Dictionary<int, double> PaymentHash = new System.Collections.Generic.Dictionary<int, double>();

            System.Collections.Generic.Dictionary<int, ArrayList> paymentListByPaymentCodeID = new System.Collections.Generic.Dictionary<int, ArrayList>();
            foreach (EPaymentRecord paymentRecord in paymentRecordList)
            {
                if (paymentRecord != null)
                {
                    if (PaymentHash.ContainsKey(paymentRecord.PaymentCodeID))
                        PaymentHash[paymentRecord.PaymentCodeID] = (double)PaymentHash[paymentRecord.PaymentCodeID] + paymentRecord.PayRecActAmount;
                    else
                        PaymentHash.Add(paymentRecord.PaymentCodeID, paymentRecord.PayRecActAmount);

                    if (paymentRecord.PayRecIsRestDayPayment)
                        if (restDayPaymentHash.ContainsKey(paymentRecord.PaymentCodeID))
                            restDayPaymentHash[paymentRecord.PaymentCodeID] = (double)restDayPaymentHash[paymentRecord.PaymentCodeID] + paymentRecord.PayRecActAmount;
                        else
                            restDayPaymentHash.Add(paymentRecord.PaymentCodeID, paymentRecord.PayRecActAmount);
                    else
                        if (nonRestDayPaymentHash.ContainsKey(paymentRecord.PaymentCodeID))
                            nonRestDayPaymentHash[paymentRecord.PaymentCodeID] = (double)nonRestDayPaymentHash[paymentRecord.PaymentCodeID] + paymentRecord.PayRecActAmount;
                        else
                            nonRestDayPaymentHash.Add(paymentRecord.PaymentCodeID, paymentRecord.PayRecActAmount);

                    if (!paymentListByPaymentCodeID.ContainsKey(paymentRecord.PaymentCodeID))
                        paymentListByPaymentCodeID.Add(paymentRecord.PaymentCodeID, new ArrayList());
                    paymentListByPaymentCodeID[paymentRecord.PaymentCodeID].Add(paymentRecord);
                }
            }
            System.Collections.Generic.Dictionary<int, double> restDayPaymentDifferenceHash = new System.Collections.Generic.Dictionary<int, double>();
            System.Collections.Generic.Dictionary<int, double> nonRestDayPaymentDifferenceHash = new System.Collections.Generic.Dictionary<int, double>();
            System.Collections.Generic.Dictionary<int, double> PaymentDifferenceHash = new System.Collections.Generic.Dictionary<int, double>();

            foreach (int paymentCodeID in PaymentHash.Keys)
            {
                EPaymentCode paymentCode = new EPaymentCode();
                paymentCode.PaymentCodeID = paymentCodeID;
                EPaymentCode.db.select(dbConn, paymentCode);
                double originalAmount = (double)PaymentHash[paymentCodeID];
                double roundingAmount = originalAmount;
                if (string.IsNullOrEmpty(paymentCode.PaymentCodeRoundingRule))
                {
                    paymentCode.PaymentCodeRoundingRule = Values.ROUNDING_RULE_ROUND_TO;
                    paymentCode.PaymentCodeDecimalPlace = 2;
                }
                if (paymentCode.PaymentCodeRoundingRule.Equals(Values.ROUNDING_RULE_ROUND_UP, StringComparison.CurrentCultureIgnoreCase))
                {
                    roundingAmount = HROne.CommonLib.GenericRoundingFunctions.RoundingUp(originalAmount, paymentCode.PaymentCodeDecimalPlace, ExchangeCurrency.DefaultCurrencyDecimalPlaces(), paymentCode.PaymentCodeRoundingRuleIsAbsoluteValue);
                }
                else if (paymentCode.PaymentCodeRoundingRule.Equals(Values.ROUNDING_RULE_ROUND_DOWN, StringComparison.CurrentCultureIgnoreCase))
                {
                    roundingAmount = HROne.CommonLib.GenericRoundingFunctions.RoundingDown(originalAmount, paymentCode.PaymentCodeDecimalPlace, ExchangeCurrency.DefaultCurrencyDecimalPlaces(), paymentCode.PaymentCodeRoundingRuleIsAbsoluteValue);
                }
                else if (paymentCode.PaymentCodeRoundingRule.Equals(Values.ROUNDING_RULE_ROUND_TO, StringComparison.CurrentCultureIgnoreCase))
                {
                    roundingAmount = HROne.CommonLib.GenericRoundingFunctions.RoundingTo(originalAmount, paymentCode.PaymentCodeDecimalPlace, ExchangeCurrency.DefaultCurrencyDecimalPlaces());
                }
                double difference = Math.Round(roundingAmount - originalAmount, ExchangeCurrency.DefaultCurrencyDecimalPlaces(), MidpointRounding.AwayFromZero);
                PaymentDifferenceHash.Add(paymentCodeID, difference);
                //if (Math.Abs(difference) > 0)
                //{
                //    ArrayList roundingPayments = GetDeductablePaymentRecord(dbConn, EmpID, paymentRecordList, difference, paymentCodeID);
                //    differenceArrayList.AddRange(roundingPayments);
                //}

            }
            foreach (int paymentCodeID in nonRestDayPaymentHash.Keys)
            {
                EPaymentCode paymentCode = new EPaymentCode();
                paymentCode.PaymentCodeID = paymentCodeID;
                EPaymentCode.db.select(dbConn, paymentCode);
                double originalAmount = (double)nonRestDayPaymentHash[paymentCodeID];
                double roundingAmount = originalAmount;
                if (string.IsNullOrEmpty(paymentCode.PaymentCodeRoundingRule))
                {
                    paymentCode.PaymentCodeRoundingRule = Values.ROUNDING_RULE_ROUND_TO;
                    paymentCode.PaymentCodeDecimalPlace = 2;
                }
                if (paymentCode.PaymentCodeRoundingRule.Equals(Values.ROUNDING_RULE_ROUND_UP, StringComparison.CurrentCultureIgnoreCase))
                {
                    roundingAmount = HROne.CommonLib.GenericRoundingFunctions.RoundingUp(originalAmount, paymentCode.PaymentCodeDecimalPlace, ExchangeCurrency.DefaultCurrencyDecimalPlaces(), paymentCode.PaymentCodeRoundingRuleIsAbsoluteValue);
                }
                else if (paymentCode.PaymentCodeRoundingRule.Equals(Values.ROUNDING_RULE_ROUND_DOWN, StringComparison.CurrentCultureIgnoreCase))
                {
                    roundingAmount = HROne.CommonLib.GenericRoundingFunctions.RoundingDown(originalAmount, paymentCode.PaymentCodeDecimalPlace, ExchangeCurrency.DefaultCurrencyDecimalPlaces(), paymentCode.PaymentCodeRoundingRuleIsAbsoluteValue);
                }
                else if (paymentCode.PaymentCodeRoundingRule.Equals(Values.ROUNDING_RULE_ROUND_TO, StringComparison.CurrentCultureIgnoreCase))
                {
                    roundingAmount = HROne.CommonLib.GenericRoundingFunctions.RoundingTo(originalAmount, paymentCode.PaymentCodeDecimalPlace, ExchangeCurrency.DefaultCurrencyDecimalPlaces());
                }
                double difference = Math.Round(roundingAmount - originalAmount, ExchangeCurrency.DefaultCurrencyDecimalPlaces(), MidpointRounding.AwayFromZero);
                nonRestDayPaymentDifferenceHash.Add(paymentCodeID, difference);
                //if (Math.Abs(difference) > 0)
                //{
                //    ArrayList roundingPayments = GetDeductablePaymentRecord(dbConn, EmpID, paymentRecordList, difference, paymentCodeID);
                //    differenceArrayList.AddRange(roundingPayments);
                //}
            }
            foreach (int paymentCodeID in restDayPaymentHash.Keys)
            {
                EPaymentCode paymentCode = new EPaymentCode();
                paymentCode.PaymentCodeID = paymentCodeID;
                EPaymentCode.db.select(dbConn, paymentCode);
                double originalAmount = (double)restDayPaymentHash[paymentCodeID];
                double roundingAmount = originalAmount;
                if (string.IsNullOrEmpty(paymentCode.PaymentCodeRoundingRule))
                {
                    paymentCode.PaymentCodeRoundingRule = Values.ROUNDING_RULE_ROUND_TO;
                    paymentCode.PaymentCodeDecimalPlace = 2;
                }
                if (paymentCode.PaymentCodeRoundingRule.Equals(Values.ROUNDING_RULE_ROUND_UP, StringComparison.CurrentCultureIgnoreCase))
                {
                    roundingAmount = HROne.CommonLib.GenericRoundingFunctions.RoundingUp(originalAmount, paymentCode.PaymentCodeDecimalPlace, ExchangeCurrency.DefaultCurrencyDecimalPlaces(), paymentCode.PaymentCodeRoundingRuleIsAbsoluteValue);
                }
                else if (paymentCode.PaymentCodeRoundingRule.Equals(Values.ROUNDING_RULE_ROUND_DOWN, StringComparison.CurrentCultureIgnoreCase))
                {
                    roundingAmount = HROne.CommonLib.GenericRoundingFunctions.RoundingDown(originalAmount, paymentCode.PaymentCodeDecimalPlace, ExchangeCurrency.DefaultCurrencyDecimalPlaces(), paymentCode.PaymentCodeRoundingRuleIsAbsoluteValue);
                }
                else if (paymentCode.PaymentCodeRoundingRule.Equals(Values.ROUNDING_RULE_ROUND_TO, StringComparison.CurrentCultureIgnoreCase))
                {
                    roundingAmount = HROne.CommonLib.GenericRoundingFunctions.RoundingTo(originalAmount, paymentCode.PaymentCodeDecimalPlace, ExchangeCurrency.DefaultCurrencyDecimalPlaces());
                }
                double difference = Math.Round(roundingAmount - originalAmount, ExchangeCurrency.DefaultCurrencyDecimalPlaces(), MidpointRounding.AwayFromZero);
                restDayPaymentDifferenceHash.Add(paymentCodeID, difference);
                //if (Math.Abs(difference) > 0)
                //{
                //    ArrayList roundingPayments = GetDeductablePaymentRecord(dbConn, EmpID, paymentRecordList, difference, paymentCodeID);
                //    foreach (EPaymentRecord payRec in roundingPayments)
                //        payRec.PayRecIsRestDayPayment = true;
                //    differenceArrayList.AddRange(roundingPayments);
                //}
            }
            foreach (int paymentCodeID in PaymentDifferenceHash.Keys)
            {
                double difference = (double)PaymentDifferenceHash[paymentCodeID];
                // Apply rounding ONLY when acutal difference is >=0.01
                if (Math.Abs(difference) >= 0.005)  // prevent truncation error
                {
                    double nonRestDayDifference = 0;
                    double restDayDifference = 0;
                    if (nonRestDayPaymentDifferenceHash.ContainsKey(paymentCodeID))
                        nonRestDayDifference = (double)nonRestDayPaymentDifferenceHash[paymentCodeID];
                    if (restDayPaymentDifferenceHash.ContainsKey(paymentCodeID))
                        restDayDifference = (double)restDayPaymentDifferenceHash[paymentCodeID];

                    double roundDifference = difference - (nonRestDayDifference + restDayDifference);
                    if (Math.Abs(roundDifference) >= 0.005)  // prevent truncation error
                    {
                        if (nonRestDayPaymentDifferenceHash.ContainsKey(paymentCodeID) && restDayPaymentDifferenceHash.ContainsKey(paymentCodeID))
                        {
                            nonRestDayPaymentDifferenceHash[paymentCodeID] = nonRestDayDifference + roundDifference / 2;
                            restDayPaymentDifferenceHash[paymentCodeID] = restDayDifference + roundDifference / 2;
                        }
                        else if (nonRestDayPaymentDifferenceHash.ContainsKey(paymentCodeID))
                        {
                            nonRestDayPaymentDifferenceHash[paymentCodeID] = nonRestDayDifference + roundDifference;
                        }
                        else if (restDayPaymentDifferenceHash.ContainsKey(paymentCodeID))
                        {
                            restDayPaymentDifferenceHash[paymentCodeID] = restDayDifference + roundDifference;
                        }
                    }
                }
                else
                {
                    if (nonRestDayPaymentDifferenceHash.ContainsKey(paymentCodeID))
                        nonRestDayPaymentDifferenceHash.Remove(paymentCodeID);
                    if (restDayPaymentDifferenceHash.ContainsKey(paymentCodeID))
                        restDayPaymentDifferenceHash.Remove(paymentCodeID);
                }
            }
            foreach (int paymentCodeID in nonRestDayPaymentDifferenceHash.Keys)
            {
                double difference = (double)nonRestDayPaymentDifferenceHash[paymentCodeID];
                if (Math.Abs(difference) >= 0.005)  // prevent truncation error
                {
                    ArrayList roundingPayments = GetDeductablePaymentRecord(dbConn, EmpID, paymentListByPaymentCodeID[paymentCodeID], difference, paymentCodeID);
                    foreach (EPaymentRecord payRec in roundingPayments)
                        payRec.PayRecIsRestDayPayment = false;
                    differenceArrayList.AddRange(roundingPayments);
                }
            }
            foreach (int paymentCodeID in restDayPaymentDifferenceHash.Keys)
            {
                double difference = (double)restDayPaymentDifferenceHash[paymentCodeID];
                if (Math.Abs(difference) >= 0.005)  // prevent truncation error
                {
                    ArrayList roundingPayments = GetDeductablePaymentRecord(dbConn, EmpID, paymentListByPaymentCodeID[paymentCodeID], difference, paymentCodeID);
                    foreach (EPaymentRecord payRec in roundingPayments)
                        payRec.PayRecIsRestDayPayment = true;
                    differenceArrayList.AddRange(roundingPayments);
                }
            }
            return differenceArrayList;
        }

        private bool IsRecurringPaymentTrialRunConfirmExists(int EmpID, int PayPeriodID)
        {
            DBFilter dbfilter = new DBFilter();
            dbfilter.add(new Match("EmpID", EmpID));
            dbfilter.add(new Match("PayPeriodID", PayPeriodID));
            dbfilter.add(new Match("EmpPayIsRP", "Y"));
            if (EEmpPayroll.db.count(dbConn, dbfilter) > 0)
                return true;
            else
                return false;

        }

        private ArrayList ClaimsAndDeductionTrialRun(int EmpID, EPayrollPeriod payrollPeriod, DateTime PayrollGroupDateFrom, DateTime PayrollGroupDateTo)
        {
            DBFilter filter = new DBFilter();
            filter.add(new Match("EmpID", EmpID));
            filter.add(new Match("CNDEffDate", "<=", PayrollGroupDateTo));

            if (payrollPeriod.PayPeriodFr.Subtract(PayrollGroupDateFrom).Days != 0)
                filter.add(new Match("CNDEffDate", ">=", PayrollGroupDateFrom));

            OR orPayRecFilter = new OR();
            orPayRecFilter.add(new Match("PayRecID", "<=", 0));
            orPayRecFilter.add(new NullTerm("PayRecID"));
            filter.add(orPayRecFilter);


            filter.add("CNDAmount", false);
            ArrayList claimsAndDeduction = EClaimsAndDeductions.db.select(dbConn, filter);

            ArrayList paymentRecords = new ArrayList();

            foreach (EClaimsAndDeductions empCND in claimsAndDeduction)
            {

                EPaymentCode payCode = new EPaymentCode();
                payCode.PaymentCodeID = empCND.PayCodeID;

                EPayrollGroup payGroup = new EPayrollGroup();
                payGroup.PayGroupID = payrollPeriod.PayGroupID;


                if (EPaymentCode.db.select(dbConn, payCode) && EPayrollGroup.db.select(dbConn, payGroup))
                {

                    double amount = empCND.CNDAmount;
                    string Remark = string.Empty;

                    if (payGroup.PayGroupIsCNDProrata && payCode.PaymentCodeIsProrata)
                    {
                        EEmpPersonalInfo empInfo = new EEmpPersonalInfo();
                        empInfo.EmpID = EmpID;
                        EEmpPersonalInfo.db.select(dbConn, empInfo);
                        double numOfRestDay = GetTotalRestDayEntitled(dbConn, EmpID, PayrollGroupDateFrom, PayrollGroupDateTo, false);



                        EPayrollPeriod cndPayPeriod = GenerateDummyPayrollPeriod(dbConn, payGroup.PayGroupID, empCND.CNDEffDate);
                        if (cndPayPeriod != null)
                        {
                            bool blnIsFinalPayment = false;
                            EEmpTermination empTerm = EEmpTermination.GetObjectByEmpID(dbConn, EmpID);
                            if (empTerm != null)
                                if (empTerm.EmpTermLastDate > cndPayPeriod.PayPeriodFr && empTerm.EmpTermLastDate < cndPayPeriod.PayPeriodTo)
                                    blnIsFinalPayment = true;
                            amount = GetProrataAmount(dbConn, empInfo, payGroup, cndPayPeriod, cndPayPeriod.PayPeriodFr, cndPayPeriod.PayPeriodTo, payCode, amount, numOfRestDay, blnIsFinalPayment, out Remark);
                        }
                    }

                    // Full Paid
                    EPaymentRecord paymentRecord = new EPaymentRecord();
                    paymentRecord.CurrencyID = ExchangeCurrency.DefaultCurrency();//empCND.CurrencyID;
                    if (empCND.EmpAccID != 0)
                        paymentRecord.EmpAccID = empCND.EmpAccID;
                    paymentRecord.PaymentCodeID = empCND.PayCodeID;
                    paymentRecord.PayRecMethod = empCND.CNDPayMethod;
                    paymentRecord.PayRecNumOfDayAdj = empCND.CNDNumOfDayAdj;
                    paymentRecord.PayRecStatus = PaymentRecordStatus.PAYRECORDSTATUS_ACTIVE;
                    paymentRecord.PayRecType = PaymentRecordType.PAYRECORDTYPE_CND;

                    paymentRecord.PayRecCalAmount = ExchangeCurrency.Exchange(amount, empCND.CurrencyID, false);
                    paymentRecord.PayRecActAmount = HROne.CommonLib.GenericRoundingFunctions.RoundingTo(paymentRecord.PayRecCalAmount, ExchangeCurrency.DefaultCurrencyDecimalPlaces(), ExchangeCurrency.DefaultCurrencyDecimalPlaces());
                    paymentRecord.CostCenterID = empCND.CostCenterID;
                    paymentRecord.PayRecIsRestDayPayment = empCND.CNDIsRestDayPayment;
                    paymentRecord.PayRecRemark = string.IsNullOrEmpty(empCND.CNDRemark) ? Remark : string.IsNullOrEmpty(Remark) ? empCND.CNDRemark : empCND.CNDRemark + "\r\n" + Remark;
                    paymentRecord.RelatedObject = empCND;

                    paymentRecords.Add(paymentRecord);
                }
            }
            return paymentRecords;
        }

        public ArrayList RecurringPaymentTrialRun(int EmpID, EPayrollPeriod payrollPeriod, DateTime PayrollGroupDateFrom, DateTime PayrollGroupDateTo, bool IsFinalPayment)
        {
            DBFilter filter = new DBFilter();
            filter.add(new Match("EmpID", EmpID));
            filter.add(new Match("EmpRPEffFr", "<=", PayrollGroupDateTo));

            OR orFilter = new OR();
            orFilter.add(new Match("EmpRPEffTo", ">=", PayrollGroupDateFrom));
            orFilter.add(new NullTerm("EmpRPEffTo"));
            filter.add(orFilter);

            OR orEmpRPIsNonPayrollItem = new OR();
            orEmpRPIsNonPayrollItem.add(new Match("EmpRPIsNonPayrollItem", false));
            orEmpRPIsNonPayrollItem.add(new NullTerm("EmpRPIsNonPayrollItem"));
            filter.add(orEmpRPIsNonPayrollItem);

            filter.add("EmpRPEffFr", true);
            ArrayList recurringPayment = EEmpRecurringPayment.db.select(dbConn, filter);

            ArrayList paymentRecords = new ArrayList();

            EEmpPersonalInfo empPersonalInfo = new EEmpPersonalInfo();
            empPersonalInfo.EmpID = EmpID;
            EEmpPersonalInfo.db.select(dbConn, empPersonalInfo);

            EPayrollGroup payrollGroup = new EPayrollGroup();
            payrollGroup.PayGroupID = payrollPeriod.PayGroupID;
            EPayrollGroup.db.select(dbConn, payrollGroup);

            DateTime basicSalaryDateFrom = new DateTime(), basicSalaryDateTo = new DateTime();
            foreach (EEmpRecurringPayment empRP in recurringPayment)
            {
                DateTime dateFrom, dateTo;
                DateTime actualDateFrom, actualDateTo;

                dateFrom = (empRP.EmpRPEffFr > PayrollGroupDateFrom ? empRP.EmpRPEffFr : PayrollGroupDateFrom);
                if (empRP.EmpRPEffTo.ToBinary() == 0)
                    dateTo = PayrollGroupDateTo;
                else
                    dateTo = (empRP.EmpRPEffTo < PayrollGroupDateTo ? empRP.EmpRPEffTo : PayrollGroupDateTo);
                actualDateFrom = empPersonalInfo.EmpDateOfJoin > dateFrom ? empPersonalInfo.EmpDateOfJoin : dateFrom;
                actualDateTo = dateTo;

                EPaymentCode paymentCode = new EPaymentCode();
                paymentCode.PaymentCodeID = empRP.PayCodeID;
                EPaymentCode.db.select(dbConn, paymentCode);

                if (empRP.EmpRPUnit.Equals("P", StringComparison.CurrentCultureIgnoreCase) && !empRP.EmpRPUnitPeriodAsDaily)
                {
                    double numOfRestDay = GetTotalRestDayEntitled(dbConn, EmpID, actualDateFrom, actualDateTo, false);

                    EPaymentRecord paymentRecord = new EPaymentRecord();
                    paymentRecord.CurrencyID = ExchangeCurrency.DefaultCurrency();
                    paymentRecord.EmpAccID = empRP.EmpAccID;
                    paymentRecord.PaymentCodeID = empRP.PayCodeID;
                    paymentRecord.PayRecMethod = empRP.EmpRPMethod;
                    paymentRecord.CostCenterID = empRP.CostCenterID;
                    paymentRecord.PayRecNumOfDayAdj = 0;
                    paymentRecord.PayRecStatus = PaymentRecordStatus.PAYRECORDSTATUS_ACTIVE;
                    paymentRecord.PayRecType = PaymentRecordType.PAYRECORDTYPE_RECURRING;

                    string strRemark = string.Empty;
                    paymentRecord.PayRecCalAmount = GetProrataAmount(dbConn, empPersonalInfo, payrollGroup, payrollPeriod, actualDateFrom, actualDateTo, paymentCode, empRP.EmpRPAmount, numOfRestDay, IsFinalPayment, out strRemark);
                    paymentRecord.PayRecRemark = strRemark;

                    paymentRecord.PayRecCalAmount = ExchangeCurrency.Exchange(paymentRecord.PayRecCalAmount, empRP.CurrencyID, false);
                    paymentRecord.PayRecActAmount = HROne.CommonLib.GenericRoundingFunctions.RoundingTo(paymentRecord.PayRecCalAmount, ExchangeCurrency.DefaultCurrencyDecimalPlaces(), ExchangeCurrency.DefaultCurrencyDecimalPlaces());
                    if (paymentCode.PaymentTypeID.Equals(EPaymentType.SystemPaymentType.BasicSalaryPaymentType(dbConn).PaymentTypeID) && paymentCode.PaymentCodeIsWages)
                    {
                        if (basicSalaryDateFrom.Ticks.Equals(0) && basicSalaryDateTo.Ticks.Equals(0))
                        {
                            basicSalaryDateFrom = actualDateFrom;
                            basicSalaryDateTo = actualDateTo;
                            paymentRecord.PayRecNumOfDayAdj = actualDateTo.Subtract(actualDateFrom).Days + 1;
                        }
                        else
                        {
                            paymentRecord.PayRecNumOfDayAdj = (basicSalaryDateTo < actualDateTo ? actualDateTo.Subtract(basicSalaryDateTo).Days : 0) + (basicSalaryDateFrom > actualDateFrom ? basicSalaryDateFrom.Subtract(actualDateFrom).Days : 0);
                            basicSalaryDateFrom = basicSalaryDateFrom < actualDateFrom ? basicSalaryDateFrom : actualDateFrom;
                            basicSalaryDateTo = basicSalaryDateTo > actualDateTo ? basicSalaryDateTo : actualDateTo;

                        }
                        if (!payrollGroup.PayGroupRestDayHasWage)
                            paymentRecord.PayRecNumOfDayAdj -= numOfRestDay;
                    }
                    // Start 2014-06-18, Ricky So, Payscale Backpay CR
                    if (empRP.SchemeCode != null && empRP.Point != null && empRP.SchemeCode != "" && empRP.Point >= 0) // using Payscale system
                        paymentRecord.EmpRPIDforBP = empRP.EmpRPID;
                    // End 2014-06-18, Ricky So, Payscale Backpay CR
                    paymentRecords.Add(paymentRecord);

                    if (payrollGroup.PayGroupRestDayHasWage && paymentCode.PaymentCodeIsWages)
                    {

                        if (numOfRestDay > 0)
                        {
                            string formulaRemark = string.Empty;
                            bool IsDAW = false;
                            double dailyProrata = PayrollFormula.DailyProrataCaluclation(dbConn, payrollGroup.PayGroupRestDayProrataFormula, payrollGroup.PayGroupDefaultProrataFormula, EmpID, empRP.EmpRPAmount, payrollPeriod.PayPeriodFr, payrollPeriod.PayPeriodTo, payrollGroup.NumOfPeriodPerYear(), payrollPeriod.PayPeriodTo, out formulaRemark, out IsDAW);

                            EPaymentRecord restAllowPaymentRecord = new EPaymentRecord();
                            restAllowPaymentRecord.CurrencyID = ExchangeCurrency.DefaultCurrency();
                            restAllowPaymentRecord.EmpAccID = empRP.EmpAccID;
                            restAllowPaymentRecord.PaymentCodeID = empRP.PayCodeID;
                            restAllowPaymentRecord.PayRecMethod = empRP.EmpRPMethod;
                            if (paymentCode.PaymentTypeID.Equals(EPaymentType.SystemPaymentType.BasicSalaryPaymentType(dbConn).PaymentTypeID) && paymentCode.PaymentCodeIsWages)
                                restAllowPaymentRecord.PayRecNumOfDayAdj = numOfRestDay;
                            restAllowPaymentRecord.PayRecStatus = PaymentRecordStatus.PAYRECORDSTATUS_ACTIVE;
                            restAllowPaymentRecord.PayRecType = PaymentRecordType.PAYRECORDTYPE_RECURRING;
                            restAllowPaymentRecord.CostCenterID = empRP.CostCenterID;
                            restAllowPaymentRecord.PayRecIsRestDayPayment = true;
                            restAllowPaymentRecord.PayRecCalAmount = ExchangeCurrency.Exchange(dailyProrata * numOfRestDay, empRP.CurrencyID, false);
                            restAllowPaymentRecord.PayRecActAmount = Math.Round(restAllowPaymentRecord.PayRecCalAmount, ExchangeCurrency.DefaultCurrencyDecimalPlaces(), MidpointRounding.AwayFromZero);

                            EPaymentRecord restDeductPaymentRecord = new EPaymentRecord();
                            restDeductPaymentRecord.CurrencyID = ExchangeCurrency.DefaultCurrency();
                            restDeductPaymentRecord.EmpAccID = empRP.EmpAccID;
                            restDeductPaymentRecord.PaymentCodeID = empRP.PayCodeID;
                            restDeductPaymentRecord.PayRecMethod = empRP.EmpRPMethod;
                            if (paymentCode.PaymentTypeID.Equals(EPaymentType.SystemPaymentType.BasicSalaryPaymentType(dbConn).PaymentTypeID))
                                restDeductPaymentRecord.PayRecNumOfDayAdj = -numOfRestDay;
                            restDeductPaymentRecord.PayRecStatus = PaymentRecordStatus.PAYRECORDSTATUS_ACTIVE;
                            restDeductPaymentRecord.PayRecType = PaymentRecordType.PAYRECORDTYPE_RECURRING;
                            restDeductPaymentRecord.CostCenterID = empRP.CostCenterID;
                            restDeductPaymentRecord.PayRecCalAmount = -restAllowPaymentRecord.PayRecCalAmount;
                            restDeductPaymentRecord.PayRecActAmount = -restAllowPaymentRecord.PayRecActAmount;


                            paymentRecords.Add(restAllowPaymentRecord);
                            paymentRecords.Add(restDeductPaymentRecord);

                        }
                    }
                    if (payrollGroup.PayGroupLunchTimeHasWage && paymentCode.PaymentCodeIsWages)
                    {

                        double LunchTime = GetTotalLunchTime(dbConn, EmpID, actualDateFrom, actualDateTo);

                        EEmpPositionInfo currentEmpPos = AppUtils.GetLastPositionInfo(dbConn, actualDateTo, EmpID);
                        EWorkHourPattern workHourPattern = null;
                        if (currentEmpPos != null)
                        {
                            workHourPattern = new EWorkHourPattern();
                            workHourPattern.WorkHourPatternID = currentEmpPos.WorkHourPatternID;
                            if (!EWorkHourPattern.db.select(dbConn, workHourPattern))
                            {
                                workHourPattern = null;
                            }
                        }

                        if (LunchTime > 0 && workHourPattern != null)
                        {

                            string formulaRemark = string.Empty;
                            bool IsDAW = false;
                            double dailyProrata = PayrollFormula.DailyProrataCaluclation(dbConn, payrollGroup.PayGroupRestDayProrataFormula, payrollGroup.PayGroupDefaultProrataFormula, EmpID, empRP.EmpRPAmount, payrollPeriod.PayPeriodFr, payrollPeriod.PayPeriodTo, payrollGroup.NumOfPeriodPerYear(), payrollPeriod.PayPeriodTo, out formulaRemark, out IsDAW);

                            EPaymentRecord restAllowPaymentRecord = new EPaymentRecord();
                            restAllowPaymentRecord.CurrencyID = ExchangeCurrency.DefaultCurrency();
                            restAllowPaymentRecord.EmpAccID = empRP.EmpAccID;
                            restAllowPaymentRecord.PaymentCodeID = empRP.PayCodeID;
                            restAllowPaymentRecord.PayRecMethod = empRP.EmpRPMethod;
                            restAllowPaymentRecord.PayRecNumOfDayAdj = 0;
                            restAllowPaymentRecord.PayRecStatus = PaymentRecordStatus.PAYRECORDSTATUS_ACTIVE;
                            restAllowPaymentRecord.PayRecType = PaymentRecordType.PAYRECORDTYPE_RECURRING;
                            restAllowPaymentRecord.CostCenterID = empRP.CostCenterID;
                            restAllowPaymentRecord.PayRecIsRestDayPayment = true;
                            restAllowPaymentRecord.PayRecCalAmount = ExchangeCurrency.Exchange(dailyProrata / (workHourPattern.WorkHourPatternContractWorkHoursPerDay + workHourPattern.WorkHourPatternContractLunchTimeHoursPerDay) * LunchTime, empRP.CurrencyID, false);
                            restAllowPaymentRecord.PayRecActAmount = Math.Round(restAllowPaymentRecord.PayRecCalAmount, ExchangeCurrency.DefaultCurrencyDecimalPlaces(), MidpointRounding.AwayFromZero);

                            EPaymentRecord restDeductPaymentRecord = new EPaymentRecord();
                            restDeductPaymentRecord.CurrencyID = ExchangeCurrency.DefaultCurrency();
                            restDeductPaymentRecord.EmpAccID = empRP.EmpAccID;
                            restDeductPaymentRecord.PaymentCodeID = empRP.PayCodeID;
                            restDeductPaymentRecord.PayRecMethod = empRP.EmpRPMethod;
                            restDeductPaymentRecord.PayRecNumOfDayAdj = 0;
                            restDeductPaymentRecord.PayRecStatus = PaymentRecordStatus.PAYRECORDSTATUS_ACTIVE;
                            restDeductPaymentRecord.PayRecType = PaymentRecordType.PAYRECORDTYPE_RECURRING;
                            restDeductPaymentRecord.CostCenterID = empRP.CostCenterID;
                            restDeductPaymentRecord.PayRecCalAmount = -restAllowPaymentRecord.PayRecCalAmount;
                            restDeductPaymentRecord.PayRecActAmount = -restAllowPaymentRecord.PayRecActAmount;


                            paymentRecords.Add(restAllowPaymentRecord);
                            paymentRecords.Add(restDeductPaymentRecord);

                        }
                    }


                }
                else if (empRP.EmpRPUnit.Equals("D", StringComparison.CurrentCultureIgnoreCase) || (empRP.EmpRPUnit.Equals("P", StringComparison.CurrentCultureIgnoreCase) && empRP.EmpRPUnitPeriodAsDaily))
                {
                    if (!payrollGroup.PayGroupUseCNDForDailyHourlyPayment)
                    {
                        double dailyAmount = empRP.EmpRPAmount;
                        string dailyRemark = empRP.EmpRPAmount.ToString("$#,##0.00");
                        {
                            if (!empRP.EmpRPUnit.Equals("D", StringComparison.CurrentCultureIgnoreCase))
                            {
                                bool dummyISDAW;
                                dailyAmount = PayrollFormula.DailyProrataCaluclation(dbConn, empRP.EmpRPUnitPeriodAsDailyPayFormID, payrollGroup.PayGroupDefaultProrataFormula, EmpID, dailyAmount, payrollPeriod.PayPeriodFr, payrollPeriod.PayPeriodTo, payrollGroup.NumOfPeriodPerYear(), actualDateFrom, 0, out dailyRemark, out dummyISDAW);
                            }

                            double numOfWorkingDay = GetTotalWorkingDays(dbConn, EmpID, actualDateFrom, actualDateTo);

                            EPaymentRecord paymentRecord = new EPaymentRecord();
                            paymentRecord.CurrencyID = ExchangeCurrency.DefaultCurrency();
                            paymentRecord.EmpAccID = empRP.EmpAccID;
                            paymentRecord.PaymentCodeID = empRP.PayCodeID;
                            paymentRecord.PayRecMethod = empRP.EmpRPMethod;
                            if (paymentCode.PaymentTypeID.Equals(EPaymentType.SystemPaymentType.BasicSalaryPaymentType(dbConn).PaymentTypeID) && paymentCode.PaymentCodeIsWages)
                                paymentRecord.PayRecNumOfDayAdj = numOfWorkingDay;
                            paymentRecord.PayRecStatus = PaymentRecordStatus.PAYRECORDSTATUS_ACTIVE;
                            paymentRecord.PayRecType = PaymentRecordType.PAYRECORDTYPE_RECURRING;
                            paymentRecord.CostCenterID = empRP.CostCenterID;
                            paymentRecord.PayRecCalAmount = ExchangeCurrency.Exchange(dailyAmount * numOfWorkingDay, empRP.CurrencyID, false);
                            paymentRecord.PayRecActAmount = Math.Round(paymentRecord.PayRecCalAmount, ExchangeCurrency.DefaultCurrencyDecimalPlaces(), MidpointRounding.AwayFromZero);
                            paymentRecord.PayRecRemark = dailyRemark + " x " + numOfWorkingDay;
                            paymentRecords.Add(paymentRecord);
                        }
                        if (payrollGroup.PayGroupRestDayHasWage && paymentCode.PaymentCodeIsWages)
                        {
                            double restDayTaken = GetTotalRestDayTaken(dbConn, EmpID, actualDateFrom, actualDateTo);

                            EPaymentRecord restDayPaymentRecord = new EPaymentRecord();
                            restDayPaymentRecord.CurrencyID = ExchangeCurrency.DefaultCurrency();
                            restDayPaymentRecord.EmpAccID = empRP.EmpAccID;
                            restDayPaymentRecord.PaymentCodeID = empRP.PayCodeID;
                            restDayPaymentRecord.PayRecMethod = empRP.EmpRPMethod;
                            if (paymentCode.PaymentTypeID.Equals(EPaymentType.SystemPaymentType.BasicSalaryPaymentType(dbConn).PaymentTypeID) && paymentCode.PaymentCodeIsWages)
                                restDayPaymentRecord.PayRecNumOfDayAdj = restDayTaken;
                            restDayPaymentRecord.PayRecStatus = PaymentRecordStatus.PAYRECORDSTATUS_ACTIVE;
                            restDayPaymentRecord.PayRecType = PaymentRecordType.PAYRECORDTYPE_RECURRING;
                            restDayPaymentRecord.CostCenterID = empRP.CostCenterID;
                            restDayPaymentRecord.PayRecCalAmount = ExchangeCurrency.Exchange(dailyAmount * restDayTaken, empRP.CurrencyID, false);
                            restDayPaymentRecord.PayRecActAmount = Math.Round(restDayPaymentRecord.PayRecCalAmount, ExchangeCurrency.DefaultCurrencyDecimalPlaces(), MidpointRounding.AwayFromZero);
                            restDayPaymentRecord.PayRecRemark = dailyRemark + " x " + restDayTaken;
                            restDayPaymentRecord.PayRecIsRestDayPayment = true;
                            paymentRecords.Add(restDayPaymentRecord);
                        }
                        if (payrollGroup.PayGroupLunchTimeHasWage && paymentCode.PaymentCodeIsWages)
                        {

                            double LunchTime = GetTotalLunchTime(dbConn, EmpID, actualDateFrom, actualDateTo);

                            EEmpPositionInfo currentEmpPos = AppUtils.GetLastPositionInfo(dbConn, actualDateTo, EmpID);
                            EWorkHourPattern workHourPattern = null;
                            if (currentEmpPos != null)
                            {
                                workHourPattern = new EWorkHourPattern();
                                workHourPattern.WorkHourPatternID = currentEmpPos.WorkHourPatternID;
                                if (!EWorkHourPattern.db.select(dbConn, workHourPattern))
                                {
                                    workHourPattern = null;
                                }
                            }

                            if (LunchTime > 0 && workHourPattern != null)
                            {

                                string formulaRemark = string.Empty;
                                //bool IsDAW = false;
                                double dailyProrata = dailyAmount;

                                EPaymentRecord restAllowPaymentRecord = new EPaymentRecord();
                                restAllowPaymentRecord.CurrencyID = ExchangeCurrency.DefaultCurrency();
                                restAllowPaymentRecord.EmpAccID = empRP.EmpAccID;
                                restAllowPaymentRecord.PaymentCodeID = empRP.PayCodeID;
                                restAllowPaymentRecord.PayRecMethod = empRP.EmpRPMethod;
                                restAllowPaymentRecord.PayRecNumOfDayAdj = 0;
                                restAllowPaymentRecord.PayRecStatus = PaymentRecordStatus.PAYRECORDSTATUS_ACTIVE;
                                restAllowPaymentRecord.PayRecType = PaymentRecordType.PAYRECORDTYPE_RECURRING;
                                restAllowPaymentRecord.CostCenterID = empRP.CostCenterID;
                                restAllowPaymentRecord.PayRecIsRestDayPayment = true;
                                restAllowPaymentRecord.PayRecCalAmount = ExchangeCurrency.Exchange(dailyProrata / (workHourPattern.WorkHourPatternContractWorkHoursPerDay + workHourPattern.WorkHourPatternContractLunchTimeHoursPerDay) * LunchTime, empRP.CurrencyID, false);
                                restAllowPaymentRecord.PayRecActAmount = Math.Round(restAllowPaymentRecord.PayRecCalAmount, ExchangeCurrency.DefaultCurrencyDecimalPlaces(), MidpointRounding.AwayFromZero);

                                EPaymentRecord restDeductPaymentRecord = new EPaymentRecord();
                                restDeductPaymentRecord.CurrencyID = ExchangeCurrency.DefaultCurrency();
                                restDeductPaymentRecord.EmpAccID = empRP.EmpAccID;
                                restDeductPaymentRecord.PaymentCodeID = empRP.PayCodeID;
                                restDeductPaymentRecord.PayRecMethod = empRP.EmpRPMethod;
                                restDeductPaymentRecord.PayRecNumOfDayAdj = 0;
                                restDeductPaymentRecord.CostCenterID = empRP.CostCenterID;
                                restDeductPaymentRecord.PayRecStatus = PaymentRecordStatus.PAYRECORDSTATUS_ACTIVE;
                                restDeductPaymentRecord.PayRecType = PaymentRecordType.PAYRECORDTYPE_RECURRING;
                                restDeductPaymentRecord.PayRecCalAmount = -restAllowPaymentRecord.PayRecCalAmount;
                                restDeductPaymentRecord.PayRecActAmount = -restAllowPaymentRecord.PayRecActAmount;


                                paymentRecords.Add(restAllowPaymentRecord);
                                paymentRecords.Add(restDeductPaymentRecord);

                            }
                        }
                    }
                }
                else if (empRP.EmpRPUnit.Equals("H", StringComparison.CurrentCultureIgnoreCase))
                {
                    if (!payrollGroup.PayGroupUseCNDForDailyHourlyPayment)
                    {
                        double numOfWorkingHour = GetTotalWorkingHours(EmpID, payrollPeriod, actualDateFrom, actualDateTo);
                        double numOfWorkingDay = GetTotalWorkingDays(dbConn, EmpID, actualDateFrom, actualDateTo);

                        EPaymentRecord paymentRecord = new EPaymentRecord();
                        paymentRecord.CurrencyID = ExchangeCurrency.DefaultCurrency();
                        paymentRecord.EmpAccID = empRP.EmpAccID;
                        paymentRecord.PaymentCodeID = empRP.PayCodeID;
                        paymentRecord.PayRecMethod = empRP.EmpRPMethod;
                        if (paymentCode.PaymentTypeID.Equals(EPaymentType.SystemPaymentType.BasicSalaryPaymentType(dbConn).PaymentTypeID) && paymentCode.PaymentCodeIsWages)
                            paymentRecord.PayRecNumOfDayAdj = numOfWorkingDay;
                        paymentRecord.CostCenterID = empRP.CostCenterID;
                        paymentRecord.PayRecStatus = PaymentRecordStatus.PAYRECORDSTATUS_ACTIVE;
                        paymentRecord.PayRecType = PaymentRecordType.PAYRECORDTYPE_RECURRING;
                        paymentRecord.PayRecCalAmount = ExchangeCurrency.Exchange(empRP.EmpRPAmount * numOfWorkingHour, empRP.CurrencyID, false);
                        paymentRecord.PayRecActAmount = Math.Round(paymentRecord.PayRecCalAmount, ExchangeCurrency.DefaultCurrencyDecimalPlaces(), MidpointRounding.AwayFromZero);
                        paymentRecord.PayRecRemark = empRP.EmpRPAmount.ToString("$#,##0.00") + " x " + numOfWorkingHour;

                        paymentRecords.Add(paymentRecord);

                        if (payrollGroup.PayGroupRestDayHasWage && paymentCode.PaymentCodeIsWages)
                        {
                            double restDayTaken = GetTotalRestDayTaken(dbConn, EmpID, actualDateFrom, actualDateTo);

                            EEmpPositionInfo currentEmpPos = AppUtils.GetLastPositionInfo(dbConn, actualDateTo, EmpID);
                            EWorkHourPattern workHourPattern = null;
                            if (currentEmpPos != null)
                            {
                                workHourPattern = new EWorkHourPattern();
                                workHourPattern.WorkHourPatternID = currentEmpPos.WorkHourPatternID;
                                if (!EWorkHourPattern.db.select(dbConn, workHourPattern))
                                {
                                    workHourPattern = null;
                                }
                            }
                            if (restDayTaken > 0 && workHourPattern != null)
                            {
                                double hoursPerDay = payrollGroup.PayGroupLunchTimeHasWage ? (workHourPattern.WorkHourPatternContractWorkHoursPerDay + workHourPattern.WorkHourPatternContractLunchTimeHoursPerDay) : workHourPattern.WorkHourPatternContractWorkHoursPerDay;

                                EPaymentRecord restDayPaymentRecord = new EPaymentRecord();
                                restDayPaymentRecord.CurrencyID = ExchangeCurrency.DefaultCurrency();
                                restDayPaymentRecord.EmpAccID = empRP.EmpAccID;
                                restDayPaymentRecord.PaymentCodeID = empRP.PayCodeID;
                                restDayPaymentRecord.PayRecMethod = empRP.EmpRPMethod;
                                if (paymentCode.PaymentTypeID.Equals(EPaymentType.SystemPaymentType.BasicSalaryPaymentType(dbConn).PaymentTypeID) && paymentCode.PaymentCodeIsWages)
                                    restDayPaymentRecord.PayRecNumOfDayAdj = restDayTaken;
                                restDayPaymentRecord.PayRecStatus = PaymentRecordStatus.PAYRECORDSTATUS_ACTIVE;
                                restDayPaymentRecord.PayRecType = PaymentRecordType.PAYRECORDTYPE_RECURRING;
                                restDayPaymentRecord.CostCenterID = empRP.CostCenterID;
                                restDayPaymentRecord.PayRecCalAmount = ExchangeCurrency.Exchange(empRP.EmpRPAmount * hoursPerDay * restDayTaken, empRP.CurrencyID, false);
                                restDayPaymentRecord.PayRecActAmount = Math.Round(restDayPaymentRecord.PayRecCalAmount, ExchangeCurrency.DefaultCurrencyDecimalPlaces(), MidpointRounding.AwayFromZero);
                                restDayPaymentRecord.PayRecRemark = empRP.EmpRPAmount.ToString("$#,##0.00") + " x " + hoursPerDay + " x " + restDayTaken;
                                restDayPaymentRecord.PayRecIsRestDayPayment = true;
                                paymentRecords.Add(restDayPaymentRecord);
                            }
                        }
                        if (payrollGroup.PayGroupLunchTimeHasWage && paymentCode.PaymentCodeIsWages)
                        {

                            double LunchTime = GetTotalLunchTime(dbConn, EmpID, actualDateFrom, actualDateTo);


                            if (LunchTime > 0)
                            {

                                string formulaRemark = string.Empty;
                                //bool IsDAW = false;
                                double dailyProrata = empRP.EmpRPAmount;

                                EPaymentRecord restAllowPaymentRecord = new EPaymentRecord();
                                restAllowPaymentRecord.CurrencyID = ExchangeCurrency.DefaultCurrency();
                                restAllowPaymentRecord.EmpAccID = empRP.EmpAccID;
                                restAllowPaymentRecord.PaymentCodeID = empRP.PayCodeID;
                                restAllowPaymentRecord.PayRecMethod = empRP.EmpRPMethod;
                                restAllowPaymentRecord.PayRecNumOfDayAdj = 0;
                                restAllowPaymentRecord.PayRecStatus = PaymentRecordStatus.PAYRECORDSTATUS_ACTIVE;
                                restAllowPaymentRecord.PayRecType = PaymentRecordType.PAYRECORDTYPE_RECURRING;
                                restAllowPaymentRecord.CostCenterID = empRP.CostCenterID;
                                restAllowPaymentRecord.PayRecIsRestDayPayment = true;
                                restAllowPaymentRecord.PayRecCalAmount = ExchangeCurrency.Exchange(empRP.EmpRPAmount * LunchTime, empRP.CurrencyID, false);
                                restAllowPaymentRecord.PayRecActAmount = Math.Round(restAllowPaymentRecord.PayRecCalAmount, ExchangeCurrency.DefaultCurrencyDecimalPlaces(), MidpointRounding.AwayFromZero);
                                restAllowPaymentRecord.PayRecRemark = empRP.EmpRPAmount.ToString("$#,##0.00") + " x " + LunchTime;

                                paymentRecords.Add(restAllowPaymentRecord);

                            }
                        }
                    }
                }

            }
            return paymentRecords;
        }

        public ArrayList LeaveAndStatutoryHolidayTrialRun(int EmpID, EPayrollPeriod payrollPeriod, DateTime PayrollGroupDateFrom, DateTime PayrollGroupDateTo, ArrayList previousPaymentRecordResult, bool IsFinalPayment)
        {

            EPayrollGroup payrollGroup = new EPayrollGroup();
            payrollGroup.PayGroupID = payrollPeriod.PayGroupID;
            EPayrollGroup.db.select(dbConn, payrollGroup);

            ArrayList newFormulaPayment = LeavePaymentTrialRun(EmpID, payrollPeriod, previousPaymentRecordResult, PayrollGroupDateFrom, PayrollGroupDateTo);
            ArrayList oldFormulaPayment = null;
            if (payrollGroup.PayGroupPayAdvanceCompareTotalPaymentOnly)
                oldFormulaPayment = LeavePaymentTrialRun(EmpID, payrollPeriod, previousPaymentRecordResult, PayrollGroupDateFrom, PayrollGroupDateTo, true);

            if (!payrollGroup.PayGroupIsSkipStatHol)
                if (payrollGroup.PayGroupStatHolNextMonth)
                {
                    if (payrollPeriod.PayPeriodFr.Equals(PayrollGroupDateFrom))
                    {
                        //  ONLY run statutory holiday payment on FIRST payroll group if more than 1 position information with the payroll cycle
                        DateTime PayrollStatutoryDateFrom = payrollPeriod.PayPeriodFr.AddMonths(-1);
                        DateTime PayrollStatutoryDateTo = payrollPeriod.PayPeriodFr.AddDays(-1);
                        newFormulaPayment.AddRange(StatutoryHolidayTrialRun(EmpID, payrollPeriod, previousPaymentRecordResult, PayrollStatutoryDateFrom, PayrollStatutoryDateTo, false, payrollGroup.PayGroupIsStatHolUsePublicHoliday));
                        if (oldFormulaPayment != null)
                            oldFormulaPayment.AddRange(StatutoryHolidayTrialRun(EmpID, payrollPeriod, previousPaymentRecordResult, PayrollStatutoryDateFrom, PayrollStatutoryDateTo, true, payrollGroup.PayGroupIsStatHolUsePublicHoliday));
                    }
                    if (IsFinalPayment)
                    {
                        newFormulaPayment.AddRange(StatutoryHolidayTrialRun(EmpID, payrollPeriod, previousPaymentRecordResult, PayrollGroupDateFrom, PayrollGroupDateTo, false, payrollGroup.PayGroupIsStatHolUsePublicHoliday));
                        if (oldFormulaPayment != null)
                            oldFormulaPayment.AddRange(StatutoryHolidayTrialRun(EmpID, payrollPeriod, previousPaymentRecordResult, PayrollGroupDateFrom, PayrollGroupDateTo, true, payrollGroup.PayGroupIsStatHolUsePublicHoliday));
                    }
                }
                else
                {
                    newFormulaPayment.AddRange(StatutoryHolidayTrialRun(EmpID, payrollPeriod, previousPaymentRecordResult, PayrollGroupDateFrom, PayrollGroupDateTo, false, payrollGroup.PayGroupIsStatHolUsePublicHoliday));
                    if (oldFormulaPayment != null)
                        oldFormulaPayment.AddRange(StatutoryHolidayTrialRun(EmpID, payrollPeriod, previousPaymentRecordResult, PayrollGroupDateFrom, PayrollGroupDateTo, true, payrollGroup.PayGroupIsStatHolUsePublicHoliday));
                }
            ArrayList paymentRecords = new ArrayList();
            if (oldFormulaPayment != null)
            {
                double newFormulaPaymentTotal = 0;
                double oldFormulaPaymentTotal = 0;
                foreach (EPaymentRecord payment in newFormulaPayment)
                    newFormulaPaymentTotal += payment.PayRecActAmount;
                foreach (EPaymentRecord payment in oldFormulaPayment)
                    oldFormulaPaymentTotal += payment.PayRecActAmount;

                if (oldFormulaPaymentTotal > newFormulaPaymentTotal)
                    paymentRecords.AddRange(oldFormulaPayment);
                else
                    paymentRecords.AddRange(newFormulaPayment);

            }
            else
            {
                //  Compare pay Advancement inside the function.
                //  No need to compare each item
                paymentRecords.AddRange(newFormulaPayment);
            }

            return paymentRecords;
        }

        public ArrayList YearEndBonusTrialRun(int EmpID, EPayrollPeriod payrollPeriod, DateTime PayrollGroupDateFrom, DateTime PayrollGroupDateTo, bool IsFinalPaymentTrialRun)
        {
            ArrayList paymentRecords = new ArrayList();

            EPayrollGroup payrollGroup = new EPayrollGroup();
            payrollGroup.PayGroupID = payrollPeriod.PayGroupID;

            //EEmpTermination empTerm=null;
            //DBFilter empTermFilter = new DBFilter();
            //empTermFilter.add(new Match("EmpID", EmpID));
            ////  YEB is available if last employment date = last date of period
            //empTermFilter.add(new Match("EmpTermLastDate", "<=", payrollPeriod.PayPeriodTo));
            //ArrayList empTerminationList = EEmpTermination.db.select(dbConn, empTermFilter);
            //if (empTerminationList.Count > 0)
            //{
            //    empTerm = (EEmpTermination)empTerminationList[0];
            //}

            EEmpTermination empTerm = EEmpTermination.GetObjectByEmpID(dbConn, EmpID);
            if (empTerm != null)
                if (empTerm.EmpTermLastDate > payrollPeriod.PayPeriodTo)
                    empTerm = null;

            if (EPayrollGroup.db.select(dbConn, payrollGroup))
            {
                if (payrollGroup.PayGroupYEBStartPayrollMonth < 1 || payrollGroup.PayGroupYEBStartPayrollMonth > 12 || payrollGroup.PayGroupYEBStartPayrollMonth < 1 || payrollGroup.PayGroupYEBStartPayrollMonth > 12)
                    return paymentRecords;
                if ((payrollPeriod.PayPeriodFr.Month.Equals(payrollGroup.PayGroupYEBStartPayrollMonth) || payrollPeriod.PayPeriodTo.Month.Equals(payrollGroup.PayGroupYEBStartPayrollMonth)) || IsFinalPaymentTrialRun)
                {
                    DBFilter isYEBTrialRunExistsFilter = new DBFilter();
                    isYEBTrialRunExistsFilter.add(new Match("EmpID", EmpID));
                    isYEBTrialRunExistsFilter.add(new Match("EmpPayIsYEB", "Y"));

                    DBFilter payPeriodFilter = new DBFilter();
                    payPeriodFilter.add(new Match("PayPeriodFr", "<=", payrollPeriod.PayPeriodTo.AddDays(DateTime.DaysInMonth(payrollPeriod.PayPeriodTo.Year, payrollPeriod.PayPeriodTo.Month) - payrollPeriod.PayPeriodTo.Day)));
                    payPeriodFilter.add(new Match("PayPeriodTo", ">=", payrollPeriod.PayPeriodFr.AddDays(-payrollPeriod.PayPeriodFr.Day + 1)));

                    isYEBTrialRunExistsFilter.add(new IN("PayPeriodID", "Select PayPeriodID from " + EPayrollPeriod.db.dbclass.tableName, payPeriodFilter));

                    if (EEmpPayroll.db.count(dbConn, isYEBTrialRunExistsFilter).Equals(0))
                    {
                        EEmpPositionInfo empPos = AppUtils.GetLastPositionInfo(dbConn, payrollPeriod.PayPeriodTo, EmpID);

                        EYEBPlan yebPlan = new EYEBPlan();
                        yebPlan.YEBPlanID = empPos.YEBPlanID;
                        if (!EYEBPlan.db.select(dbConn, yebPlan))
                        {
                            DBFilter globalYebPlanFilter = new DBFilter();
                            globalYebPlanFilter.add(new Match("YEBPlanIsGlobal", true));
                            ArrayList globalYEBPlanList = EYEBPlan.db.select(dbConn, globalYebPlanFilter);
                            if (globalYEBPlanList.Count > 0)
                            {
                                yebPlan = (EYEBPlan)globalYEBPlanList[0];
                            }
                        }
                        if (yebPlan.YEBPlanID > 0)
                        {
                            DateTime yebPlanPeriodFr, yebPlanPeriodTo;

                            //  get effective period for YEB calculation
                            yebPlanPeriodTo = new DateTime(payrollPeriod.PayPeriodTo.Year, payrollPeriod.PayPeriodTo.Month, 1);
                            while (!yebPlanPeriodTo.Month.Equals(payrollGroup.PayGroupYEBMonthTo))
                                yebPlanPeriodTo = yebPlanPeriodTo.AddMonths(-1);
                            yebPlanPeriodTo = yebPlanPeriodTo.AddMonths(1).AddDays(-1);
                            if (!IsFinalPaymentTrialRun)
                            {
                                if (yebPlanPeriodTo > payrollPeriod.PayPeriodTo)
                                    yebPlanPeriodTo = payrollPeriod.PayPeriodTo;
                            }
                            else
                            {
                                if (yebPlanPeriodTo < empTerm.EmpTermLastDate)
                                {
                                    //  run full period of within YEB period
                                    if (payrollPeriod.PayPeriodFr.Month.Equals(payrollGroup.PayGroupYEBStartPayrollMonth) || payrollPeriod.PayPeriodTo.Month.Equals(payrollGroup.PayGroupYEBStartPayrollMonth))
                                        paymentRecords.AddRange(YearEndBonusTrialRun(EmpID, payrollPeriod, PayrollGroupDateFrom, PayrollGroupDateTo, false));
                                    while (yebPlanPeriodTo < empTerm.EmpTermLastDate)
                                        yebPlanPeriodTo = new DateTime(yebPlanPeriodTo.Year + 1, yebPlanPeriodTo.Month, 1);
                                    yebPlanPeriodTo = yebPlanPeriodTo.AddMonths(1).AddDays(-1);
                                }
                            }
                            yebPlanPeriodFr = new DateTime(yebPlanPeriodTo.Year, yebPlanPeriodTo.Month, 1);
                            while (!yebPlanPeriodFr.Month.Equals(payrollGroup.PayGroupYEBMonthFrom))
                                yebPlanPeriodFr = yebPlanPeriodFr.AddMonths(-1);

                            EEmpPersonalInfo empInfo = new EEmpPersonalInfo();
                            empInfo.EmpID = EmpID;
                            EEmpPersonalInfo.db.select(dbConn, empInfo);

                            DateTime eligibleDate = empInfo.EmpServiceDate;
                            DateTime lastProbationDate = empInfo.EmpProbaLastDate;
                            if (lastProbationDate.Ticks.Equals(0))
                                lastProbationDate = empInfo.EmpServiceDate.AddDays(-1);
                            if (yebPlan.YEBPlanEligiblePeriodIsExcludeMax3MonthsProbation)
                            {
                                DateTime tmpEstimatedEligibleDate = eligibleDate.AddMonths(3);
                                if (tmpEstimatedEligibleDate < lastProbationDate.AddDays(1))
                                    eligibleDate = tmpEstimatedEligibleDate;
                                else
                                    eligibleDate = lastProbationDate.AddDays(1);
                            }
                            if (yebPlan.YEBPlanEligiblePeriodIsCheckEveryYEBYear && eligibleDate < yebPlanPeriodFr)
                                eligibleDate = yebPlanPeriodFr;
                            if (yebPlan.YEBPlanEligibleUnit.Equals("M"))
                                eligibleDate = eligibleDate.AddMonths(yebPlan.YEBPlanEligiblePeriod);
                            else if (yebPlan.YEBPlanEligibleUnit.Equals("D"))
                                eligibleDate = eligibleDate.AddDays(yebPlan.YEBPlanEligiblePeriod);



                            if (yebPlan.YEBPlanIsEligibleAfterProbation)
                            {
                                if (lastProbationDate > eligibleDate)
                                {
                                    if (lastProbationDate.Equals(yebPlanPeriodTo)) // specially made eligible if probation-end-date == year-end-bonus-plan-end-date
                                        eligibleDate = lastProbationDate;
                                    else
                                        eligibleDate = lastProbationDate.AddDays(1);
                                }
                            }
                            if (empTerm != null)
                                // Cannot get YEB when resign date (date to submit resign letter) is before eligibleDate
                                if (empTerm.EmpTermLastDate < eligibleDate || empTerm.EmpTermResignDate < eligibleDate)
                                    //extend the eligible date so that this employee cannot get YEB
                                    eligibleDate = yebPlanPeriodTo.AddDays(1);

                            if (eligibleDate <= yebPlanPeriodTo)
                            {
                                double baseAmount = 0;
                                double multiplier = 0;
                                double divider = 0;
                                string baseAmountRemark = string.Empty;

                                baseAmount = GetYearEndBonusBaseAmount(EmpID, yebPlan, yebPlanPeriodFr, yebPlanPeriodTo, payrollPeriod, empInfo, empTerm, out baseAmountRemark);

                                if (yebPlan.YEBPlanProrataMethod.Equals("M"))
                                {
                                    if (yebPlan.YEBPlanPaymentBaseMethod.Equals(EYEBPlan.PAYMENT_BASE_AVERAGE_BASIC_SALARY2))
                                        multiplier = Utility.MonthDifference((empInfo.EmpDateOfJoin < yebPlanPeriodFr ? yebPlanPeriodFr : empInfo.EmpDateOfJoin), yebPlanPeriodTo);
                                    else
                                        multiplier = Math.Truncate(Utility.MonthDifference((empInfo.EmpServiceDate < yebPlanPeriodFr ? yebPlanPeriodFr : empInfo.EmpServiceDate), yebPlanPeriodTo));

                                    if (empTerm != null)
                                    {
                                        if (empTerm.EmpTermLastDate < yebPlanPeriodTo)
                                        {
                                            ECessationReason cessationReason = new ECessationReason();
                                            cessationReason.CessationReasonID = empTerm.CessationReasonID;
                                            if (ECessationReason.db.select(dbConn, cessationReason))
                                                if (cessationReason.CessationReasonHasProrataYEB && IsFinalPaymentTrialRun)
                                                    multiplier -= HROne.CommonLib.GenericRoundingFunctions.RoundingUp(Utility.MonthDifference(empTerm.EmpTermLastDate.AddDays(1), yebPlanPeriodTo), 0, 2);
                                                else
                                                    multiplier = 0;
                                        }
                                    }

                                    divider = Math.Truncate(Utility.MonthDifference(yebPlanPeriodFr, yebPlanPeriodTo));
                                }
                                else if (yebPlan.YEBPlanProrataMethod.Equals("D"))
                                {
                                    multiplier = ((TimeSpan)yebPlanPeriodTo.AddDays(1).Subtract((DateTime)(empInfo.EmpServiceDate < yebPlanPeriodFr ? yebPlanPeriodFr : empInfo.EmpServiceDate))).TotalDays;
                                    if (empTerm != null)
                                    {
                                        if (empTerm.EmpTermLastDate < yebPlanPeriodTo)
                                        {
                                            ECessationReason cessationReason = new ECessationReason();
                                            cessationReason.CessationReasonID = empTerm.CessationReasonID;
                                            if (ECessationReason.db.select(dbConn, cessationReason))
                                                if (cessationReason.CessationReasonHasProrataYEB && IsFinalPaymentTrialRun)
                                                    multiplier -= yebPlanPeriodTo.AddDays(1).Subtract(empTerm.EmpTermLastDate.AddDays(1)).TotalDays;
                                                else
                                                    multiplier = 0;
                                        }
                                    }

                                    divider = yebPlanPeriodTo.AddDays(1).Subtract(yebPlanPeriodFr).TotalDays;
                                }
                                if (multiplier > divider)
                                    multiplier = divider;
                                double totalAmount = (baseAmount * multiplier * yebPlan.YEBPlanMultiplier) / divider;

                                if (totalAmount > 0)
                                {
                                    //Create Payment Record
                                    EPaymentRecord paymentRecord = new EPaymentRecord();
                                    paymentRecord.CurrencyID = ExchangeCurrency.DefaultCurrency();
                                    paymentRecord.PaymentCodeID = yebPlan.YEBPlanPaymentCodeID;
                                    paymentRecord.PayRecCalAmount = totalAmount;
                                    paymentRecord.PayRecActAmount = HROne.CommonLib.GenericRoundingFunctions.RoundingTo(totalAmount, 2, 2);

                                    EEmpBankAccount empBank = EEmpBankAccount.GetDefaultBankAccount(dbConn, EmpID);
                                    if (empBank != null)
                                    {
                                        paymentRecord.PayRecMethod = "A";
                                        paymentRecord.EmpAccID = empBank.EmpBankAccountID;
                                    }
                                    else
                                    {
                                        paymentRecord.PayRecMethod = "Q";
                                    }

                                    paymentRecord.PayRecType = PaymentRecordType.PAYRECORDTYPE_YEB;
                                    paymentRecord.PayRecRemark = baseAmountRemark
                                        + (multiplier == divider ? "" :
                                          (divider.Equals(1) ? string.Empty : " / " + divider) + (multiplier.Equals(1) ? string.Empty : " x " + (Math.Truncate(multiplier * 100) / 100)))  // multipler: 7.367 displays as 7.36
                                        + (yebPlan.YEBPlanMultiplier.Equals(1) ? string.Empty : " x " + yebPlan.YEBPlanMultiplier);
                                    paymentRecord.PayRecStatus = PaymentRecordStatus.PAYRECORDSTATUS_ACTIVE;
                                    paymentRecords.Add(paymentRecord);
                                }
                            }

                        }
                    }
                }
            }
            return paymentRecords;
        }

        public double GetYearEndBonusBaseAmount(int EmpID, EYEBPlan yebPlan, DateTime yebPlanPeriodFr, DateTime yebPlanPeriodTo, EPayrollPeriod payrollPeriod, EEmpPersonalInfo empInfo, EEmpTermination empTerm, out string baseAmountRemark)
        {
            double baseAmount = 0;
            baseAmountRemark = string.Empty;
            if (yebPlan.YEBPlanPaymentBaseMethod.Equals(EYEBPlan.PAYMENT_BASE_MONTHLY_AVERAGE_WAGES))
            {
                //  Get start date of payroll cycle as As Of date
                AverageWages averageWages = new AverageWages(dbConn, EmpID, payrollPeriod.PayPeriodFr);
                baseAmount = averageWages.MonthlyWages();
                if (averageWages.TotalDays.Equals(0))
                {
                    // Get last Day of YEB Period as reference date for recurring payment
                    AND andRecurringPayment = new AND();
                    DBFilter paymentTypeFilter = new DBFilter();
                    paymentTypeFilter.add(new Match("PaymentTypeID", EPaymentType.SystemPaymentType.BasicSalaryPaymentType(dbConn).PaymentTypeID));
                    andRecurringPayment.add(new IN("PayCodeID", "Select PaymentCodeID from " + EPaymentCode.db.dbclass.tableName, paymentTypeFilter));
                    andRecurringPayment.add(new Match("EmpRPEffFr", "<=", yebPlanPeriodTo));
                    EEmpRecurringPayment empBasicSalaryRP = (EEmpRecurringPayment)AppUtils.GetLastObj(dbConn, EEmpRecurringPayment.db, "EmpRPEffFr", EmpID, andRecurringPayment);
                    if (empBasicSalaryRP != null)
                        baseAmount = empBasicSalaryRP.EmpRPAmount;
                }
            }
            else if (yebPlan.YEBPlanPaymentBaseMethod.Equals(EYEBPlan.PAYMENT_BASE_RECURRING_BASIC_SALARY))
            {
                // Get last Day of YEB Period as reference date for recurring payment
                AND andRecurringPayment = new AND();
                andRecurringPayment.add(new Match("PayCodeID", yebPlan.YEBPlanRPPaymentCodeID));
                if (empTerm != null)
                    andRecurringPayment.add(new Match("EmpRPEffFr", "<=", empTerm.EmpTermLastDate));
                else
                    andRecurringPayment.add(new Match("EmpRPEffFr", "<=", yebPlanPeriodTo));

                EEmpRecurringPayment empBasicSalaryRP = (EEmpRecurringPayment)AppUtils.GetLastObj(dbConn, EEmpRecurringPayment.db, "EmpRPEffFr", EmpID, andRecurringPayment);
                if (empBasicSalaryRP != null)
                {
                    if (ESystemParameter.IsEnabled(dbConn, ESystemParameter.PARAM_CODE_MONTHLY_ACHIEVEMENT_COMMISSION) && empBasicSalaryRP.EmpRPBasicSalary > 0)
                        baseAmount = empBasicSalaryRP.EmpRPBasicSalary;
                    else
                        baseAmount = empBasicSalaryRP.EmpRPAmount;
                }
            }
            else if (yebPlan.YEBPlanPaymentBaseMethod.Equals(EYEBPlan.PAYMENT_BASE_AVERAGE_BASIC_SALARY))
            {
                DateTime actualYEBPeriodFr = empInfo.EmpDateOfJoin < yebPlanPeriodFr ? yebPlanPeriodFr : empInfo.EmpServiceDate;
                DateTime actualYEBPeriodTo = yebPlanPeriodTo;

                double numOfMonth = Math.Truncate(Utility.MonthDifference(actualYEBPeriodFr, yebPlanPeriodTo));

                if (empTerm != null)
                    if (empTerm.EmpTermLastDate < actualYEBPeriodTo)
                    {
                        actualYEBPeriodTo = empTerm.EmpTermLastDate;
                        numOfMonth -= HROne.CommonLib.GenericRoundingFunctions.RoundingUp(Utility.MonthDifference(actualYEBPeriodTo.AddDays(1), yebPlanPeriodTo), 0, 2);
                    }
                // Get last Day of YEB Period as reference date for recurring payment
                DBFilter paymentTypeFilter = new DBFilter();
                paymentTypeFilter.add(new Match("PaymentTypeID", EPaymentType.SystemPaymentType.BasicSalaryPaymentType(dbConn).PaymentTypeID));

                DBFilter yebPayPeriodFilter = new DBFilter();
                //  Incomplete payroll month is not consider
                yebPayPeriodFilter.add(new Match("PayPeriodFr", ">=", actualYEBPeriodFr));
                // Start 000163, Ricky So, 2015/02/26
                if (actualYEBPeriodTo == Utility.LastDateOfMonth(actualYEBPeriodTo))
                    yebPayPeriodFilter.add(new Match("PayPeriodTo", "<=", Utility.LastDateOfMonth(actualYEBPeriodTo.AddMonths(-1))));
                else
                    yebPayPeriodFilter.add(new Match("PayPeriodTo", "<=", actualYEBPeriodTo.AddMonths(-1)));

                // yebPayPeriodFilter.add(new Match("PayPeriodTo", "<=", actualYEBPeriodTo));
                // End 000163, Ricky So, 2015/02/26

                DBFilter empPayrollFilter = new DBFilter();
                empPayrollFilter.add(new Match("EmpID", empInfo.EmpID));
                empPayrollFilter.add(new IN("PayPeriodID", "Select PayPeriodID from " + EPayrollPeriod.db.dbclass.tableName, yebPayPeriodFilter));


                //ArrayList empPayrollList = EEmpPayroll.db.select(dbConn, empPayrollFilter);
                //foreach (EEmpPayroll empPayroll in empPayrollList)
                //{
                //    EPayrollPeriod payPeriod = new EPayrollPeriod();
                //    payPeriod.PayPeriodID = empPayroll.PayPeriodID;
                //    if (EPayrollPeriod.db.select(dbConn, payPeriod))
                //    {
                //        //if (pay
                //    }
                //}


                DBFilter paymentRecordFilter = new DBFilter();
                paymentRecordFilter.add(new IN("EmpPayrollID", "Select EmpPayrollID from " + EEmpPayroll.db.dbclass.tableName, empPayrollFilter));
                paymentRecordFilter.add(new IN("PaymentCodeID", "Select PaymentCodeID from " + EPaymentCode.db.dbclass.tableName, paymentTypeFilter));

                ArrayList paymentRecordList = EPaymentRecord.db.select(dbConn, paymentRecordFilter);
                double totalBasicSalaryAmount = 0;
                foreach (EPaymentRecord payRecord in paymentRecordList)
                {
                    totalBasicSalaryAmount += payRecord.PayRecActAmount;
                }

                if (payrollPeriod.PayPeriodTo.Equals(actualYEBPeriodTo))
                {
                    AND andRecurringPayment = new AND();
                    andRecurringPayment.add(new IN("PayCodeID", "Select PaymentCodeID from " + EPaymentCode.db.dbclass.tableName, paymentTypeFilter));
                    if (empTerm != null)
                        andRecurringPayment.add(new Match("EmpRPEffFr", "<=", empTerm.EmpTermLastDate));
                    else
                        andRecurringPayment.add(new Match("EmpRPEffFr", "<=", yebPlanPeriodTo));

                    EEmpRecurringPayment empBasicSalaryRP = (EEmpRecurringPayment)AppUtils.GetLastObj(dbConn, EEmpRecurringPayment.db, "EmpRPEffFr", EmpID, andRecurringPayment);
                    if (empBasicSalaryRP != null)
                        totalBasicSalaryAmount += empBasicSalaryRP.EmpRPAmount;
                }
                baseAmount = HROne.CommonLib.GenericRoundingFunctions.RoundingTo(totalBasicSalaryAmount / numOfMonth, 2, 2);
                baseAmountRemark = totalBasicSalaryAmount.ToString("$#,##0.00") + (numOfMonth.Equals(1) ? string.Empty : " / " + numOfMonth);

            }
            else if (yebPlan.YEBPlanPaymentBaseMethod.Equals(EYEBPlan.PAYMENT_BASE_AVERAGE_BASIC_SALARY2))
            {
                // for Average Basic Salary2 only, calculate from JoinDate if joing from this year
                DateTime actualYEBPeriodFr = empInfo.EmpDateOfJoin < yebPlanPeriodFr ? yebPlanPeriodFr : empInfo.EmpDateOfJoin;
                DateTime actualYEBPeriodTo = yebPlanPeriodTo;

                // double numOfMonth = Math.Truncate(Utility.MonthDifference(actualYEBPeriodFr, yebPlanPeriodTo));
                double numOfMonth = Utility.MonthDifference(actualYEBPeriodFr, yebPlanPeriodTo);

                if (empTerm != null)
                    if (empTerm.EmpTermLastDate < actualYEBPeriodTo)
                    {
                        actualYEBPeriodTo = empTerm.EmpTermLastDate;
                        numOfMonth -= Utility.MonthDifference(actualYEBPeriodTo.AddDays(1), yebPlanPeriodTo);
                    }
                // Get last Day of YEB Period as reference date for recurring payment
                DBFilter paymentTypeFilter = new DBFilter();
                paymentTypeFilter.add(new Match("PaymentTypeID", EPaymentType.SystemPaymentType.BasicSalaryPaymentType(dbConn).PaymentTypeID));

                DBFilter yebPayPeriodFilter = new DBFilter();
                // include Incomplete Payroll Month 
                //yebPayPeriodFilter.add(new Match("PayPeriodFr", ">=", actualYEBPeriodFr));
                //yebPayPeriodFilter.add(new Match("PayPeriodTo", "<=", actualYEBPeriodTo));
                yebPayPeriodFilter.add(new Match("PayPeriodFr", ">=", Utility.FirstDateOfMonth(actualYEBPeriodFr)));

                // Start 000163, Ricky So, 2015/02/26
                yebPayPeriodFilter.add(new Match("PayPeriodTo", "<=", Utility.LastDateOfMonth(actualYEBPeriodTo.AddMonths(-1))));
                // yebPayPeriodFilter.add(new Match("PayPeriodTo", "<=", Utility.LastDateOfMonth(actualYEBPeriodTo)));
                // End 000163, Ricky So, 2015/02/26


                DBFilter empPayrollFilter = new DBFilter();
                empPayrollFilter.add(new Match("EmpID", empInfo.EmpID));
                empPayrollFilter.add(new IN("PayPeriodID", "Select PayPeriodID from " + EPayrollPeriod.db.dbclass.tableName, yebPayPeriodFilter));

                DBFilter paymentRecordFilter = new DBFilter();
                paymentRecordFilter.add(new IN("EmpPayrollID", "Select EmpPayrollID from " + EEmpPayroll.db.dbclass.tableName, empPayrollFilter));
                paymentRecordFilter.add(new IN("PaymentCodeID", "Select PaymentCodeID from " + EPaymentCode.db.dbclass.tableName, paymentTypeFilter));

                ArrayList paymentRecordList = EPaymentRecord.db.select(dbConn, paymentRecordFilter);
                double totalBasicSalaryAmount = 0;
                foreach (EPaymentRecord payRecord in paymentRecordList)
                {
                    totalBasicSalaryAmount += payRecord.PayRecActAmount;
                }

                if (payrollPeriod.PayPeriodTo.Equals(actualYEBPeriodTo))
                {
                    AND andRecurringPayment = new AND();
                    andRecurringPayment.add(new IN("PayCodeID", "Select PaymentCodeID from " + EPaymentCode.db.dbclass.tableName, paymentTypeFilter));
                    if (empTerm != null)
                        andRecurringPayment.add(new Match("EmpRPEffFr", "<=", empTerm.EmpTermLastDate));
                    else
                        andRecurringPayment.add(new Match("EmpRPEffFr", "<=", yebPlanPeriodTo));

                    EEmpRecurringPayment empBasicSalaryRP = (EEmpRecurringPayment)AppUtils.GetLastObj(dbConn, EEmpRecurringPayment.db, "EmpRPEffFr", EmpID, andRecurringPayment);
                    if (empBasicSalaryRP != null)
                        totalBasicSalaryAmount += empBasicSalaryRP.EmpRPAmount;
                }
                baseAmount = HROne.CommonLib.GenericRoundingFunctions.RoundingTo(totalBasicSalaryAmount / numOfMonth, 2, 2);
                baseAmountRemark = totalBasicSalaryAmount.ToString("$#,##0.00") + (numOfMonth.Equals(1) ? string.Empty : " / " + (Math.Truncate(numOfMonth * 100) / 100));

            }
            if (string.IsNullOrEmpty(baseAmountRemark))
                baseAmountRemark = baseAmount.ToString("$#,##0.00");

            return baseAmount;
        }
        public ArrayList FinalPaymentTrialRun(int EmpID)
        {
            ArrayList finalPayments = new ArrayList();
            DBFilter filter = new DBFilter();
            filter.add(new Match("EmpID", EmpID));

            ArrayList arrayList = EEmpFinalPayment.db.select(dbConn, filter);

            foreach (EEmpFinalPayment finalPayment in arrayList)
            {
                EPaymentRecord payRecord = new EPaymentRecord();
                payRecord.CurrencyID = finalPayment.CurrencyID;

                payRecord.PayRecMethod = finalPayment.EmpFinalPayMethod;
                payRecord.EmpAccID = finalPayment.EmpAccID;
                payRecord.LeaveAppID = finalPayment.LeaveAppID;
                payRecord.LeaveAppIDList = finalPayment.LeaveAppIDList;
                payRecord.PaymentCodeID = finalPayment.PayCodeID;
                payRecord.PayRecCalAmount = finalPayment.EmpFinalPayAmount;
                payRecord.PayRecActAmount = finalPayment.EmpFinalPayAmount;
                payRecord.CostCenterID = finalPayment.CostCenterID;
                payRecord.PayRecNumOfDayAdj = finalPayment.EmpFinalPayNumOfDayAdj;
                payRecord.PayRecRemark = finalPayment.EmpFinalPayRemark;
                payRecord.PayRecStatus = PaymentRecordStatus.PAYRECORDSTATUS_ACTIVE;
                payRecord.PayRecType = PaymentRecordType.PAYRECORDTYPE_FINALPAYMENT;
                payRecord.PayRecIsRestDayPayment = finalPayment.EmpFinalPayIsRestDayPayment;
                payRecord.RelatedObject = finalPayment;
                finalPayments.Add(payRecord);
            }
            return finalPayments;
        }

        private ArrayList LeavePaymentTrialRun(int EmpID, EPayrollPeriod payrollPeriod, ArrayList previousPaymentRecordResult, DateTime PayrollGroupDateFrom, DateTime PayrollGroupDateTo)
        {
            return LeavePaymentTrialRun(EmpID, payrollPeriod, previousPaymentRecordResult, PayrollGroupDateFrom, PayrollGroupDateTo, false);
        }
        protected virtual ArrayList LeavePaymentTrialRun(int EmpID, EPayrollPeriod payrollPeriod, ArrayList previousPaymentRecordResult, DateTime PayrollGroupDateFrom, DateTime PayrollGroupDateTo, bool UseDeductionFormulaOnly)
        {
            PaymentBreakDownCollection deductionPaymentBreakDownList = new PaymentBreakDownCollection();
            PaymentBreakDownCollection allowancePaymentBreakDownList = new PaymentBreakDownCollection();
            System.Collections.Generic.List<EPaymentRecord> dummyLeavePayments = new System.Collections.Generic.List<EPaymentRecord>();
            //ArrayList leavePayments = new ArrayList();
            DBFilter filter = new DBFilter();
            filter.add(new Match("EmpID", EmpID));

            EEmpTermination empTerm = EEmpTermination.GetObjectByEmpID(dbConn, EmpID);
            if (empTerm != null)
                if (empTerm.EmpTermLastDate <= PayrollGroupDateTo && empTerm.EmpTermLastDate >= PayrollGroupDateFrom)
                    filter.add(new Match("LeaveAppDateTo", "<=", empTerm.EmpTermLastDate));
                else
                    empTerm = null;
            if (empTerm == null)
                filter.add(new Match("LeaveAppDateTo", "<=", payrollPeriod.PayPeriodLeaveCutOffDate < PayrollGroupDateTo ? payrollPeriod.PayPeriodLeaveCutOffDate : PayrollGroupDateTo));

            filter.add(new Match("LeaveAppNoPayProcess", false));
            OR orPayRecFilter = new OR();
            orPayRecFilter.add(new Match("EmpPayrollID", "=", 0));
            orPayRecFilter.add(new NullTerm("EmpPayrollID"));
            filter.add(orPayRecFilter);

            //filter.add("LeaveCodeID", true);
            filter.add("LeaveAppDateFrom", true);
            ArrayList leaveApplications = ELeaveApplication.db.select(dbConn, filter);

            //ArrayList lastDeductionLeaveAppList = new ArrayList();
            //int lastDeductionPaymentCodeID = 0;
            //double lastDeductionRate = 0;
            //double lastDeductionUnit = 0;
            //double lastDeductionRatio = 0;
            //string lastDeductionFormulaRemark = string.Empty;

            //ArrayList lastAllowanceLeaveAppList = new ArrayList();
            //int lastAllowancePaymentCodeID = 0;
            //double lastAllowanceRate = 0;
            //double lastAllowanceUnit = 0;
            //double lastAllowanceRatio = 0;
            //string lastAllowanceFormulaRemark = string.Empty;


            foreach (ELeaveApplication leaveApplication in leaveApplications)
            {

                ELeaveCode leaveCode = new ELeaveCode();
                leaveCode.LeaveCodeID = leaveApplication.LeaveCodeID;
                if (ELeaveCode.db.select(dbConn, leaveCode))
                {
                    if (leaveCode.LeaveCodeIsPayrollProcessNextMonth && empTerm == null)
                    {
                        DateTime tmpCutOffDate = payrollPeriod.PayPeriodLeaveCutOffDate.AddDays(1).AddMonths(-1).AddDays(-1);
                        if (leaveApplication.LeaveAppDateTo > tmpCutOffDate)
                            continue;
                    }

                    if (!leaveCode.LeaveCodeIsSkipPayrollProcess)
                    {

                        DBFilter positionFilter = new DBFilter();
                        positionFilter.add(new Match("EmpID", EmpID));
                        positionFilter.add(new Match("EmpPosEffFr", "<=", leaveApplication.LeaveAppDateFrom));

                        OR orFilter = new OR();
                        orFilter.add(new Match("EmpPosEffTo", ">=", leaveApplication.LeaveAppDateFrom));
                        orFilter.add(new NullTerm("EmpPosEffTo"));
                        positionFilter.add(orFilter);

                        positionFilter.add("EmpPosEffFr", true);

                        ArrayList positionInfos = EEmpPositionInfo.db.select(dbConn, positionFilter);

                        EPayrollGroup leavePayGroup;

                        if (positionInfos.Count > 0)
                        {
                            EEmpPositionInfo positionInfo = (EEmpPositionInfo)positionInfos[0];

                            leavePayGroup = new EPayrollGroup();
                            leavePayGroup.PayGroupID = positionInfo.PayGroupID;
                            EPayrollGroup.db.select(dbConn, leavePayGroup);
                        }
                        else
                        {
                            leavePayGroup = new EPayrollGroup();
                            leavePayGroup.PayGroupID = payrollPeriod.PayGroupID;
                            EPayrollGroup.db.select(dbConn, leavePayGroup);
                        }

                        EPayrollPeriod leavePayPeriod;
                        if (leaveApplication.LeaveAppDateFrom >= payrollPeriod.PayPeriodFr && leaveApplication.LeaveAppDateTo <= payrollPeriod.PayPeriodTo)
                            leavePayPeriod = payrollPeriod;
                        else
                            leavePayPeriod = GenerateDummyPayrollPeriod(dbConn, leavePayGroup.PayGroupID, leaveApplication.LeaveAppDateFrom);

                        double totalRPAmount = GetTotalPeriodPayRecurringPayment(dbConn, EmpID, leaveApplication.LeaveAppDateFrom, false, PayrollProcess.PaymentCodeProrataOptionEnum.PaymentCodeProrataAny, PayrollProcess.PaymentCodeProrataOptionEnum.PaymentCodeProrataProrataOnly, PaymentCodeProrataOptionEnum.PaymentCodeProrataAny);

                        if (leavePayGroup.PayGroupIsCNDProrata && leaveCode.LeaveCodeIsCNDProrata)
                        {
                            double totalCNDAmount = GetTotalPeriodPayClaimsAndDeductions(dbConn, EmpID, leaveApplication.LeaveAppDateFrom, leavePayPeriod, false, PayrollProcess.PaymentCodeProrataOptionEnum.PaymentCodeProrataAny, PayrollProcess.PaymentCodeProrataOptionEnum.PaymentCodeProrataProrataOnly, PaymentCodeProrataOptionEnum.PaymentCodeProrataAny);
                            totalRPAmount += totalCNDAmount;
                        }
                        //ELeaveCode leaveCode = new ELeaveCode();
                        //leaveCode.LeaveCodeID = leaveApplication.LeaveCodeID;
                        //ELeaveCode.db.select(dbConn, leaveCode);


                        //if (leaveCode != null)
                        {
                            //  Calculate Deduction and Allowance Amount
                            string allowanceFormulaRemark;
                            string deductionFormulaRemark;
                            bool IsDAW = false;

                            int leaveAllowFormulaID = leaveCode.LeaveCodeLeaveAllowFormula;
                            int leaveDeductFormulaID = leaveCode.LeaveCodeLeaveDeductFormula;
                            int leaveAllowPaymentCodeID = leaveCode.LeaveCodeLeaveAllowPaymentCodeID;
                            int leaveDeductPaymentCodeID = leaveCode.LeaveCodeLeaveDeductPaymentCodeID;

                            DBFilter leaveOverrideFilter = new DBFilter();
                            leaveOverrideFilter.add(new Match("PayGroupID", leavePayGroup.PayGroupID));
                            leaveOverrideFilter.add(new Match("LeaveCodeID", leaveCode.LeaveCodeID));

                            ArrayList leaveOverrideList = EPayrollGroupLeaveCodeSetupOverride.db.select(dbConn, leaveOverrideFilter);
                            if (leaveOverrideList.Count > 0)
                            {
                                EPayrollGroupLeaveCodeSetupOverride leaveCodeOverride = (EPayrollGroupLeaveCodeSetupOverride)leaveOverrideList[0];
                                if (leaveCodeOverride.PayrollGroupLeaveCodeSetupLeaveAllowFormula > 0)
                                    leaveAllowFormulaID = leaveCodeOverride.PayrollGroupLeaveCodeSetupLeaveAllowFormula;
                                if (leaveCodeOverride.PayrollGroupLeaveCodeSetupLeaveDeductFormula > 0)
                                    leaveDeductFormulaID = leaveCodeOverride.PayrollGroupLeaveCodeSetupLeaveDeductFormula;
                                if (leaveCodeOverride.PayrollGroupLeaveCodeSetupLeaveAllowPaymentCodeID > 0)
                                    leaveAllowPaymentCodeID = leaveCodeOverride.PayrollGroupLeaveCodeSetupLeaveAllowPaymentCodeID;
                                if (leaveCodeOverride.PayrollGroupLeaveCodeSetupLeaveDeductPaymentCodeID > 0)
                                    leaveDeductPaymentCodeID = leaveCodeOverride.PayrollGroupLeaveCodeSetupLeaveDeductPaymentCodeID;
                            }
                            double dailyAllowAmount = PayrollFormula.DailyProrataCaluclation(dbConn, leaveAllowFormulaID, leavePayGroup.PayGroupDefaultProrataFormula, EmpID, totalRPAmount, leavePayPeriod.PayPeriodFr, leavePayPeriod.PayPeriodTo, leavePayGroup.NumOfPeriodPerYear(), leaveApplication.LeaveAppDateFrom, leaveApplication.LeaveCodeID, out allowanceFormulaRemark, out IsDAW);
                            if (!IsDAW)
                            {
                                double totalDailyPayment = PayrollProcess.GetTotalDailyPayRecurringPayment(dbConn, EmpID, leaveApplication.LeaveAppDateFrom, true, leavePayPeriod);
                                if (dailyAllowAmount.Equals(0))
                                {
                                    dailyAllowAmount = totalDailyPayment;
                                    allowanceFormulaRemark = totalDailyPayment.ToString("$#,##0.00##");
                                }
                                else if (!totalDailyPayment.Equals(0))
                                {
                                    dailyAllowAmount += totalDailyPayment;
                                    allowanceFormulaRemark += " + " + totalDailyPayment.ToString("$#,##0.00##");
                                }
                                if (dailyAllowAmount == 0)
                                    //  HOURLY NOT SUPPORT
                                    dailyAllowAmount = 0;// PayrollProcess.GetTotalHourlyPayRecurringPayment(EmpID, leaveApplication.LeaveAppDateFrom);
                            }
                            double dailyDeductAmount = PayrollFormula.DailyProrataCaluclation(dbConn, leaveDeductFormulaID, leavePayGroup.PayGroupDefaultProrataFormula, EmpID, totalRPAmount, leavePayPeriod.PayPeriodFr, leavePayPeriod.PayPeriodTo, leavePayGroup.NumOfPeriodPerYear(), leaveApplication.LeaveAppDateFrom, leaveApplication.LeaveCodeID, out deductionFormulaRemark, out IsDAW);

                            //  For EAO 
                            if (!leavePayGroup.PayGroupPayAdvanceCompareTotalPaymentOnly)
                            {
                                if (leavePayGroup.PayGroupPayAdvance && leaveCode.LeaveCodePayAdvance)
                                {
                                    if (dailyDeductAmount > dailyAllowAmount)
                                    {
                                        dailyAllowAmount = dailyDeductAmount;
                                        allowanceFormulaRemark = deductionFormulaRemark;
                                    }
                                }
                            }
                            else
                            {
                                if (UseDeductionFormulaOnly)
                                {
                                    dailyAllowAmount = dailyDeductAmount;
                                    allowanceFormulaRemark = deductionFormulaRemark;
                                }
                            }

                            // Get Payment Code for Allowance and Deduction
                            ////ArrayList leaveAllowPaymentCodes = GetPaymentCodeByPaymentType("LEAVEALLOW");
                            EPaymentCode leaveAllowPaymentCode = null;
                            if (leaveCode.LeaveCodePayRatio == 1)
                                leaveAllowPaymentCode = GetLeaveAllowanceCode(true, leaveAllowPaymentCodeID);
                            else if (!leaveCode.LeaveCodePayRatio.Equals(0))
                                leaveAllowPaymentCode = GetLeaveAllowanceCode(false, leaveAllowPaymentCodeID);
                            EPaymentCode leaveDeductPaymentCode = GetLeaveDeductionCode(true, leaveDeductPaymentCodeID);

                            bool isLeaveAppLinkWithPaymentRecord = false;
                            // Skip generate Leave Payment if generated item may be same
                            if (leaveCode.LeaveCodePayRatio.Equals(1.0)
                                && Math.Abs(dailyDeductAmount - dailyAllowAmount) < 0.01
                                && leaveAllowPaymentCode.PaymentCodeIsMPF == leaveDeductPaymentCode.PaymentCodeIsMPF
                                && leaveAllowPaymentCode.PaymentCodeIsWages == leaveDeductPaymentCode.PaymentCodeIsWages
                                && leaveCode.LeaveCodeUseAllowancePaymentCodeIfSameAmount)
                            {

                                // (Obsolate because rest day payment should be excluded for calculateing SMW)
                                // Generate Dummy Payment for locking Leave Payment
                                // Only empty remark for payslip
                                ArrayList leaveAllowPayments = GetDeductablePaymentRecord(dbConn, EmpID, previousPaymentRecordResult, dailyAllowAmount * leaveApplication.LeaveAppDays * leaveCode.LeaveCodePayRatio, leaveAllowPaymentCode.PaymentCodeID);
                                foreach (EPaymentRecord leaveAllowPayment in leaveAllowPayments)
                                {
                                    ////  Only set the first payment record with daily adjustment
                                    //if (leaveAllowPayment == leaveAllowPayments[0])
                                    //    leaveAllowPayment.PayRecNumOfDayAdj = leaveApplication.LeaveAppDays;
                                    //leaveAllowPayment.LeaveAppID = leaveApplication.LeaveAppID;
                                    //leaveAllowPayment.PayRecIsRestDayPayment = true;
                                    ////leaveAllowPayment.PayRecRemark = "$0"
                                    ////    + (leaveApplication.LeaveAppDays.Equals(1) ? string.Empty : " x " + leaveApplication.LeaveAppDays)
                                    ////    + (leaveCode.LeaveCodePayRatio.Equals(1) ? string.Empty : " x " + leaveCode.LeaveCodePayRatio);
                                    //leaveAllowPayment.RelatedObject = leaveApplication;

                                    PaymentBreakDownKey key = new PaymentBreakDownKey(leaveAllowPayment.PaymentCodeID, dailyAllowAmount, allowanceFormulaRemark, leaveCode.LeaveCodePayRatio, leaveAllowPayment.PayRecMethod, leaveAllowPayment.EmpAccID, true, leaveAllowPayment.CostCenterID, true);
                                    //  Only set the first payment record with daily adjustment
                                    if (leaveAllowPayment == leaveAllowPayments[0])
                                        allowancePaymentBreakDownList.AddUnit(key, leaveApplication.LeaveAppDays, leaveApplication.LeaveAppDays, leaveApplication);
                                    else
                                        allowancePaymentBreakDownList.AddUnit(key, leaveApplication.LeaveAppDays, 0, leaveApplication);
                                    isLeaveAppLinkWithPaymentRecord = true;
                                }
                                ////leavePayments.AddRange(leaveAllowPayments);
                                ArrayList tmpPaymentRecords = (ArrayList)previousPaymentRecordResult.Clone();
                                tmpPaymentRecords.AddRange(allowancePaymentBreakDownList.GeneratePaymentRecordList());

                                ArrayList leaveDeductPayments = GetDeductablePaymentRecord(dbConn, EmpID, tmpPaymentRecords, -dailyDeductAmount * leaveApplication.LeaveAppDays * 1, leaveAllowPaymentCode.PaymentCodeID);
                                foreach (EPaymentRecord leaveDeductPayment in leaveDeductPayments)
                                {
                                    ////  Only set the first payment record with daily adjustment
                                    //if (leaveDeductPayment == leaveDeductPayments[0])
                                    //    leaveDeductPayment.PayRecNumOfDayAdj = -leaveApplication.LeaveAppDays;
                                    //leaveDeductPayment.LeaveAppID = leaveApplication.LeaveAppID;
                                    ////leaveAllowPayment.PayRecRemark = "$0"
                                    ////    + (leaveApplication.LeaveAppDays.Equals(1) ? string.Empty : " x " + leaveApplication.LeaveAppDays)
                                    ////    + (leaveCode.LeaveCodePayRatio.Equals(1) ? string.Empty : " x " + leaveCode.LeaveCodePayRatio);
                                    //leaveDeductPayment.RelatedObject = leaveApplication;
                                    PaymentBreakDownKey key = new PaymentBreakDownKey(leaveDeductPayment.PaymentCodeID, -dailyDeductAmount, "-" + deductionFormulaRemark, 1, leaveDeductPayment.PayRecMethod, leaveDeductPayment.EmpAccID, false, leaveDeductPayment.CostCenterID, true);
                                    //  Only set the first payment record with daily adjustment
                                    if (leaveDeductPayment == leaveDeductPayments[0])
                                        deductionPaymentBreakDownList.AddUnit(key, leaveApplication.LeaveAppDays, -leaveApplication.LeaveAppDays, leaveApplication);
                                    else
                                        deductionPaymentBreakDownList.AddUnit(key, leaveApplication.LeaveAppDays, 0, leaveApplication);

                                    isLeaveAppLinkWithPaymentRecord = true;
                                }
                                //leavePayments.AddRange(leaveDeductPayments);
                            }
                            else
                            {
                                //  Generate Leave Allowance Payment Record
                                if (leaveAllowPaymentCode != null)
                                {

                                    //if (lastAllowancePaymentCodeID != leaveAllowPaymentCode.PaymentCodeID || Math.Abs(lastAllowanceRate - dailyAllowAmount) >= 0.01 || Math.Abs(lastAllowanceRatio - leaveCode.LeaveCodePayRatio) >= 0.01 || lastAllowanceFormulaRemark != allowanceFormulaRemark)
                                    //{
                                    //    if (lastAllowancePaymentCodeID > 0)
                                    //    {
                                    ArrayList leaveAllowPayments = GetDeductablePaymentRecord(dbConn, EmpID, previousPaymentRecordResult, dailyAllowAmount * leaveApplication.LeaveAppDays * leaveCode.LeaveCodePayRatio, leaveAllowPaymentCode.PaymentCodeID);
                                    if (leaveAllowPayments != null)
                                    {

                                        foreach (EPaymentRecord leaveAllowPayment in leaveAllowPayments)
                                        {
                                            ////  Only set the first payment record with daily adjustment
                                            //if (leaveAllowPayment == leaveAllowPayments[0])
                                            //    leaveAllowPayment.PayRecNumOfDayAdj = lastAllowanceUnit;
                                            //leaveAllowPayment.LeaveAppID = leaveApplication.LeaveAppID;
                                            //leaveAllowPayment.PayRecRemark = lastAllowanceFormulaRemark
                                            //    + (lastAllowanceUnit.Equals(1) ? string.Empty : " x " + lastAllowanceUnit)
                                            //    + (lastAllowanceRatio.Equals(1) ? string.Empty : " x " + lastAllowanceRatio);
                                            //leaveAllowPayment.RelatedObject = lastAllowanceLeaveAppList;
                                            //leaveAllowPayment.PayRecIsRestDayPayment = true;

                                            PaymentBreakDownKey key = new PaymentBreakDownKey(leaveAllowPayment.PaymentCodeID, dailyAllowAmount, allowanceFormulaRemark, leaveCode.LeaveCodePayRatio, leaveAllowPayment.PayRecMethod, leaveAllowPayment.EmpAccID, true, leaveAllowPayment.CostCenterID, true);
                                            //  Only set the first payment record with daily adjustment
                                            if (leaveAllowPayment == leaveAllowPayments[0])
                                                allowancePaymentBreakDownList.AddUnit(key, leaveApplication.LeaveAppDays, leaveApplication.LeaveAppDays, leaveApplication);
                                            else
                                                allowancePaymentBreakDownList.AddUnit(key, leaveApplication.LeaveAppDays, 0, leaveApplication);
                                        }
                                        //leavePayments.AddRange(leaveAllowPayments);
                                    }
                                    //    }
                                    //    lastAllowancePaymentCodeID = leaveAllowPaymentCode.PaymentCodeID;
                                    //    lastAllowanceRate = dailyAllowAmount;
                                    //    lastAllowanceRatio = leaveCode.LeaveCodePayRatio;
                                    //    lastAllowanceFormulaRemark = allowanceFormulaRemark;
                                    //    lastAllowanceUnit = 0;
                                    //    lastAllowanceLeaveAppList = new ArrayList();
                                    //}
                                    //lastAllowanceUnit += leaveApplication.LeaveAppDays;
                                    //lastAllowanceLeaveAppList.Add(leaveApplication);
                                    isLeaveAppLinkWithPaymentRecord = true;
                                    //ArrayList leaveAllowPayments = GetDeductablePaymentRecord(dbConn, EmpID, paymentRecords, dailyAllowAmount * leaveApplication.LeaveAppDays * leaveCode.LeaveCodePayRatio, leaveAllowPaymentCode.PaymentCodeID);
                                    //if (leaveAllowPayments != null)
                                    //{

                                    //    foreach (EPaymentRecord leaveAllowPayment in leaveAllowPayments)
                                    //    {
                                    //        //  Only set the first payment record with daily adjustment
                                    //        if (leaveAllowPayment == leaveAllowPayments[0])
                                    //            leaveAllowPayment.PayRecNumOfDayAdj = leaveApplication.LeaveAppDays;
                                    //        leaveAllowPayment.LeaveAppID = leaveApplication.LeaveAppID;
                                    //        leaveAllowPayment.PayRecRemark = allowanceFormulaRemark + " x " + leaveApplication.LeaveAppDays + " x " + leaveCode.LeaveCodePayRatio;
                                    //        leaveAllowPayment.RelatedObject = leaveApplication;
                                    //    }
                                    //    leavePayments.AddRange(leaveAllowPayments);
                                    //}
                                }
                                //  Generate Leave Deduction Payment Record

                                if (leaveDeductPaymentCode != null && dailyDeductAmount != 0)
                                {
                                    //if (lastDeductionPaymentCodeID != leaveDeductPaymentCode.PaymentCodeID || Math.Abs(lastDeductionRate - dailyDeductAmount) >= 0.01 || lastDeductionFormulaRemark != deductionFormulaRemark)
                                    //{
                                    //    if (lastDeductionPaymentCodeID > 0)
                                    //    {
                                    ArrayList tmpPaymentRecords = (ArrayList)previousPaymentRecordResult.Clone();
                                    tmpPaymentRecords.AddRange(allowancePaymentBreakDownList.GeneratePaymentRecordList());

                                    ArrayList leaveDeductPayments = GetDeductablePaymentRecord(dbConn, EmpID, tmpPaymentRecords, -dailyDeductAmount * leaveApplication.LeaveAppDays * 1, leaveDeductPaymentCode.PaymentCodeID);
                                    if (leaveDeductPayments != null)
                                    {

                                        foreach (EPaymentRecord leaveDeductPayment in leaveDeductPayments)
                                        {
                                            ////  Only set the first payment record with daily adjustment
                                            //if (leaveDeductPayment == leaveDeductPayments[0])
                                            //    leaveDeductPayment.PayRecNumOfDayAdj = -lastDeductionUnit;
                                            //leaveDeductPayment.LeaveAppID = leaveApplication.LeaveAppID;
                                            //leaveDeductPayment.PayRecRemark = "-" + lastDeductionFormulaRemark
                                            //    + (lastDeductionUnit.Equals(1) ? string.Empty : " x " + lastDeductionUnit);
                                            //// +" x " + lastDeductionRatio;
                                            //leaveDeductPayment.RelatedObject = lastDeductionLeaveAppList;

                                            PaymentBreakDownKey key = new PaymentBreakDownKey(leaveDeductPayment.PaymentCodeID, -dailyDeductAmount, "-" + deductionFormulaRemark, 1, leaveDeductPayment.PayRecMethod, leaveDeductPayment.EmpAccID, false, leaveDeductPayment.CostCenterID, true);
                                            //  Only set the first payment record with daily adjustment
                                            if (leaveDeductPayment == leaveDeductPayments[0])
                                                deductionPaymentBreakDownList.AddUnit(key, leaveApplication.LeaveAppDays, -leaveApplication.LeaveAppDays, leaveApplication);
                                            else
                                                deductionPaymentBreakDownList.AddUnit(key, leaveApplication.LeaveAppDays, 0, leaveApplication);

                                        }
                                        //leavePayments.AddRange(leaveDeductPayments);
                                    }
                                    //    }
                                    //    lastDeductionPaymentCodeID = leaveDeductPaymentCode.PaymentCodeID;
                                    //    lastDeductionRate = dailyDeductAmount;
                                    //    lastDeductionRatio = 1;
                                    //    lastDeductionFormulaRemark = deductionFormulaRemark;

                                    //    lastDeductionUnit = 0;
                                    //    lastDeductionLeaveAppList = new ArrayList();
                                    //}
                                    //lastDeductionUnit += leaveApplication.LeaveAppDays;
                                    //lastDeductionLeaveAppList.Add(leaveApplication);
                                    //isLeaveAppLinkWithPaymentRecord = true;

                                    //ArrayList leaveDeductPayments = GetDeductablePaymentRecord(dbConn, EmpID, paymentRecords, -dailyDeductAmount * leaveApplication.LeaveAppDays, leaveDeductPaymentCode.PaymentCodeID);
                                    //if (leaveDeductPayments != null)
                                    //{
                                    //    foreach (EPaymentRecord leaveDeductPayment in leaveDeductPayments)
                                    //    {
                                    //        if (leaveDeductPayment == leaveDeductPayments[0])
                                    //            leaveDeductPayment.PayRecNumOfDayAdj = -leaveApplication.LeaveAppDays;
                                    //        leaveDeductPayment.LeaveAppID = leaveApplication.LeaveAppID;
                                    //        leaveDeductPayment.PayRecRemark = "-" + deductionFormulaRemark + " x " + leaveApplication.LeaveAppDays;
                                    //        leaveDeductPayment.RelatedObject = leaveApplication;
                                    //    }

                                    //    leavePayments.AddRange(leaveDeductPayments);
                                    //}
                                }
                            }
                            //  Create Dummy Payment for marking leave without any payment item generated
                            if (!isLeaveAppLinkWithPaymentRecord)
                            {
                                EPaymentRecord dummyLeavePayment = new EPaymentRecord();
                                dummyLeavePayment.PaymentCodeID = 0;
                                dummyLeavePayment.CurrencyID = ExchangeCurrency.DefaultCurrency();
                                dummyLeavePayment.PayRecMethod = "O";
                                dummyLeavePayment.RelatedObject = leaveApplication;
                                dummyLeavePayments.Add(dummyLeavePayment);
                            }
                        }
                    }
                }
            }
            //if (lastAllowancePaymentCodeID > 0)
            //{
            //    ArrayList leaveAllowPayments = GetDeductablePaymentRecord(dbConn, EmpID, previousPaymentRecordResult, lastAllowanceRate * lastAllowanceUnit * lastAllowanceRatio, lastAllowancePaymentCodeID);
            //    if (leaveAllowPayments != null)
            //    {

            //        foreach (EPaymentRecord leaveAllowPayment in leaveAllowPayments)
            //        {
            //            //  Only set the first payment record with daily adjustment
            //            if (leaveAllowPayment == leaveAllowPayments[0])
            //                leaveAllowPayment.PayRecNumOfDayAdj = lastAllowanceUnit;
            //            leaveAllowPayment.LeaveAppID = 0;// leaveApplication.LeaveAppID;
            //            leaveAllowPayment.PayRecRemark = lastAllowanceFormulaRemark
            //                + (lastAllowanceUnit.Equals(1) ? string.Empty : " x " + lastAllowanceUnit)
            //                + (lastAllowanceRatio.Equals(1) ? string.Empty : " x " + lastAllowanceRatio);
            //            leaveAllowPayment.RelatedObject = lastAllowanceLeaveAppList;
            //            leaveAllowPayment.PayRecIsRestDayPayment = true;
            //        }
            //        leavePayments.AddRange(leaveAllowPayments);
            //    }
            //}
            //if (lastDeductionPaymentCodeID > 0)
            //{
            //    ArrayList tmpPaymentRecords = (ArrayList)previousPaymentRecordResult.Clone();
            //    tmpPaymentRecords.AddRange(leavePayments);

            //    ArrayList leaveDeductPayments = GetDeductablePaymentRecord(dbConn, EmpID, tmpPaymentRecords, -lastDeductionRate * lastDeductionUnit * lastDeductionRatio, lastDeductionPaymentCodeID);
            //    if (leaveDeductPayments != null)
            //    {

            //        foreach (EPaymentRecord leaveDeductPayment in leaveDeductPayments)
            //        {
            //            //  Only set the first payment record with daily adjustment
            //            if (leaveDeductPayment == leaveDeductPayments[0])
            //                leaveDeductPayment.PayRecNumOfDayAdj = -lastDeductionUnit;
            //            leaveDeductPayment.LeaveAppID = 0;// leaveApplication.LeaveAppID;
            //            leaveDeductPayment.PayRecRemark = "-" + lastDeductionFormulaRemark
            //                + (lastDeductionUnit.Equals(1) ? string.Empty : " x " + lastDeductionUnit);// +" x " + lastDeductionRatio;
            //            leaveDeductPayment.RelatedObject = lastDeductionLeaveAppList;
            //        }
            //        leavePayments.AddRange(leaveDeductPayments);
            //    }
            //}

            System.Collections.Generic.List<EPaymentRecord> leavePayments = allowancePaymentBreakDownList.GeneratePaymentRecordList();
            leavePayments.AddRange(deductionPaymentBreakDownList.GeneratePaymentRecordList());
            leavePayments.AddRange(dummyLeavePayments);

            foreach (EPaymentRecord paymentRecord in leavePayments)
            {
                if (paymentRecord.RelatedObject is ICollection)
                {
                    foreach (ELeaveApplication leaveApplication in (ICollection)paymentRecord.RelatedObject)
                    {
                        if (string.IsNullOrEmpty(paymentRecord.LeaveAppIDList))
                            paymentRecord.LeaveAppIDList = leaveApplication.LeaveAppID.ToString("X");
                        else
                            paymentRecord.LeaveAppIDList += "|" + leaveApplication.LeaveAppID.ToString("X");
                    }
                }
                else if (paymentRecord.RelatedObject is ELeaveApplication)
                {
                    ELeaveApplication leaveApplication = (ELeaveApplication)paymentRecord.RelatedObject;
                    if (string.IsNullOrEmpty(paymentRecord.LeaveAppIDList))
                        paymentRecord.LeaveAppIDList = leaveApplication.LeaveAppID.ToString("X");
                    else
                        paymentRecord.LeaveAppIDList += "|" + leaveApplication.LeaveAppID.ToString("X");
                }
            }
            return new ArrayList(leavePayments);

        }
        //private ArrayList StatutoryHolidayTrialRun(int EmpID, EPayrollPeriod previousPaymentRecordResult, ArrayList paymentRecords, DateTime PayrollGroupDateFrom, DateTime PayrollGroupDateTo)
        //{
        //    return StatutoryHolidayTrialRun(EmpID, previousPaymentRecordResult, paymentRecords, PayrollGroupDateFrom, PayrollGroupDateTo, false, false);
        //}
        protected virtual ArrayList StatutoryHolidayTrialRun(int EmpID, EPayrollPeriod payrollPeriod, ArrayList previousPaymentRecordResult, DateTime PayrollGroupDateFrom, DateTime PayrollGroupDateTo, bool UseDeductionFormulaOnly, bool UsePublicHolidayTable)
        {
            ArrayList leavePayments = new ArrayList();
            ArrayList statutoryHolidays = null;
            if (!UsePublicHolidayTable)
            {
                DBFilter statutoryHolidayFilter = new DBFilter();
                statutoryHolidayFilter.add(new Match("StatutoryHolidayDate", ">=", PayrollGroupDateFrom));
                statutoryHolidayFilter.add(new Match("StatutoryHolidayDate", "<=", PayrollGroupDateTo));
                statutoryHolidayFilter.add("StatutoryHolidayDate", true);
                statutoryHolidays = EStatutoryHoliday.db.select(dbConn, statutoryHolidayFilter);
            }
            else
            {
                //  get public holiday table and convert to statutory holiday table for processing
                DBFilter publicHolidayFilter = new DBFilter();
                publicHolidayFilter.add(new Match("PublicHolidayDate", ">=", PayrollGroupDateFrom));
                publicHolidayFilter.add(new Match("PublicHolidayDate", "<=", PayrollGroupDateTo));
                publicHolidayFilter.add("PublicHolidayDate", true);
                ArrayList publicHolidayList = EPublicHoliday.db.select(dbConn, publicHolidayFilter);
                statutoryHolidays = new ArrayList();
                foreach (EPublicHoliday publicHoliday in publicHolidayList)
                {
                    EStatutoryHoliday statutoryHoliday = new EStatutoryHoliday();
                    statutoryHoliday.StatutoryHolidayDate = publicHoliday.PublicHolidayDate;
                    statutoryHoliday.StatutoryHolidayDesc = publicHoliday.PublicHolidayDesc;
                    statutoryHolidays.Add(statutoryHoliday);
                }
            }
            ArrayList lastDeductionLeaveAppList = new ArrayList();
            int lastDeductionPaymentCodeID = 0;
            double lastDeductionRate = 0;
            double lastDeductionUnit = 0;
            string lastDeductionFormulaRemark = string.Empty;
            string lastDeductionRemark = string.Empty;

            ArrayList lastAllowanceLeaveAppList = new ArrayList();
            int lastAllowancePaymentCodeID = 0;
            double lastAllowanceRate = 0;
            double lastAllowanceUnit = 0;
            string lastAllowanceFormulaRemark = string.Empty;
            string lastAllowanceRemark = string.Empty;

            foreach (EStatutoryHoliday statutoryHoliday in statutoryHolidays)
            {
                //  Check if any leave application that does not obtain statutory holiday allowance

                DBFilter leaveApplicationFilter = new DBFilter();
                leaveApplicationFilter.add(new Match("EmpID", EmpID));
                leaveApplicationFilter.add(new Match("LeaveAppDateFrom", "<=", statutoryHoliday.StatutoryHolidayDate));
                leaveApplicationFilter.add(new Match("LeaveAppDateTo", ">=", statutoryHoliday.StatutoryHolidayDate));
                ArrayList leaveApplicationList = ELeaveApplication.db.select(dbConn, leaveApplicationFilter);
                bool isSkipStatutoryHolidayCalculation = false;
                foreach (ELeaveApplication leaveApp in leaveApplicationList)
                {
                    ELeaveCode leaveCode = new ELeaveCode();
                    leaveCode.LeaveCodeID = leaveApp.LeaveCodeID;
                    if (ELeaveCode.db.select(dbConn, leaveCode))
                    {
                        ELeaveType leaveType = new ELeaveType();
                        leaveType.LeaveTypeID = leaveCode.LeaveTypeID;
                        if (ELeaveType.db.select(dbConn, leaveType))
                            if (leaveType.LeaveTypeIsSkipStatutoryHolidayChecking)
                            {
                                isSkipStatutoryHolidayCalculation = true;
                                break;
                            }
                    }
                }

                if (!isSkipStatutoryHolidayCalculation)
                {
                    DBFilter positionFilter = new DBFilter();
                    positionFilter.add(new Match("EmpID", EmpID));
                    positionFilter.add(new Match("EmpPosEffFr", "<=", statutoryHoliday.StatutoryHolidayDate));

                    OR orFilter = new OR();
                    orFilter.add(new Match("EmpPosEffTo", ">=", statutoryHoliday.StatutoryHolidayDate));
                    orFilter.add(new NullTerm("EmpPosEffTo"));
                    positionFilter.add(orFilter);

                    positionFilter.add("EmpPosEffFr", true);

                    ArrayList positionInfos = EEmpPositionInfo.db.select(dbConn, positionFilter);

                    EPayrollGroup statHolGroup;

                    if (positionInfos.Count > 0)
                    {
                        EEmpPositionInfo positionInfo = (EEmpPositionInfo)positionInfos[0];

                        statHolGroup = new EPayrollGroup();
                        statHolGroup.PayGroupID = positionInfo.PayGroupID;
                        EPayrollGroup.db.select(dbConn, statHolGroup);
                    }
                    else
                    {
                        statHolGroup = new EPayrollGroup();
                        statHolGroup.PayGroupID = payrollPeriod.PayGroupID;
                        EPayrollGroup.db.select(dbConn, statHolGroup);
                    }

                    EPayrollPeriod statHolPayPeriod;
                    if (statutoryHoliday.StatutoryHolidayDate >= payrollPeriod.PayPeriodFr && statutoryHoliday.StatutoryHolidayDate <= payrollPeriod.PayPeriodTo)
                        statHolPayPeriod = payrollPeriod;
                    else
                        statHolPayPeriod = GenerateDummyPayrollPeriod(dbConn, statHolGroup.PayGroupID, statutoryHoliday.StatutoryHolidayDate);


                    double totalRPAmount = GetTotalPeriodPayRecurringPayment(dbConn, EmpID, statutoryHoliday.StatutoryHolidayDate, false, PayrollProcess.PaymentCodeProrataOptionEnum.PaymentCodeProrataAny, PayrollProcess.PaymentCodeProrataOptionEnum.PaymentCodeProrataAny, PaymentCodeProrataOptionEnum.PaymentCodeProrataProrataOnly);
                    if (statHolGroup.PayGroupIsCNDProrata)
                    {
                        double totalCNDAmount = GetTotalPeriodPayClaimsAndDeductions(dbConn, EmpID, statutoryHoliday.StatutoryHolidayDate, statHolPayPeriod, false, PayrollProcess.PaymentCodeProrataOptionEnum.PaymentCodeProrataAny, PayrollProcess.PaymentCodeProrataOptionEnum.PaymentCodeProrataAny, PaymentCodeProrataOptionEnum.PaymentCodeProrataProrataOnly);
                        totalRPAmount += totalCNDAmount;
                    }

                    int numOfPeriodPerYear = statHolGroup.NumOfPeriodPerYear();

                    string allowanceFormulaRemark;
                    string deductionFormulaRemark;
                    bool IsDAW = false;

                    double dailyAllowAmount = PayrollFormula.DailyProrataCaluclation(dbConn, statHolGroup.PayGroupStatHolAllowFormula, statHolGroup.PayGroupDefaultProrataFormula, EmpID, totalRPAmount, statHolPayPeriod.PayPeriodFr, statHolPayPeriod.PayPeriodTo, numOfPeriodPerYear, statutoryHoliday.StatutoryHolidayDate, out allowanceFormulaRemark, out IsDAW);
                    //double dailyAllowAmount = PayrollFormula.DailyProrataCaluclationForStatutoryHoliday(dbConn, statHolGroup.PayGroupStatHolAllowFormula, statHolGroup.PayGroupDefaultProrataFormula, EmpID, totalRPAmount, statHolPayPeriod.PayPeriodFr, statHolPayPeriod.PayPeriodTo, numOfPeriodPerYear, statutoryHoliday.StatutoryHolidayDate, out allowanceFormulaRemark, out IsDAW);

                    if (!IsDAW)
                    {
                        double totalDailyPayment = PayrollProcess.GetTotalDailyPayRecurringPayment(dbConn, EmpID, statutoryHoliday.StatutoryHolidayDate, true, statHolPayPeriod);
                        if (dailyAllowAmount.Equals(0))
                        {
                            dailyAllowAmount = totalDailyPayment;
                            allowanceFormulaRemark = totalDailyPayment.ToString("$#,##0.00##");
                        }
                        else if (!totalDailyPayment.Equals(0))
                        {
                            dailyAllowAmount += totalDailyPayment;
                            allowanceFormulaRemark += " + " + totalDailyPayment.ToString("$#,##0.00##");
                        }
                        if (dailyAllowAmount == 0)
                            //  HOURLY NOT SUPPORT
                            dailyAllowAmount = 0;//PayrollProcess.GetTotalHourlyPayRecurringPayment(EmpID, statutoryHoliday.StatutoryHolidayDate);
                    }
                    double dailyDeductAmount = PayrollFormula.DailyProrataCaluclation(dbConn, statHolGroup.PayGroupStatHolDeductFormula, statHolGroup.PayGroupDefaultProrataFormula, EmpID, totalRPAmount, statHolPayPeriod.PayPeriodFr, statHolPayPeriod.PayPeriodTo, numOfPeriodPerYear, statutoryHoliday.StatutoryHolidayDate, out deductionFormulaRemark, out IsDAW);
                    //double dailyDeductAmount = PayrollFormula.DailyProrataCaluclationForStatutoryHoliday(dbConn, statHolGroup.PayGroupStatHolDeductFormula, statHolGroup.PayGroupDefaultProrataFormula, EmpID, totalRPAmount, statHolPayPeriod.PayPeriodFr, statHolPayPeriod.PayPeriodTo, numOfPeriodPerYear, statutoryHoliday.StatutoryHolidayDate, out deductionFormulaRemark, out IsDAW);

                    if (!statHolGroup.PayGroupPayAdvanceCompareTotalPaymentOnly)
                    {
                        if (statHolGroup.PayGroupPayAdvance)
                        {
                            if (dailyDeductAmount > dailyAllowAmount)
                            {
                                dailyAllowAmount = dailyDeductAmount;
                                allowanceFormulaRemark = deductionFormulaRemark;
                            }
                        }
                    }
                    else
                    {
                        if (UseDeductionFormulaOnly)
                        {
                            dailyAllowAmount = dailyDeductAmount;
                            allowanceFormulaRemark = deductionFormulaRemark;
                        }
                    }

                    //  Check if Statutory Holiday Allowance is eligible 
                    EEmpPersonalInfo empInfo = new EEmpPersonalInfo();
                    empInfo.EmpID = EmpID;
                    DateTime eligibleDate = new DateTime();
                    if (EEmpPersonalInfo.db.select(dbConn, empInfo))
                    {
                        eligibleDate = empInfo.EmpServiceDate;
                        if (statHolGroup.PayGroupStatHolEligiblePeriod > 0 && !string.IsNullOrEmpty(statHolGroup.PayGroupStatHolEligibleUnit))
                        {
                            if (statHolGroup.PayGroupStatHolEligibleUnit.Equals("M"))
                                eligibleDate = eligibleDate.AddMonths(statHolGroup.PayGroupStatHolEligiblePeriod);
                            else if (statHolGroup.PayGroupStatHolEligibleUnit.Equals("D"))
                                eligibleDate = eligibleDate.AddDays(statHolGroup.PayGroupStatHolEligiblePeriod);
                        }
                    }
                    if (statHolGroup.PayGroupStatHolEligibleAfterProbation)
                    {
                        DateTime lastProbationDate = empInfo.EmpProbaLastDate; // AppUtils.GetLastProbationDate(EmpID);
                        if (lastProbationDate > eligibleDate)
                            eligibleDate = lastProbationDate.AddDays(1);
                    }
                    if (eligibleDate > statutoryHoliday.StatutoryHolidayDate)
                    {
                        dailyAllowAmount = 0;
                        if (statHolGroup.PayGroupStatHolEligibleSkipDeduction)
                            dailyDeductAmount = 0;
                    }

                    EPaymentCode leaveAllowPaymentCode = GetLeaveAllowanceCode(true, statHolGroup.PayGroupStatHolAllowPaymentCodeID);
                    EPaymentCode leaveDeductPaymentCode = GetLeaveDeductionCode(true, statHolGroup.PayGroupStatHolDeductPaymentCodeID);
                    //if (!(Math.Abs(dailyDeductAmount - dailyAllowAmount) < 0.01
                    //    && leaveAllowPaymentCode.PaymentCodeIsMPF == leaveDeductPaymentCode.PaymentCodeIsMPF
                    //    && leaveAllowPaymentCode.PaymentCodeIsWages == leaveDeductPaymentCode.PaymentCodeIsWages)
                    //    )
                    //{
                    //    //  for zero amount payment.
                    //    //  Not in use
                    //}
                    //else
                    {
                        // allowance only on or after eligible date
                        if (leaveAllowPaymentCode != null && dailyAllowAmount != 0)
                        {
                            if (lastAllowancePaymentCodeID != leaveAllowPaymentCode.PaymentCodeID || Math.Abs(lastAllowanceRate - dailyAllowAmount) >= 0.01 || lastAllowanceFormulaRemark != allowanceFormulaRemark)
                            {
                                if (lastAllowancePaymentCodeID > 0)
                                {
                                    ArrayList leaveAllowPayments = GetDeductablePaymentRecord(dbConn, EmpID, previousPaymentRecordResult, lastAllowanceRate * lastAllowanceUnit, lastAllowancePaymentCodeID);
                                    if (leaveAllowPayments != null)
                                    {

                                        foreach (EPaymentRecord leaveAllowPayment in leaveAllowPayments)
                                        {
                                            //  Only set the first payment record with daily adjustment
                                            if (leaveAllowPayment == leaveAllowPayments[0])
                                                leaveAllowPayment.PayRecNumOfDayAdj = lastAllowanceUnit;
                                            leaveAllowPayment.PayRecRemark = lastAllowanceFormulaRemark
                                                + (lastAllowanceUnit.Equals(1) ? string.Empty : " x " + lastAllowanceUnit)
                                                + "\r\n" + lastAllowanceRemark;
                                            leaveAllowPayment.PayRecIsRestDayPayment = true;
                                        }
                                        leavePayments.AddRange(leaveAllowPayments);
                                    }
                                }
                                lastAllowancePaymentCodeID = leaveAllowPaymentCode.PaymentCodeID;
                                lastAllowanceRate = dailyAllowAmount;
                                lastAllowanceFormulaRemark = allowanceFormulaRemark;
                                lastAllowanceRemark = string.Empty;
                                lastAllowanceUnit = 0;
                            }
                            lastAllowanceUnit++;
                            if (string.IsNullOrEmpty(lastAllowanceRemark))
                                lastAllowanceRemark = statutoryHoliday.StatutoryHolidayDesc;
                            else
                                lastAllowanceRemark += "\r\n" + statutoryHoliday.StatutoryHolidayDesc;
                        }

                        //ArrayList leaveAllowPayments = GetDeductablePaymentRecord(dbConn, EmpID, paymentRecords, dailyAllowAmount, leaveAllowPaymentCode.PaymentCodeID);
                        //if (leaveAllowPayments != null)
                        //{

                        //    foreach (EPaymentRecord leaveAllowPayment in leaveAllowPayments)
                        //    {
                        //        //  Only set the first payment record with daily adjustment
                        //        if (leaveAllowPayment == leaveAllowPayments[0])
                        //            leaveAllowPayment.PayRecNumOfDayAdj = 1;
                        //        leaveAllowPayment.PayRecRemark = statutoryHoliday.StatutoryHolidayDesc + " $(" + allowanceFormulaRemark + ")";
                        //    }
                        //    leavePayments.AddRange(leaveAllowPayments);
                        //}


                        if (leaveDeductPaymentCode != null && dailyDeductAmount != 0)
                        {
                            if (lastDeductionPaymentCodeID != leaveDeductPaymentCode.PaymentCodeID || Math.Abs(lastDeductionRate - dailyDeductAmount) >= 0.01 || lastDeductionFormulaRemark != deductionFormulaRemark)
                            {
                                if (lastDeductionPaymentCodeID > 0)
                                {
                                    ArrayList tmpPaymentRecords = (ArrayList)previousPaymentRecordResult.Clone();
                                    tmpPaymentRecords.AddRange(leavePayments);

                                    ArrayList leaveDeductPayments = GetDeductablePaymentRecord(dbConn, EmpID, tmpPaymentRecords, -lastDeductionRate * lastDeductionUnit, lastDeductionPaymentCodeID);
                                    if (leaveDeductPayments != null)
                                    {

                                        foreach (EPaymentRecord leaveDeductPayment in leaveDeductPayments)
                                        {
                                            //  Only set the first payment record with daily adjustment
                                            if (leaveDeductPayment == leaveDeductPayments[0])
                                                leaveDeductPayment.PayRecNumOfDayAdj = -lastDeductionUnit;
                                            leaveDeductPayment.PayRecRemark = "-" + lastDeductionFormulaRemark
                                                + (lastDeductionUnit.Equals(1) ? string.Empty : " x " + lastDeductionUnit)
                                                + "\r\n" + lastDeductionRemark;
                                        }
                                        leavePayments.AddRange(leaveDeductPayments);
                                    }
                                }
                                lastDeductionPaymentCodeID = leaveDeductPaymentCode.PaymentCodeID;
                                lastDeductionRate = dailyDeductAmount;
                                lastDeductionFormulaRemark = deductionFormulaRemark;
                                lastDeductionRemark = string.Empty;
                                lastDeductionUnit = 0;
                            }
                            lastDeductionUnit++;
                            if (string.IsNullOrEmpty(lastDeductionRemark))
                                lastDeductionRemark = statutoryHoliday.StatutoryHolidayDesc;
                            else
                                lastDeductionRemark += "\r\n" + statutoryHoliday.StatutoryHolidayDesc;

                            //ArrayList leaveDeductPayments = GetDeductablePaymentRecord(dbConn, EmpID, paymentRecords, -dailyDeductAmount * leaveApplication.LeaveAppDays, leaveDeductPaymentCode.PaymentCodeID);
                            //if (leaveDeductPayments != null)
                            //{
                            //    foreach (EPaymentRecord leaveDeductPayment in leaveDeductPayments)
                            //    {
                            //        if (leaveDeductPayment == leaveDeductPayments[0])
                            //            leaveDeductPayment.PayRecNumOfDayAdj = -leaveApplication.LeaveAppDays;
                            //        leaveDeductPayment.LeaveAppID = leaveApplication.LeaveAppID;
                            //        leaveDeductPayment.PayRecRemark = "-" + deductionFormulaRemark + " x " + leaveApplication.LeaveAppDays;
                            //        leaveDeductPayment.RelatedObject = leaveApplication;
                            //    }

                            //    leavePayments.AddRange(leaveDeductPayments);
                            //}
                        }
                    }
                    //ArrayList leaveDeductPayments = GetDeductablePaymentRecord(dbConn, EmpID, paymentRecords, -dailyDeductAmount, leaveDeductPaymentCode.PaymentCodeID);
                    //if (leaveDeductPayments != null)
                    //{
                    //    foreach (EPaymentRecord leaveDeductPayment in leaveDeductPayments)
                    //    {
                    //        if (leaveDeductPayment == leaveDeductPayments[0])
                    //            leaveDeductPayment.PayRecNumOfDayAdj = -1;
                    //        leaveDeductPayment.PayRecRemark = statutoryHoliday.StatutoryHolidayDesc + " $(" + deductionFormulaRemark + ")";
                    //    }

                    //    leavePayments.AddRange(leaveDeductPayments);
                    //}
                    //}

                }
            }
            if (lastAllowancePaymentCodeID > 0)
            {
                ArrayList leaveAllowPayments = GetDeductablePaymentRecord(dbConn, EmpID, previousPaymentRecordResult, lastAllowanceRate * lastAllowanceUnit, lastAllowancePaymentCodeID);
                if (leaveAllowPayments != null)
                {

                    foreach (EPaymentRecord leaveAllowPayment in leaveAllowPayments)
                    {
                        //  Only set the first payment record with daily adjustment
                        if (leaveAllowPayment == leaveAllowPayments[0])
                            leaveAllowPayment.PayRecNumOfDayAdj = lastAllowanceUnit;
                        leaveAllowPayment.PayRecRemark = lastAllowanceFormulaRemark
                            + (lastAllowanceUnit.Equals(1) ? string.Empty : " x " + lastAllowanceUnit)
                            + "\r\n" + lastAllowanceRemark;
                        leaveAllowPayment.PayRecIsRestDayPayment = true;
                    }
                    leavePayments.AddRange(leaveAllowPayments);
                }
            }
            if (lastDeductionPaymentCodeID > 0)
            {
                ArrayList tmpPaymentRecords = (ArrayList)previousPaymentRecordResult.Clone();
                tmpPaymentRecords.AddRange(leavePayments);

                ArrayList leaveDeductPayments = GetDeductablePaymentRecord(dbConn, EmpID, tmpPaymentRecords, -lastDeductionRate * lastDeductionUnit, lastDeductionPaymentCodeID);
                if (leaveDeductPayments != null)
                {

                    foreach (EPaymentRecord leaveDeductPayment in leaveDeductPayments)
                    {
                        //  Only set the first payment record with daily adjustment
                        if (leaveDeductPayment == leaveDeductPayments[0])
                            leaveDeductPayment.PayRecNumOfDayAdj = -lastDeductionUnit;
                        leaveDeductPayment.PayRecRemark = "-" + lastDeductionFormulaRemark
                            + (lastDeductionUnit.Equals(1) ? string.Empty : " x " + lastDeductionUnit)
                            + "\r\n" + lastDeductionRemark;
                    }
                    leavePayments.AddRange(leaveDeductPayments);
                }
            }

            return leavePayments;

        }

        public ArrayList ContractGratuityTrialRun(int EmpID, DateTime PayrollGroupDateFrom, DateTime PayrollGroupDateTo)
        {
            ArrayList gratuityPaymentList = new ArrayList();
            DBFilter empContractFilter = new DBFilter();
            empContractFilter.add(new Match("EmpContractEmployedTo", ">=", PayrollGroupDateFrom));
            empContractFilter.add(new Match("EmpContractEmployedTo", "<=", PayrollGroupDateTo));
            empContractFilter.add(new Match("EmpID", EmpID));
            ArrayList empContractList = EEmpContractTerms.db.select(dbConn, empContractFilter);
            foreach (EEmpContractTerms empContract in empContractList)
            {
                if (empContract.EmpContractGratuity > 0 && !string.IsNullOrEmpty(empContract.EmpContractGratuityMethod) && empContract.PayCodeID > 0)
                {
                    EPaymentRecord payment = new EPaymentRecord();

                    payment.PaymentCodeID = empContract.PayCodeID;
                    payment.PayRecMethod = empContract.EmpContractGratuityMethod;
                    payment.EmpAccID = empContract.EmpAccID;

                    payment.PayRecType = PaymentRecordType.PAYRECORDTYPE_RECURRING;
                    payment.PayRecStatus = PaymentRecordStatus.PAYRECORDSTATUS_ACTIVE;
                    payment.CurrencyID = ExchangeCurrency.DefaultCurrency();
                    payment.PayRecCalAmount = empContract.EmpContractGratuity;
                    payment.PayRecActAmount = HROne.CommonLib.GenericRoundingFunctions.RoundingTo(payment.PayRecCalAmount, ExchangeCurrency.DefaultCurrencyDecimalPlaces(), ExchangeCurrency.DefaultCurrencyDecimalPlaces());
                    payment.PayRecRemark = HROne.Common.WebUtility.GetLocalizedString("Gratuity") + " " + empContract.EmpContractEmployedFrom.ToString("yyyy-MM-dd") + " " + HROne.Common.WebUtility.GetLocalizedString("To") + " " + empContract.EmpContractEmployedTo.ToString("yyyy-MM-dd");

                    gratuityPaymentList.Add(payment);
                }
            }
            return gratuityPaymentList;
        }

        public ArrayList AdditionalRemunerationTrialRun(int EmpID, EPayrollPeriod payPeriod, ArrayList paymentRecords, double totalWorkingHours)
        {
            EPayrollGroup payGroup = new EPayrollGroup();
            payGroup.PayGroupID = payPeriod.PayGroupID;
            if (EPayrollGroup.db.select(dbConn, payGroup))
            {
                EPaymentCode payCode = new EPaymentCode();
                payCode.PaymentCodeID = payGroup.PayGroupAdditionalRemunerationPayCodeID;
                if (EPaymentCode.db.select(dbConn, payCode))
                {
                    double wagesForMinimumWage = GetTotalWagesWithoutRestDayPayment(dbConn, EmpID, payPeriod.PayPeriodFr, payPeriod.PayPeriodTo, paymentRecords);
                    double minimumWageRequired = GetMinimumWages(dbConn, EmpID, payPeriod.PayPeriodTo) * totalWorkingHours;
                    if (wagesForMinimumWage < minimumWageRequired)
                    {
                        ArrayList paymentList = GetDeductablePaymentRecord(dbConn, EmpID, paymentRecords, minimumWageRequired - wagesForMinimumWage, payGroup.PayGroupAdditionalRemunerationPayCodeID);
                        return paymentList;
                    }
                }
            }
            return new ArrayList();
        }

        public static DBFilter GetRecurringPaymentDBFilter(int EmpID, DateTime AsOfDate, string SortingField, bool isAscending)
        {
            DBFilter filter = new DBFilter();
            filter.add(new Match("EmpID", EmpID));
            filter.add(new Match("EmpRPEffFr", "<=", AsOfDate));

            OR orFilter = new OR();
            orFilter.add(new Match("EmpRPEffTo", ">=", AsOfDate));
            orFilter.add(new NullTerm("EmpRPEffTo"));
            filter.add(orFilter);

            OR orEmpRPIsNonPayrollItem = new OR();
            orEmpRPIsNonPayrollItem.add(new Match("EmpRPIsNonPayrollItem", false));
            orEmpRPIsNonPayrollItem.add(new NullTerm("EmpRPIsNonPayrollItem"));
            filter.add(orEmpRPIsNonPayrollItem);

            filter.add(SortingField, isAscending);
            return filter;
        }

        public enum PaymentCodeProrataOptionEnum
        {
            PaymentCodeProrataAny,
            PaymentCodeProrataProrataOnly,
            PaymentCodeProrataNoProrataOnly
        }
        //[Obsolete()]
        //public static double GetTotalPeriodPayRecurringPayment(DatabaseConnection dbConn, int EmpID, DateTime AsOfDate, bool BasicSalaryPaymentTypeOnly, PaymentCodeProrataOptionEnum PaymentCodeWithProrata, PaymentCodeProrataOptionEnum PaymentCodeWithProrataLeave)
        //{
        //    return GetTotalPeriodPayRecurringPayment(dbConn, EmpID, AsOfDate, BasicSalaryPaymentTypeOnly, PaymentCodeWithProrata, PaymentCodeWithProrataLeave, PaymentCodeProrataOptionEnum.PaymentCodeProrataAny);
        //}
        public static double GetTotalPeriodPayRecurringPayment(DatabaseConnection dbConn, int EmpID, DateTime AsOfDate, bool BasicSalaryPaymentTypeOnly, PaymentCodeProrataOptionEnum PaymentCodeWithProrata, PaymentCodeProrataOptionEnum PaymentCodeWithProrataLeave, PaymentCodeProrataOptionEnum PaymentCodeWithProrataStatutoryHoliday)
        {
            DBFilter filter = GetRecurringPaymentDBFilter(EmpID, AsOfDate, "EmpRPEffFr", true);
            filter.add(new Match("EmpRPUnit", "P"));
            filter.add(new Match("EmpRPUnitPeriodAsDaily", false));

            DBFilter paymentCodeFilter = null;
            if (BasicSalaryPaymentTypeOnly)
            {
                if (paymentCodeFilter == null)
                    paymentCodeFilter = new DBFilter();
                paymentCodeFilter.add(new Match("PaymentTypeID", EPaymentType.SystemPaymentType.BasicSalaryPaymentType(dbConn).PaymentTypeID));
            }
            if (PaymentCodeWithProrata != PaymentCodeProrataOptionEnum.PaymentCodeProrataAny)
            {
                if (paymentCodeFilter == null)
                    paymentCodeFilter = new DBFilter();
                if (PaymentCodeWithProrata.Equals(PaymentCodeProrataOptionEnum.PaymentCodeProrataProrataOnly))
                    paymentCodeFilter.add(new Match("PaymentCodeIsProrata", true));
                else
                    paymentCodeFilter.add(new Match("PaymentCodeIsProrata", false));

            }
            if (PaymentCodeWithProrataLeave != PaymentCodeProrataOptionEnum.PaymentCodeProrataAny)
            {
                if (paymentCodeFilter == null)
                    paymentCodeFilter = new DBFilter();
                if (PaymentCodeWithProrataLeave.Equals(PaymentCodeProrataOptionEnum.PaymentCodeProrataProrataOnly))
                    paymentCodeFilter.add(new Match("PaymentCodeIsProrataLeave", true));
                else
                    paymentCodeFilter.add(new Match("PaymentCodeIsProrataLeave", false));
            }
            if (PaymentCodeWithProrataStatutoryHoliday != PaymentCodeProrataOptionEnum.PaymentCodeProrataAny)
            {
                if (paymentCodeFilter == null)
                    paymentCodeFilter = new DBFilter();
                if (PaymentCodeWithProrataStatutoryHoliday.Equals(PaymentCodeProrataOptionEnum.PaymentCodeProrataProrataOnly))
                    paymentCodeFilter.add(new Match("PaymentCodeIsProrataStatutoryHoliday", true));
                else
                    paymentCodeFilter.add(new Match("PaymentCodeIsProrataStatutoryHoliday", false));
            }
            if (paymentCodeFilter != null)
                filter.add(new IN("PayCodeID", "Select PaymentCodeID from PaymentCode", paymentCodeFilter));

            ArrayList recurringPayments = EEmpRecurringPayment.db.select(dbConn, filter);

            double totalAmount = 0;

            foreach (EEmpRecurringPayment empRP in recurringPayments)
            {
                //if (empRP.EmpRPUnit.Equals("P",StringComparison.CurrentCultureIgnoreCase))
                {
                    totalAmount += ExchangeCurrency.Exchange(empRP.EmpRPAmount, empRP.CurrencyID, false);
                }
            }
            return totalAmount;
        }

        public static double GetTotalPeriodPayClaimsAndDeductions(DatabaseConnection dbConn, int EmpID, DateTime AsOfDate, EPayrollPeriod leavePayPeriod, bool BasicSalaryPaymentTypeOnly, PaymentCodeProrataOptionEnum PaymentCodeWithProrata, PaymentCodeProrataOptionEnum PaymentCodeWithProrataLeave, PaymentCodeProrataOptionEnum PaymentCodeWithProrataStatutoryHoliday)
        {
            DBFilter filter = new DBFilter();
            filter.add(new Match("EmpID", EmpID));
            filter.add(new Match("CNDEffDate", "<=", leavePayPeriod.PayPeriodTo));
            filter.add(new Match("CNDEffDate", ">=", leavePayPeriod.PayPeriodFr));

            DBFilter paymentCodeFilter = null;
            if (BasicSalaryPaymentTypeOnly)
            {
                if (paymentCodeFilter == null)
                    paymentCodeFilter = new DBFilter();
                paymentCodeFilter.add(new Match("PaymentTypeID", EPaymentType.SystemPaymentType.BasicSalaryPaymentType(dbConn).PaymentTypeID));
            }
            if (PaymentCodeWithProrata != PaymentCodeProrataOptionEnum.PaymentCodeProrataAny)
            {
                if (paymentCodeFilter == null)
                    paymentCodeFilter = new DBFilter();
                if (PaymentCodeWithProrata.Equals(PaymentCodeProrataOptionEnum.PaymentCodeProrataProrataOnly))
                    paymentCodeFilter.add(new Match("PaymentCodeIsProrata", true));
                else
                    paymentCodeFilter.add(new Match("PaymentCodeIsProrata", false));

            }
            if (PaymentCodeWithProrataLeave != PaymentCodeProrataOptionEnum.PaymentCodeProrataAny)
            {
                if (paymentCodeFilter == null)
                    paymentCodeFilter = new DBFilter();
                if (PaymentCodeWithProrataLeave.Equals(PaymentCodeProrataOptionEnum.PaymentCodeProrataProrataOnly))
                    paymentCodeFilter.add(new Match("PaymentCodeIsProrataLeave", true));
                else
                    paymentCodeFilter.add(new Match("PaymentCodeIsProrataLeave", false));
            }
            if (PaymentCodeWithProrataStatutoryHoliday != PaymentCodeProrataOptionEnum.PaymentCodeProrataAny)
            {
                if (paymentCodeFilter == null)
                    paymentCodeFilter = new DBFilter();
                if (PaymentCodeWithProrataStatutoryHoliday.Equals(PaymentCodeProrataOptionEnum.PaymentCodeProrataProrataOnly))
                    paymentCodeFilter.add(new Match("PaymentCodeIsProrataStatutoryHoliday", true));
                else
                    paymentCodeFilter.add(new Match("PaymentCodeIsProrataStatutoryHoliday", false));
            }
            if (paymentCodeFilter != null)
                filter.add(new IN("PayCodeID", "Select PaymentCodeID from PaymentCode", paymentCodeFilter));

            ArrayList cndList = EClaimsAndDeductions.db.select(dbConn, filter);

            double totalAmount = 0;

            foreach (EClaimsAndDeductions cnd in cndList)
            {
                {
                    totalAmount += ExchangeCurrency.Exchange(cnd.CNDAmount, cnd.CurrencyID, false);
                }
            }
            return totalAmount;
        }

        [Obsolete()]
        public static double GetTotalDailyPayRecurringPayment(DatabaseConnection dbConn, int EmpID, DateTime AsOfDate, bool BasicSalaryPaymentTypeOnly)
        {
            return GetTotalDailyPayRecurringPayment(dbConn, EmpID, AsOfDate, BasicSalaryPaymentTypeOnly, new EPayrollPeriod());
        }
        public static double GetTotalDailyPayRecurringPayment(DatabaseConnection dbConn, int EmpID, DateTime AsOfDate, bool BasicSalaryPaymentTypeOnly, EPayrollPeriod PayPeriod)
        {
            DBFilter filter = GetRecurringPaymentDBFilter(EmpID, AsOfDate, "EmpRPEffFr", true);
            OR orEmpRPUnitTerm = new OR();
            filter.add(orEmpRPUnitTerm);
            orEmpRPUnitTerm.add(new Match("EmpRPUnit", "D"));
            AND andPeriodAsDailyOption = new AND();
            andPeriodAsDailyOption.add(new Match("EmpRPUnit", "P"));
            andPeriodAsDailyOption.add(new Match("EmpRPUnitPeriodAsDaily", true));
            orEmpRPUnitTerm.add(andPeriodAsDailyOption);

            DBFilter paymentCodeFilter = null;
            if (BasicSalaryPaymentTypeOnly)
            {
                if (paymentCodeFilter == null)
                    paymentCodeFilter = new DBFilter();
                paymentCodeFilter.add(new Match("PaymentTypeID", EPaymentType.SystemPaymentType.BasicSalaryPaymentType(dbConn).PaymentTypeID));
            }
            if (paymentCodeFilter != null)
                filter.add(new IN("PayCodeID", "Select PaymentCodeID from PaymentCode", paymentCodeFilter));

            ArrayList recurringPayments = EEmpRecurringPayment.db.select(dbConn, filter);

            double totalAmount = 0;

            foreach (EEmpRecurringPayment empRP in recurringPayments)
            {
                if (empRP.EmpRPUnit.Equals("D", StringComparison.CurrentCultureIgnoreCase))
                {
                    totalAmount += ExchangeCurrency.Exchange(empRP.EmpRPAmount, empRP.CurrencyID, false);
                }
                else
                {
                    string dummyRemark;
                    bool dummyIsDAW;
                    EPayrollGroup payrollGroup = new EPayrollGroup();
                    payrollGroup.PayGroupID = PayPeriod.PayGroupID;
                    if (EPayrollGroup.db.select(dbConn, payrollGroup))
                        totalAmount += PayrollFormula.DailyProrataCaluclation(dbConn, empRP.EmpRPUnitPeriodAsDailyPayFormID, payrollGroup.PayGroupDefaultProrataFormula, EmpID, empRP.EmpRPAmount, PayPeriod.PayPeriodFr, PayPeriod.PayPeriodTo, payrollGroup.NumOfPeriodPerYear(), AsOfDate, 0, out dummyRemark, out dummyIsDAW);

                }
            }
            return totalAmount;
        }

        public static double GetTotalHourlyPayRecurringPayment(DatabaseConnection dbConn, int EmpID, DateTime AsOfDate)
        {
            DBFilter filter = GetRecurringPaymentDBFilter(EmpID, AsOfDate, "EmpRPEffFr", true);
            filter.add(new Match("EmpRPUnit", "H"));

            ArrayList recurringPayments = EEmpRecurringPayment.db.select(dbConn, filter);

            double totalAmount = 0;

            foreach (EEmpRecurringPayment empRP in recurringPayments)
            {
                //if (empRP.EmpRPUnit.Equals("P",StringComparison.CurrentCultureIgnoreCase))
                {
                    totalAmount += ExchangeCurrency.Exchange(empRP.EmpRPAmount, empRP.CurrencyID, false);
                }
            }
            return totalAmount;
        }

        public static ArrayList GetPaymentCodeByPaymentType(DatabaseConnection dbConn, string PaymentTypeCode)
        {

            DBFilter paymentCodeFilter = new DBFilter();

            DBFilter paymentTypeFilter = new DBFilter();
            paymentTypeFilter.add(new Match("PaymentTypeCode", PaymentTypeCode));
            IN inPaymentTypeFilter = new IN("PaymentTypeID", "Select PaymentTypeID from PaymentType", paymentTypeFilter);

            paymentCodeFilter.add(inPaymentTypeFilter);

            ArrayList paymentCodes = EPaymentCode.db.select(dbConn, paymentCodeFilter);
            return paymentCodes;
            //if (paymentCodes.Count > 0)
            //    return (EPaymentCode)paymentCodes[0];
            //else
            //    return null;

        }

        private EPaymentCode GetLeaveAllowanceCode(bool IsWages, int DefaultLeavePaymentCodeID)
        {
            EPaymentCode payCode = new EPaymentCode();
            payCode.PaymentCodeID = DefaultLeavePaymentCodeID;
            if (EPaymentCode.db.select(dbConn, payCode))
                if (payCode.PaymentCodeIsWages.Equals(IsWages ? true : false))
                    return payCode;

            EPaymentType paymentType = EPaymentType.SystemPaymentType.LeaveAllowancePaymentType(dbConn);

            DBFilter paymentCodeFilter = new DBFilter();


            paymentCodeFilter.add(new Match("PaymentTypeID", paymentType.PaymentTypeID));
            paymentCodeFilter.add(new Match("PaymentCodeIsWages", IsWages ? "1" : "0"));

            ArrayList paymentCodes = EPaymentCode.db.select(dbConn, paymentCodeFilter);

            if (paymentCodes.Count > 0)
                return (EPaymentCode)paymentCodes[0];
            else
            {
                EPaymentCode leaveAllowanceCode = new EPaymentCode();

                if (IsWages)
                {
                    leaveAllowanceCode.PaymentCode = "LEAVEALLOW_WAGES";
                    leaveAllowanceCode.PaymentCodeDesc = "Leave Allowance (Wages)";
                }
                else
                {
                    leaveAllowanceCode.PaymentCode = "LEAVEALLOW";
                    leaveAllowanceCode.PaymentCodeDesc = "Leave Allowance (Not Wages)";
                }
                leaveAllowanceCode.PaymentCodeIsWages = IsWages;
                leaveAllowanceCode.PaymentCodeIsMPF = true;
                leaveAllowanceCode.PaymentCodeIsProrata = false;
                leaveAllowanceCode.PaymentCodeIsTopUp = false;
                leaveAllowanceCode.PaymentTypeID = paymentType.PaymentTypeID;
                EPaymentCode.db.insert(dbConn, leaveAllowanceCode);
                return leaveAllowanceCode;

            }
        }
        private EPaymentCode GetLeaveDeductionCode(bool IsWages, int DefaultLeavePaymentCodeID)
        {
            EPaymentCode payCode = new EPaymentCode();
            payCode.PaymentCodeID = DefaultLeavePaymentCodeID;
            if (EPaymentCode.db.select(dbConn, payCode))
                if (payCode.PaymentCodeIsWages.Equals(IsWages ? true : false))
                    return payCode;

            DBFilter paymentCodeFilter = new DBFilter();

            EPaymentType paymentType = EPaymentType.SystemPaymentType.LeaveDeductionPaymentType(dbConn);

            paymentCodeFilter.add(new Match("PaymentTypeID", paymentType.PaymentTypeID));
            paymentCodeFilter.add(new Match("PaymentCodeIsWages", IsWages ? "1" : "0"));

            ArrayList paymentCodes = EPaymentCode.db.select(dbConn, paymentCodeFilter);

            if (paymentCodes.Count > 0)
                return (EPaymentCode)paymentCodes[0];
            else
            {
                EPaymentCode leaveADeductionCode = new EPaymentCode();


                if (IsWages)
                {
                    leaveADeductionCode.PaymentCode = "LEAVEDEDUCT_WAGES";
                    leaveADeductionCode.PaymentCodeDesc = "Leave Deduction (Wages)";
                }
                else
                {
                    leaveADeductionCode.PaymentCode = "LEAVEDEDUCT";
                    leaveADeductionCode.PaymentCodeDesc = "Leave Deduction (Not Wages)";
                }
                leaveADeductionCode.PaymentCodeIsWages = IsWages;
                leaveADeductionCode.PaymentCodeIsMPF = true;
                leaveADeductionCode.PaymentCodeIsProrata = false;
                leaveADeductionCode.PaymentCodeIsTopUp = false;
                leaveADeductionCode.PaymentTypeID = paymentType.PaymentTypeID;
                EPaymentCode.db.insert(dbConn, leaveADeductionCode);
                return leaveADeductionCode;

            }
        }
        public static ArrayList GetDeductablePaymentRecord(DatabaseConnection dbConn, int EmpID, ArrayList paymentRecords, double deductAmount, int paymentCodeID)
        {
            ArrayList mergePaymentRecordsByPaymentMethod = GetMergePaymentRecordByPaymentMethod(paymentRecords);
            ArrayList deductPaymentRecords = new ArrayList();
            EPaymentRecord remainPaymentRecord = new EPaymentRecord();

            remainPaymentRecord.CurrencyID = ExchangeCurrency.DefaultCurrency();
            remainPaymentRecord.PayRecStatus = PaymentRecordStatus.PAYRECORDSTATUS_ACTIVE;
            remainPaymentRecord.PayRecType = PaymentRecordType.PAYRECORDTYPE_RECURRING;
            remainPaymentRecord.PayRecCalAmount = 0;
            remainPaymentRecord.PayRecMethod = "O";
            remainPaymentRecord.PaymentCodeID = paymentCodeID;

            remainPaymentRecord.PayRecCalAmount = deductAmount;
            remainPaymentRecord.PayRecActAmount = remainPaymentRecord.PayRecCalAmount;


            foreach (EPaymentRecord paymentRecord in mergePaymentRecordsByPaymentMethod)
            {
                paymentRecord.PaymentCodeID = paymentCodeID;
                double creditAmount = paymentRecord.PayRecActAmount;
                paymentRecord.PayRecActAmount = 0;//    Set to 0;
                if (creditAmount + remainPaymentRecord.PayRecCalAmount >= 0 || remainPaymentRecord.PayRecCalAmount > 0)
                {
                    // Enough to deduct payment on this pay method
                    paymentRecord.PayRecCalAmount = remainPaymentRecord.PayRecCalAmount;
                    remainPaymentRecord.PayRecCalAmount = 0;

                    paymentRecord.PayRecActAmount = Math.Round(paymentRecord.PayRecCalAmount, ExchangeCurrency.DefaultCurrencyDecimalPlaces(), MidpointRounding.AwayFromZero);
                    remainPaymentRecord.PayRecActAmount = remainPaymentRecord.PayRecCalAmount;
                    deductPaymentRecords.Add(paymentRecord);
                    break;
                }
                else if (creditAmount > 0)
                {
                    //  20120801 System no longer split the payment into 2

                    //paymentRecord.PayRecCalAmount = -creditAmount;
                    //remainPaymentRecord.PayRecCalAmount += creditAmount;

                    //paymentRecord.PayRecActAmount = System.Math.Round(paymentRecord.PayRecCalAmount, ExchangeCurrency.DefaultCurrencyDecimalPlaces(), MidpointRounding.AwayFromZero);
                    //remainPaymentRecord.PayRecActAmount = remainPaymentRecord.PayRecCalAmount;
                    //deductPaymentRecords.Add(paymentRecord);
                }

            }
            remainPaymentRecord.PayRecActAmount = System.Math.Round(remainPaymentRecord.PayRecCalAmount, ExchangeCurrency.DefaultCurrencyDecimalPlaces(), MidpointRounding.AwayFromZero);
            if (Math.Abs(remainPaymentRecord.PayRecActAmount) >= 0.01 || deductPaymentRecords.Count == 0)
            {
                if (mergePaymentRecordsByPaymentMethod.Count == 1)
                {
                    remainPaymentRecord.PayRecMethod = ((EPaymentRecord)mergePaymentRecordsByPaymentMethod[0]).PayRecMethod;
                    remainPaymentRecord.EmpAccID = ((EPaymentRecord)mergePaymentRecordsByPaymentMethod[0]).EmpAccID;
                }
                else if (remainPaymentRecord.PayRecActAmount >= 0)
                {
                    EEmpBankAccount empBankAcc = EEmpBankAccount.GetDefaultBankAccount(dbConn, EmpID);
                    if (empBankAcc != null)
                    {
                        remainPaymentRecord.PayRecMethod = "A";
                        remainPaymentRecord.EmpAccID = 0;//empBankAcc.EmpBankAccountID;
                    }
                }
                deductPaymentRecords.Add(remainPaymentRecord);
            }
            return deductPaymentRecords;
        }

        private static ArrayList GetMergePaymentRecordByPaymentMethod(ArrayList paymentRecords)
        {

            ArrayList mergePaymentRecord = new ArrayList();
            foreach (EPaymentRecord paymentRecord in paymentRecords)
            {
                double paymentAmount = ExchangeCurrency.Exchange(paymentRecord.PayRecActAmount, paymentRecord.CurrencyID, false);

                EPaymentRecord usedTotalPaymentRecordByPayMethod = null;
                foreach (EPaymentRecord totalPaymentRecordByPayMethod in mergePaymentRecord)
                {
                    if (totalPaymentRecordByPayMethod.PayRecMethod == paymentRecord.PayRecMethod && totalPaymentRecordByPayMethod.CurrencyID == paymentRecord.CurrencyID && (paymentRecord.PayRecMethod == "A" && totalPaymentRecordByPayMethod.EmpAccID == paymentRecord.EmpAccID || paymentRecord.PayRecMethod != "A"))
                    {
                        usedTotalPaymentRecordByPayMethod = totalPaymentRecordByPayMethod;
                        break;
                    }

                }
                if (usedTotalPaymentRecordByPayMethod == null)
                {
                    usedTotalPaymentRecordByPayMethod = new EPaymentRecord();
                    usedTotalPaymentRecordByPayMethod.CurrencyID = ExchangeCurrency.DefaultCurrency();
                    usedTotalPaymentRecordByPayMethod.PayRecStatus = PaymentRecordStatus.PAYRECORDSTATUS_ACTIVE;
                    usedTotalPaymentRecordByPayMethod.PayRecType = paymentRecord.PayRecType;
                    usedTotalPaymentRecordByPayMethod.PayRecMethod = paymentRecord.PayRecMethod;
                    usedTotalPaymentRecordByPayMethod.EmpAccID = paymentRecord.EmpAccID;
                    usedTotalPaymentRecordByPayMethod.PaymentCodeID = 0;

                    usedTotalPaymentRecordByPayMethod.PayRecCalAmount = 0;

                    mergePaymentRecord.Add(usedTotalPaymentRecordByPayMethod);
                }

                usedTotalPaymentRecordByPayMethod.PayRecCalAmount += paymentAmount;
                usedTotalPaymentRecordByPayMethod.PayRecActAmount = System.Math.Round(usedTotalPaymentRecordByPayMethod.PayRecCalAmount, ExchangeCurrency.DefaultCurrencyDecimalPlaces(), MidpointRounding.AwayFromZero);
            }

            mergePaymentRecord.Sort(new PaymentRecordCompareByAmount(false));
            return mergePaymentRecord;

        }

        public void UndoPayroll(EEmpPayroll empPayroll)
        {


            DBFilter filter = new DBFilter();
            filter.add(new Match("EmpPayrollID", empPayroll.EmpPayrollID));
            ArrayList paymentRecords = EPaymentRecord.db.select(dbConn, filter);
            foreach (EPaymentRecord paymentRecord in paymentRecords)
            {
                if (paymentRecord.PayRecID > 0)
                {
                    DBFilter CNDFilter = new DBFilter();
                    CNDFilter.add(new Match("PayRecID", paymentRecord.PayRecID));
                    ArrayList CNDs = EClaimsAndDeductions.db.select(dbConn, CNDFilter);
                    foreach (EClaimsAndDeductions CND in CNDs)
                    {
                        CND.EmpPayrollID = null; ;
                        CND.PayRecID = null;
                        EClaimsAndDeductions.db.update(dbConn, CND);
                    }

                    //  Remove Payroll Process Compatibile with old rollback using PayRecID
                    DBFilter LeaveAppFilter = new DBFilter();
                    LeaveAppFilter.add(new Match("EmpPaymentID", paymentRecord.PayRecID));
                    ArrayList LeaveApps = ELeaveApplication.db.select(dbConn, LeaveAppFilter);
                    foreach (ELeaveApplication LeaveApp in LeaveApps)
                    {
                        LeaveApp.EmpPaymentID = 0;
                        ELeaveApplication.db.update(dbConn, LeaveApp);
                    }

                    ArrayList finalPaymentList = EEmpFinalPayment.db.select(dbConn, CNDFilter);
                    foreach (EEmpFinalPayment finalPayment in finalPaymentList)
                    {
                        finalPayment.PayRecID = null;
                        EEmpFinalPayment.db.update(dbConn, finalPayment);
                    }
                }
                EPaymentRecord.db.delete(dbConn, paymentRecord);

            }
            DBFilter CNDEmpPayrollFilter = new DBFilter();
            CNDEmpPayrollFilter.add(new Match("EmpPayrollID", empPayroll.EmpPayrollID));
            ArrayList CNDEmpPayrollList = EClaimsAndDeductions.db.select(dbConn, CNDEmpPayrollFilter);
            foreach (EClaimsAndDeductions CND in CNDEmpPayrollList)
            {
                CND.EmpPayrollID = null; ;
                CND.PayRecID = null;
                EClaimsAndDeductions.db.update(dbConn, CND);
            }

            //  Remove Payroll Process by updateing EmpPayrollID
            DBFilter LeaveAppEmpPayrollFilter = new DBFilter();
            LeaveAppEmpPayrollFilter.add(new Match("EmpPayrollID", empPayroll.EmpPayrollID));
            ArrayList LeaveAppEmpPayrollLists = ELeaveApplication.db.select(dbConn, LeaveAppEmpPayrollFilter);
            foreach (ELeaveApplication LeaveApp in LeaveAppEmpPayrollLists)
            {
                LeaveApp.EmpPayrollID = 0;
                ELeaveApplication.db.update(dbConn, LeaveApp);
            }
            MPFProcess.UndoMPF(dbConn, empPayroll);
            ORSOProcess.UndoORSO(dbConn, empPayroll);
            EEmpPayroll.db.delete(dbConn, empPayroll);
            RemoveEmptyPayrollBatch(empPayroll.PayBatchID);
        }

        private void RemoveEmptyPayrollBatch(int PayBatchID)
        {
            if (PayBatchID != 0)
            {
                DBFilter empPayrollFilter = new DBFilter();
                empPayrollFilter.add(new Match("PayBatchID", PayBatchID));
                if (EEmpPayroll.db.count(dbConn, empPayrollFilter) == 0)
                {
                    EPayrollBatch payBatch = new EPayrollBatch();
                    payBatch.PayBatchID = PayBatchID;
                    if (EPayrollBatch.db.select(dbConn, payBatch))
                        EPayrollBatch.db.delete(dbConn, payBatch);
                }
            }
        }

        public void PayrollProcessEnd(int PayPeriodID, int UserID)
        {
            EPayrollPeriod payPeriod = new EPayrollPeriod();
            payPeriod.PayPeriodID = PayPeriodID;
            EPayrollPeriod.db.select(dbConn, payPeriod);
            if (payPeriod.PayPeriodStatus != "T")
            {
                payPeriod.PayPeriodProcessEndDate = AppUtils.ServerDateTime();
                payPeriod.PayPeriodProcessEndBy = UserID;
                payPeriod.PayPeriodStatus = "E";  //Process End
                EPayrollPeriod.db.update(dbConn, payPeriod);
            }
        }

        public void RollBackPayroll(int PayPeriodID, int UserID)
        {


            EPayrollPeriod payPeriod = new EPayrollPeriod();
            payPeriod.PayPeriodID = PayPeriodID;
            if (EPayrollPeriod.db.select(dbConn, payPeriod))
            {
                DBFilter payPeriodFilter = new DBFilter();

                payPeriodFilter.add(new Match("payperiodid", PayPeriodID));
                ArrayList empPayrollList = EEmpPayroll.db.select(dbConn, payPeriodFilter);
                foreach (EEmpPayroll empPayroll in empPayrollList)
                    UndoPayroll(empPayroll);

                payPeriod.PayPeriodStatus = "N";
                payPeriod.PayPeriodRollbackBy = UserID;
                payPeriod.PayPeriodRollbackDate = AppUtils.ServerDateTime();
                EPayrollPeriod.db.update(dbConn, payPeriod);

                EPayrollGroup payGroup = new EPayrollGroup();
                payGroup.PayGroupID = payPeriod.PayGroupID;
                if (EPayrollGroup.db.select(dbConn, payGroup))

                    //  Delete Payroll Period if payroll period is auto generated
                    if (payGroup.CurrentPayPeriodID == payPeriod.PayPeriodID)
                    {
                        //  Check payroll again if 
                        empPayrollList = EEmpPayroll.db.select(dbConn, payPeriodFilter);
                        if (empPayrollList.Count == 0)
                        {
                            DBFilter payperiodFilter = new DBFilter();
                            //payperiodFilter.add(new Match("PayPeriodFr", "<", payPeriod.PayPeriodFr));
                            payperiodFilter.add(new Match("PayPeriodID", "<>", payPeriod.PayPeriodID));
                            payperiodFilter.add(new Match("PayPeriodIsAutoCreate", true));
                            payperiodFilter.add(new Match("PayGroupID", payPeriod.PayGroupID));
                            payperiodFilter.add("PayPeriodFr", false);

                            // remove payroll period if the period is not the first payroll cycle
                            ArrayList payPeriodList = EPayrollPeriod.db.select(dbConn, payperiodFilter);
                            if (payPeriodList.Count > 0)
                            {
                                EPayrollPeriod previousPayrollPeriod = (EPayrollPeriod)payPeriodList[0];
                                EPayrollPeriod.db.delete(dbConn, payPeriod);

                                payGroup.CurrentPayPeriodID = previousPayrollPeriod.PayPeriodID;
                                payGroup.PayGroupNextStartDate = payPeriod.PayPeriodFr;
                                payGroup.PayGroupNextEndDate = payPeriod.PayPeriodTo;
                                EPayrollGroup.db.update(dbConn, payGroup);
                                //  Change PayPeriodStatus from "E" to "C"
                                previousPayrollPeriod.PayPeriodStatus = "C";
                                EPayrollPeriod.db.update(dbConn, previousPayrollPeriod);
                            }
                        }
                    }
            }

        }

        public EPayrollPeriod GenerateNextPayrollPeriod(int PayGroupID)
        {


            EPayrollGroup payrollGroup = new EPayrollGroup();
            payrollGroup.PayGroupID = PayGroupID;
            if (EPayrollGroup.db.select(dbConn, payrollGroup))
            {
                if (payrollGroup.CurrentPayPeriodID != 0)
                {
                    EPayrollPeriod payPeriod = new EPayrollPeriod();
                    payPeriod.PayPeriodID = payrollGroup.CurrentPayPeriodID;
                    EPayrollPeriod.db.select(dbConn, payPeriod);

                    if (payPeriod.PayPeriodStatus == EPayrollPeriod.PAYPERIOD_STATUS_PROCESSEND_FLAG)
                    {
                        payrollGroup.CurrentPayPeriodID = 0;
                    }
                }
                if (payrollGroup.CurrentPayPeriodID == 0)
                {
                    EPayrollPeriod payrollPeriod = new EPayrollPeriod();
                    payrollPeriod.PayGroupID = payrollGroup.PayGroupID;
                    payrollPeriod.PayPeriodFr = payrollGroup.PayGroupNextStartDate;
                    payrollPeriod.PayPeriodTo = payrollGroup.PayGroupNextEndDate;
                    DateTime defaultLeaveCutOffDate = payrollGroup.PayGroupNextEndDate;
                    {
                        int actualLeaveCutOffDay = payrollGroup.PayGroupLeaveDefaultCutOffDay;
                        if (payrollGroup.PayGroupFreq == "S")
                        {
                            if (payrollGroup.PayGroupNextStartDate.Day == payrollGroup.PayGroupDefaultStartDay)
                                actualLeaveCutOffDay = payrollGroup.PayGroupLeaveDefaultCutOffDay;
                            else if (payrollGroup.PayGroupNextStartDate.Day == payrollGroup.PayGroupDefaultNextStartDay)
                                actualLeaveCutOffDay = payrollGroup.PayGroupLeaveDefaultNextCutOffDay;
                            else if (payrollGroup.PayGroupDefaultStartDay > 28)
                                actualLeaveCutOffDay = payrollGroup.PayGroupLeaveDefaultCutOffDay;
                            else
                                actualLeaveCutOffDay = payrollGroup.PayGroupLeaveDefaultNextCutOffDay;


                        }


                        while (defaultLeaveCutOffDate.Day !=
                            (actualLeaveCutOffDay > DateTime.DaysInMonth(defaultLeaveCutOffDate.Year, defaultLeaveCutOffDate.Month) ? DateTime.DaysInMonth(defaultLeaveCutOffDate.Year, defaultLeaveCutOffDate.Month) : actualLeaveCutOffDay))
                            defaultLeaveCutOffDate = defaultLeaveCutOffDate.AddDays(-1);
                    }

                    payrollPeriod.PayPeriodLeaveCutOffDate = defaultLeaveCutOffDate;
                    payrollPeriod.PayPeriodAttnFr = payrollGroup.PayGroupNextStartDate;
                    payrollPeriod.PayPeriodAttnTo = payrollGroup.PayGroupNextEndDate;
                    payrollPeriod.PayPeriodStatus = EPayrollPeriod.PAYPERIOD_STATUS_NORMAL_FLAG;
                    payrollPeriod.PayPeriodIsAutoCreate = true;
                    DateTime dtNextStartDate = payrollGroup.PayGroupNextEndDate.AddDays(1);
                    DateTime dtNextEndDate = dtNextStartDate.AddDays(1);

                    int actualNextStartDay = payrollGroup.PayGroupDefaultStartDay;

                    if (payrollGroup.PayGroupFreq == "S")
                    {
                        if (dtNextStartDate.Day == payrollGroup.PayGroupDefaultStartDay)
                            actualNextStartDay = payrollGroup.PayGroupDefaultNextStartDay;
                        else if (dtNextStartDate.Day == payrollGroup.PayGroupDefaultNextStartDay)
                            actualNextStartDay = payrollGroup.PayGroupDefaultStartDay;
                        else if (payrollGroup.PayGroupDefaultStartDay > 28)
                            actualNextStartDay = payrollGroup.PayGroupDefaultNextStartDay;
                        else
                            actualNextStartDay = payrollGroup.PayGroupDefaultStartDay;
                    }
                    int actualNextEndDay = ((actualNextStartDay - 1) + 30) % 31 + 1;
                    while (dtNextEndDate.Day !=
                        (actualNextEndDay > DateTime.DaysInMonth(dtNextEndDate.Year, dtNextEndDate.Month) ? DateTime.DaysInMonth(dtNextEndDate.Year, dtNextEndDate.Month) : actualNextEndDay))
                        dtNextEndDate = dtNextEndDate.AddDays(1);

                    payrollGroup.PayGroupNextStartDate = dtNextStartDate;
                    payrollGroup.PayGroupNextEndDate = dtNextEndDate;


                    EPayrollPeriod.db.insert(dbConn, payrollPeriod);
                    payrollGroup.CurrentPayPeriodID = payrollPeriod.PayPeriodID;
                    EPayrollGroup.db.update(dbConn, payrollGroup);
                    return payrollPeriod;
                }
            }
            return null;
        }

        public static double GetYearOfServer(DatabaseConnection dbConn, int EmpID, DateTime AsOfDate)
        {
            EEmpPersonalInfo empInfo = new EEmpPersonalInfo();
            empInfo.EmpID = EmpID;
            if (!EEmpPersonalInfo.db.select(dbConn, empInfo))
            {
                return 0;
            }


            DateTime tmpYearOfService = empInfo.EmpServiceDate;

            if (tmpYearOfService > AsOfDate)
                return 0;

            return Utility.YearDifference(tmpYearOfService, AsOfDate);

            //int intYearDiff;
            //intYearDiff = AsOfDate.Year - tmpYearOfService.Year;
            //if (tmpYearOfService.AddYears(intYearDiff) > AsOfDate)
            //    intYearDiff--;
            //int intDaysDiff = AsOfDate.Subtract(tmpYearOfService.AddYears(intYearDiff)).Days;
            //int intTotalDaysPerYear = tmpYearOfService.AddYears(intYearDiff + 1).Subtract(tmpYearOfService.AddYears(intYearDiff)).Days;

            //return (double)intYearDiff + (double)intDaysDiff / (double)intTotalDaysPerYear;
        }

        private bool IsFinalPayment(int EmpID, DateTime periodFrom, DateTime periodTo)
        {
            EEmpTermination empTerm = EEmpTermination.GetObjectByEmpID(dbConn, EmpID);
            if (empTerm == null)
                return false;
            else if (empTerm.EmpTermLastDate >= periodFrom && empTerm.EmpTermLastDate <= periodTo)
                return true;
            else
                return false;

        }

        private static EPayrollPeriod getPayrollPeriodByDate(DatabaseConnection dbConn, int PayrollGroupID, DateTime asOfDate)
        {
            DBFilter payPeriodFilter = new DBFilter();
            payPeriodFilter.add(new Match("PayGroupID", PayrollGroupID));
            payPeriodFilter.add(new Match("PayPeriodFr", "<=", asOfDate));
            payPeriodFilter.add(new Match("PayPeriodTo", ">=", asOfDate));
            ArrayList payPeriodList = EPayrollPeriod.db.select(dbConn, payPeriodFilter);
            if (payPeriodList.Count > 0)
                return (EPayrollPeriod)payPeriodList[0];
            else
                return null;
        }

        public static EPayrollPeriod GenerateDummyPayrollPeriod(DatabaseConnection dbConn, int PayrollGroupID, DateTime AsOfDate)
        {

            EPayrollPeriod payPeriod = getPayrollPeriodByDate(dbConn, PayrollGroupID, AsOfDate);
            if (payPeriod != null)
                return payPeriod;


            DateTime dtNextStartDate = new DateTime();
            DateTime dtNextEndDate = new DateTime();
            EPayrollGroup payrollGroup = new EPayrollGroup();
            payrollGroup.PayGroupID = PayrollGroupID;


            if (EPayrollGroup.db.select(dbConn, payrollGroup))
            {

                EPayrollPeriod payrollPeriod = new EPayrollPeriod();
                payrollPeriod.PayGroupID = payrollGroup.PayGroupID;
                payrollPeriod.PayPeriodFr = payrollGroup.PayGroupNextStartDate;
                payrollPeriod.PayPeriodTo = payrollGroup.PayGroupNextEndDate;
                payrollPeriod.PayPeriodLeaveCutOffDate = payrollGroup.PayGroupNextEndDate;
                payrollPeriod.PayPeriodAttnFr = payrollGroup.PayGroupNextStartDate;
                payrollPeriod.PayPeriodAttnTo = payrollGroup.PayGroupNextEndDate;
                payrollPeriod.PayPeriodStatus = "N";

                dtNextStartDate = payrollPeriod.PayPeriodFr;
                dtNextEndDate = payrollPeriod.PayPeriodTo;

                while ((AsOfDate < dtNextStartDate))
                {
                    dtNextEndDate = dtNextStartDate.AddDays(-1);
                    dtNextStartDate = dtNextEndDate.AddDays(-1);

                    int actualNextStartDay = payrollGroup.PayGroupDefaultStartDay;

                    if (payrollGroup.PayGroupFreq == "S")
                    {
                        if (dtNextStartDate.Day == payrollGroup.PayGroupDefaultStartDay)
                            actualNextStartDay = payrollGroup.PayGroupDefaultNextStartDay;
                        else if (dtNextStartDate.Day == payrollGroup.PayGroupDefaultNextStartDay)
                            actualNextStartDay = payrollGroup.PayGroupDefaultStartDay;
                        else if (payrollGroup.PayGroupDefaultStartDay > 28)
                            actualNextStartDay = payrollGroup.PayGroupDefaultNextStartDay;
                        else
                            actualNextStartDay = payrollGroup.PayGroupDefaultStartDay;
                    }
                    while (dtNextStartDate.Day !=
                        (actualNextStartDay > DateTime.DaysInMonth(dtNextStartDate.Year, dtNextStartDate.Month) ? DateTime.DaysInMonth(dtNextStartDate.Year, dtNextStartDate.Month) : actualNextStartDay))
                        dtNextStartDate = dtNextStartDate.AddDays(-1);
                }


                while (AsOfDate > dtNextEndDate)
                {
                    dtNextStartDate = dtNextEndDate.AddDays(1);
                    dtNextEndDate = dtNextStartDate.AddDays(1);

                    int actualNextStartDay = payrollGroup.PayGroupDefaultStartDay;

                    if (payrollGroup.PayGroupFreq == "S")
                    {
                        if (dtNextStartDate.Day == payrollGroup.PayGroupDefaultStartDay)
                            actualNextStartDay = payrollGroup.PayGroupDefaultNextStartDay;
                        else if (dtNextStartDate.Day == payrollGroup.PayGroupDefaultNextStartDay)
                            actualNextStartDay = payrollGroup.PayGroupDefaultStartDay;
                        else if (payrollGroup.PayGroupDefaultStartDay > 28)
                            actualNextStartDay = payrollGroup.PayGroupDefaultNextStartDay;
                        else
                            actualNextStartDay = payrollGroup.PayGroupDefaultStartDay;
                    }
                    int actualNextEndDay = ((actualNextStartDay - 1) + 30) % 31 + 1;
                    while (dtNextEndDate.Day !=
                        (actualNextEndDay > DateTime.DaysInMonth(dtNextEndDate.Year, dtNextEndDate.Month) ? DateTime.DaysInMonth(dtNextEndDate.Year, dtNextEndDate.Month) : actualNextEndDay))
                        dtNextEndDate = dtNextEndDate.AddDays(1);
                }

                if (AsOfDate >= dtNextStartDate && AsOfDate <= dtNextEndDate)
                {
                    payrollPeriod.PayPeriodFr = dtNextStartDate;
                    payrollPeriod.PayPeriodTo = dtNextEndDate;
                }
                else
                    payrollPeriod = null;
                return payrollPeriod;
            }
            else
                return null;
        }

        public static void UpdateEmpPayrollValueDate(DatabaseConnection dbConn, ArrayList PayrollBatchList, ArrayList EmpList, DateTime ValueDate, bool UpdateAutoPay, bool UpdateCheque, bool UpdateCash, bool UpdateOthers)
        {
            foreach (EPayrollBatch payBatch in PayrollBatchList)
            {
                foreach (EEmpPersonalInfo empInfo in EmpList)
                {
                    DBFilter empPayrollFilter = new DBFilter();
                    empPayrollFilter.add(new Match("EmpID", empInfo.EmpID));
                    empPayrollFilter.add(new Match("PayBatchID", payBatch.PayBatchID));
                    ArrayList empPayrollList = EEmpPayroll.db.select(dbConn, empPayrollFilter);
                    foreach (EEmpPayroll empPayroll in empPayrollList)
                    {
                        if (!empPayroll.EmpPayValueDate.Equals(ValueDate))
                        {

                            OR orPayRecMethodFilter = new OR();
                            if (UpdateAutoPay)
                                orPayRecMethodFilter.add(new Match("PayRecMethod", "A"));
                            if (UpdateCheque)
                                orPayRecMethodFilter.add(new Match("PayRecMethod", "Q"));
                            if (UpdateCash)
                                orPayRecMethodFilter.add(new Match("PayRecMethod", "C"));
                            if (UpdateOthers)
                                orPayRecMethodFilter.add(new Match("PayRecMethod", "O"));
                            DBFilter payRecFilter = new DBFilter();
                            payRecFilter.add(new Match("EmpPayrollID", empPayroll.EmpPayrollID));
                            payRecFilter.add(orPayRecMethodFilter);

                            if (EPaymentRecord.db.count(dbConn, payRecFilter) > 0)
                            {
                                empPayroll.EmpPayValueDate = ValueDate;
                                EEmpPayroll.db.update(dbConn, empPayroll);
                            }
                        }
                    }

                }
            }
        }


        public static double GetTotalEmpPayrollWorkingHours(DatabaseConnection dbConn, int EmpID, int PayrollPeriodID)
        {
            DBFilter empPayrollFilter = new DBFilter();
            empPayrollFilter.add(new Match("EmpID", EmpID));
            empPayrollFilter.add(new Match("PayPeriodID", "=", PayrollPeriodID));

            ArrayList empPayrollList = EEmpPayroll.db.select(dbConn, empPayrollFilter);

            double result = 0;
            foreach (EEmpPayroll empPayroll in empPayrollList)
            {
                result += empPayroll.EmpPayTotalWorkingHours;
            }

            //if (result.Equals(0) && dateTo >= payrollPeriod.PayPeriodTo)
            //{
            //    DBFilter attendanceRecordFilter = new DBFilter();
            //    attendanceRecordFilter.add(new Match("EmpID", EmpID));
            //    attendanceRecordFilter.add(new Match("AttendanceRecordDate", ">=", payrollPeriod.PayPeriodAttnFr));
            //    attendanceRecordFilter.add(new Match("AttendanceRecordDate", "<=", payrollPeriod.PayPeriodAttnTo)); ;

            //    ArrayList attendanceRecordList = EAttendanceRecord.db.select(dbConn, attendanceRecordFilter);
            //    foreach (EAttendanceRecord attendanceRecord in attendanceRecordList)
            //    {
            //        result += attendanceRecord.TotalWorkingHourTimeSpan().TotalHours;
            //    }
            //}

            return result;
        }

        private double GetTotalWorkingHours(int EmpID, EPayrollPeriod payrollPeriod, DateTime dateFrom, DateTime dateTo)
        {
            DBFilter empWorkingSummaryFilter = new DBFilter();
            empWorkingSummaryFilter.add(new Match("EmpID", EmpID));
            empWorkingSummaryFilter.add(new Match("EmpWorkingSummaryAsOfDate", ">=", dateFrom));
            empWorkingSummaryFilter.add(new Match("EmpWorkingSummaryAsOfDate", "<=", dateTo));

            ArrayList empWorkingSummaryList = EEmpWorkingSummary.db.select(dbConn, empWorkingSummaryFilter);

            double result = 0;
            foreach (EEmpWorkingSummary empWorkingSummary in empWorkingSummaryList)
            {
                result += empWorkingSummary.EmpWorkingSummaryTotalWorkingHours;
            }

            //if (result.Equals(0) && dateTo >= payrollPeriod.PayPeriodTo)
            //{
            //    DBFilter attendanceRecordFilter = new DBFilter();
            //    attendanceRecordFilter.add(new Match("EmpID", EmpID));
            //    attendanceRecordFilter.add(new Match("AttendanceRecordDate", ">=", payrollPeriod.PayPeriodAttnFr));
            //    attendanceRecordFilter.add(new Match("AttendanceRecordDate", "<=", payrollPeriod.PayPeriodAttnTo)); ;

            //    ArrayList attendanceRecordList = EAttendanceRecord.db.select(dbConn, attendanceRecordFilter);
            //    foreach (EAttendanceRecord attendanceRecord in attendanceRecordList)
            //    {
            //        result += attendanceRecord.TotalWorkingHourTimeSpan().TotalHours;
            //    }
            //}

            return result;
        }

        public static double GetTotalRestDayEntitled(DatabaseConnection dbConn, int EmpID, DateTime dateFrom, DateTime dateTo, bool UseDummyWorkingSummaryIfNotExists)
        {
            DBFilter empWorkingSummaryFilter = new DBFilter();
            empWorkingSummaryFilter.add(new Match("EmpID", EmpID));
            empWorkingSummaryFilter.add(new Match("EmpWorkingSummaryAsOfDate", ">=", dateFrom));
            empWorkingSummaryFilter.add(new Match("EmpWorkingSummaryAsOfDate", "<=", dateTo));

            ArrayList empWorkingSummaryList = EEmpWorkingSummary.db.select(dbConn, empWorkingSummaryFilter);

            double result = 0;
            if (empWorkingSummaryList.Count == 0 && UseDummyWorkingSummaryIfNotExists)
            {
                EEmpPersonalInfo empInfo = new EEmpPersonalInfo();
                empInfo.EmpID = EmpID;
                ArrayList list = new ArrayList();
                list.Add(empInfo);
                System.Data.DataTable table = HROne.Import.ImportEmpWorkingSummaryProcess.GenerateTemplate(dbConn, list, dateFrom, dateTo);
                foreach (DataRow row in table.Rows)
                {
                    result += (double)row[HROne.Import.ImportEmpWorkingSummaryProcess.FIELD_REST_DAY_ENTITLED];
                }
                return result;
            }
            foreach (EEmpWorkingSummary empWorkingSummary in empWorkingSummaryList)
            {
                result += empWorkingSummary.EmpWorkingSummaryRestDayEntitled;
            }
            return result;
        }

        public static double GetTotalRestDayTaken(DatabaseConnection dbConn, int EmpID, DateTime dateFrom, DateTime dateTo)
        {
            DBFilter empWorkingSummaryFilter = new DBFilter();
            empWorkingSummaryFilter.add(new Match("EmpID", EmpID));
            empWorkingSummaryFilter.add(new Match("EmpWorkingSummaryAsOfDate", ">=", dateFrom));
            empWorkingSummaryFilter.add(new Match("EmpWorkingSummaryAsOfDate", "<=", dateTo));

            ArrayList empWorkingSummaryList = EEmpWorkingSummary.db.select(dbConn, empWorkingSummaryFilter);

            double result = 0;
            foreach (EEmpWorkingSummary empWorkingSummary in empWorkingSummaryList)
            {
                result += empWorkingSummary.EmpWorkingSummaryRestDayTaken;
            }
            return result;
        }

        private static double GetTotalWorkingDays(DatabaseConnection dbConn, int EmpID, DateTime dateFrom, DateTime dateTo)
        {
            DBFilter empWorkingSummaryFilter = new DBFilter();
            empWorkingSummaryFilter.add(new Match("EmpID", EmpID));
            empWorkingSummaryFilter.add(new Match("EmpWorkingSummaryAsOfDate", ">=", dateFrom));
            empWorkingSummaryFilter.add(new Match("EmpWorkingSummaryAsOfDate", "<=", dateTo));

            ArrayList empWorkingSummaryList = EEmpWorkingSummary.db.select(dbConn, empWorkingSummaryFilter);

            double result = 0;
            foreach (EEmpWorkingSummary empWorkingSummary in empWorkingSummaryList)
                result += empWorkingSummary.EmpWorkingSummaryTotalWorkingDays;
            return result;
        }

        public static double GetTotalWagesWithoutRestDayPayment(DatabaseConnection dbConn, int EmpID, DateTime dateFrom, DateTime dateTo, ArrayList TrialRunPaymentRecordList)
        {
            double result = 0;
            DBFilter EmpPayrollFilter = new DBFilter();
            EmpPayrollFilter.add(new Match("EmpID", "=", EmpID));

            DBFilter PaylPeriodFilter = new DBFilter();
            PaylPeriodFilter.add(new Match("payPeriodFr", "<=", dateTo));
            PaylPeriodFilter.add(new Match("payPeriodTo", ">=", dateFrom));
            IN PaylPeriodInFilter = new IN("PayPeriodID", "Select PayPeriodID from PayrollPeriod", PaylPeriodFilter);
            EmpPayrollFilter.add(PaylPeriodInFilter);

            ArrayList empPayrolls = EEmpPayroll.db.select(dbConn, EmpPayrollFilter);


            foreach (EEmpPayroll empPayroll in empPayrolls)
            {

                //  Get Related Payment Record

                DBFilter paymentRecordFilter = new DBFilter();
                paymentRecordFilter.add(new Match("EmpPayrollID", empPayroll.EmpPayrollID));
                paymentRecordFilter.add(new Match("PayRecIsRestDayPayment", false));
                //paymentRecordFilter.add(paymentCodeInFilter);

                ArrayList paymentRecords = EPaymentRecord.db.select(dbConn, paymentRecordFilter);

                foreach (EPaymentRecord paymentRecord in paymentRecords)
                {
                    EPaymentCode paymentCode = new EPaymentCode();
                    paymentCode.PaymentCodeID = paymentRecord.PaymentCodeID;
                    if (EPaymentCode.db.select(dbConn, paymentCode))
                    {
                        if (paymentCode.PaymentCodeIsWages)
                            result += paymentRecord.PayRecActAmount;
                    }
                }
            }

            if (TrialRunPaymentRecordList != null)
                foreach (EPaymentRecord paymentRecord in TrialRunPaymentRecordList)
                {
                    if (paymentRecord.EmpPayrollID <= 0 && !paymentRecord.PayRecIsRestDayPayment)
                    {
                        EPaymentCode paymentCode = new EPaymentCode();
                        paymentCode.PaymentCodeID = paymentRecord.PaymentCodeID;
                        if (EPaymentCode.db.select(dbConn, paymentCode))
                        {
                            if (paymentCode.PaymentCodeIsWages)
                                result += paymentRecord.PayRecActAmount;
                        }
                    }
                }

            return result;
        }
        public static double GetTotalLunchTime(DatabaseConnection dbConn, int EmpID, DateTime dateFrom, DateTime dateTo)
        {
            DBFilter empWorkingSummaryFilter = new DBFilter();
            empWorkingSummaryFilter.add(new Match("EmpID", EmpID));
            empWorkingSummaryFilter.add(new Match("EmpWorkingSummaryAsOfDate", ">=", dateFrom));
            empWorkingSummaryFilter.add(new Match("EmpWorkingSummaryAsOfDate", "<=", dateTo));

            ArrayList empWorkingSummaryList = EEmpWorkingSummary.db.select(dbConn, empWorkingSummaryFilter);

            double result = 0;
            foreach (EEmpWorkingSummary empWorkingSummary in empWorkingSummaryList)
            {
                result += empWorkingSummary.EmpWorkingSummaryTotalLunchTimeHours;
            }
            return result;
        }

        public static double GetMinimumWages(DatabaseConnection dbConn, int EmpID, DateTime referenceDate)
        {
            EEmpPersonalInfo empInfo = new EEmpPersonalInfo();
            empInfo.EmpID = EmpID;
            if (EEmpPersonalInfo.db.select(dbConn, empInfo))
            {
                if (empInfo.EmpIsOverrideMinimumWage)
                    return empInfo.EmpOverrideMinimumHourlyRate;
            }
            DBFilter minimumWagesFilter = new DBFilter();
            minimumWagesFilter.add(new Match("MinimumWageEffectiveDate", "<=", referenceDate));
            minimumWagesFilter.add("MinimumWageEffectiveDate", false);
            ArrayList minimumWageList = EMinimumWage.db.select(dbConn, minimumWagesFilter);
            if (minimumWageList.Count > 0)
                return ((EMinimumWage)minimumWageList[0]).MinimumWageHourlyRate;

            return 0;
        }

        public static void InsertAdditionalRemuneration(DatabaseConnection dbConn, EEmpPayroll empPayroll)
        {
            EPayrollPeriod payPeriod = new EPayrollPeriod();
            payPeriod.PayPeriodID = empPayroll.PayPeriodID;
            if (EPayrollPeriod.db.select(dbConn, payPeriod))
            {
                DBFilter paymentFilter = new DBFilter();
                paymentFilter.add(new Match("EmpPayrollID", empPayroll.EmpPayrollID));
                ArrayList paymentRecords = EPaymentRecord.db.select(dbConn, paymentFilter);

                PayrollProcess payrollProcess = new PayrollProcess(dbConn);
                ArrayList additionalRemunerationList = payrollProcess.AdditionalRemunerationTrialRun(empPayroll.EmpID, payPeriod, paymentRecords, GetTotalEmpPayrollWorkingHours(dbConn, empPayroll.EmpID, empPayroll.PayPeriodID));
                foreach (EPaymentRecord paymentRecord in additionalRemunerationList)
                {
                    paymentRecord.EmpPayrollID = empPayroll.EmpPayrollID;
                    if (!empPayroll.EmpPayStatus.Equals("T"))
                        paymentRecord.PayRecType = "A";
                    EPaymentRecord.db.insert(dbConn, paymentRecord);
                }

            }
        }
        //public static double GetProrataAmount(dbConn, int EmpID, EPayrollGroup payrollGroup, EPayrollPeriod payrollPeriod, DateTime dateFrom, DateTime dateTo, EPaymentCode paymentCode, double Amount, double numOfRestDay, bool IsFinalPayment, out string Remark)
        //{
        //    EEmpPersonalInfo empInfo = new EEmpPersonalInfo();
        //    empInfo.EmpID = EmpID;
        //    if (EEmpPersonalInfo.db.select(dbConn, empInfo))
        //    {
        //        return GetProrataAmount(dbConn, empInfo, payrollGroup, payrollPeriod, dateFrom, dateTo, paymentCode, Amount, numOfRestDay, IsFinalPayment, out Remark);

        //    }
        //}
        public static double GetProrataAmount(DatabaseConnection dbConn, EEmpPersonalInfo empPersonalInfo, EPayrollGroup payrollGroup, EPayrollPeriod payrollPeriod, DateTime dateFrom, DateTime dateTo, EPaymentCode paymentCode, double Amount, double numOfRestDay, bool IsFinalPayment, out string Remark)
        {
            double TotalAmount;

            if (dateFrom < empPersonalInfo.EmpDateOfJoin)
                dateFrom = empPersonalInfo.EmpDateOfJoin;
            double DaysCount = dateTo.Subtract(dateFrom).Days + 1;
            if (!payrollGroup.PayGroupRestDayHasWage)
                DaysCount -= numOfRestDay;

            if (dateTo.Subtract(dateFrom).Days == payrollPeriod.PayPeriodTo.Subtract(payrollPeriod.PayPeriodFr).Days || !paymentCode.PaymentCodeIsProrata)
            {

                //if (empPersonalInfo.EmpDateOfJoin <= dateFrom || !paymentCode.PaymentCodeIsProrata)
                //{
                // Full Paid
                TotalAmount = Amount;
                Remark = TotalAmount.ToString("$#,##0.00##"); //+ " x 1";

                //}
                //else
                //{
                //    // New Join Prorata
                //    string formulaRemark;
                //    bool IsDAW = false; //  dummy use
                //    double dailyProrata = PayrollFormula.DailyProrataCaluclation(dbConn, payrollGroup.PayGroupNewJoinFormula, payrollGroup.PayGroupDefaultProrataFormula, EmpID, Amount, payrollPeriod.PayPeriodFr, payrollPeriod.PayPeriodTo, payrollGroup.NumOfPeriodPerYear(), payrollPeriod.PayPeriodTo, out formulaRemark, out IsDAW);

                //    double numOfDays = dateTo.Subtract(empPersonalInfo.EmpDateOfJoin).Days + 1;
                //    if (!payrollGroup.PayGroupRestDayHasWage)
                //        numOfDays -= numOfRestDay;
                //    TotalAmount = dailyProrata * numOfDays;
                //    Remark = formulaRemark
                //        + (numOfDays.Equals(1) ? string.Empty : " x " + numOfDays);

                //}
            }
            else
            {
                // Calculated in prorata basis

                string formulaRemark;
                double dailyProrata = 0;
                if (empPersonalInfo.EmpDateOfJoin < dateFrom)
                {
                    bool IsDAW = false;     //  dummy use only
                    if (IsFinalPayment)
                        dailyProrata = PayrollFormula.DailyProrataCaluclation(dbConn, payrollGroup.PayGroupTerminatedFormula, payrollGroup.PayGroupDefaultProrataFormula, empPersonalInfo.EmpID, Amount, payrollPeriod.PayPeriodFr, payrollPeriod.PayPeriodTo, payrollGroup.NumOfPeriodPerYear(), payrollPeriod.PayPeriodTo, out formulaRemark, out IsDAW);
                    else
                        dailyProrata = PayrollFormula.DailyProrataCaluclation(dbConn, payrollGroup.PayGroupExistingFormula, payrollGroup.PayGroupDefaultProrataFormula, empPersonalInfo.EmpID, Amount, payrollPeriod.PayPeriodFr, payrollPeriod.PayPeriodTo, payrollGroup.NumOfPeriodPerYear(), payrollPeriod.PayPeriodTo, out formulaRemark, out IsDAW);
                    TotalAmount = dailyProrata * DaysCount;
                    Remark = formulaRemark
                        + (DaysCount.Equals(1) ? string.Empty : " x " + DaysCount.ToString());

                }
                else
                {
                    bool IsDAW = false;     //  dummy use only
                    if (IsFinalPayment)
                        dailyProrata = PayrollFormula.DailyProrataCaluclation(dbConn, payrollGroup.PayGroupTerminatedFormula, payrollGroup.PayGroupDefaultProrataFormula, empPersonalInfo.EmpID, Amount, payrollPeriod.PayPeriodFr, payrollPeriod.PayPeriodTo, payrollGroup.NumOfPeriodPerYear(), payrollPeriod.PayPeriodTo, out formulaRemark, out IsDAW);
                    else
                        dailyProrata = PayrollFormula.DailyProrataCaluclation(dbConn, payrollGroup.PayGroupNewJoinFormula, payrollGroup.PayGroupDefaultProrataFormula, empPersonalInfo.EmpID, Amount, payrollPeriod.PayPeriodFr, payrollPeriod.PayPeriodTo, payrollGroup.NumOfPeriodPerYear(), payrollPeriod.PayPeriodTo, out formulaRemark, out IsDAW);
                    TotalAmount = dailyProrata * DaysCount;
                    Remark = formulaRemark
                        + (DaysCount.Equals(1) ? string.Empty : " x " + DaysCount.ToString());
                    //actualDateFrom = empPersonalInfo.EmpDateOfJoin;
                }

            }
            return TotalAmount;
        }
    }
}