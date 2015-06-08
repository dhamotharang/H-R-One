using System;
using System.Collections.Generic;
using System.Text;
using HROne.DataAccess;
////using perspectivemind.validation;

namespace HROne.Lib.Entities
{
    [DBClass("EmpPlaceOfResidence")]
    public class EEmpPlaceOfResidence : BaseObjectWithRecordInfo 
    {
        public static DBManager db = new DBManagerWithRecordInfo(typeof(EEmpPlaceOfResidence));
        protected int m_EmpPoRID;
        [DBField("EmpPoRID", true, true), TextSearch, Export(false)]
        public int EmpPoRID
        {
            get { return m_EmpPoRID; }
            set { m_EmpPoRID = value; modify("EmpPoRID"); }
        }
        protected int m_EmpID;
        [DBField("EmpID"), TextSearch, Export(false)]
        public int EmpID
        {
            get { return m_EmpID; }
            set { m_EmpID = value; modify("EmpID"); }
        }
        protected DateTime m_EmpPoRFrom;
        [DBField("EmpPoRFrom"), TextSearch , Export(false), Required]
        public DateTime EmpPoRFrom
        {
            get { return m_EmpPoRFrom; }
            set { m_EmpPoRFrom = value; modify("EmpPoRFrom"); }
        }
        protected DateTime m_EmpPoRTo;
        [DBField("EmpPoRTo"), TextSearch, Export(false)]
        public DateTime EmpPoRTo
        {
            get { return m_EmpPoRTo; }
            set { m_EmpPoRTo = value; modify("EmpPoRTo"); }
        }
        protected string m_EmpPoRLandLord;
        [DBField("EmpPoRLandLord"), TextSearch, DBAESEncryptStringField, MaxLength(100, 40), Export(false)]
        public string EmpPoRLandLord
        {
            get { return m_EmpPoRLandLord; }
            set { m_EmpPoRLandLord = value; modify("EmpPoRLandLord"); }
        }
        protected string m_EmpPoRLandLordAddr;
        [DBField("EmpPoRLandLordAddr"), TextSearch, DBAESEncryptStringField, MaxLength(110, 40), Export(false)]
        public string EmpPoRLandLordAddr
        {
            get { return m_EmpPoRLandLordAddr; }
            set { m_EmpPoRLandLordAddr = value; modify("EmpPoRLandLordAddr"); }
        }
        protected string m_EmpPoRPropertyAddr;
        [DBField("EmpPoRPropertyAddr"), TextSearch, DBAESEncryptStringField, MaxLength(110, 40), Export(false), Required]
        public string EmpPoRPropertyAddr
        {
            get { return m_EmpPoRPropertyAddr; }
            set { m_EmpPoRPropertyAddr = value; modify("EmpPoRPropertyAddr"); }
        }
        protected string m_EmpPoRNature;
        [DBField("EmpPoRNature"), TextSearch, DBAESEncryptStringField, MaxLength(19), Export(false), Required]
        public string EmpPoRNature
        {
            get { return m_EmpPoRNature; }
            set { m_EmpPoRNature = value; modify("EmpPoRNature"); }
        }
        protected double m_EmpPoRPayToLandER;
        [DBField("EmpPoRPayToLandER", "0.00"), TextSearch, MaxLength(10), Export(false), Required]
        public double EmpPoRPayToLandER
        {
            get { return m_EmpPoRPayToLandER; }
            set { m_EmpPoRPayToLandER = value; modify("EmpPoRPayToLandER"); }
        }
        protected double m_EmpPoRPayToLandEE;
        [DBField("EmpPoRPayToLandEE","0.00"), TextSearch, MaxLength(10), Export(false), Required]
        public double EmpPoRPayToLandEE
        {
            get { return m_EmpPoRPayToLandEE; }
            set { m_EmpPoRPayToLandEE = value; modify("EmpPoRPayToLandEE"); }
        }
        protected double m_EmpPoRRefundToEE;
        [DBField("EmpPoRRefundToEE", "0.00"), TextSearch, MaxLength(10), Export(false), Required]
        public double EmpPoRRefundToEE
        {
            get { return m_EmpPoRRefundToEE; }
            set { m_EmpPoRRefundToEE = value; modify("EmpPoRRefundToEE"); }
        }
        protected double m_EmpPoRPayToERByEE;
        [DBField("EmpPoRPayToERByEE", "0.00"), TextSearch, MaxLength(10), Export(false), Required]
        public double EmpPoRPayToERByEE
        {
            get { return m_EmpPoRPayToERByEE; }
            set { m_EmpPoRPayToERByEE = value; modify("EmpPoRPayToERByEE"); }
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
