using System;
using System.Data;
using System.Configuration;
using HROne.DataAccess;
////using perspectivemind.validation;

namespace HROne.Lib.Entities
{
    [DBClass("IncentivePayment")]

    public class EIncentivePayment : BaseObject
    {
        public static DBManager db = new DBManager(typeof(EIncentivePayment));

        protected int m_IPID;
        [DBField("IPID", true, true), TextSearch, Export(false)]
        public int IPID
        {
            get { return m_IPID; }
            set { m_IPID = value; modify("IPID"); }
        }

        protected int m_EmpID;
        [DBField("EmpID"), TextSearch, Export(false), Required]
        public int EmpID
        {
            get { return m_EmpID; }
            set { m_EmpID = value; modify("EmpID"); }
        }

        protected double m_IPPercent;
        [DBField("IPPercent", "0.00"), TextSearch, Export(false), Required]
        public double IPPercent
        {
            get { return m_IPPercent; }
            set { m_IPPercent = value; modify("IPPercent"); }
        }

        protected int m_IPImportBatchID;
        [DBField("IPImportBatchID"), TextSearch, Export(false)]
        public int IPImportBatchID
        {
            get { return m_IPImportBatchID; }
            set { m_IPImportBatchID = value; modify("IPImportBatchID"); }
        }

        protected DateTime m_IPEffDate;
        [DBField("IPEffDate"), TextSearch, Export(false)]
        public DateTime IPEffDate
        {
            get { return m_IPEffDate; }
            set { m_IPEffDate = value; modify("IPEffDate"); }
        }

    }
}