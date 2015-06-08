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
using HROne.Translation;

public partial class Attendance_Plan_Edit : HROneWebPage
{
    private const string FUNCTION_CODE = "ATT003";
    public Binding binding;
    public DBManager db = EAttendancePlan.db;
    public EAttendancePlan obj;
    public int CurID = -1;


    protected void Page_Load(object sender, EventArgs e)
    {

        if (!WebUtils.CheckAccess(Response, Session, FUNCTION_CODE, WebUtils.AccessLevel.ReadWrite))
            return;
        toolBar.FunctionCode = FUNCTION_CODE;



        binding = new Binding(dbConn, db);
        binding.add(AttendancePlanID);
        binding.add(AttendancePlanCode);
        binding.add(AttendancePlanDesc);
        binding.add(new DropDownVLBinder(db, AttendancePlanAbsentProrataPayFormID, EPayrollProrataFormula.VLPayrollProrataFormula));
        binding.add(AttendancePlanOTMinsUnit);
        binding.add(AttendancePlanLateMinsUnit);

        DBFilter overtimePaymentTypeFilter = new DBFilter();
        OR orOverTimePaymentTerms = new OR();
        orOverTimePaymentTerms.add(new Match("PaymentTypeID", EPaymentType.SystemPaymentType.BasicSalaryPaymentType(dbConn).PaymentTypeID));
        orOverTimePaymentTerms.add(new Match("PaymentTypeID", EPaymentType.SystemPaymentType.OverTimePaymentType(dbConn).PaymentTypeID));
        overtimePaymentTypeFilter.add(orOverTimePaymentTerms);

        DBFilter salaryOthersFilter = new DBFilter();
        OR orSalaryOthersTerms = new OR();
        orSalaryOthersTerms.add(new Match("PaymentTypeID", EPaymentType.SystemPaymentType.BasicSalaryPaymentType(dbConn).PaymentTypeID));
        orSalaryOthersTerms.add(new Match("PaymentTypeID", EPaymentType.SystemPaymentType.BonusPaymentType(dbConn).PaymentTypeID));
        orSalaryOthersTerms.add(new Match("PaymentTypeID", EPaymentType.SystemPaymentType.OthersPaymentType(dbConn).PaymentTypeID));
        salaryOthersFilter.add(orSalaryOthersTerms);

        binding.add(new DropDownVLBinder(db, AttendancePlanOTFormula, EAttendanceFormula.VLAttendanceFormula));
        binding.add(new DropDownVLBinder(db, AttendancePlanLateFormula, EAttendanceFormula.VLAttendanceFormula));
        binding.add(new DropDownVLBinder(db, AttendancePlanOTPayCodeID, EPaymentCode.VLPaymentCode, overtimePaymentTypeFilter));
        binding.add(new DropDownVLBinder(db, AttendancePlanLatePayCodeID, EPaymentCode.VLPaymentCode, salaryOthersFilter));
        binding.add(new DropDownVLBinder(db, AttendancePlanOTMinsRoundingRule, Values.VLRoundingRule));
        binding.add(new DropDownVLBinder(db, AttendancePlanLateMinsRoundingRule, Values.VLRoundingRule));
        binding.add(new CheckBoxBinder(db, AttendancePlanOTIncludeLunchOvertime));
        binding.add(new CheckBoxBinder(db, AttendancePlanLateIncludeEarlyLeave));
        binding.add(new CheckBoxBinder(db, AttendancePlanLateIncludeLunchLate));
        binding.add(new CheckBoxBinder(db, AttendancePlanLateIncludeLunchEarlyLeave));
        binding.add(new CheckBoxBinder(db, AttendancePlanCompensateLateByOT));
        binding.add(AttendancePlanOTRateMultiplier);
        binding.add(new CheckBoxBinder(db, AttendancePlanOTGainAsCompensationLeaveEntitle));
        binding.add(AttendancePlanLateMaxTotalToleranceMins);

        binding.add(AttendancePlanBonusMaxTotalLateCount);
        binding.add(new CheckBoxBinder(db,AttendancePlanBonusMaxTotalLateCountIncludeLunch));
        binding.add(AttendancePlanBonusMaxTotalLateMins);
        binding.add(new CheckBoxBinder(db,AttendancePlanBonusMaxTotalLateMinsIncludeLunch));
        binding.add(AttendancePlanBonusMaxTotalEarlyLeaveCount);
        binding.add(new CheckBoxBinder(db,AttendancePlanBonusMaxTotalEarlyLeaveCountIncludeLunch));
        binding.add(AttendancePlanBonusMaxTotalEarlyLeaveMins);
        binding.add(new CheckBoxBinder(db,AttendancePlanBonusMaxTotalEarlyLeaveMinsIncludeLunch));
        binding.add(AttendancePlanBonusMaxTotalSLWithMedicalCertificate);
        binding.add(AttendancePlanBonusMaxTotalSLWithoutMedicalCertificate);
        binding.add(AttendancePlanBonusMaxTotalInjuryLeave);
        binding.add(AttendancePlanBonusMaxTotalNonFullPayCasualLeave);
        binding.add(AttendancePlanBonusMaxTotalAbsentCount);
        binding.add(new CheckBoxBinder(db, AttendancePlanTerminatedHasBonus));

        binding.add(AttendancePlanBonusAmount);
        binding.add(new CheckBoxBinder(db, AttendancePlanUseBonusAmountByRecurringPayment));
        binding.add(AttendancePlanBonusOTAmount);
        binding.add(new DropDownVLBinder(db, AttendancePlanBonusAmountUnit, Values.VLPaymentUnit));
        binding.add(new CheckBoxBinder(db, AttendancePlanProrataBonusforNewJoin));
        binding.add(new CheckBoxBinder(db, AttendancePlanProrataBonusforTerminated));
        binding.add(new DropDownVLBinder(db, AttendancePlanBonusPayCodeID, EPaymentCode.VLPaymentCode, salaryOthersFilter));

        binding.add(AttendancePlanBonusPartialPaidMaxTotalLateCount);
        binding.add(new CheckBoxBinder(db,AttendancePlanBonusPartialPaidMaxTotalLateCountIncludeLunch));
        binding.add(AttendancePlanBonusPartialPaidMaxTotalLateMins);
        binding.add(new CheckBoxBinder(db,AttendancePlanBonusPartialPaidMaxTotalLateMinsIncludeLunch));
        binding.add(AttendancePlanBonusPartialPaidMaxTotalEarlyLeaveCount);
        binding.add(new CheckBoxBinder(db,AttendancePlanBonusPartialPaidMaxTotalEarlyLeaveCountIncludeLunch));
        binding.add(AttendancePlanBonusPartialPaidMaxTotalEarlyLeaveMins);
        binding.add(new CheckBoxBinder(db,AttendancePlanBonusPartialPaidMaxTotalEarlyLeaveMinsIncludeLunch));
        binding.add(AttendancePlanBonusPartialPaidMaxTotalSLWithMedicalCertificate);
        binding.add(AttendancePlanBonusPartialPaidMaxTotalSLWithoutMedicalCertificate);
        binding.add(AttendancePlanBonusPartialPaidMaxTotalInjuryLeave);
        binding.add(AttendancePlanBonusPartialPaidMaxTotalNonFullPayCasualLeave);
        binding.add(AttendancePlanBonusPartialPaidMaxTotalAbsentCount);
        binding.add(AttendancePlanBonusPartialPaidPercent);

        binding.init(Request, Session);

        if (!int.TryParse(DecryptedRequest["AttendancePlanID"], out CurID))
            CurID = -1;

        //  MUST applied Label Change before translation
        if (CurID > 0)
            ActionHeader.Text = "Edit";
        else
            ActionHeader.Text = "Add";

        HROne.Common.WebUtility.WebControlsLocalization(this, this.Controls);

        if (!Page.IsPostBack)
        {
            if (CurID > 0)
                loadObject();
            else
                toolBar.DeleteButton_Visible = false;
        }
    }

