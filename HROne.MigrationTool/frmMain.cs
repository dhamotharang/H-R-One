using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using HROne.CommonLib;
using HROne.Lib;
using HROne.Lib.Entities;
using HROne.DataAccess;
using HROne.ProductVersion;
using HROne.MigrationTool;
using HROne.Import;

namespace HROne.MigrationTool
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
        }

        private void cmdApplyDBScript_Click(object sender, EventArgs e)
        {
            string connectionString = "Data Source=" + txtServer.Text + ";User ID=" + txtUserID.Text + ";Password=" + txtPassword.Text + ";Initial Catalog=" + txtDatabase.Text;

            DatabaseConnection dbConn = new DatabaseConnection(connectionString, DatabaseConnection.DatabaseType.MSSQL);
            txtResult.Text = txtResult.Text + "DB Connection established" + "\r\n";
            Application.DoEvents();

            PatchEngine patchEngine = new PatchEngine(dbConn, "./", txtTargetDBVersion.Text);

            txtResult.Text = txtResult.Text + "Applying database upgrade scripts" + "\r\n";
            Application.DoEvents();

            patchEngine.UpdateDatabaseVersion(false); 

            txtResult.Text = txtResult.Text + "Database upgraded.  Current DB Version is : " + patchEngine.RunningDatabaseVersion() + "\r\n";
            Application.DoEvents();
            ESystemParameter.setParameter(dbConn, "DBVersion", "1.0.0");
            txtResult.Text = txtResult.Text + "Database upgraded.  Database version is reset to : " + patchEngine.RunningDatabaseVersion() + "\r\n";
            

        }

        private void cmdDecrypt_Click(object sender, EventArgs e)
        {
            string connectionString = "Data Source=" + txtServer.Text + ";User ID=" + txtUserID.Text + ";Password=" + txtPassword.Text + ";Initial Catalog=" + txtDatabase.Text;

            DatabaseConnection dbConn = new DatabaseConnection(connectionString, DatabaseConnection.DatabaseType.MSSQL);
            txtResult.Text = txtResult.Text  + "DB Connection established" + "\r\n";
            Application.DoEvents();

            DBAESEncryptStringFieldAttribute.skipEncryptionToDB = true;
            DBAESEncryptStringFieldAttribute.DEFAULT_KEY_STRING = txtCurrentKey.Text ;

            txtResult.Text = txtResult.Text + "Decrypting data, please wait....\r\n";
            Cursor.Current = Cursors.WaitCursor;
            Application.DoEvents();

            update_data(dbConn);

            txtResult.Text = txtResult.Text + "Conversion completed. Please check!\r\n";
            Cursor.Current = Cursors.Default;
            Application.DoEvents();
        }

        private void cmdEncrypt_Click(object sender, EventArgs e)
        {
            string connectionString = "Data Source=" + txtServer.Text + ";User ID=" + txtUserID.Text + ";Password=" + txtPassword.Text + ";Initial Catalog=" + txtDatabase.Text;

            DatabaseConnection dbConn = new DatabaseConnection(connectionString, DatabaseConnection.DatabaseType.MSSQL);
            txtResult.Text = txtResult.Text + "DB Connection establishe\r\n";

            DBAESEncryptStringFieldAttribute.skipEncryptionToDB = false;
            DBAESEncryptStringFieldAttribute.DEFAULT_KEY_STRING = txtNewKey.Text;

            txtResult.Text = txtResult.Text + "Encrypting data, please wait....\r\n";
            Cursor.Current = Cursors.WaitCursor;
            Application.DoEvents();

            update_data(dbConn);

            txtResult.Text = txtResult.Text + "Conversion completed. Please check!\r\n";
            Cursor.Current = Cursors.Default;
            Application.DoEvents();

        }

        private void update_data2(DatabaseConnection dbConn)
        {
#region code_update_data
            ArrayList mList = EAuditTrailDetail.db.select(dbConn, new DBFilter());
            foreach (EAuditTrailDetail o1 in mList)
                EAuditTrailDetail.db.update(dbConn, o1);

            mList = EAuthorizationWorkFlowDetail.db.select(dbConn, new DBFilter());
            foreach (EAuthorizationWorkFlowDetail o1 in mList)
                EAuthorizationWorkFlowDetail.db.update(dbConn, o1);

            mList = EAVCPlanPaymentCeiling.db.select(dbConn, new DBFilter());
            foreach (EAVCPlanPaymentCeiling o1 in mList)
                EAVCPlanPaymentCeiling.db.update(dbConn, o1);


            mList = EAVCPlanPaymentConsider.db.select(dbConn, new DBFilter());
            foreach (EAVCPlanPaymentConsider o1 in mList)
                EAVCPlanPaymentConsider.db.update(dbConn, o1);


            mList = EBankList.db.select(dbConn, new DBFilter());
            foreach (EBankList o1 in mList)
                EBankList.db.update(dbConn, o1);


            mList = ECessationReason.db.select(dbConn, new DBFilter());
            foreach (ECessationReason o1 in mList)
                ECessationReason.db.update(dbConn, o1);



            mList = ECostAllocationDetail.db.select(dbConn, new DBFilter());
            foreach (ECostAllocationDetail o1 in mList)
                ECostAllocationDetail.db.update(dbConn, o1);


            mList = ECostAllocationDetailHElement.db.select(dbConn, new DBFilter());
            foreach (ECostAllocationDetailHElement o1 in mList)
                ECostAllocationDetailHElement.db.update(dbConn, o1);


            mList = EDocumentType.db.select(dbConn, new DBFilter());
            foreach (EDocumentType o1 in mList)
                EDocumentType.db.update(dbConn, o1);


            mList = EEmploymentType.db.select(dbConn, new DBFilter());
            foreach (EEmploymentType o1 in mList)
                EEmploymentType.db.update(dbConn, o1);


            //mList = EFinalPayment.db.select(dbConn, new DBFilter());
            //foreach (EFinalPayment o1 in mList)
            //    EFinalPayment.db.update(dbConn, o1);


            mList = EInboxAttachment.db.select(dbConn, new DBFilter());
            foreach (EInboxAttachment o1 in mList)
                EInboxAttachment.db.update(dbConn, o1);


            //mList = EMPFSchemeTrustee.db.select(dbConn, new DBFilter());
            //foreach (EMPFSchemeTrustee o1 in mList)
            //    EMPFSchemeTrustee.db.update(dbConn, o1);


            mList = EPermitType.db.select(dbConn, new DBFilter());
            foreach (EPermitType o1 in mList)
                EPermitType.db.update(dbConn, o1);


            mList = EPosition.db.select(dbConn, new DBFilter());
            foreach (EPosition o1 in mList)
                EPosition.db.update(dbConn, o1);


            mList = EPublicHoliday.db.select(dbConn, new DBFilter());
            foreach (EPublicHoliday o1 in mList)
                EPublicHoliday.db.update(dbConn, o1);


            mList = EQualification.db.select(dbConn, new DBFilter());
            foreach (EQualification o1 in mList)
                EQualification.db.update(dbConn, o1);


            mList = ERank.db.select(dbConn, new DBFilter());
            foreach (ERank o1 in mList)
                ERank.db.update(dbConn, o1);


            //mList = ERequestStatus.db.select(dbConn, new DBFilter());
            //foreach (ERequestStatus o1 in mList)
            //    ERequestStatus.db.update(dbConn, o1);


            mList = ERosterClientSite.db.select(dbConn, new DBFilter());
            foreach (ERosterClientSite o1 in mList)
                ERosterClientSite.db.update(dbConn, o1);


            mList = ESkill.db.select(dbConn, new DBFilter());
            foreach (ESkill o1 in mList)
                ESkill.db.update(dbConn, o1);


            mList = ESkillLevel.db.select(dbConn, new DBFilter());
            foreach (ESkillLevel o1 in mList)
                ESkillLevel.db.update(dbConn, o1);


            mList = EStaffType.db.select(dbConn, new DBFilter());
            foreach (EStaffType o1 in mList)
                EStaffType.db.update(dbConn, o1);


            mList = EStatutoryHoliday.db.select(dbConn, new DBFilter());
            foreach (EStatutoryHoliday o1 in mList)
                EStatutoryHoliday.db.update(dbConn, o1);

// Start 0000128, KuangWei, 2014-11-12
            mList = EUploadAttendanceRecord.db.select(dbConn, new DBFilter());
            foreach (EUploadAttendanceRecord o1 in mList)
                EUploadAttendanceRecord.db.update(dbConn, o1);


            mList = EUploadClaimsAndDeductions.db.select(dbConn, new DBFilter());
            foreach (EUploadClaimsAndDeductions o1 in mList)
                EUploadClaimsAndDeductions.db.update(dbConn, o1);
  // End 0000128, KuangWei, 2014-11-12                  
//EUploadCompensationLeaveEntitle
//mList = xxxxxxx.db.select(dbConn, new DBFilter());
//foreach (xxxxxxx o1 in mList)
//    xxxxxxx.db.update(dbConn, o1);
                    
//EUploadEmpAVCPlan
//mList = xxxxxxx.db.select(dbConn, new DBFilter());
//foreach (xxxxxxx o1 in mList)
//    xxxxxxx.db.update(dbConn, o1);

 // Start 0000128, KuangWei, 2014-11-12
            mList = EUploadEmpBankAccount.db.select(dbConn, new DBFilter());
            foreach (EUploadEmpBankAccount o1 in mList)
                EUploadEmpBankAccount.db.update(dbConn, o1);
 // End 0000128, KuangWei, 2014-11-12                   
//EUploadEmpContractTerms
//mList = xxxxxxx.db.select(dbConn, new DBFilter());
//foreach (xxxxxxx o1 in mList)
//    xxxxxxx.db.update(dbConn, o1);
                    
//EUploadEmpCostCenter
//mList = xxxxxxx.db.select(dbConn, new DBFilter());
//foreach (xxxxxxx o1 in mList)
//    xxxxxxx.db.update(dbConn, o1);
                    
//EUploadEmpCostCenterDetail
//mList = xxxxxxx.db.select(dbConn, new DBFilter());
//foreach (xxxxxxx o1 in mList)
//    xxxxxxx.db.update(dbConn, o1);
                    
//EUploadEmpDependant
//mList = xxxxxxx.db.select(dbConn, new DBFilter());
//foreach (xxxxxxx o1 in mList)
//    xxxxxxx.db.update(dbConn, o1);
                    
//EUploadEmpEmergencyContact
//mList = xxxxxxx.db.select(dbConn, new DBFilter());
//foreach (xxxxxxx o1 in mList)
//    xxxxxxx.db.update(dbConn, o1);

// Start 0000128, KuangWei, 2014-11-12
            mList = EUploadEmpExtraFieldValue.db.select(dbConn, new DBFilter());
            foreach (EUploadEmpExtraFieldValue o1 in mList)
                EUploadEmpExtraFieldValue.db.update(dbConn, o1);
// End 0000128, KuangWei, 2014-11-12
                    
//EUploadEmpFinalPayment
//mList = xxxxxxx.db.select(dbConn, new DBFilter());
//foreach (xxxxxxx o1 in mList)
//    xxxxxxx.db.update(dbConn, o1);
                    
//EUploadEmpHierarchy
//mList = xxxxxxx.db.select(dbConn, new DBFilter());
//foreach (xxxxxxx o1 in mList)
//    xxxxxxx.db.update(dbConn, o1);
                    
//EUploadEmpMPFPlan
//mList = xxxxxxx.db.select(dbConn, new DBFilter());
//foreach (xxxxxxx o1 in mList)
//    xxxxxxx.db.update(dbConn, o1);
                    
//EUploadEmpORSOPlan
//mList = xxxxxxx.db.select(dbConn, new DBFilter());
//foreach (xxxxxxx o1 in mList)
//    xxxxxxx.db.update(dbConn, o1);
                    
//EUploadEmpPermit
//mList = xxxxxxx.db.select(dbConn, new DBFilter());
//foreach (xxxxxxx o1 in mList)
//    xxxxxxx.db.update(dbConn, o1);

// Start 0000128, KuangWei, 2014-11-12
            mList = EUploadEmpPersonalInfo.db.select(dbConn, new DBFilter());
            foreach (EUploadEmpPersonalInfo o1 in mList)
                EUploadEmpPersonalInfo.db.update(dbConn, o1);
// End 0000128, KuangWei, 2014-11-12
                    
//EUploadEmpPlaceOfResidence
//mList = xxxxxxx.db.select(dbConn, new DBFilter());
//foreach (xxxxxxx o1 in mList)
//    xxxxxxx.db.update(dbConn, o1);
                    
//EUploadEmpPositionInfo
//mList = xxxxxxx.db.select(dbConn, new DBFilter());
//foreach (xxxxxxx o1 in mList)
//    xxxxxxx.db.update(dbConn, o1);
                    
//EUploadEmpQualification
//mList = xxxxxxx.db.select(dbConn, new DBFilter());
//foreach (xxxxxxx o1 in mList)
//    xxxxxxx.db.update(dbConn, o1);
                    
//EUploadEmpRecurringPayment
//mList = xxxxxxx.db.select(dbConn, new DBFilter());
//foreach (xxxxxxx o1 in mList)
//    xxxxxxx.db.update(dbConn, o1);
                    
//EUploadEmpRosterTableGroup
//mList = xxxxxxx.db.select(dbConn, new DBFilter());
//foreach (xxxxxxx o1 in mList)
//    xxxxxxx.db.update(dbConn, o1);
                    
//EUploadEmpSkill
//mList = xxxxxxx.db.select(dbConn, new DBFilter());
//foreach (xxxxxxx o1 in mList)
//    xxxxxxx.db.update(dbConn, o1);
                    
//EUploadEmpSpouse
//mList = xxxxxxx.db.select(dbConn, new DBFilter());
//foreach (xxxxxxx o1 in mList)
//    xxxxxxx.db.update(dbConn, o1);
                    
//EUploadEmpTermination
//mList = xxxxxxx.db.select(dbConn, new DBFilter());
//foreach (xxxxxxx o1 in mList)
//    xxxxxxx.db.update(dbConn, o1);
                    
//EUploadEmpUniform
//mList = xxxxxxx.db.select(dbConn, new DBFilter());
//foreach (xxxxxxx o1 in mList)
//    xxxxxxx.db.update(dbConn, o1);
                    
//EUploadEmpWorkExp
//mList = xxxxxxx.db.select(dbConn, new DBFilter());
//foreach (xxxxxxx o1 in mList)
//    xxxxxxx.db.update(dbConn, o1);
                    
//EUploadEmpWorkingSummary
//mList = xxxxxxx.db.select(dbConn, new DBFilter());
//foreach (xxxxxxx o1 in mList)
//    xxxxxxx.db.update(dbConn, o1);
                    
//EUploadEmpWorkInjuryRecord
//mList = xxxxxxx.db.select(dbConn, new DBFilter());
//foreach (xxxxxxx o1 in mList)
//    xxxxxxx.db.update(dbConn, o1);
                    
//EUploadLeaveApplication
//mList = xxxxxxx.db.select(dbConn, new DBFilter());
//foreach (xxxxxxx o1 in mList)
//    xxxxxxx.db.update(dbConn, o1);
                    
//EUploadLeaveBalanceAdjustment
//mList = xxxxxxx.db.select(dbConn, new DBFilter());
//foreach (xxxxxxx o1 in mList)
//    xxxxxxx.db.update(dbConn, o1);
                    
//EUploadRosterTable
//mList = xxxxxxx.db.select(dbConn, new DBFilter());
//foreach (xxxxxxx o1 in mList)
//    xxxxxxx.db.update(dbConn, o1);


//mList = EUploadTimeCardRecord.db.select(dbConn, new DBFilter());
//foreach (EUploadTimeCardRecord o1 in mList)
//    EUploadTimeCardRecord.db.update(dbConn, o1);


//EGAPBonusEmploymentTypeEligiblePeriod
//mList = xxxxxxx.db.select(dbConn, new DBFilter());
//foreach (xxxxxxx o1 in mList)
//    xxxxxxx.db.update(dbConn, o1);

//EGAPBonusPaymentMapping
//mList = xxxxxxx.db.select(dbConn, new DBFilter());
//foreach (xxxxxxx o1 in mList)
//    xxxxxxx.db.update(dbConn, o1);

//EGAPBonusPercentageMapping
//mList = xxxxxxx.db.select(dbConn, new DBFilter());
//foreach (xxxxxxx o1 in mList)
//    xxxxxxx.db.update(dbConn, o1);

//EPS_BackpayBatchDetail
//mList = xxxxxxx.db.select(dbConn, new DBFilter());
//foreach (xxxxxxx o1 in mList)
//    xxxxxxx.db.update(dbConn, o1);

//EPS_SalaryIncrementBatch
//mList = xxxxxxx.db.select(dbConn, new DBFilter());
//foreach (xxxxxxx o1 in mList)
//    xxxxxxx.db.update(dbConn, o1);

//EPS_SalaryIncrementBatchDetail
//mList = xxxxxxx.db.select(dbConn, new DBFilter());
//foreach (xxxxxxx o1 in mList)
//    xxxxxxx.db.update(dbConn, o1);

//EUploadEmpBeneficiaries
//mList = xxxxxxx.db.select(dbConn, new DBFilter());
//foreach (xxxxxxx o1 in mList)
//    xxxxxxx.db.update(dbConn, o1);

//EUploadEmpBenefit
//mList = xxxxxxx.db.select(dbConn, new DBFilter());
//foreach (xxxxxxx o1 in mList)
//    xxxxxxx.db.update(dbConn, o1);

			// Start 0000128, KuangWei, 2014-11-12
            mList = EBenefitPlan.db.select(dbConn, new DBFilter());
            foreach (EBenefitPlan o1 in mList)
                EBenefitPlan.db.update(dbConn, o1);

            mList = ECommissionAchievement.db.select(dbConn, new DBFilter());
            foreach (ECommissionAchievement o1 in mList)
                ECommissionAchievement.db.update(dbConn, o1);

            mList = ECommissionAchievementImportBatch.db.select(dbConn, new DBFilter());
            foreach (ECommissionAchievementImportBatch o1 in mList)
                ECommissionAchievementImportBatch.db.update(dbConn, o1);

            mList = EEmpBeneficiaries.db.select(dbConn, new DBFilter());
            foreach (EEmpBeneficiaries o1 in mList)
                EEmpBeneficiaries.db.update(dbConn, o1);

            mList = EEmpBenefit.db.select(dbConn, new DBFilter());
            foreach (EEmpBenefit o1 in mList)
                EEmpBenefit.db.update(dbConn, o1);

            mList = EIncentivePayment.db.select(dbConn, new DBFilter());
            foreach (EIncentivePayment o1 in mList)
                EIncentivePayment.db.update(dbConn, o1);

            mList = EIncentivePaymentImportBatch.db.select(dbConn, new DBFilter());
            foreach (EIncentivePaymentImportBatch o1 in mList)
                EIncentivePaymentImportBatch.db.update(dbConn, o1);

            mList = EIssueCountry.db.select(dbConn, new DBFilter());
            foreach (EIssueCountry o1 in mList)
                EIssueCountry.db.update(dbConn, o1);

            mList = ENationality.db.select(dbConn, new DBFilter());
            foreach (ENationality o1 in mList)
                ENationality.db.update(dbConn, o1);

            mList = EOTClaim.db.select(dbConn, new DBFilter());
            foreach (EOTClaim o1 in mList)
                EOTClaim.db.update(dbConn, o1);

            mList = EPayrollGroupUsers.db.select(dbConn, new DBFilter());
            foreach (EPayrollGroupUsers o1 in mList)
                EPayrollGroupUsers.db.update(dbConn, o1);

            mList = EPayScale.db.select(dbConn, new DBFilter());
            foreach (EPayScale o1 in mList)
                EPayScale.db.update(dbConn, o1);

            mList = EPayScaleMap.db.select(dbConn, new DBFilter());
            foreach (EPayScaleMap o1 in mList)
                EPayScaleMap.db.update(dbConn, o1);

            mList = EPlaceOfBirth.db.select(dbConn, new DBFilter());
            foreach (EPlaceOfBirth o1 in mList)
                EPlaceOfBirth.db.update(dbConn, o1);

            mList = ERequestOTClaim.db.select(dbConn, new DBFilter());
            foreach (ERequestOTClaim o1 in mList)
                ERequestOTClaim.db.update(dbConn, o1);

            mList = ERequestOTClaimCancel.db.select(dbConn, new DBFilter());
            foreach (ERequestOTClaimCancel o1 in mList)
                ERequestOTClaimCancel.db.update(dbConn, o1);

            mList = EUploadCommissionAchievement.db.select(dbConn, new DBFilter());
            foreach (EUploadCommissionAchievement o1 in mList)
                EUploadCommissionAchievement.db.update(dbConn, o1);

            mList = EUploadIncentivePayment.db.select(dbConn, new DBFilter());
            foreach (EUploadIncentivePayment o1 in mList)
                EUploadIncentivePayment.db.update(dbConn, o1);
			// End 0000128, KuangWei, 2014-11-12
            #endregion code_update_data2
        }

        private void update_data(DatabaseConnection dbConn)
        {
            #region code_update_data

            ArrayList mList = EALProrataRoundingRule.db.select(dbConn, new DBFilter());
            foreach (EALProrataRoundingRule o1 in mList)
                EALProrataRoundingRule.db.update(dbConn, o1);

            mList = EALProrataRoundingRuleDetail.db.select(dbConn, new DBFilter());
            foreach (EALProrataRoundingRuleDetail o1 in mList)
                EALProrataRoundingRuleDetail.db.update(dbConn, o1);

            mList = EAttendanceFormula.db.select(dbConn, new DBFilter());
            foreach (EAttendanceFormula o1 in mList)
                EAttendanceFormula.db.update(dbConn, o1);

            mList = EAttendancePlan.db.select(dbConn, new DBFilter());  
            foreach (EAttendancePlan o1 in mList)  
                EAttendancePlan.db.update(dbConn, o1);
            
            mList = EAttendancePlanAdditionalPayment.db.select(dbConn, new DBFilter());
            foreach (EAttendancePlanAdditionalPayment o1 in mList)
                EAttendancePlanAdditionalPayment.db.update(dbConn, o1);
            
            mList = EAttendanceRecord.db.select(dbConn, new DBFilter());
            foreach (EAttendanceRecord o1 in mList)
                EAttendanceRecord.db.update(dbConn, o1);

            mList = EAuthorizationWorkFlow.db.select(dbConn, new DBFilter());
            foreach (EAuthorizationWorkFlow o1 in mList)
                EAuthorizationWorkFlow.db.update(dbConn, o1);
            
            mList = EAuditTrail.db.select(dbConn, new DBFilter());
            foreach (EAuditTrail o1 in mList)
                EAuditTrail.db.update(dbConn, o1);

            mList = EAuthorizationGroup.db.select(dbConn, new DBFilter());
            foreach (EAuthorizationGroup o1 in mList)
                EAuthorizationGroup.db.update(dbConn, o1);
            
            mList = EAuthorizer.db.select(dbConn, new DBFilter());
            foreach (EAuthorizer o1 in mList)
                EAuthorizer.db.update(dbConn, o1);
            
            mList = EAuthorizerDelegate.db.select(dbConn, new DBFilter());
            foreach (EAuthorizerDelegate o1 in mList)
                EAuthorizerDelegate.db.update(dbConn, o1);
            
            mList = EAVCPlan.db.select(dbConn, new DBFilter());
            foreach (EAVCPlan o1 in mList)
                EAVCPlan.db.update(dbConn, o1);
            
            mList = EAVCPlanDetail.db.select(dbConn, new DBFilter());
            foreach (EAVCPlanDetail o1 in mList)
                EAVCPlanDetail.db.update(dbConn, o1);

            mList = EClaimsAndDeductions.db.select(dbConn, new DBFilter());
            foreach (EClaimsAndDeductions o1 in mList)
                EClaimsAndDeductions.db.update(dbConn, o1);

            mList = EClaimsAndDeductionsImportBatch.db.select(dbConn, new DBFilter());
            foreach (EClaimsAndDeductionsImportBatch o1 in mList)
                EClaimsAndDeductionsImportBatch.db.update(dbConn, o1);

            mList = ECompany.db.select(dbConn, new DBFilter());
            foreach (ECompany o1 in mList)
                ECompany.db.update(dbConn, o1);
            
            mList = ECompanyBankAccount.db.select(dbConn, new DBFilter());
            foreach (ECompanyBankAccount o1 in mList)
                ECompanyBankAccount.db.update(dbConn, o1);
            
            mList = ECompanyBankAccountMap.db.select(dbConn, new DBFilter());
            foreach (ECompanyBankAccountMap o1 in mList)
                ECompanyBankAccountMap.db.update(dbConn, o1);
            
            mList = ECompensationLeaveEntitle.db.select(dbConn, new DBFilter());
            foreach (ECompensationLeaveEntitle o1 in mList)
                ECompensationLeaveEntitle.db.update(dbConn, o1);
            
            mList = ECostAllocation.db.select(dbConn, new DBFilter());
            foreach (ECostAllocation o1 in mList)
                ECostAllocation.db.update(dbConn, o1);

            mList = ECostCenter.db.select(dbConn, new DBFilter());
            foreach (ECostCenter o1 in mList)
                ECostCenter.db.update(dbConn, o1);
            
            mList = EeChannelAuthorizedSignature.db.select(dbConn, new DBFilter());
            foreach (EeChannelAuthorizedSignature o1 in mList)
                EeChannelAuthorizedSignature.db.update(dbConn, o1);
            
            mList = EEmailLog.db.select(dbConn, new DBFilter());
            foreach (EEmailLog o1 in mList)
                EEmailLog.db.update(dbConn, o1);
            
            mList = EEmpAVCPlan.db.select(dbConn, new DBFilter());
            foreach (EEmpAVCPlan o1 in mList)
                EEmpAVCPlan.db.update(dbConn, o1);
            
            mList = EEmpBankAccount.db.select(dbConn, new DBFilter());
            foreach (EEmpBankAccount o1 in mList)
                EEmpBankAccount.db.update(dbConn, o1);
            
            mList = EEmpContractTerms.db.select(dbConn, new DBFilter());
            foreach (EEmpContractTerms o1 in mList)
                EEmpContractTerms.db.update(dbConn, o1);
            
            mList = EEmpCostCenter.db.select(dbConn, new DBFilter());
            foreach (EEmpCostCenter o1 in mList)
                EEmpCostCenter.db.update(dbConn, o1);
            
            mList = EEmpCostCenterDetail.db.select(dbConn, new DBFilter());
            foreach (EEmpCostCenterDetail o1 in mList)
                EEmpCostCenterDetail.db.update(dbConn, o1);
            
            mList = EEmpDependant.db.select(dbConn, new DBFilter());
            foreach (EEmpDependant o1 in mList)
                EEmpDependant.db.update(dbConn, o1);
            
            mList = EEmpDocument.db.select(dbConn, new DBFilter());
            foreach (EEmpDocument o1 in mList)
                EEmpDocument.db.update(dbConn, o1);
            
            mList = EEmpEmergencyContact.db.select(dbConn, new DBFilter());
            foreach (EEmpEmergencyContact o1 in mList)
                EEmpEmergencyContact.db.update(dbConn, o1);
            
            mList = EEmpExtraField.db.select(dbConn, new DBFilter());
            foreach (EEmpExtraField o1 in mList)
                EEmpExtraField.db.update(dbConn, o1);
            mList = EEmpExtraFieldValue.db.select(dbConn, new DBFilter());
            foreach (EEmpExtraFieldValue o1 in mList)
                EEmpExtraFieldValue.db.update(dbConn, o1);
            mList = EEmpFinalPayment.db.select(dbConn, new DBFilter());
            foreach (EEmpFinalPayment o1 in mList)
                EEmpFinalPayment.db.update(dbConn, o1);
            mList = EEmpHierarchy.db.select(dbConn, new DBFilter());
            foreach (EEmpHierarchy o1 in mList)
                EEmpHierarchy.db.update(dbConn, o1);
            mList = EEmpMPFPlan.db.select(dbConn, new DBFilter());
            foreach (EEmpMPFPlan o1 in mList)
                EEmpMPFPlan.db.update(dbConn, o1);
            mList = EEmpORSOPlan.db.select(dbConn, new DBFilter());
            foreach (EEmpORSOPlan o1 in mList)
                EEmpORSOPlan.db.update(dbConn, o1);
            mList = EEmpPayroll.db.select(dbConn, new DBFilter());
            foreach (EEmpPayroll o1 in mList)
                EEmpPayroll.db.update(dbConn, o1);
            mList = EEmpPermit.db.select(dbConn, new DBFilter());
            foreach (EEmpPermit o1 in mList)
                EEmpPermit.db.update(dbConn, o1);
            mList = EEmpPersonalInfo.db.select(dbConn, new DBFilter());
            foreach (EEmpPersonalInfo o1 in mList)
                EEmpPersonalInfo.db.update(dbConn, o1);
            mList = EEmpPlaceOfResidence.db.select(dbConn, new DBFilter());
            foreach (EEmpPlaceOfResidence o1 in mList)
                EEmpPlaceOfResidence.db.update(dbConn, o1);
            mList = EEmpPositionInfo.db.select(dbConn, new DBFilter());
            foreach (EEmpPositionInfo o1 in mList)
                EEmpPositionInfo.db.update(dbConn, o1);
            mList = EEmpQualification.db.select(dbConn, new DBFilter());
            foreach (EEmpQualification o1 in mList)
                EEmpQualification.db.update(dbConn, o1);
            mList = EEmpRecurringPayment.db.select(dbConn, new DBFilter());
            foreach (EEmpRecurringPayment o1 in mList)
                EEmpRecurringPayment.db.update(dbConn, o1);
            mList = EEmpRequest.db.select(dbConn, new DBFilter());
            foreach (EEmpRequest o1 in mList)
                EEmpRequest.db.update(dbConn, o1);
            mList = EEmpRequestApprovalHistory.db.select(dbConn, new DBFilter());
            foreach (EEmpRequestApprovalHistory o1 in mList)
                EEmpRequestApprovalHistory.db.update(dbConn, o1);
            mList = EEmpRosterTableGroup.db.select(dbConn, new DBFilter());
            foreach (EEmpRosterTableGroup o1 in mList)
                EEmpRosterTableGroup.db.update(dbConn, o1);
            mList = EEmpSkill.db.select(dbConn, new DBFilter());
            foreach (EEmpSkill o1 in mList)
                EEmpSkill.db.update(dbConn, o1);
            mList = EEmpSpouse.db.select(dbConn, new DBFilter());
            foreach (EEmpSpouse o1 in mList)
                EEmpSpouse.db.update(dbConn, o1);
            mList = EEmpTermination.db.select(dbConn, new DBFilter());
            foreach (EEmpTermination o1 in mList)
                EEmpTermination.db.update(dbConn, o1);
            mList = EEmpTrainingEnroll.db.select(dbConn, new DBFilter());
            foreach (EEmpTrainingEnroll o1 in mList)
                EEmpTrainingEnroll.db.update(dbConn, o1);
            mList = EEmpUniform.db.select(dbConn, new DBFilter());
            foreach (EEmpUniform o1 in mList)
                EEmpUniform.db.update(dbConn, o1);
            mList = EEmpWorkExp.db.select(dbConn, new DBFilter());
            foreach (EEmpWorkExp o1 in mList)
                EEmpWorkExp.db.update(dbConn, o1);
            mList = EEmpWorkingSummary.db.select(dbConn, new DBFilter());
            foreach (EEmpWorkingSummary o1 in mList)
                EEmpWorkingSummary.db.update(dbConn, o1);
            mList = EEmpWorkInjuryRecord.db.select(dbConn, new DBFilter());
            foreach (EEmpWorkInjuryRecord o1 in mList)
                EEmpWorkInjuryRecord.db.update(dbConn, o1);
            mList = EORSORecord.db.select(dbConn, new DBFilter());
            foreach (EORSORecord o1 in mList)
                EORSORecord.db.update(dbConn, o1);
            mList = EESSLoginAudit.db.select(dbConn, new DBFilter());
            foreach (EESSLoginAudit o1 in mList)
                EESSLoginAudit.db.update(dbConn, o1);
            mList = EHierarchyElement.db.select(dbConn, new DBFilter());
            foreach (EHierarchyElement o1 in mList)
                EHierarchyElement.db.update(dbConn, o1);
            mList = EHierarchyLevel.db.select(dbConn, new DBFilter());
            foreach (EHierarchyLevel o1 in mList)
                EHierarchyLevel.db.update(dbConn, o1);
            mList = EInbox.db.select(dbConn, new DBFilter());
            foreach (EInbox o1 in mList)
                EInbox.db.update(dbConn, o1);
            mList = ELeaveApplication.db.select(dbConn, new DBFilter());
            foreach (ELeaveApplication o1 in mList)
                ELeaveApplication.db.update(dbConn, o1);
            mList = ELeaveApplicationCancel.db.select(dbConn, new DBFilter());
            foreach (ELeaveApplicationCancel o1 in mList)
                ELeaveApplicationCancel.db.update(dbConn, o1);
            mList = ELeaveBalance.db.select(dbConn, new DBFilter());
            foreach (ELeaveBalance o1 in mList)
                ELeaveBalance.db.update(dbConn, o1);

            mList = ELeaveBalanceAdjustment.db.select(dbConn, new DBFilter());
            foreach (ELeaveBalanceAdjustment o1 in mList)
                ELeaveBalanceAdjustment.db.update(dbConn, o1);
            mList = ELeaveBalanceEntitle.db.select(dbConn, new DBFilter());
            foreach (ELeaveBalanceEntitle o1 in mList)
                ELeaveBalanceEntitle.db.update(dbConn, o1);
            mList = ELeaveCode.db.select(dbConn, new DBFilter());
            foreach (ELeaveCode o1 in mList)
                ELeaveCode.db.update(dbConn, o1);
            mList = ELeavePlan.db.select(dbConn, new DBFilter());
            foreach (ELeavePlan o1 in mList)
                ELeavePlan.db.update(dbConn, o1);
            mList = ELeavePlanBroughtForward.db.select(dbConn, new DBFilter());
            foreach (ELeavePlanBroughtForward o1 in mList)
                ELeavePlanBroughtForward.db.update(dbConn, o1);
            mList = ELeavePlanEntitle.db.select(dbConn, new DBFilter());
            foreach (ELeavePlanEntitle o1 in mList)
                ELeavePlanEntitle.db.update(dbConn, o1);
            mList = ELeaveType.db.select(dbConn, new DBFilter());
            foreach (ELeaveType o1 in mList)
                ELeaveType.db.update(dbConn, o1);
            mList = ELoginAudit.db.select(dbConn, new DBFilter());
            foreach (ELoginAudit o1 in mList)
                ELoginAudit.db.update(dbConn, o1);
            mList = EMinimumWage.db.select(dbConn, new DBFilter());
            foreach (EMinimumWage o1 in mList)
                EMinimumWage.db.update(dbConn, o1);
            mList = EMPFParameter.db.select(dbConn, new DBFilter());
            foreach (EMPFParameter o1 in mList)
                EMPFParameter.db.update(dbConn, o1);
            mList = EMPFPlan.db.select(dbConn, new DBFilter());
            foreach (EMPFPlan o1 in mList)
                EMPFPlan.db.update(dbConn, o1);
            mList = EMPFRecord.db.select(dbConn, new DBFilter());
            foreach (EMPFRecord o1 in mList)
                EMPFRecord.db.update(dbConn, o1);
            mList = EMPFScheme.db.select(dbConn, new DBFilter());
            foreach (EMPFScheme o1 in mList)
                EMPFScheme.db.update(dbConn, o1);
            mList = EMPFSchemeCessationReason.db.select(dbConn, new DBFilter());
            foreach (EMPFSchemeCessationReason o1 in mList)
                EMPFSchemeCessationReason.db.update(dbConn, o1);
            mList = EMPFSchemeCessationReasonMapping.db.select(dbConn, new DBFilter());
            foreach (EMPFSchemeCessationReasonMapping o1 in mList)
                EMPFSchemeCessationReasonMapping.db.update(dbConn, o1);
            mList = EORSOPlan.db.select(dbConn, new DBFilter());
            foreach (EORSOPlan o1 in mList)
                EORSOPlan.db.update(dbConn, o1);
            mList = EORSOPlanDetail.db.select(dbConn, new DBFilter());
            foreach (EORSOPlanDetail o1 in mList)
                EORSOPlanDetail.db.update(dbConn, o1);

            mList = EPaymentCode.db.select(dbConn, new DBFilter());
            foreach (EPaymentCode o1 in mList)
                EPaymentCode.db.update(dbConn, o1);

            mList = EPaymentType.db.select(dbConn, new DBFilter());
            foreach (EPaymentType o1 in mList)
                EPaymentType.db.update(dbConn, o1);


            mList = EPaymentRecord.db.select(dbConn, new DBFilter());
            foreach (EPaymentRecord o1 in mList)
                EPaymentRecord.db.update(dbConn, o1);
            mList = EPayrollBatch.db.select(dbConn, new DBFilter());
            foreach (EPayrollBatch o1 in mList)
                EPayrollBatch.db.update(dbConn, o1);
            mList = EPayrollGroup.db.select(dbConn, new DBFilter());
            foreach (EPayrollGroup o1 in mList)
                EPayrollGroup.db.update(dbConn, o1);
            mList = EPayrollGroupLeaveCodeSetupOverride.db.select(dbConn, new DBFilter());
            foreach (EPayrollGroupLeaveCodeSetupOverride o1 in mList)
                EPayrollGroupLeaveCodeSetupOverride.db.update(dbConn, o1);
            mList = EPayrollPeriod.db.select(dbConn, new DBFilter());
            foreach (EPayrollPeriod o1 in mList)
                EPayrollPeriod.db.update(dbConn, o1);
            mList = EPayrollProrataFormula.db.select(dbConn, new DBFilter());
            foreach (EPayrollProrataFormula o1 in mList)
                EPayrollProrataFormula.db.update(dbConn, o1);
            mList = EReminderType.db.select(dbConn, new DBFilter());
            foreach (EReminderType o1 in mList)
                EReminderType.db.update(dbConn, o1);
            mList = ERequestEmpPersonalInfo.db.select(dbConn, new DBFilter());
            foreach (ERequestEmpPersonalInfo o1 in mList)
                ERequestEmpPersonalInfo.db.update(dbConn, o1);
            mList = ERequestLeaveApplication.db.select(dbConn, new DBFilter());
            foreach (ERequestLeaveApplication o1 in mList)
                ERequestLeaveApplication.db.update(dbConn, o1);
            mList = ERequestLeaveApplicationCancel.db.select(dbConn, new DBFilter());
            foreach (ERequestLeaveApplicationCancel o1 in mList)
                ERequestLeaveApplicationCancel.db.update(dbConn, o1);
            mList = ERosterClient.db.select(dbConn, new DBFilter());
            foreach (ERosterClient o1 in mList)
                ERosterClient.db.update(dbConn, o1);
            mList = ERosterCode.db.select(dbConn, new DBFilter());
            foreach (ERosterCode o1 in mList)
                ERosterCode.db.update(dbConn, o1);
            mList = ERosterCodeAdditionalPayment.db.select(dbConn, new DBFilter());
            foreach (ERosterCodeAdditionalPayment o1 in mList)
                ERosterCodeAdditionalPayment.db.update(dbConn, o1);
            mList = ERosterCodeDetail.db.select(dbConn, new DBFilter());
            foreach (ERosterCodeDetail o1 in mList)
                ERosterCodeDetail.db.update(dbConn, o1);
            mList = ERosterTable.db.select(dbConn, new DBFilter());
            foreach (ERosterTable o1 in mList)
                ERosterTable.db.update(dbConn, o1);
            mList = ERosterTableGroup.db.select(dbConn, new DBFilter());
            foreach (ERosterTableGroup o1 in mList)
                ERosterTableGroup.db.update(dbConn, o1);
            mList = ESystemFunction.db.select(dbConn, new DBFilter());
            foreach (ESystemFunction o1 in mList)
                ESystemFunction.db.update(dbConn, o1);
            mList = ESystemFunctionEmailAlert.db.select(dbConn, new DBFilter());
            foreach (ESystemFunctionEmailAlert o1 in mList)
                ESystemFunctionEmailAlert.db.update(dbConn, o1);
            mList = ESystemParameter.db.select(dbConn, new DBFilter());
            foreach (ESystemParameter o1 in mList)
                ESystemParameter.db.update(dbConn, o1);
            mList = ETaxCompany.db.select(dbConn, new DBFilter());
            foreach (ETaxCompany o1 in mList)
                ETaxCompany.db.update(dbConn, o1);
            mList = ETaxCompanyMap.db.select(dbConn, new DBFilter());
            foreach (ETaxCompanyMap o1 in mList)
                ETaxCompanyMap.db.update(dbConn, o1);
            mList = ETaxEmp.db.select(dbConn, new DBFilter());
            foreach (ETaxEmp o1 in mList)
                ETaxEmp.db.update(dbConn, o1);
            mList = ETaxEmpPayment.db.select(dbConn, new DBFilter());
            foreach (ETaxEmpPayment o1 in mList)
                ETaxEmpPayment.db.update(dbConn, o1);
            mList = ETaxEmpPlaceOfResidence.db.select(dbConn, new DBFilter());
            foreach (ETaxEmpPlaceOfResidence o1 in mList)
                ETaxEmpPlaceOfResidence.db.update(dbConn, o1);
            mList = ETaxForm.db.select(dbConn, new DBFilter());
            foreach (ETaxForm o1 in mList)
                ETaxForm.db.update(dbConn, o1);
            mList = ETaxPayment.db.select(dbConn, new DBFilter());
            foreach (ETaxPayment o1 in mList)
                ETaxPayment.db.update(dbConn, o1);
            mList = ETaxPaymentMap.db.select(dbConn, new DBFilter());
            foreach (ETaxPaymentMap o1 in mList)
                ETaxPaymentMap.db.update(dbConn, o1);
            mList = ETextTransformation.db.select(dbConn, new DBFilter());
            foreach (ETextTransformation o1 in mList)
                ETextTransformation.db.update(dbConn, o1);
            mList = ETimeCardLocationMap.db.select(dbConn, new DBFilter());
            foreach (ETimeCardLocationMap o1 in mList)
                ETimeCardLocationMap.db.update(dbConn, o1);
            mList = ETimeCardRecord.db.select(dbConn, new DBFilter());
            foreach (ETimeCardRecord o1 in mList)
                ETimeCardRecord.db.update(dbConn, o1);
            mList = ETrainingCourse.db.select(dbConn, new DBFilter());
            foreach (ETrainingCourse o1 in mList)
                ETrainingCourse.db.update(dbConn, o1);
            mList = ETrainingSeminar.db.select(dbConn, new DBFilter());
            foreach (ETrainingSeminar o1 in mList)
                ETrainingSeminar.db.update(dbConn, o1);
            mList = EUser.db.select(dbConn, new DBFilter());
            foreach (EUser o1 in mList)
                EUser.db.update(dbConn, o1);
            mList = EUserCompany.db.select(dbConn, new DBFilter());
            foreach (EUserCompany o1 in mList)
                EUserCompany.db.update(dbConn, o1);
            mList = EUserGroup.db.select(dbConn, new DBFilter());
            foreach (EUserGroup o1 in mList)
                EUserGroup.db.update(dbConn, o1);
            mList = EUserGroupAccess.db.select(dbConn, new DBFilter());
            foreach (EUserGroupAccess o1 in mList)
                EUserGroupAccess.db.update(dbConn, o1);
            mList = EUserGroupFunction.db.select(dbConn, new DBFilter());
            foreach (EUserGroupFunction o1 in mList)
                EUserGroupFunction.db.update(dbConn, o1);
            mList = EUserRank.db.select(dbConn, new DBFilter());
            foreach (EUserRank o1 in mList)
                EUserRank.db.update(dbConn, o1);
            mList = EUserReminderOption.db.select(dbConn, new DBFilter());
            foreach (EUserReminderOption o1 in mList)
                EUserReminderOption.db.update(dbConn, o1);
         
            mList = EWorkHourPattern.db.select(dbConn, new DBFilter());
            foreach (EWorkHourPattern o1 in mList)
                EWorkHourPattern.db.update(dbConn, o1);
            mList = EYEBPlan.db.select(dbConn, new DBFilter());
            foreach (EYEBPlan o1 in mList)
                EYEBPlan.db.update(dbConn, o1);
            #endregion code_update_data

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void txtCurrentKey_TextChanged(object sender, EventArgs e)
        {

        }

        private void cmdDecrypt2_Click(object sender, EventArgs e)
        {
            string connectionString = "Data Source=" + txtServer.Text + ";User ID=" + txtUserID.Text + ";Password=" + txtPassword.Text + ";Initial Catalog=" + txtDatabase.Text;

            DatabaseConnection dbConn = new DatabaseConnection(connectionString, DatabaseConnection.DatabaseType.MSSQL);
            txtResult.Text = txtResult.Text + "DB Connection established" + "\r\n";
            Application.DoEvents();

            DBAESEncryptStringFieldAttribute.skipEncryptionToDB = true;
            DBAESEncryptStringFieldAttribute.DEFAULT_KEY_STRING = txtCurrentKey.Text;

            txtResult.Text = txtResult.Text + "Decrypting data, please wait....\r\n";
            Cursor.Current = Cursors.WaitCursor;
            Application.DoEvents();

            update_data2(dbConn);

            txtResult.Text = txtResult.Text + "Conversion completed. Please check!\r\n";
            Cursor.Current = Cursors.Default;

        }

        private void cmdEncrypt2_Click(object sender, EventArgs e)
        {
            string connectionString = "Data Source=" + txtServer.Text + ";User ID=" + txtUserID.Text + ";Password=" + txtPassword.Text + ";Initial Catalog=" + txtDatabase.Text;

            DatabaseConnection dbConn = new DatabaseConnection(connectionString, DatabaseConnection.DatabaseType.MSSQL);
            txtResult.Text = txtResult.Text + "DB Connection establishe\r\n";

            DBAESEncryptStringFieldAttribute.skipEncryptionToDB = false;
            DBAESEncryptStringFieldAttribute.DEFAULT_KEY_STRING = txtNewKey.Text;

            txtResult.Text = txtResult.Text + "Encrypting data, please wait....\r\n";
            Cursor.Current = Cursors.WaitCursor;
            Application.DoEvents();

            update_data2(dbConn);

            txtResult.Text = txtResult.Text + "Conversion completed. Please check!\r\n";
            Cursor.Current = Cursors.Default;
            Application.DoEvents();
        }

        private void txtTargetDBVersion_TextChanged(object sender, EventArgs e)
        {

        }


    }
}