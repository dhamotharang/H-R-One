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

    /// <summary>
    /// Summary description for ImportEmpPositionInfoProcess
    /// </summary>
    public class ImportEmpCostCenterProcess : ImportProcessInteface
    {

        [DBClass("UploadEmpCostCenter")]
        private class EUploadEmpCostCenter : ImportDBObject
        {
            public static DBManager db = new DBManager(typeof(EUploadEmpCostCenter));

            protected int m_UploadEmpCostCenterID;
            [DBField("UploadEmpCostCenterID", true, true), TextSearch, Export(false)]
            public int UploadEmpCostCenterID
            {
                get { return m_UploadEmpCostCenterID; }
                set { m_UploadEmpCostCenterID = value; modify("UploadEmpCostCenterID"); }
            }
            protected int m_UploadEmpID;
            [DBField("UploadEmpID"), TextSearch, Export(false)]
            public int UploadEmpID
            {
                get { return m_UploadEmpID; }
                set { m_UploadEmpID = value; modify("UploadEmpID"); }
            }

            protected int m_EmpCostCenterID;
            [DBField("EmpCostCenterID"), TextSearch, Export(false)]
            public int EmpCostCenterID
            {
                get { return m_EmpCostCenterID; }
                set { m_EmpCostCenterID = value; modify("EmpCostCenterID"); }
            }
            protected int m_EmpID;
            [DBField("EmpID"), TextSearch, Export(false)]
            public int EmpID
            {
                get { return m_EmpID; }
                set { m_EmpID = value; modify("EmpID"); }
            }
            protected DateTime m_EmpCostCenterEffFr;
            [DBField("EmpCostCenterEffFr"), TextSearch, MaxLength(25), Export(false), Required]
            public DateTime EmpCostCenterEffFr
            {
                get { return m_EmpCostCenterEffFr; }
                set { m_EmpCostCenterEffFr = value; modify("EmpCostCenterEffFr"); }
            }
            protected DateTime m_EmpCostCenterEffTo;
            [DBField("EmpCostCenterEffTo"), TextSearch, MaxLength(25), Export(false)]
            public DateTime EmpCostCenterEffTo
            {
                get { return m_EmpCostCenterEffTo; }
                set { m_EmpCostCenterEffTo = value; modify("EmpCostCenterEffTo"); }
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

        [DBClass("UploadEmpCostCenterDetail")]
        private class EUploadEmpCostCenterDetail : DBObject
        {
            public static DBManager db = new DBManager(typeof(EUploadEmpCostCenterDetail));

            protected int m_UploadEmpCostCenterDetailID;
            [DBField("UploadEmpCostCenterDetailID", true, true), TextSearch, Export(false)]
            public int UploadEmpCostCenterDetailID
            {
                get { return m_UploadEmpCostCenterDetailID; }
                set { m_UploadEmpCostCenterDetailID = value; modify("UploadEmpCostCenterDetailID"); }
            }
            protected int m_UploadEmpCostCenterID;
            [DBField("UploadEmpCostCenterID"), TextSearch, Export(false)]
            public int UploadEmpCostCenterID
            {
                get { return m_UploadEmpCostCenterID; }
                set { m_UploadEmpCostCenterID = value; modify("UploadEmpCostCenterID"); }
            }


            protected int m_EmpCostCenterDetailID;
            [DBField("EmpCostCenterDetailID", true, true), TextSearch, Export(false)]
            public int EmpCostCenterDetailID
            {
                get { return m_EmpCostCenterDetailID; }
                set { m_EmpCostCenterDetailID = value; modify("EmpCostCenterDetailID"); }
            }
            protected int m_EmpCostCenterID;
            [DBField("EmpCostCenterID"), TextSearch, Export(false)]
            public int EmpCostCenterID
            {
                get { return m_EmpCostCenterID; }
                set { m_EmpCostCenterID = value; modify("EmpCostCenterID"); }
            }

            protected int m_CostCenterID;
            [DBField("CostCenterID"), TextSearch, Export(false)]
            public int CostCenterID
            {
                get { return m_CostCenterID; }
                set { m_CostCenterID = value; modify("CostCenterID"); }
            }

            protected double m_EmpCostCenterPercentage;
            [DBField("EmpCostCenterPercentage", "0.00"), TextSearch, Export(false)]
            public double EmpCostCenterPercentage
            {
                get { return m_EmpCostCenterPercentage; }
                set { m_EmpCostCenterPercentage = value; modify("EmpCostCenterPercentage"); }
            }
        }

        public const string TABLE_NAME = "cost_center";
        //  Field Const for Employee Personal Information
        private const string FIELD_EMP_NO = "Emp No";
        private const string FIELD_FROM = "From";
        private const string FIELD_TO = "To";
        private const string FIELD_COST_CENTER = "Cost Center ";
        private const string FIELD_PERCENTAGE = "Percentage ";

        
        private DateTime UploadDateTime = AppUtils.ServerDateTime();
        private DBManager tempDB = EUploadEmpCostCenter.db;
        private DBManager uploadDB = EEmpCostCenter.db;
        private DBManager tempDetailDB = EUploadEmpCostCenterDetail.db;
        private DBManager uploadDetailDB = EEmpCostCenterDetail.db;

        public ImportErrorList errors = new ImportErrorList();
        

        public ImportEmpCostCenterProcess(DatabaseConnection dbConn, string SessionID): base(dbConn, SessionID)
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

                EUploadEmpCostCenter uploadEmpCostCenter = new EUploadEmpCostCenter();
                ArrayList uploadEmpCostCenterDetailList = new ArrayList();
                ArrayList lastEmpCostCenterDetailList = null;

                string EmpNo = row[FIELD_EMP_NO].ToString().Trim();
                uploadEmpCostCenter.EmpID = Parse.GetEmpID(dbConn, EmpNo, UserID);
                if (uploadEmpCostCenter.EmpID < 0)
                    errors.addError(ImportErrorMessage.ERROR_ACCESS_DENIED_EMP_NO, new string[] { EmpNo, rowCount.ToString() });
                try
                {
                    uploadEmpCostCenter.EmpCostCenterEffFr = Parse.toDateTimeObject(row[FIELD_FROM]);
                }
                catch
                {
                    errors.addError(ImportErrorMessage.ERROR_INVALID_FIELD_VALUE, new string[] { FIELD_FROM + "=" + row[FIELD_FROM].ToString(), EmpNo, rowCount.ToString() });
                }
                try
                {
                    uploadEmpCostCenter.EmpCostCenterEffTo = Parse.toDateTimeObject(row[FIELD_TO]);
                }
                catch
                {
                    errors.addError(ImportErrorMessage.ERROR_INVALID_FIELD_VALUE, new string[] { FIELD_TO + "=" + row[FIELD_TO].ToString(), EmpNo, rowCount.ToString() });
                }
                int iFieldCount = 1;

                double totalPercentage = 0;
                while (row.Table.Columns.Contains(FIELD_COST_CENTER + iFieldCount) && row.Table.Columns.Contains(FIELD_PERCENTAGE+ iFieldCount))
                {
                    if (!row.IsNull(FIELD_COST_CENTER + iFieldCount) && !row.IsNull(FIELD_PERCENTAGE + iFieldCount))
                        if (!string.IsNullOrEmpty(row[FIELD_COST_CENTER + iFieldCount].ToString().Trim()) && !string.IsNullOrEmpty(row[FIELD_PERCENTAGE + iFieldCount].ToString().Trim()))
                        {
                            EUploadEmpCostCenterDetail uploadEmpCostCenterDetail = new EUploadEmpCostCenterDetail();
                            uploadEmpCostCenterDetail.CostCenterID = Parse.GetCostCenterID(dbConn, row[FIELD_COST_CENTER + iFieldCount].ToString(), CreateCodeIfNotExists, UserID);
                            try
                            {
                                uploadEmpCostCenterDetail.EmpCostCenterPercentage = double.Parse(row[FIELD_PERCENTAGE + iFieldCount].ToString().Trim());
                                if (uploadEmpCostCenterDetail.CostCenterID > 0)
                                {
                                    totalPercentage += uploadEmpCostCenterDetail.EmpCostCenterPercentage;
                                    uploadEmpCostCenterDetailList.Add(uploadEmpCostCenterDetail);
                                }
                                else
                                    errors.addError(ImportErrorMessage.ERROR_INVALID_FIELD_VALUE, new string[] { FIELD_COST_CENTER + iFieldCount + "=" + row[FIELD_COST_CENTER + iFieldCount].ToString(), EmpNo, rowCount.ToString() });

                            }
                            catch
                            {
                                errors.addError(ImportErrorMessage.ERROR_INVALID_FIELD_VALUE, new string[] { FIELD_PERCENTAGE + iFieldCount + "=" + row[FIELD_PERCENTAGE + iFieldCount].ToString(), EmpNo, rowCount.ToString() });
                            }
                        }
                    iFieldCount++;
                }

                if (Math.Abs(Math.Round(totalPercentage, 2) - 100) >= 0.01)
                {
                    errors.addError(ImportErrorMessage.ERROR_TOTAL_PERCENTAGE_NOT_100, new string[] { rowCount.ToString() });
                }

                uploadEmpCostCenter.SessionID = m_SessionID;
                uploadEmpCostCenter.TransactionDate = UploadDateTime;


                if (uploadEmpCostCenter.EmpID != 0 && errors.List.Count <= 0)
                {
                    if (rawDataTable.Columns.Contains(FIELD_SYNC_ID))
                    {
                        if (!row.IsNull(FIELD_SYNC_ID))
                        {
                            string strSynID = row[FIELD_SYNC_ID].ToString();
                            uploadEmpCostCenter.SynID = strSynID;
                            if (!string.IsNullOrEmpty(strSynID))
                            {
                                DBFilter synIDFilter = new DBFilter();
                                synIDFilter.add(new Match("SynID", strSynID));
                                ArrayList objSameSynIDList = EEmpCostCenter.db.select(dbConn, synIDFilter);
                                if (objSameSynIDList.Count > 0)
                                    uploadEmpCostCenter.EmpCostCenterID = ((EEmpCostCenter)objSameSynIDList[0]).EmpCostCenterID;
                            }

                            if (uploadEmpCostCenter.EmpCostCenterID != 0)
                            {
                                DBFilter empCostCenterSetailFilter = new DBFilter();
                                empCostCenterSetailFilter.add(new Match("EmpCostCenterID", uploadEmpCostCenter.EmpCostCenterID));
                                lastEmpCostCenterDetailList = EEmpCostCenterDetail.db.select(dbConn, empCostCenterSetailFilter);



                                bool IsSameDetail = true;
                                if (
                                    lastEmpCostCenterDetailList.Count == uploadEmpCostCenterDetailList.Count
                                    && uploadEmpCostCenter.EmpCostCenterEffFr == uploadEmpCostCenter.EmpCostCenterEffFr
                                    && uploadEmpCostCenter.EmpCostCenterEffTo == uploadEmpCostCenter.EmpCostCenterEffTo
                                    )

                                    foreach (EEmpCostCenterDetail empCostCenterDetail in lastEmpCostCenterDetailList)
                                    {
                                        foreach (EUploadEmpCostCenterDetail uploadEmpCostCenterDetail in uploadEmpCostCenterDetailList)
                                        {
                                            if (uploadEmpCostCenterDetail.CostCenterID == empCostCenterDetail.CostCenterID)
                                            {
                                                if (uploadEmpCostCenterDetail.EmpCostCenterPercentage - empCostCenterDetail.EmpCostCenterPercentage >= 0.01)
                                                {
                                                    uploadEmpCostCenterDetail.EmpCostCenterDetailID = empCostCenterDetail.EmpCostCenterDetailID;
                                                    IsSameDetail = false;
                                                }
                                                else
                                                {
                                                    uploadEmpCostCenterDetail.EmpCostCenterID = empCostCenterDetail.EmpCostCenterID;
                                                    uploadEmpCostCenterDetail.EmpCostCenterDetailID = empCostCenterDetail.EmpCostCenterDetailID;
                                                }
                                                break;
                                            }
                                            if (uploadEmpCostCenterDetailList[uploadEmpCostCenterDetailList.Count - 1] == uploadEmpCostCenterDetail)
                                                IsSameDetail = false;
                                        }
                                    }
                                else
                                {
                                    IsSameDetail = false;
                                }

                                if (IsSameDetail)
                                {
                                    continue;
                                }
                                else
                                {
                                    foreach (EUploadEmpCostCenterDetail uploadEmpCostCenterDetail in uploadEmpCostCenterDetailList)
                                    {
                                        uploadEmpCostCenterDetail.EmpCostCenterID = uploadEmpCostCenter.EmpCostCenterID;
                                        //uploadEmpHierarchy.EmpHierarchyID = empHierarchy.EmpHierarchyID;
                                    }
                                }

                            }
                        }
                    }
                    if (uploadEmpCostCenter.EmpCostCenterID == 0)
                    {
                        AND andTerms = new AND();
                        andTerms.add(new Match("EmpCostCenterEffFr", "<=", uploadEmpCostCenter.EmpCostCenterEffFr));

                        EEmpCostCenter lastEmpCostCenter = (EEmpCostCenter)AppUtils.GetLastObj(dbConn, uploadDB, "EmpCostCenterEffFr", uploadEmpCostCenter.EmpID, andTerms);
                        if (lastEmpCostCenter != null)
                        {
                            DBFilter empCostCenterSetailFilter = new DBFilter();
                            empCostCenterSetailFilter.add(new Match("EmpCostCenterID", lastEmpCostCenter.EmpCostCenterID));
                            lastEmpCostCenterDetailList = EEmpCostCenterDetail.db.select(dbConn, empCostCenterSetailFilter);



                            bool IsSameDetail = true;
                            if (
                                lastEmpCostCenterDetailList.Count == uploadEmpCostCenterDetailList.Count
                                && uploadEmpCostCenter.EmpCostCenterEffFr == lastEmpCostCenter.EmpCostCenterEffFr
                                && uploadEmpCostCenter.EmpCostCenterEffTo == lastEmpCostCenter.EmpCostCenterEffTo
                                )

                                foreach (EEmpCostCenterDetail empCostCenterDetail in lastEmpCostCenterDetailList)
                                {
                                    foreach (EUploadEmpCostCenterDetail uploadEmpCostCenterDetail in uploadEmpCostCenterDetailList)
                                    {
                                        if (uploadEmpCostCenterDetail.CostCenterID == empCostCenterDetail.CostCenterID)
                                        {
                                            if (uploadEmpCostCenterDetail.EmpCostCenterPercentage - empCostCenterDetail.EmpCostCenterPercentage >= 0.01)
                                            {
                                                uploadEmpCostCenterDetail.EmpCostCenterDetailID = empCostCenterDetail.EmpCostCenterDetailID;
                                                IsSameDetail = false;
                                            }
                                            else
                                            {
                                                uploadEmpCostCenterDetail.EmpCostCenterID = empCostCenterDetail.EmpCostCenterID;
                                                uploadEmpCostCenterDetail.EmpCostCenterDetailID = empCostCenterDetail.EmpCostCenterDetailID;
                                            }
                                            break;
                                        }
                                        if (uploadEmpCostCenterDetailList[uploadEmpCostCenterDetailList.Count - 1] == uploadEmpCostCenterDetail)
                                            IsSameDetail = false;
                                    }
                                }
                            else
                            {
                                IsSameDetail = false;
                            }

                            if (IsSameDetail)
                            {
                                continue;
                            }
                            else
                            {
                                // add postion terms with new ID
                                if (lastEmpCostCenter.EmpCostCenterEffFr.Equals(uploadEmpCostCenter.EmpCostCenterEffFr))
                                {
                                    uploadEmpCostCenter.EmpCostCenterID = lastEmpCostCenter.EmpCostCenterID;
                                    if (uploadEmpCostCenter.EmpCostCenterEffTo.Ticks == 0 && lastEmpCostCenter.EmpCostCenterEffTo.Ticks != 0)
                                    {
                                        EEmpCostCenter afterEmpCostCenter = (EEmpCostCenter)AppUtils.GetLastObj(dbConn, uploadDB, "EmpCostCenterEffFr", uploadEmpCostCenter.EmpID, new Match("EmpCostCenterEffFr", ">", lastEmpCostCenter.EmpCostCenterEffTo));
                                        if (afterEmpCostCenter != null)
                                            uploadEmpCostCenter.EmpCostCenterEffTo = afterEmpCostCenter.EmpCostCenterEffFr.AddDays(-1);
                                    }
                                    foreach (EUploadEmpCostCenterDetail uploadEmpCostCenterDetail in uploadEmpCostCenterDetailList)
                                    {
                                        uploadEmpCostCenterDetail.EmpCostCenterID = uploadEmpCostCenter.EmpCostCenterID;
                                        //uploadEmpHierarchy.EmpHierarchyID = empHierarchy.EmpHierarchyID;
                                    }

                                }
                                else
                                {
                                    AND lastObjAndTerms = new AND();
                                    lastObjAndTerms.add(new Match("EmpCostCenterEffFr", ">", uploadEmpCostCenter.EmpCostCenterEffFr));
                                    if (!uploadEmpCostCenter.EmpCostCenterEffTo.Ticks.Equals(0))
                                        lastObjAndTerms.add(new Match("EmpCostCenterEffFr", "<=", uploadEmpCostCenter.EmpCostCenterEffTo));

                                    EEmpCostCenter lastObj = (EEmpCostCenter)AppUtils.GetLastObj(dbConn, uploadDB, "EmpCostCenterEffFr", uploadEmpCostCenter.EmpID, lastObjAndTerms);
                                    if (lastObj != null)
                                        if (!lastObj.EmpCostCenterEffTo.Ticks.Equals(0))
                                        {
                                            errors.addError(ImportErrorMessage.ERROR_DATE_FROM_OVERLAP, new string[] { uploadEmpCostCenter.EmpCostCenterEffFr.ToString("yyyy-MM-dd"), rowCount.ToString() });
                                            continue;
                                        }
                                }
                            }
                        }
                    }
                }

                if (uploadEmpCostCenter.EmpCostCenterID <= 0)
                    uploadEmpCostCenter.ImportActionStatus = ImportDBObject.ImportActionEnum.INSERT;
                else
                    uploadEmpCostCenter.ImportActionStatus = ImportDBObject.ImportActionEnum.UPDATE;

                uploadEmpCostCenter.UploadEmpID = ParseTemp.GetUploadEmpID(dbConn, EmpNo, m_SessionID);
                if (uploadEmpCostCenter.UploadEmpID == 0)
                    if (uploadEmpCostCenter.EmpID == 0)
                        errors.addError(ImportErrorMessage.ERROR_INVALID_EMP_NO, new string[] { EmpNo, rowCount.ToString() });
                    else
                        uploadEmpCostCenter.UploadEmpID = ImportEmpPersonalInfoProcess.CreateDummyUploadEmployeeInfo(dbConn, uploadEmpCostCenter.EmpID, m_SessionID, UploadDateTime);

                Hashtable values = new Hashtable();
                tempDB.populate(uploadEmpCostCenter, values);
                PageErrors pageErrors = new PageErrors(EUploadEmpPersonalInfo.db);
                tempDB.validate(pageErrors, values);
                if (pageErrors.errors.Count == 0)
                {

                    tempDB.insert(dbConn, uploadEmpCostCenter);
                    foreach (EUploadEmpCostCenterDetail uploadCostCenterDetail in uploadEmpCostCenterDetailList)
                    {
                        uploadCostCenterDetail.UploadEmpCostCenterID = uploadEmpCostCenter.UploadEmpCostCenterID;
                        tempDetailDB.insert(dbConn, uploadCostCenterDetail);
                    }
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
            ImportEmpPositionInfoProcess import = new ImportEmpPositionInfoProcess(dbConn, sessionID);
            import.ClearTempTable();
        }

        public override void ClearTempTable()
        {
            //  Clear Old Import Session
            DBFilter sessionFilter = new DBFilter();
            if (!string.IsNullOrEmpty(m_SessionID))
                sessionFilter.add(new Match("SessionID", m_SessionID));
            ArrayList uploadEmpCostCenterList = tempDB.select(dbConn, sessionFilter);
            foreach (EUploadEmpCostCenter uploadEmpCostCenter in uploadEmpCostCenterList)
            {
                DBFilter uploadEmpCostCenterFilter = new DBFilter();
                uploadEmpCostCenterFilter.add(new Match("UploadEmpCostCenterID", uploadEmpCostCenter.UploadEmpCostCenterID));
                tempDetailDB.delete(dbConn, uploadEmpCostCenterFilter);
                tempDB.delete(dbConn, uploadEmpCostCenter);

            }

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
            ArrayList uploadEmpCostCenterList = tempDB.select(dbConn, sessionFilter);
            foreach (EUploadEmpCostCenter obj in uploadEmpCostCenterList)
            {
                EEmpCostCenter empCostCenter = new EEmpCostCenter();

                if (obj.ImportActionStatus != ImportDBObject.ImportActionEnum.INSERT)
                {
                    empCostCenter.EmpCostCenterID = obj.EmpCostCenterID;
                    uploadDB.select(dbConn, empCostCenter);
                }

                obj.ExportToObject(empCostCenter);

                if (obj.ImportActionStatus == ImportDBObject.ImportActionEnum.INSERT)
                {
                    empCostCenter.EmpID = ParseTemp.GetEmpIDFromUploadEmpID(dbConn, obj.UploadEmpID);
                    EEmpCostCenter lastObj = (EEmpCostCenter)AppUtils.GetLastObj(dbConn, EEmpCostCenter.db, "EmpCostCenterEffFr", empCostCenter.EmpID, new Match("EmpCostCenterEffFr", "<", empCostCenter.EmpCostCenterEffFr));
                    if (lastObj != null)
                        if (lastObj.EmpCostCenterEffTo.Ticks == 0)
                        {
                            lastObj.EmpCostCenterEffTo = empCostCenter.EmpCostCenterEffFr.AddDays(-1);
                            uploadDB.update(dbConn, lastObj);
                        }
                    uploadDB.insert(dbConn, empCostCenter);

                    DBFilter uploadEmpCostCenterFilter = new DBFilter();
                    uploadEmpCostCenterFilter.add(new Match("UploadEmpCostCenterID", obj.UploadEmpCostCenterID));
                    ArrayList uploadEmpCostCenterDetailList = EUploadEmpCostCenterDetail.db.select(dbConn, uploadEmpCostCenterFilter);
                    foreach (EUploadEmpCostCenterDetail uploadEmpCostCenterDetail in uploadEmpCostCenterDetailList)
                    {
                        uploadEmpCostCenterDetail.EmpCostCenterID = empCostCenter.EmpCostCenterID;
                        EEmpCostCenterDetail empCostCenterDetail = new EEmpCostCenterDetail();
                        ImportDBObject.CopyObjectProperties(uploadEmpCostCenterDetail, empCostCenterDetail);
                        uploadDetailDB.insert(dbConn, empCostCenterDetail);
                        EUploadEmpCostCenterDetail.db.delete(dbConn, uploadEmpCostCenterDetail);
                    }
                }
                else if (obj.ImportActionStatus == ImportDBObject.ImportActionEnum.UPDATE)
                {
                    uploadDB.update(dbConn, empCostCenter);

                    DBFilter empCostCenterDetailFilter = new DBFilter();
                    empCostCenterDetailFilter.add(new Match("EmpCostCenterID", empCostCenter.EmpCostCenterID));
                    uploadDetailDB.delete(dbConn, empCostCenterDetailFilter);

                    DBFilter uploadEmpCostCenterDetailFilter = new DBFilter();
                    uploadEmpCostCenterDetailFilter.add(new Match("UploadEmpCostCenterID", obj.UploadEmpCostCenterID));
                    ArrayList uploadEmpCostCenterDetailList = EUploadEmpCostCenterDetail.db.select(dbConn, uploadEmpCostCenterDetailFilter);
                    foreach (EUploadEmpCostCenterDetail uploadEmpCostCenterDetail in uploadEmpCostCenterDetailList)
                    {
                        EEmpCostCenterDetail empCostCenterDetail = new EEmpCostCenterDetail();
                        ImportDBObject.CopyObjectProperties(uploadEmpCostCenterDetail, empCostCenterDetail);
                        uploadDetailDB.insert(dbConn, empCostCenterDetail);
                        EUploadEmpCostCenterDetail.db.delete(dbConn, uploadEmpCostCenterDetail);
                    }
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
            if (IsIncludeSyncID)
                tmpDataTable.Columns.Add(FIELD_SYNC_ID, typeof(string));
            tmpDataTable.Columns.Add(FIELD_COST_CENTER + "1", typeof(string));
            tmpDataTable.Columns.Add(FIELD_PERCENTAGE + "1", typeof(double));
            tmpDataTable.Columns.Add(FIELD_COST_CENTER + "2", typeof(string));
            tmpDataTable.Columns.Add(FIELD_PERCENTAGE + "2", typeof(double));


            foreach (EEmpPersonalInfo empInfo in empList)
            {
                if (EEmpPersonalInfo.db.select(dbConn, empInfo))
                {
                    DBFilter filter = new DBFilter();
                    filter.add(new Match("EmpID", empInfo.EmpID));
                    filter.add(getCreateModifiedRecordsAfterDBTerm(ReferenceDateTime));
                    ArrayList list = EEmpCostCenter.db.select(dbConn, filter);
                    foreach (EEmpCostCenter empCostCenter in list)
                    {
                        DataRow row = tmpDataTable.NewRow();
                        //if (IsIncludeInternalID)
                        //    row[FIELD_INTERNAL_ID] = ToHexDecWithCheckDigit(empCostCenter.EmpCostCenterID);
                        row[FIELD_EMP_NO] = empInfo.EmpNo;

                        if (IsIncludeCurrentPositionInfo)
                        {
                            ImportEmpPersonalInfoProcess.AddEmployeeInfo(dbConn, row, empInfo.EmpID);
                            ImportEmpPositionInfoProcess.AddEmployeePositionInfo(dbConn, row, empInfo.EmpID);
                        }

                        row[FIELD_FROM] = empCostCenter.EmpCostCenterEffFr;
                        row[FIELD_TO] = empCostCenter.EmpCostCenterEffTo;

                        DBFilter costCenterDetailFilter = new DBFilter();
                        costCenterDetailFilter.add(new Match("EmpCostCenterID", empCostCenter.EmpCostCenterID));
                        ArrayList costCenterDetailList = EEmpCostCenterDetail.db.select(dbConn, costCenterDetailFilter);
                        for (int i = 1; i <= costCenterDetailList.Count; i++)
                        {
                            EEmpCostCenterDetail empCostCenterDetail = (EEmpCostCenterDetail)costCenterDetailList[i - 1];
                            if (!tmpDataTable.Columns.Contains(FIELD_COST_CENTER + i))
                                tmpDataTable.Columns.Add(FIELD_COST_CENTER + i, typeof(string));

                            if (!tmpDataTable.Columns.Contains(FIELD_PERCENTAGE + i))
                                tmpDataTable.Columns.Add(FIELD_PERCENTAGE + i, typeof(double));

                            ECostCenter costCenter = new ECostCenter();
                            costCenter.CostCenterID = empCostCenterDetail.CostCenterID;
                            if (ECostCenter.db.select(dbConn, costCenter))
                            {
                                row[FIELD_COST_CENTER + i] = IsShowDescription ? costCenter.CostCenterDesc : costCenter.CostCenterCode;
                                row[FIELD_PERCENTAGE + i] = empCostCenterDetail.EmpCostCenterPercentage;
                            }
                        }
                        if (IsIncludeSyncID)
                            row[FIELD_SYNC_ID] = empCostCenter.SynID;
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
