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

public partial class Emp_RecurringPayment_Edit_Control : HROneWebControl
{
    private const string FUNCTION_CODE = "PER007-1";
    public Binding binding;
    public DBManager db = EEmpRecurringPayment.db;

    public int CurrentEmpRPID
    {
        get { int CurID = -1; if (int.TryParse(ID.Value, out CurID)) return CurID; else return -1; }
        set { ID.Value = value.ToString(); }
    }

    public int PrevEmpRPID
    {
        get { int CurID = -1; if (int.TryParse(PrevID.Value, out CurID)) return CurID; else return -1; }
        set { PrevID.Value = value.ToString(); }
    }

    public int CurrentEmpID
    {
        get { int PrevCurID = -1; if (int.TryParse(HiddenEmpID.Value, out PrevCurID)) return PrevCurID; else return -1; }
        set { HiddenEmpID.Value = value.ToString(); }
    }

    public int CurrentPayCodeID
    {
        get { int CurID = -1; if (int.TryParse(PayCodeID.SelectedValue, out CurID)) return CurID; else return -1; }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        binding = new Binding(dbConn, db);
        binding.add(new HiddenBinder(db, ID, "EmpRPID"));
        binding.add(new HiddenBinder(db, HiddenEmpID, "EmpID"));
        binding.add(new TextBoxBinder(db, EmpRPEffFr.TextBox, EmpRPEffFr.ID));
        binding.add(new TextBoxBinder(db, EmpRPEffTo.TextBox, EmpRPEffTo.ID));

//        binding.add(new DropDownVLBinder(db, SchemeCode, EPayScale.VLDistinctSchemeList));

        binding.add(EmpRPBasicSalary);
        binding.add(EmpRPFPS);
        binding.add(EmpRPAmount);
        binding.add(new DropDownVLBinder(db, CurrencyID, Values.VLCurrency).setNotSelected(null));
        binding.add(new DropDownVLBinder(db, PayCodeID, EPaymentCode.VLPaymentCode).setNotSelected(null));
        binding.add(new DropDownVLBinder(db, EmpRPUnit, Values.VLPaymentUnit));
        binding.add(new CheckBoxBinder(db, EmpRPUnitPeriodAsDaily));
        binding.add(new DropDownVLBinder(db, EmpRPUnitPeriodAsDailyPayFormID, EPayrollProrataFormula.VLPayrollProrataFormula));
        binding.add(new DropDownVLBinder(db, EmpRPMethod, Values.VLPaymentMethod));
        binding.add(new CheckBoxBinder(db, EmpRPIsNonPayrollItem));

        DBFilter empBankAccountFilter = new DBFilter();
        OR bankAccountORTerm = new OR();
        bankAccountORTerm.add(new Match("EmpID", CurrentEmpID));
        {
            EEmpPersonalInfo tmpEmpInfo = new EEmpPersonalInfo();
            tmpEmpInfo.EmpID = CurrentEmpID;
            if (EEmpPersonalInfo.db.select(dbConn, tmpEmpInfo) && tmpEmpInfo.MasterEmpID > 0)
                bankAccountORTerm.add(new Match("EmpID", tmpEmpInfo.MasterEmpID));
        }
        empBankAccountFilter.add(bankAccountORTerm);
        binding.add(new DropDownVLBinder(db, EmpAccID, EEmpBankAccount.VLBankAccount, empBankAccountFilter).setNotSelected(HROne.Common.WebUtility.GetLocalizedString(EEmpBankAccount.DEFAULT_BANK_ACCOUNT_TEXT)));
        binding.add(new DropDownVLBinder(db, CostCenterID, ECostCenter.VLCostCenter).setNotSelected(HROne.Common.WebUtility.GetLocalizedString(ECostCenter.DEFAULT_COST_CENTER_TEXT)));
        binding.add(EmpRPRemark);

        init_SchemeCodeDropdown();

        binding.init(Request, Session);


        if (ESystemParameter.getParameter(dbConn, ESystemParameter.PARAM_CODE_PAYSCALE_POINT_SYSTEM) == "Y")
        {
            PayscaleRow1.Visible = true;
            PayscaleRow2.Visible = true;
            EmpRPAmount.Enabled = false;
        }

        if (ESystemParameter.getParameter(dbConn, ESystemParameter.PARAM_CODE_MONTHLY_ACHIEVEMENT_COMMISSION) == "Y")
        {
            MonthlyCommissionRow1.Visible = true;
            EmpRPAmount.Enabled = true;
            double m_value;
            if (double.TryParse(EmpRPBasicSalary.Text, out m_value))
            {
                if (m_value > 0)
                {
                    EmpRPAmount.Enabled = false;
                }

                if (double.TryParse(EmpRPFPS.Text, out m_value) && m_value > 0)
                {
                    EmpRPAmount.Enabled = false;
                }
            }
        }

        HROne.Common.WebUtility.WebControlsLocalization(Session, this.Controls);
    }

