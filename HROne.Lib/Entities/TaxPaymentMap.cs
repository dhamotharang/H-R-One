using System;
using System.Data;
using System.Configuration;
using HROne.DataAccess;
////using perspectivemind.validation;

namespace HROne.Lib.Entities
{
    /// <summary>
    /// Summary description for TaxPaymentMap
    /// </summary>
    [DBClass("TaxPaymentMap")]
    public class ETaxPaymentMap :BaseObject 
    {
        public static DBManager db = new DBManager(typeof(ETaxPaymentMap));

        protected int m_TaxPayMapID;
        [DBField("TaxPayMapID", true, true), TextSearch, Export(false)]
        public int TaxPayMapID
        {
            get { return m_TaxPayMapID; }
            set { m_TaxPayMapID = value; modify("TaxPayMapID"); }
        }

        protected int m_TaxPayID;
        [DBField("TaxPayID"), TextSearch, Export(false)]
        public int TaxPayID
        {
            get { return m_TaxPayID; }
            set { m_TaxPayID = value; modify("TaxPayID"); }
        }

        protected int m_PaymentCodeID;
        [DBField("PaymentCodeID"), TextSearch, Export(false)]
        public int PaymentCodeID
        {
            get { return m_PaymentCodeID; }
            set { m_PaymentCodeID = value; modify("PaymentCodeID"); }
        }
    }
}