using System;
using System.Collections.Generic;
using System.Text;
using HROne.DataAccess;
////using perspectivemind.validation;

namespace HROne.Lib.Entities
{
    [DBClass("RosterCode")]
    public class ERosterCode : BaseObject
    {

        public const string ROSTERTYPE_CODE_NORMAL = "N";
        public const string ROSTERTYPE_CODE_OVERNIGHT = "O";
        public const string ROSTERTYPE_CODE_RESTDAY = "R";
        public const string ROSTERTYPE_CODE_LEAVE = "L";
        public const string ROSTERTYPE_CODE_STATUTORYHOLIDAY = "S";
        public const string ROSTERTYPE_CODE_PUBLICHOLIDAY = "P";

        public static WFValueList VLRosterType = new AppUtils.NewWFTextList(new string[] { ROSTERTYPE_CODE_NORMAL, ROSTERTYPE_CODE_OVERNIGHT, ROSTERTYPE_CODE_RESTDAY, ROSTERTYPE_CODE_STATUTORYHOLIDAY, ROSTERTYPE_CODE_PUBLICHOLIDAY, ROSTERTYPE_CODE_LEAVE }, new string[] { "Normal", "Overnight", "Rest Day", "Statutory Holiday", "Public Holiday", "Leave Application" });

        public static DBManager db = new DBManager(typeof(ERosterCode));
        public static WFValueList VLRosterCode = new WFDBCodeList(ERosterCode.db, "RosterCodeID", "RosterCode", "RosterCodeDesc", "RosterCode");

        protected int m_RosterCodeID;
        [DBField("RosterCodeID", true, true), TextSearch, Export(false)]
        public int RosterCodeID
        {
            get { return m_RosterCodeID; }
            set { m_RosterCodeID = value; modify("RosterCodeID"); }
        }
        protected string m_RosterCode;
        [DBField("RosterCode"), TextSearch,MaxLength(20,10), Export(false),Required]
        public string RosterCode
        {
            get { return m_RosterCode; }
            set { m_RosterCode = value; modify("RosterCode"); }
        }
        protected string m_RosterCodeDesc;
        [DBField("RosterCodeDesc"), TextSearch, MaxLength(100, 25), Export(false), Required]
        public string RosterCodeDesc
        {
            get { return m_RosterCodeDesc; }
            set { m_RosterCodeDesc = value; modify("RosterCodeDesc"); }
        }
        protected string m_RosterCodeType;
        [DBField("RosterCodeType"), TextSearch, MaxLength(1, 1), Export(false), Required]
        public string RosterCodeType
        {
            get { return m_RosterCodeType; }
            set { m_RosterCodeType = value; modify("RosterCodeType"); }
        }
        protected int m_RosterClientID;
        [DBField("RosterClientID"), TextSearch, Export(false)]
        public int RosterClientID
        {
            get { return m_RosterClientID; }
            set { m_RosterClientID = value; modify("RosterClientID"); }
        }
        protected int m_RosterClientSiteID;
        [DBField("RosterClientSiteID"), TextSearch, Export(false)]
        public int RosterClientSiteID
        {
            get { return m_RosterClientSiteID; }
            set { m_RosterClientSiteID = value; modify("RosterClientSiteID"); }
        }
        protected DateTime m_RosterCodeInTime;
        [DBField("RosterCodeInTime","HH:mm"), TextSearch, MaxLength(5), Export(false), Required]
        public DateTime RosterCodeInTime
        {
            get { return m_RosterCodeInTime; }
            set { m_RosterCodeInTime = value; modify("RosterCodeInTime"); }
        }
        protected DateTime m_RosterCodeOutTime;
        [DBField("RosterCodeOutTime", "HH:mm"), TextSearch, MaxLength(5), Export(false), Required]
        public DateTime RosterCodeOutTime
        {
            get { return m_RosterCodeOutTime; }
            set { m_RosterCodeOutTime = value; modify("RosterCodeOutTime"); }
        }
        protected DateTime m_RosterCodeGraceInTime;
        [DBField("RosterCodeGraceInTime", "HH:mm"), TextSearch, MaxLength(5), Export(false)]
        public DateTime RosterCodeGraceInTime
        {
            get { return m_RosterCodeGraceInTime; }
            set { m_RosterCodeGraceInTime = value; modify("RosterCodeGraceInTime"); }
        }
        protected DateTime m_RosterCodeGraceOutTime;
        [DBField("RosterCodeGraceOutTime", "HH:mm"), TextSearch, MaxLength(5), Export(false)]
        public DateTime RosterCodeGraceOutTime
        {
            get { return m_RosterCodeGraceOutTime; }
            set { m_RosterCodeGraceOutTime = value; modify("RosterCodeGraceOutTime"); }
        }
        protected bool m_RosterCodeHasLunch;
        [DBField("RosterCodeHasLunch"), TextSearch, Export(false)]
        public bool RosterCodeHasLunch
        {
            get { return m_RosterCodeHasLunch; }
            set { m_RosterCodeHasLunch = value; modify("RosterCodeHasLunch"); }
        }
        protected DateTime m_RosterCodeLunchStartTime;
        [DBField("RosterCodeLunchStartTime", "HH:mm"), TextSearch, MaxLength(5), Export(false)]
        public DateTime RosterCodeLunchStartTime
        {
            get { return m_RosterCodeLunchStartTime; }
            set { m_RosterCodeLunchStartTime = value; modify("RosterCodeLunchStartTime"); }
        }
        protected DateTime m_RosterCodeLunchEndTime;
        [DBField("RosterCodeLunchEndTime", "HH:mm"), TextSearch, MaxLength(5), Export(false)]
        public DateTime RosterCodeLunchEndTime
        {
            get { return m_RosterCodeLunchEndTime; }
            set { m_RosterCodeLunchEndTime = value; modify("RosterCodeLunchEndTime"); }
        }

