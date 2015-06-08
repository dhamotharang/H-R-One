using System;
using System.Data;
using System.Configuration;
using System.Collections;
using HROne.Lib;
using HROne.Lib.Entities;
using HROne.DataAccess;

namespace HROne.Payroll
{
    public abstract class ORSOProcess
    {

        public enum ORSOJoinType
        { 
            NewJoin = 1,
            Existing = 2,
            //            Terminated = 3

        }



        public static ArrayList ORSOTrialRun(DatabaseConnection dbConn, int EmpID, EPayrollPeriod payrollPeriod, ArrayList paymentRecords, EMPFRecord mpfRecord)
        {
            ArrayList orsoRecords = new ArrayList();

            DBFilter dbFilter = new DBFilter();
            dbFilter.add(new Match("EmpID", EmpID));
            dbFilter.add(new Match("EmpORSOEffFr", "<=", payrollPeriod.PayPeriodTo));
            dbFilter.add("EmpORSOEffFr", true);
            ArrayList oEmpORSOs = EEmpORSOPlan.db.select(dbConn, dbFilter);

            if (oEmpORSOs.Count > 0)
            {
                EEmpORSOPlan oEmpORSO = (EEmpORSOPlan)oEmpORSOs[0];

                DateTime ORSOJoinDate = oEmpORSO.EmpORSOEffFr;
                
                if (ORSOJoinDate <= payrollPeriod.PayPeriodTo && ORSOJoinDate >= payrollPeriod.PayPeriodFr)
                {
                    ArrayList newJoinORSORecords = CreateNewJoinORSORecords(dbConn, EmpID, oEmpORSO.EmpORSOEffFr, ORSOJoinDate);
                    if (newJoinORSORecords != null)
                        orsoRecords.AddRange(newJoinORSORecords);
                    orsoRecords.Add(CreateORSORecord(dbConn, EmpID, payrollPeriod, paymentRecords, oEmpORSO.EmpORSOEffFr, ORSOJoinType.NewJoin, mpfRecord));
                }
                else if (ORSOJoinDate < payrollPeriod.PayPeriodFr)
                {
                    orsoRecords.Add(CreateORSORecord(dbConn, EmpID, payrollPeriod, paymentRecords, oEmpORSO.EmpORSOEffFr, ORSOJoinType.Existing, mpfRecord));
                }
                else if (payrollPeriod.PayPeriodTo < ORSOJoinDate)
                {


                }
                else
                {


                }

            }
            return orsoRecords;
        }

        private static ArrayList CreateNewJoinORSORecords(DatabaseConnection dbConn, int EmpID, DateTime ORSOJoinDate, DateTime AsOfDate)
        {
            ArrayList newJoinORSORecords = new ArrayList();

            DBFilter empPayrollFilter = new DBFilter();
            empPayrollFilter.add(new Match("EmpID", EmpID));
            IN inEmpPayrollFilter = new IN("PayPeriodID", "Select Distinct PayPeriodID from EmpPayroll ", empPayrollFilter);

            DBFilter payPeriodFilter = new DBFilter();
            payPeriodFilter.add(inEmpPayrollFilter);
            payPeriodFilter.add(new Match("PayPeriodTo", "<", AsOfDate));
            payPeriodFilter.add(new Match("PayPeriodTo", ">=", ORSOJoinDate));
            payPeriodFilter.add("PayPeriodFr", true);
            ArrayList payPeriods = EPayrollPeriod.db.select(dbConn, payPeriodFilter);

            foreach (EPayrollPeriod payrollPeriod in payPeriods)
            {

                DBFilter payPeriodFilter2 = new DBFilter();
                payPeriodFilter2.add(new Match("PayPeriodID", payrollPeriod.PayPeriodID));
                payPeriodFilter2.add(new Match("EmpID", EmpID));
                IN inEmpPayrollFilter2 = new IN("EmpPayrollID", "Select EmpPayrollID from EmpPayroll", payPeriodFilter2);

                DBFilter paymentRecordFilter = new DBFilter();
                paymentRecordFilter.add(inEmpPayrollFilter2);

                ArrayList paymentRecords = EPaymentRecord.db.select(dbConn, paymentRecordFilter);

                newJoinORSORecords.Add(CreateORSORecord(dbConn, EmpID, payrollPeriod, paymentRecords, ORSOJoinDate, ORSOJoinType.NewJoin, null));

            }

            if (newJoinORSORecords.Count == 0)
                return null;
            return newJoinORSORecords;
        }

