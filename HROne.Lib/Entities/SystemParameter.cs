using System;
using System.Collections.Generic;
using System.Text;
using HROne.DataAccess;
////using perspectivemind.validation;
using HROne.CommonLib;

namespace HROne.Lib.Entities
{
    [DBClass("SystemParameter")]
    public class ESystemParameter : BaseObject
    {
        public const string PARAM_CODE_FORCE_ENCRYPT_ON_STARTUP = "FORCE_ENCRYPT_ON_STARTUP";
        public const string PARAM_CODE_FORCE_DECRYPT_ON_STARTUP = "FORCE_DECRYPT_ON_STARTUP";

        //public const string PARAM_CODE_LEAVE_ENTITLE_CUT_OFF_MONTH = "LEAVE_ENTITLE_MONTH";
        //public const string PARAM_CODE_LEAVE_ENTITLE_CUT_OFF_DAY = "LEAVE_ENTITLE_DAY";
        //public const string PARAM_CODE_LEAVE_ENTITLE_CUT_OFF_BY_SERVICE_DATE = "LEAVE_ENTITLE_BY_SERVICE_DATE";
        public const string PARAM_CODE_TAXATION_USE_CHINESE_NAME = "TAXATION_USE_CHINESE_NAME";
        public const string PARAM_CODE_PRODUCTKEY = "PRODUCTKEY";
        public const string PARAM_CODE_PRODUCTFEATURECODE = "PRODUCTFEATURECODE";
        public const string PARAM_CODE_TRIALKEY = "TRIALKEY";
        public const string PARAM_CODE_AUTHORIZATIONCODE = "AUTHORIZATIONCODE";
        public const string PARAM_CODE_USE_ORSO = "USE_ORSO";
        public const string PARAM_CODE_PAYROLL_MAX_MONTHLY_LSPSP_AMOUNT = "MAX_MONTHLY_LSPSP_AMOUNT";
        public const string PARAM_CODE_PAYROLL_MAX_TOTAL_LSPSP_AMOUNT = "MAX_TOTAL_LSPSP_AMOUNT";
        public const string PARAM_CODE_PAY_SLIP_HIDE_LEAVE_BALANCE = "PAY_SLIP_HIDE_LEAVE_BALANCE";
        public const string PARAM_CODE_PAY_SLIP_HIDE_MINIMUM_WAGE_INFO = "PAY_SLIP_HIDE_MINIMUM_WAGE_INFO";
        public const string PARAM_CODE_PAY_SLIP_HIERARCHY_DISPLAY_SEQUENCE = "PAY_SLIP_HIERARCHY_DISPLAY_SEQUENCE";
        public const string PARAM_CODE_PAYROLL_SUMMARY_HIERARCHY_DISPLAY_SEQUENCE = "PAYROLL_SUMMARY_HIERARCHY_DISPLAY_SEQUENCE";
        public const string PARAM_CODE_LOGIN_MAX_FAIL_COUNT = "LOGIN_MAX_FAIL_COUNT";
        public const string PARAM_CODE_SESSION_TIMEOUT = "SESSION_TIMEOUT";
        public const string PARAM_CODE_DEFAULT_LANGUAGE = "DEFAULT_LANGUAGE";
        public const string PARAM_CODE_ESS_DEFAULT_LANGUAGE = "ESS_DEFAULT_LANGUAGE";
        public const string PARAM_CODE_DEFAULT_RECORDS_PER_PAGE = "DEFAULT_RECORDS_PER_PAGE";
        public const string PARAM_CODE_HKID_CHECKDIGIT_AUTO_GENERATE = "HKID_CHECKDIGIT_AUTO_GENERATE";
        public const string PARAM_CODE_REPORT_CHINESE_FONT = "REPORT_CHINESE_FONT";
        //  SMTP Server Parameter
        public const string PARAM_CODE_SMTP_SERVER_NAME = "SMTP_SERVER_NAME";
        public const string PARAM_CODE_SMTP_PORT = "SMTP_PORT";
        public const string PARAM_CODE_SMTP_ENABLE_SSL = "SMTP_ENABLE_SSL";
        public const string PARAM_CODE_SMTP_USERNAME = "SMTP_USERNAME";
        public const string PARAM_CODE_SMTP_PASSWORD = "SMTP_PASSWORD";
        public const string PARAM_CODE_SMTP_SMTP_OUTGOING_EMAIL_ADDRESS = "SMTP_OUTGOING_EMAIL_ADDRESS";

