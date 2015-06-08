using System;
using System.Collections;
using System.Text;
using HROne.DataAccess;
////using perspectivemind.validation;
using HROne.Common;

namespace HROne.Lib.Entities
{
    [DBClass("PaymentType")]
    public class EPaymentType : BaseObject
    {
        public abstract class SystemPaymentType
        {
            public static EPaymentType BasicSalaryPaymentType(DatabaseConnection dbConn)
            {
                return GetPaymentType(dbConn, "BASICSAL");
            }

            public static EPaymentType OverTimePaymentType(DatabaseConnection dbConn)
            {
                return GetPaymentType(dbConn, "OTPAY");
            }

            public static EPaymentType OthersPaymentType(DatabaseConnection dbConn)
            {
                return GetPaymentType(dbConn, "OTHERS");
            }

            public static EPaymentType LeaveAllowancePaymentType(DatabaseConnection dbConn)
            {
                return GetPaymentType(dbConn, "LEAVEALLOW");
            }

            public static EPaymentType LeaveDeductionPaymentType(DatabaseConnection dbConn)
            {
                return GetPaymentType(dbConn, "LEAVEDEDUCT");
            }

            public static EPaymentType CommissionPaymentType(DatabaseConnection dbConn)
            {
                return GetPaymentType(dbConn, "COMMISSION");
            }

            public static EPaymentType BonusPaymentType(DatabaseConnection dbConn)
            {
                return GetPaymentType(dbConn, "BONUS");
            }

            public static EPaymentType LongServicePaymentSeverancePaymentType(DatabaseConnection dbConn)
            {
                return GetPaymentType(dbConn, "LSPSP");
            }

            public static EPaymentType MPFEmployerMandatoryContributionPaymentType(DatabaseConnection dbConn)
            {
                EPaymentType paymentType = GetPaymentType(dbConn, "MPFMCER");
                if (paymentType == null)
                    paymentType = GetPaymentType(dbConn, "MPFER");
                return paymentType;
            }

            public static EPaymentType MPFEmployeeMandatoryContributionPaymentType(DatabaseConnection dbConn)
            {
                EPaymentType paymentType = GetPaymentType(dbConn, "MPFMCEE");
                if (paymentType == null)
                    paymentType = GetPaymentType(dbConn, "MPFEE");
                return paymentType;
            }

            public static EPaymentType MPFEmployerVoluntaryContributionPaymentType(DatabaseConnection dbConn)
            {
                EPaymentType paymentType = GetPaymentType(dbConn, "MPFVCER");
                if (paymentType == null)
                    paymentType = GetPaymentType(dbConn, "TOPUPER");
                return paymentType;
            }

            public static EPaymentType MPFEmployeeVoluntaryContributionPaymentType(DatabaseConnection dbConn)
            {
                EPaymentType paymentType = GetPaymentType(dbConn, "MPFVCEE");
                if (paymentType == null)
                    paymentType = GetPaymentType(dbConn, "TOPUPEE");
                return paymentType;
            }

            public static EPaymentType PFundEmployerContributionPaymentType(DatabaseConnection dbConn)
            {
                return GetPaymentType(dbConn, "PFUNDER");
            }

            public static EPaymentType PFundEmployeeContributionPaymentType(DatabaseConnection dbConn)
            {
                return GetPaymentType(dbConn, "PFUNDEE");
            }

            protected static EPaymentType GetPaymentType(DatabaseConnection dbConn, String PaymentType)
            {
                DBFilter dbFilter = new DBFilter();
                dbFilter.add(new Match("PaymentTypeCode", PaymentType));
                ArrayList arrayList = EPaymentType.db.select(dbConn, dbFilter);
                if (arrayList.Count > 0)
                    return (EPaymentType)arrayList[0];
                else
                    return null;
            }
        }

        public static DBManager db = new DBManager(typeof(EPaymentType));
        public static WFValueList VLPaymentType = new WFDBCodeList(EPaymentType.db, "PaymentTypeID", "PaymentTypeCode", "PaymentTypeDesc", "PaymentTypeCode");


