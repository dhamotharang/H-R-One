using System;
using System.Text;
using HROne.DataAccess;
////using perspectivemind.validation;

namespace HROne.Lib.Entities
{
    [DBClass("WorkHourPattern")]
    public class EWorkHourPattern : BaseObject
    {
        public static DBManager db = new DBManager(typeof(EWorkHourPattern));

        public const string WORKDAYDETERMINDMETHOD_ROSTERTABLE = "R";
        public const string WORKDAYDETERMINDMETHOD_MANUALINPUT = "I";

        public static WFValueList VLWorkHourPattern = new WFDBCodeList(EWorkHourPattern.db, "WorkHourPatternID", "WorkHourPatternCode", "WorkHourPatternDesc", "WorkHourPatternCode");
        public static WFValueList VLWorkDayDetermineMethod = new AppUtils.NewWFTextList(new string[] { WORKDAYDETERMINDMETHOD_ROSTERTABLE, WORKDAYDETERMINDMETHOD_MANUALINPUT }, new string[] { "Use Roster Table", "Manual Input" });

        protected int m_WorkHourPatternID;
        [DBField("WorkHourPatternID", true, true), TextSearch, Export(false)]
        public int WorkHourPatternID
        {
            get { return m_WorkHourPatternID; }
            set { m_WorkHourPatternID = value; modify("WorkHourPatternID"); }
        }
        protected string m_WorkHourPatternCode;
        [DBField("WorkHourPatternCode"), TextSearch, MaxLength(20, 10), Export(false), Required]
        public string WorkHourPatternCode
        {
            get { return m_WorkHourPatternCode; }
            set { m_WorkHourPatternCode = value; modify("WorkHourPatternCode"); }
        }

        protected string m_WorkHourPatternDesc;
        [DBField("WorkHourPatternDesc"), TextSearch, MaxLength(100, 100), Export(false), Required]
        public string WorkHourPatternDesc
        {
            get { return m_WorkHourPatternDesc; }
            set { m_WorkHourPatternDesc = value; modify("WorkHourPatternDesc"); }
        }

        protected string m_WorkHourPatternWorkDayDetermineMethod;
        [DBField("WorkHourPatternWorkDayDetermineMethod"), TextSearch, Export(false), Required]
        public string WorkHourPatternWorkDayDetermineMethod
        {
            get { return m_WorkHourPatternWorkDayDetermineMethod; }
            set { m_WorkHourPatternWorkDayDetermineMethod = value; modify("WorkHourPatternWorkDayDetermineMethod"); }
        }

        protected int m_WorkHourPatternSunDefaultRosterCodeID;
        [DBField("WorkHourPatternSunDefaultRosterCodeID"), TextSearch, Export(false)]
        public int WorkHourPatternSunDefaultRosterCodeID
        {
            get { return m_WorkHourPatternSunDefaultRosterCodeID; }
            set { m_WorkHourPatternSunDefaultRosterCodeID = value; modify("WorkHourPatternSunDefaultRosterCodeID"); }
        }

        protected double m_WorkHourPatternSunDefaultDayUnit;
        [DBField("WorkHourPatternSunDefaultDayUnit", "0.###"), TextSearch, Export(false), MaxLength(5)]
        public double WorkHourPatternSunDefaultDayUnit
        {
            get { return m_WorkHourPatternSunDefaultDayUnit; }
            set { m_WorkHourPatternSunDefaultDayUnit = value; modify("WorkHourPatternSunDefaultDayUnit"); }
        }

        protected int m_WorkHourPatternMonDefaultRosterCodeID;
        [DBField("WorkHourPatternMonDefaultRosterCodeID"), TextSearch, Export(false)]
        public int WorkHourPatternMonDefaultRosterCodeID
        {
            get { return m_WorkHourPatternMonDefaultRosterCodeID; }
            set { m_WorkHourPatternMonDefaultRosterCodeID = value; modify("WorkHourPatternMonDefaultRosterCodeID"); }
        }

        protected double m_WorkHourPatternMonDefaultDayUnit;
        [DBField("WorkHourPatternMonDefaultDayUnit", "0.###"), TextSearch, Export(false), MaxLength(5)]
        public double WorkHourPatternMonDefaultDayUnit
        {
            get { return m_WorkHourPatternMonDefaultDayUnit; }
            set { m_WorkHourPatternMonDefaultDayUnit = value; modify("WorkHourPatternMonDefaultDayUnit"); }
        }

