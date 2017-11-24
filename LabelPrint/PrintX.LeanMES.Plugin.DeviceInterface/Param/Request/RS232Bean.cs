using Microsoft.Win32;
using Newtonsoft.Json;
using PrintX.LeanMES.Plugin.SerialPort;
using PrintX.LeanMES.Plugin.UI.Response.Entity;
using PrintX.LeanMES.Plugin.UI.Tool;
using PrintX.PrintService;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;

namespace PrintX.LeanMES.Plugin.UI.Param.Request
{
    public class RS232Bean : IRS232
    {

        /// <summary>
        /// 获取串口列表
        /// </summary>
        /// <returns></returns>
        public override String comList()
        {

            StringBuilder builder = new StringBuilder();
            RegistryKey keyCom = Registry.LocalMachine.OpenSubKey("Hardware\\DeviceMap\\SerialComm");
            if (keyCom != null)
            {
                string[] sSubKeys = keyCom.GetValueNames();

                foreach (string sName in sSubKeys)
                {
                    string sValue = (string)keyCom.GetValue(sName);
                    builder.Append(sValue).Append(",");
                }
            }
            return builder.ToString().Substring(0, builder.ToString().Length - 1);
        }







        /// <summary>
        /// 配置com口参数,并且加入串口池
        /// </summary>
        /// <returns></returns>
        public override String comSetting()
        {


            SerialPortEntity entity = new SerialPortEntity(PortName,
                   BoundRate, DataBits, Parity, StopBits, Command);

            if (!SerialPortFactory.SerialPortPool.ContainsKey(PortName))
            {
                SerialPortFactory.SerialPortPool.Add(PortName, entity);
            }


            SerialPortEntity se = SerialPortFactory.SerialPortPool[PortName];


            BaseResponseBean bean = new BaseResponseBean();
            if (se != null)
            {
                bean.ErrorCode = 0;
                bean.ErrorMessage = "操作成功";
            }

            else
            {
                bean.ErrorCode = -1;
                bean.ErrorMessage = "操作失败";
            }
            bean.Key = "comSetting";

            return JsonConvert.SerializeObject(bean);

        }


        /// <summary>
        /// 打开串口
        /// </summary>
        /// <returns></returns>
        public override String openCom()
        {

            int errorCode = -1;
            String errorMessage = "";
            if (!SerialPortFactory.SerialPortPool.ContainsKey(PortName))
            {
                errorCode = -1;

                errorMessage = "串口未初始化";
            }

            else
            {
                errorCode = 0;
            }
            BaseResponseBean bean = new BaseResponseBean();
            if (errorCode == 0)
            {
                SerialPortEntity se = SerialPortFactory.SerialPortPool[PortName];


                if (!se.Port.IsOpen)
                {
                    se.Port.Open();
                    errorCode = 0;
                    errorMessage = "打开成功";
                }

                else
                {
                    errorCode = -1;
                    errorMessage = "串口已经处于打开状态";
                }



            }

            bean.ErrorCode = errorCode;
            bean.ErrorMessage = errorMessage;
            bean.Key = "openCom";

            return JsonConvert.SerializeObject(bean);
        }


        /// <summary>
        /// 关闭com口
        /// </summary>
        /// <returns></returns>
        public override String closeCom()
        {

            int errorCode = -1;
            String errorMessage = "";
            if (!SerialPortFactory.SerialPortPool.ContainsKey(PortName))
            {
                errorCode = -1;

                errorMessage = "串口未初始化";
            }

            else
            {
                errorCode = 0;
            }
            BaseResponseBean bean = new BaseResponseBean();
            if (errorCode == 0)
            {
                SerialPortEntity se = SerialPortFactory.SerialPortPool[PortName];


                if (se.Port.IsOpen)
                {
                    se.Port.Close();
                    errorCode = 0;
                    errorMessage = "关闭成功";
                }

                else
                {
                    errorCode = -1;
                    errorMessage = "串口已经处于关闭状态";
                }



            }

            bean.ErrorCode = errorCode;
            bean.ErrorMessage = errorMessage;
            bean.Key = "closeCom";

            return JsonConvert.SerializeObject(bean);
        }


        public override String sendComCommand()
        {

            if (String.IsNullOrEmpty(PortName))
            {

                PortName = Tools.GetAppSetting("m_portName");
            }


        
            if (String.IsNullOrEmpty(Command))
            {
                Command = Tools.GetAppSetting("m_command");
            }


            ///如果此串口没有注册，就注册
            if (!SerialPortFactory.SerialPortPool.ContainsKey(PortName))
            {
                int m_boundRate = 9600;


                if (m_boundRate < 1)
                {
                    m_boundRate = Int32.Parse(Tools.GetAppSetting("m_boundRate"));
                }
                int m_dataBits = 8;



                Parity m_parity = Parity.None;
                StopBits m_stopBits = StopBits.One;
                SerialPortEntity entity = new SerialPortEntity(PortName,
                    m_boundRate, m_dataBits, m_parity, m_stopBits, Command);
                SerialPortFactory.SerialPortPool.Add(PortName, entity);

            }

            ///否则取得注册过的串口的实体
            SerialPortEntity port = SerialPortFactory.SerialPortPool[PortName];


            
            if (!port.Port.IsOpen)
            {
                port.Port.Open();

            }


          

            ///不再等待，将sessionID推送到当前串口，从dataTeceived事件取回结果，推送回sessionID
            ///实际多少时间就是多少时间

  
            port.SendCommand(Command);

            BaseResponseBean bean = new BaseResponseBean();

            bean.ErrorCode = 0;
            bean.ErrorMessage = "发送成功";

            bean.Key = "comSendCommand";

            return JsonConvert.SerializeObject(bean);
            
        }


        /// <summary>
        /// 串口名称
        /// </summary>
        private String m_PortName;

        public String PortName
        {
            get { return m_PortName; }
            set { m_PortName = value; }
        }


        /// <summary>
        /// 波特率
        /// </summary>
        private int m_BoundRate;

        public int BoundRate
        {
            get { return m_BoundRate; }
            set { m_BoundRate = value; }
        }



        /// <summary>
        /// 停止位
        /// </summary>
        private StopBits m_StopBits;

        public StopBits StopBits
        {
            get { return m_StopBits; }
            set { m_StopBits = value; }
        }


        /// <summary>
        /// 校验位
        /// </summary>
        private Handshake m_HandShake;

        public Handshake HandShake
        {
            get { return m_HandShake; }
            set { m_HandShake = value; }
        }


        /// <summary>
        /// 数据位长度
        /// </summary>
        private int m_DataBits;

        public int DataBits
        {
            get { return m_DataBits; }
            set { m_DataBits = value; }
        }


        /// <summary>
        /// 校验位
        /// </summary>
        private Parity m_Parity;

        public Parity Parity
        {
            get { return m_Parity; }
            set { m_Parity = value; }
        }


        private String m_command;

        public String Command
        {
            get { return m_command; }
            set { m_command = value; }
        }





    }
}