        protected int m_PaymentTypeID;
        [DBField("PaymentTypeID", true, true), TextSearch, Export(false)]
        public int PaymentTypeID
        {
            get { return m_PaymentTypeID; }
            set { m_PaymentTypeID = value; modify("PaymentTypeID"); }
        }
        protected string m_PaymentTypeCode;
        [DBField("PaymentTypeCode"), TextSearch,MaxLength(20,10), Export(false), Required]
        public string PaymentTypeCode
        {
            get { return m_PaymentTypeCode; }
            set { m_PaymentTypeCode = value; modify("PaymentTypeCode"); }
        }

        protected string m_PaymentTypeDesc;
        [DBField("PaymentTypeDesc"), TextSearch, MaxLength(100,25), Export(false), Required]
        public string PaymentTypeDesc
        {
            get { return m_PaymentTypeDesc; }
            set { m_PaymentTypeDesc = value; modify("PaymentTypeDesc"); }
        }


    }

    [DBClass("Rank")]
    public class ERank : BaseObject
    {
        public static DBManager db = new DBManager(typeof(ERank));
        public static WFValueList VLRank = new AppUtils.EncryptedDBCodeList(ERank.db, "RankID", new string[] { "RankCode", "RankDesc" }, " - ", "RankCode");

        public static ERank GetObject(DatabaseConnection dbConn, object ID)
        {
            if (ID is int)
            {
                ERank obj = new ERank();
                obj.RankID = (int)ID;
                if (db.select(dbConn, obj))
                    return obj;
            }
            return null;
        }

        protected int m_RankID;
        [DBField("RankID", true, true), TextSearch, Export(false)]
        public int RankID
        {
            get { return m_RankID; }
            set { m_RankID = value; modify("RankID"); }
        }
        protected string m_RankCode;
        [DBField("RankCode"), TextSearch, DBAESEncryptStringField, MaxLength(20, 10), Export(false), Required]
        public string RankCode
        {
            get { return m_RankCode; }
            set { m_RankCode = value; modify("RankCode"); }
        }

        protected string m_RankDesc;
        [DBField("RankDesc"), TextSearch, DBAESEncryptStringField, MaxLength(100, 25), Export(false), Required]
        public string RankDesc
        {
            get { return m_RankDesc; }
            set { m_RankDesc = value; modify("RankDesc"); }
        }
    }


    [DBClass("Position")]
    public class EPosition : BaseObject
    {
        public static DBManager db = new DBManager(typeof(EPosition));

        public static WFValueList VLPosition = new AppUtils.EncryptedDBCodeList(EPosition.db, "PositionID", new string[] { "PositionCode", "PositionDesc" }, " - ", "PositionCode");

        public static EPosition GetObject(DatabaseConnection dbConn, object ID)
        {
            if (ID is int)
            {
                EPosition obj = new EPosition();
                obj.PositionID = (int)ID;
                if (db.select(dbConn, obj))
                    return obj;
            }
            return null;
        }

        protected int m_PositionID;
        [DBField("PositionID", true, true), TextSearch, Export(false)]
        public int PositionID
        {
            get { return m_PositionID; }
            set { m_PositionID = value; modify("PositionID"); }
        }
        protected string m_PositionCode;
        [DBField("PositionCode"), TextSearch, DBAESEncryptStringField, MaxLength(20, 10), Export(false), Required]
        public string PositionCode
        {
            get { return m_PositionCode; }
            set { m_PositionCode = value; modify("PositionCode"); }
        }

        protected string m_PositionDesc;
        [DBField("PositionDesc"), TextSearch, DBAESEncryptStringField, MaxLength(100, 25), Export(false), Required]
        public string PositionDesc
        {
            get { return m_PositionDesc; }
            set { m_PositionDesc = value; modify("PositionDesc"); }
        }

        protected string m_PositionCapacity;
        [DBField("PositionCapacity"), TextSearch, DBAESEncryptStringField, MaxLength(40, 25), Export(false)]
        public string PositionCapacity
        {
            get { return m_PositionCapacity; }
            set { m_PositionCapacity = value; modify("PositionCapacity"); }
        }

    }

    [DBClass("EmploymentType")]
    public class EEmploymentType : BaseObject
    {
        public static DBManager db = new DBManager(typeof(EEmploymentType));
        public static WFValueList VLEmploymentType = new WFDBCodeList(EEmploymentType.db, "EmploymentTypeID", "EmploymentTypeCode", "EmploymentTypeDesc", "EmploymentTypeCode");

