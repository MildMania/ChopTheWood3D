public class MainMenuVM : VMBase
{
    private MainMenuPhase _mainMenuPhase;

    public MainMenuVM()
    {
        PhaseBaseNode.OnTraverseStarted_Static += OnPhaseTraverseStarted;
        PhaseBaseNode.OnTraverseFinished_Static += OnPhaseTraverseFinished;

        HUDVM.OnPausePressed += OnPausePressed;
    }

    protected override void DisposeCustomActions()
    {
        PhaseBaseNode.OnTraverseStarted_Static -= OnPhaseTraverseStarted;
        PhaseBaseNode.OnTraverseFinished_Static -= OnPhaseTraverseFinished;

        HUDVM.OnPausePressed -= OnPausePressed;
    }

    private void OnPhaseTraverseStarted(PhaseBaseNode phase)
    {
        if (!(phase is MainMenuPhase))
            return;

        _mainMenuPhase = (MainMenuPhase)phase;

        if (UIMenuManager.Instance.IsMainMenuFirstOpenOccured)
            _mainMenuPhase.CompleteTraverse();
        else
            ActivateUI();
    }

    private void OnPhaseTraverseFinished(PhaseBaseNode phase)
    {
        if (!(phase is MainMenuPhase))
            return;

        _mainMenuPhase = null;

        DeactivateUI();
    }

    private void OnPausePressed()
    {
        ActivateUI();
    }

    private void ActivateUI()
    {
        MainMenuUI uiMenu = UIMenuManager.Instance.GetUIMenu<MainMenuUI>();

        UIMenuManager.Instance.OpenUIMenu(uiMenu);
    }

    private void DeactivateUI()
    {
        MainMenuUI uiMenu = UIMenuManager.Instance.GetOpenMenu<MainMenuUI>();

        if (uiMenu is default(MainMenuUI))
            return;

        UIMenuManager.Instance.CloseMenu(uiMenu);
    }

    public override void FinishListeningEvents()
    {
    }

    public override void StartListeningEvents()
    {
    }

    public void PlayPressed()
    {
        if (UIMenuManager.Instance.IsMainMenuFirstOpenOccured)
            DeactivateUI();
        else
        {
            UIMenuManager.Instance.IsMainMenuFirstOpenOccured = true;

            _mainMenuPhase.CompleteTraverse();
        }
    }

    public IPLDBase GetTotalCoinPLD()
    {
        //return new TotalCoinDrawerPLD(UserMoneyManager.Instance.UserMoneyAmount.ToString());
        return new TotalCoinDrawerPLD("0");
    }

    public IPLDBase GetLevelIDPLD()
    {
        //int levelID = ((LevelPhase)GameManager.Instance.PhaseFlowController.TreeRootNode).LevelID;

        return new LevelIDDrawerPLD("0");
    }
}
