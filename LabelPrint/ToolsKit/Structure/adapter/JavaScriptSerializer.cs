using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace PrintX.Dev.Utils.ToolsKit
{
    public class JavaScriptSerializer
    {
        private class ReferenceComparer : System.Collections.IEqualityComparer
        {
            bool System.Collections.IEqualityComparer.Equals(object x, object y)
            {
                return x == y;
            }

            int System.Collections.IEqualityComparer.GetHashCode(object obj)
            {
                return System.Runtime.CompilerServices.RuntimeHelpers.GetHashCode(obj);
            }
        }

        public enum SerializationFormat
        {
            JSON,
            JavaScript
        }

        internal const string ServerTypeFieldName = "__type";

        internal const int DefaultRecursionLimit = 100;

        private static int DefaultMaxJsonLength;

        private JavaScriptTypeResolver _typeResolver;

        private int _recursionLimit;

        private int _maxJsonLength;

        private System.Collections.Generic.Dictionary<System.Type, JavaScriptConverter> _converters;

        internal static readonly long DatetimeMinTimeTicks;

        public int MaxJsonLength
        {
            get
            {
                return this._maxJsonLength;
            }
            set
            {
                if (value < 1)
                {
                    throw new System.ArgumentOutOfRangeException("value", "Value must be a positive integer.");
                }
                this._maxJsonLength = value;
            }
        }

        public int RecursionLimit
        {
            get
            {
                return this._recursionLimit;
            }
            set
            {
                if (value < 1)
                {
                    throw new System.ArgumentOutOfRangeException("value", "RecursionLimit must be a positive integer.");
                }
                this._recursionLimit = value;
            }
        }

        internal JavaScriptTypeResolver TypeResolver
        {
            get
            {
                return this._typeResolver;
            }
        }

        private System.Collections.Generic.Dictionary<System.Type, JavaScriptConverter> Converters
        {
            get
            {
                if (this._converters == null)
                {
                    this._converters = new System.Collections.Generic.Dictionary<System.Type, JavaScriptConverter>();
                }
                return this._converters;
            }
        }

        static JavaScriptSerializer()
        {
            JavaScriptSerializer.DefaultMaxJsonLength = 5242880;
            JavaScriptSerializer.DatetimeMinTimeTicks = new System.DateTime(1970, 1, 1, 0, 0, 0, System.DateTimeKind.Utc).Ticks;
           
           
        }

        public static string SerializeInternal(object o)
        {
            return JavaScriptSerializer.CreateInstance().Serialize(o);
        }

        internal static object Deserialize(JavaScriptSerializer serializer, string input, System.Type type, int depthLimit)
        {
            if (input == null)
            {
                throw new System.ArgumentNullException("input");
            }
            if (input.Length > serializer.MaxJsonLength)
            {
                throw new System.ArgumentException("结果记录数太大，请调整查询条件后重试", "input");
            }
            object o = JavaScriptObjectDeserializer.BasicDeserialize(input, depthLimit, serializer);
            return ObjectConverter.ConvertObjectToType(o, type, serializer);
        }

        public static EntityClassAttribute GetEntityClassAttribute(System.Type type)
        {
            object[] customAttributes = type.GetCustomAttributes(typeof(EntityClassAttribute), true);
            EntityClassAttribute result = null;
            if (customAttributes != null && customAttributes.Length > 0)
            {
                result = (customAttributes[0] as EntityClassAttribute);
            }
            return result;
        }

        public static string GetPropertyKey(System.Reflection.MemberInfo prop, EntityClassAttribute classAttribute)
        {
            string text = prop.Name;
            if (classAttribute != null && classAttribute.LowerCaseKey)
            {
                text = text.ToLower();
            }
            object[] customAttributes = prop.GetCustomAttributes(typeof(FieldAttribute), true);
            string result;
            if (customAttributes == null)
            {
                result = text;
            }
            else
            {
                object[] array = customAttributes;
                for (int i = 0; i < array.Length; i++)
                {
                    object obj = array[i];
                    FieldAttribute fieldAttribute = obj as FieldAttribute;
                    if (fieldAttribute != null)
                    {
                        if (!string.IsNullOrEmpty(fieldAttribute.FieldName))
                        {
                            text = fieldAttribute.FieldName;
                        }
                    }
                }
                result = text;
            }
            return result;
        }

        public static JavaScriptSerializer CreateInstance()
        {
            return JavaScriptSerializer.CreateInstance(null);
        }

        public static JavaScriptSerializer CreateInstance(JavaScriptTypeResolver resolver)
        {
            return new JavaScriptSerializer(resolver);
        }

        private JavaScriptSerializer()
            : this(null)
        {
        }

        private JavaScriptSerializer(JavaScriptTypeResolver resolver)
        {
            this._typeResolver = resolver;
            this.RecursionLimit = 100;
            this.MaxJsonLength = JavaScriptSerializer.DefaultMaxJsonLength;
        }

        public void RegisterConverters(System.Collections.Generic.IEnumerable<JavaScriptConverter> converters)
        {
            if (converters == null)
            {
                throw new System.ArgumentNullException("converters");
            }
            foreach (JavaScriptConverter current in converters)
            {
                System.Collections.Generic.IEnumerable<System.Type> supportedTypes = current.SupportedTypes;
                if (supportedTypes != null)
                {
                    foreach (System.Type current2 in supportedTypes)
                    {
                        this.Converters[current2] = current;
                    }
                }
            }
        }

        private JavaScriptConverter GetConverter(System.Type t)
        {
            JavaScriptConverter result;
            if (this._converters != null)
            {
                while (t != null)
                {
                    if (this._converters.ContainsKey(t))
                    {
                        result = this._converters[t];
                        return result;
                    }
                    t = t.BaseType;
                }
            }
            result = null;
            return result;
        }

        internal bool ConverterExistsForType(System.Type t, out JavaScriptConverter converter)
        {
            converter = this.GetConverter(t);
            return converter != null;
        }

        public object DeserializeObject(string input)
        {
            return JavaScriptSerializer.Deserialize(this, input, null, this.RecursionLimit);
        }

        public T Deserialize<T>(string input)
        {
            return (T)((object)JavaScriptSerializer.Deserialize(this, input, typeof(T), this.RecursionLimit));
        }

        public T ConvertToType<T>(object obj)
        {
            return (T)((object)ObjectConverter.ConvertObjectToType(obj, typeof(T), this));
        }

        public string Serialize(object obj)
        {
            return this.Serialize(obj, JavaScriptSerializer.SerializationFormat.JavaScript);
        }

        public void Serialize(object obj, System.Text.StringBuilder output)
        {
            this.Serialize(obj, output, JavaScriptSerializer.SerializationFormat.JavaScript);
        }

        public string Serialize(object obj, JavaScriptSerializer.SerializationFormat serializationFormat)
        {
            System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
            this.Serialize(obj, stringBuilder, serializationFormat);
            return stringBuilder.ToString();
        }

        internal void Serialize(object obj, System.Text.StringBuilder output, JavaScriptSerializer.SerializationFormat serializationFormat)
        {
            this.SerializeValue(obj, output, 0, null, serializationFormat);
            if (output.Length > this.MaxJsonLength)
            {
                throw new System.InvalidOperationException("结果记录数太大，请调整查询条件后重试");
            }
        }

        private static void SerializeBoolean(bool o, System.Text.StringBuilder sb)
        {
            if (o)
            {
                sb.Append("true");
            }
            else
            {
                sb.Append("false");
            }
        }

        private static void SerializeUri(Uri uri, System.Text.StringBuilder sb)
        {
            sb.Append("\"").Append(uri.GetComponents(UriComponents.SerializationInfoString, UriFormat.UriEscaped)).Append("\"");
        }

        private static void SerializeGuid(System.Guid guid, System.Text.StringBuilder sb)
        {
            sb.Append("\"").Append(guid.ToString()).Append("\"");
        }

        private static void SerializeCuid(ulong cuid, System.Text.StringBuilder sb)
        {
            if (cuid == 0uL)
            {
                sb.Append("0");
            }
            else
            {
                sb.Append("\"").Append(cuid.ToString()).Append("\"");
            }
        }

        private static void SerializeCuidForSqlServer(long cuid, System.Text.StringBuilder sb)
        {
            if (cuid == 0L)
            {
                sb.Append("0");
            }
            else
            {
                sb.Append("\"").Append(cuid.ToString()).Append("\"");
            }
        }

        private static void SerializeDateTime(System.DateTime datetime, System.Text.StringBuilder sb, JavaScriptSerializer.SerializationFormat serializationFormat)
        {
            if (serializationFormat == JavaScriptSerializer.SerializationFormat.JSON)
            {
                sb.Append("\"\\/Date(");
                sb.Append((( datetime.ToUniversalTime()).Ticks - JavaScriptSerializer.DatetimeMinTimeTicks) / 10000L);
                sb.Append(")\\/\"");
            }
            else
            {
                sb.Append("new Date(");
                sb.Append(((datetime.ToUniversalTime()).Ticks - JavaScriptSerializer.DatetimeMinTimeTicks) / 10000L);
                sb.Append(")");
            }
        }

        private void SerializeCustomObject(object o, System.Text.StringBuilder sb, int depth, System.Collections.Hashtable objectsInUse, JavaScriptSerializer.SerializationFormat serializationFormat)
        {
            bool flag = true;
            System.Type type = o.GetType();
            sb.Append('{');
            if (this.TypeResolver != null)
            {
                string text = this.TypeResolver.ResolveTypeId(type);
                if (text != null)
                {
                    JavaScriptSerializer.SerializeString("__type", sb);
                    sb.Append(':');
                    this.SerializeValue(text, sb, depth, objectsInUse, serializationFormat);
                    flag = false;
                }
            }
            EntityClassAttribute entityClassAttribute = JavaScriptSerializer.GetEntityClassAttribute(type);
            System.Reflection.FieldInfo[] fields = type.GetFields(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public);
            System.Reflection.FieldInfo[] array = fields;
            for (int i = 0; i < array.Length; i++)
            {
                System.Reflection.FieldInfo fieldInfo = array[i];
                if (!fieldInfo.IsDefined(typeof(ScriptIgnoreAttribute), true))
                {
                    if (!flag)
                    {
                        sb.Append(',');
                    }
                    JavaScriptSerializer.SerializeString(JavaScriptSerializer.GetPropertyKey(fieldInfo, entityClassAttribute), sb);
                    sb.Append(':');
                    this.SerializeValue(fieldInfo.GetValue(o), sb, depth, objectsInUse, serializationFormat);
                    flag = false;
                }
            }
            System.Reflection.PropertyInfo[] properties = type.GetProperties(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.GetProperty);
            System.Reflection.PropertyInfo[] array2 = properties;
            for (int i = 0; i < array2.Length; i++)
            {
                System.Reflection.PropertyInfo propertyInfo = array2[i];
                if (!propertyInfo.IsDefined(typeof(ScriptIgnoreAttribute), true))
                {
                    System.Reflection.MethodInfo getMethod = propertyInfo.GetGetMethod();
                    if (!(getMethod == null))
                    {
                        if (getMethod.GetParameters().Length <= 0)
                        {
                            if (!flag)
                            {
                                sb.Append(',');
                            }
                            JavaScriptSerializer.SerializeString(JavaScriptSerializer.GetPropertyKey(propertyInfo, entityClassAttribute), sb);
                            sb.Append(':');
                            this.SerializeValue(getMethod.Invoke(o, null), sb, depth, objectsInUse, serializationFormat);
                            flag = false;
                        }
                    }
                }
            }
            sb.Append('}');
        }

        private void SerializeDictionary(System.Collections.IDictionary o, System.Text.StringBuilder sb, int depth, System.Collections.Hashtable objectsInUse, JavaScriptSerializer.SerializationFormat serializationFormat)
        {
            sb.Append('{');
            bool flag = true;
            System.Collections.DictionaryEntry[] array = new System.Collections.DictionaryEntry[o.Count];
            o.CopyTo(array, 0);
            for (int i = 0; i < array.Length; i++)
            {
                System.Collections.DictionaryEntry dictionaryEntry = array[i];
                if (!flag)
                {
                    sb.Append(',');
                }
                string text = dictionaryEntry.Key as string;
                if (text == null)
                {
                    throw new System.ArgumentException(string.Format(System.Globalization.CultureInfo.InvariantCulture, "Type '{0}' is not supported for serialization/deserialization of a dictionary, keys must be strings or objects.", new object[]
					{
						o.GetType().FullName
					}));
                }
                JavaScriptSerializer.SerializeString((string)dictionaryEntry.Key, sb);
                sb.Append(':');
                this.SerializeValue(dictionaryEntry.Value, sb, depth, objectsInUse, serializationFormat);
                flag = false;
            }
            sb.Append('}');
        }

        private void SerializeEnumerable(System.Collections.IEnumerable enumerable, System.Text.StringBuilder sb, int depth, System.Collections.Hashtable objectsInUse, JavaScriptSerializer.SerializationFormat serializationFormat)
        {
            sb.Append('[');
            bool flag = true;
            System.Collections.Generic.List<object> list = new System.Collections.Generic.List<object>();
            foreach (object current in enumerable)
            {
                list.Add(current);
            }
            for (int i = 0; i < list.Count; i++)
            {
                if (!flag)
                {
                    sb.Append(',');
                }
                this.SerializeValue(list[i], sb, depth, objectsInUse, serializationFormat);
                flag = false;
            }
            sb.Append(']');
        }

        private static void SerializeString(string input, System.Text.StringBuilder sb)
        {
            sb.Append('"');
            sb.Append(JavaScriptString.QuoteString(input));
            sb.Append('"');
        }

        private void SerializeValue(object o, System.Text.StringBuilder sb, int depth, System.Collections.Hashtable objectsInUse, JavaScriptSerializer.SerializationFormat serializationFormat)
        {
            if (++depth > this._recursionLimit)
            {
                throw new System.ArgumentException("RecursionLimit exceeded.");
            }
            JavaScriptConverter javaScriptConverter = null;
            if (o != null && this.ConverterExistsForType(o.GetType(), out javaScriptConverter))
            {
                System.Collections.Generic.IDictionary<string, object> dictionary = javaScriptConverter.Serialize(o, this);
                if (this.TypeResolver != null)
                {
                    string text = this.TypeResolver.ResolveTypeId(o.GetType());
                    if (text != null)
                    {
                        dictionary["__type"] = text;
                    }
                }
                sb.Append(this.Serialize(dictionary, serializationFormat));
            }
            else
            {
                if (o is DataTable)
                {
                    o = null;
                }
                else if (o is DataRow)
                {
                    o = null;
                }
                this.SerializeValueInternal(o, sb, depth, objectsInUse, serializationFormat);
            }
        }

        private void SerializeValueInternal(object o, System.Text.StringBuilder sb, int depth, System.Collections.Hashtable objectsInUse, JavaScriptSerializer.SerializationFormat serializationFormat)
        {
            if (o == null || System.DBNull.Value.Equals(o))
            {
                sb.Append("null");
            }
            else
            {
                string text = o as string;
                if (text != null)
                {
                    JavaScriptSerializer.SerializeString(text, sb);
                }
                else if (o is char)
                {
                    if ((char)o == '\0')
                    {
                        sb.Append("null");
                    }
                    else
                    {
                        JavaScriptSerializer.SerializeString(o.ToString(), sb);
                    }
                }
                else if (o is bool)
                {
                    JavaScriptSerializer.SerializeBoolean((bool)o, sb);
                }
                else if (o is System.DateTime)
                {
                    JavaScriptSerializer.SerializeDateTime((System.DateTime)o, sb, serializationFormat);
                }
                else if (o is System.TimeSpan)
                {
                    JavaScriptSerializer.SerializeDateTime(System.Convert.ToDateTime(o.ToString()), sb, serializationFormat);
                }
                else if (o is ulong)
                {
                    JavaScriptSerializer.SerializeCuid((ulong)o, sb);
                }
                else if (o is long && serializationFormat == JavaScriptSerializer.SerializationFormat.JavaScript)
                {
                    JavaScriptSerializer.SerializeCuidForSqlServer((long)o, sb);
                }
                else if (o is System.Guid)
                {
                    JavaScriptSerializer.SerializeGuid((System.Guid)o, sb);
                }
                else if (o is RawText)
                {
                    sb.Append(((RawText)o).Value);
                }
                else
                {
                    Uri uri = o as Uri;
                    if (uri != null)
                    {
                        JavaScriptSerializer.SerializeUri(uri, sb);
                    }
                    else if (o is double)
                    {
                        sb.Append(((double)o).ToString("r", System.Globalization.CultureInfo.InvariantCulture));
                    }
                    else if (o is float)
                    {
                        sb.Append(((float)o).ToString("r", System.Globalization.CultureInfo.InvariantCulture));
                    }
                    else if (o.GetType().IsPrimitive || o is decimal)
                    {
                        System.IConvertible convertible = o as System.IConvertible;
                        if (convertible == null)
                        {
                            throw new System.InvalidOperationException();
                        }
                        sb.Append(convertible.ToString(System.Globalization.CultureInfo.InvariantCulture));
                    }
                    else
                    {
                        System.Type type = o.GetType();
                        if (type.IsEnum)
                        {
                            sb.Append((int)o);
                        }
                        else
                        {
                            try
                            {
                                if (objectsInUse == null)
                                {
                                    objectsInUse = new System.Collections.Hashtable(new JavaScriptSerializer.ReferenceComparer());
                                }
                                else if (objectsInUse.ContainsKey(o))
                                {
                                    throw new System.InvalidOperationException(string.Format(System.Globalization.CultureInfo.CurrentCulture, "A circular reference was detected while serializing an object of type '{0}'.", new object[]
									{
										type.FullName
									}));
                                }
                                objectsInUse.Add(o, null);
                                System.Collections.IDictionary dictionary = o as System.Collections.IDictionary;
                                if (dictionary != null)
                                {
                                    this.SerializeDictionary(dictionary, sb, depth, objectsInUse, serializationFormat);
                                }
                                else
                                {
                                    System.Collections.IEnumerable enumerable = o as System.Collections.IEnumerable;
                                    if (enumerable != null)
                                    {
                                        this.SerializeEnumerable(enumerable, sb, depth, objectsInUse, serializationFormat);
                                    }
                                    else
                                    {
                                        this.SerializeCustomObject(o, sb, depth, objectsInUse, serializationFormat);
                                    }
                                }
                            }
                            finally
                            {
                                if (objectsInUse != null)
                                {
                                    objectsInUse.Remove(o);
                                }
                            }
                        }
                    }
                }
            }
        }
    }


    internal class RawText
    {
        private string _value;

        public string Value
        {
            get
            {
                return this._value;
            }
            set
            {
                this._value = value;
            }
        }

        public RawText(string value)
        {
            this._value = value;
        }
    }
}
