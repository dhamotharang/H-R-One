//using System;
//using System.Data;
//using System.Configuration;
//using System.Collections;
//using System.Globalization;
//using HROne.Lib.Entities;
//using HROne.DataAccess;
////using perspectivemind.validation;

//namespace HROne.Import
//{
//    [DBClass("UploadClaimsAndDeductions")]

//    public class EUploadClaimsAndDeductions : ImportDBObject
//    {
//        public static DBManager db = new DBManager(typeof(EUploadClaimsAndDeductions));

//        protected int m_UploadCNDID;
//        [DBField("UploadCNDID", true, true), TextSearch, Export(false)]
//        public int UploadCNDID
//        {
//            get { return m_UploadCNDID; }
//            set { m_UploadCNDID = value; modify("UploadCNDID"); }
//        }

//        protected DateTime m_CNDEffDate;
//        [DBField("CNDEffDate"), TextSearch, Export(false), Required]
//        public DateTime CNDEffDate
//        {
//            get { return m_CNDEffDate; }
//            set { m_CNDEffDate = value; modify("CNDEffDate"); }
//        }

//        protected int m_EmpID;
//        [DBField("EmpID"), TextSearch, Export(false), Required]
//        public int EmpID
//        {
//            get { return m_EmpID; }
//            set { m_EmpID = value; modify("EmpID"); }
//        }

//        protected int m_PayCodeID;
//        [DBField("PayCodeID"), TextSearch, Export(false), Required]
//        public int PayCodeID
//        {
//            get { return m_PayCodeID; }
//            set { m_PayCodeID = value; modify("PayCodeID"); }
//        }

//        protected double m_CNDAmount;
//        [DBField("CNDAmount", "0.00"), TextSearch, Export(false), Required]
//        public double CNDAmount
//        {
//            get { return m_CNDAmount; }
//            set { m_CNDAmount = value; modify("CNDAmount"); }
//        }

//        protected string m_CurrencyID;
//        [DBField("CurrencyID"), TextSearch, Export(false), Required]
//        public string CurrencyID
//        {
//            get { return m_CurrencyID; }
//            set { m_CurrencyID = value; modify("CurrencyID"); }
//        }

//        protected string m_CNDPayMethod;
//        [DBField("CNDPayMethod"), TextSearch, Export(false), Required]
//        public string CNDPayMethod
//        {
//            get { return m_CNDPayMethod; }
//            set { m_CNDPayMethod = value; modify("CNDPayMethod"); }
//        }

//        protected int m_EmpAccID;
//        [DBField("EmpAccID"), TextSearch, Export(false)]
//        public int EmpAccID
//        {
//            get { return m_EmpAccID; }
//            set { m_EmpAccID = value; modify("EmpAccID"); }
//        }

//        protected double m_CNDNumOfDayAdj;
//        [DBField("CNDNumOfDayAdj"), TextSearch, Export(false)]
//        public double CNDNumOfDayAdj
//        {
//            get { return m_CNDNumOfDayAdj; }
//            set { m_CNDNumOfDayAdj = value; modify("CNDNumOfDayAdj"); }
//        }

//        protected bool m_CNDIsRestDayPayment;
//        [DBField("CNDIsRestDayPayment"), TextSearch, Export(false)]
//        public bool CNDIsRestDayPayment
//        {
//            get { return m_CNDIsRestDayPayment; }
//            set { m_CNDIsRestDayPayment = value; modify("CNDIsRestDayPayment"); }
//        }

//        protected int m_CostCenterID;
//        [DBField("CostCenterID"), TextSearch, Export(false)]
//        public int CostCenterID
//        {
//            get { return m_CostCenterID; }
//            set { m_CostCenterID = value; modify("CostCenterID"); }
//        }

//        protected string m_CNDRemark;
//        [DBField("CNDRemark"), TextSearch, Export(false)]
//        public String CNDRemark
//        {
//            get { return m_CNDRemark; }
//            set { m_CNDRemark = value; modify("CNDRemark"); }
//        }

//    }


//    /// <summary>
//    /// Summary description for ImportCND
//    /// </summary>
//    public class ImportClaimsAndDeductionsProcess : ImportProcessInteface
//    {
//        public const string TABLE_NAME = "ClaimsAndDeductions";

