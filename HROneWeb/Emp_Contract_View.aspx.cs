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

public partial class Emp_Contract_View : HROneWebPage
{
    private const string FUNCTION_CODE = "PER006";
    public Binding binding;
    public DBManager db = EEmpContractTerms.db;
    public EEmpContractTerms obj;
    public int CurID = -1;
    public int CurEmpID = -1;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!WebUtils.CheckAccess(Response, Session, FUNCTION_CODE, WebUtils.AccessLevel.Read))
            return;
        toolBar.FunctionCode = FUNCTION_CODE;

        if (!int.TryParse(DecryptedRequest["EmpContractID"], out CurID))
            CurID = -1;

        if (!int.TryParse(DecryptedRequest["EmpID"], out CurEmpID))
            CurEmpID = -1;

        binding = new Binding(dbConn, db);
        binding.add(EmpContractID);
        binding.add(EmpID);
        binding.add(EmpContractCompanyName);
        binding.add(EmpContractCompanyContactNo);
        binding.add(EmpContractEmployedFrom);
        binding.add(EmpContractEmployedTo);
        binding.add(EmpContractCompanyAddr);
        binding.add(EmpContractGratuity);

        binding.add(new BlankZeroLabelVLBinder(db, PayCodeID, EPaymentCode.VLPaymentCode));
        binding.add(new LabelVLBinder(db, EmpContractGratuityMethod, Values.VLPaymentMethod));

        DBFilter empBankAccountFilter = new DBFilter();
        OR bankAccountORTerm = new OR();
        bankAccountORTerm.add(new Match("EmpID", CurEmpID));
        {
            EEmpPersonalInfo tmpEmpInfo = new EEmpPersonalInfo();
            tmpEmpInfo.EmpID = CurEmpID;
            if (EEmpPersonalInfo.db.select(dbConn, tmpEmpInfo) && tmpEmpInfo.MasterEmpID > 0)
                bankAccountORTerm.add(new Match("EmpID", tmpEmpInfo.MasterEmpID));
        }
        empBankAccountFilter.add(bankAccountORTerm); binding.add(new BlankZeroLabelVLBinder(db, EmpAccID, EEmpBankAccount.VLBankAccount, empBankAccountFilter));


        binding.add(CurrencyID);
        binding.init(Request, Session);



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
	    obj=new EEmpContractTerms();
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


    protected void Delete_Click(object sender, EventArgs e)
    {

        EEmpContractTerms c = new EEmpContractTerms();
        c.EmpContractID = CurID;
        db.select(dbConn, c);
        WebUtils.StartFunction(Session, FUNCTION_CODE, c.EmpID);
        db.delete(dbConn, c);
        WebUtils.EndFunction(dbConn);
        HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "EmpTab_Contract_View.aspx?EmpID=" + EmpID.Value);
    }
    protected void Edit_Click(object sender, EventArgs e)
    {
        HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "Emp_Contract_Edit.aspx?EmpContractID=" + EmpContractID.Value + "&EmpID=" + EmpID.Value);
    }

    protected void Back_Click(object sender, EventArgs e)
    {
        HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "EmpTab_Contract_View.aspx?EmpID=" + EmpID.Value);
    }
}
