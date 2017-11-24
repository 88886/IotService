
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;

using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Data.OleDb;




/*
 *深科特
 *Excel库
 *Item
 *2016-9-4
 * 
 * 
 * 一类是基于NPOI操作
 * 一类是基于Linq操作
 */
namespace SKT.Dev.Utils.ToolsKit
{
    public class ExcelAPI : IDisposable
    {
        private string fileName = null;
        private IWorkbook workbook = null;
        private FileStream fs = null;
        private bool disposed;

        
        public ExcelAPI(string fileName)
        {
            this.fileName = fileName;
            disposed = false;
        }


        private static Dictionary<String, int> CellTypeDIc = new Dictionary<string, int>() { { "STRING", 1 }, { "BOOLEAN", 2 }, { "DATETIME", 3 }, { "NUMERIC", 4 }, { "RICHTEXT", 5 } };


        public int DataTableToExcel(DataTable data, string sheetName, bool isColumnWritten)
        {
            int i = 0;
            int j = 0;
            int count = 0;
            ISheet sheet = null;

            fs = new FileStream(fileName, FileMode.OpenOrCreate, FileAccess.ReadWrite);
            if (fileName.IndexOf(".xlsx") > 0)
                workbook = new XSSFWorkbook();
            else if (fileName.IndexOf(".xls") > 0)
                workbook = new HSSFWorkbook();

            try
            {
                if (workbook != null)
                {
                    sheet = workbook.CreateSheet(sheetName);

                }
                else
                {
                    return -1;
                }

                if (isColumnWritten == true)
                {
                    IRow row = sheet.CreateRow(0);
                    for (j = 0; j < data.Columns.Count; ++j)
                    {
                        row.CreateCell(j).SetCellValue(data.Columns[j].ColumnName);
                    }
                    count = 1;
                }
                else
                {
                    count = 0;
                }

                for (i = 0; i < data.Rows.Count; ++i)
                {
                    IRow row = sheet.CreateRow(count);
                    for (j = 0; j < data.Columns.Count; ++j)
                    {
                        if (data.Rows[i][j] != null)
                        {
                            row.CreateCell(j).SetCellValue(data.Rows[i][j].ToString());
                        }
                        else
                        {
                            row.CreateCell(j).SetCellValue("-1");
                        }

                    }
                    ++count;
                }
                workbook.Write(fs); //写入到excel
                return count;
            }
            catch (Exception)
            {

                return -1;
            }
            finally
            {
                fs.Close();
                fs.Dispose();
            }
        }




        public bool ClassListToExcel<T>(List<T> list, String sheetName, bool writeColumnName)
        {
            DataTable dt = ConvertToDataSet(list).Tables[0];

            return DataTableToExcel(dt, sheetName, writeColumnName) > 0;
        }


        public bool DictionaryListToExcel(List<Dictionary<String, String>> listDic, String sheetName, bool isColumnWritten)
        {
            try
            {
                DataTable dt = ConvertToDataSet(listDic).Tables[0];
                return DataTableToExcel(dt, sheetName, isColumnWritten) > 0;
            }
            catch (Exception err)
            {

                return false;
            }


        }


        public DataSet ConvertToDataSet<T>(IList<T> list)
        {
            if (list == null || list.Count <= 0)
            {
                return null;
            }

            DataSet ds = new DataSet();
            DataTable dt = new DataTable(typeof(T).Name);
            DataColumn column;
            DataRow row;

            System.Reflection.PropertyInfo[] myPropertyInfo = typeof(T).GetProperties(System.Reflection.BindingFlags.Public
                | System.Reflection.BindingFlags.Instance);

            foreach (T t in list)
            {
                if (t == null)
                {
                    continue;
                }

                row = dt.NewRow();

                for (int i = 0, j = myPropertyInfo.Length; i < j; i++)
                {
                    System.Reflection.PropertyInfo pi = myPropertyInfo[i];

                    string name = pi.Name;

                    if (dt.Columns[name] == null)
                    {
                        column = new DataColumn(name, pi.PropertyType);
                        dt.Columns.Add(column);
                    }

                    row[name] = pi.GetValue(t, null);
                }

                dt.Rows.Add(row);
            }

            ds.Tables.Add(dt);

            return ds;
        }


        public static List<T> GetList<T>(DataTable table)
        {
            List<T> list = new List<T>();
            T t = default(T);
            PropertyInfo[] propertypes = null;
            string tempName = string.Empty;
            foreach (DataRow row in table.Rows)
            {
                t = Activator.CreateInstance<T>();
                propertypes = t.GetType().GetProperties();
                foreach (PropertyInfo pro in propertypes)
                {
                    tempName = pro.Name;
                    if (table.Columns.Contains(tempName))
                    {
                        object value = row[tempName];
                        if (!value.ToString().Equals(""))
                        {
                            pro.SetValue(t, value, null);
                        }
                    }
                }
                list.Add(t);
            }
            return list.Count == 0 ? null : list;
        }



