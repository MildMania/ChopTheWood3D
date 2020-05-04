using UnityEngine;

public class DefaultFlowManager : PhaseFlowManager
{
    protected override PhaseFlowController CreatePhase()
    {
        return new DefaultFlowController(GetCurLevelID());
    }

    private int GetCurLevelID()
    {
        int curSceneID = GameManager.Instance.SceneManager.CurSceneID;
        int managerSceneID = GameManager.Instance.SceneManager.ManagerSceneID;

        int levelID = curSceneID - managerSceneID;

        Debug.Log("Level ID: " + levelID);

        return levelID;
    }
}
