using System;
using System.Collections.Generic;
using System.Linq;

public abstract class PhaseComposition : PhaseBaseNode
{
    public List<PhaseBaseNode> ChildPhaseNodes { get; private set; }
    
    public PhaseComposition(int id, params PhaseBaseNode[] childPhaseNodes)
        : base(id)
    {
        ChildPhaseNodes = childPhaseNodes.ToList();

        ChildPhaseNodes.ForEach(n => n.OnNodeReset += OnChildNodeReset);
    }

    private void OnChildNodeReset(PhaseBaseNode resetedNode)
    {
        int startIndex = ChildPhaseNodes.IndexOf(resetedNode);
        int nextIndex = startIndex + 1;

        if (nextIndex == ChildPhaseNodes.Count)
            return;

        if (ChildPhaseNodes[nextIndex].IsTraversed)
            ChildPhaseNodes[nextIndex].ResetNode();
    }

    public T GetPhaseByType<T>() where T : PhaseBaseNode
    {
        return ChildPhaseNodes.FirstOrDefault(p => p.GetType() is T) as T;
    }

    public PhaseBaseNode GetPhaseByID(int id)
    {
        return ChildPhaseNodes.FirstOrDefault(p => p.ID.Equals(id));
    }

    protected override void TraverseNode()
    {
        if (ChildPhaseNodes.Count == 0)
        {
            TraverseCompleted();

            return;
        }

        TraverseComposition();
    }

    protected abstract void TraverseComposition();

    protected bool HasAnyNotTraversedNodes()
    {
        return ChildPhaseNodes.Any(val => !val.IsTraversed);
    }

    protected sealed override void ResetNodeCustomActions()
    {
        ChildPhaseNodes.ForEach(p => p.ResetNode());

        ResetCompositionNodeCustomActions();

        base.ResetNodeCustomActions();
    }

    protected virtual void ResetCompositionNodeCustomActions()
    {

    }
}
