using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Data;
using HROne.DataAccess;
using HROne.Common;
namespace HROne.Lib.Entities
{
    [DBClass("EmpAttendancePreparationProcess")]
    public class EEmpAttendancePreparationProcess : BaseObject
    {
        public static DBManager db = new DBManagerWithRecordInfo(typeof(EEmpAttendancePreparationProcess));

        public static EEmpAttendancePreparationProcess GetObject(DatabaseConnection dbConn, int ID)
        {
            EEmpAttendancePreparationProcess m_object = new EEmpAttendancePreparationProcess();
            m_object.EmpAPPID = ID;
            if (EEmpAttendancePreparationProcess.db.select(dbConn, m_object))
                return m_object;
            return null;
        }

        public static EEmpAttendancePreparationProcess GetObjectRPID(DatabaseConnection dbConn, int RPID)
        {
            DBFilter m_filter = new DBFilter();
            m_filter.add(new Match("EmpRPID", RPID));
            ArrayList m_list = EEmpAttendancePreparationProcess.db.select(dbConn, m_filter);
            if (m_list.Count > 0)
                return (EEmpAttendancePreparationProcess)m_list[0];
            else
                return null; 
        }

        protected int m_EmpAPPID;
        [DBField("EmpAPPID", true, true), TextSearch, Export(false)]
        public int EmpAPPID
        {
            get { return m_EmpAPPID; }
            set { m_EmpAPPID = value; modify("EmpAPPID"); }
        }

        protected int m_EmpID;
        [DBField("EmpID"), Int, TextSearch, Export(false)]
        public int EmpID
        {
            get { return m_EmpID; }
            set { m_EmpID = value; modify("EmpID"); }
        }

        protected int m_AttendancePreparationProcessID;
        [DBField("AttendancePreparationProcessID"), Int, TextSearch, Export(false)]
        public int AttendancePreparationProcessID
        {
            get { return m_AttendancePreparationProcessID; }
            set { m_AttendancePreparationProcessID = value; modify("AttendancePreparationProcessID"); }
        }

        protected int m_EmpRPID;
        [DBField("EmpRPID"), Int, TextSearch, Export(false), Required]
        public int EmpRPID
        {
            get { return m_EmpRPID; }
            set { m_EmpRPID = value; modify("EmpRPID"); }
        }

        //protected int m_ShiftDutyCodeID;
        //[DBField("ShiftDutyCodeID"), Int, TextSearch, Export(false), Required]
        //public int ShiftDutyCodeID
        //{
        //    get { return m_ShiftDutyCodeID; }
        //    set { m_ShiftDutyCodeID = value; modify("ShiftDutyCodeID"); }
        //}

        //protected int m_PayCalFormulaID;
        //[DBField("PayCalFormulaID"), Int, TextSearch, Export(false), Required]
        //public int PayCalFormulaID
        //{
        //    get { return m_PayCalFormulaID; }
        //    set { m_PayCalFormulaID = value; modify("PayCalFormulaID"); }
        //}

        protected string m_Day1;
        [DBField("Day1"), TextSearch, Export(false)]
        public string Day1
        {
            get { return m_Day1; }
            set { m_Day1 = value; modify("Day1"); }
        }

        protected string m_Day2;
        [DBField("Day2"), TextSearch, Export(false)]
        public string Day2
        {
            get { return m_Day2; }
            set { m_Day2 = value; modify("Day2"); }
        }

        protected string m_Day3;
        [DBField("Day3"), TextSearch, Export(false)]
        public string Day3
        {
            get { return m_Day3; }
            set { m_Day3 = value; modify("Day3"); }
        }

        protected string m_Day4;
        [DBField("Day4"), TextSearch, Export(false)]
        public string Day4
        {
            get { return m_Day4; }
            set { m_Day4 = value; modify("Day4"); }
        }

        protected string m_Day5;
        [DBField("Day5"), TextSearch, Export(false)]
        public string Day5
        {
            get { return m_Day5; }
            set { m_Day5 = value; modify("Day5"); }
        }

