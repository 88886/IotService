using System;
using System.Text;
using PrintX.PrintService;
using Newtonsoft.Json;
using PrintX.LeanMES.Plugin.UI.Listeners.Base;
using System.IO;
using System.Threading;

namespace PrintX.LeanMES.Plugin.UI
{
    public class IndexModule : BaseModule
    {


        public static String content = "";
        public IndexModule()
        {

            Get["/"] = parameter =>
            {

                Thread td = new Thread(new ThreadStart(Send));
                td.Start();
               
             
                return content;

                
            };


         

        }

        public void Send()
        {
            Thread.Sleep(3000);
            Tool.Tools.PubMessage("欢迎使用PrintX服务引擎");
        }

    }
}
