using System;
using System.Collections.Generic;
using System.Text;
using HROne.DataAccess;

namespace HROne.SaaS.FileExchangeInterface
{
    public class HSBCBankPaymentCodeDistributionProcess
    {
        protected DatabaseConnection dbConn;
        HROne.SaaS.FileExchangeInterface.HSBCFileExchangeProcess tmpProcess = new HROne.SaaS.FileExchangeInterface.HSBCFileExchangeProcess();

        protected string m_FilenameInFile;
        protected string lastLine = string.Empty;

        public string FilenameInFile
        {
            get { return m_FilenameInFile; }
        }

        public HSBCBankPaymentCodeDistributionProcess(DatabaseConnection dbConn)
        {
            this.dbConn = dbConn;

        }

        public void Load(string filename)
        {
            tmpProcess.CreateReader(filename);
            lastLine = string.Empty;
            m_FilenameInFile = tmpProcess.FilenameInFile;
        }
        public void Close()
        {
            tmpProcess.Close();
        }
        public HSBCBankPaymentCodeRecord getBankPaymentCodeRecord()
        {
            if (string.IsNullOrEmpty(lastLine))
                lastLine = tmpProcess.ReadNextLine();
            if (!string.IsNullOrEmpty(lastLine))
                if (!lastLine.StartsWith("TRAILER"))
                {
                    HSBCBankPaymentCodeRecord record = new HSBCBankPaymentCodeRecord();
                    record.m_BankAccountNo = lastLine.Substring(0, 15).Trim();
                    record.m_RemoteProfileID = lastLine.Substring(15, 18).Trim();
                    record.m_BankPaymentCode = lastLine.Substring(33, 3).Trim();
                    record.m_AutoPayInOutFlag = lastLine.Substring(36, 1).Trim();

                    lastLine = null;
                    return record;
                }
            return null;
        }
    }

    public class HSBCBankPaymentCodeRecord
    {
        protected internal string m_RemoteProfileID;
        public string RemoteProfileID
        {
            get { return m_RemoteProfileID; }
        }
        protected internal string m_BankAccountNo;
        public string BankAccountNo
        {
            get { return m_BankAccountNo; }
        }
        protected internal string m_BankPaymentCode;
        public string BankPaymentCode
        {
            get { return m_BankPaymentCode; }
        }
        protected internal string m_AutoPayInOutFlag;
        public string AutoPayInOutFlag
        {
            get { return m_AutoPayInOutFlag; }
        }
        protected internal HSBCBankPaymentCodeRecord()
        {
        }

        protected internal HSBCBankPaymentCodeRecord(string RemoteProfileID, string BankAccountNo, string BankPaymentCode, string AutoPayInOutFlag)
        {
            m_RemoteProfileID = RemoteProfileID;
            m_BankAccountNo = BankAccountNo;
            m_BankPaymentCode = BankPaymentCode;
            m_AutoPayInOutFlag = AutoPayInOutFlag;
        }
    }
}
