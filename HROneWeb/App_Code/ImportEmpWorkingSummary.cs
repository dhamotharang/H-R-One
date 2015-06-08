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


//    /// <summary>
//    /// Summary description for ImportCND
//    /// </summary>
//    public class ImportEmpWorkingSummaryProcess : ImportProcessInteface
//    {
//        [DBClass("UploadEmpWorkingSummary")]
//        protected class EUploadEmpWorkingSummary : ImportDBObject
//        {
//            public static DBManager db = new DBManager(typeof(EUploadEmpWorkingSummary));

//            protected int m_UploadEmpWorkingSummaryID;
//            [DBField("UploadEmpWorkingSummaryID", true, true), TextSearch, Export(false)]
//            public int UploadEmpWorkingSummaryID
//            {
//                get { return m_UploadEmpWorkingSummaryID; }
//                set { m_UploadEmpWorkingSummaryID = value; modify("UploadEmpWorkingSummaryID"); }
//            }
//            protected int m_EmpWorkingSummaryID;
//            [DBField("EmpWorkingSummaryID"), TextSearch, Export(false)]
//            public int EmpWorkingSummaryID
//            {
//                get { return m_EmpWorkingSummaryID; }
//                set { m_EmpWorkingSummaryID = value; modify("EmpWorkingSummaryID"); }
//            }
//            protected int m_EmpID;
//            [DBField("EmpID"), TextSearch, Export(false)]
//            public int EmpID
//            {
//                get { return m_EmpID; }
//                set { m_EmpID = value; modify("EmpID"); }
//            }
//            protected DateTime m_EmpWorkingSummaryAsOfDate;
//            [DBField("EmpWorkingSummaryAsOfDate"), TextSearch, MaxLength(10), Export(false)]
//            public DateTime EmpWorkingSummaryAsOfDate
//            {
//                get { return m_EmpWorkingSummaryAsOfDate; }
//                set { m_EmpWorkingSummaryAsOfDate = value; modify("EmpWorkingSummaryAsOfDate"); }
//            }
//            protected double m_EmpWorkingSummaryRestDayEntitled;
//            [DBField("EmpWorkingSummaryRestDayEntitled", "0.##"), TextSearch, Export(false)]
//            public double EmpWorkingSummaryRestDayEntitled
//            {
//                get { return m_EmpWorkingSummaryRestDayEntitled; }
//                set { m_EmpWorkingSummaryRestDayEntitled = value; modify("EmpWorkingSummaryRestDayEntitled"); }
//            }
//            protected double m_EmpWorkingSummaryRestDayTaken;
//            [DBField("EmpWorkingSummaryRestDayTaken", "0.##"), TextSearch, Export(false)]
//            public double EmpWorkingSummaryRestDayTaken
//            {
//                get { return m_EmpWorkingSummaryRestDayTaken; }
//                set { m_EmpWorkingSummaryRestDayTaken = value; modify("EmpWorkingSummaryRestDayTaken"); }
//            }
//            protected double m_EmpWorkingSummaryTotalWorkingDays;
//            [DBField("EmpWorkingSummaryTotalWorkingDays", "0.##"), TextSearch, Export(false)]
//            public double EmpWorkingSummaryTotalWorkingDays
//            {
//                get { return m_EmpWorkingSummaryTotalWorkingDays; }
//                set { m_EmpWorkingSummaryTotalWorkingDays = value; modify("EmpWorkingSummaryTotalWorkingDays"); }
//            }
//            protected double m_EmpWorkingSummaryTotalWorkingHours;
//            [DBField("EmpWorkingSummaryTotalWorkingHours","0.####"), TextSearch, Export(false)]
//            public double EmpWorkingSummaryTotalWorkingHours
//            {
//                get { return m_EmpWorkingSummaryTotalWorkingHours; }
//                set { m_EmpWorkingSummaryTotalWorkingHours = value; modify("EmpWorkingSummaryTotalWorkingHours"); }
//            }
//            protected double m_EmpWorkingSummaryTotalLunchTimeHours;
//            [DBField("EmpWorkingSummaryTotalLunchTimeHours", "0.####"), TextSearch, Export(false)]
//            public double EmpWorkingSummaryTotalLunchTimeHours
//            {
//                get { return m_EmpWorkingSummaryTotalLunchTimeHours; }
//                set { m_EmpWorkingSummaryTotalLunchTimeHours = value; modify("EmpWorkingSummaryTotalLunchTimeHours"); }
//            }

//        }

//        public const string FIELD_EMP_NO = "Emp No";
//        public const string FIELD_AS_OF_DATE = "Date";
//        public const string FIELD_REST_DAY_ENTITLED = "Rest Day Entitled";
//        public const string FIELD_REST_DAY_TAKEN = "Rest Day Taken";
//        public const string FIELD_TOTAL_WORKING_DAYS = "Total Working Days";
//        public const string FIELD_TOTAL_WORKING_HOURS = "Total Hours Worked";
//        public const string FIELD_TOTAL_LUNCH_HOURS = "Total Hours for Meal break";
//        public const string FIELD_EXTRA_TOTAL_STATUTORY_HOLIDAY = "Total Statutory Holiday";
//        public const string FIELD_EXTRA_TOTAL_LEAVE_APPLICATION_TAKEN = "Total Leave Application Days Taken";
//        public const string FIELD_EXTRA_TOTAL_WORKING_HOURS_EXPECTED = "Expected Total Hours Worked";
//        public const string FIELD_EXTRA_TOTAL_OVERTIME_MINS = "Total Overtime (mins)";
//        public const string FIELD_EXTRA_TOTAL_LATE_MINS = "Total Late Mins (mins)";
//        public const string FIELD_EXTRA_TOTAL_EARLYLEAVE_MINS = "Total Early Leave (mins)";

