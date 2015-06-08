using System;
using System.Collections;
using System.Data;
//using System.Data.OleDb;
using System.Text;
using HROne.Lib.Entities;
using HROne.DataAccess;
////using perspectivemind.validation;

namespace HROne.Import
{
    /// <summary>
    /// Summary description for CSVImport
    /// </summary>
    public static class CSVReader
    {
        public static DataTable parse(System.IO.Stream InputStream, bool HasHeader, string DelimiterCharacter, string QuoteCharacter)
        {
            System.IO.StreamReader textReader = new System.IO.StreamReader(InputStream);
            string line = string.Empty;
            DataTable dataTable = new DataTable();

            string sourceString=string.Empty ;
            if (HasHeader)
            {
                line = textReader.ReadLine();
                dataTable = readDataTableHeader(line, DelimiterCharacter, QuoteCharacter, textReader);
                line = string.Empty;
            }
            while (!textReader.EndOfStream)
            {
                string remainString;
                //line += textReader.ReadLine();
                if (string.IsNullOrEmpty(sourceString))
                    sourceString = textReader.ReadLine();
                else
                    sourceString += "\r\n" + textReader.ReadLine();
                ArrayList dataArray = null;

                dataArray = CSVToStringArray(sourceString, DelimiterCharacter, QuoteCharacter, out remainString);
                //line = string.Empty;

                if (string.IsNullOrEmpty(remainString.Trim()))
                {
                    DataRow row = dataTable.NewRow();
                    for (int i = 0; i < dataArray.Count; i++)
                    {
                        if (i >= dataTable.Columns.Count)
                            dataTable.Columns.Add("Column" + (i + 1));
                        row[i] = dataArray[i].ToString();

                    }
                    if (!dataTable.Columns.Contains("SourceString"))
                        dataTable.Columns.Add("SourceString");

                    row["SourceString"] = sourceString;
                    dataTable.Rows.Add(row);
                    sourceString = string.Empty;
                }
            }
            textReader.Close();
            return dataTable;
        }

        private static DataTable readDataTableHeader(string headerLine, string DelimiterCharacter, string QuoteCharacter, System.IO.StreamReader textReader)
        {
            headerLine = headerLine.Replace(".", "#");
            string dummyString;
            DataTable dataTable = new DataTable();
            ArrayList headerArray = CSVToStringArray(headerLine, DelimiterCharacter, QuoteCharacter, out dummyString);
            for (int i = 0; i < headerArray.Count; i++)
            {
                dataTable.Columns.Add(headerArray[i].ToString());
            }
            return dataTable;

        }

        private static ArrayList CSVToStringArray(string recordline, string DelimiterCharacter, string QuoteCharacter, out string RemainString)
        {
            RemainString = string.Empty;
            ArrayList result = new ArrayList();

            string tmpRecordLine = recordline;
            string tmpResult = string.Empty;
            string[] records = new string[0];
            int nextPos = 0;
            while (tmpRecordLine.IndexOf(DelimiterCharacter, nextPos) >= 0)
            {
                int intPos = tmpRecordLine.IndexOf(DelimiterCharacter, nextPos);
                string firstPart = tmpRecordLine.Substring(0, intPos);
                string lastPart = tmpRecordLine.Substring(intPos + 1);
                if (firstPart.Trim().StartsWith(QuoteCharacter))
                    if (firstPart.EndsWith(QuoteCharacter))
                    {
                        firstPart = firstPart.Substring(1, firstPart.Length - 2);
                        //if (tmpResult == string.Empty)
                        //    tmpResult = firstPart;
                        //else
                        //    tmpResult += FIELD_DELIMITER + firstPart;
                        result.Add(firstPart);
                        tmpRecordLine = lastPart;
                        nextPos = 0;
                    }
                    else
                        nextPos = intPos + 1;
                else
                {
                    //if (tmpResult == string.Empty)
                    //    tmpResult = firstPart;
                    //else
                    //    tmpResult += FIELD_DELIMITER + firstPart;
                    result.Add(firstPart);
                    tmpRecordLine = lastPart;
                    nextPos = 0;
                }

            }
            if (tmpRecordLine.Trim().StartsWith(QuoteCharacter))
                if (tmpRecordLine.EndsWith(QuoteCharacter))
                {
                    tmpRecordLine = tmpRecordLine.Substring(1, tmpRecordLine.Length - 2);
                    //if (tmpResult == string.Empty)
                    //    tmpResult = tmpRecordLine;
                    //else
                    //    tmpResult += FIELD_DELIMITER + tmpRecordLine;
                    result.Add(tmpRecordLine);
                }
                else
                {
                    RemainString = tmpRecordLine;
                    //throw new Exception("test");
                }
            else
            {
                //if (tmpResult == string.Empty)
                //    tmpResult = tmpRecordLine;
                //else
                //    tmpResult += FIELD_DELIMITER + tmpRecordLine;
                result.Add(tmpRecordLine);
            }
            //return tmpResult.Split(new string[] { FIELD_DELIMITER });
            return result;
        }
    }

