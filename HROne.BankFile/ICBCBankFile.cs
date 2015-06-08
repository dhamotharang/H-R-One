using System;
using System.Collections;
using System.Data;
using System.Configuration;
using System.IO;
using HROne.DataAccess;

namespace HROne.BankFile
{
    /// Summary description for HSBCBankFile
    /// </summary>
    public class ICBCBankFile : GenericBankFile 
    {
        private const string FIELD_DELIMITER = "";
        private const string RECORD_DELIMITER = "";

        public ICBCBankFile(DatabaseConnection dbConn)
            : base(dbConn)
        {
        }

        public override FileInfo GenerateBankFile()
        {
            NPOI.HSSF.UserModel.HSSFWorkbook workBook = new NPOI.HSSF.UserModel.HSSFWorkbook(System.IO.File.OpenRead(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ICBCBankFile.xls")));
            NPOI.HSSF.UserModel.HSSFSheet workSheet = (NPOI.HSSF.UserModel.HSSFSheet)workBook.GetSheetAt(0);

            int rowCount = 10;
            foreach (GenericBankFileDetail bankFileDetail in BankFileDetails)
            {
                NPOI.HSSF.UserModel.HSSFRow row = (NPOI.HSSF.UserModel.HSSFRow)workSheet.GetRow(rowCount);
                if (row == null)
                    row = (NPOI.HSSF.UserModel.HSSFRow)workSheet.CreateRow(rowCount);

                NPOI.HSSF.UserModel.HSSFCell cell = (NPOI.HSSF.UserModel.HSSFCell)row.GetCell(0);
                if (cell==null)
                    cell = (NPOI.HSSF.UserModel.HSSFCell)row.CreateCell(0);
                cell.SetCellValue(bankFileDetail.EmpNo);

                cell = (NPOI.HSSF.UserModel.HSSFCell)row.GetCell(1);
                if (cell == null)
                    cell = (NPOI.HSSF.UserModel.HSSFCell)row.CreateCell(1);
                cell.SetCellValue(bankFileDetail.EmpBankAccountHolderName);

                cell = (NPOI.HSSF.UserModel.HSSFCell)row.GetCell(2);
                if (cell == null)
                    cell = (NPOI.HSSF.UserModel.HSSFCell)row.CreateCell(2);
                cell.SetCellValue(bankFileDetail.BankCode + bankFileDetail.BranchCode + bankFileDetail.AccountNo);

                cell = (NPOI.HSSF.UserModel.HSSFCell)row.GetCell(3);
                if (cell == null)
                    cell = (NPOI.HSSF.UserModel.HSSFCell)row.CreateCell(3);
                cell.SetCellValue(bankFileDetail.Amount);

                rowCount++;
            }
            string exportFileName = System.IO.Path.GetTempFileName();
            System.IO.File.Delete(exportFileName);
            exportFileName += ".xls";

            System.IO.FileStream file = new System.IO.FileStream(exportFileName, System.IO.FileMode.Create);
            workBook.Write(file);
            file.Close();

            return new FileInfo(exportFileName);

        }

        public override string BankFileExtension()
        {
                return ".xls";
        }
    }
}