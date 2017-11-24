using System;
using System.Collections.Generic;
using System.Text;
using System.IO.Ports;
using System.Threading;
using SuperWebSocket;
using WebSocket4Net;

namespace PrintX.LeanMES.Plugin.ModLib
{
    public class modbus
    {
        public static SerialPort sp;
        public string modbusStatus;



        #region Constructor / Deconstructor
        public modbus()
        {

            //双重校验
            if (sp == null)
            {
                lock (new Object())
                {

                    if (sp == null)
                    {
                        sp = new SerialPort();
                    }
                }
            }
        }
        ~modbus()
        {
        }
        #endregion

        #region Open / Close Procedures
        public bool Open(string portName, int baudRate, int databits, Parity parity, StopBits stopBits)
        {
            //Ensure port isn't already opened:
            try
            {
                if (sp != null && sp.IsOpen)
                {
                    sp.Close();

                }
            }
            catch (Exception err)
            {
                Console.WriteLine(err.Message);
            }


            if (!sp.IsOpen)
            {
                //Assign desired settings to the serial port:
                sp.PortName = portName;
                sp.BaudRate = baudRate;
                sp.DataBits = databits;
                sp.Parity = parity;
                sp.StopBits = stopBits;
                sp.ReceivedBytesThreshold = 1;
                //These timeouts are default and cannot be editted through the class at this point:
                sp.ReadTimeout = 3000;
                sp.WriteTimeout = 1000;

                sp.DtrEnable = true;
                sp.RtsEnable = true;
                //sp.DataReceived += sp_DataReceived;

                try
                {
                    sp.Open();
                }
                catch (Exception err)
                {
                    modbusStatus = "Error opening " + portName + ": " + err.Message;
                    return false;
                }
                modbusStatus = portName + " opened successfully";
                return true;
            }
            else
            {
                modbusStatus = portName + " already opened";
                return false;
            }
        }

        WebSocket ws;


        void sp_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {

            try
            {
                Thread.Sleep(WaitTime);
                byte[] readBuffer = new byte[8];

                int bytesToRead = sp.BytesToRead;
                int i = 0;

                while (i < readBuffer.Length)
                {
                    readBuffer[i] = (byte)sp.ReadByte();
                }

                //
                CheckResponse(readBuffer);

                ws = new WebSocket("ws://127.0.0.1:2012");

                OpenWebSocketClient();



                String message = HexStringToString(byteToHexStr(readBuffer), Encoding.ASCII);


                ws.Send(message);

            }
            catch (Exception err)
            {
                Console.WriteLine(err.Message);
            }

        }


        public static string byteToHexStr(byte[] bytes)
        {
            string returnStr = "";
            if (bytes != null)
            {
                for (int i = 0; i < bytes.Length; i++)
                {
                    returnStr += bytes[i].ToString("X2");
                }
            }
            return returnStr;
        }


        private string HexStringToString(string hs, Encoding encode)
        {
            //以%分割字符串，并去掉空字符
            string[] chars = hs.Split(new char[] { '%' }, StringSplitOptions.RemoveEmptyEntries);
            byte[] b = new byte[chars.Length];
            //逐个字符变为16进制字节数据
            for (int i = 0; i < chars.Length; i++)
            {
                b[i] = Convert.ToByte(chars[i], 16);
            }
            //按照指定编码将字节数组变为字符串
            return encode.GetString(b);
        }
        public bool Close()
        {
            //Ensure port is opened before attempting to close:
            if (sp.IsOpen)
            {
                try
                {
                    sp.Close();
                }
                catch (Exception err)
                {
                    modbusStatus = "Error closing " + sp.PortName + ": " + err.Message;
                    return false;
                }
                modbusStatus = sp.PortName + " closed successfully";
                return true;
            }
            else
            {
                modbusStatus = sp.PortName + " is not open";
                return false;
            }
        }
        #endregion

