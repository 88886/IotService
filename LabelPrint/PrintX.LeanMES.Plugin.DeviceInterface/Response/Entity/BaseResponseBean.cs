using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PrintX.LeanMES.Plugin.UI.Response.Entity
{
    /// <summary>
    /// 错误码
    /// 错误消息
    /// 0位OK
    /// 其他为错误
    /// </summary>
    class BaseResponseBean
    {

        /// <summary>
        /// 处理结果错误码
        /// </summary>
        private int errorCode;

        public int ErrorCode
        {
            get { return errorCode; }
            set { errorCode = value; }
        }



        /// <summary>
        /// 处理结果错误消息
        /// </summary>
        private String errorMessage;

        public String ErrorMessage
        {
            get { return errorMessage; }
            set { errorMessage = value; }
        }


        /// <summary>
        /// 请求服务类型，部分客户端是需要用来区分业务的
        /// </summary>
        private String key;

        public String Key
        {
            get { return key; }
            set { key = value; }
        }
    }
}
