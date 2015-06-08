//using System;
//using System.Collections.Generic;
//using System.Text;
//using HROne.DataAccess;
//using HROne.CommonLib;

//namespace HROne.Common
//{
//    public class DBAESEncryptStringFieldAttribute : Attribute, DBFieldTranscoder 
//    {
//        const string DEFAULT_KEY_STRING = "HROneHROne";
//        string keyString = DEFAULT_KEY_STRING;
//        //bool migrateFromSimpleDBEncrypt = false;
//        public static bool skipEncryptionToDB = false;

//        public DBAESEncryptStringFieldAttribute()
//        {
//        }

//        //public DBAESEncryptStringFieldAttribute(bool migrateFromSimpleDBEncrypt)
//        //{
//        //    this.migrateFromSimpleDBEncrypt = migrateFromSimpleDBEncrypt;
//        //}

//        public DBAESEncryptStringFieldAttribute(string keyString)
//        {
//            this.keyString = keyString;
//        }

//        //public DBAESEncryptStringFieldAttribute(string keyString, bool migrateFromSimpleDBEncrypt)
//        //{
//        //    this.keyString = keyString;
//        //    this.migrateFromSimpleDBEncrypt = migrateFromSimpleDBEncrypt;

//        //}

//        //public static void decode(System.Data.DataTable table, string name, bool migrateFromSimpleDBEncrypt)
//        //{
//        //    foreach (System.Data.DataRow row in table.Rows)
//        //        row[name] = decode(Convert.ToString(row[name]), migrateFromSimpleDBEncrypt);

//        //}

//        public static void decode(System.Data.DataTable table, string name)
//        {
//            foreach (System.Data.DataRow row in table.Rows)
//                if (!row.IsNull(name))
//                    row[name] = decode(row[name]);

//        }

//        //public static object decode(object value, bool migrateFromSimpleDBEncrypt)
//        //{
//        //    DBAESEncryptStringFieldAttribute aesEncrypt = new DBAESEncryptStringFieldAttribute(migrateFromSimpleDBEncrypt);
//        //    try
//        //    {
//        //        value = aesEncrypt.fromDB(Convert.ToString(value));
//        //    }
//        //    catch (Exception e)
//        //    {
//        //        if (migrateFromSimpleDBEncrypt)
//        //        {
//        //            return DBSimpleEncryptAttribute.decode(value);
//        //        }
//        //        else
//        //            return value;
//        //    }
//        //    return value;
//        //}

//        public static object decode(object value)
//        {
//            DBAESEncryptStringFieldAttribute aesEncrypt = new DBAESEncryptStringFieldAttribute();
//            try
//            {
//                value = aesEncrypt.fromDB(value);
//            }
//            catch (Exception ex)
//            {
//                throw ex;
//            }
//            return value;
//        }

//        public object fromDB(object value)
//        {
//            if (value is string)
//            {

//                //  Check string format to minimize the time consuming caused by generating exception message
//                if (string.IsNullOrEmpty((string)value))
//                    return value;
//                if (((string)value).Length % 4 != 0)
//                    return value;

//                System.Text.RegularExpressions.Regex regExBase64 = new System.Text.RegularExpressions.Regex(@"[0-9a-zA-Z\+/=]{20,}");
//                if (!regExBase64.IsMatch((string)value))
//                    return value;

//                //if (((string)value).Contains(" "))
//                //    return value;
//                //if (((string)value).Contains("(") || ((string)value).Contains(")"))
//                //    return value;
//                byte[] bytIn;
//                try
//                {
//                    bytIn = Convert.FromBase64String((string)value);
//                }
//                catch (Exception ex)
//                {
//                    System.Diagnostics.Debug.WriteLine(ex.Message);
//                    return value;
//                }
//                if (bytIn.Length % 16 != 0)
//                {
//                    return value;
//                }
//                Crypto crypto = new Crypto(Crypto.SymmProvEnum.Rijndael);
//                try
//                {
//                    value = crypto.Decrypting(Convert.ToString(value), keyString);
//                }
//                catch (System.Security.Cryptography.CryptographicException ex)
//                {
//                    System.Diagnostics.Debug.WriteLine(ex.Message);
//                    return value;
//                    //throw ex;
//                }
//                catch (Exception ex)
//                {
//                    throw ex;
//                }
//            }
//            return value;
//        }
//        public object toDB(object value)
//        {
//            if (!skipEncryptionToDB)
//            {
//                Crypto crypto = new Crypto(Crypto.SymmProvEnum.Rijndael);
//                value = crypto.Encrypting(Convert.ToString(value), keyString);
//            }
//            return value;
//        }
//    }

//    //public class DBAESEncryptDoubleFieldAttribute : Attribute, DBFieldTranscoder
//    //{
//    //    string keyString = "HROneHROne";

//    //    public DBAESEncryptDoubleFieldAttribute()
//    //    {
//    //    }


//    //    public DBAESEncryptDoubleFieldAttribute(string keyString)
//    //    {
//    //        this.keyString = keyString;
//    //    }


//    //    public static void decode(System.Data.DataTable table, string sourceFieldName, string destinationFieldName)
//    //    {
//    //        if (table.Columns[destinationFieldName] == null)
//    //            table.Columns.Add(destinationFieldName, typeof(double));

//    //        foreach (System.Data.DataRow row in table.Rows)
//    //        {
//    //            row[destinationFieldName] = decode(Convert.ToString(row[sourceFieldName]));
//    //            row[sourceFieldName] = row[destinationFieldName];
//    //        }
//    //        //table.Columns.Remove(sourceFieldName);
//    //    }

//    //    public static object decode(object value)
//    //    {
//    //        DBAESEncryptDoubleFieldAttribute aesEncrypt = new DBAESEncryptDoubleFieldAttribute();
//    //        try
//    //        {
//    //            value = aesEncrypt.fromDB(Convert.ToString(value));
//    //        }
//    //        catch (Exception e)
//    //        {
//    //                return value;
//    //        }
//    //        return value;
//    //    }

//    //    public object fromDB(object value)
//    //    {
//    //        Crypto crypto = new Crypto(Crypto.SymmProvEnum.Rijndael);
//    //        double result;
//    //        try
//    //        {
//    //            result = Convert.ToDouble(crypto.Decrypting(Convert.ToString(value), keyString));
//    //        }
//    //        catch (Exception e)
//    //        {
//    //            return value;
//    //        }
//    //        return result;
//    //    }
//    //    public object toDB(object value)
//    //    {
//    //        Crypto crypto = new Crypto(Crypto.SymmProvEnum.Rijndael);

//    //        value = crypto.Encrypting(Convert.ToString(value), keyString);
//    //        return value;
//    //    }
//    //}
//}
