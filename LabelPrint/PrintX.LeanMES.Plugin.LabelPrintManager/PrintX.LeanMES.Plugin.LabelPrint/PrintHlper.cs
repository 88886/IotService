using LabelManager2;
using Newtonsoft.Json2;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Resources;
using System.Web.Script.Serialization;
using Newtonsoft.Json;
using System.Threading;
using System.Threading.Tasks;


namespace PrintX.LeanMES.Plugin.LabelPrint
{
    public class PrintHlper
    {

        #region 打印实例描述符等
        /// <summary>
        /// 打印实例唯一标记
        /// </summary>
        private readonly string TLB_CLASS_ID = "3624B9C0-9E5D-11D3-A896-00C04F324E22";



        /// <summary>
        /// 老式接口连扳标记，已弃用
        /// </summary>
        public static int linkFlag = 0;


        /// <summary>
        /// 老式接口打印份数，已弃用
        /// </summary>
        public static int Copys = 1;

        #endregion



        public void SendContentToPrinter(string printContent, string printerName, string lang, Type obj)
        {
            bool flag = Printer.VerifyPrinter(printerName);
            if (flag)
            {
                string message;
                if (lang == "cn")
                {
                    message = string.Format(new ResourceManager(obj).GetString("NotFountPrinter1"), printerName, printerName);
                }
                else
                {
                    message = string.Format(new ResourceManager(obj).GetString("NotFountPrinter2"), printerName, printerName);
                }
                throw new ApplicationException(message);
            }
            if (!string.IsNullOrEmpty(printContent))
            {
                RawPrinterHelper.SendStringToPrinter(printerName, printContent);
                return;
            }
            string @string;
            if (lang == "cn")
            {
                @string = new ResourceManager(obj).GetString("ZplIsEmpty1");
            }
            else
            {
                @string = new ResourceManager(obj).GetString("ZplIsEmpty2");
            }
            throw new ApplicationException(@string);
        }



        #region 老式打印接口
        /// <summary>
        /// 老式打印接口
        /// </summary>
        /// <param name="labelFilePath">标签模板路径</param>
        /// <param name="labelValue">打印内容</param>
        /// <param name="copies">打印份数</param>
        /// <param name="lang">语言</param>
        /// <param name="obj">反射类型</param>
        /// <param name="printerName">打印机名字</param>
        public void PrintLabelUseCodeSoft(string labelFilePath, string labelValue, int copies, string lang, Type obj, string printerName)
        {
            int errorCode = -1;
            String errorMessage = "";

            CheckPrintConfig(labelFilePath, lang, obj);

            Document document = null;
            application.Documents.Open(labelFilePath, false);
            document = application.ActiveDocument;

            SwitchPrinter(printerName, document);

            try
            {
                ItereatePerLabelContentPrintOld(labelValue, document);
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message + "请在services.msc服务启动Remote Procedure Call和Remote Local Call服务,并确保打印机名称正确，表单变量和系统传递的一致");
            }
            finally
            {

                ResourceRecycle(ref errorCode, ref errorMessage, ref linkFlag, document, application);
            }
        }
        #endregion


        #region 老式接口,迭代每个LabelContent并且在激活的当前文档下打印
        /// <summary>
        /// 老式接口,迭代每个LabelContent并且在激活的当前文档下打印
        /// </summary>
        /// <param name="labelValue"></param>
        /// <param name="document"></param>
        private static void ItereatePerLabelContentPrintOld(string labelValue, Document document)
        {
            List<LabelData> list = new List<LabelData>();
            List<LabelInfo> list2 = new List<LabelInfo>();


            list = Deserialization.JSONStringToList<LabelData>(labelValue);

            for (int i = 0; i < list.Count; i++)
            {
                list2 = Deserialization.JSONStringToList<LabelInfo>(new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(list[i].LabelContent));




                AssignValueToFormVariables(document, list2);
                document.PrintDocument(Copys);


            }
        }
        #endregion

        static Application application = (Application)Activator.CreateInstance(Type.GetTypeFromCLSID(new Guid("3624B9C0-9E5D-11D3-A896-00C04F324E22")));

