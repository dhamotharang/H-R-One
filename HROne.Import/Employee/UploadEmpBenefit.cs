using System;
using System.Collections;
using System.Globalization;
using System.Data;
using System.Text;
using HROne.DataAccess;
using HROne.Lib.Entities;

namespace HROne.Import
{
    public class ImportEmpBenefitProcess : ImportProcessInteface
    {

        [DBClass("UploadEmpBenefit")]
        private class EUploadEmpBenefit : ImportDBObject
        {
            public static DBManager db = new DBManager(typeof(EUploadEmpBenefit));

            protected int m_UploadEmpBenefitID;
            [DBField("UploadEmpBenefitID", true, true), TextSearch, Export(false)]
            public int UploadEmpBenefitID
            {
                get { return m_UploadEmpBenefitID; }
                set { m_UploadEmpBenefitID = value; modify("UploadEmpBenefitID"); }
            }
            protected int m_UploadEmpID;
            [DBField("UploadEmpID"), TextSearch, Export(false)]
            public int UploadEmpID
            {
                get { return m_UploadEmpID; }
                set { m_UploadEmpID = value; modify("UploadEmpID"); }
            }

            protected int m_EmpBenefitID;
            [DBField("EmpBenefitID"), TextSearch, Export(false)]
            public int EmpBenefitID
            {
                get { return m_EmpBenefitID; }
                set { m_EmpBenefitID = value; modify("EmpBenefitID"); }
            }
            protected int m_EmpID;
            [DBField("EmpID"), TextSearch, Export(false)]
            public int EmpID
            {
                get { return m_EmpID; }
                set { m_EmpID = value; modify("EmpID"); }
            }

            protected DateTime m_EmpBenefitEffectiveDate;
            [DBField("EmpBenefitEffectiveDate"), TextSearch, MaxLength(25), Export(false), Required]
            public DateTime EmpBenefitEffectiveDate
            {
                get { return m_EmpBenefitEffectiveDate; }
                set { m_EmpBenefitEffectiveDate = value; base.modify("EmpBenefitEffectiveDate"); }
            }

            protected DateTime m_EmpBenefitExpiryDate;
            [DBField("EmpBenefitExpiryDate"), TextSearch, MaxLength(25), Export(false)]
            public DateTime EmpBenefitExpiryDate
            {
                get { return m_EmpBenefitExpiryDate; }
                set { m_EmpBenefitExpiryDate = value; modify("EmpBenefitExpiryDate"); }
            }

            protected int m_EmpBenefitPlanID;
            [DBField("EmpBenefitPlanID"), TextSearch, Export(false)]
            public int EmpBenefitPlanID
            {
                get { return m_EmpBenefitPlanID; }
                set { m_EmpBenefitPlanID = value; modify("EmpBenefitPlanID"); }
            }
            
            protected double m_EmpBenefitERPremium;
            [DBField("EmpBenefitERPremium", "0.00"), TextSearch, MaxLength(11), Export(false)]
            public double EmpBenefitERPremium
            {
                get { return m_EmpBenefitERPremium; }
                set { m_EmpBenefitERPremium = value; modify("EmpBenefitERPremium"); }
            }

            protected double m_EmpBenefitEEPremium;
            [DBField("EmpBenefitEEPremium", "0.00"), TextSearch, MaxLength(11), Export(false)]
            public double EmpBenefitEEPremium
            {
                get { return m_EmpBenefitEEPremium; }
                set { m_EmpBenefitEEPremium = value; modify("EmpBenefitEEPremium"); }
            }

            protected double m_EmpBenefitChildPremium;
            [DBField("EmpBenefitChildPremium", "0.00"), TextSearch, MaxLength(11), Export(false)]
            public double EmpBenefitChildPremium
            {
                get { return m_EmpBenefitChildPremium; }
                set { m_EmpBenefitChildPremium = value; modify("EmpBenefitChildPremium"); }
            }

