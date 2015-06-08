using System;
using System.Collections.Generic;
using System.Text;

namespace HROne.HSBC
{
    public sealed class Utility
    {
        [System.Runtime.InteropServices.DllImport(@"MRI2Client.dll", SetLastError = true)]
        static extern string getKeyId(out string ret, string pubkeyfilepath);

        [System.Runtime.InteropServices.DllImport(@"MRI2Client.dll", SetLastError = true)]
        static extern string encryptFile(out string ret, string infile, string outfile, string pubkeyfilepath, string password);

        [System.Runtime.InteropServices.DllImport(@"MRI2Client.dll", SetLastError = true)]
        static extern string getHashValue(out string ret, string KeyID, string plainTextFilePath);

        [System.Runtime.InteropServices.DllImport(@"MRI2Client.dll", SetLastError = true)]
        static extern void updateCustPwd(out int ret, string pubkeyfilepath, string oldPassword, string password);

        [System.Runtime.InteropServices.DllImport(@"MRI2Client.dll", SetLastError = true)]
        static extern void verifyCustPwd(out int ret, string pubkeyfilepath, string password);

        [System.Runtime.InteropServices.DllImport(@"MRI2Client.dll", SetLastError = true)]
        static extern void isKeyExpired(out int ret, string pubkeyfilepath);

        [System.Runtime.InteropServices.DllImport(@"MRI2Client.dll", SetLastError = true)]
        static extern void keyExpiredLeftDays(out int ret, string pubkeyfilepath);

        private static bool RunAsCommandLine = true;
        public static string lastOutput = string.Empty;
        public static string lastError = string.Empty;
        private static string m_HSBCMRICommandLineDirectory = string.Empty;
        public static string HSBCMRICommandLineDirectory
        {
            set { m_HSBCMRICommandLineDirectory = value; }
        }
        public static string TestCommand()
        {
            return ExecMRI2(string.Empty);
        }
        public static string getKeyId(string pubkeyfilepath)
        {
            if (RunAsCommandLine)
            {

                string Result = ExecMRI2(" -k \"" + pubkeyfilepath + "\" ");

                if (Result.Equals("-1"))
                {
                    return string.Empty;
                }
                else
                    return Result;
            }
            else
            {
                init();
                string result;
                return getKeyId(out result, pubkeyfilepath);
            }
        }

        public static string encryptFile(string infile, string outfile, string pubkeyfilepath, string password)
        {
            if (!System.IO.File.Exists(pubkeyfilepath))
                return string.Empty;
            init();
            //if (verifyCustPwd(pubkeyfilepath, password) && !isKeyExpired(pubkeyfilepath))
            //{
            //    string KeyID = getKeyId(pubkeyfilepath);
            //    System.Diagnostics.Process process = new System.Diagnostics.Process();
            //    process.StartInfo.FileName = System.IO.Path.Combine(binFolder,"MRI2.exe");
            //    process.StartInfo.WorkingDirectory = binFolder;
            //    process.StartInfo.Arguments = " -e \"" + infile + "\" \"" + outfile + "\" \"" + pubkeyfilepath + "\" " + KeyID + " " + password + " " + System.IO.Path.Combine(binFolder, "HSBCMRIOutput.log");
            //    System.Diagnostics.Debug.WriteLine(process.StartInfo.Arguments);
            //    process.StartInfo.CreateNoWindow = false;
            //    process.StartInfo.ErrorDialog = false;
            //    process.StartInfo.LoadUserProfile = true;
            //    process.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
            //    try
            //    {
            //        process.Start();
            //    }
            //    catch (Exception ex)
            //    {
            //        //result.Delete();
            //        throw ex;
            //    }
            //    process.WaitForExit();

            //    if (new System.IO.FileInfo(outfile).Length > 0)
            //    {
            //        return true;
            //    }
            //    else
            //        throw new Exception("Unknown error found. Please check the encryption program manually");

            //}
            //else
            //    return false;
            string KeyID = getKeyId(pubkeyfilepath);
            if (verifyCustPwd(pubkeyfilepath, password) && !isKeyExpired(pubkeyfilepath))
            {
                if (RunAsCommandLine)
                {
                    string tmpOutputLogFilePath = System.IO.Path.GetTempFileName();

                    string Result = ExecMRI2(" -e \"" + infile + "\" \"" + outfile + "\" \"" + pubkeyfilepath + "\" " + KeyID + " " + password + " " + tmpOutputLogFilePath);

                    string hashValue = string.Empty;
                    if (new System.IO.FileInfo(outfile).Length > 0 && Result.Equals("0000"))
                    {
                        System.IO.StreamReader logReader = new System.IO.StreamReader(tmpOutputLogFilePath);
                        while (!logReader.EndOfStream)
                        {
                            string[] lineColumn = logReader.ReadLine().Split(new char[] { '\t' }, StringSplitOptions.None);
                            if (lineColumn[2].Trim().Equals("HASH VALUE", StringComparison.CurrentCultureIgnoreCase))
                                hashValue = lineColumn[3].Trim();
                        }
                        logReader.Close();
                    }
                    System.IO.File.Delete(tmpOutputLogFilePath);
                    if (hashValue.Equals(string.Empty))
                        throw new Exception("Unknown error found. Please check the encryption program manually");

                    return hashValue;
                }
                else
                {
                    string result = string.Empty;
                    string encryptedResult = encryptFile(out result, infile, outfile, pubkeyfilepath, password);
                    if (encryptedResult.Equals("0000"))
                        return getHashValue(KeyID, infile);
                }
            }
            return string.Empty;
        }

