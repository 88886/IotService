using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Nancy.Hosting.Self;
using System.Threading.Tasks;
using System.Threading;

namespace PrintX.LeanMES.Plugin.UI.Listeners.Hosts
{
    public class HttpService
    {
        static List<NancyHost> httpHostList = new List<NancyHost>();

        public static void addHost(String ip, Int32 port)
        {

          
            string DOMAIN = "http://" + ip + ":" + port;
            Console.WriteLine("打印服务器的地址是" + DOMAIN);
            NancyHost nancyHost = new Nancy.Hosting.Self.NancyHost(new Uri(DOMAIN));
            Thread td = new Thread(new ParameterizedThreadStart(StartDomain));
            td.Start(nancyHost);
           

        }


        public static void StartDomain(Object host)
        {
            try {
                Console.WriteLine("准备启动打印服务器nancy");
                ((NancyHost)host).Start();
                Console.WriteLine("启动成功");
            }catch(Exception err)
            {
                Console.WriteLine("启动失败"+err.Message);
            }
        }

    }
}
