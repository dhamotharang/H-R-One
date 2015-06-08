using System;
using System.Collections;
using System.Text;
using HROne.Common;
using HROne.Lib.Entities;
using HROne.DataAccess;
using CrystalDecisions.CrystalReports.Engine;
using System.Data;

namespace HROne.MPFFile
{
    public class MPFRemittanceStatementProcessFandV :GenericReportProcess
    {
        const string FIELD_MEMBER_NAME = "Member Name";
        const string FIELD_CERT_NO = "Cert No.";
        const string FIELD_HKID = "HKID No.";
        const string FIELD_JOB_CATEGORY = "Job Category";
        const string FIELD_EMPLOYMENT_DATE = "Date of Employment";
        const string FIELD_FROM = "From";
        const string FIELD_TO = "To";
        const string FIELD_RELEVANT_INCOME = "Relevant Income";
        const string FIELD_MCEE = "Employee Mandatory Contribution";
        const string FIELD_MCER = "Employer Mandatory Contribution";
        const string FIELD_VCEE = "Employee Voluntary Contribution";
        const string FIELD_VCER = "Employer Voluntary Contribution";
        const string FIELD_SURCHARGE = "Surcharge";
        const string FIELD_LAST_DATE_OF_EMPLOYMENT = "Last Date of Employment";
        const string FIELD_LSP_SP_ENTITLMENT = "LSP/SP Entitlement";
        const string FIELD_REMARKS = "Remarks";


        //DataSet.Payroll_MPFRemittanceStatement.AdditionalMemberDataTable additionalMPF;
        private DateTime PayPeriodFr, PayPeriodTo;
        private ArrayList EmpList;
        private int MPFPlanID;
        private int ORSOPlanID;
        private string ChequeNo;

        public MPFRemittanceStatementProcessFandV(DatabaseConnection dbConn, ArrayList EmpList, int MPFPlanID, int ORSOPlanID, DateTime PayPeriodFr, DateTime PayPeriodTo, string ChequeNo)
            : base(dbConn)
        {
            this.EmpList = EmpList;
            this.PayPeriodFr = PayPeriodFr;
            this.PayPeriodTo = PayPeriodTo;
            this.MPFPlanID = MPFPlanID;
            this.ORSOPlanID = ORSOPlanID;
            this.ChequeNo = ChequeNo;
        }

        public void GenerateExcelReport(string _filename)
        {
            // F&V MPF Remittance Statement is composed of MPF and PFund data.  So the current MPF Remittance Statement can only achieve 1/2 of their needs.

            if (PayPeriodFr.Ticks != 0 && PayPeriodTo.Ticks != 0 && EmpList != null)
            {
                // get MPF part of the remmittance statement
                MPFFile.GenericMPFFile mpfFile = new MPFFile.GenericMPFFile(dbConn);
                mpfFile.LoadMPFFileDetail(EmpList, MPFPlanID, PayPeriodFr, PayPeriodTo);
                System.Data.DataSet dataSet = mpfFile.CreateRemittanceStatementDataSet();

                string _schemeNo = "";
                string _companyName = "";

                DataTable m_table = null;
                NPOI.HSSF.UserModel.HSSFSheet m_worksheet = null;

                NPOI.HSSF.UserModel.HSSFWorkbook workbook = new NPOI.HSSF.UserModel.HSSFWorkbook();
                if (this.MPFPlanID > 0)
                {
                    EMPFPlan _mpfPlan = EMPFPlan.GetObject(dbConn, this.MPFPlanID);
                    if (_mpfPlan != null)
                    {
                        EMPFScheme _mpfScheme = EMPFScheme.GetObject(dbConn, _mpfPlan.MPFSchemeID);
                        if (_mpfScheme != null)
                            _schemeNo = _mpfScheme.MPFSchemeCode;
                        _companyName = _mpfPlan.MPFPlanCompanyName;
                    }

                    m_table = ConvertData(dataSet.Tables["NewJoinMember"], "", 1);
                    m_worksheet = (NPOI.HSSF.UserModel.HSSFSheet)workbook.CreateSheet("1-NewJoinMember");
                    WriteToSpreadsheet(m_table, m_worksheet, _schemeNo, _companyName, 1);

                    m_table = ConvertData(dataSet.Tables["ExistingMember"], "IsBackPayRecord = false", 1);
                    m_worksheet = (NPOI.HSSF.UserModel.HSSFSheet)workbook.CreateSheet("1-ExistingEmployee");
                    WriteToSpreadsheet(m_table, m_worksheet, _schemeNo, _companyName, 1);

                    m_table = ConvertData(dataSet.Tables["ExistingMember"], "IsBackPayRecord = true", 1);
                    m_worksheet = (NPOI.HSSF.UserModel.HSSFSheet)workbook.CreateSheet("1-TerminatedEmployee");
                    WriteToSpreadsheet(m_table, m_worksheet, _schemeNo, _companyName, 1);
                }

                if (this.ORSOPlanID > 0)
                {
                    EORSOPlan _orsoPlan = EORSOPlan.GetObject(dbConn, ORSOPlanID);
                    if (_orsoPlan != null)
                    {
                        _schemeNo = _orsoPlan.ORSOPlanSchemeNo;
                        _companyName = _orsoPlan.ORSOPlanCompanyName;
                    }

                    DataTable _orsoStaffTable = LoadORSODetail();
                    m_table = ConvertData(_orsoStaffTable, "IsBackPayRecord = false", 2);
                    m_worksheet = (NPOI.HSSF.UserModel.HSSFSheet)workbook.CreateSheet("2-ExistingEmployee");
                    WriteToSpreadsheet(m_table, m_worksheet, _schemeNo, _companyName, 2);

                    m_table = ConvertData(_orsoStaffTable, "IsBackPayRecord = true", 2);
                    m_worksheet = (NPOI.HSSF.UserModel.HSSFSheet)workbook.CreateSheet("2-TerminatedEmployee");
                    WriteToSpreadsheet(m_table, m_worksheet, _schemeNo, _companyName, 2);
                }


                // get ORSO part of remittance statement
                


                System.IO.FileStream file = new System.IO.FileStream(_filename, System.IO.FileMode.Create);
                workbook.Write(file);
                file.Close();
            }
        }

