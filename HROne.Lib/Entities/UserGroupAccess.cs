using System;
using System.Collections.Generic;
using System.Text;
using HROne.DataAccess;
////using perspectivemind.validation;

namespace HROne.Lib.Entities
{
    [DBClass("UserGroupAccess")]
    public class EUserGroupAccess : BaseObject
    {
        public static DBManager db = new DBManager(typeof(EUserGroupAccess));
        protected int m_UserGroupAccessID;
        [DBField("UserGroupAccessID", true, true), TextSearch, Export(false)]
        public int UserGroupAccessID
        {
            get { return m_UserGroupAccessID; }
            set { m_UserGroupAccessID = value; modify("UserGroupAccessID"); }
        }
        protected int m_UserID;
        [DBField("UserID"), TextSearch, Export(false)]
        public int UserID
        {
            get { return m_UserID; }
            set { m_UserID = value; modify("UserID"); }
        }
        protected int m_UserGroupID;
        [DBField("UserGroupID"), TextSearch, Export(false)]
        public int UserGroupID
        {
            get { return m_UserGroupID; }
            set { m_UserGroupID = value; modify("UserGroupID"); }
        }
 
    }
}
