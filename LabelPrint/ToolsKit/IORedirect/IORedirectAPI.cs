using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using NPOI.HSSF.UserModel;

namespace SKT.Dev.Utils.ToolsKit
{
    public class IORedirectAPI 
    {
        /// <summary>
        /// DataTable导出到Excel
        /// </summary>
        /// <param name="table">表数据</param>
        /// <param name="filename">生成的表格的名称</param> 
        /// <param name="documentType">生成的文档的类型:text/csv,text/xls等</param> 
        /// <param name="charset">编码</param>
        /// <param name="afterPrefix">后缀：xls</param> 
        public static void ExportToSpreadsheet(DataTable table, string filename, String documentType, String charset, String afterPrefix)
        {
            filename = filename + "." + afterPrefix;
            HttpContext.Current.Response.Charset = charset;
            System.Web.HttpContext.Current.Response.ContentEncoding = System.Text.Encoding.GetEncoding(charset);
            //设置输出文件类型为excel文件
            HttpContext.Current.Response.ContentType = documentType;

            HttpContext.Current.Response.AppendHeader("Content-Disposition", "attachment;filename=" + "" + HttpUtility.UrlEncode(filename, System.Text.Encoding.UTF8).ToString());

            //设置数字为文本格式
            string strStyle = "<style>td{mso-number-format:\"\\@\";}</style>";
            //定义StringWriter输出对象
            System.IO.StringWriter tw = new System.IO.StringWriter();
            //定义HtmlTextWriter对象
            HtmlTextWriter hw = new HtmlTextWriter(tw);

            hw.WriteLine(strStyle);
            GridView gv = new GridView();
            gv.DataSource = table;
            gv.DataBind();
            gv.RenderControl(hw);
            //输出
            HttpContext.Current.Response.Write("<html><head><meta http-equiv=Content-Type content=\"text/html; charset=gb2312\">");
            HttpContext.Current.Response.Write(tw.ToString());
            HttpContext.Current.Response.Flush();
            HttpContext.Current.Response.End();
            HttpContext.Current.Response.Write("</body></html>");
        }




     



        public static void DataSetToLocalExcel(DataSet ds, string outputPath)
        {
            if (ds == null || ds.Tables[0] == null && ds.Tables[0].Rows.Count == 0) { return; }

            DataTable dt2 = ds.Tables[0];
         
        }
     
    }
}
