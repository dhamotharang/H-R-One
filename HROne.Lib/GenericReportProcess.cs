using System;
using System.Collections.Generic;
using System.Text;
using HROne.DataAccess;
using CrystalDecisions.CrystalReports.Engine;

namespace HROne.Common
{
    public class GenericReportProcess :IDisposable 
    {
        protected ReportDocument reportDocument;
        protected DatabaseConnection dbConn;
        protected System.Globalization.CultureInfo reportCultureInfo = null;
        public static bool UsePDFCreator = false;

        protected GenericReportProcess(DatabaseConnection dbConn)
        {
            if (dbConn != null)
                this.dbConn = dbConn.createClone();
            this.reportCultureInfo = System.Globalization.CultureInfo.CurrentUICulture;
        }

        protected GenericReportProcess(DatabaseConnection dbConn, System.Globalization.CultureInfo reportCultureInfo)
        {
            if (dbConn != null)
                this.dbConn = dbConn.createClone();
            this.reportCultureInfo = reportCultureInfo;
        }

        public void LoadReportDocument(ReportDocument reportDocument)
        {
            if (this.reportDocument != null)
            {
                this.reportDocument.Close();
                this.reportDocument.Dispose();
            }
            this.reportDocument = reportDocument;
        }
        public virtual ReportDocument GenerateReport()
        {
            setDataSource();
            setParameters();
            return reportDocument;
        }
        protected virtual void setDataSource()
        {
        }
        protected virtual void setParameters()
        {
        }

