using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.IO;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using HROne.DataAccess;
//using perspectivemind.validation;
using HROne.Lib.Entities;
using HROne.Payroll;
using HROne.BankFile;

public partial class Payroll_GenerateBankFile_View : HROneWebPage
{

    private const string FUNCTION_CODE = "PAY010";

    public Binding binding;

    protected SearchBinding empSBinding;
    public DBManager db = EPayrollGroup.db;
    public EPayrollGroup obj;
    public int CurID = -1;
    public int CurPayPeriodID = -1;

    protected ListInfo empInfo;
    protected DataView empView;

    private static WFValueList VLCompanyAccount = ECompanyBankAccount.VLBankAccount;//new WFDBList(ECompany.db, "CompanyID", "CompanyBankCode + '-' +CompanyBranchCode + '-' + CompanyBankAccountNo", "CompanyBankCode");
    private static WFValueList VLUserName = new WFDBList(EUser.db, "UserID", "UserName");

    private bool IsAllowEdit = true;

    private BankFileControlInterface bankFileControl;

    private const string BANK_FILE_DATASET_SESSION_NAME = "BankFileDataSet";
    private const string BANK_FILE_GENERATE_DATETIME_SESSION_NAME = "BankFileDateTime";

    private bool GENERATE_AUTOPAYLIST_AFTER_BANKFILE = false;
    

    protected void Page_Load(object sender, EventArgs e)
    {
       
        if (!WebUtils.CheckAccess(Response, Session, FUNCTION_CODE, WebUtils.AccessLevel.Read))
            return;
        if (!WebUtils.CheckPermission(Session, FUNCTION_CODE, WebUtils.AccessLevel.ReadWrite))
        {
            EmployeeSelectAllPanel.Visible = false;
            IsAllowEdit = false;
        }

        if (Session["CompanyDBID"] != null)
        {
            HROne.ProductLicense license = WebUtils.productLicense(Session);
            if (!license.HasAutopayMPFFileOthers)
            {
                BankFileType.Items.FindByValue("SCB").Enabled = false;
                BankFileType.Items.FindByValue("BOC").Enabled = false;
                BankFileType.Items.FindByValue("BOCNY").Enabled = false;
                BankFileType.Items.FindByValue("ICBC").Enabled = false;
                BankFileType.Items.FindByValue("CitiBank").Enabled = false;
                BankFileType.Items.FindByValue("ANZ").Enabled = false;
                BankFileType.Items.FindByValue("BOAmerica").Enabled = false;
                BankFileType.Items.FindByValue("DBS").Enabled = false;
                BankFileType.Items.FindByValue("").Enabled = false;
            }
            btnAutoPayList.Visible = false;
            btnAutoPayListExcel.Visible = false;
        }

        binding = new Binding(dbConn, db);

        //DBFilter companyFilter = chkShowAllCompany.Checked ? null : GetCompanyBankAccountDBFilter();
        //binding.add(new DropDownVLBinder(ECompanyBankAccount.db, BankAccount, VLCompanyAccount, companyFilter, "CompanyBankAccountID"));

        //binding.init(Request, Session);

        empSBinding = new SearchBinding(dbConn, EEmpPersonalInfo.db);


        HROne.Common.WebUtility.WebControlsLocalization(this, this.Controls);

        empInfo = EmpListFooter.ListInfo;

    }

    protected void Page_PreRender(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            //loadObject();
            //if (CurID > 0)
            //{
            //    panelPayPeriod.Visible = true;
            //    payBatchView = payBatchloadData(payBatchInfo, EPayrollBatch.db, payBatchRepeater);

            //    empView = emploadData(empInfo, EEmpPayroll.db, empRepeater);
            //}
            //else
            //    panelPayPeriod.Visible = false;
            //panelPayPeriod.Visible = false;

        }
        if ((Session[BANK_FILE_GENERATE_DATETIME_SESSION_NAME] != null || !GENERATE_AUTOPAYLIST_AFTER_BANKFILE) && Session["CompanyDBID"] == null)
        {
            btnAutoPayList.Visible = true;
            btnAutoPayListExcel.Visible = true;
        }
        else
        {
            btnAutoPayList.Visible = false;
            btnAutoPayListExcel.Visible = false;
        }

