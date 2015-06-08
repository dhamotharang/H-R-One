using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;
using HROne.DataAccess;
using HROne.SaaS.Entities;

namespace HROne.SaaS.FileExchangeInterface
{
    public class HSBCAutoPayInstructionFileProces
    {
        DateTime m_SubmissionCutOffDateTime;
        string m_DefaultBankFilePath;
        string m_FileOutputFolder;
        string m_VendorCode;
        DatabaseConnection m_dbConn = null;
        public HSBCEnvironmentIndicatorEnum Environment = HSBCEnvironmentIndicatorEnum.Production;

        public HSBCAutoPayInstructionFileProces(DatabaseConnection dbConn, string DefaultBankFilePath, string FileOutputFolder, string VendorCode, DateTime SubmissionCutOffDateTime)
        {
            m_dbConn = dbConn;
            m_DefaultBankFilePath = DefaultBankFilePath;
            m_FileOutputFolder = FileOutputFolder;
            m_VendorCode = VendorCode;
            m_SubmissionCutOffDateTime = SubmissionCutOffDateTime;
        }
        public string[] CreateOutput()
        {

            string outputPathHSBC = System.IO.Path.Combine(m_FileOutputFolder, "APSMPFI." + m_VendorCode + "." + m_SubmissionCutOffDateTime.ToString("yyyyMMdd") + ".M60");
            string outputPathHASE = System.IO.Path.Combine(m_FileOutputFolder, "APSMPFI." + m_VendorCode + "." + m_SubmissionCutOffDateTime.ToString("yyyyMMdd") + ".E60");

            HSBCFileExchangeProcess HSBCAutoPayFileExchange = new HSBCFileExchangeProcess();
            HSBCAutoPayFileExchange.FileID = HSBCFileIDEnum.AISTN;
            HSBCAutoPayFileExchange.Environment = Environment;
            HSBCAutoPayFileExchange.CreateWriter(outputPathHSBC);

            HSBCFileExchangeProcess HangSengAutoPayFileExchange = new HSBCFileExchangeProcess();
            HangSengAutoPayFileExchange.FileID = HSBCFileIDEnum.AISTN;
            HangSengAutoPayFileExchange.Environment = Environment;
            HangSengAutoPayFileExchange.CreateWriter(outputPathHASE);

            DBFilter filter = new DBFilter();
            filter.add(new NullTerm("CompanyAutopayFileConsolidateDateTime"));
            filter.add(new Match("CompanyAutopayFileConfirmDateTime", "<=", m_SubmissionCutOffDateTime));

            ArrayList list = ECompanyAutopayFile.db.select(m_dbConn, filter);
            foreach (ECompanyAutopayFile autoPayFile in list)
            {
                string transactionRefreence = "HREXA" + autoPayFile.CompanyAutopayFileID.ToString("0000000000");
                transactionRefreence += CheckDigit(transactionRefreence);

                HSBCFileExchangeProcess currentFileProcess = null;
                if (autoPayFile.CompanyAutopayFileBankCode.Equals("004"))
                    currentFileProcess = HSBCAutoPayFileExchange;
                else if (autoPayFile.CompanyAutopayFileBankCode.Equals("024"))
                    currentFileProcess = HangSengAutoPayFileExchange;

                EHSBCExchangeProfile exchangeProfile = new EHSBCExchangeProfile();
                exchangeProfile.HSBCExchangeProfileID = autoPayFile.HSBCExchangeProfileID;
                EHSBCExchangeProfile.db.select(m_dbConn, exchangeProfile);
                string[] submissionHeader = new string[6];
                submissionHeader[0] = "S";
                submissionHeader[1] = exchangeProfile.HSBCExchangeProfileRemoteProfileID.PadRight(18).Substring(0, 18);
                submissionHeader[2] = string.Empty.PadRight(28);
                submissionHeader[3] = transactionRefreence.PadRight(16).Substring(0, 16);
                submissionHeader[4] = "   ";
                submissionHeader[5] = autoPayFile.CompanyAutopayFileConfirmDateTime.ToString("yyyyMMddHHmmss");

                string strSubmissionHeader = string.Join(string.Empty, submissionHeader);
                if (strSubmissionHeader.Length != currentFileProcess.RecordLength)
                    throw new Exception("Invalid submission header length");

                currentFileProcess.AddLine(strSubmissionHeader);

                string currentBankFilePath = System.IO.Path.Combine(m_DefaultBankFilePath, autoPayFile.CompanyAutopayFileDataFileRelativePath);
                FileInfo fileInfo = new System.IO.FileInfo(currentBankFilePath);

                StreamReader bankFileStream = fileInfo.OpenText();
                char[] charRead = new char[80];

                try
                {
                    while (bankFileStream.Read(charRead, 0, 80) > 0)
                    {
                        string line = new string(charRead);
                        currentFileProcess.AddLine(line);
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    bankFileStream.Close();
                }
                if (Environment == HSBCEnvironmentIndicatorEnum.Production)
                {
                    autoPayFile.CompanyAutopayFileConsolidateDateTime = AppUtils.ServerDateTime();
                    autoPayFile.CompanyAutopayFileTransactionReference = transactionRefreence;
                    ECompanyAutopayFile.db.update(m_dbConn, autoPayFile);
                }
            }
            HSBCAutoPayFileExchange.Close();
            HangSengAutoPayFileExchange.Close();
            return new string[] { outputPathHSBC, outputPathHASE };
        }

        public string CheckDigit(string originalString)
        {
            char[] charArray = originalString.ToCharArray();
            int sum = 0;
            foreach (char ch in charArray)
                sum += Convert.ToInt32(ch);
            int checkSum = sum % 10;
            return checkSum.ToString();
        }
    }
}
