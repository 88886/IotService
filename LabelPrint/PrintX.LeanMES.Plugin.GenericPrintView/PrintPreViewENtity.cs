using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PrintX.LeanMES.Plugin.GenericPrintView
{
    public class PrintPreViewEntity
    {
        /// <summary>
        /// 业务流程驱动者，业务的受力者
        /// </summary>
        private String bussinessNo;


        /// <summary>
        /// 业务的受力者，这里显得冗余，保留
        /// </summary>
        private String itemCode;

        /// <summary>
        /// 业务的施力者
        /// </summary>
        private String documentXIndex;

        public string BussinessNo
        {
            get
            {
                return bussinessNo;
            }

            set
            {
                bussinessNo = value;
            }
        }

        public string DocumentXIndex
        {
            get
            {
                return documentXIndex;
            }

            set
            {
                documentXIndex = value;
            }
        }

        public string ItemCode
        {
            get
            {
                return itemCode;
            }

            set
            {
                itemCode = value;
            }
        }
    }
}
