using System;
using System.Data;
using System.Configuration;
using System.Collections;
using HROne.Lib.Entities;
using HROne.DataAccess;

namespace HROne.Payroll
{

    public class AverageWages
    {
        protected int m_EmpID;
        public int EmpID
        {
            get { return m_EmpID; }
        }
        protected DateTime m_AsOfDate;
        public DateTime AsOfDate
        {
            get { return m_AsOfDate; }
        }

        protected double m_TotalWagesWithoutOT;
        protected double m_TotalOTWages;
        protected double m_TotalWagesExclude;
        protected double m_TotalDays;
        protected double m_TotalDaysExclude;
        protected DateTime m_PeriodFrom;
        protected DateTime m_PeriodTo;
        protected DatabaseConnection dbConn;

        public AverageWages(DatabaseConnection dbConn, int EmpID, DateTime AsOfDate)
        {
            this.dbConn = dbConn;
            LoadData(EmpID, AsOfDate);
        }

        protected AverageWages(DatabaseConnection dbConn, int EmpID, DateTime PeriodFrom, DateTime PeriodTo)
        {
            this.dbConn = dbConn;
            LoadWageDetail(EmpID, PeriodFrom, PeriodTo);
        }

        public void LoadData(int EmpID, DateTime AsOfDate)
        {
            //  Initialize Variable
            m_EmpID = EmpID;
            m_AsOfDate = AsOfDate;

            m_TotalDays = 0;
            m_TotalDaysExclude = 0;
            m_TotalWagesWithoutOT = 0;
            m_TotalOTWages = 0;
            m_TotalWagesExclude = 0;


                        //  Get Last Payroll Period
            DBFilter lastPaylPeriodFilter = new DBFilter();
            lastPaylPeriodFilter.add(new Match("payPeriodTo", "<", AsOfDate));

            DBFilter EmpIDFilter = new DBFilter();
            EmpIDFilter.add(new Match("EmpID", "=", EmpID));

            IN lastPaylPeriodInFilter = new IN("PayPeriodID", "Select Distinct PayPeriodID from EmpPayroll", EmpIDFilter);
            lastPaylPeriodFilter.add(lastPaylPeriodInFilter);
            lastPaylPeriodFilter.add("payPeriodTo", false);
            //lastPaylPeriodFilter.loadData("select top 1 * from PayrollPeriod");
            ArrayList lastPayPeriods = EPayrollPeriod.db.select(dbConn, lastPaylPeriodFilter);
            if (lastPayPeriods.Count > 0)
            {
                //  Determine the date range 
                EPayrollPeriod lastPayPeriod = (EPayrollPeriod)lastPayPeriods[0];

                LoadWageDetail(EmpID, lastPayPeriod.PayPeriodTo.AddDays(1).AddYears(-1), lastPayPeriod.PayPeriodTo);
            }
            else
            {
                DBFilter empTermFilter = new DBFilter();
                empTermFilter.add(new Match("NewEmpID", EmpID));
                ArrayList empTermList = EEmpTermination.db.select(dbConn, empTermFilter);
                foreach (EEmpTermination empTerm in empTermList)
                {
                    if (empTerm.EmpTermIsTransferCompany)
                    {
                        AverageWages averageWage = new AverageWages(dbConn, empTerm.EmpID, AsOfDate);
                        m_TotalDays = averageWage.TotalDays;
                        m_TotalDaysExclude += averageWage.TotalDaysExclude;
                        m_TotalWagesWithoutOT += averageWage.TotalWagesWithoutOT;
                        m_TotalOTWages += averageWage.TotalOTWages;
                        m_TotalWagesExclude += averageWage.TotalWagesExclude;

                        if (m_PeriodTo < averageWage.PeriodTo || m_PeriodTo.Ticks.Equals(0))
                            m_PeriodTo = averageWage.PeriodTo;
                        if (m_PeriodFrom > averageWage.PeriodFrom || m_PeriodFrom.Ticks.Equals(0))
                            m_PeriodFrom = averageWage.PeriodFrom;

                    }
                }
            }
        }

