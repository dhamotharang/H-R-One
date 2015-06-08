using System;
using System.Collections.Generic;
using System.Data;
using System.Configuration;
using System.IO;
using HROne.DataAccess;

namespace HROne.BankFile
{
    /// Summary description for HSBCBankFile
    /// </summary>
    public class HSBCBankFile : GenericBankFile 
    {
        private const string FIELD_DELIMITER = "";
        private const string RECORD_DELIMITER = ""; 
        
        protected string m_PlanCode;
        public string PlanCode
        {
            get { return m_PlanCode; }
            set { m_PlanCode = value; }
        }

        protected string m_BankPaymentCode;
        public string BankPaymentCode
        {
            get { return m_BankPaymentCode; }
            set { m_BankPaymentCode = value; }
        }
        
        protected string m_FirstPartyReference;
        public string FirstPartyReference
        {
            get { return m_FirstPartyReference; }
            set { m_FirstPartyReference = value; }
        }

        protected bool m_UseBIBFormat;
        public bool UseBIBFormat
        {
            get { return m_UseBIBFormat; }
            set { m_UseBIBFormat = value; }
        }

        protected string[] m_MultipleBankPaymentCode;
        public string[] MultipleBankPaymentCode
        {
            get { return m_MultipleBankPaymentCode; }
            set { m_MultipleBankPaymentCode = value; }
        }

        public HSBCBankFile(DatabaseConnection dbConn)
            : base(dbConn)
        {
        }

        public HSBCBankFile(HSBCBankFile templateBankFile)
            :base(templateBankFile.dbConn)
        {
            this.AccountHolderName = templateBankFile.AccountHolderName;
            this.AccountNo = templateBankFile.AccountNo;
            this.BankCode = templateBankFile.BankCode;
            this.BranchCode = templateBankFile.BranchCode;
            //this.dbConn = templateBankFile.dbConn;
            this.FirstPartyReference = templateBankFile.FirstPartyReference;
            this.m_IsGenerateChequePayment = templateBankFile.IsGenerateChequePayment;
            this.PlanCode = templateBankFile.PlanCode;
            this.UseBIBFormat = false;
            this.ValueDate = templateBankFile.ValueDate;
        }

        public DateTime GetFirstValueDate()
        {
            DateTime tmpLastValueDate = m_ValueDate;
            foreach (GenericBankFileDetail bankFileDetail in BankFileDetails)
            {
                if (tmpLastValueDate.Ticks.Equals(0) || (tmpLastValueDate > bankFileDetail.ValueDate && !bankFileDetail.ValueDate.Ticks.Equals(0)))
                    tmpLastValueDate = bankFileDetail.ValueDate;
            }
            return tmpLastValueDate;
        }

