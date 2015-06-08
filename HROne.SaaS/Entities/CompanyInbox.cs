using System;
using System.Collections.Generic;
using System.Text;
using HROne.DataAccess;

namespace HROne.SaaS.Entities
{
    [DBClass("CompanyInbox")]
    public class ECompanyInbox : DBObject
    {

        public static DBManager db = new DBManager(typeof(ECompanyInbox));
        protected int m_CompanyInboxID;
        protected int attachmentFileCount = 0;

        [DBField("CompanyInboxID", true, true), TextSearch, Export(false)]
        public int CompanyInboxID
        {
            get { return m_CompanyInboxID; }
            set { m_CompanyInboxID = value; modify("CompanyInboxID"); }
        }
        protected int m_CompanyDBID;
        [DBField("CompanyDBID"), TextSearch, Export(false)]
        public int CompanyDBID
        {
            get { return m_CompanyDBID; }
            set { m_CompanyDBID = value; modify("CompanyDBID"); }
        }
        protected string m_CompanyInboxSubject;
        [DBField("CompanyInboxSubject"), TextSearch, Export(false)]
        public string CompanyInboxSubject
        {
            get { return m_CompanyInboxSubject; }
            set { m_CompanyInboxSubject = value; modify("CompanyInboxSubject"); }
        }

        protected string m_CompanyInboxMessage;
        [DBField("CompanyInboxMessage"), TextSearch, Export(false), DBAESEncryptStringField()]
        public string CompanyInboxMessage
        {
            get { return m_CompanyInboxMessage; }
            set { m_CompanyInboxMessage = value; modify("CompanyInboxMessage"); }
        }

        protected DateTime m_CompanyInboxCreateDate;
        [DBField("CompanyInboxCreateDate","yyyy-MM-dd HH:mm:ss"), TextSearch, Export(false)]
        public DateTime CompanyInboxCreateDate
        {
            get { return m_CompanyInboxCreateDate; }
            set { m_CompanyInboxCreateDate = value; modify("CompanyInboxCreateDate"); }
        }

        protected DateTime m_CompanyInboxReadDate;
        [DBField("CompanyInboxReadDate"), TextSearch, Export(false)]
        public DateTime CompanyInboxReadDate
        {
            get { return m_CompanyInboxReadDate; }
            set { m_CompanyInboxReadDate = value; modify("CompanyInboxReadDate"); }
        }

        protected DateTime m_CompanyInboxDeleteDate;
        [DBField("CompanyInboxDeleteDate"), TextSearch, Export(false)]
        public DateTime CompanyInboxDeleteDate
        {
            get { return m_CompanyInboxDeleteDate; }
            set { m_CompanyInboxDeleteDate = value; modify("CompanyInboxDeleteDate"); }
        }

        protected DateTime m_CompanyInboxExpiryDate;
        [DBField("CompanyInboxExpiryDate"), TextSearch, Export(false)]
        public DateTime CompanyInboxExpiryDate
        {
            get { return m_CompanyInboxExpiryDate; }
            set { m_CompanyInboxExpiryDate = value; modify("CompanyInboxExpiryDate"); }
        }
        

        public ECompanyInboxAttachment AddCompanyInboxAttachment(DatabaseConnection dbConn, string AttachmentFileName, string AttachmentFullPath)
        {
            attachmentFileCount++;

            string uploadFolder = ESystemParameter.getParameter(dbConn, ESystemParameter.PARAM_CODE_BANKFILE_UPLOAD_FOLDER);

            string relativePath = System.IO.Path.Combine(System.IO.Path.Combine(this.CompanyDBID.ToString() ,"CompanyInbox"), CompanyInboxID + "_" + AppUtils.ServerDateTime().ToString("yyyyMMddHHmmss") + "-" + attachmentFileCount + ".hrd");
            string destinationFile = System.IO.Path.Combine(uploadFolder, relativePath);
            System.IO.DirectoryInfo CompanyInboxDirectoryInfo = new System.IO.DirectoryInfo(System.IO.Path.GetDirectoryName(destinationFile));
            if (!CompanyInboxDirectoryInfo.Exists)
                CompanyInboxDirectoryInfo.Create();
            zip.Compress(System.IO.Path.GetDirectoryName(AttachmentFullPath), System.IO.Path.GetFileName(AttachmentFullPath), destinationFile);
            System.IO.File.Delete(AttachmentFullPath);


            ECompanyInboxAttachment attachment = new ECompanyInboxAttachment();
            attachment.CompanyInboxID = CompanyInboxID;
            attachment.CompanyInboxAttachmentOriginalFileName = AttachmentFileName;
            attachment.CompanyInboxAttachmentStoredFileName = relativePath;
            attachment.CompanyInboxAttachmentIsCompressed = true;
            attachment.CompanyInboxAttachmentSize = new System.IO.FileInfo(destinationFile).Length;
            ECompanyInboxAttachment.db.insert(dbConn, attachment);

            return attachment;
        }

        public static ECompanyInbox CreateAndSaveCompanyInboxMessage(DatabaseConnection dbConn, int CompanyDBID, string CompanyInboxSubject, string CompanyInboxMessage, int inboxExpiryDays)
        {
            ECompanyInbox CompanyInbox = new ECompanyInbox();
            CompanyInbox.CompanyDBID = CompanyDBID;
            CompanyInbox.CompanyInboxSubject = CompanyInboxSubject;
            CompanyInbox.CompanyInboxMessage = CompanyInboxMessage;
            CompanyInbox.CompanyInboxCreateDate = AppUtils.ServerDateTime();
            if (inboxExpiryDays > 0)
                CompanyInbox.CompanyInboxExpiryDate = CompanyInbox.CompanyInboxCreateDate.AddDays(inboxExpiryDays);
            ECompanyInbox.db.insert(dbConn, CompanyInbox);
            return CompanyInbox;
        }

    }

