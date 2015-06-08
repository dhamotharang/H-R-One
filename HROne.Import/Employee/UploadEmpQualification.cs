using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Text;
using HROne.DataAccess;
//using perspectivemind.validation;
using HROne.Lib.Entities;
using System.Globalization;

namespace HROne.Import
{
        /// <summary>
    /// Summary description for ImportEmpQualificationProcess
    /// </summary>
    public class ImportEmpQualificationProcess : ImportProcessInteface
    {
        [DBClass("UploadEmpQualification")]
        private class EUploadEmpQualification : ImportDBObject
        {
            public static DBManager db = new DBManager(typeof(EUploadEmpQualification));

            protected int m_UploadEmpQualificationID;
            [DBField("UploadEmpQualificationID", true, true), TextSearch, Export(false)]
            public int UploadEmpQualificationID
            {
                get { return m_UploadEmpQualificationID; }
                set { m_UploadEmpQualificationID = value; modify("UploadEmpQualificationID"); }
            }
            protected int m_UploadEmpID;
            [DBField("UploadEmpID"), TextSearch, Export(false)]
            public int UploadEmpID
            {
                get { return m_UploadEmpID; }
                set { m_UploadEmpID = value; modify("UploadEmpID"); }
            }

            protected int m_EmpQualificationID;
            [DBField("EmpQualificationID"), TextSearch, Export(false)]
            public int EmpQualificationID
            {
                get { return m_EmpQualificationID; }
                set { m_EmpQualificationID = value; modify("EmpQualificationID"); }
            }
            protected int m_EmpID;
            [DBField("EmpID"), TextSearch, Export(false)]
            public int EmpID
            {
                get { return m_EmpID; }
                set { m_EmpID = value; modify("EmpID"); }
            }
            protected int m_QualificationID;
            [DBField("QualificationID"), TextSearch, Export(false), Required]
            public int QualificationID
            {
                get { return m_QualificationID; }
                set { m_QualificationID = value; modify("QualificationID"); }
            }
            protected DateTime m_EmpQualificationFrom;
            [DBField("EmpQualificationFrom"), TextSearch, MaxLength(25), Export(false)]
            public DateTime EmpQualificationFrom
            {
                get { return m_EmpQualificationFrom; }
                set { m_EmpQualificationFrom = value; modify("EmpQualificationFrom"); }
            }
            protected DateTime m_EmpQualificationTo;
            [DBField("EmpQualificationTo"), TextSearch, MaxLength(25), Export(false)]
            public DateTime EmpQualificationTo
            {
                get { return m_EmpQualificationTo; }
                set { m_EmpQualificationTo = value; modify("EmpQualificationTo"); }
            }
            protected string m_EmpQualificationInstitution;
            [DBField("EmpQualificationInstitution"), TextSearch, MaxLength(100, 25), Export(false)]
            public string EmpQualificationInstitution
            {
                get { return m_EmpQualificationInstitution; }
                set { m_EmpQualificationInstitution = value; modify("EmpQualificationInstitution"); }
            }
            protected string m_EmpQualificationLearningMethod;
            [DBField("EmpQualificationLearningMethod"), TextSearch, Export(false)]
            public string EmpQualificationLearningMethod
            {
                get { return m_EmpQualificationLearningMethod; }
                set { m_EmpQualificationLearningMethod = value; modify("EmpQualificationLearningMethod"); }
            }
            protected string m_EmpQualificationRemark;
            [DBField("EmpQualificationRemark"), TextSearch, Export(false)]
            public string EmpQualificationRemark
            {
                get { return m_EmpQualificationRemark; }
                set { m_EmpQualificationRemark = value; modify("EmpQualificationRemark"); }
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

        public const string TABLE_NAME = "qualification";

        //  Field Const for Employee Personal Information
        private const string FIELD_EMP_NO = "Emp No";
        private const string FIELD_FROM = "From";
        private const string FIELD_TO = "To";
        private const string FIELD_QUALIFICATION = "Qualification";
        private const string FIELD_INSTITUTION = "Institution";
        private const string FIELD_LEARNING_METHOD = "Learning Method";
        private const string FIELD_REMARK = "Remark";

        
        private DateTime UploadDateTime = AppUtils.ServerDateTime();
        private DBManager tempDB = EUploadEmpQualification.db;
        private DBManager uploadDB = EEmpQualification.db;

        public ImportErrorList errors = new ImportErrorList();
        

        public ImportEmpQualificationProcess(DatabaseConnection dbConn, string SessionID): base(dbConn, SessionID)
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

                EUploadEmpQualification uploadEmpQualification = new EUploadEmpQualification();
                //EEmpQualification lastEmpQualification = null;

                string EmpNo = row[FIELD_EMP_NO].ToString().Trim();
                uploadEmpQualification.EmpID = Parse.GetEmpID(dbConn, EmpNo, UserID);
                if (uploadEmpQualification.EmpID < 0)
                    errors.addError(ImportErrorMessage.ERROR_ACCESS_DENIED_EMP_NO, new string[] { EmpNo, rowCount.ToString() });
                try
                {
                    uploadEmpQualification.EmpQualificationFrom = Parse.toDateTimeObject(row[FIELD_FROM]);
                }
                catch
                {
                    errors.addError(ImportErrorMessage.ERROR_INVALID_FIELD_VALUE, new string[] { FIELD_FROM + "=" + row[FIELD_FROM].ToString(), EmpNo, rowCount.ToString() });
                }
                try
                {
                    uploadEmpQualification.EmpQualificationTo = Parse.toDateTimeObject(row[FIELD_TO]);
                }
                catch
                {
                    errors.addError(ImportErrorMessage.ERROR_INVALID_FIELD_VALUE, new string[] { FIELD_TO + "=" + row[FIELD_TO].ToString(), EmpNo, rowCount.ToString() });
                }
                uploadEmpQualification.QualificationID = Parse.GetQualificationID(dbConn, row[FIELD_QUALIFICATION].ToString(), CreateCodeIfNotExists, UserID);

                if (uploadEmpQualification.QualificationID == 0)
                    errors.addError(ImportErrorMessage.ERROR_INVALID_FIELD_VALUE, new string[] { FIELD_QUALIFICATION + "=" + row[FIELD_QUALIFICATION].ToString(), EmpNo, rowCount.ToString() });


                uploadEmpQualification.EmpQualificationInstitution = row[FIELD_INSTITUTION].ToString();

                if (rawDataTable.Columns.Contains(FIELD_LEARNING_METHOD))
                {
                    uploadEmpQualification.EmpQualificationLearningMethod = Import.Parse.toQualificationLearningMethod(row[FIELD_LEARNING_METHOD].ToString());
                }
                uploadEmpQualification.EmpQualificationRemark = row[FIELD_REMARK].ToString().Trim();



                uploadEmpQualification.SessionID = m_SessionID;
                uploadEmpQualification.TransactionDate = UploadDateTime;


                //  Remove the checking. Allow to more than 1 QualificationID per record.
                //if (uploadEmpQualification.EmpID != 0 && !uploadEmpQualification.EmpQualificationFrom.Ticks.Equals(0))
                //{
                //    AND andTerms = new AND();
                //    andTerms.add(new Match("EmpQualificationFrom", "<=", uploadEmpQualification.EmpQualificationFrom));
                //    andTerms.add(new Match("QualificationID", uploadEmpQualification.QualificationID));

                //    lastEmpQualification = (EEmpQualification)AppUtils.GetLastObj(dbConn, uploadDB, "EmpQualificationFrom", uploadEmpQualification.EmpID, andTerms);


                //    if (lastEmpQualification != null)
                //    {
                //        if (uploadEmpQualification.EmpQualificationInstitution == lastEmpQualification.EmpQualificationInstitution
                //            && uploadEmpQualification.EmpQualificationRemark == lastEmpQualification.EmpQualificationRemark
                //            )
                //            continue;
                //        else
                //        {
                //            // add postion terms with new ID
                //            if (lastEmpQualification.EmpQualificationFrom.Equals(uploadEmpQualification.EmpQualificationFrom))
                //            {
                //                uploadEmpQualification.EmpQualificationID = lastEmpQualification.EmpQualificationID;
                //                if (uploadEmpQualification.EmpQualificationTo.Ticks == 0)
                //                    uploadEmpQualification.EmpQualificationTo = lastEmpQualification.EmpQualificationTo;
                //            }
                //            else
                //            {
                //                AND lastObjAndTerms = new AND();
                //                lastObjAndTerms.add(new Match("EmpQualificationFrom", "<=", uploadEmpQualification.EmpQualificationFrom));
                //                lastObjAndTerms.add(new Match("QualificationID", uploadEmpQualification.QualificationID));
                //                EEmpQualification lastObj = (EEmpQualification)AppUtils.GetLastObj(dbConn, uploadDB, "EmpQualificationFrom", uploadEmpQualification.EmpID, lastObjAndTerms);
                //                if (lastObj != null && uploadEmpQualification.EmpQualificationFrom <= lastObj.EmpQualificationFrom)
                //                {
                //                    errors.addError(ImportErrorMessage.ERROR_DATE_FROM_OVERLAP, new string[] { uploadEmpQualification.EmpQualificationFrom.ToString("yyyy-MM-dd"), rowCount.ToString() });
                //                    continue;
                //                }
                //            }
                //        }
                //    }
                //}

                //if (rawDataTable.Columns.Contains(FIELD_INTERNAL_ID))
                //{
                //    try
                //    {
                //        if (!row.IsNull(FIELD_INTERNAL_ID))
                //        {
                //            int tmpID = FromHexDecWithCheckDigit((string)row[FIELD_INTERNAL_ID]);
                //            EEmpQualification tmpObj = new EEmpQualification();
                //            tmpObj.EmpQualificationID = tmpID;
                //            if (EEmpQualification.db.select(dbConn, tmpObj))
                //                uploadEmpQualification.EmpQualificationID = tmpID;
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
                        uploadEmpQualification.SynID = strSynID;
                        if (!string.IsNullOrEmpty(strSynID))
                        {
                            DBFilter synIDFilter = new DBFilter();
                            synIDFilter.add(new Match("SynID", strSynID));
                            ArrayList objSameSynIDList = EEmpQualification.db.select(dbConn, synIDFilter);
                            if (objSameSynIDList.Count > 0)
                                uploadEmpQualification.EmpQualificationID = ((EEmpQualification)objSameSynIDList[0]).EmpQualificationID;
                        }
                    }

                }