        protected bool m_RosterCodeLunchIsDeductWorkingHour;
        [DBField("RosterCodeLunchIsDeductWorkingHour"), TextSearch, Export(false)]
        public bool RosterCodeLunchIsDeductWorkingHour
        {
            get { return m_RosterCodeLunchIsDeductWorkingHour; }
            set { m_RosterCodeLunchIsDeductWorkingHour = value; modify("RosterCodeLunchIsDeductWorkingHour"); }
        }
        protected int m_RosterCodeLunchDeductWorkingHourMinsUnit;
        [DBField("RosterCodeLunchDeductWorkingHourMinsUnit"), TextSearch, MaxLength(5), Export(false)]
        public int RosterCodeLunchDeductWorkingHourMinsUnit
        {
            get { return m_RosterCodeLunchDeductWorkingHourMinsUnit; }
            set { m_RosterCodeLunchDeductWorkingHourMinsUnit = value; modify("RosterCodeLunchDeductWorkingHourMinsUnit"); }
        }

        protected string m_RosterCodeLunchDeductWorkingHourMinsRoundingRule;
        [DBField("RosterCodeLunchDeductWorkingHourMinsRoundingRule"), TextSearch, MaxLength(50), Export(false)]
        public string RosterCodeLunchDeductWorkingHourMinsRoundingRule
        {
            get { return m_RosterCodeLunchDeductWorkingHourMinsRoundingRule; }
            set { m_RosterCodeLunchDeductWorkingHourMinsRoundingRule = value; modify("RosterCodeLunchDeductWorkingHourMinsRoundingRule"); }
        }
        protected bool m_RosterCodeHasOT;
        [DBField("RosterCodeHasOT"), TextSearch, Export(false)]
        public bool RosterCodeHasOT
        {
            get { return m_RosterCodeHasOT; }
            set { m_RosterCodeHasOT = value; modify("RosterCodeHasOT"); }
        }
        protected DateTime m_RosterCodeOTStartTime;
        [DBField("RosterCodeOTStartTime", "HH:mm"), TextSearch, MaxLength(5), Export(false)]
        public DateTime RosterCodeOTStartTime
        {
            get { return m_RosterCodeOTStartTime; }
            set { m_RosterCodeOTStartTime = value; modify("RosterCodeOTStartTime"); }
        }
        protected DateTime m_RosterCodeOTEndTime;
        [DBField("RosterCodeOTEndTime", "HH:mm"), TextSearch, MaxLength(5), Export(false)]
        public DateTime RosterCodeOTEndTime
        {
            get { return m_RosterCodeOTEndTime; }
            set { m_RosterCodeOTEndTime = value; modify("RosterCodeOTEndTime"); }
        }
        protected bool m_RosterCodeIsOTStartFromOutTime;
        [DBField("RosterCodeIsOTStartFromOutTime"), TextSearch, MaxLength(5), Export(false)]
        public bool RosterCodeIsOTStartFromOutTime
        {
            get { return m_RosterCodeIsOTStartFromOutTime; }
            set { m_RosterCodeIsOTStartFromOutTime = value; modify("RosterCodeIsOTStartFromOutTime"); }
        }

