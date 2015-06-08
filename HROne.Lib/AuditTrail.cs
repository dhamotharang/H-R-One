using System;
using System.Web.SessionState;
using System.Collections;
using System.Text;
using HROne.Lib.Entities;
using HROne.DataAccess;

namespace HROne.Common
{
    
    public class AuditTrail
    {
        private static EAuditTrail PrepareAuditTrail(DatabaseConnectionWithAudit dbConnWithAudit, int UserID, string FunctionCode, bool LogDetail)
        {
            DBFilter systemFunctionFilter = new DBFilter();
            systemFunctionFilter.add(new Match("FunctionCode", FunctionCode));
            ArrayList systemFunctionList = ESystemFunction.db.select(dbConnWithAudit, systemFunctionFilter);
            if (systemFunctionList.Count > 0)
            {


                //EUser user = WebUtils.GetCurUser(session);
                EAuditTrail t = new EAuditTrail(LogDetail);
                t.UserID = UserID;
                t.FunctionID = ((ESystemFunction)systemFunctionList[0]).FunctionID;
                t.CreateDate = AppUtils.ServerDateTime();
                dbConnWithAudit.CurAuditTrail = t;
                return t;
            }
            else
                return null;
        }
        public static EAuditTrail StartFunction(DatabaseConnection dbConn, EUser user, string FunctionCode, bool LogDetail)
        {
            return StartFunction(dbConn, user.UserID, FunctionCode, 0, LogDetail);
        }
        public static EAuditTrail StartFunction(DatabaseConnection dbConn, int UserID, string FunctionCode, int EmpID, bool LogDetail)
        {
            if (dbConn is DatabaseConnectionWithAudit)
            {
                EAuditTrail t = PrepareAuditTrail((DatabaseConnectionWithAudit)dbConn, UserID, FunctionCode, LogDetail);
                t.EmpID = EmpID;
                //EAuditTrail.db.insert(dbConn, t);
                //if (!LogDetail)
                //    CurAuditTrail = null;
                return t;
            }
            return null;
        }
        public static EAuditTrail StartChildFunction(DatabaseConnection dbConn, int EmpID)
        {
            if (dbConn is DatabaseConnectionWithAudit)
            {
                DatabaseConnectionWithAudit dbConnWithAudit = (DatabaseConnectionWithAudit)dbConn;
                if (dbConnWithAudit.CurAuditTrail != null)
                {
                    if (dbConnWithAudit.CurAuditTrail.AuditTrailID <= 0)
                        EAuditTrail.db.insert(dbConn, dbConnWithAudit.CurAuditTrail);
                    dbConnWithAudit.CurAuditTrail = dbConnWithAudit.CurAuditTrail.CreateChildAuditTrail();
                    dbConnWithAudit.CurAuditTrail.EmpID = EmpID;
                    return dbConnWithAudit.CurAuditTrail;
                }
            }
            return null;
        }
        public static EAuditTrail StartChildFunction(DatabaseConnection dbConn, string FunctionCode, int EmpID)
        {
            if (dbConn is DatabaseConnectionWithAudit)
            {
                DatabaseConnectionWithAudit dbConnWithAudit = (DatabaseConnectionWithAudit)dbConn;
                if (dbConnWithAudit.CurAuditTrail != null)
                {
                    if (dbConnWithAudit.CurAuditTrail.AuditTrailID <= 0)
                        EAuditTrail.db.insert(dbConn, dbConnWithAudit.CurAuditTrail);
                    dbConnWithAudit.CurAuditTrail = dbConnWithAudit.CurAuditTrail.CreateChildAuditTrail();
                    dbConnWithAudit.CurAuditTrail.EmpID = EmpID;
                    DBFilter systemFunctionFilter = new DBFilter();
                    systemFunctionFilter.add(new Match("FunctionCode", FunctionCode));
                    ArrayList systemFunctionList = ESystemFunction.db.select(dbConn, systemFunctionFilter);
                    if (systemFunctionList.Count > 0)
                    {
                        dbConnWithAudit.CurAuditTrail.FunctionID = ((ESystemFunction)systemFunctionList[0]).FunctionID;
                    }
                    return dbConnWithAudit.CurAuditTrail;
                }
            }
            return null;
        }
        public static void EndChildFunction(DatabaseConnection dbConn)
        {
            if (dbConn is DatabaseConnectionWithAudit)
            {
                DatabaseConnectionWithAudit dbConnWithAudit = (DatabaseConnectionWithAudit)dbConn;
                if (dbConnWithAudit.CurAuditTrail != null)
                    dbConnWithAudit.CurAuditTrail = dbConnWithAudit.CurAuditTrail.GetParentAuditTrail(dbConn);
            }
        }
        public static EAuditTrail StartFunction(DatabaseConnectionWithAudit dbConn, EUser user, string FunctionCode, int EmpID, bool LogDetail)
        {
            return StartFunction(dbConn, user.UserID, FunctionCode, EmpID, LogDetail);

        }
        public static void EndFunction(DatabaseConnection dbConn)
        {
            if (dbConn is DatabaseConnectionWithAudit)
            {
                DatabaseConnectionWithAudit dbConnWithAudit = (DatabaseConnectionWithAudit)dbConn;
                dbConnWithAudit.CurAuditTrail = null;
            }
        }

    }
}
