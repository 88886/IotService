using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PrintX.LeanMES.Plugin.UI.Listeners.Base;

namespace PrintX.LeanMES.Plugin.UI.Listeners.Service
{
    class DownLoadModule : BaseModule
    {

        public DownLoadModule()
        {

            Post["/download/"] = Parameter =>
            {

                return null;

            };

        }
    }
}
