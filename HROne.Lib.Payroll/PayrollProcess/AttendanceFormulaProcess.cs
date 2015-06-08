using System;
using System.Collections.Generic;
using System.Text;
using HROne.Lib.Entities;
using HROne.DataAccess;

namespace HROne.Payroll
{
    public abstract class AttendanceFormulaProcess
    {
        //public static double GetHourlyRate(DatabaseConnection dbConn, int AttendanceFormulaID, int DefaultPayrollFormulaID, int EmpID, double totalPeriodPayment, double totalDailyPayment, double totalHourlyPayment, DateTime PayPeriodAttnFr, DateTime PayPeriodTo, int numOfPeriodPerYear, DateTime AttendanceRecordDate, bool IsOverrideHourlyPayment, double OverrideHoulyAmount, double RosterCodeWorkHourPerDay, out string hourlyRateRemark)
        //{
        //    return GetHourlyRate(dbConn, AttendanceFormulaID, DefaultPayrollFormulaID, EmpID, totalPeriodPayment, totalDailyPayment, totalHourlyPayment, PayPeriodAttnFr, PayPeriodTo, numOfPeriodPerYear, AttendanceRecordDate, IsOverrideHourlyPayment, OverrideHoulyAmount, 1, RosterCodeWorkHourPerDay, out hourlyRateRemark);
        //}
        public static double GetHourlyRate(DatabaseConnection dbConn, int AttendanceFormulaID, int DefaultPayrollFormulaID, int EmpID, double totalPeriodPayment, double totalDailyPayment, double totalHourlyPayment, DateTime PayPeriodAttnFr, DateTime PayPeriodTo, int numOfPeriodPerYear, DateTime AttendanceRecordDate, bool IsOverrideHourlyPayment, double OverrideHoulyAmount, double ratio, double RosterCodeWorkHourPerDay, out string hourlyRateRemark)
        {
            hourlyRateRemark = string.Empty;

            double hourlyRate = 0;
            EAttendanceFormula attendanceFormula = new EAttendanceFormula();
            attendanceFormula.AttendanceFormulaID = AttendanceFormulaID;
            if (!EAttendanceFormula.db.select(dbConn, attendanceFormula))
            {
                return 0;
            }
            if (attendanceFormula.AttendanceFormulaType.Equals(EAttendanceFormula.FORMULATYPE_CODE_FIX_RATE))
            {
                hourlyRate = attendanceFormula.AttendanceFormulaFixedRate * ratio;
                hourlyRateRemark = hourlyRate.ToString("$#,##0.00##");
            }
            else if (attendanceFormula.AttendanceFormulaType.Equals(EAttendanceFormula.FORMULATYPE_CODE_BY_FORMULA))
            {
                bool IsDAW = false;
                double dailyRate = HROne.Payroll.PayrollFormula.DailyProrataCaluclation(dbConn, attendanceFormula.AttendanceFormulaPayFormID, DefaultPayrollFormulaID, EmpID, totalPeriodPayment, PayPeriodAttnFr, PayPeriodTo, numOfPeriodPerYear, AttendanceRecordDate, out hourlyRateRemark, out IsDAW);
                if (!IsDAW)
                {
                    if (dailyRate.Equals(0))
                    {
                        hourlyRateRemark = totalDailyPayment.ToString("$#,##0.00##");
                        dailyRate = totalDailyPayment;
                    }
                    else if (!totalDailyPayment.Equals(0))
                    {
                        hourlyRateRemark = "(" + hourlyRateRemark + " + " + totalDailyPayment.ToString("$#,##0.00##") + ")";
                        dailyRate += totalDailyPayment;
                    }
                }
                    double hourlyPaymentRate = 0;
                    if (totalHourlyPayment != 0)
                    {
                        if (IsOverrideHourlyPayment)
                            hourlyPaymentRate = OverrideHoulyAmount;
                        else
                            hourlyPaymentRate = totalHourlyPayment;
                    }
                
                if (dailyRate.Equals(0) || attendanceFormula.AttendanceFormulaWorkHourPerDay.Equals(0))
                {
                    hourlyRateRemark = hourlyPaymentRate.ToString("$#,##0.00##") + (ratio.Equals(1) ? string.Empty : (" x " + ratio.ToString()));
                    hourlyRate = hourlyPaymentRate * ratio;
                }
                else
                {
                    double prorataHourlyRate = dailyRate / attendanceFormula.AttendanceFormulaWorkHourPerDay;// *ratio;
                    string prorataRemark = hourlyRateRemark + (attendanceFormula.AttendanceFormulaWorkHourPerDay.Equals(1) ? string.Empty : (" / " + attendanceFormula.AttendanceFormulaWorkHourPerDay));// +(ratio.Equals(1) ? string.Empty : (" x " + ratio.ToString()));

                    if (RosterCodeWorkHourPerDay > 0 && attendanceFormula.AttendanceFormulaIsUseRosterCodeForDefaultWorkHourPerDay)
                    {

                        prorataHourlyRate = dailyRate / RosterCodeWorkHourPerDay;// *ratio;
                        prorataRemark = hourlyRateRemark + (RosterCodeWorkHourPerDay.Equals(1) ? string.Empty : (" / " + RosterCodeWorkHourPerDay));// +(ratio.Equals(1) ? string.Empty : (" x " + ratio.ToString()));

                    }
                    if (hourlyPaymentRate != 0 && !IsDAW)
                    {
                        prorataRemark = "(" + prorataRemark + " + " + hourlyPaymentRate.ToString("$#,##0.00##") + ")";
                            //+ (ratio.Equals(1) ? string.Empty : (" x " + ratio.ToString())) + ")";
                        prorataHourlyRate = prorataHourlyRate + hourlyPaymentRate;
                    }
                    if (ratio!=1)
                    {
                        prorataRemark += " x " + ratio.ToString();
                        prorataHourlyRate *= ratio;
                    }
                    if (attendanceFormula.AttendanceFormulaDecimalPlace < 9)
                    {
                        if (!string.IsNullOrEmpty(attendanceFormula.AttendanceFormulaRoundingRule))
                            //  To prevent unexpected value of RoundingRule change the original value of FormulaRemark,
                            //  All re-assign value of FormulaRemark should be done inside the condition.
                            if (attendanceFormula.AttendanceFormulaRoundingRule.Equals(Values.ROUNDING_RULE_ROUND_DOWN))
                            {
                                prorataHourlyRate = HROne.CommonLib.GenericRoundingFunctions.RoundingDown(prorataHourlyRate, attendanceFormula.AttendanceFormulaDecimalPlace, attendanceFormula.AttendanceFormulaDecimalPlace);
                                //prorataRemark = prorataHourlyRate.ToString("0." + string.Empty.PadLeft(attendanceFormula.AttendanceFormulaDecimalPlace, '0'));
                            }
                            else if (attendanceFormula.AttendanceFormulaRoundingRule.Equals(Values.ROUNDING_RULE_ROUND_UP))
                            {
                                prorataHourlyRate = HROne.CommonLib.GenericRoundingFunctions.RoundingUp(prorataHourlyRate, attendanceFormula.AttendanceFormulaDecimalPlace, attendanceFormula.AttendanceFormulaDecimalPlace);
                                //prorataRemark = prorataHourlyRate.ToString("0." + string.Empty.PadLeft(attendanceFormula.AttendanceFormulaDecimalPlace, '0'));
                            }
                            else if (attendanceFormula.AttendanceFormulaRoundingRule.Equals(Values.ROUNDING_RULE_ROUND_TO))
                            {
                                prorataHourlyRate = HROne.CommonLib.GenericRoundingFunctions.RoundingTo(prorataHourlyRate, attendanceFormula.AttendanceFormulaDecimalPlace, attendanceFormula.AttendanceFormulaDecimalPlace);
                                //prorataRemark = prorataHourlyRate.ToString("0." + string.Empty.PadLeft(attendanceFormula.AttendanceFormulaDecimalPlace, '0'));
                            }

                    }
                    hourlyRateRemark = prorataRemark;
                    hourlyRate = prorataHourlyRate;

                }
            }
            return hourlyRate;
        }
    }
}
