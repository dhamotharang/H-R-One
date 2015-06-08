using System;
using System.Collections.Generic;
using System.Text;
using HROne.DataAccess;
////using perspectivemind.validation;

namespace HROne.Lib.Entities
{
    [DBClass("Inbox")]
    public class EInbox : BaseObject
    {
        public const string INBOX_TYPE_DATE_OF_BIRTH_18 = "DOB18";
        public const string INBOX_TYPE_DATE_OF_BIRTH_65 = "DOB65";
        public const string INBOX_TYPE_DATE_OF_BIRTH = "DOB";
        public const string INBOX_TYPE_PROBATION = "PROBATION";
        public const string INBOX_TYPE_TERMINATION = "TERMINATION";
        public const string INBOX_TYPE_WORKPERMITEXPIRY = "WORKPERMITEXPIRY";
        public const string INBOX_TYPE_MPF_FILE = "MPF";
        public const string INBOX_TYPE_AUTOPAY_FILE = "AUTOPAY";
        public const string INBOX_TYPE_FOR_ECHANNEL = "eChannel";
        public const string INBOX_TYPE_FOR_ECHANNEL_SUBMITTED = "submitted";

        public static DBManager db = new DBManager(typeof(EInbox));
        protected int m_InboxID;
        protected int attachmentFileCount = 0;

        [DBField("InboxID", true, true), TextSearch, Export(false)]
        public int InboxID
        {
            get { return m_InboxID; }
            set { m_InboxID = value; modify("InboxID"); }
        }
        protected int m_UserID;
        [DBField("UserID"), TextSearch, Export(false)]
        public int UserID
        {
            get { return m_UserID; }
            set { m_UserID = value; modify("UserID"); }
        }
        protected int m_InboxFromUserID;
        [DBField("InboxFromUserID"), TextSearch, Export(false)]
        public int InboxFromUserID
        {
            get { return m_InboxFromUserID; }
            set { m_InboxFromUserID = value; modify("InboxFromUserID"); }
        }

        protected string m_InboxFromUserName;
        [DBField("InboxFromUserName"), TextSearch, Export(false)]
        public string InboxFromUserName
        {
            get { return m_InboxFromUserName; }
            set { m_InboxFromUserName = value; modify("InboxFromUserName"); }
        }

        protected string m_InboxType;
        [DBField("InboxType"), TextSearch, Export(false)]
        public string InboxType
        {
            get { return m_InboxType; }
            set { m_InboxType = value; modify("InboxType"); }
        }

        protected string m_InboxSubject;
        [DBField("InboxSubject"), TextSearch, Export(false)]
        public string InboxSubject
        {
            get { return m_InboxSubject; }
            set { m_InboxSubject = value; modify("InboxSubject"); }
        }

        protected int m_EmpID;
        [DBField("EmpID"), TextSearch, Export(false)]
        public int EmpID
        {
            get { return m_EmpID; }
            set { m_EmpID = value; modify("EmpID"); }
        }

        protected int m_InboxRelatedRecID;
        [DBField("InboxRelatedRecID"), TextSearch, Export(false)]
        public int InboxRelatedRecID
        {
            get { return m_InboxRelatedRecID; }
            set { m_InboxRelatedRecID = value; modify("InboxRelatedRecID"); }
        }

        protected DateTime m_InboxRelatedReferenceDate;
        [DBField("InboxRelatedReferenceDate"), TextSearch, Export(false)]
        public DateTime InboxRelatedReferenceDate
        {
            get { return m_InboxRelatedReferenceDate; }
            set { m_InboxRelatedReferenceDate = value; modify("InboxRelatedReferenceDate"); }
        }

        protected string m_InboxMessage;
        [DBField("InboxMessage"), TextSearch, Export(false), DBAESEncryptStringField()]
        public string InboxMessage
        {
            get { return m_InboxMessage; }
            set { m_InboxMessage = value; modify("InboxMessage"); }
        }

        protected DateTime m_InboxCreateDate;
        [DBField("InboxCreateDate","yyyy-MM-dd HH:mm:ss"), TextSearch, Export(false)]
        public DateTime InboxCreateDate
        {
            get { return m_InboxCreateDate; }
            set { m_InboxCreateDate = value; modify("InboxCreateDate"); }
        }

        protected DateTime m_InboxReadDate;
        [DBField("InboxReadDate"), TextSearch, Export(false)]
        public DateTime InboxReadDate
        {
            get { return m_InboxReadDate; }
            set { m_InboxReadDate = value; modify("InboxReadDate"); }
        }

        protected DateTime m_InboxDeleteDate;
        [DBField("InboxDeleteDate"), TextSearch, Export(false)]
        public DateTime InboxDeleteDate
        {
            get { return m_InboxDeleteDate; }
            set { m_InboxDeleteDate = value; modify("InboxDeleteDate"); }
        }

