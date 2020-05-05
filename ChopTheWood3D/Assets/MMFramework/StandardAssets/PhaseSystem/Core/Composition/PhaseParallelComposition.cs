public class PhaseParallelComposition : PhaseComposition
{
    public PhaseParallelComposition(int id, params PhaseBaseNode[] childNodeArr)
    : base(id, childNodeArr)
    {
        ChildPhaseNodes.ForEach(n => n.OnTraverseFinished += OnChildNodeTraverseFinished);
    }

    protected override void TraverseComposition()
    {
        for (int i = 0; i < ChildPhaseNodes.Count; i++)
        {
            TraverseChildNode(ChildPhaseNodes[i]);
        }
    }

    void TraverseChildNode(PhaseBaseNode node)
    {
        node.Traverse();
    }

    void OnChildNodeTraverseFinished(PhaseBaseNode node)
    {
        if (HasAnyNotTraversedNodes())
            return;

        TraverseCompleted();
    }
}