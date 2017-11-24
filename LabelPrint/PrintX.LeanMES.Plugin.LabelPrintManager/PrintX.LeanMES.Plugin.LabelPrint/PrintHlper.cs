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

        #region ��ӡʵ����������
        /// <summary>
        /// ��ӡʵ��Ψһ���
        /// </summary>
        private readonly string TLB_CLASS_ID = "3624B9C0-9E5D-11D3-A896-00C04F324E22";



        /// <summary>
        /// ��ʽ�ӿ������ǣ�������
        /// </summary>
        public static int linkFlag = 0;


        /// <summary>
        /// ��ʽ�ӿڴ�ӡ������������
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



        #region ��ʽ��ӡ�ӿ�
        /// <summary>
        /// ��ʽ��ӡ�ӿ�
        /// </summary>
        /// <param name="labelFilePath">��ǩģ��·��</param>
        /// <param name="labelValue">��ӡ����</param>
        /// <param name="copies">��ӡ����</param>
        /// <param name="lang">����</param>
        /// <param name="obj">��������</param>
        /// <param name="printerName">��ӡ������</param>
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

                throw new Exception(ex.Message + "����services.msc��������Remote Procedure Call��Remote Local Call����,��ȷ����ӡ��������ȷ����������ϵͳ���ݵ�һ��");
            }
            finally
            {

                ResourceRecycle(ref errorCode, ref errorMessage, ref linkFlag, document, application);
            }
        }
        #endregion


        #region ��ʽ�ӿ�,����ÿ��LabelContent�����ڼ���ĵ�ǰ�ĵ��´�ӡ
        /// <summary>
        /// ��ʽ�ӿ�,����ÿ��LabelContent�����ڼ���ĵ�ǰ�ĵ��´�ӡ
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

        #region �°��ӡ�ӿ�
        /// <summary>
        /// �°��ӡ�ӿ�
        /// </summary>
        /// <param name="labelFilePath">��ǩģ��·��</param>
        /// <param name="labelValue">��ӡ����</param>
        /// <param name="copies">��ӡ����</param>
        /// <param name="lang">����</param>
        /// <param name="obj">��������</param>
        /// <param name="printerName">��ӡ������</param>
        /// <param name="errorCode">������</param>
        /// <param name="errorMessage">������Ϣ</param>
        /// <param name="linkFlag">������</param>
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
                    throw new Exception(ex.Message + "����services.msc��������Remote Procedure Call��Remote Local Call����,��ȷ����ӡ��������ȷ����������ϵͳ���ݵ�һ��");
                }
                finally
                {
                    ResourceRecycle(ref errorCode, ref errorMessage, ref linkFlag, document, application);


                }


            }

        }
        #endregion




        #region �°�ӿڵ���ÿ��LabelContent���Ҵ�ӡ
        /// <summary>
        /// �°�ӿڵ���ÿ��LabelContent���Ҵ�ӡ
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
                //���������
                //ClearOldFormVariables(document);
                AssignValueToFormVariables(document, list2);

                for (int m = 0; m < copies; m++)
                {
                    document.PrintDocument(1);
                }
            }

            //���������ȷ�ϴ�ӡ�ɹ��ˣ�Ĭ����ʧ�ܵ�
            errorCode = 0;
            errorMessage = "��ӡ�ɹ�";

        }
        #endregion




        #region �л���ӡ��
        /// <summary>
        /// �л���ӡ��
        /// ���ʧ�ܣ���ʹ��ϵͳĬ�ϴ�ӡ��
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




        #region Ϊ������ĵ���ߵı�������ֵ
        /// <summary>
        /// Ϊ������ĵ���ߵı�������ֵ
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


        #region ����ӡ����
        /// <summary>
        /// ����ӡ����
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


        #region ����°汾�Ĵ�ӡ�ӿڵĴ�ӡ����
        /// <summary>
        ///����°汾�Ĵ�ӡ�ӿڵĴ�ӡ����
        /// </summary>
        /// <param name="labelFilePath">��ǩģ��·��</param>
        /// <param name="lang">����</param>
        /// <param name="obj">��������</param>
        /// <param name="errorCode">������</param>
        /// <param name="errorMessage">������Ϣ</param>
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

        #region ������Դ
        /// <summary>
        /// ������Դ
        /// </summary>
        /// <param name="errorCode"></param>
        /// <param name="errorMessage"></param>
        /// <param name="linkFlag"></param>
        /// <param name="document"></param>
        private static void ResourceRecycle(ref int errorCode, ref String errorMessage, ref int linkFlag, Document document,

            Application applicaton)
        {

            linkFlag = 0;



            #region  �ͷ���Դ
            try
            {
                //�رն��Ҳ�����
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



        #region �������ֵ
        /// <summary>
        /// �����������ֵ
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
                    throw new Exception("�������ֵʧ��"+ err.Message);

                }
            }
        }
        #endregion


        #region ɱ����
        /// <summary>
        /// ��ֹ����
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
