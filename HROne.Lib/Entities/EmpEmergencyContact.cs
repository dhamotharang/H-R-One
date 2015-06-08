using System;
using System.Collections.Generic;
using System.Text;
using HROne.DataAccess;
////using perspectivemind.validation;
using HROne.Common;

namespace HROne.Lib.Entities
{
    [DBClass("EmpEmergencyContact")]
    public class EEmpEmergencyContact : BaseObjectWithRecordInfo
    {
        public static DBManager db = new DBManagerWithRecordInfo(typeof(EEmpEmergencyContact));


        protected int m_EmpEmergencyContactID;
        [DBField("EmpEmergencyContactID", true, true), TextSearch, Export(false)]
        public int EmpEmergencyContactID
        {
            get { return m_EmpEmergencyContactID; }
            set { m_EmpEmergencyContactID = value; modify("EmpEmergencyContactID"); }
        }
        protected int m_EmpID;
        [DBField("EmpID"), TextSearch, Export(false)]
        public int EmpID
        {
            get { return m_EmpID; }
            set { m_EmpID = value; modify("EmpID"); }
        }
        protected string m_EmpEmergencyContactName;
        [DBField("EmpEmergencyContactName"), DBAESEncryptStringField, TextSearch, MaxLength(100, 25), Export(false), Required]
        public string EmpEmergencyContactName
        {
            get { return m_EmpEmergencyContactName; }
            set { m_EmpEmergencyContactName = value; modify("EmpEmergencyContactName"); }
        }
        protected string m_EmpEmergencyContactGender;
        [DBField("EmpEmergencyContactGender"), DBAESEncryptStringField, TextSearch, Export(false)]
        public string EmpEmergencyContactGender
        {
            get { return m_EmpEmergencyContactGender; }
            set { m_EmpEmergencyContactGender = value; modify("EmpEmergencyContactGender"); }
        }
        protected string m_EmpEmergencyContactRelationship;
        [DBField("EmpEmergencyContactRelationship"), DBAESEncryptStringField, TextSearch, MaxLength(50, 25), Export(false)]
        public string EmpEmergencyContactRelationship
        {
            get { return m_EmpEmergencyContactRelationship; }
            set { m_EmpEmergencyContactRelationship = value; modify("EmpEmergencyContactRelationship"); }
        }
        protected string m_EmpEmergencyContactContactNoDay;
        [DBField("EmpEmergencyContactContactNoDay"), DBAESEncryptStringField, TextSearch, MaxLength(100, 25), Export(false), Required]
        public string EmpEmergencyContactContactNoDay
        {
            get { return m_EmpEmergencyContactContactNoDay; }
            set { m_EmpEmergencyContactContactNoDay = value; modify("EmpEmergencyContactContactNoDay"); }
        }
        protected string m_EmpEmergencyContactContactNoNight;
        [DBField("EmpEmergencyContactContactNoNight"), DBAESEncryptStringField, TextSearch, MaxLength(100, 25), Export(false)]
        public string EmpEmergencyContactContactNoNight
        {
            get { return m_EmpEmergencyContactContactNoNight; }
            set { m_EmpEmergencyContactContactNoNight = value; modify("EmpEmergencyContactContactNoNight"); }
        }

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