    public static class ExcelImport
    {
        public static DataSet parse(string ExcelFilePath, string ZipPassword)
        {
            return parse(ExcelFilePath, ZipPassword, string.Empty);
        }
        public static DataSet parse(string ExcelFilePath, string ZipPassword, string FirstColumnName)
        {

            //OleDbConnection cnnxls = null;
            DataSet ds = new DataSet();
            if (System.IO.Path.GetExtension(ExcelFilePath).Equals(".zip", StringComparison.CurrentCultureIgnoreCase))
            {
                string strTmpFolder = ExcelFilePath + ".dir";

                try
                {
                    zip.ExtractAll(ExcelFilePath, strTmpFolder, ZipPassword);
                    System.IO.DirectoryInfo rootDir = new System.IO.DirectoryInfo(strTmpFolder);
                    foreach (System.IO.FileInfo fileInfo in rootDir.GetFiles("*", System.IO.SearchOption.AllDirectories))
                        ds.Merge(parse(fileInfo.FullName, ZipPassword, FirstColumnName));
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    System.IO.Directory.Delete(strTmpFolder, true);
                }
                //string strConn = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + strTmpFolder + ";Extended Properties=\"text;HDR=YES;IMEX=1;FMT=Delimited;\"";
                //cnnxls = new OleDbConnection(strConn);
                //cnnxls.Open();
            }
            else if (System.IO.Path.GetExtension(ExcelFilePath).Equals(".csv", StringComparison.CurrentCultureIgnoreCase))
            {
                System.IO.FileInfo fileInfo = new System.IO.FileInfo(ExcelFilePath);
                DataTable table = CSVReader.parse(fileInfo.OpenRead(), true, ",", "\"");
                table.TableName = System.IO.Path.GetFileNameWithoutExtension(fileInfo.FullName);
                ds.Tables.Add(table);
            }
            else
            {
                NPOI.HSSF.UserModel.HSSFWorkbook workBook = new NPOI.HSSF.UserModel.HSSFWorkbook(new System.IO.FileStream(ExcelFilePath, System.IO.FileMode.Open)); // ExcelLibrary.SpreadSheet.Workbook.Load(Filename);

                for (int sheetIndex = 0; sheetIndex < workBook.NumberOfSheets; sheetIndex++)
                {
                    if (!workBook.IsSheetHidden(sheetIndex))
                    {
                        int intHeaderRow = 0;
                        NPOI.HSSF.UserModel.HSSFSheet workSheet = (NPOI.HSSF.UserModel.HSSFSheet)workBook.GetSheetAt(sheetIndex);
                        NPOI.HSSF.UserModel.HSSFRow headerRow = null;  //= (NPOI.HSSF.UserModel.HSSFRow)workSheet.GetRow(intHeaderRow);

                        if (!string.IsNullOrEmpty(FirstColumnName))
                        {
                            for (int tmpRowIdx = intHeaderRow; tmpRowIdx <= workSheet.LastRowNum; tmpRowIdx++)
                            {
                                headerRow = (NPOI.HSSF.UserModel.HSSFRow)workSheet.GetRow(tmpRowIdx);
                                if (headerRow == null)
                                    continue;
                                bool columnNameMatch = false;
                                for (int tmpColumnIndex = 0; tmpColumnIndex <= headerRow.LastCellNum; tmpColumnIndex++)
                                {
                                    if (headerRow.GetCell(tmpColumnIndex) != null)
                                    {
                                        string columnName = headerRow.GetCell(tmpColumnIndex).ToString().Trim();
                                        if (FirstColumnName.Equals(columnName))
                                        {
                                            intHeaderRow = tmpRowIdx;
                                            columnNameMatch = true;
                                            break;
                                        }
                                    }
                                }
                                if (columnNameMatch)
                                    break;

                            }
                        }
                        else
                            headerRow = (NPOI.HSSF.UserModel.HSSFRow)workSheet.GetRow(intHeaderRow);

                        if (headerRow == null)
                            continue;
                        string tableName = workSheet.SheetName.Trim();
                        DataTable table = new DataTable(tableName);
                        int intColumnIndex = 0;
                        while (intColumnIndex <= headerRow.LastCellNum)
                        {
                            if (headerRow.GetCell(intColumnIndex) != null)
                            {
                                string columnName = headerRow.GetCell(intColumnIndex).ToString().Trim();
                                if (string.IsNullOrEmpty(columnName))
                                    columnName = "Column_" + intColumnIndex;
                                if (table.Columns.Contains(columnName))
                                    columnName = "Column_" + intColumnIndex;
                                table.Columns.Add(columnName, typeof(string));

                                //  resign new value of column name to Excel for below part of import 
                                headerRow.GetCell(intColumnIndex).SetCellValue(columnName);
                            }
                            intColumnIndex++;
                        }
                        int rowCount = 1;

                        while (intHeaderRow + rowCount <= workSheet.LastRowNum)
                        {
                            int colCount = 0;

                            NPOI.HSSF.UserModel.HSSFRow row = (NPOI.HSSF.UserModel.HSSFRow)workSheet.GetRow(intHeaderRow + rowCount);
                            if (row == null)
                            {
                                rowCount++;
                                continue;
                            }

                            DataRow dataRow = table.NewRow();

                            while (colCount <= headerRow.LastCellNum)
                            {
                                if (headerRow.GetCell(colCount) != null)
                                {
                                    string columnName = headerRow.GetCell(colCount).ToString();
                                    if (table.Columns.Contains(columnName))
                                    {
                                        NPOI.HSSF.UserModel.HSSFCell cell = (NPOI.HSSF.UserModel.HSSFCell)row.GetCell(colCount);
                                        if (cell != null)
                                        {
                                            if (cell.CellType.Equals(NPOI.SS.UserModel.CellType.FORMULA))
                                            {
                                                NPOI.HSSF.UserModel.HSSFFormulaEvaluator e = new NPOI.HSSF.UserModel.HSSFFormulaEvaluator(workBook);
                                                cell = (NPOI.HSSF.UserModel.HSSFCell)e.EvaluateInCell(cell);
                                            }
                                            string fieldValue = cell.ToString();
                                            if (cell.CellType.Equals(NPOI.SS.UserModel.CellType.NUMERIC))
                                            {
                                                string format = string.Empty;
                                                //bool IsBuildinformat = false;
                                                //  Not sure whether workBook.CreateDataFormat().GetFormat(index) can obtain all the build-in format
                                                try
                                                {
                                                    format = NPOI.HSSF.UserModel.HSSFDataFormat.GetBuiltinFormat(cell.CellStyle.DataFormat);
                                                    //IsBuildinformat = true;
                                                }
                                                catch
                                                {
                                                    format = workBook.CreateDataFormat().GetFormat(cell.CellStyle.DataFormat);
                                                }

                                                //  [h]:mm:ss handle NOT support
                                                int midBlanketStartPos = format.IndexOf('[');
                                                while (midBlanketStartPos >= 0)
                                                {
                                                    int midBlanketEndPos = format.IndexOf(']', midBlanketStartPos);
                                                    format = format.Substring(0, midBlanketStartPos) + format.Substring(midBlanketStartPos + 1, midBlanketEndPos - midBlanketStartPos - 1) + format.Substring(midBlanketEndPos + 1);
                                                    midBlanketStartPos = format.IndexOf('[');
                                                }

                                                if (format.IndexOf("y", StringComparison.CurrentCultureIgnoreCase) >= 0 || format.IndexOf("d", StringComparison.CurrentCultureIgnoreCase) >= 0)
                                                    if (format.IndexOf("h", StringComparison.CurrentCultureIgnoreCase) >= 0)
                                                        fieldValue = cell.DateCellValue.ToString("yyyy-MM-dd HH:mm:ss");
                                                    else
                                                    {
                                                        DateTime date = cell.DateCellValue;
                                                        if (date.TimeOfDay.TotalSeconds > 0)
                                                            fieldValue = date.ToString("yyyy-MM-dd HH:mm:ss");
                                                        else
                                                            fieldValue = date.ToString("yyyy-MM-dd");
                                                    }
                                                else if (format.IndexOf("h", StringComparison.CurrentCultureIgnoreCase) >= 0)
                                                {
                                                    DateTime date = cell.DateCellValue;

                                                    //  default date of "Time Only" field is 1899-12-31
                                                    if (!date.Date.Ticks.Equals(new DateTime(1899, 12, 31).Ticks))
                                                        fieldValue = cell.DateCellValue.ToString("yyyy-MM-dd HH:mm:ss");
                                                    else
                                                        fieldValue = cell.DateCellValue.ToString("HH:mm:ss");
                                                }
                                                else
                                                    fieldValue = cell.NumericCellValue.ToString();
                                            }
                                            dataRow[columnName] = fieldValue;
                                        }
                                    }
                                }
 

                                colCount++;
                            }
                            table.Rows.Add(dataRow);
                            rowCount++;
                        }
                        ds.Tables.Add(table);
                    }
                }

                //string strConn = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + ExcelFilePath + ";Extended Properties=\"Excel 12.0 Xml;IMEX=1;HDR=YES;\"";
                //cnnxls = new OleDbConnection(strConn);
                //try
                //{
                //    cnnxls.Open();
                //}
                //catch
                //{
                //    cnnxls.ConnectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + ExcelFilePath + ";Extended Properties=\"Excel 8.0;IMEX=1;HDR=YES;\"";
                //    cnnxls.Open();
                //}


                //DataTable schemaTable = cnnxls.GetSchema("Tables");


                //foreach (DataRow schemaRow in schemaTable.Rows)
                //{
                //    string tableName = schemaRow["Table_Name"].ToString().Trim();
                //    if (tableName.EndsWith("$"))
                //    {
                //        OleDbDataAdapter oda = new OleDbDataAdapter("select * from [" + tableName + "]", cnnxls);
                //        try
                //        {


                //            //DataTable[] tables = oda.FillSchema(ds, SchemaType.Mapped);// 
                //            //tables[0].TableName = schemaRow["Table_Name"].ToString().Replace("$", "").Replace("#csv", "");
                //            //if (tables[0].Columns.Contains("Emp No*"))
                //            //    tables[0].Columns["Emp No*"].DataType = typeof(string);
                //            //OleDbDataReader dr = oda.SelectCommand.ExecuteReader();

                //            //while (dr.Read())
                //            //{
                //            //    DataRow row = tables[0].NewRow();
                //            //    for (int i = 0; i < tables[0].Columns.Count; i++)
                //            //        row[i] = dr[i];
                //            //    tables[0].Rows.Add(row);
                //            //}
                //            ////                    oda.Fill(tables[0]);
                //            //if (ds.Tables.Contains(tableName) && tableName.ToString().EndsWith("$"))
                //            //    ds.Tables.Remove(tableName);
                //            string actualTableName = tableName.Substring(0, tableName.Length - 1);
                //            if (!ds.Tables.Contains(actualTableName))
                //                oda.Fill(ds, actualTableName);
                //        }
                //        catch
                //        {
                //            //  unknown error caused by hidden sheet
                //        }
                //        //                oda.Fill(ds);
                //    }
                //}

                //cnnxls.Close();
            }
            foreach (DataTable tempTable in ds.Tables)
                for (int rowIdx = tempTable.Rows.Count - 1; rowIdx >= 0; rowIdx--)
                {
                    DataRow row = tempTable.Rows[rowIdx];
                    bool isEmptyRow = true;
                    foreach (DataColumn tempColumn in tempTable.Columns)
                    {
                        if (!row.IsNull(tempColumn))
                            if (!string.IsNullOrEmpty(row[tempColumn].ToString().Trim()))
                            {
                                isEmptyRow = false;
                                break; 
                            }
                    }
                    if (isEmptyRow)
                        tempTable.Rows.Remove(row);
                    else
                        break;
                }
            foreach (DataTable tempTable in ds.Tables)
                foreach (DataColumn tempColumn in tempTable.Columns)
                {
                    string tempColumnName = tempColumn.ColumnName;
                    tempColumnName = tempColumnName.Trim().Replace("*", "");
                    tempColumnName = tempColumnName.Trim().Replace("#", "");
                    tempColumn.ColumnName = tempColumnName;
                }
            return ds;
        }
    }