//        private int m_UserID;
//        public string Remark;
//        private DateTime UploadDateTime = AppUtils.ServerDateTime();
//        private DBManager tempDB = EUploadEmpWorkingSummary.db;

//        public ImportErrorList errors = new ImportErrorList();



//        public ImportEmpWorkingSummaryProcess(DatabaseConnection dbConn, string SessionID, int UserID)
//            : base(dbConn, SessionID)
//        {
//            m_UserID = UserID;
//        }


//        public override DataTable UploadToTempDatabase(string Filename, int UserID, string ZipPassword)
//        {
//            ClearTempTable();
//            DataTable rawDataTable = HROne.Import.ExcelImport.parse(Filename, ZipPassword).Tables[0];
//            ArrayList results = new ArrayList();
//            int rowCount = 1;

//            try
//            {
//                foreach (DataRow row in rawDataTable.Rows)
//                {
//                    rowCount++;
//                    string EmpNo = row[FIELD_EMP_NO].ToString();
//                    int EmpID = HROne.Import.Parse.GetEmpID(dbConn, EmpNo, UserID);
//                    DateTime AsOfDate = Import.Parse.toDateTimeObject(row[FIELD_AS_OF_DATE]);
//                    string RestDayEntitledString = row[FIELD_REST_DAY_ENTITLED].ToString();
//                    string RestDayTakenString = row[FIELD_REST_DAY_TAKEN].ToString();
//                    string TotalWorkingDaysString = row[FIELD_TOTAL_WORKING_DAYS].ToString();
//                    string TotalWorkingHoursString = row[FIELD_TOTAL_WORKING_HOURS].ToString();
//                    string TotalLunchTimeString = row[FIELD_TOTAL_LUNCH_HOURS].ToString();



//                    double RestDayEntitled = 0;
//                    double RestDayTaken = 0;
//                    double TotalWorkingDays = 0;
//                    double TotalWorkingHours = 0;
//                    double TotalLunchTime = 0;


//                    EUploadEmpWorkingSummary uploadEmpWorkingSummary = new EUploadEmpWorkingSummary();
//                    uploadEmpWorkingSummary.EmpID = EmpID;

//                    ArrayList additionUploadRecordList = new ArrayList();

//                    if (EmpID > 0 && AsOfDate.Ticks != 0 &&
//                        double.TryParse(RestDayEntitledString, out RestDayEntitled) &&
//                        double.TryParse(RestDayTakenString, out RestDayTaken) &&
//                        double.TryParse(TotalWorkingDaysString, out TotalWorkingDays) &&
//                        double.TryParse(TotalWorkingHoursString, out TotalWorkingHours) &&
//                        double.TryParse(TotalLunchTimeString, out TotalLunchTime)
//                        )
//                    {

//                        uploadEmpWorkingSummary.EmpWorkingSummaryAsOfDate = AsOfDate;
//                        uploadEmpWorkingSummary.EmpWorkingSummaryRestDayEntitled = RestDayEntitled;
//                        uploadEmpWorkingSummary.EmpWorkingSummaryRestDayTaken = RestDayTaken;
//                        uploadEmpWorkingSummary.EmpWorkingSummaryTotalWorkingDays = TotalWorkingDays;
//                        uploadEmpWorkingSummary.EmpWorkingSummaryTotalWorkingHours = TotalWorkingHours;
//                        uploadEmpWorkingSummary.EmpWorkingSummaryTotalLunchTimeHours = TotalLunchTime;
//                        uploadEmpWorkingSummary.SessionID = m_SessionID;
//                        uploadEmpWorkingSummary.TransactionDate = UploadDateTime;
//                        results.Add(uploadEmpWorkingSummary);

//                        DBFilter dbFilter = new DBFilter();
//                        dbFilter.add(new Match("EmpID", uploadEmpWorkingSummary.EmpID));
//                        dbFilter.add(new Match("EmpWorkingSummaryAsOfDate", uploadEmpWorkingSummary.EmpWorkingSummaryAsOfDate));

//                        ArrayList list = EEmpWorkingSummary.db.select(dbConn, dbFilter);
//                        foreach (EEmpWorkingSummary empWorkingSummary in list)
//                        {
//                            if (uploadEmpWorkingSummary.EmpWorkingSummaryID.Equals(0))
//                            {
//                                uploadEmpWorkingSummary.EmpWorkingSummaryID = empWorkingSummary.EmpWorkingSummaryID;
//                            }
//                            else
//                            {
//                                //  Set Next Existing record to 0;
//                                EUploadEmpWorkingSummary uploadNextWorkingSummary = new EUploadEmpWorkingSummary();
//                                ImportDBObject.CopyObjectProperties(empWorkingSummary, uploadNextWorkingSummary);

//                                uploadNextWorkingSummary.EmpWorkingSummaryRestDayEntitled = RestDayEntitled;
//                                uploadNextWorkingSummary.EmpWorkingSummaryRestDayTaken = RestDayTaken;
//                                uploadNextWorkingSummary.EmpWorkingSummaryTotalWorkingDays = TotalWorkingDays;
//                                uploadNextWorkingSummary.EmpWorkingSummaryTotalWorkingHours = TotalWorkingHours;
//                                uploadNextWorkingSummary.EmpWorkingSummaryTotalLunchTimeHours = TotalLunchTime;

