using System;
using System.Collections.Generic;
using System.Text;
using HROne.DataAccess;
////using perspectivemind.validation;
using HROne.Common;
namespace HROne.Lib.Entities
{

    [DBClass("Company")]
    public class ECompany : BaseObject
    {
        public static DBManager db = new DBManager(typeof(ECompany));
//        public static WFValueList VLCompany = new AppUtils.EncryptedDBCodeList(db, "CompanyID", "CompanyCode", "CompanyName", "CompanyCode");
        public static WFValueList VLCompany = new AppUtils.EncryptedDBCodeList(db, "CompanyID", new string[] { "CompanyCode", "CompanyName" }, " - ", "CompanyCode");
        public static WFValueList VLBankAccount = new AppUtils.BankAccountValueList(db, "CompanyID", "CompanyBankCode", "CompanyBranchCode", "CompanyBankAccountNo", "CompanyName", "CompanyID");

        public static ECompany GetObject(DatabaseConnection dbConn, object ID)
        {
            if (ID is int)
            {
                ECompany obj = new ECompany();
                obj.CompanyID = (int)ID;
                if (db.select(dbConn, obj))
                    return obj;
            }
            return null;
        }

        protected int m_CompanyID;
        [DBField("CompanyID", true, true), TextSearch, Export(false)]
        public int CompanyID
        {
            get { return m_CompanyID; }
            set { m_CompanyID = value; modify("CompanyID"); }
        }
        protected string m_CompanyCode;
        [DBField("CompanyCode"), TextSearch, DBAESEncryptStringField, MaxLength(20), Export(false), Required]
        public string CompanyCode
        {
            get { return m_CompanyCode; }
            set { m_CompanyCode = value; modify("CompanyCode"); }
        }
        protected string m_CompanyName;
        [DBField("CompanyName"), TextSearch, DBAESEncryptStringField, MaxLength(70), Export(false), Required]
        public string CompanyName
        {
            get { return m_CompanyName; }
            set { m_CompanyName = value; modify("CompanyName"); }
        }
        protected string m_CompanyAddress;
        [DBField("CompanyAddress"), TextSearch, DBAESEncryptStringField, MaxLength(90), Export(false)]
        public string CompanyAddress
        {
            get { return m_CompanyAddress; }
            set { m_CompanyAddress = value; modify("CompanyAddress"); }
        }
        protected string m_CompanyContactPerson;
        [DBField("CompanyContactPerson"), TextSearch, DBAESEncryptStringField, MaxLength(100, 40), Export(false)]
        public string CompanyContactPerson
        {
            get { return m_CompanyContactPerson; }
            set { m_CompanyContactPerson = value; modify("CompanyContactPerson"); }
        }
        protected string m_CompanyContactNo;
        [DBField("CompanyContactNo"), TextSearch, DBAESEncryptStringField, MaxLength(50, 40), Export(false)]
        public string CompanyContactNo
        {
            get { return m_CompanyContactNo; }
            set { m_CompanyContactNo = value; modify("CompanyContactNo"); }
        }
        protected string m_CompanyFaxNo;
        [DBField("CompanyFaxNo"), TextSearch, DBAESEncryptStringField, MaxLength(50, 40), Export(false)]
        public string CompanyFaxNo
        {
            get { return m_CompanyFaxNo; }
            set { m_CompanyFaxNo = value; modify("CompanyFaxNo"); }
        }
        protected string m_CompanyBRNo;
        [DBField("CompanyBRNo"), TextSearch, DBAESEncryptStringField, MaxLength(50, 40), Export(false)]
        public string CompanyBRNo
        {
            get { return m_CompanyBRNo; }
            set { m_CompanyBRNo = value; modify("CompanyBRNo"); }
        }
        //protected string m_CompanyBankCode;
        //[DBField("CompanyBankCode"), TextSearch, Int, DBAESEncryptStringField, MaxLength(3, 3), Export(false)]
        //public string CompanyBankCode
        //{
        //    get { return m_CompanyBankCode; }
        //    set { m_CompanyBankCode = value; modify("CompanyBankCode"); }
        //}
        //protected string m_CompanyBranchCode;
        //[DBField("CompanyBranchCode"), TextSearch, Int, DBAESEncryptStringField, MaxLength(3, 3), Export(false)]
        //public string CompanyBranchCode
        //{
        //    get { return m_CompanyBranchCode; }
        //    set { m_CompanyBranchCode = value; modify("CompanyBranchCode"); }
        //}
        //protected string m_CompanyBankAccountNo;
        //[DBField("CompanyBankAccountNo"), TextSearch, DBAESEncryptStringField, MaxLength(20, 20), Export(false)]
        //public string CompanyBankAccountNo
        //{
        //    get { return m_CompanyBankAccountNo; }
        //    set { m_CompanyBankAccountNo = value; modify("CompanyBankAccountNo"); }
        //}
        //protected string m_CompanyBankHolderName;
        //[DBField("CompanyBankHolderName"), TextSearch, DBAESEncryptStringField, MaxLength(100, 40), Export(false)]
        //public string CompanyBankHolderName
        //{
        //    get { return m_CompanyBankHolderName; }
        //    set { m_CompanyBankHolderName = value; modify("CompanyBankHolderName"); }
        //}
        
    }
}
