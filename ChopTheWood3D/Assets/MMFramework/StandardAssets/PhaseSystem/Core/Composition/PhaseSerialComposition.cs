public class PhaseSerialComposition : PhaseComposition
{
    public PhaseSerialComposition(int id, params PhaseBaseNode[] childNodeArr)
        : base(id, childNodeArr)
    {
        ChildPhaseNodes.ForEach(n => n.OnTraverseFinished += OnChildNodeTraverseFinished);
    }

    protected override void TraverseComposition()
    {
        TraverseChildNode(ChildPhaseNodes[0]);
    }

    void TraverseChildNode(PhaseBaseNode node)
    {
        node.Traverse();
    }

    void OnChildNodeTraverseFinished(PhaseBaseNode n)
    {
        if (HasAnyNotTraversedNodes())
        {
            int traversedNodeIndex = ChildPhaseNodes.IndexOf(n);

            PhaseBaseNode nextNode = ChildPhaseNodes[traversedNodeIndex + 1];

            TraverseChildNode(nextNode);

            return;
        }

        TraverseCompleted();
    }
}