public class PhaseGotoNode : PhaseBaseNode
{
    private PhaseBaseNode _gotoNode;

    public PhaseGotoNode(int id, PhaseBaseNode loopNode)
        : base(id)
    {
        _gotoNode = loopNode;
    }

    protected sealed override void TraverseNode()
    {
        ResetLoopNode();

        _gotoNode.Traverse();
    }

    private void ResetLoopNode()
    {
        _gotoNode.ResetNode();
    }
}
