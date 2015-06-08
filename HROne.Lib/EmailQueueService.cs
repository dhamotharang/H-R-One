using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Mail;
using HROne.Lib.Entities;
using HROne.DataAccess;

namespace HROne.TaskService
{
    public class EmailTaskFactory : HROne.TaskService.TaskFactory
    {
        protected DatabaseConnection dbConn;
        protected string ToEmail;
        protected string FromEmail;
        protected string ToName;
        protected string FromName;
        protected string subject;
        protected string body;
        protected string attachmentFilePath;
        protected string ActualAttachmentFileName;
        protected bool DeleteAttachmentAfterSent;

        public EmailTaskFactory(DatabaseConnection dbConn, string ToEmail, string FromEmail, string ToName, string FromName, string subject, string body, string attachmentFilePath, string ActualAttachmentFileName, bool DeleteAttachmentAfterSent)
        {
            this.dbConn = dbConn.createClone();
            this.ToEmail = ToEmail;
            this.FromEmail = FromEmail;
            this.ToName = ToName;
            this.FromName = FromName;
            this.subject = subject;
            this.body = body;
            this.attachmentFilePath = attachmentFilePath;
            this.ActualAttachmentFileName = ActualAttachmentFileName;
            this.DeleteAttachmentAfterSent = DeleteAttachmentAfterSent;
        }
        public override bool Execute()
        {
            HROne.Lib.EmailService emailService = new HROne.Lib.EmailService(dbConn);
            return emailService.SendEmail(ToEmail, FromEmail, ToName, FromName, subject, body, attachmentFilePath, ActualAttachmentFileName, DeleteAttachmentAfterSent);
        }
    }
    //public class EmailQueueParameter
    //{
    //    public DatabaseConnection dbConn;
    //    public string ToEmail;
    //    public string FromEmail;
    //    public string ToName;
    //    public string FromName;
    //    public string subject;
    //    public string body;
    //    public string attachmentFileName;
    //    public string ActualAttachmentFileName;
    //    public bool DeleteAttachmentAfterSent;
    //}

    //public class EmailQueueService
    //{
    //    protected System.Threading.Thread mailServiceThread = null;
    //    public static EmailQueueService ActiveEmailQueueService = null;
    //    protected System.Collections.Generic.Queue<EmailQueueParameter> mailQueue = new System.Collections.Generic.Queue<EmailQueueParameter>();

    //    public static System.Threading.Thread SendEmail(EmailQueueParameter mailParameter)
    //    {
    //        if (ActiveEmailQueueService == null)
    //            ActiveEmailQueueService = new EmailQueueService();
    //        ActiveEmailQueueService.mailQueue.Enqueue(mailParameter);
    //        if (ActiveEmailQueueService.mailServiceThread == null)
    //        {
    //            ActiveEmailQueueService.mailServiceThread = new System.Threading.Thread(new System.Threading.ThreadStart(ActiveEmailQueueService.Start));
    //            ActiveEmailQueueService.mailServiceThread.IsBackground = false;
    //            ActiveEmailQueueService.mailServiceThread.Start();
    //        }

    //        return ActiveEmailQueueService.mailServiceThread;
    //    }
    //    public void Start()
    //    {
    //        while (mailQueue.Count > 0)
    //        {
    //            EmailQueueParameter currentEmailParameter = mailQueue.Peek();

    //            EmailService emailService = new EmailService(currentEmailParameter.dbConn);
    //            bool result = emailService.SendEmail(currentEmailParameter.ToEmail, currentEmailParameter.FromEmail, currentEmailParameter.ToName, currentEmailParameter.FromName, currentEmailParameter.subject, currentEmailParameter.body);
    //            if (!result)
    //                System.Threading.Thread.Sleep(10000);
    //            mailQueue.Dequeue();
    //        }
    //        mailServiceThread = null;
    //    }

    //}
}
