using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Data;
using HROne.DataAccess;
//using perspectivemind.validation;
using HROne.Lib.Entities;
using HROne.Common;

namespace HROne.Import
{
    [DBClass("UploadEmpBankAccount")]
    public class EUploadEmpBankAccount : ImportDBObject
    {

        public static DBManager db = new DBManager(typeof(EUploadEmpBankAccount));

        protected int m_UploadEmpBankAccountID;
        [DBField("UploadEmpBankAccountID", true, true), TextSearch, Export(false)]
        public int UploadEmpBankAccountID
        {
            get { return m_UploadEmpBankAccountID; }
            set { m_UploadEmpBankAccountID = value; modify("UploadEmpBankAccountID"); }
        }
        protected int m_UploadEmpID;
        [DBField("UploadEmpID"), TextSearch, Export(false)]
        public int UploadEmpID
        {
            get { return m_UploadEmpID; }
            set { m_UploadEmpID = value; modify("UploadEmpID"); }
        }

        protected int m_EmpBankAccountID;
        [DBField("EmpBankAccountID"), TextSearch, Export(false)]
        public int EmpBankAccountID
        {
            get { return m_EmpBankAccountID; }
            set { m_EmpBankAccountID = value; modify("EmpBankAccountID"); }
        }

        protected int m_EmpID;
        [DBField("EmpID"), TextSearch, Export(false)]
        public int EmpID
        {
            get { return m_EmpID; }
            set { m_EmpID = value; modify("EmpID"); }
        }
        protected string m_EmpBankCode;
        [DBField("EmpBankCode"), TextSearch, MaxLength(3, 3), Export(false), Required]
        public string EmpBankCode
        {
            get { return m_EmpBankCode; }
            set { m_EmpBankCode = value; modify("EmpBankCode"); }
        }
        protected string m_EmpBranchCode;
        [DBField("EmpBranchCode"), TextSearch, MaxLength(3, 3), Export(false), Required]
        public string EmpBranchCode
        {
            get { return m_EmpBranchCode; }
            set { m_EmpBranchCode = value; modify("EmpBranchCode"); }
        }
        protected string m_EmpAccountNo;
        [DBField("EmpAccountNo"), TextSearch, MaxLength(9, 9), Export(false), Required]
        public string EmpAccountNo
        {
            get { return m_EmpAccountNo; }
            set { m_EmpAccountNo = value; modify("EmpAccountNo"); }
        }
        protected string m_EmpBankAccountHolderName;
        [DBField("EmpBankAccountHolderName"), TextSearch, MaxLength(100, 25), Export(false), Required]
        public string EmpBankAccountHolderName
        {
            get { return m_EmpBankAccountHolderName; }
            set { m_EmpBankAccountHolderName = value; modify("EmpBankAccountHolderName"); }
        }
        protected string m_EmpBankAccountRemark;
        [DBField("EmpBankAccountRemark"), TextSearch, DBAESEncryptStringField, Export(false)]
        public string EmpBankAccountRemark
        {
            get { return m_EmpBankAccountRemark; }
            set { m_EmpBankAccountRemark = value; modify("EmpBankAccountRemark"); }
        }
        protected bool m_EmpAccDefault;
        [DBField("EmpAccDefault"), TextSearch, Export(false), Required]
        public bool EmpAccDefault
        {
            get { return m_EmpAccDefault; }
            set { m_EmpAccDefault = value; modify("EmpAccDefault"); }
        }
        //  For Synchronize Use only
        protected string m_SynID;
        [DBField("SynID"), TextSearch, Export(false)]
        public string SynID
        {
            get { return m_SynID; }
            set { m_SynID = value; modify("SynID"); }
        }


        public static EUploadEmpBankAccount GetDefaultBankAccount(DatabaseConnection dbConn, int EmpID, int UploadEmpID, string SessionID)
        {
            if (UploadEmpID > 0)
            {
                DBFilter filter = new DBFilter();
                filter.add(new Match("UploadEmpID", UploadEmpID));
                filter.add(new Match("EmpAccDefault", "<>", 0));
                filter.add(new Match("SessionID", SessionID));
                ArrayList bankAccountList = db.select(dbConn, filter);
                if (bankAccountList.Count > 0)
                    return (EUploadEmpBankAccount)bankAccountList[0];
            }
            if (EmpID > 0)
            {
                DBFilter filter = new DBFilter();
                filter.add(new Match("EmpID", EmpID));
                filter.add(new Match("EmpAccDefault", "<>", 0));
                filter.add(new Match("SessionID", SessionID));
                ArrayList bankAccountList = db.select(dbConn, filter);
                if (bankAccountList.Count > 0)
                    return (EUploadEmpBankAccount)bankAccountList[0];
            }
            return null;
        }
    }
    public class ImportEmpBankAccountProcess : ImportProcessInteface
    {
        public const string TABLE_NAME = "bank_account";