        protected string m_Day6;
        [DBField("Day6"), TextSearch, Export(false)]
        public string Day6
        {
            get { return m_Day6; }
            set { m_Day6 = value; modify("Day6"); }
        }

        protected string m_Day7;
        [DBField("Day7"), TextSearch, Export(false)]
        public string Day7
        {
            get { return m_Day7; }
            set { m_Day7 = value; modify("Day7"); }
        }

        protected string m_Day8;
        [DBField("Day8"), TextSearch, Export(false)]
        public string Day8
        {
            get { return m_Day8; }
            set { m_Day8 = value; modify("Day8"); }
        }

        protected string m_Day9;
        [DBField("Day9"), TextSearch, Export(false)]
        public string Day9
        {
            get { return m_Day9; }
            set { m_Day9 = value; modify("Day9"); }
        }

        protected string m_Day10;
        [DBField("Day10"), TextSearch, Export(false)]
        public string Day10
        {
            get { return m_Day10; }
            set { m_Day10 = value; modify("Day10"); }
        }

        protected string m_Day11;
        [DBField("Day11"), TextSearch, Export(false)]
        public string Day11
        {
            get { return m_Day11; }
            set { m_Day11 = value; modify("Day11"); }
        }

        protected string m_Day12;
        [DBField("Day12"), TextSearch, Export(false)]
        public string Day12
        {
            get { return m_Day12; }
            set { m_Day12 = value; modify("Day12"); }
        }

        protected string m_Day13;
        [DBField("Day13"), TextSearch, Export(false)]
        public string Day13
        {
            get { return m_Day13; }
            set { m_Day13 = value; modify("Day13"); }
        }

        protected string m_Day14;
        [DBField("Day14"), TextSearch, Export(false)]
        public string Day14
        {
            get { return m_Day14; }
            set { m_Day14 = value; modify("Day14"); }
        }

        protected string m_Day15;
        [DBField("Day15"), TextSearch, Export(false)]
        public string Day15
        {
            get { return m_Day15; }
            set { m_Day15 = value; modify("Day15"); }
        }

        protected string m_Day16;
        [DBField("Day16"), TextSearch, Export(false)]
        public string Day16
        {
            get { return m_Day16; }
            set { m_Day16 = value; modify("Day16"); }
        }

        protected string m_Day17;
        [DBField("Day17"), TextSearch, Export(false)]
        public string Day17
        {
            get { return m_Day17; }
            set { m_Day17 = value; modify("Day17"); }
        }

        protected string m_Day18;
        [DBField("Day18"), TextSearch, Export(false)]
        public string Day18
        {
            get { return m_Day18; }
            set { m_Day18 = value; modify("Day18"); }
        }

        protected string m_Day19;
        [DBField("Day19"), TextSearch, Export(false)]
        public string Day19
        {
            get { return m_Day19; }
            set { m_Day19 = value; modify("Day19"); }
        }

        protected string m_Day20;
        [DBField("Day20"), TextSearch, Export(false)]
        public string Day20
        {
            get { return m_Day20; }
            set { m_Day20 = value; modify("Day20"); }
        }

        protected string m_Day21;
        [DBField("Day21"), TextSearch, Export(false)]
        public string Day21
        {
            get { return m_Day21; }
            set { m_Day21 = value; modify("Day21"); }
        }

        protected string m_Day22;
        [DBField("Day22"), TextSearch, Export(false)]
        public string Day22
        {
            get { return m_Day22; }
            set { m_Day22 = value; modify("Day22"); }
        }

        protected string m_Day23;
        [DBField("Day23"), TextSearch, Export(false)]
        public string Day23
        {
            get { return m_Day23; }
            set { m_Day23 = value; modify("Day23"); }
        }

        protected string m_Day24;
        [DBField("Day24"), TextSearch, Export(false)]
        public string Day24
        {
            get { return m_Day24; }
            set { m_Day24 = value; modify("Day24"); }
        }

