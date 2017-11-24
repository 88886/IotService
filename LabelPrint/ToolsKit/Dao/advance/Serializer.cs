
using System;
using System.Reflection;
using System.Xml;

namespace PrintX.Dev.Utils.ToolsKit
{
	internal class Serializer
	{
		private XmlNode parent;

		private XmlNode node;

		private string nodeName;

		public Serializer(XmlNode parent, XmlNode node, string nodeName)
		{
			this.parent = parent;
			this.node = node;
			this.nodeName = nodeName;
		}

		private XmlAttribute AddAttribute(XmlNode node, string keyName)
		{
			XmlAttribute result = node.OwnerDocument.CreateAttribute(keyName);
			node.Attributes.Append(result);
			return result;
		}

		private XmlNode AddChildNode(XmlNode parentNode, string nodeName)
		{
			XmlNode xmlNode = parentNode.OwnerDocument.CreateElement(nodeName);
			parentNode.AppendChild(xmlNode);
			return xmlNode;
		}

		private XmlNode AddChildNode()
		{
			return this.AddChildNode(this.parent, this.nodeName);
		}

		public void SetString(string keyName, string value)
		{
			bool flag = string.IsNullOrEmpty(value);
			if (this.node != null)
			{
				XmlAttribute xmlAttribute = this.node.Attributes[keyName];
				if (xmlAttribute == null)
				{
					if (!flag)
					{
						xmlAttribute = this.AddAttribute(this.node, keyName);
						xmlAttribute.Value = value;
					}
				}
				else if (flag)
				{
					this.node.Attributes.Remove(xmlAttribute);
				}
				else
				{
					xmlAttribute.Value = value;
				}
			}
			else if (!flag)
			{
				this.node = this.AddChildNode();
				XmlAttribute xmlAttribute = this.AddAttribute(this.node, keyName);
				xmlAttribute.Value = value;
			}
		}

		public static string GetAttrName(string name)
		{
			return name.Substring(0, 1).ToLower() + name.Substring(1);
		}

		public void Serialize(object obj)
		{
			if (obj == null)
			{
				if (this.node != null)
				{
					this.node.Attributes.RemoveAll();
				}
			}
			else
			{
				System.Type type = obj.GetType();
				System.Reflection.BindingFlags bindingAttr = System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public;
				System.Reflection.PropertyInfo[] properties = type.GetProperties(bindingAttr);
				System.Reflection.PropertyInfo[] array = properties;
				for (int i = 0; i < array.Length; i++)
				{
					System.Reflection.PropertyInfo propertyInfo = array[i];
					if (propertyInfo.CanWrite)
					{
						string attrName = Serializer.GetAttrName(propertyInfo.Name);
						System.Type propertyType = propertyInfo.PropertyType;
						object value = propertyInfo.GetValue(obj, null);
						if (ReflectionUtils.IsPrimitiveType(propertyType))
						{
							string propStr = this.GetPropStr(propertyInfo, value);
							this.SetString(attrName, propStr);
						}
						else
						{
							this.SerializeObject(attrName, propertyType, value);
						}
					}
				}
				System.Reflection.FieldInfo[] fields = type.GetFields(bindingAttr);
				System.Reflection.FieldInfo[] array2 = fields;
				for (int i = 0; i < array2.Length; i++)
				{
					System.Reflection.FieldInfo fieldInfo = array2[i];
					string attrName = Serializer.GetAttrName(fieldInfo.Name);
					System.Type fieldType = fieldInfo.FieldType;
					object value = fieldInfo.GetValue(obj);
					if (ReflectionUtils.IsPrimitiveType(fieldType))
					{
						string propStr = this.GetPropStr(fieldInfo, value);
						this.SetString(attrName, propStr);
					}
					else
					{
						this.SerializeObject(attrName, fieldType, value);
					}
				}
			}
		}

		private string GetChildNodeName(string name)
		{
			return name.Substring(0, name.Length - 1);
		}

		private void SerializeObject(string name, System.Type type, object obj)
		{
			if (obj != null)
			{
				if (this.node == null)
				{
					this.node = this.AddChildNode();
				}
				if (!type.IsArray)
				{
					new Serializer(this.node, this.AddChildNode(this.node, name), name).Serialize(obj);
				}
				else
				{
					if (!(obj is System.Array))
					{
						throw new System.Exception("不能保存 " + obj.ToString());
					}
					System.Array array = (System.Array)obj;
					name = this.GetChildNodeName(name);
					foreach (object current in array)
					{
						new Serializer(this.node, this.AddChildNode(this.node, name), name).Serialize(current);
					}
				}
			}
			else if (this.node != null)
			{
				if (!type.IsArray)
				{
					XmlNode xmlNode = this.node.SelectSingleNode(name);
					if (xmlNode != null)
					{
						this.node.RemoveChild(xmlNode);
					}
				}
				else
				{
					name = this.GetChildNodeName(name);
					XmlNodeList xmlNodeList = this.node.SelectNodes(name);
					foreach (XmlNode xmlNode in xmlNodeList)
					{
						this.node.RemoveChild(xmlNode);
					}
				}
			}
		}

