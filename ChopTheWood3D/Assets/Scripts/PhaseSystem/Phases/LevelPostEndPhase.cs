using System;

public class LevelPostEndPhase : PhaseConditionalNode
{
    public LevelPostEndPhase(int id, params PhaseBaseNode[] childNodeArr)
        : base(id, childNodeArr)
    {
    }

    protected override void CheckConditions(Action<int> callback)
    {
        int callbackNodeID = 9;

        //if (LevelEndController.Instance.DidSucceed)
        //    callbackNodeID = 8;

        callback?.Invoke(callbackNodeID);
    }
}
