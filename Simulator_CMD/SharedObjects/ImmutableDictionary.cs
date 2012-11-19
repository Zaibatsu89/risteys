using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharedObject
{
    public class ImmutableDictionary<TKey, TValue> : SortedDictionary<TKey, TValue>
    {
        public ImmutableDictionary(IEnumerable<KeyValuePair<TKey, TValue>> keysValues)
        {

            foreach (var keyValue in keysValues)
                base.Add(keyValue.Key, keyValue.Value);
        }

        public new void Add(TKey key, TValue value)
        {
            throw new InvalidOperationException("Cannot modify contents of immutable dictionary.");
        }

        public new void Clear()
        {
            throw new InvalidOperationException("Cannot modify contents of immutable dictionary.");
        }

        public new void Remove(TKey key)
        {
            throw new InvalidOperationException("Cannot modify contents of immutable dictionary.");
        }

        public new TValue this[TKey key]
        {
            get { return base[key]; }
            set
            {
                throw new InvalidOperationException("Cannot modify contents of immutable dictionary.");
            }
        }
    }
}
