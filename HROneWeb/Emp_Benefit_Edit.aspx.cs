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
using HROne.Lib.Entities;
using HROne.Translation;

public partial class Emp_Benefit_Edit : HROneWebPage
{
    private const string FUNCTION_CODE = "PER020";
    protected SearchBinding sbinding;
    public Binding binding;
    public DBManager db = EEmpBenefit.db;
    public DBManager empDB= EEmpPersonalInfo.db;
    public EEmpBenefit obj;
    public int CurID = -1;
    public int CurEmpID = -1;
    public Hashtable CurEmpBenefitGroups=new Hashtable();
    public Hashtable CurRanks = new Hashtable();
    

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!WebUtils.CheckAccess(Response, Session, FUNCTION_CODE, WebUtils.AccessLevel.ReadWrite))
            return;
        toolBar.FunctionCode = FUNCTION_CODE;

        binding = new Binding(dbConn, db);
        binding.add(EmpBenefitID);
        binding.add(EmpID);
        binding.add(new TextBoxBinder(db, EmpBenefitEffectiveDate.TextBox, EmpBenefitEffectiveDate.ID));
        binding.add(new TextBoxBinder(db, EmpBenefitExpiryDate.TextBox, EmpBenefitExpiryDate.ID));
        binding.add(new DropDownVLBinder(db, EmpBenefitPlanID, EBenefitPlan.VLBenefitPlan));
        binding.add(EmpBenefitEEPremium);
        binding.add(EmpBenefitERPremium);
        binding.add(EmpBenefitSpousePremium);
        binding.add(EmpBenefitChildPremium);
        
        binding.init(Request, Session);

        if (!int.TryParse(DecryptedRequest["EmpBenefitID"], out CurID))
            CurID = -1;

        if (!int.TryParse(DecryptedRequest["EmpID"], out CurEmpID))
            CurEmpID = -1;

        HROne.Common.WebUtility.WebControlsLocalization(this, this.Controls);

        EmpID.Value = CurEmpID.ToString();
        if (!Page.IsPostBack)
        {
            if (CurID > 0)
            {
                loadObject();
            }
            else
            {
                toolBar.DeleteButton_Visible = false;
            }
        }
    }

    protected bool loadObject() 
    {
	    obj=new EEmpBenefit();
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

	    Hashtable values=new Hashtable();
	    db.populate(obj,values);
	    binding.toControl(values);

        return true;
    }

    protected void Save_Click(object sender, EventArgs e)
    {
        EEmpBenefit c = new EEmpBenefit();

        Hashtable values = new Hashtable();
        binding.toValues(values);

        PageErrors errors = PageErrors.getErrors(db, Page.Master);
        errors.clear();

        db.validate(errors, values);

        if (!errors.isEmpty())
            return;

        db.parse(values, c);

        if (!string.IsNullOrEmpty(EmpBenefitExpiryDate.Value) && c.EmpBenefitEffectiveDate.CompareTo(c.EmpBenefitExpiryDate) > 0)
            errors.addError(HROne.Translation.PageErrorMessage.ERROR_INCORRECT_DATE_RANGE);

        if (!errors.isEmpty())
            return;

        WebUtils.StartFunction(Session, FUNCTION_CODE, c.EmpID);

        if (CurID < 0)
        {
            db.insert(dbConn, c);
            CurID = c.EmpBenefitID;
        }
        else
        {
            db.update(dbConn, c);
        }
        WebUtils.EndFunction(dbConn);

        HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "Emp_Benefit_View.aspx?EmpBenefitID=" + CurID + "&EmpID=" + c.EmpID);

    }

    protected void Delete_Click(object sender, EventArgs e)
    {
        PageErrors errors = PageErrors.getErrors(db, Page.Master);
        errors.clear();

        EEmpBenefit obj = new EEmpBenefit();
        obj.EmpBenefitID = CurID;
        if (EEmpBenefit.db.select(dbConn, obj))
        {
            WebUtils.StartFunction(Session, FUNCTION_CODE, obj.EmpID);
            db.delete(dbConn, obj);
            WebUtils.EndFunction(dbConn);
        }

        HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "EmpTab_Benefit_View.aspx?EmpID=" + EmpID.Value);
    }

    protected void Back_Click(object sender, EventArgs e)
    {
        if (CurID > 0)
            HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "Emp_Benefit_View.aspx?EmpBenefitID=" + EmpBenefitID.Value + "&EmpID=" + EmpID.Value);
        else
            HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "EmpTab_Benefit_View.aspx?EmpID="+EmpID.Value);

    }

    protected void calculateBenefit(object sender, EventArgs e)
    {
        int selBenefitPlanID = -1;
        if (!int.TryParse(EmpBenefitPlanID.SelectedValue, out selBenefitPlanID))
        {
            selBenefitPlanID = -1;
        }
        if (selBenefitPlanID > 0)
        {
            EBenefitPlan benefitPlan = new EBenefitPlan();
            benefitPlan.BenefitPlanID = selBenefitPlanID;
            if (EBenefitPlan.db.select(dbConn, benefitPlan))
            {
                double erPremium = 0;
                double eePremium = 0;
                double spousePremium = 0;
                double childPremium = 0;
                DateTime rangeStartDate = AppUtils.ServerDateTime().Date;
                DateTime rangeEndDate = AppUtils.ServerDateTime().Date;

                if (benefitPlan.BenefitPlanERPaymentBaseMethod.Equals(Values.PAYMENT_BASE_RECURRING_BASIC_SALARY))
                {
                    erPremium = AppUtils.Evaluate(getRPAmount(benefitPlan.BenefitPlanERPaymentCodeID, rangeStartDate, rangeEndDate).ToString("0.00")  + " *  " + benefitPlan.BenefitPlanERMultiplier);
                }
                else if (benefitPlan.BenefitPlanERPaymentBaseMethod.Equals(Values.PAYMENT_BASE_FIXED_AMOUNT))
                {
                    erPremium = benefitPlan.BenefitPlanERAmount;
                }

                if (benefitPlan.BenefitPlanEEPaymentBaseMethod.Equals(Values.PAYMENT_BASE_RECURRING_BASIC_SALARY))
                {
                    eePremium =  AppUtils.Evaluate(getRPAmount(benefitPlan.BenefitPlanEEPaymentCodeID, rangeStartDate, rangeEndDate) + " * " + benefitPlan.BenefitPlanEEMultiplier);
                }
                else if (benefitPlan.BenefitPlanEEPaymentBaseMethod.Equals(Values.PAYMENT_BASE_FIXED_AMOUNT))
                {
                    eePremium = benefitPlan.BenefitPlanEEAmount;
                }

                if (benefitPlan.BenefitPlanSpousePaymentBaseMethod.Equals(Values.PAYMENT_BASE_RECURRING_BASIC_SALARY))
                {
                    spousePremium = AppUtils.Evaluate(getRPAmount(benefitPlan.BenefitPlanSpousePaymentCodeID, rangeStartDate, rangeEndDate) + " * " + benefitPlan.BenefitPlanSpouseMultiplier);
                }
                else if (benefitPlan.BenefitPlanSpousePaymentBaseMethod.Equals(Values.PAYMENT_BASE_FIXED_AMOUNT))
                {
                    spousePremium = benefitPlan.BenefitPlanSpouseAmount;
                }

                if (benefitPlan.BenefitPlanChildPaymentBaseMethod.Equals(Values.PAYMENT_BASE_RECURRING_BASIC_SALARY))
                {
                    childPremium = AppUtils.Evaluate(getRPAmount(benefitPlan.BenefitPlanChildPaymentCodeID, rangeStartDate, rangeEndDate) + " * " + benefitPlan.BenefitPlanChildMultiplier);
                }
                else if (benefitPlan.BenefitPlanChildPaymentBaseMethod.Equals(Values.PAYMENT_BASE_FIXED_AMOUNT))
                {
                    childPremium = benefitPlan.BenefitPlanChildAmount;
                }

                EmpBenefitERPremium.Text = erPremium.ToString("0.00");
                EmpBenefitEEPremium.Text = eePremium.ToString("0.00");
                EmpBenefitSpousePremium.Text = spousePremium.ToString("0.00");
                EmpBenefitChildPremium.Text = childPremium.ToString("0.00");
            }
        }
    }

    private double getRPAmount(int paymentCodeID, DateTime rangeStartDate, DateTime rangeEndDate)
    {
        DBFilter filter = new DBFilter();
        filter.add(new Match("EmpID", CurEmpID));
        filter.add(new Match("PayCodeID", paymentCodeID));

        // only get Recurring Payment Item
        OR orEmpRPIsNonPayrollItem = new OR();
        orEmpRPIsNonPayrollItem.add(new Match("EmpRPIsNonPayrollItem", false));
        orEmpRPIsNonPayrollItem.add(new NullTerm("EmpRPIsNonPayrollItem"));
        filter.add(orEmpRPIsNonPayrollItem);

        AND andEmpRPEffRangeTerm = new AND();
        andEmpRPEffRangeTerm.add(new Match("EmpRPEffFr", "<=", rangeStartDate));
        {
            OR orEmpRpEffToTerm = new OR();
            orEmpRpEffToTerm.add(new Match("EmpRPEffTo", ">=", rangeStartDate));
            orEmpRpEffToTerm.add(new NullTerm("EmpRPEffTo"));
            andEmpRPEffRangeTerm.add(orEmpRpEffToTerm);
        }
        filter.add(andEmpRPEffRangeTerm);

        ArrayList rpList = EEmpRecurringPayment.db.select(dbConn, filter);
        double totalAmount = 0;
        foreach (EEmpRecurringPayment element in rpList)
        {
            totalAmount += element.EmpRPAmount;
        }

        return totalAmount;
    }

}
