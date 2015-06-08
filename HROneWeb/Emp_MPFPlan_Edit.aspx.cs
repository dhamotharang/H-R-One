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

public partial class Emp_MPFPlan_Edit : HROneWebPage
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
        new string[] { "1", "2", "3", "4", "5", "6"},
        new string[] { "MPF Only Advance", "MPF Only Basic", "MPF Advance wTopUp", "MPF Basic wTopUp", "MPF Only-ESS", "MPF ESS wTopUp" });

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!WebUtils.CheckAccess(Response, Session, FUNCTION_CODE, WebUtils.AccessLevel.ReadWrite))
            return;
        toolBar.FunctionCode = FUNCTION_CODE;

        
        binding = new Binding(dbConn, db);
        binding.add(EmpMPFID);
        binding.add(EmpID);
        binding.add(new TextBoxBinder(db, EmpMPFEffFr.TextBox, EmpMPFEffFr.ID));
        binding.add(new TextBoxBinder(db, EmpMPFEffTo.TextBox, EmpMPFEffTo.ID));
        binding.add(new DropDownVLBinder(db, MPFPlanID, EMPFPlan.VLMPFPlan).setNotSelected(null));
        binding.add(new TextBoxXMLNodeBinder(db, EmpMPFPlanClassName, "EmpMPFPlanExtendData", EmpMPFPlanClassName.ID));
        binding.add(new TextBoxXMLNodeBinder(db, EmpMPFPlanAIABenefitPlanNo, "EmpMPFPlanExtendData", EmpMPFPlanAIABenefitPlanNo.ID));
        binding.add(new DropDownListXMLNodeVLBinder(db, EmpMPFPlanSCBPFundType, VLSCBPFundType, null, "EmpMPFPlanExtendData", EmpMPFPlanSCBPFundType.ID));
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
            {
                loadObject();
            }
            else
                toolBar.DeleteButton_Visible = false;
        }
    }

    protected void Page_PreRender(object sender, EventArgs e)
    {
        if (!MPFPlanID.SelectedValue.Equals(string.Empty))
        {
            int intMPFPlanID = int.Parse(MPFPlanID.SelectedValue);

            HSBCMPFPanel.Visible = false;
            AIAMPFPanel.Visible = false;
            SCBMPFPanel.Visible = false;

            EMPFPlan mpfPlan = new EMPFPlan();
            mpfPlan.MPFPlanID = intMPFPlanID;
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
                        if (EmpMPFPlanClassName.Text.Equals(string.Empty))
                        {
                            System.Xml.XmlNodeList payCenterNode = HROne.CommonLib.Utility.GetXmlDocumentByDataString(mpfPlan.MPFPlanExtendData).GetElementsByTagName("MPFPlanDefaultClassName");
                            if (payCenterNode.Count > 0)
                                EmpMPFPlanClassName.Text = payCenterNode[0].InnerText;
                        }
                        HSBCMPFPanel.Visible = true;
                    }
                    else if (mpfScheme.MPFSchemeTrusteeCode.Equals("AIA"))
                    {
                        if (EmpMPFPlanClassName.Text.Equals(string.Empty))
                            EmpMPFPlanClassName.Text = "01";
                        AIAMPFPanel.Visible = true;
                    }
                    else if (mpfScheme.MPFSchemeCode.Equals("MT00415")
                      || mpfScheme.MPFSchemeCode.Equals("MT00423"))
                    {
                        SCBMPFPanel.Visible = true;
                    }
                    else
                    {
                    }
                }
            }
        }
        if (CurEmpID > 0 && CurID == -1 && EmpMPFEffFr.Value.ToString() == "")
        {
            EEmpPersonalInfo empOBJ = new EEmpPersonalInfo();
            DBFilter filter = new DBFilter();
            filter.add(new Match("EmpID", CurEmpID));
            ArrayList empInfoList = EEmpPersonalInfo.db.select(dbConn, filter);
            if (empInfoList.Count > 0)
            {
                empOBJ = (EEmpPersonalInfo)empInfoList[0];

                EmpMPFEffFr.Value = empOBJ.EmpDateOfJoin.ToString("yyyy-MM-dd");

            }
        }
    }

    protected bool loadObject() 
    {
	    obj=new EEmpMPFPlan();
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
        if (!HSBCMPFPanel.Visible)
        {
            EmpMPFPlanClassName.Text = string.Empty;
        }
        EEmpMPFPlan c = new EEmpMPFPlan();

        Hashtable values = new Hashtable();
        binding.toValues(values);

        PageErrors errors = PageErrors.getErrors(db, Page.Master);
        errors.clear();


        db.validate(errors, values);

        if (!errors.isEmpty())
            return;


        db.parse(values, c);

        if (c.EmpMPFEffTo.Ticks > 0 && c.EmpMPFEffTo < c.EmpMPFEffFr)
        {
            errors.addError("EmpMPFEffTo", HROne.Translation.PageErrorMessage.ERROR_DATE_TO_TOO_EARLY);
            return;
        }

        if (c.EmpMPFEffFr < new DateTime(2000, 12, 1))
        {
            errors.addError("EmpMPFEffFr", HROne.Translation.PageErrorMessage.ERROR_MPF_DATE_TO_TOO_EARLY);
            return;
        }

        AND andTerms = new AND();
        andTerms.add(new Match("EmpMPFID", "<>", c.EmpMPFID));
        andTerms.add(new Match("EmpMPFEffFr", "<=", c.EmpMPFEffFr));

        EEmpMPFPlan lastObj = (EEmpMPFPlan)AppUtils.GetLastObj(dbConn, db, "EmpMPFEffFr", c.EmpID, andTerms);
        if (lastObj != null && (c.EmpMPFEffFr <= lastObj.EmpMPFEffTo || c.EmpMPFEffFr == lastObj.EmpMPFEffFr))
        {
            errors.addError("EmpMPFEffFr", HROne.Translation.PageErrorMessage.ERROR_DATE_FROM_OVERLAP);
            return;
        }

        WebUtils.StartFunction(Session, FUNCTION_CODE, c.EmpID);
        if (CurID < 0)
        {
//            Utils.MarkCreate(Session, c);

            db.insert(dbConn, c);
            CurID = c.EmpMPFID;
//            url = Utils.BuildURL(-1, CurID);
        }
        else
        {
//            Utils.Mark(Session, c);
            db.update(dbConn, c);
        }

        if (lastObj != null)
        {
            if (lastObj.EmpMPFEffTo < lastObj.EmpMPFEffFr)
            {

                lastObj.EmpMPFEffTo = c.EmpMPFEffFr.AddDays(-1);
                db.update(dbConn, lastObj);
            }
        }
        WebUtils.EndFunction(dbConn);

        HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "Emp_MPFPlan_View.aspx?EmpMPFID="+CurID+ "&EmpID=" + c.EmpID);


    }
    protected void Delete_Click(object sender, EventArgs e)
    {

        EEmpMPFPlan c = new EEmpMPFPlan();
        c.EmpMPFID = CurID;
        EEmpMPFPlan.db.select(dbConn, c);
        WebUtils.StartFunction(Session, FUNCTION_CODE, c.EmpID);
        db.delete(dbConn, c);
        WebUtils.EndFunction(dbConn);
        HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "EmpTab_Pension_View.aspx?EmpID=" + EmpID.Value);
    }
    
    protected void Back_Click(object sender, EventArgs e)
    {
        if (CurID > 0)
            HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "Emp_MPFPlan_View.aspx?EmpMPFID=" + CurID + "&EmpID=" + EmpID.Value);
        else
            HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "EmpTab_Pension_View.aspx?EmpID=" + EmpID.Value);

    }
}
