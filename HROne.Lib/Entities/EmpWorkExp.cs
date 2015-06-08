using HROne.DataAccess;
////using perspectivemind.validation;

namespace HROne.Lib.Entities
{
    [DBClass("EmpWorkExp")]
    public class EEmpWorkExp : BaseObjectWithRecordInfo
    {
        public static DBManager db = new DBManagerWithRecordInfo(typeof(EEmpWorkExp));

        protected int m_EmpWorkExpID;
        [DBField("EmpWorkExpID", true, true), TextSearch, Export(false)]
        public int EmpWorkExpID
        {
            get { return m_EmpWorkExpID; }
            set { m_EmpWorkExpID = value; modify("EmpWorkExpID"); }
        }

        protected int m_EmpID;
        [DBField("EmpID"), TextSearch, Export(false)]
        public int EmpID
        {
            get { return m_EmpID; }
            set { m_EmpID = value; modify("EmpID"); }
        }

        protected int m_EmpWorkExpFromYear;
        [DBField("EmpWorkExpFromYear"), TextSearch, MaxLength(4), Export(false),Required]
        public int EmpWorkExpFromYear
        {
            get { return m_EmpWorkExpFromYear; }
            set { m_EmpWorkExpFromYear = value; modify("EmpWorkExpFromYear"); }
        }

        protected int m_EmpWorkExpFromMonth;
        [DBField("EmpWorkExpFromMonth"), TextSearch, Export(false)]
        public int EmpWorkExpFromMonth
        {
            get { return m_EmpWorkExpFromMonth; }
            set { m_EmpWorkExpFromMonth = value; modify("EmpWorkExpFromMonth"); }
        }

        protected int m_EmpWorkExpToYear;
        [DBField("EmpWorkExpToYear"), TextSearch, MaxLength(4), Export(false)]
        public int EmpWorkExpToYear
        {
            get { return m_EmpWorkExpToYear; }
            set { m_EmpWorkExpToYear = value; modify("EmpWorkExpToYear"); }
        }

        protected int m_EmpWorkExpToMonth;
        [DBField("EmpWorkExpToMonth"), TextSearch, Export(false)]
        public int EmpWorkExpToMonth
        {
            get { return m_EmpWorkExpToMonth; }
            set { m_EmpWorkExpToMonth = value; modify("EmpWorkExpToMonth"); }
        }

        protected string m_EmpWorkExpCompanyName;
        [DBField("EmpWorkExpCompanyName"), TextSearch, MaxLength(1000,40), Export(false)]
        public string EmpWorkExpCompanyName
        {
            get { return m_EmpWorkExpCompanyName; }
            set { m_EmpWorkExpCompanyName = value; modify("EmpWorkExpCompanyName"); }
        }

        protected string m_EmpWorkExpPosition;
        [DBField("EmpWorkExpPosition"), TextSearch, MaxLength(100, 40), Export(false)]
        public string EmpWorkExpPosition
        {
            get { return m_EmpWorkExpPosition; }
            set { m_EmpWorkExpPosition = value; modify("EmpWorkExpPosition"); }
        }

        protected int m_EmpWorkExpEmploymentTypeID;
        [DBField("EmpWorkExpEmploymentTypeID"), TextSearch, Export(false)]
        public int EmpWorkExpEmploymentTypeID
        {
            get { return m_EmpWorkExpEmploymentTypeID; }
            set { m_EmpWorkExpEmploymentTypeID = value; modify("EmpWorkExpEmploymentTypeID"); }
        }


        protected bool m_EmpWorkExpIsRelevantExperience;
        [DBField("EmpWorkExpIsRelevantExperience"), TextSearch, Export(false)]
        public bool EmpWorkExpIsRelevantExperience
        {
            get { return m_EmpWorkExpIsRelevantExperience; }
            set { m_EmpWorkExpIsRelevantExperience = value; modify("EmpWorkExpIsRelevantExperience"); }
        }

        protected string m_EmpWorkExpRemark;
        [DBField("EmpWorkExpRemark"), TextSearch, Export(false)]
        public string EmpWorkExpRemark
        {
            get { return m_EmpWorkExpRemark; }
            set { m_EmpWorkExpRemark = value; modify("EmpWorkExpRemark"); }
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
