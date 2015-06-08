using System;
using System.Collections;
using System.Data;
using System.Configuration;
using System.IO;
using HROne.Lib.Entities;
using HROne.DataAccess;

namespace HROne.MPFFile
{
    /// <summary>
    /// Summary description for HSBCOISMPFFile
    /// </summary>
    public class HSBCOISMPFFile : GenericMPFFile
    {
        private const string MPF_PLAN_XML_SEQUENCE_NODE_NAME = "MPFPlanHSBCOISSequenceNo";
        private const string MPF_PLAN_XML_SCHEMENO_NODE_NAME = "MPFPlanSchemeNo";

        public enum PaymentMethodEnum
        {
            AUTOPAY = 1,
            CHEQUE,
            CASH
        }

        private const string FIELD_DELIMITER = "";
        private const string RECORD_DELIMITER = "\r\n";

        public PaymentMethodEnum PaymentMethod;

        //private int HashCount = 0;
        //private int NewJoinEntitiesCount = 0;
        //private int ExistingEntitiesCount = 0;
        private string MPFPlanSchemeNo;
        private string MPFPlanPlanNo = "M";
        private DateTime TransactionTime;

        private bool m_FileIsEncrypted = false;
        public bool FileIsEncrypted
        {
            get { return m_FileIsEncrypted; }
        }

        private int m_SequenceNo;
        //public int SequenceNo
        //{
        //    get { return m_SequenceNo; }
        //    set { m_SequenceNo = value; }
        //}


        private string m_ChequeNum;
        public string ChequeNum
        {
            get { return m_ChequeNum; }
            set { m_ChequeNum = value; }
        }

        protected class MPFPeriod
        {
            public DateTime PeriodFrom;
            public DateTime PeriodTo;
            public ArrayList DetailStringList = null;
        }

        public HSBCOISMPFFile(DatabaseConnection dbConn)
            : base(dbConn)
        {
        }

