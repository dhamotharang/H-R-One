using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using HROne.Lib.Entities;
using HROne.DataAccess;
using System.IO;
using HROne.CommonLib;

namespace HROne.MPFFile
{

    public interface MPFFileControlInterface
    {
        GenericMPFFile CreateMPFFileObject();
    }

    public enum LSPSP_FLAG_ENUM
    {
        NONE = 0,
        LSP = 1,
        SP = 2
    }

    public enum IDENTITY_TYPE_ENUM
    {
        HKID = 1,
        PASSPORT = 2
    }

    public enum MEMBER_TYPE_ENUM
    {
        NORMAL=1,
        CASUAL=2,
        EXEMPT=3
    }


    public class GenericMPFFile
    {
        private const string FIELD_DELIMITER = ",";
        private const string RECORD_DELIMITER = "\r\n";
        public List<GenericNewJoinEmployeeMPFFileDetail> NewJoinEmployeeMPFFileDetails = new List<GenericNewJoinEmployeeMPFFileDetail>();
        public List<GenericExistingEmployeeMPFFileDetail> ExistingEmployeeMPFFileDetails = new List<GenericExistingEmployeeMPFFileDetail>();
        public List<GenericAdditionalEmployeeMPFFileDetail> AdditionalEmployeeMPFFileDetails = new List<GenericAdditionalEmployeeMPFFileDetail>();
        public List<GenericBackPaymentEmployeeMPFFileDetail> BackPaymentEmployeeMPFFileDetails = new List<GenericBackPaymentEmployeeMPFFileDetail>();

        protected double m_TotalRelevantIncome = 0;
        public double TotalRelevantIncome
        {
            get { return m_TotalRelevantIncome; }
        }

        protected double m_TotalVCRelevantIncome = 0;
        public double TotalVCRelevantIncome
        {
            get { return m_TotalVCRelevantIncome; }
        }

        protected double m_TotalNewJoinMCEE = 0;
        protected double m_TotalNewJoinMCER = 0;
        protected double m_TotalNewJoinVCEE = 0;
        protected double m_TotalNewJoinVCER = 0;

        protected double m_TotalExistingMCEE = 0;
        protected double m_TotalExistingMCER = 0;
        protected double m_TotalExistingVCEE = 0;
        protected double m_TotalExistingVCER = 0;

        protected double m_TotalAdditionalMCEE = 0;
        protected double m_TotalAdditionalMCER = 0;
        protected double m_TotalAdditionalVCEE = 0;
        protected double m_TotalAdditionalVCER = 0;

        protected double m_TotalBackPaymentMCEE = 0;
        protected double m_TotalBackPaymentMCER = 0;
        protected double m_TotalBackPaymentVCEE = 0;
        protected double m_TotalBackPaymentVCER = 0;

        public double TotalNewJoinEmployeeMC
        {
            get { return m_TotalNewJoinMCEE + m_TotalNewJoinMCER; }
        }

        public double TotalNewJoinEmployeeVC
        {
            get { return m_TotalNewJoinVCEE + m_TotalNewJoinVCER; }
        }

        public double TotalExistingEmployeeMC
        {
            get { return m_TotalExistingMCEE + m_TotalExistingMCER; }
        }

        public double TotalExistingEmployeeVC
        {
            get { return m_TotalExistingVCEE + m_TotalExistingVCER; }
        }

        public double TotalAdditionalEmployeeMC
        {
            get { return m_TotalAdditionalMCEE + m_TotalAdditionalMCER; }
        }

        public double TotalAdditionalEmployeeVC
        {
            get { return m_TotalAdditionalVCEE + m_TotalAdditionalVCER; }
        }

        public double TotalBackPaymentEmployeeMC
        {
            get { return m_TotalBackPaymentMCEE + m_TotalBackPaymentMCER; }
        }

        public double TotalBackPaymentEmployeeVC
        {
            get { return m_TotalBackPaymentVCEE + m_TotalBackPaymentVCER; }
        }

        public double TotalMCEE
        {
            get { return m_TotalNewJoinMCEE + m_TotalExistingMCEE + m_TotalAdditionalMCEE + m_TotalBackPaymentMCEE; }
        }

        public double TotalVCEE
        {
            get { return m_TotalNewJoinVCEE + m_TotalExistingVCEE + m_TotalAdditionalVCEE + m_TotalBackPaymentVCEE; }
        }

        public double TotalMCER
        {
            get { return m_TotalNewJoinMCER + m_TotalExistingMCER + m_TotalAdditionalMCER + m_TotalBackPaymentMCER; }
        }

        public double TotalVCER
        {
            get { return m_TotalNewJoinVCER + m_TotalExistingVCER + m_TotalAdditionalVCER + m_TotalBackPaymentVCER; }
        }

        protected DateTime m_PayPeriodFr;
        public DateTime PayPeriodFr
        {
            get { return m_PayPeriodFr; }
            set { m_PayPeriodFr = value; }
        }

        protected DateTime m_PayPeriodTo;
        public DateTime PayPeriodTo
        {
            get { return m_PayPeriodTo; }
            set { m_PayPeriodTo = value; }
        }

        protected string m_MPFSchemeCode;
        public string MPFSchemeCode
        {
            get { return m_MPFSchemeCode; }
            set { m_MPFSchemeCode = value; }
        }

        protected string m_MPFSchemeTrusteeCode;
        public string MPFSchemeTrusteeCode
        {
            get { return m_MPFSchemeTrusteeCode; }
            set { m_MPFSchemeTrusteeCode = value; }
        }

        protected int m_MPFSchemeID;
        public int MPFSchemeID
        {
            get { return m_MPFSchemeID; }
        }
        protected string m_MPFPlanParticipationNo;
        public string MPFPlanParticipationNo
        {
            get { return m_MPFPlanParticipationNo; }
            set { m_MPFPlanParticipationNo = value; }
        }

        protected string m_MPFPlanExtendXMLString;
        public string MPFPlanExtendXMLString
        {
            get { return m_MPFPlanExtendXMLString; }
            set { m_MPFPlanExtendXMLString = value; }
        }

        protected int m_MPFPlanID;
        public int MPFPlanID
        {
            get { return m_MPFPlanID; }
        }

        protected internal DatabaseConnection dbConn;

        public GenericMPFFile(DatabaseConnection dbConn)
        {
            this.dbConn = dbConn.createClone();
        }

