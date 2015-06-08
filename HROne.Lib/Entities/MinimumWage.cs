using System;
using System.Data;
using System.Configuration;
using HROne.DataAccess;
////using perspectivemind.validation;

namespace HROne.Lib.Entities
{
    [DBClass("MinimumWage")]

    public class EMinimumWage : BaseObject
    {
        public static DBManager db = new DBManager(typeof(EMinimumWage));

        protected int m_MinimumWageID;
        [DBField("MinimumWageID", true, true), TextSearch, Export(false)]
        public int MinimumWageID
        {
            get { return m_MinimumWageID; }
            set { m_MinimumWageID = value; modify("MinimumWageID"); }
        }

        protected DateTime m_MinimumWageEffectiveDate;
        [DBField("MinimumWageEffectiveDate"), TextSearch, Export(false), Required]
        public DateTime MinimumWageEffectiveDate
        {
            get { return m_MinimumWageEffectiveDate; }
            set { m_MinimumWageEffectiveDate = value; modify("MinimumWageEffectiveDate"); }
        }


        protected double m_MinimumWageHourlyRate;
        [DBField("MinimumWageHourlyRate", "0.00"), TextSearch, Export(false), Required]
        public double MinimumWageHourlyRate
        {
            get { return m_MinimumWageHourlyRate; }
            set { m_MinimumWageHourlyRate = value; modify("MinimumWageHourlyRate"); }
        }

       
    }
}