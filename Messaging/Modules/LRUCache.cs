using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Messaging.Modules
{
    public class LRUCache<TKey, TValue>
    {
        /* Implementation of a LRU cache with a set capacity
         *  TKey and TValue are generics used for more flexibility of the module
         */
        // Max capacity of our cache
        private int _capacity { get; }
        private Dictionary<TKey, TValue> _cacheMap;
        // We need a data type with O(1) beggining and end operations, wanted to use a Dequeue (there isn't one in c#), so a DLL would suffice.
        // The list is used to decide which item needs to be kicked out of the cache (least recently used rule)
        // choosing to use (TKey, TValue) to uniquely identify each value
        private LinkedList<TKey> _cacheQueue;

        // The above shoudl be read only, that's why I'm making only getters
        public int Capacity { get { return _capacity; } }
        public Dictionary<TKey, TValue> CacheMap { get { return _cacheMap; } }
        public LinkedList<TKey> CacheQueue { get { return _cacheQueue; } }


        public LRUCache(int capacity)
        {
            if (capacity <= 0)
                throw new ArgumentOutOfRangeException("Cache capacity should be bigger than 0, provided capacity: " + capacity);
            this._capacity = capacity;
            _cacheMap = new Dictionary<TKey, TValue>();
            _cacheQueue = new LinkedList<TKey>();
        }

        public bool IsKeyInCache(TKey key)
        {
            return _cacheMap.ContainsKey(key);
        }

        public bool IsValueInCache(TValue value)
        {
            return _cacheMap.ContainsValue(value);
        }

        public bool TryToGetValue(TKey key, out TValue value)
        {
            // Return default TValue if the key is not in the map
            if (!_cacheMap.ContainsKey(key)){
                value = default(TValue);
                return false;
            }

            // This means we have the value with the given key in our cache
            value = _cacheMap[key];
            // Put the value in front of the queue
            _cacheQueue.Remove(key);
            _cacheQueue.AddFirst(key);
            return true;
        }

        public void Put(TKey key, TValue value)
        {
            // Check if it's already in the map
            if (_cacheMap.ContainsKey(key))
            {
                // Just update it as recently used if it is in the map (aka add it in front of the queue)
                _cacheMap[key] = value;
                _cacheQueue.Remove(key);
                _cacheQueue.AddFirst(key);
                return;
            }
            // Since we didn't find it in the cache we have to actually add it
            // Check if we have space or we must kick an item out
            if (_cacheMap.Count >= _capacity)
            {
                // Kick the last item both from the queue and from the map
                var lastItem = _cacheQueue.Last();
                _cacheQueue.RemoveLast();
                _cacheMap.Remove(lastItem);
            }

            // Add them to the map and to the queue
            _cacheMap[key] = value;
            _cacheQueue.AddFirst(key);
        }

        public void Clear()
        {
            _cacheMap.Clear();
            _cacheQueue.Clear();
        }
    }
}
