using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using HROne.DataAccess;
//using perspectivemind.validation;
using HROne.Lib.Entities;

public partial class Emp_Request_EmpInfo : HROneWebControl
{
    protected DBManager db = ERequestEmpPersonalInfo.db;
    protected DBManager EmpDb = EEmpPersonalInfo.db;
    protected Binding binding, binding2;
    protected ERequestEmpPersonalInfo RequestObj;
    protected EEmpPersonalInfo EmpObj;
    public static int CurID, RequestID = -1;

    public const string firstsubject = "First Subject";
    public const string firstbody = "First Body";
    public const string secondsubject = "second Subject";
    public const string secondbody = "second Body";

    protected void Page_Load(object sender, EventArgs e)
    {
        PreRender += new EventHandler(Emp_Request_EmpInfo_PreRender);

        binding = new Binding(dbConn, db);
        binding2 = new Binding(dbConn, EmpDb);
        binding.add(RequestEmpID);
        binding.add(RequestEmpAlias);
        binding.add(new LabelVLBinder(db, RequestEmpMaritalStatus, Values.VLMaritalStatus));
        binding.add(RequestEmpPassportNo);
        binding.add(RequestEmpPassportIssuedCountry);
        binding.add(RequestEmpPassportExpiryDate);
        binding.add(RequestEmpNationality);
        binding.add(RequestEmpHomePhoneNo);
        binding.add(RequestEmpMobileNo);
        binding.add(RequestEmpOfficePhoneNo);
        binding.add(RequestEmpEmail);
        binding.add(RequestEmpResAddr);
        binding.add(new LabelVLBinder(db, RequestEmpResAddrAreaCode, Values.VLArea));
        binding.add(RequestEmpCorAdd);
        binding.add(RequestEmpCreateDate);
        binding2.add(EmpNo);
        binding2.add(EmpEngSurname);
        binding2.add(EmpEngOtherName);
        binding2.add(EmpHKID);
        binding2.add(EmpChiFullName);
        binding2.add(new LabelVLBinder(EmpDb, EmpGender, Values.VLGender));
        binding2.add(EmpDateOfBirth);
        binding2.add(EmpDateOfJoin);
        binding2.add(EmpServiceDate);
        // Start 0000092, KuangWei, 2014-10-13
        binding.add(RequestEmpPlaceOfBirth);
        // End 0000092, KuangWei, 2014-10-13

        binding.init(Request, Session);
        binding2.init(Request, Session);

    }
    void Emp_Request_EmpInfo_PreRender(object sender, EventArgs e)
    {
            loadObject();
    }
    protected bool loadObject()
    {
        RequestObj = new ERequestEmpPersonalInfo();
        RequestObj.RequestEmpID = RequestID;
        if (!db.select(dbConn, RequestObj))
            return false;

        Hashtable values = new Hashtable();
        db.populate(RequestObj, values);
        binding.toControl(values);
        

        EmpObj = new EEmpPersonalInfo();
        EmpObj.EmpID = RequestObj.EmpID;
        if (!EmpDb.select(dbConn, EmpObj))
            return false;

        Hashtable values2 = new Hashtable();
        EmpDb.populate(EmpObj, values2);
        binding2.toControl(values2);

        string DeletedTextValue = "(" + "Deleted" + ")";
        if (EmpObj.EmpAlias.ToString() != RequestObj.RequestEmpAlias.ToString())
        {
            RequestEmpAlias.ForeColor = System.Drawing.Color.Red;
            if (string.IsNullOrEmpty(RequestObj.RequestEmpAlias))
                RequestEmpAlias.Text = DeletedTextValue;
        }
        if (EmpObj.EmpMaritalStatus.ToString() != RequestObj.RequestEmpMaritalStatus.ToString())
        {
            RequestEmpMaritalStatus.ForeColor = System.Drawing.Color.Red;
            if (string.IsNullOrEmpty(RequestObj.RequestEmpMaritalStatus))
                RequestEmpMaritalStatus.Text = DeletedTextValue;
        }
        if (EmpObj.EmpPassportNo.ToString() != RequestObj.RequestEmpPassportNo.ToString())
        {
            RequestEmpPassportNo.ForeColor = System.Drawing.Color.Red;
            if (string.IsNullOrEmpty(RequestObj.RequestEmpPassportNo))
                RequestEmpPassportNo.Text = DeletedTextValue;
        }
        if (EmpObj.EmpPassportIssuedCountry.ToString() != RequestObj.RequestEmpPassportIssuedCountry.ToString())
        {
            RequestEmpPassportIssuedCountry.ForeColor = System.Drawing.Color.Red;
            if (string.IsNullOrEmpty(RequestObj.RequestEmpPassportIssuedCountry))
                RequestEmpPassportIssuedCountry.Text = DeletedTextValue;
        }
        if (EmpObj.EmpPassportExpiryDate.ToString() != RequestObj.RequestEmpPassportExpiryDate.ToString())
        {
            RequestEmpPassportExpiryDate.ForeColor = System.Drawing.Color.Red;
            if (RequestObj.RequestEmpPassportExpiryDate.Ticks.Equals(0))
                RequestEmpPassportExpiryDate.Text = DeletedTextValue;
        }
        if (EmpObj.EmpNationality.ToString() != RequestObj.RequestEmpNationality.ToString())
        {
            RequestEmpNationality.ForeColor = System.Drawing.Color.Red;
            if (string.IsNullOrEmpty(RequestObj.RequestEmpNationality))
                RequestEmpNationality.Text = DeletedTextValue;
        }
        if (EmpObj.EmpHomePhoneNo.ToString() != RequestObj.RequestEmpHomePhoneNo.ToString())
        {
            RequestEmpHomePhoneNo.ForeColor = System.Drawing.Color.Red;
            if (string.IsNullOrEmpty(RequestObj.RequestEmpHomePhoneNo))
                RequestEmpHomePhoneNo.Text = DeletedTextValue;
        }
        if (EmpObj.EmpMobileNo.ToString() != RequestObj.RequestEmpMobileNo.ToString())
        {
            RequestEmpMobileNo.ForeColor = System.Drawing.Color.Red;
            if (string.IsNullOrEmpty(RequestObj.RequestEmpMobileNo))
                RequestEmpMobileNo.Text = DeletedTextValue;
        }
        if (EmpObj.EmpOfficePhoneNo.ToString() != RequestObj.RequestEmpOfficePhoneNo.ToString())
        {
            RequestEmpOfficePhoneNo.ForeColor = System.Drawing.Color.Red;
            if (string.IsNullOrEmpty(RequestObj.RequestEmpOfficePhoneNo))
                RequestEmpOfficePhoneNo.Text = DeletedTextValue;
        }
        if (EmpObj.EmpEmail.ToString() != RequestObj.RequestEmpEmail.ToString())
        {
            RequestEmpEmail.ForeColor = System.Drawing.Color.Red;
            if (string.IsNullOrEmpty(RequestObj.RequestEmpEmail))
                RequestEmpEmail.Text = DeletedTextValue;
        }
        if (EmpObj.EmpResAddr.ToString() != RequestObj.RequestEmpResAddr.ToString())
        {
            RequestEmpResAddr.ForeColor = System.Drawing.Color.Red;
            if (string.IsNullOrEmpty(RequestObj.RequestEmpResAddr))
                RequestEmpResAddr.Text = DeletedTextValue;
        }
        if (EmpObj.EmpResAddrAreaCode.ToString() != RequestObj.RequestEmpResAddrAreaCode.ToString())
        {
            RequestEmpResAddrAreaCode.ForeColor = System.Drawing.Color.Red;
            if (string.IsNullOrEmpty(RequestObj.RequestEmpResAddrAreaCode))
                RequestEmpResAddrAreaCode.Text = DeletedTextValue;
        }
        if (EmpObj.EmpCorAddr.ToString() != RequestObj.RequestEmpCorAdd.ToString())
        {
            RequestEmpCorAdd.ForeColor = System.Drawing.Color.Red;
            if (string.IsNullOrEmpty(RequestObj.RequestEmpCorAdd))
                RequestEmpCorAdd.Text = DeletedTextValue;
        }
        // Start 0000092, KuangWei, 2014-10-13
        if (EmpObj.EmpPlaceOfBirth.ToString() != RequestObj.RequestEmpPlaceOfBirth.ToString())
        {
            RequestEmpPlaceOfBirth.ForeColor = System.Drawing.Color.Red;
            if (string.IsNullOrEmpty(RequestObj.RequestEmpPlaceOfBirth))
                RequestEmpPlaceOfBirth.Text = DeletedTextValue;
        }
        // End 0000092, KuangWei, 2014-10-13
        return true;
    }

