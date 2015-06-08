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

public partial class Emp_RecurringPayment_View : HROneWebPage
{
    private const string FUNCTION_CODE = "PER007-1";
    private const string CUSTOM003_FUNCTION_CODE = "CUSTOM003";
    public Binding binding;
    public DBManager db = EEmpRecurringPayment.db;
    public EEmpRecurringPayment obj;
    public int CurID = -1;
    public int CurEmpID = -1;
    public int CurPayCodeID = -1;
    public Hashtable CurEmpRecurringPaymentGroups=new Hashtable();
    public Hashtable CurRanks = new Hashtable();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!WebUtils.CheckAccess(Response, Session, FUNCTION_CODE, WebUtils.AccessLevel.Read))
            return;
        toolBar.FunctionCode = FUNCTION_CODE;

        if (!int.TryParse(DecryptedRequest["EmpID"], out CurEmpID))
            CurEmpID = -1;

        binding = new Binding(dbConn, db);
        binding.add(EmpRPID);
        binding.add(EmpID);
        binding.add(EmpRPEffFr);
        binding.add(EmpRPEffTo);

        binding.add(SchemeCode);
        binding.add(Capacity);
        binding.add(Point);

        binding.add(EmpRPBasicSalary);
        binding.add(EmpRPFPS);
        binding.add(EmpRPAmount); 
        
        binding.add(PayCodeID);
        binding.add(new LabelVLBinder(db, PayCodeID, EPaymentCode.VLPaymentCode));
        binding.add(new LabelVLBinder(db, CurrencyID, Values.VLCurrency));
        binding.add(new LabelVLBinder(db, EmpRPUnit, Values.VLPaymentUnit));
        binding.add(new BlankZeroLabelVLBinder(db, EmpRPUnitPeriodAsDailyPayFormID, EPayrollProrataFormula.VLPayrollProrataFormula));
        binding.add(new LabelVLBinder(db, EmpRPMethod, Values.VLPaymentMethod));
        binding.add(new LabelVLBinder(db, EmpRPIsNonPayrollItem, Values.VLTrueFalseYesNo));

        DBFilter empBankAccountFilter = new DBFilter();
        OR bankAccountORTerm = new OR();
        bankAccountORTerm.add(new Match("EmpID", CurEmpID));
        {
            EEmpPersonalInfo tmpEmpInfo = new EEmpPersonalInfo();
            tmpEmpInfo.EmpID = CurEmpID;
            if (EEmpPersonalInfo.db.select(dbConn, tmpEmpInfo) && tmpEmpInfo.MasterEmpID > 0)
                bankAccountORTerm.add(new Match("EmpID", tmpEmpInfo.MasterEmpID));
        }
        empBankAccountFilter.add(bankAccountORTerm);
        binding.add(new BlankZeroLabelVLBinder(db, EmpAccID, EEmpBankAccount.VLBankAccount, empBankAccountFilter).setTextDisplayForZero(HROne.Common.WebUtility.GetLocalizedString(EEmpBankAccount.DEFAULT_BANK_ACCOUNT_TEXT)));
        binding.add(new BlankZeroLabelVLBinder(db, CostCenterID, ECostCenter.VLCostCenter).setTextDisplayForZero(HROne.Common.WebUtility.GetLocalizedString(ECostCenter.DEFAULT_COST_CENTER_TEXT)));
        binding.add(EmpRPRemark);

        binding.init(Request, Session);


        if (!int.TryParse(DecryptedRequest["EmpRPID"], out CurID))
            CurID = -1;

        EmpID.Value = CurEmpID.ToString();

        HROne.Common.WebUtility.WebControlsLocalization(this, this.Controls);

        if (!Page.IsPostBack)
        {
            if (CurID > 0)
                loadObject();
            else
                toolBar.DeleteButton_Visible = false;
        }
        CostCenterRow.Visible = WebUtils.productLicense(Session).IsCostCenter;

