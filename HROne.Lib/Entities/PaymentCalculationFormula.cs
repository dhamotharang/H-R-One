using System;
using System.Collections.Generic;
using System.Text;
using HROne.DataAccess;
using HROne.Common;
namespace HROne.Lib.Entities
{
    [DBClass("PaymentCalculationFormula")]
    public class EPaymentCalculationFormula : BaseObject
    {
        public static DBManager db = new DBManager(typeof(EPaymentCalculationFormula));
        public static WFValueList VLShiftDutyCode = new WFDBCodeList(db, "PayCalFormulaID", "PayCalFormulaCode", "PayCalFormulaCodeDesc", "PayCalFormulaCode");

        public static EPaymentCalculationFormula GetObject(DatabaseConnection dbConn, object ID)
        {
            if (ID is int)
            {
                EPaymentCalculationFormula obj = new EPaymentCalculationFormula();
                obj.PayCalFormulaID = (int)ID;
                if (db.select(dbConn, obj))
                    return obj;
            }
            return null;
        }

        protected int m_PayCalFormulaID;
        [DBField("PayCalFormulaID", true, true), TextSearch, Export(false)]
        public int PayCalFormulaID
        {
            get { return m_PayCalFormulaID; }
            set { m_PayCalFormulaID = value; modify("PayCalFormulaID"); }
        }
        protected string m_PayCalFormulaCode;
        [DBField("PayCalFormulaCode"), TextSearch, MaxLength(20), Export(false), Required]
        public string PayCalFormulaCode
        {
            get { return m_PayCalFormulaCode; }
            set { m_PayCalFormulaCode = value; modify("PayCalFormulaCode"); }
        }
        protected string m_PayCalFormulaCodeDesc;
        [DBField("PayCalFormulaCodeDesc"), TextSearch, MaxLength(100), Export(false)]
        public string PayCalFormulaCodeDesc
        {
            get { return m_PayCalFormulaCodeDesc; }
            set { m_PayCalFormulaCodeDesc = value; modify("PayCalFormulaCodeDesc"); }
        }
    }
}
