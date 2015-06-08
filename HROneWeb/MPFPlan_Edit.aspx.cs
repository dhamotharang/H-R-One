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
using HROne.Translation;
using HROne.Lib.Entities;

public partial class MPFPlan_Edit : HROneWebPage
{
    private const string FUNCTION_CODE = "MPF001";
    public Binding binding;
    public DBManager db = EMPFPlan.db;
    public EMPFPlan obj;
    public int CurID = -1;
    

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!WebUtils.CheckAccess(Response, Session, FUNCTION_CODE, WebUtils.AccessLevel.ReadWrite))
            return;
        toolBar.FunctionCode = FUNCTION_CODE;



        binding = new Binding(dbConn, db);
        binding.add(MPFPlanID);
        binding.add(MPFPlanCode);
        binding.add(MPFPlanDesc);

        HROne.ProductLicense productLicense = WebUtils.productLicense(Session);

        DBFilter SchemeFilter = new DBFilter();
        if (productLicense.ProductType != HROne.ProductLicense.ProductLicenseType.HROne)
        {
            HROne.ProductLicense license = WebUtils.productLicense(Session);
            if (!license.HasAutopayMPFFileOthers)
            {
                OR orTrusteeCodeTerms = new OR();
                orTrusteeCodeTerms.add(new Match("MPFSchemeTrusteeCode", "HSBC"));
                orTrusteeCodeTerms.add(new Match("MPFSchemeTrusteeCode", "HangSeng"));
                SchemeFilter.add(orTrusteeCodeTerms);
            }
        }

        {
            string selectedValue = MPFSchemeTrusteeCode.SelectedValue;
            if (selectedValue != null)
                selectedValue = selectedValue.Trim();
            System.Globalization.CultureInfo ci = System.Threading.Thread.CurrentThread.CurrentUICulture;
            WebFormUtils.loadValues(dbConn, MPFSchemeTrusteeCode, EMPFScheme.VLMPFSchemeTrusteeCode, new DBFilter(SchemeFilter), ci, selectedValue, "All");
        }
        if (MPFSchemeTrusteeCode.SelectedIndex != 0)
        {
            SchemeFilter.add(new Match("MPFSchemeTrusteeCode", MPFSchemeTrusteeCode.SelectedValue));
        }
        binding.add(new DropDownVLBinder(db, MPFSchemeID, EMPFScheme.VLMPFScheme, SchemeFilter));
        binding.add(MPFPlanCompanyName);
        binding.add(MPFPlanCompanyAddress);
        binding.add(MPFPlanContactName);
        binding.add(MPFPlanContactNo);
        binding.add(MPFPlanParticipationNo);
        binding.add(new TextBoxXMLNodeBinder(db, MPFPlanPayCenter, "MPFPlanExtendData", MPFPlanPayCenter.ID));
        binding.add(new TextBoxXMLNodeBinder(db, MPFPlanDefaultClassName, "MPFPlanExtendData", MPFPlanDefaultClassName.ID));
        binding.add(new TextBoxXMLNodeBinder(db, MPFPlanEmployerID, "MPFPlanExtendData", MPFPlanEmployerID.ID));
        binding.add(new TextBoxXMLNodeBinder(db, MPFPlanSchemeNo, "MPFPlanExtendData", MPFPlanSchemeNo.ID));
        binding.add(new TextBoxXMLNodeBinder(db, MPFPlanPlanNo, "MPFPlanExtendData", MPFPlanPlanNo.ID));
        binding.add(new TextBoxXMLNodeBinder(db, MPFPlanBOCISequenceNo, "MPFPlanExtendData", MPFPlanBOCISequenceNo.ID));
        binding.add(new TextBoxXMLNodeBinder(db, MPFPlanAIAERPlanNo, "MPFPlanExtendData", MPFPlanAIAERPlanNo.ID));
        binding.add(new DropDownListXMLNodeVLBinder(db, MPFPlanAIAPayFrequency, EPayrollGroup.VLPayGroupFreq, null, "MPFPlanExtendData", MPFPlanAIAPayFrequency.ID));
        binding.add(new TextBoxXMLNodeBinder(db, MPFPlanManulifeSubSchemeNo, "MPFPlanExtendData", HROne.MPFFile.ManulifeMPFFile.MPF_PLAN_XML_SUB_SCHEME_NO));
        binding.add(new TextBoxXMLNodeBinder(db, MPFPlanManulifeGroupNo, "MPFPlanExtendData", HROne.MPFFile.ManulifeMPFFile.MPF_PLAN_XML_GROUP_NO));
        binding.add(new TextBoxXMLNodeBinder(db, MPFPlanManulifeSubGroupNo, "MPFPlanExtendData", HROne.MPFFile.ManulifeMPFFile.MPF_PLAN_XML_SUB_GROUP_NO));

        binding.add(new DropDownVLBinder(db, MPFPlanEmployerRoundingRule, Values.VLRoundingRule));
        binding.add(new DropDownVLBinder(db, MPFPlanEmployerDecimalPlace, Values.VLDecimalPlace));
        binding.add(new DropDownVLBinder(db, MPFPlanEmployeeRoundingRule, Values.VLRoundingRule));
        binding.add(new DropDownVLBinder(db, MPFPlanEmployeeDecimalPlace, Values.VLDecimalPlace));

        binding.init(Request, Session);

        if (!int.TryParse(DecryptedRequest["MPFPlanID"], out CurID))
            CurID = -1;

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
        if (MPFSchemeID.SelectedItem.Text.StartsWith("MT00245")
            || MPFSchemeID.SelectedItem.Text.StartsWith("MT00253")
            || MPFSchemeID.SelectedItem.Text.StartsWith("MT00261")
            || MPFSchemeID.SelectedItem.Text.StartsWith("MT0027A")
            || MPFSchemeID.SelectedItem.Text.StartsWith("MT00512")
            || MPFSchemeID.SelectedItem.Text.StartsWith("MT00555")
            || MPFSchemeID.SelectedItem.Text.StartsWith("MT00520")
            || MPFSchemeID.SelectedItem.Text.StartsWith("MT00563")
            )
        {
            //  HSBC/Hang Seng MPF Seheme
            HSBCMPFPanel.Visible = true;
            BOCIMPFPanel.Visible = false;
            AIAMPFPanel.Visible = false;
            ManulifePanel.Visible = false;
        }
        else if (MPFSchemeID.SelectedItem.Text.StartsWith("MT00091")
                || MPFSchemeID.SelectedItem.Text.StartsWith("MT00105")
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
        else if (MPFSchemeID.SelectedItem.Text.StartsWith("MT00431")
               || MPFSchemeID.SelectedItem.Text.StartsWith("MT00156")
               || MPFSchemeID.SelectedItem.Text.StartsWith("MT00172")
       )
        {
            //  AIA MPF Seheme
            HSBCMPFPanel.Visible = false;
            BOCIMPFPanel.Visible = false;
            AIAMPFPanel.Visible = true;
            ManulifePanel.Visible = false;
        }
        else if (MPFSchemeID.SelectedItem.Text.StartsWith("MT00377")
            || MPFSchemeID.SelectedItem.Text.StartsWith("MT00482")
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
        obj = new EMPFPlan();
        bool isNew = WebFormWorkers.loadKeys(db, obj, DecryptedRequest);
        if (!db.select(dbConn, obj))
            return false;

        if (string.IsNullOrEmpty(obj.MPFPlanEmployerRoundingRule) && string.IsNullOrEmpty(obj.MPFPlanEmployeeRoundingRule))
        {
            obj.MPFPlanEmployerRoundingRule = Values.ROUNDING_RULE_ROUND_TO;
            obj.MPFPlanEmployerDecimalPlace = 2;
            obj.MPFPlanEmployeeRoundingRule = Values.ROUNDING_RULE_ROUND_TO;
            obj.MPFPlanEmployeeDecimalPlace = 2;
        }

        Hashtable values = new Hashtable();
        db.populate(obj, values);
        binding.toControl(values);
        return true;
    }

    protected void Save_Click(object sender, EventArgs e)
    {
        PageErrors errors = PageErrors.getErrors(db, Page.Master);
        errors.clear();

        if (!HSBCMPFPanel.Visible)
        {
            MPFPlanDefaultClassName.Text = string.Empty;
            MPFPlanPayCenter.Text = string.Empty;
            MPFPlanEmployerID.Text = string.Empty;
        }
        else
        {
            if (string.IsNullOrEmpty(MPFPlanDefaultClassName.Text))
                errors.addError(string.Format(HROne.Translation.PageErrorMessage.ERROR_FIELD_REQUIRED, lblDefaultClassNameHeader.Text));
            if (string.IsNullOrEmpty(MPFPlanPayCenter.Text))
                errors.addError(string.Format(HROne.Translation.PageErrorMessage.ERROR_FIELD_REQUIRED, lblPayCenterHeader.Text));
            if (string.IsNullOrEmpty(MPFPlanEmployerID.Text))
                errors.addError(string.Format(HROne.Translation.PageErrorMessage.ERROR_FIELD_REQUIRED, lblEmployerIDHeader.Text));
        }
        if (!BOCIMPFPanel.Visible)
        {
            MPFPlanSchemeNo.Text = string.Empty;
            MPFPlanPlanNo.Text = string.Empty;
        }

        if (!AIAMPFPanel.Visible)
        {
            MPFPlanAIAERPlanNo.Text = string.Empty;
        }
        if (!errors.isEmpty())
            return;

        EMPFPlan c = new EMPFPlan();

        Hashtable values = new Hashtable();
        binding.toValues(values);

        db.validate(errors, values);

        if (!errors.isEmpty())
            return;


        db.parse(values, c);
        if (!AppUtils.checkDuplicate(dbConn, db, c, errors, "MPFPlanCode"))
            return;

        WebUtils.StartFunction(Session, FUNCTION_CODE);
        if (CurID < 0)
        {
            //            Utils.MarkCreate(Session, c);

            db.insert(dbConn, c);
            CurID = c.MPFPlanID;
            //            url = Utils.BuildURL(-1, CurID);
        }
        else
        {
            //            Utils.Mark(Session, c);
            db.update(dbConn, c);
        }
        WebUtils.EndFunction(dbConn);


        HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "MPFPlan_View.aspx?MPFPlanID=" + CurID);


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
    protected void Back_Click(object sender, EventArgs e)
    {
        if (CurID > 0)
            HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "MPFPlan_View.aspx?MPFPlanID=" + CurID);
        else
            HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "MPFPlan_List.aspx");

    }
}