        protected bool m_RosterCodeOTIncludeLunch;
        [DBField("RosterCodeOTIncludeLunch"), TextSearch, Export(false)]
        public bool RosterCodeOTIncludeLunch
        {
            get { return m_RosterCodeOTIncludeLunch; }
            set { m_RosterCodeOTIncludeLunch = value; modify("RosterCodeOTIncludeLunch"); }
        }

        protected bool m_RosterCodeOTShiftStartTimeForLate;
        [DBField("RosterCodeOTShiftStartTimeForLate"), TextSearch, Export(false)]
        public bool RosterCodeOTShiftStartTimeForLate
        {
            get { return m_RosterCodeOTShiftStartTimeForLate; }
            set { m_RosterCodeOTShiftStartTimeForLate = value; modify("RosterCodeOTShiftStartTimeForLate"); }
        }

        protected DateTime m_RosterCodeCutOffTime;
        [DBField("RosterCodeCutOffTime", "HH:mm"), TextSearch, MaxLength(5), Export(false)]
        public DateTime RosterCodeCutOffTime
        {
            get { return m_RosterCodeCutOffTime; }
            set { m_RosterCodeCutOffTime = value; modify("RosterCodeCutOffTime"); }
        }
        protected double m_RosterCodeWorkingDayUnit;
        [DBField("RosterCodeWorkingDayUnit", "0.###"), TextSearch, MaxLength(6), Export(false), Required]
        public double RosterCodeWorkingDayUnit
        {
            get { return m_RosterCodeWorkingDayUnit; }
            set { m_RosterCodeWorkingDayUnit = value; modify("RosterCodeWorkingDayUnit"); }
        }
        protected double m_RosterCodeDailyWorkingHour;
        [DBField("RosterCodeDailyWorkingHour", "0.###"), TextSearch, MaxLength(6), Export(false), Required]
        public double RosterCodeDailyWorkingHour
        {
            get { return m_RosterCodeDailyWorkingHour; }
            set { m_RosterCodeDailyWorkingHour = value; modify("RosterCodeDailyWorkingHour"); }
        }

        protected bool m_RosterCodeCountWorkHourOnly;
        [DBField("RosterCodeCountWorkHourOnly"), TextSearch, Export(false)]
        public bool RosterCodeCountWorkHourOnly
        {
            get { return m_RosterCodeCountWorkHourOnly; }
            set { m_RosterCodeCountWorkHourOnly = value; modify("RosterCodeCountWorkHourOnly"); }
        }

        protected int m_RosterCodeCountOTAfterWorkHourMin;
        [DBField("RosterCodeCountOTAfterWorkHourMin"), TextSearch, MaxLength(3), Export(false)]
        public int RosterCodeCountOTAfterWorkHourMin
        {
            get { return m_RosterCodeCountOTAfterWorkHourMin; }
            set { m_RosterCodeCountOTAfterWorkHourMin = value; modify("RosterCodeCountOTAfterWorkHourMin"); }
        }

        protected int m_RosterCodeOTMinsUnit;
        [DBField("RosterCodeOTMinsUnit"), TextSearch, MaxLength(2), Export(false)]
        public int RosterCodeOTMinsUnit
        {
            get { return m_RosterCodeOTMinsUnit; }
            set { m_RosterCodeOTMinsUnit = value; modify("RosterCodeOTMinsUnit"); }
        }

        protected string m_RosterCodeOTMinsRoundingRule;
        [DBField("RosterCodeOTMinsRoundingRule"), TextSearch, MaxLength(50), Export(false)]
        public string RosterCodeOTMinsRoundingRule
        {
            get { return m_RosterCodeOTMinsRoundingRule; }
            set { m_RosterCodeOTMinsRoundingRule = value; modify("RosterCodeOTMinsRoundingRule"); }
        }

        protected bool m_RosterCodeIsOverrideHourlyPayment;
        [DBField("RosterCodeIsOverrideHourlyPayment"), TextSearch, Export(false)]
        public bool RosterCodeIsOverrideHourlyPayment
        {
            get { return m_RosterCodeIsOverrideHourlyPayment; }
            set { m_RosterCodeIsOverrideHourlyPayment = value; modify("RosterCodeIsOverrideHourlyPayment"); }
        }
        protected double m_RosterCodeOverrideHoulyAmount;
        [DBField("RosterCodeOverrideHoulyAmount"), TextSearch, Export(false)]
        public double RosterCodeOverrideHoulyAmount
        {
            get { return m_RosterCodeOverrideHoulyAmount; }
            set { m_RosterCodeOverrideHoulyAmount = value; modify("RosterCodeOverrideHoulyAmount"); }
        }
        protected int m_LeaveCodeID;
        [DBField("LeaveCodeID"), TextSearch, Export(false)]
        public int LeaveCodeID
        {
            get { return m_LeaveCodeID; }
            set { m_LeaveCodeID = value; modify("LeaveCodeID"); }
        }

