using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Data;
using HROne.DataAccess;
using HROne.Common;
namespace HROne.Lib.Entities
{
    [DBClass("EmpBonusProcess")]
    public class EEmpBonusProcess : BaseObjectWithRecordInfo
    {
        public const string TYPE_STANDARD = "S";
        public const string TYPE_DISCRETIONARY = "D";

        public static DBManager db = new DBManagerWithRecordInfo(typeof(EEmpBonusProcess));

        public static EEmpBonusProcess GetObject(DatabaseConnection dbConn, int ID)
        {
            EEmpBonusProcess m_object = new EEmpBonusProcess();
            m_object.EmpBonusProcessID = ID;
            if (EEmpBonusProcess.db.select(dbConn, m_object))
                return m_object;
            return null;
        }

        protected int m_EmpBonusProcessID;
        [DBField("EmpBonusProcessID", true, true), TextSearch, Export(false)]
        public int EmpBonusProcessID
        {
            get { return m_EmpBonusProcessID; }
            set { m_EmpBonusProcessID = value; modify("EmpBonusProcessID"); }
        }

        protected int m_EmpID;
        [DBField("EmpID"), Int, TextSearch, Export(false)]
        public int EmpID
        {
            get { return m_EmpID; }
            set { m_EmpID = value; modify("EmpID"); }
        }

        protected int m_BonusProcessID;
        [DBField("BonusProcessID"), Int, TextSearch, Export(false)]
        public int BonusProcessID
        {
            get { return m_BonusProcessID; }
            set { m_BonusProcessID = value; modify("BonusProcessID"); }
        }

        protected string m_EmpBonusProcessType;
        [DBField("EmpBonusProcessType"), TextSearch, Export(false)]
        public string EmpBonusProcessType
        {
            get { return m_EmpBonusProcessType; }
            set { m_EmpBonusProcessType = value; modify("EmpBonusProcessType"); }
        }

        protected string m_EmpBonusProcessRank;
        [DBField("EmpBonusProcessRank"), TextSearch, Export(false)]
        public string EmpBonusProcessRank
        {
            get { return m_EmpBonusProcessRank; }
            set { m_EmpBonusProcessRank = value; modify("EmpBonusProcessRank"); }
        }

        protected double m_EmpBonusProcessTargetSalary;
        [DBField("EmpBonusProcessTargetSalary", format="#,##0.00"), TextSearch, Export(false)]
        public double EmpBonusProcessTargetSalary
        {
            get { return m_EmpBonusProcessTargetSalary; }
            set { m_EmpBonusProcessTargetSalary = value; modify("EmpBonusProcessTargetSalary"); }
        }

        protected double m_EmpBonusProcessBonusAmount;
        [DBField("EmpBonusProcessBonusAmount", format = "#,##0.00"), TextSearch, Export(false)]
        public double EmpBonusProcessBonusAmount
        {
            get { return m_EmpBonusProcessBonusAmount; }
            set { m_EmpBonusProcessBonusAmount = value; modify("EmpBonusProcessBonusAmount"); }
        }

        protected double m_EmpBonusProcessBonusProportion;
        [DBField("EmpBonusProcessBonusProportion", format = "0.0000"), TextSearch, Export(false)]
        public double EmpBonusProcessBonusProportion
        {
            get { return m_EmpBonusProcessBonusProportion; }
            set { m_EmpBonusProcessBonusProportion = value; modify("EmpBonusProcessBonusProportion"); }
        }
        
        
    }
}