		private string GetPropStr(System.Reflection.MemberInfo member, object value)
		{
			object[] customAttributes = member.GetCustomAttributes(false);
			object[] array = customAttributes;
			string result;
			for (int i = 0; i < array.Length; i++)
			{
				object obj = array[i];
				if (obj is DefaultValueAttribute)
				{
					object value2 = ((DefaultValueAttribute)obj).Value;
					if (object.Equals(value, value2))
					{
						result = "";
						return result;
					}
				}
			}
			if (value == null)
			{
				result = "";
				return result;
			}
			string text = value.ToString();
			if (value is bool)
			{
				text = text.ToLower();
			}
			result = text;
			return result;
		}

		public void DeserializeTo(object obj)
		{
			this.DeserializeTo(obj.GetType(), obj);
		}

		private void DeserializeTo(System.Type type, object obj)
		{
			System.Reflection.BindingFlags bindingAttr = System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public;
			System.Reflection.PropertyInfo[] properties = type.GetProperties(bindingAttr);
			System.Reflection.PropertyInfo[] array = properties;
			for (int i = 0; i < array.Length; i++)
			{
				System.Reflection.PropertyInfo propertyInfo = array[i];
				if (propertyInfo.CanWrite)
				{
					string attrName = Serializer.GetAttrName(propertyInfo.Name);
					System.Type propertyType = propertyInfo.PropertyType;
					if (ReflectionUtils.IsPrimitiveType(propertyType))
					{
						object value;
						if (this.GetAttrValue(propertyInfo, propertyType, attrName, out value))
						{
							propertyInfo.SetValue(obj, value, null);
						}
					}
					else
					{
						object value = this.DeserializeObject(attrName, propertyType);
						propertyInfo.SetValue(obj, value, null);
					}
				}
			}
			System.Reflection.FieldInfo[] fields = type.GetFields(bindingAttr);
			System.Reflection.FieldInfo[] array2 = fields;
			for (int i = 0; i < array2.Length; i++)
			{
				System.Reflection.FieldInfo fieldInfo = array2[i];
				string attrName = Serializer.GetAttrName(fieldInfo.Name);
				System.Type fieldType = fieldInfo.FieldType;
				if (ReflectionUtils.IsPrimitiveType(fieldType))
				{
					object value;
					if (this.GetAttrValue(fieldInfo, fieldType, attrName, out value))
					{
						fieldInfo.SetValue(obj, value);
					}
				}
				else
				{
					object value = this.DeserializeObject(attrName, fieldType);
					fieldInfo.SetValue(obj, value);
				}
			}
		}

		private object DeserializeObject(string name, System.Type type)
		{
			object result;
			if (this.node != null)
			{
				if (!type.IsArray)
				{
					XmlNode xmlNode = this.node.SelectSingleNode(name);
					if (xmlNode != null)
					{
						object obj = System.Activator.CreateInstance(type);
						new Serializer(this.node, xmlNode, name).DeserializeTo(type, obj);
						result = obj;
						return result;
					}
				}
				else
				{
					if (type.BaseType == typeof(System.Array))
					{
						name = this.GetChildNodeName(name);
						XmlNodeList xmlNodeList = this.node.SelectNodes(name);
						System.Type elementType = type.GetElementType();
						System.Array array = System.Array.CreateInstance(elementType, xmlNodeList.Count);
						for (int i = 0; i < xmlNodeList.Count; i++)
						{
							object obj2 = System.Activator.CreateInstance(elementType);
							new Serializer(this.node, xmlNodeList[i], name).DeserializeTo(elementType, obj2);
							array.SetValue(obj2, i);
						}
						result = array;
						return result;
					}
					throw new System.Exception("不能读取 " + type.ToString());
				}
			}
			result = null;
			return result;
		}

		private bool GetAttrValue(System.Reflection.MemberInfo member, System.Type type, string name, out object value)
		{
			value = null;
			XmlAttribute xmlAttribute = this.node.Attributes[name];
			bool result;
			if (xmlAttribute != null)
			{
				string value2 = xmlAttribute.Value;
				if (!string.IsNullOrEmpty(value2))
				{
					value = System.Convert.ChangeType(value2, type);
					result = true;
					return result;
				}
			}
			else
			{
				object[] customAttributes = member.GetCustomAttributes(false);
				object[] array = customAttributes;
				for (int i = 0; i < array.Length; i++)
				{
					object obj = array[i];
					if (obj is DefaultValueAttribute)
					{
						value = ((DefaultValueAttribute)obj).Value;
						result = true;
						return result;
					}
				}
			}
			result = false;
			return result;
		}
	}
}
