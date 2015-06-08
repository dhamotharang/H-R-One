using System;
using System.Data;
using System.Configuration;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using HROne.DataAccess;
using HROne.Lib.Entities;

namespace HROne.BankFile
{

    public interface BankFileControlInterface
    {
        GenericBankFile CreateBankFileObject();
        bool IsAllowChequePayment();
        bool IsShowAllPaymentMethod();
        bool HasValueDate();
    }

    public class NegativeAmountException : Exception
    {
        ArrayList ErrorBankFileDetailList;
        public NegativeAmountException(ArrayList ErrorBankFileDetailList)
            : base("Negative payment amount detected")   
        {
            this.ErrorBankFileDetailList = ErrorBankFileDetailList;
        }

        public ArrayList GetErrorBankFileDetailList()
        {
            return (ArrayList)ErrorBankFileDetailList.Clone();
        }
    }
    public class InvalidEEBankAccountException : Exception
    {
        public string EmpNo;
        public string EmpName;
        public InvalidEEBankAccountException(string EmpNo, string EmpName)
            : base("Invalid Bank Account for employee")
        {
            this.EmpNo = EmpNo;
            this.EmpName = EmpName;
        }
    }

    public class InvalidFieldValueException : Exception
    {
        public InvalidFieldValueException(string message)
            : base(message)
        {

        }

    }

    internal class EmpBankTransactionBreakDownKey
    {
        public EmpBankTransactionBreakDownKey(int EmpAccID, DateTime ValueDate)
        {
            this.m_EmpAccID = EmpAccID;
            this.m_ValueDate = ValueDate;
        }
        protected int m_EmpAccID;
        protected DateTime m_ValueDate;

        public int EmpAccID
        {
            get { return m_EmpAccID; }
        }
        public DateTime  ValueDate
        {
            get { return m_ValueDate; }
        }


        public override bool Equals(object obj)
        {
            if (obj is EmpBankTransactionBreakDownKey)
            {
                EmpBankTransactionBreakDownKey compare = (EmpBankTransactionBreakDownKey)obj;
                if (compare.m_EmpAccID.Equals(m_EmpAccID) && compare.m_ValueDate.Date.Ticks.Equals(m_ValueDate.Date.Ticks))
                    return true;
            }
            return false;
        }
        public override int GetHashCode()
        {
            return 0;
            //                return base.GetHashCode();
        }
    }
    /// <summary>
    /// Summary description for BankFile
    /// </summary>
    public class GenericBankFile
    {
        private const string FIELD_DELIMITER = ",";
        private const string RECORD_DELIMITER = "\r\n";

        protected string m_BankCode;
        public string BankCode
        {
            get { return m_BankCode; }
            set { m_BankCode = value; }
        }

        protected string m_BranchCode;
        public string BranchCode
        {
            get { return m_BranchCode; }
            set { m_BranchCode = value; }
        }

        protected string m_AccountNo;
        public string AccountNo
        {
            get { return m_AccountNo; }
            set { m_AccountNo = value; }
        }

        protected string m_AccountHolderName;
        public string AccountHolderName
        {
            get { return m_AccountHolderName; }
            set { m_AccountHolderName = value; }
        }
        
        protected DateTime m_ValueDate;
        public DateTime ValueDate
        {
            get { return m_ValueDate; }
            set { m_ValueDate = value; }
        }

        protected double m_TotalAmount;
        public double TotalAmount
        {
            get { return m_TotalAmount; }
        }

        public int RecordCount
        {
            get { return BankFileDetails.Count; }
        }

        protected bool m_IsGenerateChequePayment = false;
        public bool IsGenerateChequePayment
        {
            get { return m_IsGenerateChequePayment; }
        }

        protected bool m_hasHeader = true;
        protected bool m_hasFooter = true;

        protected DateTime m_PayPeriodFr = new DateTime();
        public DateTime PayPeriodFr
        {
            get { return m_PayPeriodFr; }
        }
        protected DateTime m_PayPeriodTo = new DateTime();
        public DateTime PayPeriodTo
        {
            get { return m_PayPeriodTo; }
        }

        public List<GenericBankFileDetail> BankFileDetails = new List<GenericBankFileDetail>();
        public List<GenericBankFileDetail> ZeroBankFileDetails = new List<GenericBankFileDetail>();
        protected DatabaseConnection dbConn;
        protected DateTime processDateTime = DateTime.Today;

        public GenericBankFile(DatabaseConnection dbConn)
        {
            this.dbConn = dbConn.createClone();
            m_IsGenerateChequePayment = false;
            processDateTime = AppUtils.ServerDateTime();
        }

