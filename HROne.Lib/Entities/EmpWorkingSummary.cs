using System;
using System.Collections.Generic;
using System.Text;
using HROne.DataAccess;
////using perspectivemind.validation;

namespace HROne.Lib.Entities
{
    [DBClass("EmpWorkingSummary")]
    public class EEmpWorkingSummary : BaseObject
    {
        public static DBManager db = new DBManager(typeof(EEmpWorkingSummary));
        protected int m_EmpWorkingSummaryID;
        [DBField("EmpWorkingSummaryID", true, true), TextSearch, Export(false)]
        public int EmpWorkingSummaryID
        {
            get { return m_EmpWorkingSummaryID; }
            set { m_EmpWorkingSummaryID = value; modify("EmpWorkingSummaryID"); }
        }
        protected int m_EmpID;
        [DBField("EmpID"), TextSearch, Export(false), Required]
        public int EmpID
        {
            get { return m_EmpID; }
            set { m_EmpID = value; modify("EmpID"); }
        }
        protected DateTime m_EmpWorkingSummaryAsOfDate;
        [DBField("EmpWorkingSummaryAsOfDate"), TextSearch, MaxLength(10), Export(false), Required]
        public DateTime EmpWorkingSummaryAsOfDate
        {
            get { return m_EmpWorkingSummaryAsOfDate; }
            set { m_EmpWorkingSummaryAsOfDate = value; modify("EmpWorkingSummaryAsOfDate"); }
        }
        protected double m_EmpWorkingSummaryRestDayEntitled;
        [DBField("EmpWorkingSummaryRestDayEntitled", "0.##"), TextSearch, MaxLength(7), Export(false)]
        public double EmpWorkingSummaryRestDayEntitled
        {
            get { return m_EmpWorkingSummaryRestDayEntitled; }
            set { m_EmpWorkingSummaryRestDayEntitled = value; modify("EmpWorkingSummaryRestDayEntitled"); }
        }
        protected double m_EmpWorkingSummaryRestDayTaken;
        [DBField("EmpWorkingSummaryRestDayTaken", "0.##"), TextSearch, MaxLength(7), Export(false)]
        public double EmpWorkingSummaryRestDayTaken
        {
            get { return m_EmpWorkingSummaryRestDayTaken; }
            set { m_EmpWorkingSummaryRestDayTaken = value; modify("EmpWorkingSummaryRestDayTaken"); }
        }
        protected double m_EmpWorkingSummaryTotalWorkingDays;
        [DBField("EmpWorkingSummaryTotalWorkingDays", "0.##"), TextSearch, MaxLength(7), Export(false)]
        public double EmpWorkingSummaryTotalWorkingDays
        {
            get { return m_EmpWorkingSummaryTotalWorkingDays; }
            set { m_EmpWorkingSummaryTotalWorkingDays = value; modify("EmpWorkingSummaryTotalWorkingDays"); }
        }
        protected double m_EmpWorkingSummaryTotalWorkingHours;
        [DBField("EmpWorkingSummaryTotalWorkingHours","0.####"), TextSearch, MaxLength(10), Export(false)]
        public double EmpWorkingSummaryTotalWorkingHours
        {
            get { return m_EmpWorkingSummaryTotalWorkingHours; }
            set { m_EmpWorkingSummaryTotalWorkingHours = value; modify("EmpWorkingSummaryTotalWorkingHours"); }
        }
        protected double m_EmpWorkingSummaryTotalLunchTimeHours;
        [DBField("EmpWorkingSummaryTotalLunchTimeHours", "0.####"), TextSearch, MaxLength(7), Export(false)]
        public double EmpWorkingSummaryTotalLunchTimeHours
        {
            get { return m_EmpWorkingSummaryTotalLunchTimeHours; }
            set { m_EmpWorkingSummaryTotalLunchTimeHours = value; modify("EmpWorkingSummaryTotalLunchTimeHours"); }
        }
        
    }
}
