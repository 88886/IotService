using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using System.Data;
using Newtonsoft.Json;
using PrintX.Dev.Utils.ToolsKit;

namespace SKT.Dev.Utils.ToolsKit.HttpClient
{
   public class HttpAPI
    {

        //json转hash
        public IHashMap JsonToHashMap(String json)
        {
            return JsonConvert.DeserializeObject<HashMap>(json);
        }


        //hash转json串
        public String HashMapJson(IHashMap map)
        {
            return JsonConvert.SerializeObject(map);
        }



        //哈希列表转json串
        public String HashMapListToJson(IHashMapList list)
        {
            return JsonConvert.SerializeObject(list);
        }


        //json串转哈希列表
        public IHashMapList JsonToHashMapList(String json)
        {
            return JsonConvert.DeserializeObject<IHashMapList>(json);
        }


        public String DataTableToJson(DataTable datasource)
        {
            return JsonConvert.SerializeObject(datasource);
        }


        public DataTable JsonToDataTable(String json)
        {
            return JsonConvert.DeserializeObject<DataTable>(json);
        }


       public static String SktGet(String uri)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
            request.Accept = "application/json";
            try
            {
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();


                return ReadResponseData(response);
            }
            catch (System.Net.WebException e)
            {
                return null;
            }
        }

        public static String SktPost(string testUrl, string jsonData)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(testUrl);
            request.AllowWriteStreamBuffering = true;
            request.Method = "POST";
            request.ContentType = "application/json";
          

            try
            {
                AddBodyContent(jsonData, request);
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                return ReadResponseData(response);
            }
            catch (System.Net.WebException e)
            {

                return null;
            }
        }


        private static String SktPut(string uri, string jsonData)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
            request.AllowWriteStreamBuffering = true;
            request.Method = "PUT";
            request.ContentType = "application/json";
            try
            {
                AddBodyContent(jsonData, request);
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                return ReadResponseData(response);
            }
            catch (System.Net.WebException e)
            {
                return null;
            }
        }

        private static void AddBodyContent(string jsonData, HttpWebRequest request)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(jsonData);
            request.ContentLength = bytes.Length;
            using (Stream requestStream = request.GetRequestStream())
            {
                requestStream.Write(bytes, 0, bytes.Length);
            }
        }


        private static String SktDel(string testUrl)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(testUrl);
            request.Accept = "application/json";
            request.Method = "DELETE";
            try
            {
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                // Display the status.
                return ReadResponseData(response);
            }
            catch (System.Net.WebException e)
            {
                return null;
            }

        }

        static String ReadResponseData(HttpWebResponse response)
        {
            // Get the stream containing content returned by the server.
            Stream dataStream = response.GetResponseStream();
            // Open the stream using a StreamReader for easy access.
            StreamReader reader = new StreamReader(dataStream);
            // Read the content.
            string responseFromServer = reader.ReadToEnd();
            // Clean up the streams and the response.
            reader.Close();
            response.Close();
            return responseFromServer;
        }
    }
}
