using System;
using System.Collections;
using System.Data;
using System.Data.OleDb;
using System.Data.Common;

namespace HROne.Export
{
    /// <summary>
    /// Summary description for CSVImport
    /// </summary>
    //public static class CSVReader
    //{
    //    public static DataTable parse(System.IO.Stream InputStream)
    //    {
    //        System.IO.StreamReader textReader = new System.IO.StreamReader(InputStream);
    //        string line = textReader.ReadLine();

    //        DataTable dataTable = readDataTableHeader(line);
    //        while (!textReader.EndOfStream)
    //        {
    //            line = textReader.ReadLine();
    //            string[] dataArray = CSVToStringArray(line);

    //            DataRow row = dataTable.NewRow();
    //            for (int i = 0; i < dataArray.Length; i++)
    //            {
    //                row[i] = dataArray[i];

    //            }
    //            dataTable.Rows.Add(row);
    //        }
    //        return dataTable;
    //    }

    //    private static DataTable readDataTableHeader(string headerLine)
    //    {
    //        DataTable dataTable = new DataTable();
    //        string[] headerArray = CSVToStringArray(headerLine);
    //        for (int i = 0; i < headerArray.Length; i++)
    //        {
    //            dataTable.Columns.Add(headerArray[i]);
    //        }
    //        return dataTable;

    //    }

    //    private static string[] CSVToStringArray(string recordline)
    //    {
    //        string tmpRecordLine=recordline;
    //        string tmpResult=string.Empty;
    //        string[] records = new string[0];
    //        int nextPos=0;
    //        while (tmpRecordLine.IndexOf(",",nextPos) >= 0)
    //        {
    //            int intPos = tmpRecordLine.IndexOf(",", nextPos);
    //            string firstPart = tmpRecordLine.Substring(0, intPos );
    //            string lastPart = tmpRecordLine.Substring(intPos + 1);
    //            if (firstPart.Trim().StartsWith("\""))
    //                if (firstPart.EndsWith("\""))
    //                {
    //                    firstPart = firstPart.Substring(1, firstPart.Length - 2);
    //                    if (tmpResult == string.Empty)
    //                        tmpResult = firstPart;
    //                    else
    //                        tmpResult += "\n" + firstPart;
    //                    tmpRecordLine = lastPart;
    //                    nextPos = 0;
    //                }
    //                else
    //                    nextPos = intPos+1;
    //            else
    //            {
    //                if (tmpResult == string.Empty)
    //                    tmpResult = firstPart;
    //                else
    //                    tmpResult += "\n" + firstPart;
    //                tmpRecordLine = lastPart;
    //                nextPos = 0;
    //            }

    //        }
    //        if (tmpRecordLine.Trim().StartsWith("\""))
    //            if (tmpRecordLine.EndsWith("\""))
    //            {
    //                tmpRecordLine = tmpRecordLine.Substring(1, tmpRecordLine.Length - 2);
    //                if (tmpResult == string.Empty)
    //                    tmpResult = tmpRecordLine;
    //                else
    //                    tmpResult += "\n" + tmpRecordLine;
    //            }
    //            else
    //            {
    //                throw new Exception("test");
    //            }
    //        else
    //        {
    //            if (tmpResult == string.Empty)
    //                tmpResult = tmpRecordLine;
    //            else
    //                tmpResult += "\n" + tmpRecordLine;
    //        }
    //        return tmpResult.Split(new char[]{'\n'});
    //    }
    //}

    public class ExcelExport
    {
        DbConnection objDBConn = null;
        DbProviderFactory objDBFactory = null;
        NPOI.HSSF.UserModel.HSSFWorkbook workbook;
        //ExcelLibrary.SpreadSheet.Workbook workbook;
        string ExcelFilePath;
        public ExcelExport(string ExcelFilePath)
        {

            //objDBFactory = System.Data.Common.DbProviderFactories.GetFactory("System.Data.OleDb");
            //objDBConn = objDBFactory.CreateConnection();
            //try
            //{
            //    objDBConn.ConnectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + ExcelFilePath + ";Extended Properties=Excel 8.0";
            //    objDBConn.Open();
            //    objDBConn.Close();
            //}
            //catch
            //{
            //    workbook = new NPOI.HSSF.UserModel.HSSFWorkbook();
            //    //workbook = new ExcelLibrary.SpreadSheet.Workbook();
            //    this.ExcelFilePath = ExcelFilePath;
            //}   

            workbook = new NPOI.HSSF.UserModel.HSSFWorkbook();
            //workbook = new ExcelLibrary.SpreadSheet.Workbook();
            this.ExcelFilePath = ExcelFilePath;

        }

