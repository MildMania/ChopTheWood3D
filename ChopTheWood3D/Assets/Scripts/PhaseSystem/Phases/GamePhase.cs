using System;

public class ChopperCutPhase : PhaseActionNode
{
    public ChopperCutPhase(int id) 
        : base(id)
    {
    }

    protected override void ProcessFlow()
    {
        //LevelEndController.Instance.OnLevelEnded += OnLevelEnded;
    }

    private void OnLevelEnded(bool didSucceed)
    {
        TraverseCompleted();
    }

    protected override void TraverseCompletedCustomActions()
    {
        //LevelEndController.Instance.OnLevelEnded -= OnLevelEnded;

        base.TraverseCompletedCustomActions();
    }
}
