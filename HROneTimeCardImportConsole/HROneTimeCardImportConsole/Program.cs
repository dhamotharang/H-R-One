using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using HROne.Import;
using HROne.Lib.Entities;
using HROne.DataAccess;

namespace HROneImportConsole
{
    class Program
    {
        public const string PARAM_CODE_TIMECARD_COLUMN_DELIMITER = "COLUMN_DELIMITER";

        public const string PARAM_CODE_TIMECARD_DATE_SEQUENCE = "TIMECARD_DATE_SEQUENCE";
        public const string PARAM_CODE_TIMECARD_DATE_YEAR_FORMAT = "TIMECARD_DATE_YEAR_FORMAT";
        public const string PARAM_CODE_TIMECARD_DATE_SEPARATOR = "TIMECARD_DATE_SEPARATOR";
        public const string PARAM_CODE_TIMECARD_TIME_SEPARATOR = "TIMECARD_TIME_SEPARATOR";

        public const string PARAM_CODE_TIMECARD_DATE_COLUMNINDEX = "TIMECARD_DATE_COLUMNINDEX";
        public const string PARAM_CODE_TIMECARD_TIME_COLUMNINDEX = "TIMECARD_TIME_COLUMNINDEX";
        public const string PARAM_CODE_TIMECARD_DATE_COLUMNINDEX_2 = "TIMECARD_DATE_COLUMNINDEX_2";
        public const string PARAM_CODE_TIMECARD_TIME_COLUMNINDEX_2 = "TIMECARD_TIME_COLUMNINDEX_2";
        public const string PARAM_CODE_TIMECARD_LOCATION_COLUMNINDEX = "TIMECARD_LOCATION_COLUMNINDEX";
        public const string PARAM_CODE_TIMECARD_TIMECARDNUM_COLUMNINDEX = "TIMECARD_TIMECARDNUM_COLUMNINDEX";

        public const string PARAM_CODE_TIMECARD_FILE_HAS_HEADER = "TIMECARD_FILE_HAS_HEADER";


        public static string logFilePath = string.Empty;

