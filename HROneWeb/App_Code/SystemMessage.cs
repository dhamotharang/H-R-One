using System;
using System.Data;
using System.Configuration;
using System.Globalization;
using HROne.DataAccess;
using HROne.Common;

namespace HROne.Translation
{

    public static class PageMessage
    {
        public static string IMPORT_SUCCESSFUL
        {
            get { return HROne.Common.WebUtility.GetLocalizedStringByCode("IMPORT_SUCCESSFUL", "Import Successful"); }
        }
        public static string REPORT_GENERATING_TO_INBOX
        {
            get { return HROne.Common.WebUtility.GetLocalizedStringByCode("REPORT_GENERATING_TO_INBOX", "Report is generating. Please check the inbox later"); }
        }
        public static string INBOX_SIZE_EXCEEDED
        {
            get { return HROne.Common.WebUtility.GetLocalizedStringByCode("INBOX_SIZE_EXCEEDED", "Exceed maximium inbox size, please delete some inbox messages"); }
        }
        public static string PRODUCT_TRIAL_WILL_EXPIRE
        {
            get { return HROne.Common.WebUtility.GetLocalizedStringByCode("PRODUCT_TRIAL_WILL_EXPIRE", "Trial Period will expire after {0} day(s)."); }
        }
        public static string BANKFILE_CONSOLIDATION_MESSAGE
        {
            get { return HROne.Common.WebUtility.GetLocalizedStringByCode("BANKFILE_CONSOLIDATION_MESSAGE", "The cut-off time for submitting payroll/MPF file is 5:00pm Monday to Saturday excluding public holidays."); }
        }
        public static string BANKFILE_CLEARING_MESSAGE
        {
            get { return HROne.Common.WebUtility.GetLocalizedStringByCode("BANKFILE_CLEARING_MESSAGE", "Because of 5-Day Clearing Week, clearing will take place from Mondays to Fridays only (except public holidays)"); }
        }
        public static string BANKFILE_REPORT_STORE_PERIOD_MESSAGE
        {
            get { return HROne.Common.WebUtility.GetLocalizedStringByCode("BANKFILE_REPORT_STORE_PERIOD_MESSAGE", "All bank messages will be stored for 90 days from date of receipt.Please download all reports for your backup."); }
        }
    }

    public static class PageErrorMessage 
    {
        public static string ERROR_DATE_FROM_OVERLAP
        {
            get { return WebUtility.GetLocalizedStringByCode("ERROR_DATE_FROM_OVERLAP", "From date cannot be overlapped."); }
        }
        public static string ERROR_ACCOUNT_REQUIRED
        {
            get { return WebUtility.GetLocalizedStringByCode("ERROR_ACCOUNT_REQUIRED", "Bank account number is required."); }
        }
        public static string ERROR_DATE_TO_TOO_EARLY
        {
            get { return WebUtility.GetLocalizedStringByCode("ERROR_DATE_TO_TOO_EARLY", "To date cannot be earlier than From date."); }
        }
        public static string ERROR_MPF_DATE_TO_TOO_EARLY
        {
            get { return WebUtility.GetLocalizedStringByCode("ERROR_MPF_DATE_TO_TOO_EARLY", "Effective date cannot be earlier then 2000-12-01."); }
        }
        public static string ERROR_TERMINATION_RECORD_NOT_FOUND
        {
            get { return WebUtility.GetLocalizedStringByCode("ERROR_TERMINATION_RECORD_NOT_FOUND", "Termination record has not been found."); }
        }
        public static string ERROR_EMP_NOT_TRANSFERABLE
        {
            get { return WebUtility.GetLocalizedStringByCode("ERROR_EMP_NOT_TRANSFERABLE", "Employee is not transferable."); }
        }
        public static string ERROR_EMP_TRANSFERRED_BEFORE
        {
            get { return WebUtility.GetLocalizedStringByCode("ERROR_EMP_TRANSFERRED_BEFORE", "Employee has been transferred before. (Emp No: {0})"); }
        }
        public static string ERROR_CODE_USED_BY_EMPLOYEE
        {
            get { return WebUtility.GetLocalizedStringByCode("ERROR_CODE_USED_BY_EMPLOYEE", "{0} '{1}' is used by the following employee: "); }
        }
        public static string ERROR_CODE_IS_IN_USE
        {
            get { return WebUtility.GetLocalizedStringByCode("ERROR_CODE_IS_IN_USE", "{0} is in use. "); }
        }

