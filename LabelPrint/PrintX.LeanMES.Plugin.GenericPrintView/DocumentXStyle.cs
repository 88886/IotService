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
    public class DocumentXStyle:MetaDataInfo
    {
        public String width
        {
            get;
            set;
        }


        public String height
        {
            get;
            set;
        }

        public String background
        {
            get;
            set;
        }

        public String boarder
        {
            get;
            set;
        }

        public String font
        {
            get;
            set;
        }


        public String margin
        {
            get;
            set;
        }

        
    }

}
