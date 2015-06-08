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

public partial class ESS_EmpView : HROneWebPage
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
        binding.add(new LabelVLBinder(db, EmpMaritalStatus, Values.VLMaritalStatus));
        binding.add(EmpPlaceOfBirth);
        binding.add(EmpPassportNo);
        binding.add(EmpPassportIssuedCountry);
        binding.add(EmpNationality);
        binding.add(EmpPassportExpiryDate);
        binding.add(EmpHomePhoneNo);
        binding.add(EmpMobileNo);
        binding.add(EmpOfficePhoneNo);
        binding.add(EmpEmail);
        binding.add(EmpInternalEmail);
        binding.add(EmpNoticePeriod);
        binding.add(new LabelVLBinder(db, EmpNoticeUnit, Values.VLEmpUnit));
        binding.add(EmpResAddr);
        binding.add(new LabelVLBinder(db, EmpResAddrAreaCode, Values.VLArea));
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

        if ((ESystemParameter.getParameter(dbConn, ESystemParameter.PARAM_CODE_ESS_FUNCTION_CHANGE_EE_INFO).Equals("N", StringComparison.CurrentCultureIgnoreCase) ? false : true))
        {
            DBFilter RequestEmpFilter = new DBFilter();
            RequestEmpFilter.add(new Match("EmpID", obj.EmpID));
            RequestEmpFilter.add(new Match("EmpRequestType", EEmpRequest.TYPE_EEPROFILE));
            RequestEmpFilter.add(new Match("EmpRequestStatus", "<>", EEmpRequest.STATUS_APPROVED));
            RequestEmpFilter.add(new Match("EmpRequestStatus", "<>", EEmpRequest.STATUS_REJECTED));
            RequestEmpFilter.add(new Match("EmpRequestStatus", "<>", EEmpRequest.STATUS_CANCELLED));
            ArrayList EmpRequestList = EEmpRequest.db.select(dbConn, RequestEmpFilter);
            if (EmpRequestList.Count > 0)
            {
                Edit.Visible = false;
            }
            else
            {
                Edit.Visible = true;
            }
        }
        else
            Edit.Visible = false;
        return true;
    }

    protected void Edit_Click(object sender, EventArgs e)
    {
        HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "~/ESS_EmpEdit.aspx");
    }
}
