using System;
using System.Collections.Generic;
using System.Text;
using HROne.DataAccess;
////using perspectivemind.validation;

namespace HROne.Lib.Entities
{
    [DBClass("LeavePlanBroughtForward")]
    public class ELeavePlanBroughtForward : BaseObject
    {
        public static DBManager db = new DBManager(typeof(ELeavePlanBroughtForward));
        protected int m_LeavePlanBroughtForwardID;
        [DBField("LeavePlanBroughtForwardID", true, true), TextSearch, Export(false)]
        public int LeavePlanBroughtForwardID
        {
            get { return m_LeavePlanBroughtForwardID; }
            set { m_LeavePlanBroughtForwardID = value; modify("LeavePlanBroughtForwardID"); }
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
        protected int m_LeavePlanBroughtForwardMax;
        [DBField("LeavePlanBroughtForwardMax"), TextSearch, MaxLength(10), Export(false), Required]
        public int LeavePlanBroughtForwardMax
        {
            get { return m_LeavePlanBroughtForwardMax; }
            set { m_LeavePlanBroughtForwardMax = value; modify("LeavePlanBroughtForwardMax"); }
        }

        protected bool m_LeavePlanBroughtForwardForfeitLastYearBroughtForwardOnly;
        [Obsolete, DBField("LeavePlanBroughtForwardForfeitLastYearBroughtForwardOnly"), TextSearch, Export(false)]
        public bool LeavePlanBroughtForwardForfeitLastYearBroughtForwardOnly
        {
            get { return m_LeavePlanBroughtForwardForfeitLastYearBroughtForwardOnly; }
            set { m_LeavePlanBroughtForwardForfeitLastYearBroughtForwardOnly = value; modify("LeavePlanBroughtForwardForfeitLastYearBroughtForwardOnly"); }
        }
        protected int m_LeavePlanBroughtForwardNumOfMonthExpired;
        [DBField("LeavePlanBroughtForwardNumOfMonthExpired"), TextSearch, Export(false)]
        public int LeavePlanBroughtForwardNumOfMonthExpired
        {
            get { return m_LeavePlanBroughtForwardNumOfMonthExpired; }
            set { m_LeavePlanBroughtForwardNumOfMonthExpired = value; modify("LeavePlanBroughtForwardNumOfMonthExpired"); }
        }
    }
}
