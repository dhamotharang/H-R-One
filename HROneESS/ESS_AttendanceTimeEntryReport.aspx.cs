using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

using HROne.DataAccess;
using HROne.Lib.Entities;

public partial class ESS_AttendanceTimeEntryReport : HROneWebPage
{
    public Binding binding;
    public DBManager db = EEmpPersonalInfo.db;
    public int CurID = -1;

    protected int selectedRosterTableGroupID
    {
        get
        {
            int parsedValue;
            if (int.TryParse(hiddenRosterTableGroupID.Value, out parsedValue) == true)
            {
                return parsedValue;
            }
            else
            {
                return -1;
            }
        }

        set
        {
            hiddenRosterTableGroupID.Value = value.ToString();
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!WebUtils.CheckAccess(Response, Session))
            return;
        binding = new Binding(dbConn, db);

        EESSUser user = WebUtils.GetCurUser(Session);
        if (user != null)
            CurID = user.EmpID;

        HROne.Common.WebUtility.WebControlsLocalization(this, this.Controls);

        if (!Page.IsPostBack)
        {
            if (CurID > 0)
            {
                loadObject();
            }
        }
    }

    protected bool loadObject()
    {
        EEmpPersonalInfo obj = new EEmpPersonalInfo();
        obj.EmpID = CurID;
        if (!db.select(dbConn, obj))
            return false;

        Hashtable values = new Hashtable();
        db.populate(obj, values);
        binding.toControl(values);
        return true;
    }

    protected int BuildCommonRosterCodeAndTermsFilter()
    {
        if (selectedRosterTableGroupID > 0)
        {
            DBFilter groupFilter = new DBFilter();
            groupFilter.add(new Match("RosterTableGroupID", selectedRosterTableGroupID));

            ERosterTableGroup group = (ERosterTableGroup)GetFirstItem(ERosterTableGroup.db.select(dbConn, groupFilter));

            if (group != null)
            {
                hiddenRosterTableGroupID.Value = group.RosterTableGroupID.ToString();
                hiddenRosterClientID.Value = group.RosterClientID.ToString();
                hiddenRosterClientSiteID.Value = group.RosterClientSiteID.ToString();

                return group.RosterTableGroupID;
            }
        }
        hiddenRosterTableGroupID.Value = "0";
        hiddenRosterClientID.Value = "0";
        hiddenRosterClientSiteID.Value = "0";

        return -1;
    }

    private Dictionary<int, List<DateTime>> LoadEmployeeList(DateTime periodFromDate, DateTime periodToDate)
    {

        Dictionary<int, List<DateTime>> staffRosterDateList = new Dictionary<int, List<DateTime>>();

        if (selectedRosterTableGroupID > 0)
        {
            EEmpRosterTableGroup empRosterInfo = GetSingleEmployeeRosterInfo(dbConn, AppUtils.ServerDateTime().Date, CurID, selectedRosterTableGroupID);

            if (empRosterInfo.EmpRosterTableGroupIsSupervisor)
            {
                ArrayList staffEmpRosterInfoList = GetStaffEmpRosterInfoList(dbConn, periodFromDate, periodToDate, empRosterInfo.RosterTableGroupID);

                foreach (EEmpRosterTableGroup staffEmpRosterInfo in staffEmpRosterInfoList)
                {
                    DateTime staffRangeFr = periodFromDate;
                    DateTime staffRangeTo = periodToDate;

                    if (!staffEmpRosterInfo.EmpRosterTableGroupEffFr.Ticks.Equals(0) && staffEmpRosterInfo.EmpRosterTableGroupEffFr > staffRangeFr)
                        staffRangeFr = staffEmpRosterInfo.EmpRosterTableGroupEffFr;

                    if (!staffEmpRosterInfo.EmpRosterTableGroupEffTo.Ticks.Equals(0) && staffEmpRosterInfo.EmpRosterTableGroupEffTo < staffRangeTo)
                        staffRangeTo = staffEmpRosterInfo.EmpRosterTableGroupEffTo;

                    List<DateTime> availableDateList;

                    if (staffRosterDateList.ContainsKey(staffEmpRosterInfo.EmpID))
                        availableDateList = staffRosterDateList[staffEmpRosterInfo.EmpID];
                    else
                    {
                        availableDateList = new List<DateTime>();
                        staffRosterDateList.Add(staffEmpRosterInfo.EmpID, availableDateList);
                    }

                    for (DateTime staffAvailableDate = staffRangeFr; staffAvailableDate <= staffRangeTo; staffAvailableDate = staffAvailableDate.AddDays(1))
                        if (!availableDateList.Contains(staffAvailableDate))
                            availableDateList.Add(staffAvailableDate);
                }

            }
        }

        return staffRosterDateList;

    }

