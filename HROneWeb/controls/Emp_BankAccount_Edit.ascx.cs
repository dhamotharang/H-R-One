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

public partial class Emp_BankAccount_Edit_Control : HROneWebControl
{
    private const string FUNCTION_CODE = "PER002";
    protected Binding binding;
    protected DBManager db = EEmpBankAccount.db;
    protected DBManager empDB = EEmpPersonalInfo.db;

    public int EmpBankAccountID
    {
        get { int CurID = -1; if (int.TryParse(ID.Value, out CurID)) return CurID; else return -1; }
        set { ID.Value = value.ToString(); }
    }

    public int EmpID
    {
        get { int PrevCurID = -1; if (int.TryParse(HiddenEmpID.Value, out PrevCurID)) return PrevCurID; else return -1; }
        set { HiddenEmpID.Value = value.ToString(); }
    }

    public string BankCode
    {
        get { return EmpBankCode.Text; }
        set { EmpBankCode.Text = value; }
    }

    public string BranchCode
    {
        get { return EmpBranchCode.Text; }
        set { EmpBranchCode.Text = value; }
    }

    public string AccountNo
    {
        get { return EmpAccountNo.Text; }
        set { EmpAccountNo.Text = value; }
    }

    public string BankAccountHolderName
    {
        get { return EmpBankAccountHolderName.Text; }
        set { EmpBankAccountHolderName.Text = value; }
    }

    public string BankAccountRemark
    {
        get { return EmpBankAccountRemark.Text; }
        set { EmpBankAccountRemark.Text = value; }
    }

    public bool DefaultAccount
    {
        get { return EmpAccDefault.Checked; }
        set { EmpAccDefault.Checked = value; }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        binding = new Binding(dbConn, db);
        binding.add(new HiddenBinder(db, ID, "EmpBankAccountID"));
        binding.add(new HiddenBinder(db, HiddenEmpID, "EmpID"));
        binding.add(EmpBankCode);
        binding.add(EmpBranchCode);
        binding.add(EmpAccountNo);
        binding.add(EmpBankAccountHolderName);
        binding.add(EmpBankAccountRemark);
        binding.add(new CheckBoxBinder(db, EmpAccDefault));

        binding.init(Request, Session);

    }
    protected void Page_PreRender(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (EmpBankAccountID > 0)
            {
                loadObject();
            }
            else
            {
                EEmpPersonalInfo empOBJ = new EEmpPersonalInfo();
                DBFilter filter = new DBFilter();
                if (EmpID > 0)
                {
                    filter.add(new Match("EmpID", EmpID));
                    ArrayList empInfoList = EEmpPersonalInfo.db.select(dbConn, filter);
                    if (empInfoList.Count > 0)
                    {
                        empOBJ = (EEmpPersonalInfo)empInfoList[0];
                        EmpBankAccountHolderName.Text = empOBJ.EmpEngFullName;

                        DBFilter empBankAccountFilter = new DBFilter();
                        empBankAccountFilter.add(new Match("EmpID", EmpID));
                        if (EEmpBankAccount.db.count(dbConn, empBankAccountFilter) == 0)
                            EmpAccDefault.Checked = true;
                    }
                }
            }
        }
    }

    protected bool loadObject()
    {
        EEmpBankAccount obj = new EEmpBankAccount();
        bool isNew = WebFormWorkers.loadKeys(db, obj, DecryptedRequest);

        DBFilter filter = new DBFilter();
        filter.add(new Match("EmpID", EmpID));
        filter.add(WebUtils.AddRankFilter(Session, "EmpID", true));
        if (EEmpPersonalInfo.db.count(dbConn, filter) == 0)
            if (EmpID <= 0)
                return false;
            else
                HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "~/AccessDeny.aspx");

        if (!db.select(dbConn, obj))
            if (EmpID <= 0)
                return false;
            else
                HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "~/AccessDeny.aspx");

        if (obj.EmpID != EmpID)
            if (EmpBankAccountID <= 0)
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

        Hashtable values = new Hashtable();
        db.populate(obj, values);
        binding.toControl(values);


        return true;
    }

    public bool Save()
    {
        EEmpBankAccount c = new EEmpBankAccount();

        Hashtable values = new Hashtable();
        binding.toValues(values);

        PageErrors errors = PageErrors.getErrors(db, Page.Master);
        errors.clear();


        db.validate(errors, values);

        if (!errors.isEmpty())
            return false;


        db.parse(values, c);

        DBFilter bankAccountFilter = new DBFilter();
        bankAccountFilter.add(new Match("EmpID", c.EmpID));
        bankAccountFilter.add(new Match("EmpBankAccountID", "<>", c.EmpBankAccountID));
        ArrayList bankAccountLIst = EEmpBankAccount.db.select(dbConn, bankAccountFilter);
        foreach (EEmpBankAccount empBankAccount in bankAccountLIst)
        {
            if (empBankAccount.EmpBankCode.Equals(c.EmpBankCode) && empBankAccount.EmpBranchCode.Equals(c.EmpBranchCode) && empBankAccount.EmpAccountNo.Equals(c.EmpAccountNo))
                if (c.EmpBankAccountRemark.Equals(empBankAccount.EmpBankAccountRemark) || string.IsNullOrEmpty(c.EmpBankAccountRemark) && string.IsNullOrEmpty(empBankAccount.EmpBankAccountRemark))
                    errors.addError(string.Format(HROne.Common.WebUtility.GetLocalizedString("ERROR_CODE_DUPLICATE"), new string[] { HROne.Common.WebUtility.GetLocalizedString("Bank Account"), c.EmpBankCode + "-" + c.EmpBranchCode + "-" + c.EmpAccountNo }));
        }
        if (!errors.isEmpty())
            return false;

        WebUtils.StartFunction(Session, FUNCTION_CODE, c.EmpID);

        if (c.EmpBankAccountID <= 0)
        {
            //            Utils.MarkCreate(Session, c);

            db.insert(dbConn, c);
            EmpBankAccountID = c.EmpBankAccountID;
            //            url = Utils.BuildURL(-1, CurID);
        }
        else
        {
            //            Utils.Mark(Session, c);
            db.update(dbConn, c);
        }
        WebUtils.EndFunction(dbConn);

        if (c.EmpAccDefault)
        {
            DBFilter filter = new DBFilter();
            filter.add(new Match("EmpBankAccountID", "<>", c.EmpBankAccountID));
            filter.add(new Match("EmpID", c.EmpID));
            filter.add(new Match("EmpAccDefault", "<>", false));
            EEmpBankAccount t = new EEmpBankAccount();
            t.EmpAccDefault = false;
            db.updateByTemplate(dbConn, t, filter);
        }

        return true;
    }

    public bool Delete()
    {
        PageErrors errors = PageErrors.getErrors(db, Page.Master);
        errors.clear();

        EEmpBankAccount obj = new EEmpBankAccount();
        obj.EmpBankAccountID = EmpBankAccountID;
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
            return false;
        }
        else
        {
            WebUtils.StartFunction(Session, FUNCTION_CODE, obj.EmpID);
            db.delete(dbConn, obj);
            WebUtils.EndFunction(dbConn);
        } 
        return true;
    }
}
