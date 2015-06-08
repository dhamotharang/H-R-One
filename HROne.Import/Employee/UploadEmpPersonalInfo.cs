using System;
using System.Collections;
using System.Data;
using System.Globalization;
using HROne.DataAccess;
//using perspectivemind.validation;
using HROne.Common;
using HROne.Lib.Entities;
using HROne.Import;

namespace HROne.Import
{
    [DBClass("UploadEmpPersonalInfo")]
    public class EUploadEmpPersonalInfo : ImportDBObject
    {
        public static DBManager db = new DBManager(typeof(EUploadEmpPersonalInfo));
        //public static WFValueList VLEmp = new WFDBList(EEmpPersonalInfo.db, "EmpID", "EmpNo", "EmpNo");
        //public static WFValueList VLEmpStatus = new AppUtils.NewWFTextList(new string[] { "A", "T" }, new string[] { "Active", "Terminated" });

        protected int m_UploadEmpID;
        [DBField("UploadEmpID", true, true), TextSearch, Export(false)]
        public int UploadEmpID
        {
            get { return m_UploadEmpID; }
            set { m_UploadEmpID = value; modify("UploadEmpID"); }
        }
        protected int m_EmpID;
        [DBField("EmpID"), TextSearch, Export(false)]
        public int EmpID
        {
            get { return m_EmpID; }
            set { m_EmpID = value; modify("EmpID"); }
        }
        protected string m_EmpNo;
        [DBField("EmpNo"), TextSearch, MaxLength(20, 10), Export(false), Required]
        public string EmpNo
        {
            get { return m_EmpNo; }
            set { m_EmpNo = value; modify("EmpNo"); }
        }
        protected string m_EmpEngSurname;
        [DBField("EmpEngSurname"), TextSearch, MaxLength(20, 25), Export(false), Required]
        public string EmpEngSurname
        {
            get { return m_EmpEngSurname; }
            set { m_EmpEngSurname = value; modify("EmpEngSurname"); }
        }
        protected string m_EmpEngOtherName;
        [DBField("EmpEngOtherName"), TextSearch, MaxLength(55, 25), Export(false)]
        public string EmpEngOtherName
        {
            get { return m_EmpEngOtherName; }
            set { m_EmpEngOtherName = value; modify("EmpEngOtherName"); }
        }
        protected string m_EmpChiFullName;
        [DBField("EmpChiFullName"), TextSearch, MaxLength(50, 25), Export(false)]
        public string EmpChiFullName
        {
            get { return m_EmpChiFullName; }
            set { m_EmpChiFullName = value; modify("EmpChiFullName"); }
        }
        protected string m_EmpAlias;
        [DBField("EmpAlias"), TextSearch, MaxLength(100, 25), Export(false)]
        public string EmpAlias
        {
            get { return m_EmpAlias; }
            set { m_EmpAlias = value; modify("EmpAlias"); }
        }
        protected string m_EmpHKID;
        [DBField("EmpHKID"), DBAESEncryptStringField, TextSearch, MaxLength(12, 25), Export(false)]
        public string EmpHKID
        {
            get { return m_EmpHKID; }
            set { m_EmpHKID = value; modify("EmpHKID"); }
        }


        protected string m_EmpGender;
        [DBField("EmpGender"), TextSearch, Export(false), Required]
        public string EmpGender
        {
            get { return m_EmpGender; }
            set { m_EmpGender = value; modify("EmpGender"); }
        }
        protected string m_EmpMaritalStatus;
        [DBField("EmpMaritalStatus"), TextSearch, Export(false), Required]
        public string EmpMaritalStatus
        {
            get { return m_EmpMaritalStatus; }
            set { m_EmpMaritalStatus = value; modify("EmpMaritalStatus"); }
        }
        protected DateTime m_EmpDateOfBirth;
        [DBField("EmpDateOfBirth"), TextSearch, MaxLength(25), Export(false), Required]
        public DateTime EmpDateOfBirth
        {
            get { return m_EmpDateOfBirth; }
            set { m_EmpDateOfBirth = value; modify("EmpDateOfBirth"); }
        }
        protected string m_EmpPlaceOfBirth;
        [DBField("EmpPlaceOfBirth"), TextSearch, MaxLength(100, 25), Export(false)]
        public string EmpPlaceOfBirth
        {
            get { return m_EmpPlaceOfBirth; }
            set { m_EmpPlaceOfBirth = value; modify("EmpPlaceOfBirth"); }
        }
        protected string m_EmpNationality;
        [DBField("EmpNationality"), TextSearch, MaxLength(100, 25), Export(false)]
        public string EmpNationality
        {
            get { return m_EmpNationality; }
            set { m_EmpNationality = value; modify("EmpNationality"); }
        }
        protected string m_EmpPassportNo;
        [DBField("EmpPassportNo"), DBAESEncryptStringField(), TextSearch, MaxLength(40, 25), Export(false)]
        public string EmpPassportNo
        {
            get { return m_EmpPassportNo; }
            set { m_EmpPassportNo = value; modify("EmpPassportNo"); }
        }
        protected string m_EmpPassportIssuedCountry;
        [DBField("EmpPassportIssuedCountry"), TextSearch, MaxLength(40, 25), Export(false)]
        public string EmpPassportIssuedCountry
        {
            get { return m_EmpPassportIssuedCountry; }
            set { m_EmpPassportIssuedCountry = value; modify("EmpPassportIssuedCountry"); }
        }
        protected DateTime m_EmpPassportExpiryDate;
        [DBField("EmpPassportExpiryDate"), TextSearch, MaxLength(25), Export(false)]
        public DateTime EmpPassportExpiryDate
        {
            get { return m_EmpPassportExpiryDate; }
            set { m_EmpPassportExpiryDate = value; modify("EmpPassportExpiryDate"); }
        }
        protected string m_EmpResAddr;
        [DBField("EmpResAddr"), DBAESEncryptStringField, TextSearch, Export(false), MaxLength(90), Required]
        public string EmpResAddr
        {
            get { return m_EmpResAddr; }
            set { m_EmpResAddr = value; modify("EmpResAddr"); }
        }
        protected string m_EmpResAddrAreaCode;
        [DBField("EmpResAddrAreaCode"), TextSearch, Export(false), Required]
        public string EmpResAddrAreaCode
        {
            get { return m_EmpResAddrAreaCode; }
            set { m_EmpResAddrAreaCode = value; modify("EmpResAddrAreaCode"); }
        }
        protected string m_EmpCorAddr;
        [DBField("EmpCorAddr"), DBAESEncryptStringField, TextSearch, Export(false), MaxLength(60)]
        public string EmpCorAddr
        {
            get { return m_EmpCorAddr; }
            set { m_EmpCorAddr = value; modify("EmpCorAddr"); }
        }
        protected DateTime m_EmpNextSalaryIncrementDate;
        [DBField("EmpNextSalaryIncrementDate"), TextSearch, MaxLength(25), Export(false)]
        public DateTime EmpNextSalaryIncrementDate
        {
            get { return m_EmpNextSalaryIncrementDate; }
            set { m_EmpNextSalaryIncrementDate = value; modify("EmpNextSalaryIncrementDate"); }
        }
        protected DateTime m_EmpDateOfJoin;
        [DBField("EmpDateOfJoin"), TextSearch, MaxLength(25), Export(false), Required]
        public DateTime EmpDateOfJoin
        {
            get { return m_EmpDateOfJoin; }
            set { m_EmpDateOfJoin = value; modify("EmpDateOfJoin"); }
        }
        protected DateTime m_EmpServiceDate;
        [DBField("EmpServiceDate"), TextSearch, MaxLength(25), Export(false), Required]
        public DateTime EmpServiceDate
        {
            get { return m_EmpServiceDate; }
            set { m_EmpServiceDate = value; modify("EmpServiceDate"); }
        }
        protected int m_EmpNoticePeriod;
        [DBField("EmpNoticePeriod"), TextSearch, MaxLength(25), Export(false)]
        public int EmpNoticePeriod
        {
            get { return m_EmpNoticePeriod; }
            set { m_EmpNoticePeriod = value; modify("EmpNoticePeriod"); }
        }
        protected string m_EmpNoticeUnit;
        [DBField("EmpNoticeUnit"), TextSearch, Export(false)]
        public string EmpNoticeUnit
        {
            get { return m_EmpNoticeUnit; }
            set { m_EmpNoticeUnit = value; modify("EmpNoticeUnit"); }
        }
        protected int m_EmpProbaPeriod;
        [DBField("EmpProbaPeriod"), TextSearch, MaxLength(25), Export(false)]
        public int EmpProbaPeriod
        {
            get { return m_EmpProbaPeriod; }
            set { m_EmpProbaPeriod = value; modify("EmpProbaPeriod"); }
        }
        protected string m_EmpProbaUnit;
        [DBField("EmpProbaUnit"), TextSearch, Export(false)]
        public string EmpProbaUnit
        {
            get { return m_EmpProbaUnit; }
            set { m_EmpProbaUnit = value; modify("EmpProbaUnit"); }
        }

        protected DateTime m_EmpProbaLastDate;
        [DBField("EmpProbaLastDate"), TextSearch, MaxLength(25), Export(false)]
        public DateTime EmpProbaLastDate
        {
            get { return m_EmpProbaLastDate; }
            set { m_EmpProbaLastDate = value; modify("EmpProbaLastDate"); }
        }
        protected string m_EmpEmail;
        [DBField("EmpEmail"), TextSearch, MaxLength(100, 25), Export(false)]
        public string EmpEmail
        {
            get { return m_EmpEmail; }
            set { m_EmpEmail = value; modify("EmpEmail"); }
        }
        protected string m_EmpInternalEmail;
        [DBField("EmpInternalEmail"), TextSearch, MaxLength(100, 25), Export(false)]
        public string EmpInternalEmail
        {
            get { return m_EmpInternalEmail; }
            set { m_EmpInternalEmail = value; modify("EmpInternalEmail"); }
        }
        protected string m_EmpHomePhoneNo;
        [DBField("EmpHomePhoneNo"), TextSearch, MaxLength(100, 25), Export(false)]
        public string EmpHomePhoneNo
        {
            get { return m_EmpHomePhoneNo; }
            set { m_EmpHomePhoneNo = value; modify("EmpHomePhoneNo"); }
        }
        protected string m_EmpMobileNo;
        [DBField("EmpMobileNo"), TextSearch, MaxLength(100, 25), Export(false)]
        public string EmpMobileNo
        {
            get { return m_EmpMobileNo; }
            set { m_EmpMobileNo = value; modify("EmpMobileNo"); }
        }
        protected string m_EmpOfficePhoneNo;
        [DBField("EmpOfficePhoneNo"), TextSearch, MaxLength(100, 25), Export(false)]
        public string EmpOfficePhoneNo
        {
            get { return m_EmpOfficePhoneNo; }
            set { m_EmpOfficePhoneNo = value; modify("EmpOfficePhoneNo"); }
        }
        protected string m_EmpTimeCardNo;
        [DBField("EmpTimeCardNo"), TextSearch, MaxLength(20, 20), Export(false)]
        public string EmpTimeCardNo
        {
            get { return m_EmpTimeCardNo; }
            set { m_EmpTimeCardNo = value; modify("EmpTimeCardNo"); }
        }
        protected string m_Remark;
        [DBField("Remark"), TextSearch, Export(false)]
        public string Remark
        {
            get { return m_Remark; }
            set { m_Remark = value; modify("Remark"); }
        }
        public string EmpEngFullName
        {
            get { string empFullName = m_EmpEngSurname + " " + m_EmpEngOtherName; return empFullName.Trim().Replace("  ", " "); }
        }
        //  For Synchronize Use only
        protected string m_SynID;
        [DBField("SynID"), TextSearch, Export(false)]
        public string SynID
        {
            get { return m_SynID; }
            set { m_SynID = value; modify("SynID"); }
        }

