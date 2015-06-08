//using System;
//using System.Collections;
//using System.Configuration;
//using System.Data;
//using System.IO;
//using System.Web;
//using System.Web.Security;
//using System.Web.UI;
//using System.Web.UI.HtmlControls;
//using System.Web.UI.WebControls;
//using System.Web.UI.WebControls.WebParts;
//using HROne.DataAccess;
////using perspectivemind.validation;
//using HROne.Lib.Entities;
//using HROne.Payroll;
//using HROne.BankFile;

//public partial class Payroll_GenerateBatchBankFile_View : HROneWebPage
//{

//    private const string FUNCTION_CODE = "PAY010";

//    public Binding binding;

//    protected SearchBinding empSBinding;
//    protected SearchBinding payBatchSBinding;
//    public DBManager db = EPayrollGroup.db;
//    public EPayrollGroup obj;
//    public int CurID = -1;
//    public int CurPayPeriodID = -1;

//    protected ListInfo empInfo;
//    protected DataView empView;
//    protected ListInfo payBatchInfo;
//    protected DataView payBatchView;

//    private static WFValueList VLCompanyAccount = ECompany.VLBankAccount;//new WFDBList(ECompany.db, "CompanyID", "CompanyBankCode + '-' +CompanyBranchCode + '-' + CompanyBankAccountNo", "CompanyBankCode");
//    private static WFValueList VLUserName = new WFDBList(EUser.db, "UserID", "UserName");

//    private bool IsAllowEdit = true;

//    private BankFileControlInterface bankFileControl;

//    private const string BANK_FILE_DATASET_SESSION_NAME = "BankFileDataSet";
//    private const string BANK_FILE_GENERATE_DATETIME_SESSION_NAME = "BankFileDateTime";

//    private bool GENERATE_AUTOPAYLIST_AFTER_BANKFILE = false;

//    protected void Page_Load(object sender, EventArgs e)
//    {
//        if (!WebUtils.CheckAccess(Response, Session, FUNCTION_CODE, WebUtils.AccessLevel.Read))
//            return;
//        if (!WebUtils.CheckPermission(Session, FUNCTION_CODE, WebUtils.AccessLevel.ReadWrite))
//        {
//            EmployeeSelectAllPanel.Visible = false;
//            IsAllowEdit = false;
//        }
//        chkPayBatchCheckAll.Attributes.Add("onclick", "checkAll('" + payBatchRepeater.ClientID + "','ItemSelect',this.checked);");
        
//        binding = new Binding(dbConn, db);

//        DBFilter companyFilter = chkShowAllCompany.Checked ? null : GetCompanyBankAccountDBFilter();
//        binding.add(new DropDownVLBinder(ECompany.db, BankAccount, VLCompanyAccount, companyFilter, "CompanyID"));

//        binding.init(Request, Session);

//        empSBinding = new SearchBinding(dbConn, EEmpPersonalInfo.db);
//        payBatchSBinding = new SearchBinding(dbConn, EPayrollGroup.db);
//        payBatchSBinding.initValues("PayBatchFileGenBy", null, VLUserName, null);

//        bankFileControl = getBankFileControl();

//        HROne.Common.WebUtility.WebControlsLocalization(this, this.Controls);

//        empInfo = EmpListFooter.ListInfo;
//        payBatchInfo = PayBatchListFooter.ListInfo;

//    }

//    protected void Page_PreRender(object sender, EventArgs e)
//    {
//        if (!Page.IsPostBack)
//        {
//            //loadObject();
//            //if (CurID > 0)
//            //{
//            //    panelPayPeriod.Visible = true;
//            //    payBatchView = payBatchloadData(payBatchInfo, EPayrollBatch.db, payBatchRepeater);

//            //    empView = emploadData(empInfo, EEmpPayroll.db, empRepeater);
//            //}
//            //else
//            //    panelPayPeriod.Visible = false;
//            panelPayPeriod.Visible = false;