        protected int m_EmploymentTypeID;
        [DBField("EmploymentTypeID", true, true), TextSearch, Export(false)]
        public int EmploymentTypeID
        {
            get { return m_EmploymentTypeID; }
            set { m_EmploymentTypeID = value; modify("EmploymentTypeID"); }
        }
        protected string m_EmploymentTypeCode;
        [DBField("EmploymentTypeCode"), TextSearch, MaxLength(20,10), Export(false), Required]
        public string EmploymentTypeCode
        {
            get { return m_EmploymentTypeCode; }
            set { m_EmploymentTypeCode = value; modify("EmploymentTypeCode"); }
        }

        protected string m_EmploymentTypeDesc;
        [DBField("EmploymentTypeDesc"), TextSearch, MaxLength(100, 25), Export(false), Required]
        public string EmploymentTypeDesc
        {
            get { return m_EmploymentTypeDesc; }
            set { m_EmploymentTypeDesc = value; modify("EmploymentTypeDesc"); }
        }
    }

    [DBClass("StaffType")]
    public class EStaffType : BaseObject
    {
        public static DBManager db = new DBManager(typeof(EStaffType));
        public static WFValueList VLStaffType = new AppUtils.EncryptedDBCodeList(EStaffType.db, "StaffTypeID", new string[] { "StaffTypeCode", "StaffTypeDesc" }, " - ", "StaffTypeCode");

        public static EStaffType GetObject(DatabaseConnection dbConn, object ID)
        {
            if (ID is int)
            {
                EStaffType obj = new EStaffType();
                obj.StaffTypeID = (int)ID;
                if (db.select(dbConn, obj))
                    return obj;
            }
            return null;
        }
        
        protected int m_StaffTypeID;
        [DBField("StaffTypeID", true, true), TextSearch, Export(false)]
        public int StaffTypeID
        {
            get { return m_StaffTypeID; }
            set { m_StaffTypeID = value; modify("StaffTypeID"); }
        }
        protected string m_StaffTypeCode;
        [DBField("StaffTypeCode"), TextSearch, DBAESEncryptStringField, MaxLength(20, 10), Export(false), Required]
        public string StaffTypeCode
        {
            get { return m_StaffTypeCode; }
            set { m_StaffTypeCode = value; modify("StaffTypeCode"); }
        }

        protected string m_StaffTypeDesc;
        [DBField("StaffTypeDesc"), TextSearch, DBAESEncryptStringField, MaxLength(100, 25), Export(false), Required]
        public string StaffTypeDesc
        {
            get { return m_StaffTypeDesc; }
            set { m_StaffTypeDesc = value; modify("StaffTypeDesc"); }
        }


    }

    [DBClass("Qualification")]
    public class EQualification : BaseObject
    {
        public static DBManager db = new DBManager(typeof(EQualification));
        public static WFValueList VLQualification = new WFDBCodeList(EQualification.db, "QualificationID", "QualificationCode", "QualificationDesc", "QualificationCode");

        protected int m_QualificationID;
        [DBField("QualificationID", true, true), TextSearch, Export(false)]
        public int QualificationID
        {
            get { return m_QualificationID; }
            set { m_QualificationID = value; modify("QualificationID"); }
        }
        protected string m_QualificationCode;
        [DBField("QualificationCode"), TextSearch, MaxLength(20,10), Export(false), Required]
        public string QualificationCode
        {
            get { return m_QualificationCode; }
            set { m_QualificationCode = value; modify("QualificationCode"); }
        }

        protected string m_QualificationDesc;
        [DBField("QualificationDesc"), TextSearch, MaxLength(100,25), Export(false), Required]
        public string QualificationDesc
        {
            get { return m_QualificationDesc; }
            set { m_QualificationDesc = value; modify("QualificationDesc"); }
        }


    }


    [DBClass("CessationReason")]
    public class ECessationReason : BaseObject
    {
        public static DBManager db = new DBManager(typeof(ECessationReason));
        public static WFValueList VLCessationReason = new AppUtils.EncryptedDBCodeList(ECessationReason.db, "CessationReasonID", new string[] { "CessationReasonCode", "CessationReasonDesc" }, " - ", "CessationReasonCode");

