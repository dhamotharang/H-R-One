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

public partial class Emp_FinalPayment_Edit : HROneWebPage
{
    private const string FUNCTION_CODE = "PER011";

    public Binding binding;
    public DBManager db = EEmpFinalPayment.db;
    public EEmpFinalPayment obj;
    public int CurID = -1;
    public int CurEmpID = -1;
    public Hashtable CurEmpFinalPaymentGroups=new Hashtable();
    public Hashtable CurRanks = new Hashtable();


    protected void Page_Load(object sender, EventArgs e)
    {
        if (!WebUtils.CheckAccess(Response, Session, FUNCTION_CODE, WebUtils.AccessLevel.ReadWrite))
            return;
        toolBar.FunctionCode = FUNCTION_CODE;

        if (!int.TryParse(DecryptedRequest["EmpFinalPayID"], out CurID))
            CurID = -1;

        if (!int.TryParse(DecryptedRequest["EmpID"], out CurEmpID))
            CurEmpID = -1;

        binding = new Binding(dbConn, db);
        binding.add(EmpFinalPayID);
        binding.add(EmpID);
        binding.add(new DropDownVLBinder(db, PayCodeID, EPaymentCode.VLPaymentCode).setNotSelected(null));
        binding.add(EmpFinalPayAmount);
        binding.add(new DropDownVLBinder(db, EmpFinalPayMethod, Values.VLPaymentMethod).setNotSelected(null));
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
        binding.add(new DropDownVLBinder(db, EmpAccID, EEmpBankAccount.VLBankAccount, empBankAccountFilter).setNotSelected(HROne.Common.WebUtility.GetLocalizedString(EEmpBankAccount.DEFAULT_BANK_ACCOUNT_TEXT)));
        binding.add(new DropDownVLBinder(db, CostCenterID, ECostCenter.VLCostCenter).setNotSelected(HROne.Common.WebUtility.GetLocalizedString(ECostCenter.DEFAULT_COST_CENTER_TEXT)));
        binding.add(EmpFinalPayRemark);
        binding.add(EmpFinalPayNumOfDayAdj);
        binding.add(new LabelVLBinder(db, EmpFinalPayIsAutoGen, Values.VLTrueFalseYesNo));
        binding.add(new CheckBoxBinder(db, EmpFinalPayIsRestDayPayment));
        binding.init(Request, Session);



        EmpID.Value = CurEmpID.ToString();

        HROne.Common.WebUtility.WebControlsLocalization(this, this.Controls);

        if (!Page.IsPostBack)
        {
            if (CurID > 0)
            {
                toolBar.DeleteButton_Visible = true;
                toolBar.SaveButton_Visible = true;
                if (!loadObject())
                {
                    toolBar.DeleteButton_Visible = false;
                    toolBar.SaveButton_Visible = false;
                }
            }
            else
            {
                toolBar.DeleteButton_Visible = false;
                if (CurEmpID <= 0)
                {
                    toolBar.SaveButton_Visible = false;
                }
            }
        }

        if (EmpFinalPayMethod.SelectedValue.Equals("A"))
        {
            BankAccountRow.Visible = true;
            lblDefaultBankAccount.Text = string.Empty;
            if (EmpAccID.SelectedValue.Equals(string.Empty))
            {
                EEmpBankAccount bankAccount = EEmpBankAccount.GetDefaultBankAccount(dbConn, CurEmpID);
                if (bankAccount != null)
                {
                    lblDefaultBankAccount.Text = bankAccount.EmpBankCode + "-" + bankAccount.EmpBranchCode + "-" + bankAccount.EmpAccountNo;
                }
            }
        }
        else
            BankAccountRow.Visible = false;

        CostCenterRow.Visible = WebUtils.productLicense(Session).IsCostCenter;
    }
    protected bool loadObject() 
    {
	    obj=new EEmpFinalPayment();
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
        EEmpFinalPayment c = new EEmpFinalPayment();

        Hashtable values = new Hashtable();
        binding.toValues(values);

        PageErrors errors = PageErrors.getErrors(db, Page.Master);
        errors.clear();


        db.validate(errors, values);

        if (!errors.isEmpty())
            return;


        db.parse(values, c);

        if (c.EmpFinalPayMethod.Equals("A"))
        {
            if (c.EmpAccID == 0)
            {
                EEmpBankAccount bankAccount = EEmpBankAccount.GetDefaultBankAccount(dbConn, c.EmpID);
                if (bankAccount != null)
                {
                    //c.EmpAccID = bankAccount.EmpBankAccountID;
                }
                else
                    errors.addError("EmpAccID", HROne.Translation.PageErrorMessage.ERROR_ACCOUNT_REQUIRED);
            }
        }

        //DBFilter filter = new DBFilter();
        //filter.add(new Match("EmpID", c.EmpID));
        //filter.add(new Match("EmpFinalPayID", "<>", c.EmpFinalPayID));
        //filter.add(new Match("PayCodeID", c.PayCodeID));
        //if (db.count(dbConn, filter) > 0)
        //{
        //    errors.addError(HROne.Translation.PageErrorMessage.ERROR_ONE_PAYMENT_PER_PCODE);
        //    return;
        //}

        if (errors.isEmpty())
        {
            WebUtils.StartFunction(Session, FUNCTION_CODE, c.EmpID);
            c.CurrencyID = "HKD";
            if (CurID < 0)
            {
                //            Utils.MarkCreate(Session, c);

                db.insert(dbConn, c);
                CurID = c.EmpFinalPayID;
                //            url = Utils.BuildURL(-1, CurID);
            }
            else
            {
                //            Utils.Mark(Session, c);
                db.update(dbConn, c);
            }
            WebUtils.EndFunction(dbConn);


            HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "Emp_FinalPayment_View.aspx?EmpID=" + c.EmpID + "&EmpFinalPayID=" + CurID);
        }

    }
    protected void Delete_Click(object sender, EventArgs e)
    {

        EEmpFinalPayment c = new EEmpFinalPayment();
        c.EmpFinalPayID = CurID;
        db.select(dbConn, c);
        WebUtils.StartFunction(Session, FUNCTION_CODE, c.EmpID);
        db.delete(dbConn, c);
        WebUtils.EndFunction(dbConn);
        HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "EmpTab_Termination_View.aspx?EmpID=" + EmpID.Value);
    }
   
    protected void Back_Click(object sender, EventArgs e)
    {
        if (CurID > 0)
            HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "Emp_FinalPayment_View.aspx?EmpID=" + EmpID.Value + "&EmpFinalPayID=" + CurID);
        else
            HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "EmpTab_Termination_View.aspx?EmpID=" + EmpID.Value);

    }
}
