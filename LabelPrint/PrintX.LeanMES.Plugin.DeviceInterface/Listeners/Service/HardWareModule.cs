using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PrintX.LeanMES.Plugin.UI.Listeners.Base;
using System.IO.Ports;
using PrintX.LeanMES.Plugin.SerialPort;
using System.Threading;

namespace PrintX.LeanMES.Plugin.UI.Listeners.Service
{
    public class HardWareModule : BaseModule
    {

        /// <summary>
        /// 第一个参数指定服务类型
        /// 第二个参数指定串口名称
        /// 第三个参数指定波特率
        /// 第四个参数指定数据位
        /// 第五个参数指定校验位
        /// 第六个参数指定停止位
        /// 第七个参数指定要发送命令，
        /// 如果第七个参数为-1，就不发送
        /// </summary>
        public HardWareModule()
        {
            Get["/HardWare/m_portName/m_boundRate/m_dataBits/m_parity/m_stopBits/m_command"] =
                parameter =>
                {
                    String m_portName = parameter.m_portName;
                    String m_command = parameter.m_command;
                    ///如果此串口没有注册，就注册
                    if (!SerialPortFactory.SerialPortPool.ContainsKey(m_portName))
                    {
                        int m_boundRate = parameter.m_boundRate;
                        int m_dataBits = parameter.m_dataBits;
                        Parity m_parity = parameter.m_parity;
                        StopBits m_stopBits = parameter.m_stopBits;
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
                    port.SendCommand(m_command);
                    Thread.Sleep(500);

                    ///等待500毫秒，从dataTeceived事件取回结果
                    return port.ReceiveContent;
                   
                };
        }

    }
}
