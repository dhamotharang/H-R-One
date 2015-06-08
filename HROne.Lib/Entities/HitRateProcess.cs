using System;
using System.Data;
using System.Configuration;
using HROne.DataAccess;
////using perspectivemind.validation;

namespace HROne.Lib.Entities
{
    [DBClass("HitRateProcess")]

    public class EHitRateProcess : BaseObject
    {
        public static DBManager db = new DBManager(typeof(EHitRateProcess));

        protected int m_HitRateProcessID;
        [DBField("HitRateProcessID", true, true), TextSearch, Export(false)]
        public int HitRateProcessID
        {
            get { return m_HitRateProcessID; }
            set { m_HitRateProcessID = value; modify("HitRateProcessID"); }
        }

        protected int m_EmpID;
        [DBField("EmpID"), TextSearch, Export(false), Required]
        public int EmpID
        {
            get { return m_EmpID; }
            set { m_EmpID = value; modify("EmpID"); }
        }

        protected int m_payCodeID;
        [DBField("payCodeID"), TextSearch, Export(false), Required]
        public int payCodeID
        {
            get { return m_payCodeID; }
            set { m_payCodeID = value; modify("payCodeID"); }
        }

        protected double m_HitRate;
        [DBField("HitRate", "0.00"), TextSearch, Export(false), Required]
        public double HitRate
        {
            get { return m_HitRate; }
            set { m_HitRate = value; modify("HitRate"); }
        }

        protected int m_HitRateProcessImportBatchID;
        [DBField("HitRateProcessImportBatchID"), TextSearch, Export(false)]
        public int HitRateProcessImportBatchID
        {
            get { return m_HitRateProcessImportBatchID; }
            set { m_HitRateProcessImportBatchID = value; modify("HitRateProcessImportBatchID"); }
        }

        //protected DateTime m_HitRateProcessEffDate;
        //[DBField("HitRateProcessEffDate"), TextSearch, Export(false)]
        //public DateTime HitRateProcessEffDate
        //{
        //    get { return m_HitRateProcessEffDate; }
        //    set { m_HitRateProcessEffDate = value; modify("HitRateProcessEffDate"); }
        //}

    }
}