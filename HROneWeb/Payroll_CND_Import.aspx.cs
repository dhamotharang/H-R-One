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
using System.Data.OleDb;
using HROne.Import;
using HROne.Lib.Entities;
using HROne.DataAccess;
//using perspectivemind.validation;

public partial class Payroll_CND_Import : HROneWebPage
{
    private const string FUNCTION_CODE = "PAY003";

    private DBManager db = EUploadClaimsAndDeductions.db;
    protected SearchBinding sbinding;
    protected Binding ebinding;
    protected ListInfo info;
    private DataView view;
    

    protected void Page_Load(object sender, EventArgs e)
    {
       
        if (!WebUtils.CheckAccess(Response, Session, FUNCTION_CODE, WebUtils.AccessLevel.ReadWrite))
            return;

        sbinding = new SearchBinding(dbConn, EEmpPersonalInfo.db);
        //sbinding.add(new HiddenMatchBinder(EmpID));
        sbinding.initValues("PayCodeID", null, EPaymentCode.VLPaymentCode, HROne.Common.WebUtility.GetSessionUICultureInfo(Session));
        sbinding.initValues("CurrencyID", null, Values.VLCurrency, HROne.Common.WebUtility.GetSessionUICultureInfo(Session));
        sbinding.initValues("CNDPayMethod", null, Values.VLPaymentMethod, HROne.Common.WebUtility.GetSessionUICultureInfo(Session));
        sbinding.initValues("CNDIsRestDayPayment", null, Values.VLYesNo, null);

        sbinding.init(DecryptedRequest, null);

        info = ListFooter.ListInfo;

        HROne.Common.WebUtility.WebControlsLocalization(this, this.Controls);
    }

    protected void Page_PreRender(object sender, EventArgs e)
    {
        //if (!Page.IsPostBack)
        {
            view = loadData(info, db, Repeater);
        }

        CostCenterHeaderCell.Visible = WebUtils.productLicense(Session).IsCostCenter;
    }

    public DataView loadData(ListInfo info, DBManager db, DataList repeater)
    {
        //filter.add(new Match("EmpPayrollID",EmpPayrollID.Value));

        ImportClaimsAndDeductionsProcess CNDImport = new ImportClaimsAndDeductionsProcess(dbConn, Session.SessionID, WebUtils.GetCurUser(Session).UserID);
        DataTable table = CNDImport.GetImportDataFromTempDatabase(info);

        if (info != null)
            if (!string.IsNullOrEmpty(info.orderby))
                if (info.orderby.Equals("EmpEngFullName", StringComparison.CurrentCultureIgnoreCase))
                {
                    if (!table.Columns.Contains("EmpEngFullName"))
                    {
                        table.Columns.Add("EmpEngFullName", typeof(string));
                        foreach (System.Data.DataRow row in table.Rows)
                        {
                            EEmpPersonalInfo empInfo = new EEmpPersonalInfo();
                            empInfo.EmpID = (int)row["EmpID"];
                            if (EEmpPersonalInfo.db.select(dbConn, empInfo))
                                row["EmpEngFullName"] = empInfo.EmpEngFullName;
                        }
                    }
                }
        table = WebUtils.DataTableSortingAndPaging(table, info);

        view = new DataView(table);
        if (repeater != null)
        {
            repeater.DataSource = view;
            repeater.DataBind();
        }
        if (table.Rows.Count > 0)
            ImportSection.Visible = true;
        else
            ImportSection.Visible = false;

        return view;
    }

    protected void Upload_Click(object sender, EventArgs e)
    {
        PageErrors errors = PageErrors.getErrors(db, Page.Master);
        errors.clear();

        if (CNDImportFile.HasFile)
        {
            //DataTable dataTable = HROne.CSVProcess.CSVReader.parse(CNDImportFile.PostedFile.InputStream);
            string strTmpFolder = HROne.Common.Folder.GetOrCreateSessionTempFolder(Session.SessionID).FullName;; //Environment.GetFolderPath(Environment.SpecialFolder.InternetCache);
            string strTmpFile = System.IO.Path.Combine(strTmpFolder, AppUtils.ServerDateTime().ToString("~yyyyMMddHHmmss_") + CNDImportFile.FileName);
            CNDImportFile.SaveAs(strTmpFile);

            ImportClaimsAndDeductionsProcess CNDImport = new ImportClaimsAndDeductionsProcess(dbConn, Session.SessionID, WebUtils.GetCurUser(Session).UserID);
            //DataTable dataTable = HROne.Import.ExcelImport.parse(strTmpFile);
            //using (OleDbConnection conn = new OleDbConnection(@"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=C:\csv\;Extended Properties='Text;'"))
            try
            {
                DataTable table = CNDImport.UploadToTempDatabase(strTmpFile, WebUtils.GetCurUser(Session).UserID, string.Empty);
                //table = WebUtils.DataTableSortingAndPaging(table, info);
                //Repeater.DataSource = table;
                //Repeater.DataBind();
                ImportSection.Visible = true;
                txtRemark.Text = string.Empty;
            }
            catch (HRImportException ex)
            {
                if (CNDImport.errors.List.Count > 0)
                    foreach (string errorString in CNDImport.errors.List)
                        errors.addError(errorString);
                else
                    errors.addError(ex.Message);
                CNDImport.ClearTempTable();
            }
            System.IO.File.Delete(strTmpFile);
        }
        else
            errors.addError(HROne.Translation.PageErrorMessage.ERROR_INVALID_FILE);
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
        //view = loadData(info, db, Repeater);

    }

