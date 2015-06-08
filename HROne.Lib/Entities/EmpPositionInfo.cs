using System;
using System.Collections;
using System.Text;
using HROne.DataAccess;
////using perspectivemind.validation;

namespace HROne.Lib.Entities
{
    [DBClass("EmpPositionInfo")]
    public class EEmpPositionInfo : BaseObjectWithRecordInfo
    {
        public static DBManager db = new DBManagerWithRecordInfo(typeof(EEmpPositionInfo));
        public static EEmpPositionInfo GetObject(DatabaseConnection dbConn, object ID)
        {
            if (ID is int)
            {
                EEmpPositionInfo obj = new EEmpPositionInfo();
                obj.EmpPosID = (int)ID;
                if (db.select(dbConn, obj))
                    return obj;
            }
            return null;
        }

        protected int m_EmpPosID;
        [DBField("EmpPosID", true, true), TextSearch, Export(false)]
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
        // Start 0000060, Miranda, 2014-07-13
        protected int m_AuthorizationWorkFlowIDOTClaims;
        [DBField("AuthorizationWorkFlowIDOTClaims"), TextSearch, Export(false)]
        public int AuthorizationWorkFlowIDOTClaims
        {
            get { return m_AuthorizationWorkFlowIDOTClaims; }
            set { m_AuthorizationWorkFlowIDOTClaims = value; modify("AuthorizationWorkFlowIDOTClaims"); }
        }
        // End 0000060, Miranda, 2014-07-13
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

        public string GetBusinessHierarchyString(DatabaseConnection dbConn)
        {
            return GetBusinessHierarchyString(dbConn, " / ");
        }
        public string GetBusinessHierarchyString(DatabaseConnection dbConn, string Delimiter)
        {
            DBFilter hLevelFilter = new DBFilter();
            hLevelFilter.add("HLevelSeqNo", true);
            string BusinessHierarchy = string.Empty;
            ArrayList HierarchyLevelList = EHierarchyLevel.db.select(dbConn, hLevelFilter);
            foreach (EHierarchyLevel hLevel in HierarchyLevelList)
            {

                DBFilter empHierarchyFilter = new DBFilter();
                empHierarchyFilter.add(new Match("EmpPosID", EmpPosID));
                empHierarchyFilter.add(new Match("HLevelID", hLevel.HLevelID));

                ArrayList empHierarchyList = EEmpHierarchy.db.select(dbConn, empHierarchyFilter);

                string currentElement = string.Empty;
                if (empHierarchyList.Count > 0)
                {
                    EEmpHierarchy empHierarchy = (EEmpHierarchy)empHierarchyList[0];

                    EHierarchyElement hElement = new EHierarchyElement();
                    hElement.HElementID = empHierarchy.HElementID;
                    if (EHierarchyElement.db.select(dbConn, hElement))

                        currentElement = hElement.HElementCode;
                }

                if (string.IsNullOrEmpty(BusinessHierarchy))
                    BusinessHierarchy = currentElement;
                else
                    BusinessHierarchy += Delimiter + currentElement;
            }
            return BusinessHierarchy;
        }

        public static DBFilter CreateDateRangeDBFilter(string SQLTableName, DateTime PeriodFrom, DateTime PeriodTo)
        {
            DBFilter empPosFilter = new DBFilter();

            DBFilterAddDateRange(empPosFilter, SQLTableName, PeriodFrom, PeriodTo); 

            return empPosFilter;
        }

        public static DBFilter DBFilterAddDateRange(DBFilter LastPositionDBFilter, string SQLTableAlias, DateTime PeriodFrom, DateTime PeriodTo)
        {
            if (!string.IsNullOrEmpty(SQLTableAlias))
                if (!SQLTableAlias.EndsWith("."))
                    SQLTableAlias += ".";
            if (!PeriodTo.Ticks.Equals(0))
            {
                OR orEmpPosEffFr = new OR();
                orEmpPosEffFr.add(new Match(SQLTableAlias + "EmpPosEffFr", "<=", PeriodTo));
                orEmpPosEffFr.add(new NullTerm(SQLTableAlias + "EmpPosEffFr"));
                LastPositionDBFilter.add(orEmpPosEffFr);
            }
            if (!PeriodFrom.Ticks.Equals(0))
            {
                OR orEmpPosEffTo = new OR();
                orEmpPosEffTo.add(new Match(SQLTableAlias + "EmpPosEffTo", ">=", PeriodFrom));
                orEmpPosEffTo.add(new NullTerm(SQLTableAlias + "EmpPosEffTo"));
                LastPositionDBFilter.add(orEmpPosEffTo);
            }
            return LastPositionDBFilter;
        }

    }
}
