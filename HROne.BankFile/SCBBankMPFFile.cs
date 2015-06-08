using System;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using HROne.DataAccess;
using HROne.Lib.Entities;

namespace HROne.BankFile
{
    /// <summary>
    /// Summary description for SCBBankFile
    /// </summary>
    public class SCBBankMPFFile : GenericBankFile
    {
        private const string FIELD_DELIMITER = "";
        private const string RECORD_DELIMITER = "\r\n";

        protected string m_ERCMGReference;
        public string ERCMGReference
        {
            get { return m_ERCMGReference; }
            set { m_ERCMGReference= value; }
        }

        protected bool m_IncludeMPFRecord = false;
        public bool IncludeMPFRecord
        {
            get { return m_IncludeMPFRecord; }
            set { m_IncludeMPFRecord = value; }
        }

        //protected DateTime m_MPFPeriodFrom;
        //public DateTime MPFPeriodFrom
        //{
        //    get { return m_MPFPeriodFrom; }
        //    set { m_MPFPeriodFrom = value; }
        //}

        //protected DateTime m_MPFPeriodTo;
        //public DateTime MPFPeriodTo
        //{
        //    get { return m_MPFPeriodTo; }
        //    set { m_MPFPeriodTo = value; }
        //}

        protected DateTime m_ContributionDueDate;
        public DateTime ContributionDueDate
        {
            get { return m_ContributionDueDate; }
            set { m_ContributionDueDate = value; }
        }

        protected string m_BatchDescription;
        public string BatchDescription
        {
            get { return m_BatchDescription; }
            set { m_BatchDescription = value; }
        }


        //protected System.Collections.Generic.List<int> loadedMPFDetailEmpID = new System.Collections.Generic.List<int>();
        protected const string EmpMPFPlanSCBPFundTypeNodeName = "EmpMPFPlanSCBPFundType";
        protected int payrollMPFRecordCount;
        public SCBBankMPFFile(DatabaseConnection dbConn)
            : base(dbConn)
        {
        }
        protected int NewJoinRecordCount;
        protected int NewJoinEECount = 0;
        protected int totalFirstContributionRecordCount = 0;
        protected double totalFirstContributionAmount= 0;
        protected Dictionary<int, string> mpfPlanIDSchemeCodeMapping = new Dictionary<int, string>();
        protected ArrayList contributionList = new System.Collections.ArrayList();
        protected List<MPFFile.GenericAdditionalEmployeeMPFFileDetail> additionalEEInformation = new List<HROne.MPFFile.GenericAdditionalEmployeeMPFFileDetail>();

        protected double totalMCEE = 0, totalMCER = 0, totalVCEE = 0, totalVCER = 0;

        public override void LoadBankFileDetail(System.Collections.ArrayList PayrollBatchList, System.Collections.ArrayList EmpList)
        {
            base.LoadBankFileDetail(PayrollBatchList, EmpList);


            contributionList.Clear();
            additionalEEInformation.Clear();
            totalMCEE = 0; 
            totalMCER = 0; 
            totalVCEE = 0; 
            totalVCER = 0;
            if (IncludeMPFRecord)
            {
                ArrayList MPFPlanList = EMPFPlan.db.select(dbConn, (DBFilter)null);

                foreach (EMPFPlan mpfPlan in MPFPlanList)
                {
                    EMPFScheme mpfScheme = new EMPFScheme();
                    mpfScheme.MPFSchemeID = mpfPlan.MPFSchemeID;
                    if (EMPFScheme.db.select(dbConn, mpfScheme) && mpfScheme.MPFSchemeTrusteeCode.Equals("RBC", StringComparison.CurrentCultureIgnoreCase))
                    {
                        mpfPlanIDSchemeCodeMapping.Add(mpfPlan.MPFPlanID, mpfScheme.MPFSchemeCode);

                        MPFFile.GenericMPFFile mpfFile = new HROne.MPFFile.GenericMPFFile(dbConn);
                        mpfFile.LoadMPFFileDetail(EmpList, mpfPlan.MPFPlanID, m_PayPeriodFr, m_PayPeriodTo);

                        totalMCEE = mpfFile.TotalMCEE;
                        totalMCER = mpfFile.TotalMCER;
                        totalVCEE = mpfFile.TotalVCEE;
                        totalVCER = mpfFile.TotalVCER;
                        contributionList.AddRange(mpfFile.ExistingEmployeeMPFFileDetails);
                        contributionList.AddRange(mpfFile.NewJoinEmployeeMPFFileDetails);
                        contributionList.AddRange(mpfFile.BackPaymentEmployeeMPFFileDetails);
                        additionalEEInformation.AddRange(mpfFile.AdditionalEmployeeMPFFileDetails);
                    }
                }
            }
        }

