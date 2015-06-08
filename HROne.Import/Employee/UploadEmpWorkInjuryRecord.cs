using System;
using System.Collections;
using System.Globalization;
using System.Data;
using System.Text;
using HROne.DataAccess;
//using perspectivemind.validation;
using HROne.Lib.Entities;

namespace HROne.Import
{
    public class ImportEmpWorkInjuryRecordProcess : ImportProcessInteface
    {
        [DBClass("UploadEmpWorkInjuryRecord")]
        public class EUploadEmpWorkInjuryRecord : ImportDBObject
        {
            public static DBManager db = new DBManager(typeof(EUploadEmpWorkInjuryRecord));

            protected int m_UploadEmpWorkInjuryRecordID;
            [DBField("UploadEmpWorkInjuryRecordID", true, true), TextSearch, Export(false)]
            public int UploadEmpWorkInjuryRecordID
            {
                get { return m_UploadEmpWorkInjuryRecordID; }
                set { m_UploadEmpWorkInjuryRecordID = value; modify("UploadEmpWorkInjuryRecordID"); }
            }
            protected int m_UploadEmpID;
            [DBField("UploadEmpID"), TextSearch, Export(false)]
            public int UploadEmpID
            {
                get { return m_UploadEmpID; }
                set { m_UploadEmpID = value; modify("UploadEmpID"); }
            }

            protected int m_EmpWorkInjuryRecordID;
            [DBField("EmpWorkInjuryRecordID"), TextSearch, Export(false)]
            public int EmpWorkInjuryRecordID
            {
                get { return m_EmpWorkInjuryRecordID; }
                set { m_EmpWorkInjuryRecordID = value; modify("EmpWorkInjuryRecordID"); }
            }

            protected int m_EmpID;
            [DBField("EmpID"), TextSearch, Export(false), Required]
            public int EmpID
            {
                get { return m_EmpID; }
                set { m_EmpID = value; modify("EmpID"); }
            }

            protected DateTime m_EmpWorkInjuryRecordAccidentDate;
            [DBField("EmpWorkInjuryRecordAccidentDate"), TextSearch, Export(false), Required]
            public DateTime EmpWorkInjuryRecordAccidentDate
            {
                get { return m_EmpWorkInjuryRecordAccidentDate; }
                set { m_EmpWorkInjuryRecordAccidentDate = value; modify("EmpWorkInjuryRecordAccidentDate"); }
            }

            protected string m_EmpWorkInjuryRecordAccidentLocation;
            [DBField("EmpWorkInjuryRecordAccidentLocation"), TextSearch, MaxLength(100, 100), Export(false)]
            public string EmpWorkInjuryRecordAccidentLocation
            {
                get { return m_EmpWorkInjuryRecordAccidentLocation; }
                set { m_EmpWorkInjuryRecordAccidentLocation = value; modify("EmpWorkInjuryRecordAccidentLocation"); }
            }

            protected string m_EmpWorkInjuryRecordAccidentReason;
            [DBField("EmpWorkInjuryRecordAccidentReason"), TextSearch, MaxLength(100, 100), Export(false)]
            public string EmpWorkInjuryRecordAccidentReason
            {
                get { return m_EmpWorkInjuryRecordAccidentReason; }
                set { m_EmpWorkInjuryRecordAccidentReason = value; modify("EmpWorkInjuryRecordAccidentReason"); }
            }

            protected string m_EmpWorkInjuryRecordInjuryNature;
            [DBField("EmpWorkInjuryRecordInjuryNature"), TextSearch, MaxLength(50), Export(false), Required]
            public string EmpWorkInjuryRecordInjuryNature
            {
                get { return m_EmpWorkInjuryRecordInjuryNature; }
                set { m_EmpWorkInjuryRecordInjuryNature = value; modify("EmpWorkInjuryRecordInjuryNature"); }
            }

