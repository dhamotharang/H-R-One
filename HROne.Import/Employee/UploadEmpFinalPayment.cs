using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Data;
using System.Text;
using HROne.DataAccess;
//using perspectivemind.validation;
using HROne.Lib.Entities;

namespace HROne.Import
{
    /// <summary>
    /// Summary description for ImportEmpFinalPaymentProcess
    /// </summary>
    public class ImportEmpFinalPaymentProcess : ImportProcessInteface
    {

        [DBClass("UploadEmpFinalPayment")]
        private class EUploadEmpFinalPayment : ImportDBObject
        {
            public static DBManager db = new DBManager(typeof(EUploadEmpFinalPayment));

            protected int m_UploadEmpFinalPayID;
            [DBField("UploadEmpFinalPayID", true, true), TextSearch, Export(false)]
            public int UploadEmpFinalPayID
            {
                get { return m_UploadEmpFinalPayID; }
                set { m_UploadEmpFinalPayID = value; modify("UploadEmpFinalPayID"); }
            }
            protected int m_UploadEmpID;
            [DBField("UploadEmpID"), TextSearch, Export(false)]
            public int UploadEmpID
            {
                get { return m_UploadEmpID; }
                set { m_UploadEmpID = value; modify("UploadEmpID"); }
            }

            protected int m_EmpFinalPayID;
            [DBField("EmpFinalPayID"), TextSearch, Export(false)]
            public int EmpFinalPayID
            {
                get { return m_EmpFinalPayID; }
                set { m_EmpFinalPayID = value; modify("EmpFinalPayID"); }
            }
            protected int m_EmpID;
            [DBField("EmpID"), TextSearch, Export(false)]
            public int EmpID
            {
                get { return m_EmpID; }
                set { m_EmpID = value; modify("EmpID"); }
            }
            protected int m_UploadEmpAccID;
            [DBField("UploadEmpAccID"), TextSearch, Export(false)]
            public int UploadEmpAccID
            {
                get { return m_UploadEmpAccID; }
                set { m_UploadEmpAccID = value; modify("UploadEmpAccID"); }
            }
            protected int m_PayCodeID;
            [DBField("PayCodeID"), TextSearch, Export(false), Required]
            public int PayCodeID
            {
                get { return m_PayCodeID; }
                set { m_PayCodeID = value; modify("PayCodeID"); }
            }
            protected double m_EmpFinalPayAmount;
            [DBField("EmpFinalPayAmount", "0.00"), TextSearch, Export(false), Required]
            public double EmpFinalPayAmount
            {
                get { return m_EmpFinalPayAmount; }
                set { m_EmpFinalPayAmount = value; modify("EmpFinalPayAmount"); }
            }
            protected string m_CurrencyID;
            [DBField("CurrencyID"), TextSearch, Export(false)]
            public string CurrencyID
            {
                get { return m_CurrencyID; }
                set { m_CurrencyID = value; modify("CurrencyID"); }
            }
            protected string m_EmpFinalPayMethod;
            [DBField("EmpFinalPayMethod"), TextSearch, Export(false), Required]
            public string EmpFinalPayMethod
            {
                get { return m_EmpFinalPayMethod; }
                set { m_EmpFinalPayMethod = value; modify("EmpFinalPayMethod"); }
            }
            protected int m_EmpAccID;
            [DBField("EmpAccID"), TextSearch, Export(false)]
            public int EmpAccID
            {
                get { return m_EmpAccID; }
                set { m_EmpAccID = value; modify("EmpAccID"); }
            }
            protected int m_CostCenterID;
            [DBField("CostCenterID"), TextSearch, Export(false)]
            public int CostCenterID
            {
                get { return m_CostCenterID; }
                set { m_CostCenterID = value; modify("CostCenterID"); }
            }
            protected string m_EmpFinalPayRemark;
            [DBField("EmpFinalPayRemark"), TextSearch, Export(false)]
            public string EmpFinalPayRemark
            {
                get { return m_EmpFinalPayRemark; }
                set { m_EmpFinalPayRemark = value; modify("EmpFinalPayRemark"); }
            }
            protected bool m_EmpFinalPayIsAutoGen;
            [DBField("EmpFinalPayIsAutoGen"), TextSearch, Export(false), Required]
            public bool EmpFinalPayIsAutoGen
            {
                get { return m_EmpFinalPayIsAutoGen; }
                set { m_EmpFinalPayIsAutoGen = value; modify("EmpFnialPayIsAutoGen"); }
            }
            protected double m_EmpFinalPayNumOfDayAdj;
            [DBField("EmpFinalPayNumOfDayAdj"), TextSearch, Export(false)]
            public double EmpFinalPayNumOfDayAdj
            {
                get { return m_EmpFinalPayNumOfDayAdj; }
                set { m_EmpFinalPayNumOfDayAdj = value; modify("EmpFinalPayNumOfDayAdj"); }
            }

