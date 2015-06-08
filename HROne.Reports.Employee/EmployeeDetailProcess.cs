using System;
using System.Collections;
using System.Text;
using HROne.Lib.Entities;
using HROne.Common;
using CrystalDecisions.CrystalReports.Engine;
using HROne.DataAccess;
using HROne.CommonLib;

namespace HROne.Reports.Employee
{
    public class EmployeeDetailProcess : GenericReportProcess
    {

        private ArrayList EmpList;

        public EmployeeDetailProcess(DatabaseConnection dbConn, ArrayList EmpList)
            : base(dbConn)
        {
            this.EmpList = EmpList;
        }

        public override ReportDocument GenerateReport()
        {

            if (EmpList.Count > 0)
            {
                string HierarchyLevel1 = " ";
                string HierarchyLevel2 = " ";
                string HierarchyLevel3 = " ";

                ArrayList HierarchyLevelList = EHierarchyLevel.db.select(dbConn, new DBFilter());
                foreach (EHierarchyLevel hLevel in HierarchyLevelList)
                {
                    if (hLevel.HLevelSeqNo.Equals(1))
                        HierarchyLevel1 = hLevel.HLevelDesc;
                    else if (hLevel.HLevelSeqNo.Equals(2))
                        HierarchyLevel2 = hLevel.HLevelDesc;
                    else if (hLevel.HLevelSeqNo.Equals(3))
                        HierarchyLevel3 = hLevel.HLevelDesc;
                }


                DataSet.EmployeeDetail ds = new DataSet.EmployeeDetail();

                ArrayList empExtraFieldList = EEmpExtraField.db.select(dbConn, new DBFilter());
                DataSet.EmployeeDetail.EmpExtraFieldDataTable empExtraFieldTable = ds.EmpExtraField;
                foreach (EEmpExtraField empExtraField in empExtraFieldList)
                {
                    DataSet.EmployeeDetail.EmpExtraFieldRow empExtraFieldRow = empExtraFieldTable.NewEmpExtraFieldRow();
                    empExtraFieldRow.EmpExtraFieldGroupName = empExtraField.EmpExtraFieldGroupName;
                    empExtraFieldRow.EmpExtraFieldID = empExtraField.EmpExtraFieldID;
                    empExtraFieldRow.EmpExtraFieldName = empExtraField.EmpExtraFieldName;

                    if (empExtraFieldRow.IsEmpExtraFieldGroupNameNull())
                        empExtraFieldRow.EmpExtraFieldGroupName = "Extra Information";
                    empExtraFieldTable.Rows.Add(empExtraFieldRow);
                }

                foreach (int EmpID in EmpList)
                {

                    DataSet.EmployeeDetail.employeedetailRow empRow = ImportEmployeeDetailRow(dbConn, ds.employeedetail, EmpID, AppUtils.ServerDateTime().Date);


                    EEmpDocument empDocument = EEmpDocument.GetProfilePhotoEmpDocument(dbConn, EmpID);
                    if (empDocument != null)
                    {

                        string documentFilePath = empDocument.GetDocumentPhysicalPath(dbConn);
                        string transferFilePath = documentFilePath;
                        string strTmpFolder = string.Empty;
                        if (empDocument.EmpDocumentIsCompressed)
                        {
                            transferFilePath = empDocument.GetExtractedFilePath(dbConn);
                        }

                        empRow.PhotoByteArray = ThumbnailToByteArray(transferFilePath, 200, 200);

                        empDocument.RemoveExtractedFile();
                    }


                    DBFilter empFilter = new DBFilter();
                    empFilter.add(new Match("EmpID", EmpID));

                    DBFilter empPosFilter = new DBFilter();
                    empPosFilter.add(new Match("EmpID", EmpID));
                    empPosFilter.add("EmpPosEffFr", false);
                    ArrayList empPosList = EEmpPositionInfo.db.select(dbConn, empPosFilter);
                    DataSet.EmployeeDetail.EmpPositionInfoDataTable empPosTable = ds.EmpPositionInfo;
                    foreach (EEmpPositionInfo empPos in empPosList)
                    {
                        DataSet.EmployeeDetail.EmpPositionInfoRow row = empPosTable.NewEmpPositionInfoRow();

                        row.CompanyID = empPos.CompanyID;
                        row.EmpID = empPos.EmpID;
                        row.EmpPosEffFr = empPos.EmpPosEffFr;
                        if (!empPos.EmpPosEffTo.Ticks.Equals(0))
                            row.EmpPosEffTo = empPos.EmpPosEffTo;
                        row.EmpPosID = empPos.EmpPosID;
                        row.LeavePlanID = empPos.LeavePlanID;
                        row.PayGroupID = empPos.PayGroupID;

                        EPosition position = new EPosition();
                        position.PositionID = empPos.PositionID;
                        if (EPosition.db.select(dbConn, position))
                        {
                            row.PositionCode = position.PositionCode;
                            row.PositionDesc = position.PositionDesc;
                            row.PositionID = position.PositionID;
                        }

                        ERank rank = new ERank();
                        rank.RankID = empPos.RankID;
                        if (ERank.db.select(dbConn, rank))
                        {
                            row.RankCode = rank.RankCode;
                            row.RankDesc = rank.RankDesc;
                            row.RankID = rank.RankID;
                        }

                        row.StaffTypeID = empPos.StaffTypeID;

                        foreach (EHierarchyLevel hLevel in HierarchyLevelList)
                        {
                            if (hLevel.HLevelSeqNo < 4 && hLevel.HLevelSeqNo > 0)
                            {

                                DBFilter empHierarchyFilter = new DBFilter();
                                empHierarchyFilter.add(new Match("EmpPosID", empPos.EmpPosID));
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

                        empPosTable.AddEmpPositionInfoRow(row);

                    }

                    DBFilter empRPFilter = new DBFilter();
                    empRPFilter.add(new Match("EmpID", EmpID));
                    empRPFilter.add("empRPEffFr", false);

                    ArrayList empRPList = EEmpRecurringPayment.db.select(dbConn, empRPFilter);
                    DataSet.EmployeeDetail.EmpRecurringPaymentDataTable empRPTable = ds.EmpRecurringPayment;
                    foreach (EEmpRecurringPayment empRP in empRPList)
                    {
                        DataSet.EmployeeDetail.EmpRecurringPaymentRow row = empRPTable.NewEmpRecurringPaymentRow();

                        row.CurrencyID = empRP.CurrencyID;
                        row.EmpAccID = empRP.EmpAccID;
                        row.EmpID = empRP.EmpID;
                        row.EmpRPAmount = Convert.ToDecimal(empRP.EmpRPAmount);
                        row.EmpRPEffFr = empRP.EmpRPEffFr;
                        if (!empRP.EmpRPEffTo.Ticks.Equals(0))
                            row.EmpRPEffTo = empRP.EmpRPEffTo;
                        row.EmpRPID = empRP.EmpRPID;

                        if (empRP.EmpRPMethod.Equals("A"))
                            row.EmpRPMethod = HROne.Common.WebUtility.GetLocalizedString("Autopay");
                        else if (empRP.EmpRPMethod.Equals("Q"))
                            row.EmpRPMethod = HROne.Common.WebUtility.GetLocalizedString("Cheque");
                        else if (empRP.EmpRPMethod.Equals("C"))
                            row.EmpRPMethod = HROne.Common.WebUtility.GetLocalizedString("Cash");
                        else if (empRP.EmpRPMethod.Equals("O"))
                            row.EmpRPMethod = HROne.Common.WebUtility.GetLocalizedString("Others");


                        row.EmpRPRemark = empRP.EmpRPRemark;
                        row.EmpRPUnit = empRP.EmpRPUnit;

                        EPaymentCode paymentCode = new EPaymentCode();
                        paymentCode.PaymentCodeID = empRP.PayCodeID;
                        if (EPaymentCode.db.select(dbConn, paymentCode))
                        {
                            row.PayCodeID = paymentCode.PaymentCodeID;
                            row.PaymentCode = paymentCode.PaymentCode;
                            row.PaymentCodeDesc = paymentCode.PaymentCodeDesc;
                            row.PaymentCodeID = paymentCode.PaymentCodeID;
//                            row.p
                        }

                        empRPTable.AddEmpRecurringPaymentRow(row);
                    }

                    EmployeeBankAccountProcess.ImportEmpBankAccountRow(dbConn, ds.EmpBankAccount, EmpID);

                    ArrayList empSpouseList = EEmpSpouse.db.select(dbConn, empFilter);
                    DataSet.EmployeeDetail.EmpSpouseDataTable empSpouseTable = ds.EmpSpouse;
                    foreach (EEmpSpouse empSpouse in empSpouseList)
                    {
                        DataSet.EmployeeDetail.EmpSpouseRow row = empSpouseTable.NewEmpSpouseRow();

                        row.EmpID = empSpouse.EmpID;
                        row.EmpSpouseChineseName = empSpouse.EmpSpouseChineseName;
                        //empSpouse.EmpSpouseDateOfBirth;
                        row.EmpSpouseHKID = empSpouse.EmpSpouseHKID;
                        row.EmpSpouseID = empSpouse.EmpSpouseID;
                        row.EmpSpouseOtherName = empSpouse.EmpSpouseOtherName;
                        row.EmpSpousePassportIssuedCountry = empSpouse.EmpSpousePassportIssuedCountry;
                        row.EmpSpousePassportNo = empSpouse.EmpSpousePassportNo;
                        row.EmpSpouseSurname = empSpouse.EmpSpouseSurname;
                        // Start 0000142, KuangWei, 2014-12-21
                        row.EmpGender = empSpouse.EmpGender;
                        if (empSpouse.EmpIsMedicalSchemaInsured)
                            row.EmpIsMedicalSchemaInsured = "Yes";
                        else
                            row.EmpIsMedicalSchemaInsured = "No";
                        row.EmpMedicalEffectiveDate = empSpouse.EmpMedicalEffectiveDate;
                        row.EmpMedicalExpiryDate = empSpouse.EmpMedicalExpiryDate;
                        // End 0000142, KuangWei, 2014-12-21
                        
                        empSpouseTable.AddEmpSpouseRow(row);
                    }

                    ArrayList empDependantList = EEmpDependant.db.select(dbConn, empFilter);
                    DataSet.EmployeeDetail.EmpDependantDataTable empDependantTable = ds.EmpDependant;
                    foreach (EEmpDependant empDependant in empDependantList)
                    {
                        DataSet.EmployeeDetail.EmpDependantRow row = empDependantTable.NewEmpDependantRow();

                        row.EmpID = empDependant.EmpID;
                        row.EmpDependantChineseName = empDependant.EmpDependantChineseName;
                        //empDependant.EmpDependantDateOfBirth;
                        row.EmpDependantGender = empDependant.EmpDependantGender;
                        row.EmpDependantHKID = empDependant.EmpDependantHKID;
                        row.EmpDependantID = empDependant.EmpDependantID;
                        row.EmpDependantOtherName = empDependant.EmpDependantOtherName;
                        row.EmpDependantPassportIssuedCountry = empDependant.EmpDependantPassportIssuedCountry;
                        row.EmpDependantPassportNo = empDependant.EmpDependantPassportNo;
                        row.EmpDependantRelationship = empDependant.EmpDependantRelationship;
                        row.EmpDependantSurname = empDependant.EmpDependantSurname;

                        empDependantTable.AddEmpDependantRow(row);
                    }

                    DBFilter empQualificationFilter = new DBFilter();
                    empQualificationFilter.add(new Match("EmpID", EmpID));
                    empQualificationFilter.add("EmpQualificationFrom", false);

                    ArrayList empQualificationList = EEmpQualification.db.select(dbConn, empQualificationFilter);
                    DataSet.EmployeeDetail.EmpQualificationDataTable empQualificationTable = ds.EmpQualification;
                    foreach (EEmpQualification empQualification in empQualificationList)
                    {
                        DataSet.EmployeeDetail.EmpQualificationRow row = empQualificationTable.NewEmpQualificationRow();

                        row.EmpID = empQualification.EmpID;

                        if (!empQualification.EmpQualificationFrom.Ticks.Equals(0))
                            row.EmpQualificationFrom = empQualification.EmpQualificationFrom;
                        if (!empQualification.EmpQualificationTo.Ticks.Equals(0))
                            row.EmpQualificationTo = empQualification.EmpQualificationTo;
                        row.EmpQualificationID = empQualification.EmpQualificationID;
                        row.EmpQualificationInstitution = empQualification.EmpQualificationInstitution;
                        row.EmpQualificationRemark = empQualification.EmpQualificationRemark;

                        EQualification qualification = new EQualification();
                        qualification.QualificationID = empQualification.QualificationID;
                        if (EQualification.db.select(dbConn, qualification))
                        {
                            row.QualificationID = qualification.QualificationID;
                            row.QualificationCode = qualification.QualificationCode;
                            row.QualificationDesc = qualification.QualificationDesc;
                        }

                        empQualificationTable.AddEmpQualificationRow(row);
                    }
                    ArrayList empSkillList = EEmpSkill.db.select(dbConn, empFilter);
                    DataSet.EmployeeDetail.EmpSkillDataTable empSkillTable = ds.EmpSkill;
                    foreach (EEmpSkill empSkill in empSkillList)
                    {
                        DataSet.EmployeeDetail.EmpSkillRow row = empSkillTable.NewEmpSkillRow();

                        row.EmpID = empSkill.EmpID;

                        row.EmpSkillID = empSkill.EmpSkillID;
                        ESkill skill = new ESkill();
                        skill.SkillID = empSkill.SkillID;
                        if (ESkill.db.select(dbConn, skill))
                        {
                            row.SkillID = skill.SkillID;
                            row.SkillCode = skill.SkillCode;
                            row.SkillDesc = skill.SkillDesc;
                        }
                        ESkillLevel skillLevel = new ESkillLevel();
                        skillLevel.SkillLevelID = empSkill.SkillLevelID;
                        if (ESkillLevel.db.select(dbConn, skillLevel))
                        {
                            row.SkillLevelID = skillLevel.SkillLevelID;
                            row.SkillLevelCode = skillLevel.SkillLevelCode;
                            row.SkillLevelDesc = skillLevel.SkillLevelDesc;
                        }
                        empSkillTable.AddEmpSkillRow(row);
                    }

                    ArrayList empWorkExpList = EEmpWorkExp.db.select(dbConn, empFilter);
                    DataSet.EmployeeDetail.EmpWorkExpDataTable empWorkExpTable = ds.EmpWorkExp;
                    foreach (EEmpWorkExp empWorkExp in empWorkExpList)
                    {
                        DataSet.EmployeeDetail.EmpWorkExpRow row = empWorkExpTable.NewEmpWorkExpRow();

                        row.EmpID = empWorkExp.EmpID;

                        row.EmpWorkExpID = empWorkExp.EmpWorkExpID;
                        row.EmpWorkExpFromMonth = empWorkExp.EmpWorkExpFromMonth;
                        row.EmpWorkExpFromYear = empWorkExp.EmpWorkExpFromYear;
                        row.EmpWorkExpToYear = empWorkExp.EmpWorkExpToYear;
                        row.EmpWorkExpToMonth = empWorkExp.EmpWorkExpToMonth;
                        row.EmpWorkExpPosition = empWorkExp.EmpWorkExpPosition;
                        row.EmpWorkExpCompanyName = empWorkExp.EmpWorkExpCompanyName;
                        row.EmpWorkExpRemark = empWorkExp.EmpWorkExpRemark;

                        empWorkExpTable.AddEmpWorkExpRow(row);
                    }

                    ArrayList empExtraFieldValueList = EEmpExtraFieldValue.db.select(dbConn, empFilter);
                    DataSet.EmployeeDetail.EmpExtraFieldValueDataTable empExtraFieldValueTable = ds.EmpExtraFieldValue;
                    foreach (EEmpExtraFieldValue empExtraFieldValue in empExtraFieldValueList)
                    {
                        DataSet.EmployeeDetail.EmpExtraFieldValueRow row = empExtraFieldValueTable.NewEmpExtraFieldValueRow();

                        row.EmpID = empExtraFieldValue.EmpID;
                        row.EmpExtraFieldValueID = empExtraFieldValue.EmpExtraFieldValueID;
                        row.EmpExtraFieldID = empExtraFieldValue.EmpExtraFieldID;
                        row.EmpExtraFieldValue = empExtraFieldValue.EmpExtraFieldValue;

                        empExtraFieldValueTable.AddEmpExtraFieldValueRow(row);
                    }

                    //DBFilter filter = new DBFilter();
                    //filter.add(new Match("P.EmpID", EmpID));

                    //select = "P.*,EmpPos.*,Pos.*";
                    //from = "from EmpPersonalInfo P LEFT JOIN EmpPositionInfo EmpPos ON P.EmpID=EmpPos.EmpID AND EmpPos.EmpPosEffTo IS NULL LEFT JOIN Position Pos ON EmpPos.PositionID=Pos.PositionID";
                    //filter.loadData(ds, "employeedetail", null, select, from, null);
                    //select = "P.*,Pos.PositionCode,Pos.PositionDesc,R.RankCode, R.RankDesc";
                    //from = "from EmpPositionInfo P LEFT JOIN Position Pos ON P.PositionID=Pos.PositionID LEFT JOIN Rank R ON P.RankID=R.RankID";
                    //filter.loadData(ds, "EmpPositionInfo", null, select, from, null);


                    //select = "P.*,C.*";
                    //from = "from EmpRecurringPayment P LEFT JOIN PaymentCode C ON P.PayCodeID=C.PaymentCodeID ";
                    //filter.loadData(ds, "EmpRecurringPayment", null, select, from, null);

                    //select = "P.*";
                    //from = "from EmpBankAccount P ";
                    //filter.loadData(ds, "EmpBankAccount", null, select, from, null);


                    //select = "P.*";
                    //from = "from EmpSpouse P ";
                    //filter.loadData(ds, "EmpSpouse", null, select, from, null);


                    //select = "P.*";
                    //from = "from EmpDependant P ";
                    //filter.loadData(ds, "EmpDependant", null, select, from, null);


                    //select = "P.*, Q.*";
                    //from = "from EmpQualification P LEFT JOIN Qualification Q on P.QualificationID=Q.QualificationID";
                    //filter.loadData(ds, "EmpQualification", null, select, from, null);


                    //select = "P.*, S.*,L.*";
                    //from = "from EmpSkill P LEFT JOIN Skill S ON P.SkillID=S.SkillID LEFT JOIN SkillLevel L on P.SkillLevelID=L.SkillLevelID";
                    //filter.loadData(ds, "EmpSkill", null, select, from, null);

                }
                //DBAESEncryptStringFieldAttribute.decode(ds.Tables["employeedetail"], "EmpHKID", true);
                //DBAESEncryptStringFieldAttribute.decode(ds.Tables["employeedetail"], "EmpPassportNo", false);
                //DBAESEncryptStringFieldAttribute.decode(ds.Tables["employeedetail"], "EmpResAddr", true);
                //DBAESEncryptStringFieldAttribute.decode(ds.Tables["employeedetail"], "EmpCorAddr", true);
                //DBAESEncryptStringFieldAttribute.decode(ds.Tables["EmpSpouse"], "EmpSpouseHKID", false);
                //DBAESEncryptStringFieldAttribute.decode(ds.Tables["EmpSpouse"], "EmpSpousePassportNo", false);
                //DBAESEncryptStringFieldAttribute.decode(ds.Tables["EmpDependant"], "EmpDependantHKID", false);
                //DBAESEncryptStringFieldAttribute.decode(ds.Tables["EmpDependant"], "EmpDependantPassportNo", false);


                if (reportDocument == null)
                {
                    reportDocument = new ReportTemplate.Report_Employee_Detail();
                }
                else
                {

                }

                reportDocument.SetDataSource(ds);
                reportDocument.SetParameterValue("HierarchyLevel1", HierarchyLevel1);
                reportDocument.SetParameterValue("HierarchyLevel2", HierarchyLevel2);
                reportDocument.SetParameterValue("HierarchyLevel3", HierarchyLevel3);

                reportDocument.SetParameterValue("HierarchyLevel1", HierarchyLevel1, "PositionInfo");
                reportDocument.SetParameterValue("HierarchyLevel2", HierarchyLevel2, "PositionInfo");
                reportDocument.SetParameterValue("HierarchyLevel3", HierarchyLevel3, "PositionInfo");

                return reportDocument;
            }
            else
                return null;

        }
        public static DataSet.EmployeeDetail.employeedetailRow ImportEmployeeDetailRow(DatabaseConnection dbConn, DataSet.EmployeeDetail.employeedetailDataTable empInfoTable, int EmpID, DateTime AsOfDate)
        {
            EEmpPersonalInfo empInfo = new EEmpPersonalInfo();
            empInfo.EmpID = EmpID;
            if (EEmpPersonalInfo.db.select(dbConn, empInfo))
            {
                DataSet.EmployeeDetail.employeedetailRow row = empInfoTable.NewemployeedetailRow();

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


                        DBFilter hLevelFilter = new DBFilter();
                        hLevelFilter.add("HLevelSeqNo", true);
                        string BusinessHierarchy = string.Empty;
                        ArrayList HierarchyLevelList = EHierarchyLevel.db.select(dbConn, hLevelFilter);
                        foreach (EHierarchyLevel hLevel in HierarchyLevelList)
                        {

                            DBFilter empHierarchyFilter = new DBFilter();
                            empHierarchyFilter.add(new Match("EmpPosID", posInfo.EmpPosID));
                            empHierarchyFilter.add(new Match("HLevelID", hLevel.HLevelID));

                            ArrayList empHierarchyList = EEmpHierarchy.db.select(dbConn, empHierarchyFilter);

                            string currentElement = string.Empty;
                            if (empHierarchyList.Count > 0)
                            {
                                EEmpHierarchy empHierarchy = (EEmpHierarchy)empHierarchyList[0];

                                EHierarchyElement hElement = new EHierarchyElement();
                                hElement.HElementID = empHierarchy.HElementID;
                                if (EHierarchyElement.db.select(dbConn, hElement))
                                {

                                    if (empInfoTable.Columns.Contains("HierarchyCode" + hLevel.HLevelSeqNo))
                                        row["HierarchyCode" + hLevel.HLevelSeqNo] = hElement.HElementCode;
                                    if (empInfoTable.Columns.Contains("HierarchyDesc" + hLevel.HLevelSeqNo))
                                        row["HierarchyDesc" + hLevel.HLevelSeqNo] = hElement.HElementDesc;
                                    currentElement = hElement.HElementCode;
                                }
                            }

                            if (string.IsNullOrEmpty(BusinessHierarchy))
                                BusinessHierarchy = currentElement;
                            else
                                BusinessHierarchy += " / " + currentElement;
                        }
                        row.BusinessHierarchy = BusinessHierarchy;
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
                else if (row.EmpResAddrAreaCode.Equals("N"))
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
                    row.YearOfService = Utility.YearDifference(empInfo.EmpServiceDate, row.EmpTermLastDate < AsOfDate ? row.EmpTermLastDate : AsOfDate);
                else
                    row.YearOfService = Utility.YearDifference(empInfo.EmpServiceDate, AsOfDate);

                if (row.YearOfService < 0)
                    row.YearOfService = 0;

                empInfoTable.AddemployeedetailRow(row);

                return row;
            }
            else
                return null;
        }

        protected byte[] ThumbnailToByteArray(string bitmapPath, int maxHeight, int maxwidth)
        {
            if (System.IO.File.Exists(bitmapPath))
            {
                System.Drawing.Image img = System.Drawing.Image.FromFile(bitmapPath);

                int newWidth = img.Size.Width;
                int newHeight = img.Size.Height;

                double ratio = Convert.ToDouble(img.Size.Width) / Convert.ToDouble(img.Size.Height);
                if (ratio > Convert.ToDouble(maxwidth) / Convert.ToDouble(maxHeight))    // Adjust width to 720, height will fall within range
                {
                    newWidth = maxwidth;
                    newHeight = Convert.ToInt32(Convert.ToDouble(img.Size.Height) * Convert.ToDouble(newWidth) / Convert.ToDouble(img.Size.Width));
                }
                else                 // Adjust height to 576, width will fall within range
                {
                    newHeight = maxHeight;
                    newWidth = Convert.ToInt32(Convert.ToDouble(img.Size.Width) * Convert.ToDouble(newHeight) / Convert.ToDouble(img.Size.Height));
                }

                System.Drawing.Bitmap bmp = new System.Drawing.Bitmap(img, newWidth, newHeight);
                bmp.SetResolution(200, 200);
                System.IO.MemoryStream ms = new System.IO.MemoryStream();
                // The following line was ImageFormat.Jpeg, but it caused sizing issues
                // in Crystal Reports.  Changing to ImageFormat.Bmp made the squashed
                // problems go away.
                bmp.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                img.Dispose();
                return ms.ToArray();
            }
            else
                return null;
        }
    }

}
