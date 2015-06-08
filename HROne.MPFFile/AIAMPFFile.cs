using System;
using System.Collections.Generic;
using System.Data;
using System.Configuration;
using System.IO;
using HROne.Lib.Entities;
using HROne.DataAccess;

namespace HROne.MPFFile
{
    /// <summary>
    /// Summary description for ManulifeMPFFile
    /// </summary>
    public class AIAMPFFile : GenericMPFFile
    {
        public const string MPF_PLAN_XML_PLAN_NO = "MPFPlanAIAERPlanNo";
        public const string MPF_PLAN_XML_PAY_FREQUENCY = "MPFPlanAIAPayFrequency";
        public const string EMP_MPF_XML_BENEFIT_PLAN_NO = "EmpMPFPlanAIABenefitPlanNo";


        private const string FIELD_DELIMITER = "";
        private const string RECORD_DELIMITER = "\r\n";


        //private int HashCount = 0;
        //private int NewJoinEntitiesCount = 0;
        //private int ExistingEntitiesCount = 0;
        private DateTime TransactionTime;


        //public int SequenceNo = 1;
        //public int SequenceNo
        //{
        //    get { return m_SequenceNo; }
        //    set { m_SequenceNo = value; }
        //}





        protected string MPFPlanPlanNo = string.Empty;
        protected string MPFPlanPayFrequency = "M";
        protected double AIATotalVCRelevantIncome = 0;

        public AIAMPFFile(DatabaseConnection dbConn)
            : base(dbConn)
        {
        }