    protected void btnGenerate_Click(object sender, EventArgs e)
    {
        PageErrors errors = PageErrors.getErrors(db, Page.Master);
        errors.clear();
        System.IO.FileInfo file = GenerateExcelFile();
        if (file != null)
        {
            TransmitFile(Response, file.FullName, "AttendanceTimeEntryReport_" + AppUtils.ServerDateTime().ToString("yyyyMMddHHmmss") + ".xls", true);
        }
    }
    protected System.IO.FileInfo GenerateExcelFile()
    {
        DateTime periodFromDate;
        DateTime periodToDate;
        if (DateTime.TryParse(PeriodFrDate.Value, out periodFromDate) && DateTime.TryParse(PeriodToDate.Value, out periodToDate))
        {
            int countDay = periodToDate.AddDays(1).Subtract(periodFromDate).Days;// get days from periodFromDate to periodToDate

            List<EHierarchyLevel> hLevelList = GetHierarchyLevelList(dbConn);

            NPOI.HSSF.UserModel.HSSFWorkbook workbook = new NPOI.HSSF.UserModel.HSSFWorkbook();
            NPOI.HSSF.UserModel.HSSFSheet workSheet = (NPOI.HSSF.UserModel.HSSFSheet)workbook.CreateSheet();

            SetWorkSheetColumnWidth(workSheet, countDay);

            createWorkSheetPeriodRow(workbook, workSheet, periodFromDate, periodToDate);

            createWorkSheetHeaderRows(workbook, workSheet, hLevelList, countDay, periodFromDate, periodToDate);

            // load data detail start
            ArrayList list = LoadRosterTableGroup(periodFromDate, periodToDate, CurID);
            if (list.Count >= 1)
            {
                selectedRosterTableGroupID = ((ERosterTableGroup)list[0]).RosterTableGroupID;
                BuildCommonRosterCodeAndTermsFilter();
                Dictionary<int, List<DateTime>> staffRosterDateList = LoadEmployeeList(periodFromDate, periodToDate);

                int RowPos = 0;
                foreach (int empID in staffRosterDateList.Keys)
                {
                    EEmpPersonalInfo empInfo = new EEmpPersonalInfo();
                    empInfo.EmpID = empID;
                    if (!EEmpPersonalInfo.db.select(dbConn, empInfo))
                        continue;

                    string positionTitle = string.Empty;
                    List<string> hElementDescList = new List<string>();
                    EEmpPositionInfo empPos = AppUtils.GetLastPositionInfo(dbConn, periodFromDate, empID);
                    if (empPos != null)
                    {
                        EPosition position = new EPosition();
                        position.PositionID = empPos.PositionID;
                        if (EPosition.db.select(dbConn, position))
                            positionTitle = position.PositionDesc;

                        foreach (EHierarchyLevel hLevel in hLevelList)
                        {
                            string hElementDesc = string.Empty;

                            DBFilter empHierarchy1Filter = new DBFilter();
                            empHierarchy1Filter.add(new Match("EmpPosID", empPos.EmpPosID));
                            empHierarchy1Filter.add(new Match("HLevelID", hLevel.HLevelID));
                            ArrayList empHierarchyList = EEmpHierarchy.db.select(dbConn, empHierarchy1Filter);
                            if (empHierarchyList.Count > 0)
                            {
                                EHierarchyElement hElement = new EHierarchyElement();
                                hElement.HElementID = ((EEmpHierarchy)empHierarchyList[0]).HElementID;
                                if (EHierarchyElement.db.select(dbConn, hElement))
                                    hElementDesc = hElement.HElementDesc;
                            }
                            hElementDescList.Add(hElementDesc);
                        }
                    }

                    createWorkSheetDetailRows(workbook, workSheet, RowPos, hElementDescList, empInfo, positionTitle, countDay, periodFromDate, empID);

                    RowPos += 2;
                }
            }
            // load data detail end

            string strTmpFolder = HROne.Common.Folder.GetOrCreateSessionTempFolder(Session.SessionID).FullName;
            string strTmpFile = System.IO.Path.Combine(strTmpFolder, AppUtils.ServerDateTime().ToString("~yyyyMMddHHmmss_AttendanceTimeCardRecordReport.xls"));
            System.IO.Stream fileoutputstream = new System.IO.FileStream(strTmpFile, System.IO.FileMode.OpenOrCreate);
            workbook.Write(fileoutputstream);
            fileoutputstream.Close();
            return new System.IO.FileInfo(strTmpFile);
        }
        return null;
    }

    public static void TransmitFile(HttpResponse Response, string FilenameWithFullPath, string clientSideFileName, bool DeleteAfterTransmit)
    {
        System.IO.FileInfo transmiteFileInfo = new System.IO.FileInfo(FilenameWithFullPath);
        if (transmiteFileInfo.Exists)
        {
            if (Response.IsClientConnected)
            {
                Response.ClearContent();
                Response.ClearHeaders();
                Response.AddHeader("Content-Disposition", "attachment;filename=" + clientSideFileName);
                Response.ContentType = "application/download";
                Response.AppendHeader("Content-Length", transmiteFileInfo.Length.ToString());
                Response.Expires = -1;
                if (DeleteAfterTransmit)
                {
                    Response.WriteFile(FilenameWithFullPath, true);
                    Response.Flush();
                    System.IO.File.Delete(FilenameWithFullPath);
                }
                else
                {
                    Response.TransmitFile(FilenameWithFullPath);
                    Response.Flush();
                }
                Response.End();
            }
            else
                transmiteFileInfo.Delete();
        }
        else
            throw new System.IO.FileNotFoundException("Internal File Not Found: " + FilenameWithFullPath, FilenameWithFullPath);
    }

