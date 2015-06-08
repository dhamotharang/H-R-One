using System;
using System.Collections;
using System.Data;
using System.Configuration;
using System.IO;
using HROne.DataAccess;
using HROne.Lib.Entities;
namespace HROne.BankFile
{
    /// Summary description for HSBCBankFile
    /// </summary>
    public class BOAmericaBankFile : GenericBankFile
    {
        private const string FIELD_DELIMITER = "\",\"";
        private const string RECORD_DELIMITER = "\r\n";

        const string FIELD_ALTERNATETRANSACTIONTYPE = "PAYROL";
        const string FIELD_BRANCHCODE = "6055";

        const string FIELD_ORIGINATORACCOUNTCURRENCY = "HKD";
        const string FIELD_PAYMENTCOLLECTIONINDICATOR = "PAY";
        const string FIELD_TRANSACTIONHANDLINGCODE = "D";

        private string m_actualFileName = string.Empty;

        protected string m_Indicator;
        public string Indicator
        {
            get { return m_Indicator; }
            set { m_Indicator = value; }
        }

        //protected string m_FileType;
        //public string FileType
        //{
        //    get { return m_FileType; }
        //    set { m_FileType = value; }
        //}

        protected string m_EFTKey;
        public string EFTKey
        {
            get { return m_EFTKey; }
            set { m_EFTKey = value; }
        }

        protected string m_CompanyName;
        public string CompanyName
        {
            get { return m_CompanyName; }
            set { m_CompanyName = value; }
        }

        protected string m_CompanyID;
        public string CompanyID
        {
            get { return m_CompanyID; }
            set { m_CompanyID = value; }
        }

        protected int m_TransactionID = 1;
        public int TransactionID
        {
            get { return m_TransactionID++; }
            set { m_TransactionID = value; }
        }

        protected string m_BankName;
        public string BankName
        {
            get { return m_BankName; }
            set { m_BankName = value; }
        }

        //protected string m_FirstPartyReference;
        //public string FirstPartyReference
        //{
        //    get { return m_FirstPartyReference; }
        //    set { m_FirstPartyReference = value; }
        //}

        protected string m_PostalAddress1;
        public string PostalAddress1
        {
            get { return m_PostalAddress1; }
            set { m_PostalAddress1 = value; }
        }

        protected string m_PostalAddress2;
        public string PostalAddress2
        {
            get { return m_PostalAddress2; }
            set { m_PostalAddress2 = value; }
        }

        protected string m_PostalCityName;
        public string PostalCityName
        {
            get { return m_PostalCityName; }
            set { m_PostalCityName = value; }
        }

        public BOAmericaBankFile(DatabaseConnection dbConn)
            : base(dbConn)
        {
        }

        protected override string GenerateBankFileHeader()
        {

            string[] bankFileHeader = new string[8];

            bankFileHeader[0] = "BULKCSV";
            bankFileHeader[1] = AccountHolderName.ToUpper().PadRight(35).Substring(0, 35).Trim();
            bankFileHeader[2] = m_CompanyID.ToUpper().PadRight(10).Substring(0, 10).Trim();
            bankFileHeader[3] = m_EFTKey.ToUpper().PadRight(4).Substring(0, 4).Trim();
            bankFileHeader[4] = AppUtils.ServerDateTime().ToString("yyyyMMdd");
            bankFileHeader[5] = ActualBankFileName().Trim();
            bankFileHeader[6] = m_Indicator.PadRight(4).Substring(0, 4).Trim();

            string bankFileHeaderData = "\"" + string.Join(FIELD_DELIMITER, bankFileHeader) + "\"";
            bankFileHeaderData = bankFileHeaderData.Replace("\"\"", string.Empty);

            //if (bankFileHeaderData.Length > 107)
            //    throw (new Exception("Incorrect Header Length:" + bankFileHeaderData.Length));
            return bankFileHeaderData;
        }

