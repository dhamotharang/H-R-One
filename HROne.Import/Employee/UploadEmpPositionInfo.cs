using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Text;
using HROne.DataAccess;
//using perspectivemind.validation;
using HROne.Lib.Entities;

namespace HROne.Import
{

    /// <summary>
    /// Summary description for ImportEmpPositionInfoProcess
    /// </summary>
    public class ImportEmpPositionInfoProcess : ImportProcessInteface
    {
        [DBClass("UploadEmpPositionInfo")]
        private class EUploadEmpPositionInfo : ImportDBObject
        {
            public static DBManager db = new DBManager(typeof(EUploadEmpPositionInfo));
            protected int m_UploadEmpPosID;
            [DBField("UploadEmpPosID", true, true), TextSearch, Export(false)]
            public int UploadEmpPosID
            {
                get { return m_UploadEmpPosID; }
                set { m_UploadEmpPosID = value; modify("UploadEmpPosID"); }
            }
            protected int m_UploadEmpID;
            [DBField("UploadEmpID"), TextSearch, Export(false)]
            public int UploadEmpID
            {
                get { return m_UploadEmpID; }
                set { m_UploadEmpID = value; modify("UploadEmpID"); }
            }

            protected int m_EmpPosID;
            [DBField("EmpPosID"), TextSearch, Export(false)]
            public int EmpPosID
            {
                get { return m_EmpPosID; }
                set { m_EmpPosID = value; modify("EmpPosID"); }
            }
            protected int m_EmpID;
            [DBField("EmpID"), TextSearch, Export(false)]
            public int EmpID
            {
                get { return m_EmpID; }
                set { m_EmpID = value; modify("EmpID"); }
            }
            protected DateTime m_EmpPosEffFr;
            [DBField("EmpPosEffFr"), TextSearch, MaxLength(25), Export(false), Required]
            public DateTime EmpPosEffFr
            {
                get { return m_EmpPosEffFr; }
                set { m_EmpPosEffFr = value; modify("EmpPosEffFr"); }
            }
            protected DateTime m_EmpPosEffTo;
            [DBField("EmpPosEffTo"), TextSearch, MaxLength(25), Export(false)]
            public DateTime EmpPosEffTo
            {
                get { return m_EmpPosEffTo; }
                set { m_EmpPosEffTo = value; modify("EmpPosEffTo"); }
            }
            protected int m_CompanyID;
            [DBField("CompanyID"), TextSearch, Export(false), Required]
            public int CompanyID
            {
                get { return m_CompanyID; }
                set { m_CompanyID = value; modify("CompanyID"); }
            }
            protected int m_PositionID;
            [DBField("PositionID"), TextSearch, Export(false), Required]
            public int PositionID
            {
                get { return m_PositionID; }
                set { m_PositionID = value; modify("PositionID"); }
            }
            protected int m_RankID;
            [DBField("RankID"), TextSearch, Export(false), Required]
            public int RankID
            {
                get { return m_RankID; }
                set { m_RankID = value; modify("RankID"); }
            }
            protected int m_EmploymentTypeID;
            [DBField("EmploymentTypeID"), TextSearch, Export(false)]
            public int EmploymentTypeID
            {
                get { return m_EmploymentTypeID; }
                set { m_EmploymentTypeID = value; modify("EmploymentTypeID"); }
            }
            protected int m_StaffTypeID;
            [DBField("StaffTypeID"), TextSearch, Export(false)]
            public int StaffTypeID
            {
                get { return m_StaffTypeID; }
                set { m_StaffTypeID = value; modify("StaffTypeID"); }
            }
            protected int m_PayGroupID;
            [DBField("PayGroupID"), TextSearch, Export(false)]
            public int PayGroupID
            {
                get { return m_PayGroupID; }
                set { m_PayGroupID = value; modify("PayGroupID"); }
            }
            protected int m_LeavePlanID;
            [DBField("LeavePlanID"), TextSearch, Export(false)]
            public int LeavePlanID
            {
                get { return m_LeavePlanID; }
                set { m_LeavePlanID = value; modify("LeavePlanID"); }
            }
            protected bool m_EmpPosIsLeavePlanResetEffectiveDate;
            [DBField("EmpPosIsLeavePlanResetEffectiveDate"), TextSearch, Export(false)]
            public bool EmpPosIsLeavePlanResetEffectiveDate
            {
                get { return m_EmpPosIsLeavePlanResetEffectiveDate; }
                set { m_EmpPosIsLeavePlanResetEffectiveDate = value; modify("EmpPosIsLeavePlanResetEffectiveDate"); }
            }
            protected int m_YEBPlanID;
            [DBField("YEBPlanID"), TextSearch, Export(false)]
            public int YEBPlanID
            {
                get { return m_YEBPlanID; }
                set { m_YEBPlanID = value; modify("YEBPlanID"); }
            }
            //protected int m_EmpFirstAuthorizationGp;
            //[DBField("EmpFirstAuthorizationGp"), TextSearch, Export(false)]
            //public int EmpFirstAuthorizationGp
            //{
            //    get { return m_EmpFirstAuthorizationGp; }
            //    set { m_EmpFirstAuthorizationGp = value; modify("EmpFirstAuthorizationGp"); }
            //}
            //protected int m_EmpSecondAuthorizationGp;
            //[DBField("EmpSecondAuthorizationGp"), TextSearch, Export(false)]
            //public int EmpSecondAuthorizationGp
            //{
            //    get { return m_EmpSecondAuthorizationGp; }
            //    set { m_EmpSecondAuthorizationGp = value; modify("EmpSecondAuthorizationGp"); }
            //}
            protected int m_AuthorizationWorkFlowIDLeaveApp;
            [DBField("AuthorizationWorkFlowIDLeaveApp"), TextSearch, Export(false)]
            public int AuthorizationWorkFlowIDLeaveApp
            {
                get { return m_AuthorizationWorkFlowIDLeaveApp; }
                set { m_AuthorizationWorkFlowIDLeaveApp = value; modify("AuthorizationWorkFlowIDLeaveApp"); }
            }
            protected int m_AuthorizationWorkFlowIDEmpInfoModified;
            [DBField("AuthorizationWorkFlowIDEmpInfoModified"), TextSearch, Export(false)]
            public int AuthorizationWorkFlowIDEmpInfoModified
            {
                get { return m_AuthorizationWorkFlowIDEmpInfoModified; }
                set { m_AuthorizationWorkFlowIDEmpInfoModified = value; modify("AuthorizationWorkFlowIDEmpInfoModified"); }
            }
            // Start 0000060, Miranda, 2014-07-15
            protected int m_AuthorizationWorkFlowIDOTClaims;
            [DBField("AuthorizationWorkFlowIDOTClaims"), TextSearch, Export(false)]
            public int AuthorizationWorkFlowIDOTClaims
            {
                get { return m_AuthorizationWorkFlowIDOTClaims; }
                set { m_AuthorizationWorkFlowIDOTClaims = value; modify("AuthorizationWorkFlowIDOTClaims"); }
            }
            // End 0000060, Miranda, 2014-07-15
            // Start 0000112, Miranda, 2014-12-10
            protected int m_AuthorizationWorkFlowIDLateWaive;
            [DBField("AuthorizationWorkFlowIDLateWaive"), TextSearch, Export(false)]
            public int AuthorizationWorkFlowIDLateWaive
            {
                get { return m_AuthorizationWorkFlowIDLateWaive; }
                set { m_AuthorizationWorkFlowIDLateWaive = value; modify("AuthorizationWorkFlowIDLateWaive"); }
            }
            // End 0000112, Miranda, 2014-12-10

            protected int m_AttendancePlanID;
            [DBField("AttendancePlanID"), TextSearch, Export(false)]
            public int AttendancePlanID
            {
                get { return m_AttendancePlanID; }
                set { m_AttendancePlanID = value; modify("AttendancePlanID"); }
            }
            
            //protected int m_EmpPosDefaultRosterCodeID;
            //[DBField("EmpPosDefaultRosterCodeID"), TextSearch, Export(false)]
            //public int EmpPosDefaultRosterCodeID
            //{
            //    get { return m_EmpPosDefaultRosterCodeID; }
            //    set { m_EmpPosDefaultRosterCodeID = value; modify("EmpPosDefaultRosterCodeID"); }
            //}

            protected int m_WorkHourPatternID;
            [DBField("WorkHourPatternID"), TextSearch, Export(false)]
            public int WorkHourPatternID
            {
                get { return m_WorkHourPatternID; }
                set { m_WorkHourPatternID = value; modify("WorkHourPatternID"); }
            }

            //protected int m_RosterTableGroupID;
            //[DBField("RosterTableGroupID"), TextSearch, Export(false)]
            //public int RosterTableGroupID
            //{
            //    get { return m_RosterTableGroupID; }
            //    set { m_RosterTableGroupID = value; modify("RosterTableGroupID"); }
            //}

            //protected bool m_EmpPosIsRosterTableGroupSupervisor;
            //[DBField("EmpPosIsRosterTableGroupSupervisor"), TextSearch, Export(false)]
            //public bool EmpPosIsRosterTableGroupSupervisor
            //{
            //    get { return m_EmpPosIsRosterTableGroupSupervisor; }
            //    set { m_EmpPosIsRosterTableGroupSupervisor = value; modify("EmpPosIsRosterTableGroupSupervisor"); }
            //}

