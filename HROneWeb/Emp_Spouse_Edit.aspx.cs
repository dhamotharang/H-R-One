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

public partial class Emp_Spouse_Edit : HROneWebPage
{
    private const string FUNCTION_CODE = "PER003";
    public Binding binding;
    public DBManager db = EEmpSpouse.db;
    public EEmpSpouse obj;
    public int CurID = -1;
    public int CurEmpID = -1;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!WebUtils.CheckAccess(Response, Session, FUNCTION_CODE, WebUtils.AccessLevel.ReadWrite))
            return;
        toolBar.FunctionCode = FUNCTION_CODE;

        bool isAutoGenerateHKIDCheckDigit = ESystemParameter.getParameter(dbConn, ESystemParameter.PARAM_CODE_HKID_CHECKDIGIT_AUTO_GENERATE).Equals("Y", StringComparison.CurrentCultureIgnoreCase) ? true : false;

        binding = new Binding(dbConn, db);
        binding.add(EmpSpouseID);
        binding.add(EmpID);
        binding.add(EmpSpouseSurname);
        binding.add(EmpSpouseOtherName);
        binding.add(EmpSpouseChineseName);
        binding.add(new TextBoxBinder(db, EmpSpouseDateOfBirth.TextBox, EmpSpouseDateOfBirth.ID));
        binding.add(new HKIDBinder(db, EmpSpouseHKID, EmpSpouseHKID_Digit, isAutoGenerateHKIDCheckDigit));
        binding.add(EmpSpousePassportNo);
        binding.add(EmpSpousePassportIssuedCountry);
        // Start 0000142, KuangWei, 2014-12-20
        binding.add(new DropDownVLBinder(db, EmpGender, Values.VLGender));
        binding.add(new CheckBoxBinder(db, EmpIsMedicalSchemaInsured));
        binding.add(new TextBoxBinder(db, EmpMedicalEffectiveDate.TextBox, EmpMedicalEffectiveDate.ID));
        binding.add(new TextBoxBinder(db, EmpMedicalExpiryDate.TextBox, EmpMedicalExpiryDate.ID));
        // End 0000142, KuangWei, 2014-12-20
        binding.init(Request, Session);

        if (!int.TryParse(DecryptedRequest["EmpSpouseID"], out CurID))
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

        // Start 0000142, KuangWei, 2014-12-20
        if (EmpIsMedicalSchemaInsured.Checked)
        {
            EmpMedicalEffectiveDate.Enabled = true;
            EmpMedicalExpiryDate.Enabled = true;
        }
        else 
        {
            EmpMedicalEffectiveDate.Enabled = false;
            EmpMedicalExpiryDate.Enabled = false;
        }
        // End 0000142, KuangWei, 2014-12-20
    }
    protected bool loadObject() 
    {
	    obj=new EEmpSpouse();
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
        EEmpSpouse c = new EEmpSpouse();

        Hashtable values = new Hashtable();
        binding.toValues(values);

        PageErrors errors = PageErrors.getErrors(db, Page.Master);
        errors.clear();

        db.validate(errors, values);

        if (EmpIsMedicalSchemaInsured.Checked)
        {
            if (string.IsNullOrEmpty(EmpMedicalEffectiveDate.TextBox.Text))
            {
                errors.addError("Please input Medical Effective Date");
            }
            if (string.IsNullOrEmpty(EmpMedicalExpiryDate.TextBox.Text))
            {
                errors.addError("Please input Expiry Date");
            }
        }


        if (!errors.isEmpty())
            return;


        db.parse(values, c);


        WebUtils.StartFunction(Session, FUNCTION_CODE, c.EmpID);
        if (CurID < 0)
        {
            DBFilter filter = new DBFilter();
            filter.add(new Match("EmpID", c.EmpID));
            ArrayList list = db.select(dbConn, filter);
            if (list.Count > 0)
            {
                c.EmpSpouseID = ((EEmpSpouse)list[0]).EmpSpouseID;
                db.update(dbConn, c);
            }
            else
            {
                db.insert(dbConn, c);
            }
            CurID = c.EmpSpouseID;
        }
        else
        {
//            Utils.Mark(Session, c);
            db.update(dbConn, c);
        }
        WebUtils.EndFunction(dbConn);


        HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "Emp_Spouse_View.aspx?EmpSpouseID="+CurID+ "&EmpID=" + c.EmpID);


    }
    protected void Delete_Click(object sender, EventArgs e)
    {

        EEmpSpouse c = new EEmpSpouse();
        c.EmpSpouseID = CurID;
        db.select(dbConn, c);
        WebUtils.StartFunction(Session, FUNCTION_CODE, c.EmpID);
        db.delete(dbConn, c);
        WebUtils.EndFunction(dbConn);
        HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "EmpTab_Family_View.aspx?EmpID=" + EmpID.Value);
    }

    protected void Back_Click(object sender, EventArgs e)
    {
        if (CurID > 0)
            HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "Emp_Spouse_View.aspx?EmpSpouseID=" + CurID + "&EmpID=" + EmpID.Value);
        else
            HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "EmpTab_Family_View.aspx?EmpID=" + EmpID.Value);

    }

    // Start 0000142, KuangWei, 2014-12-20
    protected void IsMedicalInsured_OnCheckedChanged(object sender, EventArgs e)
    {
        if (EmpIsMedicalSchemaInsured.Checked)
        {
            EmpMedicalEffectiveDate.Enabled = true;
            EmpMedicalExpiryDate.Enabled = true;
        }
        else
        {
            EmpMedicalEffectiveDate.TextBox.Text = "";
            EmpMedicalExpiryDate.TextBox.Text = "";
            EmpMedicalEffectiveDate.Enabled = false;
            EmpMedicalExpiryDate.Enabled = false;
        }
    }
    // End 0000142, KuangWei, 2014-12-20
}
