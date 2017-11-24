using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PrintX.LeanMES.Plugin.UI.Interface
{
    interface IPlc
    {


        /// <summary>
        /// 通过modbus协议读取plc寄存器的值
        /// </summary>
        /// <returns></returns>
         String modbusInteract();
      



        /// <summary>
        /// 启动三菱plc监控
        /// </summary>
        /// <returns></returns>
          String StartSLPLC();



        /// <summary>
        /// 停止三菱plc监控
        /// </summary>
        /// <returns></returns>
         String stopSLPlc();
      
    }
}
