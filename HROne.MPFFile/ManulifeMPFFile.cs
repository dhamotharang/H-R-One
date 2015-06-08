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
    /// Summary description for ManulifeMPFFile
    /// </summary>
    public class ManulifeMPFFile : GenericMPFFile
    {
        public const string MPF_PLAN_XML_SUB_SCHEME_NO = "MPFPlanManulifeSubSchemeNo";
        public const string MPF_PLAN_XML_GROUP_NO = "MPFPlanManulifeGroupNo";
        public const string MPF_PLAN_XML_SUB_GROUP_NO = "MPFPlanManulifeSubGroupNo";
        public const string MPF_PLAN_XML_SEQUENCE_NODE_NAME = "MPFPlanManulifeSequenceNo";
        public const string MPF_PLAN_XML_LAST_GENERATE_DATE = "MPFPlanManulifeLastGenerateDate";


        private const string FIELD_DELIMITER = "";
        private const string RECORD_DELIMITER = "\r\n";


        //private int HashCount = 0;
        //private int NewJoinEntitiesCount = 0;
        //private int ExistingEntitiesCount = 0;
        private string MPFPlanSchemeNo;
        private DateTime TransactionTime;


        public int SequenceNo = 1;
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

        ArrayList MPFPeriodList = null;

        protected class MPFPeriod
        {
            public DateTime PeriodFrom;
            public DateTime PeriodTo;
            public ArrayList DetailStringList = null;
            public double TotalContributionAmount = 0;
        }

        public ManulifeMPFFile(DatabaseConnection dbConn)
            : base(dbConn)
        {
        }

        public override FileInfo GenerateMPFFile()
        {
            TransactionTime = AppUtils.ServerDateTime();

            string mpfFileData = string.Empty;
            // get Scheme No
            System.Xml.XmlNodeList mpfPlanSchemeNoNode = HROne.CommonLib.Utility.GetXmlDocumentByDataString(MPFPlanExtendXMLString).GetElementsByTagName(MPF_PLAN_XML_SUB_SCHEME_NO);
            if (mpfPlanSchemeNoNode.Count > 0)
            {
                MPFPlanSchemeNo = mpfPlanSchemeNoNode[0].InnerText.Trim();
            }
            else
                MPFPlanSchemeNo = string.Empty;

            // get Group No
            string MPFPlanGroupNo;

            System.Xml.XmlNodeList mpfPlanGroupNoNode = HROne.CommonLib.Utility.GetXmlDocumentByDataString(MPFPlanExtendXMLString).GetElementsByTagName(MPF_PLAN_XML_GROUP_NO);
            if (mpfPlanGroupNoNode.Count > 0)
            {
                MPFPlanGroupNo = mpfPlanGroupNoNode[0].InnerText.Trim();
            }
            else
                MPFPlanGroupNo = string.Empty;

            // get Sub-group No
            string MPFPlanSubGroupNo;
            System.Xml.XmlNodeList mpfPlanSubGroupNoNode = HROne.CommonLib.Utility.GetXmlDocumentByDataString(MPFPlanExtendXMLString).GetElementsByTagName(MPF_PLAN_XML_SUB_GROUP_NO);
            if (mpfPlanSubGroupNoNode.Count > 0)
            {
                MPFPlanSubGroupNo = mpfPlanSubGroupNoNode[0].InnerText.Trim();
            }
            else
                MPFPlanSubGroupNo = string.Empty;

            // get File Sequence Number

            //string strLastGenerateDate;
            //System.Xml.XmlNodeList mpfPlanLastGenerateDateNode = HROne.CommonLib.Utility.GetXmlDocumentByDataString(MPFPlanExtendXMLString).GetElementsByTagName(MPF_PLAN_XML_LAST_GENERATE_DATE);
            //if (mpfPlanLastGenerateDateNode.Count > 0)
            //{
            //    strLastGenerateDate = mpfPlanLastGenerateDateNode[0].InnerText.Trim();
            //}
            //else
            //{
            //    strLastGenerateDate = string.Empty;
            //}

            //if (strLastGenerateDate.Equals(AppUtils.ServerDateTime().ToString("yyyyMMdd")))
            //{
            //    System.Xml.XmlNodeList mpfPlanSequenceNoNode = HROne.CommonLib.Utility.GetXmlDocumentByDataString(MPFPlanExtendXMLString).GetElementsByTagName(MPF_PLAN_XML_SEQUENCE_NODE_NAME);
            //    if (mpfPlanSequenceNoNode.Count > 0)
            //    {
            //        if (!int.TryParse(mpfPlanSequenceNoNode[0].InnerText.Trim(), out SequenceNo))
            //            SequenceNo = 1;
            //    }
            //    else
            //        SequenceNo = 1;
            //}
            //else
            //    SequenceNo = 1;

            //// get Scheme No
            //System.Xml.XmlNodeList mpfPlanPlanNoNode = HROne.CommonLib.Utility.GetXmlDocumentByDataString(MPFPlanExtendXMLString).GetElementsByTagName("MPFPlanPlanNo");
            //if (mpfPlanPlanNoNode.Count > 0)
            //{
            //    MPFPlanPlanNo = mpfPlanPlanNoNode[0].InnerText.Trim();
            //}
            //else
            //    MPFPlanPlanNo = string.Empty;


            MPFPeriodList = new ArrayList();


            foreach (GenericExistingEmployeeMPFFileDetail bankFileDetail in ExistingEmployeeMPFFileDetails)
                GenerateMPFDataByPeriod(bankFileDetail, MPFPeriodList);

            foreach (GenericNewJoinEmployeeMPFFileDetail mpfFileDetail in NewJoinEmployeeMPFFileDetails)
                GenerateMPFDataByPeriod(mpfFileDetail, MPFPeriodList);

            //foreach (GenericAdditionalEmployeeMPFFileDetail bankFileDetail in AdditionalEmployeeMPFFileDetails)
            //    GenerateMPFDataByPeriod(bankFileDetail, MPFPeriodList);

            foreach (GenericBackPaymentEmployeeMPFFileDetail bankFileDetail in BackPaymentEmployeeMPFFileDetails)
                GenerateMPFDataByPeriod(bankFileDetail, MPFPeriodList);

            int lineCount = 1;  //  Include Header
            for (int idx = 0; idx < MPFPeriodList.Count - 1; idx++)
            {
                MPFPeriod currentMpfPeriod=((MPFPeriod)MPFPeriodList[idx]);
                MPFPeriod NextMpfPeriod=((MPFPeriod)MPFPeriodList[idx+1]);
                if (currentMpfPeriod.PeriodFrom > NextMpfPeriod.PeriodFrom)
                {
                    MPFPeriodList.Remove(currentMpfPeriod);
                    MPFPeriodList.Insert(idx + 1, currentMpfPeriod);
                }
            }
            foreach (MPFPeriod mpfPeriod in MPFPeriodList)
            {

                string[] periodRecordHeader = new string[9];
                periodRecordHeader[0] = "S";
                periodRecordHeader[1] = mpfPeriod.PeriodFrom.ToString("yyyyMMdd");
                periodRecordHeader[2] = mpfPeriod.PeriodTo.ToString("yyyyMMdd");
                periodRecordHeader[3] = "  ";   //  leave blank
                periodRecordHeader[4] = ((double)0).ToString("00.00").Replace(".", "");    //  no surcharge support
                periodRecordHeader[5] = "        "; //  no surcharge support
                periodRecordHeader[6] = mpfPeriod.DetailStringList.Count.ToString("000000");
                periodRecordHeader[7] = mpfPeriod.TotalContributionAmount.ToString("0000000000000.00").Replace(".", "");
                periodRecordHeader[8] = ((double)0).ToString("0000000000000.00").Replace(".", "");  //  no surcharge support
                string periodRecordHeaderString = string.Join(FIELD_DELIMITER, periodRecordHeader);

                mpfFileData += RECORD_DELIMITER + periodRecordHeaderString;

                lineCount++;

                foreach (string mpfContributionString in mpfPeriod.DetailStringList)
                {
                    mpfFileData += RECORD_DELIMITER + mpfContributionString;
                    lineCount++;
                }
            }


            //HashCount = 1;

            string[] mpfFileHeader = new string[8];
            mpfFileHeader[0] = "H";
            mpfFileHeader[1] = MPFPlanGroupNo.PadRight(8).Substring(0, 8);
            mpfFileHeader[2] = MPFPlanSubGroupNo.PadRight(2).Substring(0, 2);
            mpfFileHeader[3] = "D";
            mpfFileHeader[4] = AppUtils.ServerDateTime().ToString("yyyyMMdd");
            mpfFileHeader[5] = lineCount.ToString("000000");
            mpfFileHeader[6] = ((double)TotalAdditionalEmployeeMC
                            + TotalAdditionalEmployeeVC
                            + TotalBackPaymentEmployeeMC
                            + TotalBackPaymentEmployeeVC
                            + TotalExistingEmployeeMC
                            + TotalExistingEmployeeVC
                            + TotalNewJoinEmployeeMC
                            + TotalNewJoinEmployeeVC)
                            .ToString("0000000000000.00").Replace(".", "");
            mpfFileHeader[7] = ((double)0).ToString("0000000000000.00").Replace(".", "");   //  no surcharge support

            mpfFileData = string.Join(FIELD_DELIMITER, mpfFileHeader) + mpfFileData;


            //string[] mpfFileFooter = new string[19];
            //mpfFileFooter[0] = FormNo;
            //mpfFileFooter[1] = string.Empty.PadRight(46);
            //mpfFileFooter[2] = "99";
            //mpfFileFooter[3] = "999999";
            //mpfFileFooter[4] = ((double)(this.TotalAdditionalEmployeeMC + this.TotalAdditionalEmployeeVC + this.TotalBackPaymentEmployeeMC + this.TotalBackPaymentEmployeeVC + this.TotalExistingEmployeeMC + this.TotalExistingEmployeeVC + this.TotalNewJoinEmployeeMC + this.TotalNewJoinEmployeeVC)).ToString("0000000000000000.00").Replace(".", "");
            //mpfFileFooter[5] = "        "; // leave blank for internal use
            //if (PaymentMethod == PaymentMethodEnum.CHEQUE)
            //    mpfFileFooter[6] = "C";
            //else if (PaymentMethod == PaymentMethodEnum.AUTOPAY)
            //    mpfFileFooter[6] = "B";
            //else if (PaymentMethod == PaymentMethodEnum.CASH)
            //    mpfFileFooter[6] = "D";
            //else
            //    mpfFileFooter[6] = " ";
            //mpfFileFooter[7] = "      ";
            //mpfFileFooter[8] = "        ";
            //mpfFileFooter[9] = ExistingEntitiesCount.ToString("0000000");
            //mpfFileFooter[10] = NewJoinEntitiesCount.ToString("0000000");
            //mpfFileFooter[11] = this.TotalExistingEmployeeMC.ToString("0000000000000000.00").Replace(".", "");
            //mpfFileFooter[12] = this.TotalNewJoinEmployeeMC.ToString("0000000000000000.00").Replace(".", "");
            //mpfFileFooter[13] = this.TotalExistingEmployeeVC.ToString("0000000000000000.00").Replace(".", "");
            //mpfFileFooter[14] = this.TotalNewJoinEmployeeVC.ToString("0000000000000000.00").Replace(".", "");
            //mpfFileFooter[15] = this.TotalAdditionalEmployeeMC.ToString("0000000000000000.00").Replace(".", "");
            //mpfFileFooter[16] = this.TotalAdditionalEmployeeVC.ToString("0000000000000000.00").Replace(".", "");
            //mpfFileFooter[17] = this.TotalBackPaymentEmployeeMC.ToString("0000000000000000.00").Replace(".", "");
            //mpfFileFooter[18] = this.TotalBackPaymentEmployeeVC.ToString("0000000000000000.00").Replace(".", "");


            //mpfFileData += string.Join(FIELD_DELIMITER, mpfFileFooter);

            //  HeadCount include footer record
            //HashCount++;

            //string[] mpfFileHeader = new string[9];
            //mpfFileHeader[0] = "HEADER ";
            //mpfFileHeader[1] = "MPF CONTRIBUTION DTL   ";
            //mpfFileHeader[2] = "AMCND   ";
            //mpfFileHeader[3] = "EXT ";
            //mpfFileHeader[4] = "MPF ";
            //mpfFileHeader[5] = TransactionTime.ToString("yyyyMMdd");
            //mpfFileHeader[6] = TransactionTime.ToString("HHmmss");
            //mpfFileHeader[7] = HashCount.ToString("0000000");
            //mpfFileHeader[8] = TotalRelevantIncome.ToString("0000000000000.00").Replace(".", "");

            //mpfFileData = string.Join(FIELD_DELIMITER, mpfFileHeader) + RECORD_DELIMITER + mpfFileData;

            FileInfo result = GenerateTempFileName();
            StreamWriter writer = new StreamWriter(result.OpenWrite());
            writer.Write(mpfFileData);
            writer.Close();

            //SaveSequenceNo(m_SequenceNo + 1);
            return result;
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
        private void GenerateMPFDataByPeriod(GenericMPFFileDetail mpfFileDetail,ArrayList periodList)
        {
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

                EEmpPersonalInfo empInfo = new EEmpPersonalInfo();
                empInfo.EmpID=mpfFileDetail.EmpID;
                EEmpPersonalInfo.db.select(dbConn, empInfo);

                string[] mpfFileMCDetailStringList = new string[13];
                mpfFileMCDetailStringList[0] = "D";
                mpfFileMCDetailStringList[1] = string.Empty.PadLeft(10);
                mpfFileMCDetailStringList[2] = mpfFileDetail.HKIDPassport.PadRight(15).Substring(0, 15);
                mpfFileMCDetailStringList[3] = empInfo.EmpEngSurname.PadRight(20).Substring(0, 20);
                mpfFileMCDetailStringList[4] = empInfo.EmpEngOtherName.PadRight(20).Substring(0, 20);
                mpfFileMCDetailStringList[5] = contributionDetail.RelevantIncome.ToString("00000000000.00").Replace(".", "");
                mpfFileMCDetailStringList[6] = contributionDetail.MCEE.ToString("00000000000.00").Replace(".", "");
                mpfFileMCDetailStringList[7] = contributionDetail.MCER.ToString("00000000000.00").Replace(".", "");
                mpfFileMCDetailStringList[8] = contributionDetail.VCEE.ToString("00000000000.00").Replace(".", "");
                mpfFileMCDetailStringList[9] = contributionDetail.VCER.ToString("00000000000.00").Replace(".", "");
                mpfFileMCDetailStringList[10] = ((double)0).ToString("00000000000.00").Replace(".", "");   //  no surcharge support
                mpfFileMCDetailStringList[11] = " ";    //  Not In Use Since file specification 1.7a
                mpfFileMCDetailStringList[12] = string.Empty.PadLeft(8);    //  Not In Use Since file specification 1.7a

                currentMPFPeriod.TotalContributionAmount += contributionDetail.MCER + contributionDetail.MCEE;
                currentMPFPeriod.TotalContributionAmount += contributionDetail.VCER + contributionDetail.VCEE;
                currentMPFPeriod.DetailStringList.Add(string.Join(FIELD_DELIMITER, mpfFileMCDetailStringList));

            }
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
            return MPFPlanSchemeNo.PadLeft(8, '0').Substring(0, 8) + "MANULIFE" + SequenceNo.ToString("00") + MPFFileExtension();
        }

        public override string MPFFileExtension()
        {
            return ".con";
        }
    }
}