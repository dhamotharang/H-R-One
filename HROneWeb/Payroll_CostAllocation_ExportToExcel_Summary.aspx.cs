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

public partial class Payroll_CostAllocation_ExportToExcel_Summary : HROneWebPage
{
    private const string FUNCTION_CODE = "CST005";

    public Binding binding;

    protected SearchBinding sbinding;
    public DBManager db = ECostAllocation.db;
    public EPayrollGroup obj;
    //public int CurID = -1;
    //public int CurPayPeriodID = -1;

    protected ListInfo info;
    protected DataView view;

    private bool IsAllowEdit = true;

    const int FIELD_HEADER_ROW = 2;
    const int PAYROLL_GRUOP_COLUMN = 0;
    const int COST_CENTER_COLUMN = 1;
    const int HIERARCHY_COLUMN = 2;
    const int PAYMENTDETAIL_START_COLUMN = HIERARCHY_COLUMN + 1;


    protected void Page_Load(object sender, EventArgs e)
    {

        if (!WebUtils.CheckAccess(Response, Session, FUNCTION_CODE, WebUtils.AccessLevel.Read))
            return;
        if (!WebUtils.CheckPermission(Session, FUNCTION_CODE, WebUtils.AccessLevel.ReadWrite))
        {
            IsAllowEdit = false;
            ConfirmPayrollSelectAllPanel.Visible = false;
        }
        if (!Page.IsPostBack)
        {
            if (DecryptedRequest["CostAllocationStatus"] != null)
            {
                try
                {
                    CostAllocationStatus.SelectedValue = DecryptedRequest["CostAllocationStatus"];
                }
                catch { }
            }
        }
        binding = new Binding(dbConn, db);
        //binding.add(new DropDownVLBinder(EPayrollGroup.db, PayGroupID, EPayrollGroup.VLPayrollGroup));
        //binding.add(CurrentPayPeriodID);

        //DBFilter payPeriodFilter = new DBFilter();
        //if (DecryptedRequest["PayGroupID"] != null)
        //    payPeriodFilter.add(new Match("PayGroupID", DecryptedRequest["PayGroupID"]));
        //else
        //    payPeriodFilter.add(new Match("PayGroupID", 0));
        //payPeriodFilter.add("PayPeriodFr", false);

        //DBFilter costAllocationFilter = new DBFilter();
        //costAllocationFilter.add(new Match("CostAllocationStatus", CostAllocationStatus.SelectedValue));

        //DBFilter empPayrollFilter = new DBFilter();
        //empPayrollFilter.add(new Match("EmpPayStatus", "C"));
        //empPayrollFilter.add(new IN("EmpPayrollID", "Select EmpPayrollID from " + ECostAllocation.db.dbclass.tableName, costAllocationFilter));


        //payPeriodFilter.add(new IN("PayPeriodID", "Select PayPeriodID from " + EEmpPayroll.db.dbclass.tableName, empPayrollFilter));


        //binding.add(new DropDownVLBinder(EPayrollPeriod.db, PayPeriodID, EPayrollPeriod.VLPayrollPeriod, payPeriodFilter));

        binding.init(Request, Session);

        sbinding = new SearchBinding(dbConn, EEmpPersonalInfo.db);
        sbinding.add(new DropDownVLSearchBinder(Year, "Year(pp.PayPeriodFr)", EPayrollPeriod.VLPayrollPeriodYear, false));//, null, "Year(pp.PayPeriodFR)"));
        sbinding.add(new DropDownVLSearchBinder(Month, "Month(pp.PayPeriodFr)", Values.VLMonth, false));
        sbinding.init(DecryptedRequest, null);
        //try
        //{
        //    //CurID = Int32.Parse(DecryptedRequest["PayGroupID"]);
        //    //if (!string.IsNullOrEmpty(DecryptedRequest["PayPeriodID"]))
        //    //    CurPayPeriodID = Int32.Parse(DecryptedRequest["PayPeriodID"]);
        //    //else if (!Int32.TryParse(PayPeriodID.SelectedValue, out CurPayPeriodID))
        //    //{
        //    //    EPayrollGroup obj = new EPayrollGroup();
        //    //    obj.PayGroupID = CurID;
        //    //    if (!db.select(dbConn, obj))
        //    //        CurPayPeriodID = obj.CurrentPayPeriodID;
        //    //}
        //}
        //catch (Exception ex)
        //{
        //}
        info = ListFooter.ListInfo;

        HROne.Common.WebUtility.WebControlsLocalization(this, this.Controls);
    }
    protected void Page_PreRender(object sender, EventArgs e)
    {

        //if (!Page.IsPostBack)
        {
            //if (CurID > 0)
            //{
            panelPayPeriod.Visible = true;
            //loadObject();
            view = loadData(info, db, Repeater);
            //}
            //else
            //panelPayPeriod.Visible = false;

        }

    }

