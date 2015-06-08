using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Text;
using HROne.DataAccess;
using HROne.Lib.Entities;
using HROne.Lib;
using HROne.Common;

namespace HROne.Reports.Payroll
{
    public class PayrollAllocationReport_Detail_Process : GenericExcelReportProcess 
    {
        DateTime PeriodFrom;
        DateTime PeriodTo;
        int intHierarchyLevelID;
        ushort reportEndCol=0;
        ushort summaryStart = 0;
        bool bolShowIndividual;
        string hierarchyLevelGroupingFieldName;
        const int STAFF_LEVEL_ID = -1;
        EUser currentUser;
        int selectedCompanyID;
        ArrayList empList;

        // intHierarchyLevelID = -1 if Groupby individual staff (not a level in hierarchy level)
        public PayrollAllocationReport_Detail_Process(DatabaseConnection dbConn, System.Globalization.CultureInfo reportCultureInfo, DateTime PeriodFrom, DateTime PeriodTo, int intHierarchyLevelID, bool bolShowIndividual, EUser currentUser, int selectedCompanyID, ArrayList empList)
            : base(dbConn, reportCultureInfo)
        {
            this.PeriodFrom = PeriodFrom;
            this.PeriodTo = PeriodTo;
            this.intHierarchyLevelID = intHierarchyLevelID;
            this.bolShowIndividual = bolShowIndividual;
            this.currentUser = currentUser;
            this.selectedCompanyID = selectedCompanyID;
            this.empList = empList;
        }

