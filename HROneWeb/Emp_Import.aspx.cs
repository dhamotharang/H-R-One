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

public partial class Emp_Import : HROneWebPage
{
    private const string FUNCTION_CODE = "PER999";
    private DBManager db = EUploadEmpPersonalInfo.db;
    protected SearchBinding sbinding;
    protected Binding ebinding;
    protected ListInfo info;
    private DataView view;
    

    protected void Page_Load(object sender, EventArgs e)
    {
        WebUtils.CheckAccess(Response, Session);
        ESystemFunction importFunctionCode = ESystemFunction.GetObjectByCode(dbConn, FUNCTION_CODE);

        if (importFunctionCode != null)
            if (!importFunctionCode.FunctionIsHidden)
            {
                if (!WebUtils.CheckAccess(Response, Session, FUNCTION_CODE, WebUtils.AccessLevel.ReadWrite))
                    return;
            }
            else
            {
                if (!WebUtils.CheckAccess(Response, Session, "PER001", WebUtils.AccessLevel.ReadWrite))
                    return;
            }
        else
            if (!WebUtils.CheckAccess(Response, Session, "PER001", WebUtils.AccessLevel.ReadWrite))
                return;
        
        sbinding = new SearchBinding(dbConn, db);
        sbinding.initValues("ImportAction", null, ImportDBObject.VLImportAction, HROne.Common.WebUtility.GetSessionUICultureInfo(Session));
        //sbinding.add(UploadEmpID);

        sbinding.init(DecryptedRequest, null);

        info = ListFooter.ListInfo;
        HROne.Common.WebUtility.WebControlsLocalization(this, this.Controls);

    }
    protected void Page_PreRender(object sender, EventArgs e)
    {
        //if (!IsPostBack)
            view = loadData(info, db, Repeater);
    }

