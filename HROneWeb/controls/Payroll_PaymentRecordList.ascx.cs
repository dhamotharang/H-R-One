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

public partial class Payroll_PaymentRecordList : HROneWebControl
{
    protected DBManager db = EPaymentRecord.db;
    protected SearchBinding sBinding;
    protected ListInfo info;
    protected DataView view;
    public Binding newBinding;
    private double dblTotalPaymentAmount = 0;

    public bool IsAllowEdit = true;
    private bool m_IsTrialMode;
    

    private int CurID = 0;
    private string m_EmpPayStatus = "";

    public int CurrentEmpPayrollID
    {
        get { return int.TryParse(EmpPayrollID.Value, out CurID) ? CurID : -1; }
        set
        {
            CurID = value;
            EmpPayrollID.Value = CurID.ToString();
        }
    }

    public string CurrentEmpPayStatus
    {
        get { return m_EmpPayStatus; }
        set
        {
            m_EmpPayStatus = value;
            EmpPayStatus.Value = m_EmpPayStatus;
        }
    }

    public bool IsTrialMode
    {
        get { return m_IsTrialMode; }
        set
        {
            m_IsTrialMode = value;
        }
    }

    private string m_FunctionCode;
    public string FunctionCode
    {
        get { return m_FunctionCode; }
        set { m_FunctionCode = value; }
    }

    public event EventHandler Changed;
    DBFilter empBankAccountFilter = new DBFilter();

    protected void Page_Load(object sender, EventArgs e)
    {

        Delete.OnClientClick = HROne.Translation.PromptMessage.CreateDeleteConfirmDialogJavascript(Delete);

        dblTotalPaymentAmount = 0;

        sBinding = new SearchBinding(dbConn, db);
        sBinding.add(new HiddenMatchBinder(EmpPayrollID));
        sBinding.initValues("PaymentCodeID", null, EPaymentCode.VLPaymentCode, HROne.Common.WebUtility.GetSessionUICultureInfo(Session));
        sBinding.initValues("CurrencyID", null, Values.VLCurrency, HROne.Common.WebUtility.GetSessionUICultureInfo(Session));
        sBinding.initValues("PayRecMethod", null, Values.VLPaymentMethod, HROne.Common.WebUtility.GetSessionUICultureInfo(Session));
        sBinding.initValues("EmpAccID", null, EEmpBankAccount.VLBankAccount, HROne.Common.WebUtility.GetSessionUICultureInfo(Session));
        sBinding.initValues("PayRecIsRestDayPayment", null, Values.VLYesNo, HROne.Common.WebUtility.GetSessionUICultureInfo(Session));

        sBinding.init(DecryptedRequest, null);

        newBinding = new Binding(dbConn, db);
        newBinding.add(EmpPayrollID);
        newBinding.add(new DropDownVLBinder(db, PaymentCodeID, EPaymentCode.VLPaymentCode));
        newBinding.add(new DropDownVLBinder(db, CurrencyID, Values.VLCurrency).setNotSelected(null));
        newBinding.add(PayRecActAmount);
        newBinding.add(new DropDownVLBinder(db, PayRecMethod, Values.VLPaymentMethod).setNotSelected(null));
        newBinding.add(PayRecNumOfDayAdj);
        newBinding.add(new DropDownVLBinder(db, CostCenterID, ECostCenter.VLCostCenter).setNotSelected(HROne.Common.WebUtility.GetLocalizedString(ECostCenter.DEFAULT_COST_CENTER_TEXT)));
        newBinding.add(new CheckBoxBinder(db, PayRecIsRestDayPayment));
        newBinding.add(PayRecRemark);
        {
            DBFilter filter = new DBFilter();
            DBFilter empPayrollFilter = new DBFilter();
            //empPayrollFilter.add(new Match("EmpPayrollID", DecryptedRequest["EmpPayrollID"]));
            empPayrollFilter.add(new Match("EmpPayrollID", CurrentEmpPayrollID));
            filter.add(new IN("EmpID", "Select EmpID from EmpPayroll", empPayrollFilter));
            ArrayList empInfoList = EEmpPersonalInfo.db.select(dbConn, filter);

            if (empInfoList.Count > 0)
            {
                EEmpPersonalInfo tmpEmpInfo = (EEmpPersonalInfo)empInfoList[0];
                OR bankAccountORTerm = new OR();
                bankAccountORTerm.add(new Match("EmpID", tmpEmpInfo.EmpID));
                {
                    if (tmpEmpInfo.MasterEmpID > 0)
                        bankAccountORTerm.add(new Match("EmpID", tmpEmpInfo.MasterEmpID));
                }
                empBankAccountFilter.add(bankAccountORTerm);
            }
            else
                empBankAccountFilter.add(new Match("EmpID", 0));

            newBinding.add(new DropDownVLBinder(db, EmpAccID, EEmpBankAccount.VLBankAccount, empBankAccountFilter).setNotSelected(HROne.Common.WebUtility.GetLocalizedString(EEmpBankAccount.DEFAULT_BANK_ACCOUNT_TEXT)));
        }
        newBinding.init(Request, Session);

        info = ListFooter.ListInfo;
    }
    protected void Page_PreRender(object sender, EventArgs e)
    {

        if (!Page.IsPostBack)
        {
            if (CurID > 0)
                //if (loadObject())
                //{
                    view = loadData(info, db, Repeater);
                //}
        }
        if (IsTrialMode)
        {
            if (!IsAllowEdit)
            {
                AddPanel.Visible = false;
                Delete.Visible = false;
            }

        }
        else
        {
            if (!IsAllowEdit)
            {
                AddPanel.Visible = false;
                Delete.Visible = false;
            }
        }

        CostCenterHeaderCell.Visible = WebUtils.productLicense(Session).IsCostCenter;

        CostCenterDetailCell.Visible = WebUtils.productLicense(Session).IsCostCenter;
        RemarkCell.ColSpan = detailRow.Cells.Count - 5 - (WebUtils.productLicense(Session).IsCostCenter ? 0 : 1);

    }


