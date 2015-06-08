using System;
using System.Collections.Generic;
using System.Text;

namespace HROne.Common
{
    public class PDFCreaterPrinter
    {
        const string DEFAULT_PRINTERNAME = "PDFCreator";
        static object lockObject = new object();

        public static string PDFCreaterPrinterName = string.Empty;

        PDFCreator.clsPDFCreatorClass creator = null;
        bool Ready = false;
        string pdfErrorMessage = string.Empty;
        public string printCrystalReportsToPDF(CrystalDecisions.CrystalReports.Engine.ReportDocument result)
        {
            System.Threading.Monitor.Enter(lockObject);

            if (string.IsNullOrEmpty(PDFCreaterPrinterName))
                PDFCreaterPrinterName = DEFAULT_PRINTERNAME;
            string pdfExportFilename = System.IO.Path.GetTempFileName();
            System.IO.File.Delete(pdfExportFilename);
            pdfExportFilename = pdfExportFilename.Replace(".tmp", ".pdf");

            bool creatorStarted = false;

            try
            {

                creator = new PDFCreator.clsPDFCreatorClass();
                creator.eError += new PDFCreator.__clsPDFCreator_eErrorEventHandler(creator_eError);
                creator.eReady += new PDFCreator.__clsPDFCreator_eReadyEventHandler(creator_eReady);
                String paramters = "/NoProcessingAtStartup";

                TimeSpan timeoutDuration = new TimeSpan(0, 0, 0, 120);
                DateTime lastCheck = DateTime.Now;
                DateTime startTime = lastCheck;

                while (!creator.cStart(paramters, false) && ((lastCheck - startTime) < timeoutDuration))
                {
                    if (creator.cError.Number == 2)
                        System.Threading.Thread.Sleep(1000);
                    else
                        throw new Exception("Cannot launch PDFCreator. Error: " + pdfErrorMessage);
                }
                creatorStarted = true;



                PDFCreator.clsPDFCreatorOptions opt = creator.cOptions;
                opt.UseAutosave = 1;
                opt.UseAutosaveDirectory = 1;
                opt.AutosaveDirectory = System.IO.Path.GetDirectoryName(pdfExportFilename);
                opt.AutosaveFormat = 0;
                //  Do NOT add the file extension to AutosaveFilename variable. 
                //  The creator will add the extension automatically.
                opt.AutosaveFilename = System.IO.Path.GetFileNameWithoutExtension(pdfExportFilename);
                creator.cOptions = opt;

                creator.cClearCache();
                creator.cDefaultPrinter = PDFCreaterPrinterName;

                result.PrintOptions.PrinterName = PDFCreaterPrinterName;

                result.PrintToPrinter(1, true, 0, 0);
                creator.cPrinterStop = false;
                //if (!creator.cIsPrintable(exportFileName))
                //{
                //    throw new Exception("File: " + exportFileName + " is not printable.");
                //}

                //creator.cPrintFile(exportFileName);

                Ready = false;

                lastCheck = DateTime.Now;
                startTime = lastCheck;

                while (!Ready && ((lastCheck - startTime) < timeoutDuration))
                {
                    System.Threading.Thread.Sleep(500);
                    lastCheck = DateTime.Now;
                }

                creator.cPrinterStop = true;
                System.Threading.Thread.Sleep(1000);
                creator.cClose();
                creatorStarted = false;
                if (!Ready)
                {
                    throw new Exception("PDF creation failed. This maybe due to timeout.");
                }
                if (!System.IO.File.Exists(pdfExportFilename))
                {
                    throw new Exception("Output file not found: " + pdfExportFilename);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (creatorStarted)
                    creator.cClose();
                System.Threading.Monitor.Exit(lockObject);
            }
            return pdfExportFilename;


        }
        void creator_eReady()
        {
            this.Ready = true;
        }

        void creator_eError()
        {
            pdfErrorMessage = creator.cError.Description;
        }
    }
}
