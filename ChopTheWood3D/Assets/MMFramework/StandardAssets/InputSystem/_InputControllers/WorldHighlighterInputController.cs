public class WorldHighlighterInputController : WorldInputControllerBase
{
    static WorldHighlighterInputController _instance;

    public static WorldHighlighterInputController Instance
    {
        get
        {
            if (_instance == null)
                _instance = new WorldHighlighterInputController();

            return _instance;
        }
    }

    protected override void DisposeCustomActions()
    {
        _instance = null;

        base.DisposeCustomActions();
    }
}
