using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Threading.Tasks;
using System.Configuration;
using PrintX.PrintService;
using System.IO;

namespace PrintX.LeanMES.Plugin.UI.Tool
{
    public class Tools
    {

        public static string[] GetLocalIp()
        {

            string hostname = Dns.GetHostName();
            IPHostEntry localhost = Dns.GetHostByName(hostname);
            IPAddress[] localaddr = localhost.AddressList;
            String[] ipList = new string[1];
            ipList[0] = localaddr[0].ToString();
            return ipList;

        }

        internal static int[] getPortList(int p)
        {
            int[] portList = new int[1] { 8088 };

            return portList;
        }



        public static String GetAppSetting(String key)
        {

            return ConfigurationManager.AppSettings[key];
        }



        public static void PubMessage(String message)
        {

            DateTime now = DateTime.Now;
           

           
            String prefix = now.Year + "-" + now.Month + "-" + now.Day+"-"+now.Hour+"-"+now.Minute+"-"+now.Second + "********";
           

            using (StreamWriter sw = new StreamWriter(now.Year + "-" + now.Month + "-" + now.Day + ".log", true))
            {
                sw.WriteLine(prefix + message);
                sw.Close();
                sw.Dispose();
            }

            foreach (var socketServer in Form1.socketServers)
            {
                var sessions = socketServer.GetAllSessions();

                foreach (var session in sessions)
                {
                    session.Send(message);
                }
            }
        }
    }
}