            protected string m_EmpPosRemark;
            [DBField("EmpPosRemark"), TextSearch, Export(false)]
            public string EmpPosRemark
            {
                get { return m_EmpPosRemark; }
                set { m_EmpPosRemark = value; modify("EmpPosRemark"); }
            }
            //  For Synchronize Use only
            protected string m_SynID;
            [DBField("SynID"), TextSearch, Export(false)]
            public string SynID
            {
                get { return m_SynID; }
                set { m_SynID = value; modify("SynID"); }
            }

        }

        [DBClass("UploadEmpHierarchy")]
        private class EUploadEmpHierarchy : DBObject
        {
            public static DBManager db = new DBManager(typeof(EUploadEmpHierarchy));
            protected int m_UploadEmpHierarchyID;
            [DBField("UploadEmpHierarchyID", true, true), TextSearch, Export(false)]
            public int UploadEmpHierarchyID
            {
                get { return m_UploadEmpHierarchyID; }
                set { m_UploadEmpHierarchyID = value; modify("UploadEmpHierarchyID"); }
            }
            protected int m_UploadEmpPosID;
            [DBField("UploadEmpPosID"), TextSearch, Export(false)]
            public int UploadEmpPosID
            {
                get { return m_UploadEmpPosID; }
                set { m_UploadEmpPosID = value; modify("UploadEmpPosID"); }
            }

            protected int m_EmpHierarchyID;
            [DBField("EmpHierarchyID"), TextSearch, Export(false)]
            public int EmpHierarchyID
            {
                get { return m_EmpHierarchyID; }
                set { m_EmpHierarchyID = value; modify("EmpHierarchyID"); }
            }
            protected int m_EmpID;
            [DBField("EmpID"), TextSearch, Export(false)]
            public int EmpID
            {
                get { return m_EmpID; }
                set { m_EmpID = value; modify("EmpID"); }
            }
            protected int m_EmpPosID;
            [DBField("EmpPosID"), TextSearch, Export(false)]
            public int EmpPosID
            {
                get { return m_EmpPosID; }
                set { m_EmpPosID = value; modify("EmpPosID"); }
            }
            protected int m_HElementID;
            [DBField("HElementID"), TextSearch, Export(false), Required]
            public int HElementID
            {
                get { return m_HElementID; }
                set { m_HElementID = value; modify("HElementID"); }
            }
            protected int m_HLevelID;
            [DBField("HLevelID"), TextSearch, Export(false), Required]
            public int HLevelID
            {
                get { return m_HLevelID; }
                set { m_HLevelID = value; modify("HLevelID"); }
            }
        }

        public const string TABLE_NAME = "position_history";
        private const string HIERARCHY_LEVEL_PREFIX = "Hierarchy ";

        //  Field Const for Employee Personal Information
        private const string FIELD_EMP_NO = "Emp No";
        private const string FIELD_FROM = "From";
        private const string FIELD_TO = "To";
        private const string FIELD_COMPANY = "Company";
        private const string FIELD_POSITION = "Position";
        private const string FIELD_RANK = "Rank";
        private const string FIELD_STAFF_TYPE = "Staff Type";
        private const string FIELD_EMPLOYMENT_TYPE = "Employment Type";
        private const string FIELD_PAYROLL_GROUP = "Payroll Group";
        private const string FIELD_LEAVE_PLAN = "Leave Plan";
        private const string FIELD_LEAVE_PLAN_IS_RESET_GRANT_DATE = "Is Reset Grant Date of new Leave Plan";
        private const string FIELD_YEB_PLAN = "YEB Plan";
        //private const string FIELD_ESS_1ST_AUTHORIZATION_GROUP = "First Authorization Group";
        //private const string FIELD_ESS_2ND_AUTHORIZATION_GROUP = "Second Authorization Group";
        private const string FIELD_ESS_AUTHORIZATION_WORKFLOW_LEAVEAPP = "Authorization Workflow for Leave Application";
        private const string FIELD_ESS_AUTHORIZATION_WORKFLOW_EEINFO = "Authorization Workflow for Personal Information Change";
        // Start 0000060, Miranda, 2014-07-15
        private const string FIELD_ESS_AUTHORIZATION_WORKFLOW_EEOT = "Authorization Workflow for CL Requisition";
        // End 0000060, Miranda, 2014-07-15
        // Start 0000112, Miranda, 2014-12-10
        private const string FIELD_ESS_AUTHORIZATION_WORKFLOW_LateWaive = "Authorization Workflow for Late Waive";
        // End 0000112, Miranda, 2014-12-10
        private const string FIELD_ATTENDANCE_PLAN = "Attendance Plan";
        //private const string FIELD_DEFAULT_ROSTER_CODE = "Default Roster Code";
        private const string FIELD_WORK_HOUR_PATTERN = "Work Hour Pattern";
        private const string FIELD_ROSTER_TABLE_GROUP = "Roster Table Group";
        private const string FIELD_IS_ROSTER_TABLE_GROUP_SUPERVISOR = "Is Roster Table Group Supervisor";
        private const string FIELD_REMARK = "Remark";

        
        private DateTime UploadDateTime = AppUtils.ServerDateTime();
        private DBManager tempDB = EUploadEmpPositionInfo.db;
        private DBManager uploadDB = EEmpPositionInfo.db;
        private DBManager tempHierarchyDB = EUploadEmpHierarchy.db;
        private DBManager uploadHierarchyDB = EEmpHierarchy.db;

        public ImportErrorList errors = new ImportErrorList();
        

        public ImportEmpPositionInfoProcess(DatabaseConnection dbConn, string SessionID): base(dbConn, SessionID)
        {
          
            
        }