        protected DataTable LoadORSODetail()
        {
            DataTable _table = new DataSet.Payroll_MPFRemittanceStatement.ExistingMemberDataTable();

            DBFilter _payPeriodFilter = new DBFilter();
            _payPeriodFilter.add(new Match("PayPeriodFr", "<=", this.PayPeriodTo));
            _payPeriodFilter.add(new Match("PayPeriodTo", ">=", this.PayPeriodFr));

            DBFilter _empPayrollFilter = new DBFilter();
            _empPayrollFilter.add(new IN("PayPeriodID", "SELECT PayPeriodID FROM PayrollPeriod", _payPeriodFilter));

            DBFilter _orsoRecordFilter = new DBFilter();
            _orsoRecordFilter.add(new Match("ORSOPlanID", this.ORSOPlanID));
            _orsoRecordFilter.add(new IN("EmpPayrollID", "SELECT EmpPayrollID FROM EmpPayroll", _empPayrollFilter));

            foreach (EORSORecord _orsoRecord in EORSORecord.db.select(dbConn, _orsoRecordFilter))
            {

                DataRow _newRow = _table.NewRow();

                EEmpPayroll _payroll = EEmpPayroll.GetObject(dbConn, _orsoRecord.EmpPayrollID);
                EEmpPersonalInfo _empInfo = EEmpPersonalInfo.GetObject(dbConn, _payroll.EmpID);

                _newRow["EmpID"] = _empInfo.EmpID;
                _newRow["MPFPlanID"] = _orsoRecord.ORSOPlanID;
                _newRow["EmpNo"] = _empInfo.EmpNo;
                _newRow["EmpName"] = _empInfo.EmpEngFullName;
                _newRow["SchemeJoinDate"] = _empInfo.EmpDateOfJoin;
               
                if (string.IsNullOrEmpty(_empInfo.EmpHKID))
                    _newRow["HKID"] = _empInfo.EmpPassportNo;
                else
                    _newRow["HKID"] = _empInfo.EmpHKID;

                _newRow["PeriodFrom"] = _orsoRecord.ORSORecPeriodFr;
                _newRow["PeriodTo"] = _orsoRecord.ORSORecPeriodTo;

                _newRow["RelevantIncome"] = 0; //_orsoRecord.ORSORecActRI;
                _newRow["MCEE"] = _orsoRecord.ORSORecActEE;
                _newRow["MCER"] = _orsoRecord.ORSORecActER;
                _newRow["VCEE"] = 0;
                _newRow["VCER"] = 0;
                
                EEmpTermination _empTermination = EEmpTermination.GetObjectByEmpID(dbConn, _empInfo.EmpID);
                if (_empTermination != null && _empTermination.EmpTermLastDate.Ticks != 0)
                {
                    _newRow["LastEmploymentDate"] = _empTermination.EmpTermLastDate;
                    _newRow["IsBackPayRecord"] = (_empTermination.EmpTermLastDate > this.PayPeriodFr);
                }
                else
                {
                    _newRow["LastEmploymentDate"] = DBNull.Value;
                    _newRow["IsBackPayRecord"] = false;
                }
                _newRow["IsLSP"] = false;
                _newRow["IsSP"] = false;
                _newRow["LSPSPAmount"] = 0;
                _newRow["LSPSPAmountPaidByER"] = 0;

                _table.Rows.Add(_newRow);
            }
            return _table; 
        }

