using System;
using System.Collections.Generic;
using System.Text;
using HROne.DataAccess;
//using perspectivemind.validation;
using System.Data;
using HROne.Lib.Entities;
using System.Globalization;
using System.Collections;

namespace HROne.Import
{


    public class ImportLeaveBalanceAdjustmentProcess : ImportProcessInteface
    {
        [DBClass("UploadLeaveBalanceAdjustment")]
        protected class EUploadLeaveBalanceAdjustment : ImportDBObject
        {
            public static DBManager db = new DBManager(typeof(EUploadLeaveBalanceAdjustment));

            protected int m_UploadLeaveBalAdjID;
            [DBField("UploadLeaveBalAdjID", true, true), TextSearch, Export(false)]
            public int UploadLeaveBalAdjID
            {
                get { return m_UploadLeaveBalAdjID; }
                set { m_UploadLeaveBalAdjID = value; modify("UploadLeaveBalAdjID"); }
            }
            protected int m_UploadEmpID;
            [DBField("UploadEmpID"), TextSearch, Export(false)]
            public int UploadEmpID
            {
                get { return m_UploadEmpID; }
                set { m_UploadEmpID = value; modify("UploadEmpID"); }
            }

            protected int m_LeaveBalAdjID;
            [DBField("LeaveBalAdjID"), TextSearch, Export(false)]
            public int LeaveBalAdjID
            {
                get { return m_LeaveBalAdjID; }
                set { m_LeaveBalAdjID = value; modify("LeaveBalAdjID"); }
            }
            protected int m_EmpID;
            [DBField("EmpID"), TextSearch, Export(false), Required]
            public int EmpID
            {
                get { return m_EmpID; }
                set { m_EmpID = value; modify("EmpID"); }
            }
            protected DateTime m_LeaveBalAdjDate;
            [DBField("LeaveBalAdjDate"), TextSearch, Export(false), Required]
            public DateTime LeaveBalAdjDate
            {
                get { return m_LeaveBalAdjDate; }
                set { m_LeaveBalAdjDate = value; modify("LeaveBalAdjDate"); }
            }
            protected int m_LeaveTypeID;
            [DBField("LeaveTypeID"), TextSearch, Export(false), Required]
            public int LeaveTypeID
            {
                get { return m_LeaveTypeID; }
                set { m_LeaveTypeID = value; modify("LeaveTypeID"); }
            }
            protected string m_LeaveBalAdjType;
            [DBField("LeaveBalAdjType"), TextSearch, Export(false), Required]
            public string LeaveBalAdjType
            {
                get { return m_LeaveBalAdjType; }
                set { m_LeaveBalAdjType = value; modify("LeaveBalAdjType"); }
            }
            protected double m_LeaveBalAdjValue;
            [DBField("LeaveBalAdjValue", "0.0000"), TextSearch, Export(false), Required]
            public double LeaveBalAdjValue
            {
                get { return m_LeaveBalAdjValue; }
                set { m_LeaveBalAdjValue = value; modify("LeaveBalAdjValue"); }
            }
            protected string m_LeaveBalAdjRemark;
            [DBField("LeaveBalAdjRemark"), TextSearch, Export(false)]
            public string LeaveBalAdjRemark
            {
                get { return m_LeaveBalAdjRemark; }
                set { m_LeaveBalAdjRemark = value; modify("LeaveBalAdjRemark"); }
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

        public const string TABLE_NAME = "LeaveBalanceAdjustment";

        //  Field Const for Employee Personal Information
        private const string FIELD_EMP_NO = "Emp No";
        private const string FIELD_DATE_ADJUST = "Date Adjust";
        private const string FIELD_LEAVE_TYPE = "Leave Type";
        private const string FIELD_ADJUST_TYPE = "Adjust Type";
        private const string FIELD_ADJUST_VALUE = "Adjust Value";
        private const string FIELD_REMARK = "Remark";

        
        private DateTime UploadDateTime = AppUtils.ServerDateTime();
        private DBManager tempDB = EUploadLeaveBalanceAdjustment.db;
        private DBManager uploadDB = ELeaveBalanceAdjustment.db;

        public ImportErrorList errors = new ImportErrorList();


        public ImportLeaveBalanceAdjustmentProcess(DatabaseConnection dbConn, string SessionID): base(dbConn, SessionID)
        {
          

        }

           public DataTable UploadToTempDatabase(DataTable rawDataTable, int UserID)
        {
            if (rawDataTable == null)
                return GetImportDataFromTempDatabase(null);

            int rowCount = 1;
            foreach (DataRow row in rawDataTable.Rows)
            {
                rowCount++;

                EUploadLeaveBalanceAdjustment uploadLeaveBalanceAdjustment = new EUploadLeaveBalanceAdjustment();
                //EEmpAVCPlan lastEmpAVC = null;
                //ArrayList uploadHierarchyList = new ArrayList();

                string EmpNo = row[FIELD_EMP_NO].ToString().Trim();
                uploadLeaveBalanceAdjustment.EmpID = Parse.GetEmpID(dbConn, EmpNo, UserID);
                if (uploadLeaveBalanceAdjustment.EmpID < 0)
                    errors.addError(ImportErrorMessage.ERROR_ACCESS_DENIED_EMP_NO, new string[] { EmpNo, rowCount.ToString() });
                try
                {
                    uploadLeaveBalanceAdjustment.LeaveBalAdjDate = Parse.toDateTimeObject(row[FIELD_DATE_ADJUST]);
                }
                catch
                {
                    errors.addError(ImportErrorMessage.ERROR_INVALID_FIELD_VALUE, new string[] { FIELD_DATE_ADJUST + "=" + row[FIELD_DATE_ADJUST].ToString(), EmpNo, rowCount.ToString() });
                }

                uploadLeaveBalanceAdjustment.LeaveTypeID = Import.Parse.GetLeaveTypeID(dbConn, row[FIELD_LEAVE_TYPE].ToString(), false, UserID);

                if (uploadLeaveBalanceAdjustment.LeaveTypeID == 0)
                    errors.addError(ImportErrorMessage.ERROR_INVALID_FIELD_VALUE, new string[] { FIELD_LEAVE_TYPE + "=" + row[FIELD_LEAVE_TYPE].ToString(), EmpNo, rowCount.ToString() });

                string tempString;
                if (rawDataTable.Columns.Contains(FIELD_ADJUST_TYPE))
                {
                    tempString = row[FIELD_ADJUST_TYPE].ToString().Trim();
                    if (tempString.Equals(ELeaveBalanceAdjustment.ADJUST_TYPE_RESET_BALANCE_NAME, StringComparison.CurrentCultureIgnoreCase)
                        || tempString.Equals(ELeaveBalanceAdjustment.ADJUST_TYPE_RESET_BALANCE, StringComparison.CurrentCultureIgnoreCase))
                        uploadLeaveBalanceAdjustment.LeaveBalAdjType = ELeaveBalanceAdjustment.ADJUST_TYPE_RESET_BALANCE;
                    else if (tempString.Equals(ELeaveBalanceAdjustment.ADJUST_TYPE_ADJUSTMENT_NAME, StringComparison.CurrentCultureIgnoreCase)
                        || tempString.Equals(ELeaveBalanceAdjustment.ADJUST_TYPE_ADJUSTMENT, StringComparison.CurrentCultureIgnoreCase))
                        uploadLeaveBalanceAdjustment.LeaveBalAdjType = ELeaveBalanceAdjustment.ADJUST_TYPE_ADJUSTMENT;
                    //else if (tempString.Equals(string.Empty))
                    //    uploadLeaveApp.LeaveAppUnit = string.Empty;
                    else
                    {
                        errors.addError(ImportErrorMessage.ERROR_INVALID_FIELD_VALUE, new string[] { FIELD_ADJUST_TYPE + "=" + row[FIELD_ADJUST_TYPE].ToString(), EmpNo, rowCount.ToString() });
                    }
                }
                double adjustValue;
                if (double.TryParse(row[FIELD_ADJUST_VALUE].ToString(), out adjustValue))
                    uploadLeaveBalanceAdjustment.LeaveBalAdjValue = adjustValue;
                else
                    errors.addError(ImportErrorMessage.ERROR_INVALID_FIELD_VALUE, new string[] { FIELD_ADJUST_VALUE + "=" + row[FIELD_ADJUST_VALUE].ToString(), EmpNo, rowCount.ToString() });

                uploadLeaveBalanceAdjustment.LeaveBalAdjRemark = row[FIELD_REMARK].ToString();

                uploadLeaveBalanceAdjustment.SessionID = m_SessionID;
                uploadLeaveBalanceAdjustment.TransactionDate = UploadDateTime;


                if (uploadLeaveBalanceAdjustment.EmpID != 0 && errors.List.Count <= 0)
                {
                    if (rawDataTable.Columns.Contains(FIELD_SYNC_ID))
                    {
                        if (!row.IsNull(FIELD_SYNC_ID))
                        {
                            string strSynID = row[FIELD_SYNC_ID].ToString();
                            uploadLeaveBalanceAdjustment.SynID = strSynID;
                            if (!string.IsNullOrEmpty(strSynID))
                            {
                                DBFilter synIDFilter = new DBFilter();
                                synIDFilter.add(new Match("SynID", strSynID));
                                ArrayList objSameSynIDList = ELeaveBalanceAdjustment.db.select(dbConn, synIDFilter);
                                if (objSameSynIDList.Count > 0)
                                    uploadLeaveBalanceAdjustment.LeaveBalAdjID = ((ELeaveBalanceAdjustment)objSameSynIDList[0]).LeaveBalAdjID;
                            }
                        }

                    }

                    if (uploadLeaveBalanceAdjustment.LeaveBalAdjID == 0)
                    {

                        AND andTerm = new AND();
                        andTerm.add(new Match("LeaveBalAdjDate", "=", uploadLeaveBalanceAdjustment.LeaveBalAdjDate));
                        andTerm.add(new Match("LeaveTypeID", "=", uploadLeaveBalanceAdjustment.LeaveTypeID));
                        ELeaveBalanceAdjustment lastObject = (ELeaveBalanceAdjustment)AppUtils.GetLastObj(dbConn, uploadDB, "LeaveBalAdjDate", uploadLeaveBalanceAdjustment.EmpID, andTerm);
                        if (lastObject != null)
                        {
                            uploadLeaveBalanceAdjustment.LeaveBalAdjID = lastObject.LeaveBalAdjID;
                        }
                    }
                }

                if (uploadLeaveBalanceAdjustment.LeaveBalAdjID <= 0)
                    uploadLeaveBalanceAdjustment.ImportActionStatus = ImportDBObject.ImportActionEnum.INSERT;
                else
                    uploadLeaveBalanceAdjustment.ImportActionStatus = ImportDBObject.ImportActionEnum.UPDATE;

                uploadLeaveBalanceAdjustment.UploadEmpID = ParseTemp.GetUploadEmpID(dbConn, EmpNo, m_SessionID);
                if (uploadLeaveBalanceAdjustment.UploadEmpID == 0)
                    if (uploadLeaveBalanceAdjustment.EmpID == 0)
                        errors.addError(ImportErrorMessage.ERROR_INVALID_EMP_NO, new string[] { EmpNo, rowCount.ToString() });
                    else
                        uploadLeaveBalanceAdjustment.UploadEmpID = ImportEmpPersonalInfoProcess.CreateDummyUploadEmployeeInfo(dbConn, uploadLeaveBalanceAdjustment.EmpID, m_SessionID, UploadDateTime);

                Hashtable values = new Hashtable();
                tempDB.populate(uploadLeaveBalanceAdjustment, values);
                PageErrors pageErrors = new PageErrors( EUploadLeaveBalanceAdjustment.db);
                tempDB.validate(pageErrors, values);
                if (pageErrors.errors.Count == 0)
                {

                    tempDB.insert(dbConn, uploadLeaveBalanceAdjustment);
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
            return UploadToTempDatabase(rawDataTable, UserID);
        }
        public override DataTable GetImportDataFromTempDatabase(ListInfo info)
        {
            DBFilter sessionFilter = new DBFilter();
            sessionFilter.add(new Match("SessionID", m_SessionID));
            sessionFilter.add(new MatchField("e.EmpID", "c.EmpID"));
            //if (info != null && info.orderby != null && !info.orderby.Equals(""))
            //    sessionFilter.add(info.orderby, info.order);
            DataTable result = sessionFilter.loadData(dbConn, null, "e.*, c.* ", " from " + tempDB.dbclass.tableName + " c, " + EEmpPersonalInfo.db.dbclass.tableName + " e");


            return result;

        }

        public static void ClearTempTable(DatabaseConnection dbConn, string sessionID)
        {
            ImportLeaveBalanceAdjustmentProcess import = new ImportLeaveBalanceAdjustmentProcess(dbConn, sessionID);
            import.ClearTempTable();
        }

        public override void ClearTempTable()
        {
            //  Clear Old Import Session
            DBFilter sessionFilter = new DBFilter();
            if (!string.IsNullOrEmpty(m_SessionID))
                sessionFilter.add(new Match("SessionID", m_SessionID));
            tempDB.delete(dbConn, sessionFilter);
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
            ArrayList uploadList = tempDB.select(dbConn, sessionFilter);

            foreach (EUploadLeaveBalanceAdjustment obj in uploadList)
            {
                ELeaveBalanceAdjustment leaveBalanceAdjustment = new ELeaveBalanceAdjustment ();

                if (obj.ImportActionStatus != ImportDBObject.ImportActionEnum.INSERT)
                {
                    leaveBalanceAdjustment.LeaveBalAdjID = obj.LeaveBalAdjID;
                    uploadDB.select(dbConn, leaveBalanceAdjustment);
                }

                obj.ExportToObject(leaveBalanceAdjustment);

                if (obj.ImportActionStatus == ImportDBObject.ImportActionEnum.INSERT)
                {
                    leaveBalanceAdjustment.EmpID = ParseTemp.GetEmpIDFromUploadEmpID(dbConn, obj.UploadEmpID);
                    uploadDB.insert(dbConn, leaveBalanceAdjustment);


                }
                else if (obj.ImportActionStatus == ImportDBObject.ImportActionEnum.UPDATE)
                {
                    uploadDB.update(dbConn, leaveBalanceAdjustment);
                }

                tempDB.delete(dbConn, obj);
            }
        }

        public static DataTable Export(DatabaseConnection dbConn, ArrayList empList, bool IsIncludeCurrentPositionInfo, bool IsShowDescription, bool IsIncludeSyncID, DateTime ReferenceDateTime)
        {
            DataTable tmpDataTable = new DataTable(TABLE_NAME);
            tmpDataTable.Columns.Add(FIELD_EMP_NO, typeof(string));

            if (IsIncludeCurrentPositionInfo)
            {
                ImportEmpPersonalInfoProcess.AddEmployeeInfoHeader(tmpDataTable);
                ImportEmpPositionInfoProcess.AddEmployeePositionInfoHeader(dbConn, tmpDataTable);
            }
            tmpDataTable.Columns.Add(FIELD_DATE_ADJUST, typeof(DateTime));
            tmpDataTable.Columns.Add(FIELD_LEAVE_TYPE, typeof(string));
            tmpDataTable.Columns.Add(FIELD_ADJUST_TYPE, typeof(string));
            tmpDataTable.Columns.Add(FIELD_ADJUST_VALUE, typeof(double));
            tmpDataTable.Columns.Add(FIELD_REMARK, typeof(string));

            if (IsIncludeSyncID)
                tmpDataTable.Columns.Add(FIELD_SYNC_ID, typeof(string));

            foreach (EEmpPersonalInfo empInfo in empList)
            {
                if (EEmpPersonalInfo.db.select(dbConn, empInfo))
                {
                    DBFilter filter = new DBFilter();
                    filter.add(new Match("EmpID", empInfo.EmpID));
                    filter.add(getCreateModifiedRecordsAfterDBTerm(ReferenceDateTime));
                    ArrayList list = ELeaveBalanceAdjustment.db.select(dbConn, filter);
                    foreach (ELeaveBalanceAdjustment leaveBalanceAdjustment in list)
                    {
                        DataRow row = tmpDataTable.NewRow();
                        row[FIELD_EMP_NO] = empInfo.EmpNo;

                        if (IsIncludeCurrentPositionInfo)
                        {
                            ImportEmpPersonalInfoProcess.AddEmployeeInfo(dbConn, row, empInfo.EmpID);
                            ImportEmpPositionInfoProcess.AddEmployeePositionInfo(dbConn, row, empInfo.EmpID);
                        }

                        row[FIELD_DATE_ADJUST] = leaveBalanceAdjustment.LeaveBalAdjDate;

                        ELeaveType  leaveType = new ELeaveType();
                        leaveType.LeaveTypeID = leaveBalanceAdjustment.LeaveTypeID;
                        if (ELeaveType.db.select(dbConn, leaveType))
                            row[FIELD_LEAVE_TYPE] = IsShowDescription ? leaveType.LeaveTypeDesc : leaveType.LeaveType;

                        if (leaveBalanceAdjustment.LeaveBalAdjType.Equals(ELeaveBalanceAdjustment.ADJUST_TYPE_ADJUSTMENT))
                            row[FIELD_ADJUST_TYPE] = ELeaveBalanceAdjustment.ADJUST_TYPE_ADJUSTMENT_NAME;
                        else if (leaveBalanceAdjustment.LeaveBalAdjType.Equals(ELeaveBalanceAdjustment.ADJUST_TYPE_RESET_BALANCE))
                            row[FIELD_ADJUST_TYPE] = ELeaveBalanceAdjustment.ADJUST_TYPE_RESET_BALANCE_NAME;

                        row[FIELD_ADJUST_VALUE] = leaveBalanceAdjustment.LeaveBalAdjValue;

                        row[FIELD_REMARK] = leaveBalanceAdjustment.LeaveBalAdjRemark;


                        if (IsIncludeSyncID)
                            row[FIELD_SYNC_ID] = leaveBalanceAdjustment.SynID;

                        tmpDataTable.Rows.Add(row);
                    }
                }

            }
            if (IsIncludeCurrentPositionInfo)
                ImportEmpPositionInfoProcess.RetriveHierarchyLevelHeader(dbConn, tmpDataTable);
            return tmpDataTable;
        }

    }
}