        public string GetFromUserName(DatabaseConnection dbConn)
        {
            if (string.IsNullOrEmpty(InboxFromUserName))
            {
                if (InboxFromUserID.Equals(0))
                    return "SYSTEM";
                EUser user = new EUser();
                user.UserID = InboxFromUserID;
                if (EUser.db.select(dbConn, user))
                    if (string.IsNullOrEmpty(user.UserName))
                        return user.LoginID;
                    else
                        return user.UserName;
                else
                    return InboxFromUserID.ToString();
            }
            else
                return InboxFromUserName;
        }

        public EInboxAttachment AddInboxAttachment(DatabaseConnection dbConn, string AttachmentFileName, string AttachmentFullPath)
        {
            attachmentFileCount++;
            string uploadFolder = AppUtils.GetDocumentUploadFolder(dbConn);

            string relativePath = dbConn.Connection.Database + @"\Inbox\" + InboxID + "_" + AppUtils.ServerDateTime().ToString("yyyyMMddHHmmss") + "-" + attachmentFileCount + ".hrd";
            string destinationFile = System.IO.Path.Combine(uploadFolder, relativePath);
            System.IO.DirectoryInfo inboxDirectoryInfo = new System.IO.DirectoryInfo(System.IO.Path.GetDirectoryName(destinationFile));
            if (!inboxDirectoryInfo.Exists)
                inboxDirectoryInfo.Create();
            zip.Compress(System.IO.Path.GetDirectoryName(AttachmentFullPath), System.IO.Path.GetFileName(AttachmentFullPath), destinationFile);
            System.IO.File.Delete(AttachmentFullPath);


            EInboxAttachment attachment = new EInboxAttachment();
            attachment.InboxID = InboxID;
            attachment.InboxAttachmentOriginalFileName = AttachmentFileName;
            attachment.InboxAttachmentStoredFileName = relativePath;
            attachment.InboxAttachmentIsCompressed = true;
            attachment.InboxAttachmentSize = new System.IO.FileInfo(destinationFile).Length;
            EInboxAttachment.db.insert(dbConn, attachment);

            return attachment;
        }

        public void Delete(DatabaseConnection dbConn)
        {
            if (this.EmpID > 0)
            {
                //  Reminder, keep the record to prevent re-generate the same entry from reminder procedure
                this.InboxDeleteDate = AppUtils.ServerDateTime();
                db.update(dbConn, this);
            }
            else
            {
                string uploadFolder = uploadFolder = AppUtils.GetDocumentUploadFolder(dbConn);

                DBFilter attachmentFilter = new DBFilter();
                attachmentFilter.add(new Match("InboxID", this.InboxID));
                System.Collections.ArrayList attachmentList = EInboxAttachment.db.select(dbConn, attachmentFilter);

                foreach (EInboxAttachment attachment in attachmentList)
                {
                    try
                    {
                        string UploadFile = System.IO.Path.Combine(uploadFolder, attachment.InboxAttachmentStoredFileName);
                        if (System.IO.File.Exists(UploadFile))
                            System.IO.File.Delete(UploadFile);
                    }
                    catch
                    {
                    }
                    finally
                    {
                        EInboxAttachment.db.delete(dbConn, attachment);
                    }
                }
                db.delete(dbConn, this);
            }
        }
        public static void DeleteAllByUserID(DatabaseConnection dbConn, int UserID)
        {
            DBFilter filter = new DBFilter();
            filter.add(new Match("UserID", UserID));
            System.Collections.ArrayList inboxList = db.select(dbConn, filter);
            foreach (EInbox inbox in inboxList)
            {
                inbox.Delete(dbConn);
            }
        }
        public static void DeleteAllDeletedUserID(DatabaseConnection dbConn)
        {
            OR orUserStatus = new OR();
            orUserStatus.add(new Match("UserAccountStatus", EUser.ACCOUNT_STATUS_ACTIVE));
            orUserStatus.add(new Match("UserAccountStatus", EUser.ACCOUNT_STATUS_INACTIVE));
            DBFilter userFilter = new DBFilter();
            userFilter.add(orUserStatus);
            DBFilter filter = new DBFilter();
            filter.add(new IN("NOT UserID", "SELECT u.UserID FROM " + EUser.db.dbclass.tableName + " u", userFilter));
            System.Collections.ArrayList inboxList = db.select(dbConn, filter);
            foreach (EInbox inbox in inboxList)
            {
                inbox.Delete(dbConn);
            }
        }
        public static EInbox CreateAndSaveInboxMessage(DatabaseConnection dbConn, int ToUserID, int FromUserID, int EmpID, int RelatedRecID, string FromUserName, string InboxMessageType, string InboxSubject, string InboxMessage, DateTime RelatedReferenceDate)
        {
            EInbox inbox = new EInbox();
            inbox.UserID = ToUserID;
            inbox.InboxFromUserID = 0;
            inbox.InboxFromUserName = string.Empty;
            inbox.EmpID = EmpID;
            inbox.InboxRelatedRecID = RelatedRecID;
            inbox.InboxRelatedReferenceDate = RelatedReferenceDate;
            inbox.InboxType = InboxMessageType;
            inbox.InboxSubject = InboxSubject;
            inbox.InboxMessage = InboxMessage;
            inbox.InboxCreateDate = AppUtils.ServerDateTime();
            EInbox.db.insert(dbConn, inbox);
            return inbox;
        }

