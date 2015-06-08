using System;
using System.Collections;
using System.Text;
using HROne.DataAccess;
////using perspectivemind.validation;

namespace HROne.Lib.Entities
{
    [DBClass("UserReminderOption")]
    public class EUserReminderOption : BaseObject
    {
        public static DBManager db = new DBManager(typeof(EUserReminderOption));

        protected int m_UserReminderOptionID;
        [DBField("UserReminderOptionID", true, true), TextSearch, Export(false)]
        public int UserReminderOptionID
        {
            get { return m_UserReminderOptionID; }
            set { m_UserReminderOptionID = value; modify("UserReminderOptionID"); }
        }
        protected int m_UserID;
        [DBField("UserID"), TextSearch, Export(false), Required]
        public int UserID
        {
            get { return m_UserID; }
            set { m_UserID = value; modify("UserID"); }
        }
        protected int m_ReminderTypeID;
        [DBField("ReminderTypeID"), TextSearch, Export(false)]
        public int ReminderTypeID
        {
            get { return m_ReminderTypeID; }
            set { m_ReminderTypeID = value; modify("ReminderTypeID"); }
        }
        protected int m_UserReminderOptionRemindDaysBefore;
        [DBField("UserReminderOptionRemindDaysBefore"), TextSearch, Export(false)]
        public int UserReminderOptionRemindDaysBefore
        {
            get { return m_UserReminderOptionRemindDaysBefore; }
            set { m_UserReminderOptionRemindDaysBefore = value; modify("UserReminderOptionRemindDaysBefore"); }
        }
        protected int m_UserReminderOptionRemindDaysAfter;
        [DBField("UserReminderOptionRemindDaysAfter"), TextSearch, Export(false)]
        public int UserReminderOptionRemindDaysAfter
        {
            get { return m_UserReminderOptionRemindDaysAfter; }
            set { m_UserReminderOptionRemindDaysAfter = value; modify("UserReminderOptionRemindDaysAfter"); }
        }



        public static bool IsUserGenerateReminder(DatabaseConnection dbConn, int UserID, string ReminderTypeCode, out EUserReminderOption userReminderOption)
        {
            DBFilter reminderTypeFilter = new DBFilter();
            reminderTypeFilter.add(new Match("ReminderTypeCode", ReminderTypeCode));
            DBFilter filter = new DBFilter();
            filter.add(new IN("ReminderTypeID", "Select ReminderTypeID from " + EReminderType.db.dbclass.tableName, reminderTypeFilter));
            filter.add(new Match("UserID", UserID));

            ArrayList list = EUserReminderOption.db.select(dbConn, filter);
            if (list.Count > 0)
            {
                userReminderOption = (EUserReminderOption)list[0];
                return true;
            }
            else
            {
                userReminderOption = null;
                return false;
            }
        }
    }
}