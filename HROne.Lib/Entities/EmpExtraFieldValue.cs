using HROne.DataAccess;
////using perspectivemind.validation;

namespace HROne.Lib.Entities
{
    [DBClass("EmpExtraFieldValue")]
    public class EEmpExtraFieldValue : BaseObject
    {
        public static DBManager db = new DBManager(typeof(EEmpExtraFieldValue));

        protected int m_EmpExtraFieldValueID;
        [DBField("EmpExtraFieldValueID", true, true), TextSearch, Export(false)]
        public int EmpExtraFieldValueID
        {
            get { return m_EmpExtraFieldValueID; }
            set { m_EmpExtraFieldValueID = value; modify("EmpExtraFieldValueID"); }
        }

        protected int m_EmpID;
        [DBField("EmpID"), TextSearch, Export(false), Required]
        public int EmpID
        {
            get { return m_EmpID; }
            set { m_EmpID = value; modify("EmpID"); }
        }

        protected int m_EmpExtraFieldID;
        [DBField("EmpExtraFieldID"), TextSearch, Export(false), Required]
        public int EmpExtraFieldID
        {
            get { return m_EmpExtraFieldID; }
            set { m_EmpExtraFieldID = value; modify("EmpExtraFieldID"); }
        }

        protected string m_EmpExtraFieldValue;
        [DBField("EmpExtraFieldValue"), TextSearch, DBAESEncryptStringField(), MaxLength(50), Export(false), Required]
        public string EmpExtraFieldValue
        {
            get { return m_EmpExtraFieldValue; }
            set { m_EmpExtraFieldValue = value; modify("EmpExtraFieldValue"); }
        }
    }


}
