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

public partial class Emp_AVCPlan_View : HROneWebPage
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
        if (!WebUtils.CheckAccess(Response, Session, FUNCTION_CODE, WebUtils.AccessLevel.Read))
            return;
        toolBar.FunctionCode = FUNCTION_CODE;

        binding = new Binding(dbConn, db);
        binding.add(EmpAVCID);
        binding.add(EmpID);
        binding.add(EmpAVCEffFr);
        binding.add(EmpAVCEffTo);
        binding.add(new LabelVLBinder(db, AVCPlanID, EAVCPlan.VLAVCPlan));
        binding.add(new LabelVLBinder(db, DefaultMPFPlanID, EMPFPlan.VLMPFPlan));
        binding.add(EmpAVCERBelowRI);
        binding.add(EmpAVCEEBelowRI);
        binding.add(EmpAVCERAboveRI);
        binding.add(EmpAVCEEAboveRI);
        binding.add(EmpAVCERFix);
        binding.add(EmpAVCEEFix);
        binding.add(new LabelVLBinder(db, EmpAVCEROverrideSetting, Values.VLTrueFalseYesNo));
        binding.add(new LabelVLBinder(db, EmpAVCEEOverrideSetting, Values.VLTrueFalseYesNo));
        binding.add(new LabelXMLNodeBinder(db, EmpAVCPlanBOCIVCPlanNo, "EmpAVCPlanExtendData", EmpAVCPlanBOCIVCPlanNo.ID));
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
                loadObject();
            else
                toolBar.DeleteButton_Visible = false;
        }
    }

    protected void Page_PreRender(object sender, EventArgs e)
    {
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

        BOCIAVCPanel.Visible = false;

        DBFilter empMPFPlanFilter = new DBFilter();
        empMPFPlanFilter.add(new Match("EmpID", CurEmpID));
        empMPFPlanFilter.add("EmpMPFEffFr", false);

        if (!obj.EmpAVCEffFr.Ticks.Equals(0))
        {
            empMPFPlanFilter.add(new Match("EmpMPFEffFr", "<=", obj.EmpAVCEffFr));
        }

        ArrayList empMPFPlanList = EEmpMPFPlan.db.select(dbConn, empMPFPlanFilter);


        EMPFPlan mpfPlan = new EMPFPlan();
        if (empMPFPlanList.Count > 0)
        {
            EEmpMPFPlan empMPFPlan = (EEmpMPFPlan)empMPFPlanList[0];
            mpfPlan.MPFPlanID = empMPFPlan.MPFPlanID;

        }
        else 
        {
            mpfPlan.MPFPlanID = obj.DefaultMPFPlanID;

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

        return true;
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
    protected void Edit_Click(object sender, EventArgs e)
    {
        HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "Emp_AVCPlan_Edit.aspx?EmpAVCID=" + EmpAVCID.Value + "&EmpID=" + EmpID.Value);
    }

    protected void Back_Click(object sender, EventArgs e)
    {
        HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "EmpTab_Pension_View.aspx?EmpID=" + EmpID.Value);
    }
}
