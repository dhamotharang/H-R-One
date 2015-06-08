using System;
using System.Collections;
using System.Text;
using HROne.DataAccess;
////using perspectivemind.validation;

namespace HROne.Lib.Entities
{
    [DBClass("SystemFunctionEmailAlert")]
    public class ESystemFunctionEmailAlert : BaseObject
    {
        public static DBManager db = new DBManager(typeof(ESystemFunctionEmailAlert));

        protected int m_SystemFunctionEmailAlertID;
        [DBField("SystemFunctionEmailAlertID", true, true), TextSearch, Export(false)]
        public int SystemFunctionEmailAlertID
        {
            get { return m_SystemFunctionEmailAlertID; }
            set { m_SystemFunctionEmailAlertID = value; modify("SystemFunctionEmailAlertID"); }
        }

        protected int m_FunctionID;
        [DBField("FunctionID"), TextSearch, Export(false)]
        public int FunctionID
        {
            get { return m_FunctionID; }
            set { m_FunctionID = value; modify("FunctionID"); }
        }

        protected bool m_SystemFunctionEmailAlertInsert;
        [DBField("SystemFunctionEmailAlertInsert"), TextSearch, Export(false)]
        public bool SystemFunctionEmailAlertInsert
        {
            get { return m_SystemFunctionEmailAlertInsert; }
            set { m_SystemFunctionEmailAlertInsert = value; modify("SystemFunctionEmailAlertInsert"); }
        }

        protected bool m_SystemFunctionEmailAlertUpdate;
        [DBField("SystemFunctionEmailAlertUpdate"), TextSearch, Export(false)]
        public bool SystemFunctionEmailAlertUpdate
        {
            get { return m_SystemFunctionEmailAlertUpdate; }
            set { m_SystemFunctionEmailAlertUpdate = value; modify("SystemFunctionEmailAlertUpdate"); }
        }

        protected bool m_SystemFunctionEmailAlertDelete;
        [DBField("SystemFunctionEmailAlertDelete"), TextSearch, Export(false)]
        public bool SystemFunctionEmailAlertDelete
        {
            get { return m_SystemFunctionEmailAlertDelete; }
            set { m_SystemFunctionEmailAlertDelete = value; modify("SystemFunctionEmailAlertDelete"); }
        }
    }
}
