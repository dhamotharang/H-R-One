using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using HROne.DataAccess;


namespace HROne.Common
{
    public abstract class GenericExcelReportProcess :IDisposable 
    {
        protected DatabaseConnection dbConn;
        protected System.Globalization.CultureInfo reportCultureInfo = null;
        protected GenericExcelReportProcess(DatabaseConnection dbConn)
        {
            this.dbConn = dbConn.createClone();
            this.reportCultureInfo = System.Globalization.CultureInfo.CurrentUICulture;
        }

        protected GenericExcelReportProcess(DatabaseConnection dbConn, System.Globalization.CultureInfo reportCultureInfo)
        {
            this.dbConn = dbConn.createClone();
            this.reportCultureInfo = reportCultureInfo;
        }

        public System.IO.FileInfo GenerateExcelReport()
        {
            DataSet dataSet = CreateDataSource();
            NPOI.HSSF.UserModel.HSSFWorkbook workBook = new NPOI.HSSF.UserModel.HSSFWorkbook();
            CreateWorkBookStyle(workBook);
            GenerateWorkbookDetail(workBook, dataSet);


            return SaveToFile(workBook);
        }

        protected abstract DataSet CreateDataSource();

        protected abstract void CreateWorkBookStyle(NPOI.HSSF.UserModel.HSSFWorkbook workBook);

        protected abstract void GenerateWorkbookDetail(NPOI.HSSF.UserModel.HSSFWorkbook workBook, DataSet dataSet);

        protected virtual System.IO.FileInfo SaveToFile(NPOI.HSSF.UserModel.HSSFWorkbook workBook)
        {
            string exportFileName = System.IO.Path.GetTempFileName();
            System.IO.File.Delete(exportFileName);
            exportFileName += ".xls";
            System.IO.FileInfo outputFile = new System.IO.FileInfo(exportFileName);

            //xlsDoc.FileName = exportFileName;
            //xlsDoc.Save();
            System.IO.FileStream fileStream = outputFile.Create();// new System.IO.FileStream(exportFileName, System.IO.FileMode.Create);
            workBook.Write(fileStream);
            fileStream.Close();

            return outputFile;
        }
        protected string ToCellString(int row, int col)
        {
            string cellString = "";

            int tmpCol = col;

            while (tmpCol >= 26)
            {
                byte value = (byte)(tmpCol / 26);
                cellString += Convert.ToChar(value + 64);
                tmpCol = tmpCol % 26;
            }
            cellString += Convert.ToChar((byte)tmpCol + 65);
            return cellString + (row + 1);
        }

        #region IDisposable Members

        public void Dispose()
        {
        }

        #endregion
    }
}
