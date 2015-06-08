using System;
using System.Data;
using System.Configuration;
using HROne.DataAccess;
////using perspectivemind.validation;

namespace HROne.Lib.Entities
{
    [DBClass("DoublePayAdjustment")]

    public class EDoublePayAdjustment : BaseObject
    {
        public static DBManager db = new DBManager(typeof(EDoublePayAdjustment));

        protected int m_DoublePayAdjustID;
        [DBField("DoublePayAdjustID", true, true), TextSearch, Export(false)]
        public int DoublePayAdjustID
        {
            get { return m_DoublePayAdjustID; }
            set { m_DoublePayAdjustID = value; modify("DoublePayAdjustID"); }
        }

        protected int m_EmpID;
        [DBField("EmpID"), TextSearch, Export(false), Required]
        public int EmpID
        {
            get { return m_EmpID; }
            set { m_EmpID = value; modify("EmpID"); }
        }

        protected double m_SalesAchievementRate;
        [DBField("SalesAchievementRate", "0.00"), TextSearch, Export(false), Required]
        public double SalesAchievementRate
        {
            get { return m_SalesAchievementRate; }
            set { m_SalesAchievementRate = value; modify("SalesAchievementRate"); }
        }

        protected int m_DoublePayAdjustImportBatchID;
        [DBField("DoublePayAdjustImportBatchID"), TextSearch, Export(false)]
        public int DoublePayAdjustImportBatchID
        {
            get { return m_DoublePayAdjustImportBatchID; }
            set { m_DoublePayAdjustImportBatchID = value; modify("DoublePayAdjustImportBatchID"); }
        }

        protected DateTime m_DoublePayAdjustEffDate;
        [DBField("DoublePayAdjustEffDate"), TextSearch, Export(false)]
        public DateTime DoublePayAdjustEffDate
        {
            get { return m_DoublePayAdjustEffDate; }
            set { m_DoublePayAdjustEffDate = value; modify("DoublePayAdjustEffDate"); }
        }

    }
}