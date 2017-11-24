using System;
using System.Reflection;

namespace PrintX.Dev.Utils.ToolsKit
{
	public sealed class PropInfo
	{
		private object obj;

		private System.Reflection.MemberInfo member;

		private System.Reflection.PropertyInfo prop;

		private System.Reflection.FieldInfo field;

		public string Name
		{
			get
			{
				return this.member.Name;
			}
		}

		public object Value
		{
			get
			{
				object value;
				if (this.prop != null)
				{
					value = this.prop.GetValue(this.obj, null);
				}
				else
				{
					if (!(this.field != null))
					{
						throw new System.InvalidOperationException();
					}
					value = this.field.GetValue(this.obj);
				}
				return value;
			}
			set
			{
				if (this.prop != null)
				{
					this.prop.SetValue(this.obj, value, null);
				}
				else if (this.field != null)
				{
					this.field.SetValue(this.obj, value);
				}
			}
		}

		public System.Type Type
		{
			get
			{
				return (this.prop != null) ? this.prop.PropertyType : this.field.FieldType;
			}
		}

		private PropInfo(object obj, System.Reflection.MemberInfo member)
		{
			this.obj = obj;
			this.member = member;
		}

		internal PropInfo(object obj, System.Reflection.PropertyInfo prop) 
		{
			
            this.obj = obj;
            this.prop = prop;
		}

		internal PropInfo(object obj, System.Reflection.FieldInfo field)
        {
            this.obj = obj;
			this.field = field;
		}
	}
}
