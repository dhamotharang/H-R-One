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
using HROne.Translation;

public partial class Payroll_Group_View : HROneWebPage
{
    private const string FUNCTION_CODE = "PAY001";

    public Binding binding;
    public SearchBinding sbinding;
    public DBManager db = EPayrollGroup.db;
    public int CurID = -1;
    public int CurPayPeriodID = -1;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!WebUtils.CheckAccess(Response, Session, FUNCTION_CODE, WebUtils.AccessLevel.Read))
            return;
        toolBar.FunctionCode = FUNCTION_CODE;
        PayPeriodEditPanel.Visible = toolBar.EditButton_Visible;

        toolBar.FunctionCode = FUNCTION_CODE;

        if (!int.TryParse(DecryptedRequest["PayGroupID"], out CurID))
            CurID = -1;

        sbinding = new SearchBinding(dbConn, db);
        sbinding.init(DecryptedRequest, null);

        binding = new Binding(dbConn, db);
        binding.add(PayGroupCode);
        binding.add(PayGroupDesc);
        binding.add(new LabelVLBinder(db, PayGroupFreq, EPayrollGroup.VLPayGroupFreq));
        binding.add(PayGroupDefaultStartDay);
        //binding.add(PayGroupDefaultEndDay);
        binding.add(PayGroupDefaultNextStartDay);
        binding.add(PayGroupLeaveDefaultCutOffDay);
        binding.add(PayGroupLeaveDefaultNextCutOffDay);

        binding.add(PayGroupNextStartDate);
        binding.add(PayGroupNextEndDate);

        binding.add(new BlankZeroLabelVLBinder(db, PayGroupDefaultProrataFormula, EPayrollProrataFormula.VLPayrollProrataFormula));
        binding.add(new LabelVLBinder(db, PayGroupRestDayHasWage, Values.VLTrueFalseYesNo));
        binding.add(new LabelVLBinder(db, PayGroupLunchTimeHasWage, Values.VLTrueFalseYesNo));
        binding.add(new BlankZeroLabelVLBinder(db, PayGroupRestDayProrataFormula, EPayrollProrataFormula.VLPayrollProrataFormula));
        binding.add(new LabelVLBinder(db, PayGroupAdditionalRemunerationPayCodeID, EPaymentCode.VLPaymentCode));

        binding.add(new LabelVLBinder(db, PayGroupIsCNDProrata, Values.VLTrueFalseYesNo));

        //binding.add(new LabelVLBinder(db, PayGroupLeaveAllowFormula, EPayrollProrataFormula.VLPayrollProrataFormula));
        //binding.add(new LabelVLBinder(db, PayGroupLeaveDeductFormula, EPayrollProrataFormula.VLPayrollProrataFormula));
        binding.add(new BlankZeroLabelVLBinder(db, PayGroupStatHolAllowFormula, EPayrollProrataFormula.VLPayrollProrataFormula));
        binding.add(new BlankZeroLabelVLBinder(db, PayGroupStatHolDeductFormula, EPayrollProrataFormula.VLPayrollProrataFormula));
        binding.add(new LabelVLBinder(db, PayGroupStatHolNextMonth, Values.VLTrueFalseYesNo));
        binding.add(new LabelVLBinder(db, PayGroupIsStatHolUsePublicHoliday, Values.VLTrueFalseYesNo));

        binding.add(new BlankZeroLabelVLBinder(db, PayGroupNewJoinFormula, EPayrollProrataFormula.VLPayrollProrataFormula));
        binding.add(new BlankZeroLabelVLBinder(db, PayGroupExistingFormula, EPayrollProrataFormula.VLPayrollProrataFormula));
        binding.add(new BlankZeroLabelVLBinder(db, PayGroupTerminatedFormula, EPayrollProrataFormula.VLPayrollProrataFormula));
        binding.add(new BlankZeroLabelVLBinder(db, PayGroupTerminatedALCompensationDailyFormula, EPayrollProrataFormula.VLPayrollProrataFormula));
        binding.add(new BlankZeroLabelVLBinder(db, PayGroupTerminatedALCompensationDailyFormulaAlternative, EPayrollProrataFormula.VLPayrollProrataFormula));
        binding.add(new CheckBoxBinder(db, PayGroupTerminatedALCompensationIsSkipRoundingRule));
        binding.add(new BlankZeroLabelVLBinder(db, PayGroupTerminatedALCompensationPaymentCodeID, EPaymentCode.VLPaymentCode));
        binding.add(new BlankZeroLabelVLBinder(db, PayGroupTerminatedALCompensationByEEPaymentCodeID, EPaymentCode.VLPaymentCode));

        binding.add(new LabelVLBinder(db, PayGroupTerminatedPaymentInLieuMonthlyBaseMethod, Values.VLPaymentBaseMethod));
        binding.add(new LabelVLBinder(db, PayGroupTerminatedPaymentInLieuMonthlyBaseMethodAlternative, Values.VLPaymentBaseMethod));
        binding.add(new BlankZeroLabelVLBinder(db, PayGroupTerminatedPaymentInLieuDailyFormula, EPayrollProrataFormula.VLPayrollProrataFormula));
        binding.add(new BlankZeroLabelVLBinder(db, PayGroupTerminatedPaymentInLieuDailyFormulaAlternative, EPayrollProrataFormula.VLPayrollProrataFormula));
        binding.add(new BlankZeroLabelVLBinder(db, PayGroupTerminatedPaymentInLieuERPaymentCodeID, EPaymentCode.VLPaymentCode));
        binding.add(new BlankZeroLabelVLBinder(db, PayGroupTerminatedPaymentInLieuEEPaymentCodeID, EPaymentCode.VLPaymentCode));
        binding.add(new LabelVLBinder(db, PayGroupTerminatedLSPSPMonthlyBaseMethod, Values.VLPaymentBaseMethod));
        binding.add(new LabelVLBinder(db, PayGroupTerminatedLSPSPMonthlyBaseMethodAlternative, Values.VLPaymentBaseMethod));
        binding.add(new BlankZeroLabelVLBinder(db, PayGroupTerminatedLSPPaymentCodeID, EPaymentCode.VLPaymentCode));
        binding.add(new BlankZeroLabelVLBinder(db, PayGroupTerminatedSPPaymentCodeID, EPaymentCode.VLPaymentCode));

        binding.add(new BlankZeroLabelVLBinder(db, PayGroupTerminatedRestDayCompensationDailyFormula, EPayrollProrataFormula.VLPayrollProrataFormula));
        binding.add(new BlankZeroLabelVLBinder(db, PayGroupTerminatedRestDayCompensationPaymentCodeID, EPaymentCode.VLPaymentCode));
        binding.add(new BlankZeroLabelVLBinder(db, PayGroupTerminatedStatutoryHolidayCompensationDailyFormula, EPayrollProrataFormula.VLPayrollProrataFormula));
        binding.add(new BlankZeroLabelVLBinder(db, PayGroupTerminatedStatutoryHolidayCompensationPaymentCodeID, EPaymentCode.VLPaymentCode));

        binding.add(PayGroupTerminatedALCompensationProrataEligiblePeriod);
        binding.add(new LabelVLBinder(db, PayGroupTerminatedALCompensationProrataEligibleUnit, Values.VLEmpUnit));
        binding.add(new CheckBoxBinder(db, PayGroupTerminatedALCompensationProrataEligibleCheckEveryLeaveYear));
        binding.add(new CheckBoxBinder(db, PayGroupTerminatedALCompensationEligibleAfterProbation));

        binding.add(new CheckBoxBinder(db, PayGroupPayAdvance));
        binding.add(new CheckBoxBinder(db, PayGroupPayAdvanceCompareTotalPaymentOnly));

        binding.add(new LabelVLBinder(db, PayGroupUseCNDForDailyHourlyPayment, Values.VLTrueFalseYesNo));
        binding.add(new LabelVLBinder(db, PayGroupIsSkipStatHol, Values.VLTrueFalseYesNo));
        binding.add(PayGroupStatHolEligiblePeriod);
        binding.add(new LabelVLBinder(db, PayGroupStatHolEligibleUnit, Values.VLEmpUnit));
        binding.add(new BlankZeroLabelVLBinder(db, PayGroupStatHolDeductPaymentCodeID, EPaymentCode.VLPaymentCode));
        binding.add(new BlankZeroLabelVLBinder(db, PayGroupStatHolAllowPaymentCodeID, EPaymentCode.VLPaymentCode));
        binding.add(new CheckBoxBinder(db, PayGroupStatHolEligibleAfterProbation));
        binding.add(new CheckBoxBinder(db, PayGroupStatHolEligibleSkipDeduction));
        binding.add(new LabelVLBinder(db, PayGroupYEBStartPayrollMonth, Values.VLMonth));
        binding.add(new LabelVLBinder(db, PayGroupYEBMonthFrom, Values.VLMonth));
        binding.add(new LabelVLBinder(db, PayGroupYEBMonthTo, Values.VLMonth));

        DBFilter payPeriodFilter = new DBFilter();
        payPeriodFilter.add(new Match("PayPeriodStatus", "<>", "E"));
        payPeriodFilter.add(new Match("PayGroupID", CurID));

        binding.add(new DropDownVLBinder(EPayrollPeriod.db, PayPeriodID, EPayrollPeriod.VLPayrollPeriod, payPeriodFilter));

        binding.init(Request, Session);

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

        PayGroupID.Value = CurID.ToString();
        //if (!string.IsNullOrEmpty(DecryptedRequest["PayPeriodID"]))
        //    CurPayPeriodID = Int32.Parse(DecryptedRequest["PayPeriodID"]);
        //else 
        if (!int.TryParse(PayPeriodID.SelectedValue, out CurPayPeriodID))
        {
            EPayrollGroup obj = new EPayrollGroup();
            obj.PayGroupID = CurID;
            if (db.select(dbConn, obj))
                CurPayPeriodID = obj.CurrentPayPeriodID;
        }

        if (!Page.IsPostBack)
        {
            if (CurID > 0)
                loadObject();
            else
                toolBar.DeleteButton_Visible = false;
            LoadLeaveCalculationOverrideList(LeaveCodeOverrideListFooter.ListInfo, LeaveCodeOverrideRepeater);
            // Start 0000069, Ricky So, 2014-08-08
            LoadUserList(SecurityRepeater);
            // End 0000069, Ricky So, 2014-08-08
        }
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
        if (CurPayPeriodID <= 0)
        {
            CurPayPeriodID = obj.CurrentPayPeriodID;
        }
        PayPeriodID.SelectedValue = CurPayPeriodID.ToString();
        ucPayroll_PeriodInfo.CurrentPayPeriodID = CurPayPeriodID;

        if (CurPayPeriodID > 0)
        {
            EPayrollPeriod payPeriod = new EPayrollPeriod();
            payPeriod.PayPeriodID = CurPayPeriodID;
            if (EPayrollPeriod.db.select(dbConn, payPeriod))
            {
                if (payPeriod.PayPeriodStatus.Equals("N"))
                    PayPeriodEdit.Visible = true;
                else
                    PayPeriodEdit.Visible = false;

            }
        }
        StatutoryHolidayOptionSection.Visible = !obj.PayGroupIsSkipStatHol;
        if (string.IsNullOrEmpty(obj.PayGroupFreq) || obj.PayGroupFreq.Equals("M", StringComparison.CurrentCultureIgnoreCase))
            SemiMonthlyDayOptionsRow.Visible = false;
        else
            SemiMonthlyDayOptionsRow.Visible = true;
        return true;
    }

    // Start 0000069, Ricky So, 2014-08-08
    public DataView LoadUserList(Repeater repeater)
    {
        DBFilter m_filter = sbinding.createFilter(); ;
        m_filter.add(new Match("pgu.PayGroupID", CurID));
        m_filter.add(new MatchField("pgu.UserID", "u.UserID"));
        m_filter.add("u.UserName", true);

        string m_select = "u.userName ";
        string m_from = "FROM PayrollGroupUsers pgu,Users u ";

        DataTable m_table = WebUtils.GetDataTableFromSelectQueryWithFilter(dbConn, m_select, m_from, m_filter, null);
        DataView m_view = new DataView(m_table);

        if (repeater != null)
        {
            repeater.DataSource = m_view;
            repeater.DataBind();
        }

        return m_view;

    }
    // End 0000069, Ricky So, 2014-08-08

    public DataView LoadLeaveCalculationOverrideList(ListInfo info, Repeater repeater)
    {
        DBFilter filter = new DBFilter();
        OR orLeaveCodeIsSkipPayrollProcessTerms = new OR();
        filter.add(orLeaveCodeIsSkipPayrollProcessTerms);
        orLeaveCodeIsSkipPayrollProcessTerms.add(new Match("lc.LeaveCodeIsSkipPayrollProcess", false));
        orLeaveCodeIsSkipPayrollProcessTerms.add(new NullTerm("lc.LeaveCodeIsSkipPayrollProcess"));
        filter.add(new MatchField("lc.LeaveCodeID", "pglc.LeaveCodeID"));
        filter.add(new Match("pglc.PayGroupID", CurID));

        if (info != null && info.orderby != null && !info.orderby.Equals(""))
            filter.add(info.orderby, info.order);

        string select = "lc.LeaveCode, pglc.*";
        string from = "from " + ELeaveCode.db.dbclass.tableName + " lc, " + EPayrollGroupLeaveCodeSetupOverride.db.dbclass.tableName + " pglc";

        DataTable table = WebUtils.GetDataTableFromSelectQueryWithFilter(dbConn, select, from, filter, info);


        DataView view = new DataView(table);
        LeaveCodeOverrideListFooter.Refresh();
        if (table.Rows.Count > 0)
            LeaveCodeOverrideHeaderRow.Visible = true;
        else
            LeaveCodeOverrideHeaderRow.Visible = false;
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
            Label PayrollGroupLeaveCodeSetupLeaveDeductFormula = (Label)e.Item.FindControl("PayrollGroupLeaveCodeSetupLeaveDeductFormula");
            Label PayrollGroupLeaveCodeSetupLeaveDeductPaymentCodeID = (Label)e.Item.FindControl("PayrollGroupLeaveCodeSetupLeaveDeductPaymentCodeID");
            Label PayrollGroupLeaveCodeSetupLeaveAllowFormula = (Label)e.Item.FindControl("PayrollGroupLeaveCodeSetupLeaveAllowFormula");
            Label PayrollGroupLeaveCodeSetupLeaveAllowPaymentCodeID = (Label)e.Item.FindControl("PayrollGroupLeaveCodeSetupLeaveAllowPaymentCodeID");
            WebFormUtils.LoadKeys(ELeaveCode.db, row, LeaveCodeID);

            Binding ebinding = new Binding(dbConn, EPayrollGroupLeaveCodeSetupOverride.db);
            ebinding.add(new LabelVLBinder(EPayrollGroupLeaveCodeSetupOverride.db, LeaveCodeID, ELeaveCode.VLLeaveCode));
            ebinding.add(new BlankZeroLabelVLBinder(EPayrollGroupLeaveCodeSetupOverride.db, PayrollGroupLeaveCodeSetupLeaveDeductFormula, EPayrollProrataFormula.VLPayrollProrataFormula));
            ebinding.add(new BlankZeroLabelVLBinder(EPayrollGroupLeaveCodeSetupOverride.db, PayrollGroupLeaveCodeSetupLeaveDeductPaymentCodeID, EPaymentCode.VLPaymentCode));
            ebinding.add(new BlankZeroLabelVLBinder(EPayrollGroupLeaveCodeSetupOverride.db, PayrollGroupLeaveCodeSetupLeaveAllowFormula, EPayrollProrataFormula.VLPayrollProrataFormula));
            ebinding.add(new BlankZeroLabelVLBinder(EPayrollGroupLeaveCodeSetupOverride.db, PayrollGroupLeaveCodeSetupLeaveAllowPaymentCodeID, EPaymentCode.VLPaymentCode));
            ebinding.init(Request, Session);

            ELeaveCode leaveCode = new ELeaveCode();
            ELeaveCode.db.toObject(row.Row, leaveCode);
            //LeaveCodeID.Text = string.Format("{0}-{1}", new string[] { leaveCode.LeaveCode, leaveCode.LeaveCodeDesc });

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

    protected void PayPeriodID_SelectedIndexChanged(object sender, EventArgs e)
    {
        ucPayroll_PeriodInfo.CurrentPayPeriodID = CurPayPeriodID;
        PayPeriodID.SelectedValue = CurPayPeriodID.ToString();
        if (CurPayPeriodID > 0)
        {
            EPayrollPeriod payPeriod = new EPayrollPeriod();
            payPeriod.PayPeriodID = CurPayPeriodID;
            if (EPayrollPeriod.db.select(dbConn, payPeriod))
            {
                if (payPeriod.PayPeriodStatus.Equals("N"))
                    PayPeriodEdit.Visible = true;
                else
                    PayPeriodEdit.Visible = false;

            }
        }
        //Response.Redirect(Request.Url.LocalPath + "?PayGroupID=" + DecryptedRequest["PayGroupID"] + "&PayPeriodID=" + PayPeriodID.SelectedValue);
    }
    protected void Copy_Click(object sender, EventArgs e)
    {
        int new_CurID=-1;
        EPayrollGroup o = new EPayrollGroup();
        o.PayGroupID = CurID;
        if (EPayrollGroup.db.select(dbConn, o))
        {
            EPayrollGroup newPayrollGroup = o.Copy(dbConn);
            EPayrollPeriod oldCurrentPayrollPeriod = new EPayrollPeriod();
            oldCurrentPayrollPeriod.PayPeriodID = o.CurrentPayPeriodID;
            if (EPayrollPeriod.db.select(dbConn, oldCurrentPayrollPeriod))
            {
                EPayrollPeriod newPayPeriod = oldCurrentPayrollPeriod.Copy(dbConn, newPayrollGroup.PayGroupID);
                newPayrollGroup.CurrentPayPeriodID = newPayPeriod.PayPeriodID;
                EPayrollGroup.db.update(dbConn, newPayrollGroup);
                new_CurID = newPayrollGroup.PayGroupID;
            }

            // Start 0000069, Ricky So, 2014-08-08
            DBFilter m_filter = new DBFilter();
            m_filter.add(new Match("PayGroupID", CurID));
            foreach (EPayrollGroupUsers users in EPayrollGroupUsers.db.select(dbConn, m_filter))
            {
                EPayrollGroupUsers new_users = new EPayrollGroupUsers();
                new_users.PayGroupID = new_CurID;
                new_users.UserID = users.UserID;

                EPayrollGroupUsers.db.insert(dbConn, new_users);
            }
            // End 0000069, Ricky So, 2014-08-08

            HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "Payroll_Group_View.aspx?PayGroupID=" + newPayrollGroup.PayGroupID);
        }


    }
    protected void Edit_Click(object sender, EventArgs e)
    {
        HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "Payroll_Group_Edit.aspx?PayGroupID=" + CurID);
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
        HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "Payroll_Group_List.aspx");
    }
    protected void PayPeriodEdit_Click(object sender, EventArgs e)
    {
        HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "Payroll_Period_Edit.aspx?PayPeriodID=" + PayPeriodID.SelectedValue);
    }
    protected void PayPeriodNew_Click(object sender, EventArgs e)
    {
        HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "Payroll_Period_Edit.aspx?PayGroupID=" + PayGroupID.Value);
    }

    // Start 0000069, Ricky So, 2014-08-08
    protected void SecurityRepeater_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        DataRowView row = (DataRowView)e.Item.DataItem;
//        CheckBox cb = (CheckBox)e.Item.FindControl("UserSelect");
//        WebFormUtils.LoadKeys(db, row, cb);

        Label m_userName = (Label)e.Item.FindControl("UserName");
        if (m_userName != null)
        {
            m_userName.Text = row["UserName"].ToString();
        }
    }
    // End 0000069, Ricky So, 2014-08-08

    // Start 0000069, Ricky So, 2014-08-08
    protected void UsersEdit_Click(object sender, EventArgs e)
    {
        // Start 0000069, KuangWei, 2014-09-03
        HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "Payroll_Group_User_Edit.aspx?PayGroupID=" + CurID);
        // End 0000069, KuangWei, 2014-09-03
    }
    // End 0000069, Ricky So, 2014-08-08
}
