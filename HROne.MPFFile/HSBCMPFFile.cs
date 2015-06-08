using System;
using System.Collections;
using System.Data;
using System.Configuration;
using System.IO;
using HROne.DataAccess;

namespace HROne.MPFFile
{
    /// <summary>
    /// PGP (Agreed Format) for MPF submission Interface File 
    /// </summary>
    public class HSBCMPFFile : GenericMPFFile
    {
        public enum PaymentMethodEnum
        {
            DIRECT_CREDIT_TO_BANK_ACC =1,
            DIRECT_DEBIT,
            CHEQUE
        }

        private const string FIELD_DELIMITER = "";
        private const string RECORD_DELIMITER = "\r\n";

        public PaymentMethodEnum PaymentMethod;

        private int HashCount = 0;
        private int NewJoinEntitiesCount = 0;
        private int ExistingEntitiesCount = 0;
        private string FormNo;

        protected internal DateTime TransactionTime;

        protected string m_EmployerID;
        public string EmployerID
        {
            get { return m_EmployerID; }
        }

        private string m_DefaultClassName;
        public string DefaultClassName
        {
            get { return m_DefaultClassName; }
        }

        private string m_PayCenterCode;
        public string PayCenterCode
        {
            get { return m_PayCenterCode; }
        }

        public HSBCMPFFile(DatabaseConnection dbConn)
            : base(dbConn)
        {
            TransactionTime = AppUtils.ServerDateTime();
        }

