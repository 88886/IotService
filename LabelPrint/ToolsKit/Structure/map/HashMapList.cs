using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace PrintX.Dev.Utils.ToolsKit
{
    [System.Serializable]
    public class HashMapList : System.Collections.Generic.List<IHashMap>, IHashMapList, System.Collections.Generic.IList<IHashMap>, System.Collections.Generic.ICollection<IHashMap>, System.Collections.Generic.IEnumerable<IHashMap>, System.Collections.IEnumerable
    {
        private bool hasLongValue;

        [System.NonSerialized]
        private System.Collections.Generic.IDictionary<string, System.Collections.Hashtable> indexMap;

        public HashMapList()
        {
        }

        public HashMapList(int capacity)
            : base(capacity)
        {
        }

        public void Add(string[] keys, params object[] values)
        {
            HashMap item = new HashMap(keys, values);
            this.Add(item);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            bool result;
            if (obj == null)
            {
                result = false;
            }
            else if (!(obj is HashMapList))
            {
                result = false;
            }
            else
            {
                HashMapList hashObjectList = (HashMapList)obj;
                if (base.Count != hashObjectList.Count)
                {
                    result = false;
                }
                else
                {
                    for (int i = 0; i < base.Count; i++)
                    {
                        if (!base[i].Equals(hashObjectList[i]))
                        {
                            result = false;
                            return result;
                        }
                    }
                    result = true;
                }
            }
            return result;
        }

        public new IHashMapList GetRange(int index, int count)
        {
            System.Collections.Generic.List<IHashMap> range = base.GetRange(index, count);
            IHashMapList hashObjectList = new HashMapList(count);
            foreach (IHashMap current in range)
            {
                hashObjectList.Add(current);
            }
            return hashObjectList;
        }

        public void Sort(string key, bool ascending)
        {
            base.Sort(delegate(IHashMap obj1, IHashMap obj2)
            {
                System.IComparable value = obj1.GetValue<System.IComparable>(key);
                System.IComparable value2 = obj2.GetValue<System.IComparable>(key);
                int result;
                if (value == null)
                {
                    result = (ascending ? -1 : 1);
                }
                else if (value2 == null)
                {
                    result = (ascending ? 1 : -1);
                }
                else
                {
                    result = (ascending ? value.CompareTo(value2) : value2.CompareTo(value));
                }
                return result;
            });
        }

        public new void Add(IHashMap item)
        {
            base.Add(item);
            if (this.indexMap != null)
            {
                this.AddIndexItem(item);
            }
        }

        public new void Insert(int index, IHashMap item)
        {
            base.Insert(index, item);
            if (this.indexMap != null)
            {
                this.AddIndexItem(item);
            }
        }

        public new bool Remove(IHashMap item)
        {
            bool result = base.Remove(item);
            if (this.indexMap != null)
            {
                this.RemoveIndexItem(item);
            }
            return result;
        }

        public new void RemoveAt(int index)
        {
            if (this.indexMap != null)
            {
                this.RemoveIndexItem(base[index]);
            }
            base.RemoveAt(index);
        }

        public new void RemoveRange(int index, int count)
        {
            base.RemoveRange(index, count);
            this.indexMap = null;
        }

        public new int RemoveAll(System.Predicate<IHashMap> match)
        {
            this.indexMap = null;
            return this.RemoveAll(match);
        }

        public new void AddRange(System.Collections.Generic.IEnumerable<IHashMap> collection)
        {
            base.AddRange(collection);
            this.indexMap = null;
        }

        public new void InsertRange(int index, System.Collections.Generic.IEnumerable<IHashMap> collection)
        {
            base.InsertRange(index, collection);
            this.indexMap = null;
        }

        public new void Clear()
        {
            base.Clear();
            this.indexMap = null;
        }

        private void AddIndexItem(IHashMap item)
        {
            if (this.indexMap != null)
            {
                foreach (System.Collections.Generic.KeyValuePair<string, System.Collections.Hashtable> current in this.indexMap)
                {
                    string key = current.Key;
                    System.Collections.Hashtable value = current.Value;
                    object key2;
                    if (item.TryGetValue(key, out key2))
                    {
                        value[key2] = item;
                    }
                }
            }
        }

        private void RemoveIndexItem(IHashMap item)
        {
            if (this.indexMap != null)
            {
                foreach (System.Collections.Generic.KeyValuePair<string, System.Collections.Hashtable> current in this.indexMap)
                {
                    string key = current.Key;
                    System.Collections.Hashtable value = current.Value;
                    object key2;
                    if (item.TryGetValue(key, out key2))
                    {
                        value.Remove(key2);
                    }
                }
            }
        }

        public IHashMap Find(string key, object value)
        {
            System.Collections.Hashtable index = this.GetIndex(key);
            IHashMap hashObject = (IHashMap)index[value];
            if (hashObject == null && value != null && value.GetType() == typeof(ulong) && this.hasLongValue)
            {
                long num = System.Convert.ToInt64(value);
                hashObject = (IHashMap)index[num];
            }
            return hashObject;
        }

        private System.Collections.Hashtable GetIndex(string key)
        {
            if (this.indexMap == null)
            {
                this.indexMap = new System.Collections.Generic.Dictionary<string, System.Collections.Hashtable>();
            }
            System.Collections.Hashtable hashtable;
            if (!this.indexMap.TryGetValue(key, out hashtable))
            {
                hashtable = new System.Collections.Hashtable();
                this.indexMap[key] = hashtable;
                this.BuildIndex(key, hashtable);
            }
            return hashtable;
        }

        private void BuildIndex(string key, System.Collections.Hashtable index)
        {
            this.hasLongValue = false;
            foreach (IHashMap current in this)
            {
                object obj;
                if (current.TryGetValue(key, out obj))
                {
                    if (!this.hasLongValue && obj != null && obj.GetType() == typeof(long))
                    {
                        this.hasLongValue = true;
                    }
                    index[obj] = current;
                }
            }
        }

        public System.Collections.Generic.IList<T> ToEntitys<T>() where T : class, new()
        {
            System.Collections.Generic.IList<T> list = new System.Collections.Generic.List<T>();
            foreach (IHashMap current in this)
            {
                list.Add(current.ToEntity<T>());
            }
            return list;
        }


        public DataTable HashMapListToDataTable(HashMapList list)
        {
            if (list == null || list.Count < 1)
            {

                return null;
            }
            DataTable dt = new DataTable();
            foreach (var key in list[0].Keys)
            {
                String key0 = key.ToString();
                DataColumn column = new DataColumn(key0);
                dt.Columns.Add(column);
            }

            foreach (var map in list)
            {
                DataRow row = dt.NewRow();
                foreach (var key in map)
                {
                    foreach (var key1 in map.Keys)
                    {
                        String key0 = key1.ToString();

                        row[key0] = map[key0];
                    }
                }

                dt.Rows.Add(row);

            }


            return dt;
        }
    }
}
