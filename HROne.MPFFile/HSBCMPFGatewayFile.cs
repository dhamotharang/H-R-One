using System;
using System.Collections;
using System.Data;
using System.Configuration;
using System.IO;
using HROne.DataAccess;

namespace HROne.MPFFile
{
    /// <summary>
    /// Summary description for HSBCMPFFile
    /// </summary>
    public class HSBCMPFGatewayFile : GenericMPFFile
    {

        private const string FIELD_DELIMITER = "";
        private const string RECORD_DELIMITER = "";


        private int HashCount = 0;
        private int NewJoinEntitiesCount = 0;
        private int ExistingEntitiesCount = 0;
        private string FormNo;

        protected string m_EmployerID;
        public string EmployerID
        {
            get { return m_EmployerID; }
            set { m_EmployerID = value; }
        }
        public string RemoteProfileID;
        protected internal DateTime TransactionTime;

        private string m_DefaultClassName;
        public string DefaultClassName
        {
            get { return m_DefaultClassName; }
        }

        private string m_PayCenterCode;
        public string PayCenterCode
        {
            get { return m_PayCenterCode; }
            set { m_PayCenterCode = value; }
        }
        //protected string m_EmployerName;
        //public string EmployerName
        //{
        //    get { return m_EmployerName; }
        //}
        //protected string m_EmployerAddress;
        //public string EmployerAddress
        //{
        //    get { return m_EmployerAddress; }
        //}

        //protected string m_ContactPerson;
        //public string ContactPerson
        //{
        //    get { return m_ContactPerson; }
        //}
        //protected string m_ContactPersonPhoneNumber;
        //public string ContactPersonPhoneNumber
        //{
        //    get { return m_ContactPersonPhoneNumber; }
        //}

        public string TransactionReference = string.Empty;

        public HSBCMPFGatewayFile(DatabaseConnection dbConn, string RemoteProfileID)
            : base(dbConn)
        {
            this.RemoteProfileID = RemoteProfileID;
            TransactionTime = AppUtils.ServerDateTime();
        }
        //public void LoadBankFileDetail(string MPFFileFullPath)
        //{
        //    FileInfo fileInfo = new FileInfo(MPFFileFullPath);
        //    if (fileInfo.Length % 80 != 0)
        //        throw new Exception("Incorrect file size");

        //    StreamReader mpfFileStream = fileInfo.OpenText();
        //    char[] charRead = new char[80];

        //    int lineNo = 0;

        //    try
        //    {
        //        while (mpfFileStream.Read(charRead, 0, 80) > 0)
        //        {
        //            string line = new string(charRead);
        //            lineNo++;

        //            if (lineNo == 1)
        //            {
        //                string[] mpfFileHeader1 = new string[7];
        //                mpfFileHeader1[0] = line.Substring(0, 1);
        //                mpfFileHeader1[1] = line.Substring(1, 18);
        //                mpfFileHeader1[2] = line.Substring(19, 28);
        //                mpfFileHeader1[3] = line.Substring(47, 16);
        //                mpfFileHeader1[4] = line.Substring(63, 3);
        //                mpfFileHeader1[5] = line.Substring(66, 8);
        //                mpfFileHeader1[6] = line.Substring(74, 6);

        //                if (!mpfFileHeader1[0].Equals("S"))
        //                    throw new Exception("Invalid file header on line 1");

        //                RemoteProfileID = mpfFileHeader1[1].Trim();
        //                TransactionReference = mpfFileHeader1[3].Trim();
        //                if (!DateTime.TryParseExact(mpfFileHeader1[5] + mpfFileHeader1[6], "yyyyMMddHHmmss", null, System.Globalization.DateTimeStyles.None, out TransactionTime))
        //                    TransactionTime = TransactionTime = AppUtils.ServerDateTime();

        //            }
        //            else
        //            {
        //                string[] bankFileDetailRecord = new string[10];

        //                bankFileDetailRecord[0] = line.Substring(0, 1);
        //                bankFileDetailRecord[1] = line.Substring(1, 12);
        //                bankFileDetailRecord[2] = line.Substring(13, 20);
        //                bankFileDetailRecord[3] = line.Substring(33, 3);
        //                bankFileDetailRecord[4] = line.Substring(36, 3);
        //                bankFileDetailRecord[5] = line.Substring(39, 9);
        //                bankFileDetailRecord[6] = line.Substring(48, 10);
        //                bankFileDetailRecord[7] = line.Substring(58, 4);
        //                bankFileDetailRecord[8] = line.Substring(62, 6);
        //                bankFileDetailRecord[9] = line.Substring(68, 12);

