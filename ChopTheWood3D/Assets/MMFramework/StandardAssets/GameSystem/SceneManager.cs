using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManager : IDisposable
{
    #region Events
    public Action OnSceneUnloading { get; set; }
    public Action OnSceneUnloaded { get; set; }
    public Action OnSceneLoaded { get; set; }
    #endregion

    public int CurSceneID { get; private set; }
    public Scene CurLoadedScene { get; private set; }
    public bool PostLoadCalled { get; private set; }

    private int _firstGameSceneID;

    public int ManagerSceneID { get; private set; }

    public SceneManager(int managerSceneID)
    {
        Init(managerSceneID);

        UnityEngine.SceneManagement.SceneManager.sceneLoaded += OnSceneLoad;
        UnityEngine.SceneManagement.SceneManager.sceneUnloaded += OnSceneUnload;
    }

    private void Init(int managerSceneID)
    {
        ManagerSceneID = managerSceneID;

        _firstGameSceneID = ManagerSceneID + 1;
        CurSceneID = ManagerSceneID;
    }

    #region IDisposable Support
    private bool disposedValue = false;

    protected virtual void Dispose(bool disposing)
    {
        if (!disposedValue)
        {
            if (disposing)
            {
                UnityEngine.SceneManagement.SceneManager.sceneLoaded -= OnSceneLoad;
                UnityEngine.SceneManagement.SceneManager.sceneUnloaded -= OnSceneUnload;
            }

            disposedValue = true;
        }
    }

    public void Dispose()
    {
        Dispose(true);
    }
    #endregion

    public void LoadSceneWithIndexOf(int index)
    {
        if (index.Equals(UnityEngine.SceneManagement.SceneManager.sceneCountInBuildSettings))
            index = _firstGameSceneID;

        StartScene(index);
    }

    public void LoadNextScene()
    {
        int targetSceneIndex = CurSceneID + 1;

        if (targetSceneIndex.Equals(UnityEngine.SceneManagement.SceneManager.sceneCountInBuildSettings))
            targetSceneIndex = _firstGameSceneID;

        StartScene(targetSceneIndex);
    }

    public void LoadCurScene()
    {
        StartScene(CurSceneID);
    }

    private void StartScene(int sceneID)
    {
        PostLoadCalled = false;

        if (CurLoadedScene == null || CurLoadedScene.buildIndex <= ManagerSceneID)
            ContinueLoadScene(sceneID);
        else
            CheckForCurSceneUnload(() => ContinueLoadScene(sceneID));
    }

    private void ContinueLoadScene(int sceneID)
    {
        CurSceneID = sceneID;

        //SceneManager.LoadScene(GAME_SCENE_BASE_NAME, LoadSceneMode.Additive);
        UnityEngine.SceneManagement.SceneManager.LoadScene(CurSceneID, LoadSceneMode.Additive);
    }

    private void CheckForCurSceneUnload(Action callback)
    {
        if (CurLoadedScene == null)
            callback();

        OnLoadedScenePreUnload();

        AsyncOperation unloadOp = UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync(CurSceneID);

        void onSceneUnloadDel(AsyncOperation op)
        {
            op.completed -= onSceneUnloadDel;

            callback();

            //unloadOp = SceneManager.UnloadSceneAsync(GAME_SCENE_BASE_NAME);

            //void onBaseSceneUnloadDel(AsyncOperation op2)
            //{
            //    op2.completed -= onBaseSceneUnloadDel;

            //    callback();
            //}

            //if (unloadOp.isDone)
            //    onBaseSceneUnloadDel(unloadOp);
            //else
            //    unloadOp.completed += onBaseSceneUnloadDel;
        }

        if (unloadOp.isDone)
            onSceneUnloadDel(unloadOp);
        else
            unloadOp.completed += onSceneUnloadDel;
    }

    private void OnSceneUnload(Scene unloadedScene)
    {
        OnSceneUnloaded?.Invoke();
    }

    private void OnSceneLoad(Scene loadedScene, LoadSceneMode mode)
    {
        UnityEngine.SceneManagement.SceneManager.SetActiveScene(loadedScene);

        CurLoadedScene = loadedScene;

        OnScenePostLoad();
    }

    private void OnLoadedScenePreUnload()
    {
        OnSceneUnloading?.Invoke();
    }

    private void OnScenePostLoad()
    {
        OnSceneLoaded?.Invoke();

        PostLoadCalled = true;
    }
}
