using System;
using System.Collections.Generic;
using System.Text;
using HROne.DataAccess;
////using perspectivemind.validation;
using System.Collections;
using HROne.Common;

namespace HROne.Lib.Entities
{
    [DBClass("PayrollGroup")]
    public class EPayrollGroup : BaseObjectWithRecordInfo
    {
        public static DBManager db = new DBManagerWithRecordInfo(typeof(EPayrollGroup));
        public static WFValueList VLPayrollGroup = new AppUtils.EncryptedDBCodeList(EPayrollGroup.db, "PayGroupID", new string[] { "PayGroupCode", "PayGroupDesc" }, " - ", "PayGroupCode");
        public static WFValueList VLPayGroupFreq = new AppUtils.NewWFTextList(new string[] { "M", "S" }, new string[] { "Monthly", "Semi-Monthly" });

        public static EPayrollGroup GetObject(DatabaseConnection dbConn, object ID)
        {
            if (ID is int)
            {
                EPayrollGroup obj = new EPayrollGroup();
                obj.PayGroupID = (int) ID;
                if (db.select(dbConn, obj))
                    return obj;
            }
            return null;
        }

        protected int m_PayGroupID;
        [DBField("PayGroupID", true, true), TextSearch, Export(false)]
        public int PayGroupID
        {
            get { return m_PayGroupID; }
            set { m_PayGroupID = value; modify("PayGroupID"); }
        }
        protected string m_PayGroupCode;
        [DBField("PayGroupCode"), MaxLength(20, 20), TextSearch, DBAESEncryptStringField, Export(false), Required]
        public string PayGroupCode
        {
            get { return m_PayGroupCode; }
            set { m_PayGroupCode = value; modify("PayGroupCode"); }
        }

        protected string m_PayGroupDesc;
        [DBField("PayGroupDesc"), MaxLength(100, 40), TextSearch, DBAESEncryptStringField, Export(false), Required]
        public string PayGroupDesc
        {
            get { return m_PayGroupDesc; }
            set { m_PayGroupDesc = value; modify("PayGroupDesc"); }
        }

        protected string m_PayGroupFreq = string.Empty;
        [DBField("PayGroupFreq"), TextSearch, Export(false), Required]
        public string PayGroupFreq
        {
            get { return m_PayGroupFreq; }
            set { m_PayGroupFreq = value; modify("PayGroupFreq"); }
        }

        protected int m_PayGroupDefaultStartDay;
        [DBField("PayGroupDefaultStartDay"), MaxLength(2), TextSearch, Export(false), Required]
        public int PayGroupDefaultStartDay
        {
            get { return m_PayGroupDefaultStartDay; }
            set { m_PayGroupDefaultStartDay = value; modify("PayGroupDefaultStartDay"); }
        }
        
        [Obsolete("Replaced by PayGroupDefaultNextStartDay")]
        public int PayGroupDefaultEndDay
        {
            get
            {
                //if (m_PayGroupDefaultNextStartDay == 0)
                //    return m_PayGroupDefaultEndDay;
                //else
                    return ((m_PayGroupDefaultNextStartDay - 1) + 30) % 31 + 1;
            }
        }

        protected int m_PayGroupDefaultNextStartDay;
        [DBField("PayGroupDefaultNextStartDay"), MaxLength(2), TextSearch, Export(false)]
        public int PayGroupDefaultNextStartDay
        {
            get { return m_PayGroupDefaultNextStartDay; }
            set { m_PayGroupDefaultNextStartDay = value; modify("PayGroupDefaultNextStartDay"); }
        }

        protected int m_PayGroupLeaveDefaultCutOffDay;
        [DBField("PayGroupLeaveDefaultCutOffDay"), MaxLength(2), TextSearch, Export(false), Required]
        public int PayGroupLeaveDefaultCutOffDay
        {
            get { return m_PayGroupLeaveDefaultCutOffDay; }
            set { m_PayGroupLeaveDefaultCutOffDay = value; modify("PayGroupLeaveDefaultCutOffDay"); }
        }

