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

public partial class LeavePlan_Edit : HROneWebPage
{
    private const string FUNCTION_CODE = "LEV003";
    
    public Binding binding;
    public DBManager db = ELeavePlan.db;
    public ELeavePlan obj;
    public int CurID = -1;

    
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!WebUtils.CheckAccess(Response, Session, FUNCTION_CODE, WebUtils.AccessLevel.ReadWrite))
            return;
        toolBar.FunctionCode = FUNCTION_CODE;

        

        binding = new Binding(dbConn, db);
        binding.add(LeavePlanID);
        binding.add(LeavePlanCode);
        binding.add(LeavePlanDesc);
        binding.add(new DropDownVLBinder(db, ALProrataRoundingRuleID, EALProrataRoundingRule.VLALProrataRoundingRule));
        binding.add(new CheckBoxBinder(db, LeavePlanALRoundingRuleIsApplyFirstYearBroughtForwardOnly));
        binding.add(new CheckBoxBinder(db, LeavePlanUseCommonLeaveYear));
        binding.add(LeavePlanCommonLeaveYearStartDay);
        binding.add(new DropDownVLBinder(db, LeavePlanCommonLeaveYearStartMonth, Values.VLMonth));
        binding.add(new DropDownVLBinder(db, LeavePlanNoCountFirstIncompleteYearOfService, ELeavePlan.VLYearOfServiceReferenceMethod).setNotSelected(null));
        binding.add(new CheckBoxBinder(db, LeavePlanProrataSkipFeb29));
        binding.add(new CheckBoxBinder(db, LeavePlanResetYearOfService));
        binding.add(new CheckBoxBinder(db, LeavePlanComparePreviousLeavePlan));

        binding.add(new CheckBoxBinder(db, LeavePlanUseStatutoryHolidayEntitle));
        binding.add(new CheckBoxBinder(db, LeavePlanUsePublicHolidayEntitle));
        binding.add(new CheckBoxBinder(db, LeavePlanUseRestDayEntitle));
        binding.add(new DropDownVLBinder(db, LeavePlanRestDayEntitlePeriod, ELeavePlan.VLRestDayEntitlementPeriod));
        binding.add(LeavePlanRestDayEntitleDays);
        binding.add(new DropDownVLBinder(db, LeavePlanRestDayWeeklyEntitleStartDay, ELeavePlan.VLRestDayGainWeekDay));
        binding.add(LeavePlanRestDayMonthlyEntitleProrataBase);
        binding.add(new DropDownVLBinder(db, LeavePlanRestDayMonthlyEntitleProrataRoundingRuleID, EALProrataRoundingRule.VLALProrataRoundingRule));

        binding.add(LeavePlanLeavePlanCompareRank);
        
        binding.init(Request, Session);

        if (!int.TryParse(DecryptedRequest["LeavePlanID"], out CurID))
            CurID = -1;

        HROne.ProductLicense productLicense = WebUtils.productLicense(Session);

        if (productLicense.ProductType != HROne.ProductLicense.ProductLicenseType.HROne)
        {
            ALProrataRoundingRuleRow.Visible = false;
            ResetYearOfServiceRow.Visible = false;
            ComparePreviousLeavePlanRow.Visible = false;
            RestDayStatutoryHolidayPanel.Visible = false;
        }
        else
        {
            ALProrataRoundingRuleRow.Visible = true;
            ResetYearOfServiceRow.Visible = true;
            ComparePreviousLeavePlanRow.Visible = true;
            RestDayStatutoryHolidayPanel.Visible = true;
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
                LeavePlanUseCommonLeaveYear.Checked = true;
                LeavePlanCommonLeaveYearStartDay.Text = "1";
                LeavePlanCommonLeaveYearStartMonth.SelectedValue = "1";
                LeavePlanNoCountFirstIncompleteYearOfService.SelectedValue = "False";

                toolBar.DeleteButton_Visible = false;
            }
        }
    }

    protected void Page_PreRender(object sender, EventArgs e)
    {
        CommonLeaveYearRow1.Visible = LeavePlanUseCommonLeaveYear.Checked;
        CommonLeaveYearRow2.Visible = LeavePlanUseCommonLeaveYear.Checked;

        RestDayMonthlyOption1.Visible = LeavePlanRestDayEntitlePeriod.SelectedValue.Equals("M");
        RestDayMonthlyOption2.Visible = LeavePlanRestDayEntitlePeriod.SelectedValue.Equals("M");
        RestDayWeeklyOption.Visible = LeavePlanRestDayEntitlePeriod.SelectedValue.Equals("W");

        LeavePlanComparePreviousLeavePlanOptionPanel.Visible = LeavePlanComparePreviousLeavePlan.Checked;
    }
    protected bool loadObject() 
    {
	    obj=new ELeavePlan ();
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
        ELeavePlan  c = new ELeavePlan ();

        Hashtable values = new Hashtable();
        binding.toValues(values);

        PageErrors errors = PageErrors.getErrors(db, Page.Master);
        errors.clear();


        db.validate(errors, values);

        if (!errors.isEmpty())
            return;


        db.parse(values, c);
        if (!AppUtils.checkDuplicate(dbConn, db, c, errors, "LeavePlanCode"))
            return;

        if (c.LeavePlanUseCommonLeaveYear && ((c.LeavePlanCommonLeaveYearStartMonth < 1 || c.LeavePlanCommonLeaveYearStartMonth > 12) || (c.LeavePlanCommonLeaveYearStartDay < 1 || c.LeavePlanCommonLeaveYearStartDay > 31)))
            errors.addError(string.Format(HROne.Translation.PageErrorMessage.ERROR_FIELD_REQUIRED, new string[] { lblLeavePlanCommonLeaveYearStartDate.Text }));

        if (!errors.isEmpty())
            return;

        WebUtils.StartFunction(Session, FUNCTION_CODE);
        if (CurID < 0)
        {
//            Utils.MarkCreate(Session, c);

            db.insert(dbConn, c);
            CurID = c.LeavePlanID;
//            url = Utils.BuildURL(-1, CurID);
        }
        else
        {
//            Utils.Mark(Session, c);
            db.update(dbConn, c);
        }
        WebUtils.EndFunction(dbConn);


        HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "LeavePlan_View.aspx?LeavePlanID="+CurID);


    }
    protected void Delete_Click(object sender, EventArgs e)
    {
        PageErrors errors = PageErrors.getErrors(db, Page.Master);
        errors.clear();

        ELeavePlan c = new ELeavePlan();
        c.LeavePlanID = CurID;
        if (ELeavePlan.db.select(dbConn, c))
        {

            DBFilter empPosFilter = new DBFilter();
            empPosFilter.add(new Match("LeavePlanID", c.LeavePlanID));
            empPosFilter.add("empid", true);
            ArrayList empPosList = EEmpPositionInfo.db.select(dbConn, empPosFilter);
            if (empPosList.Count > 0)
            {
                errors.addError(string.Format(HROne.Translation.PageErrorMessage.ERROR_CODE_USED_BY_EMPLOYEE, new string[] { HROne.Common.WebUtility.GetLocalizedString("Leave Plan Code"), c.LeavePlanCode }));
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
                return;

            }
            else
            {
                WebUtils.StartFunction(Session, FUNCTION_CODE);
                db.delete(dbConn, c);
                DBFilter dbFilter = new DBFilter();
                dbFilter.add(new Match("LeavePlanID", c.LeavePlanID));
                ArrayList leaveEntitleDetailList= ELeavePlanEntitle.db.select(dbConn, dbFilter);
                foreach (ELeavePlanEntitle leaveEntitlement in leaveEntitleDetailList)
                    ELeavePlanEntitle.db.delete(dbConn, leaveEntitlement);
                WebUtils.EndFunction(dbConn);
            }
        }
        HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "LeavePlan_List.aspx");
    }
    protected void Back_Click(object sender, EventArgs e)
    {
        if (CurID > 0)
            HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "LeavePlan_View.aspx?LeavePlanID=" + CurID);
        else
            HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "LeavePlan_List.aspx");

    }
}