        protected int m_WorkHourPatternTueDefaultRosterCodeID;
        [DBField("WorkHourPatternTueDefaultRosterCodeID"), TextSearch, Export(false)]
        public int WorkHourPatternTueDefaultRosterCodeID
        {
            get { return m_WorkHourPatternTueDefaultRosterCodeID; }
            set { m_WorkHourPatternTueDefaultRosterCodeID = value; modify("WorkHourPatternTueDefaultRosterCodeID"); }
        }

        protected double m_WorkHourPatternTueDefaultDayUnit;
        [DBField("WorkHourPatternTueDefaultDayUnit", "0.###"), TextSearch, Export(false), MaxLength(5)]
        public double WorkHourPatternTueDefaultDayUnit
        {
            get { return m_WorkHourPatternTueDefaultDayUnit; }
            set { m_WorkHourPatternTueDefaultDayUnit = value; modify("WorkHourPatternTueDefaultDayUnit"); }
        }

        protected int m_WorkHourPatternWedDefaultRosterCodeID;
        [DBField("WorkHourPatternWedDefaultRosterCodeID"), TextSearch, Export(false)]
        public int WorkHourPatternWedDefaultRosterCodeID
        {
            get { return m_WorkHourPatternWedDefaultRosterCodeID; }
            set { m_WorkHourPatternWedDefaultRosterCodeID = value; modify("WorkHourPatternWedDefaultRosterCodeID"); }
        }

        protected double m_WorkHourPatternWedDefaultDayUnit;
        [DBField("WorkHourPatternWedDefaultDayUnit", "0.###"), TextSearch, Export(false), MaxLength(5)]
        public double WorkHourPatternWedDefaultDayUnit
        {
            get { return m_WorkHourPatternWedDefaultDayUnit; }
            set { m_WorkHourPatternWedDefaultDayUnit = value; modify("WorkHourPatternWedDefaultDayUnit"); }
        }

        protected int m_WorkHourPatternThuDefaultRosterCodeID;
        [DBField("WorkHourPatternThuDefaultRosterCodeID"), TextSearch, Export(false)]
        public int WorkHourPatternThuDefaultRosterCodeID
        {
            get { return m_WorkHourPatternThuDefaultRosterCodeID; }
            set { m_WorkHourPatternThuDefaultRosterCodeID = value; modify("WorkHourPatternThuDefaultRosterCodeID"); }
        }

        protected double m_WorkHourPatternThuDefaultDayUnit;
        [DBField("WorkHourPatternThuDefaultDayUnit", "0.###"), TextSearch, Export(false), MaxLength(5)]
        public double WorkHourPatternThuDefaultDayUnit
        {
            get { return m_WorkHourPatternThuDefaultDayUnit; }
            set { m_WorkHourPatternThuDefaultDayUnit = value; modify("WorkHourPatternThuDefaultDayUnit"); }
        }

        protected int m_WorkHourPatternFriDefaultRosterCodeID;
        [DBField("WorkHourPatternFriDefaultRosterCodeID"), TextSearch, Export(false)]
        public int WorkHourPatternFriDefaultRosterCodeID
        {
            get { return m_WorkHourPatternFriDefaultRosterCodeID; }
            set { m_WorkHourPatternFriDefaultRosterCodeID = value; modify("WorkHourPatternFriDefaultRosterCodeID"); }
        }

        protected double m_WorkHourPatternFriDefaultDayUnit;
        [DBField("WorkHourPatternFriDefaultDayUnit", "0.###"), TextSearch, Export(false), MaxLength(5)]
        public double WorkHourPatternFriDefaultDayUnit
        {
            get { return m_WorkHourPatternFriDefaultDayUnit; }
            set { m_WorkHourPatternFriDefaultDayUnit = value; modify("WorkHourPatternFriDefaultDayUnit"); }
        }

        protected int m_WorkHourPatternSatDefaultRosterCodeID;
        [DBField("WorkHourPatternSatDefaultRosterCodeID"), TextSearch, Export(false)]
        public int WorkHourPatternSatDefaultRosterCodeID
        {
            get { return m_WorkHourPatternSatDefaultRosterCodeID; }
            set { m_WorkHourPatternSatDefaultRosterCodeID = value; modify("WorkHourPatternSatDefaultRosterCodeID"); }
        }

