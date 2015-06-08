using System;
using System.Collections.Generic;
using System.Text;
using HROne.DataAccess;
////using perspectivemind.validation;
using System.Collections;
using HROne.Common;

namespace HROne.Lib.Entities
{
    [DBClass("PayScale")]
    public class EPayScale : BaseObjectWithRecordInfo
    {
        public static DBManager db = new DBManagerWithRecordInfo(typeof(EPayScale));
        public static WFValueList VLPayScale = new AppUtils.EncryptedDBCodeList(EPayScale.db, "PayScaleID", new string[] { "PayScaleDesc" }, " - ", "PayScaleID");
//        public static WFValueList VLDistinctSchemeList = new AppUtils.WFDBDistinctList(EPayScale.db, "SchemeCode", "SchemeCode AS SchemeCode2", "SchemeCode desc");
        public static WFValueList VLDistinctSchemeList = new AppUtils.DistinctEncryptedDBCodeList(EPayScale.db, "SchemeCode");

        public static EPayScale GetObject(DatabaseConnection dbConn, object ID)
        {
            if (ID is int)
            {
                EPayScale obj = new EPayScale();
                obj.PayScaleID = (int)ID;
                if (db.select(dbConn, obj))
                    return obj;
            }
            return null;
        }
        protected int m_PayScaleID;
        [DBField("PayScaleID", true, true), TextSearch, Export(false)]
        public int PayScaleID
        {
            get { return m_PayScaleID; }
            set { m_PayScaleID = value; modify("PayScaleID"); }
        }

        protected string m_SchemeCode;
        [DBField("SchemeCode"), MaxLength(10, 10), TextSearch, DBAESEncryptStringField, Export(true), Required]
        public String SchemeCode
        {
            get { return m_SchemeCode; }
            set { m_SchemeCode = value; modify("SchemeCode"); }
        }

        protected String m_Capacity;
        [DBField("Capacity"), MaxLength(70, 60), TextSearch, DBAESEncryptStringField, Export(true), Required]
        public String Capacity
        {
            get { return m_Capacity; }
            set { m_Capacity = value; modify("Capacity"); }
        }

        protected Decimal m_FirstPoint;
        [DBField("FirstPoint", "0.00"), MaxLength(5), Export(true), Required]
        public Decimal FirstPoint
        {
            get { return m_FirstPoint; }
            set { m_FirstPoint = value; modify("FirstPoint"); }
        }

        protected Decimal m_MidPoint;
        [DBField("MidPoint", "0.00"), MaxLength(5), Export(true), Required]
        public Decimal MidPoint
        {
            get { return m_MidPoint; }
            set { m_MidPoint = value; modify("MidPoint"); }
        }

        protected Decimal m_LastPoint;
        [DBField("LastPoint", "0.00"), MaxLength(5), Export(true), Required]
        public Decimal LastPoint
        {
            get { return m_LastPoint; }
            set { m_LastPoint = value; modify("LastPoint"); }
        }
    }

    public class PayScaleComparer : IComparer
    {
        public int Compare(object x, object y)
        {
            EPayScale left = (EPayScale)x;
            EPayScale right = (EPayScale)y;

            if (left.SchemeCode.Equals(right.SchemeCode))
            {
                return left.Capacity.CompareTo(right.Capacity);
            }

            return left.SchemeCode.CompareTo(right.SchemeCode);
        }
    }
}
