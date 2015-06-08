using System;
using System.Collections;
using System.Text;
using HROne.Common;
using HROne.Lib.Entities;
using HROne.DataAccess;
using CrystalDecisions.CrystalReports.Engine;

namespace HROne.MPFFile
{
    public class MPFRemittanceStatementProcess :GenericReportProcess
    {

        //DataSet.Payroll_MPFRemittanceStatement.AdditionalMemberDataTable additionalMPF;
        private DateTime PayPeriodFr, PayPeriodTo;
        private ArrayList EmpList;
        private int MPFPlanID;
        private string ChequeNo;

        public MPFRemittanceStatementProcess(DatabaseConnection dbConn, ArrayList EmpList, int MPFPlanID, DateTime PayPeriodFr, DateTime PayPeriodTo, string ChequeNo)
            : base(dbConn)
        {
            this.EmpList = EmpList;
            this.PayPeriodFr = PayPeriodFr;
            this.PayPeriodTo = PayPeriodTo;
            this.MPFPlanID = MPFPlanID;
            this.ChequeNo = ChequeNo;
        }

        //public System.Data.DataSet GenerateDataSet()
        //{

            //DataSet.Payroll_MPFRemittanceStatement dataSet = new DataSet.Payroll_MPFRemittanceStatement();

            //DataSet.Payroll_MPFRemittanceStatement.ExistingMemberDataTable existingMPF = dataSet.ExistingMember;
            //DataSet.Payroll_MPFRemittanceStatement.NewJoinMemberDataTable newJoinMPF = dataSet.NewJoinMember;
            //additionalMPF = dataSet.AdditionalMember;

            //DataSet.Payroll_MPFRemittanceStatement.MPFPlanDataTable mpfPlan = dataSet.MPFPlan;
            //DataSet.Payroll_MPFRemittanceStatement.MPFSchemeDataTable mpfScheme = dataSet.MPFScheme;

            //string strPrintPeriod = PayPeriodFr.ToString("yyyy-MM-dd") + " - " + PayPeriodTo.ToString("yyyy-MM-dd");

            //DBFilter payPeriodFilter = new DBFilter();
            //payPeriodFilter.add(new Match("pp.PayPeriodFr", "<=", PayPeriodTo));
            //payPeriodFilter.add(new Match("pp.PayPeriodTo", ">=", PayPeriodFr));


            //foreach (EEmpPersonalInfo empInfo in EmpList)
            //{

                //EEmpPersonalInfo.db.select(dbConn, empInfo);



                //DBFilter empPayrollFilter = new DBFilter();
                //empPayrollFilter.add(new Match("EmpID", empInfo.EmpID));
                //empPayrollFilter.add(new IN("PayPeriodID", "Select PayPeriodID from PayrollPeriod pp", payPeriodFilter));


                //DBFilter mpfRecordFilter = new DBFilter();
                //mpfRecordFilter.add(new IN("EmpPayrollID", "Select EmpPayrollID from EmpPayroll", empPayrollFilter));
                //mpfRecordFilter.add(new Match("MPFPlanID", MPFPlanID));
                //mpfRecordFilter.add("MPfRecPeriodFr", true);
                //mpfRecordFilter.add("MPfRecPeriodTo", true);
                //ArrayList mpfRecords = EMPFRecord.db.select(dbConn, mpfRecordFilter);


                //DataSet.Payroll_MPFRemittanceStatement.ExistingMemberRow existingMPFRow = null;
                //DataSet.Payroll_MPFRemittanceStatement.NewJoinMemberRow newJoinMPFRow = null;
                //foreach (EMPFRecord mpfRecord in mpfRecords)
                //{
                //    EMPFPlan mpfPlanObject = new EMPFPlan();
                //    mpfPlanObject.MPFPlanID = mpfRecord.MPFPlanID;
                //    if (EMPFPlan.db.select(dbConn, mpfPlanObject))
                //    {

