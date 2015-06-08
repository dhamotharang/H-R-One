using System;
using System.Collections;
using System.Data;
using System.Configuration;
using HROne.DataAccess;
////using perspectivemind.validation;

namespace HROne.Lib.Entities
{

    public class PaymentRecordCompareByAmount : IComparer 
    {
        bool m_ascending = true;
        public PaymentRecordCompareByAmount(bool ascending)
        {
            m_ascending = ascending;
        }

        int IComparer.Compare(object x, object y)
        {
            EPaymentRecord paymentRecordX = (EPaymentRecord)x;
            EPaymentRecord paymentRecordY = (EPaymentRecord)y;
            if (x == null && y == null)
            {
                return 0;
            }
            else if (x == null && y != null)
            {
                return (m_ascending) ? -1 : 1;
            }
            else if (x != null && y == null)
            {
                return (m_ascending) ? 1 : -1;
            }
            else if (paymentRecordX.PayRecMethod.Equals("A") && paymentRecordX.PayRecActAmount > 0 && !paymentRecordY.PayRecMethod.Equals("A"))
            {
                return (m_ascending) ? 1 : -1;
            }
            else if (paymentRecordY.PayRecMethod.Equals("A") && paymentRecordY.PayRecActAmount > 0 && !paymentRecordX.PayRecMethod.Equals("A"))
            {
                return (m_ascending) ? -1 : 1;
            }
            else
            {
                return (m_ascending) ?
                    paymentRecordX.PayRecActAmount.CompareTo(paymentRecordY.PayRecActAmount) :
                   paymentRecordY.PayRecActAmount.CompareTo(paymentRecordX.PayRecActAmount);
            }
        }
    }
    public abstract class PaymentRecordType
    {
        public const string PAYRECORDTYPE_RECURRING = "R";
        public const string PAYRECORDTYPE_YEB = "Y";
        public const string PAYRECORDTYPE_CND = "C";
        public const string PAYRECORDTYPE_FINALPAYMENT = "F";
        public const string PAYRECORDTYPE_PENSION = "P";
        public const string PAYRECORDTYPE_TRIALRUN_ADJUSTMENT = "T";
        public const string PAYRECORDTYPE_CONFIRM_ADJUSTMENT = "A";
    }

    public abstract class PaymentRecordStatus
    {
        public const string PAYRECORDSTATUS_ACTIVE = "A";
    }

    [DBClass("PaymentRecord")]
    public class EPaymentRecord : BaseObject
    {

        
        public static DBManager db = new DBManager(typeof(EPaymentRecord));

        protected int m_PayRecID;
        [DBField("PayRecID", true, true), TextSearch, Export(false)]
        public int PayRecID
        {
            get { return m_PayRecID; }
            set { m_PayRecID = value; modify("PayRecID"); }
        }

        protected int m_EmpPayrollID;
        [DBField("EmpPayrollID"), TextSearch, Export(false), Required]
        public int EmpPayrollID
        {
            get { return m_EmpPayrollID; }
            set { m_EmpPayrollID = value; modify("EmpPayrollID"); }
        }

        protected int m_PaymentCodeID;
        [DBField("PaymentCodeID"), TextSearch, Export(false), Required]
        public int PaymentCodeID
        {
            get { return m_PaymentCodeID; }
            set { m_PaymentCodeID = value; modify("PaymentCodeID"); }
        }

        protected double m_PayRecCalAmount;
        [DBField("PayRecCalAmount", "0.00"), TextSearch, Export(false), Required]
        public double PayRecCalAmount
        {
            get { return m_PayRecCalAmount; }
            set { m_PayRecCalAmount = value; modify("PayRecCalAmount");}
        }

        protected double m_PayRecActAmount;
        [DBField("PayRecActAmount","0.00"), TextSearch, Export(false), Required]
        public double PayRecActAmount
        {
            get { return m_PayRecActAmount; }
            set { m_PayRecActAmount = value; modify("PayRecActAmount"); }
        }

        //protected string m_PayRecCalAmountEnc;
        //[DBField("PayRecCalAmountEnc"), DBAESEncryptStringField, TextSearch, Export(false), Required]
        //public string PayRecCalAmountEnc
        //{
        //    get { return m_PayRecCalAmountEnc; }
        //    set { m_PayRecCalAmountEnc = value; modify("PayRecCalAmountEnc"); }
        //}