        public static string ERROR_LEAVE_APP_OVERLAP
        {
            get { return WebUtility.GetLocalizedStringByCode("ERROR_LEAVE_APP_OVERLAP", "Leave dates cannot overlap with previous leave applications"); }
        }

        public static string ERROR_LEAVE_APP_TIME_OVERLAP
        {
            get { return WebUtility.GetLocalizedStringByCode("ERROR_LEAVE_APP_TIME_OVERLAP", "Leave time cannot overlap with previous leave applications"); }
        }

        public static string ERROR_POS_OVERLAP
        {
            get { return WebUtility.GetLocalizedStringByCode("ERROR_POS_OVERLAP", "Effective dates cannot overlap with previous positions"); }
        }

        public static string ERROR_TERMS_OVERLAP
        {
            get { return WebUtility.GetLocalizedStringByCode("ERROR_TERMS_OVERLAP", "Effective dates cannot overlap with previous terms"); }
        }

        public static string ERROR_DAYS_TOO_LARGE
        {
            get { return WebUtility.GetLocalizedStringByCode("ERROR_DAYS_TOO_LARGE", "Days taken is too large"); }
        }

        public static string ERROR_INVALID_HOUR
        {
            get { return WebUtility.GetLocalizedStringByCode("ERROR_INVALID_HOUR", "Invalid hours"); }
        }

        public static string ERROR_LEAVE_APP_NOT_SAME_MTH
        {
            get { return WebUtility.GetLocalizedStringByCode("ERROR_LEAVE_APP_NOT_SAME_MTH", "Leave application must be within the same month"); }
        }

        public static string ERROR_LEAVE_APP_HOURS_CLAIM_REQUIRED
        {
            get { return WebUtility.GetLocalizedStringByCode("ERROR_LEAVE_APP_HOURS_CLAIM_REQUIRED", "Hours Claim is required"); }
        }

        public static string ERROR_INCORRECT_PASSWORD
        {
            get { return WebUtility.GetLocalizedStringByCode("ERROR_INCORRECT_PASSWORD", "Incorrect password"); }
        }

        public static string ERROR_CONFIRM_PASSWORD
        {
            get { return WebUtility.GetLocalizedStringByCode("ERROR_CONFIRM_PASSWORD", "Confirm Password does not match"); }
        }

        public static string ERROR_PASSWORD_NOT_MATCH
        {
            get { return WebUtility.GetLocalizedStringByCode("ERROR_PASSWORD_NOT_MATCH", "Password does not match"); }
        }

        public static string ERROR_PASSWORD_SAME_AS_OLD_PASSWORD
        {
            get { return WebUtility.GetLocalizedStringByCode("ERROR_PASSWORD_SAME_AS_OLD_PASSWORD", "New password should not be same as old password"); }
        }
        
        public static string ERROR_INVALID_DOB
        {
            get { return WebUtility.GetLocalizedStringByCode("ERROR_INVALID_DOB", "Invalid date of birth"); }
        }

        public static string ERROR_ONE_PAYMENT_PER_PCODE
        {
            get { return WebUtility.GetLocalizedStringByCode("ERROR_ONE_PAYMENT_PER_PCODE", "There can only be one payment under the same payment code"); }
        }

        public static string ERROR_LEAVE_ADJ_OVERLAP
        {
            get { return WebUtility.GetLocalizedStringByCode("ERROR_LEAVE_ADJ_OVERLAP", "Adjust dates cannot overlap with previous leave adjustment"); }
        }

        public static string ERROR_PAY_EFF_DATE_OVERLAP
        {
            get { return WebUtility.GetLocalizedStringByCode("ERROR_PAY_EFF_DATE_OVERLAP", "Effective dates cannot overlap with previous payments under the same payment code"); }
        }

        public static string ERROR_DUPLICATE_YEAR_SERVICE
        {
            get { return WebUtility.GetLocalizedStringByCode("ERROR_DUPLICATE_YEAR_SERVICE", "Duplicated year of service"); }
        }

        public static string ERROR_DIVIDER_NOT_ZERO
        {
            get { return WebUtility.GetLocalizedStringByCode("ERROR_DIVIDER_NOT_ZERO", "Divider cannot be zero."); }
        }

        public static string ERROR_INVALID_START_DATE
        {
            get { return "{0}: " + WebUtility.GetLocalizedStringByCode("ERROR_INVALID_START_DATE", "Invald start date"); }
        }

