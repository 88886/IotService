using PrintX.LeanMES.Plugin.UI.Interface;
using PrintX.LeanMES.Plugin.UI.Param.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PrintX.LeanMES.Plugin.UI.Controller
{
    class FileActionRole:IFile
    {

       internal IFile actBean;

        public String rmDir()
        {
            return actBean.rmDir();
        }

    }
}
