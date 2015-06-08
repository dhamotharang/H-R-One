using System;
using System.Collections;
using System.Text;
using HROne.Common;
using HROne.Lib.Entities;
using HROne.LeaveCalc;
using CrystalDecisions.CrystalReports.Engine;
using HROne.DataAccess;

namespace HROne.Reports.Payroll
{
    public class LongServicePaymentSeverancePaymentProcess : GenericReportProcess
    {
        private ArrayList values;
        private DateTime AsOfDate;

        public LongServicePaymentSeverancePaymentProcess(DatabaseConnection dbConn, ArrayList EmpList, DateTime AsOfDate)
            : base(dbConn)
        {
            this.values = EmpList;
            this.AsOfDate = AsOfDate;
        }

        public override ReportDocument GenerateReport()
        {
            string HierarchyLevel1 = string.Empty;
            string HierarchyLevel2 = string.Empty;
            string HierarchyLevel3 = string.Empty;
            if (values.Count > 0)
            {

                DataSet.Payroll_LongServicePaymentSeverancePayment_List ds = new DataSet.Payroll_LongServicePaymentSeverancePayment_List();

                ArrayList HierarchyLevelList = EHierarchyLevel.db.select(dbConn, new DBFilter());
                foreach (EHierarchyLevel HierarchyLevel in HierarchyLevelList)
                {
                    if (HierarchyLevel.HLevelSeqNo.Equals(1))
                        HierarchyLevel1 = HierarchyLevel.HLevelDesc;
                    else if (HierarchyLevel.HLevelSeqNo.Equals(2))
                        HierarchyLevel2 = HierarchyLevel.HLevelDesc;
                    else if (HierarchyLevel.HLevelSeqNo.Equals(3))
                        HierarchyLevel3 = HierarchyLevel.HLevelDesc;

                }


                foreach (object obj in values)
                {
                    int EmpID = -1;
                    if (obj is int)
                        EmpID = (int)obj;
                    else if (obj is EEmpPersonalInfo)
                        EmpID = ((EEmpPersonalInfo)obj).EmpID;

                    ImportEmployeeDetailRow(ds.employeedetail, EmpID, AsOfDate);
                    DataSet.Payroll_LongServicePaymentSeverancePayment_List.LongServicePaymentSeverancePaymentRow row = ds.LongServicePaymentSeverancePayment.NewLongServicePaymentSeverancePaymentRow();
                    row.EmpID = EmpID;

                    HROne.Payroll.FinalPaymentProcess finalPaymentProcess = new HROne.Payroll.FinalPaymentProcess(dbConn, EmpID, AsOfDate, string.Empty);
                    string remark;
                    row.LongServicePayment = finalPaymentProcess.GetLongServicePaymentSeverancePaymentAmount(true, out remark);
                    row.SeverancePayment = finalPaymentProcess.GetLongServicePaymentSeverancePaymentAmount(false, out remark);

                    DBFilter mpfRecordFilter = new DBFilter();
                    DBFilter empPayrollFilter = new DBFilter();
                    empPayrollFilter.add(new Match("ep.EmpID", EmpID));
                    empPayrollFilter.add(new MatchField("ep.EmpPayrollID", "MPFRecord.EmpPayrollID"));
                    mpfRecordFilter.add(new Exists("EmpPayroll ep", empPayrollFilter));
                    ArrayList mpfRecordList = EMPFRecord.db.select(dbConn, mpfRecordFilter);

                    double mpfEmployerContribution=0;
                    foreach (EMPFRecord mpfRecord in mpfRecordList)
                        mpfEmployerContribution += mpfRecord.MPFRecActMCER;
                    row.EmployerContribution = mpfEmployerContribution;

                    ds.LongServicePaymentSeverancePayment.Rows.Add(row);
                }
                if (reportDocument == null)
                {
                    reportDocument = new ReportTemplate.Report_Payroll_LongServicePaymentSeverancePayment_List();
                }
                else
                {

                }




                reportDocument.SetDataSource(ds);
                reportDocument.SetParameterValue("HierarchyLevel1", HierarchyLevel1);
                reportDocument.SetParameterValue("HierarchyLevel2", HierarchyLevel2);
                reportDocument.SetParameterValue("HierarchyLevel3", HierarchyLevel3);
                if (reportDocument.ParameterFields["AsOfDate"] != null)
                    reportDocument.SetParameterValue("AsOfDate", this.AsOfDate);

                return reportDocument;
            }
            else
                return null;
        }
        public void ImportEmployeeDetailRow(DataSet.Payroll_LongServicePaymentSeverancePayment_List.employeedetailDataTable empInfoTable, int EmpID, DateTime AsOfDate)
        {
            EEmpPersonalInfo empInfo = new EEmpPersonalInfo();
            empInfo.EmpID = EmpID;
            if (EEmpPersonalInfo.db.select(dbConn, empInfo))
            {
                DataSet.Payroll_LongServicePaymentSeverancePayment_List.employeedetailRow row = empInfoTable.NewemployeedetailRow();

                {
                    EEmpPositionInfo posInfo = AppUtils.GetLastPositionInfo(dbConn, AsOfDate, EmpID);

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
                if (row.EmpResAddrAreaCode.Equals("H"))
                    row.EmpResAddr += ", " + HROne.Common.WebUtility.GetLocalizedString("Hong Kong");
                else if (row.EmpResAddrAreaCode.Equals("K"))
                    row.EmpResAddr += ", " + HROne.Common.WebUtility.GetLocalizedString("Kowloon");
                else if (row.EmpResAddrAreaCode.Equals("H"))
                    row.EmpResAddr += ", " + HROne.Common.WebUtility.GetLocalizedString("New Territories");
                //else
                //    row.EmpResAddr = ", " + HROne.Common.WebUtility.GetLocalizedString("Overseas");
                row.EmpServiceDate = empInfo.EmpServiceDate;
                row.EmpStatus = empInfo.EmpStatus;


                DBFilter empTermFilter = new DBFilter();
                empTermFilter.add(new Match("EmpID", EmpID));
                ArrayList empTermList = EEmpTermination.db.select(dbConn, empTermFilter);
                if (empTermList.Count > 0)
                    row.EmpTermLastDate = ((EEmpTermination)empTermList[0]).EmpTermLastDate;

                if (!row.IsEmpTermLastDateNull())
                    row.YearOfService = HROne.CommonLib.Utility.YearDifference(empInfo.EmpServiceDate, row.EmpTermLastDate < AsOfDate ? row.EmpTermLastDate : AsOfDate);
                else
                    row.YearOfService = HROne.CommonLib.Utility.YearDifference(empInfo.EmpServiceDate, AsOfDate);

                if (row.YearOfService < 0)
                    row.YearOfService = 0;

                empInfoTable.AddemployeedetailRow(row);

                //return row;
            }
            //else
            //    return null;
        }
    }
}
