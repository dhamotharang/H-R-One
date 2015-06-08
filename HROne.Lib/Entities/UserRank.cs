using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Text;
using HROne.DataAccess;
////using perspectivemind.validation;

namespace HROne.Lib.Entities
{
    [DBClass("UserRank")]
    public class EUserRank : BaseObject
    {
        public static DBManager db = new DBManager(typeof(EUserRank));
        protected int m_UserRankID;
        [DBField("UserRankID", true, true), TextSearch, Export(false)]
        public int UserRankID
        {
            get { return m_UserRankID; }
            set { m_UserRankID = value; modify("UserRankID"); }
        }
        protected int m_UserID;
        [DBField("UserID"), TextSearch, Export(false)]
        public int UserID
        {
            get { return m_UserID; }
            set { m_UserID = value; modify("UserID"); }
        }
        protected int m_RankID;
        [DBField("RankID"), TextSearch, Export(false)]
        public int RankID
        {
            get { return m_RankID; }
            set { m_RankID = value; modify("RankID"); }
        }

        public static bool InsertRankForAllUsers(DatabaseConnection dbConn, int RankID)
        {
            ArrayList users = EUser.db.select(dbConn, (DBFilter)null);
            foreach (EUser user in users)
                InsertRankForUser(dbConn, user.UserID, RankID);
            return true;
        }

        public static void DeleteRankForAllUsers(DatabaseConnection dbConn, int RankID)
        {
            DBFilter dbfilter = new DBFilter();
            dbfilter.add(new Match("RankID", RankID));
            db.delete(dbConn, dbfilter);
        }

        public static bool InsertRankForUser(DatabaseConnection dbConn, int UserID, int RankID)
        {
            DBFilter dbfilter = new DBFilter();
            dbfilter.add(new Match("UserID",UserID));
            dbfilter.add(new Match("RankID",RankID));
            ArrayList userRankList = db.select(dbConn, dbfilter);
            if (userRankList.Count == 0)
            {
                EUserRank userRank = new EUserRank();
                userRank.RankID = RankID;
                userRank.UserID = UserID;
                EUserRank.db.insert(dbConn, userRank);
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
