using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ServiceStack.Redis;

namespace PrintX.Dev.Utils.ToolsKit.RedisSDK
{
  public  class RedisAPI
    {

      public static String host = "127.0.0.1";
        public static int port = 6379;

        public static String password = null;


        public RedisAPI()
        {


        }


        ~RedisAPI()
        {


        }


        public static bool SetKeyStringValueObject<T>(String key, T value)
        {
            bool flag = false;
            using (RedisClient redisClient = new RedisClient(host, port))
            {
                flag = redisClient.Add(key, value);
                redisClient.Dispose();

            }
            return flag;

        }


        public static T getKeyStringValueObject<T>(String key)
        {
            T result;
            using (RedisClient redisClient = new RedisClient(host, port))
            {
                result = redisClient.Get<T>(key);
                redisClient.Dispose();
            }
            return result;
        }



        public static void lpop(String key)
        {
          
            using (RedisClient redisClient = new RedisClient(host, port))
            {
                redisClient.LPop(key);
                redisClient.Dispose();
            }
           
        }








        public static bool SortedSetAdd(String key, String member, double score)
        {
            bool flag = false;
            using (RedisClient redisClient = new RedisClient(host, port))
            {
                flag = redisClient.AddItemToSortedSet(key, member, score);
                redisClient.Dispose();
            }

            return flag;
        }


        public static void lpush(String key, String value)
        {

            using (RedisClient redisClient = new RedisClient(host, port))
            {
             
                redisClient.AddItemToList(key, value);
                redisClient.Dispose();
            }


        }



      


        public static List<String> lget(String key)
        {
            List<String> listArray = new List<string>();
            using (RedisClient redisClient = new RedisClient(host, port))
            {

                listArray=redisClient.GetAllItemsFromList(key);
                redisClient.Dispose();
            }

            return listArray;

        }


        public static void SetAdd(String key, String member)
        {
            using (RedisClient redisClient = new RedisClient(host, port))
            {
                redisClient.AddItemToSet(key, member);
                redisClient.Dispose();
            }


        }



      



        public static long SetBitMap(String key, int offset, int value)
        {
            long result = 0;
            using (RedisClient redisClient = new RedisClient(host, port))
            {
                result = redisClient.SetBit(key, offset, value);
                redisClient.Dispose();
            }
            return result;
        }



        public static long HashSet(String key, String member, Object value, String charset)
        {
            long result = 0;
            byte[] keyByte = StringToByteArray(member, charset);
            byte[] valueByte = StringToByteArray(value.ToString(), charset);
            using (RedisClient redisClient = new RedisClient(host, port))
            {
                result = redisClient.HSet(key, keyByte, valueByte);
                redisClient.Dispose();
            }
            return result;


        }



        public static String HashGet(String key, String member, String charset)
        {

            byte[] keyByte = StringToByteArray(member, charset);
            byte[] resultArr;
            using (RedisClient redisClient = new RedisClient(host, port))
            {
                resultArr = redisClient.HGet(key, keyByte);
                redisClient.Dispose();
            }
            return ByteArrayToString(resultArr, charset);
        }

        public static bool ExistKey(String key)
        {
            bool result = false;
            using (RedisClient redisClient = new RedisClient(host, port))
            {
                result = redisClient.Exists(key) > 0;
                redisClient.Dispose();
            }
            return result;

        }

        public static void SetKey(String key, String value, int expire, String charset)
        {

            byte[] valueByte = StringToByteArray(value, charset);
            using (RedisClient redisClient = new RedisClient(host, port))
            {
                redisClient.SetEx(key, expire, valueByte);
                redisClient.Dispose();
            }
        }



        public static void addKeyStringValueListStringToList(String key, List<String> listDataSource)
        {
            using (RedisClient redisClient = new RedisClient(host, port))
            {
                redisClient.AddRangeToList(key, listDataSource);
                redisClient.Dispose();
            }
        }


        public void addKeyStringValueListStringToSet(String key, List<String> listDataSource)
        {
            using (RedisClient redisClient = new RedisClient(host, port))
            {
                redisClient.AddRangeToSet(key, listDataSource);
                redisClient.Dispose();
            }

        }


        public static void Delete(params String[] keys)
        {
            using (RedisClient redisClient = new RedisClient(host, port))
            {
                redisClient.Del(keys);
                redisClient.Dispose();
            }
        }





        public static byte[] StringToByteArray(String s, String charset)
        {

            return System.Text.Encoding.GetEncoding(charset).GetBytes(s);
        }


        public static String ByteArrayToString(byte[] array, String charset)
        {

            return System.Text.Encoding.GetEncoding(charset).GetString(array);
        }

    }
}