//        }
//        if (Session[BANK_FILE_GENERATE_DATETIME_SESSION_NAME] != null || !GENERATE_AUTOPAYLIST_AFTER_BANKFILE)
//        {
//            btnAutoPayList.Visible = true;
//            btnAutoPayListExcel.Visible = true;
//        }
//        else
//        {
//            btnAutoPayList.Visible = false;
//            btnAutoPayListExcel.Visible = false;
//        }

//    }
//    //protected bool loadObject()
//    //{
//    //    obj = new EPayrollGroup();
//    //    bool isNew = WebFormWorkers.loadKeys(db, obj, DecryptedRequest);
//    //    if (!db.select(dbConn, obj))
//    //        return false;

//    //    Hashtable values = new Hashtable();
//    //    db.populate(obj, values);
//    //    binding.toControl(values);

//    //    if (CurPayPeriodID <= 0)
//    //    {
//    //        CurPayPeriodID = obj.CurrentPayPeriodID;
//    //    }

//    //    if (obj.CurrentPayPeriodID > 0)
//    //        panelBankFileInfo.Visible = true;
//    //    else
//    //        panelBankFileInfo.Visible = false;


//    //    return true;
//    //}



//    public DataView payBatchloadData(ListInfo info, DBManager db, Repeater repeater)
//    {
//        DateTime dtPeriodFr = new DateTime(), dtPeriodTo = new DateTime();

//        if (DateTime.TryParse(PayPeriodFr.Value, out dtPeriodFr) && DateTime.TryParse(PayPeriodTo.Value, out dtPeriodTo))
//        {

//            DBFilter filter = payBatchSBinding.createFilter();

//            //if (info != null && info.orderby != null && !info.orderby.Equals(""))
//            //    filter.add(info.orderby, info.order);

//            string select = "pg.PayGroupCode, pg.PayGroupDesc, pp.PayPeriodFr, pp.PayPeriodTo, pb.* ";
//            string from = "from [" + EPayrollGroup.db.dbclass.tableName + "] pg, [" + EPayrollPeriod.db.dbclass.tableName + "] pp, [" + EPayrollBatch.db.dbclass.tableName + "] pb ";

//            filter.add(new MatchField("pg.PayGroupID", "pp.PayGroupID"));
//            filter.add(new Match("pp.PayPeriodFr", ">=", dtPeriodFr));
//            filter.add(new Match("pp.PayPeriodTo", "<=", dtPeriodTo));


//            DBFilter empPayrollFilter = new DBFilter();
//            //empPayrollFilter.add(new MatchField("pb.payBatchID", "ep.payBatchID"));
//            empPayrollFilter.add(new Match("ep.EmpPayStatus", "C"));
//            empPayrollFilter.add(new MatchField("ep.PayPeriodID", "pp.PayPeriodID"));

//            DBFilter payRecordFilter = new DBFilter();
//            //payRecordFilter.add(new MatchField("ep.EmpPayrollID", "pr.EmpPayrollID"));

//            OR orPayMethodTerm = new OR();
//            orPayMethodTerm.add(new Match("PayRecMethod", "A"));
//            orPayMethodTerm.add(new Match("PayRecMethod", "Q"));

//            //if (bankFileControl != null)
//            //{
//            //    if (bankFileControl.IsAllowChequePayment())
//            //        orPayMethodTerm.add(new Match("PayRecMethod", "Q"));

//            //}
//            payRecordFilter.add(orPayMethodTerm);
//            //Exists existsPayRec = new Exists(EPaymentRecord.db.dbclass.tableName + " pr", payRecordFilter);
//            //empPayrollFilter.add(existsPayRec);

//            //Exists exists = new Exists("EmpPayroll ep", empPayrollFilter);
//            //        IN existsPayRec = new IN("ep.EmpPayrollID", "Select distinct pr.EmpPayrollID from (select EmpPayrollID,PayRecMethod from " + EPaymentRecord.db.dbclass.tableName + " group by EmpPayrollID, PayRecMethod) pr", payRecordFilter);
//            IN existsPayRec = new IN("ep.EmpPayrollID", "Select distinct pr.EmpPayrollID from (select distinct EmpPayrollID,PayRecMethod from " + EPaymentRecord.db.dbclass.tableName + ") pr", payRecordFilter);
//            empPayrollFilter.add(existsPayRec);