        protected void LoadWageDetail(int EmpID, DateTime PeriodFrom, DateTime PeriodTo)
        {
            //  Initialize Variable
            m_EmpID = EmpID;
            m_AsOfDate = AsOfDate;

            m_TotalDays = 0;
            m_TotalDaysExclude = 0;
            m_TotalWagesWithoutOT = 0;
            m_TotalOTWages = 0;
            m_TotalWagesExclude = 0;

                m_PeriodTo = PeriodTo;
                m_PeriodFrom = PeriodFrom;

            EEmpPersonalInfo empInfo = new EEmpPersonalInfo();
            empInfo.EmpID = EmpID;
            if (EEmpPersonalInfo.db.select(dbConn, empInfo))
            {
                if (empInfo.EmpDateOfJoin > m_PeriodFrom)
                    m_PeriodFrom = empInfo.EmpDateOfJoin;

                EPaymentType OverTimePaymentType = EPaymentType.SystemPaymentType.OverTimePaymentType(dbConn);


                DBFilter EmpPayrollFilter = new DBFilter();
                EmpPayrollFilter.add(new Match("EmpID", "=", EmpID));

                DBFilter PaylPeriodFilter = new DBFilter();
                PaylPeriodFilter.add(new Match("payPeriodFr", "<=", m_PeriodTo));
                PaylPeriodFilter.add(new Match("payPeriodTo", ">=", m_PeriodFrom));
                IN PaylPeriodInFilter = new IN("PayPeriodID", "Select PayPeriodID from PayrollPeriod", PaylPeriodFilter);
                EmpPayrollFilter.add(PaylPeriodInFilter);

                ArrayList empPayrolls = EEmpPayroll.db.select(dbConn, EmpPayrollFilter);

                //// initialize Payment Code Filter
                //DBFilter paymentCodeFilter = new DBFilter();
                //paymentCodeFilter.add(new Match("PaymentCodeIsWages", "<>", 0));
                //IN paymentCodeInFilter = new IN("PaymentCodeID", "Select PaymentCodeID from PaymentCode", paymentCodeFilter);

                foreach (EEmpPayroll empPayroll in empPayrolls)
                {
                    m_TotalDays += empPayroll.EmpPayNumOfDayCount;

                    //  Get Related Payment Record

                    DBFilter paymentRecordFilter = new DBFilter();
                    paymentRecordFilter.add(new Match("EmpPayrollID", empPayroll.EmpPayrollID));
                    //paymentRecordFilter.add(paymentCodeInFilter);

                    ArrayList paymentRecords = EPaymentRecord.db.select(dbConn, paymentRecordFilter);

                    foreach (EPaymentRecord paymentRecord in paymentRecords)
                    {
                        EPaymentCode paymentCode = new EPaymentCode();
                        paymentCode.PaymentCodeID = paymentRecord.PaymentCodeID;
                        if (EPaymentCode.db.select(dbConn, paymentCode))
                        {
                            if (paymentCode.PaymentCodeIsWages)
                            {
                                // **** Start 0000021, Ricky So, 2014-04-15
                                
                                bool m_isFullPayLeaveItems = false;
                                
                                if (!string.IsNullOrEmpty(paymentRecord.LeaveAppIDList))
                                {
                                    foreach (string m_leaveAppID in paymentRecord.LeaveAppIDList.Split(new char[] { '|' }))
                                    {

                                        int m_intLeaveAppID = int.Parse(m_leaveAppID, System.Globalization.NumberStyles.HexNumber);

                                        DBFilter m_leaveCodeFilter = new DBFilter();
                                        DBFilter m_leaveAppFilter = new DBFilter();

                                        m_leaveAppFilter.add(new Match("LeaveAppID", m_intLeaveAppID));
                                        m_leaveCodeFilter.add(new IN("LeaveCodeID", "SELECT LeaveCodeID FROM LeaveApplication", m_leaveAppFilter));

                                        ArrayList m_leaveCodeList = ELeaveCode.db.select(dbConn, m_leaveCodeFilter);

                                        if (m_leaveCodeList.Count > 0)
                                        {
                                            m_isFullPayLeaveItems = (((ELeaveCode)m_leaveCodeList[0]).LeaveCodePayRatio == 1);
                                        }

                                        if (m_isFullPayLeaveItems)
                                            break;
                                    }
                                }

                                double days = paymentRecord.PayRecNumOfDayAdj;

                                if (m_isFullPayLeaveItems == false)
                                {
                                    days = HalfDayCountOneDay(paymentRecord.PayRecNumOfDayAdj);
                                }

                                if (paymentCode.PaymentTypeID == OverTimePaymentType.PaymentTypeID)
                                {
                                    // **** Start 0000021, Ricky So, 2014-04-15
                                    //m_TotalDays += paymentRecord.PayRecNumOfDayAdj;
                                    //m_TotalOTWages += paymentRecord.PayRecActAmount;

                                    // the following 2 lines is required to fix 0000021. the above 2 lines are comment out in the fix
                                    m_TotalDays += days;

                                    if (paymentRecord.PayRecNumOfDayAdj == 0)       //通常 BAS / LEAVE / HOLIDAY 既ITEM 先有DAY ADJ
                                    {
                                        m_TotalOTWages += paymentRecord.PayRecActAmount;
                                    }
                                    else
                                    {
                                        m_TotalOTWages += (paymentRecord.PayRecActAmount / paymentRecord.PayRecNumOfDayAdj) * days;
                                    }
                                    // **** End 0000021, Ricky So, 2014-04-15
                                }
                                else
                                {
                                    // **** Start 0000021, Ricky So, 2014-04-15
                                    //m_TotalDays += paymentRecord.PayRecNumOfDayAdj;
                                    //m_TotalWagesWithoutOT += paymentRecord.PayRecActAmount;

                                    // the following 2 lines is required to fix 0000021. the above 2 lines are comment out in the fix
                                    m_TotalDays += days;

                                    if (paymentRecord.PayRecNumOfDayAdj == 0)       //通常 BAS / LEAVE / HOLIDAY 既ITEM 先有DAY ADJ
                                    {
                                        m_TotalWagesWithoutOT += paymentRecord.PayRecActAmount;
                                    }
                                    else
                                    {
                                        m_TotalWagesWithoutOT += (paymentRecord.PayRecActAmount / paymentRecord.PayRecNumOfDayAdj) * days;
                                    }
                                    // **** End, Ricky So, 2014-04-15
                                }
                            }
                        }
                    }
                }

                DBFilter empTermFilter = new DBFilter();
                empTermFilter.add(new Match("NewEmpID", EmpID));
                ArrayList empTermList = EEmpTermination.db.select(dbConn, empTermFilter);
                foreach (EEmpTermination empTerm in empTermList)
                {
                    if (empTerm.EmpTermIsTransferCompany)
                    {
                        AverageWages averageWage = new AverageWages(dbConn, empTerm.EmpID, PeriodFrom, PeriodTo);
                        m_TotalDays += averageWage.TotalDays;
                        m_TotalDaysExclude += averageWage.TotalDaysExclude;
                        m_TotalWagesWithoutOT += averageWage.TotalWagesWithoutOT;
                        m_TotalOTWages += averageWage.TotalOTWages;
                        m_TotalWagesExclude += averageWage.TotalWagesExclude;

                        if (m_PeriodTo < averageWage.PeriodTo || m_PeriodTo.Ticks.Equals(0))
                            m_PeriodTo = averageWage.PeriodTo;
                        if (m_PeriodFrom > averageWage.PeriodFrom || m_PeriodFrom.Ticks.Equals(0))
                            m_PeriodFrom = averageWage.PeriodFrom;
                    }
                }
            }

        }

        protected double HalfDayCountOneDay(double originalValue)
        {
            if (originalValue % 1 == 0)
                return originalValue;
            else
                return Math.Truncate(originalValue) + (originalValue / Math.Abs(originalValue));   // trunc(-3.3393) + (-3.3393/3.3393));
        }

        public double TotalWagesWithoutOT
        {
            get { return m_TotalWagesWithoutOT; }
        }

        public double TotalOTWages
        {
            get { return m_TotalOTWages; }
        }

        public double TotalWagesExclude
        {
            get 
            {
                if (IsWagesIncludeOT())
                    return m_TotalWagesExclude;
                else
                    return m_TotalWagesExclude + m_TotalOTWages;
            }
        }