        public const string PARAM_CODE_PAYSCALE_POINT_SYSTEM = "PASCALE_POINT_SYSTEM";
        public const string PARAM_CODE_PAYSCALE_SALARY_INCREMENT_METHOD = "PAYSCALE_SALARY_INCREMENT_METHOD";

        public const string PARAM_CODE_MONTHLY_ACHIEVEMENT_COMMISSION = "MONTHLY_ACHIEVEMENT_COMMISSION";       // for F&V to turn on
        public const string PARAM_CODE_INCENTIVE_PAYMENT = "INCENTIVE_PAYMENT";       // for F&V to turn on
        public const string PARAM_CODE_ENABLE_DOUBLE_PAY_ADJUSTMENT = "ENABLE_DOUBLE_PAY_ADJUSTMENT";       // for F&V to turn on
        public const string PARAM_CODE_ENABLE_BONUS_PROCESS = "ENABLE_BONUS_PROCESS";

        // Start 000159, Ricky So, 2015-01-23
        //public const string PARAM_CODE_COMMISSION_BASE_FACTOR = "COMMISSION_BASE_FACTOR";    // for Ingram to turn on
        // End 000159, Ricky So, 2015-01-23

        public const string PARAM_CODE_ESS_FUNCTION_CHANGE_EE_INFO = "ESS_FUNCTION_CHANGE_EE_INFO";
        public const string PARAM_CODE_ESS_FUNCTION_LEAVE_APPLICATION = "ESS_FUNCTION_LEAVE_APPLICATION";
        public const string PARAM_CODE_ESS_FUNCTION_CANCEL_LEAVE_APPLICATION = "ESS_FUNCTION_CANCEL_LEAVE_APPLICATION";
        public const string PARAM_CODE_ESS_FUNCTION_LEAVE_BALANCE_ENQUIRY = "ESS_FUNCTION_LEAVE_BALANCE_ENQUIRY";
        public const string PARAM_CODE_ESS_FUNCTION_LEAVE_BALANCE_REPORT = "ESS_FUNCTION_LEAVE_BALANCE_REPORT";
        public const string PARAM_CODE_ESS_FUNCTION_LEAVE_APPLICATION_LIST = "ESS_FUNCTION_LEAVE_APPLICATION_LIST";
        public const string PARAM_CODE_ESS_FUNCTION_ROSTER_TABLE = "ESS_FUNCTION_ROSTER_TABLE";
        public const string PARAM_CODE_ESS_FUNCTION_PRINT_PAYSLIP = "ESS_FUNCTION_PRINT_PAYSLIP";
        public const string PARAM_CODE_ESS_FUNCTION_PRINT_TAXREPORT = "ESS_FUNCTION_PRINT_TAXREPORT";
        public const string PARAM_CODE_ESS_FUNCTION_OVERALL_PAYMENT_SUMMARY = "ESS_FUNCTION_OVERALL_PAYMENT_SUMMARY";        
        public const string PARAM_CODE_ESS_FUNCTION_LEAVE_HISTORY = "ESS_FUNCTION_LEAVE_HISTORY";
        // Start 0000060, Miranda, 2014-07-22
        public const string PARAM_CODE_ESS_FUNCTION_OT_CLAIMS = "ESS_FUNCTION_OT_CLAIMS";
        public const string PARAM_CODE_ESS_FUNCTION_OT_CLAIMS_HISTORY = "ESS_FUNCTION_OT_CLAIMS_HISTORY";
        // End 0000060, Miranda, 2014-07-22
        // Start 0000112, Miranda, 2014-12-10
        public const string PARAM_CODE_ESS_FUNCTION_LATE_WAIVE = "ESS_FUNCTION_LATE_WAIVE";
        public const string PARAM_CODE_ESS_FUNCTION_LATE_WAIVE_HISTORY = "ESS_FUNCTION_LATE_WAIVE_HISTORY";
        // End 0000112, Miranda, 2014-12-10
        public const string PARAM_CODE_MPF_FILE_BOCI_ENCRYPT_PATH = "MPF_FILE_BOCI_ENCRYPT_PATH";
        public const string PARAM_CODE_ESS_LEAVE_MEDICIAL_CERT_ALERT = "ESS_LEAVE_MEDICIAL_CERT_ALERT";
        public const string PARAM_CODE_ESS_PAYSLIP_START_DATE = "ESS_PAYSLIP_START_DATE";
        public const string PARAM_CODE_ESS_PAYSLIP_AUTO_RELEASE = "ESS_PAYSLIP_AUTO_RELEASE";
        public const string PARAM_CODE_ESS_LEAVE_HISTORY_START_DATE = "ESS_LEAVE_HISTORY_START_DATE";
        // Start 0000057, KuangWei, 2014-07-08        
        public const string PARAM_CODE_ESS_FUNCTION_MONTHLY_ATTENDANCE_REPORT = "ESS_FUNCTION_MONTHLY_ATTENDANCE_REPORT";
        public const string PARAM_CODE_ESS_FUNCTION_ATTENDANCE_TIMEENTRY_LIST = "ESS_FUNCTION_ATTENDANCE_TIMEENTRY_LIST";
        public const string PARAM_CODE_ESS_FUNCTION_TIMECARD_RECORD = "ESS_FUNCTION_TIMECARD_RECORD";
        // End 0000057, KuangWei, 2014-07-08
        // Start 0000076, Miranda, 2014-08-21
        public const string PARAM_CODE_ESS_FUNCTION_ATTENDANCE_TIMEENTRY_REPORT = "ESS_FUNCTION_ATTENDANCE_TIMEENTRY_REPORT";
        // End 0000076, Miranda, 2014-08-21
        public const string PARAM_CODE_DOCUMENT_UPLOAD_FOLDER = "DOCUMENT_UPLOAD_FOLDER";
        // Start 0000060, Miranda, 2014-07-22
        // Start 0000060, Miranda, 2014-07-15
        //public const string PARAM_CODE_ESS_DEF_EOT_EXPIRY_DATE = "ESS_DEF_EOT_EXPIRY_DATE";
        public const string PARAM_CODE_ENABLE_OTCLAIM = "ENABLE_OTCLAIM";

