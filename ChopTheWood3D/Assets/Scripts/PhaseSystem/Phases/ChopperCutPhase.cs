using System;

public class ChopperCutPhase : PhaseActionNode
{
    public ChopperCutPhase(int id) 
        : base(id)
    {
    }

    protected override void ProcessFlow()
    {
        ChoppableController.Instance.OnNoVisibleChoppableLeft += OnNoVisibleChoppableLeft;
    }

    private void OnNoVisibleChoppableLeft()
    {
        TraverseCompleted();
    }

    protected override void TraverseCompletedCustomActions()
    {
        ChoppableController.Instance.OnNoVisibleChoppableLeft -= OnNoVisibleChoppableLeft;
    }

    public void CompleteTraverse()
    {
        TraverseCompleted();
    }
}
