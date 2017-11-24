using PrintX.LeanMES.Plugin.UI.Param;
using PrintX.LeanMES.Plugin.UI.Param.Request;
using PrintX.LeanMES.Plugin.UI.Response.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PrintX.LeanMES.Plugin.UI.Controller
{


    /// <summary>
    /// 充当中介，控制访问业务
    /// </summary>
    interface IAction
    {
       
    }


    public class SerialPortActionRole : IAction
    {


        IRS232 m_BaseBean;

        public IRS232 BaseBean
        {
            get { return m_BaseBean; }
            set { m_BaseBean = value; }
        }
       
        /// <summary>
        /// RS232注入实体
        /// </summary>
        public SerialPortActionRole(RS232Bean m_Rs232Bean)
        {
            this.m_BaseBean = m_Rs232Bean;
        }

        /// <summary>
        /// FTP注入实体
        /// </summary>
  

      



      
        


       

        /// <summary>
        /// 获取串口列表
        /// </summary>
        /// <returns></returns>
       public String comList()
       {
           return m_BaseBean.comList();
       }


        /// <summary>
        /// 配置串口
        /// </summary>
        /// <returns></returns>
       public String comSetting()
       {
           return m_BaseBean.comSetting();
       }


        /// <summary>
        /// 打开串口
        /// </summary>
        /// <returns></returns>
       public String openCom()
       {
           return m_BaseBean.openCom();
       }


        /// <summary>
        /// 关闭串口
        /// </summary>
        /// <returns></returns>
        public String closeCom()
       {
           return m_BaseBean.closeCom();
       }


        /// <summary>
        /// 向串口发送指令
        /// </summary>
        /// <returns></returns>
        public String sendComCommand()
        {
            return m_BaseBean.sendComCommand();
        }



      

    }
}
