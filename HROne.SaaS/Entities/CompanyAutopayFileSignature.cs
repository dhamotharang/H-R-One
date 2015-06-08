using System;
using System.Collections;
using System.Text;
using HROne.DataAccess;
using HROne.Common;
////using perspectivemind.validation;

namespace HROne.SaaS.Entities
{

    [DBClass("CompanyAutopayFileSignature")]
    public class ECompanyAutopayFileSignature : DBObject
    {
        public static DBManager db = new DBManager(typeof(ECompanyAutopayFileSignature));

        protected int m_CompanyAutopayFileSignatureID;
        [DBField("CompanyAutopayFileSignatureID", true, true), TextSearch, Export(false)]
        public int CompanyAutopayFileSignatureID
        {
            get { return m_CompanyAutopayFileSignatureID; }
            set { m_CompanyAutopayFileSignatureID = value; modify("CompanyAutopayFileSignatureID"); }
        }
        protected int m_CompanyAutopayFileID;
        [DBField("CompanyAutopayFileID"), Export(false), Required]
        public int CompanyAutopayFileID
        {
            get { return m_CompanyAutopayFileID; }
            set { m_CompanyAutopayFileID = value; modify("CompanyAutopayFileID"); }
        }
        protected int m_CompanyDBID;
        [DBField("CompanyDBID"), Export(false), Required]
        public int CompanyDBID
        {
            get { return m_CompanyDBID; }
            set { m_CompanyDBID = value; modify("CompanyDBID"); }
        }
        protected int m_UserID;
        [DBField("UserID"), Export(false), Required]
        public int UserID
        {
            get { return m_UserID; }
            set { m_UserID = value; modify("UserID"); }
        }
        protected string m_CompanyAutopayFileSignatureUserName;
        [DBField("CompanyAutopayFileSignatureUserName"), Export(false), Required]
        public string CompanyAutopayFileSignatureUserName
        {
            get { return m_CompanyAutopayFileSignatureUserName; }
            set { m_CompanyAutopayFileSignatureUserName = value; modify("CompanyAutopayFileSignatureUserName"); }
        }
        protected DateTime m_CompanyAutopayFileSignatureDateTime;
        [DBField("CompanyAutopayFileSignatureDateTime"), Export(false)]
        public DateTime CompanyAutopayFileSignatureDateTime
        {
            get { return m_CompanyAutopayFileSignatureDateTime; }
            set { m_CompanyAutopayFileSignatureDateTime = value; modify("CompanyAutopayFileSignatureDateTime"); }
        }
    }
}