                //        if (mpfPlan.Select("MPFPlanID=" + mpfPlanObject.MPFPlanID).Length == 0)
                //        {
                //            DataSet.Payroll_MPFRemittanceStatement.MPFPlanRow mpfPlanRow = mpfPlan.NewMPFPlanRow();
                //            mpfPlanRow.MPFPlanID = mpfPlanObject.MPFPlanID;
                //            mpfPlanRow.MPFPlanCode = mpfPlanObject.MPFPlanCode;
                //            mpfPlanRow.MPFPlanCompanyAddress = mpfPlanObject.MPFPlanCompanyAddress;
                //            mpfPlanRow.MPFPlanCompanyName = mpfPlanObject.MPFPlanCompanyName;
                //            mpfPlanRow.MPFPlanContactName = mpfPlanObject.MPFPlanContactName;
                //            mpfPlanRow.MPFPlanContactNo = mpfPlanObject.MPFPlanContactNo;
                //            mpfPlanRow.MPFPlanDesc = mpfPlanObject.MPFPlanDesc;
                //            mpfPlanRow.MPFPlanParticipationNo = mpfPlanObject.MPFPlanParticipationNo;
                //            mpfPlanRow.MPFSchemeID = mpfPlanObject.MPFSchemeID;
                //            if (!string.IsNullOrEmpty(mpfPlanObject.MPFPlanExtendData))
                //            {
                //                System.Xml.XmlNodeList payCenterList = Utility.GetXmlDocumentByDataString(mpfPlanObject.MPFPlanExtendData).DocumentElement.GetElementsByTagName("MPFPlanPayCenter");
                //                if (payCenterList.Count > 0)
                //                {
                //                    mpfPlanRow.MPFHSBCPayCenter = payCenterList[0].InnerText;
                //                }
                //            }
                //            EMPFScheme mpfSchemeObject = new EMPFScheme();
                //            mpfSchemeObject.MPFSchemeID = mpfPlanObject.MPFSchemeID;
                //            if (EMPFScheme.db.select(dbConn, mpfSchemeObject))
                //            {
                //                if (mpfScheme.Select("MPFSchemeID=" + mpfSchemeObject.MPFSchemeID).Length == 0)
                //                {

                //                    DataSet.Payroll_MPFRemittanceStatement.MPFSchemeRow schemeRow = mpfScheme.NewMPFSchemeRow();
                //                    schemeRow.MPFSchemeCode = mpfSchemeObject.MPFSchemeCode;
                //                    schemeRow.MPFSchemeDesc = mpfSchemeObject.MPFSchemeDesc;
                //                    schemeRow.MPFSchemeID = mpfSchemeObject.MPFSchemeID;
                //                    mpfScheme.Rows.Add(schemeRow);
                //                }
                //            }
                //            mpfPlan.Rows.Add(mpfPlanRow);
                //        }
                //    }
                //    EEmpPayroll empPayroll = new EEmpPayroll();
                //    empPayroll.EmpPayrollID = mpfRecord.EmpPayrollID;
                //    EEmpPayroll.db.select(dbConn, empPayroll);
                //    EPayrollPeriod payrollPeriod = new EPayrollPeriod();
                //    payrollPeriod.PayPeriodID = empPayroll.PayPeriodID;
                //    EPayrollPeriod.db.select(dbConn, payrollPeriod);

                //    if (mpfRecord.MPFRecType.Equals("N"))
                //    {
                //        if (newJoinMPFRow == null)
                //        {
                //            newJoinMPFRow = newJoinMPF.NewNewJoinMemberRow();
                //            LoadNewJoinMemberRowInfo(empInfo, mpfRecord, newJoinMPFRow);
                //        }
                //        else
                //        {
                //            if (!(newJoinMPFRow.EmpID == empInfo.EmpID && newJoinMPFRow.PeriodFrom.Equals(mpfRecord.MPFRecPeriodFr) && newJoinMPFRow.PeriodTo.Equals(mpfRecord.MPFRecPeriodTo)))
                //            {
                //                newJoinMPF.Rows.Add(newJoinMPFRow);
                //                newJoinMPFRow = newJoinMPF.NewNewJoinMemberRow();
                //                LoadNewJoinMemberRowInfo(empInfo, mpfRecord, newJoinMPFRow);
                //            }
                //        }

                //        newJoinMPFRow.RelevantIncome += mpfRecord.MPFRecActMCRI;
                //        newJoinMPFRow.MCEE += mpfRecord.MPFRecActMCEE;
                //        newJoinMPFRow.VCEE += mpfRecord.MPFRecActVCEE;
                //        newJoinMPFRow.MCER += mpfRecord.MPFRecActMCER;
                //        newJoinMPFRow.VCER += mpfRecord.MPFRecActVCER;

