public class MainMenuPhase : PhaseActionNode
{
    public MainMenuPhase(int id) 
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