        public override FileInfo GenerateBankFile()
        {
            payrollMPFRecordCount = 0;
            NewJoinRecordCount = 0;
            NewJoinEECount = 0;
            totalFirstContributionRecordCount = 0;
            totalFirstContributionAmount = 0;


            string[] bankFileHeader = new string[12];
            bankFileHeader[0] = ERCMGReference.PadRight(20).Substring(0, 20);
            bankFileHeader[1] = m_AccountHolderName.ToUpper().PadRight(40).Substring(0, 40);
            string companyAccountNo = m_AccountNo.PadRight(8).Substring(0, 8);
            companyAccountNo = companyAccountNo.Substring(0, 3) + "-" + companyAccountNo.Substring(3, 4) + "-" + companyAccountNo.Substring(7);
            bankFileHeader[2] = m_BranchCode.PadRight(3).Substring(0, 3) + "-" + companyAccountNo;
            if (contributionList.Count > 0)
            {
                bankFileHeader[3] = "0";
            }
            else
            {
                bankFileHeader[3] = "1";
            }

            bankFileHeader[4] = m_ValueDate.ToString("yyyyMMdd");
            bankFileHeader[5] = m_PayPeriodTo.ToString("yyyyMMdd");
            if (ContributionDueDate.Ticks.Equals(0))
                bankFileHeader[6] = ContributionDueDate.ToString("yyyyMMdd");
            else
                bankFileHeader[6] = string.Empty.PadRight(8);

            bankFileHeader[7] = m_BatchDescription.PadRight(16).Substring(0, 16);
            bankFileHeader[8] = RecordCount.ToString("00000000");
            bankFileHeader[9] = ((double)(m_TotalAmount + totalMCEE + totalVCEE)).ToString("0000000000.00");
            bankFileHeader[10] = m_TotalAmount.ToString("0000000000.00");
            bankFileHeader[11] = ((double)(totalMCEE + totalVCEE + totalMCER + totalVCER)).ToString("0000000000.00");

            string bankFileHeaderDetail = string.Join(FIELD_DELIMITER, bankFileHeader);
            if (bankFileHeaderDetail.Length != 162)
                throw new Exception("Incorrect Bank File Header Length:\r\n" + bankFileHeaderDetail);
            string bankFileData = bankFileHeaderDetail + RECORD_DELIMITER;
            string firstRemittanceStatementData = string.Empty;


            foreach (GenericBankFileDetail bankFileDetail in BankFileDetails)
            {
                string FirstContributionString;
                bankFileData += GenerateBankFileDetailWithContributionDetail(bankFileDetail, contributionList, out FirstContributionString) + RECORD_DELIMITER;

                if (!string.IsNullOrEmpty(FirstContributionString))
                    firstRemittanceStatementData += FirstContributionString;
                payrollMPFRecordCount++;
            }
            System.Collections.ArrayList remainContributionList = (System.Collections.ArrayList)contributionList.Clone();
            foreach (MPFFile.GenericMPFFileDetail mpfDetail in remainContributionList)
            {
                GenericBankFileDetail bankFileDetail = new GenericBankFileDetail();
                bankFileDetail.EmpID = mpfDetail.EmpID;
                bankFileDetail.EmpName = mpfDetail.EmpName;
                bankFileDetail.EmpNo = mpfDetail.EmpNo;
                bankFileDetail.EmpBankAccountHolderName = mpfDetail.EmpName;
                bankFileDetail.BankCode = string.Empty;
                bankFileDetail.BranchCode = string.Empty;
                bankFileDetail.AccountNo = string.Empty;
                bankFileDetail.Amount = 0;
                string FirstContributionString;

                bankFileData += GenerateBankFileDetailWithContributionDetail(bankFileDetail, contributionList, out FirstContributionString) + RECORD_DELIMITER;

                if (!string.IsNullOrEmpty(FirstContributionString))
                    firstRemittanceStatementData += FirstContributionString;
                payrollMPFRecordCount++;
            }

            if (!string.IsNullOrEmpty(firstRemittanceStatementData))
            {
                string[] firstRemittanceFileHeader = new string[8];
                firstRemittanceFileHeader[0] = bankFileHeader[0];
                firstRemittanceFileHeader[1] = bankFileHeader[1];
                firstRemittanceFileHeader[2] = bankFileHeader[2];
                firstRemittanceFileHeader[3] = bankFileHeader[4];
                firstRemittanceFileHeader[4] = m_PayPeriodFr.ToString("yyyyMMdd");
                firstRemittanceFileHeader[5] = m_PayPeriodTo.ToString("yyyyMMdd");
                firstRemittanceFileHeader[6] = totalFirstContributionRecordCount.ToString("00000000");
                firstRemittanceFileHeader[7] = totalFirstContributionAmount.ToString("0000000000.00");
                string firstRemittanceFileHeaderDetail = string.Join(FIELD_DELIMITER, firstRemittanceFileHeader);
                if (firstRemittanceFileHeaderDetail.Length != 119)
                    throw new Exception("Incorrect first contribution remittance File Header Length:\r\n" + firstRemittanceFileHeaderDetail);
                firstRemittanceStatementData = firstRemittanceFileHeaderDetail + RECORD_DELIMITER + firstRemittanceStatementData + char.ConvertFromUtf32(26);

            }
            bankFileData += char.ConvertFromUtf32(26);

            FileInfo bankFileInfo = GenerateTempFileName();
            StreamWriter writer = new StreamWriter(bankFileInfo.OpenWrite());
            writer.Write(bankFileData);
            writer.Close();

            //SaveSequenceNo(m_SequenceNo + 1);

            FileInfo zipFileInfo = new FileInfo(Path.GetTempFileName());
            zipFileInfo.MoveTo(zipFileInfo.FullName + ".zip");
            DirectoryInfo zipFolderInfo = zipFileInfo.Directory.CreateSubdirectory("SCBBankMPFFile" + AppUtils.ServerDateTime().ToString("yyyyMMddHHmmssfff"));
            System.Collections.Generic.List<FileInfo> fileInfoList = new System.Collections.Generic.List<FileInfo>();
            {
                bankFileInfo.MoveTo(System.IO.Path.Combine(zipFolderInfo.FullName, ActualBankMPFFileName()));
                fileInfoList.Add(bankFileInfo);

                if (!string.IsNullOrEmpty(firstRemittanceStatementData))
                {
                    FileInfo firstContributionFileInfo = GenerateTempFileName();
                    writer = new StreamWriter(firstContributionFileInfo.OpenWrite());
                    writer.Write(firstRemittanceStatementData);
                    writer.Close();
                    firstContributionFileInfo.MoveTo(System.IO.Path.Combine(zipFolderInfo.FullName, ActualMPFTerminationFileName()));
                    fileInfoList.Add(firstContributionFileInfo);
                }

            }
            zip.Compress(zipFolderInfo.FullName, "*", zipFileInfo.FullName);
            foreach (FileInfo fileInfo in fileInfoList)
                fileInfo.Delete();
            zipFolderInfo.Delete(true);
            return zipFileInfo;
        }