        public string ReportExportToFile(string reportTemplateFileName, string ExportFormat, bool IsLocalize)
        {
            if (reportTemplateFileName != string.Empty)
            {
                CrystalDecisions.CrystalReports.Engine.ReportDocument rptDocument = new CrystalDecisions.CrystalReports.Engine.ReportDocument();
                rptDocument.Load(reportTemplateFileName);
                LoadReportDocument(rptDocument);
            }




            CrystalDecisions.CrystalReports.Engine.ReportDocument result = GenerateReport();

            if (result.ParameterFields["ChineseFontName"] != null)
            {
                //System.Drawing.FontFamily chineseFontFamily = AppUtils.GetChineseFontFamily(dbConn);
                //if (chineseFontFamily != null)
                //    result.SetParameterValue("ChineseFontName", chineseFontFamily.Name);
                //else
                    result.SetParameterValue("ChineseFontName", string.Empty);
            }

            //        result.ExportToHttpResponse(CrystalDecisions.Shared.ExportFormatType.CrystalReport, System.IO.Path.GetTempPath() + @"\" + OutputFilenamePrefix + "_" + AppUtils.ServerDateTime().ToString("yyyyMMddHHmmss"));

            //result.Load(System.IO.Path.GetTempPath() + @"\" + OutputFilenamePrefix + "_" + AppUtils.ServerDateTime().ToString("yyyyMMddHHmmss"));
            if (reportTemplateFileName == string.Empty && IsLocalize)
                ReportSectionsLocalization(!ExportFormat.Equals("EXCEL", StringComparison.CurrentCultureIgnoreCase));

            //  Do NOT set the papersize by default for customized paper size
            //result.PrintOptions.PaperSize = CrystalDecisions.Shared.PaperSize.PaperA4;

            CrystalDecisions.Shared.ExportFormatType exportFormatType;
            string exportFileName = System.IO.Path.GetTempFileName();
            System.IO.File.Delete(exportFileName);
            string exportFileNameExtension = string.Empty;
            if (ExportFormat.Equals("EXCEL", StringComparison.CurrentCultureIgnoreCase))
            {
                result.PrintOptions.PaperSize = CrystalDecisions.Shared.PaperSize.PaperEsheet;


                //ExcelLibrary.SpreadSheet.Workbook workbook = ExcelLibrary.SpreadSheet.Workbook.Load(result.ExportToStream(CrystalDecisions.Shared.ExportFormatType.Excel));
                //foreach (ExcelLibrary.SpreadSheet.Worksheet workSheet in workbook.Worksheets)
                //{
                //    ExcelLibrary.SpreadSheet.CellCollection cellCollection = new ExcelLibrary.SpreadSheet.CellCollection();
                //    int count = 1;
                //    foreach (int key in workSheet.Cells.Rows.Keys)
                //    {
                //        ExcelLibrary.SpreadSheet.Row row = workSheet.Cells.Rows[key];
                //        cellCollection.Rows.Add(count, row);
                //        count++;
                //    }
                //    workSheet.Cells.Rows.Clear();

                //    foreach (int key in cellCollection.Rows.Keys)
                //    {
                //        workSheet.Cells.Rows.Add(key, cellCollection.Rows[key]);
                //    }
                //}
                //string exportFileName = System.IO.Path.GetTempFileName();
                //System.IO.File.Delete(exportFileName);
                //exportFileName += ".xls";
                //workbook.Save(exportFileName);
                //WebUtils.TransmitFile(Response, exportFileName, OutputFilenamePrefix + "_" + AppUtils.ServerDateTime().ToString("yyyyMMddHHmmss") + ".xls", true);

                //result.ExportToHttpResponse(CrystalDecisions.Shared.ExportFormatType.Excel, Response, true, OutputFilenamePrefix + "_" + AppUtils.ServerDateTime().ToString("yyyyMMddHHmmss"));

                exportFileNameExtension = ".xls";
                exportFormatType = CrystalDecisions.Shared.ExportFormatType.Excel;

            }
            else if (ExportFormat.Equals("WORD", StringComparison.CurrentCultureIgnoreCase))
            {
                exportFileNameExtension = ".doc";
                exportFormatType = CrystalDecisions.Shared.ExportFormatType.RichText;
            }
            else //if (ExportFormat.Equals("PDF", StringComparison.CurrentCultureIgnoreCase))
            {
                exportFileNameExtension = ".pdf";

                if (UsePDFCreator)
                {
                    exportFormatType = CrystalDecisions.Shared.ExportFormatType.NoFormat;
                    try
                    {
                        PDFCreaterPrinter printer = new PDFCreaterPrinter();
                        string outputFile = printer.printCrystalReportsToPDF(result);
                        System.IO.File.Move(outputFile, exportFileName + exportFileNameExtension);
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                        //  if any error occurs or driver not installed, use crystal report export function
                        // exportFormatType = CrystalDecisions.Shared.ExportFormatType.PortableDocFormat;

                    }
                }
                else
                    exportFormatType = CrystalDecisions.Shared.ExportFormatType.PortableDocFormat;
                //ReportSectionsIncreaseFontSize(result);
            }

            exportFileName += exportFileNameExtension;

            if (exportFormatType != CrystalDecisions.Shared.ExportFormatType.NoFormat)
            {
                if (exportFormatType == CrystalDecisions.Shared.ExportFormatType.PortableDocFormat)
                    if (reportTemplateFileName == string.Empty)
                        SetChineseFont(result, IsLocalize);
                    else
                        SetChineseFont(result, false);
                //result.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.CrystalReport, exportFileName);
                result.ExportToDisk(exportFormatType, exportFileName);
            }
            if (exportFileNameExtension == ".rtf" || exportFileNameExtension == ".doc")
            {
                string exportFileName_modified = exportFileName + "_modified" + exportFileNameExtension;
                System.IO.StreamReader oStrRdr = new System.IO.StreamReader(exportFileName);
                System.IO.StreamWriter oStrWrt = new System.IO.StreamWriter(exportFileName_modified);
                while (!oStrRdr.EndOfStream)
                {
                    string content = oStrRdr.ReadLine();
                    content = content.Replace(@"{\rtf1\ansi\ansicpgA.R..", @"{\rtf1\ansi\ansicpg");
                    oStrWrt.WriteLine(
                        content
                        );
                }
                oStrRdr.Close();
                oStrWrt.Close();

                System.IO.File.Delete(exportFileName);
                exportFileName = exportFileName_modified;

            }
            //if (exportFileNameExtension == ".rtf")
            //{
            //    string newExportFilename = exportFileName + ".pdf";

            //    //    iTextSharp.text.Document document = new iTextSharp.text.Document();
            //    //    iTextSharp.text.pdf.PdfWriter writer = iTextSharp.text.pdf.PdfWriter.GetInstance(document, System.IO.File.Create(newExportFilename));
            //    //    document.Open();
            //    //    iTextSharp.text.rtf.parser.RtfParser parser = new iTextSharp.text.rtf.parser.RtfParser(null);
            //    //    System.IO.FileStream rtfStream = System.IO.File.OpenRead(exportFileName);
            //    //    parser.ConvertRtfDocument(rtfStream, document);
            //    //    rtfStream.Close();
            //    //    document.Close();
            //    System.IO.File.Delete(exportFileName);
            //    exportFileName = newExportFilename;
            //}

            result.Close();
            result.Dispose();
            LoadReportDocument(null);
            return exportFileName;

        }