    public int CurrentRequestID
    {
        get { return RequestID; }
        set { RequestID = value; }
    }

    public String CurrentRequestStatus
    {
        get { return RequestStatus.Text; }
        set { RequestStatus.Text = value; }
    }

    protected void Cancel_Click(object sender, EventArgs e)
    {

        DBFilter filterStatus = new DBFilter();
        DBManager Requestdb = EEmpRequest.db;
        PageErrors errors = PageErrors.getErrors(db, Page.Master);;
        errors.clear();
        //------------------------------------------------------
        //Select Filter record from the EmpRequest table by EmpRequestRecordID and Request Status
        filterStatus.add(new Match("EmpRequestRecordID", RequestID));
        OR or = new OR();
        or.add(new Match("EmpRequestStatus", EEmpRequest.STATUS_SUBMITTED));
        or.add(new Match("EmpRequestStatus", EEmpRequest.STATUS_ACCEPTED));
        filterStatus.add(or);
        //------------------------------------------------------
        if (Requestdb.count(dbConn, filterStatus) > 0)
        {
            ArrayList EmpRequestList = Requestdb.select(dbConn, filterStatus);
            EEmpRequest EmpRequest = (EEmpRequest)EmpRequestList[0];
            ESSAuthorizationProcess authorization = new ESSAuthorizationProcess(dbConn);
            authorization.CancelAction(EmpRequest);
            HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "~/ESS_EmpRequestStatus.aspx");
        }

        

    }
}