        public static void GenerateInboxMessage(DatabaseConnection dbConn, int UserID)
        {
            EUserReminderOption userReminderOption;
            if (EUserReminderOption.IsUserGenerateReminder(dbConn, UserID, EInbox.INBOX_TYPE_DATE_OF_BIRTH_18, out userReminderOption))
                Generate18AgeInboxMessage(dbConn, UserID, userReminderOption.UserReminderOptionRemindDaysBefore, userReminderOption.UserReminderOptionRemindDaysAfter);
            if (EUserReminderOption.IsUserGenerateReminder(dbConn, UserID, EInbox.INBOX_TYPE_DATE_OF_BIRTH_65, out userReminderOption))
                Generate65AgeInboxMessage(dbConn, UserID, userReminderOption.UserReminderOptionRemindDaysBefore, userReminderOption.UserReminderOptionRemindDaysAfter);
            if (EUserReminderOption.IsUserGenerateReminder(dbConn, UserID, EInbox.INBOX_TYPE_DATE_OF_BIRTH, out userReminderOption))
                GenerateBirthdayInboxMessage(dbConn, UserID, userReminderOption.UserReminderOptionRemindDaysBefore, userReminderOption.UserReminderOptionRemindDaysAfter);

            if (EUserReminderOption.IsUserGenerateReminder(dbConn, UserID, EInbox.INBOX_TYPE_PROBATION, out userReminderOption))
                GenerateProbationInboxMessage(dbConn, UserID, userReminderOption.UserReminderOptionRemindDaysBefore, userReminderOption.UserReminderOptionRemindDaysAfter);
            if (EUserReminderOption.IsUserGenerateReminder(dbConn, UserID, EInbox.INBOX_TYPE_TERMINATION, out userReminderOption))
                GenerateTerminationInboxMessage(dbConn, UserID, userReminderOption.UserReminderOptionRemindDaysBefore, userReminderOption.UserReminderOptionRemindDaysAfter);
            if (EUserReminderOption.IsUserGenerateReminder(dbConn, UserID, EInbox.INBOX_TYPE_WORKPERMITEXPIRY, out userReminderOption))
                GenerateWorkPermitExpiryInboxMessage(dbConn, UserID, userReminderOption.UserReminderOptionRemindDaysBefore, userReminderOption.UserReminderOptionRemindDaysAfter);

        }
        public static void Generate18AgeInboxMessage(DatabaseConnection dbConn, int UserID, int NoticeDaysBefore, int NoticeDaysAfter)
        {
            if (NoticeDaysBefore < 0)
                NoticeDaysBefore = 30;
            if (NoticeDaysAfter < 0)
                NoticeDaysAfter = 30;

            const int YEAR_DIFFERENT = 18;
            const string INBOX_TYPE = EInbox.INBOX_TYPE_DATE_OF_BIRTH_18;
            DBFilter DateOfBirthFilter18 = new DBFilter();
            DateOfBirthFilter18.add(AppUtils.AddRankDBTerm(UserID, "EmpID", true));

            DateOfBirthFilter18.add(new Match("EmpDateOfBirth", ">=", AppUtils.ServerDateTime().Date.AddYears(-YEAR_DIFFERENT).AddDays(-NoticeDaysAfter)));
            DateOfBirthFilter18.add(new Match("EmpDateOfBirth", "<=", AppUtils.ServerDateTime().Date.AddYears(-YEAR_DIFFERENT).AddDays(NoticeDaysBefore)));

            DBFilter existsInboxFilter = new DBFilter();
            existsInboxFilter.add(new MatchField(EEmpPersonalInfo.db.dbclass.tableName + ".EmpID", EInbox.db.dbclass.tableName + ".EmpID"));
            existsInboxFilter.add(new MatchField(EEmpPersonalInfo.db.dbclass.tableName + ".EmpID", EInbox.db.dbclass.tableName + ".InboxRelatedRecID"));
            existsInboxFilter.add(new MatchField(EEmpPersonalInfo.db.dbclass.tableName + ".EmpDateOfBirth", EInbox.db.dbclass.tableName + ".InboxRelatedReferenceDate"));
            existsInboxFilter.add(new Match(EInbox.db.dbclass.tableName + ".InboxType", INBOX_TYPE));
            existsInboxFilter.add(new Match(EInbox.db.dbclass.tableName + ".UserID", UserID));
            existsInboxFilter.add(new Match(EInbox.db.dbclass.tableName + ".InboxCreateDate", ">=", AppUtils.ServerDateTime().Date.AddYears(-YEAR_DIFFERENT).AddDays(-(NoticeDaysBefore + NoticeDaysAfter))));
            Exists existsInbox = new Exists(EInbox.db.dbclass.tableName, existsInboxFilter, true);
            DateOfBirthFilter18.add(existsInbox);

            DBFilter existsEmpTerminationFilter = new DBFilter();
            existsEmpTerminationFilter.add(new MatchField(EEmpPersonalInfo.db.dbclass.tableName + ".EmpID", EEmpTermination.db.dbclass.tableName + ".EmpID"));
            existsEmpTerminationFilter.add(new Match("EmpTermLastDate", "<=", AppUtils.ServerDateTime().Date));
            Exists notExistsEmpTermination = new Exists(EEmpTermination.db.dbclass.tableName, existsEmpTerminationFilter, true);

            DateOfBirthFilter18.add(notExistsEmpTermination);


            System.Collections.ArrayList empInfoList = EEmpPersonalInfo.db.select(dbConn, DateOfBirthFilter18);
            foreach (EEmpPersonalInfo empInfo in empInfoList)
                EInbox.CreateAndSaveInboxMessage(dbConn, UserID, 0, empInfo.EmpID, empInfo.EmpID, string.Empty, INBOX_TYPE, string.Empty, string.Empty, empInfo.EmpDateOfBirth);
        }
        public static void Generate65AgeInboxMessage(DatabaseConnection dbConn, int UserID, int NoticeDaysBefore, int NoticeDaysAfter)
        {
            if (NoticeDaysBefore < 0)
                NoticeDaysBefore = 30;
            if (NoticeDaysAfter < 0)
                NoticeDaysAfter = 30;

            const int YEAR_DIFFERENT = 65;
            const string INBOX_TYPE = EInbox.INBOX_TYPE_DATE_OF_BIRTH_65;
            DBFilter DateOfBirthFilter65 = new DBFilter();
            DateOfBirthFilter65.add(AppUtils.AddRankDBTerm(UserID, "EmpID", true));

            DateOfBirthFilter65.add(new Match("EmpDateOfBirth", ">=", AppUtils.ServerDateTime().Date.AddYears(-YEAR_DIFFERENT).AddDays(-NoticeDaysAfter)));
            DateOfBirthFilter65.add(new Match("EmpDateOfBirth", "<=", AppUtils.ServerDateTime().Date.AddYears(-YEAR_DIFFERENT).AddDays(NoticeDaysBefore)));

            DBFilter existsInboxFilter = new DBFilter();
            existsInboxFilter.add(new MatchField(EEmpPersonalInfo.db.dbclass.tableName + ".EmpID", EInbox.db.dbclass.tableName + ".EmpID"));
            existsInboxFilter.add(new MatchField(EEmpPersonalInfo.db.dbclass.tableName + ".EmpID", EInbox.db.dbclass.tableName + ".InboxRelatedRecID"));
            existsInboxFilter.add(new MatchField(EEmpPersonalInfo.db.dbclass.tableName + ".EmpDateOfBirth", EInbox.db.dbclass.tableName + ".InboxRelatedReferenceDate"));
            existsInboxFilter.add(new Match(EInbox.db.dbclass.tableName + ".InboxType", INBOX_TYPE));
            existsInboxFilter.add(new Match(EInbox.db.dbclass.tableName + ".UserID", UserID));
            existsInboxFilter.add(new Match(EInbox.db.dbclass.tableName + ".InboxCreateDate", ">=", AppUtils.ServerDateTime().Date.AddYears(-YEAR_DIFFERENT).AddDays(-(NoticeDaysBefore + NoticeDaysAfter))));
            Exists existsInbox = new Exists(EInbox.db.dbclass.tableName, existsInboxFilter, true);
            DateOfBirthFilter65.add(existsInbox);

            DBFilter existsEmpTerminationFilter = new DBFilter();
            existsEmpTerminationFilter.add(new MatchField(EEmpPersonalInfo.db.dbclass.tableName + ".EmpID", EEmpTermination.db.dbclass.tableName + ".EmpID"));
            existsEmpTerminationFilter.add(new Match("EmpTermLastDate", "<=", AppUtils.ServerDateTime().Date));
            Exists notExistsEmpTermination = new Exists(EEmpTermination.db.dbclass.tableName, existsEmpTerminationFilter, true);

            DateOfBirthFilter65.add(notExistsEmpTermination);

            System.Collections.ArrayList empInfoList = EEmpPersonalInfo.db.select(dbConn, DateOfBirthFilter65);
            foreach (EEmpPersonalInfo empInfo in empInfoList)
                EInbox.CreateAndSaveInboxMessage(dbConn, UserID, 0, empInfo.EmpID, empInfo.EmpID, string.Empty, INBOX_TYPE, string.Empty, string.Empty, empInfo.EmpDateOfBirth);
        }

