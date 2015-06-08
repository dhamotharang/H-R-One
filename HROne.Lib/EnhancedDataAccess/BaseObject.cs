using System;
using System.Text;
using HROne.Lib.Entities;

namespace HROne.DataAccess
{
    public class BaseObject : DBObject
    {
        protected BaseObject oldValueObject = null;
        protected DBManager getDBManager()
        {
            Type type = this.GetType();
            System.Reflection.FieldInfo fieldInfo = type.GetField("db", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static);
            if (fieldInfo != null)
                return (DBManager)fieldInfo.GetValue(null);
            else
                return null;
        }

        public bool LoadDBObject(DatabaseConnection dbConn, object key)
        {
            DBManager db = getDBManager();
            if (db != null)
            {
                if (db.keys.Count == 1)
                {
                    DBField keyField = (DBField)db.keys[0];
                    keyField.setValue(this, key);
                    return db.select(dbConn, this);
                }
            }
            return false;
        }

        protected string GetTrailUpdate(DatabaseConnection dbConn, DBManager db)
        {
            StringBuilder b = new StringBuilder();
            db.GetTrailKey(b, this);
            BaseObject newObject = (BaseObject)db.createObject();
            db.copyObject(this, newObject);

            db.select(dbConn, newObject);
            foreach (DBField f in db.fields)
            {
                if (!f.isKey)
                {
                    string newValue = f.populate(newObject);
                    string oldValue = f.populate(oldValueObject);

                    if (this.isModified(f.name))
                        if (newValue.Equals(oldValue))
                            b.Append(f.columnName).Append("=").Append(newValue).Append("\r\n");
                        else
                            b.Append(f.columnName).Append("=").Append(oldValue).Append(" => ").Append(newValue).Append("\r\n");
                }
            }
            return b.ToString();
        }

        protected override void beforeUpdate(DatabaseConnection dbConn, DBManager db)
        {

            base.beforeUpdate(dbConn, db);
            //DBManager db = getDBManager();
            oldValueObject = (BaseObject)db.createObject();
            db.copyObject(this, oldValueObject);

            if (!db.select(dbConn, oldValueObject))
            {
                oldValueObject = null;
            }

        }

        //protected BaseObject ShadowCopy()
        //{
        //    return (BaseObject)this.MemberwiseClone();
        //}

        protected override void afterUpdate(DatabaseConnection dbConn, DBManager db)
        {

            if (dbConn is DatabaseConnectionWithAudit)
            {
                DatabaseConnectionWithAudit dbConnWithAudit = (DatabaseConnectionWithAudit)dbConn;
                EAuditTrail t = dbConnWithAudit.CurAuditTrail;
                if (t == null)
                    return;
                if (!t.LogDetail)
                {
                    InsertAuditTrailDetail(dbConnWithAudit, null);
                    return;
                }
                EAuditTrailDetail d = new EAuditTrailDetail();
                d.ActionType = EAuditTrailDetail.ACTIONTYPE_UPDATE;
                d.TableName = db.dbclass.tableName;
                if (oldValueObject == null)
                    d.Remark = db.GetTrailUpdate(this);
                else
                    d.Remark = GetTrailUpdate(dbConn, db);
                InsertAuditTrailDetail(dbConnWithAudit, d);
            }
            oldValueObject = null;
        }
        protected override void afterInsert(DatabaseConnection dbConn, DBManager db)
        {
            if (dbConn is DatabaseConnectionWithAudit)
            {
                DatabaseConnectionWithAudit dbConnWithAudit = (DatabaseConnectionWithAudit)dbConn;

                EAuditTrail t = dbConnWithAudit.CurAuditTrail;
                if (t == null)
                    return;
                if (!t.LogDetail)
                {
                    InsertAuditTrailDetail(dbConnWithAudit, null);
                    return;
                }

                EAuditTrailDetail d = new EAuditTrailDetail();
                d.ActionType = EAuditTrailDetail.ACTIONTYPE_INSERT;
                d.TableName = db.dbclass.tableName;
                d.Remark = db.GetTrailInsert(this);
                InsertAuditTrailDetail(dbConnWithAudit, d);
            }
        }

        protected override void beforeDelete(DatabaseConnection dbConn, DBManager db)
        {
            if (modifiedList.Count <= 1)
            {
                db.select(dbConn, this);
            }
        }