        protected bool m_RosterCodeUseHalfWorkingDaysHours;
        [DBField("RosterCodeUseHalfWorkingDaysHours"), TextSearch, Export(false)]
        public bool RosterCodeUseHalfWorkingDaysHours
        {
            get { return m_RosterCodeUseHalfWorkingDaysHours; }
            set { m_RosterCodeUseHalfWorkingDaysHours = value; modify("RosterCodeUseHalfWorkingDaysHours"); }
        }
        protected double m_RosterCodeUseHalfWorkingDaysHoursMaxWorkingHours;
        [DBField("RosterCodeUseHalfWorkingDaysHoursMaxWorkingHours", "0.00"), TextSearch, MaxLength(6), Export(false)]
        public double RosterCodeUseHalfWorkingDaysHoursMaxWorkingHours
        {
            get { return m_RosterCodeUseHalfWorkingDaysHoursMaxWorkingHours; }
            set { m_RosterCodeUseHalfWorkingDaysHoursMaxWorkingHours = value; modify("RosterCodeUseHalfWorkingDaysHoursMaxWorkingHours"); }
        }

        protected string m_RosterCodeColorCode;
        [DBField("RosterCodeColorCode"), TextSearch, MaxLength(10), Export(false)]
        public string RosterCodeColorCode
        {
            get { return m_RosterCodeColorCode; }
            set { m_RosterCodeColorCode = value; modify("RosterCodeColorCode"); }
        }

        protected double m_RosterCodeLunchDurationHour;
        [DBField("RosterCodeLunchDurationHour", "0.###"), TextSearch, MaxLength(6), Export(false)]
        public double RosterCodeLunchDurationHour
        {
            get { return m_RosterCodeLunchDurationHour; }
            set { m_RosterCodeLunchDurationHour = value; modify("RosterCodeLunchDurationHour"); }
        }

        protected double m_RosterCodeLunchDeductMinimumWorkHour;
        [DBField("RosterCodeLunchDeductMinimumWorkHour", "0.###"), MaxLength(6), TextSearch, Export(false)]
        public double RosterCodeLunchDeductMinimumWorkHour
        {
            get { return m_RosterCodeLunchDeductMinimumWorkHour; }
            set { m_RosterCodeLunchDeductMinimumWorkHour = value; modify("RosterCodeLunchDeductMinimumWorkHour"); }
        }

        protected int m_CostCenterID;
        [DBField("CostCenterID"), TextSearch, Export(false)]
        public int CostCenterID
        {
            get { return m_CostCenterID; }
            set { m_CostCenterID = value; modify("CostCenterID"); }
        }

        protected int m_RosterCodeLateMinsUnit;
        [DBField("RosterCodeLateMinsUnit"), TextSearch, MaxLength(3), Export(false)]
        public int RosterCodeLateMinsUnit
        {
            get { return m_RosterCodeLateMinsUnit; }
            set { m_RosterCodeLateMinsUnit = value; modify("RosterCodeLateMinsUnit"); }
        }
        protected string m_RosterCodeLateMinsRoundingRule;
        [DBField("RosterCodeLateMinsRoundingRule"), TextSearch, MaxLength(50), Export(false)]
        public string RosterCodeLateMinsRoundingRule
        {
            get { return m_RosterCodeLateMinsRoundingRule; }
            set { m_RosterCodeLateMinsRoundingRule = value; modify("RosterCodeLateMinsRoundingRule"); }
        }

        protected int m_RosterCodeEarlyLeaveMinsUnit;
        [DBField("RosterCodeEarlyLeaveMinsUnit"), TextSearch, MaxLength(3), Export(false)]
        public int RosterCodeEarlyLeaveMinsUnit
        {
            get { return m_RosterCodeEarlyLeaveMinsUnit; }
            set { m_RosterCodeEarlyLeaveMinsUnit = value; modify("RosterCodeEarlyLeaveMinsUnit"); }
        }
        protected string m_RosterCodeEarlyLeaveMinsRoundingRule;
        [DBField("RosterCodeEarlyLeaveMinsRoundingRule"), TextSearch, MaxLength(50), Export(false)]
        public string RosterCodeEarlyLeaveMinsRoundingRule
        {
            get { return m_RosterCodeEarlyLeaveMinsRoundingRule; }
            set { m_RosterCodeEarlyLeaveMinsRoundingRule = value; modify("RosterCodeEarlyLeaveMinsRoundingRule"); }
        }

