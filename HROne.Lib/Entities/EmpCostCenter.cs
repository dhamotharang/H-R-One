using System;
using System.Collections.Generic;
using System.Text;
using HROne.DataAccess;
////using perspectivemind.validation;

namespace HROne.Lib.Entities
{
    [DBClass("EmpCostCenter")]
    public class EEmpCostCenter : BaseObjectWithRecordInfo
    {
        public static DBManager db = new DBManagerWithRecordInfo(typeof(EEmpCostCenter));
        protected int m_EmpCostCenterID;
        [DBField("EmpCostCenterID", true, true), TextSearch, Export(false)]
        public int EmpCostCenterID
        {
            get { return m_EmpCostCenterID; }
            set { m_EmpCostCenterID = value; modify("EmpCostCenterID"); }
        }
        protected int m_EmpID;
        [DBField("EmpID"), TextSearch, Export(false)]
        public int EmpID
        {
            get { return m_EmpID; }
            set { m_EmpID = value; modify("EmpID"); }
        }
        protected DateTime m_EmpCostCenterEffFr;
        [DBField("EmpCostCenterEffFr"), TextSearch, MaxLength(25), Export(false), Required]
        public DateTime EmpCostCenterEffFr
        {
            get { return m_EmpCostCenterEffFr; }
            set { m_EmpCostCenterEffFr = value; modify("EmpCostCenterEffFr"); }
        }       
        protected DateTime m_EmpCostCenterEffTo;
        [DBField("EmpCostCenterEffTo"), TextSearch, MaxLength(25), Export(false)]
        public DateTime EmpCostCenterEffTo
        {
            get { return m_EmpCostCenterEffTo; }
            set { m_EmpCostCenterEffTo = value; modify("EmpCostCenterEffTo"); }
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