        // Start 0000067, Miranda, 2014-08-07
        protected DateTime m_EmpOriginalHireDate;
        [DBField("EmpOriginalHireDate"), TextSearch, MaxLength(25), Export(false)]
        public DateTime EmpOriginalHireDate
        {
            get { return m_EmpOriginalHireDate; }
            set { m_EmpOriginalHireDate = value; modify("EmpOriginalHireDate"); }
        }
        // End 0000067, Miranda, 2014-08-07

        // Start 0000092, Ricky So, 2014-09-28
        protected int m_EmpPlaceOfBirthID;
        [DBField("EmpPlaceOfBirthID"), TextSearch, Export(false)]
        public int EmpPlaceOfBirthID
        {
            get { return m_EmpPlaceOfBirthID; }
            set { m_EmpPlaceOfBirthID = value; modify("EmpPlaceOfBirthID"); }
        }

        protected int m_EmpPassportIssuedCountryID;
        [DBField("EmpPassportIssuedCountryID"), TextSearch, Export(false)]
        public int EmpPassportIssuedCountryID
        {
            get { return m_EmpPassportIssuedCountryID; }
            set { m_EmpPassportIssuedCountryID = value; modify("EmpPassportIssuedCountryID"); }
        }

        protected int m_EmpNationalityID;
        [DBField("EmpNationalityID"), TextSearch, Export(false)]
        public int EmpNationalityID
        {
            get { return m_EmpNationalityID; }
            set { m_EmpNationalityID = value; modify("EmpNationalityID"); }
        }
        // End 0000092, Ricky So, 2014-09-28
    }

    [DBClass("UploadEmpExtraFieldValue")]
    public class EUploadEmpExtraFieldValue : BaseObject
    {
        public static DBManager db = new DBManager(typeof(EUploadEmpExtraFieldValue));

        protected int m_UploadEmpExtraFieldValueID;
        [DBField("UploadEmpExtraFieldValueID", true, true), TextSearch, Export(false)]
        public int UploadEmpExtraFieldValueID
        {
            get { return m_UploadEmpExtraFieldValueID; }
            set { m_UploadEmpExtraFieldValueID = value; modify("UploadEmpExtraFieldValueID"); }
        }

        protected int m_EmpExtraFieldValueID;
        [DBField("EmpExtraFieldValueID"), TextSearch, Export(false)]
        public int EmpExtraFieldValueID
        {
            get { return m_EmpExtraFieldValueID; }
            set { m_EmpExtraFieldValueID = value; modify("EmpExtraFieldValueID"); }
        }

        protected int m_UploadEmpID;
        [DBField("UploadEmpID"), TextSearch, Export(false), Required]
        public int UploadEmpID
        {
            get { return m_UploadEmpID; }
            set { m_UploadEmpID = value; modify("UploadEmpID"); }
        }

        protected int m_EmpID;
        [DBField("EmpID"), TextSearch, Export(false), Required]
        public int EmpID
        {
            get { return m_EmpID; }
            set { m_EmpID = value; modify("EmpID"); }
        }

        protected int m_EmpExtraFieldID;
        [DBField("EmpExtraFieldID"), TextSearch, Export(false), Required]
        public int EmpExtraFieldID
        {
            get { return m_EmpExtraFieldID; }
            set { m_EmpExtraFieldID = value; modify("EmpExtraFieldID"); }
        }

        protected string m_EmpExtraFieldValue;
        [DBField("EmpExtraFieldValue"), TextSearch, MaxLength(50), Export(false), Required]
        public string EmpExtraFieldValue
        {
            get { return m_EmpExtraFieldValue; }
            set { m_EmpExtraFieldValue = value; modify("EmpExtraFieldValue"); }
        }
    }

    //[DBClass("UploadEmpUniform")]
    //public class EUploadEmpUniform : BaseObject
    //{
    //    public static DBManager db = new DBManager(typeof(EUploadEmpUniform));

    //    protected int m_UploadEmpUniformID;
    //    [DBField("UploadEmpUniformID", true, true), TextSearch, Export(false)]
    //    public int UploadEmpUniformID
    //    {
    //        get { return m_UploadEmpUniformID; }
    //        set { m_UploadEmpUniformID = value; modify("UploadEmpUniformID"); }
    //    }

    //    protected int m_EmpUniformID;
    //    [DBField("EmpUniformID"), TextSearch, Export(false)]
    //    public int EmpUniformID
    //    {
    //        get { return m_EmpUniformID; }
    //        set { m_EmpUniformID = value; modify("EmpUniformID"); }
    //    }

    //    protected int m_UploadEmpID;
    //    [DBField("UploadEmpID"), TextSearch, Export(false), Required]
    //    public int UploadEmpID
    //    {
    //        get { return m_UploadEmpID; }
    //        set { m_UploadEmpID = value; modify("UploadEmpID"); }
    //    }

    //    protected int m_EmpID;
    //    [DBField("EmpID"), TextSearch, Export(false), Required]
    //    public int EmpID
    //    {
    //        get { return m_EmpID; }
    //        set { m_EmpID = value; modify("EmpID"); }
    //    }

    //    protected string m_EmpUniformB;
    //    [DBField("EmpUniformB"), TextSearch, MaxLength(10), Export(false)]
    //    public string EmpUniformB
    //    {
    //        get { return m_EmpUniformB; }
    //        set { m_EmpUniformB = value; modify("EmpUniformB"); }
    //    }
    //    protected string m_EmpUniformW;
    //    [DBField("EmpUniformW"), TextSearch, MaxLength(10), Export(false)]
    //    public string EmpUniformW
    //    {
    //        get { return m_EmpUniformW; }
    //        set { m_EmpUniformW = value; modify("EmpUniformW"); }
    //    }
    //    protected string m_EmpUniformH;
    //    [DBField("EmpUniformH"), TextSearch, MaxLength(10), Export(false)]
    //    public string EmpUniformH
    //    {
    //        get { return m_EmpUniformH; }
    //        set { m_EmpUniformH = value; modify("EmpUniformH"); }
    //    }
    //}

    /// <summary>
    /// Summary description for ImportEmpPersonalInfoProcess
    /// </summary>
    public class ImportEmpPersonalInfoProcess : ImportProcessInteface
    {
        public const string TABLE_NAME = "personal_information";


        //  Field Const for Employee Personal Information
        private const string FIELD_EMP_NO = "Emp No";
        private const string FIELD_SURNAME = "Surname";
        private const string FIELD_OTHER_NAME = "Other Name";
        private const string FIELD_ALIAS = "Alias";
        private const string FIELD_CHINESE_NAME = "Chinese Name";
        private const string FIELD_GENDER = "Gender";
        private const string FIELD_MARITAL_STATUS = "Marital Status";
        private const string FIELD_DATE_OF_BIRTH = "Date of Birth";
        private const string FIELD_HKID = "HKID";
        private const string FIELD_PLACE_OF_BIRTH = "Place Of Birth";
        private const string FIELD_NATIONALITY = "Nationality";
        private const string FIELD_PASSPORT_NUMBER = "Passport";
        private const string FIELD_PASSPORT_ISSUE_COUNTRY = "Passport Country";
        private const string FIELD_PASSPORT_EXPIRY_DATE = "Passport Expiry Date";
        private const string FIELD_HOME_PHONE_NUMBER = "Home";
        private const string FIELD_MOBILE_PHONE_NUMBER = "Mobile";
        private const string FIELD_OFFICE_PHONE_NUMBER = "Office";
        private const string FIELD_EMAIL_ADDRESS = "Email";
        private const string FIELD_PERSONAL_EMAIL_ADDRESS = "Personal Email";
        private const string FIELD_COMPANY_EMAIL_ADDRESS = "Company Email";
        private const string FIELD_RESIDENTIAL_ADDRESS = "Residential Address";
        private const string FIELD_RESIDENTIAL_ADDRESS_AREA_CODE = "Residential Address Code";
        private const string FIELD_CORRESPONDENCE_ADDRESS = "Correspondence";
        private const string FIELD_DATE_OF_JOIN = "Join Date";
        private const string FIELD_NEXT_SALARY_INCREMENT_DATE = "Next Salary Increment Date";
        private const string FIELD_SERVICE_START_DATE = "Service Start Date";
        private const string FIELD_PROBATION_PERIOD = "Probation Period";
        private const string FIELD_PROBATION_PERIOD_UNIT = "Probation Period Unit";
        private const string FIELD_PROBATION_LAST_DATE = "Probation Last Date";
        private const string FIELD_NOTICE_PERIOD = "Notice Period";
        private const string FIELD_NOTICE_PERIOD_UNIT = "Notice Period Unit";
        private const string FIELD_TIMECARDNO = "Time Card Number";
        private const string FIELD_REMARK = "Remark";
        // Start 0000067, Miranda, 2014-08-07
        private const string FIELD_ORIGINAL_HIRE_DATE = "Original Hire Date";
        // End 0000067, Miranda, 2014-08-07

        //private const string FIELD_UNIFORM_B = "Uniform Size B";
        //private const string FIELD_UNIFORM_W = "Uniform Size W";
        //private const string FIELD_UNIFORM_H = "Uniform Size H";


        private DateTime UploadDateTime = AppUtils.ServerDateTime();
        private DBManager db = EUploadEmpPersonalInfo.db;
        private DBManager tempEmpExtraFieldValueDB = EUploadEmpExtraFieldValue.db;
        private DBManager uploadEmpExtraFieldValueDB = EEmpExtraFieldValue.db;

        public ImportErrorList errors = new ImportErrorList();