        public override FileInfo GenerateMPFFile()
        {
            EMPFPlan m_mpfPlanObj = new EMPFPlan();
            m_mpfPlanObj.MPFPlanID = MPFPlanID;
            EMPFPlan.db.select(dbConn, m_mpfPlanObj);

            TransactionTime = AppUtils.ServerDateTime();

            string mpfFileData = string.Empty;
            // get Scheme No
            System.Xml.XmlNodeList mpfPlanSchemeNoNode = HROne.CommonLib.Utility.GetXmlDocumentByDataString(MPFPlanExtendXMLString).GetElementsByTagName(MPF_PLAN_XML_SCHEMENO_NODE_NAME);
            if (mpfPlanSchemeNoNode.Count > 0)
            {
                MPFPlanSchemeNo = mpfPlanSchemeNoNode[0].InnerText.Trim();
            }
            else
                MPFPlanSchemeNo = string.Empty;

            // get Sequence No
            System.Xml.XmlNodeList mpfPlanPlanNoNode = HROne.CommonLib.Utility.GetXmlDocumentByDataString(MPFPlanExtendXMLString).GetElementsByTagName(MPF_PLAN_XML_SEQUENCE_NODE_NAME);
            if (mpfPlanPlanNoNode.Count > 0)
            {
                if (!int.TryParse(mpfPlanPlanNoNode[0].InnerText.Trim(), out m_SequenceNo))
                    m_SequenceNo = 1;
            }
            else
                m_SequenceNo = 1;

            double m_allFigures = 0;

            // CA records
            ArrayList m_caList = new ArrayList();
            double m_caFigures = 0;
            int m_caCount = 0;

            foreach (GenericNewJoinEmployeeMPFFileDetail mpfFileDetail in NewJoinEmployeeMPFFileDetails)
                m_caFigures += GenerateMPFDataByPeriod(mpfFileDetail, m_caList, "CA");
            foreach (GenericExistingEmployeeMPFFileDetail bankFileDetail in ExistingEmployeeMPFFileDetails)
                m_caFigures += GenerateMPFDataByPeriod(bankFileDetail, m_caList, "CA");
            foreach (GenericBackPaymentEmployeeMPFFileDetail bankFileDetail in BackPaymentEmployeeMPFFileDetails)
                m_caFigures += GenerateMPFDataByPeriod(bankFileDetail, m_caList, "CA");

            string m_caBody = "";
            foreach (MPFPeriod mpfPeriod in m_caList)
            {
                foreach (string mpfContributionString in mpfPeriod.DetailStringList)
                {
                    m_caBody += mpfContributionString + RECORD_DELIMITER;
                    m_caCount++;
                }
            }

            if (m_caCount > 0)
            {
                m_caCount += 2; // includes header and footer

                string[] m_caHeader = new string[2];
                m_caHeader[0] = "CA";
                m_caHeader[1] = this.MPFPlanParticipationNo.PadRight(4).Substring(0, 4);


                // writing Periodic Contribution details
                string[] m_caFooter = new string[4];
                m_caFooter[0] = "CA";
                m_caFooter[1] = this.MPFPlanParticipationNo.PadRight(4).Substring(0, 4);
                m_caFooter[2] = m_caFigures.ToString("0000000000000.00").Replace(".", "");
                m_caFooter[3] = m_caCount.ToString("00000000");

                mpfFileData += string.Join("", m_caHeader) + RECORD_DELIMITER + m_caBody + (string.Join("", m_caFooter) + RECORD_DELIMITER);
            }

            // Terminated staffs records
            int m_tdCount = 0;
            string m_tdBody = "";
            foreach (GenericExistingEmployeeMPFFileDetail bankFileDetail in ExistingEmployeeMPFFileDetails)
            {
                if (bankFileDetail.LastEmploymentDate.Ticks != 0)
                {
                    string[] m_tdRecord = new string[23];
                    m_tdRecord[0] = "TD";
                    m_tdRecord[1] = this.MPFPlanParticipationNo.PadRight(4).Substring(0, 4);
                    m_tdRecord[2] = bankFileDetail.HKIDPassport.PadRight(15).Substring(0, 15);
                    m_tdRecord[3] = this.MPFSchemeCode.PadRight(4).Substring(0, 4);
                    m_tdRecord[4] = "M";
                    m_tdRecord[5] = bankFileDetail.TermCode.PadRight(2).Substring(0, 2);
                    m_tdRecord[6] = "".PadRight(2);
                    m_tdRecord[7] = bankFileDetail.LastEmploymentDate.ToString("yyyyMMdd");
                    m_tdRecord[8] = "".PadRight(8);
                    m_tdRecord[9] = "".PadRight(8);
                    m_tdRecord[10] = "".PadRight(8);
                    m_tdRecord[11] = "".PadRight(8);
                    m_tdRecord[12] = "".PadRight(1);
                    m_tdRecord[13] = "".PadRight(15);
                    m_tdRecord[14] = "".PadRight(15);
                    m_tdRecord[15] = "".PadRight(1);
                    m_tdRecord[16] = "".PadRight(15);
                    m_tdRecord[17] = "".PadRight(15);
                    m_tdRecord[18] = "".PadRight(15);
                    m_tdRecord[19] = "".PadRight(15);
                    m_tdRecord[20] = "".PadRight(1);
                    m_tdRecord[21] = "".PadRight(15);
                    m_tdRecord[22] = "".PadRight(8);

                    m_tdBody += string.Join(FIELD_DELIMITER, m_tdRecord) + RECORD_DELIMITER;

                    m_tdCount++;
                }
            }

            if (m_tdCount > 0)
            {
                m_tdCount += 2; // includes header and footer

                string[] m_tdHeader = new string[2];
                m_tdHeader[0] = "TD";
                m_tdHeader[1] = this.MPFPlanParticipationNo.PadRight(4).Substring(0, 4);

                string[] m_tdFooter = new string[4];
                m_tdFooter[0] = "TD";
                m_tdFooter[1] = this.MPFPlanParticipationNo.PadRight(4).Substring(0, 4);
                m_tdFooter[2] = "000000000000000";
                m_tdFooter[3] = m_tdCount.ToString("00000000");

                mpfFileData += string.Join("", m_tdHeader) + RECORD_DELIMITER +
                               m_tdBody + 
                               string.Join("", m_tdFooter) + RECORD_DELIMITER;
            }

            // file level
            string[] mpfFileHeader = new string[3];
            mpfFileHeader[0] = "FH";
            mpfFileHeader[1] = m_mpfPlanObj.MPFPlanParticipationNo.PadRight(4).Substring(0, 4);
            mpfFileHeader[2] = m_mpfPlanObj.MPFPlanCompanyName.PadRight(40).Substring(0, 40);

            string[] mpfFileFooter = new string[4];
            mpfFileFooter[0] = "FT";
            mpfFileFooter[1] = MPFPlanParticipationNo.PadRight(4).Substring(0, 4);
            int m_dataTypeCount = ((m_caCount > 0) ? 1 : 0) + ((m_tdCount > 0) ? 1: 0);
            mpfFileFooter[2] = m_dataTypeCount.ToString("00");
            mpfFileFooter[3] = ((int)(m_caCount + m_tdCount + 2)).ToString("00000000");

            mpfFileData = string.Join(FIELD_DELIMITER, mpfFileHeader) + RECORD_DELIMITER + 
                          mpfFileData +  
                          string.Join(FIELD_DELIMITER, mpfFileFooter);

            FileInfo result = GenerateTempFileName();
            StreamWriter writer = new StreamWriter(result.OpenWrite());
            writer.Write(mpfFileData);
            writer.Close();

            SaveSequenceNo(m_SequenceNo + 1);
            return result;
        }

