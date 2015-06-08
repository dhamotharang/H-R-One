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
    public class ImportLeaveApplicationProcess : ImportProcessInteface
    {

        [DBClass("UploadLeaveApplication")]
        private class EUploadLeaveApplication : ImportDBObject
        {
            public static DBManager db = new DBManager(typeof(EUploadLeaveApplication));
            protected int m_UploadLeaveAppID;
            [DBField("UploadLeaveAppID", true, true), TextSearch, Export(false)]
            public int UploadLeaveAppID
            {
                get { return m_UploadLeaveAppID; }
                set { m_UploadLeaveAppID = value; modify("UploadLeaveAppID"); }
            }
            protected int m_UploadEmpID;
            [DBField("UploadEmpID"), TextSearch, Export(false)]
            public int UploadEmpID
            {
                get { return m_UploadEmpID; }
                set { m_UploadEmpID = value; modify("UploadEmpID"); }
            }

            protected int m_LeaveAppID;
            [DBField("LeaveAppID"), TextSearch, Export(false)]
            public int LeaveAppID
            {
                get { return m_LeaveAppID; }
                set { m_LeaveAppID = value; modify("LeaveAppID"); }
            }
            protected int m_EmpID;
            [DBField("EmpID"), TextSearch, Export(false)]
            public int EmpID
            {
                get { return m_EmpID; }
                set { m_EmpID = value; modify("EmpID"); }
            }
            protected int m_LeaveCodeID;
            [DBField("LeaveCodeID"), TextSearch, Export(false), Required]
            public int LeaveCodeID
            {
                get { return m_LeaveCodeID; }
                set { m_LeaveCodeID = value; modify("LeaveCodeID"); }
            }
            protected string m_LeaveAppUnit;
            [DBField("LeaveAppUnit"), TextSearch, Export(false), Required]
            public string LeaveAppUnit
            {
                get { return m_LeaveAppUnit; }
                set { m_LeaveAppUnit = value; modify("LeaveAppUnit"); }
            }
            protected DateTime m_LeaveAppDateFrom;
            [DBField("LeaveAppDateFrom"), TextSearch, MaxLength(10), Export(false), Required]
            public DateTime LeaveAppDateFrom
            {
                get { return m_LeaveAppDateFrom; }
                set { m_LeaveAppDateFrom = value; modify("LeaveAppDateFrom"); }
            }
            protected DateTime m_LeaveAppDateTo;
            [DBField("LeaveAppDateTo"), TextSearch, MaxLength(10), Export(false), Required]
            public DateTime LeaveAppDateTo
            {
                get { return m_LeaveAppDateTo; }
                set { m_LeaveAppDateTo = value; modify("LeaveAppDateTo"); }
            }
            protected DateTime m_LeaveAppTimeFrom;
            [DBField("LeaveAppTimeFrom"), TextSearch, MaxLength(25), Export(false)]
            public DateTime LeaveAppTimeFrom
            {
                get { return m_LeaveAppTimeFrom; }
                set { m_LeaveAppTimeFrom = value; modify("LeaveAppTimeFrom"); }
            }
            protected DateTime m_LeaveAppTimeTo;
            [DBField("LeaveAppTimeTo"), TextSearch, MaxLength(25), Export(false)]
            public DateTime LeaveAppTimeTo
            {
                get { return m_LeaveAppTimeTo; }
                set { m_LeaveAppTimeTo = value; modify("LeaveAppTimeTo"); }
            }
            protected double m_LeaveAppDays;
            [DBField("LeaveAppDays", "0.00"), TextSearch, MaxLength(25), Export(false), Required]
            public double LeaveAppDays
            {
                get { return m_LeaveAppDays; }
                set { m_LeaveAppDays = value; modify("LeaveAppDays"); }
            }
            protected double m_LeaveAppHours;
            [DBField("LeaveAppHours", "0.####"), TextSearch, MaxLength(6), Export(false)]
            public double LeaveAppHours
            {
                get { return m_LeaveAppHours; }
                set { m_LeaveAppHours = value; modify("LeaveAppHours"); }
            }
            protected string m_LeaveAppRemark;
            [DBField("LeaveAppRemark"), TextSearch, Export(false)]
            public string LeaveAppRemark
            {
                get { return m_LeaveAppRemark; }
                set { m_LeaveAppRemark = value; modify("LeaveAppRemark"); }
            }

            protected bool m_LeaveAppNoPayProcess;
            [DBField("LeaveAppNoPayProcess"), TextSearch, Export(false)]
            public bool LeaveAppNoPayProcess
            {
                get { return m_LeaveAppNoPayProcess; }
                set { m_LeaveAppNoPayProcess = value; modify("LeaveAppNoPayProcess"); }
            }

            protected bool m_LeaveAppHasMedicalCertificate;
            [DBField("LeaveAppHasMedicalCertificate"), TextSearch, Export(false)]
            public bool LeaveAppHasMedicalCertificate
            {
                get { return m_LeaveAppHasMedicalCertificate; }
                set { m_LeaveAppHasMedicalCertificate = value; modify("LeaveAppHasMedicalCertificate"); }
            }
            //  For Synchronize Use only
            protected string m_SynID;
            [DBField("SynID"), TextSearch, Export(false)]
            public string SynID
            {
                get { return m_SynID; }
                set { m_SynID = value; modify("SynID"); }
            }


            //  No Import is support
            //protected int m_EmpPaymentID;
            //[DBField("EmpPaymentID"), TextSearch, Export(false)]
            //public int EmpPaymentID
            //{
            //    get { return m_EmpPaymentID; }
            //    set { m_EmpPaymentID = value; modify("EmpPaymentID"); }
            //}

            //protected int m_EmpPayrollID;
            //[DBField("EmpPayrollID"), TextSearch, Export(false)]
            //public int EmpPayrollID
            //{
            //    get { return m_EmpPayrollID; }
            //    set { m_EmpPayrollID = value; modify("EmpPayrollID"); }
            //}
        }

        public const string TABLE_NAME = "leave_applications";

        //  Field Const for Employee Personal Information
        private const string FIELD_EMP_NO = "Emp No";
        private const string FIELD_FROM = "From Date";
        private const string FIELD_TO = "To Date";
        private const string FIELD_LEAVE_CODE = "Leave Code";
        private const string FIELD_DAY_TAKEN = "Days Taken";
        private const string FIELD_HOURS_CLAIM = "Hours Claim";
        private const string FIELD_UNIT = "Unit";   //  Obsolate, Replaced by Type
        private const string FIELD_TYPE = "Type";
        private const string FIELD_REMARK = "Remark";
        private const string FIELD_NOPAYROLLPROCESS = "No Payroll Process";
        private const string FIELD_MEDICIAL_CERTIFICATE = "Has Medicial Certificate";

        
        private DateTime UploadDateTime = AppUtils.ServerDateTime();
        private DBManager tempDB = EUploadLeaveApplication.db;
        private DBManager uploadDB = ELeaveApplication.db;

        public ImportErrorList errors = new ImportErrorList();
        

        public ImportLeaveApplicationProcess(DatabaseConnection dbConn, string SessionID): base(dbConn, SessionID)
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

                EUploadLeaveApplication uploadLeaveApp = new EUploadLeaveApplication();
                //ELeaveApplication lastLeaveApp = null;

                string EmpNo = row[FIELD_EMP_NO].ToString().Trim();
                uploadLeaveApp.EmpID = Parse.GetEmpID(dbConn, EmpNo, UserID);
                if (uploadLeaveApp.EmpID < 0)
                    errors.addError(ImportErrorMessage.ERROR_ACCESS_DENIED_EMP_NO, new string[] { EmpNo, rowCount.ToString() });

                uploadLeaveApp.UploadEmpID = ParseTemp.GetUploadEmpID(dbConn, EmpNo, m_SessionID);
                if (uploadLeaveApp.UploadEmpID == 0)
                    if (uploadLeaveApp.EmpID == 0)
                        errors.addError(ImportErrorMessage.ERROR_INVALID_EMP_NO, new string[] { EmpNo, rowCount.ToString() });
                    else
                        uploadLeaveApp.UploadEmpID = ImportEmpPersonalInfoProcess.CreateDummyUploadEmployeeInfo(dbConn, uploadLeaveApp.EmpID, m_SessionID, UploadDateTime);
                
                try
                {
                    uploadLeaveApp.LeaveAppDateFrom = Parse.toDateTimeObject(row[FIELD_FROM]);
                }
                catch
                {
                    uploadLeaveApp.LeaveAppDateFrom = new DateTime();
                    //errors.addError(ImportErrorMessage.ERROR_INVALID_FIELD_VALUE, new string[] { FIELD_FROM + "=" + row[FIELD_FROM].ToString(), EmpNo, rowCount.ToString() });
                }
                if (uploadLeaveApp.LeaveAppDateFrom.Ticks.Equals(0))
                    errors.addError(ImportErrorMessage.ERROR_INVALID_FIELD_VALUE, new string[] { FIELD_FROM + "=" + row[FIELD_FROM].ToString(), EmpNo, rowCount.ToString() });
                try
                {
                    uploadLeaveApp.LeaveAppDateTo = Parse.toDateTimeObject(row[FIELD_TO]);
                }
                catch
                {
                    uploadLeaveApp.LeaveAppDateTo = new DateTime();
                    //errors.addError(ImportErrorMessage.ERROR_INVALID_FIELD_VALUE, new string[] { FIELD_TO + "=" + row[FIELD_TO].ToString(), EmpNo, rowCount.ToString() });
                }
                if (uploadLeaveApp.LeaveAppDateTo.Ticks.Equals(0))
                    errors.addError(ImportErrorMessage.ERROR_INVALID_FIELD_VALUE, new string[] { FIELD_TO + "=" + row[FIELD_TO].ToString(), EmpNo, rowCount.ToString() });

                if (uploadLeaveApp.LeaveAppDateFrom > uploadLeaveApp.LeaveAppDateFrom)
                    errors.addError(ImportErrorMessage.ERROR_INVALID_FIELD_VALUE, new string[] { FIELD_TO + "=" + row[FIELD_TO].ToString(), EmpNo, rowCount.ToString() });

                uploadLeaveApp.LeaveCodeID = Parse.GetLeaveCodeID(dbConn, row[FIELD_LEAVE_CODE].ToString(), false, UserID);

                if (uploadLeaveApp.LeaveCodeID == 0)
                    errors.addError(ImportErrorMessage.ERROR_INVALID_FIELD_VALUE, new string[] { FIELD_LEAVE_CODE + "=" + row[FIELD_LEAVE_CODE].ToString(), EmpNo, rowCount.ToString() });

                double leaveDays;
                if (double.TryParse(row[FIELD_DAY_TAKEN].ToString(), out leaveDays))
                    uploadLeaveApp.LeaveAppDays = leaveDays;
                else
                    errors.addError(ImportErrorMessage.ERROR_INVALID_FIELD_VALUE, new string[] { FIELD_DAY_TAKEN + "=" + row[FIELD_DAY_TAKEN].ToString(), EmpNo, rowCount.ToString() });
                if (rawDataTable.Columns.Contains(FIELD_HOURS_CLAIM))
                {
                    if (row[FIELD_HOURS_CLAIM] != DBNull.Value)
                    {
                        double hoursClaim;
                        if (double.TryParse(row[FIELD_HOURS_CLAIM].ToString(), out hoursClaim))
                            uploadLeaveApp.LeaveAppHours = hoursClaim;
                        else
                        {
                            if (!string.IsNullOrEmpty(row[FIELD_HOURS_CLAIM].ToString().Trim()))
                                errors.addError(ImportErrorMessage.ERROR_INVALID_FIELD_VALUE, new string[] { FIELD_HOURS_CLAIM + "=" + row[FIELD_HOURS_CLAIM].ToString(), EmpNo, rowCount.ToString() });
                        }
                    }
                    else
                        uploadLeaveApp.LeaveAppHours = 0;
                }
                string tempString;
                if (rawDataTable.Columns.Contains(FIELD_TYPE))
                {
                    tempString = row[FIELD_TYPE].ToString().Replace(" ", "");
                    if (tempString.Equals("Days", StringComparison.CurrentCultureIgnoreCase)
                        || tempString.Equals("Day", StringComparison.CurrentCultureIgnoreCase)
                        || tempString.Equals("D", StringComparison.CurrentCultureIgnoreCase))
                        uploadLeaveApp.LeaveAppUnit = "D";
                    else if (tempString.Equals("Hours", StringComparison.CurrentCultureIgnoreCase)
                        || tempString.Equals("Hour", StringComparison.CurrentCultureIgnoreCase)
                        || tempString.Equals("H", StringComparison.CurrentCultureIgnoreCase))
                        uploadLeaveApp.LeaveAppUnit = "H";
                    else if (tempString.Equals("AM", StringComparison.CurrentCultureIgnoreCase)
                        || tempString.Equals("A", StringComparison.CurrentCultureIgnoreCase))
                        uploadLeaveApp.LeaveAppUnit = "A";
                    else if (tempString.Equals("PM", StringComparison.CurrentCultureIgnoreCase)
                        || tempString.Equals("P", StringComparison.CurrentCultureIgnoreCase))
                        uploadLeaveApp.LeaveAppUnit = "P";
                    else if (tempString.Equals(string.Empty))
                        uploadLeaveApp.LeaveAppUnit = string.Empty;
                    else
                    {
                        errors.addError(ImportErrorMessage.ERROR_INVALID_FIELD_VALUE, new string[] { FIELD_TYPE + "=" + row[FIELD_TYPE].ToString(), EmpNo, rowCount.ToString() });
                    }
                }
                else if (rawDataTable.Columns.Contains(FIELD_UNIT))
                {
                    //  Obsolate, replaced by Type

                    tempString = row[FIELD_UNIT].ToString().Replace(" ", "");
                    if (tempString.Equals("Days", StringComparison.CurrentCultureIgnoreCase)
                        || tempString.Equals("Day", StringComparison.CurrentCultureIgnoreCase)
                        || tempString.Equals("D", StringComparison.CurrentCultureIgnoreCase))
                        uploadLeaveApp.LeaveAppUnit = "D";
                    else if (tempString.Equals("Hours", StringComparison.CurrentCultureIgnoreCase)
                        || tempString.Equals("Hour", StringComparison.CurrentCultureIgnoreCase)
                        || tempString.Equals("H", StringComparison.CurrentCultureIgnoreCase))
                        uploadLeaveApp.LeaveAppUnit = "H";
                    else if (tempString.Equals("AM", StringComparison.CurrentCultureIgnoreCase)
                        || tempString.Equals("A", StringComparison.CurrentCultureIgnoreCase))
                        uploadLeaveApp.LeaveAppUnit = "A";
                    else if (tempString.Equals("PM", StringComparison.CurrentCultureIgnoreCase)
                        || tempString.Equals("P", StringComparison.CurrentCultureIgnoreCase))
                        uploadLeaveApp.LeaveAppUnit = "P";
                    else if (tempString.Equals(string.Empty))
                        uploadLeaveApp.LeaveAppUnit = string.Empty;
                    else
                    {
                        errors.addError(ImportErrorMessage.ERROR_INVALID_FIELD_VALUE, new string[] { FIELD_UNIT + "=" + row[FIELD_UNIT].ToString(), EmpNo, rowCount.ToString() });
                    }
                }
                uploadLeaveApp.LeaveAppRemark = row[FIELD_REMARK].ToString();

                tempString = row[FIELD_NOPAYROLLPROCESS].ToString().Replace(" ", "");
                if (tempString.Equals("Yes", StringComparison.CurrentCultureIgnoreCase)
                    || tempString.Equals("Y", StringComparison.CurrentCultureIgnoreCase))
                    uploadLeaveApp.LeaveAppNoPayProcess = true;
                else if (tempString.Equals("No", StringComparison.CurrentCultureIgnoreCase)
                    || tempString.Equals("N", StringComparison.CurrentCultureIgnoreCase)
                    || tempString.Equals(string.Empty))
                    uploadLeaveApp.LeaveAppNoPayProcess = false;
                else if (tempString.Equals(string.Empty))
                    uploadLeaveApp.LeaveAppNoPayProcess = false;
                else
                {
                    errors.addError(ImportErrorMessage.ERROR_INVALID_FIELD_VALUE, new string[] { FIELD_NOPAYROLLPROCESS + "=" + row[FIELD_NOPAYROLLPROCESS].ToString(), EmpNo, rowCount.ToString() });
                }

                //  Enforce No Payroll Process flag to true if leave code is skip payroll process
                ELeaveCode leaveCode = new ELeaveCode();
                leaveCode.LeaveCodeID = uploadLeaveApp.LeaveCodeID;
                if (ELeaveCode.db.select(dbConn, leaveCode))
                {
                    if (leaveCode.LeaveCodeIsSkipPayrollProcess)
                        uploadLeaveApp.LeaveAppNoPayProcess = true;
                }
                if (rawDataTable.Columns.Contains(FIELD_MEDICIAL_CERTIFICATE))
                {
                    tempString = row[FIELD_MEDICIAL_CERTIFICATE].ToString().Replace(" ", "");
                    if (tempString.Equals("Yes", StringComparison.CurrentCultureIgnoreCase)
                        || tempString.Equals("Y", StringComparison.CurrentCultureIgnoreCase))
                        uploadLeaveApp.LeaveAppHasMedicalCertificate = true;
                    else if (tempString.Equals("No", StringComparison.CurrentCultureIgnoreCase)
                        || tempString.Equals("N", StringComparison.CurrentCultureIgnoreCase)
                        || tempString.Equals(string.Empty))
                        uploadLeaveApp.LeaveAppHasMedicalCertificate = false;
                    else if (tempString.Equals(string.Empty))
                        uploadLeaveApp.LeaveAppHasMedicalCertificate = false;
                    else
                    {
                        errors.addError(ImportErrorMessage.ERROR_INVALID_FIELD_VALUE, new string[] { FIELD_NOPAYROLLPROCESS + "=" + row[FIELD_NOPAYROLLPROCESS].ToString(), EmpNo, rowCount.ToString() });
                    }
                }

                uploadLeaveApp.SessionID = m_SessionID;
                uploadLeaveApp.TransactionDate = UploadDateTime;

                if (errors.List.Count <= 0 && uploadLeaveApp.EmpID != 0)
                {
                    //if (rawDataTable.Columns.Contains(FIELD_INTERNAL_ID))
                    //{
                    //    try
                    //    {
                    //        if (!row.IsNull(FIELD_INTERNAL_ID))
                    //        {
                    //            int tmpID = FromHexDecWithCheckDigit((string)row[FIELD_INTERNAL_ID]);
                    //            ELeaveApplication tmpObj = new ELeaveApplication();
                    //            tmpObj.LeaveAppID = tmpID;
                    //            if (ELeaveApplication.db.select(dbConn, tmpObj))
                    //                uploadLeaveApp.LeaveAppID = tmpID;
                    //            else
                    //            {
                    //                errors.addError(ImportErrorMessage.ERROR_INVALID_FIELD_VALUE, new string[] { FIELD_INTERNAL_ID + "=" + row[FIELD_INTERNAL_ID].ToString(), EmpNo, rowCount.ToString() });
                    //                continue;
                    //            }
                    //        }
                    //    }
                    //    catch (Exception ex)
                    //    {
                    //        errors.addError(ImportErrorMessage.ERROR_INVALID_FIELD_VALUE, new string[] { FIELD_INTERNAL_ID + "=" + row[FIELD_INTERNAL_ID].ToString(), EmpNo, rowCount.ToString() });
                    //        continue;
                    //    }
                    //}
                    if (rawDataTable.Columns.Contains(FIELD_SYNC_ID))
                    {
                        if (!row.IsNull(FIELD_SYNC_ID))
                        {
                            string strSynID = row[FIELD_SYNC_ID].ToString();
                            uploadLeaveApp.SynID = strSynID;
                            if (!string.IsNullOrEmpty(strSynID))
                            {
                                DBFilter synIDFilter = new DBFilter();
                                synIDFilter.add(new Match("SynID", strSynID));
                                ArrayList objSameSynIDList = ELeaveApplication.db.select(dbConn, synIDFilter);
                                if (objSameSynIDList.Count > 0)
                                    uploadLeaveApp.LeaveAppID = ((ELeaveApplication)objSameSynIDList[0]).LeaveAppID;
                            }
                        }

                    }

                    if (uploadLeaveApp.LeaveAppID == 0)
                    {
                        AND lastObjAndTerms = new AND();
                        lastObjAndTerms.add(new Match("LeaveAppDateTo", ">=", uploadLeaveApp.LeaveAppDateFrom));
                        lastObjAndTerms.add(new Match("LeaveAppDateFrom", "<=", uploadLeaveApp.LeaveAppDateTo));
                        if (!uploadLeaveApp.LeaveAppUnit.Equals("D"))
                            lastObjAndTerms.add(new Match("LeaveAppUnit", "D"));


                        ELeaveApplication lastObj = (ELeaveApplication)AppUtils.GetLastObj(dbConn, uploadDB, "LeaveAppDateFrom", uploadLeaveApp.EmpID, lastObjAndTerms);
                        if (lastObj != null)
                        {
                            errors.addError(ImportErrorMessage.ERROR_DATE_FROM_OVERLAP, new string[] { uploadLeaveApp.LeaveAppDateFrom.ToString("yyyy-MM-dd"), rowCount.ToString() });
                            continue;
                        }


                        //lastLeaveApp = (ELeaveApplication)AppUtils.GetLastObj(dbConn, uploadDB, "LeaveAppDateFrom", uploadLeaveApp.EmpID, new Match("LeaveAppDateFrom", "<=", uploadLeaveApp.LeaveAppDateFrom));
                        //if (lastLeaveApp != null)
                        //{

                        //    //if (uploadLeaveApp.LeaveCodeID == lastLeaveApp.LeaveCodeID
                        //    //    && uploadLeaveApp.LeaveAppRemark == lastLeaveApp.LeaveAppRemark
                        //    //    )
                        //    //{
                        //    //    continue;
                        //    //}
                        //    //else
                        //    {
                        //        //// add postion terms with new ID
                        //        //if (lastLeaveApp.LeaveAppDateFrom.Equals(uploadLeaveApp.LeaveAppDateFrom) && lastLeaveApp.LeaveAppUnit.Equals(uploadLeaveApp.LeaveAppUnit) && uploadLeaveApp.LeaveAppUnit.Equals("D"))
                        //        //{
                        //        //    if (lastLeaveApp.EmpPayrollID > 0)
                        //        //    {
                        //        //        errors.addError(ImportErrorMessage.ERROR_LEAVE_DATE_PROCESSED, new string[] { uploadLeaveApp.LeaveAppDateFrom.ToString("yyyy-MM-dd"), rowCount.ToString() });
                        //        //        continue;
                        //        //    }

                        //        //    uploadLeaveApp.LeaveAppID = lastLeaveApp.LeaveAppID;
                        //        //}
                        //        //else
                        //        {


                        //            AND lastObjAndTerms = new AND();
                        //            lastObjAndTerms.add(new Match("LeaveAppDateTo", ">=", uploadLeaveApp.LeaveAppDateFrom));
                        //            lastObjAndTerms.add(new Match("LeaveAppDateFrom", "<=", uploadLeaveApp.LeaveAppDateTo));
                        //            if (!uploadLeaveApp.LeaveAppUnit.Equals("D"))
                        //                lastObjAndTerms.add(new Match("LeaveAppUnit", "D"));


                        //            ELeaveApplication lastObj = (ELeaveApplication)AppUtils.GetLastObj(dbConn, uploadDB, "LeaveAppDateFrom", uploadLeaveApp.EmpID, lastObjAndTerms);
                        //            if (lastObj != null)
                        //            {
                        //                errors.addError(ImportErrorMessage.ERROR_DATE_FROM_OVERLAP, new string[] { uploadLeaveApp.LeaveAppDateFrom.ToString("yyyy-MM-dd"), rowCount.ToString() });
                        //                continue;
                        //            }
                        //        }
                        //    }
                        //}
                    }
                }

                if (uploadLeaveApp.LeaveAppID <= 0)
                    uploadLeaveApp.ImportActionStatus = ImportDBObject.ImportActionEnum.INSERT;
                else
                    uploadLeaveApp.ImportActionStatus = ImportDBObject.ImportActionEnum.UPDATE;

                Hashtable values = new Hashtable();
                tempDB.populate(uploadLeaveApp, values);
                PageErrors pageErrors = new PageErrors(EUploadEmpPersonalInfo.db);
                tempDB.validate(pageErrors, values);
                if (pageErrors.errors.Count == 0)
                {

                    tempDB.insert(dbConn, uploadLeaveApp);
                }
                else
                {
                    pageErrors.addError(rawDataTable.TableName);
                    throw new HRImportException(pageErrors.getPrompt() + "(line " +rowCount.ToString() + ")");

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
            if (info != null && info.orderby != null && !info.orderby.Equals(""))
                sessionFilter.add(info.orderby, info.order);
            DataTable table = sessionFilter.loadData(dbConn, info, " c.* ", " from " + tempDB.dbclass.tableName + " c ");


            return table;

        }

        public static void ClearTempTable(DatabaseConnection dbConn, string sessionID)
        {
            ImportLeaveApplicationProcess import = new ImportLeaveApplicationProcess(dbConn, sessionID);
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
            ArrayList uploadLeaveAppList = tempDB.select(dbConn, sessionFilter);

            //Dictionary<int, DateTime> recalLeaveBalanceDateList = new Dictionary<int, DateTime>();

            foreach (EUploadLeaveApplication obj in uploadLeaveAppList)
            {
                ELeaveApplication leaveApplication = new ELeaveApplication();

                if (obj.ImportActionStatus != ImportDBObject.ImportActionEnum.INSERT)
                {
                    leaveApplication.LeaveAppID = obj.LeaveAppID;
                    uploadDB.select(dbConn, leaveApplication);
                }

                obj.ExportToObject(leaveApplication);

                if (obj.ImportActionStatus == ImportDBObject.ImportActionEnum.INSERT)
                {
                    leaveApplication.EmpID = ParseTemp.GetEmpIDFromUploadEmpID(dbConn, obj.UploadEmpID);

                    uploadDB.insert(dbConn, leaveApplication);

                }
                else if (obj.ImportActionStatus == ImportDBObject.ImportActionEnum.UPDATE)
                {
                    uploadDB.update(dbConn, leaveApplication);

                }
                tempDB.delete(dbConn, obj);
            }

            //foreach (int EmpID in recalLeaveBalanceDateList.Keys)
            //{
            //    HROne.LeaveCalc.LeaveBalanceCalc leaaveBalCal = new HROne.LeaveCalc.LeaveBalanceCalc(dbConn, EmpID);
            //    leaaveBalCal.RecalculateAfter(recalLeaveBalanceDateList[EmpID]);

            //}
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
            {
                ImportEmpPersonalInfoProcess.AddEmployeeInfoHeader(tmpDataTable);
                ImportEmpPositionInfoProcess.AddEmployeePositionInfoHeader(dbConn, tmpDataTable);
            }

            tmpDataTable.Columns.Add(FIELD_FROM, typeof(DateTime));
            tmpDataTable.Columns.Add(FIELD_TO, typeof(DateTime));
            tmpDataTable.Columns.Add(FIELD_LEAVE_CODE, typeof(string));
            tmpDataTable.Columns.Add(FIELD_DAY_TAKEN, typeof(double));
            tmpDataTable.Columns.Add(FIELD_HOURS_CLAIM, typeof(double));            
            tmpDataTable.Columns.Add(FIELD_TYPE, typeof(string));
            tmpDataTable.Columns.Add(FIELD_REMARK, typeof(string));
            tmpDataTable.Columns.Add(FIELD_NOPAYROLLPROCESS, typeof(string));
            tmpDataTable.Columns.Add(FIELD_MEDICIAL_CERTIFICATE, typeof(string));

            if (IsIncludeSyncID)
                tmpDataTable.Columns.Add(FIELD_SYNC_ID, typeof(string));

            foreach (EEmpPersonalInfo empInfo in empList)
            {
                if (EEmpPersonalInfo.db.select(dbConn, empInfo))
                {
                    DBFilter filter = new DBFilter();
                    filter.add(new Match("EmpID", empInfo.EmpID));
                    filter.add(getCreateModifiedRecordsAfterDBTerm(ReferenceDateTime));
                    ArrayList list = ELeaveApplication.db.select(dbConn, filter);
                    foreach (ELeaveApplication empLeaveApplication in list)
                    {
                        DataRow row = tmpDataTable.NewRow();
                        //if (IsIncludeInternalID)
                        //    row[FIELD_INTERNAL_ID] = ToHexDecWithCheckDigit(empLeaveApplication.LeaveAppID);
                        row[FIELD_EMP_NO] = empInfo.EmpNo;

                        if (IsIncludeCurrentPositionInfo)
                        {
                            ImportEmpPersonalInfoProcess.AddEmployeeInfo(dbConn, row, empInfo.EmpID);
                            ImportEmpPositionInfoProcess.AddEmployeePositionInfo(dbConn, row, empInfo.EmpID);
                        }

                        row[FIELD_FROM] = empLeaveApplication.LeaveAppDateFrom;
                        row[FIELD_TO] = empLeaveApplication.LeaveAppDateTo;

                        ELeaveCode leaveCode = new ELeaveCode();
                        leaveCode.LeaveCodeID = empLeaveApplication.LeaveCodeID;
                        if (ELeaveCode.db.select(dbConn, leaveCode))
                            row[FIELD_LEAVE_CODE] = IsShowDescription ? leaveCode.LeaveCodeDesc : leaveCode.LeaveCode;

                        row[FIELD_DAY_TAKEN] = empLeaveApplication.LeaveAppDays;
                        row[FIELD_HOURS_CLAIM] = empLeaveApplication.LeaveAppHours;
                        
                        if (empLeaveApplication.LeaveAppUnit.Equals("H"))
                            row[FIELD_TYPE] = "Hour";
                        else if (empLeaveApplication.LeaveAppUnit.Equals("D"))
                            row[FIELD_TYPE] = "Day";
                        else if (empLeaveApplication.LeaveAppUnit.Equals("A"))
                            row[FIELD_TYPE] = "AM";
                        else if (empLeaveApplication.LeaveAppUnit.Equals("P"))
                            row[FIELD_TYPE] = "PM";

                        row[FIELD_MEDICIAL_CERTIFICATE] = empLeaveApplication.LeaveAppHasMedicalCertificate ? "Yes" : "No";
                        row[FIELD_REMARK] = empLeaveApplication.LeaveAppRemark;
                        row[FIELD_NOPAYROLLPROCESS] = empLeaveApplication.LeaveAppNoPayProcess?"Yes":"No";

                        if (IsIncludeSyncID)
                            row[FIELD_SYNC_ID] = empLeaveApplication.SynID;

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