    protected bool loadObject() 
    {
	    obj=new EAttendancePlan();
	    bool isNew=WebFormWorkers.loadKeys(db, obj, DecryptedRequest);
	    if(!db.select(dbConn, obj))
		    return false;

        if (obj.AttendancePlanBonusAmountUnit.Equals("H", StringComparison.CurrentCultureIgnoreCase))
        {
            OTBonusPanel.Visible = false;
        }
        else
        {
            OTBonusPanel.Visible = true;
        }

        Hashtable values = new Hashtable();
	    db.populate(obj,values);
	    binding.toControl(values);
        RefreshOTPayrollRegion();
        return true;
    }

    protected void Save_Click(object sender, EventArgs e)
    {
        EAttendancePlan c = new EAttendancePlan();

        Hashtable values = new Hashtable();
        binding.toValues(values);

        PageErrors errors = PageErrors.getErrors(db, Page.Master);
        errors.clear();


        db.validate(errors, values);

        if (!errors.isEmpty())
            return;


        db.parse(values, c);
        if (!AppUtils.checkDuplicate(dbConn, db, c, errors, "AttendancePlanCode"))
            return;

        WebUtils.StartFunction(Session, FUNCTION_CODE);
        if (CurID < 0)
        {
//            Utils.MarkCreate(Session, c);

            db.insert(dbConn, c);
            CurID = c.AttendancePlanID;
//            url = Utils.BuildURL(-1, CurID);
        }
        else
        {
//            Utils.Mark(Session, c);
            db.update(dbConn, c);
        }
        WebUtils.EndFunction(dbConn);


        HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "Attendance_Plan_View.aspx?AttendancePlanID="+CurID);


    }
    protected void Delete_Click(object sender, EventArgs e)
    {

        PageErrors errors = PageErrors.getErrors(db, Page.Master);
        errors.clear();

        EAttendancePlan o = new EAttendancePlan();
        o.AttendancePlanID = CurID;

        if (EAttendancePlan.db.select(dbConn, o))
        {
            DBFilter empPosFilter = new DBFilter();
            empPosFilter.add(new Match("AttendancePlanID", o.AttendancePlanID));
            empPosFilter.add("EmpID", true);
            ArrayList empPosList = EEmpPositionInfo.db.select(dbConn, empPosFilter);
            if (empPosList.Count > 0)
            {
                errors.addError(string.Format(HROne.Translation.PageErrorMessage.ERROR_CODE_USED_BY_EMPLOYEE, new string[] { HROne.Common.WebUtility.GetLocalizedString("Attendance Plan"), o.AttendancePlanCode }));
                foreach (EEmpPositionInfo empPos in empPosList)
                {
                    EEmpPersonalInfo empInfo = new EEmpPersonalInfo();
                    empInfo.EmpID = empPos.EmpID;
                    if (EEmpPersonalInfo.db.select(dbConn, empInfo))
                        errors.addError("- " + empInfo.EmpNo + ", " + empInfo.EmpEngFullName);
                    else
                        EEmpPositionInfo.db.delete(dbConn, empPos);

                }
                errors.addError(HROne.Translation.PageErrorMessage.ERROR_ACTION_ABORT);
                return;
            }
            else
            {
                WebUtils.StartFunction(Session, FUNCTION_CODE);
                db.delete(dbConn, o);
                WebUtils.EndFunction(dbConn);
            }
        }
        HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "Attendance_Plan_List.aspx");

    }
    protected void Back_Click(object sender, EventArgs e)
    {
        if (CurID > 0)
            HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "Attendance_Plan_View.aspx?AttendancePlanID=" + CurID);
        else
            HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "Attendance_Plan_List.aspx");

    }
    protected void AttendancePlanBonusAmountUnit_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (AttendancePlanBonusAmountUnit.SelectedValue.Equals("H", StringComparison.CurrentCultureIgnoreCase))
        {
            OTBonusPanel.Visible = false;
        }
        else
        {
            OTBonusPanel.Visible = true;
        }

    }
    protected void RefreshOTPayrollRegion()
    {
        OTFormulaRow.Visible = !AttendancePlanOTGainAsCompensationLeaveEntitle.Checked;
        OTPaymentCodeRow.Visible = !AttendancePlanOTGainAsCompensationLeaveEntitle.Checked;
        OTRatioRow.Visible = !AttendancePlanOTGainAsCompensationLeaveEntitle.Checked;
    }
    protected void AttendancePlanOTGainAsCompensationLeaveEntitle_CheckedChanged(object sender, EventArgs e)
    {
        RefreshOTPayrollRegion();
    }
}