        private void SaveSequenceNo(int NewSequenceNo)
        {
            EMPFPlan mpfPlan = new EMPFPlan();
            mpfPlan.MPFPlanID = MPFPlanID;
            if (EMPFPlan.db.select(dbConn, mpfPlan))
            {
                System.Xml.XmlDocument xmlDoc = HROne.CommonLib.Utility.GetXmlDocumentByDataString(mpfPlan.MPFPlanExtendData);
                System.Xml.XmlNodeList nodeList = xmlDoc.GetElementsByTagName(MPF_PLAN_XML_SEQUENCE_NODE_NAME);
                System.Xml.XmlNode node;
                if (nodeList.Count > 0)
                    node = nodeList[0];
                else
                {
                    node = xmlDoc.CreateElement(MPF_PLAN_XML_SEQUENCE_NODE_NAME);
                    xmlDoc.DocumentElement.AppendChild(node);
                }

                node.InnerText = NewSequenceNo.ToString();
                mpfPlan.MPFPlanExtendData = xmlDoc.InnerXml;
                EMPFPlan.db.update(dbConn, mpfPlan);
            }

        }

        //private double GenerateMPFDataByPeriod(GenericExistingEmployeeMPFFileDetail mpfFileDetail, ArrayList periodList, string recordType)
        private double GenerateMPFDataByPeriod(GenericMPFFileDetail mpfFileDetail, ArrayList periodList, string recordType)
        {
            double m_totalFigures = 0; // for use in data segment footer
            
            //ECessationReason m_empTermReason = new ECessationReason();
            //m_empTermReason.CessationReasonID = mpfFileDetail.CessationReasonID;
            //m_empTermReason = ECessationReason.db.select(dbConn, m_empTermReason);
            //if (m_empTermReason)
            //    return 0;


            foreach (GenericMPFFileContributionDetail contributionDetail in mpfFileDetail.MPFContributionDetailList)
            {

                MPFPeriod currentMPFPeriod = null;
                foreach (MPFPeriod mpfPeriod in periodList)
                {
                    if (mpfPeriod.PeriodFrom < contributionDetail.PeriodTo && mpfPeriod.PeriodTo > contributionDetail.PeriodFrom)
                    {
                        if (mpfPeriod.PeriodFrom > contributionDetail.PeriodFrom)
                            mpfPeriod.PeriodFrom = contributionDetail.PeriodFrom;
                        if (mpfPeriod.PeriodTo < contributionDetail.PeriodTo)
                            mpfPeriod.PeriodFrom = contributionDetail.PeriodFrom;
                        currentMPFPeriod = mpfPeriod;
                    }
                }
                if (currentMPFPeriod == null)
                {
                    currentMPFPeriod = CreateMPFPeriod(contributionDetail.PeriodFrom, contributionDetail.PeriodTo);
                    periodList.Add(currentMPFPeriod);
                }

                m_totalFigures += (contributionDetail.VCEE +
                                   contributionDetail.VCER +
                                   contributionDetail.MCEE +
                                   contributionDetail.MCER +
                                   contributionDetail.RelevantIncome +
                                   contributionDetail.VCRelevantIncome);

                if (recordType == "CA")
                {
                    string[] mpfFileMCDetailStringList = new string[15];
                    mpfFileMCDetailStringList[0] = recordType.PadRight(2).Substring(0, 2);
                    mpfFileMCDetailStringList[1] = this.MPFPlanParticipationNo.PadRight(4).Substring(0, 4);
                    mpfFileMCDetailStringList[2] = mpfFileDetail.HKIDPassport.PadRight(15).Substring(0, 15);
                    mpfFileMCDetailStringList[3] = "FMPF"; // <-- hardcode for MT000288 Fidelity scheme // this.MPFSchemeCode.PadRight(4).Substring(0, 4);
                    mpfFileMCDetailStringList[4] = contributionDetail.PeriodFrom.ToString("yyyyMMdd").PadRight(8).Substring(0, 8);
                    mpfFileMCDetailStringList[5] = contributionDetail.PeriodTo.ToString("yyyyMMdd").PadRight(8).Substring(0, 8);
                    mpfFileMCDetailStringList[6] = "M"; // MPF Mandatory contribution
                    mpfFileMCDetailStringList[7] = "RG";
                    mpfFileMCDetailStringList[8] = contributionDetail.MCER.ToString("0000000000000.00").Replace(".", "");
                    mpfFileMCDetailStringList[9] = contributionDetail.MCEE.ToString("0000000000000.00").Replace(".", "");
                    mpfFileMCDetailStringList[10] = currentMPFPeriod.PeriodFrom.ToString("yyyyMMdd").PadRight(8).Substring(0, 8);
                    mpfFileMCDetailStringList[11] = "".PadRight(15);
                    mpfFileMCDetailStringList[12] = "".PadRight(2);
                    mpfFileMCDetailStringList[13] = "".PadRight(1);
                    mpfFileMCDetailStringList[14] = contributionDetail.RelevantIncome.ToString("0000000000000.00").Replace(".", "");

                    currentMPFPeriod.DetailStringList.Add(string.Join(FIELD_DELIMITER, mpfFileMCDetailStringList));
                }

                if (contributionDetail.VCEE != 0 || contributionDetail.VCER != 0)
                {
                    //string vcPlanNo = string.Empty;

                    //System.Xml.XmlNodeList vcPlanNoNodeNode = HROne.CommonLib.Utility.GetXmlDocumentByDataString(mpfFileDetail.EmpAVCPlanExtendXMLString).GetElementsByTagName("EmpAVCPlanBOCIVCPlanNo");
                    //if (vcPlanNoNodeNode.Count > 0)
                    //    vcPlanNo = vcPlanNoNodeNode[0].InnerText;
                    //if (string.IsNullOrEmpty(vcPlanNo))
                    //{
                    //    throw new Exception("Voluntary Plan No. is missing:" + mpfFileDetail.EmpNo + " - " + mpfFileDetail.EmpName);
                    //}

                    if (recordType == "CA")
                    {
                        string[] mpfFileVCDetailStringList = new string[15];
                        mpfFileVCDetailStringList[0] = recordType.PadRight(2).Substring(0, 2);
                        mpfFileVCDetailStringList[1] = this.MPFPlanParticipationNo.PadRight(4).Substring(0, 4);
                        mpfFileVCDetailStringList[2] = mpfFileDetail.HKIDPassport.PadRight(15).Substring(0, 15);
                        mpfFileVCDetailStringList[3] = this.MPFSchemeCode.PadRight(4).Substring(0, 4);
                        mpfFileVCDetailStringList[4] = contributionDetail.PeriodFrom.ToString("yyyyMMdd").PadRight(8).Substring(0, 8);
                        mpfFileVCDetailStringList[5] = contributionDetail.PeriodTo.ToString("yyyyMMdd").PadRight(8).Substring(0, 8);
                        mpfFileVCDetailStringList[6] = "V"; // Voluntary Mandatory contribution
                        mpfFileVCDetailStringList[7] = "RG";
                        mpfFileVCDetailStringList[8] = contributionDetail.VCER.ToString("0000000000000.00").Replace(".", "");
                        mpfFileVCDetailStringList[9] = contributionDetail.VCEE.ToString("0000000000000.00").Replace(".", "");
                        mpfFileVCDetailStringList[10] = currentMPFPeriod.PeriodFrom.ToString("yyyyMMdd");
                        mpfFileVCDetailStringList[11] = "".PadRight(15);
                        mpfFileVCDetailStringList[12] = "".PadRight(2);
                        mpfFileVCDetailStringList[13] = "".PadRight(1);
                        mpfFileVCDetailStringList[14] = contributionDetail.RelevantIncome.ToString("0000000000000.00").Replace(".", "");

                        currentMPFPeriod.DetailStringList.Add(string.Join(FIELD_DELIMITER, mpfFileVCDetailStringList));
                    }
                }
            }
            return m_totalFigures;
        }