//            IN exists = new IN("pb.payBatchID", "Select distinct ep.payBatchID from " + EEmpPayroll.db.dbclass.tableName + " ep", empPayrollFilter);


//            filter.add(exists);
//            DataTable table = WebUtils.GetDataTableFromSelectQueryWithFilter(select, from, filter, payBatchInfo);

//            if (table.Rows.Count != 0)
//            {
//                panelPayPeriod.Visible = true;
//                ReportToolbarPanel.Visible = true & IsAllowEdit;
//            }
//            else
//            {
//                panelPayPeriod.Visible = false;
//                ReportToolbarPanel.Visible = false;
//            }
//            payBatchView = new DataView(table);

//            PayBatchListFooter.Refresh();

//            if (repeater != null)
//            {
//                repeater.DataSource = payBatchView;
//                repeater.DataBind();
//            }

//            return payBatchView;
//        }
//        else
//        {
//            panelPayPeriod.Visible = false;
//            ReportToolbarPanel.Visible = false;
//            return null;
//        }
//    }
//    protected void payBatchChangeOrder_Click(object sender, EventArgs e)
//    {
//        LinkButton l = (LinkButton)sender;
//        String id = l.ID.Substring(1);
//        if (payBatchInfo.orderby == null)
//            payBatchInfo.order = true;
//        else if (payBatchInfo.orderby.Equals(id))
//            payBatchInfo.order = !payBatchInfo.order;
//        else
//            payBatchInfo.order = true;
//        payBatchInfo.orderby = id;

//        payBatchView = payBatchloadData(payBatchInfo, EPayrollBatch.db, payBatchRepeater);

//    }
//    protected void payBatchRepeater_ItemDataBound(object sender, RepeaterItemEventArgs e)
//    {
//        DataRowView row = (DataRowView)e.Item.DataItem;
//        CheckBox cb = (CheckBox)e.Item.FindControl("ItemSelect");
//        WebFormUtils.LoadKeys(EPayrollBatch.db, row, cb);
//        if (row["PayBatchFileGenDate"] != DBNull.Value)
//            cb.Checked = false;
//    }

//    public DataView emploadData(ListInfo info, DBManager db, Repeater repeater)
//    {
//        Session.Remove(BANK_FILE_DATASET_SESSION_NAME);
//        Session.Remove(BANK_FILE_GENERATE_DATETIME_SESSION_NAME);

//        DateTime dtPeriodFr = new DateTime(), dtPeriodTo = new DateTime();

//        if (DateTime.TryParse(PayPeriodFr.Value, out dtPeriodFr) && DateTime.TryParse(PayPeriodTo.Value, out dtPeriodTo))
//        {
//            DBFilter filter = empSBinding.createFilter();

//            //if (info != null && info.orderby != null && !info.orderby.Equals(""))
//            //    filter.add(info.orderby, info.order);

//            string select = "* ";
//            string from = "from [" + EEmpPersonalInfo.db.dbclass.tableName + "] e ";

//            DBFilter empPayrollFilter = new DBFilter();
//            //empPayrollFilter.add(new MatchField("e.EmpID", "ep.EmpID"));
//            //empPayrollFilter.add(WebUtils.AddRankFilter(Session, "e.EmpID", true));

//            empPayrollFilter.add(new Match("ep.EmpPayStatus", "C"));
//            empPayrollFilter.add(new MatchField("ep.PayPeriodID", "pp.PayPeriodID"));
//            empPayrollFilter.add(new Match("pp.PayPeriodFr", ">=", dtPeriodFr));
//            empPayrollFilter.add(new Match("pp.PayPeriodTo", "<=", dtPeriodTo));

//            ArrayList payBatchList = WebUtils.SelectedRepeaterItemToBaseObjectList(EPayrollBatch.db, payBatchRepeater, "ItemSelect");
//            if (payBatchList.Count > 0)
//            {
//                OR orPayBatchID = new OR();
//                foreach (EPayrollBatch payBatch in payBatchList)
//                    orPayBatchID.add(new Match("ep.PayBatchID", payBatch.PayBatchID));
//                empPayrollFilter.add(orPayBatchID);
//            }
//            else
//            {
//                empPayrollFilter.add(new Match("ep.PayBatchID", 0));
//            }

