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
public partial class YEBPlan_Edit : HROneWebPage
{
    private const string FUNCTION_CODE = "SYS007";

    public Binding binding;
    public DBManager db = EYEBPlan.db;
    public int CurID = -1;
    

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!WebUtils.CheckAccess(Response, Session, FUNCTION_CODE, WebUtils.AccessLevel.ReadWrite))
            return;
        toolBar.FunctionCode = FUNCTION_CODE;

        

        binding = new Binding(dbConn, db);
        binding.add(YEBPlanID);

        binding.add(YEBPlanCode);
        binding.add(YEBPlanDesc);

        binding.add(new DropDownVLBinder(db, YEBPlanPaymentBaseMethod, EYEBPlan.VLPaymentBaseMethod));

        //  need Basic Salary Filter
        DBFilter RPPaymentCodeFilter = new DBFilter();
        RPPaymentCodeFilter.add(new Match("PaymentTypeID", EPaymentType.SystemPaymentType.BasicSalaryPaymentType(dbConn).PaymentTypeID));
        binding.add(new DropDownVLBinder(db, YEBPlanRPPaymentCodeID, EPaymentCode.VLPaymentCode, RPPaymentCodeFilter));
        binding.add(YEBPlanMultiplier);
        binding.add(new DropDownVLBinder(db, YEBPlanProrataMethod, EYEBPlan.VLProrataMethod));

        //  need Basic Salary or others Filter
        DBFilter paymentCodeFilter = new DBFilter();
        OR orPaymentTypeFilter = new OR();
        orPaymentTypeFilter.add(new Match("PaymentTypeID", EPaymentType.SystemPaymentType.BasicSalaryPaymentType(dbConn).PaymentTypeID));
        orPaymentTypeFilter.add(new Match("PaymentTypeID", EPaymentType.SystemPaymentType.OthersPaymentType(dbConn).PaymentTypeID));
        orPaymentTypeFilter.add(new Match("PaymentTypeID", EPaymentType.SystemPaymentType.BonusPaymentType(dbConn).PaymentTypeID));
        paymentCodeFilter.add(orPaymentTypeFilter);
        binding.add(new DropDownVLBinder(db, YEBPlanPaymentCodeID, EPaymentCode.VLPaymentCode, paymentCodeFilter));
        binding.add(YEBPlanEligiblePeriod);
        binding.add(new DropDownVLBinder(db, YEBPlanEligibleUnit, Values.VLEmpUnit));
        binding.add(new CheckBoxBinder(db, YEBPlanIsEligibleAfterProbation));
        binding.add(new CheckBoxBinder(db, YEBPlanEligiblePeriodIsCheckEveryYEBYear));
        binding.add(new CheckBoxBinder(db, YEBPlanEligiblePeriodIsExcludeMax3MonthsProbation));
        binding.add(new CheckBoxBinder(db, YEBPlanIsGlobal));
        binding.init(Request, Session);

        if (!int.TryParse(DecryptedRequest["YEBPlanID"], out CurID))
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
        EYEBPlan obj = new EYEBPlan();
        bool isNew = WebFormWorkers.loadKeys(db, obj, DecryptedRequest);
        if (!db.select(dbConn, obj))
            return false;

        Hashtable values = new Hashtable();
        db.populate(obj, values);
        binding.toControl(values);

        if (YEBPlanPaymentBaseMethod.SelectedValue.Equals(Values.PAYMENT_BASE_RECURRING_BASIC_SALARY))
            YEBPlanRPPaymentCodeID.Visible = true;
        else
            YEBPlanRPPaymentCodeID.Visible = false;

        return true;
    }

    protected void Save_Click(object sender, EventArgs e)
    {
        EYEBPlan c = new EYEBPlan();

        Hashtable values = new Hashtable();
        binding.toValues(values);

        PageErrors errors = PageErrors.getErrors(db, Page.Master);
        errors.clear();

        if (YEBPlanRPPaymentCodeID.Visible)
            if (YEBPlanRPPaymentCodeID.SelectedValue.Equals(""))
                errors.addError("Payment Code for formula is required");

        db.validate(errors, values);

        if (!errors.isEmpty())
            return;

        db.parse(values, c);
        if (!AppUtils.checkDuplicate(dbConn, db, c, errors, "YEBPlanCode"))
            return;

        WebUtils.StartFunction(Session, FUNCTION_CODE);
        if (CurID < 0)
        {
            db.insert(dbConn, c);
            CurID = c.YEBPlanID;
        }
        else
        {
            db.update(dbConn, c);
        }

        WebUtils.EndFunction(dbConn);

        if (c.YEBPlanIsGlobal)
        {
            DBFilter filter = new DBFilter();
            filter.add(new Match("YEBPlanID", "<>", c.YEBPlanID));
            filter.add(new Match("YEBPlanIsGlobal", "<>", false));
            EYEBPlan t = new EYEBPlan();
            t.YEBPlanIsGlobal = false;
            db.updateByTemplate(dbConn, t, filter);
        }

        HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "YEBPlan_View.aspx?YEBPlanID=" + CurID);


    }

    protected void Delete_Click(object sender, EventArgs e)
    {
        PageErrors errors = PageErrors.getErrors(db, Page.Master);
        errors.clear();

        EYEBPlan o = new EYEBPlan();
        o.YEBPlanID = CurID;
        db.select(dbConn, o);
        DBFilter empYEBPlanFilter = new DBFilter();
        empYEBPlanFilter.add(new Match("YEBPlanID", o.YEBPlanID));
        empYEBPlanFilter.add("empid", true);
        ArrayList empPosList = EEmpPositionInfo.db.select(dbConn, empYEBPlanFilter);
        if (empPosList.Count > 0)
        {
            int curEmpID = 0;
            errors.addError(string.Format(HROne.Translation.PageErrorMessage.ERROR_CODE_USED_BY_EMPLOYEE, new string[] { HROne.Common.WebUtility.GetLocalizedString("YEB Plan Code"), o.YEBPlanCode }));
            foreach (EEmpPositionInfo empPos in empPosList)
            {
                EEmpPersonalInfo empInfo = new EEmpPersonalInfo();
                empInfo.EmpID = empPos.EmpID;
                if (EEmpPersonalInfo.db.select(dbConn, empInfo))
                    if (curEmpID != empPos.EmpID)
                    {
                        errors.addError("- " + empInfo.EmpNo + ", " + empInfo.EmpEngFullName);
                        curEmpID = empPos.EmpID;
                    }
                    else
                        EEmpPositionInfo.db.delete(dbConn, empPos);

            }
            errors.addError(HROne.Translation.PageErrorMessage.ERROR_ACTION_ABORT);
            return;
        }
        else
        {
            WebUtils.StartFunction(Session, FUNCTION_CODE);
            db.delete(dbConn, o);
            WebUtils.EndFunction(dbConn);

        }

        HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "YEBPlan_List.aspx");
    }

    protected void Back_Click(object sender, EventArgs e)
    {
        if (CurID > 0)
            HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "YEBPlan_View.aspx?YEBPlanID=" + CurID);
        else
            HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "YEBPlan_List.aspx");

    }
    protected void YEBPlanPaymentBaseMethod_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (YEBPlanPaymentBaseMethod.SelectedValue.Equals(Values.PAYMENT_BASE_RECURRING_BASIC_SALARY))
            YEBPlanRPPaymentCodeID.Visible = true;
        else
            YEBPlanRPPaymentCodeID.Visible = false;
    }
}