//                                uploadNextWorkingSummary.SessionID = m_SessionID;
//                                uploadNextWorkingSummary.TransactionDate = UploadDateTime;
//                                uploadNextWorkingSummary.ImportActionStatus = ImportDBObject.ImportActionEnum.UPDATE;

//                                additionUploadRecordList.Add(uploadNextWorkingSummary);
//                            }
//                        }
//                    }
//                    else
//                    {
//                        double doubleValue;
//                        if (EmpID == 0)
//                            errors.addError(ImportErrorMessage.ERROR_INVALID_EMP_NO, new string[] { EmpNo, rowCount.ToString() });
//                        else if (AsOfDate.Ticks == 0)
//                            errors.addError(ImportErrorMessage.ERROR_INVALID_FIELD_VALUE, new string[] { FIELD_AS_OF_DATE + "=" + row[FIELD_AS_OF_DATE], EmpNo, rowCount.ToString() });
//                        else if (!double.TryParse(RestDayEntitledString, out doubleValue))
//                            errors.addError(ImportErrorMessage.ERROR_INVALID_FIELD_VALUE, new string[] { FIELD_REST_DAY_ENTITLED + "=" + RestDayEntitledString, EmpNo, rowCount.ToString() });
//                        else if (!double.TryParse(RestDayTakenString, out doubleValue))
//                            errors.addError(ImportErrorMessage.ERROR_INVALID_FIELD_VALUE, new string[] { FIELD_REST_DAY_TAKEN + "=" + RestDayTakenString, EmpNo, rowCount.ToString() });
//                        else if (!double.TryParse(TotalWorkingDaysString, out doubleValue))
//                            errors.addError(ImportErrorMessage.ERROR_INVALID_FIELD_VALUE, new string[] { FIELD_TOTAL_WORKING_DAYS + "=" + TotalWorkingDaysString, EmpNo, rowCount.ToString() });
//                        else if (!double.TryParse(TotalWorkingHoursString, out doubleValue))
//                            errors.addError(ImportErrorMessage.ERROR_INVALID_FIELD_VALUE, new string[] { FIELD_TOTAL_WORKING_HOURS + "=" + TotalWorkingHoursString, EmpNo, rowCount.ToString() });
//                        else if (!double.TryParse(TotalLunchTimeString, out doubleValue))
//                            errors.addError(ImportErrorMessage.ERROR_INVALID_FIELD_VALUE, new string[] { FIELD_TOTAL_LUNCH_HOURS + "=" + TotalLunchTimeString, EmpNo, rowCount.ToString() });
//                    }

//                    if (uploadEmpWorkingSummary.EmpWorkingSummaryID <= 0)
//                        uploadEmpWorkingSummary.ImportActionStatus = ImportDBObject.ImportActionEnum.INSERT;
//                    else
//                        uploadEmpWorkingSummary.ImportActionStatus = ImportDBObject.ImportActionEnum.UPDATE;

//                    if (errors.List.Count == 0)
//                    {
//                        Hashtable values = new Hashtable();
//                        tempDB.populate(uploadEmpWorkingSummary, values);
//                        PageErrors pageErrors = new PageErrors(tempDB);
//                        tempDB.validate(pageErrors, values);
//                        if (pageErrors.errors.Count == 0)
//                        {

//                            tempDB.insert(dbConn, uploadEmpWorkingSummary);
//                            foreach (EUploadEmpWorkingSummary uploadNextWorkingSummary in additionUploadRecordList)
//                            {
//                                tempDB.insert(dbConn, uploadNextWorkingSummary);
//                            }
//                        }
//                        else
//                        {
//                            pageErrors.addError(rawDataTable.TableName);
//                            throw new HRImportException(pageErrors.getPrompt() + "(line " + rowCount.ToString() + ")");

//                        }
//                    }
//                }
//            }
//            catch(Exception e)
//            {
//                errors.addError(e.Message, null);
//            }
//            if (errors.List.Count > 0)
//            {
//                ClearTempTable();
//                throw (new HRImportException(rawDataTable.TableName + "\r\n"+ errors.Message()));
//            }
//            return GetImportDataFromTempDatabase(null);
//        }
//        public override DataTable GetImportDataFromTempDatabase(ListInfo info)
//        {
//            DBFilter sessionFilter = new DBFilter();
//            sessionFilter.add(new Match("SessionID", m_SessionID));
//            sessionFilter.add(new MatchField("c.EmpID", "e.EmpID"));
//            //if (info != null && info.orderby != null && !info.orderby.Equals(""))
//            //    sessionFilter.add(info.orderby, info.order);

//            return sessionFilter.loadData(dbConn, null, "e.*, c.* ", " from " + tempDB.dbclass.tableName + " c, " + EEmpPersonalInfo.db.dbclass.tableName + " e");

//        }

//        public static void ClearTempTable(DatabaseConnection dbConn, string sessionID)
//        {
//            ImportEmpWorkingSummaryProcess import = new ImportEmpWorkingSummaryProcess(dbConn, sessionID, 0);
//            import.ClearTempTable();
//        }

//        public override void ClearTempTable()
//        {
//            //  Clear Old Import Session
//            DBFilter sessionFilter = new DBFilter();
//            if (!string.IsNullOrEmpty(m_SessionID))
//                sessionFilter.add(new Match("SessionID", m_SessionID));
//            tempDB.delete(dbConn, sessionFilter);
//        }

//        public override void ImportToDatabase()
//        {
//            DataTable dataTable = GetImportDataFromTempDatabase(null);
//            if (dataTable.Rows.Count > 0)
//            {