//            DBFilter payRecordFilter = new DBFilter();
//            //payRecordFilter.add(new MatchField("ep.EmpPayrollID", "pr.EmpPayrollID"));

//            OR orPayMethodTerm = new OR();
//            orPayMethodTerm.add(new Match("PayRecMethod", "A"));

//            if (bankFileControl != null)
//            {
//                if (bankFileControl.IsAllowChequePayment())
//                    orPayMethodTerm.add(new Match("PayRecMethod", "Q"));

//            }

//            payRecordFilter.add(orPayMethodTerm);

//            //        IN existsPayRec = new IN("ep.EmpPayrollID", "Select distinct pr.EmpPayrollID from (select EmpPayrollID,PayRecMethod from " + EPaymentRecord.db.dbclass.tableName + " group by EmpPayrollID, PayRecMethod) pr", payRecordFilter);
//            IN existsPayRec = new IN("ep.EmpPayrollID", "Select distinct pr.EmpPayrollID from (select distinct EmpPayrollID,PayRecMethod from " + EPaymentRecord.db.dbclass.tableName + " ) pr", payRecordFilter);
//            empPayrollFilter.add(existsPayRec);

//            IN exists = new IN("e.EmpID", "Select distinct ep.EmpID from " + EEmpPayroll.db.dbclass.tableName + " ep, " + EPayrollPeriod.db.dbclass.tableName + " pp", empPayrollFilter);

//            filter.add(exists);
//            filter.add(WebUtils.AddRankFilter(Session, "e.EmpID", true));

//            DataTable table = WebUtils.GetDataTableFromSelectQueryWithFilter(dbConn, select, from, filter, info);


//            if (table.Rows.Count != 0)
//            {
//                ReportToolbarPanel.Visible = true & IsAllowEdit;
//            }
//            else
//            {
//                ReportToolbarPanel.Visible = false;
//            }
//            empView = new DataView(table);

//            EmpListFooter.Refresh();

//            if (repeater != null)
//            {
//                repeater.DataSource = empView;
//                repeater.DataBind();
//            }

//            return empView;
//        }
//        else
//        {
//            panelPayPeriod.Visible = false;
//            ReportToolbarPanel.Visible = false;
//            return null;
//        }
//    }
//    protected void empChangeOrder_Click(object sender, EventArgs e)
//    {
//        LinkButton l = (LinkButton)sender;
//        String id = l.ID.Substring(1);
//        if (empInfo.orderby == null)
//            empInfo.order = true;
//        else if (empInfo.orderby.Equals(id))
//            empInfo.order = !empInfo.order;
//        else
//            empInfo.order = true;
//        empInfo.orderby = id;

//        empView = emploadData(empInfo, EEmpPayroll.db, empRepeater);

//    }
//    protected void empRepeater_ItemDataBound(object sender, RepeaterItemEventArgs e)
//    {
//        DataRowView row = (DataRowView)e.Item.DataItem;
//        CheckBox cb = (CheckBox)e.Item.FindControl("ItemSelect");
//        WebFormUtils.LoadKeys(EEmpPersonalInfo.db, row, cb);
//        cb.Visible = IsAllowEdit;
//    }

//    protected void btnGenerate_Click(object sender, EventArgs e)
//    {
//        FileInfo bankFileInfo = CreateBankFile();
//        if (bankFileInfo != null)
//        {
//            if (Session[BANK_FILE_GENERATE_DATETIME_SESSION_NAME] != null)
//            {
//                DateTime lastGenerateDateTime = (DateTime)Session[BANK_FILE_GENERATE_DATETIME_SESSION_NAME];
//                WebUtils.RegisterDownloadFileJavaScript(this, bankFileInfo.FullName, "BankFile_" + lastGenerateDateTime.ToString("yyyyMMddHHmmss") + bankFileInfo.Extension, true, 0);
//            }
//        }
//    }