                //    }
                //    else if (mpfRecord.MPFRecType.Equals("A"))
                //    {
                //        if (newJoinMPFRow != null)
                //            if (newJoinMPFRow.EmpID == empInfo.EmpID)
                //            {
                //                if (!(newJoinMPFRow.PeriodFrom.Equals(mpfRecord.MPFRecPeriodFr) && newJoinMPFRow.PeriodTo.Equals(mpfRecord.MPFRecPeriodTo)))
                //                {
                //                    newJoinMPF.Rows.Add(newJoinMPFRow);
                //                    newJoinMPFRow = newJoinMPF.NewNewJoinMemberRow();
                //                    LoadNewJoinMemberRowInfo(empInfo, mpfRecord, newJoinMPFRow);

                //                }
                //                newJoinMPFRow.RelevantIncome += mpfRecord.MPFRecActMCRI;
                //                newJoinMPFRow.MCEE += mpfRecord.MPFRecActMCEE;
                //                newJoinMPFRow.VCEE += mpfRecord.MPFRecActVCEE;
                //                newJoinMPFRow.MCER += mpfRecord.MPFRecActMCER;
                //                newJoinMPFRow.VCER += mpfRecord.MPFRecActVCER;
                //            }

                //        if (existingMPFRow != null)
                //            if (existingMPFRow.EmpID == empInfo.EmpID)
                //            {
                //                if (!(existingMPFRow.PeriodFrom.Equals(mpfRecord.MPFRecPeriodFr) && existingMPFRow.PeriodTo.Equals(mpfRecord.MPFRecPeriodTo)))
                //                {
                //                    existingMPF.Rows.Add(existingMPFRow);
                //                    existingMPFRow = existingMPF.NewExistingMemberRow();
                //                    LoadExistingMemberRowInfo(empInfo, mpfRecord, existingMPFRow);
                //                }

                //                existingMPFRow.RelevantIncome += mpfRecord.MPFRecActMCRI;
                //                existingMPFRow.MCEE += mpfRecord.MPFRecActMCEE;
                //                existingMPFRow.VCEE += mpfRecord.MPFRecActVCEE;
                //                existingMPFRow.MCER += mpfRecord.MPFRecActMCER;
                //                existingMPFRow.VCER += mpfRecord.MPFRecActVCER;
                //            }

                //    }
                //    else
                //    {
                //        if (existingMPFRow == null)
                //        {
                //            existingMPFRow = existingMPF.NewExistingMemberRow();
                //            LoadExistingMemberRowInfo(empInfo, mpfRecord, existingMPFRow);

                //        }
                //        else
                //        {
                //            if (!(existingMPFRow.EmpID == empInfo.EmpID && existingMPFRow.PeriodFrom.Equals(mpfRecord.MPFRecPeriodFr) && existingMPFRow.PeriodTo.Equals(mpfRecord.MPFRecPeriodTo)))
                //            {
                //                existingMPF.Rows.Add(existingMPFRow);
                //                existingMPFRow = existingMPF.NewExistingMemberRow();
                //                LoadExistingMemberRowInfo(empInfo, mpfRecord, existingMPFRow);
                //            }
                //        }

                //        existingMPFRow.RelevantIncome += mpfRecord.MPFRecActMCRI;
                //        existingMPFRow.MCEE += mpfRecord.MPFRecActMCEE;
                //        existingMPFRow.VCEE += mpfRecord.MPFRecActVCEE;
                //        existingMPFRow.MCER += mpfRecord.MPFRecActMCER;
                //        existingMPFRow.VCER += mpfRecord.MPFRecActVCER;
                //    }
                //}
                //if (existingMPFRow != null)
                //{
                //    existingMPF.Rows.Add(existingMPFRow);
                //}

                //if (newJoinMPFRow != null)
                //{
                //    newJoinMPF.Rows.Add(newJoinMPFRow);
                //}


            //}
            //return dataSet;
        //}

