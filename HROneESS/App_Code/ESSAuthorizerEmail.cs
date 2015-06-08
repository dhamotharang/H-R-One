using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using HROne.DataAccess;
//using perspectivemind.validation;
using HROne.Lib.Entities;
using System.Net.Mail;

public class AuthorizerMailAddress
{
    public string Address;
    public string DisplayName;

    public AuthorizerMailAddress(string Address, string DisplayName)
    {
        this.Address = Address;
        this.DisplayName = DisplayName;
    }
}

/// <summary>
/// Summary description for ESSAuthorizerEmail
/// </summary>
public class ESSAuthorizationProcess
{
    protected DatabaseConnection dbConn;
    public ESSAuthorizationProcess(DatabaseConnection dbConn)
    {
        this.dbConn = dbConn;
    }

    public bool IsPrimaryAuthorizer(int AuthorizerEmpID)
    {
        DBFilter authorizerFilter = new DBFilter();

        authorizerFilter.add(new Match("EmpID", AuthorizerEmpID));
        return (EAuthorizer.db.count(dbConn, authorizerFilter) > 0);
    }
    public List<EAuthorizationGroup> GetAuthorizerAuthorizationGroupList(int AuthorizerEmpID)
    {
        DBFilter authorizerFilter = new DBFilter();

        OR orDelegateEmpIDTerms = new OR();
        orDelegateEmpIDTerms.add(new Match("auther.EmpID", AuthorizerEmpID));
        DBFilter authorizerDelegateFilter = new DBFilter();
        authorizerDelegateFilter.add(new Match("ad.AuthorizerDelegateEmpID", AuthorizerEmpID));
        orDelegateEmpIDTerms.add(new IN("auther.EmpID", "SELECT ad.EmpID FROM " + EAuthorizerDelegate.db.dbclass.tableName + " ad", authorizerDelegateFilter));
        authorizerFilter.add(orDelegateEmpIDTerms);

        DBFilter authorizationFilter = new DBFilter();
        authorizationFilter.add(new IN("AuthorizationGroupID", "SELECT DISTINCT auther.AuthorizationGroupID FROM " + EAuthorizer.db.dbclass.tableName + " auther", authorizerFilter));
        ArrayList list = EAuthorizationGroup.db.select(dbConn, authorizationFilter);
        return new List<EAuthorizationGroup>((EAuthorizationGroup[])list.ToArray(typeof(EAuthorizationGroup)));
    }
    public EAuthorizationWorkFlowDetail GetCurrentWorkFlowDetailObject(ArrayList workflowDetailList, int LastActionWorkflowIndex, int AuthorizerEmpID)
    {
        foreach (EAuthorizationWorkFlowDetail workFlowDetail in workflowDetailList)
        {
            //  Check next authorization group that match with authorizer
            if (LastActionWorkflowIndex < workFlowDetail.AuthorizationWorkFlowIndex
            || (LastActionWorkflowIndex >= workFlowDetail.AuthorizationWorkFlowIndex && workFlowDetail.AuthorizationWorkFlowIndex == ((EAuthorizationWorkFlowDetail)workflowDetailList[workflowDetailList.Count - 1]).AuthorizationWorkFlowIndex))
            {
                if (workFlowDetail.GetActualAutorizerObjectList(dbConn, AuthorizerEmpID).Count > 0)

                    return workFlowDetail;
            }
        }
        return null;
    }
    public int GetAuthorizationWorkflowID(int ApplicantEmpID, string RequestType)
    {
        EEmpPositionInfo EmpPosition = AppUtils.GetLastPositionInfo(dbConn, AppUtils.ServerDateTime().Date, ApplicantEmpID);
        if (EmpPosition != null)
            if (RequestType.Equals(EEmpRequest.TYPE_EELEAVEAPP) || RequestType.Equals(EEmpRequest.TYPE_EELEAVECANCEL))
                return EmpPosition.AuthorizationWorkFlowIDLeaveApp;
            else if (RequestType.Equals(EEmpRequest.TYPE_EEPROFILE))
                return EmpPosition.AuthorizationWorkFlowIDEmpInfoModified;
            // Start 0000060, Miranda, 2014-07-13
            else if (RequestType.Equals(EEmpRequest.TYPE_EEOTCLAIM) || RequestType.Equals(EEmpRequest.TYPE_EEOTCLAIMCANCEL))
                return EmpPosition.AuthorizationWorkFlowIDOTClaims;
            // End 0000060, Miranda, 2014-07-13
            // Start 0000112, Miranda, 2014-12-10
            else if (RequestType.Equals(EEmpRequest.TYPE_EELATEWAIVE) || RequestType.Equals(EEmpRequest.TYPE_EELATEWAIVECANCEL))
                return EmpPosition.AuthorizationWorkFlowIDLateWaive;
            // End 0000112, Miranda, 2014-12-10
        return 0;
    }

    public ArrayList GetAuthorizationWorkFlowDetailList(int ApplicantEmpID, string RequestType)
    {
        int AuthorizationWorkFlowID = GetAuthorizationWorkflowID(ApplicantEmpID, RequestType);
        DBFilter AuthorizationWorkFlowDetailFilter = new DBFilter();
        AuthorizationWorkFlowDetailFilter.add(new Match("AuthorizationWorkFlowID", AuthorizationWorkFlowID));
        AuthorizationWorkFlowDetailFilter.add("AuthorizationWorkFlowIndex", true);
        return EAuthorizationWorkFlowDetail.db.select(dbConn, AuthorizationWorkFlowDetailFilter);

    }
    public System.Collections.Generic.List<AuthorizerMailAddress> GetAuthorizerEmailAddressList(int AuthorizationGroupID)
    {
        System.Collections.Generic.List<AuthorizerMailAddress> mailAddressList = new System.Collections.Generic.List<AuthorizerMailAddress>();

        EAuthorizationGroup group = new EAuthorizationGroup();
        group.AuthorizationGroupID = AuthorizationGroupID;
        if (EAuthorizationGroup.db.select(dbConn, group))
        {
            DBFilter filterAuthorizer = new DBFilter();
            filterAuthorizer.add(new Match("AuthorizationGroupID", AuthorizationGroupID));
            filterAuthorizer.add(new Match("AuthorizerSkipEmailAlert", false));

            ArrayList EmpAuthorizerList = EAuthorizer.db.select(dbConn, filterAuthorizer);
            foreach (EAuthorizer authorizer in EmpAuthorizerList)
            {
                DBFilter authorizerFilter = new DBFilter();
                OR orDelegateEmpIDTerms = new OR();
                orDelegateEmpIDTerms.add(new Match("EmpID", authorizer.EmpID));
                DBFilter authorizerDelegateFilter = new DBFilter();
                authorizerDelegateFilter.add(new Match("ad.EmpID", authorizer.EmpID));
                orDelegateEmpIDTerms.add(new IN("EmpID", "SELECT ad.AuthorizerDelegateEmpID FROM " + EAuthorizerDelegate.db.dbclass.tableName + " ad", authorizerDelegateFilter));
                authorizerFilter.add(orDelegateEmpIDTerms);

                ArrayList AuthorizerEmpInfoList = EEmpPersonalInfo.db.select(dbConn, authorizerFilter);
                foreach (EEmpPersonalInfo AuthorizerEmpInfo in AuthorizerEmpInfoList)
                {
                    if (!AuthorizerEmpInfo.EmpInternalEmail.Trim().Equals(string.Empty))
                        mailAddressList.Add(new AuthorizerMailAddress(AuthorizerEmpInfo.EmpInternalEmail, AuthorizerEmpInfo.EmpEngDisplayName));
                    else if (!AuthorizerEmpInfo.EmpEmail.Trim().Equals(string.Empty))
                        mailAddressList.Add(new AuthorizerMailAddress(AuthorizerEmpInfo.EmpEmail, AuthorizerEmpInfo.EmpEngDisplayName));
                }
            }
            if (!string.IsNullOrEmpty(group.AuthorizationGroupEmailAddress))
            {
                string[] groupMailAddressList = group.AuthorizationGroupEmailAddress.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (string groupMail in groupMailAddressList)
                    mailAddressList.Add(new AuthorizerMailAddress(groupMail, string.Empty));
            }
        }
        return mailAddressList;
    }

    public EEmpPersonalInfo GetEmpInfo(int EmpID)
    {
        DBManager EmpInfoDB = EEmpPersonalInfo.db;
        EEmpPersonalInfo obj = new EEmpPersonalInfo();
        obj.EmpID = EmpID;
        if (EmpInfoDB.select(dbConn, obj))
            return obj;
        else
            return obj;
    }
    public EEmpPersonalInfo GetEmpInfo(string EmpNo)
    {
        EmpNo = EmpNo.Trim().ToUpper();

        DBFilter dbfilter = new DBFilter();
        OR orEmpNo = new OR();
        orEmpNo.add(new Match("EmpNo", EmpNo));
        DBFieldTranscoder transcoder = EEmpPersonalInfo.db.getField("EmpNo").transcoder;
        if (transcoder != null)
            orEmpNo.add(new Match("EmpNo", transcoder.toDB(EmpNo)));

        dbfilter.add(orEmpNo);

        ArrayList list = EEmpPersonalInfo.db.select(dbConn, dbfilter);
        if (list.Count > 0)
            return ((EEmpPersonalInfo)list[0]);
        else
            return null;
    }
    public EAuthorizationGroup GetAuthorizationGroup(int AuthorizationGroupID)
    {
        DBManager AuthorizationGroupDB = EAuthorizationGroup.db;
        EAuthorizationGroup obj = new EAuthorizationGroup();
        obj.AuthorizationGroupID = AuthorizationGroupID;
        if (AuthorizationGroupDB.select(dbConn, obj))
            return obj;
        else
            return obj;
    }

    private string GenerateLeaveDateTimeString(ERequestLeaveApplication requestLeaveApp)
    {
        if (requestLeaveApp.RequestLeaveAppDateFrom.Date.Equals(requestLeaveApp.RequestLeaveAppDateTo.Date))
        {
            string requestLeaveAppDateTimeString = requestLeaveApp.RequestLeaveAppDateFrom.ToString("yyyy-MM-dd");
            if (!(requestLeaveApp.RequestLeaveAppTimeFrom.Ticks.Equals(0) && requestLeaveApp.RequestLeaveAppTimeTo.Ticks.Equals(0)))
                requestLeaveAppDateTimeString += " " + requestLeaveApp.RequestLeaveAppTimeFrom.ToString("HH:mm") + " TO " + requestLeaveApp.RequestLeaveAppTimeTo.ToString("HH:mm");

            return requestLeaveAppDateTimeString;
        }
        else
        {
            return requestLeaveApp.RequestLeaveAppDateFrom.ToString("yyyy-MM-dd") +
                   ((!string.IsNullOrEmpty(requestLeaveApp.RequestLeaveAppDateFromAM)) ? requestLeaveApp.RequestLeaveAppDateFromAM : "") +
                   " TO " +
                   requestLeaveApp.RequestLeaveAppDateTo.ToString("yyyy-MM-dd") +
                   ((!string.IsNullOrEmpty(requestLeaveApp.RequestLeaveAppDateToAM)) ? requestLeaveApp.RequestLeaveAppDateToAM : "");
        }
    }

    public Hashtable GenerateEmpInfoHashTable(EEmpPersonalInfo CurrentEmpInfo, Hashtable originalTable)
    {
        originalTable.Add("%EMP_NO%", CurrentEmpInfo.EmpNo);
        originalTable.Add("%EMP_NAME%", CurrentEmpInfo.EmpEngFullNameWithAlias);
        return originalTable;
    }

    public Hashtable GenerateRequestLeaveApplicationHashTable(ERequestLeaveApplication requestLeaveApp, Hashtable originalTable)
    {

        originalTable.Add("%LEAVEAPP_PERIOD%", GenerateLeaveDateTimeString(requestLeaveApp));

        ELeaveCode leaveCode = new ELeaveCode();
        leaveCode.LeaveCodeID = requestLeaveApp.RequestLeaveCodeID;
        if (ELeaveCode.db.select(dbConn, leaveCode))
        {
            // Start 0000080, KuangWei, 2014-09-16
            originalTable.Add("%LEAVEAPP_CODE%", leaveCode.LeaveCode);
            // End 0000080, KuangWei, 2014-09-16
            originalTable.Add("%LEAVE_CODE%", leaveCode.LeaveCodeDesc);
            if (leaveCode.LeaveTypeID.Equals(ELeaveType.COMPENSATION_LEAVE_TYPE(dbConn).LeaveTypeID))
                originalTable.Add("%LEAVEAPP_HOURSCLAIM%", requestLeaveApp.RequestLeaveAppHours);
        }
        // Start 0000080, KuangWei, 2014-09-17
        if (requestLeaveApp.RequestLeaveAppUnit.Equals("D"))
            originalTable.Add("%LEAVEAPP_UNIT%", "Day(s)");
        else if (requestLeaveApp.RequestLeaveAppUnit.Equals("H"))
            originalTable.Add("%LEAVEAPP_UNIT%", "Hour(s)");
        // End 0000080, KuangWei, 2014-09-17
        else if (requestLeaveApp.RequestLeaveAppUnit.Equals("A"))
            originalTable.Add("%LEAVEAPP_UNIT%", "A.M.");
        else if (requestLeaveApp.RequestLeaveAppUnit.Equals("P"))
            originalTable.Add("%LEAVEAPP_UNIT%", "P.M.");

        // Start 0000094, Ricky So, 2014-09-10
        if (requestLeaveApp.RequestLeaveAppUnit.Equals("H"))
            originalTable.Add("%LEAVEAPP_DAYS%", requestLeaveApp.RequestLeaveAppHours);
        else
            originalTable.Add("%LEAVEAPP_DAYS%", requestLeaveApp.RequestLeaveDays);

        // originalTable.Add("%LEAVEAPP_DAYS%", requestLeaveApp.RequestLeaveDays);
        // End 0000094, Ricky So, 2014-09-10
        originalTable.Add("%LEAVEAPP_REMARK%", string.IsNullOrEmpty(requestLeaveApp.RequestLeaveAppRemark) ? string.Empty : requestLeaveApp.RequestLeaveAppRemark);

        return originalTable;
    }

    private Hashtable GenerateRequestRedirectLinkHashTable(EEmpRequest empRequest, Hashtable originalTable)
    {
        Uri uri = (Uri)new Page().Session["CurrentURI"];

        originalTable.Add("%REQUEST_URL%", uri.GetLeftPart(UriPartial.Authority) + VirtualPathUtility.ToAbsolute("~/ESS_EmpRequestDetail.aspx") + "?TargetEmpID=" + empRequest.EmpID + "&EmpRequestRecordID=" + empRequest.EmpRequestRecordID + "&EmpRequestID=" + empRequest.EmpRequestID);
        return originalTable;
    }
    private List<AuthorizerMailAddress> GetAllAuthorizerEmailAddressList(ArrayList workflowDetailList, int LastActionWorkflowIndex, int CurrentActoinWorkFlowIndex, bool MandatoryEMailListOnNextGroupAuthorizer)
    {
        List<AuthorizerMailAddress> beforeOtherGroupAlertAuthorizerMailAddressList = null;
        List<AuthorizerMailAddress> currentGroupAuthorizerMailAddressList = null;
        List<AuthorizerMailAddress> nextGroupAuthorizerMailAddressList = null;
        List<AuthorizerMailAddress> afterOtherGroupAlertAuthorizerMailAddressList = null;
        GetAllAuthorizerEmailAddressList(workflowDetailList, LastActionWorkflowIndex, CurrentActoinWorkFlowIndex, MandatoryEMailListOnNextGroupAuthorizer,
            out beforeOtherGroupAlertAuthorizerMailAddressList,
            out currentGroupAuthorizerMailAddressList,
            out nextGroupAuthorizerMailAddressList,
            out afterOtherGroupAlertAuthorizerMailAddressList);

        List<AuthorizerMailAddress> resultAuthorizerMailAddressList = new List<AuthorizerMailAddress>();
        resultAuthorizerMailAddressList.AddRange(beforeOtherGroupAlertAuthorizerMailAddressList);
        resultAuthorizerMailAddressList.AddRange(currentGroupAuthorizerMailAddressList);
        resultAuthorizerMailAddressList.AddRange(nextGroupAuthorizerMailAddressList);
        resultAuthorizerMailAddressList.AddRange(afterOtherGroupAlertAuthorizerMailAddressList);
        return resultAuthorizerMailAddressList;
    }
    private void GetAllAuthorizerEmailAddressList(ArrayList workflowDetailList, int LastActionWorkflowIndex, int CurrentActoinWorkFlowIndex, bool MandatoryEMailListOnNextGroupAuthorizer,
        out List<AuthorizerMailAddress> beforeOtherGroupAlertAuthorizerMailAddressList,
        out List<AuthorizerMailAddress> currentGroupAuthorizerMailAddressList,
        out List<AuthorizerMailAddress> nextGroupAuthorizerMailAddressList,
        out List<AuthorizerMailAddress> afterOtherGroupAlertAuthorizerMailAddressList)
    {
        beforeOtherGroupAlertAuthorizerMailAddressList = new List<AuthorizerMailAddress>();
        currentGroupAuthorizerMailAddressList = new List<AuthorizerMailAddress>();
        nextGroupAuthorizerMailAddressList = new List<AuthorizerMailAddress>();
        afterOtherGroupAlertAuthorizerMailAddressList = new List<AuthorizerMailAddress>();
        int currentActionWorkFlowIndex = 0;
        int nextActionWorkFlowIndex = 0;
        foreach (EAuthorizationWorkFlowDetail workFlowDetail in workflowDetailList)
        {
            //  Check next authorization group and always e-mail to next authorization
            if (currentActionWorkFlowIndex <= 0)
            {
                if (LastActionWorkflowIndex < workFlowDetail.AuthorizationWorkFlowIndex
                || (LastActionWorkflowIndex >= workFlowDetail.AuthorizationWorkFlowIndex && workFlowDetail.AuthorizationWorkFlowIndex == ((EAuthorizationWorkFlowDetail)workflowDetailList[workflowDetailList.Count - 1]).AuthorizationWorkFlowIndex))
                {
                    currentGroupAuthorizerMailAddressList.AddRange(GetAuthorizerEmailAddressList(workFlowDetail.AuthorizationGroupID));

                    if (workFlowDetail.AuthorizationWorkFlowIndex >= CurrentActoinWorkFlowIndex)
                        currentActionWorkFlowIndex = workFlowDetail.AuthorizationWorkFlowIndex;
                    continue;
                }
            }
            else if (nextActionWorkFlowIndex <= 0)
            {
                nextActionWorkFlowIndex = workFlowDetail.AuthorizationWorkFlowIndex;

                EAuthorizationGroup alertAuthorizationGroup = GetAuthorizationGroup(workFlowDetail.AuthorizationGroupID);
                if (alertAuthorizationGroup != null)
                    if (alertAuthorizationGroup.AuthorizationGroupIsReceiveOtherGrpAlert || MandatoryEMailListOnNextGroupAuthorizer)
                        nextGroupAuthorizerMailAddressList.AddRange(GetAuthorizerEmailAddressList(alertAuthorizationGroup.AuthorizationGroupID));
                continue;
            }
            //  e-mail to next authorization only if AuthorizationGroupIsReceiveOtherGrpAlert=true
            if (currentActionWorkFlowIndex != workFlowDetail.AuthorizationWorkFlowIndex && nextActionWorkFlowIndex != workFlowDetail.AuthorizationWorkFlowIndex)
            {
                EAuthorizationGroup alertAuthorizationGroup = GetAuthorizationGroup(workFlowDetail.AuthorizationGroupID);
                if (alertAuthorizationGroup != null)
                    if (alertAuthorizationGroup.AuthorizationGroupIsReceiveOtherGrpAlert)
                        if (LastActionWorkflowIndex <= workFlowDetail.AuthorizationWorkFlowIndex)
                            beforeOtherGroupAlertAuthorizerMailAddressList.AddRange(GetAuthorizerEmailAddressList(alertAuthorizationGroup.AuthorizationGroupID));
                        else
                            afterOtherGroupAlertAuthorizerMailAddressList.AddRange(GetAuthorizerEmailAddressList(alertAuthorizationGroup.AuthorizationGroupID));
            }
        }
    }

