using System;
using System.Collections.Generic;
using System.Text;
using HROne.DataAccess;
////using perspectivemind.validation;

namespace HROne.Lib.Entities
{
    public class Values
    {
        public const string ROUNDING_RULE_NO_ROUND = "NOROUND";
        public const string ROUNDING_RULE_ROUND_TO = "TO";
        public const string ROUNDING_RULE_ROUND_UP = "UP";
        public const string ROUNDING_RULE_ROUND_DOWN = "DOWN";

        public const string PAYMENT_BASE_MONTHLY_AVERAGE_WAGES = "MAW";
        public const string PAYMENT_BASE_RECURRING_BASIC_SALARY = "BASICSAL";
        // Start 0000070, Miranda, 2014-08-27
        public const string PAYMENT_BASE_FIXED_AMOUNT = "FA";// Fixed Amount
        // End 0000070, Miranda, 2014-08-27

        //public static WFValueList VLEmpStatus = new AppUtils.NewWFTextList(new string[] { "A", "T", "W" }, new string[] { "Active", "Terminate", "Terminated" });
        public static AppUtils.NewWFTextList VLPaymentUnit = new AppUtils.NewWFTextList(new string[] { "H", "D", "P" }, new string[] { "Hourly", "Daily", "Once per payroll cycle" });
        public static AppUtils.NewWFTextList VLLeaveUnit = new AppUtils.NewWFTextList(new string[] { ELeaveApplication.LEAVEUNIT_DAYS, ELeaveApplication.LEAVEUNIT_AM, ELeaveApplication.LEAVEUNIT_PM, ELeaveApplication.LEAVEUNIT_HOUR }, new string[] { "Day", "A.M.", "P.M.", "Hour" });
        //public static WFValueList VLLeaveUnit = new AppUtils.NewWFTextList(new string[] { "D" }, new string[] { "Day"});
        public static AppUtils.NewWFTextList VLEmpUnit = new AppUtils.NewWFTextList(new string[] { "D", "M" }, new string[] { "Day", "Month" });
        public static AppUtils.NewWFTextList VLArea = new AppUtils.NewWFTextList(new string[] { "H", "K", "N", "O" }, new string[] { "Hong Kong", "Kowloon", "New Territories", "Overseas" });
        public static AppUtils.NewWFTextList VLGender = new AppUtils.NewWFTextList(new string[] { "M", "F" }, new string[] { "Male", "Female" });
        public static AppUtils.NewWFTextList VLMaritalStatus = new AppUtils.NewWFTextList(new string[] { "Single", "Widowed", "Divorced", "Separated", "Married" }, new string[] { "Single", "Widowed", "Divorced", "Living Apart", "Married" });
        public static AppUtils.NewWFTextList VLCurrency = new AppUtils.NewWFTextList(new string[] { "HKD", "USD", "EUR" });
        public static AppUtils.NewWFTextList VLPaymentMethod = new AppUtils.NewWFTextList(new string[] { "A", "C", "Q", "O" }, new string[] { "Autopay", "Cash", "Cheque", "Other" });
        public static AppUtils.NewWFTextList VLUsrPasswordUnit = new AppUtils.NewWFTextList(new string[] { "D", "M", "Y" }, new string[] { "Day", "Month", "Year" });
        public static AppUtils.NewWFTextList VLDependantRelationship = new AppUtils.NewWFTextList(
            new string[] { "brother", "brother-in-law", "daughter", "father", "father-in-law", "friend", "mother", "mother-in-law", "other relative", "sister", "sister-in-law", "son", "spouse" },
            new string[] { "brother", "brother-in-law", "daughter", "father", "father-in-law", "friend", "mother", "mother-in-law", "other relative", "sister", "sister-in-law", "son", "Spouse" });
        public static AppUtils.NewWFTextList VLYesNo = new AppUtils.NewWFTextList(new string[] { "1", "0" }, new string[] { "Yes", "No" });
        public static AppUtils.NewWFTextList VLTrueFalseYesNo = new AppUtils.NewWFTextList(new string[] { "True", "False" }, new string[] { "Yes", "No" });
        public static AppUtils.NewWFTextList VLMonth
            = new AppUtils.NewWFTextList(new string[] { "1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12" }
            , new string[] { "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December" });
        public static AppUtils.NewWFTextList VLRoundingRule = new AppUtils.NewWFTextList(new string[] { ROUNDING_RULE_ROUND_TO, ROUNDING_RULE_ROUND_UP, ROUNDING_RULE_ROUND_DOWN }, new string[] { "Round to", "Round up", "Round down" });
        public static AppUtils.NewWFTextList VLRoundingRuleWithNoRounding = new AppUtils.NewWFTextList(new string[] { ROUNDING_RULE_NO_ROUND, ROUNDING_RULE_ROUND_TO, ROUNDING_RULE_ROUND_UP, ROUNDING_RULE_ROUND_DOWN }, new string[] { "No Rounding", "Round to", "Round up", "Round down" });
        public static AppUtils.NewWFTextList VLDecimalPlace = new AppUtils.NewWFTextList(new string[] { "2", "1", "0" });
        public static AppUtils.NewWFTextList VL8DecimalPlace = new AppUtils.NewWFTextList(new string[] { "8", "7", "6", "5", "4", "3", "2", "1", "0" });

        public static WFValueList VLPaymentBaseMethod = new AppUtils.NewWFTextList(new string[] { PAYMENT_BASE_MONTHLY_AVERAGE_WAGES, PAYMENT_BASE_RECURRING_BASIC_SALARY }, new string[] { "Monthly Average Wages", "Recurring Basic Salary" });
    }
}
