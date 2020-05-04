public class CatchSuccessPhaseActionNode : PhaseActionNode
{
    public CatchSuccessPhaseActionNode(int id)
        : base(id)
    {
    }

    protected override void ProcessFlow()
    {
        throw new System.NotImplementedException();
    }
}
