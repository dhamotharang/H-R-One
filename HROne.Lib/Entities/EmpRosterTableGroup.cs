using System;
using System.Collections.Generic;
using System.Text;
using HROne.DataAccess;
////using perspectivemind.validation;

namespace HROne.Lib.Entities
{
    [DBClass("EmpRosterTableGroup")]
    public class EEmpRosterTableGroup : BaseObjectWithRecordInfo
    {
        public static DBManager db = new DBManagerWithRecordInfo(typeof(EEmpRosterTableGroup));
        protected int m_EmpRosterTableGroupID;
        [DBField("EmpRosterTableGroupID", true, true), TextSearch, Export(false)]
        public int EmpRosterTableGroupID
        {
            get { return m_EmpRosterTableGroupID; }
            set { m_EmpRosterTableGroupID = value; modify("EmpRosterTableGroupID"); }
        }
        protected int m_EmpID;
        [DBField("EmpID"), TextSearch, Export(false)]
        public int EmpID
        {
            get { return m_EmpID; }
            set { m_EmpID = value; modify("EmpID"); }
        }
        protected DateTime m_EmpRosterTableGroupEffFr;
        [DBField("EmpRosterTableGroupEffFr"), TextSearch, MaxLength(25), Export(false), Required]
        public DateTime EmpRosterTableGroupEffFr
        {
            get { return m_EmpRosterTableGroupEffFr; }
            set { m_EmpRosterTableGroupEffFr = value; modify("EmpRosterTableGroupEffFr"); }
        }
        protected DateTime m_EmpRosterTableGroupEffTo;
        [DBField("EmpRosterTableGroupEffTo"), TextSearch, MaxLength(25), Export(false)]
        public DateTime EmpRosterTableGroupEffTo
        {
            get { return m_EmpRosterTableGroupEffTo; }
            set { m_EmpRosterTableGroupEffTo = value; modify("EmpRosterTableGroupEffTo"); }
        }
        protected int m_RosterTableGroupID;
        [DBField("RosterTableGroupID"), TextSearch, Export(false), Required]
        public int RosterTableGroupID
        {
            get { return m_RosterTableGroupID; }
            set { m_RosterTableGroupID = value; modify("RosterTableGroupID"); }
        }
        protected bool m_EmpRosterTableGroupIsSupervisor;
        [DBField("EmpRosterTableGroupIsSupervisor"), TextSearch, Export(false), Required]
        public bool EmpRosterTableGroupIsSupervisor
        {
            get { return m_EmpRosterTableGroupIsSupervisor; }
            set { m_EmpRosterTableGroupIsSupervisor = value; modify("EmpRosterTableGroupIsSupervisor"); }
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
