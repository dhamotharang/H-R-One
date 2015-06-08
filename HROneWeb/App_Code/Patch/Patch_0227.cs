//using System;
//using System.Collections;
//using HROne.DataAccess;
////using perspectivemind.validation;

//namespace HROne.Patch
//{
//    public class Patch_0227
//    {
//        [DBClass("PayrollGroup")]
//        private class OLDPayrollGroup : DBObject
//        {
//            public static DBManager db = new DBManager(typeof(OLDPayrollGroup));

//            protected int m_PayGroupID;
//            [DBField("PayGroupID", true, true), TextSearch, Export(false)]
//            public int PayGroupID
//            {
//                get { return m_PayGroupID; }
//                set { m_PayGroupID = value; modify("PayGroupID"); }
//            }

//            protected int m_PayGroupDefaultProrataFormula;
//            [DBField("PayGroupDefaultProrataFormula"), TextSearch, Export(false), Required]
//            public int PayGroupDefaultProrataFormula
//            {
//                get { return m_PayGroupDefaultProrataFormula; }
//                set { m_PayGroupDefaultProrataFormula = value; modify("PayGroupDefaultProrataFormula"); }
//            }


//            protected int m_PayGroupNewJoinFormula;
//            [DBField("PayGroupNewJoinFormula"), TextSearch, Export(false), Required]
//            public int PayGroupNewJoinFormula
//            {
//                get { return m_PayGroupNewJoinFormula; }
//                set { m_PayGroupNewJoinFormula = value; modify("PayGroupNewJoinFormula"); }
//            }

//            protected int m_PayGroupTerminatedFormula;
//            [DBField("PayGroupTerminatedFormula"), TextSearch, Export(false), Required]
//            public int PayGroupTerminatedFormula
//            {
//                get { return m_PayGroupTerminatedFormula; }
//                set { m_PayGroupTerminatedFormula = value; modify("PayGroupTerminatedFormula"); }
//            }


//            protected int m_PayGroupTerminatedALCompensationDailyFormula;
//            [DBField("PayGroupTerminatedALCompensationDailyFormula"), TextSearch, Export(false), Required]
//            public int PayGroupTerminatedALCompensationDailyFormula
//            {
//                get { return m_PayGroupTerminatedALCompensationDailyFormula; }
//                set { m_PayGroupTerminatedALCompensationDailyFormula = value; modify("PayGroupTerminatedALCompensationDailyFormula"); }
//            }


//            protected int m_PayGroupTerminatedPaymentInLieuDailyFormula;
//            [DBField("PayGroupTerminatedPaymentInLieuDailyFormula"), TextSearch, Export(false), Required]
//            public int PayGroupTerminatedPaymentInLieuDailyFormula
//            {
//                get { return m_PayGroupTerminatedPaymentInLieuDailyFormula; }
//                set { m_PayGroupTerminatedPaymentInLieuDailyFormula = value; modify("PayGroupTerminatedPaymentInLieuDailyFormula"); }
//            }



//            protected int m_PayGroupStatHolDeductFormula;
//            [DBField("PayGroupStatHolDeductFormula"), TextSearch, Export(false), Required]
//            public int PayGroupStatHolDeductFormula
//            {
//                get { return m_PayGroupStatHolDeductFormula; }
//                set { m_PayGroupStatHolDeductFormula = value; modify("PayGroupStatHolDeductFormula"); }
//            }

//            protected int m_PayGroupStatHolAllowFormula;
//            [DBField("PayGroupStatHolAllowFormula"), TextSearch, Export(false), Required]
//            public int PayGroupStatHolAllowFormula
//            {
//                get { return m_PayGroupStatHolAllowFormula; }
//                set { m_PayGroupStatHolAllowFormula = value; modify("PayGroupStatHolAllowFormula"); }
//            }

//        }

//        [DBClass("LeaveCode")]
//        private class OLDLeaveCode : DBObject
//        {
//            public static DBManager db = new DBManager(typeof(OLDLeaveCode));

//            protected int m_LeaveCodeID;
//            [DBField("LeaveCodeID", true, true), TextSearch, Export(false)]
//            public int LeaveCodeID
//            {
//                get { return m_LeaveCodeID; }
//                set { m_LeaveCodeID = value; modify("LeaveCodeID"); }
//            }
//            protected int m_LeaveCodeLeaveAllowFormula;
//            [DBField("LeaveCodeLeaveAllowFormula"), TextSearch, Export(false), Required]
//            public int LeaveCodeLeaveAllowFormula
//            {
//                get { return m_LeaveCodeLeaveAllowFormula; }
//                set { m_LeaveCodeLeaveAllowFormula = value; modify("LeaveCodeLeaveAllowFormula"); }

//            }
//            protected int m_LeaveCodeLeaveDeductFormula;
//            [DBField("LeaveCodeLeaveDeductFormula"), TextSearch, Export(false), Required]
//            public int LeaveCodeLeaveDeductFormula
//            {
//                get { return m_LeaveCodeLeaveDeductFormula; }
//                set { m_LeaveCodeLeaveDeductFormula = value; modify("LeaveCodeLeaveDeductFormula"); }
//            }
//        }

//        [DBClass("AttendanceFormula")]
//        private class OLDAttendanceFormula : DBObject
//        {
//            public static DBManager db = new DBManager(typeof(OLDAttendanceFormula));

//            protected int m_AttendanceFormulaID;
//            [DBField("AttendanceFormulaID", true, true), TextSearch, Export(false)]
//            public int AttendanceFormulaID
//            {
//                get { return m_AttendanceFormulaID; }
//                set { m_AttendanceFormulaID = value; modify("AttendanceFormulaID"); }
//            }
//            protected int m_AttendanceFormulaPayFormID;
//            [DBField("AttendanceFormulaPayFormID"), TextSearch, Export(false)]
//            public int AttendanceFormulaPayFormID
//            {
//                get { return m_AttendanceFormulaPayFormID; }
//                set { m_AttendanceFormulaPayFormID = value; modify("AttendanceFormulaPayFormID"); }
//            }
//        }

