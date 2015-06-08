using System;
using System.Data;
using System.Configuration;
using HROne.DataAccess;

namespace HROne.Lib.Entities
{
    [DBClass("AttendancePreparationImportBatch")]
    public
        class EAttendancePreparationImportBatch : BaseObject
    {
        public static DBManager db = new DBManager(typeof(EAttendancePreparationImportBatch));
        public static WFValueList VLBatchID = new WFDBList(EAttendancePreparationImportBatch.db, "AttendancePreparationImportBatchID", "AttendancePreparationImportBatchRemark", "AttendancePreparationImportBatchRemark");

        public static EAttendancePreparationImportBatch GetObject(DatabaseConnection dbConn, int ID)
        {
            if (ID > 0)
            {
                EAttendancePreparationImportBatch obj = new EAttendancePreparationImportBatch();
                obj.AttendancePreparationImportBatchID = (int)ID;
                if (db.select(dbConn, obj))
                    return obj;
            }
            return null;
        }

        protected int m_AttendancePreparationImportBatchID;
        [DBField("AttendancePreparationImportBatchID", true, true), TextSearch, Export(false)]
        public int AttendancePreparationImportBatchID
        {
            get { return m_AttendancePreparationImportBatchID; }
            set { m_AttendancePreparationImportBatchID = value; modify("AttendancePreparationImportBatchID"); }
        }

        protected DateTime m_AttendancePreparationImportBatchDateTime;
        [DBField("AttendancePreparationImportBatchDateTime", "yyyy-MM-dd HH:mm:ss"), TextSearch, Export(false)]
        public DateTime AttendancePreparationImportBatchDateTime
        {
            get { return m_AttendancePreparationImportBatchDateTime; }
            set { m_AttendancePreparationImportBatchDateTime = value; modify("AttendancePreparationImportBatchDateTime"); }
        }

        protected int m_AttendancePreparationImportBatchUploadedBy;
        [DBField("AttendancePreparationImportBatchUploadedBy"), TextSearch, Export(false)]
        public int AttendancePreparationImportBatchUploadedBy
        {
            get { return m_AttendancePreparationImportBatchUploadedBy; }
            set { m_AttendancePreparationImportBatchUploadedBy = value; modify("AttendancePreparationImportBatchUploadedBy"); }
        }

        protected string m_AttendancePreparationImportBatchRemark;
        [DBField("AttendancePreparationImportBatchRemark"), TextSearch, Export(false)]
        public string AttendancePreparationImportBatchRemark
        {
            get { return m_AttendancePreparationImportBatchRemark; }
            set { m_AttendancePreparationImportBatchRemark = value; modify("AttendancePreparationImportBatchRemark"); }
        }
    }
}