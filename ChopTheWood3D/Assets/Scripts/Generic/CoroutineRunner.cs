using UnityEngine;

public class CoroutineRunner : MonoBehaviour
{
    static CoroutineRunner _instance;

    public static CoroutineRunner Instance
    {
        get
        {
            if(_instance == null)
            {
                _instance = FindObjectOfType<CoroutineRunner>();

                if(_instance == null)
                {
                    _instance = new GameObject("CoroutineRunner")
                        .AddComponent<CoroutineRunner>();
                }
            }

            return _instance;
        }
    }
}
