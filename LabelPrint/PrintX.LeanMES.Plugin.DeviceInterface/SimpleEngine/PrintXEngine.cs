using System;
using System.Collections;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Text;
using System.Web;
using Newtonsoft.Json;
using PrintX.PrintService;
using PrintX.LeanMES.Plugin.UI.Task;
using System.Collections.Generic;
using PrintX.LeanMES.Plugin.LabelPrint;
using PrintX.Dev.Utils.ToolsKit;
using System.Diagnostics;
using PrintX.LeanMES.Plugin.UI;

namespace Bend.Util
{

    /// <summary>
    /// 参考
    /// </summary>
    public class HttpProcessor
    {
        public TcpClient socket;
        public HttpServer srv;

        private Stream inputStream;
        public StreamWriter outputStream;

        public String http_method;
        public String http_url;
        public String http_protocol_versionstring;
        public Hashtable httpHeaders = new Hashtable();


        private static int MAX_POST_SIZE = 10 * 1024 * 1024; // 10MB

        public HttpProcessor(TcpClient s, HttpServer srv)
        {
            this.socket = s;
            this.srv = srv;
        }


        private string streamReadLine(Stream inputStream)
        {
            int next_char;
            string data = "";
            while (true)
            {
                next_char = inputStream.ReadByte();
                if (next_char == '\n') { break; }
                if (next_char == '\r') { continue; }
                if (next_char == -1) { Thread.Sleep(1); continue; };
                data += Convert.ToChar(next_char);
            }
            return data;
        }
        public void process()
        {
            // we can't use a StreamReader for input, because it buffers up extra data on us inside it's
            // "processed" view of the world, and we want the data raw after the headers
            inputStream = new BufferedStream(socket.GetStream());

            // we probably shouldn't be using a streamwriter for all output from handlers either
            outputStream = new StreamWriter(new BufferedStream(socket.GetStream()));
            try
            {
                parseRequest();
                readHeaders();
                if (http_method.Equals("GET"))
                {
                    handleGETRequest();
                }
                else if (http_method.Equals("POST"))
                {
                    handlePOSTRequest();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: " + e.ToString());
                writeFailure();
            }
            outputStream.Flush();
            // bs.Flush(); // flush any remaining output
            inputStream = null; outputStream = null; // bs = null;            
            socket.Close();
        }

        public void parseRequest()
        {
            String request = streamReadLine(inputStream);
            string[] tokens = request.Split(' ');
            if (tokens.Length != 3)
            {
                throw new Exception("invalid http request line");
            }
            http_method = tokens[0].ToUpper();
            http_url = tokens[1];
            http_protocol_versionstring = tokens[2];

            Console.WriteLine("starting: " + request);
        }

        public void readHeaders()
        {
            Console.WriteLine("readHeaders()");
            String line;
            while ((line = streamReadLine(inputStream)) != null)
            {
                if (line.Equals(""))
                {
                    Console.WriteLine("got headers");
                    return;
                }

                int separator = line.IndexOf(':');
                if (separator == -1)
                {
                    throw new Exception("invalid http header line: " + line);
                }
                String name = line.Substring(0, separator);
                int pos = separator + 1;
                while ((pos < line.Length) && (line[pos] == ' '))
                {
                    pos++; // strip any spaces
                }

                string value = line.Substring(pos, line.Length - pos);
                Console.WriteLine("header: {0}:{1}", name, value);
                httpHeaders[name] = value;
            }
        }

        public void handleGETRequest()
        {
            srv.handleGETRequest(this);
        }

        private const int BUF_SIZE = 4096;
        public void handlePOSTRequest()
        {


            Console.WriteLine("get post data start");
            int content_len = 0;
            MemoryStream ms = new MemoryStream();
            if (this.httpHeaders.ContainsKey("Content-Length"))
            {
                content_len = Convert.ToInt32(this.httpHeaders["Content-Length"]);
                if (content_len > MAX_POST_SIZE)
                {
                    throw new Exception(
                        String.Format("POST Content-Length({0}) too big for this simple server",
                          content_len));
                }
                byte[] buf = new byte[BUF_SIZE];
                int to_read = content_len;
                while (to_read > 0)
                {
                    Console.WriteLine("starting Read, to_read={0}", to_read);

                    int numread = this.inputStream.Read(buf, 0, Math.Min(BUF_SIZE, to_read));
                    Console.WriteLine("read finished, numread={0}", numread);
                    if (numread == 0)
                    {
                        if (to_read == 0)
                        {
                            break;
                        }
                        else
                        {
                            throw new Exception("client disconnected during post");
                        }
                    }
                    to_read -= numread;
                    ms.Write(buf, 0, numread);
                }
                ms.Seek(0, SeekOrigin.Begin);
            }
            Console.WriteLine("get post data end");
            srv.handlePOSTRequest(this, new StreamReader(ms));

        }

        public void writeSuccess(string content_type = "text/html")
        {

            outputStream.WriteLine("HTTP/1.0 200 OK");
            outputStream.WriteLine("Content-Type: " + content_type);
            outputStream.WriteLine("Access-Control-Allow-Origin:*");
            outputStream.WriteLine("Connection: close");
            outputStream.WriteLine("");
        }

        public void writeFailure()
        {

            outputStream.WriteLine("HTTP/1.0 404 File not found");
            outputStream.WriteLine("Access-Control-Allow-Origin:*");
            outputStream.WriteLine("Connection: close");
            outputStream.WriteLine("");
        }
    }

