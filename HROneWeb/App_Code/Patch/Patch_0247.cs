//using System;
//using System.Collections;
//using HROne.DataAccess;
////using perspectivemind.validation;

//namespace HROne.Patch
//{
//    public class Patch_0247
//    {

//        [DBClass("RosterCode")]
//        private class EOLDRosterCode : DBObject
//        {
//            public static DBManager db = new DBManager(typeof(EOLDRosterCode));

//            protected int m_RosterCodeID;
//            [DBField("RosterCodeID", true, true), TextSearch, Export(false)]
//            public int RosterCodeID
//            {
//                get { return m_RosterCodeID; }
//                set { m_RosterCodeID = value; modify("RosterCodeID"); }
//            }

//            protected bool m_RosterCodeHasLunch;
//            [DBField("RosterCodeHasLunch"), TextSearch, Export(false)]
//            public bool RosterCodeHasLunch
//            {
//                get { return m_RosterCodeHasLunch; }
//                set { m_RosterCodeHasLunch = value; modify("RosterCodeHasLunch"); }
//            }
//            protected DateTime m_RosterCodeLunchStartTime;
//            [DBField("RosterCodeLunchStartTime", "HH:mm"), TextSearch, MaxLength(5), Export(false)]
//            public DateTime RosterCodeLunchStartTime
//            {
//                get { return m_RosterCodeLunchStartTime; }
//                set { m_RosterCodeLunchStartTime = value; modify("RosterCodeLunchStartTime"); }
//            }
//            protected DateTime m_RosterCodeLunchEndTime;
//            [DBField("RosterCodeLunchEndTime", "HH:mm"), TextSearch, MaxLength(5), Export(false)]
//            public DateTime RosterCodeLunchEndTime
//            {
//                get { return m_RosterCodeLunchEndTime; }
//                set { m_RosterCodeLunchEndTime = value; modify("RosterCodeLunchEndTime"); }
//            }

//            //protected bool m_RosterCodeLunchIsDeductWorkingHour;
//            //[DBField("RosterCodeLunchIsDeductWorkingHour"), TextSearch, Export(false)]
//            //public bool RosterCodeLunchIsDeductWorkingHour
//            //{
//            //    get { return m_RosterCodeLunchIsDeductWorkingHour; }
//            //    set { m_RosterCodeLunchIsDeductWorkingHour = value; modify("RosterCodeLunchIsDeductWorkingHour"); }
//            //}
//            //protected int m_RosterCodeLunchDeductWorkingHourMinsUnit;
//            //[DBField("RosterCodeLunchDeductWorkingHourMinsUnit"), TextSearch, MaxLength(5), Export(false), Required]
//            //public int RosterCodeLunchDeductWorkingHourMinsUnit
//            //{
//            //    get { return m_RosterCodeLunchDeductWorkingHourMinsUnit; }
//            //    set { m_RosterCodeLunchDeductWorkingHourMinsUnit = value; modify("RosterCodeLunchDeductWorkingHourMinsUnit"); }
//            //}

//            //protected string m_RosterCodeLunchDeductWorkingHourMinsRoundingRule;
//            //[DBField("RosterCodeLunchDeductWorkingHourMinsRoundingRule"), TextSearch, MaxLength(50), Export(false), Required]
//            //public string RosterCodeLunchDeductWorkingHourMinsRoundingRule
//            //{
//            //    get { return m_RosterCodeLunchDeductWorkingHourMinsRoundingRule; }
//            //    set { m_RosterCodeLunchDeductWorkingHourMinsRoundingRule = value; modify("RosterCodeLunchDeductWorkingHourMinsRoundingRule"); }
//            //}
//            //protected DateTime m_RosterCodeCutOffTime;
//            //[DBField("RosterCodeCutOffTime", "HH:mm"), TextSearch, MaxLength(5), Export(false)]
//            //public DateTime RosterCodeCutOffTime
//            //{
//            //    get { return m_RosterCodeCutOffTime; }
//            //    set { m_RosterCodeCutOffTime = value; modify("RosterCodeCutOffTime"); }
//            //}
//            protected double m_RosterCodeWorkingDayUnit;
//            [DBField("RosterCodeWorkingDayUnit", "0.00"), TextSearch, MaxLength(13), Export(false), Required]
//            public double RosterCodeWorkingDayUnit
//            {
//                get { return m_RosterCodeWorkingDayUnit; }
//                set { m_RosterCodeWorkingDayUnit = value; modify("RosterCodeWorkingDayUnit"); }
//            }
//            protected double m_RosterCodeDailyWorkingHour;
//            [DBField("RosterCodeDailyWorkingHour", "0.00"), TextSearch, MaxLength(13), Export(false), Required]
//            public double RosterCodeDailyWorkingHour
//            {
//                get { return m_RosterCodeDailyWorkingHour; }
//                set { m_RosterCodeDailyWorkingHour = value; modify("RosterCodeDailyWorkingHour"); }
//            }