        protected ImportEmpPositionInfoProcess importEmpPos;
        protected ImportEmpBankAccountProcess importEmpBankAcc;
        protected ImportEmpBeneficiariesProcess importEmpBeneficiaries;
        protected ImportEmpBenefitProcess importEmpBenefit;
        protected ImportEmpMPFPlanProcess importEmpMPF;
        protected ImportEmpAVCPlanProcess importEmpAVC;
        protected ImportEmpORSOPlanProcess importEmpORSO;
        protected ImportEmpSpouseProcess importEmpSpouse;
        protected ImportEmpDependantProcess importEmpDependant;
        protected ImportEmpQualificationProcess importEmpQualification;
        protected ImportEmpSkillProcess importEmpSkill;
        protected ImportEmpPlaceOfResidenceProcess importEmpPlaceOfResidence;
        protected ImportEmpContractTermsProcess importEmpContractTerms;
        protected ImportEmpTerminationProcess importEmpTermination;
        protected ImportLeaveApplicationProcess importLeaveApplication;
        protected ImportEmpCostCenterProcess importEmpCostCenter;
        protected ImportEmpPermitProcess importEmpPermit;
        protected ImportEmpEmergencyContactProcess importEmpEmergencyContact;
        protected ImportEmpWorkExpProcess importEmpWorkExp;
        protected ImportEmpWorkInjuryRecordProcess importEmpWorkInjuryRecord;
        protected ImportEmpRecurringPaymentProcess importEmpPay;
        protected ImportEmpFinalPaymentProcess importEmpFinalPayment;
        protected ImportEmpRosterTableGroupProcess importEmpRosterTableGroup;
        protected ImportLeaveBalanceAdjustmentProcess importLeaveBalanceAdjustment;
        protected ImportCompensationLeaveEntitleProcess importCompensationLeaveEntitle;
        // Start 0000192, Miranda, 2015-05-08
        protected ImportEmpCostCenterExportProcess importEmpCostCenterExport;
        // End 0000192, Miranda, 2015-05-08

        public ImportEmpPersonalInfoProcess(DatabaseConnection dbConn, string SessionID)
            : base(dbConn, SessionID)
        {

            importEmpPos = new ImportEmpPositionInfoProcess(dbConn, m_SessionID);
            importEmpBankAcc = new ImportEmpBankAccountProcess(dbConn, m_SessionID);
            importEmpBenefit = new ImportEmpBenefitProcess(dbConn, m_SessionID);
            importEmpBeneficiaries = new ImportEmpBeneficiariesProcess(dbConn, m_SessionID);
            importEmpMPF = new ImportEmpMPFPlanProcess(dbConn, m_SessionID);
            importEmpAVC = new ImportEmpAVCPlanProcess(dbConn, m_SessionID);
            importEmpORSO = new ImportEmpORSOPlanProcess(dbConn, m_SessionID);
            importEmpSpouse = new ImportEmpSpouseProcess(dbConn, m_SessionID);
            importEmpDependant = new ImportEmpDependantProcess(dbConn, m_SessionID);
            importEmpQualification = new ImportEmpQualificationProcess(dbConn, m_SessionID);
            importEmpSkill = new ImportEmpSkillProcess(dbConn, m_SessionID);
            importEmpPlaceOfResidence = new ImportEmpPlaceOfResidenceProcess(dbConn, m_SessionID);
            importEmpContractTerms = new ImportEmpContractTermsProcess(dbConn, m_SessionID);
            importEmpTermination = new ImportEmpTerminationProcess(dbConn, m_SessionID);
            importLeaveApplication = new ImportLeaveApplicationProcess(dbConn, m_SessionID);
            importEmpCostCenter = new ImportEmpCostCenterProcess(dbConn, m_SessionID);
            importEmpPermit = new ImportEmpPermitProcess(dbConn, m_SessionID);
            importEmpEmergencyContact = new ImportEmpEmergencyContactProcess(dbConn, m_SessionID);
            importEmpWorkExp = new ImportEmpWorkExpProcess(dbConn, m_SessionID);
            importEmpWorkInjuryRecord = new ImportEmpWorkInjuryRecordProcess(dbConn, m_SessionID);
            importEmpRosterTableGroup = new ImportEmpRosterTableGroupProcess(dbConn, m_SessionID);
            importLeaveBalanceAdjustment = new ImportLeaveBalanceAdjustmentProcess(dbConn, m_SessionID);
            importCompensationLeaveEntitle = new ImportCompensationLeaveEntitleProcess(dbConn, m_SessionID);

            importEmpPay = new ImportEmpRecurringPaymentProcess(dbConn, m_SessionID);
            importEmpFinalPayment = new ImportEmpFinalPaymentProcess(dbConn, m_SessionID);

            // Start 0000192, Miranda, 2015-05-08
            importEmpCostCenterExport = new ImportEmpCostCenterExportProcess(dbConn, m_SessionID);
            // End 0000192, Miranda, 2015-05-08

        }