        //                GenericBankFileDetail bankFileDetail = new GenericBankFileDetail();
        //                if (!bankFileDetailRecord[0].Equals(" "))
        //                    throw new Exception("Invalid record header on record detail" + " (line " + lineNo + ")");

        //                bankFileDetail.EmpNo = bankFileDetailRecord[1] + bankFileDetailRecord[8];
        //                bankFileDetail.EmpNo = bankFileDetail.EmpNo.Trim();
        //                bankFileDetail.EmpBankAccountHolderName = bankFileDetailRecord[2].Trim();
        //                bankFileDetail.BankCode = bankFileDetailRecord[3].Trim();
        //                bankFileDetail.BranchCode = bankFileDetailRecord[4].Trim();
        //                bankFileDetail.AccountNo = bankFileDetailRecord[5].Trim();
        //                double tmpRecordAmount = 0;
        //                if (!double.TryParse(bankFileDetailRecord[6].Insert(8, "."), out tmpRecordAmount))
        //                    throw new Exception("Invalid amount on record detail" + " (line " + lineNo + ")");
        //                bankFileDetail.Amount = tmpRecordAmount;

        //                int valueDateNumeric = 0;
        //                if (m_PlanCode == "F" || m_PlanCode == "G")
        //                {
        //                    if (!bankFileDetailRecord[7].Trim().Equals(string.Empty))
        //                        throw new Exception("Invalid value date on record detail" + " (line " + lineNo + ")");
        //                }
        //                else
        //                {
        //                    if (!int.TryParse(bankFileDetailRecord[7], out valueDateNumeric))
        //                        throw new Exception("Invalid value date on record detail" + " (line " + lineNo + ")");
        //                    int days = valueDateNumeric / 100;
        //                    int month = valueDateNumeric % 100;
        //                    bankFileDetail.ValueDate = new DateTime(currentDateTime.Year, month, days);
        //                    //  find a value date which is nearest to the current date
        //                    if (bankFileDetail.ValueDate < currentDateTime.AddDays(-180))
        //                        //  to prevent 29 Feb issue, use "new DateTime" to re-assign the value
        //                        bankFileDetail.ValueDate = new DateTime(currentDateTime.Year + 1, month, days);
        //                    if (bankFileDetail.ValueDate.Day != days && bankFileDetail.ValueDate.Month != month)
        //                        throw new Exception("Invalid value date on record detail" + " (line " + lineNo + ")");

        //                }
        //                m_TotalAmount += bankFileDetail.Amount;
        //                BankFileDetails.Add(bankFileDetail);
        //            }
        //        }

