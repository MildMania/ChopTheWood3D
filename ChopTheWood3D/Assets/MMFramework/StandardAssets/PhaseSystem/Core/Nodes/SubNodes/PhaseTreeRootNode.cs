public class PhaseTreeRootNode : PhaseBaseNode
{
    protected PhaseBaseNode _rootNode;

    public PhaseTreeRootNode(int id, PhaseBaseNode rootNode)
        : base(id)
    {
        _rootNode = rootNode;
    }

    protected override void TraverseNode()
    {
        _rootNode.OnTraverseFinished += OnRootNodeTraversed;

        _rootNode.Traverse();
    }

    void OnRootNodeTraversed(PhaseBaseNode n)
    {
        n.OnTraverseFinished -= OnRootNodeTraversed;

        TraverseCompleted();
    }
}
