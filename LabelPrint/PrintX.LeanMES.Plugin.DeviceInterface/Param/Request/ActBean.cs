using Newtonsoft.Json;
using PrintX.LeanMES.Plugin.UI.MessageMap;
using PrintX.LeanMES.Plugin.UI.Response.Entity;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace PrintX.LeanMES.Plugin.UI.Param.Request
{

    /// <summary>
    /// 三菱PLC实体类
    /// </summary>
    public class ActBean
    {

        private String dir;

        ActResponseBean actResponseBean;

        public String Dir
        {
            get { return dir; }
            set { dir = value; }
        }


        /// <summary>
        /// 移除本地目录
        /// </summary>
        /// <returns></returns>
        internal String rmDir()
        {

            if (!Directory.Exists(Dir))
            {
                actResponseBean.ErrorCode = -1;
                actResponseBean.ErrorMessage = EnumMessageMap.DirNotFoqund.ToString();
            }

            Directory.Delete(Dir);

            actResponseBean.ErrorCode = 0;
            actResponseBean.ErrorMessage = EnumMessageMap.OperationSuccessfully.ToString();

            return JsonConvert.SerializeObject(actResponseBean);
        }
    }
}
