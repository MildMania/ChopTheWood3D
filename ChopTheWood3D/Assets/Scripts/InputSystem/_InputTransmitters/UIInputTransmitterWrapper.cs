public class UIInputTransmitterWrapper : InputTransmitterWrapper<UIInputTransmitter>, IUIInputPointerDown, IUIInputPointerUp, IUIInputBeginDrag, IUIInputDrag, IUIInputEndDrag
{
    void IUIInputBeginDrag.OnBeginDrag(Input_UI_OnBeginDrag e)
    {
        foreach(var pair in ReceiverDict)
            pair.Value.RaiseOnBeginDrag(e);
    }

    void IUIInputDrag.OnDrag(Input_UI_OnDrag e)
    {
        foreach (var pair in ReceiverDict)
            pair.Value.RaiseOnDrag(e);
    }

    void IUIInputEndDrag.OnEndDrag(Input_UI_OnEndDrag e)
    {
        foreach (var pair in ReceiverDict)
            pair.Value.RaiseOnEndDrag(e);
    }

    void IUIInputPointerDown.OnPointerDown(Input_UI_OnPointerDown e)
    {
        foreach (var pair in ReceiverDict)
            pair.Value.RaiseOnPointerDown(e);
    }

    void IUIInputPointerUp.OnPointerUp(Input_UI_OnPointerUp e)
    {
        foreach (var pair in ReceiverDict)
            pair.Value.RaiseOnPointerUp(e);
    }
}