        public virtual void LoadBankFileDetail(ArrayList PayrollBatchList, ArrayList EmpList)
        {
            BankFileDetails.Clear();
            m_TotalAmount = 0;

            //  Move autoPayList out of the EmpList loop for sharing bank account feature between roles
            Dictionary<EmpBankTransactionBreakDownKey, GenericBankFileDetail> autoPayList = new Dictionary<EmpBankTransactionBreakDownKey, GenericBankFileDetail>();
            List<int> PayPeriodIDList = new List<int>();
            foreach (EEmpPersonalInfo empInfo in EmpList)
            {
                Dictionary<EmpBankTransactionBreakDownKey, GenericBankFileDetail> chequePaymentList = new Dictionary<EmpBankTransactionBreakDownKey, GenericBankFileDetail>();

                DBFilter empPayrollFilter = new DBFilter();
                empPayrollFilter.add(new Match("EmpID", empInfo.EmpID));
                OR orPayrollBatch = new OR();
                foreach (EPayrollBatch payBatch in PayrollBatchList)
                    orPayrollBatch.add(new Match("PayBatchID", payBatch.PayBatchID));
                empPayrollFilter.add(orPayrollBatch);

                ArrayList empPayrollList = EEmpPayroll.db.select(dbConn, empPayrollFilter);

                foreach (EEmpPayroll empPayroll in empPayrollList)
                {
                    if (!PayPeriodIDList.Contains(empPayroll.PayPeriodID))
                        PayPeriodIDList.Add(empPayroll.PayPeriodID);

                    if (!m_ValueDate.Ticks.Equals(0))
                        empPayroll.EmpPayValueDate = m_ValueDate;
                    if (empPayroll.EmpPayValueDate.Ticks.Equals(0))
                    {
                        EPayrollBatch payBatch = new EPayrollBatch();
                        payBatch.PayBatchID = empPayroll.PayBatchID;
                        if (EPayrollBatch.db.select(dbConn, payBatch))
                            empPayroll.EmpPayValueDate = payBatch.PayBatchValueDate;
                    }

                    DBFilter paymentRecordFilter = new DBFilter();
                    paymentRecordFilter.add(new Match("EmpPayrollID", empPayroll.EmpPayrollID));
                    OR orPaymentMethodTerm = new OR();
                    orPaymentMethodTerm.add(new Match("PayRecMethod", "A"));
                    if (m_IsGenerateChequePayment)
                        orPaymentMethodTerm.add(new Match("PayRecMethod", "Q"));
                    paymentRecordFilter.add(orPaymentMethodTerm);

                    ArrayList paymentRecordList = EPaymentRecord.db.select(dbConn, paymentRecordFilter);
                    foreach (EPaymentRecord paymentRecord in paymentRecordList)
                    {
                        if (paymentRecord.PayRecMethod.Equals("A"))
                        {

                            EEmpBankAccount empAcc = new EEmpBankAccount();
                            empAcc.EmpBankAccountID = paymentRecord.EmpAccID;
                            if (!EEmpBankAccount.db.select(dbConn, empAcc))
                            {
                                empAcc = EEmpBankAccount.GetDefaultBankAccount(dbConn, empInfo.EmpID);
                                if (empAcc == null)
                                {
                                    EEmpPersonalInfo.db.select(dbConn, empInfo);
                                    throw new InvalidEEBankAccountException(empInfo.EmpNo, empInfo.EmpEngFullName);
                                }
                            }
                            EmpBankTransactionBreakDownKey key = new EmpBankTransactionBreakDownKey(empAcc.EmpBankAccountID, empPayroll.EmpPayValueDate);

                            GenericBankFileDetail BankFileDetail;
                            if (autoPayList.ContainsKey(key))
                                BankFileDetail = autoPayList[key];
                            else
                            {
                                BankFileDetail = CreateBankFileDetail(empAcc.EmpID);
                                BankFileDetail.EmpBankAccountHolderName = empAcc.EmpBankAccountHolderName.Trim();
                                BankFileDetail.BankCode = empAcc.EmpBankCode;
                                BankFileDetail.BranchCode = empAcc.EmpBranchCode;
                                BankFileDetail.AccountNo = empAcc.EmpAccountNo;
                                BankFileDetail.ValueDate = empPayroll.EmpPayValueDate;
                                autoPayList.Add(key, BankFileDetail);
                            }
                            BankFileDetail.Amount += paymentRecord.PayRecActAmount;
                            BankFileDetail.Amount = Math.Round(BankFileDetail.Amount, 2, MidpointRounding.AwayFromZero);
                            m_TotalAmount += paymentRecord.PayRecActAmount;
                        }
                        else if (paymentRecord.PayRecMethod.Equals("Q") && m_IsGenerateChequePayment)
                        {
                            EmpBankTransactionBreakDownKey key = new EmpBankTransactionBreakDownKey(0, empPayroll.EmpPayValueDate);
                            GenericBankFileDetail BankFileDetail;
                            if (chequePaymentList.ContainsKey(key))
                                BankFileDetail = chequePaymentList[key];
                            else
                            {
                                BankFileDetail = CreateBankFileDetail(empInfo.EmpID);
                                BankFileDetail.ValueDate = empPayroll.EmpPayValueDate;
                                BankFileDetail.IsChequePayment = true;
                                chequePaymentList.Add(key, BankFileDetail);
                            }
                            BankFileDetail.Amount += paymentRecord.PayRecActAmount;
                            BankFileDetail.Amount = Math.Round(BankFileDetail.Amount, 2, MidpointRounding.AwayFromZero);
                            m_TotalAmount += paymentRecord.PayRecActAmount;
                        }
                    }
                }

                BankFileDetails.AddRange(chequePaymentList.Values);
                chequePaymentList.Clear();
            }
            BankFileDetails.AddRange(autoPayList.Values);
            autoPayList.Clear();

            List<GenericBankFileDetail> NegativeBankFileDetailList = new List<GenericBankFileDetail>();
            List<GenericBankFileDetail> zeroAmountBankFileDetails = new List<GenericBankFileDetail>();
            foreach (GenericBankFileDetail bankFileDetail in BankFileDetails)
                if (bankFileDetail != null)
                    if (bankFileDetail.Amount < 0)
                        NegativeBankFileDetailList.Add(bankFileDetail);
                    else if (Math.Abs(bankFileDetail.Amount) <= 0.005)
                        zeroAmountBankFileDetails.Add(bankFileDetail);
            foreach (GenericBankFileDetail bankFileDetail in zeroAmountBankFileDetails)
            {
                BankFileDetails.Remove(bankFileDetail);
                ZeroBankFileDetails.Add(bankFileDetail);
            }
            foreach (int PayPeriodID in PayPeriodIDList)
            {
                EPayrollPeriod payPeriod = new EPayrollPeriod();
                payPeriod.PayPeriodID = PayPeriodID;
                if (EPayrollPeriod.db.select(dbConn, payPeriod))
                {
                    if (m_PayPeriodFr.Ticks.Equals(0) || m_PayPeriodFr > payPeriod.PayPeriodFr)
                        m_PayPeriodFr = payPeriod.PayPeriodFr;
                    if (m_PayPeriodTo.Ticks.Equals(0) || m_PayPeriodTo < payPeriod.PayPeriodTo)
                        m_PayPeriodTo = payPeriod.PayPeriodTo;
                }
            }
            if (NegativeBankFileDetailList.Count > 0)
                throw new NegativeAmountException(new ArrayList(NegativeBankFileDetailList));
        }

