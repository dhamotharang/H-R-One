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

public partial class ESS_Edit : HROneWebPage
{
    public Binding binding;
    public DBManager db = EEmpPersonalInfo.db;
    public EEmpPersonalInfo obj;
    public int CurID = -1;



    protected void Page_Load(object sender, EventArgs e)
    {
        if (!WebUtils.CheckAccess(Response, Session))
            return;
        binding = new Binding(dbConn, db);
        binding.add(EmpID);
        binding.add(EmpNo);
        binding.add(EmpAlias);
        binding.add(EmpEngSurname);
        binding.add(EmpEngOtherName);
        binding.add(EmpHKID);
        binding.add(EmpChiFullName);
        binding.add(new LabelVLBinder(db, EmpGender, Values.VLGender));
        binding.add(EmpDateOfBirth);
        binding.add(EmpDateOfJoin);
        binding.add(EmpServiceDate);
        binding.add(new DropDownVLBinder(db, EmpMaritalStatus, Values.VLMaritalStatus));
        // Start 0000092, KuangWei, 2014-10-13
        //binding.add(EmpPlaceOfBirth);
        binding.add(EmpPassportNo);
        //binding.add(EmpPassportIssuedCountry);
        binding.add(new TextBoxBinder(db, EmpPassportExpiryDate.TextBox, EmpPassportExpiryDate.ID));
        //binding.add(EmpNationality);
        binding.add(new DropDownVLBinder(db, EmpPlaceOfBirthID, EPlaceOfBirth.VLPlaceOfBirth));
        binding.add(new DropDownVLBinder(db, EmpPassportIssuedCountryID, EIssueCountry.VLCountry));
        binding.add(new DropDownVLBinder(db, EmpNationalityID, ENationality.VLNationality));
        // End 0000092, KuangWei, 2014-10-13
        binding.add(EmpHomePhoneNo);
        binding.add(EmpMobileNo);
        binding.add(EmpOfficePhoneNo);
        binding.add(EmpEmail);
        binding.add(EmpInternalEmail);
        binding.add(EmpNoticePeriod);
        binding.add(new LabelVLBinder(db, EmpNoticeUnit, Values.VLEmpUnit));
        binding.add(EmpResAddr);
        binding.add(new DropDownVLBinder(db, EmpResAddrAreaCode, Values.VLArea));
        binding.add(EmpCorAddr);
        binding.add(EmpProbaPeriod);
        binding.add(new LabelVLBinder(db, EmpProbaUnit, Values.VLEmpUnit));
        binding.add(EmpProbaLastDate);

        binding.init(Request, Session);

        HROne.Common.WebUtility.WebControlsLocalization(this, this.Controls);


        EESSUser user = WebUtils.GetCurUser(Session);
        if (user != null)
        {
            CurID = user.EmpID;
        }


        if (!Page.IsPostBack)
        {

            if (CurID > 0)
            {
                loadObject();
            }
            else
            {

            }
        }
        
    }

    protected bool loadObject()
    {
        obj = new EEmpPersonalInfo();
        obj.EmpID = CurID;
        if (!db.select(dbConn, obj))
            return false;

        Hashtable values = new Hashtable();
        db.populate(obj, values);
        binding.toControl(values);
        return true;
    }
    protected void Save_Click(object sender, EventArgs e)
    {

        DateTime createDate = DateTime.Now;
        EEmpPersonalInfo c = new EEmpPersonalInfo();
        EEmpRequest EmpRequest = new EEmpRequest();
                

        Hashtable values = new Hashtable();
        binding.toValues(values);

        PageErrors errors = PageErrors.getErrors(db, Page);
        errors.clear();


        db.validate(errors, values);

        if (!errors.isEmpty())
            return;


        db.parse(values, c);

        if (!errors.isEmpty())
            return;

        ERequestEmpPersonalInfo RequestEmpProfile = new ERequestEmpPersonalInfo();
        // Start 0000092, KuangWei, 2014-10-17
        if (c.EmpNationalityID > 0)
        {
            ENationality m_nationality = new ENationality();
            m_nationality.NationalityID = c.EmpNationalityID;
            if (ENationality.db.select(dbConn, m_nationality))
            {
                c.EmpNationality = m_nationality.NationalityDesc;
            }
        }else
            c.EmpNationality = "";

        if (c.EmpPlaceOfBirthID > 0)
        {
            EPlaceOfBirth m_placeOfBirth = new EPlaceOfBirth();
            m_placeOfBirth.PlaceOfBirthID = c.EmpPlaceOfBirthID;
            if (EPlaceOfBirth.db.select(dbConn, m_placeOfBirth))
            {
                c.EmpPlaceOfBirth = m_placeOfBirth.PlaceOfBirthDesc;
            }
        }else
            c.EmpPlaceOfBirth = "";

        if (c.EmpPassportIssuedCountryID > 0)
        {
            EIssueCountry m_issueCountry = new EIssueCountry();
            m_issueCountry.CountryID = c.EmpPassportIssuedCountryID;
            if (EIssueCountry.db.select(dbConn, m_issueCountry))
            {
                c.EmpPassportIssuedCountry = m_issueCountry.CountryDesc;
            }
        }else
            c.EmpPassportIssuedCountry = "";


        RequestEmpProfile.RequestEmpPlaceOfBirth = c.EmpPlaceOfBirth;
        RequestEmpProfile.RequestEmpPlaceOfBirthID = c.EmpPlaceOfBirthID;
        RequestEmpProfile.RequestEmpPassportIssuedCountryID = c.EmpPassportIssuedCountryID;
        RequestEmpProfile.RequestEmpNationalityID = c.EmpNationalityID;
        // End 0000092, KuangWei, 2014-10-17
        RequestEmpProfile.EmpID = c.EmpID;
        RequestEmpProfile.RequestEmpAlias = c.EmpAlias;
        RequestEmpProfile.RequestEmpMaritalStatus = c.EmpMaritalStatus;
        RequestEmpProfile.RequestEmpPassportNo = c.EmpPassportNo;
        RequestEmpProfile.RequestEmpPassportIssuedCountry = c.EmpPassportIssuedCountry;
        RequestEmpProfile.RequestEmpPassportExpiryDate = c.EmpPassportExpiryDate;
        RequestEmpProfile.RequestEmpNationality = c.EmpNationality;
        RequestEmpProfile.RequestEmpHomePhoneNo = c.EmpHomePhoneNo;
        RequestEmpProfile.RequestEmpMobileNo = c.EmpMobileNo;
        RequestEmpProfile.RequestEmpOfficePhoneNo = c.EmpOfficePhoneNo;
        RequestEmpProfile.RequestEmpEmail = c.EmpEmail;
        RequestEmpProfile.RequestEmpResAddr = c.EmpResAddr;
        RequestEmpProfile.RequestEmpResAddrAreaCode = c.EmpResAddrAreaCode;
        RequestEmpProfile.RequestEmpCorAdd = c.EmpCorAddr;

        try
        {
            ESSAuthorizationProcess authorization = new ESSAuthorizationProcess(dbConn);
            authorization.SubmitEmployeeInfoChange(RequestEmpProfile);
        }
        catch (Exception ex)
        {
            errors.addError(ex.Message);

        }

        if (!errors.isEmpty())
            return;
        HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "~/ESS_EmpRequestStatus.aspx");


    }
}
