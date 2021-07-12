using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace GN_Shiro
{
    public static class LogWriter
    {
        public static void Log(string Message)
        {

            if (Program.LoggingEnabled == 1)
            {/*
                StreamWriter sw = null;
                try
                {
                    sw = new StreamWriter(AppDomain.CurrentDomain.BaseDirectory + "\\ShiroGameLog.txt", true);
                    sw.WriteLine(DateTime.Now.ToString() + ":" + Message);
                    sw.Flush();
                    sw.Close();
                }
                catch { }*/
            }

        }
    }
}
