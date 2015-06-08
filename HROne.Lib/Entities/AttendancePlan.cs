using System;
using System.Collections.Generic;
using System.Text;
using HROne.DataAccess;
////using perspectivemind.validation;

namespace HROne.Lib.Entities
{
    [DBClass("AttendancePlan")]
    public class EAttendancePlan : BaseObject
    {
        public static DBManager db = new DBManager(typeof(EAttendancePlan));
        public static WFValueList VLAttendancePlan = new WFDBCodeList(EAttendancePlan.db, "AttendancePlanID", "AttendancePlanCode", "AttendancePlanDesc", "AttendancePlanCode");

        protected int m_AttendancePlanID;
        [DBField("AttendancePlanID", true, true), TextSearch, Export(false)]
        public int AttendancePlanID
        {
            get { return m_AttendancePlanID; }
            set { m_AttendancePlanID = value; modify("AttendancePlanID"); }
        }
        protected string m_AttendancePlanCode;
        [DBField("AttendancePlanCode"), TextSearch,MaxLength(20,10), Export(false),Required]
        public string AttendancePlanCode
        {
            get { return m_AttendancePlanCode; }
            set { m_AttendancePlanCode = value; modify("AttendancePlanCode"); }
        }
        protected string m_AttendancePlanDesc;
        [DBField("AttendancePlanDesc"), TextSearch, MaxLength(100, 25), Export(false), Required]
        public string AttendancePlanDesc
        {
            get { return m_AttendancePlanDesc; }
            set { m_AttendancePlanDesc = value; modify("AttendancePlanDesc"); }
        }
        protected int m_AttendancePlanAbsentProrataPayFormID;
        [DBField("AttendancePlanAbsentProrataPayFormID"), TextSearch, Export(false)]
        public int AttendancePlanAbsentProrataPayFormID
        {
            get { return m_AttendancePlanAbsentProrataPayFormID; }
            set { m_AttendancePlanAbsentProrataPayFormID = value; modify("AttendancePlanAbsentProrataPayFormID"); }
        }
        protected int m_AttendancePlanOTFormula;
        [DBField("AttendancePlanOTFormula"), TextSearch, Export(false)]
        public int AttendancePlanOTFormula
        {
            get { return m_AttendancePlanOTFormula; }
            set { m_AttendancePlanOTFormula = value; modify("AttendancePlanOTFormula"); }
        }
        protected int m_AttendancePlanLateFormula;
        [DBField("AttendancePlanLateFormula"), TextSearch, Export(false)]
        public int AttendancePlanLateFormula
        {
            get { return m_AttendancePlanLateFormula; }
            set { m_AttendancePlanLateFormula = value; modify("AttendancePlanLateFormula"); }
        }
        protected int m_AttendancePlanOTMinsUnit;
        [DBField("AttendancePlanOTMinsUnit"), TextSearch, MaxLength(2), Export(false), Required]
        public int AttendancePlanOTMinsUnit
        {
            get { return m_AttendancePlanOTMinsUnit; }
            set { m_AttendancePlanOTMinsUnit = value; modify("AttendancePlanOTMinsUnit"); }
        }
        protected int m_AttendancePlanLateMinsUnit;
        [DBField("AttendancePlanLateMinsUnit"), TextSearch, MaxLength(2), Export(false), Required]
        public int AttendancePlanLateMinsUnit
        {
            get { return m_AttendancePlanLateMinsUnit; }
            set { m_AttendancePlanLateMinsUnit = value; modify("AttendancePlanLateMinsUnit"); }
        }

        protected int m_AttendancePlanOTPayCodeID;
        [DBField("AttendancePlanOTPayCodeID"), TextSearch, Export(false)]
        public int AttendancePlanOTPayCodeID
        {
            get { return m_AttendancePlanOTPayCodeID; }
            set { m_AttendancePlanOTPayCodeID = value; modify("AttendancePlanOTPayCodeID"); }
        }

        protected double m_AttendancePlanOTRateMultiplier;
        [DBField("AttendancePlanOTRateMultiplier","0.####"), TextSearch, MaxLength(6), Export(false)]
        public double AttendancePlanOTRateMultiplier
        {
            get { return m_AttendancePlanOTRateMultiplier; }
            set { m_AttendancePlanOTRateMultiplier = value; modify("AttendancePlanOTRateMultiplier"); }
        }

