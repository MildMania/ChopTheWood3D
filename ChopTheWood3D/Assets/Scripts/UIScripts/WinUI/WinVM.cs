using UnityEngine;

public class WinVM : VMBase
{
    private int _playerMoneyAtStart;

    public WinVM()
    {
        PhaseBaseNode.OnTraverseStarted_Static += OnPhaseTraverseStarted;
    }

    protected override void DisposeCustomActions()
    {
        PhaseBaseNode.OnTraverseStarted_Static -= OnPhaseTraverseStarted;
    }

    private void OnPhaseTraverseStarted(PhaseBaseNode phaseBaseNode)
    {
        //if (phaseBaseNode is LevelWinPostPhase)
        //    ActivateUI();
        //else
        //    DeactivateUI();

        //if (phaseBaseNode is GamePhase)
        //    _playerMoneyAtStart = UserMoneyManager.Instance.UserMoneyAmount;
    }

    private void ActivateUI()
    {
        WinUI uiMenu = UIMenuManager.Instance.GetUIMenu<WinUI>();

        UIMenuManager.Instance.OpenUIMenu(uiMenu);
    }

    private void DeactivateUI()
    {
        WinUI uiMenu = UIMenuManager.Instance.GetOpenMenu<WinUI>();

        if (uiMenu is default(WinUI))
            return;

        UIMenuManager.Instance.CloseMenu(uiMenu);
    }

    public override void FinishListeningEvents()
    {
    }

    public override void StartListeningEvents()
    {
    }

    public void ContinuePressed()
    {
        GameManager.Instance.SceneManager.LoadNextScene();
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

    public IPLDBase GetNextUnlockPLD()
    {
        //int levelID = ((LevelPhase)GameManager.Instance.PhaseFlowController.TreeRootNode).LevelID;

        //MagnetObjectUnlockInfo info = MagnetObjectUnlockManager.Instance.Settings.GetClosestUnlockInfo(levelID);

        //if (info == null)
        //    return new NextUnlockDrawerPLD(Color.white, 0, true);
        //else
        //    return new NextUnlockDrawerPLD(info.UnlockSettings.PhysicalColor, info.Percantage, false);

        return new NextUnlockDrawerPLD(Color.white, 0, true);
    }
}
