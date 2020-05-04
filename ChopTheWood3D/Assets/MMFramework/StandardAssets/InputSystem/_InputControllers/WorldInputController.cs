public class WorldInputController : WorldInputControllerBase
{
    static WorldInputController _instance;

    public static WorldInputController Instance
    {
        get
        {
            if (_instance == null)
                _instance = new WorldInputController();

            return _instance;
        }
    }

    protected override void DisposeCustomActions()
    {
        _instance = null;

        base.DisposeCustomActions();
    }
}