        protected int m_PayGroupLeaveDefaultNextCutOffDay;
        [DBField("PayGroupLeaveDefaultNextCutOffDay"), MaxLength(2), TextSearch, Export(false)]
        public int PayGroupLeaveDefaultNextCutOffDay
        {
            get { return m_PayGroupLeaveDefaultNextCutOffDay; }
            set { m_PayGroupLeaveDefaultNextCutOffDay = value; modify("PayGroupLeaveDefaultNextCutOffDay"); }
        }

        protected DateTime m_PayGroupNextStartDate;
        [DBField("PayGroupNextStartDate"), TextSearch, Export(false), Required]
        public DateTime PayGroupNextStartDate
        {
            get { return m_PayGroupNextStartDate; }
            set { m_PayGroupNextStartDate = value; modify("PayGroupNextStartDate"); }
        }

        protected DateTime m_PayGroupNextEndDate;
        [DBField("PayGroupNextEndDate"), TextSearch, Export(false), Required]
        public DateTime PayGroupNextEndDate
        {
            get { return m_PayGroupNextEndDate; }
            set { m_PayGroupNextEndDate = value; modify("PayGroupNextEndDate"); }
        }

        protected int m_CurrentPayPeriodID;
        [DBField("CurrentPayPeriodID"), TextSearch, Export(false)]
        public int CurrentPayPeriodID
        {
            get { return m_CurrentPayPeriodID; }
            set { m_CurrentPayPeriodID = value; modify("CurrentPayPeriodID"); }
        }

        protected int m_PayGroupDefaultProrataFormula;
        [DBField("PayGroupDefaultProrataFormula"), TextSearch, Export(false), Required]
        public int PayGroupDefaultProrataFormula
        {
            get { return m_PayGroupDefaultProrataFormula; }
            set { m_PayGroupDefaultProrataFormula = value; modify("PayGroupDefaultProrataFormula"); }
        }

        protected bool m_PayGroupRestDayHasWage;
        [DBField("PayGroupRestDayHasWage"), TextSearch, Export(false)]
        public bool PayGroupRestDayHasWage
        {
            get { return m_PayGroupRestDayHasWage; }
            set { m_PayGroupRestDayHasWage = value; modify("PayGroupRestDayHasWage"); }
        }

        protected bool m_PayGroupLunchTimeHasWage;
        [DBField("PayGroupLunchTimeHasWage"), TextSearch, Export(false)]
        public bool PayGroupLunchTimeHasWage
        {
            get { return m_PayGroupLunchTimeHasWage; }
            set { m_PayGroupLunchTimeHasWage = value; modify("PayGroupLunchTimeHasWage"); }
        }
        
        //protected int m_PayGroupLeaveDeductFormula;
        //[Obsolete, DBField("PayGroupLeaveDeductFormula"), TextSearch, Export(false)]
        //public int PayGroupLeaveDeductFormula
        //{
        //    get { return m_PayGroupLeaveDeductFormula; }
        //    set { m_PayGroupLeaveDeductFormula = value; modify("PayGroupLeaveDeductFormula"); }
        //}

        //protected int m_PayGroupLeaveAllowFormula;
        //[Obsolete, DBField("PayGroupLeaveAllowFormula"), TextSearch, Export(false)]
        //public int PayGroupLeaveAllowFormula
        //{
        //    get { return m_PayGroupLeaveAllowFormula; }
        //    set { m_PayGroupLeaveAllowFormula = value; modify("PayGroupLeaveAllowFormula"); }
        //}

        protected int m_PayGroupNewJoinFormula;
        [DBField("PayGroupNewJoinFormula"), TextSearch, Export(false), Required]
        public int PayGroupNewJoinFormula
        {
            get { return m_PayGroupNewJoinFormula; }
            set { m_PayGroupNewJoinFormula = value; modify("PayGroupNewJoinFormula"); }
        }

