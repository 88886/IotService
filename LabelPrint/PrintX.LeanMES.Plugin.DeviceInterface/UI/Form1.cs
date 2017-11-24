using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using PrintX.LeanMES.Plugin.LabelPrintX;
using PrintX.LeanMES.Plugin.LabelPrint;

using SuperWebSocket;

using SuperSocket.SocketBase.Config;
using SuperSocket.SocketBase;
using SuperSocket.SocketEngine;
using WebSocket4Net;
using Newtonsoft.Json;
using System.Threading;
using Newtonsoft.Json2;
using System.IO;
using System.Configuration;
using System.Net;
using PrintX.LeanMES.Plugin.UI.Tool;
using PrintX.LeanMES.Plugin.UI.Listeners.Hosts;
using System.Threading.Tasks;
using PrintX.LeanMES.Plugin.UI.Setting;
using PrintX.LeanMES.Plugin.SerialPort;
using System.IO.Ports;
using PrintX.Dev.Utils.ToolsKit;
using PrintX.LeanMES.Plugin.UI.MessageMap;
using PrintX.LeanMES.Plugin.UI.Param.Request;
using PrintX.LeanMES.Plugin.UI.Controller;
using Autofac;
using PrintX.LeanMES.Plugin.UI.Response.Entity;
using Autofac.Core;
using PrintX.LeanMES.Plugin.ModLib;
using PrintX.LeanMES.Plugin.UI.Log;
using PrintX.LeanMES.Plugin.UI;


namespace PrintX.PrintService
{
    public partial class Form1 : Form
    {
        private Log _log = new Log();


        public static System.Threading.Timer m_timer;
        public static System.Threading.Timer h_timer;

        public Form1()
        {
            _log.info("程序启动");
            InitializeComponent();
            _log.info("隐藏程序");
            this.WindowState = FormWindowState.Minimized;
            this.ShowInTaskbar = false;
            SetVisibleCore(false);
        }