        protected override System.Data.DataSet CreateDataSource()
        {

            System.Data.DataSet dataSet = new System.Data.DataSet();

            DataTable empInfoTable = new DataTable("EmpInfo");
            DataTable payrollTable = new DataTable("Payment");
            DataTable hierarchyTable = new DataTable("hierarchy");
            DataTable payPeriodTable = new DataTable("payPeriod");

            dataSet.Tables.Add(empInfoTable);
            dataSet.Tables.Add(payrollTable);
            dataSet.Tables.Add(hierarchyTable);
            dataSet.Tables.Add(payPeriodTable);
            
            payPeriodTable.Columns.Add("payPeriodFr", typeof(DateTime));

            hierarchyTable.Columns.Add("LevelDesc", typeof(string));

            payrollTable.Columns.Add("EmpPayrollID", typeof(int));
            payrollTable.Columns.Add("empID", typeof(int));
            payrollTable.Columns.Add("payPeriodFr", typeof(DateTime));
            payrollTable.Columns.Add("netAmount", typeof(double));
            payrollTable.Columns.Add("EE", typeof(double));
            payrollTable.Columns.Add("ER", typeof(double));

            int firstDetailColumnPos = payrollTable.Columns.Count;

            empInfoTable.Columns.Add("Employee No.", typeof(string));
            empInfoTable.Columns.Add("EmpPayrollID", typeof(int));
            empInfoTable.Columns.Add("Employee Name", typeof(string));
            empInfoTable.Columns.Add("Alias", typeof(string));
            empInfoTable.Columns.Add("Chinese Name", typeof(string));

            empInfoTable.Columns.Add("Company", typeof(string));
            DBFilter hierarchyLevelFilter = new DBFilter();
            Hashtable hierarchyLevelHashTable = new Hashtable();
            hierarchyLevelFilter.add("HLevelSeqNo", true);
            ArrayList hierarchyLevelList = EHierarchyLevel.db.select(dbConn, hierarchyLevelFilter);

            foreach (EHierarchyLevel hlevel in hierarchyLevelList)
            {
                if (hlevel.HLevelID.Equals(intHierarchyLevelID))
                    hierarchyLevelGroupingFieldName = hlevel.HLevelDesc;
                empInfoTable.Columns.Add(hlevel.HLevelDesc, typeof(string));
                hierarchyLevelHashTable.Add(hlevel.HLevelID, hlevel);

                DataRow m_hierarchyRow = hierarchyTable.NewRow();
                m_hierarchyRow["LevelDesc"] = hlevel.HLevelDesc;
                hierarchyTable.Rows.Add(m_hierarchyRow);
            }

            empInfoTable.Columns.Add("EmpID", typeof(int));
            empInfoTable.Columns.Add("Payroll Group", typeof(string));
            empInfoTable.Columns.Add("Date Join", typeof(DateTime));
            empInfoTable.Columns.Add("Date Left", typeof(DateTime));
            empInfoTable.Columns.Add("Net Payable", typeof(double));
            empInfoTable.Columns.Add("MCEE", typeof(double));
            empInfoTable.Columns.Add("MCER", typeof(double));
            empInfoTable.Columns.Add("VCEE", typeof(double));
            empInfoTable.Columns.Add("VCER", typeof(double));
            empInfoTable.Columns.Add("PFundEE", typeof(double));
            empInfoTable.Columns.Add("PFundER", typeof(double));

            DBFilter payPeriodFilter = new DBFilter();
            if (!PeriodFrom.Ticks.Equals(0))
                payPeriodFilter.add(new Match("PayPeriodTo", ">=", PeriodFrom));
            if (!PeriodTo.Ticks.Equals(0))
                payPeriodFilter.add(new Match("PayPeriodTo", "<=", PeriodTo));


            ArrayList payPeriodList = EPayrollPeriod.db.select(dbConn, payPeriodFilter);
            if (payPeriodList.Count > 0)
            {
                // reset period
                PeriodFrom = new DateTime();
                PeriodTo = new DateTime();
            }

            DBFilter m_userCompanyFilter = new DBFilter();
            m_userCompanyFilter.add(new Match("UserID", currentUser.UserID));
            
            DBFilter m_userRankFilter = new DBFilter();
            m_userRankFilter.add(new Match("UserID", currentUser.UserID));

            Hashtable m_userCompanyList = new Hashtable();
            foreach(EUserCompany m_userCompany in EUserCompany.db.select(dbConn, m_userCompanyFilter))
            {
                m_userCompanyList.Add(m_userCompany.CompanyID, m_userCompany.CompanyID);
            }

            Hashtable m_userRankList = new Hashtable();
            foreach(EUserRank m_userRank in EUserRank.db.select(dbConn, m_userRankFilter))
            {
                m_userRankList.Add(m_userRank.RankID, m_userRank.RankID);
            }
			
			
			int[] m_EmpIDList = new int[empList.Count];
			int i=0;
			foreach (EEmpPersonalInfo m_info in empList)
			{
				m_EmpIDList[i] = m_info.EmpID;
				i++;
			}


            foreach (EPayrollPeriod payPeriod in payPeriodList)
            {
                DataRow[] m_payPeriodRows = payPeriodTable.Select("payPeriodFr = '" + ((DateTime)payPeriod.PayPeriodFr).ToString("yyyy-MM-dd") + "' ");
                if (m_payPeriodRows.Length <= 0)
                {
                    DataRow m_payPeriodRow = payPeriodTable.NewRow();
                    m_payPeriodRow["payPeriodFr"] = payPeriod.PayPeriodFr;
                    payPeriodTable.Rows.Add(m_payPeriodRow);
                }

                if (PeriodFrom > payPeriod.PayPeriodFr || PeriodFrom.Ticks.Equals(0))
                    PeriodFrom = payPeriod.PayPeriodFr;
                if (PeriodTo < payPeriod.PayPeriodTo || PeriodTo.Ticks.Equals(0))
                    PeriodTo = payPeriod.PayPeriodTo;

                DBFilter empPayrollFilter = new DBFilter();
                empPayrollFilter.add(new Match("PayPeriodID", payPeriod.PayPeriodID));
                empPayrollFilter.add(new IN("EmpID", m_EmpIDList));
                ArrayList empPayrollList = EEmpPayroll.db.select(dbConn, empPayrollFilter);

                foreach (EEmpPayroll empPayroll in empPayrollList)
                {
                    DataRow paymentRow = payrollTable.NewRow();
                    DataRow[] m_existingEmpID = payrollTable.Select("EmpID = " + empPayroll.EmpID.ToString());
                    if (m_existingEmpID.Length == 0)
                    {                       
                        EEmpPersonalInfo empInfo = new EEmpPersonalInfo();
                        empInfo.EmpID = empPayroll.EmpID;
                        if (EEmpPersonalInfo.db.select(dbConn, empInfo) == false)
                            continue;

                        DataRow row = empInfoTable.NewRow();

                        row["EmpID"] = empInfo.EmpID;
                        row["Employee No."] = empInfo.EmpNo;
                        row["EmpPayrollID"] = empPayroll.EmpPayrollID;
                        row["Employee Name"] = empInfo.EmpEngFullName;
                        row["Alias"] = empInfo.EmpAlias;
                        row["Chinese Name"] = empInfo.EmpChiFullName;
                        row["Date Join"] = empInfo.EmpDateOfJoin;

                        DBFilter empTerminationFilter = new DBFilter();
                        empTerminationFilter.add(new Match("EmpID", empInfo.EmpID));
                        ArrayList empTerminationList = EEmpTermination.db.select(dbConn, empTerminationFilter);
                        if (empTerminationList.Count > 0)
                            row["Date Left"] = ((EEmpTermination)empTerminationList[0]).EmpTermLastDate;

                        DBFilter empPosFilter = new DBFilter();

                        EEmpPositionInfo empPos = AppUtils.GetLastPositionInfo(dbConn, payPeriod.PayPeriodTo, empInfo.EmpID);


                        if (empPos != null)
                        {
                            if (!m_userCompanyList.Contains(empPos.CompanyID) || !m_userRankList.Contains(empPos.RankID))
                                continue;

                            if (selectedCompanyID > 0 && empPos.CompanyID != selectedCompanyID)
                                continue;

                            ECompany company = new ECompany();
                            company.CompanyID = empPos.CompanyID;
                            if (ECompany.db.select(dbConn, company))
                                row["Company"] = company.CompanyName;

                            EPayrollGroup payrollGroup = new EPayrollGroup();
                            payrollGroup.PayGroupID = empPos.PayGroupID;
                            if (EPayrollGroup.db.select(dbConn, payrollGroup))
                                row["Payroll Group"] = payrollGroup.PayGroupDesc;
                            DBFilter empHierarchyFilter = new DBFilter();
                            empHierarchyFilter.add(new Match("EmpPosID", empPos.EmpPosID));
                            ArrayList empHierarchyList = EEmpHierarchy.db.select(dbConn, empHierarchyFilter);
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
                        }
                        empInfoTable.Rows.Add(row);
                    }

                    double netAmount = 0;
                    
                    DBFilter paymentRecordFilter = new DBFilter();
                    paymentRecordFilter.add(new Match("EmpPayrollID", empPayroll.EmpPayrollID));
                    paymentRecordFilter.add(new Match("PayRecStatus", "A"));

                    foreach (EPaymentRecord paymentRecord in EPaymentRecord.db.select(dbConn, paymentRecordFilter))
                    {
                         netAmount += paymentRecord.PayRecActAmount;
                    }

                    paymentRow["EmpID"] = empPayroll.EmpID;
                    paymentRow["EmpPayrollID"] = empPayroll.EmpPayrollID;
                    paymentRow["payPeriodFr"] = payPeriod.PayPeriodFr;
                    paymentRow["netAmount"] = HROne.CommonLib.GenericRoundingFunctions.RoundingTo(netAmount, ExchangeCurrency.DefaultCurrencyDecimalPlaces(), ExchangeCurrency.DefaultCurrencyDecimalPlaces());

                    DBFilter mpfRecordFilter = new DBFilter();
                    mpfRecordFilter.add(new Match("EmpPayrollID", empPayroll.EmpPayrollID));

                    double m_ee = 0;
                    double m_er = 0;
                    foreach (EMPFRecord mpfRecord in EMPFRecord.db.select(dbConn, mpfRecordFilter))
                    {
                        m_ee += (mpfRecord.MPFRecActVCEE + mpfRecord.MPFRecActMCEE);
                        m_er += (mpfRecord.MPFRecActVCER + mpfRecord.MPFRecActMCER);
                    }

                    ArrayList orsoRecords = EORSORecord.db.select(dbConn, mpfRecordFilter);
                    foreach (EORSORecord orsoRecord in orsoRecords)
                    {
                        m_ee += orsoRecord.ORSORecActEE;
                        m_er += orsoRecord.ORSORecActER;
                    }
                    paymentRow["EE"] = m_ee;
                    paymentRow["ER"] = m_er;

                    payrollTable.Rows.Add(paymentRow);
                }
            }
            //DBFilter paymentCodeFilter = new DBFilter();
            //paymentCodeFilter.add("PaymentCodeDisplaySeqNo", false);
            //paymentCodeFilter.add("PaymentCode", false);
            //ArrayList paymentCodeList = EPaymentCode.db.select(dbConn, paymentCodeFilter);
            //foreach (EPaymentCode paymentCode in paymentCodeList)
            //{
            //    if (payrollTable.Columns.Contains(paymentCode.PaymentCodeDesc))
            //        payrollTable.Columns[paymentCode.PaymentCodeDesc].SetOrdinal(firstDetailColumnPos);
            //}
            return dataSet;
        }
        protected override void CreateWorkBookStyle(NPOI.HSSF.UserModel.HSSFWorkbook workBook)
        {
        }
        protected override void GenerateWorkbookDetail(NPOI.HSSF.UserModel.HSSFWorkbook workBook, System.Data.DataSet dataSet)
        {
            NPOI.HSSF.UserModel.HSSFSheet workSheet = (NPOI.HSSF.UserModel.HSSFSheet)workBook.CreateSheet("Payroll Allocation Report - Detail");

            ushort rowPos = 0;

            DataTable empInfoTable = dataSet.Tables["EmpInfo"];
            DataTable paymentTable = dataSet.Tables["payment"];
            DataTable costCenterDetailTable = dataSet.Tables["CostCenterDetail"];
            DataTable hierarchyTable = dataSet.Tables["hierarchy"];
            DataTable payPeriodTable = dataSet.Tables["payPeriod"];

            DataTable hierarchyTotalTable = new DataTable("HierarchyTotal");    // use for calculate local hierarchy total

            hierarchyTotalTable.Columns.Add("company", typeof(string));
            hierarchyTotalTable.Columns.Add("LevelDesc", typeof(string));
            hierarchyTotalTable.Columns.Add("payPeriodFr", typeof(DateTime));
            hierarchyTotalTable.Columns.Add("ee", typeof(double));
            hierarchyTotalTable.Columns.Add("er", typeof(double));
            hierarchyTotalTable.Columns.Add("netAmount", typeof(double));

            NPOI.HSSF.UserModel.HSSFFont boldFont = (NPOI.HSSF.UserModel.HSSFFont)workBook.CreateFont();
            boldFont.Boldweight = 700;

            NPOI.HSSF.UserModel.HSSFCellStyle reportHeaderStyle = (NPOI.HSSF.UserModel.HSSFCellStyle)workBook.CreateCellStyle();
            reportHeaderStyle.SetFont(boldFont);
            reportHeaderStyle.Alignment = NPOI.SS.UserModel.HorizontalAlignment.LEFT;

            NPOI.HSSF.UserModel.HSSFCellStyle groupHeaderStyle = (NPOI.HSSF.UserModel.HSSFCellStyle)workBook.CreateCellStyle();
            groupHeaderStyle.SetFont(boldFont);

            NPOI.HSSF.UserModel.HSSFCellStyle monthHeaderStyle = (NPOI.HSSF.UserModel.HSSFCellStyle)workBook.CreateCellStyle();
            monthHeaderStyle.SetFont(boldFont);
            monthHeaderStyle.DataFormat = workBook.CreateDataFormat().GetFormat("MMM-yyyy");
            monthHeaderStyle.Alignment = NPOI.SS.UserModel.HorizontalAlignment.CENTER;

            NPOI.HSSF.UserModel.HSSFCellStyle columnHeaderStyle = (NPOI.HSSF.UserModel.HSSFCellStyle)workBook.CreateCellStyle();
            columnHeaderStyle.SetFont(boldFont);
            columnHeaderStyle.BorderBottom = NPOI.SS.UserModel.BorderStyle.THIN;

            NPOI.HSSF.UserModel.HSSFCellStyle detailNumberStyle = (NPOI.HSSF.UserModel.HSSFCellStyle)workBook.CreateCellStyle();
            detailNumberStyle.DataFormat = workBook.CreateDataFormat().GetFormat("#,##0.00;(#,##0.00)");
            detailNumberStyle.Alignment = NPOI.SS.UserModel.HorizontalAlignment.RIGHT;

            NPOI.HSSF.UserModel.HSSFCellStyle detailTextStyle = (NPOI.HSSF.UserModel.HSSFCellStyle)workBook.CreateCellStyle();
            detailTextStyle.Alignment = NPOI.SS.UserModel.HorizontalAlignment.LEFT;

            NPOI.HSSF.UserModel.HSSFCellStyle subTotalStyle = (NPOI.HSSF.UserModel.HSSFCellStyle)workBook.CreateCellStyle();
            subTotalStyle.DataFormat = workBook.CreateDataFormat().GetFormat("#,##0.00;(#,##0.00)");
            subTotalStyle.BorderTop = NPOI.SS.UserModel.BorderStyle.THIN;

            DataRow[] EmpInfoRows = dataSet.Tables["EmpInfo"].Select("", "Company, " + empInfoTable.Columns[hierarchyLevelGroupingFieldName].ColumnName + ", Employee No.");

            string currentCompany = "#$%@$@#$"; //string.Empty;
            string currentHierarchyGroup = string.Empty;
            string currentEmployeeNo = string.Empty;
            string tmpCompany = "";
            string tmpHierarchy = "";
            string tmpEmployeeNo = "";
            ushort groupRowCount = 0;
            bool sectionEnded = false;


            if (bolShowIndividual == true)
            {
                summaryStart = Convert.ToUInt16(4 + hierarchyTable.Rows.Count);
            }
            else
            {
                summaryStart = 1;
            }
            reportEndCol = Convert.ToUInt16(summaryStart + (payPeriodTable.Rows.Count * 4) - 1);
            
            foreach (DataRow EmpInfoRow in EmpInfoRows)
            {
                tmpCompany = EmpInfoRow["Company"].ToString();
                tmpEmployeeNo = EmpInfoRow["Employee No."].ToString();
                tmpHierarchy = EmpInfoRow[hierarchyLevelGroupingFieldName].ToString();

                if (EmpInfoRows[0] == EmpInfoRow)
                {
                    currentHierarchyGroup = tmpHierarchy;
                    currentCompany = tmpCompany;

                    rowPos = GenerateHeader(workSheet, PeriodFrom, PeriodTo, rowPos, reportHeaderStyle, groupHeaderStyle);
                    rowPos = GenerateCompanyHeader(workSheet, dataSet, tmpCompany, rowPos, groupHeaderStyle);
                    rowPos++;
                    rowPos = GenerateColumnHeader(workSheet, dataSet, hierarchyLevelGroupingFieldName, rowPos, groupHeaderStyle, monthHeaderStyle, columnHeaderStyle);
                }

                sectionEnded = (!currentHierarchyGroup.Equals(tmpHierarchy, StringComparison.CurrentCultureIgnoreCase) ||
                                !currentCompany.Equals(tmpCompany, StringComparison.CurrentCultureIgnoreCase));
                
                if (sectionEnded)
                {
                    if (bolShowIndividual == true)
                    {
                        // print hirarchy total if show employee detail
                        rowPos = GenerateHierarchyTotal(workSheet, rowPos, subTotalStyle, groupRowCount);
                        rowPos += 2;
                    }
                    else
                    {
                        // just print hierarchy line (i.e. not subtotal) if employee detail not shown
                        ushort colPos = 0;
                        NPOI.HSSF.UserModel.HSSFRow sheetRow = (NPOI.HSSF.UserModel.HSSFRow)workSheet.CreateRow((int)rowPos);
                        NPOI.HSSF.UserModel.HSSFCell cell = (NPOI.HSSF.UserModel.HSSFCell)sheetRow.CreateCell((int)colPos);

                        cell = (NPOI.HSSF.UserModel.HSSFCell)sheetRow.CreateCell(colPos);
                        cell.SetCellValue(currentHierarchyGroup);
                        cell.CellStyle = detailTextStyle;
                        colPos++;

                        foreach (DataRow m_hiearchyTotalRow in hierarchyTotalTable.Select("LevelDesc = '" + currentHierarchyGroup + "' ", "payPeriodFr"))
                        {
                            cell = (NPOI.HSSF.UserModel.HSSFCell)sheetRow.CreateCell(colPos);
                            cell.SetCellValue((double)m_hiearchyTotalRow["EE"]);
                            cell.CellStyle = detailNumberStyle;
                            colPos++;

                            cell = (NPOI.HSSF.UserModel.HSSFCell)sheetRow.CreateCell(colPos);
                            cell.SetCellValue((double)m_hiearchyTotalRow["netAmount"]);
                            cell.CellStyle = detailNumberStyle;
                            colPos++;

                            cell = (NPOI.HSSF.UserModel.HSSFCell)sheetRow.CreateCell(colPos);
                            cell.SetCellValue((double)m_hiearchyTotalRow["ER"]);
                            cell.CellStyle = detailNumberStyle;
                            colPos++;

                            cell = (NPOI.HSSF.UserModel.HSSFCell)sheetRow.CreateCell(colPos);
                            cell.CellFormula = "SUM(" + ToCellString(rowPos, colPos - 3) + ":" + ToCellString(rowPos, colPos - 1) + ")";
                            cell.CellStyle = detailNumberStyle;
                            colPos++;
                        }
                        rowPos++;
                        groupRowCount++;
                    }

                    if (!currentCompany.Equals(tmpCompany, StringComparison.CurrentCultureIgnoreCase))
                    {
                        if (bolShowIndividual == true)
                        {
                            // print company header and column header for new company (in show-employee mode)
                            rowPos = GenerateCompanyHeader(workSheet, dataSet, tmpCompany, rowPos, groupHeaderStyle);
                            rowPos++;
                            rowPos = GenerateColumnHeader(workSheet, dataSet, hierarchyLevelGroupingFieldName, rowPos, groupHeaderStyle, monthHeaderStyle, columnHeaderStyle);
                        }
                        else
                        {
                            // print a summary for previous company (in hide employee info mode)
                            rowPos = GenerateHierarchyTotal(workSheet, rowPos, subTotalStyle, groupRowCount);
                            rowPos += 2;
                            rowPos = GenerateCompanyHeader(workSheet, dataSet, tmpCompany, rowPos, groupHeaderStyle);
                            rowPos ++;
                            rowPos = GenerateColumnHeader(workSheet, dataSet, hierarchyLevelGroupingFieldName, rowPos, groupHeaderStyle, monthHeaderStyle, columnHeaderStyle);
                        }
                        currentCompany = tmpCompany;
                        currentHierarchyGroup = tmpHierarchy;
                        groupRowCount = 0;

                        // print company information for new company
                    }
                    else if (!currentHierarchyGroup.Equals(tmpHierarchy, StringComparison.CurrentCultureIgnoreCase))
                    {
                        if (bolShowIndividual == true)
                        {
                            rowPos = GenerateColumnHeader(workSheet, dataSet, hierarchyLevelGroupingFieldName, rowPos, groupHeaderStyle, monthHeaderStyle, columnHeaderStyle);
                            groupRowCount = 0;
                        }
                        else
                        {

                        }
                        currentHierarchyGroup = tmpHierarchy;
                    }

                    hierarchyTotalTable.Clear();
                }

                if (bolShowIndividual == true)
                {
                    groupRowCount++;

                    NPOI.HSSF.UserModel.HSSFRow m_sheetRow = (NPOI.HSSF.UserModel.HSSFRow)workSheet.CreateRow(rowPos);
                    NPOI.HSSF.UserModel.HSSFCell cell;
                    ushort colPos = 0;

                    // Employee Number
                    cell = (NPOI.HSSF.UserModel.HSSFCell)m_sheetRow.CreateCell(colPos);
                    cell.SetCellValue(EmpInfoRow["Employee No."].ToString());
                    cell.CellStyle = detailTextStyle;
                    colPos++;

                    // Employee Name
                    cell = (NPOI.HSSF.UserModel.HSSFCell)m_sheetRow.CreateCell(colPos);
                    cell.SetCellValue(EmpInfoRow["Employee Name"].ToString());
                    cell.CellStyle = detailTextStyle;
                    colPos++;

                    // Alias
                    cell = (NPOI.HSSF.UserModel.HSSFCell)m_sheetRow.CreateCell(colPos);
                    cell.SetCellValue(EmpInfoRow["Alias"].ToString());
                    cell.CellStyle = detailTextStyle;
                    colPos++;

                    // Chinese Name
                    cell = (NPOI.HSSF.UserModel.HSSFCell)m_sheetRow.CreateCell(colPos);
                    cell.SetCellValue(EmpInfoRow["Chinese Name"].ToString());
                    cell.CellStyle = detailTextStyle;
                    colPos++;

                    // hierarchy
                    foreach (DataRow m_hierarchyRow in hierarchyTable.Rows)
                    {
                        cell = (NPOI.HSSF.UserModel.HSSFCell)m_sheetRow.CreateCell(colPos);
                        cell.SetCellValue(EmpInfoRow[m_hierarchyRow["LevelDesc"].ToString()].ToString());
                        cell.CellStyle = detailTextStyle;
                        colPos++;
                    }
                    
                    foreach (DataRow m_payPeriodRow in payPeriodTable.Rows)
                    {
                        //DBFilter m_paymentFilter = new DBFilter();
                        //m_paymentFilter.add(new Match("payPeriodFr", m_payPeriodRow["payPeriodFr"]));
                        //m_paymentFilter.add(new Match("EmpID", row["EmpID"]));

                        double m_er = 0;
                        double m_ee = 0;
                        double m_netAmount = 0;
                        foreach (DataRow m_paymentRow in paymentTable.Select("payPeriodFr='" + ((DateTime)m_payPeriodRow["payPeriodFr"]).ToString("yyyy-MM-dd") + "' AND " +
                                                                            "EmpID=" + EmpInfoRow["EmpID"].ToString()))
                        {
                            m_ee += (double)m_paymentRow["EE"];
                            m_er += (double)m_paymentRow["ER"];
                            m_netAmount += (double)m_paymentRow["netAmount"];
                        }

                        cell = (NPOI.HSSF.UserModel.HSSFCell)m_sheetRow.CreateCell(colPos);
                        cell.SetCellValue(m_ee);
                        cell.CellStyle = detailNumberStyle;
                        colPos++;

                        cell = (NPOI.HSSF.UserModel.HSSFCell)m_sheetRow.CreateCell(colPos);
                        cell.SetCellValue(m_netAmount);
                        cell.CellStyle = detailNumberStyle;
                        colPos++;

                        cell = (NPOI.HSSF.UserModel.HSSFCell)m_sheetRow.CreateCell(colPos);
                        cell.SetCellValue(m_er);
                        cell.CellStyle = detailNumberStyle;
                        colPos++;

                        cell = (NPOI.HSSF.UserModel.HSSFCell)m_sheetRow.CreateCell(colPos);
                        cell.CellFormula = "SUM(" + ToCellString(rowPos, colPos-3) + ":" + ToCellString(rowPos, colPos - 1) + ")";
                        cell.CellStyle = detailNumberStyle;
                        colPos++;
                    }
                    rowPos++;
                }
                else
                {
                    foreach (DataRow m_payPeriodRow in payPeriodTable.Rows)
                    {
                        if (tmpCompany == "Magazines International (Asia) Limited - SALES" && tmpHierarchy == "Sales Division")
                        {
                            double m_trash = 0;
                        }

                        double m_er = 0;
                        double m_ee = 0;
                        double m_netAmount = 0;
                        foreach (DataRow m_paymentRow in paymentTable.Select("payPeriodFr='" + ((DateTime)m_payPeriodRow["payPeriodFr"]).ToString("yyyy-MM-dd") + "' AND " +
                                                                             "EmpID=" + EmpInfoRow["EmpID"].ToString()))
                        {
                            m_ee += (double)m_paymentRow["EE"];
                            m_er += (double)m_paymentRow["ER"];
                            m_netAmount += (double)m_paymentRow["netAmount"];
                        }

                        // find from local HierarchyTotal table and add the new employee amounts
                        DataRow[] m_hierarchyTotalRows = hierarchyTotalTable.Select("payPeriodFr='" + ((DateTime)m_payPeriodRow["payPeriodFr"]).ToString("yyyy-MM-dd") + "' AND " +
//                                                                                    "Company='" + tmpCompany + "' AND " + 
                                                                                    "LevelDesc='" + tmpHierarchy + "' ");
                        if (m_hierarchyTotalRows.Length > 0)
                        {
                            m_hierarchyTotalRows[0]["EE"] = (double)m_hierarchyTotalRows[0]["EE"] + m_ee;
                            m_hierarchyTotalRows[0]["ER"] = (double)m_hierarchyTotalRows[0]["ER"] + m_er;
                            m_hierarchyTotalRows[0]["netAmount"] = (double)m_hierarchyTotalRows[0]["netAmount"] + m_netAmount;
                        }else
                        {
                            DataRow m_newHierarchyTotal = hierarchyTotalTable.Rows.Add();
                            m_newHierarchyTotal["company"] = tmpCompany;
                            m_newHierarchyTotal["LevelDesc"] = tmpHierarchy;
                            m_newHierarchyTotal["payPeriodFr"] = (DateTime)m_payPeriodRow["payPeriodFr"];
                            m_newHierarchyTotal["EE"] = m_ee;
                            m_newHierarchyTotal["ER"] = m_er;
                            m_newHierarchyTotal["netAmount"] = m_netAmount;
                           
                        }
                    }
                }
            }

            // insert last hierarchy total
            if (bolShowIndividual == true)
            {
                rowPos = GenerateHierarchyTotal(workSheet, rowPos, subTotalStyle, groupRowCount);
            }
            else
            {
                NPOI.HSSF.UserModel.HSSFRow m_sheetRow = (NPOI.HSSF.UserModel.HSSFRow)workSheet.CreateRow((int)rowPos);
                NPOI.HSSF.UserModel.HSSFCell cell = (NPOI.HSSF.UserModel.HSSFCell)m_sheetRow.CreateCell(0);
                ushort colPos = 0;

                cell = (NPOI.HSSF.UserModel.HSSFCell)m_sheetRow.CreateCell(colPos);
                cell.SetCellValue(tmpHierarchy);
                cell.CellStyle = detailTextStyle;
                colPos++;

                foreach (DataRow m_hiearchyTotalRow in hierarchyTotalTable.Select("LevelDesc = '" + tmpHierarchy + "' ", "payPeriodFr"))
                {
                    cell = (NPOI.HSSF.UserModel.HSSFCell)m_sheetRow.CreateCell(colPos);
                    cell.SetCellValue((double)m_hiearchyTotalRow["EE"]);
                    cell.CellStyle = detailNumberStyle;
                    colPos++;

                    cell = (NPOI.HSSF.UserModel.HSSFCell)m_sheetRow.CreateCell(colPos);
                    cell.SetCellValue((double)m_hiearchyTotalRow["netAmount"]);
                    cell.CellStyle = detailNumberStyle;
                    colPos++;

                    cell = (NPOI.HSSF.UserModel.HSSFCell)m_sheetRow.CreateCell(colPos);
                    cell.SetCellValue((double)m_hiearchyTotalRow["ER"]);
                    cell.CellStyle = detailNumberStyle;
                    colPos++;

                    cell = (NPOI.HSSF.UserModel.HSSFCell)m_sheetRow.CreateCell(colPos);
                    cell.CellFormula = "SUM(" + ToCellString(rowPos, colPos - 3) + ":" + ToCellString(rowPos, colPos - 1) + ")";
                    cell.CellStyle = detailNumberStyle;
                    colPos++;
                }
                groupRowCount++;
                rowPos++;
                rowPos = GenerateHierarchyTotal(workSheet, rowPos, subTotalStyle, groupRowCount);
            }

            for (int i = 0; i <= reportEndCol; i ++ )
            {
                if (i < summaryStart)
                    workSheet.SetColumnWidth(i, 15 * 254);
                else
                    workSheet.SetColumnWidth(i, 14 * 254);
            }
        }

