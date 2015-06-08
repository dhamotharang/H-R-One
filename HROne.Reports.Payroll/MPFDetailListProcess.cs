using System;
using System.Collections;
using System.Text;
using HROne.DataAccess;
using HROne.Lib.Entities;
using HROne.Common;
using CrystalDecisions.CrystalReports.Engine;

namespace HROne.Reports.Payroll
{
    public class MPFDetailListProcess : GenericReportProcess
    {
        private DateTime PayPeriodFr=new DateTime();
        private DateTime PayPeriodTo = new DateTime();
        private ArrayList EmpList;

        public MPFDetailListProcess(DatabaseConnection dbConn, ArrayList EmpList, DateTime PayPeriodFr, DateTime PayPeriodTo)
            : base(dbConn)
        {
            this.PayPeriodFr = PayPeriodFr;
            this.PayPeriodTo = PayPeriodTo;
            this.EmpList = EmpList;
        }

        public override ReportDocument GenerateReport()
        {


            if ( EmpList !=null)
            {
                DataSet.Payroll_MPFDetailList dataSet = new DataSet.Payroll_MPFDetailList();
                long lngPayPeriodFr = PayPeriodFr.Ticks;
                long lngPayPeriodTo = PayPeriodTo.Ticks;
                string strPrintPeriod=string.Empty;
                if (lngPayPeriodFr != 0 && lngPayPeriodTo != 0)
                    strPrintPeriod = PayPeriodFr.ToString("yyyy-MM-dd") + " - " + PayPeriodTo.ToString("yyyy-MM-dd");

                DataSet.Payroll_MPFDetailList.EmpInfoDataTable empInfoTable = dataSet.EmpInfo; //new Payroll_MPFDetailList.EmpInfoDataTable();
                DataSet.Payroll_MPFDetailList.MPFDetailDataTable mpfTable = dataSet.MPFDetail; //new Payroll_MPFDetailList.MPFDetailDataTable();

                foreach (EEmpPersonalInfo empInfo in EmpList)
                {

                    EEmpPersonalInfo.db.select(dbConn, empInfo);


                    DBFilter positionFilter = new DBFilter();
                    positionFilter.add(new Match("EmpID", empInfo.EmpID));
                    if (lngPayPeriodFr != 0 && lngPayPeriodTo != 0)
                    {
                        positionFilter.add(new Match("EmpPosEffFr", "<=", PayPeriodTo.CompareTo(AppUtils.ServerDateTime()) < 0 ? PayPeriodTo : AppUtils.ServerDateTime()));
                        OR orPosEffToTerms = new OR();
                        orPosEffToTerms.add(new Match("EmpPosEffTo", ">=", PayPeriodFr));
                        orPosEffToTerms.add(new NullTerm("EmpPosEffTo"));
                        positionFilter.add(orPosEffToTerms);
                    }
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

                    DataSet.Payroll_MPFDetailList.EmpInfoRow empInfoRow = empInfoTable.NewEmpInfoRow();
                    empInfoRow.EmpID = empInfo.EmpID;
                    empInfoRow.EmpNo = empInfo.EmpNo;
                    empInfoRow.EmpName = empInfo.EmpEngFullName;
                    empInfoRow.CompanyName = company.CompanyCode + " - " + company.CompanyName;
                    empInfoRow.PrintPeriod = strPrintPeriod;
                    empInfoRow.Position = position.PositionCode + " - " + position.PositionDesc;
                    empInfoRow.DateOfJoin = empInfo.EmpDateOfJoin;

                    empInfoTable.Rows.Add(empInfoRow);

                    DBFilter payPeriodFilter = new DBFilter();
                    if (lngPayPeriodFr != 0 && lngPayPeriodTo != 0)
                    {
                        payPeriodFilter.add(new Match("pp.PayPeriodFr", "<=", PayPeriodTo));
                        payPeriodFilter.add(new Match("pp.PayPeriodTo", ">=", PayPeriodFr));
                    }
                    DBFilter empPayrollFilter = new DBFilter();
                    empPayrollFilter.add(new Match("EmpID", empInfo.EmpID));
                    empPayrollFilter.add(new IN("PayPeriodID", "Select PayPeriodID from PayrollPeriod pp", payPeriodFilter));


                    DBFilter mpfRecordFilter = new DBFilter();
                    mpfRecordFilter.add(new IN("EmpPayrollID", "Select EmpPayrollID from EmpPayroll", empPayrollFilter));
                    ArrayList mpfRecords = EMPFRecord.db.select(dbConn, mpfRecordFilter);


                    foreach (EMPFRecord mpfRecord in mpfRecords)
                    {
                        EEmpPayroll empPayroll = new EEmpPayroll();
                        empPayroll.EmpPayrollID = mpfRecord.EmpPayrollID;
                        EEmpPayroll.db.select(dbConn, empPayroll);
                        EPayrollPeriod payrollPeriod = new EPayrollPeriod();
                        payrollPeriod.PayPeriodID = empPayroll.PayPeriodID;
                        EPayrollPeriod.db.select(dbConn, payrollPeriod);


                        DataSet.Payroll_MPFDetailList.MPFDetailRow mpfRow = mpfTable.NewMPFDetailRow();
                        mpfRow.MPFRecordID = mpfRecord.MPFRecordID;
                        mpfRow.EmpID = empInfo.EmpID;
                        mpfRow.Period = mpfRecord.MPFRecPeriodFr.ToString("yyyy-MM-dd") + " - " + mpfRecord.MPFRecPeriodTo.ToString("yyyy-MM-dd");
                        mpfRow.PeriodFr = mpfRecord.MPFRecPeriodFr;//payrollPeriod.PayPeriodFr;
                        mpfRow.PeriodTo = mpfRecord.MPFRecPeriodTo;//payrollPeriod.PayPeriodTo;
                        mpfRow.RelevantIncome = mpfRecord.MPFRecActMCRI;
                        mpfRow.MCEE = mpfRecord.MPFRecActMCEE;
                        mpfRow.VCEE = mpfRecord.MPFRecActVCEE;
                        mpfRow.MCER = mpfRecord.MPFRecActMCER;
                        mpfRow.VCER = mpfRecord.MPFRecActVCER;
                        mpfTable.Rows.Add(mpfRow);
                    }
                }
                if (reportDocument == null)
                {
                    reportDocument = new ReportTemplate.Report_Payroll_MPFDetailList();

                }
                else
                {

                }

                reportDocument.SetDataSource(dataSet);

                return reportDocument;
            }
            else
                return null;


        }
    }
}
