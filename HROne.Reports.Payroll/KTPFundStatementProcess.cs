using System;
using System.Collections;
using System.Text;
using CrystalDecisions.CrystalReports.Engine;
using HROne.Lib.Entities;
using HROne.Common;
using HROne.DataAccess;
using HROne.Reports.Payroll.DataSet;
using System.Data;
using HROne.CommonLib;


namespace HROne.Reports.Payroll
{
    public class KTPFundStatementProcess : GenericReportProcess 
    {
        private DateTime _payPeriodFr, _payPeriodTo;
        private ArrayList EmpList;
        private int ORSOPlanID;

        public KTPFundStatementProcess(DatabaseConnection dbConn, ArrayList EmpList, int ORSOPlanID, DateTime PayPeriodFr, DateTime PayPeriodTo)
            : base(dbConn)
        {
            this.EmpList = EmpList;
            this._payPeriodFr = PayPeriodFr;
            this._payPeriodTo = PayPeriodTo;
            this.ORSOPlanID = ORSOPlanID;
        }

        protected DataSet.Payroll_KTPFundStatement CreateDataSource()
        {
            DataSet.Payroll_KTPFundStatement dataSet = new DataSet.Payroll_KTPFundStatement();

            Payroll_KTPFundStatement.ExistingMemberDataTable existingORSO = dataSet.ExistingMember;

            Payroll_KTPFundStatement.ORSOPlanDataTable orsoPlan = dataSet.ORSOPlan;

            if (_payPeriodFr.Ticks != 0 && _payPeriodTo.Ticks != 0 && EmpList != null)
            {
                string strPrintPeriod = _payPeriodFr.ToString("yyyy-MM-dd") + " - " + _payPeriodTo.ToString("yyyy-MM-dd");

                DBFilter payPeriodFilter = new DBFilter();
                payPeriodFilter.add(new Match("pp.PayPeriodFr", "<=", _payPeriodTo));
                payPeriodFilter.add(new Match("pp.PayPeriodTo", ">=", _payPeriodFr));

                foreach (EEmpPersonalInfo empInfo in EmpList)
                {
                    EEmpPersonalInfo.db.select(dbConn, empInfo);

                    DBFilter empPayrollFilter = new DBFilter();
                    empPayrollFilter.add(new Match("EmpID", empInfo.EmpID));
                    empPayrollFilter.add(new IN("PayPeriodID", "Select PayPeriodID from PayrollPeriod pp", payPeriodFilter));


                    DBFilter orsoRecordFilter = new DBFilter();
                    orsoRecordFilter.add(new IN("EmpPayrollID", "Select EmpPayrollID from EmpPayroll", empPayrollFilter));
                    orsoRecordFilter.add(new Match("ORSOPlanID", ORSOPlanID));
                    orsoRecordFilter.add("ORSORecPeriodFr", true);
                    orsoRecordFilter.add("ORSORecPeriodTo", true);
                    ArrayList orsoRecords = EORSORecord.db.select(dbConn, orsoRecordFilter);


                    DataSet.Payroll_KTPFundStatement.ExistingMemberRow existingORSORow = null;
                    foreach (EORSORecord orsoRecord in orsoRecords)
                    {
                        EORSOPlan orsoPlanObject = new EORSOPlan();
                        orsoPlanObject.ORSOPlanID = orsoRecord.ORSOPlanID;
                        if (EORSOPlan.db.select(dbConn, orsoPlanObject))
                        {
                            if (orsoPlan.Select("ORSOPlanID=" + orsoPlanObject.ORSOPlanID).Length == 0)
                            {
                                DataSet.Payroll_KTPFundStatement.ORSOPlanRow orsoPlanRow = orsoPlan.NewORSOPlanRow();
                                orsoPlanRow.ORSOPlanID = orsoPlanObject.ORSOPlanID;
                                orsoPlanRow.ORSOPlanCode = orsoPlanObject.ORSOPlanCode;
                                orsoPlanRow.ORSOPlanCompanyName = orsoPlanObject.ORSOPlanCompanyName;
                                orsoPlanRow.ORSOPlanPayCenter = orsoPlanObject.ORSOPlanPayCenter;
                                orsoPlanRow.ORSOPlanSchemeNo = orsoPlanObject.ORSOPlanSchemeNo;
                                orsoPlanRow.ORSOPlanDesc = orsoPlanObject.ORSOPlanDesc;

                                orsoPlan.Rows.Add(orsoPlanRow);
                            }
                        }

                        EEmpPayroll empPayroll = new EEmpPayroll();
                        empPayroll.EmpPayrollID = orsoRecord.EmpPayrollID;
                        EEmpPayroll.db.select(dbConn, empPayroll);
                        EPayrollPeriod payrollPeriod = new EPayrollPeriod();
                        payrollPeriod.PayPeriodID = empPayroll.PayPeriodID;
                        EPayrollPeriod.db.select(dbConn, payrollPeriod);

                        {
                            if (existingORSORow == null)
                            {
                                existingORSORow = existingORSO.NewExistingMemberRow();
                                LoadExistingMemberRowInfo(empInfo, orsoRecord, existingORSORow);

                            }
                            else
                            {
                                if (!(existingORSORow.EmpID == empInfo.EmpID && existingORSORow.PeriodFrom.Equals(orsoRecord.ORSORecPeriodFr) && existingORSORow.PeriodTo.Equals(orsoRecord.ORSORecPeriodTo)))
                                {
                                    existingORSO.Rows.Add(existingORSORow);
                                    existingORSORow = existingORSO.NewExistingMemberRow();
                                    LoadExistingMemberRowInfo(empInfo, orsoRecord, existingORSORow);
                                }
                            }

                            existingORSORow.RelevantIncome += orsoRecord.ORSORecActRI;
                            existingORSORow.EE += orsoRecord.ORSORecActEE;
                            existingORSORow.ER += orsoRecord.ORSORecActER;

                            DBFilter m_mpfRecordFilter = new DBFilter();
                            m_mpfRecordFilter.add(new Match("EmpPayrollID", orsoRecord.EmpPayrollID));
                            foreach (EMPFRecord m_mpfRecord in EMPFRecord.db.select(dbConn, m_mpfRecordFilter))
                            {
                                existingORSORow.MpfMCEE += m_mpfRecord.MPFRecActMCEE;
                                existingORSORow.MpfMCER += m_mpfRecord.MPFRecActMCER;
                                existingORSORow.MpfVCEE += m_mpfRecord.MPFRecActVCEE;
                                existingORSORow.MpfVCER += m_mpfRecord.MPFRecActVCER;
                            }
                        }
                    }

                    if (existingORSORow != null)
                    {
                        // business requirement from user, "Basic Salary over $100,000 should be shown $100,000 only
                        if (existingORSORow.RelevantIncome > 100000)
                            existingORSORow.RelevantIncome = 100000;

                        existingORSO.Rows.Add(existingORSORow);
                    }
                }
            }

            return dataSet;
        }

