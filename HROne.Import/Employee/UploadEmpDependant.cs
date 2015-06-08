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
    public class ImportEmpDependantProcess : ImportProcessInteface
    {

        [DBClass("UploadEmpDependant")]
        private class EUploadEmpDependant : ImportDBObject
        {
            public static DBManager db = new DBManager(typeof(EUploadEmpDependant));

            protected int m_UploadEmpDependantID;
            [DBField("UploadEmpDependantID", true, true), TextSearch, Export(false)]
            public int UploadEmpDependantID
            {
                get { return m_UploadEmpDependantID; }
                set { m_UploadEmpDependantID = value; modify("UploadEmpDependantID"); }
            }
            protected int m_UploadEmpID;
            [DBField("UploadEmpID"), TextSearch, Export(false)]
            public int UploadEmpID
            {
                get { return m_UploadEmpID; }
                set { m_UploadEmpID = value; modify("UploadEmpID"); }
            }

            protected int m_EmpDependantID;
            [DBField("EmpDependantID"), TextSearch, Export(false)]
            public int EmpDependantID
            {
                get { return m_EmpDependantID; }
                set { m_EmpDependantID = value; modify("EmpDependantID"); }
            }
            protected int m_EmpID;
            [DBField("EmpID"), TextSearch, Export(false)]
            public int EmpID
            {
                get { return m_EmpID; }
                set { m_EmpID = value; modify("EmpID"); }
            }
            protected string m_EmpDependantSurname;
            [DBField("EmpDependantSurname"), TextSearch, MaxLength(20, 25), Export(false), Required]
            public string EmpDependantSurname
            {
                get { return m_EmpDependantSurname; }
                set { m_EmpDependantSurname = value; modify("EmpDependantSurname"); }
            }
            protected string m_EmpDependantOtherName;
            [DBField("EmpDependantOtherName"), TextSearch, MaxLength(40, 25), Export(false), Required]
            public string EmpDependantOtherName
            {
                get { return m_EmpDependantOtherName; }
                set { m_EmpDependantOtherName = value; modify("EmpDependantOtherName"); }
            }
            protected string m_EmpDependantChineseName;
            [DBField("EmpDependantChineseName"), TextSearch, MaxLength(50, 25), Export(false)]
            public string EmpDependantChineseName
            {
                get { return m_EmpDependantChineseName; }
                set { m_EmpDependantChineseName = value; modify("EmpDependantChineseName"); }
            }
            protected string m_EmpDependantGender;
            [DBField("EmpDependantGender"), TextSearch, Export(false)]
            public string EmpDependantGender
            {
                get { return m_EmpDependantGender; }
                set { m_EmpDependantGender = value; modify("EmpDependantGender"); }
            }
            protected DateTime m_EmpDependantDateOfBirth;
            [DBField("EmpDependantDateOfBirth"), TextSearch, MaxLength(10, 10), Export(false)]
            public DateTime EmpDependantDateOfBirth
            {
                get { return m_EmpDependantDateOfBirth; }
                set { m_EmpDependantDateOfBirth = value; modify("EmpDependantDateOfBirth"); }
            }
            protected string m_EmpDependantHKID;
            [DBField("EmpDependantHKID"), TextSearch, MaxLength(12, 25), Export(false)]
            public string EmpDependantHKID
            {
                get { return m_EmpDependantHKID; }
                set { m_EmpDependantHKID = value; modify("EmpDependantHKID"); }
            }
            protected string m_EmpDependantPassportNo;
            [DBField("EmpDependantPassportNo"), TextSearch, MaxLength(40, 25), Export(false)]
            public string EmpDependantPassportNo
            {
                get { return m_EmpDependantPassportNo; }
                set { m_EmpDependantPassportNo = value; modify("EmpDependantPassportNo"); }
            }
            protected string m_EmpDependantPassportIssuedCountry;
            [DBField("EmpDependantPassportIssuedCountry"), TextSearch, MaxLength(40, 25), Export(false)]
            public string EmpDependantPassportIssuedCountry
            {
                get { return m_EmpDependantPassportIssuedCountry; }
                set { m_EmpDependantPassportIssuedCountry = value; modify("EmpDependantPassportIssuedCountry"); }
            }
            protected string m_EmpDependantRelationship;
            [DBField("EmpDependantRelationship"), TextSearch, MaxLength(100, 25), Export(false), Required]
            public string EmpDependantRelationship
            {
                get { return m_EmpDependantRelationship; }
                set { m_EmpDependantRelationship = value; modify("EmpDependantRelationship"); }
            }
            //Start 0000190, Miranda, 2015-05-18
            protected bool m_EmpDependantMedicalSchemeInsured;
            [DBField("EmpDependantMedicalSchemeInsured"), TextSearch, Export(false), Required]
            public bool EmpDependantMedicalSchemeInsured
            {
                get { return m_EmpDependantMedicalSchemeInsured; }
                set { m_EmpDependantMedicalSchemeInsured = value; modify("EmpDependantMedicalSchemeInsured"); }
            }
            protected DateTime m_EmpDependantMedicalEffectiveDate;
            [DBField("EmpDependantMedicalEffectiveDate"), TextSearch, MaxLength(10, 10), Export(false)]
            public DateTime EmpDependantMedicalEffectiveDate
            {
                get { return m_EmpDependantMedicalEffectiveDate; }
                set { m_EmpDependantMedicalEffectiveDate = value; modify("EmpDependantMedicalEffectiveDate"); }
            }
            protected DateTime m_EmpDependantExpiryDate;
            [DBField("EmpDependantExpiryDate"), TextSearch, MaxLength(10, 10), Export(false)]
            public DateTime EmpDependantExpiryDate
            {
                get { return m_EmpDependantExpiryDate; }
                set { m_EmpDependantExpiryDate = value; modify("EmpDependantExpiryDate"); }
            }
            //End 0000190, Miranda, 2015-05-18

            //  For Synchronize Use only
            protected string m_SynID;
            [DBField("SynID"), TextSearch, Export(false)]
            public string SynID
            {
                get { return m_SynID; }
                set { m_SynID = value; modify("SynID"); }
            }
        }

        public const string TABLE_NAME = "dependant";

        //  Field Const for Employee Personal Information
        private const string FIELD_EMP_NO = "Emp No";
        private const string FIELD_SURNAME = "Surname";
        private const string FIELD_OTHER_NAME = "OtherName";
        private const string FIELD_CHINESE_NAME = "Chinese Name";
        private const string FIELD_DATE_OF_BIRTH = "Date of Birth";
        private const string FIELD_HKID = "HKID";
        private const string FIELD_GENDER = "Gender";
        private const string FIELD_RELATIONSHIP = "Relationship";
        private const string FIELD_PASSPORT_NUMBER = "Passport No";
        private const string FIELD_PASSPORT_ISSUE_COUNTRY = "Passport Issued Country";
        //Start 0000190, Miranda, 2015-05-18
        private const string FIELD_MEDICAL_SCHEME_INSURED = "Medical Scheme Insured";
        private const string FIELD_MEDICAL_EFFECTIVE_DATE = "Medical Effective Date";
        private const string FIELD_EXPIRY_DATE = "Expiry Date";
        //End 0000190, Miranda, 2015-05-18
        //private const string FIELD_SYNID = "SynID";

        
        private DateTime UploadDateTime = AppUtils.ServerDateTime();
        private DBManager tempDB = EUploadEmpDependant.db;
        private DBManager uploadDB = EEmpDependant.db;

        public ImportErrorList errors = new ImportErrorList();
        

        public ImportEmpDependantProcess(DatabaseConnection dbConn, string SessionID): base(dbConn, SessionID)
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

                EUploadEmpDependant uploadEmpDependant = new EUploadEmpDependant();
                //EEmpDependant lastEmpDependant = null;

                string EmpNo = row[FIELD_EMP_NO].ToString().Trim();
                uploadEmpDependant.EmpID = Parse.GetEmpID(dbConn, EmpNo, UserID);
                if (uploadEmpDependant.EmpID < 0)
                    errors.addError(ImportErrorMessage.ERROR_ACCESS_DENIED_EMP_NO, new string[] { EmpNo, rowCount.ToString() });
                uploadEmpDependant.EmpDependantSurname = row[FIELD_SURNAME].ToString();
                uploadEmpDependant.EmpDependantOtherName = row[FIELD_OTHER_NAME].ToString();
                uploadEmpDependant.EmpDependantChineseName = row[FIELD_CHINESE_NAME].ToString();
                try
                {
                    //  support old version template
                    if (rawDataTable.Columns.Contains(FIELD_DATE_OF_BIRTH))
                        uploadEmpDependant.EmpDependantDateOfBirth = Import.Parse.toDateTimeObject(row[FIELD_DATE_OF_BIRTH]);
                }
                catch
                {
                    errors.addError(ImportErrorMessage.ERROR_INVALID_FIELD_VALUE, new string[] { FIELD_DATE_OF_BIRTH + "=" + row[FIELD_DATE_OF_BIRTH].ToString(), EmpNo, rowCount.ToString() });
                }
                uploadEmpDependant.EmpDependantHKID = row[FIELD_HKID].ToString().Trim();

                string tempString = row[FIELD_GENDER].ToString();
                if (!String.IsNullOrEmpty(tempString))
                    if (tempString.Equals("Male", StringComparison.CurrentCultureIgnoreCase)
                        || tempString.Equals("M", StringComparison.CurrentCultureIgnoreCase))
                        uploadEmpDependant.EmpDependantGender = "M";
                    else if (tempString.Equals("Female", StringComparison.CurrentCultureIgnoreCase)
                        || tempString.Equals("F", StringComparison.CurrentCultureIgnoreCase))
                        uploadEmpDependant.EmpDependantGender = "F";
                    else
                        uploadEmpDependant.EmpDependantGender = string.Empty;
                //else
                    //    errors.addError(ImportErrorMessage.ERROR_INVALID_GENDER, new string[] { tempString, rowCount.ToString() });

                uploadEmpDependant.EmpDependantRelationship = row[FIELD_RELATIONSHIP].ToString();

                //Start 0000190, Miranda, 2015-05-18
                uploadEmpDependant.EmpDependantMedicalSchemeInsured = Parse.toBoolean(row[FIELD_MEDICAL_SCHEME_INSURED].ToString());

                DateTime m_EffectiveDate = new DateTime();
                DateTime m_ExpiryDate = new DateTime();
                try
                {
                    if (rawDataTable.Columns.Contains(FIELD_MEDICAL_EFFECTIVE_DATE))
                        m_EffectiveDate = Import.Parse.toDateTimeObject(row[FIELD_MEDICAL_EFFECTIVE_DATE]);
                }
                catch
                {
                    errors.addError(ImportErrorMessage.ERROR_INVALID_FIELD_VALUE, new string[] { FIELD_MEDICAL_EFFECTIVE_DATE + "=" + row[FIELD_MEDICAL_EFFECTIVE_DATE].ToString(), EmpNo, rowCount.ToString() });
                }

                try
                {
                    if (rawDataTable.Columns.Contains(FIELD_EXPIRY_DATE))
                        m_ExpiryDate = Import.Parse.toDateTimeObject(row[FIELD_EXPIRY_DATE]);
                }
                catch
                {
                    errors.addError(ImportErrorMessage.ERROR_INVALID_FIELD_VALUE, new string[] { FIELD_EXPIRY_DATE + "=" + row[FIELD_EXPIRY_DATE].ToString(), EmpNo, rowCount.ToString() });
                }

                if (uploadEmpDependant.EmpDependantMedicalSchemeInsured)
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
                uploadEmpDependant.EmpDependantMedicalEffectiveDate = m_EffectiveDate;
                uploadEmpDependant.EmpDependantExpiryDate = m_ExpiryDate;
                //End 0000190, Miranda, 2015-05-18

                uploadEmpDependant.EmpDependantPassportNo = row[FIELD_PASSPORT_NUMBER].ToString();
                uploadEmpDependant.EmpDependantPassportIssuedCountry = row[FIELD_PASSPORT_ISSUE_COUNTRY].ToString();

                //if (rawDataTable.Columns.Contains(FIELD_SYNID))
                //{
                //    uploadEmpDependant.SynID = row[FIELD_SYNID].ToString();
                //}
                uploadEmpDependant.SessionID = m_SessionID;
                uploadEmpDependant.TransactionDate = UploadDateTime;


                if (uploadEmpDependant.EmpID != 0)
                {
                    //if (rawDataTable.Columns.Contains(FIELD_INTERNAL_ID))
                    //{
                    //    try
                    //    {
                    //        if (!row.IsNull(FIELD_INTERNAL_ID))
                    //        {
                    //            int tmpID = FromHexDecWithCheckDigit((string)row[FIELD_INTERNAL_ID]);
                    //            EEmpDependant tmpObj = new EEmpDependant();
                    //            tmpObj.EmpDependantID = tmpID;
                    //            if (EEmpDependant.db.select(dbConn, tmpObj))
                    //                uploadEmpDependant.EmpDependantID = tmpID;
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
                            uploadEmpDependant.SynID = strSynID;
                            if (!string.IsNullOrEmpty(strSynID))
                            {
                                DBFilter synIDFilter = new DBFilter();
                                synIDFilter.add(new Match("SynID", strSynID));
                                ArrayList objSameSynIDList = EEmpDependant.db.select(dbConn, synIDFilter);
                                if (objSameSynIDList.Count > 0)
                                    uploadEmpDependant.EmpDependantID = ((EEmpDependant)objSameSynIDList[0]).EmpDependantID;
                            }
                        }

                    }

                    if (uploadEmpDependant.EmpDependantID == 0)
                    {
                        DBFilter filter = new DBFilter();
                        filter.add(new Match("EmpID", uploadEmpDependant.EmpID));
                        ArrayList list = uploadDB.select(dbConn, filter);
                        bool IsSameEntryExists = false;
                        foreach (EEmpDependant currentEmpDependant in list)
                        {

                            if (uploadEmpDependant.EmpDependantHKID == currentEmpDependant.EmpDependantHKID
                                && uploadEmpDependant.EmpDependantSurname == currentEmpDependant.EmpDependantSurname
                                && uploadEmpDependant.EmpDependantOtherName == currentEmpDependant.EmpDependantOtherName
                                && uploadEmpDependant.EmpDependantChineseName == currentEmpDependant.EmpDependantChineseName
                                && uploadEmpDependant.EmpDependantRelationship == currentEmpDependant.EmpDependantRelationship
                                && uploadEmpDependant.EmpDependantPassportNo == currentEmpDependant.EmpDependantPassportNo
                                && uploadEmpDependant.EmpDependantPassportIssuedCountry == currentEmpDependant.EmpDependantPassportIssuedCountry
                                && uploadEmpDependant.EmpDependantDateOfBirth == currentEmpDependant.EmpDependantDateOfBirth
                                //Start 0000190, Miranda, 2015-05-18
                                && uploadEmpDependant.EmpDependantMedicalSchemeInsured == currentEmpDependant.EmpDependantMedicalSchemeInsured
                                && uploadEmpDependant.EmpDependantMedicalEffectiveDate == currentEmpDependant.EmpDependantMedicalEffectiveDate
                                && uploadEmpDependant.EmpDependantExpiryDate == currentEmpDependant.EmpDependantExpiryDate
                                //End 0000190, Miranda, 2015-05-18

                                )
                            {
                                IsSameEntryExists = true;
                            }
                            //if (!string.IsNullOrEmpty(uploadEmpDependant.SynID) && !string.IsNullOrEmpty(currentEmpDependant.SynID))
                            //    if (currentEmpDependant.SynID.Equals(uploadEmpDependant.SynID, StringComparison.CurrentCultureIgnoreCase))
                            //    {
                            //        uploadEmpDependant.EmpDependantID = currentEmpDependant.EmpDependantID;
                            //        IsSameEntryExists = false;
                            //        break;
                            //    }
                        }
                        if (IsSameEntryExists)
                            continue;
                    }
                }

                if (uploadEmpDependant.EmpDependantID <= 0)
                    uploadEmpDependant.ImportActionStatus = ImportDBObject.ImportActionEnum.INSERT;
                else
                    uploadEmpDependant.ImportActionStatus = ImportDBObject.ImportActionEnum.UPDATE;

                uploadEmpDependant.UploadEmpID = ParseTemp.GetUploadEmpID(dbConn, EmpNo, m_SessionID);
                if (uploadEmpDependant.UploadEmpID == 0)
                    if (uploadEmpDependant.EmpID == 0)
                        errors.addError(ImportErrorMessage.ERROR_INVALID_EMP_NO, new string[] { EmpNo, rowCount.ToString() });
                    else
                        uploadEmpDependant.UploadEmpID = ImportEmpPersonalInfoProcess.CreateDummyUploadEmployeeInfo(dbConn, uploadEmpDependant.EmpID, m_SessionID, UploadDateTime);

                Hashtable values = new Hashtable();
                tempDB.populate(uploadEmpDependant, values);
                PageErrors pageErrors = new PageErrors(EUploadEmpPersonalInfo.db);
                tempDB.validate(pageErrors, values);
                if (pageErrors.errors.Count == 0)
                {

                    tempDB.insert(dbConn, uploadEmpDependant);
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
            ImportEmpDependantProcess import = new ImportEmpDependantProcess(dbConn, sessionID);
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
            ArrayList uploadEmpDependantList = tempDB.select(dbConn, sessionFilter);
            foreach (EUploadEmpDependant obj in uploadEmpDependantList)
            {
                EEmpDependant EmpDependant = new EEmpDependant();

                if (obj.ImportActionStatus != ImportDBObject.ImportActionEnum.INSERT)
                {
                    EmpDependant.EmpDependantID = obj.EmpDependantID;
                    uploadDB.select(dbConn, EmpDependant);
                }

                obj.ExportToObject(EmpDependant);

                if (obj.ImportActionStatus == ImportDBObject.ImportActionEnum.INSERT)
                {
                    EmpDependant.EmpID = ParseTemp.GetEmpIDFromUploadEmpID(dbConn, obj.UploadEmpID);
                    uploadDB.insert(dbConn, EmpDependant);


                }
                else if (obj.ImportActionStatus == ImportDBObject.ImportActionEnum.UPDATE)
                {
                    uploadDB.update(dbConn, EmpDependant);
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
            tmpDataTable.Columns.Add(FIELD_GENDER, typeof(string));
            tmpDataTable.Columns.Add(FIELD_RELATIONSHIP, typeof(string));
            tmpDataTable.Columns.Add(FIELD_PASSPORT_NUMBER, typeof(string));
            tmpDataTable.Columns.Add(FIELD_PASSPORT_ISSUE_COUNTRY, typeof(string));
            //Start 0000190, Miranda, 2015-05-18
            tmpDataTable.Columns.Add(FIELD_MEDICAL_SCHEME_INSURED, typeof(string));
            tmpDataTable.Columns.Add(FIELD_MEDICAL_EFFECTIVE_DATE, typeof(DateTime));
            tmpDataTable.Columns.Add(FIELD_EXPIRY_DATE, typeof(DateTime));
            //End 0000190, Miranda, 2015-05-18

            if (IsIncludeSyncID)
                tmpDataTable.Columns.Add(FIELD_SYNC_ID, typeof(string));

            foreach (EEmpPersonalInfo empInfo in empList)
            {
                if (EEmpPersonalInfo.db.select(dbConn, empInfo))
                {
                    DBFilter filter = new DBFilter();
                    filter.add(new Match("EmpID", empInfo.EmpID));
                    filter.add(getCreateModifiedRecordsAfterDBTerm(ReferenceDateTime));
                    ArrayList list = EEmpDependant.db.select(dbConn, filter);
                    foreach (EEmpDependant empDependant in list)
                    {
                        DataRow row = tmpDataTable.NewRow();
                        //if (IsIncludeInternalID)
                        //    row[FIELD_INTERNAL_ID] = ToHexDecWithCheckDigit(empDependant.EmpDependantID);
                        row[FIELD_EMP_NO] = empInfo.EmpNo;

                        if (IsIncludeCurrentPositionInfo)
                        {
                            ImportEmpPersonalInfoProcess.AddEmployeeInfo(dbConn, row, empInfo.EmpID);
                            ImportEmpPositionInfoProcess.AddEmployeePositionInfo(dbConn, row, empInfo.EmpID);
                        }

                        row[FIELD_SURNAME] = empDependant.EmpDependantSurname;
                        row[FIELD_OTHER_NAME] = empDependant.EmpDependantOtherName;
                        row[FIELD_CHINESE_NAME] = empDependant.EmpDependantChineseName;
                        row[FIELD_DATE_OF_BIRTH] = empDependant.EmpDependantDateOfBirth;
                        row[FIELD_HKID] = empDependant.EmpDependantHKID;
                        row[FIELD_GENDER] = empDependant.EmpDependantGender;
                        row[FIELD_RELATIONSHIP] = empDependant.EmpDependantRelationship;
                        row[FIELD_PASSPORT_NUMBER] = empDependant.EmpDependantPassportNo;
                        row[FIELD_PASSPORT_ISSUE_COUNTRY] = empDependant.EmpDependantPassportIssuedCountry;
                        // Start 0000190, Miranda, 2015-05-18
                        row[FIELD_MEDICAL_SCHEME_INSURED] = (empDependant.EmpDependantMedicalSchemeInsured) ? "Y" : "N";
                        row[FIELD_MEDICAL_EFFECTIVE_DATE] = empDependant.EmpDependantMedicalEffectiveDate;
                        row[FIELD_EXPIRY_DATE] = empDependant.EmpDependantExpiryDate;
                        // End 0000190, Miranda, 2014-05-18
                        if (IsIncludeSyncID)
                            row[FIELD_SYNC_ID] = empDependant.SynID;

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
