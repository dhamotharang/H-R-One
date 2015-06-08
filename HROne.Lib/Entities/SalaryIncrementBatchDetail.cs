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
    [DBClass("PS_SalaryIncrementBatchDetail")]
    public class ESalaryIncrementBatchDetail : BaseObjectWithRecordInfo
    {
        public static DBManager db = new DBManagerWithRecordInfo(typeof(ESalaryIncrementBatchDetail));

        public static ESalaryIncrementBatchDetail GetObject(DatabaseConnection dbConn, int ID)
        {
            if (ID > 0)
            {
                ESalaryIncrementBatchDetail obj = new ESalaryIncrementBatchDetail();
                obj.BatchID = (int)ID;
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

        protected int m_BatchID;
        [DBField("BatchID"), TextSearch, Export(false)]
        public int BatchID
        {
            get { return m_BatchID; }
            set { m_BatchID = value; modify("BatchID"); }
        }

        protected int m_EmpID;
        [DBField("EmpID"), TextSearch, Export(false)]
        public int EmpID
        {
            get { return m_EmpID; }
            set { m_EmpID = value; modify("EmpID"); }
        }

        protected int m_EmpRPID;
        [DBField("EmpRPID"), TextSearch, Export(false)]
        public int EmpRPID
        {
            get { return m_EmpRPID; }
            set { m_EmpRPID = value; modify("EmpRPID"); }
        }

        protected string m_SchemeCode;
        [DBField("SchemeCode"), MaxLength(10, 10), TextSearch, DBAESEncryptStringField, Export(true), Required]
        public string SchemeCode
        {
            get { return m_SchemeCode; }
            set { m_SchemeCode = value; modify("SchemeCode"); }
        }

        protected string m_Capacity;
        [DBField("Capacity"), MaxLength(70, 60), TextSearch, DBAESEncryptStringField, Export(true), Required]
        public string Capacity
        {
            get { return m_Capacity; }
            set { m_Capacity = value; modify("Capacity"); }
        }

        protected decimal m_CurrentPoint;
        [DBField("CurrentPoint", format="0.00"), TextSearch, Export(true)]
        public decimal CurrentPoint
        {
            get { return m_CurrentPoint; }
            set { m_CurrentPoint = value; modify("CurrentPoint"); }
        }

        protected decimal m_NewPoint;
        [DBField("NewPoint", format="0.00"), TextSearch, Export(true)]
        public decimal NewPoint
        {
            get { return m_NewPoint; }
            set { m_NewPoint = value; modify("NewPoint"); }
        }
    }
}












