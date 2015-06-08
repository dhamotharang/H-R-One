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


public partial class BenefitPlan_View : HROneWebPage
{
    private const string FUNCTION_CODE = "SYS022";

    public Binding binding;
    public DBManager db = EBenefitPlan.db;
    public int CurID = -1;


    protected void Page_Load(object sender, EventArgs e)
    {
        if (!WebUtils.CheckAccess(Response, Session, FUNCTION_CODE, WebUtils.AccessLevel.Read))
            return;
        toolBar.FunctionCode = FUNCTION_CODE;

        binding = new Binding(dbConn, db);
        binding.add(BenefitPlanCode);
        binding.add(BenefitPlanDesc);

        binding.add(new LabelVLBinder(db, BenefitPlanERPaymentBaseMethod, EBenefitPlan.VLPaymentBaseMethod));
        binding.add(new LabelVLBinder(db, BenefitPlanERPaymentCodeID, EPaymentCode.VLPaymentCode));
        binding.add(BenefitPlanERMultiplier);
        binding.add(BenefitPlanERAmount);
        binding.add(new LabelVLBinder(db, BenefitPlanEEPaymentBaseMethod, EBenefitPlan.VLPaymentBaseMethod));
        binding.add(new LabelVLBinder(db, BenefitPlanEEPaymentCodeID, EPaymentCode.VLPaymentCode));
        binding.add(BenefitPlanEEMultiplier);
        binding.add(BenefitPlanEEAmount);
        binding.add(new LabelVLBinder(db, BenefitPlanSpousePaymentBaseMethod, EBenefitPlan.VLPaymentBaseMethod));
        binding.add(new LabelVLBinder(db, BenefitPlanSpousePaymentCodeID, EPaymentCode.VLPaymentCode));
        binding.add(BenefitPlanSpouseMultiplier);
        binding.add(BenefitPlanSpouseAmount);
        binding.add(new LabelVLBinder(db, BenefitPlanChildPaymentBaseMethod, EBenefitPlan.VLPaymentBaseMethod));
        binding.add(new LabelVLBinder(db, BenefitPlanChildPaymentCodeID, EPaymentCode.VLPaymentCode));
        binding.add(BenefitPlanChildMultiplier);
        binding.add(BenefitPlanChildAmount);
        binding.init(Request, Session);

        if (!int.TryParse(DecryptedRequest["BenefitPlanID"], out CurID))
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
        EBenefitPlan obj = new EBenefitPlan();
        bool isNew = WebFormWorkers.loadKeys(db, obj, DecryptedRequest);
        if (!db.select(dbConn, obj))
            return false;

        bool isERRP = obj.BenefitPlanERPaymentBaseMethod.Equals(Values.PAYMENT_BASE_RECURRING_BASIC_SALARY);
        bool isERFA = obj.BenefitPlanERPaymentBaseMethod.Equals(Values.PAYMENT_BASE_FIXED_AMOUNT);
        spanBenefitPlanERRP.Visible = isERRP;
        spanBenefitPlanERFA.Visible = isERFA;
        bool isEERP = obj.BenefitPlanEEPaymentBaseMethod.Equals(Values.PAYMENT_BASE_RECURRING_BASIC_SALARY);
        bool isEEFA = obj.BenefitPlanEEPaymentBaseMethod.Equals(Values.PAYMENT_BASE_FIXED_AMOUNT);
        spanBenefitPlanEERP.Visible = isEERP;
        spanBenefitPlanEEFA.Visible = isEEFA;
        bool isSpouseRP = obj.BenefitPlanSpousePaymentBaseMethod.Equals(Values.PAYMENT_BASE_RECURRING_BASIC_SALARY);
        bool isSpouseFA = obj.BenefitPlanSpousePaymentBaseMethod.Equals(Values.PAYMENT_BASE_FIXED_AMOUNT);
        spanBenefitPlanSpouseRP.Visible = isSpouseRP;
        spanBenefitPlanSpouseFA.Visible = isSpouseFA;
        bool isChildRP = obj.BenefitPlanChildPaymentBaseMethod.Equals(Values.PAYMENT_BASE_RECURRING_BASIC_SALARY);
        bool isChildFA = obj.BenefitPlanChildPaymentBaseMethod.Equals(Values.PAYMENT_BASE_FIXED_AMOUNT);
        spanBenefitPlanChildRP.Visible = isChildRP;
        spanBenefitPlanChildFA.Visible = isChildFA;

        Hashtable values = new Hashtable();
        db.populate(obj, values);
        binding.toControl(values);

        return true;
    }

    protected void Edit_Click(object sender, EventArgs e)
    {
        HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "BenefitPlan_Edit.aspx?BenefitPlanID=" + CurID);
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
        HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "BenefitPlan_List.aspx");

    }
}