        public void DropTable(string TableName)
        {
            objDBConn.Open();

            OleDbCommand command = (OleDbCommand)CreateCommand("Drop Table [" +TableName + "];");
            try
            {
                command.ExecuteNonQuery();
            }
            catch
            {
            }
            objDBConn.Close();

        }
        private void CreateTable(string SQLStatement)
        {
            objDBConn.Open();

            OleDbCommand command = (OleDbCommand)CreateCommand(SQLStatement);
            command.ExecuteNonQuery();
            objDBConn.Close();

        }

        private void CreateTable(DataTable dataTable)
        {
            if (workbook == null)
            {
                ArrayList list = new ArrayList();
                foreach (DataColumn column in dataTable.Columns)
                {
                    column.ColumnName = column.ColumnName.Trim();

                    string dataType = string.Empty;

                    if (column.DataType.Equals(typeof(string)))
                        dataType = "text";
                    else if (column.DataType.Equals(typeof(double)))
                        dataType = "number";
                    else if (column.DataType.Equals(typeof(int)))
                        dataType = "number";
                    else if (column.DataType.Equals(typeof(DateTime)))
                        dataType = "datetime";

                    list.Add("[" + column.ColumnName + "] " + dataType);
                }

                string SQLStatement =
                    "Create Table [" + dataTable.TableName.Replace("$", "")
                    + "] ("
                    + string.Join(", ", (string[])list.ToArray(typeof(string)))
                    + ");";
                DropTable(dataTable.TableName.Replace("$", ""));
                CreateTable(SQLStatement);
            }

        }
        //private ExcelLibrary.SpreadSheet.Worksheet CreateWorkSheet(DataTable dataTable)
        private NPOI.HSSF.UserModel.HSSFSheet CreateWorkSheet(DataTable dataTable)
        {
            if (workbook != null)
            {
                NPOI.HSSF.UserModel.HSSFDataFormat format = (NPOI.HSSF.UserModel.HSSFDataFormat)workbook.CreateDataFormat();

                NPOI.HSSF.UserModel.HSSFCellStyle dateCellStyle = (NPOI.HSSF.UserModel.HSSFCellStyle)workbook.CreateCellStyle();
                dateCellStyle.DataFormat = format.GetFormat("yyyy-MM-dd");

                NPOI.HSSF.UserModel.HSSFCellStyle numericCellStyle = (NPOI.HSSF.UserModel.HSSFCellStyle)workbook.CreateCellStyle();
                numericCellStyle.DataFormat = format.GetFormat("0.00##");

                NPOI.HSSF.UserModel.HSSFSheet worksheet = (NPOI.HSSF.UserModel.HSSFSheet)workbook.CreateSheet(dataTable.TableName.Replace("$", ""));
                //ExcelLibrary.SpreadSheet.Row headerRow = new ExcelLibrary.SpreadSheet.Row();
                NPOI.HSSF.UserModel.HSSFRow headerRow = (NPOI.HSSF.UserModel.HSSFRow)worksheet.CreateRow(0);
                int columnCount = 0;
                foreach (DataColumn headercolumn in dataTable.Columns)
                {

                    headercolumn.ColumnName = headercolumn.ColumnName.Trim();

                    NPOI.HSSF.UserModel.HSSFCell cell = (NPOI.HSSF.UserModel.HSSFCell)headerRow.CreateCell(columnCount); //new ExcelLibrary.SpreadSheet.Cell(headercolumn.ColumnName, new ExcelLibrary.SpreadSheet.CellFormat(ExcelLibrary.SpreadSheet.CellFormatType.Text, string.Empty));
                    cell.SetCellValue(headercolumn.ColumnName);

                    //headerRow.SetCell(columnCount,cell);
                    //worksheet.Cells[0, columnCount] = cell;//new ExcelLibrary.SpreadSheet.Cell(column.ColumnName, new ExcelLibrary.SpreadSheet.CellFormat(ExcelLibrary.SpreadSheet.CellFormatType.Text, string.Empty));
                    columnCount++;
                }
                //worksheet.Cells.Rows.Add(0, headerRow);

                int rowCount = 0;



                //NPOI.HSSF.UserModel.HSSFCellStyle numericCellStyle = (NPOI.HSSF.UserModel.HSSFCellStyle)workbook.CreateCellStyle();
                //numericCellStyle.DataFormat = NPOI.HSSF.UserModel.HSSFDataFormat.GetBuiltinFormat("0.00"); ;

                //NPOI.HSSF.UserModel.HSSFCellStyle integerCellStyle = (NPOI.HSSF.UserModel.HSSFCellStyle)workbook.CreateCellStyle();
                //integerCellStyle.DataFormat = NPOI.HSSF.UserModel.HSSFDataFormat.GetBuiltinFormat("0"); ;

                foreach (DataRow row in dataTable.Rows)
                {
                    rowCount++;
                    columnCount = 0;

                    //                    ExcelLibrary.SpreadSheet.Row detailRow = new ExcelLibrary.SpreadSheet.Row();

                    NPOI.HSSF.UserModel.HSSFRow detailRow = (NPOI.HSSF.UserModel.HSSFRow)worksheet.CreateRow(rowCount);

                    foreach (DataColumn column in dataTable.Columns)
                    {

                        //ExcelLibrary.SpreadSheet.Cell cell =new ExcelLibrary.SpreadSheet.Cell(string.Empty, new ExcelLibrary.SpreadSheet.CellFormat(ExcelLibrary.SpreadSheet.CellFormatType.Text, string.Empty));
                        NPOI.HSSF.UserModel.HSSFCell cell = (NPOI.HSSF.UserModel.HSSFCell)detailRow.CreateCell(columnCount);

                        if (column.DataType.Equals(typeof(string)))
                        {
                            //cell.Format = new ExcelLibrary.SpreadSheet.CellFormat(ExcelLibrary.SpreadSheet.CellFormatType.Text, string.Empty);
                            //cell.Value = (row[column] == System.DBNull.Value ? string.Empty : row[column].ToString());
                            cell.SetCellValue(row[column] == System.DBNull.Value ? string.Empty : row[column].ToString());
                        }
                        else if (column.DataType.Equals(typeof(double)) || column.DataType.Equals(typeof(float)))
                        {
                            //cell.Format = new ExcelLibrary.SpreadSheet.CellFormat(ExcelLibrary.SpreadSheet.CellFormatType.Number, "0.00");
                            //cell.Value = (row[column] == System.DBNull.Value ? string.Empty : row[column]);
                            if (row[column] != System.DBNull.Value)
                            {
                                double value = Convert.ToDouble(row[column].ToString());
                                if (value.Equals(double.NaN))
                                    cell.SetCellValue(string.Empty);
                                else
                                    cell.SetCellValue(value);
                            }
                            cell.CellStyle = numericCellStyle;
                        }
                        else if (column.DataType.Equals(typeof(int)))
                        {
                            //cell.Format = new ExcelLibrary.SpreadSheet.CellFormat(ExcelLibrary.SpreadSheet.CellFormatType.Number, "0.00");
                            //cell.Value = (row[column] == System.DBNull.Value ? string.Empty : row[column]);
                            if (row[column] != System.DBNull.Value)
                                cell.SetCellValue(Convert.ToDouble(row[column].ToString()));
                            //cell.CellStyle = integerCellStyle;
                        }
                        else if (column.DataType.Equals(typeof(DateTime)))
                        {
                            //cell.Format = new ExcelLibrary.SpreadSheet.CellFormat(ExcelLibrary.SpreadSheet.CellFormatType.DateTime, "yyyy-MM-dd");
                            //if (row[column] == System.DBNull.Value)
                            //    cell.Value = string.Empty;
                            //else
                            //    cell.Value = (DateTime)row[column];
                            if (row[column] != System.DBNull.Value)
                                cell.SetCellValue((DateTime)row[column]);

                            cell.CellStyle = dateCellStyle;
                        }
                        else
                        {
                            //cell.Format = new ExcelLibrary.SpreadSheet.CellFormat(ExcelLibrary.SpreadSheet.CellFormatType.Text, string.Empty);
                            //cell.Value = (row[column] == System.DBNull.Value ? string.Empty : row[column].ToString());
                            if (row[column] != System.DBNull.Value)
                                cell.SetCellValue(row[column].ToString());
                        }
                        //worksheet.Cells[rowCount, columnCount] = cell;
                        columnCount++;
                    }
                    //                    worksheet.Cells.Rows.Add(rowCount, detailRow);
                }
                //workbook.Worksheets.Add(worksheet);

                return worksheet;
            }
            else
                return null;
        }