        protected int m_PayGroupExistingFormula;
        [DBField("PayGroupExistingFormula"), TextSearch, Export(false), Required]
        public int PayGroupExistingFormula
        {
            get { return m_PayGroupExistingFormula; }
            set { m_PayGroupExistingFormula = value; modify("PayGroupExistingFormula"); }
        }

        protected int m_PayGroupRestDayProrataFormula;
        [DBField("PayGroupRestDayProrataFormula"), TextSearch, Export(false)]
        public int PayGroupRestDayProrataFormula
        {
            get { return m_PayGroupRestDayProrataFormula; }
            set { m_PayGroupRestDayProrataFormula = value; modify("PayGroupRestDayProrataFormula"); }
        }

        protected int m_PayGroupAdditionalRemunerationPayCodeID;
        [DBField("PayGroupAdditionalRemunerationPayCodeID"), TextSearch, Export(false)]
        public int PayGroupAdditionalRemunerationPayCodeID
        {
            get { return m_PayGroupAdditionalRemunerationPayCodeID; }
            set { m_PayGroupAdditionalRemunerationPayCodeID = value; modify("PayGroupAdditionalRemunerationPayCodeID"); }
        }

        protected int m_PayGroupTerminatedFormula;
        [DBField("PayGroupTerminatedFormula"), TextSearch, Export(false), Required]
        public int PayGroupTerminatedFormula
        {
            get { return m_PayGroupTerminatedFormula; }
            set { m_PayGroupTerminatedFormula = value; modify("PayGroupTerminatedFormula"); }
        }


        protected bool m_PayGroupTerminatedALCompensationIsSkipRoundingRule;
        [DBField("PayGroupTerminatedALCompensationIsSkipRoundingRule"), TextSearch, Export(false)]
        public bool PayGroupTerminatedALCompensationIsSkipRoundingRule
        {
            get { return m_PayGroupTerminatedALCompensationIsSkipRoundingRule; }
            set { m_PayGroupTerminatedALCompensationIsSkipRoundingRule = value; modify("PayGroupTerminatedALCompensationIsSkipRoundingRule"); }
        }

        protected bool m_PayGroupTerminatedALCompensationEligibleAfterProbation;
        [DBField("PayGroupTerminatedALCompensationEligibleAfterProbation"), TextSearch, Export(false)]
        public bool PayGroupTerminatedALCompensationEligibleAfterProbation
        {
            get { return m_PayGroupTerminatedALCompensationEligibleAfterProbation; }
            set { m_PayGroupTerminatedALCompensationEligibleAfterProbation = value; modify("PayGroupTerminatedALCompensationEligibleAfterProbation"); }
        }

        protected int m_PayGroupTerminatedALCompensationProrataEligiblePeriod;
        [DBField("PayGroupTerminatedALCompensationProrataEligiblePeriod"), MaxLength(2), TextSearch, Export(false)]
        public int PayGroupTerminatedALCompensationProrataEligiblePeriod
        {
            get { return m_PayGroupTerminatedALCompensationProrataEligiblePeriod; }
            set { m_PayGroupTerminatedALCompensationProrataEligiblePeriod = value; modify("PayGroupTerminatedALCompensationProrataEligiblePeriod"); }
        }

        protected string m_PayGroupTerminatedALCompensationProrataEligibleUnit;
        [DBField("PayGroupTerminatedALCompensationProrataEligibleUnit"), TextSearch, Export(false)]
        public string PayGroupTerminatedALCompensationProrataEligibleUnit
        {
            get { return m_PayGroupTerminatedALCompensationProrataEligibleUnit; }
            set { m_PayGroupTerminatedALCompensationProrataEligibleUnit = value; modify("PayGroupTerminatedALCompensationProrataEligibleUnit"); }
        }