        //public void LoadBankFileDetail(int[] PayrollBatchIDList, int[] EmpIDList)
        //{
        //    BankFileDetails.Clear();
        //    m_TotalAmount = 0;
        //    foreach (int EmpID in EmpIDList)
        //    {
        //        DBFilter empPayrollFilter = new DBFilter();
        //        empPayrollFilter.add(new Match("EmpID", EmpID));
        //        OR orPayrollBatch = new OR();
        //        foreach (int PayBatchID in PayrollBatchIDList)
        //        {
        //            orPayrollBatch.add(new Match("PayBatchID", PayBatchID));
        //        }
        //        empPayrollFilter.add(orPayrollBatch);

        //        IN inEmpPayroll = new IN("EmpPayrollID", "Select EmpPayrollID from EmpPayroll", empPayrollFilter);

        //        DBFilter paymentRecordFilter = new DBFilter();
        //        paymentRecordFilter.add(inEmpPayroll);
        //        paymentRecordFilter.add(new Match("PayRecMethod", "A"));
        //        paymentRecordFilter.add("EmpAccID", true);
        //        ArrayList paymentRecords = EPaymentRecord.db.select(dbConn, paymentRecordFilter);

        //        int empCurrentAccID = -1;
        //        GenericBankFileDetail bankFileDetail = null;
        //        foreach (EPaymentRecord paymentRecord in paymentRecords)
        //        {
        //            if (empCurrentAccID != paymentRecord.EmpAccID)
        //            {
        //                empCurrentAccID = paymentRecord.EmpAccID;
        //                bankFileDetail = CreateBankFileDetail(EmpID);
        //                EEmpBankAccount empAcc = new EEmpBankAccount();
        //                empAcc.EmpBankAccountID = empCurrentAccID;
        //                EEmpBankAccount.db.select(dbConn, empAcc);
        //                bankFileDetail.EmpBankAccountHolderName = empAcc.EmpBankAccountHolderName;
        //                bankFileDetail.BankCode = empAcc.EmpBankCode;
        //                bankFileDetail.BranchCode = empAcc.EmpBranchCode;
        //                bankFileDetail.AccountNo = empAcc.EmpAccountNo;
        //                BankFileDetails.Add(bankFileDetail);
        //            }
        //            bankFileDetail.Amount += paymentRecord.PayRecActAmount;
        //            m_TotalAmount += paymentRecord.PayRecActAmount;
        //        }
        //    }
        //}
        public virtual string ActualBankFileName()
        {
            return "BankFile_" + processDateTime.ToString("yyyyMMddHHmmss") + BankFileExtension();
        }
        public virtual string BankFileExtension()
        {
            return ".csv";
        }

