using System;
using System.Collections;
using System.Text;
using CrystalDecisions.CrystalReports.Engine;
using HROne.Lib.Entities;
using HROne.Common;
using HROne.DataAccess;


namespace HROne.Reports.Payroll
{
    public class PFundStatementProcess :GenericReportProcess 
    {

        private DateTime PayPeriodFr, PayPeriodTo;
        private ArrayList EmpList;
        private int ORSOPlanID;

        public PFundStatementProcess(DatabaseConnection dbConn, ArrayList EmpList, int ORSOPlanID, DateTime PayPeriodFr, DateTime PayPeriodTo)
            : base(dbConn)
        {
            this.EmpList = EmpList;
            this.PayPeriodFr = PayPeriodFr;
            this.PayPeriodTo = PayPeriodTo;
            this.ORSOPlanID = ORSOPlanID;
        }

        public override ReportDocument GenerateReport()
        {
            DataSet.Payroll_ORSOStatement dataSet = new DataSet.Payroll_ORSOStatement();
            DataSet.Payroll_ORSOStatement.ExistingMemberDataTable existingORSO;
            DataSet.Payroll_ORSOStatement.ORSOPlanDataTable orsoPlan;

            existingORSO = dataSet.ExistingMember;

            orsoPlan = dataSet.ORSOPlan;




            if (PayPeriodFr.Ticks != 0 && PayPeriodTo.Ticks != 0 && EmpList != null)
            {

                string strPrintPeriod = PayPeriodFr.ToString("yyyy-MM-dd") + " - " + PayPeriodTo.ToString("yyyy-MM-dd");


                DBFilter payPeriodFilter = new DBFilter();
                payPeriodFilter.add(new Match("pp.PayPeriodFr", "<=", PayPeriodTo));
                payPeriodFilter.add(new Match("pp.PayPeriodTo", ">=", PayPeriodFr));

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


                    DataSet.Payroll_ORSOStatement.ExistingMemberRow existingORSORow = null;
                    foreach (EORSORecord orsoRecord in orsoRecords)
                    {
                        EORSOPlan orsoPlanObject = new EORSOPlan();
                        orsoPlanObject.ORSOPlanID = orsoRecord.ORSOPlanID;
                        if (EORSOPlan.db.select(dbConn, orsoPlanObject))
                        {
                            if (orsoPlan.Select("ORSOPlanID=" + orsoPlanObject.ORSOPlanID).Length == 0)
                            {
                                DataSet.Payroll_ORSOStatement.ORSOPlanRow orsoPlanRow = orsoPlan.NewORSOPlanRow();
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
                        }
                    }

                    if (existingORSORow != null)
                    {
                        existingORSO.Rows.Add(existingORSORow);
                    }
                }




                if (reportDocument == null)
                {
                    reportDocument = new ReportTemplate.Report_Payroll_PFundStatement();
                }
                else
                {

                }


                reportDocument.SetDataSource(dataSet);
                reportDocument.SetParameterValue("ContributionPeriod", PayPeriodFr.ToString("yyyy-MM-dd") + " - " + PayPeriodTo.ToString("yyyy-MM-dd"));
                return reportDocument;
            }
            else
                return null;

        }

        private void LoadExistingMemberRowInfo(EEmpPersonalInfo empInfo, EORSORecord mpfRecord, DataSet.Payroll_ORSOStatement.ExistingMemberRow existingORSORow)
        {
            existingORSORow.EmpID = empInfo.EmpID;
            existingORSORow.EmpNo = empInfo.EmpNo;
            existingORSORow.HKID = empInfo.EmpHKID.Length < 7 ? empInfo.EmpPassportNo : empInfo.EmpHKID;
            existingORSORow.EmpName = empInfo.EmpEngFullName;
            existingORSORow.PeriodFrom = mpfRecord.ORSORecPeriodFr;
            existingORSORow.PeriodTo = mpfRecord.ORSORecPeriodTo;
            existingORSORow.ORSOPlanID = mpfRecord.ORSOPlanID;
            existingORSORow.RelevantIncome = 0;
            existingORSORow.EE = 0;
            existingORSORow.EE = 0;
            existingORSORow.ER = 0;
            existingORSORow.ER = 0;

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
        }
    }
}