        //private DataSet GetDataSet()
        //{
        //    objDBConn.Open();
        //    DataTable schemaTable = objDBConn.GetSchema("Tables");

        //    DataSet ds = new DataSet();

        //    foreach (DataRow schemaRow in schemaTable.Rows)
        //    {

        //        DbDataAdapter objDA = objDBFactory.CreateDataAdapter();
        //        objDA.SelectCommand = CreateCommand("select * from [" + schemaRow["Table_Name"].ToString() + "]");
        //        objDA.Fill(ds, schemaRow["Table_Name"].ToString());
        //        //                oda.Fill(ds);
        //    }
        //    objDBConn.Close();


        //    return ds;
        //}
        public void Update(DataTable dataTable)
        {
            DataSet ds = new DataSet();
            ds.Tables.Add(dataTable);
            Update(ds);
        }

        public void Update(DataSet ds)
        {
            if (workbook == null)
            {
                objDBConn.Open();
                DataTable schemaTable = objDBConn.GetSchema("Tables");
                objDBConn.Close();

                foreach (DataTable dataTable in ds.Tables)
                {
                    foreach (DataColumn col in dataTable.Columns)
                    {
                        if (col.ColumnName.Contains("."))
                            col.ColumnName=col.ColumnName.Replace(".","");
                        if (col.ColumnName.Contains("#"))
                            col.ColumnName = col.ColumnName.Replace("#", "Number");
                        if (col.ColumnName.Contains("!"))
                            col.ColumnName = col.ColumnName.Replace("!", "");
                        if (col.DataType.Equals(typeof(DateTime)))
                            foreach (DataRow row in dataTable.Rows)
                            {
                                if (!row.IsNull(col))
                                    if (((DateTime)row[col]).Ticks.Equals(0))
                                        row[col] = DBNull.Value;
                            }
                    }
                    DataSet tempDS = new DataSet();
                    string tableName = dataTable.TableName;

                    DataView view = new DataView(schemaTable);
                    view.RowFilter = "Table_Name='" + tableName + "'";
                    if (view.ToTable().Rows.Count <= 0)
                    {
                        CreateTable(dataTable);
                    }
                    OleDbDataAdapter objDA = (OleDbDataAdapter)objDBFactory.CreateDataAdapter();
                    objDA.SelectCommand = (OleDbCommand)CreateCommand("select * from [" + tableName + "]");
                    objDA.Fill(tempDS, tableName);
                    OleDbCommandBuilder objCB = (OleDbCommandBuilder)objDBFactory.CreateCommandBuilder();
                    objCB.QuotePrefix = "[";
                    objCB.QuoteSuffix = "]";
                    objCB.DataAdapter = objDA;
                    string fieldlist = string.Empty;
                    string valuelist = string.Empty;
                    objDA.Update(ds, tableName);

                }

                //foreach (DataRow schemaRow in schemaTable.Rows)
                //{
                //    DataSet tempDS = new DataSet();

                //    string tableName = schemaRow["Table_Name"].ToString();
                //    OleDbDataAdapter objDA = (OleDbDataAdapter)objDBFactory.CreateDataAdapter();
                //    objDA.SelectCommand = (OleDbCommand)CreateCommand("select * from [" + tableName + "]");
                //    objDA.Fill(tempDS, tableName);
                //    OleDbCommandBuilder objCB = (OleDbCommandBuilder)objDBFactory.CreateCommandBuilder();
                //    objCB.QuotePrefix = "[";
                //    objCB.QuoteSuffix = "]";
                //    objCB.DataAdapter = objDA;
                //    string fieldlist = string.Empty;
                //    string valuelist = string.Empty;
                //    //foreach (DataColumn column in ds.Tables[tableName].Columns)
                //    //{
                //    //    if (string.IsNullOrEmpty(fieldlist))
                //    //        fieldlist = "[" + column.ColumnName + "]";
                //    //    else
                //    //    {
                //    //        fieldlist += ", [" + column.ColumnName + "]";
                //    //    }
                //    //    if (string.IsNullOrEmpty(valuelist))
                //    //        valuelist = "?";
                //    //    else
                //    //    {
                //    //        valuelist += ", ?";
                //    //    }
                //    //}

                //    //foreach (DataRow row in ds.Tables[tableName].Rows)
                //    //{
                //    //    foreach (DataColumn column in ds.Tables[tableName].Columns)
                //    //    {
                //    //        if (string.IsNullOrEmpty(fieldlist))
                //    //            fieldlist = "[" + column.ColumnName + "]";
                //    //        else
                //    //        {
                //    //            fieldlist += ", [" + column.ColumnName + "]";
                //    //        }
                //    //        if (string.IsNullOrEmpty(valuelist))
                //    //            valuelist = "'" + row[column].ToString() + "'";
                //    //        else
                //    //        {
                //    //            valuelist += ", '" + row[column].ToString() + "'";
                //    //        }
                //    //    }

                //    //    DbCommand command = CreateCommand("INSERT INTO [" + tableName + "] (" + fieldlist + ") VALUES (" + valuelist + ")");
                //    //    command.ExecuteNonQuery();
                //    //}
                //    //objDA.InsertCommand = (OleDbCommand) CreateCommand("INSERT INTO [" + tableName + "] (" + fieldlist + ") VALUES (" + valuelist + ")");
                //    //foreach (DataColumn column in ds.Tables[tableName].Columns)
                //    //{

                //    //    objCB.GetInsertCommand().Parameters.Add("@" + column.ColumnName, OleDbType.VarWChar).SourceColumn = column.ColumnName; 
                //    //}
                //    objDA.Update(ds, tableName);
                //}
            }
            else
            {
                foreach (DataTable dataTable in ds.Tables)
                {
                    foreach (DataColumn col in dataTable.Columns)
                        if (col.DataType.Equals(typeof(DateTime)))
                            foreach (DataRow row in dataTable.Rows)
                            {
                                if (!row.IsNull(col))
                                    if (((DateTime)row[col]).Ticks.Equals(0))
                                        row[col] = DBNull.Value;
                            }
                    DataSet tempDS = new DataSet();
                    string tableName = dataTable.TableName;

                    CreateWorkSheet(dataTable);



                    //OleDbDataAdapter objDA = (OleDbDataAdapter)objDBFactory.CreateDataAdapter();
                    //objDA.SelectCommand = (OleDbCommand)CreateCommand("select * from [" + tableName + "]");
                    //objDA.Fill(tempDS, tableName);
                    //OleDbCommandBuilder objCB = (OleDbCommandBuilder)objDBFactory.CreateCommandBuilder();
                    //objCB.QuotePrefix = "[";
                    //objCB.QuoteSuffix = "]";
                    //objCB.DataAdapter = objDA;
                    //string fieldlist = string.Empty;
                    //string valuelist = string.Empty;
                    //objDA.Update(ds, tableName);

                }
                System.IO.FileStream file = new System.IO.FileStream(ExcelFilePath, System.IO.FileMode.Create);
                workbook.Write(file);
                file.Close();

            }
        }

        private DbCommand CreateCommand(string SQLStatement)
        {
            DbCommand objDBCommand = objDBFactory.CreateCommand();
            objDBCommand.Connection = objDBConn;
            objDBCommand.CommandText = SQLStatement;
            objDBCommand.CommandType = CommandType.Text;

            return objDBCommand;
        }
    }
}