        public virtual FileInfo GenerateBankFile()
        {
            string bankFileData = string.Empty;
            if (m_hasHeader)
                bankFileData = GenerateBankFileHeader();
            foreach (GenericBankFileDetail bankFileDetail in BankFileDetails)
            {
                if (string.IsNullOrEmpty(bankFileData))
                    bankFileData = GenerateBankFileDetail(bankFileDetail);
                else
                    bankFileData += RECORD_DELIMITER + GenerateBankFileDetail(bankFileDetail);
            }
            if (m_hasFooter)
                bankFileData += RECORD_DELIMITER + GenerateBankFileFooter();

            FileInfo result = GenerateTempFileName();
            StreamWriter writer = new StreamWriter(result.OpenWrite());
            writer.Write(bankFileData);
            writer.Close();
            return result;
        }

        protected virtual string GenerateBankFileHeader()
        {
            string[] bankFileHeader = new string[3];
            bankFileHeader[0] = BankCode + BranchCode + AccountNo;
            bankFileHeader[1] = TotalAmount.ToString("0.00");
            bankFileHeader[2] = ValueDate.ToString("yyyyMMdd");

            return string.Join(FIELD_DELIMITER, bankFileHeader);
        }

        protected virtual string GenerateBankFileDetail(GenericBankFileDetail bankFileDetail)
        {
            string[] bankFileDetailRecord = new string[4];
            
            bankFileDetailRecord[0] = bankFileDetail.EmpNo;
            bankFileDetailRecord[1] = bankFileDetail.EmpBankAccountHolderName;
            bankFileDetailRecord[2] = bankFileDetail.BankCode + bankFileDetail.BranchCode + bankFileDetail.AccountNo;
            bankFileDetailRecord[3] = bankFileDetail.Amount.ToString("0.00"); 
            string bankFileDetailData = String.Join(FIELD_DELIMITER, bankFileDetailRecord);
            return bankFileDetailData;
        }

        protected virtual string GenerateBankFileFooter()
        {
            string[] bankFileFooter = new string[3];
            bankFileFooter[0] = BankCode + BranchCode + AccountNo;
            bankFileFooter[1] = TotalAmount.ToString("0.00");
            bankFileFooter[2] = ValueDate.ToString("yyyyMMdd");

            return string.Join(FIELD_DELIMITER, bankFileFooter);
        }

        private GenericBankFileDetail CreateBankFileDetail(int EmpID)
        {
            GenericBankFileDetail bankFileDetail = new GenericBankFileDetail();
            EEmpPersonalInfo empInfo = new EEmpPersonalInfo();
            empInfo.EmpID = EmpID;
            if (EEmpPersonalInfo.db.select(dbConn, empInfo))
            {
                bankFileDetail.EmpID = empInfo.EmpID;
                bankFileDetail.EmpNo = empInfo.EmpNo;
                bankFileDetail.EmpName = empInfo.EmpEngFullName;
            }

            DBFilter empPosFilter = new DBFilter();
            empPosFilter.add(new Match("EmpID", EmpID));
            empPosFilter.add("EmpPosEffFr", false);
            ArrayList empPosList = EEmpPositionInfo.db.select(dbConn, empPosFilter);
            if (empPosList.Count > 0)
            {
                EEmpPositionInfo empPos = (EEmpPositionInfo)empPosList[0];
                ECompany company = new ECompany();
                company.CompanyID = empPos.CompanyID;
                ECompany.db.select(dbConn, company);
                bankFileDetail.CompanyID = company.CompanyID;
                bankFileDetail.CompanyName = company.CompanyName;
            }


            return bankFileDetail;
        }

