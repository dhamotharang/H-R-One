using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Globalization;
using HROne.Lib.Entities;
using HROne.LeaveCalc;
using HROne.DataAccess;

namespace HROne.Import
{

    /// <summary>
    /// Summary description for ImportRosterTable
    /// </summary>
    public class ImportRosterTableProcess : ImportProcessInteface
    {
        private const string FIELD_EMP_NO = "Emp. No";
        private const int ROW_MONTH = 1;
        private const int ROW_YEAR = 0;
        private const int ROW_CALENDAR_HEADER = 2;
        private int m_importYear = 0;
        private int m_importMonth = 0;


        
        private DateTime UploadDateTime = AppUtils.ServerDateTime();

        [DBClass("UploadRosterTable")]
        private class EUploadRosterTable : ImportDBObject
        {
            public static DBManager db = new DBManager(typeof(EUploadRosterTable));

            protected int m_UploadRosterTableID;
            [DBField("UploadRosterTableID", true, true), TextSearch, Export(false)]
            public int UploadRosterTableID
            {
                get { return m_UploadRosterTableID; }
                set { m_UploadRosterTableID = value; modify("UploadRosterTableID"); }
            }

            protected int m_RosterTableID;
            [DBField("RosterTableID"), TextSearch, Export(false)]
            public int RosterTableID
            {
                get { return m_RosterTableID; }
                set { m_RosterTableID = value; modify("RosterTableID"); }
            }

            protected int m_EmpID;
            [DBField("EmpID"), TextSearch, Export(false)]
            public int EmpID
            {
                get { return m_EmpID; }
                set { m_EmpID = value; modify("EmpID"); }
            }

            protected DateTime m_RosterTableDate;
            [DBField("RosterTableDate"), TextSearch, Export(false)]
            public DateTime RosterTableDate
            {
                get { return m_RosterTableDate; }
                set { m_RosterTableDate = value; modify("RosterTableDate"); }
            }
            protected int m_RosterCodeID;
            [DBField("RosterCodeID"), TextSearch, Export(false)]
            public int RosterCodeID
            {
                get { return m_RosterCodeID; }
                set { m_RosterCodeID = value; modify("RosterCodeID"); }
            }
            protected DateTime m_RosterTableOverrideInTime;
            [DBField("RosterTableOverrideInTime", "HH:mm"), TextSearch, Export(false)]
            public DateTime RosterTableOverrideInTime
            {
                get { return m_RosterTableOverrideInTime; }
                set { m_RosterTableOverrideInTime = value; modify("RosterTableOverrideInTime"); }
            }
            protected DateTime m_RosterTableOverrideOutTime;
            [DBField("RosterTableOverrideOutTime", "HH:mm"), TextSearch, Export(false)]
            public DateTime RosterTableOverrideOutTime
            {
                get { return m_RosterTableOverrideOutTime; }
                set { m_RosterTableOverrideOutTime = value; modify("RosterTableOverrideOutTime"); }
            }

        }



        public ImportErrorList errors = new ImportErrorList();

        public ImportRosterTableProcess(DatabaseConnection dbConn, string SessionID): base(dbConn, SessionID)
        {
          
        }




