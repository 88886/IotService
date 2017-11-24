using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using PrintX.PrintService;
using PrintX.LeanMES.Plugin.UI.Listeners.Base;
using System.Web;
using PrintX.LeanMES.Plugin.UI.Tool;
using PrintX.Dev.Utils.ToolsKit;

namespace PrintX.LeanMES.Plugin.UI.Listeners.Service
{
    public class PrintModule : BaseModule
    {

        public PrintModule()
        {

            ///以UTF8的方式解码
            Post["/CQRS/PrintLabel/"] = parameter =>
              {
                  String errorMessage = "";
                  int errorCode = -1;

                  try
                  {
                      String bodyData = HttpUtility.UrlDecode(GetBodyRaw(), Encoding.UTF8);
                      Form1.ProcPrintJob(bodyData, ref errorCode, ref errorMessage);
                      if (errorCode == 0)
                      {
                          errorMessage = "打印成功";
                      }
                      return "{}";
                  }
                  catch (Exception err)
                  {
                      errorCode = -1;
                      errorMessage = err.Message;
                  }

                  HashMap map = new HashMap();
                  map.Add("errorCode", errorCode);
                  map.Add("errorMessage", errorMessage);
                  String responseStr = JsonConvert.SerializeObject(map);
                  return responseStr;
              };




            ///以base64的方式解码

            Post["/"] = parameter =>
              {

                  Tools.PubMessage("收到打印请求");
                  int errorCode = -1;
                  String errorMessage = "";
                  try
                  {
                      String bodyData = GetBodyRaw();
                      string orgStr = "";
                      while (!(orgStr.StartsWith("{") && orgStr.EndsWith("}")))
                      {
                          byte[] outputb = Convert.FromBase64String(bodyData);

                          orgStr = Encoding.Default.GetString(outputb);

                          bodyData = orgStr;
                      }
                      Form1.ProcPrintJob(orgStr, ref errorCode, ref errorMessage);
                      if (errorCode == 0)
                      {

                          errorMessage = "打印成功";


                      }



                  }
                  catch (Exception err)
                  {
                      errorCode = -1;
                      errorMessage = err.Message;

                    
                  }

                  HashMap map = new HashMap();
                  map.Add("errorCode", errorCode);
                  map.Add("errorMessage", errorMessage);
                  String responseStr = JsonConvert.SerializeObject(map);
                  Tools.PubMessage(errorMessage);
                  return responseStr;

                
              };



            Get["/SetPrinter/{printer}/"] = dynamicPara =>
            {
                Setting.Setting.globalPrinter = dynamicPara.printer;
                return
                    "<HTML><head></Head><body><h1>你已经将当前打印机设置为" +
                    dynamicPara.printer + "</h1></body></HTML>";
            };

        }






    }


}
