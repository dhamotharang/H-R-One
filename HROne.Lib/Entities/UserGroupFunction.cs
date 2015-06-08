using System;
using System.Collections.Generic;
using System.Text;
using HROne.DataAccess;
////using perspectivemind.validation;

namespace HROne.Lib.Entities
{
    [DBClass("UserGroupFunction")]
    public class EUserGroupFunction : BaseObject
    {
        public static DBManager db = new DBManager(typeof(EUserGroupFunction));
        protected int m_UserGroupFunctionID;
        [DBField("UserGroupFunctionID", true, true), TextSearch, Export(false)]
        public int UserGroupFunctionID
        {
            get { return m_UserGroupFunctionID; }
            set { m_UserGroupFunctionID = value; modify("UserGroupFunctionID"); }
        }
        protected int m_UserGroupID;
        [DBField("UserGroupID"), TextSearch, Export(false), Required]
        public int UserGroupID
        {
            get { return m_UserGroupID; }
            set { m_UserGroupID = value; modify("UserGroupID"); }
        }
        protected int m_FunctionID;
        [DBField("FunctionID"), TextSearch, Export(false), Required]
        public int FunctionID
        {
            get { return m_FunctionID; }
            set { m_FunctionID = value; modify("FunctionID"); }
        }


        protected bool m_FunctionAllowRead;
        [DBField("FunctionAllowRead"), TextSearch, Export(false), Required]
        public bool FunctionAllowRead
        {
            get { return m_FunctionAllowRead; }
            set { m_FunctionAllowRead = value; modify("FunctionAllowRead"); }
        }
        protected bool m_FunctionAllowWrite;
        [DBField("FunctionAllowWrite"), TextSearch, Export(false), Required]
        public bool FunctionAllowWrite
        {
            get { return m_FunctionAllowWrite; }
            set { m_FunctionAllowWrite = value; modify("FunctionAllowWrite"); }
        }
 
    }
}