        protected double m_WorkHourPatternSatDefaultDayUnit;
        [DBField("WorkHourPatternSatDefaultDayUnit", "0.###"), TextSearch, Export(false), MaxLength(5)]
        public double WorkHourPatternSatDefaultDayUnit
        {
            get { return m_WorkHourPatternSatDefaultDayUnit; }
            set { m_WorkHourPatternSatDefaultDayUnit = value; modify("WorkHourPatternSatDefaultDayUnit"); }
        }

        protected bool m_WorkHourPatternUseStatutoryHolidayTable;
        [DBField("WorkHourPatternUseStatutoryHolidayTable"), TextSearch, Export(false)]
        public bool WorkHourPatternUseStatutoryHolidayTable
        {
            get { return m_WorkHourPatternUseStatutoryHolidayTable; }
            set { m_WorkHourPatternUseStatutoryHolidayTable = value; modify("WorkHourPatternUseStatutoryHolidayTable"); }
        }

        protected int m_WorkHourPatternStatutoryHolidayDefaultRosterCodeID;
        [DBField("WorkHourPatternStatutoryHolidayDefaultRosterCodeID"), TextSearch, Export(false)]
        public int WorkHourPatternStatutoryHolidayDefaultRosterCodeID
        {
            get { return m_WorkHourPatternStatutoryHolidayDefaultRosterCodeID; }
            set { m_WorkHourPatternStatutoryHolidayDefaultRosterCodeID = value; modify("WorkHourPatternStatutoryHolidayDefaultRosterCodeID"); }
        }

        protected bool m_WorkHourPatternUsePublicHolidayTable;
        [DBField("WorkHourPatternUsePublicHolidayTable"), TextSearch, Export(false)]
        public bool WorkHourPatternUsePublicHolidayTable
        {
            get { return m_WorkHourPatternUsePublicHolidayTable; }
            set { m_WorkHourPatternUsePublicHolidayTable = value; modify("WorkHourPatternUsePublicHolidayTable"); }
        }

        protected int m_WorkHourPatternPublicHolidayDefaultRosterCodeID;
        [DBField("WorkHourPatternPublicHolidayDefaultRosterCodeID"), TextSearch, Export(false)]
        public int WorkHourPatternPublicHolidayDefaultRosterCodeID
        {
            get { return m_WorkHourPatternPublicHolidayDefaultRosterCodeID; }
            set { m_WorkHourPatternPublicHolidayDefaultRosterCodeID = value; modify("WorkHourPatternPublicHolidayDefaultRosterCodeID"); }
        }

        protected double m_WorkHourPatternContractWorkHoursPerDay;
        [DBField("WorkHourPatternContractWorkHoursPerDay", "0.00"), MaxLength(5), Required, TextSearch, Export(false)]
        public double WorkHourPatternContractWorkHoursPerDay
        {
            get { return m_WorkHourPatternContractWorkHoursPerDay; }
            set { m_WorkHourPatternContractWorkHoursPerDay = value; modify("WorkHourPatternContractWorkHoursPerDay"); }
        }

        protected double m_WorkHourPatternContractLunchTimeHoursPerDay;
        [DBField("WorkHourPatternContractLunchTimeHoursPerDay", "0.00"), MaxLength(5), Required, TextSearch, Export(false)]
        public double WorkHourPatternContractLunchTimeHoursPerDay
        {
            get { return m_WorkHourPatternContractLunchTimeHoursPerDay; }
            set { m_WorkHourPatternContractLunchTimeHoursPerDay = value; modify("WorkHourPatternContractLunchTimeHoursPerDay"); }
        }

        protected double m_WorkHourPatternSunWorkHoursPerDay;
        [DBField("WorkHourPatternSunWorkHoursPerDay", "0.00"), MaxLength(5), TextSearch, Export(false)]
        public double WorkHourPatternSunWorkHoursPerDay
        {
            get { return m_WorkHourPatternSunWorkHoursPerDay; }
            set { m_WorkHourPatternSunWorkHoursPerDay = value; modify("WorkHourPatternSunWorkHoursPerDay"); }
        }

        protected double m_WorkHourPatternSunLunchTimeHoursPerDay;
        [DBField("WorkHourPatternSunLunchTimeHoursPerDay", "0.00"), MaxLength(5), TextSearch, Export(false)]
        public double WorkHourPatternSunLunchTimeHoursPerDay
        {
            get { return m_WorkHourPatternSunLunchTimeHoursPerDay; }
            set { m_WorkHourPatternSunLunchTimeHoursPerDay = value; modify("WorkHourPatternSunLunchTimeHoursPerDay"); }
        }