        protected int m_AttendancePlanLatePayCodeID;
        [DBField("AttendancePlanLatePayCodeID"), TextSearch, Export(false)]
        public int AttendancePlanLatePayCodeID
        {
            get { return m_AttendancePlanLatePayCodeID; }
            set { m_AttendancePlanLatePayCodeID = value; modify("AttendancePlanLatePayCodeID"); }
        }

        protected string m_AttendancePlanOTMinsRoundingRule;
        [DBField("AttendancePlanOTMinsRoundingRule"), TextSearch, MaxLength(50), Export(false), Required]
        public string AttendancePlanOTMinsRoundingRule
        {
            get { return m_AttendancePlanOTMinsRoundingRule; }
            set { m_AttendancePlanOTMinsRoundingRule = value; modify("AttendancePlanOTMinsRoundingRule"); }
        }
        protected string m_AttendancePlanLateMinsRoundingRule;
        [DBField("AttendancePlanLateMinsRoundingRule"), TextSearch, MaxLength(50), Export(false), Required]
        public string AttendancePlanLateMinsRoundingRule
        {
            get { return m_AttendancePlanLateMinsRoundingRule; }
            set { m_AttendancePlanLateMinsRoundingRule = value; modify("AttendancePlanLateMinsRoundingRule"); }
        }

        protected bool m_AttendancePlanOTGainAsCompensationLeaveEntitle;
        [DBField("AttendancePlanOTGainAsCompensationLeaveEntitle"), TextSearch, Export(false)]
        public bool AttendancePlanOTGainAsCompensationLeaveEntitle
        {
            get { return m_AttendancePlanOTGainAsCompensationLeaveEntitle; }
            set { m_AttendancePlanOTGainAsCompensationLeaveEntitle = value; modify("AttendancePlanOTGainAsCompensationLeaveEntitle"); }
        }

        protected bool m_AttendancePlanLateIncludeEarlyLeave;
        [DBField("AttendancePlanLateIncludeEarlyLeave"), TextSearch, Export(false)]
        public bool AttendancePlanLateIncludeEarlyLeave
        {
            get { return m_AttendancePlanLateIncludeEarlyLeave; }
            set { m_AttendancePlanLateIncludeEarlyLeave = value; modify("AttendancePlanLateIncludeEarlyLeave"); }
        }

        protected bool m_AttendancePlanLateIncludeLunchLate;
        [DBField("AttendancePlanLateIncludeLunchLate"), TextSearch, Export(false)]
        public bool AttendancePlanLateIncludeLunchLate
        {
            get { return m_AttendancePlanLateIncludeLunchLate; }
            set { m_AttendancePlanLateIncludeLunchLate = value; modify("AttendancePlanLateIncludeLunchLate"); }
        }
        protected bool m_AttendancePlanLateIncludeLunchEarlyLeave;
        [DBField("AttendancePlanLateIncludeLunchEarlyLeave"), TextSearch, Export(false)]
        public bool AttendancePlanLateIncludeLunchEarlyLeave
        {
            get { return m_AttendancePlanLateIncludeLunchEarlyLeave; }
            set { m_AttendancePlanLateIncludeLunchEarlyLeave = value; modify("AttendancePlanLateIncludeLunchEarlyLeave"); }
        }
        protected int m_AttendancePlanLateMaxTotalToleranceMins;
        [DBField("AttendancePlanLateMaxTotalToleranceMins"), TextSearch, MaxLength(3), Export(false)]
        public int AttendancePlanLateMaxTotalToleranceMins
        {
            get { return m_AttendancePlanLateMaxTotalToleranceMins; }
            set { m_AttendancePlanLateMaxTotalToleranceMins = value; modify("AttendancePlanLateMaxTotalToleranceMins"); }
        }
        
