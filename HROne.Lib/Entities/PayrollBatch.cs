using System;
using System.Data;
using System.Configuration;
using HROne.DataAccess;
////using perspectivemind.validation;

namespace HROne.Lib.Entities
{
    [DBClass("PayrollBatch")]

    public class EPayrollBatch : BaseObject 
    {
        public static DBManager db = new DBManager(typeof(EPayrollBatch));

        protected int m_PayBatchID;
        [DBField("PayBatchID", true, true), TextSearch, Export(false)]
        public int PayBatchID
        {
            get { return m_PayBatchID; }
            set { m_PayBatchID = value; modify("PayBatchID"); }
        }

        protected DateTime m_PayBatchConfirmDate;
        [DBField("PayBatchConfirmDate"), TextSearch, Export(false)]
        public DateTime PayBatchConfirmDate
        {
            get { return m_PayBatchConfirmDate; }
            set { m_PayBatchConfirmDate = value; modify("PayBatchConfirmDate"); }
        }

        protected DateTime m_PayBatchValueDate;
        [DBField("PayBatchValueDate"), TextSearch, Export(false)]
        public DateTime PayBatchValueDate
        {
            get { return m_PayBatchValueDate; }
            set { m_PayBatchValueDate = value; modify("PayBatchValueDate"); }
        }

        protected DateTime m_PayBatchFileGenDate;
        [DBField("PayBatchFileGenDate"), TextSearch, Export(false)]
        public DateTime PayBatchFileGenDate
        {
            get { return m_PayBatchFileGenDate; }
            set { m_PayBatchFileGenDate = value; modify("PayBatchFileGenDate"); }
        }

        protected int m_PayBatchFileGenBy;
        [DBField("PayBatchFileGenBy"), TextSearch, Export(false)]
        public int PayBatchFileGenBy
        {
            get { return m_PayBatchFileGenBy; }
            set { m_PayBatchFileGenBy = value; modify("PayBatchFileGenBy"); }
        }

        protected bool m_PayBatchIsESSPaySlipRelease;
        [DBField("PayBatchIsESSPaySlipRelease"), TextSearch, Export(false)]
        public bool PayBatchIsESSPaySlipRelease
        {
            get { return m_PayBatchIsESSPaySlipRelease; }
            set { m_PayBatchIsESSPaySlipRelease = value; modify("PayBatchIsESSPaySlipRelease"); }
        }

        protected string m_PayBatchRemark;
        [DBField("PayBatchRemark"), TextSearch, Export(false)]
        public string PayBatchRemark
        {
            get { return m_PayBatchRemark; }
            set { m_PayBatchRemark = value; modify("PayBatchRemark"); }
        }
    }
}