        public static void GenerateBirthdayInboxMessage(DatabaseConnection dbConn, int UserID, int NoticeDaysBefore, int NoticeDaysAfter)
        {
            if (NoticeDaysBefore < 0)
                NoticeDaysBefore = 30;
            if (NoticeDaysAfter < 0)
                NoticeDaysAfter = 30;

            const string INBOX_TYPE = EInbox.INBOX_TYPE_DATE_OF_BIRTH;
            DBFilter DateOfBirthFilter = new DBFilter();
            DateOfBirthFilter.add(AppUtils.AddRankDBTerm(UserID, "EmpID", true));

            //DateOfBirthFilter.add(new Match("EmpDateOfBirth", ">=", AppUtils.ServerDateTime().Date.AddYears(-YEAR_DIFFERENT).AddDays(-NOTICEDAYSAFTER)));
            //DateOfBirthFilter.add(new Match("EmpDateOfBirth", "<=", AppUtils.ServerDateTime().Date.AddYears(-YEAR_DIFFERENT).AddDays(NOTICEDAYSBEFORE)));

            DBFilter existsInboxFilter = new DBFilter();
            existsInboxFilter.add(new MatchField(EEmpPersonalInfo.db.dbclass.tableName + ".EmpID", EInbox.db.dbclass.tableName + ".EmpID"));
            existsInboxFilter.add(new MatchField(EEmpPersonalInfo.db.dbclass.tableName + ".EmpID", EInbox.db.dbclass.tableName + ".InboxRelatedRecID"));
            existsInboxFilter.add(new MatchField(EEmpPersonalInfo.db.dbclass.tableName + ".EmpDateOfBirth", EInbox.db.dbclass.tableName + ".InboxRelatedReferenceDate"));
            existsInboxFilter.add(new Match(EInbox.db.dbclass.tableName + ".InboxType", INBOX_TYPE));
            existsInboxFilter.add(new Match(EInbox.db.dbclass.tableName + ".UserID", UserID));
            existsInboxFilter.add(new Match(EInbox.db.dbclass.tableName + ".InboxCreateDate", ">=", AppUtils.ServerDateTime().Date.AddDays(-(NoticeDaysBefore + NoticeDaysAfter))));
            Exists existsInbox = new Exists(EInbox.db.dbclass.tableName, existsInboxFilter, true);
            DateOfBirthFilter.add(existsInbox);

            DBFilter existsEmpTerminationFilter = new DBFilter();
            existsEmpTerminationFilter.add(new MatchField(EEmpPersonalInfo.db.dbclass.tableName + ".EmpID", EEmpTermination.db.dbclass.tableName + ".EmpID"));
            existsEmpTerminationFilter.add(new Match("EmpTermLastDate", "<=", AppUtils.ServerDateTime().Date));
            Exists notExistsEmpTermination = new Exists(EEmpTermination.db.dbclass.tableName, existsEmpTerminationFilter, true);
            DateOfBirthFilter.add(notExistsEmpTermination);

            System.Collections.ArrayList empInfoList = EEmpPersonalInfo.db.select(dbConn, DateOfBirthFilter);
            DateTime BirthdayPeriodFrom = AppUtils.ServerDateTime().Date.AddDays(-NoticeDaysAfter);
            DateTime BirthdayPeriodTo = AppUtils.ServerDateTime().Date.AddDays(NoticeDaysBefore);
            foreach (EEmpPersonalInfo empInfo in empInfoList)
            {
                DateTime dateOfBirth = empInfo.EmpDateOfBirth;
                int yearCount = 0;
                while (dateOfBirth <= BirthdayPeriodFrom)
                {
                    yearCount++;
                    dateOfBirth = empInfo.EmpDateOfBirth.AddYears(yearCount);
                }
                if (dateOfBirth <= BirthdayPeriodTo)
                    EInbox.CreateAndSaveInboxMessage(dbConn, UserID, 0, empInfo.EmpID, empInfo.EmpID, string.Empty, INBOX_TYPE, string.Empty, string.Empty, empInfo.EmpDateOfBirth);

            }
        }