        protected double m_WorkHourPatternMonWorkHoursPerDay;
        [DBField("WorkHourPatternMonWorkHoursPerDay", "0.00"), MaxLength(5), TextSearch, Export(false)]
        public double WorkHourPatternMonWorkHoursPerDay
        {
            get { return m_WorkHourPatternMonWorkHoursPerDay; }
            set { m_WorkHourPatternMonWorkHoursPerDay = value; modify("WorkHourPatternMonWorkHoursPerDay"); }
        }

        protected double m_WorkHourPatternMonLunchTimeHoursPerDay;
        [DBField("WorkHourPatternMonLunchTimeHoursPerDay", "0.00"), MaxLength(5), TextSearch, Export(false)]
        public double WorkHourPatternMonLunchTimeHoursPerDay
        {
            get { return m_WorkHourPatternMonLunchTimeHoursPerDay; }
            set { m_WorkHourPatternMonLunchTimeHoursPerDay = value; modify("WorkHourPatternMonLunchTimeHoursPerDay"); }
        }

        protected double m_WorkHourPatternTueWorkHoursPerDay;
        [DBField("WorkHourPatternTueWorkHoursPerDay", "0.00"), MaxLength(5), TextSearch, Export(false)]
        public double WorkHourPatternTueWorkHoursPerDay
        {
            get { return m_WorkHourPatternTueWorkHoursPerDay; }
            set { m_WorkHourPatternTueWorkHoursPerDay = value; modify("WorkHourPatternTueWorkHoursPerDay"); }
        }

        protected double m_WorkHourPatternTueLunchTimeHoursPerDay;
        [DBField("WorkHourPatternTueLunchTimeHoursPerDay", "0.00"), MaxLength(5), TextSearch, Export(false)]
        public double WorkHourPatternTueLunchTimeHoursPerDay
        {
            get { return m_WorkHourPatternTueLunchTimeHoursPerDay; }
            set { m_WorkHourPatternTueLunchTimeHoursPerDay = value; modify("WorkHourPatternTueLunchTimeHoursPerDay"); }
        }

        protected double m_WorkHourPatternWedWorkHoursPerDay;
        [DBField("WorkHourPatternWedWorkHoursPerDay", "0.00"), MaxLength(5), TextSearch, Export(false)]
        public double WorkHourPatternWedWorkHoursPerDay
        {
            get { return m_WorkHourPatternWedWorkHoursPerDay; }
            set { m_WorkHourPatternWedWorkHoursPerDay = value; modify("WorkHourPatternWedWorkHoursPerDay"); }
        }

        protected double m_WorkHourPatternWedLunchTimeHoursPerDay;
        [DBField("WorkHourPatternWedLunchTimeHoursPerDay", "0.00"), MaxLength(5), TextSearch, Export(false)]
        public double WorkHourPatternWedLunchTimeHoursPerDay
        {
            get { return m_WorkHourPatternWedLunchTimeHoursPerDay; }
            set { m_WorkHourPatternWedLunchTimeHoursPerDay = value; modify("WorkHourPatternWedLunchTimeHoursPerDay"); }
        }

        protected double m_WorkHourPatternThuWorkHoursPerDay;
        [DBField("WorkHourPatternThuWorkHoursPerDay", "0.00"), MaxLength(5), TextSearch, Export(false)]
        public double WorkHourPatternThuWorkHoursPerDay
        {
            get { return m_WorkHourPatternThuWorkHoursPerDay; }
            set { m_WorkHourPatternThuWorkHoursPerDay = value; modify("WorkHourPatternThuWorkHoursPerDay"); }
        }

        protected double m_WorkHourPatternThuLunchTimeHoursPerDay;
        [DBField("WorkHourPatternThuLunchTimeHoursPerDay", "0.00"), MaxLength(5), TextSearch, Export(false)]
        public double WorkHourPatternThuLunchTimeHoursPerDay
        {
            get { return m_WorkHourPatternThuLunchTimeHoursPerDay; }
            set { m_WorkHourPatternThuLunchTimeHoursPerDay = value; modify("WorkHourPatternThuLunchTimeHoursPerDay"); }
        }

