using System;

public class ChopperCutPhase : PhaseActionNode
{
    public ChopperCutPhase(int id) 
        : base(id)
    {
    }

    protected override void ProcessFlow()
    {
    }

    public void CompleteTraverse()
    {
        TraverseCompleted();
    }
}