        private void SetChineseFont(ReportDocument result, bool IsLocalize)
        {
            System.Drawing.FontFamily chineseFontFamily = AppUtils.GetChineseFontFamily(dbConn);
            if (result.ParameterFields["ChineseFontName"] != null)
            {
                if (chineseFontFamily != null)
                    result.SetParameterValue("ChineseFontName", chineseFontFamily.Name);
                else
                    result.SetParameterValue("ChineseFontName", string.Empty);
            }
            if (IsLocalize)
                SetReportSectionChineseFont(result, chineseFontFamily);
        }

        private void SetReportSectionChineseFont(ReportDocument rptDocument, System.Drawing.FontFamily chineseFontFamily)
        {

            if (!rptDocument.IsSubreport)
                foreach (ReportDocument subReport in rptDocument.Subreports)
                    try
                    {
                        SetReportSectionChineseFont(subReport, chineseFontFamily);
                    }
                    catch (Exception e)
                    {
                        System.Diagnostics.Debug.WriteLine(e.Message);
                    }
            foreach (Section rptSection in rptDocument.ReportDefinition.Sections)
            {
                foreach (ReportObject rptObj in rptSection.ReportObjects)
                {
                    System.Reflection.PropertyInfo propInfo = rptObj.GetType().GetProperty("Text");
                    System.Reflection.PropertyInfo fontPropInfo = rptObj.GetType().GetProperty("Font");
                    if (propInfo != null && fontPropInfo != null)
                    {
                        string data = propInfo.GetValue(rptObj, null).ToString();
                        if (!IsEnglish(data) && chineseFontFamily != null)
                        {
                            System.Drawing.Font originalFont = (System.Drawing.Font)fontPropInfo.GetValue(rptObj, null);
                            System.Drawing.Font chineseFont = new System.Drawing.Font(chineseFontFamily, originalFont.Size, originalFont.Style);
                            rptObj.GetType().GetMethod("ApplyFont").Invoke(rptObj, new object[] { chineseFont });
                        }
                    }
                }
            }
        }

        public void ReportSectionsLocalization(bool IsAllowGrow)
        {
            if (reportDocument != null)
            {
                HROne.Common.WebUtility.LocalizationProcess localizationProcess = new HROne.Common.WebUtility.LocalizationProcess(dbConn, reportCultureInfo);
                localizationProcess.ReportSectionsLocalization(reportDocument, IsAllowGrow);
                //ReportSectionsLocalization(reportDocument, IsAllowGrow, reportCultureInfo);
            }
        }

        //private void ReportSectionsLocalization(ReportDocument rptDocument, bool IsAllowGrow, System.Globalization.CultureInfo ci)
        //{
        //    //System.Drawing.FontFamily chineseFontFamily = AppUtils.GetChineseFontFamily(dbConn);

        //    if (!rptDocument.IsSubreport)
        //        foreach (ReportDocument subReport in rptDocument.Subreports)
        //            try
        //            {
        //                ReportSectionsLocalization(subReport, IsAllowGrow, ci);
        //            }
        //            catch (Exception e)
        //            {
        //                System.Diagnostics.Debug.WriteLine(e.Message);
        //            }
        //    System.Resources.ResourceManager rm = AppUtils.getResourceManager();

        //    foreach (Section rptSection in rptDocument.ReportDefinition.Sections)
        //    {
        //        foreach (ReportObject rptObj in rptSection.ReportObjects)
        //        {
        //            if (rptObj is SubreportObject)
        //            {
        //                //  do nothing on subreport object because there is a bug on visual studio 2008 such that the link parameter will be broken if ANY change of subreoprt object is made.