//                foreach (DataRow row in dataTable.Rows)
//                {
//                    EUploadEmpWorkingSummary obj = new EUploadEmpWorkingSummary();
//                    tempDB.toObject(row, obj);

//                    EEmpWorkingSummary empWorkingSummary ;
//                    DBFilter dbFilter = new DBFilter();
//                    dbFilter.add(new Match("EmpID",obj.EmpID));
//                    dbFilter.add(new Match("EmpWorkingSummaryAsOfDate",obj.EmpWorkingSummaryAsOfDate));
//                    ArrayList empWorkingSummaryList =EEmpWorkingSummary.db.select(dbConn, dbFilter);
//                    if (empWorkingSummaryList.Count > 0)
//                        empWorkingSummary = (EEmpWorkingSummary)empWorkingSummaryList[0];
//                    else
//                        empWorkingSummary = new EEmpWorkingSummary();

//                    empWorkingSummary.EmpID = obj.EmpID;
//                    empWorkingSummary.EmpWorkingSummaryAsOfDate = obj.EmpWorkingSummaryAsOfDate;
//                    empWorkingSummary.EmpWorkingSummaryRestDayEntitled = obj.EmpWorkingSummaryRestDayEntitled;
//                    empWorkingSummary.EmpWorkingSummaryRestDayTaken = obj.EmpWorkingSummaryRestDayTaken;
//                    empWorkingSummary.EmpWorkingSummaryTotalWorkingDays = obj.EmpWorkingSummaryTotalWorkingDays;
//                    empWorkingSummary.EmpWorkingSummaryTotalWorkingHours = obj.EmpWorkingSummaryTotalWorkingHours;
//                    empWorkingSummary.EmpWorkingSummaryTotalLunchTimeHours = obj.EmpWorkingSummaryTotalLunchTimeHours;

//                    if (empWorkingSummary.EmpWorkingSummaryID > 0)
//                        EEmpWorkingSummary.db.update(dbConn, empWorkingSummary);
//                    else
//                        EEmpWorkingSummary.db.insert(dbConn, empWorkingSummary);

//                    tempDB.delete(dbConn, obj);

//                }
//            }
//        }

//        public static DataTable Export(DatabaseConnection dbConn, ArrayList EmpInfoList, DateTime PeriodFrom, DateTime PeriodTo)
//        {
//            DataTable tmpDataTable = new DataTable("WorkingSummary$");

//            tmpDataTable.Columns.Add(FIELD_EMP_NO, typeof(string));
//            tmpDataTable.Columns.Add("EnglishName", typeof(string));
//            tmpDataTable.Columns.Add("ChineseName", typeof(string));

//            tmpDataTable.Columns.Add("Company", typeof(string));

//            DBFilter hierarchyLevelFilter = new DBFilter();
//            Hashtable hierarchyLevelHashTable = new Hashtable();
//            hierarchyLevelFilter.add("HLevelSeqNo", true);
//            ArrayList hierarchyLevelList = EHierarchyLevel.db.select(dbConn, hierarchyLevelFilter);
//            foreach (EHierarchyLevel hlevel in hierarchyLevelList)
//            {
//                tmpDataTable.Columns.Add(hlevel.HLevelDesc, typeof(string));
//                hierarchyLevelHashTable.Add(hlevel.HLevelID, hlevel);
//            }

//            tmpDataTable.Columns.Add(FIELD_AS_OF_DATE, typeof(DateTime));
//            tmpDataTable.Columns.Add(FIELD_REST_DAY_ENTITLED, typeof(double));
//            tmpDataTable.Columns.Add(FIELD_REST_DAY_TAKEN, typeof(double));
//            tmpDataTable.Columns.Add(FIELD_TOTAL_WORKING_DAYS, typeof(double));
//            tmpDataTable.Columns.Add(FIELD_TOTAL_WORKING_HOURS, typeof(double));
//            tmpDataTable.Columns.Add(FIELD_TOTAL_LUNCH_HOURS, typeof(double));

//            foreach (EEmpPersonalInfo empInfo in EmpInfoList)
//            {
//                if (EEmpPersonalInfo.db.select(dbConn, empInfo))
//                {

//                    DBFilter empWorkingSummaryFilter = new DBFilter();
//                    empWorkingSummaryFilter.add(new Match("EmpID", empInfo.EmpID));
//                    empWorkingSummaryFilter.add(new Match("EmpWorkingSummaryAsOfDate", ">=", PeriodFrom));
//                    empWorkingSummaryFilter.add(new Match("EmpWorkingSummaryAsOfDate", "<=", PeriodTo));
//                    empWorkingSummaryFilter.add("EmpWorkingSummaryAsOfDate", true);
//                    ArrayList empWorkingSummaryList = EEmpWorkingSummary.db.select(dbConn, empWorkingSummaryFilter);
//                    foreach (EEmpWorkingSummary empWorkingSummary in empWorkingSummaryList)
//                    {
//                        DataRow row = tmpDataTable.NewRow();
//                        row[FIELD_EMP_NO] = empInfo.EmpNo;
//                        row["EnglishName"] = empInfo.EmpEngFullName;
//                        row["ChineseName"] = empInfo.EmpChiFullName;

//                        DBFilter empPosFilter = new DBFilter();