        protected ushort GenerateCompanyHeader(NPOI.HSSF.UserModel.HSSFSheet workSheet, System.Data.DataSet dataSet,
                                              string CompanyHeader, ushort RowPos,
                                              NPOI.HSSF.UserModel.HSSFCellStyle GroupHeaderStyle)
        {
            NPOI.HSSF.UserModel.HSSFRow row = (NPOI.HSSF.UserModel.HSSFRow)workSheet.CreateRow((int)RowPos);
            NPOI.HSSF.UserModel.HSSFCell cell = (NPOI.HSSF.UserModel.HSSFCell)row.CreateCell(0);
            cell.SetCellValue("Company");
            cell.CellStyle = GroupHeaderStyle;
            cell = (NPOI.HSSF.UserModel.HSSFCell)row.CreateCell(1);
            cell.SetCellValue(CompanyHeader);
            cell.CellStyle = GroupHeaderStyle;

            RowPos++;
            return RowPos;
        }

        protected ushort GenerateColumnHeader(NPOI.HSSF.UserModel.HSSFSheet workSheet, System.Data.DataSet dataSet, 
                                              string HierarchyHeader, ushort RowPos, 
                                              NPOI.HSSF.UserModel.HSSFCellStyle GroupHeaderStyle, 
                                              NPOI.HSSF.UserModel.HSSFCellStyle MonthHeaderStyle,
                                              NPOI.HSSF.UserModel.HSSFCellStyle ColumnHeaderStyle)
        {
            DataTable paymentTable = dataSet.Tables["payment"];
            DataTable hierarchyTable = dataSet.Tables["hierarchy"];
            DataTable payPeriodTable = dataSet.Tables["payPeriod"];


            NPOI.HSSF.UserModel.HSSFRow monthRow;
            NPOI.HSSF.UserModel.HSSFRow columnHeaderRow;
            NPOI.HSSF.UserModel.HSSFRow row = (NPOI.HSSF.UserModel.HSSFRow)workSheet.CreateRow((int)RowPos);
            NPOI.HSSF.UserModel.HSSFCell cell = (NPOI.HSSF.UserModel.HSSFCell)row.CreateCell(0);

            //workSheet.Cells.Add((int)RowPos, 1, HierarchyHeader, xf);
            monthRow = (NPOI.HSSF.UserModel.HSSFRow)workSheet.CreateRow((int)RowPos);
            RowPos++;
            columnHeaderRow = (NPOI.HSSF.UserModel.HSSFRow)workSheet.CreateRow((int)RowPos);

            ushort colPos = 0;

            if (bolShowIndividual == true)
            {
                cell = (NPOI.HSSF.UserModel.HSSFCell)columnHeaderRow.CreateCell(colPos);
                cell.SetCellValue("Employee No.");
                cell.CellStyle = ColumnHeaderStyle;

                colPos++;
                cell = (NPOI.HSSF.UserModel.HSSFCell)columnHeaderRow.CreateCell(colPos);
                cell.SetCellValue("English Name");
                cell.CellStyle = ColumnHeaderStyle;
                
                colPos++;
                cell = (NPOI.HSSF.UserModel.HSSFCell)columnHeaderRow.CreateCell(colPos);
                cell.SetCellValue("Alias");
                cell.CellStyle = ColumnHeaderStyle;
                
                colPos++;
                cell = (NPOI.HSSF.UserModel.HSSFCell)columnHeaderRow.CreateCell(colPos);
                cell.SetCellValue("Chinese Name");
                cell.CellStyle = ColumnHeaderStyle;

                foreach (DataRow hierarchyRow in hierarchyTable.Rows)
                {
                    colPos++;
                    cell = (NPOI.HSSF.UserModel.HSSFCell)columnHeaderRow.CreateCell(colPos);
                    cell.SetCellValue(hierarchyRow["LevelDesc"].ToString());
                    cell.CellStyle = ColumnHeaderStyle;
                }

            }
            else
            {
                cell = (NPOI.HSSF.UserModel.HSSFCell)columnHeaderRow.CreateCell(colPos);
                cell.SetCellValue(HierarchyHeader);
                cell.CellStyle = ColumnHeaderStyle;
            }

            summaryStart = Convert.ToUInt16(colPos + Convert.ToUInt16(1));

            foreach (DataRow payPeriodRow in payPeriodTable.Rows)
            {
                colPos++;

                cell = (NPOI.HSSF.UserModel.HSSFCell)monthRow.CreateCell(colPos);
                cell.SetCellValue(DateTime.Parse(payPeriodRow["payPeriodFr"].ToString()).ToString("dd-MM-yyyy"));
                cell.CellStyle = MonthHeaderStyle;


                workSheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(monthRow.RowNum, monthRow.RowNum, colPos, colPos+3));

                cell = (NPOI.HSSF.UserModel.HSSFCell)columnHeaderRow.CreateCell(colPos);
                cell.SetCellValue("MPF EE");
                cell.CellStyle = ColumnHeaderStyle;

                colPos++;
                cell = (NPOI.HSSF.UserModel.HSSFCell)columnHeaderRow.CreateCell(colPos);
                cell.SetCellValue("Net Payment");
                cell.CellStyle = ColumnHeaderStyle;

                colPos++;
                cell = (NPOI.HSSF.UserModel.HSSFCell)columnHeaderRow.CreateCell(colPos);
                cell.SetCellValue("MPF ER");
                cell.CellStyle = ColumnHeaderStyle;

                colPos++;
                cell = (NPOI.HSSF.UserModel.HSSFCell)columnHeaderRow.CreateCell(colPos);
                cell.SetCellValue("Total");
                cell.CellStyle = ColumnHeaderStyle;
            }

