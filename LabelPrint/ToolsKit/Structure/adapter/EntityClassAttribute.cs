using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PrintX.Dev.Utils.ToolsKit
{
    [System.AttributeUsage(System.AttributeTargets.Class)]
    public class EntityClassAttribute : System.Attribute
    {
        private bool _lowerCaseKey = true;

        public bool LowerCaseKey
        {
            get
            {
                return this._lowerCaseKey;
            }
            set
            {
                this._lowerCaseKey = value;
            }
        }
    }
}
