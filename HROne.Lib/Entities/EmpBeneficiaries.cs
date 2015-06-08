using System;
using HROne.DataAccess;

namespace HROne.Lib.Entities
{
    [DBClass("EmpBeneficiaries")]
    public class EEmpBeneficiaries : BaseObjectWithRecordInfo
    {
        public static DBManager db = new DBManagerWithRecordInfo(typeof(EEmpBeneficiaries));

        protected int m_EmpBeneficiariesID;
        [DBField("EmpBeneficiariesID", true, true), TextSearch, Export(false)]
        public int EmpBeneficiariesID
        {
            get { return m_EmpBeneficiariesID; }
            set { m_EmpBeneficiariesID = value; modify("EmpBeneficiariesID"); }
        }

        protected int m_EmpID;
        [DBField("EmpID"), TextSearch, Export(false)]
        public int EmpID
        {
            get { return m_EmpID; }
            set { m_EmpID = value; modify("EmpID"); }
        }

        protected string m_EmpBeneficiariesName;
        [DBField("EmpBeneficiariesName"), TextSearch, MaxLength(200, 50), Export(false), Required]
        public string EmpBeneficiariesName
        {
            get { return m_EmpBeneficiariesName; }
            set { m_EmpBeneficiariesName = value; modify("EmpBeneficiariesName"); }
        }

        protected double m_EmpBeneficiariesShare;
        [DBField("EmpBeneficiariesShare", "0.00"), TextSearch, MaxLength(6), Export(false), Required]
        public double EmpBeneficiariesShare
        {
            get { return m_EmpBeneficiariesShare; }
            set { m_EmpBeneficiariesShare = value; modify("EmpBeneficiariesShare"); }
        }

        protected string m_EmpBeneficiariesHKID;
        [DBField("EmpBeneficiariesHKID"), DBAESEncryptStringField, TextSearch, MaxLength(12), Export(false), Required]
        public string EmpBeneficiariesHKID
        {
            get { return m_EmpBeneficiariesHKID; }
            set { m_EmpBeneficiariesHKID = value; modify("EmpBeneficiariesHKID"); }
        }

        public string MaskedEmpBeneficiariesHKID
        {
            get
            {
                int midPos = m_EmpBeneficiariesHKID.Length / 2;
                if (midPos > 1) midPos--;
                string maskedHKID = m_EmpBeneficiariesHKID.Substring(0, midPos);
                for (int i = midPos; i < m_EmpBeneficiariesHKID.Length; i++)
                {
                    char ch = m_EmpBeneficiariesHKID.ToCharArray()[i];
                    if (ch >= '0' && ch <= '9' || ch >= 'A' && ch <= 'Z' || ch >= 'a' && ch <= 'z')
                        maskedHKID += "*";
                    else
                        maskedHKID += ch.ToString();

                }
                return maskedHKID;
            }
        }

        protected string m_EmpBeneficiariesRelation;
        [DBField("EmpBeneficiariesRelation"), TextSearch, MaxLength(250, 50), Export(false), Required]
        public string EmpBeneficiariesRelation
        {
            get { return m_EmpBeneficiariesRelation; }
            set { m_EmpBeneficiariesRelation = value; modify("EmpBeneficiariesRelation"); }
        }

        protected string m_EmpBeneficiariesAddress;
        [DBField("EmpBeneficiariesAddress"), TextSearch, MaxLength(250, 50), Export(false)]
        public string EmpBeneficiariesAddress
        {
            get { return m_EmpBeneficiariesAddress; }
            set { m_EmpBeneficiariesAddress = value; modify("EmpBeneficiariesAddress"); }
        }

        protected string m_EmpBeneficiariesDistrict;
        [DBField("EmpBeneficiariesDistrict"), TextSearch, MaxLength(150, 50), Export(false)]
        public string EmpBeneficiariesDistrict
        {
            get { return m_EmpBeneficiariesDistrict; }
            set { m_EmpBeneficiariesDistrict = value; modify("EmpBeneficiariesDistrict"); }
        }
        
        protected string m_EmpBeneficiariesArea;
        [DBField("EmpBeneficiariesArea"), TextSearch, MaxLength(100, 50), Export(false)]
        public string EmpBeneficiariesArea
        {
            get { return m_EmpBeneficiariesArea; }
            set { m_EmpBeneficiariesArea = value; modify("EmpBeneficiariesArea"); }
        }

        protected string m_EmpBeneficiariesCountry;
        [DBField("EmpBeneficiariesCountry"), TextSearch, MaxLength(250, 50), Export(false)]
        public string EmpBeneficiariesCountry
        {
            get { return m_EmpBeneficiariesCountry; }
            set { m_EmpBeneficiariesCountry = value; modify("EmpBeneficiariesCountry"); }
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