    private EEmpRosterTableGroup GetSingleEmployeeRosterInfo(DatabaseConnection dbConn, DateTime date, int empID, int rosterTableGroupID)
    {
        DBFilter filter = new DBFilter();
        filter.add(new Match("EmpID", empID));
        filter.add(new Match("RosterTableGroupID", rosterTableGroupID));
        filter.add(new Match("EmpRosterTableGroupEffFr", "<=", date));
        filter.add("EmpRosterTableGroupEffTo", false);
        ArrayList list = EEmpRosterTableGroup.db.select(dbConn, filter);
        if (list.Count > 0)
            return (EEmpRosterTableGroup)list[0];
        else
            return null;
    }

    private ArrayList GetStaffEmpRosterInfoList(DatabaseConnection dbConn, DateTime periodFromDate, DateTime periodToDate, int rosterTableGroupID)
    {
        DBFilter staffEmpRosterFilter = new DBFilter();
        staffEmpRosterFilter.add(new Match("RosterTableGroupID", rosterTableGroupID));
        {
            OR orFromTerm = new OR();
            orFromTerm.add(new Match("EmpRosterTableGroupEffFr", "<=", periodToDate));
            orFromTerm.add(new NullTerm("EmpRosterTableGroupEffFr"));
            staffEmpRosterFilter.add(orFromTerm);
        }
        {
            OR orToTerm = new OR();
            orToTerm.add(new Match("EmpRosterTableGroupEffTo", ">=", periodFromDate));
            orToTerm.add(new NullTerm("EmpRosterTableGroupEffTo"));
            staffEmpRosterFilter.add(orToTerm);
        }

        return EEmpRosterTableGroup.db.select(dbConn, staffEmpRosterFilter);
    }

    private List<EHierarchyLevel> GetHierarchyLevelList(DatabaseConnection dbConn)
    {
        DBFilter hierarchyLevelFilter = new DBFilter();
        hierarchyLevelFilter.add("HLevelSeqNo", true);
        ArrayList tmpHLevelList = EHierarchyLevel.db.select(dbConn, hierarchyLevelFilter);

        List<EHierarchyLevel> hLevelList = new List<EHierarchyLevel>();
        foreach (EHierarchyLevel hLevel in tmpHLevelList)
        {
            if (hLevelList.Count < 2)
                hLevelList.Add(hLevel);
            else
                break;
        }
        return hLevelList;
    }

    private void SetWorkSheetColumnWidth(NPOI.HSSF.UserModel.HSSFSheet workSheet, int countDay)
    {
        workSheet.SetColumnWidth(0, 4000);
        workSheet.SetColumnWidth(1, 4000);
        workSheet.SetColumnWidth(2, 9000);
        workSheet.SetColumnWidth(3, 4000);
        workSheet.SetColumnWidth(4, 2000);
        for (int dayIdx = 0; dayIdx < countDay; dayIdx++)
        {
            workSheet.SetColumnWidth(3 + dayIdx, 3000);
        }
        workSheet.SetColumnWidth(3 + countDay, 3000);
    }

    private void createWorkSheetPeriodRow(NPOI.HSSF.UserModel.HSSFWorkbook workbook, NPOI.HSSF.UserModel.HSSFSheet workSheet, 
        DateTime periodFromDate, DateTime periodToDate)
    {
        // create header styles
        NPOI.HSSF.UserModel.HSSFCellStyle HeaderStyleLeft = (NPOI.HSSF.UserModel.HSSFCellStyle)workbook.CreateCellStyle();
        NPOI.HSSF.UserModel.HSSFFont HeaderFont = (NPOI.HSSF.UserModel.HSSFFont)workbook.CreateFont();
        HeaderFont.Boldweight = 900;
        HeaderFont.FontHeightInPoints = 16;
        HeaderStyleLeft.SetFont(HeaderFont);

        NPOI.HSSF.UserModel.HSSFCellStyle HeaderStyleCenter = (NPOI.HSSF.UserModel.HSSFCellStyle)workbook.CreateCellStyle();
        HeaderStyleCenter.CloneStyleFrom(HeaderStyleLeft);
        HeaderStyleCenter.Alignment = NPOI.SS.UserModel.HorizontalAlignment.CENTER;

        NPOI.HSSF.UserModel.HSSFCellStyle HeaderStyleRight = (NPOI.HSSF.UserModel.HSSFCellStyle)workbook.CreateCellStyle();
        HeaderStyleRight.CloneStyleFrom(HeaderStyleLeft);
        HeaderStyleRight.Alignment = NPOI.SS.UserModel.HorizontalAlignment.RIGHT;

        // Create header row
        NPOI.HSSF.UserModel.HSSFRow HeaderRow = (NPOI.HSSF.UserModel.HSSFRow)workSheet.CreateRow(0);
        NPOI.HSSF.UserModel.HSSFCell HeaderCell = (NPOI.HSSF.UserModel.HSSFCell)HeaderRow.CreateCell(5);
        HeaderCell.SetCellValue(periodFromDate.ToString("dd-MMM-yy"));
        HeaderCell.CellStyle = HeaderStyleRight;

        HeaderCell = (NPOI.HSSF.UserModel.HSSFCell)HeaderRow.CreateCell(6);
        HeaderCell.SetCellValue("~");
        HeaderCell.CellStyle = HeaderStyleCenter;

        HeaderCell = (NPOI.HSSF.UserModel.HSSFCell)HeaderRow.CreateCell(7);
        HeaderCell.SetCellValue(periodToDate.ToString("dd-MMM-yy"));
        HeaderCell.CellStyle = HeaderStyleLeft;
    }

