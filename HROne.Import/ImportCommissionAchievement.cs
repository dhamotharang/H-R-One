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
    [DBClass("UploadCommissionAchievement")]
    public class EUploadCommissionAchievement : ImportDBObject
    {
        public static DBManager db = new DBManager(typeof(EUploadCommissionAchievement));

        protected int m_UploadCAID;
        [DBField("UploadCAID", true, true), TextSearch, Export(false)]
        public int UploadCAID
        {
            get { return m_UploadCAID; }
            set { m_UploadCAID = value; modify("UploadCAID"); }
        }

        protected int m_EmpID;
        [DBField("EmpID"), TextSearch, Export(false), Required]
        public int EmpID
        {
            get { return m_EmpID; }
            set { m_EmpID = value; modify("EmpID"); }
        }

        protected double m_CAPercent;
        [DBField("CAPercent", "0.00"), TextSearch, Export(false), Required]
        public double CAPercent
        {
            get { return m_CAPercent; }
            set { m_CAPercent = value; modify("CAPercent"); }
        }

        protected DateTime m_CAEffDate;
        [DBField("CAEffDate"), TextSearch, Export(false)]
        public DateTime CAEffDate
        {
            get { return m_CAEffDate; }
            set { m_CAEffDate = value; modify("CAEffDate"); }
        }
    }


    /// <summary>
    /// Summary description for ImportCND
    /// </summary>
    public class ImportCommissionAchievementProcess : ImportProcessInteface
    {
        public const string TABLE_NAME = "CommissionAchievement";
        public const string FIELD_EMP_NO = "Emp No";
        public const string FIELD_BASIC_SALARY = "Target Salary";
        public const string FIELD_FPS = "FPS(%)";
        public const string FIELD_PERCENT = "Achievement(%)";
        public const string FIELD_EFF_DATE = "As At Date";

        private int m_UserID;
        public string Remark;
        private DateTime UploadDateTime = AppUtils.ServerDateTime();
        private DBManager db = EUploadCommissionAchievement.db;

        public ImportErrorList errors = new ImportErrorList();

        public ImportCommissionAchievementProcess(DatabaseConnection dbConn, string SessionID, int UserID)
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
                        EUploadCommissionAchievement CA = new EUploadCommissionAchievement();
                        CA.EmpID = EmpID;
                        CA.CAPercent = HROne.CommonLib.GenericRoundingFunctions.RoundingTo(percent, 2, 2);
                        CA.CAEffDate = effDate;
                        CA.SessionID = m_SessionID;
                        CA.TransactionDate = UploadDateTime;
                        CA.ImportActionStatus = ImportDBObject.ImportActionEnum.UPDATE;
                        
                        db.insert(dbConn, CA);
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
        public override DataTable UploadToTempDatabase(string Filename, int UserID, string ZipPassword)
        {
            ClearTempTable();
            DataTable rawDataTable = HROne.Import.ExcelImport.parse(Filename, ZipPassword).Tables[0];
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
            ImportCommissionAchievementProcess import = new ImportCommissionAchievementProcess(dbConn, sessionID, 0);
            import.ClearTempTable();
        }

        public override void ClearTempTable()
        {
            //  Clear Old Import Session
            DBFilter sessionFilter = new DBFilter();
            if (!string.IsNullOrEmpty(m_SessionID))
                sessionFilter.add(new Match("SessionID", m_SessionID));
            EUploadCommissionAchievement.db.delete(dbConn, sessionFilter);
            
        }

        public override void ImportToDatabase()
        {
            DataTable dataTable = GetImportDataFromTempDatabase(null);
            if (dataTable.Rows.Count > 0)
            {
                ECommissionAchievementImportBatch batchDetail = new ECommissionAchievementImportBatch();
                batchDetail.CAImportBatchDateTime = AppUtils.ServerDateTime();
                //batchDetail.CAImportBatchOriginalFilename = OriginalBatchFilename;
                batchDetail.CAImportBatchRemark = Remark;
                batchDetail.CAImportBatchUploadedBy = m_UserID;
                ECommissionAchievementImportBatch.db.insert(dbConn, batchDetail);

                foreach (DataRow row in dataTable.Rows)
                {
                    EUploadCommissionAchievement obj = new EUploadCommissionAchievement();
                    EUploadCommissionAchievement.db.toObject(row, obj);

                    ECommissionAchievement CA = new ECommissionAchievement();
                    CA.CAPercent = obj.CAPercent;
                    CA.CAEffDate = obj.CAEffDate;
                    CA.EmpID = obj.EmpID;
                    CA.CAImportBatchID = batchDetail.CAImportBatchID;

                    ECommissionAchievement.db.insert(dbConn, CA);
                    EUploadCommissionAchievement.db.delete(dbConn, obj);
                }
            }
        }

        public static DataTable ExportTemplate(DatabaseConnection dbConn, ArrayList empList, bool IsIncludeCurrentPositionInfo)
        {
            DataTable tmpDataTable = new DataTable(TABLE_NAME);
            tmpDataTable.Columns.Add(HROne.Import.ImportCommissionAchievementProcess.FIELD_EMP_NO, typeof(string));

            if (IsIncludeCurrentPositionInfo)
            {
                ImportEmpPersonalInfoProcess.AddEmployeeInfoHeader(tmpDataTable);
                ImportEmpPositionInfoProcess.AddEmployeePositionInfoHeader(dbConn, tmpDataTable);
            }

            tmpDataTable.Columns.Add(HROne.Import.ImportCommissionAchievementProcess.FIELD_BASIC_SALARY, typeof(double));
            tmpDataTable.Columns.Add(HROne.Import.ImportCommissionAchievementProcess.FIELD_FPS, typeof(double));

            tmpDataTable.Columns.Add(HROne.Import.ImportCommissionAchievementProcess.FIELD_EFF_DATE, typeof(DateTime));
            tmpDataTable.Columns.Add(HROne.Import.ImportCommissionAchievementProcess.FIELD_PERCENT, typeof(double));

            foreach (EEmpPersonalInfo empInfo in empList)
            {
                if (EEmpPersonalInfo.db.select(dbConn, empInfo))
                {
                    DBFilter m_recurringPaymentFilter = new DBFilter();
                    m_recurringPaymentFilter.add(new Match("EmpID", empInfo.EmpID));
                    m_recurringPaymentFilter.add(new NullTerm("EmpRPEffTo"));
                    m_recurringPaymentFilter.add(AppUtils.GetPayemntCodeDBTermByPaymentType(dbConn, "PayCodeID", "BASICSAL"));
                    foreach (EEmpRecurringPayment m_rpInfo in EEmpRecurringPayment.db.select(dbConn, m_recurringPaymentFilter))
                    {
                        DataRow row = tmpDataTable.NewRow();
                        row[FIELD_EMP_NO] = empInfo.EmpNo;

                        if (IsIncludeCurrentPositionInfo)
                        {
                            ImportEmpPersonalInfoProcess.AddEmployeeInfo(dbConn, row, empInfo.EmpID);
                            ImportEmpPositionInfoProcess.AddEmployeePositionInfo(dbConn, row, empInfo.EmpID);
                        }
                        row[FIELD_BASIC_SALARY] = m_rpInfo.EmpRPBasicSalary;
                        row[FIELD_FPS] = m_rpInfo.EmpRPFPS;

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