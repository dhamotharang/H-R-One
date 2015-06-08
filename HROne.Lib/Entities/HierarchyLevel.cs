using System;
using System.Collections.Generic;
using System.Text;
using HROne.DataAccess;
////using perspectivemind.validation;


namespace HROne.Lib.Entities
{
    [DBClass("HierarchyLevel")]
    public class EHierarchyLevel : BaseObject
    {
        public static DBManager db = new DBManager(typeof(EHierarchyLevel));
        public static WFValueList VLHierarchy = new WFDBCodeList(EHierarchyLevel.db, "HLevelID", "HLevelCode", "HLevelDesc", "HLevelSeqNo");

        protected int m_HLevelID;
        [DBField("HLevelID", true, true), TextSearch, Export(false)]
        public int HLevelID
        {
            get { return m_HLevelID; }
            set { m_HLevelID = value; modify("HLevelID"); }
        }
        protected string m_HLevelCode;
        [DBField("HLevelCode"), TextSearch,MaxLength(20, 10), Export(false), Required]
        public string HLevelCode
        {
            get { return m_HLevelCode; }
            set { m_HLevelCode = value; modify("HLevelCode"); }
        }
        protected string m_HLevelDesc;
        [DBField("HLevelDesc"), TextSearch,MaxLength(100,25), Export(false), Required]
        public string HLevelDesc
        {
            get { return m_HLevelDesc; }
            set { m_HLevelDesc = value; modify("HLevelDesc"); }
        }
        protected int m_HLevelSeqNo;
        [DBField("HLevelSeqNo"), TextSearch, Export(false), Required]
        public int HLevelSeqNo
        {
            get { return m_HLevelSeqNo; }
            set { m_HLevelSeqNo = value; modify("HLevelSeqNo"); }
        }



    }
}