        protected bool m_AttendancePlanOTIncludeLunchOvertime;
        [DBField("AttendancePlanOTIncludeLunchOvertime"), TextSearch, Export(false)]
        public bool AttendancePlanOTIncludeLunchOvertime
        {
            get { return m_AttendancePlanOTIncludeLunchOvertime; }
            set { m_AttendancePlanOTIncludeLunchOvertime = value; modify("AttendancePlanOTIncludeLunchOvertime"); }
        }

        protected bool m_AttendancePlanCompensateLateByOT;
        [DBField("AttendancePlanCompensateLateByOT"), TextSearch, Export(false)]
        public bool AttendancePlanCompensateLateByOT
        {
            get { return m_AttendancePlanCompensateLateByOT; }
            set { m_AttendancePlanCompensateLateByOT = value; modify("AttendancePlanCompensateLateByOT"); }
        }

        protected int m_AttendancePlanBonusMaxTotalLateCount;
        [DBField("AttendancePlanBonusMaxTotalLateCount"), TextSearch, MaxLength(2), Export(false)]
        public int AttendancePlanBonusMaxTotalLateCount
        {
            get { return m_AttendancePlanBonusMaxTotalLateCount; }
            set { m_AttendancePlanBonusMaxTotalLateCount = value; modify("AttendancePlanBonusMaxTotalLateCount"); }
        }
        protected bool m_AttendancePlanBonusMaxTotalLateCountIncludeLunch;
        [DBField("AttendancePlanBonusMaxTotalLateCountIncludeLunch"), TextSearch, MaxLength(2), Export(false)]
        public bool AttendancePlanBonusMaxTotalLateCountIncludeLunch
        {
            get { return m_AttendancePlanBonusMaxTotalLateCountIncludeLunch; }
            set { m_AttendancePlanBonusMaxTotalLateCountIncludeLunch = value; modify("AttendancePlanBonusMaxTotalLateCountIncludeLunch"); }
        }
        protected int m_AttendancePlanBonusMaxTotalLateMins;
        [DBField("AttendancePlanBonusMaxTotalLateMins"), TextSearch, MaxLength(4), Export(false)]
        public int AttendancePlanBonusMaxTotalLateMins
        {
            get { return m_AttendancePlanBonusMaxTotalLateMins; }
            set { m_AttendancePlanBonusMaxTotalLateMins = value; modify("AttendancePlanBonusMaxTotalLateMins"); }
        }
        protected bool m_AttendancePlanBonusMaxTotalLateMinsIncludeLunch;
        [DBField("AttendancePlanBonusMaxTotalLateMinsIncludeLunch"), TextSearch, MaxLength(2), Export(false)]
        public bool AttendancePlanBonusMaxTotalLateMinsIncludeLunch
        {
            get { return m_AttendancePlanBonusMaxTotalLateMinsIncludeLunch; }
            set { m_AttendancePlanBonusMaxTotalLateMinsIncludeLunch = value; modify("AttendancePlanBonusMaxTotalLateMinsIncludeLunch"); }
        }

        protected int m_AttendancePlanBonusMaxTotalEarlyLeaveCount;
        [DBField("AttendancePlanBonusMaxTotalEarlyLeaveCount"), TextSearch, MaxLength(2), Export(false)]
        public int AttendancePlanBonusMaxTotalEarlyLeaveCount
        {
            get { return m_AttendancePlanBonusMaxTotalEarlyLeaveCount; }
            set { m_AttendancePlanBonusMaxTotalEarlyLeaveCount = value; modify("AttendancePlanBonusMaxTotalEarlyLeaveCount"); }
        }
        protected bool m_AttendancePlanBonusMaxTotalEarlyLeaveCountIncludeLunch;
        [DBField("AttendancePlanBonusMaxTotalEarlyLeaveCountIncludeLunch"), TextSearch, MaxLength(2), Export(false)]
        public bool AttendancePlanBonusMaxTotalEarlyLeaveCountIncludeLunch
        {
            get { return m_AttendancePlanBonusMaxTotalEarlyLeaveCountIncludeLunch; }
            set { m_AttendancePlanBonusMaxTotalEarlyLeaveCountIncludeLunch = value; modify("AttendancePlanBonusMaxTotalEarlyLeaveCountIncludeLunch"); }
        }

