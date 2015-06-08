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
using HROne.Lib.Entities;
using System.Net.Mail;

public partial class LateWaiveRecord : HROneWebControl
{
    public Binding binding;
    public SearchBinding sbinding;
    public DBManager db = ERequestLateWaive.db;
    public DBManager sdb = EAttendanceRecord.db;

    public ERequestLateWaive obj;
    public int UserID = -1;
    public string UserName = "";
    public int CurEmpID = -1;
    public int CurRequestID = -1;

    public bool ShowAuthorizeOption = true;

    protected const string CONFIRM_MESSAGE = "Are you sure?";
    protected const string AL_AUTHORIZED_BEFORE_PROBATION_MESSAGE = "Staff is under probation and not entitled to late waive. Continue to proceed the authorization?";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!WebUtils.CheckAccess(Response, Session))
            return;
        binding = new Binding(dbConn, db);
        binding.add(EmpID);
        binding.add(AttendanceRecordID);
        binding.add(RequestLateWaiveReason);
        binding.init(Request, Session);


        EESSUser user = WebUtils.GetCurUser(Session);
        if (user != null)
        {
            UserID = user.EmpID;
            UserName = user.EmpEngFullNameWithAlias;
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

                ApprovedBy.Text = UserName;
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
        filterStatus.add(new Match("EmpRequestType", EEmpRequest.TYPE_EELATEWAIVE ));
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
            obj = new ERequestLateWaive();
            obj.RequestLateWaiveID = CurRequestID;
            if (!db.select(dbConn, obj))
                return false;

            EAttendanceRecord aObj = new EAttendanceRecord();
            aObj.AttendanceRecordID = obj.AttendanceRecordID;
            if (sdb.select(dbConn, aObj))
            {
                // Start 0000112, Miranda, 2015-01-17
                AttendanceRecordDate.Text = aObj.AttendanceRecordDate.ToString("yyyy-MM-dd");
                ERosterCode rcObj = new ERosterCode();
                rcObj.RosterCodeID = aObj.RosterCodeID;
                if(ERosterCode.db.select(dbConn, rcObj)) {
                    RosterCode.Text = rcObj.RosterCode;
                    RosterCodeDesc.Text = rcObj.RosterCodeDesc;
                    RosterCodeInTime.Text = rcObj.RosterCodeInTime.ToString("HH:mm");
                    RosterCodeOutTime.Text = rcObj.RosterCodeOutTime.ToString("HH:mm");
                }
                AttendanceRecordWorkStart.Text = aObj.AttendanceRecordWorkStart.ToString("HH:mm");
                AttendanceRecordWorkEnd.Text = aObj.AttendanceRecordWorkEnd.ToString("HH:mm");
                // End 0000112, Miranda, 2015-01-17
                AttendanceRecordActualLateMins.Text = aObj.AttendanceRecordActualLateMins.ToString();
            }

            Hashtable values = new Hashtable();
            db.populate(obj, values);
            binding.toControl(values);

            LateWaiveDateToPlaceHolder.Visible = true;

            string LateWaiveAuthorizedMessage = CONFIRM_MESSAGE;
            
            Authorize.Attributes.Add("onclick", "return confirm(\"" + HROne.Common.WebUtility.GetLocalizedString(LateWaiveAuthorizedMessage) + "\");");
            Reject.Attributes.Add("onclick", "return confirm(\"" + HROne.Common.WebUtility.GetLocalizedString(CONFIRM_MESSAGE) + "\");");


            return true;
        }
        else
            return false;
    }

    protected void LateWaiveAuthorize_Click(object sender, EventArgs e)
    {
        DBManager db = EEmpRequest.db;
        DBFilter RequestFilter = new DBFilter();

        PageErrors errors = PageErrors.getErrors(db, Page.Master);
        errors.clear();

        RequestFilter.add(new Match("EmpRequestRecordID", CurRequestID));
        RequestFilter.add(new Match("EmpRequestType", EEmpRequest.TYPE_EELATEWAIVE));
        ArrayList RequestList = EEmpRequest.db.select(dbConn, RequestFilter);
        if (RequestList.Count > 0)
        {
            EEmpRequest empRequest = (EEmpRequest)RequestList[0];
            try
            {
                ESSAuthorizationProcess authorization = new ESSAuthorizationProcess(dbConn);
                empRequest.EmpRequestRejectReason = RejectReason.Text;
                authorization.AuthorizeLateWaive(empRequest, UserID);
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

    protected void LateWaiveReject_Click(object sender, EventArgs e)
    {
        DBFilter RequestFilter = new DBFilter();
        PageErrors errors = PageErrors.getErrors(db, Page.Master);;
        errors.clear();

        RequestFilter.add(new Match("EmpRequestRecordID", CurRequestID));
        RequestFilter.add(new Match("EmpRequestType", EEmpRequest.TYPE_EELATEWAIVE));
        ArrayList RequestList = EEmpRequest.db.select(dbConn, RequestFilter);
        if (RequestList.Count > 0)
        {

            EEmpRequest empRequest = (EEmpRequest)RequestList[0];
            empRequest.EmpRequestRejectReason = RejectReason.Text;
            try
            {
                ESSAuthorizationProcess authorization = new ESSAuthorizationProcess(dbConn);
                authorization.RejectLateWaive(empRequest, UserID);
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
        filterStatus.add(new Match("EmpRequestType", EEmpRequest.TYPE_EELATEWAIVE));
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