        public static void GenerateProbationInboxMessage(DatabaseConnection dbConn, int UserID, int NoticeDaysBefore, int NoticeDaysAfter)
        {
            if (NoticeDaysBefore < 0)
                NoticeDaysBefore = 15;
            if (NoticeDaysAfter < 0)
                NoticeDaysAfter = 15;

            const string INBOX_TYPE = EInbox.INBOX_TYPE_PROBATION;
            DBFilter ProbationFilter = new DBFilter();
            ProbationFilter.add(AppUtils.AddRankDBTerm(UserID, "EmpID", true));

            ProbationFilter.add(new Match("EmpProbaLastDate", ">=", AppUtils.ServerDateTime().Date.AddDays(-NoticeDaysAfter)));
            ProbationFilter.add(new Match("EmpProbaLastDate", "<=", AppUtils.ServerDateTime().Date.AddDays(NoticeDaysBefore)));

            DBFilter existsInboxFilter = new DBFilter();
            existsInboxFilter.add(new MatchField(EEmpPersonalInfo.db.dbclass.tableName + ".EmpID", EInbox.db.dbclass.tableName + ".EmpID"));
            existsInboxFilter.add(new MatchField(EEmpPersonalInfo.db.dbclass.tableName + ".EmpID", EInbox.db.dbclass.tableName + ".InboxRelatedRecID"));
            existsInboxFilter.add(new MatchField(EEmpPersonalInfo.db.dbclass.tableName + ".EmpProbaLastDate", EInbox.db.dbclass.tableName + ".InboxRelatedReferenceDate"));
            existsInboxFilter.add(new Match(EInbox.db.dbclass.tableName + ".InboxType", INBOX_TYPE));
            existsInboxFilter.add(new Match(EInbox.db.dbclass.tableName + ".UserID", UserID));
            existsInboxFilter.add(new Match(EInbox.db.dbclass.tableName + ".InboxCreateDate", ">=", AppUtils.ServerDateTime().Date.AddDays(-(NoticeDaysBefore + NoticeDaysAfter))));
            Exists existsInbox = new Exists(EInbox.db.dbclass.tableName, existsInboxFilter, true);
            ProbationFilter.add(existsInbox);

            DBFilter existsEmpTerminationFilter = new DBFilter();
            existsEmpTerminationFilter.add(new MatchField(EEmpPersonalInfo.db.dbclass.tableName + ".EmpID", EEmpTermination.db.dbclass.tableName + ".EmpID"));
            existsEmpTerminationFilter.add(new Match("EmpTermLastDate", "<=", AppUtils.ServerDateTime().Date));
            Exists notExistsEmpTermination = new Exists(EEmpTermination.db.dbclass.tableName, existsEmpTerminationFilter, true);
            ProbationFilter.add(notExistsEmpTermination);

            System.Collections.ArrayList empInfoList = EEmpPersonalInfo.db.select(dbConn, ProbationFilter);
            foreach (EEmpPersonalInfo empInfo in empInfoList)
                EInbox.CreateAndSaveInboxMessage(dbConn, UserID, 0, empInfo.EmpID, empInfo.EmpID, string.Empty, INBOX_TYPE, string.Empty, string.Empty, empInfo.EmpProbaLastDate);

        }