        protected bool m_PayGroupTerminatedALCompensationProrataEligibleCheckEveryLeaveYear;
        [DBField("PayGroupTerminatedALCompensationProrataEligibleCheckEveryLeaveYear"), TextSearch, Export(false), Required]
        public bool PayGroupTerminatedALCompensationProrataEligibleCheckEveryLeaveYear
        {
            get { return m_PayGroupTerminatedALCompensationProrataEligibleCheckEveryLeaveYear; }
            set { m_PayGroupTerminatedALCompensationProrataEligibleCheckEveryLeaveYear = value; modify("PayGroupTerminatedALCompensationProrataEligibleCheckEveryLeaveYear"); }
        }

        protected int m_PayGroupTerminatedALCompensationDailyFormula;
        [DBField("PayGroupTerminatedALCompensationDailyFormula"), TextSearch, Export(false), Required]
        public int PayGroupTerminatedALCompensationDailyFormula
        {
            get { return m_PayGroupTerminatedALCompensationDailyFormula; }
            set { m_PayGroupTerminatedALCompensationDailyFormula = value; modify("PayGroupTerminatedALCompensationDailyFormula"); }
        }

        protected int m_PayGroupTerminatedALCompensationDailyFormulaAlternative;
        [DBField("PayGroupTerminatedALCompensationDailyFormulaAlternative"), TextSearch, Export(false)]
        public int PayGroupTerminatedALCompensationDailyFormulaAlternative
        {
            get { return m_PayGroupTerminatedALCompensationDailyFormulaAlternative; }
            set { m_PayGroupTerminatedALCompensationDailyFormulaAlternative = value; modify("PayGroupTerminatedALCompensationDailyFormulaAlternative"); }
        }

        protected int m_PayGroupTerminatedALCompensationPaymentCodeID;
        [DBField("PayGroupTerminatedALCompensationPaymentCodeID"), TextSearch, Export(false), Required]
        public int PayGroupTerminatedALCompensationPaymentCodeID
        {
            get { return m_PayGroupTerminatedALCompensationPaymentCodeID; }
            set { m_PayGroupTerminatedALCompensationPaymentCodeID = value; modify("PayGroupTerminatedALCompensationPaymentCodeID"); }
        }
        protected int m_PayGroupTerminatedALCompensationByEEPaymentCodeID;
        [DBField("PayGroupTerminatedALCompensationByEEPaymentCodeID"), TextSearch, Export(false)]
        public int PayGroupTerminatedALCompensationByEEPaymentCodeID
        {
            get { return m_PayGroupTerminatedALCompensationByEEPaymentCodeID; }
            set { m_PayGroupTerminatedALCompensationByEEPaymentCodeID = value; modify("PayGroupTerminatedALCompensationByEEPaymentCodeID"); }
        }
        
        protected string m_PayGroupTerminatedPaymentInLieuMonthlyBaseMethod;
        [DBField("PayGroupTerminatedPaymentInLieuMonthlyBaseMethod"), TextSearch, Export(false), Required]
        public string PayGroupTerminatedPaymentInLieuMonthlyBaseMethod 
        {
            get { return m_PayGroupTerminatedPaymentInLieuMonthlyBaseMethod; }
            set { m_PayGroupTerminatedPaymentInLieuMonthlyBaseMethod = value; modify("PayGroupTerminatedPaymentInLieuMonthlyBaseMethod"); }
        }

        protected string m_PayGroupTerminatedPaymentInLieuMonthlyBaseMethodAlternative;
        [DBField("PayGroupTerminatedPaymentInLieuMonthlyBaseMethodAlternative"), TextSearch, Export(false)]
        public string PayGroupTerminatedPaymentInLieuMonthlyBaseMethodAlternative
        {
            get { return m_PayGroupTerminatedPaymentInLieuMonthlyBaseMethodAlternative; }
            set { m_PayGroupTerminatedPaymentInLieuMonthlyBaseMethodAlternative = value; modify("PayGroupTerminatedPaymentInLieuMonthlyBaseMethodAlternative"); }
        }

