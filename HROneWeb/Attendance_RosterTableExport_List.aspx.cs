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

public partial class Attendance_RosterTableExport_List : HROneWebPage
{
    private const string FUNCTION_CODE = "ATT004";
    protected SearchBinding empSBinding, sbinding;
    public DBManager db = EEmpPersonalInfo.db;
    public EPayrollGroup obj;

    protected ListInfo empInfo;
    protected DataView empView;
    

    protected void Page_Load(object sender, EventArgs e)
    {
       
        if (!WebUtils.CheckAccess(Response, Session, FUNCTION_CODE, WebUtils.AccessLevel.Read))
            return;

        Year.Attributes.Add("onchange", "year=parseInt(" + Year.ClientID + ".value); if (!(year>1900)) {if (" + Year.ClientID + ".value!='') alert('Invalid Year');return true; }if (" + Month.ClientID + ".selectedIndex==0) return true;");
        Month.Attributes.Add("onchange", "year=parseInt(" + Year.ClientID + ".value); if (!(year>1900)) return true;");

        empSBinding = new SearchBinding(dbConn, EEmpPersonalInfo.db);
        empSBinding.add(new DropDownVLSearchBinder(Month, "Month", Values.VLMonth));
        empSBinding.initValues("EmpStatus", null, EEmpPersonalInfo.VLEmpStatus, HROne.Common.WebUtility.GetSessionUICultureInfo(Session));
        empSBinding.init(DecryptedRequest, null);

        empInfo = ListFooter.ListInfo;

        HROne.Common.WebUtility.WebControlsLocalization(this, this.Controls);
    }

