using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace PrintX.Dev.Utils.ToolsKit
{
	public static class ReflectionUtils
	{
		public static PropInfo FindProp(object obj, string name)
		{
			PropInfo result;
			if (obj == null)
			{
				result = null;
			}
			else
			{
				System.Reflection.BindingFlags bindingAttr = System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public;
				System.Type type = obj.GetType();
				System.Reflection.PropertyInfo property = type.GetProperty(name, bindingAttr);
				if (property != null)
				{
					result = new PropInfo(obj, property);
				}
				else
				{
					System.Reflection.FieldInfo field = type.GetField(name, bindingAttr);
					if (field != null)
					{
						result = new PropInfo(obj, field);
					}
					else
					{
						result = null;
					}
				}
			}
			return result;
		}

		public static PropInfo FindProp(object obj, System.Reflection.MemberInfo member)
		{
			PropInfo result;
			if (member is System.Reflection.PropertyInfo)
			{
				result = new PropInfo(obj, (System.Reflection.PropertyInfo)member);
			}
			else if (member is System.Reflection.FieldInfo)
			{
				result = new PropInfo(obj, (System.Reflection.FieldInfo)member);
			}
			else
			{
				result = null;
			}
			return result;
		}

		public static PropInfo[] GetProps(object obj)
		{
			System.Reflection.BindingFlags bindingAttr = System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public;
			System.Type type = obj.GetType();
			System.Collections.Generic.List<PropInfo> list = new System.Collections.Generic.List<PropInfo>();
			System.Reflection.PropertyInfo[] properties = type.GetProperties(bindingAttr);
			for (int i = 0; i < properties.Length; i++)
			{
				System.Reflection.PropertyInfo prop = properties[i];
				list.Add(new PropInfo(obj, prop));
			}
			System.Reflection.FieldInfo[] fields = type.GetFields(bindingAttr);
			for (int i = 0; i < fields.Length; i++)
			{
				System.Reflection.FieldInfo field = fields[i];
				list.Add(new PropInfo(obj, field));
			}
			PropInfo[] array = new PropInfo[list.Count];
			list.CopyTo(array);
			return array;
		}

		public static bool IsPrimitiveType(System.Type type)
		{
			return ReflectionUtils.DoGetIsPrimitiveType(type);
		}

		internal static bool IsPrimitiveType(System.Type type, out System.Type underlyingType)
		{
			underlyingType = type;
			bool flag = ReflectionUtils.DoGetIsPrimitiveType(type);
			bool result;
			if (flag)
			{
				result = true;
			}
			else if (type.IsValueType && type.IsGenericType)
			{
				underlyingType = System.Nullable.GetUnderlyingType(type);
				result = ReflectionUtils.DoGetIsPrimitiveType(underlyingType);
			}
			else
			{
				result = false;
			}
			return result;
		}

		private static bool DoGetIsPrimitiveType(System.Type type)
		{
			return type.IsPrimitive || type == typeof(string) || type == typeof(System.DateTime) || type == typeof(decimal);
		}

		public static PropInfo GetProp(object obj, string name)
		{
			PropInfo propInfo = ReflectionUtils.FindProp(obj, name);
			if (propInfo == null)
			{
				throw new System.Exception(ReflectionUtils.GetNoPropMessage(obj, name));
			}
			return propInfo;
		}

		public static object GetPropValue(object obj, string name)
		{
			PropInfo prop = ReflectionUtils.GetProp(obj, name);
			return prop.Value;
		}

		public static void SetPropValue(object obj, string name, object value)
		{
			PropInfo prop = ReflectionUtils.GetProp(obj, name);
			prop.Value = value;
		}

		public static string GetNoPropMessage(object obj, string name)
		{
			return ReflectionUtils.GetNoPropMessage(obj.GetType(), name);
		}

		public static string GetNoPropMessage(System.Type type, string name)
		{
			return string.Format("“{0}.{1}”属性不存在。", type.Name, name);
		}

		public static System.Type GetArrayElementType(System.Type type)
		{
			System.Type result;
			if (type.IsArray)
			{
				result = type.GetElementType();
			}
			else if (typeof(System.Collections.ICollection).IsAssignableFrom(type))
			{
				result = ReflectionUtils.GetCollectionElementType(type);
			}
			else if (typeof(System.Collections.IEnumerable).IsAssignableFrom(type))
			{
				result = ReflectionUtils.GetEnumeratorElementType(type);
			}
			else
			{
				result = null;
			}
			return result;
		}

		private static System.Type GetCollectionElementType(System.Type type)
		{
			System.Reflection.PropertyInfo defaultIndexer = ReflectionUtils.GetDefaultIndexer(type);
			return (defaultIndexer != null) ? defaultIndexer.PropertyType : null;
		}

		private static System.Reflection.PropertyInfo GetDefaultIndexer(System.Type type)
		{
			System.Reflection.PropertyInfo result;
			if (typeof(System.Collections.IDictionary).IsAssignableFrom(type))
			{
				result = null;
			}
			else
			{
				System.Reflection.MemberInfo[] defaultMembers = type.GetDefaultMembers();
				System.Reflection.PropertyInfo propertyInfo = null;
				if (defaultMembers != null && defaultMembers.Length > 0)
				{
					System.Type type2 = type;
					while (type2 != null)
					{
						for (int i = 0; i < defaultMembers.Length; i++)
						{
							if (defaultMembers[i] is System.Reflection.PropertyInfo)
							{
								System.Reflection.PropertyInfo propertyInfo2 = (System.Reflection.PropertyInfo)defaultMembers[i];
								if (propertyInfo2.DeclaringType == type2 && propertyInfo2.CanRead)
								{
									System.Reflection.ParameterInfo[] parameters = propertyInfo2.GetGetMethod().GetParameters();
									if (parameters.Length == 1 && parameters[0].ParameterType == typeof(int))
									{
										propertyInfo = propertyInfo2;
										break;
									}
								}
							}
						}
						if (propertyInfo != null)
						{
							break;
						}
						type2 = type2.BaseType;
					}
				}
				if (propertyInfo == null)
				{
					throw new System.InvalidOperationException(string.Format("You must implement a default accessor on {0} because it inherits from ICollection.", type.FullName));
				}
				result = propertyInfo;
			}
			return result;
		}

		private static System.Type GetEnumeratorElementType(System.Type type)
		{
			System.Type result;
			if (type.IsGenericType && type.GetGenericArguments().Length == 1)
			{
				result = type.GetGenericArguments()[0];
			}
			else if (!typeof(System.Collections.IEnumerable).IsAssignableFrom(type))
			{
				result = null;
			}
			else
			{
				System.Reflection.MethodInfo methodInfo = type.GetMethod("GetEnumerator", new System.Type[0]);
				if (methodInfo == null || !typeof(System.Collections.IEnumerator).IsAssignableFrom(methodInfo.ReturnType))
				{
					methodInfo = null;
					System.Reflection.MemberInfo[] member = type.GetMember("System.Collections.Generic.IEnumerable<*", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic);
					for (int i = 0; i < member.Length; i++)
					{
						System.Reflection.MemberInfo memberInfo = member[i];
						methodInfo = (memberInfo as System.Reflection.MethodInfo);
						if (methodInfo != null && typeof(System.Collections.IEnumerator).IsAssignableFrom(methodInfo.ReturnType))
						{
							break;
						}
						methodInfo = null;
					}
					if (methodInfo == null)
					{
						methodInfo = type.GetMethod("System.Collections.IEnumerable.GetEnumerator", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic, null, new System.Type[0], null);
					}
				}
				if (methodInfo == null || !typeof(System.Collections.IEnumerator).IsAssignableFrom(methodInfo.ReturnType))
				{
					result = null;
				}
				else
				{
					System.Reflection.PropertyInfo property = methodInfo.ReturnType.GetProperty("Current");
					System.Type type2 = (property == null) ? typeof(object) : property.PropertyType;
					System.Reflection.MethodInfo method = type.GetMethod("Add", new System.Type[]
					{
						type2
					});
					if (method == null && type2 != typeof(object))
					{
						type2 = typeof(object);
					}
					result = type2;
				}
			}
			return result;
		}
	}
}