        public static int CreateDummyUploadEmployeeInfo(DatabaseConnection dbConn, int EmpID, string sessionID, DateTime transactionDate)
        {
            EEmpPersonalInfo empInfo = new EEmpPersonalInfo();
            empInfo.EmpID = EmpID;
            if (EEmpPersonalInfo.db.select(dbConn, empInfo))
            {
                EUploadEmpPersonalInfo uploadEmp = new EUploadEmpPersonalInfo();
                uploadEmp.ImportFromObject(empInfo);
                uploadEmp.SessionID = sessionID;
                uploadEmp.TransactionDate = transactionDate;
                uploadEmp.ImportActionStatus = ImportDBObject.ImportActionEnum.NONE;
                EUploadEmpPersonalInfo.db.insert(dbConn, uploadEmp);
                return uploadEmp.UploadEmpID;
            }
            return 0;

        }
        public DataTable UploadToTempDatabase(DataTable rawDataTable, int UserID, bool CreateCodeIfNotExists)
        {
            if (rawDataTable != null)
            {
                ArrayList results = new ArrayList();
                int rowCount = 1;
                foreach (DataRow row in rawDataTable.Rows)
                {
                    rowCount++;

                    EUploadEmpPersonalInfo empInfo = new EUploadEmpPersonalInfo();
                    //EUploadEmpUniform empUniform = null;
                    ArrayList uploadEmpExtraFieldValueList = new ArrayList();

                    empInfo.EmpNo = row[FIELD_EMP_NO].ToString();
                    //if (rawDataTable.Columns.Contains(FIELD_INTERNAL_ID))
                    //{
                    //    try
                    //    {
                    //        if (!row.IsNull(FIELD_INTERNAL_ID))
                    //        {
                    //            int tmpID = FromHexDecWithCheckDigit((string)row[FIELD_INTERNAL_ID]);
                    //            EEmpPersonalInfo tmpEmpInfo = new EEmpPersonalInfo();
                    //            tmpEmpInfo.EmpID = tmpID;
                    //            if (EEmpPersonalInfo.db.select(dbConn, tmpEmpInfo))
                    //                empInfo.EmpID = HROne.Import.Parse.GetEmpID(dbConn, tmpEmpInfo.EmpNo, UserID);
                    //            else
                    //            {
                    //                errors.addError(ImportErrorMessage.ERROR_INVALID_FIELD_VALUE, new string[] { FIELD_INTERNAL_ID + "=" + row[FIELD_INTERNAL_ID].ToString(), empInfo.EmpNo, rowCount.ToString() });
                    //                continue;
                    //            }
                    //            if (empInfo.EmpID < 0)
                    //            {
                    //                errors.addError(ImportErrorMessage.ERROR_ACCESS_DENIED_EMP_NO, new string[] { empInfo.EmpNo, rowCount.ToString() });
                    //                continue;
                    //            }
                    //        }
                    //    }
                    //    catch (Exception ex)
                    //    {
                    //        errors.addError(ImportErrorMessage.ERROR_INVALID_FIELD_VALUE, new string[] { FIELD_INTERNAL_ID + "=" + row[FIELD_INTERNAL_ID].ToString(), empInfo.EmpNo, rowCount.ToString() });
                    //        continue;
                    //    }
                    //}
                    if (rawDataTable.Columns.Contains(FIELD_SYNC_ID))
                    {
                        if (!row.IsNull(FIELD_SYNC_ID))
                        {
                            string strSynID = row[FIELD_SYNC_ID].ToString();
                            empInfo.SynID = strSynID;
                            if (!string.IsNullOrEmpty(strSynID))
                            {
                                DBFilter synIDFilter = new DBFilter();
                                synIDFilter.add(new Match("SynID", strSynID));
                                ArrayList objSameSynIDList = EEmpPersonalInfo.db.select(dbConn, synIDFilter);
                                if (objSameSynIDList.Count > 0)
                                    empInfo.EmpID = HROne.Import.Parse.GetEmpID(dbConn, ((EEmpPersonalInfo)objSameSynIDList[0]).EmpNo, UserID);

                                if (empInfo.EmpID < 0)
                                {
                                    errors.addError(ImportErrorMessage.ERROR_ACCESS_DENIED_EMP_NO, new string[] { empInfo.EmpNo, rowCount.ToString() });
                                    continue;
                                }
                            }
                        }

                    }
                    if (empInfo.EmpID == 0)
                        empInfo.EmpID = HROne.Import.Parse.GetEmpID(dbConn, empInfo.EmpNo, UserID);

                    if (empInfo.EmpID < 0)
                        errors.addError(ImportErrorMessage.ERROR_ACCESS_DENIED_EMP_NO, new string[] { empInfo.EmpNo, rowCount.ToString() });
                    if (ParseTemp.GetUploadEmpID(dbConn, empInfo.EmpNo, m_SessionID) > 0)
                        errors.addError(ImportErrorMessage.ERROR_DUPLICATE_EMP_NO, new string[] { empInfo.EmpNo, rowCount.ToString() });

                    empInfo.EmpEngSurname = row[FIELD_SURNAME].ToString();
                    empInfo.EmpEngOtherName = row[FIELD_OTHER_NAME].ToString();
                    empInfo.EmpAlias = row[FIELD_ALIAS].ToString();
                    empInfo.EmpChiFullName = row[FIELD_CHINESE_NAME].ToString();

                    string tempString = row[FIELD_GENDER].ToString();
                    if (tempString.Equals("Male", StringComparison.CurrentCultureIgnoreCase)
                        || tempString.Equals("M", StringComparison.CurrentCultureIgnoreCase))
                        empInfo.EmpGender = "M";
                    else if (tempString.Equals("Female", StringComparison.CurrentCultureIgnoreCase)
                        || tempString.Equals("F", StringComparison.CurrentCultureIgnoreCase))
                        empInfo.EmpGender = "F";
                    else
                        errors.addError(ImportErrorMessage.ERROR_INVALID_GENDER, new string[] { tempString, rowCount.ToString() });

                    tempString = row[FIELD_MARITAL_STATUS].ToString();
                    if (tempString.Equals("Single", StringComparison.CurrentCultureIgnoreCase))
                        empInfo.EmpMaritalStatus = "Single";
                    else if (tempString.Equals("Married", StringComparison.CurrentCultureIgnoreCase))
                        empInfo.EmpMaritalStatus = "Married";
                    else if (tempString.Equals("Divorced", StringComparison.CurrentCultureIgnoreCase))
                        empInfo.EmpMaritalStatus = "Divorced";
                    else if (tempString.Equals("Widowed", StringComparison.CurrentCultureIgnoreCase))
                        empInfo.EmpMaritalStatus = "Widowed";
                    else if (tempString.Equals("Separated", StringComparison.CurrentCultureIgnoreCase))
                        empInfo.EmpMaritalStatus = "Separated";
                    else if (tempString.Equals("Living Apart", StringComparison.CurrentCultureIgnoreCase))
                        empInfo.EmpMaritalStatus = "Separated";
                    else
                        errors.addError(ImportErrorMessage.ERROR_INVALID_MARITAL_STATUS, new string[] { tempString, rowCount.ToString() });
                    try
                    {
                        empInfo.EmpDateOfBirth = Parse.toDateTimeObject(row[FIELD_DATE_OF_BIRTH]);
                    }
                    catch
                    {
                        errors.addError(ImportErrorMessage.ERROR_INVALID_FIELD_VALUE, new string[] { FIELD_DATE_OF_BIRTH + "=" + row[FIELD_DATE_OF_BIRTH].ToString(), empInfo.EmpNo, rowCount.ToString() });
                    }
                    empInfo.EmpHKID = row[FIELD_HKID].ToString().Trim();

                    //empInfo.EmpPlaceOfBirth = row[FIELD_PLACE_OF_BIRTH].ToString();
                    //empInfo.EmpNationality = row[FIELD_NATIONALITY].ToString();

                    // Start 0000100, Ricky So, 2014/11/12
                    if (row[FIELD_NATIONALITY].ToString() != "")
                    // End 0000100, Ricky So, 2014/11/12
                    {
                        empInfo.EmpNationalityID = Parse.GetNationalityID(dbConn, row[FIELD_NATIONALITY].ToString(), CreateCodeIfNotExists, UserID);
                        if (empInfo.EmpNationalityID.Equals(0))
                        {
                            errors.addError(ImportErrorMessage.ERROR_INVALID_FIELD_VALUE, new string[] { FIELD_NATIONALITY + "=" + row[FIELD_NATIONALITY].ToString(), empInfo.EmpNo, rowCount.ToString() });
                            empInfo.EmpNationality = "";
                        }
                        else
                        {
                            ENationality m_nationality = ENationality.GetObject(dbConn, empInfo.EmpNationalityID);
                            empInfo.EmpNationality = m_nationality.NationalityDesc;
                        }
                    }
                    // Start 0000100, Ricky So, 2014/11/12
                    if (row[FIELD_PLACE_OF_BIRTH].ToString() != "")
                    // End 0000100, Ricky So, 2014/11/12
                    {
                        empInfo.EmpPlaceOfBirthID = Parse.GetPlaceOfBirthID(dbConn, row[FIELD_PLACE_OF_BIRTH].ToString(), CreateCodeIfNotExists, UserID);
                        if (empInfo.EmpPlaceOfBirthID.Equals(0))
                        {
                            errors.addError(ImportErrorMessage.ERROR_INVALID_FIELD_VALUE, new string[] { FIELD_PLACE_OF_BIRTH + "=" + row[FIELD_PLACE_OF_BIRTH].ToString(), empInfo.EmpNo, rowCount.ToString() });
                            empInfo.EmpPlaceOfBirth = "";
                        }
                        else
                        {
                            EPlaceOfBirth m_placeOfBirth = EPlaceOfBirth.GetObject(dbConn, empInfo.EmpPlaceOfBirthID);
                            empInfo.EmpPlaceOfBirth = m_placeOfBirth.PlaceOfBirthDesc;
                        }
                    }
                     

                    empInfo.EmpPassportNo = row[FIELD_PASSPORT_NUMBER].ToString();

                    
                    //empInfo.EmpPassportIssuedCountry = row[FIELD_PASSPORT_ISSUE_COUNTRY].ToString();
                    // Start 0000100, Ricky So, 2014/11/12
                    if (row[FIELD_PASSPORT_ISSUE_COUNTRY].ToString() != "")
                    // End 0000100, Ricky So, 2014/11/12
                    {
                        empInfo.EmpPassportIssuedCountryID = Parse.GetIssueCountryID(dbConn, row[FIELD_PASSPORT_ISSUE_COUNTRY].ToString(), CreateCodeIfNotExists, UserID);
                        if (empInfo.EmpPassportIssuedCountryID.Equals(0))
                        {
                            errors.addError(ImportErrorMessage.ERROR_INVALID_FIELD_VALUE, new string[] { FIELD_PASSPORT_ISSUE_COUNTRY + "=" + row[FIELD_PASSPORT_ISSUE_COUNTRY].ToString(), empInfo.EmpNo, rowCount.ToString() });
                            empInfo.EmpPassportIssuedCountry = "";
                        }
                        else
                        {
                            EIssueCountry m_issueCountry = EIssueCountry.GetObject(dbConn, empInfo.EmpPassportIssuedCountryID);
                            empInfo.EmpPassportIssuedCountry = m_issueCountry.CountryDesc;
                        }
                    }
                    
                    try
                    {
                        empInfo.EmpPassportExpiryDate = Parse.toDateTimeObject(row[FIELD_PASSPORT_EXPIRY_DATE]);
                    }
                    catch
                    {
                        errors.addError(ImportErrorMessage.ERROR_INVALID_FIELD_VALUE, new string[] { FIELD_PASSPORT_EXPIRY_DATE + "=" + row[FIELD_PASSPORT_EXPIRY_DATE].ToString(), empInfo.EmpNo, rowCount.ToString() });
                    }

                    empInfo.EmpHomePhoneNo = row[FIELD_HOME_PHONE_NUMBER].ToString();
                    empInfo.EmpMobileNo = row[FIELD_MOBILE_PHONE_NUMBER].ToString();
                    empInfo.EmpOfficePhoneNo = row[FIELD_OFFICE_PHONE_NUMBER].ToString();
                    if (rawDataTable.Columns.Contains(FIELD_EMAIL_ADDRESS))
                        empInfo.EmpEmail = row[FIELD_EMAIL_ADDRESS].ToString();
                    if (rawDataTable.Columns.Contains(FIELD_PERSONAL_EMAIL_ADDRESS))
                        empInfo.EmpEmail = row[FIELD_PERSONAL_EMAIL_ADDRESS].ToString();
                    if (rawDataTable.Columns.Contains(FIELD_COMPANY_EMAIL_ADDRESS))
                        empInfo.EmpInternalEmail = row[FIELD_COMPANY_EMAIL_ADDRESS].ToString();

                    empInfo.EmpResAddr = row[FIELD_RESIDENTIAL_ADDRESS].ToString();

                    tempString = row[FIELD_RESIDENTIAL_ADDRESS_AREA_CODE].ToString().Replace(" ", "");
                    if (tempString.Equals("HongKong", StringComparison.CurrentCultureIgnoreCase)
                        || tempString.Equals("HK", StringComparison.CurrentCultureIgnoreCase)
                        || tempString.Equals("H", StringComparison.CurrentCultureIgnoreCase))
                        empInfo.EmpResAddrAreaCode = "H";
                    else if (tempString.Equals("Kowloon", StringComparison.CurrentCultureIgnoreCase)
                        || tempString.Equals("K", StringComparison.CurrentCultureIgnoreCase))
                        empInfo.EmpResAddrAreaCode = "K";
                    else if (tempString.Equals("NewTerritories", StringComparison.CurrentCultureIgnoreCase)
                        || tempString.Equals("NT", StringComparison.CurrentCultureIgnoreCase)
                        || tempString.Equals("N", StringComparison.CurrentCultureIgnoreCase))
                        empInfo.EmpResAddrAreaCode = "N";
                    else if (tempString.Equals("Overseas", StringComparison.CurrentCultureIgnoreCase)
                        || tempString.Equals("O", StringComparison.CurrentCultureIgnoreCase))
                        empInfo.EmpResAddrAreaCode = "O";
                    else
                        errors.addError(ImportErrorMessage.ERROR_INVALID_AREA_CODE, new string[] { row[FIELD_RESIDENTIAL_ADDRESS_AREA_CODE].ToString(), rowCount.ToString() });

                    empInfo.EmpCorAddr = row[FIELD_CORRESPONDENCE_ADDRESS].ToString();

                    if (ESystemParameter.IsEnabled(dbConn, ESystemParameter.PARAM_CODE_PAYSCALE_POINT_SYSTEM))
                    {
                        try
                        {
                            empInfo.EmpNextSalaryIncrementDate = Parse.toDateTimeObject(row[FIELD_NEXT_SALARY_INCREMENT_DATE]);
                        }
                        catch
                        {
                            errors.addError(ImportErrorMessage.ERROR_INVALID_FIELD_VALUE, new string[] { FIELD_NEXT_SALARY_INCREMENT_DATE + "=" + row[FIELD_NEXT_SALARY_INCREMENT_DATE].ToString(), empInfo.EmpNo, rowCount.ToString() });
                        }
                    }

                    try
                    {
                        empInfo.EmpDateOfJoin = Parse.toDateTimeObject(row[FIELD_DATE_OF_JOIN]);
                    }
                    catch
                    {
                        errors.addError(ImportErrorMessage.ERROR_INVALID_FIELD_VALUE, new string[] { FIELD_DATE_OF_JOIN + "=" + row[FIELD_DATE_OF_JOIN].ToString(), empInfo.EmpNo, rowCount.ToString() });
                    }
                    try
                    {
                        empInfo.EmpServiceDate = Parse.toDateTimeObject(row[FIELD_SERVICE_START_DATE]);
                    }
                    catch
                    {
                        errors.addError(ImportErrorMessage.ERROR_INVALID_FIELD_VALUE, new string[] { FIELD_SERVICE_START_DATE + "=" + row[FIELD_SERVICE_START_DATE].ToString(), empInfo.EmpNo, rowCount.ToString() });
                    }

                    int tempInt = 0;
                    if (int.TryParse(row[FIELD_PROBATION_PERIOD].ToString(), out tempInt))
                        empInfo.EmpProbaPeriod = tempInt;

                    tempString = row[FIELD_PROBATION_PERIOD_UNIT].ToString().Replace(" ", "");
                    if (tempString.Equals("Days", StringComparison.CurrentCultureIgnoreCase)
                        || tempString.Equals("Day", StringComparison.CurrentCultureIgnoreCase)
                        || tempString.Equals("D", StringComparison.CurrentCultureIgnoreCase))
                        empInfo.EmpProbaUnit = "D";
                    else if (tempString.Equals("Months", StringComparison.CurrentCultureIgnoreCase)
                        || tempString.Equals("Month", StringComparison.CurrentCultureIgnoreCase)
                        || tempString.Equals("M", StringComparison.CurrentCultureIgnoreCase))
                        empInfo.EmpProbaUnit = "M";
                    else if (tempString.Equals(string.Empty))
                        empInfo.EmpProbaUnit = string.Empty;
                    else
                        errors.addError(ImportErrorMessage.ERROR_INVALID_FIELD_VALUE, new string[] { FIELD_PROBATION_PERIOD_UNIT + "=" + row[FIELD_PROBATION_PERIOD_UNIT].ToString(), empInfo.EmpNo, rowCount.ToString() });

                    try
                    {
                        empInfo.EmpProbaLastDate = Parse.toDateTimeObject(row[FIELD_PROBATION_LAST_DATE]);
                    }
                    catch
                    {
                        errors.addError(ImportErrorMessage.ERROR_INVALID_FIELD_VALUE, new string[] { FIELD_PROBATION_LAST_DATE + "=" + row[FIELD_PROBATION_LAST_DATE].ToString(), empInfo.EmpNo, rowCount.ToString() });
                    }

                    if (int.TryParse(row[FIELD_NOTICE_PERIOD].ToString(), out tempInt))
                        empInfo.EmpNoticePeriod = tempInt;

                    tempString = row[FIELD_NOTICE_PERIOD_UNIT].ToString().Replace(" ", "");
                    if (tempString.Equals("Days", StringComparison.CurrentCultureIgnoreCase)
                        || tempString.Equals("Day", StringComparison.CurrentCultureIgnoreCase)
                        || tempString.Equals("D", StringComparison.CurrentCultureIgnoreCase))
                        empInfo.EmpNoticeUnit = "D";
                    else if (tempString.Equals("Months", StringComparison.CurrentCultureIgnoreCase)
                        || tempString.Equals("Month", StringComparison.CurrentCultureIgnoreCase)
                        || tempString.Equals("M", StringComparison.CurrentCultureIgnoreCase))
                        empInfo.EmpNoticeUnit = "M";
                    else if (tempString.Equals(string.Empty))
                        empInfo.EmpNoticeUnit = string.Empty;
                    else
                        errors.addError(ImportErrorMessage.ERROR_INVALID_FIELD_VALUE, new string[] { FIELD_NOTICE_PERIOD_UNIT + "=" + row[FIELD_NOTICE_PERIOD_UNIT].ToString(), empInfo.EmpNo, rowCount.ToString() });


                    empInfo.Remark = row[FIELD_REMARK].ToString();

                    if (rawDataTable.Columns.Contains(FIELD_TIMECARDNO))
                        empInfo.EmpTimeCardNo = row[FIELD_TIMECARDNO].ToString();
                    // Start 0000067, Miranda, 2014-08-07
                    if (rawDataTable.Columns.Contains(FIELD_ORIGINAL_HIRE_DATE))
                    {
                        try
                        {
                            empInfo.EmpOriginalHireDate = Parse.toDateTimeObject(row[FIELD_ORIGINAL_HIRE_DATE]);
                        }
                        catch
                        {
                            errors.addError(ImportErrorMessage.ERROR_INVALID_FIELD_VALUE, new string[] { FIELD_ORIGINAL_HIRE_DATE + "=" + row[FIELD_ORIGINAL_HIRE_DATE].ToString(), empInfo.EmpNo, rowCount.ToString() });
                        }
                    }
                    // End 0000067, Miranda, 2014-08-07

                    //if (rawDataTable.Columns.Contains(FIELD_UNIFORM_B) || rawDataTable.Columns.Contains(FIELD_UNIFORM_B) || rawDataTable.Columns.Contains(FIELD_UNIFORM_B))
                    //{
                    //    empUniform = new EUploadEmpUniform();
                    //    if (empInfo.EmpID > 0)
                    //        empUniform.EmpID = empInfo.EmpID;
                    //    empUniform.EmpUniformB = row[FIELD_UNIFORM_B].ToString();
                    //    empUniform.EmpUniformW = row[FIELD_UNIFORM_W].ToString();
                    //    empUniform.EmpUniformH = row[FIELD_UNIFORM_H].ToString();
                    //}
                    ArrayList empExtraFieldList = EEmpExtraField.db.select(dbConn, new DBFilter());
                    foreach (EEmpExtraField empExtraField in empExtraFieldList)
                        if (row.Table.Columns.Contains(empExtraField.EmpExtraFieldName))
                        {
                            if (row[empExtraField.EmpExtraFieldName] != null)
                            {
                                EUploadEmpExtraFieldValue uploadEmpExtraFieldValue = new EUploadEmpExtraFieldValue();
                                if (empInfo.EmpID > 0)
                                    uploadEmpExtraFieldValue.EmpID = empInfo.EmpID;
                                uploadEmpExtraFieldValue.EmpExtraFieldID = empExtraField.EmpExtraFieldID;
                                uploadEmpExtraFieldValue.EmpExtraFieldValue = row[empExtraField.EmpExtraFieldName].ToString().Trim();
                                uploadEmpExtraFieldValueList.Add(uploadEmpExtraFieldValue);
                            }
                        }

                    empInfo.SessionID = m_SessionID;
                    empInfo.TransactionDate = UploadDateTime;
                    if (empInfo.EmpID <= 0)
                        empInfo.ImportActionStatus = ImportDBObject.ImportActionEnum.INSERT;
                    else
                        empInfo.ImportActionStatus = ImportDBObject.ImportActionEnum.UPDATE;


                    //if (empInfo.EmpNo.Trim().Length > 0 
                    //    && empInfo.EmpEngSurname.Trim().Length > 0 
                    //    && empInfo.EmpEngOtherName.Trim().Length > 0 
                    //    && empInfo.EmpGender.Trim().Length > 0 
                    //    && empInfo.EmpMaritalStatus.Trim().Length > 0
                    //    && empInfo.EmpDateOfBirth.Ticks > 0
                    //    && empInfo.EmpResAddr.Trim().Length > 0
                    //    && empInfo.EmpResAddrAreaCode.Trim().Length > 0
                    //    )
                    Hashtable values = new Hashtable();
                    EUploadEmpPersonalInfo.db.populate(empInfo, values);
                    PageErrors pageErrors = new PageErrors(EUploadEmpPersonalInfo.db);
                    EUploadEmpPersonalInfo.db.validate(pageErrors, values);
                    if (pageErrors.errors.Count == 0)
                    {

                        db.insert(dbConn, empInfo);
                        //if (empUniform != null)
                        //{
                        //    empUniform.UploadEmpID = empInfo.UploadEmpID;
                        //    EUploadEmpUniform.db.insert(dbConn, empUniform);
                        //}
                        foreach (EUploadEmpExtraFieldValue uploadEmpExtraFieldValue in uploadEmpExtraFieldValueList)
                        {
                            uploadEmpExtraFieldValue.UploadEmpID = empInfo.UploadEmpID;
                            tempEmpExtraFieldValueDB.insert(dbConn, uploadEmpExtraFieldValue);
                        }
                        results.Add(empInfo);
                    }
                    else
                    {
                        pageErrors.addError(rawDataTable.TableName);
                        throw new HRImportException(pageErrors.getPrompt() + "(line " + rowCount.ToString() + ")");

                        //if (EmpID == 0)
                        //    errors.addError(ImportErrorMessage.ERROR_INVALID_EMP_NO, new string[] { EmpNo, rowCount.ToString() });
                        //else if (PayCodeID == 0)
                        //    errors.addError(ImportErrorMessage.ERROR_INVALID_FIELD_VALUE, new string[] { FIELD_PAYMENT_CODE + "=" + PaymentCode, EmpNo, rowCount.ToString() });
                        //else if (PayCodeID == 0)
                        //    errors.addError(ImportErrorMessage.ERROR_INVALID_FIELD_VALUE, new string[] { FIELD_PAYMENT_CODE + "=" + PaymentCode, EmpNo, rowCount.ToString() });
                        //else if (EffDate.Ticks == 0)
                        //    errors.addError(ImportErrorMessage.ERROR_INVALID_FIELD_VALUE, new string[] { FIELD_EFFECTIVE_DATE + "=" + EffDateString, EmpNo, rowCount.ToString() });
                        //else if (double.TryParse(amountString, out amount))
                        //    errors.addError(ImportErrorMessage.ERROR_INVALID_FIELD_VALUE, new string[] { FIELD_AMOUNT + "=" + amountString, EmpNo, rowCount.ToString() });
                    }

                }
                if (errors.List.Count > 0)
                {
                    ClearTempTable();
                    throw (new HRImportException(rawDataTable.TableName + "\r\n" + errors.Message()));
                }
            }
            return GetImportDataFromTempDatabase(null);

        }
        public override DataTable UploadToTempDatabase(string Filename, int UserID, string ZipPassword)
        {
            //  Set default value of CreateCodeIfNotExists to "true" for compatable with previous behaviour of Import Console 
            return UploadToTempDatabase(Filename, UserID, ZipPassword, true);
        }