        protected int m_PayGroupTerminatedPaymentInLieuDailyFormula;
        [DBField("PayGroupTerminatedPaymentInLieuDailyFormula"), TextSearch, Export(false), Required]
        public int PayGroupTerminatedPaymentInLieuDailyFormula
        {
            get { return m_PayGroupTerminatedPaymentInLieuDailyFormula; }
            set { m_PayGroupTerminatedPaymentInLieuDailyFormula = value; modify("PayGroupTerminatedPaymentInLieuDailyFormula"); }
        }

        protected int m_PayGroupTerminatedPaymentInLieuDailyFormulaAlternative;
        [DBField("PayGroupTerminatedPaymentInLieuDailyFormulaAlternative"), TextSearch, Export(false)]
        public int PayGroupTerminatedPaymentInLieuDailyFormulaAlternative
        {
            get { return m_PayGroupTerminatedPaymentInLieuDailyFormulaAlternative; }
            set { m_PayGroupTerminatedPaymentInLieuDailyFormulaAlternative = value; modify("PayGroupTerminatedPaymentInLieuDailyFormulaAlternative"); }
        }

        protected int m_PayGroupTerminatedPaymentInLieuERPaymentCodeID;
        [DBField("PayGroupTerminatedPaymentInLieuERPaymentCodeID"), TextSearch, Export(false), Required]
        public int PayGroupTerminatedPaymentInLieuERPaymentCodeID
        {
            get { return m_PayGroupTerminatedPaymentInLieuERPaymentCodeID; }
            set { m_PayGroupTerminatedPaymentInLieuERPaymentCodeID = value; modify("PayGroupTerminatedPaymentInLieuERPaymentCodeID"); }
        }

        protected int m_PayGroupTerminatedPaymentInLieuEEPaymentCodeID;
        [DBField("PayGroupTerminatedPaymentInLieuEEPaymentCodeID"), TextSearch, Export(false), Required]
        public int PayGroupTerminatedPaymentInLieuEEPaymentCodeID
        {
            get { return m_PayGroupTerminatedPaymentInLieuEEPaymentCodeID; }
            set { m_PayGroupTerminatedPaymentInLieuEEPaymentCodeID = value; modify("PayGroupTerminatedPaymentInLieuEEPaymentCodeID"); }
        }

        protected string m_PayGroupTerminatedLSPSPMonthlyBaseMethod;
        [DBField("PayGroupTerminatedLSPSPMonthlyBaseMethod"), TextSearch, Export(false), Required]
        public string PayGroupTerminatedLSPSPMonthlyBaseMethod
        {
            get { return m_PayGroupTerminatedLSPSPMonthlyBaseMethod; }
            set { m_PayGroupTerminatedLSPSPMonthlyBaseMethod = value; modify("PayGroupTerminatedLSPSPMonthlyBaseMethod"); }
        }

        protected string m_PayGroupTerminatedLSPSPMonthlyBaseMethodAlternative;
        [DBField("PayGroupTerminatedLSPSPMonthlyBaseMethodAlternative"), TextSearch, Export(false)]
        public string PayGroupTerminatedLSPSPMonthlyBaseMethodAlternative
        {
            get { return m_PayGroupTerminatedLSPSPMonthlyBaseMethodAlternative; }
            set { m_PayGroupTerminatedLSPSPMonthlyBaseMethodAlternative = value; modify("PayGroupTerminatedLSPSPMonthlyBaseMethodAlternative"); }
        }

        protected int m_PayGroupTerminatedLSPPaymentCodeID;
        [DBField("PayGroupTerminatedLSPPaymentCodeID"), TextSearch, Export(false), Required]
        public int PayGroupTerminatedLSPPaymentCodeID
        {
            get { return m_PayGroupTerminatedLSPPaymentCodeID; }
            set { m_PayGroupTerminatedLSPPaymentCodeID = value; modify("PayGroupTerminatedLSPPaymentCodeID"); }
        }

