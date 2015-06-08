using System;
using System.Collections.Generic;
using System.Text;
using HROne.DataAccess;
////using perspectivemind.validation;
using HROne.Common;

namespace HROne.Lib.Entities
{
    [DBClass("RequestEmpPersonalInfo")]
    public class ERequestEmpPersonalInfo : BaseObject
    {
        public static DBManager db = new DBManager(typeof(ERequestEmpPersonalInfo));

        protected int m_RequestEmpID;
        [DBField("RequestEmpID", true, true), TextSearch, Export(false)]
        public int RequestEmpID
        {
            get { return m_RequestEmpID; }
            set { m_RequestEmpID = value; modify("RequestEmpID"); }
        }
        protected int m_EmpID;
        [DBField("EmpID"), TextSearch, Export(false),Required]
        public int EmpID
        {
            get { return m_EmpID; }
            set { m_EmpID = value; modify("EmpID"); }
        }
        protected string m_RequestEmpAlias;
        [DBField("RequestEmpAlias"), TextSearch, DBAESEncryptStringField, MaxLength(20, 10), Export(false)]
        public string RequestEmpAlias
        {
            get { return m_RequestEmpAlias; }
            set { m_RequestEmpAlias = value; modify("RequestEmpAlias"); }
        }
        protected string m_RequestEmpMaritalStatus;
        [DBField("RequestEmpMaritalStatus"), DBAESEncryptStringField, TextSearch, MaxLength(40, 25), Export(false)]
        public string RequestEmpMaritalStatus
        {
            get { return m_RequestEmpMaritalStatus; }
            set { m_RequestEmpMaritalStatus = value; modify("RequestEmpMaritalStatus"); }
        }
        protected string m_RequestEmpPassportNo;
        [DBField("RequestEmpPassportNo"), TextSearch, DBAESEncryptStringField, MaxLength(40, 25), Export(false)]
        public string RequestEmpPassportNo
        {
            get { return m_RequestEmpPassportNo; }
            set { m_RequestEmpPassportNo = value; modify("RequestEmpPassportNo"); }
        }
        protected string m_RequestEmpPassportIssuedCountry;
        [DBField("RequestEmpPassportIssuedCountry"), TextSearch, DBAESEncryptStringField, MaxLength(40, 25), Export(false)]
        public string RequestEmpPassportIssuedCountry
        {
            get { return m_RequestEmpPassportIssuedCountry; }
            set { m_RequestEmpPassportIssuedCountry = value; modify("RequestEmpPassportIssuedCountry"); }
        }
        protected string m_RequestEmpNationality;
        [DBField("RequestEmpNationality"), TextSearch, DBAESEncryptStringField, MaxLength(100, 25), Export(false)]
        public string RequestEmpNationality
        {
            get { return m_RequestEmpNationality; }
            set { m_RequestEmpNationality = value; modify("RequestEmpNationality"); }
        }
        protected DateTime m_RequestEmpPassportExpiryDate;
        [DBField("RequestEmpPassportExpiryDate"), TextSearch, MaxLength(10, 10), Export(false)]
        public DateTime RequestEmpPassportExpiryDate
        {
            get { return m_RequestEmpPassportExpiryDate; }
            set { m_RequestEmpPassportExpiryDate = value; modify("RequestEmpPassportExpiryDate"); }
        }
        protected string m_RequestEmpHomePhoneNo;
        [DBField("RequestEmpHomePhoneNo"), TextSearch, DBAESEncryptStringField, MaxLength(100, 25), Export(false)]
        public string RequestEmpHomePhoneNo
        {
            get { return m_RequestEmpHomePhoneNo; }
            set { m_RequestEmpHomePhoneNo = value; modify("RequestEmpHomePhoneNo"); }
        }
        protected string m_RequestEmpMobileNo;
        [DBField("RequestEmpMobileNo"), TextSearch, DBAESEncryptStringField, MaxLength(100, 25), Export(false)]
        public string RequestEmpMobileNo
        {
            get { return m_RequestEmpMobileNo; }
            set { m_RequestEmpMobileNo = value; modify("RequestEmpMobileNo"); }
        }
        protected string m_RequestEmpOfficePhoneNo;
        [DBField("RequestEmpOfficePhoneNo"), TextSearch, DBAESEncryptStringField, MaxLength(100, 25), Export(false)]
        public string RequestEmpOfficePhoneNo
        {
            get { return m_RequestEmpOfficePhoneNo; }
            set { m_RequestEmpOfficePhoneNo = value; modify("RequestEmpOfficePhoneNo"); }
        }
        protected string m_RequestEmpEmail;
        [DBField("RequestEmpEmail"), TextSearch, DBAESEncryptStringField, MaxLength(100, 25), Export(false)]
        public string RequestEmpEmail
        {
            get { return m_RequestEmpEmail; }
            // Start 0000128, KuangWei, 2014-11-13
            //set { m_RequestEmpEmail = value; modify("TmpEmail"); }
            set { m_RequestEmpEmail = value; modify("RequestEmpEmail"); }
            // End 0000128, KuangWei, 2014-11-13
        }
        protected string m_RequestEmpResAddr;
        [DBField("RequestEmpResAddr"), TextSearch, DBAESEncryptStringField, MaxLength(100, 25), Export(false)]
        public string RequestEmpResAddr
        {
            get { return m_RequestEmpResAddr; }
            set { m_RequestEmpResAddr = value; modify("RequestEmpResAddr"); }
        }
        protected string m_RequestEmpResAddrAreaCode;
        [DBField("RequestEmpResAddrAreaCode"), TextSearch, DBAESEncryptStringField, Export(false), Required]
        public string RequestEmpResAddrAreaCode
        {
            get { return m_RequestEmpResAddrAreaCode; }
            set { m_RequestEmpResAddrAreaCode = value; modify("RequestEmpResAddrAreaCode"); }
        }

        