            RowPos++;
            return RowPos;
        }


        protected ushort GenerateHeader(NPOI.HSSF.UserModel.HSSFSheet workSheet, DateTime PeriodFrom, DateTime PeriodTo, ushort RowPos, NPOI.HSSF.UserModel.HSSFCellStyle ReportHeaderStyle, NPOI.HSSF.UserModel.HSSFCellStyle ReportSubHeaderStyle)
        {
            NPOI.HSSF.UserModel.HSSFCell cell = (NPOI.HSSF.UserModel.HSSFCell)workSheet.CreateRow((int)RowPos).CreateCell(0);

            //workSheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(cell.RowIndex, cell.ColumnIndex, cell.RowIndex, cell.ColumnIndex+5));
            cell.SetCellValue("Payroll Allocation Report - Detail");
            cell.CellStyle = ReportHeaderStyle;

            RowPos++;
            NPOI.HSSF.UserModel.HSSFRow row = (NPOI.HSSF.UserModel.HSSFRow)workSheet.CreateRow((int)RowPos);
            cell = (NPOI.HSSF.UserModel.HSSFCell)row.CreateCell(0);
            cell.SetCellValue("From");
            cell.CellStyle = ReportSubHeaderStyle;
            cell = (NPOI.HSSF.UserModel.HSSFCell)row.CreateCell(1);
            cell.SetCellValue(PeriodFrom.ToString("yyyy-MM-dd"));
            cell.CellStyle = ReportSubHeaderStyle;
            cell = (NPOI.HSSF.UserModel.HSSFCell)row.CreateCell(2);
            cell.SetCellValue("To");
            cell.CellStyle = ReportSubHeaderStyle;
            cell = (NPOI.HSSF.UserModel.HSSFCell)row.CreateCell(3);
            cell.SetCellValue(PeriodTo.ToString("yyyy-MM-dd"));
            cell.CellStyle = ReportSubHeaderStyle;

            RowPos += 2;

            return RowPos;
        }

