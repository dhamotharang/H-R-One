using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Globalization;
using HROne.Lib.Entities;
using HROne.DataAccess;

namespace HROne.Import
{

    /// <summary>
    /// Summary description for ImportRosterTable
    /// </summary>
    public class ImportTimeCardRecordProcess : ImportProcessInteface
    {



        
        private DateTime UploadDateTime = AppUtils.ServerDateTime();
        private DBManager db = EUploadTimeCardRecord.db;

        [DBClass("UploadTimeCardRecord")]
        private class EUploadTimeCardRecord : ImportDBObject
        {
            public static DBManager db = new DBManager(typeof(EUploadTimeCardRecord));

            protected int m_UploadTimeCardRecordID;
            [DBField("UploadTimeCardRecordID", true, true), TextSearch, Export(false)]
            public int UploadTimeCardRecordID
            {
                get { return m_UploadTimeCardRecordID; }
                set { m_UploadTimeCardRecordID = value; modify("UploadTimeCardRecordID"); }
            }

            protected int m_TimeCardRecordID;
            [DBField("TimeCardRecordID"), TextSearch, Export(false)]
            public int TimeCardRecordID
            {
                get { return m_TimeCardRecordID; }
                set { m_TimeCardRecordID = value; modify("TimeCardRecordID"); }
            }

            protected int m_EmpID;
            [DBField("EmpID"), TextSearch, Export(false)]
            public int EmpID
            {
                get { return m_EmpID; }
                set { m_EmpID = value; modify("EmpID"); }
            }

            protected string m_TimeCardRecordCardNo;
            [DBField("TimeCardRecordCardNo"), TextSearch, Export(false)]
            public string TimeCardRecordCardNo
            {
                get { return m_TimeCardRecordCardNo; }
                set { m_TimeCardRecordCardNo = value; modify("TimeCardRecordCardNo"); }
            }

            protected DateTime m_TimeCardRecordDateTime;
            [DBField("TimeCardRecordDateTime","yyyy-MM-dd HH:mm:ss"), TextSearch, Export(false)]
            public DateTime TimeCardRecordDateTime
            {
                get { return m_TimeCardRecordDateTime; }
                set { m_TimeCardRecordDateTime = value; modify("TimeCardRecordDateTime"); }
            }

            protected string m_TimeCardRecordLocation;
            [DBField("TimeCardRecordLocation"), TextSearch, Export(false)]
            public string TimeCardRecordLocation
            {
                get { return m_TimeCardRecordLocation; }
                set { m_TimeCardRecordLocation = value; modify("TimeCardRecordLocation"); }
            }

            protected ETimeCardRecord.TimeCardRecordInOutIndexEnum m_TimeCardRecordInOutIndex;
            [DBField("TimeCardRecordInOutIndex"), TextSearch, Export(false)]
            public ETimeCardRecord.TimeCardRecordInOutIndexEnum TimeCardRecordInOutIndex
            {
                get { return m_TimeCardRecordInOutIndex; }
                set { m_TimeCardRecordInOutIndex = value; modify("TimeCardRecordInOutIndex"); }
            }

            protected string m_TimeCardRecordOriginalData;
            [DBField("TimeCardRecordOriginalData"), TextSearch, Export(false)]
            public string TimeCardRecordOriginalData
            {
                get { return m_TimeCardRecordOriginalData; }
                set { m_TimeCardRecordOriginalData = value; modify("TimeCardRecordOriginalData"); }
            }
        }



        public ImportErrorList errors = new ImportErrorList();

        public ImportTimeCardRecordProcess(DatabaseConnection dbConn, string SessionID): base(dbConn, SessionID)
        {
          
        }

        public string ColumnDelimiter = string.Empty;
        public string DateSequence = string.Empty;
        public string YearFormat = string.Empty;
        public string DateSeparator = string.Empty;
        public string TimeSequence = "Hms";
        public string TimeSeparator = string.Empty;
        public int DateColumnIndex = 0;
        public int DateColumnIndex2 = 0;
        public int TimeColumnIndex = 0;
        public int TimeColumnIndex2 = 0;
        public int LocationColumnIndex = 0;
        public int TimeCardNumColumnIndex = 0;
        public bool UploadFileHasHeader = false;