        public DataTable UploadToTempDatabase(string Filename, int UserID, string ZipPassword, bool CreateCodeIfNotExists)
        {
            DataSet rawDataSet = HROne.Import.ExcelImport.parse(Filename, ZipPassword);

            ClearTempTable();
            try
            {
                foreach (DataTable rawDataTable in rawDataSet.Tables)
                {
                    if (rawDataTable.TableName.StartsWith(TABLE_NAME, StringComparison.CurrentCultureIgnoreCase))
                        UploadToTempDatabase(rawDataTable, UserID, CreateCodeIfNotExists);
                }

                foreach (DataTable rawDataTable in rawDataSet.Tables)
                {
                    if (rawDataTable.TableName.StartsWith(ImportEmpPositionInfoProcess.TABLE_NAME, StringComparison.CurrentCultureIgnoreCase))
                        importEmpPos.UploadToTempDatabase(rawDataTable, UserID, CreateCodeIfNotExists);
                    else if (rawDataTable.TableName.StartsWith(ImportEmpBankAccountProcess.TABLE_NAME, StringComparison.CurrentCultureIgnoreCase))
                        importEmpBankAcc.UploadToTempDatabase(rawDataTable, UserID);
                    else if (rawDataTable.TableName.StartsWith(ImportEmpBenefitProcess.TABLE_NAME, StringComparison.CurrentCultureIgnoreCase))
                        importEmpBenefit.UploadToTempDatabase(rawDataTable, UserID);

                    else if (rawDataTable.TableName.StartsWith(ImportEmpBeneficiariesProcess.TABLE_NAME, StringComparison.CurrentCultureIgnoreCase))
                        importEmpBeneficiaries.UploadToTempDatabase(rawDataTable, UserID);

                    else if (rawDataTable.TableName.StartsWith(ImportEmpMPFPlanProcess.TABLE_NAME, StringComparison.CurrentCultureIgnoreCase))
                        importEmpMPF.UploadToTempDatabase(rawDataTable, UserID);
                    else if (rawDataTable.TableName.StartsWith(ImportEmpAVCPlanProcess.TABLE_NAME, StringComparison.CurrentCultureIgnoreCase))
                        importEmpAVC.UploadToTempDatabase(rawDataTable, UserID);
                    else if (rawDataTable.TableName.StartsWith(ImportEmpORSOPlanProcess.TABLE_NAME, StringComparison.CurrentCultureIgnoreCase))
                        importEmpORSO.UploadToTempDatabase(rawDataTable, UserID);
                    else if (rawDataTable.TableName.StartsWith(ImportEmpSpouseProcess.TABLE_NAME, StringComparison.CurrentCultureIgnoreCase))
                        importEmpSpouse.UploadToTempDatabase(rawDataTable, UserID);
                    else if (rawDataTable.TableName.StartsWith(ImportEmpDependantProcess.TABLE_NAME, StringComparison.CurrentCultureIgnoreCase))
                        importEmpDependant.UploadToTempDatabase(rawDataTable, UserID);
                    else if (rawDataTable.TableName.StartsWith(ImportEmpQualificationProcess.TABLE_NAME, StringComparison.CurrentCultureIgnoreCase))
                        importEmpQualification.UploadToTempDatabase(rawDataTable, UserID, CreateCodeIfNotExists);
                    else if (rawDataTable.TableName.StartsWith(ImportEmpSkillProcess.TABLE_NAME, StringComparison.CurrentCultureIgnoreCase))
                        importEmpSkill.UploadToTempDatabase(rawDataTable, UserID, CreateCodeIfNotExists);
                    else if (rawDataTable.TableName.StartsWith(ImportEmpPlaceOfResidenceProcess.TABLE_NAME, StringComparison.CurrentCultureIgnoreCase))
                        importEmpPlaceOfResidence.UploadToTempDatabase(rawDataTable, UserID);
                    else if (rawDataTable.TableName.StartsWith(ImportEmpContractTermsProcess.TABLE_NAME, StringComparison.CurrentCultureIgnoreCase))
                        importEmpContractTerms.UploadToTempDatabase(rawDataTable, UserID);
                    else if (rawDataTable.TableName.StartsWith(ImportEmpTerminationProcess.TABLE_NAME, StringComparison.CurrentCultureIgnoreCase))
                        importEmpTermination.UploadToTempDatabase(rawDataTable, UserID, CreateCodeIfNotExists);
                    else if (rawDataTable.TableName.StartsWith(ImportLeaveApplicationProcess.TABLE_NAME, StringComparison.CurrentCultureIgnoreCase))
                        importLeaveApplication.UploadToTempDatabase(rawDataTable, UserID);
                    else if (rawDataTable.TableName.StartsWith(ImportEmpCostCenterProcess.TABLE_NAME, StringComparison.CurrentCultureIgnoreCase))
                        importEmpCostCenter.UploadToTempDatabase(rawDataTable, UserID, CreateCodeIfNotExists);
                    else if (rawDataTable.TableName.StartsWith(ImportEmpPermitProcess.TABLE_NAME, StringComparison.CurrentCultureIgnoreCase))
                        importEmpPermit.UploadToTempDatabase(rawDataTable, UserID, CreateCodeIfNotExists);
                    else if (rawDataTable.TableName.StartsWith(ImportEmpEmergencyContactProcess.TABLE_NAME, StringComparison.CurrentCultureIgnoreCase))
                        importEmpEmergencyContact.UploadToTempDatabase(rawDataTable, UserID);
                    else if (rawDataTable.TableName.StartsWith(ImportEmpWorkExpProcess.TABLE_NAME, StringComparison.CurrentCultureIgnoreCase))
                        importEmpWorkExp.UploadToTempDatabase(rawDataTable, UserID);
                    else if (rawDataTable.TableName.StartsWith(ImportEmpWorkInjuryRecordProcess.TABLE_NAME, StringComparison.CurrentCultureIgnoreCase))
                        importEmpWorkInjuryRecord.UploadToTempDatabase(rawDataTable, UserID);
                    else if (rawDataTable.TableName.StartsWith(ImportEmpRosterTableGroupProcess.TABLE_NAME, StringComparison.CurrentCultureIgnoreCase))
                        importEmpRosterTableGroup.UploadToTempDatabase(rawDataTable, UserID);
                    else if (rawDataTable.TableName.StartsWith(ImportLeaveBalanceAdjustmentProcess.TABLE_NAME, StringComparison.CurrentCultureIgnoreCase))
                        importLeaveBalanceAdjustment.UploadToTempDatabase(rawDataTable, UserID);
                    else if (rawDataTable.TableName.StartsWith(ImportCompensationLeaveEntitleProcess.TABLE_NAME, StringComparison.CurrentCultureIgnoreCase))
                        importCompensationLeaveEntitle.UploadToTempDatabase(rawDataTable, UserID);
                    // Start 0000192, Miranda, 2015-05-08
                    else if (rawDataTable.TableName.StartsWith(ImportEmpCostCenterExportProcess.TABLE_NAME, StringComparison.CurrentCultureIgnoreCase))
                        importEmpCostCenterExport.UploadToTempDatabase(rawDataTable, UserID, CreateCodeIfNotExists);
                    // End 0000192, Miranda, 2015-05-08
                }

                foreach (DataTable rawDataTable in rawDataSet.Tables)
                {
                    if (rawDataTable.TableName.StartsWith(ImportEmpRecurringPaymentProcess.TABLE_NAME, StringComparison.CurrentCultureIgnoreCase))
                        importEmpPay.UploadToTempDatabase(rawDataTable, UserID);
                    else if (rawDataTable.TableName.StartsWith(ImportEmpFinalPaymentProcess.TABLE_NAME, StringComparison.CurrentCultureIgnoreCase))
                        importEmpFinalPayment.UploadToTempDatabase(rawDataTable, UserID);

                }
            }
            catch (HRImportException he)
            {
                ClearTempTable();
                throw he;
            }
            return GetImportDataFromTempDatabase(null);
        }