        //private static double CalculateProrataFactor(int EmpID, EPayrollPeriod payrollPeriod)
        //{
        //    EEmpPersonalInfo oEmp = new EEmpPersonalInfo();
        //    oEmp.EmpID = EmpID;
        //    EEmpPersonalInfo.db.select(dbConn, oEmp);

        //    DateTime dt1AgeMin = oEmp.EmpDateOfBirth.AddYears(AGE_MINIMUM);
        //    DateTime dt1AgeMax = oEmp.EmpDateOfBirth.AddYears(AGE_MAXIMUM);

        //    EEmpTermination empTermination = EEmpTermination.GetObjectByEmpID(dbConn, EmpID);
        //    bool blnTerminated;
        //    if (empTermination != null)
        //    {
        //        if (empTermination.EmpTermLastDate <= payrollPeriod.PayPeriodTo && empTermination.EmpTermLastDate >= payrollPeriod.PayPeriodFr)
        //            blnTerminated = true;
        //        else
        //            blnTerminated = false;
        //    }
        //    else
        //        blnTerminated = false;

        //    double prorataFactor = 1;
        //    if (oEmp.EmpDateOfJoin < dt1AgeMin)
        //    {
        //        if (dt1AgeMin <= payrollPeriod.PayPeriodTo && dt1AgeMin >= payrollPeriod.PayPeriodFr)
        //        {
        //            if (blnTerminated)
        //                prorataFactor = (double)(empTermination.EmpTermLastDate.Subtract(dt1AgeMin).Days + 1) / (empTermination.EmpTermLastDate.Subtract(payrollPeriod.PayPeriodFr > oEmp.EmpDateOfJoin ? payrollPeriod.PayPeriodFr : oEmp.EmpDateOfJoin).Days + 1);
        //            else
        //                prorataFactor = (double)(payrollPeriod.PayPeriodTo.Subtract(dt1AgeMin).Days + 1) / (payrollPeriod.PayPeriodTo.Subtract(payrollPeriod.PayPeriodFr > oEmp.EmpDateOfJoin ? payrollPeriod.PayPeriodFr : oEmp.EmpDateOfJoin).Days + 1);
        //        }
        //        if (dt1AgeMin >= payrollPeriod.PayPeriodTo)
        //            prorataFactor = 0;
        //    }

        //    if (dt1AgeMax <= payrollPeriod.PayPeriodTo && dt1AgeMax >= payrollPeriod.PayPeriodFr)
        //    {
        //        if (blnTerminated)
        //        {
        //            if (empTermination.EmpTermLastDate < dt1AgeMax)
        //                prorataFactor = 1;
        //            else
        //                prorataFactor = 1.0 - (double)(empTermination.EmpTermLastDate.Subtract(dt1AgeMax).Days + 1) / (empTermination.EmpTermLastDate.Subtract(payrollPeriod.PayPeriodFr > oEmp.EmpDateOfJoin ? payrollPeriod.PayPeriodFr : oEmp.EmpDateOfJoin).Days + 1);
        //        }
        //        else
        //            prorataFactor = 1.0 - (double)(payrollPeriod.PayPeriodTo.Subtract(dt1AgeMax).Days + 1) / (payrollPeriod.PayPeriodTo.Subtract(payrollPeriod.PayPeriodFr > oEmp.EmpDateOfJoin ? payrollPeriod.PayPeriodFr : oEmp.EmpDateOfJoin).Days + 1);
        //    }
        //    if (dt1AgeMax <= payrollPeriod.PayPeriodFr)
        //        prorataFactor = 0;

        //    return prorataFactor;
        //}

