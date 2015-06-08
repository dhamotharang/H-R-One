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

public partial class OTClaimRecord : HROneWebControl
{
    public Binding binding;
    public SearchBinding sbinding;
    public DBManager db = ERequestOTClaim.db;

    public ERequestOTClaim obj;
    public int UserID = -1;
    public string UserName = "";
    public int CurEmpID = -1;
    public int CurRequestID = -1;

    public bool ShowAuthorizeOption = true;

    protected const string CONFIRM_MESSAGE = "Are you sure?";
    protected const string AL_AUTHORIZED_BEFORE_PROBATION_MESSAGE = "Staff is under probation and not entitled to OT. Continue to proceed the authorization?";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!WebUtils.CheckAccess(Response, Session))
            return;
        binding = new Binding(dbConn, db);
        binding.add(EmpID);
        binding.add(RequestOTClaimPeriodFrom);
        binding.add(RequestOTClaimPeriodTo);
        binding.add(RequestOTClaimHourFrom);
        binding.add(RequestOTClaimHourTo);
        binding.add(RequestOTHours);
        binding.add(RequestOTClaimRemark);
        // Start 0000060, Miranda, 2014-07-22
        binding.add(new TextBoxBinder(db, RequestOTClaimEffectiveDate.TextBox, RequestOTClaimEffectiveDate.ID));
        binding.add(new TextBoxBinder(db, RequestOTClaimDateExpiry.TextBox, RequestOTClaimDateExpiry.ID));
        // End 0000060, Miranda, 2014-07-22
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

                DateTime dtFrom;
                if (DateTime.TryParse(RequestOTClaimPeriodFrom.Text, out dtFrom) && RequestOTClaimDateExpiry.TextBox.Text == "")
                {
                    RequestOTClaimDateExpiry.Value = calExpiryDate(RequestOTClaimPeriodFrom.Text).ToString("yyyy-MM-dd");
                }

                RequestOTClaimEffectiveDate.Value = AppUtils.ServerDateTime().ToString("yyyy-MM-dd");// Default CL Requisition effective date is today
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
        filterStatus.add(new Match("EmpRequestType", EEmpRequest.TYPE_EEOTCLAIM ));
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
            obj = new ERequestOTClaim();
            obj.RequestOTClaimID = CurRequestID;
            if (!db.select(dbConn, obj))
                return false;

            Hashtable values = new Hashtable();
            db.populate(obj, values);
            binding.toControl(values);

            OTClaimDateToPlaceHolder.Visible = true;
            TimeRow.Visible = true;

            string OTAuthorizedMessage = CONFIRM_MESSAGE;
            HoursClaimPanel.Visible = true;
            
            Authorize.Attributes.Add("onclick", "return confirm(\"" + HROne.Common.WebUtility.GetLocalizedString(OTAuthorizedMessage) + "\");");
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
        RequestFilter.add(new Match("EmpRequestType", EEmpRequest.TYPE_EEOTCLAIM));
        ArrayList RequestList = EEmpRequest.db.select(dbConn, RequestFilter);
        if (RequestList.Count > 0)
        {
            EEmpRequest empRequest = (EEmpRequest)RequestList[0];
            try
            {
                // Start 0000060, Miranda, 2014-07-22
                ERequestOTClaim reqOTClaim = new ERequestOTClaim();
                Hashtable values = new Hashtable();
                binding.toValues(values);
                ERequestOTClaim.db.parse(values, reqOTClaim);

                DBFilter filterOT = new DBFilter();
                filterOT.add(new Match("RequestOTClaimID", CurRequestID));
                ArrayList RequestOTList = ERequestOTClaim.db.select(dbConn, filterOT);
                if (RequestOTList.Count > 0)
                {
                    ERequestOTClaim empOTRequest = (ERequestOTClaim)RequestOTList[0];
                    empOTRequest.RequestOTClaimDateExpiry = reqOTClaim.RequestOTClaimDateExpiry;
                    empOTRequest.RequestOTClaimEffectiveDate = reqOTClaim.RequestOTClaimEffectiveDate;
                    ERequestOTClaim.db.update(dbConn, empOTRequest);
                }
                // End 0000060, Miranda, 2014-07-22

                ESSAuthorizationProcess authorization = new ESSAuthorizationProcess(dbConn);
                empRequest.EmpRequestRejectReason = RejectReason.Text;
                authorization.AuthorizeOTClaim(empRequest, UserID);
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
        RequestFilter.add(new Match("EmpRequestType", EEmpRequest.TYPE_EEOTCLAIM));
        ArrayList RequestList = EEmpRequest.db.select(dbConn, RequestFilter);
        if (RequestList.Count > 0)
        {

            EEmpRequest empRequest = (EEmpRequest)RequestList[0];
            empRequest.EmpRequestRejectReason = RejectReason.Text;
            try
            {
                ESSAuthorizationProcess authorization = new ESSAuthorizationProcess(dbConn);
                authorization.RejectOTClaim(empRequest, UserID);
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
        filterStatus.add(new Match("EmpRequestType", EEmpRequest.TYPE_EEOTCLAIM));
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

    private DateTime calExpiryDate(string periodFromDate)
    {
        DateTime dtFrom;
        DateTime dtTo = DateTime.Today;
        int expiryInt;

        string essDefEotExpiry = ESystemParameter.getParameter(dbConn, ESystemParameter.PARAM_CODE_ESS_DEF_EOT_EXPIRY);
        string essDefEotExpiryType = ESystemParameter.getParameter(dbConn, ESystemParameter.PARAM_CODE_ESS_DEF_EOT_EXPIRY_TYPE);

        if (string.IsNullOrEmpty(essDefEotExpiry)) // Default is one Month 
        {
            expiryInt = 1;
        }
        else
        {
            int.TryParse(essDefEotExpiry, out expiryInt);
        }

        if (DateTime.TryParse(periodFromDate, out dtFrom))
        {
            if (string.IsNullOrEmpty(essDefEotExpiryType) || essDefEotExpiryType.Equals("M")) // Default type is Month, "M" means Month
            {
                dtTo = dtFrom.AddMonths(expiryInt).AddDays(-1);
            }
            else if (essDefEotExpiryType.Equals("Y")) // "Y" means Year
            {
                dtTo = dtFrom.AddYears(expiryInt).AddDays(-1);
            }
            else if (essDefEotExpiryType.Equals("D")) // "D" means Day
            {
                dtTo = dtFrom.AddDays(expiryInt).AddDays(-1);
            }
        }

        // start Ricky So, 2014-08-21, as requested by HK Ballet, default to last day of the month
        dtTo = dtTo.AddDays(-1 * (dtTo.Day-1)).AddMonths(1).AddDays(-1);

        // end

        return dtTo;
    }
    // End 0000060, Miranda, 2014-07-22

}
