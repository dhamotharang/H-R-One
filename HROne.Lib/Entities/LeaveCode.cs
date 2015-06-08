using System;
using System.Collections.Generic;
using System.Text;
using HROne.DataAccess;
////using perspectivemind.validation;

namespace HROne.Lib.Entities
{
    [DBClass("LeaveCode")]
    public class ELeaveCode : BaseObject
    {
        public static DBManager db = new DBManager(typeof(ELeaveCode));
        public static WFValueList VLLeaveCode = new AppUtils.EncryptedDBCodeList(ELeaveCode.db, "LeaveCodeID", new string[] { "LeaveCode", "LeaveCodeDesc" }, " - ", "LeaveCode");//new WFDBList(ELeaveCode.db, "LeaveCodeID", "LeaveCodeDesc", "LeaveCodeDesc");

        public static ELeaveCode GetObject(DatabaseConnection dbConn, int ID)
        {
            ELeaveCode obj = new ELeaveCode();
            obj.LeaveCodeID = ID;

            if (ELeaveCode.db.select(dbConn, obj))
                return obj;

            return null; 
        }

        protected int m_LeaveCodeID;
        [DBField("LeaveCodeID", true, true), TextSearch, Export(false)]
        public int LeaveCodeID
        {
            get { return m_LeaveCodeID; }
            set { m_LeaveCodeID = value; modify("LeaveCodeID"); }
        }
        protected int m_LeaveTypeID;
        [DBField("LeaveTypeID"), TextSearch, Export(false),Required]
        public int LeaveTypeID
        {
            get { return m_LeaveTypeID; }
            set { m_LeaveTypeID = value; modify("LeaveTypeID"); }
        }
        protected string m_LeaveCode;
        [DBField("LeaveCode"), TextSearch, MaxLength(20), Export(false), Required]
        public string LeaveCode
        {
            get { return m_LeaveCode; }
            set { m_LeaveCode = value; modify("LeaveCode"); }
        }
        protected string m_LeaveCodeDesc;
        [DBField("LeaveCodeDesc"), TextSearch,MaxLength(100, 40), Export(false), Required]
        public string LeaveCodeDesc
        {
            get { return m_LeaveCodeDesc; }
            set { m_LeaveCodeDesc = value; modify("LeaveCodeDesc"); }
        }
        protected double m_LeaveCodePayRatio;
        [DBField("LeaveCodePayRatio", "0.00"), TextSearch, MaxLength(6), Export(false), Required]
        public double LeaveCodePayRatio
        {
            get { return m_LeaveCodePayRatio; }
            set { m_LeaveCodePayRatio = value; modify("LeaveCodePayRatio"); }
        }
        protected int m_LeaveCodeLeaveAllowPaymentCodeID;
        [DBField("LeaveCodeLeaveAllowPaymentCodeID"), TextSearch, Export(false)]
        public int LeaveCodeLeaveAllowPaymentCodeID
        {
            get { return m_LeaveCodeLeaveAllowPaymentCodeID; }
            set { m_LeaveCodeLeaveAllowPaymentCodeID = value; modify("LeaveCodeLeaveAllowPaymentCodeID"); }
        }
        protected int m_LeaveCodeLeaveAllowFormula;
        [DBField("LeaveCodeLeaveAllowFormula"), TextSearch, Export(false), Required]
        public int LeaveCodeLeaveAllowFormula
        {
            get { return m_LeaveCodeLeaveAllowFormula; }
            set { m_LeaveCodeLeaveAllowFormula = value; modify("LeaveCodeLeaveAllowFormula"); }

        }
        protected int m_LeaveCodeLeaveDeductPaymentCodeID;
        [DBField("LeaveCodeLeaveDeductPaymentCodeID"), TextSearch, Export(false)]
        public int LeaveCodeLeaveDeductPaymentCodeID
        {
            get { return m_LeaveCodeLeaveDeductPaymentCodeID; }
            set { m_LeaveCodeLeaveDeductPaymentCodeID = value; modify("LeaveCodeLeaveDeductPaymentCodeID"); }
        }
        protected int m_LeaveCodeLeaveDeductFormula;
        [DBField("LeaveCodeLeaveDeductFormula"), TextSearch, Export(false), Required]
        public int LeaveCodeLeaveDeductFormula
        {
            get { return m_LeaveCodeLeaveDeductFormula; }
            set { m_LeaveCodeLeaveDeductFormula = value; modify("LeaveCodeLeaveDeductFormula"); }
        }