        protected int m_AttendancePlanBonusMaxTotalEarlyLeaveMins;
        [DBField("AttendancePlanBonusMaxTotalEarlyLeaveMins"), TextSearch, MaxLength(4), Export(false)]
        public int AttendancePlanBonusMaxTotalEarlyLeaveMins
        {
            get { return m_AttendancePlanBonusMaxTotalEarlyLeaveMins; }
            set { m_AttendancePlanBonusMaxTotalEarlyLeaveMins = value; modify("AttendancePlanBonusMaxTotalEarlyLeaveMins"); }
        }
        protected bool m_AttendancePlanBonusMaxTotalEarlyLeaveMinsIncludeLunch;
        [DBField("AttendancePlanBonusMaxTotalEarlyLeaveMinsIncludeLunch"), TextSearch, MaxLength(2), Export(false)]
        public bool AttendancePlanBonusMaxTotalEarlyLeaveMinsIncludeLunch
        {
            get { return m_AttendancePlanBonusMaxTotalEarlyLeaveMinsIncludeLunch; }
            set { m_AttendancePlanBonusMaxTotalEarlyLeaveMinsIncludeLunch = value; modify("AttendancePlanBonusMaxTotalEarlyLeaveMinsIncludeLunch"); }
        }

        protected int m_AttendancePlanBonusMaxTotalSLWithMedicalCertificate;
        [DBField("AttendancePlanBonusMaxTotalSLWithMedicalCertificate"), TextSearch, MaxLength(2), Export(false)]
        public int AttendancePlanBonusMaxTotalSLWithMedicalCertificate
        {
            get { return m_AttendancePlanBonusMaxTotalSLWithMedicalCertificate; }
            set { m_AttendancePlanBonusMaxTotalSLWithMedicalCertificate = value; modify("AttendancePlanBonusMaxTotalSLWithMedicalCertificate"); }
        }

        protected int m_AttendancePlanBonusMaxTotalSLWithoutMedicalCertificate;
        [DBField("AttendancePlanBonusMaxTotalSLWithoutMedicalCertificate"), TextSearch, MaxLength(2), Export(false)]
        public int AttendancePlanBonusMaxTotalSLWithoutMedicalCertificate
        {
            get { return m_AttendancePlanBonusMaxTotalSLWithoutMedicalCertificate; }
            set { m_AttendancePlanBonusMaxTotalSLWithoutMedicalCertificate = value; modify("AttendancePlanBonusMaxTotalSLWithoutMedicalCertificate"); }
        }

        protected int m_AttendancePlanBonusMaxTotalAbsentCount;
        [DBField("AttendancePlanBonusMaxTotalAbsentCount"), TextSearch, MaxLength(2),Export(false)]
        public int AttendancePlanBonusMaxTotalAbsentCount
        {
            get { return m_AttendancePlanBonusMaxTotalAbsentCount; }
            set { m_AttendancePlanBonusMaxTotalAbsentCount = value; modify("AttendancePlanBonusMaxTotalAbsentCount"); }
        }

        protected int m_AttendancePlanBonusMaxTotalNonFullPayCasualLeave;
        [DBField("AttendancePlanBonusMaxTotalNonFullPayCasualLeave"), TextSearch, MaxLength(2), Export(false)]
        public int AttendancePlanBonusMaxTotalNonFullPayCasualLeave
        {
            get { return m_AttendancePlanBonusMaxTotalNonFullPayCasualLeave; }
            set { m_AttendancePlanBonusMaxTotalNonFullPayCasualLeave = value; modify("AttendancePlanBonusMaxTotalNonFullPayCasualLeave"); }
        }

        protected int m_AttendancePlanBonusMaxTotalInjuryLeave;
        [DBField("AttendancePlanBonusMaxTotalInjuryLeave"), TextSearch, MaxLength(2), Export(false)]
        public int AttendancePlanBonusMaxTotalInjuryLeave
        {
            get { return m_AttendancePlanBonusMaxTotalInjuryLeave; }
            set { m_AttendancePlanBonusMaxTotalInjuryLeave = value; modify("AttendancePlanBonusMaxTotalInjuryLeave"); }
        }

