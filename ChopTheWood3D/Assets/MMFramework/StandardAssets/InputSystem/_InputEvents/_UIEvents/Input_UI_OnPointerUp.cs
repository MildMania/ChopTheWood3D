using UnityEngine.EventSystems;

public class Input_UI_OnPointerUp : InputEvent
{
    public PointerEventData EventData;

    public Input_UI_OnPointerUp(PointerEventData eventData)
    {
        EventData = eventData;
    }
}
