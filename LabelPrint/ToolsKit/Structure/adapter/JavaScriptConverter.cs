using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PrintX.Dev.Utils.ToolsKit
{




    public abstract class JavaScriptConverter
    {
        public abstract System.Collections.Generic.IEnumerable<System.Type> SupportedTypes
        {
            get;
        }

        public abstract object Deserialize(System.Collections.Generic.IDictionary<string, object> dictionary, System.Type type, JavaScriptSerializer serializer);

        public abstract System.Collections.Generic.IDictionary<string, object> Serialize(object obj, JavaScriptSerializer serializer);
    }
}