        public override ReportDocument GenerateReport()
        {

            if (PayPeriodFr.Ticks != 0 && PayPeriodTo.Ticks != 0 && EmpList != null)
            {
                MPFFile.GenericMPFFile mpfFile = new MPFFile.GenericMPFFile(dbConn);
                mpfFile.LoadMPFFileDetail(EmpList, MPFPlanID, PayPeriodFr, PayPeriodTo);
                System.Data.DataSet dataSet = mpfFile.CreateRemittanceStatementDataSet();

                if (reportDocument == null)
                {
                    if (mpfFile.MPFSchemeTrusteeCode.Equals("HSBC") || mpfFile.MPFSchemeTrusteeCode.Equals("HangSeng"))
                        reportDocument = new ReportTemplate.Report_Payroll_MPFRemittanceStatement_HSBC();
                    else if (mpfFile.MPFSchemeTrusteeCode.Equals("AIA"))
                        reportDocument = new ReportTemplate.Report_Payroll_MPFRemittanceStatement_AIA();
                    else if (mpfFile.MPFSchemeTrusteeCode.Equals("AXA"))
                        reportDocument = new ReportTemplate.Report_Payroll_MPFRemittanceStatement_AXA();
                    else if (mpfFile.MPFSchemeTrusteeCode.Equals("BOCI"))
                        reportDocument = new ReportTemplate.Report_Payroll_MPFRemittanceStatement_BOCI();
                    else
                        reportDocument = new ReportTemplate.Report_Payroll_MPFRemittanceStatement();
                }
                else
                {

                }


                reportDocument.SetDataSource(dataSet);
                //foreach (ReportDocument o in reportDocument.Subreports)
                //{
                //    o.SetDataSource(dataSet);
                //}

                reportDocument.SetParameterValue("ContributionPeriod", PayPeriodFr.ToString("yyyy-MM-dd") + " - " + PayPeriodTo.ToString("yyyy-MM-dd"));
                reportDocument.SetParameterValue("ContributionPeriodFrom", PayPeriodFr);
                reportDocument.SetParameterValue("ContributionPeriodTo", PayPeriodTo);
                reportDocument.SetParameterValue("TotalAdditionalEmployeeMC", mpfFile.TotalAdditionalEmployeeMC);
                reportDocument.SetParameterValue("TotalAdditionalEmployeeVC", mpfFile.TotalAdditionalEmployeeVC);
                reportDocument.SetParameterValue("TotalBackpayEmployeeMC", mpfFile.TotalBackPaymentEmployeeMC);
                reportDocument.SetParameterValue("TotalBackpayEmployeeVC", mpfFile.TotalBackPaymentEmployeeVC);
                reportDocument.SetParameterValue("TotalExistingEmployeeMC", mpfFile.TotalExistingEmployeeMC);
                reportDocument.SetParameterValue("TotalExistingEmployeeVC", mpfFile.TotalExistingEmployeeVC);
                reportDocument.SetParameterValue("TotalNewJoinEmployeeMC", mpfFile.TotalNewJoinEmployeeMC);
                reportDocument.SetParameterValue("TotalNewJoinEmployeeVC", mpfFile.TotalNewJoinEmployeeVC);
                
                if (mpfFile.MPFSchemeTrusteeCode.Equals("AIA"))
                    reportDocument.SetParameterValue("ChequeNo", this.ChequeNo);
                
                return reportDocument;
            }
            else
                return null;

        }

        //private void LoadExistingMemberRowInfo(EEmpPersonalInfo empInfo, EMPFRecord mpfRecord, DataSet.Payroll_MPFRemittanceStatement.ExistingMemberRow existingMPFRow)
        //{
        //    existingMPFRow.EmpID = empInfo.EmpID;
        //    existingMPFRow.EmpNo = empInfo.EmpNo;
        //    existingMPFRow.HKID = empInfo.EmpHKID.Length < 7 ? empInfo.EmpPassportNo : empInfo.EmpHKID;
        //    existingMPFRow.EmpName = empInfo.EmpEngFullName;
        //    existingMPFRow.PeriodFrom = mpfRecord.MPFRecPeriodFr;
        //    existingMPFRow.PeriodTo = mpfRecord.MPFRecPeriodTo;
        //    existingMPFRow.MPFPlanID = mpfRecord.MPFPlanID;
        //    existingMPFRow.RelevantIncome = 0;
        //    existingMPFRow.MCEE = 0;
        //    existingMPFRow.VCEE = 0;
        //    existingMPFRow.MCER = 0;
        //    existingMPFRow.VCER = 0;

        //    DBFilter empTerminationFilter = new DBFilter();
        //    empTerminationFilter.add(new Match("EmpID", empInfo.EmpID));
        //    empTerminationFilter.add(new Match("EmpTermLastDate", "<=", mpfRecord.MPFRecPeriodTo));
        //    empTerminationFilter.add(new Match("EmpTermLastDate", ">=", mpfRecord.MPFRecPeriodFr));
        //    ArrayList empTerminations = EEmpTermination.db.select(dbConn, empTerminationFilter);
        //    if (empTerminations.Count > 0)
        //    {
        //        EEmpTermination empTermination = (EEmpTermination)empTerminations[0];
        //        existingMPFRow.LastEmploymentDate = empTermination.EmpTermLastDate;