        //        if (m_TotalAmount - tmpHeaderTotalAmount >= 0.01 || RecordCount != tmpHeaderRecordCount)
        //            throw new Exception("Total Amount / Record Count does not match with File Header");
        //    }
        //    finally
        //    {
        //        mpfFileStream.Close();
        //    }
        //}
        public override FileInfo GenerateMPFFile()
        {
            string mpfFileData = string.Empty;

            // get Form No
            if (MPFSchemeCode.Equals("MT00261", StringComparison.CurrentCultureIgnoreCase))
                FormNo = "INB1";
            else if (MPFSchemeCode.Equals("MT00245", StringComparison.CurrentCultureIgnoreCase))
                FormNo = "INB2";
            else if (MPFSchemeCode.Equals("MT00512", StringComparison.CurrentCultureIgnoreCase))
                FormNo = "INB7";
            else if (MPFSchemeCode.Equals("MT00555", StringComparison.CurrentCultureIgnoreCase))
                FormNo = "INBA";
            else if (MPFSchemeCode.Equals("MT0027A", StringComparison.CurrentCultureIgnoreCase))
                FormNo = "HAB1";
            else if (MPFSchemeCode.Equals("MT00253", StringComparison.CurrentCultureIgnoreCase))
                FormNo = "HAB2";
            else if (MPFSchemeCode.Equals("MT00520", StringComparison.CurrentCultureIgnoreCase))
                FormNo = "HAB7";
            else if (MPFSchemeCode.Equals("MT00563", StringComparison.CurrentCultureIgnoreCase))
                FormNo = "HABA";
            else
                FormNo = "    ";
            // get Employer ID
            System.Xml.XmlNodeList mpfEmployerIDNode = HROne.CommonLib.Utility.GetXmlDocumentByDataString(MPFPlanExtendXMLString).GetElementsByTagName("MPFPlanEmployerID");
            if (mpfEmployerIDNode.Count > 0)
            {
                m_EmployerID = mpfEmployerIDNode[0].InnerText.PadRight(8).Substring(0, 8);
            }
            else
                m_EmployerID = string.Empty.PadRight(8);


            System.Xml.XmlNodeList defaultClassNameNode = HROne.CommonLib.Utility.GetXmlDocumentByDataString(MPFPlanExtendXMLString).GetElementsByTagName("MPFPlanDefaultClassName");
            if (defaultClassNameNode.Count > 0)
            {
                m_DefaultClassName = defaultClassNameNode[0].InnerText.Trim().ToUpper();
            }
            else
                m_DefaultClassName = string.Empty;

            System.Xml.XmlNodeList payCenterNode = HROne.CommonLib.Utility.GetXmlDocumentByDataString(MPFPlanExtendXMLString).GetElementsByTagName("MPFPlanPayCenter");
            if (payCenterNode.Count > 0)
            {
                m_PayCenterCode = payCenterNode[0].InnerText.PadRight(4).Substring(0, 4).ToUpper();
            }
            else
                m_PayCenterCode = string.Empty.PadRight(4);

            {
                HashCount = 1;
                string[] mpfFileHeader3 = new string[8];
                mpfFileHeader3[0] = " ";
                mpfFileHeader3[1] = "0";
                if (FormNo.Substring(2).Equals("B1"))
                    mpfFileHeader3[2] = "B5";
                else if (FormNo.Substring(2).Equals("B2"))
                    mpfFileHeader3[2] = "B6";
                else if (FormNo.Substring(2).Equals("B7"))
                    mpfFileHeader3[2] = "BM";
                else if (FormNo.Substring(2).Equals("BA"))
                    mpfFileHeader3[2] = "BX";
                mpfFileHeader3[3] = PayPeriodFr.ToString("yyyyMMdd");
                mpfFileHeader3[4] = PayPeriodTo.ToString("yyyyMMdd");


                mpfFileHeader3[5] = m_PayCenterCode.PadRight(4).Substring(0, 4).ToUpper();


                mpfFileHeader3[6] = "D";
                mpfFileHeader3[7] = string.Empty.PadLeft(55);


                string mpfFileHeaderString3 = string.Join(FIELD_DELIMITER, mpfFileHeader3);
                if (mpfFileHeaderString3.Length != 80)
                    throw new Exception("Invalid Length on header line 3");
                mpfFileData = mpfFileHeaderString3;
            }
            foreach (GenericExistingEmployeeMPFFileDetail bankFileDetail in ExistingEmployeeMPFFileDetails)
            {
                mpfFileData += GenerateExistingMPFFileDetail(bankFileDetail) + RECORD_DELIMITER;
            }
            foreach (GenericNewJoinEmployeeMPFFileDetail mpfFileDetail in NewJoinEmployeeMPFFileDetails)
            {
                mpfFileData += GenerateNewJoinMPFFileDetail(mpfFileDetail) + RECORD_DELIMITER;
            }
            //foreach (GenericAdditionalEmployeeMPFFileDetail bankFileDetail in AdditionalEmployeeMPFFileDetails)
            //{
            //    mpfFileData += GenerateAdditionalMPFFileDetail(bankFileDetail) + RECORD_DELIMITER;
            //}
            foreach (GenericBackPaymentEmployeeMPFFileDetail bankFileDetail in BackPaymentEmployeeMPFFileDetails)
            {
                mpfFileData += GenerateBackPaymentMPFFileDetail(bankFileDetail) + RECORD_DELIMITER;
            }
            {
                HashCount++;

                string[] mpfFileFooter = new string[9];
                mpfFileFooter[0] = " ";
                mpfFileFooter[1] = "9";
                mpfFileFooter[2] = ((double)(this.TotalAdditionalEmployeeMC + this.TotalAdditionalEmployeeVC + this.TotalBackPaymentEmployeeMC + this.TotalBackPaymentEmployeeVC + this.TotalExistingEmployeeMC + this.TotalExistingEmployeeVC + this.TotalNewJoinEmployeeMC + this.TotalNewJoinEmployeeVC)).ToString("0000000000000000.00").Replace(".", "");
                mpfFileFooter[3] = "D";
                mpfFileFooter[4] = "      ";
                mpfFileFooter[5] = "        ";
                mpfFileFooter[6] = ExistingEntitiesCount.ToString("0000000");
                mpfFileFooter[7] = NewJoinEntitiesCount.ToString("0000000");
                mpfFileFooter[8] = string.Empty.PadRight(31);

                string mpfFileFooterString = string.Join(FIELD_DELIMITER, mpfFileFooter);
                if (mpfFileFooterString.Length != 80)
                    throw new Exception("Invalid Length on header line 3");

                mpfFileData += mpfFileFooterString;
            }

            {
                string[] mpfFileHeader2 = new string[8];
                mpfFileHeader2[0] = "M";
                mpfFileHeader2[1] = EmployerID.PadRight(8).Substring(0, 8);
                mpfFileHeader2[2] = "0000";
                mpfFileHeader2[3] = FormNo.Substring(0, 2);
                mpfFileHeader2[4] = HashCount.ToString("0000000");
                mpfFileHeader2[5] = TransactionTime.ToString("yyyyMMdd");
                mpfFileHeader2[6] = TransactionTime.ToString("HHmmss");
                mpfFileHeader2[7] = string.Empty.PadRight(44);
                string mpfFileHeaderString2 = string.Join(FIELD_DELIMITER, mpfFileHeader2);
                if (mpfFileHeaderString2.Length != 80)
                    throw new Exception("Invalid Length on header line 3");
                mpfFileData = mpfFileHeaderString2 + RECORD_DELIMITER
                + mpfFileData;

            }

            //{
            //    string[] mpfFileHeader1 = new string[7];
            //    mpfFileHeader1[0] = "S";
            //    mpfFileHeader1[1] = RemoteProfileID.PadRight(18).Substring(0, 18);
            //    mpfFileHeader1[2] = string.Empty.PadRight(28);
            //    mpfFileHeader1[3] = TransactionReference.PadRight(16).Substring(0, 16);
            //    mpfFileHeader1[4] = string.Empty.PadRight(3);
            //    mpfFileHeader1[5] = TransactionTime.ToString("yyyyMMdd");
            //    mpfFileHeader1[6] = TransactionTime.ToString("HHmmss");

            //    string mpfFileHeaderString1 = string.Join(FIELD_DELIMITER, mpfFileHeader1);
            //    if (mpfFileHeaderString1.Length != 80)
            //        throw new Exception("Invalid Length on header line 3");
            //    mpfFileData = mpfFileHeaderString1 + RECORD_DELIMITER
            //        + mpfFileData;
            //}



            FileInfo result = GenerateTempFileName();
            StreamWriter writer = new StreamWriter(result.OpenWrite());
            writer.Write(mpfFileData);
            writer.Close();
            return result;

        }

