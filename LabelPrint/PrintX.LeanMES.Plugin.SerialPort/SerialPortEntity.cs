using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.Ports;
using System.Threading;
using SuperWebSocket;
using Newtonsoft.Json;

namespace PrintX.LeanMES.Plugin.SerialPort
{
    public class SerialPortEntity
    {



        public static String SessionID = null;

        private SerialPortParameter m_serialPortParameter;

        public SerialPortParameter SerialPortParameter
        {
            get { return m_serialPortParameter; }
            set { m_serialPortParameter = value; }
        }


        private System.IO.Ports.SerialPort m_port;

        public System.IO.Ports.SerialPort Port
        {
            get { return m_port; }
            set { m_port = value; }
        }


        /// <summary>
        /// 发送串口
        /// </summary>
        /// <param name="m_portName"></param>
        /// <param name="m_boundRate"></param>
        /// <param name="m_dataBits"></param>
        /// <param name="m_parity"></param>
        /// <param name="m_stopBits"></param>
        /// <param name="m_command"></param>
        public SerialPortEntity(String m_portName,
            int m_boundRate, int m_dataBits, Parity m_parity,
            StopBits m_stopBits, String m_command)
        {

            m_serialPortParameter = new SerialPort.SerialPortParameter();
            m_serialPortParameter.BoundRate = m_boundRate;
            m_serialPortParameter.DataBits = m_dataBits;
            m_serialPortParameter.Parity = m_parity;
            m_serialPortParameter.StopBits = m_stopBits;
            m_serialPortParameter.PortName = m_portName;

            m_serialPortParameter.Command = m_command;

            m_port = new System.IO.Ports.SerialPort();
            m_port.BaudRate = m_serialPortParameter.BoundRate;
            m_port.DataBits = m_serialPortParameter.DataBits;
            m_port.Parity = m_serialPortParameter.Parity;
            m_port.StopBits = m_serialPortParameter.StopBits;
            m_port.PortName = m_serialPortParameter.PortName;
            m_port.DtrEnable = true;
            m_port.RtsEnable = true;
            m_port.ReceivedBytesThreshold = 1;
            m_port.ReadTimeout = 5000;
            m_port.WriteTimeout = 5000;


            m_port.DataReceived += f_dataReceived;







        }



        /// <summary>
        /// 一级监控
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void SerialPortEntity_ElasedMaster(object sender, System.Timers.ElapsedEventArgs e)
        {
           
        }



        /// <summary>
        /// 二级监控
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void SerialPortEntity_ElapsedSlave(object sender, System.Timers.ElapsedEventArgs e)
        {
            foreach (var obj in currentSessionPool)
            {
                var session=obj.Value;

                if (null == session || !session.Connected)
                {

                    currentSessionPool.Remove(obj.Key);
                }

            }
        }



        public static WebSocketSession currentSession;



        public static Dictionary<String, WebSocketSession> currentSessionPool = new Dictionary<string, WebSocketSession>();



     

        /// <summary>
        /// 串口接收数据事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void f_dataReceived(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
        {
            Thread.Sleep(300);
            receiveContent = m_port.ReadExisting().Replace("\r\n", "");


            foreach (WebSocketSession session in currentSessionPool.Values)
            {
                if (session != null && session.Connected)
                {

                    //推送回发送了获取重量指令的client,也就是订阅了获取重量
                    //消息的client
                    Dictionary<String, Object> pushData = new Dictionary<string, object>();
                    pushData.Add("key", "com_weighting");
                    //主要用于称重，兼容旧的业务以及新的扩展
                    //多客户端推送

                    pushData.Add("Key", "comSendCommand");

                    pushData.Add("weighting", receiveContent);
                    pushData.Add("value", receiveContent);

                    String jsonStr = JsonConvert.SerializeObject(pushData);
                    session.Send(jsonStr);


                }

            }

        }


        private String receiveContent;

        public String ReceiveContent
        {
            get { return receiveContent; }
            set { receiveContent = value; }
        }



        /// <summary>
        /// 向串口发送指令
        /// </summary>
        /// <param name="command"></param>
        public void SendCommand(String command)
        {

            string hexCommand = tool.GetHexString(command);
            byte[] datat = tool.GetByteData(hexCommand);
            m_port.DiscardInBuffer();
            m_port.DiscardOutBuffer();
            m_port.Write(datat, 0, datat.Length);
        }


    }
}