        public override ReportDocument GenerateReport()
        {

            DataSet.Payroll_KTPFundStatement dataSet = CreateDataSource();

            if (reportDocument == null)
            {
                reportDocument = new ReportTemplate.Report_Payroll_KTPFundStatement();
            }

            reportDocument.SetDataSource(dataSet);
            
            if (_payPeriodFr.Month != _payPeriodTo.Month)            
            {            
                reportDocument.SetParameterValue("ContributionPeriod", _payPeriodFr.ToString("dd MMMM yyyy") + " - " + _payPeriodTo.ToString("dd MMMM yyyy"));                
            }
            else
            {
                reportDocument.SetParameterValue("ContributionPeriod", _payPeriodFr.ToString("MMMM yyyy"));
            }
            return reportDocument;
        }

        private void LoadExistingMemberRowInfo(EEmpPersonalInfo empInfo, EORSORecord mpfRecord, DataSet.Payroll_KTPFundStatement.ExistingMemberRow existingORSORow)
        {
            existingORSORow.EmpID = empInfo.EmpID;
            existingORSORow.EmpNo = empInfo.EmpNo;
            existingORSORow.MemberID = "";
            //existingORSORow.HKID = empInfo.EmpHKID.Length < 7 ? empInfo.EmpPassportNo : empInfo.EmpHKID;
            existingORSORow.EmpName = empInfo.EmpEngFullName;
            existingORSORow.EmpSex = empInfo.EmpGender;
            existingORSORow.EmpDOB = empInfo.EmpDateOfBirth;
            existingORSORow.EmpDateJoin = empInfo.EmpDateOfJoin;
            existingORSORow.PeriodFrom = mpfRecord.ORSORecPeriodFr;
            existingORSORow.PeriodTo = mpfRecord.ORSORecPeriodTo;
            existingORSORow.ORSOPlanID = mpfRecord.ORSOPlanID;
            existingORSORow.RelevantIncome = 0;
            existingORSORow.EE = 0;
            existingORSORow.EE = 0;
            existingORSORow.ER = 0;
            existingORSORow.ER = 0;
            existingORSORow.MpfMCEE = 0;
            existingORSORow.MpfMCER = 0;
            existingORSORow.MpfVCEE = 0;
            existingORSORow.MpfVCER = 0;
            existingORSORow.TermCode = "";


            DBFilter empTerminationFilter = new DBFilter();
            empTerminationFilter.add(new Match("EmpID", empInfo.EmpID));
            empTerminationFilter.add(new Match("EmpTermLastDate", "<=", mpfRecord.ORSORecPeriodTo));
            empTerminationFilter.add(new Match("EmpTermLastDate", ">=", mpfRecord.ORSORecPeriodFr));
            ArrayList empTerminations = EEmpTermination.db.select(dbConn, empTerminationFilter);
            if (empTerminations.Count > 0)
            {
                EEmpTermination empTermination = (EEmpTermination)empTerminations[0];
                existingORSORow.LastEmploymentDate = empTermination.EmpTermLastDate;

                ECessationReason cessationReason = new ECessationReason();
                cessationReason.CessationReasonID = empTermination.CessationReasonID;
                ECessationReason.db.select(dbConn, cessationReason);
                existingORSORow.TermCode = cessationReason.CessationReasonCode;
            }
            // get member ID

            DBFilter m_empFieldFilter = new DBFilter();
            m_empFieldFilter.add(new Match("EmpExtraFieldName", "P-Fund Member ID"));

            int m_extraFieldID = 0;
            ArrayList m_fieldList = EEmpExtraField.db.select(dbConn, m_empFieldFilter);
            if (m_fieldList.Count > 0)
                m_extraFieldID = ((EEmpExtraField)m_fieldList[0]).EmpExtraFieldID;

            if (m_extraFieldID > 0)
            {
                DBFilter m_empFieldValueFilter = new DBFilter();
                m_empFieldValueFilter.add(new Match("EmpID", empInfo.EmpID));
                m_empFieldValueFilter.add(new Match("EmpExtraFieldID", m_extraFieldID));

                ArrayList m_fieldValueList = EEmpExtraFieldValue.db.select(dbConn, m_empFieldValueFilter);
                if (m_fieldValueList.Count > 0)
                    existingORSORow.MemberID = ((EEmpExtraFieldValue)m_fieldValueList[0]).EmpExtraFieldValue;
            }

            // get P-Fund effective date (i.e. 1st contribution date)
            DBFilter m_empOrsoPlanFilter = new DBFilter();
            m_empOrsoPlanFilter.add(new Match("EmpID", empInfo.EmpID));
            m_empOrsoPlanFilter.add("EmpORSOEffFr", true);

            ArrayList m_orsoPlanList = EEmpORSOPlan.db.select(dbConn, m_empOrsoPlanFilter);
            if (m_orsoPlanList.Count > 0)
            {
                existingORSORow.OrsoEffDate = ((EEmpORSOPlan)m_orsoPlanList[0]).EmpORSOEffFr;
            }
        }