        protected double m_WorkHourPatternFriWorkHoursPerDay;
        [DBField("WorkHourPatternFriWorkHoursPerDay", "0.00"), MaxLength(5), TextSearch, Export(false)]
        public double WorkHourPatternFriWorkHoursPerDay
        {
            get { return m_WorkHourPatternFriWorkHoursPerDay; }
            set { m_WorkHourPatternFriWorkHoursPerDay = value; modify("WorkHourPatternFriWorkHoursPerDay"); }
        }

        protected double m_WorkHourPatternFriLunchTimeHoursPerDay;
        [DBField("WorkHourPatternFriLunchTimeHoursPerDay", "0.00"), MaxLength(5), TextSearch, Export(false)]
        public double WorkHourPatternFriLunchTimeHoursPerDay
        {
            get { return m_WorkHourPatternFriLunchTimeHoursPerDay; }
            set { m_WorkHourPatternFriLunchTimeHoursPerDay = value; modify("WorkHourPatternFriLunchTimeHoursPerDay"); }
        }

        protected double m_WorkHourPatternSatWorkHoursPerDay;
        [DBField("WorkHourPatternSatWorkHoursPerDay", "0.00"), MaxLength(5), TextSearch, Export(false)]
        public double WorkHourPatternSatWorkHoursPerDay
        {
            get { return m_WorkHourPatternSatWorkHoursPerDay; }
            set { m_WorkHourPatternSatWorkHoursPerDay = value; modify("WorkHourPatternSatWorkHoursPerDay"); }
        }

        protected double m_WorkHourPatternSatLunchTimeHoursPerDay;
        [DBField("WorkHourPatternSatLunchTimeHoursPerDay", "0.00"), MaxLength(5), TextSearch, Export(false)]
        public double WorkHourPatternSatLunchTimeHoursPerDay
        {
            get { return m_WorkHourPatternSatLunchTimeHoursPerDay; }
            set { m_WorkHourPatternSatLunchTimeHoursPerDay = value; modify("WorkHourPatternSatLunchTimeHoursPerDay"); }
        }

        public int GetDefaultRosterCodeID(DatabaseConnection dbConn, DateTime date)
        {
            return GetDefaultRosterCodeID(dbConn, date, false, false);
        }

        public int GetDefaultRosterCodeID(DatabaseConnection dbConn, DateTime date, bool SkipStatutoryHolidayChecking, bool SkipPublicHolidayChecking)
        {
            if (!WorkHourPatternWorkDayDetermineMethod.Equals(WORKDAYDETERMINDMETHOD_ROSTERTABLE))
                return 0;
            else
            {
                if (this.m_WorkHourPatternUseStatutoryHolidayTable)
                {
                    if (EStatutoryHoliday.IsHoliday(dbConn, date))
                        if (!SkipStatutoryHolidayChecking)
                            return this.m_WorkHourPatternStatutoryHolidayDefaultRosterCodeID;
                }
                if (this.m_WorkHourPatternUsePublicHolidayTable)
                {
                    if (EPublicHoliday.IsHoliday(dbConn, date))
                        if (!SkipPublicHolidayChecking)
                            return this.m_WorkHourPatternPublicHolidayDefaultRosterCodeID;
                }
                
                if (date.DayOfWeek.Equals(DayOfWeek.Sunday))
                    return m_WorkHourPatternSunDefaultRosterCodeID;
                else if (date.DayOfWeek.Equals(DayOfWeek.Monday))
                    return m_WorkHourPatternMonDefaultRosterCodeID;
                else if (date.DayOfWeek.Equals(DayOfWeek.Tuesday))
                    return m_WorkHourPatternTueDefaultRosterCodeID;
                else if (date.DayOfWeek.Equals(DayOfWeek.Wednesday))
                    return m_WorkHourPatternWedDefaultRosterCodeID;
                else if (date.DayOfWeek.Equals(DayOfWeek.Thursday))
                    return m_WorkHourPatternThuDefaultRosterCodeID;
                else if (date.DayOfWeek.Equals(DayOfWeek.Friday))
                    return m_WorkHourPatternFriDefaultRosterCodeID;
                else if (date.DayOfWeek.Equals(DayOfWeek.Saturday))
                    return m_WorkHourPatternSatDefaultRosterCodeID;
                else
                    return 0;
            }
        }

