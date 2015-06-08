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

public partial class Emp_MPFPlan_View : HROneWebPage
{
    private const string FUNCTION_CODE = "PER008";
    public Binding binding;
    public DBManager db = EEmpMPFPlan.db;
    public EEmpMPFPlan obj;
    public int CurID = -1;
    public int CurEmpID = -1;
    public Hashtable CurEmpMPFPlanGroups=new Hashtable();
    public Hashtable CurRanks = new Hashtable();

    protected WFValueList VLSCBPFundType = new HROne.DataAccess.WFTextList(
    new string[] { "1", "2", "3", "4", "5", "6" },
    new string[] { "MPF Only Advance", "MPF Only Basic", "MPF Advance wTopUp", "MPF Basic wTopUp", "MPF Only-ESS", "MPF ESS wTopUp" });

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!WebUtils.CheckAccess(Response, Session, FUNCTION_CODE, WebUtils.AccessLevel.Read))
            return;
        toolBar.FunctionCode = FUNCTION_CODE;

        binding = new Binding(dbConn, db);
        binding.add(EmpMPFID);
        binding.add(EmpID);
        binding.add(EmpMPFEffFr);
        binding.add(EmpMPFEffTo);
        binding.add(new LabelVLBinder(db, MPFPlanID, EMPFPlan.VLMPFPlan));
        binding.add(new LabelXMLNodeBinder(db, EmpMPFPlanClassName, "EmpMPFPlanExtendData", EmpMPFPlanClassName.ID));
        binding.add(new LabelXMLNodeBinder(db, EmpMPFPlanAIABenefitPlanNo, "EmpMPFPlanExtendData", EmpMPFPlanAIABenefitPlanNo.ID));
        binding.add(new LabelXMLNodeVLBinder(db, EmpMPFPlanSCBPFundType, VLSCBPFundType, null, "EmpMPFPlanExtendData", EmpMPFPlanSCBPFundType.ID));
        binding.init(Request, Session);

        if (!int.TryParse(DecryptedRequest["EmpMPFID"], out CurID))
            CurID = -1; 

        if (!int.TryParse(DecryptedRequest["EmpID"], out CurEmpID))
            CurEmpID = -1; 

        EmpID.Value = CurEmpID.ToString();

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
        obj = new EEmpMPFPlan();
        bool isNew = WebFormWorkers.loadKeys(db, obj, DecryptedRequest);

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

        Hashtable values = new Hashtable();
        db.populate(obj, values);
        binding.toControl(values);

        HSBCMPFPanel.Visible = false;
        AIAMPFPanel.Visible = false;
        SCBMPFPanel.Visible = false;
        EMPFPlan mpfPlan = new EMPFPlan();
        mpfPlan.MPFPlanID = obj.MPFPlanID;
        if (EMPFPlan.db.select(dbConn, mpfPlan))
        {
            EMPFScheme mpfScheme = new EMPFScheme();
            mpfScheme.MPFSchemeID = mpfPlan.MPFSchemeID;
            if (EMPFScheme.db.select(dbConn, mpfScheme))
            {
                if (mpfScheme.MPFSchemeTrusteeCode.Equals("HSBC")
                || mpfScheme.MPFSchemeTrusteeCode.Equals("HangSeng")
            )
                {
                    HSBCMPFPanel.Visible = true;
                }
                else if (mpfScheme.MPFSchemeTrusteeCode.Equals("AIA"))
                {
                    AIAMPFPanel.Visible = true;
                }
                else if (mpfScheme.MPFSchemeCode.Equals("MT00415")
                 || mpfScheme.MPFSchemeCode.Equals("MT00423"))
                {
                    SCBMPFPanel.Visible = true;
                }
            }
        }


        return true;
    }

    protected void Delete_Click(object sender, EventArgs e)
    {

        EEmpMPFPlan c = new EEmpMPFPlan();
        c.EmpMPFID = CurID;
        db.select(dbConn, c);
        WebUtils.StartFunction(Session, FUNCTION_CODE, c.EmpID);
        db.delete(dbConn, c);
        WebUtils.EndFunction(dbConn);
        HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "EmpTab_Pension_View.aspx?EmpID=" + EmpID.Value);
    }

    protected void Edit_Click(object sender, EventArgs e)
    {
        HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "Emp_MPFPlan_Edit.aspx?EmpMPFID=" + EmpMPFID.Value + "&EmpID=" + EmpID.Value);
    }

    protected void Back_Click(object sender, EventArgs e)
    {
        HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "EmpTab_Pension_View.aspx?EmpID=" + EmpID.Value);
    }
}
