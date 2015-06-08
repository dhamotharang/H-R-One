using System;
using System.Collections.Generic;
using System.Collections;
using System.IO;
using HROne.DataAccess;
using HROne.Lib.Entities;
using HROne.Reports.Payroll;
using HROne.BankFile;

namespace HROne.TaskService
{

    public class GenerateBankFileTaskFactory : TaskService.TaskFactory
    {
        protected DatabaseConnection dbConn;
        protected EUser user;
        protected string ReportName;
        protected GenericBankFile bankFileProcess;
        protected ArrayList payBatchList;
        protected ArrayList empList;
        protected DateTime valueDate;
        protected DateTime PayPeriodFr;
        protected System.Globalization.CultureInfo reportCultureInfo;
        public GenerateBankFileTaskFactory(DatabaseConnection dbConn, EUser user, string ReportName, GenericBankFile bankFileProcess, ArrayList payBatchList, ArrayList empList, DateTime valueDate, DateTime PayPeriodFr, System.Globalization.CultureInfo reportCultureInfo)
        {
            this.dbConn = dbConn.createClone();
            this.user = user;
            this.ReportName = ReportName;
            this.bankFileProcess = bankFileProcess;
            this.payBatchList = payBatchList;
            this.empList = empList;
            this.valueDate = valueDate;
            this.PayPeriodFr = PayPeriodFr;
            this.reportCultureInfo = reportCultureInfo;
        }

        public override bool Execute()
        {
            try
            {
                bankFileProcess.LoadBankFileDetail(payBatchList, empList);
                FileInfo bankFile = bankFileProcess.GenerateBankFile();
                DateTime generateDate = AppUtils.ServerDateTime();

                string strTmpFolder = HROne.Common.Folder.GetOrCreateApplicationTempFolder().FullName;// System.IO.Path.GetTempPath(); //Environment.GetFolderPath(Environment.SpecialFolder.InternetCache);

                string autoPayFileName = bankFile.FullName;
                string autoPayFileNameExtension = autoPayFileName.Substring(autoPayFileName.LastIndexOf("."));
                string outputAutoPayFileName = bankFileProcess.ActualBankFileName(); //"BankFile_" + generateDate.ToString("yyyyMMddHHmmss") + autoPayFileNameExtension;
                string strAutoPayTmpFile = System.IO.Path.Combine(strTmpFolder, System.IO.Path.GetFileName(autoPayFileName));
                System.IO.File.Move(autoPayFileName, strAutoPayTmpFile);

                HROne.BankFile.AutopayListProcess autoPayListProcess = new HROne.BankFile.AutopayListProcess(dbConn, bankFileProcess.CreateAutopayListDataSet(), PayPeriodFr, reportCultureInfo);

                string exportFileName = autoPayListProcess.ReportExportToFile(string.Empty, "PDF", true);
                string exportFileNameExtension = exportFileName.Substring(exportFileName.LastIndexOf("."));
                string outputFileName = "AutopayList_" + generateDate.ToString("yyyyMMddHHmmss") + exportFileNameExtension;
                string strTmpFile = System.IO.Path.Combine(strTmpFolder, System.IO.Path.GetFileName(exportFileName));
                System.IO.File.Move(exportFileName, strTmpFile);

                foreach (EPayrollBatch payBatch in payBatchList)
                {
                    payBatch.PayBatchFileGenDate = generateDate;
                    payBatch.PayBatchValueDate = valueDate;
                    payBatch.PayBatchFileGenBy = user.UserID;
                    EPayrollBatch.db.update(dbConn, payBatch);
                }
                HROne.Payroll.PayrollProcess.UpdateEmpPayrollValueDate(dbConn, payBatchList, empList, valueDate, true, bankFileProcess.IsGenerateChequePayment, false, false);

                string InboxMessageType = string.Empty;
                if (bankFileProcess is HSBCBankFile && !(bankFileProcess is HSBCBankFileEncrypted))
                    if (!((HSBCBankFile)bankFileProcess).UseBIBFormat)
                        InboxMessageType = EInbox.INBOX_TYPE_AUTOPAY_FILE + "|" + EInbox.INBOX_TYPE_FOR_ECHANNEL;

                EInbox inbox = EInbox.CreateAndSaveInboxMessage(dbConn, user.UserID, 0, 0, 0, string.Empty, InboxMessageType, ReportName, string.Empty, new DateTime());

                inbox.AddInboxAttachment(dbConn, outputAutoPayFileName, strAutoPayTmpFile);
                inbox.AddInboxAttachment(dbConn, outputFileName, strTmpFile);

                if (bankFileProcess is HSBCBankFileEncrypted)
                {
                    HROne.BankFile.HSBCBankFileEncryptedCoverProcess coverProcess = new HROne.BankFile.HSBCBankFileEncryptedCoverProcess(dbConn, (HSBCBankFileEncrypted)bankFileProcess);

                    string exportCoverFileName = coverProcess.ReportExportToFile(string.Empty, "PDF", true);
                    string exportCoverFileNameExtension = exportCoverFileName.Substring(exportCoverFileName.LastIndexOf("."));
                    string outputCoverFileName = "Cover_" + generateDate.ToString("yyyyMMddHHmmss") + exportCoverFileNameExtension;
                    string strCoverTmpFile = System.IO.Path.Combine(strTmpFolder, System.IO.Path.GetFileName(exportCoverFileName));
                    System.IO.File.Move(exportCoverFileName, strCoverTmpFile);

                    inbox.AddInboxAttachment(dbConn, outputCoverFileName, strCoverTmpFile);
                    coverProcess.Dispose();
                }

                dbConn.Dispose();
                autoPayListProcess.Dispose();
                return true;
            }
            catch (NegativeAmountException ex)
            {
                string message = HROne.Common.WebUtility.GetLocalizedStringByCode("ERROR_BANKFILE_NEGATIVE", "Bank file should not contain negative amount");
                ArrayList errorBankFileList = ex.GetErrorBankFileDetailList();
                foreach (GenericBankFileDetail detail in errorBankFileList)
                {
                    message += "\r\n- " + HROne.Common.WebUtility.GetLocalizedString("EmpNo") + ": " + detail.EmpNo + ", " + HROne.Common.WebUtility.GetLocalizedString("Amount") + ": " + detail.Amount;
                }
                EInbox inbox = EInbox.CreateAndSaveInboxMessage(dbConn, user.UserID, 0, 0, 0, string.Empty, string.Empty, ReportName, message, new DateTime());
            }
            catch (InvalidEEBankAccountException ex)
            {
                string message = ex.Message;
                message += "\r\n- " + ex.EmpNo + " - " + ex.EmpName;
                EInbox inbox = EInbox.CreateAndSaveInboxMessage(dbConn, user.UserID, 0, 0, 0, string.Empty, string.Empty, ReportName, message, new DateTime());
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
