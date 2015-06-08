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


    public class ImportEmpRosterTableGroupProcess : ImportProcessInteface
    {
        [DBClass("UploadEmpRosterTableGroup")]
        private class EUploadEmpRosterTableGroup : ImportDBObject
        {
            public static DBManager db = new DBManager(typeof(EUploadEmpRosterTableGroup));

            protected int m_UploadEmpRosterTableGroupID;
            [DBField("UploadEmpRosterTableGroupID", true, true), TextSearch, Export(false)]
            public int UploadEmpRosterTableGroupID
            {
                get { return m_UploadEmpRosterTableGroupID; }
                set { m_UploadEmpRosterTableGroupID = value; modify("UploadEmpRosterTableGroupID"); }
            }
            protected int m_UploadEmpID;
            [DBField("UploadEmpID"), TextSearch, Export(false)]
            public int UploadEmpID
            {
                get { return m_UploadEmpID; }
                set { m_UploadEmpID = value; modify("UploadEmpID"); }
            }

            protected int m_EmpRosterTableGroupID;
            [DBField("EmpRosterTableGroupID"), TextSearch, Export(false)]
            public int EmpRosterTableGroupID
            {
                get { return m_EmpRosterTableGroupID; }
                set { m_EmpRosterTableGroupID = value; modify("EmpRosterTableGroupID"); }
            }
            protected int m_EmpID;
            [DBField("EmpID"), TextSearch, Export(false)]
            public int EmpID
            {
                get { return m_EmpID; }
                set { m_EmpID = value; modify("EmpID"); }
            }
            protected DateTime m_EmpRosterTableGroupEffFr;
            [DBField("EmpRosterTableGroupEffFr"), TextSearch, MaxLength(25), Export(false), Required]
            public DateTime EmpRosterTableGroupEffFr
            {
                get { return m_EmpRosterTableGroupEffFr; }
                set { m_EmpRosterTableGroupEffFr = value; modify("EmpRosterTableGroupEffFr"); }
            }
            protected DateTime m_EmpRosterTableGroupEffTo;
            [DBField("EmpRosterTableGroupEffTo"), TextSearch, MaxLength(25), Export(false)]
            public DateTime EmpRosterTableGroupEffTo
            {
                get { return m_EmpRosterTableGroupEffTo; }
                set { m_EmpRosterTableGroupEffTo = value; modify("EmpRosterTableGroupEffTo"); }
            }
            protected int m_RosterTableGroupID;
            [DBField("RosterTableGroupID"), TextSearch, Export(false), Required]
            public int RosterTableGroupID
            {
                get { return m_RosterTableGroupID; }
                set { m_RosterTableGroupID = value; modify("RosterTableGroupID"); }
            }
            protected bool m_EmpRosterTableGroupIsSupervisor;
            [DBField("EmpRosterTableGroupIsSupervisor"), TextSearch, Export(false), Required]
            public bool EmpRosterTableGroupIsSupervisor
            {
                get { return m_EmpRosterTableGroupIsSupervisor; }
                set { m_EmpRosterTableGroupIsSupervisor = value; modify("EmpRosterTableGroupIsSupervisor"); }
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

        public const string TABLE_NAME = "RosterTableGroup";

        //  Field Const for Employee Personal Information
        private const string FIELD_EMP_NO = "Emp No";
        private const string FIELD_FROM = "From";
        private const string FIELD_TO = "To";
        private const string FIELD_ROSTERTABLEGROUP = "Roster Table Group";
        private const string FIELD_ROSTERTABLEGROUP_IS_SUPERVISOR = "Is Supervisor";

        
        private DateTime UploadDateTime = AppUtils.ServerDateTime();
        private DBManager tempDB = EUploadEmpRosterTableGroup.db;
        private DBManager uploadDB = EEmpRosterTableGroup.db;

        public ImportErrorList errors = new ImportErrorList();
        

        public ImportEmpRosterTableGroupProcess(DatabaseConnection dbConn, string SessionID): base(dbConn, SessionID)
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

                EUploadEmpRosterTableGroup uploadEmpRosterTableGroup = new EUploadEmpRosterTableGroup();
                EEmpRosterTableGroup lastEmpRosterTableGroup = null;
                ArrayList uploadHierarchyList = new ArrayList();

                string EmpNo = row[FIELD_EMP_NO].ToString().Trim();
                uploadEmpRosterTableGroup.EmpID = Parse.GetEmpID(dbConn, EmpNo, UserID);
                if (uploadEmpRosterTableGroup.EmpID < 0)
                    errors.addError(ImportErrorMessage.ERROR_ACCESS_DENIED_EMP_NO, new string[] { EmpNo, rowCount.ToString() });
                try
                {
                    uploadEmpRosterTableGroup.EmpRosterTableGroupEffFr = Parse.toDateTimeObject(row[FIELD_FROM]);
                }
                catch
                {
                    errors.addError(ImportErrorMessage.ERROR_INVALID_FIELD_VALUE, new string[] { FIELD_FROM + "=" + row[FIELD_FROM].ToString(), EmpNo, rowCount.ToString() });
                }
                try
                {
                    uploadEmpRosterTableGroup.EmpRosterTableGroupEffTo = Parse.toDateTimeObject(row[FIELD_TO]);
                }
                catch
                {
                    errors.addError(ImportErrorMessage.ERROR_INVALID_FIELD_VALUE, new string[] { FIELD_TO + "=" + row[FIELD_TO].ToString(), EmpNo, rowCount.ToString() });
                }
                uploadEmpRosterTableGroup.RosterTableGroupID = Parse.GetRosterTableGroupID(dbConn, row[FIELD_ROSTERTABLEGROUP].ToString());
                uploadEmpRosterTableGroup.EmpRosterTableGroupIsSupervisor = row[FIELD_ROSTERTABLEGROUP_IS_SUPERVISOR].ToString().Equals("Yes", StringComparison.CurrentCultureIgnoreCase) || row[FIELD_ROSTERTABLEGROUP_IS_SUPERVISOR].ToString().Equals("Y", StringComparison.CurrentCultureIgnoreCase);

                if (uploadEmpRosterTableGroup.RosterTableGroupID == 0)
                    errors.addError(ImportErrorMessage.ERROR_INVALID_FIELD_VALUE, new string[] { FIELD_ROSTERTABLEGROUP + "=" + row[FIELD_ROSTERTABLEGROUP].ToString(), EmpNo, rowCount.ToString() });


                uploadEmpRosterTableGroup.SessionID = m_SessionID;
                uploadEmpRosterTableGroup.TransactionDate = UploadDateTime;


                if (uploadEmpRosterTableGroup.EmpID != 0 && errors.List.Count <= 0)
                {
                    //if (rawDataTable.Columns.Contains(FIELD_INTERNAL_ID))
                    //{
                    //    try
                    //    {
                    //        if (!row.IsNull(FIELD_INTERNAL_ID))
                    //        {
                    //            int tmpID = FromHexDecWithCheckDigit((string)row[FIELD_INTERNAL_ID]);
                    //            EEmpRosterTableGroup tmpObj = new EEmpRosterTableGroup();
                    //            tmpObj.EmpRosterTableGroupID = tmpID;
                    //            if (EEmpRosterTableGroup.db.select(dbConn, tmpObj))
                    //                uploadEmpRosterTableGroup.EmpRosterTableGroupID = tmpID;
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
                            uploadEmpRosterTableGroup.SynID = strSynID;
                            if (!string.IsNullOrEmpty(strSynID))
                            {
                                DBFilter synIDFilter = new DBFilter();
                                synIDFilter.add(new Match("SynID", strSynID));
                                ArrayList objSameSynIDList = EEmpRosterTableGroup.db.select(dbConn, synIDFilter);
                                if (objSameSynIDList.Count > 0)
                                    uploadEmpRosterTableGroup.EmpRosterTableGroupID = ((EEmpRosterTableGroup)objSameSynIDList[0]).EmpRosterTableGroupID;
                            }
                        }

                    }

                    if (uploadEmpRosterTableGroup.EmpRosterTableGroupID == 0)
                    {
                        // allow multiple group on same date
                        // do NOT allow same group id appear on same date

                        AND lastEmpRosterTableGroupAndTerms = new AND();
                        lastEmpRosterTableGroupAndTerms.add(new Match("EmpRosterTableGroupEffFr", "<=", uploadEmpRosterTableGroup.EmpRosterTableGroupEffFr));
                        lastEmpRosterTableGroupAndTerms.add(new Match("RosterTableGroupID", uploadEmpRosterTableGroup.RosterTableGroupID));

                        lastEmpRosterTableGroup = (EEmpRosterTableGroup)AppUtils.GetLastObj(dbConn, uploadDB, "EmpRosterTableGroupEffFr", uploadEmpRosterTableGroup.EmpID, lastEmpRosterTableGroupAndTerms);
                        if (lastEmpRosterTableGroup != null)
                        {

                            if (uploadEmpRosterTableGroup.RosterTableGroupID == lastEmpRosterTableGroup.RosterTableGroupID
                                && uploadEmpRosterTableGroup.EmpRosterTableGroupIsSupervisor == lastEmpRosterTableGroup.EmpRosterTableGroupIsSupervisor
                                && uploadEmpRosterTableGroup.EmpRosterTableGroupEffTo == lastEmpRosterTableGroup.EmpRosterTableGroupEffTo)
                            {
                                continue;
                            }
                            else
                            {
                                // add postion terms with new ID
                                if (lastEmpRosterTableGroup.EmpRosterTableGroupEffFr.Equals(uploadEmpRosterTableGroup.EmpRosterTableGroupEffFr)
                                && uploadEmpRosterTableGroup.RosterTableGroupID == lastEmpRosterTableGroup.RosterTableGroupID
                                && uploadEmpRosterTableGroup.EmpRosterTableGroupIsSupervisor == lastEmpRosterTableGroup.EmpRosterTableGroupIsSupervisor)
                                {
                                    uploadEmpRosterTableGroup.EmpRosterTableGroupID = lastEmpRosterTableGroup.EmpRosterTableGroupID;
                                    //if (uploadEmpRosterTableGroup.EmpRosterTableGroupEffTo.Ticks == 0 && lastEmpRosterTableGroup.EmpRosterTableGroupEffTo.Ticks != 0)
                                    //{
                                    //    EEmpRosterTableGroup afterEmpRosterTableGroup = (EEmpRosterTableGroup)AppUtils.GetLastObj(dbConn, uploadDB, "EmpRosterTableGroupEffFr", uploadEmpRosterTableGroup.EmpID, new Match("EmpRosterTableGroupEffFr", ">", lastEmpRosterTableGroup.EmpRosterTableGroupEffTo));
                                    //    if (afterEmpRosterTableGroup != null)
                                    //        uploadEmpRosterTableGroup.EmpRosterTableGroupEffTo = afterEmpRosterTableGroup.EmpRosterTableGroupEffFr.AddDays(-1);
                                    //}
                                }
                                //else
                                //{
                                //    AND lastObjAndTerms = new AND();
                                //    lastObjAndTerms.add(new Match("EmpRosterTableGroupEffFr", ">", uploadEmpRosterTableGroup.EmpRosterTableGroupEffFr));
                                //    if (!uploadEmpRosterTableGroup.EmpRosterTableGroupEffTo.Ticks.Equals(0))
                                //        lastObjAndTerms.add(new Match("EmpRosterTableGroupEffFr", "<=", uploadEmpRosterTableGroup.EmpRosterTableGroupEffTo));
                                //    lastObjAndTerms.add(new Match("RosterTableGroupID", uploadEmpRosterTableGroup.RosterTableGroupID));
                                //    EEmpRosterTableGroup lastObj = (EEmpRosterTableGroup)AppUtils.GetLastObj(dbConn, uploadDB, "EmpRosterTableGroupEffFr", uploadEmpRosterTableGroup.EmpID, lastObjAndTerms);
                                //    if (lastObj != null)
                                //        if (!lastObj.EmpRosterTableGroupEffTo.Ticks.Equals(0))
                                //        {
                                //            errors.addError(ImportErrorMessage.ERROR_DATE_FROM_OVERLAP, new string[] { uploadEmpRosterTableGroup.EmpRosterTableGroupEffFr.ToString("yyyy-MM-dd"), rowCount.ToString() });
                                //            continue;
                                //        }

                                //}
                            }
                        }
                    }
                }

                if (uploadEmpRosterTableGroup.EmpRosterTableGroupID <= 0)
                    uploadEmpRosterTableGroup.ImportActionStatus = ImportDBObject.ImportActionEnum.INSERT;
                else
                    uploadEmpRosterTableGroup.ImportActionStatus = ImportDBObject.ImportActionEnum.UPDATE;

                uploadEmpRosterTableGroup.UploadEmpID = ParseTemp.GetUploadEmpID(dbConn, EmpNo, m_SessionID);
                if (uploadEmpRosterTableGroup.UploadEmpID == 0)
                    if (uploadEmpRosterTableGroup.EmpID == 0)
                        errors.addError(ImportErrorMessage.ERROR_INVALID_EMP_NO, new string[] { EmpNo, rowCount.ToString() });
                    else
                        uploadEmpRosterTableGroup.UploadEmpID = ImportEmpPersonalInfoProcess.CreateDummyUploadEmployeeInfo(dbConn, uploadEmpRosterTableGroup.EmpID, m_SessionID, UploadDateTime);

                Hashtable values = new Hashtable();
                tempDB.populate(uploadEmpRosterTableGroup, values);
                PageErrors pageErrors = new PageErrors(EUploadEmpPersonalInfo.db);
                tempDB.validate(pageErrors, values);
                if (pageErrors.errors.Count == 0)
                {

                    tempDB.insert(dbConn, uploadEmpRosterTableGroup);
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
            if (info != null && info.orderby != null && !info.orderby.Equals(""))
                sessionFilter.add(info.orderby, info.order);
            DataTable table = sessionFilter.loadData(dbConn, info, " c.* ", " from " + tempDB.dbclass.tableName + " c ");


            return table;

        }

        public static void ClearTempTable(DatabaseConnection dbConn, string sessionID)
        {
            ImportEmpRosterTableGroupProcess import = new ImportEmpRosterTableGroupProcess(dbConn, sessionID);
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
            ArrayList uploadEmpRosterTableGroupList = tempDB.select(dbConn, sessionFilter);
            foreach (EUploadEmpRosterTableGroup obj in uploadEmpRosterTableGroupList)
            {
                EEmpRosterTableGroup EmpRosterTableGroup = new EEmpRosterTableGroup();

                if (obj.ImportActionStatus != ImportDBObject.ImportActionEnum.INSERT)
                {
                    EmpRosterTableGroup.EmpRosterTableGroupID = obj.EmpRosterTableGroupID;
                    uploadDB.select(dbConn, EmpRosterTableGroup);
                }

                obj.ExportToObject(EmpRosterTableGroup);

                if (obj.ImportActionStatus == ImportDBObject.ImportActionEnum.INSERT)
                {
                    EmpRosterTableGroup.EmpID = ParseTemp.GetEmpIDFromUploadEmpID(dbConn, obj.UploadEmpID);
                    //DBFilter emplastPosFilter = new DBFilter();
                    //EEmpRosterTableGroup lastObj = (EEmpRosterTableGroup)AppUtils.GetLastObj(dbConn, uploadDB, "EmpRosterTableGroupEffFr", EmpRosterTableGroup.EmpID, new Match("EmpRosterTableGroupEffFr", "<", EmpRosterTableGroup.EmpRosterTableGroupEffFr));
                    //if (lastObj != null)
                    //    if (lastObj.EmpRosterTableGroupEffTo.Ticks == 0)
                    //    {
                    //        lastObj.EmpRosterTableGroupEffTo = EmpRosterTableGroup.EmpRosterTableGroupEffFr.AddDays(-1);
                    //        uploadDB.update(dbConn, lastObj);
                    //    }
                    uploadDB.insert(dbConn, EmpRosterTableGroup);


                }
                else if (obj.ImportActionStatus == ImportDBObject.ImportActionEnum.UPDATE)
                {
                    uploadDB.update(dbConn, EmpRosterTableGroup);
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

            tmpDataTable.Columns.Add(FIELD_FROM, typeof(DateTime));
            tmpDataTable.Columns.Add(FIELD_TO, typeof(DateTime));
            tmpDataTable.Columns.Add(FIELD_ROSTERTABLEGROUP, typeof(string));
            tmpDataTable.Columns.Add(FIELD_ROSTERTABLEGROUP_IS_SUPERVISOR, typeof(string));
            if (IsIncludeSyncID)
                tmpDataTable.Columns.Add(FIELD_SYNC_ID, typeof(string));

            foreach (EEmpPersonalInfo empInfo in empList)
            {
                if (EEmpPersonalInfo.db.select(dbConn, empInfo))
                {
                    DBFilter filter = new DBFilter();
                    filter.add(new Match("EmpID", empInfo.EmpID));
                    filter.add(getCreateModifiedRecordsAfterDBTerm(ReferenceDateTime));
                    ArrayList list = EEmpRosterTableGroup.db.select(dbConn, filter);
                    foreach (EEmpRosterTableGroup EmpRosterTableGroup in list)
                    {
                        DataRow row = tmpDataTable.NewRow();
                        //if (IsIncludeInternalID)
                        //    row[FIELD_INTERNAL_ID] = ToHexDecWithCheckDigit(EmpRosterTableGroup.EmpRosterTableGroupID);
                        row[FIELD_EMP_NO] = empInfo.EmpNo;

                        if (IsIncludeCurrentPositionInfo)
                        {
                            ImportEmpPersonalInfoProcess.AddEmployeeInfo(dbConn, row, empInfo.EmpID);
                            ImportEmpPositionInfoProcess.AddEmployeePositionInfo(dbConn, row, empInfo.EmpID);
                        }

                        row[FIELD_FROM] = EmpRosterTableGroup.EmpRosterTableGroupEffFr;
                        row[FIELD_TO] = EmpRosterTableGroup.EmpRosterTableGroupEffTo;

                        ERosterTableGroup rosterTableGroup = new ERosterTableGroup();
                        rosterTableGroup.RosterTableGroupID = EmpRosterTableGroup.RosterTableGroupID;
                        if (ERosterTableGroup.db.select(dbConn, rosterTableGroup))
                            row[FIELD_ROSTERTABLEGROUP] = IsShowDescription ? rosterTableGroup.RosterTableGroupDesc : rosterTableGroup.RosterTableGroupCode;

                        row[FIELD_ROSTERTABLEGROUP_IS_SUPERVISOR] = EmpRosterTableGroup.EmpRosterTableGroupIsSupervisor ? "Yes" : "No";
                        //row[FIELD_IS_ROSTER_TABLE_GROUP_SUPERVISOR] = empPositionInfo.EmpPosIsRosterTableGroupSupervisor ? "Yes" : "No";
                        if (IsIncludeSyncID)
                            row[FIELD_SYNC_ID] = EmpRosterTableGroup.SynID;

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
