using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PrintX.LeanMES.Plugin.UI.Param.Request
{
    /// <summary>
    /// 实现IRequestBean接口
    /// </summary>

    public  class IRS232 
    {
        /// <summary>
        /// 服务类型
        /// </summary>
        private String key;

        public virtual String Key
        {
            get { return key; }
            set { key = value; }
        }



      


        /// <summary>
        /// 获取串口列表
        /// </summary>
        /// <returns></returns>
        public virtual String comList()
        {
            return null;
        }


        /// <summary>
        /// 配置com口参数
        /// </summary>
        /// <returns></returns>
        public virtual String comSetting() { return null;}

        /// <summary>
        /// 打开串口
        /// </summary>
        /// <returns></returns>
        public virtual String openCom() { return null;}



        /// <summary>
        /// 关闭com口
        /// </summary>
        /// <returns></returns>
        public virtual String closeCom() { return null;}


        /// <summary>
        /// 向串口发送指令
        /// </summary>
        /// <returns></returns>
        public virtual String sendComCommand() { return null;}


      



    }
}
