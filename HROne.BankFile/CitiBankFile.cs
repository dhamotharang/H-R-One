using System;
using System.Data;
using System.Configuration;
using System.IO;
using HROne.DataAccess;

namespace HROne.BankFile
{
    /// <summary>
    /// Summary description for CitibankFile
    /// </summary>
    public class CitiBankFile :GenericBankFile 
    {
        private const string FIELD_DELIMITER = "@";
        private const string RECORD_DELIMITER = "\r\n";

        public enum BankFileFormatEnum
        {
            EXCEL=0,
            TEXT=1
        }

        public BankFileFormatEnum BankFileFormat;

        protected string m_ProductCode;
        public string ProductCode
        {
            get { return m_ProductCode; }
            set { m_ProductCode = value; }
        }
        protected string m_PaymentDetails;
        public string PaymentDetails
        {
            get { return m_PaymentDetails; }
            set { m_PaymentDetails = value; }
        }

        public CitiBankFile(DatabaseConnection dbConn, bool IsGenerateChequePayment)
            : base(dbConn)
        {
            m_IsGenerateChequePayment = IsGenerateChequePayment;
        }

        public override FileInfo GenerateBankFile()
        {
            return GenerateBankFile(false);
        }

        public FileInfo GenerateBankFile(bool IsIncludeCheque)
        {
            if (BankFileFormat.Equals(BankFileFormatEnum.EXCEL))
            {
                return GenerateBankFileExcel();
            }
            else
            {
                //  no header for CitiBank Bank File format
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
        }
        public FileInfo GenerateBankFileExcel()
        {
            if (BankFileDetails.Count > 0)
            {
                FileInfo result = GenerateTempFileName();
                string exportFileName = result.FullName;
                result.Delete();
                //System.IO.File.Copy(Server.MapPath("~/template/HistoryList_Template.xls"), exportFileName, true);
                HROne.Export.ExcelExport export = new HROne.Export.ExcelExport(exportFileName);
                System.Data.DataSet dataSet = new System.Data.DataSet();//export.GetDataSet();
                DataTable dataTable = new DataTable("BankFile$");
                dataSet.Tables.Add(dataTable);
                dataTable.Columns.Add("Product Code", typeof(string));
                dataTable.Columns.Add("Debit Acct Country Code", typeof(string));
                dataTable.Columns.Add("Debit Acct Number", typeof(string));
                dataTable.Columns.Add("Payment Currency", typeof(string));
                dataTable.Columns.Add("Payment Amount", typeof(double));
                dataTable.Columns.Add("Value Date", typeof(string));
                dataTable.Columns.Add("Transacation Reference Num", typeof(string));
                dataTable.Columns.Add("Ordering Party Name", typeof(string));
                dataTable.Columns.Add("Beneficiary Name", typeof(string));
                dataTable.Columns.Add("Beneficiary Account", typeof(string));
                dataTable.Columns.Add("Payment Details", typeof(string));
                foreach (GenericBankFileDetail bankFileDetail in BankFileDetails)
                {
                    DataRow row = dataTable.NewRow();
                    if (string.IsNullOrEmpty(ProductCode) && !bankFileDetail.IsChequePayment)
                        continue;
                    if (bankFileDetail.IsChequePayment)
                    {
                        row["Product Code"] = "PLC";
                        row["Beneficiary Name"] = bankFileDetail.EmpName;
                        row["Beneficiary Account"] = string.Empty;
                    }
                    else
                    {
                        row["Product Code"] = m_ProductCode;
                        row["Beneficiary Name"] = bankFileDetail.EmpBankAccountHolderName.PadRight(70).Substring(0, 70).Trim();
                        row["Beneficiary Account"] = bankFileDetail.BankCode.PadRight(3).Substring(0, 3) + "-" + bankFileDetail.BranchCode.PadRight(3).Substring(0, 3) + "-" + bankFileDetail.AccountNo;
                    }

                    row["Debit Acct Country Code"] = "HK";
                    row["Debit Acct Number"] = m_BranchCode + m_AccountNo;
                    row["Payment Currency"] = "HKD";
                    row["Payment Amount"] = bankFileDetail.Amount;
                    row["Value Date"] = m_ValueDate.ToString("yyyyMMdd");
                    row["Transacation Reference Num"] = bankFileDetail.EmpNo.PadRight(15).Substring(0, 15).Trim();
                    row["Ordering Party Name"] = m_AccountHolderName;
                    row["Payment Details"] = m_PaymentDetails;

                    dataTable.Rows.Add(row);
                }
                export.Update(dataSet);

                return result;
            }
            else
                return null;

        }

        protected override string GenerateBankFileDetail(GenericBankFileDetail bankFileDetail)
        {
            if (BankFileFormat.Equals(BankFileFormatEnum.EXCEL))
            {
                return string.Empty;
            }
            else
                return GenerateBankFileDetailText(bankFileDetail);
        }

        public string GenerateBankFileDetailText(GenericBankFileDetail bankFileDetail)
        {
            if (string.IsNullOrEmpty(ProductCode) && !bankFileDetail.IsChequePayment)
                return string.Empty;
            string[] bankFileDetailPaymentRecord = new string[113];
            if (bankFileDetail.IsChequePayment)
            {
                bankFileDetailPaymentRecord[0] = "PLC";
                bankFileDetailPaymentRecord[19] = bankFileDetail.EmpName;
                bankFileDetailPaymentRecord[33] = string.Empty;
            }
            else
            {
                bankFileDetailPaymentRecord[0] = m_ProductCode;
                bankFileDetailPaymentRecord[19] = bankFileDetail.EmpBankAccountHolderName.ToUpper().PadRight(20).Substring(0, 20).Trim();
                bankFileDetailPaymentRecord[33] = bankFileDetail.BankCode.PadRight(3).Substring(0, 3) + "-" + bankFileDetail.BranchCode.PadRight(3).Substring(0, 3) + "-" + bankFileDetail.AccountNo; ;
            }
            bankFileDetailPaymentRecord[1] = "HK";
            bankFileDetailPaymentRecord[2] = m_BranchCode + m_AccountNo;
            bankFileDetailPaymentRecord[3] = "HKD";
            bankFileDetailPaymentRecord[4] = bankFileDetail.Amount.ToString("0.00");
            bankFileDetailPaymentRecord[5] = string.Empty;
            bankFileDetailPaymentRecord[6] = m_ValueDate.ToString("yyyyMMdd");
            bankFileDetailPaymentRecord[7] = bankFileDetail.EmpNo.PadRight(15).Substring(0, 15).Trim();
            bankFileDetailPaymentRecord[8] = string.Empty;
            bankFileDetailPaymentRecord[9] = string.Empty;
            bankFileDetailPaymentRecord[10] = string.Empty;
            bankFileDetailPaymentRecord[11] = string.Empty;
            bankFileDetailPaymentRecord[12] = string.Empty;
            bankFileDetailPaymentRecord[13] = m_AccountHolderName.ToUpper().PadRight(20).Substring(0, 20).Trim();
            bankFileDetailPaymentRecord[14] = string.Empty;
            bankFileDetailPaymentRecord[15] = string.Empty;
            bankFileDetailPaymentRecord[16] = string.Empty;
            bankFileDetailPaymentRecord[17] = string.Empty;
            bankFileDetailPaymentRecord[18] = string.Empty;
            bankFileDetailPaymentRecord[20] = string.Empty;
            bankFileDetailPaymentRecord[21] = string.Empty;
            bankFileDetailPaymentRecord[22] = string.Empty;
            bankFileDetailPaymentRecord[23] = string.Empty;
            bankFileDetailPaymentRecord[24] = string.Empty;
            bankFileDetailPaymentRecord[25] = string.Empty;
            bankFileDetailPaymentRecord[26] = string.Empty;
            bankFileDetailPaymentRecord[27] = string.Empty;
            bankFileDetailPaymentRecord[28] = string.Empty;
            bankFileDetailPaymentRecord[29] = string.Empty;
            bankFileDetailPaymentRecord[30] = string.Empty;
            bankFileDetailPaymentRecord[31] = string.Empty;
            bankFileDetailPaymentRecord[32] = string.Empty;
            bankFileDetailPaymentRecord[35] = m_PaymentDetails;

            //bankFileDetailInvoiceRecord[0] = "I";
            //bankFileDetailInvoiceRecord[1] = string.Empty;
            //bankFileDetailInvoiceRecord[2] = string.Empty;
            //bankFileDetailInvoiceRecord[3] = string.Empty;
            //bankFileDetailInvoiceRecord[4] = bankFileDetail.Amount.ToString("0.00");


            string bankFileDetailData = String.Join(FIELD_DELIMITER, bankFileDetailPaymentRecord); //+ FIELD_DELIMITER + RECORD_DELIMITER + String.Join(FIELD_DELIMITER, bankFileDetailInvoiceRecord) + FIELD_DELIMITER;
            return bankFileDetailData;
        }
        public override string BankFileExtension()
        {
            if (BankFileFormat.Equals(BankFileFormatEnum.EXCEL))
            {
                return ".xls";
            }
            else
            {
                return "";
            }
        }
    }


}