        public void LoadFromFile(string path)
        {
            FileInfo fileInfo = new FileInfo(path);
            StreamReader bankFileStream = fileInfo.OpenText();
            try
            {

                string content = bankFileStream.ReadLine();
                if (content.Length < 62)
                    throw new Exception("Invalid Header");
                {
                    string[] mpfFileHeader = new string[9];
                    mpfFileHeader[0] = content.Substring(0, 7);     //  HeaderID:   HEADER
                    mpfFileHeader[1] = content.Substring(7, 23);    //  Description
                    mpfFileHeader[2] = content.Substring(30, 8);    //  Filename:   AMCND
                    mpfFileHeader[3] = content.Substring(38, 4);    //  Sending System
                    mpfFileHeader[4] = content.Substring(42, 4);    //  Receiving System
                    mpfFileHeader[5] = content.Substring(46, 8);    //  Date Header:    YYYYMMDD
                    mpfFileHeader[6] = content.Substring(54, 6);    //  Time Header:    HHmmss
                    //  others will not be loaded since different format has different data

                    if (!mpfFileHeader[0].Equals("HEADER "))
                        throw new Exception("Invalid Header on line 1");
                    if (!mpfFileHeader[2].Equals("AMCND   "))
                        throw new Exception("Invalid Filename on line 1");
                    if (!DateTime.TryParseExact(mpfFileHeader[5] + mpfFileHeader[6], "yyyyMMddHHmmss", null, System.Globalization.DateTimeStyles.None, out TransactionTime))
                        throw new Exception("Invalid Transaction Datetime on line 1");

                    content = bankFileStream.ReadLine();

                    string[] mpfFileHeader2 = new string[10];
                    mpfFileHeader2[0] = content.Substring(0, 4);    //  FormNo
                    mpfFileHeader2[1] = content.Substring(4, 46);   //  TransactionCommonInfo
                    mpfFileHeader2[2] = content.Substring(50, 2);   //  Record Code: 90
                    mpfFileHeader2[3] = content.Substring(52, 6);   //  Sequence No: 000001
                    mpfFileHeader2[4] = content.Substring(58, 8);   //  Contribution File From
                    mpfFileHeader2[5] = content.Substring(66, 8);   //  Contribution File To
                    mpfFileHeader2[6] = content.Substring(74, 4);   //  Pay Center Code
                    mpfFileHeader2[7] = content.Substring(78, 8);   //  Employer ID
                    mpfFileHeader2[8] = content.Substring(86, 1);   //  PaymentMethod

                    if (mpfFileHeader2[0].Equals("INB1", StringComparison.CurrentCultureIgnoreCase))
                        m_MPFSchemeCode = "MT00261";
                    else if (mpfFileHeader2[0].Equals("INB2", StringComparison.CurrentCultureIgnoreCase))
                        m_MPFSchemeCode = "MT00245";
                    else if (mpfFileHeader2[0].Equals("INB7", StringComparison.CurrentCultureIgnoreCase))
                        m_MPFSchemeCode = "MT00512";
                    else if (mpfFileHeader2[0].Equals("INBA", StringComparison.CurrentCultureIgnoreCase))
                        m_MPFSchemeCode = "MT00555";
                    else if (mpfFileHeader2[0].Equals("HAB1", StringComparison.CurrentCultureIgnoreCase))
                        m_MPFSchemeCode = "MT0027A";
                    else if (mpfFileHeader2[0].Equals("HAB2", StringComparison.CurrentCultureIgnoreCase))
                        m_MPFSchemeCode = "MT00253";
                    else if (mpfFileHeader2[0].Equals("HAB7", StringComparison.CurrentCultureIgnoreCase))
                        m_MPFSchemeCode = "MT00520";
                    else if (mpfFileHeader2[0].Equals("HABA", StringComparison.CurrentCultureIgnoreCase))
                        m_MPFSchemeCode = "MT00563";
                    else
                        throw new Exception("Invalid Form Type on line 2");

                    if (!mpfFileHeader2[2].Equals("90"))
                        throw new Exception("Invalid Record Code on line 2");

                    if (!mpfFileHeader2[3].Equals("000001"))
                        throw new Exception("Invalid Sequence No on line 2");

                    if (!DateTime.TryParseExact(mpfFileHeader2[4], "yyyyMMdd", null, System.Globalization.DateTimeStyles.None, out   m_PayPeriodFr))
                        throw new Exception("Invalid Contribution Period on line 2");

                    if (!DateTime.TryParseExact(mpfFileHeader2[5], "yyyyMMdd", null, System.Globalization.DateTimeStyles.None, out   m_PayPeriodTo))
                        throw new Exception("Invalid Contribution Period on line 2");

                    if (mpfFileHeader2[8] == "C")
                        PaymentMethod = PaymentMethodEnum.CHEQUE;
                    else if (mpfFileHeader2[8] == "B")
                        PaymentMethod = PaymentMethodEnum.DIRECT_CREDIT_TO_BANK_ACC;

                    else if (mpfFileHeader2[8] == "D")
                        PaymentMethod = PaymentMethodEnum.DIRECT_DEBIT;
                    else
                        throw new Exception("Invalid Payment Method on line 2");

                    m_MPFPlanExtendXMLString = HROne.CommonLib.Utility.SetXmlElementFromXmlString(m_MPFPlanExtendXMLString, "EmpMPFPlanExtendData", "MPFPlanPayCenter", mpfFileHeader2[6]);
                    m_MPFPlanExtendXMLString = HROne.CommonLib.Utility.SetXmlElementFromXmlString(m_MPFPlanExtendXMLString, "EmpMPFPlanExtendData", "MPFPlanEmployerID", mpfFileHeader2[7]);

                    int lineNo=2;
                    while (!bankFileStream.EndOfStream)
                    {
                        content = bankFileStream.ReadLine();
                        lineNo++;
                        if (content.Length < 90)
                            throw new Exception("Invalid length on line " + lineNo);

                        string[] mpfFileDetail = new string[5];
                        mpfFileDetail[0] = content.Substring(0, 4);    //  FormNo
                        mpfFileDetail[1] = content.Substring(4, 46);   //  TransactionCommonInfo
                        mpfFileDetail[2] = content.Substring(50, 2);   //  Record Code: 91, 92, 93, 94, 99
                        mpfFileDetail[3] = content.Substring(52, 6);   //  Sequence No: line no - 1
                        mpfFileDetail[4] = content.Substring(58);   //  Contribution File From

                        if (!(mpfFileHeader2[0].Equals(mpfFileDetail[0])))
                        {
                            throw new Exception("Invalid record header on line " + lineNo);
                        }

                        if (mpfFileDetail[2] == "99")
                            break;

                        if (mpfFileDetail[3] != ((int)(lineNo - 1)).ToString("000000"))
                            throw new Exception("Invalid sequence number on line " + lineNo);

                        string contributionDetailContent = mpfFileDetail[4];

                        HROne.Lib.Entities.EEmpPersonalInfo dummyEmpInfo = null;
                        //HROne.Lib.Entities.EEmpPersonalInfo.db.select(dbConn, dummyEmpInfo);
                        if (mpfFileDetail[2] == "91")
                        {
                            GenericExistingEmployeeMPFFileDetail mpfDataDetail = new GenericExistingEmployeeMPFFileDetail(this, dummyEmpInfo);
                            this.ExistingEmployeeMPFFileDetails.Add(mpfDataDetail);

                            mpfDataDetail.HKIDPassport = contributionDetailContent.Substring(0, 16);
                            mpfDataDetail.SchemeJoinDate = DateTime.ParseExact(contributionDetailContent.Substring(16, 8), "yyyyMMdd", null);

                            GenericMPFFileContributionDetail mpfContributionDetail = new GenericMPFFileContributionDetail();
                            mpfDataDetail.MPFContributionDetailList.Add(mpfContributionDetail);
                            mpfContributionDetail.PeriodTo = DateTime.ParseExact(contributionDetailContent.Substring(24, 8), "yyyyMMdd", null);
                            mpfContributionDetail.RelevantIncome = double.Parse(contributionDetailContent.Substring(32, 13) + "." + contributionDetailContent.Substring(45, 2));
                            mpfContributionDetail.MCER = double.Parse(contributionDetailContent.Substring(47, 7) + "." + contributionDetailContent.Substring(54, 2));
                            mpfContributionDetail.MCEE = double.Parse(contributionDetailContent.Substring(56, 7) + "." + contributionDetailContent.Substring(63, 2));
                            mpfContributionDetail.VCER = double.Parse(contributionDetailContent.Substring(65, 7) + "." + contributionDetailContent.Substring(72, 2));
                            mpfContributionDetail.VCEE = double.Parse(contributionDetailContent.Substring(74, 7) + "." + contributionDetailContent.Substring(81, 2));
                            mpfContributionDetail.PeriodFrom = DateTime.ParseExact(contributionDetailContent.Substring(83, 8), "yyyyMMdd", null);

                            if (!string.IsNullOrEmpty(contributionDetailContent.Substring(91, 8).Trim()))
                            {
                                mpfDataDetail.LastEmploymentDate = DateTime.ParseExact(contributionDetailContent.Substring(91, 8), "yyyyMMdd", null);
                                mpfDataDetail.TermCode = contributionDetailContent.Substring(99, 2);
                                mpfDataDetail.LastPaymentDate = DateTime.ParseExact(contributionDetailContent.Substring(101, 8), "yyyyMMdd", null);
                                if (contributionDetailContent.Substring(109, 2) == "L")
                                    mpfDataDetail.LspSpFlag = LSPSP_FLAG_ENUM.LSP;
                                else if (contributionDetailContent.Substring(109, 2) == "S")
                                    mpfDataDetail.LspSpFlag = LSPSP_FLAG_ENUM.SP;
                                else
                                    mpfDataDetail.LspSpFlag = LSPSP_FLAG_ENUM.NONE;
                                if (mpfDataDetail.LspSpFlag != LSPSP_FLAG_ENUM.NONE)
                                {
                                    mpfDataDetail.LSPSPAmount = double.Parse(contributionDetailContent.Substring(111, 13) + "." + contributionDetailContent.Substring(124, 2));
                                    mpfDataDetail.LSPSPAmountPaidByER = double.Parse(contributionDetailContent.Substring(126, 13) + "." + contributionDetailContent.Substring(139, 2));
                                }
                            }
                        }
                        else if (mpfFileDetail[2] == "92")
                        {
                            GenericNewJoinEmployeeMPFFileDetail mpfDataDetail = new GenericNewJoinEmployeeMPFFileDetail(this, dummyEmpInfo);
                            this.NewJoinEmployeeMPFFileDetails.Add(mpfDataDetail);

                            mpfDataDetail.HKIDPassport = contributionDetailContent.Substring(0, 16);
                            if (contributionDetailContent.Substring(16, 1) == "H")
                                mpfDataDetail.HKIDType = IDENTITY_TYPE_ENUM.HKID;
                            else if (contributionDetailContent.Substring(16, 1) == "P")
                                mpfDataDetail.HKIDType = IDENTITY_TYPE_ENUM.PASSPORT;
                            mpfDataDetail.SchemeJoinDate = DateTime.ParseExact(contributionDetailContent.Substring(17, 8), "yyyyMMdd", null);

                            mpfDataDetail.EmpMPFPlanExtendXMLString = HROne.CommonLib.Utility.SetXmlElementFromXmlString(mpfDataDetail.EmpMPFPlanExtendXMLString, "EmpMPFPlanExtend", "EmpMPFPlanClassName", contributionDetailContent.Substring(25, 8));
                            mpfDataDetail.EmpSurname = contributionDetailContent.Substring(32, 30);
                            mpfDataDetail.EmpOtherName = contributionDetailContent.Substring(62, 20);
                            contributionDetailContent.Substring(82, 4); //  Member Initial, not supported
                            mpfDataDetail.DateOfBirth = DateTime.ParseExact(contributionDetailContent.Substring(86, 8), "yyyyMMdd", null);
                            mpfDataDetail.Sex = contributionDetailContent.Substring(94, 1);

                            int contributionStartCharIndex = 95;
                            for (int periodCount = 0; periodCount < 12; periodCount++)
                            {
                                string periodToString = contributionDetailContent.Substring(contributionStartCharIndex + 8 * periodCount, 8);
                                if (string.IsNullOrEmpty(periodToString.Trim()))
                                    break;
                                string RelevantIncomeString = contributionDetailContent.Substring(contributionStartCharIndex + 8 * 12 + 15 * periodCount, 15);
                                string MCERString = contributionDetailContent.Substring(contributionStartCharIndex + (8 + 15) * 12 + 9 * periodCount, 9);
                                string MCEEString = contributionDetailContent.Substring(contributionStartCharIndex + (8 + 15 + 9) * 12 + 9 * periodCount, 9);
                                string VCERString = contributionDetailContent.Substring(contributionStartCharIndex + (8 + 15 + 9 + 9) * 12 + 9 * periodCount, 9);
                                string VCEEString = contributionDetailContent.Substring(contributionStartCharIndex + (8 + 15 + 9 + 9 + 9) * 12 + 9 * periodCount, 9);
                                string periodFromString = contributionDetailContent.Substring(contributionStartCharIndex + (8 + 15 + 9 + 9 + 9 + 9) * 12 + 8 * periodCount, 8);

                                GenericMPFFileContributionDetail mpfContributionDetail = new GenericMPFFileContributionDetail();
                                mpfDataDetail.MPFContributionDetailList.Add(mpfContributionDetail);
                                mpfContributionDetail.PeriodTo = DateTime.ParseExact(periodToString, "yyyyMMdd", null);
                                mpfContributionDetail.RelevantIncome = double.Parse(RelevantIncomeString.Substring(0, 13) + "." + RelevantIncomeString.Substring(0, 2));
                                mpfContributionDetail.MCER = double.Parse(MCERString.Substring(0, 7) + "." + MCERString.Substring(0, 2));
                                mpfContributionDetail.MCEE = double.Parse(MCEEString.Substring(0, 7) + "." + MCEEString.Substring(0, 2));
                                mpfContributionDetail.VCER = double.Parse(VCERString.Substring(0, 7) + "." + VCERString.Substring(0, 2));
                                mpfContributionDetail.VCEE = double.Parse(VCEEString.Substring(0, 7) + "." + VCEEString.Substring(0, 2));
                                mpfContributionDetail.PeriodFrom = DateTime.ParseExact(periodFromString, "yyyyMMdd", null);
                            }

                            contributionDetailContent.Substring(899, 6);  // saluation, not required
                            if (contributionDetailContent.Substring(905, 1) == "1")
                                mpfDataDetail.MemberType = MEMBER_TYPE_ENUM.NORMAL;
                            else if (contributionDetailContent.Substring(905, 1) == "2")
                                mpfDataDetail.MemberType = MEMBER_TYPE_ENUM.CASUAL;
                            else if (contributionDetailContent.Substring(905, 1) == "3")
                                mpfDataDetail.MemberType = MEMBER_TYPE_ENUM.EXEMPT;
                            else
                                mpfDataDetail.MemberType = MEMBER_TYPE_ENUM.NORMAL;
                            mpfDataDetail.DateJoined = DateTime.ParseExact(contributionDetailContent.Substring(906, 8), "yyyyMMdd", null);
                            mpfDataDetail.IsTransfer = contributionDetailContent.Substring(906, 8) == "1" ? true : false;

                        }
                        else if (mpfFileDetail[2] == "93")
                        {
                            GenericAdditionalEmployeeMPFFileDetail mpfDataDetail = new GenericAdditionalEmployeeMPFFileDetail(this, dummyEmpInfo);
                            this.AdditionalEmployeeMPFFileDetails.Add(mpfDataDetail);

                            mpfDataDetail.HKIDPassport = contributionDetailContent.Substring(0, 16);
                            mpfDataDetail.SchemeJoinDate = DateTime.ParseExact(contributionDetailContent.Substring(16, 8), "yyyyMMdd", null);

                            GenericMPFFileContributionDetail mpfContributionDetail = new GenericMPFFileContributionDetail();
                            mpfDataDetail.MPFContributionDetailList.Add(mpfContributionDetail);
                            mpfContributionDetail.PeriodTo = DateTime.ParseExact(contributionDetailContent.Substring(24, 8), "yyyyMMdd", null);
                            mpfContributionDetail.RelevantIncome = double.Parse(contributionDetailContent.Substring(32, 13) + "." + contributionDetailContent.Substring(45, 2));
                            mpfContributionDetail.MCER = double.Parse(contributionDetailContent.Substring(47, 7) + "." + contributionDetailContent.Substring(54, 2));
                            mpfContributionDetail.MCEE = double.Parse(contributionDetailContent.Substring(56, 7) + "." + contributionDetailContent.Substring(63, 2));
                            mpfContributionDetail.VCER = double.Parse(contributionDetailContent.Substring(65, 7) + "." + contributionDetailContent.Substring(72, 2));
                            mpfContributionDetail.VCEE = double.Parse(contributionDetailContent.Substring(74, 7) + "." + contributionDetailContent.Substring(81, 2));
                            mpfContributionDetail.PeriodFrom = DateTime.ParseExact(contributionDetailContent.Substring(83, 8), "yyyyMMdd", null);

                            if (!string.IsNullOrEmpty(contributionDetailContent.Substring(91, 8).Trim()))
                            {
                                mpfDataDetail.LastEmploymentDate = DateTime.ParseExact(contributionDetailContent.Substring(91, 8), "yyyyMMdd", null);
                                mpfDataDetail.TermCode = contributionDetailContent.Substring(99, 2);
                                mpfDataDetail.LastPaymentDate = DateTime.ParseExact(contributionDetailContent.Substring(101, 8), "yyyyMMdd", null);
                                if (contributionDetailContent.Substring(109, 2) == "L")
                                    mpfDataDetail.LspSpFlag = LSPSP_FLAG_ENUM.LSP;
                                else if (contributionDetailContent.Substring(109, 2) == "S")
                                    mpfDataDetail.LspSpFlag = LSPSP_FLAG_ENUM.SP;
                                else
                                    mpfDataDetail.LspSpFlag = LSPSP_FLAG_ENUM.NONE;
                                if (mpfDataDetail.LspSpFlag != LSPSP_FLAG_ENUM.NONE)
                                {
                                    mpfDataDetail.LSPSPAmount = double.Parse(contributionDetailContent.Substring(111, 13) + "." + contributionDetailContent.Substring(124, 2));
                                    mpfDataDetail.LSPSPAmountPaidByER = double.Parse(contributionDetailContent.Substring(126, 13) + "." + contributionDetailContent.Substring(139, 2));
                                }
                            }
                        }
                        else if (mpfFileDetail[2] == "94")
                        {
                            GenericAdditionalEmployeeMPFFileDetail mpfDataDetail = new GenericAdditionalEmployeeMPFFileDetail(this, dummyEmpInfo);
                            this.AdditionalEmployeeMPFFileDetails.Add(mpfDataDetail);

                            mpfDataDetail.HKIDPassport = contributionDetailContent.Substring(0, 16);
                            mpfDataDetail.SchemeJoinDate = DateTime.ParseExact(contributionDetailContent.Substring(16, 8), "yyyyMMdd", null);

                            GenericMPFFileContributionDetail mpfContributionDetail = new GenericMPFFileContributionDetail();
                            mpfDataDetail.MPFContributionDetailList.Add(mpfContributionDetail);
                            mpfContributionDetail.PeriodTo = DateTime.ParseExact(contributionDetailContent.Substring(24, 8), "yyyyMMdd", null);
                            mpfContributionDetail.RelevantIncome = double.Parse(contributionDetailContent.Substring(32, 13) + "." + contributionDetailContent.Substring(45, 2));
                            mpfContributionDetail.MCER = double.Parse(contributionDetailContent.Substring(47, 7) + "." + contributionDetailContent.Substring(54, 2));
                            mpfContributionDetail.MCEE = double.Parse(contributionDetailContent.Substring(56, 7) + "." + contributionDetailContent.Substring(63, 2));
                            mpfContributionDetail.VCER = double.Parse(contributionDetailContent.Substring(65, 7) + "." + contributionDetailContent.Substring(72, 2));
                            mpfContributionDetail.VCEE = double.Parse(contributionDetailContent.Substring(74, 7) + "." + contributionDetailContent.Substring(81, 2));
                            mpfContributionDetail.PeriodFrom = DateTime.ParseExact(contributionDetailContent.Substring(83, 8), "yyyyMMdd", null);

                            //if (string.IsNullOrEmpty(contributionDetailContent.Substring(91, 8).Trim()))
                            //{
                            //    mpfDataDetail.LastEmploymentDate = DateTime.ParseExact(contributionDetailContent.Substring(91, 8), "yyyyMMdd", null);
                            //    mpfDataDetail.TermCode = contributionDetailContent.Substring(99, 2);
                            //    mpfDataDetail.LastPaymentDate = DateTime.ParseExact(contributionDetailContent.Substring(101, 8), "yyyyMMdd", null);
                            //    if (contributionDetailContent.Substring(109, 2) == "L")
                            //        mpfDataDetail.LspSpFlag = LSPSP_FLAG_ENUM.LSP;
                            //    else if (contributionDetailContent.Substring(109, 2) == "S")
                            //        mpfDataDetail.LspSpFlag = LSPSP_FLAG_ENUM.SP;
                            //    else
                            //        mpfDataDetail.LspSpFlag = LSPSP_FLAG_ENUM.NONE;
                            //    if (mpfDataDetail.LspSpFlag != LSPSP_FLAG_ENUM.NONE)
                            //    {
                            //        mpfDataDetail.LSPSPAmount = double.Parse(contributionDetailContent.Substring(111, 13) + "." + contributionDetailContent.Substring(124, 2));
                            //        mpfDataDetail.LSPSPAmountPaidByER = double.Parse(contributionDetailContent.Substring(126, 13) + "." + contributionDetailContent.Substring(139, 2));
                            //    }
                            //}
                        }
                    }
                }
            }
            finally
            {
                bankFileStream.Close();
            }
        }