        protected void WriteCellText(NPOI.HSSF.UserModel.HSSFRow _row, int _colIndex, double _value, NPOI.HSSF.UserModel.HSSFCellStyle _cellStyle)
        {
            NPOI.HSSF.UserModel.HSSFCell _cell = (NPOI.HSSF.UserModel.HSSFCell)_row.CreateCell(_colIndex);

            _cell.CellStyle = _cellStyle;
            _cell.SetCellValue(_value);
        }

        protected void WriteCellText(NPOI.HSSF.UserModel.HSSFRow _row, int _colIndex, DateTime _value, NPOI.HSSF.UserModel.HSSFCellStyle _cellStyle)
        {
            NPOI.HSSF.UserModel.HSSFCell _cell = (NPOI.HSSF.UserModel.HSSFCell)_row.CreateCell(_colIndex);

            _cell.CellStyle = _cellStyle;
            _cell.SetCellValue(_value);
        }

        protected void WriteCellText(NPOI.HSSF.UserModel.HSSFRow _row, int _colIndex, string _value, NPOI.HSSF.UserModel.HSSFCellStyle _cellStyle)
        {
            NPOI.HSSF.UserModel.HSSFCell _cell = (NPOI.HSSF.UserModel.HSSFCell)_row.CreateCell(_colIndex);
 
            _cell.CellStyle = _cellStyle;
            _cell.SetCellValue(_value);
        }

        protected void WriteCellText(NPOI.HSSF.UserModel.HSSFRow _row, int _colIndex, string _value)
        {
            NPOI.HSSF.UserModel.HSSFCell _cell = (NPOI.HSSF.UserModel.HSSFCell)_row.CreateCell(_colIndex);

            _cell.SetCellValue(_value);
        }

