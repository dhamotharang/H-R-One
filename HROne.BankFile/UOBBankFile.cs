using System;
using System.Data;
using System.Configuration;
using System.IO;
using HROne.DataAccess;
using HROne.Lib.Entities;

namespace HROne.BankFile
{
    /// <summary>
    /// Summary description for UOBBankFile
    /// </summary>
    public class UOBBankFile : GenericBankFile
    {
        private const string FIELD_DELIMITER = "";
        private const string RECORD_DELIMITER = "\r\n";

        public enum TransactionTypeEnum
        {
            DirectDebit = 8,
            SundryCredit = 20,
            SalaryCredit = 22
        }

        //protected string m_BatchID;
        //public string BatchID
        //{
        //    get { return m_BatchID; }
        //    set { m_BatchID = value; }
        //}

        protected string m_Particulars;
        public string Particulars
        {
            get { return m_Particulars; }
            set { m_Particulars = value; }
        }

        //protected string m_SecondPartyReference;
        //public string SecondPartyReference
        //{
        //    get { return m_SecondPartyReference; }
        //    set { m_SecondPartyReference = value; }
        //}

        //protected bool m_IsSecondPartyReferenceIncludeEmpNo;
        //public bool IsSecondPartyReferenceIncludeEmpNo
        //{
        //    get { return m_IsSecondPartyReferenceIncludeEmpNo; }
        //    set { m_IsSecondPartyReferenceIncludeEmpNo = value; }
        //}

        protected double m_totalCreditAmount;
        public double totalCreditAmount
        {
            get { return m_totalCreditAmount; }
            set { m_totalCreditAmount = value; }
        }

        protected int m_detailCount;
        public int detailCount
        {
            get { return m_detailCount; }
            set { m_detailCount = value; }
        }

        protected TransactionTypeEnum m_TransactionType;
        public TransactionTypeEnum TransactionType
        {
            get { return m_TransactionType; }
            set { m_TransactionType = value; }
        }

        public UOBBankFile(DatabaseConnection dbConn)
            : base(dbConn)
        {
            m_hasFooter = true;
            m_detailCount = 0;
            m_totalCreditAmount = 0;
        }

        protected override string GenerateBankFileHeader()
        {
            string[] bankFileHeader = new string[13];

            // Record Type 9(1)
            bankFileHeader[0] = "1";
            // Service Type X(10)
            bankFileHeader[1] = "IBGINORM  ";
            // Originating Bank Code 9(3)
            bankFileHeader[2] = BankCode.PadRight(3).Substring(0, 3);
            // Originating Branch Code 9(3)
            bankFileHeader[3] = BranchCode.PadRight(3).Substring(0, 3);
            // Originating Account No 9(11)
            bankFileHeader[4] = ((string)("0" + AccountNo)).PadRight(11).Substring(0, 11);
            // Originating A/C Name
            bankFileHeader[5] = AccountHolderName.PadRight(20).Substring(0, 20);
            // Creation Date 9(8)
            bankFileHeader[6] = AppUtils.ServerDateTime().ToString("yyyyMMdd");
            // Value Date 9(8)
            bankFileHeader[7] = ValueDate.ToString("yyyyMMdd");
            // Filler X(5)
            bankFileHeader[8] = "     ";
            // Hash Indicator X(1)
            bankFileHeader[9] = " ";
            // Payment Advice Header Line1 X(105)
            bankFileHeader[10] = "".PadRight(105);
            // Payment Advice Header Line2 X(105)
            bankFileHeader[11] = "".PadRight(105);
            // Filler X(320)
            bankFileHeader[12] = "".PadRight(320);


            string bankFileHeaderData = string.Join(FIELD_DELIMITER, bankFileHeader);
            if (bankFileHeaderData.Length != 600)
                throw new Exception("Incorrect Bank File Header Length:\r\n" + bankFileHeaderData);
            return bankFileHeaderData;
        }