        public static void GenerateTerminationInboxMessage(DatabaseConnection dbConn, int UserID, int NoticeDaysBefore, int NoticeDaysAfter)
        {
            if (NoticeDaysBefore < 0)
                NoticeDaysBefore = 15;
            if (NoticeDaysAfter < 0)
                NoticeDaysAfter = 15;

            const string INBOX_TYPE = EInbox.INBOX_TYPE_TERMINATION;
            DBFilter TerminationFilter = new DBFilter();
            TerminationFilter.add(AppUtils.AddRankDBTerm(UserID, "EmpID", true));

            TerminationFilter.add(new Match("EmpTermLastDate", ">=", AppUtils.ServerDateTime().Date.AddDays(-NoticeDaysAfter)));
            TerminationFilter.add(new Match("EmpTermLastDate", "<=", AppUtils.ServerDateTime().Date.AddDays(NoticeDaysBefore)));

            DBFilter existsInboxFilter = new DBFilter();
            existsInboxFilter.add(new MatchField(EEmpTermination.db.dbclass.tableName + ".EmpID", EInbox.db.dbclass.tableName + ".EmpID"));
            existsInboxFilter.add(new MatchField(EEmpTermination.db.dbclass.tableName + ".EmpTermID", EInbox.db.dbclass.tableName + ".InboxRelatedRecID"));
            existsInboxFilter.add(new MatchField(EEmpTermination.db.dbclass.tableName + ".EmpTermLastDate", EInbox.db.dbclass.tableName + ".InboxRelatedReferenceDate"));
            existsInboxFilter.add(new Match(EInbox.db.dbclass.tableName + ".InboxType", INBOX_TYPE));
            existsInboxFilter.add(new Match(EInbox.db.dbclass.tableName + ".UserID", UserID));
            existsInboxFilter.add(new Match(EInbox.db.dbclass.tableName + ".InboxCreateDate", ">=", AppUtils.ServerDateTime().Date.AddDays(-(NoticeDaysBefore + NoticeDaysAfter))));
            Exists existsInbox = new Exists(EInbox.db.dbclass.tableName, existsInboxFilter, true);
            TerminationFilter.add(existsInbox);


            System.Collections.ArrayList empInfoList = EEmpTermination.db.select(dbConn, TerminationFilter);
            foreach (EEmpTermination empTerm in empInfoList)
            {
                //EEmpPersonalInfo empInfo = new EEmpPersonalInfo();
                //empInfo.EmpID = empTerm.EmpID;
                //if (EEmpPersonalInfo.db.select(dbConn, empInfo))
                EInbox.CreateAndSaveInboxMessage(dbConn, UserID, 0, empTerm.EmpID, empTerm.EmpTermID, string.Empty, INBOX_TYPE, string.Empty, string.Empty, empTerm.EmpTermLastDate);
            }
        }

