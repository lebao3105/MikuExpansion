using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization.Json;
using System.IO;

namespace MikuExpansion.Helpers
{
    public class Json<T>
    {
        protected static DataContractJsonSerializer serializer =
            new DataContractJsonSerializer(typeof(T));

        public T ReadContent { get; protected set; }

        public Json()
        {
            // Do nothing
        }

        public Json(string content)
        {
            Deserialize(content);
        }

        public void Deserialize(string content)
            => ReadContent = (T)serializer.ReadObject(
                new MemoryStream(Encoding.UTF8.GetBytes(content)));

        public string Serialize(T from)
        {
            var s = new MemoryStream();
            serializer.WriteObject(s, from);
            return new StreamReader(s).ReadToEnd();
        }
    }

    public class KeyedJson<K, V> : Json<IDictionary<K, V>>
    {
        public KeyedJson(string content) : base(content)
        {
        }
        
        public V this[K index]
        {
            get { return ReadContent[index]; }
            set { ReadContent[index] = value; }
        }

        public bool ContainsKey(K key) => ReadContent.ContainsKey(key);
        public IEnumerable<KeyValuePair<K, V>>
               Where(Func<KeyValuePair<K, V>, bool> predicate) => ReadContent.Where(predicate);

        public void Remove(K key) => ReadContent.Remove(key);
        public void Remove(KeyValuePair<K, V> pair) => ReadContent.Remove(pair);
        public void Remove(K key, V value) => Remove(new KeyValuePair<K, V>(key, value));
    }

    public class StringKeyedJson<V> : KeyedJson<string, V>
    {
        public StringKeyedJson(string content) : base(content)
        {
        }
    }

    public class ArrayOfJson<V> : Json<IList<V>>
    {
        public ArrayOfJson(string content) : base(content)
        {
        }

        public V this[int index]
        {
            get { return ReadContent.ElementAt(index); }
            set { ReadContent[index] = value; }
        }

        public int Count => ReadContent.Count;

        public bool Contains(V key) => ReadContent.Contains(key);
        public IEnumerable<V>
               Where(Func<V, bool> predicate) => ReadContent.Where(predicate);

        public void Remove(V key) => ReadContent.Remove(key);
        public void RemoveAt(int index) => ReadContent.RemoveAt(index);
    }
}
