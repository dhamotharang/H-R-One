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
    /// Summary description for ImportEmpRecurringPaymentProcess
    /// </summary>
    public class ImportEmpRecurringPaymentProcess : ImportProcessInteface
    {

        [DBClass("UploadEmpRecurringPayment")]
        protected class EUploadEmpRecurringPayment : ImportDBObject
        {
            public static DBManager db = new DBManager(typeof(EUploadEmpRecurringPayment));

            protected int m_UploadEmpRPID;
            [DBField("UploadEmpRPID", true, true), TextSearch, Export(false)]
            public int UploadEmpRPID
            {
                get { return m_UploadEmpRPID; }
                set { m_UploadEmpRPID = value; modify("UploadEmpRPID"); }
            }
            protected int m_UploadEmpID;
            [DBField("UploadEmpID"), TextSearch, Export(false)]
            public int UploadEmpID
            {
                get { return m_UploadEmpID; }
                set { m_UploadEmpID = value; modify("UploadEmpID"); }
            }
            protected int m_UploadEmpAccID;
            [DBField("UploadEmpAccID"), TextSearch, Export(false)]
            public int UploadEmpAccID
            {
                get { return m_UploadEmpAccID; }
                set { m_UploadEmpAccID = value; modify("UploadEmpAccID"); }
            }

            protected int m_EmpRPID;
            [DBField("EmpRPID"), TextSearch, Export(false)]
            public int EmpRPID
            {
                get { return m_EmpRPID; }
                set { m_EmpRPID = value; modify("EmpRPID"); }
            }
            protected int m_EmpID;
            [DBField("EmpID"), TextSearch, Export(false)]
            public int EmpID
            {
                get { return m_EmpID; }
                set { m_EmpID = value; modify("EmpID"); }
            }
            protected DateTime m_EmpRPEffFr;
            [DBField("EmpRPEffFr"), TextSearch, MaxLength(25), Export(false), Required]
            public DateTime EmpRPEffFr
            {
                get { return m_EmpRPEffFr; }
                set { m_EmpRPEffFr = value; modify("EmpRPEffFr"); }
            }
            protected DateTime m_EmpRPEffTo;
            [DBField("EmpRPEffTo"), TextSearch, MaxLength(25), Export(false)]
            public DateTime EmpRPEffTo
            {
                get { return m_EmpRPEffTo; }
                set { m_EmpRPEffTo = value; modify("EmpRPEffTo"); }
            }
            protected int m_PayCodeID;
            [DBField("PayCodeID"), TextSearch, Export(false), Required]
            public int PayCodeID
            {
                get { return m_PayCodeID; }
                set { m_PayCodeID = value; modify("PayCodeID"); }
            }

            protected double m_EmpRPBasicSalary;
            [DBField("EmpRPBasicSalary", "0.00"), TextSearch, MaxLength(11), Export(false)]
            public double EmpRPBasicSalary
            {
                get { return m_EmpRPBasicSalary; }
                set { m_EmpRPBasicSalary = value; modify("EmpRPBasicSalary"); }
            }

            protected double m_EmpRPFPS;
            [DBField("EmpRPFPS", "0.00"), TextSearch, MaxLength(6), Export(false)]
            public double EmpRPFPS
            {
                get { return m_EmpRPFPS; }
                set { m_EmpRPFPS = value; modify("EmpRPFPS"); }
            }

            protected string m_SchemeCode;
            [DBField("SchemeCode"), TextSearch, Export(false)]
            public string SchemeCode
            {
                get { return m_SchemeCode; }
                set { m_SchemeCode = value; modify("SchemeCode"); }
            }
            protected string m_Capacity;
            [DBField("Capacity"), TextSearch, Export(false)]
            public string Capacity
            {
                get { return m_Capacity; }
                set { m_Capacity = value; modify("Capacity"); }
            }
            protected decimal m_Point;
            [DBField("Point", "0.00"), TextSearch, MaxLength(5), Export(false)]
            public decimal Point
            {
                get { return m_Point; }
                set { m_Point = value; modify("Point"); }
            }
            protected double m_EmpRPAmount;
            [DBField("EmpRPAmount", "0.00"), TextSearch, MaxLength(25), Export(false), Required]
            public double EmpRPAmount
            {
                get { return m_EmpRPAmount; }
                set { m_EmpRPAmount = value; modify("EmpRPAmount"); }
            }

            protected string m_CurrencyID;
            [DBField("CurrencyID"), TextSearch, Export(false)]
            public string CurrencyID
            {
                get { return m_CurrencyID; }
                set { m_CurrencyID = value; modify("CurrencyID"); }
            }
            protected string m_EmpRPUnit;
            [DBField("EmpRPUnit"), TextSearch, Export(false), Required]
            public string EmpRPUnit
            {
                get { return m_EmpRPUnit; }
                set { m_EmpRPUnit = value; modify("EmpRPUnit"); }
            }
            protected string m_EmpRPMethod;
            [DBField("EmpRPMethod"), TextSearch, Export(false), Required]
            public string EmpRPMethod
            {
                get { return m_EmpRPMethod; }
                set { m_EmpRPMethod = value; modify("EmpRPMethod"); }
            }
            protected int m_EmpAccID;
            [DBField("EmpAccID"), TextSearch, Export(false)]
            public int EmpAccID
            {
                get { return m_EmpAccID; }
                set { m_EmpAccID = value; modify("EmpAccID"); }
            }
            protected bool m_EEmpRPIsNonPayrollItem;
            [DBField("EmpRPIsNonPayrollItem"), TextSearch, Export(false)]
            public bool EmpRPIsNonPayrollItem
            {
                get { return m_EEmpRPIsNonPayrollItem; }
                set { m_EEmpRPIsNonPayrollItem = value; modify("EmpRPIsNonPayrollItem"); }
            }
            protected int m_CostCenterID;
            [DBField("CostCenterID"), TextSearch, Export(false)]
            public int CostCenterID
            {
                get { return m_CostCenterID; }
                set { m_CostCenterID = value; modify("CostCenterID"); }
            }
            protected string m_EmpRPRemark;
            [DBField("EmpRPRemark"), TextSearch, Export(false)]
            public string EmpRPRemark
            {
                get { return m_EmpRPRemark; }
                set { m_EmpRPRemark = value; modify("EmpRPRemark"); }
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

        public const string TABLE_NAME = "recurring_history";

        //  Field Const for Employee Personal Information
        protected const string FIELD_EMP_NO = "Emp No";
        protected const string FIELD_FROM = "From";
        protected const string FIELD_TO = "To";
        protected const string FIELD_PAYMENT_CODE = "Payment Code";
        protected const string FIELD_SCHEME_CODE = "Scheme Code";
        protected const string FIELD_CAPACITY = "Capacity";
        protected const string FIELD_POINT = "Point";
        protected const string FIELD_AMOUNT = "Amount";
        protected const string FIELD_UNIT = "Unit";
        protected const string FIELD_METHOD = "Method";
        protected const string FIELD_ACCOUNT_NO = "Account No";
        protected const string FIELD_IS_NON_PAYROLL_ITEM = "Is Non Payroll Item";
        protected const string FIELD_COST_CENTER = "Cost Center";
        protected const string FIELD_REMARK = "Remark";
        protected const string FIELD_FPS = "Fixed % of Salary";
        // Start 0000121, KuangWei, 2014-11-06
        //protected const string FIELD_BASIC_SALARY = "Basic Salary";
        protected const string FIELD_BASIC_SALARY = "Target Salary";
        // End 0000121, KuangWei, 2014-11-06

        protected DateTime UploadDateTime = AppUtils.ServerDateTime();
        protected DBManager tempDB = EUploadEmpRecurringPayment.db;
        protected DBManager uploadDB = EEmpRecurringPayment.db;

        public ImportErrorList errors = new ImportErrorList();
        

        public ImportEmpRecurringPaymentProcess(DatabaseConnection dbConn, string SessionID): base(dbConn, SessionID)
        {
          
            
        }

        public virtual DataTable UploadToTempDatabase(DataTable rawDataTable, int UserID)
        {
            if (rawDataTable == null)
                return GetImportDataFromTempDatabase(null);

            int rowCount = 1;
            foreach (DataRow row in rawDataTable.Rows)
            {
                if (rawDataTable == null)
                    return GetImportDataFromTempDatabase(null);

                rowCount++;

                EUploadEmpRecurringPayment uploadEmpPay = new EUploadEmpRecurringPayment();
                EEmpRecurringPayment lastEmpPay = null;

                string EmpNo = row[FIELD_EMP_NO].ToString().Trim();
                uploadEmpPay.EmpID = Parse.GetEmpID(dbConn, EmpNo, UserID);
                if (uploadEmpPay.EmpID < 0)
                    errors.addError(ImportErrorMessage.ERROR_ACCESS_DENIED_EMP_NO, new string[] { EmpNo, rowCount.ToString() });
                try
                {
                    uploadEmpPay.EmpRPEffFr = Parse.toDateTimeObject(row[FIELD_FROM]);
                }
                catch
                {
                    errors.addError(ImportErrorMessage.ERROR_INVALID_FIELD_VALUE, new string[] { FIELD_FROM + "=" + row[FIELD_FROM].ToString(), EmpNo, rowCount.ToString() });
                }
                try
                {
                    uploadEmpPay.EmpRPEffTo = Parse.toDateTimeObject(row[FIELD_TO]);
                }
                catch
                {
                    errors.addError(ImportErrorMessage.ERROR_INVALID_FIELD_VALUE, new string[] { FIELD_TO + "=" + row[FIELD_TO].ToString(), EmpNo, rowCount.ToString() });
                }
                uploadEmpPay.PayCodeID = Parse.GetPaymentCodeID(dbConn, row[FIELD_PAYMENT_CODE].ToString());

                if (uploadEmpPay.PayCodeID == 0)
                    errors.addError(ImportErrorMessage.ERROR_INVALID_FIELD_VALUE, new string[] { FIELD_PAYMENT_CODE + "=" + row[FIELD_PAYMENT_CODE].ToString(), EmpNo, rowCount.ToString() });

                double amount = 0;
                if (double.TryParse(row[FIELD_AMOUNT].ToString(), out amount))
                    uploadEmpPay.EmpRPAmount = amount;
                else
                    errors.addError(ImportErrorMessage.ERROR_INVALID_FIELD_VALUE, new string[] { FIELD_AMOUNT + "=" + row[FIELD_AMOUNT].ToString(), EmpNo, rowCount.ToString() });

                if (ESystemParameter.getParameter(dbConn, ESystemParameter.PARAM_CODE_PAYSCALE_POINT_SYSTEM) == "Y")
                {
                    if (amount > 0)
                    {
                        decimal m_point = 0;
                        if (decimal.TryParse(row[FIELD_POINT].ToString(), out m_point))
                        {
                            uploadEmpPay.Point = m_point;
                        }
                        else
                            errors.addError(ImportErrorMessage.ERROR_INVALID_FIELD_VALUE, new string[] { FIELD_POINT + "=" + row[FIELD_POINT].ToString(), EmpNo, rowCount.ToString() });
                    
                        uploadEmpPay.SchemeCode = row[FIELD_SCHEME_CODE].ToString().Trim();
                        if (uploadEmpPay.SchemeCode == "")
                            errors.addError(ImportErrorMessage.ERROR_INVALID_FIELD_VALUE, new string[] { FIELD_SCHEME_CODE + "=" + row[FIELD_SCHEME_CODE].ToString(), EmpNo, rowCount.ToString() });
                     
                        uploadEmpPay.Capacity = row[FIELD_CAPACITY].ToString().Trim();
                        if (uploadEmpPay.Capacity == "")
                            errors.addError(ImportErrorMessage.ERROR_INVALID_FIELD_VALUE, new string[] { FIELD_CAPACITY + "=" + row[FIELD_CAPACITY].ToString(), EmpNo, rowCount.ToString() });
                    }
                }

                if (ESystemParameter.getParameter(dbConn, ESystemParameter.PARAM_CODE_MONTHLY_ACHIEVEMENT_COMMISSION) == "Y")       // for F&V
                {
                    decimal m_fps;
                    decimal m_basicSalary;

                    if (decimal.TryParse(row[FIELD_FPS].ToString(), out m_fps))
                    {
                        uploadEmpPay.EmpRPFPS = System.Convert.ToDouble(m_fps);
                    }
                    else
                    {
                        errors.addError(ImportErrorMessage.ERROR_INVALID_FIELD_VALUE, new string[] { FIELD_FPS + "=" + row[FIELD_FPS].ToString(), EmpNo, rowCount.ToString() });
                    }

                    if (decimal.TryParse(row[FIELD_BASIC_SALARY].ToString(), out m_basicSalary))
                    {
                        uploadEmpPay.EmpRPBasicSalary = System.Convert.ToDouble(m_basicSalary);
                    }
                    else
                    {
                        errors.addError(ImportErrorMessage.ERROR_INVALID_FIELD_VALUE, new string[] { FIELD_BASIC_SALARY + "=" + row[FIELD_BASIC_SALARY].ToString(), EmpNo, rowCount.ToString() });
                    }
                }

                uploadEmpPay.CurrencyID = HROne.Lib.ExchangeCurrency.DefaultCurrency();

                uploadEmpPay.EmpRPUnit = Parse.toRecurringPaymentUnit(row[FIELD_UNIT].ToString());
                uploadEmpPay.EmpRPMethod = Parse.toPaymentMethodCode(row[FIELD_METHOD].ToString());

                string BankCode = row[FIELD_ACCOUNT_NO].ToString();
                uploadEmpPay.EmpAccID = HROne.Import.Parse.GetEmpAccID(dbConn, BankCode, uploadEmpPay.EmpID);

                uploadEmpPay.UploadEmpID = ParseTemp.GetUploadEmpID(dbConn, EmpNo, m_SessionID);
                if (uploadEmpPay.UploadEmpID == 0)
                    if (uploadEmpPay.EmpID == 0)
                        errors.addError(ImportErrorMessage.ERROR_INVALID_EMP_NO, new string[] { EmpNo, rowCount.ToString() });
                    else
                        uploadEmpPay.UploadEmpID = ImportEmpPersonalInfoProcess.CreateDummyUploadEmployeeInfo(dbConn, uploadEmpPay.EmpID, m_SessionID, UploadDateTime);


                //  Check if the bank account no does not exist in database
                if (uploadEmpPay.EmpAccID == 0 && !BankCode.Trim().Equals(string.Empty))
                {
                    uploadEmpPay.UploadEmpAccID = ParseTemp.GetUploadEmpAccID(dbConn, BankCode, uploadEmpPay.UploadEmpID);
                    if (uploadEmpPay.UploadEmpAccID == 0)
                        errors.addError(ImportErrorMessage.ERROR_INVALID_FIELD_VALUE, new string[] { FIELD_ACCOUNT_NO + "=" + BankCode, EmpNo, rowCount.ToString() });
                }
                if ((uploadEmpPay.EmpAccID > 0 || uploadEmpPay.UploadEmpAccID > 0) && !uploadEmpPay.EmpRPMethod.Equals("A"))
                    errors.addError(ImportErrorMessage.ERROR_INVALID_FIELD_VALUE, new string[] { FIELD_METHOD + "=" + row[FIELD_METHOD].ToString(), EmpNo, rowCount.ToString() });

                if (uploadEmpPay.EmpRPMethod.Equals("A") && uploadEmpPay.EmpAccID == 0 && uploadEmpPay.UploadEmpAccID == 0)
                {

                    EUploadEmpBankAccount uploadBankAccount = EUploadEmpBankAccount.GetDefaultBankAccount(dbConn, uploadEmpPay.EmpID, ParseTemp.GetUploadEmpID(dbConn, EmpNo, m_SessionID), m_SessionID);

                    if (uploadBankAccount != null)
                    {
                        //if (uploadBankAccount.EmpBankAccountID > 0)
                        //    uploadEmpPay.EmpAccID = uploadBankAccount.EmpBankAccountID;
                        //else
                        //    uploadEmpPay.UploadEmpAccID = uploadBankAccount.UploadEmpBankAccountID;
                    }
                    else
                        if (uploadEmpPay.EmpID > 0)
                        {
                            EEmpBankAccount bankAccount = EEmpBankAccount.GetDefaultBankAccount(dbConn, uploadEmpPay.EmpID);
                            if (bankAccount != null)
                            {
                                //uploadEmpPay.EmpAccID = bankAccount.EmpBankAccountID;
                            }
                            else
                                errors.addError(ImportErrorMessage.ERROR_INVALID_FIELD_VALUE, new string[] { FIELD_ACCOUNT_NO + "=" + BankCode, EmpNo, rowCount.ToString() });
                        }
                        else
                            errors.addError(ImportErrorMessage.ERROR_INVALID_FIELD_VALUE, new string[] { FIELD_ACCOUNT_NO + "=" + BankCode, EmpNo, rowCount.ToString() });
                }

                if (rawDataTable.Columns.Contains(FIELD_IS_NON_PAYROLL_ITEM))
                {
                    uploadEmpPay.EmpRPIsNonPayrollItem = row[FIELD_IS_NON_PAYROLL_ITEM].ToString().Equals("Yes", StringComparison.CurrentCultureIgnoreCase) || row[FIELD_IS_NON_PAYROLL_ITEM].ToString().Equals("Y", StringComparison.CurrentCultureIgnoreCase);
                }

                if (rawDataTable.Columns.Contains(FIELD_COST_CENTER))
                {
                    string CostCenter = row[FIELD_COST_CENTER].ToString();
                    if (!string.IsNullOrEmpty(CostCenter))
                    {
                        uploadEmpPay.CostCenterID = HROne.Import.Parse.GetCostCenterID(dbConn, CostCenter, false, UserID);
                        if (uploadEmpPay.CostCenterID <= 0)
                            errors.addError(ImportErrorMessage.ERROR_INVALID_FIELD_VALUE, new string[] { FIELD_COST_CENTER + "=" + CostCenter, EmpNo, rowCount.ToString() });
                    }
                    else 
                        uploadEmpPay.CostCenterID = 0;
                }

                uploadEmpPay.EmpRPRemark = row[FIELD_REMARK].ToString().Trim();



                uploadEmpPay.SessionID = m_SessionID;
                uploadEmpPay.TransactionDate = UploadDateTime;


                if (uploadEmpPay.EmpID != 0 && errors.List.Count <= 0)
                {
                    //if (rawDataTable.Columns.Contains(FIELD_INTERNAL_ID))
                    //{
                    //    try
                    //    {
                    //        if (!row.IsNull(FIELD_INTERNAL_ID))
                    //        {
                    //            int tmpID = FromHexDecWithCheckDigit((string)row[FIELD_INTERNAL_ID]);
                    //            EEmpRecurringPayment tmpObj = new EEmpRecurringPayment();
                    //            tmpObj.EmpRPID = tmpID;
                    //            if (EEmpRecurringPayment.db.select(dbConn, tmpObj))
                    //                uploadEmpPay.EmpRPID = tmpID;
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
                            uploadEmpPay.SynID = strSynID;
                            if (!string.IsNullOrEmpty(strSynID))
                            {
                                DBFilter synIDFilter = new DBFilter();
                                synIDFilter.add(new Match("SynID", strSynID));
                                ArrayList objSameSynIDList = EEmpRecurringPayment.db.select(dbConn, synIDFilter);
                                if (objSameSynIDList.Count > 0)
                                    uploadEmpPay.EmpRPID = ((EEmpRecurringPayment)objSameSynIDList[0]).EmpRPID;
                            }
                        }

                    }

                    if (uploadEmpPay.EmpRPID == 0)
                    {

                        AND andTerms = new AND();
                        andTerms.add(new Match("EmpRPEffFr", "<=", uploadEmpPay.EmpRPEffFr));
                        andTerms.add(new Match("PayCodeID", uploadEmpPay.PayCodeID));

                        lastEmpPay = (EEmpRecurringPayment)AppUtils.GetLastObj(dbConn, uploadDB, "EmpRPEffFr", uploadEmpPay.EmpID, andTerms);


                        if (lastEmpPay != null)
                        {
                            if (uploadEmpPay.EmpAccID == lastEmpPay.EmpAccID
                                && Math.Abs(uploadEmpPay.EmpRPAmount - lastEmpPay.EmpRPAmount) < 0.01
                                && uploadEmpPay.EmpRPMethod == lastEmpPay.EmpRPMethod
                                && uploadEmpPay.EmpRPUnit == lastEmpPay.EmpRPUnit
                                && uploadEmpPay.PayCodeID == lastEmpPay.PayCodeID
                                && uploadEmpPay.EmpRPEffFr == lastEmpPay.EmpRPEffFr
                                && uploadEmpPay.EmpRPEffTo == lastEmpPay.EmpRPEffTo
                                && uploadEmpPay.SchemeCode == lastEmpPay.SchemeCode
                                && uploadEmpPay.Capacity == lastEmpPay.Capacity
                                && uploadEmpPay.Point == lastEmpPay.Point
                                && uploadEmpPay.CostCenterID == lastEmpPay.CostCenterID
                                && uploadEmpPay.EmpRPRemark == lastEmpPay.EmpRPRemark
                                && uploadEmpPay.EmpRPIsNonPayrollItem == lastEmpPay.EmpRPIsNonPayrollItem
                                )
                                continue;
                            else
                            {
                                // add postion terms with new ID
                                if (lastEmpPay.EmpRPEffFr.Equals(uploadEmpPay.EmpRPEffFr))
                                {
                                    uploadEmpPay.EmpRPID = lastEmpPay.EmpRPID;
                                    if (uploadEmpPay.EmpRPEffTo.Ticks == 0 && lastEmpPay.EmpRPEffTo.Ticks != 0)
                                    {
                                        AND andNextTerms = new AND();
                                        andNextTerms.add(new Match("EmpRPEffFr", ">", lastEmpPay.EmpRPEffTo));
                                        andNextTerms.add(new Match("PayCodeID", uploadEmpPay.PayCodeID));

                                        EEmpRecurringPayment afterEmpRP = (EEmpRecurringPayment)AppUtils.GetLastObj(dbConn, uploadDB, "EmpRPEffFr", uploadEmpPay.EmpID, andNextTerms);
                                        if (afterEmpRP != null)
                                            uploadEmpPay.EmpRPEffTo = afterEmpRP.EmpRPEffFr.AddDays(-1);
                                    }
                                }
                                else
                                {
                                    AND lastObjAndTerms = new AND();
                                    lastObjAndTerms.add(new Match("EmpRPEffFr", ">", uploadEmpPay.EmpRPEffFr));
                                    if (!uploadEmpPay.EmpRPEffTo.Ticks.Equals(0))
                                        lastObjAndTerms.add(new Match("EmpRPEffFr", "<=", uploadEmpPay.EmpRPEffTo));
                                    lastObjAndTerms.add(new Match("PayCodeID", uploadEmpPay.PayCodeID));
                                    EEmpRecurringPayment lastObj = (EEmpRecurringPayment)AppUtils.GetLastObj(dbConn, uploadDB, "EmpRPEffFr", uploadEmpPay.EmpID, lastObjAndTerms);
                                    if (lastObj != null)
                                        if (!lastObj.EmpRPEffTo.Ticks.Equals(0))
                                        {
                                            errors.addError(ImportErrorMessage.ERROR_DATE_FROM_OVERLAP, new string[] { uploadEmpPay.EmpRPEffFr.ToString("yyyy-MM-dd"), rowCount.ToString() });
                                            continue;
                                        }
                                }

                            }
                        }
                    }
                }
                if (uploadEmpPay.EmpRPID <= 0)
                    uploadEmpPay.ImportActionStatus = ImportDBObject.ImportActionEnum.INSERT;
                else
                    uploadEmpPay.ImportActionStatus = ImportDBObject.ImportActionEnum.UPDATE;


                Hashtable values = new Hashtable();
                tempDB.populate(uploadEmpPay, values);
                PageErrors pageErrors = new PageErrors(tempDB);
                tempDB.validate(pageErrors, values);
                if (pageErrors.errors.Count == 0)
                {

                    tempDB.insert(dbConn, uploadEmpPay);
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
            ImportEmpRecurringPaymentProcess import = new ImportEmpRecurringPaymentProcess(dbConn, sessionID);
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
        public virtual void ImportToDatabase(int UploadEmpID)
        {
            DBFilter sessionFilter = new DBFilter();
            sessionFilter.add(new Match("SessionID", m_SessionID));
            if (UploadEmpID > 0)
                sessionFilter.add(new Match("UploadEmpID", UploadEmpID));
            ArrayList uploadEmpPayList = tempDB.select(dbConn, sessionFilter);
            foreach (EUploadEmpRecurringPayment obj in uploadEmpPayList)
            {
                EEmpRecurringPayment empRP = new EEmpRecurringPayment();

                if (obj.ImportActionStatus != ImportDBObject.ImportActionEnum.INSERT)
                {
                    empRP.EmpRPID = obj.EmpRPID;
                    uploadDB.select(dbConn, empRP);
                }

                obj.ExportToObject(empRP);

                if (obj.ImportActionStatus == ImportDBObject.ImportActionEnum.INSERT)
                {
                    empRP.EmpID = ParseTemp.GetEmpIDFromUploadEmpID(dbConn, obj.UploadEmpID);
                    AND andTerms = new AND();
                    andTerms.add(new Match("EmpRPEffFr", "<=", empRP.EmpRPEffFr));
                    andTerms.add(new Match("PayCodeID", empRP.PayCodeID));

                    EEmpRecurringPayment lastObj = (EEmpRecurringPayment)AppUtils.GetLastObj(dbConn, EEmpRecurringPayment.db, "EmpRPEffFr", empRP.EmpID, andTerms);
                    if (lastObj != null)
                        if (lastObj.EmpRPEffTo.Ticks == 0)
                        {
                            lastObj.EmpRPEffTo = empRP.EmpRPEffFr.AddDays(-1);
                            uploadDB.update(dbConn, lastObj);
                        }

                    if (obj.UploadEmpAccID > 0)
                        empRP.EmpAccID = ParseTemp.GetEmpBankAccIDFromUploadEmpBankAccID(dbConn, obj.UploadEmpAccID);
                    uploadDB.insert(dbConn, empRP);
                    //DBFilter emplastPosFilter = new DBFilter();
                }
                else if (obj.ImportActionStatus == ImportDBObject.ImportActionEnum.UPDATE)
                {
                    uploadDB.update(dbConn, empRP);

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
            //if (IsIncludeInternalID)
            //    tmpDataTable.Columns.Add(FIELD_INTERNAL_ID, typeof(string));
            tmpDataTable.Columns.Add(FIELD_EMP_NO, typeof(string));

            if (IsIncludeCurrentPositionInfo)
            {
                ImportEmpPersonalInfoProcess.AddEmployeeInfoHeader(tmpDataTable);
                ImportEmpPositionInfoProcess.AddEmployeePositionInfoHeader(dbConn, tmpDataTable);
            }

            tmpDataTable.Columns.Add(FIELD_FROM, typeof(DateTime));
            tmpDataTable.Columns.Add(FIELD_TO, typeof(DateTime));
            tmpDataTable.Columns.Add(FIELD_PAYMENT_CODE, typeof(string));
            if (ESystemParameter.getParameter(dbConn, ESystemParameter.PARAM_CODE_PAYSCALE_POINT_SYSTEM) == "Y")        // for WaiJe
            {
                tmpDataTable.Columns.Add(FIELD_SCHEME_CODE, typeof(string));
                tmpDataTable.Columns.Add(FIELD_CAPACITY, typeof(string));
                tmpDataTable.Columns.Add(FIELD_POINT, typeof(decimal));
            }

            if (ESystemParameter.getParameter(dbConn, ESystemParameter.PARAM_CODE_MONTHLY_ACHIEVEMENT_COMMISSION) == "Y")       // for F&V
            {
                tmpDataTable.Columns.Add(FIELD_BASIC_SALARY, typeof(decimal));
                tmpDataTable.Columns.Add(FIELD_FPS, typeof(decimal));
            }

            tmpDataTable.Columns.Add(FIELD_AMOUNT, typeof(double));
            tmpDataTable.Columns.Add(FIELD_UNIT, typeof(string));
            tmpDataTable.Columns.Add(FIELD_METHOD, typeof(string));
            tmpDataTable.Columns.Add(FIELD_ACCOUNT_NO, typeof(string));
            tmpDataTable.Columns.Add(FIELD_IS_NON_PAYROLL_ITEM, typeof(string));
            tmpDataTable.Columns.Add(FIELD_COST_CENTER, typeof(string));
            tmpDataTable.Columns.Add(FIELD_REMARK, typeof(string));
            if (IsIncludeSyncID)
                tmpDataTable.Columns.Add(FIELD_SYNC_ID, typeof(string));

            foreach (EEmpPersonalInfo empInfo in empList)
            {
                if (EEmpPersonalInfo.db.select(dbConn, empInfo))
                {
                    DBFilter filter = new DBFilter();
                    filter.add(new Match("EmpID", empInfo.EmpID));
                    filter.add(getCreateModifiedRecordsAfterDBTerm(ReferenceDateTime));
                    ArrayList list = EEmpRecurringPayment.db.select(dbConn, filter);
                    foreach (EEmpRecurringPayment empRecurringPayment in list)
                    {
                        DataRow row = tmpDataTable.NewRow();
                        //if (IsIncludeInternalID)
                        //    row[FIELD_INTERNAL_ID] = ToHexDecWithCheckDigit(empRecurringPayment.EmpRPID);
                        row[FIELD_EMP_NO] = empInfo.EmpNo;

                        if (IsIncludeCurrentPositionInfo)
                        {
                            ImportEmpPersonalInfoProcess.AddEmployeeInfo(dbConn, row, empInfo.EmpID);
                            ImportEmpPositionInfoProcess.AddEmployeePositionInfo(dbConn, row, empInfo.EmpID);
                        }

                        row[FIELD_FROM] = empRecurringPayment.EmpRPEffFr;
                        row[FIELD_TO] = empRecurringPayment.EmpRPEffTo;

                        EPaymentCode paymentCode = new EPaymentCode();
                        paymentCode.PaymentCodeID = empRecurringPayment.PayCodeID;
                        if (EPaymentCode.db.select(dbConn, paymentCode))
                            row[FIELD_PAYMENT_CODE] = IsShowDescription ? paymentCode.PaymentCodeDesc : paymentCode.PaymentCode;


                        if (ESystemParameter.getParameter(dbConn, ESystemParameter.PARAM_CODE_PAYSCALE_POINT_SYSTEM) == "Y")        // for WaiJe
                        {
                            row[FIELD_SCHEME_CODE] = empRecurringPayment.SchemeCode;
                            row[FIELD_CAPACITY] = empRecurringPayment.Capacity;
                            row[FIELD_POINT] = empRecurringPayment.Point;
                        }

                        if (ESystemParameter.getParameter(dbConn, ESystemParameter.PARAM_CODE_MONTHLY_ACHIEVEMENT_COMMISSION) == "Y")       // for F&V
                        {
                            row[FIELD_BASIC_SALARY] = empRecurringPayment.EmpRPBasicSalary;
                            row[FIELD_FPS] = empRecurringPayment.EmpRPFPS;
                        }

                        row[FIELD_AMOUNT] = empRecurringPayment.EmpRPAmount;
                        if (empRecurringPayment.EmpRPUnit.Equals("H"))
                            row[FIELD_UNIT] = "Hourly";
                        else if (empRecurringPayment.EmpRPUnit.Equals("D"))
                            row[FIELD_UNIT] = "Daily";
                        else if (empRecurringPayment.EmpRPUnit.Equals("P"))
                            row[FIELD_UNIT] = "Once per payroll cycle";

                        if (empRecurringPayment.EmpRPMethod.Equals("A"))
                        {
                            row[FIELD_METHOD] = "Autopay";
                            EEmpBankAccount empBankAccount = new EEmpBankAccount();
                            empBankAccount.EmpBankAccountID = empRecurringPayment.EmpAccID;
                            if (EEmpBankAccount.db.select(dbConn, empBankAccount))
                                row[FIELD_ACCOUNT_NO] = empBankAccount.EmpBankCode + "-" + empBankAccount.EmpBranchCode + "-" + empBankAccount.EmpAccountNo;
                        }
                        else if (empRecurringPayment.EmpRPMethod.Equals("Q"))
                            row[FIELD_METHOD] = "Cheque";
                        else if (empRecurringPayment.EmpRPMethod.Equals("C"))
                            row[FIELD_METHOD] = "Cash";
                        else
                            row[FIELD_METHOD] = "Others";



                        row[FIELD_IS_NON_PAYROLL_ITEM] = empRecurringPayment.EmpRPIsNonPayrollItem ? "Yes" : "No";


                        ECostCenter costCenter = new ECostCenter();
                        costCenter.CostCenterID = empRecurringPayment.CostCenterID;
                        if (ECostCenter.db.select(dbConn, costCenter))
                            row[FIELD_COST_CENTER] = IsShowDescription ? costCenter.CostCenterDesc : costCenter.CostCenterCode;

                        row[FIELD_REMARK] = empRecurringPayment.EmpRPRemark;
                        if (IsIncludeSyncID)
                            row[FIELD_SYNC_ID] = empRecurringPayment.SynID;

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
