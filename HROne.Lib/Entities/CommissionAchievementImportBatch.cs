using System;
using System.Data;
using System.Configuration;
using HROne.DataAccess;
////using perspectivemind.validation;


namespace HROne.Lib.Entities
{
    [DBClass("CommissionAchievementImportBatch")]
    public class ECommissionAchievementImportBatch : BaseObject
    {
        public static DBManager db = new DBManager(typeof(ECommissionAchievementImportBatch));
        public static WFValueList VLBatchID = new WFDBList(ECommissionAchievementImportBatch.db, "CAImportBatchID", "CAImportBatchRemark", "CAImportBatchRemark");

        public static ECommissionAchievementImportBatch GetObject(DatabaseConnection dbConn, int ID)
        {
            if (ID > 0)
            {
                ECommissionAchievementImportBatch obj = new ECommissionAchievementImportBatch();
                obj.CAImportBatchID = (int)ID;
                if (db.select(dbConn, obj))
                    return obj;
            }
            return null;
        }

        protected int m_CAImportBatchID;
        [DBField("CAImportBatchID", true, true), TextSearch, Export(false)]
        public int CAImportBatchID
        {
            get { return m_CAImportBatchID; }
            set { m_CAImportBatchID = value; modify("CAImportBatchID"); }
        }

        protected DateTime m_CAImportBatchDateTime;
        [DBField("CAImportBatchDateTime","yyyy-MM-dd HH:mm:ss"), TextSearch, Export(false)]
        public DateTime CAImportBatchDateTime
        {
            get { return m_CAImportBatchDateTime; }
            set { m_CAImportBatchDateTime = value; modify("CAImportBatchDateTime"); }
        }

        protected int m_CAImportBatchUploadedBy;
        [DBField("CAImportBatchUploadedBy"), TextSearch, Export(false)]
        public int CAImportBatchUploadedBy
        {
            get { return m_CAImportBatchUploadedBy; }
            set { m_CAImportBatchUploadedBy = value; modify("CAImportBatchUploadedBy"); }
        }

        protected string m_CAImportBatchRemark;
        [DBField("CAImportBatchRemark"), TextSearch, Export(false)]
        public string CAImportBatchRemark
        {
            get { return m_CAImportBatchRemark; }
            set { m_CAImportBatchRemark = value; modify("CAImportBatchRemark"); }
        }        
    }
}