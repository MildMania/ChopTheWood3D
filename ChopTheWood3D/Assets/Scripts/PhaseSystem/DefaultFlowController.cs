public class DefaultFlowController : PhaseFlowController
{
    public DefaultFlowController(int levelID)
        : base()
    {
        LevelPhase levelPhase = (LevelPhase)TreeRootNode;

        levelPhase.LevelID = levelID;
    }

    protected override PhaseBaseNode CreateRootNode()
    {
        return new LevelPhase(0,
            new MainMenuPhase(1),
            new LogThrowPhase(11),
            new GhoustCutPhase(10),
            new ChopperCutPhase(2),
            new LevelEndPhase(3,
                new PhaseSerialComposition(4,
                    new LevelWinPhase(6, 1),
                    new LevelWinPostPhase(7)),
                new PhaseSerialComposition(5,
                    new LevelLosePhase(8, 1.5f),
                    new LevelLosePostPhase(9))));
    }
}