        public HSBCMPFGatewayFile ConvertToMPFGatewayFile(string RemoteProfileID)
        {
            HSBCMPFGatewayFile mpfGatewayFile = new HSBCMPFGatewayFile(dbConn, RemoteProfileID);
            mpfGatewayFile.EmployerID = EmployerID;
            mpfGatewayFile.MPFPlanExtendXMLString = MPFPlanExtendXMLString;
            mpfGatewayFile.MPFPlanParticipationNo = MPFPlanParticipationNo;
            mpfGatewayFile.MPFSchemeCode = MPFSchemeCode;
            mpfGatewayFile.MPFSchemeTrusteeCode = MPFSchemeTrusteeCode;
            mpfGatewayFile.PayCenterCode = PayCenterCode;
            mpfGatewayFile.PayPeriodFr = PayPeriodFr;
            mpfGatewayFile.PayPeriodTo = PayPeriodTo;
            mpfGatewayFile.TransactionTime = TransactionTime;
            foreach (GenericAdditionalEmployeeMPFFileDetail mpfDetail in AdditionalEmployeeMPFFileDetails)
                mpfGatewayFile.AdditionalEmployeeMPFFileDetails.Add(mpfDetail);

            foreach (GenericBackPaymentEmployeeMPFFileDetail mpfDetail in BackPaymentEmployeeMPFFileDetails)
                mpfGatewayFile.BackPaymentEmployeeMPFFileDetails.Add(mpfDetail);

            foreach (GenericExistingEmployeeMPFFileDetail mpfDetail in ExistingEmployeeMPFFileDetails)
                mpfGatewayFile.ExistingEmployeeMPFFileDetails.Add(mpfDetail);

            foreach (GenericNewJoinEmployeeMPFFileDetail mpfDetail in NewJoinEmployeeMPFFileDetails)
                mpfGatewayFile.NewJoinEmployeeMPFFileDetails.Add(mpfDetail);

            return mpfGatewayFile;
        }
        public override FileInfo GenerateMPFFile()
        {

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

            HashCount = 1;

            string[] mpfFileHeader2 = new string[10];
            mpfFileHeader2[0] = FormNo;
            mpfFileHeader2[1] = string.Empty.PadRight(46);
            mpfFileHeader2[2] = "90";
            mpfFileHeader2[3] = HashCount.ToString("000000");
            mpfFileHeader2[4] = PayPeriodFr.ToString("yyyyMMdd");
            mpfFileHeader2[5] = PayPeriodTo.ToString("yyyyMMdd");
            mpfFileHeader2[6] = m_PayCenterCode;

            mpfFileHeader2[7] = m_EmployerID;
            
            if (PaymentMethod == PaymentMethodEnum.CHEQUE)
                mpfFileHeader2[8] = "C";
            else if (PaymentMethod == PaymentMethodEnum.DIRECT_CREDIT_TO_BANK_ACC)
                mpfFileHeader2[8] = "B";
            else if (PaymentMethod == PaymentMethodEnum.DIRECT_DEBIT)
                mpfFileHeader2[8] = "D";
            else
                mpfFileHeader2[8] = " ";
            mpfFileHeader2[9] = string.Empty.PadLeft(8);
            AddTransactionCommonInfoString(mpfFileHeader2);

            string mpfFileData = string.Join(FIELD_DELIMITER, mpfFileHeader2) + RECORD_DELIMITER;

            foreach (GenericExistingEmployeeMPFFileDetail bankFileDetail in ExistingEmployeeMPFFileDetails)
            {
                mpfFileData += GenerateExistingMPFFileDetail(bankFileDetail) + RECORD_DELIMITER;
            }
            foreach (GenericNewJoinEmployeeMPFFileDetail mpfFileDetail in NewJoinEmployeeMPFFileDetails)
            {
                mpfFileData += GenerateNewJoinMPFFileDetail(mpfFileDetail) + RECORD_DELIMITER;
            }
            foreach (GenericAdditionalEmployeeMPFFileDetail bankFileDetail in AdditionalEmployeeMPFFileDetails)
            {
                mpfFileData += GenerateAdditionalMPFFileDetail(bankFileDetail) + RECORD_DELIMITER;
            }
            foreach (GenericBackPaymentEmployeeMPFFileDetail bankFileDetail in BackPaymentEmployeeMPFFileDetails)
            {
                mpfFileData += GenerateBackPaymentMPFFileDetail(bankFileDetail) + RECORD_DELIMITER;
            }

            string[] mpfFileFooter = new string[19];
            mpfFileFooter[0] = FormNo;
            mpfFileFooter[1] = string.Empty.PadRight(46);
            mpfFileFooter[2] = "99";
            mpfFileFooter[3] = "999999";
            mpfFileFooter[4] = ((double)(this.TotalAdditionalEmployeeMC + this.TotalAdditionalEmployeeVC + this.TotalBackPaymentEmployeeMC + this.TotalBackPaymentEmployeeVC + this.TotalExistingEmployeeMC + this.TotalExistingEmployeeVC + this.TotalNewJoinEmployeeMC + this.TotalNewJoinEmployeeVC)).ToString("0000000000000000.00").Replace(".", "");
            mpfFileFooter[5] = "        "; // leave blank for internal use
            if (PaymentMethod == PaymentMethodEnum.CHEQUE)
                mpfFileFooter[6] = "C";
            else if (PaymentMethod == PaymentMethodEnum.DIRECT_CREDIT_TO_BANK_ACC)
                mpfFileFooter[6] = "B";
            else if (PaymentMethod == PaymentMethodEnum.DIRECT_DEBIT)
                mpfFileFooter[6] = "D";
            else
                mpfFileFooter[6] = " ";
            mpfFileFooter[7] = "      ";
            mpfFileFooter[8] = "        ";
            mpfFileFooter[9] = ExistingEntitiesCount.ToString("0000000");
            mpfFileFooter[10] = NewJoinEntitiesCount.ToString("0000000");
            mpfFileFooter[11] = this.TotalExistingEmployeeMC.ToString("0000000000000000.00").Replace(".", "");
            mpfFileFooter[12] = this.TotalNewJoinEmployeeMC.ToString("0000000000000000.00").Replace(".", "");
            mpfFileFooter[13] = this.TotalExistingEmployeeVC.ToString("0000000000000000.00").Replace(".", "");
            mpfFileFooter[14] = this.TotalNewJoinEmployeeVC.ToString("0000000000000000.00").Replace(".", "");
            mpfFileFooter[15] = this.TotalAdditionalEmployeeMC.ToString("0000000000000000.00").Replace(".", "");
            mpfFileFooter[16] = this.TotalAdditionalEmployeeVC.ToString("0000000000000000.00").Replace(".", "");
            mpfFileFooter[17] = this.TotalBackPaymentEmployeeMC.ToString("0000000000000000.00").Replace(".", "");
            mpfFileFooter[18] = this.TotalBackPaymentEmployeeVC.ToString("0000000000000000.00").Replace(".", "");

            AddTransactionCommonInfoString(mpfFileFooter);

            mpfFileData += string.Join(FIELD_DELIMITER, mpfFileFooter);

            //  HeadCount include footer record
            HashCount++;

            string[] mpfFileHeader = new string[9];
            mpfFileHeader[0] = "HEADER ";
            mpfFileHeader[1] = "MPF CONTRIBUTION DTL   ";
            mpfFileHeader[2] = "AMCND   ";
            mpfFileHeader[3] = "EXT ";
            mpfFileHeader[4] = "MPF ";
            mpfFileHeader[5] = TransactionTime.ToString("yyyyMMdd");
            mpfFileHeader[6] = TransactionTime.ToString("HHmmss");
            mpfFileHeader[7] = HashCount.ToString("0000000");
            mpfFileHeader[8] = TotalRelevantIncome.ToString("0000000000000.00").Replace(".", "");

            mpfFileData = string.Join(FIELD_DELIMITER, mpfFileHeader) + RECORD_DELIMITER + mpfFileData;

            FileInfo result = GenerateTempFileName();
            StreamWriter writer = new StreamWriter(result.OpenWrite());
            writer.Write(mpfFileData);
            writer.Close();
            return result;

        }

