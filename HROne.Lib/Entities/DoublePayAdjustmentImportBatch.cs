using System;
using System.Data;
using System.Configuration;
using HROne.DataAccess;
////using perspectivemind.validation;


namespace HROne.Lib.Entities
{
    [DBClass("DoublePayAdjustmentImportBatch")]
    public 
        class EDoublePayAdjustmentImportBatch : BaseObject
    {
        public static DBManager db = new DBManager(typeof(EDoublePayAdjustmentImportBatch));
        public static WFValueList VLBatchID = new WFDBList(EDoublePayAdjustmentImportBatch.db, "DoublePayAdjustImportBatchID", "DoublePayAdjustImportBatchRemark", "DoublePayAdjustImportBatchRemark");

        public static EDoublePayAdjustmentImportBatch GetObject(DatabaseConnection dbConn, int ID)
        {
            if (ID > 0)
            {
                EDoublePayAdjustmentImportBatch obj = new EDoublePayAdjustmentImportBatch();
                obj.DoublePayAdjustImportBatchID = (int)ID;
                if (db.select(dbConn, obj))
                    return obj;
            }
            return null;
        }

        protected int m_DoublePayAdjustImportBatchID;
        [DBField("DoublePayAdjustImportBatchID", true, true), TextSearch, Export(false)]
        public int DoublePayAdjustImportBatchID
        {
            get { return m_DoublePayAdjustImportBatchID; }
            set { m_DoublePayAdjustImportBatchID = value; modify("DoublePayAdjustImportBatchID"); }
        }

        protected DateTime m_DoublePayAdjustImportBatchDateTime;
        [DBField("DoublePayAdjustImportBatchDateTime", "yyyy-MM-dd HH:mm:ss"), TextSearch, Export(false)]
        public DateTime DoublePayAdjustImportBatchDateTime
        {
            get { return m_DoublePayAdjustImportBatchDateTime; }
            set { m_DoublePayAdjustImportBatchDateTime = value; modify("DoublePayAdjustImportBatchDateTime"); }
        }

        protected int m_DoublePayAdjustImportBatchUploadedBy;
        [DBField("DoublePayAdjustImportBatchUploadedBy"), TextSearch, Export(false)]
        public int DoublePayAdjustImportBatchUploadedBy
        {
            get { return m_DoublePayAdjustImportBatchUploadedBy; }
            set { m_DoublePayAdjustImportBatchUploadedBy = value; modify("DoublePayAdjustImportBatchUploadedBy"); }
        }

        protected string m_DoublePayAdjustImportBatchRemark;
        [DBField("DoublePayAdjustImportBatchRemark"), TextSearch, Export(false)]
        public string DoublePayAdjustImportBatchRemark
        {
            get { return m_DoublePayAdjustImportBatchRemark; }
            set { m_DoublePayAdjustImportBatchRemark = value; modify("DoublePayAdjustImportBatchRemark"); }
        }
    }
}