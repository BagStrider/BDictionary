using System;
using System.Collections.Generic;
using UnityEngine;

namespace BagSerializer
{
    [Serializable]
    public class SerializedTypeDictionary<T> : ISerializationCallbackReceiver
    {
        [SerializeReference] private List<T> _values = new ();

        private Dictionary<Type, T> _dict = new ();

        public void OnBeforeSerialize()
        {
            _values.Clear();

            foreach (var kv in _dict)
            {
                _values.Add(kv.Value);
            }
        }

        public void OnAfterDeserialize()
        {
            _dict.Clear();
            for (int i = 0; i < _values.Count; i++)
            {
                if (_values[i] == null) continue;
               
                _dict[_values[i].GetType()] = _values[i];
            }
        }

        public bool Contains(T value) => _dict.ContainsKey(typeof(T));
        public void Add(T value)
        {
            if (value == null) throw new ArgumentNullException();
            _dict[typeof(T)] = value;
        }
        public void Remove<T>() => _dict.Remove(typeof(T));
        public bool TryGet(out T value) => _dict.TryGetValue(typeof(T), out value);
    }
}