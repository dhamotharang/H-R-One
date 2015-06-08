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
    public class ImportEmpContractTermsProcess : ImportProcessInteface
    {

        [DBClass("UploadEmpContractTerms")]
        private class EUploadEmpContractTerms : ImportDBObject
        {
            public static DBManager db = new DBManager(typeof(EUploadEmpContractTerms));

            protected int m_UploadEmpContractID;
            [DBField("UploadEmpContractID", true, true), TextSearch, Export(false)]
            public int UploadEmpContractID
            {
                get { return m_UploadEmpContractID; }
                set { m_UploadEmpContractID = value; modify("UploadEmpContractID"); }
            }
            protected int m_UploadEmpID;
            [DBField("UploadEmpID"), TextSearch, Export(false)]
            public int UploadEmpID
            {
                get { return m_UploadEmpID; }
                set { m_UploadEmpID = value; modify("UploadEmpID"); }
            }

            protected int m_EmpContractID;
            [DBField("EmpContractID"), TextSearch, Export(false)]
            public int EmpContractID
            {
                get { return m_EmpContractID; }
                set { m_EmpContractID = value; modify("EmpContractID"); }
            }
            protected int m_EmpID;
            [DBField("EmpID"), TextSearch, Export(false)]
            public int EmpID
            {
                get { return m_EmpID; }
                set { m_EmpID = value; modify("EmpID"); }
            }

            protected string m_EmpContractCompanyName;
            [DBField("EmpContractCompanyName"), TextSearch, MaxLength(100, 25), Export(false), Required]
            public string EmpContractCompanyName
            {
                get { return m_EmpContractCompanyName; }
                set { m_EmpContractCompanyName = value; modify("EmpContractCompanyName"); }
            }
            protected string m_EmpContractCompanyContactNo;
            [DBField("EmpContractCompanyContactNo"), TextSearch, MaxLength(100, 25), Export(false)]
            public string EmpContractCompanyContactNo
            {
                get { return m_EmpContractCompanyContactNo; }
                set { m_EmpContractCompanyContactNo = value; modify("EmpContractCompanyContactNo"); }
            }
            protected string m_EmpContractCompanyAddr;
            [DBField("EmpContractCompanyAddr"), TextSearch, MaxLength(100), Export(false)]
            public string EmpContractCompanyAddr
            {
                get { return m_EmpContractCompanyAddr; }
                set { m_EmpContractCompanyAddr = value; modify("EmpContractCompanyAddr"); }
            }
            protected DateTime m_EmpContractEmployedFrom;
            [DBField("EmpContractEmployedFrom"), TextSearch, MaxLength(25), Export(false)]
            public DateTime EmpContractEmployedFrom
            {
                get { return m_EmpContractEmployedFrom; }
                set { m_EmpContractEmployedFrom = value; modify("EmpContractEmployedFrom"); }
            }
            protected DateTime m_EmpContractEmployedTo;
            [DBField("EmpContractEmployedTo"), TextSearch, MaxLength(25), Export(false)]
            public DateTime EmpContractEmployedTo
            {
                get { return m_EmpContractEmployedTo; }
                set { m_EmpContractEmployedTo = value; modify("EmpContractEmployedTo"); }
            }
            protected double m_EmpContractGratuity;
            [DBField("EmpContractGratuity"), TextSearch, MaxLength(25), Export(false)]
            public double EmpContractGratuity
            {
                get { return m_EmpContractGratuity; }
                set { m_EmpContractGratuity = value; modify("EmpContractGratuity"); }
            }
            protected string m_CurrencyID;
            [DBField("CurrencyID"), TextSearch, Export(false)]
            public string CurrencyID
            {
                get { return m_CurrencyID; }
                set { m_CurrencyID = value; modify("CurrencyID"); }
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

        public const string TABLE_NAME = "contracts";

         //  Field Const for Employee Personal Information
        private const string FIELD_EMP_NO = "Emp No";
        private const string FIELD_FROM = "From";
        private const string FIELD_TO = "To";
        private const string FIELD_COMPANYNAME = "Company Name";
        private const string FIELD_COMPANYADDRESS = "Company Address";
        private const string FIELD_CONTRACT_NO = "Contract No";
        private const string FIELD_GRATUITY = "Gratuity";

        
        private DateTime UploadDateTime = AppUtils.ServerDateTime();
        private DBManager tempDB = EUploadEmpContractTerms.db;
        private DBManager uploadDB = EEmpContractTerms.db;

        public ImportErrorList errors = new ImportErrorList();
        

        public ImportEmpContractTermsProcess(DatabaseConnection dbConn, string SessionID): base(dbConn, SessionID)
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

                EUploadEmpContractTerms uploadEmpContract = new EUploadEmpContractTerms();
                //EEmpContractTerms lastEmpContact = null;

                string EmpNo = row[FIELD_EMP_NO].ToString().Trim();
                uploadEmpContract.EmpID = Parse.GetEmpID(dbConn, EmpNo, UserID);
                if (uploadEmpContract.EmpID < 0)
                    errors.addError(ImportErrorMessage.ERROR_ACCESS_DENIED_EMP_NO, new string[] { EmpNo, rowCount.ToString() });
                try
                {
                    uploadEmpContract.EmpContractEmployedFrom = Parse.toDateTimeObject(row[FIELD_FROM]);
                }
                catch
                {
                    errors.addError(ImportErrorMessage.ERROR_INVALID_FIELD_VALUE, new string[] { FIELD_FROM + "=" + row[FIELD_FROM].ToString(), EmpNo, rowCount.ToString() });
                }
                try
                {
                    uploadEmpContract.EmpContractEmployedTo = Parse.toDateTimeObject(row[FIELD_TO]);
                }
                catch
                {
                    errors.addError(ImportErrorMessage.ERROR_INVALID_FIELD_VALUE, new string[] { FIELD_TO + "=" + row[FIELD_TO].ToString(), EmpNo, rowCount.ToString() });
                }
                uploadEmpContract.EmpContractCompanyName = row[FIELD_COMPANYNAME].ToString();
                uploadEmpContract.EmpContractCompanyAddr = row[FIELD_COMPANYADDRESS].ToString();
                uploadEmpContract.EmpContractCompanyContactNo = row[FIELD_CONTRACT_NO].ToString();

                double amount = 0;
                if (row[FIELD_GRATUITY] != null)
                    if (!row[FIELD_GRATUITY].ToString().Equals(string.Empty))
                        if (double.TryParse(row[FIELD_GRATUITY].ToString(), out amount))
                            uploadEmpContract.EmpContractGratuity = amount;
                        else
                            errors.addError(ImportErrorMessage.ERROR_INVALID_FIELD_VALUE, new string[] { FIELD_GRATUITY + "=" + row[FIELD_GRATUITY].ToString(), EmpNo, rowCount.ToString() });



                uploadEmpContract.SessionID = m_SessionID;
                uploadEmpContract.TransactionDate = UploadDateTime;


                //if (uploadEmpContact.EmpID != 0)
                //{
                //    AND andTerms = new AND();
                //    andTerms.add(new Match("EmpContractEmployedFrom", "<=", uploadEmpContact.EmpContractEmployedFrom));

                //    lastEmpContact = (EEmpContractTerms)AppUtils.GetLastObj(dbConn, uploadDB, "EmpContractEmployedFrom", uploadEmpContact.EmpID, andTerms);
                //    if (lastEmpContact != null)
                //    {

                //        if (uploadEmpPay.EmpPoRNature == lastEmpPay.EmpPoRNature
                //            && uploadEmpPay.EmpPoRLandLord == lastEmpPay.EmpPoRLandLord
                //            && uploadEmpPay.EmpPoRPropertyAddr == lastEmpPay.EmpPoRPropertyAddr
                //            && uploadEmpPay.EmpPoRLandLordAddr == lastEmpPay.EmpPoRLandLordAddr
                //            )
                //        {
                //            continue;
                //        }
                //        else
                //        {
                //            // add postion terms with new ID
                //            if (lastEmpContact.EmpPoRFrom.Equals(uploadEmpContact.EmpPoRFrom))
                //            {
                //                uploadEmpContact.EmpPoRID = lastEmpContact.EmpPoRID;
                //                if (uploadEmpContact.EmpPoRTo.Ticks == 0)
                //                    uploadEmpContact.EmpPoRTo = lastEmpContact.EmpPoRTo;

                //            }
                //            else
                //            {
                //                EEmpContractTerms lastObj = (EEmpContractTerms)AppUtils.GetLastObj(dbConn, uploadDB, "EmpPoRFrom", uploadEmpContact.EmpID);
                //                if (lastObj != null && uploadEmpContact.EmpPoRFrom <= lastObj.EmpPoRFrom)
                //                {
                //                    errors.addError(HROne.Translation.PageErrorMessage.ERROR_DATE_FROM_OVERLAP, new string[] { uploadEmpContact.EmpPoRFrom.ToString("yyyy-MM-dd"), rowCount.ToString() });
                //                    continue;
                //                }
                //            }
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
                //            EEmpContractTerms tmpObj = new EEmpContractTerms();
                //            tmpObj.EmpContractID = tmpID;
                //            if (EEmpContractTerms.db.select(dbConn, tmpObj))
                //                uploadEmpContact.EmpContractID = tmpID;
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
                        uploadEmpContract.SynID = strSynID;
                        if (!string.IsNullOrEmpty(strSynID))
                        {
                            DBFilter synIDFilter = new DBFilter();
                            synIDFilter.add(new Match("SynID", strSynID));
                            ArrayList objSameSynIDList = EEmpContractTerms.db.select(dbConn, synIDFilter);
                            if (objSameSynIDList.Count > 0)
                                uploadEmpContract.EmpContractID = ((EEmpContractTerms)objSameSynIDList[0]).EmpContractID;
                        }
                    }

                }

                if (uploadEmpContract.EmpContractID <= 0)
                    uploadEmpContract.ImportActionStatus = ImportDBObject.ImportActionEnum.INSERT;
                else
                    uploadEmpContract.ImportActionStatus = ImportDBObject.ImportActionEnum.UPDATE;

                uploadEmpContract.UploadEmpID = ParseTemp.GetUploadEmpID(dbConn, EmpNo, m_SessionID);
                if (uploadEmpContract.UploadEmpID == 0)
                    if (uploadEmpContract.EmpID == 0)
                        errors.addError(ImportErrorMessage.ERROR_INVALID_EMP_NO, new string[] { EmpNo, rowCount.ToString() });
                    else
                        uploadEmpContract.UploadEmpID = ImportEmpPersonalInfoProcess.CreateDummyUploadEmployeeInfo(dbConn, uploadEmpContract.EmpID, m_SessionID, UploadDateTime);

                Hashtable values = new Hashtable();
                tempDB.populate(uploadEmpContract, values);
                PageErrors pageErrors = new PageErrors(EUploadEmpPersonalInfo.db);
                tempDB.validate(pageErrors, values);
                if (pageErrors.errors.Count == 0)
                {

                    tempDB.insert(dbConn, uploadEmpContract);
                }
                else
                {
                    pageErrors.addError(rawDataTable.TableName);
                    throw new HRImportException(pageErrors.getPrompt() + "(line " +rowCount.ToString() + ")");

                    //if (EmpID == 0)
                    //    errors.addError(HROne.Translation.PageErrorMessage.ERROR_INVALID_EMP_NO, new string[] { EmpNo, rowCount.ToString() });
                    //else if (PayCodeID == 0)
                    //    errors.addError(HROne.Translation.PageErrorMessage.ERROR_INVALID_FIELD_VALUE, new string[] { FIELD_PAYMENT_CODE + "=" + PaymentCode, EmpNo, rowCount.ToString() });
                    //else if (PayCodeID == 0)
                    //    errors.addError(HROne.Translation.PageErrorMessage.ERROR_INVALID_FIELD_VALUE, new string[] { FIELD_PAYMENT_CODE + "=" + PaymentCode, EmpNo, rowCount.ToString() });
                    //else if (EffDate.Ticks == 0)
                    //    errors.addError(HROne.Translation.PageErrorMessage.ERROR_INVALID_FIELD_VALUE, new string[] { FIELD_EFFECTIVE_DATE + "=" + EffDateString, EmpNo, rowCount.ToString() });
                    //else if (double.TryParse(amountString, out amount))
                    //    errors.addError(HROne.Translation.PageErrorMessage.ERROR_INVALID_FIELD_VALUE, new string[] { FIELD_AMOUNT + "=" + amountString, EmpNo, rowCount.ToString() });
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
            ImportEmpContractTermsProcess import = new ImportEmpContractTermsProcess(dbConn, sessionID);
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
            ArrayList uploadEmpContractList = tempDB.select(dbConn, sessionFilter);
            foreach (EUploadEmpContractTerms obj in uploadEmpContractList)
            {
                EEmpContractTerms empContract = new EEmpContractTerms();

                if (obj.ImportActionStatus != ImportDBObject.ImportActionEnum.INSERT)
                {
                    empContract.EmpContractID = obj.EmpContractID;
                    uploadDB.select(dbConn, empContract);
                }

                obj.ExportToObject(empContract);

                if (obj.ImportActionStatus == ImportDBObject.ImportActionEnum.INSERT)
                {
                    //DBFilter emplastPosFilter = new DBFilter();
                    //EEmpPlaceOfResidence lastObj = (EEmpPlaceOfResidence)AppUtils.GetLastObj(dbConn, uploadDB, "EmpPoRFrom", empContract.EmpID, new Match("EmpPoRFrom", "<", empContract.EmpPoRFrom));
                    //if (lastObj != null)
                    //    if (lastObj.EmpPoRTo.Ticks == 0)
                    //    {
                    //        lastObj.EmpPoRTo = empContract.EmpPoRFrom.AddDays(-1);
                    //        uploadDB.update(dbConn, lastObj);
                    //    }
                    empContract.EmpID = ParseTemp.GetEmpIDFromUploadEmpID(dbConn, obj.UploadEmpID);
                    uploadDB.insert(dbConn, empContract);

                }
                else if (obj.ImportActionStatus == ImportDBObject.ImportActionEnum.UPDATE)
                {
                    uploadDB.update(dbConn, empContract);

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

            tmpDataTable.Columns.Add(FIELD_FROM, typeof(DateTime));
            tmpDataTable.Columns.Add(FIELD_TO, typeof(DateTime));
            tmpDataTable.Columns.Add(FIELD_COMPANYNAME, typeof(string));
            tmpDataTable.Columns.Add(FIELD_COMPANYADDRESS, typeof(string));
            tmpDataTable.Columns.Add(FIELD_CONTRACT_NO, typeof(string));
            tmpDataTable.Columns.Add(FIELD_GRATUITY, typeof(double));
            if (IsIncludeSyncID)
                tmpDataTable.Columns.Add(FIELD_SYNC_ID, typeof(string));

            foreach (EEmpPersonalInfo empInfo in empList)
            {
                if (EEmpPersonalInfo.db.select(dbConn, empInfo))
                {
                    DBFilter filter = new DBFilter();
                    filter.add(new Match("EmpID", empInfo.EmpID));
                    filter.add(getCreateModifiedRecordsAfterDBTerm(ReferenceDateTime));
                    ArrayList list = EEmpContractTerms.db.select(dbConn, filter);
                    foreach (EEmpContractTerms empContract in list)
                    {
                        DataRow row = tmpDataTable.NewRow();
                        //if (IsIncludeInternalID)
                        //    row[FIELD_INTERNAL_ID] = ToHexDecWithCheckDigit(empContract.EmpContractID);
                        row[FIELD_EMP_NO] = empInfo.EmpNo;

                        if (IsIncludeCurrentPositionInfo)
                        {
                            ImportEmpPersonalInfoProcess.AddEmployeeInfo(dbConn, row, empInfo.EmpID);
                            ImportEmpPositionInfoProcess.AddEmployeePositionInfo(dbConn, row, empInfo.EmpID);
                        }

                        row[FIELD_FROM] = empContract.EmpContractEmployedFrom;
                        row[FIELD_TO] = empContract.EmpContractEmployedTo;
                        row[FIELD_COMPANYNAME] = empContract.EmpContractCompanyName;
                        row[FIELD_COMPANYADDRESS] = empContract.EmpContractCompanyAddr;
                        row[FIELD_CONTRACT_NO] = empContract.EmpContractCompanyContactNo;
                        row[FIELD_GRATUITY] = empContract.EmpContractGratuity;
                        if (IsIncludeSyncID)
                            row[FIELD_SYNC_ID] = empContract.SynID;

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
