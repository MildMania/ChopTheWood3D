public class DefaultFlowManager : PhaseFlowManager
{
    protected override PhaseFlowController CreatePhase()
    {
        return new DefaultFlowController();
    }
}
