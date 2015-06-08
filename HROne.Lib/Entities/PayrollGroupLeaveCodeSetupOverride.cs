using System;
using HROne.DataAccess;
using HROne.Common;

namespace HROne.Lib.Entities
{
    [DBClass("PayrollGroupLeaveCodeSetupOverride")]
    public class EPayrollGroupLeaveCodeSetupOverride : BaseObject
    {
        public static DBManager db = new DBManager(typeof(EPayrollGroupLeaveCodeSetupOverride));

        protected int m_PayrollGroupLeaveCodeSetupOverrideID;
        [DBField("PayrollGroupLeaveCodeSetupOverrideID", true, true), TextSearch, Export(false)]
        public int PayrollGroupLeaveCodeSetupOverrideID
        {
            get { return m_PayrollGroupLeaveCodeSetupOverrideID; }
            set { m_PayrollGroupLeaveCodeSetupOverrideID = value; modify("PayrollGroupLeaveCodeSetupOverrideID"); }
        }
        protected int m_PayGroupID;
        [DBField("PayGroupID"), Required, Export(false)]
        public int PayGroupID
        {
            get { return m_PayGroupID; }
            set { m_PayGroupID = value; modify("PayGroupID"); }
        }
        protected int m_LeaveCodeID;
        [DBField("LeaveCodeID"), Required, Export(false)]
        public int LeaveCodeID
        {
            get { return m_LeaveCodeID; }
            set { m_LeaveCodeID = value; modify("LeaveCodeID"); }
        }
        protected int m_PayrollGroupLeaveCodeSetupLeaveDeductFormula;
        [DBField("PayrollGroupLeaveCodeSetupLeaveDeductFormula"), TextSearch, Export(false)]
        public int PayrollGroupLeaveCodeSetupLeaveDeductFormula
        {
            get { return m_PayrollGroupLeaveCodeSetupLeaveDeductFormula; }
            set { m_PayrollGroupLeaveCodeSetupLeaveDeductFormula = value; modify("PayrollGroupLeaveCodeSetupLeaveDeductFormula"); }
        }
        protected int m_PayrollGroupLeaveCodeSetupLeaveDeductPaymentCodeID;
        [DBField("PayrollGroupLeaveCodeSetupLeaveDeductPaymentCodeID"), TextSearch, Export(false)]
        public int PayrollGroupLeaveCodeSetupLeaveDeductPaymentCodeID
        {
            get { return m_PayrollGroupLeaveCodeSetupLeaveDeductPaymentCodeID; }
            set { m_PayrollGroupLeaveCodeSetupLeaveDeductPaymentCodeID = value; modify("PayrollGroupLeaveCodeSetupLeaveDeductPaymentCodeID"); }
        }
        protected int m_PayrollGroupLeaveCodeSetupLeaveAllowFormula;
        [DBField("PayrollGroupLeaveCodeSetupLeaveAllowFormula"), TextSearch, Export(false)]
        public int PayrollGroupLeaveCodeSetupLeaveAllowFormula
        {
            get { return m_PayrollGroupLeaveCodeSetupLeaveAllowFormula; }
            set { m_PayrollGroupLeaveCodeSetupLeaveAllowFormula = value; modify("PayrollGroupLeaveCodeSetupLeaveAllowFormula"); }
        }

        protected int m_PayrollGroupLeaveCodeSetupLeaveAllowPaymentCodeID;
        [DBField("PayrollGroupLeaveCodeSetupLeaveAllowPaymentCodeID"), TextSearch, Export(false)]
        public int PayrollGroupLeaveCodeSetupLeaveAllowPaymentCodeID
        {
            get { return m_PayrollGroupLeaveCodeSetupLeaveAllowPaymentCodeID; }
            set { m_PayrollGroupLeaveCodeSetupLeaveAllowPaymentCodeID = value; modify("PayrollGroupLeaveCodeSetupLeaveAllowPaymentCodeID"); }
        }

    }
}
