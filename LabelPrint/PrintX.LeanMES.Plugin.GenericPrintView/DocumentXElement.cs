using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/// <summary>
/// item
/// 2017-11-9
/// 文档标题实体
/// </summary>
namespace PrintX.LeanMES.Plugin.GenericPrintView
{
    public class DocumentXElement:MetaDataInfo
    {
      

        /// <summary>
        /// 标题内容和索引
        /// </summary>
        private String content;
        private String contentIndex;


        /// <summary>
        /// 元素样式
        /// </summary>
        private DocumentXStyle documentXStyle;


        public string Content
        {
            get
            {
                return content;
            }

            set
            {
                content = value;
            }
        }

        public string ContentIndex
        {
            get
            {
                return contentIndex;
            }

            set
            {
                contentIndex = value;
            }
        }

        public DocumentXStyle DocumentXStyle
        {
            get
            {
                return documentXStyle;
            }

            set
            {
                documentXStyle = value;
            }
        }
    }
}