            protected DateTime m_EmpWorkInjuryRecordReportedDate;
            [DBField("EmpWorkInjuryRecordReportedDate"), TextSearch, Export(false)]
            public DateTime EmpWorkInjuryRecordReportedDate
            {
                get { return m_EmpWorkInjuryRecordReportedDate; }
                set { m_EmpWorkInjuryRecordReportedDate = value; modify("EmpWorkInjuryRecordReportedDate"); }
            }

            protected DateTime m_EmpWorkInjuryRecordChequeReceivedDate;
            [DBField("EmpWorkInjuryRecordChequeReceivedDate"), TextSearch, Export(false)]
            public DateTime EmpWorkInjuryRecordChequeReceivedDate
            {
                get { return m_EmpWorkInjuryRecordChequeReceivedDate; }
                set { m_EmpWorkInjuryRecordChequeReceivedDate = value; modify("EmpWorkInjuryRecordChequeReceivedDate"); }
            }

            protected DateTime m_EmpWorkInjuryRecordSettleDate;
            [DBField("EmpWorkInjuryRecordSettleDate"), TextSearch, Export(false)]
            public DateTime EmpWorkInjuryRecordSettleDate
            {
                get { return m_EmpWorkInjuryRecordSettleDate; }
                set { m_EmpWorkInjuryRecordSettleDate = value; modify("EmpWorkInjuryRecordSettleDate"); }
            }

