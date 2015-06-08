using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using HROne.DataAccess;
////using perspectivemind.validation;

namespace HROne.Lib.Entities
{
    [DBClass("LeaveType")]
    public class ELeaveType : BaseObject
    {
        public static DBManager db = new DBManager(typeof(ELeaveType));
        public static WFValueList VLLeaveType = new AppUtils.EncryptedDBCodeList(ELeaveType.db, "LeaveTypeID", new string[] { "LeaveType", "LeaveTypeDesc" }, " - ", "LeaveType");

        public const string LEAVETYPECODE_ANNUAL = "ANNUAL";
        public const string LEAVETYPECODE_SLCAT1 = "SLCAT1";
        public const string LEAVETYPECODE_SLCAT2 = "SLCAT2";
        public const string LEAVETYPECODE_INJURY = "INJURY";
        public const string LEAVETYPECODE_COMPENSATION = "COMPENSATION";
        public const string LEAVETYPECODE_RESTDAY = "REST";
        public const string LEAVETYPECODE_STATUTORYHOLIDAY = "STATUTORYHOLIDAY";
        public const string LEAVETYPECODE_PUBLICHOLIDAY = "PUBLICHOLIDAY";
        public const string LEAVETYPECODE_BIRTHDAY = "BIRTHDAY";

        public static ELeaveType ANNUAL_LEAVE_TYPE(DatabaseConnection dbConn)
        {
                return GetObject(dbConn, LEAVETYPECODE_ANNUAL);
        }

        public static ELeaveType SLCAT1_LEAVE_TYPE(DatabaseConnection dbConn)
        {
                return GetObject(dbConn, LEAVETYPECODE_SLCAT1);
        }

        public static ELeaveType SLCAT2_LEAVE_TYPE(DatabaseConnection dbConn)
        {
                return GetObject(dbConn, LEAVETYPECODE_SLCAT2);
        }

        public static ELeaveType INJURY_LEAVE_TYPE(DatabaseConnection dbConn)
        {
                return GetObject(dbConn, LEAVETYPECODE_INJURY);
        }

        public static ELeaveType COMPENSATION_LEAVE_TYPE(DatabaseConnection dbConn)
        {
                return GetObject(dbConn, LEAVETYPECODE_COMPENSATION);
        }

        public static ELeaveType STATUTORYHOLIDAY_LEAVE_TYPE(DatabaseConnection dbConn)
        {
                return GetObject(dbConn, LEAVETYPECODE_STATUTORYHOLIDAY);
        }

        public static ELeaveType PUBLICHOLIDAY_LEAVE_TYPE(DatabaseConnection dbConn)
        {
            return GetObject(dbConn, LEAVETYPECODE_PUBLICHOLIDAY);
        }
        public static ELeaveType RESTDAY_LEAVE_TYPE(DatabaseConnection dbConn)
        {
                return GetObject(dbConn, LEAVETYPECODE_RESTDAY);
        }

        public static ELeaveType BITYHDAY_LEAVE_TYPE(DatabaseConnection dbConn)
        {
            return GetObject(dbConn, LEAVETYPECODE_BIRTHDAY);
        }
        
        protected int m_LeaveTypeID;
        [DBField("LeaveTypeID", true, true), TextSearch, Export(false)]
        public int LeaveTypeID
        {
            get { return m_LeaveTypeID; }
            set { m_LeaveTypeID = value; modify("LeaveTypeID"); }
        }
        protected string m_LeaveType;
        [DBField("LeaveType"), TextSearch,MaxLength(20), Export(false),Required]
        public string LeaveType
        {
            get { return m_LeaveType; }
            set { m_LeaveType = value.Trim().ToUpper(); modify("LeaveType"); }
        }
        protected string m_LeaveTypeDesc;
        [DBField("LeaveTypeDesc"), TextSearch,MaxLength(100, 40), Export(false), Required]
        public string LeaveTypeDesc
        {
            get { return m_LeaveTypeDesc; }
            set { m_LeaveTypeDesc = value; modify("LeaveTypeDesc"); }
        }
        protected int m_LeaveDecimalPlace;
        [DBField("LeaveDecimalPlace"), TextSearch, MaxLength(1),IntRange(0,4), Export(false), Required]
        public int LeaveDecimalPlace
        {
            get { return m_LeaveDecimalPlace; }
            set { m_LeaveDecimalPlace = value; modify("LeaveDecimalPlace"); }
        }

