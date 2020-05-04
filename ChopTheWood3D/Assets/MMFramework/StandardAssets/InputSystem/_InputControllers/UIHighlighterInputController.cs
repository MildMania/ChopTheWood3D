public class UIHighlighterInputController : InputControllerBase
{
    static UIHighlighterInputController _instance;

    public static UIHighlighterInputController Instance
    {
        get
        {
            if (_instance == null)
                _instance = new UIHighlighterInputController();

            return _instance;
        }
    }

    protected override void DisposeCustomActions()
    {
        _instance = null;

        base.DisposeCustomActions();
    }
}