        public DataTable ExcelToDataTable(string sheetName, bool isFirstRowColumn)
        {
            ISheet sheet = null;
            DataTable data = new DataTable();
            int startRow = 0;
            try
            {
                fs = new FileStream(fileName, FileMode.Open, FileAccess.Read);
                if (fileName.IndexOf(".xlsx") > 0)
                    workbook = new XSSFWorkbook(fs);
                else if (fileName.IndexOf(".xls") > 0)
                    workbook = new HSSFWorkbook(fs);

                if (sheetName != null)
                {
                    sheet = workbook.GetSheet(sheetName);
                    if (sheet == null)
                    {
                        sheet = workbook.GetSheetAt(0);
                    }
                }
                else
                {
                    sheet = workbook.GetSheetAt(0);
                }
                if (sheet != null)
                {
                    IRow firstRow = sheet.GetRow(0);
                    int cellCount = firstRow.LastCellNum;
                    if (isFirstRowColumn)
                    {
                        for (int i = firstRow.FirstCellNum; i < cellCount; ++i)
                        {
                            ICell cell = firstRow.GetCell(i);
                            if (cell != null)
                            {
                                string cellValue = "-1";
                                cellValue = GetCellValue(firstRow, i, ref cellValue);


                                if (cellValue != null)
                                {

                                    if (!data.Columns.Contains(cellValue))
                                    {
                                        DataColumn column = new DataColumn(cellValue);
                                        data.Columns.Add(column);
                                    }

                                    else
                                    {
                                        cellValue = Guid.NewGuid().ToString();
                                        DataColumn column = new DataColumn(cellValue);

                                        data.Columns.Add(column);

                                    }

                                }
                            }
                        }
                        startRow = sheet.FirstRowNum + 1;
                    }
                    else
                    {
                        startRow = sheet.FirstRowNum;
                        for (int i = firstRow.FirstCellNum; i < cellCount; ++i)
                        {
                            DataColumn column = new DataColumn(i.ToString());
                            data.Columns.Add(column);
                        }
                    }


                    int rowCount = sheet.LastRowNum;
                    for (int i = startRow; i <= rowCount; ++i)
                    {
                        String strVal = "";

                        IRow row = sheet.GetRow(i);
                        if (row == null) continue;
                        DataRow dataRow = data.NewRow();
                        for (int j = row.FirstCellNum; j < cellCount; ++j)
                        {

                            if (row.Cells.Count == cellCount)
                            {
                                if (row.Cells[j] != null)
                                {
                                    GetCellValue(row, j, ref strVal);

                                    dataRow[j] = strVal;
                                }
                            }


                            else if (row.Cells.Count < cellCount)
                            {
                                if (j >= row.Cells.Count)
                                {
                                    dataRow[j] = "-1";

                                }

                                else
                                {
                                    if (row.Cells[j] != null)
                                    {
                                        GetCellValue(row, j, ref strVal);

                                        dataRow[j] = strVal;
                                    }
                                }
                            }


                            else
                            {
                                dataRow[j] = "-1";
                            }
                        }
                        data.Rows.Add(dataRow);
                    }
                }

                return data;
            }
            catch (Exception err)
            {

                return null;
            }
        }

        private static string GetCellValue(IRow firstRow, int i, ref    string cellValue)
        {
            String cellType = firstRow.Cells[i].CellType.ToString();


            int flag = CellTypeDIc[cellType.ToUpper()];

            try
            {
                switch (flag)
                {
                    case 1:
                        cellValue = firstRow.Cells[i].StringCellValue;
                        break;

                    case 2:
                        cellValue = firstRow.Cells[i].BooleanCellValue.ToString();
                        break;

                    case 3:
                        cellValue = firstRow.Cells[i].DateCellValue.ToLongDateString();
                        break;

                    case 4:

                        if (IsDateTime(firstRow, i))
                        {

                            cellValue = firstRow.Cells[i].DateCellValue.ToString();
                        }
                        else
                        {
                            cellValue = firstRow.Cells[i].NumericCellValue.ToString();
                        }


                        break;


                    case 5:
                        cellValue = firstRow.Cells[i].RichStringCellValue.ToString();
                        break;

                    default:
                        cellValue = "-1";
                        break;

                }


                return cellValue;
            }
            catch (Exception e)
            {
                return "-1";
            }
        }

        private static bool IsDateTime(IRow firstRow, int i)
        {

            return DateUtil.IsCellDateFormatted(firstRow.Cells[i]);


        }



