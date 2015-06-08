using System;
using System.Data;
using System.Configuration;
using HROne.DataAccess;
////using perspectivemind.validation;
using System.Collections;

namespace HROne.Lib.Entities
{
    [DBClass("PayrollPeriod")]
    public class EPayrollPeriod: BaseObject 
    {
        public static DBManager db = new DBManager(typeof(EPayrollPeriod));

        public const string PAYPERIOD_STATUS_NORMAL_FLAG = "N";
        public const string PAYPERIOD_STATUS_TRIALRUN_FLAG = "T";
        public const string PAYPERIOD_STATUS_CONFIRM_FLAG = "C";
        public const string PAYPERIOD_STATUS_PROCESSEND_FLAG = "E";

        public static WFValueList VLPayPeriodStatus = new AppUtils.NewWFTextList(new string[] { PAYPERIOD_STATUS_NORMAL_FLAG, PAYPERIOD_STATUS_TRIALRUN_FLAG, PAYPERIOD_STATUS_CONFIRM_FLAG, PAYPERIOD_STATUS_PROCESSEND_FLAG }, new string[] { "Normal", "Trial Run", "Confirm", "Process End" });
        public static WFValueList VLPayrollPeriod = new WFDBCodeList(EPayrollPeriod.db, "PayPeriodID", "CONVERT(VARCHAR(10),PayPeriodFr,111)", "CONVERT(VARCHAR(10),PayPeriodTo,111)", "PayPeriodFr desc");
//        public static WFValueList VLPayrollPeriodYear = new WFDBCodeList(EPayrollPeriod.db, "Distinct Year(PayPeriodFr)", " Year(PayPeriodFr)", " Year(PayPeriodFr)", "Year(PayperiodFr) desc");
        public static WFValueList VLPayrollPeriodYear = new AppUtils.WFDBDistinctList(EPayrollPeriod.db, "Year(PayPeriodFr)", " Year(PayPeriodFr)", "Year(PayperiodFr) desc");


        public static EPayrollPeriod GetObject(DatabaseConnection dbConn, int ID)
        {
            if (ID > 0)
            {
                EPayrollPeriod obj = new EPayrollPeriod();
                obj.PayPeriodID = ID;
                if (EPayrollPeriod.db.select(dbConn, obj))
                    return obj;
            }
            return null;
        }

        protected int m_PayPeriodID;
        [DBField("PayPeriodID", true, true), TextSearch, Export(false)]
        public int PayPeriodID
        {
            get { return m_PayPeriodID; }
            set { m_PayPeriodID = value; modify("PayPeriodID"); }
        }

        protected int m_PayGroupID;
        [DBField("PayGroupID"), TextSearch, Export(false),Required ]
        public int PayGroupID
        {
            get { return m_PayGroupID; }
            set { m_PayGroupID = value; modify("PayGroupID"); }
        }

        protected DateTime m_PayPeriodFr;
        [DBField("PayPeriodFr"), TextSearch, Export(false),Required ]
        public DateTime  PayPeriodFr
        {
            get { return m_PayPeriodFr; }
            set { m_PayPeriodFr = value; modify("PayPeriodFr"); }
        }

        protected DateTime m_PayPeriodTo;
        [DBField("PayPeriodTo"), TextSearch, Export(false), Required]
        public DateTime  PayPeriodTo
        {
            get { return m_PayPeriodTo; }
            set { m_PayPeriodTo = value; modify("PayPeriodTo"); }
        }

        protected DateTime m_PayPeriodLeaveCutOffDate;
        [DBField("PayPeriodLeaveCutOffDate"), TextSearch, Export(false), Required]
        public DateTime  PayPeriodLeaveCutOffDate
        {
            get { return m_PayPeriodLeaveCutOffDate; }
            set { m_PayPeriodLeaveCutOffDate = value; modify("PayPeriodLeaveCutOffDate"); }
        }

        protected DateTime m_PayPeriodAttnFr;
        [DBField("PayPeriodAttnFr"), TextSearch, Export(false), Required]
        public DateTime  PayPeriodAttnFr
        {
            get { return m_PayPeriodAttnFr; }
            set { m_PayPeriodAttnFr = value; modify("PayPeriodAttnFr"); }
        }

        protected DateTime m_PayPeriodAttnTo;
        [DBField("PayPeriodAttnTo"), TextSearch, Export(false), Required,  ]
        public DateTime  PayPeriodAttnTo
        {
            get { return m_PayPeriodAttnTo; }
            set { m_PayPeriodAttnTo = value; modify("PayPeriodAttnTo"); }
        }