    public abstract class HttpServer
    {

        protected int port;
        TcpListener listener;
        bool is_active = true;

        public HttpServer(int port)
        {
            this.port = port;
        }

        public void listen()
        {
            try
            {
                listener = new TcpListener(5555);
                listener.Start();
                while (is_active)
                {
                    TcpClient s = listener.AcceptTcpClient();
                    HttpProcessor processor = new HttpProcessor(s, this);
                    Thread thread = new Thread(new ThreadStart(processor.process));
                    thread.Start();
                    Thread.Sleep(1);
                }
            }
            catch (Exception err)
            {
                Console.WriteLine(err.Message);
                System.Environment.Exit(System.Environment.ExitCode);
                listener.Stop();
                GC.Collect();
            }
        }

        public abstract void handleGETRequest(HttpProcessor p);
        public abstract void handlePOSTRequest(HttpProcessor p, StreamReader inputData);
    }


    public class SKTHttpEngine : HttpServer
    {
        public SKTHttpEngine(int port)
            : base(port)
        {
        }
        public override void handleGETRequest(HttpProcessor p)
        {

            p.writeSuccess();
            p.outputStream.WriteLine(IndexModule.content);
        }


        public override void handlePOSTRequest(HttpProcessor p, StreamReader inputData)
        {
            //这是回写的数据
            HashMap responseObj = new HashMap();
            HashMap responseData = new HashMap();


            Console.WriteLine("POST request: {0}", p.http_url);
            string data = inputData.ReadToEnd();
            //这是来自客户端POST的数据，UTF8编码

            String bodyData = HttpUtility.UrlDecode(data, Encoding.UTF8);
            //这是解码后的数据


            int errorCode = -1;
            String errorMessage = "";

            String serviceType = "";
            try
            {
                HashMap requestObj = JsonConvert.DeserializeObject<HashMap>(bodyData);


                if (requestObj == null)
                {
                    errorCode = -1;
                    throw new Exception("解析到了空的HashMap对象");
                }

                if (!requestObj.ContainsKey("key"))
                {
                    errorCode = -1;
                    throw new Exception("未能找到注册的服务类型KEY");
                }
                serviceType = requestObj["key"].ToString();
                if (String.IsNullOrEmpty(serviceType))
                {
                    errorCode = -1;
                    throw new Exception("传递了空的服务类型");
                }

                switch (serviceType)
                {
                    case "printJob":
                        PrintJob job = new PrintJob();
                        job.ProcessPrintJob(bodyData, ref errorCode, ref errorMessage);
                        break;

                    case "printerList":
                        List<String> list = Printer.GetLocalPrinters();
                        responseData.Add("printerList", list);
                        errorCode = 0;
                        break;

                    default:
                        errorCode = -1;
                        throw new Exception("传递了未知的服务类型");

                }


                responseObj.Add("errorCode", errorCode);
                responseObj.Add("errorMessage", errorMessage);
                responseObj.Add("key", serviceType);

                responseData.Add("queryString", data);
                responseObj.Add("data", responseData);

                String responseText = JsonConvert.SerializeObject(responseObj);
                p.writeSuccess();
                p.outputStream.WriteLine(responseText);
            }
            catch (Exception e)
            {
                errorCode = -1;
                errorMessage = e.Message;
                Console.WriteLine("SKTHttpEngine处理请求失败,原因为" + e.Message);
            }

        }
    }

    public class SatrtEngine : IDisposable
    {
        public static int Start()
        {
            HttpServer httpServer = new SKTHttpEngine(5555);

            Thread thread = new Thread(new ThreadStart(httpServer.listen));

            thread.Start();
            return 0;
        }

        public void Dispose()
        {
            this.Dispose();
            Process.GetCurrentProcess().Kill();
        }
    }

}



