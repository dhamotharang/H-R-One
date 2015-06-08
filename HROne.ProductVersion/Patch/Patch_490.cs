using System;
using System.Collections;
using System.Collections.Generic;
using HROne.DataAccess;
using System.Data.Common;
using System.Data;

namespace HROne.ProductVersion.Patch
{
    public class Patch_490
    {
        [DBClass("AuthorizationWorkFlow")]
        protected class EDummyAuthorizationWorkFlow : BaseObject
        {
            public static DBManager db = new DBManager(typeof(EDummyAuthorizationWorkFlow));

            protected int m_AuthorizationWorkFlowID;
            [DBField("AuthorizationWorkFlowID", true, true), TextSearch, Export(false)]
            public int AuthorizationWorkFlowID
            {
                get { return m_AuthorizationWorkFlowID; }
                set { m_AuthorizationWorkFlowID = value; modify("AuthorizationWorkFlowID"); }
            }
            protected string m_AuthorizationWorkFlowCode;
            [DBField("AuthorizationWorkFlowCode"), TextSearch, MaxLength(20, 20), Export(false), Required]
            public string AuthorizationWorkFlowCode
            {
                get { return m_AuthorizationWorkFlowCode; }
                set { m_AuthorizationWorkFlowCode = value; modify("AuthorizationWorkFlowCode"); }
            }
            protected string m_AuthorizationWorkFlowDescription;
            [DBField("AuthorizationWorkFlowDescription"), TextSearch, MaxLength(100, 100), Export(false), Required]
            public string AuthorizationWorkFlowDescription
            {
                get { return m_AuthorizationWorkFlowDescription; }
                set { m_AuthorizationWorkFlowDescription = value; modify("AuthorizationWorkFlowDescription"); }
            }

        }

        [DBClass("AuthorizationWorkFlowDetail")]
        protected class EDummyAuthorizationWorkFlowDetail : BaseObject
        {
            public static DBManager db = new DBManager(typeof(EDummyAuthorizationWorkFlowDetail));

            protected int m_AuthorizationWorkFlowDetailID;
            [DBField("AuthorizationWorkFlowDetailID", true, true), TextSearch, Export(false)]
            public int AuthorizationWorkFlowDetailID
            {
                get { return m_AuthorizationWorkFlowDetailID; }
                set { m_AuthorizationWorkFlowDetailID = value; modify("AuthorizationWorkFlowDetailID"); }
            }
            protected int m_AuthorizationWorkFlowID;
            [DBField("AuthorizationWorkFlowID"), TextSearch, Export(false), Required]
            public int AuthorizationWorkFlowID
            {
                get { return m_AuthorizationWorkFlowID; }
                set { m_AuthorizationWorkFlowID = value; modify("AuthorizationWorkFlowID"); }
            }
            protected int m_AuthorizationWorkFlowIndex;
            [DBField("AuthorizationWorkFlowIndex"), TextSearch, Export(false), Required]
            public int AuthorizationWorkFlowIndex
            {
                get { return m_AuthorizationWorkFlowIndex; }
                set { m_AuthorizationWorkFlowIndex = value; modify("AuthorizationWorkFlowIndex"); }
            }
            protected int m_AuthorizationGroupID;
            [DBField("AuthorizationGroupID"), TextSearch, Export(false), Required]
            public int AuthorizationGroupID
            {
                get { return m_AuthorizationGroupID; }
                set { m_AuthorizationGroupID = value; modify("AuthorizationGroupID"); }
            }
        }

