using System;
using System.Collections.Generic;
using System.Text;
using HROne.DataAccess;
////using perspectivemind.validation;

namespace HROne.Lib.Entities
{
    [DBClass("ORSOPlanDetail")]
    public class EORSOPlanDetail : BaseObject
    {
        public static DBManager db = new DBManager(typeof(EORSOPlanDetail));
        public static WFValueList VLORSOPlanDetail = new WFDBList(EORSOPlanDetail.db, "ORSOPlanDetailID", "ORSOPlanID", "ORSOPlanDetailYearOfService");

        protected int m_ORSOPlanDetailID;
        [DBField("ORSOPlanDetailID", true, true), TextSearch, Export(false)]
        public int ORSOPlanDetailID
        {
            get { return m_ORSOPlanDetailID; }
            set { m_ORSOPlanDetailID = value; modify("ORSOPlanDetailID"); }
        }
        protected int m_ORSOPlanID;
        [DBField("ORSOPlanID"), TextSearch, Export(false)]
        public int ORSOPlanID
        {
            get { return m_ORSOPlanID; }
            set { m_ORSOPlanID = value; modify("ORSOPlanID"); }
        }
        protected int m_ORSOPlanDetailYearOfService;
        [DBField("ORSOPlanDetailYearOfService"), TextSearch, Export(false), Required]
        public int ORSOPlanDetailYearOfService
        {
            get { return m_ORSOPlanDetailYearOfService; }
            set { m_ORSOPlanDetailYearOfService = value; modify("ORSOPlanDetailYearOfService"); }
        }
        protected double m_ORSOPlanDetailER;
        [DBField("ORSOPlanDetailER", "0.00"), TextSearch, Export(false), Required]
        public double ORSOPlanDetailER
        {
            get { return m_ORSOPlanDetailER; }
            set { m_ORSOPlanDetailER = value; modify("ORSOPlanDetailER"); }
        }
        protected double m_ORSOPlanDetailERFix;
        [DBField("ORSOPlanDetailERFix", "0.00"), TextSearch, Export(false), Required]
        public double ORSOPlanDetailERFix
        {
            get { return m_ORSOPlanDetailERFix; }
            set { m_ORSOPlanDetailERFix = value; modify("ORSOPlanDetailERFix"); }
        }
        protected double m_ORSOPlanDetailEE;
        [DBField("ORSOPlanDetailEE", "0.00"), TextSearch, Export(false), Required]
        public double ORSOPlanDetailEE
        {
            get { return m_ORSOPlanDetailEE; }
            set { m_ORSOPlanDetailEE = value; modify("ORSOPlanDetailEE"); }
        }
        protected double m_ORSOPlanDetailEEFix;
        [DBField("ORSOPlanDetailEEFix", "0.00"), TextSearch, Export(false), Required]
        public double ORSOPlanDetailEEFix
        {
            get { return m_ORSOPlanDetailEEFix; }
            set { m_ORSOPlanDetailEEFix = value; modify("ORSOPlanDetailEEFix"); }
        }
    }
}