        static void Main(string[] args)
        {
            const string TIMECARD_IMPORT_FUNCTION_CODE = "ATT006";
            //string sourceFile = string.Empty;
            string zipPassword = string.Empty;
            if (args.GetLength(0) > 2)
            {
                zipPassword = args[2];
            }
            if (args.GetLength(0) > 1)
            {
                logFilePath = args[1];
                if (!System.IO.Directory.Exists(logFilePath))
                {
                    logFilePath = string.Empty;
                    WriteErrorLog("Log Path does not exists");
                    WriteErrorLog("Parameter: " + string.Join(" ", args));

                    return;
                }
                else
                {
                    logFilePath = ((string)(new System.IO.DirectoryInfo(logFilePath).FullName + System.IO.Path.DirectorySeparatorChar.ToString())).Replace(System.IO.Path.DirectorySeparatorChar.ToString() + System.IO.Path.DirectorySeparatorChar.ToString(), System.IO.Path.DirectorySeparatorChar.ToString());
                }
            }
            string[] fileNameList=null;
            if (args.GetLength(0) > 0)
            {
                string filePath = args[0];
                string fileName = System.IO.Path.GetFileName(filePath);
                string folderName = System.IO.Path.GetDirectoryName(filePath);
                fileNameList = System.IO.Directory.GetFiles(folderName, fileName, System.IO.SearchOption.TopDirectoryOnly);
            }

            if (fileNameList == null)
            {
                WriteErrorLog("Import File not found");
                WriteErrorLog("Parameter: " + string.Join(" ", args));
                return;
            }



            HROneConfig HROneConfig = new HROneConfig(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\HROne.config"));
            DatabaseConnection dbConn = HROneConfig.GetDatabaseConnection();
            if (dbConn != null)
            {
                ImportTimeCardRecordProcess timeCardRecordImport = new ImportTimeCardRecordProcess(dbConn, "IMPORT_" + AppUtils.ServerDateTime().ToString("yyyyMMddHHmmssSSS"));
                timeCardRecordImport.DateSequence = ESystemParameter.getParameter(dbConn, PARAM_CODE_TIMECARD_DATE_SEQUENCE);
                timeCardRecordImport.DateSeparator = ESystemParameter.getParameter(dbConn, PARAM_CODE_TIMECARD_DATE_SEPARATOR);
                timeCardRecordImport.YearFormat = ESystemParameter.getParameter(dbConn, PARAM_CODE_TIMECARD_DATE_YEAR_FORMAT);
                timeCardRecordImport.TimeSeparator = ESystemParameter.getParameter(dbConn, PARAM_CODE_TIMECARD_TIME_SEPARATOR);

                timeCardRecordImport.DateColumnIndex = int.Parse(ESystemParameter.getParameter(dbConn, PARAM_CODE_TIMECARD_DATE_COLUMNINDEX));
                timeCardRecordImport.TimeColumnIndex = int.Parse(ESystemParameter.getParameter(dbConn, PARAM_CODE_TIMECARD_TIME_COLUMNINDEX));
                timeCardRecordImport.DateColumnIndex2 = int.Parse(ESystemParameter.getParameter(dbConn, PARAM_CODE_TIMECARD_DATE_COLUMNINDEX_2));
                timeCardRecordImport.TimeColumnIndex2 = int.Parse(ESystemParameter.getParameter(dbConn, PARAM_CODE_TIMECARD_TIME_COLUMNINDEX_2));
                timeCardRecordImport.LocationColumnIndex = int.Parse(ESystemParameter.getParameter(dbConn, PARAM_CODE_TIMECARD_LOCATION_COLUMNINDEX));
                timeCardRecordImport.TimeCardNumColumnIndex = int.Parse(ESystemParameter.getParameter(dbConn, PARAM_CODE_TIMECARD_TIMECARDNUM_COLUMNINDEX));

                timeCardRecordImport.ColumnDelimiter = ESystemParameter.getParameter(dbConn, PARAM_CODE_TIMECARD_COLUMN_DELIMITER);

                timeCardRecordImport.UploadFileHasHeader = ESystemParameter.getParameter(dbConn, PARAM_CODE_TIMECARD_FILE_HAS_HEADER).Equals("Y");
                //DataTable dataTable = HROne.Import.ExcelImport.parse(strTmpFile);
                //using (OleDbConnection conn = new OleDbConnection(@"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=C:\csv\;Extended Properties='Text;'"))
                HROne.Common.AuditTrail.StartFunction(dbConn, new HROne.Lib.Entities.EUser(), TIMECARD_IMPORT_FUNCTION_CODE, false);
                try
                {
                    foreach (string sourceFile in fileNameList)
                    {
                        DataTable timeCardRecordTable = timeCardRecordImport.UploadToTempDatabase(sourceFile, 0, zipPassword);
                        timeCardRecordImport.ImportToDatabase();
                        timeCardRecordImport.ClearTempTable();
                        WriteProgramLog(sourceFile + " has been imported successfully");
                        //WriteProgramLog(timeCardRecordTable.Rows.Count + " time card record(s) imported");
                        //WriteProgramLog("0 time card record(s) imported");
                    }
                }
                catch (HRImportException ex)
                {
                    if (timeCardRecordImport.errors.List.Count > 0)
                        foreach (string errorString in timeCardRecordImport.errors.List)
                            WriteErrorLog(errorString);
                    else
                        WriteErrorLog(ex.Message);
                }
                catch (Exception ex)
                {
                    WriteErrorLog(ex.Message);
                    //return;
                }
                HROne.Common.AuditTrail.EndFunction(dbConn);
            }
            else
            {
                WriteErrorLog("no connection");
            }
        }

        public static void WriteLine(string message)
        {
            Console.WriteLine(AppUtils.ServerDateTime().ToString("yyyy-MM-dd HH:mm:ss") + "\t" + message);
        }

        public static void WriteProgramLog(string message)
        {
            const string logFile = "Import.log";
            WriteLine(message);
            System.IO.StreamWriter writer = System.IO.File.AppendText(logFilePath + logFile);
            writer.WriteLine(AppUtils.ServerDateTime().ToString("yyyy-MM-dd HH:mm:ss") + "\t" + message);
            writer.Close();

        }
        public static void WriteErrorLog(string message)
        {
            const string logFile = "Error.log";
            WriteLine(message);
            System.IO.StreamWriter writer = System.IO.File.AppendText(logFilePath + logFile);
            writer.WriteLine(AppUtils.ServerDateTime().ToString("yyyy-MM-dd HH:mm:ss") + "\t" + message);
            writer.Close();

        }
    }


}
