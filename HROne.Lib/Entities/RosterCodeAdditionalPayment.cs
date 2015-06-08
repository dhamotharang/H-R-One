using System;
using System.Collections.Generic;
using System.Text;
using HROne.DataAccess;
////using perspectivemind.validation;

namespace HROne.Lib.Entities
{
    [DBClass("RosterCodeAdditionalPayment")]
    public class ERosterCodeAdditionalPayment : BaseObject
    {
        public static DBManager db = new DBManager(typeof(ERosterCodeAdditionalPayment));

        protected int m_RosterCodeAdditionalPaymentID;
        [DBField("RosterCodeAdditionalPaymentID", true, true), TextSearch, Export(false)]
        public int RosterCodeAdditionalPaymentID
        {
            get { return m_RosterCodeAdditionalPaymentID; }
            set { m_RosterCodeAdditionalPaymentID = value; modify("RosterCodeAdditionalPaymentID"); }
        }
        protected int m_RosterCodeID;
        [DBField("RosterCodeID"), TextSearch, Export(false)]
        public int RosterCodeID
        {
            get { return m_RosterCodeID; }
            set { m_RosterCodeID = value; modify("RosterCodeID"); }
        }
        protected int m_PaymentCodeID;
        [DBField("PaymentCodeID"), TextSearch, Export(false), Required]
        public int PaymentCodeID
        {
            get { return m_PaymentCodeID; }
            set { m_PaymentCodeID = value; modify("PaymentCodeID"); }
        }
        protected double m_RosterCodeAdditionalPaymentAmount;
        [DBField("RosterCodeAdditionalPaymentAmount", "0.00"), MaxLength(10), TextSearch, Export(false), Required]
        public double RosterCodeAdditionalPaymentAmount
        {
            get { return m_RosterCodeAdditionalPaymentAmount; }
            set { m_RosterCodeAdditionalPaymentAmount = value; modify("RosterCodeAdditionalPaymentAmount"); }
        }
    }
}
