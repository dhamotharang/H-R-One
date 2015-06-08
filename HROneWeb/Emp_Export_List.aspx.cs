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

public partial class Emp_Export_List : HROneWebPage
{
    private const string FUNCTION_CODE = "PER900";
    private const string FUNCTION_CHECKBOX_PREFIX = "FUNCTION_";
    protected SearchBinding empSBinding, sbinding;
    public DBManager db = EEmpPersonalInfo.db;
    public EPayrollGroup obj;

    protected ListInfo empInfo;
    protected DataView empView;

    private string[] export_Function_List = new string[]
    {
        "PER001",
        "PER002",
        "PER003",
        "PER004",
        "PER005",
        "PER006",
        "PER007",
        "PER007_1",
        "PER008",
        "PER009",
        "PER010", 
        "PER011",
        "PER012",
        "PER013",
        "PER015",
        "PER016",
        "PER017",
        "PER018", 
        "PER019"
        // Start 0000070, Miranda, 2014-09-08
        ,"PER020"
        ,"PER021"
        // End 0000070, Miranda, 2014-09-08
        // Start 0000196, KuangWei, 2015-05-22
        ,"PER022"
        // End 0000196, KuangWei, 2015-05-22
    };


    //WFValueList functionList = new AppUtils.NewWFTextList(
    //    new string[] { 
    //        "PER001", 
    //        "PER002", 
    //        "PER003" 
    //    },
    //    new string[] { 
    //        "Personal Information", 
    //        "Position Information", 
    //        "Terminated" }
    //    );
    
    protected void Page_Load(object sender, EventArgs e)
    {
       
        if (!WebUtils.CheckAccess(Response, Session, FUNCTION_CODE, WebUtils.AccessLevel.Read))
            return;

        foreach (string functionCode in export_Function_List)
        {
            CheckBox functionCheckBox = (CheckBox)this.FunctionList.FindControl(FUNCTION_CHECKBOX_PREFIX + functionCode);
            if (!WebUtils.CheckPermission(Session, functionCode.Replace("_", "-"), WebUtils.AccessLevel.Read))
            {
                functionCheckBox.Visible = false;
                functionCheckBox.Checked = false;
            }
            else
                functionCheckBox.Visible = true;
        }

        empSBinding = new SearchBinding(dbConn, EEmpPersonalInfo.db);
        empSBinding.initValues("EmpStatus", null, EEmpPersonalInfo.VLEmpStatus, HROne.Common.WebUtility.GetSessionUICultureInfo(Session));
        empSBinding.init(DecryptedRequest, null);

        HROne.Common.WebUtility.WebControlsLocalization(this, this.Controls);

        empInfo = ListFooter.ListInfo;
        empInfo.recordPerPage = 0;
    }

    protected void Page_PreRender(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            empView = emploadData(empInfo, EEmpPayroll.db, empRepeater);
        }

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
        filter.add(new Exists(EEmpPersonalInfo.db.dbclass.tableName + " ee", empInfoFilter));
        DataTable table = filter.loadData(dbConn, null, select, from);
        table = EmployeeSearchControl1.FilterEncryptedEmpInfoField(table, info);

        ListFooter.Refresh();

        if (table.Rows.Count != 0)
        {
            btnGenerate.Visible = true;
        }
        else
        {
            btnGenerate.Visible = false;
        }
        empView = new DataView(table);

        if (repeater != null)
        {
            repeater.DataSource = empView;
            repeater.DataBind();
        }

