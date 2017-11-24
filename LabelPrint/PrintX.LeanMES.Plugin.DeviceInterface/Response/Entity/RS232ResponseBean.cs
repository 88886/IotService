using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PrintX.LeanMES.Plugin.UI.Response.Entity
{
    class RS232ResponseBean:BaseResponseBean
    {

        String[] comList;

        public String[] ComList
        {
            get { return comList; }
            set { comList = value; }
        }





    }
}
