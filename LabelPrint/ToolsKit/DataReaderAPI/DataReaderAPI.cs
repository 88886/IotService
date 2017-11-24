using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using System.Text.RegularExpressions;
using PrintX.Dev.Utils.ToolsKit;

namespace SKT.Dev.Utils.ToolsKit.DataReaderAPI
{
    public class DataReaderAPI
    {
        //从sqlDataReader获取字典，该字典的key，就是select 所看到的结果，只有一行的情况
        public static Dictionary<String, Object> DataReaderToHashTable(SqlDataReader rdr)
        {
            Dictionary<String, Object> HashObject = Enumerable.Range(0, rdr.FieldCount)
                          .ToDictionary(
                              i => rdr.GetName(i),
                              i => rdr.GetValue(i)
                              );
            return HashObject;
        }


        //从sqlDataReader获取字典列表，该字典列表的key，就是select 所看到的结果，多行的情况
        public static List<Dictionary<String, Object>> DataReaderToHashTableList(SqlDataReader rdr)
        {
            List<Dictionary<String, Object>> HashObjectList = new List<Dictionary<String, Object>>();
            while (rdr.Read())
            {
                Dictionary<String, Object> dic = Enumerable.Range(0, rdr.FieldCount)
                              .ToDictionary(
                                  i => rdr.GetName(i),
                                  i => rdr.GetValue(i)
                                  );
                HashObjectList.Add(dic);
            }

            return HashObjectList;


        }


        public static HashMapList DataReaderToHashMapList(SqlDataReader rdr)
        {
            HashMapList HashObjectList = new HashMapList();
            while (rdr.Read())
            {
                Dictionary<String, Object> dic = Enumerable.Range(0, rdr.FieldCount)
                              .ToDictionary(
                                  i => rdr.GetName(i),
                                  i => rdr.GetValue(i)
                                  );
                HashMap map = new HashMap();
                DictionaryToHashMap(dic, ref map);
                HashObjectList.Add(map);
            }

            return HashObjectList;

        }

        private static void DictionaryToHashMap(Dictionary<string, object> dic, ref HashMap map)
        {
            for (int i = 0; i < dic.Keys.Count; i++)
            {
                String key = dic.Keys.ElementAt(i);
                String value = dic[key].ToString();
                map.Add(key, value);
            }
        }








        public static int GetTargetValueInt(Dictionary<String, Object> dic, String key)
        {
            String result = "";
            try
            {
                result = GetTargetValueString(dic, key);
                return Int32.Parse(result);
            }
            catch (Exception err)
            {
                return -1;
            }
        }


        //从一个字典中，获取健值为key的value，该key对应的目标值是DateTime类型
        public static DateTime GetTargetValueDateTime(Dictionary<String, Object> dic, String key)
        {
            String result = "";
            try
            {
                result = GetTargetValueString(dic, key);
                return DateTime.Parse(result);
            }
            catch (Exception err)
            {
                DateTime t = new DateTime();

                return t;
            }

        }


        //从一个字典中，获取健值为key的value，该key对应的目标值是DateTime类型
        public static DateTime GetTargetValueDateTime(Dictionary<String, Object> dic, String key, ref bool flag)
        {
            String result = "";
            try
            {
                result = GetTargetValueString(dic, key);

                DateTime t = DateTime.Parse(result);
                flag = true;
                return t;
            }
            catch (Exception err)
            {
                DateTime t = new DateTime();
                flag = false;
                return t;
            }

        }


        //从一个字典中，获取健值为key的value，该key对应的目标值是整数
        public static float GetTargetValueFloat(Dictionary<String, Object> dic, String key)
        {
            String result = "";
            try
            {
                result = GetTargetValueString(dic, key);
                return Single.Parse(result);
            }
            catch (Exception err)
            {
                return -1;
            }
        }

        //从一个字典中，获取健值为key的value，该key对应的目标值是双精度
        public static double GetTargetValueDouble(Dictionary<String, Object> dic, String key)
        {
            String result = "";
            try
            {
                result = GetTargetValueString(dic, key);
                return Double.Parse(result);
            }
            catch (Exception err)
            {
                return -1;
            }
        }

        //从一个字典中，获取健值为key的value，该key对应的目标值是精确数
        public static decimal GetTargetValueDecimal(Dictionary<String, Object> dic, String key)
        {
            String result = "";
            try
            {
                result = GetTargetValueString(dic, key);
                return Decimal.Parse(result);
            }
            catch (Exception err)
            {
                return -1;
            }
        }


        //从一个字典中，获取健值为key的value，该key对应的目标值是字符串
        public static String GetTargetValueString(Dictionary<String, Object> dic, String key)
        {
            bool flag = false;
            object result = GetTargetValue(dic, key, ref flag);
            if (flag && result != null)
            {
                String resultString = result.ToString();
                if (String.IsNullOrEmpty(resultString))
                {
                    return " ";
                }
                return resultString;

            }

            return "-1";
        }


        //从一个字典中，获取健值为key的value，该key对应的目标值是字节
        public static byte GetTargetValueByte(Dictionary<String, Object> dic, String key)
        {

            String result = "";
            try
            {
                result = GetTargetValueString(dic, key);
                return Byte.Parse(result);
            }
            catch (Exception err)
            {
                return new byte();
            }

        }

        //从一个字典中，获取健值为key的value，该key对应的目标值是Object类型
        public static object GetTargetValue(Dictionary<String, Object> dic, String key, ref bool flag)
        {
            flag = false;
            if (dic == null)
                return null;
            if (!dic.ContainsKey(key))
                return null;
            flag = true;
            return dic[key];
        }

        #region 将 Json 解析成 DateTable add by peter on 2016-8-10
        /// <summary>    
        /// 将 Json 解析成 DateTable
        /// Json 数据格式如:  
        ///{table:[{column1:1,column2:2,column3:3},{column1:1,column2:2,column3:3}]} 
        /// </summary>    
        /// <param name="strJson">要解析的 Json 字符串</param>    
        /// <returns>返回 DateTable</returns>    
        public static DataTable JsonToDataTable(string strJson)
        {
            // 取出表名    
            var rg = new Regex(@"(?<={)[^:]+(?=:\[)", RegexOptions.IgnoreCase);
            string strName = rg.Match(strJson).Value;
            DataTable tb = null;
            // 去除表名    
            strJson = strJson.Substring(strJson.IndexOf("[") + 1);
            strJson = strJson.Substring(0, strJson.IndexOf("]"));
            // 获取数据    
            rg = new Regex(@"(?<={)[^}]+(?=})");
            MatchCollection mc = rg.Matches(strJson);
            for (int i = 0; i < mc.Count; i++)
            {
                string strRow = mc[i].Value;
                string[] strRows = strRow.Split(',');
                // 创建表    
                if (tb == null)
                {
                    tb = new DataTable();
                    tb.TableName = strName;
                    foreach (string str in strRows)
                    {
                        var dc = new DataColumn();
                        string[] strCell = str.Split(':');
                        dc.ColumnName = strCell[0].Replace("\"", "");
                        tb.Columns.Add(dc);
                    }
                    tb.AcceptChanges();
                }
                // 增加内容    
                DataRow dr = tb.NewRow();
                for (int j = 0; j < strRows.Length; j++)
                {
                    dr[j] = strRows[j].Split(':')[1].Replace("\"", "");
                }
                tb.Rows.Add(dr);
                tb.AcceptChanges();
            }
            return tb;
        }
        #endregion

    }
}