//                        EEmpPositionInfo empPos = AppUtils.GetLastPositionInfo(dbConn, empWorkingSummary.EmpWorkingSummaryAsOfDate, empInfo.EmpID);
//                        if (empPos != null)
//                        {
//                            ECompany company = new ECompany();
//                            company.CompanyID = empPos.CompanyID;
//                            if (ECompany.db.select(dbConn, company))
//                                row["Company"] = company.CompanyCode;
//                            DBFilter empHierarchyFilter = new DBFilter();
//                            empHierarchyFilter.add(new Match("EmpPosID", empPos.EmpPosID));
//                            ArrayList empHierarchyList = EEmpHierarchy.db.select(dbConn, empHierarchyFilter);
//                            foreach (EEmpHierarchy empHierarchy in empHierarchyList)
//                            {
//                                EHierarchyLevel hierarchyLevel = (EHierarchyLevel)hierarchyLevelHashTable[empHierarchy.HLevelID];
//                                if (hierarchyLevel != null)
//                                {
//                                    EHierarchyElement hierarchyElement = new EHierarchyElement();
//                                    hierarchyElement.HElementID = empHierarchy.HElementID;
//                                    if (EHierarchyElement.db.select(dbConn, hierarchyElement))
//                                        row[hierarchyLevel.HLevelDesc] = hierarchyElement.HElementCode;
//                                }
//                            }
//                        }

//                        row[FIELD_AS_OF_DATE] = empWorkingSummary.EmpWorkingSummaryAsOfDate;
//                        row[FIELD_REST_DAY_ENTITLED] = empWorkingSummary.EmpWorkingSummaryRestDayEntitled;
//                        row[FIELD_REST_DAY_TAKEN] = empWorkingSummary.EmpWorkingSummaryRestDayTaken;
//                        row[FIELD_TOTAL_WORKING_DAYS] = empWorkingSummary.EmpWorkingSummaryTotalWorkingDays;
//                        row[FIELD_TOTAL_WORKING_HOURS] = empWorkingSummary.EmpWorkingSummaryTotalWorkingHours;
//                        row[FIELD_TOTAL_LUNCH_HOURS] = empWorkingSummary.EmpWorkingSummaryTotalLunchTimeHours;
//                        tmpDataTable.Rows.Add(row);
//                    }
//                }
//            }
//            return tmpDataTable;
//        }

//        public static DataTable GenerateTemplate(DatabaseConnection dbConn, ArrayList EmpInfoList, DateTime PeriodFrom, DateTime PeriodTo)
//        {
//            DataTable tmpDataTable = new DataTable("WorkingSummary$");

//            tmpDataTable.Columns.Add(FIELD_EMP_NO, typeof(string));
//            tmpDataTable.Columns.Add("EnglishName", typeof(string));
//            tmpDataTable.Columns.Add("ChineseName", typeof(string));

//            tmpDataTable.Columns.Add("Company", typeof(string));

//            DBFilter hierarchyLevelFilter = new DBFilter();
//            Hashtable hierarchyLevelHashTable = new Hashtable();
//            hierarchyLevelFilter.add("HLevelSeqNo", true);
//            ArrayList hierarchyLevelList = EHierarchyLevel.db.select(dbConn, hierarchyLevelFilter);
//            foreach (EHierarchyLevel hlevel in hierarchyLevelList)
//            {
//                tmpDataTable.Columns.Add(hlevel.HLevelDesc, typeof(string));
//                hierarchyLevelHashTable.Add(hlevel.HLevelID, hlevel);
//            }

//            tmpDataTable.Columns.Add(FIELD_AS_OF_DATE, typeof(DateTime));
//            tmpDataTable.Columns.Add(FIELD_REST_DAY_ENTITLED, typeof(double));
//            tmpDataTable.Columns.Add(FIELD_REST_DAY_TAKEN, typeof(double));
//            tmpDataTable.Columns.Add(FIELD_TOTAL_WORKING_DAYS, typeof(double));
//            tmpDataTable.Columns.Add(FIELD_TOTAL_WORKING_HOURS, typeof(double));
//            tmpDataTable.Columns.Add(FIELD_TOTAL_LUNCH_HOURS, typeof(double));

//            //  Extra Information
//            tmpDataTable.Columns.Add(FIELD_EXTRA_TOTAL_STATUTORY_HOLIDAY, typeof(double));
//            tmpDataTable.Columns.Add(FIELD_EXTRA_TOTAL_LEAVE_APPLICATION_TAKEN, typeof(double));

//            tmpDataTable.Columns.Add(FIELD_EXTRA_TOTAL_WORKING_HOURS_EXPECTED, typeof(double));
//            tmpDataTable.Columns.Add(FIELD_EXTRA_TOTAL_OVERTIME_MINS, typeof(double));
//            tmpDataTable.Columns.Add(FIELD_EXTRA_TOTAL_LATE_MINS, typeof(double));
//            tmpDataTable.Columns.Add(FIELD_EXTRA_TOTAL_EARLYLEAVE_MINS, typeof(double));


//            foreach (EEmpPersonalInfo empInfo in EmpInfoList)
//            {
//                if (EEmpPersonalInfo.db.select(dbConn, empInfo))
//                {

//                    DBFilter empPosFilter = new DBFilter();
//                    empPosFilter.add(new Match("EmpID", empInfo.EmpID));
//                    empPosFilter.add(new Match("EmpPosEffFr", "<=", PeriodTo));
//                    OR empPosEffOR = new OR();
//                    empPosEffOR.add(new Match("EmpPosEffTo", ">=", PeriodFrom));
//                    empPosEffOR.add(new NullTerm("EmpPosEffTo"));
//                    empPosFilter.add(empPosEffOR);
//                    empPosFilter.add("EmpPosEffFr", true);

//                    ArrayList empPosList = EEmpPositionInfo.db.select(dbConn, empPosFilter);

//                    EWorkHourPattern currentWorkHourPattern = null;

