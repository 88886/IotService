using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PrintX.LeanMES.Plugin.LabelPrintX;
using Newtonsoft.Json;
using PrintX.LeanMES.Plugin.LabelPrint;
using Newtonsoft.Json2;
using System.Threading.Tasks;
using System.Windows.Forms;
using PrintX.Dev.Utils.ToolsKit;
using PrintX.LeanMES.Plugin.UI.Tool;

namespace PrintX.LeanMES.Plugin.UI.Task
{
    public class PrintJob
    {
        public void ProcessPrintJob(string e, ref int errorCode, ref String errorMessage)
        {

            Tool.Tools.PubMessage("收到打印请求");
            LabelPrintX.LabelPrint print = new LabelPrintX.LabelPrint();

            HashMap map = JsonConvert.DeserializeObject<HashMap>(e);

            if (!map.ContainsKey("labelJsonData"))
            {
                errorCode = -1;
                errorMessage = "没有传递labelJsonData字段";
                Tool.Tools.PubMessage(errorMessage);
                return;
            }
            String LabelContent = map["labelJsonData"].ToString();
            String printer = map.GetValue<String>("printName");

            if (!map.ContainsKey("tempatePath"))
            {
                errorCode = -1;
                errorMessage = "没有传递tempatePath字段";
                Tool.Tools.PubMessage(errorMessage);

                return;
            }
            String tempatePath = map.GetValue<String>("tempatePath");
            int copys = map.GetValue<int>("copys");
            int linkFlag = map.GetValue<int>("linkFlag");

            Tools.PubMessage("是否连板--------" + linkFlag);
            if (linkFlag < 1)
            {
                //连扳标记
                linkFlag = 0;

            }

            PrintX.LeanMES.Plugin.LabelPrint.PrintHlper.linkFlag = linkFlag;
            if (copys < 1)
            {
                //打印份数
                PrintX.LeanMES.Plugin.LabelPrint.PrintHlper.Copys = 1;
                copys = 1;

            }

            Tools.PubMessage("打印份数--------" + copys);
            string snString = "";
            List<LabelData> list = new List<LabelData>();
            List<LabelInfo> list2 = new List<LabelInfo>();
            list = Deserialization.JSONStringToList<LabelData>(LabelContent);
            try
            {
                //加速赋值
                Parallel.For(0, list.Count, i =>
                {
                    list2 = Deserialization.JSONStringToList<LabelInfo>(JsonConvert.SerializeObject(list[i].LabelContent));
                    for (int j = 0; j < list2.Count; j++)
                    {
                        snString = list2[j].Value;
                    }
                });
            }
            catch (Exception err)
            {
                errorMessage = "解析打印内容的json失败，请校验格式是否正确,您传递的格式为:" + e + "触发的异常信息为：\n" + err.Message;
                errorCode = -1;
              
            }

            try
            {

                Tools.PubMessage("准备打印--------");

                Tools.PubMessage("打印模板--------" + tempatePath);

                Tools.PubMessage("打印机--------" + printer);
                print.PrintLabelUseCodeSoft9(tempatePath, LabelContent, copys, "cn", printer, ref  errorCode,

                ref  errorMessage, linkFlag);

            }
            catch (Exception er)
            {
                errorMessage = er.Message;
                errorCode = -1;
            }

            //组装返回结果
            HashMap responseBean = new HashMap();

            if (errorCode == 0)
            {
                errorMessage = "打印成功";
            }


            Tool.Tools.PubMessage(errorMessage);
        }

    }
}