        public void WriteToSpreadsheet(DataTable _dataTable, NPOI.HSSF.UserModel.HSSFSheet _worksheet, string _schemeNo, string _companyName, int _billingClass)
        {
            if (_worksheet != null)
            {
                NPOI.HSSF.UserModel.HSSFDataFormat _format = (NPOI.HSSF.UserModel.HSSFDataFormat)_worksheet.Workbook.CreateDataFormat();

                // font setting(bold)
                NPOI.HSSF.UserModel.HSSFFont _boldFont = (NPOI.HSSF.UserModel.HSSFFont)_worksheet.Workbook.CreateFont();
                _boldFont.Boldweight = (short)NPOI.SS.UserModel.FontBoldWeight.BOLD; //900;

                // font setting(underline)
                NPOI.HSSF.UserModel.HSSFFont _sectionHeaderFont = (NPOI.HSSF.UserModel.HSSFFont)_worksheet.Workbook.CreateFont();
                _sectionHeaderFont.Underline = (byte)1;
                _sectionHeaderFont.Boldweight = (short)NPOI.SS.UserModel.FontBoldWeight.BOLD; //900;

                // style setting
                NPOI.HSSF.UserModel.HSSFCellStyle _sectionHeaderStyle = (NPOI.HSSF.UserModel.HSSFCellStyle)_worksheet.Workbook.CreateCellStyle();
                _sectionHeaderStyle.SetFont(_sectionHeaderFont);

                
                NPOI.HSSF.UserModel.HSSFCellStyle _dateCellStyle = (NPOI.HSSF.UserModel.HSSFCellStyle)_worksheet.Workbook.CreateCellStyle();
                _dateCellStyle.DataFormat = _format.GetFormat("yyyy-MM-dd");
                _dateCellStyle.Alignment = NPOI.SS.UserModel.HorizontalAlignment.CENTER;

                NPOI.HSSF.UserModel.HSSFCellStyle _doubleCellStyle = (NPOI.HSSF.UserModel.HSSFCellStyle)_worksheet.Workbook.CreateCellStyle();
                _doubleCellStyle.DataFormat = _format.GetFormat("#,##0.00");
                _doubleCellStyle.Alignment = NPOI.SS.UserModel.HorizontalAlignment.RIGHT;

                NPOI.HSSF.UserModel.HSSFCellStyle _boldTextCellStyle = (NPOI.HSSF.UserModel.HSSFCellStyle)_worksheet.Workbook.CreateCellStyle();
                _boldTextCellStyle.SetFont(_boldFont);

                //NPOI.HSSF.UserModel.HSSFCellStyle _underlineTextCellStyle = (NPOI.HSSF.UserModel.HSSFCellStyle)_worksheet.Workbook.CreateCellStyle();
                //_underlineTextCellStyle.SetFont(_underlineFont);
                
                // bottom line style
                NPOI.HSSF.UserModel.HSSFCellStyle _boldTextWithBottomBorderCellStyle = (NPOI.HSSF.UserModel.HSSFCellStyle)_worksheet.Workbook.CreateCellStyle();
                _boldTextWithBottomBorderCellStyle.SetFont(_boldFont);
                _boldTextWithBottomBorderCellStyle.BorderBottom = NPOI.SS.UserModel.BorderStyle.THIN;
                _boldTextWithBottomBorderCellStyle.Alignment = NPOI.SS.UserModel.HorizontalAlignment.CENTER;

                NPOI.HSSF.UserModel.HSSFCellStyle _bottomBorderCellStyle = (NPOI.HSSF.UserModel.HSSFCellStyle)_worksheet.Workbook.CreateCellStyle();
                _bottomBorderCellStyle.BorderBottom = NPOI.SS.UserModel.BorderStyle.THIN;

                NPOI.HSSF.UserModel.HSSFCellStyle _chequeAmountCellStyle = (NPOI.HSSF.UserModel.HSSFCellStyle)_worksheet.Workbook.CreateCellStyle();
                _chequeAmountCellStyle.BorderBottom = NPOI.SS.UserModel.BorderStyle.THIN;
                _chequeAmountCellStyle.DataFormat = _format.GetFormat("$#,##0.00");

                // header style
                NPOI.HSSF.UserModel.HSSFCellStyle _leftHeaderStyle = (NPOI.HSSF.UserModel.HSSFCellStyle)_worksheet.Workbook.CreateCellStyle();
                _leftHeaderStyle.SetFont(_boldFont);
                _leftHeaderStyle.WrapText = true;

                NPOI.HSSF.UserModel.HSSFCellStyle _rightHeaderStyle = (NPOI.HSSF.UserModel.HSSFCellStyle)_worksheet.Workbook.CreateCellStyle();
                _rightHeaderStyle.SetFont(_boldFont);
                _rightHeaderStyle.Alignment = NPOI.SS.UserModel.HorizontalAlignment.RIGHT;
                _rightHeaderStyle.WrapText = true;

                NPOI.HSSF.UserModel.HSSFCellStyle _centerHeaderStyle = (NPOI.HSSF.UserModel.HSSFCellStyle)_worksheet.Workbook.CreateCellStyle();
                _centerHeaderStyle.SetFont(_boldFont);
                _centerHeaderStyle.Alignment = NPOI.SS.UserModel.HorizontalAlignment.CENTER;
                _centerHeaderStyle.WrapText = true;


                // START !!!
                NPOI.HSSF.UserModel.HSSFRow _row = (NPOI.HSSF.UserModel.HSSFRow)_worksheet.CreateRow(0);
                
                NPOI.HSSF.UserModel.HSSFCell _cell = (NPOI.HSSF.UserModel.HSSFCell)_row.CreateCell(0);
                WriteCellText(_row, 0, "Scheme No.");
                WriteCellText(_row, 1, _schemeNo);

                _row = (NPOI.HSSF.UserModel.HSSFRow)_worksheet.CreateRow(1);
                WriteCellText(_row, 0, "Billing Class");
                WriteCellText(_row, 1, _billingClass.ToString("00"));

                _row = (NPOI.HSSF.UserModel.HSSFRow)_worksheet.CreateRow(2);
                WriteCellText(_row, 0, "Co. Name");
                WriteCellText(_row, 1, _companyName.ToUpper());

                _row = (NPOI.HSSF.UserModel.HSSFRow)_worksheet.CreateRow(4);

                switch(_worksheet.SheetName)
                {
                    case "NewJoinMember":
                        WriteCellText(_row, 0, "Remittance Statement for NEW EMPLOYEES", _sectionHeaderStyle);
                        break;
                    case "ExistingEmployee":
                        WriteCellText(_row, 0, "Remittance Statement for EXISTING EMPLOYEES", _sectionHeaderStyle);
                        break;
                    case "TerminatedEmployee":
                        WriteCellText(_row, 0, "Remittance Statement for TERMINATED EMPLOYEES", _sectionHeaderStyle);
                        break;
                    default:
                        break;
                }

                // column header
                _row = (NPOI.HSSF.UserModel.HSSFRow)_worksheet.CreateRow(6);
                WriteCellText(_row, 0, "Member Name", _leftHeaderStyle);
                WriteCellText(_row, 1, "Cert No.", _leftHeaderStyle);
                WriteCellText(_row, 2, "HKID no.", _leftHeaderStyle);
                WriteCellText(_row, 3, "Job Category", _leftHeaderStyle);
                WriteCellText(_row, 4, "Date of Employment\n(dd/mm/yyyy)", _centerHeaderStyle);
                WriteCellText(_row, 5, "Payroll Period\n(dd/mm/yyyy)", _centerHeaderStyle);
                WriteCellText(_row, 7, "Relevant Income", _rightHeaderStyle);
                WriteCellText(_row, 8, "Employee Mandatory Contribution", _rightHeaderStyle);
                WriteCellText(_row, 9, "Employer Mandatory Contribution", _rightHeaderStyle);
                WriteCellText(_row, 10, "Employee Voluntary Contribution", _rightHeaderStyle);
                WriteCellText(_row, 11, "Employer Voluntary Contribution", _rightHeaderStyle);
                WriteCellText(_row, 12, "Surcharge", _rightHeaderStyle);
                WriteCellText(_row, 13, "Last Date of Employment\n(dd/mm/yyyy)", _centerHeaderStyle);
                WriteCellText(_row, 14, "LSP/ SP Entitlement#\n(Y/N)", _leftHeaderStyle);
                WriteCellText(_row, 15, "Remarks^\n(Code: 1/2/3/4/5/6/7)", _leftHeaderStyle);

                // column header 2
                _row = (NPOI.HSSF.UserModel.HSSFRow)_worksheet.CreateRow(7);
                WriteCellText(_row, 0, "", _boldTextWithBottomBorderCellStyle);
                WriteCellText(_row, 1, ".", _boldTextWithBottomBorderCellStyle);
                WriteCellText(_row, 2, ".", _boldTextWithBottomBorderCellStyle);
                WriteCellText(_row, 3, "", _boldTextWithBottomBorderCellStyle);
                WriteCellText(_row, 4, "", _boldTextWithBottomBorderCellStyle);
                WriteCellText(_row, 5, "From", _boldTextWithBottomBorderCellStyle);
                WriteCellText(_row, 6, "To", _boldTextWithBottomBorderCellStyle);
                WriteCellText(_row, 7, "", _boldTextWithBottomBorderCellStyle);
                WriteCellText(_row, 8, "", _boldTextWithBottomBorderCellStyle);
                WriteCellText(_row, 9, "", _boldTextWithBottomBorderCellStyle);
                WriteCellText(_row, 10, "", _boldTextWithBottomBorderCellStyle);
                WriteCellText(_row, 11, "", _boldTextWithBottomBorderCellStyle);
                WriteCellText(_row, 12, "", _boldTextWithBottomBorderCellStyle);
                WriteCellText(_row, 13, "", _boldTextWithBottomBorderCellStyle);
                WriteCellText(_row, 14, "", _boldTextWithBottomBorderCellStyle);
                WriteCellText(_row, 15, "", _boldTextWithBottomBorderCellStyle);

                // merge column header
                NPOI.SS.Util.CellRangeAddress _cellRangeAddress;

                for (int i = 0; i < 16; i++)
                {
                    if (i == 5)
                        _cellRangeAddress = new NPOI.SS.Util.CellRangeAddress(6, 6, 5, 6);  // merge "Payroll Period"
                    else if (i == 6)
                        continue;
                    else
                        _cellRangeAddress = new NPOI.SS.Util.CellRangeAddress(6, 7, i, i);

                    _worksheet.AddMergedRegion(_cellRangeAddress);
                }

                // generate data
                int _currentRow = 8;
                int _dataStartRow = 8;
                int _dataEndRow = 0;
                foreach (DataRow _dataRow in _dataTable.Rows)
                {
                    _row = (NPOI.HSSF.UserModel.HSSFRow)_worksheet.CreateRow(_currentRow);
                    WriteCellText(_row, 0, _dataRow[FIELD_MEMBER_NAME].ToString());

                    WriteCellText(_row, 1, _dataRow[FIELD_CERT_NO].ToString());
                    WriteCellText(_row, 2, _dataRow[FIELD_HKID].ToString());
                    WriteCellText(_row, 3, _dataRow[FIELD_JOB_CATEGORY].ToString());
                    WriteCellText(_row, 4, ((DateTime)_dataRow[FIELD_EMPLOYMENT_DATE]), _dateCellStyle);
                    WriteCellText(_row, 5, ((DateTime)_dataRow[FIELD_FROM]), _dateCellStyle);
                    WriteCellText(_row, 6, ((DateTime)_dataRow[FIELD_TO]), _dateCellStyle);
                    WriteCellText(_row, 7, (double)_dataRow[FIELD_RELEVANT_INCOME], _doubleCellStyle);
                    WriteCellText(_row, 8, (double)_dataRow[FIELD_MCEE], _doubleCellStyle);
                    WriteCellText(_row, 9, (double)_dataRow[FIELD_MCER], _doubleCellStyle);
                    WriteCellText(_row, 10, (double)_dataRow[FIELD_VCEE], _doubleCellStyle);
                    WriteCellText(_row, 11, (double)_dataRow[FIELD_VCER], _doubleCellStyle);
                    WriteCellText(_row, 12, (double)_dataRow[FIELD_SURCHARGE], _doubleCellStyle);

                    if (_dataRow[FIELD_LAST_DATE_OF_EMPLOYMENT] != DBNull.Value)
                        WriteCellText(_row, 13, (double)_dataRow[FIELD_LAST_DATE_OF_EMPLOYMENT], _dateCellStyle);

                    WriteCellText(_row, 14, _dataRow[FIELD_LSP_SP_ENTITLMENT].ToString());
                    WriteCellText(_row, 15, _dataRow[FIELD_REMARKS].ToString());

                    _currentRow++;
                }
                _dataEndRow = _currentRow - 1;

                _currentRow++;

                _row = (NPOI.HSSF.UserModel.HSSFRow)_worksheet.CreateRow(_currentRow);
                WriteCellText(_row, 0, "Note:");

                _currentRow++;
                _row = (NPOI.HSSF.UserModel.HSSFRow)_worksheet.CreateRow(_currentRow);
                WriteCellText(_row, 0, "# Please indicate Yes/ No if reported Last Date of Employment for the member. If Yes, Notice of Termination form for the reimbursement of LSP/ SP will be provided");

                _currentRow++;
                _row = (NPOI.HSSF.UserModel.HSSFRow)_worksheet.CreateRow(_currentRow);
                WriteCellText(_row, 0, "^ 1. Rejoin   2. Intra-group transfer  3. back-payment for terminated member  4. Over age 65   5. Overseas member  6.New in Billing Class 02  7. Work < 60 days");

                _currentRow += 2;

                _row = (NPOI.HSSF.UserModel.HSSFRow)_worksheet.CreateRow(_currentRow);
                WriteCellText(_row, 0, "Cheque No.");
                WriteCellText(_row, 1, ChequeNo, _bottomBorderCellStyle);
                WriteCellText(_row, 3, "Cheque Amount");

                // write Cheque Amount
                //_row = (NPOI.HSSF.UserModel.HSSFRow)_worksheet.CreateRow(_currentRow);
                _cell = (NPOI.HSSF.UserModel.HSSFCell)_row.CreateCell(4);
                _cell.CellStyle = _chequeAmountCellStyle;
                if (_dataEndRow >= _dataStartRow)
                    _cell.CellFormula = "SUM(I" + Convert.ToInt32(_dataStartRow + 1).ToString("0") + ":L" + Convert.ToInt32(_dataEndRow + 1).ToString("0") + ")";

                _currentRow += 6;
                _row = (NPOI.HSSF.UserModel.HSSFRow)_worksheet.CreateRow(_currentRow);
                WriteCellText(_row, 0, " ", _bottomBorderCellStyle);
                WriteCellText(_row, 1, "", _bottomBorderCellStyle);
                WriteCellText(_row, 2, "", _bottomBorderCellStyle);

                _currentRow++;
                _row = (NPOI.HSSF.UserModel.HSSFRow)_worksheet.CreateRow(_currentRow);
                WriteCellText(_row, 0, "Authorized Signature and Co. Chop");

                _worksheet.SetColumnWidth(0, 20 * 256);
                _worksheet.SetColumnWidth(1, 15 * 256);
                _worksheet.SetColumnWidth(2, 14 * 256);
                _worksheet.SetColumnWidth(3, 14 * 256);
                _worksheet.SetColumnWidth(4, 14 * 256);
                _worksheet.SetColumnWidth(5, 14 * 256);
                _worksheet.SetColumnWidth(6, 14 * 256);
                _worksheet.SetColumnWidth(7, 14 * 256);
                _worksheet.SetColumnWidth(8, 14 * 256);
                _worksheet.SetColumnWidth(9, 14 * 256);
                _worksheet.SetColumnWidth(10, 14 * 256);
                _worksheet.SetColumnWidth(11, 14 * 256);
                _worksheet.SetColumnWidth(12, 14 * 256);
                _worksheet.SetColumnWidth(13, 14 * 256);
                _worksheet.SetColumnWidth(14, 15 * 256);
                _worksheet.SetColumnWidth(15, 30 * 256);

                //_worksheet.Workbook.SetPrintArea(_worksheet
                _worksheet.PrintSetup.Landscape = true;
                _worksheet.PrintSetup.FitWidth = (short) 1;

                _worksheet.SetMargin(NPOI.SS.UserModel.MarginType.TopMargin, (double)0);
                _worksheet.SetMargin(NPOI.SS.UserModel.MarginType.BottomMargin, (double)0);
                _worksheet.SetMargin(NPOI.SS.UserModel.MarginType.LeftMargin, (double)0);
                _worksheet.SetMargin(NPOI.SS.UserModel.MarginType.RightMargin, (double)0);

                _worksheet.PrintSetup.HeaderMargin = (double)0;
                _worksheet.PrintSetup.FooterMargin = (double)0;

            }
        }

