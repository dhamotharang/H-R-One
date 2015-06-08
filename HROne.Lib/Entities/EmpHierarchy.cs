using System;
using System.Collections.Generic;
using System.Text;
using HROne.DataAccess;
////using perspectivemind.validation;

namespace HROne.Lib.Entities
{
    [DBClass("EmpHierarchy")]
    public class EEmpHierarchy : BaseObject
    {
        public static DBManager db = new DBManager(typeof(EEmpHierarchy));
        protected int m_EmpHierarchyID;
        [DBField("EmpHierarchyID", true, true), TextSearch, Export(false)]
        public int EmpHierarchyID
        {
            get { return m_EmpHierarchyID; }
            set { m_EmpHierarchyID = value; modify("EmpHierarchyID"); }
        }
        protected int m_EmpID;
        [DBField("EmpID"), TextSearch, Export(false)]
        public int EmpID
        {
            get { return m_EmpID; }
            set { m_EmpID = value; modify("EmpID"); }
        }
        protected int m_EmpPosID;
        [DBField("EmpPosID"), TextSearch, Export(false)]
        public int EmpPosID
        {
            get { return m_EmpPosID; }
            set { m_EmpPosID = value; modify("EmpPosID"); }
        }
        protected int m_HElementID;
        [DBField("HElementID"), TextSearch, Export(false),Required]
        public int HElementID
        {
            get { return m_HElementID; }
            set { m_HElementID = value; modify("HElementID"); }
        }
        protected int m_HLevelID;
        [DBField("HLevelID"), TextSearch, Export(false), Required]
        public int HLevelID
        {
            get { return m_HLevelID; }
            set { m_HLevelID = value; modify("HLevelID"); }
        }
    }
}
