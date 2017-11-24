using PrintX.Dev.Utils.ToolsKit.RedisSDK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace PrintX.LeanMES.Plugin.UI.Log
{

    interface ILog
    {


    }

    class Log : ILog
    {


        private static readonly object signal = new object();


        internal void info(String message)
        {


            DateTime now = DateTime.Now;
            String prefix = now.Year + "-" + now.Month + "-" + now.Day + "-" + now.Hour + "-" + now.Minute + "-" + now.Second + "********";
            message = prefix + message;
            String preDir = "log\\" + now.Year + now.Month + "\\";
            if (!Directory.Exists(preDir))
            {
                try
                {
                    Directory.CreateDirectory(preDir);
                }
                catch (Exception err)
                {
                    Console.WriteLine(err.Message);
                }
            }



            lock (signal)
            {
                using (StreamWriter sw = new StreamWriter(preDir + now.Year + "-" + now.Month + "-" + now.Day + ".log", true))
                {
                    sw.WriteLine(message);
                    sw.Close();
                  

                }
            }



            Console.WriteLine(message);

        }
    }
}
