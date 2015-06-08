using System;
using System.Collections.Generic;
using System.Text;
using HROne.DataAccess;
////using perspectivemind.validation;

namespace HROne.Lib.Entities
{
    [DBClass("EmpTermination")]
    public class EEmpTermination : BaseObjectWithRecordInfo
    {
        public static DBManager db = new DBManagerWithRecordInfo(typeof(EEmpTermination));
        protected int m_EmpTermID;
        [DBField("EmpTermID", true, true), TextSearch, Export(false)]
        public int EmpTermID
        {
            get { return m_EmpTermID; }
            set { m_EmpTermID = value; modify("EmpTermID"); }
        }
        protected int m_EmpID;
        [DBField("EmpID"), TextSearch, Export(false)]
        public int EmpID
        {
            get { return m_EmpID; }
            set { m_EmpID = value; modify("EmpID"); }
        }
        protected int m_CessationReasonID;
        [DBField("CessationReasonID"), TextSearch, Export(false),Required]
        public int CessationReasonID
        {
            get { return m_CessationReasonID; }
            set { m_CessationReasonID = value; modify("CessationReasonID"); }
        }
        protected DateTime m_EmpTermResignDate;
        [DBField("EmpTermResignDate"), TextSearch,MaxLength(10), Export(false), Required]
        public DateTime EmpTermResignDate
        {
            get { return m_EmpTermResignDate; }
            set { m_EmpTermResignDate = value; modify("EmpTermResignDate"); }
        }
        protected int m_EmpTermNoticePeriod;
        [DBField("EmpTermNoticePeriod"), TextSearch, MaxLength(5), Export(false), Required]
        public int EmpTermNoticePeriod
        {
            get { return m_EmpTermNoticePeriod; }
            set { m_EmpTermNoticePeriod = value; modify("EmpTermNoticePeriod"); }
        }
        protected string m_EmpTermNoticeUnit;
        [DBField("EmpTermNoticeUnit"), TextSearch, Export(false), Required]
        public string EmpTermNoticeUnit
        {
            get { return m_EmpTermNoticeUnit; }
            set { m_EmpTermNoticeUnit = value; modify("EmpTermNoticeUnit"); }
        }
        protected DateTime m_EmpTermLastDate;
        [DBField("EmpTermLastDate"), TextSearch, MaxLength(10), Export(false), Required]
        public DateTime EmpTermLastDate
        {
            get { return m_EmpTermLastDate; }
            set { m_EmpTermLastDate = value; modify("EmpTermLastDate"); }
        }
        protected string m_EmpTermRemark;
        [DBField("EmpTermRemark"), TextSearch, DBAESEncryptStringField,  Export(false)]
        public string EmpTermRemark
        {
            get { return m_EmpTermRemark; }
            set { m_EmpTermRemark = value; modify("EmpTermRemark"); }
        }
        protected bool m_EmpTermIsTransferCompany;
        [DBField("EmpTermIsTransferCompany"), TextSearch, Export(false)]
        public bool EmpTermIsTransferCompany
        {
            get { return m_EmpTermIsTransferCompany; }
            set { m_EmpTermIsTransferCompany = value; modify("EmpTermIsTransferCompany"); }
        }
        protected int m_NewEmpID;
        [DBField("NewEmpID"), TextSearch, Export(false)]
        public int NewEmpID
        {
            get { return m_NewEmpID; }
            set { m_NewEmpID = value; modify("NewEmpID"); }
        }

        //  For Synchronize Use only
        protected string m_SynID;
        [DBField("SynID"), TextSearch, Export(false)]
        public string SynID
        {
            get { return m_SynID; }
            set { m_SynID = value; modify("SynID"); }
        }

        public DateTime GetExpectedLastEmploymentDate()
        {
            DateTime expectedLastEmployDate = m_EmpTermResignDate;
            if (m_EmpTermNoticeUnit.Equals("D"))
                expectedLastEmployDate = expectedLastEmployDate.AddDays(m_EmpTermNoticePeriod).AddDays(-1);
            else if (m_EmpTermNoticeUnit.Equals("M"))
                expectedLastEmployDate = expectedLastEmployDate.AddMonths(m_EmpTermNoticePeriod).AddDays(-1);
            return expectedLastEmployDate;

        }
        public static EEmpTermination GetObjectByEmpID(DatabaseConnection dbConn, int EmpID)
        {
            //  Get Termination Information
            DBFilter empTerminationFilter = new DBFilter();
            empTerminationFilter.add(new Match("EmpID", EmpID));
            empTerminationFilter.add("EmpTermID", false);

            System.Collections.ArrayList empTerminationList = db.select(dbConn, empTerminationFilter);
            if (empTerminationList.Count > 0)

                return (EEmpTermination)empTerminationList[0];
            else
                return null;
        }

    }
}
