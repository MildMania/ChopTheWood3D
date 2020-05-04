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

    public override void ResetNode()
    {
        ChildPhaseNodes.ForEach(p => p.ResetNode());
    }
}
