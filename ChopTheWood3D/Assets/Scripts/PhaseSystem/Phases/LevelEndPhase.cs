using System;

public class LevelEndPhase : PhaseConditionalNode
{
    public LevelEndPhase(int id, params PhaseBaseNode[] childNodeArr) 
        : base(id, childNodeArr)
    {
    }

    protected override void CheckConditions(Action<int> callback)
    {
        int callbackNodeID = 5;

        //if (LevelEndController.Instance.DidSucceed)
        //    callbackNodeID = 4;

        callback?.Invoke(callbackNodeID);
    }
}