        private DateTime ConvertDateWithFormatSetting(string DateString)
        {
            try
            {
                int Year = 0;
                int Month = 0;
                int Day = 0;

                string strDateString = DateString;

                foreach (char c in DateSequence.ToCharArray())
                {
                    if (string.IsNullOrEmpty(strDateString))
                        break;
                    if (c.Equals('Y'))
                        if (YearFormat.Equals("YYYY"))
                        {
                            Year = int.Parse(strDateString.Substring(0, 4));
                            strDateString = strDateString.Substring(4);
                        }
                        else
                        {
                            Year = int.Parse(strDateString.Substring(0, 2));
                            if (Year > 80)
                                Year += 1900;
                            else
                                Year += 2000;
                            strDateString = strDateString.Substring(2);
                        }
                    else if (c.Equals('M'))
                    {
                        Month = int.Parse(strDateString.Substring(0, 2));
                        strDateString = strDateString.Substring(2);
                    }
                    else if (c.Equals('D'))
                    {
                        Day = int.Parse(strDateString.Substring(0, 2));
                        strDateString = strDateString.Substring(2);
                    }
                    if (!string.IsNullOrEmpty(DateSeparator))
                    {
                        if (strDateString.StartsWith(DateSeparator))
                            strDateString = strDateString.Substring(1);
                        else
                        {
                            int pos = strDateString.IndexOf(DateSeparator);
                            if (pos >= 0)
                            {
                                strDateString = strDateString.Substring(pos + 1);
                            }
                        }
                    }
                }

                return new DateTime(Year, Month, Day);
            }
            catch
            {
                throw new Exception("Invalid Date/Time Format\nDate:" + DateString);

            }
        }

        private TimeSpan ConvertTimeWithFormatSetting(string TimeString)
        {
            try
            {
                int Hour = 0;
                int Minute = 0;
                int Second = 0;

                string strTimeString = TimeString;

                foreach (char c in TimeSequence.ToCharArray())
                {
                    if (string.IsNullOrEmpty(strTimeString))
                        break;
                    if (c.Equals('H'))
                    {
                        Hour = int.Parse(strTimeString.Substring(0, 2));
                        strTimeString = strTimeString.Substring(2);
                    }
                    else if (c.Equals('m'))
                    {
                        Minute = int.Parse(strTimeString.Substring(0, 2));
                        strTimeString = strTimeString.Substring(2);
                    }
                    else if (c.Equals('s'))
                    {
                        Second = int.Parse(strTimeString.Substring(0, 2));
                        strTimeString = strTimeString.Substring(2);
                    }
                    if (!string.IsNullOrEmpty(TimeSeparator))
                    {
                        if (strTimeString.StartsWith(TimeSeparator))
                            strTimeString = strTimeString.Substring(1);
                        else
                        {
                            int pos = strTimeString.IndexOf(TimeSeparator);
                            if (pos >= 0)
                            {
                                strTimeString = strTimeString.Substring(pos + 1);
                            }
                        }
                    }
                }
                return new TimeSpan(Hour, Minute, Second);
            }
            catch
            {
                throw new Exception("Invalid Time Format\nTime:" + TimeString);

            }
        }

        private DateTime ConvertDateTimeWithFormatSetting(string DateTimeString)
        {
            try
            {
                int Year = 0;
                int Month = 0;
                int Day = 0;

                string strDateTimeString = DateTimeString;

                foreach (char c in DateSequence.ToCharArray())
                {
                    if (string.IsNullOrEmpty(strDateTimeString))
                        break;
                    if (c.Equals('Y'))
                        if (YearFormat.Equals("YYYY"))
                        {
                            Year = int.Parse(strDateTimeString.Substring(0, 4));
                            strDateTimeString = strDateTimeString.Substring(4);
                        }
                        else
                        {
                            Year = int.Parse(strDateTimeString.Substring(0, 2));
                            if (Year > 80)
                                Year += 1900;
                            else
                                Year += 2000;
                            strDateTimeString = strDateTimeString.Substring(2);
                        }
                    else if (c.Equals('M'))
                    {
                        Month = int.Parse(strDateTimeString.Substring(0, 2));
                        strDateTimeString = strDateTimeString.Substring(2);
                    }
                    else if (c.Equals('D'))
                    {
                        Day = int.Parse(strDateTimeString.Substring(0, 2));
                        strDateTimeString = strDateTimeString.Substring(2);
                    }
                    if (!string.IsNullOrEmpty(DateSeparator))
                    {
                        if (strDateTimeString.StartsWith(DateSeparator))
                            strDateTimeString = strDateTimeString.Substring(1);
                        else
                        {
                            int pos = strDateTimeString.IndexOf(DateSeparator);
                            if (pos >= 0)
                            {
                                strDateTimeString = strDateTimeString.Substring(pos + 1);
                            }
                        }
                    }
                }

                int Hour = 0;
                int Minute = 0;
                int Second = 0;

                strDateTimeString = strDateTimeString.Trim();

                foreach (char c in TimeSequence.ToCharArray())
                {
                    if (string.IsNullOrEmpty(strDateTimeString))
                        break;
                    if (c.Equals('H'))
                    {
                        Hour = int.Parse(strDateTimeString.Substring(0, 2));
                        strDateTimeString = strDateTimeString.Substring(2);
                    }
                    else if (c.Equals('m'))
                    {
                        Minute = int.Parse(strDateTimeString.Substring(0, 2));
                        strDateTimeString = strDateTimeString.Substring(2);
                    }
                    else if (c.Equals('s'))
                    {
                        Second = int.Parse(strDateTimeString.Substring(0, 2));
                        strDateTimeString = strDateTimeString.Substring(2);
                    }
                    if (!string.IsNullOrEmpty(TimeSeparator))
                    {
                        if (strDateTimeString.StartsWith(TimeSeparator))
                            strDateTimeString = strDateTimeString.Substring(1);
                        else
                        {
                            int pos = strDateTimeString.IndexOf(TimeSeparator);
                            if (pos >= 0)
                            {
                                strDateTimeString = strDateTimeString.Substring(pos + 1);
                            }
                        }
                    }
                }
                return new DateTime(Year, Month, Day, Hour, Minute, Second);
            }
            catch
            {
                throw new Exception("Invalid Date/Time Format:" + DateTimeString);

            }
        }


