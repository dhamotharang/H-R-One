using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace HROne.CommonLib
{
    public class FileIOProcess
    {
        public static string DefaultUploadFolder()
        {
            string strPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Document");
            DirectoryInfo dirInfo= new DirectoryInfo(strPath);
            if (dirInfo.Exists)
                return dirInfo.FullName;
            else
                return string.Empty;
        }

        public static bool IsFileExists(string Path, bool checkFolderWritePermission, out string ErrorMessage)
        {
            FileInfo fileInfo = new FileInfo(Path);
            if (fileInfo.Exists)
                if (checkFolderWritePermission)
                {
                    return IsFolderAllowWritePermission(fileInfo.DirectoryName, out ErrorMessage);
                }
                else
                {
                    ErrorMessage = string.Empty;
                    return true;
                }
            else
            {
                ErrorMessage = "File not found:" + Path;
                return false;
            }
        }

        public static bool IsFolderExists(string Path, out string ErrorMessage)
        {
            if (Directory.Exists(Path))
            {
                ErrorMessage = string.Empty;
                return true;
            }
            else
            {
                ErrorMessage = "Path not found:" + Path;
                return false;
            }
        }
        public static bool IsFolderAllowWritePermission(string Path, out string ErrorMessage)
        {
            if (IsFolderExists(Path, out ErrorMessage))
            {
                FileInfo file = new FileInfo(System.IO.Path.Combine(Path, "~dummy.txt"));
                try
                {
                    file.Create().Close();
                    file.Delete();
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.Write(ex.Message);
                    ErrorMessage = "Please set the folder \"" + Path + "\" with FULL permission for User: \"" + System.Environment.UserName + "\" or Usergroup: \"Users\" ";
                    return false;
                }
                //System.Security.Permissions.FileIOPermission writePermission = new System.Security.Permissions.FileIOPermission(System.Security.Permissions.FileIOPermissionAccess.Write, BOCIEncryptFileInfo.Directory.FullName);
                //if (!System.Security.SecurityManager.IsGranted(writePermission))
                //    errors.addError("Please set the folder \"" + strBOCIEncryptPath + "\" with FULL permission for \"Users\" group");
            }
            else
            {
                return false;
            }
            ErrorMessage = string.Empty;
            return true;
        }

        public static bool MoveDocument(string SourcePath, string DestinationPath)
        {
            string ErrorMessage;

            DirectoryInfo sourceDirInfo = new DirectoryInfo(SourcePath);
            DirectoryInfo destinationDirInfo = new DirectoryInfo(DestinationPath);

            if (!sourceDirInfo.FullName.Equals(destinationDirInfo.FullName))
                if (IsFolderAllowWritePermission(SourcePath, out ErrorMessage) && IsFolderAllowWritePermission(DestinationPath, out ErrorMessage))
                {
                    FileInfo[] moveFileInfoList = sourceDirInfo.GetFiles("*.hrd");
                    foreach (FileInfo fileInfo in moveFileInfoList)
                    {
                        try
                        {
                            fileInfo.MoveTo(System.IO.Path.Combine(destinationDirInfo.FullName, fileInfo.Name));
                        }
                        catch (Exception ex)
                        {
                            throw ex;
                        }
                    }
                }
            return true;
        }
    }
}