        #region 新版打印接口
        /// <summary>
        /// 新版打印接口
        /// </summary>
        /// <param name="labelFilePath">标签模板路径</param>
        /// <param name="labelValue">打印内容</param>
        /// <param name="copies">打印份数</param>
        /// <param name="lang">语言</param>
        /// <param name="obj">反射类型</param>
        /// <param name="printerName">打印机名字</param>
        /// <param name="errorCode">错误码</param>
        /// <param name="errorMessage">错误消息</param>
        /// <param name="linkFlag">连扳标记</param>
        public void PrintLabelUseCodeSoft9(string labelFilePath, string labelValue, int copies, string lang, Type obj, string printerName,

            ref int errorCode, ref String errorMessage, int linkFlag
            )
        {


            lock (flag)
            {

                CheckPrintConfig9(labelFilePath, lang, obj, ref errorCode, ref errorMessage);

                application.Documents.Open(labelFilePath);

                Document document = application.ActiveDocument;
                SwitchPrinter(printerName, document);

                try
                {
                    ItereatePerLabelContentAndPrint(labelValue, copies, ref errorCode, ref errorMessage, document);

                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message + "请在services.msc服务启动Remote Procedure Call和Remote Local Call服务,并确保打印机名称正确，表单变量和系统传递的一致");
                }
                finally
                {
                    ResourceRecycle(ref errorCode, ref errorMessage, ref linkFlag, document, application);


                }


            }

        }
        #endregion




        #region 新版接口迭代每个LabelContent并且打印
        /// <summary>
        /// 新版接口迭代每个LabelContent并且打印
        /// </summary>
        /// <param name="labelValue"></param>
        /// <param name="copies"></param>
        /// <param name="errorCode"></param>
        /// <param name="errorMessage"></param>
        /// <param name="document"></param>
        private static void ItereatePerLabelContentAndPrint(string labelValue, int copies, ref int errorCode, ref String errorMessage, Document document)
        {


            List<LabelData> list = new List<LabelData>();
            List<LabelInfo> list2 = new List<LabelInfo>();
            list = Deserialization.JSONStringToList<LabelData>(labelValue);

            for (int i = 0; i < list.Count; i++)
            {
                list2 = Deserialization.JSONStringToList<LabelInfo>(new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(list[i].LabelContent));
                //清空脏数据
                //ClearOldFormVariables(document);
                AssignValueToFormVariables(document, list2);

                for (int m = 0; m < copies; m++)
                {
                    document.PrintDocument(1);
                }
            }

            //到这里可以确认打印成功了，默认是失败的
            errorCode = 0;
            errorMessage = "打印成功";

        }
        #endregion




        #region 切换打印机
        /// <summary>
        /// 切换打印机
        /// 如果失败，将使用系统默认打印机
        /// </summary>
        /// <param name="printerName"></param>
        /// <param name="document"></param>
        private static void SwitchPrinter(string printerName, Document document)
        {
            if (!string.IsNullOrEmpty(printerName))
            {
                try
                {
                    if (!document.Printer.SwitchTo(printerName, "", true))
                    {

                    }
                }
                catch (Exception err)
                {
                    Console.WriteLine(err.Message);
                }
            }
        }
        #endregion




        #region 为激活的文档里边的表单变量赋值
        /// <summary>
        /// 为激活的文档里边的表单变量赋值
        /// </summary>
        /// <param name="document"></param>
        /// <param name="list2"></param>
        private static void AssignValueToFormVariables(Document document, List<LabelInfo> list2)
        {
            for (int j = 0; j < list2.Count; j++)
            {
                if (linkFlag == 1)
                {
                    document.Variables.FormVariables.Item(list2[j].Name + (j + 1).ToString()).Value = list2[j].Value;
                }
                else
                {
                    document.Variables.FormVariables.Item(list2[j].Name).Value = list2[j].Value;
                }
            }
        }
        #endregion


        #region 检查打印配置
        /// <summary>
        /// 检查打印配置
        /// </summary>
        /// <param name="labelFilePath"></param>
        /// <param name="lang"></param>
        /// <param name="obj"></param>
        private void CheckPrintConfig(string labelFilePath, string lang, Type obj)
        {
            if (!File.Exists(labelFilePath))
            {
                string message;
                if (lang == "cn")
                {
                    message = string.Format(new ResourceManager(obj).GetString("NotFountLabTemplate1"), labelFilePath);
                }
                else
                {
                    message = string.Format(new ResourceManager(obj).GetString("NotFountLabTemplate2"), labelFilePath);
                }
                throw new ApplicationException(message);
            }
            if (!Deserialization.ComIsRegistered(this.TLB_CLASS_ID))
            {
                string @string;
                if (lang == "cn")
                {
                    @string = new ResourceManager(obj).GetString("NotFountCodeSoft1");
                }
                else
                {
                    @string = new ResourceManager(obj).GetString("NotFountCodeSoft2");
                }
                throw new ApplicationException(@string);
            }
        }
        #endregion


        #region 检查新版本的打印接口的打印配置
        /// <summary>
        ///检查新版本的打印接口的打印配置
        /// </summary>
        /// <param name="labelFilePath">标签模板路径</param>
        /// <param name="lang">语言</param>
        /// <param name="obj">反射类型</param>
        /// <param name="errorCode">错误码</param>
        /// <param name="errorMessage">错误消息</param>
        private void CheckPrintConfig9(string labelFilePath, string lang, Type obj, ref int errorCode, ref String errorMessage)
        {
            if (!File.Exists(labelFilePath))
            {
                string message;
                if (lang == "cn")
                {
                    message = string.Format(new ResourceManager(obj).GetString("NotFountLabTemplate1"), labelFilePath);
                }
                else
                {
                    message = string.Format(new ResourceManager(obj).GetString("NotFountLabTemplate2"), labelFilePath);
                }
                errorCode = -1;
                errorMessage = message;
                throw new ApplicationException(message);
            }
            if (!Deserialization.ComIsRegistered(this.TLB_CLASS_ID))
            {
                string @string;
                if (lang == "cn")
                {
                    @string = new ResourceManager(obj).GetString("NotFountCodeSoft1");
                }
                else
                {
                    @string = new ResourceManager(obj).GetString("NotFountCodeSoft2");
                }

                errorCode = -1;
                errorMessage = @string;
                throw new ApplicationException(@string);
            }
        }
        #endregion


        static readonly Object flag = new object();

        #region 回收资源
        /// <summary>
        /// 回收资源
        /// </summary>
        /// <param name="errorCode"></param>
        /// <param name="errorMessage"></param>
        /// <param name="linkFlag"></param>
        /// <param name="document"></param>
        private static void ResourceRecycle(ref int errorCode, ref String errorMessage, ref int linkFlag, Document document,

            Application applicaton)
        {

            linkFlag = 0;



            #region  释放资源
            try
            {
                //关闭而且不保存
                applicaton.Documents.CloseAll(true);
            }
            catch (Exception e)
            {
                errorCode = -1;
                errorMessage = e.Message;
            }


            #endregion
            GC.Collect();


            if (errorCode != 0)
            {
                throw new Exception(errorMessage);
            }
        }
        #endregion



        #region 清除表单脏值
        /// <summary>
        /// 清除表单变量脏值
        /// </summary>
        /// <param name="document"></param>
        private static void ClearOldFormVariables(Document document)
        {
            for (int k = 0; k < 100; k++)
            {
                try
                {
                    var item = document.Variables.FormVariables.Item(k);
                    if (item != null)
                    {
                        item.Value = "";
                    }
                }
                catch (Exception err)
                {
                    throw new Exception("清理表单脏值失败"+ err.Message);

                }
            }
        }
        #endregion


        #region 杀进程
        /// <summary>
        /// 终止进程
        /// </summary>
        /// <param name="ProcessName"></param>
        private static void KillProcess(string ProcessName)
        {
            Process[] processesByName = Process.GetProcessesByName(ProcessName);
            {
                for (int i = 0; i < processesByName.Length; i++)
                {
                    Process process = processesByName[i];
                    if (!process.CloseMainWindow())
                    {
                        process.Kill();
                    }
                }
            }
        }
        #endregion


    }
}
