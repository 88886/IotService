using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PrintX.Dev.Utils.ToolsKit
{
    [System.Serializable]
    public class HashMap : Dictionary<string, object>, IHashMap, IDictionary<string, object>, ICollection<System.Collections.Generic.KeyValuePair<string, object>>, System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<string, object>>, System.Collections.IEnumerable
    {
        public new object this[string key]
        {
            get
            {
                object result;
                try
                {
                    result = base[key];
                }
                catch (System.Collections.Generic.KeyNotFoundException)
                {
                    throw new KeyNotFoundException(string.Format("关键字“{0}”不在HashMap中。", key));
                }
                return result;
            }
            set
            {
                base[key] = value;
            }
        }

        public HashMap()
        {
        }

        public HashMap(System.Collections.Generic.IDictionary<string, object> dictionary)
            : base(dictionary)
        {

        }

        protected HashMap(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context)
            : base(info, context)
        {

        }

        public HashMap(string[] keys, params object[] values)
        {
            HashMap.InternalAdd(this, keys, values);
        }

        public IHashMap Clone()
        {
            HashMap hashObject = new HashMap();
            foreach (System.Collections.Generic.KeyValuePair<string, object> current in this)
            {
                hashObject.Add(current.Key, current.Value);
            }
            return hashObject;
        }

        public void Add(string[] keys, params object[] values)
        {
            HashMap.InternalAdd(this, keys, values);
        }

        public bool CheckSetValue(string key, object value)
        {
            bool result;
            if (base.ContainsKey(key))
            {
                result = false;
            }
            else
            {
                this[key] = value;
                result = true;
            }
            return result;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            bool result;
            if (obj == null)
            {
                result = false;
            }
            else if (!(obj is HashMap))
            {
                result = false;
            }
            else
            {
                HashMap hashObject = (HashMap)obj;
                if (base.Count != hashObject.Count)
                {
                    result = false;
                }
                else
                {
                    foreach (string current in base.Keys)
                    {
                        if (!hashObject.ContainsKey(current))
                        {
                            result = false;
                            return result;
                        }
                        object obj2 = this[current];
                        object obj3 = hashObject[current];
                        if (obj2 != null || obj3 != null)
                        {
                            if (!object.Equals(this[current], hashObject[current]))
                            {
                                result = false;
                                return result;
                            }
                        }
                    }
                    result = true;
                }
            }
            return result;
        }

        private static void InternalAdd(HashMap obj, string[] keys, object[] values)
        {
            if (keys.Length != values.Length)
            {
                throw new System.InvalidOperationException("Keys和Values的长度不一致！");
            }
            if (keys.Length == 0)
            {
                throw new System.InvalidOperationException("添加数据必须有一项！");
            }
            for (int i = 0; i < keys.Length; i++)
            {
                obj.Add(keys[i], values[i]);
            }
        }

        public void CopyTo(System.Collections.Generic.IDictionary<string, object> dict)
        {
            foreach (System.Collections.Generic.KeyValuePair<string, object> current in this)
            {
                dict[current.Key] = current.Value;
            }
        }

        public T GetValue<T>(string key)
        {
            return this.GetValue<T>(key, default(T));
        }

        public T GetValue<T>(string key, T defaultValue)
        {
            T result;
            try
            {
                object obj;
                if (base.TryGetValue(key, out obj))
                {
                    System.Type typeFromHandle = typeof(T);
                    System.Type type = typeFromHandle;
                    if (obj == System.DBNull.Value)
                    {
                        result = defaultValue;
                        return result;
                    }
                    if (obj != null && ReflectionUtils.IsPrimitiveType(typeFromHandle, out type))
                    {
                        result = (T)((object)HashMap.ChangeType(obj, type));
                        return result;
                    }
                    if (obj != null && type != typeFromHandle && type.IsEnum && !obj.GetType().IsEnum)
                    {
                        result = (T)((object)System.Enum.ToObject(type, obj));
                        return result;
                    }
                    if (obj == null && type.IsSubclassOf(typeof(System.ValueType)))
                    {
                        result = defaultValue;
                        return result;
                    }
                    result = (T)((object)obj);
                    return result;
                }
            }
            catch (System.Exception err)
            {

            }
            result = defaultValue;
            return result;
        }

        internal static object ChangeType(object value, System.Type type)
        {
            object result;
            if (type == typeof(bool) && value.GetType() == typeof(string))
            {
                result = ((string)value == "1");
            }
            else
            {
                result = System.Convert.ChangeType(value, type, null);
            }
            return result;
        }

        public T ToEntity<T>() where T : class, new()
        {
            return EntityFactory.GetEntity<T, IHashMap>(this, (IHashMap ho, string key) => ho.GetValue<object>(key));
        }
    }
}
