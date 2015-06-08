using System;
using System.Collections;
using System.Text;
using HROne.Common;
using HROne.DataAccess;

namespace HROne.Import
{
    public class EmployeeInformationExportProcess : GenericExcelReportProcess 
    {
        protected ArrayList employeeList;
        protected IList functionList;
        protected bool IsIncludedEmployeeNameHierarchy;
        protected bool IsDisplayCodeOnly;
        protected bool IsShowSyncID;
        protected string exportFileName;
        protected DateTime ReferenceAfterDateTime;
        protected bool IsDataTransfer;

        public EmployeeInformationExportProcess(DatabaseConnection dbConn, System.Globalization.CultureInfo reportCultureInfo, ArrayList employeeList, IList functionList, bool IsIncludedEmployeeNameHierarchy, bool IsDisplayCodeOnly)
            : this(dbConn, reportCultureInfo, employeeList, functionList, IsIncludedEmployeeNameHierarchy, IsDisplayCodeOnly, false, new DateTime(), false)
        {
        }
        public EmployeeInformationExportProcess(DatabaseConnection dbConn, System.Globalization.CultureInfo reportCultureInfo, ArrayList employeeList, IList functionList, bool IsIncludedEmployeeNameHierarchy, bool IsDisplayCodeOnly, bool IsShowSyncID)
            : this(dbConn, reportCultureInfo, employeeList, functionList, IsIncludedEmployeeNameHierarchy, IsDisplayCodeOnly, IsShowSyncID, new DateTime(), false)
        {
        }
        public EmployeeInformationExportProcess(DatabaseConnection dbConn, System.Globalization.CultureInfo reportCultureInfo, ArrayList employeeList, IList functionList, bool IsIncludedEmployeeNameHierarchy, bool IsDisplayCodeOnly, bool IsShowSyncID, DateTime ReferenceAfterDateTime)
            : this(dbConn, reportCultureInfo, employeeList, functionList, IsIncludedEmployeeNameHierarchy, IsDisplayCodeOnly, IsShowSyncID, ReferenceAfterDateTime, false)
        {
        }
        public EmployeeInformationExportProcess(DatabaseConnection dbConn, System.Globalization.CultureInfo reportCultureInfo, ArrayList employeeList, IList functionList, bool IsIncludedEmployeeNameHierarchy, bool IsDisplayCodeOnly, bool IsShowSyncID, DateTime ReferenceAfterDateTime, bool IsDataTransfer)
            : base(dbConn, reportCultureInfo)
        {
            this.employeeList = employeeList;
            this.functionList = functionList;
            this.IsIncludedEmployeeNameHierarchy = IsIncludedEmployeeNameHierarchy;
            this.IsDisplayCodeOnly = IsDisplayCodeOnly;
            this.IsShowSyncID = IsShowSyncID;
            this.ReferenceAfterDateTime = ReferenceAfterDateTime;
            this.IsDataTransfer = IsDataTransfer;
        }
        protected override System.Data.DataSet CreateDataSource()
        {
            System.Data.DataSet excelDataSet = new System.Data.DataSet();

            foreach (string functionCode in functionList)
            {
                if (functionCode.Equals("PER001"))
                    excelDataSet.Tables.Add(HROne.Import.ImportEmpPersonalInfoProcess.Export(dbConn, employeeList, IsIncludedEmployeeNameHierarchy, IsShowSyncID, ReferenceAfterDateTime));
                if (functionCode.Equals("PER002"))
                    excelDataSet.Tables.Add(HROne.Import.ImportEmpBankAccountProcess.Export(dbConn, employeeList, IsIncludedEmployeeNameHierarchy, IsShowSyncID, ReferenceAfterDateTime));
                if (functionCode.Equals("PER003"))
                {
                    excelDataSet.Tables.Add(HROne.Import.ImportEmpSpouseProcess.Export(dbConn, employeeList, IsIncludedEmployeeNameHierarchy, IsShowSyncID, ReferenceAfterDateTime));
                    excelDataSet.Tables.Add(HROne.Import.ImportEmpDependantProcess.Export(dbConn, employeeList, IsIncludedEmployeeNameHierarchy, IsShowSyncID, ReferenceAfterDateTime));
                }
                if (functionCode.Equals("PER004"))
                {
                    excelDataSet.Tables.Add(HROne.Import.ImportEmpQualificationProcess.Export(dbConn, employeeList, IsIncludedEmployeeNameHierarchy, !IsDisplayCodeOnly, IsShowSyncID, ReferenceAfterDateTime));
                    excelDataSet.Tables.Add(HROne.Import.ImportEmpSkillProcess.Export(dbConn, employeeList, IsIncludedEmployeeNameHierarchy, !IsDisplayCodeOnly, IsShowSyncID, ReferenceAfterDateTime));
                }
                if (functionCode.Equals("PER005"))
                    excelDataSet.Tables.Add(HROne.Import.ImportEmpPlaceOfResidenceProcess.Export(dbConn, employeeList, IsIncludedEmployeeNameHierarchy, IsShowSyncID, ReferenceAfterDateTime));
                if (functionCode.Equals("PER006"))
                    excelDataSet.Tables.Add(HROne.Import.ImportEmpContractTermsProcess.Export(dbConn, employeeList, IsIncludedEmployeeNameHierarchy, IsShowSyncID, ReferenceAfterDateTime));
                if (functionCode.Equals("PER007"))
                    excelDataSet.Tables.Add(HROne.Import.ImportEmpPositionInfoProcess.Export(dbConn, employeeList, IsIncludedEmployeeNameHierarchy, !IsDisplayCodeOnly, IsShowSyncID, ReferenceAfterDateTime));
                if (functionCode.Equals("PER007_1"))
                    excelDataSet.Tables.Add(HROne.Import.ImportEmpRecurringPaymentProcess.Export(dbConn, employeeList, IsIncludedEmployeeNameHierarchy, !IsDisplayCodeOnly, IsShowSyncID, ReferenceAfterDateTime));
                if (functionCode.Equals("PER008"))
                {
                    excelDataSet.Tables.Add(HROne.Import.ImportEmpMPFPlanProcess.Export(dbConn, employeeList, IsIncludedEmployeeNameHierarchy, !IsDisplayCodeOnly, IsShowSyncID, ReferenceAfterDateTime));
                    excelDataSet.Tables.Add(HROne.Import.ImportEmpAVCPlanProcess.Export(dbConn, employeeList, IsIncludedEmployeeNameHierarchy, !IsDisplayCodeOnly, IsShowSyncID, ReferenceAfterDateTime));
                    excelDataSet.Tables.Add(HROne.Import.ImportEmpORSOPlanProcess.Export(dbConn, employeeList, IsIncludedEmployeeNameHierarchy, !IsDisplayCodeOnly, IsShowSyncID, ReferenceAfterDateTime));
                }
                if (functionCode.Equals("PER009"))
                    excelDataSet.Tables.Add(HROne.Import.ImportLeaveApplicationProcess.Export(dbConn, employeeList, IsIncludedEmployeeNameHierarchy, !IsDisplayCodeOnly, IsShowSyncID, ReferenceAfterDateTime));
                if (functionCode.Equals("PER011"))
                {
                    excelDataSet.Tables.Add(HROne.Import.ImportEmpTerminationProcess.Export(dbConn, employeeList, IsIncludedEmployeeNameHierarchy, !IsDisplayCodeOnly, IsShowSyncID, ReferenceAfterDateTime));
                    // Start 0000196, KuangWei, 2015-05-22
                    //if (!IsDataTransfer)
                    //{
                    //    excelDataSet.Tables.Add(HROne.Import.ImportEmpFinalPaymentProcess.Export(dbConn, employeeList, IsIncludedEmployeeNameHierarchy, !IsDisplayCodeOnly, IsShowSyncID, ReferenceAfterDateTime));
                    //}
                    // End 0000196, KuangWei, 2015-05-22
                }
                if (functionCode.Equals("PER012"))
                    excelDataSet.Tables.Add(HROne.Import.ImportEmpCostCenterProcess.Export(dbConn, employeeList, IsIncludedEmployeeNameHierarchy, !IsDisplayCodeOnly, IsShowSyncID, ReferenceAfterDateTime));
                if (functionCode.Equals("PER013"))
                    excelDataSet.Tables.Add(HROne.Import.ImportEmpPermitProcess.Export(dbConn, employeeList, IsIncludedEmployeeNameHierarchy, !IsDisplayCodeOnly, IsShowSyncID, ReferenceAfterDateTime));
                if (functionCode.Equals("PER015"))
                    excelDataSet.Tables.Add(HROne.Import.ImportEmpEmergencyContactProcess.Export(dbConn, employeeList, IsIncludedEmployeeNameHierarchy, IsShowSyncID, ReferenceAfterDateTime));
                if (functionCode.Equals("PER016"))
                    excelDataSet.Tables.Add(HROne.Import.ImportEmpWorkExpProcess.Export(dbConn, employeeList, IsIncludedEmployeeNameHierarchy, !IsDisplayCodeOnly, IsShowSyncID, ReferenceAfterDateTime));
                if (functionCode.Equals("PER017"))
                    excelDataSet.Tables.Add(HROne.Import.ImportEmpWorkInjuryRecordProcess.Export(dbConn, employeeList, IsIncludedEmployeeNameHierarchy, IsShowSyncID, ReferenceAfterDateTime));
                if (functionCode.Equals("PER019"))
                    excelDataSet.Tables.Add(HROne.Import.ImportEmpRosterTableGroupProcess.Export(dbConn, employeeList, IsIncludedEmployeeNameHierarchy, !IsDisplayCodeOnly, IsShowSyncID, ReferenceAfterDateTime));
                if (functionCode.Equals("PER010"))
                    excelDataSet.Tables.Add(HROne.Import.ImportLeaveBalanceAdjustmentProcess.Export(dbConn, employeeList, IsIncludedEmployeeNameHierarchy, !IsDisplayCodeOnly, IsShowSyncID, ReferenceAfterDateTime));
                if (functionCode.Equals("PER018"))
                    excelDataSet.Tables.Add(HROne.Import.ImportCompensationLeaveEntitleProcess.Export(dbConn, employeeList, IsIncludedEmployeeNameHierarchy, !IsDisplayCodeOnly, IsShowSyncID, ReferenceAfterDateTime));
                // Start 0000070, Miranda, 2014-09-08
                if (functionCode.Equals("PER020"))
                    excelDataSet.Tables.Add(HROne.Import.ImportEmpBenefitProcess.Export(dbConn, employeeList, IsIncludedEmployeeNameHierarchy, !IsDisplayCodeOnly, IsShowSyncID, ReferenceAfterDateTime));
                if (functionCode.Equals("PER021"))
                    excelDataSet.Tables.Add(HROne.Import.ImportEmpBeneficiariesProcess.Export(dbConn, employeeList, IsIncludedEmployeeNameHierarchy, !IsDisplayCodeOnly, IsShowSyncID, ReferenceAfterDateTime));
                // End 0000070, Miranda, 2014-09-08
                // Start 0000196, KuangWei, 2015-05-22
                if (functionCode.Equals("PER022"))
                {
                    excelDataSet.Tables.Add(HROne.Import.ImportEmpFinalPaymentProcess.Export(dbConn, employeeList, IsIncludedEmployeeNameHierarchy, !IsDisplayCodeOnly, IsShowSyncID, ReferenceAfterDateTime));
                }
                // End 0000196, KuangWei, 2015-05-22
            }
            return excelDataSet;
        }
        protected override System.IO.FileInfo SaveToFile(NPOI.HSSF.UserModel.HSSFWorkbook workBook)
        {

            return new System.IO.FileInfo(exportFileName);
        }
        protected override void GenerateWorkbookDetail(NPOI.HSSF.UserModel.HSSFWorkbook workBook, System.Data.DataSet dataSet)
        {
            exportFileName = System.IO.Path.GetTempFileName();
            System.IO.File.Delete(exportFileName);
            exportFileName += ".xls";
            HROne.Export.ExcelExport export = new HROne.Export.ExcelExport(exportFileName);
            export.Update(dataSet);

        }
        protected override void CreateWorkBookStyle(NPOI.HSSF.UserModel.HSSFWorkbook workBook)
        {
        }
    }
}
