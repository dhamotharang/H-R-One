using System;
using HROne.DataAccess;

namespace HROne.Lib.Entities
{
    [DBClass("BankFileDailySeq")]
    public class EBankFileDailySeq : BaseObject
    {
        public static DBManager db = new DBManager(typeof(EBankFileDailySeq));
//        public static WFValueList VLShiftDutyCode = new WFDBCodeList(db, "ShiftDutyCodeID", "ShiftDutyCode", "ShiftDutyCodeDesc", "ShiftDutyCode");

        public static EBankFileDailySeq GetObject(DatabaseConnection dbConn, string bankCode, DateTime bankFileDate)
        {
            DBFilter m_filter = new DBFilter();
            m_filter.add(new Match("BankCode", bankCode));
            m_filter.add(new Match("BankFileDate", bankFileDate));

            foreach (EBankFileDailySeq m_seq in db.select(dbConn, m_filter))
            {
                return m_seq;
            }
            return null;
        }

        public static EBankFileDailySeq GetObject(DatabaseConnection dbConn, object ID)
        {
            if (ID is int)
            {
                EBankFileDailySeq obj = new EBankFileDailySeq();
                obj.BankFileDailySeqID = (int)ID;
                if (db.select(dbConn, obj))
                    return obj;
            }
            return null;
        }

        protected int m_BankFileDailySeqID;
        [DBField("BankFileDailySeqID", true, true), TextSearch, Export(false)]
        public int BankFileDailySeqID
        {
            get { return m_BankFileDailySeqID; }
            set { m_BankFileDailySeqID = value; modify("BankFileDailySeqID"); }
        }

        protected string m_BankCode;
        [DBField("BankCode"), TextSearch, MaxLength(20), Export(false), Required]
        public string BankCode
        {
            get { return m_BankCode; }
            set { m_BankCode = value; modify("BankCode"); }
        }

        protected DateTime m_BankFileDate;
        [DBField("BankFileDate"), TextSearch, MaxLength(10), Export(false), Required]
        public DateTime BankFileDate
        {
            get { return m_BankFileDate; }
            set { m_BankFileDate = value; modify("BankFileDate"); }
        }

        protected int m_BankFileSeq;
        [DBField("BankFileSeq"), TextSearch, Export(false), Required]
        public int BankFileSeq
        {
            get { return m_BankFileSeq; }
            set { m_BankFileSeq = value; modify("BankFileSeq"); }
        }   
    }
}