        protected string GenerateBankFileDetailWithContributionDetail(GenericBankFileDetail bankFileDetail, System.Collections.ArrayList contributionList, out string FirstContributionString)
        {
            EEmpPersonalInfo empInfo = new EEmpPersonalInfo();
            empInfo.EmpID = bankFileDetail.EmpID;
            EEmpPersonalInfo.db.select(dbConn, empInfo);

            string HKIDPassport;
            if (empInfo.EmpHKID.Length < 7)
            {
                HKIDPassport = empInfo.EmpPassportNo.Trim();
                //HKIDType = IDENTITY_TYPE_ENUM.PASSPORT;
            }
            else
            {
                HKIDPassport = empInfo.EmpHKID.Trim();
                //HKIDType = IDENTITY_TYPE_ENUM.HKID;
            }
            if (string.IsNullOrEmpty(HKIDPassport.Replace("()", "").Trim()))
                throw new Exception(HROne.Common.WebUtility.GetLocalizedStringByCode("ERROR_HKID_PASSPORT_REQUIRED", "Either HKID or Passport Number is required") + "(" + HROne.Common.WebUtility.GetLocalizedString("EmpNo") + ": " + empInfo.EmpNo + ")");

            string defaultPFundType = "2";
            EEmpMPFPlan empMPFPlan = (EEmpMPFPlan)AppUtils.GetLastObj(dbConn, EEmpMPFPlan.db, "EmpMPFEffFr", empInfo.EmpID);
            if (empMPFPlan != null)
            {
                string pFundType = HROne.CommonLib.Utility.GetXMLElementFromXmlString(empMPFPlan.EmpMPFPlanExtendData, "EmpMPFPlanExtendData", EmpMPFPlanSCBPFundTypeNodeName);
                if (!string.IsNullOrEmpty(pFundType.Trim()))
                    defaultPFundType = pFundType;
            }

            string[] bankFileDetailPaymentRecord = new string[22];
            bankFileDetailPaymentRecord[0] = bankFileDetail.EmpNo.PadRight(10).Substring(0, 10);
            bankFileDetailPaymentRecord[1] = bankFileDetail.EmpName.ToUpper().PadRight(20).Substring(0, 20);
            bankFileDetailPaymentRecord[2] = HKIDPassport.PadRight(15).Substring(0, 15);
            bankFileDetailPaymentRecord[3] = defaultPFundType;
            if (!string.IsNullOrEmpty(bankFileDetail.BankCode) || !string.IsNullOrEmpty(bankFileDetail.BranchCode) || !string.IsNullOrEmpty(bankFileDetail.AccountNo))
                bankFileDetailPaymentRecord[4] = bankFileDetail.BankCode.PadRight(3).Substring(0, 3) + "-" + bankFileDetail.BranchCode.PadRight(3).Substring(0, 3) + bankFileDetail.AccountNo.PadRight(11).Substring(0, 11);
            else
                bankFileDetailPaymentRecord[4] = string.Empty.PadRight(18);

            GenerateMPFDetail(empInfo, bankFileDetail.Amount, bankFileDetailPaymentRecord, contributionList, out FirstContributionString);
            //bankFileDetailPaymentRecord[15] = bankFileDetail.Amount.ToString("0.00").PadLeft(12);
            //bankFileDetailPaymentRecord[16] = bankFileDetail.Amount.ToString("0.00").PadLeft(12);


            string bankFileDetailData = String.Join(FIELD_DELIMITER, bankFileDetailPaymentRecord);
            if (bankFileDetailData.Length != 236)
                throw new Exception("Incorrect Bank File Detail Length:\r\n" + bankFileDetailData);

            return bankFileDetailData;
        }

