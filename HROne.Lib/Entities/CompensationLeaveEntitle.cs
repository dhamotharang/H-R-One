using System;
using System.Collections.Generic;
using System.Text;
using HROne.DataAccess;
////using perspectivemind.validation;
using HROne.Common;

namespace HROne.Lib.Entities
{
    [DBClass("CompensationLeaveEntitle")]
    public class ECompensationLeaveEntitle : BaseObjectWithRecordInfo
    {
        public static DBManager db = new DBManager(typeof(ECompensationLeaveEntitle));
        public static WFValueList VLLeaveEntitleYear = new AppUtils.WFDBDistinctList(db, "Year(CompensationLeaveEntitleEffectiveDate)", " Year(CompensationLeaveEntitleEffectiveDate)", "Year(CompensationLeaveEntitleEffectiveDate) desc");

        protected int m_CompensationLeaveEntitleID;
        [DBField("CompensationLeaveEntitleID", true, true), TextSearch, Export(false)]
        public int CompensationLeaveEntitleID
        {
            get { return m_CompensationLeaveEntitleID; }
            set { m_CompensationLeaveEntitleID = value; modify("CompensationLeaveEntitleID"); }
        }

        protected int m_EmpID;
        [DBField("EmpID"), TextSearch, Export(false), Required]
        public int EmpID
        {
            get { return m_EmpID; }
            set { m_EmpID = value; modify("EmpID"); }
        }

        protected DateTime m_CompensationLeaveEntitleEffectiveDate;
        [DBField("CompensationLeaveEntitleEffectiveDate"), TextSearch, Export(false), Required]
        public DateTime CompensationLeaveEntitleEffectiveDate
        {
            get { return m_CompensationLeaveEntitleEffectiveDate; }
            set { m_CompensationLeaveEntitleEffectiveDate = value; modify("CompensationLeaveEntitleEffectiveDate"); }
        }

        //protected string m_CompensationLeaveEntitleClaimType;
        //[DBField("CompensationLeaveEntitleType"), TextSearch, Export(false), Required]
        //public string CompensationLeaveEntitleType
        //{
        //    get { return m_CompensationLeaveEntitleClaimType; }
        //    set { m_CompensationLeaveEntitleClaimType = value; modify("CompensationLeaveEntitleType"); }
        //}

        protected DateTime m_CompensationLeaveEntitleClaimPeriodFrom;
        [DBField("CompensationLeaveEntitleClaimPeriodFrom"), TextSearch, MaxLength(10), Export(false), Required]
        public DateTime CompensationLeaveEntitleClaimPeriodFrom
        {
            get { return m_CompensationLeaveEntitleClaimPeriodFrom; }
            set { m_CompensationLeaveEntitleClaimPeriodFrom = value; modify("CompensationLeaveEntitleClaimPeriodFrom"); }
        }

        protected DateTime m_CompensationLeaveEntitleClaimPeriodTo;
        [DBField("CompensationLeaveEntitleClaimPeriodTo"), TextSearch, MaxLength(10), Export(false), Required]
        public DateTime CompensationLeaveEntitleClaimPeriodTo
        {
            get { return m_CompensationLeaveEntitleClaimPeriodTo; }
            set { m_CompensationLeaveEntitleClaimPeriodTo = value; modify("CompensationLeaveEntitleClaimPeriodTo"); }
        }

        protected DateTime m_CompensationLeaveEntitleClaimHourFrom;
        [DBField("CompensationLeaveEntitleClaimHourFrom", "HH:mm"), TextSearch, MaxLength(5), Export(false)]
        public DateTime CompensationLeaveEntitleClaimHourFrom
        {
            get { return m_CompensationLeaveEntitleClaimHourFrom; }
            set { m_CompensationLeaveEntitleClaimHourFrom = value; modify("CompensationLeaveEntitleClaimHourFrom"); }
        }

        protected DateTime m_CompensationLeaveEntitleClaimHourTo;
        [DBField("CompensationLeaveEntitleClaimHourTo", "HH:mm"), TextSearch, MaxLength(5), Export(false)]
        public DateTime CompensationLeaveEntitleClaimHourTo
        {
            get { return m_CompensationLeaveEntitleClaimHourTo; }
            set { m_CompensationLeaveEntitleClaimHourTo = value; modify("CompensationLeaveEntitleClaimHourTo"); }
        }

