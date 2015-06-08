using System;
using System.Collections;
using System.Text;
using HROne.Lib.Entities;
using HROne.Common;
using HROne.DataAccess;
using CrystalDecisions.CrystalReports.Engine;
using HROne.Payroll;
namespace HROne.Reports.Payroll
{
    public class EEDAWListReportProcess : GenericReportProcess
    {
        private ArrayList empList;
        private DateTime AsOfDate;


        public EEDAWListReportProcess(DatabaseConnection dbConn, ArrayList EEmpPersonalInfoList, DateTime AsOfDate)
            : base(dbConn)
        {
            empList = EEmpPersonalInfoList;
            this.AsOfDate = AsOfDate;

        }


        public override ReportDocument GenerateReport()
        {

            DataSet.Payroll_EEDAWList dataSet = new DataSet.Payroll_EEDAWList();

            DataSet.Payroll_EEDAWList.EmpDAWDataTable dataTable = dataSet.EmpDAW; //new Payroll_MPFDetailList.EmpInfoDataTable();
            if (empList != null && AsOfDate.Ticks != 0)
            {

                foreach (EEmpPersonalInfo empInfo in empList)
                {

                    EEmpPersonalInfo.db.select(dbConn, empInfo);


                    DBFilter positionFilter = new DBFilter();
                    positionFilter.add(new Match("EmpID", empInfo.EmpID));
                    //if (lngPayPeriodFr != 0 && lngPayPeriodTo != 0)
                    //{
                    positionFilter.add(new Match("EmpPosEffFr", "<=", AsOfDate));
                    OR orPosEffToTerms = new OR();
                    orPosEffToTerms.add(new Match("EmpPosEffTo", ">=", AsOfDate));
                    orPosEffToTerms.add(new NullTerm("EmpPosEffTo"));
                    positionFilter.add(orPosEffToTerms);
                    //}
                    positionFilter.add("EmpPosEffFr", false);

                    ArrayList empPositionInfos = EEmpPositionInfo.db.select(dbConn, positionFilter);
                    EEmpPositionInfo empPositionInfo;
                    if (empPositionInfos.Count > 0)
                        empPositionInfo = (EEmpPositionInfo)empPositionInfos[0];
                    else
                        empPositionInfo = new EEmpPositionInfo();

                    ECompany company = new ECompany();
                    company.CompanyID = empPositionInfo.CompanyID;
                    ECompany.db.select(dbConn, company);

                    EPosition position = new EPosition();
                    position.PositionID = empPositionInfo.PositionID;
                    EPosition.db.select(dbConn, position);

                    DataSet.Payroll_EEDAWList.EmpDAWRow eeDAWRow = dataTable.NewEmpDAWRow();
                    eeDAWRow.EmpID = empInfo.EmpID;
                    eeDAWRow.EmpNo = empInfo.EmpNo;
                    eeDAWRow.EmpName = empInfo.EmpEngFullName;
                    eeDAWRow.EmpAlias = empInfo.EmpAlias;

                    eeDAWRow.JoinDate = empInfo.EmpDateOfJoin;
                    // Start 0000026, Miranda, 2014-04-14
                    eeDAWRow.ServiceDate = empInfo.EmpServiceDate;
                    // End 0000026, Miranda, 2014-04-14

                    /*
                    DateTime dtPeriodFr, dtPeriodTo;
                    double totalWages, totalExcludeWages, totalOTWages;
                    double totalDays, totalExcludeDays, totalOTDays;

                
                    eeDAWRow.AverageWages = PayrollFormula.DailyAverageWages(empInfo.EmpID, dtAsOfDate, out totalWages, out totalDays, out dtPeriodFr, out dtPeriodTo);
                    eeDAWRow.TotalWages = totalWages;
                    eeDAWRow.TotalWorkDays=totalDays;
                    eeDAWRow.PeriodFrom= dtPeriodFr;
                    eeDAWRow.PeriodTo = dtPeriodTo;
                    double eeOTAverageWages = PayrollFormula.OTDailyAverageWages(empInfo.EmpID, dtAsOfDate, out totalOTWages, out totalOTDays,out dtPeriodFr,out dtPeriodTo);
                    eeDAWRow.OTWages = totalOTWages;

                    PayrollFormula.ExcludeWages(empInfo.EmpID, dtAsOfDate, out totalExcludeWages, out totalExcludeDays, out dtPeriodFr, out dtPeriodTo);
                    eeDAWRow.ExcludeWages = totalExcludeWages;
                    eeDAWRow.ExcludeDays = totalExcludeDays;
                    if (eeDAWRow.AverageWages * 0.2 < eeOTAverageWages)
                    {
                        eeDAWRow.AverageWages += eeOTAverageWages;
                        eeDAWRow.ExcludeWages -= totalOTWages;
                    }
                     */
                    AverageWages averageWages = new AverageWages(dbConn, empInfo.EmpID, AsOfDate);
                    eeDAWRow.AverageWages = averageWages.DailyWages();
                    eeDAWRow.TotalWages = averageWages.TotalWagesWithoutOT;
                    eeDAWRow.TotalWorkDays = averageWages.TotalDays;
                    if (!averageWages.PeriodFrom.Ticks.Equals(0))
                        eeDAWRow.PeriodFrom = averageWages.PeriodFrom;
                    if (!averageWages.PeriodTo.Ticks.Equals(0))
                        eeDAWRow.PeriodTo = averageWages.PeriodTo;
                    eeDAWRow.OTWages = averageWages.TotalOTWages;
                    eeDAWRow.IsWagesIncludeOT = averageWages.IsWagesIncludeOT();
                    eeDAWRow.ExcludeWages = averageWages.TotalWagesExclude;
                    eeDAWRow.ExcludeDays = averageWages.TotalDaysExclude;

                    Double totalPeriodPayment;
                    totalPeriodPayment = HROne.Payroll.PayrollProcess.GetTotalPeriodPayRecurringPayment(dbConn, 
                                                                                                        empInfo.EmpID,  
                                                                                                        AsOfDate, 
                                                                                                        true, 
                                                                                                        HROne.Payroll.PayrollProcess.PaymentCodeProrataOptionEnum.PaymentCodeProrataProrataOnly, 
                                                                                                        HROne.Payroll.PayrollProcess.PaymentCodeProrataOptionEnum.PaymentCodeProrataProrataOnly, 
                                                                                                        HROne.Payroll.PayrollProcess.PaymentCodeProrataOptionEnum.PaymentCodeProrataProrataOnly);


                    eeDAWRow.BasicSalary = totalPeriodPayment;

                    // Start 0000050, 2014/06/24, Ricky So
                    //eeDAWRow.PayPeriodDays = GetPayPeriodDays(AsOfDate, empInfo.EmpID);
                    eeDAWRow.PayPeriodDays = DateTime.DaysInMonth(AsOfDate.Year, AsOfDate.Month);
                    // End 0000050

                    //empInfoRow.CompanyName = company.CompanyCode + " - " + company.CompanyName;
                    //empInfoRow.PrintPeriod = strPrintPeriod;
                    //empInfoRow.Position = position.PositionCode + " - " + position.PositionDesc;



                    dataTable.Rows.Add(eeDAWRow);


                }

                if (reportDocument == null)
                {
                    reportDocument = new ReportTemplate.Report_Payroll_EEDAWList();
                    //reportDocument.Load(@"ReportTemplate\Report_Payroll_DiscrepancyList.rpt");
                }
                else
                {

                }
                reportDocument.SetDataSource(dataSet);

                reportDocument.SetParameterValue("AsOfDate", AsOfDate);
                //rpt.SetDataSource((DataTable)mpfTable);
                return reportDocument;
            }
            else
                return null;
        }