    public void CancelAction(EEmpRequest empRequest)
    {

        EEmpPersonalInfo ApplicantEmpInfo = GetEmpInfo(empRequest.EmpID);

        empRequest.EmpRequestStatus = EEmpRequest.STATUS_CANCELLED;
        empRequest.EmpRequestLastActionBy = ApplicantEmpInfo.EmpEngFullNameWithAlias;
        empRequest.EmpRequestLastActionByEmpID = ApplicantEmpInfo.EmpID;
        EEmpRequest.db.update(dbConn, empRequest);


        Hashtable mailContentParameterTable = new Hashtable();
        mailContentParameterTable = GenerateEmpInfoHashTable(ApplicantEmpInfo, mailContentParameterTable);

        if (empRequest.EmpRequestType == EEmpRequest.TYPE_EELEAVEAPP)
        {
            DBFilter RequestEmpFilter = new DBFilter();
            RequestEmpFilter.add(new Match("RequestLeaveAppID", empRequest.EmpRequestRecordID));
            ArrayList empRequestList = ERequestLeaveApplication.db.select(dbConn, RequestEmpFilter);
            if (empRequestList.Count > 0)
            {
                ERequestLeaveApplication empLeaveRequest = (ERequestLeaveApplication)empRequestList[0];

                mailContentParameterTable = GenerateRequestLeaveApplicationHashTable(empLeaveRequest, mailContentParameterTable);
            }
        }
        else if (empRequest.EmpRequestType == EEmpRequest.TYPE_EELEAVECANCEL)
        {
            DBFilter RequestEmpFilter = new DBFilter();
            RequestEmpFilter.add(new Match("RequestLeaveAppCancelID", empRequest.EmpRequestRecordID));
            ArrayList empRequestList = ERequestLeaveApplicationCancel.db.select(dbConn, RequestEmpFilter);
            if (empRequestList.Count > 0)
            {
                ERequestLeaveApplicationCancel requestLeaveAppCancel = (ERequestLeaveApplicationCancel)empRequestList[0];
                mailContentParameterTable.Add("%LEAVEAPPCANCEL_REMARK%", requestLeaveAppCancel.RequestLeaveAppCancelReason);
                ELeaveApplication leaveApp = new ELeaveApplication();
                leaveApp.LeaveAppID = requestLeaveAppCancel.LeaveAppID;
                if (ELeaveApplication.db.select(dbConn, leaveApp))
                {
                    ERequestLeaveApplication dummyRequestLeave = ERequestLeaveApplication.CreateDummyRequestLeaveAppliction(leaveApp);
                    mailContentParameterTable = GenerateRequestLeaveApplicationHashTable(dummyRequestLeave, mailContentParameterTable);
                }
            }
        }
        // Start 0000060, Miranda, 2014-07-13
        if (empRequest.EmpRequestType == EEmpRequest.TYPE_EEOTCLAIM)
        {
            DBFilter RequestEmpFilter = new DBFilter();
            RequestEmpFilter.add(new Match("RequestOTClaimID", empRequest.EmpRequestRecordID));
            ArrayList empRequestList = ERequestOTClaim.db.select(dbConn, RequestEmpFilter);
            if (empRequestList.Count > 0)
            {
                ERequestOTClaim empOTRequest = (ERequestOTClaim)empRequestList[0];

                mailContentParameterTable = GenerateRequestOTClaimHashTable(empOTRequest, mailContentParameterTable);
            }
        }
        else if (empRequest.EmpRequestType == EEmpRequest.TYPE_EEOTCLAIMCANCEL)
        {
            DBFilter RequestEmpFilter = new DBFilter();
            RequestEmpFilter.add(new Match("RequestOTClaimCancelID", empRequest.EmpRequestRecordID));
            ArrayList empRequestList = ERequestOTClaimCancel.db.select(dbConn, RequestEmpFilter);
            if (empRequestList.Count > 0)
            {
                ERequestOTClaimCancel requestOTClaimCancel = (ERequestOTClaimCancel)empRequestList[0];
                mailContentParameterTable.Add("%OTCLAIMCANCEL_REMARK%", requestOTClaimCancel.RequestOTClaimCancelReason);
                EOTClaim OTClaim = new EOTClaim();
                OTClaim.OTClaimID = requestOTClaimCancel.OTClaimID;
                if (EOTClaim.db.select(dbConn, OTClaim))
                {
                    ERequestOTClaim dummyRequestOT = ERequestOTClaim.CreateDummyRequestOTClaim(OTClaim);
                    mailContentParameterTable = GenerateRequestOTClaimHashTable(dummyRequestOT, mailContentParameterTable);
                }
            }
        }
        // End 0000060, Miranda, 2014-07-13
        // Start 0000112, Miranda, 2014-12-10
        if (empRequest.EmpRequestType == EEmpRequest.TYPE_EELATEWAIVE)
        {
            DBFilter RequestEmpFilter = new DBFilter();
            RequestEmpFilter.add(new Match("RequestLateWaiveID", empRequest.EmpRequestRecordID));
            ArrayList empRequestList = ERequestLateWaive.db.select(dbConn, RequestEmpFilter);
            if (empRequestList.Count > 0)
            {
                ERequestLateWaive empLateWaiveRequest = (ERequestLateWaive)empRequestList[0];

                mailContentParameterTable = GenerateRequestLateWaiveHashTable(empLateWaiveRequest, mailContentParameterTable);
            }
        }
        else if (empRequest.EmpRequestType == EEmpRequest.TYPE_EELATEWAIVECANCEL)
        {
            DBFilter RequestEmpFilter = new DBFilter();
            RequestEmpFilter.add(new Match("RequestLateWaiveCancelID", empRequest.EmpRequestRecordID));
            ArrayList empRequestList = ERequestLateWaiveCancel.db.select(dbConn, RequestEmpFilter);
            if (empRequestList.Count > 0)
            {
                ERequestLateWaiveCancel requestLateWaiveCancel = (ERequestLateWaiveCancel)empRequestList[0];
                mailContentParameterTable.Add("%LATEWAIVECANCEL_REMARK%", requestLateWaiveCancel.RequestLateWaiveCancelReason);
                ELateWaive lateWaive = new ELateWaive();
                lateWaive.LateWaiveID = requestLateWaiveCancel.LateWaiveID;
                if (ELateWaive.db.select(dbConn, lateWaive))
                {
                    ERequestLateWaive dummyRequestLateWaive = ERequestLateWaive.CreateDummyRequestLateWaive(lateWaive);
                    mailContentParameterTable = GenerateRequestLateWaiveHashTable(dummyRequestLateWaive, mailContentParameterTable);
                }
            }
        }
        // End 0000112, Miranda, 2014-12-10

        ArrayList workflowDetailList = GetAuthorizationWorkFlowDetailList(empRequest.EmpID, empRequest.EmpRequestType);
        List<AuthorizerMailAddress> authorizerEmailAddressList = GetAllAuthorizerEmailAddressList(workflowDetailList, 0, empRequest.EmpRequestLastAuthorizationWorkFlowIndex, true);
        SendCancelledByApplicantEmail(empRequest.EmpRequestType, authorizerEmailAddressList, ApplicantEmpInfo, mailContentParameterTable);
    }

    public void SubmitEmployeeInfoChange(ERequestEmpPersonalInfo requestEmpInfo)
    {


        EEmpPersonalInfo ApplicantEmpInfo = GetEmpInfo(requestEmpInfo.EmpID);

        Hashtable mailContentParameterTable = new Hashtable();
        mailContentParameterTable = GenerateEmpInfoHashTable(ApplicantEmpInfo, mailContentParameterTable);


        ArrayList workflowDetailList = GetAuthorizationWorkFlowDetailList(requestEmpInfo.EmpID, EEmpRequest.TYPE_EEPROFILE);
        if (workflowDetailList.Count <= 0)
        {
            throw new Exception("Request Cancel due to No Authorizer for this employee");
        }

        List<AuthorizerMailAddress> authorizerEmailAddressList = GetAllAuthorizerEmailAddressList(workflowDetailList, 0, 0, false);

        DateTime createDate = AppUtils.ServerDateTime();
        requestEmpInfo.RequestEmpCreateDate = createDate;
        ERequestEmpPersonalInfo.db.insert(dbConn, requestEmpInfo);

        EEmpRequest empRequest = new EEmpRequest();
        empRequest.EmpID = requestEmpInfo.EmpID;
        empRequest.EmpRequestType = EEmpRequest.TYPE_EEPROFILE;
        empRequest.EmpRequestRecordID = requestEmpInfo.RequestEmpID;
        empRequest.EmpRequestCreateDate = createDate;
        empRequest.EmpRequestModifyDate = createDate;
        empRequest.EmpRequestStatus = EEmpRequest.STATUS_SUBMITTED;

        EEmpRequest.db.insert(dbConn, empRequest);

        mailContentParameterTable = GenerateRequestRedirectLinkHashTable(empRequest, mailContentParameterTable);

        SendRequestEmail(empRequest.EmpRequestType, authorizerEmailAddressList, ApplicantEmpInfo, mailContentParameterTable);
    }

    public void SubmitLeaveApplication(ERequestLeaveApplication requestLeaveApplication)
    {
        EEmpPersonalInfo ApplicantEmpInfo = GetEmpInfo(requestLeaveApplication.EmpID);

        Hashtable mailContentParameterTable = new Hashtable();
        mailContentParameterTable = GenerateEmpInfoHashTable(ApplicantEmpInfo, mailContentParameterTable);

        mailContentParameterTable = GenerateRequestLeaveApplicationHashTable(requestLeaveApplication, mailContentParameterTable);

        ArrayList workflowDetailList = GetAuthorizationWorkFlowDetailList(requestLeaveApplication.EmpID, EEmpRequest.TYPE_EELEAVEAPP);
        if (workflowDetailList.Count <= 0)
        {
            throw new Exception("Request Cancel due to No Authorizer for this employee");
        }

        List<AuthorizerMailAddress> authorizerEmailAddressList = GetAllAuthorizerEmailAddressList(workflowDetailList, 0, 0, false);

        DateTime createDate = AppUtils.ServerDateTime();
        requestLeaveApplication.RequestLeaveAppCreateDate = createDate;
        ERequestLeaveApplication.db.insert(dbConn, requestLeaveApplication);

        EEmpRequest empRequest = new EEmpRequest();
        empRequest.EmpID = requestLeaveApplication.EmpID;
        empRequest.EmpRequestType = EEmpRequest.TYPE_EELEAVEAPP;
        empRequest.EmpRequestRecordID = requestLeaveApplication.RequestLeaveAppID;
        empRequest.EmpRequestCreateDate = createDate;
        empRequest.EmpRequestModifyDate = createDate;
        empRequest.EmpRequestStatus = EEmpRequest.STATUS_SUBMITTED;
        // Start 0000065, KuangWei, 2014-08-21
        empRequest.EmpRequestFromDate = requestLeaveApplication.RequestLeaveAppDateFrom;
        empRequest.EmpRequestToDate = requestLeaveApplication.RequestLeaveAppDateTo;
        // Start 0000201, Ricky S0, 2015-05-29
        empRequest.EmpRequestFromDateAM = requestLeaveApplication.RequestLeaveAppDateFromAM;
        empRequest.EmpRequestToDateAM = requestLeaveApplication.RequestLeaveAppDateToAM;
        // End 0000201, Ricky S0, 2015-05-29

        if (requestLeaveApplication.RequestLeaveAppUnit.Equals("D"))
        {
            if (requestLeaveApplication.RequestLeaveDays <= 1.0)
            {
                empRequest.EmpRequestDuration = requestLeaveApplication.RequestLeaveDays + " day";
            }
            else
            {
                empRequest.EmpRequestDuration = requestLeaveApplication.RequestLeaveDays + " days";
            }
        }
        else if (requestLeaveApplication.RequestLeaveAppUnit.Equals("H"))
        {
            // Start 0000094, Ricky So, 2014-09-10
            if (requestLeaveApplication.RequestLeaveAppHours <= 1.0)
            //if (requestLeaveApplication.RequestLeaveDays <= 1.0)
            // End 0000094, Ricky So, 2014-09-10
            {
                // Start 0000094, Ricky So, 2014-09-10
                empRequest.EmpRequestDuration = requestLeaveApplication.RequestLeaveAppHours + " hour";
                //empRequest.EmpRequestDuration = requestLeaveApplication.RequestLeaveDays + " hour";
                // End 0000094, Ricky So, 2014-09-10
            }
            else
            {
                // Start 0000094, Ricky So, 2014-09-10
                empRequest.EmpRequestDuration = requestLeaveApplication.RequestLeaveAppHours + " hours";
                //empRequest.EmpRequestDuration = requestLeaveApplication.RequestLeaveDays + " hours";
                // End 0000094, Ricky So, 2014-09-10
            }
        }
        else if (requestLeaveApplication.RequestLeaveAppUnit.Equals("A"))
        {
            empRequest.EmpRequestDuration = requestLeaveApplication.RequestLeaveDays + " A.M.";
        }
        else if (requestLeaveApplication.RequestLeaveAppUnit.Equals("P"))
        {
            empRequest.EmpRequestDuration = requestLeaveApplication.RequestLeaveDays + " P.M.";
        }
        // End 0000065, KuangWei, 2014-08-21

        EEmpRequest.db.insert(dbConn, empRequest);

        mailContentParameterTable = GenerateRequestRedirectLinkHashTable(empRequest, mailContentParameterTable);

        SendRequestEmail(empRequest.EmpRequestType, authorizerEmailAddressList, ApplicantEmpInfo, mailContentParameterTable);

    }

    // Start 0000060, Miranda, 2014-07-13
    private string GenerateOTDateTimeString(ERequestOTClaim requestOTClaim)
    {
        if (requestOTClaim.RequestOTClaimPeriodFrom.Date.Equals(requestOTClaim.RequestOTClaimPeriodTo.Date))
        {
            string requestOTClaimDateTimeString = requestOTClaim.RequestOTClaimPeriodFrom.ToString("yyyy-MM-dd");
            if (!(requestOTClaim.RequestOTClaimHourFrom.Ticks.Equals(0) && requestOTClaim.RequestOTClaimHourTo.Ticks.Equals(0)))
                requestOTClaimDateTimeString += " " + requestOTClaim.RequestOTClaimHourFrom.ToString("HH:mm") + " TO " + requestOTClaim.RequestOTClaimHourTo.ToString("HH:mm");
            return requestOTClaimDateTimeString;
        }
        else
            return requestOTClaim.RequestOTClaimPeriodFrom.ToString("yyyy-MM-dd") + " TO " + requestOTClaim.RequestOTClaimPeriodTo.ToString("yyyy-MM-dd");
    }

    public Hashtable GenerateRequestOTClaimHashTable(ERequestOTClaim requestOTClaim, Hashtable originalTable)
    {
        originalTable.Add("%OTCLAIM_PERIOD%", GenerateOTDateTimeString(requestOTClaim));
        originalTable.Add("%OTCLAIM_HOURSCLAIM%", requestOTClaim.RequestOTHours);
        originalTable.Add("%OTCLAIM_REMARK%", string.IsNullOrEmpty(requestOTClaim.RequestOTClaimRemark) ? string.Empty : requestOTClaim.RequestOTClaimRemark);
        return originalTable;
    }

    public void SubmitOTClaim(ERequestOTClaim requestOTClaim)
    {
        EEmpPersonalInfo ApplicantEmpInfo = GetEmpInfo(requestOTClaim.EmpID);

        Hashtable mailContentParameterTable = new Hashtable();
        mailContentParameterTable = GenerateEmpInfoHashTable(ApplicantEmpInfo, mailContentParameterTable);

        mailContentParameterTable = GenerateRequestOTClaimHashTable(requestOTClaim, mailContentParameterTable);

        ArrayList workflowDetailList = GetAuthorizationWorkFlowDetailList(requestOTClaim.EmpID, EEmpRequest.TYPE_EEOTCLAIM);
        if (workflowDetailList.Count <= 0)
        {
            throw new Exception("Request Cancel due to No Authorizer for this employee");
        }

        List<AuthorizerMailAddress> authorizerEmailAddressList = GetAllAuthorizerEmailAddressList(workflowDetailList, 0, 0, false);

        DateTime createDate = AppUtils.ServerDateTime();
        requestOTClaim.RequestOTClaimCreateDate = createDate;
        ERequestOTClaim.db.insert(dbConn, requestOTClaim);

        EEmpRequest empRequest = new EEmpRequest();
        empRequest.EmpID = requestOTClaim.EmpID;
        empRequest.EmpRequestType = EEmpRequest.TYPE_EEOTCLAIM;
        empRequest.EmpRequestRecordID = requestOTClaim.RequestOTClaimID;
        empRequest.EmpRequestCreateDate = createDate;
        empRequest.EmpRequestModifyDate = createDate;
        empRequest.EmpRequestStatus = EEmpRequest.STATUS_SUBMITTED;
        empRequest.EmpRequestFromDate = requestOTClaim.RequestOTClaimPeriodFrom;
        empRequest.EmpRequestToDate = requestOTClaim.RequestOTClaimPeriodTo;

        if (requestOTClaim.RequestOTHours > 1)
        {
            empRequest.EmpRequestDuration = requestOTClaim.RequestOTHours.ToString() + " hours";
        }else
        {
            empRequest.EmpRequestDuration = requestOTClaim.RequestOTHours.ToString() + " hour";
        }
        EEmpRequest.db.insert(dbConn, empRequest);

        mailContentParameterTable = GenerateRequestRedirectLinkHashTable(empRequest, mailContentParameterTable);

        SendRequestEmail(empRequest.EmpRequestType, authorizerEmailAddressList, ApplicantEmpInfo, mailContentParameterTable);
    }
    public void AuthorizeOTClaim(EEmpRequest empRequest, int AuthorizerEmpID)
    {
        AuthorizeAction(empRequest, AuthorizerEmpID);
    }
    // End 0000060, Miranda, 2014-07-13

    // Start 0000112, Miranda, 2014-12-10
    public Hashtable GenerateRequestLateWaiveHashTable(ERequestLateWaive requestLateWaive, Hashtable originalTable)
    {
        EAttendanceRecord attendanceRecord = new EAttendanceRecord();
        attendanceRecord.AttendanceRecordID = requestLateWaive.AttendanceRecordID;
        if (EAttendanceRecord.db.select(dbConn, attendanceRecord))
        {
            originalTable.Add("%LATEWAIVE_DATE%", attendanceRecord.AttendanceRecordDate.ToString("yyyy-MM-dd"));
            originalTable.Add("%LATEWAIVE_MINS%", attendanceRecord.AttendanceRecordActualLateMins.ToString());
        }
        originalTable.Add("%LATEWAIVE_REMARK%", string.IsNullOrEmpty(requestLateWaive.RequestLateWaiveReason) ? string.Empty : requestLateWaive.RequestLateWaiveReason);
        return originalTable;
    }