        public override FileInfo GenerateMPFFile()
        {
            TransactionTime = AppUtils.ServerDateTime();

            int mpfFileContributionRecordCount = 0;
            int mpfFileTerminationRecordCount = 0;

            string mpfFileContributionHeaderString = string.Empty;
            string mpfFileTerminationHeaderString = string.Empty;
            string mpfFileContributionData = string.Empty;
            string mpfFileTerminationData = string.Empty;


            // get Plan No
            System.Xml.XmlNodeList mpfPlanPlanNoNode = HROne.CommonLib.Utility.GetXmlDocumentByDataString(MPFPlanExtendXMLString).GetElementsByTagName(MPF_PLAN_XML_PLAN_NO);
            if (mpfPlanPlanNoNode.Count > 0)
            {
                MPFPlanPlanNo = mpfPlanPlanNoNode[0].InnerText.Trim();
            }
            else
                MPFPlanPlanNo = string.Empty;

            // get Payroll Frequency
            System.Xml.XmlNodeList mpfPlanPayFrequencyNode = HROne.CommonLib.Utility.GetXmlDocumentByDataString(MPFPlanExtendXMLString).GetElementsByTagName(MPF_PLAN_XML_PAY_FREQUENCY);
            if (mpfPlanPayFrequencyNode.Count > 0)
            {
                MPFPlanPayFrequency = mpfPlanPayFrequencyNode[0].InnerText.Trim();
            }


            foreach (GenericExistingEmployeeMPFFileDetail mpfFileDetail in ExistingEmployeeMPFFileDetails)
            {
                string terminationLine = string.Empty;
                mpfFileContributionData += GenerateMPFDetailLine(mpfFileDetail, out terminationLine) + RECORD_DELIMITER;
                mpfFileContributionRecordCount += mpfFileDetail.MPFContributionDetailList.Count;

                if (!string.IsNullOrEmpty(terminationLine))
                {
                    mpfFileTerminationData += terminationLine + RECORD_DELIMITER;
                    mpfFileTerminationRecordCount++;
                }
            }
            foreach (GenericNewJoinEmployeeMPFFileDetail mpfFileDetail in NewJoinEmployeeMPFFileDetails)
            {
                string terminationLine = string.Empty;
                mpfFileContributionData += GenerateMPFDetailLine(mpfFileDetail, out terminationLine) + RECORD_DELIMITER;
                mpfFileContributionRecordCount += mpfFileDetail.MPFContributionDetailList.Count;

                if (!string.IsNullOrEmpty(terminationLine))
                {
                    mpfFileTerminationData += terminationLine + RECORD_DELIMITER;
                    mpfFileTerminationRecordCount++;
                }
            }
            foreach (GenericAdditionalEmployeeMPFFileDetail mpfFileDetail in AdditionalEmployeeMPFFileDetails)
            {
                string terminationLine = string.Empty;
                mpfFileContributionData += GenerateMPFDetailLine(mpfFileDetail, out terminationLine) + RECORD_DELIMITER;
                mpfFileContributionRecordCount += mpfFileDetail.MPFContributionDetailList.Count;

                if (!string.IsNullOrEmpty(terminationLine))
                {
                    mpfFileTerminationData += terminationLine + RECORD_DELIMITER;
                    mpfFileTerminationRecordCount++;
                }
            }
            foreach (GenericBackPaymentEmployeeMPFFileDetail mpfFileDetail in BackPaymentEmployeeMPFFileDetails)
            {
                string terminationLine = string.Empty;
                mpfFileContributionData += GenerateMPFDetailLine(mpfFileDetail, out terminationLine) + RECORD_DELIMITER;
                mpfFileContributionRecordCount += mpfFileDetail.MPFContributionDetailList.Count;

                if (!string.IsNullOrEmpty(terminationLine))
                {
                    mpfFileTerminationData += terminationLine + RECORD_DELIMITER;
                    mpfFileTerminationRecordCount++;
                }
            }


            //HashCount = 1;
            {
                string[] mpfFileContributionHeaderArray = new string[12];
                mpfFileContributionHeaderArray[0] = mpfFileContributionRecordCount.ToString("00000");
                mpfFileContributionHeaderArray[1] = MPFPlanPlanNo.PadRight(6).Substring(0, 6);
                mpfFileContributionHeaderArray[2] = TransactionTime.ToString("yyyyMMdd");
                mpfFileContributionHeaderArray[3] = ((double)m_TotalNewJoinMCER
                                + m_TotalExistingMCER
                                + m_TotalAdditionalMCER
                                + m_TotalBackPaymentMCER
                                )
                                .ToString("000000000000000.00").Replace(".", "");
                mpfFileContributionHeaderArray[4] = ((double)m_TotalNewJoinMCEE
                                + m_TotalExistingMCEE
                                + m_TotalAdditionalMCEE
                                + m_TotalBackPaymentMCEE
                                )
                                .ToString("000000000000000.00").Replace(".", "");
                mpfFileContributionHeaderArray[5] = ((double)m_TotalNewJoinVCER
                                + m_TotalExistingVCER
                                + m_TotalAdditionalVCER
                                + m_TotalBackPaymentVCER
                                )
                                .ToString("000000000000000.00").Replace(".", "");
                mpfFileContributionHeaderArray[6] = ((double)m_TotalNewJoinVCEE
                                + m_TotalExistingVCEE
                                + m_TotalAdditionalVCEE
                                + m_TotalBackPaymentVCEE
                                )
                                .ToString("000000000000000.00").Replace(".", "");
                mpfFileContributionHeaderArray[7] = AIATotalVCRelevantIncome.ToString("000000000000000.00").Replace(".", string.Empty);
                mpfFileContributionHeaderArray[8] = TotalRelevantIncome.ToString("000000000000000.00").Replace(".", string.Empty);
                mpfFileContributionHeaderArray[9] = string.Empty.PadRight(20);
                mpfFileContributionHeaderArray[10] = "  ";
                mpfFileContributionHeaderArray[11] = PayPeriodFr.ToString("yyyyMMdd");

                mpfFileContributionHeaderString = string.Join(FIELD_DELIMITER, mpfFileContributionHeaderArray);

                if (mpfFileContributionHeaderString.Length != 151)
                    throw new Exception("Invalid length for file header");
                mpfFileContributionData = mpfFileContributionHeaderString + RECORD_DELIMITER + mpfFileContributionData;
            }

            if (mpfFileTerminationRecordCount>0)
            {
                string[] mpfFileTerminationHeaderArray = new string[3];
                mpfFileTerminationHeaderArray[0] = mpfFileTerminationRecordCount.ToString("00000");
                mpfFileTerminationHeaderArray[1] = MPFPlanPlanNo.PadRight(6).Substring(0, 6);
                mpfFileTerminationHeaderArray[2] = string.Empty.PadRight(20);

                mpfFileTerminationHeaderString = string.Join(FIELD_DELIMITER, mpfFileTerminationHeaderArray);

                mpfFileTerminationData = mpfFileTerminationHeaderString + RECORD_DELIMITER + mpfFileTerminationData;
            }


            FileInfo contributionFileInfo = GenerateTempFileName();
            StreamWriter writer = new StreamWriter(contributionFileInfo.OpenWrite());
            writer.Write(mpfFileContributionData);
            writer.Close();

            //SaveSequenceNo(m_SequenceNo + 1);

            FileInfo zipFileInfo = new FileInfo(Path.GetTempFileName());
            zipFileInfo.MoveTo(zipFileInfo.FullName + ".zip");
            DirectoryInfo zipFolderInfo = zipFileInfo.Directory.CreateSubdirectory("AIAMPFFile" + AppUtils.ServerDateTime().ToString("yyyyMMddHHmmssfff"));
            List<FileInfo> fileInfoList = new List<FileInfo>();
            {
                contributionFileInfo.MoveTo(System.IO.Path.Combine(zipFolderInfo.FullName, ActualMPFContributionFileName()));
                fileInfoList.Add(contributionFileInfo);

                if (mpfFileTerminationRecordCount > 0)
                {
                    FileInfo terminationFileInfo = GenerateTempFileName();
                    writer = new StreamWriter(terminationFileInfo.OpenWrite());
                    writer.Write(mpfFileTerminationData);
                    writer.Close();
                    terminationFileInfo.MoveTo(System.IO.Path.Combine(zipFolderInfo.FullName, ActualMPFTerminationFileName()));
                    fileInfoList.Add(terminationFileInfo);
                }

            }
            zip.Compress(zipFolderInfo.FullName, "*", zipFileInfo.FullName);
            foreach (FileInfo fileInfo in fileInfoList)
                fileInfo.Delete();
            zipFolderInfo.Delete(true);
            return zipFileInfo;

        }

