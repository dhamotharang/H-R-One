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
using HROne.Translation;
using HROne.Lib.Entities;

public partial class PaymentCode_View : HROneWebPage
{
    private const string FUNCTION_CODE = "SYS004";

    public Binding binding;
    public DBManager db = EPaymentCode.db;
    public EPaymentCode obj;
    public int CurID = -1;
    

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!WebUtils.CheckAccess(Response, Session, FUNCTION_CODE, WebUtils.AccessLevel.Read))
            return;
        toolBar.FunctionCode = FUNCTION_CODE;

        binding = new Binding(dbConn, db);
        binding.add(PaymentCodeID);
        binding.add(PaymentCode);
        binding.add(PaymentCodeDesc);
        binding.add(new CheckBoxBinder(db, PaymentCodeIsProrata));
        binding.add(new CheckBoxBinder(db, PaymentCodeIsProrataLeave));
        binding.add(new CheckBoxBinder(db, PaymentCodeIsProrataStatutoryHoliday));
        binding.add(new LabelVLBinder(db, PaymentCodeIsMPF, Values.VLTrueFalseYesNo));
        binding.add(new LabelVLBinder(db, PaymentCodeIsWages, Values.VLTrueFalseYesNo));
        binding.add(new LabelVLBinder(db, PaymentCodeIsTopUp, Values.VLTrueFalseYesNo));
        binding.add(new LabelVLBinder(db, PaymentCodeIsORSO, Values.VLTrueFalseYesNo));
        binding.add(new LabelVLBinder(db, PaymentTypeID, EPaymentType.VLPaymentType));
        binding.add(new LabelVLBinder(db, PaymentCodeRoundingRule, Values.VLRoundingRule));
        binding.add(PaymentCodeDecimalPlace);
        binding.add(new LabelVLBinder(db, PaymentCodeHideInPaySlip, Values.VLTrueFalseYesNo));
        binding.add(PaymentCodeDisplaySeqNo);
        binding.add(new LabelVLBinder(db, PaymentCodeNotRemoveContributionFromTopUp, Values.VLTrueFalseYesNo));
        // Start 000159, Ricky So, 2015-01-23
        binding.add(new LabelVLBinder(db, PaymentCodeIsHitRateBased, Values.VLTrueFalseYesNo));
        binding.add(PaymentCodeDefaultRateAtMonth1);
        binding.add(PaymentCodeDefaultRateAtMonth2);
        binding.add(PaymentCodeDefaultRateAtMonth3);
        // End 000159, Ricky So, 2015-01-23
        binding.init(Request, Session);

        if (!int.TryParse(DecryptedRequest["PaymentCodeID"], out CurID))
            CurID = -1;

        HROne.Common.WebUtility.WebControlsLocalization(this, this.Controls);

        HROne.ProductLicense productLicense = WebUtils.productLicense(Session);
        if (productLicense.ProductType != HROne.ProductLicense.ProductLicenseType.HROne)
            PaymentCodeNotRemoveContributionFromTopUpRow.Visible = false;
        else
            PaymentCodeNotRemoveContributionFromTopUpRow.Visible = true;

		if(!Page.IsPostBack) 
		{
            if (CurID > 0)
            {
                loadObject();

            }
            else
                toolBar.DeleteButton_Visible = false;
        }
    }
    protected bool loadObject()
    {
        obj = new EPaymentCode();
        bool isNew = WebFormWorkers.loadKeys(db, obj, DecryptedRequest);
        if (!db.select(dbConn, obj))
            return false;

        Hashtable values = new Hashtable();
        if (string.IsNullOrEmpty(obj.PaymentCodeRoundingRule))
        {
            obj.PaymentCodeRoundingRule = Values.ROUNDING_RULE_ROUND_TO;
            obj.PaymentCodeDecimalPlace = 2;
        }
        db.populate(obj, values);
        binding.toControl(values);

        if (obj.PaymentCodeRoundingRuleIsAbsoluteValue)
            PaymentCodeRoundingRuleIsAbsoluteValue.Visible = true;
        else
            PaymentCodeRoundingRuleIsAbsoluteValue.Visible = false;
        loadTaxPayment();
        return true;
    }

    protected void loadTaxPayment()
    {
        //DBFilter filter;
        ArrayList list = new ArrayList();
        list.Add("B");
        list.Add("F");
        list.Add("G");
        list.Add("E");
        // Start 0000020, KuangWei, 2014-08-01
        list.Add("M");
        // End 0000020, KuangWei, 2014-08-01
        TaxPaymentRepeater.DataSource = list;
        TaxPaymentRepeater.DataBind();


    }

    protected void TaxPaymentRepeater_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        string TaxFormCode = (string)e.Item.DataItem;

        Label lblTaxFormType = (Label)e.Item.FindControl("TaxFormType");
        lblTaxFormType.Text = "IR56" + TaxFormCode;
        Label c = (Label)e.Item.FindControl("TaxPayID");

        DBFilter TaxPaymentMapFilter = new DBFilter();
        TaxPaymentMapFilter.add(new Match("tpm.PaymentCodeID", CurID));

        DBFilter TaxPaymentFilter = new DBFilter();
        TaxPaymentFilter.add(new Match("TaxFormType", TaxFormCode));
        TaxPaymentFilter.add(new IN("TaxPayID", "Select tpm.TaxPayID FROM " + ETaxPaymentMap.db.dbclass.tableName + " tpm", TaxPaymentMapFilter));

        ArrayList taxPaymentList = ETaxPayment.db.select(dbConn, TaxPaymentFilter);
        if (taxPaymentList.Count>0)
        {
            ETaxPayment taxPay = (ETaxPayment)taxPaymentList[0];
            c.Text = taxPay.TaxPayCode + " - " + taxPay.TaxPayDesc;
        }
        else
        {
            c.Text = "";
        }


    }