        public static string getHashValue(string KeyID, string plainTextFilePath)
        {
            if (RunAsCommandLine)
            {
                throw new Exception("function is not supported in command mode");
            }
            else
            {

                init();
                string result;
                return getHashValue(out result, KeyID, plainTextFilePath);
            }
        }

        public static bool updateCustPwd(string pubkeyfilepath, string oldPassword, string password)
        {
            if (RunAsCommandLine)
            {

                string Result = ExecMRI2(" -p \"" + pubkeyfilepath + "\" " + oldPassword + " " + password);

                if (Result.Equals("1"))
                {
                    return true;
                }
                else
                    return false;
            }
            else
            {

                init();
                int result;
                updateCustPwd(out result, pubkeyfilepath, oldPassword, password);
                if (result == 0)
                    return false;
                return true;
            }
        }

        public static bool verifyCustPwd(string pubkeyfilepath, string password)
        {
            if (RunAsCommandLine)
            {
                string tmpPubKeyFile = System.IO.Path.GetTempFileName();
                System.IO.File.Copy(pubkeyfilepath, tmpPubKeyFile, true);

                try
                {
                    bool result = updateCustPwd(tmpPubKeyFile, password, password);
                    return result;
                }
                catch (Exception)
                {
                    return false;
                }
                finally
                {
                    System.IO.File.Delete(tmpPubKeyFile);
                }
            }
            else
            {
                init();
                int result;
                verifyCustPwd(out result, pubkeyfilepath, password);
                if (result == 0)
                    return false;
                return true;
            }
        }

        public static bool isKeyExpired(string pubkeyfilepath)
        {
            if (RunAsCommandLine)
            {
                DateTime dtExpiryDate = getExpiryDate(pubkeyfilepath);
                if (dtExpiryDate > DateTime.Today)
                    return false;
                else
                    return true;
            }
            else
            {
                init();
                int result;
                isKeyExpired(out result, pubkeyfilepath);
                if (result == 0)
                    return false;
                return true;
            }
        }

        public static DateTime getExpiryDate(string pubkeyfilepath)
        {
            if (RunAsCommandLine)
            {

                string Result = ExecMRI2(" -d \"" + pubkeyfilepath);

                if (Result.Equals("-1"))
                    return new DateTime();

                DateTime dtExpiryDate;

                if (DateTime.TryParseExact(Result, "ddMMyyyy", null, System.Globalization.DateTimeStyles.None, out dtExpiryDate))
                {
                    return dtExpiryDate;
                }
                else
                    throw new Exception("Unknown error found. Please check the encryption program manually: " + Result);
            }
            else
            {
                throw new Exception("function is not supported in DLL mode");
            }
        }
        public static int keyExpiredLeftDays(string pubkeyfilepath)
        {
            if (RunAsCommandLine)
            {
                DateTime dtExpiryDate = getExpiryDate(pubkeyfilepath);
                if (dtExpiryDate > DateTime.Today)
                    return ((TimeSpan)dtExpiryDate.Subtract(DateTime.Today)).Days;
                else
                    return 0;
            }
            else
            {
                init();
                int result;
                keyExpiredLeftDays(out result, pubkeyfilepath);
                return result;
            }
        }

        private static string ExecMRI2(string arguments)
        {
            string binFolder = AppDomain.CurrentDomain.SetupInformation.PrivateBinPath;
            if (string.IsNullOrEmpty(binFolder))
                binFolder = string.Empty;
            if (!string.IsNullOrEmpty(m_HSBCMRICommandLineDirectory))
                binFolder = m_HSBCMRICommandLineDirectory;
            string MRI2Path = System.IO.Path.Combine(binFolder, "MRI2.exe");
            if (System.IO.File.Exists(MRI2Path))
            {
                System.Diagnostics.Process process = new System.Diagnostics.Process();
                process.StartInfo.FileName = System.IO.Path.Combine(binFolder, "MRI2.exe");
                process.StartInfo.WorkingDirectory = binFolder;
                process.StartInfo.Arguments = arguments;
                System.Diagnostics.Debug.WriteLine(process.StartInfo.Arguments);
                process.StartInfo.CreateNoWindow = true;
                process.StartInfo.ErrorDialog = false;
                process.StartInfo.LoadUserProfile = false;
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.RedirectStandardError = true;
                process.StartInfo.UseShellExecute = false;  // do not use shellexec commandline for providing output to text
                process.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
                try
                {
                    process.Start();
                }
                catch (Exception ex)
                {
                    //result.Delete();
                    throw ex;
                }
                StringBuilder output = new StringBuilder();
                StringBuilder error = new StringBuilder();
                //output.Append(process.StandardOutput.ReadToEnd());
                //error.Append(process.StandardError.ReadToEnd());
                //while (!process.HasExited)
                //{
                //    output.Append(process.StandardOutput.ReadToEnd());
                //    error.Append(process.StandardError.ReadToEnd());
                //    System.Threading.Thread.Sleep(100);
                //}
                process.WaitForExit();
                output.Append(process.StandardOutput.ReadToEnd());
                error.Append(process.StandardError.ReadToEnd());
                process.Close();
                lastOutput = output.ToString();
                lastError = error.ToString();
                string[] resultLines = output.ToString().Split(new string[] { "\r\n", "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries);
                string result = resultLines[resultLines.GetUpperBound(0)];
                int resultPosition = result.LastIndexOf("Return:");
                if (resultPosition > 0)
                    return result.Substring(resultPosition + 7).Trim();
                else
                    return result.Trim();
            }
            else
                throw new Exception("File Not found: " + MRI2Path);
            //return string.Empty;
        }

        private static void init()
        {
            //if (System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase.IndexOf("HROne", StringComparison.CurrentCultureIgnoreCase) < 0)
            //    throw new Exception("Invalid HROne Product");
        }
    }
}