        protected override string GenerateExistingMPFFileDetail(GenericExistingEmployeeMPFFileDetail mpfDataDetail)
        {
            string result = string.Empty;
            foreach (GenericMPFFileContributionDetail mpfContributionDetail in mpfDataDetail.MPFContributionDetailList)
            {
                HashCount++;
                ExistingEntitiesCount++;
                {
                    string[] mpfFileDetail1 = new string[12];
                    mpfFileDetail1[0] = " ";
                    mpfFileDetail1[1] = "1";
                    mpfFileDetail1[2] = mpfDataDetail.HKIDPassport.PadRight(12).Substring(0, 12);
                    mpfFileDetail1[3] = mpfDataDetail.SchemeJoinDate.ToString("yyyyMMdd");
                    mpfFileDetail1[4] = mpfContributionDetail.PeriodFrom.ToString("yyyyMMdd");
                    mpfFileDetail1[5] = mpfContributionDetail.PeriodTo.ToString("yyyyMMdd");
                    mpfFileDetail1[6] = mpfContributionDetail.RelevantIncome.ToString("000000000.00").Replace(".", "");
                    mpfFileDetail1[7] = mpfContributionDetail.MCER.ToString("00000.00").Replace(".", "");
                    mpfFileDetail1[8] = mpfContributionDetail.MCEE.ToString("00000.00").Replace(".", "");
                    mpfFileDetail1[9] = mpfContributionDetail.VCER.ToString("000000.00").Replace(".", "");
                    mpfFileDetail1[10] = mpfContributionDetail.VCEE.ToString("000000.00").Replace(".", "");
                    if (!mpfDataDetail.LastEmploymentDate.Ticks.Equals(0))
                        mpfFileDetail1[11] = "C";
                    else
                        mpfFileDetail1[11] = " ";

                    string mpfFileDetailString1 = string.Join(FIELD_DELIMITER, mpfFileDetail1);
                    if (mpfFileDetailString1.Length != 80)
                        throw new Exception("Invalid Length on detail");
                    if (string.IsNullOrEmpty(result))
                        result = mpfFileDetailString1;
                    else
                        result += mpfFileDetailString1;
                }
                if (!mpfDataDetail.LastEmploymentDate.Ticks.Equals(0))
                {

                    {
                        HashCount++;
                        string[] mpfFileDetail2 = new string[8];
                        mpfFileDetail2[0] = " ";
                        mpfFileDetail2[1] = "1";
                        mpfFileDetail2[2] = mpfDataDetail.HKIDPassport.PadRight(12).Substring(0, 12);
                        if (mpfDataDetail.LspSpFlag == LSPSP_FLAG_ENUM.LSP)
                            mpfFileDetail2[3] = "L";
                        else if (mpfDataDetail.LspSpFlag == LSPSP_FLAG_ENUM.SP)
                            mpfFileDetail2[3] = "S";
                        else
                            mpfFileDetail2[3] = " ";
                        mpfFileDetail2[4] = mpfDataDetail.LSPSPAmount.ToString("0000000000000.00").Replace(".", "");
                        mpfFileDetail2[5] = mpfDataDetail.LSPSPAmountPaidByER.ToString("0000000000000.00").Replace(".", "");
                        mpfFileDetail2[6] = string.Empty.PadRight(34);
                        mpfFileDetail2[7] = "X";

                        string mpfFileDetailString2 = string.Join(FIELD_DELIMITER, mpfFileDetail2);
                        if (mpfFileDetailString2.Length != 80)
                            throw new Exception("Invalid Length on detail");
                        result += RECORD_DELIMITER + mpfFileDetailString2;

                    }
                    HashCount++;
                    {
                        string[] mpfFileDetail3 = new string[7];

                        mpfFileDetail3[0] = " ";
                        mpfFileDetail3[1] = "3";
                        mpfFileDetail3[2] = mpfDataDetail.HKIDPassport.PadRight(12).Substring(0, 12);
                        mpfFileDetail3[3] = mpfDataDetail.LastEmploymentDate.ToString("yyyyMMdd");
                        mpfFileDetail3[4] = mpfDataDetail.TermCode.PadRight(2).Substring(0, 2);
                        mpfFileDetail3[5] = mpfDataDetail.LastPaymentDate.ToString("yyyyMMdd");
                        mpfFileDetail3[6] = string.Empty.PadRight(48);

                        string mpfFileDetailString3 = string.Join(FIELD_DELIMITER, mpfFileDetail3);
                        if (mpfFileDetailString3.Length != 80)
                            throw new Exception("Invalid Length on detail");
                        result += RECORD_DELIMITER + mpfFileDetailString3;
                    }
                }

            }
            return result;
        }
 