        // Start 0000112, Ricky So, 2014/12/18
        public const string PARAM_CODE_ENABLE_LATE_WAIVE = "ENABLE_LATE_WAIVE";
        // End 0000112, Ricky So, 2014/12/18
        
        public const string PARAM_CODE_ENABLE_TIMECARD_RECORD = "ENABLE_TIMECARD_RECORD";
        public const string PARAM_CODE_ENABLE_ATTENDANCE_TIMEENTRY_LIST = "ENABLE_ATTENDANCE_TIMEENTRY_LIST";
        public const string PARAM_CODE_ENABLE_MONTHLY_ATTENDANCE_REPORT = "ENABLE_MONTHLY_ATTENDANCE_REPORT";
        // End 0000060, Miranda, 2014-07-15
        public const string PARAM_CODE_ESS_DEF_EOT_EXPIRY = "ESS_DEF_EOT_EXPIRY";
        public const string PARAM_CODE_ESS_DEF_EOT_EXPIRY_TYPE = "ESS_DEF_EOT_EXPIRY_TYPE";
        // End 0000060, Miranda, 2014-07-22

        public const string PARAM_CODE_WF_SEND_ACCEPT_EMAIL = "WF_SEND_ACCEPT_EMAIL";

        public const string PARAM_CODE_EMAIL_AUDIT_TRAIL_ADDRESS = "EMAIL_AUDIT_TRAIL_ADDRESS";

        public const string PARAM_CODE_ECHANNEL_SIGNATURE_REQUIRED_FOR_AUTOPAY_FILE = "ECHANNEL_SIGNATURE_REQUIRED_FOR_AUTOPAY_FILE";
        public const string PARAM_CODE_ECHANNEL_SIGNATURE_REQUIRED_FOR_MPF_FILE = "ECHANNEL_SIGNATURE_REQUIRED_FOR_MPF_FILE";
        public const string PARAM_CODE_ECHANNEL_NUM_SIGNATURE_REQUIRED = "ECHANNEL_NUM_SIGNATURE_REQUIRED";

        public const string PARAM_CODE_EMP_NO_AUTO_GENERATE = "AUTO_GENERATE_EMPNO";
        public const string PARAM_CODE_EMP_NO_FORMAT = "AUTO_GENERATE_EMPNO_FORMAT";

