using System;
using System.Data;
using System.Configuration;
using HROne.DataAccess;
////using perspectivemind.validation;

namespace HROne.Lib.Entities
{
    /// <summary>
    /// Summary description for PayrollProrataFormula
    /// </summary>
    /// 
    [DBClass("PayrollProrataFormula")]

    public class EPayrollProrataFormula :BaseObject 
    {
        public const string DEFAULT_FOEMULA_CODE = "<DEFAULT>";

        public static DBManager db = new DBManager(typeof(EPayrollProrataFormula));
        public static WFValueList VLPayrollProrataFormula = new WFDBList(EPayrollProrataFormula.db, "PayFormID", "PayFormCode + ' - ' +PayFormDesc", "PayFormCode");

        protected int m_PayFormID;
        [DBField("PayFormID", true, true), TextSearch, Export(false)]
        public int PayFormID
        {
            get { return m_PayFormID; }
            set { m_PayFormID = value; modify("PayFormID"); }
        }

        protected string m_PayFormCode;
        [DBField("PayFormCode"), MaxLength(20), TextSearch, Export(false), Required]
        public string PayFormCode
        {
            get { return m_PayFormCode; }
            set { m_PayFormCode = value; modify("PayFormCode"); }
        }

        protected string m_PayFormDesc;
        [DBField("PayFormDesc"), MaxLength(100,40), TextSearch, Export(false), Required]
        public string PayFormDesc
        {
            get { return m_PayFormDesc; }
            set { m_PayFormDesc = value; modify("PayFormDesc"); }
        }

        protected int m_PayFormPaymentType;
        [DBField("PayFormPaymentType"), TextSearch, Export(false)]
        public int PayFormPaymentType
        {
            get { return m_PayFormPaymentType; }
            set { m_PayFormPaymentType = value; modify("PayFormPaymentType"); }
        }

        protected double m_PayFormMultiplier;
        [DBField("PayFormMultiplier", "0.0000"), TextSearch, Export(false)]
        public double PayFormMultiplier
        {
            get { return m_PayFormMultiplier; }
            set { m_PayFormMultiplier = value; modify("PayFormMultiplier"); }
        }

        protected double m_PayFormDivider;
        [DBField("PayFormDivider", "0.0000"), TextSearch, Export(false)]
        public double PayFormDivider
        {
            get { return m_PayFormDivider; }
            set { m_PayFormDivider = value; modify("PayFormDivider"); }
        }

        protected string m_PayFormIsSys;
        [DBField("PayFormIsSys"), TextSearch, Export(false)]
        public string PayFormIsSys
        {
            get { return m_PayFormIsSys; }
            set { m_PayFormIsSys = value; modify("PayFormIsSys"); }
        }

        protected int m_ReferencePayFormID;
        [DBField("ReferencePayFormID"), TextSearch, Export(false)]
        public int ReferencePayFormID
        {
            get { return m_ReferencePayFormID; }
            set { m_ReferencePayFormID = value; modify("ReferencePayFormID"); }
        }

        protected int m_PayFormDecimalPlace;
        [DBField("PayFormDecimalPlace"), TextSearch, MaxLength(1), Export(false), Required]
        public int PayFormDecimalPlace
        {
            get { return m_PayFormDecimalPlace; }
            set { m_PayFormDecimalPlace = value; modify("PayFormDecimalPlace"); }
        }

        protected string m_PayFormRoundingRule;
        [DBField("PayFormRoundingRule"), TextSearch, MaxLength(50, 25), Export(false), Required]
        public string PayFormRoundingRule
        {
            get { return m_PayFormRoundingRule; }
            set { m_PayFormRoundingRule = value; modify("PayFormRoundingRule"); }
        }

        public bool IsDAWFormula(DatabaseConnection dbConn)
        {
            if (m_PayFormCode.Equals("DAW"))
                return true;
            else if (m_PayFormIsSys.Equals("Y",StringComparison.CurrentCultureIgnoreCase))
                return false;
            else if (m_ReferencePayFormID <= 0)
                return false;
            else
            {
                EPayrollProrataFormula prorataFormula = new EPayrollProrataFormula();
                prorataFormula.PayFormID = m_ReferencePayFormID;
                if (EPayrollProrataFormula.db.select(dbConn, prorataFormula))
                    return prorataFormula.IsDAWFormula(dbConn);
                else
                    return false;
            }
        }
    }
}