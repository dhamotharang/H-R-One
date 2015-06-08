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
using HROne.CommonLib;

public partial class Average_Cost_Centre_Export : HROneWebPage
{
    private const string FUNCTION_CODE = "CST007";

    protected SearchBinding sbinding, binding;
    public DBManager db = EEmpPersonalInfo.db;

    protected ListInfo info;
    protected DataView view;

    private bool IsAllowEdit = true;
    DateTime startMonth, tempMonth, endMonth;
    string peroid;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!WebUtils.CheckAccess(Response, Session, FUNCTION_CODE, WebUtils.AccessLevel.Read))
            return;
        if (!WebUtils.CheckPermission(Session, FUNCTION_CODE, WebUtils.AccessLevel.ReadWrite))
        {
            IsAllowEdit = false;
            ConfirmPayrollSelectAllPanel.Visible = false;
        }

        Year.Attributes.Add("onchange", "year=parseInt(" + Year.ClientID + ".value); if (!(year>1900)) {if (" + Year.ClientID + ".value!='') alert('Invalid Year');return true; }if (" + Month.ClientID + ".selectedIndex==0) return true;");
        Month.Attributes.Add("onchange", "year=parseInt(" + Year.ClientID + ".value); if (!(year>1900)) return true;");
        Year2.Attributes.Add("onchange", "year2=parseInt(" + Year2.ClientID + ".value); if (!(year2>1900)) {if (" + Year2.ClientID + ".value!='') alert('Invalid Year');return true; }if (" + Month2.ClientID + ".selectedIndex==0) return true;");
        Month2.Attributes.Add("onchange", "year2=parseInt(" + Year2.ClientID + ".value); if (!(year2>1900)) return true;");

        sbinding = new SearchBinding(dbConn, db);
        sbinding.init(DecryptedRequest, null);

        binding = new SearchBinding(dbConn, EEmpCostCenter.db);
        binding.add(new DropDownVLSearchBinder(Month, "Month", Values.VLMonth));
        binding.add(new DropDownVLSearchBinder(Month2, "Month", Values.VLMonth));
        binding.init(DecryptedRequest, null);

        info = ListFooter.ListInfo;

        HROne.Common.WebUtility.WebControlsLocalization(this, this.Controls);
    }

    protected void Page_PreRender(object sender, EventArgs e)
    {
        panelPayPeriod.Visible = true;

        if (view != null && view.Count > 0)
        {
            btnExport.Visible = true;
            btnExport2.Visible = true;
        }
        else
        {
            btnExport.Visible = false;
            btnExport2.Visible = false;
        }

        //if (!Page.IsPostBack)
        //    view = loadData(info, db, Repeater);
    }

    public DataView loadData(ListInfo info, DBManager db, Repeater repeater)
    {
        int m_year;
        int m_month;
        int m_year2;
        int m_month2;

        if (int.TryParse(Year.Text, out m_year) && m_year > 1980)
            if (int.TryParse(Month.SelectedValue, out m_month) && m_month > 0)
                if (int.TryParse(Year2.Text, out m_year2) && m_year2 > 1980)
                    if (int.TryParse(Month2.SelectedValue, out m_month2) && m_month2 > 0)
                    {
                        DateTime m_fromDate = new DateTime(m_year, m_month, 1);
                        DateTime m_toDate = new DateTime(m_year2, m_month2, DateTime.DaysInMonth(m_year2, m_month2));

                        if (m_fromDate > m_toDate)
                        {
                            PageErrors errors = PageErrors.getErrors(db, Page.Master);
                            errors.addError("Invalid Date Range Parameter: From-Date must be prior to To-Date");
                        }
                        else
                        {
                            DBFilter filter = new DBFilter();
                            OR orEffTo = new OR();

                            filter.add(new Match("EmpCostCenterEffFr", "<=", m_toDate));
                            orEffTo.add(new Match("EmpCostCenterEffTo", ">=", m_fromDate));
                            orEffTo.add(new NullTerm("EmpCostCenterEffTo"));
                            filter.add(orEffTo);

                            DBFilter empfilter = sbinding.createFilter();
                            empfilter.add(new IN("e.EmpID", "Select distinct EmpID from " + EEmpCostCenter.db.dbclass.tableName, filter));
                            //empfilter.add(WebUtils.AddRankFilter(Session, "EmpID", true));

                            string select = " e.* ";
                            string from = " from " + db.dbclass.tableName + " e ";

                            // Start 0000185, KuangWei, 2015-04-21
                            DBFilter empInfoFilter = EmployeeSearchControl1.GetEmpInfoFilter(AppUtils.ServerDateTime(), AppUtils.ServerDateTime());
                            empInfoFilter.add(new MatchField("e.EmpID", "ee.EmpID"));
                            empfilter.add(new Exists(EEmpPersonalInfo.db.dbclass.tableName + " ee", empInfoFilter));

                            DataTable table = WebUtils.GetDataTableFromSelectQueryWithFilter(dbConn, select, from, empfilter, info);
                            table = EmployeeSearchControl1.FilterEncryptedEmpInfoField(table, info);
                            // End 0000185, KuangWei, 2015-04-21

                            view = new DataView(table);
                            ListFooter.Refresh();

                            if (repeater != null)
                            {
                                repeater.DataSource = view;
                                repeater.DataBind();
                            }
                        }
                    }
        return view;
    }

    protected void DateRange_Changed(object sender, EventArgs e)
    {
        if (Page.IsPostBack)
            view = loadData(info, db, Repeater);
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

        view = loadData(info, db, Repeater);
    }

    protected void Repeater_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        DataRowView row = (DataRowView)e.Item.DataItem;
        CheckBox cb = (CheckBox)e.Item.FindControl("ItemSelect");
        WebFormUtils.LoadKeys(db, row, cb);
        cb.Visible = IsAllowEdit;
    }

    // Start 0000185, KuangWei, 2015-04-21
    protected void Search_Click(object sender, EventArgs e)
    {
        view = loadData(info, db, Repeater);
    }

    protected void Reset_Click(object sender, EventArgs e)
    {
        sbinding.clear();
        EmployeeSearchControl1.Reset();
        EmployeeSearchControl1.EmpStatusValue = "A";
        view = loadData(info, db, Repeater);
    }
    // End 0000185, KuangWei, 2015-04-21  

    public const string TABLE_NAME = "AverageCostCentreExport";
    private const string FIELD_COMPANY = "Company";
    private const string FIELD_DIVISION = "Divistion";
    private const string FIELD_DEPARTMENT = "Department";
    private const string FIELD_SECTION = "Section";
    private const string FIELD_EMP_NO = "Emp No";
    private const string FIELD_EMP_NAME = "Employee Name";
    private const string FIELD_ALIAS = "Alias";
    private const string FIELD_POSITION = "Position";
    private const string FIELD_FROM = "From";
    private const string FIELD_TO = "To";
    private const string FIELD_COST_CENTER = "Cost Center";
    private const string FIELD_PERCENTAGE = "%";

    protected void btnExport_Click(object sender, EventArgs e)
    {
        startMonth = DateTime.Parse(Year.Text + "-" + Month.SelectedValue + "-1");
        tempMonth = DateTime.Parse(Year2.Text + "-" + Month2.SelectedValue + "-1");
        endMonth = tempMonth.AddMonths(1).AddDays(-1);

        int report_range_days = ((TimeSpan)(endMonth - startMonth)).Days + 1;

        if (report_range_days <= 0)
        {
            PageErrors errors = PageErrors.getErrors(db, Page.Master);
            errors.addError("Invalid Date Range Parameter: From-Date must be prior to To-Date");
            return;
        }

        peroid = "Period: " + startMonth.ToString("yyyy-MM-dd") + " to " + endMonth.ToString("yyyy-MM-dd");

        ArrayList list = WebUtils.SelectedRepeaterItemToBaseObjectList(db, Repeater, "ItemSelect");
        if (list.Count > 0)
        {
            string exportFileName = System.IO.Path.GetTempFileName();
            System.IO.File.Delete(exportFileName);
            exportFileName += ".xls";
            HROne.Export.ExcelExport export = new HROne.Export.ExcelExport(exportFileName);

            DataTable tmpDataTable = new DataTable(TABLE_NAME);
            tmpDataTable.Columns.Add(FIELD_COMPANY, typeof(string));
            tmpDataTable.Columns.Add(FIELD_DIVISION, typeof(string));
            tmpDataTable.Columns.Add(FIELD_DEPARTMENT, typeof(string));
            tmpDataTable.Columns.Add(FIELD_SECTION, typeof(string));
            tmpDataTable.Columns.Add(FIELD_EMP_NO, typeof(string));
            tmpDataTable.Columns.Add(FIELD_EMP_NAME, typeof(string));
            tmpDataTable.Columns.Add(FIELD_ALIAS, typeof(string));
            tmpDataTable.Columns.Add(FIELD_POSITION, typeof(string));
            tmpDataTable.Columns.Add(FIELD_FROM, typeof(DateTime));
            tmpDataTable.Columns.Add(FIELD_TO, typeof(DateTime));
            tmpDataTable.Columns.Add(FIELD_COST_CENTER, typeof(string));
            tmpDataTable.Columns.Add(FIELD_PERCENTAGE, typeof(double));

            foreach (EEmpPersonalInfo obj in list)
            {
                if (EEmpPersonalInfo.db.select(dbConn, obj))
                {
                    DBFilter filter = new DBFilter();
                    filter.add(new Match("EmpID", obj.EmpID));
                    ArrayList empCsts = EEmpCostCenter.db.select(dbConn, filter);

                    foreach (EEmpCostCenter empCostCenter in empCsts)
                    {
                        DBFilter empCstDetailFilter = new DBFilter();
                        empCstDetailFilter.add(new Match("EmpCostCenterID", empCostCenter.EmpCostCenterID));
                        ArrayList empCstDetailList = EEmpCostCenterDetail.db.select(dbConn, empCstDetailFilter);
                        foreach (EEmpCostCenterDetail detail in empCstDetailList)
                        {
                            DataRow row = tmpDataTable.NewRow();

                            EEmpPositionInfo empPos = AppUtils.GetLastPositionInfo(dbConn, AppUtils.ServerDateTime(), obj.EmpID);
                            if (empPos != null)
                            {
                                DBFilter positionFilter = new DBFilter();
                                positionFilter.add(new Match("PositionID", empPos.PositionID));
                                ArrayList positionList = EPosition.db.select(dbConn, positionFilter);
                                EPosition postion = (EPosition)positionList[0];
                                row[FIELD_POSITION] = postion.PositionDesc;

                                ECompany company = new ECompany();
                                company.CompanyID = empPos.CompanyID;
                                if (ECompany.db.select(dbConn, company))
                                    row[FIELD_COMPANY] = company.CompanyName;
                                DBFilter empHierarchyFilter = new DBFilter();
                                empHierarchyFilter.add(new Match("EmpPosID", empPos.EmpPosID));
                                ArrayList empHierarchyList = EEmpHierarchy.db.select(dbConn, empHierarchyFilter);
                                foreach (EEmpHierarchy empHierarchy in empHierarchyList)
                                {
                                    EHierarchyElement hierarchyElement = new EHierarchyElement();
                                    hierarchyElement.HElementID = empHierarchy.HElementID;
                                    if (EHierarchyElement.db.select(dbConn, hierarchyElement))
                                    {
                                        if (hierarchyElement.HLevelID == 1)
                                            row[FIELD_DEPARTMENT] = hierarchyElement.HElementDesc;
                                        else if (hierarchyElement.HLevelID == 2)
                                            row[FIELD_DIVISION] = hierarchyElement.HElementDesc;
                                        else if (hierarchyElement.HLevelID == 3)
                                            row[FIELD_SECTION] = hierarchyElement.HElementDesc;
                                    }
                                }
                            }

                            row[FIELD_EMP_NO] = obj.EmpNo;
                            row[FIELD_EMP_NAME] = obj.EmpEngFullName;
                            row[FIELD_ALIAS] = obj.EmpAlias;

                            //Fields From and To
                            row[FIELD_FROM] = empCostCenter.EmpCostCenterEffFr;
                            row[FIELD_TO] = empCostCenter.EmpCostCenterEffTo;

                            //Field Cost Center
                            ECostCenter costCenter = new ECostCenter();
                            costCenter.CostCenterID = detail.CostCenterID;
                            if (ECostCenter.db.select(dbConn, costCenter))
                            {
                                row[FIELD_COST_CENTER] = costCenter.CostCenterDesc;//costCenter.CostCenterCode
                            }

                            //Field average percentage
                            TimeSpan ts;
                            if ((empCostCenter.EmpCostCenterEffTo <= DateTime.Parse("0001-01-01")) && empCostCenter.EmpCostCenterEffFr < startMonth) // check EmpCostCenterEffTo is null and EmpCostCenterEffFr less than Date-Range-From
                            {
                                row[FIELD_FROM] = startMonth;
                                row[FIELD_TO] = endMonth;
                                ts = endMonth - startMonth;
                            }
                            else if ((empCostCenter.EmpCostCenterEffTo <= DateTime.Parse("0001-01-01"))) // check EmpCostCenterEffTo is null, use Date-Range-To value
                            {
                                row[FIELD_TO] = endMonth;
                                ts = endMonth - empCostCenter.EmpCostCenterEffFr;
                            }
                            else if (empCostCenter.EmpCostCenterEffFr < startMonth) // check EmpCostCenterEffFr less than Date-Range-From, use Date-Range-From value
                            {
                                row[FIELD_FROM] = startMonth;
                                ts = empCostCenter.EmpCostCenterEffTo - startMonth;
                            }
                            else
                            {
                                ts = empCostCenter.EmpCostCenterEffTo - empCostCenter.EmpCostCenterEffFr;
                            }
                            double day = ts.Days + 1;
                            row[FIELD_PERCENTAGE] = detail.EmpCostCenterPercentage * (day / report_range_days);

                            //add row data to data table
                            tmpDataTable.Rows.Add(row);
                        }
                    }
                }
            }
            GenerateExcelReport(tmpDataTable, exportFileName);

            WebUtils.TransmitFile(Response, exportFileName, "AverageCostCenterExportReport_" + AppUtils.ServerDateTime().ToString("yyyyMMddHHmmss") + ".xls", true);
            Response.End();
        }
        else
        {
            PageErrors errors = PageErrors.getErrors(db, Page.Master);
            errors.addError("Employee not selected");
        }

        //view = loadData(info, db, Repeater);
    }

    private void GenerateExcelReport(DataTable tmpDataTable, string exportFileName)
    {
        int columnCount = 0;
        int lastRowIndex = 0;

        // Set column style
        NPOI.HSSF.UserModel.HSSFWorkbook workbook = new NPOI.HSSF.UserModel.HSSFWorkbook();
        NPOI.HSSF.UserModel.HSSFDataFormat format = (NPOI.HSSF.UserModel.HSSFDataFormat)workbook.CreateDataFormat();
        NPOI.HSSF.UserModel.HSSFCellStyle dateCellStyle = (NPOI.HSSF.UserModel.HSSFCellStyle)workbook.CreateCellStyle();
        dateCellStyle.DataFormat = format.GetFormat("yyyy-MM-dd");
        dateCellStyle.Alignment = NPOI.SS.UserModel.HorizontalAlignment.LEFT;
        NPOI.HSSF.UserModel.HSSFSheet worksheet = (NPOI.HSSF.UserModel.HSSFSheet)workbook.CreateSheet("AverageCostCentreExport");
        NPOI.HSSF.UserModel.HSSFCellStyle numericStyle = (NPOI.HSSF.UserModel.HSSFCellStyle)workbook.CreateCellStyle();
        numericStyle.DataFormat = NPOI.HSSF.UserModel.HSSFDataFormat.GetBuiltinFormat("#,##0.00");
        numericStyle.Alignment = NPOI.SS.UserModel.HorizontalAlignment.RIGHT;
        NPOI.HSSF.UserModel.HSSFCellStyle style = (NPOI.HSSF.UserModel.HSSFCellStyle)workbook.CreateCellStyle();
        style.Alignment = NPOI.SS.UserModel.HorizontalAlignment.RIGHT;

        // Set column width
        worksheet.SetColumnWidth(0, 40 * 256);
        worksheet.SetColumnWidth(2, 20 * 256);
        worksheet.SetColumnWidth(5, 15 * 256);
        worksheet.SetColumnWidth(7, 15 * 256);
        worksheet.SetColumnWidth(8, 15 * 256);
        worksheet.SetColumnWidth(9, 15 * 256);
        worksheet.SetColumnWidth(10, 12 * 256);

        // Set column title
        NPOI.HSSF.UserModel.HSSFRow headerRow = (NPOI.HSSF.UserModel.HSSFRow)worksheet.CreateRow(lastRowIndex);
        headerRow.CreateCell(0).SetCellValue("Average Cost Centre Export");
        headerRow = (NPOI.HSSF.UserModel.HSSFRow)worksheet.CreateRow(lastRowIndex + 1);
        headerRow.CreateCell(0).SetCellValue(peroid);
        headerRow = (NPOI.HSSF.UserModel.HSSFRow)worksheet.CreateRow(lastRowIndex + 3);

        foreach (DataColumn headercolumn in tmpDataTable.Columns)
        {
            NPOI.HSSF.UserModel.HSSFCell cell = (NPOI.HSSF.UserModel.HSSFCell)headerRow.CreateCell(columnCount);
            cell.SetCellValue(headercolumn.ColumnName);
            if (columnCount == 11)
            {
                cell.CellStyle = style;
            }
            columnCount++;
        }

        // Set value for every row
        foreach (DataRow row in tmpDataTable.Rows)
        {
            NPOI.HSSF.UserModel.HSSFRow detailRow = (NPOI.HSSF.UserModel.HSSFRow)worksheet.CreateRow(lastRowIndex + 4);

            detailRow.CreateCell(0).SetCellValue(row[FIELD_COMPANY].ToString());
            detailRow.CreateCell(1).SetCellValue(row[FIELD_DIVISION].ToString());
            detailRow.CreateCell(2).SetCellValue(row[FIELD_DEPARTMENT].ToString());
            detailRow.CreateCell(3).SetCellValue(row[FIELD_SECTION].ToString());
            detailRow.CreateCell(4).SetCellValue(row[FIELD_EMP_NO].ToString());
            detailRow.CreateCell(5).SetCellValue(row[FIELD_EMP_NAME].ToString());
            detailRow.CreateCell(6).SetCellValue(row[FIELD_ALIAS].ToString());
            detailRow.CreateCell(7).SetCellValue(row[FIELD_POSITION].ToString());

            NPOI.HSSF.UserModel.HSSFCell cell = (NPOI.HSSF.UserModel.HSSFCell)detailRow.CreateCell(8);
            cell.SetCellValue((DateTime)row[FIELD_FROM]);
            cell.CellStyle = dateCellStyle;

            cell = (NPOI.HSSF.UserModel.HSSFCell)detailRow.CreateCell(9);
            cell.SetCellValue((DateTime)row[FIELD_TO]);
            cell.CellStyle = dateCellStyle;

            detailRow.CreateCell(10).SetCellValue(row[FIELD_COST_CENTER].ToString());
            cell = (NPOI.HSSF.UserModel.HSSFCell)detailRow.CreateCell(11);
            cell.SetCellValue((double)row[FIELD_PERCENTAGE]);
            cell.CellStyle = numericStyle;

            lastRowIndex++;
        }

        System.IO.FileStream file = new System.IO.FileStream(exportFileName, System.IO.FileMode.Create);
        workbook.Write(file);
        file.Close();
    }
}
