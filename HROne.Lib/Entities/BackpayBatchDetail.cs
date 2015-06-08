using System;
using System.Collections.Generic;
using System.Text;
using HROne.DataAccess;
////using perspectivemind.validation;
using System.Collections;
using HROne.Common;

namespace HROne.Lib.Entities
{
    [DBClass("PS_BackpayBatchDetail")]
    public class EBackpayBatchDetail : BaseObjectWithRecordInfo
    {
        public static DBManager db = new DBManagerWithRecordInfo(typeof(EBackpayBatchDetail));

        public static EBackpayBatchDetail GetObject(DatabaseConnection dbConn, object ID)
        {
            if (ID is int)
            {
                EBackpayBatchDetail obj = new EBackpayBatchDetail();
                obj.DetailID = (int)ID;
                if (db.select(dbConn, obj))
                    return obj;
            }
            return null;
        }
        protected int m_DetailID;
        [DBField("DetailID", true, true), TextSearch, Export(false)]
        public int DetailID
        {
            get { return m_DetailID; }
            set { m_DetailID = value; modify("DetailID"); }
        }

        protected DateTime m_AnnounceDate;
        [DBField("AnnounceDate", "yyyy-MM-dd"), TextSearch, Export(false), Required]
        public DateTime AnnounceDate
        {
            get { return m_AnnounceDate; }
            set { m_AnnounceDate = value; modify("AnnounceDate"); }
        }

        protected DateTime m_EffectiveDate;
        [DBField("EffectiveDate", "yyyy-MM-dd"), TextSearch, Export(false), Required]
        public DateTime EffectiveDate
        {
            get { return m_EffectiveDate; }
            set { m_EffectiveDate = value; modify("EffectiveDate"); }
        }

        protected DateTime m_BackpayDate;
        [DBField("BackpayDate", "yyyy-MM-dd"), TextSearch, Export(false), Required]
        public DateTime BackpayDate
        {
            get { return m_BackpayDate; }
            set { m_BackpayDate = value; modify("BackpayDate"); }
        }

        protected string m_SchemeCode;
        [DBField("SchemeCode"), MaxLength(10, 10), TextSearch, DBAESEncryptStringField, Export(true), Required]
        public String SchemeCode
        {
            get { return m_SchemeCode; }
            set { m_SchemeCode = value.Trim().ToUpper(); modify("SchemeCode"); }
        }

        protected decimal m_Point;
        [DBField("Point", "0.00"), MaxLength(5), Export(false), Required]
        public Decimal Point
        {
            get { return m_Point; }
            set { m_Point = value; modify("Point"); }
        }

        protected Decimal m_Salary;
        [DBField("Salary", "#,##0.00"), MaxLength(15), Export(false), Required]
        public Decimal Salary
        {
            get { return m_Salary; }
            set { m_Salary = value; modify("Salary"); }
        }

        protected Decimal m_CurrentSalary;
        [DBField("CurrentSalary", "#,##0.00"), MaxLength(15), Export(false), Required]
        public Decimal CurrentSalary
        {
            get { return m_CurrentSalary; }
            set { m_CurrentSalary = value; modify("CurrentSalary"); }
        }

        protected string m_PaymentCode;
        [DBField("PaymentCode"), TextSearch, DBAESEncryptStringField, MaxLength(20, 20), Export(false), Required]
        public string PaymentCode
        {
            get { return m_PaymentCode; }
            set { m_PaymentCode = value.Trim().ToUpper(); modify("PaymentCode"); }
        }
    }
}