        protected bool m_LeaveCodePayAdvance;
        [DBField("LeaveCodePayAdvance"), TextSearch, Export(false)]
        public bool LeaveCodePayAdvance
        {
            get { return m_LeaveCodePayAdvance; }
            set { m_LeaveCodePayAdvance = value; modify("LeaveCodePayAdvance"); }
        }

        protected bool m_LeaveCodeHideInESS;
        [DBField("LeaveCodeHideInESS"), TextSearch, Export(false)]
        public bool LeaveCodeHideInESS
        {
            get { return m_LeaveCodeHideInESS; }
            set { m_LeaveCodeHideInESS = value; modify("LeaveCodeHideInESS"); }
        }


        protected bool m_LeaveCodeIsSkipPayrollProcess;
        [DBField("LeaveCodeIsSkipPayrollProcess"), TextSearch, Export(false)]
        public bool LeaveCodeIsSkipPayrollProcess
        {
            get { return m_LeaveCodeIsSkipPayrollProcess; }
            set { m_LeaveCodeIsSkipPayrollProcess = value; modify("LeaveCodeIsSkipPayrollProcess"); }
        }
        protected bool m_LeaveCodeIsPayrollProcessNextMonth;
        [DBField("LeaveCodeIsPayrollProcessNextMonth"), TextSearch, Export(false)]
        public bool LeaveCodeIsPayrollProcessNextMonth
        {
            get { return m_LeaveCodeIsPayrollProcessNextMonth; }
            set { m_LeaveCodeIsPayrollProcessNextMonth = value; modify("LeaveCodeIsPayrollProcessNextMonth"); }
        }

        protected bool m_LeaveCodeUseAllowancePaymentCodeIfSameAmount;
        [DBField("LeaveCodeUseAllowancePaymentCodeIfSameAmount"), TextSearch, Export(false)]
        public bool LeaveCodeUseAllowancePaymentCodeIfSameAmount
        {
            get { return m_LeaveCodeUseAllowancePaymentCodeIfSameAmount; }
            set { m_LeaveCodeUseAllowancePaymentCodeIfSameAmount = value; modify("LeaveCodeUseAllowancePaymentCodeIfSameAmount"); }
        }

        protected bool m_LeaveCodeIsCNDProrata;
        [DBField("LeaveCodeIsCNDProrata"), TextSearch, Export(false)]
        public bool LeaveCodeIsCNDProrata
        {
            get { return m_LeaveCodeIsCNDProrata; }
            set { m_LeaveCodeIsCNDProrata = value; modify("LeaveCodeIsCNDProrata"); }
        }

        protected bool m_LeaveCodeIsShowMedicalCertOption;
        [DBField("LeaveCodeIsShowMedicalCertOption"), TextSearch, Export(false)]
        public bool LeaveCodeIsShowMedicalCertOption
        {
            get { return m_LeaveCodeIsShowMedicalCertOption; }
            set { m_LeaveCodeIsShowMedicalCertOption = value; modify("LeaveCodeIsShowMedicalCertOption"); }
        }

        protected string m_LeaveAppUnit;
        [DBField("LeaveAppUnit"), TextSearch, MaxLength(100, 40), Export(false), Required]
        public string LeaveAppUnit
        {
            get { return m_LeaveAppUnit; }
            set { m_LeaveAppUnit = value; modify("LeaveAppUnit"); }
        }

    }
}