        public override DataTable GetImportDataFromTempDatabase(ListInfo info)
        {
            DBFilter sessionFilter = new DBFilter();
            sessionFilter.add(new Match("SessionID", m_SessionID));
            //if (info != null && info.orderby != null && !info.orderby.Equals(""))
            //    sessionFilter.add(info.orderby, info.order);
            DataTable table = sessionFilter.loadData(dbConn, null, " c.* ", " from " + db.dbclass.tableName + " c ");



            DBAESEncryptStringFieldAttribute.decode(table, "EmpHKID");
            DBAESEncryptStringFieldAttribute.decode(table, "EmpPassportNo");
            DBAESEncryptStringFieldAttribute.decode(table, "EmpResAddr");
            DBAESEncryptStringFieldAttribute.decode(table, "EmpCorAddr");

            return table;

        }

        public static void ClearTempTable(DatabaseConnection dbConn, string sessionID)
        {
            ImportEmpPersonalInfoProcess import = new ImportEmpPersonalInfoProcess(dbConn, sessionID);
            import.ClearTempTable();
        }

        public override void ClearTempTable()
        {
            //  Clear Old Import Session


            DBFilter sessionFilter = new DBFilter();
            if (!string.IsNullOrEmpty(m_SessionID))
                sessionFilter.add(new Match("SessionID", m_SessionID));
            DBFilter UploadEmpIDFilter = new DBFilter();
            UploadEmpIDFilter.add(new IN("UploadEmpID", "Select UploadEmpID from " + EUploadEmpPersonalInfo.db.dbclass.tableName, sessionFilter));
            EUploadEmpExtraFieldValue.db.delete(dbConn, UploadEmpIDFilter);
            //EUploadEmpUniform.db.delete(dbConn, UploadEmpIDFilter);
            db.delete(dbConn, sessionFilter);

            importEmpPos.ClearTempTable();
            importEmpBankAcc.ClearTempTable();
            importEmpMPF.ClearTempTable();
            importEmpAVC.ClearTempTable();
            importEmpORSO.ClearTempTable();
            importEmpSpouse.ClearTempTable();
            importEmpDependant.ClearTempTable();
            importEmpQualification.ClearTempTable();
            importEmpSkill.ClearTempTable();
            importEmpPlaceOfResidence.ClearTempTable();
            importEmpContractTerms.ClearTempTable();
            importEmpTermination.ClearTempTable();
            importLeaveApplication.ClearTempTable();
            importEmpCostCenter.ClearTempTable();
            importEmpPermit.ClearTempTable();
            importEmpEmergencyContact.ClearTempTable();
            importEmpWorkExp.ClearTempTable();
            importEmpWorkInjuryRecord.ClearTempTable();
            importEmpRosterTableGroup.ClearTempTable();
            importLeaveBalanceAdjustment.ClearTempTable();
            importCompensationLeaveEntitle.ClearTempTable();

            importEmpPay.ClearTempTable();
            importEmpFinalPayment.ClearTempTable();

        }