    //protected bool loadObject()
    //{
    //    obj = new EPayrollGroup();
    //    bool isNew = WebFormWorkers.loadKeys(EPayrollGroup.db, obj, DecryptedRequest);
    //    if (!EPayrollGroup.db.select(dbConn, obj))
    //        return false;

    //    Hashtable values = new Hashtable();
    //    EPayrollGroup.db.populate(obj, values);
    //    binding.toControl(values);

    //    if (CurPayPeriodID <= 0)
    //    {
    //        CurPayPeriodID = obj.CurrentPayPeriodID;
    //    }
    //    try
    //    {
    //        PayPeriodID.SelectedValue = CurPayPeriodID.ToString();
    //        ucPayroll_PeriodInfo.CurrentPayPeriodID = CurPayPeriodID;
    //    }
    //    catch (Exception ex)
    //    {
    //        CurPayPeriodID = 0;
    //    }
    //    if (PayPeriodID.SelectedIndex == 0)
    //    {
    //        CurPayPeriodID = 0;
    //        ucPayroll_PeriodInfo.CurrentPayPeriodID = 0;
    //    }
    //    if (CurPayPeriodID > 0)
    //        panelCostAllocationAdjustmentDetail.Visible = true;
    //    else
    //        panelCostAllocationAdjustmentDetail.Visible = false;


    //    return true;
    //}

    //protected void CostAllocationStatus_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    Response.Redirect(Request.Url.LocalPath + "?PayGroupID=" + PayGroupID.SelectedValue + "&CostAllocationStatus=" + CostAllocationStatus.SelectedValue);
    //}
    //protected void PayGroupID_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    Response.Redirect(Request.Url.LocalPath + "?PayGroupID=" + PayGroupID.SelectedValue + "&CostAllocationStatus=" + CostAllocationStatus.SelectedValue);
    //}
    //protected void PayPeriodID_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    Response.Redirect(Request.Url.LocalPath + "?PayGroupID=" + PayGroupID.SelectedValue + "&PayPeriodID=" + PayPeriodID.SelectedValue + "&CostAllocationStatus=" + CostAllocationStatus.SelectedValue);
    //}