        public override DataTable UploadToTempDatabase(string Filename, int UserID, string ZipPassword)
        {
            ClearTempTable();

            NPOI.HSSF.UserModel.HSSFWorkbook workBook = new NPOI.HSSF.UserModel.HSSFWorkbook(new System.IO.FileStream(Filename, System.IO.FileMode.Open)); // ExcelLibrary.SpreadSheet.Workbook.Load(Filename);
            NPOI.HSSF.UserModel.HSSFSheet workSheet = null;

            //foreach (ExcelLibrary.SpreadSheet.Worksheet tmpWorkSheet in workBook.Worksheets)
            //{
            //    if (tmpWorkSheet.Name.Trim().Equals("RosterTable"))
            //    {
            //        workSheet = tmpWorkSheet;
            //        break;
            //    }
            //}
            try
            {
                workSheet = (NPOI.HSSF.UserModel.HSSFSheet)workBook.GetSheet("RosterTable");
                if (workSheet == null)
                    workSheet = (NPOI.HSSF.UserModel.HSSFSheet)workBook.GetSheetAt(0);

            }
            catch
            {
                if (workSheet == null)
                    workSheet = (NPOI.HSSF.UserModel.HSSFSheet)workBook.GetSheetAt(0);
            }
            if (workSheet.GetRow(ROW_YEAR).GetCell(0).StringCellValue.Trim().StartsWith("Year", StringComparison.CurrentCultureIgnoreCase))
            {
                if (workSheet.GetRow(ROW_YEAR).GetCell(1).CellType.Equals(NPOI.SS.UserModel.CellType.NUMERIC))
                    m_importYear = Convert.ToInt32(workSheet.GetRow(ROW_YEAR).GetCell(1).NumericCellValue);
                else
                    errors.addError("Invalid Year", null);
            }
            if (workSheet.GetRow(ROW_MONTH).GetCell(0).StringCellValue.Trim().StartsWith("Month", StringComparison.CurrentCultureIgnoreCase))
            {
                if (workSheet.GetRow(ROW_MONTH).GetCell(1).CellType.Equals(NPOI.SS.UserModel.CellType.NUMERIC))
                    m_importMonth = Convert.ToInt32(workSheet.GetRow(ROW_MONTH).GetCell(1).NumericCellValue);
                else
                    errors.addError("Invalid Month", null);

                //if (!int.TryParse(workSheet.GetRow(1).GetCell(1).StringCellValue.Trim(), out m_importMonth))
                //    errors.addError("Invalid Month", null);
            }

            if (errors.List.Count > 0)
            {
                throw (new HRImportException(errors.Message()));
            }

            int intHeaterRow = ROW_CALENDAR_HEADER;
            int intEmpColumn = 0;

            ArrayList results = new ArrayList();
            int rowCount = 1;

            NPOI.HSSF.UserModel.HSSFRow headerRow = (NPOI.HSSF.UserModel.HSSFRow)workSheet.GetRow(intHeaterRow);

            while (intEmpColumn <= headerRow.LastCellNum)
            {
                if (headerRow.GetCell(intEmpColumn) != null)
                    if (headerRow.GetCell(intEmpColumn).StringCellValue.Trim().Equals(FIELD_EMP_NO, StringComparison.CurrentCultureIgnoreCase))
                    {
                        break;
                    }
                intEmpColumn++;
            }
            if (intEmpColumn > headerRow.LastCellNum)
            {
                //  do exception
            }

            int intEmptyEmpNoCount = 0;

            try
            {
                while (intHeaterRow + rowCount <= workSheet.LastRowNum)
                {
                    int EmpID = 0;
                    string EmpNo = string.Empty;
                    int colCount = 0;

                    NPOI.HSSF.UserModel.HSSFRow row = (NPOI.HSSF.UserModel.HSSFRow)workSheet.GetRow(intHeaterRow + rowCount);
                    if (row == null)
                    {
                        rowCount++;
                        intEmptyEmpNoCount++;
                        continue;
                    }
                    if (row.GetCell(intEmpColumn) == null)
                    {
                        rowCount++;
                        intEmptyEmpNoCount++;
                        continue;
                    }

                    EmpNo = row.GetCell(intEmpColumn).ToString().Trim();

                    if (string.IsNullOrEmpty(EmpNo))
                    {
                        rowCount++;
                        intEmptyEmpNoCount++;
                        continue;
                    }

                    intEmptyEmpNoCount = 0;
                    EmpID = Import.Parse.GetEmpID(dbConn, EmpNo, UserID);
                    if (EmpID < 0)
                        errors.addError(ImportErrorMessage.ERROR_ACCESS_DENIED_EMP_NO, new string[] { EmpNo, rowCount.ToString() });
                    else if (EmpID == 0)
                        errors.addError(ImportErrorMessage.ERROR_INVALID_EMP_NO, new string[] { EmpNo, rowCount.ToString() });
                    colCount = intEmptyEmpNoCount + 1;
                    int lastImportDay = 0;
                    int intCurrentMonth = m_importMonth;
                    int intCurrentYear = m_importYear;
                    while (colCount < headerRow.LastCellNum)
                    {

                        if (headerRow.GetCell(colCount) == null)
                        {
                            if (lastImportDay <= 0)
                            {
                                colCount++;
                                continue;
                            }
                            else
                                break;
                        }
                        if (headerRow.GetCell(colCount).CellType.Equals(NPOI.SS.UserModel.CellType.NUMERIC))
                        {
                            int day = Convert.ToInt32(headerRow.GetCell(colCount).NumericCellValue);
                            if (day > 0 && day <= DateTime.DaysInMonth(intCurrentYear, intCurrentMonth))
                            {
                                if (lastImportDay > day)
                                {
                                    intCurrentMonth++;
                                    if (intCurrentMonth > 12)
                                    {
                                        intCurrentMonth = 1;
                                        intCurrentYear++;
                                    }
                                }
                                lastImportDay = day;

                                string RosterCode;
                                if (row.GetCell(colCount) != null)
                                    RosterCode = row.GetCell(colCount).ToString().Trim();
                                else
                                    RosterCode = string.Empty;

                                EUploadRosterTable uploadRosterTable = new EUploadRosterTable();
                                uploadRosterTable.EmpID = EmpID;
                                uploadRosterTable.RosterTableDate = new DateTime(intCurrentYear, intCurrentMonth, day);
                                DateTime inTime, outTime;
                                uploadRosterTable.RosterCodeID = Import.Parse.GetRosterCodeID(dbConn, RosterCode, out inTime, out outTime);
                                if (!inTime.Ticks.Equals(0))
                                    uploadRosterTable.RosterTableOverrideInTime = inTime;
                                if (!outTime.Ticks.Equals(0))
                                    uploadRosterTable.RosterTableOverrideOutTime = outTime;
                                uploadRosterTable.SessionID = m_SessionID;
                                uploadRosterTable.TransactionDate = UploadDateTime;
                                uploadRosterTable.ImportActionStatus = ImportDBObject.ImportActionEnum.UPDATE;
                                if (uploadRosterTable.RosterCodeID <= 0 && !string.IsNullOrEmpty(RosterCode))
                                {
                                    errors.addError(ImportErrorMessage.ERROR_INVALID_FIELD_VALUE, new string[] { RosterCode, EmpNo, rowCount.ToString() });
                                }
                                else
                                    EUploadRosterTable.db.insert(dbConn, uploadRosterTable);
                            }
                        }

                        colCount++;
                    }

                    rowCount++;
                }
            }
            catch (Exception e)
            {
                errors.addError(e.Message, null);
            }
            if (errors.List.Count > 0)
            {
                ClearTempTable();
                throw (new HRImportException(errors.Message()));
            }
            return GetImportDataFromTempDatabase(null);

            //org.in2bits.MyXls.XlsDocument xlsDoc = new org.in2bits.MyXls.XlsDocument(Filename);
            //org.in2bits.MyXls.Worksheet workSheet;

            //try
            //{
            //    workSheet = xlsDoc.Workbook.Worksheets["RosterTable"];

            //}
            //catch
            //{
            //    workSheet = xlsDoc.Workbook.Worksheets[0];
            //}

            //if (workSheet.Rows[1].GetCell(1).Value.ToString().Trim().Equals("Year", StringComparison.CurrentCultureIgnoreCase))
            //{

            //    if (!int.TryParse(workSheet.Rows[1].CellAtCol(2).Value.ToString().Trim(), out m_importYear))
            //        errors.addError("Invalid Year", null);
            //}
            //if (workSheet.Rows[2].GetCell(1).Value.ToString().Trim().Equals("Month", StringComparison.CurrentCultureIgnoreCase))
            //{

            //    if (!int.TryParse(workSheet.Rows[2].CellAtCol(2).Value.ToString().Trim(), out m_importMonth))
            //        errors.addError("Invalid Month", null);
            //}

            //if (errors.List.Count > 0)
            //{
            //    throw (new HRImportException(errors.Message()));
            //}

            //ushort intHeaderRow = 3;


            //ArrayList results = new ArrayList();
            //ushort rowCount = 1;

            //try
            //{
            //    while (intHeaderRow + rowCount <= workSheet.Rows.MaxRow)
            //    {
            //        int EmpID = 0;
            //        string EmpNo = string.Empty;
            //        ushort colCount = 1;
            //        org.in2bits.MyXls.Row row = workSheet.Rows[(ushort)(intHeaderRow + rowCount)];
            //        while (colCount <= row.MaxCellCol)
            //        {
            //            if (workSheet.Rows[intHeaderRow].CellAtCol(colCount).Value.ToString().Trim().Equals(FIELD_EMP_NO, StringComparison.CurrentCultureIgnoreCase))
            //            {
            //                EmpNo = row.CellAtCol(colCount).Value.ToString().Trim();
            //                EmpID = Import.Parse.GetEmpID(dbConn, EmpNo, UserID);
            //                if (EmpID <= 0)
            //                    errors.addError(ImportErrorMessage.ERROR_INVALID_EMP_NO, new string[] { EmpNo, rowCount.ToString() });
            //                break;
            //            }
            //            colCount++;
            //        }

            //        colCount = 1;
            //        while (colCount <= workSheet.Rows[(ushort)(intHeaderRow + rowCount)].MaxCellCol)
            //        {
            //            if (workSheet.Cells .Rows[(ushort)(intHeaderRow)].CellExists(colCount))
            //            {
            //                string RosterCode = string.Empty;
            //                if (workSheet.Rows[(ushort)(intHeaderRow + rowCount)].CellExists(colCount))
            //                    if (workSheet.Rows[(ushort)(intHeaderRow + rowCount)].CellAtCol(colCount).Value != null)
            //                        RosterCode = row.CellAtCol(colCount).Value.ToString().Trim();
            //                int day = 0;
            //                if (int.TryParse(workSheet.Rows[intHeaderRow].GetCell(colCount).Value.ToString().Trim(), out day))
            //                {
            //                    if (day > 0 && day <= DateTime.DaysInMonth(m_importYear, m_importMonth))
            //                    {
            //                        EUploadRosterTable uploadRosterTable = new EUploadRosterTable();
            //                        uploadRosterTable.EmpID = EmpID;
            //                        uploadRosterTable.RosterTableDate = new DateTime(m_importYear, m_importMonth, day);
            //                        uploadRosterTable.RosterCodeID = Import.Parse.*ID(dbConn, RosterCode);
            //                        uploadRosterTable.SessionID = m_SessionID;
            //                        uploadRosterTable.TransactionDate = UploadDateTime;
            //                        uploadRosterTable.ImportActionStatus = ImportDBObject.ImportActionEnum.UPDATE;
            //                        if (uploadRosterTable.RosterCodeID <= 0 && !string.IsNullOrEmpty(RosterCode))
            //                        {
            //                            errors.addError(ImportErrorMessage.ERROR_INVALID_FIELD_VALUE, new string[] { RosterCode, EmpNo, rowCount.ToString() });
            //                        }
            //                        else
            //                            EUploadRosterTable.db.insert(dbConn, uploadRosterTable);
            //                    }
            //                }
            //            }
            //            colCount++;
            //        }

            //        rowCount++;
            //    }
            //}
            //catch (Exception e)
            //{
            //    errors.addError(e.Message, null);
            //}
            //if (errors.List.Count > 0)
            //{
            //    ClearTempTable();
            //    throw (new HRImportException(errors.Message()));
            //}
            //return GetImportDataFromTempDatabase(null);
        }


