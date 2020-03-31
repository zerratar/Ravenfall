using System;
using Shinobytes.Ravenfall.RavenNet.Models;

namespace Shinobytes.Ravenfall.RavenNet.Modules
{
    public class ObjectHandler : EntityHandler<SceneObject>
    {
        public ObjectHandler()
            : base((a, b) => a.Id == b.Id)
        {
        }
        public override string Name => "Object Handler";
    }
}
