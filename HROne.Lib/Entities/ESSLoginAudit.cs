using System;
using System.Collections.Generic;
using System.Text;
using HROne.DataAccess;
////using perspectivemind.validation;

namespace HROne.Lib.Entities
{
    [DBClass("ESSLoginAudit")]
    public class EESSLoginAudit : DBObject
    {

        public static DBManager db = new DBManager(typeof(EESSLoginAudit));

        protected int m_ESSLoginAuditID;
        [DBField("ESSLoginAuditID", true, true), TextSearch, Export(false)]
        public int ESSLoginAuditID
        {
            get { return m_ESSLoginAuditID; }
            set { m_ESSLoginAuditID = value; modify("ESSLoginAuditID"); }
        }

        protected int m_EmpID;
        [DBField("EmpID"), TextSearch, Export(false)]
        public int EmpID
        {
            get { return m_EmpID; }
            set { m_EmpID = value; modify("EmpID"); }
        }

        protected string m_ESSLoginAuditLoginID;
        [DBField("ESSLoginAuditLoginID"), TextSearch, Export(false)]
        public string ESSLoginAuditLoginID
        {
            get { return m_ESSLoginAuditLoginID; }
            set { m_ESSLoginAuditLoginID = value; modify("ESSLoginAuditLoginID"); }
        }

        protected string m_ESSLoginAuditLoginMachine;
        [DBField("ESSLoginAuditLoginMachine"), TextSearch, Export(false)]
        public string ESSLoginAuditLoginMachine
        {
            get { return m_ESSLoginAuditLoginMachine; }
            set { m_ESSLoginAuditLoginMachine = value; modify("ESSLoginAuditLoginMachine"); }
        }

        protected string m_ESSLoginAuditLoginIPAddress;
        [DBField("ESSLoginAuditLoginIPAddress"), TextSearch, Export(false)]
        public string ESSLoginAuditLoginIPAddress
        {
            get { return m_ESSLoginAuditLoginIPAddress; }
            set { m_ESSLoginAuditLoginIPAddress = value; modify("ESSLoginAuditLoginIPAddress"); }
        }

        protected string m_ESSLoginAuditLoginAgent;
        [DBField("ESSLoginAuditLoginAgent"), TextSearch, Export(false)]
        public string ESSLoginAuditLoginAgent
        {
            get { return m_ESSLoginAuditLoginAgent; }
            set { m_ESSLoginAuditLoginAgent = value; modify("ESSLoginAuditLoginAgent"); }
        }

        protected DateTime m_ESSLoginAuditLoginDateTime;
        [DBField("ESSLoginAuditLoginDateTime"), TextSearch, Export(false)]
        public DateTime ESSLoginAuditLoginDateTime
        {
            get { return m_ESSLoginAuditLoginDateTime; }
            set { m_ESSLoginAuditLoginDateTime = value; modify("ESSLoginAuditLoginDateTime"); }
        }

        protected bool m_ESSLoginAuditIsLoginFail;
        [DBField("ESSLoginAuditIsLoginFail"), TextSearch, Export(false)]
        public bool ESSLoginAuditIsLoginFail
        {
            get { return m_ESSLoginAuditIsLoginFail; }
            set { m_ESSLoginAuditIsLoginFail = value; modify("ESSLoginAuditIsLoginFail"); }
        }

        protected string m_ESSLoginAuditLoginErrorMesage;
        [DBField("ESSLoginAuditLoginErrorMesage"), TextSearch, Export(false)]
        public string ESSLoginAuditLoginErrorMesage
        {
            get { return m_ESSLoginAuditLoginErrorMesage; }
            set { m_ESSLoginAuditLoginErrorMesage = value; modify("ESSLoginAuditLoginErrorMesage"); }
        }

        public static void CreateLoginAudit(DatabaseConnection dbConn, int EmpID, string LoginID, System.Web.HttpRequest request, DateTime LoginServerDateTime, bool IsFail, string message)
        {
            if (dbConn != null)
            {
                EESSLoginAudit loginAudit = new EESSLoginAudit();
                loginAudit.EmpID = EmpID;
                loginAudit.ESSLoginAuditLoginID = LoginID;
                loginAudit.ESSLoginAuditLoginAgent = request.UserAgent;
                loginAudit.ESSLoginAuditLoginDateTime = LoginServerDateTime;
                loginAudit.ESSLoginAuditLoginErrorMesage = message;
                loginAudit.ESSLoginAuditLoginIPAddress = request.UserHostAddress;
                loginAudit.ESSLoginAuditLoginMachine = request.UserHostName;
                loginAudit.ESSLoginAuditIsLoginFail = IsFail;
                EESSLoginAudit.db.insert(dbConn, loginAudit);
            }
        }
    }
}