        public double TotalDays
        {
            get { return m_TotalDays; }
        }

        public double TotalDaysExclude
        {
            get
            { return m_TotalDaysExclude; }
        }

        public DateTime PeriodFrom
        {
            get { return m_PeriodFrom; }
        }

        public DateTime PeriodTo
        {
            get { return m_PeriodTo; }
        }

        public double DailyWagesWithoutOT()
        {
            if (m_TotalDays > 0)
                return (m_TotalWagesWithoutOT / m_TotalDays);
            else
                return 0;
        }

        public double OTDailyWages()
        {
            if (m_TotalDays > 0)
                return (m_TotalOTWages/ m_TotalDays);
            else
                return 0;
        }
        public double DailyWages()
        {
            if (m_TotalDays > 0)
                if (IsWagesIncludeOT())
                    return (m_TotalWagesWithoutOT + m_TotalOTWages) / m_TotalDays;
                else
                    return DailyWagesWithoutOT();
            else
                return 0;
                
        }

        public double MonthlyWages()
        {
            if (m_TotalDays > 0)
            {
                DateTime yearlyStartDate = m_PeriodTo.AddDays(1).AddYears(-1);
                double totalCalendarDaysCount = ((TimeSpan)m_PeriodTo.Subtract(yearlyStartDate)).TotalDays + 1;
                if (IsWagesIncludeOT())
                    return ((m_TotalWagesWithoutOT + m_TotalOTWages) * totalCalendarDaysCount) / (12 * m_TotalDays);
                else
                    return ((m_TotalWagesWithoutOT) * totalCalendarDaysCount) / (12 * m_TotalDays);
            }
            else
                return 0;
            //if (m_TotalDays > 0)
            //    if (IsWagesIncludeOT())
            //        return (m_TotalWagesWithoutOT + m_TotalOTWages) / HROne.CommonLib.Utility.MonthDifference(PeriodFrom, PeriodTo);
            //    else
            //        return (m_TotalWagesWithoutOT) / HROne.CommonLib.Utility.MonthDifference(PeriodFrom, PeriodTo);
            //else
            //    return 0;
        }

        public bool IsWagesIncludeOT()
        {
            //  When compare, need include any OT inside wages
            return ((m_TotalWagesWithoutOT + m_TotalOTWages) * 0.2 <= m_TotalOTWages);
        }
    }

    /// <summary>
    /// Summary description for PayrollFormula
    /// </summary>
    public abstract class PayrollFormula
    {
        public static double DailyProrataCaluclation(DatabaseConnection dbConn, string ProrataFormulaCode, int DefaultPayrollFormulaID, int EmpID, double amount, DateTime periodFrom, DateTime periodTo, int numOfPeriodPerYear, DateTime asOfDate, out string FormulaRemarkString, out bool IsDAW)
        {
            return DailyProrataCaluclation(dbConn, ProrataFormulaCode, DefaultPayrollFormulaID, EmpID, amount, periodFrom, periodTo, numOfPeriodPerYear, asOfDate, 0, out FormulaRemarkString, out IsDAW);
        }

