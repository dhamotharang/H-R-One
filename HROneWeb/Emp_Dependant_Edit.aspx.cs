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

public partial class Emp_Dependant_Edit : HROneWebPage
{
    private const string FUNCTION_CODE = "PER003";
    public Binding binding;
    public DBManager db = EEmpDependant.db;
    public EEmpDependant obj;
    public int CurID = -1;
    public int CurEmpID = -1;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!WebUtils.CheckAccess(Response, Session, FUNCTION_CODE, WebUtils.AccessLevel.ReadWrite))
            return;
        toolBar.FunctionCode = FUNCTION_CODE;

        bool isAutoGenerateHKIDCheckDigit = ESystemParameter.getParameter(dbConn, ESystemParameter.PARAM_CODE_HKID_CHECKDIGIT_AUTO_GENERATE).Equals("Y", StringComparison.CurrentCultureIgnoreCase) ? true : false;

        binding = new Binding(dbConn, db);
        binding.add(EmpDependantID);
        binding.add(EmpID);
        binding.add(EmpDependantSurname);
        binding.add(EmpDependantOtherName);
        binding.add(EmpDependantChineseName);
        binding.add(new TextBoxBinder(db, EmpDependantDateOfBirth.TextBox, EmpDependantDateOfBirth.ID));
        binding.add(new HKIDBinder(db, EmpDependantHKID, EmpDependantHKID_Digit, isAutoGenerateHKIDCheckDigit));
        binding.add(new DropDownVLBinder(db,EmpDependantGender, Values.VLGender));
        binding.add(new DropDownVLBinder(db, EmpDependantRelationship, Values.VLDependantRelationship));
        binding.add(EmpDependantPassportNo);
        binding.add(EmpDependantPassportIssuedCountry);
        //Start 0000190, Miranda, 2015-04-30
        binding.add(new CheckBoxBinder(db, EmpDependantMedicalSchemeInsured));
        binding.add(new TextBoxBinder(db, EmpDependantMedicalEffectiveDate.TextBox, EmpDependantMedicalEffectiveDate.ID));
        binding.add(new TextBoxBinder(db, EmpDependantExpiryDate.TextBox, EmpDependantExpiryDate.ID));
        //End 0000190, Miranda, 2015-04-30
        binding.init(Request, Session);

        if (!int.TryParse(DecryptedRequest["EmpDependantID"], out CurID))
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
    protected bool loadObject() 
    {
	    obj=new EEmpDependant();
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
        //Start 0000190, Miranda, 2015-04-30
        isMedicalSchemeInsured(EmpDependantMedicalSchemeInsured.Checked);
        //End 0000190, Miranda, 2015-04-30
        return true;
    }

    protected void Save_Click(object sender, EventArgs e)
    {
        EEmpDependant c = new EEmpDependant();

        Hashtable values = new Hashtable();
        binding.toValues(values);

        PageErrors errors = PageErrors.getErrors(db, Page.Master);
        errors.clear();


        db.validate(errors, values);

        if (!errors.isEmpty())
            return;


        db.parse(values, c);
        WebUtils.StartFunction(Session, FUNCTION_CODE, c.EmpID);

        if (CurID < 0)
        {
//            Utils.MarkCreate(Session, c);

            db.insert(dbConn, c);
            CurID = c.EmpDependantID;
//            url = Utils.BuildURL(-1, CurID);
        }
        else
        {
//            Utils.Mark(Session, c);
            db.update(dbConn, c);
        }
        WebUtils.EndFunction(dbConn);


        HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "Emp_Dependant_View.aspx?EmpDependantID="+CurID+ "&EmpID=" + c.EmpID);


    }
    protected void Delete_Click(object sender, EventArgs e)
    {

        EEmpDependant c = new EEmpDependant();
        c.EmpDependantID= CurID;
        db.select(dbConn, c);
        WebUtils.StartFunction(Session, FUNCTION_CODE, c.EmpID);
        db.delete(dbConn, c);
        WebUtils.EndFunction(dbConn);
        HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "EmpTab_Family_View.aspx?EmpID=" + EmpID.Value);
    }

    protected void Back_Click(object sender, EventArgs e)
    {
        if (CurID > 0)
            HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "Emp_Dependant_View.aspx?EmpDependantID=" + CurID + "&EmpID=" + EmpID.Value);
        else
            HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "EmpTab_Family_View.aspx?EmpID=" + EmpID.Value);

    }

    //Start 0000190, Miranda, 2015-04-30
    protected void EmpDependantMedicalSchemeInsured_Changed(object sender, EventArgs e)
    {
         isMedicalSchemeInsured(EmpDependantMedicalSchemeInsured.Checked);
    }

    protected void isMedicalSchemeInsured(bool b)
    {
        if (b)
        {
            EmpDependantMedicalEffectiveDate.Enabled = true;
            EmpDependantExpiryDate.Enabled = true;
        }
        else {
            EmpDependantMedicalEffectiveDate.Enabled = false;
            EmpDependantMedicalEffectiveDate.Value = null;
            EmpDependantExpiryDate.Enabled = false;
            EmpDependantExpiryDate.Value = null;
        }
    }
    //End 0000190, Miranda, 2015-04-30
}