        protected DataTable ConvertData(DataTable _staffTable, string _whereClause, int _jobCategory)
        {
            DataTable _table = CreateEmptyTable();

            foreach (DataRow _row in _staffTable.Select(_whereClause))
            {
                DataRow _newRow = _table.NewRow();

                if (_staffTable.Columns.Contains("EmpName"))
                    _newRow[FIELD_MEMBER_NAME] = _row["EmpName"];
                else
                    _newRow[FIELD_MEMBER_NAME] = _row["EmpSurname"] + " " + _row["EmpOtherName"];

                _newRow[FIELD_CERT_NO] = "";
                _newRow[FIELD_HKID] = _row["HKID"].ToString();
                _newRow[FIELD_JOB_CATEGORY] = _jobCategory.ToString("0");
                _newRow[FIELD_EMPLOYMENT_DATE] = _row["SchemeJoinDate"];
                _newRow[FIELD_FROM] = _row["PeriodFrom"];
                _newRow[FIELD_TO] = _row["PeriodTo"];
                _newRow[FIELD_RELEVANT_INCOME] = _row["RelevantIncome"];
                _newRow[FIELD_MCEE] = _row["MCEE"];
                _newRow[FIELD_MCER] = _row["MCER"];
                _newRow[FIELD_VCEE] = _row["VCEE"];
                _newRow[FIELD_VCER] = _row["VCER"];
                _newRow[FIELD_SURCHARGE] = 0;
                if (_staffTable.Columns.Contains("LastEmploymentDate") && _row["LastEmploymentDate"] != DBNull.Value)
                {
                    _newRow[FIELD_LAST_DATE_OF_EMPLOYMENT] = _row["LastEmploymentDate"];
                    _newRow[FIELD_LSP_SP_ENTITLMENT] = "Y";
                }
                else
                {
                    _newRow[FIELD_LAST_DATE_OF_EMPLOYMENT] = DBNull.Value;
                    _newRow[FIELD_LSP_SP_ENTITLMENT] = "";
                }
                _newRow[FIELD_REMARKS] = "";    //^ 1. Rejoin   2. Intra-group transfer  3. back-payment for terminated member  4. Over age 65   5. Overseas member  6.New in Billing Class 02  7. Work < 60 days



                if (_table.TableName == "NewJoinMember")
                {
                    DBFilter _terminatedStaffFilter = new DBFilter();
                    _terminatedStaffFilter.add(new Match("EmpStatus", "T"));

                    string _encryptedHKID;
                    string _encryptedPassportNo;
                    _encryptedHKID = AppUtils.Encode(EEmpPersonalInfo.db.getField("EmpHKID"), _row["HKID"].ToString());
                    _encryptedPassportNo = AppUtils.Encode(EEmpPersonalInfo.db.getField("EmpPassportNo"), _row["HKID"].ToString());

                    OR _or = new OR();
                    _or.add(new Match("EmpHKID", _encryptedHKID));
                    _or.add(new Match("EmpPassportNo", _encryptedHKID));

                    _terminatedStaffFilter.add(_or);

                    ArrayList _terminatedStaffList = EEmpPersonalInfo.db.select(dbConn, _terminatedStaffFilter);
                    if (_terminatedStaffList.Count > 0)
                    {
                        _newRow[FIELD_REMARKS] = "1";   // Rejoin
                    }
                }

                if (_jobCategory == 1)
                {
                    // get extra fields from EmpExtraField
                    EEmpExtraField _extraField1 = EEmpExtraField.GetObjectByName(dbConn, "MPF Topup Cert. No");
                    if (_extraField1 != null)
                    {
                        DBFilter m_filter = new DBFilter();
                        m_filter.add(new Match("EmpID", (int)_row["EmpID"]));
                        m_filter.add(new Match("EmpExtraFieldID", _extraField1.EmpExtraFieldID));
                        ArrayList m_list = EEmpExtraFieldValue.db.select(dbConn, m_filter);
                        if (m_list.Count > 0)
                        {
                            _newRow[FIELD_CERT_NO] = ((EEmpExtraFieldValue)m_list[0]).EmpExtraFieldValue;
                        }
                    }
                }
                else
                {
                    EEmpExtraField _extraField2 = EEmpExtraField.GetObjectByName(dbConn, "VC Cert. No");
                    if (_extraField2 != null)
                    {
                        DBFilter m_filter = new DBFilter();
                        m_filter.add(new Match("EmpID", (int)_row["EmpID"]));
                        m_filter.add(new Match("EmpExtraFieldID", _extraField2.EmpExtraFieldID));
                        ArrayList m_list = EEmpExtraFieldValue.db.select(dbConn, m_filter);
                        if (m_list.Count > 0)
                        {
                            _newRow[FIELD_CERT_NO] = ((EEmpExtraFieldValue)m_list[0]).EmpExtraFieldValue;
                        }
                    }
                }
                _table.Rows.Add(_newRow);
            }
            return _table;
        }

