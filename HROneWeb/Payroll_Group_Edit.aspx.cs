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

public partial class Payroll_Group_Edit : HROneWebPage
{
    private const string FUNCTION_CODE = "PAY001";
    public Binding binding;
    public DBManager db = EPayrollGroup.db;
    public int CurID = -1;
    


    protected void Page_Load(object sender, EventArgs e)
    {
        if (!WebUtils.CheckAccess(Response, Session, FUNCTION_CODE, WebUtils.AccessLevel.ReadWrite))
            return;
        toolBar.FunctionCode = FUNCTION_CODE;

        DBFilter LSPSPPaymentCodeFilter = new DBFilter();
        LSPSPPaymentCodeFilter.add(new Match("PaymentTypeID", EPaymentType.SystemPaymentType.LongServicePaymentSeverancePaymentType(dbConn).PaymentTypeID));

        binding = new Binding(dbConn, db);
        binding.add(PayGroupID);
        binding.add(PayGroupCode);
        binding.add(PayGroupDesc);
        //binding.add(new DropDownVLBinder(db, PayGroupFreq, Values.VLGender).setNotSelected(null));
        binding.add(new DropDownVLBinder(db, PayGroupFreq, EPayrollGroup.VLPayGroupFreq ));

        binding.add(PayGroupDefaultStartDay);
        //binding.add(PayGroupDefaultEndDay);
        binding.add(PayGroupDefaultNextStartDay);
        binding.add(PayGroupLeaveDefaultCutOffDay);
        binding.add(PayGroupLeaveDefaultNextCutOffDay);

        binding.add(new TextBoxBinder(db, PayGroupNextStartDate.TextBox, PayGroupNextStartDate.ID));
        binding.add(new TextBoxBinder(db, PayGroupNextEndDate.TextBox, PayGroupNextEndDate.ID));

        DBFilter excludeDefaultPayrollProrataFormulaFilter = new DBFilter();
        excludeDefaultPayrollProrataFormulaFilter.add(new Match("PayFormCode", "<>", EPayrollProrataFormula.DEFAULT_FOEMULA_CODE));
        binding.add(new DropDownVLBinder(db, PayGroupDefaultProrataFormula, EPayrollProrataFormula.VLPayrollProrataFormula, excludeDefaultPayrollProrataFormulaFilter));
        binding.add(new CheckBoxBinder(db, PayGroupRestDayHasWage));
        binding.add(new CheckBoxBinder(db, PayGroupLunchTimeHasWage));        
        binding.add(new DropDownVLBinder(db, PayGroupRestDayProrataFormula, EPayrollProrataFormula.VLPayrollProrataFormula));
        binding.add(new CheckBoxBinder(db, PayGroupIsCNDProrata));

        DBFilter paymentCodeIsWageFilter = new DBFilter();
        paymentCodeIsWageFilter.add(new Match("PaymentCodeIsWages", true));
        binding.add(new DropDownVLBinder(db, PayGroupAdditionalRemunerationPayCodeID, EPaymentCode.VLPaymentCode, paymentCodeIsWageFilter));
        //binding.add(new DropDownVLBinder(db, PayGroupLeaveAllowFormula, EPayrollProrataFormula.VLPayrollProrataFormula));
        //binding.add(new DropDownVLBinder(db, PayGroupLeaveDeductFormula, EPayrollProrataFormula.VLPayrollProrataFormula));
        binding.add(new CheckBoxBinder(db, PayGroupIsSkipStatHol));
        binding.add(new DropDownVLBinder(db, PayGroupStatHolAllowFormula, EPayrollProrataFormula.VLPayrollProrataFormula));
        binding.add(new DropDownVLBinder(db, PayGroupStatHolDeductFormula, EPayrollProrataFormula.VLPayrollProrataFormula));
        binding.add(new CheckBoxBinder(db, PayGroupStatHolNextMonth));
        binding.add(new CheckBoxBinder(db, PayGroupIsStatHolUsePublicHoliday));
        
        binding.add(new DropDownVLBinder(db, PayGroupNewJoinFormula, EPayrollProrataFormula.VLPayrollProrataFormula));
        binding.add(new DropDownVLBinder(db, PayGroupExistingFormula, EPayrollProrataFormula.VLPayrollProrataFormula));
        binding.add(new DropDownVLBinder(db, PayGroupTerminatedFormula, EPayrollProrataFormula.VLPayrollProrataFormula));

        binding.add(PayGroupTerminatedALCompensationProrataEligiblePeriod);
        binding.add(new DropDownVLBinder(db, PayGroupTerminatedALCompensationProrataEligibleUnit, Values.VLEmpUnit));
        binding.add(new CheckBoxBinder(db, PayGroupTerminatedALCompensationProrataEligibleCheckEveryLeaveYear));
        binding.add(new CheckBoxBinder(db, PayGroupTerminatedALCompensationEligibleAfterProbation));

        binding.add(new DropDownVLBinder(db, PayGroupTerminatedALCompensationDailyFormula, EPayrollProrataFormula.VLPayrollProrataFormula));
        binding.add(new DropDownVLBinder(db, PayGroupTerminatedALCompensationDailyFormulaAlternative, EPayrollProrataFormula.VLPayrollProrataFormula));
        
        binding.add(new CheckBoxBinder(db, PayGroupTerminatedALCompensationIsSkipRoundingRule));
        binding.add(new DropDownVLBinder(db, PayGroupTerminatedALCompensationPaymentCodeID, EPaymentCode.VLPaymentCode ));
        binding.add(new DropDownVLBinder(db, PayGroupTerminatedALCompensationByEEPaymentCodeID, EPaymentCode.VLPaymentCode));
        
        binding.add(new DropDownVLBinder(db, PayGroupTerminatedPaymentInLieuMonthlyBaseMethod, Values.VLPaymentBaseMethod));
        binding.add(new DropDownVLBinder(db, PayGroupTerminatedPaymentInLieuMonthlyBaseMethodAlternative, Values.VLPaymentBaseMethod));
        binding.add(new DropDownVLBinder(db, PayGroupTerminatedPaymentInLieuDailyFormula, EPayrollProrataFormula.VLPayrollProrataFormula));
        binding.add(new DropDownVLBinder(db, PayGroupTerminatedPaymentInLieuDailyFormulaAlternative, EPayrollProrataFormula.VLPayrollProrataFormula));
        binding.add(new DropDownVLBinder(db, PayGroupTerminatedPaymentInLieuERPaymentCodeID, EPaymentCode.VLPaymentCode));
        binding.add(new DropDownVLBinder(db, PayGroupTerminatedPaymentInLieuEEPaymentCodeID , EPaymentCode.VLPaymentCode));
        binding.add(new DropDownVLBinder(db, PayGroupTerminatedLSPSPMonthlyBaseMethod, Values.VLPaymentBaseMethod));
        binding.add(new DropDownVLBinder(db, PayGroupTerminatedLSPSPMonthlyBaseMethodAlternative, Values.VLPaymentBaseMethod));
        binding.add(new DropDownVLBinder(db, PayGroupTerminatedLSPPaymentCodeID, EPaymentCode.VLPaymentCode, LSPSPPaymentCodeFilter));
        binding.add(new DropDownVLBinder(db, PayGroupTerminatedSPPaymentCodeID, EPaymentCode.VLPaymentCode, LSPSPPaymentCodeFilter));

        binding.add(new DropDownVLBinder(db, PayGroupTerminatedRestDayCompensationDailyFormula, EPayrollProrataFormula.VLPayrollProrataFormula));
        binding.add(new DropDownVLBinder(db, PayGroupTerminatedRestDayCompensationPaymentCodeID, EPaymentCode.VLPaymentCode));
        binding.add(new DropDownVLBinder(db, PayGroupTerminatedStatutoryHolidayCompensationDailyFormula, EPayrollProrataFormula.VLPayrollProrataFormula));
        binding.add(new DropDownVLBinder(db, PayGroupTerminatedStatutoryHolidayCompensationPaymentCodeID, EPaymentCode.VLPaymentCode));

        binding.add(new CheckBoxBinder(db, PayGroupPayAdvance));
        binding.add(new CheckBoxBinder(db, PayGroupPayAdvanceCompareTotalPaymentOnly));
        binding.add(new CheckBoxBinder(db, PayGroupUseCNDForDailyHourlyPayment));
        binding.add(PayGroupStatHolEligiblePeriod);
        binding.add(new DropDownVLBinder(db, PayGroupStatHolEligibleUnit, Values.VLEmpUnit));
        binding.add(new DropDownVLBinder(db, PayGroupStatHolAllowPaymentCodeID, EPaymentCode.VLPaymentCode));
        binding.add(new DropDownVLBinder(db, PayGroupStatHolDeductPaymentCodeID, EPaymentCode.VLPaymentCode));
        binding.add(new CheckBoxBinder(db, PayGroupStatHolEligibleAfterProbation));
        binding.add(new CheckBoxBinder(db, PayGroupStatHolEligibleSkipDeduction));
        binding.add(new DropDownVLBinder(db, PayGroupYEBStartPayrollMonth, Values.VLMonth));
        binding.add(new DropDownVLBinder(db, PayGroupYEBMonthFrom, Values.VLMonth));
        binding.add(new DropDownVLBinder(db, PayGroupYEBMonthTo, Values.VLMonth));
        binding.init(Request, Session);

        if (!int.TryParse(DecryptedRequest["PayGroupID"], out CurID))
            CurID = -1;

        HROne.ProductLicense productLicense = WebUtils.productLicense(Session);
        if (productLicense.ProductType != HROne.ProductLicense.ProductLicenseType.HROne)
        {
            FinalPaymentHolidayPanel.Visible = false;
            PayGroupIsCNDProrataRow.Visible = false;
            PayGroupStatHolNextMonthRow.Visible = false;
            PayGroupPayAdvanceCompareTotalPaymentOnly.Visible = false;
            PayGroupUseCNDForDailyHourlyPaymentSection.Visible = false;
        }
        else
        {
            FinalPaymentHolidayPanel.Visible = true;
            PayGroupIsCNDProrataRow.Visible = true;
            PayGroupStatHolNextMonthRow.Visible = true;
            PayGroupPayAdvanceCompareTotalPaymentOnly.Visible = true;
            PayGroupUseCNDForDailyHourlyPaymentSection.Visible = true;
        }

        HROne.Common.WebUtility.WebControlsLocalization(this, this.Controls);

        if (!Page.IsPostBack)
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
                    PayGroupNewJoinFormula.SelectedValue = defaultPayrollFormulaID.ToString();
                    PayGroupStatHolAllowFormula.SelectedValue = defaultPayrollFormulaID.ToString();
                    PayGroupStatHolDeductFormula.SelectedValue = defaultPayrollFormulaID.ToString();
                    PayGroupTerminatedALCompensationDailyFormula.SelectedValue = defaultPayrollFormulaID.ToString();
                    PayGroupTerminatedFormula.SelectedValue = defaultPayrollFormulaID.ToString();
                    PayGroupTerminatedPaymentInLieuDailyFormula.SelectedValue = defaultPayrollFormulaID.ToString();
                    PayGroupTerminatedRestDayCompensationDailyFormula.SelectedValue = defaultPayrollFormulaID.ToString();
                    PayGroupTerminatedStatutoryHolidayCompensationDailyFormula.SelectedValue = defaultPayrollFormulaID.ToString();
                }
                toolBar.DeleteButton_Visible = false;
            }
            LoadLeaveCalculationOverrideList(LeaveCodeOverrideListFooter.ListInfo, LeaveCodeOverrideRepeater);
        }

    }
    protected void Page_PreRender(object sender, EventArgs e)
    {
        StatutoryHolidayOptionSection.Visible = !PayGroupIsSkipStatHol.Checked;
        if (string.IsNullOrEmpty(PayGroupFreq.SelectedValue) || PayGroupFreq.SelectedValue.Equals("M", StringComparison.CurrentCultureIgnoreCase))
            SemiMonthlyDayOptionsRow.Visible = false;
        else
            SemiMonthlyDayOptionsRow.Visible = true;
    }
    protected bool loadObject()
    {
        EPayrollGroup obj = new EPayrollGroup();
        bool isNew = WebFormWorkers.loadKeys(db, obj, DecryptedRequest);
        if (!db.select(dbConn, obj))
            return false;

        Hashtable values = new Hashtable();
        db.populate(obj, values);
        binding.toControl(values);
        return true;
    }

    public DataView LoadLeaveCalculationOverrideList(ListInfo info, Repeater repeater)
    {
        DBFilter filter = new DBFilter();
        OR orLeaveCodeIsSkipPayrollProcessTerms = new OR();
        filter.add(orLeaveCodeIsSkipPayrollProcessTerms);
        orLeaveCodeIsSkipPayrollProcessTerms.add(new Match("lc.LeaveCodeIsSkipPayrollProcess", false));
        orLeaveCodeIsSkipPayrollProcessTerms.add(new NullTerm("lc.LeaveCodeIsSkipPayrollProcess"));

        if (info != null && info.orderby != null && !info.orderby.Equals(""))
            filter.add(info.orderby, info.order);

        string select = "lc.*";
        string from = "from " + ELeaveCode.db.dbclass.tableName + " lc ";

        DataTable table = WebUtils.GetDataTableFromSelectQueryWithFilter(dbConn, select, from, filter, info);


        DataView view = new DataView(table);
        LeaveCodeOverrideListFooter.Refresh();

        if (repeater != null)
        {
            repeater.DataSource = view;
            repeater.DataBind();
        }

        return view;
    }
    protected void LeaveCodeOverrideRepeater_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        {
            DataRowView row = (DataRowView)e.Item.DataItem;

            Label LeaveCodeID = (Label)e.Item.FindControl("LeaveCodeID");
            DropDownList PayrollGroupLeaveCodeSetupLeaveDeductFormula = (DropDownList)e.Item.FindControl("PayrollGroupLeaveCodeSetupLeaveDeductFormula");
            DropDownList PayrollGroupLeaveCodeSetupLeaveDeductPaymentCodeID = (DropDownList)e.Item.FindControl("PayrollGroupLeaveCodeSetupLeaveDeductPaymentCodeID");
            DropDownList PayrollGroupLeaveCodeSetupLeaveAllowFormula = (DropDownList)e.Item.FindControl("PayrollGroupLeaveCodeSetupLeaveAllowFormula");
            DropDownList PayrollGroupLeaveCodeSetupLeaveAllowPaymentCodeID = (DropDownList)e.Item.FindControl("PayrollGroupLeaveCodeSetupLeaveAllowPaymentCodeID");
            WebFormUtils.LoadKeys(ELeaveCode.db, row, LeaveCodeID);

            Binding ebinding = new Binding(dbConn, EPayrollGroupLeaveCodeSetupOverride.db);
            //ebinding.add(new LabelVLBinder(EPayrollGroupLeaveCodeSetupOverride.db, LeaveCodeID, ELeaveCode.VLLeaveCode));
            ebinding.add(new DropDownVLBinder(EPayrollGroupLeaveCodeSetupOverride.db, PayrollGroupLeaveCodeSetupLeaveDeductFormula, EPayrollProrataFormula.VLPayrollProrataFormula));
            ebinding.add(new DropDownVLBinder(EPayrollGroupLeaveCodeSetupOverride.db, PayrollGroupLeaveCodeSetupLeaveDeductPaymentCodeID, EPaymentCode.VLPaymentCode));
            ebinding.add(new DropDownVLBinder(EPayrollGroupLeaveCodeSetupOverride.db, PayrollGroupLeaveCodeSetupLeaveAllowFormula, EPayrollProrataFormula.VLPayrollProrataFormula));
            ebinding.add(new DropDownVLBinder(EPayrollGroupLeaveCodeSetupOverride.db, PayrollGroupLeaveCodeSetupLeaveAllowPaymentCodeID, EPaymentCode.VLPaymentCode));
            ebinding.init(Request, Session);

            ELeaveCode leaveCode = new ELeaveCode();
            ELeaveCode.db.toObject(row.Row, leaveCode);
            LeaveCodeID.Text = string.Format("{0}-{1}", new string[] { leaveCode.LeaveCode, leaveCode.LeaveCodeDesc });

            DBFilter payrollGroupLeaveCodeSetupOverrideFilter = new DBFilter();
            payrollGroupLeaveCodeSetupOverrideFilter.add(new Match("PayGroupID", CurID));
            payrollGroupLeaveCodeSetupOverrideFilter.add(new Match("LeaveCodeID", leaveCode.LeaveCodeID));
            ArrayList overrideList = EPayrollGroupLeaveCodeSetupOverride.db.select(dbConn, payrollGroupLeaveCodeSetupOverrideFilter);
            if (overrideList.Count > 0)
            {
                EPayrollGroupLeaveCodeSetupOverride setupOverride = (EPayrollGroupLeaveCodeSetupOverride)overrideList[0];
                Hashtable hashTable = new Hashtable();
                EPayrollGroupLeaveCodeSetupOverride.db.populate(setupOverride, hashTable);
                ebinding.toControl(hashTable);
            }
        }
    }

    protected void Save_Click(object sender, EventArgs e)
    {
        EPayrollGroup c = new EPayrollGroup();

        Hashtable values = new Hashtable();
        binding.toValues(values);

        PageErrors errors = PageErrors.getErrors(db, Page.Master);
        errors.clear();


        db.validate(errors, values);

        if (!errors.isEmpty())
            return;


        db.parse(values, c);

        if (CurID < 0)
            c.PayGroupIsPublic = true;  // new payroll group is default to public 

        if (!c.PayGroupIsSkipStatHol)
        {
            if (c.PayGroupStatHolAllowFormula <= 0)
                errors.addError(lblPayGroupStatHolAllowFormula.Text, HROne.Common.WebUtility.GetLocalizedString("validate.required.prompt"));
            if (c.PayGroupStatHolAllowPaymentCodeID <= 0)
                errors.addError(lblPayGroupStatHolAllowPaymentCodeID.Text, HROne.Common.WebUtility.GetLocalizedString("validate.required.prompt"));
            if (c.PayGroupStatHolDeductFormula <= 0)
                errors.addError(lblPayGroupStatHolDeductFormula.Text, HROne.Common.WebUtility.GetLocalizedString("validate.required.prompt"));
            if (c.PayGroupStatHolDeductPaymentCodeID <= 0)
                errors.addError(lblPayGroupStatHolDeductPaymentCodeID.Text, HROne.Common.WebUtility.GetLocalizedString("validate.required.prompt"));
        }
        c.PayGroupCode = c.PayGroupCode.ToUpper();
        if (!AppUtils.checkDuplicate(dbConn, db, c, errors, "PayGroupCode"))
            return;

        if (c.PayGroupNextEndDate < c.PayGroupNextStartDate)
            errors.addError(lblPayGroupNextEndDate.Text, HROne.Translation.PageErrorMessage.ERROR_DATE_TO_TOO_EARLY);
        if (c.PayGroupDefaultStartDay > 31 || c.PayGroupDefaultStartDay < 1)
            errors.addError(lblPayGroupDefaultStartDay.Text, HROne.Translation.PageErrorMessage.ERROR_INVALID_START_DATE);
        if (c.PayGroupLeaveDefaultCutOffDay > 31 || c.PayGroupLeaveDefaultCutOffDay < 1)
            errors.addError(lblPayGroupLeaveDefaultCutOffDay.Text, HROne.Translation.PageErrorMessage.ERROR_INVALID_END_DATE);

        if (c.PayGroupFreq.Equals("S", StringComparison.CurrentCultureIgnoreCase))
        {
            if (c.PayGroupDefaultNextStartDay > 31 || c.PayGroupDefaultNextStartDay < 1)
                errors.addError(lblPayGroupDefaultNextStartDay.Text, HROne.Translation.PageErrorMessage.ERROR_INVALID_START_DATE);
            if (c.PayGroupLeaveDefaultNextCutOffDay > 31 || c.PayGroupLeaveDefaultNextCutOffDay < 1)
                errors.addError(lblPayGroupLeaveDefaultNextCutOffDay.Text, HROne.Translation.PageErrorMessage.ERROR_INVALID_END_DATE);
        }

        if (c.PayGroupNextStartDate >= c.PayGroupNextEndDate)
            errors.addError("PayGroupNextStartDate", HROne.Translation.PageErrorMessage.ERROR_INCORRECT_DATE_RANGE + ": " + lblPayGroupNextEndDate.Text);
        else if (((TimeSpan)c.PayGroupNextEndDate.Subtract(c.PayGroupNextStartDate)).TotalDays > 31)
            errors.addError("PayGroupNextStartDate", HROne.Translation.PageErrorMessage.ERROR_INCORRECT_DATE_RANGE + ": " + lblPayGroupNextEndDate.Text);


        if (!errors.isEmpty())
            return;

//        WebUtils.StartFunction(Session, Functions.FUNC_EMP_EDIT , 0);
        WebUtils.StartFunction(Session, FUNCTION_CODE);
        if (CurID < 0)
        {
            //            Utils.MarkCreate(Session, c);

            db.insert(dbConn, c);
            CurID = c.PayGroupID;
            //            url = Utils.BuildURL(-1, CurID);
        }
        else
        {
            //            Utils.Mark(Session, c);
            db.update(dbConn, c);
        }
//        WebUtils.StartFunction(Session, Functions.FUNC_EMP_EDIT, 0);

        db.select(dbConn, c);
        if (c.CurrentPayPeriodID == 0)
        {
            HROne.Payroll.PayrollProcess payrollProcess = new HROne.Payroll.PayrollProcess(dbConn);
            payrollProcess.GenerateNextPayrollPeriod(c.PayGroupID);
        }

        foreach (RepeaterItem item in LeaveCodeOverrideRepeater.Items)
        {
            Label LeaveCodeID = (Label)item.FindControl("LeaveCodeID");
            DropDownList PayrollGroupLeaveCodeSetupLeaveDeductFormula = (DropDownList)item.FindControl("PayrollGroupLeaveCodeSetupLeaveDeductFormula");
            DropDownList PayrollGroupLeaveCodeSetupLeaveDeductPaymentCodeID = (DropDownList)item.FindControl("PayrollGroupLeaveCodeSetupLeaveDeductPaymentCodeID");
            DropDownList PayrollGroupLeaveCodeSetupLeaveAllowFormula = (DropDownList)item.FindControl("PayrollGroupLeaveCodeSetupLeaveAllowFormula");
            DropDownList PayrollGroupLeaveCodeSetupLeaveAllowPaymentCodeID = (DropDownList)item.FindControl("PayrollGroupLeaveCodeSetupLeaveAllowPaymentCodeID");

            ELeaveCode o = new ELeaveCode();
            WebFormUtils.GetKeys(ELeaveCode.db, o, LeaveCodeID);

            Binding ebinding = new Binding(dbConn, EPayrollGroupLeaveCodeSetupOverride.db);
            ebinding.add(new DropDownVLBinder(EPayrollGroupLeaveCodeSetupOverride.db, PayrollGroupLeaveCodeSetupLeaveDeductFormula, EPayrollProrataFormula.VLPayrollProrataFormula));
            ebinding.add(new DropDownVLBinder(EPayrollGroupLeaveCodeSetupOverride.db, PayrollGroupLeaveCodeSetupLeaveDeductPaymentCodeID, EPaymentCode.VLPaymentCode));
            ebinding.add(new DropDownVLBinder(EPayrollGroupLeaveCodeSetupOverride.db, PayrollGroupLeaveCodeSetupLeaveAllowFormula, EPayrollProrataFormula.VLPayrollProrataFormula));
            ebinding.add(new DropDownVLBinder(EPayrollGroupLeaveCodeSetupOverride.db, PayrollGroupLeaveCodeSetupLeaveAllowPaymentCodeID, EPaymentCode.VLPaymentCode));

            Hashtable hashTable = new Hashtable();
            ebinding.toValues(hashTable);
            EPayrollGroupLeaveCodeSetupOverride setupOverride = new EPayrollGroupLeaveCodeSetupOverride();
            EPayrollGroupLeaveCodeSetupOverride.db.parse(hashTable, setupOverride);
            setupOverride.PayGroupID = c.PayGroupID;
            setupOverride.LeaveCodeID = o.LeaveCodeID;
            if (setupOverride.PayrollGroupLeaveCodeSetupLeaveAllowFormula <= 0
                && setupOverride.PayrollGroupLeaveCodeSetupLeaveAllowPaymentCodeID <= 0
                && setupOverride.PayrollGroupLeaveCodeSetupLeaveDeductFormula <= 0
                && setupOverride.PayrollGroupLeaveCodeSetupLeaveDeductPaymentCodeID <= 0)
            {
                DBFilter setupOverrideFilter = new DBFilter();
                setupOverrideFilter.add(new Match("LeaveCodeID", setupOverride.LeaveCodeID));
                setupOverrideFilter.add(new Match("PayGroupID", setupOverride.PayGroupID));
                ArrayList setupOverrideList = EPayrollGroupLeaveCodeSetupOverride.db.select(dbConn, setupOverrideFilter);
                foreach (EPayrollGroupLeaveCodeSetupOverride deletedSetupOverride in setupOverrideList)
                    EPayrollGroupLeaveCodeSetupOverride.db.delete(dbConn, deletedSetupOverride);
            }
            else
            {
                DBFilter setupOverrideFilter = new DBFilter();
                setupOverrideFilter.add(new Match("LeaveCodeID", setupOverride.LeaveCodeID));
                setupOverrideFilter.add(new Match("PayGroupID", setupOverride.PayGroupID));
                ArrayList setupOverrideList = EPayrollGroupLeaveCodeSetupOverride.db.select(dbConn, setupOverrideFilter);
                if (setupOverrideList.Count > 0)
                {
                    setupOverride.PayrollGroupLeaveCodeSetupOverrideID = ((EPayrollGroupLeaveCodeSetupOverride)setupOverrideList[0]).PayrollGroupLeaveCodeSetupOverrideID;
                    EPayrollGroupLeaveCodeSetupOverride.db.update(dbConn, setupOverride);
                }
                else
                {
                    EPayrollGroupLeaveCodeSetupOverride.db.insert(dbConn, setupOverride);
                }
            }

        }
        WebUtils.EndFunction(dbConn);

//        Response.Redirect(Request.Url.LocalPath + "?PayGroupID=" + CurID);
        HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "Payroll_Group_View.aspx?PayGroupID=" + CurID);


    }
    protected void Delete_Click(object sender, EventArgs e)
    {
        EPayrollGroup o = new EPayrollGroup();
        o.PayGroupID = CurID;
        if (EPayrollGroup.db.select(dbConn, o))
        {
            PageErrors errors = PageErrors.getErrors(db, Page.Master);
            errors.clear();

            DBFilter payPeriodFilter = new DBFilter();
            payPeriodFilter.add(new Match("PayGroupID", o.PayGroupID));
            DBFilter empPayrollFilter = new DBFilter();

            empPayrollFilter.add(new IN("PayPeriodID", "Select PayPeriodID from PayrollPeriod", payPeriodFilter));

            ArrayList empPayrollList = EEmpPayroll.db.select(dbConn, empPayrollFilter);
            if (empPayrollList.Count == 0)
            {
                DBFilter empPosFilter = new DBFilter();
                empPosFilter.add(new Match("PayGroupID", o.PayGroupID));
                empPosFilter.add("empid", true);
                ArrayList empPosList = EEmpPositionInfo.db.select(dbConn, empPosFilter);
                if (empPosList.Count > 0)
                {
                    errors.addError(string.Format(HROne.Translation.PageErrorMessage.ERROR_CODE_USED_BY_EMPLOYEE, new string[] { HROne.Common.WebUtility.GetLocalizedString("Payroll Code"), o.PayGroupCode }));

                    //errors.addError("Payroll Code '" + o.PayGroupCode + "' is mapped by the following employee: ");
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

                }
                else
                {
                    EPayrollPeriod.db.delete(dbConn, payPeriodFilter);
                    WebUtils.StartFunction(Session, FUNCTION_CODE);
                    EPayrollGroup.db.delete(dbConn, o);
                    WebUtils.EndFunction(dbConn);
                }
            }
            else
            {
                EPayrollGroup.db.select(dbConn, o);
                errors.addError("Payroll Code '" + o.PayGroupCode + "' is in use. Action abort!");

            }

            if (!errors.isEmpty())
                return;
        }
        HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "Payroll_Group_List.aspx");
    }
    protected void Back_Click(object sender, EventArgs e)
    {
        if (CurID > 0)
            HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "Payroll_Group_View.aspx?PayGroupID=" + CurID);
        else
            HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "Payroll_Group_List.aspx");

    }
}