            protected string m_EmpWorkInjuryRecordRemark;
            [DBField("EmpWorkInjuryRecordRemark"), TextSearch, Export(false)]
            public string EmpWorkInjuryRecordRemark
            {
                get { return m_EmpWorkInjuryRecordRemark; }
                set { m_EmpWorkInjuryRecordRemark = value; modify("EmpWorkInjuryRecordRemark"); }
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



        public const string TABLE_NAME = "injury_record";

        //  Field Const for Employee Personal Information
        private const string FIELD_EMP_NO = "Emp No";
        private const string FIELD_ACCIDENT_DATE = "Accident Date";
        private const string FIELD_ACCIDENT_LOCATION = "Accident Location";
        private const string FIELD_ACCIDENT_REASON = "Accident Reason";
        private const string FIELD_INJURY_NATURE = "Injury Nature";
        private const string FIELD_REPORT_DATE = "Reported Date";
        private const string FIELD_CHEQUE_RECEIVED_DATE = "Cheque Received Date";
        private const string FIELD_SETTLE_DATE = "Settle Date";
        private const string FIELD_REMARK = "Remark";

        
        private DateTime UploadDateTime = AppUtils.ServerDateTime();
        private DBManager tempDB = EUploadEmpWorkInjuryRecord.db;
        private DBManager uploadDB = EEmpWorkInjuryRecord.db;

        public ImportErrorList errors = new ImportErrorList();
        

        public ImportEmpWorkInjuryRecordProcess(DatabaseConnection dbConn, string SessionID): base(dbConn, SessionID)
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

                EUploadEmpWorkInjuryRecord uploadEmpWorkInjuryRecord = new EUploadEmpWorkInjuryRecord();
                //EEmpWorkInjuryRecord lastEmpWorkInjuryRecord = null;

                string EmpNo = row[FIELD_EMP_NO].ToString().Trim();
                uploadEmpWorkInjuryRecord.EmpID = Parse.GetEmpID(dbConn, EmpNo, UserID);
                if (uploadEmpWorkInjuryRecord.EmpID < 0)
                    errors.addError(ImportErrorMessage.ERROR_ACCESS_DENIED_EMP_NO, new string[] { EmpNo, rowCount.ToString() });
                try
                {
                    uploadEmpWorkInjuryRecord.EmpWorkInjuryRecordAccidentDate = Import.Parse.toDateTimeObject(row[FIELD_ACCIDENT_DATE]);
                }
                catch
                {
                    errors.addError(ImportErrorMessage.ERROR_INVALID_FIELD_VALUE, new string[] { FIELD_ACCIDENT_DATE + "=" + row[FIELD_ACCIDENT_DATE].ToString(), EmpNo, rowCount.ToString() });
                }
                uploadEmpWorkInjuryRecord.EmpWorkInjuryRecordAccidentLocation = row[FIELD_ACCIDENT_LOCATION].ToString();
                uploadEmpWorkInjuryRecord.EmpWorkInjuryRecordAccidentReason = row[FIELD_ACCIDENT_REASON].ToString();
                uploadEmpWorkInjuryRecord.EmpWorkInjuryRecordInjuryNature = row[FIELD_INJURY_NATURE].ToString();
                try
                {
                    uploadEmpWorkInjuryRecord.EmpWorkInjuryRecordReportedDate = Import.Parse.toDateTimeObject(row[FIELD_REPORT_DATE]);
                }
                catch
                {
                    errors.addError(ImportErrorMessage.ERROR_INVALID_FIELD_VALUE, new string[] { FIELD_REPORT_DATE + "=" + row[FIELD_REPORT_DATE].ToString(), EmpNo, rowCount.ToString() });
                }
                try
                {
                    uploadEmpWorkInjuryRecord.EmpWorkInjuryRecordChequeReceivedDate = Import.Parse.toDateTimeObject(row[FIELD_CHEQUE_RECEIVED_DATE]);
                }
                catch
                {
                    errors.addError(ImportErrorMessage.ERROR_INVALID_FIELD_VALUE, new string[] { FIELD_CHEQUE_RECEIVED_DATE + "=" + row[FIELD_CHEQUE_RECEIVED_DATE].ToString(), EmpNo, rowCount.ToString() });
                }
                try
                {
                    uploadEmpWorkInjuryRecord.EmpWorkInjuryRecordSettleDate = Import.Parse.toDateTimeObject(row[FIELD_SETTLE_DATE]);
                }
                catch
                {
                    errors.addError(ImportErrorMessage.ERROR_INVALID_FIELD_VALUE, new string[] { FIELD_CHEQUE_RECEIVED_DATE + "=" + row[FIELD_CHEQUE_RECEIVED_DATE].ToString(), EmpNo, rowCount.ToString() });
                }

                uploadEmpWorkInjuryRecord.EmpWorkInjuryRecordRemark = row[FIELD_REMARK].ToString();



                uploadEmpWorkInjuryRecord.SessionID = m_SessionID;
                uploadEmpWorkInjuryRecord.TransactionDate = UploadDateTime;


                if (uploadEmpWorkInjuryRecord.EmpID != 0)
                {
                    //if (rawDataTable.Columns.Contains(FIELD_INTERNAL_ID))
                    //{
                    //    try
                    //    {
                    //        if (!row.IsNull(FIELD_INTERNAL_ID))
                    //        {
                    //            int tmpID = FromHexDecWithCheckDigit((string)row[FIELD_INTERNAL_ID]);
                    //            EEmpWorkInjuryRecord tmpObj = new EEmpWorkInjuryRecord();
                    //            tmpObj.EmpWorkInjuryRecordID = tmpID;
                    //            if (EEmpWorkInjuryRecord.db.select(dbConn, tmpObj))
                    //                uploadEmpWorkInjuryRecord.EmpWorkInjuryRecordID = tmpID;
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
                            uploadEmpWorkInjuryRecord.SynID = strSynID;
                            if (!string.IsNullOrEmpty(strSynID))
                            {
                                DBFilter synIDFilter = new DBFilter();
                                synIDFilter.add(new Match("SynID", strSynID));
                                ArrayList objSameSynIDList = EEmpWorkInjuryRecord.db.select(dbConn, synIDFilter);
                                if (objSameSynIDList.Count > 0)
                                    uploadEmpWorkInjuryRecord.EmpWorkInjuryRecordID = ((EEmpWorkInjuryRecord)objSameSynIDList[0]).EmpWorkInjuryRecordID;
                            }
                        }

                    }

