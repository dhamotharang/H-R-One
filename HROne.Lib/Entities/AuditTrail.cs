using System;
using System.Collections;
using System.Text;
using HROne.DataAccess;
////using perspectivemind.validation;

namespace HROne.Lib.Entities
{
    [DBClass("AuditTrail")]
    public class EAuditTrail : DBObject
    {
        protected bool m_LogDetail = false;
        public bool LogDetail
        {
            get { return m_LogDetail; }
        }
        public static DBManager db = new DBManager(typeof(EAuditTrail));
        protected EAuditTrail ParentAuditTrailObject = null;
        public EAuditTrail()
        {
            m_LogDetail = false;
        }
        public EAuditTrail(bool LogDetail)
        {
            m_LogDetail = LogDetail;
        }
        protected int m_AuditTrailID;
        [DBField("AuditTrailID", true, true), TextSearch, Export(false)]
        public int AuditTrailID
        {
            get { return m_AuditTrailID; }
            set { m_AuditTrailID = value; modify("AuditTrailID"); }
        }
        protected int m_UserID;
        [DBField("UserID"), TextSearch, Export(false)]
        public int UserID
        {
            get { return m_UserID; }
            set { m_UserID = value; modify("UserID"); }
        }
        protected int m_FunctionID;
        [DBField("FunctionID"), TextSearch, Export(false)]
        public int FunctionID
        {
            get { return m_FunctionID; }
            set { m_FunctionID = value; modify("FunctionID"); }
        }
        protected int m_EmpID;
        [DBField("EmpID"), TextSearch, Export(false)]
        public int EmpID
        {
            get { return m_EmpID; }
            set { m_EmpID = value; modify("EmpID"); }
        }
        protected DateTime m_CreateDate;
        [DBField("CreateDate"), TextSearch, Export(false)]
        public DateTime CreateDate
        {
            get { return m_CreateDate; }
            set { m_CreateDate = value; modify("CreateDate"); }
        }
        protected int m_ParentAuditTrailID;
        [DBField("ParentAuditTrailID"), TextSearch, Export(false)]
        public int ParentAuditTrailID
        {
            get { return m_ParentAuditTrailID; }
            set { m_ParentAuditTrailID = value; modify("ParentAuditTrailID"); }
        }
        public EAuditTrail GetParentAuditTrail(DatabaseConnection dbConn)
        {
            if (ParentAuditTrailObject == null && m_ParentAuditTrailID > 0)
            {
                EAuditTrail parent = new EAuditTrail();
                parent.AuditTrailID = m_ParentAuditTrailID;
                if (EAuditTrail.db.select(dbConn, parent))
                    ParentAuditTrailObject = parent;
                return ParentAuditTrailObject;
            }
            else
                return ParentAuditTrailObject;
        }
        public EAuditTrail CreateChildAuditTrail()
        {
            EAuditTrail child = new EAuditTrail();
            child.UserID = UserID;
            child.FunctionID = FunctionID;
            child.CreateDate = CreateDate;
            child.EmpID = EmpID;
            child.m_LogDetail = m_LogDetail;
            child.ParentAuditTrailObject = this;
            child.m_ParentAuditTrailID = this.AuditTrailID;
            return child;
        }

