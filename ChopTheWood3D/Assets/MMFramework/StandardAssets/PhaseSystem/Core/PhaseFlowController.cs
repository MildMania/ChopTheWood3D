public abstract class PhaseFlowController
{
    public PhaseBaseNode TreeRootNode { get; private set; }

    public PhaseFlowController()
    {
        TreeRootNode = CreateRootNode();
    }

    protected abstract PhaseBaseNode CreateRootNode();

    public void StartPhases()
    {
        TreeRootNode.Traverse();
    }
}