        protected int m_PayGroupTerminatedSPPaymentCodeID;
        [DBField("PayGroupTerminatedSPPaymentCodeID"), TextSearch, Export(false), Required]
        public int PayGroupTerminatedSPPaymentCodeID
        {
            get { return m_PayGroupTerminatedSPPaymentCodeID; }
            set { m_PayGroupTerminatedSPPaymentCodeID = value; modify("PayGroupTerminatedSPPaymentCodeID"); }
        }

        protected int m_PayGroupTerminatedRestDayCompensationDailyFormula;
        [DBField("PayGroupTerminatedRestDayCompensationDailyFormula"), TextSearch, Export(false)]
        public int PayGroupTerminatedRestDayCompensationDailyFormula
        {
            get { return m_PayGroupTerminatedRestDayCompensationDailyFormula; }
            set { m_PayGroupTerminatedRestDayCompensationDailyFormula = value; modify("PayGroupTerminatedRestDayCompensationDailyFormula"); }
        }

        protected int m_PayGroupTerminatedRestDayCompensationPaymentCodeID;
        [DBField("PayGroupTerminatedRestDayCompensationPaymentCodeID"), TextSearch, Export(false)]
        public int PayGroupTerminatedRestDayCompensationPaymentCodeID
        {
            get { return m_PayGroupTerminatedRestDayCompensationPaymentCodeID; }
            set { m_PayGroupTerminatedRestDayCompensationPaymentCodeID = value; modify("PayGroupTerminatedRestDayCompensationPaymentCodeID"); }
        }

        protected int m_PayGroupTerminatedStatutoryHolidayCompensationDailyFormula;
        [DBField("PayGroupTerminatedStatutoryHolidayCompensationDailyFormula"), TextSearch, Export(false)]
        public int PayGroupTerminatedStatutoryHolidayCompensationDailyFormula
        {
            get { return m_PayGroupTerminatedStatutoryHolidayCompensationDailyFormula; }
            set { m_PayGroupTerminatedStatutoryHolidayCompensationDailyFormula = value; modify("PayGroupTerminatedStatutoryHolidayCompensationDailyFormula"); }
        }

        protected int m_PayGroupTerminatedStatutoryHolidayCompensationPaymentCodeID;
        [DBField("PayGroupTerminatedStatutoryHolidayCompensationPaymentCodeID"), TextSearch, Export(false)]
        public int PayGroupTerminatedStatutoryHolidayCompensationPaymentCodeID
        {
            get { return m_PayGroupTerminatedStatutoryHolidayCompensationPaymentCodeID; }
            set { m_PayGroupTerminatedStatutoryHolidayCompensationPaymentCodeID = value; modify("PayGroupTerminatedStatutoryHolidayCompensationPaymentCodeID"); }
        }

        protected bool m_PayGroupIsSkipStatHol;
        [DBField("PayGroupIsSkipStatHol"), TextSearch, Export(false)]
        public bool PayGroupIsSkipStatHol
        {
            get { return m_PayGroupIsSkipStatHol; }
            set { m_PayGroupIsSkipStatHol = value; modify("PayGroupIsSkipStatHol"); }
        }

        protected int m_PayGroupStatHolDeductFormula;
        [DBField("PayGroupStatHolDeductFormula"), TextSearch, Export(false)]
        public int PayGroupStatHolDeductFormula
        {
            get { return m_PayGroupStatHolDeductFormula; }
            set { m_PayGroupStatHolDeductFormula = value; modify("PayGroupStatHolDeductFormula"); }
        }

        protected int m_PayGroupStatHolAllowFormula;
        [DBField("PayGroupStatHolAllowFormula"), TextSearch, Export(false)]
        public int PayGroupStatHolAllowFormula
        {
            get { return m_PayGroupStatHolAllowFormula; }
            set { m_PayGroupStatHolAllowFormula = value; modify("PayGroupStatHolAllowFormula"); }
        }

