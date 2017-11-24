using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace PrintX.Dev.Utils.ToolsKit
{
    internal static class ObjectConverter
    {
        private static readonly System.Type[] s_emptyTypeArray = new System.Type[0];

        private static System.Type _listGenericType = typeof(System.Collections.Generic.List<>);

        private static System.Type _enumerableGenericType = typeof(System.Collections.Generic.IEnumerable<>);

        private static System.Type _dictionaryGenericType = typeof(System.Collections.Generic.Dictionary<,>);

        private static System.Type _idictionaryGenericType = typeof(System.Collections.Generic.IDictionary<,>);

        internal static bool IsClientInstantiatableType(System.Type t, JavaScriptSerializer serializer)
        {
            bool result;
            if (t == null || t.IsAbstract || t.IsInterface || t.IsArray)
            {
                result = false;
            }
            else if (t == typeof(object))
            {
                result = false;
            }
            else
            {
                JavaScriptConverter javaScriptConverter = null;
                if (serializer.ConverterExistsForType(t, out javaScriptConverter))
                {
                    result = true;
                }
                else if (t.IsValueType)
                {
                    result = true;
                }
                else
                {
                    System.Reflection.ConstructorInfo constructor = t.GetConstructor(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public, null, ObjectConverter.s_emptyTypeArray, null);
                    result = !(constructor == null);
                }
            }
            return result;
        }

        private static void AddItemToList(System.Collections.IList oldList, System.Collections.IList newList, System.Type elementType, JavaScriptSerializer serializer)
        {
            foreach (object current in oldList)
            {
                newList.Add(ObjectConverter.ConvertObjectToType(current, elementType, serializer));
            }
        }

        private static void AssignToPropertyOrField(object propertyValue, object o, string memberName, JavaScriptSerializer serializer)
        {
            System.Collections.IDictionary dictionary = o as System.Collections.IDictionary;
            if (dictionary != null)
            {
                propertyValue = ObjectConverter.ConvertObjectToType(propertyValue, null, serializer);
                dictionary[memberName] = propertyValue;
            }
            else
            {
                System.Type type = o.GetType();
                System.Reflection.PropertyInfo property = type.GetProperty(memberName, System.Reflection.BindingFlags.IgnoreCase | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public);
                if (property != null)
                {
                    System.Reflection.MethodInfo setMethod = property.GetSetMethod();
                    if (setMethod != null)
                    {
                        propertyValue = ObjectConverter.ConvertObjectToType(propertyValue, property.PropertyType, serializer);
                        setMethod.Invoke(o, new object[]
						{
							propertyValue
						});
                        return;
                    }
                }
                System.Reflection.FieldInfo field = type.GetField(memberName, System.Reflection.BindingFlags.IgnoreCase | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public);
                if (field != null)
                {
                    propertyValue = ObjectConverter.ConvertObjectToType(propertyValue, field.FieldType, serializer);
                    field.SetValue(o, propertyValue);
                }
            }
        }

        private static object ConvertDictionaryToObject(System.Collections.Generic.IDictionary<string, object> dictionary, System.Type type, JavaScriptSerializer serializer)
        {
            System.Type type2 = type;
            string text = null;
            object obj = dictionary;
            object o;
            if (dictionary.TryGetValue("__type", out o))
            {
                text = (ObjectConverter.ConvertObjectToType(o, typeof(string), serializer) as string);
                if (text != null)
                {
                    if (serializer.TypeResolver != null)
                    {
                        type2 = serializer.TypeResolver.ResolveType(text);
                        if (type2 == null)
                        {
                            throw new System.InvalidOperationException();
                        }
                    }
                    dictionary.Remove("__type");
                }
            }
            JavaScriptConverter javaScriptConverter = null;
            object result;
            if (type2 != null && serializer.ConverterExistsForType(type2, out javaScriptConverter))
            {
                result = javaScriptConverter.Deserialize(dictionary, type2, serializer);
            }
            else
            {
                if (text != null || (type2 != null && ObjectConverter.IsClientInstantiatableType(type2, serializer)))
                {
                    obj = System.Activator.CreateInstance(type2);
                }
                System.Collections.Generic.List<string> list = new System.Collections.Generic.List<string>(dictionary.Keys);
                if (type != null && type.IsGenericType && (typeof(System.Collections.IDictionary).IsAssignableFrom(type) || type.GetGenericTypeDefinition() == ObjectConverter._idictionaryGenericType) && type.GetGenericArguments().Length == 2)
                {
                    System.Type type3 = type.GetGenericArguments()[0];
                    if (type3 != typeof(string) && type3 != typeof(object))
                    {
                        throw new System.InvalidOperationException(string.Format(System.Globalization.CultureInfo.InvariantCulture, "Type '{0}' is not supported for serialization/deserialization of a dictionary, keys must be strings or objects.", new object[]
						{
							type.FullName
						}));
                    }
                    System.Type type4 = type.GetGenericArguments()[1];
                    System.Collections.IDictionary dictionary2 = null;
                    if (ObjectConverter.IsClientInstantiatableType(type, serializer))
                    {
                        dictionary2 = (System.Collections.IDictionary)System.Activator.CreateInstance(type);
                    }
                    else
                    {
                        System.Type type5 = ObjectConverter._dictionaryGenericType.MakeGenericType(new System.Type[]
						{
							type3,
							type4
						});
                        dictionary2 = (System.Collections.IDictionary)System.Activator.CreateInstance(type5);
                    }
                    if (dictionary2 != null)
                    {
                        foreach (string current in list)
                        {
                            dictionary2[current] = ObjectConverter.ConvertObjectToType(dictionary[current], type4, serializer);
                        }
                        result = dictionary2;
                        return result;
                    }
                }
                if (type != null && !type.IsAssignableFrom(obj.GetType()))
                {
                    System.Reflection.ConstructorInfo constructor = type.GetConstructor(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public, null, ObjectConverter.s_emptyTypeArray, null);
                    if (constructor == null)
                    {
                        throw new System.MissingMethodException(string.Format(System.Globalization.CultureInfo.InvariantCulture, "No parameterless constructor defined for type of '{0}'.", new object[]
						{
							type.FullName
						}));
                    }
                    throw new System.InvalidOperationException(string.Format(System.Globalization.CultureInfo.InvariantCulture, "Cannot deserialize object graph into type of '{0}'.", new object[]
					{
						type.FullName
					}));
                }
                else
                {
                    foreach (string current in list)
                    {
                        object propertyValue = dictionary[current];
                        ObjectConverter.AssignToPropertyOrField(propertyValue, obj, current, serializer);
                    }
                    result = obj;
                }
            }
            return result;
        }

        internal static object ConvertObjectToType(object o, System.Type type, JavaScriptSerializer serializer)
        {
            object result;
            if (o == null)
            {
                if (type == typeof(char))
                {
                    result = '\0';
                }
                else
                {
                    if (type != null && type.IsValueType && (!type.IsGenericType || !(type.GetGenericTypeDefinition() == typeof(System.Nullable<>))))
                    {
                        throw new System.InvalidOperationException("Cannot convert null to a value type.");
                    }
                    result = null;
                }
            }
            else if (o.GetType() == type)
            {
                result = o;
            }
            else
            {
                result = ObjectConverter.ConvertObjectToTypeInternal(o, type, serializer);
            }
            return result;
        }

        private static object ConvertObjectToTypeInternal(object o, System.Type type, JavaScriptSerializer serializer)
        {
            System.Collections.Generic.IDictionary<string, object> dictionary = o as System.Collections.Generic.IDictionary<string, object>;
            object result;
            if (dictionary != null)
            {
                result = ObjectConverter.ConvertDictionaryToObject(dictionary, type, serializer);
            }
            else
            {
                System.Collections.IList list = o as System.Collections.IList;
                if (list != null)
                {
                    result = ObjectConverter.ConvertListToObject(list, type, serializer);
                }
                else if (type == null || o.GetType() == type)
                {
                    result = o;
                }
                else
                {
                    TypeConverter converter = TypeDescriptor.GetConverter(type);
                    if (converter.CanConvertFrom(o.GetType()))
                    {
                        result = converter.ConvertFrom(null, System.Globalization.CultureInfo.InvariantCulture, o);
                    }
                    else if (converter.CanConvertFrom(typeof(string)))
                    {
                        TypeConverter converter2 = TypeDescriptor.GetConverter(o);
                        string text = converter2.ConvertToInvariantString(o);
                        result = converter.ConvertFromInvariantString(text);
                    }
                    else
                    {
                        if (!type.IsAssignableFrom(o.GetType()))
                        {
                            throw new System.InvalidOperationException(string.Format(System.Globalization.CultureInfo.CurrentCulture, "Cannot convert object of type '{0}' to type '{1}'", new object[]
							{
								o.GetType(),
								type
							}));
                        }
                        result = o;
                    }
                }
            }
            return result;
        }

        private static System.Collections.IList ConvertListToObject(System.Collections.IList list, System.Type type, JavaScriptSerializer serializer)
        {
            System.Type type2;
            System.Collections.IList result;
            if (!(type == null) && !(type == typeof(object)) && !type.IsArray && !(type == typeof(System.Collections.ArrayList)) && !(type == typeof(System.Collections.IEnumerable)) && !(type == typeof(System.Collections.IList)) && !(type == typeof(System.Collections.ICollection)))
            {
                if (type.IsGenericType && type.GetGenericArguments().Length == 1)
                {
                    type2 = type.GetGenericArguments()[0];
                    System.Type type3 = ObjectConverter._enumerableGenericType.MakeGenericType(new System.Type[]
					{
						type2
					});
                    if (type3.IsAssignableFrom(type))
                    {
                        System.Type type4 = ObjectConverter._listGenericType.MakeGenericType(new System.Type[]
						{
							type2
						});
                        System.Collections.IList list2;
                        if (ObjectConverter.IsClientInstantiatableType(type, serializer) && typeof(System.Collections.IList).IsAssignableFrom(type))
                        {
                            list2 = (System.Collections.IList)System.Activator.CreateInstance(type);
                        }
                        else
                        {
                            if (type4.IsAssignableFrom(type))
                            {
                                throw new System.InvalidOperationException(string.Format(System.Globalization.CultureInfo.InvariantCulture, "Cannot create instance of {0}.", new object[]
								{
									type.FullName
								}));
                            }
                            list2 = (System.Collections.IList)System.Activator.CreateInstance(type4);
                        }
                        ObjectConverter.AddItemToList(list, list2, type2, serializer);
                        result = list2;
                        return result;
                    }
                }
                else if (ObjectConverter.IsClientInstantiatableType(type, serializer) && typeof(System.Collections.IList).IsAssignableFrom(type))
                {
                    System.Collections.IList list2 = (System.Collections.IList)System.Activator.CreateInstance(type);
                    ObjectConverter.AddItemToList(list, list2, null, serializer);
                    result = list2;
                    return result;
                }
                throw new System.InvalidOperationException(string.Format(System.Globalization.CultureInfo.CurrentCulture, "Type '{0}' is not supported for deserialization of an array.", new object[]
				{
					type.FullName
				}));
            }
            type2 = typeof(object);
            if (type != null && type != typeof(object))
            {
                type2 = type.GetElementType();
            }
            System.Collections.ArrayList arrayList = new System.Collections.ArrayList();
            ObjectConverter.AddItemToList(list, arrayList, type2, serializer);
            if (type == typeof(System.Collections.ArrayList) || type == typeof(System.Collections.IEnumerable) || type == typeof(System.Collections.IList) || type == typeof(System.Collections.ICollection))
            {
                result = arrayList;
            }
            else
            {
                result = arrayList.ToArray(type2);
            }
            return result;
        }
    }
}
