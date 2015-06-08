using System;
using System.Collections.Generic;
using System.IO;

namespace HROne.Common
{
    public abstract class Folder
    {
        protected static string GetDefaultApplicationTempFolder()
        {
            string path = AppDomain.CurrentDomain.SetupInformation.ApplicationBase;

            while (path.EndsWith(System.IO.Path.DirectorySeparatorChar.ToString()))
                path = path.Substring(0, path.Length - 1);
            string applicationName = path.Substring(path.LastIndexOf(System.IO.Path.DirectorySeparatorChar) + 1);
            //applicationName = "~" + applicationName + "_Temp"; 
            return System.IO.Path.Combine(System.IO.Path.GetTempPath(), applicationName);
        }

        public static void ClearApplicationTempFolder()
        {
            string tmpFolder = GetDefaultApplicationTempFolder();
            if (System.IO.Directory.Exists(tmpFolder))
            {
                try
                {
                    System.IO.Directory.Delete(tmpFolder, true);
                }
                catch (System.Security.SecurityException)
                {
                    System.IO.DirectoryInfo tmpPathDirInfo = new System.IO.DirectoryInfo(tmpFolder);
                    System.Security.AccessControl.DirectorySecurity dSecurity = tmpPathDirInfo.GetAccessControl();
                    try
                    {
                        bool tryModify = false;
                        dSecurity.ModifyAccessRule(System.Security.AccessControl.AccessControlModification.Reset, new System.Security.AccessControl.FileSystemAccessRule("everyone", System.Security.AccessControl.FileSystemRights.FullControl, System.Security.AccessControl.InheritanceFlags.ObjectInherit | System.Security.AccessControl.InheritanceFlags.ContainerInherit, System.Security.AccessControl.PropagationFlags.None, System.Security.AccessControl.AccessControlType.Allow), out tryModify);
                        if (tryModify)
                        {
                            tmpPathDirInfo.SetAccessControl(dSecurity);
                            tmpPathDirInfo.Delete(true);
                        }
                        else
                        {
                            dSecurity.AddAccessRule(new System.Security.AccessControl.FileSystemAccessRule("everyone", System.Security.AccessControl.FileSystemRights.FullControl, System.Security.AccessControl.InheritanceFlags.ObjectInherit | System.Security.AccessControl.InheritanceFlags.ContainerInherit, System.Security.AccessControl.PropagationFlags.None, System.Security.AccessControl.AccessControlType.Allow));
                            tmpPathDirInfo.SetAccessControl(dSecurity);
                            tmpPathDirInfo.Delete(true);
                        }
                    }
                    catch
                    {
                    }
                }
                catch
                {
                }
            }
        }
        public static DirectoryInfo GetOrCreateSessionTempFolder(string SessionID)
        {
            try
            {
                string tmpPath = System.IO.Path.Combine(GetOrCreateApplicationTempFolder().FullName, SessionID);
                System.IO.DirectoryInfo tmpPathDirInfo = new System.IO.DirectoryInfo(tmpPath);
                if (!tmpPathDirInfo.Exists)
                {
                    tmpPathDirInfo.Create();
                    System.Security.AccessControl.DirectorySecurity dSecurity = tmpPathDirInfo.GetAccessControl();
                    dSecurity.AddAccessRule(new System.Security.AccessControl.FileSystemAccessRule("everyone", System.Security.AccessControl.FileSystemRights.FullControl, System.Security.AccessControl.InheritanceFlags.ObjectInherit | System.Security.AccessControl.InheritanceFlags.ContainerInherit, System.Security.AccessControl.PropagationFlags.None, System.Security.AccessControl.AccessControlType.Allow));
                    tmpPathDirInfo.SetAccessControl(dSecurity);
                }
                return tmpPathDirInfo;
            }
            catch
            {
            }
            return new DirectoryInfo(System.IO.Path.GetTempPath());
        }
        public static DirectoryInfo GetOrCreateApplicationTempFolder()
        {

            System.IO.DirectoryInfo tmpPathDirInfo = new System.IO.DirectoryInfo(GetDefaultApplicationTempFolder());
            if (!tmpPathDirInfo.Exists)
            {
                tmpPathDirInfo.Create();
                System.Security.AccessControl.DirectorySecurity dSecurity = tmpPathDirInfo.GetAccessControl();
                dSecurity.AddAccessRule(new System.Security.AccessControl.FileSystemAccessRule("everyone", System.Security.AccessControl.FileSystemRights.FullControl, System.Security.AccessControl.InheritanceFlags.ObjectInherit | System.Security.AccessControl.InheritanceFlags.ContainerInherit, System.Security.AccessControl.PropagationFlags.None, System.Security.AccessControl.AccessControlType.Allow));
                tmpPathDirInfo.SetAccessControl(dSecurity);
            }
            return tmpPathDirInfo;

        }
    }
}
