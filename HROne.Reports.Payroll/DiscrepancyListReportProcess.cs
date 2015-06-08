using System;
using System.Collections;
using System.Text;
using HROne.Lib.Entities;
using HROne.Common;
using HROne.DataAccess;
using CrystalDecisions.CrystalReports.Engine;

namespace HROne.Reports.Payroll
{
    public class DiscrepancyListReportProcess : GenericReportProcess
    {
        private ArrayList empList;
        private int intProcessYear, intReferenceYear, intProcessMonth, intReferenceMonth;

        public DiscrepancyListReportProcess(DatabaseConnection dbConn, ArrayList EEmpPersonalInfoList, int ProcessYear, int ProcessMonth, int ReferenceYear, int ReferenceMonth)
            : base(dbConn)
        {
            empList = EEmpPersonalInfoList;
            intProcessMonth = ProcessMonth;
            intProcessYear = ProcessYear;
            intReferenceMonth = ReferenceMonth;
            intReferenceYear = ReferenceYear;

        }


        public override ReportDocument GenerateReport()
        {


            DataSet.Payroll_DiscrepancyList dataSet = new DataSet.Payroll_DiscrepancyList();


            if (intProcessYear > 0 && intProcessMonth > 0 && intReferenceYear > 0 && intReferenceMonth > 0 && empList != null)
            {


                DataSet.Payroll_DiscrepancyList.DiscrepancyListDataTable dataTable = dataSet.DiscrepancyList;

                foreach (EEmpPersonalInfo empInfo in empList)
                {
                    DateTime dtPayPeriodFr = new DateTime(intReferenceYear, intReferenceMonth, 1);
                    DateTime dtPayPeriodTo = new DateTime(intProcessYear, intProcessMonth, 1).AddMonths(1).AddDays(-1);

                    EEmpPersonalInfo.db.select(dbConn, empInfo);


                    DBFilter positionFilter = new DBFilter();
                    positionFilter.add(new Match("EmpID", empInfo.EmpID));
                    //if (lngPayPeriodFr != 0 && lngPayPeriodTo != 0)
                    //{
                    positionFilter.add(new Match("EmpPosEffFr", "<=", dtPayPeriodTo));
                    OR orPosEffToTerms = new OR();
                    orPosEffToTerms.add(new Match("EmpPosEffTo", ">=", dtPayPeriodFr));
                    orPosEffToTerms.add(new NullTerm("EmpPosEffTo"));
                    positionFilter.add(orPosEffToTerms);
                    //}
                    positionFilter.add("EmpPosEffFr", false);

                    ArrayList empPositionInfos = EEmpPositionInfo.db.select(dbConn, positionFilter);
                    EEmpPositionInfo empPositionInfo;
                    if (empPositionInfos.Count > 0)
                    {
                        empPositionInfo = (EEmpPositionInfo)empPositionInfos[0];

                        ECompany company = new ECompany();
                        company.CompanyID = empPositionInfo.CompanyID;
                        ECompany.db.select(dbConn, company);

                        EPayrollGroup payGroup = new EPayrollGroup();
                        payGroup.PayGroupID = empPositionInfo.PayGroupID;
                        EPayrollGroup.db.select(dbConn, payGroup);

                        ERank rank = new ERank();
                        rank.RankID = empPositionInfo.RankID;
                        ERank.db.select(dbConn, rank);

                        EPosition position = new EPosition();
                        position.PositionID = empPositionInfo.PositionID;
                        EPosition.db.select(dbConn, position);

                        DataSet.Payroll_DiscrepancyList.DiscrepancyListRow row = dataTable.NewDiscrepancyListRow();
                        row.EmpID = empInfo.EmpID;
                        row.EmpNo = empInfo.EmpNo;
                        row.EmpName = empInfo.EmpEngFullName;
                        row.CompanyID = company.CompanyID;
                        row.CompanyCode = company.CompanyCode;
                        row.CompanyName = company.CompanyName;
                        row.PayGroupID = payGroup.PayGroupID;
                        row.PayGroupDesc = payGroup.PayGroupDesc;
                        row.Rank = rank.RankDesc;
                        row.Position = position.PositionDesc;
                        row.LastNetPay = getNetIncome(empInfo.EmpID, intReferenceMonth, intReferenceYear);
                        row.CurrentNetPay = getNetIncome(empInfo.EmpID, intProcessMonth, intProcessYear);
                        dataTable.Rows.Add(row);
                    }
                }


                if (reportDocument == null)
                {
                    reportDocument = new ReportTemplate.Report_Payroll_DiscrepancyList();
                    //reportDocument.Load(@"ReportTemplate\Report_Payroll_DiscrepancyList.rpt");
                }
                else
                {

                }
                reportDocument.SetDataSource(dataSet);

                reportDocument.SetParameterValue("ProcessYear", intProcessYear);
                reportDocument.SetParameterValue("ProcessMonth", intProcessMonth);
                reportDocument.SetParameterValue("ReferenceYear", intReferenceYear);
                reportDocument.SetParameterValue("ReferenceMonth", intReferenceMonth);


                //            rpt.SetDataSource((DataTable)mpfTable);
                return reportDocument;

            }
            else
                return null;

        }
        private double getNetIncome(int EmpID, int Month, int Year)
        {
            double netIncome = 0;
            DateTime dtPayPeriodFr = new DateTime(Year, Month, 1);
            DateTime dtPayPeriodTo = new DateTime(Year, Month, 1).AddMonths(1).AddDays(-1);


            DBFilter payPeriodFilter = new DBFilter();
            //if (lngPayPeriodFr != 0 && lngPayPeriodTo != 0)
            //{
            payPeriodFilter.add(new Match("pp.PayPeriodTo", "<=", dtPayPeriodTo));
            payPeriodFilter.add(new Match("pp.PayPeriodTo", ">=", dtPayPeriodFr));
            //}
            DBFilter empPayrollFilter = new DBFilter();
            empPayrollFilter.add(new Match("EmpID", EmpID));
            empPayrollFilter.add(new IN("PayPeriodID", "Select PayPeriodID from PayrollPeriod pp", payPeriodFilter));


            DBFilter payRecordFilter = new DBFilter();
            payRecordFilter.add(new IN("EmpPayrollID", "Select EmpPayrollID from EmpPayroll", empPayrollFilter));
            ArrayList payRecords = EPaymentRecord.db.select(dbConn, payRecordFilter);


            foreach (EPaymentRecord payRecord in payRecords)
            {
                netIncome += payRecord.PayRecActAmount;
            }
            return netIncome;
        }

    }
}
