using System;
using System.Collections.Generic;
using System.Text;
using HROne.DataAccess;
//using perspectivemind.validation;
using System.Data;
using HROne.Lib.Entities;
using System.Globalization;
using System.Collections;

namespace HROne.Import
{


    public class ImportCompensationLeaveEntitleProcess : ImportProcessInteface
    {
        [DBClass("UploadCompensationLeaveEntitle")]
        protected class EUploadCompensationLeaveEntitle : ImportDBObject
        {
            public static DBManager db = new DBManager(typeof(EUploadCompensationLeaveEntitle));

            protected int m_UploadCompensationLeaveEntitleID;
            [DBField("UploadCompensationLeaveEntitleID", true, true), TextSearch, Export(false)]
            public int UploadCompensationLeaveEntitleID
            {
                get { return m_UploadCompensationLeaveEntitleID; }
                set { m_UploadCompensationLeaveEntitleID = value; modify("UploadCompensationLeaveEntitleID"); }
            }
            protected int m_UploadEmpID;
            [DBField("UploadEmpID"), TextSearch, Export(false)]
            public int UploadEmpID
            {
                get { return m_UploadEmpID; }
                set { m_UploadEmpID = value; modify("UploadEmpID"); }
            }

            protected int m_CompensationLeaveEntitleID;
            [DBField("CompensationLeaveEntitleID"), TextSearch, Export(false)]
            public int CompensationLeaveEntitleID
            {
                get { return m_CompensationLeaveEntitleID; }
                set { m_CompensationLeaveEntitleID = value; modify("CompensationLeaveEntitleID"); }
            }

            protected int m_EmpID;
            [DBField("EmpID"), TextSearch, Export(false), Required]
            public int EmpID
            {
                get { return m_EmpID; }
                set { m_EmpID = value; modify("EmpID"); }
            }

            protected DateTime m_CompensationLeaveEntitleEffectiveDate;
            [DBField("CompensationLeaveEntitleEffectiveDate"), TextSearch, Export(false), Required]
            public DateTime CompensationLeaveEntitleEffectiveDate
            {
                get { return m_CompensationLeaveEntitleEffectiveDate; }
                set { m_CompensationLeaveEntitleEffectiveDate = value; modify("CompensationLeaveEntitleEffectiveDate"); }
            }

            //protected string m_CompensationLeaveEntitleClaimType;
            //[DBField("CompensationLeaveEntitleType"), TextSearch, Export(false), Required]
            //public string CompensationLeaveEntitleType
            //{
            //    get { return m_CompensationLeaveEntitleClaimType; }
            //    set { m_CompensationLeaveEntitleClaimType = value; modify("CompensationLeaveEntitleType"); }
            //}

            protected DateTime m_CompensationLeaveEntitleClaimPeriodFrom;
            [DBField("CompensationLeaveEntitleClaimPeriodFrom"), TextSearch, MaxLength(10), Export(false), Required]
            public DateTime CompensationLeaveEntitleClaimPeriodFrom
            {
                get { return m_CompensationLeaveEntitleClaimPeriodFrom; }
                set { m_CompensationLeaveEntitleClaimPeriodFrom = value; modify("CompensationLeaveEntitleClaimPeriodFrom"); }
            }

            protected DateTime m_CompensationLeaveEntitleClaimPeriodTo;
            [DBField("CompensationLeaveEntitleClaimPeriodTo"), TextSearch, MaxLength(10), Export(false), Required]
            public DateTime CompensationLeaveEntitleClaimPeriodTo
            {
                get { return m_CompensationLeaveEntitleClaimPeriodTo; }
                set { m_CompensationLeaveEntitleClaimPeriodTo = value; modify("CompensationLeaveEntitleClaimPeriodTo"); }
            }

            protected DateTime m_CompensationLeaveEntitleClaimHourFrom;
            [DBField("CompensationLeaveEntitleClaimHourFrom", "HH:mm"), TextSearch, MaxLength(5), Export(false)]
            public DateTime CompensationLeaveEntitleClaimHourFrom
            {
                get { return m_CompensationLeaveEntitleClaimHourFrom; }
                set { m_CompensationLeaveEntitleClaimHourFrom = value; modify("CompensationLeaveEntitleClaimHourFrom"); }
            }

