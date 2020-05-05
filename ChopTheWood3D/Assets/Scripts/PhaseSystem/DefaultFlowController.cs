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
        GhostCutPhase ghostCutPhase = new GhostCutPhase(2);

        return new LevelPhase(0,
            new MainMenuPhase(1),
            ghostCutPhase,
            new ChopperCutPhase(3, 2.0f),
            new LevelEndPhase(4,
                new PhaseGotoNode(5, ghostCutPhase),
                new LevelPostEndPhase(6,
                    new PhaseSerialComposition(7,
                        new LevelWinPhase(9, 1),
                        new LevelWinPostPhase(10)),
                    new PhaseSerialComposition(8,
                        new LevelLosePhase(11, 1.5f),
                        new LevelLosePostPhase(12)))));
    }
}
