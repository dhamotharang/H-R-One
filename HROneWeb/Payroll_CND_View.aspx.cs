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

public partial class Payroll_CND_View : HROneWebPage
{
    private const string FUNCTION_CODE = "PAY003";
    protected SearchBinding sbinding;
    protected SearchBinding sPayrollProcessedBinding;
    protected ListInfo info;
    //protected DataView view;

    //public Binding binding;
    public Binding newBinding;
    public DBManager db = EClaimsAndDeductions.db;
    public EEmpPersonalInfo obj;
    public int CurID = -1;

    private bool IsAllowEdit = true;
    
    

    protected void Page_Load(object sender, EventArgs e)
    {
       
        if (!WebUtils.CheckAccess(Response, Session, FUNCTION_CODE, WebUtils.AccessLevel.Read))
            return;
        toolBar.FunctionCode = FUNCTION_CODE;
        if (!toolBar.DeleteButton_Visible) 
        {
            CNDAddPanel.Visible = false;
        //    Delete.Visible = false;
            IsAllowEdit = false;
        }

        if (!int.TryParse(DecryptedRequest["EmpID"], out CurID))
        {
            CurID = -1;
            CNDAddPanel.Visible = false;
            toolBar.DeleteButton_Visible = false;
            IsAllowEdit = false;
        }

        //binding = new Binding(dbConn, EEmpPersonalInfo.db);
        //binding.add(EmpID);

        sPayrollProcessedBinding = new SearchBinding(dbConn, db);
        sPayrollProcessedBinding.add(new HiddenMatchBinder(EmpID));
        sPayrollProcessedBinding.initValues("PayCodeID", null, EPaymentCode.VLPaymentCode, null);
        sPayrollProcessedBinding.initValues("CurrencyID", null, Values.VLCurrency, null);
        sPayrollProcessedBinding.initValues("CNDPayMethod", null, Values.VLPaymentMethod, null);
        sPayrollProcessedBinding.initValues("EmpAccID", null, EEmpBankAccount.VLBankAccount, null);
        sPayrollProcessedBinding.initValues("CNDIsRestDayPayment", null, Values.VLYesNo, null);
        sPayrollProcessedBinding.initValues("CostCenterID", null, ECostCenter.VLCostCenter, null);

        sbinding = new SearchBinding(dbConn, db);
        sbinding.add(new HiddenMatchBinder(EmpID));
        sbinding.initValues("PayCodeID", null, EPaymentCode.VLPaymentCode , HROne.Common.WebUtility.GetSessionUICultureInfo(Session));
        sbinding.initValues("CurrencyID", null, Values.VLCurrency , HROne.Common.WebUtility.GetSessionUICultureInfo(Session));
        sbinding.initValues("CNDPayMethod", null, Values.VLPaymentMethod , HROne.Common.WebUtility.GetSessionUICultureInfo(Session));
        sbinding.initValues("EmpAccID",null,EEmpBankAccount.VLBankAccount , HROne.Common.WebUtility.GetSessionUICultureInfo(Session));
        sbinding.initValues("CNDIsRestDayPayment", null, Values.VLYesNo, null);
        sbinding.initValues("CostCenterID", null, ECostCenter.VLCostCenter, null);
        sbinding.init(DecryptedRequest, null);

        newBinding = new Binding(dbConn, db);
        newBinding.add(EmpID);
        newBinding.add(new TextBoxBinder(db, CNDEffDate.TextBox, CNDEffDate.ID));
        newBinding.add(new DropDownVLBinder(db, PayCodeID, EPaymentCode.VLPaymentCode));
        newBinding.add(new DropDownVLBinder(db, CurrencyID, Values.VLCurrency));
        newBinding.add(CNDAmount);
        newBinding.add(new DropDownVLBinder(db, CNDPayMethod, Values.VLPaymentMethod).setNotSelected(null));
        newBinding.add(new DropDownVLBinder(db,CostCenterID, ECostCenter.VLCostCenter).setNotSelected(HROne.Common.WebUtility.GetLocalizedString(ECostCenter.DEFAULT_COST_CENTER_TEXT)));
        newBinding.add(new CheckBoxBinder(db, CNDIsRestDayPayment));
        newBinding.add(CNDNumOfDayAdj);
        newBinding.add(CNDRemark);

        DBFilter empBankAccountFilter = new DBFilter();
        OR bankAccountORTerm = new OR();
        bankAccountORTerm.add(new Match("EmpID", CurID));
        {
            EEmpPersonalInfo tmpEmpInfo = new EEmpPersonalInfo();
            tmpEmpInfo.EmpID = CurID;
            if (EEmpPersonalInfo.db.select(dbConn, tmpEmpInfo) && tmpEmpInfo.MasterEmpID > 0)
                bankAccountORTerm.add(new Match("EmpID", tmpEmpInfo.MasterEmpID));
        }
        empBankAccountFilter.add(bankAccountORTerm);
        newBinding.add(new DropDownVLBinder(db, EmpAccID, EEmpBankAccount.VLBankAccount, empBankAccountFilter).setNotSelected(HROne.Common.WebUtility.GetLocalizedString(EEmpBankAccount.DEFAULT_BANK_ACCOUNT_TEXT)));

        newBinding.init(Request, Session);


        EmpID.Value = CurID.ToString();

        info = ListFooter.ListInfo;

        HROne.Common.WebUtility.WebControlsLocalization(this, this.Controls);
    }

