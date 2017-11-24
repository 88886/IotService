using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PrintX.Dev.Utils.ToolsKit
{
    public abstract class JavaScriptTypeResolver
    {
        public abstract System.Type ResolveType(string id);

        public abstract string ResolveTypeId(System.Type type);
    }
}