        public static string ERROR_INVALID_END_DATE
        {
            get { return "{0}: " + WebUtility.GetLocalizedStringByCode("ERROR_INVALID_END_DATE", "Invald end date"); }
        }

        public static string ERROR_INVALID_PERIOD
        {
            get { return WebUtility.GetLocalizedStringByCode("ERROR_INVALID_PERIOD", "Invald start/end dates"); }
        }

        public static string ERROR_REC_CND_NOT_SELECT
        {
            get { return WebUtility.GetLocalizedStringByCode("ERROR_REC_CND_NOT_SELECT", "Recurring Payment or Claims And Deduction should be selected!"); }
        }

        public static string ERROR_EMP_TYPE_NOT_SELECT
        {
            get { return WebUtility.GetLocalizedStringByCode("ERROR_EMP_TYPE_NOT_SELECT", "At least one of the employee type should be selected!"); }
        }

        public static string ERROR_INCORRECT_DATE_RANGE
        {
            get { return WebUtility.GetLocalizedStringByCode("ERROR_INCORRECT_DATE_RANGE", "Incorrect Date Range"); }
        }

        public static string ERROR_DATE_END_TOO_EARLY
        {
            get { return WebUtility.GetLocalizedStringByCode("ERROR_DATE_END_TOO_EARLY", "End Date should be after Start Date"); }
        }
        public static string ERROR_DELETE_EMP_PAYMENT_EXISTS
        {
            get { return WebUtility.GetLocalizedStringByCode("ERROR_DELETE_EMP_PAYMENT_EXISTS", "Emp no {0} has payment record(s). Delete action abort!"); }
            
        }
        public static string ERROR_ACTION_ABORT
        {
            get { return WebUtility.GetLocalizedStringByCode("ERROR_ACTION_ABORT", "Action abort!"); }
        }

        public static string ERROR_FIELD_REQUIRED
        {
            get { return WebUtility.GetLocalizedStringByCode("ERROR_FIELD_REQUIRED", "{0} is required."); }
        }
        public static string ERROR_LEAVE_ALLOWANCE_SHOULD_BE_WAGES
        {
            get { return WebUtility.GetLocalizedStringByCode("ERROR_LEAVE_ALLOWANCE_SHOULD_BE_WAGES", "Leave Allowance should be wages"); }
        }
        public static string ERROR_LEAVE_ALLOWANCE_SHOULD_NOT_BE_WAGES
        {
            get { return WebUtility.GetLocalizedStringByCode("ERROR_LEAVE_ALLOWANCE_SHOULD_NOT_BE_WAGES", "Leave Allowance should not be wages"); }
        }
        public static string ERROR_INVALID_FILE
        {
            get { return WebUtility.GetLocalizedStringByCode("ERROR_INVALID_FILE", "Invalid file or file not selected!"); }
        }
        public static string ERROR_HKID_PASSPORTCOUNTRY_REQUIRED
        {
            get { return WebUtility.GetLocalizedStringByCode("ERROR_HKID_PASSPORTCOUNTRY_REQUIRED", "Either HKID or Passport Number with Issued Country is required"); }
        }
        public static string ERROR_HKID_PASSPORT_REQUIRED
        {
            get { return WebUtility.GetLocalizedStringByCode("ERROR_HKID_PASSPORT_REQUIRED", "Either HKID or Passport Number is required"); }
        }
        public static string ERROR_MAX_LICENSE_LIMITCH_REACH
        {
            get { return WebUtility.GetLocalizedStringByCode("ERROR_MAX_LICENSE_LIMITCH_REACH", "The maximum license limited has reach ({0}). Action Abort!"); }
        }
        public static string ERROR_INVALID_VALUE_DATE
        {
            get { return WebUtility.GetLocalizedStringByCode("ERROR_INVALID_VALUE_DATE", "Incorrect Value Date format."); }
        }
        public static string ERROR_TOTAL_PERCENTAGE_NOT_100
        {
            get { return WebUtility.GetLocalizedStringByCode("ERROR_TOTAL_PERCENTAGE_NOT_100", "Total percentage must be 100%"); } 
        }
        public static string ERROR_NUMBR_RANGE
        {
            get { return WebUtility.GetLocalizedStringByCode("ERROR_NUMBR_RANGE", "Number must be between {0} and {1}"); }
        }
        public static string ERROR_DATE_FORMAT
        {
            get { return WebUtility.GetLocalizedStringByCode("ERROR_DATE_FORMAT", "Invalid Date Format!"); }
        }
        public static string ERROR_CND_IMPORT_BATCH_PAYROLL_PROCESSING
        {
            get { return WebUtility.GetLocalizedStringByCode("ERROR_CND_IMPORT_BATCH_PAYROLL_PROCESSING", "Fail to undo this import batch because the following employees are processed:"); }
        }
        public static string ERROR_CA_IMPORT_BATCH_PAYROLL_PROCESSING
        {
            get { return WebUtility.GetLocalizedStringByCode("ERROR_CA_IMPORT_BATCH_PAYROLL_PROCESSING", "Fail to undo this import batch because the following employees are processed:"); }
        }
        public static string ERROR_IP_IMPORT_BATCH_PAYROLL_PROCESSING
        {
            get { return WebUtility.GetLocalizedStringByCode("ERROR_IP_IMPORT_BATCH_PAYROLL_PROCESSING", "Fail to undo this import batch because the following employees are processed:"); }
        }
        public static string ERROR_NO_RECORD_IMPORT
        {
            get { return WebUtility.GetLocalizedStringByCode("ERROR_NO_RECORD_IMPORT", "No record can be imported."); }
        }
        public static string ERROR_SUPER_USER_REQUIRED
        {
            get { return WebUtility.GetLocalizedStringByCode("ERROR_SUPER_USER_REQUIRED", "The system should be least 1 user allowed to access User Group Setup and User Maintenance."); }
        }
        public static string ERROR_DATE_TOO_EARLY
        {
            get { return WebUtility.GetLocalizedStringByCode("ERROR_DATE_TOO_EARLY", "Date cannot be earlier than Date of Join"); }
        }
        public static string ERROR_DATE_TOO_LATE
        {
            get { return WebUtility.GetLocalizedStringByCode("ERROR_DATE_TOO_LATE", "Date cannot be later than Last Employment Date"); }
        }
        public static string ERROR_INVALID_PERMISSION
        {
            get { return WebUtility.GetLocalizedStringByCode("ERROR_INVALID_PERMISSION", "Invalid Permission!"); }
        }
        public static string ERROR_BANKFILE_NEGATIVE
        {
            get { return WebUtility.GetLocalizedStringByCode("ERROR_BANKFILE_NEGATIVE", "Bank file should not contain negative amount"); }
        }
        public static string ERROR_NO_EMPLOYEE_SELECTED
        {
            get { return WebUtility.GetLocalizedStringByCode("ERROR_NO_EMPLOYEE_SELECTED", "No employee has been selected."); }
        }
        public static string ERROR_AVCPLAN_CEILING_NOT_NUMERIC
        {
            get { return WebUtility.GetLocalizedStringByCode("ERROR_AVCPLAN_CEILING_NOT_NUMERIC", "Ceiling amount of Payment {0} is not numeric"); }
        }

