using Shinobytes.Ravenfall.RavenNet.Models;

public class PlayerInspectAction : EntityAction
{
    public PlayerInspectAction()
        : base(4, "Player Inspect")
    {
    }

    public override bool Invoke(
        Player player,
        Entity obj,
        int parameterId)
    {
        return false;
    }
}
