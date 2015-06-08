using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using HROne.DataAccess;
//using perspectivemind.validation;
using HROne.Lib.Entities;

public partial class Payroll_Formula_View : HROneWebPage
{
    private const string FUNCTION_CODE = "PAY002";

    public Binding binding;
    public DBManager db = EPayrollProrataFormula.db;
    public int CurID = -1;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!WebUtils.CheckAccess(Response, Session, FUNCTION_CODE, WebUtils.AccessLevel.Read))
            return;
        toolBar.FunctionCode = FUNCTION_CODE;

        binding = new Binding(dbConn, db);
        binding.add(PayFormID);
        binding.add(PayFormCode);
        binding.add(PayFormDesc);
        //binding.add(new LabelVLBinder(db, PayFormPaymentType, EPaymentType.VLPaymentType));
        binding.add(PayFormMultiplier);
        binding.add(PayFormDivider);
        binding.add(PayFormIsSys);
        binding.add(new LabelVLBinder(db, ReferencePayFormID, EPayrollProrataFormula.VLPayrollProrataFormula));
        binding.add(new LabelVLBinder(db, PayFormRoundingRule, Values.VLRoundingRuleWithNoRounding));
        binding.add(new LabelVLBinder(db, PayFormDecimalPlace, Values.VL8DecimalPlace));

        binding.init(Request, Session);

        if (!int.TryParse(DecryptedRequest["PayFormID"], out CurID))
            CurID = -1;

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
        EPayrollProrataFormula obj = new EPayrollProrataFormula();
        bool isNew = WebFormWorkers.loadKeys(db, obj, DecryptedRequest);
        if (!db.select(dbConn, obj))
            return false;

        Hashtable values = new Hashtable();
        db.populate(obj, values);
        binding.toControl(values);
        bool IsSysFormula = obj.PayFormIsSys.Equals("Y");
        toolBar.EditButton_Visible = !IsSysFormula;
        PaymentIsSystemDefault.Visible = IsSysFormula;
        PaymentIsNotSystemDefault.Visible = !IsSysFormula;
        toolBar.DeleteButton_Visible = !IsSysFormula;
        if (obj.ReferencePayFormID.Equals(0))
        {
            UseMonthlyPaymentLabel.Visible = true;
            UseExistingFormulaLabel.Visible = false;
            ReferencePayFormID.Visible = false;
            FormulaParameter.Visible = true;
        }
        else
        {
            UseMonthlyPaymentLabel.Visible = false;
            UseExistingFormulaLabel.Visible = true;
            ReferencePayFormID.Visible = true;
            FormulaParameter.Visible = false;
        }
        if (string.IsNullOrEmpty(obj.PayFormRoundingRule))
            PayFormDecimalPlace.Visible = false;
        else
            if (obj.PayFormRoundingRule.Equals(Values.ROUNDING_RULE_NO_ROUND))
            {
                PayFormDecimalPlace.Visible = false;
                PayFormDecimalPlaceDesc.Visible = false;
            }
            else
            {
                PayFormDecimalPlace.Visible = true;
                PayFormDecimalPlaceDesc.Visible = true;
            }

        return true;
    }

    protected void Edit_Click(object sender, EventArgs e)
    {
        HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "Payroll_Formula_Edit.aspx?PayFormID=" + CurID);
    }

    protected void Back_Click(object sender, EventArgs e)
    {
        HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "Payroll_Formula_List.aspx");
    }

    protected void Delete_Click(object sender, EventArgs e)
    {

        EPayrollProrataFormula c = new EPayrollProrataFormula();
        c.PayFormID = CurID;
        if (EPayrollProrataFormula.db.select(dbConn, c))
        {
            PageErrors errors = PageErrors.getErrors(db, Page.Master);
            errors.clear();

            if (IsInUsed(c, errors))
                return;

            WebUtils.StartFunction(Session, FUNCTION_CODE);
            db.delete(dbConn, c);
            WebUtils.EndFunction(dbConn);
        }
        HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "Payroll_Formula_List.aspx");
    }

    protected bool IsInUsed(EPayrollProrataFormula o, PageErrors errors)
    {
        int PayrollProrataFormulaID = o.PayFormID;
        DBFilter leaveFormulaFilter = new DBFilter();
        OR orLeaveFormula = new OR();
        orLeaveFormula.add(new Match("LeaveCodeLeaveAllowFormula", PayrollProrataFormulaID));
        orLeaveFormula.add(new Match("LeaveCodeLeaveDeductFormula", PayrollProrataFormulaID));
        leaveFormulaFilter.add(orLeaveFormula);
        ArrayList leaveCodeList = ELeaveCode.db.select(dbConn, leaveFormulaFilter);
        if (leaveCodeList.Count > 0)
        {
            errors.addError(string.Format(HROne.Translation.PageErrorMessage.ERROR_CODE_IS_IN_USE, new string[] { o.PayFormCode }));
            errors.addError(HROne.Translation.PageErrorMessage.ERROR_ACTION_ABORT);
            return true;
        }

        DBFilter payGroupFormulaFilter = new DBFilter();
        OR orPayGroupFormula = new OR();
        orPayGroupFormula.add(new Match("PayGroupDefaultProrataFormula", PayrollProrataFormulaID));
        orPayGroupFormula.add(new Match("PayGroupNewJoinFormula", PayrollProrataFormulaID));
        orPayGroupFormula.add(new Match("PayGroupStatHolAllowFormula", PayrollProrataFormulaID));
        orPayGroupFormula.add(new Match("PayGroupStatHolDeductFormula", PayrollProrataFormulaID));
        orPayGroupFormula.add(new Match("PayGroupTerminatedALCompensationDailyFormula", PayrollProrataFormulaID));
        orPayGroupFormula.add(new Match("PayGroupTerminatedFormula", PayrollProrataFormulaID));
        orPayGroupFormula.add(new Match("PayGroupTerminatedPaymentInLieuDailyFormula", PayrollProrataFormulaID));

        payGroupFormulaFilter.add(orPayGroupFormula);
        ArrayList payGroupList = EPayrollGroup.db.select(dbConn, payGroupFormulaFilter);
        if (payGroupList.Count > 0)
        {
            errors.addError(string.Format(HROne.Translation.PageErrorMessage.ERROR_CODE_IS_IN_USE, new string[] { o.PayFormCode }));
            errors.addError(HROne.Translation.PageErrorMessage.ERROR_ACTION_ABORT);
            return true;
        }

        DBFilter attnFormulaFormulaFilter = new DBFilter();
        OR orAttnFormulaFormula = new OR();
        orAttnFormulaFormula.add(new Match("AttendanceFormulaPayFormID", PayrollProrataFormulaID));

        attnFormulaFormulaFilter.add(orAttnFormulaFormula);
        ArrayList attnFormulaList = EAttendanceFormula.db.select(dbConn, attnFormulaFormulaFilter);
        if (attnFormulaList.Count > 0)
        {
            errors.addError(string.Format(HROne.Translation.PageErrorMessage.ERROR_CODE_IS_IN_USE, new string[] { o.PayFormCode }));
            errors.addError(HROne.Translation.PageErrorMessage.ERROR_ACTION_ABORT);
            return true;
        }
        return false;
    }
}
