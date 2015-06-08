using System;
using System.Data;
using System.Configuration;
using HROne.DataAccess;
////using perspectivemind.validation;

namespace HROne.Lib.Entities
{
    /// <summary>
    /// Summary description for ORSORecord
    /// </summary>
    [DBClass("ORSORecord")]
    public class EORSORecord : BaseObject
    {
        public static DBManager db = new DBManager(typeof(EORSORecord));
        public static WFValueList VLORSORecStatus = new AppUtils.NewWFTextList(new string[] { "N", "E", "T" }, new string[] { "New Join", "Existing", "Terminated" });

        protected int m_ORSORecordID;
        [DBField("ORSORecordID", true, true), TextSearch, Export(false)]
        public int ORSORecordID
        {
            get { return m_ORSORecordID; }
            set { m_ORSORecordID = value; modify("ORSORecordID"); }
        }

        protected int m_EmpPayrollID;
        [DBField("EmpPayrollID"), TextSearch, Export(false), Required]
        public int EmpPayrollID
        {
            get { return m_EmpPayrollID; }
            set { m_EmpPayrollID = value; modify("EmpPayrollID"); }
        }

        protected int m_ORSOPlanID;
        [DBField("ORSOPlanID"), TextSearch, Export(false), Required]
        public int ORSOPlanID
        {
            get { return m_ORSOPlanID; }
            set { m_ORSOPlanID = value; modify("ORSOPlanID"); }
        }



        protected DateTime m_ORSORecPeriodFr;
        [DBField("ORSORecPeriodFr"), TextSearch, Export(false), Required]
        public DateTime ORSORecPeriodFr
        {
            get { return m_ORSORecPeriodFr; }
            set { m_ORSORecPeriodFr = value; modify("ORSORecPeriodFr"); }
        }

        protected DateTime m_ORSORecPeriodTo;
        [DBField("ORSORecPeriodTo"), TextSearch, Export(false), Required]
        public DateTime ORSORecPeriodTo
        {
            get { return m_ORSORecPeriodTo; }
            set { m_ORSORecPeriodTo = value; modify("ORSORecPeriodTo"); }
        }

        protected string m_ORSORecType;
        [DBField("ORSORecType"), TextSearch, Export(false), Required]
        public string ORSORecType
        {
            get { return m_ORSORecType; }
            set { m_ORSORecType = value; modify("ORSORecType"); }
        }

        protected double m_ORSORecCalRI;
        [DBField("ORSORecCalRI", "0.00"), TextSearch, Export(false), Required]
        public double ORSORecCalRI
        {
            get { return m_ORSORecCalRI; }
            set { m_ORSORecCalRI = value; modify("ORSORecCalRI"); }
        }

        protected double m_ORSORecCalER;
        [DBField("ORSORecCalER", "0.00"), TextSearch, Export(false), Required]
        public double ORSORecCalER
        {
            get { return m_ORSORecCalER; }
            set { m_ORSORecCalER = value; modify("ORSORecCalER"); }
        }

        protected double m_ORSORecCalEE;
        [DBField("ORSORecCalEE", "0.00"), TextSearch, Export(false), Required]
        public double ORSORecCalEE
        {
            get { return m_ORSORecCalEE; }
            set { m_ORSORecCalEE = value; modify("ORSORecCalEE"); }
        }


        protected double m_ORSORecActRI;
        [DBField("ORSORecActRI", "0.00"), TextSearch, Export(false), Required]
        public double ORSORecActRI
        {
            get { return m_ORSORecActRI; }
            set { m_ORSORecActRI = value; modify("ORSORecActRI"); }
        }

        protected double m_ORSORecActER;
        [DBField("ORSORecActER", "0.00"), TextSearch, Export(false), Required]
        public double ORSORecActER
        {
            get { return m_ORSORecActER; }
            set { m_ORSORecActER = value; modify("ORSORecActER"); }
        }

        protected double m_ORSORecActEE;
        [DBField("ORSORecActEE", "0.00"), TextSearch, Export(false), Required]
        public double ORSORecActEE
        {
            get { return m_ORSORecActEE; }
            set { m_ORSORecActEE = value; modify("ORSORecActEE"); }
        }

        protected int m_CostCenterID;
        [DBField("CostCenterID"), TextSearch, Export(false)]
        public int CostCenterID
        {
            get { return m_CostCenterID; }
            set { m_CostCenterID = value; modify("CostCenterID"); }
        }        
    }
}