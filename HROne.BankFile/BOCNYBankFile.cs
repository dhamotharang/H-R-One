using System;
using System.Data;
using System.Configuration;
using System.IO;
using HROne.DataAccess;

namespace HROne.BankFile
{
    /// Bank file for Bank of China (Nanyang) 
    /// </summary>
    public class BOCNYBankFile : GenericBankFile 
    {
        private const string FIELD_DELIMITER = "";
        private const string RECORD_DELIMITER = "\r\n";

        public enum FileTypeEnum
        {
            Payroll,
            Autopay
        }
        protected FileTypeEnum m_FileType;
        public FileTypeEnum FileType
        {
            get { return m_FileType; }
            set { m_FileType = value; }
        }

        //public override FileInfo GenerateBankFile()
        //{
        //    //  no header for BOC Bank File format
        //    string bankFileData = string.Empty;
        //    foreach (GenericBankFileDetail bankFileDetail in BankFileDetails)
        //    {
        //        bankFileData += GenerateBankFileDetail(bankFileDetail) + RECORD_DELIMITER;
        //    }

        //    bankFileData += string.Empty.PadRight(80);



        //    FileInfo result = GenerateTempFileName();
        //    StreamWriter writer = new StreamWriter(result.OpenWrite());
        //    writer.Write(bankFileData);
        //    writer.Close();
        //    return result;
        //}
        public BOCNYBankFile(DatabaseConnection dbConn)
            : base(dbConn)
        {
            m_hasHeader = false;
            m_hasFooter = true;
        }

        protected override string GenerateBankFileDetail(GenericBankFileDetail bankFileDetail)
        {
            string[] bankFileDetailRecord = new string[8];
            bankFileDetailRecord[0] = bankFileDetail.BankCode.PadRight(3).Substring(0, 3);
            bankFileDetailRecord[1] = bankFileDetail.BranchCode.PadRight(3).Substring(0, 3);
            bankFileDetailRecord[2] = bankFileDetail.AccountNo.PadRight(9).Substring(0, 9);
            bankFileDetailRecord[3] = bankFileDetail.EmpBankAccountHolderName.ToUpper().PadRight(20).Substring(0, 20);
            bankFileDetailRecord[4] = bankFileDetail.Amount.ToString("0.00").PadLeft(12);
            bankFileDetailRecord[5] = string.Empty.PadLeft(18);
            // For last 2 column, Payroll file = 6 + 0 chars, Autopay file = 5 + 1 Chars
            bankFileDetailRecord[6] = string.Empty.PadLeft(5);
            bankFileDetailRecord[7] = string.Empty.PadLeft(1);

            string bankFileDetailData = String.Join(FIELD_DELIMITER, bankFileDetailRecord);

            if (bankFileDetailData.Length != 71)
                throw (new Exception("Incorrect Detail Length of Payment for " + bankFileDetail.EmpBankAccountHolderName + ":" + bankFileDetailData.Length));
            return bankFileDetailData;
        }

        protected override string GenerateBankFileFooter()
        {
            string[] bankFileFooter = new string[7];
            bankFileFooter[0] = "CI";
            bankFileFooter[1] = BankCode.PadRight(3).Substring(0, 3) + BranchCode.PadRight(3).Substring(0, 3) + AccountNo.PadRight(8).Substring(0, 8);
            bankFileFooter[2] = this.AccountHolderName.ToUpper().PadRight(50).Substring(0, 50);
            if (FileType == FileTypeEnum.Autopay)
            {
                bankFileFooter[3] = "CR";
                bankFileFooter[4] = ValueDate.ToString("yyyyMMdd");
                bankFileFooter[5] = RecordCount.ToString("00000");
                bankFileFooter[6] = TotalAmount.ToString("0.00").PadLeft(12);

                string bankFileFooterData = string.Join(FIELD_DELIMITER, bankFileFooter);
                if (bankFileFooterData.Length != 93)
                    throw (new Exception("Incorrect Detail Length of Payment for Footer"));
                return bankFileFooterData;
            }
            else
            {
                bankFileFooter[3] = ValueDate.ToString("yyyyMMdd");
                bankFileFooter[4] = RecordCount.ToString("00000");
                bankFileFooter[5] = TotalAmount.ToString("0.00").PadLeft(12);
                bankFileFooter[6] = string.Empty.PadLeft(7);

                string bankFileFooterData = string.Join(FIELD_DELIMITER, bankFileFooter);
                if (bankFileFooterData.Length != 98)
                    throw (new Exception("Incorrect Detail Length of Payment for Footer"));
                return bankFileFooterData;
            }

        }

        public override string BankFileExtension()
        {
                return ".dat";
        }
    }
}