        protected int m_CessationReasonID;
        [DBField("CessationReasonID", true, true), TextSearch, Export(false)]
        public int CessationReasonID
        {
            get { return m_CessationReasonID; }
            set { m_CessationReasonID = value; modify("CessationReasonID"); }
        }
        protected string m_CessationReasonCode;
        [DBField("CessationReasonCode"), TextSearch, DBAESEncryptStringField, MaxLength(20, 10), Export(false), Required]
        public string CessationReasonCode
        {
            get { return m_CessationReasonCode; }
            set { m_CessationReasonCode = value; modify("CessationReasonCode"); }
        }

        protected string m_CessationReasonDesc;
        [DBField("CessationReasonDesc"), TextSearch, DBAESEncryptStringField, MaxLength(100, 25), Export(false), Required]
        public string CessationReasonDesc
        {
            get { return m_CessationReasonDesc; }
            set { m_CessationReasonDesc = value; modify("CessationReasonDesc"); }
        }
        protected bool m_CessationReasonIsSeverancePay;
        [DBField("CessationReasonIsSeverancePay"), TextSearch, Export(false)]
        public bool CessationReasonIsSeverancePay
        {
            get { return m_CessationReasonIsSeverancePay; }
            set { m_CessationReasonIsSeverancePay = value; modify("CessationReasonIsSeverancePay"); }
        }
        protected bool m_CessationReasonIsLongServicePay;
        [DBField("CessationReasonIsLongServicePay"), TextSearch, Export(false)]
        public bool CessationReasonIsLongServicePay
        {
            get { return m_CessationReasonIsLongServicePay; }
            set { m_CessationReasonIsLongServicePay = value; modify("CessationReasonIsLongServicePay"); }
        }
        protected bool m_CessationReasonHasProrataYEB;
        [DBField("CessationReasonHasProrataYEB"), TextSearch, Export(false)]
        public bool CessationReasonHasProrataYEB
        {
            get { return m_CessationReasonHasProrataYEB; }
            set { m_CessationReasonHasProrataYEB = value; modify("CessationReasonHasProrataYEB"); }
        }
        
    }

    [DBClass("Skill")]
    public class ESkill : BaseObject
    {
        public static DBManager db = new DBManager(typeof(ESkill));
        public static WFValueList VLSkill = new WFDBCodeList(ESkill.db, "SkillID", "SkillCode", "SkillDesc", "SkillCode");

        protected int m_SkillID;
        [DBField("SkillID", true, true), TextSearch, Export(false)]
        public int SkillID
        {
            get { return m_SkillID; }
            set { m_SkillID = value; modify("SkillID"); }
        }
        protected string m_SkillCode;
        [DBField("SkillCode"), TextSearch, MaxLength(20,10), Export(false), Required]
        public string SkillCode
        {
            get { return m_SkillCode; }
            set { m_SkillCode = value; modify("SkillCode"); }
        }

        protected string m_SkillDesc;
        [DBField("SkillDesc"), TextSearch, MaxLength(100,25), Export(false), Required]
        public string SkillDesc
        {
            get { return m_SkillDesc; }
            set { m_SkillDesc = value; modify("SkillDesc"); }
        }


    }

    [DBClass("SkillLevel")]
    public class ESkillLevel : BaseObject
    {
        public static DBManager db = new DBManager(typeof(ESkillLevel));
        public static WFValueList VLSkillLevel = new WFDBCodeList(ESkillLevel.db, "SkillLevelID", "SkillLevelCode", "SkillLevelDesc", "SkillLevelCode");

        protected int m_SkillLevelID;
        [DBField("SkillLevelID", true, true), TextSearch, Export(false)]
        public int SkillLevelID
        {
            get { return m_SkillLevelID; }
            set { m_SkillLevelID = value; modify("SkillLevelID"); }
        }
        protected string m_SkillLevelCode;
        [DBField("SkillLevelCode"), TextSearch, MaxLength(20,10), Export(false), Required]
        public string SkillLevelCode
        {
            get { return m_SkillLevelCode; }
            set { m_SkillLevelCode = value; modify("SkillLevelCode"); }
        }

        protected string m_SkillLevelDesc;
        [DBField("SkillLevelDesc"), TextSearch, MaxLength(100,25), Export(false), Required]
        public string SkillLevelDesc
        {
            get { return m_SkillLevelDesc; }
            set { m_SkillLevelDesc = value; modify("SkillLevelDesc"); }
        }


    }