    [DBClass("CompanyInboxAttachment")]
    public class ECompanyInboxAttachment : DBObject
    {
        public static DBManager db = new DBManager(typeof(ECompanyInboxAttachment));
        protected string m_ExtractedFilename = string.Empty;

        protected int m_CompanyInboxAttachmentID;
        [DBField("CompanyInboxAttachmentID", true, true), TextSearch, Export(false)]
        public int CompanyInboxAttachmentID
        {
            get { return m_CompanyInboxAttachmentID; }
            set { m_CompanyInboxAttachmentID = value; modify("CompanyInboxAttachmentID"); }
        }

        protected int m_CompanyInboxID;
        [DBField("CompanyInboxID"), TextSearch, Export(false), Required]
        public int CompanyInboxID
        {
            get { return m_CompanyInboxID; }
            set { m_CompanyInboxID = value; modify("CompanyInboxID"); }
        }

        protected string m_CompanyInboxAttachmentOriginalFileName;
        [DBField("CompanyInboxAttachmentOriginalFileName"), TextSearch, Export(false)]
        public string CompanyInboxAttachmentOriginalFileName
        {
            get { return m_CompanyInboxAttachmentOriginalFileName; }
            set { m_CompanyInboxAttachmentOriginalFileName = value; modify("CompanyInboxAttachmentOriginalFileName"); }
        }

        protected string m_CompanyInboxAttachmentStoredFileName;
        [DBField("CompanyInboxAttachmentStoredFileName"), TextSearch, Export(false)]
        public string CompanyInboxAttachmentStoredFileName
        {
            get { return m_CompanyInboxAttachmentStoredFileName; }
            set { m_CompanyInboxAttachmentStoredFileName = value; modify("CompanyInboxAttachmentStoredFileName"); }
        }

        protected bool m_CompanyInboxAttachmentIsCompressed;
        [DBField("CompanyInboxAttachmentIsCompressed"), TextSearch, Export(false)]
        public bool CompanyInboxAttachmentIsCompressed
        {
            get { return m_CompanyInboxAttachmentIsCompressed; }
            set { m_CompanyInboxAttachmentIsCompressed = value; modify("CompanyInboxAttachmentIsCompressed"); }
        }

        protected long m_CompanyInboxAttachmentSize;
        [DBField("CompanyInboxAttachmentSize"), TextSearch, Export(false), Required]
        public long CompanyInboxAttachmentSize
        {
            get { return m_CompanyInboxAttachmentSize; }
            set { m_CompanyInboxAttachmentSize = value; modify("CompanyInboxAttachmentSize"); }
        }

        

        public string GetDocumentPhysicalPath(DatabaseConnection dbConn)
        {
            //string pathDelimiter = System.IO.Path.DirectorySeparatorChar.ToString();
            string uploadFolder = ESystemParameter.getParameter(dbConn, ESystemParameter.PARAM_CODE_BANKFILE_UPLOAD_FOLDER);

            string documentFilePath = System.IO.Path.Combine(uploadFolder, CompanyInboxAttachmentStoredFileName);
            //documentFilePath = documentFilePath.Replace(pathDelimiter + pathDelimiter, pathDelimiter);

            return documentFilePath;
        }

        public void Extract(DatabaseConnection dbConn)
        {
            if (CompanyInboxAttachmentIsCompressed)
            {
                string strHRDocumentPath = GetDocumentPhysicalPath(dbConn);
                if (System.IO.File.Exists(strHRDocumentPath))
                {
                    //string pathDelimiter = System.IO.Path.DirectorySeparatorChar.ToString();
                    if (string.IsNullOrEmpty(m_ExtractedFilename))
                    {
                        string strTmpFolder = System.IO.Path.Combine(System.IO.Path.GetTempPath(), CompanyInboxAttachmentStoredFileName + ".dir");
                        //strTmpFolder = strTmpFolder.Replace(pathDelimiter + pathDelimiter, pathDelimiter);


                        zip.ExtractAll(strHRDocumentPath, strTmpFolder, string.Empty);
                        string[] fileList = System.IO.Directory.GetFiles(strTmpFolder);
                        if (fileList.GetLength(0) > 0)
                        {
                            m_ExtractedFilename = fileList[0];
                        }
                    }
                    else
                        if (!System.IO.File.Exists(m_ExtractedFilename))
                        {
                            m_ExtractedFilename = string.Empty;
                            Extract(dbConn);
                        }
                }
            }
        }

        public string GetExtractedFilePath(DatabaseConnection dbConn)
        {
            if (CompanyInboxAttachmentIsCompressed)
            {
                Extract(dbConn);
                return m_ExtractedFilename;
            }
            else
                return string.Empty;
        }

        public void RemoveExtractedFile()
        {
            if (!string.IsNullOrEmpty(m_ExtractedFilename))
            {
                if (System.IO.File.Exists(m_ExtractedFilename))
                {
                    System.IO.FileInfo fileInfo = new System.IO.FileInfo(m_ExtractedFilename);

                    fileInfo.Delete();
                    fileInfo.Directory.Delete(true);

                }
                m_ExtractedFilename = string.Empty;
            }
        }
    }
}