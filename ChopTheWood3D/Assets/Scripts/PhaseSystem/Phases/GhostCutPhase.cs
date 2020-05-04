public class GhostCutPhase : PhaseActionNode
{
    public GhostCutPhase(int id)
        : base(id)
    {
    }

    protected override void ProcessFlow()
    {
    }

    public void CompleteTraverse()
    {
        TraverseCompleted();
    }
}
