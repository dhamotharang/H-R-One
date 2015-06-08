using System;
using System.Collections.Generic;
using System.Text;

namespace HROne.MPFFile
{


    public class HSBCMPFGatewayFileEncrypted : HSBCMPFGatewayFile
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

        protected string m_ProductAndVersion;
        public string ProductAndVersion
        {
            get { return m_ProductAndVersion; }
        }

        public HSBCMPFGatewayFileEncrypted(HROne.DataAccess.DatabaseConnection dbConn, string RemoteProfileID, string ProductAndVersion)
            : base(dbConn, RemoteProfileID)
        {
            //this.m_RemoteProfileID = RemoteProfileID;
            //this.m_ContactPerson = ContactPerson;
            //this.m_ContactPersonPhoneNumber = ContactPersonPhoneNumber;
            this.m_ProductAndVersion = ProductAndVersion;
        }

        public override System.IO.FileInfo GenerateMPFFile()
        {
            System.IO.FileInfo originalFileInfo = base.GenerateMPFFile();
            System.IO.FileInfo encryptedFileInfo = GenerateTempFileName();

            string m_HashTotal = HROne.HSBC.Utility.encryptFile(originalFileInfo.FullName, encryptedFileInfo.FullName, publicKeyFile, publicKeyPassword);
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
        //public override string ActualMPFFileName()
        //{
        //    return EmployerID + "_" + AppUtils.ServerDateTime().ToString("yyyyMMddHHmmss") + MPFFileExtension();
        //}

        //public override string MPFFileExtension()
        //{
        //    return "." + PayCenterCode;
        //}
    }
}
