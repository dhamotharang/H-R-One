using System;
using System.Collections.Generic;
using System.Text;
using HROne.DataAccess;
////using perspectivemind.validation;

namespace HROne.Lib.Entities
{
    [DBClass("LeavePlanEntitle")]
    public class ELeavePlanEntitle : BaseObject
    {
        public static DBManager db = new DBManager(typeof(ELeavePlanEntitle));
        protected int m_LeavePlanEntitleID;
        [DBField("LeavePlanEntitleID", true, true), TextSearch, Export(false)]
        public int LeavePlanEntitleID
        {
            get { return m_LeavePlanEntitleID; }
            set { m_LeavePlanEntitleID = value; modify("LeavePlanEntitleID"); }
        }
        protected int m_LeavePlanID;
        [DBField("LeavePlanID"), TextSearch, Export(false)]
        public int LeavePlanID
        {
            get { return m_LeavePlanID; }
            set { m_LeavePlanID = value; modify("LeavePlanID"); }
        }
        protected int m_LeaveTypeID;
        [DBField("LeaveTypeID"), TextSearch, Export(false)]
        public int LeaveTypeID
        {
            get { return m_LeaveTypeID; }
            set { m_LeaveTypeID = value; modify("LeaveTypeID"); }
        }
        protected int m_LeavePlanEntitleYearOfService;
        [DBField("LeavePlanEntitleYearOfService"), TextSearch,MaxLength(5), Export(false),Required]
        public int LeavePlanEntitleYearOfService
        {
            get { return m_LeavePlanEntitleYearOfService; }
            set { m_LeavePlanEntitleYearOfService = value; modify("LeavePlanEntitleYearOfService"); }
        }
        protected double m_LeavePlanEntitleDays;
        [DBField("LeavePlanEntitleDays", "0.#"), TextSearch,MaxLength(5), Export(false),Required]
        public double LeavePlanEntitleDays
        {
            get { return m_LeavePlanEntitleDays; }
            set { m_LeavePlanEntitleDays = value; modify("LeavePlanEntitleDays"); }
        }
    }
}
