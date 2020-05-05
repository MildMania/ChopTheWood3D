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
        GhostCutPhase ghostCutPhase = new GhostCutPhase(4);

        return new LevelPhase(0,
            new MainMenuPhase(1),
            ghostCutPhase,
            new ChopperCutPhase(5, 1.5f),
            new LevelEndPhase(3,
                new PhaseGotoNode(6, ghostCutPhase),
                new LevelPostEndPhase(7,
                    new PhaseSerialComposition(8,
                        new LevelWinPhase(10, 1),
                        new LevelWinPostPhase(11)),
                    new PhaseSerialComposition(9,
                        new LevelLosePhase(12, 1.5f),
                        new LevelLosePostPhase(13)))));
    }
}