//    //protected void btnGenerateWithAutopayList_Click(object sender, EventArgs e)
//    //{
//    //    FileInfo bankFileInfo = CreateBankFile(true);
//    //    if (bankFileInfo != null)
//    //    {
//    //        WebUtils.TransmitFile(Response, bankFileInfo.FullName, "BankFile_" + AppUtils.ServerDateTime().ToString("yyyyMMddHHmmss") + bankFileInfo.Extension, true);
//    //    }

//    //}

//    protected FileInfo CreateBankFile()
//    {

//        PageErrors errors = PageErrors.getErrors(db, Page.Master);


//        ArrayList empList = WebUtils.SelectedRepeaterItemToBaseObjectList(EEmpPersonalInfo.db, empRepeater, "ItemSelect");
//        ArrayList payBatchList = WebUtils.SelectedRepeaterItemToBaseObjectList(EPayrollBatch.db, payBatchRepeater, "ItemSelect");

//        DateTime processDateTime = AppUtils.ServerDateTime();


//        if (empList.Count > 0 && payBatchList.Count > 0)
//        {
//            GenericBankFile bankFile = null;
//            DateTime valueDate = new DateTime();
//            try
//            {
//                bankFile = getBankFileObject();
//                if (DateTime.TryParse(ValueDate.Value, out valueDate))
//                {
//                    bankFile.ValueDate = valueDate;
//                }
//                else
//                {
//                    errors.addError(HROne.Translation.PageErrorMessage.ERROR_INVALID_VALUE_DATE);
//                }
//                bankFile.LoadBankFileDetail(payBatchList, empList);
//            }
//            catch (NegativeAmountException ex)
//            {
//                errors.addError("Negative amount detected");
//                ArrayList errorBankFileList = ex.GetErrorBankFileDetailList();
//                foreach (GenericBankFileDetail detail in errorBankFileList)
//                {
//                    errors.addError("- EmpNo: " + detail.EmpNo + ", Amount: " + detail.Amount);
//                }
//            }
//            catch (InvalidEEBankAccountException ex)
//            {
//                errors.addError(ex.Message);
//                errors.addError("- " + ex.EmpNo + " - " + ex.EmpName);
//            }
//            catch (Exception ex)
//            {
//                errors.addError(ex.Message);
//            }
//            if (bankFile != null)
//            {
//                if (BankAccount.SelectedValue != string.Empty)
//                {
//                    ECompany company = new ECompany();
//                    company.CompanyID = int.Parse(BankAccount.SelectedValue);
//                    ECompany.db.select(dbConn, company);
//                    bankFile.BankCode = company.CompanyBankCode.Trim();
//                    bankFile.BranchCode = company.CompanyBranchCode.Trim();
//                    bankFile.AccountNo = company.CompanyBankAccountNo.Trim();
//                    bankFile.AccountHolderName = company.CompanyBankHolderName.Trim().ToUpper();
//                }
//                else
//                {
//                    errors.addError("Bank Account has not been selected.");
//                }
//                if (errors.isEmpty())
//                {
//                    DateTime generateDate = AppUtils.ServerDateTime();
//                    FileInfo bankFileInfo;
//                    try
//                    {
//                        bankFileInfo = bankFile.GenerateBankFile();
//                    }
//                    catch (Exception ex)
//                    {
//                        errors.addError(ex.Message);
//                        return null;
//                    }
//                    foreach (EPayrollBatch payBatch in payBatchList)
//                    {
//                        payBatch.PayBatchFileGenDate = generateDate;
//                        payBatch.PayBatchValueDate = valueDate;
//                        payBatch.PayBatchFileGenBy = WebUtils.GetCurUser(Session).UserID;
//                        EPayrollBatch.db.update(dbConn, payBatch);
//                    }
//                    PayrollProcess.UpdateEmpPayrollValueDate(payBatchList, empList, valueDate, true, bankFile.IsGenerateChequePayment, false, false);
//                    Session[BANK_FILE_GENERATE_DATETIME_SESSION_NAME] = AppUtils.ServerDateTime();
//                    Session[BANK_FILE_DATASET_SESSION_NAME] = bankFile.CreateAutopayListDataSet();
//                    return bankFileInfo;

