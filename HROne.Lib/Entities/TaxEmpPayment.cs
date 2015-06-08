using System;
using System.Data;
using System.Configuration;

using HROne.DataAccess;
////using perspectivemind.validation;


namespace HROne.Lib.Entities
{
    [DBClass("TaxEmpPayment")]

    public class ETaxEmpPayment : BaseObject
    {
        public static DBManager db = new DBManager(typeof(ETaxEmpPayment));

        protected int m_TaxEmpPayID;
        [DBField("TaxEmpPayID", true, true), TextSearch, Export(false)]
        public int TaxEmpPayID
        {
            get { return m_TaxEmpPayID; }
            set { m_TaxEmpPayID = value; modify("TaxEmpPayID"); }
            
        }

        protected int m_TaxEmpID;
        [DBField("TaxEmpID"), TextSearch, Export(false), Required]
        public int TaxEmpID
        {
            get { return m_TaxEmpID; }
            set { m_TaxEmpID = value; modify("TaxEmpID"); }
        }

        protected int m_TaxPayID;
        [DBField("TaxPayID"), TextSearch, Export(false), Required]
        public int TaxPayID
        {
            get { return m_TaxPayID; }
            set { m_TaxPayID = value; modify("TaxPayID"); }
        }

        protected string m_TaxEmpPayNature;
        [DBField("TaxEmpPayNature"), TextSearch, Export(false)]
        public string TaxEmpPayNature
        {
            get { return m_TaxEmpPayNature; }
            set { m_TaxEmpPayNature = value; modify("TaxEmpPayNature"); }
        }
        protected DateTime m_TaxEmpPayPeriodFr;
        [DBField("TaxEmpPayPeriodFr"), TextSearch, Export(false), Required]
        public DateTime TaxEmpPayPeriodFr
        {
            get { return m_TaxEmpPayPeriodFr; }
            set { m_TaxEmpPayPeriodFr = value; modify("TaxEmpPayPeriodFr"); }
        }
        protected DateTime m_TaxEmpPayPeriodTo;
        [DBField("TaxEmpPayPeriodTo"), TextSearch, Export(false), Required]
        public DateTime TaxEmpPayPeriodTo
        {
            get { return m_TaxEmpPayPeriodTo; }
            set { m_TaxEmpPayPeriodTo = value; modify("TaxEmpPayPeriodTo"); }
        }

        protected long m_TaxEmpPayAmount;
        [DBField("TaxEmpPayAmount", "0"), Int, IntRange(0, 999999999), TextSearch, MaxLength(9), Export(false)]
        public long TaxEmpPayAmount
        {
            get { return m_TaxEmpPayAmount; }
            set { m_TaxEmpPayAmount = value; modify("TaxEmpPayAmount"); }
        }

        protected ETaxEmp m_taxEmp;
        public ETaxEmp RelatedTaxEmp
        {
            get { return m_taxEmp; }
            set { m_taxEmp = value; }
        }
      
    }
}