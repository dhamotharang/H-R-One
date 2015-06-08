using System;
using System.Collections;
using System.Text;
using HROne.DataAccess;
////using perspectivemind.validation;
using HROne.Common;

namespace HROne.Lib.Entities
{
    [DBClass("EmpPersonalInfo")]
    public class EEmpPersonalInfo : BaseObjectWithRecordInfo 
    {
        public static DBManager db = new DBManagerWithRecordInfo(typeof(EEmpPersonalInfo));
        public static WFValueList VLEmp = new WFDBList(EEmpPersonalInfo.db, "EmpID", "EmpNo", "EmpNo");
        public static WFValueList VLEmpStatus = new AppUtils.NewWFTextList(new string[] { "A", "T" }, new string[] { "ACTIVE_STAFF", "Terminated"});

        public static EEmpPersonalInfo GetObject(DatabaseConnection dbConn, int EmpID)
        {
            EEmpPersonalInfo m_obj = new EEmpPersonalInfo();
            m_obj.EmpID = EmpID;

            if (EEmpPersonalInfo.db.select(dbConn, m_obj))
                return m_obj;
            else
                return null;
        }

        protected int m_EmpID;
        [DBField("EmpID", true, true), TextSearch, Export(false)]
        public int EmpID
        {
            get { return m_EmpID; }
            set { m_EmpID = value; modify("EmpID"); }
        }
        protected string m_EmpNo;
        [DBField("EmpNo"), TextSearch, DBAESEncryptStringField, MaxLength(20), Export(false), Required]
        public string EmpNo
        {
            get { return m_EmpNo; }
            set { m_EmpNo = string.IsNullOrEmpty(value) ? value : value.Trim().ToUpper(); modify("EmpNo"); }
        }
        protected string m_EmpStatus;
        [DBField("EmpStatus"), TextSearch, Export(false)]
        public string EmpStatus
        {
            get { return m_EmpStatus; }
            set { m_EmpStatus = value; modify("EmpStatus"); }
        }
        protected string m_EmpEngSurname;
        [DBField("EmpEngSurname"), TextSearch, DBAESEncryptStringField, MaxLength(20), Export(false), Required]
        public string EmpEngSurname
        {
            get { return m_EmpEngSurname; }
            set { m_EmpEngSurname = string.IsNullOrEmpty(value) ? value : value.Trim(); modify("EmpEngSurname"); }
        }
        protected string m_EmpEngOtherName;
        [DBField("EmpEngOtherName"), TextSearch, DBAESEncryptStringField, MaxLength(55, 40), Export(false)]
        public string EmpEngOtherName
        {
            get { return m_EmpEngOtherName; }
            set { m_EmpEngOtherName = string.IsNullOrEmpty(value) ? value : value.Trim().Replace("  ", " "); modify("EmpEngOtherName"); }
        }
        protected string m_EmpChiFullName;
        [DBField("EmpChiFullName"), TextSearch, DBAESEncryptStringField, MaxLength(50, 40), Export(false)]
        public string EmpChiFullName
        {
            get { return m_EmpChiFullName; }
            set { m_EmpChiFullName = string.IsNullOrEmpty(value) ? value : value.Trim(); modify("EmpChiFullName"); }
        }
        protected string m_EmpAlias;
        [DBField("EmpAlias"), TextSearch, DBAESEncryptStringField, MaxLength(100, 40), Export(false)]
        public string EmpAlias
        {
            get { return m_EmpAlias; }
            set { m_EmpAlias = string.IsNullOrEmpty(value) ? value : value.Trim(); modify("EmpAlias"); }
        }
        protected string m_EmpHKID;
        [DBField("EmpHKID"), DBAESEncryptStringField, TextSearch, MaxLength(12), Export(false)]
        public string EmpHKID
        {
            get { return m_EmpHKID; }
            set { m_EmpHKID = value; modify("EmpHKID"); }
        }

        public string MaskedEmpHKID
        {
            get 
            {
                int midPos = m_EmpHKID.Length / 2;
                if (midPos > 1) midPos--;
                string maskedHKID = m_EmpHKID.Substring(0, midPos);
                for (int i = midPos; i < m_EmpHKID.Length; i++)
                {
                    char ch = m_EmpHKID.ToCharArray()[i];
                    if (ch >= '0' && ch <= '9' || ch >= 'A' && ch <= 'Z' || ch >= 'a' && ch <= 'z')
                        maskedHKID += "*";
                    else
                        maskedHKID += ch.ToString();

                }
                return maskedHKID; 
            }
        }

