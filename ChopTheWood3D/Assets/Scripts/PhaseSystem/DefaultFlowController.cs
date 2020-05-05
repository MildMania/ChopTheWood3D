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
        PhaseSerialComposition cutSerialPhase = new PhaseSerialComposition(2,
            new GhostCutPhase(4),
            new ChopperCutPhase(5));

        return new LevelPhase(0,
            new MainMenuPhase(1),
            cutSerialPhase,
            new LevelEndPhase(3,
                2.0f,
                new PhaseGotoNode(6, cutSerialPhase),
                new LevelPostEndPhase(7,
                    new PhaseSerialComposition(8,
                        new LevelWinPhase(10, 1),
                        new LevelWinPostPhase(11)),
                    new PhaseSerialComposition(9,
                        new LevelLosePhase(12, 1.5f),
                        new LevelLosePostPhase(13)))));
    }
}
