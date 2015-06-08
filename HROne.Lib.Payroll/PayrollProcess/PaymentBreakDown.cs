using System;
using System.Collections.Generic;
using System.Text;
using HROne.Lib.Entities;

namespace HROne.Payroll
{
    public class PaymentBreakDown
    {
        public PaymentBreakDown(PaymentBreakDownKey key, double unit, double DayAdjusts)
            : this(key, unit, DayAdjusts, null)
        {
        }
        public PaymentBreakDown(PaymentBreakDownKey key, double unit, double DayAdjusts, object relatedObject)
        {
            this.key = key;
            this.unit = unit;
            this.DayAdjusts = DayAdjusts;
            if (relatedObject != null)
                relatedObjectList.Add(relatedObject);
        }
        public PaymentBreakDownKey key;
        public double unit = 0;
        public double DayAdjusts = 0;
        public List<object> relatedObjectList = new List<object>();
    }

    public class PaymentBreakDownCollection : Dictionary<PaymentBreakDownKey, PaymentBreakDown> 
    {
        public void AddRange(PaymentBreakDownCollection breakDownCollection)
        {
            foreach (PaymentBreakDownKey key in breakDownCollection.Keys)
            {
                PaymentBreakDown breakDown= breakDownCollection[key];
                this.AddUnit(key, breakDown.unit, breakDown.DayAdjusts, breakDown.relatedObjectList);
            }
        }

        public string DefaultPayMethod = string.Empty;
        public PaymentBreakDown AddUnit(PaymentBreakDownKey key, double unitAdjust, double dayAdjusts)
        {
            return AddUnit(key, unitAdjust, dayAdjusts, null);
        }

        public PaymentBreakDown AddUnit(PaymentBreakDownKey key, double unitAdjust, double dayAdjusts, object relatedObject)
        {
            if (this.ContainsKey(key))
            {
                PaymentBreakDown breakDown = this[key];
                breakDown.unit += unitAdjust;
                breakDown.DayAdjusts += dayAdjusts;
                breakDown.relatedObjectList.Add(relatedObject);
                return breakDown;
            }
            else
            {
                PaymentBreakDown breakDown = new PaymentBreakDown(key.ShadowCopy(), unitAdjust, dayAdjusts, relatedObject);
                this.Add(breakDown.key, breakDown);
                return breakDown;
            }
        }
        public List<EPaymentRecord> GeneratePaymentRecordList()
        {
            List<EPaymentRecord> list = new List<EPaymentRecord>();
            foreach (PaymentBreakDown breakDown in this.Values)
            {
                EPaymentRecord paymentRecord = new EPaymentRecord();
                paymentRecord.CostCenterID = breakDown.key.CostCenterID;
                paymentRecord.CurrencyID = breakDown.key.CurrencyID;
                paymentRecord.EmpAccID = breakDown.key.EmpAccID;
                paymentRecord.PaymentCodeID = breakDown.key.PaymentCodeID;
                paymentRecord.PayRecCalAmount = breakDown.key.Rate * breakDown.unit * breakDown.key.RateMultiplier;
                paymentRecord.PayRecActAmount = HROne.CommonLib.GenericRoundingFunctions.RoundingTo(paymentRecord.PayRecCalAmount, HROne.Lib.ExchangeCurrency.DefaultCurrencyDecimalPlaces(), 9);
                paymentRecord.PayRecIsRestDayPayment = breakDown.key.IsRestDayPayment;
                paymentRecord.PayRecMethod = string.IsNullOrEmpty(breakDown.key.PaymentMethod) ? DefaultPayMethod : breakDown.key.PaymentMethod;
                paymentRecord.PayRecNumOfDayAdj = breakDown.DayAdjusts;
                paymentRecord.PayRecStatus = "A";
                if (breakDown.key.IsGenerateRemark)
                    paymentRecord.PayRecRemark = (string.IsNullOrEmpty(breakDown.key.RateRemark) ? breakDown.key.Rate.ToString("#,##0.00######") : breakDown.key.RateRemark) + " x " + breakDown.unit.ToString() + (breakDown.key.RateMultiplier.Equals(1.0) ? string.Empty : " x " + breakDown.key.RateMultiplier.ToString());
                if (breakDown.relatedObjectList.Count > 0)
                    paymentRecord.RelatedObject = breakDown.relatedObjectList;
                list.Add(paymentRecord);
            }
            return list;
        }
    }
    public class PaymentBreakDownKey
    {
        protected int m_PaymentCodeID = 0;
        protected double m_Rate = 0;
        protected string m_RateRemark = string.Empty;
        protected double m_RateMultiplier = 1;
        protected int m_CostCenterID = 0;
        protected bool m_IsRestDayPayment = false;
        public string CurrencyID = HROne.Lib.ExchangeCurrency.DefaultCurrency();
        protected string m_PaymentMethod = string.Empty;
        protected int m_EmpAccID = 0;
        protected bool m_IsGenerateRemark;
        //public PaymentBreakDownKey(int PaymentCodeID, double Rate)
        //{
        //    this.m_PaymentCodeID = PaymentCodeID;
        //    this.m_Rate = Rate;
        //    this.m_CostCenterID = 0;
        //}
        public PaymentBreakDownKey(int PaymentCodeID, double Rate, string PaymentMethod, int EmpAccID, bool IsRestDayPayment, int CostCenterID, bool IsGenerateRemark)
            : this(PaymentCodeID, Rate, string.Empty, 1, PaymentMethod, EmpAccID, IsRestDayPayment, CostCenterID, IsGenerateRemark)
        {
        }