        public DataTable UploadToTempDatabase(DataTable rawDataTable, int UserID, bool CreateCodeIfNotExists)
        {
            if (rawDataTable == null)
                return GetImportDataFromTempDatabase(null);

            int rowCount = 1;
            foreach (DataRow row in rawDataTable.Rows)
            {
                rowCount++;

                EUploadEmpPositionInfo uploadEmpPos = new EUploadEmpPositionInfo();
                EEmpPositionInfo lastEmpPos = null;
                ArrayList uploadHierarchyList = new ArrayList();
                ArrayList lastHierarchyList = null;

                string EmpNo = row[FIELD_EMP_NO].ToString().Trim();
                uploadEmpPos.EmpID = Parse.GetEmpID(dbConn, EmpNo, UserID);
                if (uploadEmpPos.EmpID < 0)
                    errors.addError(ImportErrorMessage.ERROR_ACCESS_DENIED_EMP_NO, new string[] { EmpNo, rowCount.ToString() });
                try
                {
                    uploadEmpPos.EmpPosEffFr = Parse.toDateTimeObject(row[FIELD_FROM]);
                }
                catch
                {
                    errors.addError(ImportErrorMessage.ERROR_INVALID_FIELD_VALUE, new string[] { FIELD_FROM + "=" + row[FIELD_FROM].ToString(), EmpNo, rowCount.ToString() });
                }
                try
                {
                    uploadEmpPos.EmpPosEffTo = Parse.toDateTimeObject(row[FIELD_TO]);
                }
                catch
                {
                    errors.addError(ImportErrorMessage.ERROR_INVALID_FIELD_VALUE, new string[] { FIELD_TO + "=" + row[FIELD_TO].ToString(), EmpNo, rowCount.ToString() });
                }
                if (!row[FIELD_COMPANY].ToString().Trim().Equals(string.Empty))
                {
                    uploadEmpPos.CompanyID = Parse.GetCompanyID(dbConn, row[FIELD_COMPANY].ToString(), CreateCodeIfNotExists, UserID);
                    if (uploadEmpPos.CompanyID.Equals(0))
                        errors.addError(ImportErrorMessage.ERROR_INVALID_FIELD_VALUE, new string[] { FIELD_COMPANY + "=" + row[FIELD_COMPANY].ToString(), EmpNo, rowCount.ToString() });
                }
                else
                    uploadEmpPos.CompanyID = 0;
                if (!row[FIELD_POSITION].ToString().Trim().Equals(string.Empty))
                {
                    uploadEmpPos.PositionID = Parse.GetPositionID(dbConn, row[FIELD_POSITION].ToString(), CreateCodeIfNotExists, UserID);
                    if (uploadEmpPos.PositionID.Equals(0))
                        errors.addError(ImportErrorMessage.ERROR_INVALID_FIELD_VALUE, new string[] { FIELD_POSITION + "=" + row[FIELD_POSITION].ToString(), EmpNo, rowCount.ToString() });
                }
                else
                    uploadEmpPos.PositionID = 0;
                if (!row[FIELD_RANK].ToString().Trim().Equals(string.Empty))
                {
                    uploadEmpPos.RankID = Parse.GetRankID(dbConn, row[FIELD_RANK].ToString(), CreateCodeIfNotExists, UserID);
                    if (uploadEmpPos.RankID.Equals(0))
                        errors.addError(ImportErrorMessage.ERROR_INVALID_FIELD_VALUE, new string[] { FIELD_RANK + "=" + row[FIELD_RANK].ToString(), EmpNo, rowCount.ToString() });
                }
                else
                    uploadEmpPos.RankID = 0;

                if (!row[FIELD_STAFF_TYPE].ToString().Trim().Equals(string.Empty))
                {
                    uploadEmpPos.StaffTypeID = Parse.GetStaffTypeID(dbConn, row[FIELD_STAFF_TYPE].ToString(), CreateCodeIfNotExists, UserID);
                    if (uploadEmpPos.StaffTypeID.Equals(0))
                        errors.addError(ImportErrorMessage.ERROR_INVALID_FIELD_VALUE, new string[] { FIELD_STAFF_TYPE + "=" + row[FIELD_STAFF_TYPE].ToString(), EmpNo, rowCount.ToString() });
                }
                else
                    uploadEmpPos.StaffTypeID = 0;

                if (rawDataTable.Columns.Contains(FIELD_EMPLOYMENT_TYPE))
                    if (!row[FIELD_EMPLOYMENT_TYPE].ToString().Trim().Equals(string.Empty))
                    {
                        uploadEmpPos.EmploymentTypeID = Parse.GetEmploymentTypeID(dbConn, row[FIELD_EMPLOYMENT_TYPE].ToString(), CreateCodeIfNotExists, UserID);
                        if (uploadEmpPos.EmploymentTypeID.Equals(0))
                            errors.addError(ImportErrorMessage.ERROR_INVALID_FIELD_VALUE, new string[] { FIELD_EMPLOYMENT_TYPE + "=" + row[FIELD_EMPLOYMENT_TYPE].ToString(), EmpNo, rowCount.ToString() });
                    }
                    else
                        uploadEmpPos.EmploymentTypeID = 0;
                if (!row[FIELD_PAYROLL_GROUP].ToString().Trim().Equals(string.Empty))
                {
                    uploadEmpPos.PayGroupID = Parse.GetPayrollGroupID(dbConn, row[FIELD_PAYROLL_GROUP].ToString(), CreateCodeIfNotExists, UserID);
                    if (uploadEmpPos.PayGroupID.Equals(0))
                        errors.addError(ImportErrorMessage.ERROR_INVALID_FIELD_VALUE, new string[] { FIELD_PAYROLL_GROUP + "=" + row[FIELD_PAYROLL_GROUP].ToString(), EmpNo, rowCount.ToString() });
                }
                else
                    uploadEmpPos.PayGroupID = 0;
                if (!row[FIELD_LEAVE_PLAN].ToString().Trim().Equals(string.Empty))
                {
                    uploadEmpPos.LeavePlanID = Parse.GetLeavePlanID(dbConn, row[FIELD_LEAVE_PLAN].ToString(), CreateCodeIfNotExists, UserID);
                    if (uploadEmpPos.LeavePlanID.Equals(0))
                        errors.addError(ImportErrorMessage.ERROR_INVALID_FIELD_VALUE, new string[] { FIELD_LEAVE_PLAN + "=" + row[FIELD_LEAVE_PLAN].ToString(), EmpNo, rowCount.ToString() });
                }
                else
                    uploadEmpPos.LeavePlanID = 0;

                if (rawDataTable.Columns.Contains(FIELD_LEAVE_PLAN_IS_RESET_GRANT_DATE))
                {
                    uploadEmpPos.EmpPosIsLeavePlanResetEffectiveDate = row[FIELD_LEAVE_PLAN_IS_RESET_GRANT_DATE].ToString().Equals("Yes", StringComparison.CurrentCultureIgnoreCase) || row[FIELD_LEAVE_PLAN_IS_RESET_GRANT_DATE].ToString().Equals("Y", StringComparison.CurrentCultureIgnoreCase);
                }

                if (rawDataTable.Columns.Contains(FIELD_YEB_PLAN))
                {
                    if (!row[FIELD_YEB_PLAN].ToString().Trim().Equals(string.Empty))
                    {
                        uploadEmpPos.YEBPlanID = Parse.GetYEBPlanID(dbConn, row[FIELD_YEB_PLAN].ToString(), false, UserID);
                        if (uploadEmpPos.YEBPlanID.Equals(0))
                            errors.addError(ImportErrorMessage.ERROR_INVALID_FIELD_VALUE, new string[] { FIELD_YEB_PLAN + "=" + row[FIELD_YEB_PLAN].ToString(), EmpNo, rowCount.ToString() });
                    }
                    else
                        uploadEmpPos.YEBPlanID = 0;
                }

                //if (rawDataTable.Columns.Contains(FIELD_ESS_1ST_AUTHORIZATION_GROUP))
                //{
                //    if (!row[FIELD_ESS_1ST_AUTHORIZATION_GROUP].ToString().Trim().Equals(string.Empty))
                //    {
                //        uploadEmpPos.EmpFirstAuthorizationGp = Parse.GetAuthorizationGroupID(dbConn, row[FIELD_ESS_1ST_AUTHORIZATION_GROUP].ToString(), false, UserID);
                //        if (uploadEmpPos.EmpFirstAuthorizationGp.Equals(0))
                //            errors.addError(ImportErrorMessage.ERROR_INVALID_FIELD_VALUE, new string[] { FIELD_ESS_1ST_AUTHORIZATION_GROUP + "=" + row[FIELD_ESS_1ST_AUTHORIZATION_GROUP].ToString(), EmpNo, rowCount.ToString() });
                //    }
                //    else
                //        uploadEmpPos.EmpFirstAuthorizationGp = 0;
                //}

                //if (rawDataTable.Columns.Contains(FIELD_ESS_2ND_AUTHORIZATION_GROUP))
                //{
                //    if (!row[FIELD_ESS_2ND_AUTHORIZATION_GROUP].ToString().Trim().Equals(string.Empty))
                //    {
                //        uploadEmpPos.EmpSecondAuthorizationGp = Parse.GetAuthorizationGroupID(dbConn, row[FIELD_ESS_2ND_AUTHORIZATION_GROUP].ToString(), false, UserID);
                //        if (uploadEmpPos.EmpSecondAuthorizationGp.Equals(0))
                //            errors.addError(ImportErrorMessage.ERROR_INVALID_FIELD_VALUE, new string[] { FIELD_ESS_2ND_AUTHORIZATION_GROUP + "=" + row[FIELD_ESS_2ND_AUTHORIZATION_GROUP].ToString(), EmpNo, rowCount.ToString() });
                //    }
                //    else
                //        uploadEmpPos.EmpSecondAuthorizationGp = 0;
                //}
                if (rawDataTable.Columns.Contains(FIELD_ESS_AUTHORIZATION_WORKFLOW_LEAVEAPP))
                {
                    if (!row[FIELD_ESS_AUTHORIZATION_WORKFLOW_LEAVEAPP].ToString().Trim().Equals(string.Empty))
                    {
                        uploadEmpPos.AuthorizationWorkFlowIDLeaveApp = Parse.GetAuthorizationWorkFlowID(dbConn, row[FIELD_ESS_AUTHORIZATION_WORKFLOW_LEAVEAPP].ToString(), false, UserID);
                        if (uploadEmpPos.AuthorizationWorkFlowIDLeaveApp.Equals(0))
                            errors.addError(ImportErrorMessage.ERROR_INVALID_FIELD_VALUE, new string[] { FIELD_ESS_AUTHORIZATION_WORKFLOW_LEAVEAPP + "=" + row[FIELD_ESS_AUTHORIZATION_WORKFLOW_LEAVEAPP].ToString(), EmpNo, rowCount.ToString() });
                    }
                    else
                        uploadEmpPos.AuthorizationWorkFlowIDLeaveApp = 0;
                }

                if (rawDataTable.Columns.Contains(FIELD_ESS_AUTHORIZATION_WORKFLOW_EEINFO))
                {
                    if (!row[FIELD_ESS_AUTHORIZATION_WORKFLOW_EEINFO].ToString().Trim().Equals(string.Empty))
                    {
                        uploadEmpPos.AuthorizationWorkFlowIDEmpInfoModified = Parse.GetAuthorizationWorkFlowID(dbConn, row[FIELD_ESS_AUTHORIZATION_WORKFLOW_EEINFO].ToString(), false, UserID);
                        if (uploadEmpPos.AuthorizationWorkFlowIDEmpInfoModified.Equals(0))
                            errors.addError(ImportErrorMessage.ERROR_INVALID_FIELD_VALUE, new string[] { FIELD_ESS_AUTHORIZATION_WORKFLOW_EEINFO + "=" + row[FIELD_ESS_AUTHORIZATION_WORKFLOW_EEINFO].ToString(), EmpNo, rowCount.ToString() });
                    }
                    else
                        uploadEmpPos.AuthorizationWorkFlowIDEmpInfoModified = 0;
                }
                // Start 0000060, Miranda, 2014-07-15
                if (rawDataTable.Columns.Contains(FIELD_ESS_AUTHORIZATION_WORKFLOW_EEOT))
                {
                    if (!row[FIELD_ESS_AUTHORIZATION_WORKFLOW_EEOT].ToString().Trim().Equals(string.Empty))
                    {
                        uploadEmpPos.AuthorizationWorkFlowIDOTClaims = Parse.GetAuthorizationWorkFlowID(dbConn, row[FIELD_ESS_AUTHORIZATION_WORKFLOW_EEOT].ToString(), false, UserID);
                        if (uploadEmpPos.AuthorizationWorkFlowIDOTClaims.Equals(0))
                            errors.addError(ImportErrorMessage.ERROR_INVALID_FIELD_VALUE, new string[] { FIELD_ESS_AUTHORIZATION_WORKFLOW_EEOT + "=" + row[FIELD_ESS_AUTHORIZATION_WORKFLOW_EEOT].ToString(), EmpNo, rowCount.ToString() });
                    }
                    else
                        uploadEmpPos.AuthorizationWorkFlowIDOTClaims = 0;
                }
                // End 0000060, Miranda, 2014-07-15
                // Start 0000112, Miranda, 2014-12-10
                if (rawDataTable.Columns.Contains(FIELD_ESS_AUTHORIZATION_WORKFLOW_LateWaive))
                {
                    if (!row[FIELD_ESS_AUTHORIZATION_WORKFLOW_LateWaive].ToString().Trim().Equals(string.Empty))
                    {
                        uploadEmpPos.AuthorizationWorkFlowIDLateWaive = Parse.GetAuthorizationWorkFlowID(dbConn, row[FIELD_ESS_AUTHORIZATION_WORKFLOW_LateWaive].ToString(), false, UserID);
                        if (uploadEmpPos.AuthorizationWorkFlowIDLateWaive.Equals(0))
                            errors.addError(ImportErrorMessage.ERROR_INVALID_FIELD_VALUE, new string[] { FIELD_ESS_AUTHORIZATION_WORKFLOW_LateWaive + "=" + row[FIELD_ESS_AUTHORIZATION_WORKFLOW_LateWaive].ToString(), EmpNo, rowCount.ToString() });
                    }
                    else
                        uploadEmpPos.AuthorizationWorkFlowIDLateWaive = 0;
                }
                // End 0000112, Miranda, 2014-12-10

                if (rawDataTable.Columns.Contains(FIELD_ATTENDANCE_PLAN))
                {
                    if (!row[FIELD_ATTENDANCE_PLAN].ToString().Trim().Equals(string.Empty))
                    {
                        uploadEmpPos.AttendancePlanID = Parse.GetAttendancePlanID(dbConn, row[FIELD_ATTENDANCE_PLAN].ToString(), false, UserID);
                        if (uploadEmpPos.AttendancePlanID.Equals(0))
                            errors.addError(ImportErrorMessage.ERROR_INVALID_FIELD_VALUE, new string[] { FIELD_ATTENDANCE_PLAN + "=" + row[FIELD_ATTENDANCE_PLAN].ToString(), EmpNo, rowCount.ToString() });
                    }
                    else
                        uploadEmpPos.AttendancePlanID = 0;
                }

                //if (rawDataTable.Columns.Contains(FIELD_DEFAULT_ROSTER_CODE))
                //{
                //    if (!row[FIELD_DEFAULT_ROSTER_CODE].ToString().Trim().Equals(string.Empty))
                //    {
                //        uploadEmpPos.EmpPosDefaultRosterCodeID = Parse.*ID(dbConn, row[FIELD_DEFAULT_ROSTER_CODE].ToString());
                //        if (uploadEmpPos.EmpPosDefaultRosterCodeID.Equals(0))
                //            errors.addError(ImportErrorMessage.ERROR_INVALID_FIELD_VALUE, new string[] { FIELD_DEFAULT_ROSTER_CODE + "=" + row[FIELD_DEFAULT_ROSTER_CODE].ToString(), EmpNo, rowCount.ToString() });
                //    }
                //}

                if (rawDataTable.Columns.Contains(FIELD_WORK_HOUR_PATTERN))
                {
                    if (!row[FIELD_WORK_HOUR_PATTERN].ToString().Trim().Equals(string.Empty))
                    {
                        uploadEmpPos.WorkHourPatternID = Parse.GetWorkHourPatternID(dbConn, row[FIELD_WORK_HOUR_PATTERN].ToString());
                        if (uploadEmpPos.WorkHourPatternID.Equals(0))
                            errors.addError(ImportErrorMessage.ERROR_INVALID_FIELD_VALUE, new string[] { FIELD_WORK_HOUR_PATTERN + "=" + row[FIELD_WORK_HOUR_PATTERN].ToString(), EmpNo, rowCount.ToString() });
                    }
                    else
                        uploadEmpPos.WorkHourPatternID = 0;
                }
                //if (rawDataTable.Columns.Contains(FIELD_ROSTER_TABLE_GROUP))
                //{
                //    if (!row[FIELD_ROSTER_TABLE_GROUP].ToString().Trim().Equals(string.Empty))
                //    {
                //        uploadEmpPos.RosterTableGroupID = Parse.GetRosterTableGroupID(dbConn, row[FIELD_ROSTER_TABLE_GROUP].ToString());
                //        if (uploadEmpPos.RosterTableGroupID.Equals(0))
                //            errors.addError(ImportErrorMessage.ERROR_INVALID_FIELD_VALUE, new string[] { FIELD_ROSTER_TABLE_GROUP + "=" + row[FIELD_ROSTER_TABLE_GROUP].ToString(), EmpNo, rowCount.ToString() });
                //    }
                //    else
                //        uploadEmpPos.RosterTableGroupID = 0;
                //}

                if (rawDataTable.Columns.Contains(FIELD_REMARK))
                {
                    //if (!row[FIELD_REMARK].ToString().Trim().Equals(string.Empty))
                    //{
                        uploadEmpPos.EmpPosRemark = row[FIELD_REMARK].ToString();
                    //}
                }

                //if (rawDataTable.Columns.Contains(FIELD_IS_ROSTER_TABLE_GROUP_SUPERVISOR))
                //{
                //    uploadEmpPos.EmpPosIsRosterTableGroupSupervisor = row[FIELD_IS_ROSTER_TABLE_GROUP_SUPERVISOR].ToString().Equals("Yes", StringComparison.CurrentCultureIgnoreCase) || row[FIELD_IS_ROSTER_TABLE_GROUP_SUPERVISOR].ToString().Equals("Y", StringComparison.CurrentCultureIgnoreCase);
                //}
                int iFieldCount = 1;

                while (row.Table.Columns[HIERARCHY_LEVEL_PREFIX + iFieldCount] != null)
                {
                    string HierarchyLevelColumnName = HIERARCHY_LEVEL_PREFIX + iFieldCount;
                    if (!row[HierarchyLevelColumnName].ToString().Trim().Equals(string.Empty))
                    {
                        int hierarchyLevelID = Parse.GetHierarchyLevelID(dbConn, iFieldCount);
                        if (hierarchyLevelID > 0)
                        {
                            EUploadEmpHierarchy uploadEmpHierarchy = new EUploadEmpHierarchy();
                            uploadEmpHierarchy.EmpID = uploadEmpPos.EmpID;
                            uploadEmpHierarchy.HLevelID = hierarchyLevelID;
                            if (!row[HierarchyLevelColumnName].ToString().Trim().Equals(string.Empty))
                            {
                                uploadEmpHierarchy.HElementID = Parse.GetHierarchyElementID(dbConn, row[HierarchyLevelColumnName].ToString(), uploadEmpPos.CompanyID, iFieldCount, CreateCodeIfNotExists, UserID);
                                if (uploadEmpHierarchy.HElementID <= 0)
                                    errors.addError(ImportErrorMessage.ERROR_INVALID_FIELD_VALUE, new string[] { HierarchyLevelColumnName + "=" + row[HierarchyLevelColumnName].ToString(), EmpNo, rowCount.ToString() });
                            }
                            else
                                uploadEmpHierarchy.HElementID = 0;

                            uploadHierarchyList.Add(uploadEmpHierarchy);
                        }
                    }
                    iFieldCount++;
                }


                uploadEmpPos.SessionID = m_SessionID;
                uploadEmpPos.TransactionDate = UploadDateTime;


                if (uploadEmpPos.EmpID != 0 && errors.List.Count <= 0)
                {
                    if (rawDataTable.Columns.Contains(FIELD_SYNC_ID))
                    {
                        if (!row.IsNull(FIELD_SYNC_ID))
                        {
                            string strSynID = row[FIELD_SYNC_ID].ToString();
                            uploadEmpPos.SynID = strSynID;
                            if (!string.IsNullOrEmpty(strSynID))
                            {
                                DBFilter synIDFilter = new DBFilter();
                                synIDFilter.add(new Match("SynID", strSynID));
                                ArrayList objSameSynIDList = EEmpPositionInfo.db.select(dbConn, synIDFilter);
                                if (objSameSynIDList.Count > 0)
                                    uploadEmpPos.EmpPosID = ((EEmpPositionInfo)objSameSynIDList[0]).EmpPosID;
                            }

                            if (uploadEmpPos.EmpPosID != 0)
                            {
                                DBFilter empHierarchyFilter = new DBFilter();
                                empHierarchyFilter.add(new Match("EmpPosID", uploadEmpPos.EmpPosID));
                                lastHierarchyList = EEmpHierarchy.db.select(dbConn, empHierarchyFilter);

                                foreach (EEmpHierarchy empHierarchy in lastHierarchyList)
                                {
                                    foreach (EUploadEmpHierarchy uploadEmpHierarchy in uploadHierarchyList)
                                    {
                                        if (uploadEmpHierarchy.HLevelID == empHierarchy.HLevelID)
                                        {
                                            if (uploadEmpHierarchy.HElementID != empHierarchy.HElementID)
                                            {
                                                uploadEmpHierarchy.EmpHierarchyID = empHierarchy.EmpHierarchyID;
                                            }
                                            else
                                            {
                                                uploadEmpHierarchy.EmpPosID = empHierarchy.EmpPosID;
                                                uploadEmpHierarchy.EmpHierarchyID = empHierarchy.EmpHierarchyID;
                                            }
                                            break;
                                        }
                                    }

                                }
                            }
                        }
                    }
                    if (uploadEmpPos.EmpPosID == 0)
                    {

                        lastEmpPos = AppUtils.GetLastPositionInfo(dbConn, uploadEmpPos.EmpPosEffFr, uploadEmpPos.EmpID);
                        if (lastEmpPos != null)
                        {
                            DBFilter empHierarchyFilter = new DBFilter();
                            empHierarchyFilter.add(new Match("EmpPosID", lastEmpPos.EmpPosID));
                            lastHierarchyList = EEmpHierarchy.db.select(dbConn, empHierarchyFilter);



                            bool IsSameHierarchy = true;
                            if (uploadEmpPos.CompanyID == lastEmpPos.CompanyID
                                && uploadEmpPos.LeavePlanID == lastEmpPos.LeavePlanID
                                && (!rawDataTable.Columns.Contains(FIELD_LEAVE_PLAN_IS_RESET_GRANT_DATE) || uploadEmpPos.EmpPosIsLeavePlanResetEffectiveDate == lastEmpPos.EmpPosIsLeavePlanResetEffectiveDate)
                                && uploadEmpPos.PayGroupID == lastEmpPos.PayGroupID
                                && uploadEmpPos.PositionID == lastEmpPos.PositionID
                                && uploadEmpPos.RankID == lastEmpPos.RankID
                                && uploadEmpPos.StaffTypeID == lastEmpPos.StaffTypeID
                                && (!rawDataTable.Columns.Contains(FIELD_EMPLOYMENT_TYPE) || uploadEmpPos.EmploymentTypeID == lastEmpPos.EmploymentTypeID)
                                && (!rawDataTable.Columns.Contains(FIELD_YEB_PLAN) || uploadEmpPos.YEBPlanID == lastEmpPos.YEBPlanID)
                                //&& (!rawDataTable.Columns.Contains(FIELD_ESS_1ST_AUTHORIZATION_GROUP) || uploadEmpPos.EmpFirstAuthorizationGp == lastEmpPos.EmpFirstAuthorizationGp)
                                //&& (!rawDataTable.Columns.Contains(FIELD_ESS_2ND_AUTHORIZATION_GROUP) || uploadEmpPos.EmpSecondAuthorizationGp == lastEmpPos.EmpSecondAuthorizationGp)
                                && (!rawDataTable.Columns.Contains(FIELD_ESS_AUTHORIZATION_WORKFLOW_LEAVEAPP) || uploadEmpPos.AuthorizationWorkFlowIDLeaveApp == lastEmpPos.AuthorizationWorkFlowIDLeaveApp)
                                && (!rawDataTable.Columns.Contains(FIELD_ESS_AUTHORIZATION_WORKFLOW_EEINFO) || uploadEmpPos.AuthorizationWorkFlowIDEmpInfoModified == lastEmpPos.AuthorizationWorkFlowIDEmpInfoModified)
                                // Start 0000060, Miranda, 2014-07-15
                                && (!rawDataTable.Columns.Contains(FIELD_ESS_AUTHORIZATION_WORKFLOW_EEOT) || uploadEmpPos.AuthorizationWorkFlowIDOTClaims == lastEmpPos.AuthorizationWorkFlowIDOTClaims)
                                // End 0000060, Miranda, 2014-07-15
                                // Start 0000112, Miranda, 2014-12-10
                                && (!rawDataTable.Columns.Contains(FIELD_ESS_AUTHORIZATION_WORKFLOW_LateWaive) || uploadEmpPos.AuthorizationWorkFlowIDLateWaive == lastEmpPos.AuthorizationWorkFlowIDLateWaive)
                                // End 0000112, Miranda, 2014-12-10
                                && (!rawDataTable.Columns.Contains(FIELD_ATTENDANCE_PLAN) || uploadEmpPos.AttendancePlanID == lastEmpPos.AttendancePlanID)
                                //&& (!rawDataTable.Columns.Contains(FIELD_DEFAULT_ROSTER_CODE) || uploadEmpPos.EmpPosDefaultRosterCodeID == lastEmpPos.EmpPosDefaultRosterCodeID)
                                && (!rawDataTable.Columns.Contains(FIELD_WORK_HOUR_PATTERN) || uploadEmpPos.WorkHourPatternID == lastEmpPos.WorkHourPatternID)
                                //&& (!rawDataTable.Columns.Contains(FIELD_ROSTER_TABLE_GROUP) || uploadEmpPos.RosterTableGroupID == lastEmpPos.RosterTableGroupID)
                                //&& (!rawDataTable.Columns.Contains(FIELD_IS_ROSTER_TABLE_GROUP_SUPERVISOR) || uploadEmpPos.EmpPosIsRosterTableGroupSupervisor == lastEmpPos.EmpPosIsRosterTableGroupSupervisor)
                                && lastHierarchyList.Count == uploadHierarchyList.Count
                                && uploadEmpPos.EmpPosEffFr == lastEmpPos.EmpPosEffFr
                                && uploadEmpPos.EmpPosEffTo == lastEmpPos.EmpPosEffTo
                                && uploadEmpPos.EmpPosRemark == lastEmpPos.EmpPosRemark
                                )
                                foreach (EEmpHierarchy empHierarchy in lastHierarchyList)
                                {
                                    foreach (EUploadEmpHierarchy uploadEmpHierarchy in uploadHierarchyList)
                                    {
                                        if (uploadEmpHierarchy.HLevelID == empHierarchy.HLevelID)
                                        {
                                            if (uploadEmpHierarchy.HElementID != empHierarchy.HElementID)
                                            {
                                                uploadEmpHierarchy.EmpHierarchyID = empHierarchy.EmpHierarchyID;
                                                IsSameHierarchy = false;
                                            }
                                            else
                                            {
                                                uploadEmpHierarchy.EmpPosID = empHierarchy.EmpPosID;
                                                uploadEmpHierarchy.EmpHierarchyID = empHierarchy.EmpHierarchyID;
                                            }
                                            break;
                                        }
                                        if (uploadHierarchyList[uploadHierarchyList.Count - 1] == uploadEmpHierarchy)
                                            IsSameHierarchy = false;
                                    }

                                }
                            else
                            {
                                IsSameHierarchy = false;
                            }

                            if (IsSameHierarchy)
                            {
                                continue;
                            }
                            else
                            {
                                // add postion terms with new ID
                                if (lastEmpPos.EmpPosEffFr.Equals(uploadEmpPos.EmpPosEffFr))
                                {
                                    uploadEmpPos.EmpPosID = lastEmpPos.EmpPosID;
                                    if (uploadEmpPos.EmpPosEffTo.Ticks == 0 && lastEmpPos.EmpPosEffTo.Ticks != 0)
                                    {
                                        EEmpPositionInfo afterEmpPos = (EEmpPositionInfo)AppUtils.GetLastObj(dbConn, uploadDB, "EmpPosEffFr", uploadEmpPos.EmpID, new Match("EmpPosEffFr", ">", lastEmpPos.EmpPosEffTo));
                                        if (afterEmpPos != null)
                                            uploadEmpPos.EmpPosEffTo = afterEmpPos.EmpPosEffFr.AddDays(-1);
                                    }
                                    foreach (EUploadEmpHierarchy uploadEmpHierarchy in uploadHierarchyList)
                                    {
                                        uploadEmpHierarchy.EmpPosID = uploadEmpPos.EmpPosID;
                                        //uploadEmpHierarchy.EmpHierarchyID = empHierarchy.EmpHierarchyID;
                                    }

                                }
                                else
                                {
                                    AND lastObjAndTerms = new AND();
                                    lastObjAndTerms.add(new Match("EmpPosEffFr", ">", uploadEmpPos.EmpPosEffFr));
                                    if (!uploadEmpPos.EmpPosEffTo.Ticks.Equals(0))
                                        lastObjAndTerms.add(new Match("EmpPosEffFr", "<=", uploadEmpPos.EmpPosEffTo));

                                    EEmpPositionInfo lastObj = (EEmpPositionInfo)AppUtils.GetLastObj(dbConn, uploadDB, "EmpPosEffFr", uploadEmpPos.EmpID, lastObjAndTerms);
                                    if (lastObj != null)
                                        if (!lastObj.EmpPosEffTo.Ticks.Equals(0))
                                        {
                                            errors.addError(ImportErrorMessage.ERROR_DATE_FROM_OVERLAP, new string[] { uploadEmpPos.EmpPosEffFr.ToString("yyyy-MM-dd"), rowCount.ToString() });
                                            continue;
                                        }
                                }
                            }
                        }
                    }
                }

                if (uploadEmpPos.EmpPosID <= 0)
                    uploadEmpPos.ImportActionStatus = ImportDBObject.ImportActionEnum.INSERT;
                else
                    uploadEmpPos.ImportActionStatus = ImportDBObject.ImportActionEnum.UPDATE;

                uploadEmpPos.UploadEmpID = ParseTemp.GetUploadEmpID(dbConn, EmpNo, m_SessionID);
                if (uploadEmpPos.UploadEmpID == 0)
                    if (uploadEmpPos.EmpID == 0)
                        errors.addError(ImportErrorMessage.ERROR_INVALID_EMP_NO, new string[] { EmpNo, rowCount.ToString() });
                    else
                        uploadEmpPos.UploadEmpID = ImportEmpPersonalInfoProcess.CreateDummyUploadEmployeeInfo(dbConn, uploadEmpPos.EmpID, m_SessionID, UploadDateTime);

                Hashtable values = new Hashtable();
                tempDB.populate(uploadEmpPos, values);
                PageErrors pageErrors = new PageErrors(EUploadEmpPersonalInfo.db);
                tempDB.validate(pageErrors, values);
                if (pageErrors.errors.Count == 0)
                {

                    tempDB.insert(dbConn, uploadEmpPos);
                    foreach (EUploadEmpHierarchy uploadHierarchy in uploadHierarchyList)
                    {
                        uploadHierarchy.UploadEmpPosID = uploadEmpPos.UploadEmpPosID;
                        tempHierarchyDB.insert(dbConn, uploadHierarchy);
                    }
                }
                else
                {
                    pageErrors.addError(rawDataTable.TableName);
                    throw new HRImportException(pageErrors.getPrompt() + "(line " +rowCount.ToString() + ")");

                    //if (EmpID == 0)
                    //    errors.addError(ImportErrorMessage.ERROR_INVALID_EMP_NO, new string[] { EmpNo, rowCount.ToString() });
                    //else if (PayCodeID == 0)
                    //    errors.addError(ImportErrorMessage.ERROR_INVALID_FIELD_VALUE, new string[] { FIELD_PAYMENT_CODE + "=" + PaymentCode, EmpNo, rowCount.ToString() });
                    //else if (PayCodeID == 0)
                    //    errors.addError(ImportErrorMessage.ERROR_INVALID_FIELD_VALUE, new string[] { FIELD_PAYMENT_CODE + "=" + PaymentCode, EmpNo, rowCount.ToString() });
                    //else if (EffDate.Ticks == 0)
                    //    errors.addError(ImportErrorMessage.ERROR_INVALID_FIELD_VALUE, new string[] { FIELD_EFFECTIVE_DATE + "=" + EffDateString, EmpNo, rowCount.ToString() });
                    //else if (double.TryParse(amountString, out amount))
                    //    errors.addError(ImportErrorMessage.ERROR_INVALID_FIELD_VALUE, new string[] { FIELD_AMOUNT + "=" + amountString, EmpNo, rowCount.ToString() });
                }

            }
            if (errors.List.Count > 0)
            {
                ClearTempTable();
                throw (new HRImportException(rawDataTable.TableName + "\r\n"+ errors.Message()));
            }
            return GetImportDataFromTempDatabase(null);
        }