        private void AddTransactionCommonInfoString(string[] TransactionFieldArray)
        {
            int numOfCharacters=0;
            foreach(string s in TransactionFieldArray)
            {
                numOfCharacters+=s.Length;
            }

            string[] CommonInfo= new string[5];
            
            CommonInfo[0] = m_EmployerID;
            CommonInfo[1] = numOfCharacters.ToString("0000");
            CommonInfo[2] = TransactionTime.ToString("yyyyMMdd");
            CommonInfo[3] = TransactionTime.ToString("HHmmss");
            CommonInfo[4] = string.Empty.PadRight(20);

            TransactionFieldArray[1] = string.Join(string.Empty, CommonInfo);
        }

        protected override string GenerateExistingMPFFileDetail(GenericExistingEmployeeMPFFileDetail mpfDataDetail)
        {
            string result = string.Empty;
            foreach (GenericMPFFileContributionDetail mpfContributionDetail in mpfDataDetail.MPFContributionDetailList)
            {
                HashCount++;
                ExistingEntitiesCount++;
                string[] mpfFileDetail = new string[19];
                mpfFileDetail[0] = FormNo;
                mpfFileDetail[1] = string.Empty.PadRight(46);
                mpfFileDetail[2] = "91";
                mpfFileDetail[3] = HashCount.ToString("000000");
                mpfFileDetail[4] = mpfDataDetail.HKIDPassport.PadRight(16).Substring(0, 16);
                mpfFileDetail[5] = mpfDataDetail.SchemeJoinDate.ToString("yyyyMMdd");
                mpfFileDetail[6] += mpfContributionDetail.PeriodTo.ToString("yyyyMMdd");
                mpfFileDetail[7] += mpfContributionDetail.RelevantIncome.ToString("0000000000000.00").Replace(".", "");
                mpfFileDetail[8] += mpfContributionDetail.MCER.ToString("0000000.00").Replace(".", "");
                mpfFileDetail[9] += mpfContributionDetail.MCEE.ToString("0000000.00").Replace(".", "");
                mpfFileDetail[10] += mpfContributionDetail.VCER.ToString("0000000.00").Replace(".", "");
                mpfFileDetail[11] += mpfContributionDetail.VCEE.ToString("0000000.00").Replace(".", "");
                mpfFileDetail[12] += mpfContributionDetail.PeriodFrom.ToString("yyyyMMdd");

                if (!mpfDataDetail.LastEmploymentDate.Ticks.Equals(0))
                {
                    mpfFileDetail[13] = mpfDataDetail.LastEmploymentDate.ToString("yyyyMMdd");
                    mpfFileDetail[14] = mpfDataDetail.TermCode.PadRight(2).Substring(0, 2);
                    mpfFileDetail[15] = mpfDataDetail.LastPaymentDate.ToString("yyyyMMdd");
                    if (mpfDataDetail.LspSpFlag == LSPSP_FLAG_ENUM.LSP || mpfDataDetail.LspSpFlag == LSPSP_FLAG_ENUM.SP)
                    {
                        if (mpfDataDetail.LspSpFlag == LSPSP_FLAG_ENUM.LSP)
                            mpfFileDetail[16] = "L";
                        else if (mpfDataDetail.LspSpFlag == LSPSP_FLAG_ENUM.SP)
                            mpfFileDetail[16] = "S";
                        mpfFileDetail[17] = mpfDataDetail.LSPSPAmount.ToString("0000000000000.00").Replace(".", "");
                        mpfFileDetail[18] = mpfDataDetail.LSPSPAmountPaidByER.ToString("0000000000000.00").Replace(".", "");
                    }
                    else
                    {
                        mpfFileDetail[16] = string.Empty.PadRight(1);
                        mpfFileDetail[17] = string.Empty.PadRight(15, '0');
                        mpfFileDetail[18] = string.Empty.PadRight(15, '0');
                    }
                }
                else
                {
                    mpfFileDetail[13] = string.Empty.PadRight(8);
                    mpfFileDetail[14] = string.Empty.PadRight(2);
                    mpfFileDetail[15] = string.Empty.PadRight(8);
                    mpfFileDetail[16] = string.Empty.PadRight(1);
                    mpfFileDetail[17] = string.Empty.PadRight(15, '0');
                    mpfFileDetail[18] = string.Empty.PadRight(15, '0');
                }

                AddTransactionCommonInfoString(mpfFileDetail);

                if (result.Equals(string.Empty))
                    result = string.Join(FIELD_DELIMITER, mpfFileDetail);
                else
                    result += RECORD_DELIMITER + string.Join(FIELD_DELIMITER, mpfFileDetail);
            }
            return result;
        }
 