//                }
//            }
//        }
//        else
//            errors.addError("Employee or Payroll Batch not selected");
//        return null;
//    }
//    private GenericBankFile getBankFileObject()
//    {
//        GenericBankFile bankFileObject=null;

//        if (bankFileControl != null)
//            bankFileObject = bankFileControl.CreateBankFileObject();
//        else
//            bankFileObject = new GenericBankFile();
//        //if (BankFileType.SelectedValue.Equals("HSBC"))
//        //    bankFileObject = bankFileControlCreateBankFileObject();
//        //if (BankFileType.SelectedValue.Equals("SCB"))
//        //    bankFileObject = Payroll_GenerateBankFile_SCBControl.CreateBankFileObject();
//        //if (BankFileType.SelectedValue.Equals("CitiBank"))
//        //    bankFileObject = Payroll_GenerateBankFile_CitiBankControl.CreateBankFileObject();
//        //if (bankFileObject == null)
//        //    bankFileObject=new GenericBankFile();
//        return bankFileObject;
//    }
//    protected void btnRefreshEmpList_Click(object sender, EventArgs e)
//    {

//    }

//    private BankFileControlInterface getBankFileControl()
//    {
//        BankFileControlInterface tmpBankFileControl = null;

//        //if (BankFileType.SelectedValue.Equals("HSBC"))
//        //{
//        //    Payroll_GenerateBankFile_HSBCControl.Visible = true;
//        //    tmpBankFileControl = Payroll_GenerateBankFile_HSBCControl;
//        //}
//        //else
//        //    Payroll_GenerateBankFile_HSBCControl.Visible = false;
//        //if (BankFileType.SelectedValue.Equals("SCB"))
//        //{
//        //    Payroll_GenerateBankFile_SCBControl.Visible = true;
//        //    tmpBankFileControl = Payroll_GenerateBankFile_SCBControl;
//        //}
//        //else
//        //    Payroll_GenerateBankFile_SCBControl.Visible = false;
//        //if (BankFileType.SelectedValue.Equals("CitiBank"))
//        //{
//        //    Payroll_GenerateBankFile_CitiBankControl.Visible = true;
//        //    tmpBankFileControl = Payroll_GenerateBankFile_CitiBankControl;
//        //}
//        //else
//        //    Payroll_GenerateBankFile_CitiBankControl.Visible = false;
//        //if (BankFileType.SelectedValue.Equals("BOC"))
//        //{
//        //    Payroll_GenerateBankFile_BOCControl.Visible = true;
//        //    tmpBankFileControl = Payroll_GenerateBankFile_BOCControl;
//        //}
//        //else
//        //    Payroll_GenerateBankFile_BOCControl.Visible = false;
//        //if (BankFileType.SelectedValue.Equals("ICBC"))
//        //{
//        //    Payroll_GenerateBankFile_ICBCControl.Visible = true;
//        //    tmpBankFileControl = Payroll_GenerateBankFile_ICBCControl;
//        //}
//        //else
//        //    Payroll_GenerateBankFile_ICBCControl.Visible = false;

//        foreach (Control ctrl in this.BankControlListPanel.Controls)
//        {
//            if (ctrl is BankFileControlInterface)
//            {
//                if (ctrl.ID.EndsWith("_" + BankFileType.SelectedValue + "Control"))
//                {
//                    ctrl.Visible = true;
//                    tmpBankFileControl = (BankFileControlInterface)ctrl;
//                }
//                else
//                    ctrl.Visible = false;
//            }
//        }



//        return tmpBankFileControl;
//    }

//    protected void BankFileType_SelectedIndexChanged(object sender, EventArgs e)
//    {
//        BankFileType.Focus();
//        //payBatchView = payBatchloadData(payBatchInfo, EPayrollBatch.db, payBatchRepeater);
//        //empView = emploadData(empInfo, EEmpPayroll.db, empRepeater);

//    }
//    protected void btnAutoPayList_Click(object sender, EventArgs e)
//    {
//        PageErrors errors = PageErrors.getErrors(db, Page.Master);
//        errors.clear();

