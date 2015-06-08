using System;
using System.Collections;
using System.Text;
using HROne.DataAccess;
////using perspectivemind.validation;
using HROne.Common;

namespace HROne.Lib.Entities
{
    [DBClass("ALProrataRoundingRule")]
    public class EALProrataRoundingRule : BaseObject
    {
        public static DBManager db = new DBManager(typeof(EALProrataRoundingRule));
        public static WFValueList VLALProrataRoundingRule = new AppUtils.EncryptedDBCodeList(EALProrataRoundingRule.db, "ALProrataRoundingRuleID", new string[] { "ALProrataRoundingRuleCode", "ALProrataRoundingRuleDesc" }, " - ", "ALProrataRoundingRuleCode");

        protected int m_ALProrataRoundingRuleID;
        [DBField("ALProrataRoundingRuleID", true, true), TextSearch, Export(false)]
        public int ALProrataRoundingRuleID
        {
            get { return m_ALProrataRoundingRuleID; }
            set { m_ALProrataRoundingRuleID = value; modify("ALProrataRoundingRuleID"); }
        }
        protected string m_ALProrataRoundingRuleCode;
        [DBField("ALProrataRoundingRuleCode"), TextSearch, DBAESEncryptStringField, MaxLength(20, 20), Export(false), Required]
        public string ALProrataRoundingRuleCode
        {
            get { return m_ALProrataRoundingRuleCode; }
            set { m_ALProrataRoundingRuleCode = value; modify("ALProrataRoundingRuleCode"); }
        }
        protected string m_ALProrataRoundingRuleDesc;
        [DBField("ALProrataRoundingRuleDesc"), TextSearch, DBAESEncryptStringField, MaxLength(100, 100), Export(false), Required]
        public string ALProrataRoundingRuleDesc
        {
            get { return m_ALProrataRoundingRuleDesc; }
            set { m_ALProrataRoundingRuleDesc = value; modify("ALProrataRoundingRuleDesc"); }
        }

        public double Rounding(DatabaseConnection dbConn, double number)
        {
            double floatingPointNumber = number - Math.Truncate(number);
            if (floatingPointNumber == 0)
                return number;
            DBFilter filter = new DBFilter();
            filter.add(new Match("ALProrataRoundingRuleID", ALProrataRoundingRuleID));
            filter.add("ALProrataRoundingRuleDetailRangeTo",true);
            ArrayList roundingDetailList = EALProrataRoundingRuleDetail.db.select(dbConn, filter);
            foreach (EALProrataRoundingRuleDetail roundingDetail in roundingDetailList)
            {
                if (floatingPointNumber <= roundingDetail.ALProrataRoundingRuleDetailRangeTo)
                    return Math.Truncate(number) + roundingDetail.ALProrataRoundingRuleDetailRoundTo;
            }
            return number;
        }
    }
}
