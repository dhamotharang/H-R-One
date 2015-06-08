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

public partial class ORSOPlan_Edit : HROneWebPage
{
    private const string FUNCTION_CODE = "MPF004";
    public Binding binding;
    public DBManager db = EORSOPlan.db;
    public EORSOPlan obj;
    public int CurID = -1;
    

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!WebUtils.CheckAccess(Response, Session, FUNCTION_CODE, WebUtils.AccessLevel.ReadWrite))
            return;
        toolBar.FunctionCode = FUNCTION_CODE;

        

        binding = new Binding(dbConn, db);
        binding.add(ORSOPlanID);
        binding.add(ORSOPlanCode);
        binding.add(ORSOPlanDesc);
        binding.add(ORSOPlanSchemeNo);
        binding.add(new CheckBoxBinder(db, ORSOPlanEmployerResidual));
        binding.add(new CheckBoxBinder(db, ORSOPlanEmployeeResidual));
        binding.add(ORSOPlanEmployerResidualCap);
        binding.add(ORSOPlanEmployeeResidualCap);
        binding.add(ORSOPlanCompanyName);
        binding.add(ORSOPlanPayCenter);
        binding.add(ORSOPlanMaxEmployerVC);
        binding.add(ORSOPlanMaxEmployeeVC);
        binding.add(new DropDownVLBinder(db, ORSOPlanEmployerRoundingRule, Values.VLRoundingRule));
        binding.add(new DropDownVLBinder(db, ORSOPlanEmployerDecimalPlace, Values.VLDecimalPlace));
        binding.add(new DropDownVLBinder(db, ORSOPlanEmployeeRoundingRule, Values.VLRoundingRule));
        binding.add(new DropDownVLBinder(db, ORSOPlanEmployeeDecimalPlace, Values.VLDecimalPlace));
        
        binding.init(Request, Session);

        if (!int.TryParse(DecryptedRequest["ORSOPlanID"], out CurID))
            CurID = -1;

        HROne.Common.WebUtility.WebControlsLocalization(this, this.Controls);

		if(!Page.IsPostBack) 
		{
            if (CurID > 0)
            {
                loadObject();

            }
            else
                toolBar.DeleteButton_Visible = false;
        }
    }
    protected bool loadObject() 
    {
	    obj=new EORSOPlan();
	    bool isNew=WebFormWorkers.loadKeys(db, obj, DecryptedRequest);
	    if(!db.select(dbConn, obj))
		    return false;

        if (string.IsNullOrEmpty(obj.ORSOPlanEmployerRoundingRule) && string.IsNullOrEmpty(obj.ORSOPlanEmployeeRoundingRule))
        {
            obj.ORSOPlanEmployerRoundingRule = Values.ROUNDING_RULE_ROUND_TO;
            obj.ORSOPlanEmployerDecimalPlace = 2;
            obj.ORSOPlanEmployeeRoundingRule = Values.ROUNDING_RULE_ROUND_TO;
            obj.ORSOPlanEmployeeDecimalPlace = 2;
        }

	    Hashtable values=new Hashtable();
	    db.populate(obj,values);
	    binding.toControl(values);
        return true;
    }

    protected void Save_Click(object sender, EventArgs e)
    {
        EORSOPlan c = new EORSOPlan();

        Hashtable values = new Hashtable();
        binding.toValues(values);

        PageErrors errors = PageErrors.getErrors(db, Page.Master);
        errors.clear();


        db.validate(errors, values);

        if (!errors.isEmpty())
            return;


        db.parse(values, c);
        if (!AppUtils.checkDuplicate(dbConn, db, c, errors, "ORSOPlanCode"))
            return;

        WebUtils.StartFunction(Session, FUNCTION_CODE);
        if (CurID < 0)
        {
//            Utils.MarkCreate(Session, c);

            db.insert(dbConn, c);
            CurID = c.ORSOPlanID;
//            url = Utils.BuildURL(-1, CurID);
        }
        else
        {
//            Utils.Mark(Session, c);
            db.update(dbConn, c);
        }
        WebUtils.EndFunction(dbConn);


        HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "ORSOPlan_View.aspx?ORSOPlanID="+CurID);


    }
    protected void Delete_Click(object sender, EventArgs e)
    {
        PageErrors errors = PageErrors.getErrors(db, Page.Master);
        errors.clear();

        EORSOPlan o = new EORSOPlan();
        o.ORSOPlanID = CurID;
        if (db.select(dbConn, o))
        {
            DBFilter empORSOFilter = new DBFilter();
            empORSOFilter.add(new Match("ORSOPlanID", o.ORSOPlanID));
            empORSOFilter.add("empid", true);
            ArrayList empORSOList = EEmpORSOPlan.db.select(dbConn, empORSOFilter);
            if (empORSOList.Count > 0)
            {
                int curEmpID = 0;
                errors.addError(string.Format(HROne.Translation.PageErrorMessage.ERROR_CODE_USED_BY_EMPLOYEE, new string[] { HROne.Common.WebUtility.GetLocalizedString("P-Fund Plan Code"), o.ORSOPlanCode }));
                foreach (EEmpORSOPlan empORSOPlan in empORSOList)
                {
                    EEmpPersonalInfo empInfo = new EEmpPersonalInfo();
                    empInfo.EmpID = empORSOPlan.EmpID;
                    if (EEmpPersonalInfo.db.select(dbConn, empInfo))
                        if (curEmpID != empORSOPlan.EmpID)
                        {
                            errors.addError("- " + empInfo.EmpNo + ", " + empInfo.EmpEngFullName);
                            curEmpID = empORSOPlan.EmpID;
                        }
                        else
                            EEmpORSOPlan.db.delete(dbConn, empORSOPlan);

                }
                errors.addError(HROne.Translation.PageErrorMessage.ERROR_ACTION_ABORT);
                return;
            }
            else
            {

                WebUtils.StartFunction(Session, FUNCTION_CODE);
                db.delete(dbConn, o);
                DBFilter obj = new DBFilter();
                obj.add(new Match("ORSOPlanID", o.ORSOPlanID));
                ArrayList objList = EORSOPlanDetail.db.select(dbConn, obj);
                foreach (EORSOPlanDetail match in objList)
                    EORSOPlanDetail.db.delete(dbConn, match);
                WebUtils.EndFunction(dbConn);

            }
        }
        HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "ORSOPlan_List.aspx");
    }
    protected void Back_Click(object sender, EventArgs e)
    {
        if (CurID > 0)
            HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "ORSOPlan_View.aspx?ORSOPlanID=" + CurID);
        else
            HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "ORSOPlan_List.aspx");

    }
}