        public override DataTable GetImportDataFromTempDatabase(ListInfo info)
        {
            DBFilter sessionFilter = new DBFilter();
            sessionFilter.add(new Match("SessionID", m_SessionID));
            sessionFilter.add("EmpID", true);
            //if (info != null && info.orderby != null && !info.orderby.Equals(""))
            //    sessionFilter.add(info.orderby, info.order);
            ArrayList uploadRosterTableList = EUploadRosterTable.db.select(dbConn, sessionFilter);

            DataTable resultTable = new DataTable();
            resultTable.Columns.Add("EmpNo", typeof(string));
            resultTable.Columns.Add("EmpName", typeof(string));
            for (int day = 1; day <= 31; day++)
                resultTable.Columns.Add("Roster" + day.ToString("00"), typeof(string));

            int lastEmpID = 0;
            DataRow rosterTableRow = null;
            foreach (EUploadRosterTable uploadRosterTable in uploadRosterTableList)
            {
                if (!lastEmpID.Equals(uploadRosterTable.EmpID))
                {
                    lastEmpID = uploadRosterTable.EmpID;
                    rosterTableRow = resultTable.NewRow();
                    resultTable.Rows.Add(rosterTableRow);

                    EEmpPersonalInfo empInfo = new EEmpPersonalInfo();
                    empInfo.EmpID = uploadRosterTable.EmpID;
                    if (EEmpPersonalInfo.db.select(dbConn, empInfo))
                    {
                        rosterTableRow["EmpNo"] = empInfo.EmpNo;
                        rosterTableRow["EmpName"] = empInfo.EmpEngFullName;
                    }
                }
                ERosterCode rosterCode = new ERosterCode();
                rosterCode.RosterCodeID = uploadRosterTable.RosterCodeID;
                if (ERosterCode.db.select(dbConn, rosterCode))
                    rosterTableRow["Roster" + uploadRosterTable.RosterTableDate.ToString("dd")] = rosterCode.RosterCode;

            }
            return resultTable;
        }

