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
    public class ImportEmpCostCenterExportProcess : ImportProcessInteface
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

        public const string TABLE_NAME = "costCenter_export";
        //  Field Const for Employee Personal Information
        private const string FIELD_EMP_NO = "Emp No";
        private const string FIELD_STAFF_NAME = "Staff Name";
        private const string FIELD_JOB_TITLE = "Job Title";
        private const string FIELD_FROM = "From";
        private const string FIELD_TO = "To";
        private const string _FIELD_COST_CENTER = "Cost Center";
        private const string _FIELD_PERCENTAGE = "%";
        private const string FIELD_COST_CENTER = "Cost Center ";
        private const string FIELD_PERCENTAGE = "Percentage ";

        
        private DateTime UploadDateTime = AppUtils.ServerDateTime();
        private DBManager tempDB = EUploadEmpCostCenter.db;
        private DBManager uploadDB = EEmpCostCenter.db;
        private DBManager tempDetailDB = EUploadEmpCostCenterDetail.db;
        private DBManager uploadDetailDB = EEmpCostCenterDetail.db;

        public ImportErrorList errors = new ImportErrorList();
        protected ImportEmpCostCenterProcess importEmpCostCenter;

        public ImportEmpCostCenterExportProcess(DatabaseConnection dbConn, string SessionID): base(dbConn, SessionID)
        {

            importEmpCostCenter = new ImportEmpCostCenterProcess(dbConn, SessionID);
        }


        public DataTable UploadToTempDatabase(DataTable rawDataTable, int UserID, bool CreateCodeIfNotExists)
        {
            if (rawDataTable == null)
                return GetImportDataFromTempDatabase(null);

            int rowCount = 1;
            int costCenterFieldCounter = 0;
            string lastCostCenter = "";
            foreach (DataRow row in rawDataTable.Rows)
            {
                EUploadEmpCostCenter uploadEmpCostCenter = new EUploadEmpCostCenter();
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

                if (row.Table.Columns.Contains(_FIELD_COST_CENTER) && row.Table.Columns.Contains(_FIELD_PERCENTAGE))
                {
                    if (!string.IsNullOrEmpty(row[_FIELD_COST_CENTER].ToString().Trim()) && !string.IsNullOrEmpty(row[_FIELD_PERCENTAGE].ToString().Trim()))
                    {
                        if (!row[_FIELD_COST_CENTER].ToString().Trim().Equals(lastCostCenter))
                        {
                            costCenterFieldCounter++;
                            lastCostCenter = row[_FIELD_COST_CENTER].ToString().Trim();
                        }
                    }
                }
            }
            DataSet dataSet = new DataSet();
            DataTable tmpDataTable = dataSet.Tables.Add(TABLE_NAME);
            tmpDataTable.Columns.Add(FIELD_EMP_NO, typeof(string));
            tmpDataTable.Columns.Add(FIELD_FROM, typeof(DateTime));
            tmpDataTable.Columns.Add(FIELD_TO, typeof(DateTime));
            for (int i = 1; i <= costCenterFieldCounter; i++ )
            {
                tmpDataTable.Columns.Add(FIELD_COST_CENTER + i, typeof(string));
                tmpDataTable.Columns.Add(FIELD_PERCENTAGE + i, typeof(double));
            }
            
            ArrayList mRows = new ArrayList();
            foreach (DataRow row in rawDataTable.Rows)
            {
                if (!mRows.Contains(row))
                {
                    int iFieldCount = 1;
                    DataRow newRow = tmpDataTable.NewRow();
                    newRow[FIELD_EMP_NO] = row[FIELD_EMP_NO].ToString().Trim();
                    newRow[FIELD_FROM] = Parse.toDateTimeObject(row[FIELD_FROM]);
                    newRow[FIELD_TO] = Parse.toDateTimeObject(row[FIELD_TO]);
                    newRow[FIELD_COST_CENTER + iFieldCount] = row[_FIELD_COST_CENTER].ToString().Trim();
                    newRow[FIELD_PERCENTAGE + iFieldCount] = Parse.toDecimal(row[_FIELD_PERCENTAGE]);
                    mRows.Add(row);
                    foreach (DataRow nextRow in rawDataTable.Rows)
                    {
                        if (!mRows.Contains(nextRow))
                        {
                            string nextEmpNo = nextRow[FIELD_EMP_NO].ToString().Trim();
                            DateTime nextFrom = Parse.toDateTimeObject(nextRow[FIELD_FROM]);
                            DateTime nextTo = Parse.toDateTimeObject(nextRow[FIELD_TO]);
                            //if (!row.IsNull(_FIELD_COST_CENTER) && !row.IsNull(_FIELD_PERCENTAGE))
                            if (newRow[FIELD_EMP_NO].Equals(nextEmpNo) && newRow[FIELD_FROM].Equals(nextFrom) && newRow[FIELD_TO].Equals(nextTo))
                            {
                                iFieldCount++;
                                newRow[FIELD_COST_CENTER + iFieldCount] = nextRow[_FIELD_COST_CENTER].ToString().Trim();
                                newRow[FIELD_PERCENTAGE + iFieldCount] = Parse.toDecimal(nextRow[_FIELD_PERCENTAGE]);
                                mRows.Add(nextRow);
                            }
                        }
                    }
                    tmpDataTable.Rows.Add(newRow);
                }
            }
            if (errors.List.Count > 0)
            {
                ClearTempTable();
                throw (new HRImportException(rawDataTable.TableName + "\r\n" + errors.Message()));
            }
            importEmpCostCenter.UploadToTempDatabase(tmpDataTable, UserID, CreateCodeIfNotExists);
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
            
        }
    }
}