        public override void ImportToDatabase()
        {

            DBFilter sessionFilter = new DBFilter();
            sessionFilter.add(new Match("SessionID", m_SessionID));

            ArrayList uploadEmpInfoList = EUploadEmpPersonalInfo.db.select(dbConn, sessionFilter);
            foreach (EUploadEmpPersonalInfo obj in uploadEmpInfoList)
            {
                EEmpPersonalInfo empInfo = new EEmpPersonalInfo();

                if (obj.ImportActionStatus != ImportDBObject.ImportActionEnum.INSERT)
                {
                    empInfo.EmpID = obj.EmpID;
                    EEmpPersonalInfo.db.select(dbConn, empInfo);
                }

                obj.ExportToObject(empInfo);
                if (string.IsNullOrEmpty(empInfo.EmpStatus))
                    empInfo.EmpStatus = "A";

                AppUtils.StartChildFunction(dbConn, empInfo.EmpID);
                if (obj.ImportActionStatus == ImportDBObject.ImportActionEnum.INSERT)
                {
                    EEmpPersonalInfo.db.insert(dbConn, empInfo);
                    obj.EmpID = empInfo.EmpID;
                    EUploadEmpPersonalInfo.db.update(dbConn, obj);


                    DBFilter uploadEmpFilter = new DBFilter();
                    uploadEmpFilter.add(new Match("UploadEmpID", obj.UploadEmpID));
                    //ArrayList uploadEmpUniformList = EUploadEmpUniform.db.select(dbConn, uploadEmpFilter);
                    //foreach (EUploadEmpUniform uploadEmpUniform in uploadEmpUniformList)
                    //{
                    //    uploadEmpUniform.EmpID = empInfo.EmpID;
                    //    EEmpUniform empUniform = new EEmpUniform();
                    //    ImportDBObject.CopyObjectProperties(uploadEmpUniform, empUniform);
                    //    EEmpUniform.db.insert(dbConn, empUniform);
                    //}

                    ArrayList uploadEmpExtraFieldValueList = EUploadEmpExtraFieldValue.db.select(dbConn, uploadEmpFilter);
                    foreach (EUploadEmpExtraFieldValue uploadEmpExtraFieldValue in uploadEmpExtraFieldValueList)
                    {
                        uploadEmpExtraFieldValue.EmpID = empInfo.EmpID;
                        EEmpExtraFieldValue empExtraFieldValue = new EEmpExtraFieldValue();
                        ImportDBObject.CopyObjectProperties(uploadEmpExtraFieldValue, empExtraFieldValue);
                        uploadEmpExtraFieldValueDB.insert(dbConn, empExtraFieldValue);
                    }
                }
                else if (obj.ImportActionStatus == ImportDBObject.ImportActionEnum.UPDATE)
                {
                    EEmpPersonalInfo.db.update(dbConn, empInfo);

                    DBFilter uploadEmpFilter = new DBFilter();
                    uploadEmpFilter.add(new Match("UploadEmpID", obj.UploadEmpID));
                    //ArrayList uploadEmpUniformList = EUploadEmpUniform.db.select(dbConn, uploadEmpFilter);
                    //foreach (EUploadEmpUniform uploadEmpUniform in uploadEmpUniformList)
                    //{

                    //    EEmpUniform empUniform = new EEmpUniform();
                    //    ImportDBObject.CopyObjectProperties(uploadEmpUniform, empUniform);

                    //    DBFilter empUniformFilter = new DBFilter();
                    //    empUniformFilter.add(new Match("EmpID", empInfo.EmpID));
                    //    ArrayList empUniformList = EEmpUniform.db.select(dbConn, empUniformFilter);
                    //    if (empUniformList.Count > 0)
                    //    {
                    //        empUniform.EmpUniformID = ((EEmpUniform)empUniformList[0]).EmpUniformID;
                    //        EEmpUniform.db.update(dbConn, empUniform);
                    //    }
                    //    else
                    //        EEmpUniform.db.insert(dbConn, empUniform);
                    //} 
                    ArrayList uploadEmpExtraFieldValueList = EUploadEmpExtraFieldValue.db.select(dbConn, uploadEmpFilter);
                    foreach (EUploadEmpExtraFieldValue uploadEmpExtraFieldValue in uploadEmpExtraFieldValueList)
                    {

                        EEmpExtraFieldValue empExtraFieldValue = new EEmpExtraFieldValue();
                        ImportDBObject.CopyObjectProperties(uploadEmpExtraFieldValue, empExtraFieldValue);

                        DBFilter empFieldValueListFilter = new DBFilter();
                        empFieldValueListFilter.add(new Match("EmpID", empInfo.EmpID));
                        empFieldValueListFilter.add(new Match("EmpExtraFieldID", uploadEmpExtraFieldValue.EmpExtraFieldID));
                        ArrayList empFieldValueListList = uploadEmpExtraFieldValueDB.select(dbConn, empFieldValueListFilter);
                        if (empFieldValueListList.Count > 0)
                        {
                            empExtraFieldValue.EmpExtraFieldValueID = ((EEmpExtraFieldValue)empFieldValueListList[0]).EmpExtraFieldValueID;
                            uploadEmpExtraFieldValueDB.update(dbConn, empExtraFieldValue);
                        }
                        else
                            uploadEmpExtraFieldValueDB.insert(dbConn, empExtraFieldValue);
                    }
                }
                importEmpPos.ImportToDatabase(obj.UploadEmpID);
                importEmpBankAcc.ImportToDatabase(obj.UploadEmpID);
                importEmpBenefit.ImportToDatabase(obj.UploadEmpID);
                importEmpBeneficiaries.ImportToDatabase(obj.UploadEmpID);
                importEmpPay.ImportToDatabase(obj.UploadEmpID);
                importEmpMPF.ImportToDatabase(obj.UploadEmpID);
                importEmpAVC.ImportToDatabase(obj.UploadEmpID);
                importEmpORSO.ImportToDatabase(obj.UploadEmpID);
                importEmpSpouse.ImportToDatabase(obj.UploadEmpID);
                importEmpDependant.ImportToDatabase(obj.UploadEmpID);
                importEmpQualification.ImportToDatabase(obj.UploadEmpID);
                importEmpSkill.ImportToDatabase(obj.UploadEmpID);
                importEmpPlaceOfResidence.ImportToDatabase(obj.UploadEmpID);
                importEmpContractTerms.ImportToDatabase(obj.UploadEmpID);
                importEmpTermination.ImportToDatabase(obj.UploadEmpID);
                importLeaveApplication.ImportToDatabase(obj.UploadEmpID);
                importEmpFinalPayment.ImportToDatabase(obj.UploadEmpID);
                importEmpCostCenter.ImportToDatabase(obj.UploadEmpID);
                importEmpPermit.ImportToDatabase(obj.UploadEmpID);
                importEmpEmergencyContact.ImportToDatabase(obj.UploadEmpID);
                importEmpWorkExp.ImportToDatabase(obj.UploadEmpID);
                importEmpWorkInjuryRecord.ImportToDatabase(obj.UploadEmpID);
                importEmpRosterTableGroup.ImportToDatabase(obj.UploadEmpID);
                importLeaveBalanceAdjustment.ImportToDatabase(obj.UploadEmpID);
                importCompensationLeaveEntitle.ImportToDatabase(obj.UploadEmpID);
                AppUtils.EndChildFunction(dbConn);

                EUploadEmpPersonalInfo.db.delete(dbConn, obj);
            }

            importEmpPos.ImportToDatabase();
            importEmpBankAcc.ImportToDatabase();
            importEmpPay.ImportToDatabase();
            importEmpMPF.ImportToDatabase();
            importEmpAVC.ImportToDatabase();
            importEmpORSO.ImportToDatabase();
            importEmpSpouse.ImportToDatabase();
            importEmpDependant.ImportToDatabase();
            importEmpQualification.ImportToDatabase();
            importEmpSkill.ImportToDatabase();
            importEmpPlaceOfResidence.ImportToDatabase();
            importEmpContractTerms.ImportToDatabase();
            importEmpTermination.ImportToDatabase();
            importLeaveApplication.ImportToDatabase();
            importEmpFinalPayment.ImportToDatabase();
            importEmpCostCenter.ImportToDatabase();
            importEmpPermit.ImportToDatabase();
            importEmpEmergencyContact.ImportToDatabase();
            importEmpWorkExp.ImportToDatabase();
            importEmpWorkInjuryRecord.ImportToDatabase();
            importEmpRosterTableGroup.ImportToDatabase();
            importLeaveBalanceAdjustment.ImportToDatabase();
            importCompensationLeaveEntitle.ImportToDatabase();
        }