        protected override string GenerateNewJoinMPFFileDetail(GenericNewJoinEmployeeMPFFileDetail mpfDataDetail)
        {
            string result = string.Empty;

            HashCount++;
            NewJoinEntitiesCount++;

            {
                string[] mpfFileDetail = new string[12];
                mpfFileDetail[0] = " ";
                mpfFileDetail[1] = "2";
                mpfFileDetail[2] = "E";
                if (mpfDataDetail.HKIDType == IDENTITY_TYPE_ENUM.HKID)
                    mpfFileDetail[3] = "H";
                else if (mpfDataDetail.HKIDType == IDENTITY_TYPE_ENUM.PASSPORT)
                    mpfFileDetail[3] = "P";
                else
                    mpfFileDetail[3] = " ";
                mpfFileDetail[4] = mpfDataDetail.HKIDPassport.PadRight(12).Substring(0, 12);
                mpfFileDetail[5] = mpfDataDetail.SchemeJoinDate.ToString("yyyyMMdd");
                if (mpfDataDetail.MemberType == MEMBER_TYPE_ENUM.NORMAL)
                    mpfFileDetail[6] = "1";
                else if (mpfDataDetail.MemberType == MEMBER_TYPE_ENUM.CASUAL)
                    mpfFileDetail[6] = "1";
                else if (mpfDataDetail.MemberType == MEMBER_TYPE_ENUM.EXEMPT)
                    mpfFileDetail[6] = "2";
                else
                    mpfFileDetail[6] = " ";

                mpfFileDetail[7] = mpfDataDetail.DateJoined.ToString("yyyyMMdd");
                System.Xml.XmlNodeList classNameNode = HROne.CommonLib.Utility.GetXmlDocumentByDataString(mpfDataDetail.EmpMPFPlanExtendXMLString).GetElementsByTagName("EmpMPFPlanClassName");
                if (classNameNode.Count > 0)
                {
                    mpfFileDetail[8] = classNameNode[0].InnerText.PadRight(8).Substring(0, 8).ToUpper();
                    if (string.IsNullOrEmpty(mpfFileDetail[8].Trim()))
                        mpfFileDetail[8] = DefaultClassName.PadRight(8).Substring(0, 8).ToUpper();
                }
                else
                    mpfFileDetail[8] = DefaultClassName.PadRight(8).Substring(0, 8).ToUpper();
                mpfFileDetail[9] = "      ";   // saluation, not required
                mpfFileDetail[10] = mpfDataDetail.IsTransfer ? "1" : " ";
                mpfFileDetail[11] = string.Empty.PadRight(32);

                string mpfFileDetailString1 = string.Join(FIELD_DELIMITER, mpfFileDetail);
                if (mpfFileDetailString1.Length != 80)
                    throw new Exception("Invalid Length on detail");
                result = mpfFileDetailString1;
            }

            HashCount++;
            {
                string[] mpfFileDetail2 = new string[12];
                mpfFileDetail2[0] = " ";
                mpfFileDetail2[1] = "2";
                mpfFileDetail2[2] = "X";
                if (mpfDataDetail.HKIDType == IDENTITY_TYPE_ENUM.HKID)
                    mpfFileDetail2[3] = "H";
                else if (mpfDataDetail.HKIDType == IDENTITY_TYPE_ENUM.PASSPORT)
                    mpfFileDetail2[3] = "P";
                else
                    mpfFileDetail2[3] = " ";
                mpfFileDetail2[4] = mpfDataDetail.HKIDPassport.PadRight(12).Substring(0, 12);

                mpfFileDetail2[5] = mpfDataDetail.EmpSurname.Replace(",", " ").Replace("  ", " ").PadRight(30).Substring(0, 30).ToUpper();
                mpfFileDetail2[6] = mpfDataDetail.EmpOtherName.Replace(",", " ").Replace("  ", " ").PadRight(20).Substring(0, 20).ToUpper();
                mpfFileDetail2[7] = "   ";  // Member Initial, N/A
                mpfFileDetail2[8] = mpfDataDetail.DateOfBirth.ToString("yyyyMMdd");
                mpfFileDetail2[9] = mpfDataDetail.Sex.PadRight(1).Substring(0, 1).ToUpper();
                mpfFileDetail2[10] = "  ";

                string mpfFileDetailString2 = string.Join(FIELD_DELIMITER, mpfFileDetail2);
                if (mpfFileDetailString2.Length != 80)
                    throw new Exception("Invalid Length on detail");
                result += RECORD_DELIMITER + mpfFileDetailString2;
            }
            foreach (GenericMPFFileContributionDetail mpfContributionDetail in mpfDataDetail.MPFContributionDetailList)
            {
                HashCount++;
                string[] mpfFileDetail3 = new string[13];
                mpfFileDetail3[0] = " ";
                mpfFileDetail3[1] = "2";
                mpfFileDetail3[2] = "C";
                if (mpfDataDetail.HKIDType == IDENTITY_TYPE_ENUM.HKID)
                    mpfFileDetail3[3] = "H";
                else if (mpfDataDetail.HKIDType == IDENTITY_TYPE_ENUM.PASSPORT)
                    mpfFileDetail3[3] = "P";
                else
                    mpfFileDetail3[3] = " ";
                mpfFileDetail3[4] = mpfDataDetail.HKIDPassport.PadRight(12).Substring(0, 12);
                mpfFileDetail3[5] = mpfContributionDetail.PeriodFrom.ToString("yyyyMMdd");
                mpfFileDetail3[6] = mpfContributionDetail.PeriodTo.ToString("yyyyMMdd");
                mpfFileDetail3[7] = mpfContributionDetail.RelevantIncome.ToString("000000000.00").Replace(".", "");
                mpfFileDetail3[8] = mpfContributionDetail.MCER.ToString("00000.00").Replace(".", "");
                mpfFileDetail3[9] = mpfContributionDetail.MCEE.ToString("00000.00").Replace(".", "");
                mpfFileDetail3[10] = mpfContributionDetail.VCER.ToString("000000.00").Replace(".", "");
                mpfFileDetail3[11] = mpfContributionDetail.VCEE.ToString("000000.00").Replace(".", "");
                mpfFileDetail3[12] = string.Empty.PadRight(7);

                string mpfFileDetailString3 = string.Join(FIELD_DELIMITER, mpfFileDetail3);
                if (mpfFileDetailString3.Length != 80)
                    throw new Exception("Invalid Length on detail");
                result += RECORD_DELIMITER + mpfFileDetailString3;

            }

            foreach (GenericAdditionalEmployeeMPFFileDetail additionDetail in AdditionalEmployeeMPFFileDetails)
            {
                if (additionDetail.EmpID == mpfDataDetail.EmpID)
                {
                    if (!additionDetail.LastEmploymentDate.Ticks.Equals(0))
                    {
                        if (additionDetail.LspSpFlag == LSPSP_FLAG_ENUM.LSP || additionDetail.LspSpFlag == LSPSP_FLAG_ENUM.SP)
                        {
                            HashCount++;
                            string[] mpfFileDetail2 = new string[8];
                            mpfFileDetail2[0] = " ";
                            mpfFileDetail2[1] = "1";
                            mpfFileDetail2[2] = additionDetail.HKIDPassport.PadRight(12).Substring(0, 12);
                            if (additionDetail.LspSpFlag == LSPSP_FLAG_ENUM.LSP)
                                mpfFileDetail2[3] = "L";
                            else if (additionDetail.LspSpFlag == LSPSP_FLAG_ENUM.SP)
                                mpfFileDetail2[3] = "S";
                            mpfFileDetail2[4] = additionDetail.LSPSPAmount.ToString("0000000000000.00").Replace(".", "");
                            mpfFileDetail2[5] = additionDetail.LSPSPAmountPaidByER.ToString("0000000000000.00").Replace(".", "");
                            mpfFileDetail2[6] = string.Empty.PadRight(34);
                            mpfFileDetail2[7] = "X";

                            string mpfFileDetailString2 = string.Join(FIELD_DELIMITER, mpfFileDetail2);
                            if (mpfFileDetailString2.Length != 80)
                                throw new Exception("Invalid Length on detail");
                            result += RECORD_DELIMITER + mpfFileDetailString2;

                        }
                        HashCount++;
                        {
                            string[] mpfFileDetail3 = new string[7];

                            mpfFileDetail3[0] = " ";
                            mpfFileDetail3[1] = "3";
                            mpfFileDetail3[2] = additionDetail.HKIDPassport.PadRight(12).Substring(0, 12);
                            mpfFileDetail3[3] = additionDetail.LastEmploymentDate.ToString("yyyyMMdd");
                            mpfFileDetail3[4] = additionDetail.TermCode.PadRight(2).Substring(0, 2);
                            mpfFileDetail3[5] = additionDetail.LastPaymentDate.ToString("yyyyMMdd");
                            mpfFileDetail3[6] = string.Empty.PadRight(48);

                            string mpfFileDetailString3 = string.Join(FIELD_DELIMITER, mpfFileDetail3);
                            if (mpfFileDetailString3.Length != 80)
                                throw new Exception("Invalid Length on detail");
                            result += RECORD_DELIMITER + mpfFileDetailString3;
                        }
                    }
                }
            }

            return result;
        }


