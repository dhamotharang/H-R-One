using System;
using System.Data;
using System.Configuration;
using System.IO;
using HROne.DataAccess;
using HROne.Lib.Entities;

namespace HROne.BankFile
{
    /// <summary>
    /// Summary description for SCBiPaymentBankFile
    /// </summary>
    public class SCBiPaymentBankFile : GenericBankFile
    {
        private const string FIELD_DELIMITER = ",";
        private const string RECORD_DELIMITER = "\r\n";
        private const string QUOTATION_DELIMITER = "\"";//double quotations

        protected string m_PaymentCustomerReference;
        protected string m_PaymentCustomerMemo;

        public string PaymentCustomerReference
        {
            get { return m_PaymentCustomerReference; }
            set { m_PaymentCustomerReference= value; }
        }

        public string PaymentCustomerMemo
        {
            get { return m_PaymentCustomerMemo; }
            set { m_PaymentCustomerMemo = value; }
        }

        public SCBiPaymentBankFile(DatabaseConnection dbConn)
            : base(dbConn)
        {
        }

        public override FileInfo GenerateBankFile()
        {
            string[] bankFileHeader = new string[2];
            bankFileHeader[0] = "H";
            bankFileHeader[1] = "P";

            string bankFileHeaderDetail = string.Join(FIELD_DELIMITER, bankFileHeader);
            //if (bankFileHeaderDetail.Length != 11)
            //    throw new Exception("Incorrect Bank File Header Length:\r\n" + bankFileHeaderDetail);
            string bankFileData = bankFileHeaderDetail + RECORD_DELIMITER;

            foreach (GenericBankFileDetail bankFileDetail in BankFileDetails)
            {
                bankFileData += GenerateBankFileDetail(bankFileDetail) + RECORD_DELIMITER;
            }

            string[] bankFileFooter = new string[3];
            bankFileFooter[0] = "T";
            bankFileFooter[1] = RecordCount.ToString("0");
            bankFileFooter[2] = TotalAmount.ToString("0.00");
            string bankFileFooterDetail = string.Join(FIELD_DELIMITER, bankFileFooter);
            //if (bankFileFooterDetail.Length != 34)
            //    throw new Exception("Incorrect Bank File Header Length:\r\n" + bankFileFooterDetail);
            bankFileData += bankFileFooterDetail + RECORD_DELIMITER;

            FileInfo result = GenerateTempFileName();
            StreamWriter writer = new StreamWriter(result.OpenWrite());
            writer.Write(bankFileData);
            writer.Close();
            return result;
        }

        protected override string GenerateBankFileDetail(GenericBankFileDetail bankFileDetail)
        {
            string[] bankFileDetailPaymentRecord = new string[63];

            bankFileDetailPaymentRecord[0] = "P";
            bankFileDetailPaymentRecord[1] = "PAY";
            bankFileDetailPaymentRecord[2] = "BA";
            bankFileDetailPaymentRecord[3] = "";
            
            string PaymentReference = m_PaymentCustomerReference.Contains(FIELD_DELIMITER) ? QUOTATION_DELIMITER + m_PaymentCustomerReference + QUOTATION_DELIMITER : m_PaymentCustomerReference;
            if (PaymentReference.Length > 16)
                bankFileDetailPaymentRecord[4] = PaymentReference.Substring(0, 16);
            else
                bankFileDetailPaymentRecord[4] = PaymentReference;

            string PaymentMemo = m_PaymentCustomerMemo.Contains(FIELD_DELIMITER) ? QUOTATION_DELIMITER + m_PaymentCustomerMemo + QUOTATION_DELIMITER : m_PaymentCustomerMemo;

            if (PaymentMemo.Length > 255)
                bankFileDetailPaymentRecord[5] = PaymentMemo.Substring(0, 255);
            else
                bankFileDetailPaymentRecord[5] = PaymentMemo;

            //P7
            bankFileDetailPaymentRecord[6] = "HK";
            //P8
            bankFileDetailPaymentRecord[7] = "HKG";
            //P9
            bankFileDetailPaymentRecord[8] = this.m_BranchCode + this.m_AccountNo;
            //P10
            bankFileDetailPaymentRecord[9] = m_ValueDate.ToString("dd/MM/yyyy");
            //P11
            if (bankFileDetail.EmpBankAccountHolderName.Length > 35)
                bankFileDetailPaymentRecord[10] = bankFileDetail.EmpBankAccountHolderName.ToUpper().Substring(0, 35);
            else
                bankFileDetailPaymentRecord[10] = bankFileDetail.EmpBankAccountHolderName.ToUpper();
            //P12
            bankFileDetailPaymentRecord[11] = "";
            //P13
            bankFileDetailPaymentRecord[12] = "";
            //P14
            bankFileDetailPaymentRecord[13] = "";
            //P15
            bankFileDetailPaymentRecord[14] = "";
            //P16
            if (!string.IsNullOrEmpty(bankFileDetail.BankCode))
            {
                EBankSwift m_swiftObj = EBankSwift.GetObjectByBankCode(dbConn, bankFileDetail.BankCode);
                if (m_swiftObj != null)
                {
                    bankFileDetailPaymentRecord[15] = m_swiftObj.SwiftCode;
                    //P17
                    bankFileDetailPaymentRecord[16] = m_swiftObj.LocalClearingCode;
                }
                else
                {
                    bankFileDetailPaymentRecord[15] = "";
                    //P17
                    bankFileDetailPaymentRecord[16] = "";
                }
            }
            else
            {
                bankFileDetailPaymentRecord[15] = "";
                //P17
                bankFileDetailPaymentRecord[16] = "";
            }
            //P18
            bankFileDetailPaymentRecord[17] = "";
            //P19 - kay insist P19 is empty
            bankFileDetailPaymentRecord[18] = "";
            //P20
            bankFileDetailPaymentRecord[19] = bankFileDetail.BranchCode + bankFileDetail.AccountNo;
            //P21
            bankFileDetailPaymentRecord[20] = "";
            //P22
            bankFileDetailPaymentRecord[21] = "";
            for (int i = 22; i <= 36; i++)
            {
                bankFileDetailPaymentRecord[i] = "";
            }
            bankFileDetailPaymentRecord[37] = "HKD";
            bankFileDetailPaymentRecord[38] = bankFileDetail.Amount.ToString("0.00");
            for (int j = 39; j <= 62; j++)
            {
                bankFileDetailPaymentRecord[j] = "";
            }

            string bankFileDetailData = String.Join(FIELD_DELIMITER, bankFileDetailPaymentRecord);

            //if (bankFileDetailData.Length != 262)
            //    throw new Exception("Incorrect Bank File Detail Length:\r\n" + bankFileDetailData);
            return bankFileDetailData;
        }
    }    
}