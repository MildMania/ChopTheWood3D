using System;

public class LevelEndPhase : PhaseConditionalNode
{
    public LevelEndPhase(int id, params PhaseBaseNode[] childNodeArr) 
        : base(id, childNodeArr)
    {
    }

    protected override void CheckConditions(Action<int> callback)
    {
        int callbackNodeID = 7;

        if (ThrowerControllerManager.Instance.AnyThrowPhaseLeft)
            callbackNodeID = 6;

        callback?.Invoke(callbackNodeID);
    }
}