        public PaymentBreakDownKey(int PaymentCodeID, double Rate, string RateRemark, double RateMultiplier, string PaymentMethod, int EmpAccID, bool IsRestDayPayment, int CostCenterID, bool IsGenerateRemark)
        {
            this.m_PaymentCodeID = PaymentCodeID;
            this.m_Rate = Rate;
            this.m_RateRemark = RateRemark;
            this.m_RateMultiplier = RateMultiplier;
            this.m_CostCenterID = CostCenterID;
            this.m_IsRestDayPayment = IsRestDayPayment;
            this.m_PaymentMethod = PaymentMethod;
            this.m_EmpAccID = EmpAccID;
            this.m_IsGenerateRemark = IsGenerateRemark;
        }

        public int PaymentCodeID
        {
            get { return m_PaymentCodeID; }
        }
        public double Rate
        {
            get { return m_Rate; }
        }
        public string RateRemark
        {
            get { return m_RateRemark; }
        }
        public double RateMultiplier
        {
            get { return m_RateMultiplier; }
        }
        public string PaymentMethod
        {
            get { return m_PaymentMethod; }
        }
        public int EmpAccID
        {
            get { return m_EmpAccID; }
        }
        public int CostCenterID
        {
            get { return m_CostCenterID; }
        }
        public bool IsRestDayPayment
        {
            get { return m_IsRestDayPayment; }
        }

        public bool IsGenerateRemark
        {
            get { return m_IsGenerateRemark; }
        }

        public override bool Equals(object obj)
        {
            if (obj is PaymentBreakDownKey)
            {
                PaymentBreakDownKey compare = (PaymentBreakDownKey)obj;
                if (compare.m_PaymentCodeID.Equals(m_PaymentCodeID)
                    && compare.m_CostCenterID.Equals(m_CostCenterID)
                    && Math.Abs(compare.m_Rate - m_Rate) < 0.005
                    && compare.m_RateRemark.Equals(m_RateRemark)
                    && compare.m_RateMultiplier.Equals(m_RateMultiplier)
                    && compare.PaymentMethod.Equals(m_PaymentMethod)
                    && compare.EmpAccID.Equals(m_EmpAccID)
                    && compare.IsRestDayPayment.Equals(m_IsRestDayPayment)
                    && compare.IsGenerateRemark.Equals(m_IsGenerateRemark)
                    )
                    return true;
            }
            return false;
        }
        public override int GetHashCode()
        {
            return 0;
            //                return base.GetHashCode();
        }


        public PaymentBreakDownKey ShadowCopy()
        {
            return (PaymentBreakDownKey)this.MemberwiseClone();
        }

    }

}
