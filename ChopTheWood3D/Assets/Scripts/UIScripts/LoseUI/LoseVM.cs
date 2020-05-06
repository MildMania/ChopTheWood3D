using UnityEngine;

public class LoseVM : VMBase
{
    private int _playerMoneyAtStart = 0;

    public LoseVM()
    {
        PhaseBaseNode.OnTraverseStarted_Static += OnPhaseTraverseStarted;
        PhaseBaseNode.OnTraverseFinished_Static += OnPhaseTraverseFinished;
    }

    protected override void DisposeCustomActions()
    {
        PhaseBaseNode.OnTraverseStarted_Static -= OnPhaseTraverseStarted;
        PhaseBaseNode.OnTraverseFinished_Static -= OnPhaseTraverseFinished;
    }

    private void OnPhaseTraverseStarted(PhaseBaseNode phase)
    {
        if (!(phase is LevelLosePostPhase))
            return;

        ActivateUI();

        //if (phase is GhostCutPhase)
        //    _playerMoneyAtStart = UserMoneyManager.Instance.UserMoneyAmount;
    }

    private void OnPhaseTraverseFinished(PhaseBaseNode phase)
    {
        if (!(phase is LevelLosePostPhase))
            return;

        DeactivateUI();
    }

    private void ActivateUI()
    {
        LoseUI uiMenu = UIMenuManager.Instance.GetUIMenu<LoseUI>();

        UIMenuManager.Instance.OpenUIMenu(uiMenu);
    }

    private void DeactivateUI()
    {
        LoseUI uiMenu = UIMenuManager.Instance.GetOpenMenu<LoseUI>();

        if (uiMenu is default(LoseUI))
            return;

        UIMenuManager.Instance.CloseMenu(uiMenu);
    }

    public override void FinishListeningEvents()
    {
    }

    public override void StartListeningEvents()
    {
    }

    public void RetryPressed()
    {
        GameManager.Instance.SceneManager.LoadCurScene();
    }

    public IPLDBase GetSessionCoinPLD()
    {
        //int curMoney = UserMoneyManager.Instance.UserMoneyAmount;

        //return new SessionCoinDrawerPLD(curMoney - _playerMoneyAtStart);

        return new SessionCoinDrawerPLD(0);
    }

    public IPLDBase GetPrevNextUnlockPLD()
    {
        //int levelID = ((LevelPhase)GameManager.Instance.PhaseFlowController.TreeRootNode).LevelID;
        //int prevLevelID = levelID - 1;

        //MagnetObjectUnlockInfo prevUnlockInfo = MagnetObjectUnlockManager.Instance.Settings.GetClosestUnlockInfo(prevLevelID);
        //MagnetObjectUnlockInfo curUnlockInfo = MagnetObjectUnlockManager.Instance.Settings.GetClosestUnlockInfo(levelID);

        //if (prevUnlockInfo is default(MagnetObjectUnlockInfo))
        //    prevUnlockInfo = curUnlockInfo;

        //if (curUnlockInfo == null)
        //    return new NextUnlockDrawerPLD(Color.white, 0, true);
        //else if (!curUnlockInfo.UnlockSettings.Color.Equals(prevUnlockInfo.UnlockSettings.Color))
        //    return new NextUnlockDrawerPLD(curUnlockInfo.UnlockSettings.PhysicalColor, 0, true);
        //else
        //    return new NextUnlockDrawerPLD(prevUnlockInfo.UnlockSettings.PhysicalColor, prevUnlockInfo.Percantage, true);

        return new NextUnlockDrawerPLD(Color.white, 0, true);
    }
}
