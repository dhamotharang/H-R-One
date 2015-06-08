using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using HROne.DataAccess;

namespace HROne.Taxation
{
    public sealed class TaxationDiskProcess
    {
        DatabaseConnection dbConn;
        int taxFormID;
        public TaxationDiskProcess(DatabaseConnection dbConn, int taxFormID)
        {
            this.dbConn = dbConn;
            this.taxFormID = taxFormID;
        }

        public string GenerateToFile()
        {
            System.Text.Encoding big5 = HROne.Taxation.TaxationGeneration.GetTaxationFileEncoding();
            
            string exportFileName = exportFileName = System.IO.Path.GetTempFileName();
            exportFileName = System.IO.Path.GetTempFileName();
            System.IO.StreamWriter writer = new System.IO.StreamWriter(exportFileName, false, big5);

            //Encoding utf8 = System.Text.Encoding.GetEncoding("utf-8");
            string taxFileData = HROne.Taxation.TaxationGeneration.GenerateTaxationFileData(dbConn, taxFormID);
            //byte[] taxFileByteArray = utf8.GetBytes(taxFileData);
            //byte[] taxFileByteArrayBig5 = System.Text.Encoding.Convert(utf8, big5, taxFileByteArray);
            byte[] taxFileByteArrayBig5 = big5.GetBytes(taxFileData);

            char[] taxFileCharArrayBig5 = big5.GetChars(taxFileByteArrayBig5);
            writer.Write(taxFileCharArrayBig5);
            writer.Close();
            return exportFileName;
        }

        public string GenerateToXML()
        {
            XmlDocument doc = HROne.Taxation.TaxationGeneration.GenerateTaxationXMLData(dbConn, taxFormID);

            string exportFileName;

            exportFileName = System.IO.Path.GetTempFileName();

            doc.Save(exportFileName);

            return exportFileName;
        }
    }
}
