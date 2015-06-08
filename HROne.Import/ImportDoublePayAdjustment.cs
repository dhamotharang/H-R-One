using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Globalization;
using HROne.Lib.Entities;
using HROne.DataAccess;
//using perspectivemind.validation;

namespace HROne.Import
{
    [DBClass("UploadDoublePayAdjustment")]
    public class EUploadDoublePayAdjustment : ImportDBObject
    {
        public static DBManager db = new DBManager(typeof(EUploadDoublePayAdjustment));

        protected int m_UploadDoublePayAdjustID;
        [DBField("UploadDoublePayAdjustID", true, true), TextSearch, Export(false)]
        public int UploadDoublePayAdjustID
        {
            get { return m_UploadDoublePayAdjustID; }
            set { m_UploadDoublePayAdjustID = value; modify("UploadDoublePayAdjustID"); }
        }

        protected int m_EmpID;
        [DBField("EmpID"), TextSearch, Export(false), Required]
        public int EmpID
        {
            
            get { return m_EmpID; }
            set { m_EmpID = value; modify("EmpID"); }
        }

        protected double m_SalesAchievementRate;
        [DBField("SalesAchievementRate", "0.00"), TextSearch, Export(false), Required]
        public double SalesAchievementRate
        {
            get { return m_SalesAchievementRate; }
            set { m_SalesAchievementRate = value; modify("SalesAchievementRate"); }
        }

        protected DateTime m_DoublePayAdjustEffDate;
        [DBField("DoublePayAdjustEffDate"), TextSearch, Export(false)]
        public DateTime DoublePayAdjustEffDate
        {
            get { return m_DoublePayAdjustEffDate; }
            set { m_DoublePayAdjustEffDate = value; modify("DoublePayAdjustEffDate"); }
        }
    }


    /// <summary>
    /// Summary descrDoublePayAdjusttion for ImportCND
    /// </summary>
    public class ImportDoublePayAdjustmentProcess : ImportProcessInteface
    {
        public const string TABLE_NAME = "DoublePayAdjustment";
        public const string FIELD_EMP_NO = "Emp No";
        public const string FIELD_PERCENT = "ASBP(%)";
        public const string FIELD_EFF_DATE = "As At Date";

        private int m_UserID;
        public string Remark;
        private DateTime UploadDateTime = AppUtils.ServerDateTime();
        private DBManager db = EUploadDoublePayAdjustment.db;

        public ImportErrorList errors = new ImportErrorList();

        public ImportDoublePayAdjustmentProcess(DatabaseConnection dbConn, string SessionID, int UserID)
            : base(dbConn, SessionID)
        {
            m_UserID = UserID;
        }

        public DataTable UploadToTempDatabase(DataTable rawDataTable, int UserID)
        {
            if (rawDataTable == null)
                return GetImportDataFromTempDatabase(null);

            int rowCount = 1;
            //ArrayList results = new ArrayList();
            try
            {
                foreach (DataRow row in rawDataTable.Rows)
                {
                    rowCount++;
                    string EmpNo = row[FIELD_EMP_NO].ToString();
                    int EmpID = HROne.Import.Parse.GetEmpID(dbConn, EmpNo, UserID);
                    if (EmpID < 0)
                        errors.addError(ImportErrorMessage.ERROR_ACCESS_DENIED_EMP_NO, new string[] { EmpNo, rowCount.ToString() });

                    //string EffDateString = row[FIELD_EFFECTIVE_DATE].ToString();
                    //DateTime EffDate = Import.Parse.toDateTimeObject(row[FIELD_EFFECTIVE_DATE]);

                    string percentString = row[FIELD_PERCENT].ToString();
                    double percent = 0;

                    string effDateString = row[FIELD_EFF_DATE].ToString();
                    DateTime effDate = new DateTime();

                    //if (EmpID > 0 && EffDate.Ticks != 0 && double.TryParse(percentString, out percent))
                    if (EmpID > 0 && double.TryParse(percentString, out percent) && DateTime.TryParse(effDateString, out effDate))
                    {
                        EUploadDoublePayAdjustment DoublePayAdjust = new EUploadDoublePayAdjustment();
                        DoublePayAdjust.EmpID = EmpID;
                        DoublePayAdjust.SalesAchievementRate = HROne.CommonLib.GenericRoundingFunctions.RoundingTo(percent, 2, 2);
                        DoublePayAdjust.DoublePayAdjustEffDate = effDate;
                        DoublePayAdjust.SessionID = m_SessionID;
                        DoublePayAdjust.TransactionDate = UploadDateTime;
                        DoublePayAdjust.ImportActionStatus = ImportDBObject.ImportActionEnum.UPDATE;

                        db.insert(dbConn, DoublePayAdjust);
                    }
                    else
                    {
                        if (EmpID == 0)
                            errors.addError(ImportErrorMessage.ERROR_INVALID_EMP_NO, new string[] { EmpNo, rowCount.ToString() });
                        else if (effDate.Ticks == 0)
                            errors.addError(ImportErrorMessage.ERROR_INVALID_FIELD_VALUE, new string[] { FIELD_EFF_DATE + "=" + effDateString, EmpNo, rowCount.ToString() });
                        else if (!double.TryParse(percentString, out percent))
                            errors.addError(ImportErrorMessage.ERROR_INVALID_FIELD_VALUE, new string[] { FIELD_PERCENT + "=" + percentString, EmpNo, rowCount.ToString() });
                    }
                }
            }
            catch (Exception e)
            {
                errors.addError(e.Message, null);
            }
            if (errors.List.Count > 0)
            {
                ClearTempTable();
                throw (new HRImportException(rawDataTable.TableName + "\r\n" + errors.Message()));
            }
            return GetImportDataFromTempDatabase(null);
        }
        public override DataTable UploadToTempDatabase(string Filename, int UserID, string ZDoublePayAdjustPassword)
        {
            ClearTempTable();
            DataTable rawDataTable = HROne.Import.ExcelImport.parse(Filename, ZDoublePayAdjustPassword).Tables[0];
            return UploadToTempDatabase(rawDataTable, UserID);
        }

