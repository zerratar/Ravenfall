using Shinobytes.Ravenfall.RavenNet.Models;

public class ExamineAction : EntityAction
{
    public ExamineAction()
        : base(0, "Examine")
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