        protected string m_EmpGender;
        [DBField("EmpGender"), DBAESEncryptStringField, TextSearch, Export(false), Required]
        public string EmpGender
        {
            get { return m_EmpGender; }
            set { m_EmpGender = value; modify("EmpGender"); }
        }
        protected string m_EmpMaritalStatus;
        [DBField("EmpMaritalStatus"), DBAESEncryptStringField, TextSearch, Export(false), Required]
        public string EmpMaritalStatus
        {
            get { return m_EmpMaritalStatus; }
            set { m_EmpMaritalStatus = value; modify("EmpMaritalStatus"); }
        }
        protected DateTime m_EmpDateOfBirth;
        [DBField("EmpDateOfBirth"), TextSearch, Export(false), Required]
        public DateTime EmpDateOfBirth
        {
            get { return m_EmpDateOfBirth; }
            set { m_EmpDateOfBirth = value; modify("EmpDateOfBirth"); }
        }
        protected string m_EmpPlaceOfBirth;
        [DBField("EmpPlaceOfBirth"), DBAESEncryptStringField, TextSearch, MaxLength(100, 40), Export(false)]
        public string EmpPlaceOfBirth
        {
            get { return m_EmpPlaceOfBirth; }
            set { m_EmpPlaceOfBirth = value; modify("EmpPlaceOfBirth"); }
        }
        protected string m_EmpNationality;
        [DBField("EmpNationality"), DBAESEncryptStringField, TextSearch, MaxLength(100, 40), Export(false)]
        public string EmpNationality
        {
            get { return m_EmpNationality; }
            set { m_EmpNationality = value; modify("EmpNationality"); }
        }
        protected string m_EmpPassportNo;
        [DBField("EmpPassportNo"), DBAESEncryptStringField, TextSearch, MaxLength(40, 25), Export(false)]
        public string EmpPassportNo
        {
                get { return m_EmpPassportNo; }
            set { m_EmpPassportNo = value; modify("EmpPassportNo"); }
        }
        protected string m_EmpPassportIssuedCountry;
        [DBField("EmpPassportIssuedCountry"), DBAESEncryptStringField, TextSearch, MaxLength(40, 40), Export(false)]
        public string EmpPassportIssuedCountry
        {
            get { return m_EmpPassportIssuedCountry; }
            set { m_EmpPassportIssuedCountry = value; modify("EmpPassportIssuedCountry"); }
        }
        protected DateTime m_EmpPassportExpiryDate;
        [DBField("EmpPassportExpiryDate"), TextSearch, Export(false)]
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
        [DBField("EmpResAddrAreaCode"), DBAESEncryptStringField, TextSearch, Export(false), Required]
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
        protected DateTime m_EmpDateOfJoin;
        [DBField("EmpDateOfJoin"), TextSearch, Export(false), Required]
        public DateTime EmpDateOfJoin
        {
            get { return m_EmpDateOfJoin; }
            set { m_EmpDateOfJoin = value; modify("EmpDateOfJoin"); }
        }
        protected DateTime m_EmpServiceDate;
        [DBField("EmpServiceDate"), TextSearch, Export(false), Required]
        public DateTime EmpServiceDate
        {
            get { return m_EmpServiceDate; }
            set { m_EmpServiceDate = value; modify("EmpServiceDate"); }
        }
        protected DateTime m_EmpNextSalaryIncrementDate;
        [DBField("EmpNextSalaryIncrementDate"), TextSearch, Export(false)]
        public DateTime EmpNextSalaryIncrementDate
        {
            get { return m_EmpNextSalaryIncrementDate; }
            set { m_EmpNextSalaryIncrementDate = value; modify("EmpNextSalaryIncrementDate"); }
        }
        protected int m_EmpNoticePeriod;
        [DBField("EmpNoticePeriod"), TextSearch, MaxLength(3), Export(false)]
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
        [DBField("EmpProbaPeriod"), TextSearch, MaxLength(3), Export(false)]
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
        [DBField("EmpProbaLastDate"), TextSearch, Export(false)]
        public DateTime EmpProbaLastDate
        {
            get 
            {
                if ((m_EmpProbaLastDate.Ticks.Equals(0) || m_EmpDateOfJoin > m_EmpProbaLastDate) && !m_EmpDateOfJoin.Ticks.Equals(0))
                {
                    DateTime tmpProbaLastDate = new DateTime();
                    if (EmpProbaPeriod > 0)
                    {
                        if (EmpProbaUnit.Equals("D"))
                            tmpProbaLastDate = m_EmpDateOfJoin.AddDays(EmpProbaPeriod).AddDays(-1);
                        else if (EmpProbaUnit.Equals("M"))
                            tmpProbaLastDate = m_EmpDateOfJoin.AddMonths(EmpProbaPeriod).AddDays(-1);
                    }
                    return tmpProbaLastDate;

                }
                else
                    return m_EmpProbaLastDate;
            }
            set { m_EmpProbaLastDate = value; modify("EmpProbaLastDate"); }
        }
        protected string m_EmpEmail = string.Empty;
        [DBField("EmpEmail"), DBAESEncryptStringField, TextSearch, MaxLength(100, 40), Export(false)]
        public string EmpEmail
        {
            get { return m_EmpEmail; }
            set { m_EmpEmail = value; modify("EmpEmail"); }
        }
        protected string m_EmpHomePhoneNo;
        [DBField("EmpHomePhoneNo"), DBAESEncryptStringField, TextSearch, MaxLength(100, 40), Export(false)]
        public string EmpHomePhoneNo
        {
            get { return m_EmpHomePhoneNo; }
            set { m_EmpHomePhoneNo = value; modify("EmpHomePhoneNo"); }
        }
        protected string m_EmpMobileNo;
        [DBField("EmpMobileNo"), DBAESEncryptStringField, TextSearch, MaxLength(100, 40), Export(false)]
        public string EmpMobileNo
        {
            get { return m_EmpMobileNo; }
            set { m_EmpMobileNo = value; modify("EmpMobileNo"); }
        }
        protected string m_EmpOfficePhoneNo;
        [DBField("EmpOfficePhoneNo"), DBAESEncryptStringField, TextSearch, MaxLength(100, 40), Export(false)]
        public string EmpOfficePhoneNo
        {
            get { return m_EmpOfficePhoneNo; }
            set { m_EmpOfficePhoneNo = value; modify("EmpOfficePhoneNo"); }
        }
        protected string m_Remark;
        [DBField("Remark"), DBAESEncryptStringField, TextSearch, Export(false)]
        public string Remark
        {
            get { return m_Remark; }
            set { m_Remark = value; modify("Remark"); }
        }
        protected int m_PreviousEmpID;
        [DBField("PreviousEmpID"), TextSearch, Export(false)]
        public int PreviousEmpID
        {
            get { return m_PreviousEmpID; }
            set { m_PreviousEmpID = value; modify("PreviousEmpID"); }
        }
        protected string m_EmpInternalEmail = string.Empty;
        [DBField("EmpInternalEmail"), DBAESEncryptStringField, MaxLength(100, 40), TextSearch, Export(false)]
        public string EmpInternalEmail
        {
            get { return m_EmpInternalEmail; }
            set { m_EmpInternalEmail = value; modify("EmpInternalEmail"); }
        }
        protected string m_EmpPW;
        [DBField("EmpPW"), TextSearch, MaxLength(40), Export(false)]
        public string EmpPW
        {
            get { return m_EmpPW; }
            set { m_EmpPW = value; modify("EmpPW"); }
        }
        protected string m_EmpTimeCardNo;
        [DBField("EmpTimeCardNo"), TextSearch, MaxLength(20), Export(false)]
        public string EmpTimeCardNo
        {
            get { return m_EmpTimeCardNo; }
            set { m_EmpTimeCardNo = value; modify("EmpTimeCardNo"); }
        }
        // Start 0000067, Miranda, 2014-08-07
        protected DateTime m_EmpOriginalHireDate;
        [DBField("EmpOriginalHireDate"), TextSearch, Export(false)]
        public DateTime EmpOriginalHireDate
        {
            get { return m_EmpOriginalHireDate; }
            set { m_EmpOriginalHireDate = value; modify("EmpOriginalHireDate"); }
        }
        // End 0000067, Miranda, 2014-08-07

