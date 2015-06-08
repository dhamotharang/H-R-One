using HROne.Lib.Entities;

namespace HROne.DataAccess
{
    public class DatabaseConnectionWithAudit : DatabaseConnection
    {
        protected EAuditTrail m_CurAuditTrail = null;
        public EAuditTrail CurAuditTrail
        {
            get { return m_CurAuditTrail; }
            set { m_CurAuditTrail = value; }
        }
        public int UserID = 0;

        public DatabaseConnectionWithAudit(string connectionString, DatabaseType databaseType)
            : base(connectionString, databaseType)
        {
        }

        public override void RollbackTransaction()
        {
            if (dbTransaction != null)
            {
                base.RollbackTransaction();
                CurAuditTrail = null;
            }
        }

        public override DatabaseConnection createClone()
        {
            DatabaseConnectionWithAudit dbConn = (DatabaseConnectionWithAudit)base.createClone();
            dbConn.UserID = UserID;
            return dbConn;
        }
    }
}
