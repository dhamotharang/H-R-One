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


    public class ImportEmpAVCPlanProcess : ImportProcessInteface
    {
        [DBClass("UploadEmpAVCPlan")]
        private class EUploadEmpAVCPlan : ImportDBObject
        {
            public static DBManager db = new DBManager(typeof(EUploadEmpAVCPlan));

            protected int m_UploadEmpAVCID;
            [DBField("UploadEmpAVCID", true, true), TextSearch, Export(false)]
            public int UploadEmpAVCID
            {
                get { return m_UploadEmpAVCID; }
                set { m_UploadEmpAVCID = value; modify("UploadEmpAVCID"); }
            }
            protected int m_UploadEmpID;
            [DBField("UploadEmpID"), TextSearch, Export(false)]
            public int UploadEmpID
            {
                get { return m_UploadEmpID; }
                set { m_UploadEmpID = value; modify("UploadEmpID"); }
            }

            protected int m_EmpAVCID;
            [DBField("EmpAVCID"), TextSearch, Export(false)]
            public int EmpAVCID
            {
                get { return m_EmpAVCID; }
                set { m_EmpAVCID = value; modify("EmpAVCID"); }
            }
            protected int m_EmpID;
            [DBField("EmpID"), TextSearch, Export(false)]
            public int EmpID
            {
                get { return m_EmpID; }
                set { m_EmpID = value; modify("EmpID"); }
            }
            protected DateTime m_EmpAVCEffFr;
            [DBField("EmpAVCEffFr"), TextSearch, MaxLength(25), Export(false), Required]
            public DateTime EmpAVCEffFr
            {
                get { return m_EmpAVCEffFr; }
                set { m_EmpAVCEffFr = value; modify("EmpAVCEffFr"); }
            }
            protected DateTime m_EmpAVCEffTo;
            [DBField("EmpAVCEffTo"), TextSearch, MaxLength(25), Export(false)]
            public DateTime EmpAVCEffTo
            {
                get { return m_EmpAVCEffTo; }
                set { m_EmpAVCEffTo = value; modify("EmpAVCEffTo"); }
            }
            protected int m_AVCPlanID;
            [DBField("AVCPlanID"), TextSearch, Export(false), Required]
            public int AVCPlanID
            {
                get { return m_AVCPlanID; }
                set { m_AVCPlanID = value; modify("AVCPlanID"); }
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

        public const string TABLE_NAME = "AVC_plan_history";

        //  Field Const for Employee Personal Information
        private const string FIELD_EMP_NO = "Emp No";
        private const string FIELD_FROM = "From";
        private const string FIELD_TO = "To";
        private const string FIELD_AVCPLAN = "AVC Plan Code";

        
        private DateTime UploadDateTime = AppUtils.ServerDateTime();
        private DBManager tempDB = EUploadEmpAVCPlan.db;
        private DBManager uploadDB = EEmpAVCPlan.db;

        public ImportErrorList errors = new ImportErrorList();
        

        public ImportEmpAVCPlanProcess(DatabaseConnection dbConn, string SessionID): base(dbConn, SessionID)
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

                EUploadEmpAVCPlan uploadEmpAVC = new EUploadEmpAVCPlan();
                EEmpAVCPlan lastEmpAVC = null;
                ArrayList uploadHierarchyList = new ArrayList();

                string EmpNo = row[FIELD_EMP_NO].ToString().Trim();
                uploadEmpAVC.EmpID = Parse.GetEmpID(dbConn, EmpNo, UserID);
                if (uploadEmpAVC.EmpID < 0)
                    errors.addError(ImportErrorMessage.ERROR_ACCESS_DENIED_EMP_NO, new string[] { EmpNo, rowCount.ToString() });
                try
                {
                    uploadEmpAVC.EmpAVCEffFr = Parse.toDateTimeObject(row[FIELD_FROM]);
                }
                catch
                {
                    errors.addError(ImportErrorMessage.ERROR_INVALID_FIELD_VALUE, new string[] { FIELD_FROM + "=" + row[FIELD_FROM].ToString(), EmpNo, rowCount.ToString() });
                }

                try
                {
                    uploadEmpAVC.EmpAVCEffTo = Parse.toDateTimeObject(row[FIELD_TO]);
                }
                catch
                {
                    errors.addError(ImportErrorMessage.ERROR_INVALID_FIELD_VALUE, new string[] { FIELD_TO + "=" + row[FIELD_TO].ToString(), EmpNo, rowCount.ToString() });
                }
                uploadEmpAVC.AVCPlanID = Parse.GetAVCPlanID(dbConn, row[FIELD_AVCPLAN].ToString(), false, UserID);

                if (uploadEmpAVC.AVCPlanID == 0)
                    errors.addError(ImportErrorMessage.ERROR_INVALID_FIELD_VALUE, new string[] { FIELD_AVCPLAN + "=" + row[FIELD_AVCPLAN].ToString(), EmpNo, rowCount.ToString() });


                uploadEmpAVC.SessionID = m_SessionID;
                uploadEmpAVC.TransactionDate = UploadDateTime;


                if (uploadEmpAVC.EmpID != 0 && errors.List.Count <= 0)
                {
                    //if (rawDataTable.Columns.Contains(FIELD_INTERNAL_ID))
                    //{
                    //    try
                    //    {
                    //        if (!row.IsNull(FIELD_INTERNAL_ID))
                    //        {
                    //            int tmpID = FromHexDecWithCheckDigit((string)row[FIELD_INTERNAL_ID]);
                    //            EEmpAVCPlan tmpObj = new EEmpAVCPlan();
                    //            tmpObj.EmpAVCID = tmpID;
                    //            if (EEmpAVCPlan.db.select(dbConn, tmpObj))
                    //                uploadEmpAVC.EmpAVCID = tmpID;
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
                            uploadEmpAVC.SynID = strSynID;
                            if (!string.IsNullOrEmpty(strSynID))
                            {
                                DBFilter synIDFilter = new DBFilter();
                                synIDFilter.add(new Match("SynID", strSynID));
                                ArrayList objSameSynIDList = EEmpAVCPlan.db.select(dbConn, synIDFilter);
                                if (objSameSynIDList.Count > 0)
                                    uploadEmpAVC.EmpAVCID = ((EEmpAVCPlan)objSameSynIDList[0]).EmpAVCID;
                            }
                        }
                    }

                    if (uploadEmpAVC.EmpAVCID == 0)
                    {
                        lastEmpAVC = (EEmpAVCPlan)AppUtils.GetLastObj(dbConn, uploadDB, "EmpAVCEffFr", uploadEmpAVC.EmpID, new Match("EmpAVCEffFr", "<=", uploadEmpAVC.EmpAVCEffFr));
                        if (lastEmpAVC != null)
                        {

                            if (uploadEmpAVC.AVCPlanID == lastEmpAVC.AVCPlanID && uploadEmpAVC.EmpAVCEffTo == lastEmpAVC.EmpAVCEffTo)
                            {
                                continue;
                            }
                            else
                            {
                                // add postion terms with new ID
                                if (lastEmpAVC.EmpAVCEffFr.Equals(uploadEmpAVC.EmpAVCEffFr))
                                {
                                    uploadEmpAVC.EmpAVCID = lastEmpAVC.EmpAVCID;
                                    if (uploadEmpAVC.EmpAVCEffTo.Ticks == 0 && lastEmpAVC.EmpAVCEffTo.Ticks != 0)
                                    {
                                        EEmpAVCPlan afterEmpAVC = (EEmpAVCPlan)AppUtils.GetLastObj(dbConn, uploadDB, "EmpAVCEffFr", uploadEmpAVC.EmpID, new Match("EmpAVCEffFr", ">", lastEmpAVC.EmpAVCEffTo));
                                        if (afterEmpAVC != null)
                                            uploadEmpAVC.EmpAVCEffTo = afterEmpAVC.EmpAVCEffFr.AddDays(-1);
                                    }
                                }
                                else
                                {
                                    AND lastObjAndTerms = new AND();
                                    lastObjAndTerms.add(new Match("EmpAVCEffFr", ">", uploadEmpAVC.EmpAVCEffFr));
                                    if (!uploadEmpAVC.EmpAVCEffTo.Ticks.Equals(0))
                                        lastObjAndTerms.add(new Match("EmpAVCEffFr", "<=", uploadEmpAVC.EmpAVCEffTo));

                                    EEmpAVCPlan lastObj = (EEmpAVCPlan)AppUtils.GetLastObj(dbConn, uploadDB, "EmpAVCEffFr", uploadEmpAVC.EmpID, lastObjAndTerms);
                                    if (lastObj != null)
                                        if (!lastObj.EmpAVCEffTo.Ticks.Equals(0))
                                        {
                                            errors.addError(ImportErrorMessage.ERROR_DATE_FROM_OVERLAP, new string[] { uploadEmpAVC.EmpAVCEffFr.ToString("yyyy-MM-dd"), rowCount.ToString() });
                                            continue;
                                        }

                                }
                            }
                        }
                    }
                }

                if (uploadEmpAVC.EmpAVCID <= 0)
                    uploadEmpAVC.ImportActionStatus = ImportDBObject.ImportActionEnum.INSERT;
                else
                    uploadEmpAVC.ImportActionStatus = ImportDBObject.ImportActionEnum.UPDATE;

                uploadEmpAVC.UploadEmpID = ParseTemp.GetUploadEmpID(dbConn, EmpNo, m_SessionID);
                if (uploadEmpAVC.UploadEmpID == 0)
                    if (uploadEmpAVC.EmpID == 0)
                        errors.addError(ImportErrorMessage.ERROR_INVALID_EMP_NO, new string[] { EmpNo, rowCount.ToString() });
                    else
                        uploadEmpAVC.UploadEmpID = ImportEmpPersonalInfoProcess.CreateDummyUploadEmployeeInfo(dbConn, uploadEmpAVC.EmpID, m_SessionID, UploadDateTime);

                Hashtable values = new Hashtable();
                tempDB.populate(uploadEmpAVC, values);
                PageErrors pageErrors = new PageErrors(EUploadEmpPersonalInfo.db);
                tempDB.validate(pageErrors, values);
                if (pageErrors.errors.Count == 0)
                {

                    tempDB.insert(dbConn, uploadEmpAVC);
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
            ImportEmpAVCPlanProcess import = new ImportEmpAVCPlanProcess(dbConn, sessionID);
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
            ArrayList uploadEmpAVCList = tempDB.select(dbConn, sessionFilter);
            foreach (EUploadEmpAVCPlan obj in uploadEmpAVCList)
            {
                EEmpAVCPlan empAVCPlan = new EEmpAVCPlan();

                if (obj.ImportActionStatus != ImportDBObject.ImportActionEnum.INSERT)
                {
                    empAVCPlan.EmpAVCID = obj.EmpAVCID;
                    uploadDB.select(dbConn, empAVCPlan);
                }

                obj.ExportToObject(empAVCPlan);

                if (obj.ImportActionStatus == ImportDBObject.ImportActionEnum.INSERT)
                {
                    empAVCPlan.EmpID = ParseTemp.GetEmpIDFromUploadEmpID(dbConn, obj.UploadEmpID);
                    DBFilter emplastPosFilter = new DBFilter();
                    EEmpAVCPlan lastObj = (EEmpAVCPlan)AppUtils.GetLastObj(dbConn, uploadDB, "EmpAVCEffFr", empAVCPlan.EmpID, new Match("EmpAVCEffFr", "<", empAVCPlan.EmpAVCEffFr));
                    if (lastObj != null)
                        if (lastObj.EmpAVCEffTo.Ticks == 0)
                        {
                            lastObj.EmpAVCEffTo = empAVCPlan.EmpAVCEffFr.AddDays(-1);
                            uploadDB.update(dbConn, lastObj);
                        }
                    uploadDB.insert(dbConn, empAVCPlan);


                }
                else if (obj.ImportActionStatus == ImportDBObject.ImportActionEnum.UPDATE)
                {
                    uploadDB.update(dbConn, empAVCPlan);
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
            ////if (IsIncludeInternalID)
            ////    tmpDataTable.Columns.Add(FIELD_INTERNAL_ID, typeof(string));
            tmpDataTable.Columns.Add(FIELD_EMP_NO, typeof(string));

            if (IsIncludeCurrentPositionInfo)
            {
                ImportEmpPersonalInfoProcess.AddEmployeeInfoHeader(tmpDataTable);
                ImportEmpPositionInfoProcess.AddEmployeePositionInfoHeader(dbConn, tmpDataTable);
            }
            tmpDataTable.Columns.Add(FIELD_FROM, typeof(DateTime));
            tmpDataTable.Columns.Add(FIELD_TO, typeof(DateTime));
            tmpDataTable.Columns.Add(FIELD_AVCPLAN, typeof(string));
            if (IsIncludeSyncID)
                tmpDataTable.Columns.Add(FIELD_SYNC_ID, typeof(string));

            foreach (EEmpPersonalInfo empInfo in empList)
            {
                if (EEmpPersonalInfo.db.select(dbConn, empInfo))
                {
                    DBFilter filter = new DBFilter();
                    filter.add(new Match("EmpID", empInfo.EmpID));
                    filter.add(getCreateModifiedRecordsAfterDBTerm(ReferenceDateTime));
                    ArrayList list = EEmpAVCPlan.db.select(dbConn, filter);
                    foreach (EEmpAVCPlan empAVCPlan in list)
                    {
                        DataRow row = tmpDataTable.NewRow();
                        //if (IsIncludeInternalID)
                        //    row[FIELD_INTERNAL_ID] = ToHexDecWithCheckDigit(empAVCPlan.EmpAVCID);
                        row[FIELD_EMP_NO] = empInfo.EmpNo;

                        if (IsIncludeCurrentPositionInfo)
                        {
                            ImportEmpPersonalInfoProcess.AddEmployeeInfo(dbConn, row, empInfo.EmpID);
                            ImportEmpPositionInfoProcess.AddEmployeePositionInfo(dbConn, row, empInfo.EmpID);
                        }

                        row[FIELD_FROM] = empAVCPlan.EmpAVCEffFr;
                        row[FIELD_TO] = empAVCPlan.EmpAVCEffTo;

                        EAVCPlan avcPlan = new EAVCPlan();
                        avcPlan.AVCPlanID = empAVCPlan.AVCPlanID;
                        if (EAVCPlan.db.select(dbConn, avcPlan))
                            row[FIELD_AVCPLAN] = IsShowDescription ? avcPlan.AVCPlanDesc : avcPlan.AVCPlanCode;

                        if (IsIncludeSyncID)
                            row[FIELD_SYNC_ID] = empAVCPlan.SynID;

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