        public override DataTable UploadToTempDatabase(string Filename, int UserID, string ZipPassword)
        {
            ClearTempTable();
            DataTable rawDataTable = HROne.Import.ExcelImport.parse(Filename, ZipPassword).Tables[TABLE_NAME];
            return UploadToTempDatabase(rawDataTable, UserID, true);
        }
        public override DataTable GetImportDataFromTempDatabase(ListInfo info)
        {
            DBFilter sessionFilter = new DBFilter();
            sessionFilter.add(new Match("SessionID", m_SessionID));
            if (info != null && info.orderby != null && !info.orderby.Equals(""))
                sessionFilter.add(info.orderby, info.order);
            DataTable table = sessionFilter.loadData(dbConn, info, " c.* ", " from " + tempDB.dbclass.tableName + " c ");


            return table;

        }

        public static void ClearTempTable(DatabaseConnection dbConn, string sessionID)
        {
            ImportEmpPositionInfoProcess import = new ImportEmpPositionInfoProcess(dbConn, sessionID);
            import.ClearTempTable();
        }

        public override void ClearTempTable()
        {
            //  Clear Old Import Session
            DBFilter sessionFilter = new DBFilter();
            if (!string.IsNullOrEmpty(m_SessionID))
                sessionFilter.add(new Match("SessionID", m_SessionID));
            ArrayList uploadEmpPosList = tempDB.select(dbConn, sessionFilter);
            foreach (EUploadEmpPositionInfo uploadEmpPos in uploadEmpPosList)
            {
                DBFilter uploadEmpPosFilter = new DBFilter();
                uploadEmpPosFilter.add(new Match("UploadEmpPosID", uploadEmpPos.UploadEmpPosID));
                tempHierarchyDB.delete(dbConn, uploadEmpPosFilter);
                tempDB.delete(dbConn, uploadEmpPos);

            }

        }