        public DataSet.Payroll_AutoPayList CreateAutopayListDataSet()
        {
            DataSet.Payroll_AutoPayList dataSet = new DataSet.Payroll_AutoPayList();
            

            foreach (GenericBankFileDetail bankFileDetail in BankFileDetails)
            {
                DataSet.Payroll_AutoPayList.AutoPayListRow row = dataSet.AutoPayList.NewAutoPayListRow();
                row.EmpID = bankFileDetail.EmpID;
                row.EmpNo = bankFileDetail.EmpNo;
                row.EmpName = bankFileDetail.EmpName;
                row.CompanyID = bankFileDetail.CompanyID;
                row.CompanyName = bankFileDetail.CompanyName;
                row.AccountNo = bankFileDetail.BankCode + "-" + bankFileDetail.BranchCode + "-" + bankFileDetail.AccountNo;
                row.AccountHolderName = bankFileDetail.EmpBankAccountHolderName;
                row.Amount = bankFileDetail.Amount;
                if (!bankFileDetail.ValueDate.Ticks.Equals(0))
                row.ValueDate = bankFileDetail.ValueDate;
                dataSet.AutoPayList.Rows.Add(row);
            }
            foreach (GenericBankFileDetail bankFileDetail in ZeroBankFileDetails)
            {
                DataSet.Payroll_AutoPayList.ZeroAutoPayListRow row = dataSet.ZeroAutoPayList.NewZeroAutoPayListRow();
                row.EmpID = bankFileDetail.EmpID;
                row.EmpNo = bankFileDetail.EmpNo;
                row.EmpName = bankFileDetail.EmpName;
                row.CompanyID = bankFileDetail.CompanyID;
                row.CompanyName = bankFileDetail.CompanyName;
                row.AccountNo = bankFileDetail.BankCode + "-" + bankFileDetail.BranchCode + "-" + bankFileDetail.AccountNo;
                row.AccountHolderName = bankFileDetail.EmpBankAccountHolderName;
                row.Amount = bankFileDetail.Amount;
                dataSet.ZeroAutoPayList.Rows.Add(row);
            }
            return dataSet;
        }

        protected FileInfo GenerateTempFileName()
        {
            string exportFileName = Path.GetTempFileName();
            FileInfo fileInfo = new FileInfo(exportFileName);
            //fileInfo.MoveTo(exportFileName += BankFileExtension());
            return fileInfo;

        }
    }

    public class GenericBankFileDetail
    {
        protected int m_EmpID;
        public int EmpID
        {
            get { return m_EmpID; }
            set { m_EmpID = value; }
        }

        protected string m_EmpNo;
        public string EmpNo
        {
            get { return m_EmpNo; }
            set { m_EmpNo = value; }
        }

        protected string m_EmpName;
        public string EmpName
        {
            get { return m_EmpName; }
            set { m_EmpName = value; }
        }

        protected int m_CompanyID;
        public int CompanyID
        {
            get { return m_CompanyID; }
            set { m_CompanyID = value; }
        }

        protected string m_CompanyName;
        public string CompanyName
        {
            get { return m_CompanyName; }
            set { m_CompanyName = value; }
        }

        protected bool m_IsChequePayment = false;
        public bool IsChequePayment
        {
            get { return m_IsChequePayment; }
            set { m_IsChequePayment = value; }
        }

        protected string m_EmpBankAccountHolderName;
        public string EmpBankAccountHolderName
        {
            get { return m_EmpBankAccountHolderName; }
            set { m_EmpBankAccountHolderName = value; }
        }

        protected string m_BankCode;
        public string BankCode
        {
            get { return m_BankCode; }
            set { m_BankCode = value; }
        }

        protected string m_BranchCode;
        public string BranchCode
        {
            get { return m_BranchCode; }
            set { m_BranchCode = value; }
        }

        protected string m_AccountNo;
        public string AccountNo
        {
            get { return m_AccountNo; }
            set { m_AccountNo = value; }
        }

        protected double m_Amount;
        public double Amount
        {
            get { return m_Amount; }
            set { m_Amount = value; }
        }

        protected DateTime m_ValueDate;
        public DateTime ValueDate
        {
            get { return m_ValueDate; }
            set { m_ValueDate = value; }
        }

        
    }
}