        protected void GenerateMPFDetail(EEmpPersonalInfo empInfo, double payrollAmount, string[] bankFileDetailPaymentRecord, System.Collections.ArrayList contributionList, out string FirstContributionString)
        {
            DateTime PayPeriodStartDate = m_PayPeriodFr, PayPeriodEndDate = m_PayPeriodTo;
            DateTime MPFCommenceDate = new DateTime();
            DateTime TerminationDate = new DateTime();
            DateTime DateOfBirth = empInfo.EmpDateOfBirth;
            DateTime DateAge18 = empInfo.EmpDateOfBirth.AddYears(18);
            DateTime DateAge65 = empInfo.EmpDateOfBirth.AddYears(65);
            DateTime DateJoinCompany = empInfo.EmpDateOfJoin > empInfo.EmpServiceDate ? empInfo.EmpServiceDate : empInfo.EmpDateOfJoin;
            DateTime MPFContributionStartDate = new DateTime(), MPFContributionEndDate = new DateTime();
            double totalRelevantIncome = 0;
            double totalBasicSalary = 0;
            double totalMCER = 0, totalMCEE = 0, totalVCER = 0, totalVCEE = 0;
            FirstContributionString = string.Empty;

            MPFFile.GenericMPFFileDetail lastmMPFDetail = null;

            string defaultPFundType = string.Empty;
            System.Collections.Generic.List<string[]> firstRemittanceStatementList = new System.Collections.Generic.List<string[]>();
            foreach (MPFFile.GenericMPFFileDetail mpfDetail in contributionList)
            {
                if (mpfDetail.EmpID.Equals(empInfo.EmpID))
                {
                    int newJoinRecordCount = 0;
                    bankFileDetailPaymentRecord[2] = mpfDetail.HKIDPassport.PadRight(15).Substring(0, 15);

                    string pFundType = HROne.CommonLib.Utility.GetXMLElementFromXmlString(mpfDetail.EmpMPFPlanExtendXMLString, "EmpMPFPlanExtendData", EmpMPFPlanSCBPFundTypeNodeName);
                    if (!string.IsNullOrEmpty(pFundType.Trim()))
                        defaultPFundType = pFundType;

                    lastmMPFDetail = mpfDetail;

                    PayPeriodStartDate = PayPeriodFr;
                    PayPeriodEndDate = PayPeriodTo;

                    if (PayPeriodStartDate < DateJoinCompany)
                        PayPeriodStartDate = DateJoinCompany;

                    MPFCommenceDate = mpfDetail.SchemeJoinDate;
                    if (MPFCommenceDate < DateAge18)
                        MPFCommenceDate = DateAge18;

                    if (mpfDetail is MPFFile.GenericExistingEmployeeMPFFileDetail)
                        TerminationDate = ((MPFFile.GenericExistingEmployeeMPFFileDetail)mpfDetail).LastEmploymentDate;
                    else if (mpfDetail is MPFFile.GenericNewJoinEmployeeMPFFileDetail)
                    {
                        NewJoinEECount++;
                        foreach (MPFFile.GenericAdditionalEmployeeMPFFileDetail additionalDetail in additionalEEInformation)
                        {
                            if (additionalDetail.EmpID.Equals(empInfo.EmpID))
                                TerminationDate = additionalDetail.LastEmploymentDate;
                        }
                    }
                    foreach (MPFFile.GenericMPFFileContributionDetail contributionDetail in mpfDetail.MPFContributionDetailList)
                    {
                        double relevantIncome = 0;
                        double mcER = 0;
                        double mcEE = 0;
                        double basicSalary = 0;
                        double vcER = 0;
                        double vcEE = 0;
                        if (MPFContributionStartDate.Ticks.Equals(0) || MPFContributionStartDate > contributionDetail.PeriodFrom)
                            MPFContributionStartDate = contributionDetail.PeriodFrom;

                        if (MPFContributionEndDate.Ticks.Equals(0) || MPFContributionEndDate < contributionDetail.PeriodTo)
                            MPFContributionEndDate = contributionDetail.PeriodTo;


                        if (contributionDetail.MCEE != 0 || contributionDetail.MCER != 0)
                        {
                            totalRelevantIncome += contributionDetail.RelevantIncome;
                            totalMCER += contributionDetail.MCER;
                            totalMCEE += contributionDetail.MCEE;
                            relevantIncome = contributionDetail.RelevantIncome;
                            mcER = contributionDetail.MCER;
                            mcEE = contributionDetail.MCEE;
                        }
                        if (contributionDetail.VCEE != 0 || contributionDetail.VCER != 0)
                        {
                            totalBasicSalary += contributionDetail.VCRelevantIncome;
                            totalVCER += contributionDetail.VCER;
                            totalVCEE += contributionDetail.VCEE;
                            basicSalary = contributionDetail.VCRelevantIncome;
                            vcER = contributionDetail.VCER;
                            vcEE = contributionDetail.VCEE;
                        }

                        if (mpfDetail is MPFFile.GenericNewJoinEmployeeMPFFileDetail)
                        {
                            newJoinRecordCount++;
                            totalFirstContributionAmount += contributionDetail.MCEE
                                + contributionDetail.MCER
                                + contributionDetail.VCEE
                                + contributionDetail.VCER;
                            DateTime activeMPFContributionStartDate = contributionDetail.PeriodFrom;
                            DateTime activeMPFContributionEndDate = contributionDetail.PeriodTo;

                            if (activeMPFContributionStartDate < MPFCommenceDate && MPFCommenceDate <= activeMPFContributionEndDate)
                                activeMPFContributionStartDate = MPFCommenceDate;
                            if (!TerminationDate.Ticks.Equals(0) && activeMPFContributionStartDate <= TerminationDate && TerminationDate < activeMPFContributionEndDate)
                                activeMPFContributionEndDate = TerminationDate;
                            if (activeMPFContributionStartDate < DateAge65 && DateAge65 <= activeMPFContributionEndDate)
                                activeMPFContributionEndDate = DateAge65.AddDays(-1);

                            string[] firstRemittanceStatement = new string[18];
                            firstRemittanceStatement[0] = bankFileDetailPaymentRecord[0];
                            firstRemittanceStatement[1] = NewJoinEECount.ToString("00000");
                            firstRemittanceStatement[2] = newJoinRecordCount.ToString("00");
                            firstRemittanceStatement[3] = bankFileDetailPaymentRecord[1];
                            firstRemittanceStatement[4] = bankFileDetailPaymentRecord[2];
                            firstRemittanceStatement[5] = DateJoinCompany.ToString("yyyyMMdd");
                            firstRemittanceStatement[6] = bankFileDetailPaymentRecord[4];
                            firstRemittanceStatement[7] = activeMPFContributionStartDate.ToString("yyyyMMdd");
                            firstRemittanceStatement[8] = activeMPFContributionEndDate.ToString("yyyyMMdd");
                            firstRemittanceStatement[9] = activeMPFContributionStartDate.ToString("yyyyMMdd");
                            firstRemittanceStatement[10] = activeMPFContributionEndDate.ToString("yyyyMMdd");
                            firstRemittanceStatement[11] = relevantIncome.ToString("000000000.00");
                            firstRemittanceStatement[12] = mcER.ToString("000000000.00");
                            firstRemittanceStatement[13] = mcEE.ToString("000000000.00");
                            firstRemittanceStatement[14] = vcER.ToString("000000000.00");
                            firstRemittanceStatement[15] = vcEE.ToString("000000000.00");
                            firstRemittanceStatement[16] = bankFileDetailPaymentRecord[3];
                            firstRemittanceStatement[17] = basicSalary.ToString("000000000.00");
                            firstRemittanceStatementList.Add(firstRemittanceStatement);
                        }
                    }
                    if (!(mpfDetail is MPFFile.GenericBackPaymentEmployeeMPFFileDetail))
                    {
                        if (MPFContributionStartDate < MPFCommenceDate && MPFCommenceDate <= MPFContributionEndDate)
                            MPFContributionStartDate = MPFCommenceDate;
                        if (!TerminationDate.Ticks.Equals(0) && MPFContributionStartDate <= TerminationDate && TerminationDate < MPFContributionEndDate)
                            MPFContributionEndDate = TerminationDate;
                        if (MPFContributionStartDate < DateAge65 && DateAge65 <= MPFContributionEndDate)
                            MPFContributionEndDate = DateAge65.AddDays(-1);
                    }
                    totalFirstContributionRecordCount += newJoinRecordCount;

                    //  Assume that only 1 mpf detail per employee is included in file
                    break;
                }
            }

            if (lastmMPFDetail != null)
                contributionList.Remove(lastmMPFDetail);

            if (!string.IsNullOrEmpty(defaultPFundType))
                bankFileDetailPaymentRecord[3] = defaultPFundType;
            else if (lastmMPFDetail == null && mpfPlanIDSchemeCodeMapping.Count > 0)
                bankFileDetailPaymentRecord[3] = "8";    //  set PFund Type to "Exempt" if there exists MPF Contribution Records in the batch but did not match with EmpID 

            bankFileDetailPaymentRecord[5] = PayPeriodStartDate.ToString("yyyyMMdd");
            bankFileDetailPaymentRecord[6] = PayPeriodEndDate.ToString("yyyyMMdd");

            if (!MPFCommenceDate.Ticks.Equals(0))
                bankFileDetailPaymentRecord[7] = MPFCommenceDate.ToString("yyyyMMdd");
            else
                bankFileDetailPaymentRecord[7] = string.Empty.PadRight(8);

            if (!TerminationDate.Ticks.Equals(0))
                bankFileDetailPaymentRecord[8] = TerminationDate.ToString("yyyyMMdd");
            else
                bankFileDetailPaymentRecord[8] = string.Empty.PadRight(8);

            if (!DateOfBirth.Ticks.Equals(0))
                bankFileDetailPaymentRecord[9] = DateOfBirth.ToString("yyyyMMdd");
            else
                bankFileDetailPaymentRecord[9] = string.Empty.PadRight(8);

            if (!DateJoinCompany.Ticks.Equals(0))
                bankFileDetailPaymentRecord[10] = DateJoinCompany.ToString("yyyyMMdd");
            else
                bankFileDetailPaymentRecord[10] = string.Empty.PadRight(8);

            if (!MPFContributionStartDate.Ticks.Equals(0))
                bankFileDetailPaymentRecord[11] = MPFContributionStartDate.ToString("yyyyMMdd");
            else
                bankFileDetailPaymentRecord[11] = string.Empty.PadRight(8);

            if (!MPFContributionEndDate.Ticks.Equals(0))
                bankFileDetailPaymentRecord[12] = MPFContributionEndDate.ToString("yyyyMMdd");
            else
                bankFileDetailPaymentRecord[12] = string.Empty.PadRight(8);
            bankFileDetailPaymentRecord[13] = totalRelevantIncome.ToString("000000000.00");
            bankFileDetailPaymentRecord[14] = totalBasicSalary.ToString("000000000.00");
            bankFileDetailPaymentRecord[15] = ((double)(payrollAmount + totalMCEE + totalVCEE)).ToString("000000000.00");
            bankFileDetailPaymentRecord[16] = payrollAmount.ToString("000000000.00");
            bankFileDetailPaymentRecord[17] = ((double)(totalMCEE + totalMCER + totalVCEE + totalVCER)).ToString("000000000.00");
            bankFileDetailPaymentRecord[18] = totalMCER.ToString("000000000.00");
            bankFileDetailPaymentRecord[19] = totalMCEE.ToString("000000000.00");
            bankFileDetailPaymentRecord[20] = totalVCER.ToString("000000000.00");
            bankFileDetailPaymentRecord[21] = totalVCEE.ToString("000000000.00");

            foreach (string[] firstRemittanceStatement in firstRemittanceStatementList)
            {
                firstRemittanceStatement[16] = bankFileDetailPaymentRecord[3];
                string firstRemittanceStatementString = string.Join(FIELD_DELIMITER, firstRemittanceStatement);
                if (firstRemittanceStatementString.Length != 183)
                    throw new Exception("Incorrect Bank File Detail Length:\r\n" + firstRemittanceStatementString);
                FirstContributionString += firstRemittanceStatementString + RECORD_DELIMITER;

            }

        }

        public string ActualBankMPFFileName()
        {
            return "PayrollFile_" + processDateTime.ToString("yyyyMMdd") + ".txt";
        }

        public string ActualMPFTerminationFileName()
        {
            return "FirstContributionRemittance_" + processDateTime.ToString("yyyyMMdd") + ".txt";
        }

        public override string BankFileExtension()
        {
            return ".zip";
        }
    }    
}