            protected int m_LeaveAppID;
            [DBField("LeaveAppID"), TextSearch, Export(false)]
            public int LeaveAppID
            {
                get { return m_LeaveAppID; }
                set { m_LeaveAppID = value; modify("LeaveAppID"); }
            }

            public object m_PayRecID;
            [DBField("PayRecID"), TextSearch, Export(false)]
            public object PayRecID
            {
                get { return m_PayRecID; }
                set { m_PayRecID = value; modify("PayRecID"); }
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

        public const string TABLE_NAME = "final_payment";

        //  Field Const for Employee Personal Information
        private const string FIELD_EMP_NO = "Emp No";
        private const string FIELD_PAYMENT_CODE = "Payment Code";
        private const string FIELD_AMOUNT = "Final Pay Amount";
        private const string FIELD_METHOD = "Pay Method";
        private const string FIELD_ACCOUNT_NO = "Account";
        private const string FIELD_NUM_OF_DAY_ADJUST = "No of Day Adjust";
        private const string FIELD_COST_CENTER = "Cost Center";
        private const string FIELD_REMARK = "Remark";

        
        private DateTime UploadDateTime = AppUtils.ServerDateTime();
        private DBManager tempDB = EUploadEmpFinalPayment.db;
        private DBManager uploadDB = EEmpFinalPayment.db;

        public ImportErrorList errors = new ImportErrorList();
        

        public ImportEmpFinalPaymentProcess(DatabaseConnection dbConn, string SessionID): base(dbConn, SessionID)
        {
          
            
        }