        public override void ImportToDatabase()
        {
            ImportToDatabase(0);
        }
        public void ImportToDatabase(int UploadEmpID)
        {
            DBFilter sessionFilter = new DBFilter();
            sessionFilter.add(new Match("SessionID", m_SessionID));
            if (UploadEmpID > 0)
                sessionFilter.add(new Match("UploadEmpID", UploadEmpID));
            ArrayList uploadEmpPosList = tempDB.select(dbConn, sessionFilter);
            foreach (EUploadEmpPositionInfo obj in uploadEmpPosList)
            {

                EEmpPositionInfo empPosInfo = new EEmpPositionInfo();

                if (obj.ImportActionStatus != ImportDBObject.ImportActionEnum.INSERT)
                {
                    empPosInfo.EmpPosID = obj.EmpPosID;
                    uploadDB.select(dbConn, empPosInfo);
                }

                obj.ExportToObject(empPosInfo);

                if (obj.ImportActionStatus == ImportDBObject.ImportActionEnum.INSERT)
                {
                    empPosInfo.EmpID = ParseTemp.GetEmpIDFromUploadEmpID(dbConn, obj.UploadEmpID);
                    DBFilter emplastPosFilter = new DBFilter();
                    EEmpPositionInfo lastObj = (EEmpPositionInfo)AppUtils.GetLastObj(dbConn, EEmpPositionInfo.db, "EmpPosEffFr", empPosInfo.EmpID, new Match("EmpPosEffFr", "<", empPosInfo.EmpPosEffFr));
                    if (lastObj != null)
                        if (lastObj.EmpPosEffTo.Ticks == 0)
                        {
                            lastObj.EmpPosEffTo = empPosInfo.EmpPosEffFr.AddDays(-1);
                            uploadDB.update(dbConn, lastObj);
                        }
                    uploadDB.insert(dbConn, empPosInfo);

                    DBFilter uploadEmpPosFilter = new DBFilter();
                    uploadEmpPosFilter.add(new Match("UploadEmpPosID", obj.UploadEmpPosID));
                    ArrayList uploadEmpHierarchyList = EUploadEmpHierarchy.db.select(dbConn, uploadEmpPosFilter);
                    foreach (EUploadEmpHierarchy uploadEmpHierarchy in uploadEmpHierarchyList)
                    {
                        uploadEmpHierarchy.EmpPosID = empPosInfo.EmpPosID;
                        uploadEmpHierarchy.EmpID = empPosInfo.EmpID;
                        EEmpHierarchy empHierarchy = new EEmpHierarchy();
                        ImportDBObject.CopyObjectProperties(uploadEmpHierarchy, empHierarchy);
                        uploadHierarchyDB.insert(dbConn, empHierarchy);
                        EUploadEmpHierarchy.db.delete(dbConn, uploadEmpHierarchy);
                    }
                }
                else if (obj.ImportActionStatus == ImportDBObject.ImportActionEnum.UPDATE)
                {
                    uploadDB.update(dbConn, empPosInfo);


                    DBFilter uploadEmpPosFilter = new DBFilter();
                    uploadEmpPosFilter.add(new Match("UploadEmpPosID", obj.UploadEmpPosID));
                    ArrayList uploadEmpHierarchyList = EUploadEmpHierarchy.db.select(dbConn, uploadEmpPosFilter);
                    foreach (EUploadEmpHierarchy uploadEmpHierarchy in uploadEmpHierarchyList)
                    {

                        EEmpHierarchy empHierarchy = new EEmpHierarchy();
                        ImportDBObject.CopyObjectProperties(uploadEmpHierarchy, empHierarchy);

                        DBFilter empHierarchyFilter = new DBFilter();
                        empHierarchyFilter.add(new Match("EmpPosID", empPosInfo.EmpPosID));
                        empHierarchyFilter.add(new Match("HLevelID", uploadEmpHierarchy.HLevelID));
                        ArrayList empHierarchyList = uploadHierarchyDB.select(dbConn, empHierarchyFilter);
                        if (empHierarchyList.Count > 0)
                        {
                            empHierarchy.EmpHierarchyID = ((EEmpHierarchy)empHierarchyList[0]).EmpHierarchyID;
                            uploadHierarchyDB.update(dbConn, empHierarchy);
                        }
                        else
                            uploadHierarchyDB.insert(dbConn, empHierarchy);
                        EUploadEmpHierarchy.db.delete(dbConn, uploadEmpHierarchy);
                    }
                }
                EUploadEmpPositionInfo.db.delete(dbConn, obj);
            }
        }

