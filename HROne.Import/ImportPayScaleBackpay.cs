using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Globalization;
using HROne.Lib.Entities;
using HROne.DataAccess;
using System.Data.SqlClient;
//using perspectivemind.validation;

namespace HROne.Import
{

    /// <summary>
    /// Summary description for ImportCND
    /// </summary>
    public class ImportPayScaleBackpay : ImportProcessInteface
    {
        public const string FIELD_ANNOUNCE_DATE = "AnnounceDate";
        public const string FIELD_EFFECTIVE_DATE = "EffectiveDate";
        public const string FIELD_BACKPAY_DATE = "BackpayDate";
        public const string FIELD_SCHEME_CODE = "SchemeCode";
        public const string FIELD_PAYMENT_CODE = "PaymentCode";
        public const string FIELD_POINT = "Point";
        public const string FIELD_CURRENT_SALARY = "CurrentSalary";
        public const string FIELD_SALARY = "NewSalary";

        protected int m_UserID;
        protected DateTime UploadDateTime = AppUtils.ServerDateTime();
        protected DBManager tempDB = EBackpayBatchDetail.db;

        protected ArrayList m_validatorList = new ArrayList();

        public ImportErrorList errors = new ImportErrorList();

        public ImportPayScaleBackpay(DatabaseConnection dbConn, string SessionID, int UserID)
            : base(dbConn, SessionID)
        {
            m_UserID = UserID;
            init();
        }

        protected void init()
        {
            m_validatorList.Add(new ImportFieldValidator(FIELD_ANNOUNCE_DATE, ImportErrorMessages.ERROR_INVALID_FIELD_VALUE, 
                                                         ImportFieldValidator.EnumCheckOptions.Required | 
                                                         ImportFieldValidator.EnumCheckOptions.Date));

            m_validatorList.Add(new ImportFieldValidator(FIELD_EFFECTIVE_DATE, ImportErrorMessages.ERROR_INVALID_FIELD_VALUE,
                                                         ImportFieldValidator.EnumCheckOptions.Required |
                                                         ImportFieldValidator.EnumCheckOptions.Date));

            m_validatorList.Add(new ImportFieldValidator(FIELD_BACKPAY_DATE, ImportErrorMessages.ERROR_INVALID_FIELD_VALUE,
                                                         ImportFieldValidator.EnumCheckOptions.Required |
                                                         ImportFieldValidator.EnumCheckOptions.Date));

            m_validatorList.Add(new ImportFieldValidator(FIELD_SCHEME_CODE, ImportErrorMessages.ERROR_INVALID_FIELD_VALUE,
                                                         ImportFieldValidator.EnumCheckOptions.Required));

            m_validatorList.Add(new ImportFieldValidator(FIELD_PAYMENT_CODE, ImportErrorMessages.ERROR_INVALID_FIELD_VALUE,
                                                         ImportFieldValidator.EnumCheckOptions.Required));

            m_validatorList.Add(new ImportFieldValidator(FIELD_POINT, ImportErrorMessages.ERROR_INVALID_FIELD_VALUE,
                                                         ImportFieldValidator.EnumCheckOptions.Required |
                                                         ImportFieldValidator.EnumCheckOptions.Numeric));

            m_validatorList.Add(new ImportFieldValidator(FIELD_CURRENT_SALARY, ImportErrorMessages.ERROR_INVALID_FIELD_VALUE,
                                                         ImportFieldValidator.EnumCheckOptions.Required |
                                                         ImportFieldValidator.EnumCheckOptions.Decimal));

            m_validatorList.Add(new ImportFieldValidator(FIELD_SALARY, ImportErrorMessages.ERROR_INVALID_FIELD_VALUE,
                                                         ImportFieldValidator.EnumCheckOptions.Required |
                                                         ImportFieldValidator.EnumCheckOptions.Decimal));
        }

