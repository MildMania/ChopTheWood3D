using System;

public abstract class PhaseConditionalNode : PhaseComposition
{
    public PhaseConditionalNode(int id, params PhaseBaseNode[] childNodeArr)
        : base(id, childNodeArr)
    {
        ChildPhaseNodes.ForEach(n => n.OnTraverseFinished += OnChildNodeTraverseFinished);
    }

    protected sealed override void TraverseComposition()
    {
        CheckConditions(OnConditionReturn);
    }

    private void OnConditionReturn(int nextPhaseID)
    {
        PhaseBaseNode resultNode = GetPhaseByID(nextPhaseID);

        resultNode.Traverse();
    }

    private void OnChildNodeTraverseFinished(PhaseBaseNode n)
    {
        TraverseCompleted();
    }

    /// <summary>
    /// A method to allow registering events or invoking callback directly.
    /// After conditional check, INVOKE callback to continue to the next phase with its ID.
    /// </summary>
    /// <param name="callback">Condition result callback with the next phase's ID.</param>
    protected abstract void CheckConditions(Action<int> callback);
}