        // Start 0000050, 2014/06/24, Ricky So
        //protected int GetPayPeriodDays(DateTime asOfDate, int empID)
        //{
        //    // return 0 if cannot locate a PayPeiod by asOfDate

        //    DBFilter empIDFilter = new DBFilter();
        //    empIDFilter.add(new Match("EmpID", empID));

        //    DBFilter payrollPeriodFilter = new DBFilter();
        //    payrollPeriodFilter.add(new Match("PayPeriodFr", "<=", AsOfDate));
        //    payrollPeriodFilter.add(new Match("PayPeriodTo", ">=", AsOfDate));
        //    payrollPeriodFilter.add(new IN("PayPeriodID", "SELECT PayPeriodID FROM EmpPayroll", empIDFilter));
        //    payrollPeriodFilter.add("PayPeriodID", false);

        //    //userSec001Filter.add(new IN("UserID", "Select distinct UserID from " + EUserGroupAccess.db.dbclass.tableName, userGrpAccessSec001Filter));


        //    ArrayList payrollPeriodList = EPayrollPeriod.db.select(dbConn, payrollPeriodFilter);

        //    if (payrollPeriodList.Count > 0)
        //    {
        //        DateTime fromDate = ((EPayrollPeriod) payrollPeriodList[0]).PayPeriodFr;
        //        DateTime toDate = ((EPayrollPeriod)payrollPeriodList[0]).PayPeriodTo;

        //        return (new TimeSpan(toDate.Ticks - fromDate.Ticks).Days);
        //    }

        //    return 0;
        //}
        // End 0000050
    }
}
