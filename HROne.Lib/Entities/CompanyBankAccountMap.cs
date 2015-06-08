using System;
using System.Collections.Generic;
using System.Text;
using HROne.DataAccess;
////using perspectivemind.validation;
using HROne.Common;
namespace HROne.Lib.Entities
{

    [DBClass("CompanyBankAccountMap")]
    public class ECompanyBankAccountMap : BaseObject
    {
        public static DBManager db = new DBManager(typeof(ECompanyBankAccountMap));

        protected int m_CompanyBankAccountMapID;
        [DBField("CompanyBankAccountMapID", true, true), TextSearch, Export(false)]
        public int CompanyBankAccountMapID
        {
            get { return m_CompanyBankAccountMapID; }
            set { m_CompanyBankAccountMapID = value; modify("CompanyBankAccountMapID"); }
        }
        protected int m_CompanyID;
        [DBField("CompanyID"), Required, Export(false)]
        public int CompanyID
        {
            get { return m_CompanyID; }
            set { m_CompanyID = value; modify("CompanyID"); }
        }
        protected int m_CompanyBankAccountID;
        [DBField("CompanyBankAccountID"), Required, Export(false)]
        public int CompanyBankAccountID
        {
            get { return m_CompanyBankAccountID; }
            set { m_CompanyBankAccountID = value; modify("CompanyBankAccountID"); }
        }
    }
}
