using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PrintX.LeanMES.Plugin.SerialPort
{
   public class SerialPortFactory
    {
       //定义串口池，以及串口池配置
       //每一个串口配置为一个

       private static Dictionary<String, SerialPortEntity> m_serialPortPool = new Dictionary<string, SerialPortEntity>();

       public static Dictionary<String, SerialPortEntity> SerialPortPool
        {
            get { return SerialPortFactory.m_serialPortPool; }
            set { SerialPortFactory.m_serialPortPool = value; }
        }

    }
}
