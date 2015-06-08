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

public partial class Attendance_Plan_View : HROneWebPage
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
        binding.add(new BlankZeroLabelVLBinder(db, AttendancePlanAbsentProrataPayFormID, EPayrollProrataFormula.VLPayrollProrataFormula));
        binding.add(AttendancePlanOTMinsUnit);
        binding.add(AttendancePlanLateMinsUnit);

        binding.add(new BlankZeroLabelVLBinder(db, AttendancePlanOTFormula, EAttendanceFormula.VLAttendanceFormula));
        binding.add(new BlankZeroLabelVLBinder(db, AttendancePlanLateFormula, EAttendanceFormula.VLAttendanceFormula));
        binding.add(new BlankZeroLabelVLBinder(db, AttendancePlanOTPayCodeID, EPaymentCode.VLPaymentCode));
        binding.add(new BlankZeroLabelVLBinder(db, AttendancePlanLatePayCodeID, EPaymentCode.VLPaymentCode));
        binding.add(new LabelVLBinder(db, AttendancePlanOTMinsRoundingRule, Values.VLRoundingRule));
        binding.add(new LabelVLBinder(db, AttendancePlanLateMinsRoundingRule, Values.VLRoundingRule));
        binding.add(new LabelVLBinder(db, AttendancePlanOTIncludeLunchOvertime, Values.VLTrueFalseYesNo));
        binding.add(new LabelVLBinder(db, AttendancePlanLateIncludeEarlyLeave, Values.VLTrueFalseYesNo));
        binding.add(new LabelVLBinder(db, AttendancePlanLateIncludeLunchLate, Values.VLTrueFalseYesNo));
        binding.add(new LabelVLBinder(db, AttendancePlanLateIncludeLunchEarlyLeave, Values.VLTrueFalseYesNo));
        binding.add(new LabelVLBinder(db, AttendancePlanCompensateLateByOT, Values.VLTrueFalseYesNo));
        binding.add(AttendancePlanOTRateMultiplier);
        binding.add(new LabelVLBinder(db, AttendancePlanOTGainAsCompensationLeaveEntitle, Values.VLTrueFalseYesNo));
        binding.add(AttendancePlanLateMaxTotalToleranceMins);

        binding.add(AttendancePlanBonusMaxTotalLateCount);
        binding.add(AttendancePlanBonusMaxTotalLateMins);
        binding.add(AttendancePlanBonusMaxTotalEarlyLeaveCount);
        binding.add(AttendancePlanBonusMaxTotalEarlyLeaveMins);
        binding.add(AttendancePlanBonusMaxTotalSLWithMedicalCertificate);
        binding.add(AttendancePlanBonusMaxTotalSLWithoutMedicalCertificate);
        binding.add(AttendancePlanBonusMaxTotalInjuryLeave);
        binding.add(AttendancePlanBonusMaxTotalNonFullPayCasualLeave);
        binding.add(AttendancePlanBonusMaxTotalAbsentCount);

        binding.add(new LabelVLBinder(db, AttendancePlanTerminatedHasBonus, Values.VLTrueFalseYesNo));

        binding.add(AttendancePlanBonusAmount);
        binding.add(AttendancePlanBonusOTAmount);
        binding.add(new LabelVLBinder(db, AttendancePlanBonusAmountUnit, Values.VLPaymentUnit));
        binding.add(new BlankZeroLabelVLBinder(db, AttendancePlanBonusPayCodeID, EPaymentCode.VLPaymentCode));

        binding.add(AttendancePlanBonusPartialPaidMaxTotalLateCount);
        binding.add(AttendancePlanBonusPartialPaidMaxTotalLateMins);
        binding.add(AttendancePlanBonusPartialPaidMaxTotalEarlyLeaveCount);
        binding.add(AttendancePlanBonusPartialPaidMaxTotalEarlyLeaveMins);
        binding.add(AttendancePlanBonusPartialPaidMaxTotalSLWithMedicalCertificate);
        binding.add(AttendancePlanBonusPartialPaidMaxTotalSLWithoutMedicalCertificate);
        binding.add(AttendancePlanBonusPartialPaidMaxTotalInjuryLeave);
        binding.add(AttendancePlanBonusPartialPaidMaxTotalNonFullPayCasualLeave);
        binding.add(AttendancePlanBonusPartialPaidMaxTotalAbsentCount);
        binding.add(AttendancePlanBonusPartialPaidPercent);

        binding.init(Request, Session);

        HROne.Common.WebUtility.WebControlsLocalization(this, this.Controls);

        if (!int.TryParse(DecryptedRequest["AttendancePlanID"], out CurID))
            CurID = -1;
        AttendancePlan_AdditionalPayment1.CurrentAttendancePlanID = CurID;

    }

    protected void Page_PreRender(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            if (CurID > 0)
            {
                loadObject();
            }
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
        if (obj.AttendancePlanUseBonusAmountByRecurringPayment)
            AttendancePlanUseBonusAmountByRecurringPayment.Visible = true;
        else
            AttendancePlanUseBonusAmountByRecurringPayment.Visible = false;
        if (obj.AttendancePlanProrataBonusforNewJoin)
            AttendancePlanProrataBonusforNewJoin.Visible = true;
        else
            AttendancePlanProrataBonusforNewJoin.Visible = false;

        if (obj.AttendancePlanProrataBonusforTerminated)
            AttendancePlanProrataBonusforTerminated.Visible = true;
        else
            AttendancePlanProrataBonusforTerminated.Visible = false;

        if (obj.AttendancePlanBonusMaxTotalLateCountIncludeLunch)
            AttendancePlanBonusMaxTotalLateCountIncludeLunch.Visible = true;
        else
            AttendancePlanBonusMaxTotalLateCountIncludeLunch.Visible = false;

        if (obj.AttendancePlanBonusMaxTotalLateMinsIncludeLunch)
            AttendancePlanBonusMaxTotalLateMinsIncludeLunch.Visible = true;
        else
            AttendancePlanBonusMaxTotalLateMinsIncludeLunch.Visible = false;

        if (obj.AttendancePlanBonusMaxTotalEarlyLeaveCountIncludeLunch)
            AttendancePlanBonusMaxTotalEarlyLeaveCountIncludeLunch.Visible = true;
        else
            AttendancePlanBonusMaxTotalEarlyLeaveCountIncludeLunch.Visible = false;

        if (obj.AttendancePlanBonusMaxTotalEarlyLeaveMinsIncludeLunch)
            AttendancePlanBonusMaxTotalEarlyLeaveMinsIncludeLunch.Visible = true;
        else
            AttendancePlanBonusMaxTotalEarlyLeaveMinsIncludeLunch.Visible = false;

        if (obj.AttendancePlanBonusPartialPaidMaxTotalLateCountIncludeLunch)
            AttendancePlanBonusPartialPaidMaxTotalLateCountIncludeLunch.Visible = true;
        else
            AttendancePlanBonusPartialPaidMaxTotalLateCountIncludeLunch.Visible = false;

        if (obj.AttendancePlanBonusPartialPaidMaxTotalLateMinsIncludeLunch)
            AttendancePlanBonusPartialPaidMaxTotalLateMinsIncludeLunch.Visible = true;
        else
            AttendancePlanBonusPartialPaidMaxTotalLateMinsIncludeLunch.Visible = false;

        if (obj.AttendancePlanBonusPartialPaidMaxTotalEarlyLeaveCountIncludeLunch)
            AttendancePlanBonusPartialPaidMaxTotalEarlyLeaveCountIncludeLunch.Visible = true;
        else
            AttendancePlanBonusPartialPaidMaxTotalEarlyLeaveCountIncludeLunch.Visible = false;

        if (obj.AttendancePlanBonusPartialPaidMaxTotalEarlyLeaveMinsIncludeLunch)
            AttendancePlanBonusPartialPaidMaxTotalEarlyLeaveMinsIncludeLunch.Visible = true;
        else
            AttendancePlanBonusPartialPaidMaxTotalEarlyLeaveMinsIncludeLunch.Visible = false;

        Hashtable values = new Hashtable();
	    db.populate(obj,values);
	    binding.toControl(values);

        OTFormulaRow.Visible = !obj.AttendancePlanOTGainAsCompensationLeaveEntitle;
        OTPaymentCodeRow.Visible = !obj.AttendancePlanOTGainAsCompensationLeaveEntitle;
        OTRatioRow.Visible = !obj.AttendancePlanOTGainAsCompensationLeaveEntitle;

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
    protected void Edit_Click(object sender, EventArgs e)
    {
        HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "Attendance_Plan_Edit.aspx?AttendancePlanID=" + CurID);
    }
    protected void Back_Click(object sender, EventArgs e)
    {
        HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "Attendance_Plan_List.aspx");
    }

}
