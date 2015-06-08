using System;
using System.Collections;
using System.Text;
using HROne.DataAccess;
using HROne.Common;
////using perspectivemind.validation;

namespace HROne.SaaS.Entities
{

    [DBClass("CompanyAutopayFile")]
    public class ECompanyAutopayFile : DBObject
    {
        public static DBManager db = new DBManager(typeof(ECompanyAutopayFile));

        protected int m_CompanyAutopayFileID;
        [DBField("CompanyAutopayFileID", true, true), TextSearch, Export(false)]
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
        protected int m_HSBCExchangeProfileID;
        [DBField("HSBCExchangeProfileID"), Export(false)]
        public int HSBCExchangeProfileID
        {
            get { return m_HSBCExchangeProfileID; }
            set { m_HSBCExchangeProfileID = value; modify("HSBCExchangeProfileID"); }
        }
        protected string m_CompanyAutopayFileBankCode;
        [DBField("CompanyAutopayFileBankCode"), Export(false), Required]
        public string CompanyAutopayFileBankCode
        {
            get { return m_CompanyAutopayFileBankCode; }
            set { m_CompanyAutopayFileBankCode = value; modify("CompanyAutopayFileBankCode"); }
        }
        protected string m_CompanyAutopayFileDataFileRelativePath;
        [DBField("CompanyAutopayFileDataFileRelativePath"), Export(false), Required]
        public string CompanyAutopayFileDataFileRelativePath
        {
            get { return m_CompanyAutopayFileDataFileRelativePath; }
            set { m_CompanyAutopayFileDataFileRelativePath = value; modify("CompanyAutopayFileDataFileRelativePath"); }
        }
        protected string m_CompanyAutopayFileReportFileRelativePath;
        [DBField("CompanyAutopayFileReportFileRelativePath"), Export(false), Required]
        public string CompanyAutopayFileReportFileRelativePath
        {
            get { return m_CompanyAutopayFileReportFileRelativePath; }
            set { m_CompanyAutopayFileReportFileRelativePath = value; modify("CompanyAutopayFileReportFileRelativePath"); }
        }

        protected DateTime m_CompanyAutopayFileValueDate;
        [DBField("CompanyAutopayFileValueDate"), Export(false)]
        public DateTime CompanyAutopayFileValueDate
        {
            get { return m_CompanyAutopayFileValueDate; }
            set { m_CompanyAutopayFileValueDate = value; modify("CompanyAutopayFileValueDate"); }
        }
        protected DateTime m_CompanyAutopayFileSubmitDateTime;
        [DBField("CompanyAutopayFileSubmitDateTime"), Export(false)]
        public DateTime CompanyAutopayFileSubmitDateTime
        {
            get { return m_CompanyAutopayFileSubmitDateTime; }
            set { m_CompanyAutopayFileSubmitDateTime = value; modify("CompanyAutopayFileSubmitDateTime"); }
        }
        protected DateTime m_CompanyAutopayFileConfirmDateTime;
        [DBField("CompanyAutopayFileConfirmDateTime"), Export(false)]
        public DateTime CompanyAutopayFileConfirmDateTime
        {
            get { return m_CompanyAutopayFileConfirmDateTime; }
            set { m_CompanyAutopayFileConfirmDateTime = value; modify("CompanyAutopayFileConfirmDateTime"); }
        }
        protected DateTime m_CompanyAutopayFileConsolidateDateTime;
        [DBField("CompanyAutopayFileConsolidateDateTime"), Export(false)]
        public DateTime CompanyAutopayFileConsolidateDateTime
        {
            get { return m_CompanyAutopayFileConsolidateDateTime; }
            set { m_CompanyAutopayFileConsolidateDateTime = value; modify("CompanyAutopayFileConsolidateDateTime"); }
        }
        protected string m_CompanyAutopayFileTransactionReference;
        [DBField("CompanyAutopayFileTransactionReference"), Export(false)]
        public string CompanyAutopayFileTransactionReference
        {
            get { return m_CompanyAutopayFileTransactionReference; }
            set { m_CompanyAutopayFileTransactionReference = value; modify("CompanyAutopayFileTransactionReference"); }
        }
    }
}