        public static DataTable Export(DatabaseConnection dbConn, ArrayList empList, bool IsIncludeCurrentPositionInfo)
        {
            return Export(dbConn, empList, IsIncludeCurrentPositionInfo, false, new DateTime());
        }
        public static DataTable Export(DatabaseConnection dbConn, ArrayList empList, bool IsIncludeCurrentPositionInfo, bool IsIncludeSyncID, DateTime ReferenceDateTime)
        {
            DataTable tmpDataTable = new DataTable(TABLE_NAME);
            //if (IsIncludeInternalID)
            //    tmpDataTable.Columns.Add(FIELD_INTERNAL_ID, typeof(string));

            tmpDataTable.Columns.Add(FIELD_EMP_NO, typeof(string));

            tmpDataTable.Columns.Add(FIELD_SURNAME, typeof(string));
            tmpDataTable.Columns.Add(FIELD_OTHER_NAME, typeof(string));
            tmpDataTable.Columns.Add(FIELD_ALIAS, typeof(string));
            tmpDataTable.Columns.Add(FIELD_CHINESE_NAME, typeof(string));
            tmpDataTable.Columns.Add(FIELD_GENDER, typeof(string));
            tmpDataTable.Columns.Add(FIELD_MARITAL_STATUS, typeof(string));
            tmpDataTable.Columns.Add(FIELD_DATE_OF_BIRTH, typeof(DateTime));
            tmpDataTable.Columns.Add(FIELD_HKID, typeof(string));
            tmpDataTable.Columns.Add(FIELD_PLACE_OF_BIRTH, typeof(string));
            tmpDataTable.Columns.Add(FIELD_NATIONALITY, typeof(string));
            tmpDataTable.Columns.Add(FIELD_PASSPORT_NUMBER, typeof(string));
            tmpDataTable.Columns.Add(FIELD_PASSPORT_ISSUE_COUNTRY, typeof(string));
            tmpDataTable.Columns.Add(FIELD_PASSPORT_EXPIRY_DATE, typeof(DateTime));
            tmpDataTable.Columns.Add(FIELD_HOME_PHONE_NUMBER, typeof(string));
            tmpDataTable.Columns.Add(FIELD_MOBILE_PHONE_NUMBER, typeof(string));
            tmpDataTable.Columns.Add(FIELD_OFFICE_PHONE_NUMBER, typeof(string));
            tmpDataTable.Columns.Add(FIELD_PERSONAL_EMAIL_ADDRESS, typeof(string));
            tmpDataTable.Columns.Add(FIELD_COMPANY_EMAIL_ADDRESS, typeof(string));
            tmpDataTable.Columns.Add(FIELD_RESIDENTIAL_ADDRESS, typeof(string));
            tmpDataTable.Columns.Add(FIELD_RESIDENTIAL_ADDRESS + " 1", typeof(string));
            tmpDataTable.Columns.Add(FIELD_RESIDENTIAL_ADDRESS + " 2", typeof(string));
            tmpDataTable.Columns.Add(FIELD_RESIDENTIAL_ADDRESS + " 3", typeof(string));
            tmpDataTable.Columns.Add(FIELD_RESIDENTIAL_ADDRESS_AREA_CODE, typeof(string));
            tmpDataTable.Columns.Add(FIELD_CORRESPONDENCE_ADDRESS, typeof(string));
            tmpDataTable.Columns.Add(FIELD_DATE_OF_JOIN, typeof(DateTime));
            tmpDataTable.Columns.Add(FIELD_SERVICE_START_DATE, typeof(DateTime));
            tmpDataTable.Columns.Add(FIELD_PROBATION_PERIOD, typeof(double));
            tmpDataTable.Columns.Add(FIELD_PROBATION_PERIOD_UNIT, typeof(string));
            tmpDataTable.Columns.Add(FIELD_PROBATION_LAST_DATE, typeof(DateTime));
            tmpDataTable.Columns.Add(FIELD_NOTICE_PERIOD, typeof(double));
            tmpDataTable.Columns.Add(FIELD_NOTICE_PERIOD_UNIT, typeof(string));
            tmpDataTable.Columns.Add(FIELD_TIMECARDNO, typeof(string));
            tmpDataTable.Columns.Add(FIELD_REMARK, typeof(string));
            if (ESystemParameter.IsEnabled(dbConn, ESystemParameter.PARAM_CODE_PAYSCALE_POINT_SYSTEM))
            {
                tmpDataTable.Columns.Add(FIELD_NEXT_SALARY_INCREMENT_DATE, typeof(DateTime));
            }

            // Start 0000067, Miranda, 2014-08-07
            tmpDataTable.Columns.Add(FIELD_ORIGINAL_HIRE_DATE, typeof(DateTime));
            // End 0000067, Miranda, 2014-08-07
            //tmpDataTable.Columns.Add(FIELD_UNIFORM_B, typeof(string));
            //tmpDataTable.Columns.Add(FIELD_UNIFORM_W, typeof(string));
            //tmpDataTable.Columns.Add(FIELD_UNIFORM_H, typeof(string));

            AddExtraFieldHeader(dbConn, tmpDataTable);

            if (IsIncludeSyncID)
                tmpDataTable.Columns.Add(FIELD_SYNC_ID, typeof(string));

            foreach (EEmpPersonalInfo empInfo in empList)
            {
                if (EEmpPersonalInfo.db.select(dbConn, empInfo))
                {
                    if (empInfo.RecordCreatedDateTime >= ReferenceDateTime || empInfo.RecordLastModifiedDateTime >= ReferenceDateTime)
                    {
                        DataRow row = tmpDataTable.NewRow();
                        //if (IsIncludeInternalID)
                        //    row[FIELD_INTERNAL_ID] = ToHexDecWithCheckDigit(empInfo.EmpID);
                        row[FIELD_EMP_NO] = empInfo.EmpNo;

                        row[FIELD_SURNAME] = empInfo.EmpEngSurname;
                        row[FIELD_OTHER_NAME] = empInfo.EmpEngOtherName;
                        row[FIELD_ALIAS] = empInfo.EmpAlias;
                        row[FIELD_CHINESE_NAME] = empInfo.EmpChiFullName;
                        row[FIELD_GENDER] = empInfo.EmpGender;
                        if (empInfo.EmpMaritalStatus.Equals("Separated", StringComparison.CurrentCultureIgnoreCase))
                            row[FIELD_MARITAL_STATUS] = "Living Apart";
                        else
                            row[FIELD_MARITAL_STATUS] = empInfo.EmpMaritalStatus;
                        row[FIELD_DATE_OF_BIRTH] = empInfo.EmpDateOfBirth;
                        row[FIELD_HKID] = empInfo.EmpHKID;
                        // Start 0000092, Ricky So, 2014/09/28
                        // export Place Of Birth code
                        // row[FIELD_PLACE_OF_BIRTH] = empInfo.EmpPlaceOfBirth;
                        if (empInfo.EmpPlaceOfBirthID > 0)
                        {
                            EPlaceOfBirth m_placeOfBirth = new EPlaceOfBirth();
                            m_placeOfBirth.PlaceOfBirthID = empInfo.EmpPlaceOfBirthID;
                            if (EPlaceOfBirth.db.select(dbConn, m_placeOfBirth))
                            {
                                row[FIELD_PLACE_OF_BIRTH] = m_placeOfBirth.PlaceOfBirthCode;
                            }
                        }
                        // End 0000092, Ricky So, 2014/09/28

                        // Start 0000092, Ricky So, 2014/09/28
                        // export Nationality Code
                        //row[FIELD_NATIONALITY] = empInfo.EmpNationality;
                        if (empInfo.EmpNationalityID > 0)
                        {
                            ENationality m_nationality = new ENationality();
                            m_nationality.NationalityID = empInfo.EmpNationalityID;
                            if (ENationality.db.select(dbConn, m_nationality))
                            {
                                row[FIELD_NATIONALITY] = m_nationality.NationalityCode;
                            }
                        }
                        // End 0000092, Ricky So, 2014/09/28

                        row[FIELD_PASSPORT_NUMBER] = empInfo.EmpPassportNo;
                        // Start 0000092, Ricky So, 2014/09/28
                        // export Country Code
                        //row[FIELD_PASSPORT_ISSUE_COUNTRY] = empInfo.EmpPassportIssuedCountry;
                        if (empInfo.EmpPassportIssuedCountryID > 0)
                        {
                            EIssueCountry m_issueContry = new EIssueCountry();
                            m_issueContry.CountryID = empInfo.EmpPassportIssuedCountryID;
                            if (EIssueCountry.db.select(dbConn, m_issueContry))
                            {
                                row[FIELD_PASSPORT_ISSUE_COUNTRY] = m_issueContry.CountryCode;
                            }
                        }
                        // End 0000092, Ricky So, 2014/09/28

                        
                        row[FIELD_PASSPORT_EXPIRY_DATE] = empInfo.EmpPassportExpiryDate;
                        row[FIELD_HOME_PHONE_NUMBER] = empInfo.EmpHomePhoneNo;
                        row[FIELD_MOBILE_PHONE_NUMBER] = empInfo.EmpMobileNo;
                        row[FIELD_OFFICE_PHONE_NUMBER] = empInfo.EmpOfficePhoneNo;
                        row[FIELD_PERSONAL_EMAIL_ADDRESS] = empInfo.EmpEmail;
                        row[FIELD_COMPANY_EMAIL_ADDRESS] = empInfo.EmpInternalEmail;
                        row[FIELD_RESIDENTIAL_ADDRESS] = empInfo.EmpResAddr;

                        string[] address = empInfo.EmpResAddr.Split(new string[] { "\r\n" }, StringSplitOptions.None);
                        if (address.GetLength(0) >= 1)
                            row[FIELD_RESIDENTIAL_ADDRESS + " 1"] = address[0];
                        if (address.GetLength(0) >= 2)
                            row[FIELD_RESIDENTIAL_ADDRESS + " 2"] = address[1];
                        if (address.GetLength(0) >= 3)
                        {
                            row[FIELD_RESIDENTIAL_ADDRESS + " 3"] = address[2];
                            for (int i = 3; i < address.GetLength(0); i++)
                                row[FIELD_RESIDENTIAL_ADDRESS + " 3"] += " " + address[i];
                        }

                        row[FIELD_RESIDENTIAL_ADDRESS_AREA_CODE] = empInfo.EmpResAddrAreaCode;
                        row[FIELD_CORRESPONDENCE_ADDRESS] = empInfo.EmpCorAddr;
                        if (ESystemParameter.IsEnabled(dbConn, ESystemParameter.PARAM_CODE_PAYSCALE_POINT_SYSTEM))
                        {
                            row[FIELD_NEXT_SALARY_INCREMENT_DATE] = empInfo.EmpNextSalaryIncrementDate;
                        }
                        row[FIELD_DATE_OF_JOIN] = empInfo.EmpDateOfJoin;
                        row[FIELD_SERVICE_START_DATE] = empInfo.EmpServiceDate;
                        row[FIELD_PROBATION_PERIOD] = empInfo.EmpProbaPeriod;
                        row[FIELD_PROBATION_PERIOD_UNIT] = empInfo.EmpProbaUnit;
                        row[FIELD_PROBATION_LAST_DATE] = empInfo.EmpProbaLastDate;
                        row[FIELD_NOTICE_PERIOD] = empInfo.EmpNoticePeriod;
                        row[FIELD_NOTICE_PERIOD_UNIT] = empInfo.EmpNoticeUnit;
                        row[FIELD_TIMECARDNO] = empInfo.EmpTimeCardNo;
                        row[FIELD_REMARK] = empInfo.Remark;
                        // Start 0000067, Miranda, 2014-08-07
                        row[FIELD_ORIGINAL_HIRE_DATE] = empInfo.EmpOriginalHireDate;
                        // End 0000067, Miranda, 2014-08-07

                        //DBFilter empUniformFilter = new DBFilter();
                        //empUniformFilter.add(new Match("EmpID", empInfo.EmpID));
                        //ArrayList empUniformList = EEmpUniform.db.select(dbConn, empUniformFilter);
                        //if (empUniformList.Count > 0)
                        //{
                        //    row[FIELD_UNIFORM_B] = ((EEmpUniform)empUniformList[0]).EmpUniformB;
                        //    row[FIELD_UNIFORM_W] = ((EEmpUniform)empUniformList[0]).EmpUniformW;
                        //    row[FIELD_UNIFORM_H] = ((EEmpUniform)empUniformList[0]).EmpUniformH;
                        //}

                        AddExtraFieldInfo(dbConn, row, empInfo.EmpID);

                        if (IsIncludeSyncID)
                            row[FIELD_SYNC_ID] = empInfo.SynID;

                        tmpDataTable.Rows.Add(row);
                    }
                }

            }
            RetriveExtraFieldHeader(dbConn, tmpDataTable);
            return tmpDataTable;
        }

        public static void AddEmployeeInfoHeader(DataTable dataTable)
        {
            dataTable.Columns.Add("Employee Name", typeof(String));
            dataTable.Columns.Add("Employee Alias", typeof(String));
            dataTable.Columns.Add("Employee Name in Chinese", typeof(String));
        }

        public static void AddEmployeeInfo(DatabaseConnection dbConn, DataRow row, int EmpID)
        {
            EEmpPersonalInfo empInfo = new EEmpPersonalInfo();
            empInfo.EmpID = EmpID;
            if (EEmpPersonalInfo.db.select(dbConn, empInfo))
            {
                row["Employee Name"] = empInfo.EmpEngFullName;
                row["Employee Alias"] = empInfo.EmpAlias;
                row["Employee Name in Chinese"] = empInfo.EmpChiFullName;
            }
        }

        public static void AddExtraFieldHeader(DatabaseConnection dbConn, DataTable dataTable)
        {
            ArrayList empExtraFieldList = EEmpExtraField.db.select(dbConn, new DBFilter());
            foreach (EEmpExtraField empExtraField in empExtraFieldList)
                dataTable.Columns.Add("ExtraFieldID_" + empExtraField.EmpExtraFieldID, typeof(String));

        }

        public static void AddExtraFieldInfo(DatabaseConnection dbConn, DataRow row, int EmpID)
        {
            DBFilter empExtraFieldValueFilter = new DBFilter();
            empExtraFieldValueFilter.add(new Match("EmpID", EmpID));
            ArrayList empExtraFieldValueList = EEmpExtraFieldValue.db.select(dbConn, empExtraFieldValueFilter);
            foreach (EEmpExtraFieldValue empExtraFieldValue in empExtraFieldValueList)
            {
                EEmpExtraField empExtraField = new EEmpExtraField();
                empExtraField.EmpExtraFieldID = empExtraFieldValue.EmpExtraFieldID;
                if (EEmpExtraField.db.select(dbConn, empExtraField))
                {

                    row["ExtraFieldID_" + empExtraField.EmpExtraFieldID] = empExtraFieldValue.EmpExtraFieldValue;
                }

            }
        }

        public static void RetriveExtraFieldHeader(DatabaseConnection dbConn, DataTable dataTable)
        {
            ArrayList empExtraFieldList = EEmpExtraField.db.select(dbConn, new DBFilter());
            foreach (EEmpExtraField empExtraField in empExtraFieldList)
            {
                if (!dataTable.Columns.Contains(empExtraField.EmpExtraFieldName))
                    dataTable.Columns["ExtraFieldID_" + empExtraField.EmpExtraFieldID].ColumnName = empExtraField.EmpExtraFieldName;
                else
                    dataTable.Columns["ExtraFieldID_" + empExtraField.EmpExtraFieldID].ColumnName = empExtraField.EmpExtraFieldName + "(Duplicate Extra Info Field)";

            }
        }
    }
}