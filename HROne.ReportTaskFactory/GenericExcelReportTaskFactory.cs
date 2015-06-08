using System;
using System.Collections.Generic;
using System.IO;
using HROne.DataAccess;
using HROne.Lib.Entities;
using HROne.Reports.Payroll;

namespace HROne.TaskService
{
    public class GenericExcelReportTaskFactory : TaskFactory
    {
        protected DatabaseConnection dbConn;
        protected EUser user;
        protected string ReportName;
        protected HROne.Common.GenericExcelReportProcess reportProcess;
        protected string OutputFilenamePrefix;

        public GenericExcelReportTaskFactory(DatabaseConnection dbConn, EUser user, string ReportName, HROne.Common.GenericExcelReportProcess reportProcess, string OutputFilenamePrefix)
        {
            this.dbConn = dbConn.createClone();
            this.user = user;
            this.ReportName = ReportName;
            this.reportProcess = reportProcess;
            this.OutputFilenamePrefix = OutputFilenamePrefix;
        }

        public override bool Execute()
        {
            try
            {
                string exportFileName = reportProcess.GenerateExcelReport().FullName;

                string exportFileNameExtension = exportFileName.Substring(exportFileName.LastIndexOf("."));
                string outputFileName = OutputFilenamePrefix + "_" + AppUtils.ServerDateTime().ToString("yyyyMMddHHmmss") + exportFileNameExtension;

                EInbox inbox = EInbox.CreateAndSaveInboxMessage(dbConn, user.UserID, 0, 0, 0, string.Empty, string.Empty, ReportName, string.Empty, new DateTime());
                string strTmpFolder = HROne.Common.Folder.GetOrCreateApplicationTempFolder().FullName;// System.IO.Path.GetTempPath(); //Environment.GetFolderPath(Environment.SpecialFolder.InternetCache);
                string strTmpFile = System.IO.Path.Combine(strTmpFolder, System.IO.Path.GetFileName(exportFileName));
                System.IO.File.Move(exportFileName, strTmpFile);

                inbox.AddInboxAttachment(dbConn, outputFileName, strTmpFile);

                //string uploadFolder = AppUtils.GetDocumentUploadFolder(dbConn);
                //string relativePath = @"Inbox\" + inbox.InboxID + "_" + AppUtils.ServerDateTime().ToString("yyyyMMddHHmmss") + ".hrd";
                //string destinationFile = System.IO.Path.Combine(uploadFolder, relativePath);
                //System.IO.DirectoryInfo inboxDirectoryInfo = new DirectoryInfo(System.IO.Path.GetDirectoryName(destinationFile));
                //if (!inboxDirectoryInfo.Exists)
                //    inboxDirectoryInfo.Create();
                //zip.Compress(strTmpFolder, System.IO.Path.GetFileName(strTmpFile), destinationFile);
                //System.IO.File.Delete(outputFileName);


                //EInboxAttachment attachment = new EInboxAttachment();
                //attachment.InboxID = inbox.InboxID;
                //attachment.InboxAttachmentOriginalFileName = outputFileName;
                //attachment.InboxAttachmentStoredFileName = relativePath;
                //attachment.InboxAttachmentIsCompressed = true;
                //attachment.InboxAttachmentSize = new System.IO.FileInfo(destinationFile).Length;
                //EInboxAttachment.db.insert(dbConn, attachment);

                dbConn.Dispose();
                reportProcess.Dispose();
                return true;
            }
            catch (Exception ex)
            {
                EInbox inbox = EInbox.CreateAndSaveInboxMessage(dbConn, user.UserID, 0, 0, 0, string.Empty, string.Empty, ReportName, ex.Message + "\r\n" + ex.StackTrace, new DateTime());
                dbConn.Dispose();
                reportProcess.Dispose();
            }
            return false;
        }
    }
}
