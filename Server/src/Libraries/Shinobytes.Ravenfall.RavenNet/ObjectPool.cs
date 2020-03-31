using Shinobytes.Ravenfall.RavenNet.Extensions;
using System;
using System.Collections.Concurrent;
using System.Threading;

namespace Shinobytes.Ravenfall.RavenNet
{
    /// <summary>
    ///     A fairly simple object pool for items that will be created a lot.
    /// </summary>
    /// <typeparam name="T">The type that is pooled.</typeparam>
    /// <threadsafety static="true" instance="true"/>
    public sealed class ObjectPool<T> where T : IRecyclable
    {
        private int numberCreated;
        public int NumberCreated { get { return numberCreated; } }

        // Available objects
        private readonly ConcurrentBag<T> pool = new ConcurrentBag<T>();

        // Unavailable objects
        private readonly ConcurrentDictionary<T, bool> inuse = new ConcurrentDictionary<T, bool>();

        /// <summary>
        ///     The generator for creating new objects.
        /// </summary>
        /// <returns></returns>
        private readonly Func<T> objectFactory;
        private readonly int max;
        /// <summary>
        ///     Internal constructor for our ObjectPool.
        /// </summary>
        internal ObjectPool(Func<T> objectFactory, int max = int.MaxValue)
        {
            this.objectFactory = objectFactory;
            this.max = max;
        }

        /// <summary>
        ///     Returns a pooled object of type T, if none are available another is created.
        /// </summary>
        /// <returns>An instance of T.</returns>
        internal T GetObject()
        {
            T item;
            while (!pool.TryTake(out item))
            {
                if (Volatile.Read(ref numberCreated) < max)
                {
                    Interlocked.Increment(ref numberCreated);
                    item = objectFactory.Invoke();
                    break;
                }
            }

            if (!inuse.TryAdd(item, true))
            {
                throw new Exception("Duplicate pull " + typeof(T).Name);
            }

            return item;
        }

        /// <summary>
        ///     Returns an object to the pool.
        /// </summary>
        /// <param name="item">The item to return.</param>
        internal void PutObject(T item)
        {
            if (inuse.TryRemove(item, out bool b))
            {
                pool.Add(item);
            }     
        }
    }
}