        private void LoadPrinters()
        {
            try
            {
                List<String> printerList = Printer.GetLocalPrinters();
                printerList.Add("请选择打印机");
                foreach (var key in printerList)
                {
                    this.comboBox1.Items.Add(key);
                }
            }
            catch (Exception err)
            {
                _log.info(err.Message);
                MessageBox.Show(err.Message);
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {


            try
            {
                _log.info("初始化配置");
                InitConfig();

                _log.info("启动WebSoccket服务器");
                //启动WebSoccket服务器
                StartWebSocketServer();

                String ip = Tools.GetAppSetting("ip");

                using (StreamReader sr = new StreamReader("home.csv"))
                {

                    IndexModule.content = sr.ReadToEnd().Replace("127.0.0.1", ip);
                    sr.Close();
                    sr.Dispose();
                }

                //启动SKT HTTP引擎
                _log.info("启动PrintX HTTP引擎");

                Bend.Util.SatrtEngine.Start();

                StartNancy();

                //初始化IOC容器
                _log.info("初始化IOC容器");
                InitIocContainer();

                //开启同步加速任务
                _log.info("开启同步加速任务");

            }
            catch (Exception er)
            {
                _log.info(er.Message);
            }


        }



        public static ContainerBuilder builder;


        public static Autofac.IContainer container;


        private void InitIocContainer()
        {

            builder = new ContainerBuilder();

            builder.RegisterType<SerialPortActionRole>();

            builder.RegisterType<RS232Bean>();



            builder.RegisterType<RS232ResponseBean>();

            container = builder.Build();
        }






        private static void StartNancy()
        {
            Thread td = new Thread(new
                ThreadStart(StartDomain));
            td.Start();
        }



        public static void StartDomain()
        {

            String ip = Tools.GetAppSetting("ip");
            HttpService.addHost(ip, 27777);

        }


        public static void PoolintHttpServer(object para)
        {
            //确保HTTP监听服务正确运行
        }


        public static void PoolingWebSocketServer(object para)
        {

            //确保websocket监听服务正确运行
        }

        public static void InitConfig()
        {


        }

        public static void AutoStart()
        {


        }




        public static WebSocket websocket = new WebSocket("ws://localhost:2012");

        public static List<WebSocketServer> socketServers;

        public void StartWebSocketServer()
        {
            Int32 socketserverCount = 1;
            try
            {
                socketserverCount=Int32.Parse(Tools.GetAppSetting("socketserver"));

            }catch(Exception err)
            {
                _log.info(err.Message);
            }

            socketServers = new List<WebSocketServer>(socketserverCount);

            for(int i=0;i< socketServers.Count;i++)
            {
                WebSocketServer socketServer = socketServers[i];
                socketServer.Setup(new RootConfig(),
                    new ServerConfig
                    {
                        Name = "SuperWebSocket",
                        Ip = "Any",
                        Port = 2012+i


                    });

                socketServer.NewMessageReceived += new SessionHandler<WebSocketSession, string>(WebSocketServer_NewMessageReceived);

                bool flag = socketServer.Start();
            }

            OpenWebSocketClient();
        }



        protected void WebSocketServer_NewMessageReceived(WebSocketSession session, string e)
        {

            Console.WriteLine("收到websocket请求,sessionID为" + session.SessionID);

            Console.WriteLine("收到websocket请求,内容为" + e);




            try
            {

                EnumMessageMap enNo = getRequestNo(e,session.SessionID);
                String result = e;

                int errorCode = 0;
                String errorMessage = "";
                switch (enNo)
                {

                    //获取打印机列表
                    case EnumMessageMap.printerList:

                        result = ProcGetPrinterList();

                        break;

                    //执行小规模打印任务
                    case EnumMessageMap.printJob:

                        result = ProcPrintJob(e, ref errorCode, ref errorMessage);

                        break;

                    //旧业务写程下载+清空目录
                    case EnumMessageMap.BurnSoft:

                        result = BurnSoftDown(e);

                        break;

                    //获取本机在局域网中的IP
                    case EnumMessageMap.httpServer:

                        result = ProcIp(session, e);

                        break;

                    //旧业务，获取合众思壮电子秤重量
                    case EnumMessageMap.com_weighting:

                        result = ProcSerialPort(session, e);

                        break;


                    //获取串口列表
                    case EnumMessageMap.comList:

                        result = comList(session, e);

                        break;


                    //配置串口
                    case EnumMessageMap.comSetting:

                        result = comSetting(session, e);

                        break;


                    case EnumMessageMap.openCom:

                        String sessionID = session.SessionID;

                        if (!SerialPortEntity.currentSessionPool.ContainsKey(sessionID))
                        {
                            SerialPortEntity.currentSessionPool.Add(sessionID, session);
                        }

                        result = openCom(e);

                        break;


                    case EnumMessageMap.closeCom:

                        result = closeCom(e);

                        break;

                    //向串口发送十六进制指令
                    case EnumMessageMap.sendComCommand:

                        result = sendComCommand(session, e);

                        break;


                    //创建文件
                    case EnumMessageMap.createFile:

                        result = createFile(session, e);

                        break;


                    //删除文件
                    case EnumMessageMap.deleteFile:

                        result = deleteFile(session, e);

                        break;

                    //重命名文件
                    case EnumMessageMap.renameFile:

                        result = renameFile(session, e);

                        break;


                    //解析文件配置
                    case EnumMessageMap.resolveFileSetting:

                        result = resolveFile(session, e);

                        break;


                    //删除目录
                    case EnumMessageMap.rmDir:

                        result = rmDir(session, e);

                        break;


                    //监控目录
                    case EnumMessageMap.watchDir:

                        result = watchDir(session, e);

                        break;

                    //创建目录
                    case EnumMessageMap.mkDir:

                        result = mkDir(session, e);

                        break;


                    //重命名或者移动目录
                    case EnumMessageMap.mvDir:

                        result = mvDir(session, e);

                        break;

                    //配置FTP访问方式
                    case EnumMessageMap.ftpSetting:

                        result = ftpSetting(session, e);

                        break;


                    //启用三菱plc
                    case EnumMessageMap.StartSLPLC:

                        result = FetchValueFromPlc(session, e);

                        break;


                    //停用三菱plc
                    case EnumMessageMap.stopSLPlc:

                        result = stopSLPlc(session, e);

                        break;


                    //通过modbus协议从plc从机设备的某个寄存器地址读取值
                    case EnumMessageMap.modbusInteract:

                        result = modbusInteract(session, e);

                        break;


                    //向指定的sessionID串（逗号分隔），推送消息，主要是用于订阅延迟返回
                    //的一些消息
                    case EnumMessageMap.pushMessage:

                        result = pushMessage(e);

                        break;


                    case EnumMessageMap.BroadCastMessage:

                        BroadCastMsg(session, e);

                        result = EnumMessageMap.OK.ToString();

                        break;

                    default:

                        break;
                }

                session.Send(result);

                Console.WriteLine("正确处理了请求" + result);
            }
            catch (Exception err)
            {
                Console.WriteLine("处理结果异常" + err.Message);
                try
                {
                    session.Send(e);
                }
                catch (Exception er)
                {
                    Console.WriteLine("回发结果异常" + er.Message);
                }
            }

        }

        private void BroadCastMsg(WebSocketSession sessionSender, String msg)
        {

            foreach (var socketServer in socketServers)
            {

                foreach (var session in socketServer.GetAllSessions())
                {
                    if (sessionSender.SessionID != session.SessionID)
                        session.Send(msg);
                }
            }

        }


        /// <summary>
        /// 关闭串口
        /// </summary>
        /// <returns></returns>
        private string closeCom(String e)
        {

            var role = container.Resolve<SerialPortActionRole>();

            RS232Bean bean = container.Resolve<RS232Bean>();



            HashMap paras = JsonConvert.DeserializeObject<HashMap>(e);

            bean.PortName = paras.GetValue<String>("PortName");

            role.BaseBean = bean;

            return role.closeCom();

        }

        private string openCom(string e)
        {




            var role = container.Resolve<SerialPortActionRole>();

            RS232Bean bean = container.Resolve<RS232Bean>();


            HashMap paras = JsonConvert.DeserializeObject<HashMap>(e);


            bean.PortName = paras.GetValue<String>("PortName");

            role.BaseBean = bean;

            return role.openCom();



        }

        private string pushMessage(string e)
        {
            throw new NotImplementedException();
        }

        private string modbusInteract(WebSocketSession session, string e)
        {
            throw new NotImplementedException();
        }

        private string stopSLPlc(WebSocketSession session, string e)
        {
            throw new NotImplementedException();
        }


        private string FetchValueFromPlc(WebSocketSession session, string e)
        {

            HashMap obj = JsonConvert.DeserializeObject<HashMap>(e);

            int slaveMachineID = obj.GetValue<int>("SlaveID");

            byte address = (byte)slaveMachineID;
            ushort start = obj.GetValue<ushort>("start");
            ushort registers = obj.GetValue<ushort>("registers");
            short[] values = new short[registers];

            int waitTime = obj.GetValue<int>("waitTime");

            String subscribe = obj.GetValue<String>("SubscribeWay");

            modbus.SubscribeWay = subscribe;


            modbus.WaitTime = waitTime;


            modbus md = new modbus();

            md.SendFc3NewVersion(address, start, registers, ref values);

            if (!String.IsNullOrEmpty(subscribe) && subscribe == "发送之后阻塞直到获取返回")
            {
                md.Close();
            }
            return values.ToString();

        }

        private string ftpSetting(WebSocketSession session, string e)
        {
            throw new NotImplementedException();
        }

        private string mvDir(WebSocketSession session, string e)
        {
            throw new NotImplementedException();
        }

        private string mkDir(WebSocketSession session, string e)
        {
            throw new NotImplementedException();
        }

        private string watchDir(WebSocketSession session, string e)
        {
            throw new NotImplementedException();
        }

        private string rmDir(WebSocketSession session, string e)
        {

            container.Resolve<FileActionRole>();

            var role = container.Resolve<FileActionRole>();


            return role.rmDir();


        }

        private string resolveFile(WebSocketSession session, string e)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 向串口发送十六进制指令
        /// 并且是否订阅串口回发的消息
        /// 此次发送后是否关闭串口
        /// </summary>
        /// <param name="session"></param>
        /// <param name="e"></param>
        /// <returns></returns>
        private string sendComCommand(WebSocketSession session, string e)
        {


            var role = container.Resolve<SerialPortActionRole>();

            RS232Bean bean = container.Resolve<RS232Bean>();


            HashMap paras = JsonConvert.DeserializeObject<HashMap>(e);


            bean.PortName = paras.GetValue<String>("PortName");

            bean.Command = paras.GetValue<String>("Command");

            role.BaseBean = bean;

            //引用当前的session，当然在websocket连接没有断开之前，一直有效


            return role.sendComCommand();

        }

        private string renameFile(WebSocketSession session, string e)
        {
            throw new NotImplementedException();
        }

        private string deleteFile(WebSocketSession session, string e)
        {
            throw new NotImplementedException();
        }


        /// <summary>
        /// 创建文件
        /// </summary>
        /// <param name="session"></param>
        /// <param name="e"></param>
        /// <returns></returns>
        private string createFile(WebSocketSession session, string e)
        {

            throw new NotImplementedException();

        }

        private string comSetting(WebSocketSession session, string e)
        {
            var role = container.Resolve<SerialPortActionRole>();


            RS232Bean bean = container.Resolve<RS232Bean>();

            HashMap paras = JsonConvert.DeserializeObject<HashMap>(e);


            try
            {

                String portName = paras.GetValue<String>("PortName");

                bean.PortName = portName;
            }
            catch (Exception er)
            {
                Tools.PubMessage(er.Message);
                bean.PortName = Tools.GetAppSetting("m_portName");
            }


            try
            {

                String parity = paras.GetValue<String>("Parity");

                bean.Parity = (Parity)Enum.Parse(typeof(Parity), parity);

            }
            catch (Exception)
            {
                bean.Parity = Parity.None;
            }


            try
            {
                int dataBits = paras.GetValue<int>("DataBits");


                bean.DataBits = dataBits;
            }
            catch (Exception)
            {
                bean.DataBits = 8;
            }


            try
            {
                String stopBits = paras.GetValue<String>("StopBits");


                bean.StopBits = (StopBits)Enum.Parse(typeof(StopBits), stopBits);
            }
            catch (Exception)
            {
                bean.StopBits = StopBits.One;
            }


            try
            {
                int boundRate = paras.GetValue<int>("boundRate");

                if (boundRate == 0)
                {
                    boundRate = 9600;
                }
                bean.BoundRate = boundRate;
            }
            catch (Exception)
            {
                bean.BoundRate = 9600;
            }


            role.BaseBean = bean;
            return role.comSetting();




        }

        private string comList(WebSocketSession session, string e)
        {


            var actionRole = container.Resolve<SerialPortActionRole>();

            String[] comListArray = actionRole.comList().Split(',');

            var responseBean = container.Resolve<RS232ResponseBean>();

            responseBean.ComList = comListArray;

            responseBean.Key = "comList";

            return JsonConvert.SerializeObject(responseBean);
        }

        private string ProcSerialPort(WebSocketSession session, string e)
        {



            HashMap parameter = JsonConvert.DeserializeObject<HashMap>(e);
            String m_portName = parameter.GetValue<String>("m_portName");

            if (String.IsNullOrEmpty(m_portName))
            {

                m_portName = Tools.GetAppSetting("m_portName");
            }


            String m_command = parameter.GetValue<String>("m_command");
            if (String.IsNullOrEmpty(m_command))
            {
                m_command = Tools.GetAppSetting("m_command");
            }


            ///如果此串口没有注册，就注册
            if (!SerialPortFactory.SerialPortPool.ContainsKey(m_portName))
            {
                int m_boundRate = parameter.GetValue<int>("m_boundRate");


                if (m_boundRate < 1)
                {
                    m_boundRate = Int32.Parse(Tools.GetAppSetting("m_boundRate"));
                }
                int m_dataBits = 8;



                Parity m_parity = Parity.None;
                StopBits m_stopBits = StopBits.One;
                SerialPortEntity entity = new SerialPortEntity(m_portName,
                    m_boundRate, m_dataBits, m_parity, m_stopBits, m_command);
                SerialPortFactory.SerialPortPool.Add(m_portName, entity);

            }

            ///否则取得注册过的串口的实体
            SerialPortEntity port = SerialPortFactory.SerialPortPool[m_portName];
            if (!port.Port.IsOpen)
            {
                port.Port.Open();

            }

            ///不再等待，将sessionID推送到当前串口，从dataTeceived事件取回结果，推送回sessionID
            ///实际多少时间就是多少时间

            SerialPortEntity.currentSession = session;

            port.SendCommand(m_command);

            return "{}";

        }

        private string ProcIp(WebSocketSession session, string e)
        {

            String[] ip = Tools.GetLocalIp();
            String ipStr = "";
            Parallel.For(0, ip.Length, i =>
            {
                ipStr += ip[i] + ",";
            });
            ipStr += ip[0] + ",";
            HashMap map = new HashMap();
            map.Add("httpServer", ipStr);
            map.Add("websocketserver", ipStr);
            map.Add("key", "httpServer");
            return JsonConvert.SerializeObject(map);
        }




        private string BurnSoftDown(string e)
        {
            return null;

        }


        public static String DownLoadBurnSoftPath = "";
        public static String DownLoadfilePath = "";
        public static String DownLoadfileName = "";


        public void StartDownLoad()
        {
            ShowSaveFileDialog(DownLoadBurnSoftPath, DownLoadfilePath, DownLoadfileName);
        }

        public static bool SavePhotoFromUrl(String path, string FileName, string Url)
        {
            bool Value = false;
            WebResponse response = null;
            Stream stream = null;

            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);

                response = request.GetResponse();
                stream = response.GetResponseStream();

                if (!response.ContentType.ToLower().StartsWith("text/"))
                {
                    Value = SaveBinaryFile(path, response, FileName);

                }

            }
            catch (Exception err)
            {
                string aa = err.ToString();
            }
            return Value;
        }
        /// <summary>
        /// Save a binary file to disk.
        /// </summary>
        /// <param name="response">The response used to save the file</param>
        // 将二进制文件保存到磁盘
        private static bool SaveBinaryFile(String path, WebResponse response, string FileName)
        {
            bool Value = true;
            byte[] buffer = new byte[1024];

            try
            {
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }


                DeleteFiles(path);


                if (File.Exists(FileName))
                    File.Delete(FileName);
                Stream outStream = System.IO.File.Create(FileName);
                Stream inStream = response.GetResponseStream();

                int l;
                do
                {
                    l = inStream.Read(buffer, 0, buffer.Length);
                    if (l > 0)
                        outStream.Write(buffer, 0, l);
                }
                while (l > 0);

                outStream.Close();
                inStream.Close();
            }
            catch
            {
                Value = false;
            }