        public override DataTable UploadToTempDatabase(string Filename, int UserID, string ZipPassword)
        {
            ClearTempTable();
            DataTable rawDataTable;
            if (Filename.EndsWith(".xls"))
            {
                rawDataTable = HROne.Import.ExcelImport.parse(Filename, ZipPassword).Tables[0];

                // Force override setting for excel format
                DateSeparator = "-";
                TimeSeparator = ":";
                DateSequence = "YMD";
                TimeSequence = "Hms";
                YearFormat = "YYYY";
            }
            else
            {
                System.IO.FileStream reader = System.IO.File.OpenRead(Filename);
                rawDataTable = CSVReader.parse(reader, UploadFileHasHeader, ColumnDelimiter, "\"");
                reader.Close();
            }
            if (rawDataTable!=null)
                foreach (DataRow row in rawDataTable.Rows)
                {
                    EUploadTimeCardRecord uploadTimeCardRecord = new EUploadTimeCardRecord();
                    EUploadTimeCardRecord uploadTimeCardRecord2 = null;
                    if (rawDataTable.Columns.Contains("SourceString"))
                        uploadTimeCardRecord.TimeCardRecordOriginalData = row["SourceString"].ToString();
                    else
                    {
                        foreach (object value in row.ItemArray)
                        {
                            string stringValue = string.Empty;

                            if (value != null)
                                stringValue = value.ToString();
                            if (string.IsNullOrEmpty(uploadTimeCardRecord.TimeCardRecordOriginalData))
                                uploadTimeCardRecord.TimeCardRecordOriginalData = stringValue;
                            else
                                uploadTimeCardRecord.TimeCardRecordOriginalData += "," + stringValue;
                        }
                    }
                    if (rawDataTable.Columns.Count >= TimeCardNumColumnIndex)
                        uploadTimeCardRecord.TimeCardRecordCardNo = row[TimeCardNumColumnIndex - 1].ToString().Trim();
                    else
                        errors.addError("Time Card column not found", null);

                    if (rawDataTable.Columns.Count >= DateColumnIndex && rawDataTable.Columns.Count >= TimeColumnIndex)
                    {
                        try
                        {
                            uploadTimeCardRecord.TimeCardRecordDateTime = getDateTime(row, DateColumnIndex, TimeColumnIndex);
                        }
                        catch (Exception)
                        {
                            //  add empty datetime to skip import
                            uploadTimeCardRecord.TimeCardRecordDateTime = new DateTime();
                            //errors.addError(ex.Message, null);
                        }
                    }
                    else
                        errors.addError("Date/Time column not found", null);
                    if (DateColumnIndex2 > 0 && TimeColumnIndex2 > 0)
                        if (rawDataTable.Columns.Count >= DateColumnIndex2 && rawDataTable.Columns.Count >= TimeColumnIndex2)
                        {
                            uploadTimeCardRecord2 = new EUploadTimeCardRecord();
                            try
                            {
                                uploadTimeCardRecord2.TimeCardRecordDateTime = getDateTime(row, DateColumnIndex2, TimeColumnIndex2);
                            }
                            catch (Exception)
                            {
                                //  add empty datetime to skip import
                                uploadTimeCardRecord2.TimeCardRecordDateTime = new DateTime();
                                //errors.addError(ex.Message, null);
                            }
                        }
                        else
                            errors.addError("2nd Date/Time column not found", null);

                    if (rawDataTable.Columns.Count >= LocationColumnIndex)
                        uploadTimeCardRecord.TimeCardRecordLocation = ETimeCardLocationMap.ConvertToNewLocationCode(dbConn, row[LocationColumnIndex - 1].ToString());
                    else
                        errors.addError("Location column not found", null);
                    uploadTimeCardRecord.TimeCardRecordInOutIndex = ETimeCardRecord.TimeCardRecordInOutIndexEnum.Unspecify;  //unspecify
                    uploadTimeCardRecord.EmpID = Import.Parse.GetEmpIDFromCardNo(uploadTimeCardRecord.TimeCardRecordCardNo);

                    if (errors.List.Count > 0)
                    {
                        throw new HRImportException(errors.Message());
                    }

                    if (uploadTimeCardRecord.TimeCardRecordOriginalData.Length <= 450)
                    {
                        //  Compare previous import record only when length <=450
                        DBFilter timeCardRecordFilter = new DBFilter();
                        timeCardRecordFilter.add(new Match("TimeCardRecordOriginalData", uploadTimeCardRecord.TimeCardRecordOriginalData));
                        ArrayList list = ETimeCardRecord.db.select(dbConn, timeCardRecordFilter);
                        if (list.Count > 0)
                        {
                            uploadTimeCardRecord.TimeCardRecordID = ((ETimeCardRecord)list[0]).TimeCardRecordID;
                            uploadTimeCardRecord.ImportActionStatus = ImportDBObject.ImportActionEnum.UPDATE;
                        }
                        else
                            uploadTimeCardRecord.ImportActionStatus = ImportDBObject.ImportActionEnum.INSERT;
                    }
                    else
                    {
                        //  Up to 450 character is stored for original data
                        uploadTimeCardRecord.TimeCardRecordOriginalData = uploadTimeCardRecord.TimeCardRecordOriginalData.Substring(0, 450);
                        uploadTimeCardRecord.ImportActionStatus = ImportDBObject.ImportActionEnum.INSERT;
                    }

                    uploadTimeCardRecord.SessionID = m_SessionID;
                    uploadTimeCardRecord.TransactionDate = AppUtils.ServerDateTime();

                    if (!uploadTimeCardRecord.TimeCardRecordDateTime.Ticks.Equals(0))
                        EUploadTimeCardRecord.db.insert(dbConn, uploadTimeCardRecord);

                    if (uploadTimeCardRecord2 != null)
                    {
                        uploadTimeCardRecord2.EmpID = uploadTimeCardRecord.EmpID;
                        uploadTimeCardRecord2.TimeCardRecordCardNo = uploadTimeCardRecord.TimeCardRecordCardNo;
                        uploadTimeCardRecord2.TimeCardRecordInOutIndex = uploadTimeCardRecord.TimeCardRecordInOutIndex;
                        uploadTimeCardRecord2.TimeCardRecordLocation = uploadTimeCardRecord.TimeCardRecordLocation;
                        uploadTimeCardRecord2.TimeCardRecordOriginalData = uploadTimeCardRecord.TimeCardRecordOriginalData + ":idx2";
                        uploadTimeCardRecord2.SessionID = uploadTimeCardRecord.SessionID;
                        uploadTimeCardRecord2.TransactionDate = uploadTimeCardRecord.TransactionDate;

                        if (uploadTimeCardRecord2.TimeCardRecordOriginalData.Length <= 450)
                        {
                            //  Compare previous import record only when length <=450
                            DBFilter timeCardRecordFilter = new DBFilter();
                            timeCardRecordFilter.add(new Match("TimeCardRecordOriginalData", uploadTimeCardRecord2.TimeCardRecordOriginalData));
                            ArrayList list = ETimeCardRecord.db.select(dbConn, timeCardRecordFilter);
                            if (list.Count > 0)
                            {
                                uploadTimeCardRecord2.TimeCardRecordID = ((ETimeCardRecord)list[0]).TimeCardRecordID;
                                uploadTimeCardRecord2.ImportActionStatus = ImportDBObject.ImportActionEnum.UPDATE;
                            }
                            else
                                uploadTimeCardRecord2.ImportActionStatus = ImportDBObject.ImportActionEnum.INSERT;
                        }
                        else
                        {
                            //  Up to 450 character is stored for original data
                            uploadTimeCardRecord2.TimeCardRecordOriginalData = uploadTimeCardRecord.TimeCardRecordOriginalData.Substring(0, 450);
                            uploadTimeCardRecord2.ImportActionStatus = ImportDBObject.ImportActionEnum.INSERT;
                        }
                        if (!uploadTimeCardRecord2.TimeCardRecordDateTime.Ticks.Equals(0))
                            EUploadTimeCardRecord.db.insert(dbConn, uploadTimeCardRecord2);
                    }
                }
            return GetImportDataFromTempDatabase(null);
        }