        protected override string GenerateBackPaymentMPFFileDetail(GenericBackPaymentEmployeeMPFFileDetail mpfDataDetail)
        {
            string result = string.Empty;
            foreach (GenericMPFFileContributionDetail mpfContributionDetail in mpfDataDetail.MPFContributionDetailList)
            {
                HashCount++;
                ExistingEntitiesCount++;
                {
                    string[] mpfFileDetail1 = new string[12];
                    mpfFileDetail1[0] = " ";
                    mpfFileDetail1[1] = "4";
                    mpfFileDetail1[2] = mpfDataDetail.HKIDPassport.PadRight(12).Substring(0, 12);
                    mpfFileDetail1[3] = mpfDataDetail.SchemeJoinDate.ToString("yyyyMMdd");
                    mpfFileDetail1[4] = mpfContributionDetail.PeriodFrom.ToString("yyyyMMdd");
                    mpfFileDetail1[5] = mpfContributionDetail.PeriodTo.ToString("yyyyMMdd");
                    mpfFileDetail1[6] = mpfContributionDetail.RelevantIncome.ToString("000000000.00").Replace(".", "");
                    mpfFileDetail1[7] = mpfContributionDetail.MCER.ToString("00000.00").Replace(".", "");
                    mpfFileDetail1[8] = mpfContributionDetail.MCEE.ToString("00000.00").Replace(".", "");
                    mpfFileDetail1[9] = mpfContributionDetail.VCER.ToString("000000.00").Replace(".", "");
                    mpfFileDetail1[10] = mpfContributionDetail.VCEE.ToString("000000.00").Replace(".", "");
                    if (!mpfDataDetail.LastEmploymentDate.Ticks.Equals(0))
                        mpfFileDetail1[11] = "C";
                    else
                        mpfFileDetail1[11] = " ";

                    string mpfFileDetailString1 = string.Join(FIELD_DELIMITER, mpfFileDetail1);
                    if (mpfFileDetailString1.Length != 80)
                        throw new Exception("Invalid Length on detail");
                    if (string.IsNullOrEmpty(result))
                        result = mpfFileDetailString1;
                    else
                        result += mpfFileDetailString1;
                }
                if (!mpfDataDetail.LastEmploymentDate.Ticks.Equals(0))
                {
                    if (mpfDataDetail.LspSpFlag == LSPSP_FLAG_ENUM.LSP || mpfDataDetail.LspSpFlag == LSPSP_FLAG_ENUM.SP)
                    {
                        HashCount++;
                        string[] mpfFileDetail2 = new string[8];
                        mpfFileDetail2[0] = " ";
                        mpfFileDetail2[1] = "1";
                        mpfFileDetail2[2] = mpfDataDetail.HKIDPassport.PadRight(12).Substring(0, 12);
                        if (mpfDataDetail.LspSpFlag == LSPSP_FLAG_ENUM.LSP)
                            mpfFileDetail2[3] = "L";
                        else if (mpfDataDetail.LspSpFlag == LSPSP_FLAG_ENUM.SP)
                            mpfFileDetail2[3] = "S";
                        mpfFileDetail2[4] = mpfDataDetail.LSPSPAmount.ToString("0000000000000.00").Replace(".", "");
                        mpfFileDetail2[5] = mpfDataDetail.LSPSPAmountPaidByER.ToString("0000000000000.00").Replace(".", "");
                        mpfFileDetail2[6] = string.Empty.PadRight(34);
                        mpfFileDetail2[7] = "X";

                        string mpfFileDetailString2 = string.Join(FIELD_DELIMITER, mpfFileDetail2);
                        if (mpfFileDetailString2.Length != 80)
                            throw new Exception("Invalid Length on detail");
                        result += RECORD_DELIMITER + mpfFileDetailString2;

                    }
                    HashCount++;
                    {
                        string[] mpfFileDetail3 = new string[7];

                        mpfFileDetail3[0] = " ";
                        mpfFileDetail3[1] = "3";
                        mpfFileDetail3[2] = mpfDataDetail.HKIDPassport.PadRight(12).Substring(0, 12);
                        mpfFileDetail3[3] = mpfDataDetail.LastEmploymentDate.ToString("yyyyMMdd");
                        mpfFileDetail3[4] = mpfDataDetail.TermCode.PadRight(2).Substring(0, 2);
                        mpfFileDetail3[5] = mpfDataDetail.LastPaymentDate.ToString("yyyyMMdd");
                        mpfFileDetail3[6] = string.Empty.PadRight(48);

                        string mpfFileDetailString3 = string.Join(FIELD_DELIMITER, mpfFileDetail3);
                        if (mpfFileDetailString3.Length != 80)
                            throw new Exception("Invalid Length on detail");
                        result += RECORD_DELIMITER + mpfFileDetailString3;
                    }
                }

            }
            return result;
        }