        public static void GenerateWorkPermitExpiryInboxMessage(DatabaseConnection dbConn, int UserID, int NoticeDaysBefore, int NoticeDaysAfter)
        {
            if (NoticeDaysBefore < 0)
                NoticeDaysBefore = 30;
            if (NoticeDaysAfter < 0)
                NoticeDaysAfter = 30;

            const string INBOX_TYPE = EInbox.INBOX_TYPE_WORKPERMITEXPIRY;
            DBFilter WorkPermitExpiryFilter = new DBFilter();
            WorkPermitExpiryFilter.add(AppUtils.AddRankDBTerm(UserID, "EmpID", true));

            WorkPermitExpiryFilter.add(new Match("EmpPermitExpiryDate", ">=", AppUtils.ServerDateTime().Date.AddDays(-NoticeDaysAfter)));
            WorkPermitExpiryFilter.add(new Match("EmpPermitExpiryDate", "<=", AppUtils.ServerDateTime().Date.AddDays(NoticeDaysBefore)));

            DBFilter existsInboxFilter = new DBFilter();
            existsInboxFilter.add(new MatchField(EEmpPermit.db.dbclass.tableName + ".EmpID", EInbox.db.dbclass.tableName + ".EmpID"));
            existsInboxFilter.add(new MatchField(EEmpPermit.db.dbclass.tableName + ".EmpPermitID", EInbox.db.dbclass.tableName + ".InboxRelatedRecID"));
            existsInboxFilter.add(new MatchField(EEmpPermit.db.dbclass.tableName + ".EmpPermitExpiryDate", EInbox.db.dbclass.tableName + ".InboxRelatedReferenceDate"));
            existsInboxFilter.add(new Match(EInbox.db.dbclass.tableName + ".InboxType", INBOX_TYPE));
            existsInboxFilter.add(new Match(EInbox.db.dbclass.tableName + ".UserID", UserID));
            existsInboxFilter.add(new Match(EInbox.db.dbclass.tableName + ".InboxCreateDate", ">=", AppUtils.ServerDateTime().Date.AddDays(-(NoticeDaysBefore + NoticeDaysAfter))));
            Exists existsInbox = new Exists(EInbox.db.dbclass.tableName, existsInboxFilter, true);
            WorkPermitExpiryFilter.add(existsInbox);

            DBFilter existsEmpTerminationFilter = new DBFilter();
            existsEmpTerminationFilter.add(new MatchField(EEmpPermit.db.dbclass.tableName + ".EmpID", EEmpTermination.db.dbclass.tableName + ".EmpID"));
            existsEmpTerminationFilter.add(new Match("EmpTermLastDate", "<=", AppUtils.ServerDateTime().Date));
            Exists notExistsEmpTermination = new Exists(EEmpTermination.db.dbclass.tableName, existsEmpTerminationFilter, true);
            WorkPermitExpiryFilter.add(notExistsEmpTermination);

            System.Collections.ArrayList empInfoList = EEmpPermit.db.select(dbConn, WorkPermitExpiryFilter);
            foreach (EEmpPermit empWorkPermit in empInfoList)
                EInbox.CreateAndSaveInboxMessage(dbConn, UserID, 0, empWorkPermit.EmpID, empWorkPermit.EmpPermitID, string.Empty, INBOX_TYPE, string.Empty, string.Empty, empWorkPermit.EmpPermitExpiryDate);

        }
    }

    [DBClass("InboxAttachment")]
    public class EInboxAttachment : BaseObject
    {
        public static DBManager db = new DBManager(typeof(EInboxAttachment));
        protected string m_ExtractedFilename = string.Empty;

        protected int m_InboxAttachmentID;
        [DBField("InboxAttachmentID", true, true), TextSearch, Export(false)]
        public int InboxAttachmentID
        {
            get { return m_InboxAttachmentID; }
            set { m_InboxAttachmentID = value; modify("InboxAttachmentID"); }
        }

