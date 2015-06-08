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
    public class ImportEmpSpouseProcess : ImportProcessInteface
    {


        [DBClass("UploadEmpSpouse")]
        private class EUploadEmpSpouse : ImportDBObject
        {
            public static DBManager db = new DBManager(typeof(EUploadEmpSpouse));

            protected int m_UploadEmpSpouseID;
            [DBField("UploadEmpSpouseID", true, true), TextSearch, Export(false)]
            public int UploadEmpSpouseID
            {
                get { return m_UploadEmpSpouseID; }
                set { m_UploadEmpSpouseID = value; modify("UploadEmpSpouseID"); }
            }


            protected int m_UploadEmpID;
            [DBField("UploadEmpID"), TextSearch, Export(false)]
            public int UploadEmpID
            {
                get { return m_UploadEmpID; }
                set { m_UploadEmpID = value; modify("UploadEmpID"); }
            }

            protected int m_EmpSpouseID;
            [DBField("EmpSpouseID"), TextSearch, Export(false)]
            public int EmpSpouseID
            {
                get { return m_EmpSpouseID; }
                set { m_EmpSpouseID = value; modify("EmpSpouseID"); }
            }


            protected int m_EmpID;
            [DBField("EmpID"), TextSearch, Export(false)]
            public int EmpID
            {
                get { return m_EmpID; }
                set { m_EmpID = value; modify("EmpID"); }
            }
            protected string m_EmpSpouseSurname;
            [DBField("EmpSpouseSurname"), TextSearch, MaxLength(20, 25), Export(false), Required]
            public string EmpSpouseSurname
            {
                get { return m_EmpSpouseSurname; }
                set { m_EmpSpouseSurname = value; modify("EmpSpouseSurname"); }
            }
            protected string m_EmpSpouseOtherName;
            [DBField("EmpSpouseOtherName"), TextSearch, MaxLength(40, 25), Export(false), Required]
            public string EmpSpouseOtherName
            {
                get { return m_EmpSpouseOtherName; }
                set { m_EmpSpouseOtherName = value; modify("EmpSpouseOtherName"); }
            }
            protected string m_EmpSpouseChineseName;
            [DBField("EmpSpouseChineseName"), TextSearch, MaxLength(50, 25), Export(false)]
            public string EmpSpouseChineseName
            {
                get { return m_EmpSpouseChineseName; }
                set { m_EmpSpouseChineseName = value; modify("EmpSpouseChineseName"); }
            }
            protected DateTime m_EmpSpouseDateOfBirth;
            [DBField("EmpSpouseDateOfBirth"), TextSearch, MaxLength(10, 10), Export(false)]
            public DateTime EmpSpouseDateOfBirth
            {
                get { return m_EmpSpouseDateOfBirth; }
                set { m_EmpSpouseDateOfBirth = value; modify("EmpSpouseDateOfBirth"); }
            }
            protected string m_EmpSpouseHKID;
            [DBField("EmpSpouseHKID"), TextSearch, MaxLength(12, 25), Export(false)]
            public string EmpSpouseHKID
            {
                get { return m_EmpSpouseHKID; }
                set { m_EmpSpouseHKID = value; modify("EmpSpouseHKID"); }
            }
            protected string m_EmpSpousePassportNo;
            [DBField("EmpSpousePassportNo"), TextSearch, MaxLength(40, 25), Export(false)]
            public string EmpSpousePassportNo
            {
                get { return m_EmpSpousePassportNo; }
                set { m_EmpSpousePassportNo = value; modify("EmpSpousePassportNo"); }
            }
            protected string m_EmpSpousePassportIssuedCountry;
            [DBField("EmpSpousePassportIssuedCountry"), TextSearch, MaxLength(40, 25), Export(false)]
            public string EmpSpousePassportIssuedCountry
            {
                get { return m_EmpSpousePassportIssuedCountry; }
                set { m_EmpSpousePassportIssuedCountry = value; modify("EmpSpousePassportIssuedCountry"); }
            }

            // Start 0000142, KuangWei, 2014-12-21
            protected string m_EmpGender;
            [DBField("EmpGender"), DBAESEncryptStringField, TextSearch, Export(false)]
            public string EmpGender
            {
                get { return m_EmpGender; }
                set { m_EmpGender = value; modify("EmpGender"); }
            }

            protected bool m_EmpIsMedicalSchemaInsured;
            [DBField("EmpIsMedicalSchemaInsured"), TextSearch, Export(false)]
            public bool EmpIsMedicalSchemaInsured
            {
                get { return m_EmpIsMedicalSchemaInsured; }
                set { m_EmpIsMedicalSchemaInsured = value; modify("EmpIsMedicalSchemaInsured"); }
            }

            protected DateTime m_EmpMedicalEffectiveDate;
            [DBField("EmpMedicalEffectiveDate"), TextSearch, Export(false)]
            public DateTime EmpMedicalEffectiveDate
            {
                get { return m_EmpMedicalEffectiveDate; }
                set { m_EmpMedicalEffectiveDate = value; modify("EmpMedicalEffectiveDate"); }
            }

            protected DateTime m_EmpMedicalExpiryDate;
            [DBField("EmpMedicalExpiryDate"), TextSearch, Export(false)]
            public DateTime EmpMedicalExpiryDate
            {
                get { return m_EmpMedicalExpiryDate; }
                set { m_EmpMedicalExpiryDate = value; modify("EmpMedicalExpiryDate"); }
            }
            // End 0000142, KuangWei, 2014-12-21

            //  For Synchronize Use only
            protected string m_SynID;
            [DBField("SynID"), TextSearch, Export(false)]
            public string SynID
            {
                get { return m_SynID; }
                set { m_SynID = value; modify("SynID"); }
            }
        }

        public const string TABLE_NAME = "spouse";

        //  Field Const for Employee Personal Information
        private const string FIELD_EMP_NO = "Emp No";
        private const string FIELD_SURNAME = "Surname";
        private const string FIELD_OTHER_NAME = "OtherName";
        private const string FIELD_CHINESE_NAME = "Chinese Name";
        private const string FIELD_DATE_OF_BIRTH = "Date of Birth";
        private const string FIELD_HKID = "HKID";
        private const string FIELD_PASSPORT_NUMBER = "Passport No";
        private const string FIELD_PASSPORT_ISSUE_COUNTRY = "Passport Issued Country";
        // Start 0000142, KuangWei, 2014-12-20
        private const string FIELD_SEX = "Sex";
        private const string FIELD_MEDICAL_SCHEME_INSURED = "Medical Scheme Insured";
        private const string FIELD_MEDICAL_EFFECTIVE_DATE = "Medical Effective Date";
        private const string FIELD_EXPIRY_DATE = "Expiry Date";
        // End 0000142, KuangWei, 2014-12-20
        
        private DateTime UploadDateTime = AppUtils.ServerDateTime();
        private DBManager tempDB = EUploadEmpSpouse.db;
        private DBManager uploadDB = EEmpSpouse.db;

        public ImportErrorList errors = new ImportErrorList();
        

        public ImportEmpSpouseProcess(DatabaseConnection dbConn, string SessionID): base(dbConn, SessionID)
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

                EUploadEmpSpouse uploadEmpSpouse = new EUploadEmpSpouse();
                EEmpSpouse lastEmpSpouse = null;

                string EmpNo = row[FIELD_EMP_NO].ToString().Trim();
                uploadEmpSpouse.EmpID = Parse.GetEmpID(dbConn, EmpNo, UserID);
                if (uploadEmpSpouse.EmpID < 0)
                    errors.addError(ImportErrorMessage.ERROR_ACCESS_DENIED_EMP_NO, new string[] { EmpNo, rowCount.ToString() });
                uploadEmpSpouse.EmpSpouseSurname = row[FIELD_SURNAME].ToString();
                uploadEmpSpouse.EmpSpouseOtherName = row[FIELD_OTHER_NAME].ToString();
                uploadEmpSpouse.EmpSpouseChineseName = row[FIELD_CHINESE_NAME].ToString();
                try
                {
                    //  support old version template
                    if (rawDataTable.Columns.Contains(FIELD_DATE_OF_BIRTH))
                        uploadEmpSpouse.EmpSpouseDateOfBirth = Import.Parse.toDateTimeObject(row[FIELD_DATE_OF_BIRTH]);
                }
                catch
                {
                    errors.addError(ImportErrorMessage.ERROR_INVALID_FIELD_VALUE, new string[] { FIELD_DATE_OF_BIRTH + "=" + row[FIELD_DATE_OF_BIRTH].ToString(), EmpNo, rowCount.ToString() });
                }
                uploadEmpSpouse.EmpSpouseHKID = row[FIELD_HKID].ToString().Trim();
                uploadEmpSpouse.EmpSpousePassportNo = row[FIELD_PASSPORT_NUMBER].ToString();
                uploadEmpSpouse.EmpSpousePassportIssuedCountry = row[FIELD_PASSPORT_ISSUE_COUNTRY].ToString();

                // Start 0000142, KuangWei, 2014-12-21
                uploadEmpSpouse.EmpGender = row[FIELD_SEX].ToString();

                // Start 000142, Ricky So, 2015-01-08
                //int tempInt = 0;
                //if (int.TryParse(row[FIELD_MEDICAL_SCHEME_INSURED].ToString(), out tempInt))
                //    //uploadEmpSpouse.EmpIsMedicalSchemaInsured = tempInt;
                //    uploadEmpSpouse.EmpIsMedicalSchemaInsured = true;
                //else
                //    uploadEmpSpouse.EmpIsMedicalSchemaInsured = false;
                uploadEmpSpouse.EmpIsMedicalSchemaInsured = Parse.toBoolean(row[FIELD_MEDICAL_SCHEME_INSURED].ToString());

                DateTime m_EffectiveDate = new DateTime();
                DateTime m_ExpiryDate = new DateTime();
                // End 000142, Ricky So, 2015-01-08
                try
                {
                    //  support old version template
                    // Start 000142, Ricky So, 2015-01-08
                    //if (rawDataTable.Columns.Contains(FIELD_MEDICAL_EFFECTIVE_DATE))
                    //    uploadEmpSpouse.EmpMedicalEffectiveDate = Import.Parse.toDateTimeObject(row[FIELD_MEDICAL_EFFECTIVE_DATE]);
                    if (rawDataTable.Columns.Contains(FIELD_MEDICAL_EFFECTIVE_DATE))
                        m_EffectiveDate = Import.Parse.toDateTimeObject(row[FIELD_MEDICAL_EFFECTIVE_DATE]);
                    // End 000142, Ricky So, 2015-01-08
                }
                catch
                {
                    errors.addError(ImportErrorMessage.ERROR_INVALID_FIELD_VALUE, new string[] { FIELD_MEDICAL_EFFECTIVE_DATE + "=" + row[FIELD_MEDICAL_EFFECTIVE_DATE].ToString(), EmpNo, rowCount.ToString() });
                }

                try
                {
                    //  support old version template
                    // Start 000142, Ricky So, 2015-01-08
                    //if (rawDataTable.Columns.Contains(FIELD_EXPIRY_DATE))
                    //    uploadEmpSpouse.EmpMedicalExpiryDate = Import.Parse.toDateTimeObject(row[FIELD_EXPIRY_DATE]);
                    if (rawDataTable.Columns.Contains(FIELD_EXPIRY_DATE))
                        m_ExpiryDate = Import.Parse.toDateTimeObject(row[FIELD_EXPIRY_DATE]);
                    // End 000142, Ricky So, 2015-01-08
                }
                catch
                {
                    errors.addError(ImportErrorMessage.ERROR_INVALID_FIELD_VALUE, new string[] { FIELD_EXPIRY_DATE + "=" + row[FIELD_EXPIRY_DATE].ToString(), EmpNo, rowCount.ToString() });
                }
                // End 0000142, KuangWei, 2014-12-21

                // Start 000142, Ricky So, 2015-01-08
                if (uploadEmpSpouse.EmpIsMedicalSchemaInsured)
                {
                    if (m_EffectiveDate.Ticks == 0)
                        errors.addError(ImportErrorMessage.ERROR_INVALID_FIELD_VALUE, new string[] { FIELD_MEDICAL_EFFECTIVE_DATE + "=" + row[FIELD_MEDICAL_EFFECTIVE_DATE].ToString(), EmpNo, rowCount.ToString() });
                    
                    if (m_ExpiryDate.Ticks == 0)
                        errors.addError(ImportErrorMessage.ERROR_INVALID_FIELD_VALUE, new string[] { FIELD_EXPIRY_DATE + "=" + row[FIELD_EXPIRY_DATE].ToString(), EmpNo, rowCount.ToString() });
                }
                else
                {
                    if (m_EffectiveDate.Ticks > 0)
                        errors.addError(ImportErrorMessage.ERROR_INVALID_FIELD_VALUE, new string[] { FIELD_MEDICAL_EFFECTIVE_DATE + "=" + row[FIELD_MEDICAL_EFFECTIVE_DATE].ToString(), EmpNo, rowCount.ToString() });

                    if (m_ExpiryDate.Ticks > 0)
                        errors.addError(ImportErrorMessage.ERROR_INVALID_FIELD_VALUE, new string[] { FIELD_EXPIRY_DATE + "=" + row[FIELD_EXPIRY_DATE].ToString(), EmpNo, rowCount.ToString() });                    
                }
                uploadEmpSpouse.EmpMedicalEffectiveDate = m_EffectiveDate;
                uploadEmpSpouse.EmpMedicalExpiryDate = m_ExpiryDate;
                // End 000142, Ricky So, 2015-01-08

                uploadEmpSpouse.SessionID = m_SessionID;
                uploadEmpSpouse.TransactionDate = UploadDateTime;

                if (uploadEmpSpouse.EmpID != 0)
                {
                    //if (rawDataTable.Columns.Contains(FIELD_INTERNAL_ID))
                    //{
                    //    try
                    //    {
                    //        if (!row.IsNull(FIELD_INTERNAL_ID))
                    //        {
                    //            int tmpID = FromHexDecWithCheckDigit((string)row[FIELD_INTERNAL_ID]);
                    //            EEmpSpouse tmpObj = new EEmpSpouse();
                    //            tmpObj.EmpSpouseID = tmpID;
                    //            if (EEmpSpouse.db.select(dbConn, tmpObj))
                    //                uploadEmpSpouse.EmpSpouseID = tmpID;
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
                            uploadEmpSpouse.SynID = strSynID;
                            if (!string.IsNullOrEmpty(strSynID))
                            {
                                DBFilter synIDFilter = new DBFilter();
                                synIDFilter.add(new Match("SynID", strSynID));
                                ArrayList objSameSynIDList = EEmpSpouse.db.select(dbConn, synIDFilter);
                                if (objSameSynIDList.Count > 0)
                                    uploadEmpSpouse.EmpSpouseID = ((EEmpSpouse)objSameSynIDList[0]).EmpSpouseID;
                            }
                        }

                    }

                    if (uploadEmpSpouse.EmpSpouseID == 0)
                    {
                        lastEmpSpouse = (EEmpSpouse)AppUtils.GetLastObj(dbConn, uploadDB, "EmpSpouseID", uploadEmpSpouse.EmpID);
                        if (lastEmpSpouse != null)
                        {

                            if (uploadEmpSpouse.EmpSpouseHKID == lastEmpSpouse.EmpSpouseHKID
                                && uploadEmpSpouse.EmpSpouseSurname == lastEmpSpouse.EmpSpouseSurname
                                && uploadEmpSpouse.EmpSpouseOtherName == lastEmpSpouse.EmpSpouseOtherName
                                && uploadEmpSpouse.EmpSpouseChineseName == lastEmpSpouse.EmpSpouseChineseName
                                && uploadEmpSpouse.EmpSpousePassportNo == lastEmpSpouse.EmpSpousePassportNo
                                && uploadEmpSpouse.EmpSpousePassportIssuedCountry == lastEmpSpouse.EmpSpousePassportIssuedCountry
                                && uploadEmpSpouse.EmpSpouseDateOfBirth == lastEmpSpouse.EmpSpouseDateOfBirth
                                // Start 0000142, KuangWei, 2014-12-21
                                && uploadEmpSpouse.EmpGender == lastEmpSpouse.EmpGender
                                && uploadEmpSpouse.EmpIsMedicalSchemaInsured == lastEmpSpouse.EmpIsMedicalSchemaInsured
                                && uploadEmpSpouse.EmpMedicalEffectiveDate == lastEmpSpouse.EmpMedicalEffectiveDate
                                && uploadEmpSpouse.EmpMedicalExpiryDate == lastEmpSpouse.EmpMedicalExpiryDate
                                // End 0000142, KuangWei, 2014-12-21
                                )
                            {
                                continue;
                            }
                            else
                            {
                                uploadEmpSpouse.EmpSpouseID = lastEmpSpouse.EmpSpouseID;
                            }
                        }
                    }
                }

                if (uploadEmpSpouse.EmpSpouseID <= 0)
                    uploadEmpSpouse.ImportActionStatus = ImportDBObject.ImportActionEnum.INSERT;
                else
                    uploadEmpSpouse.ImportActionStatus = ImportDBObject.ImportActionEnum.UPDATE;

                uploadEmpSpouse.UploadEmpID = ParseTemp.GetUploadEmpID(dbConn, EmpNo, m_SessionID);
                if (uploadEmpSpouse.UploadEmpID == 0)
                    if (uploadEmpSpouse.EmpID == 0)
                        errors.addError(ImportErrorMessage.ERROR_INVALID_EMP_NO, new string[] { EmpNo, rowCount.ToString() });
                    else
                        uploadEmpSpouse.UploadEmpID = ImportEmpPersonalInfoProcess.CreateDummyUploadEmployeeInfo(dbConn, uploadEmpSpouse.EmpID, m_SessionID, UploadDateTime);

                Hashtable values = new Hashtable();
                tempDB.populate(uploadEmpSpouse, values);
                PageErrors pageErrors = new PageErrors(EUploadEmpPersonalInfo.db);
                tempDB.validate(pageErrors, values);
                if (pageErrors.errors.Count == 0)
                {

                    tempDB.insert(dbConn, uploadEmpSpouse);
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
            ImportEmpSpouseProcess import = new ImportEmpSpouseProcess(dbConn, sessionID);
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
            ArrayList uploadEmpSpouseList = tempDB.select(dbConn, sessionFilter);
            foreach (EUploadEmpSpouse obj in uploadEmpSpouseList)
            {
                EEmpSpouse empSpouse = new EEmpSpouse();

                if (obj.ImportActionStatus != ImportDBObject.ImportActionEnum.INSERT)
                {
                    empSpouse.EmpSpouseID = obj.EmpSpouseID;
                    uploadDB.select(dbConn, empSpouse);
                }

                obj.ExportToObject(empSpouse);

                if (obj.ImportActionStatus == ImportDBObject.ImportActionEnum.INSERT)
                {
                    empSpouse.EmpID = ParseTemp.GetEmpIDFromUploadEmpID(dbConn, obj.UploadEmpID);
                    uploadDB.insert(dbConn, empSpouse);


                }
                else if (obj.ImportActionStatus == ImportDBObject.ImportActionEnum.UPDATE)
                {
                    uploadDB.update(dbConn, empSpouse);
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

            tmpDataTable.Columns.Add(FIELD_SURNAME, typeof(string));
            tmpDataTable.Columns.Add(FIELD_OTHER_NAME, typeof(string));
            tmpDataTable.Columns.Add(FIELD_CHINESE_NAME, typeof(string));
            tmpDataTable.Columns.Add(FIELD_DATE_OF_BIRTH, typeof(DateTime));
            tmpDataTable.Columns.Add(FIELD_HKID, typeof(string));
            tmpDataTable.Columns.Add(FIELD_PASSPORT_NUMBER, typeof(string));
            tmpDataTable.Columns.Add(FIELD_PASSPORT_ISSUE_COUNTRY, typeof(string));

            // Start 0000142, KuangWei, 2014-12-20
            tmpDataTable.Columns.Add(FIELD_SEX, typeof(string));
            tmpDataTable.Columns.Add(FIELD_MEDICAL_SCHEME_INSURED, typeof(string));
            tmpDataTable.Columns.Add(FIELD_MEDICAL_EFFECTIVE_DATE, typeof(DateTime));
            tmpDataTable.Columns.Add(FIELD_EXPIRY_DATE, typeof(DateTime));
            // End 0000142, KuangWei, 2014-12-20

            if (IsIncludeSyncID)
                tmpDataTable.Columns.Add(FIELD_SYNC_ID, typeof(string));

            foreach (EEmpPersonalInfo empInfo in empList)
            {
                if (EEmpPersonalInfo.db.select(dbConn, empInfo))
                {
                    DBFilter filter = new DBFilter();
                    filter.add(new Match("EmpID", empInfo.EmpID));
                    filter.add(getCreateModifiedRecordsAfterDBTerm(ReferenceDateTime));
                    ArrayList list = EEmpSpouse.db.select(dbConn, filter);
                    foreach (EEmpSpouse empSpouse in list)
                    {
                        DataRow row = tmpDataTable.NewRow();
                        //if (IsIncludeInternalID)
                        //    row[FIELD_INTERNAL_ID] = ToHexDecWithCheckDigit(empSpouse.EmpSpouseID);
                        row[FIELD_EMP_NO] = empInfo.EmpNo;

                        if (IsIncludeCurrentPositionInfo)
                        {
                            ImportEmpPersonalInfoProcess.AddEmployeeInfo(dbConn, row, empInfo.EmpID);
                            ImportEmpPositionInfoProcess.AddEmployeePositionInfo(dbConn, row, empInfo.EmpID);
                        }

                        row[FIELD_SURNAME] = empSpouse.EmpSpouseSurname;
                        row[FIELD_OTHER_NAME] = empSpouse.EmpSpouseOtherName;
                        row[FIELD_CHINESE_NAME] = empSpouse.EmpSpouseChineseName;
                        row[FIELD_DATE_OF_BIRTH] = empSpouse.EmpSpouseDateOfBirth;
                        row[FIELD_HKID] = empSpouse.EmpSpouseHKID;
                        row[FIELD_PASSPORT_NUMBER] = empSpouse.EmpSpousePassportNo;
                        row[FIELD_PASSPORT_ISSUE_COUNTRY] = empSpouse.EmpSpousePassportIssuedCountry;
                        // Start 0000142, KuangWei, 2014-12-20
                        row[FIELD_SEX] = empSpouse.EmpGender;
                        row[FIELD_MEDICAL_SCHEME_INSURED] = (empSpouse.EmpIsMedicalSchemaInsured) ? "Y" : "N";
                        row[FIELD_MEDICAL_EFFECTIVE_DATE] = empSpouse.EmpMedicalEffectiveDate;
                        row[FIELD_EXPIRY_DATE] = empSpouse.EmpMedicalExpiryDate;
                        // End 0000142, KuangWei, 2014-12-20
                        if (IsIncludeSyncID)
                            row[FIELD_SYNC_ID] = empSpouse.SynID;

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