        public static bool DBPatch(DatabaseConnection dbConn)
        {
            CreatedWorkflowCount=0;

            System.Data.Common.DbCommand command = dbConn.CreateCommand();

            command.CommandText = "UPDATE EmpPositionInfo SET EmpFirstAuthorizationGp=0 WHERE EmpFirstAuthorizationGp IS NULL";
            dbConn.ExecuteNonQuery(command);
            command.CommandText = "UPDATE EmpPositionInfo SET EmpSecondAuthorizationGp=0 WHERE EmpSecondAuthorizationGp IS NULL";
            dbConn.ExecuteNonQuery(command);

            command.CommandText = "SELECT DISTINCT EmpFirstAuthorizationGp, EmpSecondAuthorizationGp FROM EmpPositionInfo ";
            DataTable table = dbConn.ExecuteToDataTable(command);

            foreach (DataRow row in table.Rows)
            {
                List<int> EEGroupIDList = new List<int>();
                List<int> LeaveGroupIDList = new List<int>();
                foreach (DataColumn column in table.Columns)
                {
                    int authorizationGroupID = Convert.ToInt32(row[column]);
                    command.CommandText = "SELECT * FROM AuthorizationGroup WHERE AuthorizationGroupID=" + authorizationGroupID;
                    DataTable authorizationGroupTable = dbConn.ExecuteToDataTable(command);
                    if (authorizationGroupTable.Rows.Count > 0)
                    {
                        DataRow authorizationGroupRow = authorizationGroupTable.Rows[0];
                        if (Convert.ToInt32(authorizationGroupRow["AuthorizationGroupIsApproveLeave"]) != 0)
                            LeaveGroupIDList.Add(authorizationGroupID);
                        if (Convert.ToInt32(authorizationGroupRow["AuthorizationGroupIsApproveEEInfo"]) != 0)
                            EEGroupIDList.Add(authorizationGroupID);
                    }

                }
                {
                    int leaveWorkflowID = GetOrCreateWorkflow(dbConn, LeaveGroupIDList);
                    if (leaveWorkflowID > 0)
                    {
                        command.CommandText = @"UPDATE EmpPositionInfo 
                                        SET AuthorizationWorkFlowIDLeaveApp=" + leaveWorkflowID +
                                            @" WHERE EmpFirstAuthorizationGp=" + row["EmpFirstAuthorizationGp"].ToString() +
                                            @" AND EmpSecondAuthorizationGp=" + row["EmpSecondAuthorizationGp"].ToString();
                        dbConn.ExecuteNonQuery(command);
                    }
                }
                {
                    int EEWorkflowID = GetOrCreateWorkflow(dbConn, EEGroupIDList);
                    if (EEWorkflowID > 0)
                    {
                        command.CommandText = @"UPDATE EmpPositionInfo 
                                        SET AuthorizationWorkFlowIDEmpInfoModified=" + EEWorkflowID +
                                            @" WHERE EmpFirstAuthorizationGp=" + row["EmpFirstAuthorizationGp"].ToString() +
                                            @" AND EmpSecondAuthorizationGp=" + row["EmpSecondAuthorizationGp"].ToString();
                        dbConn.ExecuteNonQuery(command);
                    }
                }
            }

            return true;
        }
        protected static int CreatedWorkflowCount = 0;
        protected static int GetOrCreateWorkflow(DatabaseConnection dbConn, List<int> GroupIDList)
        {
            if (GroupIDList.Count > 0)
            {
                DBFilter filter = new DBFilter();
                for (int idx = 0; idx < GroupIDList.Count; idx++)
                {
                    int AuthorizationGroupID = GroupIDList[idx];
                    DBFilter inDetailFilter = new DBFilter();
                    inDetailFilter.add(new Match("AuthorizationGroupID", AuthorizationGroupID));
                    inDetailFilter.add(new Match("AuthorizationWorkFlowIndex", (idx + 1)));

                    filter.add(new IN("AuthorizationWorkFlowID", "SELECT AuthorizationWorkFlowID FROM " + EDummyAuthorizationWorkFlowDetail.db.dbclass.tableName, inDetailFilter));

                }

                DBFilter notInDetailFilter = new DBFilter();
                notInDetailFilter.add(new Match("AuthorizationWorkFlowIndex", ">", GroupIDList.Count));

                filter.add(new IN("NOT AuthorizationWorkFlowID", "SELECT AuthorizationWorkFlowID FROM " + EDummyAuthorizationWorkFlowDetail.db.dbclass.tableName, notInDetailFilter));
                ArrayList authorizationWorkFlowList = EDummyAuthorizationWorkFlow.db.select(dbConn, filter);
                EDummyAuthorizationWorkFlow availableWorkFlow = null;
                if (authorizationWorkFlowList.Count > 0)
                    availableWorkFlow = (EDummyAuthorizationWorkFlow)authorizationWorkFlowList[0];
                else
                {
                    CreatedWorkflowCount++;
                    availableWorkFlow = new EDummyAuthorizationWorkFlow();
                    availableWorkFlow.AuthorizationWorkFlowCode = "WORKFLOW_" + CreatedWorkflowCount;
                    availableWorkFlow.AuthorizationWorkFlowDescription = "Work Flow " + CreatedWorkflowCount;
                    EDummyAuthorizationWorkFlow.db.insert(dbConn, availableWorkFlow);

                    for (int idx = 0; idx < GroupIDList.Count; idx++)
                    {
                        int AuthorizationGroupID = GroupIDList[idx];
                        EDummyAuthorizationWorkFlowDetail detail = new EDummyAuthorizationWorkFlowDetail();
                        detail.AuthorizationWorkFlowID = availableWorkFlow.AuthorizationWorkFlowID;
                        detail.AuthorizationGroupID = AuthorizationGroupID;
                        detail.AuthorizationWorkFlowIndex = idx + 1;

                        EDummyAuthorizationWorkFlowDetail.db.insert(dbConn, detail);
                    }
                }
                return availableWorkFlow.AuthorizationWorkFlowID;
            }
            else
                return 0;
        }

    }
}