        protected override string GenerateNewJoinMPFFileDetail(GenericNewJoinEmployeeMPFFileDetail mpfDataDetail)
        {
            HashCount++;
            NewJoinEntitiesCount++;
            string[] mpfFileDetail = new string[24];
            mpfFileDetail[0] = FormNo;
            mpfFileDetail[1] = string.Empty.PadRight(46);
            mpfFileDetail[2] = "92";
            mpfFileDetail[3] = HashCount.ToString("000000");
            mpfFileDetail[4] = mpfDataDetail.HKIDPassport.PadRight(16).Substring(0, 16);
            if (mpfDataDetail.HKIDType == IDENTITY_TYPE_ENUM.HKID)
                mpfFileDetail[5] = "H";
            else if (mpfDataDetail.HKIDType == IDENTITY_TYPE_ENUM.PASSPORT)
                mpfFileDetail[5] = "P";
            else
                mpfFileDetail[5] = " ";
            mpfFileDetail[6] = mpfDataDetail.SchemeJoinDate.ToString("yyyyMMdd");
            System.Xml.XmlNodeList classNameNode = HROne.CommonLib.Utility.GetXmlDocumentByDataString(mpfDataDetail.EmpMPFPlanExtendXMLString).GetElementsByTagName("EmpMPFPlanClassName");
            if (classNameNode.Count > 0)
            {
                mpfFileDetail[7] = classNameNode[0].InnerText.PadRight(8).Substring(0, 8).ToUpper();
                if (string.IsNullOrEmpty(mpfFileDetail[7] .Trim()))
                    mpfFileDetail[7] = DefaultClassName.PadRight(8).Substring(0, 8).ToUpper();
            }
            else
                mpfFileDetail[7] = DefaultClassName.PadRight(8).Substring(0, 8).ToUpper();

            mpfFileDetail[8] = mpfDataDetail.EmpSurname.Replace(",", " ").Replace("  ", " ").PadRight(30).Substring(0, 30).ToUpper();
            mpfFileDetail[9] = mpfDataDetail.EmpOtherName.Replace(",", " ").Replace("  ", " ").PadRight(20).Substring(0, 20).ToUpper();
            mpfFileDetail[10] = "   ";  // Member Initial, N/A
            mpfFileDetail[11] = mpfDataDetail.DateOfBirth.ToString("yyyyMMdd");
            mpfFileDetail[12] = mpfDataDetail.Sex.PadRight(1).Substring(0, 1).ToUpper();

            mpfFileDetail[13] = string.Empty;
            mpfFileDetail[14] = string.Empty;
            mpfFileDetail[15] = string.Empty;
            mpfFileDetail[16] = string.Empty;
            mpfFileDetail[17] = string.Empty;
            mpfFileDetail[18] = string.Empty;
            mpfFileDetail[19] = string.Empty;

            int periodCount = 0;
            foreach (GenericMPFFileContributionDetail mpfContributionDetail in mpfDataDetail.MPFContributionDetailList)
            {
                periodCount++;

                mpfFileDetail[13] += mpfContributionDetail.PeriodTo.ToString("yyyyMMdd");
                mpfFileDetail[14] += mpfContributionDetail.RelevantIncome.ToString("0000000000000.00").Replace(".", "");
                mpfFileDetail[15] += mpfContributionDetail.MCER.ToString("0000000.00").Replace(".", "");
                mpfFileDetail[16] += mpfContributionDetail.MCEE.ToString("0000000.00").Replace(".", "");
                mpfFileDetail[17] += mpfContributionDetail.VCER.ToString("0000000.00").Replace(".", "");
                mpfFileDetail[18] += mpfContributionDetail.VCEE.ToString("0000000.00").Replace(".", "");
                mpfFileDetail[19] += mpfContributionDetail.PeriodFrom.ToString("yyyyMMdd");
            }

            for (int i = periodCount; i < 12; i++)
            {
                mpfFileDetail[13] += string.Empty.PadLeft(8);
                mpfFileDetail[14] += string.Empty.PadLeft(15, '0');
                mpfFileDetail[15] += string.Empty.PadLeft(9, '0');
                mpfFileDetail[16] += string.Empty.PadLeft(9, '0');
                mpfFileDetail[17] += string.Empty.PadLeft(9, '0');
                mpfFileDetail[18] += string.Empty.PadLeft(9, '0');
                mpfFileDetail[19] += string.Empty.PadLeft(8);
            }
            mpfFileDetail[20] = "      ";   // saluation, not required
            if (mpfDataDetail.MemberType == MEMBER_TYPE_ENUM.NORMAL)
                mpfFileDetail[21] = "1";
            else if (mpfDataDetail.MemberType == MEMBER_TYPE_ENUM.CASUAL)
                mpfFileDetail[21] = "2";
            else if (mpfDataDetail.MemberType == MEMBER_TYPE_ENUM.EXEMPT)
                mpfFileDetail[21] = "3";
            else
                mpfFileDetail[21] = " ";
            mpfFileDetail[22] = mpfDataDetail.DateJoined.ToString("yyyyMMdd");
            mpfFileDetail[23] = mpfDataDetail.IsTransfer ? "1" : " ";

            AddTransactionCommonInfoString(mpfFileDetail);

            return string.Join(FIELD_DELIMITER, mpfFileDetail);
        }