        protected override void afterDelete(DatabaseConnection dbConn, DBManager db)
        {

            if (dbConn is DatabaseConnectionWithAudit)
            {
                DatabaseConnectionWithAudit dbConnWithAudit = (DatabaseConnectionWithAudit)dbConn;
                EAuditTrail t = dbConnWithAudit.CurAuditTrail;
                if (t == null)
                    return;
                if (!t.LogDetail)
                {
                    InsertAuditTrailDetail(dbConnWithAudit, null);
                    return;
                }

                EAuditTrailDetail d = new EAuditTrailDetail();
                d.ActionType = EAuditTrailDetail.ACTIONTYPE_DELETE;
                d.TableName = db.dbclass.tableName;
                d.Remark = db.GetTrailInsert(this);
                InsertAuditTrailDetail(dbConnWithAudit, d);
            }
        }

        protected virtual void InsertAuditTrailDetail(DatabaseConnectionWithAudit dbConnWithAudit, EAuditTrailDetail d)
        {
            EAuditTrail t = dbConnWithAudit.CurAuditTrail;
            if (t != null)
            {
                if (t.AuditTrailID <= 0)
                    EAuditTrail.db.insert(dbConnWithAudit, t);
                if (d != null)
                {
                    d.AuditTrailID = t.AuditTrailID;
                    EAuditTrailDetail.db.insert(dbConnWithAudit, d);
                }
            }
        }
        //protected string m_LastUpdateBy;
        //[DBField("lastupdateby"), TextSearch, Export(false)]
        //public string LastUpdateBy
        //{
        //    get { return m_LastUpdateBy; }
        //    set { m_LastUpdateBy = value; modify("LastUpdateBy"); }
        //}
        //protected DateTime m_LastUpdateTime;
        //[DBField("lastupdatedt", "dd-MMM-yyyy HH:mm"), DBColumn(17), TextSearch, Export(false)]
        //public DateTime LastUpdateTime
        //{
        //    get { return m_LastUpdateTime; }
        //    set { m_LastUpdateTime = value; modify("LastUpdateTime"); }
        //}


    }

    public class BaseObjectWithRecordInfo : BaseObject
    {
        protected DateTime m_RecordCreatedDateTime;
        [DBField("RecordCreatedDateTime"), TextSearch, Export(false)]
        public DateTime RecordCreatedDateTime
        {
            get { return m_RecordCreatedDateTime; }
            set { m_RecordCreatedDateTime = value; base.modify("RecordCreatedDateTime"); }
        }

        protected int m_RecordCreatedBy;
        [DBField("RecordCreatedBy"), TextSearch, Export(false)]
        public int RecordCreatedBy
        {
            get { return m_RecordCreatedBy; }
            set { m_RecordCreatedBy = value; base.modify("RecordCreatedBy"); }
        }

        protected DateTime m_RecordLastModifiedDateTime;
        [DBField("RecordLastModifiedDateTime"), TextSearch, Export(false)]
        public DateTime RecordLastModifiedDateTime
        {
            get { return m_RecordLastModifiedDateTime; }
            set { m_RecordLastModifiedDateTime = value; base.modify("RecordLastModifiedDateTime"); }
        }

        protected int m_RecordLastModifiedBy;
        [DBField("RecordLastModifiedBy"), TextSearch, Export(false)]
        public int RecordLastModifiedBy
        {
            get { return m_RecordLastModifiedBy; }
            set { m_RecordLastModifiedBy = value; base.modify("RecordLastModifiedBy"); }
        }

        protected override void beforeInsert(DatabaseConnection dbConn, DBManager db)
        {
            base.beforeInsert(dbConn, db);
            DBField syncIDField = db.getField("SynID");
            if (syncIDField != null)
            {
                if (string.IsNullOrEmpty((string)syncIDField.getValue(this)))
                {
                    syncIDField.setValue(this, Guid.NewGuid().ToString());
                }
            }
        }

        protected override void beforeUpdate(DatabaseConnection dbConn, DBManager db)
        {
            base.beforeUpdate(dbConn, db);
            DBField syncIDField = db.getField("SynID");
            if (syncIDField != null)
            {
                if (string.IsNullOrEmpty((string)syncIDField.getValue(this)))
                {
                    syncIDField.setValue(this, Guid.NewGuid().ToString().ToUpper());
                }
            }
        }
    }
}
