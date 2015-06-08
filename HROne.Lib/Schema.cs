using System;
using System.Collections.Generic;
using System.Text;
using HROne.Lib.Entities;
using HROne.DataAccess;

namespace HROne.Lib
{
    public abstract class Schema
    {
        public static string GetValueFromID(DatabaseConnection dbConn, string fieldName, string fieldValue)
        {
            try
            {
                string tmpfieldName = fieldName;
                if (tmpfieldName.Equals("PreviousEmpID", StringComparison.CurrentCultureIgnoreCase) || tmpfieldName.Equals("NewEmpID", StringComparison.CurrentCultureIgnoreCase))
                    tmpfieldName = "EmpID";
                if (tmpfieldName.EndsWith("PaymentCodeID", StringComparison.CurrentCultureIgnoreCase) || tmpfieldName.EndsWith("PayCodeID", StringComparison.CurrentCultureIgnoreCase))
                    tmpfieldName = "PaymentCodeID";
                if (tmpfieldName.Equals("DefaultMPFPlanID", StringComparison.CurrentCultureIgnoreCase))
                    tmpfieldName = "MPFPlanID";
                if (tmpfieldName.Equals("AttendanceFormulaPayFormID", StringComparison.CurrentCultureIgnoreCase) || tmpfieldName.Equals("ReferencePayFormID", StringComparison.CurrentCultureIgnoreCase))
                    tmpfieldName = "PayFormID";
                if (tmpfieldName.StartsWith("LeaveCode", StringComparison.CurrentCultureIgnoreCase) && tmpfieldName.EndsWith("Formula", StringComparison.CurrentCultureIgnoreCase))
                    tmpfieldName = "PayFormID";
                if (tmpfieldName.StartsWith("PayGroup", StringComparison.CurrentCultureIgnoreCase) && tmpfieldName.EndsWith("Formula", StringComparison.CurrentCultureIgnoreCase))
                    tmpfieldName = "PayFormID";
                if (tmpfieldName.Equals("CurrentPayPeriodID", StringComparison.CurrentCultureIgnoreCase))
                    tmpfieldName = "PayPeriodID";
                if (tmpfieldName.Equals("EmpPosDefaultRosterCodeID", StringComparison.CurrentCultureIgnoreCase))
                    tmpfieldName = "RosterCodeID";
                if (tmpfieldName.Equals("RosterClientMappingSiteCodeToHLevelID", StringComparison.CurrentCultureIgnoreCase))
                    tmpfieldName = "HLevelID";
                if (tmpfieldName.Equals("EmpFirstAuthorizationGp", StringComparison.CurrentCultureIgnoreCase) || tmpfieldName.Equals("EmpSecondAuthorizationGp", StringComparison.CurrentCultureIgnoreCase))
                    tmpfieldName = "AuthorizationGroupID";



                if (tmpfieldName.StartsWith("A", StringComparison.CurrentCultureIgnoreCase))
                {
                    if (tmpfieldName.Equals("ALProrataRoundingRuleID", StringComparison.CurrentCultureIgnoreCase))
                    {
                        EALProrataRoundingRule obj = new EALProrataRoundingRule();
                        obj.ALProrataRoundingRuleID = int.Parse(fieldValue);
                        if (EALProrataRoundingRule.db.select(dbConn, obj))
                            return obj.ALProrataRoundingRuleCode + " - " + obj.ALProrataRoundingRuleDesc;
                        else
                            return string.Empty;
                    }
                    if (tmpfieldName.Equals("AttendancePlanID", StringComparison.CurrentCultureIgnoreCase))
                    {
                        EAttendancePlan obj = new EAttendancePlan();
                        obj.AttendancePlanID = int.Parse(fieldValue);
                        if (EAttendancePlan.db.select(dbConn, obj))
                            return obj.AttendancePlanCode + " - " + obj.AttendancePlanDesc;
                        else
                            return string.Empty;
                    }
                    if (tmpfieldName.Equals("AuthorizationGroupID", StringComparison.CurrentCultureIgnoreCase))
                    {
                        EAuthorizationGroup obj = new EAuthorizationGroup();
                        obj.AuthorizationGroupID = int.Parse(fieldValue);
                        if (EAuthorizationGroup.db.select(dbConn, obj))
                            return obj.AuthorizationCode + " - " + obj.AuthorizationDesc;
                        else
                            return string.Empty;
                    }
                    if (tmpfieldName.Equals("AVCPlanID", StringComparison.CurrentCultureIgnoreCase))
                    {
                        EAVCPlan obj = new EAVCPlan();
                        obj.AVCPlanID = int.Parse(fieldValue);
                        if (EAVCPlan.db.select(dbConn, obj))
                            return obj.AVCPlanCode + " - " + obj.AVCPlanDesc;
                        else
                            return string.Empty;
                    }
                }
                else if (tmpfieldName.StartsWith("C", StringComparison.CurrentCultureIgnoreCase))
                {
                    if (tmpfieldName.Equals("CessationReasonID", StringComparison.CurrentCultureIgnoreCase))
                    {
                        ECessationReason obj = new ECessationReason();
                        obj.CessationReasonID = int.Parse(fieldValue);
                        if (ECessationReason.db.select(dbConn, obj))
                            return obj.CessationReasonCode + " - " + obj.CessationReasonDesc;
                        else
                            return string.Empty;
                    }
                    if (tmpfieldName.Equals("CompanyID", StringComparison.CurrentCultureIgnoreCase))
                    {
                        ECompany obj = new ECompany();
                        obj.CompanyID = int.Parse(fieldValue);
                        if (ECompany.db.select(dbConn, obj))
                            return obj.CompanyCode + " - " + obj.CompanyName;
                        else
                            return string.Empty;
                    }


                    if (tmpfieldName.Equals("CostAllocationDetailID", StringComparison.CurrentCultureIgnoreCase))
                    {
                        ECostAllocationDetail obj = new ECostAllocationDetail();
                        obj.CostAllocationDetailID = int.Parse(fieldValue);
                        if (ECostAllocationDetail.db.select(dbConn, obj))
                            return GetValueFromID(dbConn, "PayCodeID", obj.PaymentCodeID.ToString()) + ", " + GetValueFromID(dbConn, "CostCenterID", obj.CostCenterID.ToString());
                        else
                            return string.Empty;
                    }
                    if (tmpfieldName.Equals("CostAllocationID", StringComparison.CurrentCultureIgnoreCase))
                    {
                        ECostAllocation obj = new ECostAllocation();
                        obj.CostAllocationID = int.Parse(fieldValue);
                        if (ECostAllocation.db.select(dbConn, obj))
                            return GetValueFromID(dbConn, "EmpPayrollID", obj.EmpPayrollID.ToString());
                        else
                            return string.Empty;
                    }
                    if (tmpfieldName.Equals("CostCenterID", StringComparison.CurrentCultureIgnoreCase))
                    {
                        ECostCenter obj = new ECostCenter();
                        obj.CostCenterID = int.Parse(fieldValue);
                        if (ECostCenter.db.select(dbConn, obj))
                            return obj.CostCenterCode + " - " + obj.CostCenterDesc;
                        else
                            return string.Empty;
                    }
                }
                else if (tmpfieldName.StartsWith("D", StringComparison.CurrentCultureIgnoreCase))
                {
                    if (tmpfieldName.Equals("DocumentTypeID", StringComparison.CurrentCultureIgnoreCase))
                    {
                        EDocumentType obj = new EDocumentType();
                        obj.DocumentTypeID = int.Parse(fieldValue);
                        if (EDocumentType.db.select(dbConn, obj))
                            return obj.DocumentTypeCode + " - " + obj.DocumentTypeDesc;
                        else
                            return string.Empty;
                    }
                }
                else if (tmpfieldName.StartsWith("E", StringComparison.CurrentCultureIgnoreCase))
                {
                    if (tmpfieldName.Equals("EmpAccID", StringComparison.CurrentCultureIgnoreCase))
                    {
                        EEmpBankAccount obj = new EEmpBankAccount();
                        obj.EmpBankAccountID = int.Parse(fieldValue);
                        if (EEmpBankAccount.db.select(dbConn, obj))
                            return obj.EmpBankCode + "-" + obj.EmpBranchCode + "-" + obj.EmpAccountNo;
                        //return obj.EmpBankCode + "-" + obj.EmpBranchCode + "-" + string.Empty.PadRight(obj.EmpAccountNo.Length, 'X');
                        else
                            return string.Empty;
                    }
                    if (tmpfieldName.Equals("EmpCostCenterID", StringComparison.CurrentCultureIgnoreCase))
                    {
                        EEmpCostCenter obj = new EEmpCostCenter();
                        obj.EmpCostCenterID = int.Parse(fieldValue);
                        if (EEmpCostCenter.db.select(dbConn, obj))
                            return GetValueFromID(dbConn, "EmpID", obj.EmpID.ToString());
                        else
                            return string.Empty;
                    }
                    if (tmpfieldName.Equals("EmpExtraFieldID", StringComparison.CurrentCultureIgnoreCase))
                    {
                        EEmpExtraField obj = new EEmpExtraField();
                        obj.EmpExtraFieldID = int.Parse(fieldValue);
                        if (EEmpExtraField.db.select(dbConn, obj))
                            return obj.EmpExtraFieldName;
                        else
                            return string.Empty;
                    }
                    if (tmpfieldName.Equals("EmpID", StringComparison.CurrentCultureIgnoreCase))
                    {
                        EEmpPersonalInfo obj = new EEmpPersonalInfo();
                        obj.EmpID = int.Parse(fieldValue);
                        if (EEmpPersonalInfo.db.select(dbConn, obj))
                            return obj.EmpNo + " - " + obj.EmpEngFullName;
                        else
                            return string.Empty;
                    }
                    if (tmpfieldName.Equals("EmpPayrollID", StringComparison.CurrentCultureIgnoreCase))
                    {
                        EEmpPayroll obj = new EEmpPayroll();
                        obj.EmpPayrollID = int.Parse(fieldValue);
                        if (EEmpPayroll.db.select(dbConn, obj))
                            return GetValueFromID(dbConn, "EmpID", obj.EmpID.ToString()) + " : " + GetValueFromID(dbConn, "PayPeriodID", obj.PayPeriodID.ToString());
                        else
                            return string.Empty;
                    }
                    if (tmpfieldName.Equals("EmpPosID", StringComparison.CurrentCultureIgnoreCase))
                    {
                        EEmpPositionInfo obj = new EEmpPositionInfo();
                        obj.EmpPosID = int.Parse(fieldValue);
                        if (EEmpPositionInfo.db.select(dbConn, obj))
                            return obj.EmpPosEffFr.ToString("yyyy-MM-dd") + " - " + (obj.EmpPosEffTo.Ticks.Equals(0) ? "Present" : obj.EmpPosEffTo.ToString("yyyy-MM-dd"));
                        else
                            return string.Empty;
                    }
                    if (tmpfieldName.Equals("EmploymentTypeID", StringComparison.CurrentCultureIgnoreCase))
                    {
                        EEmploymentType obj = new EEmploymentType();
                        obj.EmploymentTypeID = int.Parse(fieldValue);
                        if (EEmploymentType.db.select(dbConn, obj))
                            return obj.EmploymentTypeCode + " - " + obj.EmploymentTypeDesc;
                        else
                            return string.Empty;
                    }
                }
                else if (tmpfieldName.StartsWith("F", StringComparison.CurrentCultureIgnoreCase))
                {
                    if (tmpfieldName.Equals("FunctionID", StringComparison.CurrentCultureIgnoreCase))
                    {
                        ESystemFunction obj = new ESystemFunction();
                        obj.FunctionID = int.Parse(fieldValue);
                        if (ESystemFunction.db.select(dbConn, obj))
                            return obj.FunctionCode + " - " + obj.Description;
                        else
                            return string.Empty;
                    }
                }
                else if (tmpfieldName.StartsWith("H", StringComparison.CurrentCultureIgnoreCase))
                {
                    if (tmpfieldName.Equals("HElementID", StringComparison.CurrentCultureIgnoreCase))
                    {
                        EHierarchyElement obj = new EHierarchyElement();
                        obj.HElementID = int.Parse(fieldValue);
                        if (EHierarchyElement.db.select(dbConn, obj))
                            return obj.HElementCode + " - " + obj.HElementDesc;
                        else
                            return string.Empty;
                    }
                    if (tmpfieldName.Equals("HLevelID", StringComparison.CurrentCultureIgnoreCase))
                    {
                        EHierarchyLevel obj = new EHierarchyLevel();
                        obj.HLevelID = int.Parse(fieldValue);
                        if (EHierarchyLevel.db.select(dbConn, obj))
                            return obj.HLevelCode + " - " + obj.HLevelDesc;
                        else
                            return string.Empty;
                    }
                }
                else if (tmpfieldName.StartsWith("L", StringComparison.CurrentCultureIgnoreCase))
                {
                    if (tmpfieldName.Equals("LeaveTypeID", StringComparison.CurrentCultureIgnoreCase))
                    {
                        ELeaveType obj = new ELeaveType();
                        obj.LeaveTypeID = int.Parse(fieldValue);
                        if (ELeaveType.db.select(dbConn, obj))
                            return obj.LeaveType + " - " + obj.LeaveTypeDesc;
                        else
                            return string.Empty;
                    }
                    if (tmpfieldName.Equals("LeaveCodeID", StringComparison.CurrentCultureIgnoreCase))
                    {
                        ELeaveCode obj = new ELeaveCode();
                        obj.LeaveCodeID = int.Parse(fieldValue);
                        if (ELeaveCode.db.select(dbConn, obj))
                            return obj.LeaveCode + " - " + obj.LeaveCodeDesc;
                        else
                            return string.Empty;
                    }
                    if (tmpfieldName.Equals("LeavePlanID", StringComparison.CurrentCultureIgnoreCase))
                    {
                        ELeavePlan obj = new ELeavePlan();
                        obj.LeavePlanID = int.Parse(fieldValue);
                        if (ELeavePlan.db.select(dbConn, obj))
                            return obj.LeavePlanCode + " - " + obj.LeavePlanDesc;
                        else
                            return string.Empty;
                    }
                }
                else if (tmpfieldName.StartsWith("M", StringComparison.CurrentCultureIgnoreCase))
                {
                    if (tmpfieldName.Equals("MPFPlanID", StringComparison.CurrentCultureIgnoreCase))
                    {
                        EMPFPlan obj = new EMPFPlan();
                        obj.MPFPlanID = int.Parse(fieldValue);
                        if (EMPFPlan.db.select(dbConn, obj))
                            return obj.MPFPlanCode + " - " + obj.MPFPlanDesc;
                        else
                            return string.Empty;
                    }

                    if (tmpfieldName.Equals("MPFSchemeID", StringComparison.CurrentCultureIgnoreCase))
                    {
                        EMPFScheme obj = new EMPFScheme();
                        obj.MPFSchemeID = int.Parse(fieldValue);
                        if (EMPFScheme.db.select(dbConn, obj))
                            return obj.MPFSchemeCode + " - " + obj.MPFSchemeDesc;
                        else
                            return string.Empty;
                    }
                    if (tmpfieldName.Equals("MPFSchemeCessationReasonID", StringComparison.CurrentCultureIgnoreCase))
                    {
                        EMPFSchemeCessationReason obj = new EMPFSchemeCessationReason();
                        obj.MPFSchemeCessationReasonID = int.Parse(fieldValue);
                        if (EMPFSchemeCessationReason.db.select(dbConn, obj))
                            return obj.MPFSchemeCessationReasonCode + " - " + obj.MPFSchemeCessationReasonDesc;
                        else
                            return string.Empty;
                    }

                }
                else if (tmpfieldName.StartsWith("O", StringComparison.CurrentCultureIgnoreCase))
                {
                    if (tmpfieldName.Equals("ORSOPlanID", StringComparison.CurrentCultureIgnoreCase))
                    {
                        EORSOPlan obj = new EORSOPlan();
                        obj.ORSOPlanID = int.Parse(fieldValue);
                        if (EORSOPlan.db.select(dbConn, obj))
                            return obj.ORSOPlanCode + " - " + obj.ORSOPlanDesc;
                        else
                            return string.Empty;
                    }
                }
                else if (tmpfieldName.StartsWith("P", StringComparison.CurrentCultureIgnoreCase))
                {
                    if (tmpfieldName.Equals("PayGroupID", StringComparison.CurrentCultureIgnoreCase) || tmpfieldName.Equals("PayrollGroupID", StringComparison.CurrentCultureIgnoreCase))
                    {
                        EPayrollGroup obj = new EPayrollGroup();
                        obj.PayGroupID = int.Parse(fieldValue);
                        if (EPayrollGroup.db.select(dbConn, obj))
                            return obj.PayGroupCode + " - " + obj.PayGroupDesc;
                        else
                            return string.Empty;
                    }
                    if (tmpfieldName.Equals("PayFormID", StringComparison.CurrentCultureIgnoreCase))
                    {
                        EPayrollProrataFormula obj = new EPayrollProrataFormula();
                        obj.PayFormID = int.Parse(fieldValue);
                        if (EPayrollProrataFormula.db.select(dbConn, obj))
                            return obj.PayFormCode + " - " + obj.PayFormDesc;
                        else
                            return string.Empty;
                    }
                    if (tmpfieldName.Equals("PaymentCodeID", StringComparison.CurrentCultureIgnoreCase) || tmpfieldName.Equals("PayCodeID", StringComparison.CurrentCultureIgnoreCase))
                    {
                        EPaymentCode obj = new EPaymentCode();
                        obj.PaymentCodeID = int.Parse(fieldValue);
                        if (EPaymentCode.db.select(dbConn, obj))
                            return obj.PaymentCode + " - " + obj.PaymentCodeDesc;
                        else
                            return string.Empty;
                    }
                    if (tmpfieldName.Equals("PaymentTypeID", StringComparison.CurrentCultureIgnoreCase))
                    {
                        EPaymentType obj = new EPaymentType();
                        obj.PaymentTypeID = int.Parse(fieldValue);
                        if (EPaymentType.db.select(dbConn, obj))
                            return obj.PaymentTypeCode + " - " + obj.PaymentTypeDesc;
                        else
                            return string.Empty;
                    }
                    if (tmpfieldName.Equals("PayPeriodID", StringComparison.CurrentCultureIgnoreCase))
                    {
                        EPayrollPeriod obj = new EPayrollPeriod();
                        obj.PayPeriodID = int.Parse(fieldValue);
                        if (EPayrollPeriod.db.select(dbConn, obj))
                            return GetValueFromID(dbConn, "PayrollGroupID", obj.PayGroupID.ToString()) + ": " + obj.PayPeriodFr.ToString("yyyy-MM-dd") + " to " + obj.PayPeriodTo.ToString("yyyy-MM-dd");
                        else
                            return string.Empty;
                    }
                    if (tmpfieldName.Equals("PermitTypeID", StringComparison.CurrentCultureIgnoreCase))
                    {
                        EPermitType obj = new EPermitType();
                        obj.PermitTypeID = int.Parse(fieldValue);
                        if (EPermitType.db.select(dbConn, obj))
                            return obj.PermitTypeCode + " - " + obj.PermitTypeDesc;
                        else
                            return string.Empty;
                    }
                    if (tmpfieldName.Equals("PositionID", StringComparison.CurrentCultureIgnoreCase))
                    {
                        EPosition obj = new EPosition();
                        obj.PositionID = int.Parse(fieldValue);
                        if (EPosition.db.select(dbConn, obj))
                            return obj.PositionCode + " - " + obj.PositionDesc;
                        else
                            return string.Empty;
                    }
                }
                else if (tmpfieldName.StartsWith("Q", StringComparison.CurrentCultureIgnoreCase))
                {
                    if (tmpfieldName.Equals("QualificationID", StringComparison.CurrentCultureIgnoreCase))
                    {
                        EQualification obj = new EQualification();
                        obj.QualificationID = int.Parse(fieldValue);
                        if (EQualification.db.select(dbConn, obj))
                            return obj.QualificationCode + " - " + obj.QualificationDesc;
                        else
                            return string.Empty;
                    }
                }
                else if (tmpfieldName.StartsWith("R", StringComparison.CurrentCultureIgnoreCase))
                {
                    if (tmpfieldName.Equals("RankID", StringComparison.CurrentCultureIgnoreCase))
                    {
                        ERank obj = new ERank();
                        obj.RankID = int.Parse(fieldValue);
                        if (ERank.db.select(dbConn, obj))
                            return obj.RankCode + " - " + obj.RankDesc;
                        else
                            return string.Empty;
                    }
                    if (tmpfieldName.Equals("ReminderTypeID", StringComparison.CurrentCultureIgnoreCase))
                    {
                        EReminderType obj = new EReminderType();
                        obj.ReminderTypeID = int.Parse(fieldValue);
                        if (EReminderType.db.select(dbConn, obj))
                            return obj.ReminderTypeCode + " - " + obj.ReminderTypeDesc;
                        else
                            return string.Empty;
                    }
                    if (tmpfieldName.Equals("RosterClientID", StringComparison.CurrentCultureIgnoreCase))
                    {
                        ERosterClient obj = new ERosterClient();
                        obj.RosterClientID = int.Parse(fieldValue);
                        if (ERosterClient.db.select(dbConn, obj))
                            return obj.RosterClientCode + " - " + obj.RosterClientName;
                        else
                            return string.Empty;
                    }
                    if (tmpfieldName.Equals("RosterClientSiteID", StringComparison.CurrentCultureIgnoreCase))
                    {
                        ERosterClientSite obj = new ERosterClientSite();
                        obj.RosterClientSiteID = int.Parse(fieldValue);
                        if (ERosterClientSite.db.select(dbConn, obj))
                            return obj.RosterClientSiteCode;
                        else
                            return string.Empty;
                    }
                    if (tmpfieldName.Equals("RosterCodeID", StringComparison.CurrentCultureIgnoreCase))
                    {
                        ERosterCode obj = new ERosterCode();
                        obj.RosterCodeID = int.Parse(fieldValue);
                        if (ERosterCode.db.select(dbConn, obj))
                            return obj.RosterCode + " - " + obj.RosterCodeDesc;
                        else
                            return string.Empty;
                    }
                }
                else if (tmpfieldName.StartsWith("S", StringComparison.CurrentCultureIgnoreCase))
                {
                    if (tmpfieldName.Equals("SkillID", StringComparison.CurrentCultureIgnoreCase))
                    {
                        ESkill obj = new ESkill();
                        obj.SkillID = int.Parse(fieldValue);
                        if (ESkill.db.select(dbConn, obj))
                            return obj.SkillCode + " - " + obj.SkillDesc;
                        else
                            return string.Empty;
                    }
                    if (tmpfieldName.Equals("SkillLevelID", StringComparison.CurrentCultureIgnoreCase))
                    {
                        ESkillLevel obj = new ESkillLevel();
                        obj.SkillLevelID = int.Parse(fieldValue);
                        if (ESkillLevel.db.select(dbConn, obj))
                            return obj.SkillLevelCode + " - " + obj.SkillLevelDesc;
                        else
                            return string.Empty;
                    }
                    if (tmpfieldName.Equals("StaffTypeID", StringComparison.CurrentCultureIgnoreCase))
                    {
                        EStaffType obj = new EStaffType();
                        obj.StaffTypeID = int.Parse(fieldValue);
                        if (EStaffType.db.select(dbConn, obj))
                            return obj.StaffTypeCode + " - " + obj.StaffTypeDesc;
                        else
                            return string.Empty;
                    }

                }
                else if (tmpfieldName.StartsWith("T", StringComparison.CurrentCultureIgnoreCase))
                {
                    if (tmpfieldName.Equals("TaxCompID", StringComparison.CurrentCultureIgnoreCase))
                    {
                        ETaxCompany obj = new ETaxCompany();
                        obj.TaxCompID = int.Parse(fieldValue);
                        if (ETaxCompany.db.select(dbConn, obj))
                            return obj.TaxCompEmployerName;
                        else
                            return string.Empty;
                    }
                    if (tmpfieldName.Equals("TaxEmpID", StringComparison.CurrentCultureIgnoreCase))
                    {
                        ETaxEmp obj = new ETaxEmp();
                        obj.TaxEmpID = int.Parse(fieldValue);
                        if (ETaxEmp.db.select(dbConn, obj))
                            return obj.TaxEmpSurname + ", " + obj.TaxEmpOtherName + ", " + GetValueFromID(dbConn, "TaxFormID", obj.TaxFormID.ToString());
                        else
                            return string.Empty;
                    }
                    if (tmpfieldName.Equals("TaxFormID", StringComparison.CurrentCultureIgnoreCase))
                    {
                        ETaxForm obj = new ETaxForm();
                        obj.TaxFormID = int.Parse(fieldValue);
                        if (ETaxForm.db.select(dbConn, obj))
                            return "Tax Year :" + obj.TaxFormYear + ", Form: IR56" + obj.TaxFormType;
                        else
                            return string.Empty;
                    }
                    if (tmpfieldName.Equals("TaxPayID", StringComparison.CurrentCultureIgnoreCase))
                    {
                        ETaxPayment obj = new ETaxPayment();
                        obj.TaxPayID = int.Parse(fieldValue);
                        if (ETaxPayment.db.select(dbConn, obj))
                            return obj.TaxPayCode + " - " + obj.TaxPayDesc;
                        else
                            return string.Empty;
                    }
                    if (tmpfieldName.Equals("TrainingCourseID", StringComparison.CurrentCultureIgnoreCase))
                    {
                        ETrainingCourse obj = new ETrainingCourse();
                        obj.TrainingCourseID = int.Parse(fieldValue);
                        if (ETrainingCourse.db.select(dbConn, obj))
                            return obj.TrainingCourseCode + " - " + obj.TrainingCourseName;
                        else
                            return string.Empty;
                    }
                    if (tmpfieldName.Equals("TrainingSeminarID", StringComparison.CurrentCultureIgnoreCase))
                    {
                        ETrainingSeminar obj = new ETrainingSeminar();
                        obj.TrainingSeminarID = int.Parse(fieldValue);
                        if (ETrainingSeminar.db.select(dbConn, obj))
                            return GetValueFromID(dbConn, "TrainingCourseID", obj.TrainingCourseID.ToString()) + ": " + obj.TrainingSeminarDateFrom.ToString("yyyy-MM-dd") + " to " + obj.TrainingSeminarDateTo.ToString("yyyy-MM-dd");
                        else
                            return string.Empty;
                    }
                }
                else if (tmpfieldName.StartsWith("U", StringComparison.CurrentCultureIgnoreCase))
                {
                    if (tmpfieldName.Equals("UserID", StringComparison.CurrentCultureIgnoreCase))
                    {
                        EUser obj = new EUser();
                        obj.UserID = int.Parse(fieldValue);
                        if (EUser.db.select(dbConn, obj))
                            return obj.LoginID + " - " + obj.UserName;
                        else
                            return string.Empty;
                    }
                    if (tmpfieldName.Equals("UserGroupID", StringComparison.CurrentCultureIgnoreCase))
                    {
                        EUserGroup obj = new EUserGroup();
                        obj.UserGroupID = int.Parse(fieldValue);
                        if (EUserGroup.db.select(dbConn, obj))
                            return obj.UserGroupName + " - " + obj.UserGroupDesc;
                        else
                            return string.Empty;
                    }

                }
                else if (tmpfieldName.StartsWith("Y", StringComparison.CurrentCultureIgnoreCase))
                {
                    if (tmpfieldName.Equals("YebPlanID", StringComparison.CurrentCultureIgnoreCase))
                    {
                        EYEBPlan obj = new EYEBPlan();
                        obj.YEBPlanID = int.Parse(fieldValue);
                        if (EYEBPlan.db.select(dbConn, obj))
                            return obj.YEBPlanCode + " - " + obj.YEBPlanDesc;
                        else
                            return string.Empty;
                    }
                }

                if (tmpfieldName.EndsWith("ID") && !tmpfieldName.EndsWith("HKID") && !tmpfieldName.EndsWith("LoginID") && !tmpfieldName.EndsWith("CurrencyID") && !tmpfieldName.EndsWith("LeaveAppID") && !tmpfieldName.EndsWith("EmpPaymentID") && !tmpfieldName.EndsWith("PayBatchID") && !tmpfieldName.EndsWith("PayRecID") && !tmpfieldName.EndsWith("RosterTableID") && !tmpfieldName.EndsWith("SynID") && !tmpfieldName.EndsWith("CNDImportBatchID"))
                {
                    if (ESystemParameter.getParameter(dbConn, "DebugMode").Equals("Y"))
                    {
                        throw new Exception("ID field not define:" + fieldName);
                    }
                }
                return string.Empty;
            }
            catch (Exception ex)
            {
                if (ESystemParameter.getParameter(dbConn, "DebugMode").Equals("Y"))
                    throw ex;
                else
                    return string.Empty;
            }
        }

    }
}