    protected void Page_PreRender(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            //            empView = emploadData(empInfo, EEmpPayroll.db, empRepeater);
        }

    }

    protected void YearMonth_SelectedIndexChanged(object sender, EventArgs e)
    {
        empView = emploadData(empInfo, db, empRepeater);
    }

    public DataView emploadData(ListInfo info, DBManager db, Repeater repeater)
    {

        DBFilter filter = new DBFilter();// empSBinding.createFilter();

        //if (info != null && info.orderby != null && !info.orderby.Equals(""))
        //    filter.add(info.orderby, info.order);

        string select = "* ";
        string from = "from [" + EEmpPersonalInfo.db.dbclass.tableName + "] e ";


        DBFilter empInfoFilter = EmployeeSearchControl1.GetEmpInfoFilter(AppUtils.ServerDateTime(), AppUtils.ServerDateTime());
        empInfoFilter.add(new MatchField("e.EmpID", "ee.EmpID"));

        int m_Year, m_Month;
        if (int.TryParse(Year.Text, out m_Year) && int.TryParse(Month.SelectedValue, out m_Month))
        {
            DateTime dtPeriodFr = new DateTime(m_Year, m_Month, 1);
            DateTime dtPeriodTo = new DateTime(m_Year, m_Month, DateTime.DaysInMonth(m_Year, m_Month));

            empInfoFilter.add(new Match("EmpDateOfJoin", "<=", dtPeriodTo));
            DBFilter empTermFilter = new DBFilter();
            empTermFilter.add(new MatchField("ee.EmpID", "et.EmpID"));
            empTermFilter.add(new Match("et.EmpTermLastDate", "<", dtPeriodFr));
            empInfoFilter.add(new Exists(EEmpTermination.db.dbclass.tableName + " et ", empTermFilter, true));

        }
        else
        {
            btnGenerate.Visible = false;
            empView = null;
            repeater.DataSource = null;
            repeater.DataBind();

            return null;
        }

        filter.add(new Exists(EEmpPersonalInfo.db.dbclass.tableName + " ee", empInfoFilter));

        if (m_Year < 1800)
        {
            return null;
        }

        DataTable table = filter.loadData(dbConn, null, select, from);
        table = EmployeeSearchControl1.FilterEncryptedEmpInfoField(table, info);

        if (table.Rows.Count != 0)
        {
            btnGenerate.Visible = true;
        }
        else
        {
            btnGenerate.Visible = false;
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
        empInfo.recordPerPage = -1;

        empView = emploadData(empInfo, db, empRepeater);

    }
    protected void Reset_Click(object sender, EventArgs e)
    {
        empSBinding.clear();
        EmployeeSearchControl1.Reset();
        empInfo.page = 0;
        empInfo.recordPerPage = -1;

        empView = emploadData(empInfo, db, empRepeater);
    }

    protected void btnGenerate_Click(object sender, EventArgs e)
    {
        PageErrors errors = PageErrors.getErrors(db, Page.Master);
        errors.clear();



        int m_Year = 0;
        int m_Month = 0;
        if (!int.TryParse(Year.Text, out m_Year) || !int.TryParse(Month.SelectedValue, out m_Month))
        {
            errors.addError("Invalid Year/Month");
        }

        if (m_Year < 1900)
            errors.addError("Invalid Year");

        if (errors.isEmpty())
        {

            ArrayList list = new ArrayList();
            list = WebUtils.SelectedRepeaterItemToBaseObjectList(EEmpPersonalInfo.db, empRepeater, "ItemSelect");

            if (list.Count > 0)
            {
                GenerateRosterTableData(list, m_Year, m_Month);
            }
        }
        emploadData(empInfo, db, empRepeater);
    }

    private void GenerateRosterTableData(ArrayList EmpInfoList, int year, int month)
    {
        DateTime dateStart = new DateTime(year, month, 1);
        DateTime dateEnd = new DateTime(year, month, DateTime.DaysInMonth(year, month));
        string exportFileName = System.IO.Path.GetTempFileName();
        System.IO.File.Delete(exportFileName);
        exportFileName += ".xls";

        const string FIELD_EMP_NO = "Emp. No";

        const int COLUMN_HEADER_ROW = 2;


        NPOI.HSSF.UserModel.HSSFWorkbook workBook = new NPOI.HSSF.UserModel.HSSFWorkbook();
        NPOI.HSSF.UserModel.HSSFSheet workSheet = (NPOI.HSSF.UserModel.HSSFSheet)workBook.CreateSheet("RosterTable");

        NPOI.HSSF.UserModel.HSSFCellStyle upperLineStyle = (NPOI.HSSF.UserModel.HSSFCellStyle)workBook.CreateCellStyle();
        upperLineStyle.BorderTop = NPOI.SS.UserModel.BorderStyle.THIN;

        NPOI.HSSF.UserModel.HSSFCellStyle bottomLineStyle = (NPOI.HSSF.UserModel.HSSFCellStyle)workBook.CreateCellStyle();
        bottomLineStyle.BorderBottom = NPOI.SS.UserModel.BorderStyle.THIN;

        NPOI.HSSF.UserModel.HSSFCellStyle leftLineStyle = (NPOI.HSSF.UserModel.HSSFCellStyle)workBook.CreateCellStyle();
        leftLineStyle.BorderLeft = NPOI.SS.UserModel.BorderStyle.THIN;

        NPOI.HSSF.UserModel.HSSFCellStyle rightLineStyle = (NPOI.HSSF.UserModel.HSSFCellStyle)workBook.CreateCellStyle();
        rightLineStyle.BorderRight = NPOI.SS.UserModel.BorderStyle.THIN;

        NPOI.HSSF.UserModel.HSSFCellStyle upperLeftLineStyle = (NPOI.HSSF.UserModel.HSSFCellStyle)workBook.CreateCellStyle();
        upperLeftLineStyle.BorderTop = NPOI.SS.UserModel.BorderStyle.THIN;
        upperLeftLineStyle.BorderLeft = NPOI.SS.UserModel.BorderStyle.THIN;

        NPOI.HSSF.UserModel.HSSFCellStyle bottomLeftLineStyle = (NPOI.HSSF.UserModel.HSSFCellStyle)workBook.CreateCellStyle();
        bottomLeftLineStyle.BorderBottom = NPOI.SS.UserModel.BorderStyle.THIN;
        bottomLeftLineStyle.BorderLeft = NPOI.SS.UserModel.BorderStyle.THIN;

        NPOI.HSSF.UserModel.HSSFCellStyle upperRightLineStyle = (NPOI.HSSF.UserModel.HSSFCellStyle)workBook.CreateCellStyle();
        upperRightLineStyle.BorderTop = NPOI.SS.UserModel.BorderStyle.THIN;
        upperRightLineStyle.BorderRight = NPOI.SS.UserModel.BorderStyle.THIN;

        NPOI.HSSF.UserModel.HSSFCellStyle bottomRightLineStyle = (NPOI.HSSF.UserModel.HSSFCellStyle)workBook.CreateCellStyle();
        bottomRightLineStyle.BorderBottom = NPOI.SS.UserModel.BorderStyle.THIN;
        bottomRightLineStyle.BorderRight = NPOI.SS.UserModel.BorderStyle.THIN;

        workSheet.CreateRow(0).CreateCell(0).SetCellValue("Year");
        workSheet.GetRow(0).CreateCell(1).SetCellValue(year);
        workSheet.CreateRow(1).CreateCell(0).SetCellValue("Month");
        workSheet.GetRow(1).CreateCell(1).SetCellValue(month);

        NPOI.HSSF.UserModel.HSSFRow headerRow = (NPOI.HSSF.UserModel.HSSFRow)workSheet.CreateRow(COLUMN_HEADER_ROW);
        DBFilter hLevelFilter = new DBFilter();
        hLevelFilter.add("HLevelSeqNo",true);

        int HIERARCHYLEVEL_COLUMN = 0;
        ArrayList hLevelList = EHierarchyLevel.db.select(dbConn, hLevelFilter);
        for (int levelIndex = 0; levelIndex < hLevelList.Count; levelIndex++)
        {
            EHierarchyLevel hLevel = (EHierarchyLevel)hLevelList[levelIndex];
            headerRow.CreateCell(HIERARCHYLEVEL_COLUMN + levelIndex).SetCellValue(hLevel.HLevelDesc);
        }

        int POSITION_COLUMN = headerRow.LastCellNum;
        headerRow.CreateCell(POSITION_COLUMN).SetCellValue(HROne.Common.WebUtility.GetLocalizedString("Position"));
        int EMPNO_COLUMN = headerRow.LastCellNum;
        headerRow.CreateCell(EMPNO_COLUMN).SetCellValue(FIELD_EMP_NO);
        headerRow.CreateCell(EMPNO_COLUMN + 1).SetCellValue(HROne.Common.WebUtility.GetLocalizedString("Name"));
        headerRow.CreateCell(EMPNO_COLUMN + 2).SetCellValue(HROne.Common.WebUtility.GetLocalizedString("Alias"));
        headerRow.CreateCell(EMPNO_COLUMN + 3).SetCellValue(HROne.Common.WebUtility.GetLocalizedString("Chinese Name"));

        NPOI.HSSF.UserModel.HSSFCellStyle sundayStyle = (NPOI.HSSF.UserModel.HSSFCellStyle)workBook.CreateCellStyle();
        NPOI.HSSF.UserModel.HSSFFont sundayFont = (NPOI.HSSF.UserModel.HSSFFont)workBook.CreateFont();
        sundayFont.Color = NPOI.HSSF.Util.HSSFColor.RED.index;
        sundayStyle.SetFont(sundayFont);

        Hashtable styleList = new Hashtable();

        ArrayList availableRosterClientList = new ArrayList();
        ArrayList availableRosterClientSiteList = new ArrayList();

        #region Create Column Header
        int ROSTER_DETAIL_COLUMN = headerRow.LastCellNum;

        for (int i = 1; i <= DateTime.DaysInMonth(year, month); i++)
        {
            //workSheet.Cells.Add(HEADER_ROW, ROSTAER_DETAIL_COLUMN + i - 1,i);
            NPOI.HSSF.UserModel.HSSFCell headerCell = (NPOI.HSSF.UserModel.HSSFCell)headerRow.CreateCell(ROSTER_DETAIL_COLUMN + i - 1);
            headerCell.SetCellValue(i);
            if (new DateTime(year, month, i).DayOfWeek == DayOfWeek.Sunday)
                headerCell.CellStyle = sundayStyle;
        }
        #endregion
        #region Create Employee Roster Detail
        int recordCount = 0;
        foreach (EEmpPersonalInfo empInfo in EmpInfoList)
        {
            if (EEmpPersonalInfo.db.select(dbConn, empInfo))
            {
                recordCount++;

                //workSheet.Cells.Add(HEADER_ROW + recordCount, 1,empInfo.EmpNo);
                //workSheet.Cells.Add(HEADER_ROW + recordCount, 2,empInfo.EmpEngFullName);

                NPOI.HSSF.UserModel.HSSFRow detailRow = (NPOI.HSSF.UserModel.HSSFRow)workSheet.CreateRow(COLUMN_HEADER_ROW + recordCount);
                EEmpPositionInfo empPos = AppUtils.GetLastPositionInfo(dbConn, dateEnd, empInfo.EmpID);
                if (empPos != null)
                {
                    for (int levelIndex = 0; levelIndex < hLevelList.Count; levelIndex++)
                    {
                        EHierarchyLevel hLevel = (EHierarchyLevel)hLevelList[levelIndex];
                        DBFilter empHierarchyFilter = new DBFilter();
                        empHierarchyFilter.add(new Match("EmpPosID", empPos.EmpPosID));
                        empHierarchyFilter.add(new Match("HLevelID", hLevel.HLevelID));
                        ArrayList empHierarchyList = EEmpHierarchy.db.select(dbConn, empHierarchyFilter);
                        if (empHierarchyList.Count > 0)
                        {
                            EEmpHierarchy empHierarchy = (EEmpHierarchy)empHierarchyList[0];
                            EHierarchyElement hElement = new EHierarchyElement();
                            hElement.HElementID = empHierarchy.HElementID;
                            if (EHierarchyElement.db.select(dbConn, hElement))
                                detailRow.CreateCell(HIERARCHYLEVEL_COLUMN + levelIndex).SetCellValue(hElement.HElementDesc);
                        }
                    }

                    EPosition position = new EPosition();
                    position.PositionID = empPos.PositionID;
                    if (EPosition.db.select(dbConn, position))
                        detailRow.CreateCell(POSITION_COLUMN).SetCellValue(position.PositionDesc);
                }
                detailRow.CreateCell(EMPNO_COLUMN).SetCellValue(empInfo.EmpNo);
                detailRow.CreateCell(EMPNO_COLUMN + 1).SetCellValue(empInfo.EmpEngFullName);
                detailRow.CreateCell(EMPNO_COLUMN + 2).SetCellValue(empInfo.EmpAlias);
                detailRow.CreateCell(EMPNO_COLUMN + 3).SetCellValue(empInfo.EmpChiFullName);



                DBFilter rosterTableFilter = new DBFilter();
                rosterTableFilter.add(new Match("EmpID", empInfo.EmpID));
                rosterTableFilter.add(new Match("RosterTableDate", ">=", dateStart));
                rosterTableFilter.add(new Match("RosterTableDate", "<=", dateEnd));
                ArrayList rosterTableList = ERosterTable.db.select(dbConn, rosterTableFilter);
                foreach (ERosterTable rosterTable in rosterTableList)
                {
                    ERosterCode rosterCode = new ERosterCode();
                    rosterCode.RosterCodeID = rosterTable.RosterCodeID;
                    if (ERosterCode.db.select(dbConn, rosterCode))
                    {
                        string value = string.Empty;
                        //if (workSheet.Rows[(ushort)(HEADER_ROW + recordCount)].CellExists ((ushort)(ROSTAER_DETAIL_COLUMN + rosterTable.RosterTableDate.Day - 1)) )
                        //    value = workSheet.Rows[(ushort)(HEADER_ROW+ recordCount)].CellAtCol( (ushort)(ROSTAER_DETAIL_COLUMN + rosterTable.RosterTableDate.Day - 1)).Value.ToString();
                        //if (string.IsNullOrEmpty(value))
                        //    workSheet.Cells.Add(HEADER_ROW + recordCount, ROSTAER_DETAIL_COLUMN + rosterTable.RosterTableDate.Day - 1,rosterCode.RosterCode);
                        //else
                        //    workSheet.Cells.Add(HEADER_ROW + recordCount, ROSTAER_DETAIL_COLUMN + rosterTable.RosterTableDate.Day - 1,value + "|" + rosterCode.RosterCode);

                        int cellColIndex = ROSTER_DETAIL_COLUMN + rosterTable.RosterTableDate.Day - 1;
                        NPOI.HSSF.UserModel.HSSFCell rosterCell = (NPOI.HSSF.UserModel.HSSFCell)detailRow.GetCell(cellColIndex);
                        if (rosterCell == null)
                            rosterCell = (NPOI.HSSF.UserModel.HSSFCell)detailRow.CreateCell(cellColIndex);
                        else
                            value = rosterCell.StringCellValue;
                        string rosterCodeValue = rosterCode.RosterCode;
                        if (!rosterTable.RosterTableOverrideInTime.Ticks.Equals(0) || !rosterTable.RosterTableOverrideOutTime.Ticks.Equals(0))
                        {
                            DateTime inTime = rosterCode.RosterCodeInTime;
                            DateTime outTime = rosterCode.RosterCodeOutTime;
                            if (!rosterTable.RosterTableOverrideInTime.Ticks.Equals(0))
                                inTime = rosterTable.RosterTableOverrideInTime;
                            if (!rosterTable.RosterTableOverrideOutTime.Ticks.Equals(0))
                                outTime = rosterTable.RosterTableOverrideOutTime;
                            rosterCodeValue += "(" + inTime.ToString("HHmm") + "~" + outTime.ToString("HHmm") + ")";
                        }
                        if (string.IsNullOrEmpty(value))
                        {
                            rosterCell.SetCellValue(rosterCodeValue);
                            //if (!string.IsNullOrEmpty(rosterCode.RosterCodeColorCode))
                            //{
                            //    //System.Drawing.Color color = System.Drawing.ColorTranslator.FromHtml(rosterCode.RosterCodeColorCode);
                            //    //System.Drawing.Color fontcolor = WebUtils.ComputeTextColor(color);
                            //    //rosterCell.CellStyle.FillForegroundColor = workBook.GetCustomPalette().FindSimilarColor(color.R, color.G, color.B).GetIndex();
                            //    //rosterCell.CellStyle.FillPattern = NPOI.SS.UserModel.FillPatternType.SOLID_FOREGROUND;
                            //    //rosterCell.CellStyle.FillBackgroundColor = workBook.GetCustomPalette().FindSimilarColor(fontcolor.R, fontcolor.G, fontcolor.B).GetIndex();
                            //    string styleCode = "RosterCode" + "_" + rosterCode.RosterCode;
                            //    if (styleList.Contains(styleCode))
                            //        rosterCell.CellStyle = (NPOI.HSSF.UserModel.HSSFCellStyle)styleList[styleCode];
                            //    else
                            //    {
                            //        NPOI.HSSF.UserModel.HSSFCellStyle rosterCodeStyle = (NPOI.HSSF.UserModel.HSSFCellStyle)workBook.CreateCellStyle();
                            //        System.Drawing.Color color = System.Drawing.ColorTranslator.FromHtml(rosterCode.RosterCodeColorCode);
                            //        System.Drawing.Color fontcolor = WebUtils.ComputeTextColor(color);
                            //        rosterCodeStyle.FillForegroundColor = workBook.GetCustomPalette().FindSimilarColor(color.R, color.G, color.B).GetIndex();
                            //        rosterCodeStyle.FillPattern = NPOI.SS.UserModel.FillPatternType.SOLID_FOREGROUND;
                            //        rosterCodeStyle.FillBackgroundColor = workBook.GetCustomPalette().FindSimilarColor(fontcolor.R, fontcolor.G, fontcolor.B).GetIndex();
                            //        styleList.Add(styleCode, rosterCodeStyle);
                            //        rosterCell.CellStyle = rosterCodeStyle;
                            //    }
                            //}
                        }
                        else
                        {
                            rosterCell.SetCellValue(value + "|" + rosterCodeValue);
                            //rosterCell.CellStyle=workBook.GetCellStyleAt(0);
                        }

                    }
                }
                for (DateTime dateIndex = dateStart; dateIndex <= dateEnd; dateIndex = dateIndex.AddDays(1))
                {
                    string value = string.Empty;
                    //if (workSheet.Rows[(ushort)(HEADER_ROW + recordCount)].CellExists((ushort)(ROSTAER_DETAIL_COLUMN + dateIndex.Day - 1)) )
                    //    value = workSheet.Rows[(ushort)(HEADER_ROW + recordCount)].CellAtCol((ushort)(ROSTAER_DETAIL_COLUMN + dateIndex.Day - 1)).Value.ToString();
                    int cellColIndex = ROSTER_DETAIL_COLUMN + dateIndex.Day - 1;
                    NPOI.HSSF.UserModel.HSSFCell rosterCell = (NPOI.HSSF.UserModel.HSSFCell)detailRow.GetCell(cellColIndex);
                    if (rosterCell == null)
                        rosterCell = (NPOI.HSSF.UserModel.HSSFCell)detailRow.CreateCell(cellColIndex);
                    else
                        value = rosterCell.StringCellValue;

                    if (string.IsNullOrEmpty(value))
                    {
                        EEmpPositionInfo empPosInfo = AppUtils.GetLastPositionInfo(dbConn, dateIndex, empInfo.EmpID);
                        if (empPosInfo != null)
                            if ((empPosInfo.EmpPosEffTo.Ticks.Equals(0) || dateIndex <= empPosInfo.EmpPosEffTo))
                            {
                                EWorkHourPattern workPattern = new EWorkHourPattern();
                                workPattern.WorkHourPatternID = empPosInfo.WorkHourPatternID;
                                if (EWorkHourPattern.db.select(dbConn, workPattern))
                                {

                                    ERosterCode rosterCode = new ERosterCode();
                                    rosterCode.RosterCodeID = workPattern.GetDefaultRosterCodeID(dbConn, dateIndex);
                                    if (ERosterCode.db.select(dbConn, rosterCode))
                                    {
                                        //workSheet.Cells.Add(HEADER_ROW + recordCount, ROSTAER_DETAIL_COLUMN + dateIndex.Day - 1, rosterCode.RosterCode);
                                        rosterCell.SetCellValue(rosterCode.RosterCode);
                                    }
                                }

                                DBFilter empRosterTableGroupListFilter = new DBFilter();
                                empRosterTableGroupListFilter.add(new Match("EmpID", empInfo.EmpID));
                                empRosterTableGroupListFilter.add(new Match("empRosterTableGroupEffFr", "<=", dateIndex));
                                OR orEmpPosEffToTerms = new OR();
                                orEmpPosEffToTerms.add(new Match("empRosterTableGroupEffTo", ">=", dateIndex));
                                orEmpPosEffToTerms.add(new NullTerm("empRosterTableGroupEffTo"));
                                empRosterTableGroupListFilter.add(orEmpPosEffToTerms);
                                ArrayList empRosterTableGroupList = EEmpRosterTableGroup.db.select(dbConn, empRosterTableGroupListFilter);

                                foreach (EEmpRosterTableGroup empRosterTableGroup in empRosterTableGroupList)
                                {
                                    ERosterTableGroup rosterTableGroup = new ERosterTableGroup();
                                    rosterTableGroup.RosterTableGroupID = empRosterTableGroup.RosterTableGroupID;
                                    if (ERosterTableGroup.db.select(dbConn, rosterTableGroup))
                                    {
                                        if (rosterTableGroup.RosterClientSiteID > 0)
                                        {
                                            if (!availableRosterClientSiteList.Contains(rosterTableGroup.RosterClientSiteID))
                                                availableRosterClientSiteList.Add(rosterTableGroup.RosterClientSiteID);
                                        }
                                        else if (rosterTableGroup.RosterClientID > 0)
                                        {
                                            if (!availableRosterClientList.Contains(rosterTableGroup.RosterClientID))
                                                availableRosterClientList.Add(rosterTableGroup.RosterClientID);
                                        }
                                    }
                                }
                            }
                    }
                }
                DBFilter leaveAppFilter = new DBFilter();
                leaveAppFilter.add(new Match("EmpID", empInfo.EmpID));
                leaveAppFilter.add(new Match("LeaveAppDateTo", ">=", dateStart));
                leaveAppFilter.add(new Match("LeaveAppDateFrom", "<=", dateEnd));
                ArrayList leaveAppList = ELeaveApplication.db.select(dbConn, leaveAppFilter);
                foreach (ELeaveApplication leaveApp in leaveAppList)
                {
                    ELeaveCode leaveCode = new ELeaveCode();
                    leaveCode.LeaveCodeID = leaveApp.LeaveCodeID;
                    if (ELeaveCode.db.select(dbConn, leaveCode))
                    {
                        //if (leaveCode.LeaveCodeColorCode.Length == 6)
                        //{
                        //    try
                        //    {
                        //        int red = System.Int32.Parse(leaveCode.LeaveCodeColorCode.Substring(0, 2), System.Globalization.NumberStyles.AllowHexSpecifier);
                        //        int green = System.Int32.Parse(leaveCode.LeaveCodeColorCode.Substring(2, 2), System.Globalization.NumberStyles.AllowHexSpecifier);
                        //        int blue = System.Int32.Parse(leaveCode.LeaveCodeColorCode.Substring(4, 2), System.Globalization.NumberStyles.AllowHexSpecifier);

                        //        string Color = System.Drawing.Color.FromArgb(red, green, blue).ToKnownColor().ToString();

                        //        for (DateTime dateIndex = leaveApp.LeaveAppDateFrom; dateIndex <= leaveApp.LeaveAppDateTo; dateIndex = dateIndex.AddDays(1))
                        //        {
                        //            org.in2bits.MyXls.Cell cell = workSheet.Rows[(ushort)(HEADER_ROW + recordCount)].CellAtCol((ushort)(ROSTAER_DETAIL_COLUMN + dateIndex.Day - 1));
                        //            //cell.Pattern = 1;
                        //            //cell.PatternColor = org.in2bits.MyXls.Colors.Yellow;

                        //        }
                        //    }
                        //    catch
                        //    {
                        //    }

                        //}
                    }
                }
            }
        }
#endregion
        #region Create Roster Code Liet
        OR orRosterCodeTerm = new OR();
        foreach (int rosterClientID in availableRosterClientList)
        {
            AND andRosterCodeTerms = new AND();
            orRosterCodeTerm.add(new Match("RosterClientID", rosterClientID));
        }
        foreach (int rosterClientSiteID in availableRosterClientSiteList)
        {
            AND andRosterCodeTerms = new AND();
            orRosterCodeTerm.add(new Match("RosterClientSiteID", rosterClientSiteID));
        }
        orRosterCodeTerm.add(new Match("RosterClientID", 0));
        DBFilter rosterCodeListFilter = new DBFilter();
        rosterCodeListFilter.add(orRosterCodeTerm);
        rosterCodeListFilter.add("RosterCode", true);
        ArrayList rosterCodeList = ERosterCode.db.select(dbConn, rosterCodeListFilter);

        int ROSTER_CODE_START_ROW = COLUMN_HEADER_ROW + recordCount + 5;
        int rosterCodeCount = 0;
        int maxColumnCount = 3;
        int columnCellWidth = 9;
        int maxRowCount = (int)(rosterCodeList.Count / maxColumnCount) + (rosterCodeList.Count % maxColumnCount == 0 ? 0 : 1);
        foreach (ERosterCode rosterCode in rosterCodeList)
        {
            int currentRowNum = rosterCodeCount % maxRowCount;
            int currentColumnNum = (rosterCodeCount / maxRowCount) * columnCellWidth;

            rosterCodeCount++;

            NPOI.HSSF.UserModel.HSSFRow rosterCodeRow = (NPOI.HSSF.UserModel.HSSFRow)workSheet.GetRow(ROSTER_CODE_START_ROW + currentRowNum);
            if (rosterCodeRow == null)
                rosterCodeRow = (NPOI.HSSF.UserModel.HSSFRow)workSheet.CreateRow(ROSTER_CODE_START_ROW + currentRowNum);

            NPOI.HSSF.UserModel.HSSFCell rosterCell = (NPOI.HSSF.UserModel.HSSFCell)rosterCodeRow.CreateCell(ROSTER_DETAIL_COLUMN + currentColumnNum);
            rosterCell.SetCellValue(rosterCode.RosterCode);

            rosterCell = (NPOI.HSSF.UserModel.HSSFCell)rosterCodeRow.CreateCell(ROSTER_DETAIL_COLUMN + currentColumnNum + 1);
            rosterCell.SetCellValue(rosterCode.RosterCodeDesc);

            if (rosterCodeCount.Equals(1))
            {
                rosterCell = (NPOI.HSSF.UserModel.HSSFCell)rosterCodeRow.CreateCell(ROSTER_DETAIL_COLUMN - 1);
                rosterCell.SetCellValue("Code:");
            }


        }

        for (int rowIdx = ROSTER_CODE_START_ROW - 1; rowIdx < ROSTER_CODE_START_ROW + maxRowCount + 1; rowIdx++)
        {
            NPOI.HSSF.UserModel.HSSFRow rosterCodeRow = (NPOI.HSSF.UserModel.HSSFRow)workSheet.GetRow(rowIdx);
            if (rosterCodeRow == null)
                rosterCodeRow = (NPOI.HSSF.UserModel.HSSFRow)workSheet.CreateRow(rowIdx);

            if (rowIdx == ROSTER_CODE_START_ROW - 1)
            {
                for (int colIdx = ROSTER_DETAIL_COLUMN - 1; colIdx < ROSTER_DETAIL_COLUMN + maxColumnCount * columnCellWidth; colIdx++)
                {
                    NPOI.HSSF.UserModel.HSSFCell rosterCell = (NPOI.HSSF.UserModel.HSSFCell)rosterCodeRow.GetCell(colIdx);
                    if (rosterCell == null)
                        rosterCell = (NPOI.HSSF.UserModel.HSSFCell)rosterCodeRow.CreateCell(colIdx);
                    if (colIdx == ROSTER_DETAIL_COLUMN - 1)
                        rosterCell.CellStyle = upperLeftLineStyle;
                    else if (colIdx == ROSTER_DETAIL_COLUMN + maxColumnCount * columnCellWidth - 1)
                        rosterCell.CellStyle = upperRightLineStyle;
                    else
                        rosterCell.CellStyle = upperLineStyle;

                }

            }
            else if (rowIdx == ROSTER_CODE_START_ROW + maxRowCount)
            {
                for (int colIdx = ROSTER_DETAIL_COLUMN - 1; colIdx < ROSTER_DETAIL_COLUMN + maxColumnCount * columnCellWidth; colIdx++)
                {
                    NPOI.HSSF.UserModel.HSSFCell rosterCell = (NPOI.HSSF.UserModel.HSSFCell)rosterCodeRow.GetCell(colIdx);
                    if (rosterCell == null)
                        rosterCell = (NPOI.HSSF.UserModel.HSSFCell)rosterCodeRow.CreateCell(colIdx);
                    if (colIdx == ROSTER_DETAIL_COLUMN - 1)
                        rosterCell.CellStyle = bottomLeftLineStyle;
                    else if (colIdx == ROSTER_DETAIL_COLUMN + maxColumnCount * columnCellWidth - 1)
                        rosterCell.CellStyle = bottomRightLineStyle;
                    else
                        rosterCell.CellStyle = bottomLineStyle;

                }

            }
            else
            {
                for (int colIdx = ROSTER_DETAIL_COLUMN - 1; colIdx < ROSTER_DETAIL_COLUMN + maxColumnCount * columnCellWidth; colIdx++)
                {
                    NPOI.HSSF.UserModel.HSSFCell rosterCell = (NPOI.HSSF.UserModel.HSSFCell)rosterCodeRow.GetCell(colIdx);
                    if (rosterCell == null)
                        rosterCell = (NPOI.HSSF.UserModel.HSSFCell)rosterCodeRow.CreateCell(colIdx);
                    if (colIdx == ROSTER_DETAIL_COLUMN - 1)
                        rosterCell.CellStyle = leftLineStyle;
                    else if (colIdx == ROSTER_DETAIL_COLUMN + maxColumnCount * columnCellWidth - 1)
                        rosterCell.CellStyle = rightLineStyle;
                    //else
                    //    rosterCell.CellStyle = bottomLineStyle;

                }
            }


        }
#endregion

        //doc.FileName = exportFileName;
        //doc.Save();
        System.IO.FileStream file = new System.IO.FileStream(exportFileName, System.IO.FileMode.Create);
        workBook.Write(file);
        file.Close();

        WebUtils.TransmitFile(Response, exportFileName, "RosterTable_" + AppUtils.ServerDateTime().ToString("yyyyMMddHHmmss") + ".xls", true);
        Response.End();

    }
}