        protected string m_Day25;
        [DBField("Day25"), TextSearch, Export(false)]
        public string Day25
        {
            get { return m_Day25; }
            set { m_Day25 = value; modify("Day25"); }
        }

        protected string m_Day26;
        [DBField("Day26"), TextSearch, Export(false)]
        public string Day26
        {
            get { return m_Day26; }
            set { m_Day26 = value; modify("Day26"); }
        }

        protected string m_Day27;
        [DBField("Day27"), TextSearch, Export(false)]
        public string Day27
        {
            get { return m_Day27; }
            set { m_Day27 = value; modify("Day27"); }
        }

        protected string m_Day28;
        [DBField("Day28"), TextSearch, Export(false)]
        public string Day28
        {
            get { return m_Day28; }
            set { m_Day28 = value; modify("Day28"); }
        }

        protected string m_Day29;
        [DBField("Day29"), TextSearch, Export(false)]
        public string Day29
        {
            get { return m_Day29; }
            set { m_Day29 = value; modify("Day29"); }
        }

        protected string m_Day30;
        [DBField("Day30"), TextSearch, Export(false)]
        public string Day30
        {
            get { return m_Day30; }
            set { m_Day30 = value; modify("Day30"); }
        }

        protected string m_Day31;
        [DBField("Day31"), TextSearch, Export(false)]
        public string Day31
        {
            get { return m_Day31; }
            set { m_Day31 = value; modify("Day31"); }
        }

        protected int m_TotalHours;
        [DBField("TotalHours"), Int, TextSearch, Export(false), Required]
        public int TotalHours
        {
            get { return m_TotalHours; }
            set { m_TotalHours = value; modify("TotalHours"); }
        }

        protected string m_Remarks;
        [DBField("Remarks"), TextSearch, Export(false)]
        public string Remarks
        {
            get { return m_Remarks; }
            set { m_Remarks = value; modify("Remarks"); }
        }

        protected double m_ReductionOthers;
        [DBField("ReductionOthers", format = "#,##0.00"), TextSearch, Export(false), Required]
        public double ReductionOthers
        {
            get { return m_ReductionOthers; }
            set { m_ReductionOthers = value; modify("ReductionOthers"); }
        }

        protected double m_ReductionUniformTimecard;
        [DBField("ReductionUniformTimecard", format = "#,##0.00"), TextSearch, Export(false), Required]
        public double ReductionUniformTimecard
        {
            get { return m_ReductionUniformTimecard; }
            set { m_ReductionUniformTimecard = value; modify("ReductionUniformTimecard"); }
        }

        protected double m_BackpayAllowance;
        [DBField("BackpayAllowance", format = "#,##0.00"), TextSearch, Export(false), Required]
        public double BackpayAllowance
        {
            get { return m_BackpayAllowance; }
            set { m_BackpayAllowance = value; modify("BackpayAllowance"); }
        }

        protected double m_BackpayOthers;
        [DBField("BackpayOthers", format = "#,##0.00"), TextSearch, Export(false), Required]
        public double BackpayOthers
        {
            get { return m_BackpayOthers; }
            set { m_BackpayOthers = value; modify("BackpayOthers"); }
        }

        protected double m_BackpayUniformTimecard;
        [DBField("BackpayUniformTimecard", format = "#,##0.00"), TextSearch, Export(false), Required]
        public double BackpayUniformTimecard
        {
            get { return m_BackpayUniformTimecard; }
            set { m_BackpayUniformTimecard = value; modify("BackpayUniformTimecard"); }
        }

        protected double m_CNDAmount;
        [DBField("CNDAmount", format = "#,##0.00"), TextSearch, Export(false), Required]
        public double CNDAmount
        {
            get { return m_CNDAmount; }
            set { m_CNDAmount = value; modify("CNDAmount"); }
        }

        protected int m_APPImportBatchID;
        [DBField("APPImportBatchID"), Int, TextSearch, Export(false)]
        public int APPImportBatchID
        {
            get { return m_APPImportBatchID; }
            set { m_APPImportBatchID = value; modify("APPImportBatchID"); }
        }
    }
}