        protected string m_RequestEmpCorAdd;
        [DBField("RequestEmpCorAdd"), TextSearch, DBAESEncryptStringField, MaxLength(100, 25), Export(false)]
        public string RequestEmpCorAdd
        {
            get { return m_RequestEmpCorAdd; }
            set { m_RequestEmpCorAdd = value; modify("RequestEmpCorAdd"); }
        }
        protected DateTime m_RequestEmpCreateDate;
        [DBField("RequestEmpCreateDate"), TextSearch, Export(false)]
        public DateTime RequestEmpCreateDate
        {
            get { return m_RequestEmpCreateDate; }
            set { m_RequestEmpCreateDate = value; modify("RequestEmpCreateDate"); }
        }

        // Start 0000092, KuangWei, 2014-10-17
        protected string m_RequestEmpPlaceOfBirth;
        [DBField("RequestEmpPlaceOfBirth"), DBAESEncryptStringField, TextSearch, MaxLength(100, 40), Export(false)]
        public string RequestEmpPlaceOfBirth
        {
            get { return m_RequestEmpPlaceOfBirth; }
            set { m_RequestEmpPlaceOfBirth = value; modify("RequestEmpPlaceOfBirth"); }
        }

        protected int m_RequestEmpPlaceOfBirthID;
        [DBField("RequestEmpPlaceOfBirthID"), TextSearch, Export(false)]
        public int RequestEmpPlaceOfBirthID
        {
            get { return m_RequestEmpPlaceOfBirthID; }
            set { m_RequestEmpPlaceOfBirthID = value; modify("RequestEmpPlaceOfBirthID"); }
        }

        protected int m_RequestEmpPassportIssuedCountryID;
        [DBField("RequestEmpPassportIssuedCountryID"), TextSearch, Export(false)]
        public int RequestEmpPassportIssuedCountryID
        {
            get { return m_RequestEmpPassportIssuedCountryID; }
            set { m_RequestEmpPassportIssuedCountryID = value; modify("RequestEmpPassportIssuedCountryID"); }
        }

        protected int m_RequestEmpNationalityID;
        [DBField("RequestEmpNationalityID"), TextSearch, Export(false)]
        public int RequestEmpNationalityID
        {
            get { return m_RequestEmpNationalityID; }
            set { m_RequestEmpNationalityID = value; modify("RequestEmpNationalityID"); }
        }
        // End 0000092, KuangWei, 2014-10-17
    }
}
