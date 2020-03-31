using System;
using System.Collections;
using System.Collections.Generic;

namespace Shinobytes.Ravenfall.RavenNet.Extensions
{
    /// <summary>Represents a circular enumerator.</summary>
    /// <typeparam name="T">The type of the elements of the enumerator.</typeparam>
    public class CircularEnumerator<T> : IEnumerator<T>, IDisposable, IEnumerator
    {
        private readonly IEnumerable<T> source;
        private IEnumerator<T> enumerator;
        private bool disposed;

        /// <summary>
        ///     Initializes a new instance of the <see cref="T:Solidicon.Solidity.Core.CircularEnumerator`1" /> class.
        /// </summary>
        /// <param name="source">The source enumerable.</param>
        /// <exception cref="T:System.ArgumentNullException">thrown if source is null.</exception>
        public CircularEnumerator(IEnumerable<T> source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            this.source = source;
            this.enumerator = source.GetEnumerator();
        }

        public T Current
        {
            get
            {
                return this.enumerator.Current;
            }
        }

        /// <summary>
        ///     Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// <filterpriority>2</filterpriority>
        public void Dispose()
        {
            if (this.disposed)
                return;
            this.enumerator.Dispose();
            this.disposed = true;
        }

        object IEnumerator.Current
        {
            get
            {
                return (object)this.Current;
            }
        }

        /// <summary>
        ///     Advances the enumerator to the next element of the collection.
        /// </summary>
        /// <returns>
        ///     true if the enumerator was successfully advanced to the next element; false if the enumerator has passed the end of
        ///     the collection.
        /// </returns>
        /// <exception cref="T:System.InvalidOperationException">The collection was modified after the enumerator was created. </exception>
        /// <filterpriority>2</filterpriority>
        public bool MoveNext()
        {
            if (this.disposed)
                throw new ObjectDisposedException(this.GetType().Name);
            if (this.enumerator.MoveNext())
                return true;
            this.enumerator.Dispose();
            this.enumerator = this.source.GetEnumerator();
            return this.enumerator.MoveNext();
        }

        /// <summary>
        ///     Sets the enumerator to its initial position, which is before the first element in the collection.
        /// </summary>
        /// <exception cref="T:System.InvalidOperationException">The collection was modified after the enumerator was created. </exception>
        /// <filterpriority>2</filterpriority>
        public void Reset()
        {
            this.enumerator.Dispose();
            this.enumerator = this.source.GetEnumerator();
        }
    }
}
