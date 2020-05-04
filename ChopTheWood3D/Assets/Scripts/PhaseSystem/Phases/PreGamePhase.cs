public class GhoustCutPhase : PhaseActionNode
{
    public GhoustCutPhase(int id)
        : base(id)
    {
    }

    protected override void ProcessFlow()
    {
        //MagnetObjectDropController.OnAllMagnetObjectsDropped += OnAllMagnetObjectsDropped;
    }

    private void OnAllMagnetObjectsDropped()
    {
        TraverseCompleted();
    }

    protected override void TraverseCompletedCustomActions()
    {
        //MagnetObjectDropController.OnAllMagnetObjectsDropped -= OnAllMagnetObjectsDropped;

        base.TraverseCompletedCustomActions();
    }
}