        protected override string GenerateBankFileDetail(GenericBankFileDetail bankFileDetail)
        {

            EBankList banklist = new EBankList();
            banklist.BankCode = bankFileDetail.BankCode;
            if (EBankList.db.select(dbConn, banklist))
                BankName = banklist.BankName;

            string[] bankFileDetailRecord = new string[45];
            bankFileDetailRecord[0] = "TRN";
            bankFileDetailRecord[1] = "PAYROL"; //  PAYROL - Hong Kong Payroll ACH Payment (AUTOPAY)
            //bankFileDetailRecord[2] = "omit";.Substring(0, 3)
            bankFileDetailRecord[4] = "6055";   //  6055 -  BOA Hong Kong, HK    
            bankFileDetailRecord[5] = AccountNo.PadRight(31).Substring(0, 31).TrimEnd();
            //bankFileDetailRecord[6] = "";//bankFileDetail.BranchCode;//.Substring(0, 9);
            bankFileDetailRecord[7] = "HKD";
            bankFileDetailRecord[8] = "PAY";    //  PAY - Payment
            bankFileDetailRecord[9] = "D";  //  D - Payment Only
            //bankFileDetailRecord[10] = FIELD_POSTINGINDICATOR;//.Substring(0, 1);
            //bankFileDetailRecord[11] = "";//.Substring(0, 16);
            //bankFileDetailRecord[12] = FIELD_PRIORITYINDICATOR;//.Substring(0, 1);
            bankFileDetailRecord[13] = AppUtils.ServerDateTime().ToString("MMddHHmmss") + TransactionID.ToString("000000");
            //bankFileDetailRecord[14] = "";//.Substring(0, 2);
            bankFileDetailRecord[15] = AccountHolderName.ToUpper().PadRight(35).Substring(0, 35).Trim();
            //bankFileDetailRecord[16] = "";.Substring(0, 15);


            bankFileDetailRecord[17] = PostalAddress1.PadRight(35).Substring(0, 35).Trim();
            bankFileDetailRecord[18] = PostalAddress2.PadRight(35).Substring(0, 35).Trim();
            bankFileDetailRecord[19] = PostalCityName.PadRight(30).Substring(0, 30).Trim();
            bankFileDetailRecord[22] = "HK";
            bankFileDetailRecord[23] = bankFileDetail.EmpName.ToUpper().PadRight(70).Substring(0, 70).Trim();
            //bankFileDetailRecord[25] = "ADDRESS LINE 1"+" " ";
            //bankFileDetailRecord[27] = "HONG KONG";
            bankFileDetailRecord[30] = "HK";
            bankFileDetailRecord[31] = bankFileDetail.ValueDate.ToString("yyyyMMdd");
            bankFileDetailRecord[33] = "HKD";
            bankFileDetailRecord[34] = bankFileDetail.Amount.ToString("0.00");
            bankFileDetailRecord[37] = BankName.ToUpper().PadRight(33).Substring(0, 33).Trim();
            bankFileDetailRecord[39] = bankFileDetail.BankCode.PadRight(3).Substring(0, 3) + bankFileDetail.BranchCode.PadRight(3).Substring(0, 3);
            bankFileDetailRecord[41] = bankFileDetail.AccountNo.PadRight(9).Substring(0, 9).Trim();
            bankFileDetailRecord[43] = "HONG KONG";
            bankFileDetailRecord[44] = "HK";
            string bankFileDetailData = "\"" + String.Join(FIELD_DELIMITER, bankFileDetailRecord) + "\"";
            bankFileDetailData = bankFileDetailData.Replace("\"\"", string.Empty);
            //if (bankFileDetailData.Length > 715)
            //    throw (new Exception("Incorrect Detail Length of Payment for " + bankFileDetail.EmpBankAccountHolderName + ":" + bankFileDetailData.Length));
            return bankFileDetailData;
        }

        protected override string GenerateBankFileFooter()
        {
            string[] bankFileFooter = new string[3];
            bankFileFooter[0] = "BULKCSVTRAILER";
            bankFileFooter[1] = RecordCount.ToString();
            bankFileFooter[2] = TotalAmount.ToString("0.00").Replace(".", string.Empty);
            //bankFileFooter[3] = string.Empty.PadRight(3479);

            string bankFileFooterData = "\"" + string.Join(FIELD_DELIMITER, bankFileFooter) + "\"";
            bankFileFooterData = bankFileFooterData.Replace("\"\"", string.Empty);

            //if (bankFileFooterData.Length > 41)
            //    throw (new Exception("Incorrect footer Length:" + bankFileFooterData.Length));
            return bankFileFooterData;

        }

        public override string ActualBankFileName()
        {
            //  Filename must be within 32 characters
            if (string.IsNullOrEmpty(m_actualFileName))
                m_actualFileName = base.ActualBankFileName();
            return m_actualFileName;
        }

        public override string BankFileExtension()
        {
            return ".csv";
        }
    }
}