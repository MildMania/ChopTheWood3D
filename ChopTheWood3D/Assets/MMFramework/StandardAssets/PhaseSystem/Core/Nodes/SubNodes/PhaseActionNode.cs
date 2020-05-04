public abstract class PhaseActionNode : PhaseBaseNode
{
    public PhaseActionNode(int id)
        : base(id)
    {
    }

    protected sealed override void TraverseNode()
    {
        ProcessFlow();
    }

    protected abstract void ProcessFlow();
}