    [DBClass("StatutoryHoliday")]
    public class EStatutoryHoliday : BaseObject
    {
        public static DBManager db = new DBManager(typeof(EStatutoryHoliday));

        protected int m_StatutoryHolidayID;
        [DBField("StatutoryHolidayID",true,true), TextSearch, Export(false)]
        public int StatutoryHolidayID
        {
            get { return m_StatutoryHolidayID; }
            set { m_StatutoryHolidayID = value; modify("StatutoryHolidayID"); }
        }
        protected DateTime m_StatutoryHolidayDate;
        [DBField("StatutoryHolidayDate"), TextSearch, Export(false), Required]
        public DateTime StatutoryHolidayDate
        {
            get { return m_StatutoryHolidayDate; }
            set { m_StatutoryHolidayDate = value; modify("StatutoryHolidayDate"); }
        }


        protected string m_StatutoryHolidayDesc;
        [DBField("StatutoryHolidayDesc"), TextSearch, MaxLength(100,25), Export(false), Required]
        public string StatutoryHolidayDesc
        {
            get { return m_StatutoryHolidayDesc; }
            set { m_StatutoryHolidayDesc = value; modify("StatutoryHolidayDesc"); }
        }

        public static bool IsHoliday(DatabaseConnection dbConn, DateTime date)
        {
            DBFilter filter = new DBFilter();
            filter.add(new Match("StatutoryHolidayDate", date));

            if (db.count(dbConn, filter) > 0)
                return true;
            else
                return false;
        }
        public static bool IsHoliday(DatabaseConnection dbConn, DateTime date, out string HolidayDescription)
        {
            DBFilter filter = new DBFilter();
            filter.add(new Match("StatutoryHolidayDate", date));

            ArrayList holidayList = db.select(dbConn, filter);
            if (holidayList.Count > 0)
            {
                HolidayDescription = ((EStatutoryHoliday)holidayList[0]).StatutoryHolidayDesc;
                return true;
            }
            else
            {
                HolidayDescription = string.Empty;
                return false;
            }
        }
    }

    [DBClass("PublicHoliday")]
    public class EPublicHoliday : BaseObject
    {
        public static DBManager db = new DBManager(typeof(EPublicHoliday));

        protected int m_PublicHolidayID;
        [DBField("PublicHolidayID", true, true), TextSearch, Export(false)]
        public int PublicHolidayID
        {
            get { return m_PublicHolidayID; }
            set { m_PublicHolidayID = value; modify("PublicHolidayID"); }
        }
        protected DateTime m_PublicHolidayDate;
        [DBField("PublicHolidayDate"), TextSearch, Export(false), Required]
        public DateTime PublicHolidayDate
        {
            get { return m_PublicHolidayDate; }
            set { m_PublicHolidayDate = value; modify("PublicHolidayDate"); }
        }

        protected string m_PublicHolidayDesc;
        [DBField("PublicHolidayDesc"), TextSearch, MaxLength(100, 25), Export(false), Required]
        public string PublicHolidayDesc
        {
            get { return m_PublicHolidayDesc; }
            set { m_PublicHolidayDesc = value; modify("PublicHolidayDesc"); }
        }

        public static bool IsHoliday(DatabaseConnection dbConn, DateTime date)
        {
            DBFilter filter = new DBFilter();
            filter.add(new Match("PublicHolidayDate", date));

            if (db.count(dbConn, filter) > 0)
                return true;
            else
                return false;
        }

        public static bool IsHoliday(DatabaseConnection dbConn, DateTime date, out string HolidayDescription)
        {
            DBFilter filter = new DBFilter();
            filter.add(new Match("PublicHolidayDate", date));

            ArrayList holidayList = db.select(dbConn, filter);
            if (holidayList.Count > 0)
            {
                HolidayDescription = ((EPublicHoliday)holidayList[0]).PublicHolidayDesc;
                return true;
            }
            else
            {
                HolidayDescription = string.Empty;
                return false;
            }
        }
    }

    [DBClass("BankList")]
    public class EBankList : BaseObject
    {
        public static DBManager db = new DBManager(typeof(EBankList));
        public static WFValueList VLSkillLevel = new WFDBCodeList(db, "BankCode", "BankCode", "BankName", "BankCode");