        public static double DailyProrataCaluclation(DatabaseConnection dbConn, string ProrataFormulaCode, int DefaultPayrollFormulaID, int EmpID, double amount, DateTime periodFrom, DateTime periodTo, int numOfPeriodPerYear, DateTime asOfDate, int LeaveCodeID, out string FormulaRemarkString, out bool IsDAW)
        {
            if (ProrataFormulaCode.Equals(EPayrollProrataFormula.DEFAULT_FOEMULA_CODE, StringComparison.CurrentCultureIgnoreCase))
            {
                EPayrollProrataFormula defaultProrataFormula = new EPayrollProrataFormula();

                defaultProrataFormula.PayFormID = DefaultPayrollFormulaID;
                if (!EPayrollProrataFormula.db.select(dbConn, defaultProrataFormula))
                    return DailyProrataCaluclation(dbConn, "SYS001", DefaultPayrollFormulaID, EmpID, amount, periodFrom, periodTo, numOfPeriodPerYear, asOfDate, LeaveCodeID, out FormulaRemarkString, out IsDAW);
                else if (defaultProrataFormula.PayFormCode.Equals(EPayrollProrataFormula.DEFAULT_FOEMULA_CODE, StringComparison.CurrentCultureIgnoreCase))
                    return DailyProrataCaluclation(dbConn, "SYS001", DefaultPayrollFormulaID, EmpID, amount, periodFrom, periodTo, numOfPeriodPerYear, asOfDate, LeaveCodeID, out FormulaRemarkString, out IsDAW);
                else
                    return DailyProrataCaluclation(dbConn, DefaultPayrollFormulaID, DefaultPayrollFormulaID, EmpID, amount, periodFrom, periodTo, numOfPeriodPerYear, asOfDate, LeaveCodeID, out FormulaRemarkString, out IsDAW);
            }
            if (ProrataFormulaCode.Equals("SYS001", StringComparison.CurrentCultureIgnoreCase))
            {
                // Payment within Payroll Cycle / (Calendar day within Payroll Cycle)
                int numOfDay = (periodTo.Subtract(periodFrom).Days + 1);
                FormulaRemarkString = "( " + amount.ToString("$#,##0.00##") + " / " + numOfDay + " )";
                IsDAW = false;
                return amount / numOfDay;
            }
            if (ProrataFormulaCode.Equals("SYS002", StringComparison.CurrentCultureIgnoreCase))
            {
                // Yearly / 365
                FormulaRemarkString = "( " + amount.ToString("$#,##0.00##") + " x " + numOfPeriodPerYear + " / 365 )";
                IsDAW = false;
                return amount * numOfPeriodPerYear / 365;
            }
            if (ProrataFormulaCode.Equals("SYS003", StringComparison.CurrentCultureIgnoreCase))
            {
                // Monthly / Calendar day per month
                int numOfDayPerMonth = new DateTime(asOfDate.AddMonths(1).Year, asOfDate.AddMonths(1).Month, 1).Subtract(new DateTime(asOfDate.Year, asOfDate.Month, 1)).Days;
                if (numOfPeriodPerYear == 12)
                    FormulaRemarkString = "( " + amount.ToString("$#,##0.00##") + " / " + numOfDayPerMonth + " )";
                else
                    FormulaRemarkString = "( " + amount.ToString("$#,##0.00##") + " x " + (numOfPeriodPerYear / 12) + " / " + numOfDayPerMonth + " )";
                IsDAW = false;
                return amount * numOfPeriodPerYear / 12 / numOfDayPerMonth;
            }
            if (ProrataFormulaCode.Equals("SYS004", StringComparison.CurrentCultureIgnoreCase))
            {
                // Payment within Payroll Cycle / (Working day within Payroll Cycle)
                double numOfDay = (periodTo.Subtract(periodFrom).TotalDays + 1) - PayrollProcess.GetTotalRestDayEntitled(dbConn, EmpID, periodFrom, periodTo, true);
                FormulaRemarkString = "( " + amount.ToString("$#,##0.00##") + " / " + numOfDay + " )";
                IsDAW = false;
                return amount / numOfDay;

            }
            if (ProrataFormulaCode.Equals("SYS005", StringComparison.CurrentCultureIgnoreCase))
            {
                // Monthly / working day per month
                DateTime monthlyDateFrom = new DateTime(asOfDate.Year, asOfDate.Month, 1);
                DateTime monthlyDateTo = new DateTime(asOfDate.AddMonths(1).Year, asOfDate.AddMonths(1).Month, 1);
                double numOfDayPerMonth = ((TimeSpan)monthlyDateTo.Subtract(monthlyDateFrom)).TotalDays - PayrollProcess.GetTotalRestDayEntitled(dbConn, EmpID, monthlyDateFrom, monthlyDateTo, true);
                if (numOfPeriodPerYear == 12)
                    FormulaRemarkString = "( " + amount.ToString("$#,##0.00##") + " / " + numOfDayPerMonth + " )";
                else
                    FormulaRemarkString = "( " + amount.ToString("$#,##0.00##") + " x " + (numOfPeriodPerYear / 12) + " / " + numOfDayPerMonth + " )";
                IsDAW = false;
                return amount * numOfPeriodPerYear / 12 / numOfDayPerMonth;
            }
            if (ProrataFormulaCode.Equals("DAW", StringComparison.CurrentCulture))
            {
                if (LeaveCodeID > 0)
                {
                    asOfDate = getDAWAsOfDate(dbConn, EmpID, LeaveCodeID, asOfDate);
                }else
                {
                    if (IsStatutoryHoliday(dbConn, asOfDate))
                    {
                        while (IsStatutoryHoliday(dbConn, asOfDate.AddDays(-1)))
                        {
                            asOfDate = asOfDate.AddDays(-1);
                        }
                    }
                }
                AverageWages averageWages = new AverageWages(dbConn, EmpID, asOfDate);
                double DAW = averageWages.DailyWages();
                if (DAW <= 0)
                    return DailyProrataCaluclation(dbConn, "SYS001", DefaultPayrollFormulaID, EmpID, amount, periodFrom, periodTo, numOfPeriodPerYear, asOfDate, LeaveCodeID, out FormulaRemarkString, out IsDAW);
                else
                {
                    FormulaRemarkString = DAW.ToString("$#,##0.00##");
                    IsDAW = true;
                    return DAW;
                }
            }
            FormulaRemarkString=string.Empty;
            IsDAW = false;
            return 0;
        }

        protected static bool IsStatutoryHoliday(DatabaseConnection dbConn, DateTime asOfDate)
        {
            ArrayList statutoryHolidays = null; 
            DBFilter statutoryHolidayFilter = new DBFilter();
            statutoryHolidayFilter.add(new Match("StatutoryHolidayDate", ">=", asOfDate));
            statutoryHolidayFilter.add(new Match("StatutoryHolidayDate", "<=", asOfDate));
            statutoryHolidays = EStatutoryHoliday.db.select(dbConn, statutoryHolidayFilter);
            if (statutoryHolidays.Count > 0)
            {
                return true;
            }
            return false;
        }

        public static double DailyProrataCaluclationForStatutoryHoliday(DatabaseConnection dbConn, int ProrataFormulaID, int DefaultPayrollFormulaID, int EmpID, double amount, DateTime periodFrom, DateTime periodTo, int numOfPeriodPerYear, DateTime asOfDate, int LeaveCodeID, out string FormulaRemarkString, out bool IsDAW)
        {
            EPayrollProrataFormula prorataFormula = new EPayrollProrataFormula();

            prorataFormula.PayFormID = ProrataFormulaID;
            if (!EPayrollProrataFormula.db.select(dbConn, prorataFormula))
                return DailyProrataCaluclation(dbConn, EPayrollProrataFormula.DEFAULT_FOEMULA_CODE, DefaultPayrollFormulaID, EmpID, amount, periodFrom, periodTo, numOfPeriodPerYear, asOfDate, LeaveCodeID, out FormulaRemarkString, out IsDAW);
            if (prorataFormula.PayFormIsSys.Equals("Y", StringComparison.CurrentCultureIgnoreCase))
                return DailyProrataCaluclation(dbConn, prorataFormula.PayFormCode, DefaultPayrollFormulaID, EmpID, amount, periodFrom, periodTo, numOfPeriodPerYear, asOfDate, LeaveCodeID, out FormulaRemarkString, out IsDAW);
            else
            {
                double dailyAmount = 0;

                if (prorataFormula.ReferencePayFormID > 0)
                {
                    dailyAmount = DailyProrataCaluclation(dbConn, prorataFormula.ReferencePayFormID, DefaultPayrollFormulaID, EmpID, amount, periodFrom, periodTo, numOfPeriodPerYear, asOfDate, LeaveCodeID, out FormulaRemarkString, out IsDAW);
                }
                else
                {
                    IsDAW = false;

                    dailyAmount = amount * prorataFormula.PayFormMultiplier / prorataFormula.PayFormDivider;
                    FormulaRemarkString = amount.ToString("$#,##0.00##")
                        + (prorataFormula.PayFormMultiplier.Equals(1) ? "" : " x " + prorataFormula.PayFormMultiplier)
                        + (prorataFormula.PayFormDivider.Equals(1) ? "" : " / " + prorataFormula.PayFormDivider);
                }
                if (prorataFormula.PayFormDecimalPlace < 9)
                {
                    if (!string.IsNullOrEmpty(prorataFormula.PayFormRoundingRule))

                        //  To prevent unexpected value of RoundingRule change the original value of FormulaRemark,
                        //  All re-assign value of FormulaRemark should be done inside the condition.
                        if (prorataFormula.PayFormRoundingRule.Equals(Values.ROUNDING_RULE_ROUND_DOWN))
                        {
                            dailyAmount = HROne.CommonLib.GenericRoundingFunctions.RoundingDown(dailyAmount, prorataFormula.PayFormDecimalPlace, prorataFormula.PayFormDecimalPlace);
                            FormulaRemarkString = dailyAmount.ToString("$#,##0." + string.Empty.PadLeft(prorataFormula.PayFormDecimalPlace, '#'));
                        }
                        else if (prorataFormula.PayFormRoundingRule.Equals(Values.ROUNDING_RULE_ROUND_UP))
                        {
                            dailyAmount = HROne.CommonLib.GenericRoundingFunctions.RoundingUp(dailyAmount, prorataFormula.PayFormDecimalPlace, prorataFormula.PayFormDecimalPlace);
                            FormulaRemarkString = dailyAmount.ToString("$#,##0." + string.Empty.PadLeft(prorataFormula.PayFormDecimalPlace, '#'));
                        }
                        else if (prorataFormula.PayFormRoundingRule.Equals(Values.ROUNDING_RULE_ROUND_TO))
                        {
                            dailyAmount = HROne.CommonLib.GenericRoundingFunctions.RoundingTo(dailyAmount, prorataFormula.PayFormDecimalPlace, prorataFormula.PayFormDecimalPlace);
                            FormulaRemarkString = dailyAmount.ToString("$#,##0." + string.Empty.PadLeft(prorataFormula.PayFormDecimalPlace, '#'));
                        }

                }
                return dailyAmount;
            }
        }