//        [DBClass("PayrollProrataFormula")]
//        private class OLDPayrollProrataFormula : DBObject
//        {
//            public const string DEFAULT_FOEMULA_CODE = "<DEFAULT>";

//            public static DBManager db = new DBManager(typeof(OLDPayrollProrataFormula));

//            protected int m_PayFormID;
//            [DBField("PayFormID", true, true), TextSearch, Export(false)]
//            public int PayFormID
//            {
//                get { return m_PayFormID; }
//                set { m_PayFormID = value; modify("PayFormID"); }
//            }

//            protected string m_PayFormCode;
//            [DBField("PayFormCode"), TextSearch, Export(false), Required]
//            public string PayFormCode
//            {
//                get { return m_PayFormCode; }
//                set { m_PayFormCode = value; modify("PayFormCode"); }
//            }


//        }

//        public static bool DBPatch(DatabaseConnection dbConn)
//        {

//            DBFilter payrollFormulaFilter = new DBFilter();
//            payrollFormulaFilter.add(new Match("PayFormCode", "<DEFAULT>"));
//            ArrayList defaultPayrollFormulaList = OLDPayrollProrataFormula.db.select(dbConn, payrollFormulaFilter);
//            if (defaultPayrollFormulaList.Count > 0)
//            {
//                int DefaultPayrollFormulaID = ((OLDPayrollProrataFormula)defaultPayrollFormulaList[0]).PayFormID;

//                ArrayList payrollGroupList = OLDPayrollGroup.db.select(dbConn, new DBFilter());

//                int commonPayrollFormulaID = 0;

//                foreach (OLDPayrollGroup payGroup in payrollGroupList)
//                {
//                    int payGroupDefaultFormula = 0;
//                    if (payGroup.PayGroupNewJoinFormula.Equals(payGroup.PayGroupTerminatedFormula) && payGroup.PayGroupTerminatedFormula > 0)
//                    {
//                        payGroupDefaultFormula = payGroup.PayGroupNewJoinFormula;
//                        payGroup.PayGroupDefaultProrataFormula = payGroupDefaultFormula;
//                        payGroup.PayGroupNewJoinFormula = DefaultPayrollFormulaID;
//                        payGroup.PayGroupTerminatedFormula = DefaultPayrollFormulaID;

//                        if (commonPayrollFormulaID.Equals(0))
//                            commonPayrollFormulaID = payGroupDefaultFormula;
//                        else if (commonPayrollFormulaID > 0)
//                            if (!commonPayrollFormulaID.Equals(payGroupDefaultFormula))
//                                commonPayrollFormulaID = -1;

//                        //if (payGroup.PayGroupLeaveAllowFormula.Equals(payGroupDefaultFormula))
//                        //    payGroup.PayGroupLeaveAllowFormula = DefaultPayrollFormulaID;
//                        //if (payGroup.PayGroupLeaveDeductFormula.Equals(payGroupDefaultFormula))
//                        //    payGroup.PayGroupLeaveDeductFormula = DefaultPayrollFormulaID;
//                        if (payGroup.PayGroupStatHolAllowFormula.Equals(payGroupDefaultFormula))
//                            payGroup.PayGroupStatHolAllowFormula = DefaultPayrollFormulaID;
//                        if (payGroup.PayGroupStatHolDeductFormula.Equals(payGroupDefaultFormula))
//                            payGroup.PayGroupStatHolDeductFormula = DefaultPayrollFormulaID;
//                        if (payGroup.PayGroupTerminatedALCompensationDailyFormula.Equals(payGroupDefaultFormula))
//                            payGroup.PayGroupTerminatedALCompensationDailyFormula = DefaultPayrollFormulaID;
//                        if (payGroup.PayGroupTerminatedPaymentInLieuDailyFormula.Equals(payGroupDefaultFormula))
//                            payGroup.PayGroupTerminatedPaymentInLieuDailyFormula = DefaultPayrollFormulaID;

//                        OLDPayrollGroup.db.update(dbConn, payGroup);
//                    }
//                }

//                if (commonPayrollFormulaID > 0)
//                {
//                    ArrayList leaveCodeList = OLDLeaveCode.db.select(dbConn, new DBFilter());
//                    foreach (OLDLeaveCode leaveCode in leaveCodeList)
//                    {
//                        if (leaveCode.LeaveCodeLeaveAllowFormula.Equals(commonPayrollFormulaID))
//                            leaveCode.LeaveCodeLeaveAllowFormula = DefaultPayrollFormulaID;
//                        if (leaveCode.LeaveCodeLeaveDeductFormula.Equals(commonPayrollFormulaID))
//                            leaveCode.LeaveCodeLeaveDeductFormula = DefaultPayrollFormulaID;
//                        OLDLeaveCode.db.update(dbConn, leaveCode);
//                    }
//                    ArrayList attendanceFormulaList = OLDAttendanceFormula.db.select(dbConn, new DBFilter());
//                    foreach (OLDAttendanceFormula attendanceFormula in attendanceFormulaList)
//                    {
//                        if (attendanceFormula.AttendanceFormulaPayFormID.Equals(commonPayrollFormulaID))
//                            attendanceFormula.AttendanceFormulaPayFormID = DefaultPayrollFormulaID;
//                        OLDAttendanceFormula.db.update(dbConn, attendanceFormula);
//                    }
//                }
//                return true;
//            }
//            else
//                return false;
//        }

//    }
//}