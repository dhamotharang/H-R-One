using System;
using System.Collections.Generic;
using System.Collections;
using System.IO;
using HROne.DataAccess;
using HROne.Lib.Entities;
using HROne.Reports.Payroll;
using HROne.MPFFile;

namespace HROne.TaskService
{

    public class GenerateMPFContributionFileTaskFactory : TaskService.TaskFactory
    {
        protected DatabaseConnection dbConn;
        protected EUser user;
        protected string ReportName;
        protected GenericMPFFile mpfFileProcess;
        protected ArrayList empList;
        protected int MPFPlanID;
        protected DateTime PayPeriodFr;
        protected DateTime PayPeriodTo;
        protected System.Globalization.CultureInfo reportCultureInfo;

        public GenerateMPFContributionFileTaskFactory(DatabaseConnection dbConn, EUser user, string ReportName, GenericMPFFile mpfFileProcess, ArrayList empList, int MPFPlanID, DateTime PayPeriodFr, DateTime PayPeriodTo, System.Globalization.CultureInfo reportCultureInfo)
        {
            this.dbConn = dbConn.createClone();
            this.user = user;
            this.ReportName = ReportName;
            this.mpfFileProcess = mpfFileProcess;
            this.MPFPlanID = MPFPlanID;
            this.empList = empList;
            this.PayPeriodFr = PayPeriodFr;
            this.PayPeriodTo = PayPeriodTo;
            this.reportCultureInfo = reportCultureInfo;
        }

        public override bool Execute()
        {
            try
            {
                mpfFileProcess.LoadMPFFileDetail(empList, MPFPlanID, PayPeriodFr, PayPeriodTo);
                FileInfo mpfFile = mpfFileProcess.GenerateMPFFile();
                DateTime generateDate = AppUtils.ServerDateTime();

                string strTmpFolder = HROne.Common.Folder.GetOrCreateApplicationTempFolder().FullName;// System.IO.Path.GetTempPath(); //Environment.GetFolderPath(Environment.SpecialFolder.InternetCache);

                string mpfFileName = mpfFile.FullName;
                string mpfFileNameExtension = mpfFileName.Substring(mpfFileName.LastIndexOf("."));
                string outputMPFFileName = mpfFileProcess.ActualMPFFileName(); //"MPFFile_" + generateDate.ToString("yyyyMMddHHmmss") + mpfFileNameExtension;
                string strMPFTmpFile = System.IO.Path.Combine(strTmpFolder, System.IO.Path.GetFileName(mpfFileName));
                System.IO.File.Move(mpfFileName, strMPFTmpFile);
                
                string ChequeNo = "";   // cheque no for AIA only, not applicable to iMgr

                HROne.MPFFile.MPFRemittanceStatementProcess remittanceStatementProcess = new HROne.MPFFile.MPFRemittanceStatementProcess(dbConn, empList, MPFPlanID, PayPeriodFr, PayPeriodTo, ChequeNo);

                string exportFileName = remittanceStatementProcess.ReportExportToFile(string.Empty, "PDF", true);
                string exportFileNameExtension = exportFileName.Substring(exportFileName.LastIndexOf("."));
                string outputFileName = "MPFRemittanceStatement_" + generateDate.ToString("yyyyMMddHHmmss") + exportFileNameExtension;
                string strTmpFile = System.IO.Path.Combine(strTmpFolder, System.IO.Path.GetFileName(exportFileName));
                System.IO.File.Move(exportFileName, strTmpFile);

                string InboxMessageType = string.Empty;
                // currently only gateway format is supported for directly submitted
                if (mpfFileProcess is HSBCMPFGatewayFile && !(mpfFileProcess is HSBCMPFGatewayFileEncrypted))
                    InboxMessageType = EInbox.INBOX_TYPE_MPF_FILE + "|" + EInbox.INBOX_TYPE_FOR_ECHANNEL;

                EInbox inbox = EInbox.CreateAndSaveInboxMessage(dbConn, user.UserID, 0, 0, 0, string.Empty, InboxMessageType, ReportName, string.Empty, new DateTime());

                inbox.AddInboxAttachment(dbConn, outputMPFFileName, strMPFTmpFile);
                inbox.AddInboxAttachment(dbConn, outputFileName, strTmpFile);

                if (mpfFileProcess is HSBCMPFGatewayFileEncrypted)
                {
                    HROne.MPFFile.HSBCMPFGatewayFileEncryptedCoverProcess coverProcess = new HROne.MPFFile.HSBCMPFGatewayFileEncryptedCoverProcess(dbConn, (HSBCMPFGatewayFileEncrypted)mpfFileProcess);

                    string exportCoverFileName = coverProcess.ReportExportToFile(string.Empty, "PDF", true);
                    string exportCoverFileNameExtension = exportCoverFileName.Substring(exportCoverFileName.LastIndexOf("."));
                    string outputCoverFileName = "Cover_" + generateDate.ToString("yyyyMMddHHmmss") + exportCoverFileNameExtension;
                    string strCoverTmpFile = System.IO.Path.Combine(strTmpFolder, System.IO.Path.GetFileName(exportCoverFileName));
                    System.IO.File.Move(exportCoverFileName, strCoverTmpFile);

                    inbox.AddInboxAttachment(dbConn, outputCoverFileName, strCoverTmpFile);
                    coverProcess.Dispose();
                }

                dbConn.Dispose();
                remittanceStatementProcess.Dispose();
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
