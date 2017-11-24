using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PrintX.Dev.Utils.ToolsKit
{
    [System.AttributeUsage(System.AttributeTargets.Property)]
    public class FieldAttribute : System.Attribute
    {
        private string fieldName;

        public string FieldName
        {
            get
            {
                return this.fieldName;
            }
        }

        public FieldAttribute(string fieldName)
        {
            this.fieldName = fieldName;
        }
    }
}
