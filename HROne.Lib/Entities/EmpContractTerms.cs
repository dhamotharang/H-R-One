using System;
using System.Collections.Generic;
using System.Text;
using HROne.DataAccess;
////using perspectivemind.validation;

namespace HROne.Lib.Entities
{
    [DBClass("EmpContractTerms")]
    public class EEmpContractTerms : BaseObjectWithRecordInfo
    {
        public static DBManager db = new DBManagerWithRecordInfo(typeof(EEmpContractTerms));
        protected int m_EmpContractID;
        [DBField("EmpContractID", true, true), TextSearch, Export(false)]
        public int EmpContractID
        {
            get { return m_EmpContractID; }
            set { m_EmpContractID = value; modify("EmpContractID"); }
        }
        protected int m_EmpID;
        [DBField("EmpID"), TextSearch, Export(false)]
        public int EmpID
        {
            get { return m_EmpID; }
            set { m_EmpID = value; modify("EmpID"); }
        }
        protected string m_EmpContractCompanyName;
        [DBField("EmpContractCompanyName"), TextSearch,MaxLength(100,25), Export(false), Required]
        public string EmpContractCompanyName
        {
            get { return m_EmpContractCompanyName; }
            set { m_EmpContractCompanyName = value; modify("EmpContractCompanyName"); }
        }
        protected string m_EmpContractCompanyContactNo;
        [DBField("EmpContractCompanyContactNo"), TextSearch, MaxLength(100, 25), Export(false)]
        public string EmpContractCompanyContactNo
        {
            get { return m_EmpContractCompanyContactNo; }
            set { m_EmpContractCompanyContactNo = value; modify("EmpContractCompanyContactNo"); }
        }
        protected string m_EmpContractCompanyAddr;
        [DBField("EmpContractCompanyAddr"), TextSearch, MaxLength(100), Export(false)]
        public string EmpContractCompanyAddr
        {
            get { return m_EmpContractCompanyAddr; }
            set { m_EmpContractCompanyAddr = value; modify("EmpContractCompanyAddr"); }
        }
        protected DateTime m_EmpContractEmployedFrom;
        [DBField("EmpContractEmployedFrom"), TextSearch, MaxLength(25), Export(false)]
        public DateTime EmpContractEmployedFrom
        {
            get { return m_EmpContractEmployedFrom; }
            set { m_EmpContractEmployedFrom = value; modify("EmpContractEmployedFrom"); }
        }
        protected DateTime m_EmpContractEmployedTo;
        [DBField("EmpContractEmployedTo"), TextSearch, MaxLength(25), Export(false)]
        public DateTime EmpContractEmployedTo
        {
            get { return m_EmpContractEmployedTo; }
            set { m_EmpContractEmployedTo = value; modify("EmpContractEmployedTo"); }
        }
        protected double m_EmpContractGratuity;
        [DBField("EmpContractGratuity"), TextSearch, MaxLength(25), Export(false)]
        public double EmpContractGratuity
        {
            get { return m_EmpContractGratuity; }
            set { m_EmpContractGratuity = value; modify("EmpContractGratuity"); }
        }
        protected string m_CurrencyID;
        [DBField("CurrencyID"), TextSearch, Export(false)]
        public string CurrencyID
        {
            get { return m_CurrencyID; }
            set { m_CurrencyID = value; modify("CurrencyID"); }
        }
        protected int m_PayCodeID;
        [DBField("PayCodeID"), TextSearch, Export(false)]
        public int PayCodeID
        {
            get { return m_PayCodeID; }
            set { m_PayCodeID = value; modify("PayCodeID"); }
        }
        protected string m_EmpContractGratuityMethod;
        [DBField("EmpContractGratuityMethod"), TextSearch, Export(false)]
        public string EmpContractGratuityMethod
        {
            get { return m_EmpContractGratuityMethod; }
            set { m_EmpContractGratuityMethod = value; modify("EmpContractGratuityMethod"); }
        }
        protected int m_EmpAccID;
        [DBField("EmpAccID"), TextSearch, Export(false)]
        public int EmpAccID
        {
            get { return m_EmpAccID; }
            set { m_EmpAccID = value; modify("EmpAccID"); }
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
