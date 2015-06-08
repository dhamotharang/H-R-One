using System;
using System.Collections;
using System.Text;
using System.IO;
using HROne.DataAccess;
using HROne.SaaS.Entities;

namespace HROne.SaaS.FileExchangeInterface
{
    public class HSBCMPFContributionFileProces
    {
        DateTime m_SubmissionCutOffDateTime;
        string m_DefaultBankFilePath;
        string m_FileOutputFolder;
        string m_VendorCode;
        DatabaseConnection m_dbConn = null;
        public HSBCEnvironmentIndicatorEnum Environment = HSBCEnvironmentIndicatorEnum.Production;

        public HSBCMPFContributionFileProces(DatabaseConnection dbConn, string DefaultBankFilePath, string FileOutputFolder, string VendorCode, DateTime SubmissionCutOffDateTime)
        {
            m_dbConn = dbConn;
            m_DefaultBankFilePath = DefaultBankFilePath;
            m_FileOutputFolder = FileOutputFolder;
            m_VendorCode = VendorCode;
            m_SubmissionCutOffDateTime = SubmissionCutOffDateTime;
        }
        public string[] CreateOutput()
        {

            string outputPathAMPFF_HSBC = System.IO.Path.Combine(m_FileOutputFolder, "APSMPFI." + m_VendorCode + "." + m_SubmissionCutOffDateTime.ToString("yyyyMMdd") + ".M50");
            string outputPathAMPFF_HASE = System.IO.Path.Combine(m_FileOutputFolder, "APSMPFI." + m_VendorCode + "." + m_SubmissionCutOffDateTime.ToString("yyyyMMdd") + ".E50");

            string outputPathAMCND_HSBC = System.IO.Path.Combine(m_FileOutputFolder, "APSMPFI." + m_VendorCode + "." + m_SubmissionCutOffDateTime.ToString("yyyyMMdd") + ".M23");
            string outputPathAMCND_HASE = System.IO.Path.Combine(m_FileOutputFolder, "APSMPFI." + m_VendorCode + "." + m_SubmissionCutOffDateTime.ToString("yyyyMMdd") + ".E23");

            
            HSBCFileExchangeProcess HSBCAMPFFFileExchange = new HSBCFileExchangeProcess();
            HSBCAMPFFFileExchange.FileID = HSBCFileIDEnum.AMPFF;
            HSBCAMPFFFileExchange.Environment = Environment;
            HSBCAMPFFFileExchange.CreateWriter(outputPathAMPFF_HSBC);

            HSBCFileExchangeProcess HangSengAMPFFFileExchange = new HSBCFileExchangeProcess();
            HangSengAMPFFFileExchange.FileID = HSBCFileIDEnum.AMPFF;
            HangSengAMPFFFileExchange.Environment = Environment;
            HangSengAMPFFFileExchange.CreateWriter(outputPathAMPFF_HASE);

            HSBCFileExchangeProcess HSBCAMCNDFileExchange = new HSBCFileExchangeProcess();
            HSBCAMCNDFileExchange.FileID = HSBCFileIDEnum.AMCND;
            HSBCAMCNDFileExchange.Environment = Environment;
            HSBCAMCNDFileExchange.CreateWriter(outputPathAMCND_HSBC);

            HSBCFileExchangeProcess HangSengAMCNDFileExchange = new HSBCFileExchangeProcess();
            HangSengAMCNDFileExchange.FileID = HSBCFileIDEnum.AMCND;
            HangSengAMCNDFileExchange.Environment = Environment;
            HangSengAMCNDFileExchange.CreateWriter(outputPathAMCND_HASE);

            DBFilter filter = new DBFilter();
            filter.add(new NullTerm("CompanyMPFFileConsolidateDateTime"));
            filter.add(new Match("CompanyMPFFileConfirmDateTime", "<=", m_SubmissionCutOffDateTime));

            ArrayList list = ECompanyMPFFile.db.select(m_dbConn, filter);
            foreach (ECompanyMPFFile mpfFile in list)
            {
                string transactionRefreence = "HREXM" + mpfFile.CompanyMPFFileID.ToString("0000000000");
                transactionRefreence += CheckDigit(transactionRefreence);

                HSBCFileExchangeProcess currentFileProcess = null;

                if (mpfFile.CompanyMPFFileFileType.Equals("AMPFF"))
                {
                    if (mpfFile.CompanyMPFFileTrusteeCode.Equals("HSBC"))
                        currentFileProcess = HSBCAMPFFFileExchange;
                    else if (mpfFile.CompanyMPFFileTrusteeCode.Equals("HangSeng"))
                        currentFileProcess = HangSengAMPFFFileExchange;

                    EHSBCExchangeProfile exchangeProfile = new EHSBCExchangeProfile();
                    exchangeProfile.HSBCExchangeProfileID = mpfFile.HSBCExchangeProfileID;
                    EHSBCExchangeProfile.db.select(m_dbConn, exchangeProfile);
                    string[] submissionHeader = new string[6];
                    submissionHeader[0] = "S";
                    submissionHeader[1] = exchangeProfile.HSBCExchangeProfileRemoteProfileID.PadRight(18).Substring(0, 18);
                    submissionHeader[2] = string.Empty.PadRight(28);
                    submissionHeader[3] = transactionRefreence.PadRight(16).Substring(0, 16);
                    submissionHeader[4] = "   ";
                    submissionHeader[5] = mpfFile.CompanyMPFFileConfirmDateTime.ToString("yyyyMMddHHmmss");

                    string strSubmissionHeader = string.Join(string.Empty, submissionHeader);
                    if (strSubmissionHeader.Length != currentFileProcess.RecordLength)
                        throw new Exception("Invalid submission header length");

                    currentFileProcess.AddLine(strSubmissionHeader);


                    string currentBankFilePath = System.IO.Path.Combine(m_DefaultBankFilePath, mpfFile.CompanyMPFFileDataFileRelativePath);
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
                }
                else if (mpfFile.CompanyMPFFileFileType.Equals("AMCND"))
                {
                    if (mpfFile.CompanyMPFFileTrusteeCode.Equals("HSBC"))
                        currentFileProcess = HSBCAMCNDFileExchange;
                    else if (mpfFile.CompanyMPFFileTrusteeCode.Equals("HangSeng"))
                        currentFileProcess = HangSengAMCNDFileExchange;

                    ECompanyDatabase companyDB = new ECompanyDatabase();
                    companyDB.CompanyDBID = mpfFile.CompanyDBID;
                    ECompanyDatabase.db.select(m_dbConn, companyDB);
                    //string[] submissionHeader = new string[7];
                    //submissionHeader[0] = "S";
                    //submissionHeader[1] = companyDB.CompanyDBClientCode.PadRight(18).Substring(0, 18);
                    //submissionHeader[2] = string.Empty.PadRight(28);
                    //submissionHeader[3] = transactionRefreence.PadRight(16).Substring(0, 16);
                    //submissionHeader[4] = "   ";
                    //submissionHeader[5] = mpfFile.CompanyMPFFileConfirmDateTime.ToString("yyyyMMddHHmmss");
                    //submissionHeader[6] = string.Empty.PadRight(1420);

                    //string strSubmissionHeader = string.Join(string.Empty, submissionHeader);
                    //if (strSubmissionHeader.Length != currentFileProcess.RecordLength)
                    //    throw new Exception("Invalid submission header length");

                    //currentFileProcess.AddLine(strSubmissionHeader);


                    string currentBankFilePath = System.IO.Path.Combine(m_DefaultBankFilePath, mpfFile.CompanyMPFFileDataFileRelativePath);
                    FileInfo fileInfo = new System.IO.FileInfo(currentBankFilePath);

                    StreamReader bankFileStream = fileInfo.OpenText();

                    try
                    {
                        while (!bankFileStream.EndOfStream)
                        {
                            string line = bankFileStream.ReadLine();
                            if (!line.StartsWith("HEADER "))
                                currentFileProcess.AddLine(line.PadRight(currentFileProcess.RecordLength));
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
                }
                if (Environment == HSBCEnvironmentIndicatorEnum.Production)
                {
                    mpfFile.CompanyMPFFileConsolidateDateTime = AppUtils.ServerDateTime();
                    mpfFile.CompanyMPFFileTransactionReference = transactionRefreence;
                    ECompanyMPFFile.db.update(m_dbConn, mpfFile);
                }
            }
            HSBCAMPFFFileExchange.Close();
            HangSengAMPFFFileExchange.Close();
            HSBCAMCNDFileExchange.Close();
            HangSengAMCNDFileExchange.Close();


            return new string[] { outputPathAMPFF_HSBC, outputPathAMPFF_HASE, outputPathAMCND_HSBC, outputPathAMCND_HASE };
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
