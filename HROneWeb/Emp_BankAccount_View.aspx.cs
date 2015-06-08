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
using HROne.Translation;

public partial class Emp_BankAccount_View : HROneWebPage
{
    private const string FUNCTION_CODE = "PER002";

    public Binding binding;
    public DBManager db = EEmpBankAccount.db;
    public EEmpBankAccount obj;
    public int CurID = -1;
    public int CurEmpID = -1;
    public Hashtable CurEmpBankAccountGroups=new Hashtable();
    public Hashtable CurRanks = new Hashtable();
    

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!WebUtils.CheckAccess(Response, Session, FUNCTION_CODE, WebUtils.AccessLevel.Read))
            return;
        toolBar.FunctionCode = FUNCTION_CODE;
        

        binding = new Binding(dbConn, db);
        binding.add(EmpBankAccountID);
        binding.add(EmpID);
        binding.add(EmpBankCode);
        binding.add(EmpBranchCode);
        binding.add(EmpAccountNo);
        binding.add(EmpBankAccountHolderName);
        binding.add(new CheckBoxBinder(db, EmpAccDefault));
        binding.add(EmpBankAccountRemark);
        binding.init(Request, Session);

        if (!int.TryParse(DecryptedRequest["EmpBankAccountID"], out CurID))
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
                TitleCode.Text = EmpBankAccountHolderName.Text;
            }
            else
                toolBar.DeleteButton_Visible = false;
        }
    }
    protected bool loadObject() 
    {
	    obj=new EEmpBankAccount();
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
        //DBFilter filter = new DBFilter();
        //filter.add(new Match("EmpBankAccountID", obj.EmpBankAccountID));
        //filter.add(WebUtils.AddRankFilter(Session, "EmpID", true));
        //ArrayList empBankAccountList = EEmpBankAccount.db.select(dbConn, filter);
        //if (empBankAccountList.Count == 0)
        //    return false;
        //obj = (EEmpBankAccount)empBankAccountList[0];

	    Hashtable values=new Hashtable();
	    db.populate(obj,values);
	    binding.toControl(values);


        return true;
    }

    protected void Delete_Click(object sender, EventArgs e)
    {
        PageErrors errors = PageErrors.getErrors(db, Page.Master);
        errors.clear();

        EEmpBankAccount obj = new EEmpBankAccount();
        obj.EmpBankAccountID = CurID;
        db.select(dbConn, obj);
        DBFilter paymentRecordFilter = new DBFilter();
        paymentRecordFilter.add(new Match("EmpAccID", obj.EmpBankAccountID));

        IN inTerms = new IN("EmpPayrollID", "Select EmpPayrollID From " + EPaymentRecord.db.dbclass.tableName, paymentRecordFilter);

        DBFilter empPayrollFilter = new DBFilter();
        empPayrollFilter.add(inTerms);
        empPayrollFilter.add("empid", true);
        ArrayList empPayrollList = EEmpPayroll.db.select(dbConn, empPayrollFilter);
        if (empPayrollList.Count > 0)
        {
            errors.addError(string.Format(HROne.Translation.PageErrorMessage.ERROR_CODE_IS_IN_USE, new string[] { HROne.Common.WebUtility.GetLocalizedString("Bank Account") }));
            errors.addError(HROne.Translation.PageErrorMessage.ERROR_ACTION_ABORT);
            return;
        }
        else
        {
            WebUtils.StartFunction(Session, FUNCTION_CODE, obj.EmpID);
            db.delete(dbConn, obj);
            WebUtils.EndFunction(dbConn);
        }
        HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "EmpTab_BankAccount_View.aspx?EmpID=" + EmpID.Value);
    }
    protected void Edit_Click(object sender, EventArgs e)
    {
        HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "Emp_BankAccount_Edit.aspx?EmpBankAccountID=" + EmpBankAccountID.Value + "&EmpID=" + EmpID.Value);
    }

    protected void Back_Click(object sender, EventArgs e)
    {
        HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "EmpTab_BankAccount_View.aspx?EmpID=" + EmpID.Value);
        
    }
}
