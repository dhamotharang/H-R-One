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

public partial class OTClaimCancelRecord : HROneWebControl
{
    public Binding OTClaimBinding;
    public Binding cancelBinding;
    public SearchBinding sbinding;
    public DBManager db = ERequestOTClaimCancel.db;

    public ERequestOTClaimCancel obj;
    public int UserID = -1;
    public int CurEmpID = -1;
    public int CurRequestID = -1;

    public bool ShowAuthorizeOption = true;

    protected const string CONFIRM_MESSAGE = "Are you sure?";

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!WebUtils.CheckAccess(Response, Session))
            return;
        OTClaimBinding = new Binding(dbConn, EOTClaim.db);
        OTClaimBinding.add(EmpID);
        OTClaimBinding.add(OTClaimDateFrom);
        OTClaimBinding.add(OTClaimDateTo);
        OTClaimBinding.add(OTClaimTimeFrom);
        OTClaimBinding.add(OTClaimTimeTo);
        OTClaimBinding.add(OTClaimHours);
        OTClaimBinding.add(OTClaimRemark);
        OTClaimBinding.init(Request, Session);

        cancelBinding = new Binding(dbConn, ERequestOTClaimCancel.db);
        cancelBinding.add(RequestOTClaimCancelReason);
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
        filterStatus.add(new Match("EmpRequestType", EEmpRequest.TYPE_EEOTCLAIMCANCEL));
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
            obj = new ERequestOTClaimCancel();
            obj.RequestOTClaimCancelID = CurRequestID;
            if (!db.select(dbConn, obj))
                return false;

            Hashtable OTCancelValues = new Hashtable();
            db.populate(obj, OTCancelValues);
            cancelBinding.toControl(OTCancelValues);

            EOTClaim OTClaim = new EOTClaim();
            OTClaim.OTClaimID = obj.OTClaimID;
            if (!EOTClaim.db.select(dbConn, OTClaim))
                return false;

            Hashtable OTClaimValues = new Hashtable();
            EOTClaim.db.populate(OTClaim, OTClaimValues);
            OTClaimBinding.toControl(OTClaimValues);

            OTClaimDateToPlaceHolder.Visible = true;
            TimeRow.Visible = true;

            HoursClaimPanel.Visible = true;

            Reject.Attributes.Add("onclick", "return confirm(\"" + HROne.Common.WebUtility.GetLocalizedString(CONFIRM_MESSAGE) + "\");");

            return true;
        }
        else
            return false;
    }

    protected void OTAuthorize_Click(object sender, EventArgs e)
    {
        DBManager db = EEmpRequest.db;
        DBFilter RequestFilter = new DBFilter();

        PageErrors errors = PageErrors.getErrors(db, Page.Master);
        errors.clear();

        RequestFilter.add(new Match("EmpRequestRecordID", CurRequestID));
        RequestFilter.add(new Match("EmpRequestType", EEmpRequest.TYPE_EEOTCLAIMCANCEL));
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

    protected void OTReject_Click(object sender, EventArgs e)
    {

        DBFilter RequestFilter = new DBFilter();
        PageErrors errors = PageErrors.getErrors(db, Page.Master);;
        errors.clear();

        RequestFilter.add(new Match("EmpRequestRecordID", CurRequestID));
        RequestFilter.add(new Match("EmpRequestType", EEmpRequest.TYPE_EEOTCLAIMCANCEL));
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
        filterStatus.add(new Match("EmpRequestType", EEmpRequest.TYPE_EEOTCLAIMCANCEL));
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
