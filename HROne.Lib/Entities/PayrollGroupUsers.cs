using System;
using System.Collections.Generic;
using System.Text;
using HROne.DataAccess;
////using perspectivemind.validation;
using System.Collections;
using HROne.Common;

namespace HROne.Lib.Entities
{
    [DBClass("PayrollGroupUsers")]
    public class EPayrollGroupUsers :BaseObjectWithRecordInfo
    {
        public static DBManager db = new DBManagerWithRecordInfo(typeof(EPayrollGroupUsers));

        public static EPayrollGroupUsers GetObject(DatabaseConnection dbConn, object ID)
        {
            if (ID is int)
            {
                EPayrollGroupUsers obj = new EPayrollGroupUsers();
                obj.PayGroupID = (int)ID;
                if (db.select(dbConn, obj))
                    return obj;
            }
            return null;
        }

        protected int m_PayGroupUsersID;
        [DBField("PayGroupUsersID", true, true), TextSearch, Export(false)]
        public int PayGroupUsersID
        {
            get { return m_PayGroupUsersID; }
            set { m_PayGroupUsersID = value; modify("PayGroupUsersID"); }
        }

        protected int m_PayGroupID;
        [DBField("PayGroupID"), TextSearch, Export(false)]
        public int PayGroupID
        {
            get { return m_PayGroupID; }
            set { m_PayGroupID = value; modify("PayGroupID"); }
        }

        protected int m_UserID;
        [DBField("UserID"), TextSearch, Export(false)]
        public int UserID
        {
            get { return m_UserID; }
            set { m_UserID = value; modify("UserID"); }
        }
    }
}