        protected double m_AttendancePlanBonusAmount;
        [DBField("AttendancePlanBonusAmount", "0.00"), TextSearch, Export(false)]
        public double AttendancePlanBonusAmount
        {
            get { return m_AttendancePlanBonusAmount; }
            set { m_AttendancePlanBonusAmount = value; modify("AttendancePlanBonusAmount"); }
        }

        protected string m_AttendancePlanBonusAmountUnit;
        [DBField("AttendancePlanBonusAmountUnit", "0.00"), TextSearch, Export(false)]
        public string AttendancePlanBonusAmountUnit
        {
            get { return m_AttendancePlanBonusAmountUnit; }
            set { m_AttendancePlanBonusAmountUnit = value; modify("AttendancePlanBonusAmountUnit"); }
        }

        protected double m_AttendancePlanBonusOTAmount;
        [DBField("AttendancePlanBonusOTAmount", "0.00"), TextSearch, Export(false)]
        public double AttendancePlanBonusOTAmount
        {
            get { return m_AttendancePlanBonusOTAmount; }
            set { m_AttendancePlanBonusOTAmount = value; modify("AttendancePlanBonusOTAmount"); }
        }

        protected int m_AttendancePlanBonusPayCodeID;
        [DBField("AttendancePlanBonusPayCodeID"), TextSearch, Export(false)]
        public int AttendancePlanBonusPayCodeID
        {
            get { return m_AttendancePlanBonusPayCodeID; }
            set { m_AttendancePlanBonusPayCodeID = value; modify("AttendancePlanBonusPayCodeID"); }
        }
        protected bool m_AttendancePlanUseBonusAmountByRecurringPayment;
        [DBField("AttendancePlanUseBonusAmountByRecurringPayment"), TextSearch, Export(false)]
        public bool AttendancePlanUseBonusAmountByRecurringPayment
        {
            get { return m_AttendancePlanUseBonusAmountByRecurringPayment; }
            set { m_AttendancePlanUseBonusAmountByRecurringPayment = value; modify("AttendancePlanUseBonusAmountByRecurringPayment"); }
        }
        protected bool m_AttendancePlanProrataBonusforNewJoin;
        [DBField("AttendancePlanProrataBonusforNewJoin"), TextSearch, Export(false)]
        public bool AttendancePlanProrataBonusforNewJoin
        {
            get { return m_AttendancePlanProrataBonusforNewJoin; }
            set { m_AttendancePlanProrataBonusforNewJoin = value; modify("AttendancePlanProrataBonusforNewJoin"); }
        }
        protected bool m_AttendancePlanProrataBonusforTerminated;
        [DBField("AttendancePlanProrataBonusforTerminated"), TextSearch, Export(false)]
        public bool AttendancePlanProrataBonusforTerminated
        {
            get { return m_AttendancePlanProrataBonusforTerminated; }
            set { m_AttendancePlanProrataBonusforTerminated = value; modify("AttendancePlanProrataBonusforTerminated"); }
        }
        protected bool m_AttendancePlanTerminatedHasBonus;
        [DBField("AttendancePlanTerminatedHasBonus"), TextSearch, Export(false)]
        public bool AttendancePlanTerminatedHasBonus
        {
            get { return m_AttendancePlanTerminatedHasBonus; }
            set { m_AttendancePlanTerminatedHasBonus = value; modify("AttendancePlanTerminatedHasBonus"); }
        }

        protected double m_AttendancePlanBonusPartialPaidPercent;
        [DBField("AttendancePlanBonusPartialPaidPercent", "0.00"), TextSearch, MaxLength(5), Export(false)]
        public double AttendancePlanBonusPartialPaidPercent
        {
            get { return m_AttendancePlanBonusPartialPaidPercent; }
            set { m_AttendancePlanBonusPartialPaidPercent = value; modify("AttendancePlanBonusPartialPaidPercent"); }
        }

