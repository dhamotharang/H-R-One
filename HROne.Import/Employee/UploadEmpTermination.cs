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
    public class ImportEmpTerminationProcess : ImportProcessInteface
    {

        [DBClass("UploadEmpTermination")]
        private class EUploadEmpTermination : ImportDBObject
        {
            public static DBManager db = new DBManager(typeof(EUploadEmpTermination));

            protected int m_UploadEmpTermID;
            [DBField("UploadEmpTermID", true, true), TextSearch, Export(false)]
            public int UploadEmpTermID
            {
                get { return m_UploadEmpTermID; }
                set { m_UploadEmpTermID = value; modify("UploadEmpTermID"); }
            }
            protected int m_UploadEmpID;
            [DBField("UploadEmpID"), TextSearch, Export(false)]
            public int UploadEmpID
            {
                get { return m_UploadEmpID; }
                set { m_UploadEmpID = value; modify("UploadEmpID"); }
            }

            protected int m_EmpTermID;
            [DBField("EmpTermID"), TextSearch, Export(false)]
            public int EmpTermID
            {
                get { return m_EmpTermID; }
                set { m_EmpTermID = value; modify("EmpTermID"); }
            }
            protected int m_EmpID;
            [DBField("EmpID"), TextSearch, Export(false)]
            public int EmpID
            {
                get { return m_EmpID; }
                set { m_EmpID = value; modify("EmpID"); }
            }

            protected int m_CessationReasonID;
            [DBField("CessationReasonID"), TextSearch, Export(false), Required]
            public int CessationReasonID
            {
                get { return m_CessationReasonID; }
                set { m_CessationReasonID = value; modify("CessationReasonID"); }
            }
            protected DateTime m_EmpTermResignDate;
            [DBField("EmpTermResignDate"), TextSearch, MaxLength(10), Export(false), Required]
            public DateTime EmpTermResignDate
            {
                get { return m_EmpTermResignDate; }
                set { m_EmpTermResignDate = value; modify("EmpTermResignDate"); }
            }
            protected int m_EmpTermNoticePeriod;
            [DBField("EmpTermNoticePeriod"), TextSearch, MaxLength(5), Export(false), Required]
            public int EmpTermNoticePeriod
            {
                get { return m_EmpTermNoticePeriod; }
                set { m_EmpTermNoticePeriod = value; modify("EmpTermNoticePeriod"); }
            }
            protected string m_EmpTermNoticeUnit;
            [DBField("EmpTermNoticeUnit"), TextSearch, Export(false), Required]
            public string EmpTermNoticeUnit
            {
                get { return m_EmpTermNoticeUnit; }
                set { m_EmpTermNoticeUnit = value; modify("EmpTermNoticeUnit"); }
            }
            protected DateTime m_EmpTermLastDate;
            [DBField("EmpTermLastDate"), TextSearch, MaxLength(10), Export(false), Required]
            public DateTime EmpTermLastDate
            {
                get { return m_EmpTermLastDate; }
                set { m_EmpTermLastDate = value; modify("EmpTermLastDate"); }
            }
            protected string m_EmpTermRemark;
            [DBField("EmpTermRemark"), TextSearch, Export(false)]
            public string EmpTermRemark
            {
                get { return m_EmpTermRemark; }
                set { m_EmpTermRemark = value; modify("EmpTermRemark"); }
            }
            protected bool m_EmpTermIsTransferCompany;
            [DBField("EmpTermIsTransferCompany"), TextSearch, Export(false)]
            public bool EmpTermIsTransferCompany
            {
                get { return m_EmpTermIsTransferCompany; }
                set { m_EmpTermIsTransferCompany = value; modify("EmpTermIsTransferCompany"); }
            }
            protected int m_NewEmpID;
            [DBField("NewEmpID"), TextSearch, Export(false)]
            public int NewEmpID
            {
                get { return m_NewEmpID; }
                set { m_NewEmpID = value; modify("NewEmpID"); }
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
        public const string TABLE_NAME = "termination_detail";

        //  Field Const for Employee Personal Information
        private const string FIELD_EMP_NO = "Emp No";
        private const string FIELD_CESSATION_REASON = "CessationReason";
        private const string FIELD_RESIGN_DATE = "Resign Date";
        private const string FIELD_NOTICE_PERIOD = "Notice Period";
        private const string FIELD_NOTICE_PERIOD_UNIT = "Notice Period Unit";
        private const string FIELD_LAST_DATE = "Last Date";
        private const string FIELD_REMARK = "Remark";
        private const string FIELD_NEW_EMP_NO = "New Emp No";

        
        private DateTime UploadDateTime = AppUtils.ServerDateTime();
        private DBManager tempDB = EUploadEmpTermination.db;
        private DBManager uploadDB = EEmpTermination.db;

        public ImportErrorList errors = new ImportErrorList();
        

        public ImportEmpTerminationProcess(DatabaseConnection dbConn, string SessionID): base(dbConn, SessionID)
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

                EUploadEmpTermination uploadEmpTermination = new EUploadEmpTermination();
                EEmpTermination lastEmpTermination = null;

                string EmpNo = row[FIELD_EMP_NO].ToString().Trim();
                uploadEmpTermination.EmpID = Parse.GetEmpID(dbConn, EmpNo, UserID);
                if (uploadEmpTermination.EmpID < 0)
                    errors.addError(ImportErrorMessage.ERROR_ACCESS_DENIED_EMP_NO, new string[] { EmpNo, rowCount.ToString() });
                uploadEmpTermination.CessationReasonID = Parse.GetCessationReasonID(dbConn, row[FIELD_CESSATION_REASON].ToString(), CreateCodeIfNotExists, UserID);
                if (uploadEmpTermination.CessationReasonID<=0)
                    errors.addError(ImportErrorMessage.ERROR_INVALID_FIELD_VALUE, new string[] { FIELD_CESSATION_REASON + "=" + row[FIELD_CESSATION_REASON].ToString(), EmpNo, rowCount.ToString() });
                try
                {
                    uploadEmpTermination.EmpTermResignDate = Parse.toDateTimeObject(row[FIELD_RESIGN_DATE]);
                }
                catch
                {
                    errors.addError(ImportErrorMessage.ERROR_INVALID_FIELD_VALUE, new string[] { FIELD_RESIGN_DATE + "=" + row[FIELD_RESIGN_DATE].ToString(), EmpNo, rowCount.ToString() });
                }
                if (uploadEmpTermination.EmpTermResignDate.Ticks.Equals(0))
                    errors.addError(ImportErrorMessage.ERROR_INVALID_FIELD_VALUE, new string[] { FIELD_RESIGN_DATE + "=" + row[FIELD_RESIGN_DATE].ToString(), EmpNo, rowCount.ToString() });

                string tempString;
                tempString = row[FIELD_NOTICE_PERIOD_UNIT].ToString().Replace(" ", "");
                if (tempString.Equals("Days", StringComparison.CurrentCultureIgnoreCase)
                    || tempString.Equals("Day", StringComparison.CurrentCultureIgnoreCase)
                    || tempString.Equals("D", StringComparison.CurrentCultureIgnoreCase))
                    uploadEmpTermination.EmpTermNoticeUnit = "D";
                else if (tempString.Equals("Months", StringComparison.CurrentCultureIgnoreCase)
                    || tempString.Equals("Month", StringComparison.CurrentCultureIgnoreCase)
                    || tempString.Equals("M", StringComparison.CurrentCultureIgnoreCase))
                    uploadEmpTermination.EmpTermNoticeUnit = "M";
                else if (tempString.Equals(string.Empty))
                    uploadEmpTermination.EmpTermNoticeUnit = string.Empty;
                else
                {
                    uploadEmpTermination.EmpTermNoticeUnit = string.Empty;
                    errors.addError(ImportErrorMessage.ERROR_INVALID_FIELD_VALUE, new string[] { FIELD_NOTICE_PERIOD_UNIT + "=" + row[FIELD_NOTICE_PERIOD_UNIT].ToString(), EmpNo, rowCount.ToString() });
                }
                int period = 0;
                if (int.TryParse(row[FIELD_NOTICE_PERIOD].ToString(), out period))
                    uploadEmpTermination.EmpTermNoticePeriod = period;
                else
                    if (uploadEmpTermination.EmpTermNoticeUnit.Equals(string.Empty) )
                    {
                        if (uploadEmpTermination.EmpID > 0)
                        {
                            //  Get Notice Period from EmpInfo
                            EEmpPersonalInfo empinfo = new EEmpPersonalInfo();
                            empinfo.EmpID = uploadEmpTermination.EmpID;
                            EEmpPersonalInfo.db.select(dbConn, empinfo);
                            if (!empinfo.EmpNoticeUnit.Equals(string.Empty))
                            {
                                uploadEmpTermination.EmpTermNoticeUnit = empinfo.EmpNoticeUnit;
                                uploadEmpTermination.EmpTermNoticePeriod = empinfo.EmpNoticePeriod;
                            }
                            else
                            {
                                errors.addError(ImportErrorMessage.ERROR_INVALID_FIELD_VALUE, new string[] { FIELD_NOTICE_PERIOD + "='" + row[FIELD_NOTICE_PERIOD].ToString() + "'", EmpNo, rowCount.ToString() });
                                errors.addError(ImportErrorMessage.ERROR_INVALID_FIELD_VALUE, new string[] { FIELD_NOTICE_PERIOD_UNIT + "='" + row[FIELD_NOTICE_PERIOD_UNIT].ToString() + "'", EmpNo, rowCount.ToString() });
                            }
                        }
                    }
                    else
                        errors.addError(ImportErrorMessage.ERROR_INVALID_FIELD_VALUE, new string[] { FIELD_NOTICE_PERIOD + "='" + row[FIELD_NOTICE_PERIOD].ToString() + "'", EmpNo, rowCount.ToString() });
                try
                {
                    uploadEmpTermination.EmpTermLastDate = Parse.toDateTimeObject(row[FIELD_LAST_DATE]);
                }
                catch
                {
                    errors.addError(ImportErrorMessage.ERROR_INVALID_FIELD_VALUE, new string[] { FIELD_LAST_DATE + "=" + row[FIELD_LAST_DATE].ToString(), EmpNo, rowCount.ToString() });
                }

                if (uploadEmpTermination.EmpTermLastDate.Ticks == 0)
                    if (uploadEmpTermination.EmpTermNoticeUnit.Equals("M"))
                        uploadEmpTermination.EmpTermLastDate = uploadEmpTermination.EmpTermResignDate.AddMonths(uploadEmpTermination.EmpTermNoticePeriod).AddDays(-1);
                    else if (uploadEmpTermination.EmpTermNoticeUnit.Equals("D"))
                        uploadEmpTermination.EmpTermLastDate = uploadEmpTermination.EmpTermResignDate.AddDays(uploadEmpTermination.EmpTermNoticePeriod).AddDays(-1);
                    else
                        errors.addError(ImportErrorMessage.ERROR_INVALID_FIELD_VALUE, new string[] { FIELD_LAST_DATE + "='" + row[FIELD_LAST_DATE].ToString() + "'", EmpNo, rowCount.ToString() });
                if (rawDataTable.Columns.Contains(FIELD_NEW_EMP_NO))
                {
                    if (!string.IsNullOrEmpty(row[FIELD_NEW_EMP_NO].ToString()))
                    {
                        //  Use UserID = 0 to allow searching all EmpID
                        int newEmpID = Parse.GetEmpID(dbConn, row[FIELD_NEW_EMP_NO].ToString().Trim(), 0);
                        if (newEmpID > 0)
                            if (newEmpID != uploadEmpTermination.EmpID)
                            {
                                uploadEmpTermination.EmpTermIsTransferCompany = true;
                                uploadEmpTermination.NewEmpID = newEmpID;
                            }
                            else
                                errors.addError(ImportErrorMessage.ERROR_INVALID_FIELD_VALUE, new string[] { FIELD_NEW_EMP_NO + "='" + row[FIELD_NEW_EMP_NO].ToString() + "'", EmpNo, rowCount.ToString() });
                        else
                            errors.addError(ImportErrorMessage.ERROR_INVALID_FIELD_VALUE, new string[] { FIELD_NEW_EMP_NO + "='" + row[FIELD_NEW_EMP_NO].ToString() + "'", EmpNo, rowCount.ToString() });
                    }
                }
                uploadEmpTermination.EmpTermRemark = row[FIELD_REMARK].ToString();



                uploadEmpTermination.SessionID = m_SessionID;
                uploadEmpTermination.TransactionDate = UploadDateTime;


                if (uploadEmpTermination.EmpID != 0)
                {
                    //if (rawDataTable.Columns.Contains(FIELD_INTERNAL_ID))
                    //{
                    //    try
                    //    {
                    //        if (!row.IsNull(FIELD_INTERNAL_ID))
                    //        {
                    //            int tmpID = FromHexDecWithCheckDigit((string)row[FIELD_INTERNAL_ID]);
                    //            EEmpTermination tmpObj = new EEmpTermination();
                    //            tmpObj.EmpTermID = tmpID;
                    //            if (EEmpTermination.db.select(dbConn, tmpObj))
                    //                uploadEmpTermination.EmpTermID = tmpID;
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
                            uploadEmpTermination.SynID = strSynID;
                            if (!string.IsNullOrEmpty(strSynID))
                            {
                                DBFilter synIDFilter = new DBFilter();
                                synIDFilter.add(new Match("SynID", strSynID));
                                ArrayList objSameSynIDList = EEmpTermination.db.select(dbConn, synIDFilter);
                                if (objSameSynIDList.Count > 0)
                                    uploadEmpTermination.EmpTermID = ((EEmpTermination)objSameSynIDList[0]).EmpTermID;
                            }
                        }

                    }

                    if (uploadEmpTermination.EmpTermID == 0)
                    {
                        lastEmpTermination = (EEmpTermination)AppUtils.GetLastObj(dbConn, uploadDB, "EmpTermID", uploadEmpTermination.EmpID);
                        if (lastEmpTermination != null)
                        {

                            if (uploadEmpTermination.CessationReasonID == lastEmpTermination.CessationReasonID
                                && uploadEmpTermination.EmpTermResignDate == lastEmpTermination.EmpTermResignDate
                                && uploadEmpTermination.EmpTermNoticeUnit == lastEmpTermination.EmpTermNoticeUnit
                                && uploadEmpTermination.EmpTermNoticePeriod == lastEmpTermination.EmpTermNoticePeriod
                                && uploadEmpTermination.EmpTermLastDate == lastEmpTermination.EmpTermLastDate
                                && uploadEmpTermination.EmpTermRemark == lastEmpTermination.EmpTermRemark
                                && uploadEmpTermination.NewEmpID == lastEmpTermination.NewEmpID
                                )
                            {
                                continue;
                            }
                            else
                            {
                                uploadEmpTermination.EmpTermID = lastEmpTermination.EmpTermID;
                            }
                        }
                    }
                }

                if (uploadEmpTermination.EmpTermID <= 0)
                    uploadEmpTermination.ImportActionStatus = ImportDBObject.ImportActionEnum.INSERT;
                else
                    uploadEmpTermination.ImportActionStatus = ImportDBObject.ImportActionEnum.UPDATE;

                uploadEmpTermination.UploadEmpID = ParseTemp.GetUploadEmpID(dbConn, EmpNo, m_SessionID);
                if (uploadEmpTermination.UploadEmpID == 0)
                    if (uploadEmpTermination.EmpID == 0)
                        errors.addError(ImportErrorMessage.ERROR_INVALID_EMP_NO, new string[] { EmpNo, rowCount.ToString() });
                    else
                        uploadEmpTermination.UploadEmpID = ImportEmpPersonalInfoProcess.CreateDummyUploadEmployeeInfo(dbConn, uploadEmpTermination.EmpID, m_SessionID, UploadDateTime);

                Hashtable values = new Hashtable();
                tempDB.populate(uploadEmpTermination, values);
                PageErrors pageErrors = new PageErrors(EUploadEmpPersonalInfo.db);
                tempDB.validate(pageErrors, values);
                if (pageErrors.errors.Count == 0)
                {

                    tempDB.insert(dbConn, uploadEmpTermination);
                }
                else
                {
                    pageErrors.addError(rawDataTable.TableName);
                    throw new HRImportException(pageErrors.getPrompt() + " on line " + rowCount);

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
            ImportEmpTerminationProcess import = new ImportEmpTerminationProcess(dbConn, sessionID);
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
            ArrayList uploadEmpTerminationList = tempDB.select(dbConn, sessionFilter);
            foreach (EUploadEmpTermination obj in uploadEmpTerminationList)
            {
                EEmpTermination empTermination = new EEmpTermination();

                if (obj.ImportActionStatus != ImportDBObject.ImportActionEnum.INSERT)
                {
                    empTermination.EmpTermID = obj.EmpTermID;
                    uploadDB.select(dbConn, empTermination);
                }

                obj.ExportToObject(empTermination);

                if (obj.ImportActionStatus == ImportDBObject.ImportActionEnum.INSERT)
                {
                    empTermination.EmpID = ParseTemp.GetEmpIDFromUploadEmpID(dbConn, obj.UploadEmpID);
                    uploadDB.insert(dbConn, empTermination);

                    //  Upload Employee Status to Terminated
                    EEmpPersonalInfo empInfo = new EEmpPersonalInfo();
                    empInfo.EmpID = empTermination.EmpID;
                    empInfo.EmpStatus = "T";
                    EEmpPersonalInfo.db.update(dbConn, empInfo);
                }
                else if (obj.ImportActionStatus == ImportDBObject.ImportActionEnum.UPDATE)
                {
                    uploadDB.update(dbConn, empTermination);
                    //  Upload Employee Status to Terminated
                    EEmpPersonalInfo empInfo = new EEmpPersonalInfo();
                    empInfo.EmpID = empTermination.EmpID;
                    empInfo.EmpStatus = "T";
                    EEmpPersonalInfo.db.update(dbConn, empInfo);

                }
                tempDB.delete(dbConn, obj);
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
            {
                ImportEmpPersonalInfoProcess.AddEmployeeInfoHeader(tmpDataTable);
                ImportEmpPositionInfoProcess.AddEmployeePositionInfoHeader(dbConn, tmpDataTable);
            }

            tmpDataTable.Columns.Add(FIELD_CESSATION_REASON, typeof(string));
            tmpDataTable.Columns.Add(FIELD_RESIGN_DATE, typeof(DateTime));
            tmpDataTable.Columns.Add(FIELD_NOTICE_PERIOD, typeof(double));
            tmpDataTable.Columns.Add(FIELD_NOTICE_PERIOD_UNIT, typeof(string));
            tmpDataTable.Columns.Add(FIELD_LAST_DATE, typeof(DateTime));
            tmpDataTable.Columns.Add(FIELD_NEW_EMP_NO, typeof(string));
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
                    ArrayList list = EEmpTermination.db.select(dbConn, filter);
                    foreach (EEmpTermination empTermination in list)
                    {
                        DataRow row = tmpDataTable.NewRow();
                        //if (IsIncludeInternalID)
                        //    row[FIELD_INTERNAL_ID] = ToHexDecWithCheckDigit(empTermination.EmpTermID);
                        row[FIELD_EMP_NO] = empInfo.EmpNo;

                        if (IsIncludeCurrentPositionInfo)
                        {
                            ImportEmpPersonalInfoProcess.AddEmployeeInfo(dbConn, row, empInfo.EmpID);
                            ImportEmpPositionInfoProcess.AddEmployeePositionInfo(dbConn, row, empInfo.EmpID);
                        }

                        ECessationReason cessationReason = new ECessationReason();
                        cessationReason.CessationReasonID = empTermination.CessationReasonID;
                        if (ECessationReason.db.select(dbConn, cessationReason))
                            row[FIELD_CESSATION_REASON] = IsShowDescription ? cessationReason.CessationReasonDesc : cessationReason.CessationReasonCode;

                        row[FIELD_RESIGN_DATE] = empTermination.EmpTermResignDate;
                        row[FIELD_NOTICE_PERIOD] = empTermination.EmpTermNoticePeriod;

                        if (empTermination.EmpTermNoticeUnit.Equals("D"))
                            row[FIELD_NOTICE_PERIOD_UNIT] = "Day";
                        else if (empTermination.EmpTermNoticeUnit.Equals("M"))
                            row[FIELD_NOTICE_PERIOD_UNIT] = "Month";

                        row[FIELD_LAST_DATE] = empTermination.EmpTermLastDate;
                        row[FIELD_REMARK] = empTermination.EmpTermRemark;

                        row[FIELD_NEW_EMP_NO] = string.Empty;
                        if (empTermination.NewEmpID > 0)
                        {
                            EEmpPersonalInfo newEmp = new EEmpPersonalInfo();
                            newEmp.EmpID = empTermination.NewEmpID;
                            if (EEmpPersonalInfo.db.select(dbConn, newEmp))
                                row[FIELD_NEW_EMP_NO] = newEmp.EmpNo;
                        }
                        if (IsIncludeSyncID)
                            row[FIELD_SYNC_ID] = empTermination.SynID;

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