        //  Field Const for Employee Personal Information
        private const string FIELD_EMP_NO = "Emp No";
        private const string FIELD_BANK_CODE = "Bank Code";
        private const string FIELD_BRANCH_CODE = "Branch Code";
        private const string FIELD_ACC_NO = "Bank Account No";
        private const string FIELD_HOLDER_NAME = "Holder Name";
        private const string FIELD_DEFAULTACCOUNT = "Is Default Account";
        private const string FIELD_REMARK = "Remark";

        
        private DateTime UploadDateTime = AppUtils.ServerDateTime();
        private DBManager tempDB = EUploadEmpBankAccount.db;
        private DBManager uploadDB = EEmpBankAccount.db;

        public ImportErrorList errors = new ImportErrorList();
        

        public ImportEmpBankAccountProcess(DatabaseConnection dbConn, string SessionID): base(dbConn, SessionID)
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

                EUploadEmpBankAccount uploadEmpBankAcc = new EUploadEmpBankAccount();

                string EmpNo = row[FIELD_EMP_NO].ToString().Trim();
                uploadEmpBankAcc.EmpID = Parse.GetEmpID(dbConn, EmpNo, UserID);
                if (uploadEmpBankAcc.EmpID < 0)
                    errors.addError(ImportErrorMessage.ERROR_ACCESS_DENIED_EMP_NO, new string[] { EmpNo, rowCount.ToString() });
                uploadEmpBankAcc.EmpBankCode = row[FIELD_BANK_CODE].ToString();
                uploadEmpBankAcc.EmpBranchCode = row[FIELD_BRANCH_CODE].ToString();
                uploadEmpBankAcc.EmpAccountNo = row[FIELD_ACC_NO].ToString();

                uploadEmpBankAcc.EmpBankAccountHolderName = row[FIELD_HOLDER_NAME].ToString();
                uploadEmpBankAcc.EmpAccDefault = row[FIELD_DEFAULTACCOUNT].ToString().Equals("Yes", StringComparison.CurrentCultureIgnoreCase) || row[FIELD_DEFAULTACCOUNT].ToString().Equals("Y", StringComparison.CurrentCultureIgnoreCase);

                uploadEmpBankAcc.EmpBankAccountID = Parse.GetEmpAccID(dbConn, uploadEmpBankAcc.EmpBankCode, uploadEmpBankAcc.EmpBranchCode, uploadEmpBankAcc.EmpAccountNo, uploadEmpBankAcc.EmpID);

                if (rawDataTable.Columns.Contains(FIELD_REMARK))
                    uploadEmpBankAcc.EmpBankAccountRemark = row[FIELD_REMARK].ToString();


                uploadEmpBankAcc.SessionID = m_SessionID;
                uploadEmpBankAcc.TransactionDate = UploadDateTime;


