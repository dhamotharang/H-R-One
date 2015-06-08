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
using System.Net.Mail;

public partial class LeaveApplicationCancelRecord : HROneWebControl
{
    public Binding leaveAppBinding;
    public Binding cancelBinding;
    public SearchBinding sbinding;
    public DBManager db = ERequestLeaveApplicationCancel.db;

    public ERequestLeaveApplicationCancel obj;
    public int UserID = -1;
    public int CurEmpID = -1;
    public int CurRequestID = -1;

    public bool ShowAuthorizeOption = true;

    protected const string CONFIRM_MESSAGE = "Are you sure?";
    //protected const string AL_AUTHORIZED_BEFORE_PROBATION_MESSAGE = "Staff is under probation and not entitled to annual leave. Continue to proceed the authorization?";

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!WebUtils.CheckAccess(Response, Session))
            return;
        leaveAppBinding = new Binding(dbConn, ELeaveApplication.db);
        leaveAppBinding.add(EmpID);
        //binding.add(new LabelVLBinder(db, EmpName,"EmpID", EEmpPersonalInfo.VLEmp));
        leaveAppBinding.add(new LabelVLBinder(db, LeaveCodeID, ELeaveCode.VLLeaveCode));
        leaveAppBinding.add(new LabelVLBinder(db, LeaveAppUnit, Values.VLLeaveUnit));
        leaveAppBinding.add(LeaveAppDateFrom);
        leaveAppBinding.add(LeaveAppDateTo);
        leaveAppBinding.add(LeaveAppTimeFrom);
        leaveAppBinding.add(LeaveAppTimeTo);
        leaveAppBinding.add(LeaveAppDays);
        leaveAppBinding.add(LeaveAppHours);
        leaveAppBinding.add(LeaveAppRemark);
        leaveAppBinding.add(new LabelVLBinder(db, LeaveAppHasMedicalCertificate, Values.VLTrueFalseYesNo));
        leaveAppBinding.init(Request, Session);

        cancelBinding = new Binding(dbConn, ERequestLeaveApplicationCancel.db);
        cancelBinding.add(RequestLeaveAppCancelReason);
        cancelBinding.init(Request, Session);

        EESSUser user = WebUtils.GetCurUser(Session);
        if (user != null)
        {
            UserID = user.EmpID;
        }
        if (!Int32.TryParse(DecryptedRequest["TargetEmpID"], out CurEmpID))
            CurEmpID = -1;
        if (!Int32.TryParse(DecryptedRequest["EmpRequestRecordID"], out CurRequestID))
            CurRequestID = -1;
        EmpID.Value = CurEmpID.ToString();


        if (!Page.IsPostBack)
        {
            if (UserID > 0)
            {
                loadObject();

            }
        }
    }
    protected void Page_PreRender(object sender, EventArgs e)
    {
        AuthorizerOptionSectionRow.Visible = ShowAuthorizeOption;
    }
    protected bool loadObject()
    {
        DBFilter filterStatus = new DBFilter();
        filterStatus.add(new Match("EmpRequestRecordID", CurRequestID));
        filterStatus.add(new Match("EmpRequestType", EEmpRequest.TYPE_EELEAVECANCEL));
        ArrayList empRequestList = EEmpRequest.db.select(dbConn, filterStatus);
        if (empRequestList.Count>0)
        {
            EEmpRequest empRequest = (EEmpRequest)empRequestList[0];
            if (empRequest.EmpID == UserID)
            {
                string submitStatus = empRequest.EmpRequestStatus;
                if ((submitStatus.Equals(EEmpRequest.STATUS_SUBMITTED) || submitStatus.Equals(EEmpRequest.STATUS_ACCEPTED)) && !submitStatus.Equals(EEmpRequest.STATUS_APPROVED))
                {
                    btnCancel.Visible = true;
                    RejectReasonRow.Visible = false;
                }
                else
                {
                    btnCancel.Visible = false;
                    if (!string.IsNullOrEmpty(empRequest.EmpRequestRejectReason))
                    {
                        RejectReasonRow.Visible = true;
                        lblRejectReason.Text = empRequest.EmpRequestRejectReason;
                    }
                    else
                        RejectReasonRow.Visible = false;
                }
            }
            else
            {
                btnCancel.Visible = false;
                RejectReasonRow.Visible = false;
            }
            obj = new ERequestLeaveApplicationCancel();
            obj.RequestLeaveAppCancelID = CurRequestID;
            if (!db.select(dbConn, obj))
                return false;

            Hashtable leaveCancelValues = new Hashtable();
            db.populate(obj, leaveCancelValues);
            cancelBinding.toControl(leaveCancelValues);

            ELeaveApplication leaveApp = new ELeaveApplication();
            leaveApp.LeaveAppID = obj.LeaveAppID;
            if (!ELeaveApplication.db.select(dbConn, leaveApp))
                return false;

            Hashtable leaveAppValues = new Hashtable();
            ELeaveApplication.db.populate(leaveApp, leaveAppValues);
            leaveAppBinding.toControl(leaveAppValues);
            //EEmpPersonalInfo EmpInfo = new EEmpPersonalInfo();
            //EmpInfo.EmpID = obj.EmpID;
            //if (EEmpPersonalInfo.db.select(dbConn, EmpInfo))
            //    EmpName.Text = EmpInfo.EmpNo + " - " + EmpInfo.EmpEngFullNameWithAlias;

            if (leaveApp.LeaveAppUnit.Equals("D"))
            {
                TimeRow.Visible = false;
                LeaveAppDateToPlaceHolder.Visible = true;
            }
            else
            {
                TimeRow.Visible = true;
                LeaveAppDateToPlaceHolder.Visible = false;
            }

            //string leaveAuthorizedMessage = CONFIRM_MESSAGE;
            ELeaveCode leaveCode = new ELeaveCode();
            leaveCode.LeaveCodeID = leaveApp.LeaveCodeID;
            if (ELeaveCode.db.select(dbConn, leaveCode))
            {
                //LeaveCodeIsShowMedicalCertOptionPanel.Visible = leaveCode.LeaveCodeIsShowMedicalCertOption;

                if (leaveCode.LeaveTypeID.Equals(ELeaveType.COMPENSATION_LEAVE_TYPE(dbConn).LeaveTypeID))
                    HoursClaimPanel.Visible = true;
                else
                    HoursClaimPanel.Visible = false;

                //if (leaveCode.LeaveTypeID.Equals(ELeaveType.ANNUAL_LEAVE_TYPE(dbConn).LeaveTypeID))
                //{
                //    EEmpPersonalInfo empInfo = new EEmpPersonalInfo();
                //    empInfo.EmpID = obj.EmpID;
                //    if (EEmpPersonalInfo.db.select(dbConn, empInfo))
                //    {
                //        if (AppUtils.ServerDateTime().Date <= empInfo.EmpProbaLastDate || obj.LeaveAppDateFrom <= empInfo.EmpProbaLastDate)
                //            leaveAuthorizedMessage = AL_AUTHORIZED_BEFORE_PROBATION_MESSAGE;
                //    }
                //}
            }
            else
                HoursClaimPanel.Visible = false;

            //Authorize.Attributes.Add("onclick", "return confirm(\"" + HROne.Common.WebUtility.GetLocalizedString(leaveAuthorizedMessage) + "\");");
            Reject.Attributes.Add("onclick", "return confirm(\"" + HROne.Common.WebUtility.GetLocalizedString(CONFIRM_MESSAGE) + "\");");


            return true;
        }
        else
            return false;
    }

    protected void LeaveAuthorize_Click(object sender, EventArgs e)
    {


        DBManager db = EEmpRequest.db;
        DBFilter RequestFilter = new DBFilter();

        PageErrors errors = PageErrors.getErrors(db, Page.Master);
        errors.clear();

        RequestFilter.add(new Match("EmpRequestRecordID", CurRequestID));
        RequestFilter.add(new Match("EmpRequestType", EEmpRequest.TYPE_EELEAVECANCEL));
        ArrayList RequestList = EEmpRequest.db.select(dbConn, RequestFilter);
        if (RequestList.Count > 0)
        {
            EEmpRequest empRequest = (EEmpRequest)RequestList[0];
            try
            {
                ESSAuthorizationProcess authorization = new ESSAuthorizationProcess(dbConn);
                empRequest.EmpRequestRejectReason = RejectReason.Text; 
                authorization.AuthorizeAction(empRequest, UserID);
                HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "~/ESS_EmpSupervisorApproval.aspx");
            }
            catch (Exception ex)
            {
                errors.addError(ex.Message);
            }

        }
        else
            errors.addError("Request Cancel due to no permission!");
    }

    protected void LeaveReject_Click(object sender, EventArgs e)
    {

        DBFilter RequestFilter = new DBFilter();
        PageErrors errors = PageErrors.getErrors(db, Page.Master);;
        errors.clear();

        RequestFilter.add(new Match("EmpRequestRecordID", CurRequestID));
        RequestFilter.add(new Match("EmpRequestType", EEmpRequest.TYPE_EELEAVECANCEL));
        ArrayList RequestList = EEmpRequest.db.select(dbConn, RequestFilter);
        if (RequestList.Count > 0)
        {

            EEmpRequest empRequest = (EEmpRequest)RequestList[0];
            empRequest.EmpRequestRejectReason = RejectReason.Text;
            try
            {
                ESSAuthorizationProcess authorization = new ESSAuthorizationProcess(dbConn);
                authorization.RejectedAction(empRequest, UserID);
                HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "~/ESS_EmpSupervisorApproval.aspx");
            }
            catch (Exception ex)
            {
                errors.addError(ex.Message);
            }

        }
        else
            errors.addError("Request Cancel due to no permission!");

    }


    protected void btnCancel_Click(object sender, EventArgs e)
    {
        DBFilter filterStatus = new DBFilter();

        //------------------------------------------------------
        //Select Filter record from the EmpRequest table by EmpRequestRecordID and Request Status
        filterStatus.add(new Match("EmpRequestRecordID", CurRequestID));
        filterStatus.add(new Match("EmpRequestType", EEmpRequest.TYPE_EELEAVECANCEL));
        OR orStatus = new OR();

        orStatus.add(new Match("EmpRequestStatus", EEmpRequest.STATUS_SUBMITTED));
        orStatus.add(new Match("EmpRequestStatus", EEmpRequest.STATUS_ACCEPTED));

        filterStatus.add(orStatus);
        filterStatus.add(new Match("EmpRequestStatus", "<>", EEmpRequest.STATUS_APPROVED));
        //------------------------------------------------------
        ArrayList empRequestList = EEmpRequest.db.select(dbConn, filterStatus);
        if (empRequestList.Count > 0)
        {
            EEmpRequest EmpRequest = (EEmpRequest)empRequestList[0];
            ESSAuthorizationProcess authorization = new ESSAuthorizationProcess(dbConn);
            authorization.CancelAction(EmpRequest);
            HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "~/ESS_EmpRequestStatus.aspx");
        }

    }
}
