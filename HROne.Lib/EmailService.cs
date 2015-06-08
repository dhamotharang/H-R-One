using System;
using System.Collections;
using System.Text;
using System.Net.Mail;
using HROne.Lib.Entities;
using HROne.DataAccess;

namespace HROne.Lib
{
    public class EmailService
    {
        protected class MessageDetail
        {
            public MailMessage mailMessage;
            public int trialCount = 0;
            public DateTime startTime;
            public ArrayList attachmentList;
            public bool deleteAttachment;
        }

        SmtpClient smtpClient = null;

        public string DefaultFromEmailAccount = string.Empty;
        public string failRecipent = string.Empty;
        public bool SendAsync = true;
        protected int pendingQueue = 0;
        protected bool IsLoadFromSystemParameter = false;
        protected DatabaseConnection dbConn = null;
        public int PendingQueueCount
        {
            get { return pendingQueue; }
        }
        protected void EmailService_OnSendCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            MessageDetail messageDetail = (MessageDetail)e.UserState;
            string mailerrorMessage = string.Empty;
            if (e.Error != null)
            {
                mailerrorMessage = e.Error.Message;
                Resend(messageDetail, mailerrorMessage);
            }
            else
                FinalizeMailing(messageDetail, mailerrorMessage);

        }
        public EmailService(DatabaseConnection dbConn)
        {
            this.dbConn = dbConn;
            LoadFromSystemParameter();
        }

        public EmailService(string SMTPServer, int Port, string UserID, string Password, bool EnableSSL)
        {
            IsLoadFromSystemParameter = false;
            if (!string.IsNullOrEmpty(SMTPServer))
            {

                SetSMTPParameter(SMTPServer, Port, UserID, Password, EnableSSL);
            }
        }

        public void LoadFromSystemParameter()
        {
            IsLoadFromSystemParameter = true;
            int port;
            string smtpServer = ESystemParameter.getParameter(dbConn, ESystemParameter.PARAM_CODE_SMTP_SERVER_NAME);
            string userID = ESystemParameter.getParameter(dbConn, ESystemParameter.PARAM_CODE_SMTP_USERNAME);
            string password;
            try
            {
                password = ESystemParameter.getParameterWithEncryption(dbConn, ESystemParameter.PARAM_CODE_SMTP_PASSWORD);
            }
            catch
            {
                password = string.Empty;
            }
            bool EnableSSL = ESystemParameter.getParameter(dbConn, ESystemParameter.PARAM_CODE_SMTP_ENABLE_SSL).Equals("Y", StringComparison.CurrentCultureIgnoreCase) ? true : false;
            //string outgoingEmail = ESystemParameter.getParameter(ESystemParameter.PARAM_CODE_SMTP_SMTP_OUTGOING_EMAIL_ADDRESS);
            //if (!string.IsNullOrEmpty(outgoingEmail))
            //{
            //    FromEmail = outgoingEmail;
            //    FromName = string.Empty;
            //}
            if (!int.TryParse(ESystemParameter.getParameter(dbConn, ESystemParameter.PARAM_CODE_SMTP_PORT), out port))
                port = 0;

            if (!string.IsNullOrEmpty(smtpServer))
            {
                SetSMTPParameter(smtpServer, port, userID, password, EnableSSL);
            }
        }

        public void SetSMTPParameter(string SMTPServer, int Port, string UserID, string Password, bool EnableSSL)
        {
            if (!string.IsNullOrEmpty(SMTPServer))
            {
                if (smtpClient == null)
                {
                    smtpClient = new SmtpClient();
                    smtpClient.SendCompleted += new SendCompletedEventHandler(EmailService_OnSendCompleted);
                }

                smtpClient.Host = SMTPServer;
                if (Port > 0)
                    smtpClient.Port = Port;
                else
                    smtpClient.Port = 25;

                smtpClient.UseDefaultCredentials = false;
                /* Email with Authentication */
                smtpClient.Credentials = new System.Net.NetworkCredential(UserID, Password);
                smtpClient.EnableSsl = EnableSSL;
                smtpClient.Timeout = 10000; 
                //------------------------------------------------------
                //Email function
            }
            else
            {
                if (smtpClient != null)
                {
                    smtpClient.SendCompleted -= new SendCompletedEventHandler(EmailService_OnSendCompleted);
                    smtpClient = null;
                }
            }
        }


        public bool SendEmail(string ToEmail, string FromEmail, string ToName, string FromName, string subject, string body)
        {
            return SendEmail(ToEmail, FromEmail, ToName, FromName, subject, body, string.Empty, string.Empty, false);
        }
        //public bool SendEmail(string ToEmail, string FromEmail, string ToName, string FromName, string subject, string body, string attachmentFileName, bool DeleteAttachment)
        //{
        //    return SendEmail(ToEmail, FromEmail, ToName, FromName, subject, body, attachmentFileName, attachmentFileName, DeleteAttachment);
        //}