        private static EORSORecord CreateORSORecord(DatabaseConnection dbConn, int EmpID, EPayrollPeriod payrollPeriod, ArrayList paymentRecords, DateTime ORSOJoinDate, ORSOJoinType ORSOJoinType, EMPFRecord mpfRecord)
        {
            //double RIProrateFactor = CalculateProrataFactor(EmpID, payrollPeriod);
            EORSORecord orsoRecord = new EORSORecord();
            switch (ORSOJoinType)
            {
                case ORSOJoinType.NewJoin:
                    orsoRecord.ORSORecType = "N";
                    break;
                case ORSOJoinType.Existing:
                    orsoRecord.ORSORecType = "E";
                    break;
                //case ORSOJoinType.Terminated:
                //    ORSORecord.ORSORecType = "T";
                //break;
            }
            orsoRecord.ORSORecCalRI = 0;
            orsoRecord.ORSOPlanID = GetORSOPlanID(dbConn, EmpID, payrollPeriod.PayPeriodTo);

            if (orsoRecord.ORSOPlanID > 0)
            {
                EEmpPersonalInfo oEmp = new EEmpPersonalInfo();
                oEmp.EmpID = EmpID;
                EEmpPersonalInfo.db.select(dbConn, oEmp);

                //DateTime dt1Age18 = oEmp.EmpDateOfBirth.AddYears(18);
                //DateTime dt1Age65 = oEmp.EmpDateOfBirth.AddYears(65);

                EEmpTermination empTermination = EEmpTermination.GetObjectByEmpID(dbConn, EmpID);

                orsoRecord.ORSORecPeriodFr = payrollPeriod.PayPeriodFr;
                orsoRecord.ORSORecPeriodTo = payrollPeriod.PayPeriodTo;

                if (orsoRecord.ORSORecPeriodFr > oEmp.EmpDateOfJoin && orsoRecord.ORSORecPeriodTo < oEmp.EmpDateOfJoin)
                    orsoRecord.ORSORecPeriodFr = oEmp.EmpDateOfJoin;
                //if (ORSORecord.ORSORecPeriodFr > dt1Age18 && ORSORecord.ORSORecPeriodTo < dt1Age18)
                //    ORSORecord.ORSORecPeriodFr = dt1Age18;
                if (empTermination != null)
                {
                    // Start 0000186, Ricky So, 2016/04/15
                    // incomplete month --> no orso contribution
                    if (orsoRecord.ORSORecPeriodTo > empTermination.EmpTermLastDate && orsoRecord.ORSORecPeriodFr <= empTermination.EmpTermLastDate)
                    {
                        orsoRecord.ORSORecPeriodTo = empTermination.EmpTermLastDate;
                        orsoRecord.ORSORecActEE = 0;
                        orsoRecord.ORSORecActER = 0;
                        orsoRecord.ORSORecActRI = 0;
                        orsoRecord.ORSORecCalEE = 0;
                        orsoRecord.ORSORecCalER = 0;
                        orsoRecord.ORSORecCalRI = 0;
                        return orsoRecord;
                    }
                    // End 0000186, Ricky So, 2016/04/15
                    if (orsoRecord.ORSORecPeriodTo < empTermination.EmpTermLastDate && orsoRecord.ORSORecPeriodFr >= empTermination.EmpTermLastDate)
                    {
                        orsoRecord.ORSORecPeriodTo = empTermination.EmpTermLastDate;
                    }
                }
                //if (ORSORecord.ORSORecPeriodTo < dt1Age65 && ORSORecord.ORSORecPeriodFr > dt1Age65)
                //    ORSORecord.ORSORecPeriodTo = dt1Age65;


                foreach (EPaymentRecord paymentRecord in paymentRecords)
                {
                    EPaymentCode paymentCode = new EPaymentCode();
                    paymentCode.PaymentCodeID = paymentRecord.PaymentCodeID;
                    EPaymentCode.db.select(dbConn, paymentCode);
                    if (paymentCode.PaymentCodeIsORSO)
                    {
                        orsoRecord.ORSORecCalRI += paymentRecord.PayRecActAmount;
                    }
                }
                orsoRecord.ORSORecActRI = orsoRecord.ORSORecCalRI;

                if (orsoRecord.ORSOPlanID > 0)
                {

                    DBFilter oldORSORecordFilter = new DBFilter();

                    DBFilter empIDFilter = new DBFilter();
                    empIDFilter.add(new Match("EmpID", EmpID));

                    oldORSORecordFilter.add(new IN("EmpPayrollID", "Select EmpPayrollID from EMPPayroll ", empIDFilter));
                    oldORSORecordFilter.add(new Match("ORSORecPeriodFr", "<=", orsoRecord.ORSORecPeriodTo));
                    oldORSORecordFilter.add(new Match("ORSORecPeriodTo", ">=", orsoRecord.ORSORecPeriodFr));
                    ArrayList oldORSORecords = EORSORecord.db.select(dbConn, oldORSORecordFilter);

                    EORSORecord oldTotalORSORecord = new EORSORecord();
                    EORSORecord newTotalORSORecord = new EORSORecord();

                    foreach (EORSORecord oldORSORecord in oldORSORecords)
                    {
                        oldTotalORSORecord.ORSORecActRI += oldORSORecord.ORSORecActRI;
                        oldTotalORSORecord.ORSORecActER += oldORSORecord.ORSORecActER;
                        oldTotalORSORecord.ORSORecActEE += oldORSORecord.ORSORecActEE;

                    }


                    newTotalORSORecord.ORSORecCalRI = oldTotalORSORecord.ORSORecActRI + orsoRecord.ORSORecCalRI;

                    newTotalORSORecord.ORSORecCalER = CalculateERAmount(dbConn, oEmp, orsoRecord.ORSOPlanID, newTotalORSORecord.ORSORecCalRI, payrollPeriod, mpfRecord);
                    newTotalORSORecord.ORSORecCalEE = CalculateEEAmount(dbConn, oEmp, orsoRecord.ORSOPlanID, newTotalORSORecord.ORSORecCalRI, payrollPeriod, ORSOJoinDate, mpfRecord);

                    orsoRecord.ORSORecCalER = newTotalORSORecord.ORSORecCalER - oldTotalORSORecord.ORSORecActER;
                    orsoRecord.ORSORecCalEE = newTotalORSORecord.ORSORecCalEE - oldTotalORSORecord.ORSORecActEE;



                    orsoRecord.ORSORecActER = Math.Round(orsoRecord.ORSORecCalER, 2, MidpointRounding.AwayFromZero);
                    orsoRecord.ORSORecActEE = Math.Round(orsoRecord.ORSORecCalEE, 2, MidpointRounding.AwayFromZero);

                    EORSOPlan orsoPlan = new EORSOPlan();
                    orsoPlan.ORSOPlanID = orsoRecord.ORSOPlanID;
                    if (EORSOPlan.db.select(dbConn, orsoPlan))
                    {
                        if (string.IsNullOrEmpty(orsoPlan.ORSOPlanEmployerRoundingRule))
                        {
                            orsoPlan.ORSOPlanEmployerRoundingRule = Values.ROUNDING_RULE_ROUND_TO;
                            orsoPlan.ORSOPlanEmployerDecimalPlace = 2;
                        }
                        if (orsoPlan.ORSOPlanEmployerRoundingRule.Equals(Values.ROUNDING_RULE_ROUND_TO))
                            orsoRecord.ORSORecActER = HROne.CommonLib.GenericRoundingFunctions.RoundingTo(orsoRecord.ORSORecCalER, orsoPlan.ORSOPlanEmployerDecimalPlace, ExchangeCurrency.DefaultCurrencyDecimalPlaces());
                        else if (orsoPlan.ORSOPlanEmployerRoundingRule.Equals(Values.ROUNDING_RULE_ROUND_UP))
                            orsoRecord.ORSORecActER = HROne.CommonLib.GenericRoundingFunctions.RoundingUp(orsoRecord.ORSORecCalER, orsoPlan.ORSOPlanEmployerDecimalPlace, ExchangeCurrency.DefaultCurrencyDecimalPlaces());
                        else if (orsoPlan.ORSOPlanEmployerRoundingRule.Equals(Values.ROUNDING_RULE_ROUND_DOWN))
                            orsoRecord.ORSORecActER = HROne.CommonLib.GenericRoundingFunctions.RoundingDown(orsoRecord.ORSORecCalER, orsoPlan.ORSOPlanEmployerDecimalPlace, ExchangeCurrency.DefaultCurrencyDecimalPlaces());

                        if (string.IsNullOrEmpty(orsoPlan.ORSOPlanEmployeeRoundingRule))
                        {
                            orsoPlan.ORSOPlanEmployeeRoundingRule = Values.ROUNDING_RULE_ROUND_TO;
                            orsoPlan.ORSOPlanEmployeeDecimalPlace = 2;
                        }
                        if (orsoPlan.ORSOPlanEmployeeRoundingRule.Equals(Values.ROUNDING_RULE_ROUND_TO))
                            orsoRecord.ORSORecActEE = HROne.CommonLib.GenericRoundingFunctions.RoundingTo(orsoRecord.ORSORecCalEE, orsoPlan.ORSOPlanEmployeeDecimalPlace, ExchangeCurrency.DefaultCurrencyDecimalPlaces());
                        else if (orsoPlan.ORSOPlanEmployeeRoundingRule.Equals(Values.ROUNDING_RULE_ROUND_UP))
                            orsoRecord.ORSORecActEE = HROne.CommonLib.GenericRoundingFunctions.RoundingUp(orsoRecord.ORSORecCalEE, orsoPlan.ORSOPlanEmployeeDecimalPlace, ExchangeCurrency.DefaultCurrencyDecimalPlaces());
                        else if (orsoPlan.ORSOPlanEmployeeRoundingRule.Equals(Values.ROUNDING_RULE_ROUND_DOWN))
                            orsoRecord.ORSORecActEE = HROne.CommonLib.GenericRoundingFunctions.RoundingDown(orsoRecord.ORSORecCalEE, orsoPlan.ORSOPlanEmployeeDecimalPlace, ExchangeCurrency.DefaultCurrencyDecimalPlaces());
                    }
                }

                return orsoRecord;
            }
            else
                return null;
        }