        public static string ERROR_COSTCENTER_PERECNTAGE_NOT_NUMERIC
        {
            get { return WebUtility.GetLocalizedStringByCode("ERROR_COSTCENTER_PERECNTAGE_NOT_NUMERIC", "Percentage of cost center {0} is not numeric"); }
        }

        public static string ERROR_REMINDER_DAYS_NOT_NUMERIC
        {
            get { return WebUtility.GetLocalizedStringByCode("ERROR_REMINDER_DAYS_NOT_NUMERIC", "Days for {0} is not numeric"); }
        }

        public static string ERROR_SAAS_FILE_SUBMIT_AFTER_CUTOFF
        {
            get { return WebUtility.GetLocalizedStringByCode("ERROR_SAAS_BANK_FILE_SUBMIT_AFTER_CUTOFF", "File submittion service is stopped after {0}. Please try again on next date"); }
        }
        public static string ERROR_SURNAME_CONTAIN_SPACE
        {
            get { return WebUtility.GetLocalizedStringByCode("ERROR_SURNAME_CONTAIN_SPACE", "Surname cannot contain space"); }
        }
        public static string ERROR_MISSING_PAYSCALE_POINTS
        {
            get { return WebUtility.GetLocalizedStringByCode("ERROR_MISSING_PAYSCALE_POINTS", "Missing pay scale points"); }
        }
        public static string ERROR_REMOVE_BATCH
        {
            get { return WebUtility.GetLocalizedStringByCode("ERROR_REMOVE_BATCH", "The batch is either confirmed or applied, and cannot be removed"); }
        }
		// Start 0000044, Miranda, 2014-05-09
        public static string ALERT_NEW_STAFF_NO_BEEN_USED
        {
            get { return WebUtility.GetLocalizedStringByCode("ALERT_NEW_STAFF_NO_BEEN_USED", "The staff number {0} has been used by another user, new staff number {1} has been assigned."); }
        }
		// End 0000044, Miranda, 2014-05-09

    }






