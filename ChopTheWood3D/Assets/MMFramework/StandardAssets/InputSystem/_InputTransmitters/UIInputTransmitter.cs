public class UIInputTransmitter : InputTransmitter
{
    protected override InputControllerBase GetInputController()
    {
        return UIInputController.Instance;
    }

    protected override void RegisterToInputControllerEvents()
    {
    }

    protected override void UnregisterToInputControllerEvents()
    {
    }

    public void RaiseOnBeginDrag(Input_UI_OnBeginDrag e)
    {
        Raise(typeof(Input_UI_OnBeginDrag), e);
    }

    public void RaiseOnDrag(Input_UI_OnDrag e)
    {
        Raise(typeof(Input_UI_OnDrag), e);
    }

    public void RaiseOnEndDrag(Input_UI_OnEndDrag e)
    {
        Raise(typeof(Input_UI_OnEndDrag), e);
    }

    public void RaiseOnPointerDown(Input_UI_OnPointerDown e)
    {
        Raise(typeof(Input_UI_OnPointerDown), e);
    }

    public void RaiseOnPointerUp(Input_UI_OnPointerUp e)
    {
        Raise(typeof(Input_UI_OnPointerUp), e);
    }
}