                if (uploadEmpQualification.EmpQualificationID <= 0)
                    uploadEmpQualification.ImportActionStatus = ImportDBObject.ImportActionEnum.INSERT;
                else
                    uploadEmpQualification.ImportActionStatus = ImportDBObject.ImportActionEnum.UPDATE;

                uploadEmpQualification.UploadEmpID = ParseTemp.GetUploadEmpID(dbConn, EmpNo, m_SessionID);
                if (uploadEmpQualification.UploadEmpID == 0)
                    if (uploadEmpQualification.EmpID == 0)
                        errors.addError(ImportErrorMessage.ERROR_INVALID_EMP_NO, new string[] { EmpNo, rowCount.ToString() });
                    else
                        uploadEmpQualification.UploadEmpID = ImportEmpPersonalInfoProcess.CreateDummyUploadEmployeeInfo(dbConn, uploadEmpQualification.EmpID, m_SessionID, UploadDateTime);

                Hashtable values = new Hashtable();
                tempDB.populate(uploadEmpQualification, values);
                PageErrors pageErrors = new PageErrors(tempDB);
                tempDB.validate(pageErrors, values);
                if (pageErrors.errors.Count == 0)
                {

                    tempDB.insert(dbConn, uploadEmpQualification);
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
            ImportEmpQualificationProcess import = new ImportEmpQualificationProcess(dbConn, sessionID);
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
            ArrayList uploadEmppQualificationList = tempDB.select(dbConn, sessionFilter);
            foreach (EUploadEmpQualification obj in uploadEmppQualificationList)
            {
                EEmpQualification empQualification = new EEmpQualification();

                if (obj.ImportActionStatus != ImportDBObject.ImportActionEnum.INSERT)
                {
                    empQualification.EmpQualificationID = obj.EmpQualificationID;
                    uploadDB.select(dbConn, empQualification);
                }

                obj.ExportToObject(empQualification);

                if (obj.ImportActionStatus == ImportDBObject.ImportActionEnum.INSERT)
                {


                    //AND andTerms = new AND();
                    //andTerms.add(new Match("EmpQualificationFrom", "<=", empQualification.EmpQualificationFrom));
                    //andTerms.add(new Match("QualificationID", empQualification.QualificationID));

                    //EEmpQualification lastObj = (EEmpQualification)AppUtils.GetLastObj(dbConn, EEmpQualification.db, "EmpQualificationFrom", empQualification.EmpID, andTerms);
                    //if (lastObj != null)
                    //    if (lastObj.EmpQualificationTo.Ticks == 0)
                    //    {
                    //        lastObj.EmpQualificationTo = empQualification.EmpQualificationFrom.AddDays(-1);
                    //        uploadDB.update(dbConn, lastObj);
                    //    }

                    empQualification.EmpID = ParseTemp.GetEmpIDFromUploadEmpID(dbConn, obj.UploadEmpID);
                    uploadDB.insert(dbConn, empQualification);
                    //DBFilter emplastPosFilter = new DBFilter();
                }
                else if (obj.ImportActionStatus == ImportDBObject.ImportActionEnum.UPDATE)
                {
                    uploadDB.update(dbConn, empQualification);

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
            tmpDataTable.Columns.Add(FIELD_QUALIFICATION, typeof(string));
            tmpDataTable.Columns.Add(FIELD_INSTITUTION, typeof(string));
            tmpDataTable.Columns.Add(FIELD_LEARNING_METHOD, typeof(string));
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
                    ArrayList list = EEmpQualification.db.select(dbConn, filter);
                    foreach (EEmpQualification empQualification in list)
                    {
                        DataRow row = tmpDataTable.NewRow();
                        //if (IsIncludeInternalID)
                        //    row[FIELD_INTERNAL_ID] = ToHexDecWithCheckDigit(empQualification.EmpQualificationID);
                        row[FIELD_EMP_NO] = empInfo.EmpNo;

                        if (IsIncludeCurrentPositionInfo)
                        {
                            ImportEmpPersonalInfoProcess.AddEmployeeInfo(dbConn, row, empInfo.EmpID);
                            ImportEmpPositionInfoProcess.AddEmployeePositionInfo(dbConn, row, empInfo.EmpID);
                        }

                        row[FIELD_FROM] = empQualification.EmpQualificationFrom;
                        row[FIELD_TO] = empQualification.EmpQualificationTo;

                        EQualification qualification = new EQualification();
                        qualification.QualificationID = empQualification.QualificationID;
                        if (EQualification.db.select(dbConn, qualification))
                            row[FIELD_QUALIFICATION] = IsShowDescription ? qualification.QualificationDesc : qualification.QualificationCode;
                        row[FIELD_INSTITUTION] = empQualification.EmpQualificationInstitution;

                        if (!string.IsNullOrEmpty(empQualification.EmpQualificationLearningMethod))
                        {
                            if (empQualification.EmpQualificationLearningMethod.Equals(EEmpQualification.LEARNING_METHOD_CODE_ONCAMPUS))
                                row[FIELD_LEARNING_METHOD] = EEmpQualification.LEARNING_METHOD_DESC_ONCAMPUS;
                            else if (empQualification.EmpQualificationLearningMethod.Equals(EEmpQualification.LEARNING_METHOD_CODE_DISTANCE_LEARNING))
                                row[FIELD_LEARNING_METHOD] = EEmpQualification.LEARNING_METHOD_DESC_DISTANCE_LEARNING;
                        }

                        row[FIELD_REMARK] = empQualification.EmpQualificationRemark;
                        if (IsIncludeSyncID)
                            row[FIELD_SYNC_ID] = empQualification.SynID;

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