        public void LoadMPFFileDetail(ArrayList EmpList, int MPFPlanID, DateTime PayPeriodFr, DateTime PayPeriodTo)
        {
            this.m_PayPeriodFr = PayPeriodFr;
            this.m_PayPeriodTo = PayPeriodTo;
            this.m_MPFPlanID = MPFPlanID;

            NewJoinEmployeeMPFFileDetails.Clear();
            ExistingEmployeeMPFFileDetails.Clear();
            AdditionalEmployeeMPFFileDetails.Clear();
            BackPaymentEmployeeMPFFileDetails.Clear();

            m_TotalNewJoinMCEE = 0;
            m_TotalNewJoinMCER = 0;
            m_TotalNewJoinVCEE = 0;
            m_TotalNewJoinVCER = 0;

            m_TotalExistingMCEE = 0;
            m_TotalExistingMCER = 0;
            m_TotalExistingVCEE = 0;
            m_TotalExistingVCER = 0;

            m_TotalAdditionalMCEE = 0;
            m_TotalAdditionalMCER = 0;
            m_TotalAdditionalVCEE = 0;
            m_TotalAdditionalVCER = 0;

            m_TotalBackPaymentMCEE = 0;
            m_TotalBackPaymentMCER = 0;
            m_TotalBackPaymentVCEE = 0;
            m_TotalBackPaymentVCER = 0;

            EMPFPlan mpfPlanObject = new EMPFPlan();
            mpfPlanObject.MPFPlanID = MPFPlanID;
            if (EMPFPlan.db.select(dbConn, mpfPlanObject))
            {
                m_MPFPlanParticipationNo = mpfPlanObject.MPFPlanParticipationNo;
                //mpfPlanRow.MPFSchemeID = mpfPlanObject.MPFSchemeID;
                if (!string.IsNullOrEmpty(mpfPlanObject.MPFPlanExtendData))
                {
                    m_MPFPlanExtendXMLString = mpfPlanObject.MPFPlanExtendData;
                }
                m_MPFSchemeID = mpfPlanObject.MPFSchemeID;
                EMPFScheme mpfSchemeObject = new EMPFScheme();
                mpfSchemeObject.MPFSchemeID = m_MPFSchemeID;
                if (EMPFScheme.db.select(dbConn, mpfSchemeObject))
                {
                    m_MPFSchemeCode = mpfSchemeObject.MPFSchemeCode;
                    m_MPFSchemeTrusteeCode = mpfSchemeObject.MPFSchemeTrusteeCode;
                }

                DBFilter payPeriodFilter = new DBFilter();
                payPeriodFilter.add(new Match("pp.PayPeriodTo", "<=", PayPeriodTo));
                payPeriodFilter.add(new Match("pp.PayPeriodTo", ">=", PayPeriodFr));

                foreach (EEmpPersonalInfo empInfo in EmpList)
                {
                    LoadEmpMPFDetail(empInfo, payPeriodFilter);
                }
            }
        }
        protected void LoadEmpMPFDetail(EEmpPersonalInfo empInfo, DBFilter payPeriodFilter)
        {
            EEmpPersonalInfo.db.select(dbConn, empInfo);
            if (empInfo.MasterEmpID > 0 && empInfo.EmpIsCombineMPF)
                return;
            DBFilter empPayrollFilter = new DBFilter();
            empPayrollFilter.add(empInfo.GetAllRoleEmpIDTerms(dbConn, "EmpID", EEmpPersonalInfo.RoleFilterOptionEnum.MPF));
            empPayrollFilter.add(new Match("EmpPayStatus", "C"));
            empPayrollFilter.add(new IN("PayPeriodID", "Select PayPeriodID from PayrollPeriod pp", payPeriodFilter));


            DBFilter mpfRecordFilter = new DBFilter();
            mpfRecordFilter.add(new IN("EmpPayrollID", "Select EmpPayrollID from EmpPayroll", empPayrollFilter));
            mpfRecordFilter.add(new Match("MPFPlanID", MPFPlanID));
            mpfRecordFilter.add("MPfRecPeriodFr", true);
            mpfRecordFilter.add("MPfRecPeriodTo", true);
            ArrayList mpfRecords = EMPFRecord.db.select(dbConn, mpfRecordFilter);

            GenericExistingEmployeeMPFFileDetail existingEmployeeMPFDetail = null;
            GenericNewJoinEmployeeMPFFileDetail newJoinEmployeeMPFDetail = null;
            GenericAdditionalEmployeeMPFFileDetail additionalEmployeeMPFDetail = null;
            GenericBackPaymentEmployeeMPFFileDetail backPaymentEmployeeMPFDetail = null;



            foreach (EMPFRecord mpfRecord in mpfRecords)
            {

                EEmpPayroll empPayroll = new EEmpPayroll();
                empPayroll.EmpPayrollID = mpfRecord.EmpPayrollID;
                EEmpPayroll.db.select(dbConn, empPayroll);
                EPayrollPeriod payrollPeriod = new EPayrollPeriod();
                payrollPeriod.PayPeriodID = empPayroll.PayPeriodID;
                EPayrollPeriod.db.select(dbConn, payrollPeriod);

                if (payrollPeriod.PayPeriodFr < m_PayPeriodFr)
                    m_PayPeriodFr = payrollPeriod.PayPeriodFr;
                if (payrollPeriod.PayPeriodTo > m_PayPeriodTo)
                    m_PayPeriodTo = payrollPeriod.PayPeriodTo;

                if (mpfRecord.MPFRecType.Equals("A"))
                {
                    if (newJoinEmployeeMPFDetail != null)
                        if (newJoinEmployeeMPFDetail.EmpID == newJoinEmployeeMPFDetail.EmpID)
                        {
                            //  Temporily change MPF record as "New Join" Record if previous record is also new join record
                            mpfRecord.MPFRecType = "N";
                        }
                    if (mpfRecord.MPFRecType.Equals("A"))
                    {
                        if (mpfRecord.CanConsiderAsFirstContribution(dbConn))
                            //  Temporily change MPF record as "New Join" Record if this is the first record of MPF List and and match with first MPF Record
                            mpfRecord.MPFRecType = "N";

                    }
                    if (mpfRecord.MPFRecType.Equals("A"))
                    {
                        //  Temporily change MPF record as "Existing" Record
                        mpfRecord.MPFRecType.Equals("E");
                    }
                }


                if (mpfRecord.MPFRecType.Equals("N"))
                {
                    //  New Join Employee
                    DBFilter empTerminationFilter = new DBFilter();
                    empTerminationFilter.add(new Match("EmpID", empInfo.EmpID));
                    empTerminationFilter.add(new Match("EmpTermLastDate", "<=", mpfRecord.MPFRecPeriodTo));
                    empTerminationFilter.add(new Match("EmpTermLastDate", ">=", mpfRecord.MPFRecPeriodFr));
                    if (EEmpTermination.db.count(dbConn, empTerminationFilter) != 0)
                    {
                        //  According to Section 11.9.5 of HSBC Electronic Provision (ver 2.6)
                        //  Only Information is added. No RI and contribution information is included

                        if (additionalEmployeeMPFDetail == null)
                        {
                            additionalEmployeeMPFDetail = new GenericAdditionalEmployeeMPFFileDetail(this, empInfo);
                            additionalEmployeeMPFDetail.AddContributionDetail(mpfRecord, true);
                            AdditionalEmployeeMPFFileDetails.Add(additionalEmployeeMPFDetail);
                        }
                        else
                        {
                            if (additionalEmployeeMPFDetail.EmpID != empInfo.EmpID)
                            {
                                additionalEmployeeMPFDetail = new GenericAdditionalEmployeeMPFFileDetail(this, empInfo);
                                additionalEmployeeMPFDetail.AddContributionDetail(mpfRecord, true);
                                AdditionalEmployeeMPFFileDetails.Add(additionalEmployeeMPFDetail);
                            }
                        }

                    }

                    if (newJoinEmployeeMPFDetail == null)
                    {
                        newJoinEmployeeMPFDetail = new GenericNewJoinEmployeeMPFFileDetail(this, empInfo);
                        newJoinEmployeeMPFDetail.AddContributionDetail(mpfRecord, true);
                        NewJoinEmployeeMPFFileDetails.Add(newJoinEmployeeMPFDetail);
                    }
                    else
                    {
                        if (newJoinEmployeeMPFDetail.EmpID != empInfo.EmpID)
                        {
                            newJoinEmployeeMPFDetail = new GenericNewJoinEmployeeMPFFileDetail(this, empInfo);
                            newJoinEmployeeMPFDetail.AddContributionDetail(mpfRecord, true);
                            NewJoinEmployeeMPFFileDetails.Add(newJoinEmployeeMPFDetail);
                        }
                        else
                            newJoinEmployeeMPFDetail.AddContributionDetail(mpfRecord, true);
                    }

                    //m_TotalRelevantIncome += mpfRecord.MPFRecActMCRI;
                    m_TotalNewJoinMCEE += mpfRecord.MPFRecActMCEE;
                    m_TotalNewJoinMCER += mpfRecord.MPFRecActMCER;
                    m_TotalNewJoinVCEE += mpfRecord.MPFRecActVCEE;
                    m_TotalNewJoinVCER += mpfRecord.MPFRecActVCER; 

                }
                else
                {
                    if (existingEmployeeMPFDetail != null)
                    {
                        if (existingEmployeeMPFDetail.EmpID != empInfo.EmpID)
                            existingEmployeeMPFDetail = null;
                        else
                        {
                            existingEmployeeMPFDetail.AddContributionDetail(mpfRecord, true);
                            m_TotalExistingMCEE += mpfRecord.MPFRecActMCEE;
                            m_TotalExistingMCER += mpfRecord.MPFRecActMCER;
                            m_TotalExistingVCEE += mpfRecord.MPFRecActVCEE;
                            m_TotalExistingVCER += mpfRecord.MPFRecActVCER;
                        }
                    }
                    else if (backPaymentEmployeeMPFDetail != null)
                    {
                        if (backPaymentEmployeeMPFDetail.EmpID != empInfo.EmpID)
                            backPaymentEmployeeMPFDetail = null;
                        else
                        {
                            backPaymentEmployeeMPFDetail.AddContributionDetail(mpfRecord, true);
                            m_TotalBackPaymentMCEE += mpfRecord.MPFRecActMCEE;
                            m_TotalBackPaymentMCER += mpfRecord.MPFRecActMCER;
                            m_TotalBackPaymentVCEE += mpfRecord.MPFRecActVCEE;
                            m_TotalBackPaymentVCER += mpfRecord.MPFRecActVCER;
                        }
                    }

                    if (existingEmployeeMPFDetail == null && backPaymentEmployeeMPFDetail == null)
                    {
                        existingEmployeeMPFDetail = new GenericExistingEmployeeMPFFileDetail(this, empInfo);
                        existingEmployeeMPFDetail.AddContributionDetail(mpfRecord, true);
                        if (existingEmployeeMPFDetail.LastEmploymentDate.Ticks.Equals(0) || existingEmployeeMPFDetail.LastEmploymentDate >= mpfRecord.MPFRecPeriodFr)
                        {
                            ExistingEmployeeMPFFileDetails.Add(existingEmployeeMPFDetail);
                            m_TotalExistingMCEE += mpfRecord.MPFRecActMCEE;
                            m_TotalExistingMCER += mpfRecord.MPFRecActMCER;
                            m_TotalExistingVCEE += mpfRecord.MPFRecActVCEE;
                            m_TotalExistingVCER += mpfRecord.MPFRecActVCER;
                        }
                        else
                        {
                            //  Back Payment
                            existingEmployeeMPFDetail = null;
                            backPaymentEmployeeMPFDetail = new GenericBackPaymentEmployeeMPFFileDetail(this, empInfo);
                            backPaymentEmployeeMPFDetail.AddContributionDetail(mpfRecord, true);
                            BackPaymentEmployeeMPFFileDetails.Add(backPaymentEmployeeMPFDetail);
                            m_TotalBackPaymentMCEE += mpfRecord.MPFRecActMCEE;
                            m_TotalBackPaymentMCER += mpfRecord.MPFRecActMCER;
                            m_TotalBackPaymentVCEE += mpfRecord.MPFRecActVCEE;
                            m_TotalBackPaymentVCER += mpfRecord.MPFRecActVCER;
                        }

                    }


                    //m_TotalRelevantIncome += mpfRecord.MPFRecActMCRI;

                }
                m_TotalRelevantIncome += mpfRecord.MPFRecActMCRI;
                m_TotalVCRelevantIncome += mpfRecord.MPFRecActVCRI;
            }

        }
        public virtual string ActualMPFFileName()
        {
            return "MPFFile" + AppUtils.ServerDateTime().ToString("yyyyMMddHHmmss") + MPFFileExtension();
        }

