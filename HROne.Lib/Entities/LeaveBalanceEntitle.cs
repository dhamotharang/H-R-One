using System;
using System.Collections.Generic;
using System.Text;
using HROne.DataAccess;
////using perspectivemind.validation;

namespace HROne.Lib.Entities
{
    [DBClass("LeaveBalanceEntitle")]
    public class ELeaveBalanceEntitle :BaseObject
    {

        public static DBManager db = new DBManager(typeof(ELeaveBalanceEntitle));
        protected int m_LeaveBalanceEntitleID;
        [DBField("LeaveBalanceEntitleID", true, true), TextSearch, Export(false)]
        public int LeaveBalanceEntitleID
        {
            get { return m_LeaveBalanceEntitleID; }
            set { m_LeaveBalanceEntitleID = value; modify("LeaveBalanceEntitleID"); }
        }
        protected int m_EmpID;
        [DBField("EmpID"), TextSearch, Export(false)]
        public int EmpID
        {
            get { return m_EmpID; }
            set { m_EmpID = value; modify("EmpID"); }
        }
        protected int m_LeaveTypeID;
        [DBField("LeaveTypeID"), TextSearch, Export(false)]
        public int LeaveTypeID
        {
            get { return m_LeaveTypeID; }
            set { m_LeaveTypeID = value; modify("LeaveTypeID"); }
        }
        protected DateTime m_LeaveBalanceEntitleEffectiveDate;
        [DBField("LeaveBalanceEntitleEffectiveDate"), TextSearch, Export(false)]
        public DateTime LeaveBalanceEntitleEffectiveDate
        {
            get { return m_LeaveBalanceEntitleEffectiveDate; }
            set { m_LeaveBalanceEntitleEffectiveDate = value; modify("LeaveBalanceEntitleEffectiveDate"); }
        }
        protected DateTime m_LeaveBalanceEntitleGrantPeriodFrom;
        [DBField("LeaveBalanceEntitleGrantPeriodFrom"), TextSearch, Export(false)]
        public DateTime LeaveBalanceEntitleGrantPeriodFrom
        {
            get { return m_LeaveBalanceEntitleGrantPeriodFrom; }
            set { m_LeaveBalanceEntitleGrantPeriodFrom = value; modify("LeaveBalanceEntitleGrantPeriodFrom"); }
        }
        protected DateTime m_LeaveBalanceEntitleGrantPeriodTo;
        [DBField("LeaveBalanceEntitleGrantPeriodTo"), TextSearch, Export(false)]
        public DateTime LeaveBalanceEntitleGrantPeriodTo
        {
            get { return m_LeaveBalanceEntitleGrantPeriodTo; }
            set { m_LeaveBalanceEntitleGrantPeriodTo = value; modify("LeaveBalanceEntitleGrantPeriodTo"); }
        }
        protected DateTime m_LeaveBalanceEntitleDateExpiry;
        [DBField("LeaveBalanceEntitleDateExpiry"), TextSearch, Export(false)]
        public DateTime LeaveBalanceEntitleDateExpiry
        {
            get { return m_LeaveBalanceEntitleDateExpiry; }
            set { m_LeaveBalanceEntitleDateExpiry = value; modify("LeaveBalanceEntitleDateExpiry"); }
        }

        protected double m_LeaveBalanceEntitleDays;
        [DBField("LeaveBalanceEntitleDays"), TextSearch, Export(false)]
        public double LeaveBalanceEntitleDays
        {
            get { return m_LeaveBalanceEntitleDays; }
            set { m_LeaveBalanceEntitleDays = value; modify("LeaveBalanceEntitleDays"); }
        }

        protected override void afterUpdate(DatabaseConnection dbConn, DBManager db)
        {
            int oldLeaveTypeID = 0;
            int newLeaveTypeID = m_LeaveTypeID;
            DateTime oldLeaveBalanceEntitleDateExpiry = new DateTime();
            DateTime newLeaveBalanceEntitleDateExpiry = m_LeaveBalanceEntitleDateExpiry;
            if (oldValueObject != null)
            {
                ELeaveBalanceEntitle oldLeaveBalanceEntitle = (ELeaveBalanceEntitle)oldValueObject;
                oldLeaveTypeID = oldLeaveBalanceEntitle.LeaveTypeID;
                oldLeaveBalanceEntitleDateExpiry = oldLeaveBalanceEntitle.LeaveBalanceEntitleDateExpiry;
            }
            {
                ELeaveType leaveType = new ELeaveType();
                if (!oldLeaveBalanceEntitleDateExpiry.Ticks.Equals(0))
                {
                    leaveType.LeaveTypeID = oldLeaveTypeID;
                    if (ELeaveType.db.select(dbConn, leaveType))
                        ELeaveBalance.DeleteLeaveBalanceAfter(dbConn, this.EmpID, leaveType.LeaveTypeID, oldLeaveBalanceEntitleDateExpiry);
                }
                if (!newLeaveBalanceEntitleDateExpiry.Ticks.Equals(0))
                {
                    leaveType.LeaveTypeID = newLeaveTypeID;
                    if (ELeaveType.db.select(dbConn, leaveType))
                        ELeaveBalance.DeleteLeaveBalanceAfter(dbConn, this.EmpID, leaveType.LeaveTypeID, newLeaveBalanceEntitleDateExpiry);
                }
            }

            base.afterUpdate(dbConn, db);
        }

        public String PrintMe()
        {
            return "\t================================================================" + Environment.NewLine +
                   "\tLeaveBalanceEntitleID = " + this.LeaveBalanceEntitleID.ToString() + Environment.NewLine +
                   "\tEmpID = " + this.EmpID.ToString() + Environment.NewLine +
                   "\tLeaveTypeID = " + this.LeaveTypeID.ToString() + Environment.NewLine +
                   "\tLeaveBalanceEntitleEffectiveDate = " + this.LeaveBalanceEntitleEffectiveDate.ToString("yyyy-MM-dd") + Environment.NewLine +
                   "\tLeaveBalanceEntitleDateExpiry = " + this.LeaveBalanceEntitleDateExpiry.ToString("yyyy-MM-dd") + Environment.NewLine +
                   "\tLeaveBalanceEntitleDays = " + this.LeaveBalanceEntitleDays.ToString("0.00") + Environment.NewLine +
                   "\tLeaveBalanceEntitleGrantPeriodFrom = " + this.LeaveBalanceEntitleGrantPeriodFrom.ToString("0.00") + Environment.NewLine +
                   "\tLeaveBalanceEntitleGrantPeriodTo = " + this.LeaveBalanceEntitleGrantPeriodTo.ToString("0.00") + Environment.NewLine +
                   "\t================================================================";
        }




    }
}