//    protected void Save_Click(object sender, EventArgs e)
//    {
//        EPaymentCode c = new EPaymentCode();

//        Hashtable values = new Hashtable();
//        binding.toValues(values);

//        PageErrors errors = PageErrors.getErrors(db, Page.Master);
//        errors.clear();


//        db.validate(errors, values);

//        if (!errors.isEmpty())
//            return;


//        db.parse(values, c);
//        if (!AppUtils.checkDuplicate(dbConn, db, c, errors, "PaymentCode"))
//            return;

//        WebUtils.StartFunction(Session, FUNCTION_CODE);

//        if (CurID < 0)
//        {
////            Utils.MarkCreate(Session, c);

//            db.insert(dbConn, c);
//            CurID = c.PaymentCodeID;
////            url = Utils.BuildURL(-1, CurID);
//        }
//        else
//        {
////            Utils.Mark(Session, c);
//            db.update(dbConn, c);
//        }
//        WebUtils.EndFunction(dbConn);

//        Response.Redirect(Request.Url.LocalPath+"?PaymentCodeID="+CurID);


//    }
    protected void Delete_Click(object sender, EventArgs e)
    {
        PageErrors errors = PageErrors.getErrors(db, Page.Master);
        errors.clear();

        EPaymentCode obj = new EPaymentCode();
        obj.PaymentCodeID = CurID;

        db.select(dbConn, obj);
        
        DBFilter paymentRecordFilter = new DBFilter();
        paymentRecordFilter.add(new Match("PaymentCodeID", obj.PaymentCodeID));

        IN inTerms = new IN("EmpPayrollID", "Select EmpPayrollID From " + EPaymentRecord.db.dbclass.tableName, paymentRecordFilter);

        DBFilter empPayrollFilter = new DBFilter();
        empPayrollFilter.add(inTerms);
        empPayrollFilter.add("empid", true);
        ArrayList empPayrollList = EEmpPayroll.db.select(dbConn, empPayrollFilter);
        if (empPayrollList.Count > 0)
        {
            int lastEmpID = 0;
            errors.addError(string.Format(HROne.Translation.PageErrorMessage.ERROR_CODE_USED_BY_EMPLOYEE, new string[] { HROne.Common.WebUtility.GetLocalizedString("Payment Code"), obj.PaymentCode }));
            foreach (EEmpPayroll empPayroll in empPayrollList)
            {
                if (lastEmpID != empPayroll.EmpID)
                {
                    EEmpPersonalInfo empInfo = new EEmpPersonalInfo();
                    empInfo.EmpID = empPayroll.EmpID;
                    if (EEmpPersonalInfo.db.select(dbConn, empInfo))
                    {
                        errors.addError("- " + empInfo.EmpNo + ", " + empInfo.EmpEngFullName);
                        lastEmpID = empPayroll.EmpID;
                    }
                    else
                    {
                        EEmpPayroll.db.delete(dbConn, empPayroll);
                    }
                }
            } 
            errors.addError(HROne.Translation.PageErrorMessage.ERROR_ACTION_ABORT);
        }
        else
        {
            WebUtils.StartFunction(Session, FUNCTION_CODE);
            db.delete(dbConn, obj);
            WebUtils.EndFunction(dbConn);
            HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "PaymentCode_List.aspx");
        }

    }
    protected void Edit_Click(object sender, EventArgs e)
    {
        HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "PaymentCode_Edit.aspx?PaymentCodeID=" + CurID);
    }
    protected void Back_Click(object sender, EventArgs e)
    {
        HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "PaymentCode_List.aspx");
    }
}
