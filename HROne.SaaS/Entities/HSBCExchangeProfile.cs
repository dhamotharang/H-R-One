using System;
using System.Collections;
using System.Text;
using HROne.DataAccess;
using HROne.Common;

namespace HROne.SaaS.Entities
{
    [DBClass("HSBCExchangeProfile")]
    public class EHSBCExchangeProfile : DBObject
    {
        public static DBManager db = new DBManager(typeof(EHSBCExchangeProfile));
        public static WFValueList VLRemoteProfileList = new AppUtils.EncryptedDBCodeList(db, "HSBCExchangeProfileID", new string[] { "HSBCExchangeProfileRemoteProfileID" }, string.Empty, "HSBCExchangeProfileRemoteProfileID");

        protected int m_HSBCExchangeProfileID;
        [DBField("HSBCExchangeProfileID", true, true), TextSearch, Export(false)]
        public int HSBCExchangeProfileID
        {
            get { return m_HSBCExchangeProfileID; }
            set { m_HSBCExchangeProfileID = value; modify("HSBCExchangeProfileID"); }
        }
        protected int m_CompanyDBID;
        [DBField("CompanyDBID"), Export(false), Required]
        public int CompanyDBID
        {
            get { return m_CompanyDBID; }
            set { m_CompanyDBID = value; modify("CompanyDBID"); }
        }

        protected string m_HSBCExchangeProfileRemoteProfileID;
        [DBField("HSBCExchangeProfileRemoteProfileID"), TextSearch, DBAESEncryptStringField(true), MaxLength(18), Export(false), Required]
        public string HSBCExchangeProfileRemoteProfileID
        {
            get { return m_HSBCExchangeProfileRemoteProfileID; }
            set { m_HSBCExchangeProfileRemoteProfileID = (value == null ? string.Empty : value.ToUpper().Trim()); modify("HSBCExchangeProfileRemoteProfileID"); }
        }

        protected string m_HSBCExchangeProfileBankCode;
        [DBField("HSBCExchangeProfileBankCode"), TextSearch, DBAESEncryptStringField(true), Export(false), Required]
        public string HSBCExchangeProfileBankCode
        {
            get { return m_HSBCExchangeProfileBankCode; }
            set { m_HSBCExchangeProfileBankCode = (value == null ? string.Empty : value.Trim()); modify("HSBCExchangeProfileBankCode"); }
        }

        protected bool m_HSBCExchangeProfileIsLocked;
        [DBField("HSBCExchangeProfileIsLocked"), Export(false)]
        public bool HSBCExchangeProfileIsLocked
        {
            get { return m_HSBCExchangeProfileIsLocked; }
            set { m_HSBCExchangeProfileIsLocked = value; modify("HSBCExchangeProfileIsLocked"); }
        }
    }
}
