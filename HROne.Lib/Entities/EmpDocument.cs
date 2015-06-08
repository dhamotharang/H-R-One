using System;
using System.Collections.Generic;
using System.Text;
using HROne.DataAccess;
////using perspectivemind.validation;

namespace HROne.Lib.Entities
{
    [DBClass("EmpDocument")]
    public class EEmpDocument : BaseObject
    {
        public static DBManager db = new DBManager(typeof(EEmpDocument));
        protected string m_ExtractedFilename = string.Empty;

        protected int m_EmpDocumentID;
        [DBField("EmpDocumentID", true, true), TextSearch, Export(false)]
        public int EmpDocumentID
        {
            get { return m_EmpDocumentID; }
            set { m_EmpDocumentID = value; modify("EmpDocumentID"); }
        }
        protected int m_EmpID;
        [DBField("EmpID"), TextSearch, Export(false)]
        public int EmpID
        {
            get { return m_EmpID; }
            set { m_EmpID = value; modify("EmpID"); }
        }
        protected int m_DocumentTypeID;
        [DBField("DocumentTypeID"), TextSearch, Export(false), Required]
        public int DocumentTypeID
        {
            get { return m_DocumentTypeID; }
            set { m_DocumentTypeID = value; modify("DocumentTypeID"); }
        }

        protected string m_EmpDocumentOriginalFileName;
        [DBField("EmpDocumentOriginalFileName"), TextSearch, Export(false)]
        public string EmpDocumentOriginalFileName
        {
            get { return m_EmpDocumentOriginalFileName; }
            set { m_EmpDocumentOriginalFileName = value; modify("EmpDocumentOriginalFileName"); }
        }

        protected string m_EmpDocumentDesc;
        [DBField("EmpDocumentDesc"), TextSearch, MaxLength(250, 50), Export(false)]
        public string EmpDocumentDesc
        {
            get { return m_EmpDocumentDesc; }
            set { m_EmpDocumentDesc = value; modify("EmpDocumentDesc"); }
        }

        protected string m_EmpDocumentStoredFileName;
        [DBField("EmpDocumentStoredFileName"), TextSearch, Export(false)]
        public string EmpDocumentStoredFileName
        {
            get { return m_EmpDocumentStoredFileName; }
            set { m_EmpDocumentStoredFileName = value; modify("EmpDocumentStoredFileName"); }
        }

        protected bool m_EmpDocumentIsCompressed;
        [DBField("EmpDocumentIsCompressed"), TextSearch, Export(false)]
        public bool EmpDocumentIsCompressed
        {
            get { return m_EmpDocumentIsCompressed; }
            set { m_EmpDocumentIsCompressed = value; modify("EmpDocumentIsCompressed"); }
        }

        protected bool m_EmpDocumentIsProfilePhoto;
        [DBField("EmpDocumentIsProfilePhoto"), TextSearch, Export(false)]
        public bool EmpDocumentIsProfilePhoto
        {
            get { return m_EmpDocumentIsProfilePhoto; }
            set { m_EmpDocumentIsProfilePhoto = value; modify("EmpDocumentIsProfilePhoto"); }
        }

        public string GetDocumentPhysicalPath(DatabaseConnection dbConn)
        {
            //string pathDelimiter = System.IO.Path.DirectorySeparatorChar.ToString();

            string documentFilePath = System.IO.Path.Combine(AppUtils.GetDocumentUploadFolder(dbConn), EmpDocumentStoredFileName);
            //documentFilePath = documentFilePath.Replace(pathDelimiter + pathDelimiter, pathDelimiter);

            return documentFilePath;
        }

        public void Extract(DatabaseConnection dbConn)
        {
            if (EmpDocumentIsCompressed)
            {
                string strHRDocumentPath = GetDocumentPhysicalPath(dbConn);
                if (System.IO.File.Exists(strHRDocumentPath))
                {
                    //string pathDelimiter = System.IO.Path.DirectorySeparatorChar.ToString();
                    if (string.IsNullOrEmpty(m_ExtractedFilename))
                    {
                        string strTmpFolder = System.IO.Path.Combine(System.IO.Path.GetTempPath() , EmpDocumentStoredFileName + ".dir");
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
            if (EmpDocumentIsCompressed)
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

        public static EEmpDocument GetProfilePhotoEmpDocument(DatabaseConnection dbConn, int EmpID)
        {
            DBFilter filter = new DBFilter();
            filter.add(new Match("EmpID", EmpID));
            filter.add(new Match("EmpDocumentIsProfilePhoto", "<>", false));
            System.Collections.ArrayList empDocumentList = EEmpDocument.db.select(dbConn, filter);
            if (empDocumentList.Count > 0)
                return (EEmpDocument)empDocumentList[0];
            else
                return null;
        }





    }
}