    public DataView loadData(ListInfo info, DBManager db, DataList repeater)
    {
        DBFilter filter = sBinding.createFilter();
        if (info != null && info.orderby != null && !info.orderby.Equals(""))
            filter.add(info.orderby, info.order);

        string select = "c.*";
        string from = "from " + db.dbclass.tableName + " c ";

        //filter.add(new Match("EmpPayrollID",EmpPayrollID.Value));
        DataTable table = WebUtils.GetDataTableFromSelectQueryWithFilter(dbConn, select, from, filter, info);
        //HROne.Common.DBAESEncryptDoubleFieldAttribute.decode(table, "PayRecActAmountEnc", "PayRecActAmount");
        //HROne.Common.DBAESEncryptDoubleFieldAttribute.decode(table, "PayRecCalAmountEnc", "PayRecCalAmount");

        ListFooter.Refresh();
        view = new DataView(table);
        if (repeater != null)
        {
            repeater.DataSource = view;
            repeater.DataBind();

            lblPaymentTotal.Text = dblTotalPaymentAmount.ToString("$#,##0.00");
        }
        Changed(this, null);
        return view;
    }

    protected void ChangeOrder_Click(object sender, EventArgs e)
    {
        LinkButton l = (LinkButton)sender;
        String id = l.ID.Substring(1);
        if (info.orderby == null)
            info.order = true;
        else if (info.orderby.Equals(id))
            info.order = !info.order;
        else
            info.order = true;
        info.orderby = id;

        Repeater.EditItemIndex = -1;
        view = loadData(info, db, Repeater);

    }
    protected void Add_Click(object sender, EventArgs e)
    {
        //  Hard code set Currency to HKD
        CurrencyID.SelectedValue = "HKD";

        Repeater.EditItemIndex = -1;
        EPaymentRecord c = new EPaymentRecord();

        Hashtable values = new Hashtable();
        newBinding.toValues(values);

        PageErrors errors = PageErrors.getErrors(db, Page.Master);
        errors.clear();


        db.validate(errors, values);

        if (!errors.isEmpty())
            return;


        db.parse(values, c);

        EEmpPayroll empPayroll = new EEmpPayroll();
        empPayroll.EmpPayrollID = c.EmpPayrollID;

        if (EEmpPayroll.db.select(dbConn, empPayroll))
        {
            if (c.PayRecMethod.Equals("A"))
            {
                if (c.EmpAccID == 0)
                {
                    EEmpBankAccount bankAccount = EEmpBankAccount.GetDefaultBankAccount(dbConn, empPayroll.EmpID);
                    if (bankAccount != null)
                    {
                        //c.EmpAccID = bankAccount.EmpBankAccountID;
                    }
                    else
                        errors.addError("EmpAccID", HROne.Translation.PageErrorMessage.ERROR_ACCOUNT_REQUIRED);
                }
            }
            if (!errors.isEmpty())
                return;


            c.PayRecCalAmount = 0;
            //c.PayRecNumOfDayAdj = 0;
            c.PayRecStatus = PaymentRecordStatus.PAYRECORDSTATUS_ACTIVE;
            if (EmpPayStatus.Value == "C")
                c.PayRecType = PaymentRecordType.PAYRECORDTYPE_CONFIRM_ADJUSTMENT;
            else
                c.PayRecType = PaymentRecordType.PAYRECORDTYPE_TRIALRUN_ADJUSTMENT;

            WebUtils.StartFunction(Session, m_FunctionCode, empPayroll.EmpID);
            db.insert(dbConn, c);
            WebUtils.EndFunction(dbConn);

            PaymentCodeID.SelectedIndex = 0;
            CurrencyID.SelectedIndex = 0;
            PayRecActAmount.Text = string.Empty;
            PayRecMethod.SelectedIndex = 0;
            PayRecNumOfDayAdj.Text = string.Empty;
            PayRecRemark.Text = string.Empty;


            view = loadData(info, db, Repeater);
        }

        //Response.Redirect(Request.Url.LocalPath + "?" + Request.QueryString);
    }

