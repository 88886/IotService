using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/// <summary>
/// item
/// 2017-11-9
/// 防火防盗防败家娘们儿
/// </summary>
namespace PrintX.LeanMES.Plugin.GenericPrintView
{
    public class DocumentX : MetaDataInfo
    {

        /// <summary>
        /// 文档附件信息
        /// </summary>
        AttachInfoX attachInfoX;
        private String accthcInfoXIndex;


        /// <summary>
        /// 文档内容，内容数据源索引
        /// </summary>
        IList<IList<DocumentXElement>> elementGroupList;

        private String elementGroupListIndex;


        public AttachInfoX AttachInfoX
        {
            get
            {
                return attachInfoX;
            }

            set
            {
                attachInfoX = value;
            }
        }

        public string ElementGroupListIndex
        {
            get
            {
                return elementGroupListIndex;
            }

            set
            {
                elementGroupListIndex = value;
            }
        }

        public IList<IList<DocumentXElement>> ElementList
        {
            get
            {
                return elementGroupList;
            }

            set
            {
                elementGroupList = value;
            }
        }

        public string AccthcInfoXIndex
        {
            get
            {
                return accthcInfoXIndex;
            }

            set
            {
                accthcInfoXIndex = value;
            }
        }
    }
}
