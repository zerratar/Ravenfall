using Shinobytes.Ravenfall.RavenNet.Models;

public class ExamineAction : SceneObjectAction
{
    public ExamineAction()
        : base(0, "Examine")
    {
    }

    public override bool Invoke(Player player, SceneObject obj, int parameterId)
    {
        return false;
    }
}