        // Start 0000084, Ricky So, 2014-08-22
        private static double CalculateEEAmount(DatabaseConnection dbConn, EEmpPersonalInfo empInfo, int ORSOPlanID, double TotalVCRI, EPayrollPeriod payrollPeriod, DateTime ORSOJoinDate, EMPFRecord mpfRecord)
        {
            double totalVC = 0;

            EORSOPlan orsoPlan = new EORSOPlan();
            orsoPlan.ORSOPlanID = ORSOPlanID;
            if (EORSOPlan.db.select(dbConn, orsoPlan))
            {
                //  Use Service Year to Compare
                double YearOfService = HROne.Payroll.PayrollProcess.GetYearOfServer(dbConn, empInfo.EmpID, payrollPeriod.PayPeriodFr);
                EORSOPlanDetail orsoPlanDetailFrom = orsoPlan.GetORSOPlanDetail(dbConn, YearOfService);

                if (orsoPlanDetailFrom == null || TotalVCRI <= 0)
                    return 0;
                else
                {
                    totalVC = TotalVCRI * orsoPlanDetailFrom.ORSOPlanDetailEE / 100;

                    // Cap before residual
                    if (orsoPlan.ORSOPlanEmployeeResidual)
                    {
                        totalVC = (totalVC > orsoPlan.ORSOPlanEmployeeResidualCap) ? orsoPlan.ORSOPlanEmployeeResidualCap : totalVC;
                        // Deduct mandatory MPF contribution (use residual)
                        if (mpfRecord != null)
                            totalVC = (totalVC > mpfRecord.MPFRecActMCEE) ? (totalVC - mpfRecord.MPFRecActMCEE) : 0;
                    }

                    totalVC += orsoPlanDetailFrom.ORSOPlanDetailEEFix;
                    return totalVC < orsoPlan.ORSOPlanMaxEmployeeVC ? totalVC : orsoPlan.ORSOPlanMaxEmployeeVC;
                }
            }
            else
                return 0;
        }

