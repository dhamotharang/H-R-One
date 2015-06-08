using System;
using System.Collections;
using System.Text;
using HROne.Lib.Entities;
using HROne.Common;
using HROne.DataAccess;
using CrystalDecisions.CrystalReports.Engine;

namespace HROne.Reports.Payroll
{
    public class NetPaymentListByPaymentMethodProcess : GenericReportProcess
    {

        private ArrayList EmpList;
        private ArrayList PayrollBatchList;
        private bool ByAutoPay, ByCheque, ByCash, ByOthers;

        public NetPaymentListByPaymentMethodProcess(DatabaseConnection dbConn, ArrayList EmpList, ArrayList payBatchList, bool ByAutoPay, bool ByCheque, bool ByCash, bool ByOthers)
            : base(dbConn)
        {
            this.EmpList = EmpList;
            this.PayrollBatchList = payBatchList;
            this.ByAutoPay = ByAutoPay;
            this.ByCheque = ByCheque;
            this.ByCash = ByCash;
            this.ByOthers = ByOthers;
        }


        public override CrystalDecisions.CrystalReports.Engine.ReportDocument GenerateReport()
        {
            

            DataSet.Payroll_NetPaymentList dataSet = new DataSet.Payroll_NetPaymentList();

            if (ByAutoPay)
            {
                DataSet.Payroll_NetPaymentList.PaymentMethodRow payMethodRow = dataSet.PaymentMethod.NewPaymentMethodRow();
                payMethodRow.PayMethodCode = "A";
                payMethodRow.PayMethodDesc = "Autopay";
                dataSet.PaymentMethod.AddPaymentMethodRow(payMethodRow);
            }

            if (ByCheque)
            {
                DataSet.Payroll_NetPaymentList.PaymentMethodRow payMethodRow = dataSet.PaymentMethod.NewPaymentMethodRow();
                payMethodRow.PayMethodCode = "Q";
                payMethodRow.PayMethodDesc = "Cheque";
                dataSet.PaymentMethod.AddPaymentMethodRow(payMethodRow);
            }
            if (ByCash)
            {
                DataSet.Payroll_NetPaymentList.PaymentMethodRow payMethodRow = dataSet.PaymentMethod.NewPaymentMethodRow();
                payMethodRow.PayMethodCode = "C";
                payMethodRow.PayMethodDesc = "Cash";
                dataSet.PaymentMethod.AddPaymentMethodRow(payMethodRow);
            }
            if (ByOthers)
            {
                DataSet.Payroll_NetPaymentList.PaymentMethodRow payMethodRow = dataSet.PaymentMethod.NewPaymentMethodRow();
                payMethodRow.PayMethodCode = "O";
                payMethodRow.PayMethodDesc = "Others";
                dataSet.PaymentMethod.AddPaymentMethodRow(payMethodRow);
            }
            foreach (EEmpPersonalInfo empInfo in EmpList)
            {

                if (EEmpPersonalInfo.db.select(dbConn, empInfo))
                {
                    DataSet.Payroll_NetPaymentList.employeedetailRow row = dataSet.employeedetail.NewemployeedetailRow();

                    {
                        EEmpPositionInfo posInfo = AppUtils.GetLastPositionInfo(dbConn, empInfo.EmpID);

                        if (posInfo != null)
                        {
                            ECompany company = new ECompany();
                            company.CompanyID = posInfo.CompanyID;
                            if (ECompany.db.select(dbConn, company))
                            {
                                row.CompanyCode = company.CompanyCode;
                                row.CompanyID = company.CompanyID;
                                row.CompanyName = company.CompanyName;
                            }
                            else
                            {
                                row.CompanyCode = string.Empty;
                                row.CompanyID = 0;
                                row.CompanyName = "-";
                            }
                            row.EmpPosEffFr = posInfo.EmpPosEffFr;
                            row.EmpPosEffTo = posInfo.EmpPosEffTo;
                            //posInfo.EmploymentTypeID 
                            row.EmpPosID = posInfo.EmpPosID;
                            row.LeavePlanID = posInfo.LeavePlanID;
                            row.PayGroupID = posInfo.PayGroupID;
                            EPosition position = new EPosition();
                            position.PositionID = posInfo.PositionID;
                            if (EPosition.db.select(dbConn, position))
                            {
                                row.PositionCode = position.PositionCode;
                                row.PositionDesc = position.PositionDesc;
                                row.PositionID = position.PositionID;
                            }
                            else
                            {
                                row.PositionCode = string.Empty;
                                row.PositionDesc = "-";
                                row.PositionID = 0;
                            }

                            row.RankID = posInfo.RankID;
                            row.Remark = empInfo.Remark;
                            row.StaffTypeID = posInfo.StaffTypeID;
                            //posInfo.YebPlanID;

                            ArrayList HierarchyLevelList = EHierarchyLevel.db.select(dbConn, new DBFilter());
                            foreach (EHierarchyLevel hLevel in HierarchyLevelList)
                            {
                                if (hLevel.HLevelSeqNo < 4 && hLevel.HLevelSeqNo > 0)
                                {

                                    DBFilter empHierarchyFilter = new DBFilter();
                                    empHierarchyFilter.add(new Match("EmpPosID", posInfo.EmpPosID));
                                    empHierarchyFilter.add(new Match("HLevelID", hLevel.HLevelID));

                                    ArrayList empHierarchyList = EEmpHierarchy.db.select(dbConn, empHierarchyFilter);

                                    if (empHierarchyList.Count > 0)
                                    {
                                        EEmpHierarchy empHierarchy = (EEmpHierarchy)empHierarchyList[0];

                                        EHierarchyElement hElement = new EHierarchyElement();
                                        hElement.HElementID = empHierarchy.HElementID;
                                        EHierarchyElement.db.select(dbConn, hElement);

                                        row["HierarchyCode" + hLevel.HLevelSeqNo] = hElement.HElementCode;
                                        row["HierarchyDesc" + hLevel.HLevelSeqNo] = hElement.HElementDesc;

                                    }
                                }
                            }
                        }
                        else
                        {
                            row.CompanyCode = string.Empty;
                            row.CompanyID = 0;
                            row.CompanyName = "-";
                            row.PositionCode = string.Empty;
                            row.PositionDesc = "-";
                            row.PositionID = 0;
                        }
                    }

                    row.EmpAlias = empInfo.EmpAlias;
                    row.EmpChiFullName = empInfo.EmpChiFullName;
                    row.EmpCorAddr = empInfo.EmpCorAddr;
                    row.EmpDateOfBirth = empInfo.EmpDateOfBirth;
                    row.EmpDateOfJoin = empInfo.EmpDateOfJoin;
                    row.EmpEmail = empInfo.EmpEmail;
                    row.EmpEngOtherName = empInfo.EmpEngOtherName;
                    row.EmpEngSurname = empInfo.EmpEngSurname;
                    row.EmpEngFullName = empInfo.EmpEngFullName;
                    row.EmpGender = empInfo.EmpGender;
                    row.EmpHKID = empInfo.EmpHKID;
                    row.EmpHomePhoneNo = empInfo.EmpHomePhoneNo;
                    row.EmpID = empInfo.EmpID;
                    row.EmpMaritalStatus = empInfo.EmpMaritalStatus;
                    row.EmpMobileNo = empInfo.EmpMobileNo;
                    row.EmpNationality = empInfo.EmpNationality;
                    row.EmpNo = empInfo.EmpNo;
                    row.EmpNoticePeriod = empInfo.EmpNoticePeriod;
                    row.EmpNoticeUnit = empInfo.EmpNoticeUnit;
                    row.EmpOfficePhoneNo = empInfo.EmpOfficePhoneNo;
                    row.EmpPassportExpiryDate = empInfo.EmpPassportExpiryDate;
                    row.EmpPassportIssuedCountry = empInfo.EmpPassportIssuedCountry;
                    row.EmpPassportNo = empInfo.EmpPassportNo;
                    row.EmpPlaceOfBirth = empInfo.EmpPlaceOfBirth;
                    row.EmpProbaLastDate = empInfo.EmpProbaLastDate;
                    row.EmpProbaPeriod = empInfo.EmpProbaPeriod;
                    row.EmpProbaUnit = empInfo.EmpProbaUnit;
                    row.EmpResAddr = empInfo.EmpResAddr;
                    row.EmpResAddrAreaCode = empInfo.EmpResAddrAreaCode;
                    row.EmpServiceDate = empInfo.EmpServiceDate;
                    row.EmpStatus = empInfo.EmpStatus;


                    DBFilter empTermFilter = new DBFilter();
                    empTermFilter.add(new Match("EmpID", empInfo.EmpID));
                    ArrayList empTermList = EEmpTermination.db.select(dbConn, empTermFilter);
                    if (empTermList.Count > 0)
                        row.EmpTermLastDate = ((EEmpTermination)empTermList[0]).EmpTermLastDate;


                    dataSet.employeedetail.AddemployeedetailRow(row);



                    DBFilter empPayrollFilter = new DBFilter();
                    empPayrollFilter.add(new Match("EmpID", empInfo.EmpID));
                    OR orPayrollBatch = new OR();
                    foreach (EPayrollBatch payBatch in PayrollBatchList)
                        orPayrollBatch.add(new Match("PayBatchID", payBatch.PayBatchID));
                    empPayrollFilter.add(orPayrollBatch);

                    IN inEmpPayroll = new IN("EmpPayrollID", "Select EmpPayrollID from EmpPayroll", empPayrollFilter);

                    DBFilter autoPayPaymentRecordFilter = new DBFilter();
                    autoPayPaymentRecordFilter.add(inEmpPayroll);
                    autoPayPaymentRecordFilter.add("EmpAccID", true);
                    ArrayList autoPayPaymentRecords = EPaymentRecord.db.select(dbConn, autoPayPaymentRecordFilter);

                    foreach (EPaymentRecord paymentRecord in autoPayPaymentRecords)
                    {
                        DataSet.Payroll_NetPaymentList.PaymentRecordRow paymentRow = dataSet.PaymentRecord.NewPaymentRecordRow();
                        paymentRow.PayRecID = paymentRecord.PayRecID;
                        paymentRow.EmpID = empInfo.EmpID;
                        paymentRow.PayRecActAmount = paymentRecord.PayRecActAmount;
                        paymentRow.PayRecMethod = paymentRecord.PayRecMethod;
                        paymentRow.EmpAccountNo = string.Empty;

                        EEmpPayroll empPayroll = new EEmpPayroll();
                        empPayroll.EmpPayrollID = paymentRecord.EmpPayrollID;
                        if (EEmpPayroll.db.select(dbConn, empPayroll))
                        {
                            if (!empPayroll.EmpPayValueDate.Ticks.Equals(0))
                                paymentRow.EmpPayValueDate = empPayroll.EmpPayValueDate;
                        }

                        if (paymentRecord.PayRecMethod.Equals("A", StringComparison.CurrentCultureIgnoreCase))
                        {
                            //  Only Autopay payment show account no
                            EEmpBankAccount empBankAccount = new EEmpBankAccount();
                            empBankAccount.EmpBankAccountID = paymentRecord.EmpAccID;
                            if (EEmpBankAccount.db.select(dbConn, empBankAccount))
                                if (empBankAccount.EmpID.Equals(empInfo.EmpID))
                                {
                                    paymentRow.EmpAccountNo = empBankAccount.EmpBankCode + "-" + empBankAccount.EmpBranchCode + "-" + empBankAccount.EmpAccountNo;
                                    paymentRow.EmpAccID = paymentRecord.EmpAccID;
                                }
                        }
                        dataSet.PaymentRecord.AddPaymentRecordRow(paymentRow);
                    }

                }
            }
            if (reportDocument == null)
            {
                reportDocument = new ReportTemplate.Report_Payroll_NetPaymentListByPaymentMethod();
            }
            else
            {

            }

            reportDocument.SetDataSource(dataSet);

            return reportDocument;
        }
    }
}
