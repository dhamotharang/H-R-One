using System;
using System.Data;
using System.Configuration;
using System.IO;
using HROne.DataAccess;

namespace HROne.BankFile
{
    /// Summary description for BOCBankFile
    /// </summary>
    public class BOCBankFile : GenericBankFile 
    {
        private const string FIELD_DELIMITER = "";
        private const string RECORD_DELIMITER = "\r\n";

        public BOCBankFile(DatabaseConnection dbConn)
            : base(dbConn)
        {
        }

        public override FileInfo GenerateBankFile()
        {
            //  no header for BOC Bank File format
            string bankFileData = string.Empty;
            foreach (GenericBankFileDetail bankFileDetail in BankFileDetails)
            {
                bankFileData += GenerateBankFileDetail(bankFileDetail) + RECORD_DELIMITER;
            }

            FileInfo result = GenerateTempFileName();
            StreamWriter writer = new StreamWriter(result.OpenWrite());
            writer.Write(bankFileData);
            writer.Close();
            return result;
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
            bankFileDetailRecord[6] = string.Empty.PadLeft(6);
            bankFileDetailRecord[7] = string.Empty.PadLeft(1);

            string bankFileDetailData = String.Join(FIELD_DELIMITER, bankFileDetailRecord);

            if (bankFileDetailData.Length != 72)
                throw (new Exception("Incorrect Detail Length of Payment for " + bankFileDetail.EmpBankAccountHolderName + ":" + bankFileDetailData.Length));
            return bankFileDetailData;
        }
        public override string BankFileExtension()
        {
                return ".dat";
        }
    }
}