//        public const string FIELD_EMP_NO = "Emp No";
//        public const string FIELD_PAYMENT_CODE = "Payment Code";
//        public const string FIELD_BANK_ACCOUNT_NO = "Bank Account No";
//        public const string FIELD_EFFECTIVE_DATE = "Effective Date";
//        public const string FIELD_PAYMENT_METHOD = "Payment Method";
//        public const string FIELD_AMOUNT = "Amount";
//        public const string FIELD_NUM_OF_DAY_ADJUST = "No of Day Adjust";
//        public const string FIELD_COST_CENTER = "Cost Center";
//        public const string FIELD_REST_PAYMENT = "Rest Payment";
//        public const string FIELD_REMARK = "Remark";

//        private string m_SessionID;
//        private int m_UserID;
//        public string Remark;
//        private DateTime UploadDateTime = AppUtils.ServerDateTime();
//        private DBManager db = EUploadClaimsAndDeductions.db;

//        public ImportErrorList errors = new ImportErrorList();



//        public ImportClaimsAndDeductionsProcess(string SessionID, int UserID)
//        {
//            m_SessionID = SessionID;
//            m_UserID = UserID;
//        }

//        public DataTable UploadToTempDatabase(DataTable rawDataTable, int UserID)
//        {
//            if (rawDataTable == null)
//                return GetImportDataFromTempDatabase(null);

//            int rowCount = 1;
//            //ArrayList results = new ArrayList();
//            try
//            {
//                foreach (DataRow row in rawDataTable.Rows)
//                {
//                    rowCount++;
//                    string EmpNo = row[FIELD_EMP_NO].ToString();
//                    int EmpID = HROne.Import.Parse.GetEmpID(dbConn, EmpNo, UserID);
//                    string PaymentCode = row[FIELD_PAYMENT_CODE].ToString();
//                    int PayCodeID = HROne.Import.Parse.GetPaymentCodeID(PaymentCode);
//                    string BankCode = row[FIELD_BANK_ACCOUNT_NO].ToString();
//                    int BankAccID = HROne.Import.Parse.GetEmpAccID(dbConn, BankCode, EmpID);
//                    string EffDateString = row[FIELD_EFFECTIVE_DATE].ToString();
//                    long tryEffDateString = 0;
//                    DateTime EffDate = Import.Parse.toDateTimeObject(row[FIELD_EFFECTIVE_DATE]);

//                    //if (EffDateString.Trim().Length == 8 && long.TryParse(EffDateString, out tryEffDateString))
//                    //{
//                    //    EffDate = new DateTime(int.Parse(EffDateString.Substring(0, 4)), int.Parse(EffDateString.Substring(4, 2)), int.Parse(EffDateString.Substring(6, 2)));
//                    //}
//                    string PayMethod = row[FIELD_PAYMENT_METHOD].ToString().Trim();

//                    string Remark = row[FIELD_REMARK].ToString();

//                    string amountString = row[FIELD_AMOUNT].ToString();
//                    double amount = 0;

//                    double NumOfDayAdjust = 0;

//                    if (rawDataTable.Columns.Contains(FIELD_NUM_OF_DAY_ADJUST))
//                    {
//                        string NumOfDayAdjustString = row[FIELD_NUM_OF_DAY_ADJUST].ToString();
//                        if (!string.IsNullOrEmpty(NumOfDayAdjustString.Trim()))
//                            if (!double.TryParse(NumOfDayAdjustString, out NumOfDayAdjust))
//                                errors.addError(ImportErrorMessage.ERROR_INVALID_FIELD_VALUE, new string[] { FIELD_NUM_OF_DAY_ADJUST + "=" + NumOfDayAdjustString, EmpNo, rowCount.ToString() });

//                    }

//                    bool IsRestDayPayment = false;
//                    if (rawDataTable.Columns.Contains(FIELD_REST_PAYMENT))
//                        if (row.IsNull(FIELD_REST_PAYMENT))
//                            IsRestDayPayment = false;
//                        else
//                            IsRestDayPayment = (row[FIELD_REST_PAYMENT].ToString().Equals("Y", StringComparison.CurrentCultureIgnoreCase) || row[FIELD_REST_PAYMENT].ToString().Equals("Yes", StringComparison.CurrentCultureIgnoreCase));

