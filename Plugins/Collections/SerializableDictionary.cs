// @FengYan 2024 Copyright Reserved.

using System;
using System.Collections.Generic;
using UnityEngine;

namespace CsLibs.Collections
{
    /// <summary>
    /// 可以配合Unity Inspector使用的可序列化字典
    /// </summary>
    /// <typeparam name="TKey">键</typeparam>
    /// <typeparam name="TValue">值</typeparam>
    [Serializable]
    public class SerializableDictionary<TKey, TValue> : Dictionary<TKey, TValue>, ISerializationCallbackReceiver
    {
        [Serializable]
        public struct KeyValue
        {
            public TKey key;
            public TValue value;
            public static implicit operator KeyValue(KeyValuePair<TKey, TValue> pair)
            {
                return new KeyValue()
                {
                    key = pair.Key,
                    value = pair.Value
                };
            }
        }
        
        public List<KeyValue> keyValues = new();
        public void OnBeforeSerialize()
        {

            if (keyValues.Count > Count)
                AddNewValue();
        
            else if (keyValues.Count < Count)
                UpdateSerializedValues();

        }

        void UpdateSerializedValues()
        {
            keyValues.Clear();
            foreach(var pair in this)
                keyValues.Add(pair);
        }

        void AddNewValue()
        {
            var current = keyValues[^1];
            TryAdd(current.key, current.value);
        }
        public void OnAfterDeserialize()
        {
            Clear();

            for (var i = 0; keyValues != null && i < keyValues.Count; i++)
            {
                var current = keyValues[i];
                TryAdd(current.key, current.value);
            }
        }
    }
}