//            protected bool m_RosterCodeCountWorkHourOnly;
//            [DBField("RosterCodeCountWorkHourOnly"), TextSearch, Export(false)]
//            public bool RosterCodeCountWorkHourOnly
//            {
//                get { return m_RosterCodeCountWorkHourOnly; }
//                set { m_RosterCodeCountWorkHourOnly = value; modify("RosterCodeCountWorkHourOnly"); }
//            }


//            //protected bool m_RosterCodeUseHalfWorkingDaysHours;
//            //[DBField("RosterCodeUseHalfWorkingDaysHours"), TextSearch, Export(false)]
//            //public bool RosterCodeUseHalfWorkingDaysHours
//            //{
//            //    get { return m_RosterCodeUseHalfWorkingDaysHours; }
//            //    set { m_RosterCodeUseHalfWorkingDaysHours = value; modify("RosterCodeUseHalfWorkingDaysHours"); }
//            //}
//            //protected double m_RosterCodeUseHalfWorkingDaysHoursMaxWorkingHours;
//            //[DBField("RosterCodeUseHalfWorkingDaysHoursMaxWorkingHours", "0.00"), TextSearch, MaxLength(6), Export(false)]
//            //public double RosterCodeUseHalfWorkingDaysHoursMaxWorkingHours
//            //{
//            //    get { return m_RosterCodeUseHalfWorkingDaysHoursMaxWorkingHours; }
//            //    set { m_RosterCodeUseHalfWorkingDaysHoursMaxWorkingHours = value; modify("RosterCodeUseHalfWorkingDaysHoursMaxWorkingHours"); }
//            //}


//            protected double m_RosterCodeLunchDurationHour;
//            [DBField("RosterCodeLunchDurationHour", "0.00"), TextSearch, MaxLength(6), Export(false)]
//            public double RosterCodeLunchDurationHour
//            {
//                get { return m_RosterCodeLunchDurationHour; }
//                set { m_RosterCodeLunchDurationHour = value; modify("RosterCodeLunchDurationHour"); }
//            }

//            protected double m_RosterCodeLunchDeductMinimumWorkHour;
//            [DBField("RosterCodeLunchDeductMinimumWorkHour", "0.00"), TextSearch, Export(false)]
//            public double RosterCodeLunchDeductMinimumWorkHour
//            {
//                get { return m_RosterCodeLunchDeductMinimumWorkHour; }
//                set { m_RosterCodeLunchDeductMinimumWorkHour = value; modify("RosterCodeLunchDeductMinimumWorkHour"); }
//            }


//        }


//        public static bool DBPatch(DatabaseConnection dbConn)
//        {
//            ArrayList rosterCodeList = EOLDRosterCode.db.select(dbConn, new DBFilter());
//            foreach (EOLDRosterCode oldRosterCode in rosterCodeList)
//            {
//                if (oldRosterCode.RosterCodeHasLunch)
//                {
//                    if (!oldRosterCode.RosterCodeCountWorkHourOnly)
//                    {
//                        if (oldRosterCode.RosterCodeLunchStartTime.Ticks > 0 && oldRosterCode.RosterCodeLunchEndTime.Ticks > 0)
//                        {
//                            double lunchDuration = oldRosterCode.RosterCodeLunchEndTime.TimeOfDay.Subtract(oldRosterCode.RosterCodeLunchStartTime.TimeOfDay).TotalHours;
//                            if (lunchDuration < 0)
//                                lunchDuration += 24;
//                            oldRosterCode.RosterCodeLunchDurationHour = lunchDuration;
//                            oldRosterCode.RosterCodeLunchDeductMinimumWorkHour = oldRosterCode.RosterCodeDailyWorkingHour * 0.5 + lunchDuration;
//                        }
//                        else
//                        {
//                            oldRosterCode.RosterCodeLunchDurationHour = 1;
//                            oldRosterCode.RosterCodeLunchDeductMinimumWorkHour = oldRosterCode.RosterCodeDailyWorkingHour * 0.5 + 1;
//                        }
//                        EOLDRosterCode.db.update(dbConn, oldRosterCode);
//                    }
//                }
//            }
//            return true;
//        }

//    }
//}