        public static DataTable Export(DatabaseConnection dbConn, ArrayList empList, bool IsIncludeCurrentPositionInfo, bool IsShowDescription)
        {
            return Export(dbConn, empList, IsIncludeCurrentPositionInfo, IsShowDescription, false, new DateTime());
        }
        public static DataTable Export(DatabaseConnection dbConn, ArrayList empList, bool IsIncludeCurrentPositionInfo, bool IsShowDescription, bool IsIncludeSyncID, DateTime ReferenceDateTime)
        {
            DataTable tmpDataTable = new DataTable(TABLE_NAME);
            //if (IsIncludeInternalID)
            //    tmpDataTable.Columns.Add(FIELD_INTERNAL_ID, typeof(string));
            tmpDataTable.Columns.Add(FIELD_EMP_NO, typeof(string));

            if (IsIncludeCurrentPositionInfo)
                ImportEmpPersonalInfoProcess.AddEmployeeInfoHeader(tmpDataTable);

            tmpDataTable.Columns.Add(FIELD_FROM, typeof(DateTime));
            tmpDataTable.Columns.Add(FIELD_TO, typeof(DateTime));
            tmpDataTable.Columns.Add(FIELD_COMPANY, typeof(String));
            tmpDataTable.Columns.Add(FIELD_POSITION, typeof(String));
            tmpDataTable.Columns.Add(FIELD_RANK, typeof(String));
            tmpDataTable.Columns.Add(FIELD_STAFF_TYPE, typeof(String));
            tmpDataTable.Columns.Add(FIELD_EMPLOYMENT_TYPE, typeof(String));
            tmpDataTable.Columns.Add(FIELD_PAYROLL_GROUP, typeof(String));
            tmpDataTable.Columns.Add(FIELD_LEAVE_PLAN, typeof(String));
            tmpDataTable.Columns.Add(FIELD_LEAVE_PLAN_IS_RESET_GRANT_DATE, typeof(String));
            
            tmpDataTable.Columns.Add(FIELD_YEB_PLAN, typeof(String));
            //tmpDataTable.Columns.Add(FIELD_ESS_1ST_AUTHORIZATION_GROUP, typeof(String));
            //tmpDataTable.Columns.Add(FIELD_ESS_2ND_AUTHORIZATION_GROUP, typeof(String));
            tmpDataTable.Columns.Add(FIELD_ESS_AUTHORIZATION_WORKFLOW_LEAVEAPP, typeof(String));
            tmpDataTable.Columns.Add(FIELD_ESS_AUTHORIZATION_WORKFLOW_EEINFO, typeof(String));
            // Start 0000060, Miranda, 2014-07-15
            tmpDataTable.Columns.Add(FIELD_ESS_AUTHORIZATION_WORKFLOW_EEOT, typeof(String));
            // End 0000060, Miranda, 2014-07-15
            // Start 0000112, Miranda, 2014-12-10
            tmpDataTable.Columns.Add(FIELD_ESS_AUTHORIZATION_WORKFLOW_LateWaive, typeof(String));
            // End 0000112, Miranda, 2014-12-10
            tmpDataTable.Columns.Add(FIELD_ATTENDANCE_PLAN, typeof(String));
            //tmpDataTable.Columns.Add(FIELD_DEFAULT_ROSTER_CODE, typeof(String));
            tmpDataTable.Columns.Add(FIELD_WORK_HOUR_PATTERN, typeof(String));
            tmpDataTable.Columns.Add(FIELD_ROSTER_TABLE_GROUP, typeof(String));
            tmpDataTable.Columns.Add(FIELD_IS_ROSTER_TABLE_GROUP_SUPERVISOR, typeof(String));
            tmpDataTable.Columns.Add(FIELD_REMARK, typeof(string));
            AddHierarchyLevelHeader(dbConn, tmpDataTable);

            if (IsIncludeSyncID)
                tmpDataTable.Columns.Add(FIELD_SYNC_ID, typeof(string));

            foreach (EEmpPersonalInfo empInfo in empList)
            {
                if (EEmpPersonalInfo.db.select(dbConn, empInfo))
                {
                    DBFilter filter = new DBFilter();
                    filter.add(new Match("EmpID", empInfo.EmpID));
                    filter.add(getCreateModifiedRecordsAfterDBTerm(ReferenceDateTime));
                    ArrayList list = EEmpPositionInfo.db.select(dbConn, filter);
                    foreach (EEmpPositionInfo empPositionInfo in list)
                    {
                        DataRow row = tmpDataTable.NewRow();
                        //if (IsIncludeInternalID)
                        //    row[FIELD_INTERNAL_ID] = ToHexDecWithCheckDigit(empPositionInfo.EmpPosID);
                        row[FIELD_EMP_NO] = empInfo.EmpNo;

                        if (IsIncludeCurrentPositionInfo)
                            ImportEmpPersonalInfoProcess.AddEmployeeInfo(dbConn, row, empInfo.EmpID);

                        row[FIELD_FROM] = empPositionInfo.EmpPosEffFr;
                        row[FIELD_TO] = empPositionInfo.EmpPosEffTo;

                        ECompany company = new ECompany();
                        company.CompanyID = empPositionInfo.CompanyID;
                        if (ECompany.db.select(dbConn, company))
                            row[FIELD_COMPANY] = IsShowDescription ? company.CompanyName : company.CompanyCode;

                        EPosition position = new EPosition();
                        position.PositionID = empPositionInfo.PositionID;
                        if (EPosition.db.select(dbConn, position))
                            row[FIELD_POSITION] = IsShowDescription ? position.PositionDesc : position.PositionCode;

                        ERank rank = new ERank();
                        rank.RankID = empPositionInfo.RankID;
                        if (ERank.db.select(dbConn, rank))
                            row[FIELD_RANK] = IsShowDescription ? rank.RankDesc : rank.RankCode;

                        EStaffType staffType = new EStaffType();
                        staffType.StaffTypeID = empPositionInfo.StaffTypeID;
                        if (EStaffType.db.select(dbConn, staffType))
                            row[FIELD_STAFF_TYPE] = IsShowDescription ? staffType.StaffTypeDesc : staffType.StaffTypeCode;

                        EEmploymentType employmentType = new EEmploymentType();
                        employmentType.EmploymentTypeID = empPositionInfo.EmploymentTypeID;
                        if (EEmploymentType.db.select(dbConn, employmentType))
                            row[FIELD_EMPLOYMENT_TYPE] = IsShowDescription ? employmentType.EmploymentTypeDesc : employmentType.EmploymentTypeCode;

                        EPayrollGroup payrollGroup = new EPayrollGroup();
                        payrollGroup.PayGroupID = empPositionInfo.PayGroupID;
                        if (EPayrollGroup.db.select(dbConn, payrollGroup))
                            row[FIELD_PAYROLL_GROUP] = IsShowDescription ? payrollGroup.PayGroupDesc : payrollGroup.PayGroupCode;

                        ELeavePlan leavePlan = new ELeavePlan();
                        leavePlan.LeavePlanID = empPositionInfo.LeavePlanID;
                        if (ELeavePlan.db.select(dbConn, leavePlan))
                            row[FIELD_LEAVE_PLAN] = IsShowDescription ? leavePlan.LeavePlanDesc : leavePlan.LeavePlanCode;

                        row[FIELD_LEAVE_PLAN_IS_RESET_GRANT_DATE] = empPositionInfo.EmpPosIsLeavePlanResetEffectiveDate ? "Yes" : "No";

                        EYEBPlan YEBPlan = new EYEBPlan();
                        YEBPlan.YEBPlanID = empPositionInfo.YEBPlanID;
                        if (EYEBPlan.db.select(dbConn, YEBPlan))
                            row[FIELD_YEB_PLAN] = IsShowDescription ? YEBPlan.YEBPlanDesc : YEBPlan.YEBPlanCode;

                        //EAuthorizationGroup AuthorizationGroup = new EAuthorizationGroup();
                        //AuthorizationGroup.AuthorizationGroupID = empPositionInfo.EmpFirstAuthorizationGp;
                        //if (EAuthorizationGroup.db.select(dbConn, AuthorizationGroup))
                        //    row[FIELD_ESS_1ST_AUTHORIZATION_GROUP] = IsShowDescription ? AuthorizationGroup.AuthorizationDesc : AuthorizationGroup.AuthorizationCode;

                        //AuthorizationGroup = new EAuthorizationGroup();
                        //AuthorizationGroup.AuthorizationGroupID = empPositionInfo.EmpSecondAuthorizationGp;
                        //if (EAuthorizationGroup.db.select(dbConn, AuthorizationGroup))
                        //    row[FIELD_ESS_2ND_AUTHORIZATION_GROUP] = IsShowDescription ? AuthorizationGroup.AuthorizationDesc : AuthorizationGroup.AuthorizationCode;

                        EAuthorizationWorkFlow AuthorizationWorkFlow = new EAuthorizationWorkFlow();
                        AuthorizationWorkFlow.AuthorizationWorkFlowID = empPositionInfo.AuthorizationWorkFlowIDLeaveApp;
                        if (EAuthorizationWorkFlow.db.select(dbConn, AuthorizationWorkFlow))
                            row[FIELD_ESS_AUTHORIZATION_WORKFLOW_LEAVEAPP] = IsShowDescription ? AuthorizationWorkFlow.AuthorizationWorkFlowDescription : AuthorizationWorkFlow.AuthorizationWorkFlowCode;

                        AuthorizationWorkFlow = new EAuthorizationWorkFlow();
                        AuthorizationWorkFlow.AuthorizationWorkFlowID = empPositionInfo.AuthorizationWorkFlowIDEmpInfoModified;
                        if (EAuthorizationWorkFlow.db.select(dbConn, AuthorizationWorkFlow))
                            row[FIELD_ESS_AUTHORIZATION_WORKFLOW_EEINFO] = IsShowDescription ? AuthorizationWorkFlow.AuthorizationWorkFlowDescription : AuthorizationWorkFlow.AuthorizationWorkFlowCode;
                        // Start 0000060, Miranda, 2014-07-15
                        AuthorizationWorkFlow = new EAuthorizationWorkFlow();
                        AuthorizationWorkFlow.AuthorizationWorkFlowID = empPositionInfo.AuthorizationWorkFlowIDOTClaims;
                        if (EAuthorizationWorkFlow.db.select(dbConn, AuthorizationWorkFlow))
                            row[FIELD_ESS_AUTHORIZATION_WORKFLOW_EEOT] = IsShowDescription ? AuthorizationWorkFlow.AuthorizationWorkFlowDescription : AuthorizationWorkFlow.AuthorizationWorkFlowCode;
                        // End 0000060, Miranda, 2014-07-15
                        // Start 0000112, Miranda, 2014-12-10
                        AuthorizationWorkFlow = new EAuthorizationWorkFlow();
                        AuthorizationWorkFlow.AuthorizationWorkFlowID = empPositionInfo.AuthorizationWorkFlowIDLateWaive;
                        if (EAuthorizationWorkFlow.db.select(dbConn, AuthorizationWorkFlow))
                            row[FIELD_ESS_AUTHORIZATION_WORKFLOW_LateWaive] = IsShowDescription ? AuthorizationWorkFlow.AuthorizationWorkFlowDescription : AuthorizationWorkFlow.AuthorizationWorkFlowCode;
                        // End 0000112, Miranda, 2014-12-10

                        EAttendancePlan AttendancePlan = new EAttendancePlan();
                        AttendancePlan.AttendancePlanID = empPositionInfo.AttendancePlanID;
                        if (EAttendancePlan.db.select(dbConn, AttendancePlan))
                            row[FIELD_ATTENDANCE_PLAN] = IsShowDescription ? AttendancePlan.AttendancePlanDesc : AttendancePlan.AttendancePlanCode;

                        //ERosterCode RosterCode = new ERosterCode();
                        //RosterCode.RosterCodeID = empPositionInfo.EmpPosDefaultRosterCodeID;
                        //if (ERosterCode.db.select(dbConn, RosterCode))
                        //    row[FIELD_DEFAULT_ROSTER_CODE] = RosterCode.RosterCode;

                        EWorkHourPattern WorkHourPattern = new EWorkHourPattern();
                        WorkHourPattern.WorkHourPatternID = empPositionInfo.WorkHourPatternID;
                        if (EWorkHourPattern.db.select(dbConn, WorkHourPattern))
                            row[FIELD_WORK_HOUR_PATTERN] = IsShowDescription ? WorkHourPattern.WorkHourPatternDesc : WorkHourPattern.WorkHourPatternCode;

                        //ERosterTableGroup RosterTableGroup = new ERosterTableGroup();
                        //RosterTableGroup.RosterTableGroupID = empPositionInfo.RosterTableGroupID;
                        //if (ERosterTableGroup.db.select(dbConn, RosterTableGroup))
                        //    row[FIELD_ROSTER_TABLE_GROUP] = IsShowDescription ? RosterTableGroup.RosterTableGroupDesc : RosterTableGroup.RosterTableGroupCode;

                        //row[FIELD_IS_ROSTER_TABLE_GROUP_SUPERVISOR] = empPositionInfo.EmpPosIsRosterTableGroupSupervisor ? "Yes" : "No";

                        row[FIELD_REMARK] = empPositionInfo.EmpPosRemark;
                        AddHierarchyInfo(dbConn, row, empPositionInfo.EmpPosID, IsShowDescription);

                        if (IsIncludeSyncID)
                            row[FIELD_SYNC_ID] = empPositionInfo.SynID;

                        tmpDataTable.Rows.Add(row);
                    }
                }

            }

            return tmpDataTable;
        }