        protected bool m_EmpIsOverrideMinimumWage;
        [DBField("EmpIsOverrideMinimumWage"), TextSearch, Export(false)]
        public bool EmpIsOverrideMinimumWage
        {
            get { return m_EmpIsOverrideMinimumWage; }
            set { m_EmpIsOverrideMinimumWage = value; modify("EmpIsOverrideMinimumWage"); }
        }

        protected double m_EmpOverrideMinimumHourlyRate;
        [DBField("EmpOverrideMinimumHourlyRate", "0.00"), TextSearch, MaxLength(6), Export(false)]
        public double EmpOverrideMinimumHourlyRate
        {
            get { return m_EmpOverrideMinimumHourlyRate; }
            set { m_EmpOverrideMinimumHourlyRate = value; modify("EmpOverrideMinimumHourlyRate"); }
        }

        protected int m_MasterEmpID;
        [DBField("MasterEmpID"), TextSearch, Export(false)]
        public int MasterEmpID
        {
            get { return m_MasterEmpID; }
            set { m_MasterEmpID = value; modify("MasterEmpID"); }
        }

        protected bool m_EmpIsCombinePaySlip;
        [DBField("EmpIsCombinePaySlip"), TextSearch, Export(false)]
        public bool EmpIsCombinePaySlip
        {
            get { return m_EmpIsCombinePaySlip; }
            set { m_EmpIsCombinePaySlip = value; modify("EmpIsCombinePaySlip"); }
        }

