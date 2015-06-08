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
    [DBClass("UploadHitRateProcess")]
    public class EUploadHitRateProcess : ImportDBObject
    {
        public static DBManager db = new DBManager(typeof(EUploadHitRateProcess));

        protected int m_UploadHitRateProcessID;
        [DBField("UploadHitRateProcessID", true, true), TextSearch, Export(false)]
        public int UploadHitRateProcessID
        {
            get { return m_UploadHitRateProcessID; }
            set { m_UploadHitRateProcessID = value; modify("UploadHitRateProcessID"); }
        }

        protected int m_EmpID;
        [DBField("EmpID"), TextSearch, Export(false), Required]
        public int EmpID
        {
            get { return m_EmpID; }
            set { m_EmpID = value; modify("EmpID"); }
        }

        protected int m_payCodeID;
        [DBField("payCodeID"), TextSearch, Export(false), Required]
        public int payCodeID
        {
            get { return m_payCodeID; }
            set { m_payCodeID = value; modify("payCodeID"); }
        }

        protected double m_HitRate;
        [DBField("HitRate", "0.00"), TextSearch, Export(false), Required]
        public double HitRate
        {
            get { return m_HitRate; }
            set { m_HitRate = value; modify("HitRate"); }
        }

        //protected DateTime m_HitRateProcessEffDate;
        //[DBField("HitRateProcessEffDate"), TextSearch, Export(false)]
        //public DateTime HitRateProcessEffDate
        //{
        //    get { return m_HitRateProcessEffDate; }
        //    set { m_HitRateProcessEffDate = value; modify("HitRateProcessEffDate"); }
        //}
    }


    /// <summary>
    /// Summary descrHitRateProcesstion for ImportCND
    /// </summary>
    public class ImportHitRateBasedPaymentProcess : ImportProcessInteface
    {
        public const string TABLE_NAME = "HitRateBasedPaymentProcess";
        public const string FIELD_EMP_NO = "Emp No";
        public const string FIELD_PAYMENT_CODE = "Payment Code";
        public const string FIELD_PERCENT = "Hit Rate(%)";
//        public const string FIELD_EFF_DATE = "As At Date";

        private int m_UserID;
        public string Remark;
        private DateTime UploadDateTime = AppUtils.ServerDateTime();
        private DBManager db = EUploadHitRateProcess.db;

        public ImportErrorList errors = new ImportErrorList();

        public ImportHitRateBasedPaymentProcess(DatabaseConnection dbConn, string SessionID, int UserID)
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

                    string paymentCode = row[FIELD_PAYMENT_CODE].ToString();
                    int paymentCodeID = Parse.GetPaymentCodeID(dbConn, paymentCode);

                    string percentString = row[FIELD_PERCENT].ToString();
                    double percent = 0;

                    //string effDateString = row[FIELD_EFF_DATE].ToString();
                    //DateTime effDate = new DateTime();

                    if (EmpID > 0 && double.TryParse(percentString, out percent)  && paymentCodeID > 0)
                    {
                        EUploadHitRateProcess HitRateProcess = new EUploadHitRateProcess();
                        HitRateProcess.EmpID = EmpID;
                        HitRateProcess.payCodeID = paymentCodeID;
                        HitRateProcess.HitRate = HROne.CommonLib.GenericRoundingFunctions.RoundingTo(percent, 2, 2);
                        HitRateProcess.payCodeID = paymentCodeID;
                        HitRateProcess.SessionID = m_SessionID;
                        HitRateProcess.TransactionDate = UploadDateTime;
                        HitRateProcess.ImportActionStatus = ImportDBObject.ImportActionEnum.UPDATE;

                        db.insert(dbConn, HitRateProcess);
                    }
                    else
                    {
                        if (EmpID == 0)
                            errors.addError(ImportErrorMessage.ERROR_INVALID_EMP_NO, new string[] { EmpNo, rowCount.ToString() });
                        else if (percent <= 0)
                            errors.addError(ImportErrorMessage.ERROR_INVALID_FIELD_VALUE, new string[] { FIELD_PERCENT + "=" + percentString, EmpNo, rowCount.ToString() });
                        else if (paymentCodeID <= 0)
                            errors.addError(ImportErrorMessage.ERROR_INVALID_FIELD_VALUE, new string[] { FIELD_PAYMENT_CODE + "=" + paymentCode, EmpNo, rowCount.ToString() });
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

        public override DataTable UploadToTempDatabase(string Filename, int UserID, string ZHitRateProcessPassword)
        {
            ClearTempTable();
            DataTable rawDataTable = HROne.Import.ExcelImport.parse(Filename, ZHitRateProcessPassword).Tables[0];
            return UploadToTempDatabase(rawDataTable, UserID);
        }

        public override DataTable GetImportDataFromTempDatabase(ListInfo info)
        {
            DBFilter sessionFilter = new DBFilter();
            sessionFilter.add(new Match("SessionID", m_SessionID));
            sessionFilter.add(new MatchField("c.EmpID", "e.EmpID"));
            //if (info != null && info.orderby != null && !info.orderby.Equals(""))
            //    sessionFilter.add(info.orderby, info.order);

            return sessionFilter.loadData(dbConn, null, "e.*, c.* ", "from " + db.dbclass.tableName + " c, EmpPersonalInfo e ");
        }

        public static void ClearTempTable(DatabaseConnection dbConn, string sessionID)
        {
            ImportHitRateBasedPaymentProcess import = new ImportHitRateBasedPaymentProcess(dbConn, sessionID, 0);
            import.ClearTempTable();
        }

        public override void ClearTempTable()
        {
            //  Clear Old Import Session
            DBFilter sessionFilter = new DBFilter();
            if (!string.IsNullOrEmpty(m_SessionID))
                sessionFilter.add(new Match("SessionID", m_SessionID));
            EUploadHitRateProcess.db.delete(dbConn, sessionFilter);
        }

        public override void ImportToDatabase()
        {
            DataTable dataTable = GetImportDataFromTempDatabase(null);
            if (dataTable.Rows.Count > 0)
            {
                EHitRateProcessImportBatch batchDetail = new EHitRateProcessImportBatch();
                batchDetail.HitRateProcessImportBatchDateTime = AppUtils.ServerDateTime();
                //batchDetail.HitRateProcessImportBatchOriginalFilename = OriginalBatchFilename;
                batchDetail.HitRateProcessImportBatchRemark = Remark;
                batchDetail.HitRateProcessImportBatchUploadedBy = m_UserID;
                EHitRateProcessImportBatch.db.insert(dbConn, batchDetail);
                

                foreach (DataRow row in dataTable.Rows)
                {
                    EUploadHitRateProcess obj = new EUploadHitRateProcess();
                    EUploadHitRateProcess.db.toObject(row, obj);

                    EHitRateProcess HitRateProcess = new EHitRateProcess();
                    HitRateProcess.HitRate = obj.HitRate;
                    HitRateProcess.payCodeID = obj.payCodeID;
                    HitRateProcess.EmpID = obj.EmpID;
                    HitRateProcess.HitRateProcessImportBatchID = batchDetail.HitRateProcessImportBatchID;

                    EHitRateProcess.db.insert(dbConn, HitRateProcess);
                    EUploadHitRateProcess.db.delete(dbConn, obj);
                }
            }
        }

        public static DataTable ExportTemplate(DatabaseConnection dbConn, ArrayList empList, bool IsIncludeCurrentPositionInfo)
        {
            DataTable tmpDataTable = new DataTable(TABLE_NAME);
            tmpDataTable.Columns.Add(HROne.Import.ImportHitRateBasedPaymentProcess.FIELD_EMP_NO, typeof(string));

            if (IsIncludeCurrentPositionInfo)
            {
                ImportEmpPersonalInfoProcess.AddEmployeeInfoHeader(tmpDataTable);
                ImportEmpPositionInfoProcess.AddEmployeePositionInfoHeader(dbConn, tmpDataTable);
            }

            //tmpDataTable.Columns.Add(HROne.Import.ImportHitRateBasedPaymentProcess.FIELD_EFF_DATE, typeof(DateTime));
            tmpDataTable.Columns.Add(HROne.Import.ImportHitRateBasedPaymentProcess.FIELD_PAYMENT_CODE, typeof(string));
            tmpDataTable.Columns.Add(HROne.Import.ImportHitRateBasedPaymentProcess.FIELD_PERCENT, typeof(double));

            foreach (EEmpPersonalInfo empInfo in empList)
            {
                if (EEmpPersonalInfo.db.select(dbConn, empInfo))
                {
                    DBFilter m_empRPFilter = new DBFilter();
                    m_empRPFilter.add(new Match("EmpID", empInfo.EmpID));
                    m_empRPFilter.add(new NullTerm("EmpRPEffTo"));
                    m_empRPFilter.add("EmpRPEffFr", true);
                    foreach (EEmpRecurringPayment m_rpInfo in EEmpRecurringPayment.db.select(dbConn, m_empRPFilter))
                    {
                        EPaymentCode m_paymentCode = EPaymentCode.GetObject(dbConn, m_rpInfo.PayCodeID);
                        if (m_paymentCode.PaymentCodeIsHitRateBased)
                        {
                            DataRow row = tmpDataTable.NewRow();
                        
                            row[FIELD_EMP_NO] = empInfo.EmpNo;

                            if (IsIncludeCurrentPositionInfo)
                            {
                                ImportEmpPersonalInfoProcess.AddEmployeeInfo(dbConn, row, empInfo.EmpID);
                                ImportEmpPositionInfoProcess.AddEmployeePositionInfo(dbConn, row, empInfo.EmpID);
                            }

                            row[FIELD_PAYMENT_CODE] = m_paymentCode.PaymentCode;

                            int m_monthDiff = AppUtils.ServerDateTime().Year * 12 + AppUtils.ServerDateTime().Month - 
                                                 (empInfo.EmpServiceDate.Year * 12 + empInfo.EmpServiceDate.Month);

                            if (m_paymentCode.PaymentCodeDefaultRateAtMonth1 > 0 && m_monthDiff == 0)   // service start in current month
                            {
                                row[FIELD_PERCENT] = m_paymentCode.PaymentCodeDefaultRateAtMonth1;
                            }
                            else if (m_paymentCode.PaymentCodeDefaultRateAtMonth2 > 0 && m_monthDiff == 1) // emp service start in last month
                            {
                                row[FIELD_PERCENT] = m_paymentCode.PaymentCodeDefaultRateAtMonth2;
                            }
                            else if (m_paymentCode.PaymentCodeDefaultRateAtMonth2 > 0 && m_monthDiff == 2) // emp service start in 2 months before this month
                            {
                                row[FIELD_PERCENT] = m_paymentCode.PaymentCodeDefaultRateAtMonth3;
                            }
                            tmpDataTable.Rows.Add(row);
                        }
                    }
                }
            }
            if (IsIncludeCurrentPositionInfo)
                ImportEmpPositionInfoProcess.RetriveHierarchyLevelHeader(dbConn, tmpDataTable);
            return tmpDataTable;
        }
    }
}