        #region CRC Computation
        private void GetCRC(byte[] message, ref byte[] CRC)
        {
            //Function expects a modbus message of any length as well as a 2 byte CRC array in which to 
            //return the CRC values:

            ushort CRCFull = 0xFFFF;
            byte CRCHigh = 0xFF, CRCLow = 0xFF;
            char CRCLSB;

            for (int i = 0; i < (message.Length) - 2; i++)
            {
                CRCFull = (ushort)(CRCFull ^ message[i]);

                for (int j = 0; j < 8; j++)
                {
                    CRCLSB = (char)(CRCFull & 0x0001);
                    CRCFull = (ushort)((CRCFull >> 1) & 0x7FFF);

                    if (CRCLSB == 1)
                        CRCFull = (ushort)(CRCFull ^ 0xA001);
                }
            }
            CRC[1] = CRCHigh = (byte)((CRCFull >> 8) & 0xFF);
            CRC[0] = CRCLow = (byte)(CRCFull & 0xFF);
        }
        #endregion

        #region Build Message
        private void BuildMessage(byte address, byte type, ushort start, ushort registers, ref byte[] message)
        {
            //Array to receive CRC bytes:
            byte[] CRC = new byte[2];

            message[0] = address;
            message[1] = type;
            message[2] = (byte)(start >> 8);
            message[3] = (byte)start;
            message[4] = (byte)(registers >> 8);
            message[5] = (byte)registers;

            GetCRC(message, ref CRC);
            message[message.Length - 2] = CRC[0];
            message[message.Length - 1] = CRC[1];
        }
        #endregion

        #region Check Response
        private bool CheckResponse(byte[] response)
        {
            //Perform a basic CRC check:
            byte[] CRC = new byte[2];
            GetCRC(response, ref CRC);
            if (CRC[0] == response[response.Length - 2] && CRC[1] == response[response.Length - 1])
                return true;
            else
                return false;
        }
        #endregion

        #region Get Response
        private void GetResponse(ref byte[] response)
        {
            //There is a bug in .Net 2.0 DataReceived Event that prevents people from using this
            //event as an interrupt to handle data (it doesn't fire all of the time).  Therefore
            //we have to use the ReadByte command for a fixed length as it's been shown to be reliable.
            for (int i = 0; i < response.Length; i++)
            {
                response[i] = (byte)(sp.ReadByte());
            }
        }
        #endregion

        /// <summary>
        /// 订阅消息的方式
        /// </summary>
        private static String subscribeWay;

        public static String SubscribeWay
        {
            get { return subscribeWay; }
            set { subscribeWay = value; }
        }



        /// <summary>
        /// 发送指令之后的最长等待时间
        /// </summary>
        private static int waitTime;

        public static int WaitTime
        {
            get { return waitTime; }
            set { waitTime = value; }
        }


        /// <summary>
        /// 订阅标记
        /// </summary>
        private static bool subscribeFLag;

        public static bool SubscribeFLag
        {
            get { return modbus.subscribeFLag; }
            set { modbus.subscribeFLag = value; }
        }



