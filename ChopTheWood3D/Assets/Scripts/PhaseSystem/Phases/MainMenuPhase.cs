public class MainMenuPhase : PhaseActionNode
{
    public MainMenuPhase(int id) 
        : base(id)
    {
    }

    protected override void ProcessFlow()
    {
        TraverseCompleted();
    }

    public void CompleteTraverse()
    {
        TraverseCompleted();
    }
}
