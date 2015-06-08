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

public partial class MPFPlan_View : HROneWebPage
{
    private const string FUNCTION_CODE = "MPF001";
    public Binding binding;
    public DBManager db = EMPFPlan.db;
    public EMPFPlan obj;
    public int CurID = -1;
    

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!WebUtils.CheckAccess(Response, Session, FUNCTION_CODE, WebUtils.AccessLevel.Read))
            return;
        toolBar.FunctionCode = FUNCTION_CODE;

        

        binding = new Binding(dbConn, db);
        binding.add(MPFPlanID);
        binding.add(MPFPlanCode);
        binding.add(MPFPlanDesc);
        binding.add(new LabelVLBinder(db, MPFSchemeID, EMPFScheme.VLMPFScheme));
        binding.add(MPFPlanCompanyName);
        binding.add(MPFPlanCompanyAddress);
        binding.add(MPFPlanContactName);
        binding.add(MPFPlanContactNo);
        binding.add(MPFPlanParticipationNo);
        binding.add(new LabelXMLNodeBinder(db, MPFPlanPayCenter, "MPFPlanExtendData", MPFPlanPayCenter.ID));
        binding.add(new LabelXMLNodeBinder(db, MPFPlanDefaultClassName, "MPFPlanExtendData", MPFPlanDefaultClassName.ID));
        binding.add(new LabelXMLNodeBinder(db, MPFPlanEmployerID, "MPFPlanExtendData", MPFPlanEmployerID.ID));

        binding.add(new LabelXMLNodeBinder(db, MPFPlanSchemeNo, "MPFPlanExtendData", MPFPlanSchemeNo.ID));
        binding.add(new LabelXMLNodeBinder(db, MPFPlanPlanNo, "MPFPlanExtendData", MPFPlanPlanNo.ID));
        binding.add(new LabelXMLNodeBinder(db, MPFPlanBOCISequenceNo, "MPFPlanExtendData", MPFPlanBOCISequenceNo.ID));
        binding.add(new LabelXMLNodeBinder(db, MPFPlanAIAERPlanNo, "MPFPlanExtendData", MPFPlanAIAERPlanNo.ID));
        binding.add(new LabelXMLNodeVLBinder(db, MPFPlanAIAPayFrequency, EPayrollGroup.VLPayGroupFreq, null, "MPFPlanExtendData", MPFPlanAIAPayFrequency.ID));

        binding.add(new LabelXMLNodeBinder(db, MPFPlanManulifeSubSchemeNo, "MPFPlanExtendData", HROne.MPFFile.ManulifeMPFFile.MPF_PLAN_XML_SUB_SCHEME_NO));
        binding.add(new LabelXMLNodeBinder(db, MPFPlanManulifeGroupNo, "MPFPlanExtendData", HROne.MPFFile.ManulifeMPFFile.MPF_PLAN_XML_GROUP_NO));
        binding.add(new LabelXMLNodeBinder(db, MPFPlanManulifeSubGroupNo, "MPFPlanExtendData", HROne.MPFFile.ManulifeMPFFile.MPF_PLAN_XML_SUB_GROUP_NO));

        binding.add(new LabelVLBinder(db, MPFPlanEmployerRoundingRule, Values.VLRoundingRule));
        binding.add(MPFPlanEmployerDecimalPlace);
        binding.add(new LabelVLBinder(db, MPFPlanEmployeeRoundingRule, Values.VLRoundingRule));
        binding.add(MPFPlanEmployeeDecimalPlace);

        binding.init(Request, Session);

        if (!int.TryParse(DecryptedRequest["MPFPlanID"], out CurID))
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

    protected void Page_PreRender(object sender, EventArgs e)
    {
        if (MPFSchemeID.Text.StartsWith("MT00245")
            || MPFSchemeID.Text.StartsWith("MT00253")
            || MPFSchemeID.Text.StartsWith("MT00261")
            || MPFSchemeID.Text.StartsWith("MT0027A")
            || MPFSchemeID.Text.StartsWith("MT00512")
            || MPFSchemeID.Text.StartsWith("MT00555")
            || MPFSchemeID.Text.StartsWith("MT00520")
            || MPFSchemeID.Text.StartsWith("MT00563")
            )
        {
            //  HSBC/Hang Seng MPF Seheme
            HSBCMPFPanel.Visible = true;
            BOCIMPFPanel.Visible = false;
            AIAMPFPanel.Visible = false;
            ManulifePanel.Visible = false;
        }
        else if (MPFSchemeID.Text.StartsWith("MT00091")
                || MPFSchemeID.Text.StartsWith("MT00105")
                )
        {
            //  BOCI MPF Seheme
            HSBCMPFPanel.Visible = false;
            BOCIMPFPanel.Visible = true;
            AIAMPFPanel.Visible = false;
            ManulifePanel.Visible = false;
            if (MPFPlanBOCISequenceNo.Text.Equals(string.Empty))
                MPFPlanBOCISequenceNo.Text = "1";
        }
        else if (MPFSchemeID.Text.StartsWith("MT00431")
               || MPFSchemeID.Text.StartsWith("MT00156")
               || MPFSchemeID.Text.StartsWith("MT00172")
       )
        {
            //  AIA MPF Seheme
            HSBCMPFPanel.Visible = false;
            BOCIMPFPanel.Visible = false;
            AIAMPFPanel.Visible = true;
            ManulifePanel.Visible = false;
        }
        else if (MPFSchemeID.Text.StartsWith("MT00377")
            || MPFSchemeID.Text.StartsWith("MT00482")
        )
        {
            //  Manulife MPF Seheme
            HSBCMPFPanel.Visible = false;
            BOCIMPFPanel.Visible = false;
            AIAMPFPanel.Visible = false;
            ManulifePanel.Visible = true;
        }

        else
        {
            HSBCMPFPanel.Visible = false;
            BOCIMPFPanel.Visible = false;
            AIAMPFPanel.Visible = false;
            ManulifePanel.Visible = false;
        }

    }

    protected bool loadObject() 
    {
	    obj=new EMPFPlan();
	    bool isNew=WebFormWorkers.loadKeys(db, obj, DecryptedRequest);
	    if(!db.select(dbConn, obj))
		    return false;

        if (string.IsNullOrEmpty(obj.MPFPlanEmployerRoundingRule) && string.IsNullOrEmpty(obj.MPFPlanEmployeeRoundingRule))
        {
            obj.MPFPlanEmployerRoundingRule = Values.ROUNDING_RULE_ROUND_TO;
            obj.MPFPlanEmployerDecimalPlace = 2;
            obj.MPFPlanEmployeeRoundingRule = Values.ROUNDING_RULE_ROUND_TO;
            obj.MPFPlanEmployeeDecimalPlace = 2;
        }

	    Hashtable values=new Hashtable();
	    db.populate(obj,values);
	    binding.toControl(values);
        return true;
    }

    protected void Delete_Click(object sender, EventArgs e)
    {
        PageErrors errors = PageErrors.getErrors(db, Page.Master);
        errors.clear();

        EMPFPlan o = new EMPFPlan();
        o.MPFPlanID = CurID;
        db.select(dbConn, o);
        DBFilter empMPFFilter = new DBFilter();
        empMPFFilter.add(new Match("MPFPlanID", o.MPFPlanID));
        empMPFFilter.add("empid", true);
        ArrayList empMPFList = EEmpMPFPlan.db.select(dbConn, empMPFFilter);
        if (empMPFList.Count > 0)
        {
            int curEmpID = 0;
            errors.addError(string.Format(HROne.Translation.PageErrorMessage.ERROR_CODE_USED_BY_EMPLOYEE, new string[] { HROne.Common.WebUtility.GetLocalizedString("MPF Plan Code"), o.MPFPlanCode }));
            foreach (EEmpMPFPlan empMPFPlan in empMPFList)
            {
                EEmpPersonalInfo empInfo = new EEmpPersonalInfo();
                empInfo.EmpID = empMPFPlan.EmpID;
                if (EEmpPersonalInfo.db.select(dbConn, empInfo))
                    if (curEmpID != empMPFPlan.EmpID)
                    {
                        errors.addError("- " + empInfo.EmpNo + ", " + empInfo.EmpEngFullName);
                        curEmpID = empMPFPlan.EmpID;
                    }
                    else
                        EEmpMPFPlan.db.delete(dbConn, empMPFPlan);

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
        HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "MPFPlan_List.aspx");
    }
    protected void Edit_Click(object sender, EventArgs e)
    {
        HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "MPFPlan_Edit.aspx?MPFPlanID=" + CurID);
    }

    protected void Back_Click(object sender, EventArgs e)
    {
        HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "MPFPlan_List.aspx");
    }
}