    protected void Repeater_ItemDataBound(object sender, DataListItemEventArgs e)
    {
        EPaymentRecord obj = new EPaymentRecord();
        db.toObject(((DataRowView)e.Item.DataItem).Row, obj);
        Hashtable values = new Hashtable();
        db.populate(obj, values);
        dblTotalPaymentAmount += obj.PayRecActAmount;

        if (e.Item.ItemIndex == Repeater.EditItemIndex)
        {
            Binding eBinding;
            eBinding = new Binding(dbConn, db);
            eBinding.add((HtmlInputHidden)e.Item.FindControl("PayRecID"));
            eBinding.add(new DropDownVLBinder(db, (DropDownList)e.Item.FindControl("PaymentCodeID"), EPaymentCode.VLPaymentCode));
            eBinding.add(new DropDownVLBinder(db, (DropDownList)e.Item.FindControl("CurrencyID"), Values.VLCurrency));
            eBinding.add((TextBox)e.Item.FindControl("PayRecActAmount"));
            eBinding.add((TextBox)e.Item.FindControl("PayRecNumOfDayAdj"));
            eBinding.add(new DropDownVLBinder(db, (DropDownList)e.Item.FindControl("CostCenterID"), ECostCenter.VLCostCenter).setNotSelected(HROne.Common.WebUtility.GetLocalizedString(ECostCenter.DEFAULT_COST_CENTER_TEXT)));
            eBinding.add(new CheckBoxBinder(db, (CheckBox)e.Item.FindControl("PayRecIsRestDayPayment")));
            eBinding.add(new DropDownVLBinder(db, (DropDownList)e.Item.FindControl("PayRecMethod"), Values.VLPaymentMethod).setNotSelected(null));
            eBinding.add((TextBox)e.Item.FindControl("PayRecRemark"));

            //DBFilter filter = new DBFilter();
            //DBFilter inFilter = new DBFilter();
            //inFilter.add(new Match("EmpPayrollID", EmpPayrollID.Value));
            //filter.add(new IN("EmpID", "Select EmpID from EmpPayroll", inFilter));
            eBinding.add(new DropDownVLBinder(db, (DropDownList)e.Item.FindControl("EmpAccID"), EEmpBankAccount.VLBankAccount, empBankAccountFilter).setNotSelected(HROne.Common.WebUtility.GetLocalizedString(EEmpBankAccount.DEFAULT_BANK_ACCOUNT_TEXT)));



            eBinding.init(Request, Session);

            eBinding.toControl(values);

            if (EmpPayStatus.Value == "C" && obj.PayRecType != "A")
            {
                ((DropDownList)e.Item.FindControl("PaymentCodeID")).Enabled = false; ;
                ((TextBox)e.Item.FindControl("PayRecActAmount")).Enabled = false;
                ((TextBox)e.Item.FindControl("PayRecNumOfDayAdj")).Enabled = false;
                //((TextBox)e.Item.FindControl("PayRecRemark")).Enabled = false;
                e.Item.FindControl("DeleteItem").Visible = false;
            }

            ((HtmlTableCell)e.Item.FindControl("CostCenterDetailCell")).Visible = WebUtils.productLicense(Session).IsCostCenter;
            ((HtmlTableCell)e.Item.FindControl("RemarkCell")).ColSpan = ((HtmlTableRow)e.Item.FindControl("detailRow")).Cells.Count - 5 - (WebUtils.productLicense(Session).IsCostCenter ? 0 : 1);

        }
        else
        {
            Binding eBinding;
            eBinding = new Binding(dbConn, db);
            eBinding.add((HtmlInputHidden)e.Item.FindControl("PayRecID"));
            //eBinding.add(new DropDownVLBinder(db, (DropDownList)e.Item.FindControl("PaymentCodeID"), EPaymentCode.VLPaymentCode));
            //eBinding.add(new DropDownVLBinder(db, (DropDownList)e.Item.FindControl("CurrencyID"), Values.VLCurrency));
            //eBinding.add((TextBox)e.Item.FindControl("PayRecActAmount"));
            //eBinding.add((TextBox)e.Item.FindControl("PayRecNumOfDayAdj"));
            //eBinding.add(new DropDownVLBinder(db, (DropDownList)e.Item.FindControl("PayRecMethod"), Values.VLPaymentMethod));
            //eBinding.add((TextBox)e.Item.FindControl("PayRecRemark"));

            //DBFilter filter = new DBFilter();
            //DBFilter inFilter = new DBFilter();
            //inFilter.add(new Match("EmpPayrollID", EmpPayrollID.Value));
            //filter.add(new IN("EmpID", "Select EmpID from EmpPayroll", inFilter));
            eBinding.add(new BlankZeroLabelVLBinder(db, (Label)e.Item.FindControl("EmpAccID"), EEmpBankAccount.VLBankAccount, empBankAccountFilter).setTextDisplayForZero(HROne.Common.WebUtility.GetLocalizedString(EEmpBankAccount.DEFAULT_BANK_ACCOUNT_TEXT)));
            eBinding.add(new BlankZeroLabelVLBinder(db, (Label)e.Item.FindControl("CostCenterID"), ECostCenter.VLCostCenter).setTextDisplayForZero(HROne.Common.WebUtility.GetLocalizedString(ECostCenter.DEFAULT_COST_CENTER_TEXT)));


            eBinding.init(Request, Session);

            eBinding.toControl(values);

            e.Item.FindControl("Edit").Visible = IsAllowEdit;
            if (EmpPayStatus.Value == "C" && obj.PayRecType != "A")
            {
                e.Item.FindControl("DeleteItem").Visible = false;
            }
            else
            {
                e.Item.FindControl("DeleteItem").Visible = true & IsAllowEdit;
            }
            //HtmlInputHidden h = (HtmlInputHidden)e.Item.FindControl("PayRecID");
            //h.Value = ((DataRowView)e.Item.DataItem)["PayRecID"].ToString();

            ((HtmlTableCell)e.Item.FindControl("CostCenterDetailCell")).Visible = WebUtils.productLicense(Session).IsCostCenter;
            ((HtmlTableCell)e.Item.FindControl("RemarkCell")).ColSpan = ((HtmlTableRow)e.Item.FindControl("detailRow")).Cells.Count - 5 - (WebUtils.productLicense(Session).IsCostCenter ? 0 : 1);

        }
        HROne.Common.WebUtility.WebControlsLocalization(Session, e.Item.Controls);
    }

