using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Messaging.Modules;

namespace UnitTests.ModuleTests
{
    [TestFixture]
    public class LRUCacheTests
    {
        private LRUCache<int, string> cache;
        [Test]
        public void TestInit()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => { new LRUCache<int, int>(-1); }, "Cache capacity should be bigger than 0, provided capacity: -1");
        }

        [SetUp]
        public void SetUp()
        {
            this.cache = new LRUCache<int, string>(5);
            for (int i = 0; i < 5; i++)
            {
                // this adds 0 -> 'a'
                //           1 -> 'b'
                //           2 -> 'c' and so on
                cache.Put(i, Convert.ToString(Convert.ToChar(Convert.ToInt32('a') + i)));
            }
        }
        [TearDown]
        public void TearDown()
        {
            this.cache.Clear();
        }

        [Test]
        public void TestPut() 
        {
            int i, j;
            Assert.IsTrue(cache.IsKeyInCache(0));
            Assert.IsTrue(cache.IsKeyInCache(1));
            Assert.IsTrue(cache.IsKeyInCache(2));
            Assert.IsTrue(cache.IsKeyInCache(3));
            Assert.IsTrue(cache.IsKeyInCache(4));
            Assert.IsFalse(cache.IsKeyInCache(5));
            Assert.IsFalse(cache.IsKeyInCache(6));
            Assert.IsFalse(cache.IsKeyInCache(10));
            Assert.IsFalse(cache.IsKeyInCache(int.MaxValue));
            Assert.IsFalse(cache.IsKeyInCache(-int.MaxValue));

            // Now let's check if the least recently used value is kicked when we try to add a new value
            cache.Put(10, "new value");
            Assert.IsTrue(cache.IsKeyInCache(10));
            Assert.IsFalse(cache.IsKeyInCache(0));

            cache.Put(0, "0 is back");
            Assert.IsTrue(cache.IsKeyInCache(0));
            Assert.IsFalse(cache.IsKeyInCache(1));

            // try bigger numbers
            int size = 9000;
            var bigCache = new LRUCache<int, int>(size);
            for (i = 1; i <= size; ++i)
            {
                bigCache.Put(i, i);
            }
            for (i = size + 1; i <= 2*size; ++i)
            {
                bigCache.Put(i, i);
                Assert.IsTrue(bigCache.IsKeyInCache(i));
                Assert.IsFalse(bigCache.IsKeyInCache(i - size));
            }
        }
        [Test]
        public void TestTryToGet()
        {
            string value;
            // Check if the values are there or not
            Assert.IsTrue(cache.TryToGetValue(0, out value));
            Assert.IsTrue(cache.TryToGetValue(1, out value));
            Assert.IsTrue(cache.TryToGetValue(2, out value));
            Assert.IsTrue(cache.TryToGetValue(3, out value));
            Assert.IsTrue(cache.TryToGetValue(4, out value));
            Assert.IsFalse(cache.TryToGetValue(5, out value));

            // Check the actual values now
            cache.TryToGetValue(0, out value);
            Assert.AreEqual(value, "a");
            cache.TryToGetValue(1, out value);
            Assert.AreEqual(value, "b");
            cache.TryToGetValue(2, out value);
            Assert.AreEqual(value, "c");
            cache.TryToGetValue(3, out value);
            Assert.AreEqual(value, "d");
            cache.TryToGetValue(4, out value);
            Assert.AreEqual(value, "e");
            cache.TryToGetValue(5, out value);
            Assert.AreNotEqual(value, "f");
            
            // try to get a value that is not in the cache and check against the default value
            // default value for strings is null
            cache.TryToGetValue(int.MaxValue, out value);
            Assert.AreEqual(value, null);

            // try bigger numbers
            int i;
            int size = 1000;
            int intValue;
            var bigCache = new LRUCache<int, int>(size);
            for (i = 1; i <= size; ++i)
            {
                bigCache.Put(i, i);
            }
            for (i = 1; i <= size; ++i)
            {
                for (int j = 1; j <= size; ++j)
                {
                    bigCache.TryToGetValue(j, out intValue);
                    Assert.AreEqual(intValue, j);
                }
            }
        }
    }
}
