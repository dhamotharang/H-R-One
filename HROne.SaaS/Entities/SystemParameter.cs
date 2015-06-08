using System;
using System.Collections.Generic;
using System.Text;
using HROne.DataAccess;
////using perspectivemind.validation;
using HROne.CommonLib;

namespace HROne.SaaS.Entities
{
    [DBClass("SystemParameter")]
    public class ESystemParameter : DBObject
    {
        public const string PARAM_CODE_DEFAULT_DOCUMENT_FOLDER = "DEFAULT_DOCUMENT_FOLDER";
        public const string PARAM_CODE_BANKFILE_UPLOAD_FOLDER = "BANKFILE_UPLOAD_FOLDER";
        public const string PARAM_CODE_BANKFILE_LAST_CANCEL_TIME = "BANKFILE_LAST_CANCEL_TIME";
        public const string PARAM_CODE_BANKFILE_CUTOFF_TIME = "BANKFILE_CUTOFF_TIME";

        public const string PARAM_CODE_LOGIN_MAX_FAIL_COUNT = "LOGIN_MAX_FAIL_COUNT";
        public const string PARAM_CODE_SESSION_TIMEOUT = "SESSION_TIMEOUT";
        public const string PARAM_CODE_DEFAULT_RECORDS_PER_PAGE = "DEFAULT_RECORDS_PER_PAGE";
        public const string PARAM_CODE_DEFAULT_MAX_INBOX_SIZE_MB = "DEFAULT_MAX_INBOX_SIZE_MB";

        //  SMTP Server Parameter
        public const string PARAM_CODE_SMTP_SERVER_NAME = "SMTP_SERVER_NAME";
        public const string PARAM_CODE_SMTP_PORT = "SMTP_PORT";
        public const string PARAM_CODE_SMTP_ENABLE_SSL = "SMTP_ENABLE_SSL";
        public const string PARAM_CODE_SMTP_USERNAME = "SMTP_USERNAME";
        public const string PARAM_CODE_SMTP_PASSWORD = "SMTP_PASSWORD";
        public const string PARAM_CODE_SMTP_SMTP_OUTGOING_EMAIL_ADDRESS = "SMTP_OUTGOING_EMAIL_ADDRESS";

        public const string PARAM_CODE_BANKKEY_HSBC_PATH = "BANKKEY_HSBC_PATH";
        public const string PARAM_CODE_BANKKEY_HSBC_PASSWORD = "BANKKEY_HSBC_PASSWORD";
        public const string PARAM_CODE_BANKKEY_HASE_PATH = "BANKKEY_HASE_PATH";
        public const string PARAM_CODE_BANKKEY_HASE_PASSWORD = "BANKKEY_HASE_PASSWORD";
        public const string PARAM_CODE_HSBC_MRI_DIRECTORY = "HSBC_MRI_DIRECTORY";

      

        private const string KEY = "HROneSaaS";
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