        private MPFPeriod CreateMPFPeriod(DateTime PeriodFrom, DateTime PeriodTo)
        {
            MPFPeriod mpfPeriod = new MPFPeriod();
            mpfPeriod.PeriodFrom = PeriodFrom;
            mpfPeriod.PeriodTo = PeriodTo;
            mpfPeriod.DetailStringList = new ArrayList();
            return mpfPeriod;
        }
        //protected override string GenerateExistingMPFFileDetail(GenericExistingEmployeeMPFFileDetail mpfDataDetail)
        //{
        //    string result = string.Empty;
        //    foreach (GenericMPFFileContributionDetail mpfContributionDetail in mpfDataDetail.MPFContributionDetailList)
        //    {
        //        HashCount++;
        //        ExistingEntitiesCount++;
        //        string[] mpfFileDetail = new string[19];
        //        mpfFileDetail[0] = FormNo;
        //        mpfFileDetail[1] = string.Empty.PadRight(46);
        //        mpfFileDetail[2] = "91";
        //        mpfFileDetail[3] = HashCount.ToString("000000");
        //        mpfFileDetail[4] = mpfDataDetail.HKIDPassport.PadRight(16).Substring(0, 16);
        //        mpfFileDetail[5] = mpfDataDetail.SchemeJoinDate.ToString("yyyyMMdd");
        //        mpfFileDetail[6] += mpfContributionDetail.PeriodTo.ToString("yyyyMMdd");
        //        mpfFileDetail[7] += mpfContributionDetail.RelevantIncome.ToString("0000000000000.00").Replace(".", "");
        //        mpfFileDetail[8] += mpfContributionDetail.MCER.ToString("0000000.00").Replace(".", "");
        //        mpfFileDetail[9] += mpfContributionDetail.MCEE.ToString("0000000.00").Replace(".", "");
        //        mpfFileDetail[10] += mpfContributionDetail.VCER.ToString("0000000.00").Replace(".", "");
        //        mpfFileDetail[11] += mpfContributionDetail.VCEE.ToString("0000000.00").Replace(".", "");
        //        mpfFileDetail[12] += mpfContributionDetail.PeriodFrom.ToString("yyyyMMdd");