        public void GenerateExcelReport(string exportFileName)
        {
            DataSet.Payroll_KTPFundStatement dataSet = CreateDataSource();

            int lastRowIndex = 0;

            // Set column style
            NPOI.HSSF.UserModel.HSSFWorkbook workbook = new NPOI.HSSF.UserModel.HSSFWorkbook();
            NPOI.HSSF.UserModel.HSSFSheet worksheet = (NPOI.HSSF.UserModel.HSSFSheet)workbook.CreateSheet("KTPF Contribution Report");

            // Date format
            NPOI.HSSF.UserModel.HSSFDataFormat format = (NPOI.HSSF.UserModel.HSSFDataFormat)workbook.CreateDataFormat();
            NPOI.HSSF.UserModel.HSSFCellStyle dateCellStyle = (NPOI.HSSF.UserModel.HSSFCellStyle)workbook.CreateCellStyle();
            dateCellStyle.DataFormat = format.GetFormat("yyyy-MM-dd");
            dateCellStyle.Alignment = NPOI.SS.UserModel.HorizontalAlignment.RIGHT;
            dateCellStyle.BorderBottom = NPOI.SS.UserModel.BorderStyle.DOTTED;
            dateCellStyle.BorderLeft = NPOI.SS.UserModel.BorderStyle.DOTTED;
            dateCellStyle.BorderRight = NPOI.SS.UserModel.BorderStyle.DOTTED;

            // Numeric format
            NPOI.HSSF.UserModel.HSSFCellStyle numericStyle = (NPOI.HSSF.UserModel.HSSFCellStyle)workbook.CreateCellStyle();
            numericStyle.DataFormat = workbook.CreateDataFormat().GetFormat("#,##0.00;(#,##0.00);-");
            numericStyle.Alignment = NPOI.SS.UserModel.HorizontalAlignment.RIGHT;
            numericStyle.BorderBottom = NPOI.SS.UserModel.BorderStyle.DOTTED;
            numericStyle.BorderLeft = NPOI.SS.UserModel.BorderStyle.DOTTED;
            numericStyle.BorderRight = NPOI.SS.UserModel.BorderStyle.DOTTED;

            // String left format
            NPOI.HSSF.UserModel.HSSFCellStyle stringLeftStyle = (NPOI.HSSF.UserModel.HSSFCellStyle)workbook.CreateCellStyle();
            stringLeftStyle.Alignment = NPOI.SS.UserModel.HorizontalAlignment.LEFT;
            stringLeftStyle.BorderBottom = NPOI.SS.UserModel.BorderStyle.DOTTED;
            stringLeftStyle.BorderLeft = NPOI.SS.UserModel.BorderStyle.DOTTED;
            stringLeftStyle.BorderRight = NPOI.SS.UserModel.BorderStyle.DOTTED;

            // String center format
            NPOI.HSSF.UserModel.HSSFCellStyle stringCenterStyle = (NPOI.HSSF.UserModel.HSSFCellStyle)workbook.CreateCellStyle();
            stringCenterStyle.Alignment = NPOI.SS.UserModel.HorizontalAlignment.CENTER;
            stringCenterStyle.BorderBottom = NPOI.SS.UserModel.BorderStyle.DOTTED;
            stringCenterStyle.BorderLeft = NPOI.SS.UserModel.BorderStyle.DOTTED;
            stringCenterStyle.BorderRight = NPOI.SS.UserModel.BorderStyle.DOTTED;

            // Column 0 style
            NPOI.HSSF.UserModel.HSSFCellStyle column0Style = (NPOI.HSSF.UserModel.HSSFCellStyle)workbook.CreateCellStyle();
            column0Style.BorderLeft = NPOI.SS.UserModel.BorderStyle.THIN;
            column0Style.Alignment = NPOI.SS.UserModel.HorizontalAlignment.LEFT;
            column0Style.BorderBottom = NPOI.SS.UserModel.BorderStyle.DOTTED;
            column0Style.BorderRight = NPOI.SS.UserModel.BorderStyle.DOTTED;

            // Column 10, 11 style
            NPOI.HSSF.UserModel.HSSFCellStyle column10To11Style = (NPOI.HSSF.UserModel.HSSFCellStyle)workbook.CreateCellStyle();
            column10To11Style.BorderRight = NPOI.SS.UserModel.BorderStyle.THIN;
            column10To11Style.BorderBottom = NPOI.SS.UserModel.BorderStyle.DOTTED;
            column10To11Style.BorderLeft = NPOI.SS.UserModel.BorderStyle.DOTTED;

            // Column 4 style
            NPOI.HSSF.UserModel.HSSFCellStyle column4Style = (NPOI.HSSF.UserModel.HSSFCellStyle)workbook.CreateCellStyle();
            column4Style.DataFormat = workbook.CreateDataFormat().GetFormat("#,##0.00;(#,##0.00);-");
            column4Style.BorderRight = NPOI.SS.UserModel.BorderStyle.THIN;
            column4Style.BorderBottom = NPOI.SS.UserModel.BorderStyle.DOTTED;
            column4Style.BorderLeft = NPOI.SS.UserModel.BorderStyle.DOTTED;

            // Column 8 style
            NPOI.HSSF.UserModel.HSSFCellStyle column8Style = (NPOI.HSSF.UserModel.HSSFCellStyle)workbook.CreateCellStyle();
            column8Style.DataFormat = format.GetFormat("yyyy-MM-dd");
            column8Style.Alignment = NPOI.SS.UserModel.HorizontalAlignment.RIGHT;
            column8Style.BorderRight = NPOI.SS.UserModel.BorderStyle.THIN;
            column8Style.BorderBottom = NPOI.SS.UserModel.BorderStyle.DOTTED;
            column8Style.BorderLeft = NPOI.SS.UserModel.BorderStyle.DOTTED;

            // Align right
            NPOI.HSSF.UserModel.HSSFCellStyle style = (NPOI.HSSF.UserModel.HSSFCellStyle)workbook.CreateCellStyle();
            style.Alignment = NPOI.SS.UserModel.HorizontalAlignment.RIGHT;

            // Bottom border
            NPOI.HSSF.UserModel.HSSFCellStyle bottomBorderStyle = (NPOI.HSSF.UserModel.HSSFCellStyle)workbook.CreateCellStyle();
            bottomBorderStyle.BorderBottom = NPOI.SS.UserModel.BorderStyle.THIN;

            // Bold style
            NPOI.HSSF.UserModel.HSSFCellStyle boldStyle = (NPOI.HSSF.UserModel.HSSFCellStyle)workbook.CreateCellStyle();
            NPOI.SS.UserModel.IFont boldFont = workbook.CreateFont();
            boldFont.Boldweight = 700;
            boldStyle.SetFont(boldFont);

            // Header Border
            NPOI.HSSF.UserModel.HSSFCellStyle headerBorderStyle = (NPOI.HSSF.UserModel.HSSFCellStyle)workbook.CreateCellStyle();
            headerBorderStyle.BorderBottom = NPOI.SS.UserModel.BorderStyle.THIN;
            headerBorderStyle.BorderTop = NPOI.SS.UserModel.BorderStyle.THIN;
            headerBorderStyle.BorderRight = NPOI.SS.UserModel.BorderStyle.DOTTED;
            headerBorderStyle.SetFont(boldFont);
            headerBorderStyle.WrapText = true;
            headerBorderStyle.Alignment = NPOI.SS.UserModel.HorizontalAlignment.CENTER;

            // Header Left Border
            NPOI.HSSF.UserModel.HSSFCellStyle headerLeftBorderStyle = (NPOI.HSSF.UserModel.HSSFCellStyle)workbook.CreateCellStyle();
            headerLeftBorderStyle.BorderBottom = NPOI.SS.UserModel.BorderStyle.THIN;
            headerLeftBorderStyle.BorderTop = NPOI.SS.UserModel.BorderStyle.THIN;
            headerLeftBorderStyle.BorderLeft = NPOI.SS.UserModel.BorderStyle.THIN;
            headerLeftBorderStyle.BorderRight = NPOI.SS.UserModel.BorderStyle.DOTTED;
            headerLeftBorderStyle.SetFont(boldFont);
            headerLeftBorderStyle.WrapText = true;
            headerLeftBorderStyle.Alignment = NPOI.SS.UserModel.HorizontalAlignment.CENTER;

            // Header Right Border
            NPOI.HSSF.UserModel.HSSFCellStyle headerRightBorderStyle = (NPOI.HSSF.UserModel.HSSFCellStyle)workbook.CreateCellStyle();
            headerRightBorderStyle.BorderBottom = NPOI.SS.UserModel.BorderStyle.THIN;
            headerRightBorderStyle.BorderTop = NPOI.SS.UserModel.BorderStyle.THIN;
            headerRightBorderStyle.BorderRight = NPOI.SS.UserModel.BorderStyle.THIN;
            headerRightBorderStyle.SetFont(boldFont);
            headerRightBorderStyle.WrapText = true;
            headerRightBorderStyle.Alignment = NPOI.SS.UserModel.HorizontalAlignment.CENTER;

            // Total numeric format
            NPOI.HSSF.UserModel.HSSFCellStyle totalNumericStyle = (NPOI.HSSF.UserModel.HSSFCellStyle)workbook.CreateCellStyle();
            totalNumericStyle.DataFormat = workbook.CreateDataFormat().GetFormat("#,##0.00;(#,##0.00);-");
            totalNumericStyle.Alignment = NPOI.SS.UserModel.HorizontalAlignment.RIGHT;
            totalNumericStyle.BorderTop = NPOI.SS.UserModel.BorderStyle.THIN;

            // Grey color
            NPOI.HSSF.UserModel.HSSFCellStyle grey25Style = (NPOI.HSSF.UserModel.HSSFCellStyle)workbook.CreateCellStyle();
            grey25Style.Alignment = NPOI.SS.UserModel.HorizontalAlignment.CENTER;
            grey25Style.FillForegroundColor = NPOI.HSSF.Util.HSSFColor.GREY_25_PERCENT.index;
            grey25Style.FillPattern = NPOI.SS.UserModel.FillPatternType.SOLID_FOREGROUND;
            grey25Style.SetFont(boldFont);

            // Yellow color
            NPOI.HSSF.UserModel.HSSFCellStyle yellowStyle = (NPOI.HSSF.UserModel.HSSFCellStyle)workbook.CreateCellStyle();
            yellowStyle.Alignment = NPOI.SS.UserModel.HorizontalAlignment.CENTER;
            yellowStyle.FillForegroundColor = NPOI.HSSF.Util.HSSFColor.LIGHT_YELLOW.index;
            yellowStyle.FillPattern = NPOI.SS.UserModel.FillPatternType.SOLID_FOREGROUND;
            yellowStyle.SetFont(boldFont);

            // Set column width
            worksheet.SetColumnWidth(0, 12 * 256);
            worksheet.SetColumnWidth(1, 20 * 256);
            worksheet.SetColumnWidth(2, 15 * 256);
            worksheet.SetColumnWidth(3, 15 * 256);
            worksheet.SetColumnWidth(4, 15 * 256);
            worksheet.SetColumnWidth(5, 7 * 256);
            worksheet.SetColumnWidth(6, 15 * 256);
            worksheet.SetColumnWidth(7, 15 * 256);
            worksheet.SetColumnWidth(8, 15 * 256);
            worksheet.SetColumnWidth(9, 15 * 256);
            worksheet.SetColumnWidth(10, 15 * 256);
            worksheet.SetColumnWidth(11, 25 * 256);

            // Set column title
            NPOI.HSSF.UserModel.HSSFRow headerRow = (NPOI.HSSF.UserModel.HSSFRow)worksheet.CreateRow(0);

            NPOI.HSSF.UserModel.HSSFCell headerCell = (NPOI.HSSF.UserModel.HSSFCell)headerRow.CreateCell(0);
            headerCell.SetCellValue("KTPF Contribution Report");
            headerCell.CellStyle = boldStyle;

            headerRow = (NPOI.HSSF.UserModel.HSSFRow)worksheet.CreateRow(1);
            headerCell = (NPOI.HSSF.UserModel.HSSFCell)headerRow.CreateCell(0);
            if (dataSet.ORSOPlan.Rows.Count > 0)
            {
                Payroll_KTPFundStatement.ORSOPlanRow m_orsoPlan = (Payroll_KTPFundStatement.ORSOPlanRow) dataSet.ORSOPlan.Rows[0];

                headerCell.SetCellValue(m_orsoPlan.ORSOPlanCompanyName);
            }
            headerCell.CellStyle = boldStyle;

            string m_reportPeriod = "";
            if (_payPeriodFr.Month != _payPeriodTo.Month)
            {
                m_reportPeriod = _payPeriodFr.ToString("dd MMMM yyyy") + " - " + _payPeriodTo.ToString("dd MMMM yyyy");
            }
            else
            {
                m_reportPeriod = _payPeriodFr.ToString("MMMM yyyy");
            }

            headerRow = (NPOI.HSSF.UserModel.HSSFRow)worksheet.CreateRow(2);
            headerCell = (NPOI.HSSF.UserModel.HSSFCell)headerRow.CreateCell(0);
            headerCell.SetCellValue(m_reportPeriod);
            headerCell.CellStyle = boldStyle;

            // Merge header
            headerRow = (NPOI.HSSF.UserModel.HSSFRow)worksheet.CreateRow(3);

            // Merge cell from 5-8
            NPOI.SS.Util.CellRangeAddress cellRangeAddress = new NPOI.SS.Util.CellRangeAddress(3, (short)3, 5, (short)8);
            worksheet.AddMergedRegion(cellRangeAddress);
            headerCell = (NPOI.HSSF.UserModel.HSSFCell)headerRow.CreateCell(5);
            headerCell.SetCellValue("For New Joiner");
            headerCell.CellStyle = yellowStyle;

            ((NPOI.HSSF.UserModel.HSSFSheet)worksheet).SetEnclosedBorderOfRegion(cellRangeAddress, NPOI.SS.UserModel.BorderStyle.THIN, NPOI.HSSF.Util.HSSFColor.BLACK.index);

            // Merge cell from 9-10
            cellRangeAddress = new NPOI.SS.Util.CellRangeAddress(3, (short)3, 9, (short)10);
            worksheet.AddMergedRegion(cellRangeAddress);
            headerCell = (NPOI.HSSF.UserModel.HSSFCell)headerRow.CreateCell(9);
            headerCell.SetCellValue("For Resigned Staff");
            headerCell.CellStyle = grey25Style;
            ((NPOI.HSSF.UserModel.HSSFSheet)worksheet).SetEnclosedBorderOfRegion(cellRangeAddress, NPOI.SS.UserModel.BorderStyle.THIN, NPOI.HSSF.Util.HSSFColor.BLACK.index);

            headerRow = (NPOI.HSSF.UserModel.HSSFRow)worksheet.CreateRow(4);
            headerRow.HeightInPoints = 40;

            // column A
            headerCell = (NPOI.HSSF.UserModel.HSSFCell)headerRow.CreateCell(0);
            headerCell.SetCellValue("Member ID");
            headerCell.CellStyle = headerLeftBorderStyle;

            // column B
            headerCell = (NPOI.HSSF.UserModel.HSSFCell)headerRow.CreateCell(1);
            headerCell.SetCellValue("Employee Name");
            headerCell.CellStyle = headerBorderStyle;

            // column C
            headerCell = (NPOI.HSSF.UserModel.HSSFCell)headerRow.CreateCell(2);
            headerCell.SetCellValue("Basic Salary");
            headerCell.CellStyle = headerBorderStyle;

            // column D
            headerCell = (NPOI.HSSF.UserModel.HSSFCell)headerRow.CreateCell(3);
            headerCell.SetCellValue("KTPF Contribution");
            headerCell.CellStyle = headerBorderStyle;
            
            // column E
            headerCell = (NPOI.HSSF.UserModel.HSSFCell)headerRow.CreateCell(4);
            headerCell.SetCellValue("Employer MPF Contribution");
            headerCell.CellStyle = headerRightBorderStyle;


            // column F
            headerCell = (NPOI.HSSF.UserModel.HSSFCell)headerRow.CreateCell(5);
            headerCell.SetCellValue("Sex");
            headerCell.CellStyle = headerBorderStyle;


            // column G
            headerCell = (NPOI.HSSF.UserModel.HSSFCell)headerRow.CreateCell(6);
            headerCell.SetCellValue("Date Of Birth");
            headerCell.CellStyle = headerBorderStyle;


            // column H
            headerCell = (NPOI.HSSF.UserModel.HSSFCell)headerRow.CreateCell(7);
            headerCell.SetCellValue("Date Join");
            headerCell.CellStyle = headerBorderStyle;

            // column I
            headerCell = (NPOI.HSSF.UserModel.HSSFCell)headerRow.CreateCell(8);
            headerCell.SetCellValue("Effective Date");
            headerCell.CellStyle = headerRightBorderStyle;

            // column J
            headerCell = (NPOI.HSSF.UserModel.HSSFCell)headerRow.CreateCell(9);
            headerCell.SetCellValue("Termination Date");
            headerCell.CellStyle = headerBorderStyle;

            // column K
            headerCell = (NPOI.HSSF.UserModel.HSSFCell)headerRow.CreateCell(10);
            headerCell.SetCellValue("Termination Mode");
            headerCell.CellStyle = headerRightBorderStyle;

            // column L
            headerCell = (NPOI.HSSF.UserModel.HSSFCell)headerRow.CreateCell(11);
            headerCell.SetCellValue("Remarks");
            headerCell.CellStyle = headerRightBorderStyle;

            // Create total
            int length = dataSet.ExistingMember.Rows.Count + 5;
            NPOI.HSSF.UserModel.HSSFRow totalRow = (NPOI.HSSF.UserModel.HSSFRow)worksheet.CreateRow(lastRowIndex + length);

            NPOI.HSSF.UserModel.HSSFCell totalCell = (NPOI.HSSF.UserModel.HSSFCell)totalRow.CreateCell(0);
            totalCell.CellStyle = totalNumericStyle;
            totalCell = (NPOI.HSSF.UserModel.HSSFCell)totalRow.CreateCell(5);
            totalCell.CellStyle = totalNumericStyle;
            totalCell = (NPOI.HSSF.UserModel.HSSFCell)totalRow.CreateCell(6);
            totalCell.CellStyle = totalNumericStyle;
            totalCell = (NPOI.HSSF.UserModel.HSSFCell)totalRow.CreateCell(7);
            totalCell.CellStyle = totalNumericStyle;
            totalCell = (NPOI.HSSF.UserModel.HSSFCell)totalRow.CreateCell(8);
            totalCell.CellStyle = totalNumericStyle;
            totalCell = (NPOI.HSSF.UserModel.HSSFCell)totalRow.CreateCell(9);
            totalCell.CellStyle = totalNumericStyle;
            totalCell = (NPOI.HSSF.UserModel.HSSFCell)totalRow.CreateCell(10);
            totalCell.CellStyle = totalNumericStyle;
            totalCell = (NPOI.HSSF.UserModel.HSSFCell)totalRow.CreateCell(11);
            totalCell.CellStyle = totalNumericStyle;

            totalCell = (NPOI.HSSF.UserModel.HSSFCell)totalRow.CreateCell(1);
            totalCell.SetCellValue("Total");
            totalCell.CellStyle = totalNumericStyle;

            totalCell = (NPOI.HSSF.UserModel.HSSFCell)totalRow.CreateCell(2);
            totalCell.SetCellFormula("SUM(C5:C" + totalRow.RowNum.ToString("0") + ")");
            totalCell.CellStyle = totalNumericStyle;

            totalCell = (NPOI.HSSF.UserModel.HSSFCell)totalRow.CreateCell(3);
            totalCell.SetCellFormula("SUM(D5:D" + totalRow.RowNum.ToString("0") + ")");
            totalCell.CellStyle = totalNumericStyle;

            totalCell = (NPOI.HSSF.UserModel.HSSFCell)totalRow.CreateCell(4);
            totalCell.SetCellFormula("SUM(E5:E" + totalRow.RowNum.ToString("0") + ")");
            totalCell.CellStyle = totalNumericStyle;

            int rowLength = 0;

            // Set value for every row

            dataSet.ExistingMember.DefaultView.Sort = "EmpName";
            DataTable m_table = dataSet.ExistingMember.DefaultView.ToTable();


            foreach (DataRow m_row in m_table.Rows)
            //foreach (DataRow row in tmpDataTable.Rows)
            {
                NPOI.HSSF.UserModel.HSSFRow detailRow = (NPOI.HSSF.UserModel.HSSFRow)worksheet.CreateRow(lastRowIndex + 5);
                rowLength++;

                if (lastRowIndex == (m_table.Rows.Count))
                {
                    detailRow.RowStyle = bottomBorderStyle;
                }

                NPOI.HSSF.UserModel.HSSFCell cell = (NPOI.HSSF.UserModel.HSSFCell)detailRow.CreateCell(0);
                cell.SetCellValue(m_row["MemberID"].ToString());
                cell.CellStyle = column0Style;

                cell = (NPOI.HSSF.UserModel.HSSFCell)detailRow.CreateCell(1);
                cell.SetCellValue(m_row["EmpName"].ToString());
                cell.CellStyle = stringLeftStyle;

                cell = (NPOI.HSSF.UserModel.HSSFCell)detailRow.CreateCell(2);
                cell.SetCellValue((double)m_row["RelevantIncome"]);
                cell.CellStyle = numericStyle;

                cell = (NPOI.HSSF.UserModel.HSSFCell)detailRow.CreateCell(3);
                cell.SetCellValue((double)m_row["ER"]);
                cell.CellStyle = numericStyle;

                cell = (NPOI.HSSF.UserModel.HSSFCell)detailRow.CreateCell(4);
                cell.SetCellValue((double)m_row["MpfMCER"]);
                cell.CellStyle = column4Style;

                DateTime m_periodFrom = (DateTime)m_row["PeriodFrom"];
                DateTime m_effDate = (DateTime)m_row["OrsoEffDate"];

                {
                    cell = (NPOI.HSSF.UserModel.HSSFCell)detailRow.CreateCell(5);
                    cell.CellStyle = stringCenterStyle;
                    if (m_periodFrom.Year == m_effDate.Year && m_periodFrom.Year == m_effDate.Month)
                        cell.SetCellValue(m_row["EmpSex"].ToString());

                    cell = (NPOI.HSSF.UserModel.HSSFCell)detailRow.CreateCell(6);
                    cell.CellStyle = dateCellStyle;
                    
                    if (m_periodFrom.Year == m_effDate.Year && m_periodFrom.Year == m_effDate.Month)
                        try
                        {
                            cell.SetCellValue((DateTime)m_row["EmpDOB"]);
                        }
                        catch { }

                    cell = (NPOI.HSSF.UserModel.HSSFCell)detailRow.CreateCell(7);
                    cell.CellStyle = dateCellStyle;
                    if (m_periodFrom.Year == m_effDate.Year && m_periodFrom.Year == m_effDate.Month)
                        try
                        {
                            cell.SetCellValue((DateTime)m_row["EmpDateJoin"]);
                        }
                        catch { }

                    cell = (NPOI.HSSF.UserModel.HSSFCell)detailRow.CreateCell(8);
                    cell.CellStyle = column8Style;
                    if (m_periodFrom.Year == m_effDate.Year && m_periodFrom.Year == m_effDate.Month)
                        try
                        {
                            cell.SetCellValue((DateTime)m_row["OrsoEffDate"]);
                        }
                        catch { }
                }
                cell = (NPOI.HSSF.UserModel.HSSFCell)detailRow.CreateCell(9);
                cell.CellStyle = dateCellStyle;
                cell.SetCellValue("");// put something into the cell and so as the border line can be shown
                if (m_periodFrom.Year == m_effDate.Year && m_periodFrom.Year == m_effDate.Month)
                    try
                    {
                        cell.SetCellValue((DateTime)m_row["LastEmploymentDate"]);
                    }
                    catch { }

                cell = (NPOI.HSSF.UserModel.HSSFCell)detailRow.CreateCell(10);
                cell.SetCellValue(m_row["TermCode"].ToString());
                cell.CellStyle = column10To11Style;

                cell = (NPOI.HSSF.UserModel.HSSFCell)detailRow.CreateCell(11);
                //cell.SetCellValue(row[FIELD_REMARKS].ToString());
                cell.CellStyle = column10To11Style;

                lastRowIndex++;
            }

            System.IO.FileStream file = new System.IO.FileStream(exportFileName, System.IO.FileMode.Create);
            workbook.Write(file);
            file.Close();
        }
    }
}
