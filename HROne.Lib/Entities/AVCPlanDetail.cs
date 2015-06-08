using System;
using System.Collections.Generic;
using System.Text;
using HROne.DataAccess;
////using perspectivemind.validation;

namespace HROne.Lib.Entities
{
    [DBClass("AVCPlanDetail")]
    public class EAVCPlanDetail : BaseObject
    {
        public static DBManager db = new DBManager(typeof(EAVCPlanDetail));
        public static WFValueList VLAVCPlanDetail = new WFDBList(EAVCPlanDetail.db, "AVCPlanDetailID", "AVCPlanID", "AVCPlanDetailYearOfService");

        protected int m_AVCPlanDetailID;
        [DBField("AVCPlanDetailID", true, true), TextSearch, Export(false)]
        public int AVCPlanDetailID
        {
            get { return m_AVCPlanDetailID; }
            set { m_AVCPlanDetailID = value; modify("AVCPlanDetailID"); }
        }
        protected int m_AVCPlanID;
        [DBField("AVCPlanID"), TextSearch, Export(false)]
        public int AVCPlanID
        {
            get { return m_AVCPlanID; }
            set { m_AVCPlanID = value; modify("AVCPlanID"); }
        }
        protected int m_AVCPlanDetailYearOfService;
        [DBField("AVCPlanDetailYearOfService"), MaxLength(2), TextSearch, Export(false), Required]
        public int AVCPlanDetailYearOfService
        {
            get { return m_AVCPlanDetailYearOfService; }
            set { m_AVCPlanDetailYearOfService = value; modify("AVCPlanDetailYearOfService"); }
        }
        protected double m_AVCPlanDetailERBelowRI;
        [DBField("AVCPlanDetailERBelowRI", "0.00"), MaxLength(6), TextSearch, Export(false), Required]
        public double AVCPlanDetailERBelowRI
        {
            get { return m_AVCPlanDetailERBelowRI; }
            set { m_AVCPlanDetailERBelowRI = value; modify("AVCPlanDetailERBelowRI"); }
        }
        protected double m_AVCPlanDetailERAboveRI;
        [DBField("AVCPlanDetailERAboveRI", "0.00"), MaxLength(6), TextSearch, Export(false), Required]
        public double AVCPlanDetailERAboveRI
        {
            get { return m_AVCPlanDetailERAboveRI; }
            set { m_AVCPlanDetailERAboveRI = value; modify("AVCPlanDetailERAboveRI"); }
        }
        protected double m_AVCPlanDetailERFix;
        [DBField("AVCPlanDetailERFix", "0.00"), MaxLength(11), TextSearch, Export(false), Required]
        public double AVCPlanDetailERFix
        {
            get { return m_AVCPlanDetailERFix; }
            set { m_AVCPlanDetailERFix = value; modify("AVCPlanDetailERFix"); }
        }
        protected double m_AVCPlanDetailEEBelowRI;
        [DBField("AVCPlanDetailEEBelowRI", "0.00"), MaxLength(6), TextSearch, Export(false), Required]
        public double AVCPlanDetailEEBelowRI
        {
            get { return m_AVCPlanDetailEEBelowRI; }
            set { m_AVCPlanDetailEEBelowRI = value; modify("AVCPlanDetailEEBelowRI"); }
        }
        protected double m_AVCPlanDetailEEAboveRI;
        [DBField("AVCPlanDetailEEAboveRI", "0.00"), MaxLength(6), TextSearch, Export(false), Required]
        public double AVCPlanDetailEEAboveRI
        {
            get { return m_AVCPlanDetailEEAboveRI; }
            set { m_AVCPlanDetailEEAboveRI = value; modify("AVCPlanDetailEEAboveRI"); }
        }
        protected double m_AVCPlanDetailEEFix;
        [DBField("AVCPlanDetailEEFix", "0.00"), MaxLength(11), TextSearch, Export(false), Required]
        public double AVCPlanDetailEEFix
        {
            get { return m_AVCPlanDetailEEFix; }
            set { m_AVCPlanDetailEEFix = value; modify("AVCPlanDetailEEFix"); }
        }
    }
}
