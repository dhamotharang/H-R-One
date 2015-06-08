using System;
using System.Collections.Generic;
using System.Text;

namespace HROne.CommonLib
{
    public class Logger
    {
        public static void log(String s)
        {
            String path = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "\\logs");

            System.IO.StreamWriter file = null;

            bool isExists = System.IO.Directory.Exists(path);

            if (!isExists)
                System.IO.Directory.CreateDirectory(path);
            try
            {
                file = new System.IO.StreamWriter(path + "\\log_" + DateTime.Now.ToString("yyyyMMdd") + ".log", true);

                file.WriteLine("[" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + "] " + s);
            }
            catch 
            {
                ;
            }
            finally
            {
                if (file != null)
                    file.Close();
            }
        }
    }
}