//        PayscaleRow1.Visible = (ESystemParameter.getParameter(dbConn, ESystemParameter.PARAM_CODE_PAYSCALE_POINT_SYSTEM) == "Y");
//        PayscaleRow2.Visible = (ESystemParameter.getParameter(dbConn, ESystemParameter.PARAM_CODE_PAYSCALE_POINT_SYSTEM) == "Y");
        PayscaleRow1.Visible = (ESystemParameter.getParameter(dbConn, ESystemParameter.PARAM_CODE_PAYSCALE_POINT_SYSTEM) == "Y");
        PayscaleRow2.Visible = (ESystemParameter.getParameter(dbConn, ESystemParameter.PARAM_CODE_PAYSCALE_POINT_SYSTEM) == "Y");
        MonthlyCommissionRow1.Visible = (ESystemParameter.getParameter(dbConn, ESystemParameter.PARAM_CODE_MONTHLY_ACHIEVEMENT_COMMISSION) == "Y");
        // Start 000159, Ricky so, 2015-01-23
        //NonSalaryBasedComissionBaseRow1.Visible = (ESystemParameter.getParameter(dbConn, ESystemParameter.PARAM_CODE_ENABLE_SALES_TARGET) == "Y");
        // End 000159, Ricky so, 2015-01-23

        //Start 0000168, Ricky So, 2015-05-07
        ESystemFunction m_function = ESystemFunction.GetObjectByCode(dbConn, CUSTOM003_FUNCTION_CODE);
        if (m_function != null && !m_function.FunctionIsHidden && WebUtils.CheckPermission(Session, CUSTOM003_FUNCTION_CODE, WebUtils.AccessLevel.Read))
        {
            winson_header.Visible = true;
            winson_content.Visible = true;
        }
        //End 0000168, Ricky So, 2015-05-07
    }

    protected bool loadObject() 
    {
	    obj=new EEmpRecurringPayment();
	    bool isNew=WebFormWorkers.loadKeys(db, obj, DecryptedRequest);

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

        Emp_RecurringPayment_List1.CurrentEmpID = obj.EmpID;
        Emp_RecurringPayment_List1.CurrentPayCodeID = obj.PayCodeID;

        CurPayCodeID = obj.PayCodeID;

	    Hashtable values=new Hashtable();
	    db.populate(obj,values);
	    binding.toControl(values);

        if (obj.EmpRPMethod.Equals("A"))
        {
            BankAccountRow.Visible = true;
            lblDefaultBankAccount.Text = string.Empty;
            if (obj.EmpAccID == 0)
            {
                EEmpBankAccount bankAccount = EEmpBankAccount.GetDefaultBankAccount(dbConn, CurEmpID);
                if (bankAccount != null)
                {
                    lblDefaultBankAccount.Text = bankAccount.EmpBankCode + "-" + bankAccount.EmpBranchCode + "-" + bankAccount.EmpAccountNo;
                }
            }
        }
        else
            BankAccountRow.Visible = false;

        if (obj.EmpRPUnit.Equals("P"))
        {
            //EmpRPUnitPeriodAsDaily.Visible = true;
            if (obj.EmpRPUnitPeriodAsDaily)
            {
                EmpRPUnitPeriodAsDaily.Visible = true;
                EmpRPUnitPeriodAsDailyPayFormIDRow.Visible = true;
            }
            else
            {
                EmpRPUnitPeriodAsDaily.Visible = false;
                EmpRPUnitPeriodAsDailyPayFormIDRow.Visible = false;
            }
        }
        else
        {
            EmpRPUnitPeriodAsDaily.Visible = false;
            EmpRPUnitPeriodAsDailyPayFormIDRow.Visible = false;
        }
        // Start 0000166, KuangWei, 2015-02-04
        init_ShiftDutyCodeLabel();
        init_PayCalFormulaCodeLabel();
        // End 0000166, KuangWei, 2015-02-04
        return true;
    }

    protected void Delete_Click(object sender, EventArgs e)
    {

        EEmpRecurringPayment c = new EEmpRecurringPayment();
        c.EmpRPID = CurID;
        if (db.select(dbConn, c))
        {

            WebUtils.StartFunction(Session, FUNCTION_CODE, c.EmpID);
            db.delete(dbConn, c);
            WebUtils.EndFunction(dbConn);
        }
        HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "EmpTab_Position_View.aspx?EmpID=" + EmpID.Value);
    }

    protected void Edit_Click(object sender, EventArgs e)
    {
        HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "Emp_RecurringPayment_Edit.aspx?EmpID=" + EmpID.Value + "&EmpRPID=" + EmpRPID.Value);
    }

    protected void New_Click(object sender, EventArgs e)
    {
        HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "Emp_RecurringPayment_Edit.aspx?EmpID=" + EmpID.Value + "&OldEmpRPID=" + EmpRPID.Value);
    }

    protected void Back_Click(object sender, EventArgs e)
    {
        HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "EmpTab_Position_View.aspx?EmpID=" + EmpID.Value);
    }

    // Start 0000166, KuangWei, 2015-02-05
    protected void init_ShiftDutyCodeLabel()
    {
        EEmpRPWinson m_winsonRP = EEmpRPWinson.GetObjectByRPID(dbConn, CurID);

        if (m_winsonRP != null)
        {
            EShiftDutyCode m_shiftDutyCode = EShiftDutyCode.GetObject(dbConn, m_winsonRP.EmpRPShiftDutyID);

            if (m_shiftDutyCode != null)
            {
                ShiftDutyCode.Text = m_shiftDutyCode.ShiftDutyCode + " - " + m_shiftDutyCode.ShiftDutyFromTime.ToString("HH:mm") + " to " + m_shiftDutyCode.ShiftDutyToTime.ToString("HH:mm");
            }
            else
                ShiftDutyCode.Text = "";

        }
    }

    protected void init_PayCalFormulaCodeLabel()
    {
        EEmpRPWinson m_winsonRP = EEmpRPWinson.GetObjectByRPID(dbConn, CurID);

        if (m_winsonRP != null)
        {
            EPaymentCalculationFormula m_calFormula = EPaymentCalculationFormula.GetObject(dbConn, m_winsonRP.EmpRPPayCalFormulaID);

            if (m_calFormula != null)
            {
                PayCalFormulaCode.Text = m_calFormula.PayCalFormulaCode;
            }
            else
                PayCalFormulaCode.Text = "";
        }        
    }
    // End 0000166, KuangWei, 2015-02-05
}