        protected override string GenerateAdditionalMPFFileDetail(GenericAdditionalEmployeeMPFFileDetail mpfDataDetail)
        {
            string result = string.Empty;
            foreach (GenericMPFFileContributionDetail mpfContributionDetail in mpfDataDetail.MPFContributionDetailList)
            {
                HashCount++;
                string[] mpfFileDetail = new string[19];
                mpfFileDetail[0] = FormNo;
                mpfFileDetail[1] = string.Empty.PadRight(46);
                mpfFileDetail[2] = "93";
                mpfFileDetail[3] = HashCount.ToString("000000");
                mpfFileDetail[4] = mpfDataDetail.HKIDPassport.PadRight(16).Substring(0, 16);
                mpfFileDetail[5] = mpfDataDetail.SchemeJoinDate.ToString("yyyyMMdd");
                mpfFileDetail[6] += mpfContributionDetail.PeriodTo.ToString("yyyyMMdd");
                mpfFileDetail[7] += mpfContributionDetail.RelevantIncome.ToString("0000000000000.00").Replace(".", "");
                mpfFileDetail[8] += mpfContributionDetail.MCER.ToString("0000000.00").Replace(".", "");
                mpfFileDetail[9] += mpfContributionDetail.MCEE.ToString("0000000.00").Replace(".", "");
                mpfFileDetail[10] += mpfContributionDetail.VCER.ToString("0000000.00").Replace(".", "");
                mpfFileDetail[11] += mpfContributionDetail.VCEE.ToString("0000000.00").Replace(".", "");
                mpfFileDetail[12] += mpfContributionDetail.PeriodFrom.ToString("yyyyMMdd");

                if (!mpfDataDetail.LastEmploymentDate.Ticks.Equals(0))
                {
                    mpfFileDetail[13] = mpfDataDetail.LastEmploymentDate.ToString("yyyyMMdd");
                    mpfFileDetail[14] = mpfDataDetail.TermCode.PadRight(2).Substring(0, 2);
                    mpfFileDetail[15] = mpfDataDetail.LastPaymentDate.ToString("yyyyMMdd");

                    if (mpfDataDetail.LspSpFlag == LSPSP_FLAG_ENUM.LSP || mpfDataDetail.LspSpFlag == LSPSP_FLAG_ENUM.SP)
                    {
                        if (mpfDataDetail.LspSpFlag == LSPSP_FLAG_ENUM.LSP)
                            mpfFileDetail[16] = "L";
                        else if (mpfDataDetail.LspSpFlag == LSPSP_FLAG_ENUM.SP)
                            mpfFileDetail[16] = "S";
                        mpfFileDetail[17] = mpfDataDetail.LSPSPAmount.ToString("0000000000000.00").Replace(".", "");
                        mpfFileDetail[18] = mpfDataDetail.LSPSPAmountPaidByER.ToString("0000000000000.00").Replace(".", "");
                    }
                    else
                    {
                        mpfFileDetail[16] = string.Empty.PadRight(1);
                        mpfFileDetail[17] = string.Empty.PadRight(15, '0');
                        mpfFileDetail[18] = string.Empty.PadRight(15, '0');
                    }
                }
                else
                {
                    mpfFileDetail[13] = string.Empty.PadRight(8);
                    mpfFileDetail[14] = string.Empty.PadRight(2);
                    mpfFileDetail[15] = string.Empty.PadRight(8);
                    mpfFileDetail[16] = string.Empty.PadRight(1);
                    mpfFileDetail[17] = string.Empty.PadRight(15, '0');
                    mpfFileDetail[18] = string.Empty.PadRight(15, '0');
                }

                AddTransactionCommonInfoString(mpfFileDetail);

                if (result.Equals(string.Empty))
                    result = string.Join(FIELD_DELIMITER, mpfFileDetail);
                else
                    result += RECORD_DELIMITER + string.Join(FIELD_DELIMITER, mpfFileDetail);
            }
            return result;
        }

