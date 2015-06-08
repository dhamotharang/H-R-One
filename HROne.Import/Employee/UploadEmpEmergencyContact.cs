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
    public class ImportEmpEmergencyContactProcess : ImportProcessInteface
    {

        [DBClass("UploadEmpEmergencyContact")]
        private class EUploadEmpEmergencyContact : ImportDBObject
        {
            public static DBManager db = new DBManager(typeof(EUploadEmpEmergencyContact));

            protected int m_UploadEmpEmergencyContactID;
            [DBField("UploadEmpEmergencyContactID", true, true), TextSearch, Export(false)]
            public int UploadEmpEmergencyContactID
            {
                get { return m_UploadEmpEmergencyContactID; }
                set { m_UploadEmpEmergencyContactID = value; modify("UploadEmpEmergencyContactID"); }
            }
            protected int m_UploadEmpID;
            [DBField("UploadEmpID"), TextSearch, Export(false)]
            public int UploadEmpID
            {
                get { return m_UploadEmpID; }
                set { m_UploadEmpID = value; modify("UploadEmpID"); }
            }

            protected int m_EmpEmergencyContactID;
            [DBField("EmpEmergencyContactID"), TextSearch, Export(false)]
            public int EmpEmergencyContactID
            {
                get { return m_EmpEmergencyContactID; }
                set { m_EmpEmergencyContactID = value; modify("EmpEmergencyContactID"); }
            }
            protected int m_EmpID;
            [DBField("EmpID"), TextSearch, Export(false)]
            public int EmpID
            {
                get { return m_EmpID; }
                set { m_EmpID = value; modify("EmpID"); }
            }
            protected string m_EmpEmergencyContactName;
            [DBField("EmpEmergencyContactName"), TextSearch, MaxLength(50, 25), Export(false), Required]
            public string EmpEmergencyContactName
            {
                get { return m_EmpEmergencyContactName; }
                set { m_EmpEmergencyContactName = value; modify("EmpEmergencyContactName"); }
            }
            protected string m_EmpEmergencyContactGender;
            [DBField("EmpEmergencyContactGender"), TextSearch, Export(false)]
            public string EmpEmergencyContactGender
            {
                get { return m_EmpEmergencyContactGender; }
                set { m_EmpEmergencyContactGender = value; modify("EmpEmergencyContactGender"); }
            }
            protected string m_EmpEmergencyContactRelationship;
            [DBField("EmpEmergencyContactRelationship"), TextSearch, MaxLength(100, 25), Export(false)]
            public string EmpEmergencyContactRelationship
            {
                get { return m_EmpEmergencyContactRelationship; }
                set { m_EmpEmergencyContactRelationship = value; modify("EmpEmergencyContactRelationship"); }
            }
            protected string m_EmpEmergencyContactContactNoDay;
            [DBField("EmpEmergencyContactContactNoDay"), TextSearch, MaxLength(100, 25), Export(false), Required]
            public string EmpEmergencyContactContactNoDay
            {
                get { return m_EmpEmergencyContactContactNoDay; }
                set { m_EmpEmergencyContactContactNoDay = value; modify("EmpEmergencyContactContactNoDay"); }
            }
            protected string m_EmpEmergencyContactContactNoNight;
            [DBField("EmpEmergencyContactContactNoNight"), TextSearch, MaxLength(100, 25), Export(false)]
            public string EmpEmergencyContactContactNoNight
            {
                get { return m_EmpEmergencyContactContactNoNight; }
                set { m_EmpEmergencyContactContactNoNight = value; modify("EmpEmergencyContactContactNoNight"); }
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

        public const string TABLE_NAME = "EmergencyContact";

        //  Field Const for Employee Personal Information
        private const string FIELD_EMP_NO = "Emp No";
        private const string FIELD_NAME = "Name";
        private const string FIELD_GENDER = "Gender";
        private const string FIELD_RELATIONSHIP = "Relationship";
        private const string FIELD_CONTACT_NO_DAY = "Phone Number (Day)";
        private const string FIELD_CONTACT_NO_NIGHT = "Phone Number (Night)";

        
        private DateTime UploadDateTime = AppUtils.ServerDateTime();
        private DBManager tempDB = EUploadEmpEmergencyContact.db;
        private DBManager uploadDB = EEmpEmergencyContact.db;

        public ImportErrorList errors = new ImportErrorList();
        

        public ImportEmpEmergencyContactProcess(DatabaseConnection dbConn, string SessionID): base(dbConn, SessionID)
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

                EUploadEmpEmergencyContact uploadEmpEmergencyContact = new EUploadEmpEmergencyContact();
                //EEmpEmergencyContact lastEmpEmergencyContact = null;

                string EmpNo = row[FIELD_EMP_NO].ToString().Trim();
                uploadEmpEmergencyContact.EmpID = Parse.GetEmpID(dbConn, EmpNo, UserID);
                if (uploadEmpEmergencyContact.EmpID < 0)
                    errors.addError(ImportErrorMessage.ERROR_ACCESS_DENIED_EMP_NO, new string[] { EmpNo, rowCount.ToString() });
                uploadEmpEmergencyContact.EmpEmergencyContactName = row[FIELD_NAME].ToString();

                string tempString = row[FIELD_GENDER].ToString();
                if (!String.IsNullOrEmpty(tempString))
                    if (tempString.Equals("Male", StringComparison.CurrentCultureIgnoreCase)
                        || tempString.Equals("M", StringComparison.CurrentCultureIgnoreCase))
                        uploadEmpEmergencyContact.EmpEmergencyContactGender = "M";
                    else if (tempString.Equals("Female", StringComparison.CurrentCultureIgnoreCase)
                        || tempString.Equals("F", StringComparison.CurrentCultureIgnoreCase))
                        uploadEmpEmergencyContact.EmpEmergencyContactGender = "F";
                    else
                        uploadEmpEmergencyContact.EmpEmergencyContactGender = string.Empty;
                //else
                    //    errors.addError(ImportErrorMessage.ERROR_INVALID_GENDER, new string[] { tempString, rowCount.ToString() });

                uploadEmpEmergencyContact.EmpEmergencyContactRelationship = row[FIELD_RELATIONSHIP].ToString();


                uploadEmpEmergencyContact.EmpEmergencyContactContactNoDay = row[FIELD_CONTACT_NO_DAY].ToString();
                uploadEmpEmergencyContact.EmpEmergencyContactContactNoNight = row[FIELD_CONTACT_NO_NIGHT].ToString();

                uploadEmpEmergencyContact.SessionID = m_SessionID;
                uploadEmpEmergencyContact.TransactionDate = UploadDateTime;


                if (uploadEmpEmergencyContact.EmpID != 0)
                {
                    //if (rawDataTable.Columns.Contains(FIELD_INTERNAL_ID))
                    //{
                    //    try
                    //    {
                    //        if (!row.IsNull(FIELD_INTERNAL_ID))
                    //        {
                    //            int tmpID = FromHexDecWithCheckDigit((string)row[FIELD_INTERNAL_ID]);
                    //            EEmpEmergencyContact tmpObj = new EEmpEmergencyContact();
                    //            tmpObj.EmpEmergencyContactID = tmpID;
                    //            if (EEmpEmergencyContact.db.select(dbConn, tmpObj))
                    //                uploadEmpEmergencyContact.EmpEmergencyContactID = tmpID;
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
                            uploadEmpEmergencyContact.SynID = strSynID;
                            if (!string.IsNullOrEmpty(strSynID))
                            {
                                DBFilter synIDFilter = new DBFilter();
                                synIDFilter.add(new Match("SynID", strSynID));
                                ArrayList objSameSynIDList = EEmpEmergencyContact.db.select(dbConn, synIDFilter);
                                if (objSameSynIDList.Count > 0)
                                    uploadEmpEmergencyContact.EmpEmergencyContactID = ((EEmpEmergencyContact)objSameSynIDList[0]).EmpEmergencyContactID;
                            }
                        }

                    }

                    if (uploadEmpEmergencyContact.EmpEmergencyContactID == 0)
                    {
                        DBFilter filter = new DBFilter();
                        filter.add(new Match("EmpID", uploadEmpEmergencyContact.EmpID));
                        ArrayList list = uploadDB.select(dbConn, filter);
                        bool IsSameEntryExists = false;
                        foreach (EEmpEmergencyContact currentEmpEmergencyContact in list)
                        {

                            if (uploadEmpEmergencyContact.EmpEmergencyContactName == currentEmpEmergencyContact.EmpEmergencyContactName
                                && uploadEmpEmergencyContact.EmpEmergencyContactRelationship == currentEmpEmergencyContact.EmpEmergencyContactRelationship
                                && uploadEmpEmergencyContact.EmpEmergencyContactGender == currentEmpEmergencyContact.EmpEmergencyContactGender
                                && uploadEmpEmergencyContact.EmpEmergencyContactContactNoDay == currentEmpEmergencyContact.EmpEmergencyContactContactNoDay
                                && uploadEmpEmergencyContact.EmpEmergencyContactContactNoNight == currentEmpEmergencyContact.EmpEmergencyContactContactNoNight
                                )
                            {
                                IsSameEntryExists = true;
                            }
                            if (!string.IsNullOrEmpty(uploadEmpEmergencyContact.EmpEmergencyContactName) && !string.IsNullOrEmpty(currentEmpEmergencyContact.EmpEmergencyContactName))
                                if (currentEmpEmergencyContact.EmpEmergencyContactName.Equals(uploadEmpEmergencyContact.EmpEmergencyContactName, StringComparison.CurrentCultureIgnoreCase))
                                {
                                    uploadEmpEmergencyContact.EmpEmergencyContactID = currentEmpEmergencyContact.EmpEmergencyContactID;
                                    IsSameEntryExists = false;
                                    break;
                                }
                        }
                        if (IsSameEntryExists)
                            continue;
                    }
                }

                if (uploadEmpEmergencyContact.EmpEmergencyContactID <= 0)
                    uploadEmpEmergencyContact.ImportActionStatus = ImportDBObject.ImportActionEnum.INSERT;
                else
                    uploadEmpEmergencyContact.ImportActionStatus = ImportDBObject.ImportActionEnum.UPDATE;

                uploadEmpEmergencyContact.UploadEmpID = ParseTemp.GetUploadEmpID(dbConn, EmpNo, m_SessionID);
                if (uploadEmpEmergencyContact.UploadEmpID == 0)
                    if (uploadEmpEmergencyContact.EmpID == 0)
                        errors.addError(ImportErrorMessage.ERROR_INVALID_EMP_NO, new string[] { EmpNo, rowCount.ToString() });
                    else
                        uploadEmpEmergencyContact.UploadEmpID = ImportEmpPersonalInfoProcess.CreateDummyUploadEmployeeInfo(dbConn, uploadEmpEmergencyContact.EmpID, m_SessionID, UploadDateTime);

                Hashtable values = new Hashtable();
                tempDB.populate(uploadEmpEmergencyContact, values);
                PageErrors pageErrors = new PageErrors(EUploadEmpPersonalInfo.db);
                tempDB.validate(pageErrors, values);
                if (pageErrors.errors.Count == 0)
                {

                    tempDB.insert(dbConn, uploadEmpEmergencyContact);
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
            ImportEmpEmergencyContactProcess import = new ImportEmpEmergencyContactProcess(dbConn, sessionID);
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
            ArrayList uploadEmpEmergencyContactList = tempDB.select(dbConn, sessionFilter);
            foreach (EUploadEmpEmergencyContact obj in uploadEmpEmergencyContactList)
            {
                EEmpEmergencyContact EmpEmergencyContact = new EEmpEmergencyContact();

                if (obj.ImportActionStatus != ImportDBObject.ImportActionEnum.INSERT)
                {
                    EmpEmergencyContact.EmpEmergencyContactID = obj.EmpEmergencyContactID;
                    uploadDB.select(dbConn, EmpEmergencyContact);
                }

                obj.ExportToObject(EmpEmergencyContact);

                if (obj.ImportActionStatus == ImportDBObject.ImportActionEnum.INSERT)
                {
                    EmpEmergencyContact.EmpID = ParseTemp.GetEmpIDFromUploadEmpID(dbConn, obj.UploadEmpID);
                    uploadDB.insert(dbConn, EmpEmergencyContact);


                }
                else if (obj.ImportActionStatus == ImportDBObject.ImportActionEnum.UPDATE)
                {
                    uploadDB.update(dbConn, EmpEmergencyContact);
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

            tmpDataTable.Columns.Add(FIELD_NAME, typeof(string));
            tmpDataTable.Columns.Add(FIELD_GENDER, typeof(string));
            tmpDataTable.Columns.Add(FIELD_RELATIONSHIP, typeof(string));
            tmpDataTable.Columns.Add(FIELD_CONTACT_NO_DAY, typeof(string));
            tmpDataTable.Columns.Add(FIELD_CONTACT_NO_NIGHT, typeof(string));

            if (IsIncludeSyncID)
                tmpDataTable.Columns.Add(FIELD_SYNC_ID, typeof(string));

            foreach (EEmpPersonalInfo empInfo in empList)
            {
                if (EEmpPersonalInfo.db.select(dbConn, empInfo))
                {
                    DBFilter filter = new DBFilter();
                    filter.add(new Match("EmpID", empInfo.EmpID));
                    filter.add(getCreateModifiedRecordsAfterDBTerm(ReferenceDateTime));
                    ArrayList list = EEmpEmergencyContact.db.select(dbConn, filter);
                    foreach (EEmpEmergencyContact empEmergencyContact in list)
                    {
                        DataRow row = tmpDataTable.NewRow();
                        //if (IsIncludeInternalID)
                        //    row[FIELD_INTERNAL_ID] = ToHexDecWithCheckDigit(empEmergencyContact.EmpEmergencyContactID);
                        row[FIELD_EMP_NO] = empInfo.EmpNo;

                        if (IsIncludeCurrentPositionInfo)
                        {
                            ImportEmpPersonalInfoProcess.AddEmployeeInfo(dbConn, row, empInfo.EmpID);
                            ImportEmpPositionInfoProcess.AddEmployeePositionInfo(dbConn, row, empInfo.EmpID);
                        }

                        row[FIELD_NAME] = empEmergencyContact.EmpEmergencyContactName;
                        row[FIELD_GENDER] = empEmergencyContact.EmpEmergencyContactGender;
                        row[FIELD_RELATIONSHIP] = empEmergencyContact.EmpEmergencyContactRelationship;
                        row[FIELD_CONTACT_NO_DAY] = empEmergencyContact.EmpEmergencyContactContactNoDay ;
                        row[FIELD_CONTACT_NO_NIGHT] = empEmergencyContact.EmpEmergencyContactContactNoNight ;
                        if (IsIncludeSyncID)
                            row[FIELD_SYNC_ID] = empEmergencyContact.SynID;

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
