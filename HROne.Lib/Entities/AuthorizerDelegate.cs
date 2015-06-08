using System;
using System.Collections.Generic;
using System.Text;
using HROne.DataAccess;

namespace HROne.Lib.Entities
{
    [DBClass("AuthorizerDelegate")]
    public class EAuthorizerDelegate : BaseObject
    {
        public static DBManager db = new DBManager(typeof(EAuthorizerDelegate));

        protected int m_AuthorizerDelegateID;
        [DBField("AuthorizerDelegateID", true, true), TextSearch, Export(false)]
        public int AuthorizerDelegateID
        {
            get { return m_AuthorizerDelegateID; }
            set { m_AuthorizerDelegateID = value; modify("AuthorizerDelegateID"); }
        }

        protected int m_EmpID;
        [DBField("EmpID"), TextSearch, Export(false), Required]
        public int EmpID
        {
            get { return m_EmpID; }
            set { m_EmpID = value; modify("EmpID"); }
        }

        protected int m_AuthorizerDelegateEmpID;
        [DBField("AuthorizerDelegateEmpID"), TextSearch, Export(false), Required]
        public int AuthorizerDelegateEmpID
        {
            get { return m_AuthorizerDelegateEmpID; }
            set { m_AuthorizerDelegateEmpID = value; modify("AuthorizerDelegateEmpID"); }
        }
    }
}