        public static void AddEmployeePositionInfoHeader(DatabaseConnection dbConn, DataTable dataTable)
        {
            dataTable.Columns.Add(FIELD_COMPANY, typeof(String));
            AddHierarchyLevelHeader(dbConn, dataTable);
            dataTable.Columns.Add(FIELD_POSITION, typeof(String));
        }

        public static void AddEmployeePositionInfo(DatabaseConnection dbConn, DataRow row, int EmpID)
        {
            EEmpPositionInfo empPositionInfo = AppUtils.GetLastPositionInfo(dbConn, AppUtils.ServerDateTime().Date, EmpID);
            if (empPositionInfo != null)
            {
                ECompany company = new ECompany();
                company.CompanyID = empPositionInfo.CompanyID;
                if (ECompany.db.select(dbConn, company))
                    row[FIELD_COMPANY] = company.CompanyName;

                AddHierarchyInfo(dbConn, row, empPositionInfo.EmpPosID, true);

                EPosition position = new EPosition();
                position.PositionID = empPositionInfo.PositionID;
                if (EPosition.db.select(dbConn, position))
                    row[FIELD_POSITION] = position.PositionDesc;
            }
        }


        public static void AddHierarchyLevelHeader(DatabaseConnection dbConn, DataTable dataTable)
        {
            DBFilter hierarchyLevelFilter = new DBFilter();
            hierarchyLevelFilter.add("HLevelSeqNo", true);
            ArrayList hierarcyLevelList = EHierarchyLevel.db.select(dbConn, hierarchyLevelFilter);
            foreach (EHierarchyLevel hierarchyLevel in hierarcyLevelList)
                dataTable.Columns.Add(HIERARCHY_LEVEL_PREFIX + hierarchyLevel.HLevelSeqNo, typeof(String));

        }

