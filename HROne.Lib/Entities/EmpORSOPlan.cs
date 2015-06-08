using System;
using System.Collections.Generic;
using System.Text;
using HROne.DataAccess;
////using perspectivemind.validation;

namespace HROne.Lib.Entities
{
    [DBClass("EmpORSOPlan")]
    public class EEmpORSOPlan : BaseObjectWithRecordInfo
    {
        public static DBManager db = new DBManagerWithRecordInfo(typeof(EEmpORSOPlan));
        protected int m_EmpORSOID;
        [DBField("EmpORSOID", true, true), TextSearch, Export(false)]
        public int EmpORSOID
        {
            get { return m_EmpORSOID; }
            set { m_EmpORSOID = value; modify("EmpORSOID"); }
        }
        protected int m_EmpID;
        [DBField("EmpID"), TextSearch, Export(false)]
        public int EmpID
        {
            get { return m_EmpID; }
            set { m_EmpID = value; modify("EmpID"); }
        }
        protected DateTime m_EmpORSOEffFr;
        [DBField("EmpORSOEffFr"), TextSearch, MaxLength(25), Export(false),Required]
        public DateTime EmpORSOEffFr
        {
            get { return m_EmpORSOEffFr; }
            set { m_EmpORSOEffFr = value; modify("EmpORSOEffFr"); }
        }       
        protected DateTime m_EmpORSOEffTo;
        [DBField("EmpORSOEffTo"), TextSearch, MaxLength(25), Export(false)]
        public DateTime EmpORSOEffTo
        {
            get { return m_EmpORSOEffTo; }
            set { m_EmpORSOEffTo = value; modify("EmpORSOEffTo"); }
        }       
        protected int m_ORSOPlanID;
        [DBField("ORSOPlanID"), TextSearch, Export(false),Required]
        public int ORSOPlanID
        {
            get { return m_ORSOPlanID; }
            set { m_ORSOPlanID = value; modify("ORSOPlanID"); }
        }

        //  For Synchronize Use only
        protected string m_SynID;
        [DBField("SynID"), TextSearch, Export(false)]
        public string SynID
        {
            get { return m_SynID; }
            set { m_SynID = value; modify("SynID"); }
        }

    }
}
