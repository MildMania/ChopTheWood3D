public class ChopperCutPhase : PhaseActionNode
{
    private float _delay;
    public ChopperCutPhase(int id, float delay) 
        : base(id)
    {
        _delay = delay;
    }

    protected override void ProcessFlow()
    {
    }

    protected override void TraverseCompletedCustomActions()
    {
    }

    public void CompleteTraverse()
    {
        Utilities.WaitForSeconds(CoroutineRunner.Instance, _delay, OnDelayCompleted);
    }

    private void OnDelayCompleted()
    {
        TraverseCompleted();
    }
}
