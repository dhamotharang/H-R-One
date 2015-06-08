using System;
using System.Data;
using System.Configuration;

using HROne.DataAccess;
////using perspectivemind.validation;

namespace HROne.Lib.Entities
{
    [DBClass("TaxPayment")]
    public class ETaxPayment : BaseObject 
    {
        public static DBManager db = new DBManager(typeof(ETaxPayment));
        public static WFValueList VLTaxPaymentWithNature = new WFDBCodeList(ETaxPayment.db, "TaxPayID", "TaxPayCode + ' ' + TaxPayDesc","TaxPayNature", "TaxPayCode");
        public static WFValueList VLTaxPayment = new WFDBCodeList(ETaxPayment.db, "TaxPayID", "TaxPayCode", "TaxPayDesc", "TaxPayCode");

        protected int m_TaxPayID;
        [DBField("TaxPayID", true, true), TextSearch, Export(false)]
        public int TaxPayID
        {
            get { return m_TaxPayID; }
            set { m_TaxPayID = value; modify("TaxPayID"); }
        }

        protected string m_TaxFormType;
        [DBField("TaxFormType"), TextSearch, Export(false), Required]
        public string TaxFormType
        {
            get { return m_TaxFormType; }
            set { m_TaxFormType = value; modify("TaxFormType"); }
        }

        protected string m_TaxPayCode;
        [DBField("TaxPayCode"), TextSearch, Export(false), Required]
        public string TaxPayCode
        {
            get { return m_TaxPayCode; }
            set { m_TaxPayCode = value; modify("TaxPayCode"); }
        }

        protected string m_TaxPayDesc;
        [DBField("TaxPayDesc"), TextSearch, Export(false), Required]
        public string TaxPayDesc
        {
            get { return m_TaxPayDesc; }
            set { m_TaxPayDesc = value; modify("TaxPayDesc"); }
        }

        protected string m_TaxPayNature;
        [DBField("TaxPayNature"), TextSearch, Export(false)]
        public string TaxPayNature
        {
            get { return m_TaxPayNature; }
            set { m_TaxPayNature = value; modify("TaxPayNature"); }
        }

        protected string m_TaxPayIsShowNature;
        [DBField("TaxPayIsShowNature"), TextSearch, Export(false)]
        public string TaxPayIsShowNature
        {
            get { return m_TaxPayIsShowNature; }
            set { m_TaxPayIsShowNature = value; modify("TaxPayIsShowNature"); }
        }
    }
}