        protected string m_BankCode;
        [DBField("BankCode", true, false), TextSearch, MaxLength(3, 3), Export(false), Required]
        public string BankCode
        {
            get { return m_BankCode; }
            set { m_BankCode = value; modify("BankCode"); }
        }

        protected string m_BankName;
        [DBField("BankName"), TextSearch, MaxLength(100, 25), Export(false), Required]
        public string BankName
        {
            get { return m_BankName; }
            set { m_BankName = value; modify("BankName"); }
        }


    }

    [DBClass("PermitType")]
    public class EPermitType : BaseObject
    {
        public static DBManager db = new DBManager(typeof(EPermitType));
        public static WFValueList VLPermitType = new WFDBCodeList(EPermitType.db, "PermitTypeID", "PermitTypeCode", "PermitTypeDesc", "PermitTypeCode");

        protected int m_PermitTypeID;
        [DBField("PermitTypeID", true, true), TextSearch, Export(false)]
        public int PermitTypeID
        {
            get { return m_PermitTypeID; }
            set { m_PermitTypeID = value; modify("PermitTypeID"); }
        }
        protected string m_PermitTypeCode;
        [DBField("PermitTypeCode"), TextSearch, MaxLength(20, 10), Export(false), Required]
        public string PermitTypeCode
        {
            get { return m_PermitTypeCode; }
            set { m_PermitTypeCode = value; modify("PermitTypeCode"); }
        }

        protected string m_PermitTypeDesc;
        [DBField("PermitTypeDesc"), TextSearch, MaxLength(100, 25), Export(false), Required]
        public string PermitTypeDesc
        {
            get { return m_PermitTypeDesc; }
            set { m_PermitTypeDesc = value; modify("PermitTypeDesc"); }
        }


    }

    [DBClass("DocumentType")]
    public class EDocumentType : BaseObject
    {
        public static DBManager db = new DBManager(typeof(EDocumentType));
        public static WFValueList VLDocumentType = new WFDBCodeList(EDocumentType.db, "DocumentTypeID", "DocumentTypeCode", "DocumentTypeDesc", "DocumentTypeCode");

        protected int m_DocumentTypeID;
        [DBField("DocumentTypeID", true, true), TextSearch, Export(false)]
        public int DocumentTypeID
        {
            get { return m_DocumentTypeID; }
            set { m_DocumentTypeID = value; modify("DocumentTypeID"); }
        }
        protected string m_DocumentTypeCode;
        [DBField("DocumentTypeCode"), TextSearch, MaxLength(20, 10), Export(false), Required]
        public string DocumentTypeCode
        {
            get { return m_DocumentTypeCode; }
            set { m_DocumentTypeCode = value; modify("DocumentTypeCode"); }
        }

        protected string m_DocumentTypeDesc;
        [DBField("DocumentTypeDesc"), TextSearch, MaxLength(100, 25), Export(false), Required]
        public string DocumentTypeDesc
        {
            get { return m_DocumentTypeDesc; }
            set { m_DocumentTypeDesc = value; modify("DocumentTypeDesc"); }
        }

        protected bool m_DocumentTypeIsSystem;
        [DBField("DocumentTypeIsSystem"), TextSearch, Export(false)]
        public bool DocumentTypeIsSystem
        {
            get { return m_DocumentTypeIsSystem; }
            set { m_DocumentTypeIsSystem = value; modify("DocumentTypeIsSystem"); }
        }


    }

    // Start 0000092, KuangWei, 2014-09-09
    [DBClass("IssueCountry")]
    public class EIssueCountry : BaseObject
    {
        public static DBManager db = new DBManager(typeof(EIssueCountry));
//        public static WFValueList VLCountry = new AppUtils.EncryptedDBCodeList(ECountry.db, "CountryID", new string[] { "CountryDesc" }, string.Empty, "CountryDesc");
        public static WFValueList VLCountry = new WFDBCodeList(EIssueCountry.db, "CountryID", "CountryCode", "CountryDesc", "CountryCode");

        public static EIssueCountry GetObject(DatabaseConnection dbConn, int ID)
        {
            EIssueCountry obj = new EIssueCountry();
            obj.CountryID = ID;
            if (db.select(dbConn, obj))
                return obj;
            return null; 
        }

        protected int m_CountryID;
        [DBField("CountryID", true, true), TextSearch, Export(false)]
        public int CountryID
        {
            get { return m_CountryID; }
            set { m_CountryID = value; modify("CountryID"); }
        }

