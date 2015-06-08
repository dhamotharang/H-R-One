using System;
using System.Collections;
using System.Data;
using System.Configuration;
using HROne.DataAccess;
////using perspectivemind.validation;

namespace HROne.Lib.Entities
{
    /// <summary>
    /// Summary description for MPFRecord
    /// </summary>
    [DBClass("MPFRecord")]
    public class EMPFRecord :BaseObject 
    {
        public static DBManager db = new DBManager(typeof(EMPFRecord));
        public static WFValueList VLMPFRecStatus = new AppUtils.NewWFTextList(new string[] { "N", "E", "T" }, new string[] { "New Join", "Existing", "Terminated" });

        protected int m_MPFRecordID;
        [DBField("MPFRecordID", true, true), TextSearch, Export(false)]
        public int MPFRecordID
        {
            get { return m_MPFRecordID; }
            set { m_MPFRecordID = value; modify("MPFRecordID"); }
        }

        protected int m_EmpPayrollID;
        [DBField("EmpPayrollID"), TextSearch, Export(false), Required]
        public int EmpPayrollID
        {
            get { return m_EmpPayrollID; }
            set { m_EmpPayrollID = value; modify("EmpPayrollID"); }
        }

        protected int m_MPFPlanID;
        [DBField("MPFPlanID"), TextSearch, Export(false), Required]
        public int MPFPlanID
        {
            get { return m_MPFPlanID; }
            set { m_MPFPlanID = value; modify("MPFPlanID"); }
        }

        protected int m_AVCPlanID;
        [DBField("AVCPlanID"), TextSearch, Export(false), Required]
        public int AVCPlanID
        {
            get { return m_AVCPlanID; }
            set { m_AVCPlanID = value; modify("AVCPlanID"); }
        }
        

        protected DateTime m_MPFRecPeriodFr;
        [DBField("MPFRecPeriodFr"), TextSearch, Export(false), Required]
        public DateTime MPFRecPeriodFr
        {
            get { return m_MPFRecPeriodFr; }
            set { m_MPFRecPeriodFr = value; modify("MPFRecPeriodFr"); }
        }

        protected DateTime m_MPFRecPeriodTo;
        [DBField("MPFRecPeriodTo"), TextSearch, Export(false), Required]
        public DateTime MPFRecPeriodTo
        {
            get { return m_MPFRecPeriodTo; }
            set { m_MPFRecPeriodTo = value; modify("MPFRecPeriodTo"); }
        }

        protected string m_MPFRecType;
        [DBField("MPFRecType"), TextSearch, Export(false), Required]
        public string MPFRecType
        {
            get { return m_MPFRecType; }
            set { m_MPFRecType = value; modify("MPFRecType"); }
        }

        protected double m_MPFRecCalMCRI;
        [DBField("MPFRecCalMCRI", "0.00"), TextSearch, Export(false), Required]
        public double MPFRecCalMCRI
        {
            get { return m_MPFRecCalMCRI; }
            set { m_MPFRecCalMCRI = value; modify("MPFRecCalMCRI"); }
        }

        protected double m_MPFRecCalMCER;
        [DBField("MPFRecCalMCER", "0.00"), TextSearch, Export(false), Required]
        public double MPFRecCalMCER
        {
            get { return m_MPFRecCalMCER; }
            set { m_MPFRecCalMCER = value; modify("MPFRecCalMCER"); }
        }

        protected double m_MPFRecCalMCEE;
        [DBField("MPFRecCalMCEE", "0.00"), TextSearch, Export(false), Required]
        public double MPFRecCalMCEE
        {
            get { return m_MPFRecCalMCEE; }
            set { m_MPFRecCalMCEE = value; modify("MPFRecCalMCEE"); }
        }

        protected double m_MPFRecCalVCRI;
        [DBField("MPFRecCalVCRI", "0.00"), TextSearch, Export(false), Required]
        public double MPFRecCalVCRI
        {
            get { return m_MPFRecCalVCRI; }
            set { m_MPFRecCalVCRI = value; modify("MPFRecCalVCRI"); }
        }

        protected double m_MPFRecCalVCER;
        [DBField("MPFRecCalVCER", "0.00"), TextSearch, Export(false), Required]
        public double MPFRecCalVCER
        {
            get { return m_MPFRecCalVCER; }
            set { m_MPFRecCalVCER = value; modify("MPFRecCalVCER"); }
        }

        protected double m_MPFRecCalVCEE;
        [DBField("MPFRecCalVCEE", "0.00"), TextSearch, Export(false), Required]
        public double MPFRecCalVCEE
        {
            get { return m_MPFRecCalVCEE; }
            set { m_MPFRecCalVCEE = value; modify("MPFRecCalVCEE"); }
        }

