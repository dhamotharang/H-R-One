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
using HROne.Lib.Entities;
using HROne.Translation;

public partial class Emp_Beneficiaries_Edit : HROneWebPage
{
    private const string FUNCTION_CODE = "PER021";
    protected SearchBinding sbinding;
    public Binding binding;
    public DBManager db = EEmpBeneficiaries.db;
    public DBManager empDB= EEmpPersonalInfo.db;
    public EEmpBeneficiaries obj;
    public int CurID = -1;
    public int CurEmpID = -1;
    public Hashtable CurEmpBeneficiariesGroups = new Hashtable();
    public Hashtable CurRanks = new Hashtable();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!WebUtils.CheckAccess(Response, Session, FUNCTION_CODE, WebUtils.AccessLevel.ReadWrite))
            return;
        toolBar.FunctionCode = FUNCTION_CODE;

        bool isAutoGenerateHKIDCheckDigit = ESystemParameter.getParameter(dbConn, ESystemParameter.PARAM_CODE_HKID_CHECKDIGIT_AUTO_GENERATE).Equals("Y", StringComparison.CurrentCultureIgnoreCase) ? true : false;

        binding = new Binding(dbConn, db);
        binding.add(EmpBeneficiariesID);
        binding.add(EmpID);
        binding.add(EmpBeneficiariesName);
        binding.add(EmpBeneficiariesShare);
        binding.add(new HKIDBinder(db, EmpBeneficiariesHKID, EmpBeneficiariesHKID_Digit, isAutoGenerateHKIDCheckDigit));
        binding.add(EmpBeneficiariesRelation);
        binding.add(EmpBeneficiariesAddress);
        binding.add(EmpBeneficiariesDistrict);
        // Start 0000139, Miranda, 2014-12-20
        binding.add(new DropDownVLBinder(db, EmpBeneficiariesArea, Values.VLArea));
        // End 0000139, Miranda, 2014-12-20
        binding.add(EmpBeneficiariesCountry);
        
        binding.init(Request, Session);

        HROne.ProductLicense productLicense = WebUtils.productLicense(Session);
        if (productLicense.ProductType != HROne.ProductLicense.ProductLicenseType.HROne)
        {
            EmpBeneficiariesHKID.MaxLength = 8;
        }

        if (!int.TryParse(DecryptedRequest["EmpBeneficiariesID"], out CurID))
            CurID = -1;

        if (!int.TryParse(DecryptedRequest["EmpID"], out CurEmpID))
            CurEmpID = -1;

        HROne.Common.WebUtility.WebControlsLocalization(this, this.Controls);

        EmpID.Value = CurEmpID.ToString();
        if (!Page.IsPostBack)
        {
            if (CurID > 0)
            {
                loadObject();
            }
            else
            {
                toolBar.DeleteButton_Visible = false;
                // Start 0000139, Miranda, 2014-12-20
                EEmpPersonalInfo empObj = new EEmpPersonalInfo();
                DBFilter filter = new DBFilter();
                if (CurEmpID > 0)
                {
                    filter.add(new Match("EmpID", CurEmpID));
                    ArrayList empInfoList = empDB.select(dbConn, filter);
                    if (empInfoList.Count > 0)
                    {
                        empObj = (EEmpPersonalInfo)empInfoList[0];
                        EmpBeneficiariesAddress.Text = empObj.EmpResAddr;
                        EmpBeneficiariesArea.SelectedValue = empObj.EmpResAddrAreaCode;
                    }
                }
                // End 0000139, Miranda, 2014-12-20
            }
        }
    }

    protected bool loadObject() 
    {
	    obj=new EEmpBeneficiaries();
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
        EEmpBeneficiaries c = new EEmpBeneficiaries();

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
            db.insert(dbConn, c);
            CurID = c.EmpBeneficiariesID;
        }
        else
        {
            db.update(dbConn, c);
        }
        WebUtils.EndFunction(dbConn);

        HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "Emp_Beneficiaries_View.aspx?EmpBeneficiariesID=" + CurID + "&EmpID=" + c.EmpID);

    }

    protected void Delete_Click(object sender, EventArgs e)
    {
        PageErrors errors = PageErrors.getErrors(db, Page.Master);
        errors.clear();

        EEmpBeneficiaries obj = new EEmpBeneficiaries();
        obj.EmpBeneficiariesID = CurID;
        if (EEmpBeneficiaries.db.select(dbConn, obj))
        {
            WebUtils.StartFunction(Session, FUNCTION_CODE, obj.EmpID);
            db.delete(dbConn, obj);
            WebUtils.EndFunction(dbConn);
        }
        HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "EmpTab_Beneficiaries_View.aspx?EmpID=" + EmpID.Value);
    }

    protected void Back_Click(object sender, EventArgs e)
    {
        if (CurID > 0)
            HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "Emp_Beneficiaries_View.aspx?EmpBeneficiariesID=" + EmpBeneficiariesID.Value + "&EmpID=" + EmpID.Value);
        else
            HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "EmpTab_Beneficiaries_View.aspx?EmpID="+EmpID.Value);

    }
}
