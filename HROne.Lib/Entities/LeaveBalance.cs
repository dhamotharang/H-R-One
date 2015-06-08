using System;
using System.Collections.Generic;
using System.Text;
using HROne.DataAccess;
////using perspectivemind.validation;

namespace HROne.Lib.Entities
{
    [DBClass("LeaveBalance")]
    public class ELeaveBalance :BaseObject
    {
        public enum LeaveBalanceUnit
        {
            Hour = 0,
            Day = 1
        }

        public static DBManager db = new DBManager(typeof(ELeaveBalance));
        protected int m_LeaveBalanceID;
        [DBField("LeaveBalanceID", true, true), TextSearch, Export(false)]
        public int LeaveBalanceID
        {
            get { return m_LeaveBalanceID; }
            set { m_LeaveBalanceID = value; modify("LeaveBalanceID"); }
        }
        protected int m_EmpID;
        [DBField("EmpID"), TextSearch, Export(false)]
        public int EmpID
        {
            get { return m_EmpID; }
            set { m_EmpID = value; modify("EmpID"); }
        }
        protected int m_LeaveTypeID;
        [DBField("LeaveTypeID"), TextSearch, Export(false)]
        public int LeaveTypeID
        {
            get { return m_LeaveTypeID; }
            set { m_LeaveTypeID = value; modify("LeaveTypeID"); }
        }
        protected DateTime m_LeaveBalanceEffectiveDate;
        [DBField("LeaveBalanceEffectiveDate"), TextSearch, Export(false)]
        public DateTime LeaveBalanceEffectiveDate
        {
            get { return m_LeaveBalanceEffectiveDate; }
            set { m_LeaveBalanceEffectiveDate = value; modify("LeaveBalanceEffectiveDate"); }
        }
        protected double m_LeaveBalanceBF;
        [DBField("LeaveBalanceBF"), TextSearch, Export(false)]
        public double LeaveBalanceBF
        {
            get { return m_LeaveBalanceBF; }
            set { m_LeaveBalanceBF = value; modify("LeaveBalanceBF"); }
        }
        protected double m_LeaveBalanceForfeiture;
        [DBField("LeaveBalanceForfeiture"), TextSearch, Export(false)]
        public double LeaveBalanceForfeiture
        {
            get { return m_LeaveBalanceForfeiture; }
            set { m_LeaveBalanceForfeiture = value; modify("LeaveBalanceForfeiture"); }
        }
        protected double m_LeaveBalanceEntitled;
        [DBField("LeaveBalanceEntitled"), TextSearch, Export(false)]
        public double LeaveBalanceEntitled
        {
            get { return m_LeaveBalanceEntitled; }
            set { m_LeaveBalanceEntitled = value; modify("LeaveBalanceEntitled"); }
        }
        protected int m_LeaveBalancePeriod;
        [DBField("LeaveBalancePeriod"), TextSearch, Export(false)]
        public int LeaveBalancePeriod
        {
            get { return m_LeaveBalancePeriod; }
            set { m_LeaveBalancePeriod = value; modify("LeaveBalancePeriod"); }
        }
        protected bool m_LeaveBalanceIsSettlement;
        [DBField("LeaveBalanceIsSettlement"), TextSearch, Export(false)]
        public bool LeaveBalanceIsSettlement
        {
            get { return m_LeaveBalanceIsSettlement; }
            set { m_LeaveBalanceIsSettlement = value; modify("LeaveBalanceIsSettlement"); }
        }
        protected bool m_LeaveBalanceIsFirst;
        [DBField("LeaveBalanceIsFirst"), TextSearch, Export(false)]
        public bool LeaveBalanceIsFirst
        {
            get { return m_LeaveBalanceIsFirst; }
            set { m_LeaveBalanceIsFirst = value; modify("LeaveBalanceIsFirst"); }
        }

        public double getBalanceWithoutProrata()
        {
            double tmpBalance =  LeaveBalanceBF
                + Adjust
                - LeaveBalanceForfeiture
                - m_ExpiryForfeit
                - Taken;
            // fix the precision problem caused by too many math operation on floating point number
            tmpBalance = HROne.CommonLib.GenericRoundingFunctions.RoundingTo(tmpBalance, 9, 9);

            return tmpBalance;
        }

