public class ChopperCutPhase : PhaseActionNode
{
    public ChopperCutPhase(int id) 
        : base(id)
    {
    }

    protected override void ProcessFlow()
    {
    }

    protected override void TraverseCompletedCustomActions()
    {
    }

    public void CompleteTraverse()
    {
        TraverseCompleted();
    }
}
