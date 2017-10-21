using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace MDW_wf.Connectivity
{
    public static class Logger
    {
        static string sSource;
        static string sLog;
        static object myLock = new object();

        public static void WriteLog(string sEvent)
        {
            lock(myLock)
            {
                try
                {
                    string path = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
                    string date = DateTime.Now.ToString();

                    using (StreamWriter sw = new StreamWriter(path + "\\log.txt",true))
                    {
                        sw.WriteLine(date + " " + sEvent);
                        sw.Close();
                    }
                }
                catch { }
            }
        }
        
    }
}
