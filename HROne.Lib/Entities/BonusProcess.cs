using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Data;
using HROne.DataAccess;
using HROne.Common;
namespace HROne.Lib.Entities
{
    [DBClass("BonusProcess")]
    public class EBonusProcess : BaseObjectWithRecordInfo
    {
        public const string STATUS_CONFIRMED = "C";
        public const string STATUS_NORMAL = "N";
        public const string STATUS_CANCELLED = "X";

        public const string STATUS_CONFIRMED_DESC = "Confirmed";
        public const string STATUS_NORMAL_DESC = "Normal";
        public const string STATUS_CANCELLED_DESC = "Cancelled";

        public static DBManager db = new DBManagerWithRecordInfo(typeof(EBonusProcess));
        public static WFValueList VLBonusProcessList = new WFDBCodeList(EBonusProcess.db, "BonusProcessID", "CONVERT(nvarchar(7), BonusProcessMonth, 121)", "BonusProcessDesc", "BonusProcessMonth, BonusProcessDesc");

//        public static WFValueList VLBankAccount = new AppUtils.BankAccountValueList(db, "EmpBankAccountID", "EmpBankCode", "EmpBranchCode", "EmpAccountNo", "EmpBankAccountRemark", "EmpBankAccountID");
        //public const string DEFAULT_BANK_ACCOUNT_TEXT = "Default Bank Account";

        public static EBonusProcess GetObject(DatabaseConnection dbConn, int ID)
        {
            EBonusProcess m_object = new EBonusProcess();
            m_object.BonusProcessID = ID;
            if (EBonusProcess.db.select(dbConn, m_object))
                return m_object;
            return null;
        }

        protected int m_BonusProcessID;
        [DBField("BonusProcessID", true, true), TextSearch, Export(false)]
        public int BonusProcessID
        {
            get { return m_BonusProcessID; }
            set { m_BonusProcessID = value; modify("BonusProcessID"); }
        }

        protected DateTime m_BonusProcessMonth;
        [DBField("BonusProcessMonth", format="yyyy-MM-dd"), TextSearch, Export(false), Required]
        public DateTime BonusProcessMonth
        {
            get { return m_BonusProcessMonth; }
            set { m_BonusProcessMonth = value; modify("BonusProcessMonth"); }
        }
        protected string m_BonusProcessDesc;
        [DBField("BonusProcessDesc"), TextSearch, DBAESEncryptStringField, MaxLength(100, 50), Export(false), Required]
        public string BonusProcessDesc
        {
            get { return m_BonusProcessDesc; }
            set { m_BonusProcessDesc = value; modify("BonusProcessDesc"); }
        }
        protected string m_BonusProcessStatus;
        [DBField("BonusProcessStatus"), TextSearch, MaxLength(3, 3), Export(false), Required]
        public string BonusProcessStatus
        {
            get { return m_BonusProcessStatus; }
            set { m_BonusProcessStatus = value; modify("BonusProcessStatus"); }
        }
        protected DateTime m_BonusProcessPayDate;
        [DBField("BonusProcessPayDate", format="yyyy-MM-dd"), TextSearch, Export(false), Required]
        public DateTime BonusProcessPayDate
        {
            get { return m_BonusProcessPayDate; }
            set { m_BonusProcessPayDate = value; modify("BonusProcessPayDate"); }
        }
        protected int m_BonusProcessPayCodeID;
        [DBField("BonusProcessPayCodeID"), TextSearch, Export(false), Required]
        public int BonusProcessPayCodeID
        {
            get { return m_BonusProcessPayCodeID; }
            set { m_BonusProcessPayCodeID = value; modify("BonusProcessPayCodeID"); }
        }
        protected DateTime m_BonusProcessSalaryMonth;
        [DBField("BonusProcessSalaryMonth", format = "yyyy-MM-dd"), TextSearch, Export(false), Required]
        public DateTime BonusProcessSalaryMonth
        {
            get { return m_BonusProcessSalaryMonth; }
            set { m_BonusProcessSalaryMonth = value; modify("BonusProcessSalaryMonth"); }
        }

        protected DateTime m_BonusProcessPeriodTo;
        [DBField("BonusProcessPeriodTo", format = "yyyy-MM-dd"), TextSearch, Export(false), Required]
        public DateTime BonusProcessPeriodTo
        {
            get { return m_BonusProcessPeriodTo; }
            set { m_BonusProcessPeriodTo = value; modify("BonusProcessPeriodTo"); }
        }

        protected DateTime m_BonusProcessPeriodFr;
        [DBField("BonusProcessPeriodFr", format = "yyyy-MM-dd"), TextSearch, Export(false), Required]
        public DateTime BonusProcessPeriodFr
        {
            get { return m_BonusProcessPeriodFr; }
            set { m_BonusProcessPeriodFr = value; modify("BonusProcessPeriodFr"); }
        }

        protected double m_BonusProcessStdRate;
        [DBField("BonusProcessStdRate", format = "0.0000"), MaxLength(6,6), TextSearch, Export(false), Required]
        public double BonusProcessStdRate
        {
            get { return m_BonusProcessStdRate; }
            set { m_BonusProcessStdRate = value; modify("BonusProcessStdRate"); }
        }

        protected double m_BonusProcessRank1;
        [DBField("BonusProcessRank1", format = "0.00"), MaxLength(6, 6), TextSearch, Export(false), Required]
        public double BonusProcessRank1
        {
            get { return m_BonusProcessRank1; }
            set { m_BonusProcessRank1 = value; modify("BonusProcessRank1"); }
        }
        protected double m_BonusProcessRank2;
        [DBField("BonusProcessRank2", format = "0.00"), MaxLength(6, 6), TextSearch, Export(false), Required]
        public double BonusProcessRank2
        {
            get { return m_BonusProcessRank2; }
            set { m_BonusProcessRank2 = value; modify("BonusProcessRank2"); }
        }
        protected double m_BonusProcessRank3;
        [DBField("BonusProcessRank3", format = "0.00"), MaxLength(6, 6), TextSearch, Export(false), Required]
        public double BonusProcessRank3
        {
            get { return m_BonusProcessRank3; }
            set { m_BonusProcessRank3 = value; modify("BonusProcessRank3"); }
        }
        protected double m_BonusProcessRank4;
        [DBField("BonusProcessRank4", format = "0.00"), MaxLength(6, 6), TextSearch, Export(false), Required]
        public double BonusProcessRank4
        {
            get { return m_BonusProcessRank4; }
            set { m_BonusProcessRank4 = value; modify("BonusProcessRank4"); }
        }
        protected double m_BonusProcessRank5;
        [DBField("BonusProcessRank5", format = "0.00"), MaxLength(6, 6), TextSearch, Export(false), Required]
        public double BonusProcessRank5
        {
            get { return m_BonusProcessRank5; }
            set { m_BonusProcessRank5 = value; modify("BonusProcessRank5"); }
        }

    
    }
}