            if (Value)
            {

                MessageBox.Show("烧录软件已经下载成功\r\n下载路径为：" + FileName);
            }
            else
            {
                MessageBox.Show("烧录软件已经下载失败");
            }

            return Value;
        }


        #region 保存对话框
        private void ShowSaveFileDialog(String remoteFile, String localPath, String fileName)
        {

            SavePhotoFromUrl(localPath, localPath + "\\" + fileName, remoteFile);

        }

        #endregion



        /// <summary>
        /// 直接删除目录下的所有文件及文件夹(保留目录) 
        /// </summary>
        /// <param name="strDir">目录地址 </param>
        public static void DeleteFiles(string filePath)
        {
            if (Directory.Exists(filePath))
            {
                string[] strDirs = Directory.GetDirectories(filePath);
                //目录下的文件
                string[] strFiles = Directory.GetFiles(filePath);

                foreach (string strFile in strFiles)
                {
                    File.Delete(strFile);
                }

                foreach (string strdir in strDirs)
                {
                    Directory.Delete(strdir, true);
                }

            }
            else
            {

                return;
            }
        }








        static String connectionString = ConfigurationManager.AppSettings["conn"];






        public static string ProcPrintJob(string e, ref int errorCode, ref String errorMessage)
        {
            LabelPrint print = new LabelPrint();
            HashMap map = JsonConvert.DeserializeObject<HashMap>(e);
            String LabelContent = map["labelJsonData"].ToString();
            String printer = map.GetValue<String>("printName");

            //如果设置了全局当前打印机，就用当前打印机打印
            //提供web修改接口
            if (Setting.globalPrinter != null && Setting.globalPrinter != "")
            {
                printer = Setting.globalPrinter;
            }
            String tempatePath = map.GetValue<String>("tempatePath");
            int copys = map.GetValue<int>("copys");
            try
            {
                PrintX.LeanMES.Plugin.LabelPrint.PrintHlper.Copys = copys;
            }
            catch (Exception err)
            {
                Console.WriteLine(err.Message);
                copys = 1;
            }

            int linkFlag = 0;

            try
            {
                map.GetValue<int>("linkFlag");
            }
            catch (Exception err)
            {
                Console.WriteLine(err.Message);
                linkFlag = 1;
            }

            Tools.PubMessage("是否连板--------" + linkFlag);
            if (linkFlag < 1)
            {
                //连扳标记
                linkFlag = 0;

            }
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

                print.PrintLabelUseCodeSoft9(tempatePath, LabelContent, copys, "cn", printer, ref errorCode,

                ref errorMessage, linkFlag);

            }
            catch (Exception er)
            {
                Tools.PubMessage("发生异常咯--------" + er.Message);
                errorMessage = er.Message;
                errorCode = -1;
            }