    protected void init_SchemeCodeDropdown()
    {
        if (SchemeCode.Items.Count <= 0)
        {
            SchemeCode.Items.Add("Not Selected");

            DataTable m_table = AppUtils.runSelectSQL("DISTINCT c.SchemeCode as SchemeCode", "From PayScale c ", null, dbConn);

            foreach (DataRow row in m_table.Rows)
            {
                SchemeCode.Items.Add(row["SchemeCode"].ToString());
            }
        }
    }

    protected void Page_PreRender(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            if (CurrentEmpRPID > 0)
            {
                loadObject();
            }
            else if (PrevEmpRPID > 0)
            {

                loadObject();

            }
            else
            {
                if (DecryptedRequest["NonPayrollItem"] == "Y")
                    EmpRPIsNonPayrollItem.Checked = true;
            }
        }
        CurrencyID.SelectedValue = "HKD";

        if (EmpRPMethod.SelectedValue.Equals("A"))
        {
            BankAccountRow.Visible = true;
            lblDefaultBankAccount.Text = string.Empty;
            if (EmpAccID.SelectedValue.Equals(string.Empty))
            {
                EEmpBankAccount bankAccount = EEmpBankAccount.GetDefaultBankAccount(dbConn, CurrentEmpID);
                if (bankAccount != null)
                {
                    lblDefaultBankAccount.Text = bankAccount.EmpBankCode + "-" + bankAccount.EmpBranchCode + "-" + bankAccount.EmpAccountNo;
                }
            }
        }
        else
            BankAccountRow.Visible = false;

        if (EmpRPUnit.SelectedValue.Equals("P"))
        {
            EmpRPUnitPeriodAsDaily.Visible = true;
            if (EmpRPUnitPeriodAsDaily.Checked)
                EmpRPUnitPeriodAsDailyPayFormIDRow.Visible = true;
            else
                EmpRPUnitPeriodAsDailyPayFormIDRow.Visible = false;
        }
        else
        {
            EmpRPUnitPeriodAsDaily.Visible = false;
            EmpRPUnitPeriodAsDailyPayFormIDRow.Visible = false;
        }

        HROne.ProductLicense productLicense = WebUtils.productLicense(Session);

        CostCenterRow.Visible = productLicense.IsCostCenter;

