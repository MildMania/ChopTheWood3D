using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

static partial class Utilities
{
    static YieldInstruction _waitForFixedUpdate;

    public static YieldInstruction WaitForFixedUpdate
    {
        get
        {
            if(_waitForFixedUpdate == null)
                _waitForFixedUpdate = new UnityEngine.WaitForFixedUpdate();

            return _waitForFixedUpdate;
        }
    }

    static YieldInstruction _waitForEndOfFrame;

    public static YieldInstruction WaitForEndOfFrame
    {
        get
        {
            if(_waitForEndOfFrame == null)
                _waitForEndOfFrame = new UnityEngine.WaitForEndOfFrame();

            return _waitForEndOfFrame;
        }
    }


    public static List<T> FindObjectsOfTypeAll<T>()
    {
        List<T> results = new List<T>();

        results.AddRange(FindDontDestroyObjectsOfTypeAll<T>());

        for (int i = 0; i < UnityEngine.SceneManagement.SceneManager.sceneCount; i++)
        {
            var s = UnityEngine.SceneManagement.SceneManager.GetSceneAt(i);
            if (s.isLoaded)
            {
                //Debug.Log("<color=magenta>Scene is loaded: " + s.name + "</color>");

                var allGameObjects = s.GetRootGameObjects();
                for (int j = 0; j < allGameObjects.Length; j++)
                {
                    var go = allGameObjects[j];
                    results.AddRange(go.GetComponentsInChildren<T>(true));
                }
            }
        }
        return results;
    }

    static List<T> FindDontDestroyObjectsOfTypeAll<T>()
    {
        List<T> results = new List<T>();

        GameObject dontDestroyRoot = GameObject.Find("DontDestroy");

        if (dontDestroyRoot == null)
            return results;

        results.AddRange(dontDestroyRoot.GetComponentsInChildren<T>(true));

        return results;
    }

    public static Coroutine WaitForSeconds(this MonoBehaviour mb, float duration, System.Action callback)
    {
        if (callback == null)
            return null;

        return mb.StartCoroutine(WaitForSeconds(duration, callback));
    }

    static IEnumerator WaitForSeconds(float duration, System.Action callback)
    {
        yield return new WaitForSeconds(duration);

        if (callback != null)
            callback();
    }

    public static void OnWaitedForFixedUpdate(this MonoBehaviour mb, System.Action callback)
    {
        if (callback == null)
            return;

        mb.StartCoroutine(WaitForFixedUpdateProgress(callback));
    }

    static IEnumerator WaitForFixedUpdateProgress(System.Action callback)
    {
        yield return WaitForFixedUpdate;

        if (callback != null)
            callback();
    }
}
