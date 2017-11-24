using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Configuration;
using System.IO;

namespace PrintX.LeanMES.Plugin.SerialPort
{
    public class tool
    {


        public static bool ishex(char x)
        {
            bool re = false;
            if ((x <= '9') && (x >= '0'))
            {
                re = true;
            }
            else if ((x <= 'F') && (x >= 'A'))
            {
                re = true;
            }
            else if ((x <= 'f') && (x >= 'a'))
            {
                re = true;
            }

            return re;
        }

        public static byte[] GetByteData(string s)
        {
            byte[] data = new byte[s.Length / 2];
            for (int i = 0; i < s.Length / 2; i++)
            {
                if (s[i * 2] <= '9')
                {
                    data[i] = (byte)((s[i * 2] - '0') * 16);
                }
                else if (s[i * 2] <= 'f' && s[i * 2] >= 'a')
                {
                    data[i] = (byte)((s[i * 2] - 'a' + 10) * 16);
                }
                else if (s[i * 2] <= 'F' && s[i * 2] >= 'A')
                {
                    data[i] = (byte)((s[i * 2] - 'A' + 10) * 16);
                }

                if (s[i * 2 + 1] <= '9')
                {
                    data[i] = (byte)(data[i] + (byte)((s[i * 2 + 1] - '0')));
                }
                else if (s[i * 2 + 1] <= 'f' && s[i * 2 + 1] >= 'a')
                {
                    data[i] = (byte)(data[i] + (byte)((s[i * 2 + 1] - 'a' + 10)));
                }
                else if (s[i * 2 + 1] <= 'F' && s[i * 2 + 1] >= 'A')
                {
                    data[i] = (byte)(data[i] + (byte)((s[i * 2 + 1] - 'A' + 10)));
                }
            }
            return data;
        }



        public static string GetHexString(string str)
        {
            int len = str.Length;
            string datarev = "";
            int i = 0;
            for (i = 0; i < (len) / 3; i++)
            {
                if ((ishex(str[3 * i])) && (ishex(str[3 * i + 1])) && (str[3 * i + 2] == ' '))
                {
                    datarev = datarev + str[3 * i] + str[3 * i + 1];
                }
                else if ((ishex(str[3 * i])) && (ishex(str[3 * i + 1])) && (3 * i + 2 == len))
                {
                    datarev = datarev + str[3 * i] + str[3 * i + 1];
                }
            }
            if (len - i * 3 == 2)
            {
                if ((ishex(str[len - 1])) && (ishex(str[len - 2])))
                {
                    datarev = datarev + str[len - 2] + str[len - 1];
                }
            }
            return datarev;
        }
        public bool ishexstring(string strl)
        {
            string del = "  ";
            string str = strl.Trim(del.ToCharArray());
            int len = str.Length;
            bool re = false;
            for (int i = 0; i < (len) / 3; i++)
            {
                if ((ishex(str[3 * i])) && (ishex(str[3 * i + 1])) && (str[3 * i + 2] == ' '))
                {
                    re = true;
                }
                else if ((ishex(str[3 * i])) && (ishex(str[3 * i + 1])) && (3 * i + 2 == len))
                {
                    re = true;
                }
                else
                {
                    re = false;
                }
            }
            return re;
        }
        /// <summary>
        /// 十进制转换为二进制
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public static string DecToBin(string x)
        {
            string z = null;
            int X = Convert.ToInt32(x);
            int i = 0;
            long a, b = 0;
            while (X > 0)
            {
                a = X % 2;
                X = X / 2;
                b = b + a * Pow(10, i);
                i++;
            }
            z = Convert.ToString(b);
            if (z == "0")
            {
                z = "00";
            }


            return z;
        }