//                    DateTime lastStartDate = PeriodFrom;
//                    foreach (EEmpPositionInfo empPos in empPosList)
//                    {
//                        if (currentWorkHourPattern == null)
//                        {
//                            EWorkHourPattern tempWorkHourPattern = new EWorkHourPattern();
//                            tempWorkHourPattern.WorkHourPatternID = empPos.WorkHourPatternID;
//                            if (EWorkHourPattern.db.select(dbConn, tempWorkHourPattern))
//                            {
//                                currentWorkHourPattern = tempWorkHourPattern;
//                                if (empPos.EmpPosEffFr > PeriodFrom)
//                                {
//                                    //  generate working summary before the period for first join date

//                                    CreateWorkingSummaryTemplateRow(dbConn, tmpDataTable, hierarchyLevelHashTable, empInfo, lastStartDate, empPos.EmpPosEffFr.AddDays(-1), currentWorkHourPattern);
//                                    lastStartDate = empPos.EmpPosEffFr;
//                                }
//                            }

//                        }
//                        else if (currentWorkHourPattern.WorkHourPatternID != empPos.WorkHourPatternID)
//                        {

//                            EWorkHourPattern tempWorkHourPattern = new EWorkHourPattern();
//                            tempWorkHourPattern.WorkHourPatternID = empPos.WorkHourPatternID;
//                            if (EWorkHourPattern.db.select(dbConn, tempWorkHourPattern))
//                            {
//                                CreateWorkingSummaryTemplateRow(dbConn, tmpDataTable, hierarchyLevelHashTable, empInfo, lastStartDate, empPos.EmpPosEffFr.AddDays(-1), currentWorkHourPattern);
//                                lastStartDate = empPos.EmpPosEffFr;

//                                currentWorkHourPattern = tempWorkHourPattern;
//                            }
//                        }
//                    }
//                    EEmpTermination empTermination = EEmpTermination.GetObjectByEmpID(dbConn, empInfo.EmpID);
//                    if (empTermination !=null)
//                        if (empTermination.EmpTermLastDate >= lastStartDate && empTermination.EmpTermLastDate < PeriodTo)
//                        {
//                            CreateWorkingSummaryTemplateRow(dbConn, tmpDataTable, hierarchyLevelHashTable, empInfo, lastStartDate, empTermination.EmpTermLastDate, currentWorkHourPattern);
//                            lastStartDate = empTermination.EmpTermLastDate.AddDays(1);
//                        }

//                    CreateWorkingSummaryTemplateRow(dbConn, tmpDataTable, hierarchyLevelHashTable, empInfo, lastStartDate, PeriodTo, currentWorkHourPattern);
//                    lastStartDate = PeriodTo.AddDays(1);

//                }
//            }
//            return tmpDataTable;
//        }

//        private static DataRow CreateWorkingSummaryRow(DatabaseConnection dbConn, DataTable dataTable, Hashtable hierarchyLevelHashTable, EEmpPersonalInfo empInfo, DateTime AsOfDate, double RestDayEntitled, double RestDayTaken, double TotalWorkingDays, double TotalWorkingHours)
//        {
//            DataRow row = dataTable.NewRow();
//            row[FIELD_EMP_NO] = empInfo.EmpNo;
//            row["EnglishName"] = empInfo.EmpEngFullName;
//            row["ChineseName"] = empInfo.EmpChiFullName;

//            DBFilter empPosFilter = new DBFilter();

//            EEmpPositionInfo empPos = AppUtils.GetLastPositionInfo(dbConn, AsOfDate, empInfo.EmpID);
//            if (empPos != null)
//            {
//                ECompany company = new ECompany();
//                company.CompanyID = empPos.CompanyID;
//                if (ECompany.db.select(dbConn, company))
//                    row["Company"] = company.CompanyCode;
//                DBFilter empHierarchyFilter = new DBFilter();
//                empHierarchyFilter.add(new Match("EmpPosID", empPos.EmpPosID));
//                ArrayList empHierarchyList = EEmpHierarchy.db.select(dbConn, empHierarchyFilter);
//                foreach (EEmpHierarchy empHierarchy in empHierarchyList)
//                {
//                    EHierarchyLevel hierarchyLevel = (EHierarchyLevel)hierarchyLevelHashTable[empHierarchy.HLevelID];
//                    if (hierarchyLevel != null)
//                    {
//                        EHierarchyElement hierarchyElement = new EHierarchyElement();
//                        hierarchyElement.HElementID = empHierarchy.HElementID;
//                        if (EHierarchyElement.db.select(dbConn, hierarchyElement))
//                            row[hierarchyLevel.HLevelDesc] = hierarchyElement.HElementCode;
//                    }
//                }
//            }

//            row[FIELD_AS_OF_DATE] = AsOfDate;
//            row[FIELD_REST_DAY_ENTITLED] = RestDayEntitled;
//            row[FIELD_REST_DAY_TAKEN] = RestDayTaken;
//            row[FIELD_TOTAL_WORKING_DAYS] = TotalWorkingDays;
//            row[FIELD_TOTAL_WORKING_HOURS] = TotalWorkingHours;
//            dataTable.Rows.Add(row);
//            return row;
//        }

