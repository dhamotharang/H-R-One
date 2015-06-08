using System;
using System.Data;
using System.Configuration;
using System.IO;
using HROne.DataAccess;

namespace HROne.BankFile
{
    /// <summary>
    /// Summary description for SCBBankFile
    /// </summary>
    public class SCBBankFile : GenericBankFile
    {
        private const string FIELD_DELIMITER = "";
        private const string RECORD_DELIMITER = "\r\n";

        protected string m_CustomerReference;
        public string CustomerReference
        {
            get { return m_CustomerReference; }
            set { m_CustomerReference= value; }
        }

        public SCBBankFile(DatabaseConnection dbConn)
            : base(dbConn)
        {
        }

        public override FileInfo GenerateBankFile()
        {
            string[] bankFileHeader = new string[5];

            bankFileHeader[0] = m_AccountHolderName.ToUpper().PadRight(40).Substring(0, 40);
            string companyAccountNo = m_AccountNo.PadRight(8).Substring(0, 8);
            companyAccountNo = companyAccountNo.Substring(0, 3) + "-" + companyAccountNo.Substring(3, 4) + "-" + companyAccountNo.Substring(7);
            bankFileHeader[1] = m_BranchCode.PadRight(3).Substring(0, 3) + "-" + companyAccountNo;
            bankFileHeader[2] = m_ValueDate.ToString("dd/MM/yy");
            bankFileHeader[3] = RecordCount.ToString("00000000");
            bankFileHeader[4] = m_TotalAmount.ToString("0.00").PadLeft(13);

            string bankFileHeaderDetail = string.Join(FIELD_DELIMITER, bankFileHeader);
            if (bankFileHeaderDetail.Length != 83)
                throw new Exception("Incorrect Bank File Header Length:\r\n" + bankFileHeaderDetail);
            string bankFileData = bankFileHeaderDetail + RECORD_DELIMITER;

            foreach (GenericBankFileDetail bankFileDetail in BankFileDetails)
            {
                bankFileData += GenerateBankFileDetail(bankFileDetail) + RECORD_DELIMITER;
            }
            bankFileData += string.Empty.PadRight(80);

            FileInfo result = GenerateTempFileName();
            StreamWriter writer = new StreamWriter(result.OpenWrite());
            writer.Write(bankFileData);
            writer.Close();
            return result;
        }

        protected override string GenerateBankFileDetail(GenericBankFileDetail bankFileDetail)
        {
            string[] bankFileDetailPaymentRecord = new string[5];
            bankFileDetailPaymentRecord[0] = bankFileDetail.EmpNo.PadRight(10).Substring(0, 10);
            bankFileDetailPaymentRecord[1] = bankFileDetail.EmpBankAccountHolderName.ToUpper().PadRight(20).Substring(0, 20);
            bankFileDetailPaymentRecord[2] = bankFileDetail.BankCode.PadRight(3).Substring(0, 3) + "-" + bankFileDetail.BranchCode.PadRight(3).Substring(0, 3) + bankFileDetail.AccountNo.PadRight(11).Substring(0, 11);
            bankFileDetailPaymentRecord[3] = bankFileDetail.Amount.ToString("0.00").PadLeft(12);
            bankFileDetailPaymentRecord[4] = m_CustomerReference.PadRight(18).Substring(0, 18);

            string bankFileDetailData = String.Join(FIELD_DELIMITER, bankFileDetailPaymentRecord);
            if (bankFileDetailData.Length != 78)
                throw new Exception("Incorrect Bank File Detail Length:\r\n" + bankFileDetailData);
            return bankFileDetailData;
        }
        public override string BankFileExtension()
        {
            return ".txt";
        }
    }    
}