using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Shinobytes.Ravenfall.RavenNet
{
    /// <summary>
    ///     Interface for all items that can be returned to an object pool.
    /// </summary>
    /// <threadsafety static="true" instance="true"/>
    public interface IRecyclable
    {
        void Recycle();
    }
}