        public void LoadBankFileDetail(string BankFileFullPath)
        {
            m_TotalAmount = 0;

            int tmpHeaderRecordCount = -1;
            double tmpHeaderTotalAmount = -1;
            DateTime currentDateTime = AppUtils.ServerDateTime();

            //int RecordCount = 0;
            //double TotalAmount = 0;

            FileInfo fileInfo = new FileInfo(BankFileFullPath);
            if (fileInfo.Length % 80 != 0)
                throw new Exception("Incorrect file size");

            StreamReader bankFileStream = fileInfo.OpenText();
            char[] charRead = new char[80];

            int lineNo = 0;

            try
            {
                while (bankFileStream.Read(charRead, 0, 80) > 0)
                {
                    string line = new string(charRead);
                    lineNo++;

                    if (lineNo == 1)
                    {
                        string[] bankFileHeader = new string[13];
                        bankFileHeader[0] = line.Substring(0, 1);
                        bankFileHeader[1] = line.Substring(1, 12);
                        bankFileHeader[2] = line.Substring(13, 3);
                        bankFileHeader[3] = line.Substring(16, 12);
                        bankFileHeader[4] = line.Substring(28, 6);
                        bankFileHeader[5] = line.Substring(34, 1);
                        bankFileHeader[6] = line.Substring(35, 8);
                        bankFileHeader[7] = line.Substring(43, 5);
                        bankFileHeader[8] = line.Substring(48, 10);
                        bankFileHeader[9] = line.Substring(58, 7);
                        bankFileHeader[10] = line.Substring(65, 12);
                        bankFileHeader[11] = line.Substring(77, 2);
                        bankFileHeader[12] = line.Substring(79, 1);

                        m_PlanCode = bankFileHeader[0];
                        m_BranchCode = bankFileHeader[1].Substring(0, 3).Trim();
                        m_AccountNo = bankFileHeader[1].Substring(3, 9).Trim();
                        m_BankPaymentCode = bankFileHeader[2].Trim();
                        m_FirstPartyReference = bankFileHeader[3].Trim();
                        if (m_PlanCode == "F" || m_PlanCode == "G")
                        {
                            if (!DateTime.TryParseExact(bankFileHeader[4], "ddMMyy", null, System.Globalization.DateTimeStyles.None, out m_ValueDate))
                                throw new Exception("Invalid value date on file header");
                        }
                        if (!bankFileHeader[5].Equals("K"))
                            throw new Exception("Invalid media indicator");
                        if (!bankFileHeader[6].Equals(string.Empty.PadLeft(8, '*')))
                            throw new Exception("Invalid filename on file header");
                        {
                            if (!bankFileHeader[7].Trim().Equals(string.Empty) && !bankFileHeader[8].Trim().Equals(string.Empty))
                                if (!int.TryParse(bankFileHeader[7], out tmpHeaderRecordCount) || !double.TryParse(bankFileHeader[8].Insert(8, "."), out tmpHeaderTotalAmount))
                                {
                                    tmpHeaderRecordCount = -1;
                                    tmpHeaderTotalAmount = -1;
                                }
                            if (!bankFileHeader[9].Trim().Equals(string.Empty) && !bankFileHeader[10].Trim().Equals(string.Empty))
                            {
                                if (tmpHeaderRecordCount > 0 && tmpHeaderTotalAmount > 0)
                                    throw new Exception("Invalid Record Count/Total Amount on file header");
                                if (!int.TryParse(bankFileHeader[9], out tmpHeaderRecordCount) || !double.TryParse(bankFileHeader[10].Insert(10, "."), out tmpHeaderTotalAmount))
                                    throw new Exception("Invalid Record Count/Total Amount on file header");
                            }
                        }
                        if (!bankFileHeader[12].Equals("1"))
                            throw new Exception("Invalid end mark on file header");
                    }
                    else
                    {
                        string[] bankFileDetailRecord = new string[10];

                        bankFileDetailRecord[0] = line.Substring(0, 1);
                        bankFileDetailRecord[1] = line.Substring(1, 12);
                        bankFileDetailRecord[2] = line.Substring(13, 20);
                        bankFileDetailRecord[3] = line.Substring(33, 3);
                        bankFileDetailRecord[4] = line.Substring(36, 3);
                        bankFileDetailRecord[5] = line.Substring(39, 9);
                        bankFileDetailRecord[6] = line.Substring(48, 10);
                        bankFileDetailRecord[7] = line.Substring(58, 4);
                        bankFileDetailRecord[8] = line.Substring(62, 6);
                        bankFileDetailRecord[9] = line.Substring(68, 12);

                        GenericBankFileDetail bankFileDetail = new GenericBankFileDetail();
                        if (!bankFileDetailRecord[0].Equals(" "))
                            throw new Exception("Invalid record header on record detail" + " (line " + lineNo + ")");

                        bankFileDetail.EmpNo = bankFileDetailRecord[1] + bankFileDetailRecord[8];
                        bankFileDetail.EmpNo = bankFileDetail.EmpNo.Trim();
                        bankFileDetail.EmpBankAccountHolderName = bankFileDetailRecord[2].Trim();
                        bankFileDetail.BankCode = bankFileDetailRecord[3].Trim();
                        bankFileDetail.BranchCode = bankFileDetailRecord[4].Trim();
                        bankFileDetail.AccountNo = bankFileDetailRecord[5].Trim();
                        double tmpRecordAmount = 0;
                        if (!double.TryParse(bankFileDetailRecord[6].Insert(8, "."), out tmpRecordAmount))
                            throw new Exception("Invalid amount on record detail" + " (line " + lineNo + ")");
                        bankFileDetail.Amount = tmpRecordAmount;

                        int valueDateNumeric = 0;
                        if (m_PlanCode == "F" || m_PlanCode == "G")
                        {
                            if (!bankFileDetailRecord[7].Trim().Equals(string.Empty))
                                throw new Exception("Invalid value date on record detail" + " (line " + lineNo + ")");
                        }
                        else
                        {
                            if (!int.TryParse(bankFileDetailRecord[7], out valueDateNumeric))
                                throw new Exception("Invalid value date on record detail" + " (line " + lineNo + ")");
                            int days = valueDateNumeric / 100;
                            int month = valueDateNumeric % 100;
                            bankFileDetail.ValueDate = new DateTime(currentDateTime.Year, month, days);
                            //  find a value date which is nearest to the current date
                            if (bankFileDetail.ValueDate < currentDateTime.AddDays(-180))
                                //  to prevent 29 Feb issue, use "new DateTime" to re-assign the value
                                bankFileDetail.ValueDate = new DateTime(currentDateTime.Year + 1, month, days);
                            if (bankFileDetail.ValueDate.Day != days && bankFileDetail.ValueDate.Month != month)
                                throw new Exception("Invalid value date on record detail" + " (line " + lineNo + ")");

                        }
                        m_TotalAmount += bankFileDetail.Amount;
                        BankFileDetails.Add(bankFileDetail);
                    }
                }

                if (m_TotalAmount - tmpHeaderTotalAmount >= 0.01 || RecordCount != tmpHeaderRecordCount)
                    throw new Exception("Total Amount / Record Count does not match with File Header");
            }
            finally
            {
                bankFileStream.Close();
            }
        }