        //        ECessationReason cessationReason = new ECessationReason();
        //        cessationReason.CessationReasonID = empTermination.CessationReasonID;
        //        ECessationReason.db.select(dbConn, cessationReason);
        //        existingMPFRow.TermCode = cessationReason.CessationReasonCode;
        //    }
        //}

        //private void LoadNewJoinMemberRowInfo(EEmpPersonalInfo empInfo, EMPFRecord mpfRecord, DataSet.Payroll_MPFRemittanceStatement.NewJoinMemberRow newJoinMPFRow)
        //{
        //    newJoinMPFRow.EmpID = empInfo.EmpID;
        //    newJoinMPFRow.EmpNo = empInfo.EmpNo;
        //    if (empInfo.EmpHKID.Length < 7)
        //    {
        //        newJoinMPFRow.HKID = empInfo.EmpPassportNo;
        //        newJoinMPFRow.HKIDType = 2;
        //    }
        //    else
        //    {
        //        newJoinMPFRow.HKID = empInfo.EmpHKID;
        //        newJoinMPFRow.HKIDType = 1;
        //    }
        //    newJoinMPFRow.EmpSurname = empInfo.EmpEngSurname;
        //    newJoinMPFRow.EmpOtherName = empInfo.EmpEngOtherName;
        //    newJoinMPFRow.Sex = empInfo.EmpGender;
        //    newJoinMPFRow.DateOfBirth = empInfo.EmpDateOfBirth;
        //    newJoinMPFRow.MemberType = 1;

        //    DBFilter empMPFFilter = new DBFilter();
        //    empMPFFilter.add(new Match("empid", empInfo.EmpID));
        //    empMPFFilter.add("EmpMPFEffFr", true);
        //    ArrayList empMPFs = EEmpMPFPlan.db.select(dbConn, empMPFFilter);
        //    if (empMPFs.Count > 0)
        //        newJoinMPFRow.SchemeJoinDate = ((EEmpMPFPlan)empMPFs[0]).EmpMPFEffFr;

        //    newJoinMPFRow.PeriodFrom = mpfRecord.MPFRecPeriodFr;
        //    newJoinMPFRow.PeriodTo = mpfRecord.MPFRecPeriodTo;
        //    newJoinMPFRow.MPFPlanID = mpfRecord.MPFPlanID;
        //    newJoinMPFRow.RelevantIncome = 0;
        //    newJoinMPFRow.MCEE = 0;
        //    newJoinMPFRow.VCEE = 0;
        //    newJoinMPFRow.MCER = 0;
        //    newJoinMPFRow.VCER = 0;

        //    DBFilter empTerminationFilter = new DBFilter();
        //    empTerminationFilter.add(new Match("EmpID", empInfo.EmpID));
        //    empTerminationFilter.add(new Match("EmpTermLastDate", "<=", mpfRecord.MPFRecPeriodTo));
        //    empTerminationFilter.add(new Match("EmpTermLastDate", ">=", mpfRecord.MPFRecPeriodFr));
        //    ArrayList empTerminations = EEmpTermination.db.select(dbConn, empTerminationFilter);
        //    if (empTerminations.Count > 0)
        //    {
        //        EEmpTermination empTermination = (EEmpTermination)empTerminations[0];

        //        DataSet.Payroll_MPFRemittanceStatement.AdditionalMemberRow additionalMPFRow = additionalMPF.NewAdditionalMemberRow();

        //        additionalMPFRow.EmpID = empInfo.EmpID;
        //        additionalMPFRow.EmpNo = empInfo.EmpNo;
        //        additionalMPFRow.HKID = empInfo.EmpHKID.Length < 7 ? empInfo.EmpPassportNo : empInfo.EmpHKID;
        //        additionalMPFRow.EmpName = empInfo.EmpEngFullName;
        //        additionalMPFRow.PeriodFrom = mpfRecord.MPFRecPeriodFr;
        //        additionalMPFRow.PeriodTo = empTermination.EmpTermLastDate;
        //        additionalMPFRow.MPFPlanID = mpfRecord.MPFPlanID;



        //        ECessationReason cessationReason = new ECessationReason();
        //        cessationReason.CessationReasonID = empTermination.CessationReasonID;
        //        ECessationReason.db.select(dbConn, cessationReason);

        //        additionalMPFRow.TermCode = cessationReason.CessationReasonCode;
        //        additionalMPF.Rows.Add(additionalMPFRow);
        //    }
        //}
    }
}
