using System;
using System.Collections;
using System.Text;
using HROne.DataAccess;
using HROne.Common;
////using perspectivemind.validation;

namespace HROne.SaaS.Entities
{

    [DBClass("HSBCBankPaymentCode")]
    public class EHSBCBankPaymentCode : DBObject
    {
        public static DBManager db = new DBManager(typeof(EHSBCBankPaymentCode));

        protected int m_HSBCBankPaymentCodeID;
        [DBField("HSBCBankPaymentCodeID", true, true), TextSearch, Export(false)]
        public int HSBCBankPaymentCodeID
        {
            get { return m_HSBCBankPaymentCodeID; }
            set { m_HSBCBankPaymentCodeID = value; modify("HSBCBankPaymentCodeID"); }
        }
        protected int m_CompanyDBID;
        [DBField("CompanyDBID"), Export(false), Required]
        public int CompanyDBID
        {
            get { return m_CompanyDBID; }
            set { m_CompanyDBID = value; modify("CompanyDBID"); }
        }
        protected int m_HSBCExchangeProfileID;
        [DBField("HSBCExchangeProfileID"), Export(false)]
        public int HSBCExchangeProfileID
        {
            get { return m_HSBCExchangeProfileID; }
            set { m_HSBCExchangeProfileID = value; modify("HSBCExchangeProfileID"); }
        }
        protected string m_HSBCBankPaymentCodeBankAccountNo;
        [DBField("HSBCBankPaymentCodeBankAccountNo"), Export(false), Required]
        public string HSBCBankPaymentCodeBankAccountNo
        {
            get { return m_HSBCBankPaymentCodeBankAccountNo; }
            set { m_HSBCBankPaymentCodeBankAccountNo = value; modify("HSBCBankPaymentCodeBankAccountNo"); }
        }
        protected string m_HSBCBankPaymentCode;
        [DBField("HSBCBankPaymentCode"), Export(false), Required]
        public string HSBCBankPaymentCode
        {
            get { return m_HSBCBankPaymentCode; }
            set { m_HSBCBankPaymentCode = value; modify("HSBCBankPaymentCode"); }
        }
        protected string m_HSBCBankPaymentCodeAutoPayInOutFlag;
        [DBField("HSBCBankPaymentCodeAutoPayInOutFlag"), Export(false), Required]
        public string HSBCBankPaymentCodeAutoPayInOutFlag
        {
            get { return m_HSBCBankPaymentCodeAutoPayInOutFlag; }
            set { m_HSBCBankPaymentCodeAutoPayInOutFlag = value; modify("HSBCBankPaymentCodeAutoPayInOutFlag"); }
        }
    }
}