        protected bool m_EmpIsCombineMPF;
        [DBField("EmpIsCombineMPF"), TextSearch, Export(false)]
        public bool EmpIsCombineMPF
        {
            get { return m_EmpIsCombineMPF; }
            set { m_EmpIsCombineMPF = value; modify("EmpIsCombineMPF"); }
        }

        protected bool m_EmpIsCombineTax;
        [DBField("EmpIsCombineTax"), TextSearch, Export(false)]
        public bool EmpIsCombineTax
        {
            get { return m_EmpIsCombineTax; }
            set { m_EmpIsCombineTax = value; modify("EmpIsCombineTax"); }
        }

        //  For Synchronize Use only
        protected string m_SynID;
        [DBField("SynID"), TextSearch, Export(false)]
        public string SynID
        {
            get { return m_SynID; }
            set { m_SynID = value; modify("SynID"); }
        }


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

        protected string m_EmpESSLanguage;
        [DBField("EmpESSLanguage"), TextSearch, Export(false)]
        public string EmpESSLanguage
        {
            get { return m_EmpESSLanguage; }
            set { m_EmpESSLanguage = value; modify("EmpESSLanguage"); }
        }

        public string EmpEngFullName
        {
            get 
            { 
                string empFullName = m_EmpEngSurname + " " + m_EmpEngOtherName;
                return empFullName.Replace("\r", " ").Replace("\n", " ").Trim().Replace("  ", " ");
            }
        }

        public string EmpEngFullNameWithAlias
        {
            get 
            {
                string tmpAlias = m_EmpAlias;
                if (!string.IsNullOrEmpty(m_EmpAlias))
                {
                    tmpAlias = tmpAlias.Replace("\r", " ").Replace("\n", " ").Trim().Replace("  ", " ");
                }
                return EmpEngFullName + (string.IsNullOrEmpty(tmpAlias) ? string.Empty : " (" + tmpAlias + ")"); 
            }
        }

        public string EmpEngDisplayName
        {
            get
            {
                if (!string.IsNullOrEmpty(m_EmpAlias))
                {
                    string tmpAlias = m_EmpAlias;
                    tmpAlias = tmpAlias.Replace("\r", " ").Replace("\n", " ").Trim().Replace("  ", " ");
                    if (!string.IsNullOrEmpty(tmpAlias))
                        return (tmpAlias + " " + m_EmpEngSurname).Replace("\r", " ").Replace("\n", " ").Trim().Replace("  ", " ");
                }
                return EmpEngFullName;
            }
        }

        public EEmpPersonalInfo GetPreviousEmpInfo(DatabaseConnection dbConn)
        {
            DBFilter empTermFilter = new DBFilter();
            empTermFilter.add(new Match("NewEmpID", m_EmpID));

            DBFilter empFilter = new DBFilter();
            empFilter.add(new IN("EmpID", "Select EmpID FROM " + EEmpTermination.db.dbclass.tableName, empTermFilter));

            ArrayList empList = EEmpPersonalInfo.db.select(dbConn, empFilter);
            if (empList.Count > 0)
                return (EEmpPersonalInfo)empList[0];
            else
                return null;
        }

        public ArrayList GetOtherRoleList(DatabaseConnection dbConn)
        {
            return GetOtherRoleList(dbConn, RoleFilterOptionEnum.None);
        }