        protected double m_CompensationLeaveEntitleHoursClaim;
        [DBField("CompensationLeaveEntitleHoursClaim", "0.####"), MaxLength(7), TextSearch, Export(false), Required]
        public double CompensationLeaveEntitleHoursClaim
        {
            get { return m_CompensationLeaveEntitleHoursClaim; }
            set { m_CompensationLeaveEntitleHoursClaim = value; modify("CompensationLeaveEntitleHoursClaim"); }
        }

        protected string m_CompensationLeaveEntitleApprovedBy;
        [DBField("CompensationLeaveEntitleApprovedBy"), TextSearch, Export(false)]
        public string CompensationLeaveEntitleApprovedBy
        {
            get { return m_CompensationLeaveEntitleApprovedBy; }
            set { m_CompensationLeaveEntitleApprovedBy = value; modify("CompensationLeaveEntitleApprovedBy"); }
        }

        protected string m_CompensationLeaveEntitleRemark;
        [DBField("CompensationLeaveEntitleRemark"), TextSearch, Export(false)]
        public string CompensationLeaveEntitleRemark
        {
            get { return m_CompensationLeaveEntitleRemark; }
            set { m_CompensationLeaveEntitleRemark = value; modify("CompensationLeaveEntitleRemark"); }
        }

        protected bool m_CompensationLeaveEntitleIsAutoGenerated;
        [DBField("CompensationLeaveEntitleIsAutoGenerated"), TextSearch, Export(false)]
        public bool CompensationLeaveEntitleIsAutoGenerated
        {
            get { return m_CompensationLeaveEntitleIsAutoGenerated; }
            set { m_CompensationLeaveEntitleIsAutoGenerated = value; modify("CompensationLeaveEntitleIsAutoGenerated"); }
        }

        protected DateTime m_CompensationLeaveEntitleDateExpiry;
        [DBField("CompensationLeaveEntitleDateExpiry"), TextSearch, MaxLength(10), Export(false)]
        public DateTime CompensationLeaveEntitleDateExpiry
        {
            get { return m_CompensationLeaveEntitleDateExpiry; }
            set { m_CompensationLeaveEntitleDateExpiry = value; modify("CompensationLeaveEntitleDateExpiry"); }
        }

        // Start 0000060, Miranda, 2014-07-22
        protected bool m_CompensationLeaveEntitleIsOTClaim;
        [DBField("CompensationLeaveEntitleIsOTClaim"), TextSearch, Export(false)]
        public bool CompensationLeaveEntitleIsOTClaim
        {
            get { return m_CompensationLeaveEntitleIsOTClaim; }
            set { m_CompensationLeaveEntitleIsOTClaim = value; modify("CompensationLeaveEntitleIsOTClaim"); }
        }
        // End 0000060, Miranda, 2014-07-22

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

            ELeaveBalance.DeleteLeaveBalanceAfter(dbConn, m_EmpID, ELeaveType.COMPENSATION_LEAVE_TYPE(dbConn).LeaveTypeID, m_CompensationLeaveEntitleEffectiveDate);

        }

        protected override void afterUpdate(DatabaseConnection dbConn, DBManager db)
        {
            DateTime oldLeaveAdjustDate = new DateTime();
            DateTime newLeaveAdjustDate = m_CompensationLeaveEntitleEffectiveDate;
            if (oldValueObject != null)
            {
                ECompensationLeaveEntitle oldCompLeaveEntitle = (ECompensationLeaveEntitle)oldValueObject;
                oldLeaveAdjustDate = oldCompLeaveEntitle.CompensationLeaveEntitleEffectiveDate;
            }
            {
                ELeaveBalance.DeleteLeaveBalanceAfter(dbConn, m_EmpID, ELeaveType.COMPENSATION_LEAVE_TYPE(dbConn).LeaveTypeID, oldLeaveAdjustDate);
                ELeaveBalance.DeleteLeaveBalanceAfter(dbConn, m_EmpID, ELeaveType.COMPENSATION_LEAVE_TYPE(dbConn).LeaveTypeID, newLeaveAdjustDate);
            }

            base.afterUpdate(dbConn, db);
        }

        protected override void afterDelete(DatabaseConnection dbConn, DBManager db)
        {
            base.afterDelete(dbConn, db);

            ELeaveBalance.DeleteLeaveBalanceAfter(dbConn, m_EmpID, ELeaveType.COMPENSATION_LEAVE_TYPE(dbConn).LeaveTypeID, m_CompensationLeaveEntitleEffectiveDate);
        }
    }
}
