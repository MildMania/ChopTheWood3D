public class LogThrowPhase : PhaseActionNode
{
    public LogThrowPhase(int id)
        : base(id)
    {
    }

    protected override void ProcessFlow()
    {
        TraverseCompleted();
    }
}