        #region
        public bool SendFc16NewVersion(byte address, ushort start, ushort registers, short[] values
          )
        {
            //Ensure port is open:
            if (sp.IsOpen)
            {
                //Clear in/out buffers:
                sp.DiscardOutBuffer();
                sp.DiscardInBuffer();
                //Message is 1 addr + 1 fcn + 2 start + 2 reg + 1 count + 2 * reg vals + 2 CRC
                byte[] message = new byte[9 + 2 * registers];
                //Function 16 response is fixed at 8 bytes
                byte[] response = new byte[8];

                //Add bytecount to message:
                message[6] = (byte)(registers * 2);
                //Put write values into message prior to sending:
                for (int i = 0; i < registers; i++)
                {
                    message[7 + 2 * i] = (byte)(values[i] >> 8);
                    message[8 + 2 * i] = (byte)(values[i]);
                }
                //Build outgoing message:
                BuildMessage(address, (byte)16, start, registers, ref message);

                //Send Modbus message to Serial Port:
                try
                {
                    sp.Write(message, 0, message.Length);
                }
                catch (Exception err)
                {
                    modbusStatus = "Error in write event: " + err.Message;
                    return false;
                }

                if (SubscribeWay == "发送之后阻塞直到获取返回")
                {
                    Thread.Sleep(waitTime);
                    GetResponse(ref response);

                    //Evaluate message:
                    if (CheckResponse(response))
                    {
                        modbusStatus = "Write successful";
                        return true;
                    }
                    else
                    {
                        modbusStatus = "CRC error";
                        return false;
                    }

                }

                else
                {

                    //subscribe the events
                    if (!SubscribeFLag)
                    {
                        SubscribeFLag = true;
                        sp.DataReceived += sp_DataReceived;
                    }


                    return true;
                }
            }
            else
            {
                modbusStatus = "Serial port not open";
                return false;
            }
        }
        #endregion

        #region Function 3 - Read Registers
        public bool SendFc3NewVersion(byte address, ushort start, ushort registers, ref short[] values)
        {
            //Ensure port is open:
            if (sp.IsOpen)
            {
                //Clear in/out buffers:
                sp.DiscardOutBuffer();
                sp.DiscardInBuffer();
                //Function 3 request is always 8 bytes:
                byte[] message = new byte[8];
                //Function 3 response buffer:
                byte[] response = new byte[5 + 2 * registers];
                //Build outgoing modbus message:
                BuildMessage(address, (byte)3, start, registers, ref message);
                //Send modbus message to Serial Port:
                try
                {
                    sp.Write(message, 0, message.Length);
                    //写入指令以后，获取返回值


                }
                catch (Exception err)
                {
                    modbusStatus = "Error in read event: " + err.Message;
                    return false;
                }
                //Evaluate message:


                if (SubscribeWay == "发送之后阻塞直到获取返回")
                {
                    Thread.Sleep(WaitTime);
                    GetResponse(ref response);
                    if (CheckResponse(response))
                    {
                        //Return requested register values:
                        for (int i = 0; i < (response.Length - 5) / 2; i++)
                        {
                            values[i] = response[2 * i + 3];
                            values[i] <<= 8;
                            values[i] += response[2 * i + 4];
                        }

                        return true;
                    }
                    else
                    {

                        return false;
                    }
                }
                else
                {
                    //subscribe the events
                    if (!SubscribeFLag)
                    {
                        SubscribeFLag = true;
                        sp.DataReceived += sp_DataReceived;
                    }

                    return true;
                }
            }
            else
            {
                modbusStatus = "Serial port not open";
                return false;
            }

        }
        #endregion




        protected void WebSocketServer_NewDataReceived(WebSocketSession session, byte[] e)
        {
            session.Send(e, 0, e.Length);
        }

        void OpenWebSocketClient()
        {
            ws.Opened += new EventHandler(websocket_Opened);
            ws.Closed += new EventHandler(websocket_Closed);
            ws.MessageReceived += new EventHandler<MessageReceivedEventArgs>(websocket_MessageReceived);
            ws.Open();

        }


        private void websocket_Opened(object sender, EventArgs e)
        {
            ws.Send("{\"key\":\"commandType\"}");

        }


        private void websocket_Error(object sender, EventArgs e)
        {
            ws.Close();
        }


        private void websocket_Closed(object sender, EventArgs e)
        {
            ws.Dispose();
        }

        private void websocket_MessageReceived(Object sender, MessageReceivedEventArgs e)
        {
            String message = e.Message;
            if (!string.IsNullOrEmpty(message))
            {
                if (message == "ok")
                {
                    ws.Close();
                }
            }
            //   websocket.Send(e.Message);
        }

    }
}