        private static DateTime getDAWAsOfDate(DatabaseConnection dbConn, int EmpID, int LeaveCodeID, DateTime LeaveDateFrom)
        {
            DBFilter leaveApplicationFilter = new DBFilter();
            leaveApplicationFilter.add(new Match("EmpID", EmpID));
            leaveApplicationFilter.add(new Match("LeaveCodeID", LeaveCodeID));
            leaveApplicationFilter.add(new Match("LeaveAppDateFrom", "<=", LeaveDateFrom.AddDays(-1)));
            leaveApplicationFilter.add(new Match("LeaveAppDateTo", ">=", LeaveDateFrom.AddDays(-1)));
            leaveApplicationFilter.add("LeaveAppDateFrom", true);
            ArrayList leaveAppList = ELeaveApplication.db.select(dbConn, leaveApplicationFilter);
            if (leaveAppList.Count > 0)
            {
                ELeaveApplication leaveApp = (ELeaveApplication)leaveAppList[0];
                return getDAWAsOfDate(dbConn, EmpID, LeaveCodeID, leaveApp.LeaveAppDateFrom);
            }
            return LeaveDateFrom;
            
        }
        public static double DailyProrataCaluclation(DatabaseConnection dbConn, int ProrataFormulaID, int DefaultPayrollFormulaID, int EmpID, double amount, DateTime periodFrom, DateTime periodTo, int numOfPeriodPerYear, DateTime asOfDate, out string FormulaRemarkString, out bool IsDAW)
        {
            return DailyProrataCaluclation(dbConn, ProrataFormulaID, DefaultPayrollFormulaID, EmpID, amount, periodFrom, periodTo, numOfPeriodPerYear, asOfDate, 0, out FormulaRemarkString, out IsDAW);
        }

        public static double DailyProrataCaluclation(DatabaseConnection dbConn, int ProrataFormulaID, int DefaultPayrollFormulaID, int EmpID, double amount, DateTime periodFrom, DateTime periodTo, int numOfPeriodPerYear, DateTime asOfDate, int LeaveCodeID, out string FormulaRemarkString, out bool IsDAW)
        {
            EPayrollProrataFormula prorataFormula = new EPayrollProrataFormula();

            prorataFormula.PayFormID = ProrataFormulaID;
            if (!EPayrollProrataFormula.db.select(dbConn, prorataFormula))
                return DailyProrataCaluclation(dbConn, EPayrollProrataFormula.DEFAULT_FOEMULA_CODE, DefaultPayrollFormulaID, EmpID, amount, periodFrom, periodTo, numOfPeriodPerYear, asOfDate, LeaveCodeID, out FormulaRemarkString, out IsDAW);
            if (prorataFormula.PayFormIsSys.Equals("Y", StringComparison.CurrentCultureIgnoreCase))
                return DailyProrataCaluclation(dbConn, prorataFormula.PayFormCode, DefaultPayrollFormulaID, EmpID, amount, periodFrom, periodTo, numOfPeriodPerYear, asOfDate, LeaveCodeID, out FormulaRemarkString, out IsDAW);
            else
            {
                double dailyAmount = 0;

                if (prorataFormula.ReferencePayFormID > 0)
                {
                    dailyAmount = DailyProrataCaluclation(dbConn, prorataFormula.ReferencePayFormID, DefaultPayrollFormulaID, EmpID, amount, periodFrom, periodTo, numOfPeriodPerYear, asOfDate, LeaveCodeID, out FormulaRemarkString, out IsDAW);
                }
                else
                {
                    IsDAW = false;

                    dailyAmount = amount * prorataFormula.PayFormMultiplier / prorataFormula.PayFormDivider;
                    FormulaRemarkString = amount.ToString("$#,##0.00##")
                        + (prorataFormula.PayFormMultiplier.Equals(1) ? "" : " x " + prorataFormula.PayFormMultiplier)
                        + (prorataFormula.PayFormDivider.Equals(1) ? "" : " / " + prorataFormula.PayFormDivider);
                }
                if (prorataFormula.PayFormDecimalPlace < 9)
                {
                    if (!string.IsNullOrEmpty(prorataFormula.PayFormRoundingRule))

                        //  To prevent unexpected value of RoundingRule change the original value of FormulaRemark,
                        //  All re-assign value of FormulaRemark should be done inside the condition.
                        if (prorataFormula.PayFormRoundingRule.Equals(Values.ROUNDING_RULE_ROUND_DOWN))
                        {
                            dailyAmount = HROne.CommonLib.GenericRoundingFunctions.RoundingDown(dailyAmount, prorataFormula.PayFormDecimalPlace, prorataFormula.PayFormDecimalPlace);
                            FormulaRemarkString = dailyAmount.ToString("$#,##0." + string.Empty.PadLeft(prorataFormula.PayFormDecimalPlace, '#'));
                        }
                        else if (prorataFormula.PayFormRoundingRule.Equals(Values.ROUNDING_RULE_ROUND_UP))
                        {
                            dailyAmount = HROne.CommonLib.GenericRoundingFunctions.RoundingUp(dailyAmount, prorataFormula.PayFormDecimalPlace, prorataFormula.PayFormDecimalPlace);
                            FormulaRemarkString = dailyAmount.ToString("$#,##0." + string.Empty.PadLeft(prorataFormula.PayFormDecimalPlace, '#'));
                        }
                        else if (prorataFormula.PayFormRoundingRule.Equals(Values.ROUNDING_RULE_ROUND_TO))
                        {
                            dailyAmount = HROne.CommonLib.GenericRoundingFunctions.RoundingTo(dailyAmount, prorataFormula.PayFormDecimalPlace, prorataFormula.PayFormDecimalPlace);
                            FormulaRemarkString = dailyAmount.ToString("$#,##0." + string.Empty.PadLeft(prorataFormula.PayFormDecimalPlace, '#'));
                        }

                }
                return dailyAmount;
            }
        }

