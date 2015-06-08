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


public partial class LeaveApplicationCancel_Form : HROneWebControl
{
    public Binding leaveApplicationBinding;
    public SearchBinding sbinding;

    public ELeaveApplication obj;
    public int UserID = -1;
    public int CurLeaveAppID = -1;


    protected const string CONFIRM_MESSAGE = "Are you sure?";

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!WebUtils.CheckAccess(Response, Session))
            return;
        leaveApplicationBinding = new Binding(dbConn, ELeaveApplication.db);
        leaveApplicationBinding.add(EmpID);
        leaveApplicationBinding.add(LeaveAppID);
        //binding.add(new LabelVLBinder(db, EmpName,"EmpID", EEmpPersonalInfo.VLEmp));
        leaveApplicationBinding.add(new LabelVLBinder(ELeaveApplication.db, LeaveCodeID, ELeaveCode.VLLeaveCode));
        leaveApplicationBinding.add(new LabelVLBinder(ELeaveApplication.db, LeaveAppUnit, Values.VLLeaveUnit));
        leaveApplicationBinding.add(LeaveAppDateFrom);
        leaveApplicationBinding.add(LeaveAppDateTo);
        leaveApplicationBinding.add(LeaveAppTimeFrom);
        leaveApplicationBinding.add(LeaveAppTimeTo);
        leaveApplicationBinding.add(LeaveDays);
        leaveApplicationBinding.add(LeaveAppHours);
        leaveApplicationBinding.add(LeaveAppRemark);
        leaveApplicationBinding.add(new LabelVLBinder(ELeaveApplication.db, LeaveAppHasMedicalCertificate, Values.VLTrueFalseYesNo));
        leaveApplicationBinding.init(Request, Session);


        EESSUser user = WebUtils.GetCurUser(Session);
        if (user != null)
        {
            UserID = user.EmpID;
        }
        if (!Int32.TryParse(DecryptedRequest["LeaveAppID"], out CurLeaveAppID))
            CurLeaveAppID = -1;


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
    }
    protected bool loadObject()
    {

        obj = new ELeaveApplication();
        obj.LeaveAppID = CurLeaveAppID;
        if (!ELeaveApplication.db.select(dbConn, obj))
            return false;

        if (obj.EmpID != UserID)
            return false;
        Hashtable values = new Hashtable();
        ELeaveApplication.db.populate(obj, values);
        leaveApplicationBinding.toControl(values);
        //EEmpPersonalInfo EmpInfo = new EEmpPersonalInfo();
        //EmpInfo.EmpID = obj.EmpID;
        //if (EEmpPersonalInfo.db.select(dbConn, EmpInfo))
        //    EmpName.Text = EmpInfo.EmpNo + " - " + EmpInfo.EmpEngFullNameWithAlias;

        if (obj.LeaveAppUnit.Equals("D"))
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
        leaveCode.LeaveCodeID = obj.LeaveCodeID;
        if (ELeaveCode.db.select(dbConn, leaveCode))
        {
            LeaveCodeIsShowMedicalCertOptionPanel.Visible = leaveCode.LeaveCodeIsShowMedicalCertOption;

            if (leaveCode.LeaveTypeID.Equals(ELeaveType.COMPENSATION_LEAVE_TYPE(dbConn).LeaveTypeID))
                HoursClaimPanel.Visible = true;
            else
                HoursClaimPanel.Visible = false;

        }
        else
            HoursClaimPanel.Visible = false;



        return true;
    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        DateTime createDate = DateTime.Now;
        ERequestLeaveApplicationCancel c = new ERequestLeaveApplicationCancel();
        EEmpRequest EmpRequest = new EEmpRequest();


        Binding leaveCancelBinding = new Binding(dbConn, ERequestLeaveApplicationCancel.db);
        leaveCancelBinding.add(EmpID);
        leaveCancelBinding.add(LeaveAppID);
        leaveCancelBinding.add(RequestLeaveAppCancelReason);

        Hashtable values = new Hashtable();
        leaveCancelBinding.toValues(values);

        PageErrors errors = PageErrors.getErrors(ERequestLeaveApplicationCancel.db, Page);
        errors.clear();


        ERequestLeaveApplicationCancel.db.validate(errors, values);

        if (!errors.isEmpty())
            return;


        ERequestLeaveApplicationCancel.db.parse(values, c);

        if (!errors.isEmpty())
            return;



        try
        {
            ESSAuthorizationProcess authorization = new ESSAuthorizationProcess(dbConn);
            authorization.SubmitLeaveApplicatoinCancel(c);
        }
        catch (Exception ex)
        {
            errors.addError(ex.Message);

        }

        if (!errors.isEmpty())
            return;
        HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "~/ESS_EmpRequestStatus.aspx");


    }
}
