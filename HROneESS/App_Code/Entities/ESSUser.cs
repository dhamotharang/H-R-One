using System;
using System.Collections.Generic;
using System.Text;
using HROne.DataAccess;
//using perspectivemind.validation;
using HROne.Common;

namespace HROne.Lib.Entities
{
    [DBClass("EmpPersonalInfo")]
    public class EESSUser : BaseObject
    {
        public static DBManager db = new DBManager(typeof(EESSUser));
        public static WFValueList VLEmp = new WFDBList(EESSUser.db, "EmpID", "EmpNo", "EmpNo");
        public static WFValueList VLEmpStatus = new WFTextList(new string[] { "A", "T" }, new string[] { "Active", "Terminated" });

        public int m_EmpID;
        [DBField("EmpID", true, true), TextSearch, Export(false)]
        public int EmpID
        {
            get { return m_EmpID; }
            set { m_EmpID = value; modify("EmpID"); }
        }
        public string m_EmpNo;
        [DBField("EmpNo"), DBAESEncryptStringField, TextSearch, MaxLength(12, 25), Export(false)]
        public string EmpNo
        {
            get { return m_EmpNo; }
            set { m_EmpNo = value; modify("EmpNo"); }
        }
        public string m_EmpPW;
        [DBField("EmpPW"), TextSearch, MaxLength(40,50), Export(false)]
        public string EmpPW
        {
            get { return m_EmpPW; }
            set { m_EmpPW = value; modify("EmpPW"); }
        }
        public string m_EmpEngSurname;
        [DBField("EmpEngSurname"), DBAESEncryptStringField, TextSearch, MaxLength(20, 25), Export(false), Required]
        public string EmpEngSurname
        {
            get { return m_EmpEngSurname; }
            set { m_EmpEngSurname = value; modify("EmpEngSurname"); }
        }
        public string m_EmpEngOtherName;
        [DBField("EmpEngOtherName"), DBAESEncryptStringField, TextSearch, MaxLength(55, 25), Export(false), Required]
        public string EmpEngOtherName
        {
            get { return m_EmpEngOtherName; }
            set { m_EmpEngOtherName = value; modify("EmpEngOtherName"); }
        }
        protected string m_EmpAlias;
        [DBField("EmpAlias"), TextSearch, DBAESEncryptStringField, MaxLength(100, 25), Export(false)]
        public string EmpAlias
        {
            get { return m_EmpAlias; }
            set { m_EmpAlias = value; modify("EmpAlias"); }
        }
        public string m_EmpChiFullName;
        [DBField("EmpChiFullName"), DBAESEncryptStringField, TextSearch, MaxLength(50, 25), Export(false)]
        public string EmpChiFullName
        {
            get { return m_EmpChiFullName; }
            set { m_EmpChiFullName = value; modify("EmpChiFullName"); }
        }
 
        public string m_EmpInternalEmail;
        [DBField("EmpInternalEmail"), DBAESEncryptStringField, TextSearch, MaxLength(100), Export(false)]
        public string EmpInternalEmail
        {
            get { return m_EmpInternalEmail; }
            set { m_EmpInternalEmail = value; modify("EmpInternalEmail"); }
        }
        public string m_EmpHKID;
        [DBField("EmpHKID"), DBAESEncryptStringField(), TextSearch, MaxLength(12, 25), Export(false)]
        public string EmpHKID
        {
            get { return m_EmpHKID; }
            set { m_EmpHKID = value; modify("EmpHKID"); }
        }
        public string m_EmpPassportNo;
        [DBField("EmpPassportNo"), DBAESEncryptStringField(), TextSearch, MaxLength(40, 25), Export(false)]
        public string EmpPassportNo
        {
            get { return m_EmpPassportNo; }
            set { m_EmpPassportNo = value; modify("EmpPassportNo"); }
        }

        public string EmpEngFullName
        {
            get { string empFullName = m_EmpEngSurname + " " + m_EmpEngOtherName; return empFullName.Trim().Replace("  ", " "); }
        }

        public string EmpEngFullNameWithAlias
        {
            get { return EmpEngFullName + (string.IsNullOrEmpty(m_EmpAlias) ? string.Empty : (m_EmpAlias.Trim().Equals(string.Empty) ? string.Empty : " (" + m_EmpAlias + ")")); }
        }

        public string m_EmpESSLanguage;
        [DBField("EmpESSLanguage"), TextSearch, Export(false)]
        public string EmpESSLanguage
        {
            get { return m_EmpESSLanguage; }
            set { m_EmpESSLanguage = value; modify("EmpESSLanguage"); }
        }
    }
}
