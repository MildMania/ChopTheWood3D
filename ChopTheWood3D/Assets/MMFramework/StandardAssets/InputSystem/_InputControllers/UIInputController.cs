public class UIInputController : InputControllerBase
{
    static UIInputController _instance;

    public static UIInputController Instance
    {
        get
        {
            if (_instance == null)
                _instance = new UIInputController();

            return _instance;
        }
    }

    protected override void DisposeCustomActions()
    {
        _instance = null;

        base.DisposeCustomActions();
    }
}