    private void createWorkSheetHeaderRows(NPOI.HSSF.UserModel.HSSFWorkbook workbook, NPOI.HSSF.UserModel.HSSFSheet workSheet, 
        List<EHierarchyLevel> hLevelList, int countDay, DateTime periodFromDate, DateTime periodToDate)
    {
        // Create column header styles
        NPOI.HSSF.UserModel.HSSFCellStyle columnHeaderStyle = (NPOI.HSSF.UserModel.HSSFCellStyle)workbook.CreateCellStyle();
        columnHeaderStyle.Alignment = NPOI.SS.UserModel.HorizontalAlignment.CENTER;
        columnHeaderStyle.VerticalAlignment = NPOI.SS.UserModel.VerticalAlignment.CENTER;
        NPOI.HSSF.UserModel.HSSFFont columnHeaderFont = (NPOI.HSSF.UserModel.HSSFFont)workbook.CreateFont();
        columnHeaderFont.Boldweight = 900;
        columnHeaderStyle.SetFont(columnHeaderFont);
        columnHeaderStyle.BorderTop = NPOI.SS.UserModel.BorderStyle.THIN;
        columnHeaderStyle.BorderLeft = NPOI.SS.UserModel.BorderStyle.THIN;
        columnHeaderStyle.BorderRight = NPOI.SS.UserModel.BorderStyle.THIN;
        columnHeaderStyle.BorderBottom = NPOI.SS.UserModel.BorderStyle.THIN;

        NPOI.HSSF.UserModel.HSSFCellStyle columnHeaderStyleFirstTop = (NPOI.HSSF.UserModel.HSSFCellStyle)workbook.CreateCellStyle();
        columnHeaderStyleFirstTop.CloneStyleFrom(columnHeaderStyle);
        columnHeaderStyleFirstTop.BorderTop = NPOI.SS.UserModel.BorderStyle.MEDIUM;
        columnHeaderStyleFirstTop.BorderLeft = NPOI.SS.UserModel.BorderStyle.MEDIUM;

        NPOI.HSSF.UserModel.HSSFCellStyle columnHeaderStyleMiddleTop = (NPOI.HSSF.UserModel.HSSFCellStyle)workbook.CreateCellStyle();
        columnHeaderStyleMiddleTop.CloneStyleFrom(columnHeaderStyle);
        columnHeaderStyleMiddleTop.BorderTop = NPOI.SS.UserModel.BorderStyle.MEDIUM;

        NPOI.HSSF.UserModel.HSSFCellStyle columnHeaderStyleLastTop = (NPOI.HSSF.UserModel.HSSFCellStyle)workbook.CreateCellStyle();
        columnHeaderStyleLastTop.CloneStyleFrom(columnHeaderStyle);
        columnHeaderStyleLastTop.BorderTop = NPOI.SS.UserModel.BorderStyle.MEDIUM;
        columnHeaderStyleLastTop.BorderRight = NPOI.SS.UserModel.BorderStyle.MEDIUM;

        NPOI.HSSF.UserModel.HSSFCellStyle columnHeaderStyleFirstBottom = (NPOI.HSSF.UserModel.HSSFCellStyle)workbook.CreateCellStyle();
        columnHeaderStyleFirstBottom.CloneStyleFrom(columnHeaderStyle);
        columnHeaderStyleFirstBottom.BorderLeft = NPOI.SS.UserModel.BorderStyle.MEDIUM;
        columnHeaderStyleFirstBottom.BorderBottom = NPOI.SS.UserModel.BorderStyle.MEDIUM;

        NPOI.HSSF.UserModel.HSSFCellStyle columnHeaderStyleMiddleBottom = (NPOI.HSSF.UserModel.HSSFCellStyle)workbook.CreateCellStyle();
        columnHeaderStyleMiddleBottom.CloneStyleFrom(columnHeaderStyle);
        columnHeaderStyleMiddleBottom.BorderBottom = NPOI.SS.UserModel.BorderStyle.MEDIUM;

        NPOI.HSSF.UserModel.HSSFCellStyle columnHeaderStyleLastBottom = (NPOI.HSSF.UserModel.HSSFCellStyle)workbook.CreateCellStyle();
        columnHeaderStyleLastBottom.CloneStyleFrom(columnHeaderStyle);
        columnHeaderStyleLastBottom.BorderRight = NPOI.SS.UserModel.BorderStyle.MEDIUM;
        columnHeaderStyleLastBottom.BorderBottom = NPOI.SS.UserModel.BorderStyle.MEDIUM;

        // Create column header rows
        const int COLUMN_HEADER_ROW = 2;
        NPOI.HSSF.UserModel.HSSFRow columnHeaderRow1 = (NPOI.HSSF.UserModel.HSSFRow)workSheet.CreateRow(COLUMN_HEADER_ROW);
        NPOI.HSSF.UserModel.HSSFRow columnHeaderRow2 = (NPOI.HSSF.UserModel.HSSFRow)workSheet.CreateRow(COLUMN_HEADER_ROW + 1);
        NPOI.HSSF.UserModel.HSSFCell columnHeaderCell;

        int colpos = 0;
        foreach (EHierarchyLevel hLevel in hLevelList)
        {
            columnHeaderCell = (NPOI.HSSF.UserModel.HSSFCell)columnHeaderRow1.CreateCell(colpos);
            columnHeaderCell.SetCellValue(hLevel.HLevelDesc);
            columnHeaderCell.CellStyle = columnHeaderStyleFirstTop;
            columnHeaderCell = (NPOI.HSSF.UserModel.HSSFCell)columnHeaderRow2.CreateCell(colpos);
            columnHeaderCell.SetCellValue(string.Empty);
            columnHeaderCell.CellStyle = columnHeaderStyleFirstBottom;

            colpos++;
        }
        columnHeaderCell = (NPOI.HSSF.UserModel.HSSFCell)columnHeaderRow1.CreateCell(colpos);
        columnHeaderCell.SetCellValue("Name");
        columnHeaderCell.CellStyle = columnHeaderStyleMiddleTop;
        columnHeaderCell = (NPOI.HSSF.UserModel.HSSFCell)columnHeaderRow2.CreateCell(colpos);
        columnHeaderCell.SetCellValue(string.Empty);
        columnHeaderCell.CellStyle = columnHeaderStyleMiddleBottom;

        colpos++;

        columnHeaderCell = (NPOI.HSSF.UserModel.HSSFCell)columnHeaderRow1.CreateCell(colpos);
        columnHeaderCell.SetCellValue("Title");
        columnHeaderCell.CellStyle = columnHeaderStyleMiddleTop;
        columnHeaderCell = (NPOI.HSSF.UserModel.HSSFCell)columnHeaderRow2.CreateCell(colpos);
        columnHeaderCell.SetCellValue(string.Empty);
        columnHeaderCell.CellStyle = columnHeaderStyleMiddleBottom;

        colpos++;

        columnHeaderCell = (NPOI.HSSF.UserModel.HSSFCell)columnHeaderRow1.CreateCell(colpos);
        columnHeaderCell.SetCellValue("Staff No.");
        columnHeaderCell.CellStyle = columnHeaderStyleMiddleTop;
        columnHeaderCell = (NPOI.HSSF.UserModel.HSSFCell)columnHeaderRow2.CreateCell(colpos);
        columnHeaderCell.SetCellValue(string.Empty);
        columnHeaderCell.CellStyle = columnHeaderStyleMiddleBottom;

        colpos++;

        for (int colIdx = 0; colIdx < colpos; colIdx++)
        {
            workSheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(COLUMN_HEADER_ROW, COLUMN_HEADER_ROW + 1, colIdx, colIdx));
        }

