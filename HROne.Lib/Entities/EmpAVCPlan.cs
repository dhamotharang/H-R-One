using System;
using System.Collections.Generic;
using System.Text;
using HROne.DataAccess;
////using perspectivemind.validation;

namespace HROne.Lib.Entities
{
    [DBClass("EmpAVCPlan")]
    public class EEmpAVCPlan : BaseObjectWithRecordInfo
    {
        public static DBManager db = new DBManagerWithRecordInfo(typeof(EEmpAVCPlan));
        protected int m_EmpAVCID;
        [DBField("EmpAVCID", true, true), TextSearch, Export(false)]
        public int EmpAVCID
        {
            get { return m_EmpAVCID; }
            set { m_EmpAVCID = value; modify("EmpAVCID"); }
        }
        protected int m_EmpID;
        [DBField("EmpID"), TextSearch, Export(false)]
        public int EmpID
        {
            get { return m_EmpID; }
            set { m_EmpID = value; modify("EmpID"); }
        }
        protected DateTime m_EmpAVCEffFr;
        [DBField("EmpAVCEffFr"), TextSearch, MaxLength(25), Export(false),Required]
        public DateTime EmpAVCEffFr
        {
            get { return m_EmpAVCEffFr; }
            set { m_EmpAVCEffFr = value; modify("EmpAVCEffFr"); }
        }       
        protected DateTime m_EmpAVCEffTo;
        [DBField("EmpAVCEffTo"), TextSearch, MaxLength(25), Export(false)]
        public DateTime EmpAVCEffTo
        {
            get { return m_EmpAVCEffTo; }
            set { m_EmpAVCEffTo = value; modify("EmpAVCEffTo"); }
        }       
        protected int m_AVCPlanID;
        [DBField("AVCPlanID"), TextSearch, Export(false),Required]
        public int AVCPlanID
        {
            get { return m_AVCPlanID; }
            set { m_AVCPlanID = value; modify("AVCPlanID"); }
        }
        protected bool m_EmpAVCEROverrideSetting;
        [DBField("EmpAVCEROverrideSetting"), TextSearch, Export(false)]
        public bool EmpAVCEROverrideSetting
        {
            get { return m_EmpAVCEROverrideSetting; }
            set { m_EmpAVCEROverrideSetting = value; modify("EmpAVCEROverrideSetting"); }
        }

        protected double m_EmpAVCERBelowRI;
        [DBField("EmpAVCERBelowRI", "0.00"), MaxLength(6), TextSearch, Export(false)]
        public double EmpAVCERBelowRI
        {
            get { return m_EmpAVCERBelowRI; }
            set { m_EmpAVCERBelowRI = value; modify("EmpAVCERBelowRI"); }
        }
        protected double m_EmpAVCERAboveRI;
        [DBField("EmpAVCERAboveRI", "0.00"), MaxLength(6), TextSearch, Export(false)]
        public double EmpAVCERAboveRI
        {
            get { return m_EmpAVCERAboveRI; }
            set { m_EmpAVCERAboveRI = value; modify("EmpAVCERAboveRI"); }
        }
        protected double m_EmpAVCERFix;
        [DBField("EmpAVCERFix", "0.00"), MaxLength(11), TextSearch, Export(false)]
        public double EmpAVCERFix
        {
            get { return m_EmpAVCERFix; }
            set { m_EmpAVCERFix = value; modify("EmpAVCERFix"); }
        }

        protected bool m_EmpAVCEEOverrideSetting;
        [DBField("EmpAVCEEOverrideSetting"), TextSearch, Export(false)]
        public bool EmpAVCEEOverrideSetting
        {
            get { return m_EmpAVCEEOverrideSetting; }
            set { m_EmpAVCEEOverrideSetting = value; modify("EmpAVCEEOverrideSetting"); }
        }
        protected double m_EmpAVCEEBelowRI;
        [DBField("EmpAVCEEBelowRI", "0.00"), MaxLength(6), TextSearch, Export(false)]
        public double EmpAVCEEBelowRI
        {
            get { return m_EmpAVCEEBelowRI; }
            set { m_EmpAVCEEBelowRI = value; modify("EmpAVCEEBelowRI"); }
        }
        protected double m_EmpAVCEEAboveRI;
        [DBField("EmpAVCEEAboveRI", "0.00"), MaxLength(6), TextSearch, Export(false)]
        public double EmpAVCEEAboveRI
        {
            get { return m_EmpAVCEEAboveRI; }
            set { m_EmpAVCEEAboveRI = value; modify("EmpAVCEEAboveRI"); }
        }
        protected double m_EmpAVCEEFix;
        [DBField("EmpAVCEEFix", "0.00"), MaxLength(11), TextSearch, Export(false)]
        public double EmpAVCEEFix
        {
            get { return m_EmpAVCEEFix; }
            set { m_EmpAVCEEFix = value; modify("EmpAVCEEFix"); }
        }
        protected int m_DefaultMPFPlanID;
        [DBField("DefaultMPFPlanID"), TextSearch, Export(false), Required]
        public int DefaultMPFPlanID
        {
            get { return m_DefaultMPFPlanID; }
            set { m_DefaultMPFPlanID = value; modify("DefaultMPFPlanID"); }
        }

        protected string m_EmpAVCPlanExtendData;
        [DBField("EmpAVCPlanExtendData"), TextSearch, Export(false)]
        public string EmpAVCPlanExtendData
        {
            get { return m_EmpAVCPlanExtendData; }
            set { m_EmpAVCPlanExtendData = value; modify("EmpAVCPlanExtendData"); }
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
