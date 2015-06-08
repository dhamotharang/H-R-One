using System;
using System.Collections;
using System.Globalization;
using System.Data;
using System.Text;
using HROne.DataAccess;
using HROne.Lib.Entities;

namespace HROne.Import
{
    public class ImportEmpBeneficiariesProcess : ImportProcessInteface
    {

        [DBClass("UploadEmpBeneficiaries")]
        private class EUploadEmpBeneficiaries : ImportDBObject
        {
            public static DBManager db = new DBManager(typeof(EUploadEmpBeneficiaries));

            protected int m_UploadEmpBeneficiariesID;
            [DBField("UploadEmpBeneficiariesID", true, true), TextSearch, Export(false)]
            public int UploadEmpBeneficiariesID
            {
                get { return m_UploadEmpBeneficiariesID; }
                set { m_UploadEmpBeneficiariesID = value; modify("UploadEmpBeneficiariesID"); }
            }
            protected int m_UploadEmpID;
            [DBField("UploadEmpID"), TextSearch, Export(false)]
            public int UploadEmpID
            {
                get { return m_UploadEmpID; }
                set { m_UploadEmpID = value; modify("UploadEmpID"); }
            }

            protected int m_EmpBeneficiariesID;
            [DBField("EmpBeneficiariesID"), TextSearch, Export(false)]
            public int EmpBeneficiariesID
            {
                get { return m_EmpBeneficiariesID; }
                set { m_EmpBeneficiariesID = value; modify("EmpBeneficiariesID"); }
            }
            protected int m_EmpID;
            [DBField("EmpID"), TextSearch, Export(false)]
            public int EmpID
            {
                get { return m_EmpID; }
                set { m_EmpID = value; modify("EmpID"); }
            }

            protected string m_EmpBeneficiariesName;
            [DBField("EmpBeneficiariesName"), TextSearch, MaxLength(200, 50), Export(false), Required]
            public string EmpBeneficiariesName
            {
                get { return m_EmpBeneficiariesName; }
                set { m_EmpBeneficiariesName = value; modify("EmpBeneficiariesName"); }
            }

            protected double m_EmpBeneficiariesShare;
            [DBField("EmpBeneficiariesShare", "0.00"), TextSearch, MaxLength(6), Export(false), Required]
            public double EmpBeneficiariesShare
            {
                get { return m_EmpBeneficiariesShare; }
                set { m_EmpBeneficiariesShare = value; modify("EmpBeneficiariesShare"); }
            }

            protected string m_EmpBeneficiariesHKID;
            [DBField("EmpBeneficiariesHKID"), DBAESEncryptStringField, TextSearch, MaxLength(12, 25), Export(false), Required]
            public string EmpBeneficiariesHKID
            {
                get { return m_EmpBeneficiariesHKID; }
                set { m_EmpBeneficiariesHKID = value; modify("EmpBeneficiariesHKID"); }
            }

            protected string m_EmpBeneficiariesRelation;
            [DBField("EmpBeneficiariesRelation"), TextSearch, MaxLength(250, 50), Export(false), Required]
            public string EmpBeneficiariesRelation
            {
                get { return m_EmpBeneficiariesRelation; }
                set { m_EmpBeneficiariesRelation = value; modify("EmpBeneficiariesRelation"); }
            }

            protected string m_EmpBeneficiariesAddress;
            [DBField("EmpBeneficiariesAddress"), TextSearch, MaxLength(250, 50), Export(false)]
            public string EmpBeneficiariesAddress
            {
                get { return m_EmpBeneficiariesAddress; }
                set { m_EmpBeneficiariesAddress = value; modify("EmpBeneficiariesAddress"); }
            }

            protected string m_EmpBeneficiariesDistrict;
            [DBField("EmpBeneficiariesDistrict"), TextSearch, MaxLength(150, 50), Export(false)]
            public string EmpBeneficiariesDistrict
            {
                get { return m_EmpBeneficiariesDistrict; }
                set { m_EmpBeneficiariesDistrict = value; modify("EmpBeneficiariesDistrict"); }
            }

            protected string m_EmpBeneficiariesArea;
            [DBField("EmpBeneficiariesArea"), TextSearch, MaxLength(100, 50), Export(false)]
            public string EmpBeneficiariesArea
            {
                get { return m_EmpBeneficiariesArea; }
                set { m_EmpBeneficiariesArea = value; modify("EmpBeneficiariesArea"); }
            }