        //protected string m_PayRecActAmountEnc;
        //[DBField("PayRecActAmountEnc", "0.00"), DBAESEncryptStringField, TextSearch, Export(false), Required]
        //public string PayRecActAmountEnc
        //{
        //    get { return m_PayRecActAmountEnc; }
        //    set { m_PayRecActAmountEnc = value; modify("PayRecActAmountEnc"); }
        //}

        protected string m_CurrencyID;
        [DBField("CurrencyID"), TextSearch, Export(false), Required]
        public string CurrencyID
        {
            get { return m_CurrencyID; }
            set { m_CurrencyID = value; modify("CurrencyID"); }
        }

        protected string m_PayRecMethod;
        [DBField("PayRecMethod"), TextSearch, Export(false), Required]
        public string PayRecMethod
        {
            get { return m_PayRecMethod; }
            set { m_PayRecMethod = value; modify("PayRecMethod"); }
        }

        protected int m_EmpAccID;
        [DBField("EmpAccID"), TextSearch, Export(false)]
        public int EmpAccID
        {
            get { return m_EmpAccID; }
            set { m_EmpAccID = value; modify("EmpAccID"); }
        }
        protected int m_CostCenterID;
        [DBField("CostCenterID"), TextSearch, Export(false)]
        public int CostCenterID
        {
            get { return m_CostCenterID; }
            set { m_CostCenterID = value; modify("CostCenterID"); }
        }

        /*
         * R - Recurring Payment
         * C - Claims and Deduction
         * T - Trial Run Adjust
         * A - Confirm Adjust
         */
        protected string m_PayRecType;
        [DBField("PayRecType"), TextSearch, Export(false), Required]
        public string PayRecType
        {
            get { return m_PayRecType; }
            set { m_PayRecType = value; modify("PayRecType"); }
        }

        protected string m_PayRecStatus;
        [DBField("PayRecStatus"), TextSearch, Export(false), Required]
        public string PayRecStatus
        {
            get { return m_PayRecStatus; }
            set { m_PayRecStatus = value; modify("PayRecStatus"); }
        }

        protected bool m_PayRecIsRestDayPayment;
        [DBField("PayRecIsRestDayPayment"), TextSearch, Export(false)]
        public bool PayRecIsRestDayPayment
        {
            get { return m_PayRecIsRestDayPayment; }
            set { m_PayRecIsRestDayPayment = value; modify("PayRecIsRestDayPayment"); }
        }

        protected double m_PayRecNumOfDayAdj;
        [DBField("PayRecNumOfDayAdj", "0.##"), MaxLength(5), TextSearch, Export(false)]
        public double PayRecNumOfDayAdj
        {
            get { return m_PayRecNumOfDayAdj; }
            set { m_PayRecNumOfDayAdj = value; modify("PayRecNumOfDayAdj"); }
        }

        protected int m_LeaveAppID;
        [DBField("LeaveAppID"), TextSearch, Export(false)]
        public int LeaveAppID
        {
            get { return m_LeaveAppID; }
            set { m_LeaveAppID = value; modify("LeaveAppID"); }
        }

        protected string m_LeaveAppIDList;
        [DBField("LeaveAppIDList"), TextSearch, Export(false)]
        public string LeaveAppIDList
        {
            get { return m_LeaveAppIDList; }
            set { m_LeaveAppIDList = value; modify("LeaveAppIDList"); }
        }
        
        protected String m_PayRecRemark;
        [DBField("PayRecRemark"), TextSearch, Export(false)]
        public String PayRecRemark
        {
            get { return m_PayRecRemark; }
            set { m_PayRecRemark = value; modify("PayRecRemark"); }
        }

        protected int m_EmpRPIDforBP;
        [DBField("EmpRPIDforBP"), TextSearch, Export(false)]
        public int EmpRPIDforBP
        {
            get { return m_EmpRPIDforBP; }
            set { m_EmpRPIDforBP = value; modify("EmpRPIDforBP"); }
        }

        public object RelatedObject;

    }


}