using System;
using System.Collections.Generic;
using System.Text;
using HROne.DataAccess;
////using perspectivemind.validation;

namespace HROne.Lib.Entities
{
    [DBClass("AttendancePlanAdditionalPayment")]
    public class EAttendancePlanAdditionalPayment : BaseObject
    {
        public static DBManager db = new DBManager(typeof(EAttendancePlanAdditionalPayment));

        protected int m_AttendancePlanAdditionalPaymentID;
        [DBField("AttendancePlanAdditionalPaymentID", true, true), TextSearch, Export(false)]
        public int AttendancePlanAdditionalPaymentID
        {
            get { return m_AttendancePlanAdditionalPaymentID; }
            set { m_AttendancePlanAdditionalPaymentID = value; modify("AttendancePlanAdditionalPaymentID"); }
        }
        protected int m_AttendancePlanID;
        [DBField("AttendancePlanID"), TextSearch, Export(false)]
        public int AttendancePlanID
        {
            get { return m_AttendancePlanID; }
            set { m_AttendancePlanID = value; modify("AttendancePlanID"); }
        }
        protected int m_PaymentCodeID;
        [DBField("PaymentCodeID"), TextSearch, Export(false), Required]
        public int PaymentCodeID
        {
            get { return m_PaymentCodeID; }
            set { m_PaymentCodeID = value; modify("PaymentCodeID"); }
        }
        protected double m_AttendancePlanAdditionalPaymentAmount;
        [DBField("AttendancePlanAdditionalPaymentAmount", "0.00"), MaxLength(10), TextSearch, Export(false), Required]
        public double AttendancePlanAdditionalPaymentAmount
        {
            get { return m_AttendancePlanAdditionalPaymentAmount; }
            set { m_AttendancePlanAdditionalPaymentAmount = value; modify("AttendancePlanAdditionalPaymentAmount"); }
        }

        protected int m_AttendancePlanAdditionalPaymentMaxLateMins;
        [DBField("AttendancePlanAdditionalPaymentMaxLateMins"), MaxLength(3), TextSearch, Export(false)]
        public int AttendancePlanAdditionalPaymentMaxLateMins
        {
            get { return m_AttendancePlanAdditionalPaymentMaxLateMins; }
            set { m_AttendancePlanAdditionalPaymentMaxLateMins = value; modify("AttendancePlanAdditionalPaymentMaxLateMins"); }
        }

        protected int m_AttendancePlanAdditionalPaymentMaxEarlyLeaveMins;
        [DBField("AttendancePlanAdditionalPaymentMaxEarlyLeaveMins"), MaxLength(3), TextSearch, Export(false)]
        public int AttendancePlanAdditionalPaymentMaxEarlyLeaveMins
        {
            get { return m_AttendancePlanAdditionalPaymentMaxEarlyLeaveMins; }
            set { m_AttendancePlanAdditionalPaymentMaxEarlyLeaveMins = value; modify("AttendancePlanAdditionalPaymentMaxEarlyLeaveMins"); }
        }

        protected int m_AttendancePlanAdditionalPaymentMinOvertimeMins;
        [DBField("AttendancePlanAdditionalPaymentMinOvertimeMins"), MaxLength(3), TextSearch, Export(false)]
        public int AttendancePlanAdditionalPaymentMinOvertimeMins
        {
            get { return m_AttendancePlanAdditionalPaymentMinOvertimeMins; }
            set { m_AttendancePlanAdditionalPaymentMinOvertimeMins = value; modify("AttendancePlanAdditionalPaymentMinOvertimeMins"); }
        }

        protected DateTime m_AttendancePlanAdditionalPaymentRosterAcrossTime;
        [DBField("AttendancePlanAdditionalPaymentRosterAcrossTime", "HH:mm"), TextSearch, MaxLength(5), Export(false)]
        public DateTime AttendancePlanAdditionalPaymentRosterAcrossTime
        {
            get { return m_AttendancePlanAdditionalPaymentRosterAcrossTime; }
            set { m_AttendancePlanAdditionalPaymentRosterAcrossTime = value; modify("AttendancePlanAdditionalPaymentRosterAcrossTime"); }
        }
    }
}
