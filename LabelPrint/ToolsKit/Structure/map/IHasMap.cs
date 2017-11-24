using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PrintX.Dev.Utils.ToolsKit
{
    public interface IHashMap : System.Collections.Generic.IDictionary<string, object>, System.Collections.Generic.ICollection<System.Collections.Generic.KeyValuePair<string, object>>, System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<string, object>>, System.Collections.IEnumerable
    {
        IHashMap Clone();

        void Add(string[] keys, params object[] values);

        void CopyTo(System.Collections.Generic.IDictionary<string, object> dict);

        T GetValue<T>(string key);

        T GetValue<T>(string key, T defaultValue);

        bool CheckSetValue(string key, object value);

        T ToEntity<T>() where T : class, new();
    }
}