        protected DateTime getDateTime(DataRow row, int DateColumnIndex, int TimeColumnIndex)
        {
            if (DateColumnIndex == TimeColumnIndex)
            {
                DateTime dateTime = new DateTime();
                if (row[DateColumnIndex - 1] is DateTime)
                    dateTime = ((DateTime)row[DateColumnIndex - 1]);
                else if (!row[DateColumnIndex - 1].ToString().Trim().Equals(string.Empty))
                    dateTime = ConvertDateTimeWithFormatSetting(row[DateColumnIndex - 1].ToString().Trim());
                else
                    return new DateTime();
                return dateTime;
            }
            else
            {

                DateTime date = new DateTime();
                if (row[DateColumnIndex - 1] is DateTime)
                    date = ((DateTime)row[DateColumnIndex - 1]).Date;
                else if (!row[DateColumnIndex - 1].ToString().Trim().Equals(string.Empty))
                    date = ConvertDateWithFormatSetting(row[DateColumnIndex - 1].ToString().Trim());
                else
                    return new DateTime();

                TimeSpan time = new TimeSpan();
                if (row[TimeColumnIndex - 1] is DateTime)
                    time = ((DateTime)row[TimeColumnIndex - 1]).TimeOfDay;
                else if (!row[TimeColumnIndex - 1].ToString().Trim().Equals(string.Empty))
                    time = ConvertTimeWithFormatSetting(row[TimeColumnIndex - 1].ToString().Trim());
                else
                    return new DateTime();
                return date.Add(time);
            }
        }