                    if (uploadEmpWorkInjuryRecord.EmpWorkInjuryRecordID == 0)
                    {
                        DBFilter filter = new DBFilter();
                        filter.add(new Match("EmpID", uploadEmpWorkInjuryRecord.EmpID));
                        ArrayList list = uploadDB.select(dbConn, filter);
                        bool IsSameEntryExists = false;
                        foreach (EEmpWorkInjuryRecord currentEmpWorkInjuryRecord in list)
                        {

                            if (uploadEmpWorkInjuryRecord.EmpWorkInjuryRecordAccidentDate == currentEmpWorkInjuryRecord.EmpWorkInjuryRecordAccidentDate
                                && uploadEmpWorkInjuryRecord.EmpWorkInjuryRecordAccidentLocation == currentEmpWorkInjuryRecord.EmpWorkInjuryRecordAccidentLocation
                                && uploadEmpWorkInjuryRecord.EmpWorkInjuryRecordAccidentReason == currentEmpWorkInjuryRecord.EmpWorkInjuryRecordAccidentReason
                                && uploadEmpWorkInjuryRecord.EmpWorkInjuryRecordInjuryNature == currentEmpWorkInjuryRecord.EmpWorkInjuryRecordInjuryNature
                                && uploadEmpWorkInjuryRecord.EmpWorkInjuryRecordReportedDate == currentEmpWorkInjuryRecord.EmpWorkInjuryRecordReportedDate
                                && uploadEmpWorkInjuryRecord.EmpWorkInjuryRecordChequeReceivedDate == currentEmpWorkInjuryRecord.EmpWorkInjuryRecordChequeReceivedDate
                                && uploadEmpWorkInjuryRecord.EmpWorkInjuryRecordSettleDate == currentEmpWorkInjuryRecord.EmpWorkInjuryRecordSettleDate
                                && uploadEmpWorkInjuryRecord.EmpWorkInjuryRecordRemark == currentEmpWorkInjuryRecord.EmpWorkInjuryRecordRemark
                                )
                            {
                                IsSameEntryExists = true;
                                break;
                            }
                        }
                        if (IsSameEntryExists)
                            continue;
                    }
                }

                if (uploadEmpWorkInjuryRecord.EmpWorkInjuryRecordID <= 0)
                    uploadEmpWorkInjuryRecord.ImportActionStatus = ImportDBObject.ImportActionEnum.INSERT;
                else
                    uploadEmpWorkInjuryRecord.ImportActionStatus = ImportDBObject.ImportActionEnum.UPDATE;

                uploadEmpWorkInjuryRecord.UploadEmpID = ParseTemp.GetUploadEmpID(dbConn, EmpNo, m_SessionID);
                if (uploadEmpWorkInjuryRecord.UploadEmpID == 0)
                    if (uploadEmpWorkInjuryRecord.EmpID == 0)
                        errors.addError(ImportErrorMessage.ERROR_INVALID_EMP_NO, new string[] { EmpNo, rowCount.ToString() });
                    else
                        uploadEmpWorkInjuryRecord.UploadEmpID = ImportEmpPersonalInfoProcess.CreateDummyUploadEmployeeInfo(dbConn, uploadEmpWorkInjuryRecord.EmpID, m_SessionID, UploadDateTime);

                Hashtable values = new Hashtable();
                tempDB.populate(uploadEmpWorkInjuryRecord, values);
                PageErrors pageErrors = new PageErrors(EUploadEmpPersonalInfo.db);
                tempDB.validate(pageErrors, values);
                if (pageErrors.errors.Count == 0)
                {

                    tempDB.insert(dbConn, uploadEmpWorkInjuryRecord);
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
            ImportEmpWorkInjuryRecordProcess import = new ImportEmpWorkInjuryRecordProcess(dbConn, sessionID);
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
            ArrayList uploadEmpWorkInjuryRecordList = tempDB.select(dbConn, sessionFilter);
            foreach (EUploadEmpWorkInjuryRecord obj in uploadEmpWorkInjuryRecordList)
            {
                EEmpWorkInjuryRecord EmpWorkInjuryRecord = new EEmpWorkInjuryRecord();

                if (obj.ImportActionStatus != ImportDBObject.ImportActionEnum.INSERT)
                {
                    EmpWorkInjuryRecord.EmpWorkInjuryRecordID = obj.EmpWorkInjuryRecordID;
                    uploadDB.select(dbConn, EmpWorkInjuryRecord);
                }

                obj.ExportToObject(EmpWorkInjuryRecord);

                if (obj.ImportActionStatus == ImportDBObject.ImportActionEnum.INSERT)
                {
                    EmpWorkInjuryRecord.EmpID = ParseTemp.GetEmpIDFromUploadEmpID(dbConn, obj.UploadEmpID);
                    uploadDB.insert(dbConn, EmpWorkInjuryRecord);


                }
                else if (obj.ImportActionStatus == ImportDBObject.ImportActionEnum.UPDATE)
                {
                    uploadDB.update(dbConn, EmpWorkInjuryRecord);
                }
                tempDB.delete(dbConn, obj);
            }
        }

