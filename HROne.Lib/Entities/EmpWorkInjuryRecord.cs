using System;
using HROne.DataAccess;
////using perspectivemind.validation;

namespace HROne.Lib.Entities
{
    [DBClass("EmpWorkInjuryRecord")]
    public class EEmpWorkInjuryRecord : BaseObjectWithRecordInfo
    {
        public static DBManager db = new DBManagerWithRecordInfo(typeof(EEmpWorkInjuryRecord));

        protected int m_EmpWorkInjuryRecordID;
        [DBField("EmpWorkInjuryRecordID", true, true), TextSearch, Export(false)]
        public int EmpWorkInjuryRecordID
        {
            get { return m_EmpWorkInjuryRecordID; }
            set { m_EmpWorkInjuryRecordID = value; modify("EmpWorkInjuryRecordID"); }
        }

        protected int m_EmpID;
        [DBField("EmpID"), TextSearch, Export(false), Required]
        public int EmpID
        {
            get { return m_EmpID; }
            set { m_EmpID = value; modify("EmpID"); }
        }

        protected DateTime m_EmpWorkInjuryRecordAccidentDate;
        [DBField("EmpWorkInjuryRecordAccidentDate"), TextSearch, Export(false), Required]
        public DateTime EmpWorkInjuryRecordAccidentDate
        {
            get { return m_EmpWorkInjuryRecordAccidentDate; }
            set { m_EmpWorkInjuryRecordAccidentDate = value; modify("EmpWorkInjuryRecordAccidentDate"); }
        }

        protected string m_EmpWorkInjuryRecordAccidentLocation;
        [DBField("EmpWorkInjuryRecordAccidentLocation"), TextSearch, MaxLength(100, 100), Export(false)]
        public string EmpWorkInjuryRecordAccidentLocation
        {
            get { return m_EmpWorkInjuryRecordAccidentLocation; }
            set { m_EmpWorkInjuryRecordAccidentLocation = value; modify("EmpWorkInjuryRecordAccidentLocation"); }
        }

        protected string m_EmpWorkInjuryRecordAccidentReason;
        [DBField("EmpWorkInjuryRecordAccidentReason"), TextSearch, MaxLength(100,100), Export(false)]
        public string EmpWorkInjuryRecordAccidentReason
        {
            get { return m_EmpWorkInjuryRecordAccidentReason; }
            set { m_EmpWorkInjuryRecordAccidentReason = value; modify("EmpWorkInjuryRecordAccidentReason"); }
        }

        protected string m_EmpWorkInjuryRecordInjuryNature;
        [DBField("EmpWorkInjuryRecordInjuryNature"), TextSearch, MaxLength(50), Export(false), Required]
        public string EmpWorkInjuryRecordInjuryNature
        {
            get { return m_EmpWorkInjuryRecordInjuryNature; }
            set { m_EmpWorkInjuryRecordInjuryNature = value; modify("EmpWorkInjuryRecordInjuryNature"); }
        }

        protected DateTime m_EmpWorkInjuryRecordReportedDate;
        [DBField("EmpWorkInjuryRecordReportedDate"), TextSearch, Export(false)]
        public DateTime EmpWorkInjuryRecordReportedDate
        {
            get { return m_EmpWorkInjuryRecordReportedDate; }
            set { m_EmpWorkInjuryRecordReportedDate = value; modify("EmpWorkInjuryRecordReportedDate"); }
        }

        protected DateTime m_EmpWorkInjuryRecordChequeReceivedDate;
        [DBField("EmpWorkInjuryRecordChequeReceivedDate"), TextSearch, Export(false)]
        public DateTime EmpWorkInjuryRecordChequeReceivedDate
        {
            get { return m_EmpWorkInjuryRecordChequeReceivedDate; }
            set { m_EmpWorkInjuryRecordChequeReceivedDate = value; modify("EmpWorkInjuryRecordChequeReceivedDate"); }
        }

        protected DateTime m_EmpWorkInjuryRecordSettleDate;
        [DBField("EmpWorkInjuryRecordSettleDate"), TextSearch, Export(false)]
        public DateTime EmpWorkInjuryRecordSettleDate
        {
            get { return m_EmpWorkInjuryRecordSettleDate; }
            set { m_EmpWorkInjuryRecordSettleDate = value; modify("EmpWorkInjuryRecordSettleDate"); }
        }

        protected string m_EmpWorkInjuryRecordRemark;
        [DBField("EmpWorkInjuryRecordRemark"), TextSearch, Export(false)]
        public string EmpWorkInjuryRecordRemark
        {
            get { return m_EmpWorkInjuryRecordRemark; }
            set { m_EmpWorkInjuryRecordRemark = value; modify("EmpWorkInjuryRecordRemark"); }
        }

        //  For Synchronize Use only
        protected string m_SynID;
        [DBField("SynID"), TextSearch, Export(false)]
        public string SynID
        {
            get { return m_SynID; }
            set { m_SynID = value; modify("SynID"); }
        }

    }


}