        protected int m_InboxID;
        [DBField("InboxID"), TextSearch, Export(false), Required]
        public int InboxID
        {
            get { return m_InboxID; }
            set { m_InboxID = value; modify("InboxID"); }
        }

        protected string m_InboxAttachmentOriginalFileName;
        [DBField("InboxAttachmentOriginalFileName"), TextSearch, Export(false)]
        public string InboxAttachmentOriginalFileName
        {
            get { return m_InboxAttachmentOriginalFileName; }
            set { m_InboxAttachmentOriginalFileName = value; modify("InboxAttachmentOriginalFileName"); }
        }

        protected string m_InboxAttachmentStoredFileName;
        [DBField("InboxAttachmentStoredFileName"), TextSearch, Export(false)]
        public string InboxAttachmentStoredFileName
        {
            get { return m_InboxAttachmentStoredFileName; }
            set { m_InboxAttachmentStoredFileName = value; modify("InboxAttachmentStoredFileName"); }
        }

        protected bool m_InboxAttachmentIsCompressed;
        [DBField("InboxAttachmentIsCompressed"), TextSearch, Export(false)]
        public bool InboxAttachmentIsCompressed
        {
            get { return m_InboxAttachmentIsCompressed; }
            set { m_InboxAttachmentIsCompressed = value; modify("InboxAttachmentIsCompressed"); }
        }

        protected long m_InboxAttachmentSize;
        [DBField("InboxAttachmentSize"), TextSearch, Export(false), Required]
        public long InboxAttachmentSize
        {
            get { return m_InboxAttachmentSize; }
            set { m_InboxAttachmentSize = value; modify("InboxAttachmentSize"); }
        }

        

        public string GetDocumentPhysicalPath(DatabaseConnection dbConn)
        {
            //string pathDelimiter = System.IO.Path.DirectorySeparatorChar.ToString();

            string documentFilePath = System.IO.Path.Combine(AppUtils.GetDocumentUploadFolder(dbConn), InboxAttachmentStoredFileName);
            //documentFilePath = documentFilePath.Replace(pathDelimiter + pathDelimiter, pathDelimiter);

            return documentFilePath;
        }

        public void Extract(DatabaseConnection dbConn)
        {
            if (InboxAttachmentIsCompressed)
            {
                string strHRDocumentPath = GetDocumentPhysicalPath(dbConn);
                if (System.IO.File.Exists(strHRDocumentPath))
                {
                    //string pathDelimiter = System.IO.Path.DirectorySeparatorChar.ToString();
                    if (string.IsNullOrEmpty(m_ExtractedFilename))
                    {
                        string strTmpFolder = System.IO.Path.Combine(System.IO.Path.GetTempPath(), InboxAttachmentStoredFileName + ".dir");
                        //strTmpFolder = strTmpFolder.Replace(pathDelimiter + pathDelimiter, pathDelimiter);


                        zip.ExtractAll(strHRDocumentPath, strTmpFolder, string.Empty);
                        string[] fileList = System.IO.Directory.GetFiles(strTmpFolder);
                        if (fileList.GetLength(0) > 0)
                        {
                            m_ExtractedFilename = fileList[0];
                        }
                    }
                    else
                        if (!System.IO.File.Exists(m_ExtractedFilename))
                        {
                            m_ExtractedFilename = string.Empty;
                            Extract(dbConn);
                        }
                }
            }
        }

        public string GetExtractedFilePath(DatabaseConnection dbConn)
        {
            if (InboxAttachmentIsCompressed)
            {
                Extract(dbConn);
                return m_ExtractedFilename;
            }
            else
                return string.Empty;
        }

        public void RemoveExtractedFile()
        {
            if (!string.IsNullOrEmpty(m_ExtractedFilename))
            {
                if (System.IO.File.Exists(m_ExtractedFilename))
                {
                    System.IO.FileInfo fileInfo = new System.IO.FileInfo(m_ExtractedFilename);

                    fileInfo.Delete();
                    fileInfo.Directory.Delete(true);

                }
                m_ExtractedFilename = string.Empty;
            }
        }

        public static long GetTotalSize(DatabaseConnection dbConn, int UserID)
        {
            DBFilter inboxFilter = new DBFilter();
            if (UserID > 0)
                inboxFilter.add(new Match("UserID", UserID));
            DBFilter filter = new DBFilter();
            filter.add(new IN("InboxID", " SELECT inb.InboxID FROM " + EInbox.db.dbclass.tableName + " inb", inboxFilter));
            System.Data.DataTable table = filter.loadData(dbConn, "SELECT SUM(InboxAttachmentSize) FROM " + db.dbclass.tableName);
            if (table.Rows.Count == 0)
                return 0;
            if (table.Rows[0][0] == DBNull.Value)
                return 0;
            else
                return Convert.ToInt64(table.Rows[0][0]);
        }
    }
}