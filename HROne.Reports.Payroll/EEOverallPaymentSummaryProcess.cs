using System;
using System.Collections;
using System.Text;
using HROne.Lib.Entities;
using HROne.Common;
using CrystalDecisions.CrystalReports.Engine;
using HROne.DataAccess;

namespace HROne.Reports.Payroll
{
    public class EEOverallPaymentSummaryProcess : GenericReportProcess
    {
        DateTime PayPeriodFr;
        ArrayList EmpList;

        DateTime dtPayPeriodFr;
        DateTime dtPayPeriodTo;

        DataSet.Payroll_EEOverallPayrollSummary dataSet = null;

        public EEOverallPaymentSummaryProcess(DatabaseConnection dbConn, ArrayList EmpList, DateTime PayPeriodFr)
            : base(dbConn)
        {
            this.EmpList = EmpList;
            this.PayPeriodFr = PayPeriodFr;
        }

        public override ReportDocument GenerateReport()
        {
            dataSet = new DataSet.Payroll_EEOverallPayrollSummary();


            if (PayPeriodFr.Ticks != 0 && EmpList.Count > 0)
            {
                //  initialize Payment Code for Employer Contribution

                ArrayList mcERPaymentCodeList = HROne.Payroll.PayrollProcess.GetPaymentCodeByPaymentType(dbConn, EPaymentType.SystemPaymentType.MPFEmployerMandatoryContributionPaymentType(dbConn).PaymentTypeCode);
                EPaymentCode mpfMCERPaymentCode;
                if (mcERPaymentCodeList.Count > 0)
                    mpfMCERPaymentCode = (EPaymentCode)mcERPaymentCodeList[0];
                else
                {
                    mpfMCERPaymentCode = new EPaymentCode();
                    mpfMCERPaymentCode.PaymentCodeID = -1;
                    mpfMCERPaymentCode.PaymentCode = "MCER";
                    mpfMCERPaymentCode.PaymentCodeDesc = "Employer Mandatory Contribution";
                }

                ArrayList vcERPaymentCodeList = HROne.Payroll.PayrollProcess.GetPaymentCodeByPaymentType(dbConn, EPaymentType.SystemPaymentType.MPFEmployerVoluntaryContributionPaymentType(dbConn).PaymentTypeCode);
                EPaymentCode mpfVCERPaymentCode;
                if (vcERPaymentCodeList.Count > 0)
                    mpfVCERPaymentCode = (EPaymentCode)vcERPaymentCodeList[0];
                else
                {
                    mpfVCERPaymentCode = new EPaymentCode();
                    mpfVCERPaymentCode.PaymentCodeID = -2;
                    mpfVCERPaymentCode.PaymentCode = "VCER";
                    mpfVCERPaymentCode.PaymentCodeDesc = "Employer Voluntary Contribution";
                }

                ArrayList pfundERPaymentCodeList = HROne.Payroll.PayrollProcess.GetPaymentCodeByPaymentType(dbConn, EPaymentType.SystemPaymentType.PFundEmployerContributionPaymentType(dbConn).PaymentTypeCode);
                EPaymentCode pFundERPaymentCode;
                if (pfundERPaymentCodeList.Count > 0)
                    pFundERPaymentCode = (EPaymentCode)pfundERPaymentCodeList[0];
                else
                {
                    pFundERPaymentCode = new EPaymentCode();
                    pFundERPaymentCode.PaymentCodeID = -3;
                    pFundERPaymentCode.PaymentCode = "PFUNDER";
                    pFundERPaymentCode.PaymentCodeDesc = "Employer P-Fund Contribution";
                }
                
                dtPayPeriodFr = PayPeriodFr;
                dtPayPeriodTo = dtPayPeriodFr.AddMonths(12).AddDays(-1);


                string strPrintPeriod = string.Empty;
                //if (lngPayPeriodFr != 0 && lngPayPeriodTo != 0)
                strPrintPeriod = dtPayPeriodFr.ToString("yyyy-MM-dd") + " - " + dtPayPeriodTo.ToString("yyyy-MM-dd");

                DataSet.Payroll_EEOverallPayrollSummary.EmpInfoDataTable empInfoTable = dataSet.EmpInfo;
                DataSet.Payroll_EEOverallPayrollSummary.OverallPaymentDataTable payTable = dataSet.OverallPayment;

                DataSet.Payroll_EEOverallPayrollSummary.OverallContributionDataTable contributionTable = dataSet.OverallContribution;
                foreach (EEmpPersonalInfo empInfo in EmpList)
                {

                    EEmpPersonalInfo.db.select(dbConn, empInfo);


                    DBFilter positionFilter = new DBFilter();
                    positionFilter.add(new Match("EmpID", empInfo.EmpID));
                    //if (lngPayPeriodFr != 0 && lngPayPeriodTo != 0)
                    //{
                    positionFilter.add(new Match("EmpPosEffFr", "<=", dtPayPeriodTo.CompareTo(AppUtils.ServerDateTime()) < 0 ? dtPayPeriodTo : AppUtils.ServerDateTime()));
                    OR orPosEffToTerms = new OR();
                    orPosEffToTerms.add(new Match("EmpPosEffTo", ">=", dtPayPeriodFr));
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

                    DataSet.Payroll_EEOverallPayrollSummary.EmpInfoRow empInfoRow = empInfoTable.NewEmpInfoRow();
                    empInfoRow.EmpID = empInfo.EmpID;
                    empInfoRow.EmpNo = empInfo.EmpNo;
                    empInfoRow.EmpName = empInfo.EmpEngFullName;
                    empInfoRow.JoinDate = empInfo.EmpDateOfJoin;
                    empInfoRow.CompanyCode = company.CompanyCode;
                    empInfoRow.CompanyName = company.CompanyName;
                    //                empInfoRow.PrintPeriod = strPrintPeriod;
                    empInfoRow.PositionDesc = position.PositionDesc;

                    empInfoTable.Rows.Add(empInfoRow);

                    DBFilter payPeriodFilter = new DBFilter();
                    //if (lngPayPeriodFr != 0 && lngPayPeriodTo != 0)
                    //{
                    payPeriodFilter.add(new Match("pp.PayPeriodTo", "<=", dtPayPeriodTo));
                    payPeriodFilter.add(new Match("pp.PayPeriodTo", ">=", dtPayPeriodFr));
                    //}
                    DBFilter empPayrollFilter = new DBFilter();
                    empPayrollFilter.add(new Match("EmpID", empInfo.EmpID));
                    empPayrollFilter.add(new IN("PayPeriodID", "Select PayPeriodID from PayrollPeriod pp", payPeriodFilter));


                    DBFilter payRecordFilter = new DBFilter();
                    payRecordFilter.add(new IN("EmpPayrollID", "Select EmpPayrollID from EmpPayroll", empPayrollFilter));
                    ArrayList payRecords = EPaymentRecord.db.select(dbConn, payRecordFilter);

                    ArrayList mpfRecords = EMPFRecord.db.select(dbConn, payRecordFilter);

                    ArrayList orsoRecords = EORSORecord.db.select(dbConn, payRecordFilter);
                    foreach (EPaymentRecord payRecord in payRecords)
                    {
                        EEmpPayroll empPayroll = new EEmpPayroll();
                        empPayroll.EmpPayrollID = payRecord.EmpPayrollID;
                        EEmpPayroll.db.select(dbConn, empPayroll);

                        EPayrollPeriod payrollPeriod = new EPayrollPeriod();
                        payrollPeriod.PayPeriodID = empPayroll.PayPeriodID;
                        EPayrollPeriod.db.select(dbConn, payrollPeriod);

                        EPaymentCode payCode = new EPaymentCode();
                        payCode.PaymentCodeID = payRecord.PaymentCodeID;
                        EPaymentCode.db.select(dbConn, payCode);

                        DataSet.Payroll_EEOverallPayrollSummary.OverallPaymentRow payRow = payTable.NewOverallPaymentRow();

                        payRow.EmpID = empInfo.EmpID;
                        payRow.PaymentCode = payCode.PaymentCode;
                        payRow.PaymentCodeDisplaySeqNo = payCode.PaymentCodeDisplaySeqNo;

                        DBFilter taxPaymentMapFilter = new DBFilter();
                        taxPaymentMapFilter.add(new Match("PaymentCodeID", payRecord.PaymentCodeID));
                        if (ETaxPaymentMap.db.count(dbConn, taxPaymentMapFilter) > 0)
                            payRow.IsTaxable = true;
                        else
                            payRow.IsTaxable = false;

                        for (int fieldsNo = 1; fieldsNo < 13; fieldsNo++)
                        {
                            payRow["Amount" + fieldsNo.ToString("00")] = 0;
                        }

                        payRow["Amount" + ((payrollPeriod.PayPeriodTo.Month + 12 - dtPayPeriodFr.Month) % 12 + 1).ToString("00")] = payRecord.PayRecActAmount;

                        payTable.Rows.Add(payRow);
                        try
                        {
                            DataSet.Payroll_EEOverallPayrollSummary.PaymentCodeDataTable paymentCodeTable = dataSet.PaymentCode;
                            DataSet.Payroll_EEOverallPayrollSummary.PaymentCodeRow paymentCodeRow = paymentCodeTable.NewPaymentCodeRow();
                            paymentCodeRow.EmpID = empInfo.EmpID;
                            paymentCodeRow.PaymentCodeID = payCode.PaymentCodeID;
                            paymentCodeRow.PaymentCode = payCode.PaymentCode;
                            paymentCodeRow.PaymentCodeDesc = payCode.PaymentCodeDesc;
                            paymentCodeTable.Rows.Add(paymentCodeRow);
                            
                        }
                        catch
                        {
                        }
                        try
                        {
                            DataSet.Payroll_EEOverallPayrollSummary.GrandTotalPaymentCodeDataTable paymentCodeTable = dataSet.GrandTotalPaymentCode;
                            if (paymentCodeTable.Select("PaymentCodeID=" + payCode.PaymentCodeID.ToString()).Length <= 0)
                            {
                                DataSet.Payroll_EEOverallPayrollSummary.GrandTotalPaymentCodeRow paymentCodeRow = paymentCodeTable.NewGrandTotalPaymentCodeRow();
                                paymentCodeRow.PaymentCodeID = payCode.PaymentCodeID;
                                paymentCodeRow.PaymentCode = payCode.PaymentCode;
                                paymentCodeRow.PaymentCodeDesc = payCode.PaymentCodeDesc;
                                paymentCodeTable.Rows.Add(paymentCodeRow);
                            }
                        }
                        catch
                        {
                        }
                    }
                    foreach (EMPFRecord mpfRecord in mpfRecords)
                    {
                        EEmpPayroll empPayroll = new EEmpPayroll();
                        empPayroll.EmpPayrollID = mpfRecord.EmpPayrollID;
                        EEmpPayroll.db.select(dbConn, empPayroll);

                        EPayrollPeriod payrollPeriod = new EPayrollPeriod();
                        payrollPeriod.PayPeriodID = empPayroll.PayPeriodID;
                        EPayrollPeriod.db.select(dbConn, payrollPeriod);


                        DataSet.Payroll_EEOverallPayrollSummary.OverallContributionRow contributionRow = contributionTable.NewOverallContributionRow();

                        contributionRow.EmpID = empInfo.EmpID;
                        contributionRow.PaymentCode = mpfMCERPaymentCode.PaymentCode;


                        for (int fieldsNo = 1; fieldsNo < 13; fieldsNo++)
                        {
                            contributionRow["Amount" + fieldsNo.ToString("00")] = 0;
                        }

                        contributionRow["Amount" + ((payrollPeriod.PayPeriodTo.Month + 12 - dtPayPeriodFr.Month) % 12 + 1).ToString("00")] = mpfRecord.MPFRecActMCER;

                        contributionTable.Rows.Add(contributionRow);

                        contributionRow = contributionTable.NewOverallContributionRow();

                        contributionRow.EmpID = empInfo.EmpID;
                        contributionRow.PaymentCode = mpfVCERPaymentCode.PaymentCode;


                        for (int fieldsNo = 1; fieldsNo < 13; fieldsNo++)
                        {
                            contributionRow["Amount" + fieldsNo.ToString("00")] = 0;
                        }

                        contributionRow["Amount" + ((payrollPeriod.PayPeriodTo.Month + 12 - dtPayPeriodFr.Month) % 12 + 1).ToString("00")] = mpfRecord.MPFRecActVCER;

                        contributionTable.Rows.Add(contributionRow);
                        try
                        {
                            DataSet.Payroll_EEOverallPayrollSummary.PaymentCodeDataTable paymentCodeTable = dataSet.PaymentCode;
                            DataSet.Payroll_EEOverallPayrollSummary.PaymentCodeRow paymentCodeRow = paymentCodeTable.NewPaymentCodeRow();
                            paymentCodeRow.EmpID = empInfo.EmpID;
                            paymentCodeRow.PaymentCodeID = mpfMCERPaymentCode.PaymentCodeID;
                            paymentCodeRow.PaymentCode = mpfMCERPaymentCode.PaymentCode;
                            paymentCodeRow.PaymentCodeDesc = mpfMCERPaymentCode.PaymentCodeDesc;
                            paymentCodeTable.Rows.Add(paymentCodeRow);
                        }
                        catch
                        {
                        }
                        try
                        {
                            DataSet.Payroll_EEOverallPayrollSummary.GrandTotalPaymentCodeDataTable paymentCodeTable = dataSet.GrandTotalPaymentCode;
                            if (paymentCodeTable.Select("PaymentCodeID=" + mpfMCERPaymentCode.PaymentCodeID.ToString()).Length <= 0)
                            {
                                DataSet.Payroll_EEOverallPayrollSummary.GrandTotalPaymentCodeRow paymentCodeRow = paymentCodeTable.NewGrandTotalPaymentCodeRow();
                                paymentCodeRow.PaymentCodeID = mpfMCERPaymentCode.PaymentCodeID;
                                paymentCodeRow.PaymentCode = mpfMCERPaymentCode.PaymentCode;
                                paymentCodeRow.PaymentCodeDesc = mpfMCERPaymentCode.PaymentCodeDesc;
                                paymentCodeTable.Rows.Add(paymentCodeRow);
                            }
                        }
                        catch
                        {
                        }

                        try
                        {
                            DataSet.Payroll_EEOverallPayrollSummary.PaymentCodeDataTable paymentCodeTable = dataSet.PaymentCode;
                            DataSet.Payroll_EEOverallPayrollSummary.PaymentCodeRow paymentCodeRow = paymentCodeTable.NewPaymentCodeRow();
                            paymentCodeRow.EmpID = empInfo.EmpID;
                            paymentCodeRow.PaymentCodeID = mpfVCERPaymentCode.PaymentCodeID;
                            paymentCodeRow.PaymentCode = mpfVCERPaymentCode.PaymentCode;
                            paymentCodeRow.PaymentCodeDesc = mpfVCERPaymentCode.PaymentCodeDesc;
                            paymentCodeTable.Rows.Add(paymentCodeRow);
                        }
                        catch
                        {
                        }
                        try
                        {
                            DataSet.Payroll_EEOverallPayrollSummary.GrandTotalPaymentCodeDataTable paymentCodeTable = dataSet.GrandTotalPaymentCode;
                            if (paymentCodeTable.Select("PaymentCodeID=" + mpfVCERPaymentCode.PaymentCodeID.ToString()).Length <= 0)
                            {
                                DataSet.Payroll_EEOverallPayrollSummary.GrandTotalPaymentCodeRow paymentCodeRow = paymentCodeTable.NewGrandTotalPaymentCodeRow();
                                paymentCodeRow.PaymentCodeID = mpfVCERPaymentCode.PaymentCodeID;
                                paymentCodeRow.PaymentCode = mpfVCERPaymentCode.PaymentCode;
                                paymentCodeRow.PaymentCodeDesc = mpfVCERPaymentCode.PaymentCodeDesc;
                                paymentCodeTable.Rows.Add(paymentCodeRow);
                            }
                        }
                        catch
                        {
                        }

                    }
                    foreach (EORSORecord orsoRecord in orsoRecords)
                    {
                        EEmpPayroll empPayroll = new EEmpPayroll();
                        empPayroll.EmpPayrollID = orsoRecord.EmpPayrollID;
                        EEmpPayroll.db.select(dbConn, empPayroll);

                        EPayrollPeriod payrollPeriod = new EPayrollPeriod();
                        payrollPeriod.PayPeriodID = empPayroll.PayPeriodID;
                        EPayrollPeriod.db.select(dbConn, payrollPeriod);


                        DataSet.Payroll_EEOverallPayrollSummary.OverallContributionRow contributionRow = contributionTable.NewOverallContributionRow();

                        contributionRow.EmpID = empInfo.EmpID;
                        contributionRow.PaymentCode = pFundERPaymentCode.PaymentCode;


                        for (int fieldsNo = 1; fieldsNo < 13; fieldsNo++)
                        {
                            contributionRow["Amount" + fieldsNo.ToString("00")] = 0;
                        }

                        contributionRow["Amount" + ((payrollPeriod.PayPeriodTo.Month + 12 - dtPayPeriodFr.Month) % 12 + 1).ToString("00")] = orsoRecord.ORSORecActER;

                        contributionTable.Rows.Add(contributionRow);


                        try
                        {
                            DataSet.Payroll_EEOverallPayrollSummary.PaymentCodeDataTable paymentCodeTable = dataSet.PaymentCode;
                            DataSet.Payroll_EEOverallPayrollSummary.PaymentCodeRow paymentCodeRow = paymentCodeTable.NewPaymentCodeRow();
                            paymentCodeRow.EmpID = empInfo.EmpID;
                            paymentCodeRow.PaymentCodeID = pFundERPaymentCode.PaymentCodeID;
                            paymentCodeRow.PaymentCode = pFundERPaymentCode.PaymentCode;
                            paymentCodeRow.PaymentCodeDesc = pFundERPaymentCode.PaymentCode;
                            paymentCodeTable.Rows.Add(paymentCodeRow);
                        }
                        catch
                        {
                        }
                        try
                        {
                            DataSet.Payroll_EEOverallPayrollSummary.GrandTotalPaymentCodeDataTable paymentCodeTable = dataSet.GrandTotalPaymentCode;
                            if (paymentCodeTable.Select("PaymentCodeID=" + pFundERPaymentCode.PaymentCodeID.ToString()).Length <= 0)
                            {
                                DataSet.Payroll_EEOverallPayrollSummary.GrandTotalPaymentCodeRow paymentCodeRow = paymentCodeTable.NewGrandTotalPaymentCodeRow();
                                paymentCodeRow.PaymentCodeID = pFundERPaymentCode.PaymentCodeID;
                                paymentCodeRow.PaymentCode = pFundERPaymentCode.PaymentCode;
                                paymentCodeRow.PaymentCodeDesc = pFundERPaymentCode.PaymentCode;
                                paymentCodeTable.Rows.Add(paymentCodeRow);
                            }
                        }
                        catch
                        {
                        }

                    }
                }
                if (reportDocument == null)
                {
                    reportDocument = new ReportTemplate.Report_Payroll_EEOverallPaymentSummary();
                    //reportDocument.Load(@"ReportTemplate\Report_Payroll_DiscrepancyList.rpt");
                }
                else
                {

                }



                return base.GenerateReport();
            }
            else
                return null;
        }
        protected override void setDataSource()
        {
            reportDocument.SetDataSource(dataSet);
            //reportDocument.Subreports["Report_Payroll_EEOverallPaymentSummary_Payment"].SetDataSource(dataSet);
        }
        protected override void setParameters()
        {
            reportDocument.SetParameterValue("ReportPeriod", dtPayPeriodFr.ToString("yyyy-MM-dd") + " - " + dtPayPeriodTo.ToString("yyyy-MM-dd"));
            for (int i = 0; i < 12; i++)
            {
                reportDocument.SetParameterValue("Month" + (i + 1).ToString("00"), dtPayPeriodFr.AddMonths(i).ToString("MMM"));
                //reportDocument.SetParameterValue("Month" + (i + 1).ToString("00"), dtPayPeriodFr.AddMonths(i).ToString("MMM"), "Report_Payroll_EEOverallPaymentSummary_Payment");
                //reportDocument.SetParameterValue("Month" + (i + 1).ToString("00"), dtPayPeriodFr.AddMonths(i).ToString("MMM"), "Report_Payroll_EEOverallPaymentSummary_Contribution");
            }
        }
    }
}