        protected int m_PayGroupStatHolAllowPaymentCodeID;
        [DBField("PayGroupStatHolAllowPaymentCodeID"), TextSearch, Export(false)]
        public int PayGroupStatHolAllowPaymentCodeID
        {
            get { return m_PayGroupStatHolAllowPaymentCodeID; }
            set { m_PayGroupStatHolAllowPaymentCodeID = value; modify("PayGroupStatHolAllowPaymentCodeID"); }
        }
        protected int m_PayGroupStatHolDeductPaymentCodeID;
        [DBField("PayGroupStatHolDeductPaymentCodeID"), TextSearch, Export(false)]
        public int PayGroupStatHolDeductPaymentCodeID
        {
            get { return m_PayGroupStatHolDeductPaymentCodeID; }
            set { m_PayGroupStatHolDeductPaymentCodeID = value; modify("PayGroupStatHolDeductPaymentCodeID"); }
        }

        protected bool m_PayGroupIsStatHolUsePublicHoliday;
        [DBField("PayGroupIsStatHolUsePublicHoliday"), TextSearch, Export(false)]
        public bool PayGroupIsStatHolUsePublicHoliday
        {
            get { return m_PayGroupIsStatHolUsePublicHoliday; }
            set { m_PayGroupIsStatHolUsePublicHoliday = value; modify("PayGroupIsStatHolUsePublicHoliday"); }
        }
        

        protected bool m_PayGroupPayAdvance;
        [DBField("PayGroupPayAdvance"), TextSearch, Export(false)]
        public bool PayGroupPayAdvance
        {
            get { return m_PayGroupPayAdvance; }
            set { m_PayGroupPayAdvance = value; modify("PayGroupPayAdvance"); }
        }
        protected bool m_PayGroupPayAdvanceCompareTotalPaymentOnly;
        [DBField("PayGroupPayAdvanceCompareTotalPaymentOnly"), TextSearch, Export(false)]
        public bool PayGroupPayAdvanceCompareTotalPaymentOnly
        {
            get { return m_PayGroupPayAdvanceCompareTotalPaymentOnly; }
            set { m_PayGroupPayAdvanceCompareTotalPaymentOnly = value; modify("PayGroupPayAdvanceCompareTotalPaymentOnly"); }
        }
        
        protected int m_PayGroupStatHolEligiblePeriod;
        [DBField("PayGroupStatHolEligiblePeriod"), MaxLength(2), TextSearch, Export(false)]
        public int PayGroupStatHolEligiblePeriod
        {
            get { return m_PayGroupStatHolEligiblePeriod; }
            set { m_PayGroupStatHolEligiblePeriod = value; modify("PayGroupStatHolEligiblePeriod"); }
        }

        protected string m_PayGroupStatHolEligibleUnit;
        [DBField("PayGroupStatHolEligibleUnit"), TextSearch, Export(false)]
        public string PayGroupStatHolEligibleUnit
        {
            get { return m_PayGroupStatHolEligibleUnit; }
            set { m_PayGroupStatHolEligibleUnit = value; modify("PayGroupStatHolEligibleUnit"); }
        }

        protected bool m_PayGroupStatHolEligibleAfterProbation;
        [DBField("PayGroupStatHolEligibleAfterProbation"), TextSearch, Export(false)]
        public bool PayGroupStatHolEligibleAfterProbation
        {
            get { return m_PayGroupStatHolEligibleAfterProbation; }
            set { m_PayGroupStatHolEligibleAfterProbation = value; modify("PayGroupStatHolEligibleAfterProbation"); }
        }

        protected bool m_PayGroupStatHolEligibleSkipDeduction;
        [DBField("PayGroupStatHolEligibleSkipDeduction"), TextSearch, Export(false)]
        public bool PayGroupStatHolEligibleSkipDeduction
        {
            get { return m_PayGroupStatHolEligibleSkipDeduction; }
            set { m_PayGroupStatHolEligibleSkipDeduction = value; modify("PayGroupStatHolEligibleSkipDeduction"); }
        }

