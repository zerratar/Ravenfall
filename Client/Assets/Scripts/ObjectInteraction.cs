using System.Linq;

public class ObjectInteraction
{
    public NetworkPlayer Interactor { get; set; }
    public NetworkObject NetworkObject { get; set; }
    public StaticObject StaticObject { get; set; }
    public ServerObject ObjectData => NetworkObject ? NetworkObject.Data : StaticObject.ObjectData;
    public int ServerId => NetworkObject ? NetworkObject.ServerId : StaticObject.Instance + 1;
    public UnityEngine.Vector3 Position => NetworkObject ? NetworkObject.transform.position : StaticObject.Position;
    public int ActionId { get; set; }

    internal static ObjectInteraction Create(NetworkPlayer interactor, NetworkObject worldObject, int actionId = 0)
    {
        return new ObjectInteraction()
        {
            Interactor = interactor,
            NetworkObject = worldObject,
            ActionId = actionId
        };
    }

    internal static ObjectInteraction Create(NetworkPlayer interactor, StaticObject staticObject, int actionId = 0)
    {
        return new ObjectInteraction()
        {
            Interactor = interactor,
            StaticObject = staticObject,
            ActionId = actionId
        };
    }

    public bool TryGetAction(out float distance, out ServerAction action, out float interactionRange)
    {
        var data = ObjectData;
        distance = UnityEngine.Vector3.Distance(Position, Interactor.transform.position);
        action = ActionId == -1 ? data.Actions[0] : data.Actions.FirstOrDefault(x => x.Id == ActionId);
        interactionRange = action.Range > 0 ? action.Range : data.InteractionRange > 0 ? data.InteractionRange : Interactor.ObjectInteractionRange;
        return action;
    }

}
