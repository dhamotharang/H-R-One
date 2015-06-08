using System;
using System.Data;
using System.Configuration;
using System.IO;
using HROne.DataAccess;

namespace HROne.BankFile
{
    /// <summary>
    /// Summary description for DBSBankFile
    /// </summary>
    public class DBSBankFile : GenericBankFile
    {
        private const string FIELD_DELIMITER = "\",\"";
        private const string RECORD_DELIMITER = "\r\n";

        public enum TransactionTypeEnum
        {
            DirectDebit = 8,
            SundryCredit = 20,
            SalaryCredit = 22
        }

        protected string m_BatchID;
        public string BatchID
        {
            get { return m_BatchID; }
            set { m_BatchID = value; }
        }

        protected string m_BatchName;
        public string BatchName
        {
            get { return m_BatchName; }
            set { m_BatchName = value; }
        }

        protected string m_SecondPartyReference;
        public string SecondPartyReference
        {
            get { return m_SecondPartyReference; }
            set { m_SecondPartyReference = value; }
        }

        protected bool m_IsSecondPartyReferenceIncludeEmpNo;
        public bool IsSecondPartyReferenceIncludeEmpNo
        {
            get { return m_IsSecondPartyReferenceIncludeEmpNo; }
            set { m_IsSecondPartyReferenceIncludeEmpNo = value; }
        }

        protected TransactionTypeEnum m_TransactionType;
        public TransactionTypeEnum TransactionType
        {
            get { return m_TransactionType; }
            set { m_TransactionType = value; }
        }

        public DBSBankFile(DatabaseConnection dbConn)
            : base(dbConn)
        {
            m_hasFooter = false;
        }

        protected override string GenerateBankFileHeader()
        {
            string[] bankFileHeader = new string[5];

            bankFileHeader[0] = m_BatchID.PadRight(5).Substring(0, 5).Trim();
            bankFileHeader[1] = ((int)m_TransactionType).ToString("00");
            string companyAccountNo = BankCode.PadRight(3).Substring(0, 3) + "-" + BranchCode.PadRight(3).Substring(0, 3) + "-" + AccountNo.PadRight(9).Substring(0, 9).Trim();
            bankFileHeader[2] = companyAccountNo;
            bankFileHeader[3] = AccountHolderName.PadRight(20).Substring(0, 20);
            bankFileHeader[4] = m_BatchName;

            string bankFileHeaderData = "\"" + string.Join(FIELD_DELIMITER, bankFileHeader) + "\"";
            //if (bankFileHeaderDetail.Length != 83)
            //    throw new Exception("Incorrect Bank File Header Length:\r\n" + bankFileHeaderDetail);
            return bankFileHeaderData;
        }

        protected override string GenerateBankFileDetail(GenericBankFileDetail bankFileDetail)
        {
            string[] bankFileDetailPaymentRecord = new string[4];
            bankFileDetailPaymentRecord[0] = bankFileDetail.BankCode.PadRight(3).Substring(0, 3) + "-" + bankFileDetail.BranchCode.PadRight(3).Substring(0, 3) + "-" + bankFileDetail.AccountNo.PadRight(9).Substring(0,9).Trim();
            bankFileDetailPaymentRecord[1] = bankFileDetail.EmpBankAccountHolderName.ToUpper().PadRight(20).Substring(0, 20).Trim();
            bankFileDetailPaymentRecord[2] = bankFileDetail.Amount.ToString("0.00");
            string detailSecondPartyReference = m_SecondPartyReference;
            if (m_IsSecondPartyReferenceIncludeEmpNo)
                detailSecondPartyReference = bankFileDetail.EmpNo + " " + detailSecondPartyReference;
            bankFileDetailPaymentRecord[3] = detailSecondPartyReference.PadRight(18).Substring(0, 18).Trim();

            string bankFileDetailData = "\"" + String.Join(FIELD_DELIMITER, bankFileDetailPaymentRecord) + "\"";
            //if (bankFileDetailData.Length != 78)
            //    throw new Exception("Incorrect Bank File Detail Length:\r\n" + bankFileDetailData);
            return bankFileDetailData;
        }
        public override string BankFileExtension()
        {
            return ".txt";
        }
    }    
}