        protected bool ValidateBusinessRule(EBackpayBatchDetail inputObject, out string errorMessage)
        {
            if (inputObject.Point < 0)
            {
                errorMessage = String.Format("Point must be not be a negative number (Point = {0})", new string[] { inputObject.Point.ToString("0") });
                return false;
            }
            if (inputObject.Salary < 0)
            {
                errorMessage = string.Format("Salary must be a positive value (Salary = {0})", new string[] { inputObject.Salary.ToString("0.00") });
                return false;
            }
            if (HROne.Import.Parse.GetPaymentCodeID(dbConn, inputObject.PaymentCode) <= 0)
            {
                errorMessage = string.Format("Invalid Payment Code ({0}).", new string[] { inputObject.PaymentCode });
                return false;
            }
            if (inputObject.BackpayDate.CompareTo(inputObject.EffectiveDate) <= 0)
            {
                errorMessage = string.Format("Backpay Date({0}) must be after Effective Date({1}).", inputObject.BackpayDate.ToString("yyyy-MM-dd"), inputObject.EffectiveDate.ToString("yyyy-MM-dd"));
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

            errorMessage = "";
            colName = "";
            return true;
        }

        protected EBackpayBatchDetail Transform(DataRow row, int UserID)
        {
            EBackpayBatchDetail m_new = new EBackpayBatchDetail();

            m_new.AnnounceDate = HROne.Import.Parse.toDateTimeObject(row[FIELD_ANNOUNCE_DATE]);
            m_new.EffectiveDate = HROne.Import.Parse.toDateTimeObject(row[FIELD_EFFECTIVE_DATE]);
            m_new.BackpayDate = HROne.Import.Parse.toDateTimeObject(row[FIELD_BACKPAY_DATE]);
            m_new.SchemeCode = row[FIELD_SCHEME_CODE].ToString().Trim();
            m_new.Point = HROne.Import.Parse.toInteger(row[FIELD_POINT]);
            m_new.CurrentSalary = HROne.Import.Parse.toDecimal(row[FIELD_CURRENT_SALARY]);
            m_new.Salary = HROne.Import.Parse.toDecimal(row[FIELD_SALARY]);
            m_new.PaymentCode = row[FIELD_PAYMENT_CODE].ToString();

            return m_new;
        }

        public override DataTable UploadToTempDatabase(string Filename, int UserID, string ZipPassword)
        {
            DataTable rawDataTable = HROne.Import.ExcelImport.parse(Filename, ZipPassword).Tables[0];
            ArrayList results = new ArrayList();
            int rowCount = 0;
            string m_errorMessage;
            string m_colName;

            try 
            {
                dbConn.BeginTransaction();

                foreach (DataRow row in rawDataTable.Rows)
                {
                    rowCount++;
                    if (!ValidateInputData(row, out m_errorMessage, out m_colName))
                    {
                        errors.addError(m_errorMessage, new string[] { rowCount.ToString("0"), m_colName });
                    }
                    else
                    {
                        EBackpayBatchDetail m_upload = Transform(row, UserID);  // m_util.Transform(row);

                        if (!ValidateBusinessRule(m_upload, out m_errorMessage))
                        {
                            errors.addError(m_errorMessage + " (row#={0})", new string[] { rowCount.ToString("0") });
                        }
                        else
                        {
                            // update payscalemap
                            DBFilter m_existingRecordFilter = new DBFilter();
                            m_existingRecordFilter.add(new Match("SchemeCode", AppUtils.Encode(EBackpayBatchDetail.db.getField("SchemeCode"), m_upload.SchemeCode)));
                            m_existingRecordFilter.add(new NullTerm("ExpiryDate"));
                            m_existingRecordFilter.add(new Match("Point", m_upload.Point));

                            ArrayList m_existingRecordList = EPayScaleMap.db.select(dbConn, m_existingRecordFilter);

                            EPayScaleMap m_newMap = new EPayScaleMap();
                            m_newMap.SchemeCode = m_upload.SchemeCode;
                            m_newMap.Point = m_upload.Point;
                            m_newMap.Salary = m_upload.Salary;
                            m_newMap.EffectiveDate = m_upload.EffectiveDate;

                            if (m_existingRecordList.Count > 0)
                            {
                                EPayScaleMap m_currentMap = (EPayScaleMap)m_existingRecordList[0];

                                if (m_upload.EffectiveDate < m_currentMap.EffectiveDate)
                                {
                                    errors.addError("Invalid Effective Date({0}).  It must be before current effective date({1}).",
                                                    new string[] { m_upload.EffectiveDate.ToString("yyyy-MM-dd"), m_currentMap.EffectiveDate.ToString("yyyy-MM-dd") });
                                    break;
                                }
                                else
                                {
                                    // expire current map
                                    m_currentMap.ExpiryDate = m_upload.EffectiveDate.AddDays(-1);
                                    EPayScaleMap.db.update(dbConn, m_currentMap);
                                    // insert new salary

                                    EPayScaleMap.db.insert(dbConn, m_newMap);
                                    EBackpayBatchDetail.db.insert(dbConn, m_upload);
                                }

                            }
                            else
                            {
                                EPayScaleMap.db.insert(dbConn, m_newMap);
                                EBackpayBatchDetail.db.insert(dbConn, m_upload);
                            }

                            // go through recurring payment
                            DBFilter m_recurringPaymentFilter = new DBFilter();
                            DBFilter m_paymentTypeFilter = new DBFilter();
                            DBFilter m_paymentCodeFilter = new DBFilter();
                            m_paymentTypeFilter.add(new Match("PaymentTypeCode", "BASICSAL"));
                            m_paymentCodeFilter.add(new IN("PaymentTypeID", "SELECT PaymentTypeID FROM PaymentType", m_paymentTypeFilter));
                            m_recurringPaymentFilter.add(new IN("PayCodeID", "SELECT PaymentCodeID FROM PaymentCode", m_paymentCodeFilter));
                            m_recurringPaymentFilter.add(new Match("SchemeCode", AppUtils.Encode(EEmpRecurringPayment.db.getField("SchemeCode"), m_upload.SchemeCode)));
                            m_recurringPaymentFilter.add(new Match("Point", m_upload.Point));
                            m_recurringPaymentFilter.add(new Match("EmpRpEffFr", "<=", m_upload.EffectiveDate));
                            OR m_orDate = new OR();
                            m_orDate.add(new NullTerm("EmpRpEffTo"));
                            m_orDate.add(new Match("EmpRPEffTo", ">=", m_upload.EffectiveDate));
                            m_recurringPaymentFilter.add(m_orDate);
                            m_recurringPaymentFilter.add("EmpID", true);
                            m_recurringPaymentFilter.add("EmpRpEffFr", true);

                            foreach (EEmpRecurringPayment m_rp in EEmpRecurringPayment.db.select(dbConn, m_recurringPaymentFilter))
                            {
                                // find payment records of each recurring payment
                                DBFilter m_PaymentRecordFilter = new DBFilter();
                                DBFilter m_EmpPayrollFilter = new DBFilter();
                                DBFilter m_PayrollPeriodFilter = new DBFilter();

                                m_PayrollPeriodFilter.add(new Match("PayPeriodFr", ">=", m_upload.EffectiveDate));

                                m_EmpPayrollFilter.add(new Match("EmpID", m_rp.EmpID));
                                m_EmpPayrollFilter.add(new IN("PayPeriodID", "SELECT PayPeriodID FROM PayrollPeriod", m_PayrollPeriodFilter));

                                m_PaymentRecordFilter.add(new Match("PaymentCodeID", m_rp.PayCodeID));
                                m_PaymentRecordFilter.add(new IN("EmpPayrollID", "SELECT EmpPayrollID FROM EmpPayroll", m_EmpPayrollFilter));

                                string m_remarks = "";
                                double m_amount = 0;

                                foreach (EPaymentRecord m_payRecords in EPaymentRecord.db.select(dbConn, m_PaymentRecordFilter))
                                {
                                    if (m_remarks != "")
                                        m_remarks = m_remarks + " + ";

                                    m_remarks = ((m_payRecords.PayRecActAmount / m_rp.EmpRPAmount) * (System.Convert.ToDouble(m_upload.Salary) - m_rp.EmpRPAmount)).ToString("0.00");

                                    m_amount = ((m_payRecords.PayRecActAmount / m_rp.EmpRPAmount) * (System.Convert.ToDouble(m_upload.Salary) - m_rp.EmpRPAmount));
                                }


                                // insert to claims & deduction
                                EClaimsAndDeductions m_cnd = new EClaimsAndDeductions();
                                m_cnd.CNDEffDate = m_upload.BackpayDate;
                                m_cnd.CNDAmount = m_amount;
                                m_cnd.EmpID = m_rp.EmpID;
                                m_cnd.CNDRemark = m_remarks;
                                m_cnd.EmpAccID = m_rp.EmpAccID;
                                m_cnd.CNDPayMethod = m_rp.EmpRPMethod;
                                m_cnd.CostCenterID = m_rp.CostCenterID;
                                m_cnd.CurrencyID = m_rp.CurrencyID;
                                m_cnd.PayCodeID = HROne.Import.Parse.GetPaymentCodeID(dbConn, m_upload.PaymentCode);

                                EClaimsAndDeductions.db.insert(dbConn, m_cnd);
                            }
                        }
                    }
                }
                dbConn.CommitTransaction();            
            }
            catch (SqlException sqlEx)
            {
                errors.addError(sqlEx.Message, new string[] { });
                dbConn.RollbackTransaction();
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