//        private static DataRow CreateWorkingSummaryTemplateRow(DatabaseConnection dbConn, DataTable dataTable, Hashtable hierarchyLevelHashTable, EEmpPersonalInfo empInfo, DateTime PeriodFrom, DateTime PeriodTo, EWorkHourPattern currentWorkHourPattern)
//        {
//            double restDayCount = 0;
//            double statutoryHolidayCount = 0;
//            double restDayTaken = 0;
//            double statutoryHolidayTaken = 0;
//            double workHourPatternWorkingDaysCount = 0;
//            double workHourPatternLunchTimeCount = 0;
//            double workHourPatternWorkingHourCount = 0;
//            double totalWorkingDays = 0;
//            double totalWorkingHours = 0;
//            double totalLunchMins = 0;
//            double totalWorkingHoursExpected = 0;
//            double totalLateMins = 0;
//            double totalEarlyLeaveMins = 0;
//            double totalOvertimeMins = 0;
//            double totalLeaveApplicationDayTaken = 0;

//            EEmpTermination empTermination = EEmpTermination.GetObjectByEmpID(dbConn, empInfo.EmpID);

//            bool hasStatutoryHolidayRosterCode = false;
//            //  Check if Roster Code List contains Statutory Holiday Roster Code
//            ArrayList rosterCodeList = ERosterCode.db.select(dbConn, new DBFilter());
//            foreach (ERosterCode rosterCode in rosterCodeList)
//            {
//                if (rosterCode.RosterCodeType.Equals(ERosterCode.ROSTERTYPE_CODE_STATUTORYHOLIDAY))
//                {
//                    hasStatutoryHolidayRosterCode = true;
//                    break;
//                }
//            }

//            if (currentWorkHourPattern != null)
//            {

//                for (DateTime asOfDate = PeriodFrom; asOfDate <= PeriodTo; asOfDate = asOfDate.AddDays(1))
//                {
//                    if (EStatutoryHoliday.IsHoliday(dbConn, asOfDate))// && currentWorkHourPattern.WorkHourPatternUseStatutoryHolidayTable)
//                        statutoryHolidayCount++;
//                    //rest day must be integer and rest within 24 hour
//                    restDayCount += (1 - currentWorkHourPattern.GetDefaultDayUnit(dbConn, asOfDate, false, false));

//                    double workDayUnit = 0;
//                    double workHourUnit = 0;
//                    double LunchTimeUnit = 0;
//                    if (empInfo.EmpDateOfJoin <= asOfDate)
//                    {
//                        workDayUnit = currentWorkHourPattern.GetDefaultDayUnit(dbConn, asOfDate, false, false);
//                        LunchTimeUnit = currentWorkHourPattern.GetDefaultLunch(dbConn, asOfDate);
//                        workHourUnit = currentWorkHourPattern.GetDefaultWorkHour(dbConn, asOfDate);
//                        DBFilter leaveAppFilter = new DBFilter();
//                        leaveAppFilter.add(new Match("EmpID", empInfo.EmpID));
//                        leaveAppFilter.add(new Match("LeaveAppDateFrom", "<=", asOfDate));
//                        leaveAppFilter.add(new Match("LeaveAppDateTo", ">=", asOfDate));
//                        leaveAppFilter.add(new Match("LeaveAppNoPayProcess", false));

//                        ArrayList leaveAppList = ELeaveApplication.db.select(dbConn, leaveAppFilter);
//                        foreach (ELeaveApplication leaveApp in leaveAppList)
//                        {
//                            if (leaveApp.LeaveAppDateFrom.Equals(leaveApp.LeaveAppDateTo))
//                            {
//                                workDayUnit -= leaveApp.LeaveAppDays;
//                                double currentDayDefaultDayUnit = currentWorkHourPattern.GetDefaultDayUnit(dbConn, asOfDate, false, false);
//                                if (currentDayDefaultDayUnit * leaveApp.LeaveAppDays > 0)
//                                    workHourUnit -= currentWorkHourPattern.GetDefaultWorkHour(dbConn, asOfDate) / currentDayDefaultDayUnit * leaveApp.LeaveAppDays;
//                            }
//                            else
//                            {
//                                workDayUnit = 0;
//                                workHourUnit = 0;
//                                LunchTimeUnit = 0;
//                            }
//                        }
//                        if (workDayUnit < 0) workDayUnit = 0;
//                        if (workHourUnit < 0) workHourUnit = 0;
//                        if (workDayUnit < 1) LunchTimeUnit = 0;
//                        if (empTermination != null)
//                            if (empTermination.EmpTermLastDate < asOfDate)
//                            {
//                                workDayUnit = 0;
//                                workHourUnit = 0;
//                                LunchTimeUnit = 0;
//                            }
//                    }
//                    workHourPatternWorkingDaysCount += workDayUnit;
//                    workHourPatternWorkingHourCount += workHourUnit;
//                    workHourPatternLunchTimeCount += LunchTimeUnit;
//                }
//                DBFilter leaveAppTakenFilter = new DBFilter();
//                leaveAppTakenFilter.add(new Match("EmpID", empInfo.EmpID));
//                leaveAppTakenFilter.add(new Match("LeaveAppDateFrom", "<=", PeriodTo));
//                leaveAppTakenFilter.add(new Match("LeaveAppDateTo", ">=", PeriodFrom));
//                leaveAppTakenFilter.add(new Match("LeaveAppNoPayProcess", false));
//                ArrayList leaveAppTakenList = ELeaveApplication.db.select(dbConn, leaveAppTakenFilter);
//                foreach (ELeaveApplication leaveApp in leaveAppTakenList)
//                {
//                    totalLeaveApplicationDayTaken += leaveApp.LeaveAppDays;
//                }
//            }
//            DBFilter attendanceRecordFilter = new DBFilter();
//            attendanceRecordFilter.add(new Match("EmpID", empInfo.EmpID));
//            attendanceRecordFilter.add(new Match("AttendanceRecordDate", ">=", PeriodFrom));
//            attendanceRecordFilter.add(new Match("AttendanceRecordDate", "<=", PeriodTo));
//            attendanceRecordFilter.add("AttendanceRecordDate", true);
//            ArrayList attendanceRecordList = EAttendanceRecord.db.select(dbConn, attendanceRecordFilter);
//            foreach (EAttendanceRecord attendanceRecord in attendanceRecordList)
//            {
//                ERosterCode rosterCode = new ERosterCode();
//                rosterCode.RosterCodeID = attendanceRecord.RosterCodeID;
//                if (ERosterCode.db.select(dbConn, rosterCode))
//                {
//                    if (rosterCode.RosterCodeType.Equals(ERosterCode.ROSTERTYPE_CODE_RESTDAY))
//                        restDayTaken++;
//                    if (rosterCode.RosterCodeType.Equals(ERosterCode.ROSTERTYPE_CODE_STATUTORYHOLIDAY))
//                        statutoryHolidayTaken++;
//                }
//                double workingHours = attendanceRecord.TotalWorkingHourTimeSpan(dbConn).TotalHours;
//                if (workingHours <= 0)
//                    workingHours = attendanceRecord.AttendanceRecordActualWorkingHour + Convert.ToDouble(attendanceRecord.AttendanceRecordActualEarlyLeaveMins - attendanceRecord.AttendanceRecordActualLateMins + attendanceRecord.AttendanceRecordActualOvertimeMins) / 60.0;