                //if (rawDataTable.Columns.Contains(FIELD_INTERNAL_ID))
                //{
                //    try
                //    {
                //        if (!row.IsNull(FIELD_INTERNAL_ID))
                //        {
                //            int tmpID = FromHexDecWithCheckDigit((string)row[FIELD_INTERNAL_ID]);
                //            EEmpBankAccount tmpObj = new EEmpBankAccount();
                //            tmpObj.EmpBankAccountID = tmpID;
                //            if (EEmpBankAccount.db.select(dbConn, tmpObj))
                //                uploadEmpBankAcc.EmpBankAccountID = tmpID;
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
                        uploadEmpBankAcc.SynID = strSynID;
                        if (!string.IsNullOrEmpty(strSynID))
                        {
                            DBFilter synIDFilter = new DBFilter();
                            synIDFilter.add(new Match("SynID", strSynID));
                            ArrayList objSameSynIDList = EEmpBankAccount.db.select(dbConn, synIDFilter);
                            if (objSameSynIDList.Count > 0)
                                uploadEmpBankAcc.EmpBankAccountID = ((EEmpBankAccount)objSameSynIDList[0]).EmpBankAccountID;
                        }
                    }

                }

                if (uploadEmpBankAcc.EmpBankAccountID <= 0)
                    uploadEmpBankAcc.ImportActionStatus = ImportDBObject.ImportActionEnum.INSERT;
                else
                    uploadEmpBankAcc.ImportActionStatus = ImportDBObject.ImportActionEnum.UPDATE;

                uploadEmpBankAcc.UploadEmpID = ParseTemp.GetUploadEmpID(dbConn, EmpNo, m_SessionID);
                if (uploadEmpBankAcc.UploadEmpID == 0)
                    if (uploadEmpBankAcc.EmpID == 0)
                        errors.addError(ImportErrorMessage.ERROR_INVALID_EMP_NO, new string[] { EmpNo, rowCount.ToString() });
                    else
                        uploadEmpBankAcc.UploadEmpID = ImportEmpPersonalInfoProcess.CreateDummyUploadEmployeeInfo(dbConn, uploadEmpBankAcc.EmpID, m_SessionID, UploadDateTime);

                Hashtable values = new Hashtable();
                tempDB.populate(uploadEmpBankAcc, values);
                PageErrors pageErrors = new PageErrors(EUploadEmpPersonalInfo.db);
                tempDB.validate(pageErrors, values);
                if (pageErrors.errors.Count == 0)
                {

                    tempDB.insert(dbConn, uploadEmpBankAcc);
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
            ImportEmpBankAccountProcess import = new ImportEmpBankAccountProcess(dbConn, sessionID);
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
            ArrayList uploadEmpBankAccList = tempDB.select(dbConn, sessionFilter);
            foreach (EUploadEmpBankAccount obj in uploadEmpBankAccList)
            {

                EEmpBankAccount empBankAcc = new EEmpBankAccount();

                if (obj.ImportActionStatus != ImportDBObject.ImportActionEnum.INSERT)
                {
                    empBankAcc.EmpBankAccountID = obj.EmpBankAccountID;
                    uploadDB.select(dbConn, empBankAcc);
                }

                obj.ExportToObject(empBankAcc);

                if (obj.ImportActionStatus == ImportDBObject.ImportActionEnum.INSERT)
                {


                    empBankAcc.EmpID = ParseTemp.GetEmpIDFromUploadEmpID(dbConn, obj.UploadEmpID);
                    uploadDB.insert(dbConn, empBankAcc);
                    obj.EmpBankAccountID = empBankAcc.EmpBankAccountID;
                    tempDB.update(dbConn, obj);
                }
                else if (obj.ImportActionStatus == ImportDBObject.ImportActionEnum.UPDATE)
                {
                    uploadDB.update(dbConn, empBankAcc);

                }
                if (empBankAcc.EmpAccDefault)
                {
                    DBFilter filter = new DBFilter();
                    filter.add(new Match("EmpBankAccountID", "<>", empBankAcc.EmpBankAccountID));
                    filter.add(new Match("EmpID", empBankAcc.EmpID));
                    filter.add(new Match("EmpAccDefault", "<>", false));
                    EEmpBankAccount t = new EEmpBankAccount();
                    t.EmpAccDefault = false;
                    uploadDB.updateByTemplate(dbConn, t, filter);
                }
                tempDB.delete(dbConn, obj);
            }
        }
        public static DataTable Export(DatabaseConnection dbConn, ArrayList empList, bool IsIncludeCurrentPositionInfo)
        {
            return Export(dbConn, empList, IsIncludeCurrentPositionInfo, false, new DateTime());
        }
        public static DataTable Export(DatabaseConnection dbConn, ArrayList empList, bool IsIncludeCurrentPositionInfo, bool IsIncludeSyncID, DateTime ReferenceDateTime)
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

            tmpDataTable.Columns.Add(FIELD_BANK_CODE, typeof(string));
            tmpDataTable.Columns.Add(FIELD_BRANCH_CODE, typeof(string));
            tmpDataTable.Columns.Add(FIELD_ACC_NO, typeof(string));
            tmpDataTable.Columns.Add(FIELD_HOLDER_NAME, typeof(string));
            tmpDataTable.Columns.Add(FIELD_DEFAULTACCOUNT, typeof(string));
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
                    ArrayList list = EEmpBankAccount.db.select(dbConn, filter);
                    foreach (EEmpBankAccount empBankAccount in list)
                    {
                        DataRow row = tmpDataTable.NewRow();
                        //if (IsIncludeInternalID)
                        //    row[FIELD_INTERNAL_ID] = ToHexDecWithCheckDigit(empBankAccount.EmpBankAccountID);
                        row[FIELD_EMP_NO] = empInfo.EmpNo;

                        if (IsIncludeCurrentPositionInfo)
                        {
                            ImportEmpPersonalInfoProcess.AddEmployeeInfo(dbConn, row, empInfo.EmpID);
                            ImportEmpPositionInfoProcess.AddEmployeePositionInfo(dbConn, row, empInfo.EmpID);
                        }

                        row[FIELD_BANK_CODE] = empBankAccount.EmpBankCode;
                        row[FIELD_BRANCH_CODE] = empBankAccount.EmpBranchCode;
                        row[FIELD_ACC_NO] = empBankAccount.EmpAccountNo;
                        row[FIELD_HOLDER_NAME] = empBankAccount.EmpBankAccountHolderName;
                        row[FIELD_DEFAULTACCOUNT] = empBankAccount.EmpAccDefault ? "Yes" : "No";
                        row[FIELD_REMARK] = empBankAccount.EmpBankAccountRemark;

                        if (IsIncludeSyncID)
                            row[FIELD_SYNC_ID] = empBankAccount.SynID;

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
