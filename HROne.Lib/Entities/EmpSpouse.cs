using System;
using System.Collections.Generic;
using System.Text;
using HROne.DataAccess;
////using perspectivemind.validation;
using HROne.Common;

namespace HROne.Lib.Entities
{

    [DBClass("EmpSpouse")]
    public class EEmpSpouse : BaseObjectWithRecordInfo
    {
        public static DBManager db = new DBManagerWithRecordInfo(typeof(EEmpSpouse));

        protected int m_EmpSpouseID;
        [DBField("EmpSpouseID", true, true), TextSearch, Export(false)]
        public int EmpSpouseID
        {
            get { return m_EmpSpouseID; }
            set { m_EmpSpouseID = value; modify("EmpSpouseID"); }
        }


        protected int m_EmpID;
        [DBField("EmpID"), TextSearch, Export(false)]
        public int EmpID
        {
            get { return m_EmpID; }
            set { m_EmpID = value; modify("EmpID"); }
        }
        protected string m_EmpSpouseSurname;
        [DBField("EmpSpouseSurname"), DBAESEncryptStringField, TextSearch, MaxLength(20), Export(false), Required]
        public string EmpSpouseSurname
        {
            get { return m_EmpSpouseSurname; }
            set { m_EmpSpouseSurname = string.IsNullOrEmpty(value) ? value : value.Trim().Replace("  ", " "); modify("EmpSpouseSurname"); }
        }
        protected string m_EmpSpouseOtherName;
        [DBField("EmpSpouseOtherName"), DBAESEncryptStringField, TextSearch, MaxLength(40), Export(false), Required]
        public string EmpSpouseOtherName
        {
            get { return m_EmpSpouseOtherName; }
            set { m_EmpSpouseOtherName = string.IsNullOrEmpty(value) ? value : value.Trim().Replace("  ", " "); modify("EmpSpouseOtherName"); }
        }
        protected string m_EmpSpouseChineseName;
        [DBField("EmpSpouseChineseName"), DBAESEncryptStringField, TextSearch, MaxLength(50, 25), Export(false)]
        public string EmpSpouseChineseName
        {
            get { return m_EmpSpouseChineseName; }
            set { m_EmpSpouseChineseName = string.IsNullOrEmpty(value) ? value : value.Trim().Replace("  ", " "); modify("EmpSpouseChineseName"); }
        }
        protected DateTime m_EmpSpouseDateOfBirth;
        [DBField("EmpSpouseDateOfBirth"), TextSearch, MaxLength(10, 10), Export(false)]
        public DateTime EmpSpouseDateOfBirth
        {
            get { return m_EmpSpouseDateOfBirth; }
            set { m_EmpSpouseDateOfBirth = value; modify("EmpSpouseDateOfBirth"); }
        }
        protected string m_EmpSpouseHKID;
        [DBField("EmpSpouseHKID"), DBAESEncryptStringField(), TextSearch, MaxLength(12), Export(false)]
        public string EmpSpouseHKID
        {
            get { return m_EmpSpouseHKID; }
            set { m_EmpSpouseHKID = value; modify("EmpSpouseHKID"); }
        }
        protected string m_EmpSpousePassportNo;
        [DBField("EmpSpousePassportNo"), DBAESEncryptStringField(), TextSearch, MaxLength(40), Export(false)]
        public string EmpSpousePassportNo
        {
            get { return m_EmpSpousePassportNo; }
            set { m_EmpSpousePassportNo = value; modify("EmpSpousePassportNo"); }
        }
        protected string m_EmpSpousePassportIssuedCountry;
        [DBField("EmpSpousePassportIssuedCountry"), DBAESEncryptStringField, TextSearch, MaxLength(40), Export(false)]
        public string EmpSpousePassportIssuedCountry
        {
            get { return m_EmpSpousePassportIssuedCountry; }
            set { m_EmpSpousePassportIssuedCountry = value; modify("EmpSpousePassportIssuedCountry"); }
        }
        // Start 0000142, KuangWei, 2014-12-20
        protected string m_EmpGender;
        // Start 0000188, Ricky So, 2015-04-16
        //[DBField("EmpGender"), DBAESEncryptStringField, TextSearch, Export(false), Required]
        [DBField("EmpGender"), DBAESEncryptStringField, TextSearch, Export(false)]
        // End 0000188, Ricky So, 2015-04-16
        public string EmpGender
        {
            get { return m_EmpGender; }
            set { m_EmpGender = value; modify("EmpGender"); }
        }

        protected bool m_EmpIsMedicalSchemaInsured;
        [DBField("EmpIsMedicalSchemaInsured"), TextSearch, Export(false)]
        public bool EmpIsMedicalSchemaInsured
        {
            get { return m_EmpIsMedicalSchemaInsured; }
            set { m_EmpIsMedicalSchemaInsured = value; modify("EmpIsMedicalSchemaInsured"); }
        }

        protected DateTime m_EmpMedicalEffectiveDate;
        [DBField("EmpMedicalEffectiveDate"), TextSearch, Export(false)]
        public DateTime EmpMedicalEffectiveDate
        {
            get { return m_EmpMedicalEffectiveDate; }
            set { m_EmpMedicalEffectiveDate = value; modify("EmpMedicalEffectiveDate"); }
        }

        protected DateTime m_EmpMedicalExpiryDate;
        [DBField("EmpMedicalExpiryDate"), TextSearch, Export(false)]
        public DateTime EmpMedicalExpiryDate
        {
            get { return m_EmpMedicalExpiryDate; }
            set { m_EmpMedicalExpiryDate = value; modify("EmpMedicalExpiryDate"); }
        }
        // End 0000142, KuangWei, 2014-12-20

        //  For Synchronize Use only
        protected string m_SynID;
        [DBField("SynID"), TextSearch, Export(false)]
        public string SynID
        {
            get { return m_SynID; }
            set { m_SynID = value; modify("SynID"); }
        }
    }
}