        protected FileInfo GenerateSingleBankFile(string BankPaymentCode, List<GenericBankFileDetail> BankFileDetails)
        {
            int RecordCount = 0;
            double TotalAmount = 0;
            string bankFileData = string.Empty;
            foreach (GenericBankFileDetail bankFileDetail in BankFileDetails)
            {
                bankFileData += GenerateBankFileDetail(bankFileDetail);
                TotalAmount += bankFileDetail.Amount;
                RecordCount++;
            }


            string[] bankFileHeader = new string[13];

            bankFileHeader[0] = m_PlanCode.PadRight(1).Substring(0, 1);
            bankFileHeader[1] = m_BranchCode.PadRight(3).Substring(0, 3) + m_AccountNo.PadRight(9).Substring(0, 9);
            bankFileHeader[2] = BankPaymentCode.PadRight(3).Substring(0, 3);
            bankFileHeader[3] = m_FirstPartyReference.PadRight(12).Substring(0, 12);

            if (m_PlanCode == "F" || m_PlanCode == "G")
                bankFileHeader[4] = ValueDate.ToString("ddMMyy");
            else
                bankFileHeader[4] = string.Empty.PadLeft(6, ' ');
            //  Media: K - Diskette
            bankFileHeader[5] = "K";
            bankFileHeader[6] = string.Empty.PadLeft(8, '*');
            if (RecordCount >= 100000 || TotalAmount >= 99999999.99)
            {
                bankFileHeader[7] = string.Empty.PadLeft(5, ' ');
                bankFileHeader[8] = string.Empty.PadLeft(10, ' ');
                bankFileHeader[9] = RecordCount.ToString("0000000");
                bankFileHeader[10] = TotalAmount.ToString("0000000000.00").Replace(".", "");
            }
            else
            {
                bankFileHeader[7] = RecordCount.ToString("00000");
                bankFileHeader[8] = TotalAmount.ToString("00000000.00").Replace(".", "");
                bankFileHeader[9] = string.Empty.PadLeft(7, ' ');
                bankFileHeader[10] = string.Empty.PadLeft(12, ' ');
            }
            bankFileHeader[11] = "  ";
            bankFileHeader[12] = "1";

            string bankFileHeaderData = string.Join(FIELD_DELIMITER, bankFileHeader) + RECORD_DELIMITER;

            if (bankFileHeaderData.Length != 80)
                throw (new Exception("Incorrect Header Length:" + bankFileHeaderData.Length));


            FileInfo result = GenerateTempFileName();
            StreamWriter writer = new StreamWriter(result.OpenWrite());
            writer.Write(bankFileHeaderData);
            writer.Write(bankFileData);
            writer.Close();

            //string filename = System.IO.Path.Combine(result.DirectoryName, "BankFile_" + AppUtils.ServerDateTime().ToString("yyyyMMddHHmmss") + "_" + BankPaymentCode + BankFileExtension());
            //int subIndex = 0;
            //while (File.Exists(filename))
            //{
            //    subIndex++;
            //    filename = System.IO.Path.Combine(result.DirectoryName, "BankFile_" + AppUtils.ServerDateTime().ToString("yyyyMMddHHmmss") + "_" + BankPaymentCode + "-" + subIndex + BankFileExtension());
            //}
            //result.MoveTo(filename);
            return result;

        }

