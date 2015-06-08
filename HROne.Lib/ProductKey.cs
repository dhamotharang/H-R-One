using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using HROne.CommonLib;
using HROne.Lib.Entities;
using HROne.DataAccess;

namespace HROne
{
    public class ProductLicense
    {
        public enum ProductLicenseType
        {
            NONE,
            HROne,  //  do NOT change HROne since ToString() is in use
            HROneLite,
            iMGR,  
            HROneSaaS = iMGR, //  do NOT change HROneSaaS since ToString() is in use
        }
        private const Crypto.SymmProvEnum symProvEnum = Crypto.SymmProvEnum.RC2;
        private const ushort FLAG_TRIAL_KEY = 1024;
        private const byte FLAG_PAYROLL = 128;
        private const byte FLAG_TAXATION = 64;
        private const byte FLAG_LEAVE = 32;
        private const byte FLAG_ESS = 16;
        private const byte FLAG_Attendance = 8;
        private const byte FLAG_CostCenter = 4;
        private const byte FLAG_Training = 2;

        private const int DEFAULT_TRIALPERIOD = 30;
        private const int PRODUCT_TRIALPERIOD = 60;

        private const string FEATURE_CODE_MAX_EMPLOYEE = "MAX_EMPLOYEE";
        private const string FEATURE_CODE_MAX_INBOX_SIZE_MB = "MAX_INBOX_SIZE_MB";
        private const string FEATURE_CODE_AUTOPAYMPF_HAS_HSBCHASE = "AUTOPAYMPF_HAS_HSBCHASE";
        private const string FEATURE_CODE_AUTOPAYMPF_HAS_OTHERS = "AUTOPAYMPF_HAS_OTHERS";

        private bool m_IsValidProductKey = false;
        public bool IsValidProductKey
        {
            get { return m_IsValidProductKey; }
        }

        private ushort m_SerialNo;
        public ushort SerialNo
        {
            get { return m_SerialNo; }
            //set { m_SerialNo = value; }
        }

        private ProductLicenseType m_ProductType = ProductLicenseType.NONE;
        public ProductLicenseType ProductType
        {
            get { return m_ProductType; }
        }
        //private string m_ProductCode;
        //public string ProductCode
        //{
        //    get { return m_ProductCode; }
        //    //set { m_ProductCode = value; }
        //}

        private ushort m_NumOfUsers;
        public ushort NumOfUsers
        {
            get { return m_NumOfUsers; }
            //set { m_NumOfUsers = value; }
        }

        private ushort m_NumOfCompanies;
        public ushort NumOfCompanies
        {
            get { return m_NumOfCompanies; }
            //set { m_NumOfCompanies = value; }
        }

        private uint m_NumOfEmployees;
        public uint NumOfEmployees
        {
            get 
            {
                return m_NumOfEmployees; 
            }
            //set { m_NumOfUsers = value; }
        }

        //private ushort m_TrialDays;
        //public ushort TrialDays
        //{
        //    get { return m_TrialDays; }
        //    set { m_TrialDays = value; }
        //}

        //private bool m_IsTrialKey;
        //public bool IsTrialKey
        //{
        //    get { return m_IsTrialKey; }
        //    set { m_IsTrialKey = value; }
        //}

        private bool m_IsPayroll;
        public bool IsPayroll
        {
            get { return m_IsPayroll; }
            //set { m_IsPayroll = value; }
        }

        private bool m_IsTaxation;
        public bool IsTaxation
        {
            get { return m_IsTaxation; }
            //set { m_IsTaxation = value; }
        }
        private bool m_IsLeaveManagement;
        public bool IsLeaveManagement
        {
            get { return m_IsLeaveManagement; }
            //set { m_IsLeaveManagement = value; }
        }

        private bool m_IsESS;
        public bool IsESS
        {
            get { return m_IsESS; }
            //set { m_IsESS = value; }
        }

        private bool m_IsAttendance;
        public bool IsAttendance
        {
            get { return m_IsAttendance; }
            //set { m_IsAttendance = value; }
        }

        private bool m_IsCostCenter;
        public bool IsCostCenter
        {
            get { return m_IsCostCenter; }
            //set { m_IsCostCenter = value; }
        }

