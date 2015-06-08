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


    public class ImportEmpORSOPlanProcess : ImportProcessInteface
    {
        [DBClass("UploadEmpORSOPlan")]
        private class EUploadEmpORSOPlan : ImportDBObject
        {
            public static DBManager db = new DBManager(typeof(EUploadEmpORSOPlan));

            protected int m_UploadEmpORSOID;
            [DBField("UploadEmpORSOID", true, true), TextSearch, Export(false)]
            public int UploadEmpORSOID
            {
                get { return m_UploadEmpORSOID; }
                set { m_UploadEmpORSOID = value; modify("UploadEmpORSOID"); }
            }
            protected int m_UploadEmpID;
            [DBField("UploadEmpID"), TextSearch, Export(false)]
            public int UploadEmpID
            {
                get { return m_UploadEmpID; }
                set { m_UploadEmpID = value; modify("UploadEmpID"); }
            }

            protected int m_EmpORSOID;
            [DBField("EmpORSOID"), TextSearch, Export(false)]
            public int EmpORSOID
            {
                get { return m_EmpORSOID; }
                set { m_EmpORSOID = value; modify("EmpORSOID"); }
            }
            protected int m_EmpID;
            [DBField("EmpID"), TextSearch, Export(false)]
            public int EmpID
            {
                get { return m_EmpID; }
                set { m_EmpID = value; modify("EmpID"); }
            }
            protected DateTime m_EmpORSOEffFr;
            [DBField("EmpORSOEffFr"), TextSearch, MaxLength(25), Export(false), Required]
            public DateTime EmpORSOEffFr
            {
                get { return m_EmpORSOEffFr; }
                set { m_EmpORSOEffFr = value; modify("EmpORSOEffFr"); }
            }
            protected DateTime m_EmpORSOEffTo;
            [DBField("EmpORSOEffTo"), TextSearch, MaxLength(25), Export(false)]
            public DateTime EmpORSOEffTo
            {
                get { return m_EmpORSOEffTo; }
                set { m_EmpORSOEffTo = value; modify("EmpORSOEffTo"); }
            }
            protected int m_ORSOPlanID;
            [DBField("ORSOPlanID"), TextSearch, Export(false), Required]
            public int ORSOPlanID
            {
                get { return m_ORSOPlanID; }
                set { m_ORSOPlanID = value; modify("ORSOPlanID"); }
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

        public const string TABLE_NAME = "PFund_plan_history";

        //  Field Const for Employee Personal Information
        private const string FIELD_EMP_NO = "Emp No";
        private const string FIELD_FROM = "From";
        private const string FIELD_TO = "To";
        private const string FIELD_ORSOPLAN = "P-Fund Plan Code";

        
        private DateTime UploadDateTime = AppUtils.ServerDateTime();
        private DBManager tempDB = EUploadEmpORSOPlan.db;
        private DBManager uploadDB = EEmpORSOPlan.db;

        public ImportErrorList errors = new ImportErrorList();
        

        public ImportEmpORSOPlanProcess(DatabaseConnection dbConn, string SessionID): base(dbConn, SessionID)
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

                EUploadEmpORSOPlan uploadEmpORSO = new EUploadEmpORSOPlan();
                EEmpORSOPlan lastEmpORSO = null;
                ArrayList uploadHierarchyList = new ArrayList();

                string EmpNo = row[FIELD_EMP_NO].ToString().Trim();
                uploadEmpORSO.EmpID = Parse.GetEmpID(dbConn, EmpNo, UserID);
                if (uploadEmpORSO.EmpID < 0)
                    errors.addError(ImportErrorMessage.ERROR_ACCESS_DENIED_EMP_NO, new string[] { EmpNo, rowCount.ToString() });
                try
                {
                    uploadEmpORSO.EmpORSOEffFr = Parse.toDateTimeObject(row[FIELD_FROM]);
                }
                catch
                {
                    errors.addError(ImportErrorMessage.ERROR_INVALID_FIELD_VALUE, new string[] { FIELD_FROM + "=" + row[FIELD_FROM].ToString(), EmpNo, rowCount.ToString() });
                }
                try
                {
                    uploadEmpORSO.EmpORSOEffTo = Parse.toDateTimeObject(row[FIELD_TO]);
                }
                catch
                {
                    errors.addError(ImportErrorMessage.ERROR_INVALID_FIELD_VALUE, new string[] { FIELD_TO + "=" + row[FIELD_TO].ToString(), EmpNo, rowCount.ToString() });
                }
                uploadEmpORSO.ORSOPlanID = Parse.GetORSOPlanID(dbConn, row[FIELD_ORSOPLAN].ToString(), false, UserID);

                if (uploadEmpORSO.ORSOPlanID == 0)
                    errors.addError(ImportErrorMessage.ERROR_INVALID_FIELD_VALUE, new string[] { FIELD_ORSOPLAN + "=" + row[FIELD_ORSOPLAN].ToString(), EmpNo, rowCount.ToString() });


                uploadEmpORSO.SessionID = m_SessionID;
                uploadEmpORSO.TransactionDate = UploadDateTime;


                if (uploadEmpORSO.EmpID != 0 && errors.List.Count <= 0)
                {
                    //if (rawDataTable.Columns.Contains(FIELD_INTERNAL_ID))
                    //{
                    //    try
                    //    {
                    //        if (!row.IsNull(FIELD_INTERNAL_ID))
                    //        {
                    //            int tmpID = FromHexDecWithCheckDigit((string)row[FIELD_INTERNAL_ID]);
                    //            EEmpORSOPlan tmpObj = new EEmpORSOPlan();
                    //            tmpObj.EmpORSOID = tmpID;
                    //            if (EEmpORSOPlan.db.select(dbConn, tmpObj))
                    //                uploadEmpORSO.EmpORSOID = tmpID;
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
                            uploadEmpORSO.SynID = strSynID;
                            if (!string.IsNullOrEmpty(strSynID))
                            {
                                DBFilter synIDFilter = new DBFilter();
                                synIDFilter.add(new Match("SynID", strSynID));
                                ArrayList objSameSynIDList = EEmpORSOPlan.db.select(dbConn, synIDFilter);
                                if (objSameSynIDList.Count > 0)
                                    uploadEmpORSO.EmpORSOID = ((EEmpORSOPlan)objSameSynIDList[0]).EmpORSOID;
                            }
                        }

                    }
                    if (uploadEmpORSO.EmpORSOID == 0)
                    {
                        lastEmpORSO = (EEmpORSOPlan)AppUtils.GetLastObj(dbConn, uploadDB, "EmpORSOEffFr", uploadEmpORSO.EmpID, new Match("EmpORSOEffFr", "<=", uploadEmpORSO.EmpORSOEffFr));
                        if (lastEmpORSO != null)
                        {

                            if (uploadEmpORSO.ORSOPlanID == lastEmpORSO.ORSOPlanID && uploadEmpORSO.EmpORSOEffTo == lastEmpORSO.EmpORSOEffTo)
                            {
                                continue;
                            }
                            else
                            {
                                // add postion terms with new ID
                                if (lastEmpORSO.EmpORSOEffFr.Equals(uploadEmpORSO.EmpORSOEffFr))
                                {
                                    uploadEmpORSO.EmpORSOID = lastEmpORSO.EmpORSOID;
                                    if (uploadEmpORSO.EmpORSOEffTo.Ticks == 0 && lastEmpORSO.EmpORSOEffTo.Ticks != 0)
                                    {
                                        EEmpORSOPlan afterEmpORSO = (EEmpORSOPlan)AppUtils.GetLastObj(dbConn, uploadDB, "EmpORSOEffFr", uploadEmpORSO.EmpID, new Match("EmpORSOEffFr", ">", lastEmpORSO.EmpORSOEffTo));
                                        if (afterEmpORSO != null)
                                            uploadEmpORSO.EmpORSOEffTo = afterEmpORSO.EmpORSOEffFr.AddDays(-1);
                                    }
                                }
                                else
                                {
                                    AND lastObjAndTerms = new AND();
                                    lastObjAndTerms.add(new Match("EmpORSOEffFr", ">", uploadEmpORSO.EmpORSOEffFr));
                                    if (!uploadEmpORSO.EmpORSOEffTo.Ticks.Equals(0))
                                        lastObjAndTerms.add(new Match("EmpORSOEffFr", "<=", uploadEmpORSO.EmpORSOEffTo));

                                    EEmpORSOPlan lastObj = (EEmpORSOPlan)AppUtils.GetLastObj(dbConn, uploadDB, "EmpORSOEffFr", uploadEmpORSO.EmpID, lastObjAndTerms);
                                    if (lastObj != null)
                                        if (!lastObj.EmpORSOEffTo.Ticks.Equals(0))
                                        {
                                            errors.addError(ImportErrorMessage.ERROR_DATE_FROM_OVERLAP, new string[] { uploadEmpORSO.EmpORSOEffFr.ToString("yyyy-MM-dd"), rowCount.ToString() });
                                            continue;
                                        }

                                }
                            }
                        }
                    }
                }

                if (uploadEmpORSO.EmpORSOID <= 0)
                    uploadEmpORSO.ImportActionStatus = ImportDBObject.ImportActionEnum.INSERT;
                else
                    uploadEmpORSO.ImportActionStatus = ImportDBObject.ImportActionEnum.UPDATE;

                uploadEmpORSO.UploadEmpID = ParseTemp.GetUploadEmpID(dbConn, EmpNo, m_SessionID);
                if (uploadEmpORSO.UploadEmpID == 0)
                    if (uploadEmpORSO.EmpID == 0)
                        errors.addError(ImportErrorMessage.ERROR_INVALID_EMP_NO, new string[] { EmpNo, rowCount.ToString() });
                    else
                        uploadEmpORSO.UploadEmpID = ImportEmpPersonalInfoProcess.CreateDummyUploadEmployeeInfo(dbConn, uploadEmpORSO.EmpID, m_SessionID, UploadDateTime);

                Hashtable values = new Hashtable();
                tempDB.populate(uploadEmpORSO, values);
                PageErrors pageErrors = new PageErrors(EUploadEmpPersonalInfo.db);
                tempDB.validate(pageErrors, values);
                if (pageErrors.errors.Count == 0)
                {

                    tempDB.insert(dbConn, uploadEmpORSO);
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
            ImportEmpORSOPlanProcess import = new ImportEmpORSOPlanProcess(dbConn, sessionID);
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
            ArrayList uploadEmpORSOList = tempDB.select(dbConn, sessionFilter);
            foreach (EUploadEmpORSOPlan obj in uploadEmpORSOList)
            {
                EEmpORSOPlan empORSOPlan = new EEmpORSOPlan();

                if (obj.ImportActionStatus != ImportDBObject.ImportActionEnum.INSERT)
                {
                    empORSOPlan.EmpORSOID = obj.EmpORSOID;
                    uploadDB.select(dbConn, empORSOPlan);
                }

                obj.ExportToObject(empORSOPlan);

                if (obj.ImportActionStatus == ImportDBObject.ImportActionEnum.INSERT)
                {
                    empORSOPlan.EmpID = ParseTemp.GetEmpIDFromUploadEmpID(dbConn, obj.UploadEmpID);
                    DBFilter emplastPosFilter = new DBFilter();
                    EEmpORSOPlan lastObj = (EEmpORSOPlan)AppUtils.GetLastObj(dbConn, uploadDB, "EmpORSOEffFr", empORSOPlan.EmpID, new Match("EmpORSOEffFr", "<", empORSOPlan.EmpORSOEffFr));
                    if (lastObj != null)
                        if (lastObj.EmpORSOEffTo.Ticks == 0)
                        {
                            lastObj.EmpORSOEffTo = empORSOPlan.EmpORSOEffFr.AddDays(-1);
                            uploadDB.update(dbConn, lastObj);
                        }
                    uploadDB.insert(dbConn, empORSOPlan);


                }
                else if (obj.ImportActionStatus == ImportDBObject.ImportActionEnum.UPDATE)
                {
                    uploadDB.update(dbConn, empORSOPlan);
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
            tmpDataTable.Columns.Add(FIELD_ORSOPLAN, typeof(string));
            if (IsIncludeSyncID)
                tmpDataTable.Columns.Add(FIELD_SYNC_ID, typeof(string));

            foreach (EEmpPersonalInfo empInfo in empList)
            {
                if (EEmpPersonalInfo.db.select(dbConn, empInfo))
                {
                    DBFilter filter = new DBFilter();
                    filter.add(new Match("EmpID", empInfo.EmpID));
                    filter.add(getCreateModifiedRecordsAfterDBTerm(ReferenceDateTime));
                    ArrayList list = EEmpORSOPlan.db.select(dbConn, filter);
                    foreach (EEmpORSOPlan empORSOPlan in list)
                    {
                        DataRow row = tmpDataTable.NewRow();
                        //if (IsIncludeInternalID)
                        //    row[FIELD_INTERNAL_ID] = ToHexDecWithCheckDigit(empORSOPlan.EmpORSOID);
                        row[FIELD_EMP_NO] = empInfo.EmpNo;

                        if (IsIncludeCurrentPositionInfo)
                        {
                            ImportEmpPersonalInfoProcess.AddEmployeeInfo(dbConn, row, empInfo.EmpID);
                            ImportEmpPositionInfoProcess.AddEmployeePositionInfo(dbConn, row, empInfo.EmpID);
                        }

                        row[FIELD_FROM] = empORSOPlan.EmpORSOEffFr;
                        row[FIELD_TO] = empORSOPlan.EmpORSOEffTo;

                        EORSOPlan ORSOPlan = new EORSOPlan();
                        ORSOPlan.ORSOPlanID = empORSOPlan.ORSOPlanID;
                        if (EORSOPlan.db.select(dbConn, ORSOPlan))
                            row[FIELD_ORSOPLAN] = IsShowDescription ? ORSOPlan.ORSOPlanDesc : ORSOPlan.ORSOPlanCode;

                        if (IsIncludeSyncID)
                            row[FIELD_SYNC_ID] = empORSOPlan.SynID;

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