        public DataTable UploadToTempDatabase(DataTable rawDataTable, int UserID)
        {
            if (rawDataTable == null)
                return GetImportDataFromTempDatabase(null);

            int rowCount = 1;
            foreach (DataRow row in rawDataTable.Rows)
            {
                rowCount++;

                EUploadEmpFinalPayment uploadEmpFinalPay = new EUploadEmpFinalPayment();
                //EEmpFinalPayment lastEmpFinalPay;

                string EmpNo = row[FIELD_EMP_NO].ToString().Trim();
                uploadEmpFinalPay.EmpID = Parse.GetEmpID(dbConn, EmpNo, UserID);
                if (uploadEmpFinalPay.EmpID < 0)
                    errors.addError(ImportErrorMessage.ERROR_ACCESS_DENIED_EMP_NO, new string[] { EmpNo, rowCount.ToString() });
                uploadEmpFinalPay.PayCodeID = Parse.GetPaymentCodeID(dbConn, row[FIELD_PAYMENT_CODE].ToString());

                if (uploadEmpFinalPay.PayCodeID == 0)
                    errors.addError(ImportErrorMessage.ERROR_INVALID_FIELD_VALUE, new string[] { FIELD_PAYMENT_CODE + "=" + row[FIELD_PAYMENT_CODE].ToString(), EmpNo, rowCount.ToString() });


                double amount = 0;
                if (double.TryParse(row[FIELD_AMOUNT].ToString(), out amount))
                    uploadEmpFinalPay.EmpFinalPayAmount = amount;
                else
                    errors.addError(ImportErrorMessage.ERROR_INVALID_FIELD_VALUE, new string[] { FIELD_AMOUNT + "=" + row[FIELD_AMOUNT].ToString(), EmpNo, rowCount.ToString() });

                uploadEmpFinalPay.CurrencyID = HROne.Lib.ExchangeCurrency.DefaultCurrency();

                uploadEmpFinalPay.EmpFinalPayMethod = Parse.toPaymentMethodCode(row[FIELD_METHOD].ToString());

                string BankCode = row[FIELD_ACCOUNT_NO].ToString();
                uploadEmpFinalPay.EmpAccID = HROne.Import.Parse.GetEmpAccID(dbConn, BankCode, uploadEmpFinalPay.EmpID);

                //  Check if the bank account no does not exist in database
                if (uploadEmpFinalPay.EmpAccID == 0 && !BankCode.Trim().Equals(string.Empty))
                    errors.addError(ImportErrorMessage.ERROR_INVALID_FIELD_VALUE, new string[] { FIELD_ACCOUNT_NO + "=" + BankCode, EmpNo, rowCount.ToString() });

                if (uploadEmpFinalPay.EmpAccID > 0 && !uploadEmpFinalPay.EmpFinalPayMethod.Equals("A"))
                    errors.addError(ImportErrorMessage.ERROR_INVALID_FIELD_VALUE, new string[] { FIELD_METHOD + "=" + row[FIELD_METHOD].ToString(), EmpNo, rowCount.ToString() });

                if (uploadEmpFinalPay.EmpFinalPayMethod.Equals("A") && uploadEmpFinalPay.EmpAccID == 0)
                {

                    EUploadEmpBankAccount uploadBankAccount = EUploadEmpBankAccount.GetDefaultBankAccount(dbConn, uploadEmpFinalPay.EmpID, ParseTemp.GetUploadEmpID(dbConn, EmpNo, m_SessionID), m_SessionID);

                    if (uploadBankAccount != null)
                    {
                        //if (uploadBankAccount.EmpBankAccountID > 0)
                        //    uploadEmpFinalPay.EmpAccID = uploadBankAccount.EmpBankAccountID;
                        //else
                        //    uploadEmpFinalPay.UploadEmpAccID = uploadBankAccount.UploadEmpBankAccountID;
                    }
                    else
                        if (uploadEmpFinalPay.EmpID > 0)
                        {
                            EEmpBankAccount bankAccount = EEmpBankAccount.GetDefaultBankAccount(dbConn, uploadEmpFinalPay.EmpID);
                            if (bankAccount != null)
                            {
                                //uploadEmpFinalPay.EmpAccID = bankAccount.EmpBankAccountID;
                            }
                            else
                                errors.addError(ImportErrorMessage.ERROR_INVALID_FIELD_VALUE, new string[] { FIELD_ACCOUNT_NO + "=" + BankCode, EmpNo, rowCount.ToString() });
                        }
                        else
                            errors.addError(ImportErrorMessage.ERROR_INVALID_FIELD_VALUE, new string[] { FIELD_ACCOUNT_NO + "=" + BankCode, EmpNo, rowCount.ToString() });
                }

                if (rawDataTable.Columns.Contains(FIELD_COST_CENTER))
                {
                    string CostCenter = row[FIELD_COST_CENTER].ToString();
                    if (!string.IsNullOrEmpty(CostCenter))
                    {
                        uploadEmpFinalPay.CostCenterID = HROne.Import.Parse.GetCostCenterID(dbConn, CostCenter, false, UserID);
                        if (uploadEmpFinalPay.CostCenterID <= 0)
                            errors.addError(ImportErrorMessage.ERROR_INVALID_FIELD_VALUE, new string[] { FIELD_COST_CENTER + "=" + CostCenter, EmpNo, rowCount.ToString() });
                    }
                    else
                        uploadEmpFinalPay.CostCenterID = 0;
                }

                uploadEmpFinalPay.EmpFinalPayRemark = row[FIELD_REMARK].ToString().Trim();

                double NumOfDayAdjust = 0;

                if (rawDataTable.Columns.Contains(FIELD_NUM_OF_DAY_ADJUST))
                {
                    string NumOfDayAdjustString = row[FIELD_NUM_OF_DAY_ADJUST].ToString();
                    if (!string.IsNullOrEmpty(NumOfDayAdjustString))
                        if (!double.TryParse(NumOfDayAdjustString, out NumOfDayAdjust))
                            errors.addError(ImportErrorMessage.ERROR_INVALID_FIELD_VALUE, new string[] { FIELD_NUM_OF_DAY_ADJUST + "=" + NumOfDayAdjustString, EmpNo, rowCount.ToString() });
                        else
                            uploadEmpFinalPay.EmpFinalPayNumOfDayAdj = NumOfDayAdjust;
                    else
                        uploadEmpFinalPay.EmpFinalPayNumOfDayAdj = 0;
                }
                
                uploadEmpFinalPay.EmpFinalPayIsAutoGen = false;


                uploadEmpFinalPay.SessionID = m_SessionID;
                uploadEmpFinalPay.TransactionDate = UploadDateTime;

                //  Final Payment do NOT have unique constraint, skip checking duplicate record
                //if (uploadEmpFinalPay.EmpID != 0)
                //{
                //    AND andTerms = new AND();
                //    andTerms.add(new Match("PayCodeID", uploadEmpFinalPay.PayCodeID));

                //    lastEmpFinalPay = (EEmpFinalPayment)AppUtils.GetLastObj(dbConn, uploadDB, "EmpFinalPayID", uploadEmpFinalPay.EmpID, andTerms);


                //    if (lastEmpFinalPay != null)
                //    {
                //        if (uploadEmpFinalPay.EmpAccID == lastEmpFinalPay.EmpAccID
                //            && Math.Abs(uploadEmpFinalPay.EmpFinalPayAmount - lastEmpFinalPay.EmpFinalPayAmount) < 0.01
                //            && uploadEmpFinalPay.EmpFinalPayMethod == lastEmpFinalPay.EmpFinalPayMethod
                //            && uploadEmpFinalPay.PayCodeID == lastEmpFinalPay.PayCodeID
                //            )
                //            continue;
                //        else
                //        {
                //            // add postion terms with new ID
                //                uploadEmpFinalPay.EmpFinalPayID = lastEmpFinalPay.EmpFinalPayID;
                //        }
                //    }
                //}
                //if (rawDataTable.Columns.Contains(FIELD_INTERNAL_ID))
                //{
                //    try
                //    {
                //        if (!row.IsNull(FIELD_INTERNAL_ID))
                //        {
                //            int tmpID = FromHexDecWithCheckDigit((string)row[FIELD_INTERNAL_ID]);
                //            EEmpFinalPayment tmpObj = new EEmpFinalPayment();
                //            tmpObj.EmpFinalPayID = tmpID;
                //            if (EEmpFinalPayment.db.select(dbConn, tmpObj))
                //                uploadEmpFinalPay.EmpFinalPayID = tmpID;
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
                        uploadEmpFinalPay.SynID = strSynID;
                        if (!string.IsNullOrEmpty(strSynID))
                        {
                            DBFilter synIDFilter = new DBFilter();
                            synIDFilter.add(new Match("SynID", strSynID));
                            ArrayList objSameSynIDList = EEmpFinalPayment.db.select(dbConn, synIDFilter);
                            if (objSameSynIDList.Count > 0)
                                uploadEmpFinalPay.EmpFinalPayID = ((EEmpFinalPayment)objSameSynIDList[0]).EmpFinalPayID;
                        }
                    }

                }

