using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AjaxPro;

namespace PrintX.Dev.Utils.ToolsKit
{
    class JSonObject
    {
    }

    public class JsonObjecttAPI
    {
        public static bool checkParam(JavaScriptObject obj, Dictionary<String, String> kVtype)
        {
            if (obj == null)
                return false;

            foreach (var key in kVtype.Keys)
            {
                if (!checkKey(obj, key))
                    return false;
                //to do
            }

            return true;
        }


        public static bool checkKey(JavaScriptObject obj, String key)
        {
            if (obj == null)
                return false;

            if (String.IsNullOrEmpty(key))
                return false;

            if (!obj.Contains(key))
                return false;


            return true;
        }


        public static bool GetJSObjValue(JavaScriptObject obj, String key, ref object result)
        {


            if (!checkKey(obj, key))
            {
                return false;
            }
            result = obj[key];
            return true;

        }





        public static bool GetJSObjValueString(JavaScriptObject obj, String key, ref String result)
        {
            if (!checkKey(obj, key))
            {
                return false;
            }

            result = obj[key].Value;
            return true;

        }





        public static bool GetJSObjValueInt(JavaScriptObject obj, String key, ref int result)
        {
            if (!checkKey(obj, key))
            {
                return false;
            }

            return Int32.TryParse(obj[key].Value, out result);


        }




        public static bool GetJSObjValueFloat(JavaScriptObject obj, String key, ref float result)
        {
            if (!checkKey(obj, key))
            {
                return false;
            }

            return Single.TryParse(obj[key].Value, out result);


        }




        public static bool GetJSObjValueDouble(JavaScriptObject obj, String key, ref double result)
        {
            if (!checkKey(obj, key))
            {
                return false;
            }

            return Double.TryParse(obj[key].Value, out result);


        }





        public static bool GetJSObjValueDouble(JavaScriptObject obj, String key, ref Byte result)
        {
            if (!checkKey(obj, key))
            {
                return false;
            }

            return Byte.TryParse(obj[key].Value, out result);
        }



        public static bool GetJSObjValueDouble(JavaScriptObject obj, String key, ref Decimal result)
        {
            if (!checkKey(obj, key))
            {
                return false;
            }

            return Decimal.TryParse(obj[key].Value, out result);


        }



        public static bool GetJSObjValueDouble(JavaScriptObject obj, String key, ref DateTime result)
        {
            if (!checkKey(obj, key))
            {
                return false;
            }

            return DateTime.TryParse(obj[key].Value, out result);
        }





        public static T JsonToEntity<T>(JavaScriptObject javascriptObj)
        {
            return (T)JavaScriptDeserializer.
                DeserializeFromJson(AjaxPro.JavaScriptSerializer.
                Serialize(javascriptObj),
            typeof(T));
        }

        public static Dictionary<String, Object> JsonToDictionary(JavaScriptObject javascriptObj)
        {
            return Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<String, Object>>(AjaxPro.JavaScriptSerializer.
                 Serialize(javascriptObj));

        }






    }
}
