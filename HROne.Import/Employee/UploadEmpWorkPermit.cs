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
    public class ImportEmpPermitProcess : ImportProcessInteface
    {

        [DBClass("UploadEmpPermit")]
        private class EUploadEmpPermit : ImportDBObject
        {
            public static DBManager db = new DBManager(typeof(EUploadEmpPermit));

            protected int m_UploadEmpPermitID;
            [DBField("UploadEmpPermitID", true, true), TextSearch, Export(false)]
            public int UploadEmpPermitID
            {
                get { return m_UploadEmpPermitID; }
                set { m_UploadEmpPermitID = value; modify("UploadEmpPermitID"); }
            }
            protected int m_UploadEmpID;
            [DBField("UploadEmpID"), TextSearch, Export(false)]
            public int UploadEmpID
            {
                get { return m_UploadEmpID; }
                set { m_UploadEmpID = value; modify("UploadEmpID"); }
            }

            protected int m_EmpPermitID;
            [DBField("EmpPermitID"), TextSearch, Export(false)]
            public int EmpPermitID
            {
                get { return m_EmpPermitID; }
                set { m_EmpPermitID = value; modify("EmpPermitID"); }
            }
            protected int m_EmpID;
            [DBField("EmpID"), TextSearch, Export(false)]
            public int EmpID
            {
                get { return m_EmpID; }
                set { m_EmpID = value; modify("EmpID"); }
            }

            protected int m_PermitTypeID;
            [DBField("PermitTypeID"), TextSearch, Export(false), Required]
            public int PermitTypeID
            {
                get { return m_PermitTypeID; }
                set { m_PermitTypeID = value; modify("PermitTypeID"); }
            }
            protected string m_EmpPermitNo;
            [DBField("EmpPermitNo"), TextSearch, MaxLength(50, 20), Export(false)]
            public string EmpPermitNo
            {
                get { return m_EmpPermitNo; }
                set { m_EmpPermitNo = value; modify("EmpPermitNo"); }
            }

            protected DateTime m_EmpPermitIssueDate;
            [DBField("EmpPermitIssueDate"), TextSearch, MaxLength(11), Export(false)]
            public DateTime EmpPermitIssueDate
            {
                get { return m_EmpPermitIssueDate; }
                set { m_EmpPermitIssueDate = value; modify("EmpPermitIssueDate"); }
            }
            protected DateTime m_EmpPermitExpiryDate;
            [DBField("EmpPermitExpiryDate"), TextSearch, MaxLength(11), Export(false)]
            public DateTime EmpPermitExpiryDate
            {
                get { return m_EmpPermitExpiryDate; }
                set { m_EmpPermitExpiryDate = value; modify("EmpPermitExpiryDate"); }
            }
            protected string m_EmpPermitRemark;
            [DBField("EmpPermitRemark"), TextSearch, Export(false)]
            public string EmpPermitRemark
            {
                get { return m_EmpPermitRemark; }
                set { m_EmpPermitRemark = value; modify("EmpPermitRemark"); }
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

        public const string TABLE_NAME = "work_permit";

        //  Field Const for Employee Personal Information
        private const string FIELD_EMP_NO = "Emp No";
        private const string FIELD_PERMIT_TYPE = "Permit Type";
        private const string FIELD_PERMIT_NO = "Permit Number";
        private const string FIELD_ISSUE_DATE = "Issue Date";
        private const string FIELD_EXPIRY_DATE = "Expiry Date";
        private const string FIELD_REMARK = "Remark";

        
        private DateTime UploadDateTime = AppUtils.ServerDateTime();
        private DBManager tempDB = EUploadEmpPermit.db;
        private DBManager uploadDB = EEmpPermit.db;

        public ImportErrorList errors = new ImportErrorList();
        

        public ImportEmpPermitProcess(DatabaseConnection dbConn, string SessionID): base(dbConn, SessionID)
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

                EUploadEmpPermit uploadEmpPermit = new EUploadEmpPermit();
                //EEmpPermit lastEmpPermit = null;

                string EmpNo = row[FIELD_EMP_NO].ToString().Trim();
                uploadEmpPermit.EmpID = Parse.GetEmpID(dbConn, EmpNo, UserID);
                if (uploadEmpPermit.EmpID < 0)
                    errors.addError(ImportErrorMessage.ERROR_ACCESS_DENIED_EMP_NO, new string[] { EmpNo, rowCount.ToString() });
                uploadEmpPermit.PermitTypeID = Parse.GetPermitID(dbConn, row[FIELD_PERMIT_TYPE].ToString(), CreateCodeIfNotExists, UserID);
                uploadEmpPermit.EmpPermitNo = row[FIELD_PERMIT_NO].ToString();
                try
                {
                    uploadEmpPermit.EmpPermitIssueDate = Import.Parse.toDateTimeObject(row[FIELD_ISSUE_DATE]);
                }
                catch
                {
                    errors.addError(ImportErrorMessage.ERROR_INVALID_FIELD_VALUE, new string[] { FIELD_ISSUE_DATE + "=" + row[FIELD_ISSUE_DATE].ToString(), EmpNo, rowCount.ToString() });
                }
                try
                {
                    uploadEmpPermit.EmpPermitExpiryDate = Import.Parse.toDateTimeObject(row[FIELD_EXPIRY_DATE]);
                }
                catch
                {
                    errors.addError(ImportErrorMessage.ERROR_INVALID_FIELD_VALUE, new string[] { FIELD_EXPIRY_DATE + "=" + row[FIELD_EXPIRY_DATE].ToString(), EmpNo, rowCount.ToString() });
                }
                uploadEmpPermit.EmpPermitRemark = row[FIELD_REMARK].ToString();



                uploadEmpPermit.SessionID = m_SessionID;
                uploadEmpPermit.TransactionDate = UploadDateTime;


                if (uploadEmpPermit.EmpID != 0)
                {
                    //if (rawDataTable.Columns.Contains(FIELD_INTERNAL_ID))
                    //{
                    //    try
                    //    {
                    //        if (!row.IsNull(FIELD_INTERNAL_ID))
                    //        {
                    //            int tmpID = FromHexDecWithCheckDigit((string)row[FIELD_INTERNAL_ID]);
                    //            EEmpPermit tmpObj = new EEmpPermit();
                    //            tmpObj.EmpPermitID = tmpID;
                    //            if (EEmpPermit.db.select(dbConn, tmpObj))
                    //                uploadEmpPermit.EmpPermitID = tmpID;
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
                            uploadEmpPermit.SynID = strSynID;
                            if (!string.IsNullOrEmpty(strSynID))
                            {
                                DBFilter synIDFilter = new DBFilter();
                                synIDFilter.add(new Match("SynID", strSynID));
                                ArrayList objSameSynIDList = EEmpPermit.db.select(dbConn, synIDFilter);
                                if (objSameSynIDList.Count > 0)
                                    uploadEmpPermit.EmpPermitID = ((EEmpPermit)objSameSynIDList[0]).EmpPermitID;
                            }
                        }

                    }

                    if (uploadEmpPermit.EmpPermitID == 0)
                    {
                        DBFilter filter = new DBFilter();
                        filter.add(new Match("EmpID", uploadEmpPermit.EmpID));
                        ArrayList list = uploadDB.select(dbConn, filter);
                        bool IsSameEntryExists = false;
                        foreach (EEmpPermit currentEmpPermit in list)
                        {

                            if (uploadEmpPermit.PermitTypeID == currentEmpPermit.PermitTypeID
                                && uploadEmpPermit.EmpPermitNo == currentEmpPermit.EmpPermitNo
                                && uploadEmpPermit.EmpPermitIssueDate == currentEmpPermit.EmpPermitIssueDate
                                && uploadEmpPermit.EmpPermitExpiryDate == currentEmpPermit.EmpPermitExpiryDate
                                && uploadEmpPermit.EmpPermitRemark == currentEmpPermit.EmpPermitRemark
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

                if (uploadEmpPermit.EmpPermitID <= 0)
                    uploadEmpPermit.ImportActionStatus = ImportDBObject.ImportActionEnum.INSERT;
                else
                    uploadEmpPermit.ImportActionStatus = ImportDBObject.ImportActionEnum.UPDATE;

                uploadEmpPermit.UploadEmpID = ParseTemp.GetUploadEmpID(dbConn, EmpNo, m_SessionID);
                if (uploadEmpPermit.UploadEmpID == 0)
                    if (uploadEmpPermit.EmpID == 0)
                        errors.addError(ImportErrorMessage.ERROR_INVALID_EMP_NO, new string[] { EmpNo, rowCount.ToString() });
                    else
                        uploadEmpPermit.UploadEmpID = ImportEmpPersonalInfoProcess.CreateDummyUploadEmployeeInfo(dbConn, uploadEmpPermit.EmpID, m_SessionID, UploadDateTime);

                Hashtable values = new Hashtable();
                tempDB.populate(uploadEmpPermit, values);
                PageErrors pageErrors = new PageErrors(EUploadEmpPersonalInfo.db);
                tempDB.validate(pageErrors, values);
                if (pageErrors.errors.Count == 0)
                {

                    tempDB.insert(dbConn, uploadEmpPermit);
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
            ImportEmpPermitProcess import = new ImportEmpPermitProcess(dbConn, sessionID);
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
            ArrayList uploadEmpPermitList = tempDB.select(dbConn, sessionFilter);
            foreach (EUploadEmpPermit obj in uploadEmpPermitList)
            {
                EEmpPermit EmpPermit = new EEmpPermit();

                if (obj.ImportActionStatus != ImportDBObject.ImportActionEnum.INSERT)
                {
                    EmpPermit.EmpPermitID = obj.EmpPermitID;
                    uploadDB.select(dbConn, EmpPermit);
                }

                obj.ExportToObject(EmpPermit);

                if (obj.ImportActionStatus == ImportDBObject.ImportActionEnum.INSERT)
                {
                    EmpPermit.EmpID = ParseTemp.GetEmpIDFromUploadEmpID(dbConn, obj.UploadEmpID);
                    uploadDB.insert(dbConn, EmpPermit);


                }
                else if (obj.ImportActionStatus == ImportDBObject.ImportActionEnum.UPDATE)
                {
                    uploadDB.update(dbConn, EmpPermit);
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

            tmpDataTable.Columns.Add(FIELD_PERMIT_TYPE, typeof(string));
            tmpDataTable.Columns.Add(FIELD_PERMIT_NO, typeof(string));
            tmpDataTable.Columns.Add(FIELD_ISSUE_DATE, typeof(DateTime));
            tmpDataTable.Columns.Add(FIELD_EXPIRY_DATE, typeof(DateTime));
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
                    ArrayList list = EEmpPermit.db.select(dbConn, filter);
                    foreach (EEmpPermit empPermit in list)
                    {
                        DataRow row = tmpDataTable.NewRow();
                        //if (IsIncludeInternalID)
                        //    row[FIELD_INTERNAL_ID] = ToHexDecWithCheckDigit(empPermit.EmpPermitID);
                        row[FIELD_EMP_NO] = empInfo.EmpNo;

                        if (IsIncludeCurrentPositionInfo)
                        {
                            ImportEmpPersonalInfoProcess.AddEmployeeInfo(dbConn, row, empInfo.EmpID);
                            ImportEmpPositionInfoProcess.AddEmployeePositionInfo(dbConn, row, empInfo.EmpID);
                        }

                        EPermitType permitType = new EPermitType();
                        permitType.PermitTypeID = empPermit.PermitTypeID;
                        if (EPermitType.db.select(dbConn, permitType))
                            row[FIELD_PERMIT_TYPE] = IsShowDescription ? permitType.PermitTypeDesc : permitType.PermitTypeCode;
                        row[FIELD_PERMIT_NO] = empPermit.EmpPermitNo;
                        row[FIELD_ISSUE_DATE] = empPermit.EmpPermitIssueDate;
                        row[FIELD_EXPIRY_DATE] = empPermit.EmpPermitExpiryDate;
                        row[FIELD_REMARK] = empPermit.EmpPermitRemark;

                        if (IsIncludeSyncID)
                            row[FIELD_SYNC_ID] = empPermit.SynID;

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