        //            }
        //            else
        //            {
        //                try
        //                {
        //                    rptObj.ObjectFormat.EnableCanGrow = true;// IsAllowGrow;
        //                    //if (!IsAllowGrow)
        //                    //{
        //                    //    rptObj.Left *= 4;
        //                    //    rptObj.Width *= 4;
        //                    //}
        //                }
        //                catch
        //                { }
        //            }
        //            System.Reflection.PropertyInfo propInfo = rptObj.GetType().GetProperty("Text");
        //            if (rptObj is TextObject)
        //            {
        //                //                    System.Reflection.PropertyInfo propInfo = rptObj.GetType().GetProperty("Text");
        //                if (propInfo != null)
        //                {
        //                    string originalString = propInfo.GetValue(rptObj, null).ToString();
        //                    string newString = rm.GetString(originalString, ci);
        //                    if (!string.IsNullOrEmpty(newString))
        //                        propInfo.SetValue(rptObj, newString, null);
        //                    else
        //                    {
        //                        originalString = originalString.Trim();
        //                        newString = rm.GetString(originalString.Trim(), ci);
        //                        if (!string.IsNullOrEmpty(newString))
        //                            propInfo.SetValue(rptObj, newString, null);
        //                        else
        //                        {
        //                            if (originalString.EndsWith(":"))
        //                            {
        //                                originalString = originalString.Substring(0, originalString.Length - 1).Trim();
        //                                newString = rm.GetString(originalString.Trim(), ci);
        //                                if (!string.IsNullOrEmpty(newString))
        //                                    propInfo.SetValue(rptObj, newString + ":", null);
        //                            }
        //                        }
        //                    }
        //                }
        //            }

        //            //System.Reflection.PropertyInfo fontPropInfo = rptObj.GetType().GetProperty("Font");
        //            //if (propInfo != null && fontPropInfo != null)
        //            //{
        //            //    string data = propInfo.GetValue(rptObj, null).ToString();
        //            //    if (!IsEnglish(data) && chineseFontFamily != null)
        //            //    {
        //            //        System.Drawing.Font originalFont = (System.Drawing.Font)fontPropInfo.GetValue(rptObj, null);
        //            //        System.Drawing.Font chineseFont = new System.Drawing.Font(chineseFontFamily, originalFont.Size, originalFont.Style);
        //            //        rptObj.GetType().GetMethod("ApplyFont").Invoke(rptObj, new object[] { chineseFont });
        //            //    }
        //            //}
        //        }
        //    }

        //}

        //private static void ReportSectionsIncreaseFontSize(ReportDocument rptDocument)
        //{

        //    if (!rptDocument.IsSubreport)
        //        foreach (ReportDocument subReport in rptDocument.Subreports)
        //            try
        //            {
        //                ReportSectionsIncreaseFontSize(subReport);
        //            }
        //            catch (Exception e)
        //            {
        //                System.Diagnostics.Debug.WriteLine(e.Message);
        //            }

        //    foreach (Section rptSection in rptDocument.ReportDefinition.Sections)
        //    {
        //        foreach (ReportObject rptObj in rptSection.ReportObjects)
        //        {
        //            System.Reflection.PropertyInfo propInfo = rptObj.GetType().GetProperty("Text");
        //            System.Reflection.PropertyInfo fontPropInfo = rptObj.GetType().GetProperty("Font");
        //            if (fontPropInfo != null)
        //            {
        //                if (propInfo != null)
        //                {
        //                    string data = propInfo.GetValue(rptObj, null).ToString();
        //                    System.Diagnostics.Debug.WriteLine(data);
        //                }
        //                System.Drawing.Font originalFont = (System.Drawing.Font)fontPropInfo.GetValue(rptObj, null);
        //                System.Drawing.Font newFont = new System.Drawing.Font(originalFont.FontFamily, originalFont.Size + 1, originalFont.Style, originalFont.Unit);
        //                rptObj.GetType().GetMethod("ApplyFont").Invoke(rptObj, new object[] { newFont });
        //            }
        //        }
        //    }

        //}

        private static bool IsEnglish(string input)
        {
            foreach (char chr in input)
            {
                System.Globalization.UnicodeCategory cat = char.GetUnicodeCategory(chr);
                if (cat == System.Globalization.UnicodeCategory.OtherLetter)
                {
                    return false;
                }
            }
            return true;
        }


        #region IDisposable Members

        public void Dispose()
        {
            if (reportDocument != null)
            {
                reportDocument.Close();
                reportDocument.Dispose();
            }
            if (dbConn != null)
                dbConn.Dispose();
        }

        #endregion
    }
}