            protected double m_EmpBenefitSpousePremium;
            [DBField("EmpBenefitSpousePremium", "0.00"), TextSearch, MaxLength(11), Export(false)]
            public double EmpBenefitSpousePremium
            {
                get { return m_EmpBenefitSpousePremium; }
                set { m_EmpBenefitSpousePremium = value; modify("EmpBenefitSpousePremium"); }
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

        public const string TABLE_NAME = "benefit";

        private const string FIELD_EMP_NO = "Emp No";
        private const string FIELD_EFFECTIVE_DATE = "Effective Date";
        private const string FIELD_EXPIRY_DATE = "Expiry Date";
        private const string FIELD_BENEFIT_PLAN = "Benefit Plan";
        private const string FIELD_ER_PREMIUM = "ER Premium";
        private const string FIELD_EE_PREMIUM = "EE Premium";
        private const string FIELD_CHILD_PREMIUM = "Child Premium";
        private const string FIELD_SPOUSE_PREMIUM = "Spouse Premium";

        
        private DateTime UploadDateTime = AppUtils.ServerDateTime();
        private DBManager tempDB = EUploadEmpBenefit.db;
        private DBManager uploadDB = EEmpBenefit.db;

        public ImportErrorList errors = new ImportErrorList();
        

        public ImportEmpBenefitProcess(DatabaseConnection dbConn, string SessionID): base(dbConn, SessionID)
        {
        }

        public DataTable UploadToTempDatabase(DataTable rawDataTable, int UserID)
        {
            if (rawDataTable == null)
                return GetImportDataFromTempDatabase(null);

            decimal m_ERPremium;
            decimal m_EEPremium;
            decimal m_childPremium;
            decimal m_spousePremium;

            int rowCount = 1;
            foreach (DataRow row in rawDataTable.Rows)
            {
                rowCount++;

                EUploadEmpBenefit uploadEmpBenefit = new EUploadEmpBenefit();

                string EmpNo = row[FIELD_EMP_NO].ToString().Trim();
                uploadEmpBenefit.EmpID = Parse.GetEmpID(dbConn, EmpNo, UserID);
                uploadEmpBenefit.EmpBenefitID = 0;
                if (uploadEmpBenefit.EmpID < 0)
                    errors.addError(ImportErrorMessage.ERROR_ACCESS_DENIED_EMP_NO, new string[] { EmpNo, rowCount.ToString() });

                try
                {
                    uploadEmpBenefit.EmpBenefitEffectiveDate = Parse.toDateTimeObject(row[FIELD_EFFECTIVE_DATE]);
                }
                catch
                {
                    errors.addError(ImportErrorMessage.ERROR_INVALID_FIELD_VALUE, new string[] { FIELD_EFFECTIVE_DATE + "=" + row[FIELD_EFFECTIVE_DATE].ToString(), EmpNo, rowCount.ToString() });
                }

                try
                {
                    uploadEmpBenefit.EmpBenefitExpiryDate = Parse.toDateTimeObject(row[FIELD_EXPIRY_DATE]);
                }
                catch
                {
                    errors.addError(ImportErrorMessage.ERROR_INVALID_FIELD_VALUE, new string[] { FIELD_EXPIRY_DATE + "=" + row[FIELD_EXPIRY_DATE].ToString(), EmpNo, rowCount.ToString() });
                }

                if (rawDataTable.Columns.Contains(FIELD_BENEFIT_PLAN))
                    uploadEmpBenefit.EmpBenefitPlanID = Import.Parse.GetBenefitPlanID(dbConn, row[FIELD_BENEFIT_PLAN].ToString(), false, UserID);

                if (uploadEmpBenefit.EmpBenefitPlanID <= 0)
                {
                    errors.addError( ImportErrorMessage.ERROR_INVALID_FIELD_VALUE, new string[] {FIELD_BENEFIT_PLAN + "=" + row[FIELD_BENEFIT_PLAN].ToString(), EmpNo, rowCount.ToString()});
                }

                if (decimal.TryParse(row[FIELD_ER_PREMIUM].ToString(), out m_ERPremium))
                {
                    uploadEmpBenefit.EmpBenefitERPremium = System.Convert.ToDouble(m_ERPremium);
                }
                else
                {
                    errors.addError(ImportErrorMessage.ERROR_INVALID_FIELD_VALUE, new string[] { FIELD_ER_PREMIUM + "=" + row[FIELD_ER_PREMIUM].ToString(), EmpNo, rowCount.ToString() });
                }

                if (decimal.TryParse(row[FIELD_EE_PREMIUM].ToString(), out m_EEPremium))
                {
                    uploadEmpBenefit.EmpBenefitEEPremium = System.Convert.ToDouble(m_EEPremium);
                }
                else
                {
                    errors.addError(ImportErrorMessage.ERROR_INVALID_FIELD_VALUE, new string[] { FIELD_EE_PREMIUM + "=" + row[FIELD_EE_PREMIUM].ToString(), EmpNo, rowCount.ToString() });
                }

                if (decimal.TryParse(row[FIELD_CHILD_PREMIUM].ToString(), out m_childPremium))
                {
                    uploadEmpBenefit.EmpBenefitChildPremium = System.Convert.ToDouble(m_childPremium);
                }
                else
                {
                    errors.addError(ImportErrorMessage.ERROR_INVALID_FIELD_VALUE, new string[] { FIELD_CHILD_PREMIUM + "=" + row[FIELD_CHILD_PREMIUM].ToString(), EmpNo, rowCount.ToString() });
                }

                if (decimal.TryParse(row[FIELD_SPOUSE_PREMIUM].ToString(), out m_spousePremium))
                {
                    uploadEmpBenefit.EmpBenefitSpousePremium = System.Convert.ToDouble(m_spousePremium);
                }
                else
                {
                    errors.addError(ImportErrorMessage.ERROR_INVALID_FIELD_VALUE, new string[] { FIELD_SPOUSE_PREMIUM + "=" + row[FIELD_SPOUSE_PREMIUM].ToString(), EmpNo, rowCount.ToString() });
                }

                uploadEmpBenefit.SessionID = m_SessionID;
                uploadEmpBenefit.TransactionDate = UploadDateTime;


                if (uploadEmpBenefit.EmpID != 0)
                {
                    if (rawDataTable.Columns.Contains(FIELD_SYNC_ID))
                    {
                        if (!row.IsNull(FIELD_SYNC_ID))
                        {
                            string strSynID = row[FIELD_SYNC_ID].ToString();
                            uploadEmpBenefit.SynID = strSynID;
                            if (!string.IsNullOrEmpty(strSynID))
                            {
                                DBFilter synIDFilter = new DBFilter();
                                synIDFilter.add(new Match("SynID", strSynID));
                                ArrayList objSameSynIDList = EEmpBenefit.db.select(dbConn, synIDFilter);
                                if (objSameSynIDList.Count > 0)
                                    uploadEmpBenefit.EmpBenefitID = ((EEmpBenefit)objSameSynIDList[0]).EmpBenefitID;
                            }
                        }

                    }

                    if (uploadEmpBenefit.EmpBenefitID == 0)
                    {

                        DBFilter filter = new DBFilter();
                        filter.add(new Match("EmpID", uploadEmpBenefit.EmpID));
                        filter.add(new Match("EmpBenefitEffectiveDate", uploadEmpBenefit.EmpBenefitEffectiveDate));
                        filter.add(new Match("EmpBenefitPlanID", uploadEmpBenefit.EmpBenefitPlanID));
                        ArrayList list = uploadDB.select(dbConn, filter);
                        bool IsSameEntryExists = false;
                        foreach (EEmpBenefit currentEmpBenefit in list)
                        {
                            uploadEmpBenefit.EmpBenefitID = currentEmpBenefit.EmpBenefitID;

                            if (uploadEmpBenefit.EmpBenefitEffectiveDate == currentEmpBenefit.EmpBenefitEffectiveDate
                                && uploadEmpBenefit.EmpBenefitExpiryDate == currentEmpBenefit.EmpBenefitExpiryDate
                                && uploadEmpBenefit.EmpBenefitPlanID == currentEmpBenefit.EmpBenefitPlanID
                                && Math.Abs(uploadEmpBenefit.EmpBenefitERPremium - currentEmpBenefit.EmpBenefitERPremium) < 0.01
                                && Math.Abs(uploadEmpBenefit.EmpBenefitEEPremium - currentEmpBenefit.EmpBenefitEEPremium) < 0.01
                                && Math.Abs(uploadEmpBenefit.EmpBenefitChildPremium - currentEmpBenefit.EmpBenefitChildPremium) < 0.01
                                && Math.Abs(uploadEmpBenefit.EmpBenefitSpousePremium - currentEmpBenefit.EmpBenefitSpousePremium) < 0.01
                                )
                            {
                                IsSameEntryExists = true;
                                break;
                            }
                        }
                        if (IsSameEntryExists)
                            continue;
                    }
                }

                if (uploadEmpBenefit.EmpBenefitID <= 0)
                    uploadEmpBenefit.ImportActionStatus = ImportDBObject.ImportActionEnum.INSERT;
                else
                    uploadEmpBenefit.ImportActionStatus = ImportDBObject.ImportActionEnum.UPDATE;

                uploadEmpBenefit.UploadEmpID = ParseTemp.GetUploadEmpID(dbConn, EmpNo, m_SessionID);
                if (uploadEmpBenefit.UploadEmpID == 0)
                    if (uploadEmpBenefit.EmpID == 0)
                        errors.addError(ImportErrorMessage.ERROR_INVALID_EMP_NO, new string[] { EmpNo, rowCount.ToString() });
                    else
                        uploadEmpBenefit.UploadEmpID = ImportEmpPersonalInfoProcess.CreateDummyUploadEmployeeInfo(dbConn, uploadEmpBenefit.EmpID, m_SessionID, UploadDateTime);

                Hashtable values = new Hashtable();
                tempDB.populate(uploadEmpBenefit, values);
                PageErrors pageErrors = new PageErrors(EUploadEmpPersonalInfo.db);
                tempDB.validate(pageErrors, values);
                if (pageErrors.errors.Count == 0)
                {

                    tempDB.insert(dbConn, uploadEmpBenefit);
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
            ImportEmpBenefitProcess import = new ImportEmpBenefitProcess(dbConn, sessionID);
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
            ArrayList uploadEmpBenefitList = tempDB.select(dbConn, sessionFilter);
            foreach (EUploadEmpBenefit obj in uploadEmpBenefitList)
            {
                EEmpBenefit EmpBenefit = new EEmpBenefit();

                if (obj.ImportActionStatus != ImportDBObject.ImportActionEnum.INSERT)
                {
                    EmpBenefit.EmpBenefitID = obj.EmpBenefitID;
                    uploadDB.select(dbConn, EmpBenefit);
                }

                obj.ExportToObject(EmpBenefit);

                if (obj.ImportActionStatus == ImportDBObject.ImportActionEnum.INSERT)
                {
                    EmpBenefit.EmpID = ParseTemp.GetEmpIDFromUploadEmpID(dbConn, obj.UploadEmpID);
                    uploadDB.insert(dbConn, EmpBenefit);
                }
                else if (obj.ImportActionStatus == ImportDBObject.ImportActionEnum.UPDATE)
                {
                    uploadDB.update(dbConn, EmpBenefit);
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
            tmpDataTable.Columns.Add(FIELD_EMP_NO, typeof(string));

            if (IsIncludeCurrentPositionInfo)
            {
                ImportEmpPersonalInfoProcess.AddEmployeeInfoHeader(tmpDataTable);
                ImportEmpPositionInfoProcess.AddEmployeePositionInfoHeader(dbConn, tmpDataTable);
            }

            tmpDataTable.Columns.Add(FIELD_EFFECTIVE_DATE, typeof(DateTime));
            tmpDataTable.Columns.Add(FIELD_EXPIRY_DATE, typeof(DateTime));
            tmpDataTable.Columns.Add(FIELD_BENEFIT_PLAN, typeof(string));
            tmpDataTable.Columns.Add(FIELD_ER_PREMIUM, typeof(decimal));
            tmpDataTable.Columns.Add(FIELD_EE_PREMIUM, typeof(decimal));
            tmpDataTable.Columns.Add(FIELD_CHILD_PREMIUM, typeof(decimal));
            tmpDataTable.Columns.Add(FIELD_SPOUSE_PREMIUM, typeof(decimal));
            if (IsIncludeSyncID)
                tmpDataTable.Columns.Add(FIELD_SYNC_ID, typeof(string));

            foreach (EEmpPersonalInfo empInfo in empList)
            {
                if (EEmpPersonalInfo.db.select(dbConn, empInfo))
                {
                    DBFilter filter = new DBFilter();
                    filter.add(new Match("EmpID", empInfo.EmpID));
                    filter.add(getCreateModifiedRecordsAfterDBTerm(ReferenceDateTime));
                    ArrayList list = EEmpBenefit.db.select(dbConn, filter);
                    foreach (EEmpBenefit empBenefit in list)
                    {
                        DataRow row = tmpDataTable.NewRow();
                        row[FIELD_EMP_NO] = empInfo.EmpNo;

                        if (IsIncludeCurrentPositionInfo)
                        {
                            ImportEmpPersonalInfoProcess.AddEmployeeInfo(dbConn, row, empInfo.EmpID);
                            ImportEmpPositionInfoProcess.AddEmployeePositionInfo(dbConn, row, empInfo.EmpID);
                        }

                        row[FIELD_EFFECTIVE_DATE] = empBenefit.EmpBenefitEffectiveDate;
                        row[FIELD_EXPIRY_DATE] = empBenefit.EmpBenefitExpiryDate;

                        EBenefitPlan benefitPlan = new EBenefitPlan();
                        if (benefitPlan.LoadDBObject(dbConn, empBenefit.EmpBenefitPlanID))
                            row[FIELD_BENEFIT_PLAN] = IsShowDescription ? benefitPlan.BenefitPlanDesc : benefitPlan.BenefitPlanCode;

                        row[FIELD_ER_PREMIUM] = empBenefit.EmpBenefitERPremium;
                        row[FIELD_EE_PREMIUM] = empBenefit.EmpBenefitEEPremium;
                        row[FIELD_CHILD_PREMIUM] = empBenefit.EmpBenefitChildPremium;
                        row[FIELD_SPOUSE_PREMIUM] = empBenefit.EmpBenefitSpousePremium;

                        if (IsIncludeSyncID)
                            row[FIELD_SYNC_ID] = empBenefit.SynID;

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
