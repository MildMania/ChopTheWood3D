using UnityEngine.EventSystems;

public class Input_UI_OnBeginDrag : InputEvent
{
    public PointerEventData EventData;

    public Input_UI_OnBeginDrag(PointerEventData eventData)
    {
        EventData = eventData;
    }
}