        public double GetDefaultDayUnit(DatabaseConnection dbConn, DateTime date, bool SkipStatutoryHolidayChecking, bool SkipPublicHolidayChecking)
        {
            if (WorkHourPatternWorkDayDetermineMethod.Equals(WORKDAYDETERMINDMETHOD_ROSTERTABLE))
            {
                ERosterCode rosterCode = new ERosterCode();
                rosterCode.RosterCodeID = GetDefaultRosterCodeID(dbConn, date, SkipStatutoryHolidayChecking, SkipPublicHolidayChecking);
                if (ERosterCode.db.select(dbConn, rosterCode))
                    if (rosterCode.RosterCodeType.Equals(ERosterCode.ROSTERTYPE_CODE_NORMAL) || rosterCode.RosterCodeType.Equals(ERosterCode.ROSTERTYPE_CODE_OVERNIGHT))
                        return rosterCode.RosterCodeWorkingDayUnit;
                return 0;
            }
            else
            {
                if (this.m_WorkHourPatternUseStatutoryHolidayTable)
                {
                    if (EStatutoryHoliday.IsHoliday(dbConn, date))
                        if (!SkipStatutoryHolidayChecking)
                            return 0;
                }
                if (this.m_WorkHourPatternUsePublicHolidayTable)
                {
                    if (EPublicHoliday.IsHoliday(dbConn, date))
                        if (!SkipPublicHolidayChecking)
                            return 0;
                }

                if (date.DayOfWeek.Equals(DayOfWeek.Sunday))
                    return m_WorkHourPatternSunDefaultDayUnit;
                else if (date.DayOfWeek.Equals(DayOfWeek.Monday))
                    return m_WorkHourPatternMonDefaultDayUnit;
                else if (date.DayOfWeek.Equals(DayOfWeek.Tuesday))
                    return m_WorkHourPatternTueDefaultDayUnit;
                else if (date.DayOfWeek.Equals(DayOfWeek.Wednesday))
                    return m_WorkHourPatternWedDefaultDayUnit;
                else if (date.DayOfWeek.Equals(DayOfWeek.Thursday))
                    return m_WorkHourPatternThuDefaultDayUnit;
                else if (date.DayOfWeek.Equals(DayOfWeek.Friday))
                    return m_WorkHourPatternFriDefaultDayUnit;
                else if (date.DayOfWeek.Equals(DayOfWeek.Saturday))
                    return m_WorkHourPatternSatDefaultDayUnit;
                else
                    return 0;
            }
        }

        public double GetDefaultWorkHour(DatabaseConnection dbConn, DateTime date)
        {
            if (WorkHourPatternWorkDayDetermineMethod.Equals(WORKDAYDETERMINDMETHOD_ROSTERTABLE))
            {
                ERosterCode rosterCode = new ERosterCode();
                rosterCode.RosterCodeID = GetDefaultRosterCodeID(dbConn, date, false, false);
                if (ERosterCode.db.select(dbConn, rosterCode))
                    if (rosterCode.RosterCodeType.Equals(ERosterCode.ROSTERTYPE_CODE_NORMAL) || rosterCode.RosterCodeType.Equals(ERosterCode.ROSTERTYPE_CODE_OVERNIGHT))
                        return rosterCode.RosterCodeDailyWorkingHour;
                return 0;
            }
            else
            {
                if (this.m_WorkHourPatternUseStatutoryHolidayTable)
                {
                    if (EStatutoryHoliday.IsHoliday(dbConn, date))
                            return 0;
                }
                if (this.m_WorkHourPatternUsePublicHolidayTable)
                {
                    if (EPublicHoliday.IsHoliday(dbConn, date))
                            return 0;
                }

                if (date.DayOfWeek.Equals(DayOfWeek.Sunday))
                    //if (m_WorkHourPatternSunWorkHoursPerDay > 0)
                        return m_WorkHourPatternSunWorkHoursPerDay;
                    //else
                    //    return m_WorkHourPatternSunDefaultDayUnit * m_WorkHourPatternContractWorkHoursPerDay;
                else if (date.DayOfWeek.Equals(DayOfWeek.Monday))
                    //if (m_WorkHourPatternMonWorkHoursPerDay > 0)
                        return m_WorkHourPatternMonWorkHoursPerDay;
                    //else
                    //    return m_WorkHourPatternMonDefaultDayUnit * m_WorkHourPatternContractWorkHoursPerDay;
                else if (date.DayOfWeek.Equals(DayOfWeek.Tuesday))
                    //if (m_WorkHourPatternTueWorkHoursPerDay > 0)
                        return m_WorkHourPatternTueWorkHoursPerDay;
                    //else
                    //    return m_WorkHourPatternTueDefaultDayUnit * m_WorkHourPatternContractWorkHoursPerDay;
                else if (date.DayOfWeek.Equals(DayOfWeek.Wednesday))
                    //if (m_WorkHourPatternWedWorkHoursPerDay > 0)
                        return m_WorkHourPatternWedWorkHoursPerDay;
                    //else
                    //    return m_WorkHourPatternWedDefaultDayUnit * m_WorkHourPatternContractWorkHoursPerDay;
                else if (date.DayOfWeek.Equals(DayOfWeek.Thursday))
                    //if (m_WorkHourPatternThuWorkHoursPerDay > 0)
                        return m_WorkHourPatternThuWorkHoursPerDay;
                    //else
                    //    return m_WorkHourPatternThuDefaultDayUnit * m_WorkHourPatternContractWorkHoursPerDay;
                else if (date.DayOfWeek.Equals(DayOfWeek.Friday))
                    //if (m_WorkHourPatternFriWorkHoursPerDay > 0)
                        return m_WorkHourPatternFriWorkHoursPerDay;
                    //else
                    //    return m_WorkHourPatternFriDefaultDayUnit * m_WorkHourPatternContractWorkHoursPerDay;
                else if (date.DayOfWeek.Equals(DayOfWeek.Saturday))
                    //if (m_WorkHourPatternSatWorkHoursPerDay > 0)
                        return m_WorkHourPatternSatWorkHoursPerDay;
                    //else
                    //    return m_WorkHourPatternSatDefaultDayUnit * m_WorkHourPatternContractWorkHoursPerDay;
                else
                    return 0;
            }
        }

