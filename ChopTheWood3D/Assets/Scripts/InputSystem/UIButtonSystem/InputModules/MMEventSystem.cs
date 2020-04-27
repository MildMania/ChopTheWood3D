using UnityEngine.EventSystems;

public class MMEventSystem : EventSystem
{
    static MMEventSystem _instance;

    public static MMEventSystem Instance
    {
        get
        {
            if (_instance == null)
                _instance = FindObjectOfType<MMEventSystem>();

            return _instance;
        }
    }

    protected override void Update()
    {
    }

    public void UpdateFrame()
    {
        base.Update();
    }
}
