using System;
using System.Collections.Generic;
using System.Text;
using HROne.DataAccess;
////using perspectivemind.validation;

namespace HROne.Lib.Entities
{
    [DBClass("MPFParameter")]
    public class EMPFParameter : BaseObject
    {
        public static DBManager db = new DBManager(typeof(EMPFParameter));

        protected int m_MPFParamID;
        [DBField("MPFParamID", true, true), TextSearch, Export(false)]
        public int MPFParamID
        {
            get { return m_MPFParamID; }
            set { m_MPFParamID = value; modify("MPFParamID"); }
        }
        protected DateTime m_MPFParamEffFr;
        [DBField("MPFParamEffFr"), TextSearch, Export(false), Required]
        public DateTime MPFParamEffFr
        {
            get { return m_MPFParamEffFr; }
            set { m_MPFParamEffFr = value; modify("MPFParamEffFr"); }
        }
        protected double m_MPFParamMinMonthly;
        [DBField("MPFParamMinMonthly"), TextSearch, Export(false), Required]
        public double MPFParamMinMonthly
        {
            get { return m_MPFParamMinMonthly; }
            set { m_MPFParamMinMonthly = value; modify("MPFParamMinMonthly"); }
        }
        protected double m_MPFParamMaxMonthly;
        [DBField("MPFParamMaxMonthly"), TextSearch, Export(false), Required]
        public double MPFParamMaxMonthly
        {
            get { return m_MPFParamMaxMonthly; }
            set { m_MPFParamMaxMonthly = value; modify("MPFParamMaxMonthly"); }
        }
        protected double m_MPFParamMinDaily;
        [DBField("MPFParamMinDaily"), TextSearch, Export(false), Required]
        public double MPFParamMinDaily
        {
            get { return m_MPFParamMinDaily; }
            set { m_MPFParamMinDaily = value; modify("MPFParamMinDaily"); }
        }
        protected int m_MPFParamMaxDaily;
        [DBField("MPFParamMaxDaily"), TextSearch, Export(false), Required]
        public int MPFParamMaxDaily
        {
            get { return m_MPFParamMaxDaily; }
            set { m_MPFParamMaxDaily = value; modify("MPFParamMaxDaily"); }
        }
        protected double m_MPFParamEEPercent;
        [DBField("MPFParamEEPercent"), TextSearch, Export(false), Required]
        public double  MPFParamEEPercent
        {
            get { return m_MPFParamEEPercent; }
            set { m_MPFParamEEPercent = value; modify("MPFParamEEPercent"); }
        }
        protected double m_MPFParamERPercent;
        [DBField("MPFParamERPercent"), TextSearch, Export(false), Required]
        public double MPFParamERPercent
        {
            get { return m_MPFParamERPercent; }
            set { m_MPFParamERPercent = value; modify("MPFParamERPercent"); }
        }
    }
}
