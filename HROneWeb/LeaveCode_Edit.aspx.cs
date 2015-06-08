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
using HROne.Translation;
using HROne.Lib.Entities;
public partial class LeaveCode_Edit : HROneWebPage
{
    private const string FUNCTION_CODE = "LEV002";

    public Binding binding;
    public DBManager db = ELeaveCode.db;
    public ELeaveCode obj;
    public int CurID = -1;
    

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!WebUtils.CheckAccess(Response, Session, FUNCTION_CODE, WebUtils.AccessLevel.ReadWrite))
            return;
        toolBar.FunctionCode = FUNCTION_CODE;

        DBFilter LeaveTypeFilter = new DBFilter();
        LeaveTypeFilter.add(new Match("LeaveTypeIsDisabled", false));

        binding = new Binding(dbConn, db);
        binding.add(LeaveCodeID);
        binding.add(new DropDownVLBinder(db, LeaveTypeID, ELeaveType.VLLeaveType, LeaveTypeFilter));
        binding.add(LeaveCode);
        binding.add(LeaveCodeDesc);
        binding.add(LeaveCodePayRatio);
        binding.add(new DropDownVLBinder(db, LeaveCodeLeaveAllowPaymentCodeID, EPaymentCode.VLPaymentCode));
        binding.add(new DropDownVLBinder(db, LeaveCodeLeaveDeductPaymentCodeID, EPaymentCode.VLPaymentCode));
        binding.add(new DropDownVLBinder(db, LeaveCodeLeaveDeductFormula, EPayrollProrataFormula.VLPayrollProrataFormula));
        binding.add(new DropDownVLBinder(db, LeaveCodeLeaveAllowFormula, EPayrollProrataFormula.VLPayrollProrataFormula));
        binding.add(new CheckBoxBinder(db, LeaveCodeIsSkipPayrollProcess));
        binding.add(new CheckBoxBinder(db, LeaveCodePayAdvance));
        binding.add(new CheckBoxBinder(db, LeaveCodeIsPayrollProcessNextMonth));
        binding.add(new CheckBoxBinder(db, LeaveCodeIsShowMedicalCertOption));
        binding.add(new CheckBoxBinder(db, LeaveCodeHideInESS));
        binding.add(new CheckBoxBinder(db, LeaveCodeUseAllowancePaymentCodeIfSameAmount));
        binding.add(new CheckBoxBinder(db, LeaveCodeIsCNDProrata));
        binding.add(LeaveAppUnit);
        binding.init(Request, Session);

        if (!int.TryParse(DecryptedRequest["LeaveCodeID"], out CurID))
            CurID = -1;

        HROne.ProductLicense productLicense = WebUtils.productLicense(Session);
        if (productLicense.IsESS)
            ESSRow.Visible = true;
        else
            ESSRow.Visible = false;
        if (productLicense.ProductType != HROne.ProductLicense.ProductLicenseType.HROne)
        {
            LeaveCodeIsCNDProrataRow.Visible = false;
        }
        else
        {
            LeaveCodeIsCNDProrataRow.Visible = true;
        }
        HROne.Common.WebUtility.WebControlsLocalization(this, this.Controls);

		if(!Page.IsPostBack) 
		{
            if (CurID > 0)
            {
                loadObject();
            }
            else
            {
                DBFilter defaultPayrollProrataFormulaFilter = new DBFilter();
                defaultPayrollProrataFormulaFilter.add(new Match("PayFormCode", EPayrollProrataFormula.DEFAULT_FOEMULA_CODE));
                ArrayList defaultPayrollFormulaList = EPayrollProrataFormula.db.select(dbConn, defaultPayrollProrataFormulaFilter);
                if (defaultPayrollFormulaList.Count > 0)
                {
                    int defaultPayrollFormulaID = ((EPayrollProrataFormula)defaultPayrollFormulaList[0]).PayFormID;
                    LeaveCodeLeaveAllowFormula.SelectedValue = defaultPayrollFormulaID.ToString();
                    LeaveCodeLeaveDeductFormula.SelectedValue = defaultPayrollFormulaID.ToString();
                }
                toolBar.DeleteButton_Visible = false;
            }
        }

        PayrollProcessPanel.Visible = !LeaveCodeIsSkipPayrollProcess.Checked;


    }

    protected bool loadObject() 
    {
	    obj=new ELeaveCode ();
	    bool isNew=WebFormWorkers.loadKeys(db, obj, DecryptedRequest);
	    if(!db.select(dbConn, obj))
		    return false;

	    Hashtable values=new Hashtable();
	    db.populate(obj,values);
	    binding.toControl(values);


        return true;
    }

    protected void Save_Click(object sender, EventArgs e)
    {

        PageErrors errors = PageErrors.getErrors(db, Page.Master);
        errors.clear();

        if (LeaveCodeIsSkipPayrollProcess.Checked)
        {
            DBFilter defaultPayrollProrataFormulaFilter = new DBFilter();
            defaultPayrollProrataFormulaFilter.add(new Match("PayFormCode", EPayrollProrataFormula.DEFAULT_FOEMULA_CODE));
            ArrayList defaultPayrollFormulaList = EPayrollProrataFormula.db.select(dbConn, defaultPayrollProrataFormulaFilter);
            if (defaultPayrollFormulaList.Count > 0)
            {
                int defaultPayrollFormulaID = ((EPayrollProrataFormula)defaultPayrollFormulaList[0]).PayFormID;
                LeaveCodeLeaveAllowFormula.SelectedValue = defaultPayrollFormulaID.ToString();
                LeaveCodeLeaveDeductFormula.SelectedValue = defaultPayrollFormulaID.ToString();
                LeaveCodePayRatio.Text = "1";
                LeaveCodePayAdvance.Checked = false;

            }

        }


        Hashtable values = new Hashtable();
        binding.toValues(values);

        db.validate(errors, values);

        if (!errors.isEmpty())
            return;


        ELeaveCode c = new ELeaveCode();
        db.parse(values, c);

        if (!c.LeaveCodeIsSkipPayrollProcess)
        {
            if (c.LeaveCodeLeaveAllowPaymentCodeID.Equals(0))
                errors.addError(lblLeaveAllowPaymentCode.Text, HROne.Common.WebUtility.GetLocalizedString("validate.required.prompt"));
            if (c.LeaveCodeLeaveDeductPaymentCodeID.Equals(0))
                errors.addError(lblLeaveDeductPaymentCode.Text, HROne.Common.WebUtility.GetLocalizedString("validate.required.prompt"));

            if (c.LeaveCodePayRatio.Equals(1))
            {
                EPaymentCode allowancePaymentCode = new EPaymentCode();
                allowancePaymentCode.PaymentCodeID = c.LeaveCodeLeaveAllowPaymentCodeID;
                if (EPaymentCode.db.select(dbConn, allowancePaymentCode))
                {
                    if (!allowancePaymentCode.PaymentCodeIsWages)
                        errors.addError(HROne.Translation.PageErrorMessage.ERROR_LEAVE_ALLOWANCE_SHOULD_BE_WAGES);
                }
                else
                {
                    if (!c.LeaveCodeLeaveAllowPaymentCodeID.Equals(c.LeaveCodeLeaveDeductPaymentCodeID))
                        errors.addError(HROne.Translation.PageErrorMessage.ERROR_LEAVE_ALLOWANCE_SHOULD_BE_WAGES);
                }
            }
            else if (!c.LeaveCodePayRatio.Equals(0))
            {
                EPaymentCode allowancePaymentCode = new EPaymentCode();
                allowancePaymentCode.PaymentCodeID = c.LeaveCodeLeaveAllowPaymentCodeID;
                if (EPaymentCode.db.select(dbConn, allowancePaymentCode))
                {
                    if (allowancePaymentCode.PaymentCodeIsWages)
                        errors.addError(HROne.Translation.PageErrorMessage.ERROR_LEAVE_ALLOWANCE_SHOULD_NOT_BE_WAGES);
                }
                else
                {
                    //if (!c.LeaveCodeLeaveAllowPaymentCodeID.Equals(c.LeaveCodeLeaveDeductPaymentCodeID))
                    errors.addError(HROne.Translation.PageErrorMessage.ERROR_LEAVE_ALLOWANCE_SHOULD_NOT_BE_WAGES);
                }
            }
        }

        if (!errors.isEmpty())
            return;

        WebUtils.StartFunction(Session, FUNCTION_CODE);
        if (CurID < 0)
        {
//            Utils.MarkCreate(Session, c);

            db.insert(dbConn, c);
            CurID = c.LeaveCodeID;
//            url = Utils.BuildURL(-1, CurID);
        }
        else
        {
//            Utils.Mark(Session, c);
            db.update(dbConn, c);
        }
        WebUtils.EndFunction(dbConn);


        HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "LeaveCode_View.aspx?LeaveCodeID="+CurID);


    }

    protected void Delete_Click(object sender, EventArgs e)
    {
        PageErrors errors = PageErrors.getErrors(db, Page.Master);
        errors.clear();

        ELeaveCode obj = new ELeaveCode();
        obj.LeaveCodeID = CurID;
        db.select(dbConn, obj);
        DBFilter leaveApplicationFilter = new DBFilter();
        leaveApplicationFilter.add(new Match("LeaveCodeID", obj.LeaveCodeID));
        leaveApplicationFilter.add("empid", true);
        ArrayList leaveApplicationList = ELeaveApplication.db.select(dbConn, leaveApplicationFilter);
        if (leaveApplicationList.Count > 0)
        {
            int curEmpID = 0;
            errors.addError(string.Format(HROne.Translation.PageErrorMessage.ERROR_CODE_USED_BY_EMPLOYEE, new string[] { HROne.Common.WebUtility.GetLocalizedString("Leave Code"), obj.LeaveCode }));
            foreach (ELeaveApplication leaveApplication in leaveApplicationList)
            {
                EEmpPersonalInfo empInfo = new EEmpPersonalInfo();
                empInfo.EmpID = leaveApplication.EmpID;
                if (EEmpPersonalInfo.db.select(dbConn, empInfo))
                {
                    if (curEmpID != leaveApplication.EmpID)
                    {
                        errors.addError("- " + empInfo.EmpNo + ", " + empInfo.EmpEngFullName);
                        curEmpID = leaveApplication.EmpID;
                    }
                }
                else
                    ELeaveApplication.db.delete(dbConn, leaveApplication);

            }
            errors.addError(HROne.Translation.PageErrorMessage.ERROR_ACTION_ABORT);
            return;
        }
        else
        {

            WebUtils.StartFunction(Session, FUNCTION_CODE);
            db.delete(dbConn, obj);
            WebUtils.EndFunction(dbConn);
        }
        HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "LeaveCode_List.aspx");
    }

    protected void Back_Click(object sender, EventArgs e)
    {
        if (CurID > 0)
            HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "LeaveCode_View.aspx?LeaveCodeID=" + CurID);
        else
            HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "LeaveCode_List.aspx");

    }
}
