using System;
using System.Data;
using System.Configuration;
using HROne.DataAccess;
////using perspectivemind.validation;

namespace HROne.Lib.Entities
{
    [DBClass("ClaimsAndDeductionsImportBatch")]

    public class EClaimsAndDeductionsImportBatch : BaseObject
    {
        public static DBManager db = new DBManager(typeof(EClaimsAndDeductionsImportBatch));

        protected int m_CNDImportBatchID;
        [DBField("CNDImportBatchID", true, true), TextSearch, Export(false)]
        public int CNDImportBatchID
        {
            get { return m_CNDImportBatchID; }
            set { m_CNDImportBatchID = value; modify("CNDImportBatchID"); }
        }

        protected DateTime m_CNDImportBatchDateTime;
        [DBField("CNDImportBatchDateTime","yyyy-MM-dd HH:mm:ss"), TextSearch, Export(false)]
        public DateTime CNDImportBatchDateTime
        {
            get { return m_CNDImportBatchDateTime; }
            set { m_CNDImportBatchDateTime = value; modify("CNDImportBatchDateTime"); }
        }

        protected int m_CNDImportBatchUploadedBy;
        [DBField("CNDImportBatchUploadedBy"), TextSearch, Export(false)]
        public int CNDImportBatchUploadedBy
        {
            get { return m_CNDImportBatchUploadedBy; }
            set { m_CNDImportBatchUploadedBy = value; modify("CNDImportBatchUploadedBy"); }
        }

        protected string m_CNDImportBatchRemark;
        [DBField("CNDImportBatchRemark"), TextSearch, Export(false)]
        public string CNDImportBatchRemark
        {
            get { return m_CNDImportBatchRemark; }
            set { m_CNDImportBatchRemark = value; modify("CNDImportBatchRemark"); }
        }

        
    }
}