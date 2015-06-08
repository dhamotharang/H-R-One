using System;
using System.Collections.Generic;
using System.Text;
using HROne.DataAccess;
////using perspectivemind.validation;

namespace HROne.Lib.Entities
{
    [DBClass("EmpRequest")]
    public class EEmpRequest : BaseObject
    {
        public static DBManager db = new DBManager(typeof(EEmpRequest));

		// Start 0000060, Miranda, 2014-07-13
        public static WFValueList VLRequestType2 = new WFTextList(new string[] { TYPE_EEPROFILE, TYPE_EELEAVEAPP, TYPE_EELEAVECANCEL, TYPE_EEOTCLAIM, TYPE_EEOTCLAIMCANCEL });
        public static WFValueList VLRequestType = new WFTextList(new string[] { TYPE_EEPROFILE, TYPE_EELEAVEAPP, TYPE_EELEAVECANCEL });
        //public static WFValueList VLRequestType = new WFTextList(new string[] { TYPE_EEPROFILE, TYPE_EELEAVEAPP, TYPE_EELEAVECANCEL });
		// End 0000060, Miranda, 2014-07-13

        public static WFValueList VLRequestStatus = new WFTextList(new string[] { 
            STATUS_SUBMITTED, 
            STATUS_CANCELLED, 
            STATUS_ACCEPTED,
            STATUS_REJECTED,
            STATUS_APPROVED,
        });

        public const string TYPE_EEPROFILE = "EEPROFILE";
        public const string TYPE_EELEAVEAPP = "EELEAVEAPP";
        public const string TYPE_EELEAVECANCEL = "EELEAVECANCEL";
		// Start 0000060, Miranda, 2014-07-13
        public const string TYPE_EEOTCLAIM = "EEOTCLAIM";
        public const string TYPE_EEOTCLAIMCANCEL = "EEOTCLAIMCANCEL";
		// End 0000060, Miranda, 2014-07-13
        // Start 0000112, Miranda, 2014-12-10
        public const string TYPE_EELATEWAIVE = "EELATEWAIVE";
        public const string TYPE_EELATEWAIVECANCEL = "EELATEWAIVECANCEL";
        // End 0000112, Miranda, 2014-12-10
        //public const string TYPE_EELEAVE = "EELEAVE";
        //public const string STATUS_USRSUBMIT = "USRSUBMIT";
        //public const string STATUS_USRCANCEL = "USRCANCEL";
        //public const string STATUS_APPROVED = "APPROVED";
        //public const string STATUS_FSTAPP = "1STAPPROVED";
        //public const string STATUS_SNDAPP = STATUS_APPROVED;
        //public const string STATUS_FSTREJ = "1STREJECTED";
        //public const string STATUS_SNDREJ = "2NDREJECTED";

        public const string STATUS_SUBMITTED = "Submitted";
        public const string STATUS_CANCELLED = "Cancelled";
        public const string STATUS_ACCEPTED = "Accepted";
        public const string STATUS_REJECTED = "Rejected";
        public const string STATUS_APPROVED = "Approved";

        protected int m_EmpRequestID;
        [DBField("EmpRequestID", true, true), TextSearch, Export(false)]
        public int EmpRequestID
        {
            get { return m_EmpRequestID; }
            set { m_EmpRequestID = value; modify("EmpRequestID"); }
        }
        protected int m_EmpID;
        [DBField("EmpID"), TextSearch, Export(false), Required]
        public int EmpID
        {
            get { return m_EmpID; }
            set { m_EmpID = value; modify("EmpID"); }
        }
        protected string m_EmpRequestType;
        [DBField("EmpRequestType"), TextSearch, MaxLength(50, 10), Export(false)]
        public string EmpRequestType
        {
            get { return m_EmpRequestType; }
            set { m_EmpRequestType = value; modify("EmpRequestType"); }
        }
        protected int m_EmpRequestRecordID;
        [DBField("EmpRequestRecordID"), TextSearch, Export(false), Required]
        public int EmpRequestRecordID
        {
            get { return m_EmpRequestRecordID; }
            set { m_EmpRequestRecordID = value; modify("EmpRequestRecordID"); }
        }
        // Start 0000065, KuangWei, 2014-08-20
        protected DateTime m_EmpRequestFromDate;
        [DBField("EmpRequestFromDate"), TextSearch, Export(false)]
        public DateTime EmpRequestFromDate
        {
            get { return m_EmpRequestFromDate; }
            set { m_EmpRequestFromDate = value; modify("EmpRequestFromDate"); }
        }

        // Start 0000201, Ricky So, 2015-05-29
        protected string m_EmpRequestFromDateAM;
        [DBField("EmpRequestFromDateAM"), TextSearch, Export(false)]
        public string EmpRequestFromDateAM
        {
            get { return m_EmpRequestFromDateAM; }
            set { m_EmpRequestFromDateAM = value; modify("EmpRequestFromDateAM"); }
        }
        // End 0000201, Ricky So, 2015-05-29

        protected DateTime m_EmpRequestToDate;
        [DBField("EmpRequestToDate"), TextSearch, Export(false)]
        public DateTime EmpRequestToDate
        {
            get { return m_EmpRequestToDate; }
            set { m_EmpRequestToDate = value; modify("EmpRequestToDate"); }
        }

        // Start 0000201, Ricky So, 2015-05-29
        protected string m_EmpRequestToDateAM;
        [DBField("EmpRequestToDateAM"), TextSearch, Export(false)]
        public string EmpRequestToDateAM
        {
            get { return m_EmpRequestToDateAM; }
            set { m_EmpRequestToDateAM = value; modify("EmpRequestToDateAM"); }
        }
        // End 0000201, Ricky So, 2015-05-29
        
