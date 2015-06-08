using System;
using System.Collections.Generic;
using System.Text;
using HROne.DataAccess;
////using perspectivemind.validation;
using HROne.Common;

namespace HROne.Lib.Entities
{
    [DBClass("ALProrataRoundingRuleDetail")]
    public class EALProrataRoundingRuleDetail : BaseObject
    {
        public static DBManager db = new DBManager(typeof(EALProrataRoundingRuleDetail));

        protected int m_ALProrataRoundingRuleDetailID;
        [DBField("ALProrataRoundingRuleDetailID", true, true), TextSearch, Export(false)]
        public int ALProrataRoundingRuleDetailID
        {
            get { return m_ALProrataRoundingRuleDetailID; }
            set { m_ALProrataRoundingRuleDetailID = value; modify("ALProrataRoundingRuleDetailID"); }
        }

        protected int m_ALProrataRoundingRuleID;
        [DBField("ALProrataRoundingRuleID"), TextSearch, Export(false)]
        public int ALProrataRoundingRuleID
        {
            get { return m_ALProrataRoundingRuleID; }
            set { m_ALProrataRoundingRuleID = value; modify("ALProrataRoundingRuleID"); }
        }

        protected double m_ALProrataRoundingRuleDetailRangeTo;
        [DBField("ALProrataRoundingRuleDetailRangeTo", "0.####"), MaxLength(6), Required]
        public double ALProrataRoundingRuleDetailRangeTo
        {
            get { return m_ALProrataRoundingRuleDetailRangeTo; }
            set { m_ALProrataRoundingRuleDetailRangeTo = value; modify("ALProrataRoundingRuleDetailRangeTo"); }
        }
        protected double m_ALProrataRoundingRuleDetailRoundTo;
        [DBField("ALProrataRoundingRuleDetailRoundTo", "0.####"), MaxLength(6), Required]
        public double ALProrataRoundingRuleDetailRoundTo
        {
            get { return m_ALProrataRoundingRuleDetailRoundTo; }
            set { m_ALProrataRoundingRuleDetailRoundTo = value; modify("ALProrataRoundingRuleDetailRoundTo"); }
        }

    }
}