        //private static double CalculateEEAmount(DatabaseConnection dbConn, EEmpPersonalInfo empInfo, int ORSOPlanID, double TotalVCRI, EPayrollPeriod payrollPeriod, DateTime ORSOJoinDate)
        //{
        //    double totalVC = 0;

        //    EORSOPlan orsoPlan = new EORSOPlan();
        //    orsoPlan.ORSOPlanID  = ORSOPlanID;
        //    if (EORSOPlan.db.select(dbConn, orsoPlan))
        //    {

        //        //  Use Service Year to Compare
        //        double YearOfService = HROne.Payroll.PayrollProcess.GetYearOfServer(dbConn, empInfo.EmpID, payrollPeriod.PayPeriodFr);
        //        EORSOPlanDetail orsoPlanDetailFrom = orsoPlan.GetORSOPlanDetail(dbConn, YearOfService);

        //        if (TotalVCRI <= 0)
        //            return 0;
        //        else
        //        {
        //            totalVC = TotalVCRI * orsoPlanDetailFrom.ORSOPlanDetailEE/ 100;
        //            totalVC += orsoPlanDetailFrom.ORSOPlanDetailEEFix;
        //            return totalVC < orsoPlan.ORSOPlanMaxEmployeeVC ? totalVC : orsoPlan.ORSOPlanMaxEmployeeVC;
        //        }
        //    }
        //    else
        //        return 0;
        //}
        // End 0000084, Ricky So, 2014-08-22

