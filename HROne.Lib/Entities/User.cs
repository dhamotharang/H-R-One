using System;
using System.Collections;
using System.Text;
using HROne.DataAccess;
using HROne.Common;
////using perspectivemind.validation;

namespace HROne.Lib.Entities
{
    [DBClass("Users")]
    public class EUser : BaseObject
    {
        public static DBManager db = new DBManager(typeof(EUser));
        public static WFValueList VLAccountStatus = new AppUtils.NewWFTextList(new string[] { ACCOUNT_STATUS_ACTIVE, ACCOUNT_STATUS_INACTIVE }, new string[] { "Active", "Inactive/Locked" });
        public static WFValueList VLUserName = new WFDBList(db, "UserID", "UserName");

        public const string ACCOUNT_STATUS_ACTIVE = "A";
        public const string ACCOUNT_STATUS_INACTIVE = "I";
        public const string ACCOUNT_STATUS_MARKDELETED = "A";

        public static EUser GetObject(DatabaseConnection dbConn, object ID)
        {
            if (ID is int)
            {
                EUser obj = new EUser();
                obj.UserID = (int)ID;
                if (db.select(dbConn, obj))
                    return obj;
            }
            return null;
        }

        protected int m_UserID;
        [DBField("UserID", true, true), TextSearch, Export(false)]
        public int UserID
        {
            get { return m_UserID; }
            set { m_UserID = value; modify("UserID"); }
        }
        protected string m_LoginID;
        [DBField("LoginID"),MaxLength(20,20), TextSearch, Export(false),Required]
        public string LoginID
        {
            get { return m_LoginID; }
            set { m_LoginID = value; modify("LoginID"); }
        }
        protected string m_UserName;
        [DBField("UserName"),MaxLength(100,40), TextSearch, Export(false), Required]
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
        protected string m_UserLanguage;
        [DBField("UserLanguage"), TextSearch, Export(false)]
        public string UserLanguage
        {
            get { return m_UserLanguage; }
            set { m_UserLanguage = value; modify("UserLanguage"); }
        }
        
        protected bool m_UserIsKeepConnected;
        [DBField("UserIsKeepConnected"), TextSearch, Export(false)]
        public bool UserIsKeepConnected
        {
            get { return m_UserIsKeepConnected; }
            set { m_UserIsKeepConnected = value; modify("UserIsKeepConnected"); }
        }

        protected bool m_UsersCannotCreateUsersWithMorePermission;
        [DBField("UsersCannotCreateUsersWithMorePermission"), TextSearch, Export(false)]
        public bool UsersCannotCreateUsersWithMorePermission
        {
            get { return m_UsersCannotCreateUsersWithMorePermission; }
            set { m_UsersCannotCreateUsersWithMorePermission = value; modify("UsersCannotCreateUsersWithMorePermission"); }
        }


        public bool IsAllowSubmitPermission(DatabaseConnection dbConn, int ActiveUserID)
        {
            DBFilter filter = new DBFilter();
            filter.add(new Match("UserID", m_UserID));

            ArrayList userGroupPermissionList = new ArrayList();
            {
                ArrayList userGroupAccessList = EUserGroupAccess.db.select(dbConn, filter);
                foreach (EUserGroupAccess userGroupAccess in userGroupAccessList)
                    userGroupPermissionList.Add(userGroupAccess.UserGroupID);
            }

            ArrayList companyPermissionList = new ArrayList();
            {
                ArrayList userCompanyList = EUserCompany.db.select(dbConn, filter);
                foreach (EUserCompany userCompany in userCompanyList)
                    companyPermissionList.Add(userCompany.CompanyID);
            }

            ArrayList rankPermissionList = new ArrayList();
            {
                ArrayList userRankList = EUserRank.db.select(dbConn, filter);
                foreach (EUserRank userRank in userRankList)
                    rankPermissionList.Add(userRank.RankID);
            }
            // Start 0000069, KuangWei, 2014-08-26
            ArrayList payrollGroupPermissionList = new ArrayList();
            {
                ArrayList payrollGroupUsersList = EPayrollGroupUsers.db.select(dbConn, filter);
                foreach (EPayrollGroupUsers payrollGroupUsers in payrollGroupUsersList)
                    payrollGroupPermissionList.Add(payrollGroupUsers.PayGroupID);
            }
            return EUser.IsAllowSubmitPermission(dbConn, ActiveUserID, companyPermissionList, userGroupPermissionList, rankPermissionList, payrollGroupPermissionList);
            // End 0000069, KuangWei, 2014-08-26
        }