        public static void AddHierarchyInfo(DatabaseConnection dbConn, DataRow row, int EmpPosID, bool IsShowDescription)
        {
            DBFilter empHierarchyFilter = new DBFilter();
            empHierarchyFilter.add(new Match("EmpPosID", EmpPosID));
            ArrayList hierarchyElementList = EEmpHierarchy.db.select(dbConn, empHierarchyFilter);
            foreach (EEmpHierarchy empHierarchy in hierarchyElementList)
            {
                EHierarchyLevel level = new EHierarchyLevel();
                level.HLevelID = empHierarchy.HLevelID;
                if (EHierarchyLevel.db.select(dbConn, level))
                {
                    EHierarchyElement element = new EHierarchyElement();
                    element.HElementID = empHierarchy.HElementID;
                    EHierarchyElement.db.select(dbConn, element);

                    row[HIERARCHY_LEVEL_PREFIX + level.HLevelSeqNo] = IsShowDescription ? element.HElementDesc : element.HElementCode;
                }

            }
        }

        public static void RetriveHierarchyLevelHeader(DatabaseConnection dbConn, DataTable dataTable)
        {
            DBFilter hierarchyLevelFilter = new DBFilter();
            hierarchyLevelFilter.add("HLevelSeqNo", true);
            ArrayList hierarcyLevelList = EHierarchyLevel.db.select(dbConn, hierarchyLevelFilter);
            foreach (EHierarchyLevel hierarchyLevel in hierarcyLevelList)
            {
                if (!dataTable.Columns.Contains(hierarchyLevel.HLevelDesc))
                    dataTable.Columns[HIERARCHY_LEVEL_PREFIX + hierarchyLevel.HLevelSeqNo].ColumnName = hierarchyLevel.HLevelDesc;
                else
                    dataTable.Columns[HIERARCHY_LEVEL_PREFIX + hierarchyLevel.HLevelSeqNo].ColumnName = hierarchyLevel.HLevelDesc + "(Level " + hierarchyLevel.HLevelSeqNo + ")";

            }
        }
    }
}