    public abstract class PromptMessage
    {
        public static string CreateConfirmDialogJavascript(string message)
        {
            message = message.Replace(@"\", @"\\").Replace("\r", "\\r").Replace("\n", "\\n").Replace("\"", "\\\"");
            return "if (!confirm('" + message + "')) return false;";
        }

        public static string CreateConfirmDialogJavascript(string message, System.Web.UI.Control PostBackControl)
        {
            message = message.Replace(@"\", @"\\").Replace("\r", "\\r").Replace("\n", "\\n").Replace("\"", "\\\"");
            return "return showModalConfirmMessage('" + message + "','" + PostBackControl.UniqueID + "');";
        }

        public static string CreateDeleteConfirmDialogJavascript(System.Web.UI.Control PostBackControl)
        {
            return CreateConfirmDialogJavascript(HROne.Common.WebUtility.GetLocalizedStringByCode("DELETE_GENERIC_JAVASCRIPT", "Are you sure?"), PostBackControl);
        }

        [Obsolete("Replaced by CreateDeleteConfirmDialogJavascript(string ButtonClientID)")]
        public static string DELETE_GENERIC_JAVASCRIPT
        {
            get { return CreateConfirmDialogJavascript(HROne.Common.WebUtility.GetLocalizedStringByCode("DELETE_GENERIC_JAVASCRIPT", "Are you sure?")); }
        }
        public static string REJOIN_TERMINATED_EE_JAVASCRIPT
        {
            get { return CreateConfirmDialogJavascript(HROne.Common.WebUtility.GetLocalizedStringByCode("REJOIN_TERMINATED_EE_JAVASCRIPT", "Rejoin action will remove ALL termination detail but payroll detail for final payment will be keep.\\r\\nStill Continue?")); }
        }
        public static string PAYROLL_PROCESS_END_GENERIC_JAVASCRIPT
        {
            get { return CreateConfirmDialogJavascript(HROne.Common.WebUtility.GetLocalizedStringByCode("PAYROLL_PROCESS_END_GENERIC_JAVASCRIPT", "Are you sure to end this payroll process?")); }
        }
        public static string PAYROLL_ROLLBACK_PAYROLL_PERIOD_GENERIC_JAVASCRIPT
        {
            get { return CreateConfirmDialogJavascript(HROne.Common.WebUtility.GetLocalizedStringByCode("PAYROLL_ROLLBACK_PAYROLL_PERIOD_GENERIC_JAVASCRIPT", "Are you sure to rollback to previous payroll process?")); }
        }
        //[Obsolete("Replaced by CreateDeleteConfirmDialogJavascript(string ButtonClientID)")]
        //public static string LEAVEAPP_FORCE_DELETE_JAVASCRIPT
        //{
        //    get { return CreateConfirmDialogJavascript(HROne.Common.WebUtility.GetLocalizedStringByCode("LEAVEAPP_FORCE_DELETE_JAVASCRIPT", "The leave application is payroll processed.\nYou need to adjust the payroll record manually.\nAre you sure to delete?")); }
        //}
        //[Obsolete("Replaced by CreateDeleteConfirmDialogJavascript(string ButtonClientID)")]
        //public static string LEAVEAPP_FORCE_EDIT_JAVASCRIPT
        //{
        //    get { return CreateConfirmDialogJavascript(HROne.Common.WebUtility.GetLocalizedStringByCode("LEAVEAPP_FORCE_EDIT_JAVASCRIPT", "The leave application is payroll processed.\nYou need to adjust the payroll record manually.\nAre you sure to save?")); }
        //}
        [Obsolete("Use PageMessage class")]
        public static string IMPORT_SUCCESSFUL
        {
            get { return PageMessage.IMPORT_SUCCESSFUL; }
        }
        [Obsolete("Use PageMessage class")]
        public static string REPORT_GENERATING_TO_INBOX
        {
            get { return PageMessage.REPORT_GENERATING_TO_INBOX; }
        }
        //public static string ARE_YOU_SURE_JAVASCRIPT
        //{
        //    get { return "if (!confirm('" + HROne.Common.WebUtility.GetLocalizedStringByCode("ARE_YOU_SURE_JAVASCRIPT", "Are you sure?") + "')) return false;"; }
        //}
    }


}