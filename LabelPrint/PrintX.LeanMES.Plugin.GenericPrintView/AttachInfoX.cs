using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/// <summary>
/// item
/// 2017-11-9
/// </summary>
namespace PrintX.LeanMES.Plugin.GenericPrintView
{

    /// <summary>
    /// 文档附加信息
    /// </summary>
    public class AttachInfoX : MetaDataInfo
    {
        /// <summary>
        /// 文件名与索引
        /// </summary>
        private String documentName;

        private String documentNameIndex;



        /// <summary>
        /// 文件编码与索引
        /// </summary>
        private String documentCharset;
        private String documentCharsetIndex;


        /// <summary>
        /// 文件作者与索引
        /// </summary>
        private String authorName;
        private String authorNameIndex;



        /// <summary>
        /// 文件大小与索引
        /// </summary>
        private String documentSize;
        private String documentSizeIndex;


        /// <summary>
        /// 文件网络路径与索引
        /// </summary>
        private String documentUrl;
        private String documentUrlIndex;


        /// <summary>
        /// 文件物理路径与索引
        /// </summary>
        private String documentPath;
        private String documentPathIndex;

        public string DocumentName
        {
            get
            {
                return documentName;
            }

            set
            {
                documentName = value;
            }
        }

        public string DocumentNameIndex
        {
            get
            {
                return documentNameIndex;
            }

            set
            {
                documentNameIndex = value;
            }
        }

        public string DocumentCharset
        {
            get
            {
                return documentCharset;
            }

            set
            {
                documentCharset = value;
            }
        }

        public string DocumentCharsetIndex
        {
            get
            {
                return documentCharsetIndex;
            }

            set
            {
                documentCharsetIndex = value;
            }
        }

        public string AuthorName
        {
            get
            {
                return authorName;
            }

            set
            {
                authorName = value;
            }
        }

        public string AuthorNameIndex
        {
            get
            {
                return authorNameIndex;
            }

            set
            {
                authorNameIndex = value;
            }
        }



        public string DocumentUrl
        {
            get
            {
                return documentUrl;
            }

            set
            {
                documentUrl = value;
            }
        }

        public string DocumentUrlIndex
        {
            get
            {
                return documentUrlIndex;
            }

            set
            {
                documentUrlIndex = value;
            }
        }

        public string DocumentPath
        {
            get
            {
                return documentPath;
            }

            set
            {
                documentPath = value;
            }
        }

        public string DocumentPathIndex
        {
            get
            {
                return documentPathIndex;
            }

            set
            {
                documentPathIndex = value;
            }
        }

        public string DocumentSizeIndex
        {
            get
            {
                return documentSizeIndex;
            }

            set
            {
                documentSizeIndex = value;
            }
        }

        public string DocumentSize
        {
            get
            {
                return documentSize;
            }

            set
            {
                documentSize = value;
            }
        }
    }
}