        private bool m_IsTraining;
        public bool IsTraining
        {
            get { return m_IsTraining; }
            //set { m_IsCostCenter = value; }
        }

        private uint m_MaxInboxSizeMB;
        public uint MaxInboxSizeMB
        {
            get
            {
                return m_MaxInboxSizeMB;
            }
            //set { m_NumOfUsers = value; }
        }

        private bool m_HasAutopayMPFFileHSBCHASE = true;
        public bool HasAutopayMPFFileHSBCHASE
        {
            get { return m_HasAutopayMPFFileHSBCHASE; }
        }

        private bool m_HasAutopayMPFFileOthers = true;
        public bool HasAutopayMPFFileOthers
        {
            get { return m_HasAutopayMPFFileOthers; }
        }
        
        //private bool m_IsAllowDocumentAllow;
        public bool IsAllowDocumentAllow
        {
            get { return ProductType==ProductLicenseType.HROne; }
        }

        private string m_productKey = string.Empty;
        public string ProductKey
        {
            get { return m_productKey; }
        }

        private string m_TrialKey = string.Empty;
        public string TrialKey
        {
            get { return m_TrialKey; }
        }

        private DateTime m_LastTrialDate;
        public DateTime LastTrialDate
        {
            get { return m_LastTrialDate; }
        }

        public bool IsTrialPeriodExpiry()
        {
            return (LastTrialDate < AppUtils.ServerDateTime().Date);
        }
        protected string m_AuthorizationCode;
        public string AuthorizationCode
        {
            get { return m_AuthorizationCode; }
        }

        public bool IsValidAuthorizationCode()
        {
            return this.IsValidAuthorizationCode(m_AuthorizationCode);
        }

        public ProductLicense()
        {
            //m_ProductCode = "HROne";

        }



        public void LoadProductLicense(DatabaseConnection dbConn)
        {
            string tmpProductKey = ESystemParameter.getParameter(dbConn, ESystemParameter.PARAM_CODE_PRODUCTKEY);
            string tmpProductFeature = ESystemParameter.getParameter(dbConn, ESystemParameter.PARAM_CODE_PRODUCTFEATURECODE);
            string tmpTrialKey = ESystemParameter.getParameter(dbConn, ESystemParameter.PARAM_CODE_TRIALKEY);
            string tmpAuthCode = ESystemParameter.getParameter(dbConn, ESystemParameter.PARAM_CODE_AUTHORIZATIONCODE);
            LoadProductKey(tmpProductKey);
            SetFeatureByCode(tmpProductFeature);
            LoadTrialKey(tmpTrialKey, dbConn);
            IsValidAuthorizationCode(tmpAuthCode);
        }

        public void SaveProductLicense(DatabaseConnection dbConn)
        {
            SetNewTrialKey(dbConn);
            ESystemParameter.setParameter(dbConn, ESystemParameter.PARAM_CODE_PRODUCTKEY, ProductKey);
            ESystemParameter.setParameter(dbConn, ESystemParameter.PARAM_CODE_TRIALKEY, TrialKey);
            ESystemParameter.setParameter(dbConn, ESystemParameter.PARAM_CODE_AUTHORIZATIONCODE, AuthorizationCode);

        }