        protected string m_CountryCode;
        [DBField("CountryCode"), TextSearch, MaxLength(10), Export(false), Required]
        public string CountryCode
        {
            get { return m_CountryCode; }
            set { m_CountryCode = value; modify("CountryCode"); }
        }

        protected string m_CountryDesc;
        [DBField("CountryDesc"), TextSearch, MaxLength(100, 25), Export(false), Required]
        public string CountryDesc
        {
            get { return m_CountryDesc; }
            set { m_CountryDesc = value; modify("CountryDesc"); }
        }
    }

    [DBClass("PlaceOfBirth")]
    public class EPlaceOfBirth : BaseObject
    {
        public static DBManager db = new DBManager(typeof(EPlaceOfBirth));
        // public static WFValueList VLPlaceOfBirth = new AppUtils.EncryptedDBCodeList(EPlaceOfBirth.db, "PlaceOfBirthID", new string[] { "PlaceOfBirthDesc" }, string.Empty, "PlaceOfBirthDesc");
        public static WFValueList VLPlaceOfBirth = new WFDBCodeList(EPlaceOfBirth.db, "PlaceOfBirthID", "PlaceOfBirthCode", "PlaceOfBirthDesc", "PlaceOfBirthCode");

        public static EPlaceOfBirth GetObject(DatabaseConnection dbConn, int ID)
        {
            EPlaceOfBirth obj = new EPlaceOfBirth();
            obj.PlaceOfBirthID = ID;
            if (db.select(dbConn, obj))
                return obj;
            return null; 
        }

        protected int m_PlaceOfBirthID;
        [DBField("PlaceOfBirthID", true, true), TextSearch, Export(false)]
        public int PlaceOfBirthID
        {
            get { return m_PlaceOfBirthID; }
            set { m_PlaceOfBirthID = value; modify("PlaceOfBirthID"); }
        }

        protected string m_PlaceOfBirthCode;
        [DBField("PlaceOfBirthCode"), TextSearch, MaxLength(20, 10), Export(false), Required]
        public string PlaceOfBirthCode
        {
            get { return m_PlaceOfBirthCode; }
            set { m_PlaceOfBirthCode = value; modify("PlaceOfBirthCode"); }
        }

        protected string m_PlaceOfBirthDesc;
        [DBField("PlaceOfBirthDesc"), TextSearch, MaxLength(100, 25), Export(false), Required]
        public string PlaceOfBirthDesc
        {
            get { return m_PlaceOfBirthDesc; }
            set { m_PlaceOfBirthDesc = value; modify("PlaceOfBirthDesc"); }
        }
    }

    [DBClass("Nationality")]
    public class ENationality : BaseObject
    {
        public static DBManager db = new DBManager(typeof(ENationality));       
        //public static WFValueList VLNationality = new AppUtils.EncryptedDBCodeList(ENationality.db, "NationalityID", new string[] { "NationalityDesc" }, string.Empty, "NationalityDesc");
        public static WFValueList VLNationality = new WFDBCodeList(ENationality.db, "NationalityID", "NationalityCode", "NationalityDesc", "NationalityCode");

        public static ENationality GetObject(DatabaseConnection dbConn, int ID)
        {
            ENationality obj = new ENationality();
            obj.NationalityID = ID;
            if (db.select(dbConn, obj))
                return obj;
            return null; 
        }

        protected int m_NationalityID;
        [DBField("NationalityID", true, true), TextSearch, Export(false)]
        public int NationalityID
        {
            get { return m_NationalityID; }
            set { m_NationalityID = value; modify("NationalityID"); }
        }
        protected string m_NationalityCode;
        [DBField("NationalityCode"), TextSearch, MaxLength(20, 10), Export(false), Required]
        public string NationalityCode
        {
            get { return m_NationalityCode; }
            set { m_NationalityCode = value; modify("NationalityCode"); }
        }

        protected string m_NationalityDesc;
        [DBField("NationalityDesc"), TextSearch, MaxLength(100, 25), Export(false), Required]
        public string NationalityDesc
        {
            get { return m_NationalityDesc; }
            set { m_NationalityDesc = value; modify("NationalityDesc"); }
        }
    }
    // End 0000092, KuangWei, 2014-09-09

}