        public string GetLogText(DatabaseConnection dbConn, string EmpNoFilter, bool ShowHeaderOnly, bool ShowKeyIDOnly, bool DoNotConvertID)
        {
            StringBuilder logTextBuilder = new StringBuilder();
            bool skipGenerateLog = false;

            DBFilter auditTrailDetailFilter = new DBFilter();
            auditTrailDetailFilter.add(new Match("AuditTrailID", this.AuditTrailID));
            ArrayList auditTrailDetailList = EAuditTrailDetail.db.select(dbConn, auditTrailDetailFilter);

            int empID = this.EmpID;
            if (!string.IsNullOrEmpty(EmpNoFilter.Trim()))
            {
                if (empID.Equals(0))
                {
                    foreach (EAuditTrailDetail auditTrailDetail in auditTrailDetailList)
                    {
                        string tmpRemark = auditTrailDetail.Remark.Replace(" ", "");
                        int pos = tmpRemark.IndexOf("\nEmpID=", StringComparison.CurrentCultureIgnoreCase);
                        if (pos >= 0)
                        {
                            string empIDString = tmpRemark.Substring(pos + 1, tmpRemark.IndexOf("\n", pos + 1) - pos);
                            empID = int.Parse(empIDString.Substring(empIDString.IndexOf("=") + 1));
                        }
                    }
                }
                EEmpPersonalInfo empInfo = new EEmpPersonalInfo();
                empInfo.EmpID = empID;
                if (EEmpPersonalInfo.db.select(dbConn, empInfo))
                {
                    if (empInfo.EmpNo.IndexOf(EmpNoFilter.Trim(), StringComparison.CurrentCultureIgnoreCase) < 0)
                        skipGenerateLog = true;
                }
                else
                    skipGenerateLog = true;
            }
            if (!skipGenerateLog)
            {
                ESystemFunction function = new ESystemFunction();
                function.FunctionID = this.FunctionID;
                if (ESystemFunction.db.select(dbConn, function))
                {
                    logTextBuilder.AppendLine(string.Empty.PadLeft(78, '-'));
                    EUser user = new EUser();
                    user.UserID = this.UserID;
                    if (EUser.db.select(dbConn, user))
                        logTextBuilder.AppendLine("User     : \t" + user.LoginID + " - " + user.UserName);
                    else if (this.UserID.Equals(0))
                        logTextBuilder.AppendLine("User     : \t" + "System");
                    else
                        logTextBuilder.AppendLine("User     : \t" + this.UserID + " (Unknown)");

                    logTextBuilder.AppendLine("Function : \t" + function.FunctionCode + " - " + function.Description);
                    logTextBuilder.AppendLine("Date/Time: \t" + this.CreateDate.ToString("yyyy-MM-dd HH:mm:ss"));

                    if (!this.EmpID.Equals(0))
                    {
                        EEmpPersonalInfo empInfo = new EEmpPersonalInfo();
                        empInfo.EmpID = this.EmpID;
                        if (EEmpPersonalInfo.db.select(dbConn, empInfo))
                            logTextBuilder.AppendLine("Employee : \t" + empInfo.EmpNo + " - " + empInfo.EmpEngFullName);
                    }

                    logTextBuilder.AppendLine(string.Empty.PadLeft(78, '-'));

                    if (!ShowHeaderOnly)
                    {
                        foreach (EAuditTrailDetail auditTrailDetail in auditTrailDetailList)
                        {
                            if ((auditTrailDetail.Remark.Replace(" ", "").IndexOf("EmpID=" + empID, StringComparison.CurrentCultureIgnoreCase) >= 0 || this.EmpID.Equals(empID)) && !string.IsNullOrEmpty(EmpNoFilter.Trim()) || string.IsNullOrEmpty(EmpNoFilter.Trim()))
                            {
                                logTextBuilder.Append(auditTrailDetail.GetLogText(dbConn, ShowKeyIDOnly, DoNotConvertID));
                            }
                        }
                    }

                }
                //logTextBuilder.AppendLine(string.Empty);
            }
            return logTextBuilder.ToString();
        }

    }

    [DBClass("AuditTrailDetail")]
    public class EAuditTrailDetail : DBObject
    {
        public const string ACTIONTYPE_UPDATE = "Update";
        public const string ACTIONTYPE_INSERT = "Insert";
        public const string ACTIONTYPE_DELETE = "Delete";
        public const string ACTIONTYPE_MARKDELETE = "MarkDelete";

