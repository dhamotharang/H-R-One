using System;
using System.Collections;
using System.Text;
using HROne.DataAccess;
using HROne.Common;
////using perspectivemind.validation;

namespace HROne.SaaS.Entities
{

    [DBClass("CompanyMPFFileSignature")]
    public class ECompanyMPFFileSignature : DBObject
    {
        public static DBManager db = new DBManager(typeof(ECompanyMPFFileSignature));

        protected int m_CompanyMPFFileSignatureID;
        [DBField("CompanyMPFFileSignatureID", true, true), TextSearch, Export(false)]
        public int CompanyMPFFileSignatureID
        {
            get { return m_CompanyMPFFileSignatureID; }
            set { m_CompanyMPFFileSignatureID = value; modify("CompanyMPFFileSignatureID"); }
        }
        protected int m_CompanyMPFFileID;
        [DBField("CompanyMPFFileID"), Export(false), Required]
        public int CompanyMPFFileID
        {
            get { return m_CompanyMPFFileID; }
            set { m_CompanyMPFFileID = value; modify("CompanyMPFFileID"); }
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
        protected string m_CompanyMPFFileSignatureUserName;
        [DBField("CompanyMPFFileSignatureUserName"), Export(false), Required]
        public string CompanyMPFFileSignatureUserName
        {
            get { return m_CompanyMPFFileSignatureUserName; }
            set { m_CompanyMPFFileSignatureUserName = value; modify("CompanyMPFFileSignatureUserName"); }
        }
        protected DateTime m_CompanyMPFFileSignatureDateTime;
        [DBField("CompanyMPFFileSignatureDateTime"), Export(false), Required]
        public DateTime CompanyMPFFileSignatureDateTime
        {
            get { return m_CompanyMPFFileSignatureDateTime; }
            set { m_CompanyMPFFileSignatureDateTime = value; modify("CompanyMPFFileSignatureDateTime"); }
        }
    }
}
