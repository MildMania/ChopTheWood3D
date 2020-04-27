using UnityEngine.EventSystems;

public class Input_UI_OnEndDrag : InputEvent
{
    public PointerEventData EventData;

    public Input_UI_OnEndDrag(PointerEventData eventData)
    {
        EventData = eventData;
    }
}