        public static void ClearTempTable(DatabaseConnection dbConn, string sessionID)
        {
            ImportRosterTableProcess import = new ImportRosterTableProcess(dbConn, sessionID);
            import.ClearTempTable();
        }

        public override void ClearTempTable()
        {
            //  Clear Old Import Session
            DBFilter sessionFilter = new DBFilter();
            if (!string.IsNullOrEmpty(m_SessionID))
                sessionFilter.add(new Match("SessionID", m_SessionID));
            EUploadRosterTable.db.delete(dbConn, sessionFilter);
        }

        public override void ImportToDatabase()
        {
            DBFilter sessionFilter = new DBFilter();
            sessionFilter.add(new Match("SessionID", m_SessionID));
            sessionFilter.add("EmpID", true);

            DBFilter removeOldRosterTable = new DBFilter();
            DBFilter existsUploadRosterTable = new DBFilter();

            existsUploadRosterTable.add(new MatchField(ERosterTable.db.dbclass.tableName + ".EmpID", "ur.EmpID"));
            existsUploadRosterTable.add(new MatchField(ERosterTable.db.dbclass.tableName + ".RosterTableDate", "ur.RosterTableDate"));
            existsUploadRosterTable.add(new Match("ur.SessionID", m_SessionID));

            removeOldRosterTable.add(new Exists(EUploadRosterTable.db.dbclass.tableName + " ur", existsUploadRosterTable));
            //ArrayList recalLeaveBalanceEmpIDList = new ArrayList();
            //DateTime firstDate = new DateTime();
            ArrayList oldRosterTableList = ERosterTable.db.select(dbConn, removeOldRosterTable);

            foreach (ERosterTable oldRosterTable in oldRosterTableList)
            {
                //if (firstDate > oldRosterTable.RosterTableDate || firstDate.Ticks.Equals(0))
                //    firstDate = oldRosterTable.RosterTableDate;
                //if (oldRosterTable.LeaveAppID > 0)
                //{
                //    ELeaveApplication leaveApp = new ELeaveApplication();
                //    leaveApp.LeaveAppID = oldRosterTable.LeaveAppID;
                //    if (ELeaveApplication.db.select(dbConn, leaveApp))
                //        if (leaveApp.EmpPayrollID <= 0)
                //        {
                //            ELeaveApplication.db.delete(dbConn, leaveApp);
                //            if (!recalLeaveBalanceEmpIDList.Contains(oldRosterTable.EmpID))
                //                recalLeaveBalanceEmpIDList.Add(oldRosterTable.EmpID);
                //        }
                //}
                ERosterTable.db.delete(dbConn, oldRosterTable);

            }

            ArrayList uploadRosterTableList = EUploadRosterTable.db.select(dbConn, sessionFilter);
            foreach (EUploadRosterTable obj in uploadRosterTableList)
            {
                //if (firstDate > obj.RosterTableDate || firstDate.Ticks.Equals(0))
                //    firstDate = obj.RosterTableDate;
                if (obj.RosterCodeID > 0)
                {
                    ERosterCode rosterCode = new ERosterCode();
                    rosterCode.RosterCodeID = obj.RosterCodeID;
                    if (ERosterCode.db.select(dbConn, rosterCode))
                    {

                        ERosterTable rosterTable = new ERosterTable();
                        rosterTable.EmpID = obj.EmpID;
                        rosterTable.RosterTableDate = obj.RosterTableDate;
                        rosterTable.RosterCodeID = obj.RosterCodeID;
                        if (!obj.RosterTableOverrideInTime.Ticks.Equals(0) && !rosterCode.RosterCodeInTime.TimeOfDay.Equals(obj.RosterTableOverrideInTime.TimeOfDay))
                            rosterTable.RosterTableOverrideInTime = obj.RosterTableOverrideInTime;
                        if (!obj.RosterTableOverrideOutTime.Ticks.Equals(0) && !rosterCode.RosterCodeOutTime.TimeOfDay.Equals(obj.RosterTableOverrideOutTime.TimeOfDay))
                            rosterTable.RosterTableOverrideOutTime = obj.RosterTableOverrideOutTime;


                        //if (rosterCode.RosterCodeType.Equals(ERosterCode.ROSTERTYPE_CODE_LEAVE) && rosterCode.LeaveCodeID > 0)
                        //{
                        //    ELeaveCode leaveCode = new ELeaveCode();
                        //    leaveCode.LeaveCodeID = rosterCode.LeaveCodeID;
                        //    if (ELeaveCode.db.select(dbConn, leaveCode))
                        //    {
                        //        DBFilter leaveFilter = new DBFilter();
                        //        leaveFilter.add(new Match("EmpID", obj.EmpID));
                        //        leaveFilter.add(new Match("LeaveAppDateFrom", obj.RosterTableDate));
                        //        leaveFilter.add(new Match("LeaveAppDateTo", obj.RosterTableDate));
                        //        leaveFilter.add(new Match("LeaveAppDays", 1));
                        //        leaveFilter.add(new Match("LeaveCodeID", leaveCode.LeaveCodeID));
                        //        ArrayList oldLeaveAppList = ELeaveApplication.db.select(dbConn, leaveFilter);
                        //        if (oldLeaveAppList.Count > 0)
                        //            rosterTable.LeaveAppID = ((ELeaveApplication)oldLeaveAppList[0]).LeaveAppID;
                        //        else
                        //        {
                        //            ELeaveApplication leaveApp = new ELeaveApplication();
                        //            leaveApp.LeaveAppDateFrom = obj.RosterTableDate;
                        //            leaveApp.LeaveAppDateTo = obj.RosterTableDate;
                        //            leaveApp.LeaveAppDays = 1;
                        //            leaveApp.LeaveAppUnit = "D";
                        //            leaveApp.LeaveCodeID = leaveCode.LeaveCodeID;
                        //            leaveApp.EmpID = obj.EmpID;

                        //            ELeaveApplication.db.insert(dbConn, leaveApp);

                        //            rosterTable.LeaveAppID = leaveApp.LeaveAppID;
                        //        }
                        //        if (!recalLeaveBalanceEmpIDList.Contains(obj.EmpID))
                        //            recalLeaveBalanceEmpIDList.Add(obj.EmpID);
                        //    }
                        //}



                        ERosterTable.db.insert(dbConn, rosterTable);
                    }
                }
                EUploadRosterTable.db.delete(dbConn, obj);

            }
            //foreach (int EmpID in recalLeaveBalanceEmpIDList)
            //{
            //    LeaveBalanceCalc leaaveBalCal = new LeaveBalanceCalc(dbConn, EmpID);
            //    leaaveBalCal.RecalculateAfter(firstDate);
            //}
        }
    }
}