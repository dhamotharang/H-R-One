using System;
using System.Collections;
using System.Data;
using System.Configuration;
using System.IO;
using HROne.DataAccess;

namespace HROne.BankFile
{
    /// Summary description for HSBCBankFile
    /// </summary>
    public class ANZBankFile : GenericBankFile 
    {
        private const string FIELD_DELIMITER = "";
        private const string RECORD_DELIMITER = "\r\n"; 
        
        protected string m_MyProductCode;
        public string MyProductCode
        {
            get { return m_MyProductCode; }
            set { m_MyProductCode = value; }
        }

        protected string m_PaymentProduct;
        public string PaymentProduct
        {
            get { return m_PaymentProduct; }
            set { m_PaymentProduct = value; }
        }

        protected string m_ClientCode;
        public string ClientCode
        {
            get { return m_ClientCode; }
            set { m_ClientCode = value; }
        }
        
        //protected string m_FirstPartyReference;
        //public string FirstPartyReference
        //{
        //    get { return m_FirstPartyReference; }
        //    set { m_FirstPartyReference = value; }
        //}

        public ANZBankFile(DatabaseConnection dbConn)
            : base(dbConn)
        {
        }

        protected override string GenerateBankFileHeader()
        {
            string[] bankFileHeader = new string[11];

            bankFileHeader[0] = "H";
            bankFileHeader[1] = m_ClientCode.PadRight(10).Substring(0, 10);
            bankFileHeader[2] = m_MyProductCode.PadRight(10).Substring(0, 10);
            bankFileHeader[3] = string.Empty.PadRight(1);
            bankFileHeader[4] = m_PaymentProduct.PadRight(10);
            bankFileHeader[5] = AccountNo.PadRight(20).Substring(0, 20);

            bankFileHeader[6] = ValueDate.ToString("ddMMyyyy");
            bankFileHeader[7] = "ACH".PadRight(20).Substring(0, 20);
            bankFileHeader[8] = string.Empty.PadLeft(40, ' ');
            bankFileHeader[9] = string.Empty.PadLeft(20, ' ');
            bankFileHeader[10] = string.Empty.PadLeft(2085, ' ');

            string bankFileHeaderData = string.Join(FIELD_DELIMITER, bankFileHeader);

            if (bankFileHeaderData.Length != 2225)
                throw (new Exception("Incorrect Header Length:" + bankFileHeaderData.Length));
            return bankFileHeaderData;
        }

        protected override string GenerateBankFileDetail(GenericBankFileDetail bankFileDetail)
        {
            string[] bankFileDetailRecord = new string[81];

            bankFileDetailRecord[0] = "D";
            bankFileDetailRecord[1] = string.Empty.PadRight(20);
            bankFileDetailRecord[2] = "PAYROLL".PadRight(20).Substring(0, 20);
            bankFileDetailRecord[3] = string.Empty.PadRight(10).Substring(0, 10);
            bankFileDetailRecord[4] = string.Empty.PadRight(10);
            //  Trim the account holder name to 20 character even though the max length is 140 characters
            bankFileDetailRecord[5] = bankFileDetail.EmpBankAccountHolderName.PadRight(20).Substring(0, 20).PadRight(140).Substring(0, 140);
            bankFileDetailRecord[6] = string.Empty.PadRight(20);
            bankFileDetailRecord[7] = string.Empty.PadRight(255);

            //  Bank Branch Search - unknown but Mandatory
            bankFileDetailRecord[8] = ((string)(bankFileDetail.BankCode.PadRight(3).Substring(0, 3) + bankFileDetail.BranchCode.PadRight(3).Substring(0, 3))).PadRight(11).Substring(0, 11);
            bankFileDetailRecord[9] = string.Empty.PadRight(40);
            bankFileDetailRecord[10] = string.Empty.PadRight(40);
            bankFileDetailRecord[11] = string.Empty.PadRight(11);
            bankFileDetailRecord[12] = string.Empty.PadRight(150);
            bankFileDetailRecord[13] = string.Empty.PadRight(30);
            bankFileDetailRecord[14] = bankFileDetail.AccountNo.PadRight(9).Substring(0, 9).PadRight(34).Substring(0, 34);
            bankFileDetailRecord[15] = "HKD";
            //  The file spec skip content for line 17 (bankFileDetailRecord[16])
            bankFileDetailRecord[17] = "HONG KONG".PadRight(150).Substring(0, 150); ; //  Address
            bankFileDetailRecord[18] = "HONG KONG".PadRight(30).Substring(0, 30); ; //  Country
            bankFileDetailRecord[19] = string.Empty.PadRight(1337);
            bankFileDetailRecord[57] = "HKD";
            bankFileDetailRecord[58] = " ";
            bankFileDetailRecord[59] = string.Empty.PadRight(10);
            bankFileDetailRecord[60] = "P";
            bankFileDetailRecord[61] = bankFileDetail.Amount.ToString(string.Empty.PadLeft(13, '0') + ".00").Replace(".", string.Empty);
            //  The size of Line 68 Cheque Number is changed from 6 to 10
            bankFileDetailRecord[62] = string.Empty.PadRight(237); 
            bankFileDetailRecord[77] = string.Empty.PadRight(140);  //  Credit Details
            bankFileDetailRecord[78] = string.Empty.PadRight(1250);

            string bankFileDetailData = String.Join(FIELD_DELIMITER, bankFileDetailRecord);
            if (bankFileDetailData.Length != 3969)
                throw (new Exception("Incorrect Detail Length of Payment for " + bankFileDetail.EmpBankAccountHolderName + ":" + bankFileDetailData.Length));
            return bankFileDetailData;
        }

        protected override string GenerateBankFileFooter()
        {
            string[] bankFileFooter = new string[9];
            bankFileFooter[0] = "T";
            bankFileFooter[1] = RecordCount.ToString("00000");
            bankFileFooter[2] = TotalAmount.ToString(string.Empty.PadLeft(13, '0') + ".00").Replace(".", string.Empty);
            bankFileFooter[3] = string.Empty.PadRight(3479);

            string bankFileFooterData = string.Join(FIELD_DELIMITER, bankFileFooter);

            if (bankFileFooterData.Length != 3500)
                throw (new Exception("Incorrect Header Length:" + bankFileFooterData.Length));
            return bankFileFooterData;

        }

        public override string BankFileExtension()
        {
                return ".txt";
        }
    }
}