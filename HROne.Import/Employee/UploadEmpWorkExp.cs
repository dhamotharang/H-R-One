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
    public class ImportEmpWorkExpProcess : ImportProcessInteface
    {

        [DBClass("UploadEmpWorkExp")]
        private class EUploadEmpWorkExp : ImportDBObject
        {
            public static DBManager db = new DBManager(typeof(EUploadEmpWorkExp));

            protected int m_UploadEmpWorkExpID;
            [DBField("UploadEmpWorkExpID", true, true), TextSearch, Export(false)]
            public int UploadEmpWorkExpID
            {
                get { return m_UploadEmpWorkExpID; }
                set { m_UploadEmpWorkExpID = value; modify("UploadEmpWorkExpID"); }
            }
            protected int m_UploadEmpID;
            [DBField("UploadEmpID"), TextSearch, Export(false)]
            public int UploadEmpID
            {
                get { return m_UploadEmpID; }
                set { m_UploadEmpID = value; modify("UploadEmpID"); }
            }

            protected int m_EmpWorkExpID;
            [DBField("EmpWorkExpID"), TextSearch, Export(false)]
            public int EmpWorkExpID
            {
                get { return m_EmpWorkExpID; }
                set { m_EmpWorkExpID = value; modify("EmpWorkExpID"); }
            }
            protected int m_EmpID;
            [DBField("EmpID"), TextSearch, Export(false)]
            public int EmpID
            {
                get { return m_EmpID; }
                set { m_EmpID = value; modify("EmpID"); }
            }

            protected int m_EmpWorkExpFromYear;
            [DBField("EmpWorkExpFromYear"), TextSearch, MaxLength(4), Export(false), Required]
            public int EmpWorkExpFromYear
            {
                get { return m_EmpWorkExpFromYear; }
                set { m_EmpWorkExpFromYear = value; modify("EmpWorkExpFromYear"); }
            }

            protected int m_EmpWorkExpFromMonth;
            [DBField("EmpWorkExpFromMonth"), TextSearch, Export(false)]
            public int EmpWorkExpFromMonth
            {
                get { return m_EmpWorkExpFromMonth; }
                set { m_EmpWorkExpFromMonth = value; modify("EmpWorkExpFromMonth"); }
            }

            protected int m_EmpWorkExpToYear;
            [DBField("EmpWorkExpToYear"), TextSearch, MaxLength(4), Export(false)]
            public int EmpWorkExpToYear
            {
                get { return m_EmpWorkExpToYear; }
                set { m_EmpWorkExpToYear = value; modify("EmpWorkExpToYear"); }
            }

            protected int m_EmpWorkExpToMonth;
            [DBField("EmpWorkExpToMonth"), TextSearch, Export(false)]
            public int EmpWorkExpToMonth
            {
                get { return m_EmpWorkExpToMonth; }
                set { m_EmpWorkExpToMonth = value; modify("EmpWorkExpToMonth"); }
            }

            protected string m_EmpWorkExpCompanyName;
            [DBField("EmpWorkExpCompanyName"), TextSearch, MaxLength(1000, 40), Export(false)]
            public string EmpWorkExpCompanyName
            {
                get { return m_EmpWorkExpCompanyName; }
                set { m_EmpWorkExpCompanyName = value; modify("EmpWorkExpCompanyName"); }
            }

            protected string m_EmpWorkExpPosition;
            [DBField("EmpWorkExpPosition"), TextSearch, MaxLength(100, 40), Export(false)]
            public string EmpWorkExpPosition
            {
                get { return m_EmpWorkExpPosition; }
                set { m_EmpWorkExpPosition = value; modify("EmpWorkExpPosition"); }
            }

            protected int m_EmpWorkExpEmploymentTypeID;
            [DBField("EmpWorkExpEmploymentTypeID"), TextSearch, Export(false)]
            public int EmpWorkExpEmploymentTypeID
            {
                get { return m_EmpWorkExpEmploymentTypeID; }
                set { m_EmpWorkExpEmploymentTypeID = value; modify("EmpWorkExpEmploymentTypeID"); }
            }


            protected bool m_EmpWorkExpIsRelevantExperience;
            [DBField("EmpWorkExpIsRelevantExperience"), TextSearch, Export(false)]
            public bool EmpWorkExpIsRelevantExperience
            {
                get { return m_EmpWorkExpIsRelevantExperience; }
                set { m_EmpWorkExpIsRelevantExperience = value; modify("EmpWorkExpIsRelevantExperience"); }
            }

            protected string m_EmpWorkExpRemark;
            [DBField("EmpWorkExpRemark"), TextSearch, Export(false)]
            public string EmpWorkExpRemark
            {
                get { return m_EmpWorkExpRemark; }
                set { m_EmpWorkExpRemark = value; modify("EmpWorkExpRemark"); }
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

        public const string TABLE_NAME = "working_experience";

        //  Field Const for Employee Personal Information
        private const string FIELD_EMP_NO = "Emp No";
        private const string FIELD_FROM_YEAR = "From (Year)";
        private const string FIELD_FROM_MONTH = "From (Month)";
        private const string FIELD_TO_YEAR = "To (Year)";
        private const string FIELD_TO_MONTH = "To (Month)";
        private const string FIELD_COMPANY = "Company Name";
        private const string FIELD_POSITION = "Position Employed";
        private const string FIELD_EMPLOYMENT_TYPE = "Employment Type";
        private const string FIELD_IS_RELEVANT_EXPERIENCE = "Is Relevant Experience";
        private const string FIELD_REMARK = "Remark";

        
        private DateTime UploadDateTime = AppUtils.ServerDateTime();
        private DBManager tempDB = EUploadEmpWorkExp.db;
        private DBManager uploadDB = EEmpWorkExp.db;

        public ImportErrorList errors = new ImportErrorList();
        

        public ImportEmpWorkExpProcess(DatabaseConnection dbConn, string SessionID): base(dbConn, SessionID)
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

                EUploadEmpWorkExp uploadEmpWorkExp = new EUploadEmpWorkExp();
                //EEmpWorkExp lastEmpWorkExp = null;

                string EmpNo = row[FIELD_EMP_NO].ToString().Trim();
                uploadEmpWorkExp.EmpID = Parse.GetEmpID(dbConn, EmpNo, UserID);
                if (uploadEmpWorkExp.EmpID < 0)
                    errors.addError(ImportErrorMessage.ERROR_ACCESS_DENIED_EMP_NO, new string[] { EmpNo, rowCount.ToString() });

                string tempString = row[FIELD_FROM_YEAR].ToString();
                if (!string.IsNullOrEmpty(tempString))
                {
                    int tmpInteger = 0;
                    if (int.TryParse(tempString, out tmpInteger))
                        uploadEmpWorkExp.EmpWorkExpFromYear = tmpInteger;
                    else
                        errors.addError(ImportErrorMessage.ERROR_INVALID_FIELD_VALUE, new string[] { FIELD_FROM_YEAR + "=" + tempString, EmpNo, rowCount.ToString() });
                }
                else
                    uploadEmpWorkExp.EmpWorkExpFromYear = 0;

                tempString = row[FIELD_FROM_MONTH].ToString();
                if (!string.IsNullOrEmpty(tempString))
                {
                    int tmpInteger = 0;
                    if (int.TryParse(tempString, out tmpInteger))
                        uploadEmpWorkExp.EmpWorkExpFromMonth = tmpInteger;
                    else
                        errors.addError(ImportErrorMessage.ERROR_INVALID_FIELD_VALUE, new string[] { FIELD_FROM_MONTH + "=" + tempString, EmpNo, rowCount.ToString() });
                }
                else
                    uploadEmpWorkExp.EmpWorkExpFromMonth = 0;

                tempString = row[FIELD_TO_YEAR].ToString();
                if (!string.IsNullOrEmpty(tempString))
                {
                    int tmpInteger = 0;
                    if (int.TryParse(tempString, out tmpInteger))
                        uploadEmpWorkExp.EmpWorkExpToYear = tmpInteger;
                    else
                        errors.addError(ImportErrorMessage.ERROR_INVALID_FIELD_VALUE, new string[] { FIELD_TO_YEAR + "=" + tempString, EmpNo, rowCount.ToString() });
                }
                else
                    uploadEmpWorkExp.EmpWorkExpToYear = 0;

                tempString = row[FIELD_TO_MONTH].ToString();
                if (!string.IsNullOrEmpty(tempString))
                {
                    int tmpInteger = 0;
                    if (int.TryParse(tempString, out tmpInteger))
                        uploadEmpWorkExp.EmpWorkExpToMonth = tmpInteger;
                    else
                        errors.addError(ImportErrorMessage.ERROR_INVALID_FIELD_VALUE, new string[] { FIELD_TO_MONTH + "=" + tempString, EmpNo, rowCount.ToString() });
                }
                else
                    uploadEmpWorkExp.EmpWorkExpToMonth = 0;

                uploadEmpWorkExp.EmpWorkExpCompanyName = row[FIELD_COMPANY].ToString();
                uploadEmpWorkExp.EmpWorkExpPosition = row[FIELD_POSITION].ToString();
                uploadEmpWorkExp.EmpWorkExpRemark = row[FIELD_REMARK].ToString();

                if (rawDataTable.Columns.Contains(FIELD_EMPLOYMENT_TYPE))
                    uploadEmpWorkExp.EmpWorkExpEmploymentTypeID = Import.Parse.GetEmploymentTypeID(dbConn, row[FIELD_EMPLOYMENT_TYPE].ToString(), false, UserID);

                if (rawDataTable.Columns.Contains(FIELD_IS_RELEVANT_EXPERIENCE))
                    uploadEmpWorkExp.EmpWorkExpIsRelevantExperience = Import.Parse.toBoolean(row[FIELD_IS_RELEVANT_EXPERIENCE].ToString());

                uploadEmpWorkExp.SessionID = m_SessionID;
                uploadEmpWorkExp.TransactionDate = UploadDateTime;


                if (uploadEmpWorkExp.EmpID != 0)
                {
                    //if (rawDataTable.Columns.Contains(FIELD_INTERNAL_ID))
                    //{
                    //    try
                    //    {
                    //        if (!row.IsNull(FIELD_INTERNAL_ID))
                    //        {
                    //            int tmpID = FromHexDecWithCheckDigit((string)row[FIELD_INTERNAL_ID]);
                    //            EEmpWorkExp tmpObj = new EEmpWorkExp();
                    //            tmpObj.EmpWorkExpID = tmpID;
                    //            if (EEmpWorkExp.db.select(dbConn, tmpObj))
                    //                uploadEmpWorkExp.EmpWorkExpID = tmpID;
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
                            uploadEmpWorkExp.SynID = strSynID;
                            if (!string.IsNullOrEmpty(strSynID))
                            {
                                DBFilter synIDFilter = new DBFilter();
                                synIDFilter.add(new Match("SynID", strSynID));
                                ArrayList objSameSynIDList = EEmpWorkExp.db.select(dbConn, synIDFilter);
                                if (objSameSynIDList.Count > 0)
                                    uploadEmpWorkExp.EmpWorkExpID = ((EEmpWorkExp)objSameSynIDList[0]).EmpWorkExpID;
                            }
                        }

                    }

                    if (uploadEmpWorkExp.EmpWorkExpID == 0)
                    {

                        DBFilter filter = new DBFilter();
                        filter.add(new Match("EmpID", uploadEmpWorkExp.EmpID));
                        ArrayList list = uploadDB.select(dbConn, filter);
                        bool IsSameEntryExists = false;
                        foreach (EEmpWorkExp currentEmpWorkExp in list)
                        {

                            if (uploadEmpWorkExp.EmpWorkExpFromYear == currentEmpWorkExp.EmpWorkExpFromYear
                                && uploadEmpWorkExp.EmpWorkExpFromMonth == currentEmpWorkExp.EmpWorkExpFromMonth
                                && uploadEmpWorkExp.EmpWorkExpToYear == currentEmpWorkExp.EmpWorkExpToYear
                                && uploadEmpWorkExp.EmpWorkExpToMonth == currentEmpWorkExp.EmpWorkExpToMonth
                                && uploadEmpWorkExp.EmpWorkExpCompanyName == currentEmpWorkExp.EmpWorkExpCompanyName
                                && uploadEmpWorkExp.EmpWorkExpPosition == currentEmpWorkExp.EmpWorkExpPosition
                                && uploadEmpWorkExp.EmpWorkExpEmploymentTypeID == currentEmpWorkExp.EmpWorkExpEmploymentTypeID
                                && uploadEmpWorkExp.EmpWorkExpIsRelevantExperience == currentEmpWorkExp.EmpWorkExpIsRelevantExperience
                                && uploadEmpWorkExp.EmpWorkExpRemark == currentEmpWorkExp.EmpWorkExpRemark
                                )
                            {
                                IsSameEntryExists = true;
                            }
                            //if (!string.IsNullOrEmpty(uploadEmpWorkExp.EmpWorkExpName) && !string.IsNullOrEmpty(currentEmpWorkExp.EmpWorkExpName))
                            //    if (currentEmpWorkExp.EmpWorkExpName.Equals(uploadEmpWorkExp.EmpWorkExpName, StringComparison.CurrentCultureIgnoreCase))
                            //    {
                            //        uploadEmpWorkExp.EmpWorkExpID = currentEmpWorkExp.EmpWorkExpID;
                            //        IsSameEntryExists = false;
                            //        break;
                            //    }
                        }
                        if (IsSameEntryExists)
                            continue;
                    }
                }

                if (uploadEmpWorkExp.EmpWorkExpID <= 0)
                    uploadEmpWorkExp.ImportActionStatus = ImportDBObject.ImportActionEnum.INSERT;
                else
                    uploadEmpWorkExp.ImportActionStatus = ImportDBObject.ImportActionEnum.UPDATE;

                uploadEmpWorkExp.UploadEmpID = ParseTemp.GetUploadEmpID(dbConn, EmpNo, m_SessionID);
                if (uploadEmpWorkExp.UploadEmpID == 0)
                    if (uploadEmpWorkExp.EmpID == 0)
                        errors.addError(ImportErrorMessage.ERROR_INVALID_EMP_NO, new string[] { EmpNo, rowCount.ToString() });
                    else
                        uploadEmpWorkExp.UploadEmpID = ImportEmpPersonalInfoProcess.CreateDummyUploadEmployeeInfo(dbConn, uploadEmpWorkExp.EmpID, m_SessionID, UploadDateTime);

                Hashtable values = new Hashtable();
                tempDB.populate(uploadEmpWorkExp, values);
                PageErrors pageErrors = new PageErrors(EUploadEmpPersonalInfo.db);
                tempDB.validate(pageErrors, values);
                if (pageErrors.errors.Count == 0)
                {

                    tempDB.insert(dbConn, uploadEmpWorkExp);
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
            ImportEmpWorkExpProcess import = new ImportEmpWorkExpProcess(dbConn, sessionID);
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
            ArrayList uploadEmpWorkExpList = tempDB.select(dbConn, sessionFilter);
            foreach (EUploadEmpWorkExp obj in uploadEmpWorkExpList)
            {
                EEmpWorkExp EmpWorkExp = new EEmpWorkExp();

                if (obj.ImportActionStatus != ImportDBObject.ImportActionEnum.INSERT)
                {
                    EmpWorkExp.EmpWorkExpID = obj.EmpWorkExpID;
                    uploadDB.select(dbConn, EmpWorkExp);
                }

                obj.ExportToObject(EmpWorkExp);

                if (obj.ImportActionStatus == ImportDBObject.ImportActionEnum.INSERT)
                {
                    EmpWorkExp.EmpID = ParseTemp.GetEmpIDFromUploadEmpID(dbConn, obj.UploadEmpID);
                    uploadDB.insert(dbConn, EmpWorkExp);


                }
                else if (obj.ImportActionStatus == ImportDBObject.ImportActionEnum.UPDATE)
                {
                    uploadDB.update(dbConn, EmpWorkExp);
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

            tmpDataTable.Columns.Add(FIELD_FROM_YEAR, typeof(int));
            tmpDataTable.Columns.Add(FIELD_FROM_MONTH, typeof(int));
            tmpDataTable.Columns.Add(FIELD_TO_YEAR, typeof(int));
            tmpDataTable.Columns.Add(FIELD_TO_MONTH, typeof(int));
            tmpDataTable.Columns.Add(FIELD_COMPANY, typeof(string));
            tmpDataTable.Columns.Add(FIELD_POSITION, typeof(string));
            tmpDataTable.Columns.Add(FIELD_EMPLOYMENT_TYPE, typeof(string));
            tmpDataTable.Columns.Add(FIELD_IS_RELEVANT_EXPERIENCE, typeof(string));
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
                    ArrayList list = EEmpWorkExp.db.select(dbConn, filter);
                    foreach (EEmpWorkExp empWorkExp in list)
                    {
                        DataRow row = tmpDataTable.NewRow();
                        //if (IsIncludeInternalID)
                        //    row[FIELD_INTERNAL_ID] = ToHexDecWithCheckDigit(empWorkExp.EmpWorkExpID);
                        row[FIELD_EMP_NO] = empInfo.EmpNo;

                        if (IsIncludeCurrentPositionInfo)
                        {
                            ImportEmpPersonalInfoProcess.AddEmployeeInfo(dbConn, row, empInfo.EmpID);
                            ImportEmpPositionInfoProcess.AddEmployeePositionInfo(dbConn, row, empInfo.EmpID);
                        }

                        if (empWorkExp.EmpWorkExpFromYear != 0)
                            row[FIELD_FROM_YEAR] = empWorkExp.EmpWorkExpFromYear;
                        if (empWorkExp.EmpWorkExpFromMonth != 0)
                            row[FIELD_FROM_MONTH] = empWorkExp.EmpWorkExpFromMonth;
                        if (empWorkExp.EmpWorkExpToYear != 0)
                            row[FIELD_TO_YEAR] = empWorkExp.EmpWorkExpToYear;
                        if (empWorkExp.EmpWorkExpToMonth != 0)
                            row[FIELD_TO_MONTH] = empWorkExp.EmpWorkExpToMonth;

                        row[FIELD_COMPANY] = empWorkExp.EmpWorkExpCompanyName;
                        row[FIELD_POSITION] = empWorkExp.EmpWorkExpPosition;

                        EEmploymentType employmentType = new EEmploymentType();
                        if (employmentType.LoadDBObject(dbConn, empWorkExp.EmpWorkExpEmploymentTypeID))
                            row[FIELD_EMPLOYMENT_TYPE] = IsShowDescription ? employmentType.EmploymentTypeDesc : employmentType.EmploymentTypeCode;
                        row[FIELD_IS_RELEVANT_EXPERIENCE] = empWorkExp.EmpWorkExpIsRelevantExperience ? "Yes" : "No";
                        row[FIELD_REMARK] = empWorkExp.EmpWorkExpRemark;

                        if (IsIncludeSyncID)
                            row[FIELD_SYNC_ID] = empWorkExp.SynID;

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