        public override DataTable GetImportDataFromTempDatabase(ListInfo info)
        {
            return null;
        }

        public static void ClearTempTable(DatabaseConnection dbConn, string sessionID)
        {
            ImportTimeCardRecordProcess import = new ImportTimeCardRecordProcess(dbConn, sessionID);
            import.ClearTempTable();
        }

        public override void ClearTempTable()
        {
            //  Clear Old Import Session
            DBFilter sessionFilter = new DBFilter();
            if (!string.IsNullOrEmpty(m_SessionID))
                sessionFilter.add(new Match("SessionID", m_SessionID));
            EUploadTimeCardRecord.db.delete(dbConn, sessionFilter);
        }

        public override void ImportToDatabase()
        {
            DBFilter sessionFilter = new DBFilter();
            sessionFilter.add(new Match("SessionID", m_SessionID));
            sessionFilter.add("EmpID", true);

            ArrayList uploadTimeCardRecordList = EUploadTimeCardRecord.db.select(dbConn, sessionFilter);
            foreach (EUploadTimeCardRecord obj in uploadTimeCardRecordList)
            {

                ETimeCardRecord timeCardRecord = new ETimeCardRecord();
                timeCardRecord.TimeCardRecordID = obj.TimeCardRecordID;
                if (!ETimeCardRecord.db.select(dbConn, timeCardRecord))
                    timeCardRecord.TimeCardRecordID = 0;
                   
                timeCardRecord.EmpID = obj.EmpID;
                timeCardRecord.TimeCardRecordOriginalData = obj.TimeCardRecordOriginalData;
                timeCardRecord.TimeCardRecordCardNo = obj.TimeCardRecordCardNo;
                timeCardRecord.TimeCardRecordDateTime = obj.TimeCardRecordDateTime;
                timeCardRecord.TimeCardRecordLocation = obj.TimeCardRecordLocation;
                timeCardRecord.TimeCardRecordInOutIndex = obj.TimeCardRecordInOutIndex;
                if (obj.ImportActionStatus.Equals(ImportDBObject.ImportActionEnum.INSERT))
                    ETimeCardRecord.db.insert(dbConn, timeCardRecord);
                else
                    ETimeCardRecord.db.update(dbConn, timeCardRecord);
                EUploadTimeCardRecord.db.delete(dbConn, obj);

            }
        }
    }
}