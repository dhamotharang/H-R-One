using System;
using System.Collections.Generic;
using System.Collections;
using System.IO;
using HROne.DataAccess;
using HROne.Lib.Entities;
using HROne.Taxation;

namespace HROne.TaskService
{

    public class TaxationDiskTaskFactory : TaskService.TaskFactory
    {
        protected DatabaseConnection dbConn;
        protected EUser user;
        protected string ReportName;
        protected int taxFormID;
        protected string NameOfSignature;
        protected string fileFormat;

        public TaxationDiskTaskFactory(DatabaseConnection dbConn, EUser user, string ReportName, int taxFormID, string NameOfSignature, string fileFormat)
        {
            this.dbConn = dbConn.createClone();
            this.user = user;
            this.ReportName = ReportName;
            this.taxFormID = taxFormID;
            this.NameOfSignature = NameOfSignature;
            this.fileFormat = fileFormat;
        }

        public override bool Execute()
        {
            try
            {
                TaxationDiskProcess process = new TaxationDiskProcess(dbConn, taxFormID);

                string taxFileName;
                string fileExtension = ".txt";
                if (fileFormat.Equals("XML", StringComparison.CurrentCultureIgnoreCase))
                {
                    taxFileName = process.GenerateToXML();
                    fileExtension = ".xml";
                }
                else
                    taxFileName = process.GenerateToFile();

                DateTime generateDate = AppUtils.ServerDateTime();

                string strTmpFolder = HROne.Common.Folder.GetOrCreateApplicationTempFolder().FullName;// System.IO.Path.GetTempPath(); //Environment.GetFolderPath(Environment.SpecialFolder.InternetCache);

                string autoPayFileNameExtension = taxFileName.Substring(taxFileName.LastIndexOf("."));
                string outputTaxFileName = "TaxationFile_" + AppUtils.ServerDateTime().ToString("yyyyMMddHHmmss") + fileExtension;
                string strTaxTmpFile = System.IO.Path.Combine(strTmpFolder, System.IO.Path.GetFileName(taxFileName));
                System.IO.File.Move(taxFileName, strTaxTmpFile);

                string strTmpReportFilePath = string.Empty;
                string reportOutputFileName = string.Empty;
                if (!fileExtension.Equals(".xml"))
                {

                    HROne.Reports.Taxation.ControlListProcess rpt = new HROne.Reports.Taxation.ControlListProcess(dbConn, taxFormID, NameOfSignature);

                    string reportFileName = rpt.ReportExportToFile(string.Empty, "PDF", false);
                    string reportFileNameExtension = reportFileName.Substring(reportFileName.LastIndexOf("."));
                    reportOutputFileName = "TaxControlList_" + generateDate.ToString("yyyyMMddHHmmss") + reportFileNameExtension;
                    strTmpReportFilePath = System.IO.Path.Combine(strTmpFolder, System.IO.Path.GetFileName(reportFileName));
                    System.IO.File.Move(reportFileName, strTmpReportFilePath);
                    rpt.Dispose();
                }
                string InboxMessageType = string.Empty;

                EInbox inbox = EInbox.CreateAndSaveInboxMessage(dbConn, user.UserID, 0, 0, 0, string.Empty, InboxMessageType, ReportName, string.Empty, new DateTime());

                inbox.AddInboxAttachment(dbConn, outputTaxFileName, strTaxTmpFile);
                if (!string.IsNullOrEmpty(strTmpReportFilePath))
                    inbox.AddInboxAttachment(dbConn, reportOutputFileName, strTmpReportFilePath);

                dbConn.Dispose();
                return true;
            }
            catch (Exception ex)
            {
                EInbox inbox = EInbox.CreateAndSaveInboxMessage(dbConn, user.UserID, 0, 0, 0, string.Empty, string.Empty, ReportName, ex.Message + "\r\n" + ex.StackTrace, new DateTime());
                dbConn.Dispose();
            }
            return false;
        }
    }
}