            protected DateTime m_CompensationLeaveEntitleClaimHourTo;
            [DBField("CompensationLeaveEntitleClaimHourTo", "HH:mm"), TextSearch, MaxLength(5), Export(false)]
            public DateTime CompensationLeaveEntitleClaimHourTo
            {
                get { return m_CompensationLeaveEntitleClaimHourTo; }
                set { m_CompensationLeaveEntitleClaimHourTo = value; modify("CompensationLeaveEntitleClaimHourTo"); }
            }

            protected double m_CompensationLeaveEntitleHoursClaim;
            [DBField("CompensationLeaveEntitleHoursClaim", "0.####"), MaxLength(7), TextSearch, Export(false), Required]
            public double CompensationLeaveEntitleHoursClaim
            {
                get { return m_CompensationLeaveEntitleHoursClaim; }
                set { m_CompensationLeaveEntitleHoursClaim = value; modify("CompensationLeaveEntitleHoursClaim"); }
            }

            protected string m_CompensationLeaveEntitleApprovedBy;
            [DBField("CompensationLeaveEntitleApprovedBy"), TextSearch, Export(false)]
            public string CompensationLeaveEntitleApprovedBy
            {
                get { return m_CompensationLeaveEntitleApprovedBy; }
                set { m_CompensationLeaveEntitleApprovedBy = value; modify("CompensationLeaveEntitleApprovedBy"); }
            }

            protected string m_CompensationLeaveEntitleRemark;
            [DBField("CompensationLeaveEntitleRemark"), TextSearch, Export(false)]
            public string CompensationLeaveEntitleRemark
            {
                get { return m_CompensationLeaveEntitleRemark; }
                set { m_CompensationLeaveEntitleRemark = value; modify("CompensationLeaveEntitleRemark"); }
            }

            protected bool m_CompensationLeaveEntitleIsAutoGenerated;
            [DBField("CompensationLeaveEntitleIsAutoGenerated"), TextSearch, Export(false)]
            public bool CompensationLeaveEntitleIsAutoGenerated
            {
                get { return m_CompensationLeaveEntitleIsAutoGenerated; }
                set { m_CompensationLeaveEntitleIsAutoGenerated = value; modify("CompensationLeaveEntitleIsAutoGenerated"); }
            }