        public virtual string MPFFileExtension()
        {
            return ".csv";
        }

        public virtual FileInfo GenerateMPFFile()
        {
            string[] bankFileHeader = new string[3];
            //bankFileHeader[0] = BankCode + BranchCode + AccountNo;
            //bankFileHeader[1] = TotalAmount.ToString("0.00");
            //bankFileHeader[2] = ValueDate.ToString("yyyyMMdd");

            throw new Exception("MPF file generation is not available for this scheme");

            //string bankFileData = string.Join(FIELD_DELIMITER, bankFileHeader) + RECORD_DELIMITER;
            //foreach (GenericNewJoinEmployeeMPFFileDetail bankFileDetail in NewJoinEmployeeMPFFileDetails)
            //{
            //    bankFileData += GenerateNewJoinMPFFileDetail(bankFileDetail) + RECORD_DELIMITER;
            //}
            //foreach (GenericExistingEmployeeMPFFileDetail bankFileDetail in ExistingEmployeeMPFFileDetails)
            //{
            //    bankFileData += GenerateExistingMPFFileDetail(bankFileDetail) + RECORD_DELIMITER;
            //}
            //foreach (GenericAdditionalEmployeeMPFFileDetail bankFileDetail in AdditionalEmployeeMPFFileDetails)
            //{
            //    bankFileData += GenerateAdditionalMPFFileDetail(bankFileDetail) + RECORD_DELIMITER;
            //}

            //foreach (GenericBackPaymentEmployeeMPFFileDetail bankFileDetail in BackPaymentEmployeeMPFFileDetails)
            //{
            //    bankFileData += GenerateBackPaymentMPFFileDetail(bankFileDetail) + RECORD_DELIMITER;
            //}

            //FileInfo result = GenerateTempFileName();
            //StreamWriter writer = new StreamWriter(result.OpenWrite());
            //writer.Write(bankFileData);
            //writer.Close();
            //return result;
        }

        protected FileInfo GenerateTempFileName()
        {
            string exportFileName = Path.GetTempFileName();
            FileInfo fileInfo = new FileInfo(exportFileName);
            //fileInfo.MoveTo(exportFileName += MPFFileExtension());
            return fileInfo;

        }

        protected virtual string GenerateNewJoinMPFFileDetail(GenericNewJoinEmployeeMPFFileDetail mpfFileDetail)
        {
           return string.Empty;
        }
        protected virtual string GenerateExistingMPFFileDetail(GenericExistingEmployeeMPFFileDetail mpfFileDetail)
        {
            return string.Empty;
        }
        protected virtual string GenerateAdditionalMPFFileDetail(GenericAdditionalEmployeeMPFFileDetail mpfFileDetail)
        {
            return string.Empty;
        }
        protected virtual string GenerateBackPaymentMPFFileDetail(GenericBackPaymentEmployeeMPFFileDetail mpfFileDetail)
        {
            return string.Empty;
        }