    public abstract class ImportProcessInteface
    {
        //void SetMessageCultureInfo(CultureInfo ci);
        protected DatabaseConnection dbConn;
        protected string m_SessionID;
        //protected const string FIELD_INTERNAL_ID = "Internal ID";
        protected const string FIELD_SYNC_ID = "SynID";

        public ImportProcessInteface(DatabaseConnection dbConn, string SessionID)
        {
            this.dbConn = dbConn;
            this.m_SessionID = SessionID;
        }
        public abstract DataTable UploadToTempDatabase(string Filename, int UserID, string CSVPassword);
        public abstract DataTable GetImportDataFromTempDatabase(ListInfo info);
        public abstract void ClearTempTable();
        public abstract void ImportToDatabase();
        public static DBTerm getCreateModifiedRecordsAfterDBTerm(DateTime AfterReferenceDateTime)
        {
            OR orRecordDateTimeTerms = new OR();
            if (!AfterReferenceDateTime.Ticks.Equals(0))
            {
                orRecordDateTimeTerms.add(new Match("RecordCreatedDateTime", ">=", AfterReferenceDateTime));
                orRecordDateTimeTerms.add(new Match("RecordLastModifiedDateTime", ">=", AfterReferenceDateTime));
            }
            return orRecordDateTimeTerms;
        }
        //protected static string ToHexDecWithCheckDigit(int ID)
        //{
        //    int checkdigit = 15 - ID % 16;
        //    return "0x" + ID.ToString("X") + checkdigit.ToString("X");
        //}
        //protected static int FromHexDecWithCheckDigit(string HexDecWithCheckDigit)
        //{
        //    if (string.IsNullOrEmpty(HexDecWithCheckDigit))
        //        return 0;
        //    if (HexDecWithCheckDigit.StartsWith("0x"))
        //    {
        //        HexDecWithCheckDigit = HexDecWithCheckDigit.Substring(2);
        //        string HexDecString = HexDecWithCheckDigit.Substring(0, HexDecWithCheckDigit.Length - 1);
        //        string CheckDigitString = HexDecWithCheckDigit.Substring(HexDecWithCheckDigit.Length - 1);