        protected int m_AttendancePlanBonusPartialPaidMaxTotalLateCount;
        [DBField("AttendancePlanBonusPartialPaidMaxTotalLateCount"), TextSearch, MaxLength(2), Export(false)]
        public int AttendancePlanBonusPartialPaidMaxTotalLateCount
        {
            get { return m_AttendancePlanBonusPartialPaidMaxTotalLateCount; }
            set { m_AttendancePlanBonusPartialPaidMaxTotalLateCount = value; modify("AttendancePlanBonusPartialPaidMaxTotalLateCount"); }
        }
        protected bool m_AttendancePlanBonusPartialPaidMaxTotalLateCountIncludeLunch;
        [DBField("AttendancePlanBonusPartialPaidMaxTotalLateCountIncludeLunch"), TextSearch, MaxLength(2), Export(false)]
        public bool AttendancePlanBonusPartialPaidMaxTotalLateCountIncludeLunch
        {
            get { return m_AttendancePlanBonusPartialPaidMaxTotalLateCountIncludeLunch; }
            set { m_AttendancePlanBonusPartialPaidMaxTotalLateCountIncludeLunch = value; modify("AttendancePlanBonusPartialPaidMaxTotalLateCountIncludeLunch"); }
        }
        protected int m_AttendancePlanBonusPartialPaidMaxTotalLateMins;
        [DBField("AttendancePlanBonusPartialPaidMaxTotalLateMins"), TextSearch, MaxLength(4), Export(false)]
        public int AttendancePlanBonusPartialPaidMaxTotalLateMins
        {
            get { return m_AttendancePlanBonusPartialPaidMaxTotalLateMins; }
            set { m_AttendancePlanBonusPartialPaidMaxTotalLateMins = value; modify("AttendancePlanBonusPartialPaidMaxTotalLateMins"); }
        }
        protected bool m_AttendancePlanBonusPartialPaidMaxTotalLateMinsIncludeLunch;
        [DBField("AttendancePlanBonusPartialPaidMaxTotalLateMinsIncludeLunch"), TextSearch, MaxLength(2), Export(false)]
        public bool AttendancePlanBonusPartialPaidMaxTotalLateMinsIncludeLunch
        {
            get { return m_AttendancePlanBonusPartialPaidMaxTotalLateMinsIncludeLunch; }
            set { m_AttendancePlanBonusPartialPaidMaxTotalLateMinsIncludeLunch = value; modify("AttendancePlanBonusPartialPaidMaxTotalLateMinsIncludeLunch"); }
        }

        protected int m_AttendancePlanBonusPartialPaidMaxTotalEarlyLeaveCount;
        [DBField("AttendancePlanBonusPartialPaidMaxTotalEarlyLeaveCount"), TextSearch, MaxLength(2), Export(false)]
        public int AttendancePlanBonusPartialPaidMaxTotalEarlyLeaveCount
        {
            get { return m_AttendancePlanBonusPartialPaidMaxTotalEarlyLeaveCount; }
            set { m_AttendancePlanBonusPartialPaidMaxTotalEarlyLeaveCount = value; modify("AttendancePlanBonusPartialPaidMaxTotalEarlyLeaveCount"); }
        }
        protected bool m_AttendancePlanBonusPartialPaidMaxTotalEarlyLeaveCountIncludeLunch;
        [DBField("AttendancePlanBonusPartialPaidMaxTotalEarlyLeaveCountIncludeLunch"), TextSearch, MaxLength(2), Export(false)]
        public bool AttendancePlanBonusPartialPaidMaxTotalEarlyLeaveCountIncludeLunch
        {
            get { return m_AttendancePlanBonusPartialPaidMaxTotalEarlyLeaveCountIncludeLunch; }
            set { m_AttendancePlanBonusPartialPaidMaxTotalEarlyLeaveCountIncludeLunch = value; modify("AttendancePlanBonusPartialPaidMaxTotalEarlyLeaveCountIncludeLunch"); }
        }
        protected int m_AttendancePlanBonusPartialPaidMaxTotalEarlyLeaveMins;
        [DBField("AttendancePlanBonusPartialPaidMaxTotalEarlyLeaveMins"), TextSearch, MaxLength(4), Export(false)]
        public int AttendancePlanBonusPartialPaidMaxTotalEarlyLeaveMins
        {
            get { return m_AttendancePlanBonusPartialPaidMaxTotalEarlyLeaveMins; }
            set { m_AttendancePlanBonusPartialPaidMaxTotalEarlyLeaveMins = value; modify("AttendancePlanBonusPartialPaidMaxTotalEarlyLeaveMins"); }
        }
        protected bool m_AttendancePlanBonusPartialPaidMaxTotalEarlyLeaveMinsIncludeLunch;
        [DBField("AttendancePlanBonusPartialPaidMaxTotalEarlyLeaveMinsIncludeLunch"), TextSearch, MaxLength(2), Export(false)]
        public bool AttendancePlanBonusPartialPaidMaxTotalEarlyLeaveMinsIncludeLunch
        {
            get { return m_AttendancePlanBonusPartialPaidMaxTotalEarlyLeaveMinsIncludeLunch; }
            set { m_AttendancePlanBonusPartialPaidMaxTotalEarlyLeaveMinsIncludeLunch = value; modify("AttendancePlanBonusPartialPaidMaxTotalEarlyLeaveMinsIncludeLunch"); }
        }