    protected void Page_PreRender(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            if (CurID > 0)
                if (!loadObject())
                {
                    CNDAddPanel.Visible = false;
                    toolBar.DeleteButton_Visible = false;
                    IsAllowEdit = false;
                }
            loadData(info, db , Repeater);
            PayrollProcessedLoadData(PayrollProcessedListFooter.ListInfo, db, PayrollProcessedRepeater);
        }

        CostCenterHeaderCell.Visible = WebUtils.productLicense(Session).IsCostCenter;
        PayrollProcessed_CostCenterHeaderCell.Visible = WebUtils.productLicense(Session).IsCostCenter;

        CostCenterDetailCell.Visible = WebUtils.productLicense(Session).IsCostCenter;
        RemarkCell.ColSpan = detailRow.Cells.Count - 5 - (WebUtils.productLicense(Session).IsCostCenter ? 0 : 1);
    }


    public DataView loadData(ListInfo info, DBManager db, DataList repeater)
    {
        DBFilter filter = sbinding.createFilter();

        //if (info != null && info.orderby != null && !info.orderby.Equals(""))
        //{
        //    info.orderby = "is_processed, " + info.orderby;
        //}
        //else
        //{
        //    info.orderby = "is_processed, CNDEffDate";
        //    info.order = false;
        //}

        //string select = "c.*, (CASE WHEN payrecid>0 THEN 1 Else 0 END) is_processed ";
        //string from = "from " + db.dbclass.tableName + " c ";

        if (info != null && info.orderby != null && !info.orderby.Equals(""))
        {
        }
        else
        {
            info.orderby = "CNDEffDate";
            info.order = false;
        }

        string select = "c.* ";
        string from = "from " + db.dbclass.tableName + " c ";

        filter.add(new NullTerm("PayRecID"));
        filter.add(WebUtils.AddRankFilter(Session, "c.EmpID", true));
        DataTable table = WebUtils.GetDataTableFromSelectQueryWithFilter(dbConn, select, from, filter, info);
        DataView view = new DataView(table);
        if (repeater != null)
        {
            repeater.DataSource = view;
            repeater.DataBind();
        }

        return view;
    }

    public DataView PayrollProcessedLoadData(ListInfo info, DBManager db, DataList repeater)
    {
        DBFilter filter = sPayrollProcessedBinding.createFilter();

        if (info != null && info.orderby != null && !info.orderby.Equals(""))
        {
        }
        else
        {
            info.orderby = "CNDEffDate";
            info.order = false;
        }

        string select = "c.* ";
        string from = "from " + db.dbclass.tableName + " c ";

        filter.add(new Match("PayRecID", ">", 0));
        //filter.add(new Match("EmpPayrollID", ">", 0));

        filter.add(WebUtils.AddRankFilter(Session, "c.EmpID", true));
        DataTable table = WebUtils.GetDataTableFromSelectQueryWithFilter(dbConn, select, from, filter, info);
        DataView view = new DataView(table);
        if (PayrollProcessedRepeater != null)
        {
            PayrollProcessedRepeater.DataSource = view;
            PayrollProcessedRepeater.DataBind();
        }

        return view;
    }    
    protected bool loadObject()
    {
        obj = new EEmpPersonalInfo();
        bool isNew = WebFormWorkers.loadKeys(EEmpPersonalInfo.db, obj, DecryptedRequest);

        DBFilter filter = new DBFilter();
        filter.add(new Match("EmpID", obj.EmpID));
        filter.add(WebUtils.AddRankFilter(Session, "EmpID", true));
        ArrayList empInfoList = EEmpPersonalInfo.db.select(dbConn, filter);
        if (empInfoList.Count == 0)
            return false;
        obj = (EEmpPersonalInfo)empInfoList[0];

        //Hashtable values = new Hashtable();
        //EEmpPersonalInfo.db.populate(obj, values);
        //binding.toControl(values);
        ucEmp_Header.CurrentEmpID  = obj.EmpID ;
        return true;
    }

    protected void ChangePage(object sender, EventArgs e)
    {
        PayrollProcessedLoadData(PayrollProcessedListFooter.ListInfo, db, PayrollProcessedRepeater);
    }

    protected void ChangeOrder_Click(object sender, EventArgs e)
    {
        LinkButton l = (LinkButton)sender;
        String id = l.ID.Substring(1);
        if (info.orderby == null)
            info.order = true;
        else if (info.orderby.Contains(id))
            info.order = !info.order;
        else
            info.order = true;
        info.orderby = id;

        Repeater.EditItemIndex = -1;
        loadData(info, db, Repeater);

    }
    protected void PayrollProcessedChangeOrder_Click(object sender, EventArgs e)
    {
        LinkButton l = (LinkButton)sender;
        String id = l.ID.Substring(l.ID.IndexOf("_") + 1);
        if (PayrollProcessedListFooter.ListInfo.orderby == null)
            PayrollProcessedListFooter.ListInfo.order = true;
        else if (PayrollProcessedListFooter.ListInfo.orderby.Contains(id))
            PayrollProcessedListFooter.ListInfo.order = !PayrollProcessedListFooter.ListInfo.order;
        else
            PayrollProcessedListFooter.ListInfo.order = true;
        PayrollProcessedListFooter.ListInfo.orderby = id;

        PayrollProcessedRepeater.EditItemIndex = -1;
        PayrollProcessedLoadData(PayrollProcessedListFooter.ListInfo, db, PayrollProcessedRepeater);

    }

    protected void Add_Click(object sender, EventArgs e)
    {
        //  Hard code set Currency to HKD
        CurrencyID.SelectedValue = "HKD";

        Repeater.EditItemIndex = -1;
        EClaimsAndDeductions c = new EClaimsAndDeductions();

        Hashtable values = new Hashtable();
        newBinding.toValues(values);

        PageErrors errors = PageErrors.getErrors(db, Page.Master);
        errors.clear();


        db.validate(errors, values);

        if (!errors.isEmpty())
            return;


        db.parse(values, c);

        if (c.CNDPayMethod.Equals("A"))
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
        else
            c.EmpAccID = 0;
        if (!errors.isEmpty())
            return;

        WebUtils.StartFunction(Session, FUNCTION_CODE, c.EmpID);
        db.insert(dbConn, c);
        WebUtils.EndFunction(dbConn);

        CNDEffDate.Value = string.Empty;
        PayCodeID.SelectedIndex = 0;
        CurrencyID.SelectedIndex = 0;
        CNDAmount.Text = string.Empty;
        CNDPayMethod.SelectedIndex = 0;
        CNDRemark.Text = string.Empty;
        EmpAccID.SelectedIndex = 0;
        CNDIsRestDayPayment.Checked = false;
        CNDNumOfDayAdj.Text = string.Empty;
        loadData(info, db, Repeater);
    }

    protected void Repeater_ItemDataBound(object sender, DataListItemEventArgs e)
    {
        Binding ebinding;

        EClaimsAndDeductions obj = new EClaimsAndDeductions();
        db.toObject(((DataRowView)e.Item.DataItem).Row, obj);

        if (e.Item.ItemIndex == Repeater.EditItemIndex)
        {
            ebinding = new Binding(dbConn, db);
            ebinding.add(new TextBoxBinder(db, ((WebDatePicker)e.Item.FindControl("CNDEffDate")).TextBox, "CNDEffDate"));
            ebinding.add((HtmlInputHidden)e.Item.FindControl("CNDID"));
            ebinding.add(new DropDownVLBinder(db, (DropDownList)e.Item.FindControl("PayCodeID"), EPaymentCode.VLPaymentCode));
            ebinding.add(new DropDownVLBinder(db, (DropDownList)e.Item.FindControl("CurrencyID"), Values.VLCurrency));
            ebinding.add((TextBox)e.Item.FindControl("CNDAmount"));
            ebinding.add(new CheckBoxBinder(db, (CheckBox)e.Item.FindControl("CNDIsRestDayPayment")));
            ebinding.add(new DropDownVLBinder(db, (DropDownList)e.Item.FindControl("CNDPayMethod"), Values.VLPaymentMethod).setNotSelected(null));
            ebinding.add(new DropDownVLBinder(db, (DropDownList)e.Item.FindControl("CostCenterID"), ECostCenter.VLCostCenter).setNotSelected(HROne.Common.WebUtility.GetLocalizedString(ECostCenter.DEFAULT_COST_CENTER_TEXT)));
            ebinding.add((TextBox)e.Item.FindControl("CNDNumOfDayAdj"));
            ebinding.add((TextBox)e.Item.FindControl("CNDRemark"));

            DBFilter empBankAccountFilter = new DBFilter();
            OR bankAccountORTerm = new OR();
            bankAccountORTerm.add(new Match("EmpID", CurID));
            {
                EEmpPersonalInfo tmpEmpInfo = new EEmpPersonalInfo();
                tmpEmpInfo.EmpID = CurID;
                if (EEmpPersonalInfo.db.select(dbConn, tmpEmpInfo) && tmpEmpInfo.MasterEmpID > 0)
                    bankAccountORTerm.add(new Match("EmpID", tmpEmpInfo.MasterEmpID));
            }
            empBankAccountFilter.add(bankAccountORTerm);
            ebinding.add(new DropDownVLBinder(db, (DropDownList)e.Item.FindControl("EmpAccID"), EEmpBankAccount.VLBankAccount, empBankAccountFilter).setNotSelected(HROne.Common.WebUtility.GetLocalizedString(EEmpBankAccount.DEFAULT_BANK_ACCOUNT_TEXT)));



            ebinding.init(Request, Session);

            Hashtable values = new Hashtable();
            db.populate(obj, values);

            ebinding.toControl(values);

            ((HtmlTableCell)e.Item.FindControl("CostCenterDetailCell")).Visible = WebUtils.productLicense(Session).IsCostCenter;
            ((HtmlTableCell)e.Item.FindControl("RemarkCell")).ColSpan = ((HtmlTableRow)e.Item.FindControl("detailRow")).Cells.Count - 5 - (WebUtils.productLicense(Session).IsCostCenter ? 0 : 1);
        }
        else
        {
            ((Button)e.Item.FindControl("Edit")).Visible = IsAllowEdit;
            ((CheckBox)e.Item.FindControl("DeleteItem")).Visible = IsAllowEdit;
            if (obj.PayRecID != null)
            {
                ((Button)e.Item.FindControl("Edit")).Visible = false;
                ((CheckBox)e.Item.FindControl("DeleteItem")).Visible = false;
            }

            ebinding = new Binding(dbConn, db);
            ebinding.add((HtmlInputHidden)e.Item.FindControl("CNDID"));
            ebinding.add(new BlankZeroLabelVLBinder(db, (Label)e.Item.FindControl("CostCenterID"), ECostCenter.VLCostCenter).setTextDisplayForZero(HROne.Common.WebUtility.GetLocalizedString(ECostCenter.DEFAULT_COST_CENTER_TEXT)));

            DBFilter empBankAccountFilter = new DBFilter();
            OR bankAccountORTerm = new OR();
            bankAccountORTerm.add(new Match("EmpID", CurID));
            {
                EEmpPersonalInfo tmpEmpInfo = new EEmpPersonalInfo();
                tmpEmpInfo.EmpID = CurID;
                if (EEmpPersonalInfo.db.select(dbConn, tmpEmpInfo) && tmpEmpInfo.MasterEmpID > 0)
                    bankAccountORTerm.add(new Match("EmpID", tmpEmpInfo.MasterEmpID));
            }
            empBankAccountFilter.add(bankAccountORTerm);
            ebinding.add(new BlankZeroLabelVLBinder(db, (Label)e.Item.FindControl("EmpAccID"), EEmpBankAccount.VLBankAccount, empBankAccountFilter).setTextDisplayForZero(HROne.Common.WebUtility.GetLocalizedString(EEmpBankAccount.DEFAULT_BANK_ACCOUNT_TEXT)));

            Label PayPeriodID = (Label)e.Item.FindControl("PayPeriodID");
            if (PayPeriodID != null)
                ebinding.add(new BlankZeroLabelVLBinder(db, PayPeriodID, EPayrollPeriod.VLPayrollPeriod));

            ebinding.init(Request, Session);

            Hashtable values = new Hashtable();
            db.populate(obj, values);

            if (obj.EmpPayrollID is int)
            {
                int EmpPayrollID = (int)obj.EmpPayrollID;
                if (EmpPayrollID > 0)
                {
                    EEmpPayroll empPayroll = new EEmpPayroll();
                    empPayroll.EmpPayrollID = EmpPayrollID;
                    if (EEmpPayroll.db.select(dbConn, empPayroll))
                        values.Add("PayPeriodID", empPayroll.PayPeriodID.ToString());
                }
            }
            ebinding.toControl(values);

            ((HtmlTableCell)e.Item.FindControl("CostCenterDetailCell")).Visible = WebUtils.productLicense(Session).IsCostCenter;
            ((HtmlTableCell)e.Item.FindControl("RemarkCell")).ColSpan = ((HtmlTableRow)e.Item.FindControl("detailRow")).Cells.Count - 5 - (WebUtils.productLicense(Session).IsCostCenter ? 0 : 1) - (PayPeriodID != null ? 2 : 0);
        }

        HROne.Common.WebUtility.WebControlsLocalization(this, e.Item.Controls);

    }

    protected void Repeater_ItemCommand(object source, DataListCommandEventArgs e)
    {
        Button b = (Button)e.CommandSource;




        if (b.ID.Equals("Edit"))
        {
            Repeater.EditItemIndex = e.Item.ItemIndex;
            //CNDAddPanel.Visible = false;
            loadData(info, db, Repeater);
            WebUtils.SetEnabledControlSection(CNDAddPanel, false);
        }
        else if (b.ID.Equals("Cancel"))
        {
            Repeater.EditItemIndex = -1;
            //CNDAddPanel.Visible = IsAllowEdit;
            loadData(info, db, Repeater);
            WebUtils.SetEnabledControlSection(CNDAddPanel, true);
        }
        else if (b.ID.Equals("Save"))
        {
            Binding ebinding;

            //  Hard code set Currency to HKD
            ((DropDownList)e.Item.FindControl("CurrencyID")).SelectedValue = "HKD";

            ebinding = new Binding(dbConn, db);
            ebinding.add(EmpID);
            ebinding.add(new TextBoxBinder(db, ((WebDatePicker)e.Item.FindControl("CNDEffDate")).TextBox, "CNDEffDate"));
            ebinding.add((HtmlInputHidden)e.Item.FindControl("CNDID"));
            ebinding.add(new DropDownVLBinder(db, (DropDownList)e.Item.FindControl("PayCodeID"), EPaymentCode.VLPaymentCode));
            ebinding.add(new DropDownVLBinder(db, (DropDownList)e.Item.FindControl("CurrencyID"), Values.VLCurrency));
            ebinding.add((TextBox)e.Item.FindControl("CNDAmount"));
            ebinding.add(new CheckBoxBinder(db, (CheckBox)e.Item.FindControl("CNDIsRestDayPayment")));
            ebinding.add(new DropDownVLBinder(db, (DropDownList)e.Item.FindControl("CNDPayMethod"), Values.VLPaymentMethod).setNotSelected(null));
            ebinding.add(new DropDownVLBinder(db, (DropDownList)e.Item.FindControl("CostCenterID"), ECostCenter.VLCostCenter).setNotSelected(HROne.Common.WebUtility.GetLocalizedString(ECostCenter.DEFAULT_COST_CENTER_TEXT)));
            ebinding.add((TextBox)e.Item.FindControl("CNDNumOfDayAdj"));
            ebinding.add((TextBox)e.Item.FindControl("CNDRemark"));

            DBFilter empBankAccountFilter = new DBFilter();
            OR bankAccountORTerm = new OR();
            bankAccountORTerm.add(new Match("EmpID", CurID));
            {
                EEmpPersonalInfo tmpEmpInfo = new EEmpPersonalInfo();
                tmpEmpInfo.EmpID = CurID;
                if (EEmpPersonalInfo.db.select(dbConn, tmpEmpInfo) && tmpEmpInfo.MasterEmpID > 0)
                    bankAccountORTerm.add(new Match("EmpID", tmpEmpInfo.MasterEmpID));
            }
            empBankAccountFilter.add(bankAccountORTerm);
            ebinding.add(new DropDownVLBinder(db, (DropDownList)e.Item.FindControl("EmpAccID"), EEmpBankAccount.VLBankAccount, empBankAccountFilter).setNotSelected(HROne.Common.WebUtility.GetLocalizedString(EEmpBankAccount.DEFAULT_BANK_ACCOUNT_TEXT)));

            ebinding.init(Request, Session);


            EClaimsAndDeductions obj = new EClaimsAndDeductions();
            Hashtable values = new Hashtable();

            PageErrors errors = PageErrors.getErrors(db, Page.Master);
            errors.clear();


            ebinding.toValues(values);
            db.validate(errors, values);

            if (!errors.isEmpty())
            {
                HROne.Common.WebUtility.WebControlsLocalization(this, e.Item.Controls);
                return;
            }


            db.parse(values, obj);

            if (obj.CNDPayMethod.Equals("A"))
            {
                if (obj.EmpAccID == 0)
                {
                    EEmpBankAccount bankAccount = EEmpBankAccount.GetDefaultBankAccount(dbConn, int.Parse(EmpID.Value));
                    if (bankAccount != null)
                    {
                        //obj.EmpAccID = bankAccount.EmpBankAccountID;
                    }
                    else
                        errors.addError("EmpAccID", HROne.Translation.PageErrorMessage.ERROR_ACCOUNT_REQUIRED);
                }
            }
            else
                obj.EmpAccID = 0;

            if (!errors.isEmpty())
            {
                HROne.Common.WebUtility.WebControlsLocalization(this, e.Item.Controls);
                return;
            }

            WebUtils.StartFunction(Session, FUNCTION_CODE, obj.EmpID);
            db.update(dbConn, obj);
            WebUtils.EndFunction(dbConn);

            Repeater.EditItemIndex = -1;
            //CNDAddPanel.Visible = IsAllowEdit;
            loadData(info, db, Repeater);
            WebUtils.SetEnabledControlSection(CNDAddPanel, true);
        }


    }

    protected void Delete_Click(object sender, EventArgs e)
    {

        ArrayList list = new ArrayList();
        foreach (DataListItem item in Repeater.Items)
        {
            CheckBox c = (CheckBox)item.FindControl("DeleteItem");
            HtmlInputHidden h = (HtmlInputHidden)item.FindControl("CNDID");
            if (c.Checked)
            {
                EClaimsAndDeductions obj = new EClaimsAndDeductions();
                obj.CNDID = Int32.Parse(h.Value);
                list.Add(obj);
            }
        }
        WebUtils.StartFunction(Session, FUNCTION_CODE, CurID);
        foreach (EClaimsAndDeductions obj in list)
        {
            if (db.select(dbConn, obj))
                db.delete(dbConn, obj);
        }
        WebUtils.EndFunction(dbConn);
        loadData(info, db, Repeater);
    }
    protected void Back_Click(object sender, EventArgs e)
    {
        HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "Payroll_CND_List.aspx");
    }
}