        //        int checkDigit = 0;
        //        int id = 0;
        //        if (int.TryParse(CheckDigitString, System.Globalization.NumberStyles.AllowHexSpecifier, null, out checkDigit)
        //        && int.TryParse(HexDecString, System.Globalization.NumberStyles.AllowHexSpecifier, null, out id))
        //        {
        //            if (15 - id % 16 == checkDigit)
        //                return id;
        //        }
        //    }
        //    throw new Exception("Invalid Hexadecimal vallue: " + HexDecWithCheckDigit);
        //}
    }

    public abstract class ImportDBObject : DBObject
    {
        public const string FIELD_VALUE_OVERRIDE_EMPTY = "(empty)";

        public static void CopyObjectProperties(DBObject From, DBObject To)
        {
            if (From != null && To != null)
                foreach (System.Reflection.PropertyInfo propInfo in From.GetType().GetProperties())
                {
                    string propertyName = propInfo.Name;

                    System.Reflection.PropertyInfo destinationPropInfo = To.GetType().GetProperty(propertyName);
                    if (destinationPropInfo != null)
                    {
                        if (destinationPropInfo.CanWrite)
                        {
                            if (From is ImportDBObject)
                            {
                                if (!((ImportDBObject)From).isImportModified(propertyName))
                                    continue;
                            }
                            object value = propInfo.GetValue(From, null);
                            destinationPropInfo.SetValue(To, value, null);
                        }
                        else
                            continue;
                    }
                }
        }

