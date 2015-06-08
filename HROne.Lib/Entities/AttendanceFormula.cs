using System;
using System.Collections.Generic;
using System.Text;
using HROne.DataAccess;
////using perspectivemind.validation;

namespace HROne.Lib.Entities
{
    [DBClass("AttendanceFormula")]
    public class EAttendanceFormula : BaseObject
    {
        public const string FORMULATYPE_CODE_BY_FORMULA = "F";
        public const string FORMULATYPE_CODE_FIX_RATE = "R";
        public static WFValueList VLFormulaType = new AppUtils.NewWFTextList(new string[] { FORMULATYPE_CODE_BY_FORMULA, FORMULATYPE_CODE_FIX_RATE }, new string[] { "Use Payroll Formula", "Fix Hourly Rate" });

        public static DBManager db = new DBManager(typeof(EAttendanceFormula));
        public static WFValueList VLAttendanceFormula = new WFDBList(EAttendanceFormula.db, "AttendanceFormulaID", "AttendanceFormulaCode + ' - ' + AttendanceFormulaDesc", "AttendanceFormulaCode");

        protected int m_AttendanceFormulaID;
        [DBField("AttendanceFormulaID", true, true), TextSearch, Export(false)]
        public int AttendanceFormulaID
        {
            get { return m_AttendanceFormulaID; }
            set { m_AttendanceFormulaID = value; modify("AttendanceFormulaID"); }
        }
        protected string m_AttendanceFormulaCode;
        [DBField("AttendanceFormulaCode"), TextSearch,MaxLength(20), Export(false),Required]
        public string AttendanceFormulaCode
        {
            get { return m_AttendanceFormulaCode; }
            set { m_AttendanceFormulaCode = value; modify("AttendanceFormulaCode"); }
        }
        protected string m_AttendanceFormulaDesc;
        [DBField("AttendanceFormulaDesc"), TextSearch, MaxLength(100, 40), Export(false), Required]
        public string AttendanceFormulaDesc
        {
            get { return m_AttendanceFormulaDesc; }
            set { m_AttendanceFormulaDesc = value; modify("AttendanceFormulaDesc"); }
        }
        protected string m_AttendanceFormulaType;
        [DBField("AttendanceFormulaType"), TextSearch, Export(false), Required]
        public string AttendanceFormulaType
        {
            get { return m_AttendanceFormulaType; }
            set { m_AttendanceFormulaType = value; modify("AttendanceFormulaType"); }
        }
        protected int m_AttendanceFormulaPayFormID;
        [DBField("AttendanceFormulaPayFormID"), TextSearch, Export(false)]
        public int AttendanceFormulaPayFormID
        {
            get { return m_AttendanceFormulaPayFormID; }
            set { m_AttendanceFormulaPayFormID = value; modify("AttendanceFormulaPayFormID"); }
        }
        protected double m_AttendanceFormulaWorkHourPerDay;
        [DBField("AttendanceFormulaWorkHourPerDay","0.##"), TextSearch, MaxLength(6), Export(false)]
        public double AttendanceFormulaWorkHourPerDay
        {
            get { return m_AttendanceFormulaWorkHourPerDay; }
            set { m_AttendanceFormulaWorkHourPerDay = value; modify("AttendanceFormulaWorkHourPerDay"); }
        }
        protected bool m_AttendanceFormulaIsUseRosterCodeForDefaultWorkHourPerDay;
        [DBField("AttendanceFormulaIsUseRosterCodeForDefaultWorkHourPerDay"), TextSearch, Export(false)]
        public bool AttendanceFormulaIsUseRosterCodeForDefaultWorkHourPerDay
        {
            get { return m_AttendanceFormulaIsUseRosterCodeForDefaultWorkHourPerDay; }
            set { m_AttendanceFormulaIsUseRosterCodeForDefaultWorkHourPerDay = value; modify("AttendanceFormulaIsUseRosterCodeForDefaultWorkHourPerDay"); }
        }
        
        protected double m_AttendanceFormulaFixedRate;
        [DBField("AttendanceFormulaFixedRate", "0.##"), TextSearch, MaxLength(6), Export(false)]
        public double AttendanceFormulaFixedRate
        {
            get { return m_AttendanceFormulaFixedRate; }
            set { m_AttendanceFormulaFixedRate = value; modify("AttendanceFormulaFixedRate"); }
        }

        protected int m_AttendanceFormulaDecimalPlace;
        [DBField("AttendanceFormulaDecimalPlace"), TextSearch, MaxLength(1), Export(false)]
        public int AttendanceFormulaDecimalPlace
        {
            get { return m_AttendanceFormulaDecimalPlace; }
            set { m_AttendanceFormulaDecimalPlace = value; modify("AttendanceFormulaDecimalPlace"); }
        }

        protected string m_AttendanceFormulaRoundingRule;
        [DBField("AttendanceFormulaRoundingRule"), TextSearch, MaxLength(50, 25), Export(false)]
        public string AttendanceFormulaRoundingRule
        {
            get { return m_AttendanceFormulaRoundingRule; }
            set { m_AttendanceFormulaRoundingRule = value; modify("AttendanceFormulaRoundingRule"); }
        }
    }


}