    protected void Repeater_ItemDataBound(object sender, DataListItemEventArgs e)
    {
        EUploadClaimsAndDeductions obj = new EUploadClaimsAndDeductions();
        db.toObject(((DataRowView)e.Item.DataItem).Row, obj);

        if (e.Item.ItemIndex == Repeater.EditItemIndex)
        {
            ebinding = new Binding(dbConn, db);
            ebinding.add((TextBox)e.Item.FindControl("CNDEffDate"));
            ebinding.add((HtmlInputHidden)e.Item.FindControl("CNDID"));
            ebinding.add(new DropDownVLBinder(db, (DropDownList)e.Item.FindControl("PayCodeID"), EPaymentCode.VLPaymentCode));
            ebinding.add(new DropDownVLBinder(db, (DropDownList)e.Item.FindControl("CurrencyID"), Values.VLCurrency));
            ebinding.add((TextBox)e.Item.FindControl("CNDAmount"));
            ebinding.add(new DropDownVLBinder(db, (DropDownList)e.Item.FindControl("CNDPayMethod"), Values.VLPaymentMethod));
            ebinding.add((TextBox)e.Item.FindControl("CNDRemark"));

            DBFilter empBankAccountFilter = new DBFilter();
            OR bankAccountORTerm = new OR();
            bankAccountORTerm.add(new Match("EmpID", obj.EmpID));
            {
                EEmpPersonalInfo tmpEmpInfo = new EEmpPersonalInfo();
                tmpEmpInfo.EmpID = obj.EmpID;
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
            ((HtmlTableCell)e.Item.FindControl("RemarkCell")).ColSpan = ((HtmlTableRow)e.Item.FindControl("detailRow")).Cells.Count - 4 - (WebUtils.productLicense(Session).IsCostCenter ? 0 : 1);
        }
        else
        {
            //((Button)e.Item.FindControl("Edit")).Visible = IsAllowEdit;
            //if (obj.PayRecID != null)
            //{
            //    ((Button)e.Item.FindControl("Edit")).Visible = false;
            //    ((CheckBox)e.Item.FindControl("DeleteItem")).Visible = false;
            //}
            HtmlInputHidden h = (HtmlInputHidden)e.Item.FindControl("UploadCNDID");
            h.Value = ((DataRowView)e.Item.DataItem)["UploadCNDID"].ToString();

            ebinding = new Binding(dbConn, db);
            ebinding.add(new BlankZeroLabelVLBinder(db, (Label)e.Item.FindControl("CostCenterID"), ECostCenter.VLCostCenter).setTextDisplayForZero(HROne.Common.WebUtility.GetLocalizedString(ECostCenter.DEFAULT_COST_CENTER_TEXT)));

            DBFilter empBankAccountFilter = new DBFilter();
            OR bankAccountORTerm = new OR();
            bankAccountORTerm.add(new Match("EmpID", obj.EmpID));
            {
                EEmpPersonalInfo tmpEmpInfo = new EEmpPersonalInfo();
                tmpEmpInfo.EmpID = obj.EmpID;
                if (EEmpPersonalInfo.db.select(dbConn, tmpEmpInfo) && tmpEmpInfo.MasterEmpID > 0)
                    bankAccountORTerm.add(new Match("EmpID", tmpEmpInfo.MasterEmpID));
            }
            empBankAccountFilter.add(bankAccountORTerm);
            ebinding.add(new BlankZeroLabelVLBinder(db, (Label)e.Item.FindControl("EmpAccID"), EEmpBankAccount.VLBankAccount, empBankAccountFilter).setTextDisplayForZero(HROne.Common.WebUtility.GetLocalizedString(EEmpBankAccount.DEFAULT_BANK_ACCOUNT_TEXT)));

            ebinding.init(Request, Session);

            Hashtable values = new Hashtable();
            db.populate(obj, values);

            ebinding.toControl(values);
            ((HtmlTableCell)e.Item.FindControl("CostCenterDetailCell")).Visible = WebUtils.productLicense(Session).IsCostCenter;
            ((HtmlTableCell)e.Item.FindControl("RemarkCell")).ColSpan = ((HtmlTableRow)e.Item.FindControl("detailRow")).Cells.Count - 4 - (WebUtils.productLicense(Session).IsCostCenter ? 0 : 1);
        }
        HROne.Common.WebUtility.WebControlsLocalization(this, e.Item.Controls);

    }
    protected void Import_Click(object sender, EventArgs e)
    {
        ImportClaimsAndDeductionsProcess CNDImport = new ImportClaimsAndDeductionsProcess(dbConn, Session.SessionID, WebUtils.GetCurUser(Session).UserID);
        CNDImport.Remark = txtRemark.Text;
        WebUtils.StartFunction(Session, FUNCTION_CODE);
        CNDImport.ImportToDatabase();
        WebUtils.EndFunction(dbConn);
        //loadData(info, db, Repeater);
        PageErrors.getErrors(db, Page).addError(HROne.Translation.PageMessage.IMPORT_SUCCESSFUL);

    }
    protected void ChangePage(object sender, EventArgs e)
    {
        //view = loadData(info, db, Repeater);
    }
}