        protected bool m_LeaveTypeIsUseWorkHourPattern;
        [DBField("LeaveTypeIsUseWorkHourPattern"), TextSearch, Export(false), Required]
        public bool LeaveTypeIsUseWorkHourPattern
        {
            get { return m_LeaveTypeIsUseWorkHourPattern; }
            set { m_LeaveTypeIsUseWorkHourPattern = value; modify("LeaveTypeIsUseWorkHourPattern"); }
        }

        protected bool m_LeaveTypeIsSkipStatutoryHolidayChecking;
        [DBField("LeaveTypeIsSkipStatutoryHolidayChecking"), TextSearch, Export(false), Required]
        public bool LeaveTypeIsSkipStatutoryHolidayChecking
        {
            get { return m_LeaveTypeIsSkipStatutoryHolidayChecking; }
            set { m_LeaveTypeIsSkipStatutoryHolidayChecking = value; modify("LeaveTypeIsSkipStatutoryHolidayChecking"); }
        }
        
        protected bool m_LeaveTypeIsSkipPublicHolidayChecking;
        [DBField("LeaveTypeIsSkipPublicHolidayChecking"), TextSearch, Export(false), Required]
        public bool LeaveTypeIsSkipPublicHolidayChecking
        {
            get { return m_LeaveTypeIsSkipPublicHolidayChecking; }
            set { m_LeaveTypeIsSkipPublicHolidayChecking = value; modify("LeaveTypeIsSkipPublicHolidayChecking"); }
        }

        protected bool m_LeaveTypeIsESSHideLeaveBalance;
        [DBField("LeaveTypeIsESSHideLeaveBalance"), TextSearch, Export(false), Required]
        public bool LeaveTypeIsESSHideLeaveBalance
        {
            get { return m_LeaveTypeIsESSHideLeaveBalance; }
            set { m_LeaveTypeIsESSHideLeaveBalance = value; modify("LeaveTypeIsESSHideLeaveBalance"); }
        }

        protected bool m_LeaveTypeIsESSIgnoreEntitlement;
        [DBField("LeaveTypeIsESSIgnoreEntitlement"), TextSearch, Export(false), Required]
        public bool LeaveTypeIsESSIgnoreEntitlement
        {
            get { return m_LeaveTypeIsESSIgnoreEntitlement; }
            set { m_LeaveTypeIsESSIgnoreEntitlement = value; modify("LeaveTypeIsESSIgnoreEntitlement"); }
        }
        
        protected bool m_LeaveTypeIsESSRestrictNegativeBalanceAsOfToday;
        [DBField("LeaveTypeIsESSRestrictNegativeBalanceAsOfToday"), TextSearch, Export(false), Required]
        public bool LeaveTypeIsESSRestrictNegativeBalanceAsOfToday
        {
            get { return m_LeaveTypeIsESSRestrictNegativeBalanceAsOfToday; }
            set { m_LeaveTypeIsESSRestrictNegativeBalanceAsOfToday = value; modify("LeaveTypeIsESSRestrictNegativeBalanceAsOfToday"); }
        }

        protected bool m_LeaveTypeIsESSRestrictNegativeBalanceAsOfApplicationDateFrom;
        [DBField("LeaveTypeIsESSRestrictNegativeBalanceAsOfApplicationDateFrom"), TextSearch, Export(false), Required]
        public bool LeaveTypeIsESSRestrictNegativeBalanceAsOfApplicationDateFrom
        {
            get { return m_LeaveTypeIsESSRestrictNegativeBalanceAsOfApplicationDateFrom; }
            set { m_LeaveTypeIsESSRestrictNegativeBalanceAsOfApplicationDateFrom = value; modify("LeaveTypeIsESSRestrictNegativeBalanceAsOfApplicationDateFrom"); }
        }

        protected bool m_LeaveTypeIsESSRestrictNegativeBalanceAsOfApplicationDateTo;
        [DBField("LeaveTypeIsESSRestrictNegativeBalanceAsOfApplicationDateTo"), TextSearch, Export(false), Required]
        public bool LeaveTypeIsESSRestrictNegativeBalanceAsOfApplicationDateTo
        {
            get { return m_LeaveTypeIsESSRestrictNegativeBalanceAsOfApplicationDateTo; }
            set { m_LeaveTypeIsESSRestrictNegativeBalanceAsOfApplicationDateTo = value; modify("LeaveTypeIsESSRestrictNegativeBalanceAsOfApplicationDateTo"); }
        }

