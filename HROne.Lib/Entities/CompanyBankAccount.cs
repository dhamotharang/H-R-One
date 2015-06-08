using System;
using System.Collections.Generic;
using System.Text;
using HROne.DataAccess;
////using perspectivemind.validation;
using HROne.Common;
namespace HROne.Lib.Entities
{

    [DBClass("CompanyBankAccount")]
    public class ECompanyBankAccount : BaseObject
    {
        public static DBManager db = new DBManager(typeof(ECompanyBankAccount));
        public static WFValueList VLBankAccount = new AppUtils.BankAccountValueList(db, "CompanyBankAccountID", "CompanyBankAccountBankCode", "CompanyBankAccountBranchCode", "CompanyBankAccountAccountNo", "CompanyBankAccountHolderName", "CompanyBankAccountID");

        protected int m_CompanyBankAccountID;
        [DBField("CompanyBankAccountID", true, true), TextSearch, Export(false)]
        public int CompanyBankAccountID
        {
            get { return m_CompanyBankAccountID; }
            set { m_CompanyBankAccountID = value; modify("CompanyBankAccountID"); }
        }
        protected string m_CompanyBankAccountBankCode;
        [DBField("CompanyBankAccountBankCode"), TextSearch, Int, DBAESEncryptStringField, MaxLength(3, 3), Export(false), Required]
        public string CompanyBankAccountBankCode
        {
            get { return m_CompanyBankAccountBankCode; }
            set { m_CompanyBankAccountBankCode = value; modify("CompanyBankAccountBankCode"); }
        }
        protected string m_CompanyBankAccountBranchCode;
        [DBField("CompanyBankAccountBranchCode"), TextSearch, Int, DBAESEncryptStringField, MaxLength(3, 3), Export(false), Required]
        public string CompanyBankAccountBranchCode
        {
            get { return m_CompanyBankAccountBranchCode; }
            set { m_CompanyBankAccountBranchCode = value; modify("CompanyBankAccountBranchCode"); }
        }
        protected string m_CompanyBankAccountAccountNo;
        [DBField("CompanyBankAccountAccountNo"), TextSearch, DBAESEncryptStringField, MaxLength(20, 20), Export(false), Required]
        public string CompanyBankAccountAccountNo
        {
            get { return m_CompanyBankAccountAccountNo; }
            set { m_CompanyBankAccountAccountNo = value; modify("CompanyBankAccountAccountNo"); }
        }
        protected string m_CompanyBankAccountHolderName;
        [DBField("CompanyBankAccountHolderName"), TextSearch, DBAESEncryptStringField, MaxLength(100, 40), Export(false), Required]
        public string CompanyBankAccountHolderName
        {
            get { return m_CompanyBankAccountHolderName; }
            set { m_CompanyBankAccountHolderName = value; modify("CompanyBankAccountHolderName"); }
        }

    }
}
