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
    public class ImportEmpPlaceOfResidenceProcess : ImportProcessInteface
    {

        [DBClass("UploadEmpPlaceOfResidence")]
        private class EUploadEmpPlaceOfResidence : ImportDBObject
        {
            public static DBManager db = new DBManager(typeof(EUploadEmpPlaceOfResidence));

            protected int m_UploadEmpPoRID;
            [DBField("UploadEmpPoRID", true, true), TextSearch, Export(false)]
            public int UploadEmpPoRID
            {
                get { return m_UploadEmpPoRID; }
                set { m_UploadEmpPoRID = value; modify("UploadEmpPoRID"); }
            }
            protected int m_UploadEmpID;
            [DBField("UploadEmpID"), TextSearch, Export(false)]
            public int UploadEmpID
            {
                get { return m_UploadEmpID; }
                set { m_UploadEmpID = value; modify("UploadEmpID"); }
            }

            protected int m_EmpPoRID;
            [DBField("EmpPoRID"), TextSearch, Export(false)]
            public int EmpPoRID
            {
                get { return m_EmpPoRID; }
                set { m_EmpPoRID = value; modify("EmpPoRID"); }
            }
            protected int m_EmpID;
            [DBField("EmpID"), TextSearch, Export(false)]
            public int EmpID
            {
                get { return m_EmpID; }
                set { m_EmpID = value; modify("EmpID"); }
            }
            protected DateTime m_EmpPoRFrom;
            [DBField("EmpPoRFrom"), TextSearch, MaxLength(25), Export(false), Required]
            public DateTime EmpPoRFrom
            {
                get { return m_EmpPoRFrom; }
                set { m_EmpPoRFrom = value; modify("EmpPoRFrom"); }
            }
            protected DateTime m_EmpPoRTo;
            [DBField("EmpPoRTo"), TextSearch, MaxLength(25), Export(false)]
            public DateTime EmpPoRTo
            {
                get { return m_EmpPoRTo; }
                set { m_EmpPoRTo = value; modify("EmpPoRTo"); }
            }
            protected string m_EmpPoRLandLord;
            [DBField("EmpPoRLandLord"), TextSearch, MaxLength(100, 25), Export(false)]
            public string EmpPoRLandLord
            {
                get { return m_EmpPoRLandLord; }
                set { m_EmpPoRLandLord = value; modify("EmpPoRLandLord"); }
            }
            protected string m_EmpPoRLandLordAddr;
            [DBField("EmpPoRLandLordAddr"), TextSearch, MaxLength(110, 25), Export(false)]
            public string EmpPoRLandLordAddr
            {
                get { return m_EmpPoRLandLordAddr; }
                set { m_EmpPoRLandLordAddr = value; modify("EmpPoRLandLordAddr"); }
            }
            protected string m_EmpPoRPropertyAddr;
            [DBField("EmpPoRPropertyAddr"), TextSearch, MaxLength(110, 25), Export(false), Required]
            public string EmpPoRPropertyAddr
            {
                get { return m_EmpPoRPropertyAddr; }
                set { m_EmpPoRPropertyAddr = value; modify("EmpPoRPropertyAddr"); }
            }
            protected string m_EmpPoRNature;
            [DBField("EmpPoRNature"), TextSearch, MaxLength(19, 25), Export(false), Required]
            public string EmpPoRNature
            {
                get { return m_EmpPoRNature; }
                set { m_EmpPoRNature = value; modify("EmpPoRNature"); }
            }
            protected double m_EmpPoRPayToLandER;
            [DBField("EmpPoRPayToLandER", "0.00"), TextSearch, MaxLength(25), Export(false), Required]
            public double EmpPoRPayToLandER
            {
                get { return m_EmpPoRPayToLandER; }
                set { m_EmpPoRPayToLandER = value; modify("EmpPoRPayToLandER"); }
            }
            protected double m_EmpPoRPayToLandEE;
            [DBField("EmpPoRPayToLandEE", "0.00"), TextSearch, MaxLength(25), Export(false), Required]
            public double EmpPoRPayToLandEE
            {
                get { return m_EmpPoRPayToLandEE; }
                set { m_EmpPoRPayToLandEE = value; modify("EmpPoRPayToLandEE"); }
            }
            protected double m_EmpPoRRefundToEE;
            [DBField("EmpPoRRefundToEE", "0.00"), TextSearch, MaxLength(25), Export(false), Required]
            public double EmpPoRRefundToEE
            {
                get { return m_EmpPoRRefundToEE; }
                set { m_EmpPoRRefundToEE = value; modify("EmpPoRRefundToEE"); }
            }
            protected double m_EmpPoRPayToERByEE;
            [DBField("EmpPoRPayToERByEE", "0.00"), TextSearch, MaxLength(25), Export(false), Required]
            public double EmpPoRPayToERByEE
            {
                get { return m_EmpPoRPayToERByEE; }
                set { m_EmpPoRPayToERByEE = value; modify("EmpPoRPayToERByEE"); }
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

        public const string TABLE_NAME = "accomodation";

        //  Field Const for Employee Personal Information
        private const string FIELD_EMP_NO = "Emp No";
        private const string FIELD_FROM = "From";
        private const string FIELD_TO = "To";
        private const string FIELD_NATURE = "Nature";
        private const string FIELD_LANDLORD = "Landlord";
        private const string FIELD_ADDRESS = "Address";
        private const string FIELD_LANDLORD_ADDRESS = "Landlord Address";
        private const string FIELD_ER_RENT_PAID = "Rent Paid to Landlord by Employer";
        private const string FIELD_EE_RENT_PAID = "Rent Paid to Landlord by Employee";
        private const string FIELD_EE_RENT_REFUND = "Rent Refunded to Employee";
        private const string FIELD_EE_RENT_PAID_TO_ER = "Rent Paid to Employer by Employee";

        
        private DateTime UploadDateTime = AppUtils.ServerDateTime();
        private DBManager tempDB = EUploadEmpPlaceOfResidence.db;
        private DBManager uploadDB = EEmpPlaceOfResidence.db;

        public ImportErrorList errors = new ImportErrorList();
        

        public ImportEmpPlaceOfResidenceProcess(DatabaseConnection dbConn, string SessionID): base(dbConn, SessionID)
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

                EUploadEmpPlaceOfResidence uploadEmpPoR = new EUploadEmpPlaceOfResidence();
                EEmpPlaceOfResidence lastEmpPoR = null;

                string EmpNo = row[FIELD_EMP_NO].ToString().Trim();
                uploadEmpPoR.EmpID = Parse.GetEmpID(dbConn, EmpNo, UserID);
                if (uploadEmpPoR.EmpID < 0)
                    errors.addError(ImportErrorMessage.ERROR_ACCESS_DENIED_EMP_NO, new string[] { EmpNo, rowCount.ToString() });
                try
                {
                    uploadEmpPoR.EmpPoRFrom = Parse.toDateTimeObject(row[FIELD_FROM]);
                }
                catch
                {
                    errors.addError(ImportErrorMessage.ERROR_INVALID_FIELD_VALUE, new string[] { FIELD_FROM + "=" + row[FIELD_FROM].ToString(), EmpNo, rowCount.ToString() });
                }
                try
                {
                    uploadEmpPoR.EmpPoRTo = Parse.toDateTimeObject(row[FIELD_TO]);
                }
                catch
                {
                    errors.addError(ImportErrorMessage.ERROR_INVALID_FIELD_VALUE, new string[] { FIELD_TO + "=" + row[FIELD_TO].ToString(), EmpNo, rowCount.ToString() });
                }
                uploadEmpPoR.EmpPoRNature = row[FIELD_NATURE].ToString();
                uploadEmpPoR.EmpPoRLandLord = row[FIELD_LANDLORD].ToString();
                uploadEmpPoR.EmpPoRPropertyAddr = row[FIELD_ADDRESS].ToString();
                uploadEmpPoR.EmpPoRLandLordAddr = row[FIELD_LANDLORD_ADDRESS].ToString();

                double amount = 0;
                if (double.TryParse(row[FIELD_ER_RENT_PAID].ToString(), out amount))
                    uploadEmpPoR.EmpPoRPayToLandER = amount;
                else
                    errors.addError(ImportErrorMessage.ERROR_INVALID_FIELD_VALUE, new string[] { FIELD_ER_RENT_PAID + "=" + row[FIELD_ER_RENT_PAID].ToString(), EmpNo, rowCount.ToString() });

                if (double.TryParse(row[FIELD_EE_RENT_PAID].ToString(), out amount))
                    uploadEmpPoR.EmpPoRPayToLandEE = amount;
                else
                    errors.addError(ImportErrorMessage.ERROR_INVALID_FIELD_VALUE, new string[] { FIELD_EE_RENT_PAID + "=" + row[FIELD_EE_RENT_PAID].ToString(), EmpNo, rowCount.ToString() });

                if (double.TryParse(row[FIELD_EE_RENT_REFUND].ToString(), out amount))
                    uploadEmpPoR.EmpPoRRefundToEE = amount;
                else
                    errors.addError(ImportErrorMessage.ERROR_INVALID_FIELD_VALUE, new string[] { FIELD_EE_RENT_REFUND + "=" + row[FIELD_EE_RENT_REFUND].ToString(), EmpNo, rowCount.ToString() });

                if (double.TryParse(row[FIELD_EE_RENT_PAID_TO_ER].ToString(), out amount))
                    uploadEmpPoR.EmpPoRPayToERByEE = amount;
                else
                    errors.addError(ImportErrorMessage.ERROR_INVALID_FIELD_VALUE, new string[] { FIELD_EE_RENT_PAID_TO_ER + "=" + row[FIELD_EE_RENT_PAID_TO_ER].ToString(), EmpNo, rowCount.ToString() });

                //uploadEmpPoR.EmpPoRPayToLandER = double.Parse(row[FIELD_ER_RENT_PAID].ToString());
                //uploadEmpPoR.EmpPoRPayToLandEE = double.Parse(row[FIELD_EE_RENT_PAID].ToString());
                //uploadEmpPoR.EmpPoRRefundToEE  = double.Parse(row[FIELD_EE_RENT_REFUND].ToString());
                //uploadEmpPoR.EmpPoRPayToERByEE = double.Parse(row[FIELD_EE_RENT_PAID_TO_ER].ToString());


                uploadEmpPoR.SessionID = m_SessionID;
                uploadEmpPoR.TransactionDate = UploadDateTime;


                if (uploadEmpPoR.EmpID != 0)
                {
                    AND andTerms = new AND();
                    andTerms.add(new Match("EmpPoRFrom", "<=", uploadEmpPoR.EmpPoRFrom));

                    lastEmpPoR = (EEmpPlaceOfResidence)AppUtils.GetLastObj(dbConn, uploadDB, "EmpPoRFrom", uploadEmpPoR.EmpID, andTerms);
                    if (lastEmpPoR != null)
                    {

                        if (uploadEmpPoR.EmpPoRNature == lastEmpPoR.EmpPoRNature
                            && uploadEmpPoR.EmpPoRLandLord == lastEmpPoR.EmpPoRLandLord
                            && uploadEmpPoR.EmpPoRPropertyAddr == lastEmpPoR.EmpPoRPropertyAddr
                            && uploadEmpPoR.EmpPoRLandLordAddr == lastEmpPoR.EmpPoRLandLordAddr
                            && uploadEmpPoR.EmpPoRTo == lastEmpPoR.EmpPoRTo 
                            )
                        {
                            continue;
                        }
                        else
                        {
                            // add postion terms with new ID
                            if (lastEmpPoR.EmpPoRFrom.Equals(uploadEmpPoR.EmpPoRFrom))
                            {
                                uploadEmpPoR.EmpPoRID = lastEmpPoR.EmpPoRID;
                                if (uploadEmpPoR.EmpPoRTo.Ticks == 0 && lastEmpPoR.EmpPoRTo.Ticks != 0)
                                {
                                    EEmpPlaceOfResidence afterEmPoR = (EEmpPlaceOfResidence)AppUtils.GetLastObj(dbConn, uploadDB, "EmpPoRFrom", uploadEmpPoR.EmpID, new Match("EmpPoRFrom", ">", lastEmpPoR.EmpPoRTo));
                                    if (afterEmPoR != null)
                                    uploadEmpPoR.EmpPoRTo = afterEmPoR.EmpPoRFrom.AddDays(-1);
                                }

                            }
                            else
                            {
                                EEmpPlaceOfResidence lastObj = (EEmpPlaceOfResidence)AppUtils.GetLastObj(dbConn, uploadDB, "EmpPoRFrom", uploadEmpPoR.EmpID);
                                if (lastObj != null && uploadEmpPoR.EmpPoRFrom <= lastObj.EmpPoRFrom)
                                {
                                    errors.addError(ImportErrorMessage.ERROR_DATE_FROM_OVERLAP, new string[] { uploadEmpPoR.EmpPoRFrom.ToString("yyyy-MM-dd"), rowCount.ToString() });
                                    continue;
                                }
                            }
                        }
                    }
                }

                //if (rawDataTable.Columns.Contains(FIELD_INTERNAL_ID))
                //{
                //    try
                //    {
                //        if (!row.IsNull(FIELD_INTERNAL_ID))
                //        {
                //            int tmpID = FromHexDecWithCheckDigit((string)row[FIELD_INTERNAL_ID]);
                //            EEmpPlaceOfResidence tmpObj = new EEmpPlaceOfResidence();
                //            tmpObj.EmpPoRID = tmpID;
                //            if (EEmpPlaceOfResidence.db.select(dbConn, tmpObj))
                //                uploadEmpPoR.EmpPoRID = tmpID;
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
                        uploadEmpPoR.SynID = strSynID;
                        if (!string.IsNullOrEmpty(strSynID))
                        {
                            DBFilter synIDFilter = new DBFilter();
                            synIDFilter.add(new Match("SynID", strSynID));
                            ArrayList objSameSynIDList = EEmpPlaceOfResidence.db.select(dbConn, synIDFilter);
                            if (objSameSynIDList.Count > 0)
                                uploadEmpPoR.EmpPoRID = ((EEmpPlaceOfResidence)objSameSynIDList[0]).EmpPoRID;
                        }
                    }

                }

                if (uploadEmpPoR.EmpPoRID <= 0)
                    uploadEmpPoR.ImportActionStatus = ImportDBObject.ImportActionEnum.INSERT;
                else
                    uploadEmpPoR.ImportActionStatus = ImportDBObject.ImportActionEnum.UPDATE;

                uploadEmpPoR.UploadEmpID = ParseTemp.GetUploadEmpID(dbConn, EmpNo, m_SessionID);
                if (uploadEmpPoR.UploadEmpID == 0)
                    if (uploadEmpPoR.EmpID == 0)
                        errors.addError(ImportErrorMessage.ERROR_INVALID_EMP_NO, new string[] { EmpNo, rowCount.ToString() });
                    else
                        uploadEmpPoR.UploadEmpID = ImportEmpPersonalInfoProcess.CreateDummyUploadEmployeeInfo(dbConn, uploadEmpPoR.EmpID, m_SessionID, UploadDateTime);

                Hashtable values = new Hashtable();
                tempDB.populate(uploadEmpPoR, values);
                PageErrors pageErrors = new PageErrors(EUploadEmpPersonalInfo.db);
                tempDB.validate(pageErrors, values);
                if (pageErrors.errors.Count == 0)
                {

                    tempDB.insert(dbConn, uploadEmpPoR);
                }
                else
                {
                    pageErrors.addError(rawDataTable.TableName);
                    throw new HRImportException(pageErrors.getPrompt() + "(line " +rowCount.ToString() + ")");

                    //if (EmpID == 0)
                    //    errors.addError(ImportErrorMessage.ERROR_INVALID_EMP_NO, new string[] { EmpNo, rowCount.ToString() });
                    //else if (PayCodeID == 0)
                    //    errors.addError(ImportErrorMessage.ERROR_INVALID_FIELD_VALUE, new string[] { FIELD_PAYMENT_CODE + "=" + PaymentCode, EmpNo, rowCount.ToString() });
                    //else if (PayCodeID == 0)
                    //    errors.addError(ImportErrorMessage.ERROR_INVALID_FIELD_VALUE, new string[] { FIELD_PAYMENT_CODE + "=" + PaymentCode, EmpNo, rowCount.ToString() });
                    //else if (EffDate.Ticks == 0)
                    //    errors.addError(ImportErrorMessage.ERROR_INVALID_FIELD_VALUE, new string[] { FIELD_EFFECTIVE_DATE + "=" + EffDateString, EmpNo, rowCount.ToString() });
                    //else if (double.TryParse(amountString, out amount))
                    //    errors.addError(ImportErrorMessage.ERROR_INVALID_FIELD_VALUE, new string[] { FIELD_AMOUNT + "=" + amountString, EmpNo, rowCount.ToString() });
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
            ImportEmpPositionInfoProcess import = new ImportEmpPositionInfoProcess(dbConn, sessionID);
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
            ArrayList uploadEmpPoRList = tempDB.select(dbConn, sessionFilter);
            foreach (EUploadEmpPlaceOfResidence obj in uploadEmpPoRList)
            {
                EEmpPlaceOfResidence empPoR = new EEmpPlaceOfResidence();

                if (obj.ImportActionStatus != ImportDBObject.ImportActionEnum.INSERT)
                {
                    empPoR.EmpPoRID = obj.EmpPoRID;
                    uploadDB.select(dbConn, empPoR);
                }

                obj.ExportToObject(empPoR);

                if (obj.ImportActionStatus == ImportDBObject.ImportActionEnum.INSERT)
                {
                    DBFilter emplastPosFilter = new DBFilter();
                    EEmpPlaceOfResidence lastObj = (EEmpPlaceOfResidence)AppUtils.GetLastObj(dbConn, uploadDB, "EmpPoRFrom", empPoR.EmpID, new Match("EmpPoRFrom", "<", empPoR.EmpPoRFrom));
                    if (lastObj != null)
                        if (lastObj.EmpPoRTo.Ticks == 0)
                        {
                            lastObj.EmpPoRTo = empPoR.EmpPoRFrom.AddDays(-1);
                            uploadDB.update(dbConn, lastObj);
                        }
                    empPoR.EmpID = ParseTemp.GetEmpIDFromUploadEmpID(dbConn, obj.UploadEmpID);
                    uploadDB.insert(dbConn, empPoR);

                }
                else if (obj.ImportActionStatus == ImportDBObject.ImportActionEnum.UPDATE)
                {
                    uploadDB.update(dbConn, empPoR);

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
            tmpDataTable.Columns.Add(FIELD_NATURE, typeof(string));
            tmpDataTable.Columns.Add(FIELD_LANDLORD, typeof(string));
            tmpDataTable.Columns.Add(FIELD_ADDRESS, typeof(string));
            tmpDataTable.Columns.Add(FIELD_LANDLORD_ADDRESS, typeof(string));
            tmpDataTable.Columns.Add(FIELD_ER_RENT_PAID, typeof(double));
            tmpDataTable.Columns.Add(FIELD_EE_RENT_PAID, typeof(double));
            tmpDataTable.Columns.Add(FIELD_EE_RENT_REFUND, typeof(double));
            tmpDataTable.Columns.Add(FIELD_EE_RENT_PAID_TO_ER, typeof(double));

            if (IsIncludeSyncID)
                tmpDataTable.Columns.Add(FIELD_SYNC_ID, typeof(string));

            foreach (EEmpPersonalInfo empInfo in empList)
            {
                if (EEmpPersonalInfo.db.select(dbConn, empInfo))
                {
                    DBFilter filter = new DBFilter();
                    filter.add(new Match("EmpID", empInfo.EmpID));
                    filter.add(getCreateModifiedRecordsAfterDBTerm(ReferenceDateTime));
                    ArrayList list = EEmpPlaceOfResidence.db.select(dbConn, filter);
                    foreach (EEmpPlaceOfResidence empPlaceOfResidence in list)
                    {
                        DataRow row = tmpDataTable.NewRow();
                        //if (IsIncludeInternalID)
                        //    row[FIELD_INTERNAL_ID] = ToHexDecWithCheckDigit(empPlaceOfResidence.EmpPoRID);
                        row[FIELD_EMP_NO] = empInfo.EmpNo;

                        if (IsIncludeCurrentPositionInfo)
                        {
                            ImportEmpPersonalInfoProcess.AddEmployeeInfo(dbConn, row, empInfo.EmpID);
                            ImportEmpPositionInfoProcess.AddEmployeePositionInfo(dbConn, row, empInfo.EmpID);
                        }

                        row[FIELD_FROM] = empPlaceOfResidence.EmpPoRFrom;
                        row[FIELD_TO] = empPlaceOfResidence.EmpPoRTo;
                        row[FIELD_NATURE] = empPlaceOfResidence.EmpPoRNature;
                        row[FIELD_LANDLORD] = empPlaceOfResidence.EmpPoRLandLord;
                        row[FIELD_ADDRESS] = empPlaceOfResidence.EmpPoRPropertyAddr;
                        row[FIELD_LANDLORD_ADDRESS] = empPlaceOfResidence.EmpPoRLandLordAddr;
                        row[FIELD_ER_RENT_PAID] = empPlaceOfResidence.EmpPoRPayToLandER;
                        row[FIELD_EE_RENT_PAID] = empPlaceOfResidence.EmpPoRPayToLandEE;
                        row[FIELD_EE_RENT_REFUND] = empPlaceOfResidence.EmpPoRRefundToEE;
                        row[FIELD_EE_RENT_PAID_TO_ER] = empPlaceOfResidence.EmpPoRPayToERByEE;

                        if (IsIncludeSyncID)
                            row[FIELD_SYNC_ID] = empPlaceOfResidence.SynID;

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
