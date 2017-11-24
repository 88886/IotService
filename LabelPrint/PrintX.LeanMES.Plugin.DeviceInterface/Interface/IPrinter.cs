using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PrintX.LeanMES.Plugin.UI.Interface
{
    interface IPrinter
    {

        /// <summary>
        /// 获取打印机列表
        /// </summary>
        /// <returns></returns>
          String printerList();
      

        /// <summary>
        /// 执行打印任务
        /// </summary>
        /// <returns></returns>
          String printJob();
    }
}