                if (uploadEmpFinalPay.EmpFinalPayID <= 0)
                    uploadEmpFinalPay.ImportActionStatus = ImportDBObject.ImportActionEnum.INSERT;
                else
                    uploadEmpFinalPay.ImportActionStatus = ImportDBObject.ImportActionEnum.UPDATE;

                uploadEmpFinalPay.UploadEmpID = ParseTemp.GetUploadEmpID(dbConn, EmpNo, m_SessionID);
                if (uploadEmpFinalPay.UploadEmpID == 0)
                    if (uploadEmpFinalPay.EmpID == 0)
                        errors.addError(ImportErrorMessage.ERROR_INVALID_EMP_NO, new string[] { EmpNo, rowCount.ToString() });
                    else
                        uploadEmpFinalPay.UploadEmpID = ImportEmpPersonalInfoProcess.CreateDummyUploadEmployeeInfo(dbConn, uploadEmpFinalPay.EmpID, m_SessionID, UploadDateTime);

                Hashtable values = new Hashtable();
                tempDB.populate(uploadEmpFinalPay, values);
                PageErrors pageErrors = new PageErrors(tempDB);
                tempDB.validate(pageErrors, values);
                if (pageErrors.errors.Count == 0)
                {

                    tempDB.insert(dbConn, uploadEmpFinalPay);
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
            ImportEmpFinalPaymentProcess import = new ImportEmpFinalPaymentProcess(dbConn, sessionID);
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
            ArrayList uploadEmpFinalPayList = tempDB.select(dbConn, sessionFilter);
            foreach (EUploadEmpFinalPayment obj in uploadEmpFinalPayList)
            {
                EEmpFinalPayment EmpFinalPay = new EEmpFinalPayment();

                if (obj.ImportActionStatus != ImportDBObject.ImportActionEnum.INSERT)
                {
                    EmpFinalPay.EmpFinalPayID = obj.EmpFinalPayID;
                    uploadDB.select(dbConn, EmpFinalPay);
                }

                obj.ExportToObject(EmpFinalPay);

                if (obj.ImportActionStatus == ImportDBObject.ImportActionEnum.INSERT)
                {


                    EmpFinalPay.EmpID = ParseTemp.GetEmpIDFromUploadEmpID(dbConn, obj.UploadEmpID);
                    if (obj.UploadEmpAccID > 0)
                        EmpFinalPay.EmpAccID = ParseTemp.GetEmpBankAccIDFromUploadEmpBankAccID(dbConn, obj.UploadEmpAccID);
                    uploadDB.insert(dbConn, EmpFinalPay);
                    //DBFilter emplastPosFilter = new DBFilter();
                }
                else if (obj.ImportActionStatus == ImportDBObject.ImportActionEnum.UPDATE)
                {
                    uploadDB.update(dbConn, EmpFinalPay);

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

            tmpDataTable.Columns.Add(FIELD_PAYMENT_CODE, typeof(string));
            tmpDataTable.Columns.Add(FIELD_AMOUNT, typeof(double));
            tmpDataTable.Columns.Add(FIELD_METHOD, typeof(string));
            tmpDataTable.Columns.Add(FIELD_ACCOUNT_NO, typeof(string));
            tmpDataTable.Columns.Add(FIELD_COST_CENTER, typeof(string));
            tmpDataTable.Columns.Add(FIELD_NUM_OF_DAY_ADJUST, typeof(double));
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
                    ArrayList list = EEmpFinalPayment.db.select(dbConn, filter);
                    foreach (EEmpFinalPayment empFinalPayment in list)
                    {
                        DataRow row = tmpDataTable.NewRow();
                        //if (IsIncludeInternalID)
                        //    row[FIELD_INTERNAL_ID] = ToHexDecWithCheckDigit(empFinalPayment.EmpFinalPayID);
                        row[FIELD_EMP_NO] = empInfo.EmpNo;

                        if (IsIncludeCurrentPositionInfo)
                        {
                            ImportEmpPersonalInfoProcess.AddEmployeeInfo(dbConn, row, empInfo.EmpID);
                            ImportEmpPositionInfoProcess.AddEmployeePositionInfo(dbConn, row, empInfo.EmpID);
                        }

                        EPaymentCode paymentCode = new EPaymentCode();
                        paymentCode.PaymentCodeID = empFinalPayment.PayCodeID;
                        if (EPaymentCode.db.select(dbConn, paymentCode))
                            row[FIELD_PAYMENT_CODE] = IsShowDescription ? paymentCode.PaymentCodeDesc : paymentCode.PaymentCode;

                        row[FIELD_AMOUNT] = empFinalPayment.EmpFinalPayAmount;

                        if (empFinalPayment.EmpFinalPayMethod.Equals("A"))
                        {
                            row[FIELD_METHOD] = "Autopay";
                            EEmpBankAccount empBankAccount = new EEmpBankAccount();
                            empBankAccount.EmpBankAccountID = empFinalPayment.EmpAccID;
                            if (EEmpBankAccount.db.select(dbConn, empBankAccount))
                                row[FIELD_ACCOUNT_NO] = empBankAccount.EmpBankCode + "-" + empBankAccount.EmpBranchCode + "-" + empBankAccount.EmpAccountNo;
                        }
                        else if (empFinalPayment.EmpFinalPayMethod.Equals("Q"))
                            row[FIELD_METHOD] = "Cheque";
                        else if (empFinalPayment.EmpFinalPayMethod.Equals("C"))
                            row[FIELD_METHOD] = "Cash";
                        else
                            row[FIELD_METHOD] = "Others";

                        ECostCenter costCenter = new ECostCenter();
                        costCenter.CostCenterID = empFinalPayment.CostCenterID;
                        if (ECostCenter.db.select(dbConn, costCenter))
                            row[FIELD_COST_CENTER] = IsShowDescription ? costCenter.CostCenterDesc : costCenter.CostCenterCode;

                        row[FIELD_NUM_OF_DAY_ADJUST] = empFinalPayment.EmpFinalPayNumOfDayAdj;
                        row[FIELD_REMARK] = empFinalPayment.EmpFinalPayRemark;
                        if (IsIncludeSyncID)
                            row[FIELD_SYNC_ID] = empFinalPayment.SynID;

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