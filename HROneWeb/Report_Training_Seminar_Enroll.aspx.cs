using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using HROne.DataAccess;
//using perspectivemind.validation;
using HROne.Lib.Entities;

public partial class Report_Training_Seminar_Enroll : HROneWebPage
{
    private const string FUNCTION_CODE = "TRA004";
    protected DBManager db = EEmpPersonalInfo.db;
    protected SearchBinding binding;
    protected ListInfo info;
    protected DataView view;
    

    protected void Page_Load(object sender, EventArgs e)
    {
       
        if (!WebUtils.CheckAccess(Response, Session, FUNCTION_CODE, WebUtils.AccessLevel.Read))
            return;

        

        binding = new SearchBinding(dbConn, db);
        //binding.add(new LikeSearchBinder(EmpNo, "EmpNo"));
        //binding.add(new LikeSearchBinder(EmpName, "EmpEngSurname", "EmpEngOtherName", "EmpChiFullName"));
        //binding.add(new FieldDateRangeSearchBinder(JoinDateFrom, JoinDateTo, "EmpDateOfJoin").setUseCurDate(false));
        //binding.add(new DropDownVLSearchBinder(EmpStatus, "EmpStatus", EEmpPersonalInfo.VLEmpStatus).setLocale(ci));
        //binding.add(new DropDownVLSearchBinder((DropDownList)EmployeeSearchControl1.AdditionElementControl.FindControl("PayGroup"), "pp.PayGroupID", EPayrollGroup.VLPayrollGroup).setLocale(ci));
        binding.initValues("EmpStatus", null, EEmpPersonalInfo.VLEmpStatus, ci);
        binding.init(DecryptedRequest, null);

        info = ListFooter.ListInfo;

        HROne.Common.WebUtility.WebControlsLocalization(this, this.Controls);

        if (!IsPostBack)
        {
            WebFormUtils.loadValues(dbConn, TrainingCourseList, ETrainingCourse.VLTrainingCourse, new DBFilter());
        }
    }
    protected void Page_PreRender(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            view = loadEmpData(info, db, Repeater);
        }

    }

    public DataView loadEmpData(ListInfo info, DBManager db, Repeater repeater)
    {
        DBFilter filter = binding.createFilter();

        //if (info != null && info.orderby != null && !info.orderby.Equals(""))
        //    filter.add(info.orderby, info.order);

        string select = " e.* ";
        string from = "from [" + db.dbclass.tableName + "] e ";

        IN inTerm = new IN("e.EmpID", "Select ete.EmpID from EmpTrainingEnroll ete, TrainingSeminar ts ", filter);

        filter.add(new MatchField("ete.TrainingSeminarID", "ts.TrainingSeminarID"));

        //DateTime dtTrainingSeminarFrom;
        //DateTime dtTrainingSeminarTo;
        //if (DateTime.TryParse(((WebDatePicker)EmployeeSearchControl1.AdditionElementControl.FindControl("TrainingSeminarDateFrom")).Value, out dtTrainingSeminarFrom))
        //    filter.add(new Match("ts.TrainingSeminarDateTo", ">=", dtTrainingSeminarFrom));
        //if (DateTime.TryParse(((WebDatePicker)EmployeeSearchControl1.AdditionElementControl.FindControl("TrainingSeminarDateTo")).Value, out dtTrainingSeminarTo))
        //    filter.add(new Match("ts.TrainingSeminarDateFrom", "<=", dtTrainingSeminarTo));

        DBFilter resultFilter = new DBFilter();
        resultFilter.add(inTerm);

        DBFilter empInfoFilter = EmployeeSearchControl1.GetEmpInfoFilter(AppUtils.ServerDateTime(), AppUtils.ServerDateTime());
        empInfoFilter.add(new MatchField("e.EmpID", "ee.EmpID"));
        resultFilter.add(new Exists(EEmpPersonalInfo.db.dbclass.tableName + " ee", empInfoFilter));

        DataTable table = resultFilter.loadData(dbConn, null, select, from);
        table = EmployeeSearchControl1.FilterEncryptedEmpInfoField(table, info);

        view = new DataView(table);
        //if (info != null)
        //{
        //    info.loadPageList(null, PrevPage, NextPage, FirstPage, LastPage);

        //    CurPage.Value = info.page.ToString();
        //    NumPage.Value = info.numPage.ToString();
        //}
        if (repeater != null)
        {
            repeater.DataSource = table;
            repeater.DataBind();
        }

        return view;
    }
    protected void Search_Click(object sender, EventArgs e)
    {
        info.page = 0;
        view = loadEmpData(info, db, Repeater);

    }
    protected void Reset_Click(object sender, EventArgs e)
    {
        binding.clear();
        EmployeeSearchControl1.Reset();
        info.page = 0;
        view = loadEmpData(info, db, Repeater);

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

        view = loadEmpData(info, db, Repeater);

    }
    protected void Repeater_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        DataRowView row = (DataRowView)e.Item.DataItem;
        CheckBox cb = (CheckBox)e.Item.FindControl("ItemSelect");
        WebFormUtils.LoadKeys(db, row, cb);
    }

    protected void btnGenerate_Click(object sender, EventArgs e)
    {
        PageErrors errors = PageErrors.getErrors(db, Page.Master);
        errors.clear();

        ArrayList empList = WebUtils.SelectedRepeaterItemToBaseObjectList(EEmpPersonalInfo.db, Repeater, "ItemSelect");

        if (empList.Count > 0)
        {
            DateTime dtTrainingSeminarFrom;
            DateTime dtTrainingSeminarTo;
            //if (!DateTime.TryParse(((WebDatePicker)EmployeeSearchControl1.AdditionElementControl.FindControl("TrainingSeminarDateFrom")).Value, out dtTrainingSeminarFrom))
            //    dtTrainingSeminarFrom = new DateTime();
            //if (!DateTime.TryParse(((WebDatePicker)EmployeeSearchControl1.AdditionElementControl.FindControl("TrainingSeminarDateTo")).Value, out dtTrainingSeminarTo))
            //    dtTrainingSeminarTo = new DateTime();
            if (!DateTime.TryParse(TrainingSeminarDateFrom.Value, out dtTrainingSeminarFrom))
                dtTrainingSeminarFrom = new DateTime();
            if (!DateTime.TryParse(TrainingSeminarDateTo.Value, out dtTrainingSeminarTo))
                dtTrainingSeminarTo = new DateTime();

            GenerateReport(empList, dtTrainingSeminarFrom, dtTrainingSeminarTo, ReportSortBy.SelectedValue);
        }
        //ArrayList empList = new ArrayList();
        //ArrayList payBatchList = new ArrayList();

        //foreach (RepeaterItem i in Repeater.Items)
        //{
        //    CheckBox cb = (CheckBox)i.FindControl("ItemSelect");
        //    if (cb.Checked)
        //    {
        //        EEmpPersonalInfo o = (EEmpPersonalInfo)EEmpPersonalInfo.db.createObject();
        //        WebFormUtils.GetKeys(EEmpPersonalInfo.db, o, cb);
        //        empList.Add(o);
        //    }

        //}

        //string strEmpList = string.Empty;
        //if (empList.Count > 0)
        //{

        //    //foreach (EEmpPersonalInfo o in empList)
        //    //{
        //    //    if (strEmpList == string.Empty)
        //    //        strEmpList = ((EEmpPersonalInfo)o).EmpID.ToString();
        //    //    else
        //    //        strEmpList += "_" + ((EEmpPersonalInfo)o).EmpID.ToString();

        //    //}
        //    DateTime dtPayPeriodFr;
        //    DateTime dtPayPeriodTo;
        //    //String strPayPeriodRequest = string.Empty;
        //    if (DateTime.TryParse(((WebDatePicker)EmployeeSearchControl1.AdditionElementControl.FindControl("PayPeriodFr")).Value, out dtPayPeriodFr) && DateTime.TryParse(((WebDatePicker)EmployeeSearchControl1.AdditionElementControl.FindControl("PayPeriodTo")).Value, out dtPayPeriodTo))
        //    {
        //        //strPayPeriodRequest = "&PayPeriodFr=" + dtPayPeriodFr.Ticks + "&PayPeriodTo=" + dtPayPeriodTo.Ticks;
        //    }
        //    else
        //    {
        //        dtPayPeriodFr = new DateTime();
        //        dtPayPeriodTo = new DateTime();
        //        //strPayPeriodRequest = "&PayPeriodFr=" + 0 + "&PayPeriodTo=" + 0;
        //    }

        //    HROne.Reports.Payroll.MPFDetailListProcess rpt = new HROne.Reports.Payroll.MPFDetailListProcess(empList, dtPayPeriodFr, dtPayPeriodTo);

        //    string reportFileName = WebUtils.GetLocalizedReportFile(Server.MapPath("~/Report_Payroll_MPFDetailList.rpt"));

        //    WebUtils.ReportExport(dbConn, user, errors, lblReportHeader.Text, Response, rpt, reportFileName, ((Button)sender).CommandArgument, "MPFDetailList");

        //    //Server.Transfer("Report_Payroll_MPFDetailList_View.aspx?"
        //    //    + "EmpID=" + strEmpList
        //    //    + strPayPeriodRequest
        //    //    );

        //    //            errors.addError("Complete");
        //    //Response.Write("<script>alert('Completed'); </script>");
        //}
        //else
        //    errors.addError("Employee not selected");

        ////        emploadData(empInfo, EEmpPayroll.db, empRepeater);
    }

    private void GenerateReport(ArrayList empList, DateTime PeriodFrom, DateTime PeriodTo, string SortBy)
    {
        DataTable dataTable = new DataTable();
        dataTable.Columns.Add("EmpID", typeof(int));
        dataTable.Columns.Add("EmpNo", typeof(string));
        dataTable.Columns.Add("EmpName", typeof(string));
        dataTable.Columns.Add("EmpAlias", typeof(string));
        dataTable.Columns.Add("Company", typeof(string));

        DBFilter hierarchyLevelFilter = new DBFilter();
        Hashtable hierarchyLevelHashTable = new Hashtable();
        hierarchyLevelFilter.add("HLevelSeqNo", true);
        ArrayList hierarchyLevelList = EHierarchyLevel.db.select(dbConn, hierarchyLevelFilter);
        foreach (EHierarchyLevel hlevel in hierarchyLevelList)
        {
            dataTable.Columns.Add(hlevel.HLevelDesc, typeof(string));
            hierarchyLevelHashTable.Add(hlevel.HLevelID, hlevel);
        }
        dataTable.Columns.Add("Position", typeof(string));

        dataTable.Columns.Add("TrainingSeminarID", typeof(int));
        dataTable.Columns.Add("TrainingCourseName", typeof(string));
        dataTable.Columns.Add("TrainingSeminarDesc", typeof(string));
        dataTable.Columns.Add("TrainingSeminarDateFrom", typeof(DateTime));
        dataTable.Columns.Add("TrainingSeminarDateTo", typeof(DateTime));
        dataTable.Columns.Add("TrainingSeminarDuration", typeof(double));
        dataTable.Columns.Add("TrainingSeminarDurationUnit", typeof(string));
        dataTable.Columns.Add("TrainingSeminarTrainer", typeof(string));


        foreach (EEmpPersonalInfo empInfo in empList)
        {
            if (EEmpPersonalInfo.db.select(dbConn, empInfo))
            {

                EEmpPositionInfo empPos = AppUtils.GetLastPositionInfo(dbConn, AppUtils.ServerDateTime().Date, empInfo.EmpID);

                ECompany company = new ECompany();
                EPosition position = new EPosition();
                ArrayList empHierarchyList = new ArrayList();
                if (empPos != null)
                {
                    company.CompanyID = empPos.CompanyID;
                    ECompany.db.select(dbConn, company);
                    //row["Company"] = company.CompanyCode;
                    DBFilter empHierarchyFilter = new DBFilter();
                    empHierarchyFilter.add(new Match("EmpPosID", empPos.EmpPosID));
                    empHierarchyList = EEmpHierarchy.db.select(dbConn, empHierarchyFilter);
                    //foreach (EEmpHierarchy empHierarchy in empHierarchyList)
                    //{
                    //    EHierarchyLevel hierarchyLevel = (EHierarchyLevel)hierarchyLevelHashTable[empHierarchy.HLevelID];
                    //    if (hierarchyLevel != null)
                    //    {
                    //        EHierarchyElement hierarchyElement = new EHierarchyElement();
                    //        hierarchyElement.HElementID = empHierarchy.HElementID;
                    //        if (EHierarchyElement.db.select(dbConn, hierarchyElement))
                    //            row[hierarchyLevel.HLevelDesc] = hierarchyElement.HElementDesc;
                    //    }
                    //}

                    position.PositionID = empPos.PositionID;
                    EPosition.db.select(dbConn, position);
                }

                DBFilter empTrainingSeminar = new DBFilter();
                empTrainingSeminar.add(new Match("EmpID", empInfo.EmpID));

                OR orTrainingCourse = null;
                foreach (ListItem item in TrainingCourseList.Items)
                    if (item.Selected)
                    {
                        if (orTrainingCourse == null)
                            orTrainingCourse = new OR();
                        orTrainingCourse.add(new Match("te.TrainingCourseID", item.Value));
                    }
                DBFilter trainingSeminarFilter = new DBFilter();
                if (!PeriodFrom.Ticks.Equals(0))
                    trainingSeminarFilter.add(new Match("TrainingSeminarDateFrom", ">=", PeriodFrom));
                if (!PeriodTo.Ticks.Equals(0))
                    trainingSeminarFilter.add(new Match("TrainingSeminarDateTo", "<=", PeriodTo));
                if (orTrainingCourse != null)
                    trainingSeminarFilter.add(orTrainingCourse);
                empTrainingSeminar.add(new IN("TrainingSeminarID", "Select TrainingSeminarID from " + ETrainingSeminar.db.dbclass.tableName + " te", trainingSeminarFilter));
                ArrayList empTrainingSeminarList = EEmpTrainingEnroll.db.select(dbConn, empTrainingSeminar);
                foreach (EEmpTrainingEnroll empTrainingEnroll in empTrainingSeminarList)
                {
                    ETrainingSeminar trainingSeminar = new ETrainingSeminar();
                    trainingSeminar.TrainingSeminarID = empTrainingEnroll.TrainingSeminarID;
                    if (ETrainingSeminar.db.select(dbConn, trainingSeminar))
                    {
                        DataRow row = dataTable.NewRow();
                        row["EmpID"] = empInfo.EmpID;
                        row["EmpNo"] = empInfo.EmpNo;
                        row["EmpName"] = empInfo.EmpEngFullName;
                        row["EmpAlias"] = empInfo.EmpAlias;
                        row["Company"] = company.CompanyCode;
                        foreach (EEmpHierarchy empHierarchy in empHierarchyList)
                        {
                            EHierarchyLevel hierarchyLevel = (EHierarchyLevel)hierarchyLevelHashTable[empHierarchy.HLevelID];
                            if (hierarchyLevel != null)
                            {
                                EHierarchyElement hierarchyElement = new EHierarchyElement();
                                hierarchyElement.HElementID = empHierarchy.HElementID;
                                if (EHierarchyElement.db.select(dbConn, hierarchyElement))
                                    row[hierarchyLevel.HLevelDesc] = hierarchyElement.HElementDesc;
                            }
                        }
                        row["Position"] = position.PositionDesc;

                        row["TrainingSeminarID"] = trainingSeminar.TrainingSeminarID;
                        ETrainingCourse trainingCourse = new ETrainingCourse();
                        trainingCourse.TrainingCourseID = trainingSeminar.TrainingCourseID;
                        if (ETrainingCourse.db.select(dbConn, trainingCourse))
                            row["TrainingCourseName"] = trainingCourse.TrainingCourseName;
                        else
                            row["TrainingCourseName"] = string.Empty;

                        row["TrainingSeminarDesc"] = trainingSeminar.TrainingSeminarDesc == null ? string.Empty : trainingSeminar.TrainingSeminarDesc;
                        row["TrainingSeminarDateFrom"] = trainingSeminar.TrainingSeminarDateFrom;
                        row["TrainingSeminarDateTo"] = trainingSeminar.TrainingSeminarDateTo;
                        row["TrainingSeminarDuration"] = trainingSeminar.TrainingSeminarDuration;
                        if (trainingSeminar.TrainingSeminarDurationUnit.Equals("H"))
                            row["TrainingSeminarDurationUnit"] = "Hour(s)";
                        else
                            row["TrainingSeminarDurationUnit"] = trainingSeminar.TrainingSeminarDurationUnit;

                        row["TrainingSeminarTrainer"] = trainingSeminar.TrainingSeminarTrainer;

                        dataTable.Rows.Add(row);
                    }
                }

            }
        }

        //org.in2bits.MyXls.XlsDocument document = new org.in2bits.MyXls.XlsDocument();
        //org.in2bits.MyXls.Worksheet worksheet = document.Workbook.Worksheets.Add("training report");

        NPOI.HSSF.UserModel.HSSFWorkbook workbook = new NPOI.HSSF.UserModel.HSSFWorkbook();
        NPOI.HSSF.UserModel.HSSFSheet worksheet = (NPOI.HSSF.UserModel.HSSFSheet)workbook.CreateSheet("training report");

        NPOI.HSSF.UserModel.HSSFFont boldFont = (NPOI.HSSF.UserModel.HSSFFont)workbook.CreateFont();
        boldFont.Boldweight = (short)NPOI.SS.UserModel.FontBoldWeight.BOLD;

        NPOI.HSSF.UserModel.HSSFCellStyle reportHeaderStyle = (NPOI.HSSF.UserModel.HSSFCellStyle)workbook.CreateCellStyle();
        reportHeaderStyle.SetFont(boldFont);

        NPOI.HSSF.UserModel.HSSFCellStyle columnHeaderStyleCenter = (NPOI.HSSF.UserModel.HSSFCellStyle)workbook.CreateCellStyle();
        columnHeaderStyleCenter.SetFont(boldFont);
        columnHeaderStyleCenter.Alignment = NPOI.SS.UserModel.HorizontalAlignment.CENTER;

        //NPOI.HSSF.UserModel.HSSFCellStyle numericStyle = (NPOI.HSSF.UserModel.HSSFCellStyle)workbook.CreateCellStyle();
        //numericStyle.DataFormat = workbook.CreateDataFormat().GetFormat("0.00");

        int rowCount = 0;
        //worksheet.Cells.Add(rowCount, (ushort)1, "Training Report").Font.Bold = true;
        NPOI.HSSF.UserModel.HSSFCell reportHeaderCell = (NPOI.HSSF.UserModel.HSSFCell)worksheet.CreateRow(rowCount).CreateCell(0);
        reportHeaderCell.SetCellValue("Training Report");
        reportHeaderCell.CellStyle = reportHeaderStyle;
        rowCount++;

        if (!PeriodFrom.Ticks.Equals(0) && !PeriodTo.Ticks.Equals(0))
        {
            rowCount++;
            //worksheet.Cells.Add(rowCount, (ushort)1, "Period: " + PeriodFrom.ToString("dd/MM/yyyy") + " - " + PeriodTo.ToString("dd/MM/yyyy"));
            worksheet.CreateRow(rowCount).CreateCell(0).SetCellValue("Period: " + PeriodFrom.ToString("dd/MM/yyyy") + " - " + PeriodTo.ToString("dd/MM/yyyy"));
            rowCount++;
        }
        else if (!PeriodTo.Ticks.Equals(0))
        {
            rowCount++;
            //worksheet.Cells.Add(rowCount, (ushort)1, "Up to: " + PeriodTo.ToString("dd/MM/yyyy"));
            worksheet.CreateRow(rowCount).CreateCell(0).SetCellValue("Up to: " + PeriodTo.ToString("dd/MM/yyyy"));
            rowCount++;
        }
        else if (!PeriodFrom.Ticks.Equals(0))
        {
            rowCount++;
            //worksheet.Cells.Add(rowCount, (ushort)1, "From: " + PeriodFrom.ToString("dd/MM/yyyy"));
            worksheet.CreateRow(rowCount).CreateCell(0).SetCellValue("From: " + PeriodFrom.ToString("dd/MM/yyyy"));
            rowCount++;
        }

        if (SortBy.Equals("Date", StringComparison.CurrentCultureIgnoreCase))
        {
            DataView dataView = new DataView(dataTable);
            dataView.Sort = "TrainingSeminarDateFrom, TrainingSeminarDateTo, TrainingCourseName, EmpNo";
            DataTable sortedTable = dataView.ToTable();
            rowCount++;
            //worksheet.Cells.Add(rowCount, (ushort)1, "Training Date").Font.Bold = true;
            //worksheet.Cells.Add(rowCount, (ushort)2, "Course Name").Font.Bold = true;
            //worksheet.Cells.Add(rowCount, (ushort)3, "Description").Font.Bold = true;
            //worksheet.Cells.Add(rowCount, (ushort)4, "Duration").Font.Bold = true;
            //worksheet.Rows[rowCount].GetCell(4).HorizontalAlignment = org.in2bits.MyXls.HorizontalAlignments.Centered;
            //worksheet.Cells.Add(rowCount, (ushort)5, string.Empty);
            //worksheet.Cells.Add(rowCount, (ushort)6, "Trainer").Font.Bold = true;
            //worksheet.Cells.Add(rowCount, (ushort)7, "Employee No.").Font.Bold = true;
            //worksheet.Cells.Add(rowCount, (ushort)8, "Employee Name").Font.Bold = true;
            //worksheet.Cells.Add(rowCount, (ushort)9, "Alias").Font.Bold = true;
            //worksheet.Cells.Add(rowCount, (ushort)10, "Position").Font.Bold = true;
            //worksheet.Cells.Add(rowCount, (ushort)11, "Company").Font.Bold = true;
            //worksheet.Cells.Merge(rowCount, rowCount, 4, 5);
            NPOI.HSSF.UserModel.HSSFRow columnHeaderRow = (NPOI.HSSF.UserModel.HSSFRow)worksheet.CreateRow(rowCount);
            NPOI.HSSF.UserModel.HSSFCell columnHeaderCell = (NPOI.HSSF.UserModel.HSSFCell)columnHeaderRow.CreateCell(0);
            columnHeaderCell.SetCellValue("Training Date");
            columnHeaderCell.CellStyle = reportHeaderStyle;
            columnHeaderCell = (NPOI.HSSF.UserModel.HSSFCell)columnHeaderRow.CreateCell(1);
            columnHeaderCell.SetCellValue("Course Name");
            columnHeaderCell.CellStyle = reportHeaderStyle;
            columnHeaderCell = (NPOI.HSSF.UserModel.HSSFCell)columnHeaderRow.CreateCell(2);
            columnHeaderCell.SetCellValue("Description");
            columnHeaderCell.CellStyle = reportHeaderStyle;
            columnHeaderCell = (NPOI.HSSF.UserModel.HSSFCell)columnHeaderRow.CreateCell(3);
            columnHeaderCell.SetCellValue("Duration");
            columnHeaderCell.CellStyle = columnHeaderStyleCenter;
            worksheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(rowCount, rowCount, 3, 4));
            columnHeaderCell = (NPOI.HSSF.UserModel.HSSFCell)columnHeaderRow.CreateCell(5);
            columnHeaderCell.SetCellValue("Trainer");
            columnHeaderCell.CellStyle = reportHeaderStyle;
            columnHeaderCell = (NPOI.HSSF.UserModel.HSSFCell)columnHeaderRow.CreateCell(6);
            columnHeaderCell.SetCellValue("Employee No.");
            columnHeaderCell.CellStyle = reportHeaderStyle;
            columnHeaderCell = (NPOI.HSSF.UserModel.HSSFCell)columnHeaderRow.CreateCell(7);
            columnHeaderCell.SetCellValue("Employee Name");
            columnHeaderCell.CellStyle = reportHeaderStyle;
            columnHeaderCell = (NPOI.HSSF.UserModel.HSSFCell)columnHeaderRow.CreateCell(8);
            columnHeaderCell.SetCellValue("Alias");
            columnHeaderCell.CellStyle = reportHeaderStyle;
            columnHeaderCell = (NPOI.HSSF.UserModel.HSSFCell)columnHeaderRow.CreateCell(9);
            columnHeaderCell.SetCellValue("Position");
            columnHeaderCell.CellStyle = reportHeaderStyle;
            columnHeaderCell = (NPOI.HSSF.UserModel.HSSFCell)columnHeaderRow.CreateCell(10);
            columnHeaderCell.SetCellValue("Company");
            columnHeaderCell.CellStyle = reportHeaderStyle;

            int colCount = 10;
            foreach (EHierarchyLevel hlevel in hierarchyLevelList)
            {
                colCount++;
                //worksheet.Cells.Add(rowCount, colCount, hlevel.HLevelDesc).Font.Bold = true;
                columnHeaderCell = (NPOI.HSSF.UserModel.HSSFCell)columnHeaderRow.CreateCell(colCount);
                columnHeaderCell.SetCellValue(hlevel.HLevelDesc);
                columnHeaderCell.CellStyle = reportHeaderStyle;
            }

            int currentTrainingSeminarID = 0;
            foreach (DataRow row in sortedTable.Rows)
            {
                rowCount++;
                NPOI.HSSF.UserModel.HSSFRow detailRow = (NPOI.HSSF.UserModel.HSSFRow)worksheet.CreateRow(rowCount);

                if (!currentTrainingSeminarID.Equals((int)row["TrainingSeminarID"]))
                    if (row["TrainingSeminarDateFrom"] != DBNull.Value)
                    {
                        DateTime trainingFrom = ((DateTime)row["TrainingSeminarDateFrom"]);
                        DateTime trainingTo = ((DateTime)row["TrainingSeminarDateTo"]);
                        if (trainingFrom.Equals(trainingTo))

                            //worksheet.Cells.Add(rowCount, (ushort)1, trainingFrom.ToString("dd/MM/yyyy"));
                            detailRow.CreateCell(0).SetCellValue(trainingFrom.ToString("dd/MM/yyyy"));
                        else
                            //worksheet.Cells.Add(rowCount, (ushort)1, trainingFrom.ToString("dd/MM/yyyy") + " - " + trainingTo.ToString("dd/MM/yyyy"));
                            detailRow.CreateCell(0).SetCellValue(trainingFrom.ToString("dd/MM/yyyy") + " - " + trainingTo.ToString("dd/MM/yyyy"));

                    }
                currentTrainingSeminarID = ((int)row["TrainingSeminarID"]);

                //worksheet.Cells.Add(rowCount, (ushort)2, row["TrainingCourseName"]);
                //worksheet.Cells.Add(rowCount, (ushort)3, row["TrainingSeminarDesc"]);
                //worksheet.Cells.Add(rowCount, (ushort)4, row["TrainingSeminarDuration"]);
                //worksheet.Cells.Add(rowCount, (ushort)5, row["TrainingSeminarDurationUnit"]);
                //worksheet.Cells.Add(rowCount, (ushort)6, row["TrainingSeminarTrainer"]);
                //worksheet.Cells.Add(rowCount, (ushort)7, row["EmpNo"]);
                //worksheet.Cells.Add(rowCount, (ushort)8, row["EmpName"]);
                //worksheet.Cells.Add(rowCount, (ushort)9, row["EmpAlias"]);
                //worksheet.Cells.Add(rowCount, (ushort)10, row["Position"]);
                //worksheet.Cells.Add(rowCount, (ushort)11, row["Company"]);

                detailRow.CreateCell(1).SetCellValue(row["TrainingCourseName"].ToString());
                detailRow.CreateCell(2).SetCellValue(row["TrainingSeminarDesc"].ToString());
                detailRow.CreateCell(3).SetCellValue((double)row["TrainingSeminarDuration"]);
                //detailRow.GetCell(3).CellStyle = numericStyle;
                detailRow.CreateCell(4).SetCellValue(row["TrainingSeminarDurationUnit"].ToString());
                detailRow.CreateCell(5).SetCellValue(row["TrainingSeminarTrainer"].ToString());
                detailRow.CreateCell(6).SetCellValue(row["EmpNo"].ToString());
                detailRow.CreateCell(7).SetCellValue(row["EmpName"].ToString());
                detailRow.CreateCell(8).SetCellValue(row["EmpAlias"].ToString());
                detailRow.CreateCell(9).SetCellValue(row["Position"].ToString());
                detailRow.CreateCell(10).SetCellValue(row["Company"].ToString());

                colCount = 10;
                foreach (EHierarchyLevel hlevel in hierarchyLevelList)
                {
                    colCount++;
                    if (row[hlevel.HLevelDesc] != DBNull.Value)
                        //worksheet.Cells.Add(rowCount, colCount, row[hlevel.HLevelDesc]);
                        detailRow.CreateCell(colCount).SetCellValue(row[hlevel.HLevelDesc].ToString());
                }

            }
        }
        else if (SortBy.Equals("Position", StringComparison.CurrentCultureIgnoreCase))
        {
            DataView dataView = new DataView(dataTable);
            dataView.Sort = "Position, EmpNo, TrainingSeminarDateFrom, TrainingSeminarDateTo, TrainingCourseName ";
            DataTable sortedTable = dataView.ToTable();
            rowCount++;
            //worksheet.Cells.Add(rowCount, (ushort)1, "Position").Font.Bold = true;
            //worksheet.Cells.Add(rowCount, (ushort)2, "Employee No.").Font.Bold = true;
            //worksheet.Cells.Add(rowCount, (ushort)3, "Employee Name").Font.Bold = true;
            //worksheet.Cells.Add(rowCount, (ushort)4, "Alias").Font.Bold = true;
            //worksheet.Cells.Add(rowCount, (ushort)5, "Company").Font.Bold = true;
            NPOI.HSSF.UserModel.HSSFRow columnHeaderRow = (NPOI.HSSF.UserModel.HSSFRow)worksheet.CreateRow(rowCount);
            NPOI.HSSF.UserModel.HSSFCell columnHeaderCell = (NPOI.HSSF.UserModel.HSSFCell)columnHeaderRow.CreateCell(0);
            columnHeaderCell.SetCellValue("Position");
            columnHeaderCell.CellStyle = reportHeaderStyle;
            columnHeaderCell = (NPOI.HSSF.UserModel.HSSFCell)columnHeaderRow.CreateCell(1);
            columnHeaderCell.SetCellValue("Employee No.");
            columnHeaderCell.CellStyle = reportHeaderStyle;
            columnHeaderCell = (NPOI.HSSF.UserModel.HSSFCell)columnHeaderRow.CreateCell(2);
            columnHeaderCell.SetCellValue("Employee Name");
            columnHeaderCell.CellStyle = reportHeaderStyle;
            columnHeaderCell = (NPOI.HSSF.UserModel.HSSFCell)columnHeaderRow.CreateCell(3);
            columnHeaderCell.SetCellValue("Alias");
            columnHeaderCell.CellStyle = reportHeaderStyle;
            columnHeaderCell = (NPOI.HSSF.UserModel.HSSFCell)columnHeaderRow.CreateCell(4);
            columnHeaderCell.SetCellValue("Company");
            columnHeaderCell.CellStyle = reportHeaderStyle;

            int colCount = 4;
            foreach (EHierarchyLevel hlevel in hierarchyLevelList)
            {
                colCount++;
                //worksheet.Cells.Add(rowCount, colCount, hlevel.HLevelDesc).Font.Bold = true;
                columnHeaderCell = (NPOI.HSSF.UserModel.HSSFCell)columnHeaderRow.CreateCell(colCount);
                columnHeaderCell.SetCellValue(hlevel.HLevelDesc);
                columnHeaderCell.CellStyle = reportHeaderStyle;
            }

            //worksheet.Cells.Add(rowCount, (ushort)colCount + 1, "Course Name").Font.Bold = true;
            //worksheet.Cells.Add(rowCount, (ushort)colCount + 2, "Description").Font.Bold = true;
            //worksheet.Cells.Add(rowCount, (ushort)colCount + 3, "Training Date").Font.Bold = true;
            //worksheet.Cells.Add(rowCount, (ushort)colCount + 4, "Duration").Font.Bold = true;
            //worksheet.Cells.Add(rowCount, (ushort)colCount + 5, string.Empty);
            //worksheet.Cells.Merge(rowCount, rowCount, colCount + 4, colCount + 5);
            //worksheet.Rows[rowCount].GetCell((ushort)(colCount + 4)).HorizontalAlignment = org.in2bits.MyXls.HorizontalAlignments.Centered;
            //worksheet.Cells.Add(rowCount, (ushort)colCount + 6, "Trainer").Font.Bold = true;

            columnHeaderCell = (NPOI.HSSF.UserModel.HSSFCell)columnHeaderRow.CreateCell(colCount + 1);
            columnHeaderCell.SetCellValue("Course Name");
            columnHeaderCell.CellStyle = reportHeaderStyle;
            columnHeaderCell = (NPOI.HSSF.UserModel.HSSFCell)columnHeaderRow.CreateCell(colCount + 2);
            columnHeaderCell.SetCellValue("Description");
            columnHeaderCell.CellStyle = reportHeaderStyle;
            columnHeaderCell = (NPOI.HSSF.UserModel.HSSFCell)columnHeaderRow.CreateCell(colCount + 3);
            columnHeaderCell.SetCellValue("Training Date");
            columnHeaderCell.CellStyle = reportHeaderStyle;
            columnHeaderCell = (NPOI.HSSF.UserModel.HSSFCell)columnHeaderRow.CreateCell(colCount + 4);
            columnHeaderCell.SetCellValue("Duration");
            columnHeaderCell.CellStyle = columnHeaderStyleCenter;
            worksheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(rowCount, rowCount, colCount + 4, colCount + 5));
            columnHeaderCell = (NPOI.HSSF.UserModel.HSSFCell)columnHeaderRow.CreateCell(colCount + 6);
            columnHeaderCell.SetCellValue("Trainer");
            columnHeaderCell.CellStyle = reportHeaderStyle;

            int currentEmpID = 0;
            foreach (DataRow row in sortedTable.Rows)
            {
                rowCount++;
                NPOI.HSSF.UserModel.HSSFRow detailRow = (NPOI.HSSF.UserModel.HSSFRow)worksheet.CreateRow(rowCount);

                if (!currentEmpID.Equals((int)row["EmpID"]))
                {
                    //worksheet.Cells.Add(rowCount, (ushort)1, row["Position"]);
                    //worksheet.Cells.Add(rowCount, (ushort)2, row["EmpNo"]);
                    //worksheet.Cells.Add(rowCount, (ushort)3, row["EmpName"]);
                    //worksheet.Cells.Add(rowCount, (ushort)4, row["EmpAlias"]);
                    //worksheet.Cells.Add(rowCount, (ushort)5, row["Company"]);
                    detailRow.CreateCell(0).SetCellValue(row["Position"].ToString());
                    detailRow.CreateCell(1).SetCellValue(row["EmpNo"].ToString());
                    detailRow.CreateCell(2).SetCellValue(row["EmpName"].ToString());
                    detailRow.CreateCell(3).SetCellValue(row["EmpAlias"].ToString());
                    detailRow.CreateCell(4).SetCellValue(row["Company"].ToString());


                    colCount = 4;
                    foreach (EHierarchyLevel hlevel in hierarchyLevelList)
                    {
                        colCount++;
                        if (row[hlevel.HLevelDesc] != DBNull.Value)
                            //worksheet.Cells.Add(rowCount, colCount, row[hlevel.HLevelDesc]);
                            detailRow.CreateCell(colCount).SetCellValue(row[hlevel.HLevelDesc].ToString());
                    }
                }
                currentEmpID = ((int)row["EmpID"]);

                //worksheet.Cells.Add(rowCount, (ushort)colCount + 1, row["TrainingCourseName"]);
                //worksheet.Cells.Add(rowCount, (ushort)colCount + 2, row["TrainingSeminarDesc"]);
                detailRow.CreateCell(colCount + 1).SetCellValue(row["TrainingCourseName"].ToString());
                detailRow.CreateCell(colCount + 2).SetCellValue(row["TrainingSeminarDesc"].ToString());
                if (row["TrainingSeminarDateFrom"] != DBNull.Value)
                {
                    DateTime trainingFrom = ((DateTime)row["TrainingSeminarDateFrom"]);
                    DateTime trainingTo = ((DateTime)row["TrainingSeminarDateTo"]);
                    if (trainingFrom.Equals(trainingTo))

                        //worksheet.Cells.Add(rowCount, (ushort)colCount + 3, trainingFrom.ToString("dd/MM/yyyy"));
                        detailRow.CreateCell(colCount + 3).SetCellValue(trainingFrom.ToString("dd/MM/yyyy"));
                    else
                        //worksheet.Cells.Add(rowCount, (ushort)colCount + 3, trainingFrom.ToString("dd/MM/yyyy") + " - " + trainingTo.ToString("dd/MM/yyyy"));
                        detailRow.CreateCell(colCount + 3).SetCellValue(trainingFrom.ToString("dd/MM/yyyy") + " - " + trainingTo.ToString("dd/MM/yyyy"));

                }

                //worksheet.Cells.Add(rowCount, (ushort)colCount + 4, row["TrainingSeminarDuration"]);
                //worksheet.Cells.Add(rowCount, (ushort)colCount + 5, row["TrainingSeminarDurationUnit"]);
                //worksheet.Cells.Add(rowCount, (ushort)colCount + 6, row["TrainingSeminarTrainer"]);
                detailRow.CreateCell(colCount + 4).SetCellValue((double)row["TrainingSeminarDuration"]);
                //detailRow.GetCell(colCount + 4).CellStyle = numericStyle;
                detailRow.CreateCell(colCount + 5).SetCellValue(row["TrainingSeminarDurationUnit"].ToString());
                detailRow.CreateCell(colCount + 6).SetCellValue(row["TrainingSeminarTrainer"].ToString());

            }
        }
        else if (SortBy.Equals("Course", StringComparison.CurrentCultureIgnoreCase))
        {
            DataView dataView = new DataView(dataTable);
            dataView.Sort = "TrainingCourseName, TrainingSeminarDateFrom, TrainingSeminarDateTo, EmpNo";
            DataTable sortedTable = dataView.ToTable();
            rowCount++;
            //worksheet.Cells.Add(rowCount, (ushort)1, "Course Name").Font.Bold = true;
            //worksheet.Cells.Add(rowCount, (ushort)2, "Description").Font.Bold = true;
            //worksheet.Cells.Add(rowCount, (ushort)3, "Training Date").Font.Bold = true;
            //worksheet.Cells.Add(rowCount, (ushort)4, "Duration").Font.Bold = true;
            //worksheet.Rows[rowCount].GetCell(4).HorizontalAlignment = org.in2bits.MyXls.HorizontalAlignments.Centered;
            //worksheet.Cells.Add(rowCount, (ushort)5, string.Empty);
            //worksheet.Cells.Add(rowCount, (ushort)6, "Trainer").Font.Bold = true;
            //worksheet.Cells.Add(rowCount, (ushort)7, "Employee No.").Font.Bold = true;
            //worksheet.Cells.Add(rowCount, (ushort)8, "Employee Name").Font.Bold = true;
            //worksheet.Cells.Add(rowCount, (ushort)9, "Alias").Font.Bold = true;
            //worksheet.Cells.Add(rowCount, (ushort)10, "Position").Font.Bold = true;
            //worksheet.Cells.Add(rowCount, (ushort)11, "Company").Font.Bold = true;
            //worksheet.Cells.Merge(rowCount, rowCount, 4, 5);

            NPOI.HSSF.UserModel.HSSFRow columnHeaderRow = (NPOI.HSSF.UserModel.HSSFRow)worksheet.CreateRow(rowCount);
            NPOI.HSSF.UserModel.HSSFCell columnHeaderCell = (NPOI.HSSF.UserModel.HSSFCell)columnHeaderRow.CreateCell(0);
            columnHeaderCell.SetCellValue("Course Name");
            columnHeaderCell.CellStyle = reportHeaderStyle;
            columnHeaderCell = (NPOI.HSSF.UserModel.HSSFCell)columnHeaderRow.CreateCell(1);
            columnHeaderCell.SetCellValue("Description");
            columnHeaderCell.CellStyle = reportHeaderStyle;
            columnHeaderCell = (NPOI.HSSF.UserModel.HSSFCell)columnHeaderRow.CreateCell(2);
            columnHeaderCell.SetCellValue("Training Date");
            columnHeaderCell.CellStyle = reportHeaderStyle;
            columnHeaderCell = (NPOI.HSSF.UserModel.HSSFCell)columnHeaderRow.CreateCell(3);
            columnHeaderCell.SetCellValue("Duration");
            columnHeaderCell.CellStyle = columnHeaderStyleCenter;
            worksheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(rowCount, rowCount, 3, 4));
            columnHeaderCell = (NPOI.HSSF.UserModel.HSSFCell)columnHeaderRow.CreateCell(5);
            columnHeaderCell.SetCellValue("Trainer");
            columnHeaderCell.CellStyle = reportHeaderStyle;
            columnHeaderCell = (NPOI.HSSF.UserModel.HSSFCell)columnHeaderRow.CreateCell(6);
            columnHeaderCell.SetCellValue("Employee No.");
            columnHeaderCell.CellStyle = reportHeaderStyle;
            columnHeaderCell = (NPOI.HSSF.UserModel.HSSFCell)columnHeaderRow.CreateCell(7);
            columnHeaderCell.SetCellValue("Employee Name");
            columnHeaderCell.CellStyle = reportHeaderStyle;
            columnHeaderCell = (NPOI.HSSF.UserModel.HSSFCell)columnHeaderRow.CreateCell(8);
            columnHeaderCell.SetCellValue("Alias");
            columnHeaderCell.CellStyle = reportHeaderStyle;
            columnHeaderCell = (NPOI.HSSF.UserModel.HSSFCell)columnHeaderRow.CreateCell(9);
            columnHeaderCell.SetCellValue("Position");
            columnHeaderCell.CellStyle = reportHeaderStyle;
            columnHeaderCell = (NPOI.HSSF.UserModel.HSSFCell)columnHeaderRow.CreateCell(10);
            columnHeaderCell.SetCellValue("Company");
            columnHeaderCell.CellStyle = reportHeaderStyle;

            int colCount = 10;
            foreach (EHierarchyLevel hlevel in hierarchyLevelList)
            {
                colCount++;
                //worksheet.Cells.Add(rowCount, colCount, hlevel.HLevelDesc).Font.Bold = true;
                columnHeaderCell = (NPOI.HSSF.UserModel.HSSFCell)columnHeaderRow.CreateCell(colCount);
                columnHeaderCell.SetCellValue(hlevel.HLevelDesc);
                columnHeaderCell.CellStyle = reportHeaderStyle;
            }

            foreach (DataRow row in sortedTable.Rows)
            {
                rowCount++;

                NPOI.HSSF.UserModel.HSSFRow detailRow = (NPOI.HSSF.UserModel.HSSFRow)worksheet.CreateRow(rowCount);
                //worksheet.Cells.Add(rowCount, (ushort)1, row["TrainingCourseName"]);
                //worksheet.Cells.Add(rowCount, (ushort)2, row["TrainingSeminarDesc"]);
                detailRow.CreateCell(0).SetCellValue(row["TrainingCourseName"].ToString());
                detailRow.CreateCell(1).SetCellValue(row["TrainingSeminarDesc"].ToString());

                if (row["TrainingSeminarDateFrom"] != DBNull.Value)
                {
                    DateTime trainingFrom = ((DateTime)row["TrainingSeminarDateFrom"]);
                    DateTime trainingTo = ((DateTime)row["TrainingSeminarDateTo"]);
                    if (trainingFrom.Equals(trainingTo))

                        //worksheet.Cells.Add(rowCount, (ushort)3, trainingFrom.ToString("dd/MM/yyyy"));
                        detailRow.CreateCell(2).SetCellValue(trainingFrom.ToString("dd/MM/yyyy"));
                    else
                        //worksheet.Cells.Add(rowCount, (ushort)3, trainingFrom.ToString("dd/MM/yyyy") + " - " + trainingTo.ToString("dd/MM/yyyy"));
                        detailRow.CreateCell(2).SetCellValue(trainingFrom.ToString("dd/MM/yyyy") + " - " + trainingTo.ToString("dd/MM/yyyy"));

                }

                //worksheet.Cells.Add(rowCount, (ushort)4, row["TrainingSeminarDuration"]);
                //worksheet.Cells.Add(rowCount, (ushort)5, row["TrainingSeminarDurationUnit"]);
                //worksheet.Cells.Add(rowCount, (ushort)6, row["TrainingSeminarTrainer"]);
                //worksheet.Cells.Add(rowCount, (ushort)7, row["EmpNo"]);
                //worksheet.Cells.Add(rowCount, (ushort)8, row["EmpName"]);
                //worksheet.Cells.Add(rowCount, (ushort)9, row["EmpAlias"]);
                //worksheet.Cells.Add(rowCount, (ushort)10, row["Position"]);
                //worksheet.Cells.Add(rowCount, (ushort)11, row["Company"]);
                detailRow.CreateCell(3).SetCellValue((double)row["TrainingSeminarDuration"]);
                //detailRow.GetCell(3).CellStyle = numericStyle;
                detailRow.CreateCell(4).SetCellValue(row["TrainingSeminarDurationUnit"].ToString());
                detailRow.CreateCell(5).SetCellValue(row["TrainingSeminarTrainer"].ToString());
                detailRow.CreateCell(6).SetCellValue(row["EmpNo"].ToString());
                detailRow.CreateCell(7).SetCellValue(row["EmpName"].ToString());
                detailRow.CreateCell(8).SetCellValue(row["EmpAlias"].ToString());
                detailRow.CreateCell(9).SetCellValue(row["Position"].ToString());
                detailRow.CreateCell(10).SetCellValue(row["Company"].ToString());

                colCount = 10;
                foreach (EHierarchyLevel hlevel in hierarchyLevelList)
                {
                    colCount++;
                    if (row[hlevel.HLevelDesc] != DBNull.Value)
                        //worksheet.Cells.Add(rowCount, colCount, row[hlevel.HLevelDesc]);
                        detailRow.CreateCell(colCount).SetCellValue(row[hlevel.HLevelDesc].ToString());
                }

            }
        }
        string exportFileName = System.IO.Path.GetTempFileName();
        System.IO.File.Delete(exportFileName);
        exportFileName += ".xls";
        //document.FileName = exportFileName;
        //document.Save();
        System.IO.FileStream file = new System.IO.FileStream(exportFileName, System.IO.FileMode.Create);
        workbook.Write(file);
        file.Close();
        string filename = "TrainingReport_" + AppUtils.ServerDateTime().ToString("yyyyMMddHHmmss") + ".xls";
        WebUtils.TransmitFile(Response, exportFileName, filename, true);
        return;

    }
}
