using System;
using System.Collections.Generic;
using System.Text;
using HROne.DataAccess;
////using perspectivemind.validation;

namespace HROne.Lib.Entities
{
    [DBClass("Authorizer")]
    public class EAuthorizer : BaseObject
    {

        public static DBManager db = new DBManager(typeof(EAuthorizer));

        protected int m_AuthorizerID;
        [DBField("AuthorizerID", true, true), TextSearch, Export(false)]
        public int AuthorizerID
        {
            get { return m_AuthorizerID; }
            set { m_AuthorizerID = value; modify("AuthorizerID"); }
        }

        protected int m_EmpID;
        [DBField("EmpID"), TextSearch, Export(false), Required]
        public int EmpID
        {
            get { return m_EmpID; }
            set { m_EmpID = value; modify("EmpID"); }
        }

        protected int m_AuthorizationGroupID;
        [DBField("AuthorizationGroupID"), TextSearch, Export(false), Required]
        public int AuthorizationGroupID
        {
            get { return m_AuthorizationGroupID; }
            set { m_AuthorizationGroupID = value; modify("AuthorizationGroupID"); }
        }

        protected bool m_AuthorizerIsReadOnly;
        [DBField("AuthorizerIsReadOnly"), TextSearch, Export(false)]
        public bool AuthorizerIsReadOnly
        {
            get { return m_AuthorizerIsReadOnly; }
            set { m_AuthorizerIsReadOnly = value; modify("AuthorizerIsReadOnly"); }
        }

        protected bool m_AuthorizerSkipEmailAlert;
        [DBField("AuthorizerSkipEmailAlert"), TextSearch, Export(false)]
        public bool AuthorizerSkipEmailAlert
        {
            get { return m_AuthorizerSkipEmailAlert; }
            set { m_AuthorizerSkipEmailAlert = value; modify("AuthorizerSkipEmailAlert"); }
        }
    }
}