    public DataView loadData(ListInfo info, DBManager db, Repeater repeater)
    {
        DBFilter filter = sbinding.createFilter();

        //if (info != null && info.orderby != null && !info.orderby.Equals(""))
        //    filter.add(info.orderby, info.order);

        string select = " e.*, ca.CostAllocationID ";
        string from = "from " + EEmpPersonalInfo.db.dbclass.tableName + " e, " + ECostAllocation.db.dbclass.tableName + " ca, " + EEmpPayroll.db.dbclass.tableName + " ep, " + EPayrollPeriod.db.dbclass.tableName + " pp";

        filter.add(new MatchField("e.EmpID", "ca.EmpID"));
        filter.add(new MatchField("ca.EmpPayrollID", "ep.EmpPayrollID"));
        filter.add(new MatchField("ep.PayPeriodID", "pp.PayPeriodID"));
        filter.add(new Match("ca.CostAllocationStatus", CostAllocationStatus.SelectedValue));
        //filter.add(new Match("Year(pp.PayPeriodFr)", Year.SelectedValue));
        //filter.add(new Match("Year(pp.PayPeriodTo)", Month.SelectedValue));

        //DBFilter empPayrollFilter = new DBFilter();
        //empPayrollFilter.add(new Match("ep.EmpPayStatus", "C"));
        //empPayrollFilter.add(new Match("ep.PayPeriodID", CurPayPeriodID));

        // Start 0000129, Ricky So, 2014/11/11
        filter.add(WebUtils.AddRankFilter(Session, "e.EmpID", true));
        // End 0000129, Ricky So, 2014/11/11

        //DBFilter inCostAllocationFilter = new DBFilter();
        //filter.add(new IN("ca.EmpPayrollID", "Select EmpPayrollID from " + EEmpPayroll.db.dbclass.tableName + " ep", empPayrollFilter));

        DataTable table = WebUtils.GetDataTableFromSelectQueryWithFilter(dbConn, select, from, filter, info);


        view = new DataView(table);

        ListFooter.Refresh();

        if (repeater != null)
        {
            repeater.DataSource = view;
            repeater.DataBind();
        }

        return view;
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

        view = loadData(info, EEmpPayroll.db, Repeater);

    }
    protected void Repeater_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        DataRowView row = (DataRowView)e.Item.DataItem;
        CheckBox cb = (CheckBox)e.Item.FindControl("ItemSelect");
        WebFormUtils.LoadKeys(db, row, cb);
        cb.Visible = IsAllowEdit;
    }

    protected void btnUndo_Click(object sender, EventArgs e)
    {

    }
    protected void btnExport_Click(object sender, EventArgs e)
    {
        ArrayList list = WebUtils.SelectedRepeaterItemToBaseObjectList(ECostAllocation.db, Repeater, "ItemSelect");
        int GroupingHierarchyLevelID = 1;
        string GroupingHierarchyLevelDesc = string.Empty;

        if (list.Count > 0)
        {
            string exportFileName = System.IO.Path.GetTempFileName();
            System.IO.File.Delete(exportFileName);
            exportFileName += ".xls";
            //System.IO.File.Copy(Server.MapPath("~/template/HistoryList_Template.xls"), exportFileName, true);
            HROne.Export.ExcelExport export = new HROne.Export.ExcelExport(exportFileName);
            DataSet dataSet = new DataSet();//export.GetDataSet();

            CreateSummaryDataTable(dataSet);

            DBFilter hierarchyLevelFilter = new DBFilter();
            Hashtable hierarchyLevelHashTable = new Hashtable();
            hierarchyLevelFilter.add("HLevelSeqNo", true);
            ArrayList hierarchyLevelList = EHierarchyLevel.db.select(dbConn, hierarchyLevelFilter);
            foreach (EHierarchyLevel hlevel in hierarchyLevelList)
            {
                if (string.IsNullOrEmpty(GroupingHierarchyLevelDesc))
                {
                    GroupingHierarchyLevelID = hlevel.HLevelID;
                    GroupingHierarchyLevelDesc = hlevel.HLevelDesc.Trim();
                }
                hierarchyLevelHashTable.Add(hlevel.HLevelID, hlevel);
            }

            foreach (ECostAllocation obj in list)
            {


                if (ECostAllocation.db.select(dbConn, obj))
                {

                    EEmpPayroll empPayroll = new EEmpPayroll();
                    empPayroll.EmpPayrollID = obj.EmpPayrollID;
                    EEmpPayroll.db.select(dbConn, empPayroll);

                    EPayrollPeriod payPeriod = new EPayrollPeriod();
                    payPeriod.PayPeriodID = empPayroll.PayPeriodID;
                    EPayrollPeriod.db.select(dbConn, payPeriod);

                    EPayrollGroup payGroup = new EPayrollGroup();
                    payGroup.PayGroupID = payPeriod.PayGroupID;
                    EPayrollGroup.db.select(dbConn, payGroup);

                    EEmpPersonalInfo empInfo = new EEmpPersonalInfo();
                    empInfo.EmpID = obj.EmpID;
                    EEmpPersonalInfo.db.select(dbConn, empInfo);



                    DBFilter costAllocationDetailFilter = new DBFilter();
                    costAllocationDetailFilter.add(new Match("CostAllocationID", obj.CostAllocationID));

                    ArrayList costAllocationDetailList = ECostAllocationDetail.db.select(dbConn, costAllocationDetailFilter);

                    foreach (ECostAllocationDetail detail in costAllocationDetailList)
                    {
                        ECompany company = new ECompany();
                        company.CompanyID = detail.CompanyID;
                        ECompany.db.select(dbConn, company);

                        ECostCenter costCenter = new ECostCenter();
                        costCenter.CostCenterID = detail.CostCenterID;
                        ECostCenter.db.select(dbConn, costCenter);

                        int HElementID = 0;
                        string hElementDesc = string.Empty;

                        DBFilter costAllocationDetailHierarchyFilter = new DBFilter();
                        costAllocationDetailHierarchyFilter.add(new Match("CostAllocationDetailID", detail.CostAllocationDetailID));


                        ArrayList empHierarchyList = ECostAllocationDetailHElement.db.select(dbConn, costAllocationDetailHierarchyFilter);
                        foreach (ECostAllocationDetailHElement empHierarchy in empHierarchyList)
                        {
                            EHierarchyLevel hierarchyLevel = (EHierarchyLevel)hierarchyLevelHashTable[empHierarchy.HLevelID];
                            if (hierarchyLevel != null)
                            {
                                EHierarchyElement hierarchyElement = new EHierarchyElement();
                                hierarchyElement.HElementID = empHierarchy.HElementID;
                                if (EHierarchyElement.db.select(dbConn, hierarchyElement))
                                {

                                    //  Select first hierarchy for testing
                                    if (hierarchyLevel.HLevelDesc.Equals(GroupingHierarchyLevelDesc.Trim()) && HElementID == 0)
                                    {
                                        HElementID = hierarchyElement.HElementID;
                                        hElementDesc = hierarchyElement.HElementDesc;
                                    }
                                }
                            }
                        }

                        DataTable hierarchyTable = dataSet.Tables["hierarchy"];
                        DataTable paymentTable = dataSet.Tables["payment"];
                        DataTable contributionTable = dataSet.Tables["contribution"];
                        DataRow[] rows = hierarchyTable.Select("CompanyID=" + company.CompanyID + " and PayGroupID=" + payGroup.PayGroupID + " and HElementID=" + HElementID + " and CostCenterID=" + detail.CostCenterID);
                        int hierarchyRowID = 0;
                        if (rows.Length == 0)
                        {
                            hierarchyRowID = hierarchyTable.Rows.Count + 1;
                            DataRow hierarchyRow = hierarchyTable.NewRow();
                            hierarchyRow["ID"] = hierarchyRowID;
                            hierarchyRow["Company"] = company.CompanyName;
                            hierarchyRow["PayrollGroupDesc"] = payGroup.PayGroupDesc;
                            hierarchyRow["HierarchyDesc"] = hElementDesc;
                            hierarchyRow["CostCenterDesc"] = costCenter.CostCenterDesc;
                            hierarchyRow["CompanyID"] = company.CompanyID;
                            hierarchyRow["hElementID"] = HElementID;
                            hierarchyRow["CostCenterID"] = costCenter.CostCenterID;
                            hierarchyRow["PayGroupID"] = payGroup.PayGroupID;
                            hierarchyTable.Rows.Add(hierarchyRow);

                        }
                        else
                            hierarchyRowID = (int)rows[0]["ID"];


                        EPaymentCode paymentCode = new EPaymentCode();
                        paymentCode.PaymentCodeID = detail.PaymentCodeID;
                        EPaymentCode.db.select(dbConn, paymentCode);

                        if (!detail.CostAllocationDetailIsContribution)
                        {
                            if (!paymentTable.Columns.Contains(paymentCode.PaymentCodeDesc.Trim()))
                            {
                                paymentTable.Columns.Add(paymentCode.PaymentCodeDesc.Trim(), typeof(double));
                            }

                            rows = paymentTable.Select("ID=" + hierarchyRowID);
                            DataRow paymentRow;
                            if (rows.Length == 0)
                            {
                                paymentRow = paymentTable.NewRow();
                                paymentRow["ID"] = hierarchyRowID;
                                paymentTable.Rows.Add(paymentRow);

                            }
                            else
                                paymentRow = rows[0];
                            if (paymentRow.IsNull(paymentCode.PaymentCodeDesc.Trim()))
                                paymentRow[paymentCode.PaymentCodeDesc.Trim()] = detail.CostAllocationDetailAmount;
                            else
                                paymentRow[paymentCode.PaymentCodeDesc.Trim()] = (double)paymentRow[paymentCode.PaymentCodeDesc.Trim()] + detail.CostAllocationDetailAmount;
                        }
                        else
                        {
                            rows = contributionTable.Select("ID=" + hierarchyRowID);
                            DataRow contributionRow;
                            if (rows.Length == 0)
                            {
                                contributionRow = contributionTable.NewRow();
                                contributionRow["ID"] = hierarchyRowID;
                                contributionRow["MCEE"] = 0;
                                contributionRow["MCER"] = 0;
                                contributionRow["VCEE"] = 0;
                                contributionRow["VCER"] = 0;
                                contributionRow["PFUNDEE"] = 0;
                                contributionRow["PFUNDER"] = 0;
                                contributionTable.Rows.Add(contributionRow);

                            }
                            else
                                contributionRow = rows[0];
                            if (paymentCode.PaymentTypeID.Equals(EPaymentType.SystemPaymentType.MPFEmployeeMandatoryContributionPaymentType(dbConn).PaymentTypeID))
                                contributionRow["MCEE"] = (double)contributionRow["MCEE"] + detail.CostAllocationDetailAmount;
                            else if (paymentCode.PaymentTypeID.Equals(EPaymentType.SystemPaymentType.MPFEmployeeVoluntaryContributionPaymentType(dbConn).PaymentTypeID))
                                contributionRow["VCEE"] = (double)contributionRow["VCEE"] + detail.CostAllocationDetailAmount;
                            else if (paymentCode.PaymentTypeID.Equals(EPaymentType.SystemPaymentType.MPFEmployerMandatoryContributionPaymentType(dbConn).PaymentTypeID))
                                contributionRow["MCER"] = (double)contributionRow["MCER"] + detail.CostAllocationDetailAmount;
                            else if (paymentCode.PaymentTypeID.Equals(EPaymentType.SystemPaymentType.MPFEmployerVoluntaryContributionPaymentType(dbConn).PaymentTypeID))
                                contributionRow["VCER"] = (double)contributionRow["VCER"] + detail.CostAllocationDetailAmount;
                            else if (paymentCode.PaymentTypeID.Equals(EPaymentType.SystemPaymentType.PFundEmployeeContributionPaymentType(dbConn).PaymentTypeID))
                                contributionRow["PFUNDEE"] = (double)contributionRow["PFUNDEE"] + detail.CostAllocationDetailAmount;
                            else if (paymentCode.PaymentTypeID.Equals(EPaymentType.SystemPaymentType.PFundEmployerContributionPaymentType(dbConn).PaymentTypeID))
                                contributionRow["PFUNDER"] = (double)contributionRow["PFUNDER"] + detail.CostAllocationDetailAmount;

                        }
                    }
                }

            }
            GenerateExcelReport(dataSet, exportFileName);
            //export.Update(dataSet);
            WebUtils.TransmitFile(Response, exportFileName, "CostALlocation" + (CostAllocationStatus.SelectedValue.Equals("T") ? "Trial" : "Confirm") + "_" + AppUtils.ServerDateTime().ToString("yyyyMMddHHmmss") + ".xls", true);
            Response.End();
        }

        else
        {
            PageErrors errors = PageErrors.getErrors(db, Page.Master);
            errors.addError("Employee not selected");
        }

        view = loadData(info, EEmpPayroll.db, Repeater);

    }

    private void GenerateExcelReport(DataSet dataSet, string exportFileName)
    {

        //ExcelLibrary.SpreadSheet.Worksheet execlWorksheet = null;

        DataView tmpView = new DataView(dataSet.Tables["Hierarchy"]);
        tmpView.Sort = "CompanyID, PayGroupID, HElementID Desc,CostCenterID";
        DataTable sortedHierarchyTable = tmpView.ToTable();

        int curringCompanyID = 0;
        string currentCompany = string.Empty;
        int recordCount = 0;
        int lastRowIndex = 0, lastColumnIndex = 0;
        //ExcelLibrary.SpreadSheet.Workbook excelWorkbook = new ExcelLibrary.SpreadSheet.Workbook();
        //execlWorksheet = new ExcelLibrary.SpreadSheet.Worksheet("CostAllocation");
        //excelWorkbook.Worksheets.Add(execlWorksheet);
        NPOI.HSSF.UserModel.HSSFWorkbook excelWorkbook = new NPOI.HSSF.UserModel.HSSFWorkbook();
        NPOI.HSSF.UserModel.HSSFSheet excelWorksheet = (NPOI.HSSF.UserModel.HSSFSheet)excelWorkbook.CreateSheet("CostAllocation");

        NPOI.HSSF.UserModel.HSSFRow headerRow = null;

        NPOI.HSSF.UserModel.HSSFFont headerFont = (NPOI.HSSF.UserModel.HSSFFont)excelWorkbook.CreateFont();
        headerFont.Boldweight = 1;
        NPOI.HSSF.UserModel.HSSFCellStyle headerStyle = (NPOI.HSSF.UserModel.HSSFCellStyle)excelWorkbook.CreateCellStyle();
        headerStyle.SetFont(headerFont);
        //headerStyle.BorderBottom = NPOI.SS.UserModel.CellBorderType.THICK;

        NPOI.HSSF.UserModel.HSSFCellStyle numericStyle = (NPOI.HSSF.UserModel.HSSFCellStyle)excelWorkbook.CreateCellStyle();
        numericStyle.DataFormat = NPOI.HSSF.UserModel.HSSFDataFormat.GetBuiltinFormat("#,##0.00");

        NPOI.HSSF.UserModel.HSSFFont subTotalFont = (NPOI.HSSF.UserModel.HSSFFont)excelWorkbook.CreateFont();
        subTotalFont.Boldweight = 1;
        NPOI.HSSF.UserModel.HSSFCellStyle subTotalStyle = (NPOI.HSSF.UserModel.HSSFCellStyle)excelWorkbook.CreateCellStyle();
        subTotalStyle.SetFont(subTotalFont);
        //subTotalStyle.BorderTop = NPOI.SS.UserModel.CellBorderType.THICK;
        subTotalStyle.DataFormat = NPOI.HSSF.UserModel.HSSFDataFormat.GetBuiltinFormat("#,##0.00");

        foreach (DataRow row in sortedHierarchyTable.Rows)
        {
            if (curringCompanyID != (int)row["CompanyID"])
            {
                if (recordCount > 0)
                {
                    lastRowIndex++;

                    AddSubTotalRow(excelWorksheet, currentCompany, recordCount, lastRowIndex, lastColumnIndex, headerRow, subTotalStyle);
                    recordCount = 0;

                    lastRowIndex++;
                }
                //else
                {
                    //execlWorksheet.Cells[lastRowIndex, 0].Value = row["Company"].ToString();
                    //lastRowIndex += FIELD_HEADER_ROW;
                    //execlWorksheet.Cells[lastRowIndex, PAYROLL_GRUOP_COLUMN].Value = "Payroll Group";
                    //execlWorksheet.Cells[lastRowIndex, COST_CENTER_COLUMN].Value = "Cost Ctr";
                    //execlWorksheet.Cells[lastRowIndex, HIERARCHY_COLUMN].Value = "Hierarchy";
                    NPOI.HSSF.UserModel.HSSFRow companyRow = (NPOI.HSSF.UserModel.HSSFRow)excelWorksheet.CreateRow(lastRowIndex);
                    companyRow.CreateCell(0).SetCellValue(row["Company"].ToString());
                    lastRowIndex += FIELD_HEADER_ROW;

                    headerRow = (NPOI.HSSF.UserModel.HSSFRow)excelWorksheet.CreateRow(lastRowIndex);
                    headerRow.CreateCell(PAYROLL_GRUOP_COLUMN).SetCellValue("Payroll Group");
                    headerRow.CreateCell(COST_CENTER_COLUMN).SetCellValue("Cost Ctr");
                    headerRow.CreateCell(HIERARCHY_COLUMN).SetCellValue("Hierarchy");
                }
                curringCompanyID = (int)row["CompanyID"];
                currentCompany = row["Company"].ToString();
            }
            lastRowIndex++;

            recordCount++;
            //lastRowIndex = FIELD_HEADER_ROW + recordCount;

            //execlWorksheet.Cells[lastRowIndex, PAYROLL_GRUOP_COLUMN].Value = row["PayrollGroupDesc"];
            //execlWorksheet.Cells[lastRowIndex, COST_CENTER_COLUMN].Value = row["CostCenterDesc"];
            //execlWorksheet.Cells[lastRowIndex, HIERARCHY_COLUMN].Value = row["HierarchyDesc"];
            NPOI.HSSF.UserModel.HSSFRow detailRow = (NPOI.HSSF.UserModel.HSSFRow)excelWorksheet.CreateRow(lastRowIndex);
            detailRow.CreateCell(PAYROLL_GRUOP_COLUMN).SetCellValue(row["PayrollGroupDesc"].ToString());
            detailRow.CreateCell(COST_CENTER_COLUMN).SetCellValue(row["CostCenterDesc"].ToString());
            detailRow.CreateCell(HIERARCHY_COLUMN).SetCellValue(row["HierarchyDesc"].ToString());

            lastColumnIndex = PAYMENTDETAIL_START_COLUMN - 1;
            DataRow[] paymentRows = dataSet.Tables["Payment"].Select("ID=" + row["ID"]);
            if (paymentRows.Length > 0)
            {
                //double netPayment = 0;
                DataRow paymentRow = paymentRows[0];
                foreach (DataColumn column in paymentRow.Table.Columns)
                {
                    if (!column.ColumnName.Equals("ID", StringComparison.CurrentCultureIgnoreCase))
                    {
                        lastColumnIndex++;
                        if (recordCount == 1)
                            //execlWorksheet.Cells[FIELD_HEADER_ROW, lastColumnIndex].Value = column.ColumnName;
                            headerRow.CreateCell(lastColumnIndex).SetCellValue(column.ColumnName);
                        if (!paymentRow.IsNull(column))
                        {
                            //ExcelLibrary.SpreadSheet.Cell cell = execlWorksheet.Cells[lastRowIndex, lastColumnIndex];
                            //cell.Value = HROne.CommonLib.GenericRoundingFunctions.RoundingTo((double)paymentRow[column], 2, 2);
                            //cell.FormatString = "#,##0.00";
                            //netPayment += HROne.CommonLib.GenericRoundingFunctions.RoundingTo((double)paymentRow[column], 2, 2);
                            NPOI.HSSF.UserModel.HSSFCell dataCell = (NPOI.HSSF.UserModel.HSSFCell)detailRow.CreateCell(lastColumnIndex);
                            dataCell.CellStyle = numericStyle;
                            dataCell.SetCellValue(HROne.CommonLib.GenericRoundingFunctions.RoundingTo((double)paymentRow[column], 2, 2));
                        }
                    }
                }
                lastColumnIndex++;
                if (recordCount == 1)
                    //execlWorksheet.Cells[FIELD_HEADER_ROW, lastColumnIndex].Value = "NET PAYMENT";
                    headerRow.CreateCell(lastColumnIndex).SetCellValue("NET PAYMENT");
                //ExcelLibrary.SpreadSheet.Cell netPaymentCell = execlWorksheet.Cells[lastRowIndex, lastColumnIndex];
                //netPaymentCell.FormatString = "#,##0.00";
                //netPaymentCell.Value = HROne.CommonLib.GenericRoundingFunctions.RoundingTo(netPayment, 2, 2);
                NPOI.HSSF.UserModel.HSSFCell netPaymentCell = (NPOI.HSSF.UserModel.HSSFCell)detailRow.CreateCell(lastColumnIndex);
                netPaymentCell.CellStyle = numericStyle;
                netPaymentCell.CellFormula = "SUM(" + ToCellString(lastRowIndex, PAYMENTDETAIL_START_COLUMN) + ":" + ToCellString(lastRowIndex, lastColumnIndex - 1) + ")";
            }
            lastColumnIndex++;
            DataRow[] contributionRows = dataSet.Tables["contribution"].Select("ID=" + row["ID"]);
            if (contributionRows.Length > 0)
            {
                DataRow contributionRow = contributionRows[0];
                if (recordCount == 1)
                {
                    //execlWorksheet.Cells[FIELD_HEADER_ROW, lastColumnIndex + 1].Value = "MPF (ER)";
                    //execlWorksheet.Cells[FIELD_HEADER_ROW, lastColumnIndex + 2].Value = "VC (ER)";
                    //execlWorksheet.Cells[FIELD_HEADER_ROW, lastColumnIndex + 3].Value = "P-FUND (ER)";
                    //execlWorksheet.Cells[FIELD_HEADER_ROW, lastColumnIndex + 4].Value = "Total Contribution (ER)";
                    //execlWorksheet.Cells[FIELD_HEADER_ROW, lastColumnIndex + 6].Value = "Total Contribution (EE)";
                    headerRow.CreateCell(lastColumnIndex + 1).SetCellValue("MPF (ER)");
                    headerRow.CreateCell(lastColumnIndex + 2).SetCellValue("VC (ER)");
                    headerRow.CreateCell(lastColumnIndex + 3).SetCellValue("P-FUND (ER)");
                    headerRow.CreateCell(lastColumnIndex + 4).SetCellValue("Total Contribution (ER)");
                    headerRow.CreateCell(lastColumnIndex + 6).SetCellValue("Total Contribution (EE)");
                }
                //ExcelLibrary.SpreadSheet.Cell cell=execlWorksheet.Cells[lastRowIndex, lastColumnIndex + 1];
                //cell.FormatString = "#,##0.00";
                //cell.Value = HROne.CommonLib.GenericRoundingFunctions.RoundingTo((double)contributionRow["MCER"], 2, 2);

                //cell = execlWorksheet.Cells[lastRowIndex, lastColumnIndex + 2];
                //cell.FormatString = "#,##0.00";
                //cell.Value = HROne.CommonLib.GenericRoundingFunctions.RoundingTo((double)contributionRow["VCER"], 2, 2);

                //cell = execlWorksheet.Cells[lastRowIndex, lastColumnIndex + 3];
                //cell.FormatString = "#,##0.00";
                //cell.Value = HROne.CommonLib.GenericRoundingFunctions.RoundingTo((double)contributionRow["PFUNDER"], 2, 2);

                //cell = execlWorksheet.Cells[lastRowIndex, lastColumnIndex + 4];
                //cell.FormatString = "#,##0.00";
                //cell.Value = HROne.CommonLib.GenericRoundingFunctions.RoundingTo((double)contributionRow["PFUNDER"] + (double)contributionRow["MCER"] + (double)contributionRow["VCER"], 2, 2);

                //cell = execlWorksheet.Cells[lastRowIndex, lastColumnIndex + 6];
                //cell.FormatString = "#,##0.00";
                //cell.Value = HROne.CommonLib.GenericRoundingFunctions.RoundingTo((double)contributionRow["PFUNDEE"] + (double)contributionRow["MCEE"] + (double)contributionRow["VCEE"], 2, 2);
                NPOI.HSSF.UserModel.HSSFCell cell = (NPOI.HSSF.UserModel.HSSFCell)detailRow.CreateCell(lastColumnIndex + 1);
                cell.CellStyle = numericStyle;
                cell.SetCellValue(HROne.CommonLib.GenericRoundingFunctions.RoundingTo((double)contributionRow["MCER"], 2, 2));

                cell = (NPOI.HSSF.UserModel.HSSFCell)detailRow.CreateCell(lastColumnIndex + 2);
                cell.CellStyle = numericStyle;
                cell.SetCellValue(HROne.CommonLib.GenericRoundingFunctions.RoundingTo((double)contributionRow["VCER"], 2, 2));

                cell = (NPOI.HSSF.UserModel.HSSFCell)detailRow.CreateCell(lastColumnIndex + 3);
                cell.CellStyle = numericStyle;
                cell.SetCellValue(HROne.CommonLib.GenericRoundingFunctions.RoundingTo((double)contributionRow["PFUNDER"], 2, 2));

                cell = (NPOI.HSSF.UserModel.HSSFCell)detailRow.CreateCell(lastColumnIndex + 4);
                cell.CellStyle = numericStyle;
                cell.SetCellValue(HROne.CommonLib.GenericRoundingFunctions.RoundingTo((double)contributionRow["PFUNDER"] + (double)contributionRow["MCER"] + (double)contributionRow["VCER"], 2, 2));

                cell = (NPOI.HSSF.UserModel.HSSFCell)detailRow.CreateCell(lastColumnIndex + 6);
                cell.CellStyle = numericStyle;
                cell.SetCellValue(HROne.CommonLib.GenericRoundingFunctions.RoundingTo((double)contributionRow["PFUNDEE"] + (double)contributionRow["MCEE"] + (double)contributionRow["VCEE"], 2, 2));
            }
            lastColumnIndex += 6;
        }
        lastRowIndex++;
        AddSubTotalRow(excelWorksheet, currentCompany, recordCount, lastRowIndex, lastColumnIndex, headerRow, subTotalStyle);
        //excelWorkbook.Save(exportFileName);
        System.IO.FileStream file = new System.IO.FileStream(exportFileName, System.IO.FileMode.Create);
        excelWorkbook.Write(file);
        file.Close();

    }

    private void AddSubTotalRow(NPOI.HSSF.UserModel.HSSFSheet excelWorksheet, string currentCompany, int recordCount, int lastRowIndex, int lastColumnIndex, NPOI.HSSF.UserModel.HSSFRow headerRow, NPOI.HSSF.UserModel.HSSFCellStyle subTotalStyle)
    {
        //execlWorksheet.Cells[lastRowIndex, PAYMENTDETAIL_START_COLUMN - 1].Value = "Subtotal - " + currentCompany;

        NPOI.HSSF.UserModel.HSSFRow subTotalRow = (NPOI.HSSF.UserModel.HSSFRow)excelWorksheet.CreateRow(lastRowIndex);

        NPOI.HSSF.UserModel.HSSFCell subTotalCell = (NPOI.HSSF.UserModel.HSSFCell)subTotalRow.CreateCell(0);
        subTotalCell.SetCellValue("Subtotal - " + currentCompany);
        subTotalCell.CellStyle = subTotalStyle;

        excelWorksheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(lastRowIndex, lastRowIndex, 0, PAYMENTDETAIL_START_COLUMN - 1));

        for (int iCol = PAYMENTDETAIL_START_COLUMN; iCol <= lastColumnIndex; iCol++)
        {
            //double totalPayment = 0;
            //bool IsTotalPaymentNull = true;
            //for (int iRow = lastRowIndex - recordCount; iRow < lastRowIndex; iRow++)
            //{
            //    ExcelLibrary.SpreadSheet.Cell cell = execlWorksheet.Cells[iRow, iCol];
            //    if (cell.Value is double)
            //    {
            //        IsTotalPaymentNull = false;
            //        totalPayment += (double)cell.Value;
            //    }
            //}
            //if (!IsTotalPaymentNull)
            //{
            //    ExcelLibrary.SpreadSheet.Cell totalCell = execlWorksheet.Cells[lastRowIndex, iCol];
            //    totalCell.FormatString = "#,##0.00";
            //    totalCell.Value = HROne.CommonLib.GenericRoundingFunctions.RoundingTo(totalPayment, 2, 2);
            //}

            if (headerRow.GetCell(iCol) != null)
            {
                subTotalCell = (NPOI.HSSF.UserModel.HSSFCell)subTotalRow.CreateCell(iCol);
                subTotalCell.CellFormula = "SUM(" + ToCellString(lastRowIndex - recordCount, iCol) + ":" + ToCellString(lastRowIndex - 1, iCol) + ")";
                subTotalCell.CellStyle = subTotalStyle;
            }
        }

    }

    private string ToCellString(int row, int col)
    {
        string cellString = "";

        int tmpCol = col;

        while (tmpCol >= 26)
        {
            byte value = (byte)(tmpCol / 26);
            cellString += Convert.ToChar(value + 64);
            tmpCol = tmpCol % 26;
        }
        cellString += Convert.ToChar((byte)tmpCol + 65);
        return cellString + (row + 1);
    }

    private void CreateSummaryDataTable(DataSet summaryDataSet)
    {
        DataTable hierarchyTable = summaryDataSet.Tables.Add("Hierarchy");
        hierarchyTable.Columns.Add("ID", typeof(int));
        hierarchyTable.Columns.Add("Company", typeof(string));
        hierarchyTable.Columns.Add("PayrollGroupDesc", typeof(string));

        hierarchyTable.Columns.Add("HierarchyDesc", typeof(string));
        hierarchyTable.Columns.Add("CostCenterDesc", typeof(string));
        hierarchyTable.Columns.Add("CompanyID", typeof(int));
        hierarchyTable.Columns.Add("PayGroupID", typeof(int));
        hierarchyTable.Columns.Add("HElementID", typeof(int));
        hierarchyTable.Columns.Add("CostCenterID", typeof(int));

        DataTable paymentTable = summaryDataSet.Tables.Add("Payment");
        paymentTable.Columns.Add("ID", typeof(int));

        DataTable contributionTable = summaryDataSet.Tables.Add("Contribution");
        contributionTable.Columns.Add("ID", typeof(int));
        contributionTable.Columns.Add("MCER", typeof(double));
        contributionTable.Columns.Add("MCEE", typeof(double));
        contributionTable.Columns.Add("VCER", typeof(double));
        contributionTable.Columns.Add("VCEE", typeof(double));
        contributionTable.Columns.Add("PFUNDER", typeof(double));
        contributionTable.Columns.Add("PFUNDEE", typeof(double));
    }
}
