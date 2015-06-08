using System;
using System.Collections.Generic;
using System.Text;
using HROne.DataAccess;
////using perspectivemind.validation;

namespace HROne.Lib.Entities
{
    [DBClass("EmpCostCenterDetail")]
    public class EEmpCostCenterDetail : BaseObject
    {
        public static DBManager db = new DBManager(typeof(EEmpCostCenterDetail));
        protected int m_EmpCostCenterDetailID;
        [DBField("EmpCostCenterDetailID", true, true), TextSearch, Export(false)]
        public int EmpCostCenterDetailID
        {
            get { return m_EmpCostCenterDetailID; }
            set { m_EmpCostCenterDetailID = value; modify("EmpCostCenterDetailID"); }
        }
        protected int m_EmpCostCenterID;
        [DBField("EmpCostCenterID"), TextSearch, Export(false)]
        public int EmpCostCenterID
        {
            get { return m_EmpCostCenterID; }
            set { m_EmpCostCenterID = value; modify("EmpCostCenterID"); }
        }

        protected int m_CostCenterID;
        [DBField("CostCenterID"), TextSearch, Export(false)]
        public int CostCenterID
        {
            get { return m_CostCenterID; }
            set { m_CostCenterID = value; modify("CostCenterID"); }
        }

        protected double m_EmpCostCenterPercentage;
        [DBField("EmpCostCenterPercentage","0.00"), TextSearch, Export(false)]
        public double EmpCostCenterPercentage
        {
            get { return m_EmpCostCenterPercentage; }
            set { m_EmpCostCenterPercentage = value; modify("EmpCostCenterPercentage"); }
        }
    }
}
