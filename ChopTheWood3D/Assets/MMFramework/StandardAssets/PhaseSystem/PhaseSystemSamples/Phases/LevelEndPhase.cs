using System;

public class LevelEndPhaseCondNode : PhaseConditionalNode
{
    public LevelEndPhaseCondNode(int id, params PhaseBaseNode[] nodes)
        : base(id, nodes)
    {
    }

    protected override void CheckConditions(Action<int> callback)
    {
        //callback?.Invoke(7);
        // OR
        //callback?.Invoke(8);
        // OR
        //callback?.Invoke(9);
    }
}