        protected bool m_PayGroupStatHolNextMonth;
        [DBField("PayGroupStatHolNextMonth"), TextSearch, Export(false)]
        public bool PayGroupStatHolNextMonth
        {
            get { return m_PayGroupStatHolNextMonth; }
            set { m_PayGroupStatHolNextMonth = value; modify("PayGroupStatHolNextMonth"); }
        }        

        protected int m_PayGroupYEBStartPayrollMonth;
        [DBField("PayGroupYEBStartPayrollMonth"), TextSearch, Export(false)]
        public int PayGroupYEBStartPayrollMonth
        {
            get { return m_PayGroupYEBStartPayrollMonth; }
            set { m_PayGroupYEBStartPayrollMonth = value; modify("PayGroupYEBStartPayrollMonth"); }
        }

        protected int m_PayGroupYEBMonthFrom;
        [DBField("PayGroupYEBMonthFrom"), TextSearch, Export(false)]
        public int PayGroupYEBMonthFrom
        {
            get { return m_PayGroupYEBMonthFrom; }
            set { m_PayGroupYEBMonthFrom = value; modify("PayGroupYEBMonthFrom"); }
        }

        protected int m_PayGroupYEBMonthTo;
        [DBField("PayGroupYEBMonthTo"), TextSearch, Export(false)]
        public int PayGroupYEBMonthTo
        {
            get { return m_PayGroupYEBMonthTo; }
            set { m_PayGroupYEBMonthTo = value; modify("PayGroupYEBMonthTo"); }
        }

        protected bool m_PayGroupUseCNDForDailyHourlyPayment;
        [DBField("PayGroupUseCNDForDailyHourlyPayment"), TextSearch, Export(false)]
        public bool PayGroupUseCNDForDailyHourlyPayment
        {
            get { return m_PayGroupUseCNDForDailyHourlyPayment; }
            set { m_PayGroupUseCNDForDailyHourlyPayment = value; modify("PayGroupUseCNDForDailyHourlyPayment"); }
        }

        protected bool m_PayGroupIsCNDProrata;
        [DBField("PayGroupIsCNDProrata"), TextSearch, Export(false)]
        public bool PayGroupIsCNDProrata
        {
            get { return m_PayGroupIsCNDProrata; }
            set { m_PayGroupIsCNDProrata = value; modify("PayGroupIsCNDProrata"); }
        }

        protected bool m_PayGroupIsPublic;
        [DBField("PayGroupIsPublic"), TextSearch, Export(false)]
        public bool PayGroupIsPublic
        {
            get { return m_PayGroupIsPublic; }
            set { m_PayGroupIsPublic = value; modify("PayGroupIsPublic"); }
        }

        public static EPayrollGroup GetPayrollGroup(DatabaseConnection dbConn, int PayGroupID)
        {
            DBFilter dbFilter = new DBFilter();
            dbFilter.add(new Match("PayGroupID", PayGroupID));
            ArrayList payrollGroups = EPayrollGroup.db.select(dbConn, dbFilter);
            if (payrollGroups.Count > 0)
                return (EPayrollGroup)payrollGroups[0];
            else
                return null;
        }

        public EPayrollGroup Copy(DatabaseConnection dbConn)
        {
            EPayrollGroup obj = new EPayrollGroup();
            obj.PayGroupID = PayGroupID;
            if (EPayrollGroup.db.select(dbConn, obj))
            {
                obj.PayGroupID = 0;
                obj.PayGroupCode = obj.PayGroupCode + "_COPY";
                obj.PayGroupDesc = "Copy of " + obj.PayGroupDesc;
                obj.CurrentPayPeriodID = 0;

                EPayrollGroup.db.insert(dbConn, obj);
                return obj;
            }
            return null;
        }

        public int NumOfPeriodPerYear()
        {
            if (m_PayGroupFreq.Equals("M"))
                return 12;
            else
                return 24;
        }
    }
}