        public double getBalance()
        {
            double tmpBalance = LeaveBalanceEntitled
                + LeaveBalanceBF
                + Adjust
                - LeaveBalanceForfeiture
                - m_ExpiryForfeit
                - Taken;
            // fix the precision problem caused by too many math operation on floating point number
            tmpBalance = HROne.CommonLib.GenericRoundingFunctions.RoundingTo(tmpBalance, 9, 9);

            return tmpBalance;
        }
        public DateTime LeaveBalanceEffectiveEndDate;
//        public DateTime CalculationDate;
        //protected double m_Balance;
        [Obsolete("Replaced by getBalance()")]
        public double Balance
        {
            get { return getBalance(); }
            //set { m_Balance = value;  }
        }
        protected double m_Taken;
        /// <summary>
        /// Leave taken between EffectiveDate and AsOfDate
        /// </summary>
        public double Taken
        {
            get { return m_Taken; }
            set { m_Taken = value; }
        }
        protected double m_TakenAfterAsOfDate;
        /// <summary>
        /// Leave taken between next date of AsOfDate and Effective End Date
        /// </summary>
        public double TakenAfterAsOfDate
        {
            get { return m_TakenAfterAsOfDate; }
            set { m_TakenAfterAsOfDate = value; }
        }
        /// <summary>
        /// Leave taken between Effective Start Date and Effective End Date
        /// </summary>
        public double TakenBeforeNextEffectiveDate
        {
            get { return Taken + m_TakenAfterAsOfDate; }
        }
        protected double m_Adjust;
        public double Adjust
        {
            get { return m_Adjust; }
            set { m_Adjust = value; }
        }
        protected double m_ExpiryForfeit;
        public double ExpiryForfeit
        {
            get { return m_ExpiryForfeit; }
            set { m_ExpiryForfeit = value; }
        }
        protected DateTime m_NextExpiryDate;
        public DateTime NextExpiryDate
        {
            get { return m_NextExpiryDate; }
            set { m_NextExpiryDate = value; }
        }
        protected double m_NextExpiryForfeit;
        public double NextExpiryForfeit
        {
            get { return m_NextExpiryForfeit; }
            set { m_NextExpiryForfeit = value; }
        }
        protected double m_Reserved;
        /// <summary>
        /// Leave taken after AsOfDate
        /// </summary>
        public double Reserved
        {
            get { return m_Reserved; }
            set { m_Reserved = value; }
        }

        /// <summary>
        /// Leave taken after Effective End Date
        /// </summary>
        public double ReservedAfterNextEffectiveDate
        {
            get { return m_Reserved - m_TakenAfterAsOfDate; }
        }
        protected string m_Name;
        public string Name
        {
            get { return m_Name; }
            set { m_Name = value; }
        }
        protected string m_Description;
        public string Description
        {
            get { return m_Description; }
            set { m_Description = value; }
        }
        protected string m_StringFormat;
        public string StringFormat
        {
            get { return m_StringFormat; }
            set { m_StringFormat = value; }
        }

        protected LeaveBalanceUnit m_BalanceUnit;
        public LeaveBalanceUnit BalanceUnit
        {
            get { return m_BalanceUnit; }
            set { m_BalanceUnit = value; }
        }