    public void SubmitLateWaive(ERequestLateWaive requestLateWaive)
    {
        EEmpPersonalInfo ApplicantEmpInfo = GetEmpInfo(requestLateWaive.EmpID);

        Hashtable mailContentParameterTable = new Hashtable();
        mailContentParameterTable = GenerateEmpInfoHashTable(ApplicantEmpInfo, mailContentParameterTable);

        mailContentParameterTable = GenerateRequestLateWaiveHashTable(requestLateWaive, mailContentParameterTable);

        ArrayList workflowDetailList = GetAuthorizationWorkFlowDetailList(requestLateWaive.EmpID, EEmpRequest.TYPE_EELATEWAIVE);
        if (workflowDetailList.Count <= 0)
        {
            throw new Exception("Request Cancel due to No Authorizer for this employee");
        }

        List<AuthorizerMailAddress> authorizerEmailAddressList = GetAllAuthorizerEmailAddressList(workflowDetailList, 0, 0, false);

        DateTime createDate = AppUtils.ServerDateTime();
        requestLateWaive.RequestLateWaiveCreateDate = createDate;
        ERequestLateWaive.db.insert(dbConn, requestLateWaive);

        EEmpRequest empRequest = new EEmpRequest();
        empRequest.EmpID = requestLateWaive.EmpID;
        empRequest.EmpRequestType = EEmpRequest.TYPE_EELATEWAIVE;
        empRequest.EmpRequestRecordID = requestLateWaive.RequestLateWaiveID;
        empRequest.EmpRequestCreateDate = createDate;
        empRequest.EmpRequestModifyDate = createDate;
        empRequest.EmpRequestStatus = EEmpRequest.STATUS_SUBMITTED;

        EEmpRequest.db.insert(dbConn, empRequest);

        mailContentParameterTable = GenerateRequestRedirectLinkHashTable(empRequest, mailContentParameterTable);

        SendRequestEmail(empRequest.EmpRequestType, authorizerEmailAddressList, ApplicantEmpInfo, mailContentParameterTable);
    }
    public void AuthorizeLateWaive(EEmpRequest empRequest, int AuthorizerEmpID)
    {
        AuthorizeAction(empRequest, AuthorizerEmpID);
    }
    // End 0000112, Miranda, 2014-12-10

    public void AuthorizeLeaveApplication(EEmpRequest empRequest, int AuthorizerEmpID)
    {
        AuthorizeAction(empRequest, AuthorizerEmpID);
    }
    public void AuthorizeEmployeeInformationUpdate(EEmpRequest empRequest, int AuthorizerEmpID)
    {
        AuthorizeAction(empRequest, AuthorizerEmpID);
    }

    public void AuthorizeAction(EEmpRequest empRequest, int AuthorizerEmpID)
    {

        EESSUser essUser = new EESSUser();
        essUser.EmpID = AuthorizerEmpID;
        if (EESSUser.db.select(dbConn, essUser))
        {

            EEmpPersonalInfo ApplicantEmpInfo = GetEmpInfo(empRequest.EmpID);

            Hashtable mailContentParameterTable = new Hashtable();
            mailContentParameterTable = GenerateEmpInfoHashTable(ApplicantEmpInfo, mailContentParameterTable);
            mailContentParameterTable.Add("%ACTION_USER%", essUser.EmpEngFullNameWithAlias);
            mailContentParameterTable.Add("%ACTION_REASON%", empRequest.EmpRequestRejectReason);
            mailContentParameterTable = GenerateRequestRedirectLinkHashTable(empRequest, mailContentParameterTable);


            if (empRequest.EmpRequestType == EEmpRequest.TYPE_EELEAVEAPP)
            {
                DBFilter RequestEmpFilter = new DBFilter();
                RequestEmpFilter.add(new Match("RequestLeaveAppID", empRequest.EmpRequestRecordID));
                ArrayList empRequestList = ERequestLeaveApplication.db.select(dbConn, RequestEmpFilter);
                if (empRequestList.Count > 0)
                {
                    ERequestLeaveApplication empLeaveRequest = (ERequestLeaveApplication)empRequestList[0];

                    mailContentParameterTable = GenerateRequestLeaveApplicationHashTable(empLeaveRequest, mailContentParameterTable);
                }
            }
            else if (empRequest.EmpRequestType == EEmpRequest.TYPE_EELEAVECANCEL)
            {
                DBFilter RequestEmpFilter = new DBFilter();
                RequestEmpFilter.add(new Match("RequestLeaveAppCancelID", empRequest.EmpRequestRecordID));
                ArrayList empRequestList = ERequestLeaveApplicationCancel.db.select(dbConn, RequestEmpFilter);
                if (empRequestList.Count > 0)
                {
                    ERequestLeaveApplicationCancel requestLeaveAppCancel = (ERequestLeaveApplicationCancel)empRequestList[0];
                    mailContentParameterTable.Add("%LEAVEAPPCANCEL_REMARK%", requestLeaveAppCancel.RequestLeaveAppCancelReason);
                    ELeaveApplication leaveApp = new ELeaveApplication();
                    leaveApp.LeaveAppID = requestLeaveAppCancel.LeaveAppID;
                    if (ELeaveApplication.db.select(dbConn, leaveApp))
                    {
                        ERequestLeaveApplication dummyRequestLeave = ERequestLeaveApplication.CreateDummyRequestLeaveAppliction(leaveApp);
                        mailContentParameterTable = GenerateRequestLeaveApplicationHashTable(dummyRequestLeave, mailContentParameterTable);
                    }
                }
            }
            // Start 0000060, Miranda, 2014-07-13
            if (empRequest.EmpRequestType == EEmpRequest.TYPE_EEOTCLAIM)
            {
                DBFilter RequestEmpFilter = new DBFilter();
                RequestEmpFilter.add(new Match("RequestOTClaimID", empRequest.EmpRequestRecordID));
                ArrayList empRequestList = ERequestOTClaim.db.select(dbConn, RequestEmpFilter);
                if (empRequestList.Count > 0)
                {
                    ERequestOTClaim empOTRequest = (ERequestOTClaim)empRequestList[0];

                    mailContentParameterTable = GenerateRequestOTClaimHashTable(empOTRequest, mailContentParameterTable);
                }
            }
            else if (empRequest.EmpRequestType == EEmpRequest.TYPE_EEOTCLAIMCANCEL)
            {
                DBFilter RequestEmpFilter = new DBFilter();
                RequestEmpFilter.add(new Match("RequestOTClaimCancelID", empRequest.EmpRequestRecordID));
                ArrayList empRequestList = ERequestOTClaimCancel.db.select(dbConn, RequestEmpFilter);
                if (empRequestList.Count > 0)
                {
                    ERequestOTClaimCancel requestOTClaimCancel = (ERequestOTClaimCancel)empRequestList[0];
                    mailContentParameterTable.Add("%OTCLAIMCANCEL_REMARK%", requestOTClaimCancel.RequestOTClaimCancelReason);
                    EOTClaim OTClaim = new EOTClaim();
                    OTClaim.OTClaimID = requestOTClaimCancel.OTClaimID;
                    if (EOTClaim.db.select(dbConn, OTClaim))
                    {
                        ERequestOTClaim dummyRequestLeave = ERequestOTClaim.CreateDummyRequestOTClaim(OTClaim);
                        mailContentParameterTable = GenerateRequestOTClaimHashTable(dummyRequestLeave, mailContentParameterTable);
                    }
                }
            }
            // End 0000060, Miranda, 2014-07-13
            // Start 0000112, Miranda, 2014-12-10
            if (empRequest.EmpRequestType == EEmpRequest.TYPE_EELATEWAIVE)
            {
                DBFilter RequestEmpFilter = new DBFilter();
                RequestEmpFilter.add(new Match("RequestLateWaiveID", empRequest.EmpRequestRecordID));
                ArrayList empRequestList = ERequestLateWaive.db.select(dbConn, RequestEmpFilter);
                if (empRequestList.Count > 0)
                {
                    ERequestLateWaive empLateWaiveRequest = (ERequestLateWaive)empRequestList[0];

                    mailContentParameterTable = GenerateRequestLateWaiveHashTable(empLateWaiveRequest, mailContentParameterTable);
                }
            }
            else if (empRequest.EmpRequestType == EEmpRequest.TYPE_EELATEWAIVECANCEL)
            {
                DBFilter RequestEmpFilter = new DBFilter();
                RequestEmpFilter.add(new Match("RequestLateWaiveCancelID", empRequest.EmpRequestRecordID));
                ArrayList empRequestList = ERequestLateWaiveCancel.db.select(dbConn, RequestEmpFilter);
                if (empRequestList.Count > 0)
                {
                    ERequestLateWaiveCancel requestLateWaiveCancel = (ERequestLateWaiveCancel)empRequestList[0];
                    mailContentParameterTable.Add("%LATEWAIVECANCEL_REMARK%", requestLateWaiveCancel.RequestLateWaiveCancelReason);
                    ELateWaive lateWaive = new ELateWaive();
                    lateWaive.LateWaiveID = requestLateWaiveCancel.LateWaiveID;
                    if (ELateWaive.db.select(dbConn, lateWaive))
                    {
                        ERequestLateWaive dummyRequestLeave = ERequestLateWaive.CreateDummyRequestLateWaive(lateWaive);
                        mailContentParameterTable = GenerateRequestLateWaiveHashTable(dummyRequestLeave, mailContentParameterTable);
                    }
                }
            }
            // End 0000112, Miranda, 2014-12-10

            ArrayList workflowDetailList = GetAuthorizationWorkFlowDetailList(empRequest.EmpID, empRequest.EmpRequestType);
            EAuthorizationWorkFlowDetail currentWorkFlowDetail = GetCurrentWorkFlowDetailObject(workflowDetailList, empRequest.EmpRequestLastAuthorizationWorkFlowIndex, AuthorizerEmpID);
            if (currentWorkFlowDetail != null)
            {
                int lastWorkFlowIndex = empRequest.EmpRequestLastAuthorizationWorkFlowIndex;
                empRequest.EmpRequestStatus = EEmpRequest.STATUS_ACCEPTED;
                empRequest.EmpRequestLastAuthorizationWorkFlowIndex = currentWorkFlowDetail.AuthorizationWorkFlowIndex;
                empRequest.EmpRequestLastActionByEmpID = AuthorizerEmpID;
                empRequest.EmpRequestLastActionBy = essUser.EmpEngFullNameWithAlias;

                if (currentWorkFlowDetail.AuthorizationWorkFlowIndex == ((EAuthorizationWorkFlowDetail)workflowDetailList[workflowDetailList.Count - 1]).AuthorizationWorkFlowIndex)
                {
                    empRequest.EmpRequestStatus = EEmpRequest.STATUS_APPROVED;

                    // Start 0000061, Ricky So, 2014/06/27                
                    List<AuthorizerMailAddress> authorizerList = GetAllAuthorizerEmailAddressList(workflowDetailList, 0, currentWorkFlowDetail.AuthorizationWorkFlowIndex, false);

                    if (ESystemParameter.getParameter(dbConn, ESystemParameter.PARAM_CODE_WF_SEND_ACCEPT_EMAIL) == "N")
                    {
                        EEmpPersonalInfo m_emp = EEmpPersonalInfo.GetObject(dbConn, AuthorizerEmpID);
                        if (m_emp != null)
                        {
                            List<AuthorizerMailAddress> m_removeList = new List<AuthorizerMailAddress>();

                            foreach (AuthorizerMailAddress m_mailInfo in authorizerList)
                            {
                                if (m_mailInfo.Address == m_emp.EmpEmail || m_mailInfo.Address == m_emp.EmpInternalEmail)
                                {
                                    m_removeList.Add(m_mailInfo);
                                }
                            }
                            foreach (AuthorizerMailAddress m_remvoeInfo in m_removeList)
                            {
                                authorizerList.Remove(m_remvoeInfo);
                            }
                        }
                    }
                    SendApprovedEmail(empRequest.EmpRequestType, authorizerList, ApplicantEmpInfo, mailContentParameterTable);

                    //SendApprovedEmail(empRequest.EmpRequestType, GetAllAuthorizerEmailAddressList(workflowDetailList, 0, currentWorkFlowDetail.AuthorizationWorkFlowIndex, false), ApplicantEmpInfo, mailContentParameterTable);
                    // End 0000061, Ricky So, 2014/06/27

                }
                else
                {
                    List<AuthorizerMailAddress> beforeOtherGroupAlertAuthorizerMailAddressList = null;
                    List<AuthorizerMailAddress> currentGroupAuthorizerMailAddressList = null;
                    List<AuthorizerMailAddress> nextGroupAuthorizerMailAddressList = null;
                    List<AuthorizerMailAddress> afterOtherGroupAlertAuthorizerMailAddressList = null;

                    GetAllAuthorizerEmailAddressList(workflowDetailList, lastWorkFlowIndex, currentWorkFlowDetail.AuthorizationWorkFlowIndex, true,
                        out beforeOtherGroupAlertAuthorizerMailAddressList,
                        out currentGroupAuthorizerMailAddressList,
                        out nextGroupAuthorizerMailAddressList,
                        out afterOtherGroupAlertAuthorizerMailAddressList);

                    List<AuthorizerMailAddress> authorizerEmailAddressList = new List<AuthorizerMailAddress>();
                    authorizerEmailAddressList.AddRange(nextGroupAuthorizerMailAddressList);
                    authorizerEmailAddressList.AddRange(afterOtherGroupAlertAuthorizerMailAddressList);
                    List<AuthorizerMailAddress> EmpCurrentAuthorizerList = new List<AuthorizerMailAddress>();

                    EmpCurrentAuthorizerList.AddRange(beforeOtherGroupAlertAuthorizerMailAddressList);
                    EmpCurrentAuthorizerList.AddRange(currentGroupAuthorizerMailAddressList);

                    SendRequestEmail(empRequest.EmpRequestType, authorizerEmailAddressList, ApplicantEmpInfo, mailContentParameterTable);




                    // Start 0000061, Ricky So, 2014/06/27                
                    if (ESystemParameter.getParameter(dbConn, ESystemParameter.PARAM_CODE_WF_SEND_ACCEPT_EMAIL) == "N")
                    {
                        EEmpPersonalInfo m_emp = EEmpPersonalInfo.GetObject(dbConn, AuthorizerEmpID);
                        if (m_emp != null)
                        {
                            List<AuthorizerMailAddress> m_removeList = new List<AuthorizerMailAddress>();

                            foreach (AuthorizerMailAddress m_mailInfo in EmpCurrentAuthorizerList)
                            {
                                if (m_mailInfo.Address == m_emp.EmpEmail || m_mailInfo.Address == m_emp.EmpInternalEmail)
                                {
                                    m_removeList.Add(m_mailInfo);
                                }
                            }
                            foreach (AuthorizerMailAddress m_remvoeInfo in m_removeList)
                            {
                                EmpCurrentAuthorizerList.Remove(m_remvoeInfo);
                            }
                        }
                    }
                    // End 0000061, Ricky So, 2014/06/27                



                    SendAcceptEmail(empRequest.EmpRequestType, EmpCurrentAuthorizerList, ApplicantEmpInfo, mailContentParameterTable);
                }
            }
            else
                throw new Exception("You are not correct authorization group. Action abort!");

            EEmpRequest.db.update(dbConn, empRequest);

            if (empRequest.EmpRequestStatus == EEmpRequest.STATUS_APPROVED)
            {
                if (empRequest.EmpRequestType == EEmpRequest.TYPE_EELEAVEAPP)
                {
                    ELeaveApplication leaveApp = new ELeaveApplication();
                    DBFilter RequestEmpFilter = new DBFilter();
                    RequestEmpFilter.add(new Match("RequestLeaveAppID", empRequest.EmpRequestRecordID));
                    ArrayList empRequestList = ERequestLeaveApplication.db.select(dbConn, RequestEmpFilter);
                    if (empRequestList.Count > 0)
                    {

                        ERequestLeaveApplication empLeaveRequest = (ERequestLeaveApplication)empRequestList[0];
                        leaveApp.EmpID = empLeaveRequest.EmpID;
                        leaveApp.LeaveCodeID = empLeaveRequest.RequestLeaveCodeID;
                        leaveApp.LeaveAppUnit = empLeaveRequest.RequestLeaveAppUnit;
                        leaveApp.LeaveAppDateFrom = empLeaveRequest.RequestLeaveAppDateFrom;
                        leaveApp.LeaveAppDateTo = empLeaveRequest.RequestLeaveAppDateTo;
                        leaveApp.LeaveAppTimeFrom = empLeaveRequest.RequestLeaveAppTimeFrom;
                        leaveApp.LeaveAppTimeTo = empLeaveRequest.RequestLeaveAppTimeTo;
                        // Start 0000201, Ricky So, 2015-05-29
                        leaveApp.LeaveAppDateFromAM = empLeaveRequest.RequestLeaveAppDateFromAM;
                        leaveApp.LeaveAppDateToAM = empLeaveRequest.RequestLeaveAppDateToAM;
                        // End 0000201, Ricky So, 2015-05-29
                        leaveApp.LeaveAppDays = empLeaveRequest.RequestLeaveDays;
                        leaveApp.LeaveAppHours = empLeaveRequest.RequestLeaveAppHours;
                        leaveApp.LeaveAppRemark = empLeaveRequest.RequestLeaveAppRemark;
                        leaveApp.LeaveAppHasMedicalCertificate = empLeaveRequest.RequestLeaveAppHasMedicalCertificate;
                        leaveApp.EmpRequestID = empRequest.EmpRequestID;
                        leaveApp.RequestLeaveAppID = empLeaveRequest.RequestLeaveAppID;

                        ELeaveCode leaveCode = new ELeaveCode();
                        leaveCode.LeaveCodeID = leaveApp.LeaveCodeID;
                        if (ELeaveCode.db.select(dbConn, leaveCode))
                            leaveApp.LeaveAppNoPayProcess = leaveCode.LeaveCodeIsSkipPayrollProcess;
                        ELeaveApplication.db.insert(dbConn, leaveApp);

                        //HROne.LeaveCalc.LeaveBalanceCalc leaaveBalCal = new HROne.LeaveCalc.LeaveBalanceCalc(dbConn, leaveApp.EmpID);
                        //leaaveBalCal.RecalculateAfter(leaveApp.LeaveAppDateFrom, leaveCode.LeaveTypeID);

                    }
                    else
                    {
                    }


                }
                else if (empRequest.EmpRequestType == EEmpRequest.TYPE_EEPROFILE)
                {
                    EEmpPersonalInfo EmpInfo = new EEmpPersonalInfo();
                    DBFilter RequestEmpFilter = new DBFilter();
                    RequestEmpFilter.add(new Match("RequestEmpID", empRequest.EmpRequestRecordID));
                    ArrayList EmpRequestList = ERequestEmpPersonalInfo.db.select(dbConn, RequestEmpFilter);
                    if (EmpRequestList.Count > 0)
                    {
                        ERequestEmpPersonalInfo EmpRequest = (ERequestEmpPersonalInfo)EmpRequestList[0];
                        EmpInfo.EmpID = EmpRequest.EmpID;
                        EmpInfo.EmpAlias = EmpRequest.RequestEmpAlias;
                        EmpInfo.EmpMaritalStatus = EmpRequest.RequestEmpMaritalStatus;
                        EmpInfo.EmpPassportNo = EmpRequest.RequestEmpPassportNo;
                        EmpInfo.EmpPassportIssuedCountry = EmpRequest.RequestEmpPassportIssuedCountry;
                        EmpInfo.EmpPassportExpiryDate = EmpRequest.RequestEmpPassportExpiryDate;
                        EmpInfo.EmpNationality = EmpRequest.RequestEmpNationality;
                        EmpInfo.EmpHomePhoneNo = EmpRequest.RequestEmpHomePhoneNo;
                        EmpInfo.EmpMobileNo = EmpRequest.RequestEmpMobileNo;
                        EmpInfo.EmpOfficePhoneNo = EmpRequest.RequestEmpOfficePhoneNo;
                        EmpInfo.EmpEmail = EmpRequest.RequestEmpEmail;
                        EmpInfo.EmpResAddr = EmpRequest.RequestEmpResAddr;
                        EmpInfo.EmpResAddrAreaCode = EmpRequest.RequestEmpResAddrAreaCode;
                        EmpInfo.EmpCorAddr = EmpRequest.RequestEmpCorAdd;
                        // Start 0000092, KuangWei, 2014-10-17
                        EmpInfo.EmpPlaceOfBirth = EmpRequest.RequestEmpPlaceOfBirth;
                        EmpInfo.EmpPlaceOfBirthID = EmpRequest.RequestEmpPlaceOfBirthID;
                        EmpInfo.EmpPassportIssuedCountryID = EmpRequest.RequestEmpPlaceOfBirthID;
                        EmpInfo.EmpNationalityID = EmpRequest.RequestEmpNationalityID;
                        // End 0000092, KuangWei, 2014-10-17
                        EEmpPersonalInfo.db.update(dbConn, EmpInfo);
                    }
                    else
                    {
                    }
                }
                else if (empRequest.EmpRequestType == EEmpRequest.TYPE_EELEAVECANCEL)
                {
                    DBFilter RequestLeaveCancelFilter = new DBFilter();
                    RequestLeaveCancelFilter.add(new Match("RequestLeaveAppCancelID", empRequest.EmpRequestRecordID));
                    ArrayList requestList = ERequestLeaveApplicationCancel.db.select(dbConn, RequestLeaveCancelFilter);
                    if (requestList.Count > 0)
                    {

                        ERequestLeaveApplicationCancel empLeaveCancelRequest = (ERequestLeaveApplicationCancel)requestList[0];
                        ELeaveApplicationCancel leaveAppCancel = new ELeaveApplicationCancel();
                        leaveAppCancel.EmpID = empLeaveCancelRequest.EmpID;
                        leaveAppCancel.LeaveAppID = empLeaveCancelRequest.LeaveAppID;
                        leaveAppCancel.LeaveAppCancelReason = empLeaveCancelRequest.RequestLeaveAppCancelReason;
                        leaveAppCancel.EmpRequestID = empRequest.EmpRequestID;
                        leaveAppCancel.RequestLeaveAppCancelID = empLeaveCancelRequest.RequestLeaveAppCancelID;

                        ELeaveApplicationCancel.db.insert(dbConn, leaveAppCancel);

                        ELeaveApplication leaveApp = new ELeaveApplication();
                        leaveApp.LeaveAppID = leaveAppCancel.LeaveAppID;
                        if (ELeaveApplication.db.select(dbConn, leaveApp) && leaveApp.LeaveAppCancelID <= 0)
                        {
                            leaveApp.LeaveAppCancelID = leaveAppCancel.LeaveAppCancelID;
                            ELeaveApplication leaveAppforCompensate = new ELeaveApplication();
                            if (leaveApp.EmpPayrollID <= 0)
                            {
                                leaveApp.LeaveAppNoPayProcess = true;
                            }
                            ELeaveApplication.db.update(dbConn, leaveApp);

                            ELeaveApplication.db.copyObject(leaveApp, leaveAppforCompensate);
                            leaveAppforCompensate.LeaveAppID = 0;
                            leaveAppforCompensate.LeaveAppDays = -leaveApp.LeaveAppDays;
                            leaveAppforCompensate.LeaveAppHours = -leaveApp.LeaveAppHours;
                            leaveAppforCompensate.EmpPayrollID = 0;
                            leaveAppforCompensate.EmpPaymentID = 0;
                            ELeaveApplication.db.insert(dbConn, leaveAppforCompensate);
                        }
                    }
                    else
                    {
                    }
                }
                // Start 0000060, Miranda, 2014-07-13
                else if (empRequest.EmpRequestType == EEmpRequest.TYPE_EEOTCLAIM)
                {
                    EOTClaim OTClaim = new EOTClaim();
                    DBFilter RequestEmpFilter = new DBFilter();
                    RequestEmpFilter.add(new Match("RequestOTClaimID", empRequest.EmpRequestRecordID));
                    ArrayList empRequestList = ERequestOTClaim.db.select(dbConn, RequestEmpFilter);
                    if (empRequestList.Count > 0)
                    {

                        ERequestOTClaim empOTRequest = (ERequestOTClaim)empRequestList[0];
                        OTClaim.EmpID = empOTRequest.EmpID;
                        OTClaim.OTClaimDateFrom = empOTRequest.RequestOTClaimPeriodFrom;
                        OTClaim.OTClaimDateTo = empOTRequest.RequestOTClaimPeriodTo;
                        OTClaim.OTClaimTimeFrom = empOTRequest.RequestOTClaimHourFrom;
                        OTClaim.OTClaimTimeTo = empOTRequest.RequestOTClaimHourTo;
                        OTClaim.OTClaimHours = empOTRequest.RequestOTHours;
                        OTClaim.OTClaimRemark = empOTRequest.RequestOTClaimRemark;
                        OTClaim.EmpRequestID = empRequest.EmpRequestID;
                        OTClaim.RequestOTClaimID = empOTRequest.RequestOTClaimID;

                        EOTClaim.db.insert(dbConn, OTClaim);

                        ECompensationLeaveEntitle comLeaveEntitle = new ECompensationLeaveEntitle();
                        comLeaveEntitle.CompensationLeaveEntitleApprovedBy = essUser.EmpEngFullNameWithAlias;
                        comLeaveEntitle.CompensationLeaveEntitleClaimPeriodFrom = empOTRequest.RequestOTClaimPeriodFrom;
                        comLeaveEntitle.CompensationLeaveEntitleClaimPeriodTo = empOTRequest.RequestOTClaimPeriodTo;
                        comLeaveEntitle.CompensationLeaveEntitleClaimHourFrom = empOTRequest.RequestOTClaimHourFrom;
                        comLeaveEntitle.CompensationLeaveEntitleClaimHourTo = empOTRequest.RequestOTClaimHourTo;
                        comLeaveEntitle.CompensationLeaveEntitleHoursClaim = empOTRequest.RequestOTHours;
                        comLeaveEntitle.CompensationLeaveEntitleRemark = empOTRequest.RequestOTClaimRemark;
                        comLeaveEntitle.EmpID = empOTRequest.EmpID;
                        // Start 0000060, Miranda, 2014-07-22
                        comLeaveEntitle.CompensationLeaveEntitleEffectiveDate = empOTRequest.RequestOTClaimEffectiveDate;
                        comLeaveEntitle.CompensationLeaveEntitleDateExpiry = empOTRequest.RequestOTClaimDateExpiry;
                        comLeaveEntitle.CompensationLeaveEntitleIsOTClaim = true;
                        //comLeaveEntitle.CompensationLeaveEntitleEffectiveDate = DateTime.Now;
                        //comLeaveEntitle.CompensationLeaveEntitleDateExpiry = DateTime.Now.AddMonths(1);
                        // End 0000060, Miranda, 2014-07-22
                        ECompensationLeaveEntitle.db.insert(dbConn, comLeaveEntitle);

                    }
                    else
                    {
                    }

                }
                else if (empRequest.EmpRequestType == EEmpRequest.TYPE_EEOTCLAIMCANCEL)
                {
                    DBFilter RequestOTCancelFilter = new DBFilter();
                    RequestOTCancelFilter.add(new Match("RequestOTClaimCancelID", empRequest.EmpRequestRecordID));
                    ArrayList requestList = ERequestOTClaimCancel.db.select(dbConn, RequestOTCancelFilter);
                    if (requestList.Count > 0)
                    {

                        ERequestOTClaimCancel empOTCancelRequest = (ERequestOTClaimCancel)requestList[0];
                        EOTClaimCancel OTClaimCancel = new EOTClaimCancel();
                        OTClaimCancel.EmpID = empOTCancelRequest.EmpID;
                        OTClaimCancel.OTClaimID = empOTCancelRequest.OTClaimID;
                        OTClaimCancel.OTClaimCancelReason = empOTCancelRequest.RequestOTClaimCancelReason;
                        OTClaimCancel.EmpRequestID = empRequest.EmpRequestID;
                        OTClaimCancel.RequestOTClaimCancelID = empOTCancelRequest.RequestOTClaimCancelID;

                        EOTClaimCancel.db.insert(dbConn, OTClaimCancel);

                        EOTClaim OTClaim = new EOTClaim();
                        OTClaim.OTClaimID = OTClaimCancel.OTClaimID;
                        if (EOTClaim.db.select(dbConn, OTClaim) && OTClaim.OTClaimCancelID <= 0)
                        {
                            OTClaim.OTClaimCancelID = OTClaimCancel.OTClaimCancelID;

                            EOTClaim OTClaimforCompensate = new EOTClaim();

                            EOTClaim.db.update(dbConn, OTClaim);

                            EOTClaim.db.copyObject(OTClaim, OTClaimforCompensate);
                            OTClaimforCompensate.OTClaimID = 0;
                            OTClaimforCompensate.OTClaimHours = -OTClaim.OTClaimHours;
                            EOTClaim.db.insert(dbConn, OTClaimforCompensate);
                        }

                    }
                    else
                    {
                    }
                }
                // End 0000060, Miranda, 2014-07-13
                // Start 0000112, Miranda, 2014-12-10
                else if (empRequest.EmpRequestType == EEmpRequest.TYPE_EELATEWAIVE)
                {
                    ELateWaive lateWaive = new ELateWaive();
                    DBFilter RequestEmpFilter = new DBFilter();
                    RequestEmpFilter.add(new Match("RequestLateWaiveID", empRequest.EmpRequestRecordID));
                    ArrayList empRequestList = ERequestLateWaive.db.select(dbConn, RequestEmpFilter);
                    if (empRequestList.Count > 0)
                    {

                        ERequestLateWaive empLateWaiveRequest = (ERequestLateWaive)empRequestList[0];
                        lateWaive.EmpID = empLateWaiveRequest.EmpID;
                        lateWaive.AttendanceRecordID = empLateWaiveRequest.AttendanceRecordID;
                        lateWaive.LateWaiveReason = empLateWaiveRequest.RequestLateWaiveReason;
                        lateWaive.EmpRequestID = empRequest.EmpRequestID;
                        lateWaive.RequestLateWaiveID = empLateWaiveRequest.RequestLateWaiveID;

                        ELateWaive.db.insert(dbConn, lateWaive);

                        // need set Late (mins) under AttendanceRecord to "0"
                        EAttendanceRecord aObj = new EAttendanceRecord();
                        aObj.AttendanceRecordID = empLateWaiveRequest.AttendanceRecordID;
                        if (EAttendanceRecord.db.select(dbConn, aObj))
                        {
                            aObj.AttendanceRecordWaivedLateMins += aObj.AttendanceRecordActualLateMins;
                            aObj.AttendanceRecordActualLateMins = 0;
                            EAttendanceRecord.db.update(dbConn, aObj);
                        }

                    }
                    else
                    {
                    }

                }
                else if (empRequest.EmpRequestType == EEmpRequest.TYPE_EELATEWAIVECANCEL)
                {
                    DBFilter RequestLateWaiveCancelFilter = new DBFilter();
                    RequestLateWaiveCancelFilter.add(new Match("RequestLateWaiveCancelID", empRequest.EmpRequestRecordID));
                    ArrayList requestList = ERequestLateWaiveCancel.db.select(dbConn, RequestLateWaiveCancelFilter);
                    if (requestList.Count > 0)
                    {

                        ERequestLateWaiveCancel empLateWaiveCancelRequest = (ERequestLateWaiveCancel)requestList[0];
                        ELateWaiveCancel lateWaiveCancel = new ELateWaiveCancel();
                        lateWaiveCancel.EmpID = empLateWaiveCancelRequest.EmpID;
                        lateWaiveCancel.LateWaiveID = empLateWaiveCancelRequest.LateWaiveID;
                        lateWaiveCancel.LateWaiveCancelReason = empLateWaiveCancelRequest.RequestLateWaiveCancelReason;
                        lateWaiveCancel.EmpRequestID = empRequest.EmpRequestID;
                        lateWaiveCancel.RequestLateWaiveCancelID = empLateWaiveCancelRequest.RequestLateWaiveCancelID;

                        ELateWaiveCancel.db.insert(dbConn, lateWaiveCancel);

                        ELateWaive lateWaive = new ELateWaive();
                        lateWaive.LateWaiveID = lateWaiveCancel.LateWaiveID;
                        if (ELateWaive.db.select(dbConn, lateWaive) && lateWaive.LateWaiveCancelID <= 0)
                        {
                            lateWaive.LateWaiveCancelID = lateWaiveCancel.LateWaiveCancelID;

                            ELateWaive.db.update(dbConn, lateWaive);
                        }

                    }
                    else
                    {
                    }
                }
                // End 0000112, Miranda, 2014-12-10
                SendApplicantApprovedEmail(empRequest.EmpRequestType, ApplicantEmpInfo, mailContentParameterTable);
            }
            //else
            //    throw new Exception("Request Cancel due to No Authorizer for this employee");

        }
    }

