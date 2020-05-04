using System;

public class HuntCatchPhaseCondNode : PhaseConditionalNode
{
    public HuntCatchPhaseCondNode(int id, params PhaseBaseNode[] nodes)
        : base(id, nodes)
    {
    }

    protected override void CheckConditions(Action<int> callback)
    {
        //callback?.Invoke(4);
        // OR
        //callback?.Invoke(5);
    }
}
