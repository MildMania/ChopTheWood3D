using System;

public abstract class PhaseBaseNode
{
    #region Events
    public static Action<PhaseBaseNode> OnTraverseStarted_Static;
    public static Action<PhaseBaseNode> OnTraverseFinished_Static;
    public Action<PhaseBaseNode> OnTraverseStarted;
    public Action<PhaseBaseNode> OnTraverseFinished;
    #endregion

    public PhaseBaseNode(int id)
    {
        ID = id;
    }

    public int ID { get; private set; } = -1;
    public bool IsTraversed { get; private set; }

    public void Traverse()
    {
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

    public void TraverseCompleted()
    {
        IsTraversed = true;

        TraverseCompletedCustomActions();

        OnTraverseFinished_Static?.Invoke(this);
        OnTraverseFinished?.Invoke(this);
    }

    protected virtual void TraverseCompletedCustomActions()
    {
    }

    public virtual void ResetNode()
    {
    }

    public virtual void ResetNodeCustomActions()
    {
    }
}
