using System;
using System.Collections.Generic;
using System.Text;
using HROne.DataAccess;
////using perspectivemind.validation;

namespace HROne.Lib.Entities
{
    [DBClass("RosterCodeDetail")]
    public class ERosterCodeDetail : BaseObject
    {
        public static DBManager db = new DBManager(typeof(ERosterCodeDetail));
        public static WFValueList VLRosterCodeDetail = new WFDBList(ERosterCodeDetail.db, "RosterCodeDetailID", "RosterCodeID", "RosterCodeDetailNoOfHour");

        protected int m_RosterCodeDetailID;
        [DBField("RosterCodeDetailID", true, true), TextSearch, Export(false)]
        public int RosterCodeDetailID
        {
            get { return m_RosterCodeDetailID; }
            set { m_RosterCodeDetailID = value; modify("RosterCodeDetailID"); }
        }
        protected int m_RosterCodeID;
        [DBField("RosterCodeID"), TextSearch, Export(false)]
        public int RosterCodeID
        {
            get { return m_RosterCodeID; }
            set { m_RosterCodeID = value; modify("RosterCodeID"); }
        }
        protected double m_RosterCodeDetailNoOfHour;
        [DBField("RosterCodeDetailNoOfHour", "0.00"), MaxLength(6), TextSearch, Export(false), Required]
        public double RosterCodeDetailNoOfHour
        {
            get { return m_RosterCodeDetailNoOfHour; }
            set { m_RosterCodeDetailNoOfHour = value; modify("RosterCodeDetailNoOfHour"); }
        }
        protected double m_RosterCodeDetailRate;
        [DBField("RosterCodeDetailRate", "0.00"), MaxLength(6), TextSearch, Export(false), Required]
        public double RosterCodeDetailRate
        {
            get { return m_RosterCodeDetailRate; }
            set { m_RosterCodeDetailRate = value; modify("RosterCodeDetailRate"); }
        }
    }
}