        protected bool m_LeaveTypeIsESSRestrictNegativeBalanceAsOfEndOfLeaveYear;
        [DBField("LeaveTypeIsESSRestrictNegativeBalanceAsOfEndOfLeaveYear"), TextSearch, Export(false), Required]
        public bool LeaveTypeIsESSRestrictNegativeBalanceAsOfEndOfLeaveYear
        {
            get { return m_LeaveTypeIsESSRestrictNegativeBalanceAsOfEndOfLeaveYear; }
            set { m_LeaveTypeIsESSRestrictNegativeBalanceAsOfEndOfLeaveYear = value; modify("LeaveTypeIsESSRestrictNegativeBalanceAsOfEndOfLeaveYear"); }
        }

        // Start 0000093, Ricky So, 2014-09-05
        protected double m_LeaveTypeIsESSAllowableAdvanceBalance;
        [DBField("LeaveTypeIsESSAllowableAdvanceBalance", "##0.00"), MaxLength(6), TextSearch, Export(false)]
        public double LeaveTypeIsESSAllowableAdvanceBalance
        {
            get { return m_LeaveTypeIsESSAllowableAdvanceBalance; }
            set { m_LeaveTypeIsESSAllowableAdvanceBalance = value; modify("LeaveTypeIsESSAllowableAdvanceBalance"); }
        }
        // End 0000093, Ricky So, 2014-09-05

        protected bool m_LeaveTypeIsDisabled;
        [DBField("LeaveTypeIsDisabled"), TextSearch, Export(false), Required]
        public bool LeaveTypeIsDisabled
        {
            get { return m_LeaveTypeIsDisabled; }
            set { m_LeaveTypeIsDisabled = value; modify("LeaveTypeIsDisabled"); }
        }


        public bool IsSystemUse()
        {
            return (m_LeaveType.Equals(LEAVETYPECODE_ANNUAL, StringComparison.CurrentCultureIgnoreCase)
            || m_LeaveType.Equals(LEAVETYPECODE_SLCAT1, StringComparison.CurrentCultureIgnoreCase)
            || m_LeaveType.Equals(LEAVETYPECODE_SLCAT2, StringComparison.CurrentCultureIgnoreCase)
            || m_LeaveType.Equals(LEAVETYPECODE_INJURY, StringComparison.CurrentCultureIgnoreCase)
            || m_LeaveType.Equals(LEAVETYPECODE_COMPENSATION, StringComparison.CurrentCultureIgnoreCase)
            || m_LeaveType.Equals(LEAVETYPECODE_RESTDAY, StringComparison.CurrentCultureIgnoreCase)
            || m_LeaveType.Equals(LEAVETYPECODE_STATUTORYHOLIDAY, StringComparison.CurrentCultureIgnoreCase)
            || m_LeaveType.Equals(LEAVETYPECODE_PUBLICHOLIDAY, StringComparison.CurrentCultureIgnoreCase)
            || m_LeaveType.Equals(LEAVETYPECODE_BIRTHDAY, StringComparison.CurrentCultureIgnoreCase)
            
            );
        }

        public static ELeaveType GetObject(DatabaseConnection dbConn, string LeaveTypeCode)
        {
            DBFilter filter = new DBFilter();
            filter.add(new Match("LeaveType", LeaveTypeCode));
            ArrayList leaveTypeList = ELeaveType.db.select(dbConn, filter);
            if (leaveTypeList.Count > 0)
            {
                return (ELeaveType)leaveTypeList[0];
            }
            else
                return new ELeaveType();
        }

        public static ELeaveType GetObject(DatabaseConnection dbConn, int LeaveTypeID)
        {
            DBFilter filter = new DBFilter();
            filter.add(new Match("LeaveTypeID", LeaveTypeID));
            ArrayList leaveTypeList = ELeaveType.db.select(dbConn, filter);
            if (leaveTypeList.Count > 0)
            {
                return (ELeaveType)leaveTypeList[0];
            }
            else
                return new ELeaveType();
        }

    }
}
