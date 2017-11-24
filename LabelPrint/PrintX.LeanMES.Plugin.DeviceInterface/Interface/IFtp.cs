using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PrintX.LeanMES.Plugin.UI.Interface
{
    interface IFtp
    {
        /// <summary>
        /// 初始化FTP设置
        /// </summary>
        /// <returns></returns>
        String ftpSetting();


        /// <summary>
        /// 文件解析
        /// </summary>
        /// <returns></returns>
        String resolveFile();
    }
}
