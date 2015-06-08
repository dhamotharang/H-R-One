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

public partial class Emp_Spouse_View : HROneWebPage
{
    private const string FUNCTION_CODE = "PER003";
    public Binding binding;
    public DBManager db = EEmpSpouse.db;
    public EEmpSpouse obj;
    public int CurID = -1;
    public int CurEmpID = -1;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!WebUtils.CheckAccess(Response, Session, FUNCTION_CODE, WebUtils.AccessLevel.Read))
            return;
        toolBar.FunctionCode = FUNCTION_CODE;

        binding = new Binding(dbConn, db);
        binding.add(EmpSpouseID);
        binding.add(EmpID);
        binding.add(EmpSpouseSurname);
        binding.add(EmpSpouseOtherName);
        binding.add(EmpSpouseDateOfBirth);
        binding.add(EmpSpouseChineseName);
        binding.add(EmpSpouseHKID);
        binding.add(EmpSpousePassportNo);
        binding.add(EmpSpousePassportIssuedCountry);
        // Start 0000142, KuangWei, 2014-12-18
        binding.add(new LabelVLBinder(db, EmpGender, Values.VLGender));
        binding.add(new LabelVLBinder(db, EmpIsMedicalSchemaInsured, Values.VLTrueFalseYesNo));
        binding.add(EmpMedicalEffectiveDate);
        binding.add(EmpMedicalExpiryDate);
        // End 0000142, KuangWei, 2014-12-18
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
                loadObject();
            else
                toolBar.DeleteButton_Visible = false;
        }
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

        // Start 0000142, KuangWei, 2014-12-20
        double age = AppUtils.GetAge(obj.EmpSpouseDateOfBirth);
        if (!double.IsNaN(age))
            lblAge.Text = HROne.CommonLib.GenericRoundingFunctions.RoundingDown(age, 0, 3).ToString("0");
        else
            lblAge.Text = string.Empty;
        // End 0000142, KuangWei, 2014-12-20

        return true;
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
    protected void Edit_Click(object sender, EventArgs e)
    {
        HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "Emp_Spouse_Edit.aspx?EmpSpouseID=" + EmpSpouseID.Value + "&EmpID=" + EmpID.Value);
    }

    protected void Back_Click(object sender, EventArgs e)
    {
        HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "EmpTab_Family_View.aspx?EmpID=" + EmpID.Value);
    }

    // Start 0000142, KuangWei, 2014-12-18
    public int CalculateAgeCorrect(DateTime birthDate, DateTime now)
    {
        int age = now.Year - birthDate.Year;
        if (now.Month < birthDate.Month || (now.Month == birthDate.Month && now.Day < birthDate.Day)) age--;
        return age;
    }
    // End 0000142, KuangWei, 2014-12-18
}