        DBFilter companyFilter = chkShowAllCompany.Checked ? null : GetCompanyBankAccountDBFilter();
        WebFormUtils.loadValues(dbConn, BankAccount, VLCompanyAccount, companyFilter, ci, BankAccount.SelectedValue, "combobox.notselected");
        RefreshBankFileControl();
    }
    //protected bool loadObject()
    //{
    //    obj = new EPayrollGroup();
    //    bool isNew = WebFormWorkers.loadKeys(db, obj, DecryptedRequest);
    //    if (!db.select(dbConn, obj))
    //        return false;

    //    Hashtable values = new Hashtable();
    //    db.populate(obj, values);
    //    binding.toControl(values);

    //    if (CurPayPeriodID <= 0)
    //    {
    //        CurPayPeriodID = obj.CurrentPayPeriodID;
    //    }

    //    if (obj.CurrentPayPeriodID > 0)
    //        panelBankFileInfo.Visible = true;
    //    else
    //        panelBankFileInfo.Visible = false;


    //    return true;
    //}



    public DataView emploadData(ListInfo info, DBManager db, Repeater repeater)
    {
        Session.Remove(BANK_FILE_DATASET_SESSION_NAME);
        Session.Remove(BANK_FILE_GENERATE_DATETIME_SESSION_NAME);

        DBFilter filter = empSBinding.createFilter();

        //if (info != null && info.orderby != null && !info.orderby.Equals(""))
        //    filter.add(info.orderby, info.order);

        string select = "* ";
        string from = "from [" + EEmpPersonalInfo.db.dbclass.tableName + "] e ";

        DBFilter empPayrollFilter = new DBFilter();
        empPayrollFilter.add(Payroll_PeriodSelectionList1.GetEmpPayrollDBTerm());


        DBFilter payRecordFilter = new DBFilter();
        //payRecordFilter.add(new MatchField("ep.EmpPayrollID", "pr.EmpPayrollID"));

        if (bankFileControl == null || !bankFileControl.IsShowAllPaymentMethod())
        {
            OR orPayMethodTerm = new OR();
            orPayMethodTerm.add(new Match("pr.PayRecMethod", "A"));
            if (bankFileControl != null)
            {
                if (bankFileControl.IsAllowChequePayment())
                    orPayMethodTerm.add(new Match("pr.PayRecMethod", "Q"));

            }
            payRecordFilter.add(orPayMethodTerm);
        }
        //        IN existsPayRec = new IN("ep.EmpPayrollID", "Select distinct pr.EmpPayrollID from (select EmpPayrollID,PayRecMethod from " + EPaymentRecord.db.dbclass.tableName + " group by EmpPayrollID, PayRecMethod) pr", payRecordFilter);
        IN existsPayRec = new IN("ep.EmpPayrollID", "Select distinct pr.EmpPayrollID from " + EPaymentRecord.db.dbclass.tableName + " pr", payRecordFilter);
        empPayrollFilter.add(existsPayRec);

        IN exists = new IN("e.EmpID", "Select distinct ep.EmpID from " + EEmpPayroll.db.dbclass.tableName + " ep, " + EPayrollPeriod.db.dbclass.tableName + " pp", empPayrollFilter);

        filter.add(exists);
        filter.add(WebUtils.AddRankFilter(Session, "e.EmpID", true));

        DataTable table = WebUtils.GetDataTableFromSelectQueryWithFilter(dbConn, select, from, filter, info);


        if (table.Rows.Count != 0)
        {
            ReportToolbarPanel.Visible = true & IsAllowEdit;
        }
        else
        {
            ReportToolbarPanel.Visible = false;
        }
        empView = new DataView(table);

        EmpListFooter.Refresh();

        if (repeater != null)
        {
            repeater.DataSource = empView;
            repeater.DataBind();
        }

        return empView;
        //}
        //else
        //{
        //    panelPayPeriod.Visible = false;
        //    ReportToolbarPanel.Visible = false;
        //    return null;
        //}
    }
    protected void empChangeOrder_Click(object sender, EventArgs e)
    {
        LinkButton l = (LinkButton)sender;
        String id = l.ID.Substring(1);
        if (empInfo.orderby == null)
            empInfo.order = true;
        else if (empInfo.orderby.Equals(id))
            empInfo.order = !empInfo.order;
        else
            empInfo.order = true;
        empInfo.orderby = id;

        empView = emploadData(empInfo, EEmpPayroll.db, empRepeater);

    }
    protected void empRepeater_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        DataRowView row = (DataRowView)e.Item.DataItem;
        CheckBox cb = (CheckBox)e.Item.FindControl("ItemSelect");
        WebFormUtils.LoadKeys(EEmpPersonalInfo.db, row, cb);
        cb.Visible = IsAllowEdit;
    }

    protected void btnGenerate_Click(object sender, EventArgs e)
    {
        PageErrors errors = PageErrors.getErrors(db, Page.Master);

        ArrayList empList = WebUtils.SelectedRepeaterItemToBaseObjectList(EEmpPersonalInfo.db, empRepeater, "ItemSelect");
        ArrayList payBatchList = Payroll_PeriodSelectionList1.GetPayBatchList();

        DateTime processDateTime = AppUtils.ServerDateTime();

        GenericBankFile bankFile = null;
        DateTime valueDate = new DateTime();

        if (empList.Count > 0 && payBatchList.Count > 0)
        {
            try
            {
                bankFile = getBankFileObject();
            }
            catch (Exception ex)
            {
                errors.addError(ex.Message);
                return;
            }
            if (ValueDateRow.Visible)
                if (DateTime.TryParse(ValueDate.Value, out valueDate))
                {
                    bankFile.ValueDate = valueDate;
                }
                else
                {
                    errors.addError(HROne.Translation.PageErrorMessage.ERROR_INVALID_VALUE_DATE);
                }
            if (bankFile != null)
            {
                if (BankAccount.SelectedValue != string.Empty)
                {
                    ECompanyBankAccount companyBankAccount = new ECompanyBankAccount();
                    companyBankAccount.CompanyBankAccountID = int.Parse(BankAccount.SelectedValue);
                    ECompanyBankAccount.db.select(dbConn, companyBankAccount);
                    bankFile.BankCode = companyBankAccount.CompanyBankAccountBankCode.Trim();
                    bankFile.BranchCode = companyBankAccount.CompanyBankAccountBranchCode.Trim();
                    bankFile.AccountNo = companyBankAccount.CompanyBankAccountAccountNo.Trim();
                    bankFile.AccountHolderName = companyBankAccount.CompanyBankAccountHolderName.Trim().ToUpper();

                    if (bankFile is HSBCBankFileEncrypted)
                    {


                        string keyFolderPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "BankKey");

                        string keyPath = string.Empty;
                        string HSBCKeyFileName = "HSBC.pub";
                        string HangSengKeyFileName = "HASE.pub";

                        string password = "11111111";
                        if (bankFile.BankCode.Equals("004"))
                        {
                            keyPath = System.IO.Path.Combine(keyFolderPath, HSBCKeyFileName);
                            if (Application["MasterDBConfig"] != null)
                            {
                                DatabaseConnection masterDBConn = ((DatabaseConfig)Application["MasterDBConfig"]).CreateDatabaseConnectionObject();
                                string HSBCKeyPath = HROne.SaaS.Entities.ESystemParameter.getParameter(masterDBConn, HROne.SaaS.Entities.ESystemParameter.PARAM_CODE_BANKKEY_HSBC_PATH);
                                string HSBCPassword = HROne.SaaS.Entities.ESystemParameter.getParameterWithEncryption(masterDBConn, HROne.SaaS.Entities.ESystemParameter.PARAM_CODE_BANKKEY_HSBC_PASSWORD);
                                if (!string.IsNullOrEmpty(HSBCKeyPath) && !string.IsNullOrEmpty(HSBCPassword))
                                {
                                    keyPath = HSBCKeyPath;
                                    password = HSBCPassword;
                                }
                            }
                        }
                        else if (bankFile.BankCode.Equals("024"))
                        {
                            keyPath = System.IO.Path.Combine(keyFolderPath, HangSengKeyFileName);
                            if (Application["MasterDBConfig"] != null)
                            {
                                DatabaseConnection masterDBConn = ((DatabaseConfig)Application["MasterDBConfig"]).CreateDatabaseConnectionObject();
                                string HASEKeyPath = HROne.SaaS.Entities.ESystemParameter.getParameter(masterDBConn, HROne.SaaS.Entities.ESystemParameter.PARAM_CODE_BANKKEY_HASE_PATH);
                                string HASEPassword = HROne.SaaS.Entities.ESystemParameter.getParameterWithEncryption(masterDBConn, HROne.SaaS.Entities.ESystemParameter.PARAM_CODE_BANKKEY_HASE_PASSWORD);
                                if (!string.IsNullOrEmpty(HASEKeyPath) && !string.IsNullOrEmpty(HASEPassword))
                                {
                                    keyPath = HASEKeyPath;
                                    password = HASEPassword;
                                }
                            }
                        }

                        if (System.IO.File.Exists(keyPath))
                        {
                            HSBCBankFileEncrypted bankFileEncrypted = (HSBCBankFileEncrypted)bankFile;

                            bankFileEncrypted.publicKeyFile = keyPath;
                            bankFileEncrypted.publicKeyPassword = password;
                        }
                        else
                        {
                            errors.addError("Bank Code is not supported.");
                        }
                    }
                }
                else
                {
                    errors.addError("Bank Account has not been selected.");
                }
            }

        }
        else
            errors.addError("Employee or Payroll Batch not selected");

        if (errors.isEmpty() && Response.IsClientConnected)
        {
            HROneConfig config = HROneConfig.GetCurrentHROneConfig();
            if (config.GenerateReportAsInbox)
            {
                if (EInboxAttachment.GetTotalSize(dbConn, 0) < WebUtils.productLicense(Session).MaxInboxSizeMB * 1000 * 1000)
                {
                    HROne.TaskService.GenerateBankFileTaskFactory reportTask = new HROne.TaskService.GenerateBankFileTaskFactory(dbConn, user, lblReportHeader.Text, bankFile, payBatchList, empList, valueDate, Payroll_PeriodSelectionList1.GetPeriodDateFrom(), ci);
                    AppUtils.reportTaskQueueService.AddTask(reportTask);
                    errors.addError(HROne.Translation.PageMessage.REPORT_GENERATING_TO_INBOX);
                }
                else
                    errors.addError(HROne.Translation.PageMessage.INBOX_SIZE_EXCEEDED);
            }
            else
            {
                try
                {
                    bankFile.LoadBankFileDetail(payBatchList, empList);
                    FileInfo bankFileInfo = bankFile.GenerateBankFile();
                    DateTime generateDate = AppUtils.ServerDateTime();

                    foreach (EPayrollBatch payBatch in payBatchList)
                    {
                        payBatch.PayBatchFileGenDate = generateDate;
                        payBatch.PayBatchValueDate = valueDate;
                        payBatch.PayBatchFileGenBy = WebUtils.GetCurUser(Session).UserID;
                        EPayrollBatch.db.update(dbConn, payBatch);
                    }
                    PayrollProcess.UpdateEmpPayrollValueDate(dbConn, payBatchList, empList, valueDate, true, bankFile.IsGenerateChequePayment, false, false);
                    Session[BANK_FILE_GENERATE_DATETIME_SESSION_NAME] = AppUtils.ServerDateTime();
                    Session[BANK_FILE_DATASET_SESSION_NAME] = bankFile.CreateAutopayListDataSet();

                    if (Session[BANK_FILE_GENERATE_DATETIME_SESSION_NAME] != null)
                    {
                        DateTime lastGenerateDateTime = (DateTime)Session[BANK_FILE_GENERATE_DATETIME_SESSION_NAME];
                        //WebUtils.RegisterDownloadFileJavaScript(this, bankFileInfo.FullName, "BankFile_" + lastGenerateDateTime.ToString("yyyyMMddHHmmss") + bankFileInfo.Extension, true, 0);
                        WebUtils.TransmitFile(Response, bankFileInfo.FullName, bankFile.ActualBankFileName(), true);
                    }
                }
                catch (NegativeAmountException ex)
                {
                    string message = HROne.Common.WebUtility.GetLocalizedStringByCode("ERROR_BANKFILE_NEGATIVE", "Bank file should not contain negative amount");
                    ArrayList errorBankFileList = ex.GetErrorBankFileDetailList();
                    foreach (GenericBankFileDetail detail in errorBankFileList)
                    {
                        message += "\r\n- " + HROne.Common.WebUtility.GetLocalizedString("EmpNo") + ": " + detail.EmpNo + ", " + HROne.Common.WebUtility.GetLocalizedString("Amount") + ": " + detail.Amount;
                    }
                    errors.addError(message);
                }
                catch (InvalidEEBankAccountException ex)
                {
                    string message = ex.Message;
                    message += "\r\n- " + ex.EmpNo + " - " + ex.EmpName;
                    errors.addError(message);
                }
                catch (Exception ex)
                {
                    errors.addError(ex.Message);
                }
            }
        }

    }

    //protected void btnGenerateWithAutopayList_Click(object sender, EventArgs e)
    //{
    //    FileInfo bankFileInfo = CreateBankFile(true);
    //    if (bankFileInfo != null)
    //    {
    //        WebUtils.TransmitFile(Response, bankFileInfo.FullName, "BankFile_" + AppUtils.ServerDateTime().ToString("yyyyMMddHHmmss") + bankFileInfo.Extension, true);
    //    }

    //}

    //protected FileInfo CreateBankFile()
    //{



    //    ArrayList empList = WebUtils.SelectedRepeaterItemToBaseObjectList(EEmpPersonalInfo.db, empRepeater, "ItemSelect");
    //    ArrayList payBatchList = Payroll_PeriodSelectionList1.GetPayBatchList();

    //    DateTime processDateTime = AppUtils.ServerDateTime();


    //    if (empList.Count > 0 && payBatchList.Count > 0)
    //    {
    //        GenericBankFile bankFile = null;
    //        DateTime valueDate = new DateTime();
    //        try
    //        {
    //            bankFile = getBankFileObject();
    //            if (DateTime.TryParse(ValueDate.Value, out valueDate))
    //            {
    //                bankFile.ValueDate = valueDate;
    //            }
    //            else
    //            {
    //                errors.addError(HROne.Translation.PageErrorMessage.ERROR_INVALID_VALUE_DATE);
    //            }
    //            bankFile.LoadBankFileDetail(payBatchList, empList);
    //        }
    //        catch (NegativeAmountException ex)
    //        {
    //            errors.addError(HROne.Translation.PageErrorMessage.ERROR_BANKFILE_NEGATIVE);
    //            ArrayList errorBankFileList = ex.GetErrorBankFileDetailList();
    //            foreach (GenericBankFileDetail detail in errorBankFileList)
    //            {
    //                errors.addError("- " + HROne.Common.WebUtility.GetLocalizedString("EmpNo") + ": " + detail.EmpNo + ", " + HROne.Common.WebUtility.GetLocalizedString("Amount") + ": " + detail.Amount);
    //            }
    //        }
    //        catch (InvalidEEBankAccountException ex)
    //        {
    //            errors.addError(ex.Message);
    //            errors.addError("- " + ex.EmpNo + " - " + ex.EmpName);
    //        }
    //        catch (Exception ex)
    //        {
    //            errors.addError(ex.Message);
    //        }
    //        if (bankFile != null)
    //        {
    //            if (BankAccount.SelectedValue != string.Empty)
    //            {
    //                ECompany company = new ECompany();
    //                company.CompanyID = int.Parse(BankAccount.SelectedValue);
    //                ECompany.db.select(dbConn, company);
    //                bankFile.BankCode = company.CompanyBankCode.Trim();
    //                bankFile.BranchCode = company.CompanyBranchCode.Trim();
    //                bankFile.AccountNo = company.CompanyBankAccountNo.Trim();
    //                bankFile.AccountHolderName = company.CompanyBankHolderName.Trim().ToUpper();
    //            }
    //            else
    //            {
    //                errors.addError("Bank Account has not been selected.");
    //            }
    //            if (errors.isEmpty())
    //            {
    //                DateTime generateDate = AppUtils.ServerDateTime();
    //                FileInfo bankFileInfo;
    //                try
    //                {
    //                    bankFileInfo = bankFile.GenerateBankFile();
    //                }
    //                catch (Exception ex)
    //                {
    //                    errors.addError(ex.Message);
    //                    return null;
    //                }
    //                foreach (EPayrollBatch payBatch in payBatchList)
    //                {
    //                    payBatch.PayBatchFileGenDate = generateDate;
    //                    payBatch.PayBatchValueDate = valueDate;
    //                    payBatch.PayBatchFileGenBy = WebUtils.GetCurUser(Session).UserID;
    //                    EPayrollBatch.db.update(dbConn, payBatch);
    //                }
    //                PayrollProcess.UpdateEmpPayrollValueDate(dbConn, payBatchList, empList, valueDate, true, bankFile.IsGenerateChequePayment, false, false);
    //                Session[BANK_FILE_GENERATE_DATETIME_SESSION_NAME] = AppUtils.ServerDateTime();
    //                Session[BANK_FILE_DATASET_SESSION_NAME] = bankFile.CreateAutopayListDataSet();
    //                return bankFileInfo;

    //            }
    //        }
    //    }
    //    else
    //        errors.addError("Employee or Payroll Batch not selected");
    //    return null;
    //}
    private GenericBankFile getBankFileObject()
    {
        RefreshBankFileControl();
        GenericBankFile bankFileObject = null;

        if (bankFileControl != null)
            bankFileObject = bankFileControl.CreateBankFileObject();
        else
            bankFileObject = new GenericBankFile(dbConn);
        //if (BankFileType.SelectedValue.Equals("HSBC"))
        //    bankFileObject = bankFileControlCreateBankFileObject();
        //if (BankFileType.SelectedValue.Equals("SCB"))
        //    bankFileObject = Payroll_GenerateBankFile_SCBControl.CreateBankFileObject();
        //if (BankFileType.SelectedValue.Equals("CitiBank"))
        //    bankFileObject = Payroll_GenerateBankFile_CitiBankControl.CreateBankFileObject();
        //if (bankFileObject == null)
        //    bankFileObject=new GenericBankFile();
        return bankFileObject;
    }
    protected void btnRefreshEmpList_Click(object sender, EventArgs e)
    {

    }

    private void RefreshBankFileControl()
    {
        BankFileControlInterface tmpBankFileControl = null;

        //if (BankFileType.SelectedValue.Equals("HSBC"))
        //{
        //    Payroll_GenerateBankFile_HSBCControl.Visible = true;
        //    tmpBankFileControl = Payroll_GenerateBankFile_HSBCControl;
        //}
        //else
        //    Payroll_GenerateBankFile_HSBCControl.Visible = false;
        //if (BankFileType.SelectedValue.Equals("SCB"))
        //{
        //    Payroll_GenerateBankFile_SCBControl.Visible = true;
        //    tmpBankFileControl = Payroll_GenerateBankFile_SCBControl;
        //}
        //else
        //    Payroll_GenerateBankFile_SCBControl.Visible = false;
        //if (BankFileType.SelectedValue.Equals("CitiBank"))
        //{
        //    Payroll_GenerateBankFile_CitiBankControl.Visible = true;
        //    tmpBankFileControl = Payroll_GenerateBankFile_CitiBankControl;
        //}
        //else
        //    Payroll_GenerateBankFile_CitiBankControl.Visible = false;
        //if (BankFileType.SelectedValue.Equals("BOC"))
        //{
        //    Payroll_GenerateBankFile_BOCControl.Visible = true;
        //    tmpBankFileControl = Payroll_GenerateBankFile_BOCControl;
        //}
        //else
        //    Payroll_GenerateBankFile_BOCControl.Visible = false;
        //if (BankFileType.SelectedValue.Equals("ICBC"))
        //{
        //    Payroll_GenerateBankFile_ICBCControl.Visible = true;
        //    tmpBankFileControl = Payroll_GenerateBankFile_ICBCControl;
        //}
        //else
        //    Payroll_GenerateBankFile_ICBCControl.Visible = false;
        bool blnPreviousPayChequeStatus = false;
        bool blnAfterPayChequeStatus = false;

        bool blnPreviousShowAllPayMethodStatus = false;
        bool blnAfterShowAllPayMethodStatus = false;

        foreach (Control ctrl in this.BankControlListPanel.Controls)
        {
            if (ctrl is BankFileControlInterface)
            {
                BankFileControlInterface currentBankFileControl = (BankFileControlInterface)ctrl;
                if (ctrl.Visible)
                {
                    blnPreviousPayChequeStatus = currentBankFileControl.IsAllowChequePayment();
                    blnPreviousShowAllPayMethodStatus = currentBankFileControl.IsShowAllPaymentMethod();
                }
                if (ctrl.ID.EndsWith("_" + BankFileType.SelectedValue + "Control"))
                {
                    ctrl.Visible = true;
                    tmpBankFileControl = currentBankFileControl;
                    ValueDateRow.Visible = tmpBankFileControl.HasValueDate();
                    blnAfterPayChequeStatus = currentBankFileControl.IsAllowChequePayment();
                    blnAfterShowAllPayMethodStatus = currentBankFileControl.IsShowAllPaymentMethod();
                }
                else
                    ctrl.Visible = false;
            }
        }

        bankFileControl = tmpBankFileControl;
        if (bankFileControl is Payroll_GenerateBankFile_HSBC)
        {
            string bankCode = BankAccount.SelectedItem.Text.PadRight(3).Substring(0, 3);
            if (bankCode.Equals("004"))
            {
                ((Payroll_GenerateBankFile_HSBC)bankFileControl).BankCode = "HSBC";
            }
            else if (bankCode.Equals("024"))
            {
                ((Payroll_GenerateBankFile_HSBC)bankFileControl).BankCode = "HangSeng";
            }
            else
            {
                ((Payroll_GenerateBankFile_HSBC)bankFileControl).BankCode = "(empty)";
            }

        }
        if (blnPreviousPayChequeStatus != blnAfterPayChequeStatus || blnPreviousShowAllPayMethodStatus != blnAfterShowAllPayMethodStatus)
        {
            empView = emploadData(empInfo, EEmpPayroll.db, empRepeater);
        }

    }

    protected void BankFileType_SelectedIndexChanged(object sender, EventArgs e)
    {
        BankFileType.Focus();
        //payBatchView = payBatchloadData(payBatchInfo, EPayrollBatch.db, payBatchRepeater);
        //empView = emploadData(empInfo, EEmpPayroll.db, empRepeater);

    }
    protected void btnAutoPayList_Click(object sender, EventArgs e)
    {
        PageErrors errors = PageErrors.getErrors(db, Page.Master);
        errors.clear();

        if (!GENERATE_AUTOPAYLIST_AFTER_BANKFILE)
        {
            ArrayList empList = WebUtils.SelectedRepeaterItemToBaseObjectList(EEmpPersonalInfo.db, empRepeater, "ItemSelect");
            ArrayList payBatchList = Payroll_PeriodSelectionList1.GetPayBatchList();

            if (empList.Count > 0 && payBatchList.Count > 0)
            {
                try
                {

                    HROne.BankFile.AutopayListProcess rpt = new HROne.BankFile.AutopayListProcess(dbConn, empList, payBatchList, Payroll_PeriodSelectionList1.GetPeriodDateFrom());

                    string reportFileName = WebUtils.GetLocalizedReportFile(Server.MapPath("~/Report_Payroll_Autopaylist.rpt"));
                    string filename = WebUtils.ReportExportToFile(rpt, reportFileName, ((Button)sender).CommandArgument, true);
                    if (File.Exists(filename))
                    {
                        FileInfo fileInfo = new System.IO.FileInfo(filename);
                        //WebUtils.RegisterDownloadFileJavaScript(this, fileInfo.FullName, "AutopayList_" + AppUtils.ServerDateTime().ToString("yyyyMMddHHmmss") + fileInfo.Extension, true, 0);
                        WebUtils.TransmitFile(Response, fileInfo.FullName, "AutopayList_" + AppUtils.ServerDateTime().ToString("yyyyMMddHHmmss") + fileInfo.Extension, true);
                    }
                }
                catch (NegativeAmountException ex)
                {
                    errors.addError(HROne.Translation.PageErrorMessage.ERROR_BANKFILE_NEGATIVE);
                    ArrayList errorBankFileList = ex.GetErrorBankFileDetailList();
                    foreach (GenericBankFileDetail detail in errorBankFileList)
                    {
                        errors.addError("- " + HROne.Common.WebUtility.GetLocalizedString("EmpNo") + ": " + detail.EmpNo + ", " + HROne.Common.WebUtility.GetLocalizedString("Amount") + ": " + detail.Amount);
                    }
                }
                catch (InvalidEEBankAccountException ex)
                {
                    errors.addError(ex.Message);
                    errors.addError("- " + ex.EmpNo + " - " + ex.EmpName);
                }
                catch (Exception ex)
                {
                    errors.addError(ex.Message);
                }
            }
            else
                errors.addError("Employee or Payroll Batch not selected");
        }
        else if (Session[BANK_FILE_DATASET_SESSION_NAME] != null && Session[BANK_FILE_GENERATE_DATETIME_SESSION_NAME] != null)
        {
            DataSet dataSet = (DataSet)Session[BANK_FILE_DATASET_SESSION_NAME];
            try
            {

                HROne.BankFile.AutopayListProcess rpt = new HROne.BankFile.AutopayListProcess(dataSet, Payroll_PeriodSelectionList1.GetPeriodDateFrom());

                string reportFileName = WebUtils.GetLocalizedReportFile(Server.MapPath("~/Report_Payroll_Autopaylist.rpt"));
                string filename = WebUtils.ReportExportToFile(rpt, reportFileName, ((Button)sender).CommandArgument, true);
                if (File.Exists(filename))
                {
                    FileInfo fileInfo = new System.IO.FileInfo(filename);
                    DateTime lastGenerateDateTime = (DateTime)Session[BANK_FILE_GENERATE_DATETIME_SESSION_NAME];
                    //WebUtils.RegisterDownloadFileJavaScript(this, fileInfo.FullName, "AutopayList_" + lastGenerateDateTime.ToString("yyyyMMddHHmmss") + fileInfo.Extension, true, 0);
                    WebUtils.TransmitFile(Response, fileInfo.FullName, lastGenerateDateTime.ToString("yyyyMMddHHmmss") + fileInfo.Extension, true);
                }
            }
            catch (InvalidEEBankAccountException ex)
            {
                errors.addError(ex.Message);
                errors.addError("- " + ex.EmpNo + " - " + ex.EmpName);
            }
            catch (Exception ex)
            {
                errors.addError(ex.Message);
            }
        }

        //        emploadData(empInfo, EEmpPayroll.db, empRepeater);

    }
    protected void BankAccount_SelectedIndexChanged(object sender, EventArgs e)
    {
        BankAccount.Focus();
        string bankCode = BankAccount.SelectedItem.Text.PadRight(3).Substring(0, 3);
        if (bankCode.Equals("003"))
        {
            BankFileType.SelectedValue = "SCB";
            RefreshBankFileControl();
            return;
        }
        else if (bankCode.Equals("004") || bankCode.Equals("024"))
        {
            BankFileType.SelectedValue = "HSBC";
            RefreshBankFileControl();
            return;
        }
        else if (bankCode.Equals("006"))
        {
            BankFileType.SelectedValue = "CitiBank";
            RefreshBankFileControl();
            return;
        }
        else if (bankCode.Equals("012") 
            || bankCode.Equals("014") 
            || bankCode.Equals("019") 
            || bankCode.Equals("026")
            || bankCode.Equals("030")
            || bankCode.Equals("031")
            || bankCode.Equals("033")
            || bankCode.Equals("036")
            || bankCode.Equals("064")
            || bankCode.Equals("070")
        )
        {
            BankFileType.SelectedValue = "BOC";
            RefreshBankFileControl();
            return;
        }
        else if (bankCode.Equals("016"))
        {
            BankFileType.SelectedValue = "DBS";
            RefreshBankFileControl();
            return;
        }
        else if (bankCode.Equals("043"))
        {
            BankFileType.SelectedValue = "BOCNY";
            RefreshBankFileControl();
            return;
        }
        else if (bankCode.Equals("055"))
        {
            BankFileType.SelectedValue = "BOAmerica";
            RefreshBankFileControl();
            return;
        }
        else if (bankCode.Equals("152"))
        {
            BankFileType.SelectedValue = "ANZ";
            RefreshBankFileControl();
            return;
        }
        else if (bankCode.Equals("214"))
        {
            BankFileType.SelectedValue = "ICBC";
            RefreshBankFileControl();
            return;
        }
    }
    //protected void Search_Click(object sender, EventArgs e)
    //{
    //    payBatchView = payBatchloadData(payBatchInfo, EPayrollBatch.db, payBatchRepeater);

    //    empView = emploadData(empInfo, EEmpPayroll.db, empRepeater);

    //}

    private DBFilter GetCompanyBankAccountDBFilter()
    {
        DBFilter companyPayPeriod = new DBFilter();
        companyPayPeriod.add(new MatchField("pp.PayPeriodID", "ep.PayPeriodID"));

        //DateTime dtPeriodFr = new DateTime(), dtPeriodTo = new DateTime();
        //if (DateTime.TryParse(PayPeriodFr.Value, out dtPeriodFr) && DateTime.TryParse(PayPeriodTo.Value, out dtPeriodTo))
        //{
        //    companyPayPeriod.add(new Match("pp.PayPeriodFr", ">=", dtPeriodFr));
        //    companyPayPeriod.add(new Match("pp.PayPeriodTo", "<=", dtPeriodTo));

        //}

        OR orEmpPosEffFr = new OR();
        orEmpPosEffFr.add(new MatchField("epi.EmpPosEffFr", "<=", "pp.PayPeriodTo"));
        orEmpPosEffFr.add(new NullTerm("epi.EmpPosEffFr"));
        companyPayPeriod.add(orEmpPosEffFr);

        OR orEmpPosEffTo = new OR();
        orEmpPosEffTo.add(new MatchField("epi.EmpPosEffTo", ">=", "pp.PayPeriodFr"));
        orEmpPosEffTo.add(new NullTerm("epi.EmpPosEffTo"));
        companyPayPeriod.add(orEmpPosEffTo);

        DBFilter companyEmpPayrollFilter = new DBFilter();
        companyEmpPayrollFilter.add(new MatchField("epi.EmpID", "ep.EmpID"));
        companyEmpPayrollFilter.add(new Exists(EPayrollPeriod.db.dbclass.tableName + " pp", companyPayPeriod));
        companyEmpPayrollFilter.add(Payroll_PeriodSelectionList1.GetEmpPayrollDBTerm("ep"));

        //ArrayList payBatchList = Payroll_PeriodSelectionList1.GetPayBatchList();//WebUtils.SelectedRepeaterItemToBaseObjectList(EPayrollBatch.db, payBatchRepeater, "ItemSelect");
        //if (payBatchList != null)
        //    if (payBatchList.Count > 0)
        //    {
        //        OR orPayBatchID = new OR();
        //        foreach (EPayrollBatch payBatch in payBatchList)
        //            orPayBatchID.add(new Match("ep.PayBatchID", payBatch.PayBatchID));
        //        companyEmpPayrollFilter.add(orPayBatchID);
        //    }


        DBFilter empPositionFilter = new DBFilter();
        empPositionFilter.add(new IN("epi.EmpID", "Select DISTINCT ep.EmpID FROM " + EEmpPayroll.db.dbclass.tableName + " ep", companyEmpPayrollFilter));

        DBFilter companyFilter = new DBFilter();
        companyFilter.add(new IN("CompanyID", "Select DISTINCT epi.CompanyID from " + EEmpPositionInfo.db.dbclass.tableName + " epi", empPositionFilter));

        DBFilter companyBankAccountMapFilter = new DBFilter();
        companyBankAccountMapFilter.add(new IN("CompanyBankAccountID", "SELECT DISTINCT CompanyBankAccountID FROM " + ECompanyBankAccountMap.db.dbclass.tableName, companyFilter));
        return companyBankAccountMapFilter;
    }
    protected void Payroll_PeriodSelectionList1_PayrollBatchChecked(object sender, EventArgs e)
    {
        empInfo.page = 0;
        RefreshBankFileControl();
        empView = emploadData(empInfo, EEmpPayroll.db, empRepeater);
    }

}