        // Start 0000084, Ricky So, 2014-08-22
        private static double CalculateERAmount(DatabaseConnection dbConn, EEmpPersonalInfo empInfo, int ORSOPlanID, double TotalVCRI, EPayrollPeriod payrollPeriod, EMPFRecord mpfRecord)
        {
            double totalVC = 0;

            EORSOPlan orsoPlan = new EORSOPlan();
            orsoPlan.ORSOPlanID = ORSOPlanID;
            if (EORSOPlan.db.select(dbConn, orsoPlan))
            {
                //  Use Service Year to Compare
                double YearOfService = HROne.Payroll.PayrollProcess.GetYearOfServer(dbConn, empInfo.EmpID, payrollPeriod.PayPeriodFr);
                EORSOPlanDetail orsoPlanDetailFrom = orsoPlan.GetORSOPlanDetail(dbConn, YearOfService);

                if (orsoPlanDetailFrom == null || TotalVCRI <= 0)
                    return 0;
                else
                {
                    totalVC = TotalVCRI * orsoPlanDetailFrom.ORSOPlanDetailER / 100;

                    // Cap before residual
                    if (orsoPlan.ORSOPlanEmployerResidual)
                    {
                        // Start 0000084, Ricky So, 2014-08-22
                        totalVC = (totalVC > orsoPlan.ORSOPlanEmployerResidualCap) ? orsoPlan.ORSOPlanEmployerResidualCap : totalVC;
                        // End 0000084, Ricky So, 2014-08-22
                        
                        // Deduct MPF contribution (use residual)
                        if (mpfRecord != null)
                            totalVC = (totalVC > mpfRecord.MPFRecActMCER) ? (totalVC - mpfRecord.MPFRecActMCER) : 0;
                    }

                    totalVC += orsoPlanDetailFrom.ORSOPlanDetailERFix;
                    return totalVC < orsoPlan.ORSOPlanMaxEmployerVC ? totalVC : orsoPlan.ORSOPlanMaxEmployerVC;
                }
            }
            else
                return 0;
        }

        //private static double CalculateERAmount(DatabaseConnection dbConn, EEmpPersonalInfo empInfo, int ORSOPlanID, double TotalVCRI, EPayrollPeriod payrollPeriod)
        //{
        //    double totalVC = 0;

        //    EORSOPlan orsoPlan = new EORSOPlan();
        //    orsoPlan.ORSOPlanID = ORSOPlanID;
        //    if (EORSOPlan.db.select(dbConn, orsoPlan))
        //    {
        //        //  Use Service Year to Compare
        //        double YearOfService = HROne.Payroll.PayrollProcess.GetYearOfServer(dbConn, empInfo.EmpID, payrollPeriod.PayPeriodFr);
        //        EORSOPlanDetail orsoPlanDetailFrom = orsoPlan.GetORSOPlanDetail(dbConn, YearOfService);

        //        if (TotalVCRI <= 0)
        //            return 0;
        //        else
        //        {
        //            totalVC = TotalVCRI * orsoPlanDetailFrom.ORSOPlanDetailER / 100;
        //            totalVC += orsoPlanDetailFrom.ORSOPlanDetailERFix;
        //            return totalVC < orsoPlan.ORSOPlanMaxEmployerVC ? totalVC : orsoPlan.ORSOPlanMaxEmployerVC;
        //        }
        //    }
        //    else
        //        return 0;
        //}
        // End 0000084, Ricky So, 2014-08-22

