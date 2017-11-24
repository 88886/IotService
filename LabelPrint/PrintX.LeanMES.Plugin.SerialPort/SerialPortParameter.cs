using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.Ports;

namespace PrintX.LeanMES.Plugin.SerialPort
{
   public class SerialPortParameter
    {
       /// <summary>
       /// 串口名称
       /// </summary>
        private String m_portName;

        public String PortName
        {
            get { return m_portName; }
            set { m_portName = value; }
        }

       /// <summary>
       /// 串口波特率
       /// </summary>
        private int m_boundRate;

        public int BoundRate
        {
            get { return m_boundRate; }
            set { m_boundRate = value; }
        }


       /// <summary>
       /// 串口停止位
       /// </summary>
        private StopBits m_stopBits;

        public StopBits StopBits
        {
            get { return m_stopBits; }
            set { m_stopBits = value; }
        }

       /// <summary>
       /// 串口校验位
       /// </summary>
        private Parity m_parity;

        public Parity Parity
        {
            get { return m_parity; }
            set { m_parity = value; }
        }


       /// <summary>
       /// 数据长度
       /// </summary>
        private int m_dataBits;

        public int DataBits
        {
            get { return m_dataBits; }
            set { m_dataBits = value; }
        }



      


        private String command;

        public String Command
        {
            get { return command; }
            set { command = value; }
        }

       

       




    }
}
