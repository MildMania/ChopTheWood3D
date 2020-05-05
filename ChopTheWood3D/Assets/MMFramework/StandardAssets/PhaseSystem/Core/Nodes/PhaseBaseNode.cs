using System;

public abstract class PhaseBaseNode
{
    #region Events
    public static Action<PhaseBaseNode> OnTraverseStarted_Static;
    public static Action<PhaseBaseNode> OnTraverseFinished_Static;
    public Action<PhaseBaseNode> OnTraverseStarted;
    public Action<PhaseBaseNode> OnTraverseFinished;

    public Action<PhaseBaseNode> OnNodeReset;
    #endregion

    public PhaseBaseNode(int id)
    {
        ID = id;
    }

    public int ID { get; private set; } = -1;
    public bool IsTraversed { get; private set; }

    public void Traverse()
    {
        //UnityEngine.Debug.Log("Traversing: " + GetType().ToString());

        OnTraverseStarted_Static?.Invoke(this);
        OnTraverseStarted?.Invoke(this);

        if (IsTraversed)
        {
            TraverseCompleted();

            return;
        }

        TraverseNode();
    }

    protected abstract void TraverseNode();

    protected void TraverseCompleted()
    {
        IsTraversed = true;

        TraverseCompletedCustomActions();

        OnTraverseFinished_Static?.Invoke(this);
        OnTraverseFinished?.Invoke(this);
    }

    protected virtual void TraverseCompletedCustomActions()
    {
    }

    public void ResetNode()
    {
        IsTraversed = false;

        ResetNodeCustomActions();

        OnNodeReset?.Invoke(this);
    }

    protected virtual void ResetNodeCustomActions()
    {
    }
}
