
using System;
using System.Xml;

namespace PrintX.Dev.Utils.ToolsKit
{
	public class ConfigSection
	{
		private SktSettingsSection owner;

		private XmlNode node;

		private Serializer serializer;

		public string this[string keyName]
		{
			get
			{
				return this.GetString(keyName);
			}
			set
			{
				this.SetString(keyName, value);
			}
		}

		public string Text
		{
			get
			{
				return (this.node != null) ? this.node.InnerText : null;
			}
		}

		internal ConfigSection(SktSettingsSection owner, XmlNode root, XmlNode node, string sectionName)
		{
			this.owner = owner;
			this.node = node;
			this.serializer = new Serializer(root, node, sectionName);
		}

		public string GetString(string keyName)
		{
			string result;
			if (this.node != null)
			{
				XmlAttribute xmlAttribute = this.node.Attributes[keyName];
				result = ((xmlAttribute != null) ? xmlAttribute.Value : null);
			}
			else
			{
				result = null;
			}
			return result;
		}

		public string GetString(string keyName, string defaultValue)
		{
			string @string = this.GetString(keyName);
			return (@string != null) ? @string : defaultValue;
		}

		public string DecryptGetString(string keyName)
		{
			string @string = this.GetString(keyName);
			return (@string != null) ? StringUtils.Decrypt(@string) : string.Empty;
		}

		public int GetInteger(string keyName)
		{
			return this.GetInteger(keyName, 0);
		}

		public int GetInteger(string keyName, int defaultValue)
		{
			string @string = this.GetString(keyName);
			return string.IsNullOrEmpty(@string) ? defaultValue : int.Parse(@string);
		}

		public bool GetBoolean(string keyName)
		{
			return this.GetBoolean(keyName, false);
		}

		public bool GetBoolean(string keyName, bool defaultValue)
		{
			string @string = this.GetString(keyName);
			return string.IsNullOrEmpty(@string) ? defaultValue : bool.Parse(@string);
		}

		public double GetDouble(string keyName)
		{
			return this.GetDouble(keyName, 0.0);
		}

		public double GetDouble(string keyName, double defaultValue)
		{
			string @string = this.GetString(keyName);
			return string.IsNullOrEmpty(@string) ? defaultValue : double.Parse(@string);
		}

		public System.DateTime GetDateTime(string keyName)
		{
			return this.GetDateTime(keyName, System.DateTime.MinValue);
		}

		public System.DateTime GetDateTime(string keyName, System.DateTime defaultValue)
		{
			string @string = this.GetString(keyName);
			return string.IsNullOrEmpty(@string) ? defaultValue : System.DateTime.Parse(@string);
		}

		public T GetObject<T>() where T : new()
		{
			T t = default(T);
			if (this.node != null)
			{
				t = ((default(T) == null) ? System.Activator.CreateInstance<T>() : default(T));
				this.serializer.DeserializeTo(t);
			}
			return t;
		}

		public void SetString(string keyName, string value)
		{
			this.serializer.SetString(keyName, value);
			this.owner.Modified();
		}

		public void EncryptSetString(string keyName, string value)
		{
			this.SetString(keyName, StringUtils.Encrypt(value));
		}

		public void SetInteger(string keyName, int value)
		{
			this.SetString(keyName, value.ToString());
		}

		public void SetBoolean(string keyName, bool value)
		{
			this.SetString(keyName, value.ToString().ToLower());
		}

		public void SetDouble(string keyName, double value)
		{
			this.SetString(keyName, value.ToString());
		}

		public void SetDateTime(string keyName, System.DateTime value)
		{
			bool flag = 1 == 0;
			this.SetString(keyName, value.ToString());
		}

		public void SetObject(object obj)
		{
			this.serializer.Serialize(obj);
			this.owner.Modified();
		}
	}
}
