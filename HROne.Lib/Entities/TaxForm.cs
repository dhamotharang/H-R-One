using System;
using System.Data;
using System.Configuration;

using HROne.DataAccess;
////using perspectivemind.validation;

namespace HROne.Lib.Entities
{
    /// <summary>
    /// Summary description for TaxForm
    /// </summary>
    [DBClass("TaxForm")]
    public class ETaxForm : BaseObject 
    {
        public static DBManager db = new DBManager(typeof(ETaxForm));
        public static WFValueList VLTaxFormYear = new WFDBCodeList(db, "TaxFormID", "TaxFormYear-1", "TaxFormYear", "TaxFormYear desc");
        // Start 0000020, KuangWei, 2014-07-15
        public static WFValueList VLTaxFormType = new AppUtils.NewWFTextList(new string[] { "B", "E", "F", "G", "M" }, new string[] { "B", "E", "F", "G", "M" });
        // End 0000020, KuangWei, 2014-07-15

        protected int m_TaxFormID;
        [DBField("TaxFormID", true, true), TextSearch, Export(false)]
        public int TaxFormID
        {
            get { return m_TaxFormID; }
            set { m_TaxFormID = value; modify("TaxFormID"); }
        }

        protected int m_TaxCompID;
        [DBField("TaxCompID"), TextSearch, Export(false), Required]
        public int TaxCompID
        {
            get { return m_TaxCompID; }
            set { m_TaxCompID = value; modify("TaxCompID"); }
        }

        protected string m_TaxFormSection;
        [DBField("TaxFormSection"), TextSearch, Export(false)]
        public string TaxFormSection
        {
            get { return m_TaxFormSection; }
            set { m_TaxFormSection = value; modify("TaxFormSection"); }
        }

        protected string m_TaxFormERN;
        [DBField("TaxFormERN"), TextSearch, Export(false)]
        public string TaxFormERN
        {
            get { return m_TaxFormERN; }
            set { m_TaxFormERN = value; modify("TaxFormERN"); }
        }

        protected int m_TaxFormYear;
        [DBField("TaxFormYear"), TextSearch, Export(false), Required]
        public int TaxFormYear
        {
            get { return m_TaxFormYear; }
            set { m_TaxFormYear = value; modify("TaxFormYear"); }
        }

        protected DateTime m_TaxFormSubmissionDate;
        [DBField("TaxFormSubmissionDate"), TextSearch, Export(false)]
        public DateTime TaxFormSubmissionDate
        {
            get { return m_TaxFormSubmissionDate; }
            set { m_TaxFormSubmissionDate = value; modify("TaxFormSubmissionDate"); }
        }

        protected int m_TaxFormBatchNo;
        [DBField("TaxFormBatchNo"), TextSearch, Export(false)]
        public int TaxFormBatchNo
        {
            get { return m_TaxFormBatchNo; }
            set { m_TaxFormBatchNo = value; modify("TaxFormBatchNo"); }
        }

        protected string m_TaxFormEmployerName;
        [DBField("TaxFormEmployerName"), TextSearch, Export(false)]
        public string TaxFormEmployerName
        {
            get { return m_TaxFormEmployerName; }
            set { m_TaxFormEmployerName = value; modify("TaxFormEmployerName"); }
        }

        protected string m_TaxFormEmployerAddress;
        [DBField("TaxFormEmployerAddress"), TextSearch, Export(false)]
        public string TaxFormEmployerAddress
        {
            get { return m_TaxFormEmployerAddress; }
            set { m_TaxFormEmployerAddress = value; modify("TaxFormEmployerAddress"); }
        }

        protected string m_TaxFormDesignation;
        [DBField("TaxFormDesignation"), TextSearch, Export(false)]
        public string TaxFormDesignation
        {
            get { return m_TaxFormDesignation; }
            set { m_TaxFormDesignation = value; modify("TaxFormDesignation"); }
        }

        protected string m_TaxFormType;
        [DBField("TaxFormType"), TextSearch, Export(false)]
        public string TaxFormType
        {
            get { return m_TaxFormType; }
            set { m_TaxFormType = value; modify("TaxFormType"); }
        }
    }
}