        for (int dayIdx = 0; dayIdx < countDay; dayIdx++)
        {
            DateTime currentDate = periodFromDate.AddDays(dayIdx);

            columnHeaderCell = (NPOI.HSSF.UserModel.HSSFCell)columnHeaderRow1.CreateCell(colpos + dayIdx);
            columnHeaderCell.SetCellValue(currentDate.ToString("dd-MMM-yy"));
            columnHeaderCell.CellStyle = columnHeaderStyleMiddleTop;

            columnHeaderCell = (NPOI.HSSF.UserModel.HSSFCell)columnHeaderRow2.CreateCell(colpos + dayIdx);
            columnHeaderCell.SetCellValue(currentDate.ToString("ddd"));
            columnHeaderCell.CellStyle = columnHeaderStyleMiddleBottom;
        }
        colpos += countDay;

        columnHeaderCell = (NPOI.HSSF.UserModel.HSSFCell)columnHeaderRow1.CreateCell(colpos);
        columnHeaderCell.SetCellValue("Signature");
        columnHeaderCell.CellStyle = columnHeaderStyleLastTop;
        columnHeaderCell = (NPOI.HSSF.UserModel.HSSFCell)columnHeaderRow2.CreateCell(colpos);
        columnHeaderCell.SetCellValue(string.Empty);
        columnHeaderCell.CellStyle = columnHeaderStyleLastBottom;

        workSheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(COLUMN_HEADER_ROW, COLUMN_HEADER_ROW + 1, colpos, colpos));
    }

    protected ArrayList LoadRosterTableGroup(DateTime periodFromDate, DateTime periodToDate, int userID)
    {
        DBFilter subQueryFilter = new DBFilter();
        subQueryFilter.add(new Match("EmpID", userID));
        subQueryFilter.add(new Match("EmpRosterTableGroupEffFr", "<=", periodToDate));

        OR orToTerm = new OR();
        orToTerm.add(new Match("EmpRosterTableGroupEffTo", ">=", periodFromDate));
        orToTerm.add(new NullTerm("EmpRosterTableGroupEffTo"));

        subQueryFilter.add(orToTerm);

        IN inFilter = new IN("RosterTableGroupID", "SELECT RosterTableGroupID FROM EmpRosterTableGroup", subQueryFilter);

        DBFilter filter = new DBFilter();

        filter.add(inFilter);
        filter.add("RosterTableGroupCode", true);

        ArrayList rosterTableGroupInfoList = ERosterTableGroup.db.select(dbConn, filter);

        return rosterTableGroupInfoList;
    }

    private void createWorkSheetDetailRows(NPOI.HSSF.UserModel.HSSFWorkbook workbook, NPOI.HSSF.UserModel.HSSFSheet workSheet, int RowPos,
        List<string> hElementDescList, EEmpPersonalInfo empInfo, string positionTitle, int countDay, DateTime periodFromDate, int empID)
    {
        // Create column detail styles
        NPOI.HSSF.UserModel.HSSFCellStyle detailStyle = (NPOI.HSSF.UserModel.HSSFCellStyle)workbook.CreateCellStyle();
        detailStyle.BorderLeft = NPOI.SS.UserModel.BorderStyle.THIN;
        detailStyle.BorderRight = NPOI.SS.UserModel.BorderStyle.THIN;
        detailStyle.BorderBottom = NPOI.SS.UserModel.BorderStyle.THIN;

        NPOI.HSSF.UserModel.HSSFCellStyle detailStyleCenterAlignment = (NPOI.HSSF.UserModel.HSSFCellStyle)workbook.CreateCellStyle();
        detailStyleCenterAlignment.CloneStyleFrom(detailStyle);
        detailStyleCenterAlignment.Alignment = NPOI.SS.UserModel.HorizontalAlignment.CENTER;

        NPOI.HSSF.UserModel.HSSFCellStyle detailStyleFirst = (NPOI.HSSF.UserModel.HSSFCellStyle)workbook.CreateCellStyle();
        detailStyleFirst.CloneStyleFrom(detailStyle);
        detailStyleFirst.BorderLeft = NPOI.SS.UserModel.BorderStyle.MEDIUM;

        NPOI.HSSF.UserModel.HSSFCellStyle detailStyleLast = (NPOI.HSSF.UserModel.HSSFCellStyle)workbook.CreateCellStyle();
        detailStyleLast.CloneStyleFrom(detailStyle);
        detailStyleLast.BorderRight = NPOI.SS.UserModel.BorderStyle.MEDIUM;

        // Create column detail rows
        const int DETAIL_START_ROW = 4;
        NPOI.HSSF.UserModel.HSSFRow detailRow = (NPOI.HSSF.UserModel.HSSFRow)workSheet.CreateRow(DETAIL_START_ROW + RowPos);
        NPOI.HSSF.UserModel.HSSFRow detailRow2 = (NPOI.HSSF.UserModel.HSSFRow)workSheet.CreateRow(DETAIL_START_ROW + RowPos + 1);   // second row of record

        NPOI.HSSF.UserModel.HSSFCell detailCell;
        NPOI.HSSF.UserModel.HSSFCell detailCell2;
        int colpos = 0;

        foreach (string hElementDesc in hElementDescList)
        {
            workSheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(DETAIL_START_ROW + RowPos, DETAIL_START_ROW + RowPos + 1, colpos, colpos));
            detailCell = (NPOI.HSSF.UserModel.HSSFCell)detailRow.CreateCell(colpos);
            detailCell.SetCellValue(hElementDesc);
            detailCell.CellStyle = detailStyleFirst;

            detailCell2 = (NPOI.HSSF.UserModel.HSSFCell)detailRow2.CreateCell(colpos);
            detailCell2.SetCellValue(hElementDesc);
            detailCell2.CellStyle = detailStyleFirst;

            colpos++;
        }

        workSheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(DETAIL_START_ROW + RowPos, DETAIL_START_ROW + RowPos + 1, colpos, colpos));
        detailCell = (NPOI.HSSF.UserModel.HSSFCell)detailRow.CreateCell(colpos);
        detailCell.SetCellValue(empInfo.EmpEngFullName);
        detailCell.CellStyle = detailStyleFirst;

        detailCell2 = (NPOI.HSSF.UserModel.HSSFCell)detailRow2.CreateCell(colpos);
        detailCell2.SetCellValue(empInfo.EmpEngFullName);
        detailCell2.CellStyle = detailStyleFirst;

        colpos++;

        workSheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(DETAIL_START_ROW + RowPos, DETAIL_START_ROW + RowPos + 1, colpos, colpos));
        detailCell = (NPOI.HSSF.UserModel.HSSFCell)detailRow.CreateCell(colpos);
        detailCell.SetCellValue(positionTitle);
        detailCell.CellStyle = detailStyle;

        detailCell2 = (NPOI.HSSF.UserModel.HSSFCell)detailRow2.CreateCell(colpos);
        detailCell2.SetCellValue(positionTitle);
        detailCell2.CellStyle = detailStyle;

        colpos++;

        workSheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(DETAIL_START_ROW + RowPos, DETAIL_START_ROW + RowPos + 1, colpos, colpos));
        detailCell = (NPOI.HSSF.UserModel.HSSFCell)detailRow.CreateCell(colpos);
        detailCell.SetCellValue(empInfo.EmpNo);
        detailCell.CellStyle = detailStyle;

        detailCell2 = (NPOI.HSSF.UserModel.HSSFCell)detailRow2.CreateCell(colpos);
        detailCell2.SetCellValue(empInfo.EmpNo);
        detailCell2.CellStyle = detailStyle;

        colpos++;

        for (int dayIdx = 0; dayIdx < countDay; dayIdx++)
        {
            //first line of employee record
            detailCell = (NPOI.HSSF.UserModel.HSSFCell)detailRow.CreateCell(colpos + dayIdx);
            detailCell.CellStyle = detailStyleCenterAlignment;
            // second line of employee data 
            detailCell2 = (NPOI.HSSF.UserModel.HSSFCell)detailRow2.CreateCell(colpos + dayIdx);
            detailCell2.CellStyle = detailStyleCenterAlignment;

            string currentTimeSpanRangeStr = getCurrentTimeSpanRangeStr(periodFromDate, dayIdx, empID);
            if (currentTimeSpanRangeStr != null)
            {
                detailCell.SetCellValue(currentTimeSpanRangeStr);
            }
            else
            {
                string currentRosterCodeDesc = getCurrentRosterCodeDesc(periodFromDate, dayIdx, empID);
                if (currentRosterCodeDesc != null)
                {
                    detailCell.SetCellValue(currentRosterCodeDesc);
                }
            }

            string s = getCurrentLeaveCodeDesc(periodFromDate, dayIdx, empID);
            if (s != "")
            {
                detailCell2.SetCellValue(s);
            }
        }

        colpos += countDay;
        workSheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(DETAIL_START_ROW + RowPos, DETAIL_START_ROW + RowPos + 1, colpos, colpos));

        detailCell = (NPOI.HSSF.UserModel.HSSFCell)detailRow.CreateCell(colpos);
        detailCell.SetCellValue(string.Empty);
        detailCell.CellStyle = detailStyleLast;

        detailCell2 = (NPOI.HSSF.UserModel.HSSFCell)detailRow2.CreateCell(colpos);
        detailCell2.SetCellValue(string.Empty);
        detailCell2.CellStyle = detailStyleLast;
    }

    private ERosterTable getCurrentRosterTable(DateTime periodFromDate, int dayIdx, int empID)
    {
        DateTime currentDate = periodFromDate.AddDays(dayIdx);
        List<ERosterTable> rosterTableList = ERosterTable.GetRosterTableList(dbConn, empID, currentDate);
        if (rosterTableList.Count > 0)
        {
            ERosterTable currentRosterTable = (ERosterTable)rosterTableList[0];
            return currentRosterTable;
        }
        else
        {
            return null;
        }
    }

    private TimeSpan getCurrentCutOffTimeSpan(DateTime periodFromDate, int dayIdx, int empID)
    {
        TimeSpan cutOffTimeSpan = new TimeSpan();
        ERosterTable currentRosterTable = getCurrentRosterTable(periodFromDate, dayIdx, empID);
        if (currentRosterTable != null)
        {
            ERosterCode currentRosterCode = new ERosterCode();
            currentRosterCode.RosterCodeID = currentRosterTable.RosterCodeID;
            if (ERosterCode.db.select(dbConn, currentRosterCode))
            {
                cutOffTimeSpan = currentRosterCode.RosterCodeCutOffTime.TimeOfDay;
            }
        }
        return cutOffTimeSpan;
    }

    private string getCurrentRosterCodeDesc(DateTime periodFromDate, int dayIdx, int empID)
    {
        string rosterCodeDesc = "--";
        ERosterTable currentRosterTable = getCurrentRosterTable(periodFromDate, dayIdx, empID);
        if (currentRosterTable != null)
        {
            ERosterCode currentRosterCode = new ERosterCode();
            currentRosterCode.RosterCodeID = currentRosterTable.RosterCodeID;
            if (ERosterCode.db.select(dbConn, currentRosterCode))
            {
                if (currentRosterTable.RosterTableID > 0)
                {
                    rosterCodeDesc = currentRosterCode.RosterCode + " - " + currentRosterCode.RosterCodeDesc;
                }
            }
        }
        return rosterCodeDesc;
    }

    private string getCurrentTimeSpanRangeStr(DateTime periodFromDate, int dayIdx, int empID)
    {
        DateTime currentDate = periodFromDate.AddDays(dayIdx);
        TimeSpan cutOffTimeSpan = getCurrentCutOffTimeSpan(periodFromDate, dayIdx, empID);
        TimeSpan timeSpanFrom = new TimeSpan(-1);
        TimeSpan timeSpanTo = new TimeSpan(-1);

        DBFilter timeCardRecordFilter = new DBFilter();
        OR orTimeCardRecordHolder = new OR();
        orTimeCardRecordHolder.add(new Match("EmpID", empID));
        timeCardRecordFilter.add(orTimeCardRecordHolder);
        timeCardRecordFilter.add(new Match("TimeCardRecordDateTime", ">=", currentDate.Add(cutOffTimeSpan)));
        timeCardRecordFilter.add(new Match("TimeCardRecordDateTime", "<", currentDate.AddDays(1).Add(cutOffTimeSpan)));
        timeCardRecordFilter.add(new Match("TimeCardRecordInOutIndex", ">=", 3));

        ArrayList timeCardRecordList = ETimeCardRecord.db.select(dbConn, timeCardRecordFilter);
        foreach (ETimeCardRecord timeCardRecord in timeCardRecordList)
        {
            if (timeCardRecord.TimeCardRecordInOutIndex == ETimeCardRecord.TimeCardRecordInOutIndexEnum.WorkStart)
                timeSpanFrom = timeCardRecord.TimeCardRecordDateTime.TimeOfDay;
            if (timeCardRecord.TimeCardRecordInOutIndex == ETimeCardRecord.TimeCardRecordInOutIndexEnum.WorkEnd)
                timeSpanTo = timeCardRecord.TimeCardRecordDateTime.TimeOfDay;
        }
        if (timeSpanFrom.Ticks >= 0 && timeSpanTo.Ticks >= 0)
        {
            return new DateTime().Add(timeSpanFrom).ToString("HH:mm") + " - " + new DateTime().Add(timeSpanTo).ToString("HH:mm");
        }
        else
        {
            return null;
        }
    }

    private string getCurrentLeaveCodeDesc(DateTime periodFromDate, int dayIdx, int empID)
    {
        DateTime currentDate = periodFromDate.AddDays(dayIdx);
        TimeSpan cutOffTimeSpan = getCurrentCutOffTimeSpan(periodFromDate, dayIdx, empID);

        // retrieve employee leave information of the day                    
        DBFilter leaveAppFilter = new DBFilter();
        ELeaveApplication leaveAppData = new ELeaveApplication();
        leaveAppFilter.add(new Match("EmpID", empID));
        leaveAppFilter.add(new Match("LeaveAppDateFrom", "<=", currentDate.Add(cutOffTimeSpan)));
        leaveAppFilter.add(new Match("LeaveAppDateTo", ">=", currentDate.Add(cutOffTimeSpan)));

        ArrayList leaveAppDataList = ELeaveApplication.db.select(dbConn, leaveAppFilter);
        string s = "";
        foreach (ELeaveApplication itemData in leaveAppDataList)
        {
            if (s != "")
            {
                s = s + "<br>";
            }

            // get leave code master
            DBFilter leaveCodeMasterFilter = new DBFilter();
            ELeaveCode leaveCodeMasterData = new ELeaveCode();
            leaveCodeMasterFilter.add(new Match("leaveCodeID", itemData.LeaveCodeID));
            ArrayList leaveCodeMasterDataList = ELeaveCode.db.select(dbConn, leaveCodeMasterFilter);

            if (leaveCodeMasterDataList != null)
            {
                s = s + ((ELeaveCode)leaveCodeMasterDataList[0]).LeaveCode + " - " + ((ELeaveCode)leaveCodeMasterDataList[0]).LeaveCodeDesc;
            }
        }
        if (s != "")
        {
            s.Replace("<br>", "\n");
        }
        return s;
    }

    private object GetFirstItem(ArrayList list)
    {
        if (list == null || list.Count <= 0)
            return null;
        else
            return list[0];
    }

}