        /**
         * Rounding a number larger than the original number
         * e.g. 0.1 round up to 1, -0.1 round up to 0
         **/
        [Obsolete("Replaced by HROne.CommonLib.GenericRoundingRule.RoundingUp(double amount, int decimalPlace, int defaultDecimalPlace)")]
        public static double RoundingUp(double amount, int decimalPlace, int defaultDecimalPlace)
        {
            ////  Tested Number
            ////  -   6049.30
            //int multipilier = (int)Math.Pow(10, decimalPlace); 
            //amount *= multipilier;
            //amount = Math.Ceiling(amount);
            //return Math.Round(amount / multipilier, defaultDecimalPlace, MidpointRounding.AwayFromZero);
            return HROne.CommonLib.GenericRoundingFunctions.RoundingUp(amount, decimalPlace, defaultDecimalPlace);
        }

        [Obsolete("Replaced by HROne.CommonLib.GenericRoundingRule.RoundingUp(double amount, int decimalPlace, int defaultDecimalPlace, bool AbsoluteValueOnly)")]
        public static double RoundingUp(double amount, int decimalPlace, int defaultDecimalPlace, bool AbsoluteValueOnly)
        {
            //if (amount > 0 || !AbsoluteValueOnly)
            //    return RoundingUp(amount, decimalPlace, defaultDecimalPlace);
            //else
            //    return -RoundingUp(Math.Abs(amount), decimalPlace, defaultDecimalPlace);
            return HROne.CommonLib.GenericRoundingFunctions.RoundingUp(amount, decimalPlace, defaultDecimalPlace, AbsoluteValueOnly);
        }

        /**
         * Rounding a number smaller than the original number
         * e.g. 0.1 round up to 1, -0.1 round up to 0
         **/
        [Obsolete("Replaced by HROne.CommonLib.GenericRoundingRule.RoundingDown(double amount, int decimalPlace, int defaultDecimalPlace)")]
        public static double RoundingDown(double amount, int decimalPlace, int defaultDecimalPlace)
        {
            //int multipilier = (int)Math.Pow(10, decimalPlace); 
            //amount *= multipilier;
            //amount = Math.Floor(amount);
            //return Math.Round(amount / multipilier, defaultDecimalPlace, MidpointRounding.AwayFromZero);
            return HROne.CommonLib.GenericRoundingFunctions.RoundingDown(amount, decimalPlace, defaultDecimalPlace);
        }

        [Obsolete("Replaced by HROne.CommonLib.GenericRoundingRule.RoundingDown(double amount, int decimalPlace, int defaultDecimalPlace, bool AbsoluteValueOnly)")]
        public static double RoundingDown(double amount, int decimalPlace, int defaultDecimalPlace, bool AbsoluteValueOnly)
        {
            //if (amount > 0 || !AbsoluteValueOnly)
            //    return RoundingDown(amount, decimalPlace, defaultDecimalPlace);
            //else
            //    return -RoundingDown(Math.Abs(amount), decimalPlace, defaultDecimalPlace);
            return HROne.CommonLib.GenericRoundingFunctions.RoundingDown(amount, decimalPlace, defaultDecimalPlace, AbsoluteValueOnly);
        }

        [Obsolete("Replaced by HROne.CommonLib.GenericRoundingRule.RoundingTo(double amount, int decimalPlace, int defaultDecimalPlace)")]
        public static double RoundingTo(double amount, int decimalPlace, int defaultDecimalPlace)
        {
            //int multipilier = (int)Math.Pow(10, decimalPlace); 
            //amount *= multipilier;
            //amount = Math.Round(amount, 2 + defaultDecimalPlace - decimalPlace, MidpointRounding.AwayFromZero);  //  DO NOT DELETE!! To prevent the truncation error after multipler.
            //amount = Math.Round(amount, 0, MidpointRounding.AwayFromZero);
            //return Math.Round(amount / multipilier, defaultDecimalPlace, MidpointRounding.AwayFromZero);
            return HROne.CommonLib.GenericRoundingFunctions.RoundingTo(amount, decimalPlace, defaultDecimalPlace);
        }

        //public static double DailyAverageWages(int EmpID, DateTime asOfDate)
        //{
        //    double totalAmount, totalDays;
        //    DateTime DAWPeriodFr, DAWPeriodTo;
        //    return DailyAverageWages(EmpID, asOfDate,out totalAmount,out totalDays, out DAWPeriodFr, out DAWPeriodTo);
        //}