        public HROne.MPFFile.DataSet.Payroll_MPFFirstContributionStatement CreateFirstContributionStatementDataSet()
        {
            DataSet.Payroll_MPFFirstContributionStatement dataSet = new DataSet.Payroll_MPFFirstContributionStatement();

            string defaultClassName =string.Empty;

            EMPFPlan mpfPlanObject = new EMPFPlan();
            mpfPlanObject.MPFPlanID = MPFPlanID;
            if (EMPFPlan.db.select(dbConn, mpfPlanObject))
            {
                DataSet.Payroll_MPFFirstContributionStatement.MPFPlanRow mpfPlanRow = dataSet.MPFPlan.NewMPFPlanRow();
                mpfPlanRow.MPFPlanID = mpfPlanObject.MPFPlanID;
                mpfPlanRow.MPFPlanCode = mpfPlanObject.MPFPlanCode;
                mpfPlanRow.MPFPlanCompanyAddress = mpfPlanObject.MPFPlanCompanyAddress;
                mpfPlanRow.MPFPlanCompanyName = mpfPlanObject.MPFPlanCompanyName;
                mpfPlanRow.MPFPlanContactName = mpfPlanObject.MPFPlanContactName;
                mpfPlanRow.MPFPlanContactNo = mpfPlanObject.MPFPlanContactNo;
                mpfPlanRow.MPFPlanDesc = mpfPlanObject.MPFPlanDesc;
                mpfPlanRow.MPFPlanParticipationNo = mpfPlanObject.MPFPlanParticipationNo;
                mpfPlanRow.MPFSchemeID = mpfPlanObject.MPFSchemeID;

                // get Employer ID
                System.Xml.XmlNodeList mpfEmployerIDNode = Utility.GetXmlDocumentByDataString(MPFPlanExtendXMLString).GetElementsByTagName("MPFPlanEmployerID");
                if (mpfEmployerIDNode.Count > 0)
                    mpfPlanRow.MPFHSBCEmployerID = mpfEmployerIDNode[0].InnerText.PadRight(8).Substring(0, 8);

                if (!string.IsNullOrEmpty(mpfPlanObject.MPFPlanExtendData))
                {
                    //  HSBC, Hang Seng Pay Center
                    System.Xml.XmlNodeList payCenterList = Utility.GetXmlDocumentByDataString(mpfPlanObject.MPFPlanExtendData).DocumentElement.GetElementsByTagName("MPFPlanPayCenter");
                    if (payCenterList.Count > 0)
                    {
                        mpfPlanRow.MPFHSBCPayCenter = payCenterList[0].InnerText.Trim().ToUpper();
                    }
                    //  AIA Employer Plan No.
                    System.Xml.XmlNodeList AIAERPlanNoList = Utility.GetXmlDocumentByDataString(mpfPlanObject.MPFPlanExtendData).DocumentElement.GetElementsByTagName("MPFPlanAIAERPlanNo");
                    if (AIAERPlanNoList.Count > 0)
                    {
                        mpfPlanRow.MPFPlanAIAERPlanNo = AIAERPlanNoList[0].InnerText;
                    }
                    //  BOCI Scheme No.
                    System.Xml.XmlNodeList BOCISchemeNoList = Utility.GetXmlDocumentByDataString(mpfPlanObject.MPFPlanExtendData).DocumentElement.GetElementsByTagName("MPFPlanSchemeNo");
                    if (BOCISchemeNoList.Count > 0)
                    {
                        mpfPlanRow.MPFPlanBOCISchemeNo = BOCISchemeNoList[0].InnerText;
                    }
                }

                System.Xml.XmlNodeList defaultClassNameNode = Utility.GetXmlDocumentByDataString(mpfPlanObject.MPFPlanExtendData).GetElementsByTagName("MPFPlanDefaultClassName");
                if (defaultClassNameNode.Count > 0)
                {
                    defaultClassName = defaultClassNameNode[0].InnerText.Trim().ToUpper();
                }
                else
                    defaultClassName = string.Empty;

                EMPFScheme mpfSchemeObject = new EMPFScheme();
                mpfSchemeObject.MPFSchemeID = mpfPlanObject.MPFSchemeID;
                if (EMPFScheme.db.select(dbConn, mpfSchemeObject))
                {
                    DataSet.Payroll_MPFFirstContributionStatement.MPFSchemeRow schemeRow = dataSet.MPFScheme.NewMPFSchemeRow();
                    schemeRow.MPFSchemeCode = mpfSchemeObject.MPFSchemeCode;
                    schemeRow.MPFSchemeDesc = mpfSchemeObject.MPFSchemeDesc;
                    schemeRow.MPFSchemeID = mpfSchemeObject.MPFSchemeID;
                    dataSet.MPFScheme.Rows.Add(schemeRow);
                }
                dataSet.MPFPlan.Rows.Add(mpfPlanRow);
            }

            foreach (GenericNewJoinEmployeeMPFFileDetail bankFileDetail in NewJoinEmployeeMPFFileDetails)
            {
                foreach (GenericMPFFileContributionDetail mpfDetail in bankFileDetail.MPFContributionDetailList)
                {
                    DataSet.Payroll_MPFFirstContributionStatement.NewJoinMemberRow newJoinMemberRow = dataSet.NewJoinMember.NewNewJoinMemberRow();
                    System.Xml.XmlNodeList classNameNode = Utility.GetXmlDocumentByDataString(bankFileDetail.EmpMPFPlanExtendXMLString).GetElementsByTagName("EmpMPFPlanClassName");
                    if (classNameNode.Count > 0)
                    {
                        newJoinMemberRow.ClassNo = classNameNode[0].InnerText.Trim().ToUpper();
                        if (string.IsNullOrEmpty(newJoinMemberRow.ClassNo))
                            newJoinMemberRow.ClassNo = defaultClassName;
                    }
                    else
                        newJoinMemberRow.ClassNo = defaultClassName;

                    newJoinMemberRow.DateOfBirth = bankFileDetail.DateOfBirth;
                    newJoinMemberRow.EmpID = bankFileDetail.EmpID;
                    newJoinMemberRow.EmpNo = bankFileDetail.EmpNo;
                    newJoinMemberRow.EmpOtherName = bankFileDetail.EmpOtherName;
                    newJoinMemberRow.EmpSurname = bankFileDetail.EmpSurname;

                    EEmpPersonalInfo empPersonalInfo = EEmpPersonalInfo.GetObject(dbConn, bankFileDetail.EmpID);
                    if (empPersonalInfo != null)
                    {
                        newJoinMemberRow.Alias = empPersonalInfo.EmpAlias;
                    }

                    newJoinMemberRow.HKID = bankFileDetail.HKIDPassport;
                    if (bankFileDetail.HKIDType == IDENTITY_TYPE_ENUM.HKID)
                        newJoinMemberRow.HKIDType = 1;
                    else if (bankFileDetail.HKIDType == IDENTITY_TYPE_ENUM.PASSPORT)
                        newJoinMemberRow.HKIDType = 2;
                    else
                        newJoinMemberRow.HKIDType = 0;

                    if (bankFileDetail.MemberType == MEMBER_TYPE_ENUM.NORMAL)
                        newJoinMemberRow.MemberType = 1;
                    else if (bankFileDetail.MemberType == MEMBER_TYPE_ENUM.NORMAL)
                        newJoinMemberRow.MemberType = 2;
                    else
                        newJoinMemberRow.MemberType = 0;
                    newJoinMemberRow.MPFPlanID = MPFPlanID;
                    newJoinMemberRow.SchemeJoinDate = bankFileDetail.SchemeJoinDate;
                    newJoinMemberRow.Sex = bankFileDetail.Sex;

                    newJoinMemberRow.MCEE = mpfDetail.MCEE;
                    newJoinMemberRow.MCER = mpfDetail.MCER;
                    newJoinMemberRow.PeriodFrom = mpfDetail.PeriodFrom;
                    newJoinMemberRow.PeriodTo = mpfDetail.PeriodTo;
                    newJoinMemberRow.RelevantIncome = mpfDetail.RelevantIncome;
                    newJoinMemberRow.VCEE = mpfDetail.VCEE;
                    newJoinMemberRow.VCER = mpfDetail.VCER;

                    
                    EEmpPersonalInfo empInfo = EEmpPersonalInfo.GetObject(dbConn, bankFileDetail.EmpID);

                    if (empInfo != null && empInfo.EmpDateOfJoin != null)
                    {
                        newJoinMemberRow.JoinDate = empInfo.EmpDateOfJoin;

                        DBFilter m_positionFilter = new DBFilter();
                        m_positionFilter.add(new Match("EmpID", bankFileDetail.EmpID));
                        m_positionFilter.add(new Match("EmpPosEffFr", ">=", empInfo.EmpDateOfJoin));
                        
                        OR m_or = new OR();
                        m_or.add(new Match("EmpPosEffTo", "<=", empInfo.EmpDateOfJoin));
                        m_or.add(new NullTerm("EmpPosEffTo"));
                        m_positionFilter.add(m_or);
                        foreach(EEmpPositionInfo e in EEmpPositionInfo.db.select(dbConn, m_positionFilter) )
                        {
                            ECompany m_company = new ECompany();
                            m_company.CompanyID = e.CompanyID;
                            if (ECompany.db.select(dbConn, m_company))
                            {
                                newJoinMemberRow.EmpCompany = m_company.CompanyName;
                            }

                            DBFilter hLevelFilter = new DBFilter();
                            hLevelFilter.add("HLevelSeqNo", true);
                            ArrayList HierarchyLevelList = EHierarchyLevel.db.select(dbConn, hLevelFilter);
                            ArrayList EmpHierarchyList;

                            DBFilter empHierarchyFilter = new DBFilter();
                            if (HierarchyLevelList.Count > 0)
                            {
                                empHierarchyFilter = new DBFilter();

                                empHierarchyFilter.add(new Match("EmpPosID", e.EmpPosID));
                                empHierarchyFilter.add(new Match("HLevelID", ((EHierarchyLevel)HierarchyLevelList[0]).HLevelID));

                                EmpHierarchyList = EEmpHierarchy.db.select(dbConn, empHierarchyFilter);
                                if (EmpHierarchyList.Count > 0)
                                {
                                    EHierarchyElement element = EHierarchyElement.GetObject(dbConn, ((EEmpHierarchy)EmpHierarchyList[0]).HElementID);
                                    if (element != null)
                                    {
                                        newJoinMemberRow.EmpDivision = element.HElementDesc;
                                    }
                                }

                            }

                            if (HierarchyLevelList.Count > 1)
                            {
                                empHierarchyFilter = new DBFilter();

                                empHierarchyFilter.add(new Match("EmpPosID", e.EmpPosID));
                                empHierarchyFilter.add(new Match("HLevelID", ((EHierarchyLevel)HierarchyLevelList[1]).HLevelID));

                                EmpHierarchyList = EEmpHierarchy.db.select(dbConn, empHierarchyFilter);
                                if (EmpHierarchyList.Count > 0)
                                {
                                    EHierarchyElement element = EHierarchyElement.GetObject(dbConn, ((EEmpHierarchy)EmpHierarchyList[0]).HElementID);
                                    if (element != null)
                                    {
                                        newJoinMemberRow.EmpDepartment = element.HElementDesc;
                                    }
                                }
                            }

                            break; // assume only one position history at new join date
                        }
                    }

                    dataSet.NewJoinMember.Rows.Add(newJoinMemberRow);
                }

            }

            return dataSet;
        }

