using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace PrintX.Dev.Utils.ToolsKit
{
    public interface IHashMapList : System.Collections.Generic.IList<IHashMap>, System.Collections.Generic.ICollection<IHashMap>, System.Collections.Generic.IEnumerable<IHashMap>, System.Collections.IEnumerable
    {
        void Add(string[] keys, params object[] values);

        IHashMapList GetRange(int index, int count);

        void Sort(string key, bool ascending);

        IHashMap Find(string key, object value);

        System.Collections.Generic.IList<T> ToEntitys<T>() where T : class, new();

        DataTable HashMapListToDataTable(HashMapList list);
    }
}