        public void ExportToObject(BaseObject obj)
        {
            if (obj != null)
                CopyObjectProperties(this, obj);
            //foreach (System.Reflection.PropertyInfo propInfo in this.GetType().GetProperties())
            //{
            //    string propertyName = propInfo.Name;

            //    System.Reflection.PropertyInfo destinationPropInfo = obj.GetType().GetProperty(propertyName);
            //    if (destinationPropInfo != null)
            //    {

            //        object value = propInfo.GetValue(this, null);
            //        destinationPropInfo.SetValue(obj, value,null);
            //    }
            //}
        }

        public void ImportFromObject(BaseObject obj)
        {
            if (obj != null)
                CopyObjectProperties(obj, this);
            //foreach (System.Reflection.PropertyInfo propInfo in obj.GetType().GetProperties())
            //{
            //    string propertyName = propInfo.Name;

            //    System.Reflection.PropertyInfo destinationPropInfo = this.GetType().GetProperty(propertyName);
            //    if (destinationPropInfo != null)
            //    {

            //        object value = propInfo.GetValue(obj, null);
            //        destinationPropInfo.SetValue(this, value, null);
            //    }
            //}
        }
        public static WFValueList VLImportAction = new AppUtils.NewWFTextList(new string[] { "N", "U", "I" }, new string[] { "Update Terms", "Update", "Insert" });