        //        if (!mpfDataDetail.LastEmploymentDate.Ticks.Equals(0))
        //        {
        //            mpfFileDetail[13] = mpfDataDetail.LastEmploymentDate.ToString("yyyyMMdd");
        //            mpfFileDetail[14] = mpfDataDetail.TermCode.PadRight(2).Substring(0, 2);
        //            mpfFileDetail[15] = mpfDataDetail.LastPaymentDate.ToString("yyyyMMdd");
        //            if (mpfDataDetail.LspSpFlag == LSPSP_FLAG_ENUM.LSP || mpfDataDetail.LspSpFlag == LSPSP_FLAG_ENUM.SP)
        //            {
        //                if (mpfDataDetail.LspSpFlag == LSPSP_FLAG_ENUM.LSP)
        //                    mpfFileDetail[16] = "L";
        //                else if (mpfDataDetail.LspSpFlag == LSPSP_FLAG_ENUM.SP)
        //                    mpfFileDetail[16] = "S";
        //                mpfFileDetail[17] = mpfDataDetail.LSPSPAmount.ToString("0000000000000.00").Replace(".", "");
        //                mpfFileDetail[18] = mpfDataDetail.LSPSPAmountPaidByER.ToString("0000000000000.00").Replace(".", "");
        //            }
        //            else
        //            {
        //                mpfFileDetail[16] = string.Empty.PadRight(1);
        //                mpfFileDetail[17] = string.Empty.PadRight(15, '0');
        //                mpfFileDetail[18] = string.Empty.PadRight(15, '0');
        //            }
        //        }
        //        else
        //        {
        //            mpfFileDetail[13] = string.Empty.PadRight(8);
        //            mpfFileDetail[14] = string.Empty.PadRight(2);
        //            mpfFileDetail[15] = string.Empty.PadRight(8);
        //            mpfFileDetail[16] = string.Empty.PadRight(1);
        //            mpfFileDetail[17] = string.Empty.PadRight(15, '0');
        //            mpfFileDetail[18] = string.Empty.PadRight(15, '0');
        //        }

        //        AddTransactionCommonInfoString(mpfFileDetail);

        //        if (result.Equals(string.Empty))
        //            result = string.Join(FIELD_DELIMITER, mpfFileDetail);
        //        else
        //            result += RECORD_DELIMITER + string.Join(FIELD_DELIMITER, mpfFileDetail);
        //    }
        //    return result;
        //}

