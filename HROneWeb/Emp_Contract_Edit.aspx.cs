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

public partial class Emp_Contract_Edit : HROneWebPage
{
    private const string FUNCTION_CODE = "PER006";
    public Binding binding;
    public DBManager db = EEmpContractTerms.db;
    public EEmpContractTerms obj;
    public int CurID = -1;
    public int CurEmpID = -1;
    

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!WebUtils.CheckAccess(Response, Session, FUNCTION_CODE, WebUtils.AccessLevel.ReadWrite))
            return;
        toolBar.FunctionCode = FUNCTION_CODE;

        if (!int.TryParse(DecryptedRequest["EmpContractID"], out CurID))
            CurID = -1;

        if (!int.TryParse(DecryptedRequest["EmpID"], out CurEmpID))
            CurEmpID = -1;


        DBFilter salaryOthersFilter = new DBFilter();
        OR orSalaryOthersTerms = new OR();
        orSalaryOthersTerms.add(new Match("PaymentTypeID", EPaymentType.SystemPaymentType.BasicSalaryPaymentType(dbConn).PaymentTypeID));
        orSalaryOthersTerms.add(new Match("PaymentTypeID", EPaymentType.SystemPaymentType.BonusPaymentType(dbConn).PaymentTypeID));
        orSalaryOthersTerms.add(new Match("PaymentTypeID", EPaymentType.SystemPaymentType.OthersPaymentType(dbConn).PaymentTypeID));
        salaryOthersFilter.add(orSalaryOthersTerms);

        binding = new Binding(dbConn, db);
        binding.add(EmpContractID);
        binding.add(EmpID);
        binding.add(EmpContractCompanyName);
        binding.add(EmpContractCompanyContactNo);
        binding.add(new TextBoxBinder(db, EmpContractEmployedFrom.TextBox, EmpContractEmployedFrom.ID));
        binding.add(new TextBoxBinder(db, EmpContractEmployedTo.TextBox, EmpContractEmployedTo.ID));
        binding.add(EmpContractCompanyAddr);
        binding.add(EmpContractGratuity);
        binding.add(new DropDownVLBinder(db, PayCodeID, EPaymentCode.VLPaymentCode, salaryOthersFilter));
        binding.add(new DropDownVLBinder(db, EmpContractGratuityMethod, Values.VLPaymentMethod));

        DBFilter empBankAccountFilter = new DBFilter();
        OR bankAccountORTerm = new OR();
        bankAccountORTerm.add(new Match("EmpID", CurEmpID));
        {
            EEmpPersonalInfo tmpEmpInfo = new EEmpPersonalInfo();
            tmpEmpInfo.EmpID = CurEmpID;
            if (EEmpPersonalInfo.db.select(dbConn, tmpEmpInfo) && tmpEmpInfo.MasterEmpID > 0)
                bankAccountORTerm.add(new Match("EmpID", tmpEmpInfo.MasterEmpID));
        }
        empBankAccountFilter.add(bankAccountORTerm);
        binding.add(new DropDownVLBinder(db, EmpAccID, EEmpBankAccount.VLBankAccount, empBankAccountFilter));

        binding.add(CurrencyID);
        binding.init(Request, Session);



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
        EmpID.Value = CurEmpID.ToString();

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

    protected void Save_Click(object sender, EventArgs e)
    {
        EEmpContractTerms c = new EEmpContractTerms();

        Hashtable values = new Hashtable();
        binding.toValues(values);

        PageErrors errors = PageErrors.getErrors(db, Page.Master);
        errors.clear();


        db.validate(errors, values);

        if (!errors.isEmpty())
            return;


        db.parse(values, c);

        if (c.EmpContractGratuityMethod.Equals("A"))
        {
            if (c.EmpAccID == 0)
            {
                EEmpBankAccount bankAccount = EEmpBankAccount.GetDefaultBankAccount(dbConn, c.EmpID);
                if (bankAccount != null)
                    c.EmpAccID = bankAccount.EmpBankAccountID;
                else
                    errors.addError("EmpAccID", HROne.Translation.PageErrorMessage.ERROR_ACCOUNT_REQUIRED);
            }
        }
        else
            c.EmpAccID = 0;

        if (c.EmpContractEmployedTo!=DateTime.MinValue && c.EmpContractEmployedFrom > c.EmpContractEmployedTo)
        {
            errors.addError("EmpPoRFrom", HROne.Translation.PageErrorMessage.ERROR_DATE_TO_TOO_EARLY);
            return;
        }

        WebUtils.StartFunction(Session, FUNCTION_CODE, c.EmpID);
        c.CurrencyID = "HKD";
        if (CurID < 0)
        {
//            Utils.MarkCreate(Session, c);

            db.insert(dbConn, c);
            CurID = c.EmpContractID;
            
//            url = Utils.BuildURL(-1, CurID);
        }
        else
        {
//            Utils.Mark(Session, c);
            db.update(dbConn, c);
        }
        WebUtils.EndFunction(dbConn);


        HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "Emp_Contract_View.aspx?EmpContractID="+CurID+ "&EmpID=" + c.EmpID);


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

    protected void Back_Click(object sender, EventArgs e)
    {
        if (CurID > 0)
            HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "Emp_Contract_View.aspx?EmpContractID=" + CurID + "&EmpID=" + EmpID.Value);
        else
            HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "EmpTab_Contract_View.aspx?EmpID=" + EmpID.Value);

    }
}
