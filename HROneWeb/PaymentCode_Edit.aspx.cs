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

public partial class PaymentCode_Edit : HROneWebPage
{
    private const string FUNCTION_CODE = "SYS004";

    public Binding binding;
    public DBManager db = EPaymentCode.db;
    public int CurID = -1;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!WebUtils.CheckAccess(Response, Session, FUNCTION_CODE, WebUtils.AccessLevel.ReadWrite))
            return;
        toolBar.FunctionCode = FUNCTION_CODE;

        if (!int.TryParse(DecryptedRequest["PaymentCodeID"], out CurID))
            CurID = -1;

        DBFilter paymentTypeFilter = new DBFilter();
        DBFilter uniquePaymentTypeFilter = new DBFilter();
        uniquePaymentTypeFilter.add(new IN("pc.PaymentTypeID", 
            EPaymentType.SystemPaymentType.MPFEmployeeMandatoryContributionPaymentType(dbConn).PaymentTypeID.ToString() + ", "
            + EPaymentType.SystemPaymentType.MPFEmployerMandatoryContributionPaymentType(dbConn).PaymentTypeID.ToString() + ", "
            + EPaymentType.SystemPaymentType.MPFEmployeeVoluntaryContributionPaymentType(dbConn).PaymentTypeID.ToString() + ", "
            + EPaymentType.SystemPaymentType.MPFEmployerVoluntaryContributionPaymentType(dbConn).PaymentTypeID.ToString() + ", "
            + EPaymentType.SystemPaymentType.PFundEmployeeContributionPaymentType(dbConn).PaymentTypeID.ToString() + ", "
            + EPaymentType.SystemPaymentType.PFundEmployerContributionPaymentType(dbConn).PaymentTypeID.ToString()
            , new DBFilter()));
        uniquePaymentTypeFilter.add(new Match("pc.PaymentCodeID", "<>", CurID));
        paymentTypeFilter.add(new IN("not PaymentTypeID", "Select PaymentTypeID from PaymentCode pc", uniquePaymentTypeFilter));

        binding = new Binding(dbConn, db);
        binding.add(PaymentCodeID);
        binding.add(PaymentCode);
        binding.add(PaymentCodeDesc);
        binding.add(new CheckBoxBinder(db, PaymentCodeIsProrata));
        binding.add(new CheckBoxBinder(db, PaymentCodeIsProrataLeave));
        binding.add(new CheckBoxBinder(db, PaymentCodeIsProrataStatutoryHoliday));
        binding.add(new CheckBoxBinder(db, PaymentCodeIsMPF));
        binding.add(new CheckBoxBinder(db, PaymentCodeIsWages));
        binding.add(new CheckBoxBinder(db, PaymentCodeIsTopUp));
        binding.add(new CheckBoxBinder(db, PaymentCodeIsORSO));
        binding.add(new DropDownVLBinder(db, PaymentTypeID, EPaymentType.VLPaymentType, paymentTypeFilter));
        binding.add(new DropDownVLBinder(db, PaymentCodeRoundingRule, Values.VLRoundingRule));
        binding.add(new DropDownVLBinder(db, PaymentCodeDecimalPlace, Values.VLDecimalPlace));
        binding.add(new CheckBoxBinder(db, PaymentCodeRoundingRuleIsAbsoluteValue));
        binding.add(new CheckBoxBinder(db, PaymentCodeHideInPaySlip));
        binding.add(PaymentCodeDisplaySeqNo);
        binding.add(new CheckBoxBinder(db, PaymentCodeNotRemoveContributionFromTopUp));
        // Start 000159, Ricky So, 2015-01-23
        binding.add(new CheckBoxBinder(db, PaymentCodeIsHitRateBased));
        binding.add(new TextBoxBinder(db, PaymentCodeDefaultRateAtMonth1));
        binding.add(new TextBoxBinder(db, PaymentCodeDefaultRateAtMonth2));
        binding.add(new TextBoxBinder(db, PaymentCodeDefaultRateAtMonth3));
        // End 000159, Ricky So, 2015-01-23
        binding.init(Request, Session);


        HROne.Common.WebUtility.WebControlsLocalization(this, this.Controls);

        HROne.ProductLicense productLicense = WebUtils.productLicense(Session);
        if (productLicense.ProductType != HROne.ProductLicense.ProductLicenseType.HROne)
            PaymentCodeNotRemoveContributionFromTopUpRow.Visible = false;
        else
            PaymentCodeNotRemoveContributionFromTopUpRow.Visible = true;

        if (!Page.IsPostBack)
        {
            if (CurID > 0)
            {
                loadObject();

            }
            else
                toolBar.DeleteButton_Visible = false;
            loadTaxPayment();
        }
    }
    
    protected void Page_Prerender(object sender, EventArgs e)
    {
        Row_HitRateBasedDetail.Visible = PaymentCodeIsHitRateBased.Checked;
    }

    protected bool loadObject() 
    {
	    EPaymentCode obj=new EPaymentCode();
	    bool isNew=WebFormWorkers.loadKeys(db, obj, DecryptedRequest);
	    if(!db.select(dbConn, obj))
		    return false;

	    Hashtable values=new Hashtable();
        if (string.IsNullOrEmpty(obj.PaymentCodeRoundingRule))
        {
            obj.PaymentCodeRoundingRule = Values.ROUNDING_RULE_ROUND_TO;
            obj.PaymentCodeDecimalPlace = 2;
        }
        db.populate(obj, values);
        binding.toControl(values);
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
        // Start 0000020, KuangWei, 2014-08-22
        list.Add("M");
        // End 0000020, KuangWei, 2014-08-22
        TaxPaymentRepeater.DataSource = list;
        TaxPaymentRepeater.DataBind();


    }

    protected void TaxPaymentRepeater_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        string TaxFormCode = (string)e.Item.DataItem;

        Label lblTaxFormType = (Label)e.Item.FindControl("TaxFormType");
        lblTaxFormType.Text = "IR56" + TaxFormCode;
        DropDownList c = (DropDownList)e.Item.FindControl("TaxPayID");

        DBFilter TaxPaymentMapFilter = new DBFilter();
        TaxPaymentMapFilter.add(new Match("tpm.PaymentCodeID", CurID));

        DBFilter TaxPaymentFilter = new DBFilter();
        TaxPaymentFilter.add(new Match("TaxFormType", TaxFormCode));
        TaxPaymentFilter.add(new IN("TaxPayID", "Select tpm.TaxPayID FROM " + ETaxPaymentMap.db.dbclass.tableName + " tpm", TaxPaymentMapFilter));

        ArrayList taxPaymentList = ETaxPayment.db.select(dbConn, TaxPaymentFilter);
        string selected = null;
        if (taxPaymentList.Count > 0)
        {
            ETaxPayment taxPay = (ETaxPayment)taxPaymentList[0];
            selected = taxPay.TaxPayID.ToString();
        }
        DBFilter taxFormPaymentFilter = new DBFilter();
        taxFormPaymentFilter.add(new Match("TaxFormType", TaxFormCode));

        WebFormUtils.loadValues(dbConn, c, ETaxPayment.VLTaxPaymentWithNature, taxFormPaymentFilter, null, selected, "combobox.notselected");
        c.Attributes["TaxFormCode"] = TaxFormCode;
    }

    protected void Save_Click(object sender, EventArgs e)
    {
        EPaymentCode c = new EPaymentCode();

        Hashtable values = new Hashtable();
        binding.toValues(values);

        PageErrors errors = PageErrors.getErrors(db, Page.Master);
        errors.clear();

        db.validate(errors, values);

        if (!errors.isEmpty())
            return;

        db.parse(values, c);
        if (!AppUtils.checkDuplicate(dbConn, db, c, errors, "PaymentCode"))
            return;

        WebUtils.StartFunction(Session, FUNCTION_CODE);
        if (CurID < 0)
        {
//            Utils.MarkCreate(Session, c);

            db.insert(dbConn, c);
            CurID = c.PaymentCodeID;
//            url = Utils.BuildURL(-1, CurID);
        }
        else
        {
//            Utils.Mark(Session, c);
            db.update(dbConn, c);
        }

        ArrayList list = new ArrayList();
        foreach (RepeaterItem item in TaxPaymentRepeater.Items)
        {
            DropDownList d = (DropDownList)item.FindControl("TaxPayID");
            string TaxFormCode = d.Attributes["TaxFormCode"];

            DBFilter TaxPaymentFilter = new DBFilter();
            TaxPaymentFilter.add(new Match("tp.TaxFormType", TaxFormCode));

            DBFilter TaxPaymentMapFilter = new DBFilter();
            TaxPaymentMapFilter.add(new Match("PaymentCodeID", c.PaymentCodeID));
            TaxPaymentMapFilter.add(new IN("TaxPayID", "Select tp.TaxPayID from " + ETaxPayment.db.dbclass.tableName + " tp", TaxPaymentFilter));
            ArrayList taxPaymentMapList = ETaxPaymentMap.db.select(dbConn, TaxPaymentMapFilter);
            if (taxPaymentMapList.Count > 0)
                for (int i = 0; i < taxPaymentMapList.Count; i++)
                {
                    ETaxPaymentMap taxPayMap = (ETaxPaymentMap)taxPaymentMapList[i];
                    if (i == 0)
                    {
                        int TaxPayID = 0;
                        if (!int.TryParse(d.SelectedValue, out TaxPayID))
                            ETaxPaymentMap.db.delete(dbConn, taxPayMap);
                        else
                            if (!TaxPayID.Equals(taxPayMap.TaxPayID))
                            {
                                taxPayMap.TaxPayID = TaxPayID;
                                ETaxPaymentMap.db.update(dbConn, taxPayMap);
                            }
                    }
                    else
                        ETaxPaymentMap.db.delete(dbConn, taxPayMap);
                }
            else
            {
                int TaxPayID = 0;
                if (int.TryParse(d.SelectedValue, out TaxPayID))
                {
                    ETaxPaymentMap taxPayMap = new ETaxPaymentMap();
                    taxPayMap.TaxPayID = TaxPayID;
                    taxPayMap.PaymentCodeID = c.PaymentCodeID;
                    ETaxPaymentMap.db.insert(dbConn, taxPayMap);
                }
            }
        }

        WebUtils.EndFunction(dbConn);
        HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "PaymentCode_View.aspx?PaymentCodeID="+CurID);


    }
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

    protected void Back_Click(object sender, EventArgs e)
    {
        if (CurID > 0)
            HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "PaymentCode_View.aspx?PaymentCodeID=" + CurID);
        else
            HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "PaymentCode_List.aspx");

    }

}
