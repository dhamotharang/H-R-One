using System;
using System.Collections.Generic;
using System.Text;
using HROne.DataAccess;
////using perspectivemind.validation;

namespace HROne.Lib.Entities
{
    [DBClass("EmpRequestApprovalHistory")]
    public class EEmpRequestApprovalHistory : BaseObject
    {
        public static DBManager db = new DBManager(typeof(EEmpRequestApprovalHistory));

        protected int m_EmpRequestApprovalHistoryID;
        [DBField("EmpRequestApprovalHistoryID", true, true), TextSearch, Export(false)]
        public int EmpRequestApprovalHistoryID
        {
            get { return m_EmpRequestApprovalHistoryID; }
            set { m_EmpRequestApprovalHistoryID = value; modify("EmpRequestApprovalHistoryID"); }
        }
        protected int m_EmpRequestID;
        [DBField("EmpRequestID"), TextSearch, Export(false), Required]
        public int EmpRequestID
        {
            get { return m_EmpRequestID; }
            set { m_EmpRequestID = value; modify("EmpRequestID"); }
        }
        protected DateTime m_EmpRequestApprovalHistoryCreateDateTime;
        [DBField("EmpRequestApprovalHistoryCreateDateTime"), TextSearch, Export(false)]
        public DateTime EmpRequestApprovalHistoryCreateDateTime
        {
            get { return m_EmpRequestApprovalHistoryCreateDateTime; }
            set { m_EmpRequestApprovalHistoryCreateDateTime = value; modify("EmpRequestApprovalHistoryCreateDateTime"); }
        }

        protected string m_EmpRequestApprovalHistoryStatusBefore;
        [DBField("EmpRequestApprovalHistoryStatusBefore"), TextSearch, Export(false)]
        public string EmpRequestApprovalHistoryStatusBefore
        {
            get { return m_EmpRequestApprovalHistoryStatusBefore; }
            set { m_EmpRequestApprovalHistoryStatusBefore = value; modify("EmpRequestApprovalHistoryStatusBefore"); }
        }

        protected int m_EmpRequestApprovalHistoryAuthorizationWorkFlowIndexBefore = 0;
        [DBField("EmpRequestApprovalHistoryAuthorizationWorkFlowIndexBefore"), TextSearch, Export(false)]
        public int EmpRequestApprovalHistoryAuthorizationWorkFlowIndexBefore
        {
            get { return m_EmpRequestApprovalHistoryAuthorizationWorkFlowIndexBefore; }
            set { m_EmpRequestApprovalHistoryAuthorizationWorkFlowIndexBefore = value; modify("EmpRequestApprovalHistoryAuthorizationWorkFlowIndexBefore"); }
        }

        protected string m_EmpRequestApprovalHistoryStatusAfter;
        [DBField("EmpRequestApprovalHistoryStatusAfter"), TextSearch, Export(false)]
        public string EmpRequestApprovalHistoryStatusAfter
        {
            get { return m_EmpRequestApprovalHistoryStatusAfter; }
            set { m_EmpRequestApprovalHistoryStatusAfter = value; modify("EmpRequestApprovalHistoryStatusAfter"); }
        }

        protected int m_EmpRequestApprovalHistoryAuthorizationWorkFlowIndexAfter = 0;
        [DBField("EmpRequestApprovalHistoryAuthorizationWorkFlowIndexAfter"), TextSearch, Export(false)]
        public int EmpRequestApprovalHistoryAuthorizationWorkFlowIndexAfter
        {
            get { return m_EmpRequestApprovalHistoryAuthorizationWorkFlowIndexAfter; }
            set { m_EmpRequestApprovalHistoryAuthorizationWorkFlowIndexAfter = value; modify("EmpRequestApprovalHistoryAuthorizationWorkFlowIndexAfter"); }
        }

        protected string m_EmpRequestApprovalHistoryActionBy;
        [DBField("EmpRequestApprovalHistoryActionBy"), TextSearch, Export(false)]
        public string EmpRequestApprovalHistoryActionBy
        {
            get { return m_EmpRequestApprovalHistoryActionBy; }
            set { m_EmpRequestApprovalHistoryActionBy = value; modify("EmpRequestApprovalHistoryActionBy"); }
        }
        protected int m_EmpRequestApprovalHistoryActionByEmpID;
        [DBField("EmpRequestApprovalHistoryActionByEmpID"), TextSearch, Export(false)]
        public int EmpRequestApprovalHistoryActionByEmpID
        {
            get { return m_EmpRequestApprovalHistoryActionByEmpID; }
            set { m_EmpRequestApprovalHistoryActionByEmpID = value; modify("EmpRequestApprovalHistoryActionByEmpID"); }
        }

    }
}