        public static ArrayList GenerateORSOEEPaymentRecords(DatabaseConnection dbConn, int EmpID, ArrayList ORSORecords, ArrayList paymentRecords)
        {
            ArrayList ORSOPaymentRecords = new ArrayList();
            if (ORSORecords.Count > 0)
            {
                ArrayList ORSOPaymentCodes = PayrollProcess.GetPaymentCodeByPaymentType(dbConn, EPaymentType.SystemPaymentType.PFundEmployeeContributionPaymentType(dbConn).PaymentTypeCode);
                if (ORSOPaymentCodes.Count > 0)
                {
                    EPaymentCode ORSOPaymentCode = (EPaymentCode)ORSOPaymentCodes[0];
                    double ORSOAmount = 0;
                    foreach (EORSORecord ORSORecord in ORSORecords)
                        if (ORSORecord != null)
                            ORSOAmount -= ORSORecord.ORSORecActEE;
                    if (Math.Abs(ORSOAmount) >= 0.01)
                        ORSOPaymentRecords.AddRange(PayrollProcess.GetDeductablePaymentRecord(dbConn, EmpID, paymentRecords, ORSOAmount, ORSOPaymentCode.PaymentCodeID));

                }
                foreach (EPaymentRecord paymentRecord in ORSOPaymentRecords)
                    paymentRecord.PayRecType = PaymentRecordType.PAYRECORDTYPE_PENSION;


                return ORSOPaymentRecords;
            }
            else
                return null;
        }

        //private static EEmpTermination EEmpTermination.GetObjectByEmpID(dbConn, int EmpID)
        //{
        //    DBFilter filter = new DBFilter();
        //    filter.add(new Match("empid", EmpID));


        //    ArrayList empTerminations = EEmpTermination.db.select(dbConn, filter);
        //    if (empTerminations.Count > 0)
        //        return ((EEmpTermination)empTerminations[0]);
        //    else
        //        return null;
        //}

        private static int GetORSOPlanID(DatabaseConnection dbConn, int EmpID, DateTime ReferenceDate)
        {
            DBFilter empORSOPlanFilter = new DBFilter();
            empORSOPlanFilter.add(new Match("EMPID", EmpID));
            empORSOPlanFilter.add(new Match("EmpORSOEffFr", "<=", ReferenceDate));

            OR orORSOEffFilter = new OR();
            orORSOEffFilter.add(new Match("EmpORSOEffTo", ">=", ReferenceDate));
            orORSOEffFilter.add(new NullTerm("EmpORSOEffTo"));
            empORSOPlanFilter.add(orORSOEffFilter);
            empORSOPlanFilter.add("EmpORSOEffFr", false);

            ArrayList empORSOPlans = EEmpORSOPlan.db.select(dbConn, empORSOPlanFilter);

            if (empORSOPlans.Count > 0)
                return ((EEmpORSOPlan)empORSOPlans[0]).ORSOPlanID;
            else
                return 0;

        }

