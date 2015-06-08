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


    public class ImportEmpMPFPlanProcess : ImportProcessInteface
    {
        [DBClass("UploadEmpMPFPlan")]
        private class EUploadEmpMPFPlan : ImportDBObject
        {
            public static DBManager db = new DBManager(typeof(EUploadEmpMPFPlan));

            protected int m_UploadEmpMPFID;
            [DBField("UploadEmpMPFID", true, true), TextSearch, Export(false)]
            public int UploadEmpMPFID
            {
                get { return m_UploadEmpMPFID; }
                set { m_UploadEmpMPFID = value; modify("UploadEmpMPFID"); }
            }
            protected int m_UploadEmpID;
            [DBField("UploadEmpID"), TextSearch, Export(false)]
            public int UploadEmpID
            {
                get { return m_UploadEmpID; }
                set { m_UploadEmpID = value; modify("UploadEmpID"); }
            }

            protected int m_EmpMPFID;
            [DBField("EmpMPFID"), TextSearch, Export(false)]
            public int EmpMPFID
            {
                get { return m_EmpMPFID; }
                set { m_EmpMPFID = value; modify("EmpMPFID"); }
            }
            protected int m_EmpID;
            [DBField("EmpID"), TextSearch, Export(false)]
            public int EmpID
            {
                get { return m_EmpID; }
                set { m_EmpID = value; modify("EmpID"); }
            }
            protected DateTime m_EmpMPFEffFr;
            [DBField("EmpMPFEffFr"), TextSearch, MaxLength(25), Export(false), Required]
            public DateTime EmpMPFEffFr
            {
                get { return m_EmpMPFEffFr; }
                set { m_EmpMPFEffFr = value; modify("EmpMPFEffFr"); }
            }
            protected DateTime m_EmpMPFEffTo;
            [DBField("EmpMPFEffTo"), TextSearch, MaxLength(25), Export(false)]
            public DateTime EmpMPFEffTo
            {
                get { return m_EmpMPFEffTo; }
                set { m_EmpMPFEffTo = value; modify("EmpMPFEffTo"); }
            }
            protected int m_MPFPlanID;
            [DBField("MPFPlanID"), TextSearch, Export(false), Required]
            public int MPFPlanID
            {
                get { return m_MPFPlanID; }
                set { m_MPFPlanID = value; modify("MPFPlanID"); }
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

        public const string TABLE_NAME = "mpf_plan_history";

        //  Field Const for Employee Personal Information
        private const string FIELD_EMP_NO = "Emp No";
        private const string FIELD_FROM = "From";
        private const string FIELD_TO = "To";
        private const string FIELD_MPFPLAN = "MPF Plan Code";

        
        private DateTime UploadDateTime = AppUtils.ServerDateTime();
        private DBManager tempDB = EUploadEmpMPFPlan.db;
        private DBManager uploadDB = EEmpMPFPlan.db;

        public ImportErrorList errors = new ImportErrorList();
        

        public ImportEmpMPFPlanProcess(DatabaseConnection dbConn, string SessionID): base(dbConn, SessionID)
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

                EUploadEmpMPFPlan uploadEmpMPF = new EUploadEmpMPFPlan();
                EEmpMPFPlan lastEmpMPF = null;
                ArrayList uploadHierarchyList = new ArrayList();

                string EmpNo = row[FIELD_EMP_NO].ToString().Trim();
                uploadEmpMPF.EmpID = Parse.GetEmpID(dbConn, EmpNo, UserID);
                if (uploadEmpMPF.EmpID < 0)
                    errors.addError(ImportErrorMessage.ERROR_ACCESS_DENIED_EMP_NO, new string[] { EmpNo, rowCount.ToString() });
                try
                {
                    uploadEmpMPF.EmpMPFEffFr = Parse.toDateTimeObject(row[FIELD_FROM]);
                }
                catch
                {
                    errors.addError(ImportErrorMessage.ERROR_INVALID_FIELD_VALUE, new string[] { FIELD_FROM + "=" + row[FIELD_FROM].ToString(), EmpNo, rowCount.ToString() });
                }
                try
                {
                    uploadEmpMPF.EmpMPFEffTo = Parse.toDateTimeObject(row[FIELD_TO]);
                }
                catch
                {
                    errors.addError(ImportErrorMessage.ERROR_INVALID_FIELD_VALUE, new string[] { FIELD_TO + "=" + row[FIELD_TO].ToString(), EmpNo, rowCount.ToString() });
                }
                uploadEmpMPF.MPFPlanID = Parse.GetMPFPlanID(dbConn, row[FIELD_MPFPLAN].ToString(), false, UserID);

                if (uploadEmpMPF.MPFPlanID == 0)
                    errors.addError(ImportErrorMessage.ERROR_INVALID_FIELD_VALUE, new string[] { FIELD_MPFPLAN + "=" + row[FIELD_MPFPLAN].ToString(), EmpNo, rowCount.ToString() });


                uploadEmpMPF.SessionID = m_SessionID;
                uploadEmpMPF.TransactionDate = UploadDateTime;


                if (uploadEmpMPF.EmpID != 0 && errors.List.Count<=0)
                {
                    //if (rawDataTable.Columns.Contains(FIELD_INTERNAL_ID))
                    //{
                    //    try
                    //    {
                    //        if (!row.IsNull(FIELD_INTERNAL_ID))
                    //        {
                    //            int tmpID = FromHexDecWithCheckDigit((string)row[FIELD_INTERNAL_ID]);
                    //            EEmpMPFPlan tmpObj = new EEmpMPFPlan();
                    //            tmpObj.EmpMPFID = tmpID;
                    //            if (EEmpMPFPlan.db.select(dbConn, tmpObj))
                    //                uploadEmpMPF.EmpMPFID = tmpID;
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
                            uploadEmpMPF.SynID = strSynID;
                            if (!string.IsNullOrEmpty(strSynID))
                            {
                                DBFilter synIDFilter = new DBFilter();
                                synIDFilter.add(new Match("SynID", strSynID));
                                ArrayList objSameSynIDList = EEmpMPFPlan.db.select(dbConn, synIDFilter);
                                if (objSameSynIDList.Count > 0)
                                    uploadEmpMPF.EmpMPFID = ((EEmpMPFPlan)objSameSynIDList[0]).EmpMPFID;
                            }
                        }

                    }


                    if (uploadEmpMPF.EmpMPFID == 0)
                    {
                        lastEmpMPF = (EEmpMPFPlan)AppUtils.GetLastObj(dbConn, uploadDB, "EmpMPFEffFr", uploadEmpMPF.EmpID, new Match("EmpMPFEffFr", "<=", uploadEmpMPF.EmpMPFEffFr));
                        if (lastEmpMPF != null)
                        {

                            if (uploadEmpMPF.MPFPlanID == lastEmpMPF.MPFPlanID && uploadEmpMPF.EmpMPFEffTo == lastEmpMPF.EmpMPFEffTo)
                            {
                                continue;
                            }
                            else
                            {
                                // add postion terms with new ID
                                if (lastEmpMPF.EmpMPFEffFr.Equals(uploadEmpMPF.EmpMPFEffFr))
                                {
                                    uploadEmpMPF.EmpMPFID = lastEmpMPF.EmpMPFID;
                                    if (uploadEmpMPF.EmpMPFEffTo.Ticks == 0 && lastEmpMPF.EmpMPFEffTo.Ticks != 0)
                                    {
                                        EEmpMPFPlan afterEmpMPF = (EEmpMPFPlan)AppUtils.GetLastObj(dbConn, uploadDB, "EmpMPFEffFr", uploadEmpMPF.EmpID, new Match("EmpMPFEffFr", ">", lastEmpMPF.EmpMPFEffTo));
                                        if (afterEmpMPF != null)
                                            uploadEmpMPF.EmpMPFEffTo = afterEmpMPF.EmpMPFEffFr.AddDays(-1);
                                    }
                                }
                                else
                                {
                                    AND lastObjAndTerms = new AND();
                                    lastObjAndTerms.add(new Match("EmpMPFEffFr", ">", uploadEmpMPF.EmpMPFEffFr));
                                    if (!uploadEmpMPF.EmpMPFEffTo.Ticks.Equals(0))
                                        lastObjAndTerms.add(new Match("EmpMPFEffFr", "<=", uploadEmpMPF.EmpMPFEffTo));

                                    EEmpMPFPlan lastObj = (EEmpMPFPlan)AppUtils.GetLastObj(dbConn, uploadDB, "EmpMPFEffFr", uploadEmpMPF.EmpID, lastObjAndTerms);
                                    if (lastObj != null)
                                        if (!lastObj.EmpMPFEffTo.Ticks.Equals(0))
                                        {
                                            errors.addError(ImportErrorMessage.ERROR_DATE_FROM_OVERLAP, new string[] { uploadEmpMPF.EmpMPFEffFr.ToString("yyyy-MM-dd"), rowCount.ToString() });
                                            continue;
                                        }

                                }
                            }
                        }
                    }
                }

                if (uploadEmpMPF.EmpMPFID <= 0)
                    uploadEmpMPF.ImportActionStatus = ImportDBObject.ImportActionEnum.INSERT;
                else
                    uploadEmpMPF.ImportActionStatus = ImportDBObject.ImportActionEnum.UPDATE;

                uploadEmpMPF.UploadEmpID = ParseTemp.GetUploadEmpID(dbConn, EmpNo, m_SessionID);
                if (uploadEmpMPF.UploadEmpID == 0)
                    if (uploadEmpMPF.EmpID == 0)
                        errors.addError(ImportErrorMessage.ERROR_INVALID_EMP_NO, new string[] { EmpNo, rowCount.ToString() });
                    else
                        uploadEmpMPF.UploadEmpID = ImportEmpPersonalInfoProcess.CreateDummyUploadEmployeeInfo(dbConn, uploadEmpMPF.EmpID, m_SessionID, UploadDateTime);

                Hashtable values = new Hashtable();
                tempDB.populate(uploadEmpMPF, values);
                PageErrors pageErrors = new PageErrors(EUploadEmpPersonalInfo.db);
                tempDB.validate(pageErrors, values);
                if (pageErrors.errors.Count == 0)
                {

                    tempDB.insert(dbConn, uploadEmpMPF);
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
            ImportEmpMPFPlanProcess import = new ImportEmpMPFPlanProcess(dbConn, sessionID);
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
            ArrayList uploadEmpMPFList = tempDB.select(dbConn, sessionFilter);
            foreach (EUploadEmpMPFPlan obj in uploadEmpMPFList)
            {
                EEmpMPFPlan empMPFPlan = new EEmpMPFPlan();

                if (obj.ImportActionStatus != ImportDBObject.ImportActionEnum.INSERT)
                {
                    empMPFPlan.EmpMPFID = obj.EmpMPFID;
                    uploadDB.select(dbConn, empMPFPlan);
                }

                obj.ExportToObject(empMPFPlan);

                if (obj.ImportActionStatus == ImportDBObject.ImportActionEnum.INSERT)
                {
                    empMPFPlan.EmpID = ParseTemp.GetEmpIDFromUploadEmpID(dbConn, obj.UploadEmpID);
                    DBFilter emplastPosFilter = new DBFilter();
                    EEmpMPFPlan lastObj = (EEmpMPFPlan)AppUtils.GetLastObj(dbConn, uploadDB, "EmpMPFEffFr", empMPFPlan.EmpID, new Match("EmpMPFEffFr", "<", empMPFPlan.EmpMPFEffFr));
                    if (lastObj != null)
                        if (lastObj.EmpMPFEffTo.Ticks == 0)
                        {
                            lastObj.EmpMPFEffTo = empMPFPlan.EmpMPFEffFr.AddDays(-1);
                            uploadDB.update(dbConn, lastObj);
                        }
                    uploadDB.insert(dbConn, empMPFPlan);


                }
                else if (obj.ImportActionStatus == ImportDBObject.ImportActionEnum.UPDATE)
                {
                    uploadDB.update(dbConn, empMPFPlan);
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
            tmpDataTable.Columns.Add(FIELD_MPFPLAN, typeof(string));
            if (IsIncludeSyncID)
                tmpDataTable.Columns.Add(FIELD_SYNC_ID, typeof(string));

            foreach (EEmpPersonalInfo empInfo in empList)
            {
                if (EEmpPersonalInfo.db.select(dbConn, empInfo))
                {
                    DBFilter filter = new DBFilter();
                    filter.add(new Match("EmpID", empInfo.EmpID));
                    filter.add(getCreateModifiedRecordsAfterDBTerm(ReferenceDateTime));
                    ArrayList list = EEmpMPFPlan.db.select(dbConn, filter);
                    foreach (EEmpMPFPlan empMPFPlan in list)
                    {
                        DataRow row = tmpDataTable.NewRow();
                        //if (IsIncludeInternalID)
                        //    row[FIELD_INTERNAL_ID] = ToHexDecWithCheckDigit(empMPFPlan.EmpMPFID);
                        row[FIELD_EMP_NO] = empInfo.EmpNo;

                        if (IsIncludeCurrentPositionInfo)
                        {
                            ImportEmpPersonalInfoProcess.AddEmployeeInfo(dbConn, row, empInfo.EmpID);
                            ImportEmpPositionInfoProcess.AddEmployeePositionInfo(dbConn, row, empInfo.EmpID);
                        }

                        row[FIELD_FROM] = empMPFPlan.EmpMPFEffFr;
                        row[FIELD_TO] = empMPFPlan.EmpMPFEffTo;

                        EMPFPlan mpfPlan = new EMPFPlan();
                        mpfPlan.MPFPlanID = empMPFPlan.MPFPlanID;
                        if (EMPFPlan.db.select(dbConn, mpfPlan))
                            row[FIELD_MPFPLAN] = IsShowDescription ? mpfPlan.MPFPlanDesc : mpfPlan.MPFPlanCode;

                        if (IsIncludeSyncID)
                            row[FIELD_SYNC_ID] = empMPFPlan.SynID;

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
