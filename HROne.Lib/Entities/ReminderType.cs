using System;
using System.Collections;
using System.Text;
using HROne.DataAccess;
////using perspectivemind.validation;

namespace HROne.Lib.Entities
{
    [DBClass("ReminderType")]
    public class EReminderType : BaseObject
    {
        public static DBManager db = new DBManager(typeof(EReminderType));

        protected int m_ReminderTypeID;
        [DBField("ReminderTypeID", true, true), TextSearch, Export(false)]
        public int ReminderTypeID
        {
            get { return m_ReminderTypeID; }
            set { m_ReminderTypeID = value; modify("ReminderTypeID"); }
        }
        protected string m_ReminderTypeCode;
        [DBField("ReminderTypeCode"), TextSearch, MaxLength(20, 10), Export(false), Required]
        public string ReminderTypeCode
        {
            get { return m_ReminderTypeCode; }
            set { m_ReminderTypeCode = value; modify("ReminderTypeCode"); }
        }

        protected string m_ReminderTypeDesc;
        [DBField("ReminderTypeDesc"), TextSearch, MaxLength(100, 25), Export(false), Required]
        public string ReminderTypeDesc
        {
            get { return m_ReminderTypeDesc; }
            set { m_ReminderTypeDesc = value; modify("ReminderTypeDesc"); }
        }
    }
}