        //public static double DailyAverageWages(int EmpID, DateTime asOfDate, out double totalAmount, out double totalDays, out DateTime DAWPeriodFrom, out DateTime DAWPeriodTo)
        //{
        //    AverageWages averageWages = new AverageWages(dbConn, EmpID, asOfDate);
        //    totalAmount = averageWages.TotalWagesWithoutOT;
        //    totalDays = averageWages.TotalDays;
        //    DAWPeriodFrom = averageWages.PeriodFrom;
        //    DAWPeriodTo = averageWages.PeriodTo;
        //    return averageWages.DailyWagesWithoutOT();
        //    /*
        //    totalDays = 0;
        //    totalAmount = 0;
        //    DAWPeriodTo = new DateTime();
        //    DAWPeriodFrom = new DateTime();

        //    DBFilter lastPaylPeriodFilter = new DBFilter();
        //    lastPaylPeriodFilter.add(new Match("payPeriodTo", "<", asOfDate));

        //    DBFilter EmpIDFilter = new DBFilter();
        //    EmpIDFilter.add(new Match("EmpID", "=", EmpID));

        //    IN lastPaylPeriodInFilter = new IN("PayPeriodID", "Select PayPeriodID from EMPPayroll", EmpIDFilter);
        //    lastPaylPeriodFilter.add(lastPaylPeriodInFilter);
        //    lastPaylPeriodFilter.add("payPeriodTo", false);
        //    lastPaylPeriodFilter.loadData("select top 1 * from PayrollPeriod");
        //    ArrayList lastPayPeriods = EPayrollPeriod.db.select(dbConn, lastPaylPeriodFilter);
        //    if (lastPayPeriods.Count > 0)
        //    {
        //        EPayrollPeriod lastPayPeriod = (EPayrollPeriod)lastPayPeriods[0];
        //        DAWPeriodTo = lastPayPeriod.PayPeriodTo;
        //        DAWPeriodFrom = DAWPeriodTo.AddDays(1).AddYears(-1);

        //        DBFilter EmpPayrollFilter = new DBFilter();
        //        EmpPayrollFilter.add(new Match("EmpID", "=", EmpID));

        //        DBFilter PaylPeriodFilter = new DBFilter();
        //        PaylPeriodFilter.add(new Match("payPeriodFr", "<=", DAWPeriodTo));
        //        PaylPeriodFilter.add(new Match("payPeriodTo", ">=", DAWPeriodFrom));
        //        IN PaylPeriodInFilter = new IN("PayPeriodID", "Select PayPeriodID from PayrollPeriod", PaylPeriodFilter);
        //        EmpPayrollFilter.add(PaylPeriodInFilter);

        //        ArrayList empPayrolls = EEmpPayroll.db.select(dbConn, EmpPayrollFilter);

        //        // initialize Payment Code Filter
        //        DBFilter paymentCodeFilter = new DBFilter();
        //        paymentCodeFilter.add(new Match("PaymentCodeIsWages", "<>", 0));
        //        IN paymentCodeInFilter = new IN("PaymentCodeID", "Select PaymentCodeID from PaymentCode", paymentCodeFilter);

        //        foreach (EEmpPayroll empPayroll in empPayrolls)
        //        {
        //            totalDays += empPayroll.EmpPayNumOfDayCount;

        //            DBFilter paymentRecordFilter = new DBFilter();
        //            paymentRecordFilter.add(new Match("EmpPayrollID", empPayroll.EmpPayrollID));
        //            paymentRecordFilter.add(paymentCodeInFilter);

        //            ArrayList paymentRecords = EPaymentRecord.db.select(dbConn, paymentRecordFilter);

        //            foreach (EPaymentRecord paymentRecord in paymentRecords)
        //            {
        //                totalDays += paymentRecord.PayRecNumOfDayAdj;
        //                totalAmount += paymentRecord.PayRecActAmount;
        //            }
        //        }
        //        if (totalAmount <= 0 || totalDays <= 0)
        //            return 0;
        //        else
        //            return totalAmount / totalDays;

        //    }
        //    else
        //        return 0;
        //    */
        //}

        //public static void ExcludeWages(int EmpID, DateTime asOfDate, out double totalAmount, out double totalDays, out DateTime DAWPeriodFrom, out DateTime DAWPeriodTo)
        //{
        //    AverageWages averageWages = new AverageWages(dbConn, EmpID, asOfDate);
        //    totalAmount = averageWages.TotalWagesExclude;
        //    totalDays = averageWages.TotalDaysExclude;
        //    DAWPeriodFrom = averageWages.PeriodFrom;
        //    DAWPeriodTo = averageWages.PeriodTo;
        //    return;
        //    /*
        //    totalDays = 0;
        //    totalAmount = 0;
        //    DAWPeriodTo = new DateTime();
        //    DAWPeriodFrom = new DateTime();

        //    DBFilter lastPaylPeriodFilter = new DBFilter();
        //    lastPaylPeriodFilter.add(new Match("payPeriodTo", "<", asOfDate));

        //    DBFilter EmpIDFilter = new DBFilter();
        //    EmpIDFilter.add(new Match("EmpID", "=", EmpID));

        //    IN lastPaylPeriodInFilter = new IN("PayPeriodID", "Select PayPeriodID from EMPPayroll", EmpIDFilter);
        //    lastPaylPeriodFilter.add(lastPaylPeriodInFilter);
        //    lastPaylPeriodFilter.add("payPeriodTo", false);
        //    lastPaylPeriodFilter.loadData("select top 1 * from PayrollPeriod");
        //    ArrayList lastPayPeriods = EPayrollPeriod.db.select(dbConn, lastPaylPeriodFilter);
        //    if (lastPayPeriods.Count > 0)
        //    {
        //        EPayrollPeriod lastPayPeriod = (EPayrollPeriod)lastPayPeriods[0];
        //        DAWPeriodTo = lastPayPeriod.PayPeriodTo;
        //        DAWPeriodFrom = DAWPeriodTo.AddDays(1).AddYears(-1);

        //        DBFilter EmpPayrollFilter = new DBFilter();
        //        EmpPayrollFilter.add(new Match("EmpID", "=", EmpID));

        //        DBFilter PaylPeriodFilter = new DBFilter();
        //        PaylPeriodFilter.add(new Match("payPeriodFr", "<=", DAWPeriodTo));
        //        PaylPeriodFilter.add(new Match("payPeriodTo", ">=", DAWPeriodFrom));
        //        IN PaylPeriodInFilter = new IN("PayPeriodID", "Select PayPeriodID from PayrollPeriod", PaylPeriodFilter);
        //        EmpPayrollFilter.add(PaylPeriodInFilter);