//        if (!GENERATE_AUTOPAYLIST_AFTER_BANKFILE)
//        {
//            ArrayList empList = WebUtils.SelectedRepeaterItemToBaseObjectList(EEmpPersonalInfo.db, empRepeater, "ItemSelect");
//            ArrayList payBatchList = WebUtils.SelectedRepeaterItemToBaseObjectList(EPayrollBatch.db, payBatchRepeater, "ItemSelect");

//            if (empList.Count > 0 && payBatchList.Count > 0)
//            {
//                try
//                {

//                    HROne.BankFile.AutopayListProcess rpt = new HROne.BankFile.AutopayListProcess(empList, payBatchList, DateTime.Parse(PayPeriodFr.Value));

//                    string reportFileName = WebUtils.GetLocalizedReportFile(Server.MapPath("~/Report_Payroll_Autopaylist.rpt"));
//                    string filename = WebUtils.ReportExportToFile(rpt, reportFileName, ((Button)sender).CommandArgument, true);
//                    if (File.Exists(filename))
//                    {
//                        FileInfo fileInfo = new System.IO.FileInfo(filename);
//                        WebUtils.RegisterDownloadFileJavaScript(this, fileInfo.FullName, "AutopayList_" + AppUtils.ServerDateTime().ToString("yyyyMMddHHmmss") + fileInfo.Extension, true, 0);
//                    }
//                }
//                catch (InvalidEEBankAccountException ex)
//                {
//                    errors.addError(ex.Message);
//                    errors.addError("- " + ex.EmpNo + " - " + ex.EmpName);
//                }
//                catch (Exception ex)
//                {
//                    errors.addError(ex.Message);
//                }
//            }
//            else
//                errors.addError("Employee or Payroll Batch not selected");
//        }
//        else if (Session[BANK_FILE_DATASET_SESSION_NAME] != null && Session[BANK_FILE_GENERATE_DATETIME_SESSION_NAME] != null)
//        {
//            DataSet dataSet = (DataSet)Session[BANK_FILE_DATASET_SESSION_NAME];
//            try
//            {

//                HROne.BankFile.AutopayListProcess rpt = new HROne.BankFile.AutopayListProcess(dataSet, DateTime.Parse(PayPeriodFr.Value));

//                string reportFileName = WebUtils.GetLocalizedReportFile(Server.MapPath("~/Report_Payroll_Autopaylist.rpt"));
//                string filename = WebUtils.ReportExportToFile(rpt, reportFileName, ((Button)sender).CommandArgument, true);
//                if (File.Exists(filename))
//                {
//                    FileInfo fileInfo = new System.IO.FileInfo(filename);
//                    DateTime lastGenerateDateTime = (DateTime)Session[BANK_FILE_GENERATE_DATETIME_SESSION_NAME];
//                    WebUtils.RegisterDownloadFileJavaScript(this, fileInfo.FullName, "AutopayList_" + lastGenerateDateTime.ToString("yyyyMMddHHmmss") + fileInfo.Extension, true, 0);
//                }
//            }
//            catch (InvalidEEBankAccountException ex)
//            {
//                errors.addError(ex.Message);
//                errors.addError("- " + ex.EmpNo + " - " + ex.EmpName);
//            }
//            catch (Exception ex)
//            {
//                errors.addError(ex.Message);
//            }
//        }

//        //        emploadData(empInfo, EEmpPayroll.db, empRepeater);

