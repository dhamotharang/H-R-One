using System;
using System.Collections;
using System.Text;
using HROne.DataAccess;
using HROne.Common;
////using perspectivemind.validation;

namespace HROne.SaaS.Entities
{
    [DBClass("DatabaseServer")]
    public class EDatabaseServer : DBObject
    {
        private const string keyString = "HROne";
        public static DBManager db = new DBManager(typeof(EDatabaseServer));
        public static WFValueList VLDBServerList = new AppUtils.EncryptedDBCodeList(db, "DBServerID", new string[] { "DBServerCode" }, string.Empty, "DBServerCode");

        protected int m_DBServerID;
        [DBField("DBServerID", true, true), TextSearch, Export(false)]
        public int DBServerID
        {
            get { return m_DBServerID; }
            set { m_DBServerID = value; modify("DBServerID"); }
        }
        protected string m_DBServerCode;
        [DBField("DBServerCode"), DBAESEncryptStringField, MaxLength(20, 20), TextSearch, Export(false), Required]
        public string DBServerCode
        {
            get { return m_DBServerCode; }
            set { m_DBServerCode = value; modify("DBServerCode"); }
        }
        protected string m_DBServerDBType;
        [DBField("DBServerDBType"), DBAESEncryptStringField, TextSearch, MaxLength(50, 40), Export(false), Required]
        public string DBServerDBType
        {
            get { return m_DBServerDBType; }
            set { m_DBServerDBType = value; modify("DBServerDBType"); }
        }
        protected string m_DBServerLocation;
        [DBField("DBServerLocation"), DBAESEncryptStringField, TextSearch, Export(false), Required]
        public string DBServerLocation
        {
            get { return m_DBServerLocation; }
            set { m_DBServerLocation = value; modify("DBServerLocation"); }
        }

        protected string m_DBServerSAUserID;
        [DBField("DBServerSAUserID"), DBAESEncryptStringField, TextSearch, Export(false), Required]
        public string DBServerSAUserID
        {
            get { return m_DBServerSAUserID; }
            set { m_DBServerSAUserID = value; modify("DBServerSAUserID"); }
        }

        protected string m_DBServerSAPassword;
        [DBField("DBServerSAPassword"), DBAESEncryptStringField, TextSearch, Export(false), Required]
        public string DBServerSAPassword
        {
            get { return m_DBServerSAPassword; }
            set { m_DBServerSAPassword = value; modify("DBServerSAPassword"); }
        }

        protected string m_DBServerUserID;
        [DBField("DBServerUserID"), DBAESEncryptStringField, TextSearch, Export(false), Required]
        public string DBServerUserID
        {
            get { return m_DBServerUserID; }
            set { m_DBServerUserID = value; modify("DBServerUserID"); }
        }

        protected string m_DBServerPassword;
        [DBField("DBServerPassword"), DBAESEncryptStringField, TextSearch, Export(false), Required]
        public string DBServerPassword
        {
            get { return m_DBServerPassword; }
            set { m_DBServerPassword = value; modify("DBServerPassword"); }
        }
        
        //public string GetConnectionString()
        //{
        //    //HROne.CommonLib.Crypto crypto = new HROne.CommonLib.Crypto(HROne.CommonLib.Crypto.SymmProvEnum.Rijndael);

        //    //if (m_DBServerDBType.Equals("MSSQL"))
        //    //{
        //    //    System.Data.SqlClient.SqlConnectionStringBuilder connStringBuilder = new System.Data.SqlClient.SqlConnectionStringBuilder(m_DBServerConnectionString);
        //    //    try
        //    //    {
        //    //        connStringBuilder.Password = crypto.Decrypting(connStringBuilder.Password, keyString);
        //    //    }
        //    //    catch
        //    //    {

        //    //    }
        //    //    return connStringBuilder.ConnectionString;
        //    //}
        //    //else
        //    //    return string.Empty;
        //}

        //public void  SetConnectionString(string ConnectionString)
        //{
        //    HROne.CommonLib.Crypto crypto = new HROne.CommonLib.Crypto(HROne.CommonLib.Crypto.SymmProvEnum.Rijndael);

        //    if (m_DBServerDBType.Equals("MSSQL"))
        //    {
        //        System.Data.SqlClient.SqlConnectionStringBuilder connStringBuilder = new System.Data.SqlClient.SqlConnectionStringBuilder(ConnectionString);
        //        try
        //        {
        //            connStringBuilder.Password = crypto.Encrypting(connStringBuilder.Password, keyString);
        //        }
        //        catch
        //        {

        //        }
        //        DBServerConnectionString = connStringBuilder.ConnectionString;
        //    }
        //}
    }
}