        public override FileInfo GenerateBankFile()
        {
            if (!UseBIBFormat)
                return GenerateSingleBankFile(m_BankPaymentCode, BankFileDetails);
            else
            {
                const int MaxRecordSize = 200;
                List<HSBCBankFile> BankFileList = new List<HSBCBankFile>();
                //List<GenericBankFileDetail> tmpBankFileDetails = new List<GenericBankFileDetail>();
                int paymentCodeCount=0;
                int bankFileDetailCount = 0;

                HSBCBankFile dummyBankFile = new HSBCBankFile(this);
                dummyBankFile.BankPaymentCode = m_BankPaymentCode;

                foreach (GenericBankFileDetail bankFileDetail in BankFileDetails)
                {
                    //tmpBankFileDetails.Add(bankFileDetail);
                    //  second condition is to prevent the last bank file contain 160 characters
                    if (dummyBankFile.BankFileDetails.Count >= MaxRecordSize || (dummyBankFile.BankFileDetails.Count.Equals(MaxRecordSize - 1) && BankFileDetails.Count - bankFileDetailCount == 2))
                    {
                        BankFileList.Add(dummyBankFile);

                        paymentCodeCount++;

                        string tmpBankPaymentCode = string.Empty;
                        if (MultipleBankPaymentCode.Length >= paymentCodeCount)
                            tmpBankPaymentCode = MultipleBankPaymentCode[paymentCodeCount - 1];
                        else
                            throw new InvalidFieldValueException("Bank Payment Code is not enough to generate");
                        //tmpBankFileDetails.Clear();
                        dummyBankFile = new HSBCBankFile(this);
                        dummyBankFile.BankPaymentCode = tmpBankPaymentCode;
                    }
                    bankFileDetailCount++;
                    dummyBankFile.BankFileDetails.Add(bankFileDetail);

                }
                if (dummyBankFile.BankFileDetails.Count > 0)
                    BankFileList.Add(dummyBankFile);

                if (BankFileList.Count == 1)
                {
                    UseBIBFormat = false;
                    return GenerateBankFile();
                }
                FileInfo zipFileInfo = new FileInfo(Path.GetTempFileName());
                zipFileInfo.MoveTo(zipFileInfo.FullName + ".zip");
                DirectoryInfo zipFolderInfo = zipFileInfo.Directory.CreateSubdirectory("BankFile" + AppUtils.ServerDateTime().ToString("yyyyMMddHHmmssfff"));
                List<FileInfo> fileInfoList = new List<FileInfo>();
                foreach(HSBCBankFile bankFile in BankFileList)
                {
                    FileInfo fileInfo = bankFile.GenerateBankFile();
                    fileInfo.MoveTo(System.IO.Path.Combine(zipFolderInfo.FullName, bankFile.ActualBankFileName()));
                    fileInfoList.Add(fileInfo);
                }
                zip.Compress(zipFolderInfo.FullName, "*", zipFileInfo.FullName);
                foreach (FileInfo fileInfo in fileInfoList)
                    fileInfo.Delete();
                zipFolderInfo.Delete(true);
                return zipFileInfo;
            }
        }

        protected override string GenerateBankFileDetail(GenericBankFileDetail bankFileDetail)
        {
            string[] bankFileDetailRecord = new string[10];

            bankFileDetailRecord[0] = " ";
            bankFileDetailRecord[1] = bankFileDetail.EmpNo.PadRight(12).Substring(0, 12);
            bankFileDetailRecord[2] = bankFileDetail.EmpBankAccountHolderName.PadRight(20).Substring(0, 20);
            bankFileDetailRecord[3] = bankFileDetail.BankCode.PadRight(3).Substring(0, 3);
            bankFileDetailRecord[4] = bankFileDetail.BranchCode.PadRight(3).Substring(0, 3);
            bankFileDetailRecord[5] = bankFileDetail.AccountNo.PadRight(9).Substring(0, 9);
            bankFileDetailRecord[6] = bankFileDetail.Amount.ToString("00000000.00").Replace(".", "");
            if (m_PlanCode == "F" || m_PlanCode == "G")
                bankFileDetailRecord[7] = string.Empty.PadLeft(4, ' ');
            else
                bankFileDetailRecord[7] = ValueDate.ToString("ddMM");

            bankFileDetailRecord[8] = bankFileDetail.EmpNo.PadRight(20).Substring(12, 6);
            bankFileDetailRecord[9] = string.Empty.PadLeft(12, ' ');

            string bankFileDetailData = String.Join(FIELD_DELIMITER, bankFileDetailRecord);
            if (bankFileDetailData.Length != 80)
                throw (new Exception("Incorrect Detail Length of Payment for " + bankFileDetail.EmpBankAccountHolderName + ":" + bankFileDetailData.Length));
            return bankFileDetailData;
        }
        public override string ActualBankFileName()
        {
            if (!UseBIBFormat)
                return "BankFile_" + AppUtils.ServerDateTime().ToString("yyyyMMddHHmmss") + "_" + BankPaymentCode + BankFileExtension();
            else
                return "BankFile_" + AppUtils.ServerDateTime().ToString("yyyyMMddHHmmss") + BankFileExtension();
        }

        public override string BankFileExtension()
        {
            if (!UseBIBFormat)
                return ".apc";
            else
                return ".zip";
        }
    }
}