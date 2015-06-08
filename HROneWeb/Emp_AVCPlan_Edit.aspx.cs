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

public partial class Emp_AVCPlan_Edit : HROneWebPage
{
    private const string FUNCTION_CODE = "PER008";
    public Binding binding;
    public DBManager db = EEmpAVCPlan.db;
    public EEmpAVCPlan obj;
    public int CurID = -1;
    public int CurEmpID = -1;
    public Hashtable CurEmpAVCPlanGroups=new Hashtable();
    public Hashtable CurRanks = new Hashtable();


    

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!WebUtils.CheckAccess(Response, Session, FUNCTION_CODE, WebUtils.AccessLevel.ReadWrite))
            return;
        toolBar.FunctionCode = FUNCTION_CODE;

        
        binding = new Binding(dbConn, db);
        binding.add(EmpAVCID);
        binding.add(EmpID);
        binding.add(new TextBoxBinder(db, EmpAVCEffFr.TextBox, EmpAVCEffFr.ID));
        binding.add(new TextBoxBinder(db, EmpAVCEffTo.TextBox, EmpAVCEffTo.ID));
        binding.add(new DropDownVLBinder(db, AVCPlanID, EAVCPlan.VLAVCPlan).setNotSelected(null));
        binding.add(new DropDownVLBinder(db, DefaultMPFPlanID, EMPFPlan.VLMPFPlan).setNotSelected(null));
        binding.add(EmpAVCERBelowRI);
        binding.add(EmpAVCEEBelowRI);
        binding.add(EmpAVCERAboveRI);
        binding.add(EmpAVCEEAboveRI);
        binding.add(EmpAVCERFix);
        binding.add(EmpAVCEEFix);
        binding.add(new CheckBoxBinder(db, EmpAVCEROverrideSetting));
        binding.add(new CheckBoxBinder(db, EmpAVCEEOverrideSetting));
        binding.add(new TextBoxXMLNodeBinder(db, EmpAVCPlanBOCIVCPlanNo, "EmpAVCPlanExtendData", EmpAVCPlanBOCIVCPlanNo.ID));
        binding.init(Request, Session);

        HROne.Common.WebUtility.WebControlsLocalization(this, this.Controls);

        if (!int.TryParse(DecryptedRequest["EmpAVCID"], out CurID))
            CurID = -1;

        if (!int.TryParse(DecryptedRequest["EmpID"], out CurEmpID))
            CurEmpID = -1;


        EmpID.Value = CurEmpID.ToString();
        if (!Page.IsPostBack)
        {
            if (CurID > 0)
            {
                loadObject();
            }
            else
                toolBar.DeleteButton_Visible = false;
        }
    }

    protected void Page_PreRender(object sender, EventArgs e)
    {
        BOCIAVCPanel.Visible = false;

        DBFilter empMPFPlanFilter = new DBFilter();
        empMPFPlanFilter.add(new Match("EmpID", CurEmpID));
        empMPFPlanFilter.add("EmpMPFEffFr", false);

        DateTime dateFrom = new DateTime();
        if (DateTime.TryParse(EmpAVCEffFr.Value, out dateFrom))
        {
            empMPFPlanFilter.add(new Match("EmpMPFEffFr", "<=", dateFrom));
        }

        ArrayList empMPFPlanList = EEmpMPFPlan.db.select(dbConn, empMPFPlanFilter);


        EMPFPlan mpfPlan = new EMPFPlan();
        if (empMPFPlanList.Count > 0)
        {
            EEmpMPFPlan empMPFPlan = (EEmpMPFPlan)empMPFPlanList[0];
            mpfPlan.MPFPlanID = empMPFPlan.MPFPlanID;

        }
        else if (!DefaultMPFPlanID.SelectedValue.Equals(string.Empty))
        {
            mpfPlan.MPFPlanID = int.Parse(DefaultMPFPlanID.SelectedValue);

        }
        if (EMPFPlan.db.select(dbConn, mpfPlan))
        {
            EMPFScheme mpfScheme = new EMPFScheme();
            mpfScheme.MPFSchemeID = mpfPlan.MPFSchemeID;
            if (EMPFScheme.db.select(dbConn, mpfScheme))
            {
                if (mpfScheme.MPFSchemeTrusteeCode.Equals("BOCI"))
                {
                    BOCIAVCPanel.Visible = true;
                }

                else
                {
                    BOCIAVCPanel.Visible = false;
                }
            }
        }
    }

    protected bool loadObject() 
    {
	    obj=new EEmpAVCPlan();
	    bool isNew=WebFormWorkers.loadKeys(db, obj, DecryptedRequest);

        DBFilter filter = new DBFilter();
        filter.add(new Match("EmpID", CurEmpID));
        filter.add(WebUtils.AddRankFilter(Session, "EmpID", true));
        if (EEmpPersonalInfo.db.count(dbConn, filter) == 0)
            if (CurEmpID <= 0)
                return false;
            else
                HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "~/AccessDeny.aspx");

	    if(!db.select(dbConn, obj))
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
        EEmpAVCPlan c = new EEmpAVCPlan();

        Hashtable values = new Hashtable();
        binding.toValues(values);

        PageErrors errors = PageErrors.getErrors(db, Page.Master);
        errors.clear();


        db.validate(errors, values);

        if (!errors.isEmpty())
            return;


        db.parse(values, c);

        if (c.EmpAVCEffTo.Ticks > 0 && c.EmpAVCEffTo < c.EmpAVCEffFr)
        {
            errors.addError("EmpAVCEffTo", HROne.Translation.PageErrorMessage.ERROR_DATE_TO_TOO_EARLY);
            return;
        }
        if (c.EmpAVCEffFr < new DateTime(2000, 12, 1))
        {
            errors.addError("EmpAVCEffFr", HROne.Translation.PageErrorMessage.ERROR_MPF_DATE_TO_TOO_EARLY);
            return;
        }

        AND andTerms = new AND();
        andTerms.add(new Match("EmpAVCID", "<>", c.EmpAVCID));
        andTerms.add(new Match("EmpAVCEffFr", "<=", c.EmpAVCEffFr));
        EEmpAVCPlan lastObj = (EEmpAVCPlan)AppUtils.GetLastObj(dbConn, db, "EmpAVCEffFr", c.EmpID, andTerms);
        if (lastObj != null && (c.EmpAVCEffFr <= lastObj.EmpAVCEffTo || c.EmpAVCEffFr == lastObj.EmpAVCEffFr))
        {
            errors.addError("EmpAVCEffFr", HROne.Translation.PageErrorMessage.ERROR_DATE_FROM_OVERLAP);
            return;
        }

        WebUtils.StartFunction(Session, FUNCTION_CODE, c.EmpID);
        if (CurID < 0)
        {
//            Utils.MarkCreate(Session, c);

            db.insert(dbConn, c);
            CurID = c.EmpAVCID;
//            url = Utils.BuildURL(-1, CurID);
        }
        else
        {
//            Utils.Mark(Session, c);
            db.update(dbConn, c);
        }

        if (lastObj != null)
        {
            if (lastObj.EmpAVCEffTo < lastObj.EmpAVCEffFr)
            {
                lastObj.EmpAVCEffTo = c.EmpAVCEffFr.AddDays(-1);
                db.update(dbConn, lastObj);
            }
        }
        WebUtils.EndFunction(dbConn);

        HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "Emp_AVCPlan_View.aspx?EmpAVCID="+CurID+ "&EmpID=" + c.EmpID);


    }
    protected void Delete_Click(object sender, EventArgs e)
    {

        EEmpAVCPlan c = new EEmpAVCPlan();
        c.EmpAVCID = CurID;
        EEmpAVCPlan.db.select(dbConn, c);
        WebUtils.StartFunction(Session, FUNCTION_CODE, c.EmpID);
        db.delete(dbConn, c);
        WebUtils.EndFunction(dbConn);
        HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "EmpTab_Pension_View.aspx?EmpID=" + EmpID.Value);
 
   }

    protected void Back_Click(object sender, EventArgs e)
    {
        if (CurID > 0)
            HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "Emp_AVCPlan_View.aspx?EmpAVCID=" + CurID + "&EmpID=" + EmpID.Value);
        else
            HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "EmpTab_Pension_View.aspx?EmpID=" + EmpID.Value);

    }
}