        // Start 0000069, KuangWei, 2014-08-26
        public static bool IsAllowSubmitPermission(DatabaseConnection dbConn, int ActiveUserID, ICollection CompanyPermissionList, ICollection UserGroupPermissionList, ICollection RankPermssionList, ICollection PayrollGroupPermissionList)
        {
            foreach (object company in CompanyPermissionList)
            {
                int CompanyID = 0;
                if (company is int)
                    CompanyID = (int)company;
                else if (company is ECompany)
                    CompanyID = ((ECompany)company).CompanyID;

                DBFilter UserCompanyFilter = new DBFilter();
                UserCompanyFilter.add(new Match("UserID", ActiveUserID));
                UserCompanyFilter.add(new Match("CompanyID", CompanyID));

                if (EUserCompany.db.count(dbConn, UserCompanyFilter) < 1)
                    return false;
            }
            foreach (object userGroup in UserGroupPermissionList)
            {
                int UserGroupID = 0;
                if (userGroup is int)
                    UserGroupID = (int)userGroup;
                else if (userGroup is EUserGroup)
                    UserGroupID = ((EUserGroup)userGroup).UserGroupID;

                DBFilter UserGroupAccessFilter = new DBFilter();
                UserGroupAccessFilter.add(new Match("UserID", ActiveUserID));
                UserGroupAccessFilter.add(new Match("UserGroupID", UserGroupID));

                if (EUserGroupAccess.db.count(dbConn, UserGroupAccessFilter) < 1)
                    return false;
            }
            foreach (object rank in RankPermssionList)
            {
                int RankID = 0;
                if (rank is int)
                    RankID = (int)rank;
                else if (rank is ERank)
                    RankID = ((ERank)rank).RankID;

                DBFilter UserRankFilter = new DBFilter();
                UserRankFilter.add(new Match("UserID", ActiveUserID));
                UserRankFilter.add(new Match("RankID", RankID));

                if (EUserRank.db.count(dbConn, UserRankFilter) < 1)
                    return false;
            }
            foreach (object payrollGroup in PayrollGroupPermissionList)
            {
                int PayGroupID = 0;
                if (payrollGroup is int)
                    PayGroupID = (int)payrollGroup;
                else if (payrollGroup is EPayrollGroup)
                    PayGroupID = ((EPayrollGroup)payrollGroup).PayGroupID;

                DBFilter PayrollGroupUsersFilter = new DBFilter();
                PayrollGroupUsersFilter.add(new Match("UserID", ActiveUserID));
                PayrollGroupUsersFilter.add(new Match("PayGroupID", PayGroupID));

                if (EPayrollGroupUsers.db.count(dbConn, PayrollGroupUsersFilter) < 1)
                    return false;
            }
            return true;
        }
        // End 0000069, KuangWei, 2014-08-26

        protected override void InsertAuditTrailDetail(DatabaseConnectionWithAudit dbConnWithAudit, EAuditTrailDetail d)
        {
            if (d != null)
                if (this.UserAccountStatus.Equals("D", StringComparison.CurrentCultureIgnoreCase))
                    d.ActionType = EAuditTrailDetail.ACTIONTYPE_MARKDELETE;
            base.InsertAuditTrailDetail(dbConnWithAudit, d);
        }
    }
}