        protected int m_RosterCodeLunchLateMinsUnit;
        [DBField("RosterCodeLunchLateMinsUnit"), TextSearch, MaxLength(3), Export(false)]
        public int RosterCodeLunchLateMinsUnit
        {
            get { return m_RosterCodeLunchLateMinsUnit; }
            set { m_RosterCodeLunchLateMinsUnit = value; modify("RosterCodeLunchLateMinsUnit"); }
        }
        protected string m_RosterCodeLunchLateMinsRoundingRule;
        [DBField("RosterCodeLunchLateMinsRoundingRule"), TextSearch, MaxLength(50), Export(false)]
        public string RosterCodeLunchLateMinsRoundingRule
        {
            get { return m_RosterCodeLunchLateMinsRoundingRule; }
            set { m_RosterCodeLunchLateMinsRoundingRule = value; modify("RosterCodeLunchLateMinsRoundingRule"); }
        }

        protected int m_RosterCodeLunchEarlyLeaveMinsUnit;
        [DBField("RosterCodeLunchEarlyLeaveMinsUnit"), TextSearch, MaxLength(3), Export(false)]
        public int RosterCodeLunchEarlyLeaveMinsUnit
        {
            get { return m_RosterCodeLunchEarlyLeaveMinsUnit; }
            set { m_RosterCodeLunchEarlyLeaveMinsUnit = value; modify("RosterCodeLunchEarlyLeaveMinsUnit"); }
        }
        protected string m_RosterCodeLunchEarlyLeaveMinsRoundingRule;
        [DBField("RosterCodeLunchEarlyLeaveMinsRoundingRule"), TextSearch, MaxLength(50), Export(false)]
        public string RosterCodeLunchEarlyLeaveMinsRoundingRule
        {
            get { return m_RosterCodeLunchEarlyLeaveMinsRoundingRule; }
            set { m_RosterCodeLunchEarlyLeaveMinsRoundingRule = value; modify("RosterCodeLunchEarlyLeaveMinsRoundingRule"); }
        }


        //protected int m_RosterCodeOTMinsUnit;
        //[DBField("RosterCodeOTMinsUnit"), TextSearch, Export(false)]
        //public int RosterCodeOTMinsUnit
        //{
        //    get { return m_RosterCodeOTMinsUnit; }
        //    set { m_RosterCodeOTMinsUnit = value; modify("RosterCodeOTMinsUnit"); }
        //}
        //public ERosterCodeDetail GetRosterCodeDetail(double NoOfHour)
        //{
        //    DBFilter RosterCodeDetailFilter = new DBFilter();
        //    RosterCodeDetailFilter.add(new Match("RosterCodeID", RosterCodeID));
        //    RosterCodeDetailFilter.add(new Match("RosterCodeDetailNoOfHour", ">=", NoOfHour));
        //    RosterCodeDetailFilter.add("RosterCodeDetailYearOfService", true);
        //    System.Collections.ArrayList RosterCodeDetailList = ERosterCodeDetail.db.select(dbConn, RosterCodeDetailFilter);
        //    if (RosterCodeDetailList.Count > 0)
        //        return (ERosterCodeDetail)RosterCodeDetailList[0];
        //    else
        //        return null;

        //}

        protected DateTime m_RosterCodeDayStartTime = new DateTime();
        public DateTime RosterCodeDayStartTime
        {
            get 
            {
                if (m_RosterCodeDayStartTime.Ticks.Equals(0))
                    return m_RosterCodeCutOffTime.AddDays(-1);
                else
                    return m_RosterCodeDayStartTime;
            }
            set { m_RosterCodeDayStartTime = value; modify("RosterCodeDayStartTime"); }
        }

        public int GetDefaultLunchTimeMins()
        {
            if (!RosterCodeLunchStartTime.Ticks.Equals(0) && !RosterCodeLunchEndTime.Ticks.Equals(0))
            {
                TimeSpan lunchTimeSpan = RosterCodeLunchEndTime.TimeOfDay.Subtract(RosterCodeLunchStartTime.TimeOfDay);
                if (lunchTimeSpan.TotalMinutes < 0)
                    lunchTimeSpan = lunchTimeSpan.Add(new TimeSpan(1, 0, 0, 0));
                return Convert.ToInt32(lunchTimeSpan.TotalMinutes);
            }
            else
                return 0;
        }
    }


}
