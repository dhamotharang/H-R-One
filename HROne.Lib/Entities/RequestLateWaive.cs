using System;
using System.Collections;
using System.Text;
using HROne.DataAccess;

namespace HROne.Lib.Entities
{
    [DBClass("RequestLateWaive")]
    public class ERequestLateWaive : BaseObject
    {

        public static DBManager db = new DBManager(typeof(ERequestLateWaive));
        public static WFValueList VLLateWaiveYear = new AppUtils.WFDBDistinctList(db, "Year(RequestLateWaiveCreateDate)", " Year(RequestLateWaiveCreateDate)", "Year(RequestLateWaiveCreateDate) desc");

        protected int m_RequestLateWaiveID;
        [DBField("RequestLateWaiveID", true, true), TextSearch, Export(false)]
        public int RequestLateWaiveID
        {
            get { return m_RequestLateWaiveID; }
            set { m_RequestLateWaiveID = value; modify("RequestLateWaiveID"); }
        }

        protected int m_EmpID;
        [DBField("EmpID"), TextSearch, Export(false), Required]
        public int EmpID
        {
            get { return m_EmpID; }
            set { m_EmpID = value; modify("EmpID"); }
        }

        protected int m_AttendanceRecordID;
        [DBField("AttendanceRecordID"), TextSearch, Export(false), Required]
        public int AttendanceRecordID
        {
            get { return m_AttendanceRecordID; }
            set { m_AttendanceRecordID = value; modify("AttendanceRecordID"); }
        }

        protected string m_RequestLateWaiveReason;
        [DBField("RequestLateWaiveReason"), TextSearch, Export(false)]
        public string RequestLateWaiveReason
        {
            get { return m_RequestLateWaiveReason; }
            set { m_RequestLateWaiveReason = value; modify("RequestLateWaiveReason"); }
        }

        protected DateTime m_RequestLateWaiveCreateDate;
        [DBField("RequestLateWaiveCreateDate"), TextSearch, Export(false)]
        public DateTime RequestLateWaiveCreateDate
        {
            get { return m_RequestLateWaiveCreateDate; }
            set { m_RequestLateWaiveCreateDate = value; modify("RequestLateWaiveCreateDate"); }
        }

        public static ERequestLateWaive CreateDummyRequestLateWaive(ELateWaive LateWaive)
        {
            ERequestLateWaive requestLateWaive = new ERequestLateWaive();
            requestLateWaive.EmpID = LateWaive.EmpID;
            requestLateWaive.AttendanceRecordID = LateWaive.AttendanceRecordID;
            requestLateWaive.RequestLateWaiveReason = LateWaive.LateWaiveReason;
            return requestLateWaive;
        }
    }
}
