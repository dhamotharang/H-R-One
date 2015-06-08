using System;
using System.Data;
using System.Configuration;
using HROne.DataAccess;
////using perspectivemind.validation;
using HROne.Common;

namespace HROne.Lib.Entities
{
    [DBClass("TaxCompany")]
    /// <summary>
    /// Summary description for TaxCompany
    /// </summary>
    public class ETaxCompany:BaseObject 
    {
        public static DBManager db = new DBManager(typeof(ETaxCompany));
        public static WFValueList VLTaxCompany = new AppUtils.EncryptedDBCodeList(ETaxCompany.db, "TaxCompID", new string[] { "TaxCompEmployerName" }, "-", "TaxCompEmployerName");

        protected int m_TaxCompID;
        [DBField("TaxCompID", true, true), TextSearch, Export(false)]
        public int TaxCompID
        {
            get { return m_TaxCompID; }
            set { m_TaxCompID = value; modify("TaxCompID"); }
        }

        protected string m_TaxCompEmployerName;
        [DBField("TaxCompEmployerName"), DBAESEncryptStringField, TextSearch, Export(false), MaxLength(70), Required]
        public string TaxCompEmployerName
        {
            get { return m_TaxCompEmployerName; }
            set { m_TaxCompEmployerName = value; modify("TaxCompEmployerName"); }
        }

        protected string m_TaxCompEmployerAddress;
        [DBField("TaxCompEmployerAddress"), DBAESEncryptStringField, TextSearch, Export(false), MaxLength(90), Required]
        public string TaxCompEmployerAddress
        {
            get { return m_TaxCompEmployerAddress; }
            set { m_TaxCompEmployerAddress = value; modify("TaxCompEmployerAddress"); }
        }

        protected string m_TaxCompSection;
        [DBField("TaxCompSection"), DBAESEncryptStringField, TextSearch, Export(false), MaxLength(3), Required]
        public string TaxCompSection
        {
            get { return m_TaxCompSection; }
            set { m_TaxCompSection = value; modify("TaxCompSection"); }
        }

        protected string m_TaxCompERN;
        [DBField("TaxCompERN"), DBAESEncryptStringField, TextSearch, Export(false), MaxLength(8), Required]
        public string TaxCompERN
        {
            get { return m_TaxCompERN; }
            set { m_TaxCompERN = value; modify("TaxCompERN"); }
        }


        protected string m_TaxCompDesignation;
        [DBField("TaxCompDesignation"), DBAESEncryptStringField, TextSearch, Export(false), MaxLength(25), Required]
        public string TaxCompDesignation
        {
            get { return m_TaxCompDesignation; }
            set { m_TaxCompDesignation = value; modify("TaxCompDesignation"); }
        }
    }
}