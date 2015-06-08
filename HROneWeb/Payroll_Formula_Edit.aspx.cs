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

public partial class Payroll_Formula_Edit : HROneWebPage
{
    private const string FUNCTION_CODE = "PAY002";

    public Binding binding;
    public DBManager db = EPayrollProrataFormula.db;
    public int CurID = -1;
    

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!WebUtils.CheckAccess(Response, Session, FUNCTION_CODE, WebUtils.AccessLevel.ReadWrite))
            return;
        toolBar.FunctionCode = FUNCTION_CODE;

        if (!int.TryParse(DecryptedRequest["PayFormID"], out CurID))
            CurID = -1;

        DBFilter referencePayrollProrataFormulaFilter = new DBFilter();
        referencePayrollProrataFormulaFilter.add(new Match("PayFormID", "<>", CurID));

        binding = new Binding(dbConn, db);

        binding.add(PayFormID);
        binding.add(PayFormCode);
        binding.add(PayFormDesc);

        binding.add(PayFormMultiplier);
        binding.add(PayFormDivider);
        binding.add(new DropDownVLBinder(db, ReferencePayFormID, EPayrollProrataFormula.VLPayrollProrataFormula, referencePayrollProrataFormulaFilter));
        binding.add(new DropDownVLBinder(db, PayFormRoundingRule, Values.VLRoundingRuleWithNoRounding));
        binding.add(new DropDownVLBinder(db, PayFormDecimalPlace, Values.VL8DecimalPlace));

        binding.init(Request, Session);


        HROne.Common.WebUtility.WebControlsLocalization(this, this.Controls);


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
        EPayrollProrataFormula obj = new EPayrollProrataFormula();
        bool isNew = WebFormWorkers.loadKeys(db, obj, DecryptedRequest);
        if (!db.select(dbConn, obj))
            return false;

        Hashtable values = new Hashtable();
        db.populate(obj, values);
        binding.toControl(values);
        if (obj.PayFormIsSys == "Y")
        {
            toolBar.SaveButton_Visible = false;
            toolBar.DeleteButton_Visible = false;
        }
        if (obj.ReferencePayFormID.Equals(0))
        {
            FormulaType.SelectedValue = "MONTHLYPAYMENT";
            ReferencePayFormID.Visible = false;
            FormulaParameter.Visible = true;
        }
        else
        {
            FormulaType.SelectedValue = "PAYFORMID";
            ReferencePayFormID.Visible = true;
            FormulaParameter.Visible = false;
        }
        return true;
    }
    protected void Save_Click(object sender, EventArgs e)
    {
        EPayrollProrataFormula c = new EPayrollProrataFormula();

        Hashtable values = new Hashtable();
        binding.toValues(values);

        if (PayFormRoundingRule.SelectedValue.Equals(Values.ROUNDING_RULE_NO_ROUND))
            values["PayFormDecimalPlace"] = "9";

        PageErrors errors = PageErrors.getErrors(db, Page.Master);
        errors.clear();


        db.validate(errors, values);

        if (!errors.isEmpty())
            return;


        db.parse(values, c);

        if (!ReferencePayFormID.Visible)
            c.ReferencePayFormID = 0;
        c.PayFormCode = c.PayFormCode.ToUpper();

        if (c.PayFormIsSys!="Y")
            c.PayFormIsSys = "N";

        if (FormulaParameter.Visible)
            if (c.PayFormDivider == 0)
                errors.addError("PayFormDivider", HROne.Translation.PageErrorMessage.ERROR_DIVIDER_NOT_ZERO);

        if (!AppUtils.checkDuplicate(dbConn, db, c, errors, "PayFormCode"))
            return;

        if (!errors.isEmpty())
            return;

        WebUtils.StartFunction(Session, FUNCTION_CODE);
        if (CurID < 0)
        {
            //            Utils.MarkCreate(Session, c);

            db.insert(dbConn, c);
            CurID = c.PayFormID;
            //            url = Utils.BuildURL(-1, CurID);
        }
        else
        {
            //            Utils.Mark(Session, c);
            db.update(dbConn, c);
        }
        WebUtils.EndFunction(dbConn);

        db.select(dbConn, c);

        HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "Payroll_Formula_View.aspx?PayFormID=" + CurID);


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

    protected void Back_Click(object sender, EventArgs e)
    {
        if (CurID > 0)
            HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "Payroll_Formula_View.aspx?PayFormID=" + CurID);
        else
            HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "Payroll_Formula_List.aspx");

    }
    protected void FormulaType_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (FormulaType.SelectedValue.Equals("PAYFORMID"))
        {
            ReferencePayFormID.Visible = true;
            FormulaParameter.Visible = false;
        }
        else
        {
            ReferencePayFormID.Visible = false;
            FormulaParameter.Visible = true;
        }
    }
}
