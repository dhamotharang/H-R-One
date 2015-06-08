using System;
using System.Collections.Generic;
using System.Text;
using HROne.DataAccess;
////using perspectivemind.validation;

namespace HROne.Lib.Entities
{
    [DBClass("EmpMPFPlan")]
    public class EEmpMPFPlan : BaseObjectWithRecordInfo
    {
        public static DBManager db = new DBManagerWithRecordInfo(typeof(EEmpMPFPlan));
        protected int m_EmpMPFID;
        [DBField("EmpMPFID", true, true), TextSearch, Export(false)]
        public int EmpMPFID
        {
            get { return m_EmpMPFID; }
            set { m_EmpMPFID = value; modify("EmpMPFID"); }
        }
        protected int m_EmpID;
        [DBField("EmpID"), TextSearch, Export(false)]
        public int EmpID
        {
            get { return m_EmpID; }
            set { m_EmpID = value; modify("EmpID"); }
        }
        protected DateTime m_EmpMPFEffFr;
        [DBField("EmpMPFEffFr"), TextSearch,MaxLength(25), Export(false),Required]
        public DateTime EmpMPFEffFr
        {
            get { return m_EmpMPFEffFr; }
            set { m_EmpMPFEffFr = value; modify("EmpMPFEffFr"); }
        }       
        protected DateTime m_EmpMPFEffTo;
        [DBField("EmpMPFEffTo"), TextSearch, MaxLength(25), Export(false)]
        public DateTime EmpMPFEffTo
        {
            get { return m_EmpMPFEffTo; }
            set { m_EmpMPFEffTo = value; modify("EmpMPFEffTo"); }
        }       
        protected int m_MPFPlanID;
        [DBField("MPFPlanID"), TextSearch, Export(false), Required]
        public int MPFPlanID
        {
            get { return m_MPFPlanID; }
            set { m_MPFPlanID = value; modify("MPFPlanID"); }
        }
        protected string m_EmpMPFPlanExtendData;
        [DBField("EmpMPFPlanExtendData"), TextSearch, Export(false)]
        public string EmpMPFPlanExtendData
        {
            get { return m_EmpMPFPlanExtendData; }
            set { m_EmpMPFPlanExtendData = value; modify("EmpMPFPlanExtendData"); }
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