        public bool LoadProductKey(string productkey)
        {
            m_IsValidProductKey = false;

            m_SerialNo = 0;
            m_NumOfCompanies = 1; //initial 1 company
            m_NumOfUsers = 3; //initial 3 users
            m_NumOfEmployees = 50;
            m_IsPayroll = false;
            m_IsTaxation = false;
            m_IsLeaveManagement = false;
            m_IsESS = false;
            m_IsAttendance = false;
            m_IsTraining = false;
            m_productKey = string.Empty;

            try
            {
                string preProductKey = getPreProductKey(productkey);

                if (ProductType == ProductLicenseType.HROne || ProductType == ProductLicenseType.HROneSaaS)
                    m_NumOfEmployees = uint.MaxValue;
                m_MaxInboxSizeMB = uint.MaxValue;
                string[] parts = preProductKey.Split(new char[] { '-' });
                if (parts.Length != 3)
                    throw new Exception("InvalidProductKey");
                string sSerial = parts[0];
                string sCompanyUser = parts[1];
                string sFeature = parts[2];

                m_SerialNo = Convert.ToUInt16(ConvertNumberFromBase64String(sSerial));

                ushort numCompaniesUsers = Convert.ToUInt16(ConvertNumberFromBase64String(sCompanyUser));
                m_NumOfCompanies = (ushort)(numCompaniesUsers / 100);
                m_NumOfUsers = (ushort)(numCompaniesUsers % 100);

                byte featureFlag = ((byte)ConvertNumberFromBase64String(sFeature));
                m_IsPayroll = (featureFlag & FLAG_PAYROLL) == FLAG_PAYROLL;
                m_IsTaxation = (featureFlag & FLAG_TAXATION) == FLAG_TAXATION;
                m_IsLeaveManagement = (featureFlag & FLAG_LEAVE) == FLAG_LEAVE;
                m_IsESS = (featureFlag & FLAG_ESS) == FLAG_ESS;
                m_IsAttendance = (featureFlag & FLAG_Attendance) == FLAG_Attendance;
                m_IsCostCenter = (featureFlag & FLAG_CostCenter) == FLAG_CostCenter;
                m_IsTraining = (featureFlag & FLAG_Training) == FLAG_Training;
                m_IsValidProductKey = true;
                m_productKey = productkey;

            }
            catch(Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
                return false;
            }
            return true;
        }

        public void SetFeatureByCode(string FeatureCode)
        {
            if (!string.IsNullOrEmpty(FeatureCode))
            {
                Crypto crypto = new Crypto(symProvEnum);
                string actualFeatureCode = string.Empty;
                try
                {
                    actualFeatureCode = crypto.Decrypting(FeatureCode, m_ProductType.ToString());
                }
                catch (Exception)
                {
                    throw new Exception("Invalid Product Type");
                }
                Dictionary<string, string> featureTable = new Dictionary<string, string>();
                string[] featureLineList = actualFeatureCode.Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
                foreach (string featureLine in featureLineList)
                {
                    string[] featureOptions = featureLine.Split(new string[] { "=" }, StringSplitOptions.None);
                    if (featureOptions.Length > 2)
                        throw new Exception("Invalid Feature Code: " + featureLine);
                    if (featureOptions.Length > 0)
                    {
                        string featureCode = featureOptions[0].Trim();
                        string featureValue = string.Empty;
                        if (featureOptions.Length > 1)
                            featureValue = featureOptions[1].Trim();
                        featureTable.Add(featureCode, featureValue);
                    }
                }

                if (featureTable.ContainsKey(FEATURE_CODE_MAX_INBOX_SIZE_MB))
                {
                    string value = featureTable[FEATURE_CODE_MAX_INBOX_SIZE_MB];
                    if (!string.IsNullOrEmpty(value))
                    {
                        uint intValue = 0;
                        if (uint.TryParse(value, out intValue))
                            m_MaxInboxSizeMB = intValue;
                        else
                            throw new Exception("Invalid Feature Value: " + intValue);
                    }
                }
                if (featureTable.ContainsKey(FEATURE_CODE_MAX_EMPLOYEE))
                {
                    string value = featureTable[FEATURE_CODE_MAX_EMPLOYEE];
                    if (!string.IsNullOrEmpty(value))
                    {
                        uint intValue = 0;
                        if (uint.TryParse(value, out intValue))
                            m_NumOfEmployees = intValue;
                        else
                            throw new Exception("Invalid Feature Value: " + intValue);
                    }
                }
                if (featureTable.ContainsKey(FEATURE_CODE_AUTOPAYMPF_HAS_HSBCHASE))
                {
                    string value = featureTable[FEATURE_CODE_AUTOPAYMPF_HAS_HSBCHASE];
                    if (value.Equals("Y"))
                        m_HasAutopayMPFFileHSBCHASE = true;
                    else
                        m_HasAutopayMPFFileHSBCHASE = false;
                }
                if (featureTable.ContainsKey(FEATURE_CODE_AUTOPAYMPF_HAS_OTHERS))
                {
                    string value = featureTable[FEATURE_CODE_AUTOPAYMPF_HAS_OTHERS];
                    if (value.Equals("Y"))
                        m_HasAutopayMPFFileOthers = true;
                    else
                        m_HasAutopayMPFFileOthers = false;
                }
            }
        }

