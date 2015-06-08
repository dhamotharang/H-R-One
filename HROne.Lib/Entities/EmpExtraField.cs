using HROne.DataAccess;
using System.Collections;
////using perspectivemind.validation;

namespace HROne.Lib.Entities
{
    [DBClass("EmpExtraField")]
    public class EEmpExtraField : BaseObject
    {
        public static DBManager db = new DBManager(typeof(EEmpExtraField));
        public const string FIELD_CONTROL_TYPE_TEXTAREA = "TextArea";
        public const string FIELD_CONTROL_TYPE_TEXTBOX = "TextBox";
        public const string FIELD_CONTROL_TYPE_DATE = "Date";

        public static EEmpExtraField GetObjectByName(DatabaseConnection dbConn, string fieldName)
        {

            DBFilter m_filter = new DBFilter();
            m_filter.add(new Match("EmpExtraFieldName", fieldName));
            ArrayList m_list = EEmpExtraField.db.select(dbConn, m_filter);
            if (m_list.Count > 0)
            {
                return (EEmpExtraField)m_list[0];
            }
            return null; 
        }

        protected int m_EmpExtraFieldID;
        [DBField("EmpExtraFieldID", true, true), TextSearch, Export(false)]
        public int EmpExtraFieldID
        {
            get { return m_EmpExtraFieldID; }
            set { m_EmpExtraFieldID = value; modify("EmpExtraFieldID"); }
        }
        protected string m_EmpExtraFieldName;
        [DBField("EmpExtraFieldName"), TextSearch, MaxLength(50), Export(false), Required]
        public string EmpExtraFieldName
        {
            get { return m_EmpExtraFieldName; }
            set { m_EmpExtraFieldName = value; modify("EmpExtraFieldName"); }
        }

        protected string m_EmpExtraFieldControlType;
        [DBField("EmpExtraFieldControlType"), TextSearch, MaxLength(50), Export(false), Required]
        public string EmpExtraFieldControlType
        {
            get { return m_EmpExtraFieldControlType; }
            set { m_EmpExtraFieldControlType = value; modify("EmpExtraFieldControlType"); }
        }
        protected string m_EmpExtraFieldGroupName;
        [DBField("EmpExtraFieldGroupName"), TextSearch, MaxLength(50), Export(false), Required]
        public string EmpExtraFieldGroupName
        {
            get { return m_EmpExtraFieldGroupName; }
            set { m_EmpExtraFieldGroupName = value; modify("EmpExtraFieldGroupName"); }
        }
        
    }


}
