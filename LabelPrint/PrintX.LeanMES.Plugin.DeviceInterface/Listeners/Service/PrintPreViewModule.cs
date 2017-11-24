using Nancy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/// <summary>
/// item 
/// 2017-11-9
/// 打印预览驱动
/// </summary>
namespace PrintX.LeanMES.Plugin.UI.Listeners.Service
{
    public class PrintPreViewModule:NancyModule
    {

        public PrintPreViewModule()
        {
            Post["/DocumentX/Setting"] = r =>
            {

                return null;
            };
        }
    }
}