        public static void UndoORSO(DatabaseConnection dbConn, EEmpPayroll empPayroll)
        {


            DBFilter filter = new DBFilter();
            filter.add(new Match("EmpPayrollID", empPayroll.EmpPayrollID));
            ArrayList ORSORecords = EORSORecord.db.select(dbConn, filter);
            foreach (EORSORecord ORSORecord in ORSORecords)
            {
                EORSORecord.db.delete(dbConn, ORSORecord);
            }
        }
        // Start 000162, Ricky So, 2015-01-26
        public static void Recalculate(DatabaseConnection dbConn, EEmpPayroll empPayroll)
        {
            EPayrollPeriod payPeriod = new EPayrollPeriod();
            payPeriod.PayPeriodID = empPayroll.PayPeriodID;
            if (EPayrollPeriod.db.select(dbConn, payPeriod))
            {
                DBFilter paymentFilter = new DBFilter();
                paymentFilter.add(new Match("EmpPayrollID", empPayroll.EmpPayrollID));
                ArrayList paymentRecords = EPaymentRecord.db.select(dbConn, paymentFilter);

                ArrayList mpfRecords = HROne.Payroll.MPFProcess.MPFTrialRun(dbConn, empPayroll.EmpID, payPeriod, new ArrayList());
                ArrayList orsoRecords = HROne.Payroll.ORSOProcess.ORSOTrialRun(dbConn, empPayroll.EmpID, payPeriod, paymentRecords, (mpfRecords.Count > 0) ? (EMPFRecord)mpfRecords[0] : null);

                //ArrayList mpfPaymentRecords = HROne.Payroll.MPFProcess.GenerateMPFEEPaymentRecords(dbConn, empPayroll.EmpID, payPeriod, mpfRecords, paymentRecords);
                ArrayList orsoPaymentRecords = HROne.Payroll.ORSOProcess.GenerateORSOEEPaymentRecords(dbConn, empPayroll.EmpID, orsoRecords, paymentRecords);

                if (orsoRecords != null)
                    foreach(EORSORecord orsoRecord in orsoRecords)
                    {
                        DBFilter existingORSORecordFilter = new DBFilter();
                        existingORSORecordFilter.add(new Match("EmpPayrollID", empPayroll.EmpPayrollID));
                        existingORSORecordFilter.add(new Match("ORSORecPeriodFr", orsoRecord.ORSORecPeriodFr));
                        existingORSORecordFilter.add(new Match("ORSORecPeriodTo", orsoRecord.ORSORecPeriodTo));
                        existingORSORecordFilter.add(new Match("ORSOPlanID", orsoRecord.ORSOPlanID));

                        if (!(orsoRecord.ORSORecActEE == 0 && orsoRecord.ORSORecActER == 0 && orsoRecord.ORSORecActRI == 0) || EORSORecord.db.count(dbConn, existingORSORecordFilter) <= 0)
                        {
                            orsoRecord.EmpPayrollID = empPayroll.EmpPayrollID;
                            if (!empPayroll.EmpPayStatus.Equals("T"))
                                orsoRecord.ORSORecType = "A";
                            EORSORecord.db.insert(dbConn, orsoRecord);
                        }
                    }
                
                //if (mpfRecords != null)
                    //    foreach (EMPFRecord mpfRecord in mpfRecords)
                    //{
                    //    DBFilter existingMPFRecordFilter = new DBFilter();
                    //    existingMPFRecordFilter.add(new Match("EmpPayrollID", empPayroll.EmpPayrollID));
                    //    existingMPFRecordFilter.add(new Match("MPFRecPeriodFr", mpfRecord.MPFRecPeriodFr));
                    //    existingMPFRecordFilter.add(new Match("MPFRecPeriodTo", mpfRecord.MPFRecPeriodTo));
                    //    existingMPFRecordFilter.add(new Match("MPFPlanID", mpfRecord.MPFPlanID));

                    //    if (!(mpfRecord.MPFRecActMCEE == 0 && mpfRecord.MPFRecActMCER == 0 && mpfRecord.MPFRecActMCRI == 0 && mpfRecord.MPFRecActVCEE == 0 && mpfRecord.MPFRecActVCER == 0 && mpfRecord.MPFRecActVCRI == 0) || EMPFRecord.db.count(dbConn, existingMPFRecordFilter) <= 0)
                    //    {
                    //        mpfRecord.EmpPayrollID = empPayroll.EmpPayrollID;
                    //        if (!empPayroll.EmpPayStatus.Equals("T"))
                    //            mpfRecord.MPFRecType = "A";
                    //        EMPFRecord.db.insert(dbConn, mpfRecord);
                    //    }
                    //}
                if (orsoPaymentRecords != null)
                    foreach (EPaymentRecord paymentRecord in orsoPaymentRecords)
                    {
                        paymentRecord.EmpPayrollID = empPayroll.EmpPayrollID;
                        if (!empPayroll.EmpPayStatus.Equals("T"))
                            paymentRecord.PayRecType = "A";
                        EPaymentRecord.db.insert(dbConn, paymentRecord);
                    }

                //if (mpfPaymentRecords != null)
                //    foreach (EPaymentRecord paymentRecord in mpfPaymentRecords)
                //    {
                //        paymentRecord.EmpPayrollID = empPayroll.EmpPayrollID;
                //        if (!empPayroll.EmpPayStatus.Equals("T"))
                //            paymentRecord.PayRecType = "A";
                //        EPaymentRecord.db.insert(dbConn, paymentRecord);
                //    }
            }
        }
        // End 000162, Ricky So, 2015-01-26
    }
}