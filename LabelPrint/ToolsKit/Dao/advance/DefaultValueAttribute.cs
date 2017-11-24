using System;

namespace PrintX.Dev.Utils.ToolsKit
{
	[System.AttributeUsage(System.AttributeTargets.All)]
	public sealed class DefaultValueAttribute : System.Attribute
	{
		private object value;

		public object Value
		{
			get
			{
				return this.value;
			}
		}

		public DefaultValueAttribute(string value)
		{
			this.value = value;
		}

		public DefaultValueAttribute(int value)
		{
			this.value = value;
		}

		public DefaultValueAttribute(bool value)
		{
			this.value = value;
		}

		public DefaultValueAttribute(double value)
		{
			this.value = value;
		}

		public DefaultValueAttribute(object value)
		{
			this.value = value;
		}
	}
}
