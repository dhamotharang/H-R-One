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


public partial class BenefitPlan_Edit : HROneWebPage
{
    private const string FUNCTION_CODE = "SYS022";

    public Binding binding;
    public DBManager db = EBenefitPlan.db;
    public int CurID = -1;
    

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!WebUtils.CheckAccess(Response, Session, FUNCTION_CODE, WebUtils.AccessLevel.ReadWrite))
            return;
        toolBar.FunctionCode = FUNCTION_CODE;

        binding = new Binding(dbConn, db);
        binding.add(BenefitPlanID);

        binding.add(BenefitPlanCode);
        binding.add(BenefitPlanDesc);

        // need Basic Salary Filter
        DBFilter PaymentCodeFilter = new DBFilter();
        PaymentCodeFilter.add(new Match("PaymentTypeID", EPaymentType.SystemPaymentType.BasicSalaryPaymentType(dbConn).PaymentTypeID));

        binding.add(new DropDownVLBinder(db, BenefitPlanERPaymentBaseMethod, EBenefitPlan.VLPaymentBaseMethod));
        binding.add(new DropDownVLBinder(db, BenefitPlanERPaymentCodeID, EPaymentCode.VLPaymentCode, PaymentCodeFilter));
        binding.add(BenefitPlanERMultiplier);
        binding.add(BenefitPlanERAmount);
        binding.add(new DropDownVLBinder(db, BenefitPlanEEPaymentBaseMethod, EBenefitPlan.VLPaymentBaseMethod));
        binding.add(new DropDownVLBinder(db, BenefitPlanEEPaymentCodeID, EPaymentCode.VLPaymentCode, PaymentCodeFilter));
        binding.add(BenefitPlanEEMultiplier);
        binding.add(BenefitPlanEEAmount);
        binding.add(new DropDownVLBinder(db, BenefitPlanSpousePaymentBaseMethod, EBenefitPlan.VLPaymentBaseMethod));
        binding.add(new DropDownVLBinder(db, BenefitPlanSpousePaymentCodeID, EPaymentCode.VLPaymentCode, PaymentCodeFilter));
        binding.add(BenefitPlanSpouseMultiplier);
        binding.add(BenefitPlanSpouseAmount);
        binding.add(new DropDownVLBinder(db, BenefitPlanChildPaymentBaseMethod, EBenefitPlan.VLPaymentBaseMethod));
        binding.add(new DropDownVLBinder(db, BenefitPlanChildPaymentCodeID, EPaymentCode.VLPaymentCode, PaymentCodeFilter));
        binding.add(BenefitPlanChildMultiplier);
        binding.add(BenefitPlanChildAmount);

        binding.init(Request, Session);

        if (!int.TryParse(DecryptedRequest["BenefitPlanID"], out CurID))
            CurID = -1;

        HROne.Common.WebUtility.WebControlsLocalization(this, this.Controls);

        if (!Page.IsPostBack)
        {
            if (CurID > 0)
                loadObject(sender, e);
            else
                toolBar.DeleteButton_Visible = false;
        }
    }
    protected bool loadObject(object sender, EventArgs e)
    {
        EBenefitPlan obj = new EBenefitPlan();
        bool isNew = WebFormWorkers.loadKeys(db, obj, DecryptedRequest);
        if (!db.select(dbConn, obj))
            return false;

        Hashtable values = new Hashtable();
        db.populate(obj, values);
        binding.toControl(values);

        BenefitPlanPaymentBaseMethod_SelectedIndexChanged(sender, e);

        return true;
    }

    protected void Save_Click(object sender, EventArgs e)
    {
        EBenefitPlan c = new EBenefitPlan();

        Hashtable values = new Hashtable();
        binding.toValues(values);

        PageErrors errors = PageErrors.getErrors(db, Page.Master);
        errors.clear();

        // For Employer Premium
        if (spanBenefitPlanERRP.Visible)
        {
            if (BenefitPlanERPaymentCodeID.SelectedValue.Equals(""))
                errors.addError("Payment Code for Employer Premium is required");
            if (AppUtils.Evaluate("10000 * " + BenefitPlanERMultiplier.Text) <= 0)
                errors.addError("Invaid formula for Employer Preminu");
        }
        else if (spanBenefitPlanERFA.Visible)
        {
            if (BenefitPlanERAmount.Text.Equals(""))
                errors.addError("Fixed Amount for Employer Preminu is required");
        }
        // For Employee Premium
        if (spanBenefitPlanEERP.Visible)
        {
            if (BenefitPlanEEPaymentCodeID.SelectedValue.Equals(""))
                errors.addError("Payment Code for Employee Premium is required");
            if (AppUtils.Evaluate("10000 * " + BenefitPlanEEMultiplier.Text) <= 0)
                errors.addError("Invaid formula for Employee Preminu");
        }
        else if (spanBenefitPlanEEFA.Visible)
        {
            if (BenefitPlanEEAmount.Text.Equals(""))
                errors.addError("Fixed Amount for Employee Preminu is required");
        }
        // For Spouse Premium
        if (spanBenefitPlanSpouseRP.Visible)
        {
            if (BenefitPlanSpousePaymentCodeID.SelectedValue.Equals(""))
                errors.addError("Payment Code for Spouse Premium is required");
            if (AppUtils.Evaluate("10000 * " + BenefitPlanSpouseMultiplier.Text) <= 0)
               errors.addError("Invaid formula for Spouse Preminu");
        }
        else if (spanBenefitPlanSpouseFA.Visible)
        {
            if (BenefitPlanSpouseAmount.Text.Equals(""))
                errors.addError("Fixed Amount for Spouse Preminu is required");
        }
        // For Child Premium
        if (spanBenefitPlanChildRP.Visible)
        {
            if (BenefitPlanChildPaymentCodeID.SelectedValue.Equals(""))
                errors.addError("Payment Code for Child Premium is required");
            if (AppUtils.Evaluate("10000 * " + BenefitPlanChildMultiplier.Text) <= 0)
                errors.addError("Invaid formula for Child Preminu");
        }
        else if (spanBenefitPlanChildFA.Visible)
        {
            if (BenefitPlanChildAmount.Text.Equals(""))
                errors.addError("Fixed Amount for Child Preminu is required");
        }

        db.validate(errors, values);

        if (!errors.isEmpty())
            return;

        db.parse(values, c);
        if (!AppUtils.checkDuplicate(dbConn, db, c, errors, "BenefitPlanCode"))
            return;

        WebUtils.StartFunction(Session, FUNCTION_CODE);
        if (CurID < 0)
        {
            db.insert(dbConn, c);
            CurID = c.BenefitPlanID;
        }
        else
        {
            db.update(dbConn, c);
        }

        WebUtils.EndFunction(dbConn);

        HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "BenefitPlan_View.aspx?BenefitPlanID=" + CurID);

    }

    protected void Delete_Click(object sender, EventArgs e)
    {
        PageErrors errors = PageErrors.getErrors(db, Page.Master);
        errors.clear();

        EBenefitPlan o = new EBenefitPlan();
        o.BenefitPlanID = CurID;
        db.select(dbConn, o);
        WebUtils.StartFunction(Session, FUNCTION_CODE);
        db.delete(dbConn, o);
        WebUtils.EndFunction(dbConn);

        HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "BenefitPlan_List.aspx");
    }

    protected void Back_Click(object sender, EventArgs e)
    {
        if (CurID > 0)
            HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "BenefitPlan_View.aspx?BenefitPlanID=" + CurID);
        else
            HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "BenefitPlan_List.aspx");

    }

    protected void BenefitPlanPaymentBaseMethod_SelectedIndexChanged(object sender, EventArgs e)
    {
        bool isERRP = BenefitPlanERPaymentBaseMethod.SelectedValue.Equals(Values.PAYMENT_BASE_RECURRING_BASIC_SALARY);
        bool isERFA = BenefitPlanERPaymentBaseMethod.SelectedValue.Equals(Values.PAYMENT_BASE_FIXED_AMOUNT);
        spanBenefitPlanERRP.Visible = isERRP;
        spanBenefitPlanERFA.Visible = isERFA;
        bool isEERP = BenefitPlanEEPaymentBaseMethod.SelectedValue.Equals(Values.PAYMENT_BASE_RECURRING_BASIC_SALARY);
        bool isEEFA = BenefitPlanEEPaymentBaseMethod.SelectedValue.Equals(Values.PAYMENT_BASE_FIXED_AMOUNT);
        spanBenefitPlanEERP.Visible = isEERP;
        spanBenefitPlanEEFA.Visible = isEEFA;
        bool isSpouseRP = BenefitPlanSpousePaymentBaseMethod.SelectedValue.Equals(Values.PAYMENT_BASE_RECURRING_BASIC_SALARY);
        bool isSpouseFA = BenefitPlanSpousePaymentBaseMethod.SelectedValue.Equals(Values.PAYMENT_BASE_FIXED_AMOUNT);
        spanBenefitPlanSpouseRP.Visible = isSpouseRP;
        spanBenefitPlanSpouseFA.Visible = isSpouseFA;
        bool isChildRP = BenefitPlanChildPaymentBaseMethod.SelectedValue.Equals(Values.PAYMENT_BASE_RECURRING_BASIC_SALARY);
        bool isChildFA = BenefitPlanChildPaymentBaseMethod.SelectedValue.Equals(Values.PAYMENT_BASE_FIXED_AMOUNT);
        spanBenefitPlanChildRP.Visible = isChildRP;
        spanBenefitPlanChildFA.Visible = isChildFA;
    }
}