//                    int CostCenterID = 0;
//                    if (rawDataTable.Columns.Contains(FIELD_COST_CENTER))
//                    {
//                        string CostCenter = row[FIELD_COST_CENTER].ToString();
//                        if (!string.IsNullOrEmpty(CostCenter))
//                        {
//                            CostCenterID = HROne.Import.Parse.GetCostCenterID(CostCenter, false);
//                            if (CostCenterID<=0)
//                                errors.addError(ImportErrorMessage.ERROR_INVALID_FIELD_VALUE, new string[] { FIELD_COST_CENTER + "=" + CostCenter, EmpNo, rowCount.ToString() });
//                        }
//                    }

//                    //if (!double.TryParse(amountString, out amount))
//                    //{
//                    //    throw new ImportException("Amount", amountString, "Invalid Number Format");
//                    //}


//                    if (EmpID > 0 && PayCodeID > 0 && (BankAccID > 0 || BankAccID == 0 && BankCode.Trim().Equals(string.Empty)) && EffDate.Ticks != 0 && double.TryParse(amountString, out amount))
//                    {
//                        EUploadClaimsAndDeductions CND = new EUploadClaimsAndDeductions();
//                        CND.EmpID = EmpID;
//                        CND.CNDEffDate = EffDate;
//                        CND.PayCodeID = PayCodeID;
//                        CND.CurrencyID = HROne.Lib.ExchangeCurrency.DefaultCurrency();
//                        CND.EmpAccID = BankAccID;
//                        CND.CNDNumOfDayAdj = NumOfDayAdjust;
//                        if (PayMethod.Equals("Autopay", StringComparison.CurrentCultureIgnoreCase))
//                        {
//                            CND.CNDPayMethod = "A";
//                            if (CND.EmpAccID == 0)
//                            {
//                                EEmpBankAccount bankAccount = EEmpBankAccount.GetDefaultBankAccount(dbConn, CND.EmpID);
//                                if (bankAccount != null)
//                                    CND.EmpAccID = bankAccount.EmpBankAccountID;
//                                else
//                                    errors.addError(ImportErrorMessage.ERROR_INVALID_FIELD_VALUE, new string[] { FIELD_BANK_ACCOUNT_NO + "=" + BankCode, EmpNo, rowCount.ToString() });

//                            }
//                        }
//                        else if (PayMethod.Equals("Cash", StringComparison.CurrentCultureIgnoreCase))
//                            CND.CNDPayMethod = "C";
//                        else if (PayMethod.Equals("Cheque", StringComparison.CurrentCultureIgnoreCase))
//                            CND.CNDPayMethod = "Q";
//                        else
//                            CND.CNDPayMethod = "O";
//                        CND.CNDIsRestDayPayment = IsRestDayPayment;
//                        CND.CostCenterID = CostCenterID;
//                        CND.CNDRemark = Remark;
//                        CND.CNDAmount = HROne.CommonLib.GenericRoundingFunctions.RoundingTo(amount, 2, 2);
//                        CND.SessionID = m_SessionID;
//                        CND.TransactionDate = UploadDateTime;
//                        CND.ImportActionStatus = ImportDBObject.ImportActionEnum.UPDATE;
//                        db.insert(dbConn, CND);
//                        //results.Add(CND);
//                    }
//                    else
//                    {
//                        if (EmpID == 0)
//                            errors.addError(ImportErrorMessage.ERROR_INVALID_EMP_NO, new string[] { EmpNo, rowCount.ToString() });
//                        else if (PayCodeID == 0)
//                            errors.addError(ImportErrorMessage.ERROR_INVALID_FIELD_VALUE, new string[] { FIELD_PAYMENT_CODE + "=" + PaymentCode, EmpNo, rowCount.ToString() });
//                        else if (PayCodeID == 0)
//                            errors.addError(ImportErrorMessage.ERROR_INVALID_FIELD_VALUE, new string[] { FIELD_PAYMENT_CODE + "=" + PaymentCode, EmpNo, rowCount.ToString() });
//                        else if (EffDate.Ticks == 0)
//                            errors.addError(ImportErrorMessage.ERROR_INVALID_FIELD_VALUE, new string[] { FIELD_EFFECTIVE_DATE + "=" + EffDateString, EmpNo, rowCount.ToString() });
//                        else if (BankAccID == 0 && !BankCode.Trim().Equals(string.Empty))
//                            errors.addError(ImportErrorMessage.ERROR_INVALID_FIELD_VALUE, new string[] { FIELD_BANK_ACCOUNT_NO + "=" + BankCode, EmpNo, rowCount.ToString() });
//                        else if (!double.TryParse(amountString, out amount))
//                            errors.addError(ImportErrorMessage.ERROR_INVALID_FIELD_VALUE, new string[] { FIELD_AMOUNT + "=" + amountString, EmpNo, rowCount.ToString() });
//                    }

