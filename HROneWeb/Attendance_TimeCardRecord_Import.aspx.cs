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

public partial class Attendance_TimeCardRecord_Import : HROneWebPage
{
    private const string FUNCTION_CODE = "ATT006";

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!WebUtils.CheckAccess(Response, Session, FUNCTION_CODE, WebUtils.AccessLevel.ReadWrite))
            return;


        HROne.Common.WebUtility.WebControlsLocalization(this, this.Controls);

    }



    protected void Upload_Click(object sender, EventArgs e)
    {
        PageErrors errors = PageErrors.getErrors(null, Page.Master);
        errors.clear();

        if (ImportFile.HasFile)
        {
            //DataTable dataTable = HROne.CSVProcess.CSVReader.parse(CNDImportFile.PostedFile.InputStream);
            string strTmpFolder = HROne.Common.Folder.GetOrCreateSessionTempFolder(Session.SessionID).FullName; ; //Environment.GetFolderPath(Environment.SpecialFolder.InternetCache);
            string strTmpFile = System.IO.Path.Combine(strTmpFolder, AppUtils.ServerDateTime().ToString("~yyyyMMddHHmmss_") + ImportFile.FileName);
            ImportFile.SaveAs(strTmpFile);

            ImportTimeCardRecordProcess timeCardRecordImport = new ImportTimeCardRecordProcess(dbConn, Session.SessionID);
            timeCardRecordImport.DateSequence = this.Attendance_ImportFormatParameterControl1.DateSequence;
            timeCardRecordImport.DateSeparator = this.Attendance_ImportFormatParameterControl1.DateSeparator;
            timeCardRecordImport.YearFormat = this.Attendance_ImportFormatParameterControl1.YearFormat;
            timeCardRecordImport.TimeSeparator = this.Attendance_ImportFormatParameterControl1.TimeSeparator;

            timeCardRecordImport.DateColumnIndex = this.Attendance_ImportFormatParameterControl1.DateColumnIndex;
            timeCardRecordImport.TimeColumnIndex = this.Attendance_ImportFormatParameterControl1.TimeColumnIndex;
            timeCardRecordImport.DateColumnIndex2 = this.Attendance_ImportFormatParameterControl1.DateColumnIndex2;
            timeCardRecordImport.TimeColumnIndex2 = this.Attendance_ImportFormatParameterControl1.TimeColumnIndex2;
            timeCardRecordImport.LocationColumnIndex = this.Attendance_ImportFormatParameterControl1.LocationColumnIndex;
            timeCardRecordImport.TimeCardNumColumnIndex = this.Attendance_ImportFormatParameterControl1.TimeCardNumColumnIndex;

            timeCardRecordImport.ColumnDelimiter = this.Attendance_ImportFormatParameterControl1.ColumnDelimiter;

            timeCardRecordImport.UploadFileHasHeader = this.Attendance_ImportFormatParameterControl1.UploadFileHasHeader;
            //DataTable dataTable = HROne.Import.ExcelImport.parse(strTmpFile);
            //using (OleDbConnection conn = new OleDbConnection(@"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=C:\csv\;Extended Properties='Text;'"))
            try
            {
                timeCardRecordImport.UploadToTempDatabase(strTmpFile, WebUtils.GetCurUser(Session).UserID, string.Empty);
                timeCardRecordImport.ImportToDatabase();
                errors.addError(HROne.Translation.PageMessage.IMPORT_SUCCESSFUL);
                Attendance_ImportFormatParameterControl1.SaveSettings();
            }
            catch (HRImportException ex)
            {
                if (timeCardRecordImport.errors.List.Count > 0)
                    foreach (string errorString in timeCardRecordImport.errors.List)
                        errors.addError(errorString);
                else
                    errors.addError(ex.Message);
            }
            System.IO.File.Delete(strTmpFile);
        }
        else
            errors.addError(HROne.Translation.PageErrorMessage.ERROR_INVALID_FILE);

    }
}