        private string getPreProductKey(string productkey)
        {
            Crypto crypto = new Crypto(symProvEnum);
            foreach (string productTypeName in Enum.GetNames(typeof(ProductLicenseType)))
            {
                try
                {
                    string preProductKey = crypto.Decrypting(base32.ConvertBase32ToBase64(productkey), productTypeName);
                    m_ProductType = (ProductLicenseType)Enum.Parse(typeof(ProductLicenseType), productTypeName);
                    return preProductKey;
                }
                catch
                {
                }
            }
            m_ProductType = ProductLicenseType.NONE;
            return string.Empty;
        }
        public void LoadTrialKey(string trialKey, DatabaseConnection dbConn)
        {
            m_TrialKey = string.Empty;
            m_LastTrialDate = getLastTrialDate(trialKey);
            if (m_LastTrialDate.Ticks.Equals(0))
                m_LastTrialDate = getLastTrialDate(dbConn);
            else
                m_TrialKey = trialKey;
        }

        private DateTime getLastTrialDate(string trialKey)
        {
            Crypto crypto = new Crypto(Crypto.SymmProvEnum.DES);
            try
            {
                trialKey = base32.ConvertBase32ToBase64(trialKey);

                string realTrialKey = crypto.Decrypting(trialKey, m_ProductType.ToString());
                string strYear = realTrialKey.Substring(0, 4);
                string strMonth = realTrialKey.Substring(4, 2);
                string strDay = realTrialKey.Substring(6, 2);

                return new DateTime(int.Parse(strYear), int.Parse(strMonth), int.Parse(strDay));

            }
            catch
            {
                return new DateTime();
            }
        }
        private DateTime getLastTrialDate(DatabaseConnection dbConn)
        {
            DateTime firstActionDateTime = AppUtils.ServerDateTime();
            if (dbConn != null)
            {
                DBManager db = EAuditTrail.db;
                DBFilter dbFilter = new DBFilter();
                System.Data.DataTable table = dbFilter.loadData(dbConn, "Select min(CreateDate) from " + db.dbclass.tableName);
                if (table.Rows[0][0] != DBNull.Value)
                    firstActionDateTime = ((DateTime)table.Rows[0][0]);
            }
            if (IsValidProductKey)
                return firstActionDateTime.Date.AddDays(PRODUCT_TRIALPERIOD);
            else
                return firstActionDateTime.Date.AddDays(DEFAULT_TRIALPERIOD);
        }


        public void SetNewTrialKey(DatabaseConnection dbConn)
        {
            ProductLicense originalProductKey = new ProductLicense();
            originalProductKey.LoadProductLicense(dbConn);
            if (ProductKey != originalProductKey.ProductKey && (originalProductKey.IsValidProductKey || string.IsNullOrEmpty(originalProductKey.ProductKey)))
            {
                Crypto crypto = new Crypto(Crypto.SymmProvEnum.DES);
                string trialKey = string.Empty;
                if (IsValidProductKey)
                    trialKey = crypto.Encrypting(AppUtils.ServerDateTime().Date.AddDays(PRODUCT_TRIALPERIOD).ToString("yyyyMMdd"), m_ProductType.ToString());
                else
                    trialKey = crypto.Encrypting(AppUtils.ServerDateTime().Date.AddDays(DEFAULT_TRIALPERIOD).ToString("yyyyMMdd"), m_ProductType.ToString());

                trialKey = base32.ConvertBase64ToBase32(trialKey);
                LoadTrialKey(trialKey, dbConn);
            }
        }