        public static DataTable OpenCSV(string fullFileName, Int16 firstRow = 0, Int16 firstColumn = 0, Int16 getRows = 0, Int16 getColumns = 0, bool haveTitleRow = true)
        {
            DataTable dt = new DataTable();
            FileStream fs = new FileStream(fullFileName, System.IO.FileMode.Open, System.IO.FileAccess.Read);
            StreamReader sr = new StreamReader(fs, System.Text.Encoding.Default);
            //记录每次读取的一行记录
            string strLine = "";
            //记录每行记录中的各字段内容
            string[] aryLine;
            //标示列数
            int columnCount = 0;
            //是否已建立了表的字段
            bool bCreateTableColumns = false;
            //第几行
            int iRow = 1;

            //去除无用行
            if (firstRow > 0)
            {
                for (int i = 1; i < firstRow; i++)
                {
                    sr.ReadLine();
                }
            }

            // { ",", ".", "!", "?", ";", ":", " " };
            string[] separators = { "," };
            //逐行读取CSV中的数据
            while ((strLine = sr.ReadLine()) != null)
            {
                strLine = strLine.Trim();
                aryLine = strLine.Split(separators, System.StringSplitOptions.RemoveEmptyEntries);

                if (bCreateTableColumns == false)
                {
                    bCreateTableColumns = true;
                    columnCount = aryLine.Length;
                    //创建列
                    for (int i = firstColumn; i < (getColumns == 0 ? columnCount : firstColumn + getColumns); i++)
                    {
                        DataColumn dc
                            = new DataColumn(haveTitleRow == true ? aryLine[i] : "COL" + i.ToString());
                        dt.Columns.Add(dc);
                    }

                    bCreateTableColumns = true;

                    if (haveTitleRow == true)
                    {
                        continue;
                    }
                }


                DataRow dr = dt.NewRow();
                for (int j = firstColumn; j < (getColumns == 0 ? columnCount : firstColumn + getColumns); j++)
                {
                    dr[j - firstColumn] = aryLine[j];
                }
                dt.Rows.Add(dr);

                iRow = iRow + 1;
                if (getRows > 0)
                {
                    if (iRow > getRows)
                    {
                        break;
                    }
                }

            }

            sr.Close();
            fs.Close();
            return dt;

        }






        public List<Dictionary<String, String>> ExcelToDictionaryList(String sheelName, bool isFirstRowColumn)
        {

            DataTable dt = ExcelToDataTable(sheelName, isFirstRowColumn);
            List<Dictionary<String, String>> list = new List<Dictionary<string, string>>();
            try
            {
                if (dt != null)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        Dictionary<String, String> dic = new Dictionary<string, string>();
                        for (int j = 0; j < dt.Columns.Count; j++)
                        {
                            dic.Add(dt.Columns[j].ColumnName, dt.Rows[i].ItemArray[j].ToString());
                        }
                        list.Add(dic);
                    }

                }

                return list;
            }
            catch (Exception e)
            {
                return list;
            }

        }


        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    if (fs != null)
                        fs.Close();
                }
                fs = null;
                disposed = true;
            }
        }



        public static DataSet ExcelToDataTables(String filePath,String SheetName)
        {
            OleDbConnection oledbConn = new OleDbConnection();
            try
            {
                if (Path.GetExtension(filePath) == ".xls")
                {
                    oledbConn = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + filePath + ";Extended Properties=\"Excel 8.0;HDR=NO;IMEX=1\"");
                }
                else if (Path.GetExtension(filePath) == ".xlsx")
                {
                    oledbConn = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + filePath + ";Extended Properties='Excel 12.0;HDR=NO;IMEX=1;';");
                }
                oledbConn.Open();
                OleDbCommand cmd = new OleDbCommand();
                OleDbDataAdapter oleda = new OleDbDataAdapter();
                DataSet ds = new DataSet();
                cmd.Connection = oledbConn;
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "SELECT * FROM " + SheetName;
                oleda = new OleDbDataAdapter(cmd);
                oleda.Fill(ds);
                return ds;
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                oledbConn.Close();
            }
        }



        public static void DataSetToExcelAlias(DataSet ds, String fieName)
        {

            DataTable dt = ds.Tables[0];
            string connString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + fieName + ";Extended Properties='Excel 8.0;HDR=Yes;IMEX=2'";
            OleDbConnection objConn = new OleDbConnection(connString);
            objConn.Open();

            for (int row = 0; row < dt.Rows.Count; row++)
            {
                StringBuilder sb = new StringBuilder();

                string sql = "";

                //拼接Insert语句

                sql = " insert into [sheet1$]( ";

                for (int col = 0; col < dt.Columns.Count; col++)
                {
                    if (col < dt.Columns.Count - 1)
                        sql += dt.Columns[col].ColumnName + ",";
                    else
                        sql += dt.Columns[col].ColumnName + ") values (";
                }

                sb.Append(sql);
                for (int col = 0; col < dt.Columns.Count; col++)
                {
                    if (col < dt.Columns.Count - 1)
                        sb.Append("'" + dt.Rows[row][col].ToString() + "',");
                    else
                        sb.Append("'" + dt.Rows[row][col].ToString() + "')");
                }
                OleDbCommand objCmd = new OleDbCommand(sb.ToString(), objConn);

                objCmd.ExecuteNonQuery();//执行insert语句

                
            }
        }

    }
}
