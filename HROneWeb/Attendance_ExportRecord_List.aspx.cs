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
using HROne.Lib.Entities;
using HROne.DataAccess;
//using perspectivemind.validation;

public partial class Attendance_ExportRecord_List : HROneWebPage
{
    private const string FUNCTION_CODE = "ATT009";
    protected SearchBinding empSBinding, sbinding;
    public DBManager db = EEmpPersonalInfo.db;
    public EEmpPersonalInfo obj;

    protected ListInfo empInfo;
    protected DataView empView;
    

    protected void Page_Load(object sender, EventArgs e)
    {
       
        if (!WebUtils.CheckAccess(Response, Session, FUNCTION_CODE, WebUtils.AccessLevel.Read))
            return;


        empSBinding = new SearchBinding(dbConn, db);
        empSBinding.initValues("EmpStatus", null, EEmpPersonalInfo.VLEmpStatus, HROne.Common.WebUtility.GetSessionUICultureInfo(Session));
        empSBinding.init(DecryptedRequest, null);

        empInfo = ListFooter.ListInfo;

        HROne.Common.WebUtility.WebControlsLocalization(this, this.Controls);
    }

    protected void Page_PreRender(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            empView = emploadData(empInfo, db, empRepeater);
        }

    }


    public DataView emploadData(ListInfo info, DBManager db, Repeater repeater)
    {

        DBFilter filter = new DBFilter();// empSBinding.createFilter();

        //if (info != null && info.orderby != null && !info.orderby.Equals(""))
        //    filter.add(info.orderby, info.order);

        string select = "* ";
        string from = "from [" + EEmpPersonalInfo.db.dbclass.tableName + "] e ";

        filter.add(WebUtils.AddRankFilter(Session, "e.EmpID", true));

        DBFilter attendanceRecordFilter = new DBFilter();
        DateTime dtPeriodFr, dtPeriodTo;
        if (DateTime.TryParse(PeriodFr.Value, out dtPeriodFr) && DateTime.TryParse(PeriodTo.Value, out dtPeriodTo))
        {
            if (!chkCreateTempIfNotExists.Checked)
            {
                attendanceRecordFilter.add(new Match("ar.AttendanceRecordDate", "<=", dtPeriodTo));
                attendanceRecordFilter.add(new Match("ar.AttendanceRecordDate", ">=", dtPeriodFr));
                filter.add(new IN("EmpID", "Select distinct EmpID from " + EAttendanceRecord.db.dbclass.tableName + " ar", attendanceRecordFilter));
            }
            else
            {
                filter.add(new Match("e.EmpDateOfJoin", "<=", dtPeriodTo));
                DBFilter notEmpTermFilter = new DBFilter();
                notEmpTermFilter.add(new Match("empTerm.EmpTermLastDate", "<", dtPeriodFr));
                filter.add(new IN("NOT e.EmpID", "SELECT empTerm.EmpID FROM " + EEmpTermination.db.dbclass.tableName + " empTerm", notEmpTermFilter));
            }
        }
        else
        {
            btnGenerate.Visible = false;
            btnGenerate1.Visible = false;
            empView = null;
            repeater.DataSource = null;
            repeater.DataBind();

            return null;
        }


        DBFilter empInfoFilter = EmployeeSearchControl1.GetEmpInfoFilter(AppUtils.ServerDateTime(), AppUtils.ServerDateTime());
        empInfoFilter.add(new MatchField("e.EmpID", "ee.EmpID"));
        //if (DateTime.TryParse(PeriodFr.Value, out dtPeriodFr) && DateTime.TryParse(PeriodTo.Value, out dtPeriodTo))
        //{
        //    empInfoFilter.add(new Match("EmpDateOfJoin", "<=", dtPeriodTo));
        //    DBFilter empTermFilter = new DBFilter();
        //    empTermFilter.add(new MatchField("ee.EmpID", "et.EmpID"));
        //    empTermFilter.add(new Match("et.EmpTermLastDate", "<", dtPeriodFr));
        //    empInfoFilter.add(new Exists(EEmpTermination.db.dbclass.tableName + " et ", empTermFilter, true));

        //}

        filter.add(new Exists(EEmpPersonalInfo.db.dbclass.tableName + " ee", empInfoFilter));




        DataTable table = filter.loadData(dbConn, null, select, from);
        table = EmployeeSearchControl1.FilterEncryptedEmpInfoField(table, info);

        if (table.Rows.Count != 0)
        {
            btnGenerate.Visible = true;
            btnGenerate1.Visible = true;
        }
        else
        {
            btnGenerate.Visible = false;
            btnGenerate1.Visible = false;
        }
        empView = new DataView(table);
        //if (info != null)
        //{
        //    info.loadPageList(null, empPrevPage, empNextPage, empFirstPage, empLastPage);

        //    CurPage.Value = info.page.ToString();
        //    NumPage.Value = info.numPage.ToString();
        //}
        if (repeater != null)
        {
            repeater.DataSource = empView;
            repeater.DataBind();
        }

        return empView;
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

        empView = emploadData(empInfo, db, empRepeater);

    }
    protected void empRepeater_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        DataRowView row = (DataRowView)e.Item.DataItem;
        CheckBox cb = (CheckBox)e.Item.FindControl("ItemSelect");
        WebFormUtils.LoadKeys(EEmpPersonalInfo.db, row, cb);

    }

    protected void Search_Click(object sender, EventArgs e)
    {
        empInfo.page = 0;
        empView = emploadData(empInfo, db, empRepeater);

    }
    protected void Reset_Click(object sender, EventArgs e)
    {
        EmployeeSearchControl1.Reset();
        empSBinding.clear();
        empInfo.page = 0;

        empView = emploadData(empInfo, db, empRepeater);
    }

    protected void btnGenerate_Click(object sender, EventArgs e)
    {
        PageErrors errors = PageErrors.getErrors(db, Page.Master);
        errors.clear();


        DateTime dtPeriodFr = new DateTime();
        DateTime dtPeriodTo = new DateTime();

        if (!(DateTime.TryParse(PeriodFr.Value, out dtPeriodFr) && DateTime.TryParse(PeriodTo.Value, out dtPeriodTo)))
        {
            errors.addError("Invalid Date Format"); 
        }

        if (errors.isEmpty())
        {

            ArrayList list = new ArrayList();
            list = WebUtils.SelectedRepeaterItemToBaseObjectList(EEmpPersonalInfo.db, empRepeater, "ItemSelect");

            if (list.Count > 0)
            {
                GenerateAttendanceRecordData(list, dtPeriodFr, dtPeriodTo);
            }
        }
        emploadData(empInfo, db, empRepeater);
    }

    private void GenerateAttendanceRecordData(ArrayList EmpInfoList, DateTime PeriodFrom, DateTime PeriodTo)
    {
        DataSet dataSet = new DataSet();// export.GetDataSet();

        HROne.Import.ImportAttendanceRecordProcess attendanceProcess = new HROne.Import.ImportAttendanceRecordProcess(dbConn, Session.SessionID, false);
        dataSet.Tables.Add(attendanceProcess.Export(EmpInfoList, PeriodFrom, PeriodTo, chkCreateTempIfNotExists.Checked));

        string exportFileName = System.IO.Path.GetTempFileName();
        System.IO.File.Delete(exportFileName);
        exportFileName += ".xls";

        GenerateExcelFile(dataSet.Tables[0], exportFileName);

        WebUtils.TransmitFile(Response, exportFileName, "AttendanceRecord_" + AppUtils.ServerDateTime().ToString("yyyyMMddHHmmss") + ".xls", true);
        return;

    }

    private void GenerateExcelFile(DataTable dataTable, string exportFileName)
    {
        NPOI.HSSF.UserModel.HSSFWorkbook workbook = new NPOI.HSSF.UserModel.HSSFWorkbook();
        CreateWorkSheet(workbook, dataTable);
        System.IO.FileStream file = new System.IO.FileStream(exportFileName, System.IO.FileMode.Create);
        workbook.Write(file);
        file.Close();

    }

    private NPOI.HSSF.UserModel.HSSFSheet CreateWorkSheet(NPOI.HSSF.UserModel.HSSFWorkbook workbook, DataTable dataTable)
    {
        if (workbook != null)
        {
            NPOI.HSSF.UserModel.HSSFSheet worksheet = (NPOI.HSSF.UserModel.HSSFSheet)workbook.CreateSheet(dataTable.TableName.Replace("$", ""));
            //NPOI.HSSF.UserModel.HSSFRow chineseHeaderRow = (NPOI.HSSF.UserModel.HSSFRow)worksheet.CreateRow(0);
            NPOI.HSSF.UserModel.HSSFRow headerRow = (NPOI.HSSF.UserModel.HSSFRow)worksheet.CreateRow(0);
            int columnCount = 0;

            //System.Globalization.CultureInfo chineseCI = new System.Globalization.CultureInfo("zh-cht");
            foreach (DataColumn headercolumn in dataTable.Columns)
            {

                headercolumn.ColumnName = headercolumn.ColumnName.Trim();

                if (!headercolumn.ColumnName.EndsWith("TimeCardRecordID", StringComparison.CurrentCultureIgnoreCase))
                {
                    string columnName = headercolumn.ColumnName;
                    //string chineseColumnName = HROne.Common.WebUtility.GetLocalizedString(columnName, chineseCI);
                    //if (columnName.Equals(chineseColumnName))
                    //    chineseColumnName = string.Empty;

                    NPOI.HSSF.UserModel.HSSFCell cell = (NPOI.HSSF.UserModel.HSSFCell)headerRow.CreateCell(columnCount); //new ExcelLibrary.SpreadSheet.Cell(headercolumn.ColumnName, new ExcelLibrary.SpreadSheet.CellFormat(ExcelLibrary.SpreadSheet.CellFormatType.Text, string.Empty));
                    cell.SetCellValue(columnName);

                    //cell = (NPOI.HSSF.UserModel.HSSFCell)chineseHeaderRow.CreateCell(columnCount); //new ExcelLibrary.SpreadSheet.Cell(headercolumn.ColumnName, new ExcelLibrary.SpreadSheet.CellFormat(ExcelLibrary.SpreadSheet.CellFormatType.Text, string.Empty));
                    //cell.SetCellValue(chineseColumnName);

                    //headerRow.SetCell(columnCount,cell);
                    //worksheet.Cells[0, columnCount] = cell;//new ExcelLibrary.SpreadSheet.Cell(column.ColumnName, new ExcelLibrary.SpreadSheet.CellFormat(ExcelLibrary.SpreadSheet.CellFormatType.Text, string.Empty));
                    columnCount++;
                }
            }
            //worksheet.Cells.Rows.Add(0, headerRow);

            int rowCount = 0;


            NPOI.HSSF.UserModel.HSSFDataFormat format = (NPOI.HSSF.UserModel.HSSFDataFormat)workbook.CreateDataFormat();

            NPOI.HSSF.UserModel.HSSFFont boldFont = (NPOI.HSSF.UserModel.HSSFFont)workbook.CreateFont();
            boldFont.Boldweight = (short)NPOI.SS.UserModel.FontBoldWeight.BOLD; //900;

            NPOI.HSSF.UserModel.HSSFCellStyle dateCellStyle = (NPOI.HSSF.UserModel.HSSFCellStyle)workbook.CreateCellStyle();
            dateCellStyle.DataFormat = format.GetFormat("yyyy-MM-dd");

            NPOI.HSSF.UserModel.HSSFCellStyle ManualAdjustCellStyle = (NPOI.HSSF.UserModel.HSSFCellStyle)workbook.CreateCellStyle();
            //ManualInputDateCellStyle.CloneStyleFrom(dateCellStyle);
            ManualAdjustCellStyle.SetFont(boldFont);

            //NPOI.HSSF.UserModel.HSSFCellStyle numericCellStyle = (NPOI.HSSF.UserModel.HSSFCellStyle)workbook.CreateCellStyle();
            //numericCellStyle.DataFormat = NPOI.HSSF.UserModel.HSSFDataFormat.GetBuiltinFormat("0.00"); ;

            //NPOI.HSSF.UserModel.HSSFCellStyle integerCellStyle = (NPOI.HSSF.UserModel.HSSFCellStyle)workbook.CreateCellStyle();
            //integerCellStyle.DataFormat = NPOI.HSSF.UserModel.HSSFDataFormat.GetBuiltinFormat("0"); ;

            foreach (DataRow row in dataTable.Rows)
            {
                rowCount++;
                columnCount = 0;

                //                    ExcelLibrary.SpreadSheet.Row detailRow = new ExcelLibrary.SpreadSheet.Row();

                NPOI.HSSF.UserModel.HSSFRow detailRow = (NPOI.HSSF.UserModel.HSSFRow)worksheet.CreateRow(rowCount);

                foreach (DataColumn column in dataTable.Columns)
                {

                    if (!column.ColumnName.EndsWith("TimeCardRecordID", StringComparison.CurrentCultureIgnoreCase))
                    {

                        //ExcelLibrary.SpreadSheet.Cell cell =new ExcelLibrary.SpreadSheet.Cell(string.Empty, new ExcelLibrary.SpreadSheet.CellFormat(ExcelLibrary.SpreadSheet.CellFormatType.Text, string.Empty));
                        NPOI.HSSF.UserModel.HSSFCell cell = (NPOI.HSSF.UserModel.HSSFCell)detailRow.CreateCell(columnCount);

                        if (column.DataType.Equals(typeof(string)))
                        {
                            //cell.Format = new ExcelLibrary.SpreadSheet.CellFormat(ExcelLibrary.SpreadSheet.CellFormatType.Text, string.Empty);
                            //cell.Value = (row[column] == System.DBNull.Value ? string.Empty : row[column].ToString());
                            cell.SetCellValue(row[column] == System.DBNull.Value ? string.Empty : row[column].ToString());

                            //  Override style to bold if manual adjust
                            if (cell.StringCellValue != string.Empty)
                            {
                                if (column.ColumnName.Equals(HROne.Import.ImportAttendanceRecordProcess.FIELD_WORKIN))
                                {
                                    ETimeCardRecord timeCard = new ETimeCardRecord();
                                    try
                                    {
                                        timeCard.TimeCardRecordID = (int)row[HROne.Import.ImportAttendanceRecordProcess.FIELD_WORKIN_TIMECARDID];
                                    }
                                    catch { }
                                    if (ETimeCardRecord.db.select(dbConn, timeCard))
                                    {
                                        if (!timeCard.TimeCardRecordDateTime.ToString("HH:mm").Equals(cell.StringCellValue))
                                            cell.CellStyle = ManualAdjustCellStyle;
                                    }
                                    else
                                        cell.CellStyle = ManualAdjustCellStyle;
                                }
                                else if (column.ColumnName.Equals(HROne.Import.ImportAttendanceRecordProcess.FIELD_WORKOUT))
                                {
                                    ETimeCardRecord timeCard = new ETimeCardRecord();
                                    try
                                    {
                                        timeCard.TimeCardRecordID = (int)row[HROne.Import.ImportAttendanceRecordProcess.FIELD_WORKOUT_TIMECARDID];
                                    }
                                    catch { }
                                    if (ETimeCardRecord.db.select(dbConn, timeCard))
                                    {
                                        if (!timeCard.TimeCardRecordDateTime.ToString("HH:mm").Equals(cell.StringCellValue))
                                            cell.CellStyle = ManualAdjustCellStyle;
                                    }
                                    else
                                        cell.CellStyle = ManualAdjustCellStyle;
                                }
                                else if (column.ColumnName.Equals(HROne.Import.ImportAttendanceRecordProcess.FIELD_LUNCHOUT))
                                {
                                    ETimeCardRecord timeCard = new ETimeCardRecord();
                                    try
                                    {
                                        timeCard.TimeCardRecordID = (int)row[HROne.Import.ImportAttendanceRecordProcess.FIELD_LUNCHOUT_TIMECARDID];
                                    }
                                    catch { }
                                    if (ETimeCardRecord.db.select(dbConn, timeCard))
                                    {
                                        if (!timeCard.TimeCardRecordDateTime.ToString("HH:mm").Equals(cell.StringCellValue))
                                            cell.CellStyle = ManualAdjustCellStyle;
                                    }
                                    else
                                        cell.CellStyle = ManualAdjustCellStyle;
                                }
                                else if (column.ColumnName.Equals(HROne.Import.ImportAttendanceRecordProcess.FIELD_LUNCHIN))
                                {
                                    ETimeCardRecord timeCard = new ETimeCardRecord();
                                    try
                                    {
                                        timeCard.TimeCardRecordID = (int)row[HROne.Import.ImportAttendanceRecordProcess.FIELD_LUNCHIN_TIMECARDID];
                                    }
                                    catch { }
                                    if (ETimeCardRecord.db.select(dbConn, timeCard))
                                    {
                                        if (!timeCard.TimeCardRecordDateTime.ToString("HH:mm").Equals(cell.StringCellValue))
                                            cell.CellStyle = ManualAdjustCellStyle;
                                    }
                                    else
                                        cell.CellStyle = ManualAdjustCellStyle;
                                }
                                else if (column.ColumnName.Equals(HROne.Import.ImportAttendanceRecordProcess.FIELD_WORKIN_LOCATION))
                                {
                                    ETimeCardRecord timeCard = new ETimeCardRecord();
                                    try
                                    {
                                        timeCard.TimeCardRecordID = (int)row[HROne.Import.ImportAttendanceRecordProcess.FIELD_WORKIN_TIMECARDID];
                                    }
                                    catch { }
                                    if (ETimeCardRecord.db.select(dbConn, timeCard))
                                    {
                                        if (!timeCard.TimeCardRecordLocation.Equals(cell.StringCellValue))
                                            cell.CellStyle = ManualAdjustCellStyle;
                                    }
                                    else
                                        cell.CellStyle = ManualAdjustCellStyle;
                                }
                                else if (column.ColumnName.Equals(HROne.Import.ImportAttendanceRecordProcess.FIELD_WORKOUT_LOCATION))
                                {
                                    ETimeCardRecord timeCard = new ETimeCardRecord();
                                    try
                                    {
                                        timeCard.TimeCardRecordID = (int)row[HROne.Import.ImportAttendanceRecordProcess.FIELD_WORKOUT_TIMECARDID];
                                    }
                                    catch { }
                                    if (ETimeCardRecord.db.select(dbConn, timeCard))
                                    {
                                        if (!timeCard.TimeCardRecordLocation.Equals(cell.StringCellValue))
                                            cell.CellStyle = ManualAdjustCellStyle;
                                    }
                                    else
                                        cell.CellStyle = ManualAdjustCellStyle;
                                }
                                else if (column.ColumnName.Equals(HROne.Import.ImportAttendanceRecordProcess.FIELD_LUNCHOUT_LOCATION))
                                {
                                    ETimeCardRecord timeCard = new ETimeCardRecord();
                                    try
                                    {
                                        timeCard.TimeCardRecordID = (int)row[HROne.Import.ImportAttendanceRecordProcess.FIELD_LUNCHOUT_TIMECARDID];
                                    }
                                    catch { }
                                    if (ETimeCardRecord.db.select(dbConn, timeCard))
                                    {
                                        if (!timeCard.TimeCardRecordLocation.Equals(cell.StringCellValue))
                                            cell.CellStyle = ManualAdjustCellStyle;
                                    }
                                    else
                                        cell.CellStyle = ManualAdjustCellStyle;
                                }
                                else if (column.ColumnName.Equals(HROne.Import.ImportAttendanceRecordProcess.FIELD_LUNCHIN_LOCATION))
                                {
                                    ETimeCardRecord timeCard = new ETimeCardRecord();
                                    try
                                    {
                                        timeCard.TimeCardRecordID = (int)row[HROne.Import.ImportAttendanceRecordProcess.FIELD_LUNCHIN_TIMECARDID];
                                    }
                                    catch { }
                                    if (ETimeCardRecord.db.select(dbConn, timeCard))
                                    {
                                        if (!timeCard.TimeCardRecordLocation.Equals(cell.StringCellValue))
                                            cell.CellStyle = ManualAdjustCellStyle;
                                    }
                                    else
                                        cell.CellStyle = ManualAdjustCellStyle;
                                }
                            }
                        }
                        else if (column.DataType.Equals(typeof(double)) || column.DataType.Equals(typeof(float)))
                        {
                            //cell.Format = new ExcelLibrary.SpreadSheet.CellFormat(ExcelLibrary.SpreadSheet.CellFormatType.Number, "0.00");
                            //cell.Value = (row[column] == System.DBNull.Value ? string.Empty : row[column]);
                            if (row[column] != System.DBNull.Value)
                            {
                                double value = Convert.ToDouble(row[column].ToString());
                                if (value.Equals(double.NaN))
                                    cell.SetCellValue(string.Empty);
                                else
                                    cell.SetCellValue(value);
                            }
                            //cell.CellStyle = numericCellStyle;
                        }
                        else if (column.DataType.Equals(typeof(int)))
                        {
                            //cell.Format = new ExcelLibrary.SpreadSheet.CellFormat(ExcelLibrary.SpreadSheet.CellFormatType.Number, "0.00");
                            //cell.Value = (row[column] == System.DBNull.Value ? string.Empty : row[column]);
                            if (row[column] != System.DBNull.Value)
                                cell.SetCellValue(Convert.ToDouble(row[column].ToString()));
                            //cell.CellStyle = integerCellStyle;
                        }
                        else if (column.DataType.Equals(typeof(DateTime)))
                        {
                            //cell.Format = new ExcelLibrary.SpreadSheet.CellFormat(ExcelLibrary.SpreadSheet.CellFormatType.DateTime, "yyyy-MM-dd");
                            //if (row[column] == System.DBNull.Value)
                            //    cell.Value = string.Empty;
                            //else
                            //    cell.Value = (DateTime)row[column];
                            if (row[column] != System.DBNull.Value)
                                cell.SetCellValue((DateTime)row[column]);

                            cell.CellStyle = dateCellStyle;
                        }
                        else
                        {
                            //cell.Format = new ExcelLibrary.SpreadSheet.CellFormat(ExcelLibrary.SpreadSheet.CellFormatType.Text, string.Empty);
                            //cell.Value = (row[column] == System.DBNull.Value ? string.Empty : row[column].ToString());
                            if (row[column] != System.DBNull.Value)
                                cell.SetCellValue(row[column].ToString());
                        }
                        //worksheet.Cells[rowCount, columnCount] = cell;
                        columnCount++;
                    }
                }
                //                    worksheet.Cells.Rows.Add(rowCount, detailRow);
            }
            //workbook.Worksheets.Add(worksheet);

            return worksheet;
        }
        else
            return null;
    }

    // Start 0000004, Miranda, 2014-06-19
    protected void PayPeriod_Changed(object sender, EventArgs e)
    {
        DateTime m_periodFr;

        if (DateTime.TryParse(PeriodFr.Value, out m_periodFr))
        {
            if (PeriodTo.Value == "")
            {
                PeriodTo.Value = m_periodFr.AddMonths(1).AddDays(-1).ToString("yyyy-MM-dd");
            }
        }




        //DateTime dtPeriodFr = new DateTime(), dtPeriodTo = new DateTime();
        //DateTime.TryParse(PeriodFr.Value, out dtPeriodFr);
        //DateTime.TryParse(PeriodTo.Value, out dtPeriodTo);
        //if (!dtPeriodFr.Ticks.Equals(0) && !dtPeriodTo.Ticks.Equals(0))
        //{
        //    if (dtPeriodFr >= dtPeriodTo)
        //    {
        //        if (sender == PeriodFr.TextBox)
        //            PeriodTo.Value = dtPeriodFr.AddMonths(DefaultMonthPeriod).AddDays(-1).ToString("yyyy-MM-dd");
        //        else if (sender == PeriodTo.TextBox)
        //            PeriodFr.Value = dtPeriodTo.AddDays(1).AddMonths(-DefaultMonthPeriod).ToString("yyyy-MM-dd");
        //    }
        //}
        //else
        //{
        //    if (sender == PeriodFr.TextBox && !dtPeriodFr.Ticks.Equals(0) && dtPeriodTo.Ticks.Equals(0))
        //    {
        //        PeriodTo.Value = dtPeriodFr.AddMonths(DefaultMonthPeriod).AddDays(-1).ToString("yyyy-MM-dd");

        //    }
        //    else if (sender == PeriodTo.TextBox && !dtPeriodTo.Ticks.Equals(0) && dtPeriodFr.Ticks.Equals(0))
        //        PeriodFr.Value = dtPeriodTo.AddDays(1).AddMonths(-DefaultMonthPeriod).ToString("yyyy-MM-dd");
        //}
    }

    public int DefaultMonthPeriod
    {
        set { hiddenFieldDefaultMonthPeriod.Value = value.ToString(); }
        get { return int.Parse(hiddenFieldDefaultMonthPeriod.Value); }
    }
    // End 0000004, Miranda, 2014-06-19

}
