using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Text;
using HROne.DataAccess;
////using perspectivemind.validation;

namespace HROne.Lib.Entities
{
    [DBClass("UserCompany")]
    public class EUserCompany : BaseObject
    {
        public static DBManager db = new DBManager(typeof(EUserCompany));
        protected int m_UserCompanyID;
        [DBField("UserCompanyID", true, true), TextSearch, Export(false)]
        public int UserCompanyID
        {
            get { return m_UserCompanyID; }
            set { m_UserCompanyID = value; modify("UserCompanyID"); }
        }
        protected int m_UserID;
        [DBField("UserID"), TextSearch, Export(false)]
        public int UserID
        {
            get { return m_UserID; }
            set { m_UserID = value; modify("UserID"); }
        }
        protected int m_CompanyID;
        [DBField("CompanyID"), TextSearch, Export(false)]
        public int CompanyID
        {
            get { return m_CompanyID; }
            set { m_CompanyID = value; modify("CompanyID"); }
        }

        public static bool InsertCompanyForAllUsers(DatabaseConnection dbConn, int CompanyID)
        {
            ArrayList users = EUser.db.select(dbConn, (DBFilter)null);
            foreach (EUser user in users)
                InsertCompanyForUser(dbConn, user.UserID, CompanyID);
            return true;
        }

        public static void DeleteCompanyForAllUsers(DatabaseConnection dbConn, int CompanyID)
        {
            DBFilter dbfilter = new DBFilter();
            dbfilter.add(new Match("CompanyID", CompanyID));
            db.delete(dbConn, dbfilter);
        }

        public static bool InsertCompanyForUser(DatabaseConnection dbConn, int UserID, int CompanyID)
        {
            DBFilter dbfilter = new DBFilter();
            dbfilter.add(new Match("UserID",UserID));
            dbfilter.add(new Match("CompanyID",CompanyID));
            ArrayList userCompanyList = db.select(dbConn, dbfilter);
            if (userCompanyList.Count == 0)
            {
                EUserCompany userCompany = new EUserCompany();
                userCompany.CompanyID = CompanyID;
                userCompany.UserID = UserID;
                EUserCompany.db.insert(dbConn, userCompany);
            }
            return true;
            //DBUtil.getConnection().CreateCommand()
            //ArrayList users = EUser.db.select(dbConn, (DBFilter)null);
            //foreach (EUser user in users)
            //{
            //    DBFilter filter = new DBFilter();
            //    filter.
            //}
        }
    }
}