        public bool SendEmail(string ToEmail, string FromEmail, string ToName, string FromName, string subject, string body, string attachmentFileName, string ActualAttachmentFileName, bool DeleteAttachment)
        {

            if (smtpClient != null)
            {
                DateTime startTime = AppUtils.ServerDateTime();

                if (DefaultFromEmailAccount.Equals(string.Empty))
                {
                    string outgoingEmail = ESystemParameter.getParameter(dbConn, ESystemParameter.PARAM_CODE_SMTP_SMTP_OUTGOING_EMAIL_ADDRESS);
                    if (!string.IsNullOrEmpty(outgoingEmail))
                    {
                        FromEmail = outgoingEmail;
                        FromName = string.Empty;
                    }
                }
                else
                {
                    FromEmail = DefaultFromEmailAccount;
                    FromName = string.Empty;

                }

                MailMessage message = new MailMessage();
                if (!string.IsNullOrEmpty(ToEmail))
                {
                    message.From = new MailAddress(FromEmail, FromName);
                    string[] ToEmailList = ToEmail.Split(new char[] { ';' });
                    string[] ToNameList = ToEmail.Split(new char[] { ';' });
                    if (ToNameList.Length != ToEmailList.Length)
                        message.To.Add(ToEmail);
                    else
                        for (int idx = 0; idx < ToEmailList.Length; idx++)
                            try
                            {
                                message.To.Add(new MailAddress(ToEmailList[idx], ToNameList[idx]));
                            }
                            catch (FormatException)
                            {
                                EEmailLog.AddSentEmailLog(dbConn, ToEmailList[idx], startTime, AppUtils.ServerDateTime(), 0, "Invalid E-mail format");
                            }
                    if (message.To.Count <= 0)
                        return false;
                    message.Subject = subject;
                    message.Body = body;
                    message.IsBodyHtml = false;
                    ArrayList attachmentList = new ArrayList();
                    if (System.IO.File.Exists(attachmentFileName))
                    {
                        Attachment attachment = new Attachment(attachmentFileName);
                        attachment.Name = ActualAttachmentFileName;
                        message.Attachments.Add(attachment);
                        attachmentList.Add(attachmentFileName);
                    }

                    if (SendAsync)
                    {
                        MessageDetail messageDetail = new MessageDetail();
                        messageDetail.mailMessage = message;
                        messageDetail.startTime = startTime;
                        messageDetail.attachmentList = attachmentList;
                        messageDetail.deleteAttachment = DeleteAttachment;
                        pendingQueue++;
                        SendAsyncMode(messageDetail);
                    }
                    else
                        smtpClient.Send(message);
                }
                return true;
            }
            else
                return false;
        }
        protected void SendAsyncMode(MessageDetail messageDetail)
        {
            try
            {
                //smtpClient.SendAsync(messageDetail.mailMessage, messageDetail);
                smtpClient.Send(messageDetail.mailMessage);
            }
            catch (SmtpException ex)
            {
                Resend(messageDetail, ex.Message);
                return;
            }
            FinalizeMailing(messageDetail, string.Empty);
        }
        protected void Resend(MessageDetail messageDetail, string mailerrorMessage)
        {
            if (messageDetail.trialCount < 3)
            {
                messageDetail.trialCount++;
                System.Threading.Thread.Sleep(new TimeSpan(0, 1 * messageDetail.trialCount, 0));
                if (IsLoadFromSystemParameter)
                    LoadFromSystemParameter();
                SendAsyncMode(messageDetail);
            }
            else
                FinalizeMailing(messageDetail, mailerrorMessage);
            //foreach (MailAddress mailAddress in messageCount.mailMessage.To)
            //{
            //    failRecipent += "\n" + mailAddress.Address;
            //}
        }
        protected void FinalizeMailing(MessageDetail messageCount, string mailerrorMessage)
        {

            EEmailLog.AddSentEmailLog(dbConn, messageCount.mailMessage.To.ToString(), messageCount.startTime, AppUtils.ServerDateTime(), messageCount.trialCount, mailerrorMessage);
            messageCount.mailMessage.Dispose();
            if (messageCount.deleteAttachment)
            {
                foreach (string attachmentFilePath in messageCount.attachmentList)
                {
                    try
                    {
                        System.IO.File.Delete(attachmentFilePath);
                    }
                    finally
                    {
                    }
                }
            }
            else
            {
            }
            pendingQueue--;
        }
    }
}