            protected DateTime m_CompensationLeaveEntitleDateExpiry;
            [DBField("CompensationLeaveEntitleDateExpiry"), TextSearch, MaxLength(10), Export(false)]
            public DateTime CompensationLeaveEntitleDateExpiry
            {
                get { return m_CompensationLeaveEntitleDateExpiry; }
                set { m_CompensationLeaveEntitleDateExpiry = value; modify("CompensationLeaveEntitleDateExpiry"); }
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

        public const string TABLE_NAME = "CompensatoinLeaveEntitle";

        //  Field Const for Employee Personal Information
        private const string FIELD_EMP_NO = "Emp No";
        private const string FIELD_EFFECTIVE_DATE = "Effective Date";
        private const string FIELD_CLAIM_PERIOD_FROM = "Period From";
        private const string FIELD_CLAIM_PERIOD_TO = "Period To";
        private const string FIELD_CLAIM_TIME_FROM = "Time From";
        private const string FIELD_CLAIM_TIME_TO = "Time To";
        private const string FIELD_HOUR_CLAIM = "Hours Claim";
        private const string FIELD_EXPIRY_DATE = "Date Expiry";
        private const string FIELD_APPROVED_BY = "Approved By";
        private const string FIELD_REMARK = "Remark";

        
        private DateTime UploadDateTime = AppUtils.ServerDateTime();
        private DBManager tempDB = EUploadCompensationLeaveEntitle.db;
        private DBManager uploadDB = ECompensationLeaveEntitle.db;

        public ImportErrorList errors = new ImportErrorList();


        public ImportCompensationLeaveEntitleProcess(DatabaseConnection dbConn, string SessionID): base(dbConn, SessionID)
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

                EUploadCompensationLeaveEntitle uploadCompensationLeaveEntitle = new EUploadCompensationLeaveEntitle();
                //EEmpAVCPlan lastEmpAVC = null;
                //ArrayList uploadHierarchyList = new ArrayList();

                string EmpNo = row[FIELD_EMP_NO].ToString().Trim();
                uploadCompensationLeaveEntitle.EmpID = Parse.GetEmpID(dbConn, EmpNo, UserID);
                if (uploadCompensationLeaveEntitle.EmpID < 0)
                    errors.addError(ImportErrorMessage.ERROR_ACCESS_DENIED_EMP_NO, new string[] { EmpNo, rowCount.ToString() });
                try
                {
                    uploadCompensationLeaveEntitle.CompensationLeaveEntitleEffectiveDate = Parse.toDateTimeObject(row[FIELD_EFFECTIVE_DATE]);
                }
                catch (Exception)
                {
                    errors.addError(ImportErrorMessage.ERROR_INVALID_FIELD_VALUE, new string[] { FIELD_EFFECTIVE_DATE + "=" + row[FIELD_EFFECTIVE_DATE].ToString(), EmpNo, rowCount.ToString() });
                }
                try
                {
                    uploadCompensationLeaveEntitle.CompensationLeaveEntitleClaimPeriodFrom = Parse.toDateTimeObject(row[FIELD_CLAIM_PERIOD_FROM]);
                }
                catch (Exception)
                {
                    errors.addError(ImportErrorMessage.ERROR_INVALID_FIELD_VALUE, new string[] { FIELD_CLAIM_PERIOD_FROM + "=" + row[FIELD_CLAIM_PERIOD_FROM].ToString(), EmpNo, rowCount.ToString() });
                }
                try
                {
                    uploadCompensationLeaveEntitle.CompensationLeaveEntitleClaimPeriodTo = Parse.toDateTimeObject(row[FIELD_CLAIM_PERIOD_TO]);
                }
                catch (Exception)
                {
                    errors.addError(ImportErrorMessage.ERROR_INVALID_FIELD_VALUE, new string[] { FIELD_CLAIM_PERIOD_TO + "=" + row[FIELD_CLAIM_PERIOD_TO].ToString(), EmpNo, rowCount.ToString() });
                }

                try
                {
                    uploadCompensationLeaveEntitle.CompensationLeaveEntitleClaimHourFrom = Parse.toDateTimeObject(row[FIELD_CLAIM_TIME_FROM]);
                }
                catch (Exception)
                {
                    errors.addError(ImportErrorMessage.ERROR_INVALID_FIELD_VALUE, new string[] { FIELD_CLAIM_TIME_FROM + "=" + row[FIELD_CLAIM_TIME_FROM].ToString(), EmpNo, rowCount.ToString() });
                }

                try
                {
                    uploadCompensationLeaveEntitle.CompensationLeaveEntitleClaimHourTo = Parse.toDateTimeObject(row[FIELD_CLAIM_TIME_TO]);
                }
                catch (Exception)
                {
                    errors.addError(ImportErrorMessage.ERROR_INVALID_FIELD_VALUE, new string[] { FIELD_CLAIM_TIME_TO + "=" + row[FIELD_CLAIM_TIME_TO].ToString(), EmpNo, rowCount.ToString() });
                }

                double adjustValue;
                if (double.TryParse(row[FIELD_HOUR_CLAIM].ToString(), out adjustValue))
                    uploadCompensationLeaveEntitle.CompensationLeaveEntitleHoursClaim = adjustValue;
                else
                    errors.addError(ImportErrorMessage.ERROR_INVALID_FIELD_VALUE, new string[] { FIELD_HOUR_CLAIM + "=" + row[FIELD_HOUR_CLAIM].ToString(), EmpNo, rowCount.ToString() });

                try
                {
                    uploadCompensationLeaveEntitle.CompensationLeaveEntitleDateExpiry = Parse.toDateTimeObject(row[FIELD_EXPIRY_DATE]);
                }
                catch (Exception)
                {
                    errors.addError(ImportErrorMessage.ERROR_INVALID_FIELD_VALUE, new string[] { FIELD_EXPIRY_DATE + "=" + row[FIELD_EXPIRY_DATE].ToString(), EmpNo, rowCount.ToString() });
                }

                uploadCompensationLeaveEntitle.CompensationLeaveEntitleApprovedBy = row[FIELD_APPROVED_BY].ToString();

                uploadCompensationLeaveEntitle.CompensationLeaveEntitleRemark = row[FIELD_REMARK].ToString();

                uploadCompensationLeaveEntitle.SessionID = m_SessionID;
                uploadCompensationLeaveEntitle.TransactionDate = UploadDateTime;


                if (uploadCompensationLeaveEntitle.EmpID != 0 && errors.List.Count <= 0)
                {
                    if (rawDataTable.Columns.Contains(FIELD_SYNC_ID))
                    {
                        if (!row.IsNull(FIELD_SYNC_ID))
                        {
                            string strSynID = row[FIELD_SYNC_ID].ToString();
                            uploadCompensationLeaveEntitle.SynID = strSynID;
                            if (!string.IsNullOrEmpty(strSynID))
                            {
                                DBFilter synIDFilter = new DBFilter();
                                synIDFilter.add(new Match("SynID", strSynID));
                                ArrayList objSameSynIDList = ELeaveApplication.db.select(dbConn, synIDFilter);
                                if (objSameSynIDList.Count > 0)
                                    uploadCompensationLeaveEntitle.CompensationLeaveEntitleID = ((ECompensationLeaveEntitle)objSameSynIDList[0]).CompensationLeaveEntitleID;
                            }
                        }

                    }

                    if (uploadCompensationLeaveEntitle.CompensationLeaveEntitleID == 0)
                    {
                        AND andTerm = new AND();
                        andTerm.add(new Match("CompensationLeaveEntitleEffectiveDate", "=", uploadCompensationLeaveEntitle.CompensationLeaveEntitleEffectiveDate));
                        ECompensationLeaveEntitle lastObject = (ECompensationLeaveEntitle)AppUtils.GetLastObj(dbConn, uploadDB, "CompensationLeaveEntitleEffectiveDate", uploadCompensationLeaveEntitle.EmpID, andTerm);
                        if (lastObject != null)
                        {
                            uploadCompensationLeaveEntitle.CompensationLeaveEntitleID = lastObject.CompensationLeaveEntitleID;
                        }
                    }
                }

                if (uploadCompensationLeaveEntitle.CompensationLeaveEntitleID <= 0)
                    uploadCompensationLeaveEntitle.ImportActionStatus = ImportDBObject.ImportActionEnum.INSERT;
                else
                    uploadCompensationLeaveEntitle.ImportActionStatus = ImportDBObject.ImportActionEnum.UPDATE;

                uploadCompensationLeaveEntitle.UploadEmpID = ParseTemp.GetUploadEmpID(dbConn, EmpNo, m_SessionID);
                if (uploadCompensationLeaveEntitle.UploadEmpID == 0)
                    if (uploadCompensationLeaveEntitle.EmpID == 0)
                        errors.addError(ImportErrorMessage.ERROR_INVALID_EMP_NO, new string[] { EmpNo, rowCount.ToString() });
                    else
                        uploadCompensationLeaveEntitle.UploadEmpID = ImportEmpPersonalInfoProcess.CreateDummyUploadEmployeeInfo(dbConn, uploadCompensationLeaveEntitle.EmpID, m_SessionID, UploadDateTime);

                Hashtable values = new Hashtable();
                tempDB.populate(uploadCompensationLeaveEntitle, values);
                PageErrors pageErrors = new PageErrors(tempDB);
                tempDB.validate(pageErrors, values);
                if (pageErrors.errors.Count == 0)
                {

                    tempDB.insert(dbConn, uploadCompensationLeaveEntitle);
                }
                else
                {
                    pageErrors.addError(rawDataTable.TableName);
                    throw new HRImportException(pageErrors.getPrompt() + "(line " + rowCount.ToString() + ")");

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
                throw (new HRImportException(rawDataTable.TableName + "\r\n" + errors.Message()));
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
            sessionFilter.add(new MatchField("e.EmpID", "c.EmpID"));
            //if (info != null && info.orderby != null && !info.orderby.Equals(""))
            //    sessionFilter.add(info.orderby, info.order);
            DataTable result = sessionFilter.loadData(dbConn, null, "e.*, c.* ", " from " + tempDB.dbclass.tableName + " c, " + EEmpPersonalInfo.db.dbclass.tableName + " e");


            return result;

        }

        public static void ClearTempTable(DatabaseConnection dbConn, string sessionID)
        {
            ImportLeaveBalanceAdjustmentProcess import = new ImportLeaveBalanceAdjustmentProcess(dbConn, sessionID);
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
            ArrayList uploadList = tempDB.select(dbConn, sessionFilter);

            foreach (EUploadCompensationLeaveEntitle obj in uploadList)
            {
                ECompensationLeaveEntitle compLeaveEntitle = new ECompensationLeaveEntitle();

                if (obj.ImportActionStatus != ImportDBObject.ImportActionEnum.INSERT)
                {
                    compLeaveEntitle.CompensationLeaveEntitleID = obj.CompensationLeaveEntitleID;
                    uploadDB.select(dbConn, compLeaveEntitle);
                }

                obj.ExportToObject(compLeaveEntitle);

                if (obj.ImportActionStatus == ImportDBObject.ImportActionEnum.INSERT)
                {
                    compLeaveEntitle.EmpID = ParseTemp.GetEmpIDFromUploadEmpID(dbConn, obj.UploadEmpID);
                    uploadDB.insert(dbConn, compLeaveEntitle);


                }
                else if (obj.ImportActionStatus == ImportDBObject.ImportActionEnum.UPDATE)
                {
                    uploadDB.update(dbConn, compLeaveEntitle);
                }

                tempDB.delete(dbConn, obj);
            }
        }

        public static DataTable Export(DatabaseConnection dbConn, ArrayList empList, bool IsIncludeCurrentPositionInfo, bool IsShowDescription, bool IsIncludeSyncID, DateTime ReferenceDateTime)
        {
            DataTable tmpDataTable = new DataTable(TABLE_NAME);
            tmpDataTable.Columns.Add(FIELD_EMP_NO, typeof(string));

            if (IsIncludeCurrentPositionInfo)
            {
                ImportEmpPersonalInfoProcess.AddEmployeeInfoHeader(tmpDataTable);
                ImportEmpPositionInfoProcess.AddEmployeePositionInfoHeader(dbConn, tmpDataTable);
            }
            tmpDataTable.Columns.Add(FIELD_EFFECTIVE_DATE, typeof(DateTime));
            tmpDataTable.Columns.Add(FIELD_CLAIM_PERIOD_FROM, typeof(DateTime));
            tmpDataTable.Columns.Add(FIELD_CLAIM_PERIOD_TO, typeof(DateTime));
            tmpDataTable.Columns.Add(FIELD_CLAIM_TIME_FROM, typeof(string));
            tmpDataTable.Columns.Add(FIELD_CLAIM_TIME_TO, typeof(string));
            tmpDataTable.Columns.Add(FIELD_HOUR_CLAIM, typeof(double));
            tmpDataTable.Columns.Add(FIELD_EXPIRY_DATE, typeof(DateTime));
            tmpDataTable.Columns.Add(FIELD_APPROVED_BY, typeof(string));
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
                    ArrayList list = ECompensationLeaveEntitle.db.select(dbConn, filter);
                    foreach (ECompensationLeaveEntitle compLeaveEntitle in list)
                    {
                        DataRow row = tmpDataTable.NewRow();
                        row[FIELD_EMP_NO] = empInfo.EmpNo;

                        if (IsIncludeCurrentPositionInfo)
                        {
                            ImportEmpPersonalInfoProcess.AddEmployeeInfo(dbConn, row, empInfo.EmpID);
                            ImportEmpPositionInfoProcess.AddEmployeePositionInfo(dbConn, row, empInfo.EmpID);
                        }

                        row[FIELD_EFFECTIVE_DATE] = compLeaveEntitle.CompensationLeaveEntitleEffectiveDate;
                        row[FIELD_CLAIM_PERIOD_FROM] = compLeaveEntitle.CompensationLeaveEntitleClaimPeriodFrom;
                        row[FIELD_CLAIM_PERIOD_TO] = compLeaveEntitle.CompensationLeaveEntitleClaimPeriodTo;
                        if (!compLeaveEntitle.CompensationLeaveEntitleClaimHourFrom.Ticks.Equals(0))
                            row[FIELD_CLAIM_TIME_FROM] = compLeaveEntitle.CompensationLeaveEntitleClaimHourFrom.ToString("HH:mm");
                        if (!compLeaveEntitle.CompensationLeaveEntitleClaimHourTo.Ticks.Equals(0))
                            row[FIELD_CLAIM_TIME_TO] = compLeaveEntitle.CompensationLeaveEntitleClaimHourTo.ToString("HH:mm");
                        row[FIELD_HOUR_CLAIM] = compLeaveEntitle.CompensationLeaveEntitleHoursClaim;
                        row[FIELD_EXPIRY_DATE] = compLeaveEntitle.CompensationLeaveEntitleDateExpiry;
                        row[FIELD_APPROVED_BY] = compLeaveEntitle.CompensationLeaveEntitleApprovedBy;
                        row[FIELD_REMARK] = compLeaveEntitle.CompensationLeaveEntitleRemark;

                        if (IsIncludeSyncID)
                            row[FIELD_SYNC_ID] = compLeaveEntitle.SynID;


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
