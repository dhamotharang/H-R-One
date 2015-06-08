using System;
using System.Collections.Generic;
using System.Text;
using HROne.DataAccess;
////using perspectivemind.validation;

namespace HROne.Lib.Entities
{
    [DBClass("LoginAudit")]
    public class ELoginAudit : DBObject
    {

        public static DBManager db = new DBManager(typeof(ELoginAudit));

        protected int m_LoginAuditID;
        [DBField("LoginAuditID", true, true), TextSearch, Export(false)]
        public int LoginAuditID
        {
            get { return m_LoginAuditID; }
            set { m_LoginAuditID = value; modify("LoginAuditID"); }
        }
        protected int m_UserID;
        [DBField("UserID"), TextSearch, Export(false)]
        public int UserID
        {
            get { return m_UserID; }
            set { m_UserID = value; modify("UserID"); }
        }
        protected string m_LoginAuditLoginID;
        [DBField("LoginAuditLoginID"), TextSearch, Export(false)]
        public string LoginAuditLoginID
        {
            get { return m_LoginAuditLoginID; }
            set { m_LoginAuditLoginID = value; modify("LoginAuditLoginID"); }
        }

        protected string m_LoginAuditLoginMachine;
        [DBField("LoginAuditLoginMachine"), TextSearch, Export(false)]
        public string LoginAuditLoginMachine
        {
            get { return m_LoginAuditLoginMachine; }
            set { m_LoginAuditLoginMachine = value; modify("LoginAuditLoginMachine"); }
        }

        protected string m_LoginAuditLoginIPAddress;
        [DBField("LoginAuditLoginIPAddress"), TextSearch, Export(false)]
        public string LoginAuditLoginIPAddress
        {
            get { return m_LoginAuditLoginIPAddress; }
            set { m_LoginAuditLoginIPAddress = value; modify("LoginAuditLoginIPAddress"); }
        }

        protected string m_LoginAuditLoginAgent;
        [DBField("LoginAuditLoginAgent"), TextSearch, Export(false)]
        public string LoginAuditLoginAgent
        {
            get { return m_LoginAuditLoginAgent; }
            set { m_LoginAuditLoginAgent = value; modify("LoginAuditLoginAgent"); }
        }

        protected DateTime m_LoginAuditLoginDateTime;
        [DBField("LoginAuditLoginDateTime"), TextSearch, Export(false)]
        public DateTime LoginAuditLoginDateTime
        {
            get { return m_LoginAuditLoginDateTime; }
            set { m_LoginAuditLoginDateTime = value; modify("LoginAuditLoginDateTime"); }
        }

        protected bool m_LoginAuditIsLoginFail;
        [DBField("LoginAuditIsLoginFail"), TextSearch, Export(false)]
        public bool LoginAuditIsLoginFail
        {
            get { return m_LoginAuditIsLoginFail; }
            set { m_LoginAuditIsLoginFail = value; modify("LoginAuditIsLoginFail"); }
        }

        protected string m_LoginAuditLoginErrorMesage;
        [DBField("LoginAuditLoginErrorMesage"), TextSearch, Export(false)]
        public string LoginAuditLoginErrorMesage
        {
            get { return m_LoginAuditLoginErrorMesage; }
            set { m_LoginAuditLoginErrorMesage = value; modify("LoginAuditLoginErrorMesage"); }
        }

        public static void CreateLoginAudit(DatabaseConnection dbConn, int UserID, string LoginID, System.Web.HttpRequest request, DateTime LoginServerDateTime, bool IsFail, string message)
        {
            if (dbConn != null)
            {
                ELoginAudit loginAudit = new ELoginAudit();
                loginAudit.UserID = UserID;
                loginAudit.LoginAuditLoginID = LoginID;
                loginAudit.LoginAuditLoginAgent = request.UserAgent;
                loginAudit.LoginAuditLoginDateTime = LoginServerDateTime;
                loginAudit.LoginAuditLoginErrorMesage = message;
                loginAudit.LoginAuditLoginIPAddress = request.UserHostAddress;
                loginAudit.LoginAuditLoginMachine = request.UserHostName;
                loginAudit.LoginAuditIsLoginFail = IsFail;
                ELoginAudit.db.insert(dbConn, loginAudit);
            }
        }
    }
}