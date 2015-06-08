using System;
using HROne.DataAccess;
////using perspectivemind.validation;
using HROne.Common;

namespace HROne.Lib.Entities
{
    [DBClass("UserGroup")]
    public class EUserGroup : BaseObject
    {
        public static DBManager db = new DBManager(typeof(EUserGroup));
        protected int m_UserGroupID;
        [DBField("UserGroupID", true, true), TextSearch, Export(false)]
        public int UserGroupID
        {
            get { return m_UserGroupID; }
            set { m_UserGroupID = value; modify("UserGroupID"); }
        }
        protected string m_UserGroupName;
        [DBField("UserGroupName"), MaxLength(20), TextSearch, DBAESEncryptStringField, Export(false), Required]
        public string UserGroupName
        {
            get { return m_UserGroupName; }
            set { m_UserGroupName = value; modify("UserGroupName"); }
        }
        protected string m_UserGroupDesc;
        [DBField("UserGroupDesc"), MaxLength(100, 100), TextSearch, DBAESEncryptStringField, Export(false), Required]
        public string UserGroupDesc
        {
            get { return m_UserGroupDesc; }
            set { m_UserGroupDesc = value; modify("UserGroupDesc"); }
        }

    }
}
