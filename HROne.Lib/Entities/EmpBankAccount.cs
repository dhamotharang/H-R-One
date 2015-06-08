using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Data;
using HROne.DataAccess;
////using perspectivemind.validation;
using HROne.Common;
namespace HROne.Lib.Entities
{
//    public class BankAccountValueList : WFDBList2
//    {
//        public BankAccountValueList()
//            : base(EEmpBankAccount.db, new string[] { "EmpBankAccountID", "EmpBankCode", "EmpBranchCode", "EmpAccountNo" },"EmpAccountNo")
//        {
//        }
//        public override string GetText(IDataReader reader)
//        {
//            return EEmpBankAccount.db.getField(reader.GetName(1)).transcoder.fromDB(reader.GetString(1)).ToString()
//            + EEmpBankAccount.db.getField(reader.GetName(2)).transcoder.fromDB(reader.GetString(2)).ToString()
//            +EEmpBankAccount.db.getField(reader.GetName(3)).transcoder.fromDB(reader.GetString(3)).ToString();
////            return reader.GetString(1) + "-" + reader.GetString(2) + "-" + reader.GetString(3);
//        }
//        public override string GetKey(IDataReader reader)
//        {
//            return reader.GetInt32(0).ToString();
//        }

//    }
    [DBClass("EmpBankAccount")]
    public class EEmpBankAccount : BaseObjectWithRecordInfo
    {

        public static DBManager db = new DBManagerWithRecordInfo(typeof(EEmpBankAccount));
        public static WFValueList VLBankAccount = new AppUtils.BankAccountValueList(db, "EmpBankAccountID", "EmpBankCode", "EmpBranchCode", "EmpAccountNo", "EmpBankAccountRemark", "EmpBankAccountID");
        public const string DEFAULT_BANK_ACCOUNT_TEXT = "Default Bank Account";

        public static EEmpBankAccount GetObject(DatabaseConnection dbConn, int ID)
        {
            EEmpBankAccount m_object = new EEmpBankAccount();
            m_object.EmpBankAccountID = ID;
            if (EEmpBankAccount.db.select(dbConn, m_object))
                return m_object;
            return null;
        }

        protected int m_EmpBankAccountID;
        [DBField("EmpBankAccountID", true, true), TextSearch, Export(false)]
        public int EmpBankAccountID
        {
            get { return m_EmpBankAccountID; }
            set { m_EmpBankAccountID = value; modify("EmpBankAccountID"); }
        }

        protected int m_EmpID;
        [DBField("EmpID"), TextSearch, Export(false)]
        public int EmpID
        {
            get { return m_EmpID; }
            set { m_EmpID = value; modify("EmpID"); }
        }
        protected string m_EmpBankCode;
        [DBField("EmpBankCode"), TextSearch, Int, DBAESEncryptStringField, MaxLength(3, 3), Export(false), Required]
        public string EmpBankCode
        {
            get { return m_EmpBankCode; }
            set { m_EmpBankCode = value; modify("EmpBankCode"); }
        }
        protected string m_EmpBranchCode;
        [DBField("EmpBranchCode"), TextSearch, Int, DBAESEncryptStringField, MaxLength(3, 3), Export(false), Required]
        public string EmpBranchCode
        {
            get { return m_EmpBranchCode; }
            set { m_EmpBranchCode = value; modify("EmpBranchCode"); }
        }
        protected string m_EmpAccountNo;
        [DBField("EmpAccountNo"), TextSearch, Int, DBAESEncryptStringField, MaxLength(9, 9), Export(false), Required]
        public string EmpAccountNo
        {
            get { return m_EmpAccountNo; }
            set { m_EmpAccountNo = value; modify("EmpAccountNo"); }
        }
        protected string m_EmpBankAccountHolderName;
        [DBField("EmpBankAccountHolderName"), TextSearch, DBAESEncryptStringField, MaxLength(100, 25), Export(false), Required]
        public string EmpBankAccountHolderName
        {
            get { return m_EmpBankAccountHolderName; }
            set { m_EmpBankAccountHolderName = value; modify("EmpBankAccountHolderName"); }
        }
        protected string m_EmpBankAccountRemark;
        [DBField("EmpBankAccountRemark"), TextSearch, DBAESEncryptStringField, MaxLength(100, 25), Export(false)]
        public string EmpBankAccountRemark
        {
            get { return m_EmpBankAccountRemark; }
            set { m_EmpBankAccountRemark = value; modify("EmpBankAccountRemark"); }
        }
        protected bool m_EmpAccDefault;
        [DBField("EmpAccDefault"), TextSearch, Export(false), Required]
        public bool EmpAccDefault
        {
            get { return m_EmpAccDefault; }
            set { m_EmpAccDefault = value; modify("EmpAccDefault"); }
        }

        //  For Synchronize Use only
        protected string m_SynID;
        [DBField("SynID"), TextSearch, Export(false)]
        public string SynID
        {
            get { return m_SynID; }
            set { m_SynID = value; modify("SynID"); }
        }

        public static EEmpBankAccount GetDefaultBankAccount(DatabaseConnection dbConn, int EmpID)
        {
            DBFilter filter = new DBFilter();
            filter.add(new Match("EmpID", EmpID));
            filter.add(new Match("EmpAccDefault", "<>", 0));
            ArrayList bankAccountList = EEmpBankAccount.db.select(dbConn, filter);
            if (bankAccountList.Count > 0)
                return (EEmpBankAccount)bankAccountList[0];
            else
            {
                EEmpPersonalInfo empInfo = new EEmpPersonalInfo();
                empInfo.EmpID = EmpID;
                if (EEmpPersonalInfo.db.select(dbConn, empInfo) && empInfo.MasterEmpID > 0)
                    return GetDefaultBankAccount(dbConn, empInfo.MasterEmpID);
            }
            return null;
        }
    }
}
