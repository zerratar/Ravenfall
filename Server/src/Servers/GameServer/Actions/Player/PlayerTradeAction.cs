using Shinobytes.Ravenfall.RavenNet.Models;

public class PlayerTradeAction : EntityAction
{
    public PlayerTradeAction()
        : base(5, "Player Trade")
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
