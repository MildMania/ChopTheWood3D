﻿using UnityEngine.EventSystems;

public class Input_UI_OnPointerDown : InputEvent
{
    public PointerEventData EventData;

    public Input_UI_OnPointerDown(PointerEventData eventData)
    {
        EventData = eventData;
    }
}
