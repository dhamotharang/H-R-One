using System;
using System.Collections;
using System.Text;
using HROne.CommonLib;

namespace HROne.DataAccess
{
    public class DBAESEncryptStringFieldAttribute : Attribute, DBFieldTranscoder
    {
        //const string DEFAULT_KEY_STRING = "HROneHROne";
        protected static string m_DEFAULT_KEY_STRING = "HROneHROne";
        protected static string m_RSAEncryptedKeyString = string.Empty;

        public static string DEFAULT_KEY_STRING
        {
            set
            {
                m_DEFAULT_KEY_STRING = value;
            }
            get
            {
                return m_DEFAULT_KEY_STRING;
            }
        }

        public static string RSAEncryptedKeyString
        {
            set
            {
                m_RSAEncryptedKeyString = value;
                m_InvalidRSAKey = false;
            }
        }
        protected static bool m_InvalidRSAKey = false;
        public static bool InvalidRSAKey
        {
            get { return m_InvalidRSAKey; }
        }
        string keyString = string.Empty;
        //bool migrateFromSimpleDBEncrypt = false;
        public static bool skipEncryptionToDB = false;
        protected bool skipFieldEncryption = false;

        public DBAESEncryptStringFieldAttribute(bool skipFieldEncryption)
        {
            this.skipFieldEncryption = skipFieldEncryption;
        }

        public DBAESEncryptStringFieldAttribute()
        {
        }

        //public DBAESEncryptStringFieldAttribute(bool migrateFromSimpleDBEncrypt)
        //{
        //    this.migrateFromSimpleDBEncrypt = migrateFromSimpleDBEncrypt;
        //}

        public DBAESEncryptStringFieldAttribute(string keyString)
        {
            this.keyString = keyString;
        }

        private string getKeyString(bool isUseRSAEncryptKey)
        {
            if (!string.IsNullOrEmpty(keyString))
                return keyString;
            else
            {
                // comment out due to imgr inconsistent encryption alogrithm
                // if (isUseRSAEncryptKey && !string.IsNullOrEmpty(m_RSAEncryptedKeyString))
                // {
                //     RSACrypto rsa = new RSACrypto("HROneSaaS");
                //     try
                //     {
                //         string tmpKey = rsa.Decrypting(m_RSAEncryptedKeyString);
                //         keyString = tmpKey;
                //         return tmpKey;
                //     }
                //     catch (Exception)
                //     {
                //         m_InvalidRSAKey = true;
                //         return string.Empty;
                //     }
                // }
                // else
                    return DEFAULT_KEY_STRING;
            }

        }
        //public DBAESEncryptStringFieldAttribute(string keyString, bool migrateFromSimpleDBEncrypt)
        //{
        //    this.keyString = keyString;
        //    this.migrateFromSimpleDBEncrypt = migrateFromSimpleDBEncrypt;

        //}

        //public static void decode(System.Data.DataTable table, string name, bool migrateFromSimpleDBEncrypt)
        //{
        //    foreach (System.Data.DataRow row in table.Rows)
        //        row[name] = decode(Convert.ToString(row[name]), migrateFromSimpleDBEncrypt);

        //}

        public static void decode(System.Data.DataTable table, string name)
        {
            foreach (System.Data.DataRow row in table.Rows)
                if (!row.IsNull(name))
                    row[name] = decode(row[name]);

        }

        //public static object decode(object value, bool migrateFromSimpleDBEncrypt)
        //{
        //    DBAESEncryptStringFieldAttribute aesEncrypt = new DBAESEncryptStringFieldAttribute(migrateFromSimpleDBEncrypt);
        //    try
        //    {
        //        value = aesEncrypt.fromDB(Convert.ToString(value));
        //    }
        //    catch (Exception e)
        //    {
        //        if (migrateFromSimpleDBEncrypt)
        //        {
        //            return DBSimpleEncryptAttribute.decode(value);
        //        }
        //        else
        //            return value;
        //    }
        //    return value;
        //}

        public static object decode(object value)
        {
            DBAESEncryptStringFieldAttribute aesEncrypt = new DBAESEncryptStringFieldAttribute();
            try
            {
                value = aesEncrypt.fromDB(value);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return value;
        }

        public object fromDB(object value)
        {
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
                Crypto crypto = new Crypto(Crypto.SymmProvEnum.Rijndael);
                try
                {
                    value = crypto.Decrypting(Convert.ToString(value), getKeyString(true));
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
        public object toDB(object value)
        {
            if (!skipEncryptionToDB && !skipFieldEncryption)
            {
                Crypto crypto = new Crypto(Crypto.SymmProvEnum.Rijndael);
                value = crypto.Encrypting(Convert.ToString(value), getKeyString(true));
            }
            return value;
        }
    }
}
