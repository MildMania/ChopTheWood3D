using System;

public class HUDVM : VMBase
{
    #region Events
    public Action<IPLDBase> OnProgressUpdating;
    public static Action OnPausePressed;
    #endregion

    public HUDVM()
    {
        PhaseBaseNode.OnTraverseStarted_Static += OnPhaseTraverseStarted;
    }

    protected override void DisposeCustomActions()
    {
        PhaseBaseNode.OnTraverseStarted_Static -= OnPhaseTraverseStarted;
    }

    private void OnPhaseTraverseStarted(PhaseBaseNode phaseBaseNode)
    {
        //if (phaseBaseNode is GamePhase)
        //    ActivateUI();
        //else
        //    DeactivateUI();
    }

    private void ActivateUI()
    {
        HUDUI uiMenu = UIMenuManager.Instance.GetUIMenu<HUDUI>();

        UIMenuManager.Instance.OpenUIMenu(uiMenu);
    }

    private void DeactivateUI()
    {
        HUDUI uiMenu = UIMenuManager.Instance.GetOpenMenu<HUDUI>();

        if (uiMenu is default(HUDUI))
            return;

        UIMenuManager.Instance.CloseMenu(uiMenu);
    }

    public override void FinishListeningEvents()
    {
        //LevelProgressController progressController = UnityEngine.Object.FindObjectOfType<LevelProgressController>();

        //if (progressController == null)
        //    return;

        //progressController.OnCompletionPercentageUpdated -= OnProgressUpdated;
    }

    public override void StartListeningEvents()
    {
        //LevelProgressController progressController = UnityEngine.Object.FindObjectOfType<LevelProgressController>();
        
        //progressController.OnCompletionPercentageUpdated += OnProgressUpdated;
    }

    private void OnProgressUpdated(float perc)
    {
        OnProgressUpdating?.Invoke(new LevelProgressDrawerPLD(perc));
    }

    public void PausePressed()
    {
        OnPausePressed?.Invoke();
    }

    public void RetryPressed()
    {
        GameManager.Instance.SceneManager.LoadCurScene();
    }

    public IPLDBase GetLevelIDPLD()
    {
        //int levelID = ((LevelPhase)GameManager.Instance.PhaseFlowController.TreeRootNode).LevelID;

        return new LevelIDDrawerPLD("0");
    }

    public IPLDBase GetSpeedPLD()
    {
        //float timeScale = LevelTimeScaleController.Instance.LevelTimeScaleCoef;

        return new SpeedIndicatorDrawerPLD("0");
    }
}