        public ArrayList GetOtherRoleList(DatabaseConnection dbConn, RoleFilterOptionEnum roleFilterOptionEnum)
        {
            DBFilter masterEmpIDFilter = new DBFilter();
            masterEmpIDFilter.add(new Match("EmpID", "<>", m_EmpID));
            OR orMasterEmpIDTerm = new OR();
            orMasterEmpIDTerm.add(new Match("MasterEmpID", m_EmpID));
            if (m_MasterEmpID > 0)
                orMasterEmpIDTerm.add(new Match("MasterEmpID", m_MasterEmpID));

            masterEmpIDFilter.add(orMasterEmpIDTerm);

            if (roleFilterOptionEnum == RoleFilterOptionEnum.MPF)
                masterEmpIDFilter.add(new Match("EmpIsCombineMPF", true));
            if (roleFilterOptionEnum == RoleFilterOptionEnum.Payslip)
                masterEmpIDFilter.add(new Match("EmpIsCombinePaySlip", true));
            if (roleFilterOptionEnum == RoleFilterOptionEnum.Tax)
                masterEmpIDFilter.add(new Match("EmpIsCombineTax", true));

            return EEmpPersonalInfo.db.select(dbConn, masterEmpIDFilter);
        }

        public enum RoleFilterOptionEnum
        {
            None = 0,
            Payslip = 1,
            MPF = 2,
            Tax = 3
        }

        public DBTerm GetAllRoleEmpIDTerms(DatabaseConnection dbConn, string EmpIDFieldName, RoleFilterOptionEnum roleFilterOptionEnum )
        {
            System.Collections.Generic.List<string> EmpIDGroupList = new System.Collections.Generic.List<string>();
            EmpIDGroupList.Add(m_EmpID.ToString());
            //if (m_MasterEmpID > 0)
            //    EmpIDGroupList.Add(MasterEmpID.ToString());

            ArrayList list = GetOtherRoleList(dbConn, roleFilterOptionEnum);

            foreach (EEmpPersonalInfo tmpEmpInfo in list)
                if (!EmpIDGroupList.Contains(tmpEmpInfo.EmpID.ToString()))
                {
                    //if (roleFilterOptionEnum == RoleFilterOptionEnum.MPF && !tmpEmpInfo.EmpIsCombineMPF)
                    //    continue;
                    //if (roleFilterOptionEnum == RoleFilterOptionEnum.Payslip && !tmpEmpInfo.EmpIsCombinePaySlip)
                    //    continue;
                    //if (roleFilterOptionEnum == RoleFilterOptionEnum.Tax && !tmpEmpInfo.EmpIsCombineTax)
                    //    continue;
                    EmpIDGroupList.Add(tmpEmpInfo.EmpID.ToString());
                }

            return new IN(EmpIDFieldName, string.Join(",", EmpIDGroupList.ToArray()), null);
        }

        public double GetAge(DatabaseConnection dbConn, DateTime AsOfDate)
        {
            if (string.IsNullOrEmpty(m_EmpNo))
            {
                EEmpPersonalInfo dummyEmpInfo = new EEmpPersonalInfo();
                if (dummyEmpInfo.LoadDBObject(dbConn, EmpID))
                    return dummyEmpInfo.GetAge(dbConn, AsOfDate);
                else
                    return 0;
            }
            if (this.EmpDateOfBirth.Ticks.Equals(0))
                return 0;
            DateTime tmpDateOfBirth = this.EmpDateOfBirth;

            return HROne.CommonLib.Utility.YearDifference(tmpDateOfBirth, AsOfDate.AddDays(-1));

        }


        public double GetYearOfServer(DatabaseConnection dbConn, DateTime AsOfDate)
        {
            if (string.IsNullOrEmpty(m_EmpNo))
            {
                EEmpPersonalInfo dummyEmpInfo = new EEmpPersonalInfo();
                if (dummyEmpInfo.LoadDBObject(dbConn, EmpID))
                    return dummyEmpInfo.GetYearOfServer(dbConn, AsOfDate);
                else
                    return 0;
            }
            if (this.EmpServiceDate.Ticks.Equals(0))
                return 0;
            DateTime tmpYearOfService = this.EmpServiceDate;

            if (tmpYearOfService > AsOfDate)
                return 0;

            EEmpTermination empTerm = EEmpTermination.GetObjectByEmpID(dbConn, m_EmpID);
            if (empTerm != null)
                if (empTerm.EmpTermLastDate < AsOfDate)
                    AsOfDate = empTerm.EmpTermLastDate;
            return HROne.CommonLib.Utility.YearDifference(tmpYearOfService, AsOfDate);

        }

        protected override void InsertAuditTrailDetail(DatabaseConnectionWithAudit dbConnWithAudit, EAuditTrailDetail d)
        {
            EAuditTrail t = dbConnWithAudit.CurAuditTrail;
            if (t != null)
            {
                if (t.AuditTrailID <= 0 && t.GetParentAuditTrail(dbConnWithAudit) != null)
                    t.EmpID = EmpID;
            }
            base.InsertAuditTrailDetail(dbConnWithAudit, d);
        }
    }

}
