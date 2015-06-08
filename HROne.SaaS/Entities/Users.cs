using System;
using System.Collections;
using System.Text;
using HROne.DataAccess;
using HROne.Common;
////using perspectivemind.validation;

namespace HROne.SaaS.Entities
{
    [DBClass("Users")]
    public class EUser : DBObject
    {
        public static DBManager db = new DBManager(typeof(EUser));
        public static WFValueList VLAccountStatus = new AppUtils.NewWFTextList(new string[] { "A", "I" }, new string[] { "Active", "Inactive/Locked" });
        public static WFValueList VLUserName = new WFDBList(db, "UserID", "UserName");

        protected int m_UserID;
        [DBField("UserID", true, true), TextSearch, Export(false)]
        public int UserID
        {
            get { return m_UserID; }
            set { m_UserID = value; modify("UserID"); }
        }
        protected string m_LoginID;
        [DBField("LoginID"), MaxLength(20, 20), TextSearch, Export(false), Required]
        public string LoginID
        {
            get { return m_LoginID; }
            set { m_LoginID = value; modify("LoginID"); }
        }
        protected string m_UserName;
        [DBField("UserName"), MaxLength(100, 40), TextSearch, Export(false), Required]
        public string UserName
        {
            get { return m_UserName; }
            set { m_UserName = value; modify("UserName"); }
        }
        protected string m_UserPassword;
        [DBField("UserPassword"), TextSearch, MaxLength(50, 40), Export(false), Required]
        public string UserPassword
        {
            get { return m_UserPassword; }
            set { m_UserPassword = value; modify("UserPassword"); }
        }
        protected string m_UserAccountStatus;
        [DBField("UserAccountStatus"), TextSearch, Export(false), Required]
        public string UserAccountStatus
        {
            get { return m_UserAccountStatus; }
            set { m_UserAccountStatus = value; modify("UserAccountStatus"); }
        }
        protected DateTime m_ExpiryDate;
        [DBField("ExpiryDate"), TextSearch, Export(false)]
        public DateTime ExpiryDate
        {
            get { return m_ExpiryDate; }
            set { m_ExpiryDate = value; modify("ExpiryDate"); }
        }
        protected bool m_UserChangePassword;
        [DBField("UserChangePassword"), TextSearch, Export(false)]
        public bool UserChangePassword
        {
            get { return m_UserChangePassword; }
            set { m_UserChangePassword = value; modify("UserChangePassword"); }
        }
        protected int m_UserChangePasswordPeriod;
        [DBField("UserChangePasswordPeriod"), MaxLength(3), TextSearch, Export(false)]
        public int UserChangePasswordPeriod
        {
            get { return m_UserChangePasswordPeriod; }
            set { m_UserChangePasswordPeriod = value; modify("UserChangePasswordPeriod"); }
        }
        protected string m_UserChangePasswordUnit;
        [DBField("UserChangePasswordUnit"), TextSearch, Export(false)]
        public string UserChangePasswordUnit
        {
            get { return m_UserChangePasswordUnit; }
            set { m_UserChangePasswordUnit = value; modify("UserChangePasswordUnit"); }
        }
        protected DateTime m_UserChangePasswordDate;
        [DBField("UserChangePasswordDate"), TextSearch, Export(false)]
        public DateTime UserChangePasswordDate
        {
            get { return m_UserChangePasswordDate; }
            set { m_UserChangePasswordDate = value; modify("UserChangePasswordDate"); }
        }
        protected int m_FailCount;
        [DBField("FailCount"), MaxLength(3), TextSearch, Export(false)]
        public int FailCount
        {
            get { return m_FailCount; }
            set { m_FailCount = value; modify("FailCount"); }
        }

    }
}