        public HROne.MPFFile.DataSet.Payroll_MPFRemittanceStatement CreateRemittanceStatementDataSet()
        {
            DataSet.Payroll_MPFRemittanceStatement dataSet = new DataSet.Payroll_MPFRemittanceStatement();

            string defaultClassName = string.Empty;

            EMPFPlan mpfPlanObject = new EMPFPlan();
            mpfPlanObject.MPFPlanID = MPFPlanID;
            if (EMPFPlan.db.select(dbConn, mpfPlanObject))
            {

                DataSet.Payroll_MPFRemittanceStatement.MPFPlanRow mpfPlanRow = dataSet.MPFPlan.NewMPFPlanRow();
                mpfPlanRow.MPFPlanID = mpfPlanObject.MPFPlanID;
                mpfPlanRow.MPFPlanCode = mpfPlanObject.MPFPlanCode;
                mpfPlanRow.MPFPlanCompanyAddress = mpfPlanObject.MPFPlanCompanyAddress;
                mpfPlanRow.MPFPlanCompanyName = mpfPlanObject.MPFPlanCompanyName;
                mpfPlanRow.MPFPlanContactName = mpfPlanObject.MPFPlanContactName;
                mpfPlanRow.MPFPlanContactNo = mpfPlanObject.MPFPlanContactNo;
                mpfPlanRow.MPFPlanDesc = mpfPlanObject.MPFPlanDesc;
                mpfPlanRow.MPFPlanParticipationNo = mpfPlanObject.MPFPlanParticipationNo;
                mpfPlanRow.MPFSchemeID = mpfPlanObject.MPFSchemeID;

                // get Employer ID
                System.Xml.XmlNodeList mpfEmployerIDNode = Utility.GetXmlDocumentByDataString(MPFPlanExtendXMLString).GetElementsByTagName("MPFPlanEmployerID");
                if (mpfEmployerIDNode.Count > 0)
                    mpfPlanRow.MPFHSBCEmployerID = mpfEmployerIDNode[0].InnerText.PadRight(8).Substring(0, 8);

                if (!string.IsNullOrEmpty(mpfPlanObject.MPFPlanExtendData))
                {
                    //  HSBC, Hang Seng Pay Center
                    System.Xml.XmlNodeList payCenterList = Utility.GetXmlDocumentByDataString(mpfPlanObject.MPFPlanExtendData).DocumentElement.GetElementsByTagName("MPFPlanPayCenter");
                    if (payCenterList.Count > 0)
                    {
                        mpfPlanRow.MPFHSBCPayCenter = payCenterList[0].InnerText.Trim().ToUpper();
                    }
                    //  AIA Employer Plan No.
                    System.Xml.XmlNodeList AIAERPlanNoList = Utility.GetXmlDocumentByDataString(mpfPlanObject.MPFPlanExtendData).DocumentElement.GetElementsByTagName("MPFPlanAIAERPlanNo");
                    if (AIAERPlanNoList.Count > 0)
                    {
                        mpfPlanRow.MPFPlanAIAERPlanNo = AIAERPlanNoList[0].InnerText;
                    }
                    //  BOCI Scheme No.
                    System.Xml.XmlNodeList BOCISchemeNoList = Utility.GetXmlDocumentByDataString(mpfPlanObject.MPFPlanExtendData).DocumentElement.GetElementsByTagName("MPFPlanSchemeNo");
                    if (BOCISchemeNoList.Count > 0)
                    {
                        mpfPlanRow.MPFPlanBOCISchemeNo = BOCISchemeNoList[0].InnerText;
                    }
                }

                System.Xml.XmlNodeList defaultClassNameNode = Utility.GetXmlDocumentByDataString(mpfPlanObject.MPFPlanExtendData).GetElementsByTagName("MPFPlanDefaultClassName");
                if (defaultClassNameNode.Count > 0)
                {
                    defaultClassName = defaultClassNameNode[0].InnerText.Trim().ToUpper();
                }
                else
                    defaultClassName = string.Empty;

                EMPFScheme mpfSchemeObject = new EMPFScheme();
                mpfSchemeObject.MPFSchemeID = mpfPlanObject.MPFSchemeID;
                if (EMPFScheme.db.select(dbConn, mpfSchemeObject))
                {

                    DataSet.Payroll_MPFRemittanceStatement.MPFSchemeRow schemeRow = dataSet.MPFScheme.NewMPFSchemeRow();
                    schemeRow.MPFSchemeCode = mpfSchemeObject.MPFSchemeCode;
                    schemeRow.MPFSchemeDesc = mpfSchemeObject.MPFSchemeDesc;
                    schemeRow.MPFSchemeID = mpfSchemeObject.MPFSchemeID;
                    dataSet.MPFScheme.Rows.Add(schemeRow);

                    DBFilter CessationReasonFilter = new DBFilter();
                    CessationReasonFilter.add(new Match("MPFSchemeID", mpfPlanObject.MPFSchemeID));
                    ArrayList cessationReasonList = EMPFSchemeCessationReason.db.select(dbConn, CessationReasonFilter);

                    foreach (EMPFSchemeCessationReason schemeCessationReason in cessationReasonList)
                    {
                        DataSet.Payroll_MPFRemittanceStatement.MPFSchemeCessationReasonRow schemeCessationReasonRow = dataSet.MPFSchemeCessationReason.NewMPFSchemeCessationReasonRow();
                        schemeCessationReasonRow.MPFSchemeCessationReasonCode = schemeCessationReason.MPFSchemeCessationReasonCode;
                        schemeCessationReasonRow.MPFSchemeCessationReasonDesc = schemeCessationReason.MPFSchemeCessationReasonDesc;
                        dataSet.MPFSchemeCessationReason.AddMPFSchemeCessationReasonRow(schemeCessationReasonRow);
                    }
                }
                dataSet.MPFPlan.Rows.Add(mpfPlanRow);
            }


            foreach (GenericExistingEmployeeMPFFileDetail bankFileDetail in ExistingEmployeeMPFFileDetails)
            {
                foreach (GenericMPFFileContributionDetail mpfDetail in bankFileDetail.MPFContributionDetailList)
                {

                    DataSet.Payroll_MPFRemittanceStatement.ExistingMemberRow existingMemberRow = dataSet.ExistingMember.NewExistingMemberRow();
                    existingMemberRow.EmpID = bankFileDetail.EmpID;
                    existingMemberRow.EmpName = bankFileDetail.EmpName;
                    existingMemberRow.EmpNo = bankFileDetail.EmpNo;
                    existingMemberRow.HKID = bankFileDetail.HKIDPassport;
                    existingMemberRow.SchemeJoinDate = bankFileDetail.SchemeJoinDate;
                    if (!bankFileDetail.LastEmploymentDate.Ticks.Equals(0))
                    {
                        existingMemberRow.IsLSP = (bankFileDetail.LspSpFlag == LSPSP_FLAG_ENUM.LSP);
                        existingMemberRow.IsSP = (bankFileDetail.LspSpFlag == LSPSP_FLAG_ENUM.SP);
                        existingMemberRow.LastEmploymentDate = bankFileDetail.LastEmploymentDate;
                        existingMemberRow.LSPSPAmount = bankFileDetail.LSPSPAmount;
                        existingMemberRow.LSPSPAmountPaidByER = bankFileDetail.LSPSPAmountPaidByER;
                        existingMemberRow.TermCode = bankFileDetail.TermCode;

                        InsertCessationReasonCode(dataSet.MPFSchemeCessationReason, bankFileDetail.TermCode, bankFileDetail.CessationReasonID);
                    }
                    existingMemberRow.MCEE = mpfDetail.MCEE;
                    existingMemberRow.MCER = mpfDetail.MCER;
                    existingMemberRow.MPFPlanID = MPFPlanID;
                    existingMemberRow.PeriodFrom = mpfDetail.PeriodFrom;
                    existingMemberRow.PeriodTo = mpfDetail.PeriodTo;
                    existingMemberRow.RelevantIncome = mpfDetail.RelevantIncome;
                    existingMemberRow.VCEE = mpfDetail.VCEE;
                    existingMemberRow.VCER = mpfDetail.VCER;
                    existingMemberRow.IsBackPayRecord = false;
                    dataSet.ExistingMember.Rows.Add(existingMemberRow);
                }

            }
            foreach (GenericNewJoinEmployeeMPFFileDetail bankFileDetail in NewJoinEmployeeMPFFileDetails)
            {
                foreach (GenericMPFFileContributionDetail mpfDetail in bankFileDetail.MPFContributionDetailList)
                {

                    DataSet.Payroll_MPFRemittanceStatement.NewJoinMemberRow newJoinMemberRow = dataSet.NewJoinMember.NewNewJoinMemberRow();
                    System.Xml.XmlNodeList classNameNode = Utility.GetXmlDocumentByDataString(bankFileDetail.EmpMPFPlanExtendXMLString).GetElementsByTagName("EmpMPFPlanClassName");
                    if (classNameNode.Count > 0)
                    {
                        newJoinMemberRow.ClassNo = classNameNode[0].InnerText.Trim().ToUpper();
                        if (string.IsNullOrEmpty(newJoinMemberRow.ClassNo))
                            newJoinMemberRow.ClassNo = defaultClassName;
                    }
                    else
                        newJoinMemberRow.ClassNo = defaultClassName;

                    newJoinMemberRow.DateOfBirth = bankFileDetail.DateOfBirth;
                    newJoinMemberRow.EmpID = bankFileDetail.EmpID;
                    newJoinMemberRow.EmpNo = bankFileDetail.EmpNo;
                    newJoinMemberRow.EmpOtherName = bankFileDetail.EmpOtherName;
                    newJoinMemberRow.EmpSurname = bankFileDetail.EmpSurname;
                    newJoinMemberRow.HKID = bankFileDetail.HKIDPassport;
                    if (bankFileDetail.HKIDType == IDENTITY_TYPE_ENUM.HKID)
                        newJoinMemberRow.HKIDType = 1;
                    else if (bankFileDetail.HKIDType == IDENTITY_TYPE_ENUM.PASSPORT)
                        newJoinMemberRow.HKIDType = 2;
                    else
                        newJoinMemberRow.HKIDType = 0;

                    if (bankFileDetail.MemberType == MEMBER_TYPE_ENUM.NORMAL)
                        newJoinMemberRow.MemberType = 1;
                    else if (bankFileDetail.MemberType == MEMBER_TYPE_ENUM.NORMAL)
                        newJoinMemberRow.MemberType = 2;
                    else
                        newJoinMemberRow.MemberType = 0;
                    newJoinMemberRow.MPFPlanID = MPFPlanID;
                    newJoinMemberRow.SchemeJoinDate = bankFileDetail.SchemeJoinDate;
                    newJoinMemberRow.Sex = bankFileDetail.Sex;

                    newJoinMemberRow.MCEE = mpfDetail.MCEE;
                    newJoinMemberRow.MCER = mpfDetail.MCER;
                    newJoinMemberRow.PeriodFrom = mpfDetail.PeriodFrom;
                    newJoinMemberRow.PeriodTo = mpfDetail.PeriodTo;
                    newJoinMemberRow.RelevantIncome = mpfDetail.RelevantIncome;
                    newJoinMemberRow.VCEE = mpfDetail.VCEE;
                    newJoinMemberRow.VCER = mpfDetail.VCER;
                    dataSet.NewJoinMember.Rows.Add(newJoinMemberRow);
                }

            }

            foreach (GenericAdditionalEmployeeMPFFileDetail bankFileDetail in AdditionalEmployeeMPFFileDetails)
            {
                foreach (GenericMPFFileContributionDetail mpfDetail in bankFileDetail.MPFContributionDetailList)
                {

                    HROne.MPFFile.DataSet.Payroll_MPFRemittanceStatement.AdditionalMemberRow AdditionalMemberRow = dataSet.AdditionalMember.NewAdditionalMemberRow();
                    AdditionalMemberRow.EmpID = bankFileDetail.EmpID;
                    AdditionalMemberRow.EmpName = bankFileDetail.EmpName;
                    AdditionalMemberRow.EmpNo = bankFileDetail.EmpNo;
                    AdditionalMemberRow.HKID = bankFileDetail.HKIDPassport;
                    if (!bankFileDetail.LastEmploymentDate.Ticks.Equals(0))
                    {
                        AdditionalMemberRow.IsLSP = (bankFileDetail.LspSpFlag == LSPSP_FLAG_ENUM.LSP);
                        AdditionalMemberRow.IsSP = (bankFileDetail.LspSpFlag == LSPSP_FLAG_ENUM.SP);
                        AdditionalMemberRow.LastEmploymentDate = bankFileDetail.LastEmploymentDate;
                        AdditionalMemberRow.LSPSPAmount = bankFileDetail.LSPSPAmount;
                        AdditionalMemberRow.LSPSPAmountPaidByER = bankFileDetail.LSPSPAmountPaidByER;
                        AdditionalMemberRow.TermCode = bankFileDetail.TermCode;

                        InsertCessationReasonCode(dataSet.MPFSchemeCessationReason, bankFileDetail.TermCode, bankFileDetail.CessationReasonID);
                    }
                    AdditionalMemberRow.MCEE = mpfDetail.MCEE;
                    AdditionalMemberRow.MCER = mpfDetail.MCER;
                    AdditionalMemberRow.MPFPlanID = MPFPlanID;
                    AdditionalMemberRow.PeriodFrom = mpfDetail.PeriodFrom;
                    AdditionalMemberRow.PeriodTo = mpfDetail.PeriodTo;
                    AdditionalMemberRow.RelevantIncome = mpfDetail.RelevantIncome;
                    AdditionalMemberRow.VCEE = mpfDetail.VCEE;
                    AdditionalMemberRow.VCER = mpfDetail.VCER;
                    dataSet.AdditionalMember.Rows.Add(AdditionalMemberRow);
                }

            }
            foreach (GenericBackPaymentEmployeeMPFFileDetail bankFileDetail in BackPaymentEmployeeMPFFileDetails)
            {
                foreach (GenericMPFFileContributionDetail mpfDetail in bankFileDetail.MPFContributionDetailList)
                {

                    HROne.MPFFile.DataSet.Payroll_MPFRemittanceStatement.ExistingMemberRow backPaymentMemberRow = dataSet.ExistingMember.NewExistingMemberRow();
                    backPaymentMemberRow.EmpID = bankFileDetail.EmpID;
                    backPaymentMemberRow.EmpName = bankFileDetail.EmpName;
                    backPaymentMemberRow.EmpNo = bankFileDetail.EmpNo;
                    backPaymentMemberRow.SchemeJoinDate = bankFileDetail.SchemeJoinDate;
                    backPaymentMemberRow.HKID = bankFileDetail.HKIDPassport;
                    if (!bankFileDetail.LastEmploymentDate.Ticks.Equals(0))
                    {
                        backPaymentMemberRow.IsLSP = (bankFileDetail.LspSpFlag == LSPSP_FLAG_ENUM.LSP);
                        backPaymentMemberRow.IsSP = (bankFileDetail.LspSpFlag == LSPSP_FLAG_ENUM.SP);
                        backPaymentMemberRow.LastEmploymentDate = bankFileDetail.LastEmploymentDate;
                        backPaymentMemberRow.LSPSPAmount = bankFileDetail.LSPSPAmount;
                        backPaymentMemberRow.LSPSPAmountPaidByER = bankFileDetail.LSPSPAmountPaidByER;
                        backPaymentMemberRow.TermCode = bankFileDetail.TermCode;

                        InsertCessationReasonCode(dataSet.MPFSchemeCessationReason, bankFileDetail.TermCode, bankFileDetail.CessationReasonID);
                    }
                    backPaymentMemberRow.MCEE = mpfDetail.MCEE;
                    backPaymentMemberRow.MCER = mpfDetail.MCER;
                    backPaymentMemberRow.MPFPlanID = MPFPlanID;
                    backPaymentMemberRow.PeriodFrom = mpfDetail.PeriodFrom;
                    backPaymentMemberRow.PeriodTo = mpfDetail.PeriodTo;
                    backPaymentMemberRow.RelevantIncome = mpfDetail.RelevantIncome;
                    backPaymentMemberRow.VCEE = mpfDetail.VCEE;
                    backPaymentMemberRow.VCER = mpfDetail.VCER;
                    backPaymentMemberRow.IsBackPayRecord = true;
                    dataSet.ExistingMember.Rows.Add(backPaymentMemberRow);
                }

            }
            return dataSet;
        }