//                totalWorkingDays += attendanceRecord.AttendanceRecordActualWorkingDay;
//                totalWorkingHours += workingHours;
//                totalWorkingHoursExpected += attendanceRecord.AttendanceRecordActualWorkingHour;
//                totalLateMins += attendanceRecord.AttendanceRecordActualLateMins;
//                totalEarlyLeaveMins += attendanceRecord.AttendanceRecordActualEarlyLeaveMins;
//                totalOvertimeMins += attendanceRecord.AttendanceRecordActualOvertimeMins;
//                totalLunchMins += attendanceRecord.AttendanceRecordActualLunchTimeMins;
//            }
//            if (totalWorkingDays <= 0 && totalWorkingHours <= 0 && totalLunchMins <= 0 && empInfo.EmpDateOfJoin <= PeriodFrom)
//            {
//                totalWorkingDays = workHourPatternWorkingDaysCount;
//                totalWorkingHours = workHourPatternWorkingHourCount;
//                totalLunchMins = workHourPatternLunchTimeCount * 60;
//            }
//            DataRow row = dataTable.NewRow();
//            row[FIELD_EMP_NO] = empInfo.EmpNo;
//            row["EnglishName"] = empInfo.EmpEngFullName;
//            row["ChineseName"] = empInfo.EmpChiFullName;

//            DBFilter empPosFilter = new DBFilter();

//            EEmpPositionInfo empPos = AppUtils.GetLastPositionInfo(dbConn, PeriodTo, empInfo.EmpID);
//            if (empPos != null)
//            {
//                ECompany company = new ECompany();
//                company.CompanyID = empPos.CompanyID;
//                if (ECompany.db.select(dbConn, company))
//                    row["Company"] = company.CompanyCode;
//                DBFilter empHierarchyFilter = new DBFilter();
//                empHierarchyFilter.add(new Match("EmpPosID", empPos.EmpPosID));
//                ArrayList empHierarchyList = EEmpHierarchy.db.select(dbConn, empHierarchyFilter);
//                foreach (EEmpHierarchy empHierarchy in empHierarchyList)
//                {
//                    EHierarchyLevel hierarchyLevel = (EHierarchyLevel)hierarchyLevelHashTable[empHierarchy.HLevelID];
//                    if (hierarchyLevel != null)
//                    {
//                        EHierarchyElement hierarchyElement = new EHierarchyElement();
//                        hierarchyElement.HElementID = empHierarchy.HElementID;
//                        if (EHierarchyElement.db.select(dbConn, hierarchyElement))
//                            row[hierarchyLevel.HLevelDesc] = hierarchyElement.HElementCode;
//                    }
//                }
//            }

//            row[FIELD_AS_OF_DATE] = PeriodTo;
//            row[FIELD_REST_DAY_ENTITLED] = restDayCount - statutoryHolidayCount;
//            if (hasStatutoryHolidayRosterCode)
//                row[FIELD_REST_DAY_TAKEN] = restDayTaken;
//            else
//                row[FIELD_REST_DAY_TAKEN] = restDayTaken > statutoryHolidayCount ? restDayTaken - statutoryHolidayCount : 0;
//            row[FIELD_TOTAL_WORKING_DAYS] = totalWorkingDays;
//            row[FIELD_TOTAL_WORKING_HOURS] = totalWorkingHours;
//            row[FIELD_TOTAL_LUNCH_HOURS] = totalLunchMins / 60.0;
//            row[FIELD_EXTRA_TOTAL_STATUTORY_HOLIDAY] = statutoryHolidayCount;
//            row[FIELD_EXTRA_TOTAL_LEAVE_APPLICATION_TAKEN] = totalLeaveApplicationDayTaken;
//            row[FIELD_EXTRA_TOTAL_WORKING_HOURS_EXPECTED] = totalWorkingHoursExpected;
//            row[FIELD_EXTRA_TOTAL_LATE_MINS] = totalLateMins;
//            row[FIELD_EXTRA_TOTAL_EARLYLEAVE_MINS] = totalEarlyLeaveMins;
//            row[FIELD_EXTRA_TOTAL_OVERTIME_MINS] = totalOvertimeMins;
//            dataTable.Rows.Add(row);
//            return row;
//        }
//    }
//}