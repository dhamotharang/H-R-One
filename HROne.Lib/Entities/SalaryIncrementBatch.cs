using System;
using System.Collections.Generic;
using System.Text;
using System;
using System.Collections.Generic;
using System.Text;
using HROne.DataAccess;
////using perspectivemind.validation;
using System.Collections;
using HROne.Common;

namespace HROne.Lib.Entities
{
    [DBClass("PS_SalaryIncrementBatch")]
    public class ESalaryIncrementBatch : BaseObjectWithRecordInfo
    {
        public const string STATUS_OPEN = "O";
        public const string STATUS_CONFIRMED = "C";
        public const string STATUS_APPLIED = "A";
        public const string STATUS_OPEN_DESC = "Open";
        public const string STATUS_CONFIRMED_DESC = "Confirmed";
        public const string STATUS_APPLIED_DESC = "Applied";

        public static WFValueList VLStatusDesc = new AppUtils.NewWFTextList(new string[] { STATUS_APPLIED, STATUS_CONFIRMED, STATUS_OPEN, "" }, new string[] { STATUS_APPLIED_DESC, STATUS_CONFIRMED_DESC, STATUS_OPEN_DESC, STATUS_OPEN_DESC });

        public static DBManager db = new DBManagerWithRecordInfo(typeof(ESalaryIncrementBatch));

        public static ESalaryIncrementBatch GetObject(DatabaseConnection dbConn, int ID)
        {
            if (ID > 0)
            {
                ESalaryIncrementBatch obj = new ESalaryIncrementBatch();
                obj.BatchID = (int)ID;
                if (db.select(dbConn, obj))
                    return obj;
            }
            return null;
        }

        protected int m_BatchID;
        [DBField("BatchID", true, true), TextSearch, Export(false)]
        public int BatchID
        {
            get { return m_BatchID; }
            set { m_BatchID = value; modify("BatchID"); }
        }

        protected DateTime m_AsAtDate;
        [DBField("AsAtDate", "yyyy-MM-dd"), TextSearch, Export(true)]
        public DateTime AsAtDate
        {
            get { return m_AsAtDate; }
            set { m_AsAtDate = value; modify("AsAtDate"); }
        }

        protected bool m_DeferredBatch;
        [DBField("DeferredBatch")]
        public bool DeferredBatch
        {
            get { return m_DeferredBatch; }
            set { m_DeferredBatch = value; modify("DeferredBatch"); }
        }

        protected int m_PaymentCodeID;
        [DBField("PaymentCodeID")]
        public int PaymentCodeID
        {
            get { return m_PaymentCodeID; }
            set { m_PaymentCodeID = value; modify("PaymentCodeID"); }
        }

        protected DateTime m_PaymentDate;
        [DBField("PaymentDate")]
        public DateTime PaymentDate
        {
            get { return m_PaymentDate; }
            set { m_PaymentDate = value; modify("PaymentDate"); }
        }
 
        protected string m_status;
        [DBField("Status")]
        public string Status
        {
            get { return m_status; }
            set { m_status = value; modify("Status"); }
        }

        protected DateTime m_UploadDateTime;
        [DBField("UploadDateTime", "yyyy-MM-dd HH:mm:ss"), TextSearch, Export(true)]
        public DateTime UploadDateTime
        {
            get { return m_UploadDateTime; }
            set { m_UploadDateTime = value; modify("UploadDateTime"); }
        }

        protected int m_UploadBy;
        [DBField("UploadBy")]
        public int UploadBy
        {
            get { return m_UploadBy; }
            set { m_UploadBy = value; modify("UploadBy"); }
        }

        protected DateTime m_ConfirmDateTime;
        [DBField("ConfirmDateTime", "yyyy-MM-dd HH:mm:ss"), TextSearch, Export(true)]
        public DateTime ConfirmDateTime
        {
            get { return m_ConfirmDateTime; }
            set { m_ConfirmDateTime = value; modify("ConfirmDateTime"); }
        }

        protected int m_ConfirmBy;
        [DBField("ConfirmBy")]
        public int ConfirmBy
        {
            get { return m_ConfirmBy; }
            set { m_ConfirmBy = value; modify("ConfirmBy"); }
        }
    }
}