    public void RejectedAction(EEmpRequest empRequest, int AuthorizerEmpID)
    {
        EESSUser essUser = new EESSUser();
        essUser.EmpID = AuthorizerEmpID;
        if (EESSUser.db.select(dbConn, essUser))
        {

            EEmpPersonalInfo ApplicantEmpInfo = GetEmpInfo(empRequest.EmpID);

            Hashtable mailContentParameterTable = new Hashtable();
            mailContentParameterTable = GenerateEmpInfoHashTable(ApplicantEmpInfo, mailContentParameterTable);
            mailContentParameterTable.Add("%ACTION_USER%", essUser.EmpEngFullNameWithAlias);
            mailContentParameterTable.Add("%ACTION_REASON%", empRequest.EmpRequestRejectReason);
            mailContentParameterTable = GenerateRequestRedirectLinkHashTable(empRequest, mailContentParameterTable);

            if (empRequest.EmpRequestType == EEmpRequest.TYPE_EELEAVEAPP)
            {
                DBFilter RequestEmpFilter = new DBFilter();
                RequestEmpFilter.add(new Match("RequestLeaveAppID", empRequest.EmpRequestRecordID));
                ArrayList empRequestList = ERequestLeaveApplication.db.select(dbConn, RequestEmpFilter);
                if (empRequestList.Count > 0)
                {
                    ERequestLeaveApplication empLeaveRequest = (ERequestLeaveApplication)empRequestList[0];

                    mailContentParameterTable = GenerateRequestLeaveApplicationHashTable(empLeaveRequest, mailContentParameterTable);
                }
            }
            else if (empRequest.EmpRequestType == EEmpRequest.TYPE_EELEAVECANCEL)
            {
                DBFilter RequestEmpFilter = new DBFilter();
                RequestEmpFilter.add(new Match("RequestLeaveAppCancelID", empRequest.EmpRequestRecordID));
                ArrayList empRequestList = ERequestLeaveApplicationCancel.db.select(dbConn, RequestEmpFilter);
                if (empRequestList.Count > 0)
                {
                    ERequestLeaveApplicationCancel requestLeaveAppCancel = (ERequestLeaveApplicationCancel)empRequestList[0];
                    mailContentParameterTable.Add("%LEAVEAPPCANCEL_REMARK%", requestLeaveAppCancel.RequestLeaveAppCancelReason);
                    ELeaveApplication leaveApp = new ELeaveApplication();
                    leaveApp.LeaveAppID = requestLeaveAppCancel.LeaveAppID;
                    if (ELeaveApplication.db.select(dbConn, leaveApp))
                    {
                        ERequestLeaveApplication dummyRequestLeave = ERequestLeaveApplication.CreateDummyRequestLeaveAppliction(leaveApp);
                        mailContentParameterTable = GenerateRequestLeaveApplicationHashTable(dummyRequestLeave, mailContentParameterTable);
                    }
                }
            }
            // Start 0000060, Miranda, 2014-07-13
            else if (empRequest.EmpRequestType == EEmpRequest.TYPE_EEOTCLAIM)
            {
                DBFilter RequestEmpFilter = new DBFilter();
                RequestEmpFilter.add(new Match("RequestOTClaimID", empRequest.EmpRequestRecordID));
                ArrayList empRequestList = ERequestOTClaim.db.select(dbConn, RequestEmpFilter);
                if (empRequestList.Count > 0)
                {
                    ERequestOTClaim empOTRequest = (ERequestOTClaim)empRequestList[0];

                    mailContentParameterTable = GenerateRequestOTClaimHashTable(empOTRequest, mailContentParameterTable);
                }
            }
            else if (empRequest.EmpRequestType == EEmpRequest.TYPE_EEOTCLAIMCANCEL)
            {
                DBFilter RequestEmpFilter = new DBFilter();
                RequestEmpFilter.add(new Match("RequestOTClaimCancelID", empRequest.EmpRequestRecordID));
                ArrayList empRequestList = ERequestOTClaimCancel.db.select(dbConn, RequestEmpFilter);
                if (empRequestList.Count > 0)
                {
                    ERequestOTClaimCancel requestOTClaimCancel = (ERequestOTClaimCancel)empRequestList[0];
                    mailContentParameterTable.Add("%OTCLAIMCANCEL_REMARK%", requestOTClaimCancel.RequestOTClaimCancelReason);
                    EOTClaim OTClaim = new EOTClaim();
                    OTClaim.OTClaimID = requestOTClaimCancel.OTClaimID;
                    if (EOTClaim.db.select(dbConn, OTClaim))
                    {
                        ERequestOTClaim dummyRequestOT = ERequestOTClaim.CreateDummyRequestOTClaim(OTClaim);
                        mailContentParameterTable = GenerateRequestOTClaimHashTable(dummyRequestOT, mailContentParameterTable);
                    }
                }
            }
            // End 0000060, Miranda, 2014-07-13
            // Start 0000112, Miranda, 2014-12-10
            else if (empRequest.EmpRequestType == EEmpRequest.TYPE_EELATEWAIVE)
            {
                DBFilter RequestEmpFilter = new DBFilter();
                RequestEmpFilter.add(new Match("RequestLateWaiveID", empRequest.EmpRequestRecordID));
                ArrayList empRequestList = ERequestLateWaive.db.select(dbConn, RequestEmpFilter);
                if (empRequestList.Count > 0)
                {
                    ERequestLateWaive empLateWaiveRequest = (ERequestLateWaive)empRequestList[0];

                    mailContentParameterTable = GenerateRequestLateWaiveHashTable(empLateWaiveRequest, mailContentParameterTable);
                }
            }
            else if (empRequest.EmpRequestType == EEmpRequest.TYPE_EELATEWAIVECANCEL)
            {
                DBFilter RequestEmpFilter = new DBFilter();
                RequestEmpFilter.add(new Match("RequestLateWaiveCancelID", empRequest.EmpRequestRecordID));
                ArrayList empRequestList = ERequestLateWaiveCancel.db.select(dbConn, RequestEmpFilter);
                if (empRequestList.Count > 0)
                {
                    ERequestLateWaiveCancel requestLateWaiveCancel = (ERequestLateWaiveCancel)empRequestList[0];
                    mailContentParameterTable.Add("%LATEWAIVECANCEL_REMARK%", requestLateWaiveCancel.RequestLateWaiveCancelReason);
                    ELateWaive lateWaive = new ELateWaive();
                    lateWaive.LateWaiveID = requestLateWaiveCancel.LateWaiveID;
                    if (ELateWaive.db.select(dbConn, lateWaive))
                    {
                        ERequestLateWaive dummyRequestLateWaive = ERequestLateWaive.CreateDummyRequestLateWaive(lateWaive);
                        mailContentParameterTable = GenerateRequestLateWaiveHashTable(dummyRequestLateWaive, mailContentParameterTable);
                    }
                }
            }
            // End 0000112, Miranda, 2014-12-10

            ArrayList workflowDetailList = GetAuthorizationWorkFlowDetailList(empRequest.EmpID, empRequest.EmpRequestType);
            EAuthorizationWorkFlowDetail currentWorkFlowDetail = GetCurrentWorkFlowDetailObject(workflowDetailList, empRequest.EmpRequestLastAuthorizationWorkFlowIndex, AuthorizerEmpID);

            if (currentWorkFlowDetail != null)
            {
                int lastWorkFlowIndex = empRequest.EmpRequestLastAuthorizationWorkFlowIndex;
                empRequest.EmpRequestStatus = EEmpRequest.STATUS_REJECTED;
                empRequest.EmpRequestLastAuthorizationWorkFlowIndex = currentWorkFlowDetail.AuthorizationWorkFlowIndex;
                empRequest.EmpRequestLastActionByEmpID = AuthorizerEmpID;
                empRequest.EmpRequestLastActionBy = essUser.EmpEngFullNameWithAlias;



                // Start 0000061, Ricky So, 2014/06/27
                List<AuthorizerMailAddress> authorizerList = GetAllAuthorizerEmailAddressList(workflowDetailList, 0, empRequest.EmpRequestLastAuthorizationWorkFlowIndex, false);
                if (ESystemParameter.getParameter(dbConn, ESystemParameter.PARAM_CODE_WF_SEND_ACCEPT_EMAIL) == "N")
                {
                    EEmpPersonalInfo m_emp = EEmpPersonalInfo.GetObject(dbConn, AuthorizerEmpID);
                    if (m_emp != null)
                    {
                        List<AuthorizerMailAddress> m_removeList = new List<AuthorizerMailAddress>();

                        foreach (AuthorizerMailAddress m_mailInfo in authorizerList)
                        {
                            if (m_mailInfo.Address == m_emp.EmpEmail || m_mailInfo.Address == m_emp.EmpInternalEmail)
                            {
                                m_removeList.Add(m_mailInfo);
                            }
                        }
                        foreach (AuthorizerMailAddress m_remvoeInfo in m_removeList)
                        {
                            authorizerList.Remove(m_remvoeInfo);
                        }
                    }
                }
                SendRejectedEmail(empRequest.EmpRequestType, authorizerList, ApplicantEmpInfo, mailContentParameterTable);
                //SendRejectedEmail(empRequest.EmpRequestType, GetAllAuthorizerEmailAddressList(workflowDetailList, 0, empRequest.EmpRequestLastAuthorizationWorkFlowIndex, false), ApplicantEmpInfo, mailContentParameterTable);
                // End 0000061, Ricky So, 2014/06/27   

            }
            else
                throw new Exception("You are not correct authorization group. Action abort!");

            EEmpRequest.db.update(dbConn, empRequest);

            SendApplicantRejectedEmail(empRequest.EmpRequestType, ApplicantEmpInfo, mailContentParameterTable);
        }
    }

