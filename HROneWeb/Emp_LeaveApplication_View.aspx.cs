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
public partial class Emp_LeaveApplication_View : HROneWebPage
{
    private const string FUNCTION_CODE = "PER009";

    public Binding binding;
    public DBManager db = ELeaveApplication.db;
    public ELeaveApplication obj;
    public int CurID = -1;
    public int CurEmpID = -1;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!WebUtils.CheckAccess(Response, Session, FUNCTION_CODE, WebUtils.AccessLevel.Read))
            return;
        toolBar.FunctionCode = FUNCTION_CODE;

        binding = new Binding(dbConn, db);
        binding.add(LeaveAppID);
        binding.add(EmpID);
        binding.add(new LabelVLBinder(db, LeaveCodeID, ELeaveCode.VLLeaveCode));
        binding.add(new LabelVLBinder(db, LeaveAppUnit, Values.VLLeaveUnit));
        binding.add(LeaveAppDateFrom);
        binding.add(LeaveAppDateTo);
        binding.add(new LabelVLBinder(db, LeaveAppTimeFrom, new WFHourList()));
        binding.add(new LabelVLBinder(db, LeaveAppTimeTo, new WFHourList()));
        binding.add(LeaveAppDays);
        binding.add(LeaveAppHours);
        binding.add(LeaveAppRemark);
        binding.init(Request, Session);
        binding.add(new LabelVLBinder(db, LeaveAppNoPayProcess, Values.VLTrueFalseYesNo));
        binding.add(new LabelVLBinder(db, LeaveAppHasMedicalCertificate, Values.VLTrueFalseYesNo));
        
        binding.add(new LabelVLBinder(db, PayPeriodID, EPayrollPeriod.VLPayrollPeriod));

        if (!int.TryParse(DecryptedRequest["LeaveAppID"], out CurID))
            CurID = -1;

        if (!int.TryParse(DecryptedRequest["EmpID"], out CurEmpID))
            CurEmpID = -1;

        HROne.Common.WebUtility.WebControlsLocalization(this, this.Controls);

		if(!Page.IsPostBack) 
		{
            if (CurID > 0)
                loadObject();
            else
                toolBar.DeleteButton_Visible = false;

        }

        EmpID.Value = CurEmpID.ToString();

        toolBar.CustomButton2_ClientClick = HROne.Translation.PromptMessage.CreateConfirmDialogJavascript(HROne.Common.WebUtility.GetLocalizedStringByCode("LEAVEAPP_FORCE_DELETE_JAVASCRIPT", "The leave application is payroll processed.\r\nYou need to adjust the payroll record manually.\r\nAre you sure to delete?"), toolBar.FindControl("CustomButton2"));
    }


    protected bool loadObject()
    {
        obj = new ELeaveApplication();
        bool isNew = WebFormWorkers.loadKeys(db, obj, DecryptedRequest);

        DBFilter filter = new DBFilter();
        filter.add(new Match("EmpID", CurEmpID));
        filter.add(WebUtils.AddRankFilter(Session, "EmpID", true));
        if (EEmpPersonalInfo.db.count(dbConn, filter) == 0)
            if (CurEmpID <= 0)
                return false;
            else
                HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "~/AccessDeny.aspx");

        if (!db.select(dbConn, obj))
            if (CurID <= 0)
                return false;
            else
                HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "~/AccessDeny.aspx");

        if (obj.EmpID != CurEmpID)
            if (CurID <= 0)
                return false;
            else
                HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "~/AccessDeny.aspx");

        Hashtable values = new Hashtable();
        db.populate(obj, values);

        //int empPayrollID = int.Parse((string)values["EmpPayrollID"]);
        if (obj.EmpPayrollID > 0)
        {
            EEmpPayroll empPayroll = new EEmpPayroll();
            empPayroll.EmpPayrollID = obj.EmpPayrollID;
            if (EEmpPayroll.db.select(dbConn, empPayroll))
                values.Add("PayPeriodID", empPayroll.PayPeriodID.ToString());
        }

        binding.toControl(values);
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
        if (obj.EmpPaymentID != 0 || obj.EmpPayrollID != 0)
        {
            toolBar.EditButton_Visible = false;
            toolBar.DeleteButton_Visible = false;
            toolBar.CustomButton1_Visible = true;
            toolBar.CustomButton2_Visible = true;
        }
        else
        {
            toolBar.EditButton_Visible = true;
            toolBar.DeleteButton_Visible = true;
            toolBar.CustomButton1_Visible = false;
            toolBar.CustomButton2_Visible = false;
        }

        if (obj.LeaveAppCancelID > 0)
        {
            toolBar.EditButton_Visible = false;
            toolBar.DeleteButton_Visible = false;
            toolBar.CustomButton1_Visible = false;
            toolBar.CustomButton2_Visible = false;
        }

        ELeaveCode leaveCode = new ELeaveCode();
        leaveCode.LeaveCodeID = obj.LeaveCodeID;
        if (ELeaveCode.db.select(dbConn, leaveCode))
        {
            PayrollProcessPanel.Visible = !leaveCode.LeaveCodeIsSkipPayrollProcess;
            LeaveCodeIsShowMedicalCertOptionPanel.Visible = leaveCode.LeaveCodeIsShowMedicalCertOption;
            //if (!leaveCode.LeaveCodeIsSkipPayrollProcess)
            //{
            //    ELeaveApplication leaveApp = new ELeaveApplication();
            //    leaveApp.LeaveAppID = CurID;
            //    if (ELeaveApplication.db.select(dbConn, leaveApp))
            //    {
            //        ELeaveCode prevLeaveCode = new ELeaveCode();
            //        prevLeaveCode.LeaveCodeID = leaveApp.LeaveCodeID;
            //        if (ELeaveCode.db.select(dbConn, prevLeaveCode))
            //            if (prevLeaveCode.LeaveCodeIsSkipPayrollProcess)
            //                LeaveAppNoPayProcess.Checked = false;
            //    }
            //}

            if (leaveCode.LeaveTypeID.Equals(ELeaveType.COMPENSATION_LEAVE_TYPE(dbConn).LeaveTypeID))
                HoursClaimPanel.Visible = true;
            else
                HoursClaimPanel.Visible = false;
        }
        else
            HoursClaimPanel.Visible = false;
        CurEmpID = obj.EmpID;
        return true;
    }

    protected void Delete_Click(object sender, EventArgs e)
    {

        ELeaveApplication c = new ELeaveApplication();
        c.LeaveAppID = CurID;
        db.select(dbConn, c);
        WebUtils.StartFunction(Session, FUNCTION_CODE, c.EmpID);
        db.delete(dbConn, c);
        WebUtils.EndFunction(dbConn);
        //HROne.LeaveCalc.LeaveBalanceCalc leaaveBalCal = new HROne.LeaveCalc.LeaveBalanceCalc(dbConn, c.EmpID);
        //leaaveBalCal.RecalculateAfter(c.LeaveAppDateFrom, ELeaveType.COMPENSATION_LEAVE_TYPE(dbConn).LeaveTypeID);
        HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "EmpTab_LeaveApplication_View.aspx?EmpID=" + EmpID.Value);
    }

    protected void Edit_Click(object sender, EventArgs e)
    {
        HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "Emp_LeaveApplication_Edit.aspx?LeaveAppID=" + LeaveAppID.Value + "&EmpID=" + EmpID.Value);
    }

    protected void Back_Click(object sender, EventArgs e)
    {
        HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "EmpTab_LeaveApplication_View.aspx?EmpID=" + EmpID.Value);
    }
    protected void btnForceEdit_Click(object sender, EventArgs e)
    {
        HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "Emp_LeaveApplication_Edit.aspx?LeaveAppID=" + LeaveAppID.Value + "&EmpID=" + EmpID.Value);
    }
    protected void btnForceDelete_Click(object sender, EventArgs e)
    {
        Delete_Click(sender, e);
    }
}
