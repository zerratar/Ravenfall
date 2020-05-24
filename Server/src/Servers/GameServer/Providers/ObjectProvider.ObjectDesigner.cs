using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading;

namespace RavenfallServer.Providers
{
    public partial class ObjectProvider
    {
        private ConcurrentDictionary<string, Type> loadedActionTypes;
        
        protected void AddGameObjects()
        {
            var objects = this.objectRepository.AllObjects();

            foreach (var obj in objects)
            {
                obj.Id = Interlocked.Increment(ref index);
                entities.Add(obj);
            }

            //entities.Add(Chair.Create(ref index, new Vector3(-2.96f, 7.6f, 9.83f)));

            //entities.Add(RockObject.Create(ref index, new Vector3(-11.57f, 7.6f, -12.39f)));
            //entities.Add(RockObject.Create(ref index, new Vector3(-15.43f, 7.6f, -8.64f)));

            //entities.Add(FishingSpotObject.Create(ref index, new Vector3(-4.8f, 7.6f, -10.3f)));
        }

        private void AddObjectActions()
        {
            var actions = this.objectRepository.GetActions();
            foreach (var action in actions)
            {
                Type[] actionTypes = ResolveActionTypes(action.ActionTypes);
                RegisterObjectActions(action.ObjectId, actionTypes);
            }
        }

        private void AddObjectDrops()
        {
            var drops = this.objectRepository.GetItemDrops();
            foreach (var drop in drops)
            {
                RegisterObjectItemDrop(drop.ObjectId, drop.Drops);
            }
        }

        private Type[] ResolveActionTypes(string[] actionTypes)
        {
            if (loadedActionTypes == null)
            {
                var assemblies = AppDomain.CurrentDomain.GetAssemblies();
                loadedActionTypes = new ConcurrentDictionary<string, Type>(
                    assemblies
                    .SelectMany(x => x.GetTypes().Where(x => typeof(SceneObjectAction).IsAssignableFrom(x)))
                    .ToDictionary(x => x.FullName, y => y));
            }

            return actionTypes
                .Select(x => loadedActionTypes.TryGetValue(x, out var type) ? type : null)
                .ToArray();
        }
    }
}