    // Start 0000060, Miranda, 2014-07-13
    public void RejectOTClaim(EEmpRequest empRequest, int AuthorizerEmpID)
    {
        RejectedAction(empRequest, AuthorizerEmpID);
    }
    // End 0000060, Miranda, 2014-07-13

    // Start 0000112, Miranda, 2014-12-10
    public void RejectLateWaive(EEmpRequest empRequest, int AuthorizerEmpID)
    {
        RejectedAction(empRequest, AuthorizerEmpID);
    }
    // End 0000112, Miranda, 2014-12-10

    public void RejectLeaveApplication(EEmpRequest empRequest, int AuthorizerEmpID)
    {
        RejectedAction(empRequest, AuthorizerEmpID);
    }
    public void RejectEmployeeInformationUpdate(EEmpRequest empRequest, int AuthorizerEmpID)
    {
        RejectedAction(empRequest, AuthorizerEmpID);
    }

    public string LoadOrCreateFile(string Filename, string FileContent)
    {
        string m_content = "";
        try
        {
            string FullPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, Filename);
            System.IO.FileInfo m_fileInfo = new System.IO.FileInfo(FullPath);

            if (m_fileInfo.Exists)
            {
                System.IO.StreamReader m_reader = m_fileInfo.OpenText();
                m_content = m_reader.ReadToEnd();
                m_reader.Close();
            }
            else
            {
                System.IO.StreamWriter m_writer = m_fileInfo.CreateText();
                m_writer.Write(FileContent);
                m_writer.Close();

                m_content = FileContent;
            }
        }
        catch (Exception ex)
        {
            
        }

