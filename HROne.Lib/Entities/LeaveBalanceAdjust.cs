//using System;
//using System.Collections.Generic;
//using System.Text;
//using HROne.DataAccess;
//////using perspectivemind.validation;

//namespace HROne.Lib.Entities
//{
//    [DBClass("LeaveBalanceAdjust")]
//    public class ELeaveBalanceAdjust : BaseObject
//    {
//        public static DBManager db = new DBManager(typeof(ELeaveBalanceAdjust));
//        protected int m_LeaveBalanceAdjID;
//        [DBField("LeaveBalanceAdjID", true, true), TextSearch, Export(false)]
//        public int LeaveBalanceAdjID
//        {
//            get { return m_LeaveBalanceAdjID; }
//            set { m_LeaveBalanceAdjID = value; modify("LeaveBalanceAdjID"); }
//        }
//        protected int m_EmpID;
//        [DBField("EmpID"), TextSearch, Export(false)]
//        public int EmpID
//        {
//            get { return m_EmpID; }
//            set { m_EmpID = value; modify("EmpID"); }
//        }
//        protected int m_LeaveTypeID;
//        [DBField("LeaveTypeID"), TextSearch, Export(false)]
//        public int LeaveTypeID
//        {
//            get { return m_LeaveTypeID; }
//            set { m_LeaveTypeID = value; modify("LeaveTypeID"); }
//        }

//        protected DateTime m_LeaveBalanceEffectiveDate;
//        [DBField("LeaveBalanceEffectiveDate"), TextSearch, Export(false)]
//        public DateTime LeaveBalanceEffectiveDate
//        {
//            get { return m_LeaveBalanceEffectiveDate; }
//            set { m_LeaveBalanceEffectiveDate = value; modify("LeaveBalanceEffectiveDate"); }
//        }        
//        protected double m_LeaveBalanceAdjDays;
//        [DBField("LeaveBalanceAdjDays"), TextSearch, Export(false)]
//        public double LeaveBalanceAdjDays
//        {
//            get { return m_LeaveBalanceAdjDays; }
//            set { m_LeaveBalanceAdjDays = value; modify("LeaveBalanceAdjDays"); }
//        }
//    }
//}
