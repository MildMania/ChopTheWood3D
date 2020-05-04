using UnityEngine.EventSystems;

public class Input_UI_OnDrag : InputEvent
{
    public PointerEventData EventData;

    public Input_UI_OnDrag(PointerEventData eventData)
    {
        EventData = eventData;
    }
}