        //protected override string GenerateNewJoinMPFFileDetail(GenericNewJoinEmployeeMPFFileDetail mpfDataDetail)
        //{
        //    HashCount++;
        //    NewJoinEntitiesCount++;
        //    string[] mpfFileDetail = new string[24];
        //    mpfFileDetail[0] = FormNo;
        //    mpfFileDetail[1] = string.Empty.PadRight(46);
        //    mpfFileDetail[2] = "92";
        //    mpfFileDetail[3] = HashCount.ToString("000000");
        //    mpfFileDetail[4] = mpfDataDetail.HKIDPassport.PadRight(16).Substring(0, 16);
        //    if (mpfDataDetail.HKIDType == IDENTITY_TYPE_ENUM.HKID)
        //        mpfFileDetail[5] = "H";
        //    else if (mpfDataDetail.HKIDType == IDENTITY_TYPE_ENUM.PASSPORT)
        //        mpfFileDetail[5] = "P";
        //    else
        //        mpfFileDetail[5] = " ";
        //    mpfFileDetail[6] = mpfDataDetail.SchemeJoinDate.ToString("yyyyMMdd");
        //    //System.Xml.XmlNodeList classNameNode = HROne.CommonLib.Utility.GetXmlDocumentByDataString(mpfDataDetail.EmpMPFPlanExtendXMLString).GetElementsByTagName("EmpMPFPlanClassName");
        //    //if (classNameNode.Count > 0)
        //    //    mpfFileDetail[7] = classNameNode[0].InnerText.PadRight(8).Substring(0, 8).ToUpper();
        //    //else
        //    //    mpfFileDetail[7] = DefaultClassName.PadRight(8).Substring(0,8);

        //    mpfFileDetail[8] = mpfDataDetail.EmpSurname.Replace(",", " ").Replace("  ", " ").PadRight(30).Substring(0, 30).ToUpper();
        //    mpfFileDetail[9] = mpfDataDetail.EmpOtherName.Replace(",", " ").Replace("  ", " ").PadRight(20).Substring(0, 20).ToUpper();
        //    mpfFileDetail[10] = "   ";  // Member Initial, N/A
        //    mpfFileDetail[11] = mpfDataDetail.DateOfBirth.ToString("yyyyMMdd");
        //    mpfFileDetail[12] = mpfDataDetail.Sex.PadRight(1).Substring(0, 1).ToUpper();

        //    mpfFileDetail[13] = string.Empty;
        //    mpfFileDetail[14] = string.Empty;
        //    mpfFileDetail[15] = string.Empty;
        //    mpfFileDetail[16] = string.Empty;
        //    mpfFileDetail[17] = string.Empty;
        //    mpfFileDetail[18] = string.Empty;
        //    mpfFileDetail[19] = string.Empty;

        //    int periodCount = 0;
        //    foreach (GenericMPFFileContributionDetail mpfContributionDetail in mpfDataDetail.MPFContributionDetailList)
        //    {
        //        periodCount++;

        //        mpfFileDetail[13] += mpfContributionDetail.PeriodTo.ToString("yyyyMMdd");
        //        mpfFileDetail[14] += mpfContributionDetail.RelevantIncome.ToString("0000000000000.00").Replace(".", "");
        //        mpfFileDetail[15] += mpfContributionDetail.MCER.ToString("0000000.00").Replace(".", "");
        //        mpfFileDetail[16] += mpfContributionDetail.MCEE.ToString("0000000.00").Replace(".", "");
        //        mpfFileDetail[17] += mpfContributionDetail.VCER.ToString("0000000.00").Replace(".", "");
        //        mpfFileDetail[18] += mpfContributionDetail.VCEE.ToString("0000000.00").Replace(".", "");
        //        mpfFileDetail[19] += mpfContributionDetail.PeriodFrom.ToString("yyyyMMdd");
        //    }

        //    for (int i = periodCount; i < 12; i++)
        //    {
        //        mpfFileDetail[13] += string.Empty.PadLeft(8);
        //        mpfFileDetail[14] += string.Empty.PadLeft(15, '0');
        //        mpfFileDetail[15] += string.Empty.PadLeft(9, '0');
        //        mpfFileDetail[16] += string.Empty.PadLeft(9, '0');
        //        mpfFileDetail[17] += string.Empty.PadLeft(9, '0');
        //        mpfFileDetail[18] += string.Empty.PadLeft(9, '0');
        //        mpfFileDetail[19] += string.Empty.PadLeft(8);
        //    }
        //    mpfFileDetail[20] = "      ";   // saluation, not required
        //    if (mpfDataDetail.MemberType == MEMBER_TYPE_ENUM.NORMAL)
        //        mpfFileDetail[21] = "1";
        //    else if (mpfDataDetail.MemberType == MEMBER_TYPE_ENUM.CASUAL)
        //        mpfFileDetail[21] = "2";
        //    else if (mpfDataDetail.MemberType == MEMBER_TYPE_ENUM.EXEMPT)
        //        mpfFileDetail[21] = "3";
        //    else
        //        mpfFileDetail[21] = " ";
        //    mpfFileDetail[22] = mpfDataDetail.DateJoined.ToString("yyyyMMdd");
        //    mpfFileDetail[23] = mpfDataDetail.IsTransfer ? "1" : " ";

