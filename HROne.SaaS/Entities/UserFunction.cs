using System;
using System.Collections.Generic;
using System.Text;
using HROne.DataAccess;

namespace HROne.SaaS.Entities
{
    [DBClass("UserFunction")]
    public class EUserFunction : DBObject
    {
        public static DBManager db = new DBManager(typeof(EUserFunction));
        protected int m_UserFunctionID;
        [DBField("UserFunctionID", true, true), TextSearch, Export(false)]
        public int UserFunctionID
        {
            get { return m_UserFunctionID; }
            set { m_UserFunctionID = value; modify("UserFunctionID"); }
        }
        protected int m_UserID;
        [DBField("UserID"), TextSearch, Export(false), Required]
        public int UserID
        {
            get { return m_UserID; }
            set { m_UserID = value; modify("UserID"); }
        }
        protected int m_FunctionID;
        [DBField("FunctionID"), TextSearch, Export(false), Required]
        public int FunctionID
        {
            get { return m_FunctionID; }
            set { m_FunctionID = value; modify("FunctionID"); }
        }
    }
}
