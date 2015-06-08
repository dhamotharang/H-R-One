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

public partial class Attendance_RosterTable_Import : HROneWebPage
{
    private DBManager db = ERosterTable.db;
    private const string FUNCTION_CODE = "ATT005";

    protected SearchBinding sbinding;
    protected Binding ebinding;
    protected ListInfo info;
    private DataView view;
    

    protected void Page_Load(object sender, EventArgs e)
    {
       
        if (!WebUtils.CheckAccess(Response, Session, FUNCTION_CODE, WebUtils.AccessLevel.ReadWrite))
            return;

        sbinding = new SearchBinding(dbConn, db);


        sbinding.init(DecryptedRequest, null);

        info = ListFooter.ListInfo;
        HROne.Common.WebUtility.WebControlsLocalization(this, this.Controls);


        //CNDImportFile.ControlStyle.CssClass = "button";
    }

    protected void Page_PreRender(object sender, EventArgs e)
    {
        //if (!Page.IsPostBack)
        {
            view = loadData(info, db, Repeater);
        }
    }

    public DataView loadData(ListInfo info, DBManager db, DataList repeater)
    {
        //filter.add(new Match("EmpPayrollID",EmpPayrollID.Value));

        ImportRosterTableProcess rosterTableImport = new ImportRosterTableProcess(dbConn, Session.SessionID);
        DataTable table = rosterTableImport.GetImportDataFromTempDatabase(info);
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
            string strTmpFolder = HROne.Common.Folder.GetOrCreateSessionTempFolder(Session.SessionID).FullName; ; //Environment.GetFolderPath(Environment.SpecialFolder.InternetCache);
            string strTmpFile = System.IO.Path.Combine(strTmpFolder, AppUtils.ServerDateTime().ToString("~yyyyMMddHHmmss_") + CNDImportFile.FileName);
            CNDImportFile.SaveAs(strTmpFile);

            ImportRosterTableProcess RosterTableImport = new ImportRosterTableProcess(dbConn, Session.SessionID);
            //DataTable dataTable = HROne.Import.ExcelImport.parse(strTmpFile);
            //using (OleDbConnection conn = new OleDbConnection(@"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=C:\csv\;Extended Properties='Text;'"))
            try
            {
                DataTable table = RosterTableImport.UploadToTempDatabase(strTmpFile, WebUtils.GetCurUser(Session).UserID, string.Empty);
                //Repeater.DataSource = table;
                //Repeater.DataBind();
                ImportSection.Visible = true;
            }
            catch (HRImportException ex)
            {
                if (RosterTableImport.errors.List.Count > 0)
                    foreach (string errorString in RosterTableImport.errors.List)
                        errors.addError(errorString);
                else
                    errors.addError(ex.Message);
                RosterTableImport.ClearTempTable();
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

        Repeater.EditItemIndex = -1;
        //view = loadData(info, db, Repeater);

    }

    protected void Repeater_ItemDataBound(object sender, DataListItemEventArgs e)
    {
    }
    protected void Import_Click(object sender, EventArgs e)
    {
        ImportRosterTableProcess RosterTableImport = new ImportRosterTableProcess(dbConn, Session.SessionID);
        RosterTableImport.ImportToDatabase();
        //loadData(info, db, Repeater);
        PageErrors.getErrors(db, Page).addError(HROne.Translation.PageMessage.IMPORT_SUCCESSFUL);

    }
}
