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

    /// <summary>
    /// Summary description for ImportCND
    /// </summary>
    public class ImportPayScaleSalaryIncrement : ImportProcessInteface
    {
        public const decimal ALLOWABLE_POINT_CHANGE_IN_BACKPAY = 1;
        public const string FIELD_EMP_NO = "EmpNo";
        public const string FIELD_ENG_SURNAME = "Surname";
        public const string FIELD_ENG_OTHERNAME = "OtherName";
        public const string FIELD_CHI_NAME = "ChineseName";
        public const string FIELD_COMPANY = "Company";
        public const string FIELD_AS_AT_DATE = "AsAtDate";
        public const string FIELD_SCHEME_CODE = "SchemeCode";
        public const string FIELD_CAPACITY = "Capacity";
        public const string FIELD_MIN_POINT = "MinPoint";
        public const string FIELD_MAX_POINT = "MaxPoint";
        public const string FIELD_CURRENT_POINT = "CurrentPoint";
        public const string FIELD_NEW_POINT = "NewPoint";

        protected int m_processID = -1;
        protected int m_UserID;
        protected DateTime UploadDateTime = AppUtils.ServerDateTime();
        protected DBManager tempDB = ESalaryIncrementBatchDetail.db;
               
        protected ArrayList m_validatorList = new ArrayList();
        
        //m_validatorList.add(new ImportFieldValidator(FIELD_EMP_NO, ImportErrorMessage.ERROR_INVALID_EMP_NO, ImportFieldValidator.CheckOptions.Required));

        public ImportErrorList errors = new ImportErrorList();

        public ImportPayScaleSalaryIncrement(DatabaseConnection dbConn, string SessionID, int UserID)
            : base(dbConn, SessionID)
        {
            m_UserID = UserID;
            init();
        }

        public ImportPayScaleSalaryIncrement(DatabaseConnection dbConn, string SessionID, int UserID, int ProcessID)
            : base(dbConn, SessionID)
        {
            m_UserID = UserID;
            m_processID = ProcessID;
            init();
        }

        protected void init()
        {
            m_validatorList.Add(new ImportFieldValidator(FIELD_EMP_NO, ImportErrorMessages.ERROR_INVALID_FIELD_VALUE, ImportFieldValidator.EnumCheckOptions.Required));

            //m_validatorList.Add(new ImportFieldValidator(FIELD_ENG_SURNAME, ImportErrorMessages.ERROR_INVALID_FIELD_VALUE, ImportFieldValidator.EnumCheckOptions.Required));
            //m_validatorList.Add(new ImportFieldValidator(FIELD_CHI_NAME, ImportErrorMessages.ERROR_INVALID_FIELD_VALUE, ImportFieldValidator.EnumCheckOptions.Required));
            //m_validatorList.Add(new ImportFieldValidator(FIELD_COMPANY, ImportErrorMessages.ERROR_INVALID_FIELD_VALUE, ImportFieldValidator.EnumCheckOptions.Required));

            //m_validatorList.Add(new ImportFieldValidator(FIELD_AS_AT_DATE, ImportErrorMessages.ERROR_INVALID_FIELD_VALUE, 
            //                                             ImportFieldValidator.EnumCheckOptions.Required|
            //                                             ImportFieldValidator.EnumCheckOptions.Date));

            m_validatorList.Add(new ImportFieldValidator(FIELD_SCHEME_CODE, ImportErrorMessages.ERROR_INVALID_FIELD_VALUE, ImportFieldValidator.EnumCheckOptions.Required));

            m_validatorList.Add(new ImportFieldValidator(FIELD_CAPACITY, ImportErrorMessages.ERROR_INVALID_FIELD_VALUE, ImportFieldValidator.EnumCheckOptions.Required));

            //m_validatorList.Add(new ImportFieldValidator(FIELD_MIN_POINT, ImportErrorMessages.ERROR_INVALID_FIELD_VALUE,
            //                                             ImportFieldValidator.EnumCheckOptions.Required |
            //                                             ImportFieldValidator.EnumCheckOptions.Decimal));

            //m_validatorList.Add(new ImportFieldValidator(FIELD_MAX_POINT, ImportErrorMessages.ERROR_INVALID_FIELD_VALUE,
            //                                             ImportFieldValidator.EnumCheckOptions.Required |
            //                                             ImportFieldValidator.EnumCheckOptions.Decimal));

            m_validatorList.Add(new ImportFieldValidator(FIELD_CURRENT_POINT, ImportErrorMessages.ERROR_INVALID_FIELD_VALUE, 
                                                         ImportFieldValidator.EnumCheckOptions.Required |
                                                         ImportFieldValidator.EnumCheckOptions.Decimal));

            m_validatorList.Add(new ImportFieldValidator(FIELD_NEW_POINT, ImportErrorMessages.ERROR_INVALID_FIELD_VALUE, 
                                                         ImportFieldValidator.EnumCheckOptions.Required |
                                                         ImportFieldValidator.EnumCheckOptions.Decimal));
        }

        protected bool ValidateBusinessRule(ESalaryIncrementBatchDetail inputObject, out string errorMessage)
        {

            // validate if new point is .5 or .0

            if (inputObject.NewPoint % Decimal.One != (decimal)0.5 && inputObject.NewPoint % Decimal.One != Decimal.Zero) 
            {
                errorMessage = String.Format("Point({0}) must be a mupltiple of 0.5.", new string[] { inputObject.NewPoint.ToString("0.00") });
                return false;
            }

            if (inputObject.CurrentPoint + ALLOWABLE_POINT_CHANGE_IN_BACKPAY < inputObject.NewPoint || inputObject.CurrentPoint - ALLOWABLE_POINT_CHANGE_IN_BACKPAY > inputObject.NewPoint)
            {
                errorMessage = String.Format("Point change violation. New Point ({0}) is cannot be 0.5 point more / less than the Original Point({1})", new string[] { inputObject.NewPoint.ToString("0.00"), inputObject.CurrentPoint.ToString("0.00") });
                return false;
            }

            if (inputObject.SchemeCode != "" && inputObject.Capacity != "" && inputObject.CurrentPoint >= 0)
            {
                DBFilter m_recurringFilter = new DBFilter();

                m_recurringFilter.add(new NullTerm("EmpRPEffTo"));
                m_recurringFilter.add(new Match("EmpID", inputObject.EmpID));

                string m_schemeCode = AppUtils.Encode(EEmpRecurringPayment.db.getField("SchemeCode"), inputObject.SchemeCode);
                string m_capacity = AppUtils.Encode(EEmpRecurringPayment.db.getField("Capacity"), inputObject.Capacity);

                m_recurringFilter.add(new Match("SchemeCode", m_schemeCode));
                m_recurringFilter.add(new Match("Capacity", m_capacity));
                m_recurringFilter.add(new Match("Point", inputObject.CurrentPoint));

                DBFilter m_paymentTypeFilter = new DBFilter();
                m_paymentTypeFilter.add(new Match("PaymentTypeCode", "BASICSAL"));

                DBFilter m_paymentCodeFilter = new DBFilter();
                m_paymentCodeFilter.add(new IN("PaymentTypeID", "SELECT PaymentTypeID FROM PaymentType", m_paymentTypeFilter));

                m_recurringFilter.add(new IN("PayCodeID", "SELECT PaymentCodeID FROM PaymentCode", m_paymentCodeFilter));
                m_recurringFilter.add("EmpRPID", false);

                ArrayList m_recurringList = EEmpRecurringPayment.db.select(dbConn, m_recurringFilter);

                if (m_recurringList.Count > 0)  // recurring payment matched
                {
                    EEmpRecurringPayment m_recurringPayment = (EEmpRecurringPayment) m_recurringList[0];

                    // cross check if payscale is out of range
                    DBFilter m_payScaleFilter = new DBFilter();
                    m_payScaleFilter.add(new Match("SchemeCode", m_schemeCode));
                    m_payScaleFilter.add(new Match("Capacity", m_capacity));

                    ArrayList m_payScaleList = EPayScale.db.select(dbConn, m_payScaleFilter);
                    if (m_payScaleList.Count > 0)
                    {
                        EPayScale m_payScale = (EPayScale)m_payScaleList[0];
                        if (inputObject.NewPoint < m_payScale.FirstPoint || inputObject.NewPoint > m_payScale.LastPoint)
                        {
                            errorMessage = string.Format("New Point({0}) is out of range({1}-{2}) according to PayScale Setup.", new string[] {inputObject.NewPoint.ToString("0.00"), m_payScale.FirstPoint.ToString("0.00"), m_payScale.LastPoint.ToString("0.00")});
                            return false;
                        }
                    }

                }else
                {
                    errorMessage = string.Format("Recurring Payment not matched. (Scheme Code={0}, Capacity={1}, Point={2})", new string[] {inputObject.SchemeCode, inputObject.Capacity, inputObject.CurrentPoint.ToString("0.00")});
                    return false;
                }
            }
            else
            {
                errorMessage = string.Format("Recurring Payment not matched. (Scheme Code={0}, Capacity={1}, Point={2})", new string[] {inputObject.SchemeCode, inputObject.Capacity, inputObject.CurrentPoint.ToString("0.00")});
                return false;
            }
            errorMessage = "";
            return true;
        }
        
        protected bool ValidateInputData(DataRow row, out string errorMessage, out string colName)
        {
            foreach (ImportFieldValidator v in m_validatorList)
            {
                if (!v.validate(row, out errorMessage, out colName))
                    return false;
            }

            //string EmpNo = m_row[FIELD_EMP_NO].ToString().Trim();         
            
            //int m_empID = HROne.Import.Parse.GetEmpID(dbConn, EmpNo, UserID);
            //if (m_empID < 0)
            //    m_errors.addError(ImportErrorMessage.ERROR_ACCESS_DENIED_EMP_NO, new string[] { EmpNo, rowCount.ToString() });

            errorMessage = "";
            colName = "";
            return true;
        }

        protected ESalaryIncrementBatchDetail Transform(DataRow row, int UserID)
        {
            ESalaryIncrementBatchDetail m_new = new ESalaryIncrementBatchDetail();

            string m_EmpNo = row[FIELD_EMP_NO].ToString().Trim();
            m_new.EmpID = HROne.Import.Parse.GetEmpID(dbConn, m_EmpNo, UserID);
            m_new.SchemeCode = row[FIELD_SCHEME_CODE].ToString().Trim();
            m_new.Capacity = row[FIELD_CAPACITY].ToString().Trim();
            m_new.CurrentPoint = Decimal.Parse(row[FIELD_CURRENT_POINT].ToString());
            m_new.NewPoint = Decimal.Parse(row[FIELD_NEW_POINT].ToString());

            return m_new;
        }

        public override DataTable UploadToTempDatabase(string Filename, int UserID, string ZipPassword)
        {
            ESalaryIncrementBatch m_processHeader = ESalaryIncrementBatch.GetObject(dbConn, m_processID);

            if (m_processHeader == null)
            {
                errors.addError("Salary Increment Batch ID is not initialized!", new string[]{""});
                return null;
            }

            DataTable rawDataTable = HROne.Import.ExcelImport.parse(Filename, ZipPassword).Tables[0];
            ArrayList results = new ArrayList();
            int rowCount = 0;
            string m_errorMessage;
            string m_colName;

            foreach (DataRow row in rawDataTable.Rows)
            {
                rowCount++;
                if (!ValidateInputData(row, out m_errorMessage, out m_colName))
                {
                    errors.addError(m_errorMessage, new string[] {rowCount.ToString("0"), m_colName });
                }
                else
                {
                    ESalaryIncrementBatchDetail m_upload = Transform(row, UserID);  // m_util.Transform(row);
                    m_upload.BatchID = m_processID;

                    // check batch detail existenc
                    DBFilter m_existingRecordFilter = new DBFilter();
                    m_existingRecordFilter.add(new Match("BatchID", m_processID));
                    m_existingRecordFilter.add(new Match("EmpID", m_upload.EmpID));
                    m_existingRecordFilter.add(new Match("SchemeCode", AppUtils.Encode(ESalaryIncrementBatchDetail.db.getField("SchemeCode"), m_upload.SchemeCode)));
                    m_existingRecordFilter.add(new Match("Capacity", AppUtils.Encode(ESalaryIncrementBatchDetail.db.getField("Capacity"), m_upload.Capacity)));
                    m_existingRecordFilter.add(new Match("CurrentPoint", m_upload.CurrentPoint));

                    ArrayList m_existingRecordList = ESalaryIncrementBatchDetail.db.select(dbConn, m_existingRecordFilter);

                    if (m_existingRecordList.Count > 0)
                    {
                        if (!ValidateBusinessRule(m_upload, out m_errorMessage))
                        {
                            errors.addError(m_errorMessage + "(row#={0})", new string[] {rowCount.ToString("0") });
                        }
                        else
                        {
                            ESalaryIncrementBatchDetail m_currentRecord = (ESalaryIncrementBatchDetail)m_existingRecordList[0];

                            m_currentRecord.NewPoint = m_upload.NewPoint;

                            ESalaryIncrementBatchDetail.db.update(dbConn, m_currentRecord);
                        }
                    }
                    else
                    {
                        errors.addError("Cannot find record from batch. (Row# : {0})", new string[] { rowCount.ToString("0") });
                    }
                }
            }
            if (errors.List.Count <= 0)
            {
                // update process header
                m_processHeader.Status = ESalaryIncrementBatch.STATUS_OPEN;
                m_processHeader.UploadDateTime = AppUtils.ServerDateTime();
                m_processHeader.UploadBy = UserID;
                ESalaryIncrementBatch.db.update(dbConn, m_processHeader);
            }

            return rawDataTable;
        }

        public override DataTable GetImportDataFromTempDatabase(ListInfo info)
        {
        //    DBFilter sessionFilter = new DBFilter();
        //    sessionFilter.add(new Match("SessionID", m_SessionID));
        //    sessionFilter.add(new MatchField("c.EmpID", "e.EmpID"));
        //    //if (info != null && info.orderby != null && !info.orderby.Equals(""))
        //    //    sessionFilter.add(info.orderby, info.order);

        //    return sessionFilter.loadData(dbConn, null, "e.*, c.* ", " from " + tempDB.dbclass.tableName + " c, " + EEmpPersonalInfo.db.dbclass.tableName + " e");
            return null;
        }

        //public static void ClearTempTable(DatabaseConnection dbConn, string sessionID)
        //{
        //    //ImportEmpWorkingSummaryProcess import = new ImportEmpWorkingSummaryProcess(dbConn, sessionID, 0);
        //    //import.ClearTempTable();
        //}

        public override void ClearTempTable()
        {
            ////  Clear Old Import Session
            //DBFilter sessionFilter = new DBFilter();
            //if (!string.IsNullOrEmpty(m_SessionID))
            //    sessionFilter.add(new Match("SessionID", m_SessionID));
            //tempDB.delete(dbConn, sessionFilter);
        }

        public override void ImportToDatabase()
        {
            //DataTable dataTable = GetImportDataFromTempDatabase(null);
            //if (dataTable.Rows.Count > 0)
            //{

            //    foreach (DataRow row in dataTable.Rows)
            //    {
            //        EUploadBackpay uploadObj = new EUploadBackpay();
            //        tempDB.toObject(row, uploadObj);

            //        EBackpay obj;
            //        DBFilter dbFilter = new DBFilter();
            //        dbFilter.add(new Match("EmpID", uploadObj.EmpID));
            //        dbFilter.add(new Match("PaymentCodeID", uploadObj.PaymentCodeID));
            //        dbFilter.add(new Match("AnnounceDate", uploadObj.EffectiveDate));
            //        dbFilter.add(new Match("EffectiveDate", uploadObj.EffectiveDate));
            //        ArrayList backpayList = EBackpay.db.select(dbConn, dbFilter);
            //        if (backpayList.Count > 0)
            //            obj = (EBackpay)backpayList[0];
            //        else
            //            obj = new EBackpay();

            //        obj.EmpID = uploadObj.EmpID;
            //        obj.PaymentCodeID = uploadObj.PaymentCodeID;
            //        obj.AnnounceDate = uploadObj.AnnounceDate;
            //        obj.EffectiveDate = uploadObj.EffectiveDate;
            //        obj.OriginalPoint = uploadObj.OriginalPoint;
            //        obj.NewPoint = uploadObj.NewPoint;

            //        if (obj.BackpayID > 0)
            //            EBackpay.db.update(dbConn, obj);
            //        else
            //            EBackpay.db.insert(dbConn, obj);

            //        tempDB.delete(dbConn, uploadObj);

            //    }
            //}
        }

        public static DataTable Export(DatabaseConnection dbConn, ArrayList EmpInfoList, DateTime AnnounceDate, DateTime EffectiveDate, string PaymentCode)
        {
            //DataTable tmpDataTable = new DataTable("Backpay$");

            //tmpDataTable.Columns.Add(FIELD_EMP_NO, typeof(string));
            //tmpDataTable.Columns.Add("EnglishName", typeof(string));
            //tmpDataTable.Columns.Add("ChineseName", typeof(string));

            //tmpDataTable.Columns.Add("Company", typeof(string));
            //tmpDataTable.Columns.Add("Rank", typeof(string));
            //tmpDataTable.Columns.Add(FIELD_ANNOUNCE_DATE, typeof(DateTime));
            //tmpDataTable.Columns.Add(FIELD_EFFECTIVE_DATE, typeof(DateTime));

            //tmpDataTable.Columns.Add(FIELD_PAYMENT_CODE, typeof(string));

            //tmpDataTable.Columns.Add(FIELD_ORIG_POINT, typeof(decimal));
            //tmpDataTable.Columns.Add(FIELD_NEW_POINT, typeof(decimal));

            //foreach (EEmpPersonalInfo empInfo in EmpInfoList)
            //{
            //    if (EEmpPersonalInfo.db.select(dbConn, empInfo))
            //    {

            //        DBFilter empPosFilter = new DBFilter();
            //        empPosFilter.add(new Match("EmpID", empInfo.EmpID));                    
            //        empPosFilter.add(new Match("EmpPosEffFr", "<=", EffectiveDate));
            //        empPosFilter.add(new NullTerm("EmpPosEffTo"));
            //        empPosFilter.add("EmpPosEffFr", true);

            //        ArrayList empPosList = EEmpPositionInfo.db.select(dbConn, empPosFilter);

            //        DataRow newRow = null;
            //        EEmpPositionInfo empPos =null;
            //        ECompany company = null;
            //        EPayScale payScale = null; 
            //        EBackpay backpay = null;
            //        EUploadBackpay uploadedBackpay = null; 

            //        if (empPosList.Count > 0)
            //        {
            //            empPos = (EEmpPositionInfo) empPosList[0];

            //            if (empPos.PayScaleID <= 0)
            //            {
            //                company.CompanyID = empPos.CompanyID;
            //                ECompany.GetObject(dbConn, company);

            //                payScale.PayScaleID = empPos.PayScaleID;
            //                EPayScale.GetObject(dbConn, payScale);

            //                newRow = tmpDataTable.NewRow();
            //                newRow[FIELD_EMP_NO] = empInfo.EmpNo;
            //                newRow["EnglishName"] = empInfo.EmpEngFullName;
            //                newRow["ChineseName"] = empInfo.EmpChiFullName;
            //                newRow["Company"] = company.CompanyName;
            //                newRow["Rank"] = payScale.PayScaleDesc;
            //                newRow[FIELD_ANNOUNCE_DATE] = AnnounceDate;
            //                newRow[FIELD_EFFECTIVE_DATE] = EffectiveDate;
            //                newRow[FIELD_PAYMENT_CODE] = PaymentCode;
            //                newRow[FIELD_ORIG_POINT] = empPos.PayScalePoints;
            //                newRow[FIELD_NEW_POINT] = null;

            //                DBFilter existingBackpayFilter = new DBFilter();
            //                existingBackpayFilter.add(new Match("EmpID", empPosFilter));
            //                existingBackpayFilter.add(new Match("AnnounceDate", AnnounceDate));
            //                existingBackpayFilter.add(new Match("EffectiveDate", EffectiveDate));
            //                //TODO confirm if PaymentCodeID included in uniqueness checking
            //                //existingBackpayFilter.add(new Match("PaymentCodeID", 
                            
            //                ArrayList uploadedBackpayList = EBackpayProcessDetail.db.select(dbConn, existingBackpayFilter);
            //                if (uploadedBackpayList.Count > 0)
            //                {
            //                    uploadedBackpay = (EUploadBackpay) uploadedBackpayList[0];

            //                    if (uploadedBackpay.OriginalPoint == empPos.PayScalePoints)
            //                    {
            //                        newRow[FIELD_NEW_POINT] = uploadedBackpay.NewPoint;
            //                    }
            //                }

            //                newRow[FIELD_NEW_POINT] = empPos.PayScalePoints;    // should load previous input if any.

            //                tmpDataTable.Rows.Add(newRow);
            //            }
            //        }
            //    }
            //}
            //return tmpDataTable;
            return null; 
        }
    }
}