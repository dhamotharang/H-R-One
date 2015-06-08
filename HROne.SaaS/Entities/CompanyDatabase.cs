using System;
using System.Collections;
using System.Text;
using HROne.DataAccess;
using HROne.Common;
////using perspectivemind.validation;

namespace HROne.SaaS.Entities
{

    [DBClass("CompanyDatabase")]
    public class ECompanyDatabase : DBObject
    {
        public static DBManager db = new DBManager(typeof(ECompanyDatabase));
        private const string FEATURE_CODE_MAX_EMPLOYEE = "MAX_EMPLOYEE";
        private const string FEATURE_CODE_MAX_INBOX_SIZE_MB = "MAX_INBOX_SIZE_MB";
        private const string FEATURE_CODE_AUTOPAYMPF_HAS_HSBCHASE = "AUTOPAYMPF_HAS_HSBCHASE";
        private const string FEATURE_CODE_AUTOPAYMPF_HAS_OTHERS = "AUTOPAYMPF_HAS_OTHERS";

        protected int m_CompanyDBID;
        [DBField("CompanyDBID", true, true), TextSearch, Export(false)]
        public int CompanyDBID
        {
            get { return m_CompanyDBID; }
            set { m_CompanyDBID = value; modify("CompanyDBID"); }
        }
        protected string m_CompanyDBClientCode;
        [DBField("CompanyDBClientCode"), MaxLength(20, 20), TextSearch, Export(false)]
        public string CompanyDBClientCode
        {
            get { return m_CompanyDBClientCode.ToUpper(); }
            set { m_CompanyDBClientCode = string.IsNullOrEmpty(value) ? string.Empty : value.ToUpper(); modify("CompanyDBClientCode"); }
        }
        protected string m_CompanyDBClientContactPerson;
        [DBField("CompanyDBClientContactPerson"), MaxLength(100, 100), DBAESEncryptStringField, TextSearch, Export(false), Required]
        public string CompanyDBClientContactPerson
        {
            get { return m_CompanyDBClientContactPerson; }
            set { m_CompanyDBClientContactPerson = value; modify("CompanyDBClientContactPerson"); }
        }
        protected string m_CompanyDBClientName;
        [DBField("CompanyDBClientName"), MaxLength(100, 100), DBAESEncryptStringField, TextSearch, Export(false), Required]
        public string CompanyDBClientName
        {
            get { return m_CompanyDBClientName; }
            set { m_CompanyDBClientName = value; modify("CompanyDBClientName"); }
        }
        protected string m_CompanyDBClientAddress;
        [DBField("CompanyDBClientAddress"), DBAESEncryptStringField, TextSearch, Export(false), Required]
        public string CompanyDBClientAddress
        {
            get { return m_CompanyDBClientAddress; }
            set { m_CompanyDBClientAddress = value; modify("CompanyDBClientAddress"); }
        }
        protected string m_CompanyDBClientBank;
        [DBField("CompanyDBClientBank"), TextSearch, DBAESEncryptStringField, Export(false), Required]
        public string CompanyDBClientBank
        {
            get { return m_CompanyDBClientBank; }
            set { m_CompanyDBClientBank = value; modify("CompanyDBClientBank"); }
        }
        protected int m_DBServerID;
        [DBField("DBServerID"), TextSearch, Export(false), Required]
        public int DBServerID
        {
            get { return m_DBServerID; }
            set { m_DBServerID = value; modify("DBServerID"); }
        }
        protected string m_CompanyDBSchemaName;
        [DBField("CompanyDBSchemaName"), TextSearch, DBAESEncryptStringField, Export(false)]
        public string CompanyDBSchemaName
        {
            get { return m_CompanyDBSchemaName; }
            set { m_CompanyDBSchemaName = value; modify("CompanyDBSchemaName"); }
        }
        protected bool m_CompanyDBIsActive;
        [DBField("CompanyDBIsActive"), TextSearch, Export(false)]
        public bool CompanyDBIsActive
        {
            get { return m_CompanyDBIsActive; }
            set { m_CompanyDBIsActive = value; modify("CompanyDBIsActive"); }
        }
        protected int m_CompanyDBMaxCompany;
        [DBField("CompanyDBMaxCompany"), IntRange(0, 65535), TextSearch, Export(false)]
        public int CompanyDBMaxCompany
        {
            get { return m_CompanyDBMaxCompany; }
            set { m_CompanyDBMaxCompany = value; modify("CompanyDBMaxCompany"); }
        }
        protected int m_CompanyDBMaxUser;
        [DBField("CompanyDBMaxUser"), IntRange(0, 65535), TextSearch, Export(false)]
        public int CompanyDBMaxUser
        {
            get { return m_CompanyDBMaxUser; }
            set { m_CompanyDBMaxUser = value; modify("CompanyDBMaxUser"); }
        }
        protected int m_CompanyDBMaxEmployee;
        [DBField("CompanyDBMaxEmployee"), IntRange(0, 65535), TextSearch, Export(false)]
        public int CompanyDBMaxEmployee
        {
            get { return m_CompanyDBMaxEmployee; }
            set { m_CompanyDBMaxEmployee = value; modify("CompanyDBMaxEmployee"); }
        }
        protected string m_CompanyDBProductKey;
        [DBField("CompanyDBProductKey"), TextSearch, DBAESEncryptStringField, Export(false)]
        public string CompanyDBProductKey
        {
            get { return m_CompanyDBProductKey; }
            set { m_CompanyDBProductKey = value; modify("CompanyDBProductKey"); }
        }
        protected string m_CompanyDBTrialKey;
        [DBField("CompanyDBTrialKey"), TextSearch, DBAESEncryptStringField, Export(false)]
        public string CompanyDBTrialKey
        {
            get { return m_CompanyDBTrialKey; }
            set { m_CompanyDBTrialKey = value; modify("CompanyDBTrialKey"); }
        }
        protected string m_CompanyDBAuthorizationCode;
        [DBField("CompanyDBAuthorizationCode"), TextSearch, DBAESEncryptStringField, Export(false)]
        public string CompanyDBAuthorizationCode
        {
            get { return m_CompanyDBAuthorizationCode; }
            set { m_CompanyDBAuthorizationCode = value; modify("CompanyDBAuthorizationCode"); }
        }
        protected string m_CompanyDBResetDefaultUserLoginID;
        [DBField("CompanyDBResetDefaultUserLoginID"), TextSearch, Export(false)]
        public string CompanyDBResetDefaultUserLoginID
        {
            get { return m_CompanyDBResetDefaultUserLoginID; }
            set { m_CompanyDBResetDefaultUserLoginID = value; modify("CompanyDBResetDefaultUserLoginID"); }
        }
        protected string m_CompanyDBResetDefaultUserPassword;
        [DBField("CompanyDBResetDefaultUserPassword"), TextSearch, Export(false)]
        public string CompanyDBResetDefaultUserPassword
        {
            get { return m_CompanyDBResetDefaultUserPassword; }
            set { m_CompanyDBResetDefaultUserPassword = value; modify("CompanyDBResetDefaultUserPassword"); }
        }
        protected bool m_CompanyDBAutopayMPFFileHasHSBCHASE;
        [DBField("CompanyDBAutopayMPFFileHasHSBCHASE"), TextSearch, Export(false)]
        public bool CompanyDBAutopayMPFFileHasHSBCHASE
        {
            get { return m_CompanyDBAutopayMPFFileHasHSBCHASE; }
            set { m_CompanyDBAutopayMPFFileHasHSBCHASE = value; modify("CompanyDBAutopayMPFFileHasHSBCHASE"); }
        }
        protected bool m_CompanyDBAutopayMPFFileHasOthers;
        [DBField("CompanyDBAutopayMPFFileHasOthers"), TextSearch, Export(false)]
        public bool CompanyDBAutopayMPFFileHasOthers
        {
            get { return m_CompanyDBAutopayMPFFileHasOthers; }
            set { m_CompanyDBAutopayMPFFileHasOthers = value; modify("CompanyDBAutopayMPFFileHasOthers"); }
        }
        protected bool m_CompanyDBHasEChannel;
        [DBField("CompanyDBHasEChannel"), TextSearch, Export(false)]
        public bool CompanyDBHasEChannel
        {
            get { return m_CompanyDBHasEChannel; }
            set { m_CompanyDBHasEChannel = value; modify("CompanyDBHasEChannel"); }
        }
        protected bool m_CompanyDBHasIMGR;
        [DBField("CompanyDBHasIMGR"), TextSearch, Export(false)]
        public bool CompanyDBHasIMGR
        {
            get { return m_CompanyDBHasIMGR; }
            set { m_CompanyDBHasIMGR = value; modify("CompanyDBHasIMGR"); }
        }
        protected bool m_CompanyDBHasIStaff;
        [DBField("CompanyDBHasIStaff"), TextSearch, Export(false)]
        public bool CompanyDBHasIStaff
        {
            get { return m_CompanyDBHasIStaff; }
            set { m_CompanyDBHasIStaff = value; modify("CompanyDBHasIStaff"); }
        }
        protected int m_CompanyDBInboxMaxQuotaMB;
        [DBField("CompanyDBInboxMaxQuotaMB"), IntRange(0, 500), TextSearch, Export(false)]
        public int CompanyDBInboxMaxQuotaMB
        {
            get { return m_CompanyDBInboxMaxQuotaMB; }
            set { m_CompanyDBInboxMaxQuotaMB = value; modify("CompanyDBInboxMaxQuotaMB"); }
        }
        public string getConnectionString(DatabaseConnection dbConn)
        {
            EDatabaseServer dbServer = new EDatabaseServer();
            dbServer.DBServerID = m_DBServerID;
            if (EDatabaseServer.db.select(dbConn, dbServer))
            {
                if (dbServer.DBServerDBType.Equals("MSSQL"))
                {
                    System.Data.SqlClient.SqlConnectionStringBuilder builder = new System.Data.SqlClient.SqlConnectionStringBuilder();
                    builder.DataSource = dbServer.DBServerLocation;
                    if (string.IsNullOrEmpty(m_CompanyDBSchemaName))
                        builder.InitialCatalog = m_CompanyDBClientCode;
                    else
                        builder.InitialCatalog = m_CompanyDBSchemaName;
                    builder.UserID = dbServer.DBServerUserID;
                    builder.Password = dbServer.DBServerPassword;
                    return builder.ConnectionString;
                }
            }
            return string.Empty;
        }
        public string getProductFeatureCode(DatabaseConnection dbConn)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(FEATURE_CODE_MAX_EMPLOYEE + "=" + m_CompanyDBMaxEmployee.ToString() + ";");
            int defaultMaxQuotaMB = 0;
            if (!int.TryParse(ESystemParameter.getParameter(dbConn, ESystemParameter.PARAM_CODE_DEFAULT_MAX_INBOX_SIZE_MB), out defaultMaxQuotaMB))
                defaultMaxQuotaMB = 0;
            builder.Append(FEATURE_CODE_MAX_INBOX_SIZE_MB + "=" + (defaultMaxQuotaMB > m_CompanyDBInboxMaxQuotaMB ? defaultMaxQuotaMB.ToString() : m_CompanyDBInboxMaxQuotaMB.ToString()) + ";");
            builder.Append(FEATURE_CODE_AUTOPAYMPF_HAS_HSBCHASE + "=" + (m_CompanyDBAutopayMPFFileHasHSBCHASE ? "Y" : "N") + ";");
            builder.Append(FEATURE_CODE_AUTOPAYMPF_HAS_OTHERS + "=" + (m_CompanyDBAutopayMPFFileHasOthers ? "Y" : "N") + ";");
            HROne.CommonLib.Crypto crypto = new HROne.CommonLib.Crypto(HROne.CommonLib.Crypto.SymmProvEnum.RC2);
            return crypto.Encrypting(builder.ToString(), HROne.ProductLicense.ProductLicenseType.HROneSaaS.ToString());
        }
    }
}