        //        ArrayList empPayrolls = EEmpPayroll.db.select(dbConn, EmpPayrollFilter);

        //        // initialize Payment Code Filter
        //        DBFilter paymentCodeFilter = new DBFilter();
        //        paymentCodeFilter.add(new Match("PaymentCodeIsWages", "=", 0));
        //        IN paymentCodeInFilter = new IN("PaymentCodeID", "Select PaymentCodeID from PaymentCode", paymentCodeFilter);

        //        foreach (EEmpPayroll empPayroll in empPayrolls)
        //        {
        //            //totalDays += empPayroll.EmpPayNumOfDayCount;

        //            DBFilter paymentRecordFilter = new DBFilter();
        //            paymentRecordFilter.add(new Match("EmpPayrollID", empPayroll.EmpPayrollID));
        //            paymentRecordFilter.add(paymentCodeInFilter);

        //            ArrayList paymentRecords = EPaymentRecord.db.select(dbConn, paymentRecordFilter);

        //            foreach (EPaymentRecord paymentRecord in paymentRecords)
        //            {
        //                totalDays += paymentRecord.PayRecNumOfDayAdj;
        //                totalAmount += paymentRecord.PayRecActAmount;
        //            }
        //        }

        //    }
        //    */
        //}

        //public static double OTDailyAverageWages(int EmpID, DateTime asOfDate)
        //{
        //    double totalAmount, totalDays;
        //    DateTime DAWPeriodFr, DAWPeriodTo;
        //    return OTDailyAverageWages(EmpID, asOfDate, out totalAmount, out totalDays, out DAWPeriodFr, out DAWPeriodTo);
        //}

        //public static double OTDailyAverageWages(int EmpID, DateTime asOfDate, out double totalAmount, out double totalDays, out DateTime DAWPeriodFrom, out DateTime DAWPeriodTo)
        //{
        //    AverageWages averageWages = new AverageWages(dbConn, EmpID, asOfDate);
        //    totalAmount = averageWages.TotalOTWages;
        //    totalDays = averageWages.TotalDays;
        //    DAWPeriodFrom = averageWages.PeriodFrom;
        //    DAWPeriodTo = averageWages.PeriodTo;
        //    return averageWages.OTDailyWages();
        //    /*
        //    totalDays = 0;
        //    totalAmount = 0;
        //    DAWPeriodTo = new DateTime(); 
        //    DAWPeriodFrom = new DateTime(); 

        //    DBFilter lastPaylPeriodFilter = new DBFilter();
        //    lastPaylPeriodFilter.add(new Match("payPeriodTo", "<", asOfDate));

        //    DBFilter EmpIDFilter = new DBFilter();
        //    EmpIDFilter.add(new Match("EmpID", "=", EmpID));

        //    IN lastPaylPeriodInFilter = new IN("PayPeriodID", "Select PayPeriodID from EMPPayroll", EmpIDFilter);
        //    lastPaylPeriodFilter.add(lastPaylPeriodInFilter);
        //    lastPaylPeriodFilter.add("payPeriodTo", false);
        //    lastPaylPeriodFilter.loadData("select top 1 * from PayrollPeriod");
        //    ArrayList lastPayPeriods = EPayrollPeriod.db.select(dbConn, lastPaylPeriodFilter);
        //    if (lastPayPeriods.Count > 0)
        //    {
        //        EPayrollPeriod lastPayPeriod = (EPayrollPeriod)lastPayPeriods[0];
        //        DAWPeriodTo = lastPayPeriod.PayPeriodTo;
        //        DAWPeriodFrom = DAWPeriodTo.AddDays(1).AddYears(-1);

        //        DBFilter EmpPayrollFilter = new DBFilter();
        //        EmpPayrollFilter.add(new Match("EmpID", "=", EmpID));

        //        DBFilter PaylPeriodFilter = new DBFilter();
        //        PaylPeriodFilter.add(new Match("payPeriodFr", "<=", DAWPeriodTo));
        //        PaylPeriodFilter.add(new Match("payPeriodTo", ">=", DAWPeriodFrom));
        //        IN PaylPeriodInFilter = new IN("PayPeriodID", "Select PayPeriodID from PayrollPeriod", PaylPeriodFilter);
        //        EmpPayrollFilter.add(PaylPeriodInFilter);

        //        ArrayList empPayrolls = EEmpPayroll.db.select(dbConn, EmpPayrollFilter);

        //        // initialize Payment Code Filter
        //        DBFilter paymentTypeFilter = new DBFilter();
        //        paymentTypeFilter.add(new Match("PaymentTypeCode","OTPAY"));
        //        IN paymentTypeTerms = new IN("PaymentTypeID", "Select PaymentTypeID from PaymentType", paymentTypeFilter);

        //        DBFilter paymentCodeFilter = new DBFilter();
        //        paymentCodeFilter.add(new Match("PaymentCodeIsWages", "=", 0));
        //        paymentCodeFilter.add(paymentTypeTerms);
        //        IN paymentCodeInFilter = new IN("PaymentCodeID", "Select PaymentCodeID from PaymentCode", paymentCodeFilter);

        //        foreach (EEmpPayroll empPayroll in empPayrolls)
        //        {
        //            totalDays += empPayroll.EmpPayNumOfDayCount;

        //            DBFilter paymentRecordFilter = new DBFilter();
        //            paymentRecordFilter.add(new Match("EmpPayrollID", empPayroll.EmpPayrollID));
        //            paymentRecordFilter.add(paymentCodeInFilter);

        //            ArrayList paymentRecords = EPaymentRecord.db.select(dbConn, paymentRecordFilter);

        //            foreach (EPaymentRecord paymentRecord in paymentRecords)
        //            {
        //                totalDays += paymentRecord.PayRecNumOfDayAdj;
        //                totalAmount += paymentRecord.PayRecActAmount;
        //            }
        //        }
        //        if (totalAmount <= 0 || totalDays <= 0)
        //            return 0;
        //        else
        //            return totalAmount / totalDays;

        //    }
        //    else
        //        return 0;
        //    */
        //}

    }
}