        public void InsertCessationReasonCode(DataSet.Payroll_MPFRemittanceStatement.MPFSchemeCessationReasonDataTable dataTable, string TermCode, int CessationReasonID)
        {
            if (dataTable.FindByMPFSchemeCessationReasonCode(TermCode) == null)
            {
                ECessationReason cessationReason = new ECessationReason();
                cessationReason.CessationReasonID = CessationReasonID;
                if (ECessationReason.db.select(dbConn, cessationReason))
                {
                    dataTable.AddMPFSchemeCessationReasonRow(cessationReason.CessationReasonCode, cessationReason.CessationReasonDesc);
                }
            }
        }
    }

    public class GenericExistingEmployeeMPFFileDetail :GenericMPFFileDetail 
    {
        public GenericExistingEmployeeMPFFileDetail(GenericMPFFile MPFFile, EEmpPersonalInfo empInfo)
            : base(MPFFile, empInfo)
        {
        }

        public override void AddContributionDetail(EMPFRecord mpfRecord, bool AddIfPeriodDoesNotExists)
        {
            base.AddContributionDetail(mpfRecord, AddIfPeriodDoesNotExists);

            DBFilter empTerminationFilter = new DBFilter();
            empTerminationFilter.add(new Match("EmpID", EmpID));
            empTerminationFilter.add(new Match("EmpTermLastDate", "<=", mpfRecord.MPFRecPeriodTo));
            //empTerminationFilter.add(new Match("EmpTermLastDate", ">=", mpfRecord.MPFRecPeriodFr));
            ArrayList empTerminations = EEmpTermination.db.select(dbConn, empTerminationFilter);
            if (empTerminations.Count > 0)
            {
                EEmpTermination empTermination = (EEmpTermination)empTerminations[0];
                m_LastEmploymentDate = empTermination.EmpTermLastDate;

                //ECessationReason cessationReason = new ECessationReason();
                //cessationReason.CessationReasonID = empTermination.CessationReasonID;
                //ECessationReason.db.select(dbConn, cessationReason);
                //TermCode = cessationReason.CessationReasonCode;
                m_CessationReasonID = empTermination.CessationReasonID;
                m_TermCode = EMPFSchemeCessationReasonMapping.GetMPFSchemeTermCode(dbConn, MPFFile.MPFSchemeID, empTermination.CessationReasonID);

                EEmpPayroll empPayroll = new EEmpPayroll();
                empPayroll.EmpPayrollID = mpfRecord.EmpPayrollID;
                if (EEmpPayroll.db.select(dbConn, empPayroll))
                {
                    EPayrollBatch payBatch = new EPayrollBatch();
                    payBatch.PayBatchID = empPayroll.PayBatchID;
                    if (EPayrollBatch.db.select(dbConn, payBatch))
                    {
                        if (payBatch.PayBatchValueDate.Ticks != 0)
                        {
                            m_LastPaymentDate = payBatch.PayBatchValueDate;
                        }
                        else
                            m_LastPaymentDate = payBatch.PayBatchConfirmDate;

                    }
                    else
                        m_LastPaymentDate = mpfRecord.MPFRecPeriodTo;
                }
                else
                    m_LastPaymentDate = mpfRecord.MPFRecPeriodTo;

                DBFilter paymentCodeFilter = new DBFilter();
                //OR orPaymentCodeTerms = new OR();
                //orPaymentCodeTerms.add(new Match("PaymentCode", Payroll.FinalPaymentProcess.PAYMENTCODE_LONGSERVICE));
                //orPaymentCodeTerms.add(new Match("PaymentCode", Payroll.FinalPaymentProcess.PAYMENTCODE_SEVERANCE));
                paymentCodeFilter.add(new Match("PaymentTypeID", EPaymentType.SystemPaymentType.LongServicePaymentSeverancePaymentType(dbConn).PaymentTypeID));

                DBFilter finalPaymentFilter = new DBFilter();
                finalPaymentFilter.add(new IN("PayCodeID", "Select PaymentCodeID from " + EPaymentCode.db.dbclass.tableName, paymentCodeFilter));
                finalPaymentFilter.add(new Match("EmpID", EmpID));
                ArrayList empFinalPaymentList = EEmpFinalPayment.db.select(dbConn, finalPaymentFilter);

                m_LSPSPAmount = 0;
                foreach (EEmpFinalPayment empFinalPayment in empFinalPaymentList)
                {
                    m_LSPSPAmount += empFinalPayment.EmpFinalPayAmount;
                    EPaymentCode paymentcode = new EPaymentCode();
                    paymentcode.PaymentCodeID = empFinalPayment.PayCodeID;
                    EPaymentCode.db.select(dbConn, paymentcode);
                    if (paymentcode.PaymentCode.Equals(Payroll.FinalPaymentProcess.PAYMENTCODE_LONGSERVICE))
                        m_LspSpFlag = LSPSP_FLAG_ENUM.LSP;
                    else if (paymentcode.PaymentCode.Equals(Payroll.FinalPaymentProcess.PAYMENTCODE_SEVERANCE))
                        m_LspSpFlag = LSPSP_FLAG_ENUM.SP;

                }
                DBFilter empPayrollFilter = new DBFilter();
                empPayrollFilter.add(new Match("EmpID", EmpID));

                DBFilter payRecordFilter = new DBFilter();
                payRecordFilter.add(new IN("PaymentCodeID", "Select PaymentCodeID from " + EPaymentCode.db.dbclass.tableName, paymentCodeFilter));
                payRecordFilter.add(new IN("EmpPayrollID", "Select EmpPayrollID from " + EEmpPayroll.db.dbclass.tableName, empPayrollFilter));

                ArrayList empPayRecordList = EPaymentRecord.db.select(dbConn, payRecordFilter);

                m_LSPSPAmountPaidByER = 0;

                foreach (EPaymentRecord payRecord in empPayRecordList)
                {
                    m_LSPSPAmountPaidByER += payRecord.PayRecActAmount;
                    if (LspSpFlag == LSPSP_FLAG_ENUM.NONE)
                    {
                        EPaymentCode paymentcode = new EPaymentCode();
                        paymentcode.PaymentCodeID = payRecord.PaymentCodeID;
                        EPaymentCode.db.select(dbConn, paymentcode);
                        if (paymentcode.PaymentCode.Equals(Payroll.FinalPaymentProcess.PAYMENTCODE_LONGSERVICE))
                            m_LspSpFlag = LSPSP_FLAG_ENUM.LSP;
                        else if (paymentcode.PaymentCode.Equals(Payroll.FinalPaymentProcess.PAYMENTCODE_SEVERANCE))
                            m_LspSpFlag = LSPSP_FLAG_ENUM.SP;
                    }
                }
                if (LSPSPAmount < 0.01)
                    m_LspSpFlag = LSPSP_FLAG_ENUM.NONE;
                if (LSPSPAmountPaidByER > LSPSPAmount)
                    m_LSPSPAmount = LSPSPAmountPaidByER;
            }


        }