        public static void DeleteLeaveBalanceAfter(DatabaseConnection dbConn, int EmpID, int LeaveTypeID, DateTime DateAfter)
        {
            ELeaveType leaveType_SLCat1 = ELeaveType.SLCAT1_LEAVE_TYPE(dbConn);
            ELeaveType leaveType_SLCat2 = ELeaveType.SLCAT2_LEAVE_TYPE(dbConn);

            DBFilter filter = new DBFilter();
            filter.add(new Match("EmpID", EmpID));
            if (LeaveTypeID > 0)
            {
                OR orLeaveTypeID = new OR();
                orLeaveTypeID.add(new Match("LeaveTypeID", LeaveTypeID));
                if (leaveType_SLCat1 != null && leaveType_SLCat2 != null)
                {
                    if (LeaveTypeID.Equals(leaveType_SLCat1.LeaveTypeID))
                        orLeaveTypeID.add(new Match("LeaveTypeID", leaveType_SLCat2.LeaveTypeID));
                    if (LeaveTypeID.Equals(leaveType_SLCat2.LeaveTypeID))
                        orLeaveTypeID.add(new Match("LeaveTypeID", leaveType_SLCat1.LeaveTypeID));
                }
                filter.add(orLeaveTypeID);
            }
            // Do NOT remove entitlement history. Keep history for retriving expiry date
            //DBFilter entitleFilter = new DBFilter(filter);
            if (!DateAfter.Ticks.Equals(0))
            {
                filter.add(new Match("LeaveBalanceEffectiveDate", ">", DateAfter));
            //    entitleFilter.add(new Match("LeaveBalanceEntitleEffectiveDate", ">", DateAfter));
            }
            ELeaveBalance.db.delete(dbConn, filter);
            //ELeaveBalanceEntitle.db.delete(dbConn, entitleFilter);

        }

        public String PrintMe()
        {
            return "\t================================================================" + Environment.NewLine + 
                   "\tLeaveBalanceID = " + this.LeaveBalanceID.ToString() + Environment.NewLine +
                   "\tEmpID = " + this.EmpID.ToString() + Environment.NewLine +
                   "\tLeaveTypeID = " + this.LeaveTypeID.ToString() + Environment.NewLine +
                   "\tLeaveBalanceEffectiveDate = " + this.LeaveBalanceEffectiveDate.ToString("yyyy-MM-dd") + Environment.NewLine +
                   "\tLeaveBalanceEffectiveEndDate = " + this.LeaveBalanceEffectiveEndDate.ToString("yyyy-MM-dd") + Environment.NewLine +
                   "\tBF Days = " + this.LeaveBalanceBF.ToString("0.00") + Environment.NewLine +
                   "\tEntitled Days = " + this.LeaveBalanceEntitled.ToString("0.00") + Environment.NewLine +
                   "\tForfeited Days = " + this.LeaveBalanceForfeiture.ToString("0.00") + Environment.NewLine +
                   "\tExpired Days = " + this.ExpiryForfeit.ToString("0.00") + Environment.NewLine +
                   "\tBalance C/F = " + this.Balance.ToString("0.00") + Environment.NewLine + 
                   "\tTo-be-expired Days = " + this.NextExpiryForfeit.ToString("0.00") + Environment.NewLine +
                   "\tExpiry Date = " + this.NextExpiryDate.ToString("yyyy-MM-dd") + Environment.NewLine + 
                   "\t================================================================" ;
        }

        public String PrintMe2()
        {
            return "LeaveBalanceID;EmpID;LeaveTypeID;LeaveBalanceEffectiveDate;LeaveBalanceEffectiveEndDate;BF Days;Entitled Days;Forfeited Days;Balance C/F;Expired Days;To-be-expiredDays;ExpiryDate" + Environment.NewLine +
                   this.LeaveBalanceID.ToString() + 
                   ";" + this.EmpID.ToString() + 
                   ";" + this.LeaveTypeID.ToString() + 
                   ";" + this.LeaveBalanceEffectiveDate.ToString("yyyy-MM-dd") + 
                   ";" + this.LeaveBalanceEffectiveEndDate.ToString("yyyy-MM-dd") + 
                   ";" + this.LeaveBalanceBF.ToString("0.00") + 
                   ";" + this.LeaveBalanceEntitled.ToString("0.00") + 
                   ";" + this.LeaveBalanceForfeiture.ToString("0.00") + 
                   ";" + this.ExpiryForfeit.ToString("0.00") + 
                   ";" + this.Balance.ToString("0.00") + 
                   ";" + this.NextExpiryForfeit.ToString("0.00") + 
                   ";" + this.NextExpiryDate.ToString("yyyy-MM-dd");
        }

    }
}
