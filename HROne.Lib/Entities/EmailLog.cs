using System;
using System.Collections.Generic;
using System.Text;
using HROne.DataAccess;
////using perspectivemind.validation;
using HROne.Common;

namespace HROne.Lib.Entities
{
    [DBClass("EmailLog")]
    public class EEmailLog : DBObject
    {
        public static DBManager db = new DBManager(typeof(EEmailLog));

        protected int m_EmailLogID;
        [DBField("EmailLogID", true, true), TextSearch, Export(false)]
        public int EmailLogID
        {
            get { return m_EmailLogID; }
            set { m_EmailLogID = value; modify("EmailLogID"); }
        }
        protected string m_EmailLogToAddress;
        [DBField("EmailLogToAddress"), TextSearch, Export(false)]
        public string EmailLogToAddress
        {
            get { return m_EmailLogToAddress; }
            set { m_EmailLogToAddress = value; modify("EmailLogToAddress"); }
        }
        protected DateTime  m_EmailLogStartTime;
        [DBField("EmailLogStartTime"), TextSearch, Export(false)]
        public DateTime EmailLogStartTime
        {
            get { return m_EmailLogStartTime; }
            set { m_EmailLogStartTime = value; modify("EmailLogStartTime"); }
        }
        protected DateTime  m_EmailLogEndTime;
        [DBField("EmailLogEndTime"), TextSearch, Export(false)]
        public DateTime EmailLogEndTime
        {
            get { return m_EmailLogEndTime; }
            set { m_EmailLogEndTime = value; modify("EmailLogEndTime"); }
        }
        protected int m_EmailLogTrialCount;
        [DBField("EmailLogTrialCount"), TextSearch, Export(false)]
        public int EmailLogTrialCount
        {
            get { return m_EmailLogTrialCount; }
            set { m_EmailLogTrialCount = value; modify("EmailLogTrialCount"); }
        }
        protected bool m_EmailLogIsFail;
        [DBField("EmailLogIsFail"), TextSearch, Export(false)]
        public bool EmailLogIsFail
        {
            get { return m_EmailLogIsFail; }
            set { m_EmailLogIsFail = value; modify("EmailLogIsFail"); }
        }

        protected string m_EmailLogErrorMessage;
        [DBField("EmailLogErrorMessage"), TextSearch, Export(false)]
        public string EmailLogErrorMessage
        {
            get { return m_EmailLogErrorMessage; }
            set { m_EmailLogErrorMessage = value; modify("EmailLogErrorMessage"); }
        }

        public static void AddSentEmailLog(DatabaseConnection dbConn, string ToMailAddress, DateTime StartTime, DateTime EndTime, int TrialCount, string ErrorMessage)
        {
            EEmailLog emailLog = new EEmailLog();
            emailLog.EmailLogToAddress = ToMailAddress;
            emailLog.EmailLogStartTime = StartTime;
            emailLog.EmailLogEndTime = EndTime;
            emailLog.m_EmailLogTrialCount = TrialCount;
            if (string.IsNullOrEmpty(ErrorMessage))
                emailLog.EmailLogIsFail = false;
            else
            {
                emailLog.EmailLogIsFail = true;
                emailLog.EmailLogErrorMessage = ErrorMessage;
            }

            EEmailLog.db.insert(dbConn, emailLog);
        }

    }
}
