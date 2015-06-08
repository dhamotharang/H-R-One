using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using HROne.DataAccess;
using HROne.Lib.Entities;

namespace HROne.Lib
{
    public sealed class AuditTrailReportProcess
    {
        private DatabaseConnection dbConn;
        private string SelectedFunctionList;
        private string SelectedUserList;
        private DateTime PeriodFrom;
        private DateTime PeriodTo;
        private string EmpNo;
        private bool IsShowHeaderOnly;
        private bool IsShowKeyIDOnly;
        private bool IsDoNotConvertID;
        private bool IsShowWithoutDataUpdate;
        private System.IO.TextWriter writer;
        public EventHandler Updated;
        private bool IsClosed = false;

        public AuditTrailReportProcess(DatabaseConnection dbConn, System.IO.TextWriter writer, string SelectedFunctionList, string SelectedUserList, DateTime PeriodFrom, DateTime PeriodTo, string EmpNo, bool IsShowHeaderOnly, bool IsShowKeyIDOnly, bool IsDoNotConvertID, bool IsShowWithoutDataUpdate)
        {
            this.dbConn = dbConn.createClone();
            this.writer = writer;
            this.SelectedFunctionList = SelectedFunctionList;
            this.SelectedUserList = SelectedUserList;
            this.PeriodFrom = PeriodFrom;
            this.PeriodTo = PeriodTo;
            this.EmpNo = EmpNo;
            this.IsShowHeaderOnly = IsShowHeaderOnly;
            this.IsShowKeyIDOnly = IsShowKeyIDOnly;
            this.IsDoNotConvertID = IsDoNotConvertID;
            this.IsShowWithoutDataUpdate = IsShowWithoutDataUpdate;
        }

        public void Close()
        {
            IsClosed = true;
        }

        public string GenerateToFile()
        {
            string exportFileName = string.Empty;
            if (writer == null)
            {
                exportFileName = System.IO.Path.GetTempFileName();
                writer = System.IO.File.CreateText(exportFileName);
            }

            //OR orFunctionID = null;
            //foreach (ESystemFunction function in SelectedFunctionList)
            //{
            //    if (orFunctionID == null)
            //        orFunctionID = new OR();
            //    orFunctionID.add(new Match("FunctionID", function.FunctionID));
            //}
            //OR orUserID = null;
            //foreach (EUser selectedUser in SelectedUserList)
            //{
            //    if (orUserID == null)
            //        orUserID = new OR();
            //    orUserID.add(new Match("UserID", selectedUser.UserID));
            //}
            DBFilter filter = new DBFilter();
            if (!PeriodFrom.Ticks.Equals(0))
                filter.add(new Match("CreateDate", ">=", PeriodFrom));
            if (!PeriodTo.Ticks.Equals(0))
                filter.add(new Match("CreateDate", "<", PeriodTo.AddDays(1)));

            //if (orFunctionID != null)
            //    filter.add(orFunctionID);

            //if (orUserID != null)
            //    filter.add(orUserID);

            if (!string.IsNullOrEmpty(SelectedFunctionList))
                filter.add(new IN("FunctionID", SelectedFunctionList, null));

            if (!string.IsNullOrEmpty(SelectedUserList))
                filter.add(new IN("UserID", SelectedUserList, null));

            if (!IsShowWithoutDataUpdate)
            {
                filter.add(new IN("AuditTrailID", "select distinct AuditTrailID from " + EAuditTrailDetail.db.dbclass.tableName, new DBFilter()));
            }
            //if (!string.IsNullOrEmpty(EmpNo.Text))
            //{
            //    OR orEmpNoTerm = new OR();

            //    DBFilter empNoFilter = new DBFilter();
            //    empNoFilter.add(new Match("e.EmpNo"," like ","%"+EmpNo.Text.Trim() +"%"));
            //    orEmpNoTerm.add(new IN("EmpID", "Select EmpID from " + EEmpPersonalInfo.db.dbclass.tableName + " e ", empNoFilter));
            //    //orEmpNoTerm.add(new Match("EmpID", 0));
            //    filter.add(orEmpNoTerm);
            //}
            filter.add("CreateDate", true);

            ArrayList auditTrailList = EAuditTrail.db.select(dbConn, filter);


            try
            {
                foreach (EAuditTrail auditTrail in auditTrailList)
                {
                    if (IsClosed)
                        break;
                    string logText = auditTrail.GetLogText(dbConn, EmpNo, IsShowHeaderOnly, IsShowKeyIDOnly, IsDoNotConvertID);

                    if (!string.IsNullOrEmpty(logText))
                        writer.WriteLine(logText);

                    writer.Flush();
                    if (Updated != null)
                        Updated(this, new EventArgs());
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            writer.Close();
            return exportFileName;
        }
    }
}