        public string getRequestCode()
        {
            if (IsValidProductKey)
            {
                string[] requestCodeList = getRequestCodeList();
                return requestCodeList[0];
                //Crypto crypto = new Crypto(Crypto.SymmProvEnum.DES);

                //string tmpRequestCode = base32.ConvertBase64ToBase32(crypto.Encrypting(base32.ConvertBase32ToBase64(ProductKey), getFirstMacAddress()));
                //return tmpRequestCode.Substring(0,10);
            }
            else
                return string.Empty;
        }
        public string[] getRequestCodeList()
        {
            if (IsValidProductKey)
            {
                string[] requestCodeList = getMacAddressList();

                Crypto crypto = new Crypto(Crypto.SymmProvEnum.DES);
                for (int i = requestCodeList.GetLowerBound(0); i <= requestCodeList.GetUpperBound(0); i++)
                {

                    string tmpRequestCode = base32.ConvertBase64ToBase32(crypto.Encrypting(base32.ConvertBase32ToBase64(ProductKey), requestCodeList[i]));
                    requestCodeList[i] = tmpRequestCode.Substring(0, 10);

                }
                return requestCodeList;
            }
            else
                return null;
        }

        public bool IsValidAuthorizationCode(string authorizationKey)
        {
            if (m_ProductType == ProductLicenseType.HROneSaaS)
                return true;
            this.m_AuthorizationCode = string.Empty;
            if (!string.IsNullOrEmpty(authorizationKey))
            {
                Crypto crypto = new Crypto(Crypto.SymmProvEnum.DES);

                string[] requestCodeList = getRequestCodeList();

                foreach (string key in requestCodeList)
                {
                    string base64AuthorizationKey = base32.ConvertBase32ToBase64(authorizationKey);
                    if (crypto.Encrypting(ProductType.ToString(), key).Equals(base64AuthorizationKey))
                    {
                        this.m_AuthorizationCode = authorizationKey;
                        return true;
                    }
                }
                return false;
            }
            else
                return false;
        }

        //private static string getFirstMacAddress()
        //{
        //    System.Net.NetworkInformation.NetworkInterface[] interfaceList = System.Net.NetworkInformation.NetworkInterface.GetAllNetworkInterfaces();
        //    if (interfaceList.Length > 0)
        //    {
        //        System.Net.NetworkInformation.NetworkInterface netInterface = interfaceList[0];
        //        return Convert.ToBase64String(netInterface.GetPhysicalAddress().GetAddressBytes());
        //    }
        //    return string.Empty;
        //}
        private static string[] getMacAddressList()
        {
            ArrayList list = new ArrayList();
            System.Net.NetworkInformation.NetworkInterface[] interfaceList = System.Net.NetworkInformation.NetworkInterface.GetAllNetworkInterfaces();
            foreach (System.Net.NetworkInformation.NetworkInterface netInterface  in interfaceList)
            {
                list.Add(Convert.ToBase64String(netInterface.GetPhysicalAddress().GetAddressBytes()));
            }
            return (string[])list.ToArray(typeof(string));
        }

        static string ConvertNumberToBase64String(ushort value)
        {
            char[] resultArray = Convert.ToBase64String(BitConverter.GetBytes(value)).ToCharArray();
            int lastCharPos;
            for (lastCharPos = resultArray.Length - 1; lastCharPos >= 0; lastCharPos--)
                if (resultArray[lastCharPos] != '=')
                    break;
            return new string(resultArray, 0, lastCharPos + 1);
        }
        static string ConvertNumberToBase64String(byte value)
        {
            char[] resultArray = Convert.ToBase64String(new byte[] { value }).ToCharArray();
            int lastCharPos;
            for (lastCharPos = resultArray.Length - 1; lastCharPos >= 0; lastCharPos--)
                if (resultArray[lastCharPos] != '=')
                    break;
            return new string(resultArray,0,lastCharPos+1);
        }
        static object ConvertNumberFromBase64String(string source)
        {
            while (source.Length % 4 != 0)
                source += '=';
            byte[] byteArray = Convert.FromBase64String(source);
            switch (byteArray.Length)
            {
                case 2:
                    return BitConverter.ToUInt16(byteArray, 0);
                case 1:
                    return byteArray[0];
                default:
                    return BitConverter.ToInt64(byteArray, 0);
            }
        }
        //string ConvertNumberToBase64String(long value)
        //{
        //    string result = Convert.ToBase64String(BitConverter.GetBytes(value));
        //    return result;//.Replace("", "");
        //}
    }
}