        protected string GenerateMPFDetailLine(GenericMPFFileDetail mpfFileDetail, out string TerminationDetailLine)
        {
            AIAMPFFile mpfFile = (AIAMPFFile)mpfFileDetail.MPFFile;

            string mpfFileDetailLine = string.Empty;
            TerminationDetailLine = string.Empty;

            string BenefitPlanNo = "01";
            System.Xml.XmlNodeList BenefitPlanNoNode = HROne.CommonLib.Utility.GetXmlDocumentByDataString(mpfFileDetail.EmpMPFPlanExtendXMLString).GetElementsByTagName(EMP_MPF_XML_BENEFIT_PLAN_NO);
            if (BenefitPlanNoNode.Count > 0)
            {
                string tmpBenefitPlanNo = BenefitPlanNoNode[0].InnerText.Trim();
                if (!string.IsNullOrEmpty(tmpBenefitPlanNo))
                    BenefitPlanNo = tmpBenefitPlanNo;
            }

            foreach (GenericMPFFileContributionDetail contributionDetail in mpfFileDetail.MPFContributionDetailList)
            {

                string[] mpfFileContributionDetailArray = new string[22];
                mpfFileContributionDetailArray[0] = MPFPlanPlanNo.PadRight(6).Substring(0, 6);
                mpfFileContributionDetailArray[1] = mpfFileDetail.HKIDPassport.PadRight(15).Substring(0, 15);
                mpfFileContributionDetailArray[2] = BenefitPlanNo.PadRight(6).Substring(0, 6);   //default is 01 if not exists
                mpfFileContributionDetailArray[3] = string.Empty.PadRight(17);
                mpfFileContributionDetailArray[4] = "F";    //  support Regular service ONLY
                mpfFileContributionDetailArray[5] = "N";    //  does NOT support Adjustment batch
                mpfFileContributionDetailArray[6] = " ";    //  no adjustment batch so no -ve sign
                mpfFileContributionDetailArray[7] = contributionDetail.MCER.ToString("0000000.00").Replace(".", string.Empty);
                mpfFileContributionDetailArray[8] = " ";    //  no adjustment batch so no -ve sign
                mpfFileContributionDetailArray[9] = contributionDetail.MCEE.ToString("0000000.00").Replace(".", string.Empty);
                if (contributionDetail.VCER == 0)
                {
                    mpfFileContributionDetailArray[10] = "NONE ";    //  no adjustment batch so no -ve sign
                    mpfFileContributionDetailArray[12] = " ";    //  no adjustment batch so no -ve sign
                    mpfFileContributionDetailArray[13] = ((double)0).ToString("0000000.00").Replace(".", string.Empty);
                }
                else
                {
                    mpfFileContributionDetailArray[10] = "SFA  ";    //  no adjustment batch so no -ve sign
                    mpfFileContributionDetailArray[12] = " ";    //  no adjustment batch so no -ve sign
                    mpfFileContributionDetailArray[13] = contributionDetail.VCER.ToString("0000000.00").Replace(".", string.Empty);
                }
                if (contributionDetail.VCEE == 0)
                {
                    mpfFileContributionDetailArray[11] = "NONE ";    //  no adjustment batch so no -ve sign
                    mpfFileContributionDetailArray[14] = " ";    //  no adjustment batch so no -ve sign
                    mpfFileContributionDetailArray[15] = ((double)0).ToString("0000000.00").Replace(".", string.Empty);
                }
                else
                {
                    mpfFileContributionDetailArray[11] = "SFA  ";    //  no adjustment batch so no -ve sign
                    mpfFileContributionDetailArray[14] = " ";    //  no adjustment batch so no -ve sign
                    mpfFileContributionDetailArray[15] = contributionDetail.VCEE.ToString("0000000.00").Replace(".", string.Empty);
                }
                mpfFileContributionDetailArray[16] = "00000";    // use rate=0
                mpfFileContributionDetailArray[17] = "00000";   // use rate=0

                mpfFileContributionDetailArray[18] = contributionDetail.PeriodFrom.ToString("yyyyMMdd");
                mpfFileContributionDetailArray[19] = contributionDetail.PeriodTo.ToString("yyyyMMdd");

                if (contributionDetail.VCEE == 0 && contributionDetail.VCER == 0)
                    mpfFileContributionDetailArray[20] = ((double)0).ToString("00000000000.00".Replace(".", string.Empty));
                else
                {
                    mpfFileContributionDetailArray[20] = contributionDetail.VCRelevantIncome.ToString("00000000000.00").Replace(".", string.Empty);
                    AIATotalVCRelevantIncome += contributionDetail.VCRelevantIncome;
                }
                mpfFileContributionDetailArray[21] = contributionDetail.RelevantIncome.ToString("00000000000.00").Replace(".", string.Empty);

                string tmpDetailLine = string.Join(FIELD_DELIMITER, mpfFileContributionDetailArray);

                if (tmpDetailLine.Length != 148)
                    throw new Exception("Invalid length of detail record");

                if (string.IsNullOrEmpty(mpfFileDetailLine))
                    mpfFileDetailLine = tmpDetailLine;
                else
                    mpfFileDetailLine += RECORD_DELIMITER + tmpDetailLine;


            }
            if (mpfFileDetail is GenericExistingEmployeeMPFFileDetail)
            {
                GenericExistingEmployeeMPFFileDetail existingDetail = (GenericExistingEmployeeMPFFileDetail)mpfFileDetail;
                if (!string.IsNullOrEmpty(existingDetail.TermCode))
                {
                    string[] mpfFileTerminationDetailArray = new string[11];
                    mpfFileTerminationDetailArray[0] = MPFPlanPlanNo.PadRight(6).Substring(0, 6);
                    mpfFileTerminationDetailArray[1] = BenefitPlanNo.PadRight(6).Substring(0, 6);   //default is 01 if not exists
                    mpfFileTerminationDetailArray[2] = mpfFileDetail.HKIDPassport.PadRight(15).Substring(0, 15);
                    mpfFileTerminationDetailArray[3] = string.Empty.PadRight(17);
                    mpfFileTerminationDetailArray[4] = mpfFileDetail.EmpName.PadRight(60).Substring(0, 60);
                    mpfFileTerminationDetailArray[6] = existingDetail.LastEmploymentDate.ToString("yyyyMMdd");
                    mpfFileTerminationDetailArray[7] = existingDetail.LastEmploymentDate.ToString("yyyyMMdd");
                    mpfFileTerminationDetailArray[8] = existingDetail.TermCode.Substring(0, 1);
                    mpfFileTerminationDetailArray[9] = existingDetail.LSPSPAmount.ToString("00000000000.00").Replace(".", string.Empty);
                    mpfFileTerminationDetailArray[10] = existingDetail.LSPSPAmountPaidByER > 0 ? "Y" : "N";

                    string tmpDetailLine = string.Join(FIELD_DELIMITER, mpfFileTerminationDetailArray);

                    if (tmpDetailLine.Length != 135)
                        throw new Exception("Invalid length of detail record");

                    TerminationDetailLine = tmpDetailLine;
                }
            }

            return mpfFileDetailLine;
        }

