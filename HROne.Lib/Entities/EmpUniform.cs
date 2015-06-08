using HROne.DataAccess;
////using perspectivemind.validation;

namespace HROne.Lib.Entities
{
    [DBClass("EmpUniform")]
    public class EEmpUniform : BaseObject
    {
        public static DBManager db = new DBManager(typeof(EEmpUniform));

        protected int m_EmpUniformID;
        [DBField("EmpUniformID", true, true), TextSearch, Export(false)]
        public int EmpUniformID
        {
            get { return m_EmpUniformID; }
            set { m_EmpUniformID = value; modify("EmpUniformID"); }
        }

        protected int m_EmpID;
        [DBField("EmpID"), TextSearch, Export(false)]
        public int EmpID
        {
            get { return m_EmpID; }
            set { m_EmpID = value; modify("EmpID"); }
        }

        protected string m_EmpUniformB;
        [DBField("EmpUniformB"), TextSearch, MaxLength(10), Export(false)]
        public string EmpUniformB
        {
            get { return m_EmpUniformB; }
            set { m_EmpUniformB = value; modify("EmpUniformB"); }
        }

        protected string m_EmpUniformW;
        [DBField("EmpUniformW"), TextSearch, MaxLength(10), Export(false)]
        public string EmpUniformW
        {
            get { return m_EmpUniformW; }
            set { m_EmpUniformW = value; modify("EmpUniformW"); }
        }

        protected string m_EmpUniformH;
        [DBField("EmpUniformH"), TextSearch, MaxLength(10), Export(false)]
        public string EmpUniformH
        {
            get { return m_EmpUniformH; }
            set { m_EmpUniformH = value; modify("EmpUniformH"); }
        }
    }


}
