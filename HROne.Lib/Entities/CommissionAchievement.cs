using System;
using System.Data;
using System.Configuration;
using HROne.DataAccess;
////using perspectivemind.validation;

namespace HROne.Lib.Entities
{
    [DBClass("CommissionAchievement")]

    public class ECommissionAchievement : BaseObject
    {
        public static DBManager db = new DBManager(typeof(ECommissionAchievement));

        protected int m_CAID;
        [DBField("CAID", true, true), TextSearch, Export(false)]
        public int CAID
        {
            get { return m_CAID; }
            set { m_CAID = value; modify("CAID"); }
        }

        protected int m_EmpID;
        [DBField("EmpID"), TextSearch, Export(false), Required]
        public int EmpID
        {
            get { return m_EmpID; }
            set { m_EmpID = value; modify("EmpID"); }
        }

        protected double m_CAPercent;
        [DBField("CAPercent", "0.00"), TextSearch, Export(false), Required]
        public double CAPercent
        {
            get { return m_CAPercent; }
            set { m_CAPercent = value; modify("CAPercent"); }
        }

        protected int m_CAImportBatchID;
        [DBField("CAImportBatchID"), TextSearch, Export(false)]
        public int CAImportBatchID
        {
            get { return m_CAImportBatchID; }
            set { m_CAImportBatchID = value; modify("CAImportBatchID"); }
        }

        protected DateTime m_CAEffDate;
        [DBField("CAEffDate"), TextSearch, Export(false)]
        public DateTime CAEffDate
        {
            get { return m_CAEffDate; }
            set { m_CAEffDate = value; modify("CAEffDate"); }
        }

    }
}