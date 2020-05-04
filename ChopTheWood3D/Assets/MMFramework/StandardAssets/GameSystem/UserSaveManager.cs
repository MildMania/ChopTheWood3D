using UnityEngine;

public class UserSaveManager : MonoBehaviour
{
    [SerializeField] private bool _loadSave;
    [SerializeField] private int _startingLevelID;

    private const string USER_LEVELID_KEY = "LastCompletedLevelID";

    private void Awake()
    {
        //PhaseBaseNode.OnTraverseStarted_Static += OnPhaseTraverseStarted;
    }

    private void Start()
    {
        if (_loadSave)
            LoadSave();
        else
        {
            int managerSceneID = GameManager.Instance.SceneManager.ManagerSceneID;

            GameManager.Instance.SceneManager.LoadSceneWithIndexOf(managerSceneID + _startingLevelID);
        }
    }

    private void OnDestroy()
    {
        //PhaseBaseNode.OnTraverseStarted_Static -= OnPhaseTraverseStarted;
    }

    //private void OnPhaseTraverseStarted(PhaseBaseNode phase)
    //{
    //    if (phase is LevelWinPostPhase)
    //        SaveCurLevel();
    //}

    //private void SaveCurLevel()
    //{
    //    int curLevelID = ((LevelPhase)GameManager.Instance.PhaseFlowController.TreeRootNode).LevelID;

    //    PlayerPrefs.SetInt(USER_LEVELID_KEY, curLevelID);
    //    PlayerPrefs.Save();
    //}

    private void LoadSave()
    {
        bool hasLevelSave = PlayerPrefs.HasKey(USER_LEVELID_KEY);

        if (!hasLevelSave)
            GameManager.Instance.SceneManager.LoadNextScene();
        else
        {
            int lastCompletedLevelID = PlayerPrefs.GetInt(USER_LEVELID_KEY);
            int managerSceneID = GameManager.Instance.SceneManager.ManagerSceneID;

            int nextSceneID = lastCompletedLevelID + managerSceneID + 1;

            GameManager.Instance.SceneManager.LoadSceneWithIndexOf(nextSceneID);
        }
    }
}