//                }
//            }
//            catch (Exception e)
//            {
//                errors.addError(e.Message, null);
//            }
//            if (errors.List.Count > 0)
//            {
//                ClearTempTable();
//                throw (new HRImportException(rawDataTable.TableName + '\n' + errors.Message()));
//            }
//            return GetImportDataFromTempDatabase(null);

//        }
//        public DataTable UploadToTempDatabase(string Filename, int UserID, string ZipPassword)
//        {
//            ClearTempTable();
//            DataTable rawDataTable = HROne.Import.ExcelImport.parse(Filename, ZipPassword).Tables[0];
//            return UploadToTempDatabase(rawDataTable, UserID);
//        }
//        public DataTable GetImportDataFromTempDatabase(ListInfo info)
//        {
//            DBFilter sessionFilter = new DBFilter();
//            sessionFilter.add(new Match("SessionID", m_SessionID));
//            sessionFilter.add(new MatchField("c.EmpID", "e.EmpID"));
//            //if (info != null && info.orderby != null && !info.orderby.Equals(""))
//            //    sessionFilter.add(info.orderby, info.order);

//            return sessionFilter.loadData(dbConn, null, "e.*, c.* ", " from " + db.dbclass.tableName + " c, " + EEmpPersonalInfo.db.dbclass.tableName + " e");

//        }

//        public static void ClearTempTable(string sessionID)
//        {
//            ImportClaimsAndDeductionsProcess import = new ImportClaimsAndDeductionsProcess(sessionID, 0);
//            import.ClearTempTable();
//        }

//        public void ClearTempTable()
//        {
//            //  Clear Old Import Session
//            DBFilter sessionFilter = new DBFilter();
//            if (!string.IsNullOrEmpty(m_SessionID))
//                sessionFilter.add(new Match("SessionID", m_SessionID));
//            EUploadClaimsAndDeductions.db.delete(dbConn, sessionFilter);
//        }

//        public void ImportToDatabase()
//        {
//            DataTable dataTable = GetImportDataFromTempDatabase(null);
//            if (dataTable.Rows.Count > 0)
//            {


//                EClaimsAndDeductionsImportBatch batchDetail = new EClaimsAndDeductionsImportBatch();
//                batchDetail.CNDImportBatchDateTime = AppUtils.ServerDateTime();
//                //batchDetail.CNDImportBatchOriginalFilename = OriginalBatchFilename;
//                batchDetail.CNDImportBatchRemark = Remark;
//                batchDetail.CNDImportBatchUploadedBy = m_UserID;
//                EClaimsAndDeductionsImportBatch.db.insert(dbConn, batchDetail);

//                foreach (DataRow row in dataTable.Rows)
//                {
//                    EUploadClaimsAndDeductions obj = new EUploadClaimsAndDeductions();
//                    EUploadClaimsAndDeductions.db.toObject(row, obj);

