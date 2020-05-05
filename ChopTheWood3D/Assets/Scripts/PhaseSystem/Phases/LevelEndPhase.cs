using System;

public class LevelEndPhase : PhaseConditionalNode
{
    public LevelEndPhase(int id, params PhaseBaseNode[] childNodeArr) 
        : base(id, childNodeArr)
    {
    }

    protected override void CheckConditions(Action<int> callback)
    {
        int callbackNodeID = 6;

        if (ThrowerControllerManager.Instance.AnyThrowPhaseLeft)
            callbackNodeID = 5;

        callback?.Invoke(callbackNodeID);
    }
}
