using System.Linq;

public class NpcInteraction
{
    public NetworkPlayer Interactor { get; set; }
    public NetworkNpc NetworkNpc { get; set; }
    public ServerNpc NpcData => NetworkNpc.Data;
    public int ServerId => NetworkNpc.ServerId;
    public UnityEngine.Vector3 Position => NetworkNpc.transform.position;
    public int ActionId { get; set; }

    internal static NpcInteraction Create(NetworkPlayer interactor, NetworkNpc npcObj, int actionId = 0)
    {
        var data = npcObj.Data;
        actionId = (actionId == -1 ? data.Actions[0] : data.Actions.FirstOrDefault(x => x.Id == actionId)).Id;
        return new NpcInteraction()
        {
            Interactor = interactor,
            NetworkNpc = npcObj,
            ActionId = actionId
        };
    }

    public bool TryGetAction(out float distance, out ServerAction action, out float interactionRange)
    {
        var data = NpcData;
        distance = UnityEngine.Vector3.Distance(Position, Interactor.transform.position);
        action = ActionId == -1 ? data.Actions[0] : data.Actions.FirstOrDefault(x => x.Id == ActionId);
        interactionRange = action.Range > 0 ? action.Range : data.InteractionRange > 0 ? data.InteractionRange : Interactor.ObjectInteractionRange;
        return action;
    }
}