        //    AddTransactionCommonInfoString(mpfFileDetail);

        //    return string.Join(FIELD_DELIMITER, mpfFileDetail);
        //}

        //protected override string GenerateAdditionalMPFFileDetail(GenericAdditionalEmployeeMPFFileDetail mpfDataDetail)
        //{
        //    string result = string.Empty;
        //    foreach (GenericMPFFileContributionDetail mpfContributionDetail in mpfDataDetail.MPFContributionDetailList)
        //    {
        //        HashCount++;
        //        string[] mpfFileDetail = new string[19];
        //        mpfFileDetail[0] = FormNo;
        //        mpfFileDetail[1] = string.Empty.PadRight(46);
        //        mpfFileDetail[2] = "93";
        //        mpfFileDetail[3] = HashCount.ToString("000000");
        //        mpfFileDetail[4] = mpfDataDetail.HKIDPassport.PadRight(16).Substring(0, 16);
        //        mpfFileDetail[5] = mpfDataDetail.SchemeJoinDate.ToString("yyyyMMdd");
        //        mpfFileDetail[6] += mpfContributionDetail.PeriodTo.ToString("yyyyMMdd");
        //        mpfFileDetail[7] += mpfContributionDetail.RelevantIncome.ToString("0000000000000.00").Replace(".", "");
        //        mpfFileDetail[8] += mpfContributionDetail.MCER.ToString("0000000.00").Replace(".", "");
        //        mpfFileDetail[9] += mpfContributionDetail.MCEE.ToString("0000000.00").Replace(".", "");
        //        mpfFileDetail[10] += mpfContributionDetail.VCER.ToString("0000000.00").Replace(".", "");
        //        mpfFileDetail[11] += mpfContributionDetail.VCEE.ToString("0000000.00").Replace(".", "");
        //        mpfFileDetail[12] += mpfContributionDetail.PeriodFrom.ToString("yyyyMMdd");

        //        if (!mpfDataDetail.LastEmploymentDate.Ticks.Equals(0))
        //        {
        //            mpfFileDetail[13] = mpfDataDetail.LastEmploymentDate.ToString("yyyyMMdd");
        //            mpfFileDetail[14] = mpfDataDetail.TermCode.PadRight(2).Substring(0, 2);
        //            mpfFileDetail[15] = mpfDataDetail.LastPaymentDate.ToString("yyyyMMdd");

        //            if (mpfDataDetail.LspSpFlag == LSPSP_FLAG_ENUM.LSP || mpfDataDetail.LspSpFlag == LSPSP_FLAG_ENUM.SP)
        //            {
        //                if (mpfDataDetail.LspSpFlag == LSPSP_FLAG_ENUM.LSP)
        //                    mpfFileDetail[16] = "L";
        //                else if (mpfDataDetail.LspSpFlag == LSPSP_FLAG_ENUM.SP)
        //                    mpfFileDetail[16] = "S";
        //                mpfFileDetail[17] = mpfDataDetail.LSPSPAmount.ToString("0000000000000.00").Replace(".", "");
        //                mpfFileDetail[18] = mpfDataDetail.LSPSPAmountPaidByER.ToString("0000000000000.00").Replace(".", "");
        //            }
        //            else
        //            {
        //                mpfFileDetail[16] = string.Empty.PadRight(1);
        //                mpfFileDetail[17] = string.Empty.PadRight(15, '0');
        //                mpfFileDetail[18] = string.Empty.PadRight(15, '0');
        //            }
        //        }
        //        else
        //        {
        //            mpfFileDetail[13] = string.Empty.PadRight(8);
        //            mpfFileDetail[14] = string.Empty.PadRight(2);
        //            mpfFileDetail[15] = string.Empty.PadRight(8);
        //            mpfFileDetail[16] = string.Empty.PadRight(1);
        //            mpfFileDetail[17] = string.Empty.PadRight(15, '0');
        //            mpfFileDetail[18] = string.Empty.PadRight(15, '0');
        //        }

        //        AddTransactionCommonInfoString(mpfFileDetail);

        //        if (result.Equals(string.Empty))
        //            result = string.Join(FIELD_DELIMITER, mpfFileDetail);
        //        else
        //            result += RECORD_DELIMITER + string.Join(FIELD_DELIMITER, mpfFileDetail);
        //    }
        //    return result;
        //}

