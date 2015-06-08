using System;
using System.Collections.Generic;
using System.Text;
using HROne.DataAccess;
////using perspectivemind.validation;

namespace HROne.Lib.Entities
{
    [DBClass("LeaveBalanceAdjustment")]
    public class ELeaveBalanceAdjustment : BaseObjectWithRecordInfo
    {
        public const string ADJUST_TYPE_RESET_BALANCE = "B";
        public const string ADJUST_TYPE_ADJUSTMENT = "A";

        public const string ADJUST_TYPE_RESET_BALANCE_NAME = "Reset Balance";
        public const string ADJUST_TYPE_ADJUSTMENT_NAME = "Adjustment";
        public static DBManager db = new DBManager(typeof(ELeaveBalanceAdjustment));
        public static WFValueList VLLeaveBalAdjType = new AppUtils.NewWFTextList(new string[] { ADJUST_TYPE_RESET_BALANCE, ADJUST_TYPE_ADJUSTMENT }, new string[] { ADJUST_TYPE_RESET_BALANCE_NAME, ADJUST_TYPE_ADJUSTMENT_NAME });

        protected int m_LeaveBalAdjID;
        [DBField("LeaveBalAdjID", true, true), TextSearch, Export(false)]
        public int LeaveBalAdjID
        {
            get { return m_LeaveBalAdjID; }
            set { m_LeaveBalAdjID = value; modify("LeaveBalAdjID"); }
        }
        protected int m_EmpID;
        [DBField("EmpID"), TextSearch, Export(false), Required]
        public int EmpID
        {
            get { return m_EmpID; }
            set { m_EmpID = value; modify("EmpID"); }
        }
        protected DateTime m_LeaveBalAdjDate;
        [DBField("LeaveBalAdjDate"), TextSearch, Export(false), Required]
        public DateTime LeaveBalAdjDate
        {
            get { return m_LeaveBalAdjDate; }
            set { m_LeaveBalAdjDate = value; modify("LeaveBalAdjDate"); }
        }
        protected int m_LeaveTypeID;
        [DBField("LeaveTypeID"), TextSearch, Export(false), Required]
        public int LeaveTypeID
        {
            get { return m_LeaveTypeID; }
            set { m_LeaveTypeID = value; modify("LeaveTypeID"); }
        }
        protected string m_LeaveBalAdjType;
        [DBField("LeaveBalAdjType"), TextSearch, Export(false), Required]
        public string LeaveBalAdjType
        {
            get { return m_LeaveBalAdjType; }
            set { m_LeaveBalAdjType = value; modify("LeaveBalAdjType"); }
        }
        protected double m_LeaveBalAdjValue;
        [DBField("LeaveBalAdjValue", "0.0000"), TextSearch, Export(false), Required]
        public double LeaveBalAdjValue
        {
            get { return m_LeaveBalAdjValue; }
            set { m_LeaveBalAdjValue = value; modify("LeaveBalAdjValue"); }
        }
        protected string m_LeaveBalAdjRemark;
        [DBField("LeaveBalAdjRemark"), TextSearch, Export(false)]
        public string LeaveBalAdjRemark
        {
            get { return m_LeaveBalAdjRemark; }
            set { m_LeaveBalAdjRemark = value; modify("LeaveBalAdjRemark"); }
        }

        //  For Synchronize Use only
        protected string m_SynID;
        [DBField("SynID"), TextSearch, Export(false)]
        public string SynID
        {
            get { return m_SynID; }
            set { m_SynID = value; modify("SynID"); }
        }

        protected override void afterInsert(DatabaseConnection dbConn, DBManager db)
        {
            base.afterInsert(dbConn, db);

            ELeaveBalance.DeleteLeaveBalanceAfter(dbConn, m_EmpID, m_LeaveTypeID, m_LeaveBalAdjDate);

        }

        protected override void afterUpdate(DatabaseConnection dbConn, DBManager db)
        {
            int oldLeaveTypeID = 0;
            int newLeaveTypeID = m_LeaveTypeID;
            DateTime oldLeaveAdjustDate = new DateTime();
            DateTime newLeaveAdjustDate = m_LeaveBalAdjDate;
            if (oldValueObject != null)
            {
                ELeaveBalanceAdjustment oldLeaveBalanceAdjust = (ELeaveBalanceAdjustment)oldValueObject;
                oldLeaveTypeID = oldLeaveBalanceAdjust.LeaveTypeID;
                oldLeaveAdjustDate = oldLeaveBalanceAdjust.LeaveBalAdjDate;
            }
            {
                ELeaveBalance.DeleteLeaveBalanceAfter(dbConn, this.EmpID, oldLeaveTypeID, oldLeaveAdjustDate);
                ELeaveBalance.DeleteLeaveBalanceAfter(dbConn, this.EmpID, newLeaveTypeID, newLeaveAdjustDate);
            }

            base.afterUpdate(dbConn, db);
        }

        protected override void afterDelete(DatabaseConnection dbConn, DBManager db)
        {
            base.afterDelete(dbConn, db);

            ELeaveBalance.DeleteLeaveBalanceAfter(dbConn, m_EmpID, m_LeaveTypeID, m_LeaveBalAdjDate);
        }
    }
}
