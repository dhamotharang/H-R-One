using System;
using System.Collections.Generic;
using System.Text;
using HROne.DataAccess;
////using perspectivemind.validation;

namespace HROne.Lib.Entities
{
    [DBClass("EmpPermit")]
    public class EEmpPermit : BaseObjectWithRecordInfo
    {
        public static DBManager db = new DBManagerWithRecordInfo(typeof(EEmpPermit));


        protected int m_EmpPermitID;
        [DBField("EmpPermitID", true, true), TextSearch, Export(false)]
        public int EmpPermitID
        {
            get { return m_EmpPermitID; }
            set { m_EmpPermitID = value; modify("EmpPermitID"); }
        }
        protected int m_EmpID;
        [DBField("EmpID"), TextSearch, Export(false)]
        public int EmpID
        {
            get { return m_EmpID; }
            set { m_EmpID = value; modify("EmpID"); }
        }
        protected int m_PermitTypeID;
        [DBField("PermitTypeID"), TextSearch, Export(false), Required]
        public int PermitTypeID
        {
            get { return m_PermitTypeID; }
            set { m_PermitTypeID = value; modify("PermitTypeID"); }
        }
        protected string m_EmpPermitNo;
        [DBField("EmpPermitNo"), TextSearch, MaxLength(50, 20), Export(false)]
        public string EmpPermitNo
        {
            get { return m_EmpPermitNo; }
            set { m_EmpPermitNo = value; modify("EmpPermitNo"); }
        }

        protected DateTime m_EmpPermitIssueDate;
        [DBField("EmpPermitIssueDate"), TextSearch, MaxLength(11), Export(false)]
        public DateTime EmpPermitIssueDate
        {
            get { return m_EmpPermitIssueDate; }
            set { m_EmpPermitIssueDate = value; modify("EmpPermitIssueDate"); }
        }
        protected DateTime m_EmpPermitExpiryDate;
        [DBField("EmpPermitExpiryDate"), TextSearch, MaxLength(11), Export(false)]
        public DateTime EmpPermitExpiryDate
        {
            get { return m_EmpPermitExpiryDate; }
            set { m_EmpPermitExpiryDate = value; modify("EmpPermitExpiryDate"); }
        }
        protected string m_EmpPermitRemark;
        [DBField("EmpPermitRemark"), TextSearch, Export(false)]
        public string EmpPermitRemark
        {
            get { return m_EmpPermitRemark; }
            set { m_EmpPermitRemark = value; modify("EmpPermitRemark"); }
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