        //protected override string GenerateBackPaymentMPFFileDetail(GenericBackPaymentEmployeeMPFFileDetail mpfDataDetail)
        //{
        //    string result = string.Empty;
        //    foreach (GenericMPFFileContributionDetail mpfContributionDetail in mpfDataDetail.MPFContributionDetailList)
        //    {
        //        HashCount++;
        //        ExistingEntitiesCount++;
        //        string[] mpfFileDetail = new string[19];
        //        mpfFileDetail[0] = FormNo;
        //        mpfFileDetail[1] = string.Empty.PadRight(46);
        //        mpfFileDetail[2] = "94";
        //        mpfFileDetail[3] = HashCount.ToString("000000");
        //        mpfFileDetail[4] = mpfDataDetail.HKIDPassport.PadRight(16).Substring(0, 16);
        //        mpfFileDetail[5] = mpfDataDetail.SchemeJoinDate.ToString("yyyyMMdd");
        //        mpfFileDetail[6] += this.PayPeriodTo.ToString("yyyyMMdd"); //mpfContributionDetail.PeriodTo.ToString("yyyyMMdd");
        //        mpfFileDetail[7] += mpfContributionDetail.RelevantIncome.ToString("0000000000000.00").Replace(".", "");
        //        mpfFileDetail[8] += mpfContributionDetail.MCER.ToString("0000000.00").Replace(".", "");
        //        mpfFileDetail[9] += mpfContributionDetail.MCEE.ToString("0000000.00").Replace(".", "");
        //        mpfFileDetail[10] += mpfContributionDetail.VCER.ToString("0000000.00").Replace(".", "");
        //        mpfFileDetail[11] += mpfContributionDetail.VCEE.ToString("0000000.00").Replace(".", "");
        //        mpfFileDetail[12] += this.PayPeriodFr.ToString("yyyyMMdd"); //mpfContributionDetail.PeriodFrom.ToString("yyyyMMdd");

        //        if (!mpfDataDetail.LastEmploymentDate.Ticks.Equals(0))
        //        {
        //            mpfFileDetail[13] = "        ";// mpfDataDetail.LastEmploymentDate.ToString("yyyyMMdd");
        //            mpfFileDetail[14] = "  "; mpfDataDetail.TermCode.PadRight(2).Substring(0, 2);
        //            mpfFileDetail[15] = "        "; //mpfDataDetail.LastPaymentDate.ToString("yyyyMMdd");
        //            if (mpfDataDetail.LspSpFlag == LSPSP_FLAG_ENUM.LSP || mpfDataDetail.LspSpFlag == LSPSP_FLAG_ENUM.SP)
        //            {
        //                if (mpfDataDetail.LspSpFlag == LSPSP_FLAG_ENUM.LSP)
        //                    mpfFileDetail[16] = "L";
        //                else if (mpfDataDetail.LspSpFlag == LSPSP_FLAG_ENUM.SP)
        //                    mpfFileDetail[16] = "S";
        //                mpfFileDetail[17] = mpfDataDetail.LSPSPAmount.ToString("0000000000000.00").Replace(".", "");
        //                mpfFileDetail[18] = mpfDataDetail.LSPSPAmountPaidByER.ToString("0000000000000.00").Replace(".", "");
        //            }
        //            else
        //            {
        //                mpfFileDetail[16] = string.Empty.PadRight(1);
        //                mpfFileDetail[17] = string.Empty.PadRight(15, '0');
        //                mpfFileDetail[18] = string.Empty.PadRight(15, '0');
        //            }
        //        }
        //        else
        //        {
        //            mpfFileDetail[13] = string.Empty.PadRight(8);
        //            mpfFileDetail[14] = string.Empty.PadRight(2);
        //            mpfFileDetail[15] = string.Empty.PadRight(8);
        //            mpfFileDetail[16] = string.Empty.PadRight(1);
        //            mpfFileDetail[17] = string.Empty.PadRight(15, '0');
        //            mpfFileDetail[18] = string.Empty.PadRight(15, '0');
        //        }

        //        AddTransactionCommonInfoString(mpfFileDetail);

        //        if (result.Equals(string.Empty))
        //            result = string.Join(FIELD_DELIMITER, mpfFileDetail);
        //        else
        //            result += RECORD_DELIMITER + string.Join(FIELD_DELIMITER, mpfFileDetail);
        //    }
        //    return result;
        //}

        public override string ActualMPFFileName()
        {
            return MPFPlanParticipationNo.PadRight(4).Substring(0, 4) +
                   AppUtils.ServerDateTime().ToString("yyyyMMdd") +
                   m_SequenceNo.ToString("00") + 
                   MPFFileExtension();
        }

        public override string MPFFileExtension()
        {
            return ".txt";
        }
    }
}