    public DataView loadData(ListInfo info, DBManager db, DataList repeater)
    {
        //filter.add(new Match("EmpPayrollID",EmpPayrollID.Value));

        ImportEmpPersonalInfoProcess empImport = new ImportEmpPersonalInfoProcess(dbConn, Session.SessionID);
        DataTable table = empImport.GetImportDataFromTempDatabase(info);

        if (info != null)
            if (!string.IsNullOrEmpty(info.orderby))
                if (info.orderby.Equals("EmpEngFullName", StringComparison.CurrentCultureIgnoreCase))
                {
                    if (!table.Columns.Contains("EmpEngFullName"))
                    {
                        table.Columns.Add("EmpEngFullName", typeof(string));
                        foreach (System.Data.DataRow row in table.Rows)
                        {
                            EUploadEmpPersonalInfo empInfo = new EUploadEmpPersonalInfo();
                            empInfo.UploadEmpID = (int)row["UploadEmpID"];
                            if (EUploadEmpPersonalInfo.db.select(dbConn, empInfo))
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
        if (EmpImportFile.HasFile)
        {
            //DataTable dataTable = HROne.CSVProcess.CSVReader.parse(CNDImportFile.PostedFile.InputStream);
            string strTmpFolder = HROne.Common.Folder.GetOrCreateSessionTempFolder(Session.SessionID).FullName; ; //Environment.GetFolderPath(Environment.SpecialFolder.InternetCache);
            string strTmpFile = System.IO.Path.Combine(strTmpFolder, AppUtils.ServerDateTime().ToString("~yyyyMMddHHmmss_") + EmpImportFile.FileName);
            EmpImportFile.SaveAs(strTmpFile);

            if (FileFormatList.SelectedValue.Equals("EZ-Pay", StringComparison.CurrentCultureIgnoreCase))
            {
                //  Not supported
            }
            else
            {
                ImportEmpPersonalInfoProcess empImport = new ImportEmpPersonalInfoProcess(dbConn, Session.SessionID);
                //DataTable dataTable = HROne.Import.ExcelImport.parse(strTmpFile);
                //using (OleDbConnection conn = new OleDbConnection(@"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=C:\csv\;Extended Properties='Text;'"))
                WebUtils.StartFunction(Session, FUNCTION_CODE);
                //dbConn.BeginTransaction();
                try
                {
                    DataTable table = empImport.UploadToTempDatabase(strTmpFile, WebUtils.GetCurUser(Session).UserID, ZipPassword.Text, chkAutoCreateCode.Checked);
                    table = WebUtils.DataTableSortingAndPaging(table, info);

                    //Repeater.DataSource = table;
                    //Repeater.DataBind();
                    ImportSection.Visible = true;
                }
                catch (HRImportException ex)
                {
                    //if (empImport.errors.List.Count > 0)
                    //    foreach (string errorString in empImport.errors.List)
                    //        errors.addError(errorString);
                    //else
                        errors.addError(ex.Message);
                }
                catch (Exception ex)
                {
                    errors.addError(ex.ToString());
                }
                if (errors.isEmpty())
                {
                    //dbConn.CommitTransaction();
                }
                else
                {
                    //dbConn.RollbackTransaction();
                    empImport.ClearTempTable();
                }
                WebUtils.EndFunction(dbConn);
            }
            try
            {
                System.IO.File.Delete(strTmpFile);
            }
            catch
            {
            }
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
        EUploadEmpPersonalInfo obj = new EUploadEmpPersonalInfo();
        db.toObject(((DataRowView)e.Item.DataItem).Row, obj);

        if (e.Item.ItemIndex == Repeater.EditItemIndex)
        {
            //ebinding = new Binding(dbConn, db);
            //ebinding.add((TextBox)e.Item.FindControl("CNDEffDate"));
            //ebinding.add((HtmlInputHidden)e.Item.FindControl("CNDID"));
            //ebinding.add(new DropDownVLBinder(db, (DropDownList)e.Item.FindControl("PayCodeID"), EPaymentCode.VLPaymentCode));
            //ebinding.add(new DropDownVLBinder(db, (DropDownList)e.Item.FindControl("CurrencyID"), Values.VLCurrency));
            //ebinding.add((TextBox)e.Item.FindControl("CNDAmount"));
            //ebinding.add(new DropDownVLBinder(db, (DropDownList)e.Item.FindControl("CNDPayMethod"), Values.VLPaymentMethod));
            //ebinding.add((TextBox)e.Item.FindControl("CNDRemark"));

            //DBFilter filter = new DBFilter();
            //filter.add(new Match("EmpID", obj.EmpID));
            //ebinding.add(new DropDownVLBinder(db, (DropDownList)e.Item.FindControl("EmpAccID"), EEmpBankAccount.VLBankAccount, filter));



            //ebinding.init(Request, Session);

            //Hashtable values = new Hashtable();
            //db.populate(obj, values);

            //ebinding.toControl(values);
        }
        else
        {
            //((Button)e.Item.FindControl("Edit")).Visible = IsAllowEdit;
            //if (obj.PayRecID != null)
            //{
            //    ((Button)e.Item.FindControl("Edit")).Visible = false;
            //    ((CheckBox)e.Item.FindControl("DeleteItem")).Visible = false;
            //}
            HtmlInputHidden h = (HtmlInputHidden)e.Item.FindControl("UploadEmpID");
            h.Value = ((DataRowView)e.Item.DataItem)["UploadEmpID"].ToString();
        }
    }
    protected void Import_Click(object sender, EventArgs e)
    {
        ImportEmpPersonalInfoProcess empImport = new ImportEmpPersonalInfoProcess(dbConn, Session.SessionID);
        WebUtils.StartFunction(Session, FUNCTION_CODE);
        empImport.ImportToDatabase();
        WebUtils.EndFunction(dbConn);
        empImport.ClearTempTable();
        //loadData(info, db, Repeater);
        PageErrors.getErrors(db, Page).addError(HROne.Translation.PageMessage.IMPORT_SUCCESSFUL);
    }
}