//                    EClaimsAndDeductions CND = new EClaimsAndDeductions();
//                    CND.CNDAmount = obj.CNDAmount;
//                    CND.CNDEffDate = obj.CNDEffDate;
//                    CND.CNDNumOfDayAdj = obj.CNDNumOfDayAdj;
//                    CND.CNDPayMethod = obj.CNDPayMethod;
//                    CND.CNDRemark = obj.CNDRemark;
//                    CND.CurrencyID = obj.CurrencyID;
//                    CND.EmpAccID = obj.EmpAccID;
//                    CND.EmpID = obj.EmpID;
//                    CND.PayCodeID = obj.PayCodeID;
//                    CND.CostCenterID = obj.CostCenterID;
//                    CND.CNDIsRestDayPayment = obj.CNDIsRestDayPayment;
//                    CND.CNDImportBatchID = batchDetail.CNDImportBatchID;
//                    EClaimsAndDeductions.db.insert(dbConn, CND);
//                    EUploadClaimsAndDeductions.db.delete(dbConn, obj);
//                    //DBFilter dbfilter = new DBFilter();
//                    //dbfilter.add(new Match("EmpID", obj.EmpID));
//                    //dbfilter.add(new Match("CNDEffDate", obj.CNDEffDate));
//                    //dbfilter.add(new Match("PayCodeID", obj.PayCodeID));
//                    //dbfilter.add(new Match("CNDPayMethod", obj.CNDPayMethod));
//                    //dbfilter.add(new Match("EmpAccID", obj.EmpAccID));
//                    //dbfilter.add(new Match("

//                }
//            }
//        }

//        public static DataTable ExportTemplate(ArrayList empList, bool IsIncludeCurrentPositionInfo)
//        {
//            DataTable tmpDataTable = new DataTable(TABLE_NAME);
//            tmpDataTable.Columns.Add(HROne.Import.ImportClaimsAndDeductionsProcess.FIELD_EMP_NO, typeof(string));

//            if (IsIncludeCurrentPositionInfo)
//            {
//                ImportEmpPersonalInfoProcess.AddEmployeeInfoHeader(tmpDataTable);
//                ImportEmpPositionInfoProcess.AddEmployeePositionInfoHeader(dbConn, tmpDataTable);
//            }

//            tmpDataTable.Columns.Add(HROne.Import.ImportClaimsAndDeductionsProcess.FIELD_EFFECTIVE_DATE, typeof(DateTime));
//            tmpDataTable.Columns.Add(HROne.Import.ImportClaimsAndDeductionsProcess.FIELD_PAYMENT_CODE, typeof(string));
//            tmpDataTable.Columns.Add(HROne.Import.ImportClaimsAndDeductionsProcess.FIELD_PAYMENT_METHOD, typeof(string));
//            tmpDataTable.Columns.Add(HROne.Import.ImportClaimsAndDeductionsProcess.FIELD_BANK_ACCOUNT_NO, typeof(string));
//            tmpDataTable.Columns.Add(HROne.Import.ImportClaimsAndDeductionsProcess.FIELD_AMOUNT, typeof(double));
//            tmpDataTable.Columns.Add(HROne.Import.ImportClaimsAndDeductionsProcess.FIELD_NUM_OF_DAY_ADJUST, typeof(double));
//            tmpDataTable.Columns.Add(HROne.Import.ImportClaimsAndDeductionsProcess.FIELD_REST_PAYMENT, typeof(string));
//            tmpDataTable.Columns.Add(HROne.Import.ImportClaimsAndDeductionsProcess.FIELD_COST_CENTER, typeof(string));
//            tmpDataTable.Columns.Add(HROne.Import.ImportClaimsAndDeductionsProcess.FIELD_REMARK, typeof(string));


//            foreach (EEmpPersonalInfo empInfo in empList)
//            {
//                if (EEmpPersonalInfo.db.select(dbConn, empInfo))
//                {
//                    DataRow row = tmpDataTable.NewRow();
//                    row[FIELD_EMP_NO] = empInfo.EmpNo;

//                    if (IsIncludeCurrentPositionInfo)
//                    {
//                        ImportEmpPersonalInfoProcess.AddEmployeeInfo(dbConn, row, empInfo.EmpID);
//                        ImportEmpPositionInfoProcess.AddEmployeePositionInfo(dbConn, row, empInfo.EmpID);
//                    }

//                    tmpDataTable.Rows.Add(row);
//                }

//            }
//            if (IsIncludeCurrentPositionInfo)
//                ImportEmpPositionInfoProcess.RetriveHierarchyLevelHeader(dbConn, tmpDataTable);
//            return tmpDataTable;
//        }

//    }
//}