public class PhaseTreeRootNode : PhaseBaseNode
{
    protected PhaseBaseNode _rootNode;

    public PhaseTreeRootNode(int id, PhaseBaseNode rootNode)
        : base(id)
    {
        _rootNode = rootNode;

        _rootNode.OnTraverseFinished += OnRootNodeTraversed;
    }

    protected override void TraverseNode()
    {
        _rootNode.Traverse();
    }

    void OnRootNodeTraversed(PhaseBaseNode n)
    {
        TraverseCompleted();
    }
}
