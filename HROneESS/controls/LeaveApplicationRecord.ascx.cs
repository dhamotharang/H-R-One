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

public partial class LeaveApplicationRecord : HROneWebControl
{
    public Binding binding;
    public SearchBinding sbinding;
    public DBManager db = ERequestLeaveApplication.db;

    public ERequestLeaveApplication obj;
    public int UserID = -1;
    public int CurEmpID = -1;
    public int CurRequestID = -1;

    public bool ShowAuthorizeOption = true;

    protected const string CONFIRM_MESSAGE = "Are you sure?";
    protected const string AL_AUTHORIZED_BEFORE_PROBATION_MESSAGE = "Staff is under probation and not entitled to annual leave. Continue to proceed the authorization?";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!WebUtils.CheckAccess(Response, Session))
            return;
        binding = new Binding(dbConn, db);
        binding.add(EmpID);
        //binding.add(new LabelVLBinder(db, EmpName,"EmpID", EEmpPersonalInfo.VLEmp));
        binding.add(new LabelVLBinder(db, RequestLeaveCodeID, ELeaveCode.VLLeaveCode));
        binding.add(new LabelVLBinder(db, RequestLeaveAppUnit, Values.VLLeaveUnit));
        binding.add(RequestLeaveAppDateFrom);
        binding.add(RequestLeaveAppDateFromAM);
        binding.add(RequestLeaveAppDateTo);
        binding.add(RequestLeaveAppDateToAM);
        binding.add(RequestLeaveAppTimeFrom);
        binding.add(RequestLeaveAppTimeTo);
        binding.add(RequestLeaveDays);
        binding.add(RequestLeaveAppHours);
        binding.add(RequestLeaveAppRemark);
        binding.add(new LabelVLBinder(db, RequestLeaveAppHasMedicalCertificate, Values.VLTrueFalseYesNo));
        binding.init(Request, Session);


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
        filterStatus.add(new Match("EmpRequestType", EEmpRequest.TYPE_EELEAVEAPP ));
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
                    // Start 0000063, KuangWei, 2014-08-25
                    if (!string.IsNullOrEmpty(empRequest.EmpRequestRejectReason))
                    {                        
                        RejectReasonRow.Visible = true;
                        lblReject.Visible = false;
                        lblAuthorize.Visible = true;
                        lblRejectReason.Text = empRequest.EmpRequestRejectReason;
                    }
                    else
                    {
                        RejectReasonRow.Visible = false;
                    }
                    // End 0000063, KuangWei, 2014-08-25
                }
                else
                {
                    btnCancel.Visible = false;
                    if (!string.IsNullOrEmpty(empRequest.EmpRequestRejectReason))
                    {
                        RejectReasonRow.Visible = true;
                        // Start 0000063, KuangWei, 2014-08-25
                        lblReject.Visible = true;
                        lblAuthorize.Visible = false;
                        // End 0000063, KuangWei, 2014-08-25
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
            obj = new ERequestLeaveApplication();
            obj.RequestLeaveAppID = CurRequestID;
            if (!db.select(dbConn, obj))
                return false;
            Emp_LeaveBalance_List1.EmpID = obj.EmpID;
            Emp_LeaveBalance_List1.AsOfDate = obj.RequestLeaveAppDateFrom;

            Hashtable values = new Hashtable();
            db.populate(obj, values);
            binding.toControl(values);
            //EEmpPersonalInfo EmpInfo = new EEmpPersonalInfo();
            //EmpInfo.EmpID = obj.EmpID;
            //if (EEmpPersonalInfo.db.select(dbConn, EmpInfo))
            //    EmpName.Text = EmpInfo.EmpNo + " - " + EmpInfo.EmpEngFullNameWithAlias;

            if (obj.RequestLeaveAppUnit.Equals("D"))
            {
                TimeRow.Visible = false;
                LeaveAppDateToPlaceHolder.Visible = true;
            }
            else
            {
                TimeRow.Visible = true;
                LeaveAppDateToPlaceHolder.Visible = false;
            }

            string leaveAuthorizedMessage = CONFIRM_MESSAGE;
            ELeaveCode leaveCode = new ELeaveCode();
            leaveCode.LeaveCodeID = obj.RequestLeaveCodeID;
            if (ELeaveCode.db.select(dbConn, leaveCode))
            {
                LeaveCodeIsShowMedicalCertOptionPanel.Visible = leaveCode.LeaveCodeIsShowMedicalCertOption;

                if (leaveCode.LeaveTypeID.Equals(ELeaveType.COMPENSATION_LEAVE_TYPE(dbConn).LeaveTypeID))
                    HoursClaimPanel.Visible = true;
                else
                    HoursClaimPanel.Visible = false;

                if (leaveCode.LeaveTypeID.Equals(ELeaveType.ANNUAL_LEAVE_TYPE(dbConn).LeaveTypeID))
                {
                    EEmpPersonalInfo empInfo = new EEmpPersonalInfo();
                    empInfo.EmpID = obj.EmpID;
                    if (EEmpPersonalInfo.db.select(dbConn, empInfo))
                    {
                        if (AppUtils.ServerDateTime().Date <= empInfo.EmpProbaLastDate || obj.RequestLeaveAppDateFrom <= empInfo.EmpProbaLastDate)
                            leaveAuthorizedMessage = AL_AUTHORIZED_BEFORE_PROBATION_MESSAGE;
                    }
                }
            }
            else
                HoursClaimPanel.Visible = false;

            Authorize.Attributes.Add("onclick", "return confirm(\"" + HROne.Common.WebUtility.GetLocalizedString(leaveAuthorizedMessage) + "\");");
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
        RequestFilter.add(new Match("EmpRequestType", EEmpRequest.TYPE_EELEAVEAPP));
        ArrayList RequestList = EEmpRequest.db.select(dbConn, RequestFilter);
        if (RequestList.Count > 0)
        {
            EEmpRequest empRequest = (EEmpRequest)RequestList[0];
            try
            {
                ESSAuthorizationProcess authorization = new ESSAuthorizationProcess(dbConn);
                empRequest.EmpRequestRejectReason = RejectReason.Text;
                authorization.AuthorizeLeaveApplication(empRequest, UserID);
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
        RequestFilter.add(new Match("EmpRequestType", EEmpRequest.TYPE_EELEAVEAPP));
        ArrayList RequestList = EEmpRequest.db.select(dbConn, RequestFilter);
        if (RequestList.Count > 0)
        {

            EEmpRequest empRequest = (EEmpRequest)RequestList[0];
            empRequest.EmpRequestRejectReason = RejectReason.Text;
            try
            {
                ESSAuthorizationProcess authorization = new ESSAuthorizationProcess(dbConn);
                authorization.RejectLeaveApplication(empRequest, UserID);
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
        filterStatus.add(new Match("EmpRequestType", EEmpRequest.TYPE_EELEAVEAPP));
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
