using UnityEngine;
using UnityEngine.SceneManagement;

public abstract class UIListener : MonoBehaviour
{
    public static bool CanEscape = true;

    protected virtual void Awake()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.sceneUnloaded += OnSceneUnloaded;
    }

    protected virtual void OnSceneLoaded(Scene loadedScene, LoadSceneMode loadSceneMode)
    {
    }

    protected virtual void OnSceneUnloaded(Scene loadedScene)
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        SceneManager.sceneUnloaded -= OnSceneUnloaded;
    }

    protected virtual void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && CanEscape)
            UIMenuManager.Instance.OnBackPressed();
    }
}