        public bool IsValidFormat(string path, out string TrusteeCode)
        {
            FileInfo fileInfo = new FileInfo(path);
            StreamReader bankFileStream = fileInfo.OpenText();
            try
            {
                char[] charRead = new char[30];
                TrusteeCode = string.Empty;

                if (bankFileStream.Read(charRead, 0, charRead.Length) > 0)
                {
                    string mpfFileHeaderString2 = new string(charRead);
                    string[] mpfFileHeader2 = new string[8];
                    mpfFileHeader2[0] = mpfFileHeaderString2.Substring(0, 1);
                    mpfFileHeader2[1] = mpfFileHeaderString2.Substring(1, 8).Trim();
                    mpfFileHeader2[2] = mpfFileHeaderString2.Substring(9, 4).Trim();
                    mpfFileHeader2[3] = mpfFileHeaderString2.Substring(13, 2).Trim();
                    mpfFileHeader2[4] = mpfFileHeaderString2.Substring(15, 7).Trim();
                    mpfFileHeader2[5] = mpfFileHeaderString2.Substring(22, 8).Trim();


                    string header = mpfFileHeader2[0];
                    if (header.Equals("M"))
                    {
                        string FormNo = mpfFileHeader2[3];
                        if (FormNo.StartsWith("IN"))
                            TrusteeCode = "HSBC";
                        else if (FormNo.StartsWith("HA"))
                            TrusteeCode = "HangSeng";
                        if (!string.IsNullOrEmpty(TrusteeCode))
                            return true;
                    }
                }
                return false;
            }
            finally
            {
                bankFileStream.Close();
            }
        }

        public override string ActualMPFFileName()
        {
            return EmployerID + "_" + AppUtils.ServerDateTime().ToString("yyyyMMdd") + MPFFileExtension();
        }

        public override string MPFFileExtension()
        {
            return "." + PayCenterCode;
        }
    }
}