        return m_content;
    }

    protected virtual void SendApplicantApprovedEmail(string ApplicationType, EEmpPersonalInfo ApplicantEmpInfo, Hashtable parameterTable)
    {
        // Start 0000080, KuangWei, 2014-09-16
        //const string default_LeaveMessageSubject = "Your leave application request is approved";
        const string default_LeaveMessageSubject = "[Leave Application] %EMP_NAME% %LEAVEAPP_DAYS% %LEAVEAPP_UNIT% %LEAVEAPP_CODE% on %LEAVEAPP_PERIOD%";
        // End 0000080, KuangWei, 2014-09-16
        const string default_LeaveMessageBody =
                            "Leave Date:\t%LEAVEAPP_PERIOD%\r\n" +
                            "Leave Type:\t%LEAVE_CODE%\r\n" +
                            "Taken     :\t%LEAVEAPP_DAYS% %LEAVEAPP_UNIT%\r\n" +
                            "Hours Claims:\t%LEAVEAPP_HOURSCLAIM%\r\n" +
                            "Remark    :\t%LEAVEAPP_REMARK%\r\n" +
                            "\r\n" +
                            "Authorized By:\t%ACTION_USER%\r\n" +
                            "Reason:\t%ACTION_REASON%\r\n" +
                            "\r\n" +
                            "The above leave application request is approved.\r\n";

        const string default_ProfileMessageSubject = "Approval of employee information update";
        const string default_ProfileMessageBody =
                                "Dear colleague,\r\n" +
                                "\r\n" +
                                "Your employee information update is approved.  If you have any questions, please do not hesitate to contact Human Resources Department.\r\n" +
                                "\r\n" +
                                "Authorized By:\t%ACTION_USER%\r\n" +
                                "Reason:\t%ACTION_REASON%\r\n" +
                                "\r\n" +
                                "Thank you for using the HROne Employee Self Service.\r\n";

        // Start 0000080, KuangWei, 2014-09-16
        //const string default_LeaveCancelMessageSubject = "Your leave application cancellation request is approved";
        const string default_LeaveCancelMessageSubject = "[Leave Cancellation] %EMP_NAME% %LEAVEAPP_DAYS% %LEAVEAPP_UNIT% %LEAVEAPP_CODE% on %LEAVEAPP_PERIOD%";
        // End 0000080, KuangWei, 2014-09-16
        const string default_LeaveCancelMessageBody =
                            "Leave Date:\t%LEAVEAPP_PERIOD%\r\n" +
                            "Leave Type:\t%LEAVE_CODE%\r\n" +
                            "Taken     :\t%LEAVEAPP_DAYS% %LEAVEAPP_UNIT%\r\n" +
                            "Hours Claims:\t%LEAVEAPP_HOURSCLAIM%\r\n" +
                            "Remark    :\t%LEAVEAPP_REMARK%\r\n" +
                            "Reason for Cancellation:\t%LEAVEAPPCANCEL_REMARK%\r\n" +
                            "\r\n" +
                            "Authorized By:\t%ACTION_USER%\r\n" +
                            "Reason:\t%ACTION_REASON%\r\n" +
                            "\r\n" +
                            "The above leave application cancellation request is approved.\r\n";

        // Start 0000060, Miranda, 2014-07-13
        // Start 0000080, KuangWei, 2014-09-16
        //const string default_OTMessageSubject = "Your CL Requisition request is approved";
        const string default_OTMessageSubject = "[CL Requisition] %EMP_NAME% %OTCLAIM_HOURSCLAIM% hour(s) OT on %OTCLAIM_PERIOD%";
        // End 0000080, KuangWei, 2014-09-16
        const string default_OTMessageBody =
                            "CL Requisition Date:\t%OTCLAIM_PERIOD%\r\n" +
                            "Hours Claims:\t%OTCLAIM_HOURSCLAIM%\r\n" +
                            "Remark    :\t%OTCLAIM_REMARK%\r\n" +
                            "\r\n" +
                            "Authorized By:\t%ACTION_USER%\r\n" +
                            "Reason:\t%ACTION_REASON%\r\n" +
                            "\r\n" +
                            "The above CL Requisition request is approved.\r\n";
        // Start 0000080, KuangWei, 2014-09-16
        //const string default_OTCancelMessageSubject = "Your CL Requisition cancellation request is approved";
        const string default_OTCancelMessageSubject = "[CL Requisition] %EMP_NAME% %OTCLAIM_HOURSCLAIM% hour(s) OT on %OTCLAIM_PERIOD%";
        // End 0000080, KuangWei, 2014-09-16
        const string default_OTCancelMessageBody =
                            "CL Requisition Date:\t%OTCLAIM_PERIOD%\r\n" +
                            "Hours Claims:\t%OTCLAIM_HOURSCLAIM%\r\n" +
                            "Remark    :\t%OTCLAIM_REMARK%\r\n" +
                            "Reason for Cancellation:\t%OTCLAIMCANCEL_REMARK%\r\n" +
                            "\r\n" +
                            "Authorized By:\t%ACTION_USER%\r\n" +
                            "Reason:\t%ACTION_REASON%\r\n" +
                            "\r\n" +
                            "The above CL Requisition cancellation request is approved.\r\n";
        // End 0000060, Miranda, 2014-07-13
        // Start 0000112, Miranda, 2014-12-10
        const string default_LateWaiveMessageSubject = "[Late Waive] %EMP_NAME% %LATEWAIVE_MINS% min(s) Late on %LATEWAIVE_DATE%";
        const string default_LateWaiveMessageBody =
                            "Late Date:\t%LATEWAIVE_DATE%\r\n" +
                            "Late Mins:\t%LATEWAIVE_MINS%\r\n" +
                            "Reason   :\t%LATEWAIVE_REMARK%\r\n" +
                            "\r\n" +
                            "Authorized By:\t%ACTION_USER%\r\n" +
                            "Reason:\t%ACTION_REASON%\r\n" +
                            "\r\n" +
                            "The above late waive request is approved.\r\n";
        const string default_LateWaiveCancelMessageSubject = "Your late waive cancellation request is approved";
        const string default_LateWaiveCancelMessageBody =
                            "Late Date:\t%LATEWAIVE_DATE%\r\n" +
                            "Late Mins:\t%LATEWAIVE_MINS%\r\n" +
                            "Reason   :\t%LATEWAIVE_REMARK%\r\n" +
                            "Reason for Cancellation:\t%LATEWAIVECANCEL_REMARK%\r\n" +
                            "\r\n" +
                            "Authorized By:\t%ACTION_USER%\r\n" +
                            "Reason:\t%ACTION_REASON%\r\n" +
                            "\r\n" +
                            "The above late waive cancellation request is approved.\r\n";
        // End 0000112, Miranda, 2014-12-10

        // Start 0000080, KuangWei, 2014-09-17
        //string LeaveMessageSubject = LoadOrCreateFile("EMAIL_SUBJECT_FOR_LEAVE_APPLICATION_APPROVED.txt", default_LeaveMessageSubject);
        //string LeaveMessageBody = LoadOrCreateFile("EMAIL_BODY_FOR_LEAVE_APPLICATION_APPROVED.txt", default_LeaveMessageBody);
        
        //string ProfileMessageSubject = LoadOrCreateFile("EMAIL_SUBJECT_FOR_PROFILE_CHANGE_APPROVED.txt", default_ProfileMessageSubject);
        //string ProfileMessageBody = LoadOrCreateFile("EMAIL_BODY_FOR_PROFILE_CHANGE_APPROVED.txt", default_ProfileMessageBody);

        //string LeaveCancelMessageSubject = LoadOrCreateFile("EMAIL_SUBJECT_FOR_LEAVE_CANCEL_APPROVED.txt", default_LeaveCancelMessageSubject);
        //string LeaveCancelMessageBody = LoadOrCreateFile("EMAIL_BODY_FOR_LEAVE_CANCEL_APPROVED.txt", default_LeaveCancelMessageBody);

        //string OTMessageSubject = LoadOrCreateFile("EMAIL_SUBJECT_FOR_OT_APPLICATION_APPROVED.txt", default_OTMessageSubject);
        //string OTMessageBody = LoadOrCreateFile("EMAIL_BODY_FOR_OT_APPLICATION_APPROVED.txt", default_OTMessageBody);

        //string OTCancelMessageSubject = LoadOrCreateFile("EMAIL_SUBJECT_FOR_OT_CANCEL_APPROVED.txt", default_OTCancelMessageSubject);
        //string OTCancelMessageBody = LoadOrCreateFile("EMAIL_BODY_FOR_OT_CANCEL_APPROVED.txt", default_OTCancelMessageBody);

        string LeaveMessageSubject = default_LeaveMessageSubject;
        string LeaveMessageBody = default_LeaveMessageBody;

        string ProfileMessageSubject = default_ProfileMessageSubject;
        string ProfileMessageBody = default_ProfileMessageBody;

        string LeaveCancelMessageSubject = default_LeaveCancelMessageSubject;
        string LeaveCancelMessageBody = default_LeaveCancelMessageBody;

        string OTMessageSubject = default_OTMessageSubject;
        string OTMessageBody = default_OTMessageBody;

        string OTCancelMessageSubject = default_OTCancelMessageSubject;
        string OTCancelMessageBody = default_OTCancelMessageBody;
        // End 0000080, KuangWei, 2014-09-17

        string messageSubject = string.Empty;
        string messageBody = string.Empty;
        if (ApplicationType.Equals(EEmpRequest.TYPE_EELEAVEAPP))
        {
            messageSubject = LeaveMessageSubject;
            messageBody = LeaveMessageBody;
        }
        else if (ApplicationType.Equals(EEmpRequest.TYPE_EEPROFILE))
        {
            messageSubject = ProfileMessageSubject;
            messageBody = ProfileMessageBody;
        }
        else if (ApplicationType.Equals(EEmpRequest.TYPE_EELEAVECANCEL))
        {
            messageSubject = LeaveCancelMessageSubject;
            messageBody = LeaveCancelMessageBody;
        }
        // Start 0000060, Miranda, 2014-07-13
        else if (ApplicationType.Equals(EEmpRequest.TYPE_EEOTCLAIM))
        {
            messageSubject = OTMessageSubject;
            messageBody = OTMessageBody;
        }
        else if (ApplicationType.Equals(EEmpRequest.TYPE_EEOTCLAIMCANCEL))
        {
            messageSubject = OTCancelMessageSubject;
            messageBody = OTCancelMessageBody;
        }
        // End 0000060, Miranda, 2014-07-13
        // Start 0000112, Miranda, 2014-12-10
        else if (ApplicationType.Equals(EEmpRequest.TYPE_EELATEWAIVE))
        {
            messageSubject = default_LateWaiveMessageSubject;
            messageBody = default_LateWaiveMessageBody;
        }
        else if (ApplicationType.Equals(EEmpRequest.TYPE_EELATEWAIVECANCEL))
        {
            messageSubject = default_LateWaiveCancelMessageSubject;
            messageBody = default_LateWaiveCancelMessageBody;
        }
        // End 0000112, Miranda, 2014-12-10
        SendEmailToApplicant(ApplicantEmpInfo, messageSubject, messageBody, parameterTable);
    }

    protected virtual void SendApplicantRejectedEmail(string ApplicationType, EEmpPersonalInfo ApplicantEmpInfo, Hashtable parameterTable)
    {
        // Start 0000080, KuangWei, 2014-09-16
        //const string default_LeaveMessageSubject = "Your leave application request is REJECTED";
        const string default_LeaveMessageSubject = "[Rejection of Leave Application] %EMP_NAME% %LEAVEAPP_DAYS% %LEAVEAPP_UNIT% %LEAVEAPP_CODE% on %LEAVEAPP_PERIOD%";
        // End 0000080, KuangWei, 2014-09-16
        const string default_LeaveMessageBody =
                            "Leave Date:\t%LEAVEAPP_PERIOD%\r\n" +
                            "Leave Type:\t%LEAVE_CODE%\r\n" +
                            "Taken     :\t%LEAVEAPP_DAYS% %LEAVEAPP_UNIT%\r\n" +
                            "Hours Claims:\t%LEAVEAPP_HOURSCLAIM%\r\n" +
                            "Remark    :\t%LEAVEAPP_REMARK%\r\n" +
                            "\r\n" +
                            "Rejected By:\t%ACTION_USER%\r\n" +
                            "Reason:\t%ACTION_REASON%\r\n" +
                            "\r\n" +
                            "The above leave application request is REJECTED.\r\n";
        const string default_ProfileMessageSubject = "Your employee information update request is REJECTED";
        const string default_ProfileMessageBody =
                                "Rejected By:\t%ACTION_USER%\r\n" +
                                "Reason:\t%ACTION_REASON%\r\n" +
                                "\r\n" +
                                "The above employee information update is REJECTED.\r\n";
        // Start 0000080, KuangWei, 2014-09-16
        //const string default_LeaveCancelMessageSubject = "Your leave application cancellation request is REJECTED";
        const string default_LeaveCancelMessageSubject = "[Rejection of Leave Cancellation] %EMP_NAME% %LEAVEAPP_DAYS% %LEAVEAPP_UNIT% %LEAVEAPP_CODE% on %LEAVEAPP_PERIOD%";
        // End 0000080, KuangWei, 2014-09-16
        const string default_LeaveCancelMessageBody =
                            "Leave Date:\t%LEAVEAPP_PERIOD%\r\n" +
                            "Leave Type:\t%LEAVE_CODE%\r\n" +
                            "Taken     :\t%LEAVEAPP_DAYS% %LEAVEAPP_UNIT%\r\n" +
                            "Hours Claims:\t%LEAVEAPP_HOURSCLAIM%\r\n" +
                            "Remark    :\t%LEAVEAPP_REMARK%\r\n" +
                            "Reason for Cancellation:\t%LEAVEAPPCANCEL_REMARK%\r\n" +
                            "\r\n" +
                            "Rejected By:\t%ACTION_USER%\r\n" +
                            "Reason:\t%ACTION_REASON%\r\n" +
                            "\r\n" +
                            "The above leave application cancellation request is REJECTED.\r\n";
        // Start 0000060, Miranda, 2014-07-13
        // Start 0000080, KuangWei, 2014-09-16
        //const string default_OTMessageSubject = "Your CL Requisition request is REJECTED";
        const string default_OTMessageSubject = "[Rejection of CL Requisition] %EMP_NAME% %OTCLAIM_HOURSCLAIM% hour(s) OT on %OTCLAIM_PERIOD%";
        // End 0000080, KuangWei, 2014-09-16
        const string default_OTMessageBody =
                            "CL Requisition Date:\t%OTCLAIM_PERIOD%\r\n" +
                            "Hours Claims:\t%OTCLAIM_HOURSCLAIM%\r\n" +
                            "Remark    :\t%OTCLAIM_REMARK%\r\n" +
                            "\r\n" +
                            "Rejected By:\t%ACTION_USER%\r\n" +
                            "Reason:\t%ACTION_REASON%\r\n" +
                            "\r\n" +
                            "The above CL Requisition request is REJECTED.\r\n";
        // Start 0000080, KuangWei, 2014-09-16
        //const string default_OTCancelMessageSubject = "Your CL Requisition cancellation request is REJECTED";
        const string default_OTCancelMessageSubject = "[Rejection of CL Requisition Cancellation] %EMP_NAME% %OTCLAIM_HOURSCLAIM% hour(s) OT on %OTCLAIM_PERIOD%";
        // End 0000080, KuangWei, 2014-09-16
        const string default_OTCancelMessageBody =
                            "CL Requisition Date:\t%OTCLAIM_PERIOD%\r\n" +
                            "Hours Claims:\t%OTCLAIM_HOURSCLAIM%\r\n" +
                            "Remark    :\t%OTCLAIM_REMARK%\r\n" +
                            "Reason for Cancellation:\t%OTCLAIMCANCEL_REMARK%\r\n" +
                            "\r\n" +
                            "Rejected By:\t%ACTION_USER%\r\n" +
                            "Reason:\t%ACTION_REASON%\r\n" +
                            "\r\n" +
                            "The above CL Requisition cancellation request is REJECTED.\r\n";
        // Start 0000112, Miranda, 2014-12-10
        const string default_LateWaiveMessageSubject = "[Rejection of Late Waive] %EMP_NAME% %LATEWAIVE_MINS% min(s) Late on %LATEWAIVE_DATE%";
        const string default_LateWaiveMessageBody =
                            "Late Date:\t%LATEWAIVE_DATE%\r\n" +
                            "Late Mins:\t%LATEWAIVE_MINS%\r\n" +
                            "Reason   :\t%LATEWAIVE_REMARK%\r\n" +
                            "\r\n" +
                            "Rejected By:\t%ACTION_USER%\r\n" +
                            "Reason:\t%ACTION_REASON%\r\n" +
                            "\r\n" +
                            "The above late waive request is REJECTED.\r\n";
        const string default_LateWaiveCancelMessageSubject = "Your late waive cancellation request is REJECTED";
        const string default_LateWaiveCancelMessageBody =
                            "Late Date:\t%LATEWAIVE_DATE%\r\n" +
                            "Late Mins:\t%LATEWAIVE_MINS%\r\n" +
                            "Reason   :\t%LATEWAIVE_REMARK%\r\n" +
                            "Reason for Cancellation:\t%LATEWAIVECANCEL_REMARK%\r\n" +
                            "\r\n" +
                            "Rejected By:\t%ACTION_USER%\r\n" +
                            "Reason:\t%ACTION_REASON%\r\n" +
                            "\r\n" +
                            "The above late waive cancellation request is REJECTED.\r\n";
        // End 0000112, Miranda, 2014-12-10
        // Start 0000080, KuangWei, 2014-09-17
        //string LeaveMessageSubject = LoadOrCreateFile("EMAIL_SUBJECT_FOR_LEAVE_APPLICATION_REJECT.txt", default_LeaveMessageSubject);
        //string LeaveMessageBody = LoadOrCreateFile("EMAIL_BODY_FOR_LEAVE_APPLICATION_REJECT.txt", default_LeaveMessageBody);

        //string ProfileMessageSubject = LoadOrCreateFile("EMAIL_SUBJECT_FOR_PROFILE_CHANGE_REJECT.txt", default_ProfileMessageSubject);
        //string ProfileMessageBody = LoadOrCreateFile("EMAIL_BODY_FOR_PROFILE_CHANGE_REJECT.txt", default_ProfileMessageBody);

        //string LeaveCancelMessageSubject = LoadOrCreateFile("EMAIL_SUBJECT_FOR_LEAVE_CANCEL_REJECT.txt", default_LeaveCancelMessageSubject);
        //string LeaveCancelMessageBody = LoadOrCreateFile("EMAIL_BODY_FOR_LEAVE_CANCEL_REJECT.txt", default_LeaveCancelMessageBody);

        //string OTMessageSubject = LoadOrCreateFile("EMAIL_SUBJECT_FOR_OT_APPLICATION_REJECT.txt", default_OTMessageSubject);
        //string OTMessageBody = LoadOrCreateFile("EMAIL_BODY_FOR_OT_APPLICATION_REJECT.txt", default_OTMessageBody);

        //string OTCancelMessageSubject = LoadOrCreateFile("EMAIL_SUBJECT_FOR_OT_CANCEL_REJECT.txt", default_OTCancelMessageSubject);
        //string OTCancelMessageBody = LoadOrCreateFile("EMAIL_BODY_FOR_OT_CANCEL_REJECT.txt", default_OTCancelMessageBody);

        string LeaveMessageSubject = default_LeaveMessageSubject;
        string LeaveMessageBody = default_LeaveMessageBody;

        string ProfileMessageSubject = default_ProfileMessageSubject;
        string ProfileMessageBody = default_ProfileMessageBody;

        string LeaveCancelMessageSubject = default_LeaveCancelMessageSubject;
        string LeaveCancelMessageBody = default_LeaveCancelMessageBody;

        string OTMessageSubject = default_OTMessageSubject;
        string OTMessageBody = default_OTMessageBody;

        string OTCancelMessageSubject = default_OTCancelMessageSubject;
        string OTCancelMessageBody =  default_OTCancelMessageBody;
        // End 0000080, KuangWei, 2014-09-17
        
        
        // End 0000060, Miranda, 2014-07-13
        string messageSubject = string.Empty;
        string messageBody = string.Empty;
        if (ApplicationType.Equals(EEmpRequest.TYPE_EELEAVEAPP))
        {
            messageSubject = LeaveMessageSubject;
            messageBody = LeaveMessageBody;
        }
        else if (ApplicationType.Equals(EEmpRequest.TYPE_EEPROFILE))
        {
            messageSubject = ProfileMessageSubject;
            messageBody = ProfileMessageBody;
        }
        else if (ApplicationType.Equals(EEmpRequest.TYPE_EELEAVECANCEL))
        {
            messageSubject = LeaveCancelMessageSubject;
            messageBody = LeaveCancelMessageBody;
        }
        // Start 0000060, Miranda, 2014-07-13
        else if (ApplicationType.Equals(EEmpRequest.TYPE_EEOTCLAIM))
        {
            messageSubject = OTMessageSubject;
            messageBody = OTMessageBody;
        }
        else if (ApplicationType.Equals(EEmpRequest.TYPE_EEOTCLAIMCANCEL))
        {
            messageSubject = OTCancelMessageSubject;
            messageBody = OTCancelMessageBody;
        }
        // End 0000060, Miranda, 2014-07-13
        // Start 0000112, Miranda, 2014-12-10
        else if (ApplicationType.Equals(EEmpRequest.TYPE_EELATEWAIVE))
        {
            messageSubject = default_LateWaiveMessageSubject;
            messageBody = default_LateWaiveMessageBody;
        }
        else if (ApplicationType.Equals(EEmpRequest.TYPE_EELATEWAIVECANCEL))
        {
            messageSubject = default_LateWaiveCancelMessageSubject;
            messageBody = default_LateWaiveCancelMessageBody;
        }
        // End 0000112, Miranda, 2014-12-10
        SendEmailToApplicant(ApplicantEmpInfo, messageSubject, messageBody, parameterTable);
    }
    protected virtual void SendCancelledByApplicantEmail(string ApplicationType, System.Collections.Generic.List<AuthorizerMailAddress> authorizerList, EEmpPersonalInfo ApplicantEmpInfo, Hashtable parameterTable)
    {
        // Start 0000080, KuangWei, 2014-09-16
        //const string LeaveMessageSubject = "%EMP_NO% %EMP_NAME% leave application is CANCELLED";
        const string LeaveMessageSubject = "[Leave Cancellation] %EMP_NAME% %LEAVEAPP_DAYS% %LEAVEAPP_UNIT% %LEAVEAPP_CODE% on %LEAVEAPP_PERIOD%";
        // End 0000080, KuangWei, 2014-09-16
        const string LeaveMessageBody = "Employee No:\t%EMP_NO%\r\n" +
                            "Employee:\t%EMP_NAME%\r\n" +
                            "Leave Date:\t%LEAVEAPP_PERIOD%\r\n" +
                            "Leave Type:\t%LEAVE_CODE%\r\n" +
                            "Taken     :\t%LEAVEAPP_DAYS% %LEAVEAPP_UNIT%\r\n" +
                            "Hours Claims:\t%LEAVEAPP_HOURSCLAIM%\r\n" +
                            "Remark    :\t%LEAVEAPP_REMARK%\r\n" +
                            "\r\n" +
                            "The above leave application request is CANCELLED.\r\n";
        const string ProfileMessageSubject = "%EMP_NO% %EMP_NAME% employee information update is CANCELLED";
        const string ProfileMessageBody = "Employee No:\t%EMP_NO%\r\n" +
                            "Employee:\t%EMP_NAME%\r\n" +
                            "\r\n" +
                            "The above employee information update is CANCELLED.\r\n";

        // Start 0000080, KuangWei, 2014-09-16
        //const string LeaveCancelMessageSubject = "%EMP_NO% %EMP_NAME% leave application cancellation request is CANCELLED";
        const string LeaveCancelMessageSubject = "[Leave Cancellation] %EMP_NAME% %LEAVEAPP_DAYS% %LEAVEAPP_UNIT% %LEAVEAPP_CODE% on %LEAVEAPP_PERIOD%";
        // End 0000080, KuangWei, 2014-09-16
        const string LeaveCancelMessageBody = "Employee No:\t%EMP_NO%\r\n" +
                            "Employee:\t%EMP_NAME%\r\n" +
                            "Leave Date:\t%LEAVEAPP_PERIOD%\r\n" +
                            "Leave Type:\t%LEAVE_CODE%\r\n" +
                            "Taken     :\t%LEAVEAPP_DAYS% %LEAVEAPP_UNIT%\r\n" +
                            "Remark    :\t%LEAVEAPP_REMARK%\r\n" +
                            "Reason for Cancellation:\t%LEAVEAPPCANCEL_REMARK%\r\n" +
                            "\r\n" +
                            "The above leave application cancellation request is CANCELLED.\r\n";
        // Start 0000060, Miranda, 2014-07-13
        // Start 0000080, KuangWei, 2014-09-16
        //const string OTMessageSubject = "%EMP_NO% %EMP_NAME% CL Requisition is CANCELLED";
        const string OTMessageSubject = "[CL Requisition Cancellation] %EMP_NAME% %OTCLAIM_HOURSCLAIM% hour(s) OT on %OTCLAIM_PERIOD%";
        // End 0000080, KuangWei, 2014-09-16
        const string OTMessageBody = "Employee No:\t%EMP_NO%\r\n" +
                            "Employee:\t%EMP_NAME%\r\n" +
                            "CL Requisition Date:\t%OTCLAIM_PERIOD%\r\n" +
                            "Hours Claims:\t%OTCLAIM_HOURSCLAIM%\r\n" +
                            "Remark    :\t%OTCLAIM_REMARK%\r\n" +
                            "\r\n" +
                            "The above CL Requisition request is CANCELLED.\r\n";
        // Start 0000080, KuangWei, 2014-09-16
        //const string OTCancelMessageSubject = "%EMP_NO% %EMP_NAME% CL Requisition cancellation request is CANCELLED";
        const string OTCancelMessageSubject = "[CL Requisition Cancellation] %EMP_NAME% %OTCLAIM_HOURSCLAIM% hour(s) OT on %OTCLAIM_PERIOD%";
        // End 0000080, KuangWei, 2014-09-16
        const string OTCancelMessageBody = "Employee No:\t%EMP_NO%\r\n" +
                            "Employee:\t%EMP_NAME%\r\n" +
                            "CL Requisition Date:\t%OTCLAIM_PERIOD%\r\n" +
                            "Remark    :\t%OTCLAIM_REMARK%\r\n" +
                            "Reason for Cancellation:\t%OTCLAIMCANCEL_REMARK%\r\n" +
                            "\r\n" +
                            "The above CL Requisition cancellation request is CANCELLED.\r\n";
        // End 0000060, Miranda, 2014-07-13
        // Start 0000112, Miranda, 2014-12-10
        const string LateWaiveMessageSubject = "[Late Waive Cancellation] %EMP_NAME% %LATEWAIVE_MINS% min(s) Late on %LATEWAIVE_DATE%";
        const string LateWaiveMessageBody = "Employee No:\t%EMP_NO%\r\n" +
                            "Employee:\t%EMP_NAME%\r\n" +
                            "Late Date:\t%LATEWAIVE_DATE%\r\n" +
                            "Late Mins:\t%LATEWAIVE_MINS%\r\n" +
                            "Reason   :\t%LATEWAIVE_REMARK%\r\n" +
                            "\r\n" +
                            "The above late waive request is CANCELLED.\r\n";
        const string LateWaiveCancelMessageSubject = "[Late Waive Cancellation] %EMP_NAME% %LATEWAIVE_MINS% min(s) Late on %LATEWAIVE_DATE%";
        const string LateWaiveCancelMessageBody = "Employee No:\t%EMP_NO%\r\n" +
                            "Employee:\t%EMP_NAME%\r\n" +
                            "Late Date:\t%LATEWAIVE_DATE%\r\n" +
                            "Reason   :\t%LATEWAIVE_REMARK%\r\n" +
                            "Reason for Cancellation:\t%LATEWAIVECANCEL_REMARK%\r\n" +
                            "\r\n" +
                            "The above late waive cancellation request is CANCELLED.\r\n";
        // End 0000112, Miranda, 2014-12-10

        string messageSubject = string.Empty;
        string messageBody = string.Empty;
        if (ApplicationType.Equals(EEmpRequest.TYPE_EELEAVEAPP))
        {
            messageSubject = LeaveMessageSubject;
            messageBody = LeaveMessageBody;
        }
        else if (ApplicationType.Equals(EEmpRequest.TYPE_EEPROFILE))
        {
            messageSubject = ProfileMessageSubject;
            messageBody = ProfileMessageBody;
        }
        else if (ApplicationType.Equals(EEmpRequest.TYPE_EELEAVECANCEL))
        {
            messageSubject = LeaveCancelMessageSubject;
            messageBody = LeaveCancelMessageBody;
        }
        // Start 0000060, Miranda, 2014-07-13
        else if (ApplicationType.Equals(EEmpRequest.TYPE_EEOTCLAIM))
        {
            messageSubject = OTMessageSubject;
            messageBody = OTMessageBody;
        }
        else if (ApplicationType.Equals(EEmpRequest.TYPE_EEOTCLAIMCANCEL))
        {
            messageSubject = OTCancelMessageSubject;
            messageBody = OTCancelMessageBody;
        }
        // End 0000060, Miranda, 2014-07-13
        // Start 0000112, Miranda, 2014-12-10
        else if (ApplicationType.Equals(EEmpRequest.TYPE_EELATEWAIVE))
        {
            messageSubject = LateWaiveMessageSubject;
            messageBody = LateWaiveMessageBody;
        }
        else if (ApplicationType.Equals(EEmpRequest.TYPE_EELATEWAIVECANCEL))
        {
            messageSubject = LateWaiveCancelMessageSubject;
            messageBody = LateWaiveCancelMessageBody;
        }
        // End 0000112, Miranda, 2014-12-10
        SendEmail(authorizerList, ApplicantEmpInfo, messageSubject, messageBody, parameterTable);
    }

    protected virtual void SendApprovedEmail(string ApplicationType, System.Collections.Generic.List<AuthorizerMailAddress> authorizerList, EEmpPersonalInfo ApplicantEmpInfo, Hashtable parameterTable)
    {
        // Start 0000080, KuangWei, 2014-09-16
        //const string LeaveMessageSubject = "%EMP_NO% %EMP_NAME% leave application request is approved";
        const string LeaveMessageSubject = "[Leave Application] %EMP_NAME% %LEAVEAPP_DAYS% %LEAVEAPP_UNIT% %LEAVEAPP_CODE% on %LEAVEAPP_PERIOD%";
        // End 0000080, KuangWei, 2014-09-16
        const string LeaveMessageBody = "Employee No:\t%EMP_NO%\r\n" +
                            "Employee:\t%EMP_NAME%\r\n" +
                            "Leave Date:\t%LEAVEAPP_PERIOD%\r\n" +
                            "Leave Type:\t%LEAVE_CODE%\r\n" +
                            "Taken     :\t%LEAVEAPP_DAYS% %LEAVEAPP_UNIT%\r\n" +
                            "Hours Claims:\t%LEAVEAPP_HOURSCLAIM%\r\n" +
                            "Remark    :\t%LEAVEAPP_REMARK%\r\n" +
                            "Approved By:\t%ACTION_USER%\r\n" +
                            "Reason:\t%ACTION_REASON%\r\n" +
                            "\r\n" +
                            "The above leave application request is approved.\r\n";
        const string ProfileMessageSubject = "%EMP_NO% %EMP_NAME% employee information update request is approved";
        const string ProfileMessageBody = "Employee No: %EMP_NO%\r\n" +
                            "Employee: %EMP_NAME%\r\n" +
                            "Approved By:\t%ACTION_USER%\r\n" +
                            "Reason:\t%ACTION_REASON%\r\n" +
                            "\r\n" +
                            "The above employee information update is approved.\r\n";
        // Start 0000080, KuangWei, 2014-09-16
        //const string LeaveCancelMessageSubject = "%EMP_NO% %EMP_NAME% leave application cancellation request is approved";
        const string LeaveCancelMessageSubject = "[Leave Application] %EMP_NAME% %LEAVEAPP_DAYS% %LEAVEAPP_UNIT% %LEAVEAPP_CODE% on %LEAVEAPP_PERIOD%";
        // End 0000080, KuangWei, 2014-09-16
        const string LeaveCancelMessageBody = "Employee No:\t%EMP_NO%\r\n" +
                            "Employee:\t%EMP_NAME%\r\n" +
                            "Leave Date:\t%LEAVEAPP_PERIOD%\r\n" +
                            "Leave Type:\t%LEAVE_CODE%\r\n" +
                            "Taken     :\t%LEAVEAPP_DAYS% %LEAVEAPP_UNIT%\r\n" +
                            "Hours Claims:\t%LEAVEAPP_HOURSCLAIM%\r\n" +
                            "Remark    :\t%LEAVEAPP_REMARK%\r\n" +
                            "Reason for Cancellation:\t%LEAVEAPPCANCEL_REMARK%\r\n" +
                            "Approved By:\t%ACTION_USER%\r\n" +
                            "Reason:\t%ACTION_REASON%\r\n" +
                            "\r\n" +
                            "The above leave application cancellation request is approved.\r\n";
        // Start 0000060, Miranda, 2014-07-13
        // Start 0000080, KuangWei, 2014-09-16
        //const string OTMessageSubject = "%EMP_NO% %EMP_NAME% CL Requisition request is approved";
        const string OTMessageSubject = "[CL Requisition] %EMP_NAME% %OTCLAIM_HOURSCLAIM% hour(s) OT on %OTCLAIM_PERIOD%";
        // End 0000080, KuangWei, 2014-09-16
        const string OTMessageBody = "Employee No:\t%EMP_NO%\r\n" +
                            "Employee:\t%EMP_NAME%\r\n" +
                            "CL Requisition Date:\t%OTCLAIM_PERIOD%\r\n" +
                            "Hours Claims:\t%OTCLAIM_HOURSCLAIM%\r\n" +
                            "Remark    :\t%OTCLAIM_REMARK%\r\n" +
                            "Approved By:\t%ACTION_USER%\r\n" +
                            "Reason:\t%ACTION_REASON%\r\n" +
                            "\r\n" +
                            "The above CL Requisition request is approved.\r\n";
        // Start 0000080, KuangWei, 2014-09-16
        //const string OTCancelMessageSubject = "%EMP_NO% %EMP_NAME% CL Requisition cancellation request is approved";
        const string OTCancelMessageSubject = "[CL Requisition] %EMP_NAME% %OTCLAIM_HOURSCLAIM% hour(s) OT on %OTCLAIM_PERIOD%";
        // End 0000080, KuangWei, 2014-09-16
        const string OTCancelMessageBody = "Employee No:\t%EMP_NO%\r\n" +
                            "Employee:\t%EMP_NAME%\r\n" +
                            "CL Requisition Date:\t%OTCLAIM_PERIOD%\r\n" +
                            "Hours Claims:\t%OTCLAIM_HOURSCLAIM%\r\n" +
                            "Remark    :\t%OTCLAIM_REMARK%\r\n" +
                            "Reason for Cancellation:\t%OTCLAIMCANCEL_REMARK%\r\n" +
                            "Approved By:\t%ACTION_USER%\r\n" +
                            "Reason:\t%ACTION_REASON%\r\n" +
                            "\r\n" +
                            "The above CL Requisition cancellation request is approved.\r\n";
        // End 0000060, Miranda, 2014-07-13
        // Start 0000112, Miranda, 2014-12-10
        const string LateWaiveMessageSubject = "[Late Waive] %EMP_NAME% %LATEWAIVE_MINS% min(s) late on %LATEWAIVE_DATE%";
        const string LateWaiveMessageBody = "Employee No:\t%EMP_NO%\r\n" +
                            "Employee   :\t%EMP_NAME%\r\n" +
                            "Late Date  :\t%LATEWAIVE_DATE%\r\n" +
                            "Late Mins  :\t%LATEWAIVE_MINS%\r\n" +
                            "Late Reason:\t%LATEWAIVE_REMARK%\r\n" +
                            "Approved By:\t%ACTION_USER%\r\n" +
                            "Reason:\t%ACTION_REASON%\r\n" +
                            "\r\n" +
                            "The above late waive request is approved.\r\n";
        const string LateWaiveCancelMessageSubject = "[Late Waive] %EMP_NAME% %LATEWAIVE_MINS% min(s) late on %LATEWAIVE_DATE%";
        const string LateWaiveCancelMessageBody = "Employee No:\t%EMP_NO%\r\n" +
                            "Employee   :\t%EMP_NAME%\r\n" +
                            "Late Date  :\t%LATEWAIVE_DATE%\r\n" +
                            "Late Mins  :\t%LATEWAIVE_MINS%\r\n" +
                            "Late Reason:\t%LATEWAIVE_REMARK%\r\n" +
                            "Reason for Cancellation:\t%LATEWAIVECANCEL_REMARK%\r\n" +
                            "Approved By:\t%ACTION_USER%\r\n" +
                            "Reason:\t%ACTION_REASON%\r\n" +
                            "\r\n" +
                            "The above late waive cancellation request is approved.\r\n";
        // End 0000112, Miranda, 2014-12-10
        string messageSubject = string.Empty;
        string messageBody = string.Empty;
        if (ApplicationType.Equals(EEmpRequest.TYPE_EELEAVEAPP))
        {
            messageSubject = LeaveMessageSubject;
            messageBody = LeaveMessageBody;
        }
        else if (ApplicationType.Equals(EEmpRequest.TYPE_EEPROFILE))
        {
            messageSubject = ProfileMessageSubject;
            messageBody = ProfileMessageBody;
        }
        else if (ApplicationType.Equals(EEmpRequest.TYPE_EELEAVECANCEL))
        {
            messageSubject = LeaveCancelMessageSubject;
            messageBody = LeaveCancelMessageBody;
        }
        // Start 0000060, Miranda, 2014-07-13
        else if (ApplicationType.Equals(EEmpRequest.TYPE_EEOTCLAIM))
        {
            messageSubject = OTMessageSubject;
            messageBody = OTMessageBody;
        }
        else if (ApplicationType.Equals(EEmpRequest.TYPE_EEOTCLAIMCANCEL))
        {
            messageSubject = OTCancelMessageSubject;
            messageBody = OTCancelMessageBody;
        }
        // End 0000060, Miranda, 2014-07-13
        // Start 0000112, Miranda, 2014-12-10
        else if (ApplicationType.Equals(EEmpRequest.TYPE_EELATEWAIVE))
        {
            messageSubject = LateWaiveMessageSubject;
            messageBody = LateWaiveMessageBody;
        }
        else if (ApplicationType.Equals(EEmpRequest.TYPE_EELATEWAIVECANCEL))
        {
            messageSubject = LateWaiveCancelMessageSubject;
            messageBody = LateWaiveCancelMessageBody;
        }
        // End 0000112, Miranda, 2014-12-10
        SendEmail(authorizerList, ApplicantEmpInfo, messageSubject, messageBody, parameterTable);
    }

    protected virtual void SendRejectedEmail(string ApplicationType, System.Collections.Generic.List<AuthorizerMailAddress> authorizerList, EEmpPersonalInfo ApplicantEmpInfo, Hashtable parameterTable)
    {
        // Start 0000080, KuangWei, 2014-09-16
        //string LeaveMessageSubject = "%EMP_NO% %EMP_NAME% leave application request is REJECTED";
        string LeaveMessageSubject = "[Rejection of Leave Application] %EMP_NAME% %LEAVEAPP_DAYS% %LEAVEAPP_UNIT% %LEAVEAPP_CODE% on %LEAVEAPP_PERIOD%";
        // End 0000080, KuangWei, 2014-09-16
        string LeaveMessageBody = "Employee No:\t%EMP_NO%\r\n" +
                            "Employee:\t%EMP_NAME%\r\n" +
                            "Leave Date:\t%LEAVEAPP_PERIOD%\r\n" +
                            "Leave Type:\t%LEAVE_CODE%\r\n" +
                            "Taken     :\t%LEAVEAPP_DAYS% %LEAVEAPP_UNIT%\r\n" +
                            "Hours Claims:\t%LEAVEAPP_HOURSCLAIM%\r\n" +
                            "Remark    :\t%LEAVEAPP_REMARK%\r\n" +
                            "Rejected By:\t%ACTION_USER%\r\n" +
                            "Reason:\t%ACTION_REASON%\r\n" +
                            "\r\n" +
                            "The above leave application request is REJECTED.\r\n";

        const string ProfileMessageSubject = "%EMP_NO% %EMP_NAME% employee information update request is REJECTED";
        const string ProfileMessageBody = "Employee No: %EMP_NO%\r\n" +
                            "Employee: %EMP_NAME%\r\n" +
                            "Rejected By:\t%ACTION_USER%\r\n" +
                            "Reason:\t%ACTION_REASON%\r\n" +
                            "\r\n" +
                            "The above employee information update is REJECTED.\r\n";

        // Start 0000080, KuangWei, 2014-09-16
        //string LeaveCancelMessageSubject = "%EMP_NO% %EMP_NAME% leave application cancellation request is REJECTED";
        string LeaveCancelMessageSubject = "[Rejection of Leave Cancellation] %EMP_NAME% %LEAVEAPP_DAYS% %LEAVEAPP_UNIT% %LEAVEAPP_CODE% on %LEAVEAPP_PERIOD%";
        // End 0000080, KuangWei, 2014-09-16
        string LeaveCancelMessageBody = "Employee No:\t%EMP_NO%\r\n" +
                            "Employee:\t%EMP_NAME%\r\n" +
                            "Leave Date:\t%LEAVEAPP_PERIOD%\r\n" +
                            "Leave Type:\t%LEAVE_CODE%\r\n" +
                            "Taken     :\t%LEAVEAPP_DAYS% %LEAVEAPP_UNIT%\r\n" +
                            "Hours Claims:\t%LEAVEAPP_HOURSCLAIM%\r\n" +
                            "Remark    :\t%LEAVEAPP_REMARK%\r\n" +
                            "Reason for Cancellation:\t%LEAVEAPPCANCEL_REMARK%\r\n" +
                            "Rejected By:\t%ACTION_USER%\r\n" +
                            "Reason:\t%ACTION_REASON%\r\n" +
                            "\r\n" +
                            "The above leave application cancellation request is REJECTED.\r\n";
        // Start 0000060, Miranda, 2014-07-13
        // Start 0000080, KuangWei, 2014-09-16
        //string OTMessageSubject = "%EMP_NO% %EMP_NAME% CL Requisition request is REJECTED";
        string OTMessageSubject = "[Rejection of CL Requisition] %EMP_NAME% %OTCLAIM_HOURSCLAIM% hour(s) OT on %OTCLAIM_PERIOD%";
        // End 0000080, KuangWei, 2014-09-16
        string OTMessageBody = "Employee No:\t%EMP_NO%\r\n" +
                            "Employee:\t%EMP_NAME%\r\n" +
                            "CL Requisition Date:\t%OTCLAIM_PERIOD%\r\n" +
                            "Hours Claims:\t%OTCLAIM_HOURSCLAIM%\r\n" +
                            "Remark    :\t%OTCLAIM_REMARK%\r\n" +
                            "Rejected By:\t%ACTION_USER%\r\n" +
                            "Reason:\t%ACTION_REASON%\r\n" +
                            "\r\n" +
                            "The above CL Requisition request is REJECTED.\r\n";
        // Start 0000080, KuangWei, 2014-09-16
        //string OTCancelMessageSubject = "%EMP_NO% %EMP_NAME% CL Requisition cancellation request is REJECTED";
        string OTCancelMessageSubject = "[Rejection of CL Requisition Cancellation] %EMP_NAME% %OTCLAIM_HOURSCLAIM% hour(s) OT on %OTCLAIM_PERIOD%";
        // End 0000080, KuangWei, 2014-09-16
        string OTCancelMessageBody = "Employee No:\t%EMP_NO%\r\n" +
                            "Employee:\t%EMP_NAME%\r\n" +
                            "CL Requisition Date:\t%OTCLAIM_PERIOD%\r\n" +
                            "Hours Claims:\t%OTCLAIM_HOURSCLAIM%\r\n" +
                            "Remark    :\t%OTCLAIM_REMARK%\r\n" +
                            "Reason for Cancellation:\t%OTCLAIMCANCEL_REMARK%\r\n" +
                            "Rejected By:\t%ACTION_USER%\r\n" +
                            "Reason:\t%ACTION_REASON%\r\n" +
                            "\r\n" +
                            "The above CL Requisition cancellation request is REJECTED.\r\n";
        // End 0000060, Miranda, 2014-07-13
        // Start 0000112, Miranda, 2014-12-10
        string LateWaiveMessageSubject = "[Rejection of Late Waive] %EMP_NAME% %LATEWAIVE_MINS% min(s) Late on %LATEWAIVE_DATE%";
        string LateWaiveMessageBody = "Employee No:\t%EMP_NO%\r\n" +
                            "Employee   :\t%EMP_NAME%\r\n" +
                            "Late Date  :\t%LATEWAIVE_DATE%\r\n" +
                            "Late Mins  :\t%LATEWAIVE_MINS%\r\n" +
                            "Late Reason:\t%LATEWAIVE_REMARK%\r\n" +
                            "Rejected By:\t%ACTION_USER%\r\n" +
                            "Reason:\t%ACTION_REASON%\r\n" +
                            "\r\n" +
                            "The above late waive request is REJECTED.\r\n";
        string LateWaiveCancelMessageSubject = "[Rejection of Late Waive Cancellation] %EMP_NAME% %LATEWAIVE_MINS% min(s) Late on %LATEWAIVE_DATE%";
        string LateWaiveCancelMessageBody = "Employee No:\t%EMP_NO%\r\n" +
                            "Employee   :\t%EMP_NAME%\r\n" +
                            "Late Date  :\t%LATEWAIVE_DATE%\r\n" +
                            "Late Mins  :\t%LATEWAIVE_MINS%\r\n" +
                            "Late Reason:\t%LATEWAIVE_REMARK%\r\n" +
                            "Reason for Cancellation:\t%LATEWAIVECANCEL_REMARK%\r\n" +
                            "Rejected By:\t%ACTION_USER%\r\n" +
                            "Reason:\t%ACTION_REASON%\r\n" +
                            "\r\n" +
                            "The above late waive cancellation request is REJECTED.\r\n";
        // End 0000112, Miranda, 2014-12-10
        string messageSubject = string.Empty;
        string messageBody = string.Empty;
        if (ApplicationType.Equals(EEmpRequest.TYPE_EELEAVEAPP))
        {
            messageSubject = LeaveMessageSubject;
            messageBody = LeaveMessageBody;
        }
        else if (ApplicationType.Equals(EEmpRequest.TYPE_EEPROFILE))
        {
            messageSubject = ProfileMessageSubject;
            messageBody = ProfileMessageBody;
        }
        else if (ApplicationType.Equals(EEmpRequest.TYPE_EELEAVECANCEL))
        {
            messageSubject = LeaveCancelMessageSubject;
            messageBody = LeaveCancelMessageBody;
        }
        // Start 0000060, Miranda, 2014-07-13
        else if (ApplicationType.Equals(EEmpRequest.TYPE_EEOTCLAIM))
        {
            messageSubject = OTMessageSubject;
            messageBody = OTMessageBody;
        }
        else if (ApplicationType.Equals(EEmpRequest.TYPE_EEOTCLAIMCANCEL))
        {
            messageSubject = OTCancelMessageSubject;
            messageBody = OTCancelMessageBody;
        }
        // End 0000060, Miranda, 2014-07-13
        // Start 0000112, Miranda, 2014-12-10
        else if (ApplicationType.Equals(EEmpRequest.TYPE_EELATEWAIVE))
        {
            messageSubject = LateWaiveMessageSubject;
            messageBody = LateWaiveMessageBody;
        }
        else if (ApplicationType.Equals(EEmpRequest.TYPE_EELATEWAIVECANCEL))
        {
            messageSubject = LateWaiveCancelMessageSubject;
            messageBody = LateWaiveCancelMessageBody;
        }
        // End 0000112, Miranda, 2014-12-10
        SendEmail(authorizerList, ApplicantEmpInfo, messageSubject, messageBody, parameterTable);
    }

    protected virtual void SendAcceptEmail(string ApplicationType, System.Collections.Generic.List<AuthorizerMailAddress> mailAddressList, EEmpPersonalInfo ApplicantEmpInfo, Hashtable parameterTable)
    {
        // Start 0000080, KuangWei, 2014-09-16
        //const string default_LeaveMessageSubject = "%EMP_NO% %EMP_NAME% leave application request is accepted";
        const string default_LeaveMessageSubject = "[Leave Application] %EMP_NAME% %LEAVEAPP_DAYS% %LEAVEAPP_UNIT% %LEAVEAPP_CODE% on %LEAVEAPP_PERIOD%";
        // End 0000080, KuangWei, 2014-09-16
        const string default_LeaveMessageBody = "Employee No:\t%EMP_NO%\r\n" +
                            "Employee:\t%EMP_NAME%\r\n" +
                            "Leave Date:\t%LEAVEAPP_PERIOD%\r\n" +
                            "Leave Type:\t%LEAVE_CODE%\r\n" +
                            "Taken     :\t%LEAVEAPP_DAYS% %LEAVEAPP_UNIT%\r\n" +
                            "Hours Claims:\t%LEAVEAPP_HOURSCLAIM%\r\n" +
                            "Remark    :\t%LEAVEAPP_REMARK%\r\n" +
                            "Accepted By:\t%ACTION_USER%\r\n" +
                            "Reason:\t%ACTION_REASON%\r\n" +
                            "\r\n" +
                            "The above leave application request is accepted and is passed to another authorizer.\r\n";

        const string default_ProfileMessageSubject = "%EMP_NO% %EMP_NAME% employee information update request is accepted";
        const string default_ProfileMessageBody = "Employee No: %EMP_NO%\r\n" +
                            "Employee: %EMP_NAME%\r\n" +
                            "Accepted By:\t%ACTION_USER%\r\n" +
                            "Reason:\t%ACTION_REASON%\r\n" +
                            "\r\n" +
                            "The above employee information update request is accepted and is passed to another authorizer.\r\n";

        // Start 0000080, KuangWei, 2014-09-16
        //const string default_LeaveCancelMessageSubject = "%EMP_NO% %EMP_NAME% leave application cancellation request is accepted";
        const string default_LeaveCancelMessageSubject = "[Leave application] %EMP_NAME% %LEAVEAPP_DAYS% %LEAVEAPP_UNIT% %LEAVEAPP_CODE% on %LEAVEAPP_PERIOD%";
        // End 0000080, KuangWei, 2014-09-16
        const string default_LeaveCancelMessageBody = "Employee No:\t%EMP_NO%\r\n" +
                            "Employee:\t%EMP_NAME%\r\n" +
                            "Leave Date:\t%LEAVEAPP_PERIOD%\r\n" +
                            "Leave Type:\t%LEAVE_CODE%\r\n" +
                            "Taken     :\t%LEAVEAPP_DAYS% %LEAVEAPP_UNIT%\r\n" +
                            "Hours Claims:\t%LEAVEAPP_HOURSCLAIM%\r\n" +
                            "Remark    :\t%LEAVEAPP_REMARK%\r\n" +
                            "Reason for Cancellation:\t%LEAVEAPPCANCEL_REMARK%\r\n" +
                            "Accepted By:\t%ACTION_USER%\r\n" +
                            "Reason:\t%ACTION_REASON%\r\n" +
                            "\r\n" +
                            "The above leave application cancellation request is accepted and is passed to another authorizer.\r\n";

        // Start 0000060, Miranda, 2014-07-13
        // Start 0000080, KuangWei, 2014-09-16
        //const string default_OTMessageSubject = "%EMP_NO% %EMP_NAME% CL Requisition request is accepted";
        const string default_OTMessageSubject = "[CL Requisition] %EMP_NAME% %OTCLAIM_HOURSCLAIM% hour(s) OT on %OTCLAIM_PERIOD%";
        // End 0000080, KuangWei, 2014-09-16
        const string default_OTMessageBody = "Employee No:\t%EMP_NO%\r\n" +
                            "Employee:\t%EMP_NAME%\r\n" +
                            "CL Requisition Date:\t%OTCLAIM_PERIOD%\r\n" +
                            "Hours Claims:\t%OTCLAIM_HOURSCLAIM%\r\n" +
                            "Remark    :\t%OTCLAIM_REMARK%\r\n" +
                            "Accepted By:\t%ACTION_USER%\r\n" +
                            "Reason:\t%ACTION_REASON%\r\n" +
                            "\r\n" +
                            "The above CL Requisition request is accepted and is passed to another authorizer.\r\n";
        // Start 0000080, KuangWei, 2014-09-16
        //const string default_OTCancelMessageSubject = "%EMP_NO% %EMP_NAME% CL Requisition cancellation request is accepted";
        const string default_OTCancelMessageSubject = "[CL Requisition] %EMP_NAME% %OTCLAIM_HOURSCLAIM% hour(s) OT on %OTCLAIM_PERIOD%";
        // End 0000080, KuangWei, 2014-09-16
        const string default_OTCancelMessageBody = "Employee No:\t%EMP_NO%\r\n" +
                            "Employee:\t%EMP_NAME%\r\n" +
                            "CL Requisition Date:\t%OTCLAIM_PERIOD%\r\n" +
                            "Hours Claims:\t%OTCLAIM_HOURSCLAIM%\r\n" +
                            "Remark    :\t%OTCLAIM_REMARK%\r\n" +
                            "Reason for Cancellation:\t%OTCLAIMCANCEL_REMARK%\r\n" +
                            "Accepted By:\t%ACTION_USER%\r\n" +
                            "Reason:\t%ACTION_REASON%\r\n" +
                            "\r\n" +
                            "The above CL Requisition cancellation request is accepted and is passed to another authorizer.\r\n";
        // End 0000060, Miranda, 2014-07-13
        // Start 0000112, Miranda, 2014-12-10
        const string default_LateWaiveMessageSubject = "[Late Waive] %EMP_NAME% %LATEWAIVE_MINS% min(s) Late on %LATEWAIVE_DATE%";
        const string default_LateWaiveMessageBody = "Employee No:\t%EMP_NO%\r\n" +
                            "Employee:\t%EMP_NAME%\r\n" +
                            "Late Date:\t%LATEWAIVE_DATE%\r\n" +
                            "Late Mins:\t%LATEWAIVE_MINS%\r\n" +
                            "Late Reason:\t%LATEWAIVE_REMARK%\r\n" +
                            "Accepted By:\t%ACTION_USER%\r\n" +
                            "Reason:\t%ACTION_REASON%\r\n" +
                            "\r\n" +
                            "The above late waive request is accepted and is passed to another authorizer.\r\n";
        const string default_LateWaiveCancelMessageSubject = "[Late Waive] %EMP_NAME% %LATEWAIVE_MINS% min(s) Late on %LATEWAIVE_DATE%";
        const string default_LateWaiveCancelMessageBody = "Employee No:\t%EMP_NO%\r\n" +
                            "Employee:\t%EMP_NAME%\r\n" +
                            "Late Date:\t%LATEWAIVE_DATE%\r\n" +
                            "Late Mins:\t%LATEWAIVE_MINS%\r\n" +
                            "Late Reason:\t%LATEWAIVE_REMARK%\r\n" +
                            "Reason for Cancellation:\t%LATEWAIVECANCEL_REMARK\r\n" +
                            "Accepted By:\t%ACTION_USER%\r\n" +
                            "Reason:\t%ACTION_REASON%\r\n" +
                            "\r\n" +
                            "The above late waive cancellation request is accepted and is passed to another authorizer.\r\n";
        // End 0000112, Miranda, 2014-12-10

        // Start 0000080, KuangWei, 2014-09-17
        //string LeaveMessageSubject = LoadOrCreateFile("EMAIL_SUBJECT_FOR_LEAVE_APPLICATION_ACCEPTED.txt", default_LeaveMessageSubject);
        //string LeaveMessageBody = LoadOrCreateFile("EMAIL_BODY_FOR_LEAVE_APPLICATION_ACCEPTED.txt", default_LeaveMessageBody);

        //string ProfileMessageSubject = LoadOrCreateFile("EMAIL_SUBJECT_FOR_PROFILE_CHANGE_ACCEPTED.txt", default_ProfileMessageSubject);
        //string ProfileMessageBody = LoadOrCreateFile("EMAIL_BODY_FOR_PROFILE_CHANGE_ACCEPTED.txt", default_ProfileMessageBody);

        //string LeaveCancelMessageSubject = LoadOrCreateFile("EMAIL_SUBJECT_FOR_LEAVE_CANCEL_ACCEPTED.txt", default_LeaveCancelMessageSubject);
        //string LeaveCancelMessageBody = LoadOrCreateFile("EMAIL_BODY_FOR_LEAVE_CANCEL_ACCEPTED.txt", default_LeaveCancelMessageBody);

        //string OTMessageSubject = LoadOrCreateFile("EMAIL_SUBJECT_FOR_OT_APPLICATION_ACCEPTED.txt", default_OTMessageSubject);
        //string OTMessageBody = LoadOrCreateFile("EMAIL_BODY_FOR_OT_APPLICATION_ACCEPTED.txt", default_OTMessageBody);

        //string OTCancelMessageSubject = LoadOrCreateFile("EMAIL_SUBJECT_FOR_OT_CANCEL_ACCEPTED.txt", default_OTCancelMessageSubject);
        //string OTCancelMessageBody = LoadOrCreateFile("EMAIL_BODY_FOR_OT_CANCEL_ACCEPTED.txt", default_OTCancelMessageBody);

        string LeaveMessageSubject = default_LeaveMessageSubject;
        string LeaveMessageBody = default_LeaveMessageBody;

        string ProfileMessageSubject = default_ProfileMessageSubject;
        string ProfileMessageBody = default_ProfileMessageBody;

        string LeaveCancelMessageSubject = default_LeaveCancelMessageSubject;
        string LeaveCancelMessageBody = default_LeaveCancelMessageBody;

        string OTMessageSubject = default_OTMessageSubject;
        string OTMessageBody = default_OTMessageBody;

        string OTCancelMessageSubject = default_OTCancelMessageSubject;
        string OTCancelMessageBody = default_OTCancelMessageBody;
        // End 0000080, KuangWei, 2014-09-17
        
        string messageSubject = string.Empty;
        string messageBody = string.Empty;
        if (ApplicationType.Equals(EEmpRequest.TYPE_EELEAVEAPP))
        {
            messageSubject = LeaveMessageSubject;
            messageBody = LeaveMessageBody;
        }
        else if (ApplicationType.Equals(EEmpRequest.TYPE_EEPROFILE))
        {
            messageSubject = ProfileMessageSubject;
            messageBody = ProfileMessageBody;
        }
        else if (ApplicationType.Equals(EEmpRequest.TYPE_EELEAVECANCEL))
        {
            messageSubject = LeaveCancelMessageSubject;
            messageBody = LeaveCancelMessageBody;
        }
        // Start 0000060, Miranda, 2014-07-13
        else if (ApplicationType.Equals(EEmpRequest.TYPE_EEOTCLAIM))
        {
            messageSubject = OTMessageSubject;
            messageBody = OTMessageBody;
        }
        else if (ApplicationType.Equals(EEmpRequest.TYPE_EEOTCLAIMCANCEL))
        {
            messageSubject = OTCancelMessageSubject;
            messageBody = OTCancelMessageBody;
        }
        // End 0000060, Miranda, 2014-07-13
        // Start 0000112, Miranda, 2014-12-10
        else if (ApplicationType.Equals(EEmpRequest.TYPE_EELATEWAIVE))
        {
            messageSubject = default_LateWaiveMessageSubject;
            messageBody = default_LateWaiveMessageBody;
        }
        else if (ApplicationType.Equals(EEmpRequest.TYPE_EELATEWAIVECANCEL))
        {
            messageSubject = default_LateWaiveCancelMessageSubject;
            messageBody = default_LateWaiveCancelMessageBody;
        }
        // End 0000112, Miranda, 2014-12-10
        SendEmail(mailAddressList, ApplicantEmpInfo, messageSubject, messageBody, parameterTable);
    }

    protected virtual void SendRequestEmail(string ApplicationType, System.Collections.Generic.List<AuthorizerMailAddress> mailAddressList, EEmpPersonalInfo ApplicantEmpInfo, Hashtable parameterTable)
    {
        // Start 0000080, KuangWei, 2014-09-15
        //const string LeaveMessageSubject = "%EMP_NO% %EMP_NAME% leave application request";
        const string LeaveMessageSubject = "[Leave Application] %EMP_NAME% %LEAVEAPP_DAYS% %LEAVEAPP_UNIT% %LEAVEAPP_CODE% on %LEAVEAPP_PERIOD%";
        // End 0000080, KuangWei, 2014-09-15
        const string LeaveMessageBody = "Employee No:\t%EMP_NO%\r\n" +
                            "Employee:\t%EMP_NAME%\r\n" +
                            "Leave Date:\t%LEAVEAPP_PERIOD%\r\n" +
                            "Leave Type:\t%LEAVE_CODE%\r\n" +
                            "Taken     :\t%LEAVEAPP_DAYS% %LEAVEAPP_UNIT%\r\n" +
                            "Hours Claims:\t%LEAVEAPP_HOURSCLAIM%\r\n" +
                            "Remark    :\t%LEAVEAPP_REMARK%\r\n" +
                            "Last Accepted By:\t%ACTION_USER%\r\n" +
                            "Reason:\t%ACTION_REASON%\r\n" +
                            "\r\n" +
                            "The above employee submitted a leave application request.\r\n" +
                            "Please login to HROne Employee Self Service to approve/ reject the following request.\r\n" +
                            "%REQUEST_URL%\r\n";

        const string ProfileMessageSubject = "Request for employee information update";
        const string ProfileMessageBody = "The below employee submitted an employee information update request.\r\n" +
                            "\r\n" +
                            "Employee No: %EMP_NO%\r\n" +
                            "Employee: %EMP_NAME%\r\n" +
                            "Last Accepted By:\t%ACTION_USER%\r\n" +
                            "Reason:\t%ACTION_REASON%\r\n" +
                            "\r\n" +
                            "Please login to HROne Employee Self Service to approve/ reject the following request. Thank you.\r\n" +
                            "%REQUEST_URL%\r\n";

        // Start 0000080, KuangWei, 2014-09-16
        //const string LeaveCancelMessageSubject = "%EMP_NO% %EMP_NAME% leave application cancellation request";
        const string LeaveCancelMessageSubject = "[Leave Application] %EMP_NAME% %LEAVEAPP_DAYS% %LEAVEAPP_UNIT% %LEAVEAPP_CODE% on %LEAVEAPP_PERIOD%";
        // End 0000080, KuangWei, 2014-09-16
        const string LeaveCancelMessageBody = "Employee No:\t%EMP_NO%\r\n" +
                            "Employee:\t%EMP_NAME%\r\n" +
                            "Leave Date:\t%LEAVEAPP_PERIOD%\r\n" +
                            "Leave Type:\t%LEAVE_CODE%\r\n" +
                            "Taken     :\t%LEAVEAPP_DAYS% %LEAVEAPP_UNIT%\r\n" +
                            "Hours Claims:\t%LEAVEAPP_HOURSCLAIM%\r\n" +
                            "Remark    :\t%LEAVEAPP_REMARK%\r\n" +
                            "Reason for Cancellation:\t%LEAVEAPPCANCEL_REMARK%\r\n" +
                            "Last Accepted By:\t%ACTION_USER%\r\n" +
                            "Reason:\t%ACTION_REASON%\r\n" +
                            "\r\n" +
                            "The above employee submitted a leave application cancellation request.\r\n" +
                            "Please login to HROne Employee Self Service to approve/ reject the following request.\r\n" +
                            "%REQUEST_URL%\r\n";
        // Start 0000060, Miranda, 2014-07-13
        // Start 0000080, KuangWei, 2014-09-16
        //const string OTMessageSubject = "%EMP_NO% %EMP_NAME% CL Requisition request";
        const string OTMessageSubject = "[CL Requisition] %EMP_NAME% %OTCLAIM_HOURSCLAIM% hour(s) OT on %OTCLAIM_PERIOD%";
        // End 0000080, KuangWei, 2014-09-16
        const string OTMessageBody = "Employee No:\t%EMP_NO%\r\n" +
                            "Employee:\t%EMP_NAME%\r\n" +
                            "CL Requisition Date:\t%OTCLAIM_PERIOD%\r\n" +
                            "Hours Claims:\t%OTCLAIM_HOURSCLAIM%\r\n" +
                            "Remark    :\t%OTCLAIM_REMARK%\r\n" +
                            "Last Accepted By:\t%ACTION_USER%\r\n" +
                            "Reason:\t%ACTION_REASON%\r\n" +
                            "\r\n" +
                            "The above employee submitted a CL Requisition request.\r\n" +
                            "Please login to HROne Employee Self Service to approve/ reject the following request.\r\n" +
                            "%REQUEST_URL%\r\n";
        // Start 0000080, KuangWei, 2014-09-16
        //const string OTCancelMessageSubject = "%EMP_NO% %EMP_NAME% CL Requisition cancellation request";
        const string OTCancelMessageSubject = "[CL Requisition] %EMP_NAME% %OTCLAIM_HOURSCLAIM% hour(s) OT on %OTCLAIM_PERIOD%";
        // End 0000080, KuangWei, 2014-09-16
        const string OTCancelMessageBody = "Employee No:\t%EMP_NO%\r\n" +
                            "Employee:\t%EMP_NAME%\r\n" +
                            "CL Requisition Date:\t%OTCLAIM_PERIOD%\r\n" +
                            "Hours Claims:\t%OTCLAIM_HOURSCLAIM%\r\n" +
                            "Remark    :\t%OTCLAIM_REMARK%\r\n" +
                            "Reason for Cancellation:\t%OTCLAIMCANCEL_REMARK%\r\n" +
                            "Last Accepted By:\t%ACTION_USER%\r\n" +
                            "Reason:\t%ACTION_REASON%\r\n" +
                            "\r\n" +
                            "The above employee submitted a CL Requisition cancellation request.\r\n" +
                            "Please login to HROne Employee Self Service to approve/ reject the following request.\r\n" +
                            "%REQUEST_URL%\r\n";
        // End 0000060, Miranda, 2014-07-13
        // Start 0000112, Miranda, 2014-12-10
        const string lateWaiveMessageSubject = "[Late Waive] %EMP_NAME% %LATEWAIVE_MINS% min(s) Late on %LATEWAIVE_DATE%";
        const string lateWaiveMessageBody = "Employee No:\t%EMP_NO%\r\n" +
                            "Employee:\t%EMP_NAME%\r\n" +
                            "Late Date:\t%LATEWAIVE_DATE%\r\n" +
                            "Late Mins:\t%LATEWAIVE_MINS%\r\n" +
                            "Late Reason:\t%LATEWAIVE_REMARK%\r\n" +
                            "Last Accepted By:\t%ACTION_USER%\r\n" +
                            "Reason:\t%ACTION_REASON%\r\n" +
                            "\r\n" +
                            "The above employee submitted a late waive request.\r\n" +
                            "Please login to HROne Employee Self Service to approve/ reject the following request.\r\n" +
                            "%REQUEST_URL%\r\n";
        const string lateWaiveCancelMessageSubject = "[Late Waive] %EMP_NAME% %LATEWAIVE_MINS% min(s) Late on %LATEWAIVE_DATE%";
        const string lateWaiveCancelMessageBody = "Employee No:\t%EMP_NO%\r\n" +
                            "Employee:\t%EMP_NAME%\r\n" +
                            "Late Date:\t%LATEWAIVE_DATE%\r\n" +
                            "Late Mins:\t%LATEWAIVE_MINS%\r\n" +
                            "Late Reason:\t%LATEWAIVE_REMARK%\r\n" +
                            "Reason for Cancellation:\t%LATEWAIVECANCEL_REMARK%\r\n" +
                            "Last Accepted By:\t%ACTION_USER%\r\n" +
                            "Reason:\t%ACTION_REASON%\r\n" +
                            "\r\n" +
                            "The above employee submitted a late waive cancellation request.\r\n" +
                            "Please login to HROne Employee Self Service to approve/ reject the following request.\r\n" +
                            "%REQUEST_URL%\r\n";
        // End 0000112, Miranda, 2014-12-10

        string messageSubject = string.Empty;
        string messageBody = string.Empty;
        if (ApplicationType.Equals(EEmpRequest.TYPE_EELEAVEAPP))
        {
            messageSubject = LeaveMessageSubject;
            messageBody = LeaveMessageBody;
        }
        else if (ApplicationType.Equals(EEmpRequest.TYPE_EEPROFILE))
        {
            messageSubject = ProfileMessageSubject;
            messageBody = ProfileMessageBody;
        }
        else if (ApplicationType.Equals(EEmpRequest.TYPE_EELEAVECANCEL))
        {
            messageSubject = LeaveCancelMessageSubject;
            messageBody = LeaveCancelMessageBody;
        }
        // Start 0000060, Miranda, 2014-07-13
        else if (ApplicationType.Equals(EEmpRequest.TYPE_EEOTCLAIM))
        {
            messageSubject = OTMessageSubject;
            messageBody = OTMessageBody;
        }
        else if (ApplicationType.Equals(EEmpRequest.TYPE_EEOTCLAIMCANCEL))
        {
            messageSubject = OTCancelMessageSubject;
            messageBody = OTCancelMessageBody;
        }
        // End 0000060, Miranda, 2014-07-13
        // Start 0000112, Miranda, 2014-12-10
        else if (ApplicationType.Equals(EEmpRequest.TYPE_EELATEWAIVE))
        {
            messageSubject = lateWaiveMessageSubject;
            messageBody = lateWaiveMessageBody;
        }
        else if (ApplicationType.Equals(EEmpRequest.TYPE_EELATEWAIVECANCEL))
        {
            messageSubject = lateWaiveCancelMessageSubject;
            messageBody = lateWaiveCancelMessageBody;
        }
        // End 0000112, Miranda, 2014-12-10

        SendEmail(mailAddressList, ApplicantEmpInfo, messageSubject, messageBody, parameterTable);
    }

     public virtual void SubmitLeaveApplicatoinCancel(ERequestLeaveApplicationCancel requestLeaveAppCancel)
    {
        ELeaveApplication leaveApp = new ELeaveApplication();
        leaveApp.LeaveAppID = requestLeaveAppCancel.LeaveAppID;

        if (ELeaveApplication.db.select(dbConn, leaveApp))
        {
            EEmpPersonalInfo ApplicantEmpInfo = GetEmpInfo(leaveApp.EmpID);

            Hashtable mailContentParameterTable = new Hashtable();
            mailContentParameterTable = GenerateEmpInfoHashTable(ApplicantEmpInfo, mailContentParameterTable);


            ERequestLeaveApplication empLeaveRequest = ERequestLeaveApplication.CreateDummyRequestLeaveAppliction(leaveApp);

            mailContentParameterTable = GenerateRequestLeaveApplicationHashTable(empLeaveRequest, mailContentParameterTable);
            mailContentParameterTable.Add("%LEAVEAPPCANCEL_REMARK%", requestLeaveAppCancel.RequestLeaveAppCancelReason);


            ArrayList workflowDetailList = GetAuthorizationWorkFlowDetailList(empLeaveRequest.EmpID, EEmpRequest.TYPE_EELEAVECANCEL);

            if (workflowDetailList.Count <= 0)
            {
                throw new Exception("Request Cancel due to No Authorizer for this employee");
            }

            List<AuthorizerMailAddress> authorizerEmailAddressList = GetAllAuthorizerEmailAddressList(workflowDetailList, 0, 0, false);

            DateTime createDate = AppUtils.ServerDateTime();
            requestLeaveAppCancel.RequestLeaveAppCancelCreateDateTime = createDate;
            ERequestLeaveApplicationCancel.db.insert(dbConn, requestLeaveAppCancel);

            EEmpRequest empRequest = new EEmpRequest();
            empRequest.EmpID = requestLeaveAppCancel.EmpID;
            empRequest.EmpRequestType = EEmpRequest.TYPE_EELEAVECANCEL;
            empRequest.EmpRequestRecordID = requestLeaveAppCancel.RequestLeaveAppCancelID;
            empRequest.EmpRequestCreateDate = createDate;
            empRequest.EmpRequestModifyDate = createDate;
            empRequest.EmpRequestStatus = EEmpRequest.STATUS_SUBMITTED;

            EEmpRequest.db.insert(dbConn, empRequest);

            mailContentParameterTable = GenerateRequestRedirectLinkHashTable(empRequest, mailContentParameterTable);


            SendRequestEmail(empRequest.EmpRequestType, authorizerEmailAddressList, ApplicantEmpInfo, mailContentParameterTable);
        }
    }

    protected virtual void SendEmailToApplicant(EEmpPersonalInfo ApplicantEmpInfo, string MessageSubjectTemplate, string MessageBodyTemplate, Hashtable parameterTable)
    {
        foreach (string key in parameterTable.Keys)
        {
            MessageSubjectTemplate = MessageSubjectTemplate.Replace(key, parameterTable[key].ToString());
            MessageBodyTemplate = MessageBodyTemplate.Replace(key, parameterTable[key].ToString());
        }
        string[] messageBodyLineArray = MessageBodyTemplate.Split(new string[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);
        string newMessageBody = string.Empty;
        foreach (string messageLine in messageBodyLineArray)
            if (string.IsNullOrEmpty(messageLine) || messageLine.IndexOf("%") == messageLine.LastIndexOf("%"))
                if (string.IsNullOrEmpty(newMessageBody))
                    newMessageBody = messageLine;
                else
                    newMessageBody += "\r\n" + messageLine;
        MessageBodyTemplate = newMessageBody;

        string applicantEmailAddress = ApplicantEmpInfo.EmpInternalEmail.Trim().Equals(string.Empty) ? ApplicantEmpInfo.EmpEmail : ApplicantEmpInfo.EmpInternalEmail;
        if (!applicantEmailAddress.Trim().Equals(string.Empty))
            AppUtils.Sent_Email(dbConn, applicantEmailAddress, applicantEmailAddress, ApplicantEmpInfo.EmpEngSurname + " " + ApplicantEmpInfo.EmpEngOtherName, ApplicantEmpInfo.EmpEngSurname + " " + ApplicantEmpInfo.EmpEngOtherName, MessageSubjectTemplate, MessageBodyTemplate);

    }
    protected virtual void SendEmail(System.Collections.Generic.List<AuthorizerMailAddress> mailAddressList, EEmpPersonalInfo ApplicantEmpInfo, string MessageSubjectTemplate, string MessageBodyTemplate, Hashtable parameterTable)
    {
        foreach (string key in parameterTable.Keys)
        {
            MessageSubjectTemplate = MessageSubjectTemplate.Replace(key, parameterTable[key].ToString());
            MessageBodyTemplate = MessageBodyTemplate.Replace(key, parameterTable[key].ToString());
        }
        string[] messageBodyLineArray = MessageBodyTemplate.Split(new string[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);
        string newMessageBody = string.Empty;
        foreach (string messageLine in messageBodyLineArray)
            if (string.IsNullOrEmpty(messageLine) || messageLine.IndexOf("%") == messageLine.LastIndexOf("%"))
                if (string.IsNullOrEmpty(newMessageBody))
                    newMessageBody = messageLine;
                else
                    newMessageBody += "\r\n" + messageLine;
        MessageBodyTemplate = newMessageBody;

        System.Collections.Generic.List<string> sentEmailList = new System.Collections.Generic.List<string>();
        foreach (AuthorizerMailAddress mailAddress in mailAddressList)
        {
            if (!sentEmailList.Contains(mailAddress.Address.Trim()))
            {
                string applicantEmailAddress = ApplicantEmpInfo.EmpInternalEmail.Trim().Equals(string.Empty) ? ApplicantEmpInfo.EmpEmail : ApplicantEmpInfo.EmpInternalEmail;
                AppUtils.Sent_Email(dbConn, mailAddress.Address, applicantEmailAddress, mailAddress.DisplayName, ApplicantEmpInfo.EmpEngSurname + " " + ApplicantEmpInfo.EmpEngOtherName, MessageSubjectTemplate, MessageBodyTemplate);
                sentEmailList.Add(mailAddress.Address.Trim());
            }
        }
    }
}