        public double GetDefaultLunch(DatabaseConnection dbConn, DateTime date)
        {
            if (WorkHourPatternWorkDayDetermineMethod.Equals(WORKDAYDETERMINDMETHOD_ROSTERTABLE))
            {
                ERosterCode rosterCode = new ERosterCode();
                rosterCode.RosterCodeID = GetDefaultRosterCodeID(dbConn, date, false, false);
                if (ERosterCode.db.select(dbConn, rosterCode))
                    if (rosterCode.RosterCodeType.Equals(ERosterCode.ROSTERTYPE_CODE_NORMAL) || rosterCode.RosterCodeType.Equals(ERosterCode.ROSTERTYPE_CODE_OVERNIGHT))
                        if (rosterCode.RosterCodeHasLunch)
                        {
                            double LunchTime = rosterCode.RosterCodeLunchEndTime.TimeOfDay.Subtract(rosterCode.RosterCodeLunchStartTime.TimeOfDay).TotalHours;
                            if (LunchTime < 0)
                                LunchTime += 24;
                            return LunchTime;
                        }
                return 0;
            }
            else
            {
                if (this.m_WorkHourPatternUseStatutoryHolidayTable)
                {
                    if (EStatutoryHoliday.IsHoliday(dbConn, date))
                        return 0;
                }
                if (this.m_WorkHourPatternUsePublicHolidayTable)
                {
                    if (EPublicHoliday.IsHoliday(dbConn, date))
                        return 0;
                }

                if (date.DayOfWeek.Equals(DayOfWeek.Sunday))
                    return m_WorkHourPatternSunLunchTimeHoursPerDay;
                else if (date.DayOfWeek.Equals(DayOfWeek.Monday))
                    return m_WorkHourPatternMonLunchTimeHoursPerDay;
                else if (date.DayOfWeek.Equals(DayOfWeek.Tuesday))
                    return m_WorkHourPatternTueLunchTimeHoursPerDay;
                else if (date.DayOfWeek.Equals(DayOfWeek.Wednesday))
                    return m_WorkHourPatternWedLunchTimeHoursPerDay;
                else if (date.DayOfWeek.Equals(DayOfWeek.Thursday))
                    return m_WorkHourPatternThuLunchTimeHoursPerDay;
                else if (date.DayOfWeek.Equals(DayOfWeek.Friday))
                    return m_WorkHourPatternFriLunchTimeHoursPerDay;
                else if (date.DayOfWeek.Equals(DayOfWeek.Saturday))
                    return m_WorkHourPatternSatLunchTimeHoursPerDay;
                else
                    return 0;
            }
        }
    }
}