        /// <summary>
        /// 16进制转ASCII码
        /// </summary>
        /// <param name="hexString"></param>
        /// <returns></returns>
        public static string HexToAscii(string hexString)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i <= hexString.Length - 2; i += 2)
            {
                sb.Append(
                    Convert.ToString(
                        Convert.ToChar(Int32.Parse(hexString.Substring(i, 2),
                                                   System.Globalization.NumberStyles.HexNumber))));
            }
            return sb.ToString();
        }

        /// <summary>
        /// 十进制转换为八进制
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public static string DecToOtc(string x)
        {
            string z = null;
            int X = Convert.ToInt32(x);
            int i = 0;
            long a, b = 0;
            while (X > 0)
            {
                a = X % 8;
                X = X / 8;
                b = b + a * Pow(10, i);
                i++;
            }
            z = Convert.ToString(b);
            return z;
        }

        /// <summary>
        /// 十进制转换为十六进制
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public static string DecToHex(string x)
        {
            if (string.IsNullOrEmpty(x))
            {
                return "0";
            }
            string z = null;
            int X = Convert.ToInt32(x);
            Stack a = new Stack();
            int i = 0;
            while (X > 0)
            {
                a.Push(Convert.ToString(X % 16));
                X = X / 16;
                i++;
            }
            while (a.Count != 0)
                z += ToHex(Convert.ToString(a.Pop()));
            if (string.IsNullOrEmpty(z))
            {
                z = "0";
            }
            return z;
        }

        /// <summary>
        /// 二进制转换为十进制
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public static string BinToDec(string x)
        {
            string z = null;
            if (String.IsNullOrEmpty(x))
            {
                x = "00";
            }
            Int64 X = Convert.ToInt64(x);
            int i = 0;
            long a, b = 0;
            while (X > 0)
            {
                a = X % 10;
                X = X / 10;
                b = b + a * Pow(2, i);
                i++;
            }
            z = Convert.ToString(b);
            return z;
        }

        /// <summary>
        /// 二进制转换为十进制，定长转换
        /// </summary>
        /// <param name="x"></param>
        /// <param name="iLength"></param>
        /// <returns></returns>
        public static string BinToDec(string x, short iLength)
        {
            StringBuilder sb = new StringBuilder();
            int iCount = 0;

            iCount = x.Length / iLength;

            if (x.Length % iLength > 0)
            {
                iCount += 1;
            }

            int X = 0;

            for (int i = 0; i < iCount; i++)
            {
                if ((i + 1) * iLength > x.Length)
                {
                    X = Convert.ToInt32(x.Substring(i * iLength, (x.Length - iLength)));
                }
                else
                {
                    X = Convert.ToInt32(x.Substring(i * iLength, iLength));
                }
                int j = 0;
                long a, b = 0;
                while (X > 0)
                {
                    a = X % 10;
                    X = X / 10;
                    b = b + a * Pow(2, j);
                    j++;
                }
                sb.AppendFormat("{0:D2}", b);
            }
            return sb.ToString();
        }

        /// <summary>
        /// 二进制转换为十六进制，定长转换
        /// </summary>
        /// <param name="x"></param>
        /// <param name="iLength"></param>
        /// <returns></returns>
        public static string BinToHex(string x, short iLength)
        {
            StringBuilder sb = new StringBuilder();
            int iCount = 0;

            iCount = x.Length / iLength;

            if (x.Length % iLength > 0)
            {
                iCount += 1;
            }

            int X = 0;

            for (int i = 0; i < iCount; i++)
            {
                if ((i + 1) * iLength > x.Length)
                {
                    X = Convert.ToInt32(x.Substring(i * iLength, (x.Length - iLength)));
                }
                else
                {
                    X = Convert.ToInt32(x.Substring(i * iLength, iLength));
                }
                int j = 0;
                long a, b = 0;
                while (X > 0)
                {
                    a = X % 10;
                    X = X / 10;
                    b = b + a * Pow(2, j);
                    j++;
                }
                //前补0
                sb.Append(DecToHex(b.ToString()));
            }
            return sb.ToString();
        }

        /// <summary>
        /// 八进制转换为十进制
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public static string OctToDec(string x)
        {
            string z = null;
            int X = Convert.ToInt32(x);
            int i = 0;
            long a, b = 0;
            while (X > 0)
            {
                a = X % 10;
                X = X / 10;
                b = b + a * Pow(8, i);
                i++;
            }
            z = Convert.ToString(b);
            return z;
        }


        /// <summary>
        /// 十六进制转换为十进制
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public static string HexToDec(string x)
        {
            if (string.IsNullOrEmpty(x))
            {
                return "0";
            }
            string z = null;
            Stack a = new Stack();
            int i = 0, j = 0, l = x.Length;
            long Tong = 0;
            while (i < l)
            {
                a.Push(ToDec(Convert.ToString(x[i])));
                i++;
            }
            while (a.Count != 0)
            {
                Tong = Tong + Convert.ToInt64(a.Pop()) * Pow(16, j);
                j++;
            }
            z = Convert.ToString(Tong);
            return z;
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static long Pow(long x, long y)
        {
            int i = 1;
            long X = x;
            if (y == 0)
                return 1;
            while (i < y)
            {
                x = x * X;
                i++;
            }
            return x;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public static string ToDec(string x)
        {
            switch (x)
            {
                case "A":
                    return "10";
                case "B":
                    return "11";
                case "C":
                    return "12";
                case "D":
                    return "13";
                case "E":
                    return "14";
                case "F":
                    return "15";
                default:
                    return x;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public static string ToHex(string x)
        {
            switch (x)
            {
                case "10":
                    return "A";
                case "11":
                    return "B";
                case "12":
                    return "C";
                case "13":
                    return "D";
                case "14":
                    return "E";
                case "15":
                    return "F";
                default:
                    return x;
            }
        }

        /// <summary>
        /// 将16进制BYTE数组转换成16进制字符串
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static string ToHexString(byte[] bytes) // 0xae00cf => "AE00CF "
        {
            string hexString = string.Empty;
            if (bytes != null)
            {
                StringBuilder strB = new StringBuilder();

                for (int i = 0; i < bytes.Length; i++)
                {
                    strB.Append(bytes[i].ToString("X2"));
                }
                hexString = strB.ToString();
            }
            return hexString;
        }






        internal static string hextobin(string value1)
        {
            String bin = Convert.ToString(Convert.ToInt32(value1, 16), 2);
            int length = bin.Length;

            for (int i = length; i < 8; i++)
            {
                bin = "0" + bin;
            }
            return bin;
        }
    }
}
