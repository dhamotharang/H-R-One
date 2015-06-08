using System;
using System.Collections.Generic;
using System.Text;
using HROne.DataAccess;
////using perspectivemind.validation;

namespace HROne.Lib.Entities
{
    [DBClass("EmpSkill")]
    public class EEmpSkill : BaseObjectWithRecordInfo
    {
        public static DBManager db = new DBManagerWithRecordInfo(typeof(EEmpSkill));

        protected int m_EmpSkillID;
        [DBField("EmpSkillID", true, true), TextSearch, Export(false)]
        public int EmpSkillID
        {
            get { return m_EmpSkillID; }
            set { m_EmpSkillID = value; modify("EmpSkillID"); }
        }
        protected int m_EmpID;
        [DBField("EmpID"), TextSearch, Export(false)]
        public int EmpID
        {
            get { return m_EmpID; }
            set { m_EmpID = value; modify("EmpID"); }
        }
        protected int m_SkillID;
        [DBField("SkillID"), TextSearch, Export(false), Required]
        public int SkillID
        {
            get { return m_SkillID; }
            set { m_SkillID = value; modify("SkillID"); }
        }
        protected int m_SkillLevelID;
        [DBField("SkillLevelID"), TextSearch, Export(false), Required]
        public int SkillLevelID
        {
            get { return m_SkillLevelID; }
            set { m_SkillLevelID = value; modify("SkillLevelID"); }
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
