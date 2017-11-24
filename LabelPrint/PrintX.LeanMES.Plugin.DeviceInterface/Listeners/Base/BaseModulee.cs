using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Nancy;


namespace PrintX.LeanMES.Plugin.UI.Listeners.Base
{
    /// <summary>
    /// 基础监听模块
    /// </summary>
    public class BaseModule : NancyModule
    {

        public String GetBodyRaw()
        {
            // discover the body as a raw string
            byte[] b = new byte[this.Request.Body.Length];
            this.Request.Body.Read(b, 0, Convert.ToInt32(this.Request.Body.Length));
            System.Text.UTF8Encoding encoding = new System.Text.UTF8Encoding();
            String bodyData = encoding.GetString(b);
            return bodyData;
        }
    }
}