        public override DataTable GetImportDataFromTempDatabase(ListInfo info)
        {
            DBFilter sessionFilter = new DBFilter();
            sessionFilter.add(new Match("SessionID", m_SessionID));
            sessionFilter.add(new MatchField("c.EmpID", "e.EmpID"));
            //if (info != null && info.orderby != null && !info.orderby.Equals(""))
            //    sessionFilter.add(info.orderby, info.order);

            return sessionFilter.loadData(dbConn, null, "e.*, c.* ", " from " + db.dbclass.tableName + " c, " + EEmpPersonalInfo.db.dbclass.tableName + " e");
        }

        public static void ClearTempTable(DatabaseConnection dbConn, string sessionID)
        {
            ImportDoublePayAdjustmentProcess import = new ImportDoublePayAdjustmentProcess(dbConn, sessionID, 0);
            import.ClearTempTable();
        }

        public override void ClearTempTable()
        {
            //  Clear Old Import Session
            DBFilter sessionFilter = new DBFilter();
            if (!string.IsNullOrEmpty(m_SessionID))
                sessionFilter.add(new Match("SessionID", m_SessionID));
            EUploadDoublePayAdjustment.db.delete(dbConn, sessionFilter);
        }

        public override void ImportToDatabase()
        {
            DataTable dataTable = GetImportDataFromTempDatabase(null);
            if (dataTable.Rows.Count > 0)
            {
                EDoublePayAdjustmentImportBatch batchDetail = new EDoublePayAdjustmentImportBatch();
                batchDetail.DoublePayAdjustImportBatchDateTime = AppUtils.ServerDateTime();
                //batchDetail.DoublePayAdjustImportBatchOriginalFilename = OriginalBatchFilename;
                batchDetail.DoublePayAdjustImportBatchRemark = Remark;
                batchDetail.DoublePayAdjustImportBatchUploadedBy = m_UserID;
                EDoublePayAdjustmentImportBatch.db.insert(dbConn, batchDetail);

                foreach (DataRow row in dataTable.Rows)
                {
                    EUploadDoublePayAdjustment obj = new EUploadDoublePayAdjustment();
                    EUploadDoublePayAdjustment.db.toObject(row, obj);

                    EDoublePayAdjustment DoublePayAdjust = new EDoublePayAdjustment();
                    DoublePayAdjust.SalesAchievementRate = obj.SalesAchievementRate;
                    DoublePayAdjust.DoublePayAdjustEffDate = obj.DoublePayAdjustEffDate;
                    DoublePayAdjust.EmpID = obj.EmpID;
                    DoublePayAdjust.DoublePayAdjustImportBatchID = batchDetail.DoublePayAdjustImportBatchID;

                    EDoublePayAdjustment.db.insert(dbConn, DoublePayAdjust);
                    EUploadDoublePayAdjustment.db.delete(dbConn, obj);
                }
            }
        }

        public static DataTable ExportTemplate(DatabaseConnection dbConn, ArrayList empList, bool IsIncludeCurrentPositionInfo)
        {
            DataTable tmpDataTable = new DataTable(TABLE_NAME);
            tmpDataTable.Columns.Add(HROne.Import.ImportDoublePayAdjustmentProcess.FIELD_EMP_NO, typeof(string));

            if (IsIncludeCurrentPositionInfo)
            {
                ImportEmpPersonalInfoProcess.AddEmployeeInfoHeader(tmpDataTable);
                ImportEmpPositionInfoProcess.AddEmployeePositionInfoHeader(dbConn, tmpDataTable);
            }

            tmpDataTable.Columns.Add(HROne.Import.ImportDoublePayAdjustmentProcess.FIELD_EFF_DATE, typeof(DateTime));
            tmpDataTable.Columns.Add(HROne.Import.ImportDoublePayAdjustmentProcess.FIELD_PERCENT, typeof(double));

            foreach (EEmpPersonalInfo empInfo in empList)
            {
                if (EEmpPersonalInfo.db.select(dbConn, empInfo))
                {
                    DataRow row = tmpDataTable.NewRow();
                    row[FIELD_EMP_NO] = empInfo.EmpNo;

                    if (IsIncludeCurrentPositionInfo)
                    {
                        ImportEmpPersonalInfoProcess.AddEmployeeInfo(dbConn, row, empInfo.EmpID);
                        ImportEmpPositionInfoProcess.AddEmployeePositionInfo(dbConn, row, empInfo.EmpID);
                    }
                    tmpDataTable.Rows.Add(row);
                }
            }
            if (IsIncludeCurrentPositionInfo)
                ImportEmpPositionInfoProcess.RetriveHierarchyLevelHeader(dbConn, tmpDataTable);
            return tmpDataTable;
        }
    }
}