        protected override string GenerateBankFileDetail(GenericBankFileDetail bankFileDetail)
        {
            string[] bankFileDetailPaymentRecord = new string[31];

            // Record Type 9(1)
            bankFileDetailPaymentRecord[0] = "2";
            // Receiving Bank Code 9(3)
            bankFileDetailPaymentRecord[1] = bankFileDetail.BankCode.PadRight(3).Substring(0, 3);
            // Receiving Branch Code 9(3)
            bankFileDetailPaymentRecord[2] = bankFileDetail.BranchCode.PadRight(3).Substring(0, 3);
            // Receiving Account Code 9(11)
            bankFileDetailPaymentRecord[3] = bankFileDetail.AccountNo.PadRight(11).Substring(0,11);
            // Receiving A/c Name X(20)
            bankFileDetailPaymentRecord[4] = bankFileDetail.EmpBankAccountHolderName.ToUpper().PadRight(20).Substring(0, 20);
            // Transaction Code 9(2)
            bankFileDetailPaymentRecord[5] = ((int)TransactionType).ToString("00");
            // Amount 9(9)V9(2)
            string m_amount = bankFileDetail.Amount.ToString("0.00").Replace(".", "");
            bankFileDetailPaymentRecord[6] = m_amount.PadLeft(11);
            // Particulars X(6)
            bankFileDetailPaymentRecord[7] = Particulars.PadRight(6).Substring(0, 6);
            // Reference X(18)
            bankFileDetailPaymentRecord[8] = "".PadRight(18);
            // Print Payment Advice Indicator X(1)
            bankFileDetailPaymentRecord[9] = "N";
            // Delivery Mode (Print) X(1)
            bankFileDetailPaymentRecord[10] = "".PadRight(1);
            // Delivery Mode (Email) X(1)
            bankFileDetailPaymentRecord[11] = "".PadRight(1);
            // Delivery Mode (Fax) X(1)
            bankFileDetailPaymentRecord[12] = "".PadRight(1);
            // Delivery Mode (-) X(1)
            bankFileDetailPaymentRecord[13] = "".PadRight(1);
            // Advice Format X(1)
            bankFileDetailPaymentRecord[14] = "".PadRight(1);
            // Beneficiary ID X(20)
            bankFileDetailPaymentRecord[15] = "".PadRight(20);
            // Beneficiary Name (line 1) X(35)
            bankFileDetailPaymentRecord[16] = "".PadRight(35);
            // Beneficiary Name (line 2) X(35)
            bankFileDetailPaymentRecord[17] = "".PadRight(35);
            // Beneficiary Name (Address 1) X(35)
            bankFileDetailPaymentRecord[18] = "".PadRight(35);
            // Beneficiary Name (Address 2) X(35)
            bankFileDetailPaymentRecord[19] = "".PadRight(35);
            // Beneficiary Name (Address 3) X(35)
            bankFileDetailPaymentRecord[20] = "".PadRight(35);
            // Beneficiary Name (Address 4) X(35)
            bankFileDetailPaymentRecord[21] = "".PadRight(35);
            // Beneficiary City X(17)
            bankFileDetailPaymentRecord[22] = "".PadRight(17);
            // Beneficiary Country Code X(3)
            bankFileDetailPaymentRecord[23] = "".PadRight(3);
            // Beneficiary Postal Code X(15)
            bankFileDetailPaymentRecord[24] = "".PadRight(15);
            // Email Address of Beneficiary X(50)
            bankFileDetailPaymentRecord[25] = "".PadRight(50);
            // Facsimile Number of Beneficiary X(20)
            bankFileDetailPaymentRecord[26] = "".PadRight(20);
            // Payer's name (line 1) X(35)
            bankFileDetailPaymentRecord[27] = "".PadRight(35);
            // Payer's name (line 2) X(35)
            bankFileDetailPaymentRecord[28] = "".PadRight(35); 
            // Payer/Customer Reference X(30)
            bankFileDetailPaymentRecord[29] = "".PadRight(30); 
            // Filler X(84)
            bankFileDetailPaymentRecord[30] = "".PadRight(84); 


            //bankFileDetailPaymentRecord[0] = bankFileDetail.BankCode.PadRight(3).Substring(0, 3) + "-" + bankFileDetail.BranchCode.PadRight(3).Substring(0, 3) + "-" + bankFileDetail.AccountNo.PadRight(9).Substring(0,9).Trim();
            //bankFileDetailPaymentRecord[1] = bankFileDetail.EmpBankAccountHolderName.ToUpper().PadRight(20).Substring(0, 20).Trim();
            //bankFileDetailPaymentRecord[2] = bankFileDetail.Amount.ToString("0.00");
            //string detailSecondPartyReference = m_SecondPartyReference;
            //if (m_IsSecondPartyReferenceIncludeEmpNo)
            //    detailSecondPartyReference = bankFileDetail.EmpNo + " " + detailSecondPartyReference;
            //bankFileDetailPaymentRecord[3] = detailSecondPartyReference.PadRight(18).Substring(0, 18).Trim();

            string bankFileDetailData = String.Join(FIELD_DELIMITER, bankFileDetailPaymentRecord);
            if (bankFileDetailData.Length != 600)
                throw new Exception("Incorrect Bank File Detail Length:\r\n" + bankFileDetailData);

            m_totalCreditAmount += bankFileDetail.Amount;
            m_detailCount++;
            
            return bankFileDetailData;
        }

        protected override string GenerateBankFileFooter()
        {
            string[] bankFileFooter = new string[8];
            // Record Type 9(1)
            bankFileFooter[0] = "9";
            // Total Debit Amount 9(11)V9(2)
            bankFileFooter[1] = "000".PadLeft(13);
            // Total Credit Amount 9(11)V9(2)
            bankFileFooter[2] = m_totalCreditAmount.ToString("0.00").Replace(".", "").PadLeft(13);
            // Total Debit Count 9(7)
            bankFileFooter[3] = "0".PadLeft(7);
            // Total Credit Count 9(7)
            bankFileFooter[4] = m_detailCount.ToString("0").PadLeft(7);
            // Old Hash Total
            bankFileFooter[5] = "".PadRight(13);
            // New Hash Total
            bankFileFooter[6] = "".PadRight(13);
            // Filler
            bankFileFooter[7] = "".PadRight(533);

            return string.Join(FIELD_DELIMITER, bankFileFooter);
        }

        public override string BankFileExtension()
        {
            return ".TXT";
        }

        public override string ActualBankFileName()
        {
            string m_filename;
            EBankFileDailySeq m_seq = EBankFileDailySeq.GetObject(dbConn, "UOB", AppUtils.ServerDateTime().Date);
            if (m_seq != null)
            {
                m_seq.BankFileSeq++;
                // seq mod 100 to make sure a 3-digits number remains in 2-digits
                m_filename = "UIAI" + processDateTime.ToString("ddMM") + (m_seq.BankFileSeq % 100).ToString("00") + BankFileExtension();
                EBankFileDailySeq.db.update(dbConn, m_seq);
            }else
            {
                m_filename = "UIAI" + processDateTime.ToString("ddMM") + "01" + BankFileExtension();
                m_seq = new EBankFileDailySeq();
                m_seq.BankCode = "UOB";
                m_seq.BankFileDate = AppUtils.ServerDateTime().Date;
                m_seq.BankFileSeq = 1;
                EBankFileDailySeq.db.insert(dbConn, m_seq);
            }
            
            return m_filename;
        }
    }    
}