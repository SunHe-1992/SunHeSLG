using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RedBjorn.Utils
{
    [Serializable]
    public class SerializableDictionary<TKey, TValue> : ISerializationCallbackReceiver, IEnumerable<KeyValuePair<TKey, TValue>>
    {

        Dictionary<TKey, TValue> Dictionary = new Dictionary<TKey, TValue>();

        [SerializeField]
        List<TKey> Keys = new List<TKey>();
        [SerializeField]
        List<TValue> Values = new List<TValue>();

        public int Count
        {
            get
            {
                return Dictionary.Count;
            }
        }

        public void Clear()
        {
            Dictionary.Clear();
        }

        public void Add(TKey key, TValue val)
        {
            Dictionary.Add(key, val);
        }

        public void Remove(TKey key)
        {
            Dictionary.Remove(key);
        }

        void ISerializationCallbackReceiver.OnAfterDeserialize()
        {
            Dictionary.Clear();
            for (int i = 0; i < Keys.Count; i++)
            {
                Dictionary.Add(Keys[i], Values[i]);
            }
        }

        void ISerializationCallbackReceiver.OnBeforeSerialize()
        {
            Keys.Clear();
            Values.Clear();
            foreach (var kv in Dictionary)
            {
                Keys.Add(kv.Key);
                Values.Add(kv.Value);
            }
        }

        IEnumerator<KeyValuePair<TKey, TValue>> IEnumerable<KeyValuePair<TKey, TValue>>.GetEnumerator()
        {
            return Dictionary.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return Dictionary.GetEnumerator();
        }

        public TValue this[TKey key]
        {
            get
            {
                return Dictionary.TryGetOrDefault(key);
            }
            set
            {
                Dictionary[key] = value;
            }
        }

        public TValue TryGetOrDefault(TKey key)
        {
            return Dictionary.TryGetOrDefault(key);
        }

    }
}