            protected string m_EmpBeneficiariesCountry;
            [DBField("EmpBeneficiariesCountry"), TextSearch, MaxLength(250, 50), Export(false)]
            public string EmpBeneficiariesCountry
            {
                get { return m_EmpBeneficiariesCountry; }
                set { m_EmpBeneficiariesCountry = value; modify("EmpBeneficiariesCountry"); }
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

        public const string TABLE_NAME = "beneficiaries";

        private const string FIELD_EMP_NO = "Emp No";
        private const string FIELD_NAME = "Name";
        private const string FIELD_SHARE = "Share";
        private const string FIELD_HKID = "HKID";
        private const string FIELD_RELATION = "Relation";
        private const string FIELD_ADDRESS = "Address";
        private const string FIELD_DISTRICT = "District";
        private const string FIELD_AREA = "Area";
        private const string FIELD_COUNTRY = "Country";

        
        private DateTime UploadDateTime = AppUtils.ServerDateTime();
        private DBManager tempDB = EUploadEmpBeneficiaries.db;
        private DBManager uploadDB = EEmpBeneficiaries.db;

        public ImportErrorList errors = new ImportErrorList();
        

        public ImportEmpBeneficiariesProcess(DatabaseConnection dbConn, string SessionID): base(dbConn, SessionID)
        {
        }

        public DataTable UploadToTempDatabase(DataTable rawDataTable, int UserID)
        {
            if (rawDataTable == null)
                return GetImportDataFromTempDatabase(null);

            decimal m_share;

            int rowCount = 1;
            foreach (DataRow row in rawDataTable.Rows)
            {
                rowCount++;

                EUploadEmpBeneficiaries uploadEmpBeneficiaries = new EUploadEmpBeneficiaries();

                string EmpNo = row[FIELD_EMP_NO].ToString().Trim();
                uploadEmpBeneficiaries.EmpID = Parse.GetEmpID(dbConn, EmpNo, UserID);
                if (uploadEmpBeneficiaries.EmpID < 0)
                    errors.addError(ImportErrorMessage.ERROR_ACCESS_DENIED_EMP_NO, new string[] { EmpNo, rowCount.ToString() });

                if (rawDataTable.Columns.Contains(FIELD_NAME))
                    uploadEmpBeneficiaries.EmpBeneficiariesName = row[FIELD_NAME].ToString().Trim();

                if (decimal.TryParse(row[FIELD_SHARE].ToString(), out m_share))
                {
                    uploadEmpBeneficiaries.EmpBeneficiariesShare = System.Convert.ToDouble(m_share);
                }
                else
                {
                    errors.addError(ImportErrorMessage.ERROR_INVALID_FIELD_VALUE, new string[] { FIELD_SHARE + "=" + row[FIELD_SHARE].ToString(), EmpNo, rowCount.ToString() });
                }

                if (rawDataTable.Columns.Contains(FIELD_HKID))
                    uploadEmpBeneficiaries.EmpBeneficiariesHKID = row[FIELD_HKID].ToString().Trim();
                if (rawDataTable.Columns.Contains(FIELD_RELATION))
                    uploadEmpBeneficiaries.EmpBeneficiariesRelation = row[FIELD_RELATION].ToString().Trim();
                if (rawDataTable.Columns.Contains(FIELD_ADDRESS))
                    uploadEmpBeneficiaries.EmpBeneficiariesAddress = row[FIELD_ADDRESS].ToString().Trim();
                if (rawDataTable.Columns.Contains(FIELD_DISTRICT))
                    uploadEmpBeneficiaries.EmpBeneficiariesDistrict = row[FIELD_DISTRICT].ToString().Trim();
                if (rawDataTable.Columns.Contains(FIELD_AREA))
                    uploadEmpBeneficiaries.EmpBeneficiariesArea = row[FIELD_AREA].ToString().Trim();
                if (rawDataTable.Columns.Contains(FIELD_COUNTRY))
                    uploadEmpBeneficiaries.EmpBeneficiariesCountry = row[FIELD_COUNTRY].ToString().Trim();

                uploadEmpBeneficiaries.SessionID = m_SessionID;
                uploadEmpBeneficiaries.TransactionDate = UploadDateTime;


                if (uploadEmpBeneficiaries.EmpID != 0)
                {
                    if (rawDataTable.Columns.Contains(FIELD_SYNC_ID))
                    {
                        if (!row.IsNull(FIELD_SYNC_ID))
                        {
                            string strSynID = row[FIELD_SYNC_ID].ToString();
                            uploadEmpBeneficiaries.SynID = strSynID;
                            if (!string.IsNullOrEmpty(strSynID))
                            {
                                DBFilter synIDFilter = new DBFilter();
                                synIDFilter.add(new Match("SynID", strSynID));
                                ArrayList objSameSynIDList = EEmpBeneficiaries.db.select(dbConn, synIDFilter);
                                if (objSameSynIDList.Count > 0)
                                    uploadEmpBeneficiaries.EmpBeneficiariesID = ((EEmpBeneficiaries)objSameSynIDList[0]).EmpBeneficiariesID;
                            }
                        }

                    }

                    if (uploadEmpBeneficiaries.EmpBeneficiariesID == 0)
                    {

                        DBFilter filter = new DBFilter();
                        filter.add(new Match("EmpID", uploadEmpBeneficiaries.EmpID));
                        ArrayList list = uploadDB.select(dbConn, filter);                        
                        bool IsSameEntryExists = false;
                        foreach (EEmpBeneficiaries currentEmpBeneficiaries in list)
                        {
                            if (uploadEmpBeneficiaries.EmpBeneficiariesHKID == currentEmpBeneficiaries.EmpBeneficiariesHKID)
                                uploadEmpBeneficiaries.EmpBeneficiariesID = currentEmpBeneficiaries.EmpBeneficiariesID;

                            if (uploadEmpBeneficiaries.EmpBeneficiariesName == currentEmpBeneficiaries.EmpBeneficiariesName
                                && uploadEmpBeneficiaries.EmpBeneficiariesShare == currentEmpBeneficiaries.EmpBeneficiariesShare
                                && uploadEmpBeneficiaries.EmpBeneficiariesHKID == currentEmpBeneficiaries.EmpBeneficiariesHKID
                                && uploadEmpBeneficiaries.EmpBeneficiariesRelation == currentEmpBeneficiaries.EmpBeneficiariesRelation
                                && uploadEmpBeneficiaries.EmpBeneficiariesAddress == currentEmpBeneficiaries.EmpBeneficiariesAddress
                                && uploadEmpBeneficiaries.EmpBeneficiariesDistrict == currentEmpBeneficiaries.EmpBeneficiariesDistrict
                                && uploadEmpBeneficiaries.EmpBeneficiariesArea == currentEmpBeneficiaries.EmpBeneficiariesArea
                                && uploadEmpBeneficiaries.EmpBeneficiariesCountry == currentEmpBeneficiaries.EmpBeneficiariesCountry
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

                if (uploadEmpBeneficiaries.EmpBeneficiariesID <= 0)
                    uploadEmpBeneficiaries.ImportActionStatus = ImportDBObject.ImportActionEnum.INSERT;
                else
                    uploadEmpBeneficiaries.ImportActionStatus = ImportDBObject.ImportActionEnum.UPDATE;

                uploadEmpBeneficiaries.UploadEmpID = ParseTemp.GetUploadEmpID(dbConn, EmpNo, m_SessionID);
                if (uploadEmpBeneficiaries.UploadEmpID == 0)
                    if (uploadEmpBeneficiaries.EmpID == 0)
                        errors.addError(ImportErrorMessage.ERROR_INVALID_EMP_NO, new string[] { EmpNo, rowCount.ToString() });
                    else
                        uploadEmpBeneficiaries.UploadEmpID = ImportEmpPersonalInfoProcess.CreateDummyUploadEmployeeInfo(dbConn, uploadEmpBeneficiaries.EmpID, m_SessionID, UploadDateTime);

                Hashtable values = new Hashtable();
                tempDB.populate(uploadEmpBeneficiaries, values);
                PageErrors pageErrors = new PageErrors(EUploadEmpPersonalInfo.db);
                tempDB.validate(pageErrors, values);
                if (pageErrors.errors.Count == 0)
                {
                    tempDB.insert(dbConn, uploadEmpBeneficiaries);
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
            ImportEmpBeneficiariesProcess import = new ImportEmpBeneficiariesProcess(dbConn, sessionID);
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
            ArrayList uploadEmpBeneficiariesList = tempDB.select(dbConn, sessionFilter);
            foreach (EUploadEmpBeneficiaries obj in uploadEmpBeneficiariesList)
            {
                EEmpBeneficiaries EmpBeneficiaries = new EEmpBeneficiaries();

                if (obj.ImportActionStatus != ImportDBObject.ImportActionEnum.INSERT)
                {
                    EmpBeneficiaries.EmpBeneficiariesID = obj.EmpBeneficiariesID;
                    uploadDB.select(dbConn, EmpBeneficiaries);
                }

                obj.ExportToObject(EmpBeneficiaries);

                if (obj.ImportActionStatus == ImportDBObject.ImportActionEnum.INSERT)
                {
                    EmpBeneficiaries.EmpID = ParseTemp.GetEmpIDFromUploadEmpID(dbConn, obj.UploadEmpID);
                    uploadDB.insert(dbConn, EmpBeneficiaries);
                }
                else if (obj.ImportActionStatus == ImportDBObject.ImportActionEnum.UPDATE)
                {
                    uploadDB.update(dbConn, EmpBeneficiaries);
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
            tmpDataTable.Columns.Add(FIELD_EMP_NO, typeof(string));

            if (IsIncludeCurrentPositionInfo)
            {
                ImportEmpPersonalInfoProcess.AddEmployeeInfoHeader(tmpDataTable);
                ImportEmpPositionInfoProcess.AddEmployeePositionInfoHeader(dbConn, tmpDataTable);
            }

            tmpDataTable.Columns.Add(FIELD_NAME, typeof(string));
            tmpDataTable.Columns.Add(FIELD_SHARE, typeof(decimal));
            tmpDataTable.Columns.Add(FIELD_HKID, typeof(string));
            tmpDataTable.Columns.Add(FIELD_RELATION, typeof(string));
            tmpDataTable.Columns.Add(FIELD_ADDRESS, typeof(string));
            tmpDataTable.Columns.Add(FIELD_DISTRICT, typeof(string));
            tmpDataTable.Columns.Add(FIELD_AREA, typeof(string));
            tmpDataTable.Columns.Add(FIELD_COUNTRY, typeof(string));
            if (IsIncludeSyncID)
                tmpDataTable.Columns.Add(FIELD_SYNC_ID, typeof(string));

            foreach (EEmpPersonalInfo empInfo in empList)
            {
                if (EEmpPersonalInfo.db.select(dbConn, empInfo))
                {
                    DBFilter filter = new DBFilter();
                    filter.add(new Match("EmpID", empInfo.EmpID));
                    filter.add(getCreateModifiedRecordsAfterDBTerm(ReferenceDateTime));
                    ArrayList list = EEmpBeneficiaries.db.select(dbConn, filter);
                    foreach (EEmpBeneficiaries empBeneficiaries in list)
                    {
                        DataRow row = tmpDataTable.NewRow();
                        row[FIELD_EMP_NO] = empInfo.EmpNo;

                        if (IsIncludeCurrentPositionInfo)
                        {
                            ImportEmpPersonalInfoProcess.AddEmployeeInfo(dbConn, row, empInfo.EmpID);
                            ImportEmpPositionInfoProcess.AddEmployeePositionInfo(dbConn, row, empInfo.EmpID);
                        }

                        row[FIELD_NAME] = empBeneficiaries.EmpBeneficiariesName;
                        row[FIELD_SHARE] = empBeneficiaries.EmpBeneficiariesShare;
                        row[FIELD_HKID] = empBeneficiaries.EmpBeneficiariesHKID;
                        row[FIELD_RELATION] = empBeneficiaries.EmpBeneficiariesRelation;
                        row[FIELD_ADDRESS] = empBeneficiaries.EmpBeneficiariesAddress;
                        row[FIELD_DISTRICT] = empBeneficiaries.EmpBeneficiariesDistrict;
                        row[FIELD_AREA] = empBeneficiaries.EmpBeneficiariesArea;
                        row[FIELD_COUNTRY] = empBeneficiaries.EmpBeneficiariesCountry;

                        if (IsIncludeSyncID)
                            row[FIELD_SYNC_ID] = empBeneficiaries.SynID;

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