        public enum ImportActionEnum
        {
            NONE = 0,
            UPDATE = 1,
            INSERT = 2,
            DELETE = 3
        }

        protected string m_ImportAction;
        [DBField("ImportAction"), TextSearch, Export(false), Required]
        public string ImportAction
        {
            get { return m_ImportAction; }
            set { m_ImportAction = value; base.modify("ImportAction"); }
        }

        public ImportActionEnum ImportActionStatus
        {
            get
            {
                if (m_ImportAction.Equals("U"))
                    return ImportActionEnum.UPDATE;
                else if (m_ImportAction.Equals("I"))
                    return ImportActionEnum.INSERT;
                else
                    return ImportActionEnum.NONE;
            }
            set
            {
                if (value == ImportActionEnum.UPDATE)
                    ImportAction = "U";
                else if (value == ImportActionEnum.INSERT)
                    ImportAction = "I";
                else
                    ImportAction = "N";
            }
        }


        protected string m_SessionID;
        [DBField("SessionID"), TextSearch, Export(false), Required]
        public string SessionID
        {
            get { return m_SessionID; }
            set { m_SessionID = value; base.modify("SessionID"); }
        }

        protected DateTime m_TransactionDate;
        [DBField("TransactionDate"), TextSearch, Export(false), Required]
        public DateTime TransactionDate
        {
            get { return m_TransactionDate; }
            set { m_TransactionDate = value; base.modify("TransactionDate"); }
        }

        protected ArrayList m_ModifiedFieldList= new ArrayList();
        [DBField("ModifiedFieldList"), TextSearch, Export(false), Required]
        public string ModifiedFieldList
        {
            get { return string.Join("|", (string[])m_ModifiedFieldList.ToArray(typeof(string))); }
            set { m_ModifiedFieldList = new ArrayList(value.Split(new char[] { '|' })); }
        }

        public bool isImportModified(string fieldName)
        {
            return m_ModifiedFieldList.Contains(fieldName.ToUpper());
        }

        protected override void modify(string fieldName)
        {
            base.modify(fieldName);
            m_ModifiedFieldList.Add(fieldName.ToUpper());
            base.modify("ModifiedFieldList");
        }
    }

    public class ImportErrorList
    {
        public ImportErrorList()
        {
        }
        public ArrayList List = new ArrayList();
        public void addError(string errMessage, string[] errorParameters)
        {
            if (errorParameters != null)
            {
                if (errorParameters.GetLength(0) > 0)
                {
                    List.Add(string.Format(errMessage, errorParameters));
                }
                else
                {
                    List.Add(errMessage);
                }
            }
            //{
            //    int parameterCount = errorParameters.GetLength(0);
            //    for (int i = errorParameters.GetUpperBound(0); i >= errorParameters.GetLowerBound(0); i--)
            //    {
            //        string parameterName = "%" + parameterCount.ToString().Trim();
            //        errMessage = errMessage.Replace(parameterName, errorParameters[i]);
            //        parameterCount--;
            //    }
            //}
            else
                List.Add(errMessage);
        }
        public string Message()
        {
            string message = string.Empty;
            foreach (string error in List)
                message += error + "\r\n";
            return message;

        }

    }


    public class HRImportException : Exception
    {
        public HRImportException(string Message)
            : base(Message)
        {
        }
    }

    public static class ImportErrorMessages
    {
        public static string ERROR_ACCESS_DENIED_EMP_NO = "Access Denied on Employee No: {0} . (line {1})";

        public static string ERROR_INVALID_COLUMN = "Column not found: {0} . (line {1})";
        public static string ERROR_INVALID_FIELD_VALUE = "Invalid field value({0}) in column \"{1}\", line {2})";
    }


