using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

using HROne.DataAccess;
//using perspectivemind.validation;
using HROne.Lib.Entities;

public partial class ESS_EmpRequestDetail : HROneWebPage
{
    public Binding binding, RequestBinding;
    public DBManager db = EEmpPersonalInfo.db;
    public EEmpPersonalInfo obj;
    protected bool IsAuthorizer = false;

    public int CurID, CurRequestID = -1;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!WebUtils.CheckAccess(Response, Session))
            return;
        binding = new Binding(dbConn, db);
        binding.init(Request, Session);

        EESSUser user = WebUtils.GetCurUser(Session);
        if (user != null)
        {
            CurID = user.EmpID;
        }

        if (!int.TryParse(DecryptedRequest["EmpRequestID"], out CurRequestID))
            CurRequestID = -1;

        HROne.Common.WebUtility.WebControlsLocalization(this, this.Controls);

        if (!Page.IsPostBack)
        {

            if (CurID > 0)
            {
                loadObject();
            }
            else
            {

            }
        }
    }

    protected bool loadObject()
    {
        bool allowApproval = false;

        DBManager Requestdb = EAuthorizer.db;
        DBFilter filterStatus = new DBFilter();
        filterStatus.add(new Match("EmpID", CurID));


        int targetEmpID = 0;

        EEmpRequest Request = new EEmpRequest();
        Request.EmpRequestID = CurRequestID;
        if (EEmpRequest.db.select(dbConn, Request))
        {
            targetEmpID = Request.EmpID;
            if (Request.EmpRequestType == EEmpRequest.TYPE_EELEAVEAPP)
            {
                LeaveApplicationRecord1.Visible = true;
                uc1Emp_info.Visible = true;
                uc1Emp_info.CurID = Request.EmpID;
                uc3Emp_Request_Empinfo.Visible = false;
                LeaveApplicationCancelRecord1.Visible = false;
				// Start 0000060, Miranda, 2014-07-13
                OTClaimRecord1.Visible = false;
                OTClaimCancelRecord1.Visible = false;
				// End 0000060, Miranda, 2014-07-13
                // Start 0000112, Miranda, 2014-12-10
                LateWaiveRecord1.Visible = false;
                LateWaiveCancelRecord1.Visible = false;
                // End 0000112, Miranda, 2014-12-10
            }
            else if (Request.EmpRequestType == EEmpRequest.TYPE_EEPROFILE)
            {
                LeaveApplicationRecord1.Visible = false;
                uc1Emp_info.Visible = false;
                uc3Emp_Request_Empinfo.Visible = true;
                uc3Emp_Request_Empinfo.CurrentRequestStatusLabel = HROne.Common.WebUtility.GetLocalizedString(Request.EmpRequestStatus);
                LeaveApplicationCancelRecord1.Visible = false;
				// Start 0000060, Miranda, 2014-07-13
                OTClaimRecord1.Visible = false;
                OTClaimCancelRecord1.Visible = false;
				// End 0000060, Miranda, 2014-07-13
                // Start 0000112, Miranda, 2014-12-10
                LateWaiveRecord1.Visible = false;
                LateWaiveCancelRecord1.Visible = false;
                // End 0000112, Miranda, 2014-12-10
            }
            else if (Request.EmpRequestType == EEmpRequest.TYPE_EELEAVECANCEL)
            {
                LeaveApplicationRecord1.Visible = false;
                uc1Emp_info.Visible = true;
                uc1Emp_info.CurID = Request.EmpID;
                uc3Emp_Request_Empinfo.Visible = false;
                LeaveApplicationCancelRecord1.Visible = true;
				// Start 0000060, Miranda, 2014-07-13
                OTClaimRecord1.Visible = false;
                OTClaimCancelRecord1.Visible = false;
				// End 0000060, Miranda, 2014-07-13
                // Start 0000112, Miranda, 2014-12-10
                LateWaiveRecord1.Visible = false;
                LateWaiveCancelRecord1.Visible = false;
                // End 0000112, Miranda, 2014-12-10
            }
			// Start 0000060, Miranda, 2014-07-13
            else if (Request.EmpRequestType == EEmpRequest.TYPE_EEOTCLAIM)
            {
                LeaveApplicationRecord1.Visible = false;
                uc1Emp_info.Visible = true;
                uc1Emp_info.CurID = Request.EmpID;
                uc3Emp_Request_Empinfo.Visible = false;
                LeaveApplicationCancelRecord1.Visible = false;
                OTClaimRecord1.Visible = true;
                OTClaimCancelRecord1.Visible = false;
                // Start 0000112, Miranda, 2014-12-10
                LateWaiveRecord1.Visible = false;
                LateWaiveCancelRecord1.Visible = false;
                // End 0000112, Miranda, 2014-12-10
            }
            else if (Request.EmpRequestType == EEmpRequest.TYPE_EEOTCLAIMCANCEL)
            {
                LeaveApplicationRecord1.Visible = false;
                uc1Emp_info.Visible = true;
                uc1Emp_info.CurID = Request.EmpID;
                uc3Emp_Request_Empinfo.Visible = false;
                LeaveApplicationCancelRecord1.Visible = false;
                OTClaimRecord1.Visible = false;
                OTClaimCancelRecord1.Visible = true;
                // Start 0000112, Miranda, 2014-12-10
                LateWaiveRecord1.Visible = false;
                LateWaiveCancelRecord1.Visible = false;
                // End 0000112, Miranda, 2014-12-10
            }
			// End 0000060, Miranda, 2014-07-13
            // Start 0000112, Miranda, 2014-12-10
            else if (Request.EmpRequestType == EEmpRequest.TYPE_EELATEWAIVE)
            {
                LeaveApplicationRecord1.Visible = false;
                uc1Emp_info.Visible = true;
                uc1Emp_info.CurID = Request.EmpID;
                uc3Emp_Request_Empinfo.Visible = false;
                LeaveApplicationCancelRecord1.Visible = false;
                OTClaimRecord1.Visible = false;
                OTClaimCancelRecord1.Visible = false;
                LateWaiveRecord1.Visible = true;
                LateWaiveCancelRecord1.Visible = false;
            }
            else if (Request.EmpRequestType == EEmpRequest.TYPE_EELATEWAIVECANCEL)
            {
                LeaveApplicationRecord1.Visible = false;
                uc1Emp_info.Visible = true;
                uc1Emp_info.CurID = Request.EmpID;
                uc3Emp_Request_Empinfo.Visible = false;
                LeaveApplicationCancelRecord1.Visible = false;
                OTClaimRecord1.Visible = false;
                OTClaimCancelRecord1.Visible = false;
                LateWaiveRecord1.Visible = false;
                LateWaiveCancelRecord1.Visible = true;
            }
            // End 0000112, Miranda, 2014-12-10
        }

        if (Requestdb.count(dbConn, filterStatus) > 0)
        {
            //obj = new EEmpPersonalInfo();
            //obj.EmpID = CurID;
            //if (!db.select(obj))
            //    return false;

            //Hashtable values = new Hashtable();
            //db.populate(obj, values);
            //binding.toControl(values);



            //------------------------------------------------------


            //for (int i = 1; i <= 2; i++)
            //{
            //    ArrayList EmpAuthorizerList = ESSAuthorizerEmail.Get_AuthorizerList(dbConn, targetEmpID, i);
            //    if (!(EmpAuthorizerList == null))
            //    {

            //        foreach (EAuthorizer EmpAuthorizer in EmpAuthorizerList)
            //        {
            //                if (EmpAuthorizer.EmpID  == CurID)
            //                {

            //                    EAuthorizationGroup authGroup = new EAuthorizationGroup();
            //                    authGroup.AuthorizationGroupID = EmpAuthorizer.AuthorizationGroupID;
            //                    if (EAuthorizationGroup.db.select(dbConn, authGroup))
            //                    {
            //                        IsAuthorizer = true;
            //                        if (authGroup.AuthorizationGroupIsApproveEEInfo)
            //                            allowEEInfoApproval = true;
            //                        if (authGroup.AuthorizationGroupIsApproveLeave)
            //                            allowLeaveInfoApproval = true;

            //                    }
            //                    break;
            //                }
            //        }
            //    }
            //}
            //------------------------------------------------------

            ESSAuthorizationProcess authorization = new ESSAuthorizationProcess(dbConn);
            ArrayList workFlowDetailList = authorization.GetAuthorizationWorkFlowDetailList(Request.EmpID, Request.EmpRequestType);
            EAuthorizationWorkFlowDetail currentWorkFlowDetail = authorization.GetCurrentWorkFlowDetailObject(
                workFlowDetailList,
                Request.EmpRequestLastAuthorizationWorkFlowIndex, CurID);
            if (currentWorkFlowDetail != null 
                )
                allowApproval = true;
            else
                allowApproval = false;


            if (allowApproval || Request.EmpID==CurID)
            {
                LeaveApplicationRecord1.Visible = LeaveApplicationRecord1.Visible & Request.EmpRequestType.Equals(EEmpRequest.TYPE_EELEAVEAPP);
                uc3Emp_Request_Empinfo.Visible = uc3Emp_Request_Empinfo.Visible & Request.EmpRequestType.Equals(EEmpRequest.TYPE_EEPROFILE);
                LeaveApplicationCancelRecord1.Visible = LeaveApplicationCancelRecord1.Visible & Request.EmpRequestType.Equals(EEmpRequest.TYPE_EELEAVECANCEL);
                // Start 0000060, Miranda, 2014-07-13
                OTClaimRecord1.Visible = OTClaimRecord1.Visible & Request.EmpRequestType.Equals(EEmpRequest.TYPE_EEOTCLAIM);
                OTClaimCancelRecord1.Visible = OTClaimCancelRecord1.Visible & Request.EmpRequestType.Equals(EEmpRequest.TYPE_EEOTCLAIMCANCEL);
                // End 0000060, Miranda, 2014-07-13
                // Start 0000112, Miranda, 2014-12-10
                LateWaiveRecord1.Visible = LateWaiveRecord1.Visible & Request.EmpRequestType.Equals(EEmpRequest.TYPE_EELATEWAIVE);
                LateWaiveCancelRecord1.Visible = LateWaiveCancelRecord1.Visible & Request.EmpRequestType.Equals(EEmpRequest.TYPE_EELATEWAIVECANCEL);
                // End 0000112, Miranda, 2014-12-10
            }
            else
            {
                LeaveApplicationRecord1.Visible = false;
                uc3Emp_Request_Empinfo.Visible = false;
                LeaveApplicationCancelRecord1.Visible = false;
                // Start 0000060, Miranda, 2014-07-13
                OTClaimRecord1.Visible = false;
                OTClaimCancelRecord1.Visible = false;
                // End 0000060, Miranda, 2014-07-13
                // Start 0000112, Miranda, 2014-12-10
                LateWaiveRecord1.Visible = false;
                LateWaiveCancelRecord1.Visible = false;
                // End 0000112, Miranda, 2014-12-10
            }
            LeaveApplicationRecord1.ShowAuthorizeOption = false;
            uc3Emp_Request_Empinfo.ShowAuthorizeOption = false;
            LeaveApplicationCancelRecord1.ShowAuthorizeOption = false;
            // Start 0000060, Miranda, 2014-07-13
            OTClaimRecord1.ShowAuthorizeOption = false;
            OTClaimCancelRecord1.ShowAuthorizeOption = false;
            // End 0000060, Miranda, 2014-07-13
            // Start 0000112, Miranda, 2014-12-10
            LateWaiveRecord1.ShowAuthorizeOption = false;
            LateWaiveCancelRecord1.ShowAuthorizeOption = false;
            // End 0000112, Miranda, 2014-12-10
            if (allowApproval
                && Request.EmpRequestStatus != EEmpRequest.STATUS_APPROVED
                && Request.EmpRequestStatus != EEmpRequest.STATUS_REJECTED
                && Request.EmpRequestStatus != EEmpRequest.STATUS_CANCELLED
                )
            {
                ArrayList authorizerList = currentWorkFlowDetail.GetActualAutorizerObjectList(dbConn, CurID);
                foreach (EAuthorizer authorizer in authorizerList)
                    if (!authorizer.AuthorizerIsReadOnly)
                    {
                        // **** Start 2014-04-09, 0000027, Ricky So
                        //LeaveApplicationRecord1.ShowAuthorizeOption = true;
                        //uc3Emp_Request_Empinfo.ShowAuthorizeOption = true;
                        //LeaveApplicationCancelRecord1.ShowAuthorizeOption = true;
                        //
                        LeaveApplicationRecord1.ShowAuthorizeOption = (targetEmpID != CurID);
                        uc3Emp_Request_Empinfo.ShowAuthorizeOption = (targetEmpID != CurID);
                        LeaveApplicationCancelRecord1.ShowAuthorizeOption = (targetEmpID != CurID);
                        // **** End 2014-04-09, 00000027, Ricky So
                        // Start 0000060, Miranda, 2014-07-13
                        OTClaimRecord1.ShowAuthorizeOption = (targetEmpID != CurID);
                        OTClaimCancelRecord1.ShowAuthorizeOption = (targetEmpID != CurID);
                        // End 0000060, Miranda, 2014-07-13
                        // Start 0000112, Miranda, 2014-12-10
                        LateWaiveRecord1.ShowAuthorizeOption = (targetEmpID != CurID);
                        LateWaiveCancelRecord1.ShowAuthorizeOption = (targetEmpID != CurID);
                        // End 0000112, Miranda, 2014-12-10
                    }
            }

            return true;
        }
        else if (targetEmpID == CurID)
        {
            LeaveApplicationRecord1.ShowAuthorizeOption = false;
            uc3Emp_Request_Empinfo.ShowAuthorizeOption = false;
            LeaveApplicationCancelRecord1.ShowAuthorizeOption = false;
            // Start 0000060, Miranda, 2014-07-13
            OTClaimRecord1.ShowAuthorizeOption = false;
            OTClaimCancelRecord1.ShowAuthorizeOption = false;
            // End 0000060, Miranda, 2014-07-13
            // Start 0000112, Miranda, 2014-12-10
            LateWaiveRecord1.ShowAuthorizeOption = false;
            LateWaiveCancelRecord1.ShowAuthorizeOption = false;
            // End 0000112, Miranda, 2014-12-10
            return true;
        }
        else
        {
            LeaveApplicationRecord1.Visible = false;
            uc3Emp_Request_Empinfo.Visible = false;
            LeaveApplicationCancelRecord1.Visible = false;
            // Start 0000060, Miranda, 2014-07-13
            OTClaimRecord1.Visible = false;
            OTClaimCancelRecord1.Visible = false;
            // End 0000060, Miranda, 2014-07-13
            // Start 0000112, Miranda, 2014-12-10
            LateWaiveRecord1.Visible = false;
            LateWaiveCancelRecord1.Visible = false;
            // End 0000112, Miranda, 2014-12-10
            return false;
        }
    }
}
