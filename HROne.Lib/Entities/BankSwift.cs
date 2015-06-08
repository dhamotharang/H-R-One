using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using HROne.DataAccess;
////using perspectivemind.validation;
using HROne.Common;
namespace HROne.Lib.Entities
{

    [DBClass("BankSwift")]
    public class EBankSwift : BaseObject
    {
        public static DBManager db = new DBManager(typeof(EBankSwift));

        public static EBankSwift GetObject(DatabaseConnection dbConn, int ID)
        {
            if (ID > 0)
            {
                EBankSwift obj = new EBankSwift();
                obj.BankSwiftID = (int)ID;
                if (db.select(dbConn, obj))
                    return obj;
            }
            return null;
        }

        public static EBankSwift GetObjectByBankCode(DatabaseConnection dbConn, string p_bankCode)
        {
            if (!string.IsNullOrEmpty(p_bankCode))
            {
                DBFilter m_filter = new DBFilter();
                m_filter.add(new Match("BankCode", p_bankCode));
                ArrayList m_list = EBankSwift.db.select(dbConn, m_filter);
                if (m_list.Count > 0)
                {
                    return (EBankSwift)m_list[0];
                }
            }
            return null;
        }

        public static EBankSwift GetObjectByBankCode(DatabaseConnection dbConn, string p_bankCode, string p_countryCode)
        {
            if (!string.IsNullOrEmpty(p_bankCode))
            {
                DBFilter m_filter = new DBFilter();
                m_filter.add(new Match("BankCode", p_bankCode));
                m_filter.add(new Match("CountryCode", p_countryCode));
                ArrayList m_list = EBankSwift.db.select(dbConn, m_filter);
                if (m_list.Count > 0)
                {
                    return (EBankSwift)m_list[0];
                }
            }
            return null;
        }

        protected int m_BankSwiftID;
        [DBField("BankSwiftID", true, true), TextSearch, Export(true)]
        public int BankSwiftID
        {
            get { return m_BankSwiftID; }
            set { m_BankSwiftID = value; modify("BankSwiftID"); }
        }

        protected string m_BankCode;
        [DBField("BankCode"), TextSearch, MaxLength(50), Export(true), Required]
        public string BankCode
        {
            get { return m_BankCode; }
            set { m_BankCode = value; modify("BankCode"); }
        }

        protected string m_BankName;
        [DBField("BankName"), TextSearch, MaxLength(255), Export(true), Required]
        public string BankName
        {
            get { return m_BankName; }
            set { m_BankName = value; modify("BankName"); }
        }

        protected string m_SwiftCode;
        [DBField("SwiftCode"), TextSearch, MaxLength(25), Export(true), Required]
        public string SwiftCode
        {
            get { return m_SwiftCode; }
            set { m_SwiftCode = value; modify("SwiftCode"); }
        }

        protected string m_LocalClearingCode;
        [DBField("LocalClearingCode"), TextSearch, MaxLength(10), Export(true), Required]
        public string LocalClearingCode
        {
            get { return m_LocalClearingCode; }
            set { m_LocalClearingCode = value; modify("LocalClearingCode"); }
        }

        protected string m_CountryCode;
        [DBField("CountryCode"), TextSearch, MaxLength(10), Export(true), Required]
        public string CountryCode
        {
            get { return m_CountryCode; }
            set { m_CountryCode = value; modify("CountryCode"); }
        }
    }
}
