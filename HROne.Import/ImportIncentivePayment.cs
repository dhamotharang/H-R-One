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
    [DBClass("UploadIncentivePayment")]
    public class EUploadIncentivePayment : ImportDBObject
    {
        public static DBManager db = new DBManager(typeof(EUploadIncentivePayment));

        protected int m_UploadIPID;
        [DBField("UploadIPID", true, true), TextSearch, Export(false)]
        public int UploadIPID
        {
            get { return m_UploadIPID; }
            set { m_UploadIPID = value; modify("UploadIPID"); }
        }

        protected int m_EmpID;
        [DBField("EmpID"), TextSearch, Export(false), Required]
        public int EmpID
        {
            get { return m_EmpID; }
            set { m_EmpID = value; modify("EmpID"); }
        }

        protected double m_IPPercent;
        [DBField("IPPercent", "0.00"), TextSearch, Export(false), Required]
        public double IPPercent
        {
            get { return m_IPPercent; }
            set { m_IPPercent = value; modify("IPPercent"); }
        }

        protected DateTime m_IPEffDate;
        [DBField("IPEffDate"), TextSearch, Export(false)]
        public DateTime IPEffDate
        {
            get { return m_IPEffDate; }
            set { m_IPEffDate = value; modify("IPEffDate"); }
        }
    }


    /// <summary>
    /// Summary description for ImportCND
    /// </summary>
    public class ImportIncentivePaymentProcess : ImportProcessInteface
    {
        public const string TABLE_NAME = "IncentivePayment";
        public const string FIELD_EMP_NO = "Emp No";
        public const string FIELD_PERCENT = "Incentive(%)";
        public const string FIELD_EFF_DATE = "As At Date";

        private int m_UserID;
        public string Remark;
        private DateTime UploadDateTime = AppUtils.ServerDateTime();
        private DBManager db = EUploadIncentivePayment.db;

        public ImportErrorList errors = new ImportErrorList();

        public ImportIncentivePaymentProcess(DatabaseConnection dbConn, string SessionID, int UserID)
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
                        EUploadIncentivePayment IP = new EUploadIncentivePayment();
                        IP.EmpID = EmpID;
                        IP.IPPercent = HROne.CommonLib.GenericRoundingFunctions.RoundingTo(percent, 2, 2);
                        IP.IPEffDate = effDate;
                        IP.SessionID = m_SessionID;
                        IP.TransactionDate = UploadDateTime;
                        IP.ImportActionStatus = ImportDBObject.ImportActionEnum.UPDATE;

                        db.insert(dbConn, IP);
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
            ImportIncentivePaymentProcess import = new ImportIncentivePaymentProcess(dbConn, sessionID, 0);
            import.ClearTempTable();
        }

        public override void ClearTempTable()
        {
            //  Clear Old Import Session
            DBFilter sessionFilter = new DBFilter();
            if (!string.IsNullOrEmpty(m_SessionID))
                sessionFilter.add(new Match("SessionID", m_SessionID));
            EUploadIncentivePayment.db.delete(dbConn, sessionFilter);
        }

        public override void ImportToDatabase()
        {
            DataTable dataTable = GetImportDataFromTempDatabase(null);
            if (dataTable.Rows.Count > 0)
            {
                EIncentivePaymentImportBatch batchDetail = new EIncentivePaymentImportBatch();
                batchDetail.IPImportBatchDateTime = AppUtils.ServerDateTime();
                //batchDetail.IPImportBatchOriginalFilename = OriginalBatchFilename;
                batchDetail.IPImportBatchRemark = Remark;
                batchDetail.IPImportBatchUploadedBy = m_UserID;
                EIncentivePaymentImportBatch.db.insert(dbConn, batchDetail);

                foreach (DataRow row in dataTable.Rows)
                {
                    EUploadIncentivePayment obj = new EUploadIncentivePayment();
                    EUploadIncentivePayment.db.toObject(row, obj);

                    EIncentivePayment IP = new EIncentivePayment();
                    IP.IPPercent = obj.IPPercent;
                    IP.IPEffDate = obj.IPEffDate;
                    IP.EmpID = obj.EmpID;
                    IP.IPImportBatchID = batchDetail.IPImportBatchID;

                    EIncentivePayment.db.insert(dbConn, IP);
                    EUploadIncentivePayment.db.delete(dbConn, obj);
                }
            }
        }

        public static DataTable ExportTemplate(DatabaseConnection dbConn, ArrayList empList, bool IsIncludeCurrentPositionInfo)
        {
            DataTable tmpDataTable = new DataTable(TABLE_NAME);
            tmpDataTable.Columns.Add(HROne.Import.ImportIncentivePaymentProcess.FIELD_EMP_NO, typeof(string));

            if (IsIncludeCurrentPositionInfo)
            {
                ImportEmpPersonalInfoProcess.AddEmployeeInfoHeader(tmpDataTable);
                ImportEmpPositionInfoProcess.AddEmployeePositionInfoHeader(dbConn, tmpDataTable);
            }

            tmpDataTable.Columns.Add(HROne.Import.ImportIncentivePaymentProcess.FIELD_EFF_DATE, typeof(DateTime));
            tmpDataTable.Columns.Add(HROne.Import.ImportIncentivePaymentProcess.FIELD_PERCENT, typeof(double));

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