            //重置打印份数
            PrintX.LeanMES.Plugin.LabelPrint.PrintHlper.Copys = 1;



            //组装返回结果
            HashMap responseBean = new HashMap();

            if (errorCode == 0)
            {
                errorMessage = "打印成功";
            }
            responseBean.Add("errorMessage", errorMessage);
            responseBean.Add("errorCode", errorCode);
            responseBean.Add("key", "PrintJob");


            return JsonConvert.SerializeObject(responseBean);

        }


        private string ProcGetPrinterList()
        {
            List<String> list = Printer.GetLocalPrinters();
            HashMap result = new HashMap();
            result.Add("key", "printerList");
            result.Add("printerList", list);

            return JsonConvert.SerializeObject(result);
        }

        protected void WebSocketServer_NewDataReceived(WebSocketSession session, byte[] e)
        {
            session.Send(e, 0, e.Length);
        }


        private void button1_Click(object sender, EventArgs e)
        {

        }


        void OpenWebSocketClient()
        {
            websocket.Opened += new EventHandler(websocket_Opened);
            websocket.Closed += new EventHandler(websocket_Closed);
            websocket.MessageReceived += new EventHandler<MessageReceivedEventArgs>(websocket_MessageReceived);
            websocket.Open();

        }


        private void websocket_Opened(object sender, EventArgs e)
        {
            websocket.Send("{\"key\":\"BroadCastMessage\"}");

        }


        private void websocket_Error(object sender, EventArgs e)
        {
            websocket.Close();
        }


        private void websocket_Closed(object sender, EventArgs e)
        {
            websocket.Dispose();
        }

        private void websocket_MessageReceived(Object sender, MessageReceivedEventArgs e)
        {

            //   websocket.Send(e.Message);
        }


        public static HashMap sessionList = new HashMap();

        private EnumMessageMap getRequestNo(string e,String sessionId)
        {
            HashMap map = JsonConvert.DeserializeObject<HashMap>(e);
            String key = map.GetValue<String>("key");
            EnumMessageMap enNo = (EnumMessageMap)Enum.Parse(typeof(EnumMessageMap), key);

            if (map.ContainsKey("pubmessage"))
            {
                if (!sessionList.ContainsKey(sessionId))
                {
                    sessionList.Add(sessionId, e);
                }
            }

           return enNo;
        }

        private void button3_Click(object sender, EventArgs e)
        {

        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            System.Environment.Exit(System.Environment.ExitCode);
            this.Dispose();
            this.Close();
            GC.Collect();
        }
    }

    public interface IBindesh
    {
        string encode(string str);
        string decode(string str);
    }

    public class EncryptionDecryption : IBindesh
    {
        public string encode(string str)
        {
            string htext = "";

            for (int i = 0; i < str.Length; i++)
            {
                htext = htext + (char)(str[i] + 10 - 1 * 2);
            }
            return htext;
        }

        public string decode(string str)
        {
            string dtext = "";

            for (int i = 0; i < str.Length; i++)
            {
                dtext = dtext + (char)(str[i] - 10 + 1 * 2);
            }
            return dtext;
        }
    }
}
