using System;
using System.Data;
using System.Configuration;
using HROne.DataAccess;
////using perspectivemind.validation;
using HROne.Common;


namespace HROne.Lib.Entities
{
    /// <summary>
    /// Summary description for TaxEmpPlaceOfResidence
    /// </summary>
    /// 
    [DBClass("TaxEmpPlaceOfResidence")]

    public class ETaxEmpPlaceOfResidence : BaseObject 
    {
        public static DBManager db = new DBManager(typeof(ETaxEmpPlaceOfResidence));

        protected int m_TaxEmpPlaceID;
        [DBField("TaxEmpPlaceID", true, true), TextSearch, Export(false)]
        public int TaxEmpPlaceID
        {
            get { return m_TaxEmpPlaceID; }
            set { m_TaxEmpPlaceID = value; modify("TaxEmpPlaceID"); }
        }

        protected int m_TaxEmpID;
        [DBField("TaxEmpID"), TextSearch, Export(false), Required]
        public int TaxEmpID
        {
            get { return m_TaxEmpID; }
            set { m_TaxEmpID = value; modify("TaxEmpID"); }
        }

        protected string m_TaxEmpPlaceAddress;
        [DBField("TaxEmpPlaceAddress"), DBAESEncryptStringField, TextSearch, MaxLength(110, 55), Export(false)]
        public string TaxEmpPlaceAddress
        {
            get { return m_TaxEmpPlaceAddress; }
            set { m_TaxEmpPlaceAddress = value; modify("TaxEmpPlaceAddress"); }
        }
        protected string m_TaxEmpPlaceNature;
        [DBField("TaxEmpPlaceNature"), DBAESEncryptStringField, TextSearch, MaxLength(19), Export(false)]
        public string TaxEmpPlaceNature
        {
            get { return m_TaxEmpPlaceNature; }
            set { m_TaxEmpPlaceNature = value; modify("TaxEmpPlaceNature"); }
        }
        protected DateTime m_TaxEmpPlacePeriodFr;
        [DBField("TaxEmpPlacePeriodFr"), TextSearch, Export(false), Required]
        public DateTime TaxEmpPlacePeriodFr
        {
            get { return m_TaxEmpPlacePeriodFr; }
            set { m_TaxEmpPlacePeriodFr = value; modify("TaxEmpPlacePeriodFr"); }
        }
        protected DateTime m_TaxEmpPlacePeriodTo;
        [DBField("TaxEmpPlacePeriodTo"), TextSearch, Export(false), Required]
        public DateTime TaxEmpPlacePeriodTo
        {
            get { return m_TaxEmpPlacePeriodTo; }
            set { m_TaxEmpPlacePeriodTo = value; modify("TaxEmpPlacePeriodTo"); }
        }
        protected int m_TaxEmpPlaceERRent;
        [DBField("TaxEmpPlaceERRent", "0"), Int, IntRange(0, 9999999), MaxLength(7), TextSearch, Export(false)]
        public int TaxEmpPlaceERRent
        {
            get { return m_TaxEmpPlaceERRent; }
            set { m_TaxEmpPlaceERRent = value; modify("TaxEmpPlaceERRent"); }
        }
        protected int m_TaxEmpPlaceEERent;
        [DBField("TaxEmpPlaceEERent", "0"), Int, IntRange(0, 9999999), MaxLength(7), TextSearch, Export(false)]
        public int TaxEmpPlaceEERent
        {
            get { return m_TaxEmpPlaceEERent; }
            set { m_TaxEmpPlaceEERent = value; modify("TaxEmpPlaceEERent"); }
        }
        protected int m_TaxEmpPlaceEERentRefunded;
        [DBField("TaxEmpPlaceEERentRefunded", "0"), Int, IntRange(0, 9999999), MaxLength(7), TextSearch, Export(false)]
        public int TaxEmpPlaceEERentRefunded
        {
            get { return m_TaxEmpPlaceEERentRefunded; }
            set { m_TaxEmpPlaceEERentRefunded = value; modify("TaxEmpPlaceEERentRefunded"); }
        }
        protected int m_TaxEmpPlaceERRentByEE;
        [DBField("TaxEmpPlaceERRentByEE", "0"), Int, IntRange(0, 9999999), MaxLength(7), TextSearch, Export(false)]
        public int TaxEmpPlaceERRentByEE
        {
            get { return m_TaxEmpPlaceERRentByEE; }
            set { m_TaxEmpPlaceERRentByEE = value; modify("TaxEmpPlaceERRentByEE"); }
        }

        private ETaxEmp m_taxEmp;
        public ETaxEmp RelatedTaxEmp
        {
            get { return m_taxEmp; }
            set { m_taxEmp = value; }
        }
    }
}