        protected string m_PayPeriodStatus;
        [DBField("PayPeriodStatus"), TextSearch, Export(false)]
        public string PayPeriodStatus
        {
            get { return m_PayPeriodStatus; }
            set { m_PayPeriodStatus = value; modify("PayPeriodStatus"); }
        }

        protected DateTime m_PayPeriodTrialRunDate;
        [DBField("PayPeriodTrialRunDate", "yyyy-MM-dd HH:mm:ss"), TextSearch, Export(false)]
        public DateTime PayPeriodTrialRunDate
        {
            get { return m_PayPeriodTrialRunDate; }
            set { m_PayPeriodTrialRunDate = value; modify("PayPeriodTrialRunDate"); }
        }

        protected int m_PayPeriodTrialRunBy;
        [DBField("PayPeriodTrialRunBy"), TextSearch, Export(false)]
        public int PayPeriodTrialRunBy
        {
            get { return m_PayPeriodTrialRunBy; }
            set { m_PayPeriodTrialRunBy = value; modify("PayPeriodTrialRunBy"); }
        }

        protected DateTime m_PayPeriodConfirmDate;
        [DBField("PayPeriodConfirmDate", "yyyy-MM-dd HH:mm:ss"), TextSearch, Export(false)]
        public DateTime PayPeriodConfirmDate
        {
            get { return m_PayPeriodConfirmDate; }
            set { m_PayPeriodConfirmDate = value; modify("PayPeriodConfirmDate"); }
        }

        protected int m_PayPeriodConfirmBy;
        [DBField("PayPeriodConfirmBy"), TextSearch, Export(false)]
        public int PayPeriodConfirmBy
        {
            get { return m_PayPeriodConfirmBy; }
            set { m_PayPeriodConfirmBy = value; modify("PayPeriodConfirmBy"); }
        }

        protected DateTime m_PayPeriodProcessEndDate;
        [DBField("PayPeriodProcessEndDate", "yyyy-MM-dd HH:mm:ss"), TextSearch, Export(false)]
        public DateTime PayPeriodProcessEndDate
        {
            get { return m_PayPeriodProcessEndDate; }
            set { m_PayPeriodProcessEndDate = value; modify("PayPeriodProcessEndDate"); }
        }

        protected int m_PayPeriodProcessEndBy;
        [DBField("PayPeriodProcessEndBy"), TextSearch, Export(false)]
        public int PayPeriodProcessEndBy
        {
            get { return m_PayPeriodProcessEndBy; }
            set { m_PayPeriodProcessEndBy = value; modify("PayPeriodProcessEndBy"); }
        }

        protected DateTime m_PayPeriodRollbackDate;
        [DBField("PayPeriodRollbackDate", "yyyy-MM-dd HH:mm:ss"), TextSearch, Export(false)]
        public DateTime PayPeriodRollbackDate
        {
            get { return m_PayPeriodRollbackDate; }
            set { m_PayPeriodRollbackDate = value; modify("PayPeriodRollbackDate"); }
        }

        protected int m_PayPeriodRollbackBy;
        [DBField("PayPeriodRollbackBy"), TextSearch, Export(false)]
        public int PayPeriodRollbackBy
        {
            get { return m_PayPeriodRollbackBy; }
            set { m_PayPeriodRollbackBy = value; modify("PayPeriodRollbackBy"); }
        }

        protected bool m_PayPeriodIsAutoCreate;
        [DBField("PayPeriodIsAutoCreate"), TextSearch, Export(false)]
        public bool PayPeriodIsAutoCreate
        {
            get { return m_PayPeriodIsAutoCreate; }
            set { m_PayPeriodIsAutoCreate = value; modify("PayPeriodIsAutoCreate"); }
        }

        public EPayrollPeriod Copy(DatabaseConnection dbConn, int NewPayGroupID)
        {
            EPayrollPeriod obj = new EPayrollPeriod();
            obj.PayPeriodID = PayPeriodID;
            if (EPayrollPeriod.db.select(dbConn, obj))
            {
                obj.PayPeriodID = 0;
                obj.PayPeriodConfirmBy = 0;
                obj.PayPeriodConfirmDate = new DateTime();
                obj.PayPeriodProcessEndBy = 0;
                obj.PayPeriodProcessEndDate = new DateTime();
                obj.PayPeriodRollbackBy = 0;
                obj.PayPeriodRollbackDate = new DateTime();
                obj.PayPeriodStatus = PAYPERIOD_STATUS_NORMAL_FLAG;
                obj.PayPeriodTrialRunBy = 0;
                obj.PayPeriodTrialRunDate = new DateTime();
                obj.PayGroupID = NewPayGroupID;

                EPayrollPeriod.db.insert(dbConn, obj);
                return obj;
            }
            return null;

        }
    }
}