        //private void SaveSequenceNo(int NewSequenceNo)
        //{
        //    EMPFPlan mpfPlan = new EMPFPlan();
        //    mpfPlan.MPFPlanID = MPFPlanID;
        //    if (EMPFPlan.db.select(dbConn, mpfPlan))
        //    {
        //        System.Xml.XmlDocument xmlDoc = HROne.CommonLib.Utility.GetXmlDocumentByDataString(mpfPlan.MPFPlanExtendData);
        //        System.Xml.XmlNodeList nodeList = xmlDoc.GetElementsByTagName(MPF_PLAN_XML_SEQUENCE_NODE_NAME);
        //        System.Xml.XmlNode sequenceNoNode;
        //        if (nodeList.Count > 0)
        //            sequenceNoNode = nodeList[0];
        //        else
        //        {
        //            sequenceNoNode = xmlDoc.CreateElement(MPF_PLAN_XML_SEQUENCE_NODE_NAME);
        //            xmlDoc.DocumentElement.AppendChild(sequenceNoNode);
        //        }

        //        sequenceNoNode.InnerText = NewSequenceNo.ToString();

        //        nodeList = xmlDoc.GetElementsByTagName(MPF_PLAN_XML_LAST_GENERATE_DATE);
        //        System.Xml.XmlNode lastDateNode;
        //        if (nodeList.Count > 0)
        //            lastDateNode = nodeList[0];
        //        else
        //        {
        //            lastDateNode = xmlDoc.CreateElement(MPF_PLAN_XML_LAST_GENERATE_DATE);
        //            xmlDoc.DocumentElement.AppendChild(lastDateNode);
        //        }

        //        lastDateNode.InnerText = AppUtils.ServerDateTime().ToString("yyyyMMdd");

        //        mpfPlan.MPFPlanExtendData = xmlDoc.InnerXml;
        //        EMPFPlan.db.update(dbConn, mpfPlan);
        //    }

        //}

        public string ActualMPFContributionFileName()
        {
            string filenamePayFrequencySuffex = "cnfm";
            if (MPFPlanPayFrequency.Equals("S"))
                filenamePayFrequencySuffex = "cnfk";
            return MPFPlanPlanNo.Trim() + "_" + TransactionTime.ToString("yyyyMMdd") + "-01" + filenamePayFrequencySuffex + ".txt";
        }

        public string ActualMPFTerminationFileName()
        {
            return MPFPlanPlanNo.Trim() + "_" + TransactionTime.ToString("yyyyMMdd") + "-01Term.txt";
        }

        //public override string ActualMPFFileName()
        //{
        //    return MPFPlanSchemeNo.PadLeft(8, '0').Substring(0, 8) + "MANULIFE" + SequenceNo.ToString("00") + MPFFileExtension();
        //}

        public override string MPFFileExtension()
        {
            return ".zip";
        }
    }
}