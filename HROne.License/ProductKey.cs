using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using HROne.CommonLib;

namespace HROne.License
{
    public class ProductKey
    {
        public enum ProductLicenseType
        {
            NONE,
            HROne,
            HRExpressLite
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

        private ushort m_SerialNo;
        public ushort SerialNo
        {
            get { return m_SerialNo; }
            set { m_SerialNo = value; }
        }

        private ProductLicenseType m_ProductType = ProductLicenseType.NONE;
        public ProductLicenseType ProductType
        {
            get { return m_ProductType; }
            set { m_ProductType = value; }
        }
        //private string m_ProductCode;
        //public string ProductCode
        //{
        //    get { return m_ProductCode; }
        //    set { m_ProductCode = value; }
        //}

        private ushort m_NumOfUsers;
        public ushort NumOfUsers
        {
            get { return m_NumOfUsers; }
            set { m_NumOfUsers = value; }
        }

        private ushort m_NumOfCompanies;
        public ushort NumOfCompanies
        {
            get { return m_NumOfCompanies; }
            set { m_NumOfCompanies = value; }
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

        private ushort m_TrialDays;
        public ushort TrialDays
        {
            get { return m_TrialDays; }
            set { m_TrialDays = value; }
        }

        private bool m_IsTrialKey;
        public bool IsTrialKey
        {
            get { return m_IsTrialKey; }
            set { m_IsTrialKey = value; }
        }

        private bool m_IsPayroll;
        public bool IsPayroll
        {
            get { return m_IsPayroll; }
            set { m_IsPayroll = value; }
        }

        private bool m_IsTaxation;
        public bool IsTaxation
        {
            get { return m_IsTaxation; }
            set { m_IsTaxation = value; }
        }
        private bool m_IsLeaveManagement;
        public bool IsLeaveManagement
        {
            get { return m_IsLeaveManagement; }
            set { m_IsLeaveManagement = value; }
        }

        private bool m_IsESS;
        public bool IsESS
        {
            get { return m_IsESS; }
            set { m_IsESS = value; }
        }

        private bool m_IsAttendance;
        public bool IsAttendance
        {
            get { return m_IsAttendance; }
            set { m_IsAttendance = value; }
        }

        private bool m_IsCostCenter;
        public bool IsCostCenter
        {
            get { return m_IsCostCenter; }
            set { m_IsCostCenter = value; }
        }

        private bool m_IsTraining;
        public bool IsTraining
        {
            get { return m_IsTraining; }
            set { m_IsTraining = value; }
        }

        public string GetProductKey()
        {
            string sSerialNo = ConvertNumberToBase64String(m_SerialNo);
            string sProductCode = ProductType.ToString();
            string sCompanyUser = ConvertNumberToBase64String((ushort)(m_NumOfCompanies * 100 + m_NumOfUsers));
            //string sTrialPeriod = ConvertNumberToBase64String
            //    ((ushort)((m_IsTrialKey ? FLAG_TRIAL_KEY : (ushort)0) + m_TrialDays));
            string sFeature = ConvertNumberToBase64String
                ((byte)((m_IsPayroll ? FLAG_PAYROLL : (byte)0)
                | (m_IsTaxation ? FLAG_TAXATION : (byte)0)
                | (m_IsLeaveManagement ? FLAG_LEAVE : (byte)0)
                | (m_IsESS ? FLAG_ESS : (byte)0)
                | (m_IsAttendance ? FLAG_Attendance : (byte)0)
                | (m_IsCostCenter ? FLAG_CostCenter : (byte)0)
                | (m_IsTraining ? FLAG_Training : (byte)0)
                ));

            string preProductKey = sSerialNo + "-" + sCompanyUser + "-" + sFeature;

            Crypto crypto = new Crypto(symProvEnum);
            string productKey = crypto.Encrypting(preProductKey, sProductCode);
            string decryptProductKey = crypto.Decrypting(productKey, sProductCode);

            string base32Key = base32.ConvertBase64ToBase32(productKey);
            return base32Key;

            //string sSerialNo = base32.encodeb32(m_SerialNo, 4);
            //string sProductCode = base32.ReverseEncodeString(m_ProductCode);
            //string sCompanyUser = base32.encodeb32(m_NumOfCompanies * 100 + m_NumOfUsers, 3);
            //string sTrialPeriod = base32.encodeb32
            //    (m_IsTrialKey ? FLAG_TRIAL_KEY : 0 + m_TrialDays, 2);
            //string sFeature = base32.encodeb32
            //    ((m_IsPayroll ? FLAG_PAYROLL : 0)
            //    | (m_IsTaxation ? FLAG_TAXATION : 0)
            //    | (m_IsLeaveManagement ? FLAG_LEAVE : 0)
            //    | (m_IsESS ? FLAG_ESS : 0)
            //    | (m_IsAttendance ? FLAG_Attendance : 0), 2);

            //string preProductKey = sSerialNo + sProductCode + sCompanyUser + sTrialPeriod + sFeature;
            //return base32.CheckSum(preProductKey) + preProductKey;
        }

        public void LoadProductKey(string productkey)
        {
            Crypto crypto = new Crypto(symProvEnum);
            string preProductKey = getPreProductKey(productkey);


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


        }

        private string getPreProductKey(string productkey)
        {
            Crypto crypto = new Crypto(symProvEnum);
            foreach (ProductLicenseType value in Enum.GetValues(typeof(ProductLicenseType)))
            {
                try
                {
                    string preProductKey = crypto.Decrypting(base32.ConvertBase32ToBase64(productkey), value.ToString());
                    m_ProductType = value;
                    return preProductKey;
                }
                catch
                {
                }
            }
            return string.Empty;
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
            return new string(resultArray, 0, lastCharPos + 1);
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
