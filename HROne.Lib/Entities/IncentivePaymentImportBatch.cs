using System;
using System.Data;
using System.Configuration;
using HROne.DataAccess;
////using perspectivemind.validation;


namespace HROne.Lib.Entities
{
    [DBClass("IncentivePaymentImportBatch")]
    public 
        class EIncentivePaymentImportBatch : BaseObject
    {
        public static DBManager db = new DBManager(typeof(EIncentivePaymentImportBatch));
        public static WFValueList VLBatchID = new WFDBList(EIncentivePaymentImportBatch.db, "IPImportBatchID", "IPImportBatchRemark", "IPImportBatchRemark");

        public static EIncentivePaymentImportBatch GetObject(DatabaseConnection dbConn, int ID)
        {
            if (ID > 0)
            {
                EIncentivePaymentImportBatch obj = new EIncentivePaymentImportBatch();
                obj.IPImportBatchID = (int)ID;
                if (db.select(dbConn, obj))
                    return obj;
            }
            return null;
        }

        protected int m_IPImportBatchID;
        [DBField("IPImportBatchID", true, true), TextSearch, Export(false)]
        public int IPImportBatchID
        {
            get { return m_IPImportBatchID; }
            set { m_IPImportBatchID = value; modify("IPImportBatchID"); }
        }

        protected DateTime m_IPImportBatchDateTime;
        [DBField("IPImportBatchDateTime", "yyyy-MM-dd HH:mm:ss"), TextSearch, Export(false)]
        public DateTime IPImportBatchDateTime
        {
            get { return m_IPImportBatchDateTime; }
            set { m_IPImportBatchDateTime = value; modify("IPImportBatchDateTime"); }
        }

        protected int m_IPImportBatchUploadedBy;
        [DBField("IPImportBatchUploadedBy"), TextSearch, Export(false)]
        public int IPImportBatchUploadedBy
        {
            get { return m_IPImportBatchUploadedBy; }
            set { m_IPImportBatchUploadedBy = value; modify("IPImportBatchUploadedBy"); }
        }

        protected string m_IPImportBatchRemark;
        [DBField("IPImportBatchRemark"), TextSearch, Export(false)]
        public string IPImportBatchRemark
        {
            get { return m_IPImportBatchRemark; }
            set { m_IPImportBatchRemark = value; modify("IPImportBatchRemark"); }
        }
    }
}