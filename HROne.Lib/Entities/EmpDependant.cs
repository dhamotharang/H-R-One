using System;
using System.Collections.Generic;
using System.Text;
using HROne.DataAccess;
////using perspectivemind.validation;
using HROne.Common;

namespace HROne.Lib.Entities
{
    [DBClass("EmpDependant")]
    public class EEmpDependant : BaseObjectWithRecordInfo
    {
        public static DBManager db = new DBManagerWithRecordInfo(typeof(EEmpDependant));


        protected int m_EmpDependantID;
        [DBField("EmpDependantID", true, true), TextSearch, Export(false)]
        public int EmpDependantID
        {
            get { return m_EmpDependantID; }
            set { m_EmpDependantID = value; modify("EmpDependantID"); }
        }
        protected int m_EmpID;
        [DBField("EmpID"), TextSearch, Export(false)]
        public int EmpID
        {
            get { return m_EmpID; }
            set { m_EmpID = value; modify("EmpID"); }
        }
        protected string m_EmpDependantSurname;
        [DBField("EmpDependantSurname"), DBAESEncryptStringField, TextSearch, MaxLength(20), Export(false), Required]
        public string EmpDependantSurname
        {
            get { return m_EmpDependantSurname; }
            set { m_EmpDependantSurname = string.IsNullOrEmpty(value) ? value : value.Trim().Replace("  ", " "); modify("EmpDependantSurname"); }
        }
        protected string m_EmpDependantOtherName;
        [DBField("EmpDependantOtherName"), DBAESEncryptStringField, TextSearch, MaxLength(40), Export(false), Required]
        public string EmpDependantOtherName
        {
            get { return m_EmpDependantOtherName; }
            set { m_EmpDependantOtherName = string.IsNullOrEmpty(value) ? value : value.Trim().Replace("  ", " "); modify("EmpDependantOtherName"); }
        }
        protected string m_EmpDependantChineseName;
        [DBField("EmpDependantChineseName"), DBAESEncryptStringField, TextSearch, MaxLength(50, 25), Export(false)]
        public string EmpDependantChineseName
        {
            get { return m_EmpDependantChineseName; }
            set { m_EmpDependantChineseName = string.IsNullOrEmpty(value) ? value : value.Trim().Replace("  ", " "); modify("EmpDependantChineseName"); }
        }
        protected DateTime m_EmpDependantDateOfBirth;
        [DBField("EmpDependantDateOfBirth"), TextSearch,MaxLength(10,10), Export(false)]
        public DateTime EmpDependantDateOfBirth
        {
            get { return m_EmpDependantDateOfBirth; }
            set { m_EmpDependantDateOfBirth = value; modify("EmpDependantDateOfBirth"); }
        }
        
        protected string m_EmpDependantGender;
        [DBField("EmpDependantGender"), DBAESEncryptStringField, TextSearch, Export(false)]
        public string EmpDependantGender
        {
            get { return m_EmpDependantGender; }
            set { m_EmpDependantGender = value; modify("EmpDependantGender"); }
        }
        protected string m_EmpDependantHKID;
        [DBField("EmpDependantHKID"), DBAESEncryptStringField(), TextSearch, MaxLength(12, 12), Export(false)]
        public string EmpDependantHKID
        {
            get { return m_EmpDependantHKID; }
            set { m_EmpDependantHKID = value; modify("EmpDependantHKID"); }
        }
        protected string m_EmpDependantPassportNo;
        [DBField("EmpDependantPassportNo"), DBAESEncryptStringField(), TextSearch, MaxLength(40), Export(false)]
        public string EmpDependantPassportNo
        {
            get { return m_EmpDependantPassportNo; }
            set { m_EmpDependantPassportNo = value; modify("EmpDependantPassportNo"); }
        }
        protected string m_EmpDependantPassportIssuedCountry;
        [DBField("EmpDependantPassportIssuedCountry"), DBAESEncryptStringField, TextSearch, MaxLength(40), Export(false)]
        public string EmpDependantPassportIssuedCountry
        {
            get { return m_EmpDependantPassportIssuedCountry; }
            set { m_EmpDependantPassportIssuedCountry = value; modify("EmpDependantPassportIssuedCountry"); }
        }
        protected string m_EmpDependantRelationship;
        [DBField("EmpDependantRelationship"), DBAESEncryptStringField, TextSearch, MaxLength(100, 40), Export(false), Required]
        public string EmpDependantRelationship
        {
            get { return m_EmpDependantRelationship; }
            set { m_EmpDependantRelationship = value; modify("EmpDependantRelationship"); }
        }

        //  For Synchronize Use only
        protected string m_SynID;
        [DBField("SynID"), TextSearch, Export(false)]
        public string SynID
        {
            get { return m_SynID; }
            set { m_SynID = value; modify("SynID"); }
        }

        //Start 0000190, Miranda, 2015-04-30
        protected bool m_EmpDependantMedicalSchemeInsured;
        [DBField("EmpDependantMedicalSchemeInsured"), TextSearch, Export(false), Required]
        public bool EmpDependantMedicalSchemeInsured
        {
            get { return m_EmpDependantMedicalSchemeInsured; }
            set { m_EmpDependantMedicalSchemeInsured = value; modify("EmpDependantMedicalSchemeInsured"); }
        }
        protected DateTime m_EmpDependantMedicalEffectiveDate;
        [DBField("EmpDependantMedicalEffectiveDate"), TextSearch, MaxLength(10, 10), Export(false)]
        public DateTime EmpDependantMedicalEffectiveDate
        {
            get { return m_EmpDependantMedicalEffectiveDate; }
            set { m_EmpDependantMedicalEffectiveDate = value; modify("EmpDependantMedicalEffectiveDate"); }
        }
        protected DateTime m_EmpDependantExpiryDate;
        [DBField("EmpDependantExpiryDate"), TextSearch, MaxLength(10, 10), Export(false)]
        public DateTime EmpDependantExpiryDate
        {
            get { return m_EmpDependantExpiryDate; }
            set { m_EmpDependantExpiryDate = value; modify("EmpDependantExpiryDate"); }
        }
        //End 0000190, Miranda, 2015-04-30
    }
}
