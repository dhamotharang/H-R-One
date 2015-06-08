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

public partial class Leave_BalanceAdjustment_Import : HROneWebPage
{
    private DBManager db = ELeaveBalanceAdjustment.db;
    private const string FUNCTION_CODE = "LEV005";
    protected SearchBinding sbinding;
    protected Binding ebinding;
    protected ListInfo info;
    private DataView view;
    

    protected void Page_Load(object sender, EventArgs e)
    {
       
        if (!WebUtils.CheckAccess(Response, Session, FUNCTION_CODE, WebUtils.AccessLevel.ReadWrite))
            return;

        sbinding = new SearchBinding(dbConn, EEmpPersonalInfo.db);


        sbinding.init(DecryptedRequest, null);
        sbinding.initValues("LeaveTypeID", new DBFilter(), ELeaveType.VLLeaveType, HROne.Common.WebUtility.GetSessionUICultureInfo(Session));
        sbinding.initValues("LeaveBalAdjType", new DBFilter(), ELeaveBalanceAdjustment.VLLeaveBalAdjType, HROne.Common.WebUtility.GetSessionUICultureInfo(Session));

        info = ListFooter.ListInfo;

        HROne.Common.WebUtility.WebControlsLocalization(this, this.Controls);

        if (!Page.IsPostBack)
        {
            view = loadData(info, db, Repeater);
        }

        //CNDImportFile.ControlStyle.CssClass = "button";
    }

    public DataView loadData(ListInfo info, DBManager db, Repeater repeater)
    {
        //filter.add(new Match("EmpPayrollID",EmpPayrollID.Value));

        ImportLeaveBalanceAdjustmentProcess leaveBalanceAdjustImport = new ImportLeaveBalanceAdjustmentProcess(dbConn, Session.SessionID);
        DataTable table = leaveBalanceAdjustImport.GetImportDataFromTempDatabase(info);

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

        if (ImportFile.HasFile)
        {
            //DataTable dataTable = HROne.CSVProcess.CSVReader.parse(CNDImportFile.PostedFile.InputStream);
            string strTmpFolder = HROne.Common.Folder.GetOrCreateSessionTempFolder(Session.SessionID).FullName; ; //Environment.GetFolderPath(Environment.SpecialFolder.InternetCache);
            string strTmpFile = System.IO.Path.Combine(strTmpFolder, AppUtils.ServerDateTime().ToString("~yyyyMMddHHmmss_") + ImportFile.FileName);
            ImportFile.SaveAs(strTmpFile);

            ImportLeaveBalanceAdjustmentProcess leaveBalanceAdjustImport = new ImportLeaveBalanceAdjustmentProcess(dbConn, Session.SessionID);
            //DataTable dataTable = HROne.Import.ExcelImport.parse(strTmpFile);
            //using (OleDbConnection conn = new OleDbConnection(@"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=C:\csv\;Extended Properties='Text;'"))
            try
            {
                Repeater.DataSource = leaveBalanceAdjustImport.UploadToTempDatabase(strTmpFile, WebUtils.GetCurUser(Session).UserID, string.Empty);
                Repeater.DataBind();
                ImportSection.Visible = true;
            }
            catch (HRImportException ex)
            {
                if (leaveBalanceAdjustImport.errors.List.Count > 0)
                    foreach (string errorString in leaveBalanceAdjustImport.errors.List)
                        errors.addError(errorString);
                else
                    errors.addError(ex.Message);
            }
            //System.IO.File.Delete(strTmpFile);
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

        //Repeater.EditItemIndex = -1;
        view = loadData(info, db, Repeater);

    }

    protected void Repeater_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
    }
    protected void Import_Click(object sender, EventArgs e)
    {
        ImportLeaveBalanceAdjustmentProcess leaveBalanceAdjustImport = new ImportLeaveBalanceAdjustmentProcess(dbConn, Session.SessionID);
        leaveBalanceAdjustImport.ImportToDatabase();
        loadData(info, db, Repeater);
        PageErrors.getErrors(db, Page).addError(HROne.Translation.PageMessage.IMPORT_SUCCESSFUL);

    }
    protected void btnExportTemplate_Click(object sender, EventArgs e)
    {
        string exportFileName = System.IO.Path.GetTempFileName();
        System.IO.File.Delete(exportFileName);
        exportFileName += ".xls";
        //System.IO.File.Copy(Server.MapPath("~/template/HistoryList_Template.xls"), exportFileName, true);
        HROne.Export.ExcelExport export = new HROne.Export.ExcelExport(exportFileName);

        export.Update(ImportLeaveBalanceAdjustmentProcess.Export(dbConn, new ArrayList(), false, false, false, new DateTime()));
        WebUtils.TransmitFile(Response, exportFileName, "LeaveBalanceAdjustmentTemplate_" + AppUtils.ServerDateTime().ToString("yyyyMMddHHmmss") + ".xls", true);
        return;

    }
}