        protected override string GenerateBackPaymentMPFFileDetail(GenericBackPaymentEmployeeMPFFileDetail mpfDataDetail)
        {
            string result = string.Empty;
            foreach (GenericMPFFileContributionDetail mpfContributionDetail in mpfDataDetail.MPFContributionDetailList)
            {
                HashCount++;
                string[] mpfFileDetail = new string[19];
                mpfFileDetail[0] = FormNo;
                mpfFileDetail[1] = string.Empty.PadRight(46);
                mpfFileDetail[2] = "94";
                mpfFileDetail[3] = HashCount.ToString("000000");
                mpfFileDetail[4] = mpfDataDetail.HKIDPassport.PadRight(16).Substring(0, 16);
                mpfFileDetail[5] = mpfDataDetail.SchemeJoinDate.ToString("yyyyMMdd");
                mpfFileDetail[6] += this.PayPeriodTo.ToString("yyyyMMdd"); //mpfContributionDetail.PeriodTo.ToString("yyyyMMdd");
                mpfFileDetail[7] += mpfContributionDetail.RelevantIncome.ToString("0000000000000.00").Replace(".", "");
                mpfFileDetail[8] += mpfContributionDetail.MCER.ToString("0000000.00").Replace(".", "");
                mpfFileDetail[9] += mpfContributionDetail.MCEE.ToString("0000000.00").Replace(".", "");
                mpfFileDetail[10] += mpfContributionDetail.VCER.ToString("0000000.00").Replace(".", "");
                mpfFileDetail[11] += mpfContributionDetail.VCEE.ToString("0000000.00").Replace(".", "");
                mpfFileDetail[12] += this.PayPeriodFr.ToString("yyyyMMdd"); //mpfContributionDetail.PeriodFrom.ToString("yyyyMMdd");

                //if (!mpfDataDetail.LastEmploymentDate.Ticks.Equals(0))
                {
                    mpfFileDetail[13] = "        ";// mpfDataDetail.LastEmploymentDate.ToString("yyyyMMdd");
                    mpfFileDetail[14] = "  "; mpfDataDetail.TermCode.PadRight(2).Substring(0, 2);
                    mpfFileDetail[15] = "        "; //mpfDataDetail.LastPaymentDate.ToString("yyyyMMdd");
                    //if (mpfDataDetail.LspSpFlag == LSPSP_FLAG_ENUM.LSP || mpfDataDetail.LspSpFlag == LSPSP_FLAG_ENUM.SP)
                    //{
                    //    if (mpfDataDetail.LspSpFlag == LSPSP_FLAG_ENUM.LSP)
                    //        mpfFileDetail[16] = "L";
                    //    else if (mpfDataDetail.LspSpFlag == LSPSP_FLAG_ENUM.SP)
                    //        mpfFileDetail[16] = "S";
                    //    mpfFileDetail[17] = mpfDataDetail.LSPSPAmount.ToString("0000000000000.00").Replace(".", "");
                    //    mpfFileDetail[18] = mpfDataDetail.LSPSPAmountPaidByER.ToString("0000000000000.00").Replace(".", "");
                    //}
                    //else
                    {
                        mpfFileDetail[16] = string.Empty.PadRight(1);
                        mpfFileDetail[17] = string.Empty.PadRight(15);
                        mpfFileDetail[18] = string.Empty.PadRight(15);
                    }
                }
                //else
                //{
                //    mpfFileDetail[13] = string.Empty.PadRight(8);
                //    mpfFileDetail[14] = string.Empty.PadRight(2);
                //    mpfFileDetail[15] = string.Empty.PadRight(8);
                //    mpfFileDetail[16] = string.Empty.PadRight(1);
                //    mpfFileDetail[17] = string.Empty.PadRight(15, '0');
                //    mpfFileDetail[18] = string.Empty.PadRight(15, '0');
                //}

                AddTransactionCommonInfoString(mpfFileDetail);

                if (result.Equals(string.Empty))
                    result = string.Join(FIELD_DELIMITER, mpfFileDetail);
                else
                    result += RECORD_DELIMITER + string.Join(FIELD_DELIMITER, mpfFileDetail);
            }
            return result;
        }
        public bool IsValidFormat(string path, out string TrusteeCode)
        {
            FileInfo fileInfo = new FileInfo(path);
            StreamReader bankFileStream = fileInfo.OpenText();
            try
            {
                char[] charRead = new char[62];
                TrusteeCode = string.Empty;

                if (bankFileStream.Read(charRead, 0, charRead.Length) > 0)
                {
                    string header = new string(charRead);
                    if (header.Substring(0, 7).Equals("HEADER "))
                        if (header.Substring(30, 8).Equals("AMCND   "))
                        {
                            string reminder = bankFileStream.ReadLine();
                            string nextLine = bankFileStream.ReadLine();
                            string FormNo = nextLine.Substring(0, 4);
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
        public override string MPFFileExtension()
        {
            return ".txt";
        }
    }
}