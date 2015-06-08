using System;
using HROne.DataAccess;

namespace HROne.DataAccess
{
    public class DBManagerWithRecordInfo : DBManager
    {
        public DBManagerWithRecordInfo(Type type)
            : base(type)
        {
        }
        public override bool insert(DatabaseConnection dbAccess, DBObject obj)
        {
            if (obj is BaseObjectWithRecordInfo)
            {
                BaseObjectWithRecordInfo tmpObj = (BaseObjectWithRecordInfo)obj;
                tmpObj.RecordCreatedDateTime = AppUtils.ServerDateTime();
                if (dbAccess is DatabaseConnectionWithAudit)
                    tmpObj.RecordCreatedBy = ((DatabaseConnectionWithAudit)dbAccess).UserID;
                else
                    tmpObj.RecordCreatedBy = 0;
                tmpObj.RecordLastModifiedBy = 0;
                tmpObj.RecordLastModifiedDateTime = new DateTime();
            }
            return base.insert(dbAccess, obj);
        }

        public override bool update(DatabaseConnection dbAccess, DBObject obj)
        {
            if (obj is BaseObjectWithRecordInfo)
            {
                BaseObjectWithRecordInfo tmpObj = (BaseObjectWithRecordInfo)obj;
                tmpObj.RecordLastModifiedDateTime = AppUtils.ServerDateTime();
                if (dbAccess is DatabaseConnectionWithAudit)
                    tmpObj.RecordLastModifiedBy = ((DatabaseConnectionWithAudit)dbAccess).UserID;
                else
                    tmpObj.RecordLastModifiedBy = 0;
            }
            return base.update(dbAccess, obj);
        }

        public override bool updateByTemplate(DatabaseConnection dbAccess, DBObject obj, DBFilter filter)
        {
            if (obj is BaseObjectWithRecordInfo)
            {
                BaseObjectWithRecordInfo tmpObj = (BaseObjectWithRecordInfo)obj;
                tmpObj.RecordLastModifiedDateTime = AppUtils.ServerDateTime();
                if (dbAccess is DatabaseConnectionWithAudit)
                    tmpObj.RecordLastModifiedBy = ((DatabaseConnectionWithAudit)dbAccess).UserID;
                else
                    tmpObj.RecordLastModifiedBy = 0;
            }
            return base.updateByTemplate(dbAccess, obj, filter);
        }
    }
}