        public static DataTable Export(DatabaseConnection dbConn, ArrayList empList, bool IsIncludeCurrentPositionInfo)
        {
            return Export(dbConn, empList, IsIncludeCurrentPositionInfo, false, new DateTime());
        }
        public static DataTable Export(DatabaseConnection dbConn, ArrayList empList, bool IsIncludeCurrentPositionInfo, bool IsIncludeSyncID, DateTime ReferenceDateTime)
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

            tmpDataTable.Columns.Add(FIELD_ACCIDENT_DATE, typeof(DateTime));
            tmpDataTable.Columns.Add(FIELD_ACCIDENT_LOCATION, typeof(string));
            tmpDataTable.Columns.Add(FIELD_ACCIDENT_REASON, typeof(string));
            tmpDataTable.Columns.Add(FIELD_INJURY_NATURE, typeof(string));
            tmpDataTable.Columns.Add(FIELD_REPORT_DATE, typeof(DateTime));
            tmpDataTable.Columns.Add(FIELD_CHEQUE_RECEIVED_DATE, typeof(DateTime));
            tmpDataTable.Columns.Add(FIELD_SETTLE_DATE, typeof(DateTime));
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
                    ArrayList list = EEmpWorkInjuryRecord.db.select(dbConn, filter);
                    foreach (EEmpWorkInjuryRecord empWorkInjury in list)
                    {
                        DataRow row = tmpDataTable.NewRow();
                        //if (IsIncludeInternalID)
                        //    row[FIELD_INTERNAL_ID] = ToHexDecWithCheckDigit(empWorkInjury.EmpWorkInjuryRecordID);
                        row[FIELD_EMP_NO] = empInfo.EmpNo;

                        if (IsIncludeCurrentPositionInfo)
                        {
                            ImportEmpPersonalInfoProcess.AddEmployeeInfo(dbConn, row, empInfo.EmpID);
                            ImportEmpPositionInfoProcess.AddEmployeePositionInfo(dbConn, row, empInfo.EmpID);
                        }

                        row[FIELD_ACCIDENT_DATE] = empWorkInjury.EmpWorkInjuryRecordAccidentDate;
                        row[FIELD_ACCIDENT_LOCATION] = empWorkInjury.EmpWorkInjuryRecordAccidentLocation;
                        row[FIELD_ACCIDENT_REASON] = empWorkInjury.EmpWorkInjuryRecordAccidentReason;
                        row[FIELD_INJURY_NATURE] = empWorkInjury.EmpWorkInjuryRecordInjuryNature;
                        row[FIELD_REPORT_DATE] = empWorkInjury.EmpWorkInjuryRecordReportedDate;
                        row[FIELD_CHEQUE_RECEIVED_DATE] = empWorkInjury.EmpWorkInjuryRecordReportedDate;
                        row[FIELD_SETTLE_DATE] = empWorkInjury.EmpWorkInjuryRecordSettleDate;
                        row[FIELD_REMARK] = empWorkInjury.EmpWorkInjuryRecordRemark;

                        if (IsIncludeSyncID)
                            row[FIELD_SYNC_ID] = empWorkInjury.SynID;

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