    protected void Repeater_ItemCommand(object source, DataListCommandEventArgs e)
    {
        Button b = (Button)e.CommandSource;




        if (b.ID.Equals("Edit"))
        {
            Repeater.EditItemIndex = e.Item.ItemIndex;
            //AddPanel.Visible = false;
            view = loadData(info, db, Repeater);
            WebUtils.SetEnabledControlSection(AddPanel, false);
        }
        else if (b.ID.Equals("Cancel"))
        {
            Repeater.EditItemIndex = -1;
            //AddPanel.Visible = IsAllowEdit;
            view = loadData(info, db, Repeater);
            WebUtils.SetEnabledControlSection(AddPanel, true);
        }
        else if (b.ID.Equals("Save"))
        {
            Binding eBinding;

            //  Hard code set Currency to HKD
            ((DropDownList)e.Item.FindControl("CurrencyID")).SelectedValue = "HKD";

            eBinding = new Binding(dbConn, db);
            eBinding.add(EmpPayrollID);
            eBinding.add((HtmlInputHidden)e.Item.FindControl("PayRecID"));
            eBinding.add(new DropDownVLBinder(db, (DropDownList)e.Item.FindControl("CurrencyID"), Values.VLCurrency));
            if (((DropDownList)e.Item.FindControl("PaymentCodeID")).Enabled)
                eBinding.add(new DropDownVLBinder(db, (DropDownList)e.Item.FindControl("PaymentCodeID"), EPaymentCode.VLPaymentCode));
            if (((TextBox)e.Item.FindControl("PayRecActAmount")).Enabled)
                eBinding.add((TextBox)e.Item.FindControl("PayRecActAmount"));
            if (((TextBox)e.Item.FindControl("PayRecNumOfDayAdj")).Enabled)
                eBinding.add((TextBox)e.Item.FindControl("PayRecNumOfDayAdj"));
            if (((TextBox)e.Item.FindControl("PayRecRemark")).Enabled)
                eBinding.add((TextBox)e.Item.FindControl("PayRecRemark"));
            eBinding.add(new DropDownVLBinder(db, (DropDownList)e.Item.FindControl("CostCenterID"), ECostCenter.VLCostCenter).setNotSelected(HROne.Common.WebUtility.GetLocalizedString(ECostCenter.DEFAULT_COST_CENTER_TEXT)));
            eBinding.add(new CheckBoxBinder(db, (CheckBox)e.Item.FindControl("PayRecIsRestDayPayment")));

            eBinding.add(new DropDownVLBinder(db, (DropDownList)e.Item.FindControl("PayRecMethod"), Values.VLPaymentMethod).setNotSelected(null));

            //DBFilter filter = new DBFilter();
            //DBFilter inFilter = new DBFilter();
            //inFilter.add(new Match("EmpPayrollID", EmpPayrollID.Value));
            //filter.add(new IN("EmpID", "Select EmpID from EmpPayroll", inFilter));
            eBinding.add(new DropDownVLBinder(db, (DropDownList)e.Item.FindControl("EmpAccID"), EEmpBankAccount.VLBankAccount, empBankAccountFilter).setNotSelected(HROne.Common.WebUtility.GetLocalizedString(EEmpBankAccount.DEFAULT_BANK_ACCOUNT_TEXT)));

            eBinding.init(Request, Session);


            EPaymentRecord obj = new EPaymentRecord();
            Hashtable values = new Hashtable();

            PageErrors errors = PageErrors.getErrors(db, Page.Master);
            errors.clear();


            eBinding.toValues(values);
            db.validate(errors, values);

            if (!errors.isEmpty())
            {
                HROne.Common.WebUtility.WebControlsLocalization(Session, e.Item.Controls);
                return;
            }

            db.parse(values, obj);

            EEmpPayroll empPayroll = new EEmpPayroll();
            empPayroll.EmpPayrollID = obj.EmpPayrollID;
            if (EEmpPayroll.db.select(dbConn, empPayroll))
            {

                if (obj.PayRecMethod.Equals("A"))
                {
                    if (obj.EmpAccID == 0)
                    {
                        EEmpBankAccount bankAccount = EEmpBankAccount.GetDefaultBankAccount(dbConn, empPayroll.EmpID);
                        if (bankAccount != null)
                        {
                            //obj.EmpAccID = bankAccount.EmpBankAccountID;
                        }
                        else
                            errors.addError("EmpAccID", HROne.Translation.PageErrorMessage.ERROR_ACCOUNT_REQUIRED);
                    }
                }

                if (!errors.isEmpty())
                {
                    HROne.Common.WebUtility.WebControlsLocalization(Session, e.Item.Controls);
                    return;
                }

                WebUtils.StartFunction(Session, m_FunctionCode, empPayroll.EmpID);
                db.update(dbConn, obj);
                WebUtils.EndFunction(dbConn);

                Repeater.EditItemIndex = -1;
                //AddPanel.Visible = IsAllowEdit;
                view = loadData(info, db, Repeater);
                WebUtils.SetEnabledControlSection(AddPanel, true);
            }

            //Response.Redirect(Request.Url.LocalPath + "?" + Request.QueryString);
        }


    }

    public void Refresh()
    {
        view = loadData(info, db, Repeater);
    }

    protected void Delete_Click(object sender, EventArgs e)
    {

        ArrayList list = new ArrayList();
        foreach (DataListItem item in Repeater.Items)
        {
            CheckBox c = (CheckBox)item.FindControl("DeleteItem");
            HtmlInputHidden h = (HtmlInputHidden)item.FindControl("PayRecID");
            if (c.Checked)
            {
                EPaymentRecord obj = new EPaymentRecord();
                obj.PayRecID = Int32.Parse(h.Value);
                list.Add(obj);
            }
        }
        EEmpPayroll empPayroll = new EEmpPayroll();
        empPayroll.EmpPayrollID = int.Parse(EmpPayrollID.Value);
        if (EEmpPayroll.db.select(dbConn, empPayroll))
        {

            WebUtils.StartFunction(Session, m_FunctionCode, empPayroll.EmpID);
            foreach (EPaymentRecord obj in list)
                if (db.select(dbConn, obj))
                    db.delete(dbConn, obj);
            WebUtils.EndFunction(dbConn);
        }
        view = loadData(info, db, Repeater);

        //Response.Redirect(Request.Url.LocalPath + "?" + Request.QueryString);
    }
}