        if (productLicense.ProductType != HROne.ProductLicense.ProductLicenseType.HROne)
        {
            EmpRPUnit.Items.FindByValue("D").Enabled = false;
            EmpRPUnit.Items.FindByValue("H").Enabled = false;
            EmpRPUnitPeriodAsDaily.Checked = false;
            EmpRPUnitPeriodAsDaily.Visible = false;
            EmpRPUnitPeriodAsDailyPayFormIDRow.Visible = false;
        }
    }

    protected bool loadObject()
    {
        EEmpRecurringPayment obj = new EEmpRecurringPayment();
        bool isNew = WebFormWorkers.loadKeys(db, obj, DecryptedRequest);

        DBFilter filter = new DBFilter();
        filter.add(new Match("EmpID", CurrentEmpID));
        filter.add(WebUtils.AddRankFilter(Session, "EmpID", true));
        if (EEmpPersonalInfo.db.count(dbConn, filter) == 0)
            if (CurrentEmpID <= 0)
                return false;
            else
                HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "~/AccessDeny.aspx");

        if (!db.select(dbConn, obj))
        {
            obj.EmpRPID = PrevEmpRPID;
            if (!db.select(dbConn, obj))
            {
                if (CurrentEmpRPID <= 0)
                    return false;
                else
                    HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "~/AccessDeny.aspx");
            }
            else
            {
                obj.EmpRPID = 0;
                obj.EmpRPEffFr = new DateTime();
                obj.EmpRPEffTo = new DateTime();
            }
        }

        if (obj.EmpID != CurrentEmpID)
            if (CurrentEmpRPID <= 0)
                return false;
            else
                HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "~/AccessDeny.aspx");


        Hashtable values = new Hashtable();
        db.populate(obj, values);
        binding.toControl(values);

        if (obj.SchemeCode != null && obj.SchemeCode != "")
        {
            SchemeCode.Text = obj.SchemeCode;

            RefreshCapacity();
            if (obj.Capacity != null && obj.Capacity != "")
            {
                CapacitySelect.Text = obj.Capacity;

                RefreshPoint();

                if (obj.Point != null && obj.Point > -1)
                {
                    PointSelect.Text = obj.Point.ToString("0.00");
                }
            }
        
        }
        return true;
    }

    public bool Save()
    {
        EEmpRecurringPayment c = new EEmpRecurringPayment();

        Hashtable values = new Hashtable();
        binding.toValues(values);

        PageErrors errors = PageErrors.getErrors(db, Page.Master);
        errors.clear();

        db.validate(errors, values);

        if (!errors.isEmpty())
            return false;

        db.parse(values, c);

        if (c.EmpRPMethod.Equals("A"))
        {
            if (c.EmpAccID == 0)
            {
                EEmpBankAccount bankAccount = EEmpBankAccount.GetDefaultBankAccount(dbConn, c.EmpID);
                if (bankAccount != null)
                {
                    //c.EmpAccID = bankAccount.EmpBankAccountID;
                }
                else
                    errors.addError("EmpAccID", HROne.Translation.PageErrorMessage.ERROR_ACCOUNT_REQUIRED);
            }
        }
        else
            c.EmpAccID = 0;

        AND andTerms = new AND();
        andTerms.add(new Match("EmpRPID", "<>", c.EmpRPID));
        andTerms.add(new Match("EmpRPEffFr", "<", c.EmpRPEffFr));
        andTerms.add(new Match("PayCodeID", c.PayCodeID));

        EEmpRecurringPayment lastObj = (EEmpRecurringPayment)AppUtils.GetLastObj(dbConn, db, "EmpRPEffFr", c.EmpID, andTerms);
        //if (CurID < 0)
        //{
        //    if (lastObj != null && c.EmpRPEffFr <= lastObj.EmpRPEffFr)
        //        errors.addError("EmpRPEffFr", HROne.Translation.SystemMessage.ERROR_DATE_FROM_OVERLAP);
        //}

        DBFilter filter = new DBFilter();
        filter.add(new Match("EmpID", c.EmpID));
        filter.add(new Match("PayCodeID", c.PayCodeID));
        filter.add(new Match("EmpRPID", "<>", c.EmpRPID));
        OR or = new OR();
        AND and;
        and = new AND();
        and.add(new Match("EmpRPEffFr", "<=", c.EmpRPEffFr));
        and.add(new Match("EmpRPEffTo", ">=", c.EmpRPEffFr));
        or.add(and);

        and = new AND();
        and.add(new NullTerm("EmpRPEffTo"));
        and.add(new Match("EmpRPEffFr", "=", c.EmpRPEffFr));
        or.add(and);

        if (c.EmpRPEffTo > DateTime.MinValue)
        {
            and = new AND();
            and.add(new Match("EmpRPEffFr", "<=", c.EmpRPEffTo));
            and.add(new Match("EmpRPEffTo", ">=", c.EmpRPEffTo));
            or.add(and);

            and = new AND();
            and.add(new Match("EmpRPEffFr", ">=", c.EmpRPEffFr));
            and.add(new Match("EmpRPEffFr", "<=", c.EmpRPEffTo));
            or.add(and);
        }

        filter.add(or);
        if (db.count(dbConn, filter) > 0)
            errors.addError(HROne.Translation.PageErrorMessage.ERROR_PAY_EFF_DATE_OVERLAP);
        if (!errors.isEmpty())
            return false;

        c.SchemeCode = null;
        c.Capacity = null;
        c.Point = 0; 

        if (SchemeCode.SelectedIndex > 0)
        {
            c.SchemeCode = SchemeCode.Text;
            c.Capacity = CapacitySelect.Text;

            if (PointSelect.Text != "")
            {
                c.Point = Decimal.Parse(PointSelect.Text);
            }
        }

        WebUtils.StartFunction(Session, FUNCTION_CODE, c.EmpID);
        if (CurrentEmpRPID <= 0)
        {
            //            Utils.MarkCreate(Session, c);

            db.insert(dbConn, c);
            CurrentEmpRPID = c.EmpRPID;
            //            url = Utils.BuildURL(-1, CurID);
            if (lastObj != null && lastObj.EmpRPEffFr < c.EmpRPEffFr)
            {
                if (lastObj.EmpRPEffTo < lastObj.EmpRPEffFr)
                {
                    lastObj.EmpRPEffTo = c.EmpRPEffFr.AddDays(-1);
                    db.update(dbConn, lastObj);
                }
            }
        }
        else
        {
            //            Utils.Mark(Session, c);
            db.update(dbConn, c);
        }
        WebUtils.EndFunction(dbConn);

        return true;
    }

    public bool Delete()
    {
        EEmpRecurringPayment c = new EEmpRecurringPayment();
        c.EmpRPID = CurrentEmpRPID;
        if (db.select(dbConn, c))
        {
            WebUtils.StartFunction(Session, FUNCTION_CODE, c.EmpID);
            db.delete(dbConn, c);
            WebUtils.EndFunction(dbConn);
        }

        return true;
    }

    protected void RecalculateAmountByFPS()
    {
        decimal m_basicSalary;
        decimal m_fps;  // fixed percentage of salary

        if (Decimal.TryParse(EmpRPBasicSalary.Text, out m_basicSalary))
        {
            if (Decimal.TryParse(EmpRPFPS.Text, out m_fps))
            {
                EmpRPAmount.Text = (m_basicSalary * m_fps / System.Convert.ToDecimal(100)).ToString("0.00");
            }
        }
    }

    protected void FPS_Changed(object sender, EventArgs e)
    {
        RecalculateAmountByFPS();
    }



    protected void SchemeCode_Changed(object sender, EventArgs e)
    {
        RefreshCapacity();
    }

    protected void RefreshCapacity()
    {
        CapacitySelect.Items.Clear();
        PointSelect.Items.Clear();

        if (SchemeCode.SelectedIndex > 0)
        {
            DBFilter filter = new DBFilter();

            filter.add(new Match("SchemeCode", AppUtils.Encode(EPayScale.db.getField("SchemeCode"), SchemeCode.SelectedValue)));

            CapacitySelect.Items.Add("Not Selected");

            foreach (EPayScale o in EPayScale.db.select(dbConn, filter))
            {
                CapacitySelect.Items.Add(o.Capacity);
            }
        }
    }

    protected void RefreshPoint()
    {
        PointSelect.Items.Clear();

        if (SchemeCode.SelectedIndex > 0 && CapacitySelect.SelectedIndex > 0)
        {
            DBFilter filter = new DBFilter();
            filter.add(new Match("SchemeCode", AppUtils.Encode(EPayScale.db.getField("SchemeCode"), SchemeCode.SelectedValue)));
            filter.add(new Match("Capacity", AppUtils.Encode(EPayScale.db.getField("Capacity"), CapacitySelect.SelectedValue)));
            ArrayList m_payscaleList = EPayScale.db.select(dbConn, filter);
            if (m_payscaleList.Count > 0)
            {
                EPayScale m_payscale = (EPayScale)m_payscaleList[0];

                DBFilter m_pointsFilter = new DBFilter();
                OR m_OR = new OR();
                m_pointsFilter.add(new Match("SchemeCode", AppUtils.Encode(EPayScaleMap.db.getField("SchemeCode"), m_payscale.SchemeCode)));
                m_pointsFilter.add(new Match("Point", ">=", m_payscale.FirstPoint));
                m_pointsFilter.add(new Match("Point", "<=", m_payscale.LastPoint));

                if (EmpRPEffFr.Value != "")
                {
                    m_pointsFilter.add(new Match("EffectiveDate", "<=", DateTime.Parse(EmpRPEffFr.Value)));
                    m_OR.add(new Match("ExpiryDate", ">=", DateTime.Parse(EmpRPEffFr.Value)));
                    m_OR.add(new NullTerm("ExpiryDate"));
                }
                else
                {
                    m_pointsFilter.add(new Match("EffectiveDate", "<=", AppUtils.ServerDateTime()));
                    m_OR.add(new Match("ExpiryDate", ">=", AppUtils.ServerDateTime()));
                    m_OR.add(new NullTerm("ExpiryDate"));
                }
                m_pointsFilter.add(m_OR);

                foreach (EPayScaleMap m_point in EPayScaleMap.db.select(dbConn, m_pointsFilter))
                {
                    PointSelect.Items.Add(m_point.Point.ToString("0.00"));
                }
            }
        }
    }

    protected void CapacitySelect_Changed(object sender, EventArgs e)
    {
        RefreshPoint();
    }

    protected void PointSelect_Changed(object sender, EventArgs e)
    {
        PageErrors errors = PageErrors.getErrors(db, Page.Master);
        errors.clear();

        if (SchemeCode.SelectedIndex > 0 && CapacitySelect.SelectedIndex > 0 && PointSelect.Text != "")
        {
            DBFilter filter = new DBFilter();
            OR m_OR = new OR();

            filter.add(new Match("SchemeCode", AppUtils.Encode(EPayScaleMap.db.getField("SchemeCode"), SchemeCode.SelectedValue.ToString())));
            filter.add(new Match("Point", Decimal.Parse(PointSelect.Text)));

            if (EmpRPEffFr.Value != "")
            {
                filter.add(new Match("EffectiveDate", "<=", DateTime.Parse(EmpRPEffFr.Value)));
                m_OR.add(new Match("ExpiryDate", ">=", DateTime.Parse(EmpRPEffFr.Value)));
                m_OR.add(new NullTerm("ExpiryDate"));
            }
            else
            {
                filter.add(new Match("EffectiveDate", "<=", AppUtils.ServerDateTime()));
                m_OR.add(new Match("ExpiryDate", ">=", AppUtils.ServerDateTime()));
                m_OR.add(new NullTerm("ExpiryDate"));
            }
            filter.add(m_OR);

            ArrayList m_list = EPayScaleMap.db.select(dbConn, filter);

            if (m_list.Count > 0)
            {
                EmpRPAmount.Text = ((EPayScaleMap)m_list[0]).Salary.ToString("0.00");
            }
            else
            {
                EmpRPAmount.Text = "0.00";
                errors.addError("Failed to retrieve salary");
            }
        }
    }
}
