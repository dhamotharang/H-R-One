using System;
using System.Collections;
using System.Text;
using HROne.DataAccess;
using HROne.Common;
////using perspectivemind.validation;

namespace HROne.SaaS.Entities
{

    [DBClass("CompanyMPFFile")]
    public class ECompanyMPFFile : DBObject
    {
        public static DBManager db = new DBManager(typeof(ECompanyMPFFile));

        protected int m_CompanyMPFFileID;
        [DBField("CompanyMPFFileID", true, true), TextSearch, Export(false)]
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
        protected int m_HSBCExchangeProfileID;
        [DBField("HSBCExchangeProfileID"), Export(false)]
        public int HSBCExchangeProfileID
        {
            get { return m_HSBCExchangeProfileID; }
            set { m_HSBCExchangeProfileID = value; modify("HSBCExchangeProfileID"); }
        }
        protected string m_CompanyMPFFileTrusteeCode;
        [DBField("CompanyMPFFileTrusteeCode"), Export(false), Required]
        public string CompanyMPFFileTrusteeCode
        {
            get { return m_CompanyMPFFileTrusteeCode; }
            set { m_CompanyMPFFileTrusteeCode = value; modify("CompanyMPFFileTrusteeCode"); }
        }
        protected string m_CompanyMPFFileFileType;
        [DBField("CompanyMPFFileFileType"), Export(false), Required]
        public string CompanyMPFFileFileType
        {
            get { return m_CompanyMPFFileFileType; }
            set { m_CompanyMPFFileFileType = value; modify("CompanyMPFFileFileType"); }
        }
        
        protected string m_CompanyMPFFileDataFileRelativePath;
        [DBField("CompanyMPFFileDataFileRelativePath"), Export(false), Required]
        public string CompanyMPFFileDataFileRelativePath
        {
            get { return m_CompanyMPFFileDataFileRelativePath; }
            set { m_CompanyMPFFileDataFileRelativePath = value; modify("CompanyMPFFileDataFileRelativePath"); }
        }
        protected string m_CompanyMPFFileReportFileRelativePath;
        [DBField("CompanyMPFFileReportFileRelativePath"), Export(false), Required]
        public string CompanyMPFFileReportFileRelativePath
        {
            get { return m_CompanyMPFFileReportFileRelativePath; }
            set { m_CompanyMPFFileReportFileRelativePath = value; modify("CompanyMPFFileReportFileRelativePath"); }
        }
        protected DateTime m_CompanyMPFFileSubmitDateTime;
        [DBField("CompanyMPFFileSubmitDateTime"), Export(false)]
        public DateTime CompanyMPFFileSubmitDateTime
        {
            get { return m_CompanyMPFFileSubmitDateTime; }
            set { m_CompanyMPFFileSubmitDateTime = value; modify("CompanyMPFFileSubmitDateTime"); }
        }
        protected DateTime m_CompanyMPFFileConfirmDateTime;
        [DBField("CompanyMPFFileConfirmDateTime"), Export(false)]
        public DateTime CompanyMPFFileConfirmDateTime
        {
            get { return m_CompanyMPFFileConfirmDateTime; }
            set { m_CompanyMPFFileConfirmDateTime = value; modify("CompanyMPFFileConfirmDateTime"); }
        }
        protected DateTime m_CompanyMPFFileConsolidateDateTime;
        [DBField("CompanyMPFFileConsolidateDateTime"), Export(false)]
        public DateTime CompanyMPFFileConsolidateDateTime
        {
            get { return m_CompanyMPFFileConsolidateDateTime; }
            set { m_CompanyMPFFileConsolidateDateTime = value; modify("CompanyMPFFileConsolidateDateTime"); }
        }

        protected string m_CompanyMPFFileTransactionReference;
        [DBField("CompanyMPFFileTransactionReference"), Export(false)]
        public string CompanyMPFFileTransactionReference
        {
            get { return m_CompanyMPFFileTransactionReference; }
            set { m_CompanyMPFFileTransactionReference = value; modify("CompanyMPFFileTransactionReference"); }
        }
    }
}