        protected double m_MPFRecActMCRI;
        [DBField("MPFRecActMCRI", "0.00"), TextSearch, Export(false), Required]
        public double MPFRecActMCRI
        {
            get { return m_MPFRecActMCRI; }
            set { m_MPFRecActMCRI = value; modify("MPFRecActMCRI"); }
        }

        protected double m_MPFRecActMCER;
        [DBField("MPFRecActMCER", "0.00"), TextSearch, Export(false), Required]
        public double MPFRecActMCER
        {
            get { return m_MPFRecActMCER; }
            set { m_MPFRecActMCER = value; modify("MPFRecActMCER"); }
        }

        protected double m_MPFRecActMCEE;
        [DBField("MPFRecActMCEE", "0.00"), TextSearch, Export(false), Required]
        public double MPFRecActMCEE
        {
            get { return m_MPFRecActMCEE; }
            set { m_MPFRecActMCEE = value; modify("MPFRecActMCEE"); }
        }

        protected double m_MPFRecActVCRI;
        [DBField("MPFRecActVCRI", "0.00"), TextSearch, Export(false), Required]
        public double MPFRecActVCRI
        {
            get { return m_MPFRecActVCRI; }
            set { m_MPFRecActVCRI = value; modify("MPFRecActVCRI"); }
        }

        protected double m_MPFRecActVCER;
        [DBField("MPFRecActVCER", "0.00"), TextSearch, Export(false), Required]
        public double MPFRecActVCER
        {
            get { return m_MPFRecActVCER; }
            set { m_MPFRecActVCER = value; modify("MPFRecActVCER"); }
        }

        protected double m_MPFRecActVCEE;
        [DBField("MPFRecActVCEE", "0.00"), TextSearch, Export(false), Required]
        public double MPFRecActVCEE
        {
            get { return m_MPFRecActVCEE; }
            set { m_MPFRecActVCEE = value; modify("MPFRecActVCEE"); }
        }

        protected int m_CostCenterID;
        [DBField("CostCenterID"), TextSearch, Export(false)]
        public int CostCenterID
        {
            get { return m_CostCenterID; }
            set { m_CostCenterID = value; modify("CostCenterID"); }
        }

        public bool CanConsiderAsFirstContribution(DatabaseConnection dbConn)
        {
            EEmpPayroll empPayroll = new EEmpPayroll();
            empPayroll.EmpPayrollID = EmpPayrollID;

            if (EEmpPayroll.db.select(dbConn, empPayroll))
            {

                // Check if the record is first contribution object
                DBFilter previousEMPPayrollFilter = new DBFilter();
                previousEMPPayrollFilter.add(new Match("EmpID", empPayroll.EmpID));


                DBFilter previousMPFRecordFilter = new DBFilter();
                previousMPFRecordFilter.add(new IN("EmpPayrollID", "Select EmpPayrollID from EmpPayroll", previousEMPPayrollFilter));
                previousMPFRecordFilter.add(new Match("MPFRecPeriodTo", "<", MPFRecPeriodTo));
                previousMPFRecordFilter.add(new Match("EmpPayrollID", "<>", EmpPayrollID));
                if (EMPFRecord.db.count(dbConn, previousMPFRecordFilter) <= 0)
                {
                    DBFilter empMPFPlanFilter = new DBFilter();
                    empMPFPlanFilter.add(new Match("EmpID", "=", empPayroll.EmpID));
                    empMPFPlanFilter.add(new Match("EmpMPFEffFr", "<=", MPFRecPeriodTo));
                    empMPFPlanFilter.add("EmpMPFEffFr", true);

                    ArrayList empMPFPlanList = EEmpMPFPlan.db.select(dbConn, empMPFPlanFilter);

                    if (empMPFPlanList.Count > 0)
                    {
                        EEmpMPFPlan firstEMPMPFPlan = (EEmpMPFPlan)empMPFPlanList[0];
                        if (firstEMPMPFPlan.EmpMPFEffFr >= MPFRecPeriodFr && firstEMPMPFPlan.EmpMPFEffFr <= MPFRecPeriodTo)
                        {
                            EEmpMPFPlan lastEMPMPFPlan = (EEmpMPFPlan)empMPFPlanList[empMPFPlanList.Count - 1];
                            if (lastEMPMPFPlan.MPFPlanID.Equals(MPFPlanID))
                                return true;

                        }
                    }
                }
            }
            return false;
        }
    }
}