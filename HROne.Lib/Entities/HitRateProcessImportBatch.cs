using System;
using System.Data;
using System.Configuration;
using HROne.DataAccess;
////using perspectivemind.validation;


namespace HROne.Lib.Entities
{
    [DBClass("HitRateProcessImportBatch")]
    public 
        class EHitRateProcessImportBatch : BaseObject
    {
        public static DBManager db = new DBManager(typeof(EHitRateProcessImportBatch));
        public static WFValueList VLBatchID = new WFDBList(EHitRateProcessImportBatch.db, "HitRateProcessImportBatchID", "HitRateProcessImportBatchRemark", "HitRateProcessImportBatchRemark");

        public static EHitRateProcessImportBatch GetObject(DatabaseConnection dbConn, int ID)
        {
            if (ID > 0)
            {
                EHitRateProcessImportBatch obj = new EHitRateProcessImportBatch();
                obj.HitRateProcessImportBatchID = (int)ID;
                if (db.select(dbConn, obj))
                    return obj;
            }
            return null;
        }

        protected int m_HitRateProcessImportBatchID;
        [DBField("HitRateProcessImportBatchID", true, true), TextSearch, Export(false)]
        public int HitRateProcessImportBatchID
        {
            get { return m_HitRateProcessImportBatchID; }
            set { m_HitRateProcessImportBatchID = value; modify("HitRateProcessImportBatchID"); }
        }

        protected DateTime m_HitRateProcessImportBatchDateTime;
        [DBField("HitRateProcessImportBatchDateTime", "yyyy-MM-dd HH:mm:ss"), TextSearch, Export(false)]
        public DateTime HitRateProcessImportBatchDateTime
        {
            get { return m_HitRateProcessImportBatchDateTime; }
            set { m_HitRateProcessImportBatchDateTime = value; modify("HitRateProcessImportBatchDateTime"); }
        }

        protected int m_HitRateProcessImportBatchUploadedBy;
        [DBField("HitRateProcessImportBatchUploadedBy"), TextSearch, Export(false)]
        public int HitRateProcessImportBatchUploadedBy
        {
            get { return m_HitRateProcessImportBatchUploadedBy; }
            set { m_HitRateProcessImportBatchUploadedBy = value; modify("HitRateProcessImportBatchUploadedBy"); }
        }

        protected string m_HitRateProcessImportBatchRemark;
        [DBField("HitRateProcessImportBatchRemark"), TextSearch, Export(false)]
        public string HitRateProcessImportBatchRemark
        {
            get { return m_HitRateProcessImportBatchRemark; }
            set { m_HitRateProcessImportBatchRemark = value; modify("HitRateProcessImportBatchRemark"); }
        }
    }
}