        protected ushort GenerateHierarchyTotal(NPOI.HSSF.UserModel.HSSFSheet workSheet, ushort rowPos, NPOI.HSSF.UserModel.HSSFCellStyle style, ushort groupRowCount)
        {
            if (groupRowCount <= 0)
                return rowPos;

            //workSheet.Cells.Add(rowPos, colPos, currentHierarchyGroup);
            ushort colPos; Convert.ToUInt16(summaryStart - Convert.ToUInt16(1));
            NPOI.HSSF.UserModel.HSSFRow row = (NPOI.HSSF.UserModel.HSSFRow)workSheet.CreateRow(rowPos);
            NPOI.HSSF.UserModel.HSSFCell cell;

            for (colPos = 0; colPos < summaryStart - Convert.ToUInt16(1); colPos++)
            {
                cell = (NPOI.HSSF.UserModel.HSSFCell)row.CreateCell(colPos);
                cell.CellStyle = style;
            }

            cell = (NPOI.HSSF.UserModel.HSSFCell)row.CreateCell(colPos);
            cell.SetCellValue("Total:");
            cell.CellStyle = style;
            colPos++;

            for (; colPos <= reportEndCol; colPos++)
            {
                cell = (NPOI.HSSF.UserModel.HSSFCell)row.CreateCell(colPos);
                cell.CellFormula = "SUM(" + ToCellString(rowPos - groupRowCount, colPos) + ":" + ToCellString(rowPos - Convert.ToUInt16(1), colPos) + ")";
                cell.CellStyle = style;
            }
            rowPos++;

            return rowPos;
        }
    }
}