        public static DBManager db = new DBManager(typeof(EAuditTrailDetail));
        protected int m_AuditTrailDetailID;
        [DBField("AuditTrailDetailID", true, true), TextSearch, Export(false)]
        public int AuditTrailDetailID
        {
            get { return m_AuditTrailDetailID; }
            set { m_AuditTrailDetailID = value; modify("AuditTrailDetailID"); }
        }
        protected int m_AuditTrailID;
        [DBField("AuditTrailID"), TextSearch, Export(false)]
        public int AuditTrailID
        {
            get { return m_AuditTrailID; }
            set { m_AuditTrailID = value; modify("AuditTrailID"); }
        }
        protected string m_TableName;
        [DBField("TableName"), TextSearch, Export(false)]
        public string TableName
        {
            get { return m_TableName; }
            set { m_TableName = value; modify("TableName"); }
        }
        protected string m_ActionType;
        [DBField("ActionType"), TextSearch, Export(false)]
        public string ActionType
        {
            get { return m_ActionType; }
            set { m_ActionType = value; modify("ActionType"); }
        }
        protected string m_Remark;
        [DBField("Remark"), TextSearch, Export(false),DBAESEncryptStringField]
        public string Remark
        {
            get { return m_Remark; }
            set { m_Remark = value; modify("Remark"); }
        }

        public string GetLogText(DatabaseConnection dbConn, bool ShowKeyIDOnly, bool DoNotConvertID)
        {
            StringBuilder logTextBuilder = new StringBuilder();
            logTextBuilder.AppendLine("Table    : \t" + this.TableName);
            if (this.ActionType.Equals(ACTIONTYPE_UPDATE))
                logTextBuilder.AppendLine("Action   : \t" + "Update");
            else if (this.ActionType.Equals(ACTIONTYPE_INSERT))
                logTextBuilder.AppendLine("Action  : \t" + "Insert");
            else if (this.ActionType.Equals(ACTIONTYPE_DELETE))
                logTextBuilder.AppendLine("Action  : \t" + "Delete");
            else if (this.ActionType.Equals(ACTIONTYPE_MARKDELETE))
                logTextBuilder.AppendLine("Action  : \t" + "Mark Delete");
            else
                logTextBuilder.AppendLine("Action  : \t" + this.ActionType);

            if (ShowKeyIDOnly)
            {
                string[] remarkList = this.Remark.Split(new string[] { "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries);
                logTextBuilder.AppendLine(remarkList[0]);
            }
            else
            {
                if (!DoNotConvertID)
                    logTextBuilder.AppendLine(TranslateIDToName(dbConn, this.Remark));
                else
                    logTextBuilder.AppendLine(this.Remark);
            }
            logTextBuilder.AppendLine(string.Empty.PadLeft(78, '-'));

            return logTextBuilder.ToString();
        }

        private static string TranslateIDToName(DatabaseConnection dbConn, string remark)
        {
            string[] remarkList = remark.Split(new string[] { "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries);
            string resultRemark = string.Empty;

            foreach (string line in remarkList)
            {
                string resultLine = line;
                if (line != remarkList[0])
                {
                    int equalSignPos = line.IndexOf("=");
                    if (equalSignPos > 0)
                    {
                        string fieldName = line.Substring(0, equalSignPos);
                        string fieldValue = line.Substring(fieldName.Length + 1);
                        if (!string.IsNullOrEmpty(fieldValue))
                        {
                            int changeArrowSignPos = fieldValue.IndexOf(" => ");

                            if (changeArrowSignPos < 0)
                            {
                                string lineDetail = Schema.GetValueFromID(dbConn, fieldName, fieldValue);
                                if (!string.IsNullOrEmpty(lineDetail))
                                    resultLine += "\t(" + lineDetail + ")";
                            }
                            else
                            {
                                string firstPart = fieldValue.Substring(0, changeArrowSignPos);
                                string firstDetail = Schema.GetValueFromID(dbConn, fieldName, firstPart);
                                string secondPart = fieldValue.Substring(changeArrowSignPos + 4);
                                string secondDetail = Schema.GetValueFromID(dbConn, fieldName, secondPart);
                                resultLine = fieldName + "="
                                    + firstPart + (string.IsNullOrEmpty(firstDetail) ? string.Empty : "\t(" + firstDetail + ")\t") + " => "
                                    + secondPart + (string.IsNullOrEmpty(secondDetail) ? string.Empty : "\t(" + secondDetail + ")");
                            }
                        }
                    }
                }

                if (string.IsNullOrEmpty(resultRemark))
                    resultRemark = resultLine;
                else
                    resultRemark += "\r\n" + resultLine;

            }
            return resultRemark;
        }

    }
}