        return empView;
    }
    protected void empFirstPage_Click(object sender, EventArgs e)
    {
        empView = emploadData(empInfo, db, empRepeater);

    }
    protected void empPrevPage_Click(object sender, EventArgs e)
    {
        empView = emploadData(empInfo, db, empRepeater);

    }
    protected void empNextPage_Click(object sender, EventArgs e)
    {
        empView = emploadData(empInfo, db, empRepeater);

    }
    protected void empLastPage_Click(object sender, EventArgs e)
    {
        empView = emploadData(empInfo, db, empRepeater);

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

        empView = emploadData(empInfo, db, empRepeater);

    }
    protected void Reset_Click(object sender, EventArgs e)
    {

        empView = emploadData(empInfo, db, empRepeater);
    }

    protected void btnGenerate_Click(object sender, EventArgs e)
    {
        PageErrors errors = PageErrors.getErrors(db, Page.Master);
        errors.clear();

        ArrayList functionList = new ArrayList();
        foreach (string functionCode in export_Function_List)
        {
            CheckBox functionCheckBox = (CheckBox)this.FunctionList.FindControl(FUNCTION_CHECKBOX_PREFIX + functionCode);

            if (functionCheckBox.Checked && functionCheckBox.Visible)
                functionList.Add(functionCode);
        }


        if (functionList.Count>0)
        {

            ArrayList list = new ArrayList();
            list = WebUtils.SelectedRepeaterItemToBaseObjectList(EEmpPersonalInfo.db, empRepeater, "ItemSelect");

            //if (list.Count > 0)
            {
                GenerateEmployeeExcel(list, functionList);
            }
        }
        emploadData(empInfo, db, empRepeater);
    }

    private void GenerateEmployeeExcel(ArrayList employeeList, ArrayList functionList)
    {
        PageErrors errors = PageErrors.getErrors(db, Page.Master);
        errors.clear();

        HROne.Import.EmployeeInformationExportProcess reportProcess = new HROne.Import.EmployeeInformationExportProcess(dbConn, ci, employeeList, functionList, IncludedEmployeeNameHierarchy.Checked, DisplayCodeOnly.Checked, ShowInternalID.Checked);

        if (Response.IsClientConnected)
        {
            HROneConfig config = HROneConfig.GetCurrentHROneConfig();
            if (config.GenerateReportAsInbox)
            {
                if (EInboxAttachment.GetTotalSize(dbConn, 0) < WebUtils.productLicense(Session).MaxInboxSizeMB * 1000 * 1000)
                {
                    HROne.TaskService.GenericExcelReportTaskFactory reportTask = new HROne.TaskService.GenericExcelReportTaskFactory(dbConn, user, lblReportHeader.Text, reportProcess, "EmployeeExport");
                    AppUtils.reportTaskQueueService.AddTask(reportTask);
                    errors.addError(HROne.Translation.PageMessage.REPORT_GENERATING_TO_INBOX);
                }
                else
                    errors.addError(HROne.Translation.PageMessage.INBOX_SIZE_EXCEEDED);
            }
            else
            {
                System.IO.FileInfo excelFile = reportProcess.GenerateExcelReport();
                WebUtils.TransmitFile(Response, excelFile.FullName, "EmployeeExport_" + AppUtils.ServerDateTime().ToString("yyyyMMddHHmmss") + ".xls", true);
            }
        }

        //DataSet excelDataSet= new DataSet();

        //foreach (string functionCode in functionList)
        //{
        //    if (functionCode.Equals("PER001"))
        //        excelDataSet.Tables.Add(HROne.Import.ImportEmpPersonalInfoProcess.Export(dbConn, employeeList, IncludedEmployeeNameHierarchy.Checked));
        //    if (functionCode.Equals("PER002"))
        //        excelDataSet.Tables.Add(HROne.Import.ImportEmpBankAccountProcess.Export(dbConn, employeeList, IncludedEmployeeNameHierarchy.Checked));
        //    if (functionCode.Equals("PER003"))
        //    {
        //        excelDataSet.Tables.Add(HROne.Import.ImportEmpSpouseProcess.Export(dbConn, employeeList, IncludedEmployeeNameHierarchy.Checked));
        //        excelDataSet.Tables.Add(HROne.Import.ImportEmpDependantProcess.Export(dbConn, employeeList, IncludedEmployeeNameHierarchy.Checked));
        //    }
        //    if (functionCode.Equals("PER004"))
        //    {
        //        excelDataSet.Tables.Add(HROne.Import.ImportEmpQualificationProcess.Export(dbConn, employeeList, IncludedEmployeeNameHierarchy.Checked, !DisplayCodeOnly.Checked));
        //        excelDataSet.Tables.Add(HROne.Import.ImportEmpSkillProcess.Export(dbConn, employeeList, IncludedEmployeeNameHierarchy.Checked, !DisplayCodeOnly.Checked));
        //    }
        //    if (functionCode.Equals("PER005"))
        //        excelDataSet.Tables.Add(HROne.Import.ImportEmpPlaceOfResidenceProcess.Export(dbConn, employeeList, IncludedEmployeeNameHierarchy.Checked));
        //    if (functionCode.Equals("PER006"))
        //        excelDataSet.Tables.Add(HROne.Import.ImportEmpContractTermsProcess.Export(dbConn, employeeList, IncludedEmployeeNameHierarchy.Checked));
        //    if (functionCode.Equals("PER007"))
        //        excelDataSet.Tables.Add(HROne.Import.ImportEmpPositionInfoProcess.Export(dbConn, employeeList, IncludedEmployeeNameHierarchy.Checked, !DisplayCodeOnly.Checked));
        //    if (functionCode.Equals("PER007_1"))
        //        excelDataSet.Tables.Add(HROne.Import.ImportEmpRecurringPaymentProcess.Export(dbConn, employeeList, IncludedEmployeeNameHierarchy.Checked, !DisplayCodeOnly.Checked));
        //    if (functionCode.Equals("PER008"))
        //    {
        //        excelDataSet.Tables.Add(HROne.Import.ImportEmpMPFPlanProcess.Export(dbConn, employeeList, IncludedEmployeeNameHierarchy.Checked, !DisplayCodeOnly.Checked));
        //        excelDataSet.Tables.Add(HROne.Import.ImportEmpAVCPlanProcess.Export(dbConn, employeeList, IncludedEmployeeNameHierarchy.Checked, !DisplayCodeOnly.Checked));
        //        excelDataSet.Tables.Add(HROne.Import.ImportEmpORSOPlanProcess.Export(dbConn, employeeList, IncludedEmployeeNameHierarchy.Checked, !DisplayCodeOnly.Checked));
        //    }
        //    if (functionCode.Equals("PER009"))
        //        excelDataSet.Tables.Add(HROne.Import.ImportLeaveApplicationProcess.Export(dbConn, employeeList, IncludedEmployeeNameHierarchy.Checked, !DisplayCodeOnly.Checked));
        //    if (functionCode.Equals("PER011"))
        //    {
        //        excelDataSet.Tables.Add(HROne.Import.ImportEmpTerminationProcess.Export(dbConn, employeeList, IncludedEmployeeNameHierarchy.Checked, !DisplayCodeOnly.Checked));
        //        excelDataSet.Tables.Add(HROne.Import.ImportEmpFinalPaymentProcess.Export(dbConn, employeeList, IncludedEmployeeNameHierarchy.Checked, !DisplayCodeOnly.Checked));
        //    }
        //    if (functionCode.Equals("PER012"))
        //        excelDataSet.Tables.Add(HROne.Import.ImportEmpCostCenterProcess.Export(dbConn, employeeList, IncludedEmployeeNameHierarchy.Checked, !DisplayCodeOnly.Checked));
        //    if (functionCode.Equals("PER013"))
        //        excelDataSet.Tables.Add(HROne.Import.ImportEmpPermitProcess.Export(dbConn, employeeList, IncludedEmployeeNameHierarchy.Checked, !DisplayCodeOnly.Checked));
        //    if (functionCode.Equals("PER015"))
        //        excelDataSet.Tables.Add(HROne.Import.ImportEmpEmergencyContactProcess.Export(dbConn, employeeList, IncludedEmployeeNameHierarchy.Checked));
        //    if (functionCode.Equals("PER016"))
        //        excelDataSet.Tables.Add(HROne.Import.ImportEmpWorkExpProcess.Export(dbConn, employeeList, IncludedEmployeeNameHierarchy.Checked, !DisplayCodeOnly.Checked));
        //    if (functionCode.Equals("PER017"))
        //        excelDataSet.Tables.Add(HROne.Import.ImportEmpWorkInjuryRecordProcess.Export(dbConn, employeeList, IncludedEmployeeNameHierarchy.Checked));
            
        //}

        //string exportFileName = System.IO.Path.GetTempFileName();
        //System.IO.File.Delete(exportFileName);
        //exportFileName += ".xls";
        //HROne.Export.ExcelExport excelExport = new HROne.Export.ExcelExport(exportFileName);
        //excelExport.Update(excelDataSet);
        //WebUtils.TransmitFile(Response, exportFileName, "EmployeeExport_" + AppUtils.ServerDateTime().ToString("yyyyMMddHHmmss") + ".xls", true);
        //return;
    }
}
