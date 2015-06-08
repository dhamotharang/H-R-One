using System;
using System.Collections.Generic;
using System.Text;
using HROne.DataAccess;
////using perspectivemind.validation;
using System.Collections;
using HROne.Common;

namespace HROne.Lib.Entities
{
    [DBClass("PayScaleMap")]
    public class EPayScaleMap : BaseObjectWithRecordInfo
    {
        public static DBManager db = new DBManagerWithRecordInfo(typeof(EPayScaleMap));
        public static WFValueList VLPayScale = new AppUtils.EncryptedDBCodeList(EPayScale.db, "PayScaleMapID", new string[] { "Point", "Salary" }, " - ", "Point");
        // public static WFValueList VLPayGroupFreq = new AppUtils.NewWFTextList(new string[] { "M", "S" }, new string[] { "Monthly", "Semi-Monthly" });
        public static WFValueList VLDistinctSchemeList = new AppUtils.DistinctEncryptedDBCodeList(EPayScaleMap.db, "SchemeCode");

        public static EPayScaleMap GetObject(DatabaseConnection dbConn, object ID)
        {
            if (ID is int)
            {
                EPayScaleMap obj = new EPayScaleMap();
                obj.PayScaleMapID = (int)ID;
                if (db.select(dbConn, obj))
                    return obj;
            }
            return null;
        }
        protected int m_PayScaleMapID;
        [DBField("PayScaleMapID", true, true), TextSearch, Export(false)]
        public int PayScaleMapID
        {
            get { return m_PayScaleMapID; }
            set { m_PayScaleMapID = value; modify("PayScaleMapID");  }
        }

        protected DateTime m_EffectiveDate;
        [DBField("EffectiveDate", "yyyy-MM-dd"), TextSearch, Export(false), Required]
        public DateTime EffectiveDate
        {
            get { return m_EffectiveDate; }
            set { m_EffectiveDate = value; modify("EffectiveDate"); }
        }

        protected DateTime m_ExpiryDate;
        [DBField("ExpiryDate", "yyyy-MM-dd"), TextSearch, Export(false)]
        public DateTime ExpiryDate
        {
            get { return m_ExpiryDate; }
            set { m_ExpiryDate = value; modify("ExpiryDate"); }
        }

        protected string m_SchemeCode;
        [DBField("SchemeCode"), MaxLength(10, 10), TextSearch, DBAESEncryptStringField, Export(true), Required]
        public String SchemeCode
        {
            get { return m_SchemeCode; }
            set { m_SchemeCode = value; modify("SchemeCode"); }
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
    }

    public class PayScaleMapComparer : IComparer
    {
        public int Compare(object x, object y)
        {
            EPayScaleMap left = (EPayScaleMap)x;
            EPayScaleMap right = (EPayScaleMap)y;

            if (left.SchemeCode.Equals(right.SchemeCode))
            {
                return left.Point.CompareTo(right.Point);
            }
            return left.SchemeCode.CompareTo(right.SchemeCode);
        }
    }
}