        protected DateTime m_LastEmploymentDate;
        public DateTime LastEmploymentDate
        {
            get { return m_LastEmploymentDate; }
            set { m_LastEmploymentDate = value; }
        }

        protected int m_CessationReasonID = 0;
        public int CessationReasonID
        {
            get { return m_CessationReasonID; }
            set { m_CessationReasonID = value; }
        }
        
        protected string m_TermCode;
        public string TermCode
        {
            get { return m_TermCode; }
            set { m_TermCode = value; }
        }

        protected DateTime m_LastPaymentDate;
        public DateTime LastPaymentDate
        {
            get { return m_LastPaymentDate; }
            set { m_LastPaymentDate = value; }
        }

        protected LSPSP_FLAG_ENUM m_LspSpFlag;
        public LSPSP_FLAG_ENUM LspSpFlag
        {
            get { return m_LspSpFlag; }
            set { m_LspSpFlag = value; }
        }

        protected double m_LSPSPAmount;
        public double LSPSPAmount
        {
            get { return m_LSPSPAmount; }
            set { m_LSPSPAmount = value; }
        }

        protected double m_LSPSPAmountPaidByER;
        public double LSPSPAmountPaidByER
        {
            get { return m_LSPSPAmountPaidByER; }
            set { m_LSPSPAmountPaidByER = value; }
        }
    }

    public class GenericNewJoinEmployeeMPFFileDetail : GenericMPFFileDetail
    {

        public GenericNewJoinEmployeeMPFFileDetail(GenericMPFFile MPFFile, EEmpPersonalInfo empInfo)
            : base(MPFFile, empInfo)
        {
            // Current only support normal staff
            MemberType = MEMBER_TYPE_ENUM.NORMAL;

            if (empInfo != null)
            {
                EmpSurname = empInfo.EmpEngSurname.Replace("\r", " ").Replace("\n", " ").Trim().Replace("  ", " ");
                EmpOtherName = empInfo.EmpEngOtherName.Replace("\r", " ").Replace("\n", " ").Trim().Replace("  ", " ");
                Sex = empInfo.EmpGender.Trim();
                DateOfBirth = empInfo.EmpDateOfBirth;


                //DBFilter empMPFFilter = new DBFilter();
                //empMPFFilter.add(new Match("empid", empInfo.EmpID));
                //empMPFFilter.add("EmpMPFEffFr", true);
                //ArrayList empMPFs = EEmpMPFPlan.db.select(dbConn, empMPFFilter);
                //if (empMPFs.Count > 0)
                //{
                //    SchemeJoinDate = ((EEmpMPFPlan)empMPFs[0]).EmpMPFEffFr;
                //    m_EmpMPFPlanExtendXMLString = ((EEmpMPFPlan)empMPFs[0]).EmpMPFPlanExtendData;

                //}
                DateJoined = empInfo.EmpServiceDate;

                m_IsTransfer = false;
                if (empInfo.EmpDateOfJoin.Equals(SchemeJoinDate) && empInfo.EmpDateOfJoin > empInfo.EmpServiceDate)
                    m_IsTransfer = true;


                //DBFilter empTerminationFilter = new DBFilter();
                //empTerminationFilter.add(new Match("EmpID", empInfo.EmpID));
                //empTerminationFilter.add(new Match("EmpTermLastDate", "<=", mpfRecord.MPFRecPeriodTo));
                //empTerminationFilter.add(new Match("EmpTermLastDate", ">=", mpfRecord.MPFRecPeriodFr));
                //ArrayList empTerminations = EEmpTermination.db.select(dbConn, empTerminationFilter);
                //if (empTerminations.Count > 0)
                //{
                //    EEmpTermination empTermination = (EEmpTermination)empTerminations[0];

                //    DataSet.Payroll_MPFRemittanceStatement.AdditionalMemberRow additionalMPFRow = additionalMPF.NewAdditionalMemberRow();

                //    additionalMPFRow.EmpID = empInfo.EmpID;
                //    additionalMPFRow.EmpNo = empInfo.EmpNo;
                //    additionalMPFRow.HKID = empInfo.EmpHKID.Length < 7 ? empInfo.EmpPassportNo : empInfo.EmpHKID;
                //    additionalMPFRow.EmpName = empInfo.EmpEngFullName;
                //    additionalMPFRow.PeriodFrom = mpfRecord.MPFRecPeriodFr;
                //    additionalMPFRow.PeriodTo = empTermination.EmpTermLastDate;
                //    additionalMPFRow.MPFPlanID = mpfRecord.MPFPlanID;



                //    ECessationReason cessationReason = new ECessationReason();
                //    cessationReason.CessationReasonID = empTermination.CessationReasonID;
                //    ECessationReason.db.select(dbConn, cessationReason);

                //    additionalMPFRow.TermCode = cessationReason.CessationReasonCode;
                //    additionalMPF.Rows.Add(additionalMPFRow);
                //}
            }
        }

        //protected string m_ClassNo;
        //public string ClassNo
        //{
        //    get { return m_ClassNo; }
        //    set { m_ClassNo = value; }
        //}

        protected string m_EmpSurname;
        public string EmpSurname
        {
            get { return m_EmpSurname; }
            set { m_EmpSurname = value; }
        }

        protected string m_EmpOtherName;
        public string EmpOtherName
        {
            get { return m_EmpOtherName; }
            set { m_EmpOtherName = value; }
        }

        protected DateTime m_DateOfBirth;
        public DateTime DateOfBirth
        {
            get { return m_DateOfBirth; }
            set { m_DateOfBirth = value; }
        }

        protected string m_Sex;
        public string Sex
        {
            get { return m_Sex; }
            set { m_Sex = value; }
        }

        protected MEMBER_TYPE_ENUM m_MemberType;
        public MEMBER_TYPE_ENUM MemberType
        {
            get { return m_MemberType; }
            set { m_MemberType = value; }
        }