        protected int m_AttendancePlanBonusPartialPaidMaxTotalSLWithMedicalCertificate;
        [DBField("AttendancePlanBonusPartialPaidMaxTotalSLWithMedicalCertificate"), TextSearch, MaxLength(2), Export(false)]
        public int AttendancePlanBonusPartialPaidMaxTotalSLWithMedicalCertificate
        {
            get { return m_AttendancePlanBonusPartialPaidMaxTotalSLWithMedicalCertificate; }
            set { m_AttendancePlanBonusPartialPaidMaxTotalSLWithMedicalCertificate = value; modify("AttendancePlanBonusPartialPaidMaxTotalSLWithMedicalCertificate"); }
        }

        protected int m_AttendancePlanBonusPartialPaidMaxTotalSLWithoutMedicalCertificate;
        [DBField("AttendancePlanBonusPartialPaidMaxTotalSLWithoutMedicalCertificate"), TextSearch, MaxLength(2), Export(false)]
        public int AttendancePlanBonusPartialPaidMaxTotalSLWithoutMedicalCertificate
        {
            get { return m_AttendancePlanBonusPartialPaidMaxTotalSLWithoutMedicalCertificate; }
            set { m_AttendancePlanBonusPartialPaidMaxTotalSLWithoutMedicalCertificate = value; modify("AttendancePlanBonusPartialPaidMaxTotalSLWithoutMedicalCertificate"); }
        }

        protected int m_AttendancePlanBonusPartialPaidMaxTotalAbsentCount;
        [DBField("AttendancePlanBonusPartialPaidMaxTotalAbsentCount"), TextSearch, MaxLength(2), Export(false)]
        public int AttendancePlanBonusPartialPaidMaxTotalAbsentCount
        {
            get { return m_AttendancePlanBonusPartialPaidMaxTotalAbsentCount; }
            set { m_AttendancePlanBonusPartialPaidMaxTotalAbsentCount = value; modify("AttendancePlanBonusPartialPaidMaxTotalAbsentCount"); }
        }

        protected int m_AttendancePlanBonusPartialPaidMaxTotalNonFullPayCasualLeave;
        [DBField("AttendancePlanBonusPartialPaidMaxTotalNonFullPayCasualLeave"), TextSearch, MaxLength(2), Export(false)]
        public int AttendancePlanBonusPartialPaidMaxTotalNonFullPayCasualLeave
        {
            get { return m_AttendancePlanBonusPartialPaidMaxTotalNonFullPayCasualLeave; }
            set { m_AttendancePlanBonusPartialPaidMaxTotalNonFullPayCasualLeave = value; modify("AttendancePlanBonusPartialPaidMaxTotalNonFullPayCasualLeave"); }
        }

        protected int m_AttendancePlanBonusPartialPaidMaxTotalInjuryLeave;
        [DBField("AttendancePlanBonusPartialPaidMaxTotalInjuryLeave"), TextSearch, MaxLength(2), Export(false)]
        public int AttendancePlanBonusPartialPaidMaxTotalInjuryLeave
        {
            get { return m_AttendancePlanBonusPartialPaidMaxTotalInjuryLeave; }
            set { m_AttendancePlanBonusPartialPaidMaxTotalInjuryLeave = value; modify("AttendancePlanBonusPartialPaidMaxTotalInjuryLeave"); }
        }

        
    }


}
