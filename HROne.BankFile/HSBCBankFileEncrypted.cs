using System;
using System.Collections.Generic;
using System.Text;

namespace HROne.BankFile
{


    public class HSBCBankFileEncrypted : HSBCBankFile
    {

        public string publicKeyFile;
        public string publicKeyPassword;

        protected string m_HashTotal;
        public string HashTotal
        {
            get { return m_HashTotal; }
        }

        protected string m_KeyID;
        public string KeyID
        {
            get { return m_KeyID; }
        }

        protected string m_RemoteProfileID;
        public string RemoteProfileID
        {
            get { return m_RemoteProfileID; }
        }
        protected string m_ContactPerson;
        public string ContactPerson
        {
            get { return m_ContactPerson; }
        }
        protected string m_ContactPersonPhoneNumber;
        public string ContactPersonPhoneNumber
        {
            get { return m_ContactPersonPhoneNumber; }
        }
        protected string m_ProductAndVersion;
        public string ProductAndVersion
        {
            get { return m_ProductAndVersion; }
        }
        
        public HSBCBankFileEncrypted(HROne.DataAccess.DatabaseConnection dbConn, string RemoteProfileID, string ContactPerson, string ContactPersonPhoneNumber, string ProductAndVersion)
            : base(dbConn)
        {
            this.m_RemoteProfileID = RemoteProfileID;
            this.m_ContactPerson = ContactPerson;
            this.m_ContactPersonPhoneNumber = ContactPersonPhoneNumber;
            this.m_ProductAndVersion = ProductAndVersion;
        }

        public override System.IO.FileInfo GenerateBankFile()
        {
            System.IO.FileInfo originalFileInfo = base.GenerateBankFile();
            System.IO.FileInfo encryptedFileInfo = GenerateTempFileName();

            m_HashTotal = HROne.HSBC.Utility.encryptFile(originalFileInfo.FullName, encryptedFileInfo.FullName, publicKeyFile, publicKeyPassword);
            if (!m_HashTotal.Equals(string.Empty))
            {
                m_KeyID = HROne.HSBC.Utility.getKeyId(publicKeyFile);
                originalFileInfo.Delete();

                return encryptedFileInfo;
            }
            else
            {
                return originalFileInfo;
            }
        }
        public override string ActualBankFileName()
        {
            return BranchCode.PadRight(3).Substring(0, 3) + AccountNo.PadRight(5).Substring(0, 5) + BankFileExtension();
        }

        public override string BankFileExtension()
        {
            return "." + BankPaymentCode;
        }
    }
}
