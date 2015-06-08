using System;
using System.Collections.Generic;
using System.Text;
using HROne.DataAccess;

namespace HROne.SaaS.Entities
{
    [DBClass("StatutoryHoliday")]
    public class EStatutoryHoliday : BaseObject
    {
        public static DBManager db = new DBManager(typeof(EStatutoryHoliday));

        protected int m_StatutoryHolidayID;
        [DBField("StatutoryHolidayID", true, true), TextSearch, Export(false)]
        public int StatutoryHolidayID
        {
            get { return m_StatutoryHolidayID; }
            set { m_StatutoryHolidayID = value; modify("StatutoryHolidayID"); }
        }
        protected DateTime m_StatutoryHolidayDate;
        [DBField("StatutoryHolidayDate"), TextSearch, Export(false), Required]
        public DateTime StatutoryHolidayDate
        {
            get { return m_StatutoryHolidayDate; }
            set { m_StatutoryHolidayDate = value; modify("StatutoryHolidayDate"); }
        }


        protected string m_StatutoryHolidayDesc;
        [DBField("StatutoryHolidayDesc"), TextSearch, MaxLength(100, 25), Export(false), Required]
        public string StatutoryHolidayDesc
        {
            get { return m_StatutoryHolidayDesc; }
            set { m_StatutoryHolidayDesc = value; modify("StatutoryHolidayDesc"); }
        }

        public static bool IsHoliday(DatabaseConnection dbConn, DateTime date)
        {
            DBFilter filter = new DBFilter();
            filter.add(new Match("StatutoryHolidayDate", date));

            if (db.count(dbConn, filter) > 0)
                return true;
            else
                return false;
        }
    }

    [DBClass("PublicHoliday")]
    public class EPublicHoliday : BaseObject
    {
        public static DBManager db = new DBManager(typeof(EPublicHoliday));

        protected int m_PublicHolidayID;
        [DBField("PublicHolidayID", true, true), TextSearch, Export(false)]
        public int PublicHolidayID
        {
            get { return m_PublicHolidayID; }
            set { m_PublicHolidayID = value; modify("PublicHolidayID"); }
        }
        protected DateTime m_PublicHolidayDate;
        [DBField("PublicHolidayDate"), TextSearch, Export(false), Required]
        public DateTime PublicHolidayDate
        {
            get { return m_PublicHolidayDate; }
            set { m_PublicHolidayDate = value; modify("PublicHolidayDate"); }
        }

        protected string m_PublicHolidayDesc;
        [DBField("PublicHolidayDesc"), TextSearch, MaxLength(100, 25), Export(false), Required]
        public string PublicHolidayDesc
        {
            get { return m_PublicHolidayDesc; }
            set { m_PublicHolidayDesc = value; modify("PublicHolidayDesc"); }
        }

        public static bool IsHoliday(DatabaseConnection dbConn, DateTime date)
        {
            DBFilter filter = new DBFilter();
            filter.add(new Match("PublicHolidayDate", date));

            if (db.count(dbConn, filter) > 0)
                return true;
            else
                return false;
        }
    }
}