        public const string PARAM_CODE_DB_TITLE = "DB_TITLE";


        public const string PARAM_CODE_EMP_LIST_SHOW_COMPANY = "EMP_LIST_SHOW_COMPANY";
        public const string PARAM_CODE_EMP_LIST_SHOW_H1 = "EMP_LIST_SHOW_H1";
        public const string PARAM_CODE_EMP_LIST_SHOW_H2 = "EMP_LIST_SHOW_H2";
        public const string PARAM_CODE_EMP_LIST_SHOW_H3 = "EMP_LIST_SHOW_H3";

        public const string PARAM_CODE_ENABLE_PAYROLL_GROUP_SECURITY = "ENABLE_PAYROLL_GROUP_SECURITY";


        private const string KEY = "HROne";
        private const Crypto.SymmProvEnum ENCRYPT_METHOD = Crypto.SymmProvEnum.Rijndael;
        public static string getParameter(DatabaseConnection dbConn, string Name)
        {
            ESystemParameter param = new ESystemParameter();
            param.ParameterCode = Name;
            if (ESystemParameter.db.select(dbConn, param))
                return param.ParameterValue;
            else
                return string.Empty;
        }

        public static bool IsEnabled(DatabaseConnection dbConn, string parameterName)
        {
            return ESystemParameter.getParameter(dbConn, parameterName).Equals("Y", StringComparison.CurrentCultureIgnoreCase);
        }

        public static string getParameterWithEncryption(DatabaseConnection dbConn, string Name)
        {
            string value = getParameter(dbConn, Name);
            if (value is string)
            {

                //  Check string format to minimize the time consuming caused by generating exception message
                if (string.IsNullOrEmpty((string)value))
                    return value;
                if (((string)value).Length % 4 != 0)
                    return value;

                System.Text.RegularExpressions.Regex regExBase64 = new System.Text.RegularExpressions.Regex(@"[0-9a-zA-Z\+/=]{20,}");
                if (!regExBase64.IsMatch((string)value))
                    return value;

                //if (((string)value).Contains(" "))
                //    return value;
                //if (((string)value).Contains("(") || ((string)value).Contains(")"))
                //    return value;
                byte[] bytIn;
                try
                {
                    bytIn = Convert.FromBase64String((string)value);
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                    return value;
                }
                if (bytIn.Length % 16 != 0)
                {
                    return value;
                }
                Crypto crypto = new Crypto(ENCRYPT_METHOD);
                try
                {
                    value = crypto.Decrypting(value, KEY);
                }
                catch (System.Security.Cryptography.CryptographicException ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                    return value;
                    //throw ex;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            return value;

        }

        public static void setParameter(DatabaseConnection dbConn, string Name, string Value)
        {
            ESystemParameter param = new ESystemParameter();
            param.ParameterCode = Name;
            if (ESystemParameter.db.select(dbConn, param))
            {
                param.ParameterValue = Value;
                ESystemParameter.db.update(dbConn, param);
            }
            else
            {
                param.ParameterCode = Name;
                param.ParameterValue = Value;
                ESystemParameter.db.insert(dbConn, param);
            }
        }


        public static void setParameterWithEncryption(DatabaseConnection dbConn, string Name, string Value)
        {
            Crypto crypto = new Crypto(ENCRYPT_METHOD);
            setParameter(dbConn, Name, crypto.Encrypting(Value, KEY));
        }


        public static DBManager db = new DBManager(typeof(ESystemParameter));
        protected string m_ParameterCode;
        [DBField("ParameterCode", true, false), TextSearch, Export(false)]
        public string ParameterCode
        {
            get { return m_ParameterCode; }
            set { m_ParameterCode = value; modify("ParameterCode"); }
        }
        protected string m_ParameterDesc;
        [DBField("ParameterDesc"), TextSearch, Export(false)]
        public string ParameterDesc
        {
            get { return m_ParameterDesc; }
            set { m_ParameterDesc = value; modify("ParameterDesc"); }
        }
        protected string m_ParameterValue;
        [DBField("ParameterValue"), TextSearch, Export(false)]
        public string ParameterValue
        {
            get { return m_ParameterValue; }
            set { m_ParameterValue = value; modify("ParameterValue"); }
        }
    }
}