        protected DateTime m_DateJoined;
        public DateTime DateJoined
        {
            get { return m_DateJoined; }
            set { m_DateJoined = value; }
        }

        protected bool m_IsTransfer;
        public bool IsTransfer
        {
            get { return m_IsTransfer; }
            set { m_IsTransfer = value; }
        }

    }

    public class GenericAdditionalEmployeeMPFFileDetail : GenericExistingEmployeeMPFFileDetail
    {
        public GenericAdditionalEmployeeMPFFileDetail(GenericMPFFile MPFFile, EEmpPersonalInfo empInfo)
            : base(MPFFile, empInfo)
        {
        }

        public override void AddContributionDetail(EMPFRecord mpfRecord, bool AddIfPeriodDoesNotExists)
        {
            base.AddContributionDetail(mpfRecord, AddIfPeriodDoesNotExists);
            foreach (GenericMPFFileContributionDetail detail in this.MPFContributionDetailList)
            {
                detail.RelevantIncome = 0;
                detail.VCRelevantIncome = 0;
                detail.MCEE = 0;
                detail.MCER = 0;
                detail.VCEE = 0;
                detail.VCER = 0;
            }
        }
    }

    public class GenericBackPaymentEmployeeMPFFileDetail : GenericExistingEmployeeMPFFileDetail
    {
        public GenericBackPaymentEmployeeMPFFileDetail(GenericMPFFile MPFFile, EEmpPersonalInfo empInfo)
            : base(MPFFile, empInfo)
        {
        }

    }

    public class GenericMPFFileDetail
    {
        public GenericMPFFile MPFFile;
        public List<GenericMPFFileContributionDetail> MPFContributionDetailList = new List<GenericMPFFileContributionDetail>();
        protected DatabaseConnection dbConn;

        public GenericMPFFileDetail(GenericMPFFile mpfFile, EEmpPersonalInfo empInfo)
        {
            this.MPFFile = mpfFile;
            this.dbConn = mpfFile.dbConn;
            if (empInfo != null)
            {
                EmpID = empInfo.EmpID;
                EmpNo = empInfo.EmpNo.Trim();
                if (empInfo.EmpHKID.Length < 7)
                {
                    HKIDPassport = empInfo.EmpPassportNo.Trim();
                    HKIDType = IDENTITY_TYPE_ENUM.PASSPORT;
                }
                else
                {
                    HKIDPassport = empInfo.EmpHKID.Trim();
                    HKIDType = IDENTITY_TYPE_ENUM.HKID;
                }
                if (string.IsNullOrEmpty(HKIDPassport.Replace("()", "").Trim()))
                    throw new Exception(HROne.Common.WebUtility.GetLocalizedStringByCode("ERROR_HKID_PASSPORT_REQUIRED", "Either HKID or Passport Number is required") + "(" + HROne.Common.WebUtility.GetLocalizedString("EmpNo") + ": " + empInfo.EmpNo + ")");

                EmpName = empInfo.EmpEngFullName;

                DBFilter empMPFFilter = new DBFilter();
                empMPFFilter.add(new Match("empid", empInfo.EmpID));
                empMPFFilter.add("EmpMPFEffFr", true);
                ArrayList empMPFs = EEmpMPFPlan.db.select(dbConn, empMPFFilter);
                if (empMPFs.Count > 0)
                {
                    SchemeJoinDate = ((EEmpMPFPlan)empMPFs[0]).EmpMPFEffFr;
                    m_EmpMPFPlanExtendXMLString = ((EEmpMPFPlan)empMPFs[0]).EmpMPFPlanExtendData;
                }

                DBFilter empAVCFilter = new DBFilter();
                empAVCFilter.add(new Match("empid", empInfo.EmpID));
                empAVCFilter.add("EmpAVCEffFr", true);
                ArrayList empAVCs = EEmpAVCPlan.db.select(dbConn, empAVCFilter);
                if (empAVCs.Count > 0)
                {
                    m_EmpAVCPlanExtendXMLString = ((EEmpAVCPlan)empAVCs[0]).EmpAVCPlanExtendData;
                }
            }
            //GenericMPFFileContributionDetail contributionDetail = new GenericMPFFileContributionDetail();


            //contributionDetail.PeriodFrom = PeriodFrom;
            //contributionDetail.PeriodTo = PeriodTo;
            ////MPFPlanID = mpfRecord.MPFPlanID;
            //contributionDetail.RelevantIncome = 0;
            //contributionDetail.MCEE = 0;
            //contributionDetail.VCEE = 0;
            //contributionDetail.MCER = 0;
            //contributionDetail.VCER = 0;

        }

        public virtual void AddContributionDetail(EMPFRecord mpfRecord,bool AddIfPeriodDoesNotExists)
        {
            foreach (GenericMPFFileContributionDetail contribution in MPFContributionDetailList)
            {
                if (contribution.PeriodFrom.Equals(mpfRecord.MPFRecPeriodFr) && contribution.PeriodTo.Equals(mpfRecord.MPFRecPeriodTo))
                {
                    contribution.RelevantIncome += mpfRecord.MPFRecActMCRI;
                    contribution.VCRelevantIncome += mpfRecord.MPFRecActVCRI;
                    contribution.MCEE += mpfRecord.MPFRecActMCEE;
                    contribution.VCEE += mpfRecord.MPFRecActVCEE;
                    contribution.MCER += mpfRecord.MPFRecActMCER;
                    contribution.VCER += mpfRecord.MPFRecActVCER;
                    return;
                }
            }
            if (AddIfPeriodDoesNotExists)
            {
                GenericMPFFileContributionDetail contributionDetail = new GenericMPFFileContributionDetail();


                contributionDetail.PeriodFrom = mpfRecord.MPFRecPeriodFr;
                contributionDetail.PeriodTo = mpfRecord.MPFRecPeriodTo;
                //MPFPlanID = mpfRecord.MPFPlanID;
                contributionDetail.RelevantIncome = mpfRecord.MPFRecActMCRI;
                contributionDetail.VCRelevantIncome = mpfRecord.MPFRecActVCRI;
                contributionDetail.MCEE = mpfRecord.MPFRecActMCEE;
                contributionDetail.VCEE = mpfRecord.MPFRecActVCEE;
                contributionDetail.MCER = mpfRecord.MPFRecActMCER;
                contributionDetail.VCER = mpfRecord.MPFRecActVCER;
                MPFContributionDetailList.Add(contributionDetail);
            }
        }

        protected int m_EmpID;
        public int EmpID
        {
            get { return m_EmpID; }
            set { m_EmpID = value; }
        }

        protected string m_EmpNo;
        public string EmpNo
        {
            get { return m_EmpNo; }
            set { m_EmpNo = value; }
        }

        protected string m_HKIDPassport;
        public string HKIDPassport
        {
            get { return m_HKIDPassport; }
            set { m_HKIDPassport = value; }
        }

        protected IDENTITY_TYPE_ENUM m_HKIDType;
        public IDENTITY_TYPE_ENUM HKIDType
        {
            get { return m_HKIDType; }
            set { m_HKIDType = value; }
        }

        protected string m_EmpName;
        public string EmpName
        {
            get { return m_EmpName; }
            set { m_EmpName = value; }
        }

        protected DateTime m_SchemeJoinDate;
        public DateTime SchemeJoinDate
        {
            get { return m_SchemeJoinDate; }
            set { m_SchemeJoinDate = value; }
        }

        protected string m_EmpMPFPlanExtendXMLString;
        public string EmpMPFPlanExtendXMLString
        {
            get { return m_EmpMPFPlanExtendXMLString; }
            set { m_EmpMPFPlanExtendXMLString = value; }
        }

        protected string m_EmpAVCPlanExtendXMLString;
        public string EmpAVCPlanExtendXMLString
        {
            get { return m_EmpAVCPlanExtendXMLString; }
        }
    }

    public class GenericMPFFileContributionDetail
    {
        protected DateTime m_PeriodFrom;
        public DateTime PeriodFrom
        {
            get { return m_PeriodFrom; }
            set { m_PeriodFrom = value; }
        }

        protected DateTime m_PeriodTo;
        public DateTime PeriodTo
        {
            get { return m_PeriodTo; }
            set { m_PeriodTo = value; }
        }

        protected double m_RelevantIncome;
        public double RelevantIncome
        {
            get { return m_RelevantIncome; }
            set { m_RelevantIncome = value; }
        }

        protected double m_VCRelevantIncome;
        public double VCRelevantIncome
        {
            get { return m_VCRelevantIncome; }
            set { m_VCRelevantIncome = value; }
        }
        
        protected double m_MCER;
        public double MCER
        {
            get { return m_MCER; }
            set { m_MCER = value; }
        }

        protected double m_MCEE;
        public double MCEE
        {
            get { return m_MCEE; }
            set { m_MCEE = value; }
        }

        protected double m_VCER;
        public double VCER
        {
            get { return m_VCER; }
            set { m_VCER = value; }
        }

        protected double m_VCEE;
        public double VCEE
        {
            get { return m_VCEE; }
            set { m_VCEE = value; }
        }
    }
}