//    }
//    protected void BankAccount_SelectedIndexChanged(object sender, EventArgs e)
//    {
//        BankAccount.Focus();
//        string bankCode = BankAccount.SelectedItem.Text.PadRight(3).Substring(0, 3);
//        if (bankCode.Equals("003"))
//        {
//            BankFileType.SelectedValue = "SCB";
//            getBankFileControl();
//            return;
//        }
//        else if (bankCode.Equals("004") || bankCode.Equals("024"))
//        {
//            BankFileType.SelectedValue = "HSBC";
//            getBankFileControl();
//            return;
//        }
//        else if (bankCode.Equals("006"))
//        {
//            BankFileType.SelectedValue = "CitiBank";
//            getBankFileControl();
//            return;
//        }
//        else if (bankCode.Equals("012") || bankCode.Equals("036"))
//        {
//            BankFileType.SelectedValue = "BOC";
//            getBankFileControl();
//            return;
//        }
//        else if (bankCode.Equals("043"))
//        {
//            BankFileType.SelectedValue = "BOCNY";
//            getBankFileControl();
//            return;
//        }
//        else if (bankCode.Equals("214"))
//        {
//            BankFileType.SelectedValue = "ICBC";
//            getBankFileControl();
//            return;
//        }
//    }
//    protected void Search_Click(object sender, EventArgs e)
//    {
//        payBatchView = payBatchloadData(payBatchInfo, EPayrollBatch.db, payBatchRepeater);

//        empView = emploadData(empInfo, EEmpPayroll.db, empRepeater);

//    }
//    protected void PayPeriod_Changed(object sender, EventArgs e)
//    {
//        payBatchView = payBatchloadData(payBatchInfo, EPayrollBatch.db, payBatchRepeater);
//        empView = emploadData(empInfo, db, empRepeater);

//    }
//    protected void PayBatchItem_OnCheckedChanged(object sender, EventArgs e)
//    {
//        empView = emploadData(empInfo, EEmpPayroll.db, empRepeater);

//    }

//    private DBFilter GetCompanyBankAccountDBFilter()
//    {
//        DBFilter companyPayPeriod = new DBFilter();
//        companyPayPeriod.add(new MatchField("pp.PayPeriodID", "ep.PayPeriodID"));

//        DateTime dtPeriodFr = new DateTime(), dtPeriodTo = new DateTime();
//        if (DateTime.TryParse(PayPeriodFr.Value, out dtPeriodFr) && DateTime.TryParse(PayPeriodTo.Value, out dtPeriodTo))
//        {
//            companyPayPeriod.add(new Match("pp.PayPeriodFr", ">=", dtPeriodFr));
//            companyPayPeriod.add(new Match("pp.PayPeriodTo", "<=", dtPeriodTo));

//        }

//        OR orEmpPosEffFr = new OR();
//        orEmpPosEffFr.add(new MatchField("epi.EmpPosEffFr", "<=", "pp.PayPeriodTo"));
//        orEmpPosEffFr.add(new NullTerm("epi.EmpPosEffFr"));
//        companyPayPeriod.add(orEmpPosEffFr);

//        OR orEmpPosEffTo = new OR();
//        orEmpPosEffTo.add(new MatchField("epi.EmpPosEffTo", ">=", "pp.PayPeriodFr"));
//        orEmpPosEffTo.add(new NullTerm("epi.EmpPosEffTo"));
//        companyPayPeriod.add(orEmpPosEffTo);

//        DBFilter companyEmpPayrollFilter = new DBFilter();
//        companyEmpPayrollFilter.add(new MatchField("epi.EmpID", "ep.EmpID"));
//        companyEmpPayrollFilter.add(new Exists(EPayrollPeriod.db.dbclass.tableName + " pp", companyPayPeriod));

//        ArrayList payBatchList = WebUtils.SelectedRepeaterItemToBaseObjectList(EPayrollBatch.db, payBatchRepeater, "ItemSelect");
//        if (payBatchList.Count > 0)
//        {
//            OR orPayBatchID = new OR();
//            foreach (EPayrollBatch payBatch in payBatchList)
//                orPayBatchID.add(new Match("ep.PayBatchID", payBatch.PayBatchID));
//            companyEmpPayrollFilter.add(orPayBatchID);
//        }


//        DBFilter empPositionFilter = new DBFilter();
//        empPositionFilter.add(new IN("epi.EmpID", "Select Distinct ep.EmpID FROM " + EEmpPayroll.db.dbclass.tableName + " ep", companyEmpPayrollFilter));

//        DBFilter companyFilter = new DBFilter();
//        companyFilter.add(new IN("CompanyID", "Select Distinct epi.CompanyID from " + EEmpPositionInfo.db.dbclass.tableName + " epi", empPositionFilter));

//        return companyFilter;
//    }

//}
