using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PrintX.LeanMES.Plugin.GenericPrintView
{
    public class MetaDataInfo
    {
        /// <summary>
        /// 单元的索引
        /// </summary>
        private String index;

        public string Index
        {
            get
            {
                return index;
            }

            set
            {
                index = value;
            }
        }
    }
}
