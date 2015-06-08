using System;
using System.Collections;
using System.Text;
using HROne.DataAccess;
////using perspectivemind.validation;

namespace HROne.Lib.Entities
{
    [DBClass("CostAllocation")]
    public class ECostAllocation : BaseObject
    {
        public static DBManager db = new DBManager(typeof(ECostAllocation));

        protected int m_CostAllocationID;
        [DBField("CostAllocationID", true, true), TextSearch, Export(false)]
        public int CostAllocationID
        {
            get { return m_CostAllocationID; }
            set { m_CostAllocationID = value; modify("CostAllocationID"); }
        }

        protected int m_EmpID;
        [DBField("EmpID"), TextSearch, Export(false), Required]
        public int EmpID
        {
            get { return m_EmpID; }
            set { m_EmpID = value; modify("EmpID"); }
        }


        protected int m_EmpPayrollID;
        [DBField("EmpPayrollID"), TextSearch, Export(false), Required]
        public int EmpPayrollID
        {
            get { return m_EmpPayrollID; }
            set { m_EmpPayrollID = value; modify("EmpPayrollID"); }
        }

        protected string m_CostAllocationStatus;
        [DBField("CostAllocationStatus"), TextSearch, Export(false), Required]
        public string CostAllocationStatus
        {
            get { return m_CostAllocationStatus; }
            set { m_CostAllocationStatus = value; modify("CostAllocationStatus"); }
        }

        protected DateTime m_CostAllocationTrialRunDate;
        [DBField("CostAllocationTrialRunDate"), TextSearch, Export(false)]
        public DateTime CostAllocationTrialRunDate
        {
            get { return m_CostAllocationTrialRunDate; }
            set { m_CostAllocationTrialRunDate = value; modify("CostAllocationTrialRunDate"); }
        }

        protected int m_CostAllocationTrialRunBy;
        [DBField("CostAllocationTrialRunBy"), TextSearch, Export(false)]
        public int CostAllocationTrialRunBy
        {
            get { return m_CostAllocationTrialRunBy; }
            set { m_CostAllocationTrialRunBy = value; modify("CostAllocationTrialRunBy"); }
        }

        protected DateTime m_CostAllocationConfirmDate;
        [DBField("CostAllocationConfirmDate"), TextSearch, Export(false)]
        public DateTime CostAllocationConfirmDate
        {
            get { return m_CostAllocationConfirmDate; }
            set { m_CostAllocationConfirmDate = value; modify("CostAllocationConfirmDate"); }
        }

        protected int m_CostAllocationConfirmBy;
        [DBField("CostAllocationConfirmBy"), TextSearch, Export(false)]
        public int CostAllocationConfirmBy
        {
            get { return m_CostAllocationConfirmBy; }
            set { m_CostAllocationConfirmBy = value; modify("CostAllocationConfirmBy"); }
        }

    }

    [DBClass("CostAllocationDetail")]
    public class ECostAllocationDetail : BaseObject
    {
        public static DBManager db = new DBManager(typeof(ECostAllocationDetail));

        protected int m_CostAllocationDetailID;
        [DBField("CostAllocationDetailID", true, true), TextSearch, Export(false)]
        public int CostAllocationDetailID
        {
            get { return m_CostAllocationDetailID; }
            set { m_CostAllocationDetailID = value; modify("CostAllocationDetailID"); }
        }

        protected int m_CostAllocationID;
        [DBField("CostAllocationID"), TextSearch, Export(false), Required]
        public int CostAllocationID
        {
            get { return m_CostAllocationID; }
            set { m_CostAllocationID = value; modify("CostAllocationID"); }
        }

        protected int m_CompanyID;
        [DBField("CompanyID"), TextSearch, Export(false), Required]
        public int CompanyID
        {
            get { return m_CompanyID; }
            set { m_CompanyID = value; modify("CompanyID"); }
        }

        protected int m_CostCenterID;
        [DBField("CostCenterID"), TextSearch, Export(false), Required]
        public int CostCenterID
        {
            get { return m_CostCenterID; }
            set { m_CostCenterID = value; modify("CostCenterID"); }
        }

        protected int m_PaymentCodeID;
        [DBField("PaymentCodeID"), TextSearch, Export(false), Required]
        public int PaymentCodeID
        {
            get { return m_PaymentCodeID; }
            set { m_PaymentCodeID = value; modify("PaymentCodeID"); }
        }

        protected double m_CostAllocationDetailAmount;
        [DBField("CostAllocationDetailAmount", "0.00"), TextSearch, Export(false), Required]
        public double CostAllocationDetailAmount
        {
            get { return m_CostAllocationDetailAmount; }
            set { m_CostAllocationDetailAmount = value; modify("CostAllocationDetailAmount"); }
        }

        public double CostAllocationDetailRoundCalAmount;

        protected int m_PayRecID;
        [DBField("PayRecID"), TextSearch, Export(false), Required]
        public int PayRecID
        {
            get { return m_PayRecID; }
            set { m_PayRecID = value; modify("PayRecID"); }
        }

        protected bool m_CostAllocationDetailIsContribution;
        [DBField("CostAllocationDetailIsContribution"), TextSearch, Export(false)]
        public bool CostAllocationDetailIsContribution
        {
            get { return m_CostAllocationDetailIsContribution; }
            set { m_CostAllocationDetailIsContribution = value; modify("CostAllocationDetailIsContribution"); }
        }
        

        public ArrayList HierarchyElementList;
    }

    [DBClass("CostAllocationDetailHElement")]
    public class ECostAllocationDetailHElement : BaseObject
    {
        public static DBManager db = new DBManager(typeof(ECostAllocationDetailHElement));

        protected int m_CostAllocationDetailHElementID;
        [DBField("CostAllocationDetailHElementID", true, true), TextSearch, Export(false)]
        public int CostAllocationDetailHElementID
        {
            get { return m_CostAllocationDetailHElementID; }
            set { m_CostAllocationDetailHElementID = value; modify("CostAllocationDetailHElementID"); }
        }

        protected int m_CostAllocationDetailID;
        [DBField("CostAllocationDetailID"), TextSearch, Export(false), Required]
        public int CostAllocationDetailID
        {
            get { return m_CostAllocationDetailID; }
            set { m_CostAllocationDetailID = value; modify("CostAllocationDetailID"); }
        }

        protected int m_HElementID;
        [DBField("HElementID"), TextSearch, Export(false), Required]
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

        public ECostAllocationDetail RelatedCostAllocationDetailObject;
    }
}