        protected DataTable CreateEmptyTable()
        {
            DataTable _table = new DataTable();

            _table.Columns.Add(new DataColumn(FIELD_MEMBER_NAME, typeof(string)));
            _table.Columns.Add(new DataColumn(FIELD_CERT_NO, typeof(string)));
            _table.Columns.Add(new DataColumn(FIELD_HKID, typeof(string)));
            _table.Columns.Add(new DataColumn(FIELD_JOB_CATEGORY, typeof(string)));
            _table.Columns.Add(new DataColumn(FIELD_EMPLOYMENT_DATE, typeof(DateTime)));
            _table.Columns.Add(new DataColumn(FIELD_FROM, typeof(DateTime)));
            _table.Columns.Add(new DataColumn(FIELD_TO, typeof(DateTime)));
            _table.Columns.Add(new DataColumn(FIELD_RELEVANT_INCOME, typeof(double)));
            _table.Columns.Add(new DataColumn(FIELD_MCEE, typeof(double)));
            _table.Columns.Add(new DataColumn(FIELD_MCER, typeof(double)));
            _table.Columns.Add(new DataColumn(FIELD_VCEE, typeof(double)));
            _table.Columns.Add(new DataColumn(FIELD_VCER, typeof(double)));
            _table.Columns.Add(new DataColumn(FIELD_SURCHARGE, typeof(double)));
            _table.Columns.Add(new DataColumn(FIELD_LAST_DATE_OF_EMPLOYMENT, typeof(DateTime)));
            _table.Columns.Add(new DataColumn(FIELD_LSP_SP_ENTITLMENT, typeof(string)));
            _table.Columns.Add(new DataColumn(FIELD_REMARKS, typeof(string)));

            return _table;
        }


    }
}