    public static class ImportErrorMessage
    {
        public static string ERROR_INVALID_COLUMN
        {
            get { return HROne.Common.WebUtility.GetLocalizedStringByCode("ERROR_INVALID_COLUMN", "Column not found: {0} . ") + "(line {1})"; }
        }
        public static string ERROR_INVALID_EMP_NO
        {
            get { return HROne.Common.WebUtility.GetLocalizedStringByCode("ERROR_INVALID_EMP_NO", "Invalid Employee No: {0} . ") + "(line {1})"; }
        }
        public static string ERROR_ACCESS_DENIED_EMP_NO
        {
            get { return HROne.Common.WebUtility.GetLocalizedStringByCode("ERROR_ACCESS_DENIED_EMP_NO", "Access Denied on Employee No: {0} . ") + "(line {1})"; }
        }
        public static string ERROR_DUPLICATE_EMP_NO
        {
            get { return HROne.Common.WebUtility.GetLocalizedStringByCode("ERROR_DUPLICATE_EMP_NO", "Duplicate Employee No: {0} . ") + "(line {1})"; }
        }
        public static string ERROR_INVALID_GENDER
        {
            get { return HROne.Common.WebUtility.GetLocalizedStringByCode("ERROR_INVALID_GENDER", "Invalid Gender: {0} . ") + "(line {1})"; }
        }
        public static string ERROR_INVALID_MARITAL_STATUS
        {
            get { return HROne.Common.WebUtility.GetLocalizedStringByCode("ERROR_INVALID_MARITAL_STATUS", "Invalid Marital Status: {0} . ") + "(line {1})"; }
        }
        public static string ERROR_INVALID_AREA_CODE
        {
            get { return HROne.Common.WebUtility.GetLocalizedStringByCode("ERROR_INVALID_AREA_CODE", "Invalid Area Code: {0} . ") + "(line {1})"; }
        }
        public static string ERROR_INVALID_FIELD_VALUE
        {
            get { return HROne.Common.WebUtility.GetLocalizedStringByCode("ERROR_INVALID_FIELD_VALUE", "Invalid field value: {0} on Employee No: {1} . ") + "(line {2})"; }
        }
        public static string ERROR_DATE_FROM_OVERLAP
        {
            get { return HROne.Common.WebUtility.GetLocalizedStringByCode("ERROR_DATE_FROM_OVERLAP", "Date From {0} cannot be overlapped. ") + "(line {1})"; }
        }

        public static string ERROR_INVALID_AMOUNT
        {
            get { return HROne.Common.WebUtility.GetLocalizedStringByCode("ERROR_INVALID_AMOUNT", "Invalid Amount: {0} . ") + "(line {1})"; }
        }
        public static string ERROR_TOTAL_PERCENTAGE_NOT_100
        {
            get { return HROne.Common.WebUtility.GetLocalizedStringByCode("ERROR_TOTAL_PERCENTAGE_NOT_100", "Total percentage must be 100%") + "(line {0})"; }
        }

        public static string ERROR_INVALID_PAYMENT_CODE
        {
            get { return HROne.Common.WebUtility.GetLocalizedStringByCode("ERROR_INVALID_PAYMENT_CODE", "Invalid Payment Code: {0}") + "(line {1})"; }
        }
        public static string ERROR_INVALID_POINT
        {
            get { return HROne.Common.WebUtility.GetLocalizedStringByCode("ERROR_INVALID_POINT", "Invalid Payscale Point: {0}") + "(line {1})"; }
        }
        public static string ERROR_INVALID_ANNOUNCE_DATE
        {
            get { return HROne.Common.WebUtility.GetLocalizedStringByCode("ERROR_INVALID_ANNOUNCE_DATE", "Invalid Announce Date: {0}") + "(line {1})"; }
        }
        public static string ERROR_INVALID_EFFECTIVE_DATE
        {
            get { return HROne.Common.WebUtility.GetLocalizedStringByCode("ERROR_INVALID_EFFECTIVE_DATE", "Invalid Effective Date: {0}") + "(line {1})"; }
        }
        public static string ERROR_INVALID_RANK
        {
            get { return HROne.Common.WebUtility.GetLocalizedStringByCode("ERROR_INVALID_RANK", "Invalid Rank Description: {0}") + "(line {1})"; }
        }
    }

}