        protected string m_EmpRequestDuration;
        [DBField("EmpRequestDuration"), TextSearch, Export(false)]
        public string EmpRequestDuration
        {
            get { return m_EmpRequestDuration; }
            set { m_EmpRequestDuration = value; modify("EmpRequestDuration"); }
        }
        // End 0000065, KuangWei, 2014-08-20
        protected DateTime m_EmpRequestCreateDate;
        [DBField("EmpRequestCreateDate"), TextSearch, Export(false)]
        public DateTime EmpRequestCreateDate
        {
            get { return m_EmpRequestCreateDate; }
            set { m_EmpRequestCreateDate = value; modify("EmpRequestCreateDate"); }
        }
        protected DateTime m_EmpRequestModifyDate;
        [DBField("EmpRequestModifyDate"), TextSearch, Export(false)]
        public DateTime EmpRequestModifyDate
        {
            get { return m_EmpRequestModifyDate; }
            set { m_EmpRequestModifyDate = value; modify("EmpRequestModifyDate"); }
        }
        //protected string m_EmpRequestStatus;
        //[DBField("EmpRequestStatus"), TextSearch, MaxLength(50, 10), Export(false)]
        //public string EmpRequestStatus
        //{
        //    get { return m_EmpRequestStatus; }
        //    set { m_EmpRequestStatus = value; modify("EmpRequestStatus"); }
        //}

        protected string m_EmpRequestStatus;
        [DBField("EmpRequestStatus"), TextSearch, Export(false)]
        public string EmpRequestStatus
        {
            get { return m_EmpRequestStatus; }
            set { m_EmpRequestStatus = value; modify("EmpRequestStatus"); }
        }
        protected int m_EmpRequestLastAuthorizationWorkFlowIndex = 0;
        [DBField("EmpRequestLastAuthorizationWorkFlowIndex"), TextSearch, Export(false)]
        public int EmpRequestLastAuthorizationWorkFlowIndex
        {
            get { return m_EmpRequestLastAuthorizationWorkFlowIndex; }
            set { m_EmpRequestLastAuthorizationWorkFlowIndex = value; modify("EmpRequestLastAuthorizationWorkFlowIndex"); }
        }

        protected string m_EmpRequestRejectReason;
        [DBField("EmpRequestRejectReason"), TextSearch, MaxLength(50, 10), Export(false)]
        public string EmpRequestRejectReason
        {
            get { return m_EmpRequestRejectReason; }
            set { m_EmpRequestRejectReason = value; modify("EmpRequestRejectReason"); }
        }

        protected string m_EmpRequestLastActionBy;
        [DBField("EmpRequestLastActionBy"), TextSearch, Export(false)]
        public string EmpRequestLastActionBy
        {
            get { return m_EmpRequestLastActionBy; }
            set { m_EmpRequestLastActionBy = value; modify("EmpRequestLastActionBy"); }
        }
        protected int m_EmpRequestLastActionByEmpID;
        [DBField("EmpRequestLastActionByEmpID"), TextSearch, Export(false)]
        public int EmpRequestLastActionByEmpID
        {
            get { return m_EmpRequestLastActionByEmpID; }
            set { m_EmpRequestLastActionByEmpID = value; modify("EmpRequestLastActionByEmpID"); }
        }

        public static DBTerm EndStatusDBTerms(string fieldName, bool notEndStatus)
        {
            IN inEndStatusDBTerms = new IN((notEndStatus ? "NOT " : string.Empty) + fieldName, "'" + string.Join("','", new string[] { STATUS_APPROVED, STATUS_REJECTED, STATUS_CANCELLED }) + "'", null);
            return inEndStatusDBTerms;
        }

        protected override void beforeUpdate(DatabaseConnection dbConn, DBManager db)
        {
            EmpRequestModifyDate = AppUtils.ServerDateTime();
            base.beforeUpdate(dbConn, db);
        }

        protected override void afterUpdate(DatabaseConnection dbConn, DBManager db)
        {
            EEmpRequest prevEmpRequest = (EEmpRequest)oldValueObject;
            EEmpRequestApprovalHistory empRequestApprovalHistory = new EEmpRequestApprovalHistory();
            empRequestApprovalHistory.EmpRequestID = m_EmpRequestID;
            empRequestApprovalHistory.EmpRequestApprovalHistoryCreateDateTime = EmpRequestModifyDate;
            empRequestApprovalHistory.EmpRequestApprovalHistoryActionBy = m_EmpRequestLastActionBy;
            empRequestApprovalHistory.EmpRequestApprovalHistoryActionByEmpID = m_EmpRequestLastActionByEmpID;
            empRequestApprovalHistory.EmpRequestApprovalHistoryAuthorizationWorkFlowIndexBefore = prevEmpRequest.EmpRequestLastAuthorizationWorkFlowIndex;
            empRequestApprovalHistory.EmpRequestApprovalHistoryStatusBefore = prevEmpRequest.EmpRequestStatus;
            empRequestApprovalHistory.EmpRequestApprovalHistoryAuthorizationWorkFlowIndexAfter = m_EmpRequestLastAuthorizationWorkFlowIndex; 
            empRequestApprovalHistory.EmpRequestApprovalHistoryStatusAfter = m_EmpRequestStatus;

            base.afterUpdate(dbConn, db);

            EEmpRequestApprovalHistory.db.insert(dbConn, empRequestApprovalHistory);
        }
    }
}
